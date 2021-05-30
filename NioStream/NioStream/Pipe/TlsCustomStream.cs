using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NioStream.Pipe
{
    public class  TlsCustomStream : Stream
    {


        public byte[] headBuf;
        public Stream nStream;
        public  TlsCustomStream(Stream _nStream) 
        {
            nStream = _nStream;
        }

        public void AddHead(byte[] head) 
        {
        if(head!=null && head?.Length>0)
        headBuf=head;
        }

        public  void  Dispose() 
        {
            base.Dispose();
        }
        public override bool CanRead => nStream.CanRead;

        public override bool CanSeek => nStream.CanSeek;

        public override bool CanWrite => nStream.CanWrite;

        public override long Length => headBuf == null ? nStream.Length : headBuf.Length;

        public override long Position { get =>  nStream.Position; set => nStream.Position=value; }

        public override void Flush()
        {
            nStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                if (headBuf == null)
                {
                    return nStream.Read(buffer, offset, count);
                }
                else
                {
                    headBuf.CopyTo(buffer, offset);
                    var headbufLength = headBuf.Length;
                    headBuf = null;
                    var rind = headbufLength;
                    return rind ;

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Read TlsCustomStream");
                return -1;
            };
            /*
            try {
            if (headBuf == null)
            {
                return nStream.Read(buffer, offset, count);
            }
            else 
            {
                headBuf.CopyTo(buffer, offset);
                var headbufLength = headBuf.Length;
                headBuf = null;
                var rind= nStream.Read(buffer, offset+ headbufLength, count- headbufLength);
                return rind + headbufLength;

            }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Read TlsCustomStream");
                return -1;
            };
            */
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return nStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            nStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            
            try
            {
                nStream.Write(buffer, offset, count);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Write TlsCustomStream");
            };
            }
    }
}
