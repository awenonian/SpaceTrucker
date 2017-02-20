using Microsoft.Xna.Framework;
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

        private double lowerBound;
        private double upperBound;

        private Ship ship;

        public Gun(double coolDown, double lowerBound, double upperBound, Ship ship)
        {
            this.coolDown = coolDown;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            this.ship = ship;

            canFire = true;
            timer = 0;
        }

        public void fire(Vector2 position, Vector2 direction)
        {
            fire(position, Math.Atan2(direction.Y, direction.X));
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
