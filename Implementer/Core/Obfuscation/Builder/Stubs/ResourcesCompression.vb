Imports System.IO
Imports System.Resources
Imports Helper.CecilHelper
Imports Helper.RandomizeHelper
Imports Helper.UtilsHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class ResourcesCompression
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 4
        Private Context As CompressContext
#End Region

        Public Sub New(Contex As CompressContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GenerateStubFile(), Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectToCctor(Context.InputAssembly)
        End Sub

        Private Function GenerateStubFile() As String
            If Context.PackerTask Then
                Return Source.SevenZipStub(Names.ClassName, Names.Functions(0), Names.Functions(1), Names.Functions(2), Names.Functions(3), Context)
            Else
                Return Source.ResourcesStub(Names.ClassName, Names.Functions(0), Names.Functions(1), Names.Functions(2), Names.Functions(3), Context)
            End If
        End Function

        Friend Sub InjectSevenzipLibrary(assDef As AssemblyDefinition)
            Dim encryptedBytes = My.Resources.SevenzipLib
            If Context.ResourceEncrypt Then Array.Reverse(encryptedBytes)
            If Context.ResourceCompress Then encryptedBytes = Functions.GZipedByte(encryptedBytes)

            Injecter.InjectResource(assDef.MainModule, Context.ResourceName, ResourceType.Embedded, encryptedBytes)
        End Sub

        Friend Sub CompressInjectResources(assDef As AssemblyDefinition)
            Dim tempAsm = AssemblyDefinition.CreateAssembly(New AssemblyNameDefinition(Context.Randomizer.GenerateNewAlphabetic, New Version()), Context.Randomizer.GenerateNewAlphabetic, ModuleKind.Dll)
            For Each resou As Resource In assDef.MainModule.Resources
                tempAsm.MainModule.Resources.Add(resou)
            Next

            Dim encryptedBytes As Byte() = Nothing
            Using stream As New MemoryStream()
                tempAsm.Write(stream)
                encryptedBytes = stream.ToArray()
                If Context.ResourceEncrypt Then Array.Reverse(encryptedBytes)
                If Context.ResourceCompress Then encryptedBytes = Functions.GZipedByte(encryptedBytes)
            End Using

            assDef.MainModule.Resources.Clear()
            Injecter.InjectResource(assDef.MainModule, Context.ResourceName, ResourceType.Embedded, encryptedBytes.ToArray)
        End Sub

        Public Function Initialize() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Function Resolve() As MethodDefinition
            Return GetMethod2()
        End Function

        Public Function GzipStream() As MethodDefinition
            Return GetMethod3()
        End Function

        Public Function Gzip() As MethodDefinition
            Return GetMethod4()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
    End Class
End Namespace
