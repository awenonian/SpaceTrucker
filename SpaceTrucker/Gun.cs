using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    class Gun : Managed
    {

        private Bullet b; 
        private bool canFire;
        private double timer;
        private double coolDown;

        private Vector2 offset;
        private double lowerBound;
        private double upperBound;

        private Texture2D firingArc;

        private Ship ship;

        public Gun(Vector2 offset, double coolDown, double lowerBound, double upperBound, Bullet b, Ship ship)
        {
            this.offset = offset;
            this.coolDown = coolDown;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            this.b = b;

            firingArc = generateArc(64, lowerBound, upperBound);

            this.ship = ship;

            canFire = true;
            timer = 0;
        }

        private Texture2D generateArc(int size, double lowerBound, double upperBound)
        {
            // This needs a GraphicsDevice, but I don't know how they work.
            Texture2D arcTex = new Texture2D(manager.gd, size, size);
            Color[] arc = new Color[size * size];
            for (int i = 0; i < arc.Length; i++)
            {
                int middle = size / 2;
                int dx = (i % size) - middle;
                int dy = (i / size) - middle;
                double distanceSquared = dx * dx + dy * dy;
                if (distanceSquared >= middle * middle)
                {
                    continue;
                }
                double angle = Math.Atan2(dy, dx);
                if (angle >= lowerBound && angle < upperBound)
                {
                    arc[i] = Color.Red;
                    arc[i].A = (byte) (255 * ((middle * middle) - distanceSquared) / (middle * middle));
                }
            }
            arcTex.SetData(arc);
            return arcTex;
        }

        public void fire(Vector2 position, Vector2 target)
        {
            // Rotates a vector by ship.Facing
            Vector2 rotatedOffset = new Vector2((offset.X * (float) Math.Cos(ship.Facing)) - (offset.Y * (float) Math.Sin(ship.Facing)), (offset.X * (float)Math.Sin(ship.Facing)) + (offset.Y * (float)Math.Cos(ship.Facing)));
            Vector2 origin = position + rotatedOffset;
            fire(origin, Math.Atan2(target.Y - origin.Y, target.X - origin.X));
        }

        public void fire(Vector2 position, double firingAngle)
        {
            firingAngle -= ship.Facing;
            // Should do corrections to keep it between -PI and PI
            firingAngle = firingAngle > Math.PI ? firingAngle - (2 * Math.PI) : firingAngle;
            firingAngle = firingAngle < -Math.PI ? firingAngle + (2 * Math.PI) : firingAngle;

            if (canFire && firingAngle > lowerBound && firingAngle < upperBound)
            {
                firingAngle += ship.Facing;
                manager.addBullet(b.copy(position, new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle))));
                canFire = false;
                timer = coolDown;
            }
        }
        
        public void update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timer < 0)
            {
                canFire = true;
            }
        }

        public void draw(SpriteBatch sb)
        {
            Vector2 rotatedOffset = new Vector2((offset.X * (float)Math.Cos(ship.Facing)) - (offset.Y * (float)Math.Sin(ship.Facing)), (offset.X * (float)Math.Sin(ship.Facing)) + (offset.Y * (float)Math.Cos(ship.Facing)));

            sb.Draw(firingArc, ship.Position + rotatedOffset, new Rectangle(0, 0, firingArc.Width, firingArc.Height), Color.White, (float)ship.Facing, new Vector2(firingArc.Width / 2, firingArc.Height / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
