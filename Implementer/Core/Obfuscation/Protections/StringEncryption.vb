Imports Mono.Cecil
Imports Mono.Cecil.Rocks
Imports Mono.Cecil.Cil
Imports Helper.RandomizeHelper
Imports Helper.CecilHelper
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports Helper.CryptoHelper
Imports Implementer.Core.Obfuscation.Builder
Imports Implementer.Core.Dependencing.DependenciesInfos

Namespace Core.Obfuscation.Protections
    Public Class StringEncryption
        Inherits Protection

#Region " Fields "
        Private DecryptionString As DecryptionString
        Private DecryptionXor As DecryptionXor
        Private DecryptionBase64 As DecryptionBase64
        Private DecryptionPrime As DecryptionPrime

        Private ReadOnly randSalt As Random
        Private ReadOnly Types As List(Of TypeDefinition)
        Private IsDefaultEncoding As Boolean
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
        Private EncryptToResources As EncryptType
        Private ReadOnly Rand As Random
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (String encryption ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String = "String encryption"
        Public Overrides Property Context As ProtectionContext
        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean = True
        Public Overrides ReadOnly Property ProgressIncrement As Integer = 70
        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.EncryptString Then
                _Enabled = True
                randSalt = New Random
                Types = New List(Of TypeDefinition)
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
                Rand = New Random
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            'DO NOT encrypt OVER another protection encryption , so we will check the best configuration before !!!
            If Context.Params.TaskAccept.Obfuscation.HidePublicCalls Then
                EncryptToResources = EncryptType.ByDefault
            Else
                EncryptToResources = If(Context.Params.TaskAccept.Obfuscation.CompressResources OrElse Context.Params.TaskAccept.Obfuscation.EncryptResources, EncryptType.ByDefault, EncryptType.ToResources)
                If PackerState Then
                    EncryptToResources = EncryptType.ByDefault
                End If
            End If

            'Build Decryption String Stub and retrieve its methoddefinition
            If EncryptToResources = EncryptType.ToResources Then
                DecryptionString = New DecryptionString(New ResStreamContext(Context.InputAssembly, PackerState, Context.Randomizer, Context.Randomizer.GenerateNew))
                CompletedMethods.Add(DecryptionString.ReadBinaryFromStream)
            End If

            'Build Decryption Xor Stub and retrieve its methoddefinition
            DecryptionXor = New DecryptionXor(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptionXor.DecryptXor)

            'Build Decryption Base64 Stub and retrieve those methoddefinition
            DecryptionBase64 = New DecryptionBase64(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptionBase64.FromBase64)
            CompletedMethods.Add(DecryptionBase64.GetStringFromBytes)

            'Build Decryption Prime Stub and retrieve its methoddefinition
            DecryptionPrime = New DecryptionPrime(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptionPrime.DecryptPrime)

            'ModuleDefintion types iteration 
            For Each m As ModuleDefinition In Context.InputAssembly.Modules
                Types.AddRange(m.GetAllTypes())
                For Each type As TypeDefinition In Types
                    IterateType(type)
                Next
                Types.Clear()
            Next

            'Inject encrypted data into assemblydefintion resources
            If EncryptToResources = EncryptType.ToResources Then
                DecryptionString.InjectResource()
            End If

            'Cleanup
            DeleteStubs()
        End Sub

        Private Sub DeleteStubs()
            If Not DecryptionString Is Nothing Then DecryptionString.RemoveStubFile()
            If Not DecryptionXor Is Nothing Then DecryptionXor.RemoveStubFile()
            If Not DecryptionBase64 Is Nothing Then DecryptionBase64.RemoveStubFile()
            If Not DecryptionPrime Is Nothing Then DecryptionPrime.RemoveStubFile()
        End Sub

        Private Sub IterateType(td As TypeDefinition)
            Dim publicMethods As New List(Of MethodDefinition)()
            publicMethods.AddRange(From m In td.Methods Where (m.HasBody AndAlso m.Body.Instructions.Count >= 1 AndAlso
                                                            Not CompletedMethods.Contains(m) AndAlso
                                                            Not m.Name = "get_ResourceManager" AndAlso
                                                            Not Utils.IsStronglyTypedResourceBuilder(m.DeclaringType)))
            Try
                For Each md In publicMethods
                    md.Body.SimplifyMacros
                    ProcessInstructions(md.Body)
                    md.Body.OptimizeMacros
                    md.Body.ComputeOffsets()
                    md.Body.ComputeHeader()
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            publicMethods.Clear()
        End Sub

        Private Sub ProcessInstructions(body As MethodBody)

            Try
                Dim instructions = body.Instructions
                Dim il = body.GetILProcessor()
                Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

                For Each instruction As Instruction In instructions
                    Select Case instruction.OpCode
                        Case OpCodes.Ldstr
                            If Utils.IsValidStringOperand(instruction) Then
                                If Utils.IsSettingStr(body.Method, instruction.Operand.ToString) = False Then
                                    If DecryptionString Is Nothing Then
                                        instructionsToExpand.Add(instruction)
                                    Else
                                        If Not instruction.Operand.ToString = DecryptionString.Context.ResourceName Then
                                            instructionsToExpand.Add(instruction)
                                        End If
                                    End If
                                End If
                            End If
                    End Select
                Next

                For Each instruction As Instruction In instructionsToExpand
                    Dim str As String = instruction.Operand.ToString()
                    Dim salt = randSalt.Next(1, 255)

                    IsDefaultEncoding = False

                    Dim mdFinal As MethodDefinition = New MethodDefinition(Context.Randomizer.GenerateNew, MethodAttributes.Static Or MethodAttributes.Private Or MethodAttributes.HideBySig, Context.InputAssembly.MainModule.Import(GetType(String)))
                    Dim param1 = New ParameterDefinition(Context.InputAssembly.MainModule.Import(GetType(Boolean)))
                    mdFinal.Parameters.Add(param1)
                    mdFinal.Body = New MethodBody(mdFinal)
                    Dim var1 = New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String)))
                    mdFinal.Body.Variables.Add(var1)

                    Dim encXor = EncryptXor(EncodeTo_64(str, IsDefaultEncoding), salt)

                    Dim ilProc As ILProcessor = mdFinal.Body.GetILProcessor()
                    ilProc.Body.InitLocals = True

                    Dim resEncrypted As Boolean = If((EncryptToResources = EncryptType.ToResources), True, False)
                    If resEncrypted Then
                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))
                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                        ilProc.Emit(OpCodes.Ldc_I4, DecryptionString.WriteData(encXor))
                        ilProc.Emit(OpCodes.Stloc_1)
                        ilProc.Emit(OpCodes.Ldloc_1)
                        ilProc.Emit(OpCodes.Call, DecryptionString.ReadBinaryFromStream)
                        ilProc.Emit(OpCodes.Stloc_2)
                        ilProc.Emit(OpCodes.Ldloc_2)
                    Else
                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                        ilProc.Emit(OpCodes.Ldstr, encXor)
                        ilProc.Emit(OpCodes.Stloc_1)
                        ilProc.Emit(OpCodes.Ldloc_1)
                    End If

                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))
                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Byte()))))

                    ilProc.Emit(OpCodes.Ldc_I4, salt)
                    ilProc.Emit(OpCodes.Stloc, If(resEncrypted, 3, 2))
                    ilProc.Emit(OpCodes.Ldloc, If(resEncrypted, 3, 2))
                    ilProc.Emit(OpCodes.Call, DecryptionXor.DecryptXor)
                    ilProc.Emit(OpCodes.Stloc, If(resEncrypted, 4, 3))
                    ilProc.Emit(OpCodes.Ldloc, If(resEncrypted, 4, 3))
                    ilProc.Emit(OpCodes.Ldarg_0)
                    ilProc.Emit(OpCodes.Call, DecryptionBase64.FromBase64)
                    ilProc.Emit(OpCodes.Stloc, If(resEncrypted, 5, 4))
                    ilProc.Emit(OpCodes.Ldloc, If(resEncrypted, 5, 4))
                    ilProc.Emit(OpCodes.Ldarg_0)
                    ilProc.Emit(OpCodes.Call, DecryptionBase64.GetStringFromBytes)
                    ilProc.Emit(OpCodes.Stloc_0)
                    ilProc.Emit(OpCodes.Ldloc_0)
                    ilProc.Emit(OpCodes.Ret)

                    body.Method.DeclaringType.Methods.Add(mdFinal)

                    Dim UnPrime = Rand.Next(Generator.numberUnPrime.Length)
                    Dim Prime = Rand.Next(Generator.numberPrime.Length)
                    Dim valFinale% = If(IsDefaultEncoding, Generator.numberPrime(Prime), Generator.numberUnPrime(UnPrime))

                    Dim instruct = il.Create(OpCodes.Ldc_I4, valFinale)
                    il.Replace(instruction, instruct)
                    Dim CallDecrypt = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptionPrime.DecryptPrime))
                    il.InsertAfter(instruct, CallDecrypt)
                    Dim CallMdFinal = il.Create(OpCodes.Call, mdFinal)
                    il.InsertAfter(CallDecrypt, CallMdFinal)
                    CompletedMethods.Add(mdFinal)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

        Private Function EncodeTo_64(toEncode$, defaultEnc As Boolean) As String
            Dim strByte = GetByte(toEncode, defaultEnc)
            Return ConvertToBase64String(strByte, defaultEnc)
        End Function

        Private Function GetByte(s As String, defaultEnc As Boolean) As Byte()
            Return If(defaultEnc, Encoding.Default.GetBytes(s), Encoding.UTF8.GetBytes(s))
        End Function

        Private Function ConvertToBase64String(data As Byte(), defaultEnc As Boolean) As String
            Dim builder = New StringBuilder()

            Using writer = New StringWriter(builder)
                Using transformation = New ToBase64Transform()
                    Dim bufferedOutputBytes = New Byte(transformation.OutputBlockSize - 1) {}
                    Dim i = 0
                    Dim inputBlockSize = transformation.InputBlockSize

                    While data.Length - i > inputBlockSize
                        transformation.TransformBlock(data, i, data.Length - i, bufferedOutputBytes, 0)
                        i += inputBlockSize
                        Dim str = If(defaultEnc, Encoding.Default.GetString(bufferedOutputBytes), Encoding.UTF8.GetString(bufferedOutputBytes))
                        writer.Write(str)
                    End While

                    bufferedOutputBytes = transformation.TransformFinalBlock(data, i, data.Length - i)
                    Dim strFinal = If(defaultEnc, Encoding.Default.GetString(bufferedOutputBytes), Encoding.UTF8.GetString(bufferedOutputBytes))
                    writer.Write(strFinal)
                    transformation.Clear()
                End Using

                writer.Close()
            End Using

            Return builder.ToString()
        End Function

        Private Function EncryptXor(tString As String, numInteg%) As String
            Dim sResult$ = String.Empty
            Dim sLength% = (tString.Length - 1)
            Dim jincrement% = 0
            Do While (jincrement <= sLength)
                Dim p% = (Convert.ToInt32(tString.Chars(jincrement)) Xor numInteg)
                sResult = (sResult & Char.ConvertFromUtf32(p))
                jincrement += 1
            Loop
            Return sResult
        End Function
#End Region

    End Class
End Namespace
