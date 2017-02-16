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

        protected Gun[] guns;

        public Ship(Mesh mesh, Vector2 position, float moveSpeed, float thrust, float turnSpeed) : base(mesh, position)
        {
            this.moveSpeed = moveSpeed;
            this.thrust = thrust;
            this.turnSpeed = turnSpeed;
            guns = new Gun[2];
            guns[0] = new Gun(.01f, 0, Math.PI);
            guns[1] = new Gun(.01f, -17 * Math.PI / 16, -15 * Math.PI / 16);
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);
            foreach (Gun g in guns)
            {
                g.update(gameTime);
            }
        }

        public void onCollision(Object o)
        {
            // What to do when you collide with Object o
        }

        public void fire(double firingAngle)
        {
            foreach (Gun g in guns)
            {
                g.fire(Position, Facing, firingAngle);
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
