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
    [Structure(Name = "abbyyrow", Tooltip = "This structure stores information about a row on a page and has five fields")]

    public class AbbyyRowStructure : StructureTyped<CustomRow>
    {
        private const string FirstIndex = "cells";
        private const string LastIndex = "count";
        private const string CountIndex = "top";
        private const string NewIndex = "bottom";
        private const string EmptyIndex = "istablerow";

        public AbbyyRowStructure(CustomRow row) : this(row, null, null)
        {
            Value = row;
        }
        public AbbyyRowStructure(object value, string format = null, AbstractScripter scripter = null)
            : base(value, format, scripter)
        {
            Indexes.Add(FirstIndex);
            Indexes.Add(LastIndex);
            Indexes.Add(CountIndex);
            Indexes.Add(NewIndex);
            Indexes.Add(EmptyIndex);
        }

        public override string ToString(string index = "")
        {
            return null;
        }

        public override Structure Get(string index = null)
        {
            switch (index)
            {
                case null:
                    return this;
                case "cells":
                    return this;
                case "count":
                    return new IntegerStructure(Value.Cells.Count);
                case "top":
                    return new IntegerStructure(Value.Top);
                case "bottom":
                    return new IntegerStructure(Value.Bottom);
                case "istablerow":
                    return new BooleanStructure(Value.IsTableRow);
            }
            int intIndex = 0;
            if (int.TryParse(index, out intIndex) && intIndex < Value.Cells.Count && intIndex >= 0)
            {
                return new AbbyyCellStructure(Value.Cells[intIndex]);
            }
            throw new ArgumentOutOfRangeException($"There is no value under index = {index}");
        }

        public override void Set(Structure value, string index = null)
        {
            throw new NotSupportedException("This value is read-only");
        }
    }
}
