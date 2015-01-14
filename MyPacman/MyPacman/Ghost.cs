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
        /// 
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
            throw new NotImplementedException();
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
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(this.texture != null)
            {
                spriteBatch.Draw(this.texture, this.position, Color.White);
            }
        }
    }
}
