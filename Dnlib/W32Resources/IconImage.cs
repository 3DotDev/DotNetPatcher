using System;
using System.IO;

namespace dnlib.W32Resources
{
    internal class IconImage
    {
        internal GRPICONDIRENTRY Entry;

        internal ResourceEntry Resource;
        internal long GetResourceAddress(long SectionBaseAddress, long SectionVirtualAddress)
        {
            return SectionBaseAddress + (Resource.DataAddress - SectionVirtualAddress);
        }

        internal byte[] GetImageData(BinaryReader reader, Stream stream, long location)
        {
            stream.Seek(location, SeekOrigin.Begin);
            uint size = Resource.Entry.Size;
            return reader.ReadBytes(Convert.ToInt32(size));
        }
    }
}
