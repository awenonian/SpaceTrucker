using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceTrucker
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState prevKState;
        GamePadState prevGState;
        MouseState prevMState;

        Manager manager;

        string gameTitle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            manager = new Manager();
            gameTitle = "Game";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
            manager.initialize(
                width: graphics.GraphicsDevice.Viewport.Width,
                height: graphics.GraphicsDevice.Viewport.Height);
            IsMouseVisible = true;

            prevKState = Keyboard.GetState();
            prevGState = GamePad.GetState(PlayerIndex.One);
            prevMState = Mouse.GetState();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            manager.loadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                KeyboardState kState = Keyboard.GetState();
                GamePadState gState = GamePad.GetState(PlayerIndex.One);
                MouseState mState = Mouse.GetState();
                //MouseState mState = Mouse.GetState();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                    Exit();

                manager.update(gameTime, kState, prevKState, mState, prevMState, gState, prevGState);
                prevKState = kState;
                prevGState = gState;
                prevMState = mState;

                base.Update(gameTime);
            }
        }

        ///<summary>
        ///Perform actions when the application gains focus
        /// </summary>
        protected override void OnActivated(object sender, System.
                                            EventArgs args)
        {
            Window.Title = gameTitle;
            base.OnActivated(sender, args);
        }

        ///<summary>
        ///Perform actions when the application loses focus
        ///</summary>
        protected override void OnDeactivated(object sender, System.
                                              EventArgs args)
        {
            Window.Title = gameTitle + " - Paused";
            base.OnActivated(sender, args);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            manager.draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
