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

        //private Bullet b; This should be like a Type or something. It's supposed to differentiate what type of bullet gets fired.
        private bool canFire;
        private double timer;
        private double coolDown;

        private double facing;

        private double lowerBound;
        private double upperBound;

        public Gun(double coolDown, double lowerBound, double upperBound)
        {
            this.coolDown = coolDown;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            canFire = true;
            timer = 0;
        }

        public void fire(Vector2 position, double facing, Vector2 direction)
        {
            fire(position, facing, Math.Atan2(direction.Y, direction.X));
        }

        public void fire(Vector2 position, double facing, double firingAngle)
        {
            firingAngle -= facing;
            if (canFire && firingAngle > lowerBound && firingAngle < upperBound)
            {
                firingAngle += facing;
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
