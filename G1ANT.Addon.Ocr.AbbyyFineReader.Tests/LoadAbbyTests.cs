/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr.AbbyyFineReader
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using NUnit.Framework;

namespace G1ANT.Addon.Ocr.AbbyyFineReader.Tests
{
    [TestFixture]
    public class LoadAbbyTests
    {
        [Test]
        public void LoadAbbyAddon()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.AbbyyFineReader.dll");
            Assert.IsNotNull(addon);
            Assert.IsTrue(addon.Structures.Count > 0);
            Assert.IsTrue(addon.Commands.Count > 0);
        }
    }
}
