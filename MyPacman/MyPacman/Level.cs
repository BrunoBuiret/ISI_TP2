using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPacman
{
    class Level : Sprite
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
        protected BlockTypes[][] levelMatrix;

        /// <summary>
        /// Creates a new level.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="levelMatrix"></param>
        public Level(uint width, uint height, BlockTypes[][] levelMatrix)
        {
            this.width = width;
            this.height = height;
            this.levelMatrix = levelMatrix;
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
            throw new NotImplementedException();
            // Load a door image
            // Load a wall image
            // Load a pellet image
            // Load an energizer image

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
        public override void HandleInput()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
