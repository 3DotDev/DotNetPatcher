Imports System.IO
Imports System.Security.Cryptography

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class PostAntiTamper
        Inherits Protection

#Region " Fields "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Anti-Tamper finishing ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Post Anti-Tamper"
            End Get
        End Property

        Public Overrides Property Context As ProtectionContext

        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property ProgressIncrement As Integer
            Get
                Return 98
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
            Dim md5bytes As Byte() = CType(CryptoConfig.CreateFromName("MD5"), HashAlgorithm).ComputeHash(File.ReadAllBytes(Context.Params.InputFile))
            Using stream = New FileStream(Context.Params.InputFile, FileMode.Append)
                stream.Write(md5bytes, 0, md5bytes.Length)
            End Using
        End Sub
#End Region

    End Class

End Namespace