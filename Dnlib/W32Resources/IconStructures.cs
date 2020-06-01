using System.Runtime.InteropServices;

namespace dnlib.W32Resources
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct GRPICONDIR
    {
        public ushort idReserved;
        // Reserved (must be 0)
        public ushort idType;
        // Resource type (1 for icons)
        public ushort idCount;
        // How many images?
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct GRPICONDIRENTRY
    {
        public byte Width;
        // Width, in pixels, of the image
        public byte Height;
        // Height, in pixels, of the image
        public byte ColorCount;
        // Number of colors in image (0 if >=8bpp)
        public byte Reserved;
        // Reserved
        public ushort Planes;
        // Color Planes
        public ushort BitCount;
        // Bits per pixel
        public uint BytesInRes;
        // how many bytes in this resource?
        public ushort ID;
        // the ID
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ICON_HEADER
    {
        public ushort idReserved;
        // Reserved (must be 0)
        public ushort idType;
        // Resource Type (1 for icons)
        public ushort idCount;
        // How many images?
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ICON_DIRECTORY_ENTRY
    {
        public byte Width;
        // Width, in pixels, of the image
        public byte Height;
        // Height, in pixels, of the image
        public byte ColorCount;
        // Number of colors in image (0 if >=8bpp)
        public byte Reserved;
        // Reserved ( must be 0)
        public ushort Planes;
        // Color Planes
        public ushort BitCount;
        // Bits per pixel
        public uint BytesInRes;
        // How many bytes in this resource?
        public uint ImageOffset;
        // Where in the file is this image?
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ImageResourceDirectory
    {
        public uint Characteristics;
        public uint TimeDateStamp;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public ushort NumberOfNamedEntries;
        public ushort NumberOfIdEntries;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ImageResourceDirectoryEntry
    {
        public uint Name;

        public uint OffsetToData;
        public uint GetOffset(ref bool isDir)
        {
            if ((OffsetToData & 0x80000000u) == 0x80000000u)
            {
                isDir = true;
                return OffsetToData & 0x7fffffff;
            }
            else
            {
                isDir = false;
                return OffsetToData;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ImageResourceDataEntry
    {
        public uint OffsetToData;
        public uint Size;
        public uint CodePage;

        public uint Reserved;
        public uint GetOffset(ref bool isDir)
        {
            if ((OffsetToData & 0x80000000u) == 0x80000000u)
            {
                isDir = true;
                return OffsetToData & 0x7fffffff;
            }
            else
            {
                isDir = true;
                return OffsetToData;
            }
        }
    }

}
