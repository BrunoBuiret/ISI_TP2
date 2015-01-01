using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPacman
{
    public abstract class Sprite
    {
        /// <summary>
        /// 
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// Position of the sprite.
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 direction;

        /// <summary>
        /// 
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
        /// Loads graphics resources.
        /// </summary>
        /// <param name="contentManager">Reference to the content manager.</param>
        public abstract void LoadContent(ContentManager contentManager);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// 
        /// </summary>
        public abstract void HandleInput();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
