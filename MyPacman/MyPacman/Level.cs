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
            /// Represents a pellet Pacman has to eat.
            /// </summary>
            PELLET,
            /// <summary>
            /// Represents an energizer Pacman can eat to become stronger.
            /// </summary>
            ENERGIZER
        };

        /// <summary>
        /// Represents nothing.
        /// </summary>
        protected const char CHAR_EMPTY = ' ';

        /// <summary>
        /// Represents a door only ghosts can go through.
        /// </summary>
        protected const char CHAR_DOOR = 'o';

        /// <summary>
        /// Represents a wall.
        /// </summary>
        protected const char CHAR_WALL = '#';

        /// <summary>
        /// Represents a pellet Pacman has to eat.
        /// </summary>
        protected const char CHAR_PELLET = '*';

        /// <summary>
        /// Represents an energizer Pacman can eat to become stronger.
        /// </summary>
        protected const char CHAR_ENERGIZER = '+';

        /// <summary>
        /// Represents a regex for the header of a level file.
        /// </summary>
        protected static readonly Regex HEADER_REGEX;

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
        /// Holds a texture for a wall.
        /// </summary>
        protected Texture2D wallTexture;

        /// <summary>
        /// Holds a texture for a door.
        /// </summary>
        protected Texture2D doorTexture;

        /// <summary>
        /// Holds a texture for a pellet.
        /// </summary>
        protected Texture2D pelletTexture;

        /// <summary>
        /// Holds a texture for an energizer.
        /// </summary>
        protected Texture2D energizerTexture;

        /// <summary>
        /// Static constructor of a level.
        /// </summary>
        static Level()
        {
            Level.HEADER_REGEX = new Regex("MyPacman level ([0-9]+)x([0-9]+)");
        }

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

        /// <summary>
        /// Gets or sets a block type in the matrix of a level.
        /// </summary>
        /// <param name="x">Abscissa of the block.</param>
        /// <param name="y">Ordinate of the block</param>
        public BlockTypes this[uint x, uint y]
        {
            get
            {
                if(x < this.width && y < this.height)
                {
                    return this.levelMatrix[x,y];
                }
                else
                {
                    throw new IndexOutOfRangeException(String.Format("Couldn't access cell %d,%d because it is out of range.", x, y));
                }
            }

            set
            {
                if(x < this.width && y < this.height)
                {
                    this.levelMatrix[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException(String.Format("Couldn't change cell %d,%d because it is out of range.", x, y));
                }
            }
        }

        /// <summary>
        /// Loads a level from a file.
        /// </summary>
        /// <param name="filename">Path of the file to load.</param>
        /// <returns>Reference to the loaded level.</returns>
        public static Level Load(String filename)
        {
            if(File.Exists(filename))
            {
                try
                {
                    String[] lines = File.ReadAllLines(filename);

                    if(lines.Length > 0)
                    {
                        Match headerMatch = Level.HEADER_REGEX.Match(lines[0]);

                        if(headerMatch.Success)
                        {
                            for(int i = 0, j = headerMatch.Groups.Count; i < j; i++)
                                File.AppendAllText(@"C:\Users\PtitBlond\Desktop\matches.log", headerMatch.Groups[i].ToString() + "\n");

                            if(lines.Length == UInt32.Parse(headerMatch.Groups[2].Value) + 1)
                            {
                                uint levelWidth = UInt32.Parse(headerMatch.Groups[1].Value);
                                uint levelHeight = UInt32.Parse(headerMatch.Groups[2].Value);
                                BlockTypes[,] levelMatrix = new BlockTypes[levelWidth, levelHeight];

                                // TODO: Parse file while being cautious about the dimensions
                                for(int y = 0; y < levelHeight; y++)
                                {
                                    String s = lines[y + 1];

                                    if (s.Length == levelWidth)
                                    {
                                        for(int x = 0; x < levelWidth; x++)
                                        {
                                            char c = s[x];

                                            switch(c)
                                            {
                                                case CHAR_EMPTY:
                                                    levelMatrix[x, y] = BlockTypes.EMPTY;
                                                break;

                                                case CHAR_DOOR:
                                                    levelMatrix[x, y] = BlockTypes.DOOR;
                                                break;

                                                case CHAR_WALL:
                                                    levelMatrix[x, y] = BlockTypes.WALL;
                                                break;

                                                case CHAR_PELLET:
                                                    levelMatrix[x, y] = BlockTypes.PELLET;
                                                break;

                                                case CHAR_ENERGIZER:
                                                    levelMatrix[x, y] = BlockTypes.ENERGIZER;
                                                break;

                                                default:
                                                    throw new ArgumentException(String.Format("File \"%s\" isn't valid: char %c at line %d column %d isn't valid.", filename, c, y + 2, x + 1));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new ArgumentException(String.Format("File \"%s\" isn't valid: line %d is %d long and not %d.", filename, y + 2, s.Length, levelWidth));
                                    }
                                }

                                return new Level(levelWidth, levelHeight, levelMatrix);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("File \"%s\" isn't valid: not enough lines.", filename));
                            }
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("File \"%s\" isn't valid: no header.", filename));
                        }
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("File \"%s\" is empty.", filename));
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new ArgumentException(String.Format("File \"%s\" doesn't exist.", filename));
            }
        }

        /// <summary>
        /// Initializes a level.
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads graphic contents for a level.
        /// </summary>
        /// <param name="contentManager">Reference to the content manager.</param>
        public override void LoadContent(ContentManager contentManager)
        {
            // Load a wall image
            this.wallTexture = contentManager.Load<Texture2D>(@"images\wall");

            // Load a door image
            this.doorTexture = contentManager.Load<Texture2D>(@"images\door");

            // TODO: Load a pellet image
            this.pelletTexture = contentManager.Load<Texture2D>(@"images\pellet");

            // TODO: Load an energizer image
            this.energizerTexture = contentManager.Load<Texture2D>(@"images\energizer");
        }

        /// <summary>
        /// Levels do not need to unload any non-graphic resources.
        /// </summary>
        public override void UnloadContent()
        {
            throw new NotSupportedException("Levels do not need to unload any non-graphic resources.");
        }

        /// <summary>
        /// Levels do not update themselves.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            throw new NotSupportedException("Levels do not update themselves.");
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
                            spriteBatch.Draw(this.wallTexture, new Vector2(MeasureUtility.blockXToActualX(x), MeasureUtility.blockYToActualY(y)), Color.White);
                        break;

                        case BlockTypes.DOOR:
                            // Draw the door
                            spriteBatch.Draw(this.doorTexture, new Vector2(MeasureUtility.blockXToActualX(x), MeasureUtility.blockYToActualY(y)), Color.White);
                        break;

                        case BlockTypes.PELLET:
                            // Draw the pellet
                            spriteBatch.Draw(this.pelletTexture, new Vector2(MeasureUtility.blockXToActualX(x), MeasureUtility.blockYToActualY(y)), Color.White);
                        break;

                        case BlockTypes.ENERGIZER:
                            // Draw the energizer
                            spriteBatch.Draw(this.energizerTexture, new Vector2(MeasureUtility.blockXToActualX(x), MeasureUtility.blockYToActualY(y)), Color.White);
                        break;

                        // Everything else doesn't need to appear on the window
                    }
                }
            }
        }
    }
}
