Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Mono.Cecil.Rocks
Imports Mono.Cecil.Cil
Imports Helper.RandomizeHelper
Imports Helper.UtilsHelper

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class ConstantsReplacement
        Inherits Protection

#Region " Private class "
        Private Class InitFields
            Inherits List(Of Integer)

            Private ReadOnly TypeDef As TypeDefinition
            Friend Property TheField As FieldDefinition
            Friend Property StaticCtor As MethodDefinition
            Friend ReadOnly Property CctorAlreadyExists As Boolean
            Public Property IntCount As Integer
            Private Property Conform As Boolean

            Private ReadOnly Randomizer As Randomizer

            Friend ReadOnly Property IsConform As Boolean
                Get
                    Return Conform
                End Get
            End Property

            Friend Sub New(td As TypeDefinition, Randomize As Randomizer)
                TypeDef = td
                Randomizer = Randomize
                Conform = td.IsBeforeFieldInit = False AndAlso td.HasGenericParameters = False
                If Conform Then
                    _StaticCtor = td.GetStaticConstructor
                    If _StaticCtor Is Nothing Then
                        _StaticCtor = New MethodDefinition(".cctor", MethodAttributes.Private Or MethodAttributes.SpecialName Or MethodAttributes.RTSpecialName Or MethodAttributes.Static, td.Module.TypeSystem.Void)
                    Else
                        _CctorAlreadyExists = True
                    End If
                End If
            End Sub

            Friend Function BodyModifier(value As Integer, il As ILProcessor, Ldci4Instruct As Instruction) As Instruction
                If value > 7 Then
                    If Conform Then
                        'make sure we have integer candidates before adding post stuff 
                        IntCount += 1
                        'add integer to list(of integer)
                        Add(Ldci4Instruct.Operand)
                        'determine current array's position for future use
                        Dim position = Count - 1
                        'create the field to call from the method body
                        If TheField Is Nothing Then
                            TheField = New FieldDefinition(Randomizer.GenerateNewInvisible, FieldAttributes.Private Or FieldAttributes.Static, TypeDef.Module.Import(GetType(Integer())))
                            il.Body.Method.DeclaringType.Fields.Add(TheField)
                        End If

                        Dim fieldInstruct = Instruction.Create(OpCodes.Ldsfld, il.Body.Method.DeclaringType.Module.Import(TheField))
                        Dim PositionInstruct = Instruction.Create(OpCodes.Ldc_I4, position)
                        Dim IndexInstruct = Instruction.Create(OpCodes.Ldelem_I4)

                        il.Replace(Ldci4Instruct, fieldInstruct)
                        il.InsertAfter(fieldInstruct, PositionInstruct)
                        il.InsertAfter(PositionInstruct, IndexInstruct)
                        il.Body.InitLocals = True

                        Return IndexInstruct
                    End If
                End If
                Return Nothing
            End Function

            Friend Sub PostDefinitionModifier()
                If Conform Then
                    If IntCount > 0 Then
                        If Not TheField Is Nothing Then
                            'Create PrivateImplementationDetails TypeDef
                            Dim privateImpl = TypeDef.Module.Types.Where(Function(t) t.Name = "<PrivateImplementationDetails>").FirstOrDefault
                            If privateImpl Is Nothing Then
                                privateImpl = New TypeDefinition("", "<PrivateImplementationDetails>", TypeAttributes.Class Or TypeAttributes.Sealed Or TypeAttributes.AutoLayout Or TypeAttributes.NotPublic Or TypeAttributes.AnsiClass, TypeDef.Module.Import(GetType(Object)))
                                TypeDef.Module.Types.Add(privateImpl)
                            End If

                            'Store integer() to Byte Array
                            Dim b = Functions.ConvertToByteArray(ToArray())

                            'Check if Structure with same Size already exists
                            Dim StaticArray = privateImpl.NestedTypes.Where(Function(t) t.Name = "__StaticArrayInitTypeSize=" & (Count * 4).ToString).FirstOrDefault
                            Dim FieldCandidate As FieldDefinition
                            If StaticArray Is Nothing Then
                                StaticArray = New TypeDefinition("", "__StaticArrayInitTypeSize=" & (Count * 4).ToString, TypeAttributes.Class Or TypeAttributes.Sealed Or TypeAttributes.ExplicitLayout Or TypeAttributes.NestedPrivate Or TypeAttributes.AnsiClass) With {
                                    .ClassSize = Count * 4,
                                    .PackingSize = 1,
                                    .BaseType = TypeDef.Module.Import(GetType(ValueType))
                                }
                                privateImpl.NestedTypes.Add(StaticArray)
                            End If

                            'Create field with FieldRVA attribute and set Byte array to InitialValue
                            FieldCandidate = privateImpl.Fields.Where(Function(f) f.FieldType.Resolve Is StaticArray AndAlso f.InitialValue Is b).FirstOrDefault
                            If FieldCandidate Is Nothing Then
                                FieldCandidate = New FieldDefinition(Randomizer.GenerateNewInvisible, FieldAttributes.Assembly Or FieldAttributes.Static Or FieldAttributes.InitOnly Or FieldAttributes.HasFieldRVA, TypeDef.Module.Import(StaticArray)) With {
                                    .InitialValue = b
                                }
                                privateImpl.Fields.Add(FieldCandidate)
                            End If

                            'Create (or not if exists) Static constructor and populate byte array size and InitializeArray call
                            Dim il = StaticCtor.Body.GetILProcessor()
                            If CctorAlreadyExists Then
                                Dim firstInstruct = StaticCtor.Body.Instructions(0)
                                Dim One = Instruction.Create(OpCodes.Ldc_I4, Count)
                                il.InsertBefore(firstInstruct, One)
                                Dim Two = Instruction.Create(OpCodes.Newarr, TypeDef.Module.Import(GetType(Integer)))
                                il.InsertAfter(One, Two)
                                Dim Three = Instruction.Create(OpCodes.Dup)
                                il.InsertAfter(Two, Three)
                                Dim Four = Instruction.Create(OpCodes.Ldtoken, TypeDef.Module.Import(FieldCandidate))
                                il.InsertAfter(Three, Four)
                                Dim FiveRuntimeHelpers = Instruction.Create(OpCodes.Call, TypeDef.Module.Import(GetType(Runtime.CompilerServices.RuntimeHelpers).GetMethod("InitializeArray", New Type() {GetType(Array), GetType(RuntimeFieldHandle)})))
                                il.InsertAfter(Four, FiveRuntimeHelpers)
                                Dim Six = Instruction.Create(OpCodes.Stsfld, TheField)
                                il.InsertAfter(FiveRuntimeHelpers, Six)
                            Else
                                il.Emit(OpCodes.Nop)
                                il.Emit(OpCodes.Ldc_I4, Count)
                                il.Emit(OpCodes.Newarr, TypeDef.Module.Import(GetType(Integer)))
                                il.Emit(OpCodes.Dup)
                                il.Emit(OpCodes.Ldtoken, TypeDef.Module.Import(FieldCandidate))
                                Dim RuntimeHelpers = TypeDef.Module.Import(GetType(Runtime.CompilerServices.RuntimeHelpers).GetMethod("InitializeArray", New Type() {GetType(Array), GetType(RuntimeFieldHandle)}))
                                il.Emit(OpCodes.Call, RuntimeHelpers)
                                il.Emit(OpCodes.Stsfld, TheField)
                                il.Emit(OpCodes.Ret)
                                TypeDef.Methods.Add(StaticCtor)
                            End If
                        End If
                    End If
                End If
            End Sub
        End Class
#End Region

#Region " Fields "
        Private ReadOnly Rand As Random
        Private ReadOnly Types As New List(Of TypeDefinition)
        Private ReadOnly PackerState As Boolean
        Private TheInitFields As InitFields
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Numeric encryption part 2...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Hide Constants"
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
                Return 95
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.EncryptNumeric Then
                _Enabled = True
                Rand = New Random
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            For Each m As ModuleDefinition In Context.InputAssembly.Modules
                Types.AddRange(m.GetAllTypes())
                For Each type As TypeDefinition In Types
                    IterateType(type)
                Next
                Types.Clear()
            Next

            For Each m As ModuleDefinition In Context.InputAssembly.Modules
                Types.AddRange(m.GetAllTypes().Where(Function(t) t.Name.StartsWith("__StaticArrayInitTypeSize=")))
                For Each type As TypeDefinition In Types
                    type.Name = Context.Randomizer.GenerateNewInvisible
                Next
                Types.Clear()
            Next
        End Sub

        Private Sub IterateType(td As TypeDefinition)
            Dim publicMethods As New List(Of MethodDefinition)()
            publicMethods.AddRange(From m In td.Methods Where (m.HasBody AndAlso
                                                            Not TypeExtensions.HasCustomAttributeByMemberName(m.DeclaringType, "EditorBrowsableAttribute") AndAlso
                                                            Not Utils.HasUnsafeInstructions(m)))
            Try
                TheInitFields = New InitFields(td, Context.Randomizer)

                For Each md In publicMethods
                    md.Body.SimplifyMacros
                    ProcessInstructions(md.Body)
                    md.Body.OptimizeMacros
                    md.Body.ComputeOffsets()
                    md.Body.ComputeHeader()
                Next

                TheInitFields.PostDefinitionModifier()

            Catch ex As Exception
            End Try
            publicMethods.Clear()
        End Sub

        Private Sub ProcessInstructions(body As MethodBody)

            Dim instructions = body.Instructions
            Dim il = body.GetILProcessor()
            Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

            For Each instruction As Instruction In instructions
                If instruction.OpCode = OpCodes.Ldc_I4 Then
                    If Utils.IsValidIntegerOperand(instruction) Then
                        instructionsToExpand.Add(instruction)
                    End If
                End If
            Next

            Dim tRef As TypeReference = Nothing

            For Each instruction As Instruction In instructionsToExpand
                Dim value As Integer = instruction.Operand
                Dim num As Integer = 0
                Select Case Rand.Next(1, 16)
                    Case 1
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Integer))
                        num = 4
                        Exit Select
                    Case 2
                        tRef = Context.InputAssembly.MainModule.Import(GetType(SByte))
                        num = 1
                        Exit Select
                    Case 3
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Byte))
                        num = 1
                        Exit Select
                    Case 4
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Boolean))
                        num = 1
                        Exit Select
                    Case 5
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Decimal))
                        num = 16
                        Exit Select
                    Case 6
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Short))
                        num = 2
                        Exit Select
                    Case 7
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Long))
                        num = 8
                        Exit Select
                    Case 8
                        tRef = Context.InputAssembly.MainModule.Import(GetType(UInteger))
                        num = 4
                        Exit Select
                    Case 9
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Single))
                        num = 4
                        Exit Select
                    Case 10
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Char))
                        num = 2
                        Exit Select
                    Case 11
                        tRef = Context.InputAssembly.MainModule.Import(GetType(UShort))
                        num = 2
                        Exit Select
                    Case 12
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Double))
                        num = 8
                        Exit Select
                    Case 13
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Date))
                        num = 8
                        Exit Select
                    Case 14
                        tRef = Context.InputAssembly.MainModule.Import(GetType(ConsoleKeyInfo))
                        num = 12
                        Exit Select
                    Case 15
                        tRef = Context.InputAssembly.MainModule.Import(GetType(Guid))
                        num = 16
                        Exit Select
                End Select

                Try
                    Dim nmr% = Rand.Next(1, 1000)
                    Dim flag As Boolean = Convert.ToBoolean(Rand.Next(0, 2))
                    Select Case Rand.Next(1, 4)
                        Case 1
                            Dim newOp = ((value - num) + If(flag, -nmr, nmr))
                            Dim instruct = il.Create(OpCodes.Ldc_I4, newOp)
                            il.Replace(instruction, instruct)
                            Dim lastInstruct = TheInitFields.BodyModifier(newOp, il, instruct)
                            Dim SizeOFInstruct = Instruction.Create(OpCodes.Sizeof, tRef)
                            il.InsertAfter(If(lastInstruct, instruct), SizeOFInstruct)
                            Dim AddInstruct = Instruction.Create(OpCodes.Add)
                            il.InsertAfter(SizeOFInstruct, AddInstruct)
                            Dim Ldci4Instruct = Instruction.Create(OpCodes.Ldc_I4, nmr)
                            il.InsertAfter(AddInstruct, Ldci4Instruct)
                            Dim AddSubInstruct = Instruction.Create(If(flag, OpCodes.Add, OpCodes.Sub))
                            il.InsertAfter(Ldci4Instruct, AddSubInstruct)
                            TheInitFields.BodyModifier(value, il, Ldci4Instruct)
                            Exit Select
                        Case 2
                            Dim newOp = ((value + num) + If(flag, -nmr, nmr))
                            Dim instruct = il.Create(OpCodes.Ldc_I4, newOp)
                            il.Replace(instruction, instruct)
                            Dim lastInstruct = TheInitFields.BodyModifier(newOp, il, instruct)
                            Dim SizeOFInstruct = Instruction.Create(OpCodes.Sizeof, tRef)
                            il.InsertAfter(If(lastInstruct, instruct), SizeOFInstruct)
                            Dim AddInstruct = Instruction.Create(OpCodes.Sub)
                            il.InsertAfter(SizeOFInstruct, AddInstruct)
                            Dim Ldci4Instruct = Instruction.Create(OpCodes.Ldc_I4, nmr)
                            il.InsertAfter(AddInstruct, Ldci4Instruct)
                            Dim AddSubInstruct = Instruction.Create(If(flag, OpCodes.Add, OpCodes.Sub))
                            il.InsertAfter(Ldci4Instruct, AddSubInstruct)
                            TheInitFields.BodyModifier(value, il, Ldci4Instruct)
                            Exit Select
                        Case 3
                            Dim newOp = ((value + If(flag, -nmr, nmr)) * num)
                            Dim instruct = il.Create(OpCodes.Ldc_I4, newOp)
                            il.Replace(instruction, instruct)
                            Dim lastInstruct = TheInitFields.BodyModifier(newOp, il, instruct)
                            Dim SizeOFInstruct = Instruction.Create(OpCodes.Sizeof, tRef)
                            il.InsertAfter(If(lastInstruct, instruct), SizeOFInstruct)
                            Dim AddInstruct = Instruction.Create(OpCodes.Div)
                            il.InsertAfter(SizeOFInstruct, AddInstruct)
                            Dim Ldci4Instruct = Instruction.Create(OpCodes.Ldc_I4, nmr)
                            il.InsertAfter(AddInstruct, Ldci4Instruct)
                            Dim AddSubInstruct = Instruction.Create(If(flag, OpCodes.Add, OpCodes.Sub))
                            il.InsertAfter(Ldci4Instruct, AddSubInstruct)
                            TheInitFields.BodyModifier(value, il, Ldci4Instruct)
                            Exit Select
                    End Select
                Catch ex As Exception
                    'MsgBox(body.Method.FullName)
                End Try
            Next
        End Sub
#End Region

    End Class
End Namespace
