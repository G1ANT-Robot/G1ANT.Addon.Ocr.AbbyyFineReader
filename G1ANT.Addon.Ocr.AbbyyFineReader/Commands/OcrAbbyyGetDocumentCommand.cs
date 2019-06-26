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
    [Command(Name = "ocrabbyy.getdocument", Tooltip = "Command `ocrabbyy.getdocument` allows to assign project information to a variable in order to extract different types of data from it")]
    public class OcrAbbyyGetDocumentCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "Id of a processed document returned by a call to `processfile` command. If not specified, last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Tooltip = "Name of variable (of type AbbyyDocument) where command’s result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

 
        }
        public OcrAbbyyGetDocumentCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            AbbyyManager manager = AbbyyManager.Instance;
            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;
            var doc = manager.GetDocument(docID); 
            Scripter.Variables.SetVariableValue(arguments.Result.Value, new AbbyyDocumentStructure(doc.CustomDocument));
        }
    }
}
