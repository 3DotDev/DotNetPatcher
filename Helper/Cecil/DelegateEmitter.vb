Imports Mono.Cecil
Imports Helper.RandomizeHelper

Namespace CecilHelper
    Public Class DelegateEmitter

#Region " Constants "
        Private Const DelegateTypeAttributes As TypeAttributes = TypeAttributes.Class Or TypeAttributes.Public Or TypeAttributes.Sealed
        Private Const ConstructorAttributes As MethodAttributes = MethodAttributes.Public Or MethodAttributes.SpecialName Or MethodAttributes.RTSpecialName
        Private Const DelegateMethodAttributes As MethodAttributes = MethodAttributes.Public Or MethodAttributes.Virtual Or MethodAttributes.VtableLayoutMask
#End Region

#Region " Fields "
        Private Shared voidType As TypeReference
        Private Shared objectType As TypeReference
        Private Shared nativeIntType As TypeReference
        Private Shared multidelegate As TypeReference
#End Region

#Region " Methods "

        Public Shared Function Create(asm As AssemblyDefinition, delegateName As String, returnType As TypeReference, argumentsTyps As List(Of TypeReference), arguments As Mono.Collections.Generic.Collection(Of ParameterDefinition), Randomizer As Randomizer) As TypeDefinition
            InitializeTypes(asm)

            Dim TypeDef = New TypeDefinition(String.Empty, delegateName, DelegateTypeAttributes, multidelegate)
            With TypeDef.Methods
                .Add(BuildConstructor)
                .Add(BuildInvoke(returnType, argumentsTyps, arguments, Randomizer))
            End With

            Return TypeDef
        End Function

        Private Shared Sub InitializeTypes(assDef As AssemblyDefinition)
            voidType = assDef.MainModule.Import(GetType(Void))
            objectType = assDef.MainModule.Import(GetType(Object))
            nativeIntType = assDef.MainModule.Import(GetType(IntPtr))
            multidelegate = assDef.MainModule.Import(GetType(MulticastDelegate))
        End Sub

        Private Shared Function BuildConstructor() As MethodDefinition
            Dim constructor = New MethodDefinition(".ctor", ConstructorAttributes, voidType)
            With constructor
                .Parameters.Add(New ParameterDefinition("objectInstance", ParameterAttributes.None, objectType))
                .Parameters.Add(New ParameterDefinition("functionPtr", ParameterAttributes.None, nativeIntType))
                .ImplAttributes = MethodImplAttributes.Runtime
            End With
            Return constructor
        End Function

        Private Shared Function BuildInvoke(returnType As TypeReference, argumentsTypes As List(Of TypeReference), arguments As Mono.Collections.Generic.Collection(Of ParameterDefinition), Randomizer As Randomizer) As MethodDefinition
            Dim invoke = New MethodDefinition("Invoke", DelegateMethodAttributes, returnType)
            With invoke
                For Each argument In arguments
                    .Parameters.Add(New ParameterDefinition(Randomizer.GenerateNew, argument.Attributes, argument.ParameterType))
                Next
                .ImplAttributes = MethodImplAttributes.Runtime
            End With
            Return invoke
        End Function

#End Region

    End Class
End Namespace