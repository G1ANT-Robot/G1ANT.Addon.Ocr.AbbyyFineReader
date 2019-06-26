/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr.AbbyyFineReader
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Engine;
using GStructures = G1ANT.Language;

using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Reflection;
using G1ANT.Addon.Ocr.AbbyyFineReader.Tests.Properties;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class OcrAbbyyGetDocumentTest
    {
        private Scripter scripter;
        private static string path;

        [OneTimeSetUp]
        public void Initialize()
        {
            System.Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void Init()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
            path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document1), "tif");
            scripter = new Scripter();
scripter.InitVariables.Clear();
           scripter.InitVariables.Add("file", new GStructures.TextStructure(path));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void OcrAbbyyGetDocument1Test()
        {
            scripter.Text = $@"ocrabbyy.processfile path {SpecialChars.Variable}file
                            ocrabbyy.getdocument result {SpecialChars.Text}document{SpecialChars.Text}
                            {SpecialChars.Variable}pagesCount = {SpecialChars.Variable}document{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}firstPage = {SpecialChars.Variable}document{SpecialChars.IndexBegin}0{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}rowsCount = {SpecialChars.Variable}firstpage{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}tenthRow = {SpecialChars.Variable}firstPage{SpecialChars.IndexBegin}9{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}cellsCount = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}";

            scripter.Run();
            CustomRow row = scripter.Variables.GetVariableValue<CustomRow>("tenthRow");
            Assert.IsNotNull(row);
            Assert.IsTrue(row.Cells.Count == 4);
            Assert.AreEqual("4", row.Cells[0].Text);
            Assert.IsTrue(row.Cells[1].Text.ToLower().Contains("zaliczki"));
            Assert.AreEqual("0,00", row.Cells[2].Text.Replace(".", ","));
            Assert.AreEqual("0,00", row.Cells[3].Text.Replace(".", ","));
        }


        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void OcrAbbyyStructuresDocumentIndexTest()
        {
            scripter.Text = $@"ocrabbyy.processfile path {SpecialChars.Variable}file
                            ocrabbyy.getdocument result {SpecialChars.Text}document{SpecialChars.Text}
                            {SpecialChars.Variable}lol1 = {SpecialChars.Variable}document{SpecialChars.IndexBegin}0{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol3 = {SpecialChars.Variable}document{SpecialChars.IndexBegin}pages{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol2 = {SpecialChars.Variable}document{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}pagesCount = {SpecialChars.Variable}document{SpecialChars.IndexBegin}aa{SpecialChars.IndexEnd}";

            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
            Assert.AreEqual(1, scripter.Variables.GetVariableValue<int>("lol2"));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void OcrAbbyyStructuresWrongPageIndex1Test()
        {
            scripter.Text = $@"ocrabbyy.processfile path {SpecialChars.Variable}file
                            ocrabbyy.getdocument result {SpecialChars.Text}document{SpecialChars.Text}
                            {SpecialChars.Variable}pagesCount = {SpecialChars.Variable}document{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}firstPage = {SpecialChars.Variable}document{SpecialChars.IndexBegin}0{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol1 = {SpecialChars.Variable}firstpage{SpecialChars.IndexBegin}rows{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol2 = {SpecialChars.Variable}firstpage{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol3 = {SpecialChars.Variable}firstpage{SpecialChars.IndexBegin}gasg{SpecialChars.IndexEnd}
                            ";
            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
            Assert.IsTrue(scripter.Variables.GetVariableValue<int>("lol2") > 10);
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void OcrAbbyyStructuresRowIndexTest()
        {
            scripter.Text = $@"ocrabbyy.processfile path {SpecialChars.Variable}file
                            ocrabbyy.getdocument result {SpecialChars.Text}document{SpecialChars.Text}
                            {SpecialChars.Variable}pagesCount = {SpecialChars.Variable}document{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}firstPage = {SpecialChars.Variable}document{SpecialChars.IndexBegin}0{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}rowsCount = {SpecialChars.Variable}firstpage{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}tenthRow = {SpecialChars.Variable}firstPage{SpecialChars.IndexBegin}9{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol1 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}cells{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol2 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol3 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}top{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol4 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}bottom{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol5 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}istablerow{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol6 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol7 = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}gsagsa{SpecialChars.IndexEnd}";
            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
            Assert.IsTrue(scripter.Variables.GetVariableValue<int>("lol6") == 4);
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void OcrAbbyyStructuresCellIndexTest()
        {
            scripter.Text = $@"ocrabbyy.processfile path {SpecialChars.Variable}file
                            ocrabbyy.getdocument result {SpecialChars.Text}document{SpecialChars.Text}
                            {SpecialChars.Variable}pagesCount = {SpecialChars.Variable}document{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}firstPage = {SpecialChars.Variable}document{SpecialChars.IndexBegin}0{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}rowsCount = {SpecialChars.Variable}firstpage{SpecialChars.IndexBegin}count{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}tenthRow = {SpecialChars.Variable}firstPage{SpecialChars.IndexBegin}9{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}cell = {SpecialChars.Variable}tenthRow{SpecialChars.IndexBegin}1{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol2 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}top{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol3 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}bottom{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol4 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}left{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol5 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}right{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol6 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}baseline{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol7 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}text{SpecialChars.IndexEnd}
                            {SpecialChars.Variable}lol8 = {SpecialChars.Variable}cell{SpecialChars.IndexBegin}abc{SpecialChars.IndexEnd}
                                ";
            Exception exception = Assert.Throws<ApplicationException>(delegate
            {
                scripter.Run();
            });
            Assert.IsInstanceOf<ArgumentOutOfRangeException>(exception.GetBaseException());
            Assert.IsTrue(scripter.Variables.GetVariableValue<string>("lol7").ToLower().Contains("zalicz"));
        }
    }
}
