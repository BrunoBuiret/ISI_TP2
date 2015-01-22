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
        /// <summary>
        /// Initializes pacman.
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads graphic contents for pacman.
        /// </summary>
        /// <param name="contentManager">Reference to the content manager.</param>
        public override void LoadContent(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(@"images\pacman");
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
            if(this.direction != Vector2.Zero)
            {
                this.position += this.direction * this.speed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
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
                this.direction = new Vector2(-1, 0);//
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                this.direction = new Vector2(0, -1);//
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                this.direction = new Vector2(1, 0);//
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                this.direction = new Vector2(0, 1);//
            }
            else
            {
                this.direction = Vector2.Zero;
            }
        }

        public BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(
                new Vector3(
                    this.position.X + MeasureUtility.BLOCK_WIDTH / 2,
                    this.position.Y + MeasureUtility.BLOCK_HEIGHT / 2,
                    0
                ),
                10
            );
        }
    }
}
