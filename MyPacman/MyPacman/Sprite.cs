using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPacman
{
    abstract class Sprite
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
        /// Initializes the sprite.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Loads graphics resources.
        /// </summary>
        /// <param name="contentManager">Reference to the content manager.</param>
        /// <param name="assetName">Name of the asset to load for the sprite.</param>
        public virtual void LoadContent(ContentManager contentManager, String assetName)
        {
            this.texture = contentManager.Load<Texture2D>(assetName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// 
        /// </summary>
        public abstract void HandleInput();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.texture, this.position, Color.White);
        }
    }
}
