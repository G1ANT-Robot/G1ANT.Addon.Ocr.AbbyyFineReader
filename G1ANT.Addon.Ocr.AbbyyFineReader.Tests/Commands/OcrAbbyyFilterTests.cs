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
using System.IO;

using G1ANT.Engine;
using GStruct = G1ANT.Language;

using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using G1ANT.Addon.Ocr.AbbyyFineReader.Tests.Properties;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class OcrAbbyyFilterTests
    {
        [OneTimeSetUp]
        public void Initialize()
        {
            System.Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }
        [SetUp]
        public void Init()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
        }
        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void FilterTest()
        {
            List<string> boldedTexts = new List<string>() { "In 1929", "In 1949", "In 1950,", "In 1955," };
            string path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document2), "jpg");

            Scripter scripter = new Scripter();
            scripter.InitVariables.Clear();
            scripter.Text =($@"ocrabbyy.processfile {SpecialChars.Text}{path}{SpecialChars.Text}
                                ocrabbyy.filter filter bold");
            scripter.Run();
            List<GStruct.Structure> res = scripter.Variables.GetVariableValue<List<GStruct.Structure>>("result");

            foreach (GStruct.Structure value in res)
            {
                string text = ((GStruct.TextStructure)value).Value.Trim();
                if (boldedTexts.Contains(text))
                {
                    boldedTexts.Remove(text);
                }
            }

            System.Text.StringBuilder notRecognizedBold = new System.Text.StringBuilder();
            foreach (string s in boldedTexts)
            {
                notRecognizedBold.Append($" '{s}'");
            }

            Assert.AreEqual(0, boldedTexts.Count, $"Text not recognized as boold {notRecognizedBold}");
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void InvalidFilterTest()
        {
            Scripter scripter = new Scripter();
            scripter.InitVariables.Clear();
            string invalidFilter = "filterThatDoNotExists";
            scripter.Text = ($"ocrabbyy.filter filter {invalidFilter}");
            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
        }
    }
}
