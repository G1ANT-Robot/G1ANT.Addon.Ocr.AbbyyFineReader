﻿/**
*    Copyright(C) G1ANT Robot Ltd, All rights reserved
*    Solution G1ANT.Addon.Ocr.AbbyyFineReader, Project G1ANT.Addon.Ocr.AbbyyFineReader
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
    [Addon(Name = "Ocrabbyy", Tooltip = "AbbyyFineReader Commands")]
    [Copyright(Author = "G1ANT Robot Ltd", Copyright = "G1ANT Robot Ltd", Email = "hi@g1ant.com", Website = "www.g1ant.com")]
    [License(Type = "LGPL", ResourceName = "License.txt")]
    [CommandGroup(Name = "ocrabbyy",  Tooltip = "Abbyy Optical Character Recognition, uses Abbyy.")]
    public class AbbyyFineReaderAddon : Language.Addon
    {
    }
}