using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
