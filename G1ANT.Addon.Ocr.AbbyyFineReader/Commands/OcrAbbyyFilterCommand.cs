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



using FREngine;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Command(Name = "ocrabbyy.filter", Tooltip = "This command filters text from a document by a font style")]
    public class OcrAbbyyFilterCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = false, Tooltip = "ID of a processed document. If not specified, the last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Required = false, Tooltip = "Flags of filter to apply, separated by array separator (❚): `italic`, `bold`")]
            public TextStructure Filter { get; set; } = null;

            [Argument(Tooltip = "Name of a variable where the command's result (of abbyydocument structure) will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

 
        }
        public OcrAbbyyFilterCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            AbbyyManager manager = AbbyyManager.Instance;

            FineReaderDocument.FilterFlags paramsFilter = FineReaderDocument.FilterFlags.none;

            string[] flagsString = arguments.Filter.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string flag in flagsString)
            {
                FineReaderDocument.FilterFlags currentFlag = FineReaderDocument.FilterFlags.none;
                try
                {
                    currentFlag = (FineReaderDocument.FilterFlags)Enum.Parse(typeof(FineReaderDocument.FilterFlags), flag.Trim(), true);
                    paramsFilter |= (FineReaderDocument.FilterFlags)Enum.Parse(typeof(FineReaderDocument.FilterFlags), flag.Trim(), true);
                }
                catch
                {
                    throw new ArgumentOutOfRangeException(nameof(arguments.Filter), currentFlag, $"{currentFlag} is not defined filter");
                }
            }

            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;

            FineReaderDocument document = manager.GetDocument(docID);

            document.ExtractData();

            ListStructure filteredTexts = new ListStructure(new List<Structure>());
            foreach (string s in document.Filter(paramsFilter))
                filteredTexts.Value.Add(new TextStructure(s));

            Scripter.Variables.SetVariableValue(arguments.Result.Value, filteredTexts);
        }
    }
}
