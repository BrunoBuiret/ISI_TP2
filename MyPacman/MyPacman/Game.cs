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
        /// Describes every state of the game.
        /// </summary>
        public enum GameState : byte
        {
            /// <summary>
            /// User is currently in the main menu.
            /// </summary>
            MENU,
            /// <summary>
            /// User is currently reading help.
            /// </summary>
            HELP,
            /// <summary>
            /// User is currently playing.
            /// </summary>
            PLAYING,
            /// <summary>
            /// User paused the game.
            /// </summary>
            PAUSED,
            /// <summary>
            /// User is looking at his score.
            /// </summary>
            SCORE
        };

        public enum GhostsIndex : byte
        {
            BLINKY = 0,
            CLYDE = 1,
            INKY = 2,
            PINKY = 3
        }

        public enum MovementDirection : byte
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        };

        /// <summary>
        /// Holds a reference to the graphics device manager.
        /// </summary>
        protected GraphicsDeviceManager graphics;

        /// <summary>
        /// Holds a reference to the sprite batch used to draw.
        /// </summary>
        protected SpriteBatch spriteBatch;

        //==================================================

        protected SpriteFont uiFont;

        protected Texture2D uiMenuSoundsOnTexture;

        protected Texture2D uiMenuSoundsOffTexture;

        protected Texture2D uiHelpTexture;

        protected Texture2D uiPauseTexture;

        protected Texture2D uiTopTexture;

        protected Texture2D uiLifeTexture;

        protected Texture2D uiScoreTexture;

        protected Vector2[] livesPositions;

        protected SoundEffect onPacmanDead;

        protected SoundEffect onPacmanWin;

        protected SoundEffect onEnergizerEaten;

        //==================================================

        protected GameState currentState;

        protected Boolean soundsActivated;

        protected List<Song> songsList;

        protected KeyboardState previousKeyboardState;

        protected List<Maze> mazesList;

        protected Boolean areGhostsVulnerable;

        protected TimeSpan vulnerableGhostsBeginning;

        //==================================================

        protected Byte remainingLives;

        protected UInt32 currentScore;

        protected Byte currentLevel;

        protected Pacman pacman;

        protected Ghost[] ghosts;

        /// <summary>
        /// Creates a new game.
        /// </summary>
        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferHeight = 600;
            this.Content.RootDirectory = "Content";

            MeasureUtility.totalWidth = (uint) this.graphics.PreferredBackBufferWidth;
            MeasureUtility.totalHeight = (uint) this.graphics.PreferredBackBufferHeight;
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
            this.currentState = GameState.MENU;
            this.soundsActivated = false;
            this.songsList = new List<Song>();
            this.previousKeyboardState = Keyboard.GetState();
            this.mazesList = new List<Maze>();
            foreach(String fileName in Directory.GetFiles(this.Content.RootDirectory + @"\data\", "*.lvl"))
            {
                this.mazesList.Add(Maze.Load(fileName));
            }
            this.areGhostsVulnerable = false;
            this.vulnerableGhostsBeginning = TimeSpan.Zero;
            this.livesPositions = new Vector2[8];
            this.livesPositions[0] = new Vector2(392, 4);
            this.livesPositions[1] = new Vector2(364, 14);
            this.livesPositions[2] = new Vector2(418, 13);
            this.livesPositions[3] = new Vector2(355, 41);
            this.livesPositions[4] = new Vector2(428, 40);
            this.livesPositions[5] = new Vector2(363, 63);
            this.livesPositions[6] = new Vector2(418, 63);
            this.livesPositions[7] = new Vector2(392, 73);
            this.pacman = new Pacman();
            this.pacman.Initialize();
            this.ghosts = new Ghost[4];
            this.ghosts[(int) GhostsIndex.BLINKY] = new Blinky();
            this.ghosts[(int) GhostsIndex.CLYDE] = new Clyde();
            this.ghosts[(int) GhostsIndex.INKY] = new Inky();
            this.ghosts[(int) GhostsIndex.PINKY] = new Pinky();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.uiFont = this.Content.Load<SpriteFont>(@"fonts\ui-font");

            this.uiMenuSoundsOnTexture = this.Content.Load<Texture2D>(@"images\ui-menu-sounds-on");
            this.uiMenuSoundsOffTexture = this.Content.Load<Texture2D>(@"images\ui-menu-sounds-off");
            this.uiHelpTexture = this.Content.Load<Texture2D>(@"images\ui-help");
            this.uiPauseTexture = this.Content.Load<Texture2D>(@"images\ui-pause");
            this.uiTopTexture = this.Content.Load<Texture2D>(@"images\ui-top");
            this.uiLifeTexture = this.Content.Load<Texture2D>(@"images\ui-life");
            this.uiScoreTexture = this.Content.Load<Texture2D>(@"images\ui-score");

            this.songsList.Add(this.Content.Load<Song>(@"sounds\human-1"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\human-2"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\human-3"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\orc-1"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\orc-2"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\orc-3"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\undead-1"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\undead-2"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\undead-3"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\night-elf-1"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\night-elf-2"));
            this.songsList.Add(this.Content.Load<Song>(@"sounds\night-elf-3"));

            this.onPacmanDead = this.Content.Load<SoundEffect>(@"sounds\on-pacman-dead");
            this.onPacmanWin = this.Content.Load<SoundEffect>(@"sounds\on-pacman-win");
            this.onEnergizerEaten = this.Content.Load<SoundEffect>(@"sounds\on-energizer-eaten");

            foreach(Maze maze in this.mazesList)
            {
                maze.LoadContent(this.Content);
            }

            this.pacman.LoadContent(this.Content);
            
            foreach (byte index in Enum.GetValues(typeof(GhostsIndex)))
            {
                this.ghosts[index].LoadContent(this.Content);
            }
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
        /// <see cref="https://msdn.microsoft.com/en-us/library/bb203906.aspx"/>
        /// <see cref="https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.boundingsphere.intersects.aspx"/>
        /// <see cref="https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.boundingbox.intersects.aspx"/>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();

            switch(this.currentState)
            {
                case GameState.PLAYING:
                    if(keyboardState.IsKeyDown(Keys.Escape))
                    {
                        // User pressed Escape
                        // Pause the game and open the menu
                        this.currentState = GameState.PAUSED;
                    }
                    else
                    {
                        // Update the game
                        this.pacman.HandleKeyboard(keyboardState);

                        // Handle collisions
                        if (this.pacman.Direction.X != 0)
                        {
                            // Pacman is moving horizontally
                            if (this.pacman.Direction.X > 0)
                            {
                                // Pacman is moving to the right
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.RIGHT,
                                    (uint)MeasureUtility.ActualXToBlockX(this.pacman.Position.X + MeasureUtility.BLOCK_WIDTH),
                                    (uint)MeasureUtility.ActualYToBlockY(this.pacman.Position.Y)
                                );
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.RIGHT,
                                    (uint)MeasureUtility.ActualXToBlockX(this.pacman.Position.X + MeasureUtility.BLOCK_WIDTH),
                                    (uint)MeasureUtility.ActualYToBlockY(this.pacman.Position.Y + MeasureUtility.BLOCK_HEIGHT - 1)
                                );
                            }
                            else
                            {
                                // Pacman is moving to the left
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.LEFT,
                                    (uint) MeasureUtility.ActualXToBlockX(this.pacman.Position.X - 1),
                                    (uint) MeasureUtility.ActualYToBlockY(this.pacman.Position.Y)
                                );
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.RIGHT,
                                    (uint)MeasureUtility.ActualXToBlockX(this.pacman.Position.X - 1),
                                    (uint)MeasureUtility.ActualYToBlockY(this.pacman.Position.Y + MeasureUtility.BLOCK_HEIGHT - 1)
                                );
                            }
                        }
                        else if (this.pacman.Direction.Y != 0)
                        {
                            // Pacman is moving vertically
                            if (this.pacman.Direction.Y > 0)
                            {
                                // Pacman is going down
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.DOWN,
                                    (uint) MeasureUtility.ActualXToBlockX(this.pacman.Position.X),
                                    (uint) MeasureUtility.ActualYToBlockY(this.pacman.Position.Y + MeasureUtility.BLOCK_HEIGHT)
                                );
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.DOWN,
                                    (uint)MeasureUtility.ActualXToBlockX(this.pacman.Position.X + MeasureUtility.BLOCK_WIDTH - 1),
                                    (uint)MeasureUtility.ActualYToBlockY(this.pacman.Position.Y + MeasureUtility.BLOCK_HEIGHT)
                                );
                            }
                            else
                            {
                                // Pacman is going up
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.UP,
                                    (uint)MeasureUtility.ActualXToBlockX(this.pacman.Position.X),
                                    (uint)MeasureUtility.ActualYToBlockY(this.pacman.Position.Y - 1)
                                );
                                this.HandleCollision(
                                    gameTime,
                                    MovementDirection.UP,
                                    (uint)MeasureUtility.ActualXToBlockX(this.pacman.Position.X + MeasureUtility.BLOCK_WIDTH - 1),
                                    (uint)MeasureUtility.ActualYToBlockY(this.pacman.Position.Y - 1)
                                );
                            }
                        }

                        // Update Pacman's position
                        this.pacman.Update(gameTime);

                        // Update the ghosts' position
                        foreach (byte index in Enum.GetValues(typeof(GhostsIndex)))
                        {
                            this.ghosts[index].Update(gameTime);
                        }

                        // Is pacman touched by a ghost ?
                        BoundingBox pacmanBoundingBox = this.pacman.GetBoundingBox();

                        foreach (byte index in Enum.GetValues(typeof(GhostsIndex)))
                        {
                            if(pacmanBoundingBox.Intersects(this.ghosts[index].GetBoundingBox()) && this.remainingLives > 0)
                            {
                                if(!this.areGhostsVulnerable)
                                {
                                    this.remainingLives--;
                                    this.pacman.Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetPacmanStartPosition());
                                    this.pacman.Direction = Vector2.Zero;

                                    foreach (byte subIndex in Enum.GetValues(typeof(GhostsIndex)))
                                    {
                                        this.ghosts[subIndex].Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetGhostStartPosition((GhostsIndex) subIndex));
                                        this.ghosts[subIndex].Direction = Vector2.Zero;
                                    }

                                    break;
                                }
                                else
                                {
                                    this.currentScore += 200;
                                    this.ghosts[index].Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetGhostStartPosition((GhostsIndex) index));
                                    this.ghosts[index].Direction = Vector2.Zero;
                                }
                            }
                        }

                        // Has the user lost?
                        if (this.remainingLives == 0)
                        {
                            this.currentState = GameState.SCORE;
                        }

                        // Is the level completed?
                        if(this.mazesList[this.currentLevel - 1].PelletsNumber == 0)
                        {
                            if(this.currentLevel < this.mazesList.Count)
                            {
                                this.currentLevel++;

                                this.pacman.Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetPacmanStartPosition());
                                this.pacman.Direction = Vector2.Zero;

                                foreach (byte index in Enum.GetValues(typeof(GhostsIndex)))
                                {
                                    this.ghosts[index].Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetGhostStartPosition((GhostsIndex)index));
                                    this.ghosts[index].Direction = Vector2.Zero;
                                    this.ghosts[index].CurrentMaze = this.mazesList[this.currentLevel - 1];
                                }
                            }
                            else
                            {
                                this.currentState = GameState.SCORE;
                            }
                        }

                        #if DEBUG
                        // Activate cheat codes if it is in debug mode
                        if(keyboardState.IsKeyDown(Keys.Add))
                        {
                            this.currentScore += 100000;
                        }
                        else if(keyboardState.IsKeyDown(Keys.Subtract))
                        {
                            this.currentScore -= 100000;
                        }
                        else if (keyboardState.IsKeyDown(Keys.Multiply) && this.remainingLives < 8 && this.previousKeyboardState.IsKeyUp(Keys.Multiply))
                        {
                            this.remainingLives++;
                        }
                        else if(keyboardState.IsKeyDown(Keys.Divide) && this.remainingLives > 0 && this.previousKeyboardState.IsKeyUp(Keys.Divide))
                        {
                            this.remainingLives--;
                        }
                        else if(keyboardState.IsKeyDown(Keys.PageDown) && this.currentLevel > 0 && this.previousKeyboardState.IsKeyUp(Keys.PageDown))
                        {
                            this.currentLevel--;
                        }
                        else if(keyboardState.IsKeyDown(Keys.PageUp) && this.currentLevel < 255 && this.previousKeyboardState.IsKeyUp(Keys.PageUp))
                        {
                            this.currentLevel++;
                        }
                        else if(keyboardState.IsKeyDown(Keys.F1) && this.previousKeyboardState.IsKeyUp(Keys.F1))
                        {
                            this.areGhostsVulnerable = !this.areGhostsVulnerable;
                        }
                        #endif
                    }
                break;

                case GameState.PAUSED:
                    if(keyboardState.IsKeyDown(Keys.R))
                    {
                        // User pressed R
                        // Resume the game
                        this.currentState = GameState.PLAYING;
                    }
                    else if(keyboardState.IsKeyDown(Keys.E))
                    {
                        // User pressed M
                        // Go back to the main menu
                        this.currentState = GameState.MENU;
                    }
                    else if(keyboardState.IsKeyDown(Keys.Q))
                    {
                        // User pressed Q
                        // Quit the software
                        this.Exit();
                    }
                break;

                case GameState.HELP:
                    if(keyboardState.IsKeyDown(Keys.C))
                    {
                        // User pressed C
                        // Return to the menu
                        this.currentState = GameState.MENU;
                    }
                break;

                case GameState.SCORE:
                    if(keyboardState.IsKeyDown(Keys.C))
                    {
                        // User pressed C
                        // Return to the menu
                        this.currentState = GameState.MENU;
                    }
                    else if(keyboardState.IsKeyDown(Keys.Q))
                    {
                        // User pressed Q
                        // Quit the software
                        this.Exit();
                    }
                break;

                case GameState.MENU:
                    if(keyboardState.IsKeyDown(Keys.N))
                    {
                        // User pressed N
                        // Create a new game
                        Random r = new Random();

                        this.currentState = GameState.PLAYING;
                        this.currentScore = 0;
                        this.remainingLives = 3;
                        this.currentLevel = 1;
                        this.pacman.Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetPacmanStartPosition());
                        this.pacman.Direction = Vector2.Zero;
                        this.pacman.Speed = 0.1f;

                        foreach(byte index in Enum.GetValues(typeof(GhostsIndex)))
                        {
                            this.ghosts[index].Position = MeasureUtility.blockVector2ToActualVector2(this.mazesList[this.currentLevel - 1].GetGhostStartPosition((GhostsIndex) index));
                            this.ghosts[index].Direction = Vector2.Zero;
                            this.ghosts[index].Speed = 0.1f;
                            this.ghosts[index].CurrentMaze = this.mazesList[this.currentLevel - 1];
                        }

                        MeasureUtility.mazeWidth = this.mazesList[this.currentLevel - 1].Width;
                        MeasureUtility.mazeHeight = this.mazesList[this.currentLevel - 1].Height;
                    }
                    else if(keyboardState.IsKeyDown(Keys.H))
                    {
                        // User pressed H
                        // Display help
                        this.currentState = GameState.HELP;
                    }
                    else if(keyboardState.IsKeyDown(Keys.S) && this.previousKeyboardState.IsKeyUp(Keys.S))
                    {
                        // User pressed S
                        // (De)activate sounds
                        this.soundsActivated = !this.soundsActivated;

                        if(this.soundsActivated)
                        {
                            Random r = new Random();
                            MediaPlayer.Play(this.songsList[r.Next(0, this.songsList.Count - 1)]);
                        }
                        else
                        {
                            MediaPlayer.Stop();
                        }
                    }
                    else if(keyboardState.IsKeyDown(Keys.Q))
                    {
                        // User pressed Q
                        // Quit the software
                        this.Exit();
                    }
                break;
            }

            this.previousKeyboardState = keyboardState;

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
            Vector2 textDimensions;
            Color goldColor = new Color(255, 204, 0);

            this.spriteBatch.Begin();

            switch(this.currentState)
            {
                case GameState.PLAYING:
                    // Draw the UI
                    this.spriteBatch.Draw(this.uiTopTexture, Vector2.Zero, Color.White);

                    textDimensions = this.uiFont.MeasureString(this.currentScore.ToString());
                    this.spriteBatch.DrawString(
                        this.uiFont,
                        this.currentScore.ToString(),
                        new Vector2(30, 3 + (36 - textDimensions.Y) / 2),
                        goldColor
                    );

                    textDimensions = this.uiFont.MeasureString(this.remainingLives.ToString());
                    this.spriteBatch.DrawString(
                        this.uiFont,
                        this.remainingLives.ToString(),
                        new Vector2(376 + (42 - textDimensions.X) / 2, 24 + (42 - textDimensions.Y) / 2),
                        goldColor
                    );

                    textDimensions = this.uiFont.MeasureString(this.currentLevel.ToString() + " / " + this.mazesList.Count.ToString());
                    this.spriteBatch.DrawString(
                        this.uiFont,
                        this.currentLevel.ToString() + " / " + this.mazesList.Count.ToString(),
                        new Vector2(772 - textDimensions.X, 3 + (36 - textDimensions.Y) / 2),
                        goldColor
                    );

                    for(uint i = 0; i < this.remainingLives; i++)
                    {
                        this.spriteBatch.Draw(
                            this.uiLifeTexture,
                            this.livesPositions[i],
                            Color.White
                        );
                    }

                    // Draw the maze
                    this.mazesList[this.currentLevel - 1].Draw(gameTime, this.spriteBatch);

                    // Draw the ghosts
                    foreach (byte index in Enum.GetValues(typeof(GhostsIndex)))
                    {
                        this.ghosts[index].Draw(gameTime, this.spriteBatch);
                    }

                    // Draw Pacman
                    this.pacman.Draw(gameTime, this.spriteBatch);
                break;

                case GameState.PAUSED:
                    // Draw the UI
                    this.spriteBatch.Draw(
                        this.uiPauseTexture,
                        new Vector2((this.graphics.PreferredBackBufferWidth - this.uiPauseTexture.Width) / 2, (this.graphics.PreferredBackBufferHeight - this.uiPauseTexture.Height) / 2),
                        Color.White
                    );
                break;

                case GameState.HELP:
                    // Draw the UI
                    this.spriteBatch.Draw(
                        this.uiHelpTexture,
                        new Vector2((this.graphics.PreferredBackBufferWidth - this.uiHelpTexture.Width) / 2, (this.graphics.PreferredBackBufferHeight - this.uiHelpTexture.Height) / 2),
                        Color.White
                    );
                break;

                case GameState.SCORE:
                    // Draw the UI
                    int x = (this.graphics.PreferredBackBufferWidth - this.uiScoreTexture.Width) / 2;
                    int y = (this.graphics.PreferredBackBufferHeight - this.uiScoreTexture.Height) / 2;

                    this.spriteBatch.Draw(
                        this.uiScoreTexture,
                        new Vector2(x , y),
                        Color.White
                    );

                    // Draw score
                    String scoreText = String.Format("You reached level {0} with {1}.", this.currentLevel, this.currentScore);
                    textDimensions = this.uiFont.MeasureString(scoreText);
                    x += 40 + (int) (359 - textDimensions.X) / 2;
                    y += 42;

                    this.spriteBatch.DrawString(
                        this.uiFont,
                        "You reached level ",
                        new Vector2(x, y + (63 - textDimensions.Y) / 2),
                        Color.Gold
                    );

                    x += (int) this.uiFont.MeasureString("You reached level ").X;

                    this.spriteBatch.DrawString(
                        this.uiFont,
                        this.currentLevel.ToString(),
                        new Vector2(x, y + (63 - textDimensions.Y) / 2),
                        Color.White
                    );

                    x += (int) this.uiFont.MeasureString(this.currentLevel.ToString()).X;

                    this.spriteBatch.DrawString(
                        this.uiFont,
                        " with ",
                        new Vector2(x, y + (63 - textDimensions.Y) / 2),
                        goldColor
                    );

                    x += (int) this.uiFont.MeasureString(" with ").X;

                    this.spriteBatch.DrawString(
                        this.uiFont,
                        this.currentScore.ToString(),
                        new Vector2(x, y + (63 - textDimensions.Y) / 2),
                        Color.White
                    );

                    x += (int) this.uiFont.MeasureString(this.currentScore.ToString()).X;

                    this.spriteBatch.DrawString(
                        this.uiFont,
                        ".",
                        new Vector2(x, y + (63 - textDimensions.Y) / 2),
                        goldColor
                    );
                break;

                case GameState.MENU:
                    // Draw the UI
                    Texture2D uiTexture = this.soundsActivated ? this.uiMenuSoundsOnTexture : this.uiMenuSoundsOffTexture;

                    this.spriteBatch.Draw(
                        uiTexture,
                        new Vector2((this.graphics.PreferredBackBufferWidth - uiTexture.Width) / 2, (this.graphics.PreferredBackBufferHeight - uiTexture.Height) / 2),
                        Color.White
                    );
                break;
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="direction"></param>
        /// <param name="nextX"></param>
        /// <param name="nextY"></param>
        protected void HandleCollision(GameTime gameTime, MovementDirection direction, uint nextX, uint nextY)
        {
            try
            {
                // Get the block type
                Maze.BlockTypes blockType = this.mazesList[this.currentLevel - 1][nextX, nextY];

                // And then react according to it
                BoundingBox blockBoundingBox = this.mazesList[this.currentLevel - 1].GetBoundingBoxAt(nextX, nextY);
                BoundingBox pacmanBoundingBox = this.pacman.GetBoundingBox();

                switch(blockType)
                {
                    case Maze.BlockTypes.WALL:
                    case Maze.BlockTypes.DOOR:
                        // Translate pacman's bounding box to match its next movement
                        switch(direction)
                        {
                            case MovementDirection.LEFT:
                                pacmanBoundingBox.Min.X--;
                                pacmanBoundingBox.Max.X--;
                            break;

                            case MovementDirection.UP:
                                pacmanBoundingBox.Min.Y--;
                                pacmanBoundingBox.Max.Y--;
                            break;

                            case MovementDirection.RIGHT:
                                pacmanBoundingBox.Min.X++;
                                pacmanBoundingBox.Max.X++;
                            break;

                            case MovementDirection.DOWN:
                                pacmanBoundingBox.Min.Y++;
                                pacmanBoundingBox.Max.Y++;
                            break;
                        }

                        // Then, then if it intersects with the obstacle's
                        if(pacmanBoundingBox.Intersects(blockBoundingBox))
                        {
                            this.pacman.Direction = Vector2.Zero;
                        }
                    break;

                    case Maze.BlockTypes.PELLET:
                        if(pacmanBoundingBox.Intersects(blockBoundingBox))
                        {
                            this.mazesList[this.currentLevel - 1][nextX, nextY] = Maze.BlockTypes.EMPTY;
                            this.mazesList[this.currentLevel - 1].PelletsNumber--;
                            this.currentScore += 10;
                        }
                    break;

                    case Maze.BlockTypes.ENERGIZER:
                        if(pacmanBoundingBox.Intersects(blockBoundingBox))
                        {
                            this.mazesList[this.currentLevel - 1][nextX, nextY] = Maze.BlockTypes.EMPTY;
                            this.areGhostsVulnerable = true;
                            this.vulnerableGhostsBeginning = gameTime.TotalGameTime;

                            foreach (byte index in Enum.GetValues(typeof(GhostsIndex)))
                            {
                                this.ghosts[index].IsVulnerable = true;
                            }

                            if(this.soundsActivated)
                            {
                                this.onEnergizerEaten.Play();
                            }
                        }
                    break;
                }
            }
            catch(IndexOutOfRangeException)
            {
                // Stop pacman from exiting the maze
                this.pacman.Direction = Vector2.Zero;
            }
        }
    }
}