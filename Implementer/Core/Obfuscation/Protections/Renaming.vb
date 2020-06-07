Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Mono.Cecil.Rocks
Imports Implementer.Engine.Context
Imports Implementer.Engine.Processing

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class Renaming
        Inherits Protection

#Region " Fields "
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Renaming assembly...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Renamer"
            End Get
        End Property

        Public Overrides Property Context As ProtectionContext

        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property ProgressIncrement As Integer
            Get
                Return 78
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If HasRenameTask() Then
                _Enabled = True
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()

            Dim InheritMap As New OverridesMap
            For Each modul In Context.InputAssembly.Modules
                If modul.HasTypes Then
                    For Each type In modul.GetAllTypes
                        InheritMap.AnalyzeTree(type)
                    Next
                End If
            Next

            Dim assemblyMainName$ = Context.InputAssembly.EntryPoint.DeclaringType.Namespace
            For Each modul In Context.InputAssembly.Modules
                If modul.HasTypes Then
                    For Each type In modul.GetAllTypes
                        RenameSelectedNamespace(type, assemblyMainName, InheritMap)
                    Next
                End If
            Next
        End Sub

        Private Function HasRenameTask()
            With Context.Params
                If .TaskAccept.Obfuscation.Enabled AndAlso
                (.RenamingAccept.Namespaces OrElse
                .RenamingAccept.Types OrElse
                .RenamingAccept.Methods OrElse
                .RenamingAccept.Fields OrElse
                .RenamingAccept.Events OrElse
                .RenamingAccept.Properties OrElse
                .TaskAccept.Obfuscation.RenameResourcesContent OrElse
                .RenamingAccept.CustomAttributes) Then
                    Return True
                End If
            End With
            Return False
        End Function

        ''' <summary>
        ''' INFO : Rename the main Namespace or all namespaces according to Cls_Parameters.RenameMainNamespaceSetting setting.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="assemblyMainName"></param>
        ''' <param name="processing"></param>
        Private Sub RenameSelectedNamespace(type As TypeDefinition, assemblyMainName$, InheritMap As OverridesMap)
            If Context.Params.RenamingAccept.RenameMainNamespaceSetting And RenamerState.RenameMainNamespace.Only Then
                If type.Namespace.StartsWith(assemblyMainName) Then
                    ProcessType(type)
                End If
            Else
                ProcessType(type)
            End If

            If type.HasFields Then ProcessFields(type)
            If type.HasProperties Then ProcessProperties(type)
            If type.HasMethods Then ProcessMethods(type, InheritMap)
            If type.HasEvents Then ProcessEvents(type)
        End Sub


        ''' <summary>
        ''' INFO : This is the beginning of the renamer method ! Namespaces, Types and Resources renaming.
        ''' </summary>
        ''' <param name="type"></param>
        Private Sub ProcessType(type As TypeDefinition)

            RemoveUselessCustomAttributes(type)

            Dim NamespaceOriginal As String = type.Namespace
            Dim NamespaceObfuscated As String = type.Namespace

            If Not type.Name = "<Module>" Then
                If Context.Params.RenamingAccept.Namespaces Then
                    NamespaceObfuscated = If(Context.Params.RenamingAccept.ReplaceNamespacesSetting And RenamerState.ReplaceNamespaces.Empty,
                                       String.Empty, If(Context.Params.RenamingAccept.NamespaceSerialized, Context.Randomizer.GenerateNewAlphabetic(), Context.Randomizer.GenerateNew()))
                    type.Namespace = Mapping.RenameTypeDef(type, NamespaceObfuscated, True)
                End If
            End If

            If NameChecker.IsRenamable(type) Then
                Dim TypeOriginal As String = type.Name
                Dim TypeObfuscated As String = type.Name

                If Context.Params.RenamingAccept.Types Then
                    type.Name = Mapping.RenameTypeDef(type, Context.Randomizer.GenerateNew())
                    TypeObfuscated = type.Name
                    Renamer.RenameResources(type, NamespaceOriginal, NamespaceObfuscated, TypeOriginal, TypeObfuscated)
                End If

                If Context.Params.RenamingAccept.Namespaces Then
                    type.Namespace = Mapping.RenameTypeDef(type, NamespaceObfuscated, True)
                    Renamer.RenameResources(type, NamespaceOriginal, NamespaceObfuscated, TypeOriginal, TypeObfuscated)
                End If

                If Context.Params.RenamingAccept.Properties Then Renamer.RenameResourceManager(type, Context.Randomizer)

                If Context.Params.RenamingAccept.Types OrElse Context.Params.RenamingAccept.Namespaces Then
                    Renamer.RenameInitializeComponentsValues(type, TypeOriginal, TypeObfuscated, False)
                End If
            End If
        End Sub

        Private Sub ProcessVirtualMethod(mDef As MethodDefinition, InheritM As OverridesMap)
            Dim originalMeth As String = mDef.Name
            Dim MethodPublicObf = Context.Randomizer.GenerateNew()
            For Each a In InheritM.OverridesMethods(mDef)
                Renamer.RenameVirtualMethod(a, MethodPublicObf)
            Next
            Renamer.RenameVirtualMethod(mDef, MethodPublicObf)
        End Sub
        ''' <summary>
        ''' INFO : Methods, Parameters renamer routine.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="InheritM"></param>
        Private Sub ProcessMethods(type As TypeDefinition, InheritM As OverridesMap)
            If Context.Params.RenamingAccept.Methods Then
                For Each mDef As MethodDefinition In type.Methods
                    RemoveUselessCustomAttributes(mDef)

                    If mDef.IsVirtual AndAlso mDef.IsPublic = False Then
                        'Processing only public virtuals first
                        If InheritM.OverridesMethods.ContainsKey(mDef) Then
                            Dim tdef = mDef.DeclaringType
                            If InheritM.ContainsKey(tdef) Then
                                Dim bTypes = InheritM(tdef)
                                If bTypes.Count = 0 Then
                                    ProcessVirtualMethod(mDef, InheritM)
                                Else
                                    Dim containsT = bTypes.Any(Function(t) t.Methods.Any(Function(m) Utils.MethodMatch(m, mDef)))
                                    If containsT = False Then
                                        ProcessVirtualMethod(mDef, InheritM)
                                    End If
                                End If
                            Else
                                ProcessVirtualMethod(mDef, InheritM)
                            End If
                        End If
                    ElseIf Not mDef.IsVirtual Then
                        'Processing non-virtuals
                        If NameChecker.IsRenamable(mDef) Then
                            If Not Finder.AccessorMethods(mDef.DeclaringType).Contains(mDef) Then
                                'non-accessors
                                Dim originalMeth As String = mDef.Name
                                Dim MethodPublicObf = Context.Randomizer.GenerateNew()
                                Renamer.RenameMethod(mDef, MethodPublicObf)
                            ElseIf Finder.AccessorMethods(mDef.DeclaringType).Contains(mDef) AndAlso mDef.IsSpecialName Then
                                'accessors with certain conditions
                                Dim originalMeth As String = mDef.Name
                                Dim MethodPublicObf = Context.Randomizer.GenerateNew()
                                If mDef.IsSetter OrElse mDef.IsGetter Then
                                    If Not mDef.HasCustomAttributes Then
                                        Dim prop = mDef.DeclaringType.Properties.Where(Function(f) (f.GetMethod IsNot Nothing AndAlso f.GetMethod.Name = mDef.Name) OrElse (f.SetMethod IsNot Nothing AndAlso f.SetMethod.Name = mDef.Name)).FirstOrDefault
                                        If Not prop Is Nothing Then
                                            If prop.GetMethod IsNot Nothing AndAlso prop.SetMethod IsNot Nothing Then
                                                Renamer.RenameMethod(mDef, MethodPublicObf)
                                            Else
                                                If mDef.IsGetter Then
                                                    prop = mDef.DeclaringType.Properties.Where(Function(f) f.GetMethod IsNot Nothing AndAlso f.GetMethod.Name = mDef.Name).FirstOrDefault
                                                    If Not MemberExtensions.HasCustomAttributeByMemberName(prop, "DebuggerNonUserCodeAttribute") Then
                                                        mDef.SemanticsAttributes = MethodSemanticsAttributes.None
                                                        mDef.DeclaringType.Properties.Remove(prop)
                                                    End If
                                                    Renamer.RenameMethod(mDef, MethodPublicObf)
                                                Else
                                                    Renamer.RenameMethod(mDef, MethodPublicObf)
                                                End If
                                            End If
                                        End If
                                    Else
                                        Renamer.RenameMethod(mDef, MethodPublicObf)
                                    End If
                                Else
                                    Renamer.RenameMethod(mDef, MethodPublicObf)
                                End If
                            End If
                        End If
                    End If
                    ProcessParameters(mDef)
                Next
            End If
        End Sub

        Private Sub RemoveUselessCustomAttributes(member As IMemberDefinition)
            If Context.Params.RenamingAccept.CustomAttributes Then
                Utils.RemoveCustomAttributeByName(member, "ObsoleteAttribute")
                Utils.RemoveCustomAttributeByName(member, "DescriptionAttribute")
                Utils.RemoveCustomAttributeByName(member, "CategoryAttribute")
            End If
        End Sub

        ''' <summary>
        ''' INFO : Fields renamer routine. 
        ''' </summary>
        ''' <param name="type"></param>
        Private Sub ProcessFields(type As TypeDefinition)
            If Context.Params.RenamingAccept.Fields Then
                For Each fDef As FieldDefinition In type.Fields
                    If NameChecker.IsRenamable(fDef) Then
                        Dim originalN = fDef.Name
                        Dim obfuscatedN = Context.Randomizer.GenerateNew()
                        Renamer.RenameFields(fDef, originalN, obfuscatedN, Context.Randomizer)
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' INFO : Properties, CustomAttributes (Only "AccessedThroughPropertyAttribute" attribute) renamer routine. 
        ''' </summary>
        ''' <param name="type"></param>
        Private Sub ProcessProperties(type As TypeDefinition)
            If Context.Params.RenamingAccept.Properties Then
                For Each propDef As PropertyDefinition In type.Properties
                    If NameChecker.IsRenamable(propDef) Then
                        Dim originalN = propDef.Name
                        Dim obfuscatedN = Context.Randomizer.GenerateNew()
                        Renamer.RenameProperties(propDef, originalN, obfuscatedN, Context.Params.RenamingAccept.Fields, Context.Randomizer)
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' INFO : Events renamer routine. 
        ''' </summary>
        ''' <param name="type"></param>
        Private Sub ProcessEvents(type As TypeDefinition)
            If Context.Params.RenamingAccept.Events Then
                For Each events As EventDefinition In type.Events
                    If NameChecker.IsRenamable(events) Then
                        Dim obfName As String = Context.Randomizer.GenerateNew()
                        Renamer.RenameEvent(events, obfName)
                    End If
                Next
            End If
        End Sub

        Private Sub ProcessParameters(Meth As MethodDefinition)
            Renamer.RenameParameters(Meth, Context.Randomizer)
        End Sub
#End Region

    End Class
End Namespace
