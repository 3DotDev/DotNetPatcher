Imports Implementer.Core.Obfuscation.Builder

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class PreAntiTamper
        Inherits Protection

#Region " Fields "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Anti-Tamper preparing ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Pre Anti-Tamper"
            End Get
        End Property

        Public Overrides Property Context As ProtectionContext

        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property ProgressIncrement As Integer
            Get
                Return 44
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If PackerState Then
                If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.AntiTamper AndAlso Contex.Params.TaskAccept.Packer.Enabled Then
                    _Enabled = True
                End If
            Else
                If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.AntiTamper AndAlso Contex.Params.TaskAccept.Packer.Enabled = False Then
                    _Enabled = True
                End If
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            Try
                Dim AntiTamperResolver As New AntiTamperResolver(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
                AntiTamperResolver.RemoveStubFile()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub
#End Region

    End Class

End Namespace