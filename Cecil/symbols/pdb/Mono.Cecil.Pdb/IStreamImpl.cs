using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
    class IStreamImpl : IStream
    {
        Stream stream;
        public IStreamImpl(Stream stream)
        {
            this.stream = stream;
        }

        public void Clone(out IStream ppstm)
        {
            ppstm = new IStreamImpl(stream);
        }

        public void Commit(int grfCommitFlags)
        {
            stream.Flush();
        }

        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            throw new NotSupportedException();
        }

        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new NotSupportedException();
        }

        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int val = this.stream.Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero)
                Marshal.WriteInt32(pcbRead, val);
        }

        public void Revert()
        {
            throw new NotSupportedException();
        }

        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            SeekOrigin origin = SeekOrigin.Begin;
            switch (dwOrigin)
            {
                case 0: origin = SeekOrigin.Begin; break;
                case 1: origin = SeekOrigin.Current; break;
                case 2: origin = SeekOrigin.End; break;
            }
            long val = this.stream.Seek(dlibMove, origin);
            if (plibNewPosition != IntPtr.Zero)
                Marshal.WriteInt64(plibNewPosition, val);
        }

        public void SetSize(long libNewSize)
        {
            this.stream.SetLength(libNewSize);
        }

        public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG();
            pstatstg.type = 2;
            pstatstg.cbSize = this.stream.Length;
            pstatstg.grfMode = 0;
            if (this.stream.CanRead && this.stream.CanWrite)
            {
                pstatstg.grfMode |= 2;
            }
            else if (!this.stream.CanRead)
            {
                if (this.stream.CanWrite)
                    pstatstg.grfMode |= 1;
            }

        }

        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new NotSupportedException();
        }

        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            this.stream.Write(pv, 0, cb);
            if (pcbWritten != IntPtr.Zero)
                Marshal.WriteInt32(pcbWritten, cb);
        }
    }
}
