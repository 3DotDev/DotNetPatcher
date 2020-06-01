Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DecryptionBase64
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 2
        Private ReadOnly Context As StubContext
#End Region

#Region " Constructor "
        Public Sub New(Contex As StubContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GenerateStubFile(Contex), Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectType(Context.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Private Function GenerateStubFile(Context As StubContext) As String
            Return Source.FromBase64Stub(Names.ClassName, Names.Functions(0), Names.Functions(1), Context)
        End Function

        Public Function FromBase64() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Function GetStringFromBytes() As MethodDefinition
            Return GetMethod2()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
#End Region

    End Class
End Namespace
