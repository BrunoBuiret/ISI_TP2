﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPacman
{
    class Blinky : Ghost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentManager"></param>
        public override void LoadContent(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(@"images\blinky");
        }
    }
}