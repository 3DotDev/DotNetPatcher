Imports System.ComponentModel
Imports Implementer.Core.Obfuscation.Protections
Imports Implementer.Engine.Context

Namespace Engine.Processing
    Public NotInheritable Class Configurator

#Region " Properties "
        Protected Property Protections As List(Of Protection)

        Private m_BgWorker As BackgroundWorker
        Public Property Params As Parameters
#End Region

#Region " Constructor "
        Public Sub New(Bgw As BackgroundWorker, Parameters As Parameters)
            m_BgWorker = Bgw
            Params = Parameters
            Protections = New List(Of Protection)
        End Sub
#End Region

#Region " Methods "
        Public Sub Execute()
            For Each protect In Protections
                m_BgWorker.ReportProgress(protect.ProgressIncrement, protect.ProgressMessage)
                If protect.MustReadWriteAssembly Then protect.Context.ReadAssembly()
                protect.Execute()
                If protect.MustReadWriteAssembly Then protect.Context.WriteAssembly()
            Next
        End Sub

        Protected Friend Sub Add(Protect As Protection)
            If Protect.Enabled Then
                If (Not HasTask(Protect.GetType)) Then
                    Protections.Insert(Protections.Count, Protect)
                End If
            End If
        End Sub

        Protected Friend Function HasTask(Type As Type) As Boolean
            Return Me.Protections.Any AndAlso Me.Protections.Any(Function(task As Protection) task.GetType Is Type)
        End Function

        Protected Friend Function HasTask(Of T)() As Boolean
            Return Me.Protections.Any AndAlso Me.Protections.Any(Function(task As Protection) TypeOf task Is T)
        End Function

        Protected Friend Function GetTask(Of T)() As T
            If (Me.HasTask(Of T)()) Then Return Me.Protections.Where(Function(task As Protection) TypeOf task Is T).Cast(Of T)().FirstOrDefault()
        End Function
#End Region

    End Class
End Namespace
