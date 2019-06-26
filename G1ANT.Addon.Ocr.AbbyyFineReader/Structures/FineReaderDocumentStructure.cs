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
using FREngine;
using System.Drawing;

using System.Text.RegularExpressions;
using System.Xml;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class FineReaderDocument
    {
        public enum FilterFlags
        {
            none = 0,
            bold = 0b11,
            notBold = 0b10,
            italic = 0b1100,
            notItalic = 0b1000
        }

        private FRDocument document;
        private readonly int id;

        internal FineReaderDocument(FRDocument document, int id)
        {
            this.document = document;
            this.id = id;
        }

        internal FRDocument Document => document;
        public int ID => id;

        private FineReaderTables tables = null;
        private List<FRPage> frPages = new List<FRPage>();
        private List<Block> blocks = new List<Block>();
        private List<Word> rows = new List<Word>();
        private List<FineReaderParagraph> paragraphs = new List<FineReaderParagraph>();
        private List<FineReaderCell> cells = null;
        private List<FineReaderPage> pages = null;

        public CustomDocument CustomDocument { get; set; }

        public string GetAllText()
        {
            return document.PlainText.Text;
        }

        public List<Rectangle> FindPosition(string phrase)
        {
            List<Rectangle> rectangles = new List<Rectangle>();

            if (string.IsNullOrEmpty(phrase) == false)
            {
                Regex regex = new Regex(phrase);
                foreach (Match match in regex.Matches(document.PlainText.Text))
                {
                    int startIndex = match.Index;
                    int matchLength = match.Length;

                    rectangles.Add(new Rectangle(
                        document.PlainText.Left[startIndex],
                        document.PlainText.Top[startIndex],
                        document.PlainText.Right[startIndex + matchLength - 1] - document.PlainText.Left[startIndex],
                        document.PlainText.Bottom[startIndex + matchLength - 1] - document.PlainText.Top[startIndex]));
                }
            }

            return rectangles;
        }

        public FineReaderTables Tables
        {
            get
            {
                if (tables == null)
                    tables = new FineReaderTables(document);
                return tables;
            }
        }

        public List<FineReaderCell> Cells
        {
            get
            {
                if (cells == null)
                    cells = InitCells();
                return cells;
            }
        }

        private List<FineReaderCell> InitCells()
        {
            List<FineReaderCell> cells = new List<FineReaderCell>();
            foreach (FineReaderTable t in Tables)
                foreach (FineReaderCell c in t)
                    cells.Add(c);
            return cells;
        }

        public List<FineReaderParagraph> Paragraphs { get { return paragraphs; } }

        public void ExtractData()
        {
            ReadDocument();
        }

        public List<string> Filter(FilterFlags paramsFilter)
        {
            StyleParamsEnum styleMask = ToStyleMask(paramsFilter);

            List<string> filteredTexts = new List<string>();
            CharParams charParams = AbbyyManager.Instance.Engine.CreateCharParams();

            foreach (FineReaderParagraph p in paragraphs)
            {
                int position = 0;

                while (position < p.paragraph.Length)
                {
                    p.paragraph.GetCharParams(position, charParams);
                    int newPosition = p.paragraph.NextGroup(position, 0, (int)styleMask);

                    if (DoStyleComply(charParams, paramsFilter))
                    {
                        int length = newPosition - position;
                        length += newPosition >= p.paragraph.Length ? -1 : 0;
                        filteredTexts.Add(p.Text.Substring(position, length));
                    }

                    position = newPosition;
                }
            }

            return filteredTexts;
        }

        public void ExportToXml(string fileName)
        {
            IXMLExportParams parameters = document.Application.CreateXMLExportParams();
            parameters.WriteNondeskewedCoordinates = false;
            document.Export(fileName, FileExportFormatEnum.FEF_XML, parameters);
        }

        public IList<System.Drawing.Image> GetPagesImages()
        {
            List<System.Drawing.Image> pagesBitmaps = new List<System.Drawing.Image>();
            foreach (FRPage page in document.Pages)
            {
                IHandle hBitmap = page.ImageDocument.GrayImage.GetBitmap(null);
                pagesBitmaps.Add(System.Drawing.Image.FromHbitmap(hBitmap.Handle));
            }
            return pagesBitmaps;
        }

        public void InitializeCustomDocument()
        {
            CustomDocument = new CustomDocument(GetXml());
        }

        public XmlDocument GetXml()
        {
            XmlMemoryWriter memoryWriter = new XmlMemoryWriter();

            IXMLExportParams parameters = document.Application.CreateXMLExportParams();
            parameters.WriteNondeskewedCoordinates = false;
            document.ExportToMemory(memoryWriter, FileExportFormatEnum.FEF_XML, parameters);
            XmlDocument xmlDocument = memoryWriter.GetDocument();
            return xmlDocument;
        }

        public string GetLinesText()
        {
            return CustomDocument.GetText();
        }

        private void ReadDocument()
        {
            foreach (FRPage p in document.Pages)
            {
                frPages.Add(p);
                ReadPage(p);
            }
        }

        private void ReadPage(FRPage page)
        {
            foreach (Block b in page.Layout.Blocks)
            {
                blocks.Add(b);
                ReadBlock(b);
            }
        }

        private void ReadBlock(IBlock block)
        {
            switch (block.Type)
            {
                case BlockTypeEnum.BT_Text:
                    ReadTextBlock(block.GetAsTextBlock());
                    break;
                case BlockTypeEnum.BT_RasterPicture:
                    break;
                case BlockTypeEnum.BT_Table:
                    break;
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
        }

        private void ReadTextBlock(TextBlock textBlock)
        {
            foreach (Paragraph p in textBlock.Text.Paragraphs)
            {
                paragraphs.Add(new FineReaderParagraph(p));
                foreach (Word w in p.Words)
                {
                    rows.Add(w);
                }
            }
        }

        private StyleParamsEnum ToStyleMask(FilterFlags paramsFilter)
        {
            StyleParamsEnum mask = (StyleParamsEnum)0;

            if ((paramsFilter & FilterFlags.notBold) == FilterFlags.notBold)
                mask |= StyleParamsEnum.SF_Bold;
            if ((paramsFilter & FilterFlags.notItalic) == FilterFlags.notItalic)
                mask |= StyleParamsEnum.SF_Italic;

            return mask;
        }

        private bool DoStyleComply(CharParams charParams, FilterFlags filter)
        {
            if ((filter & FilterFlags.notBold) == FilterFlags.notBold)
            {
                if ((filter & FilterFlags.bold) == FilterFlags.bold)
                {
                    if (!charParams.IsBold)
                        return false;
                }
                else
                {
                    if (charParams.IsBold)
                        return false;
                }
            }
            if ((filter & FilterFlags.notItalic) == FilterFlags.notItalic)
            {
                if ((filter & FilterFlags.italic) == FilterFlags.italic)
                {
                    if (!charParams.IsItalic)
                        return false;
                }
                else
                {
                    if (charParams.IsItalic)
                        return false;
                }
            }

            return true;
        }

        public void Close()
        {
            document.Close();
            document = null;
        }

    }
}