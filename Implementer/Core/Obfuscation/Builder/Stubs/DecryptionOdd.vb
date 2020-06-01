Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DecryptionOdd
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 1
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
            Return Source.DecryptOddStub(Names.ClassName, Names.Functions(0), Context)
        End Function

        Public Function DecryptOdd() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
#End Region

    End Class
End Namespace
