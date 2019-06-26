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

using G1ANT.Engine;
using G1ANT.Language.Semantic;

using NUnit.Framework;
using System.Threading;
using G1ANT.Language;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class ProcessScreenTests
    {
        private Scripter scripter;
        private static System.Diagnostics.Process testerApp;
        private static string appTitle = "TestApp";
        private const int TesterAppTimeout = 1000;

        [OneTimeSetUp]
        public void Initialize()
        {
            System.Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void Init()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
            scripter = new Scripter();
            scripter.InitVariables.Clear();
            testerApp = AbbyTests.StartFormTester($"Title {appTitle}");
        }
        
        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void ProcessScreenTest()
        {
            IntPtr hTesterAppWindow = testerApp.MainWindowHandle;
            RobotWin32.Rect windowRect = new RobotWin32.Rect();
            RobotWin32.GetWindowRectangle(hTesterAppWindow, ref windowRect);
            int titleBarHeight = 24;
            scripter.Text = ($"ocrabbyy.processscreen area {SpecialChars.Text}{windowRect.Left},{windowRect.Top},{windowRect.Right},{windowRect.Top + titleBarHeight}{SpecialChars.Text}");
            scripter.Run();
            int documentId = scripter.Variables.GetVariableValue<int>("result");

            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
            Assert.IsNotNull(document);
            string plainText = document.GetAllText();

            Assert.IsTrue(plainText.Contains(appTitle));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void RelativeTest()
        {
            IntPtr hTesterAppWindow = testerApp.MainWindowHandle;
            RobotWin32.Rect windowRect = new RobotWin32.Rect();
            RobotWin32.GetWindowRectangle(hTesterAppWindow, ref windowRect);
            int titleBarHeight = 24;
            scripter.Text = ($"ocrabbyy.processscreen area {SpecialChars.Text}0,0,{windowRect.Right - windowRect.Left},{titleBarHeight}{SpecialChars.Text} relative true");
            scripter.Run();
            int documentId = scripter.Variables.GetVariableValue<int>("result");

            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
            Assert.IsNotNull(document);
            string plainText = document.GetAllText();

            Assert.IsTrue(plainText.Contains(appTitle));
        }

        [Test, Timeout(AbbyTests.TestsTimeout)]
        public void LanguageTest()
        {
            IntPtr hTesterAppWindow = testerApp.MainWindowHandle;
            RobotWin32.Rect windowRect = new RobotWin32.Rect();
            RobotWin32.GetWindowRectangle(hTesterAppWindow, ref windowRect);
            int titleBarHeight = 24;

            string appTitle2 = "шестьсот";  // in case someone worry what this mean, it's six hundred

            scripter.Text = ($@"keyboard {SpecialChars.Text}title {appTitle2}{SpecialChars.Text}{SpecialChars.KeyBegin}enter{SpecialChars.KeyEnd}
                                ocrabbyy.processscreen area {SpecialChars.Text}{windowRect.Left},{windowRect.Top},{windowRect.Right},{windowRect.Top + titleBarHeight}{SpecialChars.Text} language russian");
            scripter.Run();
           
            int documentId = scripter.Variables.GetVariableValue<int>("result");

            FineReaderDocument document = AbbyyManager.Instance.GetDocument(documentId);
            Assert.IsNotNull(document);
            string plainText = document.GetAllText();

            Assert.IsTrue(plainText.Contains(appTitle2));
        }


        [TearDown]
        public void ClassCleanup()
        {
            if ((testerApp?.HasExited ?? true) == false)
            {
                testerApp.Kill();
            }
        }
    }
}