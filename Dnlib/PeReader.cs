using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using dnlib.DotNet.MD;
using dnlib.PE;
using dnlib.W32Resources;
using System.Linq;
using dnlib.IO;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace dnlib
{
    //' <summary>
    //' Reads in the header information of the Portable Executable format.
    //' </summary>
    public class PeReader
{

    #region " Fields "
    private string m_fPath;
    private PEImage m_peImage;
    private ImageCor20Header m_cor20Header;
    private bool m_isManaged;
    private string m_RunTimeVersion;
    private Icon m_MainIcon = null;
    #endregion

    #region " Properties "
        public bool IsManaged
    {
        get { return m_isManaged; }
    }

    public string GetTargetFramework
    {
        get
            {
                if (m_isManaged)
                {
                    return m_RunTimeVersion;
                }
                return String.Empty;     
            }
    }

    public bool IsILOnly
    {
        get
        {
            if (m_isManaged)
            {
                return (m_cor20Header.Flags & ComImageFlags.ILOnly) != 0;
            }
            return false;
        }
    }

    public bool Is32BitRequired
    {
        get
        {
            if (m_isManaged)
            {
                return (m_cor20Header.Flags & ComImageFlags._32BitRequired) != 0;
            }
            return false;
        }
    }

    public bool Is32BitPreferred
    {
        get
        {
            if (m_isManaged)
            {
                return (m_cor20Header.Flags & ComImageFlags._32BitPreferred) != 0;
            }
            return false;
        }
    }

    public string GetSystemType
    {
        get
        {
            switch (m_peImage.ImageNTHeaders.OptionalHeader.Subsystem)
            {
                case Subsystem.WindowsGui:
                    return "Forms";
                case Subsystem.WindowsCui:
                    return "Console";
                default:
                    return "Unsupported";
            }
        }
    }

        public FileVersionInfo GetVersionInfos
        {
            get { return FileVersionInfo.GetVersionInfo(m_fPath); }
        }

        public string GetTargetPlatform
    {
        get
        {
            switch (m_peImage.ImageNTHeaders.FileHeader.Machine)
            {
                case Machine.I386:
                    if (m_isManaged)
                    {
                        switch (((m_cor20Header.Flags & ComImageFlags._32BitRequired) != 0 ? 2 : 0) + ((m_cor20Header.Flags & ComImageFlags._32BitPreferred) != 0 ? 1 : 0))
                        {
                            case 0:
                                if (((m_cor20Header.Flags & ComImageFlags.ILOnly) != 0) == false)
                                {
                                    return "x86";
                                }
                                return "AnyCPU";
                            case 1:
                                break;
                            case 2:
                                return "x86";
                            case 3:
                                return "AnyCPU";
                        }
                        return "AnyCPU";
                    }
                    else
                    {
                        return "x86";
                    }
                case Machine.AMD64:
                    return "x64";
                case Machine.IA64:
                    return "Itanium";
                default:
                    return "Unsupported";
            }
        }
    }

    public bool isExecutable
    {
        get
        {
            if ((m_peImage.ImageNTHeaders.FileHeader.Characteristics & Characteristics.Dll) != 0)
            {
                return false;
            }
            return true;
        }
    }

    public Icon GetMainIcon
    {
        get { return m_MainIcon; }
    }

    #endregion

    #region " Constructor "
    public PeReader(string fPath)
    {
            try
            {
                m_fPath = fPath;
                m_peImage = new PEImage(File.ReadAllBytes(fPath));

                if (m_peImage.ImageNTHeaders.OptionalHeader.DataDirectories.Length >= 14)
                {
                    ImageDataDirectory DotNetDir = m_peImage.ImageNTHeaders.OptionalHeader.DataDirectories[14];
                    if (m_peImage.ToFileOffset(DotNetDir.VirtualAddress) != 0 && DotNetDir.Size >= 72)
                    {
                        m_cor20Header = new ImageCor20Header(m_peImage.CreateStream(m_peImage.ToFileOffset(DotNetDir.VirtualAddress), 0x48), false);
                        if (m_peImage.ToFileOffset(m_cor20Header.MetaData.VirtualAddress) != 0 && m_cor20Header.MetaData.Size >= 16)
                        {
                            m_isManaged = true;
                            uint mdSize = m_cor20Header.MetaData.Size;
                            RVA mdRva = m_cor20Header.MetaData.VirtualAddress;
                            MetaDataHeader mdHeader = new MetaDataHeader(m_peImage.CreateStream(m_peImage.ToFileOffset(mdRva), mdSize), false);
                            m_RunTimeVersion = mdHeader.VersionString;

                        }
                    }
                }

                if (m_isManaged == true)
                {
                    ImageSectionHeader sect = m_peImage.ImageSectionHeaders.Where(f => f.DisplayName == ".rsrc").FirstOrDefault();
                    if ((sect != null))
                    {
                        ImageDataDirectory resourceTable = m_peImage.ImageNTHeaders.OptionalHeader.DataDirectories[2];
                        if ((resourceTable != null))
                        {
                            uint rva = (uint)resourceTable.VirtualAddress;
                            uint size = sect.VirtualSize > 0 ? sect.VirtualSize : sect.SizeOfRawData;

                            if (rva >= (uint)sect.VirtualAddress && rva < (uint)sect.VirtualAddress + size)
                            {
                                Stream StreamRead = m_peImage.CreateFullStream().CreateStream();
                                long baseAddress = StreamRead.Seek(sect.PointerToRawData + (rva - (uint)sect.VirtualAddress), SeekOrigin.Begin);
                                ResourceDirectory dirInfo = new ResourceDirectory(StreamRead, baseAddress);

                                if ((dirInfo != null))
                                {
                                    using (BinaryReader reader = new BinaryReader(StreamRead))
                                    {
                                        dirInfo.Read(reader, true, 0);

                                        ResourceEntry IconGroup = null;
                                        List<ResourceEntry> IconImages = new List<ResourceEntry>();

                                        foreach (ResourceDirectory dir in dirInfo.Directorys)
                                        {
                                            if (dir.DirectoryEntry.Name == Convert.ToUInt32(Win32ResourceType.RT_GROUP_ICON))
                                            {
                                                IconGroup = dir.GetFirstEntry();
                                                break;
                                            }
                                        }

                                        foreach (ResourceDirectory dir in dirInfo.Directorys)
                                        {
                                            if (dir.DirectoryEntry.Name == Convert.ToUInt32(Win32ResourceType.RT_ICON))
                                            {
                                                IconImages = dir.GetAllEntrys();
                                                IconImages.Reverse();
                                                break;
                                            }
                                        }

                                        if (IconGroup != null)
                                        {
                                            IconResource icon = new IconResource(StreamRead, IconGroup.DataAddress, sect.PointerToRawData, (uint)sect.VirtualAddress);
                                            icon.Seek();
                                            if (!icon.Read(reader, IconImages))
                                            {
                                                m_MainIcon = null;
                                            }
                                            m_MainIcon = icon.GetIcon(reader);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex);
            }

       
  
        }
    #endregion

    #region " Methods "

    /// <summary>
    /// Reads in a block from a file and converts it to the structure
    /// </summary>
    /// <typeparam name="T">type of the struct to read</typeparam>
    /// <param name="reader">reader</param>
    /// <returns>a instance of the struct T cast from the data in the reader</returns>
    public static T FromBinaryReader<T>(BinaryReader reader)
    {
        byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
        GCHandle gch = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        T @struct = (T)Marshal.PtrToStructure(gch.AddrOfPinnedObject(), typeof(T));
        gch.Free();
        return @struct;
    }
    #endregion
 
}

}

