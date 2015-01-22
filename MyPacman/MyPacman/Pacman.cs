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
    public class Pacman : Sprite
    {
        protected enum MovementDirection : byte
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        };

        protected MovementDirection lastMovementDirection;

        protected int currentFrame;

        protected float elapsedTime;

        /// <summary>
        /// Initializes pacman.
        /// </summary>
        public override void Initialize()
        {
            this.lastMovementDirection = MovementDirection.LEFT;
            this.currentFrame = 0;
        }

        /// <summary>
        /// Loads graphic contents for pacman.
        /// </summary>
        /// <param name="contentManager">Reference to the content manager.</param>
        public override void LoadContent(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(@"images\pacman-sprite");
        }

        /// <summary>
        /// Pacman doesn't need to unload any non-graphic content.
        /// </summary>
        public override void UnloadContent()
        {
            throw new NotSupportedException("Pacman doesn't need to unload non-graphic content.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Move pacman
            if(this.direction != Vector2.Zero)
            {
                this.position += this.direction * this.speed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            // Update frame if necessary
            this.elapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;

            if(this.elapsedTime > 0.5f)
            {
                this.currentFrame = ++this.currentFrame % 2;
                this.elapsedTime -= 0.5f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public override void HandleKeyboard(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.Left))
            {
                this.direction = -Vector2.UnitX;
                this.lastMovementDirection = MovementDirection.LEFT;
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                this.direction = -Vector2.UnitY;
                this.lastMovementDirection = MovementDirection.UP;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                this.direction = Vector2.UnitX;
                this.lastMovementDirection = MovementDirection.RIGHT;
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                this.direction = Vector2.UnitY;
                this.lastMovementDirection = MovementDirection.DOWN;
            }
            else
            {
                this.direction = Vector2.Zero;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch(this.lastMovementDirection)
            {
                case MovementDirection.LEFT:
                    spriteBatch.Draw(
                        this.texture,
                        this.position,
                        new Rectangle(20, this.currentFrame * (int) MeasureUtility.BLOCK_HEIGHT, (int) MeasureUtility.BLOCK_WIDTH, (int) MeasureUtility.BLOCK_HEIGHT),
                        Color.White
                    );
                break;

                case MovementDirection.RIGHT:
                    spriteBatch.Draw(
                        this.texture,
                        this.position,
                        new Rectangle(0, this.currentFrame * (int) MeasureUtility.BLOCK_HEIGHT, (int) MeasureUtility.BLOCK_WIDTH, (int) MeasureUtility.BLOCK_HEIGHT),
                        Color.White
                    );
                break;

                case MovementDirection.UP:
                    spriteBatch.Draw(
                            this.texture,
                            this.position,
                            new Rectangle(60, this.currentFrame * (int) MeasureUtility.BLOCK_HEIGHT, (int) MeasureUtility.BLOCK_WIDTH, (int) MeasureUtility.BLOCK_HEIGHT),
                            Color.White
                        );
                break;

                case MovementDirection.DOWN:
                    spriteBatch.Draw(
                        this.texture,
                        this.position,
                        new Rectangle(40, this.currentFrame * (int)MeasureUtility.BLOCK_HEIGHT, (int) MeasureUtility.BLOCK_WIDTH, (int) MeasureUtility.BLOCK_HEIGHT),
                        Color.White
                    );
                break;
            }
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Vector3(
                    this.position.X + 1,
                    this.position.Y + 1,
                    0
                ),
                new Vector3(
                    this.position.X + MeasureUtility.BLOCK_WIDTH - 2,
                    this.position.Y + MeasureUtility.BLOCK_HEIGHT - 2,
                    0
                )
            );
        }
    }
}
