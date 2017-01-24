using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    abstract class Object
    {
        public Mesh mesh { get; }
        private Vector2 position;

        protected static Manager manager = null;

        public Vector2 Position { get; protected set; }
        
        public Vector2 Speed { get; protected set; }
        public double Facing { get; protected set; }

        public Object(Mesh mesh, Vector2 position)
        {
            this.mesh = mesh;

            Position = position;
            Speed = Vector2.Zero;
        }

        /// <summary>
        /// Should be called when the state of the game updates. Independent from processInput.
        /// </summary>
        public virtual void update(GameTime gameTime, int width, int height)
        {
            move(gameTime, width, height);
        }

        private void move(GameTime gameTime, int width, int height)
        {
            Position += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Call during draw step. Draws the character.
        /// </summary>
        /// <param name="sb">
        /// The spritebatch for drawing
        /// </param>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(mesh.sprite, Position, new Rectangle(0, 0, mesh.Width, mesh.Height), Color.White, (float) Facing, new Vector2(mesh.Width / 2, mesh.Height / 2), 1f, SpriteEffects.None, 0);
        }

        public bool collision(Object other)
        {
            return mesh.collision(Position, other.Position, other.mesh);
        }

        public static void setManager(Manager m)
        {
            manager = m;
        }
    }
}
