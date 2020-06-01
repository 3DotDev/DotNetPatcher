Imports Implementer.Core.Versions
Imports Implementer.Core.ManifestRequest
Imports Implementer.Core.IconChanger
Imports Implementer.Core.Packer
Imports Implementer.Core.Dependencing
Imports Implementer.Core.Obfuscation.Protections

Namespace Engine.Context
    Public NotInheritable Class TaskState

#Region " Fields "
        Public DllReferences As DependenciesInfos
        Public Manifest As ManifestInfos
        Public IconChanger As IconInfos
        Public Obfuscation As ObfuscationInfos
        Public Packer As PackInfos
        Public VersionInfos As Infos
#End Region

#Region " Methods "
        Public Sub CleanUp()
            If Not DllReferences Is Nothing Then DllReferences.Dispose()
            If Not Manifest Is Nothing Then Manifest.Dispose()
            If Not IconChanger Is Nothing Then IconChanger.Dispose()
        End Sub
#End Region

    End Class

End Namespace

