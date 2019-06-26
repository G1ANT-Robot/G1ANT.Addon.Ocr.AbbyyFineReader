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

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class FineReaderCell
    {
        private TableCell cell;

        public FineReaderCell(TableCell cell, int row, int column)
        {
            this.cell = cell;
            Row = row;
            Column = column;
        }

        public string Text => GetTextFromBlock(cell.Block);

        public int Row { get; private set; }
        public int Column { get; private set; }

        public int ColumnSpan { get; internal set; } = 1;
        public int RowSpan { get; internal set; } = 1;

        private string GetTextFromParagraphs(IParagraphs paragraphs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Paragraph p in paragraphs)
                sb.Append($"{p.Text}{Environment.NewLine}");

            return sb.ToString();
        }

        private string GetTextFromBlock(IBlock block)
        {
            switch (block.Type)
            {
                case BlockTypeEnum.BT_Text:
                    return GetTextFromParagraphs(block.GetAsTextBlock().Text.Paragraphs);
                case BlockTypeEnum.BT_RasterPicture:
                    break;
                case BlockTypeEnum.BT_Table:
                    return GetTextFromTable(block.GetAsTableBlock());
                case BlockTypeEnum.BT_Barcode:
                    break;
                case BlockTypeEnum.BT_Checkmark:
                    break;
                case BlockTypeEnum.BT_CheckmarkGroup:
                    break;
                case BlockTypeEnum.BT_VectorPicture:
                    break;
                case BlockTypeEnum.BT_Separator:
                    break;
                case BlockTypeEnum.BT_SeparatorGroup:
                    break;
                case BlockTypeEnum.BT_AutoAnalysis:
                    break;
                default:
                    break;
            }
            return string.Empty;
        }

        private string GetTextFromTable(TableBlock tableBlock)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableCell c in tableBlock.Cells)
                sb.Append(GetTextFromBlock(c.Block));

            return sb.ToString();
        }
    }
}
