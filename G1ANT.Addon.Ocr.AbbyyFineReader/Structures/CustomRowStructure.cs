/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr.AbbyyFineReader
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System.Collections.Generic;
using System.Linq;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class CustomRow
    {
        public List<CustomCell> Cells { get; set; } = new List<CustomCell>();

        public List<WordsGroup> CellsGroups { get; set; } = new List<WordsGroup>();

        public int Top { get; set; }

        public int Bottom { get; set; }

        public bool IsTableRow { get; set; }

        public void SortCells()
        {
            CellsGroups = CellsGroups.OrderBy(x => x.Left).ToList();
            foreach (WordsGroup cellsGroup in CellsGroups)
            {
                cellsGroup.SortCellsHorizontally();
            }
            List<CustomCell> sortedCellList = new List<CustomCell>();
            foreach (WordsGroup cellsGroup in CellsGroups)
            {
                foreach (CustomCell cell in cellsGroup.Cells)
                {
                    sortedCellList.Add(cell);
                }
            }
            Cells = sortedCellList;
        }

        public override string ToString()
        {
            string output = string.Empty;
            foreach (CustomCell cell in Cells)
            {
                output += $"{cell.ToString()} ";
            }
            return output;
        }
    }
}
