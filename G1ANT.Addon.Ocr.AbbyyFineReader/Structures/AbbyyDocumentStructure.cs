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
using G1ANT.Language;


namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    [Structure(Name = "abbyydocument", Tooltip = "This structure stores documents processed with Abbyy FineReader OCR engine and has two fields")]
    public class AbbyyDocumentStructure : StructureTyped<CustomDocument>
    {
        private const string FirstIndex = "pages";
        private const string LastIndex = "count";
        

        public AbbyyDocumentStructure(CustomDocument document) : this(document, null, null)
        {
            Value = document;
        }

        public override string ToString(string format = "")
        {
            return  Value.ToString();
        }
        public AbbyyDocumentStructure(object value, string format = null, AbstractScripter scripter = null)
            : base(value, format, scripter)
        {
            Indexes.Add(FirstIndex);
            Indexes.Add(LastIndex);
        }

        public override Structure Get(string index = null)
        {
            switch (index)
            {
                case null:
                    return this;
                case "pages":
                    List<Structure> pages = new List<Structure>();
                    foreach (CustomPage page in Value.Pages)
                    {
                        pages.Add(new AbbyyPageStructure(page));
                    }
                    return new ListStructure(pages);
                case "count":
                    return new IntegerStructure(Value.Pages.Count);
            }
            int intIndex = 0;
            if (int.TryParse(index, out intIndex) && intIndex < Value.Pages.Count && intIndex >= 0)
            {
                return new AbbyyPageStructure(Value.Pages[intIndex]);
            }
            throw new ArgumentOutOfRangeException($"There is no value under index = {index}");
        }

        public override void Set(Structure value, string index = null)
        {
            throw new NotSupportedException("This value is read-only");
        }
    }
}
