using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MyPacman
{
    public static class MeasureUtility
    {
        /// <summary>
        /// Holds the width of a block.
        /// </summary>
        public const float BLOCK_WIDTH = 20f;

        /// <summary>
        /// Holds the height of a block.
        /// </summary>
        public const float BLOCK_HEIGHT = 20f;

        /// <summary>
        /// Converts a block abscissa to an actual abscissa.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float blockXToActualX(float x)
        {
            return x * MeasureUtility.BLOCK_WIDTH;
        }

        /// <summary>
        /// Converts a block ordinate to an actual ordinate.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float blockYToActualY(float y)
        {
            return 100 + y * MeasureUtility.BLOCK_HEIGHT;
        }

        /// <summary>
        /// Converts a Vector2 with block coordinates to a Vector2 with actual coordinates.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 blockVector2ToActualVector2(Vector2 v)
        {
            v.X = MeasureUtility.blockXToActualX(v.X);
            v.Y = MeasureUtility.blockYToActualY(v.Y);
            return v;
        }
    }
}
