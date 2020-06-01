Imports System.IO
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.IO.Compression
Imports System.Text

Namespace UtilsHelper
    Public NotInheritable Class Functions

#Region " Fields "
        Private Shared ReadOnly TempFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp"
#End Region

#Region " Methods "
        Public Shared Sub DeleteFiles(Path$)
            Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories).ToList.ForEach(AddressOf DeleteFile)
        End Sub

        Private Shared Sub DeleteFile(f$)
            Try
                File.Delete(f)
            Catch ex As Exception
            End Try
        End Sub

        Public Shared Function GetTempFolder() As String
            If Not Directory.Exists(tempFolder) Then
                Directory.CreateDirectory(tempFolder)
            End If
            Return tempFolder
        End Function

        Public Shared Function StrToHex(ByRef Data As String) As String
            Dim sVal As String
            Dim sHex As String = ""
            While Data.Length > 0
                sVal = Hex(Asc(Data.Substring(0, 1).ToString()))
                Data = Data.Substring(1, Data.Length - 1)
                sHex &= sVal
            End While
            Return sHex
        End Function

        Public Shared Function AssemblyToHex(FilePath$) As String
            Dim val As String
            Using fr As New FileStream(FilePath, FileMode.Open, FileAccess.Read)
                Using br As New BinaryReader(fr)
                    Return BitConverter.ToString(br.ReadBytes(CInt(fr.Length))).Replace("-", "")
                End Using
            End Using
            Return val
        End Function

        Public Shared Function ByteArrayToHex(ba As Byte()) As String
            Dim hex As String = BitConverter.ToString(ba)
            Return hex.Replace("-", "")
        End Function

        Public Shared Function StreamToHex(ba As Stream) As String
            Dim hex As String = BitConverter.ToString(StreamToBytArray(ba))
            Return hex.Replace("-", "")
        End Function

        Public Shared Function StreamToBytArray(input As Stream) As Byte()
            Using ms As New MemoryStream()
                input.CopyTo(ms)
                Return ms.ToArray()
            End Using
        End Function

        Public Shared Function ConvertToByteArray(inputElements As Integer()) As Byte()
            Dim myFinalBytes As Byte() = New Byte(inputElements.Length * 4 - 1) {}
            For cnt As Integer = 0 To inputElements.Length - 1
                Dim myBytes As Byte() = BitConverter.GetBytes(inputElements(cnt))
                Array.Copy(myBytes, 0, myFinalBytes, cnt * 4, 4)
            Next
            Return myFinalBytes
        End Function

        Public Shared Function GZipedByte(raw As Byte()) As Byte()
            Using memory As New MemoryStream()
                Using gzip As New GZipStream(memory, CompressionMode.Compress, True)
                    gzip.Write(raw, 0, raw.Length)
                End Using
                Return memory.ToArray()
            End Using
        End Function

        Public Shared Function IsBase64StringEncoded(str$) As Boolean
            Dim r As New RegularExpressions.Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$")
            Return r.IsMatch(str)
        End Function

        Public Shared Function IsValid(file As Byte()) As Boolean
            If file.Take(2).SequenceEqual(New Byte() {77, 90}) Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValid(filepath As String, Mime As ImageFormat) As Boolean
            Try
                Dim id As Guid = GetGuidID(filepath)
                If id = Mime.Guid Then
                    Return True
                End If
            Catch
                Return False
            End Try
            Return False
        End Function

        Private Shared Function GetGuidID(filepath As String) As Guid
            Dim imagedata As Byte() = File.ReadAllBytes(filepath)
            Dim id As Guid
            Using ms As New MemoryStream(imagedata)
                Using img As Image = Image.FromStream(ms)
                    id = img.RawFormat.Guid
                End Using
            End Using
            Return id
        End Function

        ''' <summary>
        ''' Redimensionne une image selon une taille prédéfinie passée en paramétre.
        ''' </summary>
        ''' <param name="Img">Image au format Bitmap</param>
        ''' <param name="siz">Dimensons pour la future 'image</param>
        ''' <returns>Valeur de type Image</returns>
        Public Shared Function GetAutoSize(Img As Bitmap, siz As Size) As Image
            Dim imgOrg As Bitmap
            Dim imgShow As Bitmap
            Dim g As Graphics
            Dim divideBy, divideByH, divideByW As Double
            imgOrg = Img
            divideByW = imgOrg.Width / siz.Width
            divideByH = imgOrg.Height / siz.Height
            If divideByW > 1 Or divideByH > 1 Then
                If divideByW > divideByH Then
                    divideBy = divideByW
                Else
                    divideBy = divideByH
                End If
                imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
                g = Graphics.FromImage(imgShow)
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
                g.Dispose()
            Else
                imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
                g = Graphics.FromImage(imgShow)
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
                g.Dispose()
            End If
            imgOrg.Dispose()
            Return imgShow
        End Function


#End Region

    End Class
End Namespace
