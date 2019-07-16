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
    [Command(Name = "ocrabbyy.processscreen", Tooltip = "This command processes a part of the screen for further data extraction")]
    public class OcrAbbyyProcessScreenCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "Area of the screen to be processed specified in rectangle format, eg. `2⫽4⫽12⫽40`")]
            public RectangleStructure Area { get; set; } = new RectangleStructure(System.Windows.Forms.Screen.PrimaryScreen.Bounds);

            [Argument(Tooltip = "If set to true, the area coordinates are relative to the active window")]
            public BooleanStructure Relative { get; set; } = new BooleanStructure(false);

            [Argument(Tooltip = "Name of a variable where the command's result (document ID) will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(Tooltip = "Language which should be considered during text recognition")]
            public TextStructure Language { get; set; } = null;

            [Argument(Tooltip = "Importance of the chosen language")]
            public IntegerStructure LanguageWeight { get; private set; } = new IntegerStructure(100);

            [Argument(Tooltip = "List of possible keywords existing in the processed document that will have higher priority than random character strings while OCR processing")]
            public ListStructure Dictionary { get; set; } = null;

            [Argument(Tooltip = "Importance of words in chosen dictionary")]
            public IntegerStructure DictionaryWeight { get; private set; } = new IntegerStructure(100);
        }
        public OcrAbbyyProcessScreenCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            if ((arguments.Language?.Value == null) && (arguments.Dictionary?.Value == null))
            {
                arguments.Language = new TextStructure("English");
            }

            Rectangle rectangle = !arguments.Relative.Value ? arguments.Area.Value : arguments.Area.Value.ToAbsoluteCoordinates();
            Bitmap partOfScreen = RobotWin32.GetPartOfScreen(rectangle);

            AbbyyManager manager = AbbyyManager.Instance;
            FineReaderDocument imageDocument = null;

            using (MemoryStream stream = new MemoryStream())
            {
                partOfScreen.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                imageDocument = manager.CreateDocument(stream);
            }

            manager.ProcessDocument(imageDocument, null, arguments.Language?.Value, arguments.LanguageWeight.Value, arguments.DictionaryWeight.Value, ListConverter.ExtractDictionary(arguments.Dictionary?.Value));

            Scripter.Variables.SetVariableValue(arguments.Result.Value, new IntegerStructure(imageDocument.ID));
        }
    }
}
