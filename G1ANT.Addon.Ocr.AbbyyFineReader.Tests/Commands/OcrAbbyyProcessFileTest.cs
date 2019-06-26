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
using System.Reflection;
using G1ANT.Addon.Ocr.AbbyyFineReader.Tests.Properties;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class OcrAbbyyProcessFileTest
    {
        private Scripter scripter;
        private static string path;

        [OneTimeSetUp]
        public void Initialize()
        {
            System.Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void SetUp()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
            path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document3), "tif");
            scripter = new Scripter();
            scripter.InitVariables.Clear();
           scripter.InitVariables.Add("file", new GStructures.TextStructure(path));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void ProcessFileTest()
        {
            scripter.Text =($"ocrabbyy.processfile {SpecialChars.Variable}file language English");
            scripter.Run();
            int documentId = scripter.Variables.GetVariableValue<int>("result");

            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
            Assert.IsNotNull(document);
            string plainText = document.GetAllText();
            Assert.AreEqual(Properties.Resources.documentText, plainText);
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void PagesTest()
        {
            string doc4Path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.dokument4), "pdf");
            string endOfFirstPage = @"Nowa sekcja 3 Strona 1";

            List<GStructures.Structure> list = new List<GStructures.Structure>() { new GStructures.IntegerStructure(1) };
           scripter.InitVariables.Add(nameof(list), new GStructures.ListStructure(list));
            scripter.Text =($"ocrabbyy.processfile {SpecialChars.Text}{doc4Path}{SpecialChars.Text} pages {SpecialChars.Variable}{nameof(list)}");
            scripter.Run();
            FineReaderDocument document = AbbyyManager.Instance.GetDocument(scripter.Variables.GetVariableValue<int>("result"));
            Assert.IsTrue(document.GetAllText().Trim().EndsWith(endOfFirstPage));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void LanguageTest()
        {
            scripter.Text = ($"ocrabbyy.processfile {SpecialChars.Variable}file language Polish");
            scripter.Run();
            int documentId = scripter.Variables.GetVariableValue<int>("result");

            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
            Assert.IsNotNull(document);
            string plainText = document.GetAllText();
            Assert.AreNotEqual(Properties.Resources.documentText, plainText);
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void ProcessWithCustomDirectoryTest()
        {
            string path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document1), "tif");
            List<string> words = new List<string>
            {
                "inwestycje",
                "długoterminowe",
                "aktywa",
                "trwałe",
                "prac",
                "prawne",
                "pozostałych",
                "akcje",
                "papiery",
                "pożyczki",
                "podatku"
            };

            List<GStructures.Structure> wordsList = new List<GStructures.Structure>(words.Capacity);
            foreach (string word in words)
            {
                wordsList.Add(new GStructures.TextStructure(word));
            }

            scripter.InitVariables.Add(nameof(wordsList), new GStructures.ListStructure(wordsList));
            scripter.Text = ($"ocrabbyy.processfile {SpecialChars.Text}{path}{SpecialChars.Text} language Polish languageweight 0 dictionary {SpecialChars.Variable}{nameof(wordsList)}");
            scripter.Run();
            //scripter.RunLine($"ocrabbyy.processfile {SpecialChars.Text}{Initializer.BgzBilans6Path}{SpecialChars.Text} dictionary {SpecialChars.Variable}{nameof(wordsList)}");
            int documentId = scripter.Variables.GetVariableValue<int>("result");

            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
        }
    }
}
