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
    public class GetTablePossitionTests
    {
        string path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document3), "tif");

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
        public void GetTablePositionTest()
        {
            Scripter scripter = new Scripter();
            scripter.InitVariables.Clear();

            string egyptPosition;
            egyptPosition = ((GStruct.TextStructure)scripter.Variables.GetVariableValue<List<GStruct.Structure>>(nameof(egyptPosition))[0]).Value;
            string NigeriaPosition;
            scripter.Text =($@"ocrabbyy.processfile {SpecialChars.Text}{path}{SpecialChars.Text}
                               ocrabbyy.gettableposition Egypt tableindex 0 result {SpecialChars.Variable}{nameof(egyptPosition)}
                               ocrabbyy.gettableposition Nigeria tableindex 0 result {SpecialChars.Variable}{nameof(NigeriaPosition)}");
            scripter.Run();
           
            NigeriaPosition = ((GStruct.TextStructure)scripter.Variables.GetVariableValue<List<GStruct.Structure>>(nameof(NigeriaPosition))[0]).Value;
            Assert.AreEqual("8,3", egyptPosition);
            Assert.AreEqual("9,3", NigeriaPosition);
        }
    }
}
