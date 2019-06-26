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
    [Command(Name = "ocrabbyy.processfile", Tooltip = "Command `ocrabbyy.processfile` allows to assign project information to a variable in order to extract different types of data from it")]
    public class OcrAbbyyProcessFileCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Path to a file to be processed")]
            public TextStructure Path { get; set; }

            [Argument(Required = false, Tooltip = "List of numbers of pages to be processed")]
            public ListStructure Pages { get; set; } = null;

            [Argument(Tooltip = "Name of a variable (of type AbbyyDocument) where command’s result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(Tooltip = "Info about the number of tables found in the processed file")]
            public TextStructure TablesCountResult { get; set; } = new TextStructure("tablescountresult");

            [Argument(Tooltip = "The language which should be considered trying to recognise text")]
            public TextStructure Language { get; set; } = null;

            [Argument(Tooltip = "Importance of the chosen language")]
            public IntegerStructure LanguageWeight { get; private set; } = new IntegerStructure(100);

            [Argument(Tooltip = "List of possible key words that exist in processed document that will have higher priority than random character strings while OCR processing")]
            public ListStructure Dictionary { get; set; } = null;

            [Argument(Tooltip = "Importance of words in chosen dictionary")]
            public IntegerStructure DictionaryWeight { get; private set; } = new IntegerStructure(100);

 
        }
        public OcrAbbyyProcessFileCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            if ((arguments.Language?.Value == null) && (arguments.Dictionary?.Value == null))
            {
                arguments.Language = new TextStructure("English");
            }

            AbbyyManager manager = AbbyyManager.Instance;

            FineReaderDocument document = manager.CreateDocument(arguments.Path.Value);

            List<int> pageIndices = null;
            if (arguments.Pages != null && arguments.Pages.Value != null)
            {
                pageIndices = new List<int>(arguments.Pages.Value.Count);
                foreach (Structure o in arguments.Pages.Value)
                    pageIndices.Add(((IntegerStructure)o).Value - 1);
            }

            manager.ProcessDocument(document, pageIndices, arguments.Language?.Value, arguments.LanguageWeight.Value, arguments.DictionaryWeight.Value, ListConverter.ExtractDictionary(arguments.Dictionary?.Value));
            var a = document.Tables.Count;
            Scripter.Variables.SetVariableValue(arguments.Result.Value, new IntegerStructure(document.ID));
            Scripter.Variables.SetVariableValue(arguments.TablesCountResult.Value, new IntegerStructure(document.Tables.Count));
        }
    }
}
