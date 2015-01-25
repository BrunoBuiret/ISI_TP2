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
    public class Maze : Sprite
    {
        /// <summary>
        /// Represents different kinds of blocks which can be found in a maze.
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
            ENERGIZER,
            /// <summary>
            /// Represents Pacman's start point.
            /// </summary>
            PACMAN_START,
            /// <summary>
            /// Represents Blinky's start point.
            /// </summary>
            BLINKY_START,
            /// <summary>
            /// Represents Clyde's start point.
            /// </summary>
            CLYDE_START,
            /// <summary>
            /// Represents Inky's start point.
            /// </summary>
            INKY_START,
            /// <summary>
            /// Represents Pinky's start point.
            /// </summary>
            PINKY_START
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
        /// Represents Pacman's start point.
        /// </summary>
        protected const char CHAR_PACMAN_START = '0';

        /// <summary>
        /// Represents Blinky's start point.
        /// </summary>
        protected const char CHAR_BLINKY_START = '1';

        /// <summary>
        /// Represents Clyde's start point.
        /// </summary>
        protected const char CHAR_CLYDE_START = '2';

        /// <summary>
        /// Represents Inky's start point.
        /// </summary>
        protected const char CHAR_INKY_START = '3';

        /// <summary>
        /// Represents Pinky's start point.
        /// </summary>
        protected const char CHAR_PINKY_START = '4';

        /// <summary>
        /// Holds a regex for the header of a maze file.
        /// </summary>
        protected static readonly Regex HEADER_REGEX;

        /// <summary>
        /// Holds the width of a maze.
        /// </summary>
        protected readonly uint width;

        /// <summary>
        /// Gets the width of a maze.
        /// </summary>
        public uint Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// Holds the height of a maze.
        /// </summary>
        protected readonly uint height;

        /// <summary>
        /// Gets the height of a maze.
        /// </summary>
        public uint Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Holds a matrix representing the level.
        /// </summary>
        protected BlockTypes[,] levelMatrix;

        /// <summary>
        /// Gets or sets a block type in the matrix of a level.
        /// </summary>
        /// <param name="x">Abscissa of the block.</param>
        /// <param name="y">Ordinate of the block</param>
        public BlockTypes this[uint x, uint y]
        {
            get
            {
                if (x < this.width && y < this.height)
                {
                    return this.levelMatrix[x, y];
                }
                else
                {
                    throw new IndexOutOfRangeException(String.Format("Couldn't access cell {0},{1} because it is out of range.", x, y));
                }
            }

            set
            {
                if (x < this.width && y < this.height)
                {
                    this.levelMatrix[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException(String.Format("Couldn't change cell {0},{1} because it is out of range.", x, y));
                }
            }
        }

        /// <summary>
        /// Represents the number of remaining pellets.
        /// </summary>
        protected uint pelletsNumber;

        /// <summary>
        /// Gets or sets the pellets number of a maze.
        /// </summary>
        public uint PelletsNumber
        {
            get
            {
                return this.pelletsNumber;
            }

            set
            {
                this.pelletsNumber = value;
            }
        }

        /// <summary>
        /// Holds Pacman's start point.
        /// </summary>
        protected readonly Vector2 pacmanStartPosition;

        /// <summary>
        /// Gets Pacman's start point.
        /// </summary>
        /// <returns>Pacman's start point.</returns>
        public Vector2 GetPacmanStartPosition()
        {
            return this.pacmanStartPosition;
        }

        /// <summary>
        /// Holds the ghosts' start points.
        /// </summary>
        protected readonly Vector2[] ghostsStartPosition;

        /// <summary>
        /// Gets the ghosts' start points.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Ghost's start point.</returns>
        public Vector2 GetGhostStartPosition(Game.GhostsIndex index)
        {
            return this.ghostsStartPosition[(int) index];
        }

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
        static Maze()
        {
            Maze.HEADER_REGEX = new Regex("MyPacman maze ([0-9]+)x([0-9]+)");
        }

        /// <summary>
        /// Creates a new level.
        /// </summary>
        /// <param name="width">Width of the level.</param>
        /// <param name="height">Height of the level.</param>
        /// <param name="levelMatrix">Matrix representing the level.</param>
        public Maze(uint width, uint height, BlockTypes[,] levelMatrix)
        {
            // Check width
            if(levelMatrix.Length != width * height)
            {
                throw new ArgumentException(String.Format("Parameters mismatch: parameters indicate {0} cells but the matrix has {1}.", width * height, levelMatrix.Length));
            }

            // Go through the matrix to check and parse it
            this.pelletsNumber = 0;
            this.ghostsStartPosition = new Vector2[4];
            this.pacmanStartPosition = this.ghostsStartPosition[0] = this.ghostsStartPosition[1] = this.ghostsStartPosition[2] = this.ghostsStartPosition[3] = new Vector2(-1, -1);

            for(uint x = 0; x < width; x++)
            {
                for(uint y = 0; y < height; y++)
                {
                    switch(levelMatrix[x, y])
                    {
                        case BlockTypes.PELLET:
                            this.pelletsNumber++;
                        break;

                        case BlockTypes.PACMAN_START:
                            if(this.pacmanStartPosition.X == -1 && this.pacmanStartPosition.Y == -1)
                            {
                                this.pacmanStartPosition = new Vector2(x, y);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("Pacman's start position was already set at {0},{1}.", this.pacmanStartPosition.X, this.pacmanStartPosition.Y));
                            }
                        break;

                        case BlockTypes.BLINKY_START:
                            if (this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY].Y == -1)
                            {
                                this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY] = new Vector2(x, y);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("Blinky's start position was already set at {0},{1}.", this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY].X, this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY].Y));
                            }
                        break;

                        case BlockTypes.CLYDE_START:
                            if (this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE].Y == -1)
                            {
                                this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE] = new Vector2(x, y);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("Clyde's start position was already set at {0},{1}.", this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE].X, this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE].Y));
                            }
                        break;

                        case BlockTypes.INKY_START:
                            if (this.ghostsStartPosition[(int) Game.GhostsIndex.INKY].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.INKY].Y == -1)
                            {
                                this.ghostsStartPosition[(int) Game.GhostsIndex.INKY] = new Vector2(x, y);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("Inky's start position was already set at {0},{1}.", this.ghostsStartPosition[(int) Game.GhostsIndex.INKY].X, this.ghostsStartPosition[(int) Game.GhostsIndex.INKY].Y));
                            }
                        break;

                        case BlockTypes.PINKY_START:
                            if (this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY].Y == -1)
                            {
                                this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY] = new Vector2(x, y);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("Blinky's start position was already set at {0},{1}.", this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY].X, this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY].Y));
                            }
                        break;
                    }
                }
            }

            if(this.pacmanStartPosition.X == -1 && this.pacmanStartPosition.Y == -1)
            {
                throw new ArgumentException("Pacman doesn't have a start point.");
            }
            else if (this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.BLINKY].Y == -1)
            {
                throw new ArgumentException("Blinky doesn't have a start point.");
            }
            else if (this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.CLYDE].Y == -1)
            {
                throw new ArgumentException("Clyde doesn't have a start point.");
            }
            else if (this.ghostsStartPosition[(int) Game.GhostsIndex.INKY].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.INKY].Y == -1)
            {
                throw new ArgumentException("Inky doesn't have a start point.");
            }
            else if (this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY].X == -1 && this.ghostsStartPosition[(int) Game.GhostsIndex.PINKY].Y == -1)
            {
                throw new ArgumentException("Pinky doesn't have a start point.");
            }

            this.width = width;
            this.height = height;
            this.levelMatrix = levelMatrix;
        }

        /// <summary>
        /// Loads a level from a file.
        /// </summary>
        /// <param name="filename">Path of the file to load.</param>
        /// <returns>Reference to the loaded level.</returns>
        public static Maze Load(String filename)
        {
            if(File.Exists(filename))
            {
                try
                {
                    String[] lines = File.ReadAllLines(filename);

                    if(lines.Length > 0)
                    {
                        Match headerMatch = Maze.HEADER_REGEX.Match(lines[0]);

                        if(headerMatch.Success)
                        {
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

                                                case CHAR_PACMAN_START:
                                                    levelMatrix[x, y] = BlockTypes.PACMAN_START;
                                                break;

                                                case CHAR_BLINKY_START:
                                                    levelMatrix[x, y] = BlockTypes.BLINKY_START;
                                                break;

                                                case CHAR_CLYDE_START:
                                                    levelMatrix[x, y] = BlockTypes.CLYDE_START;
                                                break;

                                                case CHAR_INKY_START:
                                                    levelMatrix[x, y] = BlockTypes.INKY_START;
                                                break;

                                                case CHAR_PINKY_START:
                                                    levelMatrix[x, y] = BlockTypes.PINKY_START;
                                                break;

                                                default:
                                                    throw new ArgumentException(String.Format("File \"{0}\" isn't valid: char {1} at line {2} column {3} isn't valid.", filename, c, y + 2, x + 1));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new ArgumentException(String.Format("File \"{0}\" isn't valid: line {1} is {2} long and not {3}.", filename, y + 2, s.Length, levelWidth));
                                    }
                                }

                                return new Maze(levelWidth, levelHeight, levelMatrix);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("File \"{0}\" isn't valid: not enough lines.", filename));
                            }
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("File \"{0}\" isn't valid: no header.", filename));
                        }
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("File \"{0}\" is empty.", filename));
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new ArgumentException(String.Format("File \"{0}\" doesn't exist.", filename));
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
                            spriteBatch.Draw(this.wallTexture, new Vector2(MeasureUtility.BlockXToActualX(x), MeasureUtility.BlockYToActualY(y)), Color.White);
                        break;

                        case BlockTypes.DOOR:
                            // Draw the door
                            spriteBatch.Draw(this.doorTexture, new Vector2(MeasureUtility.BlockXToActualX(x), MeasureUtility.BlockYToActualY(y)), Color.White);
                        break;

                        case BlockTypes.PELLET:
                            // Draw the pellet
                            spriteBatch.Draw(this.pelletTexture, new Vector2(MeasureUtility.BlockXToActualX(x), MeasureUtility.BlockYToActualY(y)), Color.White);
                        break;

                        case BlockTypes.ENERGIZER:
                            // Draw the energizer
                            spriteBatch.Draw(this.energizerTexture, new Vector2(MeasureUtility.BlockXToActualX(x), MeasureUtility.BlockYToActualY(y)), Color.White);
                        break;

                        // Everything else doesn't need to appear on the window
                    }
                }
            }
        }

        /// <summary>
        /// Gets a bounding box associated with a cell.
        /// </summary>
        /// <param name="x">Cell's abscissa.</param>
        /// <param name="y">Cell's ordinate.</param>
        /// <returns>Cell's bounding box.</returns>
        public BoundingBox GetBoundingBoxAt(uint x, uint y)
        {
            if(x < this.width && y < this.height)
            {
                switch(this.levelMatrix[x, y])
                {
                    case BlockTypes.DOOR:
                        return new BoundingBox(
                            new Vector3(
                                MeasureUtility.BlockXToActualX(x),
                                MeasureUtility.BlockYToActualY(y) + 5,
                                0
                            ),
                            new Vector3(
                                MeasureUtility.BlockXToActualX(x) + MeasureUtility.BLOCK_WIDTH,
                                MeasureUtility.BlockYToActualY(y) + 15,
                                0
                            )
                        );

                    case BlockTypes.WALL:
                        return new BoundingBox(
                            new Vector3(MeasureUtility.BlockXToActualX(x), MeasureUtility.BlockYToActualY(y), 0),
                            new Vector3(MeasureUtility.BlockXToActualX(x) + MeasureUtility.BLOCK_WIDTH, MeasureUtility.BlockYToActualY(y) + MeasureUtility.BLOCK_HEIGHT, 0)
                        );

                    case BlockTypes.PELLET:
                        return new BoundingBox(
                            new Vector3(MeasureUtility.BlockXToActualX(x) + 8, MeasureUtility.BlockYToActualY(y) + 8, 0),
                            new Vector3(MeasureUtility.BlockXToActualX(x) + 10, MeasureUtility.BlockYToActualY(y) + 10, 0)
                        );

                    case BlockTypes.ENERGIZER:
                        return new BoundingBox(
                            new Vector3(MeasureUtility.BlockXToActualX(x) + 4, MeasureUtility.BlockYToActualY(y) + 4, 0),
                            new Vector3(MeasureUtility.BlockXToActualX(x) + 15, MeasureUtility.BlockYToActualY(y) + 15, 0)
                        );

                    default:
                        return new BoundingBox();
                }
            }
            else
            {
                throw new ArgumentException(String.Format("Point {0},{1} doesn't belong to this maze.", x, y));
            }
        }
    }
}
