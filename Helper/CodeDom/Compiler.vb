Imports System.CodeDom.Compiler
Imports System.IO

Namespace CodeDomHelper
    Public Class Compiler

#Region " Methods "
        Public Shared Function CreateStubFromString(MainClass$, FrmwkVersion$, str$, Optional ByVal ReferencencedAssemblies As Dictionary(Of String, Byte()) = Nothing) As String
            Try
                Dim nam = Guid.NewGuid.ToString.Replace("-", "") & ".dll"
                Dim Version = New Dictionary(Of String, String) From {{"CompilerVersion", FrmwkVersion}}
                Dim cProv As New VBCodeProvider(Version)
                Dim cParams As New CompilerParameters()
                With cParams
                    With cParams.ReferencedAssemblies
                        .Add("System.Windows.Forms.dll")
                        .Add("mscorlib.dll")
                        .Add("System.dll")
                        If str.Contains("Imports System.Linq") Then
                            .Add("System.Core.dll")
                        End If
                        If Not ReferencencedAssemblies Is Nothing Then
                            For Each it In ReferencencedAssemblies
                                File.WriteAllBytes(it.Key, it.Value)
                                .Add(it.Key)
                            Next
                        End If
                    End With
                    .CompilerOptions = "/target:library /platform:anycpu /optimize"
                    .GenerateExecutable = False
                    .OutputAssembly = My.Computer.FileSystem.SpecialDirectories.Temp & "\" & nam
                    .GenerateInMemory = False
                    .IncludeDebugInformation = False
                    .MainClass = MainClass
                End With
                Dim cResults As CompilerResults = cProv.CompileAssemblyFromSource(cParams, str)
                If cResults.Errors.Count <> 0 Then
                    For Each er In cResults.Errors
                        MsgBox("Error on line : " & vbNewLine & er.Line.ToString & vbNewLine &
                              "Error description : " & er.ErrorText & vbNewLine &
                              "File : " & vbNewLine & str)
                    Next
                Else
                    Return My.Computer.FileSystem.SpecialDirectories.Temp & "\" & nam
                End If
            Catch ex As Exception
                MsgBox("Error (CreateStubFromString) : " & vbNewLine & ex.ToString)
            End Try

            Return Nothing
        End Function
#End Region

    End Class
End Namespace