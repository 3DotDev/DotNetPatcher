﻿Imports System.Runtime.InteropServices
Imports System.IO
Imports Helper.ResourcesHelper
Imports System.Drawing

Namespace Core.IconChanger
    ''' <summary>
    ''' IconInjector Class by Mirhabi
    ''' </summary>
    Friend NotInheritable Class Replacer

        ' Basically, you can change icons with the UpdateResource api call.
        ' When you make the call you say "I'm updating an icon", and you send the icon data.
        ' The main problem is that ICO files store the icons in one set of structures, and exe/dll files store them in
        ' another set of structures. So you have to translate between the two -- you can't just load the ICO file as
        ' bytes and send them with the UpdateResource api call.

#Region " Structures "
        ' The first structure in an ICO file lets us know how many images are in the file.
        <StructLayout(LayoutKind.Sequential)> _
        Private Structure ICONDIR
            Public Reserved As UShort  ' Reserved, must be 0
            Public Type As UShort      ' Resource type, 1 for icons.
            Public Count As UShort     ' How many images.
            ' The native structure has an array of ICONDIRENTRYs as a final field.
        End Structure

        ' Each ICONDIRENTRY describes one icon stored in the ico file. The offset says where the icon image data 
        ' starts in the file. The other fields give the information required to turn that image data into a valid
        ' bitmap.
        <StructLayout(LayoutKind.Sequential)> _
        Private Structure ICONDIRENTRY
            Public Width As Byte            ' Width, in pixels, of the image
            Public Height As Byte           ' Height, in pixels, of the image
            Public ColorCount As Byte       ' Number of colors in image (0 if >=8bpp)
            Public Reserved As Byte         ' Reserved ( must be 0)
            Public Planes As UShort         ' Color Planes
            Public BitCount As UShort       ' Bits per pixel
            Public BytesInRes As Integer   ' Length in bytes of the pixel data
            Public ImageOffset As Integer  ' Offset in the file where the pixel data starts.
        End Structure

        ' Each image is stored in the file as an ICONIMAGE structure:
        'typdef struct
        '{
        '   BITMAPINFOHEADER   icHeader;      // DIB header
        '   RGBQUAD         icColors[1];   // Color table
        '   BYTE            icXOR[1];      // DIB bits for XOR mask
        '   BYTE            icAND[1];      // DIB bits for AND mask
        '} ICONIMAGE, *LPICONIMAGE;


        <StructLayout(LayoutKind.Sequential)> _
        Private Structure BITMAPINFOHEADER
            Public Size As UInteger
            Public Width As Integer
            Public Height As Integer
            Public Planes As UShort
            Public BitCount As UShort
            Public Compression As UInteger
            Public SizeImage As UInteger
            Public XPelsPerMeter As Integer
            Public YPelsPerMeter As Integer
            Public ClrUsed As UInteger
            Public ClrImportant As UInteger
        End Structure

        ' The icon in an exe/dll file is stored in a very similar structure:
        <StructLayout(LayoutKind.Sequential, Pack:=2)> _
        Private Structure GRPICONDIRENTRY
            Public Width As Byte
            Public Height As Byte
            Public ColorCount As Byte
            Public Reserved As Byte
            Public Planes As UShort
            Public BitCount As UShort
            Public BytesInRes As Integer
            Public ID As UShort
        End Structure

#End Region

#Region " Methods "

        Friend Shared Sub ReplaceFromIcon(exeFileName As String, iconFileByte As Icon)
            If File.Exists(exeFileName) AndAlso iconFileByte IsNot Nothing Then
                Dim tmpIco = Path.Combine(Path.GetTempPath, Guid.NewGuid.ToString.Replace("-", "") & ".ico")
                Using fs As New FileStream(tmpIco, FileMode.Create)
                    iconFileByte.Save(fs)
                End Using
                ReplaceIt(exeFileName, tmpIco, 1, 1)
                File.Delete(tmpIco)
            End If
        End Sub

        Private Shared Sub ReplaceIt(exeFileName As String, iconFileName As String, iconGroupID As UInteger, iconBaseID As UInteger)
            Const RT_ICON = 3UI
            Const RT_GROUP_ICON = 14UI
            Dim iconFile As IconFile = IconFile.FromIconFile(iconFileName)
            Dim hUpdate = NativeMethods.BeginUpdateResource(exeFileName, True)
            Dim data = iconFile.CreateIconGroupData(iconBaseID)
            NativeMethods.UpdateResource(hUpdate, New IntPtr(RT_GROUP_ICON), New IntPtr(iconGroupID), 0, data, data.Length)
            For i = 0 To iconFile.ImageCount - 1
                Dim image = iconFile.ImageData(i)
                NativeMethods.UpdateResource(hUpdate, New IntPtr(RT_ICON), New IntPtr(iconBaseID + i), 0, image, image.Length)
            Next
            NativeMethods.EndUpdateResource(hUpdate, False)
        End Sub
#End Region

        Private NotInheritable Class IconFile

#Region " Fields "
            Private iconDir As New ICONDIR
            Private iconEntry() As ICONDIRENTRY
            Private iconImage()() As Byte
#End Region

#Region " Properties "
            Friend ReadOnly Property ImageCount As Integer
                Get
                    Return iconDir.Count
                End Get
            End Property

            Friend ReadOnly Property ImageData(index As Integer) As Byte()
                Get
                    Return iconImage(index)
                End Get
            End Property
#End Region

#Region " Methods "

            Friend Shared Function FromIconFile(filename As String) As IconFile
                Dim instance As New IconFile
                ' Read all the bytes from the file.
                Dim fileBytes() As Byte = IO.File.ReadAllBytes(filename)
                ' First struct is an ICONDIR
                ' Pin the bytes from the file in memory so that we can read them.
                ' If we didn't pin them then they could move around (e.g. when the 
                ' garbage collector compacts the heap)
                Dim pinnedBytes = GCHandle.Alloc(fileBytes, GCHandleType.Pinned)
                ' Read the ICONDIR
                instance.iconDir = DirectCast(Marshal.PtrToStructure(pinnedBytes.AddrOfPinnedObject, GetType(ICONDIR)), ICONDIR)
                ' which tells us how many images are in the ico file. For each image, there's a ICONDIRENTRY, and associated pixel data.
                instance.iconEntry = New ICONDIRENTRY(instance.iconDir.Count - 1) {}
                instance.iconImage = New Byte(instance.iconDir.Count - 1)() {}
                ' The first ICONDIRENTRY will be immediately after the ICONDIR, so the offset to it is the size of ICONDIR
                Dim offset = Marshal.SizeOf(instance.iconDir)
                ' After reading an ICONDIRENTRY we step forward by the size of an ICONDIRENTRY            
                Dim iconDirEntryType = GetType(ICONDIRENTRY)
                Dim size = Marshal.SizeOf(iconDirEntryType)
                For i = 0 To instance.iconDir.Count - 1
                    ' Grab the structure.
                    Dim entry = DirectCast(Marshal.PtrToStructure(New IntPtr(pinnedBytes.AddrOfPinnedObject.ToInt64 + offset), iconDirEntryType), ICONDIRENTRY)
                    instance.iconEntry(i) = entry
                    ' Grab the associated pixel data.
                    instance.iconImage(i) = New Byte(entry.BytesInRes - 1) {}
                    Buffer.BlockCopy(fileBytes, entry.ImageOffset, instance.iconImage(i), 0, entry.BytesInRes)
                    offset += size
                Next
                pinnedBytes.Free()
                Return instance
            End Function

            Friend Function CreateIconGroupData(iconBaseID As UInteger) As Byte()
                ' This will store the memory version of the icon.
                Dim sizeOfIconGroupData As Integer = Marshal.SizeOf(GetType(ICONDIR)) + Marshal.SizeOf(GetType(GRPICONDIRENTRY)) * ImageCount
                Dim data(sizeOfIconGroupData - 1) As Byte
                Dim pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned)
                Marshal.StructureToPtr(iconDir, pinnedData.AddrOfPinnedObject, False)
                Dim offset = Marshal.SizeOf(iconDir)
                For i = 0 To ImageCount - 1

                    Dim bitmapheader As New BITMAPINFOHEADER
                    Dim pinnedBitmapInfoHeader = GCHandle.Alloc(bitmapheader, GCHandleType.Pinned)
                    Marshal.Copy(ImageData(i), 0, pinnedBitmapInfoHeader.AddrOfPinnedObject, Marshal.SizeOf(GetType(BITMAPINFOHEADER)))
                    pinnedBitmapInfoHeader.Free()

                    Dim grpEntry As New GRPICONDIRENTRY
                    With grpEntry
                        .Width = iconEntry(i).Width
                        .Height = iconEntry(i).Height
                        .ColorCount = iconEntry(i).ColorCount
                        .Reserved = iconEntry(i).Reserved
                        .Planes = bitmapheader.Planes
                        .BitCount = bitmapheader.BitCount
                        .BytesInRes = iconEntry(i).BytesInRes
                        .ID = CType(iconBaseID + i, UShort)
                    End With
                    Marshal.StructureToPtr(grpEntry, New IntPtr(pinnedData.AddrOfPinnedObject.ToInt64 + offset), False)
                    offset += Marshal.SizeOf(GetType(GRPICONDIRENTRY))
                Next
                pinnedData.Free()
                Return data
            End Function
#End Region

        End Class

    End Class
End Namespace