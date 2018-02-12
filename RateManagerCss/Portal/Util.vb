Imports System.IO
Imports System.Text
Imports System.Xml
Imports XCrypt
Imports System.Configuration.ConfigurationManager
Imports Mailer35

Namespace Util
    Public Class Images
        Public Shared Sub loadImage(ByVal strPath As String, ByRef objImg As System.Web.UI.WebControls.Image, ByVal ModeView As Opciones.ViewMode)
            objImg.ImageUrl = strPath
        End Sub
    End Class

    Public Class Emails
        Public Function MandarCorreoLogHotel(ByVal User As String, ByVal hotelName As String, _
                                             ByVal Peticion As String, ByVal msg As String, _
                                             ByVal sDato As String, ByVal sdatoDespues As String, _
                                             ByVal drHotel As DataRow) As Boolean
            Dim Mail As emailTemplates.Template = New emailTemplates.Template
            Dim fecha As Date
            Dim hr As Boolean

            hr = IIf(drHotel.IsNull("TransByEmail"), False, drHotel.Item("TransByEmail"))
            If Not hr Then Return False

            Try
                fecha = Now.ToString("MM/dd/yyyy") & " " & Now.ToLongTimeString
            Catch
                fecha = Now.ToString & " " & Now.ToLongTimeString
            End Try

            Dim idioma As String
            idioma = IIf(drHotel.IsNull("idiomaemail"), "en-US", drHotel.Item("idiomaemail"))

            Mail.Idioma = idioma
            Mail.SubjectParam = Peticion
            Mail.TemplateName = "T15_LogToHotel"
            Dim email As String = drHotel("EmailReservas").ToString
            Mail.To = email
            'Mail.Bcc = System.Configuration.ConfigurationSettings.AppSettings("UnivisitMail")

            Mail.AddParameter("HEADER") = ""
            Mail.AddParameter("FOOTER") = ""
            Mail.Html = True
            Mail.AddParameter("HOTEL_NOMBRE") = hotelName
            Mail.AddParameter("USER") = User
            Mail.AddParameter("DATE_TRANS") = fecha
            Mail.AddParameter("PETICION") = msg
            Mail.AddParameter("DATA") = sDato
            Mail.Send()

            'solo prueba Utility.MailerSend("Rates", Mail.GetBody)

            hr = True
            Return hr
        End Function
    End Class

    Class Utility
        Private Shared Function ascii2hex(ByVal ascii As String) As String
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

        Public Shared Function GetXml(ByVal name As String, ByVal table As String, ByVal ds As DataSet, Optional ByVal nameDataset As String = "") As String
            Dim sName As String = ds.Tables(name).TableName
            Dim sxml As String

            ds.DataSetName = name
            If Not String.IsNullOrEmpty(nameDataset) Then
                ds.DataSetName = nameDataset
            End If
            ds.Tables(name).TableName = table
            sxml = ds.GetXml.ToString
            ds.Tables(table).TableName = sName
            Return sxml
        End Function

        Public Shared Function getHttpUrlPath() As String
            Dim Port As String = HttpContext.Current.Request.ServerVariables("SERVER_PORT")
            If Port Is Nothing OrElse Port = "80" OrElse Port = "443" Then
                Port = ":"
            Else
                Port = String.Concat(":", Port)
            End If
            Dim Protocol As String = HttpContext.Current.Request.ServerVariables("SERVER_PORT_SECURE")
            If Protocol Is Nothing OrElse Protocol = "0" Then
                Protocol = "http://"
            Else
                Protocol = "https://"
            End If

            Dim urlPath As String = String.Concat(Protocol, HttpContext.Current.Request.ServerVariables("SERVER_NAME"), Port, IIf(HttpContext.Current.Request.ApplicationPath = "/", "", HttpContext.Current.Request.ApplicationPath).ToString)
            Return urlPath
        End Function

        Public Shared Function LoadImagen(ByVal idHotel As Integer)
            Dim xe As XCryptEngine = New XCryptEngine()
            xe.InitializeEngine(XCryptEngine.AlgorithmType.TripleDES)
            Dim decryptedQuery As String = idHotel  'idEmpresa, ejemplo la concha es idempresa=138
            'Ejemplo...  decrypted:HOms5CGrPIY=  valor:138, ya con la conversion a hex queda como 484F6D73354347725049593D el query
            Dim encryptedQuery As String = xe.Encrypt(decryptedQuery, "Crs-OnePage")
            Dim urlPath As String = Util.Utility.getHttpUrlPath
            Dim pathLogo As String = String.Concat(AppSettings("urlEmailLogo"), "?qtd=", ascii2hex(encryptedQuery))
            Return pathLogo
        End Function

        Public Shared Function MailerSend(ByVal Subject As String, ByVal Body As String) As Boolean
            Dim CMail As New Mailer35.NetMail.Mailer
            Dim sError As String

            sError = ""
            CMail._From_ = "victor@oz.com.mx"
            CMail._To_ = "victor@oz.com.mx"
            CMail._msg_subject = Subject
            CMail._msg_body = Body
            CMail.Credentials("smtp.gmail.com", "viktormgm@gmail.com", "viktormac")
            CMail.Send(sError)

        End Function




        Public Shared Function SetTableName(ByRef dst As DataSet) As Boolean
            For i As Integer = 0 To dst.Tables.Count - 1
                dst.Tables(i).TableName = String.Format("_{0}", dst.Tables(i).TableName)
            Next
        End Function

        Public Shared Function GeneraCorreoXslt(ByVal sDatos As String, ByVal sDatosDespues As String) As String
            Dim trans As New Xsl.XslCompiledTransform
            Dim oXmlTextReader As XmlTextReader
            Dim writer As New StringWriter
            Dim xdd As XmlDataDocument
            Dim rx As XmlTextReader
            Dim ds As DataSet
            Dim dst As DataSet
            Dim fileXsl As String
            Dim sname As String
            Dim shtml As String
            Dim str2 As String
            Dim str As String
            Dim rowArray(1) As Object
            Dim sCorreo As String

            Try

                ds = New DataSet
                dst = New DataSet
                str = "<Inventario> </Inventario>"
                str2 = "<Inventario> </Inventario>"
                If Not String.IsNullOrEmpty(sDatos) Then
                    str = sDatos
                End If
                If Not String.IsNullOrEmpty(sDatosDespues) Then
                    str2 = sDatosDespues
                End If

                rx = New XmlTextReader(str, XmlNodeType.Document, Nothing)
                ds.ReadXml(rx)

                rx = New XmlTextReader(str2, XmlNodeType.Document, Nothing)
                dst.ReadXml(rx)
                sname = dst.DataSetName
                SetTableName(dst)
                'dst.Tables(0).TableName = String.Format("_{0}", dst.Tables(0).TableName)
                ds.Merge(dst)
                If String.IsNullOrEmpty(sDatos) Then
                    ds.DataSetName = dst.DataSetName
                End If

                fileXsl = String.Concat(ConfigurationManager.AppSettings("pathRM"), String.Format("xslt\{0}\{1}.xslt", PortalCulture.GetCulture.ToString.Substring(0, 2), sname))
                'fileXsl = String.Concat("C:\ProyectosNet35\RateManager\RateManager\RateManager\Correos\xslt\es\", String.Format("{0}.xslt", sname))
                If System.IO.File.Exists(fileXsl) Then
                    oXmlTextReader = New XmlTextReader(ds.GetXml(), XmlNodeType.Document, Nothing)
                    xdd = New XmlDataDocument(ds)
                    trans.Load(fileXsl)
                    trans.Transform(xdd, Nothing, writer)
                    shtml = writer.ToString()
                    sCorreo = shtml
                Else

                End If
            Catch ex As Exception
            End Try
            Return sCorreo
        End Function
    End Class

End Namespace
