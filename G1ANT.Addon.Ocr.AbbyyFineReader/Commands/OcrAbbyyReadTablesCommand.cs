/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr.AbbyyFineReader
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FREngine;
using System.Text.RegularExpressions;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Command(Name = "ocrabbyy.readtables", Tooltip = "This command reads the content of all tables existing in a document and returns it as a list")]
    public class OcrAbbyyReadTablesCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = false, Tooltip = "ID of a processed document. If not specified, the last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Tooltip = "Name of a variable where the command's result (a list of all tables’ elements) will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }
        public OcrAbbyyReadTablesCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            AbbyyManager manager = AbbyyManager.Instance;

            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;

            FineReaderDocument document = manager.GetDocument(docID);
            document.ExtractData();

            ListStructure cellsText = new ListStructure(new List<Structure>());
            foreach (FineReaderCell c in document.Cells)
                cellsText.Value.Add(new TextStructure(c.Text));

            Scripter.Variables.SetVariableValue(arguments.Result.Value, cellsText);
        }
    }
}
