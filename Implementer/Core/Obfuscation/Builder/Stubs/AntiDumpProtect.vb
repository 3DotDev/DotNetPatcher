Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Public Class AntiDumpProtect
        Inherits Stub

#Region " Properties "
        Private Context As StubContext
#End Region

#Region " Constructor "
        Public Sub New(Contex As StubContext)
            MyBase.New("AntiDumping", "Initialize", Contex.Randomizer)
            Context = Contex

            ResolveTypeFromFile(GetType(AntiDumping).Assembly.Location, Finder.FindDefaultNamespace(Context.InputAssembly, Context.PackerTask))
            InjectToCctor(Context.InputAssembly)
        End Sub
#End Region

#Region " Methods "
        Public Function Initialize() As MethodDefinition
            Return GetMethod1()
        End Function
#End Region

    End Class
End Namespace
