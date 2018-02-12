Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Runtime.Serialization

Partial Class ctrRateAplicationPackage
    Inherits System.Web.UI.UserControl
    Protected WithEvents lstDates As System.Web.UI.WebControls.ListBox
    Protected WithEvents txtFechas As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents txtDateFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDateErrorSign As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents imgAddDate As System.Web.UI.WebControls.Image
    Protected WithEvents imgDeleteDate As System.Web.UI.WebControls.Image
    Protected WithEvents lblDateError As System.Web.UI.WebControls.Label
    Protected WithEvents chk7 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk2 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk3 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk4 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk5 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk6 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPricingNE As System.Web.UI.WebControls.Label
    Protected WithEvents lblPricingExc As System.Web.UI.WebControls.Label
    Public Event GetLstHabId(ByRef habitaciones)

    '    Public Property m_iHotelId() As Integer
    '        Get
    '            Return ViewState("HotelID")
    '        End Get
    '        Set(ByVal Value As Integer)
    '            VIEWSTATE("HotelID") = Value
    '        End Set
    '    End Property
    '    Public Property Adultos() As Integer
    '        Get
    '            Return ViewState("_Adultos")
    '        End Get
    '        Set(ByVal Value As Integer)
    '            VIEWSTATE("_Adultos") = Value
    '        End Set
    '    End Property
    '    Public Property Ninios() As Integer
    '        Get
    '            Return ViewState("_Ninios")
    '        End Get
    '        Set(ByVal Value As Integer)
    '            VIEWSTATE("_Ninios") = Value
    '        End Set
    '    End Property

    '    Public Property RatePlanRow() As RowRatePlan
    '        Get
    '            Return ViewState("_RatePlanRow")
    '        End Get
    '        Set(ByVal Value As RowRatePlan)
    '            VIEWSTATE("_RatePlanRow") = Value
    '        End Set
    '    End Property
    '    Public Property Exceptions() As String
    '        Get
    '            Return viewstate("Exception")
    '        End Get
    '        Set(ByVal Value As String)
    '            viewstate("Exception") = Value
    '        End Set
    '    End Property
    '    Public Property m_Modo() As String
    '        Get
    '            If ViewState("FareModo") Is Nothing Then ViewState("FareModo") = "NINGUNO"
    '            Return ViewState("FareModo")
    '        End Get
    '        Set(ByVal Value As String)
    '            VIEWSTATE("FareModo") = Value
    '        End Set
    '    End Property
    '    Const KEY_FAREID = "FareId"
    '    Public Property m_iFareId() As Integer
    '        Get
    '            Return ViewState(KEY_FAREID)
    '        End Get
    '        Set(ByVal Value As Integer)
    '            ViewState(KEY_FAREID) = Value
    '        End Set
    '    End Property

    '    Public Property IdRatePlan() As String
    '        Get
    '            Return viewstate("_IdRatePlan")
    '        End Get
    '        Set(ByVal Value As String)
    '            viewstate("_IdRatePlan") = Value
    '        End Set
    '    End Property

    '    Private Property idroom() As Integer
    '        Get
    '            Return viewstate("_idRoom")
    '        End Get
    '        Set(ByVal Value As Integer)
    '            viewstate("_idRoom") = Value
    '        End Set
    '    End Property

    '#Region " Web Form Designer Generated Code "

    '    'This call is required by the Web Form Designer.
    '    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    '    End Sub
    '    Protected WithEvents txtFechas As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    '    Protected WithEvents txtDateFrom As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    '    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents lblDateErrorSign As System.Web.UI.WebControls.Label
    '    Protected WithEvents imgAddDate As System.Web.UI.WebControls.Image
    '    Protected WithEvents lstDates As System.Web.UI.WebControls.ListBox
    '    Protected WithEvents imgDeleteDate As System.Web.UI.WebControls.Image
    '    Protected WithEvents lblDateError As System.Web.UI.WebControls.Label
    '    Protected WithEvents hplShowRules As System.Web.UI.WebControls.HyperLink
    '    Protected WithEvents hplHideRules As System.Web.UI.WebControls.HyperLink
    '    Protected WithEvents Chk7 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents Chk1 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents Chk2 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents Chk3 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents Chk4 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents Chk5 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents Chk6 As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents RangeValidator3 As System.Web.UI.WebControls.RangeValidator
    '    Protected WithEvents lblPreciosTarifa As System.Web.UI.WebControls.Label
    '    Protected WithEvents lblMonTar As System.Web.UI.WebControls.Label
    '    Protected WithEvents lblPrecio As System.Web.UI.WebControls.Label
    '    Protected WithEvents txtAdultFare As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    '    Protected WithEvents RVPrecio As System.Web.UI.WebControls.RegularExpressionValidator
    '    Protected WithEvents LblChildPrice As System.Web.UI.WebControls.Label
    '    Protected WithEvents txtChildFare As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    '    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    '    Protected WithEvents lblExtraAdult As System.Web.UI.WebControls.Label
    '    Protected WithEvents txtExtraAdultPrice As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents valAdultExtraPrice As System.Web.UI.WebControls.RegularExpressionValidator
    '    Protected WithEvents lblExtraChildPrice As System.Web.UI.WebControls.Label
    '    Protected WithEvents txtExtraChildPrice As System.Web.UI.WebControls.TextBox
    '    Protected WithEvents valExtraChildPrice As System.Web.UI.WebControls.RegularExpressionValidator
    '    Protected WithEvents DivRules As System.Web.UI.HtmlControls.HtmlGenericControl
    '    Protected WithEvents ddlrateplans99 As System.Web.UI.WebControls.DropDownList
    '    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    '    Protected WithEvents lblEName As System.Web.UI.WebControls.Label
    '    Protected WithEvents ddlRooms As System.Web.UI.WebControls.DropDownList
    '    Protected WithEvents btnLoad As System.Web.UI.WebControls.Button
    '    Protected WithEvents dgRooms As System.Web.UI.WebControls.DataGrid

    '    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    '    'Do not delete or move it.
    '    Private designerPlaceholderDeclaration As System.Object

    '    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    '        'CODEGEN: This method call is required by the Web Form Designer
    '        'Do not modify it using the code editor.
    '        InitializeComponent()
    '    End Sub

    '#End Region

    '    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '        Me.lblDateError.Visible = False
    '        If Not IsPostBack Then
    '            ';hplShowRules.Style.Add("display", "")
    '            ';hplHideRules.Style.Add("display", "none")
    '            loadDatos()
    '        End If

    '        If TypeOf Me.Page Is Package Then
    '            Me.txtChildFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, Package).valorIdDgChild & "','txtChildrenFare'" & ",'" & Me.txtChildFare.ClientID & "')")
    '            Me.txtAdultFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, Package).valorIdDgAdult & "','txtAdultFare'" & ",'" & Me.txtAdultFare.ClientID & "')")
    '        End If

    '        imgAddDate.Attributes.Add("onclick", "javascript:AddDate('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');")
    '        imgDeleteDate.Attributes.Add("onclick", "javascript:DeleteDate('" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "');")
    '        If Not IsPostBack Then

    '            idroom = Request.QueryString("Room")
    '            If idroom <> 0 Then
    '                Me.ddlRooms.SelectedValue = idroom
    '            Else
    '                Try
    '                    idroom = ddlRooms.SelectedValue
    '                Catch ex As Exception
    '                    idroom = 0
    '                End Try
    '            End If
    '            btnLoad_Click(sender, e)

    '        End If
    '    End Sub
    '    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        buscarTarifas()
    '    End Sub

    '    Public Sub buscarTarifas()
    '        Try
    '            idroom = ddlRooms.SelectedValue
    '            room = ddlRooms.SelectedItem.Text
    '        Catch ex As Exception
    '            idroom = 0
    '        End Try
    '        dgRooms.CurrentPageIndex = 0
    '        dgRooms.SelectedIndex = -1
    '        Dim ci As System.Globalization.CultureInfo
    '        ci = System.Threading.Thread.CurrentThread.CurrentCulture
    '        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
    '        Me.dgRooms.DataSource = GetRoomFares()
    '        Me.dgRooms.DataBind()
    '        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    '        newFare()

    '        renglonTarifa("", "none", "", "")
    '    End Sub

    '    Private Function GetRoomFares() As DataView
    '        Dim datFares As FaresData
    '        If Me.idroom <> 0 Then
    '            With New FaresSystem
    '                datFares = .GetFaresByRoomTypeId(CInt(Me.idroom), PortalCulture.GetIDCulture)
    '            End With
    '            datFares.Tables(FaresData.FARES_TABLE).Columns.Add("MaxPrice", GetType(System.Double))
    '            datFares.Tables(FaresData.FARES_TABLE).Columns.Add("MinPrice", GetType(System.Double))

    '            Dim dvFares As DataView
    '            dvFares = datFares.Tables(datFares.FARES_TABLE).DefaultView

    '            'dvFares.RowFilter = " tipotarifa <>'K' "

    '            dvFares.RowFilter &= datFares.IDRATEPLAN_FIELD & "='" & Me.idRateCode & "'"

    '            Dim dv As New DataView
    '            For Each dvr As DataRowView In dvFares
    '                dv = datFares.Tables(1).DefaultView
    '                dv.RowFilter = FaresData.PKIDFARES_FIELD & "=" & dvr(FaresData.PKIDFARES_FIELD)
    '                If dv.Count > 0 Then
    '                    dvr("MinPrice") = dv(0)(KEY_MINPRICE)
    '                    dvr("MaxPrice") = dv(0)(KEY_MAXPRICE)
    '                Else
    '                    dvr("MinPrice") = dvr(FaresData.PRICE_FIELD)
    '                    dvr("MaxPrice") = dvr(FaresData.PRICE_FIELD)
    '                End If
    '            Next

    '            Return dvFares
    '        Else
    '            Return Nothing
    '        End If

    '    End Function
    '    Public Sub filterRac()
    '        Dim ds As RatePlanData
    '        With New RatePlanFacade
    '            ds = .GetRatePlanByIdHotel(m_iHotelId)
    '        End With
    '        Me.RatePlanRow = Nothing
    '        Dim dv As DataView
    '        dv = ds.Tables(ds.RATEPLAN_TABLE).DefaultView
    '        dv.RowFilter = ds.FIELD_SegmentRacPrinc & "=1 and " & ds.FIELD_SEGMENT & "='R'"
    '    End Sub

    '    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '        loadCulture()
    '    End Sub

    '    ';Private Function GetArrivosField() As String
    '    ';    Dim NoArr As String = ""
    '    ';    Dim ck As CheckBox
    '    ';    Dim cheked As String
    '    ';    For i As Integer = 1 To 7
    '    ';        ck = Me.FindControl("Chk" & i)
    '    ';        cheked = "Y"
    '    ';        If Not ck.Checked Then cheked = "N"
    '    ';        NoArr &= cheked
    '    ';    Next
    '    ';    Return NoArr
    '    ';End Function

    '    Private Sub loadDatos()
    '        Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
    '        Me.txtDateTo.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
    '        Dim ds As RatePlanData
    '        With New RatePlanFacade
    '            ds = .GetRatePlanByIdHotel(m_iHotelId)
    '        End With
    '        Me.RatePlanRow = Nothing
    '        Dim dv As DataView
    '        dv = ds.Tables(ds.RATEPLAN_TABLE).DefaultView
    '        dv.RowFilter = ds.FIELD_SegmentRacPrinc & "=1 and " & ds.FIELD_SEGMENT & "='R'"
    '        'si hay tarifa rack la tarifa se enlazará con dicha tarifa, 
    '        If dv.Count > 0 Then
    '            Me.RatePlanRow = New RowRatePlan
    '            Me.RatePlanRow.IDRATEPLAN = dv(0)(ds.FIELD_IDRATEPLAN)
    '            Me.RatePlanRow.SEGMENT = dv(0)(ds.FIELD_SEGMENT)
    '            Me.RatePlanRow.RATECODE = dv(0)(ds.FIELD_CODIGOTARIFA)
    '        Else
    '            Me.RatePlanRow = Nothing
    '            If SaveSegmentRac() Then
    '                loadDatos()
    '                loadAllRatesplans()

    '                If TypeOf Me.Page Is Package Then
    '                    CType(Me.Page, Package).loadDatos()
    '                End If
    '            Else
    '                Response.Redirect( "/Pages/RatesPlans.aspx")
    '            End If
    '        End If
    '    End Sub

    '    Private Sub loadCulture()
    '        Me.lblStartDate.Text = PortalCulture.GetString("00108", True)
    '        Me.lblEndDate.Text = PortalCulture.GetString("00109", True)
    '        Me.lblDateErrorSign.Text = PortalCulture.GetString("00112")
    '        Me.lblPreciosTarifa.Text = PortalCulture.GetString("00131")

    '        Dim dsHotel As HotelDatos
    '        With New HotelSistema
    '            dsHotel = .GetHotelById(m_iHotelId)
    '        End With
    '        Dim strIncTax As String

    '        Response.Write("<script> var updateSeasson='" & PortalCulture.GetString("00609") & "';</script>")

    '        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
    '            lblMonTar.Text = PortalCulture.GetString("M000263") & " " & dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("Codigo")
    '            strIncTax = " " & PortalCulture.GetString("00610") & " "

    '            If Not dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).IsNull(dsHotel.FIELD_PLUSTAX) Then
    '                If dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)(dsHotel.FIELD_PLUSTAX) Then
    '                    strIncTax = " " & PortalCulture.GetString("00611") & " "
    '                End If
    '            End If

    '            If dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("Codigo") = "MXN" Then
    '                strIncTax &= "<BR> " & PortalCulture.GetString("00667")
    '            End If

    '            lblMonTar.Text &= strIncTax

    '        End If

    '        Me.lblExtraAdult.Text = PortalCulture.GetString("M000226", True)
    '        Me.lblExtraChildPrice.Text = PortalCulture.GetString("M000227", True)
    '        Me.valAdultExtraPrice.Text = PortalCulture.GetString("00116")
    '        Me.valExtraChildPrice.Text = PortalCulture.GetString("00116")
    '        RVPrecio.Text = PortalCulture.GetString("00116")
    '        Regularexpressionvalidator1.Text = PortalCulture.GetString("00116")
    '        lblPrecio.Text = PortalCulture.GetString("00087", True)
    '        Me.lblDateError.Text = PortalCulture.GetString("00137")
    '        Me.LblChildPrice.Text = PortalCulture.GetString("00088", True)
    '        lstDates.Items.Clear()
    '        Me.lstDates.Items.Add(PortalCulture.GetString("00317", True))
    '        For i As Integer = 1 To lstDatesCount()
    '            Dim it As String = lstDatesItemI(i)
    '            Me.lstDates.Items.Add(it)
    '        Next
    '    End Sub

    '    Public Function loadAllRatesplans() As RatePlanData
    '        Dim ds As RatePlanData

    '        With New RatePlanFacade
    '            ds = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture)
    '        End With

    '        Dim links As New LinkRatePlanData
    '        With New LinkRatePlanFacade
    '            links = .getList(Me.m_iHotelId, PortalCulture.GetIDCulture)
    '        End With
    '        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & ds.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & ds.FIELD_NAME & ",1,25)")
    '        ''eliminar los ratesplan que ya tienen links
    '        Dim dv As DataView
    '        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
    '            dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
    '            dv.RowFilter = links.FIELD_TargetRatePlan & "='" & r(ds.FIELD_IDRATEPLAN) & "'"
    '            If dv.Count > 0 Then 'OrElse r(ds.FIELD_SEGMENT) = "K" Then
    '                r.Delete()
    '            End If
    '        Next
    '        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()

    '        Return ds
    '    End Function

    '    Private Function SaveSegmentRac() As Boolean
    '        'crear el segmento
    '        Dim dsRate As New RatePlanData
    '        Dim rRate As DataRow
    '        Dim val As Boolean
    '        Dim idRate, NuevoIdRate As String
    '        rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
    '        With rRate
    '            .Item(dsRate.FIELD_IDRATEPLAN) = "RAC"
    '            .Item(dsRate.FIELD_DESCRIPTION) = "Only Room"
    '            .Item(dsRate.FIELD_SEGMENT) = "R"
    '            .Item(dsRate.FIELD_IDHOTEL) = Me.m_iHotelId
    '            .Item(dsRate.FIELD_CODIGOTARIFA) = "RAC"
    '            .Item(dsRate.FIELD_NAME) = "Standard Rate"
    '        End With
    '        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)
    '        Dim idDic, iddic2 As Integer
    '        Dim txtDescripcion As New CtrlIdioma
    '        With New RatePlanAccess
    '            If .InsertRtPlan(dsRate, idDic, iddic2) Then
    '                txtDescripcion.Update("Only Room", "Solo Habitación", idDic)
    '            Else
    '                Return False
    '            End If
    '        End With
    '        Me.RatePlanRow = New RowRatePlan
    '        With dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)
    '            Me.RatePlanRow.IDRATEPLAN = .Item(RatePlanData.FIELD_IDRATEPLAN)
    '            Me.RatePlanRow.SEGMENT = .Item(RatePlanData.FIELD_SEGMENT)
    '        End With
    '        Return True
    '    End Function

    '    Public Function SaveNewFare(ByVal idRoom As Integer, ByRef idtar As Integer, ByVal f1 As Date, ByVal f2 As Date) As Boolean
    '        Dim datFare As New FaresData
    '        Dim ExistCode As New FaresData
    '        Dim rowFare As DataRow
    '        Dim dv As DataView
    '        If RatePlanRow Is Nothing Then
    '            If Not SaveSegmentRac() Then
    '                Return False
    '            End If
    '        End If
    '        'buscar el codigo que le pertenece
    '        Dim room As RoomsHotelData
    '        With New RoomFacade
    '            room = .getRoomByID(idRoom)
    '        End With
    '        If room.Tables(room.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
    '        Try
    '            With datFare.Tables(FaresData.FARES_TABLE)
    '                rowFare = .NewRow()
    '                rowFare(FaresData.ENDDATE_FIELD) = f2
    '                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = CDbl(Val(txtExtraAdultPrice.Text))
    '                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = CDbl(Val(txtExtraChildPrice.Text))
    '                rowFare(FaresData.PRICE_FIELD) = CDbl(Me.txtAdultFare.Text)
    '                rowFare(FaresData.NINIORATE) = CDbl(Me.txtChildFare.Text)
    '                rowFare(FaresData.STARTDATE_FIELD) = f1
    '                rowFare(FaresData.EXCEPTION_FIELD) = Me.Exceptions
    '                rowFare(FaresData.RULESDEFAULT) = False
    '                rowFare(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
    '                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idRoom
    '                rowFare(FaresData.RATETYPE_FIELD) = RatePlanRow.SEGMENT
    '                rowFare(FaresData.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
    '                rowFare(FaresData.RATECODE_FIELD) = room.Tables(room.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ROOM_CODE) & RatePlanRow.RATECODE
    '                .Rows.Add(rowFare)
    '            End With

    '            With New FaresSystem
    '                Try
    '                    If .InsertFares(datFare) Then
    '                        idtar = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PKIDFARES_FIELD)
    '                        'esta condición es para cuando se autollenaran las tarifasrestricciones
    '                        If Me.Adultos > 0 Then
    '                            If Not SaveFaresRestrictions(idtar) Then Return False
    '                        End If
    '                    End If
    '                Catch ex As OverflowException
    '                    Me.lblDateError.Visible = True
    '                    Return False
    '                End Try
    '            End With
    '        Catch ex As Exception
    '            Return False
    '        End Try
    '        Return True
    '    End Function

    '    Private Function UpdateFare(ByVal idroom As Integer, ByVal f1 As Date, ByVal f2 As Date) As Boolean
    '        Dim datFares As New FaresData
    '        Dim ExistCode As New FaresData
    '        Dim fareRow As DataRow
    '        Dim bResult As Boolean
    '        Me.lblDateError.Visible = False
    '        With datFares
    '            fareRow = .Tables(.FARES_TABLE).NewRow()
    '            Try
    '                ' try to fill fare row data
    '                fareRow(.PKIDFARES_FIELD) = Me.m_iFareId
    '                fareRow(.ENDDATE_FIELD) = Format(f2, "yyyy/MM/dd")
    '                fareRow(.EXTRAADULTPRICE_FIELD) = Double.Parse(Me.txtExtraAdultPrice.Text)
    '                fareRow(.EXTRACHILDPRICE_FIELD) = Double.Parse(Me.txtExtraChildPrice.Text)
    '                fareRow(FaresData.EXCEPTION_FIELD) = Me.Exceptions
    '                fareRow(.HOTELROOMTYPEID_FIELD) = idroom
    '                fareRow(.PRICE_FIELD) = Double.Parse(Me.txtAdultFare.Text)
    '                fareRow(.NINIORATE) = CDbl(Me.txtChildFare.Text)
    '                fareRow(.STARTDATE_FIELD) = Format(CDate(f1), "yyyy/MM/dd")
    '                fareRow(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
    '                fareRow(FaresData.RULESDEFAULT) = False
    '                fareRow(FaresData.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
    '                .Tables(.FARES_TABLE).Rows.Add(fareRow)
    '                ' set row state to modified
    '                fareRow.AcceptChanges()
    '                fareRow(.PKIDFARES_FIELD) = fareRow(.PKIDFARES_FIELD)
    '                With New FaresSystem
    '                    Try
    '                        bResult = .ActualizaFares(datFares)
    '                    Catch ex As OverflowException
    '                        Me.lblDateError.Visible = True
    '                        Return False
    '                    End Try
    '                End With
    '                If bResult = True Then
    '                    Me.m_iFareId = .Tables(.FARES_TABLE).Rows(0)(.PKIDFARES_FIELD)
    '                End If
    '            Catch ex As Exception
    '                Return False
    '            End Try
    '        End With
    '        Return True
    '    End Function

    '    Private Function SaveFaresRestrictions(ByVal idTarifa As Integer) As Boolean
    '        Dim datRestrictions As New FaresRestrictionsData
    '        For idxAdults As Integer = 1 To Me.Adultos
    '            For idxChild As Integer = 0 To Me.Ninios
    '                'Combinaciond de adultos - niños
    '                Dim newRow As DataRow = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).NewRow()
    '                With newRow
    '                    .Item(FaresRestrictionsData.ADULTFARE_FIELD) = CDbl(Me.txtAdultFare.Text)
    '                    .Item(FaresRestrictionsData.ADULTNUMBER_FIELD) = idxAdults
    '                    .Item(FaresRestrictionsData.CHILDFARE_FIELD) = CDbl(Me.txtChildFare.Text)
    '                    .Item(FaresRestrictionsData.CHILDNUMBER_FIELD) = idxChild
    '                    .Item(FaresRestrictionsData.IDFARE_FIELD) = idTarifa
    '                    .Item(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = 0
    '                End With
    '                datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
    '            Next
    '        Next

    '        With New FaresRestrictionSystem
    '            SaveFaresRestrictions = .InsertFaresRestrictions(datRestrictions)
    '        End With
    '    End Function

    '    Public Sub cleardata()
    '        txtDateFrom.Text = ""
    '        txtDateTo.Text = ""
    '        txtAdultFare.Text = 0
    '        txtChildFare.Text = 0
    '        txtExtraAdultPrice.Text = ""
    '        txtExtraChildPrice.Text = ""
    '        Me.lstDates.Items.Clear()
    '        lstDates.Items.Add(PortalCulture.GetString("00317", True))
    '        Me.txtFechas.Text = ""
    '        Me.lblDateError.Visible = False
    '    End Sub

    '    '--------------------------------------------------------------
    '    'Esto es utilizado cuando la tarifa ya existe
    '    Public Sub LoadFare(ByVal iFareId As Integer, Optional ByVal load As Integer = 1)

    '        Dim datFares As DataSet
    '        Dim rowFare As DataRow

    '        m_iFareId = iFareId
    '        ' get fare information
    '        With New FaresSystem
    '            datFares = .GetFareByFareId(iFareId)
    '        End With
    '        cleardata()

    '        If Not datFares Is Nothing AndAlso datFares.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then
    '            rowFare = datFares.Tables(FaresData.FARES_TABLE).Rows(0)

    '            'ºIf Not rowFare(FaresData.SEGMENTPRINCIPAL) Is System.DBNull.Value AndAlso rowFare(FaresData.SEGMENTPRINCIPAL) = True Then
    '            'ºMe.ddlrateplans.Enabled = False
    '            'ºEnd If

    '            Me.txtDateFrom.Text = CType(rowFare(FaresData.STARTDATE_FIELD), DateTime).ToString("MM/dd/yyyy")
    '            Me.txtDateTo.Text = CType(rowFare(FaresData.ENDDATE_FIELD), DateTime).ToString("MM/dd/yyyy")
    '            Me.txtAdultFare.Text = CDbl(rowFare(FaresData.PRICE_FIELD))
    '            Me.txtChildFare.Text = CDbl(Val(rowFare(FaresData.NINIORATE).ToString))
    '            Me.txtExtraAdultPrice.Text = CDbl(Val(rowFare(FaresData.EXTRAADULTPRICE_FIELD)))
    '            Me.txtExtraChildPrice.Text = CDbl(Val(rowFare(FaresData.EXTRACHILDPRICE_FIELD)))
    '            Me.Exceptions = rowFare(FaresData.EXCEPTION_FIELD)
    '            Me.m_Modo = "MODIFY"

    '            'Me.Page.RegisterStartupScript("", "<script>AddDate('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');</script>")
    '        End If
    '    End Sub

    '    Private Sub getNoArrrivalsField(ByVal Field As String)
    '        Dim ck As CheckBox
    '        For i As Integer = 1 To 7
    '            ck = FindControl("Chk" & i)
    '            ck.Checked = False
    '            Try
    '                If Field.Substring(i - 1, 1) = "Y" Then
    '                    ck.Checked = True
    '                End If
    '            Catch ex As Exception
    '            End Try
    '        Next
    '    End Sub

    '    Public Sub newFare()
    '        cleardata()
    '        SetDefaultValues()
    '        Me.m_Modo = "NEW"
    '        Me.m_iFareId = 0
    '    End Sub

    '    Private Sub SetDefaultValues()
    '        ' Set default fare values of input boxes if no values entered
    '        If Me.txtDateFrom.Text.Trim.Length = 0 Then
    '            Me.txtDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
    '        End If
    '        If Me.txtDateTo.Text.Trim.Length = 0 Then
    '            Me.txtDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
    '        End If
    '        If Me.txtExtraAdultPrice.Text.Trim.Length = 0 Then
    '            Me.txtExtraAdultPrice.Text = "0"
    '        End If
    '        If Me.txtExtraChildPrice.Text.Trim.Length = 0 Then
    '            Me.txtExtraChildPrice.Text = "0"
    '        End If
    '    End Sub

    '    Public Function AddFare(ByVal idRoom As Integer, ByRef idFare As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String, ByVal rp As String, ByVal f1last As String, ByVal f2last As String) As Boolean
    '        If Not Page.IsValid Then
    '            Return False
    '        End If
    '        SetDefaultValues()
    '        Dim ds As RatePlanData
    '        With New RatePlanFacade
    '            ds = .GetDataRatePlan(Me.IdRatePlan, Me.m_iHotelId)

    '        End With
    '        If ds.Tables(ds.RATEPLAN_TABLE).Rows.Count > 0 Then
    '            With ds.Tables(ds.RATEPLAN_TABLE).Rows(0)
    '                Try
    '                    Me.RatePlanRow = New RowRatePlan
    '                    Me.RatePlanRow.IDRATEPLAN = .Item(ds.FIELD_IDRATEPLAN)
    '                    Me.RatePlanRow.SEGMENT = .Item(ds.FIELD_SEGMENT)
    '                    Me.RatePlanRow.RATECODE = .Item(ds.FIELD_CODIGOTARIFA)
    '                Catch ex As Exception
    '                    Return False
    '                End Try
    '            End With
    '        End If
    '        If Me.m_iFareId = 0 Then
    '            If SaveNewFare(idRoom, idFare, f1, f2) Then
    '                CType(Me.Page, PaginaBase).guardalog("/Pages/Package.aspx", PaginaBase.acciones.Crear, "Se creó la tarifa de la habitación " & ch.Substring(0, ch.IndexOf("--")) & " de la fecha " & f1 & " a la fecha " & f2 & " con el rateplan " & Me.RatePlanRow.RATECODE)
    '                Return True
    '            End If
    '            Return False
    '        Else
    '            If UpdateFare(idRoom, f1, f2) Then
    '                CType(Me.Page, PaginaBase).guardalog("/Pages/Package.aspx", PaginaBase.acciones.Modificar, "Se modificó la tarifa de la habitación " & ch & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.RatePlanRow.RATECODE)
    '                Return True
    '            End If
    '            Return False
    '            m_iFareId = 0
    '        End If
    '        Return False
    '    End Function

    '    Private Sub imgAddDate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '        Dim fec1 As Date = CDate(txtDateFrom.Text)
    '        Dim fec2 As Date = CDate(Me.txtDateTo.Text)
    '        Dim sw As Boolean = False
    '        For i As Integer = 1 To lstDates.Items.Count - 1
    '            Dim f1, f2 As Date
    '            f1 = CDate(lstDates.Items(i).ToString.Split("-")(0))
    '            f2 = CDate(lstDates.Items(i).ToString.Split("-")(1))
    '            If (((fec1 >= f1 And fec1 <= f2) Or ((fec2 >= f1) And fec2 <= f2)) Or ((f1 >= fec1 And f1 <= fec2) Or ((f2 >= fec1) And f2 <= fec2))) Then
    '                sw = True
    '                Exit For
    '            End If
    '        Next
    '        If Not sw Then
    '            Dim it As String = fec1 & "-" & fec2
    '            Me.lstDates.Items.Add(it)
    '        End If
    '    End Sub

    '    Public Function lstDatesCount() As Integer
    '        Return Me.txtFechas.Text.Split("$").Length() - 1
    '    End Function

    '    Public Function lstDatesItemI(ByVal i As Integer) As String
    '        Return Me.txtFechas.Text.Split("$").GetValue(i)
    '    End Function

    '    Public Function lstDatesAdd()
    '        Me.txtFechas.Text = "$" & txtDateFrom.Text & "-" & Me.txtDateTo.Text
    '    End Function

    '    'Public Function Segment(ByVal Value As String) As String
    '    '    Try
    '    '        If Value <> "0" Then
    '    '            'ºMe.ddlrateplans.SelectedValue = Value
    '    '        End If
    '    '    Catch ex As Exception
    '    '    End Try
    '    'End Function

    Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack Then

        End If
        
    End Sub
   
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
 
    End Sub
   
End Class
