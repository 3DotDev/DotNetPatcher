using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace dnlib.W32Resources
{
    internal class IconResource
    {
        private GRPICONDIR Group;

        private List<IconImage> Entries = new List<IconImage>();
        private Stream m_Stream;
        private long m_BaseAddress;
        private long VirtualAddress;
        private long SectionBaseAddress;

        private long SectionVirtualAddress;
        internal IconResource(Stream stream, long VirtualAddress, long SectionBaseAddress, long SectionVirtualAddress)
        {
            this.m_Stream = stream;
            this.SectionBaseAddress = SectionBaseAddress;
            this.SectionVirtualAddress = SectionVirtualAddress;
            this.VirtualAddress = VirtualAddress;
            this.m_BaseAddress = SectionBaseAddress + (VirtualAddress - SectionVirtualAddress);
        }

        /// <summary>
        /// Move the position of the stream to the start of the structure
        /// </summary>
        internal void Seek()
        {
            m_Stream.Seek(m_BaseAddress, SeekOrigin.Begin);
        }

        /// <summary>
        /// Read icon group from PE file header
        /// </summary>
        /// <param name="reader">reader that holds the PE image</param>
        /// <param name="iconImageData">all the ResourceEntry objects that hold the image data for the icon</param>
        internal bool Read(BinaryReader reader, List<ResourceEntry> iconImageData)
        {
            try
            {
                Group = PeReader.FromBinaryReader<GRPICONDIR>(reader);
                if (Group.idReserved != 0)
                {
                    return false;
                }

                if (Group.idType != 1)
                {
                    return false;
                }

                for (int i = 0; i <= Group.idCount - 1; i++)
                {
                    GRPICONDIRENTRY entry = PeReader.FromBinaryReader<GRPICONDIRENTRY>(reader);
                    IconImage image = new IconImage();
                    image.Entry = entry;
                    foreach (ResourceEntry bmp in iconImageData)
                    {
                        if (bmp.Name == entry.ID)
                        {
                            image.Resource = bmp;
                            Entries.Add(image);
                            break;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes the icon group as a .ico file
        /// </summary>
        /// <param name="path">path to write to</param>
        /// <param name="reader">reader that holds the PE image</param>
        internal void Write(string path, BinaryReader reader)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    ICON_HEADER header = new ICON_HEADER();

                    header.idCount = Convert.ToUInt16(Entries.Count);
                    header.idReserved = Group.idReserved;
                    header.idType = Group.idType;

                    writer.Write(RawSerialize(header));

                    long size = Marshal.SizeOf(typeof(ICON_DIRECTORY_ENTRY));

                    long headerEnd = Marshal.SizeOf(typeof(ICON_HEADER)) + (size * Entries.Count);

                    long baseImageDataOffset = headerEnd;

                    foreach (IconImage entry in Entries)
                    {
                        ICON_DIRECTORY_ENTRY ent = new ICON_DIRECTORY_ENTRY();

                        ent.ColorCount = entry.Entry.ColorCount;
                        ent.Height = entry.Entry.Height;
                        ent.Reserved = entry.Entry.Reserved;
                        ent.Width = entry.Entry.Width;
                        ent.BytesInRes = entry.Entry.BytesInRes;
                        ent.BitCount = entry.Entry.BitCount;
                        ent.Planes = entry.Entry.Planes;
                        ent.ImageOffset = Convert.ToUInt32(baseImageDataOffset);
                        baseImageDataOffset += entry.Entry.BytesInRes;

                        writer.Write(RawSerialize(ent));
                    }

                    foreach (IconImage entry in Entries)
                    {
                        writer.Write(entry.GetImageData(reader, m_Stream, entry.GetResourceAddress(SectionBaseAddress, SectionVirtualAddress)));
                    }
                }

                FileInfo info = new FileInfo(path);

                if (!info.Directory.Exists)
                {
                    info.Directory.Create();
                }

                if (info.Exists)
                {
                    info.Attributes = FileAttributes.Normal;
                    info.Delete();
                }

                File.WriteAllBytes(path, stream.ToArray());
            }
        }

        internal Icon GetIcon(BinaryReader reader)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    ICON_HEADER header = new ICON_HEADER();

                    header.idCount = Convert.ToUInt16(Entries.Count);
                    header.idReserved = Group.idReserved;
                    header.idType = Group.idType;

                    writer.Write(RawSerialize(header));

                    long size = Marshal.SizeOf(typeof(ICON_DIRECTORY_ENTRY));

                    long headerEnd = Marshal.SizeOf(typeof(ICON_HEADER)) + (size * Entries.Count);

                    long baseImageDataOffset = headerEnd;

                    foreach (IconImage entry in Entries)
                    {
                        ICON_DIRECTORY_ENTRY ent = new ICON_DIRECTORY_ENTRY();

                        ent.ColorCount = entry.Entry.ColorCount;
                        ent.Height = entry.Entry.Height;
                        ent.Reserved = entry.Entry.Reserved;
                        ent.Width = entry.Entry.Width;
                        ent.BytesInRes = entry.Entry.BytesInRes;
                        ent.BitCount = entry.Entry.BitCount;
                        ent.Planes = entry.Entry.Planes;
                        ent.ImageOffset = Convert.ToUInt32(baseImageDataOffset);
                        baseImageDataOffset += entry.Entry.BytesInRes;

                        writer.Write(RawSerialize(ent));
                    }

                    foreach (IconImage entry in Entries)
                    {
                        writer.Write(entry.GetImageData(reader, m_Stream, entry.GetResourceAddress(SectionBaseAddress, SectionVirtualAddress)));
                    }
                }
                return BytesToIcon(stream.ToArray());
            }
        }

        private static Icon BytesToIcon(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return new Icon(ms);
            }
        }

        private static byte[] RawSerialize(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.StructureToPtr(anything, buffer, false);

            byte[] rawdata = new byte[rawsize];

            Marshal.Copy(buffer, rawdata, 0, rawsize);
            Marshal.FreeHGlobal(buffer);

            return rawdata;
        }
    }
}
