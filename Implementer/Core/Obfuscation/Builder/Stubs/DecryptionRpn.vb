Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DecryptionRpn
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 2
#End Region

#Region " Constructor "
        Public Sub New(Contex As StubContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)

            ResolveTypeFromFile(GenerateStubFile(Contex), Finder.FindDefaultNamespace(Contex.InputAssembly, Contex.PackerTask))
            InjectType(Contex.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Private Function GenerateStubFile(Context As StubContext) As String
            Return Source.DecryptRPNStub(Names.ClassName, Names.Functions(0), Names.Functions(1), Context)
        End Function

        Public Function BrowseOperands() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Function CleanupExpressions() As MethodDefinition
            Return GetMethod2()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
#End Region

    End Class
End Namespace
