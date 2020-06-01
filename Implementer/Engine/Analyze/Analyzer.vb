Imports dnlib
Imports System.Drawing
Imports Helper.AssemblyHelper
Imports Implementer.Core.ManifestRequest
Imports Helper.UtilsHelper

Namespace Engine.Analyze

    ''' <summary>
    ''' INFO : This is the first step of the renamer library. 
    '''        You must pass two arguments (inputFile and outputFile properties) when instantiating this class.
    '''        You can either check if the selected file is DotNet executable by calling the isValidFile function.
    ''' </summary>
    Public Class Analyzer

#Region " Fields "
        Private ReadOnly m_pe As PeReader
        Private m_targetFramework As String = String.Empty
        Private m_assemblyVersion As String = String.Empty
        Private m_isWpfProgram As Boolean
#End Region

#Region " Events "
        Public Event FileValidated(sender As Object, e As ValidatedFile)
#End Region

#Region " Properties "
        Public Property InputFile As String
        Public Property OutputFile As String
        Public Property CurrentFile As String
#End Region

#Region " Constructor "
        ''' <summary>
        ''' INFO : Initilize a new instance of the class Analyzer.Cls_Analyzer which used to check if the selected inputfile is a valid PE and executable file. 
        ''' </summary>
        ''' <param name="inputFilePath"></param>
        ''' <param name="outputFilePath"></param>
        Public Sub New(inputFilePath$, outPutFilePath$)
            _inputFile = inputFilePath
            _outputFile = outPutFilePath
            m_pe = New PeReader(_inputFile)
        End Sub
#End Region

#Region " Methods "
        ''' <summary>
        ''' INFO : Check if inputfile is valid DotNet and executable and not Wpf !
        ''' </summary>
        Public Function IsValidFile() As Boolean
            If m_pe.isExecutable Then
                If m_pe.IsManaged Then
                    Dim infos As IDataMinimal = Loader.Minimal(_InputFile)
                    If infos.Result = Data.Message.Success Then
                        If infos.EntryPoint IsNot Nothing Then
                            m_targetFramework = infos.FrameworkVersion
                            m_assemblyVersion = infos.AssVersion
                            m_isWpfProgram = infos.IsWpf

                            If m_isWpfProgram = False Then
                                RaiseEvent FileValidated(Me, New ValidatedFile(True, m_pe, infos))
                                Return True
                            End If
                        End If
                    Else
                        RaiseEvent FileValidated(Me, New ValidatedFile(False, m_pe, Nothing))
                        Return False
                    End If
                End If
            End If
            RaiseEvent FileValidated(Me, New ValidatedFile(False, m_pe, Nothing))
            Return False
        End Function

        Public Function GetAssemblyVersion() As String
            Return m_assemblyVersion
        End Function

        Public Function GetModuleKind() As String
            Return m_pe.GetSystemType
        End Function

        Public Function GetRuntime() As String
            Return If(m_targetFramework = String.Empty, If(m_pe.GetTargetFramework.StartsWith("v4"), "v4.0", "v2.0"), m_targetFramework)
        End Function

        Public Function GetProcessArchitecture() As String
            Return m_pe.GetTargetPlatform
        End Function

        Public Function GetExecutionLevel() As String
            Try
                Return ManifestReader.ExtractManifest(_InputFile)
            Catch ex As Exception
                Return "asInvoker"
            End Try
            Return "asInvoker"
        End Function

        Public Function GetMainIcon() As Bitmap
            Return If(m_pe.GetMainIcon Is Nothing, Functions.GetAutoSize(Icon.ExtractAssociatedIcon(_InputFile).ToBitmap, New Size(48, 48)),
                Functions.GetAutoSize(m_pe.GetMainIcon.ToBitmap, New Size(48, 48)))
        End Function
#End Region

    End Class
End Namespace
