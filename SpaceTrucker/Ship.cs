using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    class Ship : Object
    {
        protected float moveSpeed;
        protected float thrust;
        protected double turnSpeed;

        protected Gun g;

        public Ship(Mesh mesh, Vector2 position, float moveSpeed, float thrust, float turnSpeed) : base(mesh, position)
        {
            this.moveSpeed = moveSpeed;
            this.thrust = thrust;
            this.turnSpeed = turnSpeed;
            g = new Gun(.01f);
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);

            g.update(gameTime);
        }

        public void onCollision(Object o)
        {
            // What to do when you collide with Object o
        }

        public void fire(double firingAngle)
        {
            // Find angle of fire, corrected for facing direction
            firingAngle -= Facing;
            firingAngle = firingAngle % (2 * Math.PI);

            // If the angle is in the allowable area (full 180 on the left, Pi/12 radian arc to the right)
            // More accurately, if it's not in the disallowable area. 
            if (!((firingAngle < -Math.PI) || (firingAngle < Math.PI && firingAngle > (13 * Math.PI / 24)) || (firingAngle < (11 * Math.PI / 24) && firingAngle > 0)))
            { 
                // Correct back
                firingAngle += Facing;
                g.fire(Position, new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle)));
            }
        }

        public void fire(Vector2 position)
        {
            fire(Math.Atan2(position.Y - Position.Y, position.X - Position.X));
        }

        public void move(Vector2 direction, GameTime gameTime)
        {
            Position += Vector2.Normalize(direction) * moveSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void turn(int direction, GameTime gameTime)
        {
            Facing += turnSpeed * Math.Sign(direction) * gameTime.ElapsedGameTime.TotalSeconds;
            Facing = Facing % (2 * Math.PI);
        }

        public void accelerate(int direction, GameTime gameTime)
        {
            Speed += (thrust * Math.Sign(direction) * (float) gameTime.ElapsedGameTime.TotalSeconds) * new Vector2((float)Math.Cos(Facing), (float)Math.Sin(Facing));

            if (Speed.Length() > moveSpeed)
            {
                Speed = Vector2.Normalize(Speed) * moveSpeed;
            }
        }
    }
}
