using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
        /// <summary>
        /// Holds a reference to the graphics device manager.
        /// </summary>
        protected GraphicsDeviceManager graphics;

        /// <summary>
        /// Holds a reference to the sprite batch used to draw.
        /// </summary>
        protected SpriteBatch spriteBatch;

        protected SpriteFont topBarFont;

        protected Texture2D uiTopTexture;

        protected uint currentScore;

        protected uint currentLevel;

        protected byte remainingLives;

        protected Maze currentMaze;

        protected Pacman pacman;

        protected Ghost[] ghosts;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferHeight = 560;
            Content.RootDirectory = "Content";

            // Initialize game vars
            this.currentScore = 0;
            this.currentLevel = 1;
            this.remainingLives = 3;
            this.currentMaze = Maze.Load(@"C:\Users\PtitBlond\Desktop\google-level.lvl");
            this.pacman = new Pacman();
            this.pacman.Speed = 0.1f;
            this.ghosts = new Ghost[4];
            this.ghosts[0] = new Blinky();
            this.ghosts[0].Position = Vector2.One * 25;
            this.ghosts[1] = new Clyde();
            this.ghosts[1].Position = Vector2.One * 50;
            this.ghosts[2] = new Inky();
            this.ghosts[2].Position = Vector2.One * 75;
            this.ghosts[3] = new Pinky();
            this.ghosts[3].Position = Vector2.One * 100;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content. Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            
            // TODO: Initialize Pacman's position
            // TODO: Initialize ghosts' position

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
            // TODO: load pacman content
            this.pacman.LoadContent(Content);
            // TODO: load ghosts content
            for(uint i = 0; i < 4; i++)
            {
                this.ghosts[i].LoadContent(Content);
            }

            // DEBUG
            this.currentMaze.LoadContent(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            // TODO: unload pacman content
            // TODO: unload ghosts content
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            // TODO: call Pacman.HandleKeyboard(keyboardState)
            // TODO: call Pacman.Update()
            this.pacman.HandleKeyboard(keyboardState);
            this.pacman.Update(gameTime);

            // TODO: call every Ghost.Update()
            for(uint i = 0; i < 4; i++)
            {
                //this.ghosts[i].Update(gameTime);
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

            // Draw the UI
            Vector2 scoreDimensions = this.topBarFont.MeasureString(this.currentScore.ToString());
            Vector2 levelDimensions = this.topBarFont.MeasureString(this.currentLevel.ToString());
            Vector2 livesDimensions = this.topBarFont.MeasureString(this.remainingLives.ToString());
            this.spriteBatch.Draw(this.uiTopTexture, Vector2.Zero, Color.White);
            this.spriteBatch.DrawString(this.topBarFont, this.currentScore.ToString(), new Vector2(30, 2 + (35 - scoreDimensions.Y) / 2), Color.Gold);
            this.spriteBatch.DrawString(this.topBarFont, this.currentLevel.ToString(), new Vector2(772 - levelDimensions.X, 2 + (35 - levelDimensions.Y) / 2), Color.Gold);
            this.spriteBatch.DrawString(this.topBarFont, this.remainingLives.ToString(), new Vector2(379 + (39 - livesDimensions.X) / 2, 25 + (39 - livesDimensions.Y) / 2), Color.Gold);

            // Draw the maze
            this.currentMaze.Draw(gameTime, this.spriteBatch);

            // Draw the ghosts
            for(uint i = 0; i < 4; i++)
            {
                this.ghosts[i].Draw(gameTime, this.spriteBatch);
            }

            // Draw Pacman
            this.pacman.Draw(gameTime, this.spriteBatch);

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
