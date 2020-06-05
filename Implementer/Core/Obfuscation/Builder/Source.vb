Imports Helper.CryptoHelper
Imports Helper.AssemblyHelper
Imports Helper.CecilHelper
Imports Helper.CodeDomHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder
    Friend NotInheritable Class Source

#Region " Fields "
        Private Shared m_AddedNamespaceStart As String = String.Empty
        Private Shared m_AddedNamespaceEnd As String = String.Empty
#End Region

#Region " Methods "
        Private Shared Sub LoadNamespacesHeaders(InputAssembly As AssemblyDefinition, PackerTask As Boolean)
            Dim NamespaceDefault = Finder.FindDefaultNamespace(InputAssembly, PackerTask)

            m_AddedNamespaceStart = "Namespace " & NamespaceDefault
            m_AddedNamespaceEnd = "End Namespace"

            If NamespaceDefault = String.Empty Then
                m_AddedNamespaceStart = String.Empty
                m_AddedNamespaceEnd = String.Empty
            End If

            If PackerTask Then
                m_AddedNamespaceStart = String.Empty
                m_AddedNamespaceEnd = String.Empty
            End If
        End Sub

#Region " STRING "
        Friend Shared Function ReadStringFromResourcesStub(ClassName$, ResourceDecryptFunc$, Decompress0$, Decompress1$, Contex As ResStreamContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim ms$ = Contex.Randomizer.GenerateNewAlphabetic

            Dim str =
                "Imports System.Windows.Forms" & vbNewLine &
                "Imports System.Collections.Generic" & vbNewLine &
                "Imports System" & vbNewLine &
                "Imports System.IO" & vbNewLine &
                "Imports System.IO.Compression" & vbNewLine & vbNewLine _
                       & Loader.GenerateInfos() & vbNewLine _
                       & "Public Class " & ClassName & vbNewLine _
                       & "    Private Shared " & ms & " As Stream " & vbNewLine _
                       & "    Public Shared Function " & ResourceDecryptFunc & " (ByVal Base_StreamPos As Integer) As String" & vbNewLine _
                       & "        Dim b_reader As New BinaryReader(" & ms & ")" & vbNewLine _
                       & "        b_reader.BaseStream.Position = Base_StreamPos" & vbNewLine _
                       & "        Return b_reader.ReadString" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Shared Sub New" & vbNewLine _
                       & "        If " & ms & " Is Nothing Then" & vbNewLine _
                       & "           Dim The_byte as Byte()" & vbNewLine _
                       & "           The_byte = " & Decompress0 & "(Assembly.GetExecutingAssembly.GetManifestResourceStream(""" & Contex.ResourceName & """))" & vbNewLine _
                       & "           " & ms & " = New MemoryStream(The_byte)" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & Generator.GenerateDeCompressWithGzipStreamFunc(Decompress0, Decompress1) & vbNewLine _
                       & "End Class"
            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function DecryptXorStub(ClassName$, DecryptXorFuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str = "Imports System" & vbNewLine _
                      & Loader.GenerateInfos() & vbNewLine _
                      & m_AddedNamespaceStart & vbNewLine _
                      & Generator.GenerateDecryptXorFunc(ClassName, DecryptXorFuncName) & vbNewLine _
                      & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function FromBase64Stub(ClassName$, Base64FuncName$, GetStringFuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str =
                "Imports System" & vbNewLine &
                "Imports System.Text" & vbNewLine &
                "Imports System.IO" & vbNewLine &
                "Imports System.Security.Cryptography" & vbNewLine _
                      & Loader.GenerateInfos() & vbNewLine _
                      & m_AddedNamespaceStart & vbNewLine _
                      & Generator.GenerateFromBase64Func(ClassName, Base64FuncName, GetStringFuncName) & vbNewLine _
                      & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function
#End Region

#Region " BOOLEAN "
        Friend Shared Function DecryptIntStub(ClassName$, DecryptIntFuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str =
                "Imports System" & vbNewLine &
                "Imports Microsoft.VisualBasic" & vbNewLine _
                        & Loader.GenerateInfos() & vbNewLine _
                        & m_AddedNamespaceStart & vbNewLine _
                        & Generator.GenerateDecryptIntFunc(ClassName, DecryptIntFuncName) & vbNewLine _
                        & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function DecryptOddStub(ClassName$, DecryptOddFuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str =
                "Imports System" & vbNewLine &
                "Imports Microsoft.VisualBasic" & vbNewLine _
                      & Loader.GenerateInfos() & vbNewLine _
                      & m_AddedNamespaceStart & vbNewLine _
                      & Generator.GenerateDecryptOddFunc(ClassName, DecryptOddFuncName) & vbNewLine _
                      & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function ReadFromResourcesStub(ClassName$, ReadFromResourcesFuncName$, contex As ResManagerContext) As String
            LoadNamespacesHeaders(contex.InputAssembly, contex.PackerTask)
            Dim str =
                "Imports System" & vbNewLine &
                "Imports Microsoft.VisualBasic" & vbNewLine &
                "Imports System.Resources" & vbNewLine _
                          & Loader.GenerateInfos() & vbNewLine _
                          & m_AddedNamespaceStart & vbNewLine _
                          & Generator.GenerateReadFromResourcesFunc(ClassName, ReadFromResourcesFuncName, contex.ResourceName) & vbNewLine _
                          & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(ClassName, contex.FrameworkVersion, str)
        End Function

        Friend Shared Function DecryptPrimeStub(className$, DecryptPrimeFuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str =
                "Imports System.Collections.Generic" & vbNewLine &
                "Imports System" & vbNewLine &
                      Loader.GenerateInfos() & vbNewLine _
                    & m_AddedNamespaceStart & vbNewLine _
                    & "Public Class " & className & vbNewLine _
                    & Generator.GenereateDecryptPrimeFunc(DecryptPrimeFuncName) & vbNewLine _
                    & "End Class" & vbNewLine & vbNewLine _
                    & m_AddedNamespaceEnd

            Return Compiler.CreateStubFromString(className, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function DecryptRPNStub(ClassName$, DecryptRPNFuncName1$, DecryptRPNFuncName2$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str =
                "Imports System.Collections.Generic" & vbNewLine &
                "Imports System" & vbNewLine &
                      Loader.GenerateInfos() & vbNewLine _
                    & m_AddedNamespaceStart & vbNewLine _
                    & "Public Class " & ClassName & vbNewLine _
                    & Generator.GenerateDecryptRPNFunc(DecryptRPNFuncName1, DecryptRPNFuncName2) & vbNewLine _
                    & "End Class" & vbNewLine & vbNewLine _
                    & m_AddedNamespaceEnd

            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

#End Region

#Region " ANTIDEBUG "
        Friend Shared Function AntiDebugStub(classname$, funcName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim FuncName2 = Contex.Randomizer.GenerateNewAlphabetic

            Dim str As String =
    "Imports System" & vbNewLine &
    "Imports System.Diagnostics" & vbNewLine &
    "Imports System.Threading" & vbNewLine &
     Loader.GenerateInfos() & vbNewLine _
    & m_AddedNamespaceStart & vbNewLine &
        "Friend Class " & classname & vbNewLine &
            "Public Shared Sub " & funcName & "()" & vbNewLine &
            "   If ((Not Environment.GetEnvironmentVariable(""COR_ENABLE_PROFILING"") Is Nothing) OrElse (Not Environment.GetEnvironmentVariable(""COR_PROFILER"") Is Nothing)) Then" & vbNewLine &
            "       Environment.FailFast(""Profiler detected"")" & vbNewLine &
            "   End If" & vbNewLine &
            "   Dim n_parameter As New Thread(New ParameterizedThreadStart(AddressOf " & FuncName2 & "))" & vbNewLine &
            "   Dim n_t As New Thread(New ParameterizedThreadStart(AddressOf " & FuncName2 & "))" & vbNewLine &
            "   n_parameter.IsBackground = True" & vbNewLine &
            "   n_t.IsBackground = True" & vbNewLine &
            "   n_parameter.Start(n_t)" & vbNewLine &
            "   Thread.Sleep(500)" & vbNewLine &
            "   n_t.Start(n_parameter)" & vbNewLine &
            "End Sub" & vbNewLine & vbNewLine &
            "Private Shared Sub " & FuncName2 & "(ByVal n_th As Object)" & vbNewLine &
            "   Thread.Sleep(&H3E8)" & vbNewLine &
            "   Dim n_thread As Thread = DirectCast(n_th, Thread)" & vbNewLine &
            "   Do While True" & vbNewLine &
            "       If (Debugger.IsAttached OrElse Debugger.IsLogging) Then" & vbNewLine &
            "           Environment.FailFast(""Debugger detected (Managed)"")" & vbNewLine &
            "       End If" & vbNewLine &
            "       If Not n_thread.IsAlive Then" & vbNewLine &
            "           Environment.FailFast(""Loop broken"")" & vbNewLine &
            "       End If" & vbNewLine &
            "       Thread.Sleep(&H3E8)" & vbNewLine &
            "   Loop" & vbNewLine &
            "End Sub" & vbNewLine & vbNewLine &
        "End Class" & vbNewLine & vbNewLine &
    m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(classname, Contex.FrameworkVersion, str)
        End Function

#End Region

#Region " ANTITAMPER "
        Friend Shared Function AntiTamperStub(className$, FuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str As String =
                "Imports System.Security.Cryptography" & vbNewLine &
                "Imports System.Windows.Forms" & vbNewLine &
                "Imports System.Collections.Generic" & vbNewLine &
                "Imports System" & vbNewLine &
                "Imports System.IO" & vbNewLine &
                "Imports System.IO.Compression" & vbNewLine & vbNewLine _
                 & Loader.GenerateInfos() & vbNewLine _
                 & m_AddedNamespaceStart & vbNewLine _
                 & "Public Class " & className & vbNewLine _
                 & "    Public Shared Sub " & FuncName & " ()" & vbNewLine _
                 & "        Dim n_l As String = (GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing)).Location" & vbNewLine _
                 & "        Dim n_b As Stream = New StreamReader(n_l).BaseStream" & vbNewLine _
                 & "        Dim n_r As New BinaryReader(n_b)" & vbNewLine _
                 & "        Dim n_b0 As String = Nothing" & vbNewLine _
                 & "        Dim n_b1 As String = Nothing" & vbNewLine _
                 & "        n_b0 = BitConverter.ToString(Ctype(CryptoConfig.CreateFromName(" & Chr(34) & "MD5" & Chr(34) & "), HashAlgorithm).ComputeHash(n_r.ReadBytes((File.ReadAllBytes(n_l).Length - 16))))" & vbNewLine _
                 & "        n_b.Seek(-16, SeekOrigin.End)" & vbNewLine _
                 & "        n_b1 = BitConverter.ToString(n_r.ReadBytes(16))" & vbNewLine _
                 & "        If (n_b0 <> n_b1) Then" & vbNewLine _
                 & "            Throw New BadImageFormatException" & vbNewLine _
                 & "        End If" & vbNewLine _
                 & "    End Sub" & vbNewLine _
                 & "End Class" & vbNewLine _
                 & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(className, Contex.FrameworkVersion, str)
        End Function
#End Region

#Region " PINVOKE "
        Friend Shared Function DynamicInvokeStub(className$, m_loadLibraryFuncName$, m_getMethProcFuncName$, m_invokeMethFuncName$, Contex As StubContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
            Dim str = "Imports System.Threading" & vbNewLine &
                        "Imports System" & vbNewLine &
                        "Imports System.Reflection.Emit" & vbNewLine &
                        "Imports System.Runtime.InteropServices" & vbNewLine & vbNewLine _
                       & Loader.GenerateInfos() & vbNewLine _
                       & m_AddedNamespaceStart & vbNewLine _
                       & "Public Class " & className & vbNewLine _
                       & "    <DllImport(""kernel32.dll"", EntryPoint :=""LoadLibrary"")> _" & vbNewLine _
                       & "    Private Shared Function " & m_loadLibraryFuncName & "(hLib As String) As IntPtr" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    <DllImport(""kernel32.dll"", EntryPoint :=""GetProcAddress"",CharSet:=CharSet.Ansi, ExactSpelling:=True)> _" & vbNewLine _
                       & "    Private Shared Function " & m_getMethProcFuncName & "(hMod As IntPtr, pName As String) As IntPtr" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Public Shared Function " & m_invokeMethFuncName & " (Of T As Class)(lib_F As String, func_N As String) As T" & vbNewLine _
                       & "        Dim l_l As IntPtr = " & m_loadLibraryFuncName & "(lib_F)" & vbNewLine _
                       & "        Dim deleg_T As System.Delegate = Marshal.GetDelegateForFunctionPointer(" & m_getMethProcFuncName & "(l_l, func_N), GetType(T))" & vbNewLine _
                       & "        Return TryCast(deleg_T, T)" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "End Class" & vbNewLine _
                       & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(className, Contex.FrameworkVersion, str)
        End Function
#End Region

        Friend Shared Function SevenZipStub(ClassName$, initializeFuncName$, resolverName$, Decompress0$, Decompress1$, Contex As CompressContext) As String
            Dim reverseStr = If(Contex.ResourceEncrypt = True, "                    Array.Reverse(d_Assembly)", String.Empty)
            Dim DecompressStr0 = "                d_Assembly = " & Decompress0 & "(mem_Stream.ToArray)"

            Dim str =
                "Imports System.Windows.Forms" & vbNewLine &
                "Imports System.Collections.Generic" & vbNewLine &
                "Imports System" & vbNewLine &
                "Imports System.Xml" & vbNewLine &
                "Imports System.IO" & vbNewLine &
                "Imports System.IO.Compression" & vbNewLine & vbNewLine _
                       & Loader.GenerateInfos() & vbNewLine _
                       & "Public Class " & ClassName & vbNewLine _
                       & "    Private Shared Function " & resolverName & " (ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly" & vbNewLine _
                       & "        Dim k_AssName As String = Cstr(New AssemblyName(args.Name).Name)" & vbNewLine _
                       & "        Dim Ass_Embly As Assembly = Nothing" & vbNewLine _
                       & "        Dim d_Assembly as Byte()" & vbNewLine _
                       & "        If k_AssName.EndsWith("".xmlserializers"", StringComparison.OrdinalIgnoreCase) Then" & vbNewLine _
                       & "            Return Nothing" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        If k_AssName.EndsWith("".resources"", StringComparison.OrdinalIgnoreCase) Then" & vbNewLine _
                       & "            Return Nothing" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        SyncLock hash_table" & vbNewLine _
                       & "            If hash_table.ContainsKey(""" & Contex.ResourceName & """) Then" & vbNewLine _
                       & "                Return Nothing" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "            Using ass_Stream As Stream = (GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing)).GetManifestResourceStream(""" & Contex.ResourceName & """)" & vbNewLine _
                       & "                If ass_Stream Is Nothing Then" & vbNewLine _
                       & "                    Return Nothing" & vbNewLine _
                       & "                End If" & vbNewLine _
                       & "                Using mem_Stream As MemoryStream = New MemoryStream" & vbNewLine _
                       & "                    Const b_Value = 4096" & vbNewLine _
                       & "                    Dim bu_ffer As Byte() = New Byte(b_Value - 1) {}" & vbNewLine _
                       & "                    Dim count_Read As Integer = ass_Stream.Read(bu_ffer, 0, b_Value)" & vbNewLine _
                       & "                    Do" & vbNewLine _
                       & "                        mem_Stream.Write(bu_ffer, 0, count_Read)" & vbNewLine _
                       & "                        count_Read = ass_Stream.Read(bu_ffer, 0, b_Value)" & vbNewLine _
                       & "                    Loop While (count_Read <> 0)" & vbNewLine _
                       & If(Contex.ResourceCompress = True, DecompressStr0, "d_Assembly = mem_Stream.ToArray()") & vbNewLine _
                       & "                " & reverseStr & vbNewLine _
                       & "                    Ass_Embly = Assembly.Load(d_Assembly)" & vbNewLine _
                       & "                    hash_table.Item(""" & Contex.ResourceName & """) = Ass_Embly" & vbNewLine _
                       & "                    Return Ass_Embly" & vbNewLine _
                       & "                End Using" & vbNewLine _
                       & "            End Using" & vbNewLine _
                       & "        End SyncLock" & vbNewLine _
                       & "        Return Nothing" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Shared Sub new" & vbNewLine _
                       & "        hash_table = New Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & If(Contex.ResourceCompress = True, Generator.GenerateCompressWithGzipByteFunc(Decompress0, Decompress1), "") & vbNewLine _
                       & "    Private Shared hash_table As Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    Public Shared Sub " & initializeFuncName & vbNewLine _
                       & "        AddHandler AppDomain.CurrentDomain.AssemblyResolve, New ResolveEventHandler(AddressOf " & resolverName & ")" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & "End Class"

            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function PackerStub(Contex As PackerContext, Names As Names) As String
            Dim ResourceAssembly = Contex.Randomizer.GenerateNewAlphabetic
            Dim decodeString = Contex.Randomizer.GenerateNewAlphabetic
            Dim Decrypt = Contex.Randomizer.GenerateNewAlphabetic
            Dim fromBase64 = Contex.Randomizer.GenerateNewAlphabetic
            Dim reverseStr As String = String.Empty
            If Contex.Reverse Then
                reverseStr = "                    Array.Reverse(a_byt)"
            End If

            Dim aesStr As String = "    Private Shared Function " & Decrypt & "(ByVal i_Byte As Byte()) As Byte()" & vbNewLine _
                        & "        Dim var_k as Byte() = " & Contex.ReferencedZipperAssembly.RefNewTypeName & ".pKey(""" & Convert.ToBase64String(SevenZipLib.SevenZipHelper.Compress(Contex.PolyXor.Key)) & """)" & vbNewLine _
                        & "        Dim var_O As Byte() = New Byte(i_Byte.Length - " & Contex.PolyXor.SaltSize.ToString & " - 1) {}" & vbNewLine _
                        & "        Dim var_S As Byte() = New Byte(" & Contex.PolyXor.SaltSize.ToString & " - 1) {}" & vbNewLine _
                        & "        Buffer.BlockCopy(i_Byte, i_Byte.Length - " & Contex.PolyXor.SaltSize.ToString & ", var_S, 0, " & Contex.PolyXor.SaltSize.ToString & ")" & vbNewLine _
                        & "        Array.Resize(Of Byte)(i_Byte, i_Byte.Length - " & Contex.PolyXor.SaltSize.ToString & ")" & vbNewLine _
                        & "        For var_j As Integer = 0 To i_Byte.Length - 1" & vbNewLine _
                        & "            var_O(var_j) = CByte(i_Byte(var_j) Xor var_k(var_j Mod var_k.Length) Xor var_S(var_j Mod var_S.Length))" & vbNewLine _
                        & "        Next" & vbNewLine _
                        & "        Return var_O" & vbNewLine _
                        & "    End Function"

            Dim str = "Imports System.Windows.Forms" & vbNewLine &
                    "Imports System.Security.Cryptography" & vbNewLine &
                    "Imports System" & vbNewLine &
                    "Imports System.Threading" & vbNewLine &
                    "Imports System.Text" & vbNewLine &
                    "Imports System.IO" & vbNewLine &
                    "Imports System.Resources" & vbNewLine &
                    "Imports System.IO.Compression" & vbNewLine &
                    "Imports " & Contex.ReferencedZipperAssembly.RefNewNamespaceName & vbNewLine & vbNewLine _
          & Loader.GenerateInfos() & vbNewLine _
          & "Friend Class " & Names.ClassName & vbNewLine & vbNewLine _
          & "    Private Delegate Function z_deleg() As Assembly" & vbNewLine _
          & "    <STAThread()> _" & vbNewLine _
          & "    Public Shared Sub Main(ByVal args As String())" & vbNewLine _
          & "        Dim y_assembly As Assembly = " & ResourceAssembly & "((""app, version=0.0.0.0, culture=neutral, publickeytoken=null"").Replace("".resources"",""""))" & vbNewLine _
          & "        Dim y_entryPoint As MethodInfo = y_assembly.EntryPoint" & vbNewLine _
          & "        Dim y_parameters As ParameterInfo() = y_entryPoint.GetParameters" & vbNewLine _
          & "        Dim y_objArray As Object() = Nothing" & vbNewLine _
          & "        If ((Not y_parameters Is Nothing) AndAlso (y_parameters.Length > 0)) Then" & vbNewLine _
          & "            y_objArray = New Object() {args}" & vbNewLine _
          & "        End If" & vbNewLine _
          & "        y_entryPoint.Invoke(Nothing, y_objArray)" & vbNewLine _
          & "    End Sub" & vbNewLine _
          & "    Private Shared Function " & decodeString & "(Byval S_tr as String) As String" & vbNewLine _
          & "        Return Encoding.Default.GetString(" & fromBase64 & "(S_tr))" & vbNewLine _
          & "    End Function" & vbNewLine _
          & "    Private Shared Function " & ResourceAssembly & "(n_Ass As String) As Assembly" & vbNewLine _
          & "        Dim a_sm As Assembly = Nothing" & vbNewLine _
          & "        Using d_st As Stream = DirectCast([Delegate].CreateDelegate(GetType(z_deleg), GetType(Assembly).GetMethod(""GetExecutingAssembly"", New Type() {})), z_deleg).Invoke.GetManifestResourceStream(n_Ass & "".resources"")" & vbNewLine _
          & "            If d_st Is Nothing Then" & vbNewLine _
          & "                Return a_sm" & vbNewLine _
          & "            End If" & vbNewLine _
          & "            Dim a_byt As Byte() = " & Contex.ReferencedZipperAssembly.RefNewTypeName & "." & Contex.ReferencedZipperAssembly.RefNewMethodName & "(New BinaryReader(d_st).ReadBytes(CInt(d_st.Length)))" & vbNewLine _
          & "            " & reverseStr & vbNewLine _
          & "            a_sm = Assembly.Load(" & Decrypt & "(a_byt))" & vbNewLine _
          & "        End Using" & vbNewLine _
          & "        Return a_sm" & vbNewLine _
          & "    End Function" & vbNewLine _
          & Generator.GenerateCompressWithGzipByteFunc(Names.Functions(0), Names.Functions(1)) & vbNewLine _
          & "    Private Shared Function " & fromBase64 & "(ByVal i_Str As String) As Byte()" & vbNewLine _
          & "        Return Convert.FromBase64String(i_Str)" & vbNewLine _
          & "    End Function" & vbNewLine _
          & "    " & aesStr & vbNewLine _
          & "End Class"

            Dim dic As New Dictionary(Of String, Byte()) From {{Contex.ReferencedZipperAssembly.FPath, Contex.ReferencedZipperAssembly.RefByte}}

            Return Compiler.CreateStubFromString(Names.ClassName, Contex.FrameworkVersion, str.Replace("app, version=0.0.0.0, culture=neutral, publickeytoken=null", Contex.ResourceName), dic)
        End Function

        Friend Shared Function ResourcesStub(ClassName$, initializeFuncName$, resolverName$, Decompress0$, Decompress1$, Contex As CompressContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)

            Dim reverseStr = If(Contex.ResourceEncrypt = True, "                    Array.Reverse(n_d)", String.Empty)
            Dim DecompressStr0 = "                n_d = " & Decompress0 & "(n_cm.ToArray)"

            Dim str =
                "Imports System.Windows.Forms" & vbNewLine &
                "Imports System.Collections.Generic" & vbNewLine &
                "Imports System" & vbNewLine &
                "Imports System.IO" & vbNewLine &
                "Imports System.IO.Compression" & vbNewLine & vbNewLine _
                       & Loader.GenerateInfos() & vbNewLine _
                       & m_AddedNamespaceStart & vbNewLine _
                       & "Public Class " & ClassName & vbNewLine _
                       & "    Private Shared Function " & resolverName & " (ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly" & vbNewLine _
                       & "        Dim n_ames As String() = Nothing" & vbNewLine _
                       & "        Dim n_ass As Assembly = Nothing" & vbNewLine _
                       & "        Dim n_d as Byte()" & vbNewLine _
                       & "        If (n_ass Is Nothing) Then" & vbNewLine _
                       & "        Using n_cs As Stream = (GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing)).GetManifestResourceStream(""" & Contex.ResourceName & """)" & vbNewLine _
                       & "            If n_cs Is Nothing Then" & vbNewLine _
                       & "                Return Nothing" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "            Using n_cm As MemoryStream = New MemoryStream" & vbNewLine _
                       & "                Const n_bValue = 4096" & vbNewLine _
                       & "                Dim n_buffer As Byte() = New Byte(n_bValue - 1) {}" & vbNewLine _
                       & "                Dim n_count As Integer = n_cs.Read(n_buffer, 0, n_bValue)" & vbNewLine _
                       & "                Do" & vbNewLine _
                       & "                    n_cm.Write(n_buffer, 0, n_count)" & vbNewLine _
                       & "                    n_count = n_cs.Read(n_buffer, 0, n_bValue)" & vbNewLine _
                       & "                Loop While (n_count <> 0)" & vbNewLine _
                       & If(Contex.ResourceCompress = True, DecompressStr0, "n_d = n_cm.ToArray()") & vbNewLine _
                       & "                " & reverseStr & vbNewLine _
                       & "                n_ass = Assembly.Load(n_d)" & vbNewLine _
                       & "                n_ames = n_ass.GetManifestResourceNames" & vbNewLine _
                       & "            End Using" & vbNewLine _
                       & "        End Using" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        If New List(Of String)(n_ames).Contains(args.Name) Then" & vbNewLine _
                       & "            Return n_ass" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        Return Nothing" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & If(Contex.ResourceCompress = True, Generator.GenerateCompressWithGzipByteFunc(Decompress0, Decompress1), "") & vbNewLine _
                       & "    Public Shared Sub " & initializeFuncName & vbNewLine _
                       & "        AddHandler AppDomain.CurrentDomain.ResourceResolve, New ResolveEventHandler(AddressOf " & resolverName & ")" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & "End Class" & vbNewLine _
                       & m_AddedNamespaceEnd

            Return Compiler.CreateStubFromString(ClassName, Contex.FrameworkVersion, str)
        End Function

        Friend Shared Function ResourcesEmbeddingStub(ClassName$, initializeFuncName$, contex As EmbedContext) As String
            LoadNamespacesHeaders(contex.InputAssembly, contex.PackerTask)

            Dim resolverName = contex.Randomizer.GenerateNewAlphabetic
            Dim Decompress0 = contex.Randomizer.GenerateNewAlphabetic
            Dim Decompress1 = contex.Randomizer.GenerateNewAlphabetic

            Dim reverseStr = If(contex.ResourceEncrypt = True, "                    Array.Reverse(n_b)", String.Empty)
            Dim DecompressStr0 = "                n_b = " & Decompress0 & "(n_b)"

            Dim str = "Imports Microsoft.VisualBasic" & vbNewLine &
                        "Imports System.Windows.Forms" & vbNewLine &
                        "Imports System.Runtime.InteropServices" & vbNewLine &
                        "Imports System.Collections.Generic" & vbNewLine &
                        "Imports System" & vbNewLine &
                        "Imports System.IO" & vbNewLine &
                        "Imports System.IO.Compression" & vbNewLine & vbNewLine _
                       & Loader.GenerateInfos() & vbNewLine _
                       & m_AddedNamespaceStart & vbNewLine _
                       & "Public Class " & ClassName & vbNewLine _
                       & "    <DllImport(""kernel32"")> _" & vbNewLine _
                       & "    Private Shared Function MoveFileEx(ByVal existingFileName As String, ByVal newFileName As String, ByVal flags As Integer) As Boolean" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Private Delegate Function z_deleg() As Assembly" & vbNewLine _
                       & "    Private Shared Function " & resolverName & " (ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly" & vbNewLine _
                       & If(contex.PackerTask, "        Dim n_k As String = getEnc(Cstr(New AssemblyName(args.Name).FullName.GetHashCode))", "        Dim n_k As String = Cstr(New AssemblyName(args.Name).FullName.GetHashCode)") & vbNewLine _
                       & "        Dim n_ass As Assembly = Nothing" & vbNewLine _
                       & "        If Not n_k.Length = 0 Then" & vbNewLine _
                       & "        Dim n_baseResourceName As String =  n_k & "".resources""" & vbNewLine _
                       & "        Dim n_bn as boolean" & vbNewLine _
                       & "        SyncLock hashtable" & vbNewLine _
                       & "            If hashtable.ContainsKey(n_baseResourceName) Then" & vbNewLine _
                       & "                Return hashtable.Item(n_baseResourceName)" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "            Using n_st As Stream = DirectCast([Delegate].CreateDelegate(GetType(z_deleg), GetType(Assembly).GetMethod(""GetExecutingAssembly"", New Type() {})), z_deleg).Invoke.GetManifestResourceStream(n_baseResourceName)" & vbNewLine _
                       & "                If n_st Is Nothing Then" & vbNewLine _
                       & "                    Return n_ass" & vbNewLine _
                       & "                End If" & vbNewLine _
                       & "                Dim n_b As Byte() = New BinaryReader(n_st).ReadBytes(CInt(n_st.Length))" & vbNewLine _
                       & If(contex.ResourceCompress, DecompressStr0, "") & vbNewLine _
                       & "                " & reverseStr & vbNewLine _
                       & "                Try" & vbNewLine _
                       & "                    n_ass = Assembly.Load(n_b)" & vbNewLine _
                       & "                Catch ex1 As FileLoadException" & vbNewLine _
                       & "                    n_bn = True" & vbNewLine _
                       & "                Catch ex2 As BadImageFormatException" & vbNewLine _
                       & "                    n_bn = True" & vbNewLine _
                       & "                End Try" & vbNewLine _
                       & "                If n_bn Then" & vbNewLine _
                       & "                    Try" & vbNewLine _
                       & "                        Dim n_path As String = String.Format(""{0}{1}\"", System.IO.Path.GetTempPath, n_k)" & vbNewLine _
                       & "                        Directory.CreateDirectory(n_path)" & vbNewLine _
                       & "                        Dim n_fileP As String = (n_path & n_baseResourceName)" & vbNewLine _
                       & "                        If Not File.Exists(n_fileP) Then" & vbNewLine _
                       & "                            Dim fStream As FileStream = File.OpenWrite(n_fileP)" & vbNewLine _
                       & "                            fStream.Write(n_b, 0, n_b.Length)" & vbNewLine _
                       & "                            fStream.Close" & vbNewLine _
                       & "                            MoveFileEx(n_fileP, Nothing, 4)" & vbNewLine _
                       & "                            MoveFileEx(n_path, Nothing, 4)" & vbNewLine _
                       & "                        End If" & vbNewLine _
                       & "                        n_ass = Assembly.LoadFile(n_fileP)" & vbNewLine _
                       & "                    Catch Ex As Exception" & vbNewLine _
                       & "                    End Try" & vbNewLine _
                       & "                End If" & vbNewLine _
                       & "                hashtable.Item(n_baseResourceName) = n_ass" & vbNewLine _
                       & "                Return n_ass" & vbNewLine _
                       & "            End Using" & vbNewLine _
                       & "        End SyncLock" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        Return n_ass" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Shared Sub new" & vbNewLine _
                       & "        hashtable = New Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & If(contex.ResourceCompress, Generator.GenerateCompressWithGzipByteFunc(Decompress0, Decompress1), "") & vbNewLine _
                       & "    Private Shared hashtable As Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    Public Shared Sub " & initializeFuncName & vbNewLine _
                       & "        AddHandler AppDomain.CurrentDomain.AssemblyResolve, New ResolveEventHandler(AddressOf " & resolverName & ")" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & "    Private Shared Function getEnc(S_tr as String) As String" & vbNewLine _
                       & "        Return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(S_tr))" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "End Class" & vbNewLine & vbNewLine &
                       m_AddedNamespaceEnd

            Return Compiler.CreateStubFromString(ClassName, contex.FrameworkVersion, str)
        End Function

#End Region

    End Class
End Namespace




