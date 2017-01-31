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

        public Gun(double coolDown)
        {
            this.coolDown = coolDown;
            canFire = true;
            timer = 0;
        }

        public void fire(Vector2 position, Vector2 direction)
        {
            if (canFire)
            {
                manager.addBullet(new Bullet(position, direction));
                canFire = false;
                timer = coolDown;
            }
        }

        public void fire(Vector2 position, double firingAngle)
        {
           
            if (canFire)
            {
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
