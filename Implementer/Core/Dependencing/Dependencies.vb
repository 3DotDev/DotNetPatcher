Imports System.Reflection
Imports System.IO
Imports System.Resources
Imports Helper.AssemblyHelper
Imports Helper.UtilsHelper
Imports System.ComponentModel

Namespace Core.Dependencing
    Public Class Dependencies
        Implements IDisposable

#Region " Fields "
        Private m_OriginalFilePath As String = String.Empty
        Private m_result As String = String.Empty
        Private m_infos As IDataFull = Nothing
        Private ReadOnly m_listOfReferences As List(Of String)
        Private ReadOnly m_RExternal As Dictionary(Of String, String)
        Private ReadOnly m_resourcesDependencies As List(Of String)
#End Region

#Region " Constructor "
        Public Sub New(OriginalFilePath As String, listOfReferences As List(Of String))
            m_listOfReferences = listOfReferences
            m_OriginalFilePath = OriginalFilePath
            m_RExternal = New Dictionary(Of String, String)
            m_resourcesDependencies = New List(Of String)
            ReturnBinaryDependencies()
        End Sub
#End Region

#Region " Methods "

        Private Sub ReturnBinaryDependencies()
            m_infos = Loader.Full(m_OriginalFilePath)
            If Not m_infos Is Nothing Then
                If Not m_infos.TypesClass Is Nothing Then
                    If m_infos.ManifestResourceStreams.Count <> 0 AndAlso m_infos.TypesClass.Any(Function(typeDef) typeDef.FullName.EndsWith(".My.Resources.Resources")) Then
                        For Each Res In m_infos.ManifestResourceStreams
                            Using read As New ResourceReader(Res)
                                For Each Dat As DictionaryEntry In read
                                    Dim data() As Byte = Nothing
                                    Dim dataType = String.Empty
                                    Dim originalDataKey$ = Dat.Key
                                    read.GetResourceData(Dat.Key, dataType, data)
                                    If dataType = "ResourceTypeCode.ByteArray" Then m_resourcesDependencies.Add(Dat.Key.ToString.Replace("_", "."))
                                Next
                                read.Close()
                            End Using
                            Res.Close()
                        Next
                    End If
                End If
            End If
        End Sub

        Public Function Analyze() As AnalysisResult
            Try
                AddToRExternal(m_OriginalFilePath)

                If Not m_listOfReferences.Count = 0 Then
                    For Each it In m_listOfReferences
                        AddToRExternal(it.ToString)
                    Next
                End If

                If Not m_RExternal.Count = 0 Then
                    If Not m_listOfReferences.Count = 0 Then
                        Dim RLbxRefExternal As New Dictionary(Of String, String)
                        For Each r In m_listOfReferences
                            If Not RLbxRefExternal.ContainsKey(New FileInfo(r).Name.ToLower) Then
                                RLbxRefExternal.Add(New FileInfo(r).Name.ToLower, r)
                            End If
                        Next

                        Dim Refcount As Integer = 0
                        For Each r In m_RExternal
                            If Not RLbxRefExternal.ContainsKey(r.Key.ToLower) Then
                                Refcount += 1
                                If Not m_resourcesDependencies.Contains(r.Value.Split(",")(0)) Then
                                    m_result &= r.Value.ToLower & vbNewLine
                                End If
                            End If
                        Next
                        RLbxRefExternal.Clear()
                    Else
                        For Each r In m_RExternal
                            If Not m_resourcesDependencies.Contains(r.Value.Split(",")(0)) Then
                                m_result &= r.Value.ToLower & vbNewLine
                            End If
                        Next
                    End If
                End If
                Return New AnalysisResult(m_result.Trim())
            Catch ex As Exception
                Return New AnalysisResult("Error : Dependencing Analyze : " & vbNewLine & ex.Message)
            End Try
        End Function

        Private Sub AddToRExternal(fPath)
            For Each item In GetExternal(fPath)
                If Not m_RExternal.ContainsKey(item.Key.ToLower) Then
                    m_RExternal.Add(item.Key.ToLower, item.Value)
                End If
            Next
        End Sub

        Private Function GetExternal(target$) As Dictionary(Of String, String)
            Dim resultsExt As New Dictionary(Of String, String)

            Try
                Dim infos As IDataFull = Loader.Full(target)
                For Each ass In infos.AssemblyReferences
                    If Not ass Is Nothing AndAlso Not IsAssemblyInGAC(ass.FullName) AndAlso Not resultsExt.ContainsKey((ass.Name & ".dll").ToLower) Then
                        resultsExt.Add((ass.Name & ".dll").ToLower, ass.FullName)
                    End If
                Next
            Catch ex As Exception
                MsgBox("Error : Dependencing GetExternal : " & vbNewLine & ex.ToString)
            End Try

            Return resultsExt
        End Function

        Private Function IsAssemblyInGAC(assemblyFullName As String) As Boolean
            Try
                Return Assembly.ReflectionOnlyLoad(assemblyFullName).GlobalAssemblyCache
            Catch
                Return False
            End Try
        End Function

        Private Function IsAssemblyInGAC(assembly As Assembly) As Boolean
            Try
                Return assembly.GlobalAssemblyCache
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub Merge(Optional ByVal bgw As BackgroundWorker = Nothing)
            If bgw Is Nothing Then
                File.WriteAllBytes(Functions.GetTempFolder & "\irpck.exe", My.Resources.ILRepack)
                File.Copy(m_OriginalFilePath, Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe", True)

                Try
                    Shell(Functions.GetTempFolder & "\irpck.exe /noRepackRes  /delaysign" & " " & Chr(34) & Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe"" /out:" & Chr(34) & m_OriginalFilePath & Chr(34) & FormatArgument(), AppWinStyle.Hide, True)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
            Else
                Dim cancelled As Boolean
                Dim ChunkSize% = 4096
                Dim SentBytes As Long = 0
                Dim Buffer0 As Byte() = New Byte(ChunkSize - 1) {}

                Try
                    Using writeStream As New FileStream(Functions.GetTempFolder & "\irpck.exe", FileMode.Create, FileAccess.ReadWrite)
                        Using ms As New MemoryStream(My.Resources.ILRepack)
                            Dim BytesRead% = ms.Read(Buffer0, 0, ChunkSize)
                            While BytesRead > 0
                                If bgw.CancellationPending Then
                                    cancelled = True
                                    Exit While
                                End If
                                writeStream.Write(Buffer0, 0, BytesRead)
                                SentBytes += BytesRead
                                BytesRead = ms.Read(Buffer0, 0, ChunkSize)
                            End While
                        End Using
                    End Using

                    If cancelled Then
                        If File.Exists(Functions.GetTempFolder & "\irpck.exe") Then
                            Try
                                File.Delete(Functions.GetTempFolder & "\irpck.exe")
                            Catch ex As Exception
                            End Try
                        End If
                    Else
                        Dim cancelled1 As Boolean
                        Dim ChunkSize1% = 4096
                        Dim SentBytes1 As Long = 0
                        Dim Buffer1 As Byte() = New Byte(ChunkSize1 - 1) {}
                        Using writeStream1 As New FileStream(Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe", FileMode.Create, FileAccess.ReadWrite)
                            Using ms1 As New FileStream(m_OriginalFilePath, FileMode.Open, FileAccess.Read)
                                Dim BytesRead1% = ms1.Read(Buffer1, 0, ChunkSize1)
                                While BytesRead1 > 0
                                    If bgw.CancellationPending Then
                                        cancelled1 = True
                                        Exit While
                                    End If
                                    writeStream1.Write(Buffer1, 0, BytesRead1)
                                    SentBytes1 += BytesRead1
                                    BytesRead1 = ms1.Read(Buffer1, 0, ChunkSize1)
                                End While
                            End Using
                        End Using

                        If cancelled1 Then
                            If File.Exists(Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe") Then
                                Try
                                    File.Delete(Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe")
                                Catch ex As Exception
                                End Try
                            End If
                        Else
                            If File.Exists(Functions.GetTempFolder & "\irpck.exe") AndAlso File.Exists(Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe") Then

                                Try
                                    Shell(Functions.GetTempFolder & "\irpck.exe /noRepackRes /delaysign" & " " & Chr(34) & Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe"" /out:" & Chr(34) & m_OriginalFilePath & Chr(34) & FormatArgument(), AppWinStyle.Hide, True)
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                Finally
                                    Try
                                        File.Delete(Functions.GetTempFolder & "\" & Path.GetFileNameWithoutExtension(m_OriginalFilePath) & "irpck.exe")
                                        File.Delete(Functions.GetTempFolder & "\irpck.exe")
                                    Catch ex As Exception
                                    End Try
                                End Try

                                If bgw.CancellationPending Then
                                    For Each p As Process In Process.GetProcessesByName("irpck")
                                        Try
                                            p.Kill()
                                            p.WaitForExit()
                                        Catch winException As Win32Exception
                                        Catch invalidException As InvalidOperationException
                                        Finally
                                            p.Dispose()
                                        End Try
                                    Next
                                End If

                            End If
                        End If
                    End If
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

            End If
        End Sub

        Private Function FormatArgument() As String
            Dim result = String.Empty
            For Each ite In m_listOfReferences
                result &= " " & Chr(34) & ite & Chr(34)
            Next
            Return result
        End Function

        Public Sub CleanUp()
            If Not m_listOfReferences.Count = 0 Then m_listOfReferences.Clear()
            If Not m_RExternal.Count = 0 Then m_RExternal.Clear()
            m_OriginalFilePath$ = String.Empty
            m_result = String.Empty
        End Sub

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If
                CleanUp()
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace

