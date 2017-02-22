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

        //private Bullet b; This will be a subclass of bullet. 
        //Each subclass should override a "copy" method that returns a new copy of it's bullet type, given a direction and position
        private bool canFire;
        private double timer;
        private double coolDown;

        private Vector2 offset;
        private double lowerBound;
        private double upperBound;

        private Texture2D firingArc;

        private Ship ship;

        public Gun(Vector2 offset, double coolDown, double lowerBound, double upperBound, Ship ship)
        {
            this.offset = offset;
            this.coolDown = coolDown;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            firingArc = generateArc(32, lowerBound, upperBound);

            this.ship = ship;

            canFire = true;
            timer = 0;
        }

        private Texture2D generateArc(int size, double lowerBound, double upperBound)
        {
            return null;
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
                manager.addBullet(new Bullet(position, new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle))));
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
    }
}
