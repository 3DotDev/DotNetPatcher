Namespace Core.Obfuscation.Protections
    Public MustInherit Class Protection

#Region " Properties "
        Public MustOverride ReadOnly Property ProgressMessage As String
        Public MustOverride ReadOnly Property ProgressIncrement As Integer
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property MustReadWriteAssembly As Boolean
        Public MustOverride Property Context As ProtectionContext
        Public MustOverride ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructors "
        Public Sub New()
        End Sub
#End Region

#Region " Methods "
        Public Overridable Sub Init()
        End Sub

        Public Overridable Sub Execute()
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function
#End Region

    End Class
End Namespace
