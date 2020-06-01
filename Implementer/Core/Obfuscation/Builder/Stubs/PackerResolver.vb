Imports System.IO
Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class PackerResolver
        Inherits Stub

#Region " Properties "
        Private Const FunctionsNumber As Short = 2
#End Region

#Region " Constructor "
        Public Sub New(Contex As PackerContext)
            MyBase.New(FunctionsNumber, Contex.Randomizer)

            ResolveTypeFromFile(GenerateStubFile(Contex), Finder.FindDefaultNamespace(Contex.InputAssembly, Contex.PackerTask))
            InjectType(Contex.InputAssembly)

            Contex.EncryptAndInjectToResources()
        End Sub
#End Region

#Region " Methods "
        Public Function GetMainMethod() As MethodDefinition
            Return Enumerable.FirstOrDefault(ResolvedTypeDef.Methods, Function(mtd) (mtd.Name = "Main"))
        End Function

        Private Function GenerateStubFile(Context As PackerContext) As String
            Return Source.PackerStub(Context, Names)
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
