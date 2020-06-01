Imports System.IO
Imports System.IO.Compression
Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DecryptionString
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 3
        Friend Context As ResStreamContext
#End Region

#Region " Constructor "
        Public Sub New(Contex As ResStreamContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GenerateStubFile(), Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectType(Context.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Private Function GenerateStubFile() As String
            Return Source.ReadStringFromResourcesStub(Names.ClassName, Names.Functions(0), Names.Functions(1), Names.Functions(2), Context)
        End Function

        Public Function ReadBinaryFromStream() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Function ReadStreamFromGzip() As MethodDefinition
            Return GetMethod2()
        End Function

        Public Function DecompressFromGzip() As MethodDefinition
            Return GetMethod3()
        End Function

        Friend Function WriteData(StringData As String) As Integer
            Dim IntegPosit% = Context.BinaryWriter.BaseStream.Position
            Context.BinaryWriter.Write(StringData)
            Context.BinaryWriter.Flush()
            Return IntegPosit
        End Function

        Public Sub InjectResource()
            If Not Context.MemoryStream Is Nothing Then
                Injecter.InjectResource(Context.InputAssembly.MainModule, Context.ResourceName, ResourceType.Embedded, CompressWithGStream(Context.MemoryStream.ToArray))
                Context.MemoryStream.Close()
                Context.BinaryWriter.Close()
            End If
        End Sub

        Private Function CompressWithGStream(raw As Byte()) As Byte()
            Using memory As New MemoryStream()
                Using gzip As New GZipStream(memory, CompressionMode.Compress, True)
                    gzip.Write(raw, 0, raw.Length)
                End Using
                Return memory.ToArray()
            End Using
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
            If Not Context.MemoryStream Is Nothing Then
                Context.MemoryStream.Dispose()
            End If
            If Not Context.BinaryWriter Is Nothing Then
                Context.BinaryWriter.Dispose()
            End If
        End Sub
#End Region

    End Class
End Namespace
