Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Runtime.Serialization
Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports System.Xml

Imports System.Web.UI.Page

Partial Public Class ctrRateAplicationExc
    Inherits UserControlBase

    Public Property m_iHotelId() As Integer
        Get
            Return ViewState("HotelID")
        End Get
        Set(ByVal Value As Integer)
            VIEWSTATE("HotelID") = Value
        End Set
    End Property
    Public Property Adultos() As Integer
        Get
            Return ViewState("_Adultos")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_Adultos") = Value
        End Set
    End Property
    Public Property Ninios() As Integer
        Get
            Return ViewState("_Ninios")
        End Get
        Set(ByVal Value As Integer)
            VIEWSTATE("_Ninios") = Value
        End Set
    End Property

    Public Property RatePlanRow() As RowRatePlan
        Get
            Return ViewState("_RatePlanRow")
        End Get
        Set(ByVal Value As RowRatePlan)
            VIEWSTATE("_RatePlanRow") = Value
        End Set
    End Property
    Public Property Exceptions() As String
        Get
            Return viewstate("Exception")
        End Get
        Set(ByVal Value As String)
            viewstate("Exception") = Value
        End Set
    End Property
    Public Property m_Modo() As String
        Get
            If ViewState("FareModo") Is Nothing Then ViewState("FareModo") = "NINGUNO"
            Return ViewState("FareModo")
        End Get
        Set(ByVal Value As String)
            VIEWSTATE("FareModo") = Value
        End Set
    End Property
    Const KEY_FAREID As String = "FareId"
    Const KEY_STARTDATE_FAREID As String = "StartDateFareId"
    Const KEY_ENDDATE_FAREID As String = "EndDateFareId"
    Public Property m_iFareId() As Integer
        Get
            Return ViewState(KEY_FAREID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_FAREID) = Value
        End Set
    End Property

    Public Property m_StartDateFareId() As Date
        Get
            Return ViewState(KEY_STARTDATE_FAREID)
        End Get
        Set(value As Date)
            ViewState(KEY_STARTDATE_FAREID) = value
        End Set
    End Property

    Public Property m_EndDateFareId() As Date
        Get
            Return ViewState(KEY_ENDDATE_FAREID)
        End Get
        Set(value As Date)
            ViewState(KEY_ENDDATE_FAREID) = value
        End Set
    End Property



    Public Function GetClientID() As String
        Return Me.lstDates.ClientID
    End Function


    'Public Property SourceRateName() As String
    '    Get
    '        Return viewstate("_SRN")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SRN") = Value
    '        'Me.ddlrateplans.Attributes.Add("onChange", "javascript:showRatePlan2('" & Me.ddlrateplans.ClientID & "','" & Me.SourceRateName & "','" & lbldescrateplan.ClientID & "')")
    '    End Set
    'End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lCal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lCal2 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblfechas As System.Web.UI.WebControls.Label

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private CurrencyScript As New StringBuilder

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Public Function GetFareFor(ByVal target As String, ByVal isNetRate As Boolean) As Double
        Dim input As TextBox = Me.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", ""))
        Dim value As Double = 0
        If input IsNot Nothing Then Double.TryParse(input.Text, value)
        Return value
    End Function

    Sub CtrlExtras()
        'If lstPeoplesExtras.Items.Count > 2 Then
        '    Me.txtExtraAdultPrice.Text = ""
        '    Me.txtExtraChildPrice.Text = ""
        '    Me.txtExtraTeenPrice.Text = ""
        'Else
        'End If
        Me.txtExtraAdultPrice.Text = ""
        Me.txtExtraChildPrice.Text = ""
        Me.txtExtraTeenPrice.Text = ""

        txtExtraAdultPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraChildPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraTeenPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)

        Me.reqExtraAdultPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        Me.reqExtraChildPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        Me.reqExtraTeenPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)

    End Sub

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Public Sub cleardata()
        Me.HasData = False
        Me.txtPromoDescription.Limpia()
        txtDateFrom.Text = ""
        txtDateTo.Text = ""
        txtAdvBooking.Text = ""
        txtMaxAdvBooking.Text = ""
        txtMinDias.Text = ""
        txtMaxDias.Text = ""
        txtAdultFare.Text = ""
        txtChildFare.Text = ""
        txtTeenFare.Text = ""
        txtDaysFree.Text = ""
        CtrlExtras()

        Dim ck As CheckBox
        For i As Integer = 1 To 7
            ck = Me.FindControl("Chk" & i)
            ck.Checked = False
        Next
        For i As Integer = 1 To 7
            ck = Me.FindControl("ChkApp" & i)
            ck.Checked = True
        Next
        Me.ddlrateplans.Enabled = True

        Me.lstDates.Items.Clear()
        lstDates.Items.Add(PortalCulture.GetString("00317", True))
        Me.txtFechas.Text = ""
        Me.lblDateError.Visible = False
        chkRules.Checked = True

        txtDescProm.Text = ""
        chkBookingWindow.Checked = False
        txtBookWindowDateFrom.Text = ""
        txtBookWindowDateTo.Text = ""
        lstPeoplesInRoom.SelectedIndex = 0
        lstPeoplesExtras.SelectedIndex = 0
        lstMinNumberAdults.SelectedIndex = 0
        lstNumberAdults.SelectedIndex = 0
        lstNumberChildrens.SelectedIndex = 0
        ddlShowRates.SelectedIndex = 0

        If Me.txtBookWindowDateFrom.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        If Me.txtBookWindowDateTo.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        ShowWindows(hplShowProWin, hplHideProWin, DivPromotionAndWindow, False)
        ShowWindows(hplShowVentanaReserva, hplHideVentanaReserva, DivWindowBooking, False)
        ShowWindows(hplShowOccupation, hplHideOccupation, DivOccupation, False)
        ShowWindows(hplShowRules, hplHideRules, DivRules, False)

    End Sub

    Sub LoadRatesSelect()
        ddlShowRates.Items.Clear()
        ddlShowRates.Items.Add(PortalCulture.GetString("01368"))
        ddlShowRates.Items.Add(PortalCulture.GetString("01369"))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Introducir aquí el código de usuario para inicializar la página
        Me.lblDateError.Visible = False
        If Not IsPostBack Then

            ShowWindows(hplShowProWin, hplHideProWin, DivPromotionAndWindow, False)
            ShowWindows(hplShowVentanaReserva, hplHideVentanaReserva, DivWindowBooking, False)
            ShowWindows(hplShowOccupation, hplHideOccupation, DivOccupation, False)
            ShowWindows(hplShowRules, hplHideRules, DivRules, False)
            loadDatos()
            LoadRatesSelect()
        Else
            Me.reqAdultFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
            Me.reqChildFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
            Me.reqTeenFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
        End If

        If Not CType(Me.Page, PaginaBase).isConfigAdolescente Then
            lblExtraAdolescente.Visible = False
            txtExtraTeenPrice.Visible = False
            txtTeenFare.Visible = False
            lblAdolescentePrice.Visible = False

            reqTeenFare.Visible = False
            reqExtraTeenPrice.Visible = False


        End If

        If TypeOf Me.Page Is FaresCataloguePromo Then
            Me.txtChildFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCataloguePromo).IdDgChild & "','txtChildrenFare'" & ",'" & Me.txtChildFare.ClientID & "')")
            Me.txtAdultFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCataloguePromo).IdDgAdult & "','txtAdultFare'" & ",'" & Me.txtAdultFare.ClientID & "')")
            Me.txtTeenFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCataloguePromo).IdDgTeen & "','txtTeenFare'" & ",'" & Me.txtTeenFare.ClientID & "')")
        End If
        imgAddDate.Attributes.Add("onclick", "javascript:AddDate('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');")
        imgDeleteDate.Attributes.Add("onclick", "javascript:DeleteDate('" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "');")
        Me.hplHideRules.NavigateUrl = "javascript:OcultarRules('0" & "','" & DivRules.ClientID & "','" & hplShowRules.ClientID & "','" & hplHideRules.ClientID & "');"
        Me.hplShowRules.NavigateUrl = "javascript:OcultarRules('1" & "','" & DivRules.ClientID & "','" & hplShowRules.ClientID & "','" & hplHideRules.ClientID & "');"

        'Ocupacion
        Me.hplHideProWin.NavigateUrl = "javascript:OcultarRules('0" & "','" & DivPromotionAndWindow.ClientID & "','" & hplShowProWin.ClientID & "','" & hplHideProWin.ClientID & "');"
        Me.hplShowProWin.NavigateUrl = "javascript:OcultarRules('1" & "','" & DivPromotionAndWindow.ClientID & "','" & hplShowProWin.ClientID & "','" & hplHideProWin.ClientID & "');"

        '//Reserva
        Me.hplHideVentanaReserva.NavigateUrl = "javascript:OcultarRules('0" & "','" & DivWindowBooking.ClientID & "','" & hplShowVentanaReserva.ClientID & "','" & hplHideVentanaReserva.ClientID & "');"
        Me.hplShowVentanaReserva.NavigateUrl = "javascript:OcultarRules('1" & "','" & DivWindowBooking.ClientID & "','" & hplShowVentanaReserva.ClientID & "','" & hplHideVentanaReserva.ClientID & "');"


        'DivOccupation
        Me.hplHideOccupation.NavigateUrl = "javascript:OcultarRules('0" & "','" & DivOccupation.ClientID & "','" & hplShowOccupation.ClientID & "','" & hplHideOccupation.ClientID & "');"
        Me.hplShowOccupation.NavigateUrl = "javascript:OcultarRules('1" & "','" & DivOccupation.ClientID & "','" & hplShowOccupation.ClientID & "','" & hplHideOccupation.ClientID & "');"

        'chk
        chkBookingWindow.Attributes("onclick") = String.Format("javascript:onCheckBoxClick('{0}', '{1}');", chkBookingWindow.ClientID, tbBookingWindow.ClientID)
        tbBookingWindow.Style("display") = IIf(chkBookingWindow.Checked, "", "none")
        'Me.ddlShowRates.Attributes("onchange") = String.Format("javascript:FireShowSelectRates('{0}','{1}','{2}');", Me.ddlShowRates.ClientID, Me.pnlTarifas.ClientID, "DivRates")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()

        GenerateScriptCurrency()
        If CurrencyScript.ToString.Trim.Length > 0 Then
            spArrayScriptContainer.InnerHtml += String.Format("<script type='text/javascript' language='javascript'>{0}</script>", CurrencyScript.ToString())
        End If
        'ddlrateplans.Attributes("onchange") = String.Format("javascript:onddlRateplansChanged('{0}', '{1}');", ddlrateplans.ClientID, lblCurrency.ClientID)
    End Sub

    Private Sub GenerateScriptCurrency()
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, 0, 0, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With
        fillCurrencyData(ds)
    End Sub

    Private Sub fillCurrencyData(ByVal ds As RatePlanData)
        Dim c As Integer = ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count

        CurrencyScript.Append("var ratesCurrencies = new Array();")
        For i As Integer = 0 To ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count - 1
            CurrencyScript.Append("ratesCurrencies.push({ratePlan:'" + ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(i).Item(RatePlanData.FIELD_IDRATEPLAN) + "', currency:'" + ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(i).Item("Moneda") + "'});")
        Next
    End Sub

    Private Function GetArrivosField() As String
        Dim NoArr As String = ""
        Dim ck As CheckBox
        Dim cheked As String
        For i As Integer = 1 To 7
            ck = Me.FindControl("Chk" & i)
            cheked = "Y"
            If Not ck.Checked Then cheked = "N"
            NoArr &= cheked
        Next
        Return NoArr
    End Function

    Private Function GetApplyDaysField() As String
        Dim NoArr As String = ""
        Dim ck As CheckBox
        Dim cheked As String
        For i As Integer = 1 To 7
            ck = Me.FindControl("ChkApp" & i)
            cheked = "Y"
            If Not ck.Checked Then cheked = "N"
            NoArr &= cheked
        Next
        Return NoArr
    End Function

    Private Sub loadDatos()
        Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
        Me.txtDateTo.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
        Dim idAsoc As Integer = Me.GetIdAsociation
        Dim ds As RatePlanData
        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(m_iHotelId, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With
        Me.RatePlanRow = Nothing
        Dim dv As DataView
        dv = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
        dv.RowFilter = RatePlanData.FIELD_SegmentRacPrinc & "=1 and " & RatePlanData.FIELD_SEGMENT & "='R'"

        'Se agrego esto para agregar los rates plan si preguntar si se tiene un RATEPLAN RAC        
        loadAllRatesplans()
        If TypeOf Me.Page Is FaresCataloguePromo Then
            CType(Me.Page, FaresCataloguePromo).loadDatos()
        End If
        Exit Sub

        'si hay tarifa rack la tarifa se enlazará con dicha tarifa, 
        If dv.Count > 0 Then
            Me.RatePlanRow = New RowRatePlan
            Me.RatePlanRow.IDRATEPLAN = dv(0)(RatePlanData.FIELD_IDRATEPLAN)
            Me.RatePlanRow.SEGMENT = dv(0)(RatePlanData.FIELD_SEGMENT)
            Me.RatePlanRow.RATECODE = dv(0)(RatePlanData.FIELD_CODIGOTARIFA)
        Else
            Me.RatePlanRow = Nothing
            If SaveSegmentRac() Then
                loadDatos()
                loadAllRatesplans()
                If TypeOf Me.Page Is FaresCataloguePromo Then
                    CType(Me.Page, FaresCataloguePromo).loadDatos()
                End If
            Else
                Response.Redirect(GeRequestApplicationPath("/Pages/RatesPlans.aspx"))
            End If
        End If
    End Sub

    Private Sub loadCulture()

        Me.lblBookWindowStartDate.Text = PortalCulture.GetString("00108", True)
        Me.lblBookWindowEndDate.Text = PortalCulture.GetString("00109", True)
        Me.lblBookWindowDateErrorSign.Text = PortalCulture.GetString("00112")
        Me.lblPromotion.Text = PortalCulture.GetString("00559", True)
        Me.RVPromotion.ErrorMessage = PortalCulture.GetString("00562")
        Me.lbPersonas.Text = PortalCulture.GetString("00082")
        Me.lblMinNumberAdults.Text = PortalCulture.GetString("01175", True)
        Me.lblNumberAdults.Text = PortalCulture.GetString("01176", True)
        Me.lblPeoplesExtras.Text = PortalCulture.GetString("00076", True)
        Me.lblNumberChildrens.Text = PortalCulture.GetString("00077", True)
        Me.lblProWin.Text = PortalCulture.GetString("01188", True)
        Me.lblOccupation.Text = PortalCulture.GetString("01170", True)
        Me.hplHideProWin.Text = PortalCulture.GetString("01171")
        Me.hplHideVentanaReserva.Text = PortalCulture.GetString("01171")
        Me.hplHideOccupation.Text = PortalCulture.GetString("01171")
        Me.hplShowProWin.Text = PortalCulture.GetString("01172")
        Me.hplShowVentanaReserva.Text = PortalCulture.GetString("01172")
        Me.hplShowOccupation.Text = PortalCulture.GetString("01172")
        Me.chkBookingWindow.Text = PortalCulture.GetString("01173")
        Me.lblWinReserva.Text = PortalCulture.GetString("01173", True)
        Me.hplShowRules.Text = PortalCulture.GetString("00327")
        Me.hplHideRules.Text = PortalCulture.GetString("00328")
        lblReservationRules.Text = PortalCulture.GetString("00106")
        Me.lblStartDate.Text = PortalCulture.GetString("00108", True)
        Me.lblEndDate.Text = PortalCulture.GetString("00109", True)
        Me.lblDateErrorSign.Text = PortalCulture.GetString("00112")
        'Me.lblPreciosTarifa.Text = PortalCulture.GetString("00131")
        Me.lblPromotion.Text = PortalCulture.GetString("00559", True)
        Me.RVPromotion.ErrorMessage = PortalCulture.GetString("01352")
        Me.lblFreeDays.Text = PortalCulture.GetString("00558", True)
        Me.rxvDaysfree.Text = PortalCulture.GetString("M000613")
        lblApplyDays.Text = PortalCulture.GetString("01357", True)


        Me.reqAdultFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqChildFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraAdultPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraChildPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraTeenPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqTeenFare.ErrorMessage = PortalCulture.GetString("M000179")

        Dim dsHotel As HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(m_iHotelId)
        End With
        Dim strIncTax As String

        Response.Write("<script> var updateSeasson='" & PortalCulture.GetString("00609") & "';</script>")

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            'lblMonTar.Text = PortalCulture.GetString("M000263") & " " & dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("Codigo")
            strIncTax = " " & PortalCulture.GetString("00610") & " "

            If Not dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).IsNull(HotelDatos.FIELD_PLUSTAX) Then
                If dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)(HotelDatos.FIELD_PLUSTAX) Then
                    strIncTax = " " & PortalCulture.GetString("00611") & " "
                End If
            End If

            If dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)("Codigo") = "MXN" Then
                strIncTax &= "<BR> " & PortalCulture.GetString("00667")
            End If

            lblMonTar.Text = strIncTax

        End If

        Me.lblExtraAdult.Text = PortalCulture.GetString("M000226", True)
        Me.lblExtraChildPrice.Text = PortalCulture.GetString("M000227", True)
        Me.valAdultExtraPrice.Text = PortalCulture.GetString("00116")
        Me.valExtraChildPrice.Text = PortalCulture.GetString("00116")
        RVPrecio.Text = PortalCulture.GetString("00116")
        Regularexpressionvalidator1.Text = PortalCulture.GetString("00116")
        Me.lblLunes.Text = PortalCulture.GetString("00300")
        Me.lblMartes.Text = PortalCulture.GetString("00301")
        Me.lblMiercoles.Text = PortalCulture.GetString("00302")
        Me.lblJueves.Text = PortalCulture.GetString("00303")
        Me.lblViernes.Text = PortalCulture.GetString("00304")
        Me.lblSabado.Text = PortalCulture.GetString("00305")
        Me.lbldomingo.Text = PortalCulture.GetString("00306")

        Me.lblLunes1.Text = PortalCulture.GetString("00300")
        Me.lblMartes1.Text = PortalCulture.GetString("00301")
        Me.lblMiercoles1.Text = PortalCulture.GetString("00302")
        Me.lblJueves1.Text = PortalCulture.GetString("00303")
        Me.lblViernes1.Text = PortalCulture.GetString("00304")
        Me.lblSabado1.Text = PortalCulture.GetString("00305")
        Me.lbldomingo1.Text = PortalCulture.GetString("00306")

        Me.lblMinDias.Text = PortalCulture.GetString("00117", True)
        Me.lblMaxDias.Text = PortalCulture.GetString("00118", True)
        Me.lblNoArrivos.Text = PortalCulture.GetString("00120", True)
        Me.lblAdvBooking.Text = PortalCulture.GetString("00119", True)
        lblPrecio.Text = PortalCulture.GetString("00087", True)
        lblRatePlan.Text = PortalCulture.GetString("00016", True)
        Me.lblDateError.Text = PortalCulture.GetString("00137")
        Me.LblChildPrice.Text = PortalCulture.GetString("00088", True)
        Me.chkRules.Text = PortalCulture.GetString("00316")
        lblExtraAdolescente.Text = PortalCulture.GetString("01311", True)
        rxvAdoslecenteFare.Text = PortalCulture.GetString("00116")
        lblAdolescentePrice.Text = PortalCulture.GetString("01282", True)
        rxvExtraTeenprice.Text = PortalCulture.GetString("00116")

        lstDates.Items.Clear()
        Me.lstDates.Items.Add(PortalCulture.GetString("00317", True))
        For i As Integer = 1 To lstDatesCount()
            Dim it As String = lstDatesItemI(i)
            Me.lstDates.Items.Add(it)
        Next

    End Sub

    Public Function loadAllRatesplans(Optional ByVal incluirPaquetesSegmentoK As Integer = 0, Optional ByVal idHabitacion As Integer = 0) As RatePlanData
        Dim ds As RatePlanData
        Dim dsActivos As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation
        With New RatePlanFacade
            dsActivos = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, 0, idAsociacion:=idAsoc, DeleteFilter:=1)
            ds = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, 0, idAsociacion:=idAsoc, DeleteFilter:=-1)
        End With

        ' IDs de planes activos
        Dim activosIds As New HashSet(Of String)
        For Each row As DataRow In dsActivos.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            activosIds.Add(row(RatePlanData.FIELD_IDRATEPLAN).ToString())
        Next

        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(Me.m_iHotelId, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With
        ds.Tables(RatePlanData.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RatePlanData.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & RatePlanData.FIELD_NAME & ",1,25)")
        Dim dv As DataView
        For Each r As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            dv = links.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = LinkRatePlanData.FIELD_TargetRatePlan & "='" & r(RatePlanData.FIELD_IDRATEPLAN) & "'"
            If incluirPaquetesSegmentoK = 0 Then
                If dv.Count > 0 Or r(RatePlanData.FIELD_SEGMENT) = "K" Then r.Delete()
            Else
                If dv.Count > 0 Then r.Delete()
            End If
        Next
        ds.Tables(RatePlanData.RATEPLAN_TABLE).AcceptChanges()

        ' Poblar dropdown agrupado Activos / Inactivos
        ddlrateplans.Items.Clear()
        ddlrateplans.Items.Add(New ListItem("── Planes Activos ──", "__GRP_ACTIVOS__"))
        If idHabitacion > 0 Then
            For Each row As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
                Dim idPlan As String = row(RatePlanData.FIELD_IDRATEPLAN).ToString()
                If activosIds.Contains(idPlan) AndAlso TieneTarifaParaHabitacion(idPlan, idHabitacion) Then
                    ddlrateplans.Items.Add(New ListItem("★ " & row("texto").ToString(), idPlan))
                End If
            Next
        End If
        For Each row As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            Dim idPlan As String = row(RatePlanData.FIELD_IDRATEPLAN).ToString()
            If activosIds.Contains(idPlan) Then
                If idHabitacion = 0 OrElse Not TieneTarifaParaHabitacion(idPlan, idHabitacion) Then
                    ddlrateplans.Items.Add(New ListItem(row("texto").ToString(), idPlan))
                End If
            End If
        Next
        ddlrateplans.Items.Add(New ListItem("── Planes Inactivos ──", "__GRP_INACTIVOS__"))
        For Each row As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            Dim idPlan As String = row(RatePlanData.FIELD_IDRATEPLAN).ToString()
            If Not activosIds.Contains(idPlan) Then
                ddlrateplans.Items.Add(New ListItem(row("texto").ToString(), idPlan))
            End If
        Next

        Return ds
    End Function

    Private Function TieneTarifaParaHabitacion(ByVal idRatePlan As String, ByVal idHabitacion As Integer) As Boolean
        If idHabitacion <= 0 Then Return False
        Try
            Dim datFares As FaresData
            Dim idAsoc As Integer = Me.GetIdAsociation
            With New FaresSystem
                datFares = .GetFaresByRoomTypeId(idHabitacion, PortalCulture.GetIDCulture, False, 1, idAsociacion:=idAsoc)
            End With
            Dim dv As DataView = datFares.Tables(FaresData.FARES_TABLE).DefaultView
            dv.RowFilter = FaresData.IDRATEPLAN_FIELD & "='" & idRatePlan & "'"
            Return dv.Count > 0
        Catch
            Return False
        End Try
    End Function

    Private Function SaveSegmentRac() As Boolean
        'crear el segmento
        Dim dsRate As New RatePlanData
        Dim rRate As DataRow
        'Dim val As Boolean
        'Dim idRate, NuevoIdRate As String
        rRate = dsRate.Tables(RatePlanData.RATEPLAN_TABLE).NewRow()
        With rRate
            .Item(RatePlanData.FIELD_IDRATEPLAN) = "RAC"
            .Item(RatePlanData.FIELD_DESCRIPTION) = "Only Room"
            .Item(RatePlanData.FIELD_SEGMENT) = "R"
            .Item(RatePlanData.FIELD_IDHOTEL) = Me.m_iHotelId
            .Item(RatePlanData.FIELD_CODIGOTARIFA) = "RAC"
            .Item(RatePlanData.FIELD_NAME) = "Standard Rate"
        End With
        dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Add(rRate)
        Dim idDic, iddic2 As Integer
        Dim txtDescripcion As New CtrlIdioma
        With New RatePlanAccess
            If .InsertRtPlan(dsRate, idDic, iddic2) Then
                txtDescripcion.Update("Only Room", "Solo Habitación", idDic)
            Else
                Return False
            End If
        End With
        Me.RatePlanRow = New RowRatePlan
        With dsRate.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)
            Me.RatePlanRow.IDRATEPLAN = .Item(RatePlanData.FIELD_IDRATEPLAN)
            Me.RatePlanRow.SEGMENT = .Item(RatePlanData.FIELD_SEGMENT)
        End With
        Return True
    End Function


    Public Function SaveNewFare(ByVal idRoom As Integer, ByRef idtar As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal descr As String, ByRef sdato As String) As Boolean
        Dim datFare As New FaresDataExc
        Dim ExistCode As New FaresDataExc
        Dim rowFare As DataRow
        'Dim sdatodespues As String
        ' Dim dv As DataView
        If RatePlanRow Is Nothing Then
            If Not SaveSegmentRac() Then
                Return False
            End If
        End If
        'buscar el codigo que le pertenece
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idRoom)
        End With
        If room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
        Try
            With datFare.Tables(FaresDataExc.FARESEXC_TABLE)
                rowFare = .NewRow()
                rowFare(FaresDataExc.ENDDATE_FIELD) = f2
                rowFare(FaresDataExc.STARTDATE_FIELD) = f1
                If Me.txtAdvBooking.Text <> "" Then
                    rowFare(FaresDataExc.ADVBOOKING_FIELD) = CInt(Val(Me.txtAdvBooking.Text))
                End If
                If Me.txtMaxAdvBooking.Text <> "" Then
                    rowFare(FaresDataExc.MAXADVBOOKING_FIELD) = CInt(Val(Me.txtMaxAdvBooking.Text))
                End If
                If Me.txtMinDias.Text <> "" Then
                    rowFare(FaresDataExc.MINDIAS_FIELD) = CInt(Val(Me.txtMinDias.Text))
                End If
                If Me.txtMaxDias.Text <> "" Then
                    rowFare(FaresDataExc.MAXDIAS_FIELD) = CInt(Val(Me.txtMaxDias.Text))
                End If
                rowFare(FaresDataExc.RULESDEFAULT) = chkRules.Checked
                rowFare(FaresDataExc.NOARRIVOS_FIELD) = GetArrivosField()
                rowFare(FaresDataExc.IDTIPOHABITACION_HOTEL_FIELD) = idRoom
                rowFare(FaresDataExc.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN

                rowFare(FaresDataExc.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.Insert()

                'Promociones, Ventanas, Ocupacion
                If txtDescProm.Text.Trim <> "" Then
                    rowFare(FaresDataExc.DESCPROMOTION_FIELD) = txtDescProm.Text
                Else
                    rowFare(FaresDataExc.DESCPROMOTION_FIELD) = DBNull.Value
                End If
                rowFare(FaresDataExc.APPLYDAY_FIELD) = GetApplyDaysField()
                If lstPeoplesInRoom.SelectedValue <> -1 Then
                    rowFare(FaresDataExc.PERSONAS_FIELD) = lstPeoplesInRoom.SelectedValue
                Else
                    rowFare(FaresDataExc.PERSONAS_FIELD) = DBNull.Value
                End If

                If lstMinNumberAdults.SelectedValue <> -1 Then
                    rowFare(FaresDataExc.MINADULTOS_FIELD) = lstMinNumberAdults.SelectedValue
                Else
                    rowFare(FaresDataExc.MINADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberAdults.SelectedValue <> -1 Then
                    rowFare(FaresDataExc.MAXADULTOS_FIELD) = lstNumberAdults.SelectedValue
                Else
                    rowFare(FaresDataExc.MAXADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberChildrens.SelectedValue <> -1 Then
                    rowFare(FaresDataExc.MAXNINIOS_FIELD) = lstNumberChildrens.SelectedValue
                Else
                    rowFare(FaresDataExc.MAXNINIOS_FIELD) = DBNull.Value
                End If

                If lstPeoplesExtras.SelectedValue <> -1 Then
                    rowFare(FaresDataExc.PERSONASEXTRAS_FIELD) = lstPeoplesExtras.SelectedValue
                Else
                    rowFare(FaresDataExc.PERSONASEXTRAS_FIELD) = DBNull.Value
                End If
                If Not String.IsNullOrEmpty(txtDaysFree.Text) Then
                    rowFare(FaresDataExc.DAYSFREE_FIELD) = txtDaysFree.Text
                Else
                    rowFare(FaresDataExc.DAYSFREE_FIELD) = DBNull.Value
                End If

                If chkBookingWindow.Checked Then
                    Try
                        rowFare(FaresDataExc.BOOKINGWINDOWSTART_FIELD) = CDate(txtBookWindowDateFrom.Text)
                        rowFare(FaresDataExc.BOOKINGWINDOWEND_FIELD) = CDate(txtBookWindowDateTo.Text)
                    Catch
                        rowFare(FaresDataExc.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                        rowFare(FaresDataExc.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                    End Try
                Else
                    rowFare(FaresDataExc.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                    rowFare(FaresDataExc.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                End If

                rowFare(FaresDataExc.PRICE_FIELD) = 0
                rowFare(FaresDataExc.NINIORATE_FIELD) = 0
                rowFare(FaresDataExc.RATEENPRICE_FIELD) = 0
                rowFare(FaresDataExc.EXTRAADULTPRICE_FIELD) = 0
                rowFare(FaresDataExc.EXTRACHILDPRICE_FIELD) = 0
                rowFare(FaresDataExc.EXTRATEENPRICE_FIELD) = 0

                Double.TryParse(Me.txtAdultFare.Text, rowFare(FaresDataExc.PRICE_FIELD))
                Double.TryParse(Me.txtChildFare.Text, rowFare(FaresDataExc.NINIORATE_FIELD))
                Double.TryParse(txtTeenFare.Text, rowFare(FaresDataExc.RATEENPRICE_FIELD))
                Double.TryParse(txtExtraAdultPrice.Text, rowFare(FaresDataExc.EXTRAADULTPRICE_FIELD))
                Double.TryParse(txtExtraChildPrice.Text, rowFare(FaresDataExc.EXTRACHILDPRICE_FIELD))
                Double.TryParse(txtExtraTeenPrice.Text, rowFare(FaresDataExc.EXTRATEENPRICE_FIELD))


                rowFare(FaresDataExc.EXCEPCIONES_FIELD) = Me.Exceptions
                .Rows.Add(rowFare)
            End With

            With New FaresExcFacade
                Try
                    If .InsertFares(datFare) Then
                        idtar = datFare.Tables(FaresDataExc.FARESEXC_TABLE).Rows(0)(FaresDataExc.PKIDFARESEXC_FIELD)
                        Addrateplan(datFare, "Descr_rateplan", descr)
                        sdato = Util.Utility.GetXml(FaresDataExc.FARESEXC_TABLE, "UpdateRatePromo", datFare)
                        If Me.Adultos > 0 Then
                            SaveFaresRestrictions(idtar)
                        End If
                    End If
                Catch ex As Exception
                    Me.lblDateError.Visible = True
                    'Catch ex As OverflowException
                    Me.lblDateError.Visible = True
                    Return False
                End Try
            End With
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    Private Function SaveFaresRestrictions(ByVal idTarifa As Integer) As Boolean
        Dim datRestrictions As New FaresRestrictionsDataExc
        For idxAdults As Integer = 1 To Me.Adultos
            For idxChild As Integer = 0 To Me.Ninios
                'Combinaciond de adultos - niños
                Dim newRow As DataRow = datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).NewRow()
                With newRow
                    .Item(FaresRestrictionsData.ADULTFARE_FIELD) = 0
                    .Item(FaresRestrictionsData.CHILDFARE_FIELD) = 0
                    Double.TryParse(Me.txtAdultFare.Text, .Item(FaresRestrictionsData.ADULTFARE_FIELD))
                    Double.TryParse(Me.txtChildFare.Text, .Item(FaresRestrictionsData.CHILDFARE_FIELD))

                    .Item(FaresRestrictionsDataExc.ADULTNUMBER_FIELD) = idxAdults
                    .Item(FaresRestrictionsDataExc.CHILDNUMBER_FIELD) = idxChild
                    .Item(FaresRestrictionsDataExc.IDFARE_FIELD) = idTarifa
                    .Item(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD) = 0
                End With
                datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows.Add(newRow)
            Next
        Next

        With New FaresRestrictionException
            SaveFaresRestrictions = .InsertFaresRestrictions(datRestrictions)
        End With
    End Function

    Function ConvDouble(ByVal valor As String) As Double
        Return If(String.IsNullOrEmpty(valor), 0, Double.Parse(valor))
    End Function

    Private Function UpdateFare(ByVal idroom As Integer, ByVal f1 As Date, ByVal f2 As Date, ByRef dsBefore As DataSet, ByRef dsFareUp As FaresDataExc, Optional ByVal publish As Boolean = True) As Boolean
        Dim datFares As New FaresDataExc
        Dim ExistCode As New FaresData
        Dim fareRow As DataRow
        Dim bResult As Boolean
        Me.lblDateError.Visible = False

        dsBefore = (New FaresExcFacade).GetFareByFareId(Me.m_iFareId)
        With datFares
            fareRow = .Tables(.FARESEXC_TABLE).NewRow()
            Try
                ' try to fill fare row data
                fareRow(.PKIDFARESEXC_FIELD) = Me.m_iFareId
                fareRow(.ENDDATE_FIELD) = Format(f2, "yyyy/MM/dd")


                fareRow(.EXCEPCIONES_FIELD) = Me.Exceptions
                fareRow(.IDTIPOHABITACION_HOTEL_FIELD) = idroom
                fareRow(.STARTDATE_FIELD) = Format(CDate(f1), "yyyy/MM/dd")

                If txtAdvBooking.Text <> "" Then
                    fareRow(FaresDataExc.ADVBOOKING_FIELD) = CInt(Val(Me.txtAdvBooking.Text))
                End If
                If txtMaxAdvBooking.Text <> "" Then
                    fareRow(FaresDataExc.MAXADVBOOKING_FIELD) = CInt(Val(Me.txtMaxAdvBooking.Text))
                End If
                If txtMinDias.Text <> "" Then
                    fareRow(FaresDataExc.MINDIAS_FIELD) = CInt(Val(Me.txtMinDias.Text))
                End If
                If txtMaxDias.Text <> "" Then
                    fareRow(FaresDataExc.MAXDIAS_FIELD) = CInt(Val(Me.txtMaxDias.Text))
                End If
                fareRow(FaresDataExc.NOARRIVOS_FIELD) = GetArrivosField()
                fareRow(FaresDataExc.RULESDEFAULT) = chkRules.Checked

                fareRow(FaresDataExc.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
                'Promociones, Ventanas, Ocupacion
                If txtDescProm.Text.Trim <> "" Then
                    fareRow(FaresDataExc.DESCPROMOTION_FIELD) = txtDescProm.Text
                Else
                    fareRow(FaresDataExc.DESCPROMOTION_FIELD) = DBNull.Value
                End If

                If Me.txtPromoDescription.IdIndice = 0 Then
                    fareRow(FaresDataExc.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.Insert()
                Else
                    fareRow(FaresDataExc.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.IdIndice
                    Me.txtPromoDescription.Update(Me.txtPromoDescription.IdIndice, publish:=publish)
                End If

                fareRow(FaresDataExc.APPLYDAY_FIELD) = GetApplyDaysField()

                If Not String.IsNullOrEmpty(txtDaysFree.Text) Then
                    fareRow(FaresDataExc.DAYSFREE_FIELD) = txtDaysFree.Text
                Else
                    fareRow(FaresDataExc.DAYSFREE_FIELD) = DBNull.Value
                End If
                If lstPeoplesInRoom.SelectedValue <> -1 Then
                    fareRow(FaresDataExc.PERSONAS_FIELD) = lstPeoplesInRoom.SelectedValue
                Else
                    fareRow(FaresDataExc.PERSONAS_FIELD) = DBNull.Value
                End If

                If lstMinNumberAdults.SelectedValue <> -1 Then
                    fareRow(FaresDataExc.MINADULTOS_FIELD) = lstMinNumberAdults.SelectedValue
                Else
                    fareRow(FaresDataExc.MINADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberAdults.SelectedValue <> -1 Then
                    fareRow(FaresDataExc.MAXADULTOS_FIELD) = lstNumberAdults.SelectedValue
                Else
                    fareRow(FaresDataExc.MAXADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberChildrens.SelectedValue <> -1 Then
                    fareRow(FaresDataExc.MAXNINIOS_FIELD) = lstNumberChildrens.SelectedValue
                Else
                    fareRow(FaresDataExc.MAXNINIOS_FIELD) = DBNull.Value
                End If

                If lstPeoplesExtras.SelectedValue <> -1 Then
                    fareRow(FaresDataExc.PERSONASEXTRAS_FIELD) = lstPeoplesExtras.SelectedValue
                Else
                    fareRow(FaresDataExc.PERSONASEXTRAS_FIELD) = DBNull.Value
                End If

                If chkBookingWindow.Checked Then
                    Try
                        fareRow(FaresDataExc.BOOKINGWINDOWSTART_FIELD) = CDate(txtBookWindowDateFrom.Text)
                        fareRow(FaresDataExc.BOOKINGWINDOWEND_FIELD) = CDate(txtBookWindowDateTo.Text)
                    Catch
                        fareRow(FaresDataExc.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                        fareRow(FaresDataExc.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                    End Try
                Else
                    fareRow(FaresDataExc.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                    fareRow(FaresDataExc.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                End If

                fareRow(FaresDataExc.PRICE_FIELD) = 0
                fareRow(FaresDataExc.NINIORATE_FIELD) = 0
                fareRow(FaresDataExc.RATEENPRICE_FIELD) = 0
                fareRow(FaresDataExc.EXTRAADULTPRICE_FIELD) = 0
                fareRow(FaresDataExc.EXTRACHILDPRICE_FIELD) = 0
                fareRow(FaresDataExc.EXTRATEENPRICE_FIELD) = 0

                Double.TryParse(Me.txtAdultFare.Text, fareRow(FaresDataExc.PRICE_FIELD))
                Double.TryParse(Me.txtChildFare.Text, fareRow(FaresDataExc.NINIORATE_FIELD))
                Double.TryParse(txtTeenFare.Text, fareRow(FaresDataExc.RATEENPRICE_FIELD))
                Double.TryParse(txtExtraAdultPrice.Text, fareRow(FaresDataExc.EXTRAADULTPRICE_FIELD))
                Double.TryParse(txtExtraChildPrice.Text, fareRow(FaresDataExc.EXTRACHILDPRICE_FIELD))
                Double.TryParse(txtExtraTeenPrice.Text, fareRow(FaresDataExc.EXTRATEENPRICE_FIELD))

                .Tables(.FARESEXC_TABLE).Rows.Add(fareRow)
                ' set row state to modified
                fareRow.AcceptChanges()
                fareRow(.PKIDFARESEXC_FIELD) = fareRow(.PKIDFARESEXC_FIELD)
                With New FaresExcFacade
                    Try
                        bResult = .ActualizaFares(datFares)
                        dsFareUp = datFares
                        dsFareUp.AcceptChanges()
                    Catch ex As OverflowException
                        Me.lblDateError.Visible = True
                        Return False
                    End Try
                End With
                If bResult = True Then
                    Me.m_iFareId = .Tables(.FARESEXC_TABLE).Rows(0)(.PKIDFARESEXC_FIELD)
                End If
            Catch ex As Exception
                Return False
            End Try
        End With
        Return True
    End Function

    '--------------------------------------------------------------
    'Esto es utilizado cuando la tarifa ya existe
    Public Sub LoadFare(ByVal iFareId As Integer, Optional ByVal load As Integer = 1)

        Dim datFares As DataSet
        Dim rowFare As DataRow
        Dim bOcupacion As Boolean = False
        Dim bpromo As Boolean = False
        Dim brules As Boolean = False
        Dim bventana As Boolean = False

        m_iFareId = iFareId
        cleardata()
        ' get fare information
        With New FaresExcFacade
            datFares = .GetFareByFareId(iFareId)
        End With

        If Not datFares Is Nothing AndAlso datFares.Tables(FaresDataExc.FARESEXC_TABLE).Rows.Count > 0 Then

            Me.ddlrateplans.Enabled = True
            rowFare = datFares.Tables(FaresDataExc.FARESEXC_TABLE).Rows(0)
            Me.chkRules.Checked = rowFare(FaresDataExc.RULESDEFAULT)
            Try
                Me.ddlrateplans.SelectedValue = rowFare(FaresDataExc.IDRATEPLAN_FIELD).ToString.ToUpper
            Catch ex As Exception
            End Try

            'If Not rowFare(FaresData.SEGMENTPRINCIPAL) Is System.DBNull.Value AndAlso rowFare(FaresDataExc.SEGMENTPRINCIPAL) = True Then
            '    Me.ddlrateplans.Enabled = False
            'End If
            Me.txtDateFrom.Text = CType(rowFare(FaresDataExc.STARTDATE_FIELD), DateTime).ToString("MM/dd/yyyy")
            Me.txtDateTo.Text = CType(rowFare(FaresDataExc.ENDDATE_FIELD), DateTime).ToString("MM/dd/yyyy")

            Me.txtMinDias.Text = ""
            Me.txtMaxDias.Text = ""
            Me.txtAdvBooking.Text = ""
            Me.txtMaxAdvBooking.Text = ""

            If Not rowFare.IsNull(FaresDataExc.NOARRIVOS_FIELD) Then
                getNoArrrivalsField(rowFare(FaresDataExc.NOARRIVOS_FIELD))
                brules = IIf(rowFare(FaresDataExc.NOARRIVOS_FIELD) = "NNNNNNN", False, True)
            End If

            If Not rowFare.IsNull(FaresDataExc.IDDICCDESCPROM_FIELD) Then
                Me.txtPromoDescription.CargaDatos(rowFare(FaresDataExc.IDDICCDESCPROM_FIELD))
            Else
                Me.txtPromoDescription.CargaDatos(0)
            End If

            If Not rowFare.IsNull(FaresDataExc.MINDIAS_FIELD) Then
                txtMinDias.Text = CInt(Val(rowFare(FaresDataExc.MINDIAS_FIELD)))
                brules = True
            End If

            If Not rowFare.IsNull(FaresDataExc.MAXDIAS_FIELD) Then
                txtMaxDias.Text = CInt(Val(rowFare(FaresDataExc.MAXDIAS_FIELD)))
                brules = True
            End If

            If Not rowFare.IsNull(FaresDataExc.ADVBOOKING_FIELD) Then
                txtAdvBooking.Text = CInt(Val(rowFare(FaresDataExc.ADVBOOKING_FIELD)))
                brules = True
            End If
            If Not rowFare.IsNull(FaresDataExc.MAXADVBOOKING_FIELD) Then
                txtMaxAdvBooking.Text = CInt(Val(rowFare(FaresDataExc.MAXADVBOOKING_FIELD)))
                brules = True
            End If
            'Promocion y Ventana de Reserva, ocupacion
            If Not rowFare.IsNull(FaresDataExc.DESCPROMOTION_FIELD) Then
                txtDescProm.Text = CType(rowFare(FaresDataExc.DESCPROMOTION_FIELD), Decimal).ToString("00.00")
                bpromo = True
            Else
                txtDescProm.Text = ""
            End If

            chkBookingWindow.Checked = False
            If Not rowFare.IsNull(FaresDataExc.BOOKINGWINDOWSTART_FIELD) AndAlso Not rowFare.IsNull(FaresDataExc.BOOKINGWINDOWEND_FIELD) Then
                chkBookingWindow.Checked = True
                txtBookWindowDateFrom.Text = CDate(rowFare(FaresDataExc.BOOKINGWINDOWSTART_FIELD)).ToString("MM/dd/yyyy")
                txtBookWindowDateTo.Text = CDate(rowFare(FaresDataExc.BOOKINGWINDOWEND_FIELD)).ToString("MM/dd/yyyy")
                bventana = True
            Else
                Me.txtBookWindowDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
                Me.txtBookWindowDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
            End If
            tbBookingWindow.Style("display") = IIf(chkBookingWindow.Checked, "", "none")

            lstPeoplesInRoom.SelectedIndex = 0
            If Not rowFare.IsNull(FaresDataExc.PERSONAS_FIELD) Then
                bOcupacion = True
                lstPeoplesInRoom.SelectedIndex = lstPeoplesInRoom.Items.IndexOf(lstPeoplesInRoom.Items.FindByValue(rowFare(FaresDataExc.PERSONAS_FIELD)))
            End If

            lstPeoplesExtras.SelectedIndex = 0
            If Not rowFare.IsNull(FaresDataExc.PERSONASEXTRAS_FIELD) Then
                bOcupacion = True
                lstPeoplesExtras.SelectedIndex = lstPeoplesExtras.Items.IndexOf(lstPeoplesExtras.Items.FindByValue(rowFare(FaresDataExc.PERSONASEXTRAS_FIELD)))
            End If

            lstMinNumberAdults.SelectedIndex = 0
            If Not rowFare.IsNull(FaresDataExc.MINADULTOS_FIELD) Then
                bOcupacion = True
                lstMinNumberAdults.SelectedIndex = lstMinNumberAdults.Items.IndexOf(lstMinNumberAdults.Items.FindByValue(rowFare(FaresDataExc.MINADULTOS_FIELD)))
            End If

            lstNumberAdults.SelectedIndex = 0
            If Not rowFare.IsNull(FaresDataExc.MAXADULTOS_FIELD) Then
                bOcupacion = True
                lstNumberAdults.SelectedIndex = lstNumberAdults.Items.IndexOf(lstNumberAdults.Items.FindByValue(rowFare(FaresDataExc.MAXADULTOS_FIELD)))
            End If

            lstNumberChildrens.SelectedIndex = 0
            If Not rowFare.IsNull(FaresDataExc.MAXNINIOS_FIELD) Then
                bOcupacion = True
                lstNumberChildrens.SelectedIndex = lstNumberChildrens.Items.IndexOf(lstNumberChildrens.Items.FindByValue(rowFare(FaresDataExc.MAXNINIOS_FIELD)))
            End If


            If Not rowFare.IsNull(FaresDataExc.DAYSFREE_FIELD) Then
                txtDaysFree.Text = rowFare(FaresDataExc.DAYSFREE_FIELD)
            Else
                txtDaysFree.Text = ""
            End If

            If Not rowFare.IsNull(FaresDataExc.APPLYDAY_FIELD) Then
                SetApplyDaysField(rowFare(FaresDataExc.APPLYDAY_FIELD))
            End If


            txtAdultFare.Text = CDbl(IIf(rowFare(FaresDataExc.PRICE_FIELD) Is DBNull.Value, _
                                             0, rowFare(FaresDataExc.PRICE_FIELD)))
            txtChildFare.Text = CDbl(IIf(rowFare(FaresDataExc.NINIORATE_FIELD) Is DBNull.Value, _
                                             0, rowFare(FaresDataExc.NINIORATE_FIELD)))

            txtTeenFare.Text = CDbl(IIf(rowFare(FaresDataExc.RATEENPRICE_FIELD) Is DBNull.Value, _
                                             0, rowFare(FaresDataExc.RATEENPRICE_FIELD)))

            If txtExtraAdultPrice.Enabled Then
                txtExtraAdultPrice.Text = CDbl(IIf(rowFare(FaresDataExc.EXTRAADULTPRICE_FIELD) Is DBNull.Value, _
                                                 0, rowFare(FaresDataExc.EXTRAADULTPRICE_FIELD)))
            End If
            If txtExtraChildPrice.Enabled Then
                txtExtraChildPrice.Text = CDbl(IIf(rowFare(FaresDataExc.EXTRACHILDPRICE_FIELD) Is DBNull.Value, _
                                                 0, rowFare(FaresDataExc.EXTRACHILDPRICE_FIELD)))
            End If
            If txtExtraTeenPrice.Enabled Then
                txtExtraTeenPrice.Text = CDbl(IIf(rowFare(FaresDataExc.EXTRATEENPRICE_FIELD) Is DBNull.Value, _
                                                  0, rowFare(FaresDataExc.EXTRATEENPRICE_FIELD)))
            End If

            Me.m_Modo = "MODIFY"
            Me.Exceptions = rowFare(FaresDataExc.EXCEPCIONES_FIELD)
            If Me.txtMinDias.Text = "0" Then Me.txtMinDias.Text = "1"

            bpromo = (Me.txtDescProm.Text.Trim().Length > 0 OrElse Me.txtDaysFree.Text.Trim().Length > 0 OrElse Me.txtPromoDescription.HasValue)

            ShowWindows(hplShowProWin, hplHideProWin, DivPromotionAndWindow, bpromo)
            ShowWindows(hplShowVentanaReserva, hplHideVentanaReserva, DivWindowBooking, bventana)
            ShowWindows(hplShowOccupation, hplHideOccupation, DivOccupation, bOcupacion)
            ShowWindows(hplShowRules, hplHideRules, DivRules, brules)
            Me.HasData = True
            'Me.Page.RegisterStartupScript("", "<script>AddDate('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');</script>")
        End If
    End Sub

    Sub ShowWindows(ByVal hplShow As HyperLink, ByVal hplhide As HyperLink, ByVal idDiv As HtmlGenericControl, ByVal show As Boolean)
        If show Then
            hplShow.Style.Add("display", "none")
            hplhide.Style.Add("display", "")
            idDiv.Style.Add("display", "")
        Else
            hplShow.Style.Add("display", "")
            hplhide.Style.Add("display", "none")
            idDiv.Style.Add("display", "none")
        End If
    End Sub


    Private Sub getNoArrrivalsField(ByVal Field As String)
        Dim ck As CheckBox
        For i As Integer = 1 To 7
            ck = FindControl("Chk" & i)
            ck.Checked = False
            Try
                If Field.Substring(i - 1, 1) = "Y" Then
                    ck.Checked = True
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub SetApplyDaysField(ByVal Field As String)
        Dim ck As CheckBox
        For i As Integer = 1 To 7
            ck = FindControl("ChkApp" & i)
            ck.Checked = False
            Try
                If Field.Substring(i - 1, 1) = "Y" Then
                    ck.Checked = True
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub

    Public Sub newFare()
        cleardata()
        SetDefaultValues()
        Me.m_Modo = "NEW"
        Me.m_iFareId = 0
    End Sub

    Private Sub SetDefaultValues()
        ' Set default fare values of input boxes if no values entered
        If Me.txtDateFrom.Text.Trim.Length = 0 Then
            Me.txtDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        If Me.txtDateTo.Text.Trim.Length = 0 Then
            Me.txtDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        If Me.txtBookWindowDateFrom.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        If Me.txtBookWindowDateTo.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
        End If

        'If Me.txtExtraAdultPrice.Text.Trim.Length = 0 Then
        '    Me.txtExtraAdultPrice.Text = "0"
        'End If
        'If Me.txtExtraChildPrice.Text.Trim.Length = 0 Then
        '    Me.txtExtraChildPrice.Text = "0"
        'End If
        'If String.IsNullOrEmpty(txtExtraTeenPrice.Text) Then
        '    txtExtraTeenPrice.Text = "0"
        'End If
        If String.IsNullOrEmpty(txtTeenFare.Text) Then
            txtTeenFare.Text = ""
        End If
    End Sub

    Function Nota(ByVal rooom As String, ByVal f1last As String, ByVal f2last As String, ByVal rp As String, ByVal f1 As String, ByVal f2 As String, ByRef sreference As String) As String
        Dim msg As String = "Se modificó la tarifa promocion de la habitación " & rooom & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.RatePlanRow.RATECODE
        Dim drhotel As DataRow = CType(Me.Page, PaginaBase).HotelInfo
        Dim idioma As String

        sreference = "Cambio de tarifa"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            sreference = "Update Rate"
            msg = "It changed the room rate " & rooom & " from " & f1last & " to " & f2last & " with rateplan " & rp & " the new date is (or remains) of " & f1 & " to " & f2 & " , with rateplan " & Me.RatePlanRow.RATECODE
        End If
        Return msg
    End Function

    Sub Addrateplan(ByVal ds As DataSet, ByVal field As String, ByVal valor As String)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    Public Function AddFare(ByVal idRoom As Integer, ByRef idFare As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String, ByVal rp As String, ByVal f1last As String, ByVal f2last As String, ByVal sroom As String, ByVal publish As Boolean, ByVal isOcupacion As Boolean, ByVal FareAdultMin As Double, ByVal FareChildMin As Double, ByVal FareJuniorMin As Double, ByRef sCorreo As String) As Boolean
        Dim flag = False
        If Page.IsValid Then
            Dim sdato As String
            Dim sdatodespues As String

            SetDefaultValues()
            Dim ds As RatePlanData
            With New RatePlanFacade
                ds = .GetDataRatePlan(Me.ddlrateplans.SelectedValue, Me.m_iHotelId)
            End With
            If ds.Tables(ds.RATEPLAN_TABLE).Rows.Count > 0 Then
                With ds.Tables(ds.RATEPLAN_TABLE).Rows(0)
                    Try
                        Me.RatePlanRow = New RowRatePlan
                        'Me.RatePlanRow.DESCRIPTION = .Item(ds.FIELD_DESCRIPTION).ToString
                        'Me.RatePlanRow.IDDICDESC = CInt(Val(.Item(ds.FIELD_IDDICDESC)))
                        Me.RatePlanRow.IDRATEPLAN = .Item(ds.FIELD_IDRATEPLAN)
                        Me.RatePlanRow.SEGMENT = .Item(ds.FIELD_SEGMENT)
                        Me.RatePlanRow.RATECODE = .Item(ds.FIELD_CODIGOTARIFA)
                    Catch ex As Exception
                    End Try
                End With
            End If

            If isOcupacion Then
                txtAdultFare.Text = FareAdultMin
                txtChildFare.Text = FareChildMin
                txtTeenFare.Text = FareJuniorMin
            End If

            If Me.m_iFareId = 0 Then
                If SaveNewFare(idRoom, idFare, f1, f2, String.Format("{0} {1}", sroom, rp), sdato) Then
                    sCorreo = (New Util.Utility).GeneraCorreoXslt("", sdato)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCataloguePromo.aspx", PaginaBase.acciones.Crear, "Se creó la tarifa promocion de la habitación " & ch.Substring(0, ch.IndexOf("--")) & " de la fecha " & f1 & " a la fecha " & f2 & " con el rateplan " & Me.RatePlanRow.RATECODE, "", "", sdato)
                    flag = True
                End If
            Else
                Dim dsFaresUp As New FaresDataExc
                Dim dsBefore As New DataSet
                'Dim sdatocorreo As String = ""
                If UpdateFare(idRoom, f1, f2, dsBefore, dsFaresUp, publish) Then
                    Dim sreference As String = ""
                    'sdato = dsBefore.GetXml.ToString

                    Addrateplan(dsBefore, "Descr_rateplan", String.Format("{0} {1}", sroom, rp))
                    sdato = Util.Utility.GetXml(dsFaresUp.FARESEXC_TABLE, "UpdateRatePromo", dsBefore)

                    Addrateplan(dsFaresUp, "Descr_rateplan", String.Format("{0} {1}", sroom, rp))
                    sdatodespues = Util.Utility.GetXml(dsFaresUp.FARESEXC_TABLE, "UpdateRatePromo", dsFaresUp)

                    Dim snota As String = Nota(ch, f1last, f2last, rp, f1, f2, sreference)
                    'sdatocorreo = CreateAvailHtml(dsBefore, dsFaresUp)
                    scorreo = (New Util.Utility).GeneraCorreoXslt(sdato, sdatodespues)
                    'CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Modificar, "Se modificó la tarifa de la habitación " & ch & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.RatePlanRow.RATECODE, "Update Rate", sdato, sdatodespues, sdatocorreo)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCataloguePromo.aspx", PaginaBase.acciones.Modificar, snota, sreference, sdato, sdatodespues)
                    Dim usuario As String = ""
                    flag = True
                End If
                m_iFareId = 0
            End If
            If Me.txtPromoDescription.HasChanges Then CType(Me.Page, PaginaBase).NotifyContentModification("Tarifa promoción de la habitación " & ch & ", y plan tarifario " & Me.RatePlanRow.RATECODE, "Tarifas Promoción")
        End If
        Return flag
    End Function

    Public Function CreateAvailHtml(ByVal dsBefore As DataSet, ByVal dsFares As FaresDataExc) As String
        Dim menu As New Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim dv As New DataView
        Dim sw As StringWriter = New StringWriter
        Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim shtml As String = ""
        Dim idioma As String
        Dim drhotel As DataRow = CType(Me.Page, PaginaBase).HotelInfo

        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))

        menu.CellSpacing = 1
        menu.CellPadding = 1
        'menu.BorderWidth = 1
        menu.Width = New System.Web.UI.WebControls.Unit(600, UnitType.Pixel)

        tr = New TableRow
        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderImportant")
        td.Text = ""
        td.ColumnSpan = 8
        tr.Cells.Add(td)
        menu.Rows.Add(tr)

        tr = New TableRow
        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00016", idioma)   '"Rate plan"  
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00090", idioma)   '"Price "   
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00279", idioma)   '"Price Child"   
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00114", idioma)   '"Extra Price"   
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00115", idioma)   '"Extra Price Child"  
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("01184", idioma)   '"Excepciones"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000267", idioma)   '"MinDias"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("01185", idioma)   '"NoArrivos"
        tr.Cells.Add(td)
        menu.Rows.Add(tr)

        For Each dr As DataRow In dsBefore.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = ddlrateplans.SelectedItem.Text
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("Precio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioNinio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraAdulto"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioNinioExtra"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("NoArrivos")
            tr.Cells.Add(td)
            menu.Rows.Add(tr)
        Next

        For Each dr As DataRow In dsFares.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = ddlrateplans.SelectedItem.Text
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("Precio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioNinio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraAdulto"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioNinioExtra"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("NoArrivos")
            tr.Cells.Add(td)
            menu.Rows.Add(tr)
        Next
        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
    End Function

    Private Function GetNoteNewRate(ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String) As String
        Dim result As String = String.Empty

        result += "Se creó la tarifa de la habitación "
        result += " de la fecha" + f1
        result += " a la fecha " + f2
        result += " con el rateplan " + RatePlanRow.RATECODE
        result += " "
        result += ""
        Return result

    End Function

    Private Sub imgAddDate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim fec1 As Date = CDate(txtDateFrom.Text)
        Dim fec2 As Date = CDate(Me.txtDateTo.Text)
        Dim sw As Boolean = False
        For i As Integer = 1 To lstDates.Items.Count - 1
            Dim f1, f2 As Date
            f1 = CDate(lstDates.Items(i).ToString.Split("-")(0))
            f2 = CDate(lstDates.Items(i).ToString.Split("-")(1))
            If (((fec1 >= f1 And fec1 <= f2) Or ((fec2 >= f1) And fec2 <= f2)) Or ((f1 >= fec1 And f1 <= fec2) Or ((f2 >= fec1) And f2 <= fec2))) Then
                sw = True
                Exit For
            End If
        Next
        If Not sw Then
            Dim it As String = fec1 & "-" & fec2
            Me.lstDates.Items.Add(it)
        End If
    End Sub

    Public Function lstDatesCount() As Integer
        Return Me.txtFechas.Text.Split("$").Length() - 1
    End Function

    Public Function lstDatesItemI(ByVal i As Integer) As String
        Return Me.txtFechas.Text.Split("$").GetValue(i)
    End Function

    Public Function lstDatesAdd()
        Me.txtFechas.Text = "$" & txtDateFrom.Text & "-" & Me.txtDateTo.Text
    End Function

    Public Function lstDatesClear()
        Me.txtFechas.Text = ""
    End Function

    Public Function Segment(ByVal Value As String) As String
        Try
            If Value <> "0" Then
                Me.ddlrateplans.SelectedValue = Value
            End If
        Catch ex As Exception
        End Try
    End Function

    Private Sub DesabilitaExtras(ByVal src As String, ByVal enabled As Boolean)
        txtExtraAdultPrice.Text = src
        txtExtraAdultPrice.Enabled = enabled
        txtExtraChildPrice.Text = src
        txtExtraChildPrice.Enabled = enabled
        txtExtraTeenPrice.Text = src
        txtExtraTeenPrice.Enabled = enabled


        Me.reqExtraAdultPrice.Enabled = enabled
        Me.reqExtraChildPrice.Enabled = enabled
        Me.reqExtraTeenPrice.Enabled = enabled

    End Sub

    Public Sub LoadRooms(ByVal idRoom As Integer)

        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idRoom)
        End With
        If Not room Is Nothing AndAlso room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count > 0 Then
            Dim rowRoom As DataRow = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)

            Dim iPeople As Integer = rowRoom(RoomsHotelData.FLD_NUMBER_PEOPLESINROOM)
            Dim iChild As Integer = rowRoom(RoomsHotelData.FLD_NUMBER_MAXCHILDREN)
            Dim iAdult As Integer = rowRoom(RoomsHotelData.FLD_NUMBER_MAXADULTS)
            Dim iExtPeople As Integer = rowRoom(RoomsHotelData.FLD_NUMBER_PEOPLESEXTRAS)
            Adultos = iAdult
            Ninios = iChild
            loadLst(lstPeoplesInRoom, True, 1, iPeople)
            loadLst(lstMinNumberAdults, True, 1, iAdult)
            loadLst(lstNumberAdults, True, 1, iAdult)
            loadLst(lstNumberChildrens, True, 0, iChild)
            loadLst(lstPeoplesExtras, True, 0, iExtPeople)

            If iExtPeople = 0 Then
                DesabilitaExtras("", False)
            Else
                DesabilitaExtras("", True)
            End If
        End If
    End Sub

    Private Sub loadLst(ByRef ddl As DropDownList, ByVal addEmpty As Boolean, ByVal valueStart As Integer, ByVal valueEnd As Integer)
        ddl.Items.Clear()
        Dim n As Integer = 0

        If addEmpty Then ddl.Items.Add(New ListItem(" ", "-1"))

        For n = valueStart To valueEnd
            ddl.Items.Add(New ListItem(n, n))
        Next

        ddl.SelectedIndex = 0
    End Sub

    Private Sub Chk1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chk1.Load

    End Sub
End Class

<Serializable()> Public Class RowRatePlanExc
    Implements ISerializable
    Public IDRATEPLAN As String
    Public SEGMENT As String
    Public DESCRIPTION As String
    Public IDDICDESC As Integer
    '  Public IDHOTELPLAN As Integer
    Public RATECODE As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal rateplan As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        With rateplan
            IDRATEPLAN = .GetValue("IdRatePlan", GetType(String))
            SEGMENT = .GetValue("Segment", GetType(String))
            DESCRIPTION = .GetValue("Description", GetType(String))
            IDDICDESC = .GetValue("IdDictionaryDescription", GetType(Integer))
            RATECODE = .GetValue("CodigoTarifa", GetType(String))
        End With
    End Sub

    Public Sub GetObjectData(ByVal rateplan As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        With rateplan
            .AddValue("IdRatePlan", IDRATEPLAN, GetType(String))
            .AddValue("Segment", SEGMENT, GetType(String))
            .AddValue("Description", DESCRIPTION, GetType(String))
            .AddValue("IdDictionaryDescription", IDDICDESC, GetType(Integer))
            .AddValue("CodigoTarifa", RATECODE, GetType(String))

        End With
    End Sub




End Class