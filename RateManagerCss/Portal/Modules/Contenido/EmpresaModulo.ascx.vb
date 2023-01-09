'Imports Contenido.Comun
'Imports Contenido.presentacion
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.General.Common.Data
Imports System.Data.SqlClient

Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos.Facade
Imports System.Configuration.ConfigurationManager

Partial Class EmpresaModulo
    Inherits System.Web.UI.UserControl

    Private strError As String

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
            Call Carga_Ciudades(False)
            Call Carga_Ciudades(True)
        End Set
    End Property

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Me.IsPostBack Then
            Me.CtrlPreserveScrolls1.Add(CtrlPreserveScrolls.TypeControl.THEWINDOW)
        End If
        If Not IsPostBack And Not Me.Editing Then
            Call Carga_Paises()
            Call Carga_Estados()
            Call Carga_Municipios(False)
            cmbMunicipioFiscal.DataSource = cmbMunicipio.DataSource
            cmbMunicipioFiscal.DataTextField = clsCommonMunicipios.FLD_NOMBRE
            cmbMunicipioFiscal.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
            cmbMunicipioFiscal.DataBind()

            Call Carga_Ciudades(False)
            cmbFiscalCiudades.DataSource = cmbCiudades.DataSource
            cmbFiscalCiudades.DataTextField = clsCommonMunicipios.FLD_NOMBRE
            cmbFiscalCiudades.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
            cmbFiscalCiudades.DataBind()

            cmbFiscalCiudades.Items.Add(PortalCulture.GetString("M0BT0000335", False))
            cmbFiscalCiudades.Items(cmbFiscalCiudades.Items.Count - 1).Value = 0

            If Not (cmbCiudades.SelectedItem Is Nothing) Then
                If cmbCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                    txtCiudad.Visible = True
                    txtCiudad.Text = ""
                Else
                    txtCiudad.Visible = False
                    txtCiudad.Text = cmbCiudades.SelectedItem.Text
                End If
                If cmbFiscalCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then ' OTRA_CIUDAD Then
                    txtCiudadFiscal.Visible = True
                    txtCiudadFiscal.Text = ""
                Else
                    txtCiudadFiscal.Visible = False
                    txtCiudadFiscal.Text = cmbFiscalCiudades.SelectedItem.Text
                End If
            End If

            Call Carga_Areas()
            If Not IsNothing(cmbArea.SelectedItem) Then
                If cmbArea.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then 'OTRA_CIUDAD Then
                    txtArea.Visible = True
                    txtArea.Text = ""
                Else
                    txtArea.Visible = False
                    txtArea.Text = cmbArea.SelectedItem.Text
                End If
            End If
            Carga_Monedas()
            CargaCorporativos()

            Call Carga_RegimenFiscal()

        End If
        If Not IsPostBack Then
            CheckOptionEnabled()
            showError(False)
        End If
        'Me.chkNombre.Attributes.Add("onclick", "javascript:CopyData('" & Me.chkNombre.ClientID & "','" & txtNombre.ClientID & "','" & txtRazonSocial.ClientID & "','0');")
        '  Me.chkDireccion.Attributes.Add("onclick", "javascript:CopyData('" & Me.chkDireccion.ClientID & "','" & txtDomicilio.ClientID & "','" & txtFiscalDom.ClientID & "','0');")
        ' Me.txtNombre.Attributes.Add("onchange", "javascript:CopyData('" & Me.chkNombre.ClientID & "','" & txtNombre.ClientID & "','" & txtRazonSocial.ClientID & "','1');")
        ' Me.txtDomicilio.Attributes.Add("onchange", "javascript:CopyData('" & Me.chkDireccion.ClientID & "','" & txtDomicilio.ClientID & "','" & txtFiscalDom.ClientID & "','1');")
    End Sub
    Public Sub showError(ByVal sw As Boolean)
        Me.lblError.Visible = sw
    End Sub
    Private Sub CargaCorporativos()
        Dim ds As DataSet
        If (New AuthUser).IsSupervisor Then
            With New HotelSistema
                ds = .GetCorporativos(0)
            End With
        Else
            With New HotelSistema
                ds = .GetCorporativos(CType(Me.Page, PaginaBase).UserIdentityName)
            End With
        End If
        ddlCorporativos.DataSource = ds
        ddlCorporativos.DataTextField = "NombreCorp"
        ddlCorporativos.DataValueField = "idCorporativo"
        ddlCorporativos.DataBind()
        If (New AuthUser).IsSupervisor Then
            ddlCorporativos.Items.Insert(0, PortalCulture.GetString("M000482"))
            ddlCorporativos.Items(0).Value = 0
        End If

    End Sub
    Private Sub Carga_Monedas()
        cmbMonedas.DataSource = (New MonedaSistema).GetMonedaListIdName
        cmbMonedas.DataTextField = "Nombre"
        cmbMonedas.DataValueField = "idMoneda"
        cmbMonedas.DataBind()
    End Sub
    Public Function LoadData(ByVal idempresa As Integer, ByVal sGuid As String, ByVal idhotel As Integer, ByRef status As Byte) As Boolean
        'Carga_Categorias(10)
        Carga_Monedas()
        CargaCorporativos()
        Carga_RegimenFiscal()
        Dim data As EmpresaDatos
        data = (New EmpresaSistema).GetCompanyById(idempresa)
        If Not (data Is Nothing) AndAlso data.Tables(EmpresaDatos.COMPANY_TABLE).Rows.Count > 0 Then
            If data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Guid) = sGuid Then
                Editing = True
                lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("00823"))
                txtCiudad.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Ciudad)

                txtContactoCorreo.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoCorreo)
                txtContactoNombre.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoNombre)
                txtCP.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_CP)
                txtDomicilio.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Domicilio)
                txtFax.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Fax)
                txtNombre.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Nombre)

                txtTel.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Telefono)
                txtRazonSocial.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_RazonSocial)
                txtFiscalDom.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_DomFiscal)

                txtRFC.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_RFC)
                If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_CP)) Then
                    txtFacturacionCP.Text = data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_CP)
                End If
                txtCiudadFiscal.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Ciudad)

                txtContactoPuesto.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoPuesto)

                txtArea.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Area)
                txtContactoTel.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoTel)
                txtContacto2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2Nombre)
                txtTel2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2Tel)
                txtPuesto2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2puesto)
                txtCorreo2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2Correo)
                txtContacto3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3Nombre)
                txtTel3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3Tel)
                txtPuesto3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3puesto)
                txtCorreo3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3Correo)
                txtGerente.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoGteNombre)
                txtTelG.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoGteTel)
                txtCorreoG.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoGteCorreo)
                txtPaginaWeb.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_PAGINAWEB)

                If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_RegimenFiscal)) Then
                    cmbFacturacionEmpresaFiscal.SelectedIndex = cmbFacturacionEmpresaFiscal.Items.IndexOf(cmbFacturacionEmpresaFiscal.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_RegimenFiscal)))
                End If


                'Call Carga_Categorias(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Rubro))
                If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IDCATEGORIA)) Then cmbCategoria.SelectedIndex = cmbCategoria.Items.IndexOf(cmbCategoria.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IDCATEGORIA)))


                Call Carga_Paises()
                cmbPaises.SelectedIndex = cmbPaises.Items.IndexOf(cmbPaises.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IdPais)))

                Dim estado As Integer = -1
                Dim municipio As Integer = -1
                Dim ciudad As Integer = data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IdCiudad)
                Dim err As String

                If ciudad > 0 Then
                    Dim citydata As clsCommonCiudades
                    citydata = (New clsFacadeCiudades).GetById(ciudad, err)
                    If Not IsNothing(citydata) Then
                        municipio = citydata.Tables(citydata.TABLA_CIUDADES).Rows(0).Item(citydata.FLD_IDMUNICIPIO)
                        Dim mundata As clsCommonMunicipios
                        mundata = (New clsFacadeMunicipios).GetById(municipio, err)

                        estado = mundata.Tables(mundata.TABLA_MUNICIPIOS).Rows(0).Item(mundata.FLD_IDESTADO)

                        Call Carga_Estados()
                        cmbEstados.SelectedIndex = cmbEstados.Items.IndexOf(cmbEstados.Items.FindByValue(estado))

                        Call Carga_Municipios(False)
                        cmbMunicipio.SelectedIndex = cmbMunicipio.Items.IndexOf(cmbMunicipio.Items.FindByValue(municipio))

                        Call Carga_Ciudades(False)
                        cmbCiudades.SelectedIndex = cmbCiudades.Items.IndexOf(cmbCiudades.Items.FindByValue(ciudad))
                        If Not (cmbCiudades.SelectedItem Is Nothing) Then
                            If cmbCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                                txtCiudad.Visible = True                        '     txtCiudad.Text = ""
                            Else
                                txtCiudad.Visible = False                        '      txtCiudad.Text = cmbCiudades.SelectedItem.Text
                            End If
                        End If
                    End If
                Else
                    'cmbEstados.Items.Clear()
                    Call Carga_Estados()
                    If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Estado)) Then
                        cmbEstados.SelectedIndex = cmbEstados.Items.IndexOf(cmbEstados.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Estado)))
                    End If
                    'cmbMunicipio.Items.Clear()
                    Call Carga_Municipios(False)
                    If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Municipio)) Then
                        cmbMunicipio.SelectedIndex = cmbMunicipio.Items.IndexOf(cmbMunicipio.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Municipio)))
                    End If

                    Call Carga_Ciudades(False)
                    cmbCiudades.SelectedIndex = cmbCiudades.Items.Count - 1
                    If Not (cmbCiudades.SelectedItem Is Nothing) Then
                        If cmbCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                            txtCiudad.Visible = True                        '     txtCiudad.Text = ""
                        Else
                            txtCiudad.Visible = False                        '      txtCiudad.Text = cmbCiudades.SelectedItem.Text
                        End If
                    End If
                End If

                If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_IDCIUDAD)) Then
                    estado = -1
                    municipio = -1
                    ciudad = data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_IDCIUDAD)

                    If ciudad > 0 Then
                        Dim citydata As clsCommonCiudades
                        citydata = (New clsFacadeCiudades).GetById(ciudad, err)
                        If Not citydata Is Nothing AndAlso citydata.Tables(citydata.TABLA_CIUDADES).Rows.Count > 0 Then
                            municipio = citydata.Tables(citydata.TABLA_CIUDADES).Rows(0).Item(citydata.FLD_IDMUNICIPIO)
                            Dim mundata As clsCommonMunicipios
                            mundata = (New clsFacadeMunicipios).GetById(municipio, err)

                            estado = mundata.Tables(mundata.TABLA_MUNICIPIOS).Rows(0).Item(mundata.FLD_IDESTADO)

                            'Call Carga_Estados()
                            cmbFiscalEstados.SelectedIndex = cmbFiscalEstados.Items.IndexOf(cmbFiscalEstados.Items.FindByValue(estado))

                            Call Carga_Municipios(True)
                            cmbMunicipioFiscal.SelectedIndex = cmbMunicipioFiscal.Items.IndexOf(cmbMunicipioFiscal.Items.FindByValue(municipio))

                            Call Carga_Ciudades(True)
                            cmbFiscalCiudades.SelectedIndex = cmbFiscalCiudades.Items.IndexOf(cmbFiscalCiudades.Items.FindByValue(ciudad))
                            If Not (cmbFiscalCiudades.SelectedItem Is Nothing) Then
                                If cmbFiscalCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                                    txtCiudadFiscal.Visible = True                       '     txtCiudad.Text = ""
                                Else
                                    txtCiudadFiscal.Visible = False                        '      txtCiudad.Text = cmbCiudades.SelectedItem.Text
                                End If
                            End If
                        Else
                            'No existe la ciudad
                            Call Carga_Municipios(True)
                            Call Carga_Ciudades(True)


                        End If
                    Else

                        If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Estado)) Then
                            cmbFiscalEstados.SelectedIndex = cmbFiscalEstados.Items.IndexOf(cmbFiscalEstados.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Estado)))
                        End If

                        Call Carga_Municipios(True)
                        If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Municipio)) Then
                            cmbMunicipioFiscal.SelectedIndex = cmbMunicipioFiscal.Items.IndexOf(cmbMunicipioFiscal.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Municipio)))
                        End If

                        '---------------------------------------------
                        Call Carga_Ciudades(True)
                        cmbFiscalCiudades.SelectedIndex = cmbFiscalCiudades.Items.Count - 1
                        If Not (cmbFiscalCiudades.SelectedItem Is Nothing) Then
                            If cmbFiscalCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                                txtCiudadFiscal.Visible = True                       '     txtCiudad.Text = ""
                            Else
                                txtCiudadFiscal.Visible = False                        '      txtCiudad.Text = cmbCiudades.SelectedItem.Text
                            End If
                        End If
                    End If
                End If
                Call Carga_Areas()
                If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_idArea)) Then
                    cmbArea.SelectedIndex = cmbArea.Items.IndexOf(cmbArea.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_idArea)))
                Else
                    cmbArea.SelectedIndex = cmbArea.Items.IndexOf(cmbArea.Items.FindByValue(0))
                End If
                If txtArea.Text.Trim = "" Then
                    If Not IsNothing(cmbArea.SelectedItem) Then txtArea.Text = cmbArea.SelectedItem.Text
                End If
                If Not IsNothing(cmbArea.SelectedItem) Then
                    If cmbArea.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                        txtArea.Visible = True
                    Else
                        txtArea.Visible = False
                    End If
                End If
                txtInventario.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_INVENTARIO)
                Try
                    cmbAdmin.SelectedValue = Load_RelatedContentAdmin(idempresa)
                Catch
                    cmbAdmin.SelectedValue = 0
                End Try
                status = data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Status)
                loadHotelData(idhotel)
                Return True


            End If
        End If
    End Function
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
    Public Sub CheckOptionEnabled()
        If Not IsPostBack Then
            If cmbAdmin.Items.Count = 0 Then
                Dim datAdmin As AdministratorData
                With New cAdministratorSystem
                    datAdmin = .GetAdmin()
                End With
                If Not IsNothing(datAdmin) Then
                    Try
                        Dim view As DataView
                        ' filtramos los administradores de contenido
                        view = datAdmin.Tables(AdministratorData.ADMINISTRATOR_TABLE).DefaultView
                        view.RowFilter = AdministratorData.TYPE_FIELD & "=" & AdministratorData.AdministratorType.Content
                        'Dim drs As DataRow()
                        'drs = datAdmin.Tables(AdministratorData.ADMINISTRATOR_TABLE).Select(AdministratorData.TYPE_FIELD & "=" & AdministratorData.AdministratorType.Content)
                        Me.cmbAdmin.DataSource = view
                        Me.cmbAdmin.DataTextField = AdministratorData.EMAIL_FIELD
                        Me.cmbAdmin.DataValueField = AdministratorData.IDUSER_FIELD
                        Me.cmbAdmin.DataBind()
                    Catch ex As Exception

                    End Try

                End If
            End If
        End If
        ' agregar opción de todos los administradores
        If PortalCulture.GetIDCulture = 1 Then
            Me.cmbAdmin.Items.Insert(0, "- Seleccionar -")
        Else
            Me.cmbAdmin.Items.Insert(0, "- Select -")
        End If
        Me.cmbAdmin.Items(0).Value = 0

    End Sub
    Private Function loadHotelData(ByVal idhotel As Integer) As Boolean
        Dim dsHotel As New HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(idhotel)
        End With
        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                If Not .IsNull(HotelDatos.FIELD_CATEGORIA) Then
                    cmbCategoria.SelectedValue = .Item(HotelDatos.FIELD_CATEGORIA)
                End If
                'If Not .IsNull(HotelDatos.FIELD_IDCIUDAD) Then
                '    cmbCiudades.SelectedValue = .Item(HotelDatos.FIELD_IDCIUDAD)
                'End If
                Try
                    If Not .IsNull(HotelDatos.fld_idcorporativo) Then
                        ddlCorporativos.SelectedValue = .Item(HotelDatos.fld_idcorporativo)
                    End If
                Catch ex As Exception

                End Try

                If Not .IsNull(HotelDatos.FIELD_IDMONEDA) Then
                    cmbMonedas.SelectedValue = .Item(HotelDatos.FIELD_IDMONEDA)
                End If
                If Not .IsNull(HotelDatos.FIELD_IsHouse) AndAlso .Item(HotelDatos.FIELD_IsHouse) Then
                    ddlCompanyType.SelectedValue = 1
                End If

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
            sdatos = Util.Utility.GetXml(EmpresaDatos.COMPANY_TABLE, "UpdateRegisterCompany", dsSource)
            If isModify Then _
                sdatosMod = Util.Utility.GetXml(EmpresaDatos.COMPANY_TABLE, "UpdateRegisterCompany", companyData)
            eAction = IIf(isModify, PaginaBase.acciones.Modificar, PaginaBase.acciones.Crear)
            sEmpresa = String.Format("Se {0} registro de la empresa: {1}", IIf(isModify, "modificó", "creó"), txtNombre.Text)

            CType(Me.Page, PaginaBase).guardalog("/registro/CompanyRegister.aspx", _
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
        Dim adminSystem As Portal.General.DataAccess.cAdministrators
        Dim userSystem As New Portal.General.Facade.cUserSystem
        Dim user As UserData
        Dim admin As AdministratorData
        Dim Prov As New PortalPartnersCfg
        Dim strAdminMail As String
        Dim idioma As String



        'adminSystem = New Portal.General.DataAccess.cAdministrators()
        'admin = adminSystem.GetAdminById(AdminId)

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

        Mail.Idioma = idioma 'PortalCulture.GetCulture.ToString
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


    Public Function Add(ByRef idempresa As Integer) As Boolean
        Dim companyData As EmpresaDatos
        Dim cadErrores As String
        Dim x As Integer

        ' Se quito el combo
        'If Me.cmbAdmin.SelectedValue = 0 Then
        '    rfv_Admin.IsValid = False
        '    Return False
        'End If


        If txtPaginaWeb.Text.Trim <> "" Then
            If Not txtPaginaWeb.Text.StartsWith("http://") Then txtPaginaWeb.Text = "http://" + txtPaginaWeb.Text
        End If

        Dim idCategoria As Integer = cmbCategoria.SelectedValue

        With New EmpresaSistema
            Dim estado As String = "."
            Dim estadof As String = "."
            Dim municipio As String = "."
            Dim municipiof As String = "."
            Dim WDTAdministrator As String() = AppSettings("WDTAdministrator").Split(",")
            If Not IsNothing(cmbEstados.SelectedItem) Then estado = cmbEstados.SelectedItem.Text
            If Not IsNothing(cmbFiscalEstados.SelectedItem) Then estadof = cmbFiscalEstados.SelectedItem.Text
            If Not IsNothing(cmbMunicipio.SelectedItem) Then municipio = cmbMunicipio.SelectedItem.Text
            If Not IsNothing(cmbMunicipioFiscal.SelectedItem) Then municipiof = cmbMunicipioFiscal.SelectedItem.Text
            '            If (cmbEstados.SelectedIndex >= 0 And cmbMunicipio.SelectedIndex >= 0 And cmbCiudades.SelectedIndex >= 0 _
            '            And cmbRubros.SelectedIndex >= 0 And cmbFiscalEstados.SelectedIndex >= 0 And cmbMunicipioFiscal.SelectedIndex >= 0 And cmbFiscalCiudades.SelectedIndex >= 0 And cmbCategoria.SelectedIndex >= 0) AndAlso .CreateCompany(txtCiudad.Text, txtComentarios.Text, _
            If (cmbCiudades.SelectedIndex >= 0 And cmbFiscalCiudades.SelectedIndex >= 0 And cmbCategoria.SelectedIndex >= 0) Then
                ' If Me.idAdmin <> 0 Then
                Dim IdAdminist As Integer = 0
                'Dim ds As Portal.General.Common.Data.UserData
                'With New Portal.General.Facade.cUserSystem
                '    ds = .GetUserByEmail((New AuthUser).Usuario)
                '    'ds = .GetUserByEmail(Context.User.Identity.Name)
                'End With
                'If ds.Tables(ds.USER_TABLE).Rows.Count > 0 Then
                '    IdAdminist = ds.Tables(ds.USER_TABLE).Rows(0).Item(ds.IDUSER_FIELD)
                'End If

                'IdAdminist = cmbAdmin.SelectedValue
                Integer.TryParse(AppSettings("AdminCompanyRegistration"), IdAdminist)

                If .CreateCompany(IdAdminist, Me.ididioma, txtCiudad.Text, "",
                     txtContactoCorreo.Text, txtContactoNombre.Text, txtCP.Text, txtDomicilio.Text,
                     estado, municipio, txtFax.Text, Date.Now, Guid.NewGuid.ToString,
                     cmbCiudades.SelectedValue, cmbPaises.SelectedValue, txtNombre.Text, 10,
                     0, txtTel.Text, txtRazonSocial.Text, txtFiscalDom.Text,
                     estadof, municipiof, cmbPaises.SelectedValue, txtRFC.Text.Trim, companyData, txtCiudadFiscal.Text.Trim, cmbFiscalCiudades.SelectedValue, cmbCategoria.SelectedValue, txtInventario.Text, txtContactoPuesto.Text, cmbArea.SelectedValue, txtArea.Text, txtContactoTel.Text,
                     txtContacto2.Text, txtTel2.Text, txtPuesto2.Text, txtCorreo2.Text, txtContacto3.Text, txtTel3.Text, txtPuesto3.Text, txtCorreo3.Text, txtGerente.Text, txtTelG.Text, txtCorreoG.Text, txtPaginaWeb.Text.Trim, False, False, Bill_CP:=txtFacturacionCP.Text, Bill_RegimenFiscal:=cmbFacturacionEmpresaFiscal.SelectedValue) Then
                    idempresa = companyData.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_idEmpresa)

                    If AppSettings("idSegmento") = "4" Then
                        For Each Administrator As String In WDTAdministrator
                            AssociateAdministrator(Administrator, idempresa, IdAdminist)
                        Next
                    End If

                    '// Guarda en bitacora, se creo una nueva empresa.
                    GuardaLogEmpresa(companyData, companyData, False)


                    EnviaCorreo(IdAdminist, txtNombre.Text, txtContactoNombre.Text, txtContactoTel.Text, txtContactoCorreo.Text)

                    Return addHotel(idempresa)
                    'If Me.txtUsuarioEmail.Text <> "" Then
                    'Me.AsociaUsuario(Me.txtUsuarioEmail.Text, idempresa)
                    'End If
                    Return True
                End If

            End If
            Return False
        End With
    End Function

    Private Sub AssociateAdministrator(ByVal email As String, ByVal IdEmpresa As Integer, ByVal IdAdminist As Integer)
        Try
            Dim cmdText As String = "spAssociateAdministrator"
            Dim cnn As New SqlConnection(AppSettings("PortalConnection"))
            Dim cmd As New SqlCommand(cmdText, cnn)
            cmd.CommandType = CommandType.StoredProcedure
            Try
                cnn.Open()
                cmd.Parameters.Add(New SqlParameter("@idEmpresa", IdEmpresa)) 'Id Del Hotel registrado
                cmd.Parameters.Add(New SqlParameter("@EmailUser", email)) 'Usuarios a los que se asociará el nuevo hotel
                cmd.Parameters.Add(New SqlParameter("@IdAdminist", IdAdminist)) 'Usuario que registra el hotel
                cmd.ExecuteNonQuery()
            Catch ex As Exception
            Finally
                If cnn IsNot Nothing AndAlso cnn.State <> ConnectionState.Closed Then
                    cnn.Close()
                End If
            End Try
        Catch ex As Exception
        End Try
    End Sub

    Private Function Create_Relatedassociation(ByVal idEmpresa As Integer) As Boolean
        Dim result As Boolean = False
        Try
            Dim idAsociacion As Integer = 0

            Dim cmdText As String = "spCreateRelatedassociation"
            Dim cnn As New SqlConnection(AppSettings("HotelConnection"))
            Dim cmd As New SqlCommand(cmdText, cnn)
            cmd.CommandType = CommandType.StoredProcedure
            If AppSettings("HotelConnection") <> String.Empty AndAlso AppSettings("IdAsociation") IsNot Nothing AndAlso Integer.TryParse(AppSettings("IdAsociation"), idAsociacion) Then
                Try
                    cnn.Open()
                    cmd.Parameters.Add(New SqlParameter("@idEmpresa", idEmpresa))
                    cmd.Parameters.Add(New SqlParameter("@idAsociacion", idAsociacion))
                    cmd.ExecuteNonQuery()
                    result = True
                Catch ex As Exception
                Finally
                    If cnn IsNot Nothing AndAlso cnn.State <> ConnectionState.Closed Then
                        cnn.Close()
                    End If
                End Try
            End If
        Catch ex As Exception
        End Try
        Return result
    End Function


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

    Public Function Update(ByVal idempresa As Integer, ByVal idhotel As Integer) As Boolean
        Dim companyData As EmpresaDatos
        Dim dsSource As New EmpresaDatos
        Dim idAdminReg As Integer
        'Se quito el combo
        'If Me.cmbAdmin.SelectedValue = 0 Then
        '    rfv_Admin.IsValid = False
        '    Return False
        'End If
        If txtPaginaWeb.Text.Trim <> "" Then
            If Not txtPaginaWeb.Text.StartsWith("http://") Then txtPaginaWeb.Text = "http://" + txtPaginaWeb.Text
        End If

        '// Obten el registro original.
        Integer.TryParse(AppSettings("AdminCompanyRegistration"), idAdminReg)
        dsSource = (New EmpresaSistema).GetCompanyById(idempresa)
        With New EmpresaSistema
            If .UpdateCompany(idempresa, txtCiudad.Text, "",
            txtContactoCorreo.Text, txtContactoNombre.Text, txtCP.Text, txtDomicilio.Text,
            cmbEstados.SelectedItem.Text, cmbMunicipio.SelectedItem.Text, txtFax.Text, Date.Now, Guid.NewGuid.ToString,
            cmbCiudades.SelectedValue, cmbPaises.SelectedValue, txtNombre.Text, 10,
            0, txtTel.Text, txtRazonSocial.Text, txtFiscalDom.Text,
            cmbFiscalEstados.SelectedItem.Text, cmbMunicipioFiscal.SelectedItem.Text, cmbPaises.SelectedValue, txtRFC.Text.Trim, companyData, txtCiudadFiscal.Text.Trim, cmbFiscalCiudades.SelectedValue, cmbCategoria.SelectedValue, txtInventario.Text, txtContactoPuesto.Text, cmbArea.SelectedValue, txtArea.Text, txtContactoTel.Text,
                txtContacto2.Text, txtTel2.Text, txtPuesto2.Text, txtCorreo2.Text, txtContacto3.Text, txtTel3.Text, txtPuesto3.Text, txtCorreo3.Text, txtGerente.Text, txtTelG.Text, txtCorreoG.Text, False, "", txtPaginaWeb.Text.Trim, False, txtFacturacionCP.Text, cmbFacturacionEmpresaFiscal.SelectedValue) Then
                Update_RelatedContentAdmin(idempresa, idAdminReg)

                '// Guarda en bitacora la modificacion de la pisible modificacion del registro.
                GuardaLogEmpresa(dsSource, companyData, True)

                '//EnviaCorreo(idAdminReg, txtNombre.Text, txtContactoNombre.Text, txtContactoTel.Text, txtContactoCorreo.Text)

                If idhotel > 0 Then
                    Dim dsHotel As New HotelDatos
                    With New HotelSistema
                        dsHotel = .GetHotelById(idhotel)
                    End With
                    If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                        With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                            .Item(HotelDatos.FIELD_CATEGORIA) = cmbCategoria.SelectedValue
                            If cmbCiudades.SelectedValue <> -1 Then .Item(HotelDatos.FIELD_IDCIUDAD) = cmbCiudades.SelectedValue
                            .Item(HotelDatos.FIELD_IDEMPRESA) = idempresa
                            .Item(HotelDatos.FIELD_IDMONEDA) = cmbMonedas.SelectedValue
                            If ddlCorporativos.SelectedValue <> 0 Then
                                .Item(HotelDatos.fld_idcorporativo) = ddlCorporativos.SelectedValue
                            End If
                            .Item(HotelDatos.FIELD_IsHouse) = CInt(ddlCompanyType.SelectedValue)
                        End With
                        With New Hoteles
                            If .ActualizaHotel(dsHotel, strError) Then
                                Return True
                            End If
                        End With
                    Else
                        Return False
                    End If

                Else
                    'crea el hotel
                    Return addHotel(idempresa)
                End If

            Else
                'Hubo error,redireccionar???
                Return False
            End If
        End With

    End Function
    Private Function addHotel(ByVal idempresa As Integer) As Boolean
        Dim strCheckin As Date
        Dim strCheckOut As Date
        strCheckin = CDate(Format(Date.Now, "yyyy/MM/dd ") & "12:00")
        strCheckOut = CDate(Format(Date.Now, "yyyy/MM/dd ") & "12:00")
        Dim row As DataRow
        Dim hotelData As New HotelDatos
        row = hotelData.Tables(HotelDatos.HOTEL_TABLE).NewRow
        ' Fill input data into new row
        With row
            .Item(HotelDatos.FIELD_CATEGORIA) = cmbCategoria.SelectedValue
            .Item(HotelDatos.FIELD_CHECKIN) = strCheckin
            .Item(HotelDatos.FIELD_CHECKOUT) = strCheckOut
            .Item(HotelDatos.FIELD_DIASLIBRES) = 0
            .Item(HotelDatos.FIELD_DIASMINCANCELAR) = 0
            .Item(HotelDatos.FIELD_EDADAPARTIRPAGAEXTRA) = 0
            If cmbCiudades.SelectedValue <> -1 Then .Item(HotelDatos.FIELD_IDCIUDAD) = cmbCiudades.SelectedValue
            .Item(HotelDatos.FIELD_IDEMPRESA) = idempresa
            .Item(HotelDatos.FIELD_IDMONEDA) = cmbMonedas.SelectedValue
            .Item(HotelDatos.FIELD_IMPUESTO) = 0
            .Item(HotelDatos.FIELD_MAXDIASRENTA) = 0
            .Item(HotelDatos.FIELD_MAXEDADNINO) = 0
            .Item(HotelDatos.FIELD_MAXNUMADULTOS) = 0
            .Item(HotelDatos.FIELD_MAXNUMCUARTOS) = 0
            .Item(HotelDatos.FIELD_MAXNUMNINOS) = 0
            .Item(HotelDatos.FIELD_AVLONCORPMODULE) = False
            If ddlCorporativos.SelectedValue <> 0 Then
                .Item(HotelDatos.fld_idcorporativo) = ddlCorporativos.SelectedValue
            End If
            .Item(HotelDatos.FIELD_IsHouse) = CInt(ddlCompanyType.SelectedValue)
        End With
        ' Add it to the table

        hotelData.Tables(HotelDatos.HOTEL_TABLE).Rows.Add(row)
        Dim result As Boolean = False
        With New HotelSistema
            result = .CreateHotel(PortalCulture.GetCulture.ToString, txtContactoCorreo.Text, hotelData)
            If (result) Then
                Create_Relatedassociation(idempresa)
            End If
        End With
        Return result


    End Function
    'Private Sub Carga_Categorias(ByVal Rubro As Integer)
    '    'Dim strSQL As String = "SELECT idCategoria,Descripcion FROM Categorias WHERE idRubro=" & Rubro
    '    Dim strSQL As String = "SELECT idCategoria, " & _
    '                            "Case " & PortalCulture.GetIDCulture.ToString & _
    '                            "when 1 then isnull((select top 1 texto from diccionario where idDiccionario = categorias.idDiccionario and idIdioma = 1),descripcion) " & _
    '                            "when 2 then isnull((select top 1 texto from diccionario where idDiccionario = categorias.idDiccionario and idIdioma = 2),descripcion) " & _
    '                            "else Descripcion " & _
    '                            "end as Descripcion  " & _
    '                            "FROM Categorias Categorias WHERE idRubro= " & Rubro

    '    Dim da As New SqlDataAdapter(strSQL, AppSettings("PortalConnection"))
    '    Dim ds As New DataSet
    '    da.Fill(ds)
    '    cmbCategoria.DataSource = ds
    '    cmbCategoria.DataValueField = "idCategoria"
    '    cmbCategoria.DataTextField = "Descripcion"
    '    cmbCategoria.DataBind()
    '    'falta la especial

    'End Sub

    Private Sub Carga_RegimenFiscal()
        cmbFacturacionEmpresaFiscal.DataSource = (New RegimenFiscalSistema).GetTaxRegime()
        cmbFacturacionEmpresaFiscal.DataTextField = RegimenFiscalDatos.FIELD_DESCRIPTION
        cmbFacturacionEmpresaFiscal.DataValueField = RegimenFiscalDatos.FIELD_TAX_REGIME
        cmbFacturacionEmpresaFiscal.DataBind()
        cmbFacturacionEmpresaFiscal.SelectedIndex = cmbFacturacionEmpresaFiscal.Items(0).Value
    End Sub

    Private Sub Carga_Paises()
        Dim strErr As String
        cmbPaises.DataSource = (New clsFacadePaises).GetPaises(PortalCulture.GetIDCulture())
        cmbPaises.DataTextField = clsCommonPaises.FLD_NOMBRE
        cmbPaises.DataValueField = clsCommonPaises.FLD_IDPAIS
        cmbPaises.DataBind()
        cmbPaises.SelectedIndex = cmbPaises.Items.IndexOf(cmbPaises.Items.FindByValue("MX"))
    End Sub
    Private Sub Carga_Estados()
        If Not (cmbPaises.SelectedItem Is Nothing) Then
            Dim strErr As String
            cmbFiscalEstados.DataSource = (New clsFacadeEstados).GetByPais(cmbPaises.SelectedValue, strErr)
            cmbFiscalEstados.DataTextField = clsCommonEstados.FLD_NOMBRE
            cmbFiscalEstados.DataValueField = clsCommonEstados.FLD_IDESTADO
            cmbFiscalEstados.DataBind()
            cmbEstados.DataSource = cmbFiscalEstados.DataSource
            cmbEstados.DataTextField = clsCommonEstados.FLD_NOMBRE
            cmbEstados.DataValueField = clsCommonEstados.FLD_IDESTADO
            cmbEstados.DataBind()
        Else
            cmbEstados.Items.Clear()
            'cmbFiscalEstados.Items.Clear()
        End If
    End Sub

    Private Sub Carga_Municipios(ByVal Fiscal As Boolean)
        Dim strErr As String
        If Fiscal Then
            If Not (cmbFiscalEstados.SelectedItem Is Nothing) Then
                Dim combo As DropDownList
                cmbMunicipioFiscal.DataSource = (New clsFacadeMunicipios).GetByIdEstado(cmbFiscalEstados.SelectedValue, strErr)
                cmbMunicipioFiscal.DataTextField = clsCommonMunicipios.FLD_NOMBRE
                cmbMunicipioFiscal.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
                cmbMunicipioFiscal.DataBind()
            Else
                cmbMunicipioFiscal.Items.Clear()
            End If
        Else
            If Not (cmbEstados.SelectedItem Is Nothing) Then
                cmbMunicipio.DataSource = (New clsFacadeMunicipios).GetByIdEstado(cmbEstados.SelectedValue, strErr)
                cmbMunicipio.DataTextField = clsCommonMunicipios.FLD_NOMBRE
                cmbMunicipio.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
                cmbMunicipio.DataBind()
            Else
                cmbMunicipio.Items.Clear()
            End If
        End If
    End Sub
    Private Sub Carga_Ciudades(ByVal Fiscal As Boolean)
        If Not Fiscal Then
            If Not (cmbMunicipio.SelectedItem Is Nothing) Then
                Dim combo As DropDownList
                Dim strErr As String

                combo = cmbCiudades
                combo.DataSource = (New clsFacadeCiudades).GetByIdMunicipio(cmbMunicipio.SelectedValue, strErr)

                combo.DataTextField = clsCommonCiudades.FLD_NOMBRE
                combo.DataValueField = clsCommonCiudades.FLD_IDCIUDAD
                combo.DataBind()
            Else
                cmbCiudades.Items.Clear()

            End If
            cmbCiudades.Items.Add(PortalCulture.GetString("00821", False))
            cmbCiudades.Items(cmbCiudades.Items.Count - 1).Value = 0
        Else
            If Not (cmbMunicipioFiscal.SelectedItem Is Nothing) Then
                Dim combo As DropDownList
                Dim strErr As String

                combo = cmbFiscalCiudades
                combo.DataSource = (New clsFacadeCiudades).GetByIdMunicipio(cmbMunicipioFiscal.SelectedValue, strErr)

                combo.DataTextField = clsCommonCiudades.FLD_NOMBRE
                combo.DataValueField = clsCommonCiudades.FLD_IDCIUDAD
                combo.DataBind()
            Else
                cmbFiscalCiudades.Items.Clear()
            End If
            cmbFiscalCiudades.Items.Add(PortalCulture.GetString("00821", False))
            cmbFiscalCiudades.Items(cmbFiscalCiudades.Items.Count - 1).Value = 0
        End If
    End Sub
    Private Sub Carga_Areas()
        If cmbCiudades.Items.Count > 0 Then
            Dim cadError As String
            cmbArea.DataTextField = clsCommonAreas.FLD_NOMBRE
            cmbArea.DataValueField = clsCommonAreas.FLD_IDAREA
            cmbArea.DataSource = (New clsFacadeAreas).GetByIdCiudad(cmbCiudades.SelectedValue, cadError)
            cmbArea.DataBind()
            cmbArea.Items.Add("--")
            cmbArea.Items(cmbArea.Items.Count - 1).Value = -1
            cmbArea.Items.Add(PortalCulture.GetString("00821", False))
            cmbArea.Items(cmbArea.Items.Count - 1).Value = 0
        End If
    End Sub

    Private Sub Copia_Ciudad()
        cmbFiscalEstados.SelectedIndex = cmbEstados.SelectedIndex
        cmbFiscalEstados_SelectedIndexChanged(New System.Object, New System.EventArgs)

        cmbMunicipioFiscal.SelectedIndex = cmbMunicipio.SelectedIndex
        cmbMunicipioFiscal_SelectedIndexChanged(New System.Object, New System.EventArgs)

        cmbFiscalCiudades.SelectedIndex = cmbCiudades.SelectedIndex
        cmbFiscalCiudades_SelectedIndexChanged(New System.Object, New System.EventArgs)
    End Sub
#Region "Eventos"
    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        Server.ClearError()
    End Sub
    Private Sub cmbPaises_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPaises.SelectedIndexChanged
        Call Carga_Estados()
        'cmbPaises.UpdateAfterCallBack = True
        cmbEstados_SelectedIndexChanged(sender, e)
        cmbFiscalEstados_SelectedIndexChanged(sender, e)
        'cmbEstados.UpdateAfterCallBack = True
        'cmbFiscalEstados.UpdateAfterCallBack = True
    End Sub

    Private Sub cmbEstados_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEstados.SelectedIndexChanged
        Call Carga_Municipios(False)
        'cmbEstados.UpdateAfterCallBack = True
        cmbMunicipio_SelectedIndexChanged(sender, e)

        cmbFiscalEstados.SelectedIndex = cmbEstados.SelectedIndex
        cmbFiscalEstados_SelectedIndexChanged(sender, e)

        'cmbFiscalEstados.UpdateAfterCallBack = True
        'cmbMunicipio.UpdateAfterCallBack = True

    End Sub

    Private Sub cmbFiscalEstados_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFiscalEstados.SelectedIndexChanged
        Call Carga_Municipios(True)
        'cmbMunicipioFiscal.UpdateAfterCallBack = True

        cmbMunicipioFiscal_SelectedIndexChanged(sender, e)

    End Sub

    Private Sub cmbMunicipio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMunicipio.SelectedIndexChanged
        Call Carga_Ciudades(False)
        'cmbMunicipio.UpdateAfterCallBack = True
        cmbCiudades_SelectedIndexChanged(sender, e)
        'cmbCiudades.UpdateAfterCallBack = True

    End Sub

    Private Sub cmbMunicipioFiscal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMunicipioFiscal.SelectedIndexChanged
        Call Carga_Ciudades(True)
        cmbFiscalCiudades_SelectedIndexChanged(sender, e)
        'cmbFiscalCiudades.UpdateAfterCallBack = True
    End Sub

    Private Sub cmbArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArea.SelectedIndexChanged
        If Not IsNothing(cmbArea.SelectedItem) Then
            If cmbArea.SelectedItem.Text = PortalCulture.GetString("00821", False) Then 'OTRA_CIUDAD Then
                txtArea.Visible = True
                txtArea.Text = ""
            Else
                txtArea.Visible = False
                txtArea.Text = cmbArea.SelectedItem.Text
            End If
        End If
        'txtArea.AutoUpdateAfterCallBack = True
        'cmbArea.UpdateAfterCallBack = True
    End Sub

    Private Sub cmbCiudades_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCiudades.SelectedIndexChanged
        If Not IsNothing(cmbCiudades.SelectedItem) Then
            If cmbCiudades.SelectedItem.Text = PortalCulture.GetString("00821", False) Then 'OTRA_CIUDAD Then
                txtCiudad.Visible = True
                txtCiudad.Text = ""
            Else
                txtCiudad.Visible = False
                txtCiudad.Text = cmbCiudades.SelectedItem.Text
            End If
            Call Carga_Areas()
            'Call Copia_Ciudad()
            'cmbCiudades.UpdateAfterCallBack = True
            cmbArea_SelectedIndexChanged(sender, e)
            'cmbArea.UpdateAfterCallBack = True
            'txtCiudad.AutoUpdateAfterCallBack = True
            'cmbFiscalEstados.AutoUpdateAfterCallBack = True
            'cmbMunicipioFiscal.AutoUpdateAfterCallBack = True
            'cmbFiscalCiudades.AutoUpdateAfterCallBack = True
        End If
    End Sub

    Private Sub cmbFiscalCiudades_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFiscalCiudades.SelectedIndexChanged
        If Not IsNothing(cmbFiscalCiudades.SelectedItem) Then
            If cmbFiscalCiudades.SelectedItem.Text = PortalCulture.GetString("00821", False) Then 'OTRA_CIUDAD Then
                txtCiudadFiscal.Visible = True
                txtCiudadFiscal.Text = ""
            Else
                txtCiudadFiscal.Visible = False
                txtCiudadFiscal.Text = cmbFiscalCiudades.SelectedItem.Text
            End If
        End If
        'txtCiudadFiscal.AutoUpdateAfterCallBack = True
    End Sub



#End Region

    Private Sub Carga_Idioma()
        lblFiscalDom.Text = PortalCulture.GetString("00822", True) 'Domicilio
        lblInformacion.Text = PortalCulture.GetString("00160", False) 'Información de empresa
        'lblTitulo.Text = PortalCulture.GetString("00823", False) 'Solicitudes de Registro
        If Editing Then
            lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("00823"))
        Else
            lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("00823"))
        End If

        lblNombre.Text = PortalCulture.GetString("00073", True) 'Nombre


        lblDomicilio.Text = PortalCulture.GetString("M000076", True) 'Dirección
        lblPais.Text = PortalCulture.GetString("M000251", True) 'Pais
        lblEstado.Text = PortalCulture.GetString("M000252", True) 'Estado
        lblMunicipio.Text = PortalCulture.GetString("M000253", True) 'Municipio
        lblCiudad.Text = PortalCulture.GetString("M000254", True) 'Ciudad
        Label2.Text = PortalCulture.GetString("00836", True) 'Area
        lblCP.Text = PortalCulture.GetString("M0BT0000164", True) 'Cod. Postal
        lblTel.Text = PortalCulture.GetString("00725", True) 'Teléfono
        lblFax.Text = PortalCulture.GetString("00837", True) 'Fax
        Label8.Text = PortalCulture.GetString("00835", True) 'Pagina web
        lblCategoria.Text = PortalCulture.GetString("M000077", True) 'Categoria
        lblInventario.Text = PortalCulture.GetString("00834", True) 'Num. Cuartos
        lblGerente.Text = PortalCulture.GetString("00833", True) 'Gerente General
        lblTelG.Text = PortalCulture.GetString("00832", True) 'Teléfono
        lblCorreoG.Text = PortalCulture.GetString("00163", True) 'Correo electronico
        lblInformacionFact.Text = PortalCulture.GetString("00831", False) 'Informacón de facturación
        lblRFC.Text = PortalCulture.GetString("00830", True) 'R.F.C
        lblRazonSocial.Text = PortalCulture.GetString("00829", True) 'Razon social
        lblFiscalEstado.Text = PortalCulture.GetString("M000050", True) 'Estado    
        lblMunicipioFiscal.Text = PortalCulture.GetString("00253", True) 'Municipio
        lblFiscalCiudad.Text = PortalCulture.GetString("00254", True) 'Ciudad
        lblFacturacionCP.Text = PortalCulture.GetString("M0BT0000164", True) 'Facturacion Codigo Postal
        lblFacturacionEmpresaFiscal.Text = PortalCulture.GetString("01669", True) 'Facturacion Empresa Fiscal

        lblContactoPuesto.Text = String.Format(PortalCulture.GetString("00826", True), "")
        lblTel1.Text = String.Format(PortalCulture.GetString("00825", True), "")
        lblInformacionContacto.Text = PortalCulture.GetString("00828", False) 'Informacion del contacto
        lblContactoNombre.Text = String.Format(PortalCulture.GetString("00827", True), "")
        lblContactoCorreo.Text = String.Format(PortalCulture.GetString("00824", True), "")
        lblContactoNombre2.Text = String.Format(PortalCulture.GetString("00827", True), "#2")
        lblContactoPuesto2.Text = String.Format(PortalCulture.GetString("00826", True), "#2")
        lblTel2.Text = String.Format(PortalCulture.GetString("00825", True), "#2")
        lblCorreo2.Text = String.Format(PortalCulture.GetString("00824", True), "#2") 'Correo Electrónico
        lblContactoNombre3.Text = String.Format(PortalCulture.GetString("00827", True), "#3") 'Contacto 3
        lblContactoPuesto3.Text = String.Format(PortalCulture.GetString("00826", True), "#3") 'Puesto
        lblTel3.Text = String.Format(PortalCulture.GetString("00825", True), "#3")
        lblCorreo3.Text = String.Format(PortalCulture.GetString("00824", True), "#3") 'Correo Electrónico
        lblChain.Text = PortalCulture.GetString("00838", True)
        lblError.Text = PortalCulture.GetString("00844")
        Me.lblHotelInformation.Text = PortalCulture.GetString("M000075")
        Me.lblMoneda.Text = PortalCulture.GetString("M000263")
        For i As Integer = 0 To 4
            Me.cmbCategoria.Items(i).Text = (i + 1) & " " & PortalCulture.GetString("M0UT00494")    '& "estrellas"
        Next
        Me.cmbCategoria.Items(5).Text = PortalCulture.GetString("00798")
        'rfvNombre.ErrorMessage = PortalCulture.GetString("00251", False) 'El nombre es requerido.
        'rfvNombre.Text = PortalCulture.GetString("00251", False) 'El nombre es requerido.
        'rfvDomicilio.Text = PortalCulture.GetString("00274", False) 'La dirección es requerida.
        ' rfvCiudad.Text = PortalCulture.GetString("00275", False) 'La ciudad de la empresa es requerida
        'rfvTel.Text = PortalCulture.GetString("00276", False) 'El telefono de la empresa es requerido
        'RangeValidator1.Text = PortalCulture.GetString("00277", False) 'Valor de inventario invalido.
        'rfvInventario.Text = PortalCulture.GetString("00278", False) 'El inventario es requerido
        'rfvGerente.Text = PortalCulture.GetString("00279", False) 'El gerente general es requerido.
        'rfvTelG.Text = PortalCulture.GetString("00276", False) 'El telefono de la empresa es requerido
        'rfvContactoTel.Text = PortalCulture.GetString("00276", False)  'El telefono de la empresa es requerido
        'rfvCorreoG.Text = PortalCulture.GetString("00311", False) 'El correo electrónico es requerido.
        'rfvArea.Text = PortalCulture.GetString("00361", False) 'El Area es requerida
        'rfvArea.ErrorMessage = PortalCulture.GetString("00361", False) 'El Area es requerida
        'rfvRFC.Text = PortalCulture.GetString("00280", False) 'El R.F.C es requerido.
        'rfvRazonSocial.Text = PortalCulture.GetString("00281", False) 'La razon social es requerida.
        'rfvDomFiscal.Text = PortalCulture.GetString("00282", False) 'El domicilio fiscal es requerida.
        'rfvFiscalCiudad.Text = PortalCulture.GetString("00284", False) 'La ciudad es requerida
        'rfvContactoNombre.Text = PortalCulture.GetString("00286", False) 'El nombre del contacto es requerido.
        'rfvPuesto.Text = PortalCulture.GetString("00287", False) 'El puesto es requerido.
        'rfvContactoCorreo.Text = PortalCulture.GetString("00288", False) 'Correo electronico no valido.
        'RegularExpressionValidator1.ErrorMessage = PortalCulture.GetString("00289", False) 'El correo electronico es requerido.


    End Sub

    'Private Sub BorraUsuario(ByVal IdUser As Integer, ByVal idempresa As Integer)
    '    Dim sError As String = ""
    '    If (New cUserCompanySystem).DeleteUserCompany(IdUser, idempresa, sError) Then

    '    Else
    '        'no se pudo borrar
    '    End If
    'End Sub
    'Private Sub AsociaUsuario(ByVal email As String, ByVal idempresa As Integer)
    '    Dim idioma As String
    '    If PortalCulture.GetIDCulture = 1 Then
    '        idioma = "es-MX"
    '    Else
    '        idioma = "en-US"
    '    End If
    '    If (New cUserCompanySystem).createUserCompany(idioma, email, idempresa) Then

    '    Else
    '        'ShowError("No se pudo ligar el usuario a esa empresa.")

    '    End If
    'End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Carga_Idioma()
    End Sub

    Public Function Desactiva_Empresa(ByVal idEmpresa As Integer) As Boolean
        Dim companyData As EmpresaDatos = New EmpresaDatos

        With New Empresas
            companyData = .LoadCompanyByID(idEmpresa)

            With companyData.Tables(EmpresaDatos.COMPANY_TABLE)

                If Not .Rows Is Nothing AndAlso .Rows.Count = 1 Then
                    .Rows(0).Item(EmpresaDatos.FIELD_Status) = 1
                End If

            End With

            Return .UpdateStatusCompany(companyData)
        End With

    End Function
End Class
