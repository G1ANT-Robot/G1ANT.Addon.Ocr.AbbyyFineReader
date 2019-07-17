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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FREngine;
using System.Text.RegularExpressions;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Command(Name = "ocrabbyy.readcell", Tooltip = "This command reads a specified cell from a given table in the document")]
    public class OcrAbbyyReadCellCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = false, Tooltip = "ID of a processed document. If not specified, the last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Required = true, Tooltip = "Index of a table in a document")]
            public IntegerStructure TableIndex { get; set; } = null;

            //[Argument(Required = true, Tooltip = "Index of a row in the table")]
            //public IntegerStructure Row { get; set; } = null;

            //[Argument(Required = true, Tooltip = "Index of a column in the table")]
            //public IntegerStructure Column { get; set; } = null;

            [Argument(Required = true, Tooltip = "Position of the cell in the table in `X,Y` format, where `X` means row number and `Y` — column index (number)")]
            public TextStructure Position { get; set; } = null;

            [Argument(Tooltip = "Offset to be added to get a proper value in `X,Y` format")]
            public TextStructure Offset { get; set; } = null;

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }
        public OcrAbbyyReadCellCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            AbbyyManager manager = AbbyyManager.Instance;
            int docID = arguments.DocumentID?.Value ?? manager.CurentDocumentCount;
            FineReaderDocument document = manager.GetDocument(docID);
            int row = 0;
            int column = 0;
            int rowOffset = 0;
            int columnOffset = 0;

            var position = arguments.Position.Value.Split(',');

            row = int.Parse(position[0]);
            column = int.Parse(position[1]);

            if (!string.IsNullOrEmpty(arguments.Offset?.Value))
            {
                var positionOffset = arguments.Offset.Value.Split(',');
                rowOffset = int.Parse(positionOffset[0]);
                columnOffset = int.Parse(positionOffset[1]);
            }

            string cellTextValue = string.Empty;
            try
            {
                cellTextValue = document.Tables[arguments.TableIndex.Value - 1][row - 1 + rowOffset, column - 1 + columnOffset].Text ?? string.Empty;
            }
            catch
            {
            }

            TextStructure cellText = new TextStructure(cellTextValue);

            Scripter.Variables.SetVariableValue(arguments.Result.Value, cellText);
        }
    }
}
