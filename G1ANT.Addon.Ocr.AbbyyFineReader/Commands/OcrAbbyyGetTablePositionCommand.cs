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
    [Command(Name = "ocrabbyy.gettableposition", Tooltip = "This command finds text in a specified table of a document and returns a list of indexes")]
    public class OcrAbbyyGetTablePositionCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Text to be found in a table")]
            public TextStructure Search { get; set; }

            [Argument(Required = false, Tooltip = "ID of a processed document. If not specified, the last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Required = true, Tooltip = "Index of a table in a document")]
            public IntegerStructure TableIndex { get; set; } = null;

            [Argument(Tooltip = "Name of a variable where the command's result (a list of indexes) will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(DefaultVariable = "timeoutOcr", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; }
        }
        public OcrAbbyyGetTablePositionCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        private AbbyyManager manager = null;

        public void Execute(Arguments arguments)
        {
            manager = AbbyyManager.Instance;
            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;
            var doc = manager.GetDocument(docID);
            List<string> table = doc.Tables[arguments.TableIndex.Value].ReturnWordPosition(arguments.Search.Value);
            ListStructure cells = new ListStructure(new List<Structure>());
            foreach (String t in table)
                cells.Value.Add(new TextStructure(t));
            Scripter.Variables.SetVariableValue(arguments.Result.Value, cells);
        }
    }
}
