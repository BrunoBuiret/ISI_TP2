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
    public abstract class Sprite
    {
        /// <summary>
        /// Sprite's texture, or current texture if there are several.
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// Holds the sprite's position.
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// Holds the sprite's direction if it is moving.
        /// </summary>
        protected Vector2 direction;

        /// <summary>
        /// Holds the speed of the sprite if it is moving.
        /// </summary>
        protected float speed;

        /// <summary>
        /// Gets or sets the texture of the sprite.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                this.texture = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the sprite.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// Gets or sets the direction of the sprite.
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = Vector2.Normalize(value);
            }
        }

        /// <summary>
        /// Gets or sets the speed of the sprite.
        /// </summary>
        public float Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                this.speed = value;
            }
        }

        /// <summary>
        /// Creates a new blank sprite.
        /// </summary>
        public Sprite()
        {
            this.texture = null;
            this.position = Vector2.Zero;
            this.direction = Vector2.Zero;
            this.speed = 0f;
        }

        /// <summary>
        /// Initializes the sprite.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Loads graphic resources.
        /// </summary>
        /// <param name="contentManager">Reference to the content manager.</param>
        public abstract void LoadContent(ContentManager contentManager);

        /// <summary>
        /// Unloads any non-graphic resources.
        /// </summary>
        public abstract void UnloadContent();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">Reference to the current keyboard state.</param>
        public abstract void HandleKeyboard(KeyboardState state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">Reference to the sprite batch.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
