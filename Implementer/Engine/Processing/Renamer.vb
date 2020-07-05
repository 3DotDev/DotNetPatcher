Imports Mono.Cecil
Imports Mono.Cecil.Cil
Imports Mono.Cecil.Rocks
Imports Helper.RandomizeHelper
Imports Helper.CecilHelper

Namespace Engine.Processing
    ''' <summary>
    ''' INFO : This is the forth step of the renamer library. 
    '''        This is the core of the rename library !
    ''' </summary>
    Friend NotInheritable Class Renamer

#Region " Methods "

        Friend Shared Function RenameMethod(method As MethodDefinition, obfName As String) As MethodDefinition
            Dim MethodOriginal = method.Name
            Dim MethodFullname = method.FullName

            If method.IsPInvokeImpl Then
                If method.PInvokeInfo.EntryPoint = String.Empty Then method.PInvokeInfo.EntryPoint = MethodOriginal
            End If

            UpdateBodyCalls(method, obfName)
            method.Name = Mapping.RenameMethodMember(method, obfName)
            Return method
        End Function

        Friend Shared Function RenameVirtualMethod(method As MethodDefinition, obfName As String) As MethodDefinition
            Dim MethodOriginal = method.Name
            Dim MethodFullname = method.FullName

            UpdateBodyCalls(method, obfName)
            method.Name = Mapping.RenameVirtualMethodMember(method, obfName)
            Return method
        End Function

        Private Shared Sub UpdateBodyCalls(Method As MethodDefinition, ObfName As String)
            For Each mo In Method.Module.Assembly.Modules
                For Each t In mo.GetAllTypes
                    For Each m In t.Methods
                        If Not Utils.MethodMatch(m, Method) Then
                            If m.HasBody Then
                                For Each instruct In m.Body.Instructions
                                    If instruct.Operand IsNot Nothing AndAlso TypeOf instruct.Operand Is MethodReference Then
                                        Dim mr = TryCast(instruct.Operand, MethodReference)
                                        If Not mr Is Nothing Then
                                            If TypeExtensions.TypeMatches(Method.DeclaringType, mr.DeclaringType) Then
                                                If Utils.MethodMatch(Method, mr) Then
                                                    mr.GetElementMethod.Name = ObfName
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                Next
            Next
        End Sub

        Private Shared Sub UpdateBodyCalls(fDef As FieldDefinition, OriginalN As String, ObfName As String)
            For Each mo In fDef.Module.Assembly.Modules
                For Each t In mo.GetAllTypes
                    For Each m In t.Methods
                        If m.HasBody Then
                            For Each instruct In m.Body.Instructions
                                If TypeOf instruct.Operand Is FieldReference Then
                                    Dim mr = TryCast(instruct.Operand, FieldReference)
                                    If Not mr Is Nothing Then
                                        If String.Equals(mr.Name, OriginalN, StringComparison.InvariantCultureIgnoreCase) Then
                                            If TypeExtensions.TypeMatches(fDef.DeclaringType, mr.DeclaringType) Then
                                                If Utils.AreSame(fDef.FieldType, mr.FieldType) Then
                                                    mr.Name = ObfName
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next
                Next
            Next
        End Sub

        Private Shared Sub RenameControlName(type As TypeDefinition, OriginalN As String, ObfName As String)
            For Each mo In type.Module.Assembly.Modules
                For Each t In mo.GetAllTypes
                    If NameChecker.IsRenamable(t) Then
                        For Each m In t.Methods
                            If NameChecker.IsRenamable(m) Then
                                If Not m.Name = "InitializeComponent" Then
                                    If m.HasBody Then
                                        For Each instruct In m.Body.Instructions
                                            If instruct.OpCode = OpCodes.Ldstr Then
                                                If TypeOf instruct.Operand Is String Then
                                                    Dim str As String = TryCast(instruct.Operand, String)
                                                    If str = OriginalN Then
                                                        instruct.Operand = ObfName
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Next
        End Sub

        ''' <summary>
        ''' INFO : Rename Parameters from method.
        ''' </summary>
        ''' <param name="method"></param>
        Friend Shared Sub RenameParameters(method As MethodDefinition, Randomizer As Randomizer)
            If method.HasParameters Then
                For Each ParDef As ParameterDefinition In method.Parameters
                    If ParDef.CustomAttributes.Count = 0 Then
                        ParDef.Name = Mapping.RenameParamMember(ParDef, Randomizer.GenerateNew())
                    End If
                Next
            End If
        End Sub


        ''' <summary>
        ''' INFO : Rename embedded Resources from Resources dir and updates method bodies.
        ''' </summary>
        ''' <param name="TypeDef"></param>
        ''' <param name="NamespaceOriginal"></param>
        ''' <param name="NamespaceObfuscated"></param>
        ''' <param name="TypeOriginal"></param>
        ''' <param name="TypeObfuscated"></param>
        Friend Shared Sub RenameResources(TypeDef As TypeDefinition, ByRef NamespaceOriginal$, ByRef NamespaceObfuscated$, TypeOriginal$, TypeObfuscated$)
            Dim ModuleDef As ModuleDefinition = TypeDef.Module

            For Each EmbRes As Resource In ModuleDef.Resources
                If Utils.IsStronglyTypedResourceBuilder(TypeDef) Then
                    If NamespaceOriginal.EndsWith(".My.Resources") Then
                        If EmbRes.Name = NamespaceOriginal.Replace(".My.Resources", "") & "." & TypeOriginal & ".resources" Then
                            RenameResourceName(EmbRes, NamespaceObfuscated, TypeObfuscated)
                        End If
                    Else
                        If EmbRes.Name = NamespaceOriginal & "." & TypeOriginal & ".resources" Then
                            RenameResourceName(EmbRes, NamespaceObfuscated, TypeObfuscated)
                        End If
                    End If
                Else
                    If EmbRes.Name = NamespaceOriginal & "." & TypeOriginal & ".resources" Then
                        RenameResourceName(EmbRes, NamespaceObfuscated, TypeObfuscated)
                    End If
                End If
            Next

            Dim types = ModuleDef.Types
            For Each td In types
                If td.HasMethods Then
                    For Each method In TypeDef.Methods
                        If method.HasBody Then
                            For Each inst In method.Body.Instructions
                                If inst.OpCode = OpCodes.Ldstr Then
                                    If NamespaceOriginal.EndsWith(".My.Resources") Then
                                        If inst.Operand.ToString() = (NamespaceOriginal.Replace(".My.Resources", "") & ".Resources") Then
                                            inst.Operand = If(NamespaceObfuscated = String.Empty, TypeObfuscated, NamespaceObfuscated & "." & TypeObfuscated)
                                        End If
                                    Else
                                        If inst.Operand.ToString() = (NamespaceOriginal & "." & TypeOriginal) Then
                                            inst.Operand = If(NamespaceObfuscated = String.Empty, TypeObfuscated, NamespaceObfuscated & "." & TypeObfuscated)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        End Sub

        Private Shared Sub RenameResourceName(EmbRes As Resource, NamespaceObfuscated As String, TypeObfuscated As String)
            EmbRes.Name = If(NamespaceObfuscated = String.Empty, TypeObfuscated & ".resources", NamespaceObfuscated & "." & TypeObfuscated & ".resources")
        End Sub

        ''' <summary>
        ''' INFO : Rename embedded Resources from Resources dir and from ResourcesManager method.
        ''' </summary>
        ''' <param name="typeDef"></param>
        Friend Shared Sub RenameResourceManager(typeDef As TypeDefinition, Randomizer As Randomizer)
            For Each pr In (From p In typeDef.Properties
                            Where Not p.GetMethod Is Nothing AndAlso p.GetMethod.Name = "get_ResourceManager" AndAlso p.GetMethod.HasBody AndAlso p.GetMethod.Body.Instructions.Count <> 0
                            Select p)
                For Each instruction In pr.GetMethod.Body.Instructions
                    If TypeOf instruction.Operand Is String Then
                        Dim NewResManagerName$ = instruction.Operand
                        For Each EmbRes As EmbeddedResource In typeDef.Module.Resources
                            If EmbRes.Name = instruction.Operand & ".resources" Then
                                NewResManagerName = Randomizer.GenerateNew()
                                EmbRes.Name = NewResManagerName & ".resources"
                            End If
                        Next
                        instruction.Operand = NewResManagerName
                    End If
                Next
            Next
        End Sub

        Friend Shared Sub RenameSettings(mDef As MethodDefinition, originalN$, obfuscatedN$)
            If Not mDef Is Nothing Then
                If Not mDef.DeclaringType.BaseType Is Nothing AndAlso mDef.DeclaringType.BaseType.Name = "ApplicationSettingsBase" Then
                    If mDef.HasBody AndAlso mDef.Body.Instructions.Count <> 0 Then
                        For Each instruction In mDef.Body.Instructions
                            If TypeOf instruction.Operand Is String Then
                                Dim Name$ = instruction.Operand
                                If originalN = Name Then
                                    If mDef.Name.StartsWith("set_") Then
                                        mDef.Name = "set_" & obfuscatedN
                                    ElseIf mDef.Name.StartsWith("get_") Then
                                        mDef.Name = "get_" & obfuscatedN
                                    End If
                                    instruction.Operand = obfuscatedN
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' INFO : Rename Property.
        ''' </summary>
        ''' <param name="prop"></param>
        ''' <param name="obfuscatedN"></param>
        Friend Shared Sub RenameProperty(ByRef prop As PropertyDefinition, obfuscatedN$)
            prop.Name = Mapping.RenamePropertyMember(prop, obfuscatedN)
        End Sub

        Friend Shared Sub RenameProperties(propDef As PropertyDefinition, originalN As String, obfuscatedN As String, ExcludeFields As Boolean, Randomizer As Randomizer)
            Dim fDef = Finder.TryGetField(propDef.DeclaringType, propDef)
            If Not fDef Is Nothing Then
                If ExcludeFields = False Then
                    If MemberExtensions.HasCustomAttributeByMemberName(fDef, "AccessedThroughPropertyAttribute") Then
                        For Each ca In (From c In fDef.CustomAttributes
                                        Where c.AttributeType.Name = "AccessedThroughPropertyAttribute" AndAlso c.HasConstructorArguments AndAlso c.ConstructorArguments(0).Value = propDef.Name
                                        Select c)
                            ca.ConstructorArguments(0) = New CustomAttributeArgument(ca.AttributeType, obfuscatedN)
                            Exit For
                        Next
                        RenameControlName(propDef.DeclaringType, originalN, obfuscatedN)
                        RenameInitializeComponentsValues(propDef.DeclaringType, originalN, obfuscatedN, True)
                        RenameProperties(propDef, Randomizer.GenerateNew(), Randomizer)
                    End If
                Else
                    RenameProperties(propDef, obfuscatedN, Randomizer)
                End If
            Else
                RenameProperties(propDef, obfuscatedN, Randomizer)
            End If
        End Sub

        Private Shared Sub RenameProperties(propDef As PropertyDefinition, obfuscatedN As String, Randomizer As Randomizer)
            RenameSettings(propDef.GetMethod, propDef.Name, obfuscatedN)
            RenameSettings(propDef.SetMethod, propDef.Name, obfuscatedN)
            RenameProperty(propDef, Randomizer.GenerateNew())
        End Sub

        ''' <summary>
        ''' INFO : Rename Field.
        ''' </summary>
        ''' <param name="field"></param>
        ''' <param name="obfuscatedN"></param>
        Friend Shared Sub RenameField(field As FieldDefinition, obfuscatedN$)
            field.Name = Mapping.RenameFieldMember(field, obfuscatedN)
        End Sub

        Friend Shared Sub RenameFields(field As FieldDefinition, originalN As String, obfuscatedN As String, Randomizer As Randomizer)
            If MemberExtensions.HasCustomAttributeByMemberName(field, "AccessedThroughPropertyAttribute") Then
                Dim pDef = Finder.TryGetProperty(field.DeclaringType, field)
                Dim isValid As Boolean
                For Each ca In (From c In field.CustomAttributes
                                Where c.AttributeType.Name = "AccessedThroughPropertyAttribute" AndAlso c.HasConstructorArguments AndAlso c.ConstructorArguments(0).Value = pDef.Name
                                Select c)
                    ca.ConstructorArguments(0) = New CustomAttributeArgument(ca.AttributeType, obfuscatedN)
                    isValid = True
                    Exit For
                Next
                If isValid Then
                    RenameControlName(field.DeclaringType, pDef.Name, obfuscatedN)
                    RenameInitializeComponentsValues(field.DeclaringType, pDef.Name, obfuscatedN, True)
                    RenameSettings(pDef.GetMethod, pDef.Name, obfuscatedN)
                    RenameSettings(pDef.SetMethod, pDef.Name, obfuscatedN)
                    RenameProperty(pDef, obfuscatedN)
                    RenameField(field, Randomizer.GenerateNew())
                End If
            Else
                UpdateBodyCalls(field, originalN, obfuscatedN)
                field.Name = Mapping.RenameFieldMember(field, obfuscatedN)
            End If
        End Sub

        ''' <summary>
        ''' INFO : Rename Event.
        ''' </summary>
        ''' <param name="events"></param>
        ''' <param name="obfuscatedN"></param>
        Friend Shared Sub RenameEvent(ByRef events As EventDefinition, obfuscatedN$)
            events.Name = Mapping.RenameEventMember(events, obfuscatedN)
        End Sub

        Friend Shared Sub RenameInitializeComponentsValues(TypeDef As TypeDefinition, OriginalKeyName$, NewKeyName$, Properties As Boolean)
            Dim methodSearch As MethodDefinition = Finder.FindMethod(TypeDef, "InitializeComponent")
            If Not methodSearch Is Nothing Then
                If methodSearch.HasBody Then
                    If methodSearch.Body.Instructions.Count <> 0 Then
                        For Each instruction As Instruction In methodSearch.Body.Instructions
                            If TypeOf instruction.Operand Is String Then
                                If Properties Then
                                    RenameInitializeComponentsGetName(instruction, OriginalKeyName, NewKeyName)
                                    RenameInitializeComponentsSetName(instruction, OriginalKeyName, NewKeyName)
                                Else
                                    RenameInitializeComponentsSetName(instruction, OriginalKeyName, NewKeyName)
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        Friend Shared Sub RenameInitializeComponentsGetName(instruction As Instruction, OriginalKeyName$, NewKeyName$)
            If Not instruction.Previous Is Nothing Then
                If instruction.Previous.OpCode = OpCodes.Callvirt AndAlso instruction.Previous.Operand.ToString.EndsWith("get_" & OriginalKeyName & "()") Then
                    If CStr(instruction.Operand) = OriginalKeyName Then
                        instruction.Operand = NewKeyName
                    End If
                End If
            End If
        End Sub

        Friend Shared Sub RenameInitializeComponentsSetName(instruction As Instruction, OriginalKeyName$, NewKeyName$)
            If Not instruction.Next Is Nothing Then
                If (instruction.Next.OpCode = OpCodes.Callvirt OrElse instruction.Next.OpCode = OpCodes.Call) AndAlso instruction.Next.ToString.EndsWith("set_Name(System.String)") Then
                    If CStr(instruction.Operand) = OriginalKeyName Then
                        instruction.Operand = NewKeyName
                    End If
                End If
            End If
        End Sub
#End Region

    End Class
End Namespace
