
Namespace Core.Obfuscation.Protections

    Public NotInheritable Class PostCompressResources
        Inherits Protection

#Region " Properties "
        Private ReadOnly IsResolver As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Dim PackOrNot = If(IsResolver, "Packing", "Obfuscating")
                Dim str = PackOrNot & " (Resources "

                If IsResolver Then
                    str &= "encrypt & compress finishing ...)"
                Else
                    If Context.Params.TaskAccept.Obfuscation.EncryptResources And Context.Params.TaskAccept.Obfuscation.CompressResources = False Then
                        str &= "encrypt finishing ...)"
                    ElseIf Context.Params.TaskAccept.Obfuscation.EncryptResources = False And Context.Params.TaskAccept.Obfuscation.CompressResources Then
                        str &= "compress finishing ...)"
                    ElseIf Context.Params.TaskAccept.Obfuscation.EncryptResources And Context.Params.TaskAccept.Obfuscation.CompressResources Then
                        str &= "encrypt & compress finishing ...)"
                    End If
                End If
                Return str
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Post Resources compression/encryption"
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
                Return 85
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerState As Boolean = False)
            MyBase.New()
            Context = Contex
            IsResolver = PackerState
            If IsResolver Then
                If Contex.Params.TaskAccept.Packer.Enabled Then
                    _Enabled = True
                End If
            Else
                If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.CompressResources OrElse Contex.Params.TaskAccept.Obfuscation.EncryptResources Then
                    _Enabled = True
                End If
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            If IsResolver Then
                Context.ResourcesCompression.InjectSevenzipLibrary(Context.InputAssembly)
            Else
                Context.ResourcesCompression.CompressInjectResources(Context.InputAssembly)
            End If
        End Sub
#End Region

    End Class
End Namespace
