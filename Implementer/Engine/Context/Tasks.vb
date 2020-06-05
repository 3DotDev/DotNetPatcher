Imports Mono.Cecil
Imports System.IO
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Helper.UtilsHelper
Imports Helper.CecilHelper
Imports Implementer.Engine.Processing
Imports Implementer.Core.Dependencing
Imports Implementer.Core.Obfuscation.Protections
Imports Implementer.Core.Versions
Imports Implementer.Core.IconChanger
Imports Implementer.Core.ManifestRequest

Namespace Engine.Context

    ''' <summary>
    ''' INFO : This is the second step of the library. 
    '''        You must pass one argument (parameter) when instantiating this class and calling the RenameAssembly routine.
    ''' </summary>
    Public NotInheritable Class Tasks

#Region " Fields "
        Private m_bgw As BackgroundWorker
        Private ReadOnly Property Parameters As Parameters
        Private m_Dependencies As Dependencies
#End Region

#Region " Events "
        Public Context As ProtectionContext
        Public Shared Event RenamedItem As RenamedItemDelegate
#End Region

#Region " Constructor "
        ''' <summary>
        ''' INFO : Initializes a new instance of the Context.Cls_Context class which allows to add parameters such as members and types state before the task of renaming starts.
        ''' </summary>
        ''' <param name="params"></param>
        Public Sub New(params As Parameters, Bgw As BackgroundWorker)
            m_bgw = Bgw
            Parameters = params
            Context = New ProtectionContext(params)
        End Sub
#End Region

#Region " Methods "
        Public Sub PreparingTask(inputF$)
            m_bgw.ReportProgress(2, "Preparing Task ...")

            Context.InputAssembly = AssemblyDefinition.ReadAssembly(inputF)
            Context.FrameworkVersion = Finder.FindFrameworkVersion(Context.InputAssembly)

            Parameters.InputFile = inputF
            Parameters.CurrentFile = Parameters.InputFile
            Context.ProtectedPath = New FileInfo(Parameters.CurrentFile).DirectoryName & "\Protected"
            Context.TempProtectedPath = Functions.GetTempFolder & "\" & Context.Randomizer.GenerateNewAlphabetic

            If Not Directory.Exists(Context.ProtectedPath) Then Directory.CreateDirectory(Context.ProtectedPath)
            If Not Directory.Exists(Context.TempProtectedPath) Then Directory.CreateDirectory(Context.TempProtectedPath)

            Context.Fi = New FileInfo(Parameters.InputFile)
            Parameters.OutputFile = Context.TempProtectedPath & "\" & New FileInfo(Parameters.CurrentFile).Name
            File.Copy(Parameters.InputFile, Parameters.OutputFile, True)

            Parameters.InputFile = Parameters.OutputFile

            Context.IsCountedTask = False
        End Sub

        ''' <summary>
        ''' INFO : Raise event when a type or a member renamed.
        ''' </summary>
        ''' <param name="it"></param>
        Public Shared Sub RaiseRenamedItemEvent(it As RenamedItem)
            Dim itemEvent As New RenamedItemEventArgs(it)
            RaiseEvent RenamedItem(Nothing, itemEvent)
        End Sub

        Public Sub EmptyTemp()
            Functions.DeleteFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp")
        End Sub

        Public Function HasObfuscationTask() As Boolean
            With Parameters.TaskAccept.Obfuscation
                If .Enabled AndAlso
              (.EncryptNumeric OrElse
              .EncryptBoolean OrElse
              .EncryptString OrElse
              .AntiTamper OrElse
              .AntiDebug OrElse
              .AntiDumper OrElse
              .AntiIlDasm OrElse
              .HidePublicCalls OrElse
              .CompressResources OrElse
              .RenameAssembly OrElse
              .ControlFlow) Then
                    Return True
                End If
            End With
            Return False
        End Function

        Public Function HasPackerTask() As Boolean
            Return Parameters.TaskAccept.Packer.Enabled
        End Function

        Public Function CheckDependencies() As AnalysisResult
            m_bgw.ReportProgress(8, "Dependencies analysis ...")
            m_Dependencies = New Dependencies(Parameters.InputFile, Parameters.TaskAccept.DllReferences.Dependencies)
            Return m_Dependencies.Analyze()
        End Function

        Public Sub DependenciesTask()
            With Parameters.TaskAccept
                If .DllReferences.Enabled Then
                    If .DllReferences.Dependencies.Count <> 0 Then
                        If .DllReferences.DependenciesMode = DependenciesInfos.DependenciesAddMode.Merged Then
                            m_bgw.ReportProgress(16, "Dependencies merging ...")
                            m_Dependencies.Merge()
                        ElseIf .DllReferences.DependenciesMode = DependenciesInfos.DependenciesAddMode.Embedded Then
                            If .Packer.Enabled = False Then DependenciesEmbedded()
                        End If
                    End If
                End If
            End With
        End Sub

        Private Function DependenciesEmbedded(Optional ByVal PackerEnabled As Boolean = False) As Embedding
            With Parameters.TaskAccept.DllReferences
                If .Enabled AndAlso .Dependencies.Count <> 0 AndAlso .DependenciesMode = DependenciesInfos.DependenciesAddMode.Embedded Then
                    m_bgw.ReportProgress(16, "Dependencies embedding ...")
                    Context.ReadAssembly()
                    Dim em As New Embedding(Context, PackerEnabled)
                    With em
                        .CreateResolverClass()
                        .InjectFiles()
                    End With
                    Context.WriteAssembly()
                    Return em
                End If
            End With
            Return Nothing
        End Function

        Public Sub ManifestTask()
            If Parameters.TaskAccept.Manifest.Modified Then
                m_bgw.ReportProgress(20, "Requested Level patching ...")
                ManifestWriter.ApplyManifest(Parameters.InputFile, Parameters.TaskAccept.Manifest.NewRequested)
            End If
        End Sub

        Public Sub VersionInfosTask()
            If Parameters.TaskAccept.VersionInfos.Enabled Then
                m_bgw.ReportProgress(25, "Version Infos patching ...")
                Injector.InjectAssemblyVersionInfos(Parameters.InputFile, Parameters.TaskAccept.VersionInfos)
            End If
        End Sub

        Public Sub IconChangerTask()
            If Parameters.TaskAccept.IconChanger.Enabled Then
                m_bgw.ReportProgress(28, "Icon Changing ...")
                Replacer.ReplaceFromIcon(Parameters.InputFile, Parameters.TaskAccept.IconChanger.NewIcon)
                Injector.InjectAssemblyVersionInfos(Parameters.InputFile, Parameters.TaskAccept.VersionInfos)
            End If
        End Sub

        Public Sub ObfuscationTask()
            'The order of execution of the tasks must not be changed or the behavior of the various protections must be modified.
            If Parameters.TaskAccept.Obfuscation.Enabled Then
                Dim conf As New Configurator(m_bgw, Parameters)
                conf.Add(New ResourcesContentRenaming(Context))
                conf.Add(New NumericEncryption(Context))
                conf.Add(New BoolEncryption(Context))
                conf.Add(New PreAntiTamper(Context))
                conf.Add(New AntiDebug(Context))
                conf.Add(New AntiDumper(Context))
                conf.Add(New AntiIlDasm(Context))
                conf.Add(New PreCompressResources(Context))
                conf.Add(New HidePinvokeCalls(Context))
                conf.Add(New StringEncryption(Context))
                conf.Add(New Scattering(Context))
                conf.Add(New Renaming(Context))
                conf.Add(New PostCompressResources(Context))
                conf.Add(New ControlFlow(Context))
                conf.Add(New ConstantsReplacement(Context))
                conf.Add(New Watermark(Context))
                conf.Add(New PostAntiTamper(Context))

                conf.Execute()
            End If
        End Sub

        Public Sub PackerTask()
            'The order of execution of the tasks must not be changed or the behavior of the various protections must be modified.
            If Parameters.TaskAccept.Packer.Enabled Then
                Dim conf As New Configurator(m_bgw, Parameters)
                conf.Add(New PackerStubCreation(Context))
                conf.Add(New EmbedDependencies(Context, True))
                conf.Add(New NumericEncryption(Context, True))
                conf.Add(New BoolEncryption(Context, True))
                conf.Add(New PreAntiTamper(Context, True))
                conf.Add(New AntiDebug(Context, True))
                conf.Add(New AntiDumper(Context, True))
                conf.Add(New AntiIlDasm(Context, True))
                conf.Add(New PreCompressResources(Context, True))
                conf.Add(New HidePinvokeCalls(Context, True))
                conf.Add(New StringEncryption(Context, True))
                conf.Add(New Scattering(Context, True))
                conf.Add(New Renaming(Context, True))
                conf.Add(New PostCompressResources(Context, True))
                conf.Add(New ControlFlow(Context, True))
                conf.Add(New ConstantsReplacement(Context, True))
                conf.Add(New Watermark(Context, True))
                conf.Add(New PostAntiTamper(Context, True))

                conf.Execute()
            End If
        End Sub

        Public Sub FinalizeTask()
            m_bgw.ReportProgress(99, "Finalizing Task ...")
            Try
                If Context.IsCountedTask Then
                    Dim files = Directory.GetFiles(Context.TempProtectedPath)
                    Dim lst = files.Where(Function(f) Regex.IsMatch(Path.GetFileNameWithoutExtension(f), "^\d+$")).Select(Of Integer)(Function(g) Path.GetFileNameWithoutExtension(g))
                    Dim lMax = lst.Max

                    File.Copy(Context.TempProtectedPath & "\" & lMax & ".exe", Context.ProtectedPath & "\" & New FileInfo(Parameters.CurrentFile).Name, True)
                Else
                    File.Copy(Context.TempProtectedPath & "\" & New FileInfo(Parameters.CurrentFile).Name, Context.ProtectedPath & "\" & New FileInfo(Parameters.CurrentFile).Name, True)
                End If

                Context.ProtectedFilePath = Context.ProtectedPath & "\" & New FileInfo(Parameters.CurrentFile).Name
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

        ''' <summary>
        ''' INFO : Clear the randomize names from the dictionary.
        ''' </summary>
        Public Sub Clean()
            Context.Randomizer.CleanUp()
            Mapping.CleanUp()
            EmptyTemp()
            Parameters.TaskAccept.CleanUp()
            If Not m_Dependencies Is Nothing Then m_Dependencies.CleanUp()
        End Sub
#End Region

    End Class

End Namespace
