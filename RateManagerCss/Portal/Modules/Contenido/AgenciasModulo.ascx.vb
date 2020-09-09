Imports System.Security.Cryptography
Imports Common.Data
Imports Facade
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.General.Common.Data
Imports System.Data.SqlClient
Imports System.IO


Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos.Facade
Imports System.Configuration.ConfigurationManager

Partial Public Class AgenciasModulo
    Inherits System.Web.UI.UserControl

    Public Property Editing() As Boolean
        Get
            If IsNothing(ViewState("Editing")) Then ViewState("Editing") = False
            Return ViewState("Editing")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editing") = Value
        End Set
    End Property

    Public Property ididioma() As Integer
        Get
            If ViewState("idioma") Is Nothing Then
                ViewState("idioma") = AppSettings("DefaultLanguageId")
            End If
            Return ViewState("idioma")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idioma") = Value
        End Set
    End Property

    Public Sub Clear()
        txtNombre.Text = ""
        txtIATA.Text = ""
        txtDomicilio.Text = ""
        txtCP.Text = ""
        txtCiudad.Text = ""
        txtArea.Text = ""
        txtTel.Text = ""
        txtFax.Text = ""
        txtPaginaWeb.Text = ""
        txtComentarios.Text = ""
        txtContactoNombre.Text = ""
        txtContactoPuesto.Text = ""
        txtContactoTel.Text = ""
        txtContactoCorreo.Text = ""
        lblError.Visible = False
        cmbPaises.SelectedIndex = cmbPaises.Items.IndexOf(cmbPaises.Items.FindByValue("MX"))
    End Sub

    Private Sub LoadResource()
        lblInformacion.Text = PortalCulture.GetString("00160", False) 'Información de empresa
        If Editing Then
            lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("00065"), PortalCulture.GetString("00823"))
        Else
            lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("00823"))
        End If
        lblNombre.Text = PortalCulture.GetString("00741", True) 'Nombre
        lblDomicilio.Text = PortalCulture.GetString("M000076", True) 'Dirección
        lblPais.Text = PortalCulture.GetString("M000251", True) 'Pais
        lblCiudad.Text = PortalCulture.GetString("M000254", True) 'Ciudad
        Label2.Text = PortalCulture.GetString("00836", True) 'Area
        lblCP.Text = PortalCulture.GetString("M0BT0000164", True) 'Cod. Postal
        lblTel.Text = PortalCulture.GetString("00725", True) 'Teléfono
        lblFax.Text = PortalCulture.GetString("00837", True) 'Fax
        Label8.Text = PortalCulture.GetString("00835", True) 'Pagina web

        lblContactoPuesto.Text = String.Format(PortalCulture.GetString("00826", True), "")
        lblTel1.Text = String.Format(PortalCulture.GetString("00825", True), "")
        lblInformacionContacto.Text = PortalCulture.GetString("00828", False) 'Informacion del contacto
        lblContactoNombre.Text = String.Format(PortalCulture.GetString("00827", True), "")
        lblContactoCorreo.Text = String.Format(PortalCulture.GetString("00824", True), "")

        lblComentarios.Text = PortalCulture.GetString("01238")


    End Sub

    Public Sub showError(ByVal sw As Boolean, Optional ByVal status As Integer = 0)
        Me.lblError.Visible = sw
        If status = 230 Then
            If sw Then lblError.Text = PortalCulture.GetString("01587")
        Else
            If sw Then lblError.Text = PortalCulture.GetString("00844")
        End If

    End Sub

    Public Sub showExito(ByVal smsg As String)
        Me.lblError.Visible = True
        Me.lblError.Text = smsg
    End Sub


    Private Function Load_RelatedContentAdmin(ByVal idEmpresa As Integer) As Integer
        Dim res As Integer = 0
        Dim cnn As New SqlConnection(AppSettings("PortalConnection"))
        Dim strSql As String = ""
        strSql = "select * from PeticionRegistro where idsolicitante = {0}"
        strSql = String.Format(strSql, idEmpresa)
        Dim cmd As New SqlCommand(strSql, cnn)
        Dim SqlReader As SqlDataReader
        cnn.Open()
        SqlReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        If SqlReader.HasRows Then
            SqlReader.Read()
            res = SqlReader.Item("AsignadoA")
        End If
        Return res
    End Function
    
    Private Function loadHotelData(ByVal idhotel As Integer) As Boolean
        Dim dsHotel As New HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(idhotel)
        End With
        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                'If Not .IsNull(HotelDatos.FIELD_CATEGORIA) Then
                '    cmbCategoria.SelectedValue = .Item(HotelDatos.FIELD_CATEGORIA)
                'End If
                'If Not .IsNull(HotelDatos.FIELD_IDCIUDAD) Then
                '    cmbCiudades.SelectedValue = .Item(HotelDatos.FIELD_IDCIUDAD)
                'End If
                Try
                    If Not .IsNull(HotelDatos.fld_idcorporativo) Then
                        ' ddlCorporativos.SelectedValue = .Item(HotelDatos.fld_idcorporativo)
                    End If
                Catch ex As Exception

                End Try

                'If Not .IsNull(HotelDatos.FIELD_IDMONEDA) Then
                '    cmbMonedas.SelectedValue = .Item(HotelDatos.FIELD_IDMONEDA)
                'End If
            End With
        End If
    End Function

    Function GuardaLogEmpresa(ByVal dsSource As EmpresaDatos, _
                              ByVal companyData As EmpresaDatos, _
                              ByVal isModify As Boolean) As Boolean
        Dim sdatosMod As String
        Dim sEmpresa As String
        Dim sdatos As String
        Dim eAction As PaginaBase.acciones
        Dim hr As Boolean

        hr = True
        Try
            sdatosMod = ""
            sdatos = Util.Utility.GetXml(EmpresaDatos.COMPANY_TABLE, "UpdateRegisterAgency", dsSource)
            If isModify Then _
                sdatosMod = Util.Utility.GetXml(EmpresaDatos.COMPANY_TABLE, "UpdateRegisterAgency", companyData)
            eAction = IIf(isModify, PaginaBase.acciones.Modificar, PaginaBase.acciones.Crear)
            sEmpresa = String.Format("Se {0} registro de la agencia: {1}", IIf(isModify, "modificó", "creó"), txtNombre.Text)

            CType(Me.Page, PaginaBase).guardalog("/registro/AgencyRegister.aspx", _
                                                 eAction, sEmpresa, "", sdatos, sdatosMod)
        Catch ex As Exception
            hr = False
        End Try
        Return hr
    End Function

    Public Shared Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function


    Public Function EnviaCorreo(ByVal AdminId As Integer, ByVal sEmpresa As String, ByVal sContactoNombre As String, ByVal sContactoTel As String, ByVal sContactoCorreo As String) As Boolean
        Dim Mail As emailTemplates.Template = New emailTemplates.Template
        Dim userSystem As New Portal.General.Facade.cUserSystem
        Dim user As UserData
        Dim Prov As New PortalPartnersCfg
        Dim strAdminMail As String
        Dim idioma As String

        user = userSystem.GetUserById(AdminId)
        If Not user Is Nothing AndAlso user.Tables.Count > 0 AndAlso user.Tables(0).Rows.Count > 0 Then
            strAdminMail = user.Tables(UserData.USER_TABLE).Rows(0)(UserData.EMAIL_FIELD)
        Else
            strAdminMail = AppSettings("UnivisitMail")
        End If


        If PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper = "ES" Then
            idioma = "es-MX"
        Else
            idioma = "en-US"
        End If

        Mail.Idioma = idioma
        Mail.SubjectParam = "Registro Empresa"
        Mail.TemplateName = "TH_REGISTRO_EMPRESA"
        Mail.Html = True

        'Mail.To = "victor@oz.com.mx"
        Mail.To = strAdminMail

        Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css ' type='text/css' rel='stylesheet'>"
        Mail.AddParameter("HEADER") = ""
        Mail.AddParameter("HOTELNAME") = sEmpresa
        Mail.AddParameter("CONTACTO") = sContactoNombre
        Mail.AddParameter("CONTACTOTEL") = sContactoTel
        Mail.AddParameter("CONTACTOEMAIL") = sContactoCorreo
        Mail.Send()
        'Util.Utility.MailerSend("registro empresa", Mail.GetBody)
    End Function


    Public Function Add(ByRef idempresa As Integer, ByRef status As Integer) As Boolean
        Dim companyData As EmpresaDatos = Nothing

        If txtPaginaWeb.Text.Trim <> "" Then
            If Not txtPaginaWeb.Text.StartsWith("http://") Then txtPaginaWeb.Text = "http://" + txtPaginaWeb.Text
        End If

        With New EmpresaSistema
            Dim estado As String = " "
            Dim estadof As String = " "
            Dim municipio As String = " "
            Dim municipiof As String = " "
            Dim IdAdminist As Integer = 0
            Dim idSegmento As Integer
            Dim strError As String = ""
            Dim idAgencia As Integer
            Dim DatosAgencia As AgencyData

            Integer.TryParse(AppSettings("IdSegmento"), idSegmento)

            Integer.TryParse(AppSettings("AdminCompanyRegistration"), IdAdminist)
            If .CreateCompanyAgency(IdAdminist, Me.ididioma, txtCiudad.Text, txtComentarios.Text, _
                    txtContactoCorreo.Text, txtContactoNombre.Text, txtCP.Text, txtDomicilio.Text, _
                    estado, municipio, txtFax.Text, Date.Now, Guid.NewGuid.ToString, _
                    0, cmbPaises.SelectedValue, txtNombre.Text, 25, _
                    0, txtTel.Text, "", "", _
                    estadof, municipiof, cmbPaises.SelectedValue, "", companyData, "", 0, 0, 0, txtContactoPuesto.Text, 0, txtArea.Text, txtContactoTel.Text, _
                    "", "", "", "", "", "", "", "", "", "", "", False, "", txtPaginaWeb.Text.Trim, False, False, idSegmento) Then
                idempresa = companyData.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_idEmpresa)
                '// Guarda en bitacora, se creo una nueva empresa.
                GuardaLogEmpresa(companyData, companyData, False)

                Dim addAgencyBand As Boolean = addAgency(idempresa, txtContactoCorreo.Text, status)


                DatosAgencia = (New AgenciaSistema).GetAgencyByIDCompany(idempresa)

                If DatosAgencia.Tables.Count > 0 AndAlso DatosAgencia.Tables(0).Rows.Count > 0 Then
                    idAgencia = DatosAgencia.Tables(0).Rows(0).Item(AgencyData.C_IDAGENCY)
                End If
                Dim dsUsuarios As UserData
                With (New Portal.General.Facade.cUserSystem)
                    dsUsuarios = .GetUserByEmail(txtContactoCorreo.Text.Trim)
                End With

                If Not (New Agentes).insertAgent(idAgencia, dsUsuarios.Tables(0).Rows(0).Item(UserData.IDUSER_FIELD), txtContactoNombre.Text.Trim, "", txtContactoCorreo.Text.Trim, strError) Then
                    lblError.Text = strError
                    lblError.Visible = True
                End If

                'Fillcorreo(dsUsuarios)
                Return addAgencyBand

            End If
            Return False
        End With
    End Function

    Public Shared Function DecryptString128Bit(ByVal vstrStringToBeDecrypted As String, ByVal vstrDecryptionKey As String) As String
        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim objRijndaelManaged As New RijndaelManaged
        Dim objMemoryStream As MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim bytDecryptionKey() As Byte
        Dim intLength As Integer
        Dim intRemaining As Integer

        Dim strReturnString As String = String.Empty

        bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted)
        intLength = Len(vstrDecryptionKey)
        If intLength >= 32 Then
            vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32)
        Else
            intLength = Len(vstrDecryptionKey)
            intRemaining = 32 - intLength
            vstrDecryptionKey = vstrDecryptionKey & Strings.StrDup(intRemaining, "X")
        End If
        bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray)
        ReDim bytTemp(bytDataToBeDecrypted.Length)
        objMemoryStream = New MemoryStream(bytDataToBeDecrypted)
        Try
            objCryptoStream = New CryptoStream(objMemoryStream, _
               objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), _
               CryptoStreamMode.Read)
            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)
            objCryptoStream.FlushFinalBlock()
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch
        End Try
        Return StripNullCharacters(Encoding.ASCII.GetString(bytTemp))
    End Function

    Public Shared Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
        Dim intPosition As Integer
        Dim strStringWithOutNulls As String
        intPosition = 1
        strStringWithOutNulls = vstrStringWithNulls
        Do While intPosition > 0
            intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)
            If intPosition > 0 Then
                strStringWithOutNulls = Microsoft.VisualBasic.Left(strStringWithOutNulls, intPosition - 1) & _
                Microsoft.VisualBasic.Right(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
            End If
            If intPosition > strStringWithOutNulls.Length Then
                Exit Do
            End If
        Loop
        Return strStringWithOutNulls
    End Function

    Private Sub Fillcorreo(ByVal ds As UserData)
        Dim ci As System.Globalization.CultureInfo
        Dim sErr As String = ""
        Dim Mail As emailTemplates.Template
        Mail = New emailTemplates.Template
        Mail.TemplateName = "TH_WORLDESTINATION_NEW_AGENCY"

        'ds = (New cUserSystem).GetUserByEmail(Me.txtContactoCorreo.Text)

        If ds.Tables(UserData.USER_TABLE).Rows.Count > 0 Then
            If (ds.Tables(UserData.USER_TABLE).Rows(0).IsNull(UserData.PASSWORDRAS_FIELD)) Then
                sErr = "Email sin password."
            Else
                Dim pas As String = ds.Tables(UserData.USER_TABLE).Rows(0).Item(UserData.PASSWORDRAS_FIELD)
                Mail.AddParameter("PASSWORD") = DecryptString128Bit(pas, crypto.PublicKey)
            End If
        Else
            lblError.Text = PortalCulture.GetString("00327", False) 'No se encontro el E-mail
            lblError.Visible = True
        End If
        Try
            Mail.To = Me.txtContactoCorreo.Text

            'Mail.SubjectParam
            Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Mail.Html = True
            Mail.AddParameter("AGENCYNAME") = Me.txtNombre.Text
            Mail.AddParameter("CONTACTO") = Me.txtContactoNombre.Text
            Mail.AddParameter("USEREMAIL") = Me.txtContactoCorreo.Text
            Mail.AddParameter("CONTACTOTEL") = Me.txtContactoTel.Text

            Mail.Send()

        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        End Try
    End Sub

    Private Function Update_RelatedContentAdmin(ByVal IdEmpresa As Integer, ByVal IdUserAdmin As Integer) As Boolean
        Dim res As Boolean
        Dim cnn As New SqlConnection(AppSettings("PortalConnection"))
        Dim StrSql As String = "Update PeticionRegistro set AsignadoA = {0} where idsolicitante = {1}"
        StrSql = String.Format(StrSql, IdUserAdmin, IdEmpresa)
        Dim cmd As New SqlCommand(StrSql, cnn)
        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            cnn.Close()
            res = True
        Catch
        End Try

        Return res
    End Function

    Public Function Update(ByVal idempresa As Integer) As Boolean
        Dim companyData As EmpresaDatos = Nothing
        Dim dsSource As New EmpresaDatos
        Dim idAdminReg As Integer

        Try

        
            If txtPaginaWeb.Text.Trim <> "" Then
                If Not txtPaginaWeb.Text.StartsWith("http://") Then txtPaginaWeb.Text = "http://" + txtPaginaWeb.Text
            End If

            '// Obten el registro original.
            Integer.TryParse(AppSettings("AdminCompanyRegistration"), idAdminReg)
            dsSource = (New EmpresaSistema).GetCompanyById(idempresa)
            If Editing AndAlso Not ViewState("_IATA_ORIG") Is Nothing AndAlso Not ViewState("_IATA_ORIG") = txtIATA.Text AndAlso Not (New Facade.AgenciaSistema).UpdateIATA(txtIATA.Text, idempresa) Then
                Return False
            End If

            With New EmpresaSistema 'deberia de modificarse el metodo para aceptar valores opcionales o crear una sobrecarga con menos parametros...
                If .UpdateCompany(idempresa, txtCiudad.Text, txtComentarios.Text, _
                txtContactoCorreo.Text, txtContactoNombre.Text, txtCP.Text, txtDomicilio.Text, _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Estado), "0", IIf(dsSource.Tables(0)(0)(dsSource.FIELD_Estado).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_Estado))), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Municipio), "0", IIf(dsSource.Tables(0)(0)(dsSource.FIELD_Municipio).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_Municipio))), _
                txtFax.Text, Date.Now, Guid.NewGuid.ToString, _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_IdCiudad), 0, dsSource.Tables(0)(0)(dsSource.FIELD_IdCiudad)), _
                cmbPaises.SelectedValue, txtNombre.Text, 25, 0, txtTel.Text, _
                IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_RazonSocial).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_RazonSocial)), _
                IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_DomFiscal).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_DomFiscal)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_BILL_Estado), ".", IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_Estado).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_Estado))), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_BILL_Municipio), ".", IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_Municipio).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_Municipio))), _
                cmbPaises.SelectedValue, _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_BILL_RFC), ".", IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_RFC).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_RFC))), _
                companyData, _
                IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_Ciudad).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_Ciudad)), _
                IIf(dsSource.Tables(0)(0)(dsSource.FIELD_BILL_IDCIUDAD).ToString().Trim() = "", ".", dsSource.Tables(0)(0)(dsSource.FIELD_BILL_IDCIUDAD)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_IDCATEGORIA), 0, dsSource.Tables(0)(0)(dsSource.FIELD_IDCATEGORIA)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_INVENTARIO), 0, dsSource.Tables(0)(0)(dsSource.FIELD_INVENTARIO)), _
                txtContactoPuesto.Text, _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_idArea), 0, dsSource.Tables(0)(0)(dsSource.FIELD_idArea)), _
                txtArea.Text, txtContactoTel.Text, _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto2Nombre), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto2Nombre)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto2Tel), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto2Tel)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto2puesto), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto2puesto)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto2Correo), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto2Correo)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto3Nombre), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto3Nombre)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto3Tel), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto3Tel)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto3puesto), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto3puesto)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_Contacto3Correo), "", dsSource.Tables(0)(0)(dsSource.FIELD_Contacto3Correo)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_ContactoGteNombre), "", dsSource.Tables(0)(0)(dsSource.FIELD_ContactoGteNombre)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_ContactoGteTel), "", dsSource.Tables(0)(0)(dsSource.FIELD_ContactoGteTel)), _
                IIf(dsSource.Tables(0)(0).IsNull(dsSource.FIELD_ContactoGteCorreo), "", dsSource.Tables(0)(0)(dsSource.FIELD_ContactoGteCorreo)), _
                False, "", txtPaginaWeb.Text.Trim, False) Then
                    Update_RelatedContentAdmin(idempresa, idAdminReg)

                    '// Guarda en bitacora la modificacion de la pisible modificacion del registro.
                    GuardaLogEmpresa(dsSource, companyData, True)
                    Clear()
                    Return True
                    '//EnviaCorreo(idAdminReg, txtNombre.Text, txtContactoNombre.Text, txtContactoTel.Text, txtContactoCorreo.Text)

                    'If idhotel > 0 Then
                    '    Dim dsHotel As New HotelDatos
                    '    With New HotelSistema
                    '        dsHotel = .GetHotelById(idhotel)
                    '    End With
                    '    If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                    '        With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                    '            .Item(HotelDatos.FIELD_CATEGORIA) = cmbCategoria.SelectedValue
                    '            If cmbCiudades.SelectedValue <> -1 Then .Item(HotelDatos.FIELD_IDCIUDAD) = cmbCiudades.SelectedValue
                    '            .Item(HotelDatos.FIELD_IDEMPRESA) = idempresa
                    '            .Item(HotelDatos.FIELD_IDMONEDA) = cmbMonedas.SelectedValue
                    '            If ddlCorporativos.SelectedValue <> 0 Then
                    '                .Item(HotelDatos.fld_idcorporativo) = ddlCorporativos.SelectedValue
                    '            End If
                    '        End With
                    '        With New Hoteles
                    '            If .ActualizaHotel(dsHotel) Then
                    '                Return True
                    '            End If
                    '        End With
                    '    Else
                    '        Return False
                    '    End If

                    'Else
                    '    'crea el hotel
                    '    Return addHotel(idempresa)
                    'End If

                Else
                    'Hubo error,redireccionar???
                    Return False
                End If
            End With
        Catch ex As Exception

        End Try
    End Function

    Private Function addAgency(ByVal idempresa As Integer, ByVal correo As String, ByRef status As Integer) As Boolean
        Dim ds As New AgencyData
        Dim daAgency As New AgenciaSistema
        status = daAgency.CreateAgency("", "", correo, idempresa, 0, 0, 0, 0, 0, txtIATA.Text, 0, "", "", 0, "", "", 0, "", "", ds)

        Return status = 0

    End Function
   
    Private Sub Carga_Paises()
        cmbPaises.DataSource = (New clsFacadePaises).GetPaises(PortalCulture.GetIDCulture())
        cmbPaises.DataTextField = clsCommonPaises.FLD_NOMBRE
        cmbPaises.DataValueField = clsCommonPaises.FLD_IDPAIS
        cmbPaises.DataBind()
        cmbPaises.SelectedIndex = cmbPaises.Items.IndexOf(cmbPaises.Items.FindByValue("MX"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here


        If Not IsPostBack And Not Me.Editing Then
            Call Carga_Paises()        
        End If
        If Not IsPostBack Then

            showError(False)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResource()
    End Sub

    Public Sub loadCompany(ByVal id As String)
        Dim dsEmpresa As EmpresaDatos
        Dim dsAgency As Common.Data.AgencyData


        With New EmpresaSistema
            dsEmpresa = .GetCompanyById(id)
        End With
        With dsEmpresa
            txtCiudad.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Ciudad).ToString()
            txtContactoCorreo.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_ContactoCorreo).ToString()
            txtContactoNombre.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_ContactoNombre).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_CP) Then txtCP.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_CP).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_Domicilio) Then txtDomicilio.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Domicilio).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_Fax) Then txtFax.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Fax).ToString()
            cmbPaises.SelectedValue = .Tables(.COMPANY_TABLE)(0)(.FIELD_IdPais).ToString()
            txtNombre.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Nombre).ToString()
            txtTel.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Telefono).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_ContactoPuesto) Then txtContactoPuesto.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_ContactoPuesto).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_Area) Then txtArea.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Area).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_ContactoTel) Then txtContactoTel.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_ContactoTel).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_PAGINAWEB) Then txtPaginaWeb.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_PAGINAWEB).ToString()
            If Not .Tables(.COMPANY_TABLE)(0).IsNull(.FIELD_Comentarios) Then txtComentarios.Text = .Tables(.COMPANY_TABLE)(0)(.FIELD_Comentarios).ToString()


        End With
        With New Facade.AgenciaSistema
            dsAgency = .GetAgencyByIDCompany(id)
        End With
        With dsAgency
            If Not .Tables(.C_TABLENAME_AGENCY)(0).IsNull(.C_IATA) Then
                txtIATA.Text = .Tables(.C_TABLENAME_AGENCY)(0)(.C_IATA).ToString()
                ViewState("_IATA_ORIG") = txtIATA.Text
            End If


        End With


    End Sub
End Class