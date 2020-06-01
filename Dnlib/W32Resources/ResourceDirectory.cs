using System;
using System.Collections.Generic;
using System.IO;

namespace dnlib.W32Resources
{
    internal class ResourceDirectory
    {
        internal ImageResourceDirectory ResourceDirectoryInfo;
        internal ImageResourceDirectoryEntry DirectoryEntry;
        internal List<ResourceDirectory> Directorys = new List<ResourceDirectory>();
        internal List<ResourceEntry> Entries = new List<ResourceEntry>();
        private Stream m_Stream;

        private long m_BaseAddress;
        public ResourceDirectory(Stream stream, long baseAddress)
        {
            this.m_Stream = stream;
            this.m_BaseAddress = baseAddress;
        }

        public ResourceDirectory(ImageResourceDirectoryEntry DirectoryEntry, Stream stream, long baseAddress) : this(stream, baseAddress)
        {
            this.DirectoryEntry = DirectoryEntry;
        }

        public void Seek()
        {
            bool isDir = false;
            uint dirLoc = DirectoryEntry.GetOffset(ref isDir);
            m_Stream.Seek(m_BaseAddress + dirLoc, SeekOrigin.Begin);
        }

        public void Read(BinaryReader reader, bool isRoot, uint parentName)
        {
            ResourceDirectoryInfo = PeReader.FromBinaryReader<ImageResourceDirectory>(reader);

            List<ImageResourceDirectoryEntry> dirs = new List<ImageResourceDirectoryEntry>();
            List<ImageResourceDataEntry> entrys = new List<ImageResourceDataEntry>();

            for (int i = 0; i <= ResourceDirectoryInfo.NumberOfNamedEntries - 1; i++)
            {
                entrys.Add(PeReader.FromBinaryReader<ImageResourceDataEntry>(reader));
            }

            for (int i = 0; i <= ResourceDirectoryInfo.NumberOfIdEntries - 1; i++)
            {
                if (isRoot)
                {
                    ImageResourceDirectoryEntry dirEntry = PeReader.FromBinaryReader<ImageResourceDirectoryEntry>(reader);
                    if (dirEntry.Name == Convert.ToUInt32(Win32ResourceType.RT_ICON) || dirEntry.Name == Convert.ToUInt32(Win32ResourceType.RT_GROUP_ICON))
                    {
                        dirs.Add(dirEntry);
                    }
                }
                else
                {
                    dirs.Add(PeReader.FromBinaryReader<ImageResourceDirectoryEntry>(reader));
                }
            }

            foreach (ImageResourceDataEntry e in entrys)
            {
                bool isDir = false;
                uint entryLoc = e.GetOffset(ref isDir);
                uint entrySize = e.Size;
                ResourceEntry entryInfo = new ResourceEntry(e, m_Stream, parentName);
                Entries.Add(entryInfo);
            }

            foreach (ImageResourceDirectoryEntry d in dirs)
            {
                bool isDir = false;
                uint dirLoc = d.GetOffset(ref isDir);
                ResourceDirectory dirInfo = new ResourceDirectory(d, m_Stream, m_BaseAddress);
                if (isDir)
                {
                    Directorys.Add(dirInfo);
                    dirInfo.Seek();
                    dirInfo.Read(reader, false, d.Name != 0 ? d.Name : parentName);
                }
                else
                {
                    dirInfo.Seek();
                    ImageResourceDataEntry entry = PeReader.FromBinaryReader<ImageResourceDataEntry>(reader);
                    uint entryLoc = entry.GetOffset(ref isDir);
                    uint entrySize = entry.Size;
                    ResourceEntry entryInfo = new ResourceEntry(entry, m_Stream, parentName);
                    entryInfo.Seek();
                    Entries.Add(entryInfo);
                }
            }
        }

        internal ResourceEntry GetFirstEntry()
        {
            if (Entries.Count > 0)
            {
                return Entries[0];
            }
            foreach (ResourceDirectory dir in Directorys)
            {
                ResourceEntry firstEntry = dir.GetFirstEntry();
                if (firstEntry != null)
                {
                    return firstEntry;
                }
            }
            return null;
        }

        internal List<ResourceEntry> GetAllEntrys()
        {
            List<ResourceEntry> list = new List<ResourceEntry>();
            return GetAllEntrys(list);
        }

        private List<ResourceEntry> GetAllEntrys(List<ResourceEntry> list)
        {
            list.AddRange(Entries);
            foreach (ResourceDirectory dir in Directorys)
            {
                dir.GetAllEntrys(list);
            }
            return list;
        }

    }
}
