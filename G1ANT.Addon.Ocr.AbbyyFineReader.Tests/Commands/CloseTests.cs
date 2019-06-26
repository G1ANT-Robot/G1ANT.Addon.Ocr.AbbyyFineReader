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

using System.Diagnostics;
using System.IO;
using System;
using NUnit.Framework;
using System.Reflection;
using G1ANT.Addon.Ocr.AbbyyFineReader.Tests.Properties;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class CloseTests
    {
        private static string path;
        private Scripter scripter;

        [OneTimeSetUp]
        public void Initialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void SetUp()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
            path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.document1), "tif");
            scripter = new Scripter();
            scripter.InitVariables.Clear();
            scripter.InitVariables.Add("file", new GStructures.TextStructure(path));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void CloseTest()
        {
            scripter.RunLine($@"ocrabbyy.processfile {SpecialChars.Variable}file result {SpecialChars.Variable}result1
                               ocrabbyy.processfile {SpecialChars.Variable}file
                               ocrabbyy.close");
            scripter.Run();
            int documentId = scripter.Variables.GetVariableValue<int>("result1");
            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
            long engineLoadedMemory = GetAllocatedMemory();
            long engineUnloadedMemory = GetAllocatedMemory();
            Assert.IsTrue(engineLoadedMemory - engineUnloadedMemory > 0x10000, $"Closing engine relesed less than 1MB of memory, relesed bytes = {engineLoadedMemory - engineUnloadedMemory}");
        }

        private static long GetAllocatedMemory()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            return Process.GetCurrentProcess().WorkingSet64;
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void CloseDocumentTest()
        {
            scripter.RunLine($@"ocrabbyy.processfile {SpecialChars.Variable}file language English result {SpecialChars.Variable}result1
                                ocrabbyy.processfile {SpecialChars.Variable}file language English result {SpecialChars.Variable}result2");
            int documentId = scripter.Variables.GetVariableValue<int>("result1");
            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
           
            int document2Id = scripter.Variables.GetVariableValue<int>("result2");
            FineReaderDocument document2 = AbbyyManager.Instance.GetDocument(document2Id);

            scripter.Text =($"ocrabbyy.close {document2Id}");
            scripter.Run();
            document.ExtractData();
            bool isDocumentClosed = false;
            try
            {
                document2.ExtractData();
            }
            catch
            {
                isDocumentClosed = true;
            }
            Assert.IsTrue(isDocumentClosed);

            scripter.Text =($"ocrabbyy.close {documentId}");
            scripter.Run();
            isDocumentClosed = false;
            try
            {
                document.ExtractData();
            }
            catch
            {
                isDocumentClosed = true;
            }
            Assert.IsTrue(isDocumentClosed);
        }
    }
}
