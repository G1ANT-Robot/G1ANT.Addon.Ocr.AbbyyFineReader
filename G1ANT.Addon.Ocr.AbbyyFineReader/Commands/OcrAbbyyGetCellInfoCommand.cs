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
    [Command(Name = "ocrabbyy.getcellinfo", Tooltip = "Command `ocrabbyy.fromscreen` allows to retrive information about table cell")]
    public class OcrAbbyyGetCellInfoCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = false, Tooltip = "Id of a processed document returned by a call to processfile command. If not specified last processed document is used")]
            public IntegerStructure DocumentID { get; set; } = null;

            [Argument(Required = false, Tooltip = "Index of a table in document")]
            public IntegerStructure TableIndex { get; set; } = new IntegerStructure(1);

            [Argument(Required = true, Tooltip = "Position of the cell in the table in format row, column")]
            public TextStructure Position { get; set; } = null;

            [Argument(Tooltip = "Name of variable (of type AbbyyDocument) where command’s result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

 
        }
        public OcrAbbyyGetCellInfoCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public void Execute(Arguments arguments)
        {
            AbbyyManager manager = AbbyyManager.Instance;
            int docID = arguments.DocumentID == null ? manager.CurentDocumentCount : arguments.DocumentID.Value;
            FineReaderDocument document = manager.GetDocument(docID);
            int row = 0;
            int column = 0;

            string[] position = arguments.Position.Value.Split(',');

            row = int.Parse(position[0]);
            column = int.Parse(position[1]);

            System.Drawing.Point cellSpans = new System.Drawing.Point(
                document.Tables[arguments.TableIndex.Value - 1][row - 1, column - 1].RowSpan,
                document.Tables[arguments.TableIndex.Value - 1][row - 1, column - 1].ColumnSpan);

            Scripter.Variables.SetVariableValue(arguments.Result.Value, new PointStructure(cellSpans));
        }
    }
}
