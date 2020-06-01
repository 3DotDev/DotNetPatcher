Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class ResourcesResolver
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 1
        Private Context As EmbedContext
#End Region

#Region " Constructor "
        Public Sub New(Contex As EmbedContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GenerateStubFile(), Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectToCctor(Context.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Private Function GenerateStubFile() As String
            Return Source.ResourcesEmbeddingStub(Names.ClassName, Names.Functions(0), Context)
        End Function

        Public Function Resolver() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
#End Region

    End Class
End Namespace
