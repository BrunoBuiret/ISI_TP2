using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyPacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;

        protected SpriteBatch spriteBatch;

        protected SpriteFont topBarFont;

        protected Texture2D uiTopTexture;

        public const float BLOCK_WIDTH = 20f;

        public const float BLOCK_HEIGHT = 20f;

        protected bool isPaused;

        protected uint currentScore;

        protected uint currentLevel;

        protected byte remainingLives;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Initialize game vars
            this.isPaused = true;
            this.currentScore = 0;
            this.currentLevel = 1;
            this.remainingLives = 3;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.topBarFont = this.Content.Load<SpriteFont>(@"fonts\topBar");
            this.uiTopTexture = this.Content.Load<Texture2D>(@"images\ui-top");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if(!this.isPaused)
            {
                // Call Ghost.Update()

                // Call Pacman.Update()
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            // Calculate the text dimensions
            Vector2 scoreDimensions = this.topBarFont.MeasureString(this.currentScore.ToString());
            Vector2 levelDimensions = this.topBarFont.MeasureString(this.currentLevel.ToString());
            Vector2 livesDimensions = this.topBarFont.MeasureString(this.remainingLives.ToString());

            // Draw the UI
            this.spriteBatch.Draw(this.uiTopTexture, Vector2.Zero, Color.White);
            this.spriteBatch.DrawString(this.topBarFont, this.currentScore.ToString(), new Vector2(30, 2 + (35 - scoreDimensions.Y) / 2), Color.Gold);
            this.spriteBatch.DrawString(this.topBarFont, this.currentLevel.ToString(), new Vector2(772 - levelDimensions.X, 2 + (35 - levelDimensions.Y) / 2), Color.Gold);
            this.spriteBatch.DrawString(this.topBarFont, this.remainingLives.ToString(), new Vector2(377 + (39 - livesDimensions.X) / 2, 23 + (39 - livesDimensions.Y) / 2), Color.Gold);

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
