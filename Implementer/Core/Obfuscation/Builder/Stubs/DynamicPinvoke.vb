Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class DynamicPinvoke
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 3
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
            Return Source.DynamicInvokeStub(Names.ClassName, Names.Functions(0), Names.Functions(1), Names.Functions(2), Context)
        End Function

        Public Function LoadLibrary() As MethodDefinition
            Return GetMethod1()
        End Function

        Public Function GetProcAddress() As MethodDefinition
            Return GetMethod2()
        End Function

        Public Function DynamicInvoke() As MethodDefinition
            Return GetMethod3()
        End Function

        Public Sub RemoveStubFile()
            DeleteStubFile()
        End Sub
#End Region

    End Class
End Namespace
