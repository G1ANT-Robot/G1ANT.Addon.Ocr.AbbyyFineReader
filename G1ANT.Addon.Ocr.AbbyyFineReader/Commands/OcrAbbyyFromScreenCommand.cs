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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FREngine;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Command(Name = "ocrabbyy.fromscreen", Tooltip = "Command `ocrabbyy.fromscreen` captures part of the screen and recognises text from it")]
    public class OcrAbbyyFromScreenCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Specifies screen area to be captured in format x0⫽y0⫽x1⫽y1 (x0,y0 – coordinates of a top left corner; x1,y1 – coordinates of a right bottom corner of the area)")]
            public RectangleStructure Area { get; set; }

            [Argument(Tooltip = "Runs the command only if condition is true")]
            public BooleanStructure Relative { get; set; } = new BooleanStructure(false);

            [Argument(Tooltip = "The language which should be considered trying to recognize text")]
            public TextStructure Language { get; set; } = new TextStructure("English");

            [Argument(DefaultVariable = "result", Tooltip = "Name of variable (of type AbbyyDocument) where command’s result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

 
        }
        public OcrAbbyyFromScreenCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        private AbbyyManager manager = null;

        public void Execute(Arguments arguments)
        {
            manager = AbbyyManager.Instance;

            System.Drawing.Rectangle rectangle = !arguments.Relative.Value ? arguments.Area.Value : arguments.Area.Value.ToAbsoluteCoordinates(); 
            System.Drawing.Bitmap partOfScreen = RobotWin32.GetPartOfScreen(rectangle);

            IEngine engine = manager.Engine;
            DocumentProcessingParams processingParams = engine.CreateDocumentProcessingParams();
            RecognizerParams recognizingParams = processingParams.PageProcessingParams.RecognizerParams;
            recognizingParams.SetPredefinedTextLanguage(arguments.Language.Value);
            engine.LoadPredefinedProfile(AbbyyManager.TextAccuracyProfile);
            FRDocument imageDocument = engine.CreateFRDocument();

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                partOfScreen.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                IReadStream imageStream = new StreamNet2AbbyyAdapter(stream);
                imageDocument.AddImageFileFromStream(imageStream);
            }

            imageDocument.Process(processingParams);


            Scripter.Variables.SetVariableValue(arguments.Result.Value, new TextStructure(imageDocument.PlainText.Text));
        }
    }
}
