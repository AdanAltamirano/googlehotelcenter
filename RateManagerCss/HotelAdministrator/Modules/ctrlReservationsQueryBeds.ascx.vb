Imports Portal.Hotel.Facade
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.Hotel.Common.Data

Partial Public Class ctrlReservationsQueryBeds
    Inherits System.Web.UI.UserControl

    Private Const KEY_IDHOTEL As String = "idHotel"
    Public Event onSendReservation(ByVal idReservacion As Integer)
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
    Public ReadOnly Property idtipohabitacionhotel() As Integer
        Get
            Return Me.lstRoomType.SelectedValue
        End Get
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
        lstRoomType.Visible = False
        lblTipoHab.Visible = False
        cvMissingRoomTypes.Visible = False
        If Not Me.IsPostBack Then
            Me.initDates()
            Me.loadRoomsTypes()
            Me.loadStatus()
            loadfilter()
            LoadAgencies()
            If Me.UserChain Then
                loadUserChainHotels()
            End If

            If idSegmento = 274 Then
                ddlAgencies.Visible = True
            End If

            Me.lstFindBy.Items.Clear()
            Me.lstFindBy.Items.Add(PortalCulture.GetString("00447"))
            Me.lstFindBy.Items.Add(PortalCulture.GetString("01402"))
            Me.lstFindBy.SelectedIndex = If(Me.QueryType = QueryTypes.PaidOnline, 1, 0)

            Dim item As ListItem = New ListItem(PortalCulture.GetString("00172", False), "0")
            DropDownListTransaction.Items.Add(item)

            item = Nothing
            item = New ListItem(PortalCulture.GetString("01052", False), "1")
            DropDownListTransaction.Items.Add(item)

            item = Nothing
            item = New ListItem(PortalCulture.GetString("01053", False), "2")
            DropDownListTransaction.Items.Add(item)

            item = Nothing
            item = New ListItem(PortalCulture.GetString("01054", False), "3")
            DropDownListTransaction.Items.Add(item)
            ShowLink(True)

        Else

        End If

        cvlNoConfirmacion.IsValid = True
        Dim script As String = String.Empty
        If Me.UserChain Then
            Me.chkHotels.Attributes.Add("onclick", "javascript:ShowOrHideHotels('" & Me.chkHotels.ClientID & "','" & Me.lblHoteles.ClientID & "','" & Me.ddlHoteles.ClientID & "');")
            script = "ShowOrHideHotels('" & Me.chkHotels.ClientID & "','" & Me.lblHoteles.ClientID & "','" & Me.ddlHoteles.ClientID & "');"
        End If

        chkDates.Attributes.Add("onclick", "javascript:ShowOrHide('" & Me.chkDates.ClientID & "');")
        'Me.Page.RegisterStartupScript("", "<script>ShowOrHide('" & Me.chkDates.ClientID & "');" & script & "</script>")
        lblHoteles.Style.Add("display", "none")
        ddlHoteles.Style.Add("display", "none")

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
        If origen = False Then
            lblCanal.Visible = False
            ddlCanal.Visible = False
        End If
        If cliente = False Then
            lblNombreCliente.Visible = False
            txtNameCustomer.Visible = False
        End If


    End Sub


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

    Private Sub loadUserChainHotels()
        Dim ds As DataSet
        With New HotelSistema
            ds = .GetHotelsCompanyByUser(Me.idUsuario)
        End With
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            ddlHoteles.DataSource = ds.Tables(0)
            ddlHoteles.DataTextField = "Nombre"
            ddlHoteles.DataValueField = "idHotel"
            ddlHoteles.DataBind()
            Dim item As ListItem = New ListItem(PortalCulture.GetString("00172", False), -1)
            ddlHoteles.Items.Insert(0, item)
            If Not Session("welcome_idhotel") Is Nothing AndAlso Session("welcome_idhotel") <> String.Empty Then
                ddlHoteles.SelectedValue = Session("welcome_idhotel")
            End If
        End If
    End Sub

    Private Sub loadRoomsTypes()
        Me.lstRoomType.Items.Clear()
        With New RoomFacade
            Dim room As RoomsHotelData
            room = .getRooms(Me.idHotel, PortalCulture.GetIDCulture)
            Me.lstRoomType.DataSource = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            Me.lstRoomType.DataTextField = RoomsHotelData.FLD_NOMBRE
            Me.lstRoomType.DataValueField = RoomsHotelData.FLD_ID_ROOM_TYPE
            Me.lstRoomType.DataBind()
            'Se le inserta el elemento en posicion 0 -- Todos los tipos --
            Dim item As New ListItem
            item.Text = "-- " & PortalCulture.GetString("M000259") & " --"
            item.Value = 0
            item.Selected = True
            Me.lstRoomType.Items.Insert(0, item)
        End With
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

        ddlCanal.Items.Clear()
        item = New ListItem
        item.Text = PortalCulture.GetString("00172")
        item.Value = "ALL"
        ddlCanal.Items.Add(item)

        item = New ListItem
        item.Text = PortalCulture.GetString("M000025")
        item.Value = "POR"
        ddlCanal.Items.Add(item)

        item = New ListItem
        item.Text = "Call Center"
        item.Value = "CCT"
        ddlCanal.Items.Add(item)

        'item = New ListItem
        'item.Text = PortalCulture.GetString("00457")
        'item.Value = "UNI"
        'ddlCanal.Items.Add(item)

        'item = New ListItem
        'item.Text = PortalCulture.GetString("00675")
        'item.Value = "HTL"
        'ddlCanal.Items.Add(item)

        'item = New ListItem
        'item.Text = "GDS"
        'item.Value = "WIZ"
        'ddlCanal.Items.Add(item)

        'item = New ListItem
        'item.Text = "ADS"
        'item.Value = "ADS"


        'ddlCanal.Items.Add(item)
    End Sub

    Public Function searchReservations(Optional ByVal newStatus As Integer = -1) As DataView

        Page.Validate()
        If Not Page.IsValid Then Exit Function
        Dim CheckIn As DateTime = Date1.selectedDate
        Dim CheckOut As DateTime = Date2.selectedDate
        Dim RoomId As Integer = Me.lstRoomType.SelectedValue
        Dim Status As Byte = IIf(newStatus = -1, Me.lstStatus.SelectedValue, newStatus)
        Dim NameClient As String = Me.txtNameCustomer.Text.Trim
        Dim sAgency As String = If(ddlAgencies.SelectedValue > -2, ddlAgencies.SelectedValue, "")
        Dim sAgente As String = If(ddlAgents.Visible, ddlAgents.SelectedValue, "-2")

        If UserChain Then
            If chkHotels.Checked Then
                Return getReservations(Me.ddlHoteles.SelectedValue, CheckIn, CheckOut, RoomId, Status, NameClient, Me.ddlFilterby.SelectedItem.Value, Me.idUsuario)
            Else
                Return getReservations(Me.idHotel, CheckIn, CheckOut, RoomId, Status, NameClient, Me.ddlFilterby.SelectedItem.Value, CInt(sAgente), sAgency)
            End If
        Else
            Return getReservations(Me.idHotel, CheckIn, CheckOut, RoomId, Status, NameClient, Me.ddlFilterby.SelectedItem.Value, CInt(sAgente), sAgency)
        End If



    End Function

    Public Function searchNoconfirmReservations(ByVal AllHotels As Boolean) As DataSet

        Page.Validate()
        If Not Page.IsValid Then Exit Function

        Return getnoconfirmreservations(AllHotels)

    End Function



    ' Store procedures
    Public Const SPGETRESERVATIONS As String = "spReservations_GetByHotelBeds"
    Public Const SPGETPAIDONLINERESERVATIONS As String = "spReservations_GetPaidOnline"
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

    Function GetIdAsociation() As Integer
        Dim IdAsociation As Integer = 0
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Function CargaAgencias(ByVal data As DataSet) As Boolean
        Dim item As ListItem
        'Dim pos As Integer = ddlAgencias.SelectedIndex

        If Me.QueryType = QueryTypes.PaidOnline Then Return False

        ' ddlAgencias.Items.Clear()
        If Not data Is Nothing AndAlso data.Tables.Count = 2 AndAlso data.Tables(1).Rows.Count > 0 Then



            'ddlAgencias.DataSource = data.Tables(1)
            'ddlAgencias.DataTextField = "agency"
            'ddlAgencias.DataValueField = "id"
            'ddlAgencias.DataBind()

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("00576"), False), "0")
            'ddlAgencias.Items.Insert(0, item)

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("M000624"), False), "-1")
            'ddlAgencias.Items.Insert(0, item)

        Else
            item = New ListItem(PortalCulture.GetString("M000624", False), "0")
            'ddlAgencias.Items.Add(item)
        End If
        'If pos <> -1 Then
        '    ddlAgencias.SelectedIndex = pos
        'End If
    End Function

    Private Function Columnas(ByVal data As DataSet)
        Dim CExport As New CExportExcell

        CExport.AddParameter("TravelerName", "Cliente")
        CExport.AddParameter("TravelerAddress", "Address")
        CExport.AddParameter("TravelerCity", "City")
        CExport.AddParameter("TravelerState", "State")
        CExport.AddParameter("TravelerZip", "Zip")
        CExport.AddParameter("TravelerPhoneHome", "Phone")
        CExport.AddParameter("cliemailcliente", "Phone")
        CExport.AddParameter("NoReservacion|'{0:D}'", "NoReservacion")
        CExport.AddParameter("CheckIn", "Llegada")
        CExport.AddParameter("NombreHabitacion", "Tipo de habitaci&#243;n")
        CExport.AddParameter("IATA", "IATA")
        CExport.AddParameter("Agency", "Agency")
        CExport.AddParameter("earlyOut", "Early Check Out")
        CExport.AddParameter("noshow", "No Show")

        CExport.DataSource = data
        Session("__CExport__") = CExport
    End Function

    Public Function getReservations(ByVal hotelId As Integer, ByVal CheckIn As DateTime, ByVal CheckOut As DateTime, ByVal RoomId As Integer, ByVal Status As Byte, ByVal NameClient As String, ByVal filter As String, Optional ByVal idUsuario As Integer = -2, Optional ByVal Agencia As String = "") As DataView
        'Crear la consulta y traer un reader
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        Dim data As New DataSet

        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = If(Me.QueryType = QueryTypes.PaidOnline, Me.SPGETPAIDONLINERESERVATIONS, Me.SPGETRESERVATIONS)
                    .Connection = New SqlConnection(ConnectionString)
                    .Parameters.Add(New SqlParameter(Me.PRM_ID_HOTEL, SqlDbType.Int)).Value = hotelId

                    If chkDates.Checked Then
                        .Parameters.Add(New SqlParameter(Me.PRM_CHECKIN, SqlDbType.DateTime)).Value = CheckIn
                        .Parameters.Add(New SqlParameter(Me.PRM_CHECKOUT, SqlDbType.DateTime)).Value = CheckOut
                    End If

                    If GetIdAsociation() > 0 Then
                        .Parameters.Add(New SqlParameter(Me.PRM_ASOCIACION, SqlDbType.Int)).Value = GetIdAsociation()
                    End If

                    .Parameters.Add(New SqlParameter(Me.PRM_ID_ROOM_HOTEL, SqlDbType.Int)).Value = RoomId
                    .Parameters.Add(New SqlParameter(Me.PRM_FILTER, SqlDbType.Int)).Value = filter

                    .Parameters.Add(New SqlParameter(Me.PRM_STATUS, SqlDbType.TinyInt)).Value = Status
                    .Parameters.Add(New SqlParameter(Me.PRM_NAME_CLIENT, SqlDbType.NVarChar, 80)).Value = NameClient
                    .Parameters.Add(New SqlParameter(PRM_IDIOMA, SqlDbType.Int)).Value = PortalCulture.GetIDCulture


                    If Not String.IsNullOrEmpty(Agencia) Then
                        .Parameters.Add(New SqlParameter(PRM_AGENCIA, SqlDbType.NVarChar, 80)).Value = Agencia
                        .Parameters.Add(New SqlParameter("@idUsuario", SqlDbType.Int)).Value = idUsuario
                    End If

                End With
                .Fill(data)

            Catch ex As Exception
                Dim exx As String = ex.Message
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

        Dim filtroStr As String = String.Empty
        If DropDownListTransaction.Visible = True Then
            Select Case DropDownListTransaction.SelectedValue
                Case 0
                    filtroStr = ""
                Case 1
                    filtroStr = " pmsAct = 'SS'" 'Reservacion
                Case 2
                    filtroStr = " pmsAct = 'CC'" 'Modificacion
                Case 3
                    filtroStr = " pmsAct = 'XX'" 'Cancelacion
            End Select
        End If

        If Me.ddlCanal.SelectedValue <> "ALL" Then
            'dv.RowFilter = "source='" & ddlCanal.SelectedValue & "'"
            filtroStr = "source='" & ddlCanal.SelectedValue & "'" & IIf(filtroStr <> String.Empty, " and " & filtroStr, "")
        End If

        If filtroStr <> String.Empty Then
            dv.RowFilter = filtroStr
        End If

        hlnkExcel.Enabled = True
        CargaAgencias(data)
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
                    .CommandText = If(Me.QueryType = QueryTypes.PaidOnline, Me.SPGETPAIDONLINERESERVATIONS, Me.SPGETRESERVATIONS)
                    .Connection = New SqlConnection(ConnectionString)

                    If Not Session("welcome_idhotel") Is Nothing AndAlso Session("welcome_idhotel") <> String.Empty Then
                        .Parameters.Add(New SqlParameter(Me.PRM_ID_HOTEL, SqlDbType.Int)).Value = Session("welcome_idhotel")
                    Else
                        .Parameters.Add(New SqlParameter(Me.PRM_ID_HOTEL, SqlDbType.Int)).Value = Me.idHotel
                    End If

                    .Parameters.Add(New SqlParameter(Me.PRM_ID_ROOM_HOTEL, SqlDbType.Int)).Value = 0
                    .Parameters.Add(New SqlParameter(Me.PRM_FILTER, SqlDbType.Int)).Value = "4"

                    If Me.QueryType = QueryTypes.PaidOnline Then
                        If Me.lstFindBy.SelectedIndex = 0 Then
                            .Parameters.Add(New SqlParameter(Me.PRM_NORES, SqlDbType.NVarChar, 24)).Value = Me.txtnoConfirmacion.Text.Trim
                        Else
                            .Parameters.Add(New SqlParameter(Me.PRM_NOAUTH, SqlDbType.NVarChar, 24)).Value = Me.txtnoConfirmacion.Text.Trim
                        End If
                    Else
                        .Parameters.Add(New SqlParameter(If(Me.QueryType = QueryTypes.PaidOnline, Me.PRM_NOAUTH, Me.PRM_NORES), SqlDbType.NVarChar, 24)).Value = Me.txtnoConfirmacion.Text.Trim
                    End If
                    If GetIdAsociation() > 0 Then
                        .Parameters.Add(New SqlParameter(Me.PRM_ASOCIACION, SqlDbType.Int)).Value = GetIdAsociation()
                    End If

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

    Private Sub cvMissingRoomTypes_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMissingRoomTypes.ServerValidate
        If Me.lstRoomType.Items.Count <= 1 Then
            cvMissingRoomTypes.IsValid = False
            args.IsValid = False
        End If
    End Sub

    Public Sub MostrarFiltroPorFiltroDeTransacciones(ByVal Value As Boolean)
        LabelTransaction.Visible = Value
        DropDownListTransaction.Visible = Value
    End Sub

    Private Sub loadResources()
        Me.lblDesde.Text = PortalCulture.GetString("M000110", True)
        Me.lblHasta.Text = PortalCulture.GetString("M000111", True)
        Me.cvMissingRoomTypes.Text = PortalCulture.GetString("M000112")
        Me.lblTipoHab.Text = PortalCulture.GetString("M000113", True)
        Me.lblStatus.Text = PortalCulture.GetString("M000115", True)
        Me.lblNombreCliente.Text = PortalCulture.GetString("M000114", True)
        btnBuscar.Text = PortalCulture.GetString("M000637")
        lblNoconfirmacion.Text = PortalCulture.GetString("00447", True)
        Me.lblSearchSpecific.Text = PortalCulture.GetString("00479")
        Me.lblSearchData.Text = PortalCulture.GetString("00478")
        Me.BtnSpecificSearch.Text = PortalCulture.GetString("M000118")
        Me.chkDates.Text = PortalCulture.GetString("00517")
        Me.lblSearchby.Text = PortalCulture.GetString("M0BT0000113", True)
        lblCanal.Text = PortalCulture.GetString("00670", True)
        LabelTransaction.Text = PortalCulture.GetString("01051", True)
        lblHoteles.Text = PortalCulture.GetString("M0BT0000150", True)
        Me.hplShow.Text = PortalCulture.GetString("01454")
        Me.hplHide.Text = PortalCulture.GetString("01455")
        'lblAgencia.Text = PortalCulture.GetString("M000524", True)
        lblAgency.Text = PortalCulture.GetString("M0BT0000073", True)
        lblAgent.Text = PortalCulture.GetString("00615", True)
        cvlNoConfirmacion.ErrorMessage = PortalCulture.GetString("M000583")
    End Sub

    Sub callFilter()
        Dim sFiltro As String

        sFiltro = PortalCulture.GetString("01380") & ","
        sFiltro &= If(chkDates.Checked, String.Format(PortalCulture.GetString("01370"), ddlFilterby.SelectedItem.Text), " ")
        sFiltro &= If(lstStatus.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01371"), lstStatus.SelectedItem.Text))
        sFiltro &= If(ddlCanal.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01372"), ddlCanal.SelectedItem.Text))
        If ddlHoteles.Items.Count > 0 Then
            sFiltro &= If(ddlHoteles.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01373"), ddlHoteles.SelectedItem.Text))
        End If
        sFiltro &= String.Format(PortalCulture.GetString("01374"), Date1.selectedDate.ToString("dd/MM/yyyy"), Date2.selectedDate.ToString("dd/MM/yyyy"))
        sFiltro &= If(lstRoomType.SelectedIndex = 0, PortalCulture.GetString("01375"), String.Format(PortalCulture.GetString("01376"), lstRoomType.SelectedItem.Text))
        sFiltro &= If(String.IsNullOrEmpty(txtNameCustomer.Text), "", String.Format(PortalCulture.GetString("01377"), txtNameCustomer.Text))
        lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.lstRoomType.Items.Count <= 1 Then
            cvMissingRoomTypes.IsValid = False
        End If
        loadResources()
        If Not Me.UserChain Then
            chkHotels.Visible = False
            lblHoteles.Visible = False
            ddlHoteles.Visible = False
        End If
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

    Private Sub lstRoomType_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstRoomType.PreRender
        Dim Sel As Integer
        Sel = Me.lstRoomType.SelectedIndex
        Me.loadRoomsTypes()
        Me.lstRoomType.Items(0).Text = "-- " & PortalCulture.GetString("M000259") & " --"
        Me.lstRoomType.SelectedIndex = Sel
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
            If TypeOf Me.Page Is ConfirmReservas Then
                Dim DV As DataView = Me.getReservationsByNum()
                If DV.Count > 0 Then
                    RaiseEvent Fillds(DV)
                Else
                    cvlNoConfirmacion.IsValid = False
                End If
            ElseIf Me.QueryType = QueryTypes.PaidOnline Then
                Dim DV As DataView = Me.getReservationsByNum()
                If DV IsNot Nothing AndAlso DV.Count > 0 Then
                    Dim dt As DataTable
                    With New Portal.General.Facade.ReservaFacade
                        dt = .GetDataReservaByNum(DV(0)("NoReservacion"))
                    End With
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        If dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDHOTEL) = Me.idHotel Or Me.Supervisor Then
                            RaiseEvent onSendReservation(dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDRESERVACION))
                        Else
                            cvlNoConfirmacion.IsValid = False
                        End If
                    Else
                        cvlNoConfirmacion.IsValid = False
                    End If

                Else
                    cvlNoConfirmacion.IsValid = False
                End If
            Else
                Dim dt As DataTable
                With New Portal.General.Facade.ReservaFacade
                    dt = .GetDataReservaByNum(txtnoConfirmacion.Text.Trim, 1)
                End With
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    '     If dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).Item(dsReservaciones.FIELD_IDHOTEL) <> Me.cInfoActual.Hotel _
                    'AndAlso Not Me.User.IsInRole("Supervisor") Then
                    '         Response.Redirect( "/Hotel_Administrator/Pages/Reservations.aspx?aut=0")
                    '     End If
                    If dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDHOTEL) = Me.idHotel Or Me.Supervisor Then
                        If Me.GetIdAsociation > 0 Then
                            If Me.GetIdAsociation = dt.Rows(0)("idAsociacion") Then
                                RaiseEvent onSendReservation(dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDRESERVACION))
                            Else
                                cvlNoConfirmacion.IsValid = False
                            End If
                        Else
                            RaiseEvent onSendReservation(dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDRESERVACION))
                        End If


                    ElseIf (dt.Rows(0)("SourceCode").ToString.ToUpper = "HB" And dt.Rows(0)("Source").ToString.ToUpper = "POR" And dt.Rows(0)("idHotel") = 0) And CType(Me.Page, PaginaBase).isUserChain Then
                        Dim idCorp As Integer = CType(Me.Page, PaginaBase).IdCorporativoUserChain
                        If dt.Rows(0)("idCorporativo") = idCorp Then
                            RaiseEvent onSendReservation(dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDRESERVACION))
                        Else
                            cvlNoConfirmacion.IsValid = False
                        End If
                    Else
                        cvlNoConfirmacion.IsValid = False
                    End If
                Else
                    cvlNoConfirmacion.IsValid = False
                End If
            End If
        Else
            cvlNoConfirmacion.IsValid = False
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

            item = New ListItem(String.Format("-- {0} --", PortalCulture.GetString("01620"), False), "-1") 'Todos los Agentes
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

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        cvlNoConfirmacion.IsValid = True
        lblFiltro.Text = String.Format(PortalCulture.GetString(If(Me.QueryType = QueryTypes.PaidOnline, "01401", "01378")), txtnoConfirmacion.Text)
        buscarNumeroReservacion()
    End Sub

    Private Sub BtnSpecificSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSpecificSearch.Click
        cvlNoConfirmacion.IsValid = True
        'If Not (ddlCanal.SelectedIndex = 0 Or ddlCanal.SelectedIndex = 4) Then
        '    ddlAgencias.SelectedIndex = 0
        'End If
        callFilter()
        RaiseEvent loadData(0)
    End Sub

    Private Function getnoconfirmreservations(ByVal AllHotels As Boolean) As DataSet

        Dim CheckIn As DateTime = Date1.selectedDate
        Dim CheckOut As DateTime = Date2.selectedDate
        Dim RoomId As Integer = Me.lstRoomType.SelectedValue
        Dim ds As ReservaDatos
        Dim idAsociac As Integer = Me.GetIdAsociation

        If Supervisor AndAlso (AllHotels Or idHotel = 0) Then
            With New ReservaFacade
                ds = .GetNoConfirmedReservations(CheckIn, CheckOut, Me.ddlFilterby.SelectedItem.Value, idAsociacion:=idAsociac)
            End With
        Else
            If Not Me.UserChain Then
                With New ReservaFacade
                    ds = .GetNoConfirmedReservations(CheckIn, CheckOut, Me.ddlFilterby.SelectedItem.Value, idHotel, idAsociacion:=idAsociac)
                End With
            Else
                If chkHotels.Checked Then
                    'falta pasar el idUsuario
                    With New ReservaFacade
                        ds = .GetNoConfirmedReservations(CheckIn, CheckOut, Me.ddlFilterby.SelectedItem.Value, Me.ddlHoteles.SelectedValue, Me.idUsuario, idAsociacion:=idAsociac)
                    End With
                Else
                    With New ReservaFacade
                        ds = .GetNoConfirmedReservations(CheckIn, CheckOut, Me.ddlFilterby.SelectedItem.Value, idHotel, idAsociacion:=idAsociac)
                    End With
                End If
            End If
        End If
        Return ds

    End Function



End Class