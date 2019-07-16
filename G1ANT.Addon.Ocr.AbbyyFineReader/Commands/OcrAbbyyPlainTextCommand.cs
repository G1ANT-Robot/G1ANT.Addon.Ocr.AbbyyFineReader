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
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Command(Name = "ocrabbyy.plaintext", Tooltip = "This command extracts text from a processed document")]
    public class OcrAbbyyPlainTextCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "ID of a processed document. If not specified, the last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Tooltip = "Method of text recognition to use: `linebyline` or `structured`")]
            public TextStructure Method { get; set; } = new TextStructure("structured");

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

 
        }
        public OcrAbbyyPlainTextCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            AbbyyManager manager = AbbyyManager.Instance;
            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;
            var doc = manager.GetDocument(docID);
            string output = string.Empty;
            switch (arguments.Method.Value.ToLower())
            {
                case "structured":
                    output = doc.GetAllText();
                    break;

                case "linebyline":
                    output = doc.GetLinesText();
                    break;

                default:
                    throw new ArgumentException("Wrong method argument. It accepts either 'structured' or 'linebyline' value.");
            }
           Scripter.Variables.SetVariableValue(arguments.Result.Value, new TextStructure(output));
        }
    }
}
