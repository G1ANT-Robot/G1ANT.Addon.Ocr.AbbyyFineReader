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
    [Structure(Name = "abbyypage" )]

    public class AbbyyPageStructure : StructureTyped<CustomPage>
    {
        private const string FirstIndex = "rows";
        private const string LastIndex = "count";

        public AbbyyPageStructure(CustomPage page) : this(page, null, null)
        {
            Value = page;
        }
        public AbbyyPageStructure(object value, string format = null, AbstractScripter scripter = null)
            : base(value, format, scripter)
        {
            Indexes.Add(FirstIndex);
            Indexes.Add(LastIndex);
        }

        public override string ToString(string format = "")
        {
            return  Value.ToString();
        }

        public override Structure Get(string index = null)
        {
            switch (index)
            {
                case null:
                    return this;
                case "rows":
                    List<Structure> rows = new List<Structure>();
                    foreach (CustomRow row in Value.Rows)
                    {
                        rows.Add(new AbbyyRowStructure(row));
                    }
                    return new ListStructure(rows);
                case "count":
                    return new IntegerStructure(Value.Rows.Count);
            }
            int intIndex = 0;
            if (int.TryParse(index, out intIndex) && intIndex < Value.Rows.Count && intIndex >= 0)
            {
                return new AbbyyRowStructure(Value.Rows[intIndex]);
            }
            throw new ArgumentOutOfRangeException($"There is no value under index = {index}");
        }

        public override void Set(Structure value, string index = null)
        {
            throw new NotSupportedException("This value is read-only");
        }
    }
}
