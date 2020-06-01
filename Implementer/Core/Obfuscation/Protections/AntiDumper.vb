Imports Implementer.Core.Obfuscation.Builder

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class AntiDumper
        Inherits Protection

#Region " Fields "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Anti-Dumper injecting ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String = "Anti-Dumper"
        Public Overrides Property Context As ProtectionContext
        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean = True
        Public Overrides ReadOnly Property ProgressIncrement As Integer = 52
        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Methods "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.AntiDumper Then
                _Enabled = True
            End If
        End Sub

        Public Overrides Sub Execute()
            Try
                Dim antiDumpProtect As New AntiDumpProtect(New StubContext(Context.InputAssembly, Context.PackerTask, Context.Randomizer))
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

#End Region

    End Class

End Namespace


