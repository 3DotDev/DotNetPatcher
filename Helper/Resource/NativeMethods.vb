Imports System.Runtime.InteropServices

Namespace ResourcesHelper

    Public NotInheritable Class NativeMethods

#Region " Methods "
        <DllImport("Kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function LoadLibrary(lpFileName As String) As IntPtr
        End Function

        <DllImport("kernel32.dll")> _
        Public Shared Function LoadLibraryEx(ByVal lpFileName As String, ByVal hReservedNull As IntPtr, ByVal dwFlags As Integer) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Public Shared Function FindResource(ByVal hModule As IntPtr, ByVal lpszName As IntPtr, ByVal lpszType As IntPtr) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Public Shared Function LoadResource(ByVal hModule As IntPtr, ByVal hResInfo As IntPtr) As IntPtr
        End Function

        <DllImport("kernel32.dll")> _
        Public Shared Function LockResource(ByVal hResData As IntPtr) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Public Shared Function SizeofResource(ByVal hModule As IntPtr, ByVal hResInfo As IntPtr) As UInteger
        End Function

        <DllImport("kernel32.dll", SetLastError:=True, EntryPoint:="FreeLibrary")> _
        Public Shared Function FreeLibrary(ByVal hModule As IntPtr) As Boolean
        End Function

        <DllImport("kernel32.dll", SetLastError:=False)> _
        Public Shared Function BeginUpdateResource(ByVal pFileName As String, <MarshalAs(UnmanagedType.Bool)> ByVal bDeleteExistingResources As Boolean) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Public Shared Function UpdateResource(ByVal hUpdate As IntPtr, ByVal lpType As IntPtr, ByVal lpName As IntPtr, ByVal wLanguage As UInt16, ByVal lpData As Byte(), ByVal cbData As UInt32) As Boolean
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Public Shared Function UpdateResource(ByVal hUpdate As IntPtr, ByVal lpType As String, ByVal lpName As String, ByVal wLanguage As UInt16, ByVal lpData As IntPtr, ByVal cbData As UInt32) As Boolean
        End Function

        <DllImport("kernel32.dll", setlasterror:=True)> _
        Public Shared Function EndUpdateResource(ByVal hUpdate As IntPtr, ByVal fDiscard As Boolean) As Boolean
        End Function

        <DllImport("Kernel32.dll")> _
        Public Shared Sub CopyMemory(Destination As IntPtr, Source As IntPtr, Length As Integer)
        End Sub

        <DllImport("Kernel32.dll")> _
        Public Shared Function FreeResource(hglbResource As IntPtr) As Integer
        End Function
#End Region

    End Class

End Namespace


