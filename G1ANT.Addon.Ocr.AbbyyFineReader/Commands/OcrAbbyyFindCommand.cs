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
    [Command(Name = "ocrabbyy.find", Tooltip = "Command `ocrabbyy.find` allows to find the text on the active screen and return its position as a 'rectangle' format")]
    public class OcrAbbyyFindCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Text that you want to find on the screen")]
            public TextStructure Search { get; set; }

            [Argument(Required = false, Tooltip = "Id of a processed document returned by a call to processfile command. If not specified last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Tooltip = "Name of variable (of type AbbyyDocument) where command’s result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

 
        }
        public OcrAbbyyFindCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        private AbbyyManager manager = null;

        public void Execute(Arguments arguments)
        {
            manager = AbbyyManager.Instance;
            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;
            var doc = manager.GetDocument(docID);

            List<Rectangle> rectangles = doc.FindPosition(arguments.Search.Value);

            ListStructure matchesRectangles = new ListStructure(new List<Structure>());

            foreach (Rectangle r in rectangles)
                matchesRectangles.Value.Add(new RectangleStructure(r));

            Scripter.Variables.SetVariableValue(arguments.Result.Value, matchesRectangles);
        }
    }
}
