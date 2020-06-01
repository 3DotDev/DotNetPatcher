Namespace CryptoHelper
    Public NotInheritable Class Generator

#Region " Fields "
        Public Shared numberPrime As Integer() = New Integer() {547, 557, 563, 569, 571, 577, 587, 593, 599, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659,
                                             739, 743, 751, 757, 761, 769, 773, 787, 797, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997,
                                             1523, 1531, 1543, 1549, 1553, 1559, 1567, 1571, 1579, 1583, 2063, 2069, 2081, 2083, 2087, 2089, 2099, 2111, 2113, 2129,
                                             3659, 3671, 3673, 3677, 3691, 3697, 3701, 3709, 3719, 3727, 4153, 4157, 4159, 4177, 4201, 4211, 4217, 4219, 4229, 4231,
                                             5281, 5297, 5303, 5309, 5323, 5333, 5347, 5351, 5381, 5387, 6311, 6317, 6323, 6329, 6337, 6343, 6353, 6359, 6361, 6367,
                                             9901, 9907, 9923, 9929, 9931, 9941, 9949, 9967, 9973}

        Public Shared numberUnPrime As Integer() = New Integer() {548, 549, 550, 570, 572, 578, 588, 594, 600, 608, 614, 618, 620, 632, 642, 644, 649, 655, 660,
                                                          740, 744, 752, 758, 763, 770, 774, 789, 799, 812, 822, 826, 889, 830, 840, 855, 858, 861, 869, 939, 949, 948, 955, 970, 972, 979, 984, 992, 999,
                                                          1525, 1535, 1547, 1550, 1554, 1561, 1569, 1573, 1585, 1584, 2065, 2070, 2082, 2085, 2090, 2091, 2092, 2112, 2116, 2130,
                                                          3660, 3675, 3674, 3679, 3696, 3699, 3703, 3711, 3720, 3728, 4156, 4158, 4160, 4178, 4209, 4215, 4218, 4220, 4230, 4236,
                                                          5282, 5298, 5306, 5326, 5335, 5348, 5354, 5386, 5388, 6313, 6319, 6325, 6330, 6339, 6345, 6351, 6360, 6365, 6363,
                                                          9905, 9909, 9928, 9930, 9946, 9950, 9970, 9974}
#End Region

#Region " Methods "
        Public Shared Function IntEncrypt(num%, integ%) As String
            Dim ch As Char
            Dim str = String.Empty
            Dim num2 = New Random().Next(0, &H13)
            Dim random As New Random
            Dim i%
            For i = 0 To &H13 - 1
                If (i = num2) Then
                    ch = ChrW(integ)
                    str = (str & num.ToString & ch.ToString)
                Else
                    str = (str & random.Next(100).ToString & ChrW(integ).ToString)
                End If
            Next i
            ch = ChrW(integ)
            Return (str & num2.ToString & ch.ToString)
        End Function

        Public Shared Function GenerateDecryptIntFunc(_ClassIntName$, _DecryptIntFuncName$) As String
            Dim str = "Public Class " & _ClassIntName & vbNewLine _
                              & "    Public Shared Function " & _DecryptIntFuncName & " (ByVal dString$, Byval integ%) As Integer" & vbNewLine _
                              & "        Dim sString As String() = dString.Split(New Char() {Strings.ChrW(integ)})" & vbNewLine _
                              & "        Dim nString As Integer() = New Integer((sString.Length - 1) - 1) {}" & vbNewLine _
                              & "        Dim iIncrement%" & vbNewLine _
                              & "        For iIncrement = 0 To nString.Length - 1" & vbNewLine _
                              & "            nString(iIncrement) = Integer.Parse(sString(iIncrement))" & vbNewLine _
                              & "        Next iIncrement" & vbNewLine _
                              & "        Return nString(nString(nString.Length - 1))" & vbNewLine _
                              & "    End Function" & vbNewLine _
                              & "End Class"
            Return str
        End Function

        Public Shared Function GenerateDecryptOddFunc(_ClassOddName$, _DecryptOddFuncName$) As String
            Dim str = "Public Class " & _ClassOddName & vbNewLine _
                              & "    Public Shared Function " & _DecryptOddFuncName & " (ByVal numInteg%) As Boolean" & vbNewLine _
                              & "        Return numInteg Mod 2 <> 0" & vbNewLine _
                              & "    End Function" & vbNewLine _
                              & "End Class"
            Return str
        End Function

        Public Shared Function GenerateDecryptXorFunc(_ClassXorName$, _DecryptXorFuncName$) As String
            Dim str = "Public Class " & _ClassXorName & vbNewLine _
                              & "    Public Shared Function " & _DecryptXorFuncName & " (ByVal tString as String, ByVal numInteg%) As String" & vbNewLine _
                              & "        Dim sResult$ = String.Empty" & vbNewLine _
                              & "        Dim sLength% = (tString.Length - 1)" & vbNewLine _
                              & "        Dim jincrement% = 0" & vbNewLine _
                              & "        Do While (jincrement <= sLength)" & vbNewLine _
                              & "            Dim p% = (Convert.ToInt32(tString.Chars(jincrement)) Xor numInteg)" & vbNewLine _
                              & "            sResult = (sResult & Char.ConvertFromUtf32(p))" & vbNewLine _
                              & "            jincrement += 1" & vbNewLine _
                              & "        Loop" & vbNewLine _
                              & "        Return sResult" & vbNewLine _
                              & "    End Function" & vbNewLine _
                              & "End Class"

            Return str
        End Function

        Public Shared Function GenerateCompressWithGzipByteFunc(_Decompress0$, Decompress1$) As String
            Return "    Public Shared Function " & _Decompress0 & "(ByVal datByte As Byte()) As Byte()" & vbNewLine _
                     & "        Try : Return " & Decompress1 & "(New GZipStream(New MemoryStream(datByte), CompressionMode.Decompress, False), datByte.Length)" & vbNewLine _
                     & "        Catch : Return Nothing : End Try" & vbNewLine _
                     & "    End Function" & vbNewLine _
                     & GenerateDeCompressWithGzipFunc(Decompress1)
        End Function

        Public Shared Function GenerateDeCompressWithGzipStreamFunc(_Decompress0$, Decompress1$) As String
            Return "    Private Shared Function " & _Decompress0 & "(ByVal datStream As Stream) As Byte()" & vbNewLine _
                     & "        Try : Return " & Decompress1 & "(New GZipStream(datStream, CompressionMode.Decompress, False), datStream.Length)" & vbNewLine _
                     & "        Catch : Return Nothing : End Try" & vbNewLine _
                     & "    End Function" & vbNewLine _
                     & GenerateDeCompressWithGzipFunc(Decompress1)
        End Function

        Private Shared Function GenerateDeCompressWithGzipFunc(Decompress1$) As String
            Return "    Public Shared Function " & Decompress1 & "(ByVal datStream As Stream, ByVal KeyInteg As Integer) As Byte()" & vbNewLine _
                     & "        Dim datByte() As Byte : Dim tbyte As Int32 = 0" & vbNewLine _
                     & "        Try : While True" & vbNewLine _
                     & "            ReDim Preserve datByte(tbyte + KeyInteg)" & vbNewLine _
                     & "            Dim br As Int32 = datStream.Read(datByte, tbyte, KeyInteg)" & vbNewLine _
                     & "            If br = 0 Then Exit While" & vbNewLine _
                     & "                tbyte += br" & vbNewLine _
                     & "              End While" & vbNewLine _
                     & "            ReDim Preserve datByte(tbyte - 1)" & vbNewLine _
                     & "            Return datByte" & vbNewLine _
                     & "        Catch : Return Nothing : End Try" & vbNewLine _
                     & "    End Function" & vbNewLine
        End Function

        Public Shared Function GenereateDecryptPrimeFunc(_FunctionName$) As String
            Return "Public Shared Function " & _FunctionName & " (Byval numberInteg as Integer) As Boolean" & vbNewLine _
                 & "        Dim boolVal As Boolean = True" & vbNewLine _
                 & "        Dim halfNum as integer = numberInteg / 2" & vbNewLine _
                 & "        Dim iIncrement as integer = 0" & vbNewLine _
                 & "        For iIncrement= 2 To halfNum" & vbNewLine _
                 & "            If (numberInteg Mod iIncrement) = 0 Then" & vbNewLine _
                 & "                boolVal = False" & vbNewLine _
                 & "            End If" & vbNewLine _
                 & "        Next" & vbNewLine _
                 & "        Return boolVal" & vbNewLine _
                 & "     End Function"
        End Function

        Public Shared Function GenerateDecryptRPNFunc(_FunctionName0$, _FunctionName1$) As String
            Return "    Public Shared Function " & _FunctionName0 & " (ByVal operands As String()) As Integer" & vbNewLine _
                 & "        Dim stackI As New Stack(Of Integer)" & vbNewLine _
                 & "        For Each opCod As String In operands" & vbNewLine _
                 & "            Select Case opCod" & vbNewLine _
                 & "                Case ""+""" & vbNewLine _
                 & "                    stackI.Push(stackI.Pop() + stackI.Pop())" & vbNewLine _
                 & "                Case ""-""" & vbNewLine _
                 & "                    stackI.Push(-stackI.Pop() + stackI.Pop())" & vbNewLine _
                 & "                Case ""*""" & vbNewLine _
                 & "                    stackI.Push(stackI.Pop() * stackI.Pop())" & vbNewLine _
                 & "                Case ""/""" & vbNewLine _
                 & "                    Dim tmpInt As Integer = stackI.Pop()" & vbNewLine _
                 & "                    stackI.Push(stackI.Pop() / tmpInt)" & vbNewLine _
                 & "                Case ""sqrt""" & vbNewLine _
                 & "                    stackI.Push(Math.Sqrt(stackI.Pop()))" & vbNewLine _
                 & "                Case Else" & vbNewLine _
                 & "                    stackI.Push(Integer.Parse(opCod))" & vbNewLine _
                 & "            End Select" & vbNewLine _
                 & "        Next" & vbNewLine _
                 & "        Return stackI.Pop()" & vbNewLine _
                 & "    End Function" & vbNewLine _
                 & "    Public Shared Function " & _FunctionName1 & " (ByVal expressionStr As String) As String()" & vbNewLine _
                 & "        Return expressionStr.ToLower().Split(New Char() {"",""c}, StringSplitOptions.RemoveEmptyEntries)" & vbNewLine _
                 & "    End Function" & vbNewLine
        End Function

        Public Shared Function GenerateReadFromResourcesFunc(ClassName$, ReadFromResourcesFuncName$, ResName$) As String
            Return "Public Class " & ClassName & vbNewLine _
                          & "    Public Shared Function " & ReadFromResourcesFuncName & " (ByVal ValueStr As String) As String" & vbNewLine _
                          & "        Dim ResourceMan As New ResourceManager(""" & ResName & """, GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing))" & vbNewLine _
                          & "        Dim strObject As String = DirectCast(ResourceMan.GetObject(ValueStr), String)" & vbNewLine _
                          & "        ResourceMan.ReleaseAllResources()" & vbNewLine _
                          & "        Return strObject" & vbNewLine _
                          & "    End Function" & vbNewLine _
                          & "End Class" & vbNewLine
        End Function

        Public Shared Function GenerateFromBase64Func(ClassName$, FromBase64FuncName$, GetStringFuncName$) As String
            Return "Public Class " & ClassName & vbNewLine _
                          & "    Public Shared Function " & FromBase64FuncName & " (stringStr As String, defEnc As Boolean) As Byte()" & vbNewLine _
                          & "        Dim bytes As Byte()" & vbNewLine _
                          & "        Using writer As MemoryStream = New MemoryStream()" & vbNewLine _
                          & "            Dim bufferedOutputBytes As Byte()" & vbNewLine _
                          & "            Dim inputBytes As Byte()" & vbNewLine _
                          & "            If defEnc Then" & vbNewLine _
                          & "                inputBytes = Encoding.Default.GetBytes(stringStr)" & vbNewLine _
                          & "            Else" & vbNewLine _
                          & "                inputBytes = Encoding.UTF8.GetBytes(stringStr)" & vbNewLine _
                          & "            End If" & vbNewLine _
                          & "            Using transformation As FromBase64Transform = New FromBase64Transform()" & vbNewLine _
                          & "                bufferedOutputBytes = New Byte(transformation.OutputBlockSize - 1) {}" & vbNewLine _
                          & "                Dim iIncrement As Integer = 0" & vbNewLine _
                          & "                While inputBytes.Length - iIncrement > 4" & vbNewLine _
                          & "                    transformation.TransformBlock(inputBytes, iIncrement, 4, bufferedOutputBytes, 0)" & vbNewLine _
                          & "                    iIncrement += 4" & vbNewLine _
                          & "                    writer.Write(bufferedOutputBytes, 0, transformation.OutputBlockSize)" & vbNewLine _
                          & "                End While" & vbNewLine _
                          & "                bufferedOutputBytes = transformation.TransformFinalBlock(inputBytes, iIncrement, inputBytes.Length - iIncrement)" & vbNewLine _
                          & "                writer.Write(bufferedOutputBytes, 0, bufferedOutputBytes.Length)" & vbNewLine _
                          & "                transformation.Clear()" & vbNewLine _
                          & "            End Using" & vbNewLine _
                          & "            writer.Position = 0" & vbNewLine _
                          & "            Dim lengthInteg As Integer" & vbNewLine _
                          & "            If writer.Length > Integer.MaxValue Then" & vbNewLine _
                          & "                lengthInteg = Integer.MaxValue" & vbNewLine _
                          & "            Else" & vbNewLine _
                          & "                lengthInteg = Convert.ToInt32(writer.Length)" & vbNewLine _
                          & "            End If" & vbNewLine _
                          & "            Dim bufferByt As Byte() = New Byte(lengthInteg - 1) {}" & vbNewLine _
                          & "            writer.Read(bufferByt, 0, lengthInteg)" & vbNewLine _
                          & "            writer.Close()" & vbNewLine _
                          & "            bytes = bufferByt" & vbNewLine _
                          & "        End Using" & vbNewLine _
                          & "        Return bytes" & vbNewLine _
                          & "    End Function" & vbNewLine _
                          & "    Public Shared Function " & GetStringFuncName & " (strByt As Byte(), defEnc As Boolean) As String" & vbNewLine _
                          & "        If defEnc Then" & vbNewLine _
                          & "            Return Encoding.Default.GetString(strByt)" & vbNewLine _
                          & "        End If" & vbNewLine _
                          & "        Return Encoding.UTF8.GetString(strByt)" & vbNewLine _
                          & "    End Function" & vbNewLine _
                          & "End Class" & vbNewLine
        End Function
#End Region

    End Class
End Namespace
