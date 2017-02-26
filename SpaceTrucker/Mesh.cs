using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    class Mesh
    {
        private float xScale;
        private float yScale;

        public Texture2D sprite { get; private set; }
        public List<Rectangle> mesh { get; private set; }

        public int Width { get { return (int)(xScale * sprite.Width); } }
        public int Height { get { return (int)(yScale * sprite.Height); } }

        public Mesh(Texture2D sprite, bool simpleCollisions)
        {
            xScale = 1;
            yScale = 1;
            this.sprite = sprite;
            Console.WriteLine(sprite.Width + ", " + sprite.Height);
            mesh = new List<Rectangle>();
            if (simpleCollisions)
            {
                // Model as a rectangle
                // This will need to be updated to act like 4 points, instead of a rectangle
                mesh.Add(sprite.Bounds);
            }
            else
            {
                mesh = calculateMesh(getFilledPixels(sprite), 5);
            }
        }

        private bool[,] getFilledPixels(Texture2D sprite)
        {
            Color[] spriteData = new Color[sprite.Width * sprite.Height];
            sprite.GetData(0, null, spriteData, 0, sprite.Width * sprite.Height);

            bool[,] filledPixels = new bool[sprite.Width, sprite.Height];

            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    if (spriteData[j * sprite.Width + i].A > 127)
                    {
                        filledPixels[i, j] = true;
                    }
                    else
                    {
                        filledPixels[i, j] = false;
                    }
                }
            }

            return filledPixels;
        }

        /// <summary>
        /// Provided a sprite, will calculate a series of boxes that contain the non transparent spaces on the sprite. 
        /// Small boxes will be removed, in interest of performance
        /// </summary>
        /// <param name="sprite">
        /// The sprite to calculate the mesh of.
        /// </param>
        /// <param name="levelOfDetail">
        /// The amount of new pixels a rectangle must include to be included. Lower numbers are higher detail, higher numbers are faster.
        /// </param>
        /// <returns>
        /// A List with the series of rectangles representing the sprite.
        /// </returns>
        private List<Rectangle> calculateMesh(bool[,] filledPixels, int levelOfDetail)
        {
            List<Rectangle> mesh = new List<Rectangle>();

            bool[,] counted = new bool[sprite.Width, sprite.Height];

            // Mark all pixels as uncounted
            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    counted[i, j] = false;
                }
            }


            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    // Skip already counted, or empty pixels.
                    if (counted[i, j] || !filledPixels[i, j])
                    {
                        counted[i, j] = true;
                        continue;
                    }
                    // Start a new rectangle, at the current position.
                    Rectangle rect = new Rectangle(i, j, 1, 1);
                    // These represent the column and row just outside the rectangle
                    int xBound = i + 2;
                    int yBound = j + 2;

                    // How many new pixels we've added to the mesh
                    int newPixels = 0;

                    // Whether or not we can grow the rectangle
                    bool canGrow = true;
                    while (canGrow)
                    {
                        // Assume we can't grow the rectangle
                        canGrow = false;

                        // Assume we can grow to the right
                        bool growRight = true;
                        // Along the height of the rectangle
                        for (int k = 0; k < rect.Height; k++)
                        {
                            // Check if the sprite boundary allows the rectangle to grow
                            if (j + k >= sprite.Height || xBound >= sprite.Width)
                            {
                                growRight = false;
                                break;
                            }
                            // If we find an empty pixel, we can't grow
                            if (!filledPixels[xBound, j + k])
                            {
                                growRight = false;
                                break;
                            }
                        }
                        // If growth would be successful, then grow, and label that we can grow again.
                        if (growRight)
                        {
                            // Update bounds
                            canGrow = true;
                            xBound++;
                            rect.Width++;

                        }
                        // Similar process, just along the width of the rectangle (at the bottom)
                        bool growDown = true;
                        for (int k = 0; k < rect.Width; k++)
                        {
                            if (i + k >= sprite.Width || yBound >= sprite.Height)
                            {
                                growDown = false;
                                break;
                            }
                            if (!filledPixels[i + k, yBound])
                            {
                                growDown = false;
                                break;
                            }
                        }
                        if (growDown)
                        {
                            // Update bounds
                            canGrow = true;
                            yBound++;
                            rect.Height++;

                        }
                    }

                    // Count the new pixels
                    for (int k = rect.X; k < rect.X + rect.Width; k++)
                    {
                        for (int l = rect.Y; l < rect.Y + rect.Height; l++)
                        {
                            if (!counted[k, l])
                            {
                                counted[k, l] = true;
                                newPixels++;
                            }

                        }
                    }
                    // If the rectangle matters
                    if (newPixels > levelOfDetail)
                    {
                        mesh.Add(rect);
                    }
                }
            }
            return mesh;
        }

        private List<Vector2> calculateMesh(bool[,] outline)
        {
            // Assume true for the outline, and false for every other point. 
            // There may be true points that are not connected to the hull, you can ignore these

            List<Vector2> points = new List<Vector2>();

            for (int i = 0; i < outline.GetLength(0); i++)
            {
                for (int j = 0; j < outline.GetLength(1); j++)
                {
                    if (outline[i,j])
                    {
                        // Found start point
                        // Find the slope, but start your search going backwards, since this is the top left most filled point
                        int[] slope = findSlope(outline, i, j, 2);
                        Vector2 end = findEnd(outline, i, j, slope[0], slope[1]);
                        points.Add(findEnd(outline, (int)end.X, (int)end.Y, -slope[0], -slope[1]));
                        points.Add(end);
                        break;
                    }
                }
            }

            findHull(outline, points);

            // Find a starting point, for this point, and only this point, trace the line in both directions, to assure you have it's end points
            // Afterwards, load points into a list, with one end point being before the other. Starting from the last end point on the list, find the next vertex
            // Through line analysis, and add it to the end of the list. Repeat until you come across the end point at the start of the list, somewhere along your line.
                // Line analysis is the following steps:
                    // In each cardinal direction, check the pixel there, and to it's right and left, continue in that direction until you find a diagonal. From that diagonal
                    // perform the same procedure, assuming a minimum of the first straight line length. Once confirmed on the straight line length, continue on that trajectory
                    // until a deviation is found. The point right before the deviation is the end point. perform the same check from there, until you complete the cycle.
            // Return this complete list.
            return points;
        }

        /// <summary>
        /// Finds the slope of the line with the given point in it (generally at its end)
        /// </summary>
        /// <param name="outline">
        /// The image to search through, passed as a boolean array containing it's outline
        /// </param>
        /// <param name="x">
        /// The x of the point to start form
        /// </param>
        /// <param name="y">
        /// The y of the point to start from
        /// </param>
        /// <param name="start">
        /// Optional parameter for which cardinal direction to start your search from 
        /// (0 for right/east, +1 for every 90 degrees counterclockwise)
        /// </param>
        /// <returns>
        /// The dx and dy of the line, in an int array, with x as [0] and y as [1]
        /// </returns>
        private int[] findSlope( bool[,] outline, int x, int y, int start = 0)
        {
            // This code needs a lot of bounds checking. I'm not going to put it in while I code the basic outline though.
            int[] slope = new int[2];
            for (int i = 0; i < 4; i++)
            {
                int length = 0;
                switch ((start + i) % 4)
                {
                    case 0:
                        // Go forward for as long as you can
                        while (outline[x+1, y])
                        {
                            length++;
                            x++;
                        }
                        // Check if we deflect up or down
                        if (outline[x+1, y+1])
                        {
                            // We need to check if the line actually continues like this, or if it's going off in a different direaction.
                            if (outline[x + length, y+1])
                            {
                                x += length;
                                y++;
                                // The initial length being to short isn't a problem, because we may have started part way through a segment,
                                // whereas if the initial length is too long (what we checked above), then we're looking at two different segments:
                                // **          ****
                                //   ***    vs     *
                                //      ***         *

                                // ? Should we actually do it that way ? 
                                // Or should any deviation be treated as a separate segment? 
                                // Or, perhaps neither should be, and that should be left up to the slope finding algorithm
                                    // ^ go with that one, number 3.
                                while (outline[x + 1, y])
                                {
                                    length++;
                                    x++;
                                }

                                slope[1] = 1;
                                slope[0] = length;
                                return slope;
                            } else
                            {
                                // The rest isn't a continuation of this line, as the slope has changed. 
                                // So this should be just a straight line

                                slope[0] = 1;
                                slope[1] = 0;
                                return slope;

                            }



                        }
                        else if (outline[x+1, y-1])
                        {
                            // Our line is sloping down as it goes left
                            slope[1] = -1;
                            // We need to confirm how long the slope is, because we may have started part way through a segment
                            if (outline[x + length, y - 1])
                            {
                                // Update our checking coordinates
                                x += length;
                                y--;
                                // Figure out how long the slope actually is
                                while (outline[x + 1, y])
                                {
                                    length++;
                                    x++;
                                }
                            }

                            slope[0] = length;
                            return slope;

                        }
                        // Turns out, this was just a straight line. Gotta return a 1 so we can trace it back
                        else
                        {
                            slope[0] = 1;
                            slope[1] = 0;
                            return slope;
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
            }
            return null;
        }

        private Vector2 findEnd(bool[,] outline, int x, int y, int dx, int dy)
        {
            // Follow slope along, checking that pattern is followed.
            // Do NOT assume that you are starting at any particular place in the segment.
                // That is, if you get a slope of 5 by 1, don't assume you have to go all 5 your first go, or your last go
            // Assume thin line (i.e. pixels are connected by corners, not faces)
                // This means that each time you cross a diagonal, at least one of the non-linear diagonals should be empty 
                // (not part of the outline, and not part of the interior)
            return Vector2.Zero;
        }

        private void findHull(bool[,] outline, List<Vector2> points)
        {
            // From last point on the list, find the next point until you come across the first point in the list
        }

        public bool collision(Vector2 pos, Vector2 otherPos, Mesh other)
        {
            // This should take care of most objects.
            if (otherPos.X > pos.X + Width)
            {
                return false;
            }
            if (otherPos.Y > pos.Y + Height)
            {
                return false;
            }
            if (otherPos.X + other.Width < pos.X)
            {
                return false;
            }
            if (otherPos.Y + other.Height < pos.Y)
            {
                return false;
            }
            // Here is more intenstive checking
            for (int i = 0; i < mesh.Count; i++)
            {
                for (int j = 0; j < other.mesh.Count; j++)
                {
                    //This is the same check as above, just rewritten, and using mesh's rectangles' coordinates
                    if (!((otherPos.X + other.mesh[j].X > pos.X + mesh[i].X + mesh[i].Width)
                        && (otherPos.Y + other.mesh[j].Y > pos.Y + mesh[i].Y + mesh[i].Height)
                        && (otherPos.X + other.mesh[j].X + other.mesh[j].Width < pos.X + mesh[i].X)
                        && (otherPos.Y + other.mesh[j].Y + other.mesh[j].Height < pos.Y + mesh[j].Y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool simpleCollision(Vector2 pos, Vector2 otherPos, Mesh other)
        {
            if (otherPos.X > pos.X + Width)
            {
                return false;
            }
            if (otherPos.Y > pos.Y + Height)
            {
                return false;
            }
            if (otherPos.X + other.Width < pos.X)
            {
                return false;
            }
            if (otherPos.Y + other.Height < pos.Y)
            {
                return false;
            }
            return true;
        }

        public void scale(float xScale, float yScale)
        {
            this.xScale = xScale;
            this.yScale = yScale;

            for (int i = 0; i < mesh.Count; i++)
            {
                mesh[i] = new Rectangle((int)(mesh[i].X * xScale), (int)(mesh[i].Y * yScale), (int)(mesh[i].Width * xScale), (int)(mesh[i].Height * yScale));
            }
        }

    }
}
