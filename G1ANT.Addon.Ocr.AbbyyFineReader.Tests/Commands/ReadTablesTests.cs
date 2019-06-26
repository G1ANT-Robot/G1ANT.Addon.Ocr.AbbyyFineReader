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
using System.IO;

using G1ANT.Engine;
using GStruct = G1ANT.Language;

using NUnit.Framework;
using System.Reflection;
using G1ANT.Addon.Ocr.AbbyyFineReader.Tests.Properties;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class ReadTablesTests
    {
        string path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document3), "tif");

        [OneTimeSetUp]
        public void Initialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }
        [SetUp]
        public void Init()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
        }
        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void ReadTabletest()
        {
            Scripter scripter = new Scripter();
            scripter.InitVariables.Clear();

            scripter.Text = ($@"ocrabbyy.processfile {SpecialChars.Text}{path}{SpecialChars.Text}
                                ocrabbyy.readtables");
            scripter.Run();
            List<GStruct.Structure> cells = scripter.Variables.GetVariableValue<List<GStruct.Structure>>("result");

            List<string> values = new List<string>() { "Egypt", "Nigeria" };

            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < values.Count; j++)
                {
                    if (values[j] == ((GStruct.TextStructure)cells[i]).Value.Trim())
                    {
                        values.Remove(values[j]);
                    }
                }
            }

            Assert.AreEqual(0, values.Count, $"Havent found values {values}");
        }
    }
}
