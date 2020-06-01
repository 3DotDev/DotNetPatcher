Imports Implementer.Engine.Analyze

Namespace Engine.Context

    ''' <summary>
    ''' INFO : This is the first step of the renamer library. 
    '''        You must pass two arguments (inputFile and outputFile properties) when instantiating this class and calling the ReadAssembly sub.
    '''        This Class inherits Cls_Analyzer.
    ''' </summary>
    Public NotInheritable Class Parameters
        Inherits Analyzer

#Region " Fields "
        Property RenamingAccept As RenamerState
        Property TaskAccept As TaskState
#End Region

#Region " Constructor "
        ''' <summary>
        ''' INFO : Initializes a new instance of the class Context.Parameters (inherits Context.Analyzer class) which used to check if the selected inputfile is a valid PE and executable file. 
        ''' </summary>
        Public Sub New(inputFile$, outputFile$)
            MyBase.New(inputFile, outputFile)
        End Sub
#End Region

    End Class
End Namespace

