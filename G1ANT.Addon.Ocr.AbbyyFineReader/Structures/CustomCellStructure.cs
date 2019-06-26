/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr.AbbyyFineReader
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class CustomCell
    {
        public int Top { get; set; }

        public int Bottom { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public int BaseLine { get; set; }

        public string Text { get; set; }

        public System.Drawing.Rectangle GetUnionBoundaries(CustomCell cell)
        {
            return DoesWordCrossWord(cell) ? System.Drawing.Rectangle.FromLTRB(Math.Min(cell.Left, Left), Math.Min(cell.Top, Top), Math.Max(cell.Right, Right), Math.Max(cell.Bottom, Bottom)) : System.Drawing.Rectangle.Empty;
        }

        public bool DoesWordCrossWord(CustomCell cell)
        {
            return ((this.Top >= cell.Top && this.Top <= cell.Bottom) ||
                    (cell.Top >= this.Top && cell.Top <= this.Bottom) ||
                    (this.Bottom <= cell.Bottom && this.Bottom >= cell.Top) ||
                    (cell.Bottom <= this.Bottom && cell.Bottom >= this.Top));
        }

        public bool IsInsideVerticalBoundaries(System.Drawing.Rectangle boundaries)
        {
            return Top >= boundaries.Top && Bottom <= boundaries.Bottom;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
