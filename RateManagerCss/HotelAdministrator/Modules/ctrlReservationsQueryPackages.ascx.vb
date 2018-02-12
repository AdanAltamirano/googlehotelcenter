Imports Portal.Hotel.Facade
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.Hotel.Common.Data
Partial Public Class ctrlReservationsQueryPackages
    Inherits System.Web.UI.UserControl

    Private Const KEY_IDHOTEL As String = "idHotel"
    Public Event onSendReservation(ByVal res As DataView)
    Public Event Fillds(ByVal res As DataView)
    Public Event loadData(ByVal GridCurrentPageIndex As Integer)
    Public Property idHotel() As Integer
        Get
            If ViewState.Item(KEY_IDHOTEL) Is Nothing Then
                Return 0
            Else
                Return ViewState.Item(KEY_IDHOTEL)
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add(KEY_IDHOTEL, Value)
        End Set
    End Property
    Private Property idSegmento() As Integer
        Get
            If Not AppSettings("IdSegmento") Is Nothing Then
                Return AppSettings("IdSegmento")
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("idSegmento") = value
        End Set
    End Property
    Public Property Supervisor() As Boolean
        Get
            Return ViewState("Sup_User")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Sup_User") = Value
        End Set
    End Property
    Public Property UserChain() As Boolean
        Get
            Return ViewState("UserChain_User")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("UserChain_User") = Value
        End Set
    End Property
    Public Property idUsuario() As Integer
        Get
            Return ViewState("idUsuario_User")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idUsuario_User") = Value
        End Set
    End Property
    Public ReadOnly Property getTxtName() As TextBox
        Get
            Return Me.txtnoConfirmacion
        End Get
    End Property
    Public ReadOnly Property getbtnName() As Button
        Get
            Return Me.btnBuscar
        End Get
    End Property
    Public WriteOnly Property hideExport() As Boolean
        Set(ByVal Value As Boolean)
            hlnkExcel.Visible = Value
        End Set
    End Property


    'Public WriteOnly Property hideAgency() As Boolean
    '    Set(ByVal Value As Boolean)
    '        lblAgencia.Visible = Value
    '        ddlAgencias.Visible = Value
    '    End Set
    'End Property

    Private Sub ShowLink(ByVal show As Boolean)
        Dim str As String = If(show, "block", "none")
        Dim str2 As String = If(Not show, "block", "none")

        Me.hplShow.Style.Add("display", str)
        Me.hplHide.Style.Add("display", str2)
    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Me.lblNoconfirmacion.Visible = (Me.QueryType <> QueryTypes.PaidOnline)
        Me.lstFindBy.Visible = (Me.QueryType = QueryTypes.PaidOnline)

        If Not Me.IsPostBack Then
            Me.initDates()
            Me.loadStatus()
            loadfilter()
            LoadAgencies()
            Me.lstFindBy.Items.Clear()
            Me.lstFindBy.Items.Add(PortalCulture.GetString("00447"))
            Me.lstFindBy.Items.Add(PortalCulture.GetString("01402"))
            Me.lstFindBy.SelectedIndex = If(Me.QueryType = QueryTypes.PaidOnline, 1, 0)
            ShowLink(True)

            If idSegmento = 274 Then
                ddlAgencies.Visible = True
            End If

        Else
        End If
        Dim script As String = String.Empty

        chkDates.Attributes.Add("onclick", "javascript:ShowOrHide('" & Me.chkDates.ClientID & "');")
        'Me.Page.RegisterStartupScript("", "<script>ShowOrHide('" & Me.chkDates.ClientID & "');" & script & "</script>")

        Me.hplHide.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "false")
        Me.hplShow.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "true")
        BtnSpecificSearch.Attributes.Add("onclick", "FireResize();")
    End Sub

    Public Sub SetStatus(ByVal status As Integer)
        Try
            lstStatus.SelectedValue = status
        Catch ex As Exception
        End Try
    End Sub

    Enum QueryTypes
        Unknow = 0
        Normal = 1
        PaidOnline = 2
    End Enum

    Public Property QueryType() As QueryTypes
        Get
            Dim result As Integer = QueryTypes.Unknow
            If Me.ViewState("QueryType") Is Nothing Then Me.ViewState("QueryType") = QueryTypes.Normal
            Integer.TryParse(Me.ViewState("QueryType"), result)
            Return result
        End Get
        Set(ByVal value As QueryTypes)
            Me.ViewState("QueryType") = value
        End Set
    End Property

    Public Sub ShowHideData(ByVal SpecificSearch As Boolean, ByVal status As Boolean, ByVal origen As Boolean, ByVal cliente As Boolean)
        If SpecificSearch = False Then
            divSpecificSearch.Style.Add("display", "none")
        End If
        If status = False Then
            lblStatus.Visible = False
            lstStatus.Visible = False
        End If
    End Sub

    Private Sub ddlAgencies_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlAgencies.SelectedIndexChanged
        Call loadAgents()
    End Sub

    Private Sub loadAgents()
        Dim DatosAgentes As Agentes
        Dim item As ListItem
        If ddlAgencies.SelectedValue > 0 Then
            ddlAgents.Visible = True
            lblAgent.Visible = True

            ddlAgents.DataSource = (New Agentes).getAgentByIdAgency(CInt(ddlAgencies.SelectedValue))
            ddlAgents.DataTextField = "Email"
            ddlAgents.DataValueField = "idUsuario"
            ddlAgents.DataBind()

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("01620"), False), "0") 'Todos los Agentes
            ddlAgents.Items.Insert(0, item)
        Else
            ddlAgents.Visible = False
            lblAgent.Visible = False
        End If
    End Sub

    Function LoadAgencies() As Boolean
        Dim item As ListItem
        Dim dataCompany As EmpresaDatos
        Dim pos As Integer = ddlAgencies.SelectedIndex

        dataCompany = (New EmpresaSistema).GetCompanyAgencyByIdSegment(Me.idSegmento)

        If Me.QueryType = QueryTypes.PaidOnline Then Return False

        ddlAgencies.Items.Clear()
        If Not dataCompany Is Nothing AndAlso dataCompany.Tables.Count > 0 AndAlso dataCompany.Tables(0).Rows.Count > 0 Then
            ddlAgencies.DataSource = dataCompany.Tables(0)
            ddlAgencies.DataTextField = "Nombre"
            ddlAgencies.DataValueField = "idAgencia"
            ddlAgencies.DataBind()

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("01619"), False), "-2") 'Todas las reservaciones
            ddlAgencies.Items.Insert(0, item)

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("00576"), False), "-1") 'Sin agencias
            ddlAgencies.Items.Insert(0, item)

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("M000624"), False), "0") 'Todas las agencias
            ddlAgencies.Items.Insert(0, item)

        Else
            item = New ListItem(PortalCulture.GetString("M000624", False), "0")
            ddlAgencies.Items.Add(item)
        End If
        If pos <> -1 Then
            ddlAgencies.SelectedIndex = pos
        End If
    End Function

    Private Sub initDates()
        Date1.minYear = Now.Year - 3
        Date1.maxYear = Now.Year + AppSettings("AniosLimit")
        Date1.selectedDate = Now
        Date2.minYear = Now.Year - 3
        Date2.maxYear = Now.Year + AppSettings("AniosLimit")
        Date2.selectedDate = Now

        If Now.Month = 12 Then
            Date2.selectedMonth = _Date.Months.january
            Date2.selectedYear = Date2.selectedYear + 1
        Else
            Date2.selectedMonth = Date2.selectedMonth + 1
        End If
    End Sub

    


    Private Sub loadStatus()
        Me.lstStatus.Items.Clear()
        Dim status() As String = PortalCulture.GetString("M000116").Split(",")  ' Undefined,Reserved,Confirmed,Cancelled
        Dim item As New ListItem
        'Sele inserta el elemento en la posicion 0 -- Todos --
        item.Text = "-- " & PortalCulture.GetString("M000258") & " --"
        item.Value = 255
        item.Selected = True
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = status(1)
        item.Value = 1
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = status(3)
        item.Value = 3
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = status(4)
        item.Value = 4
        Me.lstStatus.Items.Add(item)

        item = New ListItem
        item.Text = PortalCulture.GetString("01586")
        item.Value = 5
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("M0BT0000052")
        item.Value = 6
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("M0BT0000052") & " / " & PortalCulture.GetString("01586")
        item.Value = 7
        Me.lstStatus.Items.Add(item)

        'item = New ListItem
        'item.Text = status(2)
        'item.Value = 2
        'Me.lstStatus.Items.Add(item)       

    End Sub
    Private Sub loadfilter()
        Dim item As New ListItem
        ddlFilterby.Items.Clear()
        item = New ListItem
        item.Text = PortalCulture.GetString("00392")
        item.Value = 2
        ddlFilterby.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("00368")
        item.Value = 1
        ddlFilterby.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("00369")
        item.Value = 3
        ddlFilterby.Items.Add(item)
        ddlFilterby.SelectedValue = 1
    End Sub

    Public Function searchReservations(Optional ByVal newStatus As Integer = -1) As DataView

        Page.Validate()
        If Not Page.IsValid Then Exit Function
        Dim CheckIn As DateTime = Date1.selectedDate
        Dim CheckOut As DateTime = Date2.selectedDate
        Dim sAgency As String = If(ddlAgencies.SelectedValue > -2, ddlAgencies.SelectedValue, "")
        Dim sAgente As String = If(ddlAgents.Visible, ddlAgents.SelectedValue, "-2")

        Dim Status As Byte = IIf(newStatus = -1, Me.lstStatus.SelectedValue, newStatus)
        Return getReservations(CheckIn, CheckOut, Status, Me.ddlFilterby.SelectedItem.Value, sAgencia:=sAgency, sAgente:=sAgente)
    End Function

    Public Function searchNoconfirmReservations(ByVal AllHotels As Boolean) As DataSet
        Page.Validate()
        If Not Page.IsValid Then Exit Function
        Return getnoconfirmreservations(AllHotels)
    End Function


    ' Store procedures
    Public Const SPGETRESERVATIONS As String = "spReservations_GetPackages"
    Public Const PRM_ID_HOTEL As String = "@idHotel"
    Public Const PRM_CHECKIN As String = "@checkIn"
    Public Const PRM_CHECKOUT As String = "@checkOut"
    Public Const PRM_ID_ROOM_HOTEL As String = "@idTipoHabitacion_Hotel"
    Public Const PRM_STATUS As String = "@status"
    Public Const PRM_NAME_CLIENT As String = "@NameClient"
    Public Const PRM_FILTER As String = "@filter"
    Public Const PRM_IDIOMA As String = "@idioma"
    Public Const PRM_NORES As String = "@ConfimNum"
    Public Const PRM_NOAUTH As String = "@AuthorizationNum"
    Public Const PRM_ASOCIACION As String = "@idAsociacion"
    Public Const PRM_AGENCIA As String = "@Agencia"
    Public Const PRM_IDAGENTE As String = "@idAgente"

    Function GetIdAsociation() As Integer
        Dim IdAsociation As Integer = 0
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function


    Private Function Columnas(ByVal data As DataSet)
        Dim CExport As New CExportExcell

        CExport.AddParameter("Packageid", "ID Paquete")
        CExport.AddParameter("Itinerary", "Itinerario")
        CExport.AddParameter("TravelerName", "Viajero")
        CExport.AddParameter("FechaReservacion", "Fecha de Reservaci#243;n")
        CExport.AddParameter("Checkin", "Check In")
        CExport.AddParameter("chackOut", "Check Out")
        CExport.AddParameter("ActivityReservationId", "Actividad")
        CExport.AddParameter("Agencia", "Agencia")
        CExport.AddParameter("Agente", "Agente")
        CExport.AddParameter("Status", "Status")
        CExport.AddParameter("Fee", "Comisi#243;n")

        CExport.DataSource = data
        Session("__CExport__") = CExport
    End Function

    Public Function getReservations(ByVal CheckIn As DateTime, ByVal CheckOut As DateTime, ByVal Status As Byte, ByVal filter As String, Optional ByVal sAgencia As String = "", Optional ByVal sAgente As String = "-2") As DataView
        'Crear la consulta y traer un reader
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        Dim data As New DataSet

        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            '   Try
            With .SelectCommand
                .CommandType = CommandType.StoredProcedure
                .CommandText = Me.SPGETRESERVATIONS
                .Connection = New SqlConnection(ConnectionString)

                If chkDates.Checked Then
                    .Parameters.Add(New SqlParameter(Me.PRM_CHECKIN, SqlDbType.DateTime)).Value = CheckIn
                    .Parameters.Add(New SqlParameter(Me.PRM_CHECKOUT, SqlDbType.DateTime)).Value = CheckOut
                End If

                .Parameters.Add(New SqlParameter(Me.PRM_FILTER, SqlDbType.Int)).Value = filter
                .Parameters.Add(New SqlParameter(Me.PRM_STATUS, SqlDbType.TinyInt)).Value = Status
                .Parameters.Add(New SqlParameter(PRM_IDIOMA, SqlDbType.Int)).Value = PortalCulture.GetIDCulture

                If Not String.IsNullOrEmpty(sAgencia) Then
                    .Parameters.Add(New SqlParameter(PRM_AGENCIA, SqlDbType.NVarChar, 80)).Value = sAgencia
                    .Parameters.Add(New SqlParameter("@idUsuario", SqlDbType.Int)).Value = CInt(sAgente)
                End If

            End With
            .Fill(data)

            '  Catch ex As Exception
            '  Dim exx As String = ex.Message
            ' Finally
            If Not .SelectCommand Is Nothing Then
                If Not .SelectCommand.Connection Is Nothing Then
                    .SelectCommand.Connection.Dispose()
                End If
                .SelectCommand.Dispose()
            End If
            .Dispose()
            '     End Try
        End With
        Dim dv As DataView
        dv = data.Tables(0).DefaultView

        Dim filtroStr As String = String.Empty

        If filtroStr <> String.Empty Then
            dv.RowFilter = filtroStr
        End If

        hlnkExcel.Enabled = True

        Session("dvRes") = dv
        Columnas(data)
        Return dv
    End Function

    Private Function getReservationsByNum() As DataView
        'Crear la consulta y traer un reader
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        Dim data As New DataSet

        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = Me.SPGETRESERVATIONS
                    .Connection = New SqlConnection(ConnectionString)
                    .Parameters.Add(New SqlParameter(Me.PRM_FILTER, SqlDbType.Int)).Value = "4"
                    .Parameters.Add(New SqlParameter(Me.PRM_STATUS, SqlDbType.Int)).Value = 255
                    .Parameters.Add(New SqlParameter(Me.PRM_NORES, SqlDbType.NVarChar, 24)).Value = Me.txtnoConfirmacion.Text.Trim
                End With
                .Fill(data)
            Catch
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
        Dim dv As DataView
        dv = data.Tables(0).DefaultView
        hlnkExcel.Enabled = True
        Session("dvRes") = dv
        Return dv
    End Function





    Private Sub loadResources()
        Me.lblDesde.Text = PortalCulture.GetString("M000110", True)
        Me.lblHasta.Text = PortalCulture.GetString("M000111", True)


        Me.lblStatus.Text = PortalCulture.GetString("M000115", True)

        btnBuscar.Text = PortalCulture.GetString("M000637")
        lblNoconfirmacion.Text = PortalCulture.GetString("00447", True)
        Me.lblSearchSpecific.Text = PortalCulture.GetString("00479")
        Me.lblSearchData.Text = PortalCulture.GetString("00478")
        Me.BtnSpecificSearch.Text = PortalCulture.GetString("M000118")
        Me.chkDates.Text = PortalCulture.GetString("00517")
        Me.lblSearchby.Text = PortalCulture.GetString("M0BT0000113", True)

        Me.hplShow.Text = PortalCulture.GetString("01454")
        Me.hplHide.Text = PortalCulture.GetString("01455")
        'lblAgencia.Text = PortalCulture.GetString("M000524", True)
        lblAgency.Text = PortalCulture.GetString("M0BT0000073", True)
        lblAgent.Text = PortalCulture.GetString("00615", True)
    End Sub

    Sub callFilter()
        Dim sFiltro As String

        sFiltro = PortalCulture.GetString("01380") & ","
        sFiltro &= If(chkDates.Checked, String.Format(PortalCulture.GetString("01370"), ddlFilterby.SelectedItem.Text), " ")
        sFiltro &= If(lstStatus.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01371"), lstStatus.SelectedItem.Text))
        sFiltro &= String.Format(PortalCulture.GetString("01374"), Date1.selectedDate.ToString("dd/MM/yyyy"), Date2.selectedDate.ToString("dd/MM/yyyy"))
        lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        loadResources()

        If Not Me.IsPostBack Then
            If Me.txtnoConfirmacion.Text.Trim().Length > 0 Then
                Me.btnBuscar_Click(sender, e)
                If TypeOf Me.Page Is ConfirmReservas Then
                    If Not Session("welcome_noReservacion") Is Nothing Then
                        Session("welcome_noReservacion") = String.Empty
                    End If
                End If

            Else
                BtnSpecificSearch_Click(sender, e)
            End If
        End If

    End Sub



    Private Sub lstStatus_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstStatus.PreRender
        Dim Sel As Integer
        Sel = Me.lstStatus.SelectedIndex
        Me.loadStatus()
        Me.lstStatus.Items(0).Text = "-- " & PortalCulture.GetString("M000258") & " --"
        Me.lstStatus.SelectedIndex = Sel
    End Sub

    Public Function VerifiedIdCorporate(ByVal idPortal As Integer, ByVal idCorp As Integer) As Boolean
        Dim ds As New DataSet
        Dim dsCommand As New SqlDataAdapter
        Dim loadCommand As SqlCommand
        Dim idCorporatePortal As Integer = 0
        Try
            loadCommand = New SqlCommand("spPortalesGetCorporate", New SqlConnection(ConfigurationSettings.AppSettings("HotelConnection")))
            loadCommand.CommandType = CommandType.StoredProcedure
            loadCommand.Parameters.Add(New SqlParameter("@idPortal", SqlDbType.Int))
            dsCommand.SelectCommand = loadCommand
            dsCommand.SelectCommand.Parameters("@idPortal").Value = idPortal
            dsCommand.Fill(ds)
        Catch
        Finally
            If Not dsCommand.SelectCommand Is Nothing Then
                If Not dsCommand.SelectCommand.Connection Is Nothing Then
                    dsCommand.SelectCommand.Connection.Dispose()
                End If
                dsCommand.SelectCommand.Dispose()
            End If
            loadCommand.Dispose()
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                idCorporatePortal = ds.Tables(0).Rows(0)("idCorporativo")
            End If
        End Try
        Return (idCorp = idCorporatePortal)
    End Function

    Public Sub buscarNumeroReservacion(Optional ByVal noReservacion As String = "")
        If noReservacion <> String.Empty Then
            Me.txtnoConfirmacion.Text = noReservacion
        End If

        If Me.txtnoConfirmacion.Text.Trim <> "" Then
            Dim DV As DataView = Me.getReservationsByNum()
            RaiseEvent onSendReservation(DV)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        ' cvlNoConfirmacion.IsValid = True
        lblFiltro.Text = String.Format(PortalCulture.GetString(If(Me.QueryType = QueryTypes.PaidOnline, "01401", "01378")), txtnoConfirmacion.Text)
        buscarNumeroReservacion()
    End Sub

    Private Sub BtnSpecificSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSpecificSearch.Click
        '  cvlNoConfirmacion.IsValid = True
        'If Not (ddlCanal.SelectedIndex = 0 Or ddlCanal.SelectedIndex = 4) Then
        '    ddlAgencias.SelectedIndex = 0
        'End If
        callFilter()
        RaiseEvent loadData(0)
    End Sub

    Private Function getnoconfirmreservations(ByVal AllHotels As Boolean) As DataSet

        Dim CheckIn As DateTime = Date1.selectedDate
        Dim CheckOut As DateTime = Date2.selectedDate
        Dim ds As ReservaDatos
        Dim idAsociac As Integer = Me.GetIdAsociation

        If Supervisor AndAlso (AllHotels Or idHotel = 0) Then
            With New ReservaFacade
                ds = .GetNoConfirmedReservations(CheckIn, CheckOut, Me.ddlFilterby.SelectedItem.Value, idAsociacion:=idAsociac)
            End With
        Else

            With New ReservaFacade
                ds = .GetNoConfirmedReservations(CheckIn, CheckOut, Me.ddlFilterby.SelectedItem.Value, idHotel, idAsociacion:=idAsociac)
            End With
        End If
        Return ds

    End Function
End Class