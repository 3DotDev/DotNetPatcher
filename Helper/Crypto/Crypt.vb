Imports System.Security.Cryptography
Imports System.Text

Namespace CryptoHelper
    Public Class Crypt

#Region " Properties "
        Public ReadOnly Property SaltSize() As Integer
        Public ReadOnly Property Key() As Byte()
#End Region

#Region " Constructor "
        Public Sub New()
            _SaltSize = New Random().Next(1, 8)
            _Key = Encoding.ASCII.GetBytes(Guid.NewGuid.ToString.Replace("-", ""))
        End Sub
#End Region

#Region " Methods "
        Public Function Encrypt(input As Byte()) As Byte()
            Dim RND As New RNGCryptoServiceProvider()
            Dim Output As Byte() = New Byte(input.Length - 1) {}
            Dim Salt As Byte() = New Byte(SaltSize - 1) {}
            RND.GetBytes(Salt)
            For i As Integer = 0 To input.Length - 1
                Output(i) = CByte(input(i) Xor Key(i Mod Key.Length) Xor Salt(i Mod Salt.Length))
            Next
            Array.Resize(Output, Output.Length + SaltSize)
            Buffer.BlockCopy(Salt, 0, Output, Output.Length - SaltSize, SaltSize)
            Return Output
        End Function
#End Region

        'Public Function AESEncrypt(ByVal input As Byte()) As Byte()
        '    Dim AES As New RijndaelManaged()
        '    Dim hash As Byte() = New Byte(&H20 - 1) {}
        '    Dim temp As Byte() = New MD5CryptoServiceProvider().ComputeHash(key)
        '    Array.Copy(temp, 0, hash, 0, 16)
        '    Array.Copy(temp, 0, hash, 15, 16)
        '    AES.Key = hash
        '    AES.Mode = CipherMode.ECB
        '    Dim DESEncrypter As ICryptoTransform = AES.CreateEncryptor()
        '    Return DESEncrypter.TransformFinalBlock(input, 0, input.Length)
        'End Function

        'Public Function AESDecrypt(ByVal input As Byte()) As Byte()
        '    Dim managed As New RijndaelManaged
        '    Dim destinationArray As Byte() = New Byte(&H20 - 1) {}
        '    Dim sourceArray As Byte() = New MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Pass))
        '    Array.Copy(sourceArray, 0, destinationArray, 0, &H10)
        '    Array.Copy(sourceArray, 0, destinationArray, 15, &H10)
        '    managed.Key = destinationArray
        '    managed.Mode = CipherMode.ECB
        '    Return managed.CreateDecryptor.TransformFinalBlock(input, 0, input.Length)
        'End Function

        'Public Function Decrypt(ByVal input As Byte()) As Byte()
        '    Dim Output As Byte() = New Byte(input.Length - SaltSize - 1) {}
        '    Dim Salt As Byte() = New Byte(SaltSize - 1) {}
        '    Buffer.BlockCopy(input, input.Length - SaltSize, Salt, 0, SaltSize)
        '    Array.Resize(Of Byte)(input, input.Length - SaltSize)
        '    For i As Integer = 0 To input.Length - 1
        '        Output(i) = CByte(input(i) Xor key(i Mod key.Length) Xor Salt(i Mod Salt.Length))
        '    Next
        '    Return Output
        'End Function

    End Class
End Namespace
