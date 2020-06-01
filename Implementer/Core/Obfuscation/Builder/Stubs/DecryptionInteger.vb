Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DecryptionInteger
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 1
        Private ReadOnly Context As StubContext
#End Region

#Region " Constructor "
        Public Sub New(Contex As StubContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GenerateStubFile(), Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectType(Context.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Private Function GenerateStubFile() As String
            Return Source.DecryptIntStub(Names.ClassName, Names.Functions(0), Context)
        End Function

        Public Function DecryptInt() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
#End Region

    End Class
End Namespace
