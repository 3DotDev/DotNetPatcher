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
                              & "    Public Shared Function " & _DecryptIntFuncName & " (ByVal d_String$, Byval d_integ%) As Integer" & vbNewLine _
                              & "        Dim s_String As String() = d_String.Split(New Char() {Strings.ChrW(d_integ)})" & vbNewLine _
                              & "        Dim n_String As Integer() = New Integer((s_String.Length - 1) - 1) {}" & vbNewLine _
                              & "        Dim i_Increment%" & vbNewLine _
                              & "        For i_Increment = 0 To n_String.Length - 1" & vbNewLine _
                              & "            n_String(i_Increment) = Integer.Parse(s_String(i_Increment))" & vbNewLine _
                              & "        Next i_Increment" & vbNewLine _
                              & "        Return n_String(n_String(n_String.Length - 1))" & vbNewLine _
                              & "    End Function" & vbNewLine _
                              & "End Class"
            Return str
        End Function

        Public Shared Function GenerateDecryptOddFunc(_ClassOddName$, _DecryptOddFuncName$) As String
            Dim str = "Public Class " & _ClassOddName & vbNewLine _
                              & "    Public Shared Function " & _DecryptOddFuncName & " (ByVal num_Integ%) As Boolean" & vbNewLine _
                              & "        Return num_Integ Mod 2 <> 0" & vbNewLine _
                              & "    End Function" & vbNewLine _
                              & "End Class"
            Return str
        End Function

        Public Shared Function GenerateDecryptXorFunc(_ClassXorName$, _DecryptXorFuncName$) As String
            Dim str = "Public Class " & _ClassXorName & vbNewLine _
                              & "    Public Shared Function " & _DecryptXorFuncName & " (ByVal t_String as String, ByVal num_Integ%) As String" & vbNewLine _
                              & "        Dim s_Result$ = String.Empty" & vbNewLine _
                              & "        Dim s_Length% = (t_String.Length - 1)" & vbNewLine _
                              & "        Dim j_increment% = 0" & vbNewLine _
                              & "        Do While (j_increment <= s_Length)" & vbNewLine _
                              & "            Dim p_Xoring% = (Convert.ToInt32(t_String.Chars(j_increment)) Xor num_Integ)" & vbNewLine _
                              & "            s_Result = (s_Result & Char.ConvertFromUtf32(p_Xoring))" & vbNewLine _
                              & "            j_increment += 1" & vbNewLine _
                              & "        Loop" & vbNewLine _
                              & "        Return s_Result" & vbNewLine _
                              & "    End Function" & vbNewLine _
                              & "End Class"

            Return str
        End Function

        Public Shared Function GenerateCompressWithGzipByteFunc(_Decompress0$, Decompress1$) As String
            Return "    Public Shared Function " & _Decompress0 & "(ByVal dat_Byte As Byte()) As Byte()" & vbNewLine _
                     & "        Try : Return " & Decompress1 & "(New GZipStream(New MemoryStream(dat_Byte), CompressionMode.Decompress, False), dat_Byte.Length)" & vbNewLine _
                     & "        Catch : Return Nothing : End Try" & vbNewLine _
                     & "    End Function" & vbNewLine _
                     & GenerateDeCompressWithGzipFunc(Decompress1)
        End Function

        Public Shared Function GenerateDeCompressWithGzipStreamFunc(_Decompress0$, Decompress1$) As String
            Return "    Private Shared Function " & _Decompress0 & "(ByVal dat_Stream As Stream) As Byte()" & vbNewLine _
                     & "        Try : Return " & Decompress1 & "(New GZipStream(dat_Stream, CompressionMode.Decompress, False), dat_Stream.Length)" & vbNewLine _
                     & "        Catch : Return Nothing : End Try" & vbNewLine _
                     & "    End Function" & vbNewLine _
                     & GenerateDeCompressWithGzipFunc(Decompress1)
        End Function

        Private Shared Function GenerateDeCompressWithGzipFunc(Decompress1$) As String
            Return "    Public Shared Function " & Decompress1 & "(ByVal dat_Stream As Stream, ByVal Key_Integ As Integer) As Byte()" & vbNewLine _
                     & "        Dim dat_Byte() As Byte : Dim t_byteInt As Int32 = 0" & vbNewLine _
                     & "        Try : While True" & vbNewLine _
                     & "            ReDim Preserve dat_Byte(t_byteInt + Key_Integ)" & vbNewLine _
                     & "            Dim br_integer As Int32 = dat_Stream.Read(dat_Byte, t_byteInt, Key_Integ)" & vbNewLine _
                     & "            If br_integer = 0 Then Exit While" & vbNewLine _
                     & "                t_byteInt += br_integer" & vbNewLine _
                     & "              End While" & vbNewLine _
                     & "            ReDim Preserve dat_Byte(t_byteInt - 1)" & vbNewLine _
                     & "            Return dat_Byte" & vbNewLine _
                     & "        Catch : Return Nothing : End Try" & vbNewLine _
                     & "    End Function" & vbNewLine
        End Function

        Public Shared Function GenereateDecryptPrimeFunc(_FunctionName$) As String
            Return "Public Shared Function " & _FunctionName & " (Byval number_Integ as Integer) As Boolean" & vbNewLine _
                 & "        Dim bool_Val As Boolean = True" & vbNewLine _
                 & "        Dim half_Num as integer = number_Integ / 2" & vbNewLine _
                 & "        Dim i_Increment as integer = 0" & vbNewLine _
                 & "        For i_Increment= 2 To half_Num" & vbNewLine _
                 & "            If (number_Integ Mod i_Increment) = 0 Then" & vbNewLine _
                 & "                bool_Val = False" & vbNewLine _
                 & "            End If" & vbNewLine _
                 & "        Next" & vbNewLine _
                 & "        Return bool_Val" & vbNewLine _
                 & "     End Function"
        End Function

        Public Shared Function GenerateDecryptRPNFunc(_FunctionName0$, _FunctionName1$) As String
            Return "    Public Shared Function " & _FunctionName0 & " (ByVal ope_rands As String()) As Integer" & vbNewLine _
                 & "        Dim stack_I As New Stack(Of Integer)" & vbNewLine _
                 & "        For Each Stack_opCod As String In ope_rands" & vbNewLine _
                 & "            Select Case Stack_opCod" & vbNewLine _
                 & "                Case ""+""" & vbNewLine _
                 & "                    stack_I.Push(stack_I.Pop() + stack_I.Pop())" & vbNewLine _
                 & "                Case ""-""" & vbNewLine _
                 & "                    stack_I.Push(-stack_I.Pop() + stack_I.Pop())" & vbNewLine _
                 & "                Case ""*""" & vbNewLine _
                 & "                    stack_I.Push(stack_I.Pop() * stack_I.Pop())" & vbNewLine _
                 & "                Case ""/""" & vbNewLine _
                 & "                    Dim tmp_Integ As Integer = stack_I.Pop()" & vbNewLine _
                 & "                    stack_I.Push(stack_I.Pop() / tmp_Integ)" & vbNewLine _
                 & "                Case ""sqrt""" & vbNewLine _
                 & "                    stack_I.Push(Math.Sqrt(stack_I.Pop()))" & vbNewLine _
                 & "                Case Else" & vbNewLine _
                 & "                    stack_I.Push(Integer.Parse(Stack_opCod))" & vbNewLine _
                 & "            End Select" & vbNewLine _
                 & "        Next" & vbNewLine _
                 & "        Return stack_I.Pop()" & vbNewLine _
                 & "    End Function" & vbNewLine _
                 & "    Public Shared Function " & _FunctionName1 & " (ByVal expression_Str As String) As String()" & vbNewLine _
                 & "        Return expression_Str.ToLower().Split(New Char() {"",""c}, StringSplitOptions.RemoveEmptyEntries)" & vbNewLine _
                 & "    End Function" & vbNewLine
        End Function

        Public Shared Function GenerateReadFromResourcesFunc(ClassName$, ReadFromResourcesFuncName$, ResName$) As String
            Return "Public Class " & ClassName & vbNewLine _
                          & "    Public Shared Function " & ReadFromResourcesFuncName & " (ByVal Value_Str As String) As String" & vbNewLine _
                          & "        Dim Resource_Man As New ResourceManager(""" & ResName & """, GetType(System.Reflection.Assembly).GetMethod(""GetExecutingAssembly"").Invoke(Nothing, Nothing))" & vbNewLine _
                          & "        Dim str_Object As String = DirectCast(Resource_Man.GetObject(Value_Str), String)" & vbNewLine _
                          & "        Resource_Man.ReleaseAllResources()" & vbNewLine _
                          & "        Return str_Object" & vbNewLine _
                          & "    End Function" & vbNewLine _
                          & "End Class" & vbNewLine
        End Function

        Public Shared Function GenerateFromBase64Func(ClassName$, FromBase64FuncName$, GetStringFuncName$) As String
            Return "Public Class " & ClassName & vbNewLine _
                          & "    Public Shared Function " & FromBase64FuncName & " (string_Str As String, def_Enc As Boolean) As Byte()" & vbNewLine _
                          & "        Dim b_bytes As Byte()" & vbNewLine _
                          & "        Using m_writer As MemoryStream = New MemoryStream()" & vbNewLine _
                          & "            Dim buffered_OutputBytes As Byte()" & vbNewLine _
                          & "            Dim input_Bytes As Byte()" & vbNewLine _
                          & "            If def_Enc Then" & vbNewLine _
                          & "                input_Bytes = Encoding.Default.GetBytes(string_Str)" & vbNewLine _
                          & "            Else" & vbNewLine _
                          & "                input_Bytes = Encoding.UTF8.GetBytes(string_Str)" & vbNewLine _
                          & "            End If" & vbNewLine _
                          & "            Using transfor_mation As FromBase64Transform = New FromBase64Transform()" & vbNewLine _
                          & "                buffered_OutputBytes = New Byte(transfor_mation.OutputBlockSize - 1) {}" & vbNewLine _
                          & "                Dim i_Increment As Integer = 0" & vbNewLine _
                          & "                While input_Bytes.Length - i_Increment > 4" & vbNewLine _
                          & "                    transfor_mation.TransformBlock(input_Bytes, i_Increment, 4, buffered_OutputBytes, 0)" & vbNewLine _
                          & "                    i_Increment += 4" & vbNewLine _
                          & "                    m_writer.Write(buffered_OutputBytes, 0, transfor_mation.OutputBlockSize)" & vbNewLine _
                          & "                End While" & vbNewLine _
                          & "                buffered_OutputBytes = transfor_mation.TransformFinalBlock(input_Bytes, i_Increment, input_Bytes.Length - i_Increment)" & vbNewLine _
                          & "                m_writer.Write(buffered_OutputBytes, 0, buffered_OutputBytes.Length)" & vbNewLine _
                          & "                transfor_mation.Clear()" & vbNewLine _
                          & "            End Using" & vbNewLine _
                          & "            m_writer.Position = 0" & vbNewLine _
                          & "            Dim length_Integ As Integer" & vbNewLine _
                          & "            If m_writer.Length > Integer.MaxValue Then" & vbNewLine _
                          & "                length_Integ = Integer.MaxValue" & vbNewLine _
                          & "            Else" & vbNewLine _
                          & "                length_Integ = Convert.ToInt32(m_writer.Length)" & vbNewLine _
                          & "            End If" & vbNewLine _
                          & "            Dim buffer_Byt As Byte() = New Byte(length_Integ - 1) {}" & vbNewLine _
                          & "            m_writer.Read(buffer_Byt, 0, length_Integ)" & vbNewLine _
                          & "            m_writer.Close()" & vbNewLine _
                          & "            b_bytes = buffer_Byt" & vbNewLine _
                          & "        End Using" & vbNewLine _
                          & "        Return b_bytes" & vbNewLine _
                          & "    End Function" & vbNewLine _
                          & "    Public Shared Function " & GetStringFuncName & " (str_Byt As Byte(), def_Enc As Boolean) As String" & vbNewLine _
                          & "        If def_Enc Then" & vbNewLine _
                          & "            Return Encoding.Default.GetString(str_Byt)" & vbNewLine _
                          & "        End If" & vbNewLine _
                          & "        Return Encoding.UTF8.GetString(str_Byt)" & vbNewLine _
                          & "    End Function" & vbNewLine _
                          & "End Class" & vbNewLine
        End Function
#End Region

    End Class
End Namespace
