Imports System.Drawing
Imports System.Drawing.Imaging
Imports ImageQuantization
Imports XCrypt
Imports System.Configuration.ConfigurationManager

Partial Public Class GetLogoHotel
    Inherits PaginaBase

    Private Function ascii2hex(ByVal ascii As String) As String
        Dim hex As New StringBuilder
        Try
            For Each c As Char In ascii
                hex.Append(String.Format("{0:X2}", Asc(c)))
            Next
        Catch ex As Exception
        End Try
        Return hex.ToString
    End Function

    Private Shared Function hex2ascii(ByVal hex As String) As String
        Dim ascii As New StringBuilder
        Try
            For i As Integer = 0 To hex.Length - 1 Step 2
                ascii.Append(Chr(Integer.Parse(hex.Substring(i, 2).ToUpper, System.Globalization.NumberStyles.HexNumber)))
            Next
        Catch ex As Exception

        End Try

        Return ascii.ToString
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CompanyID As Integer = 0

        'En caso de que se tenga acceso desde algun html de correo
        'Verificamos el querystring y lo desencriptamos, para obtener el ID de la compañia a la que pertenece el logo        

        If Request.QueryString("qtd") IsNot Nothing Then
            Dim xe As XCryptEngine = New XCryptEngine()
            xe.InitializeEngine(XCryptEngine.AlgorithmType.TripleDES)
            Dim encryptedQuery As String = hex2ascii(Request.QueryString("qtd").ToString)
            xe.Key = "Crs-OnePage"
            'Ejemplo...  crypted:HOms5CGrPIY=  decrypted:138
            Dim decryptedQuery As String = xe.Decrypt(encryptedQuery)
            If IsNumeric(decryptedQuery) Then
                CompanyID = CType(decryptedQuery, Integer)
            End If
        End If

        Response.Clear()

        Try
            'Para el caso de que el id se le asigne desde querystring, ya no lo obtenemos            

            If CompanyID > 0 Then
                'Creamos una peticion web para traernos en un stream la imagen del logo
                Dim httpWebRequest As Net.WebRequest = Net.WebRequest.Create(String.Format("{0}/LogoCompany_{1}", AppSettings("VirtualDirectoryLogos"), CompanyID))
                Dim httpWebResponse As Net.WebResponse = httpWebRequest.GetResponse()
                Dim imgStream As System.IO.Stream = httpWebResponse.GetResponseStream

                Dim imgObjImage As System.Drawing.Image
                Try
                    imgObjImage = System.Drawing.Image.FromStream(imgStream, True)
                    'Redimiencionamos la imagen a escala
                    imgObjImage = ConvertImage(imgObjImage, 110, 65)

                    'Optimizamos la paleta de colores y mantenemos la transparencia del gif
                    Dim BMPQuantized As Bitmap
                    'Dim quantizer As New OctreeQuantizer(255, 8)
                    ' BMPQuantized = quantizer.Quantize(imgObjImage)
                    'BMPQuantized.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif)

                Catch ex As ArgumentException
                    'Cargamos la imagen default
                    imgObjImage = System.Drawing.Image.FromFile(String.Concat(Request.PhysicalApplicationPath, "\App_Configuration\DefaultImages\logo.gif"))
                    imgObjImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif)
                End Try

            End If

        Catch ex As Exception
            'Ocurrio un error 
        End Try

        Response.Expires = -1
        Response.ExpiresAbsolute = Now

        Response.ContentType = "image/GIF"
        Response.End()
    End Sub


    ''' <summary>
    ''' Redimenciona a escala el gif
    ''' </summary>
    ''' <param name="source">Imagen en memoria del gif</param>
    ''' <param name="MaxWidth">Limite en X donde se hara el thumbnail</param>
    ''' <param name="MaxHeight">Limite en Y donde se hara el thumbnail</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertImage(ByVal source As System.Drawing.Image, ByVal MaxWidth As Integer, ByVal MaxHeight As Integer) As System.Drawing.Image
        Dim newImg As System.Drawing.Image
        Dim nWidth As Integer = MaxWidth
        Dim nHeight As Integer = MaxHeight

        If (source.Width > MaxWidth) Then
            Dim ratio As Double = CType(CType(source.Height, Double) / CType(source.Width, Double), Double)
            Dim hFinal As Double = (nWidth) * ratio
            nHeight = CType(hFinal, Integer)
        ElseIf (source.Height > MaxHeight) Then
            Dim ratio As Double = CType(CType(source.Width, Double) / CType(source.Height, Double), Double)
            Dim wFinal As Double = (nHeight) * ratio
            nWidth = CType(wFinal, Integer)
        Else
            nWidth = source.Width
            nHeight = source.Height
        End If

        newImg = New System.Drawing.Bitmap(source, New System.Drawing.Size(nWidth, nHeight))

        Return newImg
    End Function

End Class