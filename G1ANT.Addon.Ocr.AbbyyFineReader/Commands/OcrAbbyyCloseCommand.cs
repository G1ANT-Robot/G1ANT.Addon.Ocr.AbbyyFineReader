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


namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Command(Name = "ocrabbyy.close", Tooltip = "Command `ocrabbyy.close` allows to close all documents processed by abbyy engine")]
    public class OcrAbbyyCloseCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "Id of a document to be closed, if not specified, G1ANT.Robot closes all documents and unloads abbyy engine")]
            public IntegerStructure Document { get; set; }
        }
        public OcrAbbyyCloseCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            if (arguments.Document != null)
            {
                AbbyyManager.Instance.GetDocument(arguments.Document.Value).Close();
            }
            else
            {
                AbbyyManager.Instance.Dispose();
            }
        }
    }
}
