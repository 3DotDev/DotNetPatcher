Imports System.IO
Imports Helper.CecilHelper
Imports Helper.RandomizeHelper
Imports Implementer.Core.Obfuscation.Builder
Imports Implementer.Engine.Context
Imports Mono.Cecil

Namespace Core.Obfuscation.Protections
    Public Class ProtectionContext

        Public Property PackerTask As Boolean
        Public Property PropertyFileInfo As FileInfo
        Public Property I As Integer = 0
        Public Property Fi As FileInfo
        Public Property FrameworkVersion As String
        Public Property InputAssembly As AssemblyDefinition
        Public Property IsCountedTask As Boolean
        Public Property ProtectedFilePath As String
        Public Property ProtectedPath As String
        Public Property TempProtectedPath As String
        Public Property ResourcesCompression As ResourcesCompression
        Public ReadOnly Property Params As Parameters
        Public ReadOnly Property Randomizer As Randomizer

        Public Sub New(Conf As Parameters)
            Params = Conf
            Randomizer = New Randomizer(Conf.RenamingAccept.RenamingType)
        End Sub

        Public Sub ReadAssembly()
            InputAssembly = AssemblyDefinition.ReadAssembly(Params.InputFile)
            If FrameworkVersion = "" Then FrameworkVersion = Finder.FindFrameworkVersion(InputAssembly)
            I += 1
            Fi = New FileInfo(Params.InputFile)
            Params.OutputFile = Params.InputFile.Replace(Fi.Name, I.ToString & ".exe")
            Params.InputFile = Params.OutputFile
            IsCountedTask = True
        End Sub

        Public Sub WriteAssembly()
            InputAssembly.Write(Params.OutputFile)
        End Sub
    End Class
End Namespace
