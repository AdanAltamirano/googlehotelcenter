Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Runtime.Serialization
Imports System.Text
Imports System.IO

Partial Class ctrRateAplicationNR
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
            VIEWSTATE("_Adultos") = Value
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

    Public Property RatePlanRow() As RowRatePlanNR
        Get
            Return ViewState("_RatePlanRow")
        End Get
        Set(ByVal Value As RowRatePlanNR)
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
    Const KEY_FAREID = "FareId"
    Public Property m_iFareId() As Integer
        Get
            Return ViewState(KEY_FAREID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_FAREID) = Value
        End Set
    End Property

    Public Property m_TextBoxPorcMax() As String
        Get
            Return viewstate("TextBoxPorcMaxClientId")
        End Get
        Set(ByVal Value As String)
            viewstate("TextBoxPorcMaxClientId") = Value
        End Set
    End Property

    Public Property m_TextBoxPorcMin() As String
        Get
            Return viewstate("TextBoxPorcMinClientId")
        End Get
        Set(ByVal Value As String)
            viewstate("TextBoxPorcMinClientId") = Value
        End Set
    End Property

    Public Function GetFareFor(ByVal target As String, ByVal isNetRate As Boolean) As Double
        Dim input As TextBox = Me.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", ""))
        Dim value As Double = 0
        If input IsNot Nothing Then Double.TryParse(input.Text, value)
        Return value
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
    Public ReadOnly Property SelectedddlShowRates() As Integer
        Get
            Return ddlShowRates.SelectedIndex
        End Get

    End Property

    Public Function GetClientID() As String
        Return Me.lstDates.ClientID
    End Function

    Public Property MinPercentControlId() As String
        Get
            Dim result As String = String.Empty
            If Me.ViewState("_minPercentControlId") IsNot Nothing Then result = Me.ViewState("_minPercentControlId")
            Return result
        End Get
        Set(ByVal value As String)
            Dim temp As String = Me.ViewState("_minPercentControlId")
            If temp Is Nothing Then temp = String.Empty
            If temp.Trim().Length > 0 Then temp += ", "
            temp += "#" + value
            Me.ViewState("_minPercentControlId") = temp
        End Set
    End Property

    Public Property MaxPercentControlId() As String
        Get
            Dim result As String = String.Empty
            If Me.ViewState("_maxPercentControlId") IsNot Nothing Then result = Me.ViewState("_maxPercentControlId")
            Return result
        End Get
        Set(ByVal value As String)
            Dim temp As String = Me.ViewState("_maxPercentControlId")
            If temp Is Nothing Then temp = String.Empty
            If temp.Trim().Length > 0 Then temp += ", "
            temp += "#" + value
            Me.ViewState("_maxPercentControlId") = temp
        End Set
    End Property


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

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Private CurrencyScript As New StringBuilder

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
        If Me.txtDescProm.Text.Trim().Length > 0 OrElse (Me.chkBookingWindow.Checked AndAlso (Me.txtBookWindowDateFrom.Text.Trim().Length > 0 OrElse Me.txtBookWindowDateTo.Text.Trim().Length > 0)) OrElse Me.txtPromoDescription.HasValue Then
            Me.DivPromotionAndWindow.Style.Add("display", "")
            Me.hplShowProWin.Style.Add("display", "none")
            Me.hplHideProWin.Style.Add("display", "")
        Else
            Me.DivPromotionAndWindow.Style.Add("display", "none")
            Me.hplShowProWin.Style.Add("display", "")
            Me.hplHideProWin.Style.Add("display", "none")
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Me.reqAdultFare.Enabled = (Me.IsSupervisor)
        Me.reqChildFare.Enabled = (Me.IsSupervisor)
        Me.reqTeenFare.Enabled = (Me.IsSupervisor)
        Me.reqExtraAdultPrice.Enabled = (Me.IsSupervisor)
        Me.reqExtraChildPrice.Enabled = (Me.IsSupervisor)
        Me.reqExtraTeenPrice.Enabled = (Me.IsSupervisor)
        Me.rvAdultFareNR.Enabled = (Me.IsSupervisor)

        Me.reqAdultFareNR.Enabled = Not Me.isUsuarioMixto
        Me.reqChildFareNR.Enabled = Not Me.isUsuarioMixto
        Me.reqTeenFareNR.Enabled = Not Me.isUsuarioMixto

        Me.lblDateError.Visible = False
        If Not IsPostBack Then
            Me.MinPercentControlId = Me.TextBoxPorcMin.ClientID
            Me.MaxPercentControlId = Me.TextBoxPorcMax.ClientID
            loadDatos()
            LoadRatesSelect()
            Me.ActivateShowButtons()
        Else
            If (Me.IsSupervisor) Then
                Me.reqAdultFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
                Me.reqChildFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
                Me.reqTeenFare.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
            End If

            If Not Me.isUsuarioMixto Then
                Me.reqAdultFareNR.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
                Me.reqChildFareNR.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
                Me.reqTeenFareNR.Enabled = (Me.ddlShowRates.SelectedIndex = 0)
            End If
            
        End If

        If Not CType(Me.Page, PaginaBase).isConfigAdolescente Then
            lblTeenPriceNr.Visible = False
            lblTeenPrice.Visible = False
            lblExtraAdolescenteNR.Visible = False
            lblExtraAdolescenteUV.Visible = False

            'reqExtraTeenPrice.Enabled = False
            'reqExtraTeenPriceNR.Enabled = False
            reqExtraTeenPrice.Visible = False
            reqExtraTeenPriceNR.Visible = False

            'reqTeenFare.Enabled = False
            'reqTeenFareNR.Enabled = False
            reqTeenFare.Visible = False
            reqTeenFareNR.Visible = False

            rxvTeenFareNR.Visible = False
            rxvTeenPrice.Visible = False
            valExtraTeenPrice.Visible = False
            valExtraTeenPriceNR.Visible = False

            txtExtraTeenPrice.Visible = False
            txtExtraTeenPriceNR.Visible = False
            txtTeenFare.Visible = False
            txtTeenFareNR.Visible = False
        End If

        If TypeOf Me.Page Is FaresCatalogueNR Then
            'Adultos
            Me.txtAdultFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgAdult & "','txtAdultFare'" & ",'" & Me.txtAdultFare.ClientID & "');CheckValContractUv('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtAdultFareNR.ClientID & "','" & txtAdultFare.ClientID & "','" & lblAdultValMax.ClientID & "','" & lblAdultValMin.ClientID & "');FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgAdult & "','txtAdultFareNR'" & ",'" & Me.txtAdultFareNR.ClientID & "')")
            Me.txtAdultFareNR.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgAdult & "','txtAdultFareNR'" & ",'" & Me.txtAdultFareNR.ClientID & "');CheckValContract('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtAdultFareNR.ClientID & "','" & txtAdultFare.ClientID & "','" & lblAdultValMax.ClientID & "','" & lblAdultValMin.ClientID & "');FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgAdult & "','txtAdultFare'" & ",'" & Me.txtAdultFare.ClientID & "')")

            'Child
            Me.txtChildFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgChild & "','txtChildrenFare'" & ",'" & Me.txtChildFare.ClientID & "')" &
                 "; CheckValContractUv('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID &
                                             "');FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgChild & "','txtChildrenFareNR'" & ",'" & Me.txtChildFareNR.ClientID & "')" & ";")

            Me.txtChildFareNR.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgChild & "','txtChildrenFareNR'" & ",'" & Me.txtChildFareNR.ClientID & "')" &
                                             "; CheckValContract('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID &
                                             "');FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgChild & "','txtChildrenFare'" & ",'" & Me.txtChildFare.ClientID & "')" & ";")

            'Teen
            Me.txtTeenFareNR.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgTeen & "','txtTeenFareNR'" & ",'" & Me.txtTeenFareNR.ClientID & "')" &
                                             "; CheckValContract('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtTeenFareNR.ClientID & "','" & txtTeenFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID &
                                             "');FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgTeen & "','txtTeenFare'" & ",'" & Me.txtTeenFare.ClientID & "')" & ";")

            Me.txtTeenFare.Attributes.Add("onChange", "javascript:FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgTeen & "','txtTeenFare'" & ",'" & Me.txtTeenFare.ClientID & "')" &
                 "; CheckValContractUv('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtTeenFareNR.ClientID & "','" & txtTeenFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID &
                                             "');FillPrices('" & CType(Me.Page, FaresCatalogueNR).IdDgTeen & "','txtTeenFareNR'" & ",'" & Me.txtTeenFareNR.ClientID & "')" & ";")

            'Validacion del minimo y maximo porcentaje de ganancia


            'Validacion de maximos y minimos porcentajes de ganacia de adultos extras y niños extras
            'Extra Adult
            Me.txtExtraAdultPriceNR.Attributes.Add("onChange", "javascript:CheckValContract('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtExtraAdultPriceNR.ClientID & "','" & txtExtraAdultPrice.ClientID & "','" & lblAdultExtValMax.ClientID & "','" & lblAdultExtValMin.ClientID & "')")
            Me.txtExtraAdultPrice.Attributes.Add("onChange", "javascript:CheckValContractUv('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtExtraAdultPriceNR.ClientID & "','" & txtExtraAdultPrice.ClientID & "','" & lblAdultExtValMax.ClientID & "','" & lblAdultExtValMin.ClientID & "')")

            'ExtraChildren
            Me.txtExtraChildPriceNR.Attributes.Add("onChange", "javascript:CheckValContract('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtExtraChildPriceNR.ClientID & "','" & txtExtraChildPrice.ClientID & "','" & lblChildExtValMax.ClientID & "','" & lblChildExtValMin.ClientID & "')")
            Me.txtExtraChildPrice.Attributes.Add("onChange", "javascript:CheckValContractUv('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtExtraChildPriceNR.ClientID & "','" & txtExtraChildPrice.ClientID & "','" & lblChildExtValMax.ClientID & "','" & lblChildExtValMin.ClientID & "')")

            'ExtraTeem
            Me.txtExtraTeenPriceNR.Attributes.Add("onChange", "javascript:CheckValContract('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtExtraTeenPriceNR.ClientID & "','" & txtExtraTeenPrice.ClientID & "','" & lblChildExtValMax.ClientID & "','" & lblChildExtValMin.ClientID & "')")
            Me.txtExtraTeenPrice.Attributes.Add("onChange", "javascript:CheckValContractUv('" & TextBoxPorcMin.ClientID & "','" & TextBoxPorcMax.ClientID & "','" & txtExtraTeenPriceNR.ClientID & "','" & txtExtraTeenPrice.ClientID & "','" & lblChildExtValMax.ClientID & "','" & lblChildExtValMin.ClientID & "')")







        End If

        TextBoxPorcMax.Style.Add("Display", "None")
        TextBoxPorcMin.Style.Add("Display", "None")


        imgAddDate.Attributes.Add("onclick", "javascript:AddDate('" & Me.txtDateFrom.ClientID & "','" & Me.txtDateTo.ClientID & "','" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "','" & PortalCulture.GetString("M000197") & "','" & PortalCulture.GetString("00514") & "');")
        imgDeleteDate.Attributes.Add("onclick", "javascript:DeleteDate('" & Me.lstDates.ClientID & "','" & txtFechas.ClientID & "');")
        Me.hplHideRules.NavigateUrl = "javascript:OcultarRules('0" & "','" & DivRules.ClientID & "','" & hplShowRules.ClientID & "','" & hplHideRules.ClientID & "');"
        hplShowRules.NavigateUrl = "javascript:OcultarRules('1" & "','" & DivRules.ClientID & "','" & hplShowRules.ClientID & "','" & hplHideRules.ClientID & "');"

        m_TextBoxPorcMax = TextBoxPorcMax.ClientID
        m_TextBoxPorcMin = TextBoxPorcMin.ClientID

        'Ocupacion
        Me.hplHideProWin.NavigateUrl = "javascript:OcultarRules('0" & "','" & DivPromotionAndWindow.ClientID & "','" & hplShowProWin.ClientID & "','" & hplHideProWin.ClientID & "');"
        Me.hplShowProWin.NavigateUrl = "javascript:OcultarRules('1" & "','" & DivPromotionAndWindow.ClientID & "','" & hplShowProWin.ClientID & "','" & hplHideProWin.ClientID & "');"

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
        dv = ds.Tables(ds.RATEPLAN_TABLE).DefaultView
        dv.RowFilter = ds.FIELD_SegmentRacPrinc & "=1 and " & ds.FIELD_SEGMENT & "='R'"
        ddlrateplans.DataTextField = ds.FIELD_CODIGOTARIFA
        ddlrateplans.DataValueField = ds.FIELD_IDRATEPLAN
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

        Dim ratePlans As DataTable
        Dim contracts As DataTable
        Dim idAsoc As Integer = Me.GetIdAsociation
        Dim ds As New ContractNetRateData

        With New RatePlanFacade
            ratePlans = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=idAsoc, DeleteFilter:=1).Tables(RatePlanData.RATEPLAN_TABLE)
        End With

        With New ContractNetRateFacade
            ds = .getContractsByIdRatePlanHotel(m_iHotelId, Nothing, idAsociacion:=idAsoc)
            If Not ds Is Nothing Then
                contracts = ds.Tables(ContractNetRateData.CONTRACTNR_TABLE)
            End If
        End With

        CurrencyScript.Append("var rateInfo = new Array();")

        If ratePlans IsNot Nothing AndAlso contracts IsNot Nothing Then
            For Each ratePlan As DataRow In ratePlans.Rows
                Dim contract As DataRow() = contracts.Select(ContractNetRateData.FIELD_IDRATEPLAN + "='" + ratePlan(RatePlanData.FIELD_IDRATEPLAN).ToString() + "'")
                If contract.Length > 0 Then
                    CurrencyScript.Append("rateInfo.push({ " + _
                                              "ratePlan:'" + ratePlan(RatePlanData.FIELD_IDRATEPLAN).ToString() + "', " + _
                                              "currency:'" + ratePlan("Moneda").ToString() + "', " + _
                                              "minPercent:'" + contract(0)(ContractNetRateData.FIELD_PORCENTAJEMINIMO).ToString() + "', " + _
                                              "maxPercent:'" + contract(0)(ContractNetRateData.FIELD_PORCENTAJEMAXIMO).ToString() + "' " + _
                                          "});")
                End If
            Next
        End If

    End Sub

    'Private Sub fillCurrencyData(ByVal ds As RatePlanData)
    '    Dim c As Integer = ds.Tables(ds.RATEPLAN_TABLE).Rows.Count

    '    CurrencyScript.Append("var ratesCurrencies = new Array();")
    '    For i As Integer = 0 To ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count - 1
    '        CurrencyScript.Append("ratesCurrencies.push({ratePlan:'" + ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(i).Item(RatePlanData.FIELD_IDRATEPLAN) + "', currency:'" + ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows(i).Item("Moneda") + "'});")
    '    Next

    'End Sub

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
        With New RatePlanFacade
            ds = .GetNetRatePlanByIdHotel(m_iHotelId)
        End With
        Me.RatePlanRow = Nothing
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            Me.RatePlanRow = New RowRatePlanNR
            Me.RatePlanRow.IDRATEPLAN = ds.Tables(0).Rows(0)(ds.FIELD_IDRATEPLAN)
            Me.RatePlanRow.SEGMENT = ds.Tables(0).Rows(0)(ds.FIELD_SEGMENT)
            Me.RatePlanRow.RATECODE = ds.Tables(0).Rows(0)(ds.FIELD_CODIGOTARIFA)
        Else
            Response.Redirect(GeRequestApplicationPath("/Pages/RatesPlans.aspx"))
        End If
        loadAllRatesplans(1)
        'Dim dv As DataView
        'dv = ds.Tables(ds.RATEPLAN_TABLE).DefaultView
        'dv.RowFilter = ds.FIELD_SegmentRacPrinc & "=1 and " & ds.FIELD_SEGMENT & "='R'"
        ''si hay tarifa rack la tarifa se enlazará con dicha tarifa, 
        'If dv.Count > 0 Then
        '    Me.RatePlanRow = New RowRatePlanNR
        '    Me.RatePlanRow.IDRATEPLAN = dv(0)(ds.FIELD_IDRATEPLAN)
        '    Me.RatePlanRow.SEGMENT = dv(0)(ds.FIELD_SEGMENT)
        '    Me.RatePlanRow.RATECODE = dv(0)(ds.FIELD_CODIGOTARIFA)
        'Else
        '    Me.RatePlanRow = Nothing
        '    If SaveSegmentRac() Then
        '        loadDatos()
        '        loadAllRatesplans()
        '        If TypeOf Me.Page Is FaresCatalogueNR Then
        '            CType(Me.Page, FaresCatalogueNR).loadDatos()
        '        End If
        '    Else
        '        Response.Redirect(Request.ApplicationPat & "/Pages/RatesPlans.aspx")
        '    End If
        'End If
    End Sub

    Private Sub loadCulture()

        Me.lblBookWindowStartDate.Text = PortalCulture.GetString("00108", True)
        Me.lblBookWindowEndDate.Text = PortalCulture.GetString("00109", True)
        Me.lblBookWindowDateErrorSign.Text = PortalCulture.GetString("00112")
        Me.lblPromotion.Text = PortalCulture.GetString("00559", True)
        Me.RVPromotion.ErrorMessage = PortalCulture.GetString("00562")
        Me.lbPersonas.Text = PortalCulture.GetString("00082")
        Me.lblNumberAdults.Text = PortalCulture.GetString("00075", True)
        Me.lblPeoplesExtras.Text = PortalCulture.GetString("00076", True)
        Me.lblNumberChildrens.Text = PortalCulture.GetString("00077", True)
        Me.lblProWin.Text = PortalCulture.GetString("01169", True)
        Me.lblOccupation.Text = PortalCulture.GetString("01170", True)
        Me.hplHideProWin.Text = PortalCulture.GetString("01171")
        Me.hplHideOccupation.Text = PortalCulture.GetString("01171")
        Me.hplShowProWin.Text = PortalCulture.GetString("01172")
        Me.hplShowOccupation.Text = PortalCulture.GetString("01172")
        Me.chkBookingWindow.Text = PortalCulture.GetString("01173")


        Me.hplShowRules.Text = PortalCulture.GetString("00327")
        Me.hplHideRules.Text = PortalCulture.GetString("00328")
        lblReservationRules.Text = PortalCulture.GetString("00106")
        Me.lblStartDate.Text = PortalCulture.GetString("00108", True)
        Me.lblEndDate.Text = PortalCulture.GetString("00109", True)
        Me.lblDateErrorSign.Text = PortalCulture.GetString("00112")
        'Me.lblPreciosTarifa.Text = PortalCulture.GetString("00131")

        Dim dsHotel As HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(m_iHotelId)
        End With
        Dim strIncTax As String

        Response.Write("<script> var updateSeasson='" & PortalCulture.GetString("00609") & "';</script>")

        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            'lblMonTar.Text = PortalCulture.GetString("M000263") & " " & dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("Codigo")
            strIncTax = " " & PortalCulture.GetString("00610") & " "

            If Not dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).IsNull(dsHotel.FIELD_PLUSTAX) Then
                If dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)(dsHotel.FIELD_PLUSTAX) Then
                    strIncTax = " " & PortalCulture.GetString("00611") & " "
                End If
            End If

            If dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("Codigo") = "MXN" Then
                strIncTax &= "<BR> " & PortalCulture.GetString("00667")
            End If

            lblMonTar.Text = strIncTax

        End If

        Me.lblExtraChildPriceUV.Text = PortalCulture.GetString("01130", True)
        Me.lblExtraAdultNR.Text = PortalCulture.GetString("01131", True)
        lblExtraAdultUv.Text = PortalCulture.GetString("01130", True)
        Me.lblExtraChildPriceNR.Text = PortalCulture.GetString("01133", True)
        Me.lblExtraChildPriceUV.Text = PortalCulture.GetString("01132", True)

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
        lblPrecio.Text = PortalCulture.GetString("01126", True)
        lblPrecioNR.Text = PortalCulture.GetString("01127", True).Trim
        lblRatePlan.Text = PortalCulture.GetString("00016", True)
        Me.lblDateError.Text = PortalCulture.GetString("00137")
        Me.LblChildPrice.Text = PortalCulture.GetString("01128", True)
        Me.LblChildPriceNR.Text = PortalCulture.GetString("01129", True)

        Me.chkRules.Text = PortalCulture.GetString("00316")
        lblExtraAdolescenteNR.Text = PortalCulture.GetString("01312", True)
        lblExtraChildPriceUV.Text = PortalCulture.GetString("01132", True)
        lblTeenPriceNr.Text = PortalCulture.GetString("01320", True)
        lblTeenPrice.Text = PortalCulture.GetString("01321", True)
        lblExtraAdolescenteUV.Text = PortalCulture.GetString("01313", True)

        Me.reqExtraAdultPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraChildPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraTeenPrice.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraAdultPriceNR.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraChildPriceNR.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqExtraTeenPriceNR.ErrorMessage = PortalCulture.GetString("M000179")


        Me.reqAdultFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqAdultFareNR.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqChildFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqChildFareNR.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqTeenFare.ErrorMessage = PortalCulture.GetString("M000179")
        Me.reqTeenFareNR.ErrorMessage = PortalCulture.GetString("M000179")

        lstDates.Items.Clear()
        Me.lstDates.Items.Add(PortalCulture.GetString("00317", True))
        For i As Integer = 1 To lstDatesCount()
            Dim it As String = lstDatesItemI(i)
            Me.lstDates.Items.Add(it)
        Next

        lblAdultValMax.Text = PortalCulture.GetString("01135")
        lblAdultValMin.Text = PortalCulture.GetString("01136")
        lblChildValMax.Text = PortalCulture.GetString("01135")
        lblChildValMin.Text = PortalCulture.GetString("01136")
        lblAdultExtValMax.Text = PortalCulture.GetString("01135")
        lblAdultExtValMin.Text = PortalCulture.GetString("01136")
        lblChildExtValMax.Text = PortalCulture.GetString("01135")
        lblChildExtValMin.Text = PortalCulture.GetString("01136")

        'rxvTeenPriceNr.Text = PortalCulture.GetString("01136")
        'rxvTeenPrice.Text = PortalCulture.GetString("01136")

        lblAdultValMax.Style.Add("display", "none")
        lblAdultValMin.Style.Add("display", "none")
        lblChildValMax.Style.Add("display", "none")
        lblChildValMin.Style.Add("display", "none")
        lblAdultExtValMax.Style.Add("display", "none")
        lblAdultExtValMin.Style.Add("display", "none")
        lblChildExtValMax.Style.Add("display", "none")
        lblChildExtValMin.Style.Add("display", "none")

    End Sub

    Public Function loadAllRatesplans(Optional ByVal incluirPaquetesSegmentoK As Integer = 0) As RatePlanData

        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetNetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, idAsociacion:=idAsoc)
        End With


        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(Me.m_iHotelId, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With
        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & ds.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & ds.FIELD_NAME & ",1,25)")
        ''eliminar los ratesplan que ya tienen links
        Dim dv As DataView
        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = links.FIELD_TargetRatePlan & "='" & r(ds.FIELD_IDRATEPLAN) & "'"
            ';If dv.Count > 0 Then 'OrElse r(ds.FIELD_SEGMENT) = "K" Then
            If incluirPaquetesSegmentoK = 0 Then
                If dv.Count > 0 Or r(ds.FIELD_SEGMENT) = "K" Then
                    r.Delete()
                End If
            Else
                If dv.Count > 0 Then
                    r.Delete()
                End If
            End If


        Next
        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()
        ddlrateplans.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
        ddlrateplans.DataValueField = ds.FIELD_IDRATEPLAN
        ddlrateplans.DataSource = ds
        ddlrateplans.DataBind()

        Dim idContrato As Integer = 0
        Try
            idContrato = CType(ds.Tables(ds.RATEPLAN_TABLE).Rows(0)("idContrato"), Integer)
        Catch ex As Exception

        End Try

        If idContrato <> 0 Then
            Dim contrato As ContractNetRateData
            With (New ContractNetRateFacade)
                contrato = .getContractByIdContract(idContrato, m_iHotelId)
                If Not contrato Is Nothing AndAlso contrato.Tables(0).Rows.Count > 0 Then
                    TextBoxPorcMax.Text = contrato.Tables(0).Rows(0)(contrato.FIELD_PORCENTAJEMAXIMO)
                    TextBoxPorcMin.Text = contrato.Tables(0).Rows(0)(contrato.FIELD_PORCENTAJEMINIMO)
                End If
            End With
        End If

        Return ds
    End Function



    Private Function SaveSegmentRac() As Boolean
        'crear el segmento
        Dim dsRate As New RatePlanData
        Dim rRate As DataRow
        Dim val As Boolean
        Dim idRate, NuevoIdRate As String
        rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
        With rRate
            .Item(dsRate.FIELD_IDRATEPLAN) = "RAC"
            .Item(dsRate.FIELD_DESCRIPTION) = "Only Room"
            .Item(dsRate.FIELD_SEGMENT) = "R"
            .Item(dsRate.FIELD_IDHOTEL) = Me.m_iHotelId
            .Item(dsRate.FIELD_CODIGOTARIFA) = "RAC"
            .Item(dsRate.FIELD_NAME) = "Standard Rate"
        End With
        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)
        Dim idDic, iddic2 As Integer
        Dim txtDescripcion As New CtrlIdioma
        With New RatePlanAccess
            If .InsertRtPlan(dsRate, idDic, iddic2) Then
                txtDescripcion.Update("Only Room", "Solo Habitación", idDic)
            Else
                Return False
            End If
        End With
        Me.RatePlanRow = New RowRatePlanNR
        With dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)
            Me.RatePlanRow.IDRATEPLAN = .Item(RatePlanData.FIELD_IDRATEPLAN)
            Me.RatePlanRow.SEGMENT = .Item(RatePlanData.FIELD_SEGMENT)
        End With
        Return True
    End Function


    Public Function SaveNewFare(ByVal idRoom As Integer, ByRef idtar As Integer, _
                                ByVal f1 As Date, ByVal f2 As Date, ByVal descr As String, ByRef sData As String) As Boolean
        Dim datFare As New FaresData
        Dim ExistCode As New FaresData
        Dim rowFare As DataRow
        Dim dv As DataView

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

        If room.Tables(room.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
        Try
            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()

                rowFare(FaresData.ENDDATE_FIELD) = f2

                rowFare(FaresData.PRICE_FIELD) = 0
                rowFare(FaresData.NINIORATE) = 0
                rowFare(FaresData.RATEENPRICE_FIELD) = 0

                rowFare(FaresData.PRICENR_FIELD) = 0
                rowFare(FaresData.NINIORATENR) = 0
                rowFare(FaresData.RATEENPRICENR_FIELD) = 0

                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = 0
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = 0
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = 0

                rowFare(FaresData.EXTRAADULTPRICENR_FIELD) = 0
                rowFare(FaresData.EXTRACHILDPRICENR_FIELD) = 0
                rowFare(FaresData.EXTRATEENPRICENR_FIELD) = 0

                Double.TryParse(Me.txtAdultFare.Text, rowFare(FaresData.PRICE_FIELD))
                Double.TryParse(Me.txtChildFare.Text, rowFare(FaresData.NINIORATE))
                Double.TryParse(txtTeenFare.Text, rowFare(FaresData.RATEENPRICE_FIELD))

                Double.TryParse(Me.txtAdultFareNR.Text, rowFare(FaresData.PRICENR_FIELD))
                Double.TryParse(Me.txtChildFareNR.Text, rowFare(FaresData.NINIORATENR))
                Double.TryParse(txtTeenFareNR.Text, rowFare(FaresData.RATEENPRICENR_FIELD))

                Double.TryParse(txtExtraAdultPrice.Text, rowFare(FaresData.EXTRAADULTPRICE_FIELD))
                Double.TryParse(txtExtraChildPrice.Text, rowFare(FaresData.EXTRACHILDPRICE_FIELD))
                Double.TryParse(txtExtraTeenPrice.Text, rowFare(FaresData.EXTRATEENPRICE_FIELD))

                Double.TryParse(txtExtraAdultPriceNR.Text, rowFare(FaresData.EXTRAADULTPRICENR_FIELD))
                Double.TryParse(txtExtraChildPriceNR.Text, rowFare(FaresData.EXTRACHILDPRICENR_FIELD))
                Double.TryParse(txtExtraTeenPriceNR.Text, rowFare(FaresData.EXTRATEENPRICENR_FIELD))


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
                rowFare(FaresData.RATECODE_FIELD) = room.Tables(room.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ROOM_CODE) & RatePlanRow.RATECODE

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
                    If .InsertFares(datFare) Then

                        idtar = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PKIDFARES_FIELD)
                        'esta condición es para cuando se autollenaran las tarifasrestricciones
                        If Me.Adultos > 0 Then
                            If Not SaveFaresRestrictions(idtar) Then Return False
                        End If
                        Addrateplan(datFare, "Descr_rateplan", descr)
                        sData = Util.Utility.GetXml(datFare.FARES_TABLE, "UpdateRateNR", datFare)
                    End If
                Catch ex As OverflowException
                    Me.lblDateError.Visible = True
                    Return False
                End Try
            End With

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Sub Addrateplan(ByVal ds As DataSet, ByVal field As String, ByVal valor As String)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    Function getDataXML(ByVal descr As String, ByRef ds As DataSet) As String
        With New FaresSystem
            ds = .GetFareByFareId(m_iFareId)
            Addrateplan(ds, "Descr_rateplan", descr)
        End With
        Return Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRateNR", ds)
    End Function

    Function ConvDouble(ByVal valor As String) As Double
        Return If(String.IsNullOrEmpty(valor), 0, Double.Parse(valor))
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


    Private Function UpdateFare(ByVal idroom As Integer, ByVal f1 As Date, ByVal f2 As Date, _
                                 ByRef sDataPrev As String, ByVal descr As String, ByRef sData As String, ByRef sdatocorreo As String, Optional ByVal publish As Boolean = True) As Boolean
        Dim datFares As New FaresData
        Dim ExistCode As New FaresData
        Dim fareRow As DataRow
        Dim bResult As Boolean
        Dim dsBefore As DataSet

        Me.lblDateError.Visible = False

        sDataPrev = getDataXML(descr, dsBefore)
        With datFares
            fareRow = .Tables(.FARES_TABLE).NewRow()
            Try
                ' try to fill fare row data
                fareRow(.PKIDFARES_FIELD) = Me.m_iFareId
                fareRow(.ENDDATE_FIELD) = Format(f2, "yyyy/MM/dd")

                fareRow(FaresData.EXCEPTION_FIELD) = Me.Exceptions
                fareRow(.HOTELROOMTYPEID_FIELD) = idroom

                fareRow(FaresData.PRICE_FIELD) = 0
                fareRow(FaresData.NINIORATE) = 0
                fareRow(FaresData.RATEENPRICE_FIELD) = 0

                fareRow(FaresData.PRICENR_FIELD) = 0
                fareRow(FaresData.NINIORATENR) = 0
                fareRow(FaresData.RATEENPRICENR_FIELD) = 0

                fareRow(FaresData.EXTRAADULTPRICE_FIELD) = 0
                fareRow(FaresData.EXTRACHILDPRICE_FIELD) = 0
                fareRow(FaresData.EXTRATEENPRICE_FIELD) = 0

                fareRow(FaresData.EXTRAADULTPRICENR_FIELD) = 0
                fareRow(FaresData.EXTRACHILDPRICENR_FIELD) = 0
                fareRow(FaresData.EXTRATEENPRICENR_FIELD) = 0

                Double.TryParse(Me.txtAdultFare.Text, fareRow(FaresData.PRICE_FIELD))
                Double.TryParse(Me.txtChildFare.Text, fareRow(FaresData.NINIORATE))
                Double.TryParse(txtTeenFare.Text, fareRow(FaresData.RATEENPRICE_FIELD))

                Double.TryParse(Me.txtAdultFareNR.Text, fareRow(FaresData.PRICENR_FIELD))
                Double.TryParse(Me.txtChildFareNR.Text, fareRow(FaresData.NINIORATENR))
                Double.TryParse(txtTeenFareNR.Text, fareRow(FaresData.RATEENPRICENR_FIELD))

                Double.TryParse(txtExtraAdultPrice.Text, fareRow(FaresData.EXTRAADULTPRICE_FIELD))
                Double.TryParse(txtExtraChildPrice.Text, fareRow(FaresData.EXTRACHILDPRICE_FIELD))
                Double.TryParse(txtExtraTeenPrice.Text, fareRow(FaresData.EXTRATEENPRICE_FIELD))

                Double.TryParse(txtExtraAdultPriceNR.Text, fareRow(FaresData.EXTRAADULTPRICENR_FIELD))
                Double.TryParse(txtExtraChildPriceNR.Text, fareRow(FaresData.EXTRACHILDPRICENR_FIELD))
                Double.TryParse(txtExtraTeenPriceNR.Text, fareRow(FaresData.EXTRATEENPRICENR_FIELD))

                fareRow(.STARTDATE_FIELD) = Format(CDate(f1), "yyyy/MM/dd")

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
                    Catch ex As OverflowException
                        Me.lblDateError.Visible = True
                        Return False
                    End Try
                End With
                If bResult = True Then
                    Me.m_iFareId = .Tables(.FARES_TABLE).Rows(0)(.PKIDFARES_FIELD)
                    Addrateplan(datFares, "Descr_rateplan", descr)
                    sData = Util.Utility.GetXml(datFares.FARES_TABLE, "UpdateRateNR", datFares)
                    sdatocorreo = "" ' CreateAvailHtml(dsBefore, datFares)
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
                'Combinaciond de adultos - niños
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
            SaveFaresRestrictions = .InsertFaresRestrictions(datRestrictions)
        End With
    End Function

    Sub CtrlExtras()

        Me.txtExtraAdultPrice.Text = ""
        Me.txtExtraAdultPriceNR.Text = ""
        Me.txtExtraChildPrice.Text = ""
        Me.txtExtraChildPriceNR.Text = ""
        Me.txtExtraTeenPrice.Text = ""
        Me.txtExtraTeenPriceNR.Text = ""

        txtExtraAdultPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraAdultPriceNR.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraChildPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraChildPriceNR.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraTeenPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        txtExtraTeenPriceNR.Enabled = (lstPeoplesExtras.Items.Count > 2)

        Me.reqExtraAdultPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        Me.reqExtraChildPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)
        Me.reqExtraTeenPrice.Enabled = (lstPeoplesExtras.Items.Count > 2)

        Me.reqExtraAdultPriceNR.Enabled = (lstPeoplesExtras.Items.Count > 2) AndAlso Not Me.isUsuarioMixto
        Me.reqExtraChildPriceNR.Enabled = (lstPeoplesExtras.Items.Count > 2) AndAlso Not Me.isUsuarioMixto
        Me.reqExtraTeenPriceNR.Enabled = (lstPeoplesExtras.Items.Count > 2) AndAlso Not Me.isUsuarioMixto

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
        txtAdultFareNR.Text = ""
        txtChildFareNR.Text = ""

        CtrlExtras()

        txtTeenFare.Text = ""
        txtTeenFareNR.Text = ""

        Me.varAdultFareNR.Value = ""
        Me.varChildFareNR.Value = ""
        Me.varExtraAdultPriceNR.Value = ""
        Me.varExtraChildPriceNR.Value = ""
        Me.varExtraTeenPriceNR.Value = ""
        Me.varTeenFareNR.Value = ""


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
        If Not Me.IsSupervisor Then
            'cmpvAdults.Enabled = False
            'cmpvChilds.Enabled = False
            'cmpvJuniors.Enabled = False
            compExtraAdultPrice.Enabled = False
            compExtraChildPrice.Enabled = False
            compExtraTeenPrice.Enabled = False
        End If

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
        cleardata()

        If Not datFares Is Nothing AndAlso datFares.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then

            Me.ddlrateplans.Enabled = True
            rowFare = datFares.Tables(FaresData.FARES_TABLE).Rows(0)
            Me.chkRules.Checked = rowFare(FaresData.RULESDEFAULT)

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
                Me.txtExtraChildPrice.Text = CDbl(Val(rowFare(FaresData.EXTRACHILDPRICE_FIELD)))
            End If

            Me.txtAdultFareNR.Text = CDbl(IIf(rowFare(FaresData.PRICENR_FIELD) Is DBNull.Value, 0, rowFare(FaresData.PRICENR_FIELD)))
            Me.txtChildFareNR.Text = CDbl(IIf(rowFare(FaresData.NINIORATENR) Is DBNull.Value, 0, rowFare(FaresData.NINIORATENR)))

            Me.txtTeenFare.Text = CDbl(IIf(rowFare(FaresData.RATEENPRICE_FIELD) Is DBNull.Value, 0, rowFare(FaresData.RATEENPRICE_FIELD)))
            Me.txtTeenFareNR.Text = CDbl(IIf(rowFare(FaresData.RATEENPRICENR_FIELD) Is DBNull.Value, 0, rowFare(FaresData.RATEENPRICENR_FIELD)))
            If txtExtraChildPriceNR.Enabled Then
                Me.txtExtraAdultPriceNR.Text = CDbl(IIf(rowFare(FaresData.EXTRAADULTPRICENR_FIELD) Is DBNull.Value, 0, rowFare(FaresData.EXTRAADULTPRICENR_FIELD)))
                Me.txtExtraChildPriceNR.Text = CDbl(IIf(rowFare(FaresData.EXTRACHILDPRICENR_FIELD) Is DBNull.Value, 0, rowFare(FaresData.EXTRACHILDPRICENR_FIELD)))
            End If
            If txtExtraTeenPriceNR.Enabled Then
                Me.txtExtraTeenPriceNR.Text = CDbl(IIf(rowFare(FaresData.EXTRATEENPRICENR_FIELD) Is DBNull.Value, 0, rowFare(FaresData.EXTRATEENPRICENR_FIELD)))
                Me.txtExtraTeenPrice.Text = CDbl(IIf(rowFare(FaresData.EXTRATEENPRICE_FIELD) Is DBNull.Value, 0, rowFare(FaresData.EXTRATEENPRICE_FIELD)))
            End If

            If Not rowFare.IsNull(FaresData.IDDICCDESCPROM_FIELD) Then
                Me.txtPromoDescription.CargaDatos(rowFare(FaresData.IDDICCDESCPROM_FIELD))
            Else
                Me.txtPromoDescription.CargaDatos(0)
            End If

            Me.varAdultFareNR.Value = CDbl(rowFare(FaresData.PRICE_FIELD))
            Me.varChildFareNR.Value = CDbl(Val(rowFare(FaresData.NINIORATE).ToString))
            Me.varExtraAdultPriceNR.Value = CDbl(Val(rowFare(FaresData.EXTRAADULTPRICE_FIELD)))
            Me.varExtraChildPriceNR.Value = CDbl(Val(rowFare(FaresData.EXTRACHILDPRICE_FIELD)))
            Me.varExtraTeenPriceNR.Value = CDbl(IIf(rowFare(FaresData.EXTRATEENPRICE_FIELD) Is DBNull.Value, 0, rowFare(FaresData.EXTRATEENPRICE_FIELD)))
            Me.varTeenFareNR.Value = CDbl(IIf(rowFare(FaresData.RATEENPRICE_FIELD) Is DBNull.Value, 0, rowFare(FaresData.RATEENPRICE_FIELD)))


            Me.Exceptions = rowFare(FaresData.EXCEPTION_FIELD)
            Me.txtMinDias.Text = ""
            Me.txtMaxDias.Text = ""
            Me.txtAdvBooking.Text = ""
            Me.txtMaxAdvBooking.Text = ""

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
            ActivateShowButtons()

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



        'If Me.txtExtraAdultPrice.Text.Trim.Length = 0 Then
        '    Me.txtExtraAdultPrice.Text = "0"
        'End If
        'If Me.txtExtraChildPrice.Text.Trim.Length = 0 Then
        '    Me.txtExtraChildPrice.Text = "0"
        'End If

        'If Me.txtExtraAdultPriceNR.Text.Trim.Length = 0 Then
        '    Me.txtExtraAdultPriceNR.Text = "0"
        'End If
        'If Me.txtExtraChildPriceNR.Text.Trim.Length = 0 Then
        '    Me.txtExtraChildPriceNR.Text = "0"
        'End If
        'If String.IsNullOrEmpty(txtExtraTeenPrice.Text) Then
        '    txtExtraTeenPrice.Text = "0"
        'End If
        'If String.IsNullOrEmpty(txtExtraTeenPriceNR.Text) Then
        '    txtExtraTeenPriceNR.Text = "0"
        'End If
        'If String.IsNullOrEmpty(txtTeenFareNR.Text) Then
        '    txtTeenFareNR.Text = "0"
        'End If
        'If String.IsNullOrEmpty(txtTeenFare.Text) Then
        '    txtTeenFare.Text = "0"
        'End If

        If Me.txtBookWindowDateFrom.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateFrom.Text = Date.Now.ToString("MM/dd/yyyy")
        End If
        If Me.txtBookWindowDateTo.Text.Trim.Length = 0 Then
            Me.txtBookWindowDateTo.Text = Date.Now.ToString("MM/dd/yyyy")
        End If

    End Sub

    Public Function AddFare(ByVal idRoom As Integer, ByRef idFare As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String, ByVal rp As String, ByVal f1last As String, ByVal f2last As String, ByVal sroom As String, ByVal publish As Boolean, ByVal isOcupacion As Boolean, ByVal ListFares As Queue(Of Double), ByRef scorreo As String) As Boolean
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim flag As Boolean = False

        If Page.IsValid Then
            SetDefaultValues()
            Dim ds As RatePlanData
            With New RatePlanFacade
                ds = .GetDataRatePlan(Me.ddlrateplans.SelectedValue, Me.m_iHotelId)
            End With
            If ds.Tables(ds.RATEPLAN_TABLE).Rows.Count > 0 Then
                With ds.Tables(ds.RATEPLAN_TABLE).Rows(0)
                    Try
                        Me.RatePlanRow = New RowRatePlanNR
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
                txtAdultFare.Text = ListFares.Dequeue
                txtChildFare.Text = ListFares.Dequeue
                txtTeenFare.Text = ListFares.Dequeue
                txtAdultFareNR.Text = ListFares.Dequeue
                txtChildFareNR.Text = ListFares.Dequeue
                txtTeenFareNR.Text = ListFares.Dequeue
            End If

            If Me.m_iFareId = 0 Then
                If SaveNewFare(idRoom, idFare, f1, f2, String.Format("{0} {1}", sroom, rp), sData) Then
                    scorreo = (New Util.Utility).GeneraCorreoXslt(sDataPrev, sData)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Crear, "Se creó la tarifa de la habitación " & ch.Substring(0, ch.IndexOf("--")) & " de la fecha " & f1 & " a la fecha " & f2 & " con el rateplan " & Me.RatePlanRow.RATECODE, "", sDataPrev, sData)
                    flag = True
                End If
            Else
                If UpdateFare(idRoom, f1, f2, sDataPrev, String.Format("{0} {1}", sroom, rp), sData, scorreo, publish) Then
                    scorreo = (New Util.Utility).GeneraCorreoXslt(sDataPrev, sData)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Modificar, "Se modificó la tarifa de la habitación " & ch & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.RatePlanRow.RATECODE, "", sDataPrev, sData)
                    flag = True
                End If
                m_iFareId = 0
            End If
            If Me.txtPromoDescription.HasChanges Then CType(Me.Page, PaginaBase).NotifyContentModification("Tarifa neta de la habitación " & ch & ", y plan tarifario " & Me.RatePlanRow.RATECODE, "Tarifas Netas")
        End If
        Return flag
    End Function

    Public Sub setPorcMinMax()
        Me.m_TextBoxPorcMax = TextBoxPorcMax.ClientID
        Me.m_TextBoxPorcMin = TextBoxPorcMin.ClientID
    End Sub

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
    Public Function Segment(ByVal Value As String) As String
        Try
            If Value <> "0" Then
                Me.ddlrateplans.SelectedValue = Value
            End If
        Catch ex As Exception
        End Try
    End Function

    'Private Sub ddlrateplans_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlrateplans.SelectedIndexChanged
    '    Dim idContrato As Integer = 0
    '    Dim contrato As ContractNetRateData
    '    With (New ContractNetRateFacade)
    '        contrato = .getContractsByIdRatePlanHotel(m_iHotelId, ddlrateplans.SelectedValue)
    '        If Not contrato Is Nothing AndAlso contrato.Tables(0).Rows.Count > 0 Then
    '            TextBoxPorcMax.Text = contrato.Tables(0).Rows(0)(contrato.FIELD_PORCENTAJEMAXIMO)
    '            TextBoxPorcMin.Text = contrato.Tables(0).Rows(0)(contrato.FIELD_PORCENTAJEMINIMO)
    '        End If
    '    End With
    'End Sub


    Public Sub LoadRooms(ByVal idRoom As Integer)

        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idRoom)
        End With
        If Not room Is Nothing AndAlso room.Tables(room.TBL_ROOM_HOTEL).Rows.Count > 0 Then
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

            Dim initialValue As String = String.Empty
            Dim extrasVisibles As Boolean = False
            If iExtPeople > 0 Then
                extrasVisibles = True
                initialValue = ""
            End If

            txtExtraAdultPrice.Enabled = extrasVisibles
            txtExtraChildPrice.Enabled = extrasVisibles
            txtExtraTeenPrice.Enabled = extrasVisibles
            txtExtraAdultPriceNR.Enabled = extrasVisibles
            txtExtraChildPriceNR.Enabled = extrasVisibles
            txtExtraTeenPriceNR.Enabled = extrasVisibles

            reqExtraAdultPrice.Enabled = extrasVisibles
            reqExtraChildPrice.Enabled = extrasVisibles
            reqExtraTeenPrice.Enabled = extrasVisibles

            reqExtraAdultPriceNR.Enabled = extrasVisibles AndAlso Not Me.isUsuarioMixto
            reqExtraChildPriceNR.Enabled = extrasVisibles AndAlso Not Me.isUsuarioMixto
            reqExtraTeenPriceNR.Enabled = extrasVisibles AndAlso Not Me.isUsuarioMixto

            txtExtraAdultPrice.Text = initialValue
            txtExtraChildPrice.Text = initialValue
            txtExtraTeenPrice.Text = initialValue
            txtExtraAdultPriceNR.Text = initialValue
            txtExtraChildPriceNR.Text = initialValue
            txtExtraTeenPriceNR.Text = initialValue

        End If
    End Sub

    Public Function IsValidData() As Boolean

        IsValidData = True
        Dim isFisrt As Boolean = True
        Dim isSupervisor As Boolean = CType(Me.Page, PaginaBase).IsSupervisor()

        Dim netRate As Double = 0
        Dim rate As Double = 0
        Dim minPercent As Integer = 0
        Dim maxPercent As Integer = 0

        Integer.TryParse(Me.TextBoxPorcMin.Text, minPercent)
        Integer.TryParse(Me.TextBoxPorcMax.Text, maxPercent)

        For Each pair As List(Of Control) In Me.GetNetRatesPair()

            Double.TryParse(CType(pair(0), TextBox).Text, netRate)
            Double.TryParse(CType(pair(1), TextBox).Text, rate)

            If isFisrt AndAlso Me.ddlShowRates.SelectedIndex = 0 Then
                'es el unico que no puede ser 0
                isFisrt = False
                IsValidData = ((netRate > 0 OrElse Me.isUsuarioMixto) AndAlso (Not isSupervisor OrElse rate > 0))
            End If

            If IsValidData AndAlso Not isSupervisor AndAlso rate = 0 Then
                Double.TryParse(CType(pair(2), HtmlInputHidden).Value, rate)
                CType(pair(1), TextBox).Text = rate.ToString()
                'If rate = 0 OrElse netRate * (1 + (minPercent / 100)) > rate OrElse netRate = 0 Then
                If rate = 0 OrElse netRate / (1 - (minPercent / 100)) > rate OrElse netRate / (1 - (maxPercent / 100)) < rate OrElse netRate = 0 Then
                    CType(pair(1), TextBox).Text = (netRate / (1 - (maxPercent / 100))).ToString()
                    Double.TryParse(CType(pair(1), TextBox).Text, rate)
                End If
            ElseIf IsValidData AndAlso Me.isUsuarioMixto AndAlso rate > 0 Then
                CType(pair(0), TextBox).Text = (rate * (1 - (maxPercent / 100))).ToString()
            End If

            'IsValidData = (IsValidData AndAlso (isSupervisor OrElse ((netRate * (1 + (minPercent / 100))) <= rate)))
            'IsValidData = (IsValidData AndAlso (isSupervisor OrElse ((netRate * (1 + (minPercent / 100))) < rate)) OrElse netRate * (1 + (maxPercent / 100)) > rate)
            If Not IsValidData Then Exit Function

        Next

    End Function

    Private Function GetNetRatesPair() As List(Of List(Of Control))

        Dim result As New List(Of List(Of Control))

        Dim controls As New List(Of Control)
        controls.Add(Me.txtAdultFareNR)
        controls.Add(Me.txtAdultFare)
        controls.Add(Me.varAdultFareNR)
        result.Add(controls)

        controls = New List(Of Control)
        controls.Add(Me.txtChildFareNR)
        controls.Add(Me.txtChildFare)
        controls.Add(Me.varChildFareNR)
        result.Add(controls)

        controls = New List(Of Control)
        controls.Add(Me.txtTeenFareNR)
        controls.Add(Me.txtTeenFare)
        controls.Add(Me.varTeenFareNR)
        result.Add(controls)

        controls = New List(Of Control)
        controls.Add(Me.txtExtraAdultPriceNR)
        controls.Add(Me.txtExtraAdultPrice)
        controls.Add(Me.varExtraAdultPriceNR)
        result.Add(controls)

        controls = New List(Of Control)
        controls.Add(Me.txtExtraChildPriceNR)
        controls.Add(Me.txtExtraChildPrice)
        controls.Add(Me.varExtraChildPriceNR)
        result.Add(controls)

        controls = New List(Of Control)
        controls.Add(Me.txtExtraTeenPriceNR)
        controls.Add(Me.txtExtraTeenPrice)
        controls.Add(Me.varExtraTeenPriceNR)
        result.Add(controls)

        Return result

    End Function


    Private Sub loadLst(ByRef ddl As DropDownList, ByVal addEmpty As Boolean, ByVal valueStart As Integer, ByVal valueEnd As Integer)
        ddl.Items.Clear()
        Dim n As Integer = 0

        If addEmpty Then ddl.Items.Add(New ListItem(" ", "-1"))

        For n = valueStart To valueEnd
            ddl.Items.Add(New ListItem(n, n))
        Next

        ddl.SelectedIndex = 0
    End Sub


    Protected ReadOnly Property IsSupervisor() As Boolean
        Get
            Return CType(Me.Page, PaginaBase).IsSupervisor
        End Get
    End Property

    Protected ReadOnly Property isUsuarioMixto() As Boolean
        Get
            Return CType(Me.Page, PaginaBase).IsUsuarioMixto
        End Get
    End Property

    Protected ReadOnly Property IsUserAsociation() As Boolean
        Get
            Return CType(Me.Page, PaginaBase).IsUsuarioHotelAssociation
        End Get
    End Property

End Class


<Serializable()> Public Class RowRatePlanNR
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
