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

        private Texture2D bulletSprite;
        private Mesh bulletMesh;

        public GraphicsDevice gd { get; private set; }

        public Player Player { get; private set; }

        public List<Bullet> bullets;

        /// <summary>
        /// Width of the game screen.
        /// </summary>
        private int width;
        /// <summary>
        /// Height of the game screen.
        /// </summary>
        private int height;

        private bool isGamepad;

        private Random r;

        public Manager()
        {
            height = 0;
            width = 0;
            Player = null;
            r = new Random();
        }

        public void initialize(int width, int height, GraphicsDevice gd)
        {
            this.width = width;
            this.height = height;
            this.gd = gd;
            isGamepad = false;
            Managed.setManager(this);

            Player = new Player(playerMesh, new Vector2(300, 100));
            bullets = new List<Bullet>();
        }

        public void loadContent(ContentManager content)
        {
            //<variable> = content.Load<Texture2D>(<filename w/o extension>);

            playerSprite = content.Load<Texture2D>("EnemyShip"); //Placeholder sprite, but eh.
            playerMesh = new Mesh(playerSprite, true); //Switch to false to get more accurate mesh

            bulletSprite = content.Load<Texture2D>("Bullet"); //Placeholder sprite, but eh.
            bulletMesh = new Mesh(bulletSprite, true); //Switch to false to get more accurate mesh

            new Bullet(bulletMesh, 1000f); //This seems like bad practice. Can someone clean this up?
            // For clarity, this is initializing the static values for bullets: Their mesh, and speed.
            // It seems like there may be a better way to do this
        }

        public void update(GameTime gameTime, KeyboardState kState, KeyboardState prevKState, MouseState mState, MouseState prevMState, GamePadState gState, GamePadState prevGState)
        {
            if (isGamepad)
            {
                // If someone is operating keyboard
                if (kState.GetPressedKeys().Length != 0)
                {
                    Player.processInput(kState, prevKState, mState, prevMState, gameTime);
                    isGamepad = false;
                }
                // Otherwise, assume Game Pad controls
                else
                {
                    Player.processInput(gState, prevGState, gameTime);
                }
            }
            else
            {
                // If the gamepad is being used
                if (!gState.Equals(GamePadState.Default))
                {
                    Player.processInput(gState, prevGState, gameTime);
                    isGamepad = true;
                }
                // Otherwise, use Keyboard and Mouse
                else
                {
                    Player.processInput(kState, prevKState, mState, prevMState, gameTime);
                }
            }
            Player.update(gameTime, width, height);

            for (int i = bullets.Count-1; i >= 0; i--)
            {
                bullets[i].update(gameTime, width, height);
                if (bullets[i].Position.X < 0 || bullets[i].Position.X > width || bullets[i].Position.Y < 0 || bullets[i].Position.Y > height)
                {
                    bullets.Remove(bullets[i]);
                }
            }

        }

        public void draw(SpriteBatch sb)
        {
            //sb.Draw(background, destinationRectangle: new Rectangle(0, 0, width, height));
            // Some advice said to not use NonPremultiplied, but it's the one that produces the correct result. Perhaps we should look into this.
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, null, null);
            Player.draw(sb);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            foreach (Bullet b in bullets)
            {
                b.draw(sb);
            }
            sb.End();
        }

        public void addBullet(Bullet b)
        {
            bullets.Add(b);
        }
    }
}
