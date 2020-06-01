Imports Helper.AssemblyHelper
Imports dnlib

Namespace Engine.Analyze
    Public NotInheritable Class ValidatedFile
        Inherits EventArgs

#Region " Properties "
        Public ReadOnly Property PeInfos As PeReader
        Public ReadOnly Property IsValid As Boolean
        Public ReadOnly Property Assembly As Data
#End Region

#Region " Constructor "
        Public Sub New(isvalid As Boolean, peInfos As PeReader, ass As Data)
            _IsValid = isvalid
            _Assembly = ass
            _PeInfos = peInfos
        End Sub
#End Region

    End Class
End Namespace