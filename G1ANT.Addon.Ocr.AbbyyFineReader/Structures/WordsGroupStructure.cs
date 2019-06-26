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
using System.Collections.Generic;
using System.Linq;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class WordsGroup
    {
        public List<CustomCell> Cells { get; set; } = new List<CustomCell>();

        public int Left { get; set; }

        public int Right { get; set; }

        public bool AddWord(CustomCell cell)
        {
            if (Left == 0 && Right == 0)
            {
                Cells.Add(cell);
                Left = cell.Left;
                Right = cell.Right;
                return true;
            }
            else if ((cell.Left >= Left && cell.Left <= Right) ||
                    (cell.Right <= Right && cell.Right >= Left))
            {
                Cells.Add(cell);
                Left = Math.Min(Left, cell.Left);
                Right = Math.Max(Right, cell.Right);
                return true;
            }
            return false;
        }

        public void SortCellsHorizontally()
        {
            Cells = Cells.OrderBy(x => x.Top).ToList();
        }
    }
}
