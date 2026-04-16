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

Partial Class ctrRateAplication
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


    Dim _currentCurrency As String
    Protected Property CurrentCurrency() As String
        Get
            Return Me._currentCurrency
        End Get
        Set(ByVal value As String)
            Me._currentCurrency = value
        End Set
    End Property

    'Public Property SourceRateName() As String
    '    Get
    '        Return viewstate("_SRN")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SRN") = Value
    '        'Me.ddlrateplans.Attributes.Add("onChange", "javascript:showRatePlan2('" & Me.ddlrateplans.ClientID & "','" & Me.SourceRateName & "','" & lbldescrateplan.ClientID & "')")
    '    End Set
    'End Property



#Region " C�digo generado por el Dise�ador de Web Forms "

    'El Dise�ador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lCal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lCal2 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblfechas As System.Web.UI.WebControls.Label

    'NOTA: el Dise�ador de Web Forms necesita la siguiente declaraci�n del marcador de posici�n.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private CurrencyScript As New StringBuilder

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Dise�ador de Web Forms requiere esta llamada de m�todo
        'No la modifique con el editor de c�digo.
        InitializeComponent()
    End Sub

#End Region

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Private Sub ActivateShowButtons()
        If Me.txtAdvBooking.Text.Trim().Length > 0 OrElse Me.txtMinDias.Text.Trim().Length > 0 OrElse Me.txtMaxDias.Text.Trim().Length > 0 OrElse Me.Chk1.Checked OrElse Me.Chk2.Checked OrElse Me.Chk3.Checked OrElse Me.Chk4.Checked OrElse Me.Chk5.Checked OrElse Me.Chk6.Checked OrElse Me.Chk7.Checked Then
            Me.DivRules.Style.Add("display", "")
            Me.hplShowRules.Style.Add("display", "none")
            Me.hplHideRules.Style.Add("display", "")
        Else
            Me.DivRules.Style.Add("display", "none")
            Me.hplShowRules.Style.Add("display", "")
            Me.hplHideRules.Style.Add("display", "none")
        End If

        'Promiciones 
        If Me.txtDescProm.Text.Trim().Length > 0 OrElse Me.txtPromoDescription.HasValue Then
            Me.DivPromotionAndWindow.Style.Add("display", "")
            Me.hplShowProWin.Style.Add("display", "none")
            Me.hplHideProWin.Style.Add("display", "")
        Else
            Me.DivPromotionAndWindow.Style.Add("display", "none")
            Me.hplShowProWin.Style.Add("display", "")
            Me.hplHideProWin.Style.Add("display", "none")
        End If
        'Ocupacion
        If Me.chkBookingWindow.Checked AndAlso (Me.txtBookWindowDateFrom.Text.Trim().Length > 0 OrElse Me.txtBookWindowDateTo.Text.Trim().Length > 0) Then
            Me.DivWindowBooking.Style.Add("display", "")
            Me.hplShowVentanaReserva.Style.Add("display", "none")
            Me.hplHideVentanaReserva.Style.Add("display", "")
        Else
            Me.DivWindowBooking.Style.Add("display", "none")
            Me.hplShowVentanaReserva.Style.Add("display", "")
            Me.hplHideVentanaReserva.Style.Add("display", "none")
        End If

        If Me.lstPeoplesInRoom.SelectedValue >= 0 OrElse Me.lstMinNumberAdults.SelectedValue >= 0 OrElse Me.lstNumberAdults.SelectedValue >= 0 OrElse Me.lstNumberChildrens.SelectedValue >= 0 OrElse Me.lstPeoplesExtras.SelectedValue >= 0 Then
            Me.DivOccupation.Style.Add("display", "")
            Me.hplShowOccupation.Style.Add("display", "none")
            Me.hplHideOccupation.Style.Add("display", "")
        Else
            Me.DivOccupation.Style.Add("display", "none")
            Me.hplShowOccupation.Style.Add("display", "")
            Me.hplHideOccupation.Style.Add("display", "none")
        End If
    End Sub

    Sub LoadRatesSelect()
        ddlShowRates.Items.Clear()
        ddlShowRates.Items.Add(PortalCulture.GetString("01368"))
        ddlShowRates.Items.Add(PortalCulture.GetString("01369"))
    End Sub

    Public Function GetFareFor(ByVal target As String, ByVal isNetRate As Boolean) As Double
        Dim input As TextBox = Me.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", ""))
        Dim value As Double = 0
        If input IsNot Nothing Then Double.TryParse(input.Text, value)
        Return value
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aqu� el c�digo de usuario para inicializar la p�gina
        Me.lblDateError.Visible = False
        If Not IsPostBack Then
            loadDatos()
            LoadRatesSelect()
            Me.ActivateShowButtons()
        Else
            'Verifica validadores
            Me.reqAdultFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
            Me.reqChildFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
            Me.reqTeenFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
        End If

        If Not CType(Me.Page, PaginaBase).isConfigAdolescente Then
            lblExtraAdolescente.Visible = False
            txtExtraTeenPrice.Visible = False
            txtTeenFare.Visible = False
            lblAdolescentePrice.Visible = False

            Me.reqExtraTeenPrice.Visible = False
            Me.reqTeenFare.Visible = False
        End If
        If TypeOf Me.Page Is FaresCatalogue Then
            Me.txtChildFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogue).IdDgChild & "','txtChildrenFare'" & ",'" & Me.txtChildFare.ClientID & "')")
            Me.txtAdultFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogue).IdDgAdult & "','txtAdultFare'" & ",'" & Me.txtAdultFare.ClientID & "')")
            Me.txtTeenFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogue).IdDgTeen & "','txtTeenFare'" & ",'" & Me.txtTeenFare.ClientID & "')")
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
    Public Sub filterRac()
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation
        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(m_iHotelId, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With
        Me.RatePlanRow = Nothing
        Dim dv As DataView
        dv = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
        dv.RowFilter = RatePlanData.FIELD_SegmentRacPrinc & "=1 and " & RatePlanData.FIELD_SEGMENT & "='R'"
        ddlrateplans.DataTextField = RatePlanData.FIELD_CODIGOTARIFA
        ddlrateplans.DataValueField = RatePlanData.FIELD_IDRATEPLAN
        Me.ddlrateplans.DataSource = dv
        Me.ddlrateplans.DataBind()
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

    Private Sub loadDatos()
        Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
        Me.txtDateTo.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            If Not CType(Me.Page, PaginaBase).IsUsuarioHomeAgency Then
                ds = .GetRatePlanByIdHotel(m_iHotelId, idAsociacion:=idAsoc, DeleteFilter:=1)
            Else
                ds = .GetRatePlanByConvenio(m_iHotelId, IdUsuario:=CType(Me.Page, PaginaBase).Usuario)
            End If
        End With
        Me.RatePlanRow = Nothing
        Dim dv As DataView
        dv = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
        dv.RowFilter = RatePlanData.FIELD_SegmentRacPrinc & "=1 and " & RatePlanData.FIELD_SEGMENT & "='R'"

        'Se agrego esto para agregar los rates plan si preguntar si se tiene un RATEPLAN RAC        
        loadAllRatesplans()
        If TypeOf Me.Page Is FaresCatalogue Then
            CType(Me.Page, FaresCatalogue).loadDatos()
        End If
        Exit Sub

        'si hay tarifa rack la tarifa se enlazar� con dicha tarifa, 
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
                If TypeOf Me.Page Is FaresCatalogue Then
                    CType(Me.Page, FaresCatalogue).loadDatos()
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
        'Me.chkWaitListAvailable.Text = PortalCulture.GetString("01350")

        Me.reqAdultFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqChildFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraAdultPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraChildPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraTeenPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqTeenFare.ErrorMessage = PortalCulture.GetString("M000179")

        Me.hplShowRules.Text = PortalCulture.GetString("00327")
        Me.hplHideRules.Text = PortalCulture.GetString("00328")
        lblReservationRules.Text = PortalCulture.GetString("00106")
        Me.lblStartDate.Text = PortalCulture.GetString("00108", True)
        Me.lblEndDate.Text = PortalCulture.GetString("00109", True)
        Me.lblDateErrorSign.Text = PortalCulture.GetString("00112")
        'Me.lblPreciosTarifa.Text = PortalCulture.GetString("00131")

        Me.lblPromotion.Text = PortalCulture.GetString("00559", True)
        Me.RVPromotion.ErrorMessage = PortalCulture.GetString("00562")

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
            If Not CType(Me.Page, PaginaBase).IsUsuarioHomeAgency Then
                ' Activos (DeleteFilter:=1)
                dsActivos = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, 0, idAsociacion:=idAsoc, DeleteFilter:=1)
                ' Todos incluyendo inactivos (DeleteFilter:=-1)
                ds = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, 0, idAsociacion:=idAsoc, DeleteFilter:=-1)
            Else
                dsActivos = .GetRatePlanByConvenio(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, 0, IdUsuario:=CType(Me.Page, PaginaBase).Usuario, idAsociacion:=idAsoc, DeleteFilter:=1)
                ds = .GetRatePlanByConvenio(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, 0, IdUsuario:=CType(Me.Page, PaginaBase).Usuario, idAsociacion:=idAsoc, DeleteFilter:=-1)

                Dim dr As DataRow
                dr = ds.Tables("RatePlans").NewRow
                dr("idRatePlan") = "RAC"
                Try
                    dr("Nombre") = "Est�ndar"
                Catch
                    dr("name") = "Est�ndar"
                End Try
                ds.Tables("RatePlans").ImportRow(dr)
                ds.Tables("RatePlans").Rows.Add(dr)
            End If
        End With

        ' IDs de planes activos para saber cu�les son inactivos
        Dim activosIds As New HashSet(Of String)
        For Each row As DataRow In dsActivos.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            activosIds.Add(row(RatePlanData.FIELD_IDRATEPLAN).ToString())
        Next

        ' Links (igual que antes)
        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(Me.m_iHotelId, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With

        ' Columna "texto" = "CODIGO -- Nombre" truncado
        ds.Tables(RatePlanData.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"),
        "substring(" & RatePlanData.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & RatePlanData.FIELD_NAME & ",1,25)")
        ds.Tables(RatePlanData.RATEPLAN_TABLE).AcceptChanges()

        ' Habitación viene como parámetro desde la página padre
        Dim idHabitacionActual As Integer = idHabitacion

        ' ?? Poblar el dropdown agrupado ??
        ddlrateplans.Items.Clear()

        ' GRUPO 1: ACTIVOS � planes con tarifas para la habitaci�n actual van primero
        ddlrateplans.Items.Add(New ListItem("?? Planes Activos ??", "__GRP_ACTIVOS__"))

        ' Primero: activos que tienen tarifas para la habitación seleccionada
        If idHabitacionActual > 0 Then
            For Each row As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
                Dim idPlan As String = row(RatePlanData.FIELD_IDRATEPLAN).ToString()
                If activosIds.Contains(idPlan) AndAlso TieneTarifaParaHabitacion(idPlan, idHabitacionActual) Then
                    ddlrateplans.Items.Add(New ListItem("? " & row("texto").ToString(), idPlan))
                End If
            Next
        End If

        ' Despu�s: el resto de activos
        For Each row As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            Dim idPlan As String = row(RatePlanData.FIELD_IDRATEPLAN).ToString()
            If activosIds.Contains(idPlan) Then
                ' Evitar duplicados si ya fue agregado arriba
                If idHabitacionActual = 0 OrElse Not TieneTarifaParaHabitacion(idPlan, idHabitacionActual) Then
                    ddlrateplans.Items.Add(New ListItem(row("texto").ToString(), idPlan))
                End If
            End If
        Next

        ' GRUPO 2: INACTIVOS
        ddlrateplans.Items.Add(New ListItem("?? Planes Inactivos ??", "__GRP_INACTIVOS__"))
        For Each row As DataRow In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            Dim idPlan As String = row(RatePlanData.FIELD_IDRATEPLAN).ToString()
            If Not activosIds.Contains(idPlan) Then
                ddlrateplans.Items.Add(New ListItem(row("texto").ToString(), idPlan))
            End If
        Next

        Return ds
    End Function

    ' Helper: consulta si un plan tiene tarifas guardadas para la habitación
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
                txtDescripcion.Update("Only Room", "Solo Habitaci�n", idDic)
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
        Dim strError As String = ""
        Dim datFare As New FaresData
        Dim ExistCode As New FaresData
        Dim rowFare As DataRow
        'Dim sdatodespues As String
        ' Dim dv As DataView
        lblError.Text = "entro"
        If RatePlanRow Is Nothing Then
            If Not SaveSegmentRac() Then
                Return False
            End If
        End If
        lblError.Text = "segmento"
        Dim a As String
        Dim tex As String
        For i As Integer = 0 To lstDates.Items.Count - 1
            a = a & lstDates.Items(i).Value & "/"
            tex = tex & lstDates.Items(i).Text & "/"
        Next

        'buscar el codigo que le pertenece
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idRoom)
        End With
        If room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
        lblError.Text = "Rooms"
        Try
            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()
                rowFare(FaresData.ENDDATE_FIELD) = f2
                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = ConvDouble(txtExtraAdultPrice.Text)
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = ConvDouble(txtExtraChildPrice.Text)
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = ConvDouble(txtExtraTeenPrice.Text)

                rowFare(FaresData.NINIORATE) = 0
                rowFare(FaresData.RATEENPRICE_FIELD) = 0
                rowFare(FaresData.PRICE_FIELD) = 0

                Double.TryParse(Me.txtChildFare.Text, rowFare(FaresData.NINIORATE))
                Double.TryParse(txtTeenFare.Text, rowFare(FaresData.RATEENPRICE_FIELD))
                Double.TryParse(Me.txtAdultFare.Text, rowFare(FaresData.PRICE_FIELD))

                rowFare(FaresData.STARTDATE_FIELD) = f1
                rowFare(FaresData.EXCEPTION_FIELD) = Me.Exceptions
                If Me.txtAdvBooking.Text <> "" Then
                    rowFare(FaresData.ADVBOOKING_FIELD) = CInt(Val(Me.txtAdvBooking.Text))
                End If
                If Me.txtMaxAdvBooking.Text <> "" Then
                    rowFare(FaresData.MAXADVBOOKING_FIELD) = CInt(Val(Me.txtMaxAdvBooking.Text))
                End If
                If Me.txtMinDias.Text <> "" Then
                    rowFare(FaresData.MINDIAS_FIELD) = CInt(Val(Me.txtMinDias.Text))
                End If
                If Me.txtMaxDias.Text <> "" Then
                    rowFare(FaresData.MAXDIAS_FIELD) = CInt(Val(Me.txtMaxDias.Text))
                End If
                rowFare(FaresData.RULESDEFAULT) = chkRules.Checked
                rowFare(FaresData.NOARRIVOS_FIELD) = GetArrivosField()
                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idRoom
                rowFare(FaresData.RATETYPE_FIELD) = RatePlanRow.SEGMENT
                rowFare(FaresData.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
                rowFare(FaresData.RATECODE_FIELD) = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ROOM_CODE) & RatePlanRow.RATECODE
                'rowFare(FaresData.WAITLISTAVAILABLE_FIELD) = Me.chkWaitListAvailable.Checked

                rowFare(FaresData.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.Insert()

                'Promociones, Ventanas, Ocupacion
                If txtDescProm.Text.Trim <> "" Then
                    rowFare(FaresData.DESCPROMOTION_FIELD) = txtDescProm.Text
                Else
                    rowFare(FaresData.DESCPROMOTION_FIELD) = DBNull.Value
                End If

                If lstPeoplesInRoom.SelectedValue <> -1 Then
                    rowFare(FaresData.PERSONAS_FIELD) = lstPeoplesInRoom.SelectedValue
                Else
                    rowFare(FaresData.PERSONAS_FIELD) = DBNull.Value
                End If

                If lstMinNumberAdults.SelectedValue <> -1 Then
                    rowFare(FaresData.MINADULTOS_FIELD) = lstMinNumberAdults.SelectedValue
                Else
                    rowFare(FaresData.MINADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberAdults.SelectedValue <> -1 Then
                    rowFare(FaresData.MAXADULTOS_FIELD) = lstNumberAdults.SelectedValue
                Else
                    rowFare(FaresData.MAXADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberChildrens.SelectedValue <> -1 Then
                    rowFare(FaresData.MAXNINIOS_FIELD) = lstNumberChildrens.SelectedValue
                Else
                    rowFare(FaresData.MAXNINIOS_FIELD) = DBNull.Value
                End If

                If lstPeoplesExtras.SelectedValue <> -1 Then
                    rowFare(FaresData.PERSONASEXTRAS_FIELD) = lstPeoplesExtras.SelectedValue
                Else
                    rowFare(FaresData.PERSONASEXTRAS_FIELD) = DBNull.Value
                End If

                If chkBookingWindow.Checked Then
                    Try
                        rowFare(FaresData.BOOKINGWINDOWSTART_FIELD) = CDate(txtBookWindowDateFrom.Text)
                        rowFare(FaresData.BOOKINGWINDOWEND_FIELD) = CDate(txtBookWindowDateTo.Text)
                    Catch
                        rowFare(FaresData.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                        rowFare(FaresData.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                    End Try
                Else
                    rowFare(FaresData.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                    rowFare(FaresData.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                End If
                .Rows.Add(rowFare)
            End With

            With New FaresSystem
                Try
                    If .InsertFares(datFare, strError) Then
                        idtar = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PKIDFARES_FIELD)
                        Addrateplan(datFare, "Descr_rateplan", descr)
                        sdato = Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRate", datFare)
                        'esta condici�n es para cuando se autollenaran las tarifasrestricciones
                        If Me.Adultos > 0 Then
                            lblError.Visible = True
                            lblError.Text = "restricciones"
                            If Not SaveFaresRestrictions(idtar) Then Return False
                        End If
                    End If
                    lblError.Visible = True
                    lblError.Text = strError
                Catch ex As Exception
                    lblError.Visible = True
                    lblError.Text = ex.Message
                    'Catch ex As OverflowException
                    Me.lblDateError.Visible = True
                    Return False
                End Try
            End With
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message
            Return False
        End Try
        Return True
    End Function

    Function ConvDouble(ByVal valor As String) As Double
        Return If(String.IsNullOrEmpty(valor), 0, Double.Parse(valor))
    End Function

    Private Function UpdateFare(ByVal idroom As Integer, ByVal f1 As Date, ByVal f2 As Date, ByRef dsBefore As DataSet, ByRef dsFareUp As FaresData, Optional ByVal publish As Boolean = True) As Boolean
        Dim datFares As New FaresData
        Dim ExistCode As New FaresData
        Dim fareRow As DataRow
        Dim bResult As Boolean
        Me.lblDateError.Visible = False

        dsBefore = (New FaresSystem).GetFareByFareId(Me.m_iFareId)
        With datFares
            fareRow = .Tables(.FARES_TABLE).NewRow()
            Try
                ' try to fill fare row data
                fareRow(.PKIDFARES_FIELD) = Me.m_iFareId
                fareRow(.ENDDATE_FIELD) = Format(f2, "yyyy/MM/dd")
                fareRow(.EXTRAADULTPRICE_FIELD) = ConvDouble(Me.txtExtraAdultPrice.Text)
                fareRow(.EXTRACHILDPRICE_FIELD) = ConvDouble(Me.txtExtraChildPrice.Text)
                fareRow(.EXTRATEENPRICE_FIELD) = ConvDouble(Me.txtExtraTeenPrice.Text)

                fareRow(.RATEENPRICE_FIELD) = 0
                fareRow(.NINIORATE) = 0
                fareRow(.PRICE_FIELD) = 0

                Double.TryParse(Me.txtTeenFare.Text, fareRow(.RATEENPRICE_FIELD))
                Double.TryParse(Me.txtChildFare.Text, fareRow(.NINIORATE))
                Double.TryParse(Me.txtAdultFare.Text, fareRow(.PRICE_FIELD))

                fareRow(FaresData.EXCEPTION_FIELD) = Me.Exceptions
                fareRow(.HOTELROOMTYPEID_FIELD) = idroom
                fareRow(.STARTDATE_FIELD) = Format(CDate(f1), "yyyy/MM/dd")
                'fareRow(.WAITLISTAVAILABLE_FIELD) = Me.chkWaitListAvailable.Checked

                If txtAdvBooking.Text <> "" Then
                    fareRow(FaresData.ADVBOOKING_FIELD) = CInt(Val(Me.txtAdvBooking.Text))
                End If
                If txtMaxAdvBooking.Text <> "" Then
                    fareRow(FaresData.MAXADVBOOKING_FIELD) = CInt(Val(Me.txtMaxAdvBooking.Text))
                End If
                If txtMinDias.Text <> "" Then
                    fareRow(FaresData.MINDIAS_FIELD) = CInt(Val(Me.txtMinDias.Text))
                End If
                If txtMaxDias.Text <> "" Then
                    fareRow(FaresData.MAXDIAS_FIELD) = CInt(Val(Me.txtMaxDias.Text))
                End If
                fareRow(FaresData.NOARRIVOS_FIELD) = GetArrivosField()
                fareRow(FaresData.RULESDEFAULT) = chkRules.Checked

                If Me.txtPromoDescription.IdIndice = 0 Then
                    fareRow(FaresData.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.Insert()
                Else
                    fareRow(FaresData.IDDICCDESCPROM_FIELD) = Me.txtPromoDescription.IdIndice
                    Me.txtPromoDescription.Update(Me.txtPromoDescription.IdIndice, publish:=publish)
                End If

                fareRow(FaresData.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
                'Promociones, Ventanas, Ocupacion
                If txtDescProm.Text.Trim <> "" Then
                    fareRow(FaresData.DESCPROMOTION_FIELD) = txtDescProm.Text
                Else
                    fareRow(FaresData.DESCPROMOTION_FIELD) = DBNull.Value
                End If

                If lstPeoplesInRoom.SelectedValue <> -1 Then
                    fareRow(FaresData.PERSONAS_FIELD) = lstPeoplesInRoom.SelectedValue
                Else
                    fareRow(FaresData.PERSONAS_FIELD) = DBNull.Value
                End If

                If lstMinNumberAdults.SelectedValue <> -1 Then
                    fareRow(FaresData.MINADULTOS_FIELD) = lstMinNumberAdults.SelectedValue
                Else
                    fareRow(FaresData.MINADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberAdults.SelectedValue <> -1 Then
                    fareRow(FaresData.MAXADULTOS_FIELD) = lstNumberAdults.SelectedValue
                Else
                    fareRow(FaresData.MAXADULTOS_FIELD) = DBNull.Value
                End If

                If lstNumberChildrens.SelectedValue <> -1 Then
                    fareRow(FaresData.MAXNINIOS_FIELD) = lstNumberChildrens.SelectedValue
                Else
                    fareRow(FaresData.MAXNINIOS_FIELD) = DBNull.Value
                End If

                If lstPeoplesExtras.SelectedValue <> -1 Then
                    fareRow(FaresData.PERSONASEXTRAS_FIELD) = lstPeoplesExtras.SelectedValue
                Else
                    fareRow(FaresData.PERSONASEXTRAS_FIELD) = DBNull.Value
                End If

                If chkBookingWindow.Checked Then
                    Try
                        fareRow(FaresData.BOOKINGWINDOWSTART_FIELD) = CDate(txtBookWindowDateFrom.Text)
                        fareRow(FaresData.BOOKINGWINDOWEND_FIELD) = CDate(txtBookWindowDateTo.Text)
                    Catch
                        fareRow(FaresData.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                        fareRow(FaresData.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                    End Try
                Else
                    fareRow(FaresData.BOOKINGWINDOWSTART_FIELD) = DBNull.Value
                    fareRow(FaresData.BOOKINGWINDOWEND_FIELD) = DBNull.Value
                End If



                .Tables(.FARES_TABLE).Rows.Add(fareRow)
                ' set row state to modified
                fareRow.AcceptChanges()
                fareRow(.PKIDFARES_FIELD) = fareRow(.PKIDFARES_FIELD)
                With New FaresSystem
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
                    Me.m_iFareId = .Tables(.FARES_TABLE).Rows(0)(.PKIDFARES_FIELD)
                End If
            Catch ex As Exception
                Return False
            End Try
        End With
        Return True
    End Function

    Private Function SaveFaresRestrictions(ByVal idTarifa As Integer) As Boolean
        Dim datRestrictions As New FaresRestrictionsData
        For idxAdults As Integer = 1 To Me.Adultos
            For idxChild As Integer = 0 To Me.Ninios
                'Combinaciond de adultos - ni�os
                Dim newRow As DataRow = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).NewRow()
                With newRow
                    .Item(FaresRestrictionsData.ADULTFARE_FIELD) = 0
                    .Item(FaresRestrictionsData.CHILDFARE_FIELD) = 0
                    Double.TryParse(Me.txtAdultFare.Text, .Item(FaresRestrictionsData.ADULTFARE_FIELD))
                    Double.TryParse(Me.txtChildFare.Text, .Item(FaresRestrictionsData.CHILDFARE_FIELD))

                    .Item(FaresRestrictionsData.ADULTNUMBER_FIELD) = idxAdults
                    .Item(FaresRestrictionsData.CHILDNUMBER_FIELD) = idxChild
                    .Item(FaresRestrictionsData.IDFARE_FIELD) = idTarifa
                    .Item(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = 0
                End With
                datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
            Next
        Next

        With New FaresRestrictionSystem
            lblError.Visible = True
            lblError.Text = "guarda restricciones"
            SaveFaresRestrictions = .InsertFaresRestrictions(datRestrictions)
        End With
    End Function

    Sub CtrlExtras()
        If lstPeoplesExtras.Items.Count > 2 Then
            Me.txtExtraAdultPrice.Text = ""
            Me.txtExtraChildPrice.Text = ""
            Me.txtExtraTeenPrice.Text = ""
        Else
            Me.txtExtraAdultPrice.Text = ""
            Me.txtExtraChildPrice.Text = ""
            Me.txtExtraTeenPrice.Text = ""
        End If
        txtExtraAdultPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        reqExtraAdultPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraChildPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        reqExtraChildPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraTeenPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        reqExtraTeenPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
    End Sub

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
        CtrlExtras()

        Dim ck As CheckBox
        For i As Integer = 1 To 7
            ck = Me.FindControl("Chk" & i)
            ck.Checked = False
        Next
        Me.ddlrateplans.Enabled = True
        Me.lstDates.Items.Clear()

        lstDates.Items.Add(PortalCulture.GetString("00317", True))
        Me.txtFechas.Text = ""
        Me.lblDateError.Visible = False
        chkRules.Checked = True
        'Me.chkWaitListAvailable.Enabled = True

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

        Me.ActivateShowButtons()

    End Sub

    '--------------------------------------------------------------
    'Esto es utilizado cuando la tarifa ya existe
    Public Sub LoadFare(ByVal iFareId As Integer, Optional ByVal load As Integer = 1)

        Dim datFares As DataSet
        Dim rowFare As DataRow

        m_iFareId = iFareId
        ' get fare information
        With New FaresSystem
            datFares = .GetFareByFareId(iFareId)
        End With

        If Not datFares Is Nothing AndAlso datFares.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then

            Me.ddlrateplans.Enabled = True

            rowFare = datFares.Tables(FaresData.FARES_TABLE).Rows(0)
            Me.chkRules.Checked = rowFare(FaresData.RULESDEFAULT)
            'Me.chkWaitListAvailable.Checked = rowFare(FaresData.WAITLISTAVAILABLE_FIELD)
            Try
                Me.ddlrateplans.SelectedValue = rowFare(FaresData.IDRATEPLAN_FIELD).ToString.ToUpper
            Catch ex As Exception

            End Try

            If Not rowFare(FaresData.SEGMENTPRINCIPAL) Is System.DBNull.Value AndAlso rowFare(FaresData.SEGMENTPRINCIPAL) = True Then
                Me.ddlrateplans.Enabled = False
            End If
            Me.txtDateFrom.Text = CType(rowFare(FaresData.STARTDATE_FIELD), DateTime).ToString("MM/dd/yyyy")
            Me.txtDateTo.Text = CType(rowFare(FaresData.ENDDATE_FIELD), DateTime).ToString("MM/dd/yyyy")
            Me.txtAdultFare.Text = CDbl(rowFare(FaresData.PRICE_FIELD))
            Me.txtChildFare.Text = CDbl(Val(rowFare(FaresData.NINIORATE).ToString))

            If txtExtraAdultPrice.Enabled Then
                Me.txtExtraAdultPrice.Text = CDbl(Val(rowFare(FaresData.EXTRAADULTPRICE_FIELD)))
            End If
            If txtExtraChildPrice.Enabled Then
                Me.txtExtraChildPrice.Text = CDbl(Val(rowFare(FaresData.EXTRACHILDPRICE_FIELD)))
            End If
            If txtExtraTeenPrice.Enabled Then
                txtExtraTeenPrice.Text = CDbl(IIf(rowFare(FaresData.EXTRATEENPRICE_FIELD) Is DBNull.Value,
                                                  0, rowFare(FaresData.EXTRATEENPRICE_FIELD)))
            End If

            txtTeenFare.Text = CDbl(IIf(rowFare(FaresData.RATEENPRICE_FIELD) Is DBNull.Value,
                                             0, rowFare(FaresData.RATEENPRICE_FIELD)))


            Me.Exceptions = rowFare(FaresData.EXCEPTION_FIELD)
            Me.txtMinDias.Text = ""
            Me.txtMaxDias.Text = ""
            Me.txtAdvBooking.Text = ""
            Me.txtMaxAdvBooking.Text = ""

            If Not rowFare.IsNull(FaresData.IDDICCDESCPROM_FIELD) Then
                Me.txtPromoDescription.CargaDatos(rowFare(FaresData.IDDICCDESCPROM_FIELD))
            Else
                Me.txtPromoDescription.CargaDatos(0)
            End If

            If Not rowFare.IsNull(FaresData.MINDIAS_FIELD) Then
                txtMinDias.Text = CInt(Val(rowFare(FaresData.MINDIAS_FIELD)))
            End If

            If Not rowFare.IsNull(FaresData.MAXDIAS_FIELD) Then
                txtMaxDias.Text = CInt(Val(rowFare(FaresData.MAXDIAS_FIELD)))

            End If

            If Not rowFare.IsNull(FaresData.NOARRIVOS_FIELD) Then
                getNoArrrivalsField(rowFare(FaresData.NOARRIVOS_FIELD))

            End If

            If Not rowFare.IsNull(FaresData.ADVBOOKING_FIELD) Then
                txtAdvBooking.Text = CInt(Val(rowFare(FaresData.ADVBOOKING_FIELD)))
            End If
            If Not rowFare.IsNull(FaresData.MAXADVBOOKING_FIELD) Then
                txtMaxAdvBooking.Text = CInt(Val(rowFare(FaresData.MAXADVBOOKING_FIELD)))
            End If

            'Promocion y Ventana de Reserva, ocupacion
            If Not rowFare.IsNull(FaresData.DESCPROMOTION_FIELD) Then
                txtDescProm.Text = CType(rowFare(FaresData.DESCPROMOTION_FIELD), Decimal).ToString("00.00")
            Else
                txtDescProm.Text = ""
            End If

            chkBookingWindow.Checked = False
            If Not rowFare.IsNull(FaresData.BOOKINGWINDOWSTART_FIELD) AndAlso Not rowFare.IsNull(FaresData.BOOKINGWINDOWEND_FIELD) Then
                chkBookingWindow.Checked = True
                txtBookWindowDateFrom.Text = CDate(rowFare(FaresData.BOOKINGWINDOWSTART_FIELD)).ToString("MM/dd/yyyy")
                txtBookWindowDateTo.Text = CDate(rowFare(FaresData.BOOKINGWINDOWEND_FIELD)).ToString("MM/dd/yyyy")
            Else
                Me.txtBookWindowDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
                Me.txtBookWindowDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
            End If
            tbBookingWindow.Style("display") = IIf(chkBookingWindow.Checked, "", "none")

            lstPeoplesInRoom.SelectedIndex = 0
            If Not rowFare.IsNull(FaresData.PERSONAS_FIELD) Then
                lstPeoplesInRoom.SelectedIndex = lstPeoplesInRoom.Items.IndexOf(lstPeoplesInRoom.Items.FindByValue(rowFare(FaresData.PERSONAS_FIELD)))
            End If

            lstPeoplesExtras.SelectedIndex = 0
            If Not rowFare.IsNull(FaresData.PERSONASEXTRAS_FIELD) Then
                lstPeoplesExtras.SelectedIndex = lstPeoplesExtras.Items.IndexOf(lstPeoplesExtras.Items.FindByValue(rowFare(FaresData.PERSONASEXTRAS_FIELD)))
            End If

            lstMinNumberAdults.SelectedIndex = 0
            If Not rowFare.IsNull(FaresData.MINADULTOS_FIELD) Then
                lstMinNumberAdults.SelectedIndex = lstMinNumberAdults.Items.IndexOf(lstMinNumberAdults.Items.FindByValue(rowFare(FaresData.MINADULTOS_FIELD)))
            End If

            lstNumberAdults.SelectedIndex = 0
            If Not rowFare.IsNull(FaresData.MAXADULTOS_FIELD) Then
                lstNumberAdults.SelectedIndex = lstNumberAdults.Items.IndexOf(lstNumberAdults.Items.FindByValue(rowFare(FaresData.MAXADULTOS_FIELD)))
            End If

            lstNumberChildrens.SelectedIndex = 0
            If Not rowFare.IsNull(FaresData.MAXNINIOS_FIELD) Then
                lstNumberChildrens.SelectedIndex = lstNumberChildrens.Items.IndexOf(lstNumberChildrens.Items.FindByValue(rowFare(FaresData.MAXNINIOS_FIELD)))
            End If

            Me.ActivateShowButtons()

            Me.m_Modo = "MODIFY"
            If Me.txtMinDias.Text = "0" Then Me.txtMinDias.Text = "1"
            Me.HasData = True
            'Me.Page.RegisterStartupScript("", "<script>AddDate('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');</script>")
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
        If String.IsNullOrEmpty(txtTeenFare.Text) Then
            txtTeenFare.Text = ""
        End If

        If Me.txtBookWindowDateFrom.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        If Me.txtBookWindowDateTo.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
        End If

    End Sub

    Function Nota(ByVal rooom As String, ByVal f1last As String, ByVal f2last As String, ByVal rp As String, ByVal f1 As String, ByVal f2 As String, ByRef sreference As String) As String
        Dim msg As String = "Se modific� la tarifa de la habitaci�n " & rooom & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.RatePlanRow.RATECODE
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

    Public Function SetMinRate(ByVal FareAdultMin As Double, ByVal FareChildMin As Double, ByVal FareJuniorMin As Double, ByVal isOcupacion As Boolean) As Boolean
        If isOcupacion Then
            txtAdultFare.Text = FareAdultMin
            txtChildFare.Text = FareChildMin
            txtTeenFare.Text = FareJuniorMin
        End If
    End Function

    Public Function AddFare(ByVal idRoom As Integer, ByRef idFare As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String, ByVal rp As String, ByVal f1last As String, ByVal f2last As String, ByVal sroom As String, ByVal publish As Boolean, ByVal isOcupacion As Boolean, ByVal FareAdultMin As Double, ByVal FareChildMin As Double, ByVal FareJuniorMin As Double, ByRef scorreo As String) As Boolean

        Dim flag As Boolean = False
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
                    scorreo = (New Util.Utility).GeneraCorreoXslt("", sdato)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Crear, "Se cre� la tarifa de la habitaci�n " & ch.Substring(0, ch.IndexOf("--")) & " de la fecha " & f1 & " a la fecha " & f2 & " con el rateplan " & Me.RatePlanRow.RATECODE, "", "", sdato)
                    flag = True
                End If
            Else
                Dim dsFaresUp As New FaresData
                Dim dsBefore As New DataSet

                If UpdateFare(idRoom, f1, f2, dsBefore, dsFaresUp, publish) Then

                    Dim sreference As String = ""

                    Addrateplan(dsBefore, "Descr_rateplan", String.Format("{0} {1}", sroom, rp))
                    sdato = Util.Utility.GetXml(dsFaresUp.FARES_TABLE, "UpdateRate", dsBefore)

                    Addrateplan(dsFaresUp, "Descr_rateplan", String.Format("{0} {1}", sroom, rp))
                    sdatodespues = Util.Utility.GetXml(dsFaresUp.FARES_TABLE, "UpdateRate", dsFaresUp)
                    'sdatocorreo = CreateAvailHtml(dsBefore, dsFaresUp)
                    scorreo = (New Util.Utility).GeneraCorreoXslt(sdato, sdatodespues)
                    Dim snota As String = Nota(ch, f1last, f2last, rp, f1, f2, sreference)
                    'CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Modificar, "Se modific� la tarifa de la habitaci�n " & ch & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.RatePlanRow.RATECODE, "Update Rate", sdato, sdatodespues, sdatocorreo)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Modificar, snota, sreference, sdato, sdatodespues)
                    Dim usuario As String = ""
                    flag = True

                End If
                m_iFareId = 0
            End If
            If Me.txtPromoDescription.HasChanges Then CType(Me.Page, PaginaBase).NotifyContentModification("Tarifa de la habitaci�n " & ch & ", y plan tarifario " & Me.RatePlanRow.RATECODE, "Tarifa De Habitac��n")
        End If
        Return flag
    End Function

    Public Function CreateAvailHtml(ByVal dsBefore As DataSet, ByVal dsFares As FaresData) As String
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
            td.Text = Format(dr("NiniosRate"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraAdulto"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraNinio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = IIf(dr.IsNull("Excepciones"), "NNNNNNN", dr("Excepciones"))
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = IIf(dr.IsNull("MinDias"), 0, dr("MinDias"))
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
            td.Text = Format(dr("NiniosRate"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraAdulto"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraNinio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = IIf(dr.IsNull("Excepciones"), "NNNNNNN", dr("Excepciones"))
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = IIf(dr.IsNull("MinDias"), 0, dr("MinDias"))
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

        result += "Se cre� la tarifa de la habitaci�n "
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

    Public Function GetClientID() As String
        Return Me.lstDates.ClientID
    End Function

    Public Function Segment(ByVal Value As String) As String
        Try
            If Value <> "0" Then
                Me.ddlrateplans.SelectedValue = Value
            End If
        Catch ex As Exception
        End Try
    End Function

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
            CtrlExtras()
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

End Class


<Serializable()> Public Class RowRatePlan
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


