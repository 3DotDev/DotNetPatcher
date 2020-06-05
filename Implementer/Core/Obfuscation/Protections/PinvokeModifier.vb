Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Helper.RandomizeHelper
Imports Mono.Cecil.Cil
Imports System.Runtime.InteropServices

Namespace Core.Obfuscation.Protections
    Public Class PinvokeModifier
        Implements IDisposable

#Region " Fields "
        Private originalMeth As MethodDefinition
        Private iteratedT As TypeDefinition
        Private isNotSystemVoid As Boolean
        Private charSetVal As Integer
        Private moduleName As String
        Private functionName As String
        Private returnedType As TypeReference
        Private parameters As New Mono.Collections.Generic.Collection(Of ParameterDefinition)
        Private typeRefs As List(Of TypeReference)
        Private Randomizer As Randomizer
#End Region

#Region " Constructor "
        Public Sub New(Randomize As Randomizer)
            Randomizer = Randomize
        End Sub
#End Region

#Region " Methods "
        Public Sub AddModuleRef(mDef As ModuleDefinition)
            Dim module_ref = New ModuleReference("kernel32.dll")
            If Not mDef.ModuleReferences.Any(Function(m) m.Name.ToLower = "kernel32.dll") Then
                mDef.ModuleReferences.Add(module_ref)
            Else
                module_ref = mDef.ModuleReferences.Where(Function(x) x.Name = "kernel32.dll").First
            End If
        End Sub

        Public Sub InitPinvokeInfos(originalMethod As MethodDefinition, iteratedType As TypeDefinition)
            originalMeth = originalMethod
            iteratedT = iteratedType

            charSetVal = CharsetValue(originalMeth.PInvokeInfo)
            isNotSystemVoid = If(Not originalMeth.ReturnType.ToString = "System.Void", True, False)
            moduleName = If(originalMeth.PInvokeInfo.Module.Name.EndsWith(".dll"), originalMeth.PInvokeInfo.Module.Name, originalMeth.PInvokeInfo.Module.Name & ".dll")
            functionName = If(originalMeth.PInvokeInfo.EntryPoint.ToString = Nothing, originalMeth.Name, originalMeth.PInvokeInfo.EntryPoint.ToString)
            returnedType = iteratedT.Module.Import(originalMeth.ReturnType)
            parameters = originalMeth.Parameters

            typeRefs = parameters.ToList.ConvertAll(Function(rt) rt.ParameterType)
        End Sub

        Public Sub CreatePinvokeBody(DynamicInvokeMethod As MethodDefinition)
            Dim newDelegate = DelegateEmitter.Create(iteratedT.Module.Assembly, Randomizer.GenerateNew, returnedType, typeRefs, parameters, Randomizer)

            Dim ca As New CustomAttribute(iteratedT.Module.Import(GetType(UnmanagedFunctionPointerAttribute).GetConstructor(New Type() {GetType(CallingConvention)})))
            Dim carg = New CustomAttributeArgument(iteratedT.Module.Import(GetType(CallingConvention)), 2)
            'Cdecl = 2
            'StdCall = 3

            ca.ConstructorArguments.Add(carg)

            If Not charSetVal = 1 Then
                Dim caf As New CustomAttributeArgument(iteratedT.Module.Import(GetType(CharSet)), charSetVal)
                Dim canA As New CustomAttributeNamedArgument("CharSet", caf)
                ca.Fields.Add(canA)
            End If

            newDelegate.CustomAttributes.Add(ca)

            iteratedT.Module.Types.Add(newDelegate)

            originalMeth.IsPInvokeImpl = False
            originalMeth.IsPreserveSig = False

            originalMeth.Body = New MethodBody(originalMeth)

            Dim ILProc As ILProcessor = originalMeth.Body.GetILProcessor()
            ILProc.Body.MaxStackSize = 8
            ILProc.Body.InitLocals = True

            If isNotSystemVoid Then
                originalMeth.Body.Variables.Add(New VariableDefinition(returnedType))
            End If

            originalMeth.Body.Variables.Add(New VariableDefinition(iteratedT.Module.Import(newDelegate)))

            ILProc.Emit(OpCodes.Ldstr, moduleName)
            ILProc.Emit(OpCodes.Ldstr, functionName)
            ILProc.Emit(OpCodes.Call, MemberExtensions.MakeGeneric(DynamicInvokeMethod.GetElementMethod, newDelegate.GetElementType))

            If isNotSystemVoid Then
                ILProc.Emit(OpCodes.Stloc_1)
                ILProc.Emit(OpCodes.Ldloc_1)
            Else
                ILProc.Emit(OpCodes.Stloc_0)
                ILProc.Emit(OpCodes.Ldloc_0)
            End If

            If parameters.Count <> 0 Then
                For k = 0 To parameters.Count - 1
                    ILProc.Emit(OpCodes.Ldarg, k)

                    If parameters(k).ParameterType.IsValueType Then
                        iteratedT.Module.Assembly.MainModule.Import(parameters(k).ParameterType)
                    Else
                        iteratedT.Module.Assembly.MainModule.Import(parameters(k).ParameterType.Resolve)
                    End If
                Next
            End If

            ILProc.Emit(OpCodes.Callvirt, iteratedT.Module.Import(Finder.FindMethod(newDelegate, "Invoke")))

            If isNotSystemVoid Then
                ILProc.Emit(OpCodes.Stloc_0)
                ILProc.Emit(OpCodes.Ldloc_0)
            End If

            ILProc.Emit(OpCodes.Ret)
        End Sub

        Private Function CharsetValue(pInfo As PInvokeInfo) As Integer
            If pInfo.IsCharSetNotSpec Then
                Return 1
            ElseIf pInfo.IsCharSetAnsi Then
                Return 2
            ElseIf pInfo.IsCharSetUnicode Then
                Return 3
            ElseIf pInfo.IsCharSetAuto Then
                Return 4
            End If
            Return 0
        End Function

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If
                functionName = String.Empty
                charSetVal = 1
                moduleName = String.Empty
                isNotSystemVoid = False
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
