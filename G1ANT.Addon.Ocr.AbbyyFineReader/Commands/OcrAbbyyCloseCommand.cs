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
    [Command(Name = "ocrabbyy.close", Tooltip = "This command closes all documents processed by ABBYY engine")]
    public class OcrAbbyyCloseCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "ID of a document to be closed. If not specified, all documents will be closed and ABBYY engine unloaded")]
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
