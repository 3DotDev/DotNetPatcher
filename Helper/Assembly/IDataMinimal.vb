Imports System.Reflection
Imports Helper.AssemblyHelper.Data

Namespace AssemblyHelper
    Public Interface IDataMinimal

        Property FrameworkVersion As String
        Property AssVersion As String
        Property IsWpf As Boolean
        Property Location As String
        Property EntryPoint As MethodInfo
        Property AssemblyReferences As AssemblyName()
        Property Result As Message

    End Interface

End Namespace
