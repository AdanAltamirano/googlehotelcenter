Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data

Imports Portal.TaskManager.Facade
Imports Portal.General.Rules
Imports System.Configuration.ConfigurationManager
Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos.Facade
Imports System.Data.SqlClient
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data

Public Class RegistroCasas
    Inherits PaginaBase

    Private Enum cancelpolicy
        bydays
        byhour
        specifichour
    End Enum
    Private dsCommand As New SqlDataAdapter
    Private empresasTable As EmpresaDatos
    Private tempChkList() As CheckListObject
    Private chkList() As CheckBoxList
    Private idEmpresa As Integer
    Private idHotel As Integer
    Private idOwnerDesc As Integer
    Private idTipoHabitacion_Hotel As Integer
    Private idRatePlan As Integer

    ' Stored procedure parameter names
    'Insert Empresas
    Private Const PKID_PARM As String = "@Id"
    Private Const RUBRO_PARM As String = "@idRubro"
    Private Const NOMBRE_PARM As String = "@Nombre"
    Private Const DOMICILIO_PARM As String = "@Domicilio"
    Private Const CIUDAD_PARM As String = "@Ciudad"
    Private Const IDCIUDAD_PARM As String = "@idCiudad"
    Private Const ESTADO_PARM As String = "@Estado"
    Private Const MUNICIPIO_PARM As String = "@Municipio"
    Private Const IDPAIS_PARM As String = "@idPais"
    Private Const CP_PARM As String = "@CP"
    Private Const TELEFONO_PARM As String = "@Telefono"
    Private Const FAX_PARM As String = "@Fax"
    Private Const CONTACTONOMBRE_PARM As String = "@Contacto_Nombre"
    Private Const CONTACTOEMAIL_PARM As String = "@Contacto_Email"
    Private Const BILL_PAGINAWEB_PARM As String = "@PaginaWeb"
    Private Const IDCATEGORIA_PARM As String = "@idCategoria"
    Private Const PARM_ContactoTel As String = "@Contacto_Tel"
    'Amenidad
    Private Const AMENIDAD_PARM As String = "@IdAmenidad"
    'Hotel
    Private Const IDEMPRESA_PARM As String = "@idEmpresa"
    Private Const CATEGORIA As String = "@Categoria"
    Private Const CHECKIN As String = "@Checkin"
    Private Const CHECKOUT As String = "@Checkout"
    Private Const MONEDA As String = "@idMoneda"
    Private Const MAXDIASRENTA As String = "@MaxDiasRenta"
    Private Const MAXCUARTOS As String = "@MaxNumCuartos"
    Private Const MAXNUMADULTOS As String = "@MaxNumAdultos"
    Private Const MAXNUMNINOS As String = "@MaxNumNinos"
    Private Const DIASLIBRES As String = "@DiasLibres"
    Private Const DIASMINCANCELAR As String = "@DiasMinCancelar"
    Private Const IVA As String = "@Impuesto"
    Private Const EDADEXTRANINOS As String = "@EdadAPartirPagaExtra"
    Private Const EDADMAXNINOS As String = "@MaxEdadNino"
    Private Const CITYTAX As String = "@CityTax"
    Private Const OCCTAX As String = "@OccupancyTax"
    Private Const CARGO As String = "@ServiceCharge"
    Private Const COMISION As String = "@ComisionAgentes"
    Private Const POLITICA As String = "@PoliticaCancelacion"
    Private Const FAPERTURA As String = "@FechaApertura"
    Private Const FINICIOR As String = "@FechaInicioRes"
    Private Const EMAILRESERVAS As String = "@EmailReservas"
    Private Const PROPERTYNUMBER As String = "@PropertyNumber"
    Private Const GETRATES As String = "@GetRates"
    Private Const CHAINCODE As String = "@ChainCode"
    Private Const PLUSTAX As String = "@PlusTax"
    Private Const IDCORPORATIVO As String = "@idcorporativo"
    Private Const MOROSO As String = "@EsMoroso"
    Private Const PAGO0 As String = "@EsPagoCero"
    Private Const MINNUMCUARTOS As String = "@MinNumCuartos"
    Private Const MINEDADNINO As String = "@MinEdadNinio"
    Private Const MAXOCUPACION As String = "@MaxNumOcupacion"
    Private Const MINOCUPACION As String = "@MinNumOcupacion"
    Private Const ISHOUSE As String = "@IsHouse"
    Private Const AMALLAVES_PARM As String = "@AmaLlaves"
    Private Const BANOS As String = "@Banios"
    Private Const AREA As String = "@Area"
    Private Const AREAUNIDAD As String = "@AreaUnidad"






    Private insertCommand As SqlCommand

    Private Property chkObjects As Object
        Get
            Return ViewState("idDicc")
        End Get
        Set(value As Object)
            ViewState("idDicc") = value
        End Set
    End Property


    Private Property idDicc() As Integer
        Get
            Return ViewState("idDicc")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDicc") = Value
        End Set
    End Property

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        If Not Me.IsPostBack Then
            Me.CtrlPreserveScrolls1.Add(CtrlPreserveScrolls.TypeControl.THEWINDOW)

            ddlCancelationPolicy.Items.Clear()
            ddlCancelationPolicy.Items.Insert(cancelpolicy.bydays, PortalCulture.GetString("00020"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.byhour, PortalCulture.GetString("00021"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.specifichour, PortalCulture.GetString("00381"))
            'loadDepartures()
            Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtDateTo.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
        End If

        Carga_Amenidades()
        Me.txtCancellationPolicy.Style.Add("display", "")
        Me.ddlHour.Style.Add("display", "none")
        Me.lblSep.Style.Add("display", "none")
        Me.ddlMinutes.Style.Add("display", "none")
        If Not IsPostBack And Not Me.Editing Then
            ddlCancelationPolicy.Attributes.Add("onchange", "javascript:LoadMsg('" & Me.ddlCancelationPolicy.ClientID _
             & "','" & lblAux.ClientID & "','" & lblEDaysHour.ClientID & "','" & PortalCulture.GetString("00410") _
            & "','" & PortalCulture.GetString("00409") & "','" & PortalCulture.GetString("00411") _
            & "','" & PortalCulture.GetString("00412") & "','" & PortalCulture.GetString("00413") & "')")

            txtCancelPolitiesReview.IsMultiline = False
            txtCancelPolitiesReview.MaxLength = 52

            Call Carga_Paises()
            Call Carga_Estados()
            Call Carga_Municipios(False)
            cmbCasaMunicipio.DataSource = cmbCasaMunicipio.DataSource
            cmbCasaMunicipio.DataTextField = clsCommonMunicipios.FLD_NOMBRE
            cmbCasaMunicipio.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
            cmbCasaMunicipio.DataBind()

            Call Carga_Ciudades(False)
            cmbCasaCiudades.DataSource = cmbCasaCiudades.DataSource
            cmbCasaCiudades.DataTextField = clsCommonCiudades.FLD_NOMBRE
            cmbCasaCiudades.DataValueField = clsCommonCiudades.FLD_IDCIUDAD
            cmbCasaCiudades.DataBind()

            cmbCasaCiudades.Items.Add(PortalCulture.GetString("M0BT0000335", False))
            cmbCasaCiudades.Items(cmbCasaCiudades.Items.Count - 1).Value = 0

            'If Not (cmbCasaCiudades.SelectedItem Is Nothing) Then
            'If cmbCasaCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
            'txtCiudad.Visible = True
            'txtCiudad.Text = ""
            '        Else
            '           txtCiudad.Visible = False
            '          txtCiudad.Text = cmbCasaCiudades.SelectedItem.Text
            '     End If
            'If cmbCasaCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then ' OTRA_CIUDAD Then
            'txtCiudadCasa.Visible = True
            'txtCiudadCasa.Text = ""
            ' Else
            '    txtCiudadCasa.Visible = False
            'txtCiudadCasa.Text = cmbCasaCiudades.SelectedItem.Text
            'End If
            'End If

            'Call Carga_Areas()
            'If Not IsNothing(cmbCasaArea.SelectedItem) Then
            'If cmbCasaArea.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then 'OTRA_CIUDAD Then
            'txtArea.Visible = True
            'txtArea.Text = ""
            'Else
            '   txtArea.Visible = False
            '  txtArea.Text = cmbCasaArea.SelectedItem.Text
            ' End If
            'End If
            Carga_Monedas()
            'CargaCorporativos()
        End If
        If Not IsPostBack Then
            'CheckOptionEnabled()
            showError(False)
        End If
    End Sub
    Public Sub showError(ByVal sw As Boolean)
        Me.lblError.Visible = sw
    End Sub
    Private Sub Carga_Amenidades()
        Dim ds As New DataSet
        Dim sp As String = "spGetAmenities"
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.SelectCommand = New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        sqlConn.Open()
        trans = sqlConn.BeginTransaction
        With dscommand
            Try
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.CommandText = sp
                .SelectCommand.Connection = sqlConn
                .SelectCommand.Transaction = trans
                With .SelectCommand
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@IdIdioma", Integer.Parse(IdIdiomaMenu))
                End With
                .Fill(ds)
            Catch ex As Exception
                Dim msg As String
                msg = ex.Message
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()

            End Try
        End With
        If sqlConn.State = ConnectionState.Open Then
            trans.Rollback()
            sqlConn.Close()
        End If

        'Hacer el llenado de amenidades

        Dim filteredDs As DataSet = ds.Clone()
        Dim currentCategory As String = ""
        Dim index As Integer = -1

        filteredDs.Tables.Clear()
        'Lleno otro DataSet donde cada tabla va a ser una categoria
        For Each dr As DataRow In ds.Tables(0).Rows
            If dr.Item("Categoria").ToString() <> currentCategory Then
                currentCategory = dr.Item("Categoria").ToString()
                index += 1
                filteredDs.Tables.Add(currentCategory)
                'Add columns to table
                For c As Integer = 0 To ds.Tables(0).Columns.Count - 1
                    filteredDs.Tables(index).Columns.Add(ds.Tables(0).Columns(c).Caption)
                Next
            End If
            filteredDs.Tables(index).ImportRow(dr)
        Next

        If chkList Is Nothing Then

            ReDim Preserve chkList(filteredDs.Tables.Count - 1)
        Else
            ReDim tempChkList(filteredDs.Tables.Count - 1)
        End If


        Dim idx As Integer = 0
        For Each dt As DataTable In filteredDs.Tables
            'Etiqueta
            Dim row As New HtmlGenericControl("tr")
            Dim cblr As New HtmlGenericControl("tr")
            Dim cbld As New HtmlGenericControl("td")
            'Crea el TR
            Dim category As New Label
            Dim td As New HtmlGenericControl("td")
            td.Attributes("Class") = "clsdarklabel"
            'Crea el TD  con la clase para letras del titulo
            category.Text = dt.TableName
            td.Controls.Add(category)               'Le mete el label a la etiqueta
            row.Controls.Add(td)                    'Añade el TD al TR
            amenitiesPanel.Controls.Add(row)        'Añade el TR al Panel
            'amenitiesPanel.Controls.Add(td)
            Dim chkBList As New CheckBoxList
            For Each dr As DataRow In dt.Rows
                'CheckBoxList
                Dim item As New WebControls.ListItem
                item.Text = dr.Item("Amenidad")
                item.Value = dr.Item("IdAmenidad")
                chkBList.Items.Add(item)

                'lblAmenidad1.Text = dr.Item("Amenidad")
            Next
            chkList(idx) = chkBList
            idx += 1             'Añade el CBL al TR
            cbld.Controls.Add(chkBList)
            cblr.Controls.Add(cbld)
            amenitiesPanel.Controls.Add(cblr)        'Añade el TR al Panel
        Next


    End Sub
    Private Sub Carga_Monedas()
        cmbCasaMonedas.DataSource = (New MonedaSistema).GetMonedaListIdName
        cmbCasaMonedas.DataTextField = "Nombre"
        cmbCasaMonedas.DataValueField = "idMoneda"

        'cmbCasaMonedas.SelectedIndex = 1 'Euros
        cmbCasaMonedas.DataBind()

        Dim ds As DataSet = (New MonedaSistema).GetMonedaListIdName



        curr1.Text = ds.Tables(0).Rows(cmbCasaMonedas.SelectedIndex).Item("Codigo")
        Curr2.Text = ds.Tables(0).Rows(cmbCasaMonedas.SelectedIndex).Item("Codigo")
    End Sub
    Private Sub Carga_Categorias()
        'Dim strSQL As String = "SELECT idCategoria,Descripcion FROM Categorias WHERE idRubro=" & Rubro
        Dim strSQL As String = "declare @IdIdioma as Int " & _
                                "set @IdIdioma = " & PortalCulture.GetIDCulture & _
                                " SELECT idCategoriaCasas, " & _
                                "Case " & _
                                "when @IdIdioma = 1 then isnull((select top 1 texto from diccionario where idDiccionario = categorias.idDiccionario and idIdioma = 1),descripcion) " & _
                                "when @IdIdioma = 2 then isnull((select top 1 texto from diccionario where idDiccionario = categorias.idDiccionario and idIdioma = 2),descripcion) " & _
                                "else Descripcion " & _
                                "end as Descripcion  " & _
                                "FROM ozhoteles..CategoriaCasas Categorias"
        Dim ds As New DataSet
        Dim sp As String = "spGetCasasCategorias"
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.SelectCommand = New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        sqlConn.Open()
        trans = sqlConn.BeginTransaction
        With dscommand
            Try
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.CommandText = sp
                .SelectCommand.Connection = sqlConn
                .SelectCommand.Transaction = trans
                With .SelectCommand
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@IdIdioma", Integer.Parse(IdIdiomaMenu))
                End With
                .Fill(ds)
            Catch ex As Exception
                Dim msg As String
                msg = ex.Message
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()

            End Try
        End With
        If sqlConn.State = ConnectionState.Open Then
            trans.Rollback()
            sqlConn.Close()
        End If

        'Dim da As New SqlDataAdapter(, AppSettings("HotelConnectionString"))
        'Dim ds As New DataSet
        'da.Fill(ds)
        ddlCategory.DataSource = ds
        ddlCategory.DataValueField = "idCategoriaCasas"
        ddlCategory.DataTextField = "Descripcion"
        ddlCategory.DataBind()
        'falta la especial

    End Sub
    Private Sub Carga_Unidades()
        'Dim strSQL As String = "SELECT idCategoria,Descripcion FROM Categorias WHERE idRubro=" & Rubro
        ddlUnits.Items.Clear()
        ddlUnits.Items.Add("M2")
        ddlUnits.Items.Add("FT")
        ddlUnits.Items.Add("YD")
        ddlUnits.Items.Add("INCH")
        'ddlUnits.SelectedIndex = 0
        ddlUnits.DataBind()

        '01445 Lista de espera
        'M0UT02720 Inmediato
        ddlTipoPago.Items.Clear()
        ddlTipoPago.Items.Add(PortalCulture.GetString("M0UT02720"))
        ddlTipoPago.Items.Add(PortalCulture.GetString("01445"))
        'ddlTipoPago.SelectedIndex = 0
        ddlTipoPago.DataBind()

    End Sub
    Public Function LoadData(ByVal idempresa As Integer, ByVal sGuid As String, ByVal idhotel As Integer, ByRef status As Byte) As Boolean
        Carga_Categorias()
        Carga_Monedas()
        Carga_Unidades()
        'CargaCorporativos()
        Dim data As EmpresaDatos
        data = (New EmpresaSistema).GetCompanyById(idempresa)
        If Not (data Is Nothing) AndAlso data.Tables(EmpresaDatos.COMPANY_TABLE).Rows.Count > 0 Then
            If data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Guid) = sGuid Then
                Editing = True
                lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("00823"))
                'txtCiudad.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Ciudad)

                txtCasaCorreoG.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoCorreo)
                'txtCasaContactoNombre.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoNombre)
                txtCasaCp.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_CP)
                txtCasaDomicilio.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Domicilio)
                txtCasaFax.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Fax)
                txtCasaNombre.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Nombre)

                txtCasaTel.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Telefono)
                'txtCasaRazonSocial.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_RazonSocial)
                txtCasaDomicilio.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_DomFiscal)

                'txtRFC.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_RFC)
                'txtCiudadCasa.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Ciudad)

                'txtContactoPuesto.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoPuesto)

                'txtArea.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Area)
                'txtContactoTel.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoTel)
                'txtContacto2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2Nombre)
                'txtTel2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2Tel)
                'txtPuesto2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2puesto)
                'txtCorreo2.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto2Correo)
                'txtContacto3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3Nombre)
                'txtTel3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3Tel)
                'txtPuesto3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3puesto)
                'txtCorreo3.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Contacto3Correo)
                'txtGerente.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoGteNombre)
                txtCasaTelG.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoGteTel)
                txtCasaCorreoG.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_ContactoGteCorreo)
                txtCasaPaginaWeb.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_PAGINAWEB)

                'Call Carga_Categorias(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Rubro))
                If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IDCATEGORIA)) Then
                    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IDCATEGORIA)))
                End If


                Call Carga_Paises()
                cmbCasaPaises.SelectedIndex = cmbCasaPaises.Items.IndexOf(cmbCasaPaises.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_IdPais)))

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
                        cmbCasaEstados.SelectedIndex = cmbCasaEstados.Items.IndexOf(cmbCasaEstados.Items.FindByValue(estado))

                        Call Carga_Municipios(False)
                        cmbCasaMunicipio.SelectedIndex = cmbCasaMunicipio.Items.IndexOf(cmbCasaMunicipio.Items.FindByValue(municipio))

                        Call Carga_Ciudades(False)
                        cmbCasaCiudades.SelectedIndex = cmbCasaCiudades.Items.IndexOf(cmbCasaCiudades.Items.FindByValue(ciudad))
                        'If Not (cmbCasaCiudades.SelectedItem Is Nothing) Then
                        '    If cmbCasaCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                        '        txtCiudad.Visible = True                        '     txtCiudad.Text = ""
                        '    Else
                        '        txtCiudad.Visible = False                        '      txtCiudad.Text = cmbCasaCiudades.SelectedItem.Text
                        '    End If
                        'End If
                    End If
                Else
                    'cmbCasaEstados.Items.Clear()
                    Call Carga_Estados()
                    If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Estado)) Then
                        cmbCasaEstados.SelectedIndex = cmbCasaEstados.Items.IndexOf(cmbCasaEstados.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Estado)))
                    End If
                    'cmbCasaMunicipio.Items.Clear()
                    Call Carga_Municipios(False)
                    If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Municipio)) Then
                        cmbCasaMunicipio.SelectedIndex = cmbCasaMunicipio.Items.IndexOf(cmbCasaMunicipio.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Municipio)))
                    End If

                    Call Carga_Ciudades(False)
                    cmbCasaCiudades.SelectedIndex = cmbCasaCiudades.Items.Count - 1
                    'If Not (cmbCasaCiudades.SelectedItem Is Nothing) Then
                    '    If cmbCasaCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                    '        txtCiudad.Visible = True                        '     txtCiudad.Text = ""
                    '    Else
                    '        txtCiudad.Visible = False                        '      txtCiudad.Text = cmbCasaCiudades.SelectedItem.Text
                    '    End If
                    'End If
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
                            cmbCasaEstados.SelectedIndex = cmbCasaEstados.Items.IndexOf(cmbCasaEstados.Items.FindByValue(estado))

                            Call Carga_Municipios(True)
                            cmbCasaMunicipio.SelectedIndex = cmbCasaMunicipio.Items.IndexOf(cmbCasaMunicipio.Items.FindByValue(municipio))

                            Call Carga_Ciudades(True)
                            cmbCasaCiudades.SelectedIndex = cmbCasaCiudades.Items.IndexOf(cmbCasaCiudades.Items.FindByValue(ciudad))
                            'If Not (cmbCasaCiudades.SelectedItem Is Nothing) Then
                            '    If cmbCasaCiudades.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                            '        txtCiudadCasa.Visible = True                       '     txtCiudad.Text = ""
                            '    Else
                            '        txtCiudadCasa.Visible = False                        '      txtCiudad.Text = cmbCasaCiudades.SelectedItem.Text
                            '    End If
                            'End If
                        Else
                            'No existe la ciudad
                            Call Carga_Municipios(True)
                            Call Carga_Ciudades(True)


                        End If
                    Else

                        If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Estado)) Then
                            cmbCasaEstados.SelectedIndex = cmbCasaEstados.Items.IndexOf(cmbCasaEstados.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Estado)))
                        End If

                        Call Carga_Municipios(True)
                        If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Municipio)) Then
                            cmbCasaMunicipio.SelectedIndex = cmbCasaMunicipio.Items.IndexOf(cmbCasaMunicipio.Items.FindByText(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_BILL_Municipio)))
                        End If

                        '---------------------------------------------
                        Call Carga_Ciudades(True)
                        cmbCasaCiudades.SelectedIndex = cmbCasaCiudades.Items.Count - 1

                    End If
                End If
                'Call Carga_Areas()
                'If Not IsDBNull(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_idArea)) Then
                ' cmbCasaArea.SelectedIndex = cmbCasaArea.Items.IndexOf(cmbCasaArea.Items.FindByValue(data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_idArea)))
                'Else
                '    cmbCasaArea.SelectedIndex = cmbCasaArea.Items.IndexOf(cmbCasaArea.Items.FindByValue(0))
                'End If
                'If txtArea.Text.Trim = "" Then
                ' If Not IsNothing(cmbCasaArea.SelectedItem) Then txtArea.Text = cmbCasaArea.SelectedItem.Text
                'End If
                'If Not IsNothing(cmbCasaArea.SelectedItem) Then
                ' If cmbCasaArea.SelectedItem.Text = PortalCulture.GetString("M0BT0000335", False) Then
                ' txtArea.Visible = True
                'Else
                '   txtArea.Visible = False
                'End If
            End If
            'txtInventario.Text = "" & data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_INVENTARIO)
            'Try
            'cmbCasaAdmin.SelectedValue = Load_RelatedContentAdmin(idempresa)
            'Catch
            'cmbCasaAdmin.SelectedValue = 0
            'End Try
            status = data.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Status)
            loadHotelData(idhotel)
            Return True


        End If
    End Function

    Private Sub Carga_Paises()
        Dim strErr As String
        cmbCasaPaises.DataSource = (New clsFacadePaises).GetPaises(PortalCulture.GetIDCulture())
        cmbCasaPaises.DataTextField = clsCommonPaises.FLD_NOMBRE
        cmbCasaPaises.DataValueField = clsCommonPaises.FLD_IDPAIS
        cmbCasaPaises.DataBind()
        cmbCasaPaises.SelectedIndex = cmbCasaPaises.Items.IndexOf(cmbCasaPaises.Items.FindByValue("CU"))
    End Sub
    Private Sub Carga_Estados()
        If Not (cmbCasaPaises.SelectedItem Is Nothing) Then
            Dim strErr As String
            cmbCasaEstados.DataSource = (New clsFacadeEstados).GetByPais(cmbCasaPaises.SelectedValue, strErr)
            cmbCasaEstados.DataTextField = clsCommonEstados.FLD_NOMBRE
            cmbCasaEstados.DataValueField = clsCommonEstados.FLD_IDESTADO
            cmbCasaEstados.DataBind()
            cmbCasaEstados.DataSource = cmbCasaEstados.DataSource
            cmbCasaEstados.DataTextField = clsCommonEstados.FLD_NOMBRE
            cmbCasaEstados.DataValueField = clsCommonEstados.FLD_IDESTADO
            cmbCasaEstados.DataBind()
        Else
            cmbCasaEstados.Items.Clear()
            'cmbCasaEstados.Items.Clear()
        End If
    End Sub

    Private Sub Carga_Municipios(ByVal Fiscal As Boolean)
        Dim strErr As String
        If Fiscal Then
            If Not (cmbCasaEstados.SelectedItem Is Nothing) Then
                Dim combo As DropDownList
                cmbCasaMunicipio.DataSource = (New clsFacadeMunicipios).GetByIdEstado(cmbCasaEstados.SelectedValue, strErr)
                cmbCasaMunicipio.DataTextField = clsCommonMunicipios.FLD_NOMBRE
                cmbCasaMunicipio.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
                cmbCasaMunicipio.DataBind()
            Else
                cmbCasaMunicipio.Items.Clear()
            End If
        Else
            If Not (cmbCasaEstados.SelectedItem Is Nothing) Then
                cmbCasaMunicipio.DataSource = (New clsFacadeMunicipios).GetByIdEstado(cmbCasaEstados.SelectedValue, strErr)
                cmbCasaMunicipio.DataTextField = clsCommonMunicipios.FLD_NOMBRE
                cmbCasaMunicipio.DataValueField = clsCommonMunicipios.FLD_IDMUNICIPIO
                cmbCasaMunicipio.DataBind()
            Else
                cmbCasaMunicipio.Items.Clear()
            End If
        End If
    End Sub
    Private Sub Carga_Ciudades(ByVal Fiscal As Boolean)
        If Not Fiscal Then
            If Not (cmbCasaMunicipio.SelectedItem Is Nothing) Then
                Dim combo As DropDownList
                Dim strErr As String

                combo = cmbCasaCiudades
                combo.DataSource = (New clsFacadeCiudades).GetByIdMunicipio(cmbCasaMunicipio.SelectedValue, strErr)

                combo.DataTextField = clsCommonCiudades.FLD_NOMBRE
                combo.DataValueField = clsCommonCiudades.FLD_IDCIUDAD
                combo.DataBind()
            Else
                cmbCasaCiudades.Items.Clear()

            End If
            cmbCasaCiudades.Items.Add(PortalCulture.GetString("00821", False))
            cmbCasaCiudades.Items(cmbCasaCiudades.Items.Count - 1).Value = 0
        Else
            If Not (cmbCasaMunicipio.SelectedItem Is Nothing) Then
                Dim combo As DropDownList
                Dim strErr As String

                combo = cmbCasaCiudades
                combo.DataSource = (New clsFacadeCiudades).GetByIdMunicipio(cmbCasaMunicipio.SelectedValue, strErr)

                combo.DataTextField = clsCommonCiudades.FLD_NOMBRE
                combo.DataValueField = clsCommonCiudades.FLD_IDCIUDAD
                combo.DataBind()
            Else
                cmbCasaCiudades.Items.Clear()
            End If
            cmbCasaCiudades.Items.Add(PortalCulture.GetString("00821", False))
            cmbCasaCiudades.Items(cmbCasaCiudades.Items.Count - 1).Value = 0
        End If
    End Sub
    'Private Sub Carga_Areas()
    '   If cmbCasaCiudades.Items.Count > 0 Then
    'Dim cadError As String
    '       cmbCasaArea.DataTextField = clsCommonAreas.FLD_NOMBRE
    '      cmbCasaArea.DataValueField = clsCommonAreas.FLD_IDAREA
    '     cmbCasaArea.DataSource = (New clsFacadeAreas).GetByIdCiudad(cmbCasaCiudades.SelectedValue, cadError)
    '    cmbCasaArea.DataBind()
    '   cmbCasaArea.Items.Add("--")
    '  cmbCasaArea.Items(cmbCasaArea.Items.Count - 1).Value = -1
    ' cmbCasaArea.Items.Add(PortalCulture.GetString("00821", False))
    'cmbCasaArea.Items(cmbCasaArea.Items.Count - 1).Value = 0
    ' End If
    'End Sub

    Private Sub Copia_Ciudad()
        cmbCasaEstados.SelectedIndex = cmbCasaEstados.SelectedIndex
        cmbCasaEstados_SelectedIndexChanged(New System.Object, New System.EventArgs)

        cmbCasaMunicipio.SelectedIndex = cmbCasaMunicipio.SelectedIndex
        cmbCasaMunicipio_SelectedIndexChanged(New System.Object, New System.EventArgs)

        cmbCasaCiudades.SelectedIndex = cmbCasaCiudades.SelectedIndex

    End Sub
#Region "Eventos"
    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        Server.ClearError()
    End Sub
    Private Sub cmbCasaPaises_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCasaPaises.SelectedIndexChanged
        Call Carga_Estados()
        'cmbCasaPaises.UpdateAfterCallBack = True
        cmbCasaEstados_SelectedIndexChanged(sender, e)
        cmbCasaEstados_SelectedIndexChanged(sender, e)
        'cmbCasaEstados.UpdateAfterCallBack = True
        'cmbCasaEstados.UpdateAfterCallBack = True
    End Sub
    Private Sub cmbCasaMonedas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCasaMonedas.SelectedIndexChanged
        Dim ds As DataSet = (New MonedaSistema).GetMonedaListIdName

        curr1.Text = ds.Tables(0).Rows(cmbCasaMonedas.SelectedIndex).Item("Codigo")
        Curr2.Text = ds.Tables(0).Rows(cmbCasaMonedas.SelectedIndex).Item("Codigo")
    End Sub



    Private Sub cmbCasaEstados_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCasaEstados.SelectedIndexChanged
        Call Carga_Municipios(True)
        'cmbCasaMunicipio.UpdateAfterCallBack = True

        cmbCasaMunicipio_SelectedIndexChanged(sender, e)

    End Sub



    Private Sub cmbCasaMunicipio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCasaMunicipio.SelectedIndexChanged
        Call Carga_Ciudades(True)
        'cmbCasaCiudades.UpdateAfterCallBack = True
    End Sub


    'Private Sub cmbCasaArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCasaArea.SelectedIndexChanged
    '    If Not IsNothing(cmbCasaArea.SelectedItem) Then
    '       If cmbCasaArea.SelectedItem.Text = PortalCulture.GetString("00821", False) Then 'OTRA_CIUDAD Then
    '          txtArea.Visible = True
    '         txtArea.Text = ""
    '    Else
    '       txtArea.Visible = False
    '      txtArea.Text = cmbCasaArea.SelectedItem.Text
    ' End If
    'End If
    'txtArea.AutoUpdateAfterCallBack = True
    'cmbCasaArea.UpdateAfterCallBack = True
    'End Sub



#End Region

    Private Sub Carga_Idioma()
        'Faltan cosas de idiomas
        Carga_Categorias()
        Carga_Unidades()

        'lblCasaDomicilio.Text = PortalCulture.GetString("00822", True) 'Domicilio Fiscal
        'lblInformacion.Text = PortalCulture.GetString("00160", False) 'Información de empresa
        'lblTitulo.Text = PortalCulture.GetString("00823", False) 'Solicitudes de Registro
        If Editing Then
            lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("M0UT02718"))
        Else
            lblTitulo.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("M0UT02718"))
        End If

        'M0UT02719 Tipo de Pago
        lblTipoPago.Text = PortalCulture.GetString("M0UT02719", True)
        
        'M0UT02723 Ama de llaves
        lblAmaLlaves.Text = PortalCulture.GetString("M0UT02723", True)
        'M0UT02721 Baños
        lblBanios.Text = PortalCulture.GetString("M0UT02721", True)

        'A00729 Informacion propiedad
        lblCasaTitulo.Text = PortalCulture.GetString("A00729", True)
        'A00691 Informacion de Amenidades
        lblAmenidades.Text = PortalCulture.GetString("A00691", True)

        'M0UT02722 Tarifa de la propiedad
        lblRates.Text = PortalCulture.GetString("M0UT02722", True)
        '00060 Adultos
        lblAdult.Text = PortalCulture.GetString("00060", True)
        '00061 Ninos
        lblChild.Text = PortalCulture.GetString("00061", True)

        '00618 Info Reservacion
        lblSecond.Text = PortalCulture.GetString("00618", True)
        '00519 Url Pagina Web
        lblCasaPaginaWeb.Text = PortalCulture.GetString("00519", True)
        'M0UT02724 Acerca del propietaro
        lblPropietarioDesc.Text = PortalCulture.GetString("M0UT02724", True)
        'M0UT02725 Descripcion propiedad
        lblCasaDesc.Text = PortalCulture.GetString("M0UT02725", True)
        '00108 Desde
        lblStartDate.Text = PortalCulture.GetString("00108", True)
        '00109 Hasta
        lblEndDate.Text = PortalCulture.GetString("00109", True)


        lblNombreContacto.Text = PortalCulture.GetString("00162", True)
        lblCasaNombre.Text = PortalCulture.GetString("00073", True) 'Nombre
        lblCasaDomicilio.Text = PortalCulture.GetString("M000076", True) 'Dirección
        lblCasaPais.Text = PortalCulture.GetString("M000251", True) 'Pais
        lblCasaEstado.Text = PortalCulture.GetString("M000252", True) 'Estado
        lblCasaMunicipio.Text = PortalCulture.GetString("M000253", True) 'Municipio
        lblCasaCiudad.Text = PortalCulture.GetString("M000254", True) 'Ciudad
        'Label2.Text = PortalCulture.GetString("00836", True) 'Area
        lblCasaCp.Text = PortalCulture.GetString("M0BT0000164", True) 'Cod. Postal
        lblCasaTel.Text = PortalCulture.GetString("00725", True) 'Teléfono
        lblCasaFax.Text = PortalCulture.GetString("00837", True) 'Fax
        'Label8.Text = PortalCulture.GetString("00835", True) 'Pagina web
        lblCategory.Text = PortalCulture.GetString("M000077", True) 'Categoria
        'lblCasaInventario.Text = PortalCulture.GetString("00834", True) 'Num. Cuartos
        'lblCasaGerente.Text = PortalCulture.GetString("00833", True) 'Gerente General
        lblCasaTelG.Text = PortalCulture.GetString("01640", True) 'Teléfono
        lblCasaCorreoG.Text = PortalCulture.GetString("00163", True) 'Correo electronico
        'lblCasaInformacionFact.Text = PortalCulture.GetString("00831", False) 'Informacón de facturación
        'lblCasaRFC.Text = PortalCulture.GetString("00830", True) 'R.F.C
        'lblCasaRazonSocial.Text = PortalCulture.GetString("00829", True) 'Razon social
        lblCasaEstado.Text = PortalCulture.GetString("M000050", True) 'Estado    
        lblCasaMunicipio.Text = PortalCulture.GetString("00253", True) 'Municipio
        lblCasaCiudad.Text = PortalCulture.GetString("00254", True) 'Ciudad

        'lblCasaContactoPuesto.Text = String.Format(PortalCulture.GetString("00826", True), "")
        'lblTel1.Text = String.Format(PortalCulture.GetString("00825", True), "")
        'lblInformacionContacto.Text = PortalCulture.GetString("00828", False) 'Informacion del contacto
        'lblContactoNombre.Text = String.Format(PortalCulture.GetString("00827", True), "")
        'lblContactoCorreo.Text = String.Format(PortalCulture.GetString("00824", True), "")
        'lblContactoNombre2.Text = String.Format(PortalCulture.GetString("00827", True), "#2")
        'lblContactoPuesto2.Text = String.Format(PortalCulture.GetString("00826", True), "#2")
        'lblTel2.Text = String.Format(PortalCulture.GetString("00825", True), "#2")
        'lblCorreo2.Text = String.Format(PortalCulture.GetString("00824", True), "#2") 'Correo Electrónico
        'lblContactoNombre3.Text = String.Format(PortalCulture.GetString("00827", True), "#3") 'Contacto 3
        'lblContactoPuesto3.Text = String.Format(PortalCulture.GetString("00826", True), "#3") 'Puesto
        'lblTel3.Text = String.Format(PortalCulture.GetString("00825", True), "#3")
        'lblCorreo3.Text = String.Format(PortalCulture.GetString("00824", True), "#3") 'Correo Electrónico
        'lblChain.Text = PortalCulture.GetString("00838", True)
        lblError.Text = PortalCulture.GetString("00844")
        'Me.lblHotelInformation.Text = PortalCulture.GetString("M000075")
        Me.lblCasaMoneda.Text = PortalCulture.GetString("M000263")
        'For i As Integer = 0 To ddlCategory.Items.Count - 1
        'Me.ddlCategory.Items(i).Text = "Casa Tipo " & i.ToString()
        'Next
        Me.ddlCategory.Items.Add(PortalCulture.GetString("00821")) 'Otro

        Me.lblCancel.Text = PortalCulture.GetString("00440", True)
        lblRules.Text = PortalCulture.GetString("M000589")
        'Me.lblEmailLanguage.Text = PortalCulture.GetString("M000658", True)
        'Me.lblMoneda.Text = PortalCulture.GetString("M0UT00477", True)
        'lblTitle.Text = PortalCulture.GetString("00434")
        Me.lblMaxDiasRenta.Text = PortalCulture.GetString("M000588", True)
        Me.lblPlusTax.Text = PortalCulture.GetString("M000522", True)
        Me.lblImpuesto.Text = PortalCulture.GetString("M0UT00483") & " %:"    '"Impuesto"
        Me.lblEdadMaximaNiño.Text = PortalCulture.GetString("01322") & " < "
        Me.lblDiasAnticipados.Text = PortalCulture.GetString("00396", True)
        Me.lblDays.Text = PortalCulture.GetString("00397")
        Me.lblCheckout.Text = PortalCulture.GetString("M0UT00487", True)
        Me.lblCheckin.Text = PortalCulture.GetString("M0UT00488", True)
        'Me.lblCategoria.Text = PortalCulture.GetString("M0UT00006", True)
        'Me.lblAvlOnCorpModule.Text = PortalCulture.GetString("01618", True)
        'Me.cmbCategoria.Items(0).Text = "1 " & PortalCulture.GetString("M0UT00493")    '"estrella"
        'For i As Integer = 1 To 4
        '    Me.cmbCategoria.Items(i).Text = (i + 1) & " " & PortalCulture.GetString("M0UT00494")    '& "estrellas"
        'Next
        ' Me.cmbCategoria.Items(5).Text = PortalCulture.GetString("00798")

        'lblServiceCharge.Text = PortalCulture.GetString("M0UT02696", True)
        'lblCommision.Text = PortalCulture.GetString("M0UT02697") & " %:"

        lblNoArrivals.Text = PortalCulture.GetString("M000449")
        'RequiredFieldValidator11.Text = PortalCulture.GetString("M0UT02696") & " " & PortalCulture.GetString("M0UT02715")

        'RequiredFieldValidator12.Text = PortalCulture.GetString("M0UT02697") & " " & PortalCulture.GetString("M0UT02715")
        Rangevalidator2.Text = "" '"Edad min. de Niño es numerico (1-99)"
        RangeValidator7.Text = PortalCulture.GetString("01168")    '"Edad Max. de Niño es numerico (1-99)"
        RequiredFieldValidator8.Text = PortalCulture.GetString("M0UT00502")    '"Impuesto es requerido"
        RangeValidator9.Text = PortalCulture.GetString("M0UT00503")    '"Impuesto es numerico (1-99)"
        'Me.lblConfigGDS.Text = PortalCulture.GetString("M000288")
        'Me.lblPropertyNumber.Text = PortalCulture.GetString("M000289", True)
        'Me.lblChainCode.Text = PortalCulture.GetString("M000290", True)
        'Me.lblGetRates.Text = PortalCulture.GetString("00542", True)
        'Me.rbGetRates_True.Text = PortalCulture.GetString("M000159")
        'Me.rbGetRates_False.Text = PortalCulture.GetString("M000160")
        ''***
        'Me.lblEmail.Text = PortalCulture.GetString("M000292", True)
        Me.lblLunes.Text = PortalCulture.GetString("M000300")
        Me.lblMartes.Text = PortalCulture.GetString("M000301")
        Me.lblMiercoles.Text = PortalCulture.GetString("M000302")
        Me.lblJueves.Text = PortalCulture.GetString("M000303")
        Me.lblViernes.Text = PortalCulture.GetString("M000304")
        Me.lblSabado.Text = PortalCulture.GetString("M000305")
        Me.lblDomingo.Text = PortalCulture.GetString("M000306")
        Me.lblEstanciaMin.Text = PortalCulture.GetString("M000587", True)
        Me.lblStatusA.Text = PortalCulture.GetString("M000308", True)
        Me.ddlStatus.Items(0).Text = (PortalCulture.GetString("M000309"))
        Me.ddlStatus.Items(1).Text = (PortalCulture.GetString("M000310"))
        Me.ddlStatus.Items(2).Text = (PortalCulture.GetString("M000311"))
        Me.lblError.Text = PortalCulture.GetString("M000314")
        'Me.lblErrorDate.Text = PortalCulture.GetString("M000315")
        lblCancelPolitiesFull.Text = PortalCulture.GetString("M000536", True)
        lblCancelPolitiesReview.Text = PortalCulture.GetString("M000527", True)
        'lblGuarantyPolicies.Text = PortalCulture.GetString("M000528", True)
        'lblCreditCardPolicies.Text = PortalCulture.GetString("M000529", True)
        'lblExtraCharges.Text = PortalCulture.GetString("M000530", True)
        'lblIdGal.Text = PortalCulture.GetString("M000532", True)
        'lblIdSabre.Text = PortalCulture.GetString("M000533", True)
        'lblIdWorldSpan.Text = PortalCulture.GetString("M000534", True)
        'lblIdAmadeus.Text = PortalCulture.GetString("M000535", True)
        'Me.lblErrorPWorld.Text = PortalCulture.GetString("M000537")
        'Me.lblErrorPSabre.Text = PortalCulture.GetString("M000537")
        'Me.lblErrorPGalileo.Text = PortalCulture.GetString("M000537")
        'Me.lblErrorPAmadeus.Text = PortalCulture.GetString("M000537")
        'btnSave.Text = PortalCulture.GetString("M000106")
        ddlCancelationPolicy.Items(cancelpolicy.bydays).Text = PortalCulture.GetString("00020")
        ddlCancelationPolicy.Items(cancelpolicy.byhour).Text = PortalCulture.GetString("00021")
        ddlCancelationPolicy.Items(cancelpolicy.specifichour).Text = PortalCulture.GetString("00381")
        'lblConfirmationEmail.Text = PortalCulture.GetString("00414")
        lbltitlepolity.Text = PortalCulture.GetString("00446")
        'lblMaxCuartos.Text = PortalCulture.GetString("00493", True)
        'lblErrorMail.Text = PortalCulture.GetString("00356")
        'Me.lblFaxEmail.Text = PortalCulture.GetString("00494", True)
        'Me.lblPerfil.Text = PortalCulture.GetString("00495", True)
        'Me.lblAvailOnPortal.Text = PortalCulture.GetString("00526", True)
        'Me.lblAvailOnOnePage.Text = PortalCulture.GetString("00527", True)
        'Me.lblAvailOnGDS.Text = PortalCulture.GetString("00525", True)
        'Me.lblAvailOnADS.Text = PortalCulture.GetString("01013", True)
        'Me.lblAmhm.Text = PortalCulture.GetString("00676", True)
        'lblEmprTour.Text = PortalCulture.GetString("00677", True)
        'Me.lblAllowDeposit.Text = PortalCulture.GetString("00681", True)
        'lblCadena.Text = PortalCulture.GetString("00838", True)
        'lblEsmoroso.Text = PortalCulture.GetString("01017", True)
        'lblEsPagoCero.Text = PortalCulture.GetString("01160", True)

        lblEdadNinio.Text = PortalCulture.GetString("01324")
        lblJuniorAnios.Text = PortalCulture.GetString("01324")
        lblNoCobrarAnios.Text = PortalCulture.GetString("01324")
        lblEdadMinimaNiño.Text = PortalCulture.GetString("01323") & " < "
        'lblMinCuartos.Text = PortalCulture.GetString("01165", True)
        'lblMinOcupacion.Text = PortalCulture.GetString("01166", True)
        lblMaxPeople.Text = PortalCulture.GetString("01167", True)
        'Me.chkTransUp.Text = PortalCulture.GetString("01180")
        lblEdadMaximaAdo.Text = PortalCulture.GetString("01277") & " < "
        rvEdadaAdolecente.Text = PortalCulture.GetString("01278")
        rfvEdadNinio.Text = PortalCulture.GetString("00071")
        lblErrorEdadNoCobrar.Text = PortalCulture.GetString("01331")
        lblErrorEdadNinio.Text = PortalCulture.GetString("01332")
        'Me.btnPublsh.Text = PortalCulture.GetString("01364")
        lblLatitud.Text = PortalCulture.GetString("01452", True)
        lblLongitud.Text = PortalCulture.GetString("01453", False)
        'lblSaveCurrencyShonw.Text = PortalCulture.GetString("01648", True)


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
    Private Function loadHotelData(ByVal idhotel As Integer) As Boolean
        Dim dsHotel As New HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(idhotel)
        End With
        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                If Not .IsNull(HotelDatos.FIELD_CATEGORIA) Then
                    ddlCategory.SelectedValue = .Item(HotelDatos.FIELD_CATEGORIA)
                End If
                'If Not .IsNull(HotelDatos.FIELD_IDCIUDAD) Then
                '    cmbCiudades.SelectedValue = .Item(HotelDatos.FIELD_IDCIUDAD)
                'End If


                If Not .IsNull(HotelDatos.FIELD_IDMONEDA) Then
                    cmbCasaMonedas.SelectedValue = .Item(HotelDatos.FIELD_IDMONEDA)
                End If
            End With
        End If
    End Function
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Carga_Idioma()

    End Sub



    Private Sub cmdAceptarClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAceptar.Click
        dsCommand.InsertCommand = InsertDataEmpresa()

        Dim OzHoteles As String = ConfigurationSettings.AppSettings("HotelConnectionString")
        Dim log As String = ""
        Dim connection As SqlConnection
        Dim trans As SqlTransaction
        With dsCommand
            Try
                connection = .InsertCommand.Connection
                connection.Open()
                trans = connection.BeginTransaction()
                .InsertCommand.Transaction = trans
                log &= "Empieza transacción, "
                'Insert de empresas
                idEmpresa = .InsertCommand.ExecuteScalar()
                log &= " Inserta datos de la empresa, "
                'idEmpresa = 19544
                InsertDataEmpresaAmenidades(.InsertCommand)
                log &= " Inserta amenidades de la empresa, "

                'Ya pasó el registro de empresas
                trans.Commit()
                log &= " hace el commit. "

                ChangeConnection(.InsertCommand, OzHoteles, trans)
                log = "Cambia la conexión"

                idOwnerDesc = ctrlIdiomaPropietario.Insert()
                log = "Inserta en el diccionario, "
                InsertDataHoteles(.InsertCommand)
                log &= " Inserta los datos del Hotel, "

                InsertDataRoom(.InsertCommand)
                log &= " Inserta los datos de la habitacion, "

                InsertDataRatePlan(.InsertCommand)
                log &= " Inserta los datos del rateplan, "

                InsertDataRate(.InsertCommand)
                log &= " Inserta los datos de la tarifa, "
                trans.Commit()
                log &= " Hace el commit."


            Catch ex As Exception
                trans.Rollback()
                Me.guardalog("RegistroCasas", acciones.Crear, "Error al registrar propiedad, " & log & "mensaje: " & ex.Message)
            Finally
                trans.Dispose()
                .InsertCommand.Connection.Close()
                .InsertCommand.Connection.Dispose()
                .InsertCommand.Dispose()
                .Dispose()
            End Try
            
            Response.Redirect("../Portal/Pages/Welcome.aspx")
        End With
    End Sub

    Private Sub cmdCancelarClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click

        Response.Redirect("../Portal/Pages/Welcome.aspx")
    End Sub

    Private Function loadDepartures()
        L.Checked = True
        Ma.Checked = True
        Mi.Checked = True
        J.Checked = True
        V.Checked = True
        S.Checked = True
        D.Checked = True
    End Function
    Private Function saveDeparture() As String
        Dim DepRes As String
        DepRes = ""
        If L.Checked Then
            DepRes = "Y"
        Else
            DepRes = "N"
        End If
        If Ma.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If Mi.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If J.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If V.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If S.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        If D.Checked Then
            DepRes += "Y"
        Else
            DepRes += "N"
        End If
        Return DepRes
    End Function

    Private Function InsertDataEmpresa() As SqlCommand
        If insertCommand Is Nothing Then
            insertCommand = New SqlCommand("spCasaEmpresaInsert", New SqlConnection(ConfigurationSettings.AppSettings("portalconnectionstring")))



            insertCommand.CommandType = CommandType.StoredProcedure

            With insertCommand.Parameters
                .Add(New SqlParameter(RUBRO_PARM, SqlDbType.Int))
                .Add(New SqlParameter(NOMBRE_PARM, SqlDbType.NVarChar, 180))
                .Add(New SqlParameter(DOMICILIO_PARM, SqlDbType.NVarChar, 120))
                .Add(New SqlParameter(CIUDAD_PARM, SqlDbType.NVarChar, 30))
                .Add(New SqlParameter(IDCIUDAD_PARM, SqlDbType.Int))
                .Add(New SqlParameter(ESTADO_PARM, SqlDbType.NVarChar, 30))
                .Add(New SqlParameter(MUNICIPIO_PARM, SqlDbType.NVarChar, 30))
                .Add(New SqlParameter(IDPAIS_PARM, SqlDbType.Char, 2))
                .Add(New SqlParameter(CP_PARM, SqlDbType.NVarChar, 11))
                .Add(New SqlParameter(TELEFONO_PARM, SqlDbType.NVarChar, 20))
                .Add(New SqlParameter(FAX_PARM, SqlDbType.NVarChar, 20))
                .Add(New SqlParameter(CONTACTOEMAIL_PARM, SqlDbType.NVarChar, 80))
                .Add(New SqlParameter(BILL_PAGINAWEB_PARM, SqlDbType.NVarChar, 80))
                .Add(New SqlParameter(IDCATEGORIA_PARM, SqlDbType.Int))
                .Add(New SqlParameter(PARM_ContactoTel, SqlDbType.NVarChar, 20))
                .Add(New SqlParameter("@Contacto_Nombre", SqlDbType.NVarChar, 80))


                .Item(RUBRO_PARM).Value = 10
                .Item(NOMBRE_PARM).Value = txtCasaNombre.Text
                .Item(DOMICILIO_PARM).Value = txtCasaDomicilio.Text
                .Item(CIUDAD_PARM).Value = cmbCasaCiudades.SelectedItem.Text
                .Item(IDCIUDAD_PARM).Value = Integer.Parse(cmbCasaCiudades.SelectedValue)
                .Item(ESTADO_PARM).Value = cmbCasaEstados.SelectedItem.Text
                .Item(MUNICIPIO_PARM).Value = cmbCasaMunicipio.SelectedItem.Text
                .Item(IDPAIS_PARM).Value = cmbCasaPaises.SelectedValue
                .Item(CP_PARM).Value = txtCasaCp.Text
                .Item(TELEFONO_PARM).Value = txtCasaTel.Text
                .Item(FAX_PARM).Value = txtCasaFax.Text
                .Item(CONTACTOEMAIL_PARM).Value = txtCasaCorreoG.Text
                .Item(BILL_PAGINAWEB_PARM).Value = txtCasaPaginaWeb.Text
                .Item(IDCATEGORIA_PARM).Value = ddlCategory.SelectedIndex + 1
                .Item(PARM_ContactoTel).Value = txtCasaTelG.Text

                'Valores default porque no permite null
                .Item("@Contacto_Nombre").Value = txtNombreContacto.Text

                .AddWithValue("@Status", 0)
                .AddWithValue("@fecha", DateTime.Now)
                .AddWithValue("@GUID_Registro", Guid.NewGuid.ToString)
                .AddWithValue("@Bill_RazonSocial", "Renta de casa")
                .AddWithValue("@Bill_DomFiscal", txtCasaDomicilio.Text)
                .AddWithValue("@RFC", "RFCCUBA")

                .AddWithValue("@Bill_Ciudad", cmbCasaCiudades.SelectedItem.Text)
                .AddWithValue("@Bill_Estado", cmbCasaEstados.SelectedItem.Text)
                .AddWithValue("@Bill_Municipio", cmbCasaMunicipio.SelectedItem.Text)
                .AddWithValue("@Bill_Pais", cmbCasaPaises.SelectedValue)
                .AddWithValue("@Bill_idCiudad", Integer.Parse(cmbCasaCiudades.SelectedValue))

                .AddWithValue("@Inventario", Integer.Parse(txtRooms.Text))
                .AddWithValue("@ContactoGte_tel", txtCasaTelG.Text)
                .AddWithValue("@ContactoGte_email", txtCasaCorreoG.Text)
                .AddWithValue("@ContactoGte_Nombre", txtNombreContacto.Text)
                .AddWithValue("@ContactoPuesto", "Agente de Viajes")


            End With
        End If
        Return insertCommand
    End Function

    Private Function InsertDataEmpresaAmenidades(ByVal command As SqlCommand)

        command.CommandText = "AmenidadesEmpresaCreate"
        For Each chkBList As CheckBoxList In chkList
            For Each chk As ListItem In chkBList.Items
                If chk.Selected Then
                    command.Parameters.Clear()
                    command.Parameters.AddWithValue(IDEMPRESA_PARM, idEmpresa)
                    command.Parameters.AddWithValue(AMENIDAD_PARM, chk.Value)
                    command.ExecuteNonQuery()
                End If
            Next
        Next

    End Function

    Private Function InsertDataHoteles(ByVal command As SqlCommand)
        command.CommandText = "Select IdHotel from OzHoteles..Hoteles where IdEmpresa = " & idEmpresa
        command.CommandType = CommandType.Text
        Dim strCheckin As Date
        Dim strCheckOut As Date
        strCheckin = CDate(Format(Date.Now, "yyyy/MM/dd ") & "12:00")
        strCheckOut = CDate(Format(Date.Now, "yyyy/MM/dd ") & "12:00")
        Dim row As DataRow
        Dim hotelData As New HotelDatos
        row = hotelData.Tables(HotelDatos.HOTEL_TABLE).NewRow
        ' Fill input data into new row
        With row
            .Item(HotelDatos.FIELD_CATEGORIA) = Integer.Parse(ddlCategory.SelectedValue)
            .Item(HotelDatos.FIELD_CHECKIN) = strCheckin
            .Item(HotelDatos.FIELD_CHECKOUT) = strCheckOut
            .Item(HotelDatos.FIELD_MAXDIASRENTA) = Integer.Parse(txtMaxDiasRenta.Text)
            .Item(HotelDatos.FIELD_IDMONEDA) = Integer.Parse(cmbCasaMonedas.SelectedValue)
            .Item(HotelDatos.FIELD_EDADAPARTIRPAGAEXTRA) = Integer.Parse(txtEdadMaximaAdo.Text)
            .Item(HotelDatos.FIELD_MAXEDADNINO) = Integer.Parse(txtEdadMaximaNino.Text)
            .Item(HotelDatos.FIELD_IDCIUDAD) = Integer.Parse(cmbCasaCiudades.SelectedValue)
            .Item(HotelDatos.FIELD_IDEMPRESA) = idEmpresa
            .Item(HotelDatos.FIELD_IDMONEDA) = Integer.Parse(cmbCasaMonedas.SelectedValue)
            .Item(HotelDatos.FIELD_IMPUESTO) = Integer.Parse(txtImpuesto.Text)
            .Item(HotelDatos.FIELD_MinNumCuartos) = 1
            .Item(HotelDatos.FIELD_MAXNUMCUARTOS) = 1
            .Item(HotelDatos.FIELD_MinEdadNinio) = Integer.Parse(txtEdadMinimaNino.Text)
            .Item(HotelDatos.FIELD_MaxNumOcupacion) = Integer.Parse(txtMaxPeople.Text)
            .Item(HotelDatos.FIELD_MinNumOcupacion) = 1
            .Item(HotelDatos.FIELD_MAXNUMNINOS) = Integer.Parse(txtMaxPeople.Text)
            .Item(HotelDatos.FIELD_MAXNUMADULTOS) = Integer.Parse(txtMaxPeople.Text)
            .Item(HotelDatos.FIELD_AVLONCORPMODULE) = False
            .Item(HotelDatos.FIELD_IsHouse) = True
            .Item(HotelDatos.FIELD_IdOwnerDesc) = idOwnerDesc
            .Item(HotelDatos.FIELD_AmaLlaves) = amaLlaves.Checked
            .Item(HotelDatos.FIELD_Banios) = Decimal.Parse(txtBanios.Text)
            .Item(HotelDatos.FIELD_Area) = Integer.Parse(txtArea.Text)
            .Item(HotelDatos.FIELD_AreaUnidad) = ddlUnits.SelectedValue
            .Item(HotelDatos.FIELD_STATUSAVAILABILITY) = ddlStatus.SelectedValue
            .Item(HotelDatos.FIELD_urlWebSite) = txtCasaPaginaWeb.Text
            .Item(HotelDatos.FIELD_AvailOnPortal) = True
            .Item(HotelDatos.FIELD_idDiccPoliticaCancelacionReview) = txtCancelPolitiesReview.Insert()
            .Item(HotelDatos.FIELD_idDiccPoliticaCancelacionFull) = txtCancelPolitiesFull.Insert()
            .Item(HotelDatos.FIELD_NOTAPPLYRESTRICTED) = saveDeparture()
            .Item(HotelDatos.FIELD_PLUSTAX) = chkPlusTax.Checked
            .Item(HotelDatos.FIELD_TipoPago) = Integer.Parse(ddlTipoPago.SelectedIndex)
            .Item(HotelDatos.FIELD_longitud) = Decimal.Parse(txtLongitud.Text)
            .Item(HotelDatos.FIELD_latitud) = Decimal.Parse(txtLatitud.Text)
            .Item(HotelDatos.FIELD_DIASLIBRES) = 3
            .Item(HotelDatos.FIELD_GET_RATES) = True
            .Item(HotelDatos.FIELD_CHAIN_CODE) = "UV"
            .Item(HotelDatos.FIELD_UserPerfil) = PaginaBase.PerfilHotel.Casa

            Select Case ddlCancelationPolicy.SelectedIndex
                Case cancelpolicy.bydays
                    .Item(HotelDatos.FIELD_DIASMINCANCELAR) = Integer.Parse(txtCancellationPolicy.Text)
                Case cancelpolicy.byhour
                    .Item(HotelDatos.FIELD_CancelHours) = Integer.Parse(txtCancellationPolicy.Text)
                Case cancelpolicy.specifichour
                    .Item(HotelDatos.FIELD_CancelSpecificHour) = ddlHour.SelectedValue & ddlMinutes.SelectedValue
            End Select


        End With
        ' Add it to the table

        hotelData.Tables(HotelDatos.HOTEL_TABLE).Rows.Add(row)
        Dim result As Boolean = False
        With New HotelSistema
            result = .CreateHotel(PortalCulture.GetCulture.ToString, txtCasaCorreoG.Text, hotelData)
            If (result) Then
                Create_Relatedassociation(idEmpresa)
            End If
        End With

        idHotel = command.ExecuteScalar()
        command.CommandText = "Update Hoteles set AvailOnPortal = 1,CategoriaCasa = " & ddlCategory.SelectedValue & " where idHotel =" & idHotel
        command.ExecuteNonQuery()

    End Function

    Private Function InsertDataRoom(ByVal command As SqlCommand)
        command.CommandText = "Select top 1 IdTipoHabitacion_Hotel from TipoHabitaciones_Hoteles where idHotel = " & idHotel
        command.CommandType = CommandType.Text
        Dim rooms As RoomsHotelData
        With New RoomFacade

            Dim saved As Boolean = _
            .createRoom(10, _
               idHotel, _
               0, _
               txtMaxPeople.Text, _
               0, _
               txtMaxPeople.Text, _
               txtMaxPeople.Text, _
               Me.mlDescriptionRoom.textodefault, _
               "Casa", _
               "A0S", 0, 0, _
                0, 0, 0, _
                CDbl(Val(0)), CDbl(Val(0)), CInt(Val(0)), _
               rooms, 1, 1)
            'Cambiar txtCasaNombre por Standard
        End With

        idTipoHabitacion_Hotel = command.ExecuteScalar()

        'Chicanada para agregarle a la habitacion la descripción
        command.CommandText = "Update TipoHabitaciones_Hoteles set IdDiccionarioDescripcion = " & mlDescriptionRoom.Insert() & ", IdDiccionarioNombreHabitacion = ISNULL((SELECT TOP 1 IDDiccionario from Diccionario where texto like '%" & txtCasaNombre.Text & "%'),0 ) where idHotel = " & idHotel & " AND idTipoHabitacion_Hotel = " & idTipoHabitacion_Hotel
        command.ExecuteNonQuery()

        With New RoomsInventoryFacade
            ' sdatodespues = String.Format("<NewDataSet><InventarioHabitaciones>room {0} begin date {1} end date {2} rooms {3} exception {4} </InventarioHabitaciones></NewDataSet>", tipoCuarto, inicio, fin, Rooms, GetDataExeption)
            Dim hr As Boolean = .update(idTipoHabitacion_Hotel, CDate(txtDateFrom.Text), CDate(txtDateTo.Text), 1, 0, "")
        End With


    End Function

    Private Function InsertDataRatePlan(ByVal command As SqlCommand)
        command.CommandText = "Update RatesPlan set IdDictionaryDescription = ISNULL((select IdDiccionario from diccionario where Texto like '%Tarifa Estandar%'),0), IdDiccShortDesc = ISNULL((select IdDiccionario from diccionario where Texto like '%Tarifa Estandar%'),0) where idHotel = " & idHotel
        command.CommandType = CommandType.Text
        Dim dsRate As New RatePlanData
        Dim Rp As RatePlanData
        Dim rRate As DataRow
        rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
        With rRate
            .Item(dsRate.FIELD_IDRATEPLAN) = "RAC"
            .Item(dsRate.FIELD_DESCRIPTION) = "Tarifa Estandar"
            .Item(dsRate.FIELD_SEGMENT) = "R"
            .Item(dsRate.FIELD_IDHOTEL) = idHotel
            '.Item(dsRate.FIELD_IDDICDESC) = Me.txtDescripcion.IdIndice
            .Item(dsRate.FIELD_HOTELPAYMENT) = False
            .Item(dsRate.FIELD_CODIGOTARIFA) = "RAC"
            .Item(dsRate.FIELD_NAME) = "Tarifa Estandar"
            '.Item(dsRate.FIELD_IDDICSHORTDESC) = "Tarifa Estandar"
            .Item(dsRate.FIELD_GDS) = False
            .Item(dsRate.FIELD_GDSAPPLY) = False
            .Item(dsRate.FIELD_PORTAL) = True
            .Item(dsRate.FIELD_UNIPANTALLA) = False
            .Item(dsRate.FIELD_ADS) = False
            .Item(dsRate.FIELD_COMGDS) = DBNull.Value
            .Item(dsRate.FIELD_COMPORTAL) = DBNull.Value
            .Item(dsRate.FIELD_COMONEPAGE) = DBNull.Value
            .Item(dsRate.FIELD_COMADS) = DBNull.Value
            .Item(dsRate.WAITLISTAVAILABLE_FIELD) = True
            '.Item(dsRate.FIELD_IDDICCPROMODESC) = Me.txtPromoDescription.IdIndice
            .Item(dsRate.FIELD_IDCONTRATO) = System.DBNull.Value
            .Item(dsRate.FIELD_IDRULE) = System.DBNull.Value
            .Item(dsRate.FIELD_ORDEN) = 1
            .Item(dsRate.FIELD_IDMONEDA) = cmbCasaMonedas.SelectedValue
        End With
        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)

        With New RatePlanFacade
            Dim insertedRP As Boolean = .InsertRatePlan(dsRate, Me.idDicc, 0, 0)
        End With

        command.ExecuteNonQuery()
    End Function

    Private Function InsertDataRate(ByVal command As SqlCommand)

        Dim strError As String = "No cambio"
        Dim datFare As New FaresData
        Dim ExistCode As New FaresData
        Dim rowFare As DataRow

        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idTipoHabitacion_Hotel)
        End With
        If room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
        lblError.Text = "Rooms"
        Try
            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()
                rowFare(FaresData.ENDDATE_FIELD) = CDate(txtDateTo.Text)
                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = ConvDouble(txtAdult.Text)
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = ConvDouble(txtChild.Text)
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = ConvDouble(txtAdult.Text)

                rowFare(FaresData.NINIORATE) = 0
                rowFare(FaresData.RATEENPRICE_FIELD) = 0
                rowFare(FaresData.PRICE_FIELD) = 0

                Double.TryParse(Me.txtChild.Text, rowFare(FaresData.NINIORATE))
                Double.TryParse(txtChild.Text, rowFare(FaresData.RATEENPRICE_FIELD))
                Double.TryParse(Me.txtAdult.Text, rowFare(FaresData.PRICE_FIELD))

                rowFare(FaresData.STARTDATE_FIELD) = CDate(txtDateFrom.Text)
                'rowFare(FaresData.EXCEPTION_FIELD) = Me.Exceptions
                'If Me.txtAdvBooking.Text <> "" Then
                '    rowFare(FaresData.ADVBOOKING_FIELD) = CInt(Val(Me.txtAdvBooking.Text))
                'End If
                'If Me.txtMaxAdvBooking.Text <> "" Then
                '    rowFare(FaresData.MAXADVBOOKING_FIELD) = CInt(Val(Me.txtMaxAdvBooking.Text))
                'End If
                'If Me.txtMinDias.Text <> "" Then
                '    rowFare(FaresData.MINDIAS_FIELD) = CInt(Val(Me.txtMinDias.Text))
                'End If
                'If Me.txtMaxDias.Text <> "" Then
                '    rowFare(FaresData.MAXDIAS_FIELD) = CInt(Val(Me.txtMaxDias.Text))
                'End If
                rowFare(FaresData.RULESDEFAULT) = True
                'rowFare(FaresData.NOARRIVOS_FIELD) = GetArrivosField()
                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idTipoHabitacion_Hotel
                rowFare(FaresData.RATETYPE_FIELD) = "R"
                rowFare(FaresData.IDRATEPLAN_FIELD) = "RAC"
                rowFare(FaresData.RATECODE_FIELD) = "A0SRAC"
                'rowFare(FaresData.WAITLISTAVAILABLE_FIELD) = Me.chkWaitListAvailable.Checked

                'rowFare(FaresData.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.Insert()

                'Promociones, Ventanas, Ocupacion
                'If txtDescProm.Text.Trim <> "" Then
                '    rowFare(FaresData.DESCPROMOTION_FIELD) = txtDescProm.Text
                'Else
                rowFare(FaresData.DESCPROMOTION_FIELD) = DBNull.Value
                'End If

                'If lstPeoplesInRoom.SelectedValue <> -1 Then
                '    rowFare(FaresData.PERSONAS_FIELD) = lstPeoplesInRoom.SelectedValue
                'Else
                rowFare(FaresData.PERSONAS_FIELD) = DBNull.Value
                'End If

                'If lstMinNumberAdults.SelectedValue <> -1 Then
                '    rowFare(FaresData.MINADULTOS_FIELD) = lstMinNumberAdults.SelectedValue
                'Else
                rowFare(FaresData.MINADULTOS_FIELD) = DBNull.Value
                'End If

                'If lstNumberAdults.SelectedValue <> -1 Then
                '    rowFare(FaresData.MAXADULTOS_FIELD) = lstNumberAdults.SelectedValue
                'Else
                rowFare(FaresData.MAXADULTOS_FIELD) = DBNull.Value
                'End If

                'If lstNumberChildrens.SelectedValue <> -1 Then
                '    rowFare(FaresData.MAXNINIOS_FIELD) = lstNumberChildrens.SelectedValue
                'Else
                rowFare(FaresData.MAXNINIOS_FIELD) = DBNull.Value
                'End If

                'If lstPeoplesExtras.SelectedValue <> -1 Then
                '    rowFare(FaresData.PERSONASEXTRAS_FIELD) = lstPeoplesExtras.SelectedValue
                'Else
                rowFare(FaresData.PERSONASEXTRAS_FIELD) = DBNull.Value
                'End If

                'If chkBookingWindow.Checked Then
                '    Try
                '        rowFare(FaresData.BOOKINGWINDOWSTART_FIELD) = CDate(txtBookWindowDateFrom.Text)
                '        rowFare(FaresData.BOOKINGWINDOWEND_FIELD) = CDate(txtBookWindowDateTo.Text)
                '    Catch
                rowFare(FaresData.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                rowFare(FaresData.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                '    End Try
                'Else
                'rowFare(FaresData.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                'rowFare(FaresData.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                'End If
                .Rows.Add(rowFare)
            End With

            With New FaresSystem
                Try
                    If .InsertFares(datFare, strError) Then

                    End If
                Catch
                End Try
            End With
        Catch
        End Try

    End Function

    Private Function ChangeConnection(ByRef command As SqlCommand, ByVal connectionString As String, Optional ByRef trans As SqlTransaction = Nothing)
        'Cambia la conección a OzHoteles
        command.Connection.Close()
        command.Connection.Dispose()
        command.Connection = New SqlClient.SqlConnection(connectionString)
        command.Connection.Open()
        trans = command.Connection.BeginTransaction()
        command.Transaction = trans
    End Function

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

    Private Function validaHours() As Boolean
        Dim valido As Boolean = True
        If Me.ddlCancelationPolicy.SelectedIndex <> Me.cancelpolicy.specifichour Then
            If txtCancellationPolicy.Text.Length > 3 Then
                valido = False
            End If
            lblErrorHours.Text = "0-999"
        End If
        lblErrorHours.Visible = Not valido
        Return valido
    End Function

    Function ValidaEdad(ByVal edad1 As String, ByVal edad2 As String) As Boolean
        Dim valor As Integer
        Dim valor1 As Integer

        Integer.TryParse(edad1, valor)
        Integer.TryParse(edad2, valor1)
        If valor <> 0 Then
            If (valor > valor1) Then
                Return False
            End If
        End If
        Return True
    End Function

    Function ConvDouble(ByVal valor As String) As Double
        Return If(String.IsNullOrEmpty(valor), 0, Double.Parse(valor))
    End Function


    Private Function BuildDataImpuestos() As DataSet
        Dim ds As DataSet
        Dim table As DataTable = New DataTable("Table1")

        ds = New DataSet
        With table.Columns
            .Add("idHotel", GetType(System.Int32))
            .Add("impuesto", GetType(System.Decimal))
            .Add("impuestoNuevo", GetType(System.Decimal))
        End With
        ds.Tables.Add(table)
        Return ds
    End Function
End Class

Public Class CheckListObject
    Inherits PaginaBase
    Public Property checked As Boolean
        Get
            Return ViewState("checked")
        End Get
        Set(value As Boolean)
            ViewState("checked") = value
        End Set
    End Property
    Public Property id As Integer
        Get
            Return ViewState("id")
        End Get
        Set(value As Integer)
            ViewState("id") = value
        End Set
    End Property
    Public Property text As String
        Get
            Return ViewState("text")
        End Get
        Set(value As String)
            ViewState("text") = value
        End Set
    End Property
    Sub New(ByVal id As Integer, ByVal text As String, Optional ByVal checked As Boolean = False)
        Me.text = text
        Me.id = id
        Me.checked = checked
    End Sub



End Class