Imports Implementer.Core.Dependencing

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class EmbedDependencies
        Inherits Protection

#Region " Fields "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return "Dependencies embedding ..."
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Packer Embed Dependencies"
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
                Return 16
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If Contex.Params.TaskAccept.Packer.Enabled AndAlso Contex.Params.TaskAccept.DllReferences.Enabled AndAlso
                Contex.Params.TaskAccept.DllReferences.Dependencies.Count <> 0 AndAlso
                Contex.Params.TaskAccept.DllReferences.DependenciesMode = DependenciesInfos.DependenciesAddMode.Embedded Then
                _Enabled = True

            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            Dim em As New Embedding(Context, PackerState)
            With em
                .CreateResolverClass()
                .InjectFiles()
            End With
        End Sub

#End Region

    End Class
End Namespace
