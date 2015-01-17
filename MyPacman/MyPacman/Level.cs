using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyPacman
{
    public class Level : Sprite
    {
        /// <summary>
        /// Represents a regex for the header of a level file.
        /// </summary>
        protected static const Regex headerRegex = new Regex("MyPacman ([0-9]+)x([0-9]+) level");

        /// <summary>
        /// Represents different kinds of blocks which can be found in the level.
        /// </summary>
        public enum BlockTypes : byte
        {
            /// <summary>
            /// Represents nothing.
            /// </summary>
            EMPTY,
            /// <summary>
            /// Represents a door only ghosts can go through.
            /// </summary>
            DOOR,
            /// <summary>
            /// Represents a wall.
            /// </summary>
            WALL,
            /// <summary>
            /// Represents a pellet Pacman has to eat;
            /// </summary>
            PELLET,
            /// <summary>
            /// Represents an energizer Pacman can eat to become stronger.
            /// </summary>
            ENERGIZER
        };

        /// <summary>
        /// Holds the width of the level.
        /// </summary>
        protected readonly uint width;

        /// <summary>
        /// Holds the height of the level.
        /// </summary>
        protected readonly uint height;

        /// <summary>
        /// Holds a matrix representing the level.
        /// </summary>
        protected BlockTypes[,] levelMatrix;

        /// <summary>
        /// 
        /// </summary>
        protected Texture2D wallTexture;

        /// <summary>
        /// 
        /// </summary>
        protected Texture2D doorTexture;

        /// <summary>
        /// 
        /// </summary>
        protected Texture2D pelletTexture;

        /// <summary>
        /// 
        /// </summary>
        protected Texture2D energizerTexture;

        /// <summary>
        /// Creates a new level.
        /// </summary>
        /// <param name="width">Width of the level.</param>
        /// <param name="height">Height of the level.</param>
        /// <param name="levelMatrix">Matrix representing the level.</param>
        /// <remarks>
        ///     Beware of the order when creating the level matrix: you have to describe
        ///     the contents of each column, not each line.
        ///     
        ///     To produce this 11x10 level,
        ///     <code>
        ///         EEEEEEEEEEE
        ///         EWWEWWWEWWE
        ///         EWEEEEEEEWE
        ///         EWEWWDWWEWE
        ///         EEEWEEEWEEE
        ///         EEEWEEEWEEE
        ///         EWEWWWWWEWE
        ///         EWEEEEEEEWE
        ///         EWWEWWWEWWE
        ///     </code>
        ///     You have to describe it like so,
        ///     <code>
        ///         new Level(11, 10, new Level.BlockTypes[11,10]{
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.DOOR, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.WALL, Level.BlockTypes.EMPTY},
        ///             {Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY, Level.BlockTypes.EMPTY}
        ///         });
        ///     </code>
        /// </remarks>
        public Level(uint width, uint height, BlockTypes[,] levelMatrix)
        {
            this.width = width;
            this.height = height;

            // Parse the matrix to ensure width, height
            // Parse the matrix to find the numbers of pellets
            // Parse the matrix to find the starting points of pacman and the ghosts
            // Then, save it
            this.levelMatrix = levelMatrix;
        }

        public static Level Load(String filename)
        {
            if(File.Exists(filename))
            {
                try
                {
                    String[] lines = File.ReadAllLines(filename);

                    if(lines.Length > 0)
                    {
                        Match headerMatch = Level.headerRegex.Match(lines[0]);

                        if(headerMatch.Success && lines.Length == UInt32.Parse(headerMatch.Captures[2].Value) + 1)
                        {
                            uint levelWidth = UInt32.Parse(headerMatch.Captures[1].Value);
                            uint levelHeight = UInt32.Parse(headerMatch.Captures[2].Value);
                            BlockTypes[,] levelMatrix = new BlockTypes[levelWidth, levelHeight];

                            // TODO: Parse file while being cautious about the dimensions

                            return new Level(levelWidth, levelHeight, levelMatrix);
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("File \"%s\" isn't valid.", filename));
                        }
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("File \"%s\" is empty.", filename));
                    }
                }
                catch(IOException e)
                {
                
                }
            }
            else
            {
                throw new ArgumentException(String.Format("File \"%s\" doesn't exist.", filename));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentManager"></param>
        public override void LoadContent(ContentManager contentManager)
        {
            // Load a wall image
            this.wallTexture = contentManager.Load<Texture2D>(@"images\wall");

            // Load a door image
            this.doorTexture = contentManager.Load<Texture2D>(@"images\door");

            // TODO: Load a pellet image

            // TODO: Load an energizer image

        }

        /// <summary>
        /// Levels do not need to unload any non-graphic resources.
        /// </summary>
        public override void UnloadContent()
        {
            throw new NotSupportedException("Levels do not need to unload any non-graphic resources.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Levels do not react to user input.
        /// </summary>
        public override void HandleKeyboard(KeyboardState state)
        {
            throw new NotSupportedException("Levels do not react to user input.");
        }

        /// <summary>
        /// Draws the level.
        /// </summary>
        /// <param name="spriteBatch">Reference to the sprite batch.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        /// Draws the level using the matrix and the following formulas :
        /// 
        ///  * `realX = matrixX * Game.BLOCK_WIDTH`
        ///  * `realY = matrixY * Game.BLOCK_HEIGHT + topBarShift`
        ///  
        /// Some types of blocks do not need to appear on the window, for example,
        /// startings points 
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(uint y = 0; y < this.height; y++)
            {
                for(uint x = 0; x < this.width; x++)
                {
                    switch(this.levelMatrix[x, y])
                    {
                        case BlockTypes.WALL:
                            // Draw the wall
                            spriteBatch.Draw(this.wallTexture, new Vector2(this.calculateRealX(x), this.calculateRealY(y)), Color.White);
                        break;

                        case BlockTypes.DOOR:
                            // Draw the door
                            spriteBatch.Draw(this.doorTexture, new Vector2(this.calculateRealX(x), this.calculateRealY(y)), Color.White);
                        break;

                        case BlockTypes.PELLET:
                            // Draw the pellet
                        break;

                        case BlockTypes.ENERGIZER:
                            // Draw the energizer
                        break;

                        // Everything else doesn't need to appear on the window
                    }
                }
            }
        }

        protected float calculateRealX(float x)
        {
            return x * Game.BLOCK_WIDTH;
        }

        protected float calculateRealY(float y)
        {
            return y * Game.BLOCK_HEIGHT;
        }
    }
}
