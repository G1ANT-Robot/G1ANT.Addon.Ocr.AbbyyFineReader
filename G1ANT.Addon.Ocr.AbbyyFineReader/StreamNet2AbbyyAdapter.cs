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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class StreamNet2AbbyyAdapter : Stream, IDisposable, FREngine.IReadStream, FREngine.IWriteStream
    {
        private readonly Stream underlyingStream;

        private readonly bool closeUnderlyingStream;

        public StreamNet2AbbyyAdapter(Stream netStream, bool closeUnderlyingStream = true)
        {
            this.closeUnderlyingStream = closeUnderlyingStream;
            underlyingStream = netStream;
        }

        public override bool CanRead => underlyingStream.CanRead;

        public override bool CanSeek => underlyingStream.CanSeek;

        public override bool CanWrite => underlyingStream.CanWrite;

        public override long Length => underlyingStream.Length;

        public override long Position
        {
            get { return underlyingStream.Position; }
            set { underlyingStream.Position = value; }
        }

        public override void Flush()
        {
            underlyingStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return underlyingStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return underlyingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            underlyingStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            underlyingStream.Write(buffer, offset, count);
        }

        public int Read(out byte[] Data, int Count)
        {
            Data = new byte[Count];
            return underlyingStream.Read(Data, 0, Count);
        }

        public void Write(byte[] Data)
        {
            underlyingStream.Write(Data, 0, Data.Length);
        }

        protected override void Dispose(bool disposing)
        {
            if (closeUnderlyingStream)
                base.Dispose(disposing);
        }
    }
}
