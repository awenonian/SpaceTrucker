using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    class Player : Ship
    {
        public Player(Mesh mesh, Vector2 position) : base(mesh, position, 300f, 300f, 10f)
        {
            // Done
        }

        public void processInput(GamePadState state, GamePadState prevState, GameTime gameTime)
        {
            //Gamepad controls
            //I'm gonna leave these in the shit state they are, because I don't have a gamepad to test with right now
            if (state.ThumbSticks.Left.Length() > .25)
            {
                move(state.ThumbSticks.Left, gameTime);
            }
        }

        /// <summary>
        /// This is during the input step. Takes input, and produces the necessary changes
        /// </summary>
        /// <param name="state">
        /// The current state of the keyboard
        /// </param>
        public void processInput(KeyboardState kState, KeyboardState prevKState, MouseState mState, MouseState prevMState, GameTime gameTime)
        {

            if (mState.LeftButton == ButtonState.Pressed /* && prevMState.LeftButton == ButtonState.Released */) // <-- allows autofire, uncommenting will change to semi-auto
            {
                // When facing down left, firing off-side (up left) doesn't work.
                fire(mState.Position.ToVector2());
            }

            if (kState.IsKeyDown(Keys.A))
            {
                turn(-1, gameTime);
            }
            else if (kState.IsKeyDown(Keys.D))
            {
                turn(1, gameTime);
            }
            if (kState.IsKeyDown(Keys.W))
            {
                accelerate(1, gameTime);
            }
            else if (kState.IsKeyDown(Keys.S))
            {
                accelerate(-1, gameTime);
            }
            
        }
    }
}
