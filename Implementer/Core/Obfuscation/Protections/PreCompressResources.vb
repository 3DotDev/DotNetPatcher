Imports Implementer.Core.Obfuscation.Builder

Namespace Core.Obfuscation.Protections
    Public NotInheritable Class PreCompressResources
        Inherits Protection

#Region " Properties "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Dim PackOrNot = If(PackerState, "Packing", "Obfuscating")
                Dim str = PackOrNot & " (Resources "

                If PackerState Then
                    str &= "encrypt & compress preparing ...)"
                Else
                    If Context.Params.TaskAccept.Obfuscation.EncryptResources And Context.Params.TaskAccept.Obfuscation.CompressResources = False Then
                        str &= "encrypt preparing ...)"
                    ElseIf Context.Params.TaskAccept.Obfuscation.EncryptResources = False And Context.Params.TaskAccept.Obfuscation.CompressResources Then
                        str &= "compress preparing ...)"
                    ElseIf Context.Params.TaskAccept.Obfuscation.EncryptResources And Context.Params.TaskAccept.Obfuscation.CompressResources Then
                        str &= "encrypt & compress preparing ...)"
                    End If
                End If
                Return str
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Pre Resources compression/encryption"
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
                Return 60
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
            If PackerState Then
                Context.ResourcesCompression = New ResourcesCompression(New CompressContext(Context.InputAssembly,
                                                                                        PackerState, Context.Randomizer, Context.Randomizer.GenerateNew & ".resources", True, True))
            Else
                Context.ResourcesCompression = New ResourcesCompression(New CompressContext(Context.InputAssembly,
                                                                                        PackerState, Context.Randomizer, Context.Randomizer.GenerateNew & ".resources",
                                                                                        Context.Params.TaskAccept.Obfuscation.CompressResources,
                                                                                        Context.Params.TaskAccept.Obfuscation.EncryptResources))
            End If
            Context.ResourcesCompression.RemoveStubFile()
        End Sub
#End Region

    End Class
End Namespace
