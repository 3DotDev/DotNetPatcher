Imports System.IO
Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DecryptionResources
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 1
        Private Context As ResManagerContext
#End Region

#Region " Constructor "
        Public Sub New(Contex As ResManagerContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GenerateStubFile(), Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectType(Context.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Private Function GenerateStubFile() As String
            Return Source.ReadFromResourcesStub(Names.ClassName, Names.Functions(0), Context)
        End Function

        Public Function ReadFromResource() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Sub AddResource(Name As String, Value As String)
            If Not Context.ResourceWriter Is Nothing Then
                Context.ResourceWriter.AddResource(Name, Value)
            End If
        End Sub

        Public Sub InjectResource()
            If Not Context.ResourceWriter Is Nothing Then
                Context.ResourceWriter.Close()
                Injecter.InjectResource(Context.InputAssembly, Context.ResourceName)
            End If
        End Sub

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub

        Public Sub Cleanup()
            If File.Exists(Context.ResourceNamePath) Then
                File.Delete(Context.ResourceNamePath)
            End If
        End Sub
#End Region

    End Class
End Namespace
