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
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FREngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class FineReaderTable : IReadOnlyList<FineReaderCell>
    {
        private readonly ITableBlock tableBlock;
        private List<FineReaderCell> cells = null;
        private int[,] indexMap;
        private int rowCount;
        private int columnCount;

        internal FineReaderTable(ITableBlock tableBlock)
        {
            this.tableBlock = tableBlock;
        }

        public FineReaderCell this[int index]
        {
            get
            {
                if (cells == null)
                    InitCells();
                return cells[index];
            }
        }

        public FineReaderCell this[int row, int column]
        {
            get
            {
                if (cells == null)
                    InitCells();
                return cells[indexMap[row, column]];
            }
        }

        public int Count
        {
            get
            {
                if (cells == null)
                    InitCells();
                return cells.Count;
            }
        }

        public IEnumerator<FineReaderCell> GetEnumerator()
        {
            if (cells == null)
                InitCells();
            return (IEnumerator<FineReaderCell>)cells.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (cells == null)
                InitCells();
            return cells.GetEnumerator();
        }

        private void InitCells()
        {
            rowCount = tableBlock.HSeparators.Count - 1;
            columnCount = tableBlock.VSeparators.Count - 1;
            indexMap = new int[rowCount, columnCount];
            cells = new List<FineReaderCell>(tableBlock.Cells.Count);

            int processedIndex = -1;

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                {
                    int currentIndex = tableBlock.Cells.IndexOf(j, i);

                    if (currentIndex > processedIndex)
                    {
                        processedIndex = currentIndex;
                        cells.Add(new FineReaderCell(tableBlock.Cells[currentIndex], i, j));
                        indexMap[i, j] = currentIndex;
                    }
                    else
                    {
                        indexMap[i, j] = currentIndex;

                        if (cells[currentIndex].RowSpan < (i - cells[currentIndex].Row + 1))
                            cells[currentIndex].RowSpan = i - cells[currentIndex].Row + 1;

                        if (cells[currentIndex].ColumnSpan < (j - cells[currentIndex].Column + 1))
                            cells[currentIndex].ColumnSpan = j - cells[currentIndex].Column + 1;
                    }
                }
        }

        public List<string> ReturnWordPosition(string phrase)
        {
            List<string> result = new List<string>();
            InitCells();
            if (!string.IsNullOrEmpty(phrase))
            {
                Regex regex = new Regex(phrase);

                foreach (var c in cells)
                {
                    Match match = regex.Match(c.Text);
                    if (match.Success)
                    {
                        result.Add($"{c.Row + 1},{c.Column + 1}");
                    }
                }
            }

            return result;
        }
    }
}
