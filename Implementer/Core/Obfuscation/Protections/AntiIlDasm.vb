Imports Mono.Cecil
Imports System.Runtime.CompilerServices
Imports Helper.CecilHelper

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class AntiIlDasm
        Inherits Protection

#Region " Fields "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Anti-ILDasm adding ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Anti-Debug"
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
                Return 56
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.AntiIlDasm Then
                _Enabled = True
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            Try
                Dim si As Type = GetType(SuppressIldasmAttribute)
                If AssemblyExtensions.HasCustomAttributeByAssemblyName(Context.InputAssembly, si.Name) = False Then
                    Context.InputAssembly.CustomAttributes.Add(New CustomAttribute(Context.InputAssembly.MainModule.Import(si.GetConstructor(Type.EmptyTypes))))
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub
#End Region

    End Class

End Namespace

