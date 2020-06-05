Imports System.IO
Imports System.Reflection

Namespace AssemblyHelper
    Public Class Data
        Implements IDataFull
        Implements IDataMinimal

#Region " Properties "
        Public Property FrameworkVersion As String Implements IDataFull.FrameworkVersion
        Public Property AssVersion As String Implements IDataFull.AssVersion
        Public Property IsWpf As Boolean Implements IDataFull.IsWpf
        Public Property Location As String Implements IDataFull.Location
        Public Property EntryPoint As MethodInfo Implements IDataFull.EntryPoint
        Public Property AssemblyReferences As AssemblyName() Implements IDataFull.AssemblyReferences
        Public Property Result As Message Implements IDataFull.Result
        Public Property ManifestResourceNames As IEnumerable(Of String) Implements IDataFull.ManifestResourceNames
        Public Property ManifestResourceStreams As List(Of Stream) Implements IDataFull.ManifestResourceStreams
        Public Property TypesClass As IEnumerable(Of Type) Implements IDataFull.TypesClass
        Public Property HasSerializableAttribute As Boolean Implements IDataFull.HasSerializableAttribute
#End Region

#Region " Enumerations"
        Public Enum Message
            Failed = 0
            Success = 1
        End Enum
#End Region

    End Class
End Namespace

