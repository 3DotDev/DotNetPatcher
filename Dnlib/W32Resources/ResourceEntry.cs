using System;
using System.IO;

namespace dnlib.W32Resources
{
    /// <summary>
    /// 
    /// </summary>
    internal class ResourceEntry
    {
        internal ImageResourceDataEntry Entry;

        internal uint Name;

        private Stream m_Stream;
        public ResourceEntry(ImageResourceDataEntry Entry, Stream stream, uint Name)
        {
            this.Entry = Entry;
            this.m_Stream = stream;
            this.Name = Name;
        }

        public void Seek()
        {
            m_Stream.Seek(DataAddress, SeekOrigin.Begin);
        }

        public long DataAddress
        {
            get
            {
                bool isDir = false;
                return Convert.ToInt64(Entry.GetOffset(ref isDir));
            }
        }
    }
}
