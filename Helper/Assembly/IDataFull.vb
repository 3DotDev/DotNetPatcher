Imports System.IO

Namespace AssemblyHelper
    Public Interface IDataFull
        Inherits IDataMinimal

        Property ManifestResourceNames As IEnumerable(Of String)
        Property ManifestResourceStreams As List(Of Stream)
        Property TypesClass As IEnumerable(Of Type)
        Property HasSerializableAttribute As Boolean

    End Interface
End Namespace

