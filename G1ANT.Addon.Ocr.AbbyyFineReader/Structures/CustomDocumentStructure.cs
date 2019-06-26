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
using System.Xml;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class CustomDocument
    {
        public CustomDocument(XmlDocument xmlDocument)
        {
            Initialize(xmlDocument);
        }

        public List<CustomPage> Pages { get; set; } = new List<CustomPage>();

        public void Initialize(XmlDocument xmlDocument)
        {
            XmlNamespaceManager nameSpacemanager = new XmlNamespaceManager(xmlDocument.NameTable);
            nameSpacemanager.AddNamespace("ns", "http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml");
            List<XmlElement> pages = xmlDocument.GetElementsByTagName("page").Cast<XmlElement>().ToList();
            List<CustomRow> lines = new List<CustomRow>();
            int pageIndex = 0;
            foreach (XmlElement xmlPage in pages)
            {
                List<CustomCell> cellsNotInRows = xmlPage.SelectNodes("//ns:line[not(ancestor::ns:row)]", nameSpacemanager).Cast<XmlElement>().
                Select(x => new CustomCell
                {
                    Top = int.Parse(x.Attributes["t"].Value),
                    Bottom = int.Parse(x.Attributes["b"].Value),
                    Right = int.Parse(x.Attributes["r"].Value),
                    Left = int.Parse(x.Attributes["l"].Value),
                    BaseLine = int.Parse(x.Attributes["baseline"].Value),
                    Text = (x as XmlElement).InnerText,
                }).ToList();

                List<XmlElement> rows = xmlPage.GetElementsByTagName("row").Cast<XmlElement>().ToList();

                List<CustomRow> currentPageLines = new List<CustomRow>();
                currentPageLines.AddRange(GetLinesFromWords(cellsNotInRows));
                currentPageLines.AddRange(GetLinesFromWordsInRows(rows, pageIndex));
                // sorting lines vertically
                currentPageLines = currentPageLines.OrderBy(x => x.Top).ToList();
                CustomPage page = new CustomPage()
                {
                    Index = pageIndex,
                    Rows = currentPageLines
                };
                Pages.Add(page);
                pageIndex++;
            }
        }

        private List<CustomRow> GetLinesFromWords(List<CustomCell> cellsNotInRows)
        {
            cellsNotInRows = cellsNotInRows.ToArray().ToList();
            List<CustomRow> lines = new List<CustomRow>();

            while (cellsNotInRows.Any())
            {
                CustomCell currentCell = cellsNotInRows.First();
                System.Drawing.Rectangle currentLineBoundaries = System.Drawing.Rectangle.Empty;
                // finding highest element in current line
                foreach (CustomCell cell in cellsNotInRows)
                {
                    System.Drawing.Rectangle boundaries = currentCell.GetUnionBoundaries(cell);
                    if (boundaries != System.Drawing.Rectangle.Empty &&
                        boundaries.Bottom - boundaries.Top > currentLineBoundaries.Bottom - currentLineBoundaries.Top)
                    {
                        currentLineBoundaries = boundaries;
                    }
                }
                // creating new line
                CustomRow line = new CustomRow()
                {
                    Top = currentLineBoundaries.Top,
                    Bottom = currentLineBoundaries.Bottom
                };
                lines.Add(line);
                foreach (CustomCell cell in cellsNotInRows)
                {
                    if (cell.IsInsideVerticalBoundaries(currentLineBoundaries))
                    {
                        line.Cells.Add(cell);
                    }
                }
                // removing words from words collection
                foreach (CustomCell cell in line.Cells)
                {
                    cellsNotInRows.Remove(cell);
                }
            }

            // sorting words in line (by both x and y axis)
            foreach (CustomRow line in lines)
            {
                List<CustomCell> cellsTmp = line.Cells.ToList();
                while (cellsTmp.Any())
                {
                    WordsGroup wordsGroup = new WordsGroup();
                    CustomCell cell = cellsTmp.First();
                    wordsGroup.AddWord(cell);
                    cellsTmp.Remove(cell);
                    bool addedWord = false;
                    while (addedWord)
                    {
                        foreach (CustomCell tmpCell in cellsTmp)
                        {
                            addedWord = wordsGroup.AddWord(tmpCell) ? true : addedWord;
                            cellsTmp.Remove(tmpCell);
                            if (addedWord)
                            {
                                break;
                            }
                        }
                    }
                    line.CellsGroups.Add(wordsGroup);
                }
                line.SortCells();
            }

            return lines;
        }

        private List<CustomRow> GetLinesFromWordsInRows(List<XmlElement> rows, int pageIndex)
        {
            List<CustomRow> lines = new List<CustomRow>();
            foreach (XmlElement row in rows)
            {
                CustomRow line = new CustomRow()
                {
                    IsTableRow = true,
                    Top = -1,
                    Bottom = -1
                };
                foreach (XmlElement xmlCell in row.GetElementsByTagName("cell"))
                {
                    List<CustomCell> cells = xmlCell.GetElementsByTagName("line").Cast<XmlElement>().
                        Select(x => new CustomCell
                        {
                            Top = int.Parse(x.Attributes["t"].Value),
                            Bottom = int.Parse(x.Attributes["b"].Value),
                            Right = int.Parse(x.Attributes["r"].Value),
                            Left = int.Parse(x.Attributes["l"].Value),
                            BaseLine = int.Parse(x.Attributes["baseline"].Value),
                            Text = (x as XmlElement).InnerText,
                        }).ToList();

                    if (cells.Any())
                    {
                        cells = cells.OrderBy(x => x.Top).ToList();
                        int cellTop = cells.Min(x => x.Top);
                        int cellBottom = cells.Max(x => x.Bottom);

                        line.Top = (line.Top != -1) ? Math.Min(line.Top, cellTop) : cellTop;
                        line.Bottom = (line.Bottom != -1) ? Math.Max(line.Bottom, cellBottom) : cellBottom;

                        CustomCell newCellWord = new CustomCell()
                        {
                            Top = cellTop,
                            Bottom = cellBottom,
                            Left = cells.Min(x => x.Left),
                            Right = cells.Max(x => x.Right),
                            Text = string.Empty
                        };
                        foreach (CustomCell cell in cells)
                        {
                            newCellWord.Text += (cell != cells.Last()) ? $"{cell.Text} " : cell.Text;
                        }
                        line.Cells.Add(newCellWord);
                    }
                }
                if (line.Cells.Any())
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

        public string GetText()
        {
            List<CustomRow> lines = new List<CustomRow>();
            foreach (CustomPage page in Pages)
            {
                foreach (CustomRow row in page.Rows)
                {
                    lines.Add(row);
                }
            }
            string output = string.Empty;
            foreach (CustomRow line in lines)
            {
                output += line.ToString();
                output += (line != lines.Last()) ? System.Environment.NewLine : string.Empty;
            }
            return output;
        }
    }
}
