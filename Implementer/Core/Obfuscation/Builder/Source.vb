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
                       & "    Public Shared Function " & ResourceDecryptFunc & " (ByVal BaseStreamPos As Integer) As String" & vbNewLine _
                       & "        Dim br As New BinaryReader(" & ms & ")" & vbNewLine _
                       & "        br.BaseStream.Position = BaseStreamPos" & vbNewLine _
                       & "        Return br.ReadString" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Shared Sub New" & vbNewLine _
                       & "        If " & ms & " Is Nothing Then" & vbNewLine _
                       & "           Dim by as Byte()" & vbNewLine _
                       & "           by = " & Decompress0 & "(Assembly.GetExecutingAssembly.GetManifestResourceStream(""" & Contex.ResourceName & """))" & vbNewLine _
                       & "           " & ms & " = New MemoryStream(by)" & vbNewLine _
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

        'Friend Shared Function DecryptXorType(ClassName$, DecryptXorFuncName$, Contex As StubContext) As Type
        '    LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)
        '    Dim str = "Imports System" & vbNewLine _
        '              & Loader.GenerateInfos() & vbNewLine _
        '              & Generator.GenerateDecryptXorFunc(ClassName, DecryptXorFuncName)
        '    Return Compiler.CreateTypeFromString(ClassName, Contex.FrameworkVersion, str)
        'End Function
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
            "   Dim parameter As New Thread(New ParameterizedThreadStart(AddressOf " & FuncName2 & "))" & vbNewLine &
            "   Dim t As New Thread(New ParameterizedThreadStart(AddressOf " & FuncName2 & "))" & vbNewLine &
            "   parameter.IsBackground = True" & vbNewLine &
            "   t.IsBackground = True" & vbNewLine &
            "   parameter.Start(t)" & vbNewLine &
            "   Thread.Sleep(500)" & vbNewLine &
            "   t.Start(parameter)" & vbNewLine &
            "End Sub" & vbNewLine & vbNewLine &
            "Private Shared Sub " & FuncName2 & "(ByVal th As Object)" & vbNewLine &
            "   Thread.Sleep(&H3E8)" & vbNewLine &
            "   Dim t As Thread = DirectCast(th, Thread)" & vbNewLine &
            "   Do While True" & vbNewLine &
            "       If (Debugger.IsAttached OrElse Debugger.IsLogging) Then" & vbNewLine &
            "           Environment.FailFast(""Debugger detected (Managed)"")" & vbNewLine &
            "       End If" & vbNewLine &
            "       If Not t.IsAlive Then" & vbNewLine &
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
                 & "        Dim l As String = (GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing)).Location" & vbNewLine _
                 & "        Dim b As Stream = New StreamReader(l).BaseStream" & vbNewLine _
                 & "        Dim r As New BinaryReader(b)" & vbNewLine _
                 & "        Dim b0 As String = Nothing" & vbNewLine _
                 & "        Dim b1 As String = Nothing" & vbNewLine _
                 & "        b0 = BitConverter.ToString(Ctype(CryptoConfig.CreateFromName(" & Chr(34) & "MD5" & Chr(34) & "), HashAlgorithm).ComputeHash(r.ReadBytes((File.ReadAllBytes(l).Length - 16))))" & vbNewLine _
                 & "        b.Seek(-16, SeekOrigin.End)" & vbNewLine _
                 & "        b1 = BitConverter.ToString(r.ReadBytes(16))" & vbNewLine _
                 & "        If (b0 <> b1) Then" & vbNewLine _
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
                       & "    Public Shared Function " & m_invokeMethFuncName & " (Of T As Class)(libF$, funcN$) As T" & vbNewLine _
                       & "        Dim ll As IntPtr = " & m_loadLibraryFuncName & "(libF)" & vbNewLine _
                       & "        Dim delegT As System.Delegate = Marshal.GetDelegateForFunctionPointer(" & m_getMethProcFuncName & "(ll, funcN), GetType(T))" & vbNewLine _
                       & "        Return TryCast(delegT, T)" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "End Class" & vbNewLine _
                       & m_AddedNamespaceEnd
            Return Compiler.CreateStubFromString(className, Contex.FrameworkVersion, str)
        End Function
#End Region

        Friend Shared Function SevenZipStub(ClassName$, initializeFuncName$, resolverName$, Decompress0$, Decompress1$, Contex As CompressContext) As String
            Dim reverseStr = If(Contex.ResourceEncrypt = True, "                    Array.Reverse(d)", String.Empty)
            Dim DecompressStr0 = "                d = " & Decompress0 & "(cm.ToArray)"

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
                       & "        Dim names As String() = Nothing" & vbNewLine _
                       & "        Dim k As String = Cstr(New AssemblyName(args.Name).Name)" & vbNewLine _
                       & "        'Dim directoryName As String = Path.GetDirectoryName(Assembly.GetEntryAssembly.Location)" & vbNewLine _
                       & "        'msgbox(directoryName)" & vbNewLine _
                       & "        'For each f As String in Directory.GetFiles(""*.dll"", Assembly.GetExecutingAssembly.Location)" & vbNewLine _
                       & "            'MsgbOx(Assembly.GetExecutingAssembly.Location)" & vbNewLine _
                       & "            'Try" & vbNewLine _
                       & "                'If f.EndsWith("".xmlserializers.dll"", StringComparison.OrdinalIgnoreCase) Then" & vbNewLine _
                       & "                    'Msgbox(""oui"")" & vbNewLine _
                       & "                    'Return Assembly.LoadFrom(f)" & vbNewLine _
                       & "                'End If" & vbNewLine _
                       & "             " & vbNewLine _
                       & "            'Catch Ex As Exception" & vbNewLine _
                       & "            'End Try" & vbNewLine _
                       & "        'Next" & vbNewLine _
                       & "        Dim ass As Assembly = Nothing" & vbNewLine _
                       & "        Dim d as Byte()" & vbNewLine _
                       & "        'Dim k As String = Cstr(New AssemblyName(args.Name).Name)" & vbNewLine _
                       & "        'MsGbox(k & vbnewline)" & vbNewLine _
                       & "        If k.EndsWith("".xmlserializers"", StringComparison.OrdinalIgnoreCase) Then" & vbNewLine _
                       & "            Return Nothing" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        If k.EndsWith("".resources"", StringComparison.OrdinalIgnoreCase) Then" & vbNewLine _
                       & "            Return Nothing" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        SyncLock hashtable" & vbNewLine _
                       & "            If hashtable.ContainsKey(""" & Contex.ResourceName & """) Then" & vbNewLine _
                       & "                Return Nothing" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "        Using cs As Stream = (GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing)).GetManifestResourceStream(""" & Contex.ResourceName & """)" & vbNewLine _
                       & "            If cs Is Nothing Then" & vbNewLine _
                       & "                Return Nothing" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "            Using cm As MemoryStream = New MemoryStream" & vbNewLine _
                       & "                Const bValue = 4096" & vbNewLine _
                       & "                Dim buffer As Byte() = New Byte(bValue - 1) {}" & vbNewLine _
                       & "                Dim count As Integer = cs.Read(buffer, 0, bValue)" & vbNewLine _
                       & "                Do" & vbNewLine _
                       & "                    cm.Write(buffer, 0, count)" & vbNewLine _
                       & "                    count = cs.Read(buffer, 0, bValue)" & vbNewLine _
                       & "                Loop While (count <> 0)" & vbNewLine _
                       & If(Contex.ResourceCompress = True, DecompressStr0, "d = cm.ToArray()") & vbNewLine _
                       & "                " & reverseStr & vbNewLine _
                       & "                ass = Assembly.Load(d)" & vbNewLine _
                       & "                hashtable.Item(""" & Contex.ResourceName & """) = ass" & vbNewLine _
                       & "                Return ass" & vbNewLine _
                       & "                'Msgbox(""Test"" & vbnewline & ass.GetName.Name)" & vbNewLine _
                       & "            End Using" & vbNewLine _
                       & "        End Using" & vbNewLine _
                       & "        End SyncLock" & vbNewLine _
                       & "        Return Nothing" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Shared Sub new" & vbNewLine _
                       & "        hashtable = New Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & If(Contex.ResourceCompress = True, Generator.GenerateCompressWithGzipByteFunc(Decompress0, Decompress1), "") & vbNewLine _
                       & "    Private Shared hashtable As Dictionary(Of String, Assembly)" & vbNewLine _
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
                reverseStr = "                    Array.Reverse(byt)"
            End If

            Dim aesStr As String = "    Private Shared Function " & Decrypt & "(ByVal iByte As Byte()) As Byte()" & vbNewLine _
                        & "        Dim k as Byte() = " & Contex.ReferencedZipperAssembly.RefNewTypeName & ".pKey(""" & Convert.ToBase64String(SevenZipLib.SevenZipHelper.Compress(Contex.PolyXor.Key)) & """)" & vbNewLine _
                        & "        Dim O As Byte() = New Byte(iByte.Length - " & Contex.PolyXor.SaltSize.ToString & " - 1) {}" & vbNewLine _
                        & "        Dim S As Byte() = New Byte(" & Contex.PolyXor.SaltSize.ToString & " - 1) {}" & vbNewLine _
                        & "        Buffer.BlockCopy(iByte, iByte.Length - " & Contex.PolyXor.SaltSize.ToString & ", S, 0, " & Contex.PolyXor.SaltSize.ToString & ")" & vbNewLine _
                        & "        Array.Resize(Of Byte)(iByte, iByte.Length - " & Contex.PolyXor.SaltSize.ToString & ")" & vbNewLine _
                        & "        For j As Integer = 0 To iByte.Length - 1" & vbNewLine _
                        & "            O(j) = CByte(iByte(j) Xor k(j Mod k.Length) Xor S(j Mod S.Length))" & vbNewLine _
                        & "        Next" & vbNewLine _
                        & "        Return O" & vbNewLine _
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
          & "    Private Delegate Function z() As Assembly" & vbNewLine _
          & "    <STAThread()> _" & vbNewLine _
          & "    Public Shared Sub Main(ByVal args As String())" & vbNewLine _
          & "        Dim yassembly As Assembly = " & ResourceAssembly & "((""app, version=0.0.0.0, culture=neutral, publickeytoken=null"").Replace("".resources"",""""))" & vbNewLine _
          & "        Dim yentryPoint As MethodInfo = yassembly.EntryPoint" & vbNewLine _
          & "        Dim yparameters As ParameterInfo() = yentryPoint.GetParameters" & vbNewLine _
          & "        Dim yobjArray As Object() = Nothing" & vbNewLine _
          & "        If ((Not yparameters Is Nothing) AndAlso (yparameters.Length > 0)) Then" & vbNewLine _
          & "            yobjArray = New Object() {args}" & vbNewLine _
          & "        End If" & vbNewLine _
          & "        yentryPoint.Invoke(Nothing, yobjArray)" & vbNewLine _
          & "    End Sub" & vbNewLine _
          & "    Private Shared Function " & decodeString & "(Byval Str as String) As String" & vbNewLine _
          & "        Return Encoding.Default.GetString(" & fromBase64 & "(Str))" & vbNewLine _
          & "    End Function" & vbNewLine _
          & "    Private Shared Function " & ResourceAssembly & "(nAss As String) As Assembly" & vbNewLine _
          & "        Dim asm As Assembly = Nothing" & vbNewLine _
          & "        Using st As Stream = DirectCast([Delegate].CreateDelegate(GetType(z), GetType(Assembly).GetMethod(""GetExecutingAssembly"", New Type() {})), z).Invoke.GetManifestResourceStream(nAss & "".resources"")" & vbNewLine _
          & "            If st Is Nothing Then" & vbNewLine _
          & "                Return asm" & vbNewLine _
          & "            End If" & vbNewLine _
          & "            Dim byt As Byte() = " & Contex.ReferencedZipperAssembly.RefNewTypeName & "." & Contex.ReferencedZipperAssembly.RefNewMethodName & "(New BinaryReader(st).ReadBytes(CInt(st.Length)))" & vbNewLine _
          & "            " & reverseStr & vbNewLine _
          & "            asm = Assembly.Load(" & Decrypt & "(byt))" & vbNewLine _
          & "        End Using" & vbNewLine _
          & "        Return asm" & vbNewLine _
          & "    End Function" & vbNewLine _
          & Generator.GenerateCompressWithGzipByteFunc(Names.Functions(0), Names.Functions(1)) & vbNewLine _
          & "    Private Shared Function " & fromBase64 & "(ByVal iStr As String) As Byte()" & vbNewLine _
          & "        Return Convert.FromBase64String(iStr)" & vbNewLine _
          & "    End Function" & vbNewLine _
          & "    " & aesStr & vbNewLine _
          & "End Class"

            Dim dic As New Dictionary(Of String, Byte()) From {{Contex.ReferencedZipperAssembly.FPath, Contex.ReferencedZipperAssembly.RefByte}}

            Return Compiler.CreateStubFromString(Names.ClassName, Contex.FrameworkVersion, str.Replace("app, version=0.0.0.0, culture=neutral, publickeytoken=null", Contex.ResourceName), dic)
        End Function

        Friend Shared Function ResourcesStub(ClassName$, initializeFuncName$, resolverName$, Decompress0$, Decompress1$, Contex As CompressContext) As String
            LoadNamespacesHeaders(Contex.InputAssembly, Contex.PackerTask)

            Dim reverseStr = If(Contex.ResourceEncrypt = True, "                    Array.Reverse(d)", String.Empty)
            Dim DecompressStr0 = "                d = " & Decompress0 & "(cm.ToArray)"

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
                       & "        Dim names As String() = Nothing" & vbNewLine _
                       & "        Dim ass As Assembly = Nothing" & vbNewLine _
                       & "        Dim d as Byte()" & vbNewLine _
                       & "        If (ass Is Nothing) Then" & vbNewLine _
                       & "        Using cs As Stream = (GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing)).GetManifestResourceStream(""" & Contex.ResourceName & """)" & vbNewLine _
                       & "            If cs Is Nothing Then" & vbNewLine _
                       & "                Return Nothing" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "            Using cm As MemoryStream = New MemoryStream" & vbNewLine _
                       & "                Const bValue = 4096" & vbNewLine _
                       & "                Dim buffer As Byte() = New Byte(bValue - 1) {}" & vbNewLine _
                       & "                Dim count As Integer = cs.Read(buffer, 0, bValue)" & vbNewLine _
                       & "                Do" & vbNewLine _
                       & "                    cm.Write(buffer, 0, count)" & vbNewLine _
                       & "                    count = cs.Read(buffer, 0, bValue)" & vbNewLine _
                       & "                Loop While (count <> 0)" & vbNewLine _
                       & If(Contex.ResourceCompress = True, DecompressStr0, "d = cm.ToArray()") & vbNewLine _
                       & "                " & reverseStr & vbNewLine _
                       & "                ass = Assembly.Load(d)" & vbNewLine _
                       & "                names = ass.GetManifestResourceNames" & vbNewLine _
                       & "            End Using" & vbNewLine _
                       & "        End Using" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        If New List(Of String)(names).Contains(args.Name) Then" & vbNewLine _
                       & "            Return ass" & vbNewLine _
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

            Dim reverseStr = If(contex.ResourceEncrypt = True, "                    Array.Reverse(b)", String.Empty)
            Dim DecompressStr0 = "                b = " & Decompress0 & "(b)"

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
                       & "    Private Delegate Function z() As Assembly" & vbNewLine _
                       & "    Private Shared Function " & resolverName & " (ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly" & vbNewLine _
                       & If(contex.PackerTask, "        Dim k As String = getEnc(Cstr(New AssemblyName(args.Name).FullName.GetHashCode))", "        Dim k As String = Cstr(New AssemblyName(args.Name).FullName.GetHashCode)") & vbNewLine _
                       & "        Dim ass As Assembly = Nothing" & vbNewLine _
                       & "        If Not k.Length = 0 Then" & vbNewLine _
                       & "        Dim baseResourceName As String =  k & "".resources""" & vbNewLine _
                       & "        Dim bn as boolean" & vbNewLine _
                       & "        SyncLock hashtable" & vbNewLine _
                       & "            If hashtable.ContainsKey(baseResourceName) Then" & vbNewLine _
                       & "                Return hashtable.Item(baseResourceName)" & vbNewLine _
                       & "            End If" & vbNewLine _
                       & "            Using st As Stream = DirectCast([Delegate].CreateDelegate(GetType(z), GetType(Assembly).GetMethod(""GetExecutingAssembly"", New Type() {})), z).Invoke.GetManifestResourceStream(baseResourceName)" & vbNewLine _
                       & "                If st Is Nothing Then" & vbNewLine _
                       & "                    Return ass" & vbNewLine _
                       & "                End If" & vbNewLine _
                       & "                Dim b As Byte() = New BinaryReader(st).ReadBytes(CInt(st.Length))" & vbNewLine _
                       & If(contex.ResourceCompress, DecompressStr0, "") & vbNewLine _
                       & "                " & reverseStr & vbNewLine _
                       & "                Try" & vbNewLine _
                       & "                    ass = Assembly.Load(b)" & vbNewLine _
                       & "                Catch ex1 As FileLoadException" & vbNewLine _
                       & "                    bn = True" & vbNewLine _
                       & "                Catch ex2 As BadImageFormatException" & vbNewLine _
                       & "                    bn = True" & vbNewLine _
                       & "                End Try" & vbNewLine _
                       & "                If bn Then" & vbNewLine _
                       & "                    Try" & vbNewLine _
                       & "                        Dim npath As String = String.Format(""{0}{1}\"", System.IO.Path.GetTempPath, k)" & vbNewLine _
                       & "                        Directory.CreateDirectory(npath)" & vbNewLine _
                       & "                        Dim nfileP As String = (npath & baseResourceName)" & vbNewLine _
                       & "                        If Not File.Exists(nfileP) Then" & vbNewLine _
                       & "                            Dim fStream As FileStream = File.OpenWrite(nfileP)" & vbNewLine _
                       & "                            fStream.Write(b, 0, b.Length)" & vbNewLine _
                       & "                            fStream.Close" & vbNewLine _
                       & "                            MoveFileEx(nfileP, Nothing, 4)" & vbNewLine _
                       & "                            MoveFileEx(npath, Nothing, 4)" & vbNewLine _
                       & "                        End If" & vbNewLine _
                       & "                        ass = Assembly.LoadFile(nfileP)" & vbNewLine _
                       & "                    Catch Ex As Exception" & vbNewLine _
                       & "                    End Try" & vbNewLine _
                       & "                End If" & vbNewLine _
                       & "                hashtable.Item(baseResourceName) = ass" & vbNewLine _
                       & "                Return ass" & vbNewLine _
                       & "            End Using" & vbNewLine _
                       & "        End SyncLock" & vbNewLine _
                       & "        End If" & vbNewLine _
                       & "        Return ass" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "    Shared Sub new" & vbNewLine _
                       & "        hashtable = New Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & If(contex.ResourceCompress, Generator.GenerateCompressWithGzipByteFunc(Decompress0, Decompress1), "") & vbNewLine _
                       & "    Private Shared hashtable As Dictionary(Of String, Assembly)" & vbNewLine _
                       & "    Public Shared Sub " & initializeFuncName & vbNewLine _
                       & "        AddHandler AppDomain.CurrentDomain.AssemblyResolve, New ResolveEventHandler(AddressOf " & resolverName & ")" & vbNewLine _
                       & "    End Sub" & vbNewLine _
                       & "    Private Shared Function getEnc(Str$) As String" & vbNewLine _
                       & "        Return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Str))" & vbNewLine _
                       & "    End Function" & vbNewLine _
                       & "End Class" & vbNewLine & vbNewLine &
                       m_AddedNamespaceEnd

            Return Compiler.CreateStubFromString(ClassName, contex.FrameworkVersion, str)
        End Function

#End Region

    End Class
End Namespace




