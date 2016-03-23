using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyPacman
{
    public abstract class Ghost : Sprite
    {
        /// <summary>
        /// Holds a reference to the current maze.
        /// </summary>
        Maze currentMaze;

        public Maze CurrentMaze
        {
            set
            {
                this.currentMaze = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Boolean isVulnerable;
        
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsVulnerable
        {
            get
            {
                return this.isVulnerable;
            }

            set
            {
                this.isVulnerable = value;
            }
        }


        /// <summary>
        /// Initializes a ghost.
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ghosts do not need to unload any non-graphic content.
        /// </summary>
        public override void UnloadContent()
        {
            throw new NotSupportedException("Ghosts do not need to unload any non-graphic content.");
        }


        /// <summary>
        /// General AI for the ghosts.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Determine which directions are available
            List<Vector2> availableDirections = new List<Vector2>();
            uint currentX = (uint) MeasureUtility.ActualXToBlockX(this.position.X);
            uint currentY = (uint) MeasureUtility.ActualYToBlockY(this.position.Y);

            // Left
            if(currentX > 0 && this.currentMaze[currentX - 1, currentY] != Maze.BlockTypes.WALL)
            {
                availableDirections.Add(-Vector2.UnitX);
            }
            // Up
            else if (currentY > 0 && this.currentMaze[currentX, currentY - 1] != Maze.BlockTypes.WALL)
            {
                availableDirections.Add(-Vector2.UnitY);
            }
            // Right
            else if (currentY < this.currentMaze.Width - 1 && this.currentMaze[currentX + 1, currentY] != Maze.BlockTypes.WALL)
            {
                availableDirections.Add(Vector2.UnitX);
            }
            // Down
            else if (currentY < this.currentMaze.Height - 1 && this.currentMaze[currentX, currentY + 1] != Maze.BlockTypes.WALL)
            {
                availableDirections.Add(Vector2.UnitY);
            }

            if(availableDirections.Count > 0)
            {
                Random r = new Random();
                this.direction = availableDirections[r.Next(availableDirections.Count)];
            }

            if (this.direction != Vector2.Zero)
            {
                this.position += this.direction * this.speed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Ghosts do not react to user input.
        /// </summary>
        /// <param name="state"></param>
        public override void HandleKeyboard(KeyboardState state)
        {
            throw new NotSupportedException("Ghosts do not react to user input.");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Vector3(
                    this.position.X,
                    this.position.Y,
                    0
                ),
                new Vector3(
                    this.position.X + MeasureUtility.BLOCK_WIDTH - 1,
                    this.position.Y + MeasureUtility.BLOCK_HEIGHT - 1,
                    0
                )
            );
        }
    }
}
