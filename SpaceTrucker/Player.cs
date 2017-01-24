﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    class Player : Object
    {
        private float moveSpeed;
        private float thrust;
        private double turnSpeed;

        private int controlScheme;

        public Player(Mesh mesh, Vector2 position) : base(mesh, position)
        {
            //All of these numbers need tweaking.
            moveSpeed = 300f;
            thrust = 300f;
            turnSpeed = 10f;

            controlScheme = 0;

        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);
            
            // Code goes here
        }

        public void onCollision(Object o)
        {
            // What to do when you collide with Object o
        }

        public void processInput(GamePadState state, GamePadState prevState, GameTime gameTime)
        {
            //Gamepad controls
            //I'm gonna leave these in the shit state they are, because I don't have a gamepad to test with right now
            if (state.ThumbSticks.Left.Length() > .25)
            {
                Speed = new Vector2(moveSpeed * state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            }
            else
            {
                if (controlScheme == 2)
                {
                    Speed = Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// This is during the input step. Takes input, and produces the necessary changes
        /// </summary>
        /// <param name="state">
        /// The current state of the keyboard
        /// </param>
        public void processInput(KeyboardState state, KeyboardState prevState, GameTime gameTime)
        {
            if (state.IsKeyDown(Keys.Space) && !prevState.IsKeyDown(Keys.Space))
            {
                controlScheme = (controlScheme + 1) % 2;
            }
            switch (controlScheme)
            {
               case 0: //Turning, acceleration
                    if (state.IsKeyDown(Keys.A))
                    {
                        Facing -= turnSpeed * gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else if (state.IsKeyDown(Keys.D))
                    {
                        Facing += turnSpeed * gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (state.IsKeyDown(Keys.W))
                    {
                        Speed += (float)(thrust * gameTime.ElapsedGameTime.TotalSeconds) * new Vector2((float)Math.Cos(Facing), (float)Math.Sin(Facing));
                    }
                    else if (state.IsKeyDown(Keys.S))
                    {
                        Speed -= (float)(thrust * gameTime.ElapsedGameTime.TotalSeconds) * new Vector2((float)Math.Cos(Facing), (float)Math.Sin(Facing));
                    }
                    if (Speed.Length() > moveSpeed)
                    {
                        Speed = Vector2.Normalize(Speed) * moveSpeed;
                    }
                    break;
                case 1: //Move in direction pressed
                    Vector2 direction = Vector2.Zero;

                    if (state.IsKeyDown(Keys.W))
                    {
                        direction -= Vector2.UnitY;
                    }

                    if (state.IsKeyDown(Keys.S))
                    {
                        direction += Vector2.UnitY;
                    }

                    if (state.IsKeyDown(Keys.A))
                    {
                        direction -= Vector2.UnitX;
                    }

                    if (state.IsKeyDown(Keys.D))
                    {
                        direction += Vector2.UnitX;
                    }

                    if (!direction.Equals(Vector2.Zero))
                    { 
                        Facing = Math.Atan2(direction.Y, direction.X);
                        direction = Vector2.Normalize(direction);
                    }

                    Speed = direction * moveSpeed; // This doesn't get called if no keypress is registered
                    
                    break;
            }
        }
    }
}
