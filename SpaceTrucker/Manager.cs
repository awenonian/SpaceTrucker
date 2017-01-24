using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    class Manager
    {
        //player sprites
        private Texture2D playerSprite;
        private Mesh playerMesh;

        public Player Player { get; private set; }

        /// <summary>
        /// Width of the game screen.
        /// </summary>
        private int width;
        /// <summary>
        /// Height of the game screen.
        /// </summary>
        private int height;

        private Random r;

        public Manager()
        {
            height = 0;
            width = 0;
            Player = null;
            r = new Random();
        }

        public void initialize(int width, int height)
        {
            this.width = width;
            this.height = height;

            Player = new Player(playerMesh, new Vector2(300, 100), this);
            Object.setManager(this);
        }

        public void loadContent(ContentManager content)
        {
            //<variable> = content.Load<Texture2D>(<filename w/o extension>);
            playerSprite = content.Load<Texture2D>("EnemyShip"); //Placeholder sprite, but eh.
            playerMesh = new Mesh(playerSprite, true); //Switch to false to get more accurate mesh
        }

        public void update(GameTime gameTime, KeyboardState kState, KeyboardState prevKState, GamePadState gState, GamePadState prevGState)
        {
            // If someone is operating keyboard
            if (kState.GetPressedKeys().Length != 0)
            {
                Player.processInput(kState, prevKState, gameTime);
            }
            // Otherwise, assume Game Pad controls
            else
            {
                Player.processInput(gState, prevGState, gameTime);
            }
            Player.update(gameTime, width, height);

        }

        public void draw(SpriteBatch sb)
        {
            //sb.Draw(background, destinationRectangle: new Rectangle(0, 0, width, height));
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            Player.draw(sb);
            sb.End();
        }
    }
}
