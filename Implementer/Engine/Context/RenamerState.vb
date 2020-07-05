Imports Helper.RandomizeHelper.RandomizerType

Namespace Engine.Context
    Public NotInheritable Class RenamerState

#Region " Properties "
        Public ReadOnly Property Namespaces As Boolean
        Public ReadOnly Property Types As Boolean
        Public ReadOnly Property Methods As Boolean
        Public ReadOnly Property Properties As Boolean
        Public ReadOnly Property Fields As Boolean
        Public ReadOnly Property CustomAttributes As Boolean
        Public ReadOnly Property Events As Boolean
        Public ReadOnly Property Parameters As Boolean
        Public ReadOnly Property ReplaceNamespacesSetting As ReplaceNamespaces
        Public ReadOnly Property RenameMainNamespaceSetting As RenameMainNamespace
        Public ReadOnly Property RenamingType As RenameEnum
        Public ReadOnly Property NamespaceSerialized As Boolean

#End Region

#Region " Constructor "
        Public Sub New(Namespac As Boolean, Typ As Boolean, Meth As Boolean, Prop As Boolean, Fiel As Boolean, Even As Boolean, Custom As Boolean,
                param As Boolean, ReplaceNamespace As Boolean, RenameMainNamespace As Boolean, RenamingType%, NamespacSerialized As Boolean)
            _Namespaces = Namespac
            _Types = Typ
            _Methods = Meth
            _Properties = Prop
            _Fields = Fiel
            _Events = Even
            _CustomAttributes = Custom
            _Parameters = param
            _ReplaceNamespacesSetting = ReplaceNamespacesValue(ReplaceNamespace)
            _RenameMainNamespaceSetting = RenameMainNamespaceValue(RenameMainNamespace)
            _RenamingType = RenameTypeValue(RenamingType)
            _NamespaceSerialized = NamespacSerialized
        End Sub
#End Region

#Region " Methods "
        Private Function ReplaceNamespacesValue(boolValue As Boolean) As ReplaceNamespaces
            Return If(boolValue, ReplaceNamespaces.Empty, ReplaceNamespaces.ByDefault)
        End Function

        Private Function RenameMainNamespaceValue(boolValue As Boolean) As RenameMainNamespace
            Return If(boolValue, RenameMainNamespace.Only, RenameMainNamespace.NotOnly)
        End Function

        Private Function RenameTypeValue(intValue%) As RenameEnum
            Return GetScheme(intValue)
        End Function

        Public Function GetScheme(IntVal%) As RenameEnum
            Select Case IntVal
                Case 0
                    Return RenameEnum.Alphabetic
                Case 1
                    Return RenameEnum.Greek
                Case 2
                    Return RenameEnum.Invisible
                Case 3
                    Return RenameEnum.Chinese
                Case 4
                    Return RenameEnum.Japanese
                Case 5
                    Return RenameEnum.Dot
                Case 6
                    Return RenameEnum.Symbols
                Case 7
                    Return RenameEnum.Flowing
            End Select

            Return RenameEnum.Alphabetic
        End Function

        Public Sub CleanUp()
            _Namespaces = False
            _Types = False
            _Methods = False
            _Properties = False
            _Fields = False
            _CustomAttributes = False
            _Events = False
            _Parameters = False
        End Sub
#End Region

#Region " Enumerations "
        ''' <summary>
        ''' INFO : ByDefault : Namespaces of the assembly stayed on first level of the tree. 
        '''        Empty : Namespaces are renamed by String.Empty value and store the types into the -1 level. 
        ''' </summary>
        Public Enum ReplaceNamespaces
            ByDefault = 0
            Empty = 1
        End Enum

        ''' <summary>
        ''' INFO : Full : rename all types and members. 
        '''        Medium : set to false events, variables, parameters. It will set the other one automatically to True.
        '''        Personnalize : requires you to set the boolean values manually for each types and members. 
        ''' </summary>
        Public Enum RenameRule
            Full = 0
            Medium = 1
            Personalize = 2
        End Enum

        ''' <summary>
        ''' INFO : NotOnly : Rename all namespaces.
        '''        Only : It will maybe solve many problems due to rename namespaces of merged assembly(s) !
        ''' </summary>
        Public Enum RenameMainNamespace
            NotOnly = 0
            Only = 1
        End Enum

#End Region

    End Class
End Namespace
