Imports System.Reflection
Imports System.IO

Namespace AssemblyHelper

    Public Class Loader

#Region " Fields "
        Private Shared ReadOnly m_AssList As List(Of String)
#End Region

#Region " Constructor "
        Shared Sub New()
            m_AssList = New List(Of String)
        End Sub
#End Region

#Region " Methods "
        Public Shared Function Minimal(AssPath$) As Data

            Dim tempAppDomain As AppDomain = Nothing
            'Create a directory in user Temp path
            Dim fName = Guid.NewGuid.ToString.Replace("-", "")
            Dim fileName = Path.GetFileName(AssPath)
            Dim Npath As String = Path.Combine(Path.GetTempPath, fName)
            Directory.CreateDirectory(Npath)
            'Copy main assembly file to created temp directory
            Dim tempAssemblyFilePath As String = Path.Combine(Npath, fileName)
            File.Copy(AssPath, tempAssemblyFilePath, True)

            'instanciate object to store assembly informations
            Dim AssData As New Data
            Try
                'Copy DNP Helper.dll file to created temp directory
                File.Copy(Assembly.GetExecutingAssembly.Location, Path.Combine(Npath, New FileInfo(Assembly.GetExecutingAssembly.Location).Name))
                'create appdomain with created temp directory as AppBasePath
                tempAppDomain = AppDomain.CreateDomain(Guid.NewGuid.ToString.Replace("-", ""), Nothing, Npath, "", False)

                'Create instance of Helper.AssemblyHelper.Infos type
                Dim assemblyBuffer As Byte() = File.ReadAllBytes(tempAssemblyFilePath)
                Dim anObject As Object = Nothing
                anObject = tempAppDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly.GetName.Name, "Helper.AssemblyHelper.Infos")

                Dim assemblyInspector As IAssemblyInfos = TryCast(anObject, IAssemblyInfos)

                Dim FrmwkVersion = String.Empty
                Dim AssVersion = String.Empty
                Dim IsWpf As Boolean
                Dim Location = String.Empty
                Dim EntryPoint As MethodInfo = Nothing
                Dim AssemblyReferences As AssemblyName() = Nothing
                Dim ManifestResourceNames As IEnumerable(Of String) = Nothing
                Dim ManifestResourceStreams As New List(Of Stream)
                Dim TypesClass As IEnumerable(Of Type) = Nothing
                Dim HasSerializableAttribute As Boolean = False
                Dim Result As Data.Message

                'Call GetAssemblyInfo from Helper.AssemblyHelper.Infos to load assembly from AssPath and gatter its informations.
                assemblyInspector.GetAssemblyInfo(assemblyBuffer, FrmwkVersion, AssVersion, IsWpf, EntryPoint, AssemblyReferences, ManifestResourceNames, ManifestResourceStreams, TypesClass, HasSerializableAttribute, Result)

                With AssData
                    .FrameworkVersion = FrmwkVersion
                    .AssVersion = AssVersion
                    .IsWpf = IsWpf
                    .Location = AssPath
                    .EntryPoint = EntryPoint
                    .AssemblyReferences = AssemblyReferences
                    .Result = Result
                End With
            Catch exception As Exception
                MsgBox(exception.ToString)
            Finally
                CleanDomain(tempAppDomain, Npath)
            End Try
            Return AssData
        End Function

        Public Shared Function Full(AssPath$) As Data

            Dim tempAppDomain As AppDomain = Nothing
            Dim fName = Guid.NewGuid.ToString.Replace("-", "")
            Dim fileName = Path.GetFileName(AssPath)
            Dim Npath As String = Path.Combine(Path.GetTempPath, fName)
            Directory.CreateDirectory(Npath)
            Dim tempAssemblyFilePath As String = Path.Combine(Npath, fileName)
            File.Copy(AssPath, tempAssemblyFilePath, True)

            Dim AssData As New Data
            Try
                File.Copy(Assembly.GetExecutingAssembly.Location, Path.Combine(Npath, New FileInfo(Assembly.GetExecutingAssembly.Location).Name))

                tempAppDomain = AppDomain.CreateDomain(Guid.NewGuid.ToString.Replace("-", ""), Nothing, Npath, "", False)

                Dim assemblyBuffer As Byte() = File.ReadAllBytes(tempAssemblyFilePath)
                Dim anObject As Object = Nothing
                anObject = tempAppDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly.GetName.Name, "Helper.AssemblyHelper.Infos")

                Dim assemblyInspector As IAssemblyInfos = TryCast(anObject, IAssemblyInfos)

                Dim FrmwkVersion = String.Empty
                Dim AssVersion = String.Empty
                Dim IsWpf As Boolean
                Dim Location = String.Empty
                Dim EntryPoint As MethodInfo = Nothing
                Dim AssemblyReferences As AssemblyName() = Nothing
                Dim ManifestResourceNames As IEnumerable(Of String) = Nothing
                Dim ManifestResourceStreams As New List(Of Stream)
                Dim TypesClass As IEnumerable(Of Type) = Nothing
                Dim HasSerializableAttribute As Boolean = False
                Dim Result As Data.Message

                assemblyInspector.GetAssemblyInfo(assemblyBuffer, FrmwkVersion, AssVersion, IsWpf, EntryPoint, AssemblyReferences, ManifestResourceNames, ManifestResourceStreams, TypesClass, HasSerializableAttribute, Result, True)

                With AssData
                    .FrameworkVersion = FrmwkVersion
                    .AssVersion = AssVersion
                    .IsWpf = IsWpf
                    .Location = New FileInfo(AssPath).DirectoryName
                    .EntryPoint = EntryPoint
                    .AssemblyReferences = AssemblyReferences
                    .ManifestResourceNames = ManifestResourceNames
                    .ManifestResourceStreams = ManifestResourceStreams
                    .TypesClass = TypesClass
                    .HasSerializableAttribute = HasSerializableAttribute
                    .Result = Result
                End With

            Catch exception As Exception
                MsgBox(exception.ToString)
            Finally
                CleanDomain(tempAppDomain, Npath)
            End Try
            Return AssData
        End Function

        Private Shared Sub CleanDomain(tempAppDomain As AppDomain, path$)
            If Not tempAppDomain Is Nothing Then
                Try
                    AppDomain.Unload(tempAppDomain)
                Catch ex As CannotUnloadAppDomainException
                    GC.Collect()
                    AppDomain.Unload(tempAppDomain)
                Finally
                    GC.SuppressFinalize(tempAppDomain)
                End Try

                For Each f In Directory.GetFiles(path)
                    File.Delete(f)
                Next
                For Each f In m_AssList
                    If File.Exists(f) Then
                        Try
                            File.Delete(f)
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If Directory.Exists(path) Then
                    Directory.Delete(path)
                End If

                m_AssList.Clear()
            End If
        End Sub

        Public Shared Function GenerateInfos() As String
            Return "Imports System.Reflection" & vbNewLine & vbNewLine _
            & "<" & "Assembly: AssemblyTitle(""" & Guid.NewGuid.ToString.Replace("-", "") & """)>" & vbNewLine _
            & "<Assembly: AssemblyDescription(""" & Guid.NewGuid.ToString.Replace("-", "") & """)>" & vbNewLine _
            & "<" & "Assembly: AssemblyCompany(""" & Guid.NewGuid.ToString.Replace("-", "") & """)>" & vbNewLine _
            & "<Assembly: AssemblyProduct(""" & Guid.NewGuid.ToString.Replace("-", "") & """)>" & vbNewLine _
            & "<Assembly: AssemblyCopyright(""" & Guid.NewGuid.ToString.Replace("-", "") & """)>" & vbNewLine _
            & "<" & "Assembly: AssemblyTrademark(""" & Guid.NewGuid.ToString.Replace("-", "") & """)>" & vbNewLine _
            & "<Assembly: AssemblyVersion(""" & "1.0.0.0" & """)>" & vbNewLine _
            & "<Assembly: AssemblyFileVersion(""" & "1.0.0.0" & """)>"
        End Function
#End Region

    End Class
End Namespace