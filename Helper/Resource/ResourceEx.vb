Imports System.Runtime.InteropServices
Imports System.ComponentModel

Namespace ResourcesHelper
    Public NotInheritable Class ResourceEx

#Region " Methods "
        Public Shared Function UpdateResourceEx(FilePath As String, ResName%, ByVal ResType%, ByVal ResPath As Byte()) As Boolean
            Try
                Dim hUpdate As IntPtr = NativeMethods.BeginUpdateResource(FilePath, False)
                If (hUpdate = IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error)
                If Not NativeMethods.UpdateResource(hUpdate, ResType, ResName, 0, ResPath, ResPath.Length) Then Throw New Win32Exception(Marshal.GetLastWin32Error)
                If Not NativeMethods.EndUpdateResource(hUpdate, False) Then Throw New Win32Exception(Marshal.GetLastWin32Error)
            Catch ex As Exception
                Return False
            End Try
            Return True
        End Function

        Public Shared Function ExtractResourceEx(hFile As String, idRes As String, IdType As String) As String
            Dim hModule As IntPtr = NativeMethods.LoadLibrary(hFile)
            Try
                Dim hResource As IntPtr = NativeMethods.FindResource(hModule, Marshal.StringToHGlobalUni(idRes), Marshal.StringToHGlobalUni(IdType))
                Dim resSize As Integer = NativeMethods.SizeofResource(hModule, hResource)
                Dim resData As IntPtr = NativeMethods.LoadResource(hModule, hResource)
                Dim uiBytes As Byte() = New Byte(resSize - 1) {}
                Dim HandleGC As GCHandle = GCHandle.Alloc(uiBytes, GCHandleType.Pinned)
                Dim CopyElement As IntPtr = Marshal.UnsafeAddrOfPinnedArrayElement(uiBytes, 0)

                NativeMethods.CopyMemory(CopyElement, resData, resSize)
                HandleGC.Free()
                NativeMethods.FreeResource(resData)

                Return Marshal.PtrToStringAnsi(CopyElement, resSize)
            Catch ex As Exception
                MsgBox(ex.ToString)
            Finally
                NativeMethods.FreeLibrary(hModule)
            End Try
            Return Nothing
        End Function

        Public Shared Function ExtractResourceEx(FileName As String, idRes As String, IdType As IntPtr) As Byte()
            Dim ret As IntPtr = NativeMethods.LoadLibraryEx(FileName, 0, 3)
            Try
                If ret = 0 Then Throw New Win32Exception(Marshal.GetLastWin32Error)
                Dim hglob As IntPtr = Marshal.StringToHGlobalUni("#" & idRes)
                Dim hres As IntPtr = NativeMethods.FindResource(ret, hglob, IdType)
                If hres = 0 Then Throw New Win32Exception(Marshal.GetLastWin32Error) Else Marshal.FreeHGlobal(hglob)
                Dim fRes As IntPtr = NativeMethods.LoadResource(ret, hres)
                If fRes = 0 Then Throw New Win32Exception(Marshal.GetLastWin32Error)
                Dim lRes As IntPtr = NativeMethods.LockResource(fRes)
                If lRes = 0 Then Throw New Win32Exception(Marshal.GetLastWin32Error)
                Dim szSize As Integer = NativeMethods.SizeofResource(ret, hres)
                If szSize = 0 Then Throw New Win32Exception(Marshal.GetLastWin32Error)
                Dim info(szSize - 1) As Byte
                Marshal.Copy(lRes, info, 0, szSize)
                Return info
            Catch ex As Exception
                MsgBox(ex.ToString)
            Finally
                NativeMethods.FreeLibrary(ret)
            End Try
            Return Nothing

        End Function
#End Region

    End Class
End Namespace