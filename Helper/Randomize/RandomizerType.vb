Namespace RandomizeHelper
    Public NotInheritable Class RandomizerType

#Region " Properties "
        Public Shared Property RenameSetting As RenameEnum
#End Region

#Region " Enumerations "
        Public Enum RenameEnum
            Alphabetic = 0
            Greek = 1
            Invisible = 2
            Chinese = 3
            Japanese = 4
            Dot = 5
            Symbols = 6
            Flowing = 7
        End Enum
#End Region

    End Class
End Namespace