Imports Mono.Cecil
Imports Mono.Cecil.Rocks
Imports Helper.CecilHelper
Imports System.IO
Imports Mono.Cecil.Cil
Imports Implementer.Core.Packer
Imports Helper.RandomizeHelper

Namespace Core.Obfuscation.Builder

    Public MustInherit Class Stub

#Region " Fields "
        Private AssDef As AssemblyDefinition
        Private AssDefTarget As AssemblyDefinition
        Private TypeDefResolver As TypeDefinition
        Private ResolverDll As String
        Private ReadOnly NumberOfFunctions As Integer
#End Region

#Region " Properties "
        Protected Property Names As Names
        Protected ReadOnly Property ResolvedTypeDef As TypeDefinition = Nothing
        Protected Property ReferencedZipperAssembly As ZipInfos
        Private Property Randomizer As Randomizer
#End Region

#Region " Constructor "
        Friend Sub New(NumberOfFunctions As Integer, Randomize As Randomizer)
            _Randomizer = Randomize
            Me.NumberOfFunctions = NumberOfFunctions
            _Names = New Names(NumberOfFunctions, Randomize)
        End Sub

        Friend Sub New(classN As String, FuncN1 As String, Randomize As Randomizer)
            _Randomizer = Randomize
            NumberOfFunctions = 1
            _Names = New Names(classN, FuncN1)
        End Sub
#End Region

#Region " Methods "
        Protected Sub ResolveTypeFromFile(ResolvedDll As String, Optional ByVal replaceNamespace As String = "")

            ResolverDll = ResolvedDll
            AssDef = AssemblyDefinition.ReadAssembly(ResolvedDll)

            TypeDefResolver = Finder.FindType(AssDef.MainModule, _Names.ClassName)

            If Not replaceNamespace = "" Then
                TypeDefResolver.Namespace = replaceNamespace
            End If

            _Names.ClassName = Randomizer.GenerateNew

            For i As Integer = 0 To NumberOfFunctions - 1
                Dim m = Finder.FindMethod(TypeDefResolver, _Names.Functions(i))
                m.Name = Randomizer.GenerateNew
                _Names.Functions(i) = m.Name
            Next
        End Sub

        Protected Sub InjectType(assDefTarget As AssemblyDefinition)
            Me.AssDefTarget = assDefTarget
            _ResolvedTypeDef = Injecter.InjectTypeDefinition(assDefTarget.MainModule, TypeDefResolver)
            assDefTarget.MainModule.Types.Add(ResolvedTypeDef)
        End Sub

        Protected Sub InjectToCctor(assDefTarget As AssemblyDefinition)
            Me.AssDefTarget = assDefTarget
            _ResolvedTypeDef = Injecter.InjectTypeDefinition(assDefTarget.MainModule, TypeDefResolver)

            Dim globalType = Me.AssDefTarget.MainModule.GetType("<Module>")

            Dim cctorMethod = globalType.GetStaticConstructor
            If cctorMethod Is Nothing Then
                globalType.Methods.Add(Injecter.CreateGenericCctor(Me.AssDefTarget))
                cctorMethod = globalType.GetStaticConstructor
            End If

            If cctorMethod.Body.Instructions.Count > 0 AndAlso cctorMethod.Body.Instructions.Last.OpCode = OpCodes.Ret Then
                cctorMethod.Body.Instructions.Remove(cctorMethod.Body.Instructions.Last)
            End If

            Me.AssDefTarget.MainModule.Types.Add(ResolvedTypeDef)

            Dim initializeMethod = Finder.FindMethod(Me.AssDefTarget, _Names.Functions(0))

            If Not initializeMethod Is Nothing Then
                If Not cctorMethod Is Nothing Then
                    Dim ilproc = cctorMethod.Body.GetILProcessor()
                    Dim last = ilproc.Body.Instructions.Count
                    Dim init = ilproc.Create(OpCodes.Call, initializeMethod)
                    cctorMethod.Body.Instructions.Insert(last, init)
                    ilproc.InsertAfter(init, ilproc.Create(OpCodes.Ret))
                End If
            End If
        End Sub

        Protected Function GetMethod1() As MethodDefinition
            Return Finder.FindMethod(AssDefTarget, _Names.Functions(0))
        End Function

        Protected Function GetMethod2() As MethodDefinition
            Return Finder.FindMethod(AssDefTarget, _Names.Functions(1))
        End Function

        Protected Function GetMethod3() As MethodDefinition
            Return Finder.FindMethod(AssDefTarget, _Names.Functions(2))
        End Function

        Protected Function GetMethod4() As MethodDefinition
            Return Finder.FindMethod(AssDefTarget, _Names.Functions(3))
        End Function

        Protected Sub DeleteStubFile()
            If File.Exists(ResolverDll) Then
                Try
                    File.Delete(ResolverDll)
                Catch ex As Exception
                End Try
            End If
        End Sub
#End Region

    End Class
End Namespace