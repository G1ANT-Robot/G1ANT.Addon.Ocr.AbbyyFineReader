﻿/**
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

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class FineReaderParagraph
    {
        internal Paragraph paragraph;

        public FineReaderParagraph(Paragraph paragraph)
        {
            this.paragraph = paragraph;
        }

        public string Text
        {
            get
            {
                return paragraph.Text;
            }
        }
    }
}
