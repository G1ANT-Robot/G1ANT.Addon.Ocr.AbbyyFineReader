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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class XmlMemoryWriter : FREngine.IFileWriter
    {
        private System.IO.MemoryStream stream;
        private XmlDocument document = new XmlDocument();

        public XmlMemoryWriter()
        {

        }

        public void Open(string fileName, ref int bufferSize)
        {
            stream = new System.IO.MemoryStream();
        }

        public void Write(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public void Close()
        {
            stream.Position = 0;
            document.Load(stream);
            stream.Close();
            stream.Dispose();
        }

        public XmlDocument GetDocument()
        {
            return document;
        }
    }
}
