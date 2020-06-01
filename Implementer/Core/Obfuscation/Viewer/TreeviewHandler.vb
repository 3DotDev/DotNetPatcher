Imports System.Windows.Forms
Imports Mono.Cecil
Imports Helper.CecilHelper

Namespace Core.Obfuscation.Viewer
    Public NotInheritable Class TreeviewHandler

#Region " Fields "
        Private m_AssDef As AssemblyDefinition = Nothing
#End Region

#Region " Properties "
        Public Property Filepath() As String
#End Region

#Region " Constructor "
        Sub New(FilePath$)
            _Filepath = FilePath$
        End Sub
#End Region

#Region " Methods "
        Public Function LoadTreeNode() As TreeNode
            m_AssDef = AssemblyDefinition.ReadAssembly(_Filepath)

            Dim assNode As New TreeNode(m_AssDef.FullName)
            assNode.ExpandAll()
            SetImageKey(assNode, "assembly.png")

            Dim namespaces As New Dictionary(Of String, TreeNode)

            For Each Mdef In m_AssDef.Modules
                Dim libNode As New TreeNode(Mdef.Name)
                libNode.ExpandAll()
                SetImageKey(libNode, "library.png")
                For Each tDef In Mdef.Types
                    Dim tNode As TreeNode
                    If Not namespaces.ContainsKey(tDef.Namespace) Then
                        tNode = New TreeNode(tDef.Namespace)
                        SetImageKey(tNode, "namespace.png")
                        namespaces.Add(tDef.Namespace, tNode)
                        libNode.Nodes.Add(tNode)
                    Else
                        tNode = namespaces.Item(tDef.Namespace)
                    End If
                    If (tNode.Text = tDef.Namespace) Then
                        Dim destNode As New TreeNode(tDef.Name)
                        For Each ntDef In tDef.NestedTypes
                            Dim ntNode As New TreeNode(ntDef.Name)
                            CreateMembers(ntDef, ntNode)
                            destNode.Nodes.Add(ntNode)
                        Next
                        CreateMembers(tDef, destNode)
                        tNode.Nodes.Add(destNode)
                    Else
                        Continue For
                    End If
                Next
                assNode.Nodes.Add(libNode)
            Next
            namespaces.Clear()
            Return assNode
        End Function

        Private Sub CreateMembers(ByRef OriginalType As TypeDefinition, ByRef DestNode As TreeNode)

            SetImageKey(DestNode, GetTypeImage(OriginalType))

            For Each mDef As MethodDefinition In OriginalType.Methods
                If Not Finder.AccessorMethods(OriginalType).Contains(mDef) Then
                    CreateMethodNode(mDef, DestNode)
                End If
            Next

            For Each fieldDef In OriginalType.Fields
                Dim fieldNode = New TreeNode(fieldDef.Name.ToString & " : " & fieldDef.FieldType.Name)
                SetImageKey(fieldNode, "field.png")

                DestNode.Nodes.Add(fieldNode)
            Next

            For Each propDef In OriginalType.Properties
                Dim propNode = New TreeNode(propDef.Name.ToString & " : " & propDef.PropertyType.Name)
                SetImageKey(propNode, "property.png")

                If Not propDef.GetMethod Is Nothing Then CreateMethodNode(propDef.GetMethod, propNode)
                If Not propDef.SetMethod Is Nothing Then CreateMethodNode(propDef.SetMethod, propNode)

                For Each def In propDef.OtherMethods
                    CreateMethodNode(def, propNode)
                Next

                DestNode.Nodes.Add(propNode)
            Next

            For Each EventDef In OriginalType.Events
                Dim eventNode = New TreeNode(EventDef.Name)
                SetImageKey(eventNode, "event.png")

                If Not EventDef.AddMethod Is Nothing Then CreateMethodNode(EventDef.AddMethod, eventNode)
                If Not EventDef.RemoveMethod Is Nothing Then CreateMethodNode(EventDef.RemoveMethod, eventNode)

                For Each def In EventDef.OtherMethods
                    CreateMethodNode(def, eventNode)
                Next

                DestNode.Nodes.Add(eventNode)
            Next
        End Sub

        Private Sub CreateMethodNode(mDef As MethodDefinition, DestNode As TreeNode)
            Dim methodNode As New TreeNode(mDef.Name)
            SetImageKey(methodNode, GetMethodImage(mDef))

            Dim tmpStr As String = Nothing

            For Each paramDef In mDef.Parameters
                If Not paramDef.ParameterType.Name = mDef.DeclaringType.Name Then
                    tmpStr &= String.Concat(paramDef.ParameterType.Name, ",")
                End If
            Next

            methodNode.Text &= String.Concat("(", (tmpStr?.TrimEnd(New Char() {","c, " "c})), ")")
            DestNode.Nodes.Add(methodNode)
        End Sub

        Private Function GetMethodImage(mdef As MethodDefinition) As String
            Dim str = "Method.png"
            If mdef.IsConstructor Then
                str = "Constructor.png"
            ElseIf mdef.IsPInvokeImpl Then
                str = "PInvokeMethod.png"
            End If
            Return str
        End Function

        Private Function GetTypeImage(mdef As TypeDefinition) As String
            Dim str = "class.png"
            If mdef.IsInterface Then
                str = "interface.png"
            ElseIf mdef.IsEnum Then
                str = "enum.png"
            ElseIf mdef.IsValueType Then
                str = "enumvalue.png"
            ElseIf (mdef.BaseType IsNot Nothing) AndAlso (mdef.BaseType.Name.ToLower.Contains("delegate")) Then
                str = "delegate.png"
            ElseIf mdef.IsSealed Then
                str = "staticclass.png"
            End If
            Return str
        End Function

        Private Sub SetImageKey(node As TreeNode, imageKey$)
            node.ImageKey = imageKey
            node.SelectedImageKey = imageKey
        End Sub
#End Region

    End Class
End Namespace
