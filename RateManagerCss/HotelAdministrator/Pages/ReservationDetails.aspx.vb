Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports System.Xml
Imports System.Text
Imports System.Web
Imports WSHotelCommon
Imports WSHotelFacade
Imports System.Data
Imports XCrypt
Imports PortalLibraries
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Partial Class ReservationDetails
    Inherits PaginaBase
    Private i As Integer

#Region " C�digo generado por el Dise�ador de Web Forms "

    'El Dise�ador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblExtra As System.Web.UI.WebControls.Label
    Protected WithEvents lblImpuesto As System.Web.UI.WebControls.Label
    Protected WithEvents lblEExtra As System.Web.UI.WebControls.Label
    Protected WithEvents lbl3 As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents CtrLinckPackage1 As ctrLinckPackage
    Protected WithEvents tdCostoNR As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblpolicies As System.Web.UI.WebControls.Label
    Protected WithEvents lblTest As System.Web.UI.WebControls.Label

    'NOTA: el Dise�ador de Web Forms necesita la siguiente declaraci�n del marcador de posici�n.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Dise�ador de Web Forms requiere esta llamada de m�todo
        'No la modifique con el editor de c�digo.
        InitializeComponent()
    End Sub

#End Region
    Public Const SPGETRESERVATIONS As String = "spGetDataReservations"
    Public Const PRM_ID_RVA As String = "@idReservacion"
    Private isNR As Boolean = False
    Private canSeeCArds As Boolean = False
    Dim CancelNumber As String

    Public Enum Columns As Integer
        Cuartos
        Cliente
        Adultos
        tipoHabitacion
        codigotarifa
        Preferencias
        BDPreferencias
    End Enum

    Enum tTarjeta
        ccDinerClub = 0
        ccAmericanExpress = 1
        ccJCB = 2
        ccCarteBlanche = 3
        ccVisa = 4
        ccMasterCard = 5
        ccAustralianBankCard = 6
        ccDiscover = 7
    End Enum

    Property SourceReserv() As String
        Get
            Return ViewState("_SourceReserv")
        End Get
        Set(ByVal value As String)
            ViewState("_SourceReserv") = value
        End Set
    End Property

    Property CurrencyConfirm() As String
        Get
            Return ViewState("_CurrencyConfirm")
        End Get
        Set(ByVal value As String)
            ViewState("_CurrencyConfirm") = value
        End Set
    End Property

    Property TotalConfirm() As Decimal
        Get
            Return ViewState("_TotalConfirm")
        End Get
        Set(ByVal value As Decimal)
            ViewState("_TotalConfirm") = value
        End Set
    End Property

    Property NoReserv() As String
        Get
            Return ViewState("_NoReserv")
        End Get
        Set(ByVal value As String)
            ViewState("_NoReserv") = value
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

    Private Function decodeCards(ByVal card As tTarjeta) As String
        Select Case card
            Case tTarjeta.ccVisa
                Return "VISA"
            Case tTarjeta.ccMasterCard
                Return "MASTER CARD"
            Case tTarjeta.ccAmericanExpress
                Return "AMERICAN EXPRESS"
            Case tTarjeta.ccAustralianBankCard
                Return "AUSTRALIAN BANK CARD"
            Case tTarjeta.ccCarteBlanche
                Return "CARTE BLANCHE"
            Case tTarjeta.ccDinerClub
                Return "DINERS CLUB"
            Case tTarjeta.ccDiscover
                Return "DISCOVER"
            Case tTarjeta.ccJCB
                Return "JCB"
        End Select
        Return ""
    End Function


    Dim displayCommision As Boolean = False
    Private Sub hideByHotel()
        If MyBase.IsHotel Or MyBase.IsUsuarioHotel Then

        End If
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Trace.Write("1")
        If Not MyBase.IsSupervisor AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
        End If

        Trace.Write("2")
        'Introducir aqu� el c�digo de usuario para inicializar la p�gina.
        If Me.Request.QueryString("returnUrl") IsNot Nothing AndAlso Me.Request.QueryString("returnUrl").ToString().Trim.Length > 0 Then
            Me.hplListRes.NavigateUrl = Me.ResolveUrl(HttpUtility.UrlDecode(Me.Request.QueryString("returnUrl")))
        Else
            Me.hplListRes.NavigateUrl = UrlPage(PaginaBase.pages.ReservationList)
        End If
        Trace.Write("3")


        With Me.dgReservas
            .Columns(Columns.Cuartos).HeaderText = ColumunsName(Columns.Cuartos)
            .Columns(Columns.Cliente).HeaderText = ColumunsName(Columns.Cliente)
            .Columns(Columns.Adultos).HeaderText = ColumunsName(Columns.Adultos)
            .Columns(Columns.tipoHabitacion).HeaderText = ColumunsName(Columns.tipoHabitacion)
            .Columns(Columns.Preferencias).HeaderText = ColumunsName(Columns.Preferencias)
            .Columns(Columns.codigotarifa).HeaderText = ColumunsName(Columns.codigotarifa)
        End With

        If Not Request.QueryString("hc") Is Nothing AndAlso Request.QueryString("hc") = "y" Then
            btnCancel.Style.Add("display", "none")
        End If
        If Me.IsSupervisor Then
            displayCommision = True
        End If
        If Not IsPostBack Then
            hplModify.Visible = False
        End If
        Try
            CtrLinckPackage1.idReservacion = Request.QueryString("qs")
        Catch ex As Exception
            Trace.Write("ex 4" & ex.Message)
        End Try
        btnReactive.OnClientClick = String.Format("return FireReactive('{0}');", PortalCulture.GetString("01448"))

        If Not IsSupervisor Then
            lblMail.Visible = False
            lblEmail.Visible = False
            lblEPhoneH.Visible = False
            lblPhoneH.Visible = False
            lblEPhoneW.Visible = False
            lblPhoneW.Visible = False
            lblEAddress.Visible = False
            lblAddress.Visible = False
            lblECode.Visible = False
            lblZipCode.Visible = False
            lblCiudad.Visible = False
            lblCustCity.Visible = False
            lblEstado.Visible = False
            lblCustCounty.Visible = False
            lblPais.Visible = False
            lblCustCountry.Visible = False
        End If

    End Sub

    Public Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.Cuartos
                Return PortalCulture.GetString("M000335")
            Case Columns.Cliente
                Return PortalCulture.GetString("M000585")
            Case Columns.Adultos
                Return PortalCulture.GetString("M000337")
            Case Columns.tipoHabitacion
                Return PortalCulture.GetString("M000338")
            Case Columns.Preferencias
                Return PortalCulture.GetString("M000339")
            Case Columns.codigotarifa
                Return PortalCulture.GetString("00016")
        End Select
    End Function

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim idRes As String = Request.QueryString("qs")
        Me.lblWizcom.Visible = False
        Trace.Write("5")
        getData(idRes)
        Trace.Write("6")
        LoadResources()
        lblEComision.Visible = False
        lblComision.Visible = False
        lblEComisionUV.Visible = False
        lblComisionUV.Visible = False

        Me.lblFees.Visible = False
        Me.lblEFees.Visible = False

        If displayCommision Then
            'lblEComision.Visible = True
            'lblComision.Visible = True
            'Me.lblFees.Visible = True
            'Me.lblEFees.Visible = True
        End If
        MyBase.Habilitaboton(permisos.ReservationsList, Me.hplListRes, "R")
        MyBase.Habilitaboton(permisos.ReservationsList, Me.hplModify, "R")
        Me.ADetails.Attributes("OnClick") = "javascript:ShowDetails('divDetails', '');"
        Me.imgclose.Attributes("OnClick") = "javascript:ShowDetails('divDetails', 'none');"

        Me.detailObserv.Attributes("OnClick") = "javascript:ShowDetails('divObservaciones', '');"
        Me.imgCloseObserv.Attributes("OnClick") = "javascript:ShowDetails('divObservaciones', 'none');"

        Me.aNRPolicies.Visible = False '.Attributes("OnClick") = "javascript:ShowDetails('dvNRPolicies', '');"
        Me.imgNRclose.Attributes("OnClick") = "javascript:ShowDetails('dvNRPolicies', 'none');"

        Me.aPolices.Attributes("OnClick") = "javascript:ShowDetails('divPolicies', '');"
        Me.imgPoliceClose.Attributes("OnClick") = "javascript:ShowDetails('divPolicies', 'none');"

        Me.ancItems.Attributes("OnClick") = "javascript:ShowDetails('divItems', '');"
        Me.imgCloseItems.Attributes("OnClick") = "javascript:ShowDetails('divItems', 'none');"

    End Sub

    Private Function GetStatus(ByVal idstatus As Integer, ByVal source As String, ByVal IdCorporativoPortal As Integer, ByVal isNetRateUv As Boolean, ByVal idAsociacion As Integer) As String
        lblNoCanc.Visible = True
        Me.lblNoCancelacion.Visible = True
        Me.btnCancel.Visible = False
        Me.btnReactive.Visible = False
        ADetails.Visible = False
        Select Case idstatus
            Case 1
                If Not cInfoActual.IdPais = "CU" Then
                    lblNoCanc.Visible = False
                    Me.lblNoCancelacion.Visible = False
                    If (source = "UNI") Then
                        If (Not isNetRateUv) Or Me.IsSupervisor Then
                            Me.btnCancel.Visible = True
                        End If
                    ElseIf (source = "HTL") Or Me.IsSupervisor Then 'Or ((Me.IsUsuarioNivelHotel Or Me.isUserChain) AndAlso source = "HTL") Then
                        Me.btnCancel.Visible = True
                    ElseIf Me.isUserChain AndAlso source = "POR" AndAlso IdCorporativoPortal <> -1 AndAlso IdCorporativoUserChain = IdCorporativoPortal Then
                        Me.btnCancel.Visible = True
                    End If
                    If source = "POR" And isNetRateUv And Not Me.IsSupervisor Then
                        Me.btnCancel.Visible = False
                    End If
                    If source = "POR" And Me.IsUsuarioHotelAssociation Then
                        If idAsociacion = Me.IdAsociation Then
                            Me.btnCancel.Visible = True
                        Else
                            Me.btnCancel.Visible = False
                        End If
                    End If

                    Dim allowUserChain As Boolean
                    If Boolean.TryParse(AppSettings("allowsUserchainToModifyReservation"), allowUserChain) Then
                        btnCancel.Visible = True
                    End If
                Else
                    btnCancel.Visible = True
                End If
                Return PortalCulture.GetString("M000331")
            Case 2
                Return PortalCulture.GetString("M000332")
            Case 3

                If (source = "UNI") Then
                    If (Not isNetRateUv) Or Me.IsSupervisor Then
                        Me.btnReactive.Visible = True
                    End If
                ElseIf (source = "HTL") Or Me.IsSupervisor Then
                    Me.btnReactive.Visible = True
                ElseIf Me.isUserChain AndAlso source = "POR" AndAlso IdCorporativoPortal <> -1 AndAlso IdCorporativoUserChain = IdCorporativoPortal Then
                    Me.btnReactive.Visible = True
                End If
                If source = "POR" And isNetRateUv And Not Me.IsSupervisor Then
                    Me.btnReactive.Visible = False
                End If
                If source = "POR" And Me.IsUsuarioHotelAssociation Then
                    If idAsociacion = Me.IdAsociation Then
                        Me.btnReactive.Visible = True
                    Else
                        Me.btnReactive.Visible = False
                    End If
                End If

                ADetails.Visible = True
                Return PortalCulture.GetString("M000333")
            Case 4
                lblNoCanc.Visible = False
                Me.lblNoCancelacion.Visible = False
                If Me.IsSupervisor Or source = "HTL" Then '((Me.IsUsuarioNivelHotel Or Me.isUserChain) AndAlso source = "HTL") Then
                    Me.btnCancel.Visible = True
                    Me.btnReactive.Visible = True
                ElseIf Me.isUserChain AndAlso source = "POR" AndAlso IdCorporativoPortal <> -1 AndAlso IdCorporativoUserChain = IdCorporativoPortal Then
                    If Not isNetRateUv Then
                        Me.btnCancel.Visible = True
                        Me.btnReactive.Visible = True
                    End If
                End If
                If source = "POR" And isNetRateUv And Not Me.IsSupervisor Then
                    Me.btnCancel.Visible = False
                    Me.btnReactive.Visible = False
                End If
                If source = "POR" And Me.IsUsuarioHotelAssociation Then
                    If idAsociacion = Me.IdAsociation Then
                        Me.btnCancel.Visible = True
                        Me.btnReactive.Visible = True
                    Else
                        Me.btnCancel.Visible = False
                        Me.btnReactive.Visible = False
                    End If
                End If

                Return PortalCulture.GetString("00719")
            Case 5 ' No show
                Return PortalCulture.GetString("M0BT0000052")
            Case 6
                Return PortalCulture.GetString("01393")
        End Select
    End Function

    Private Sub LoadResources()
        hplModifyRes.Text = PortalCulture.GetString("A00153")
        Me.Modify.Value = PortalCulture.GetString("M000228")
        HyperLink3.Text = PortalCulture.GetString("00095")
        Me.lblNombreCL.Text = PortalCulture.GetString("00073")
        Me.lblApelldoCL.Text = PortalCulture.GetString("00355")
        Me.lblCheckIn.Text = PortalCulture.GetString("M000078")
        Me.lblCheckOut.Text = PortalCulture.GetString("M000079")
        RFTotal.Text = PortalCulture.GetString("M000179")
        RFTotalNR.Text = PortalCulture.GetString("M000179")
        RFApellido.Text = PortalCulture.GetString("M000179")
        RFNombre.Text = PortalCulture.GetString("M000179")
        valTotalMidfy.Text = PortalCulture.GetString("M000199")
        valTotalMidfyNR.Text = PortalCulture.GetString("M000199")
        Me.lbErrorTotals.Text = PortalCulture.GetString("M0UT02713")
        Me.lblErrorCheckin.Text = PortalCulture.GetString("M0UT02712")
        Me.lblConfirmModify.Text = PortalCulture.GetString("M0UT02711")
        Me.lberrorTotalsZero.Text = PortalCulture.GetString("M0UT02714")

        lblEResCurrency.Text = PortalCulture.GetString("01447", True)
        lblEMoneyChange.Text = PortalCulture.GetString("M0BT0000030", True)
        hplModify.Text = PortalCulture.GetString("M000228")
        Me.hplListRes.Text = PortalCulture.GetString("00473")
        Me.lblMotivo.Text = PortalCulture.GetString("M000605", True)
        btnCancel.Value = PortalCulture.GetString("M000143")
        lblConfirmCancel.Text = PortalCulture.GetString("M000539")
        lblSure.Text = PortalCulture.GetString("M000540")
        lblNoCanc.Text = PortalCulture.GetString("M000541", True)
        hplCancelRes.Text = PortalCulture.GetString("M000538")
        Me.lblEIn.Text = PortalCulture.GetString("M000078", True)
        Me.lblEOut.Text = PortalCulture.GetString("M000079", True)
        lblEResDate.Text = PortalCulture.GetString("01528", True)
        Me.lblRaG.Text = PortalCulture.GetString("M000318")

        'Me.lblEAdultos.Text = PortalCulture.GetString("M000319")
        'Me.lblEchildren.Text = PortalCulture.GetString("M000320")

        'Me.lblEAdultosExt.Text = PortalCulture.GetString("00428")
        'Me.lblEchildrenExt.Text = PortalCulture.GetString("00429")
        'Me.lbelChildAges.Text = PortalCulture.GetString("01367", True)

        Me.lblECuartos.Text = PortalCulture.GetString("M000321")
        Me.lblENoches.Text = PortalCulture.GetString("M000322")
        Me.lblEStatus.Text = PortalCulture.GetString("M000323") & ":  "
        Me.lblESource.Text = PortalCulture.GetString("M000324") & "  "
        Me.lblEIdBooking.Text = PortalCulture.GetString("01418") & ":  "
        Me.lblEID.Text = PortalCulture.GetString("M000325") & " "
        Me.lblEContacto.Text = PortalCulture.GetString("M000326") & " "
        Me.lblEmail.Text = PortalCulture.GetString("M000327") & " "

        Me.lblEPhoneH.Text = PortalCulture.GetString("M000328") & " "
        Me.lblEPhoneW.Text = PortalCulture.GetString("M000329") & " "
        Me.lblECode.Text = PortalCulture.GetString("M0BT0000164") & " :"
        Me.lblCiudad.Text = PortalCulture.GetString("00254") & " :"
        Me.lblEstado.Text = PortalCulture.GetString("00252") & " :"
        Me.lblPais.Text = PortalCulture.GetString("00251") & " :"
        Me.lblCustomerData.Text = PortalCulture.GetString("00420") & "."
        Me.lblCostos.Text = PortalCulture.GetString("M000330")
        Me.lbl1.Text = PortalCulture.GetString("M000340")
        Me.lblECosto.Text = PortalCulture.GetString("M000341")
        Me.lblEImpuesto.Text = PortalCulture.GetString("M000343")
        Me.lblEComision.Text = PortalCulture.GetString("M000344")
        Me.lblETotal.Text = PortalCulture.GetString("M000345")
        Me.lblConfirm.Text = PortalCulture.GetString("M000348")
        Me.lblEFees.Text = PortalCulture.GetString("01348")
        Me.lblEFeesUV.Text = PortalCulture.GetString("01348")

        If Me.lblTotalUV.Text.Trim().Length > 0 Then
            tdTextUnivisit.InnerText = PortalCulture.GetString("01125").ToUpper
            If Me.IsSupervisor Then
                Me.lblETotalH.Text = PortalCulture.GetString("01346")
            ElseIf Me.IsUsuarioHotelAssociation Then
                Me.lblETotalH.Text = PortalCulture.GetString("01400")
            Else
                Me.lblETotalH.Text = PortalCulture.GetString("01346")
            End If
        Else
            Me.lblETotalH.Text = PortalCulture.GetString("M000349")
        End If

        Me.lblCCTitle.Text = PortalCulture.GetString("M000503")
        Me.lblCCNa.Text = PortalCulture.GetString("M000144") & " :"
        Me.lblCCNu.Text = PortalCulture.GetString("M000504")
        Me.lblCCcv.Text = PortalCulture.GetString("M000505")
        Me.lblCCExp.Text = PortalCulture.GetString("M000506")
        Me.lblCCTipo.Text = PortalCulture.GetString("M000526") & " :"
        Me.lblWizcom.Text = PortalCulture.GetString("M000584")
        Me.lblEBookingSource.Text = PortalCulture.GetString("M000595", True)
        Me.lblERF.Text = PortalCulture.GetString("M000596", True)
        Me.lblESystemCode.Text = PortalCulture.GetString("M000594", True)
        Me.lblTitleRollAway.Text = PortalCulture.GetString("M000652") & "."
        Me.lblECantidad.Text = PortalCulture.GetString("M000125", True)
        Me.lblPrice.Text = PortalCulture.GetString("M000648", True)
        Me.lblRollAwayAdult.Text = PortalCulture.GetString("M000645")
        Me.lblRollAwayChild.Text = PortalCulture.GetString("M000646")
        Me.lblCrib.Text = PortalCulture.GetString("M000647")
        Me.lblReservationData.Text = PortalCulture.GetString("00419") & "."
        Me.lblEMotivocancelacion.Text = PortalCulture.GetString("00736")
        'Me.lblETravelerName.InnerHtml = PortalCulture.GetString("00616", True)
        'Me.lblETravelerEmail.InnerHtml = PortalCulture.GetString("00617", True)
        Me.ADetails.InnerHtml = PortalCulture.GetString("00510")
        'NETRATE
        Me.lbl1UV.Text = PortalCulture.GetString("M000340")
        Me.lblETipoReservacion.Text = PortalCulture.GetString("M000162", True)
        Me.lblECostoUV.Text = PortalCulture.GetString("M000341")
        Me.lblEImpuestoUV.Text = PortalCulture.GetString("M000343")
        Me.lblETotalUV.Text = PortalCulture.GetString("M000345")
        Me.lblETotalHUV.Text = PortalCulture.GetString("M000349")
        Me.lblEComisionUV.Text = PortalCulture.GetString("M000344")
        Me.aPolices.InnerHtml = PortalCulture.GetString("00353")
        Me.aNRPolicies.InnerHtml = PortalCulture.GetString("01552")
        Me.btnReactive.Text = PortalCulture.GetString("01449")
        btnStatusPMS.Text = PortalCulture.GetString("01519")
        'Me.lblpolicies.Text = PortalCulture.GetString("00353")
        Me.lblCancelPolTitle.Text = PortalCulture.GetString("01538")
        Me.lblCardPolTitle.Text = PortalCulture.GetString("01539")
        Me.lblGuarPolTitle.Text = PortalCulture.GetString("01540")
        Me.lblCompanySegmentTitle.Text = PortalCulture.GetString("01553")
        Me.lblSegmentTitle.Text = PortalCulture.GetString("01554")

        If lblCCExp.Visible Then
            lnkShowCC.Text = PortalCulture.GetString("01171")
        Else
            lnkShowCC.Text = PortalCulture.GetString("00889")
        End If
        btnResConfirm.Value = PortalCulture.GetString("01407")
        hplConfirmCancel.Text = PortalCulture.GetString("A00143")
        hplSaveConfirm.Text = PortalCulture.GetString("A00153")
        'If Not cInfoActual.Hotel = 207 Then
        '    txtMultiCancel.Visible = False
        '    btnMultiCancel.Visible = False
        'End If
    End Sub



    Private Sub showSwitchDataEtiquetas(ByVal valor As Boolean)
        Me.lblWizcom.Visible = valor
        Me.lblEBookingSource.Visible = valor
        Me.lblBookingSource.Visible = valor
        Me.lblSystemCode.Visible = valor
        Me.lblESystemCode.Visible = valor
        Me.lblRecordLocator.Visible = valor
        Me.lblERecordLocator.Visible = valor
        Me.lblRF.Visible = valor
        Me.lblERF.Visible = valor
        Me.lblESource.Visible = Not valor
        Me.lblSource.Visible = Not valor
        lblreclocnumber.Visible = Not valor
        Me.lblIdBooking.Visible = Not valor
        Me.lblEIdBooking.Visible = Not valor

        Me.lblRecLoc.Visible = Not valor

    End Sub

    Private Function TarifasHtml(ByVal xml As resHotelDisplay, ByVal money As String) As String
        Dim dt As DateTime
        Dim dtr As DateTime
        Dim lstTarifas As New ArrayList
        Dim sHtmlRate As String
        Dim sDias As String
        Dim sComa As String
        Dim dPrecio As Double
        Dim dImporte As Double
        Dim sItem As String
        Dim sHab As String
        Dim drr As resHotelDisplay.RoomRow
        Dim icount As Integer
        sDias = ""
        sComa = ""
        sHtmlRate = ""
        sHab = ""
        dt = DateTime.Parse("1901/01/01")
        dtr = DateTime.Parse("1901/01/01")
        dPrecio = -1
        icount = 1

        For Each drRooms As resHotelDisplay.RoomRow In xml.Room
            Dim dr As resHotelDisplay.RatesRow() = drRooms.GetRatesRows()
            Dim drateArr As resHotelDisplay.RateRow()
            sHab &= +icount.ToString
            sHtmlRate &= String.Format("<tr><td colspan='2'>{0} {1}</td></tr> ", PortalCulture.GetString("00170"), sHab)
            For i As Int32 = 0 To dr.Length - 1
                drateArr = dr(i).GetRateRows()
                dPrecio = -1
                lstTarifas.Clear()
                For Each drRate As resHotelDisplay.RateRow In drateArr
                    drr = drRate.RatesRow.RoomRow

                    dImporte = If(Not drRate.IsAdultRateNull, drRate.AdultRate, 0) + If(Not drRate.IsChildRateNull, drRate.ChildRate, 0)
                    dImporte += If(Not drRate.IsExtraRateAdultNull, drRate.ExtraRateAdult, 0) + If(Not drRate.IsExtraRateChildNull, drRate.ExtraRateChild, 0)
                    'dImporte = drRate.AdultRate
                    If Not Me.IsSupervisor AndAlso Not IsUsuarioHotelAssociation Then
                        If Not drRate.IsAdultRateNRNull AndAlso drRate.AdultRateNR <> 0 Then
                            'dImporte = drRate.AdultRateNR
                            dImporte = If(Not drRate.IsAdultRateNRNull, drRate.AdultRateNR, 0) + If(Not drRate.IsChildRateNRNull, drRate.ChildRateNR, 0)
                            dImporte += If(Not drRate.IsExtraRateAdultNRNull, drRate.ExtraRateAdultNR, 0) + If(Not drRate.IsExtraRateChildNRNull, drRate.ExtraRateChildNR, 0)
                        End If
                    End If
                    sItem = String.Format("{0}/{1}", dImporte, drRate._Date)
                    If Not lstTarifas.Contains(sItem) Then
                        lstTarifas.Add(sItem)
                        dtr = dt
                        If dt <> DateTime.Parse(drRate._Date) Then
                            sDias = DateTime.Parse(drRate._Date).ToString("dd-MMM-yyy")
                            dt = DateTime.Parse(drRate._Date)
                        Else
                            sDias = DateTime.Parse(drRate._Date).ToString("dd-MMM-yyy")
                        End If
                        If (dPrecio <> dImporte) Then
                            sHtmlRate &= String.Format("<tr><td>{0}</td> <td style='padding-left:6px;'>{1} {2}</td></tr> ",
                                                        sDias, FCurrency(dImporte, 2), IIf(dImporte > 0, money, ""))
                            dPrecio = dImporte
                            sDias = ""
                            sHab = ""
                            sComa = ""
                        End If
                    End If
                Next
            Next
            icount += 1
        Next
        sHtmlRate = String.Format("<table style=''>{0}</table>", sHtmlRate)
        Return sHtmlRate

        For Each drRate As resHotelDisplay.RateRow In xml.Rate
            drr = drRate.RatesRow.RoomRow

            dImporte = If(Not drRate.IsAdultRateNull, drRate.AdultRate, 0) + If(Not drRate.IsChildRateNull, drRate.ChildRate, 0)
            dImporte += If(Not drRate.IsExtraRateAdultNull, drRate.ExtraRateAdult, 0) + If(Not drRate.IsExtraRateChildNull, drRate.ExtraRateChild, 0)
            'dImporte = drRate.AdultRate
            If Not Me.IsSupervisor AndAlso Not IsUsuarioHotelAssociation Then
                If Not drRate.IsAdultRateNRNull AndAlso drRate.AdultRateNR <> 0 Then
                    'dImporte = drRate.AdultRateNR
                    dImporte = If(Not drRate.IsAdultRateNRNull, drRate.AdultRateNR, 0) + If(Not drRate.IsChildRateNRNull, drRate.ChildRateNR, 0)
                    dImporte += If(Not drRate.IsExtraRateAdultNRNull, drRate.ExtraRateAdult, 0) + If(Not drRate.IsExtraRateChildNRNull, drRate.ExtraRateChildNR, 0)
                End If
            End If
            sItem = String.Format("{0}/{1}", dImporte, drRate._Date)
            If Not lstTarifas.Contains(sItem) Then
                lstTarifas.Add(sItem)
                dtr = dt
                If dt <> DateTime.Parse(drRate._Date) Then
                    sDias = DateTime.Parse(drRate._Date).ToString("dd-MMM-yyy")
                    dt = DateTime.Parse(drRate._Date)
                Else
                    sDias = DateTime.Parse(drRate._Date).ToString("dd-MMM-yyy")
                    'sDias = sComa & DateTime.Parse(drRate._Date).Day                
                End If

                'dImporte = If(Not drRate.IsAdultRateNull, drRate.AdultRate, 0) + If(Not drRate.IsChildRateNull, drRate.ChildRate, 0)
                'dImporte += If(Not drRate.IsExtraRateAdultNull, drRate.ExtraRateAdult, 0) + If(Not drRate.IsExtraRateChildNull, drRate.ExtraRateChild, 0)
                ''dImporte = drRate.AdultRate
                'If Not Me.IsSupervisor AndAlso Not IsUsuarioHotelAssociation Then
                '    If Not drRate.IsAdultRateNRNull AndAlso drRate.AdultRateNR <> 0 Then
                '        'dImporte = drRate.AdultRateNR
                '        dImporte = If(Not drRate.IsAdultRateNRNull, drRate.AdultRateNR, 0) + If(Not drRate.IsChildRateNRNull, drRate.ChildRateNR, 0)
                '        dImporte += If(Not drRate.IsExtraRateAdultNRNull, drRate.ExtraRateAdult, 0) + If(Not drRate.IsExtraRateChildNRNull, drRate.ExtraRateChildNR, 0)
                '    End If
                'End If

                If (dPrecio <> dImporte) Then
                    sHtmlRate &= String.Format("<tr><td>{0}</td> <td style='padding-left:6px;'>{1} {2}</td></tr> ",
                                                sDias, FCurrency(dImporte, 2), IIf(dImporte > 0, money, ""))
                    dPrecio = dImporte
                    sDias = ""
                End If
            End If
        Next
        sHtmlRate = String.Format("<table style=''>{0}</table>", sHtmlRate)
        Return sHtmlRate
    End Function

    Private Sub GetDataWS(ByVal nores As String)
        Try
            Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
            Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
            Dim xml As resHotelDisplay
            Dim isNetRateUV As Boolean = False

            Dim drR As reqHotelDisplay.HotelDisplayRow
            drR = dsreq.HotelDisplay.NewHotelDisplayRow
            drR.ConfirmNumber = nores
            If PortalCulture.GetIDCulture() = 1 Then
                drR.Language = "es-MX"
            Else
                drR.Language = "en-US"
            End If
            dsreq.HotelDisplay.AddHotelDisplayRow(drR)

            With New WSHotelFacade.clsFADisplay
                xml = .GetHotelDisplay(xdoc.DocumentElement)
            End With
            If Not xml Is Nothing AndAlso xml.Reservation.Rows.Count > 0 Then
                Dim dr As resHotelDisplay.ReservationRow = xml.Reservation.Rows(0)
                Dim source As String = String.Empty
                If xml.HotelHeader.Rows.Count > 0 Then
                    source = xml.HotelHeader(0)("Source")
                End If
                Dim AdExt As Integer = 0, NiExt As Integer = 0
                Dim totalNR As Double = 0, taxesNR As Double = 0
                Dim sChildAges As String
                Dim scoma As String
                sChildAges = ""
                scoma = ""
                If Not dr.IsIsNetRateUVNull Then
                    isNetRateUV = dr.IsNetRateUV
                    isNR = isNetRateUV
                End If
                If Not dr.IsTotalNRNull Then totalNR = dr.TotalNR
                If Not dr.IsTaxesNRNull Then taxesNR = dr.TaxesNR

                For Each drRoom As resHotelDisplay.RoomRow In xml.Room
                    If Not drRoom.IsExtraAdultsNull Then
                        AdExt += drRoom.ExtraAdults
                    End If
                    If Not drRoom.IsExtraChildrenNull Then
                        NiExt += drRoom.ExtraChildren
                    End If
                    If Not drRoom.IsChildrenAgesNull Then
                        If Not String.IsNullOrEmpty(drRoom.ChildrenAges) Then
                            sChildAges &= (scoma & drRoom.ChildrenAges)
                            scoma = ","
                        End If
                    End If

                Next

                Dim stblRooms As String = "<table border=0 cellpadding=0 width=80%>"
                Dim sHab As String
                stblRooms &= String.Format("<tr><td>&nbsp;</td><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                                          PortalCulture.GetString("00621"), PortalCulture.GetString("M000320"),
                                          PortalCulture.GetString("01367"), PortalCulture.GetString("00428"),
                                          PortalCulture.GetString("00429"))
                Dim i As Byte = 1
                For Each drRoom As resHotelDisplay.RoomRow In xml.Room
                    '  hab = String.Format("{0}" drRoom.NameRoom)
                    sHab = String.Format(PortalCulture.GetString("01396"), i)
                    stblRooms &= String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                                         sHab, drRoom.Adults, drRoom.Children, drRoom.ChildrenAges, drRoom.ExtraAdults, drRoom.ExtraChildren)
                    i += 1
                Next
                stblRooms &= "</table>"
                LitRooms.Text = stblRooms
                '((DateDiff(DateInterval.Day, Cin, Cout)).ToString)
                'xml.Reservation(0).CheckInDate
                'Dim cuartos As Byte = xml.Room.Count
                'stblRooms &= String.Format("<tr><td></td><td colspan ='5'> {0} {1}", cuartos.ToString, PortalCulture.GetString("M000321"))

                'LblNAdultosExt.Text = AdExt
                'lblNchildrenExt.Text = NiExt
                'lblChildAges.Text = If(String.IsNullOrEmpty(sChildAges), "-", sChildAges.Replace(",,", ","))

                If Not isNetRateUV Then 'NORMAL

                    aNRPolicies.Visible = False
                    If source.ToUpper().Trim() = "HTL" Then
                        lblTipoReservacion.Text = PortalCulture.GetString("01408")
                    Else
                        lblTipoReservacion.Text = PortalCulture.GetString("M0BT0000109")
                        If Not xml.Reservation(0).IsDepositTargetNull Then
                            lblTipoReservacion.Text = String.Format("{0}", PortalCulture.GetString("M0BT0000109"))

                        End If
                    End If

                    tdCostoUV.Visible = False
                    tdTextHotel.Visible = False
                    tdTextUnivisit.Visible = False

                    Me.lblTotalUV.Text = String.Empty
                    TotalConfirm = dr.Total
                    CurrencyConfirm = dr.Money

                    Me.lblTotal.Text = FCurrency(dr.Total, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblImpuestos.Text = FCurrency(dr.Taxes, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblFees.Text = FCurrency(dr.ServiceFee, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)

                    If Me.IsSupervisor Or IsUsuarioHotelAssociation Then
                        'mostrar la comision y el comisionfee
                        Me.lblTotal.Text = FCurrency((dr.Total + dr.ServiceFee), 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                        Me.lblImpuestos.Text = FCurrency((dr.Taxes + dr.ServiceFee), 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    End If
                    Me.lblTotalH.Text = FCurrency(dr.Total, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblCosto.Text = FCurrency(((dr.Total - dr.Taxes) / (DateDiff(DateInterval.Day, CDate(dr.CheckInDate), CDate(dr.CheckOutDate)))), 2) & " " & dr.Money

                    If xml.Reservation(0)("plustax").ToString.ToLower = "false" Then
                        lbl2.Visible = True
                        lblImpuestos.Visible = True
                        lblEImpuesto.Visible = True
                    End If
                Else ' NETRATE
                    aNRPolicies.Visible = Me.IsSupervisor Or IsUsuarioHotelAssociation
                    lblTipoReservacion.Text = PortalCulture.GetString("01125")
                    tdCostoUV.Visible = Me.IsSupervisor Or IsUsuarioHotelAssociation
                    tdTextHotel.Visible = Me.IsSupervisor Or IsUsuarioHotelAssociation
                    tdTextUnivisit.Visible = Me.IsSupervisor Or IsUsuarioHotelAssociation

                    'HOTEL
                    Me.lblTotal.Text = FCurrency(totalNR, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblImpuestos.Text = FCurrency(taxesNR, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblTotalH.Text = FCurrency(totalNR, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblCosto.Text = FCurrency(((totalNR - taxesNR) / (DateDiff(DateInterval.Day, CDate(dr.CheckInDate), CDate(dr.CheckOutDate)))), 2) & " " & dr.Money
                    Me.lblFees.Text = FCurrency(dr.ServiceFee, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    If xml.Reservation(0)("plustax").ToString.ToLower = "false" Then
                        lbl2.Visible = True
                        lblImpuestos.Visible = True
                        lblEImpuesto.Visible = True
                    End If

                    'UNIVISIT
                    Me.lblTotalUV.Text = FCurrency(dr.Total, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblImpuestosUV.Text = FCurrency(dr.Taxes, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblTotalHUV.Text = FCurrency(dr.Total, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    Me.lblCostoUV.Text = FCurrency(((dr.Total - dr.Taxes) / (DateDiff(DateInterval.Day, CDate(dr.CheckInDate), CDate(dr.CheckOutDate)))), 2) & " " & dr.Money
                    Me.lblFeesUV.Text = FCurrency(dr.ServiceFee, 2) & " " & If(dr.IsMoneyNull, "", dr.Money)
                    'lblNRPolicies.Text = CargaPoliticasUvNetRates()
                    If xml.Reservation(0)("plustax").ToString.ToLower = "false" Then
                        lbl2UV.Visible = True
                        lblImpuestosUV.Visible = True
                        lblEImpuestoUV.Visible = True
                    End If
                    TotalConfirm = dr.Total
                    CurrencyConfirm = dr.Money
                End If

                Me.txtNombreCL.Text = xml.Room(0).TravelerFirstName
                Me.txtApelldoCL.Text = xml.Room(0).TravelerLastName

                litDetalleTarifa.Text = TarifasHtml(xml, If(dr.IsMoneyNull, "", dr.Money))

                If xml.Reservation(0).AgeNombre <> "" Then
                    Dim strAgency As String
                    strAgency = "<table id='Table1' width='100%' align='center'>"
                    strAgency &= "      <tr>"
                    strAgency &= "<td vAlign='top'>"
                    strAgency &= "<span Class='bookingNormalLabel'>" & PortalCulture.GetString("00614") & "</span>"
                    strAgency &= "</td>"
                    strAgency &= "</tr>"
                    strAgency &= "<tr>"
                    strAgency &= "<td  vAlign='top'>"
                    strAgency &= "<p>" & xml.Reservation(0).AgeNombre & "</p>"
                    strAgency &= "                              </td>"
                    strAgency &= "</tr>"
                    strAgency &= "<tr>"
                    strAgency &= "<td  vAlign='top'>"
                    strAgency &= "<p>" & xml.Reservation(0).AgeCiudad & ", " & xml.Reservation(0).AgeEstado & ", " & xml.Reservation(0).AgePais & "</p>"
                    strAgency &= "</td>"
                    strAgency &= "</tr>"
                    strAgency &= "<tr>"
                    strAgency &= "<td vAlign='top'>"
                    strAgency &= "<p>" & PortalCulture.GetString("00615", True) & xml.Customer(0).FirstName & " " & xml.Customer(0).LastName & "</p>"
                    strAgency &= "</td>"
                    strAgency &= "</tr>"
                    If Not xml.Reservation(0).IsNull("PorcPor") Then
                        strAgency &= "<tr>"
                        strAgency &= "<td vAlign='top'>"
                        strAgency &= "<p>" & PortalCulture.GetString("M0BT0000322").Substring(0, 8) & " : " & xml.Reservation(0).PorcPor & "%</p>"
                        strAgency &= "</td>"
                        strAgency &= "</tr>"
                    End If
                    strAgency &= "</table>"
                    Me.AgencyInfo.InnerHtml = strAgency
                End If
                'Me.lblTravelerName.InnerHtml = xml.Room(0).TravelerName
                'Me.lblTravelerEmail.InnerHtml = xml.Room(0).EmailCliente

                If Not dr.IsReservationCurrencyNull AndAlso dr.ReservationCurrency.Trim <> "" AndAlso dr.Money <> dr.ReservationCurrency Then
                    If Not dr.IsReservationMoneyExchangeNull AndAlso dr.ReservationMoneyExchange <> 0 Then

                        lblEResCurrency.Visible = True
                        lblResCurrency.Visible = True
                        lblEMoneyChange.Visible = True
                        lblMoneyChange.Visible = True

                        lblResCurrency.Text = dr.ReservationCurrency

                        lblMoneyChange.Text = Format(dr.ReservationMoneyExchange, "#,###,##0.####")

                    End If

                End If

                If xml.Reservation(0).IsCancelationPoliciesNull() Or xml.Reservation(0).CancelationPolicies.Trim() = "" Then
                    trCancelPol.Visible = False
                Else
                    lblCancelationPolices.Text = xml.Reservation(0).CancelationPolicies
                End If

                If xml.Reservation(0).IsCreditCardPoliciesNull() Or xml.Reservation(0).CreditCardPolicies.Trim() = "" Then
                    trCardPol.Visible = False
                Else
                    lblCreditCardPolices.Text = xml.Reservation(0).CreditCardPolicies
                End If

                If xml.Reservation(0).IsGuaranteePoliciesNull() Or xml.Reservation(0).GuaranteePolicies.Trim() = "" Then
                    trGuarPol.Visible = False
                Else
                    lblGuaranteePolices.Text = xml.Reservation(0).GuaranteePolicies
                End If

                If (Not trCancelPol.Visible And Not trCardPol.Visible And Not trGuarPol.Visible) Then
                    aPolices.Visible = False
                End If




            End If
            If xml.Item.Count > 0 Then
                ancItems.Visible = True
                dgItems.DataSource = xml.Item
                dgItems.DataBind()
                lblTotalItems.Text = xml.Item.Compute("Sum(Price)", "")
            Else
                ancItems.Visible = False
            End If


            Dim nom, ap As String

            Me.lblAddress.Text = xml.Room(0).Address
            Me.lblZipCode.Text = If(xml.Room(0).IsNull("TravelerZipCode"), "", xml.Room(0).TravelerZipCode)
            Me.lblCustCity.Text = If(xml.Room(0).IsNull("TravelerCity"), "", xml.Room(0).TravelerCity) 'xml.Room(0).TravelerCity
            Me.lblCustCounty.Text = If(xml.Room(0).IsNull("TravelerState"), "", xml.Room(0).TravelerState) 'xml.Room(0).TravelerState
            Me.lblCustCountry.Text = If(xml.Room(0).IsNull("TravelerCountry"), "", xml.Room(0).TravelerCountry) 'xml.Room(0).TravelerCountry

            Me.lblContacto.Text = xml.Room(0).TravelerName
            Me.lblMail.Text = xml.Room(0).EmailCliente
            Me.lblPhoneH.Text = xml.Room(0).HomePhone
            Me.lblPhoneW.Text = xml.Room(0).WorkHome
            lblMotivoCancelacion.Text = PortalCulture.GetString("00737")


            If xml.Reservation.Rows.Count > 0 AndAlso xml.Reservation(0).CXNumber.Trim().Length > 0 Then

                If xml.Reservation(0).CXNumber.Trim().ToUpper().StartsWith("CXSYS") Or xml.Reservation(0).CXNumber.Trim().ToUpper().StartsWith("CXPMT") Then
                    'Es cancelacion de sistema
                    Me.lblMotivoCancelacion.Text = PortalCulture.GetString("01416")
                Else
                    Dim reason As String = xml.Reservation(0).CancellationReason.Trim()
                    Dim user As String = String.Empty
                    If xml.Reservation(0).CancellationidUser > 0 Then
                        Try
                            Dim userData As UserData = (New Portal.General.Facade.cUserSystem()).GetUserById(xml.Reservation(0).CancellationidUser)
                            If userData IsNot Nothing AndAlso userData.Tables.Contains(UserData.USER_TABLE) AndAlso userData.Tables(UserData.USER_TABLE).Rows.Count > 0 AndAlso Not userData.Tables(UserData.USER_TABLE).Rows(0).IsNull(UserData.EMAIL_FIELD) Then
                                user = userData.Tables(UserData.USER_TABLE).Rows(0)(UserData.EMAIL_FIELD)
                            End If
                        Catch ex As Exception
                        End Try
                    End If

                    If reason.Length > 0 Then
                        Me.lblMotivoCancelacion.Text = reason
                        If user.Length > 0 Then
                            Me.lblMotivoCancelacion.Text += " <br/>" + PortalCulture.GetString("00780") + ": " + user
                        End If
                    Else
                        Me.lblMotivoCancelacion.Text += " " + PortalCulture.GetString("00780") + ": "
                        If user.Length > 0 Then
                            'Me.lblMotivoCancelacion.Text += " <br/>" + PortalCulture.GetString("00780") + ": " + user
                            Me.lblMotivoCancelacion.Text += user
                        ElseIf xml.Customer.Count > 0 AndAlso xml.Customer(0).Email IsNot Nothing AndAlso xml.Customer(0).Email.Trim().ToString().Length > 0 Then
                            Me.lblMotivoCancelacion.Text += xml.Customer(0).Email
                        Else
                            Me.lblMotivoCancelacion.Text += PortalCulture.GetString("M000121")
                        End If
                    End If


                End If
            End If

            Dim dsrs As DataSet
            With New despositFacade
                dsrs = .GetReservationData(nores)
            End With

            lblDeposito.Visible = False
            If xml.Reservation(0).DepositAmount > 0 AndAlso xml.Reservation(0).Status = 1 Then
                'fue reservaci�n por dep�sito 

                If Not dsrs Is Nothing AndAlso dsrs.Tables(0).Rows.Count > 0 Then
                    lblDeposito.Text = String.Format(PortalCulture.GetString("00811"), FCurrency(dsrs.Tables(0).Rows(0).Item("dep_monto"), 2), Format(dsrs.Tables(0).Rows(0).Item("dep_moneda")))
                    lblDeposito.Visible = True
                End If
            End If

            If Not dsrs Is Nothing AndAlso dsrs.Tables(0).Rows.Count > 0 Then
                If Not dsrs.Tables(0).Rows(0).IsNull("IsZeroPayment") AndAlso dsrs.Tables(0).Rows(0)("IsZeroPayment") Then
                    lblTipoReservacion.Text = PortalCulture.GetString("01468")
                End If
            End If

            Dim IsPaymentOnline As Boolean
            Dim IsDineroMail As Boolean
            Dim IsPayU As Boolean
            Dim IsAmericanExpress As Boolean
            Dim IsAzubaPay As Boolean
            Dim IsMonthInterestBanorte As Boolean
            Dim IsDividedPay As Boolean
            Dim PaymentSource As String
            Select Case (xml.Reservation(0).PaymentSource)
                Case "1"
                    PaymentSource = "Banamex"
                Case "2"
                    PaymentSource = "Santander"
                Case "3"
                    PaymentSource = "DineroMail"
                Case "4"
                    PaymentSource = "Bancomer"
                Case "5"
                    PaymentSource = "Banorte"
                Case Else
                    PaymentSource = ""
            End Select

            IsPaymentOnline = xml.Reservation(0).OnlinePayment
            IsDineroMail = xml.Reservation(0).ReserveByDineroMail
            IsPayU = xml.Reservation(0).ReservedByPayU
            IsAmericanExpress = xml.Reservation(0).ReservedByAmericanExpress
            IsAzubaPay = xml.Reservation(0).ReservedByAzubaPay
            IsMonthInterestBanorte = xml.Tables("Reservation").Columns.Contains("MonthInterestBanorte") AndAlso Not xml.Reservation(0).IsMonthInterestBanorteNull AndAlso Not String.IsNullOrEmpty(xml.Reservation(0).MonthInterestBanorte)
            'lstPayMode.Items.Clear()
            'IsDividedPay = Not String.IsNullOrEmpty(xml.Reservation(0).NoAutorizacionDos)

            If IsPaymentOnline AndAlso IsSupervisor AndAlso xml.Reservation(0).Status = 4 Then
                Dim ds As DataSet
                With New despositFacade
                    ds = .GetReservationData(nores)
                End With
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso ds.Tables(0).Rows(0)("moneda") = "BTC" Then
                    btnResConfirm.Visible = False
                Else : btnResConfirm.Visible = True
                End If

            End If


            If xml.Reservation(0).IsReservedByPayPalNull Then xml.Reservation(0).ReservedByPayPal = False
            If xml.Reservation(0).IsOnRequestNull Then xml.Reservation(0).OnRequest = False
            If xml.Reservation(0).ReservedByPayPal = False AndAlso xml.Reservation(0).OnRequest = False _
               AndAlso Not IsPaymentOnline AndAlso Not IsDineroMail AndAlso Not IsPayU AndAlso Not IsAmericanExpress AndAlso Not IsAzubaPay Then

                Dim depositinfo As String = xml.Reservation(0).DepositInfo
                'lo quitamos X lo del redondeo
                'If resDisp.Reservation(0).AltTotal <> resDisp.Reservation(0).AltDepositAmount Then
                '    msg = HotelLanguage.GetString("HOTEL000363")
                'End If
                Dim deposit As Double = Format(CDbl(xml.Reservation(0).DepositAmount), "#,###,##0.00")
                'If Me.resDisp.Reservation(0).AltMoney = "MXN" Then
                '    deposit =                 Format(CDbl(Me.xml.Reservation(0).AltDepositAmount), "#,###,##0")
                'Else
                '    deposit = Format(CDbl(Me.resDisp.Reservation(0).AltDepositAmount), "#,###,##0.00")
                'End If

                'Me.lblpay.Text = String.Format(PortalCulture.GetString("HOTEL000360"), xml.Reservation(0).DepositReference, deposit & " " & xml.Reservation(0).Money) & "<div>" & depositinfo & "</div>"
                If deposit > 0 Then
                    Me.lblpay.Text = String.Format(PortalCulture.GetString("01391"),
                    If(xml.Reservation(0)("DepositTarget").ToString.ToUpper() = "UV", PortalCulture.GetString("01410"), PortalCulture.GetString("01411")),
                    xml.Reservation(0).DepositReference, "$ " & deposit.ToString("#,###,##0.00") & " ") ' & xml.Reservation(0).Money)
                    'Me.lblStatus.Text = PortalCulture.GetString("01393")
                End If

            ElseIf xml.Reservation(0).ReservedByPayPal Then
                'Me.lblpay.Text = PortalCulture.GetString("01387")
                Me.lblpay.Text = String.Format(PortalCulture.GetString("01387"), xml.Reservation(0).PayPalTxId, xml.Reservation(0).PayPalReceiptNo)
                'lstPayMode.Items.Add("PayPal")
                'lstPayMode.Items(0).Value = 0
            ElseIf IsPaymentOnline Then
                'Me.lblpay.Text = PortalCulture.GetString("01388")
                Dim ds As DataSet
                Dim deposit As Double
                Dim tc As Double
                With New despositFacade
                    ds = .GetReservationData(nores)
                End With

                Dim isCrypto As Boolean = False
                If ds.Tables(0).Rows.Count > 0 Then
                    If ds.Tables(0).Rows(0)("moneda") = "BTC" Then
                        isCrypto = True
                    End If
                End If

                tc = 1
                'If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                '    If Not ds.Tables(0).Rows(0).IsNull("TipoDeCambio") Then
                '        If (Not xml.Reservation(0).IsMoneyNull) And (Not xml.Reservation(0).IspaymentCurrencyNull) Then
                '            If xml.Reservation(0).paymentCurrency <> xml.Reservation(0).Money Then
                '                tc = FCurrency(ds.Tables(0).Rows(0).Item("TipoDeCambio"), 2)
                '            End If
                '        End If

                '    End If
                'End If
                Dim hr As Boolean = True
                If isNetRateUV Then
                    If IsSupervisor Or IsUsuarioHotelAssociation Then

                    Else
                        hr = False
                    End If
                End If

                If hr Then
                    Try
                        If Not xml.Reservation(0).IsPrepaymentNull Then
                            If xml.Reservation(0).Prepayment > 0 Then
                                tc = xml.Reservation(0).paymentAmount / xml.Reservation(0).Prepayment
                            Else
                                tc = xml.Reservation(0).paymentAmount / xml.Reservation(0).Total
                            End If
                        End If
                    Catch ex As Exception
                        Trace.Write(" ... " & ex.StackTrace)
                    End Try

                End If

                Double.TryParse(xml.Reservation(0).paymentAmount, deposit)

                'Poner el método de pago que se está utilizando

                If IsDineroMail Then
                    Me.lblpay.Text = PortalCulture.GetString("01390")
                    'lstPayMode.Items.Add("Dinero Mail")
                    'lstPayMode.Items(0).Value = 0
                ElseIf IsPayU Then
                    Me.lblpay.Text = String.Format(PortalCulture.GetString("01641"), xml.Reservation(0).NoAutorizacion)
                    'lstPayMode.Items.Add("Pay U")
                    'lstPayMode.Items(0).Value = 0
                ElseIf IsAmericanExpress Then
                    Me.lblpay.Text = String.Format(PortalCulture.GetString("01642"), xml.Reservation(0).NoAutorizacion)
                    'lstPayMode.Items.Add("American Express")
                    'lstPayMode.Items(0).Value = 0
                ElseIf IsAzubaPay Then
                    Me.lblpay.Text = String.Format(PortalCulture.GetString("01643"), xml.Reservation(0).NoAutorizacion)
                    'lstPayMode.Items.Add("Azuba Pay")
                    'lstPayMode.Items(0).Value = 0
                Else

                    If isCrypto Then
                        lblpay.Text = String.Format(PortalCulture.GetString("01388"), "N/A", ds.Tables(0).Rows(0)("referencia"), PortalCulture.GetString("01660"))
                    Else
                        Me.lblpay.Text = String.Format(PortalCulture.GetString("01388"), If(xml.Reservation(0).Status = 1 Or xml.Reservation(0).Status = 3, xml.Reservation(0).NoAutorizacion, "N/A"), xml.Reservation(0).PaymentReference, PaymentSource)
                    End If

                    'lstPayMode.Items.Add("Banamex")
                    'lstPayMode.Items(0).Value = 1
                    'lstPayMode.Items.Add("Bancomer")
                    'lstPayMode.Items(1).Value = 4
                    'lstPayMode.Items.Add("Banorte")
                    'lstPayMode.Items(2).Value = 5
                End If
                'If IsDividedPay Then
                '    Dim meses As String()
                '    meses = xml.Reservation(0).MonthInterestBanorte.ToString().Split(",")
                '    If IsMonthInterestBanorte Then
                '        Me.lblpay.Text += "</br>" + String.Format("Pago dividido / Tarjeta 1: ${0}, {1} Meses Sin Intereses/ Tarjeta 2: ${2}, {3} Meses Sin Intereses", xml.Reservation(0).BanortePayOne.ToString, meses(0), xml.Reservation(0).BanortePayTwo.ToString, meses(1))
                '    Else
                '        Me.lblpay.Text += "</br>" + String.Format("Pago dividido / Tarjeta 1: ${0} / Tarjeta 2: ${1}", xml.Reservation(0).BanortePayOne.ToString, xml.Reservation(0).BanortePayTwo.ToString)
                '    End If

                'End If
                If IsMonthInterestBanorte Then
                    Me.lblpay.Text += String.Format(PortalCulture.GetString("01644"), xml.Reservation(0).MonthInterestBanorte.ToString())
                End If

                If isCrypto Then
                    If xml.Reservation(0).IsAltDepositAmountNull Then xml.Reservation(0).AltDepositAmount = ""
                    lblpay.Text &= String.Format("<br/>{0}: {1} {2}", PortalCulture.GetString("00037"), xml.Reservation(0).AltDepositAmount, "BTC")
                Else
                    If hr Then
                        Me.lblpay.Text &= String.Format("<br/>{0}: {1} {2}", PortalCulture.GetString("00037"), "$ " & deposit.ToString("#,###,##0.00"), "")
                        'Me.lblpay.Text &= If(tc > 1, String.Format("<br/>{0}: {1}", PortalCulture.GetString("M0BT0000030"), tc.ToString("#####0.00")), "")
                    End If
                End If



            ElseIf IsDineroMail Then
                Me.lblpay.Text = PortalCulture.GetString("01390")
            ElseIf IsPayU Then
                Me.lblpay.Text = String.Format(PortalCulture.GetString("01641"), xml.Reservation(0).NoAutorizacion)
            ElseIf IsAmericanExpress Then
                Me.lblpay.Text = String.Format(PortalCulture.GetString("01642"), xml.Reservation(0).NoAutorizacion)
            ElseIf IsAzubaPay Then
                Me.lblpay.Text = String.Format(PortalCulture.GetString("01643"), xml.Reservation(0).NoAutorizacion)
            Else
                Me.lblpay.Text = ""
            End If

        Catch ex As Exception
            Trace.Write(" ... " & ex.StackTrace + ":" + ex.Message)
            MyBase.guardalog("ReservationDetails.aspx", 1, ex.Message)
            Response.Redirect(Me.UrlPage(PaginaBase.pages.ReservationList))

        End Try

    End Sub

    Private Function CargaPoliticasUvNetRates() As String
        Try
            Dim rssFile As String = GeRequestApplicationPath(String.Concat("/Politicas/", PortalCulture.GetCulture.ToString.Substring(0, 2), "/UvNetRatesPolicies.txt"))
            Dim tr As System.IO.TextReader
            Dim str As String

            tr = New System.IO.StreamReader(System.Web.HttpContext.Current.Server.MapPath(rssFile), System.Text.Encoding.Default)
            str = tr.ReadToEnd
            tr.Close()
            Return str
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function cargausuarios() As Boolean
        Dim ds As UsuarioHotelData
        Dim isShow As Boolean

        With New UsuarioHotelFacade
            ds = .GetUserListByIdUser(MyBase.Usuario)
            isShow = True
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                If Not ds.Tables(UsuarioHotelData.UsuarioTable).Rows(0).IsNull(UsuarioHotelData.verDatosTarjeta) Then
                    isShow = ds.Tables(UsuarioHotelData.UsuarioTable).Rows(0)(UsuarioHotelData.verDatosTarjeta)
                End If
            End If
        End With
        Return isShow
    End Function


    Public Function LoadIDSReservaConfirmNum(ByVal NoConfirm As String) As DataSet
        Dim data As New DataSet
        Dim dsCommand As New SqlDataAdapter
        Dim loadCommand As SqlCommand

        Try
            loadCommand = New SqlCommand("spReservationGetByConfirmNumber", New SqlConnection(ConfigurationSettings.AppSettings("RMSConnection")))
            loadCommand.CommandType = CommandType.StoredProcedure
            loadCommand.Parameters.Add(New SqlParameter("@ConfirNumber", SqlDbType.NVarChar, 20))

            dsCommand.SelectCommand = loadCommand
            dsCommand.SelectCommand.Parameters("@ConfirNumber").Value = NoConfirm
            dsCommand.Fill(data)
        Catch
        Finally
            If Not dsCommand.SelectCommand Is Nothing Then
                If Not dsCommand.SelectCommand.Connection Is Nothing Then
                    dsCommand.SelectCommand.Connection.Dispose()
                End If
                dsCommand.SelectCommand.Dispose()
            End If
            loadCommand.Dispose()
        End Try
        Return data
    End Function

    Public Function LoadUserSeeCards(ByVal UserId As Integer)
        Dim data As New DataSet
        Dim dsCommand As New SqlDataAdapter
        Dim loadCommand As SqlCommand

        Try
            loadCommand = New SqlCommand("GetUserCompanyAllowSeeCCByIds", New SqlConnection(ConfigurationSettings.AppSettings("PortalConnectionString")))
            loadCommand.CommandType = CommandType.StoredProcedure
            loadCommand.Parameters.Add(New SqlParameter("@UserId", SqlDbType.Int))
            loadCommand.Parameters.Add(New SqlParameter("@CompanyId", SqlDbType.Int))
            dsCommand.SelectCommand = loadCommand
            dsCommand.SelectCommand.Parameters("@UserId").Value = UserId
            dsCommand.SelectCommand.Parameters("@CompanyId").Value = cInfoActual.Empresa
            dsCommand.Fill(data)
            If data IsNot Nothing AndAlso data.Tables.Count > 0 AndAlso data.Tables(0).Rows.Count > 0 Then
                canSeeCArds = True
            Else
                'canSeeCArds = False
                'data = New DataSet
                'loadCommand = New SqlCommand("spUsuarioHotelGetByIdUser", New SqlConnection(ConfigurationSettings.AppSettings("HotelConnection")))
                'loadCommand.Parameters.Clear()
                'loadCommand.CommandType = CommandType.StoredProcedure
                'loadCommand.Parameters.Add(New SqlParameter("@iduserField", SqlDbType.Int))
                'dsCommand.SelectCommand = loadCommand
                'dsCommand.SelectCommand.Parameters("@iduserField").Value = UserId
                'dsCommand.Fill(data)
                'If data IsNot Nothing AndAlso data.Tables(0).Rows.Count > 0 AndAlso CDbl(data.Tables(0).Rows(0)("verDatosTarjeta")) Then
                '    canSeeCArds = True
                'Else
                '    canSeeCArds = False
                'End If

                loadCommand = New SqlCommand("GetUserHotelsAllowSeeCCByIds", New SqlConnection(ConfigurationSettings.AppSettings("hotelconnectionstring")))
                loadCommand.CommandType = CommandType.StoredProcedure
                loadCommand.Parameters.Add(New SqlParameter("@UserId", SqlDbType.Int))
                loadCommand.Parameters.Add(New SqlParameter("@HotelId", SqlDbType.Int))
                dsCommand.SelectCommand = loadCommand
                dsCommand.SelectCommand.Parameters("@UserId").Value = UserId
                dsCommand.SelectCommand.Parameters("@HotelId").Value = cInfoActual.Hotel
                dsCommand.Fill(data)

                If data IsNot Nothing AndAlso data.Tables.Count > 0 AndAlso data.Tables(0).Rows.Count > 0 Then
                    canSeeCArds = True
                End If

            End If
        Catch
        Finally
            If Not dsCommand.SelectCommand Is Nothing Then
                If Not dsCommand.SelectCommand.Connection Is Nothing Then
                    dsCommand.SelectCommand.Connection.Dispose()
                End If
                dsCommand.SelectCommand.Dispose()
            End If
            loadCommand.Dispose()
        End Try
    End Function

    Private Function getData(ByVal ID As Integer)
        Dim dsReservaciones As ReservaDatos
        Dim Cin, Cout As Date
        Dim dt As Date
        Dim tipoUsuario As Integer
        Me.CanShowConfirmButton = False

        With New ReservaFacade
            dsReservaciones = .GetDataReserva(ID, PortalCulture.GetIDCulture())
        End With

        If dsReservaciones.Tables(2).Rows.Count > 0 Then
            If dsReservaciones.Tables(2).Rows(0).Item("Permission") Then
                lblMail.Visible = True
                lblEmail.Visible = True
                lblEPhoneH.Visible = True
                lblPhoneH.Visible = True
                lblEPhoneW.Visible = True
                lblPhoneW.Visible = True
                lblEAddress.Visible = True
                lblAddress.Visible = True
                lblECode.Visible = True
                lblZipCode.Visible = True
                lblCiudad.Visible = True
                lblCustCity.Visible = True
                lblEstado.Visible = True
                lblCustCounty.Visible = True
                lblPais.Visible = True
                lblCustCountry.Visible = True
            End If
        End If

        If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows.Count > 0 Then
            With dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)
                tipoUsuario = .Item("TipoUsuario")
                Me.IdReservacion = ID
                Dim pagoData As New DataTable()
                Try
                    With New SqlDataAdapter("spPagosReservacionesGetById", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
                        .SelectCommand.CommandType = CommandType.StoredProcedure
                        .SelectCommand.Parameters.Add("@idReservacion", SqlDbType.Int).Value = Me.IdReservacion
                        Try
                            .SelectCommand.Connection.Open()
                            .Fill(pagoData)
                        Catch ex As Exception
                        Finally
                            .SelectCommand.Connection.Close()
                        End Try
                    End With
                Catch ex As Exception
                    Trace.Write(ex.Message)
                    pagoData = New DataTable()
                End Try

                Me.CanShowConfirmButton = (Me.IsSupervisor AndAlso ((.Item(dsReservaciones.FIELD_STATUS) = 3 AndAlso .Item("NoCancelacion").ToString().ToUpper().StartsWith("CXSYS")) OrElse .Item(dsReservaciones.FIELD_STATUS) = 4) AndAlso pagoData IsNot Nothing AndAlso pagoData.Rows.Count > 0 AndAlso .Item("IsOnlinePayment"))

                Dim cuartos As Byte = dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows.Count
                Dim dr As DataRow
                Dim moneda As String = "USD"
                Dim adul, ninio As Byte
                For Each dr In dsReservaciones.Tables(0).Rows
                    adul += IIf(dr.IsNull(dsReservaciones.FIELD_ADULTOS), 0, dr.Item(dsReservaciones.FIELD_ADULTOS))
                    ninio += IIf(dr.IsNull(dsReservaciones.FIELD_NINIOS), 0, dr.Item(dsReservaciones.FIELD_NINIOS))
                Next
                'Me.lblNAdultos.Text = adul
                'Me.lblNchildren.Text = ninio
                lblNoCancelacion.Text = dsReservaciones.Tables(0).Rows(0).Item("NoCancelacion").ToString
                Dim src As String = ""
                If Not dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).IsNull(dsReservaciones.FIELD_SOURCE) Then
                    src = .Item(dsReservaciones.FIELD_SOURCE)
                    SourceReserv = src
                End If

                Dim isNetRateUv As Boolean
                Dim idAsociacion As Integer
                isNetRateUv = If(Not .IsNull(dsReservaciones.FIELD_IsNetRateUv), .Item(dsReservaciones.FIELD_IsNetRateUv), False)
                idAsociacion = If(Not .IsNull("idAsociacion"), .Item("idAsociacion"), 0)
                Me.lblStatus.Text = GetStatus(.Item(dsReservaciones.FIELD_STATUS), src, GetIdCorporativoPortal(dsReservaciones), isNetRateUv, idAsociacion)


                If .Item(dsReservaciones.FIELD_EMPRESASEGMENTO).ToString() = "" Then
                    lblCompanySegment.Visible = False
                    lblCompanySegmentTitle.Visible = False
                Else
                    lblCompanySegment.Visible = True
                    lblCompanySegmentTitle.Visible = True
                    lblCompanySegment.Text = .Item(dsReservaciones.FIELD_EMPRESASEGMENTO).ToString()
                End If

                If .Item(dsReservaciones.FIELD_SEGMENTO).ToString() = "" Then
                    lblSegment.Visible = False
                    lblSegmentTitle.Visible = False
                Else
                    lblSegment.Visible = True
                    lblSegmentTitle.Visible = True
                    lblSegment.Text = .Item(dsReservaciones.FIELD_SEGMENTO).ToString()
                End If

                Me.lblCantAdults.Text = "0"
                Me.lblPriceAdult.Text = "0.0"
                Me.lblCantChild.Text = "0"
                Me.lblPriceChild.Text = "0.0"
                Me.lblCantCrib.Text = "0"
                Me.lblPriceCrib.Text = "0.0"

                If Not .IsNull(dsReservaciones.FIELD_RAwayAdult) Then
                    Me.lblCantAdults.Text = .Item(dsReservaciones.FIELD_RAwayAdult)
                    If Not .IsNull(dsReservaciones.FIELD_PriceRAwayAdult) Then
                        Me.lblPriceAdult.Text = FCurrency(.Item(dsReservaciones.FIELD_PriceRAwayAdult), 2)
                    End If
                End If
                If Not .IsNull(dsReservaciones.FIELD_RAwayChild) Then
                    Me.lblCantChild.Text = .Item(dsReservaciones.FIELD_RAwayChild)
                    If Not .IsNull(dsReservaciones.FIELD_PriceRAwayChild) Then
                        Me.lblPriceChild.Text = FCurrency(.Item(dsReservaciones.FIELD_PriceRAwayChild), 2)
                    End If
                End If
                If Not .IsNull(dsReservaciones.FIELD_RAwayCrib) Then
                    Me.lblCantCrib.Text = .Item(dsReservaciones.FIELD_RAwayCrib)
                    If Not .IsNull(dsReservaciones.FIELD_PriceRAwayCrib) Then
                        Me.lblPriceCrib.Text = FCurrency(.Item(dsReservaciones.FIELD_PriceRAwayCrib), 2)
                    End If
                End If
                If .Item(dsReservaciones.FIELD_WizcomPassOn).ToString <> "" Then

                    Select Case .Item(dsReservaciones.FIELD_SystemCode).ToString.ToUpper
                        Case "AA"
                            Me.lblSystemCode.Text = "Sabre"
                        Case "UA"
                            Me.lblSystemCode.Text = "Galileo"
                        Case "1A"
                            Me.lblSystemCode.Text = "Amadeus"
                        Case "TW"
                            Me.lblSystemCode.Text = "WorldSpan"
                    End Select
                    Me.lblBookingSource.Text = .Item(dsReservaciones.FIELD_BookingSource).ToString
                    Me.lblRecordLocator.Text = .Item(dsReservaciones.FIELD_RecordLocator).ToString

                    Dim ds As New DataSet
                    Dim dv As DataView
                    ds.ReadXml(Server.MapPath(GeRequestApplicationPath("/Data/RFCode.xml")))
                    dv = ds.Tables(0).DefaultView
                    If .Item(dsReservaciones.FIELD_RFCode).ToString.Length >= 2 Then
                        dv.RowFilter = "Code='" & .Item("RFCode").ToString.Substring(0, 2) & "'"
                        If dv.Count > 0 Then
                            Me.lblRF.Text = dv(0)("Description")
                        End If
                    End If
                    If .Item(dsReservaciones.FIELD_StatusConf) = False Then

                        Me.lblStatus.Text = PortalCulture.GetString("00439")
                    End If
                    showSwitchDataEtiquetas(True)
                Else
                    showSwitchDataEtiquetas(False)
                End If

                If .Item(dsReservaciones.FIELD_IDHOTEL) = "0" Then
                    Me.lblNHotel.Text = IIf(.IsNull(dsReservaciones.FIELD_HOTELGNOMBRE), "Desconocido", .Item(dsReservaciones.FIELD_HOTELGNOMBRE))
                    Me.lblCdHotel.Text = IIf(.IsNull(dsReservaciones.FIELD_HOTELGCIUDAD), "Desconocido", .Item(dsReservaciones.FIELD_HOTELGCIUDAD))
                    lblDirHotel.Text = ""
                Else
                    Me.lblNHotel.Text = .Item(dsReservaciones.FIELD_HOTELNOMBRE)
                    Dim ciudad As String = .Item(dsReservaciones.FIELD_HOTELCIUDAD) & ", " & .Item(dsReservaciones.FIELD_HOTELESTADO)
                    Me.lblCdHotel.Text = ciudad
                    Me.lblDirHotel.Text = .Item(dsReservaciones.FIELD_HOTELDOMICILIO)

                End If
                Dim ci As System.Globalization.CultureInfo
                ci = System.Threading.Thread.CurrentThread.CurrentUICulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)






                Me.txtCheckIn.Text = CDate(.Item(dsReservaciones.FIELD_CHECKIN)).ToString("MM/dd/yyyy")
                Me.txtCheckOut.Text = CDate(.Item(dsReservaciones.FIELD_CHECKOUT)).ToString("MM/dd/yyyy")
                Me.txtTotalModify.Text = CDbl(.Item(dsReservaciones.FIELD_TOTAL)).ToString("N2")
                Me.txtTotalNRModify.Text = CDbl(.Item(dsReservaciones.FIELD_TotalNR)).ToString("N2")
                Me.spnCurrencyTotal.InnerText = .Item(dsReservaciones.FIELD_MONEDA)
                Me.spnCurrencyTotalNR.InnerText = .Item(dsReservaciones.FIELD_MONEDA)
                Me.lblIn.Text = CDate(.Item(dsReservaciones.FIELD_CHECKIN)).ToString("dd/MMM/yyyy")
                Me.lblOut.Text = CDate(.Item(dsReservaciones.FIELD_CHECKOUT)).ToString("dd/MMM/yyyy")
                Me.lblResDate.Text = CDate(.Item("FechaReservacion")).ToString("dd/MMM/yyyy HH:mm:ss tt")
                System.Threading.Thread.CurrentThread.CurrentCulture = ci
                Me.lblNCuartos.Text = cuartos.ToString & "  "
                Cin = .Item(dsReservaciones.FIELD_CHECKIN)
                Cout = .Item(dsReservaciones.FIELD_CHECKOUT)
                Me.lblNNoches.Text = ((DateDiff(DateInterval.Day, Cin, Cout)).ToString) & "  "

                Me.lblID.Text = .Item(dsReservaciones.FIELD_NORESERVACION)
                If .Table.Columns.Contains("NoConfirmPMS") AndAlso .Item("NoConfirmPMS").ToString <> "0" Then
                    Me.lblPMSConfirm.Text = .Item("NoConfirmPMS")
                    'Me.lblPMSConfirm.Visible = True
                End If
                NoReserv = .Item(dsReservaciones.FIELD_NORESERVACION)
                'Dim nom, ap As String
                'Me.lblAddress.Text = .Item(dsReservaciones.FIELD_Direccion_cl).ToString
                'Me.lblContacto.Text = IIf(.IsNull(dsReservaciones.FIELD_CLIENTE), " -", .Item(dsReservaciones.FIELD_CLIENTE))
                'Me.lblMail.Text = IIf(.IsNull(dsReservaciones.FIELD_EMAILCL), " -", .Item(dsReservaciones.FIELD_EMAILCL))
                'Me.lblPhoneH.Text = IIf(.IsNull(dsReservaciones.FIELD_TELEFONOCL), " -", .Item(dsReservaciones.FIELD_TELEFONOCL))
                'Me.lblPhoneW.Text = IIf(.IsNull(dsReservaciones.FIELD_TELTRABAJOCL), " -", .Item(dsReservaciones.FIELD_TELTRABAJOCL))
                '***
                lblCCName.Text = IIf(.IsNull(dsReservaciones.FIELD_NOMBRECC), " -", .Item(dsReservaciones.FIELD_NOMBRECC))
                lblCCNumber.Text = " -"
                Me.lblCCType.Text = " -"



                If Not dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).IsNull("source") Then
                    GetDataWS(dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).Item(dsReservaciones.FIELD_NORESERVACION))
                End If

                LoadUserSeeCards(MyBase.Usuario)
                If Not .IsNull(dsReservaciones.FIELD_NUMEROCC) Then
                    Dim ccN As String = String.Empty
                    ccN = crypto.DecryptString128Bit(.Item(dsReservaciones.FIELD_NUMEROCC), crypto.PublicKey)

                    If ccN.Length >= 4 Then
                        ccN = Right(("XXXXXXXXXXXXXXXX" & Right(ccN, 4)), 16)
                    End If
                    If Not Session(AppSettings("RestTarjetas")) Is Nothing AndAlso Session(AppSettings("RestTarjetas")) = "1" AndAlso Not canSeeCArds Then
                        lblCCNumber.Text = ccN
                    Else
                        If (Not IsSupervisor And isNR = True) Or Not lblCCExp.Visible Then
                            lblCCNumber.Text = ccN
                        Else
                            lblCCNumber.Text = crypto.DecryptString128Bit(.Item(dsReservaciones.FIELD_NUMEROCC), crypto.PublicKey)
                        End If
                    End If

                ElseIf pagoData IsNot Nothing AndAlso pagoData.Rows.Count > 0 Then
                    If pagoData.Columns.Contains("OnlineCCType") AndAlso pagoData.Columns.Contains("OnlineCCNumber") Then
                        If Not pagoData.Rows(0).IsNull("OnlineCCType") AndAlso Not pagoData.Rows(0).IsNull("OnlineCCNumber") Then
                            lblCCNumber.Text = pagoData.Rows(0)("OnlineCCNumber").ToString
                            lblCCType.Text = pagoData.Rows(0)("OnlineCCType").ToString
                        End If
                    End If
                End If

                Dim cvv As String = IIf(.IsNull(dsReservaciones.FIELD_DIGITOCC), "", .Item(dsReservaciones.FIELD_DIGITOCC))

                If Not Session(AppSettings("RestTarjetas")) Is Nothing AndAlso Session(AppSettings("RestTarjetas")) = "1" Then
                    'Dim ccn As String = IIf(.IsNull(dsReservaciones.FIELD_DIGITOCC), " -", .Item(dsReservaciones.FIELD_DIGITOCC))
                    If cvv.Length >= 3 Then
                        lblCCcvNumber.Text = "XXX"
                    Else
                        lblCCcvNumber.Text = IIf(cvv = String.Empty, " -", cvv)
                    End If
                Else
                    If Not IsSupervisor And isNR = True Then
                        lblCCcvNumber.Text = "XXX"
                    Else
                        If Regex.IsMatch(cvv, "[A-Z]") Then
                            cvv = crypto.DecryptString128Bit(cvv, crypto.PublicKey)
                        End If
                        lblCCcvNumber.Text = IIf(cvv = String.Empty, " -", cvv)
                    End If

                End If

                If Not Session(AppSettings("RestTarjetas")) Is Nothing AndAlso Session(AppSettings("RestTarjetas")) = "1" AndAlso Not canSeeCArds Then
                    lnkShowCC.Visible = False
                Else
                    If (IsSupervisor Or canSeeCArds) And Not .IsNull(dsReservaciones.FIELD_NUMEROCC) Then 'se cambio la linea, quitando el isNR(IsSupervisor Or Not isNR)
                        lnkShowCC.Visible = True
                    Else
                        lnkShowCC.Visible = False
                    End If
                End If

                If Not .Item(dsReservaciones.FIELD_Tipo_CC) Is System.DBNull.Value Then
                    Me.lblCCType.Text = decodeCards(.Item(dsReservaciones.FIELD_Tipo_CC))
                End If

                If Not .IsNull(dsReservaciones.FIELD_EXPIRAMESCC) And Not .IsNull(dsReservaciones.FIELD_EXPIRANIOCC) Then
                    lblCCExpDate.Text = .Item(dsReservaciones.FIELD_EXPIRAMESCC) & "/" & .Item(dsReservaciones.FIELD_EXPIRANIOCC)
                Else
                    lblCCExpDate.Text = " -"
                End If
                '***
                Dim impuesto, total, tarifa, totalHotel As Decimal
                Dim comision As Double
                tarifa = 0
                If .IsNull(dsReservaciones.FIELD_IMPUESTO) Or .Item(dsReservaciones.FIELD_IMPUESTO) = "0" Then
                    impuesto = 0
                Else
                    impuesto = CType(.Item(dsReservaciones.FIELD_IMPUESTO), Decimal)
                    impuesto = 1 + (impuesto / 100)
                End If

                comision = 0


                Dim totsinimpuesto As Double
                Dim ComissionFee As Double
                Dim impuestocal As Double = 0, totaltotal As Double = 0
                totaltotal = .Item(dsReservaciones.FIELD_TOTAL)

                Dim dshoteles As HotelDatos
                If dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_IDHOTEL) <> 0 Then
                    With New HotelSistema
                        dshoteles = .GetHotelById(dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_IDHOTEL))
                    End With

                    Try
                        If Not dsReservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows(0).Item(ReservaDatos.FIELD_MONEDA) Is System.DBNull.Value Then
                            moneda = dsReservaciones.Tables(ReservaDatos.RESERVA_TABLE).Rows(0).Item(ReservaDatos.FIELD_MONEDA)
                        End If
                        'Dim m As MonedaDatos
                        'With New MonedaSistema
                        '    m = .GetMonedaById(dshoteles.Tables(dshoteles.HOTEL_TABLE).Rows(0).Item(dshoteles.FIELD_IDMONEDA))
                        '    moneda = m.Tables(m.MONEDA_TABLE).Rows(0).Item("codigo")
                        'End With
                    Catch ex As Exception
                        moneda = ""
                    End Try
                Else
                    If Not dsReservaciones.Tables(ReservaDatos.MONEDA_TABLE) Is Nothing AndAlso dsReservaciones.Tables(ReservaDatos.MONEDA_TABLE).Rows.Count > 0 Then
                        moneda = dsReservaciones.Tables(ReservaDatos.MONEDA_TABLE).Rows(0).Item(ReservaDatos.FIELD_MONEDA)
                    End If

                End If


                If CDbl(.Item(dsReservaciones.FIELD_IMPUESTO)) > 0 Then
                    totsinimpuesto = .Item(dsReservaciones.FIELD_TOTAL) / (1 + CDbl(.Item(dsReservaciones.FIELD_IMPUESTO) / 100))
                    impuestocal = .Item(dsReservaciones.FIELD_TOTAL) - .Item(dsReservaciones.FIELD_TOTAL) / (1 + CDbl(.Item(dsReservaciones.FIELD_IMPUESTO) / 100))
                Else
                    If .IsNull(HotelDatos.FIELD_PLUSTAX) OrElse .Item(HotelDatos.FIELD_PLUSTAX) = 1 Then
                        If .Item(dsReservaciones.FIELD_IDHOTEL) <> 0 Then
                            'traer el impuesto de la tabla hoteles
                            If dshoteles.Tables.Count > 0 AndAlso dshoteles.Tables(dshoteles.HOTEL_TABLE).Rows.Count > 0 Then
                                totsinimpuesto = .Item(dsReservaciones.FIELD_TOTAL) / (1 + CDbl(Val(dshoteles.Tables(dshoteles.HOTEL_TABLE).Rows(0).Item(dshoteles.FIELD_IMPUESTO))) / 100)
                                impuestocal = .Item(dsReservaciones.FIELD_TOTAL) - .Item(dsReservaciones.FIELD_TOTAL) / (1 + CDbl(Val(dshoteles.Tables(dshoteles.HOTEL_TABLE).Rows(0).Item(dshoteles.FIELD_IMPUESTO))) / 100)

                            End If
                        Else
                            totsinimpuesto = .Item(dsReservaciones.FIELD_TOTAL)
                        End If
                    Else
                        totsinimpuesto = .Item(dsReservaciones.FIELD_TOTAL)
                    End If
                End If

                If Not .IsNull(HotelDatos.FIELD_PLUSTAX) AndAlso .Item(HotelDatos.FIELD_PLUSTAX) Then
                    lblMsgImpuesto.Text = PortalCulture.GetString("00611")
                    lblMsgImpuesto.Visible = True
                    lblMsgImpuestoUV.Text = PortalCulture.GetString("00611")
                    lblMsgImpuestoUV.Visible = True
                Else
                    lblMsgImpuesto.Visible = False
                    lblMsgImpuestoUV.Visible = False
                End If

                If Not .IsNull(dsReservaciones.FIELD_COMISION) AndAlso .Item(dsReservaciones.FIELD_COMISION) > 0 Then
                    comision = totsinimpuesto * .Item(dsReservaciones.FIELD_COMISION) / 100
                End If
                If Not .IsNull(dsReservaciones.FIELD_COMMISIONNoPerc) Then
                    ComissionFee = CDbl(.Item(dsReservaciones.FIELD_COMMISIONNoPerc))
                Else
                    'coincidir con las versiones anterioriores
                    'las dos reservaciones que se hicieron calculava el fee multipl. el total con impuesto por el porcentaje de comision
                    If Not .IsNull(dsReservaciones.FIELD_COMISION) AndAlso Not .IsNull(dsReservaciones.FIELD_WizcomPassOn) > 0 Then
                        ComissionFee = .Item(dsReservaciones.FIELD_TOTAL) * CDbl(Val(.Item(dsReservaciones.FIELD_COMISION))) / 100
                    End If

                End If
                Dim subtotal As Double = 0
                If .IsNull(dsReservaciones.FIELD_TOTAL) Or .Item(dsReservaciones.FIELD_TOTAL) = "0" Then
                    total = 0
                Else
                    subtotal = totsinimpuesto
                    impuesto = impuestocal
                    total = subtotal
                    tarifa = subtotal / (DateDiff(DateInterval.Day, Cin, Cout))
                    totalHotel = totaltotal
                End If


                'Me.lblComision.Text = FormatCurrency(comision, 2) & " " & moneda
                'Me.lblImpuestos.Text = FormatCurrency(impuesto, 2) & " " & moneda
                'Me.lblTotal.Text = FormatCurrency(totaltotal, 2) & " " & moneda '"###0.00"
                'Me.lblCosto.Text = FormatCurrency(tarifa, 2) & " " & moneda
                'Me.lblTotalH.Text = FormatCurrency(totalHotel, 2) & " " & moneda
                'Me.lblFees.Text = FormatCurrency(ComissionFee, 2) & " " & moneda
                'If Me.IsSupervisor Then
                '    'mostrar la comision y el comisionfee
                '    Me.lblTotal.Text = FormatCurrency(totaltotal + ComissionFee, 2) & " " & moneda
                '    Me.lblImpuestos.Text = FormatCurrency(impuesto + ComissionFee, 2) & " " & moneda
                'End If

                Me.lblConfirmationNumber.Text = .Item(dsReservaciones.FIELD_NoConfGal).ToString
                Me.lblNoConf.Text = PortalCulture.GetString("M000650", True)
                If .Item(dsReservaciones.FIELD_STATUS) = 3 Then
                    Me.lblNoConf.Text = PortalCulture.GetString("M000651", True)
                    lblConfirmationNumber.Text = .Item(dsReservaciones.FIELD_NoConfCancelGal).ToString
                End If
                lblreclocnumber.Text = .Item(dsReservaciones.FIELD_Recloc).ToString
                Me.lblSource.Text = .Item(dsReservaciones.FIELD_PortalName).ToString

                Me.divClientportal.Style.Add("display", "none")
                dgReservas.Columns(Columns.Cliente).Visible = False
                If .Item("source").ToString.ToUpper = "POR" Then
                    dgReservas.Columns(Columns.Cliente).Visible = True
                    Me.divClientportal.Style.Add("display", "block")
                End If

                Me.lblIdBooking.Visible = False
                Me.lblEIdBooking.Visible = False

                dt = .Item(dsReservaciones.FIELD_CHECKIN)

                Dim aa = .Item("source").ToString.ToUpper
                If .Item("source").ToString.ToUpper = "CCT" AndAlso .Item("SourceCode").ToString.ToUpper = "WI" Then
                    If tipoUsuario <> 9 AndAlso tipoUsuario <> 10 AndAlso tipoUsuario <> 11 Then
                        Me.lblSource.Text = "Univisit Call Center"
                    Else
                        Me.lblSource.Text = .Item(dsReservaciones.FIELD_PortalName).ToString
                    End If

                    Me.divClientportal.Style.Add("display", "block")
                ElseIf .Item("source").ToString.ToUpper.Trim = "POR" AndAlso (.Item("sourceCode").ToString.ToUpper.Trim = "WL" OrElse .Item("sourceids").ToString.ToUpper.Trim = "WTL") Then
                    Me.lblSource.Text = PortalCulture.GetString("01445")
                    Me.divClientportal.Style.Add("display", "block")
                ElseIf .Item("source").ToString.ToUpper.Trim = "UNI" Then
                    Me.lblSource.Text = PortalCulture.GetString("00457")
                    Me.divClientportal.Style.Add("display", "block")
                ElseIf .Item("source").ToString.ToUpper.Trim = "HTL" Then
                    Me.lblSource.Text = PortalCulture.GetString("00650")
                    If Not .IsNull("agencyaPorRecepcion") Then
                        Dim strAgency As String
                        strAgency = "<table id='Table1' width='100%' align='center'>"
                        strAgency &= "      <tr>"
                        strAgency &= "<td vAlign='top'>"
                        strAgency &= "<span Class='bookingNormalLabel'>" & PortalCulture.GetString("00614") & "</span>"
                        strAgency &= "</td>"
                        strAgency &= "</tr>"
                        strAgency &= "<tr>"
                        strAgency &= "<td  vAlign='top'>"
                        strAgency &= "<p>" & .Item("agencyaPorRecepcion") & "</p>"
                        strAgency &= "</td>"
                        strAgency &= "</tr>"
                        strAgency &= "</table>"
                        Me.AgencyRecepcion.InnerHtml = strAgency
                        divClientportal.Style.Add("display", "block")
                    End If
                    hplModify.Visible = False
                    hplModify.NavigateUrl = GeRequestApplicationPath(String.Concat("/HotelAdministrator/Pages/ModifyPassiveReservation.aspx?NoReservation=", .Item(dsReservaciones.FIELD_NORESERVACION).ToString.Trim))
                    If GetIsPerWebSiteOriginal(dsReservaciones) Then
                        Me.lblSource.Text = "WebSite"
                        If GetIsPerWebSiteAgencyOriginal(dsReservaciones) Then
                            Me.lblIdBooking.Text = String.Format("({0}) - {1}", GetAgencyCodeOriginal(dsReservaciones), GetAgencyNameOriginal(dsReservaciones))
                            Me.lblIdBooking.Visible = True
                        End If
                    End If
                ElseIf .Item("source").ToString.ToUpper.Trim = "ADS" Then
                    Me.lblSource.Text = "ADS Pegasus"
                    Me.lblSource.Visible = True
                    Me.lblESource.Visible = True
                    Me.btnReactive.Visible = False
                    'Me.divClientportal.Style.Add("display", "")
                ElseIf .Item("source").ToString.ToUpper.Trim = "IDS" Then
                    Try
                        'Dim dsIDSB As DataSet = LoadIDSReservaConfirmNum(.Item(dsReservaciones.FIELD_NORESERVACION))

                        Me.lblSource.Text = .Item("sourceids").ToString.ToUpper.Trim
                        Me.lblSource.Visible = True
                        Me.lblESource.Visible = True
                        'If Not dsIDSB Is Nothing AndAlso dsIDSB.Tables.Count = 1 AndAlso dsIDSB.Tables(0).Rows.Count > 0 Then
                        '    Me.lblIdBooking.Text = dsIDSB.Tables(0).Rows(0)("IdBooking").ToString.ToUpper.Trim()
                        'Else
                        '    Me.lblIdBooking.Text = "-"
                        'End If
                        Me.lblIdBooking.Text = .Item("IdBooking").ToString.ToUpper.Trim
                        Me.lblIdBooking.Visible = True
                        Me.lblEIdBooking.Visible = True
                        Me.btnReactive.Visible = False

                    Catch ex As Exception
                        Me.lblSource.Text = .Item("source").ToString.Trim
                        Me.lblSource.Visible = True
                        Me.lblESource.Visible = True
                    End Try
                ElseIf Not String.IsNullOrEmpty(.Item("sourceids")) Then
                    Me.lblSource.Text = .Item("sourceids").ToString.ToUpper.Trim
                    Me.lblSource.Visible = True
                    Me.lblESource.Visible = True
                    Me.btnReactive.Visible = False
                ElseIf .Item("source").ToString.ToUpper.Trim = "POR" AndAlso .Item("SourceCode").ToString.ToUpper.Trim = "HB" AndAlso .Item("idHotel") = 0 Then
                    Me.lblSource.Text = "Hotel Best"
                End If

                If ((dt.Month >= Now.Month And dt.Year = Now.Year) Or (dt.Year > Now.Year)) Then
                Else
                    Me.btnReactive.Visible = False
                End If
                If .Item(dsReservaciones.FIELD_STATUS) <> 3 Then
                    Me.btnReactive.Visible = False
                End If

                Me.pnlConvenios.Visible = (.Item("NoConvenio").ToString.Trim().Length > 0)
                Me.pnlConvenios2.Visible = (.Item("NoConvenio").ToString.Trim().Length > 0)
                Me.lblNoConvenio.Text = .Item("NoConvenio").ToString()
                Me.lblEmpresaConvenio.Text = .Item("EmpresaConvenio").ToString()

                btnStatusPMS.Visible = False
                If (MyBase.IsSupervisor Or Me.isUserChain) AndAlso (Not .IsNull("EstatusPMS")) Then
                    btnStatusPMS.Visible = If(.Item("EstatusPMS") = 2, True, False)
                End If

            End With
            i = 1
            dgReservas.DataSource = dsReservaciones
            dgReservas.DataBind()
            Dim isShow As Boolean = cargausuarios()
            pnlTC.Visible = isShow


        End If
    End Function

    Private Sub dgReservas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(0).Text = i
            i += 1

            Dim lb As Label
            lb = e.Item.Cells(Columns.Preferencias).FindControl("lblPreferencia")


            If e.Item.Cells(Columns.BDPreferencias).Text <> "&nbsp;" Then
                Dim json As JObject
                Dim strPreferencia() As String
                Dim data As List(Of JToken)

                strPreferencia = e.Item.Cells(Columns.BDPreferencias).Text.Split(New String() {"|#UV#|"}, StringSplitOptions.None)
                If strPreferencia.Length > 1 Then
                    lb.Text = strPreferencia(0)
                    Try
                        json = JObject.Parse(strPreferencia(1))
                        data = json.Children().ToList
                        lb = e.Item.Cells(Columns.Preferencias).FindControl("Label3")
                        lb.Text = ""
                        For Each item As JProperty In data
                            item.CreateReader()
                            Select Case item.Name
                                Case "Room"
                                    If item.Value <> String.Empty Then
                                        lb = e.Item.Cells(Columns.tipoHabitacion).FindControl("lblTipoHab")
                                        lb.Text = item.Value
                                    End If
                                Case "RatePlan"
                                    If item.Value <> String.Empty Then
                                        lb = e.Item.Cells(Columns.codigotarifa).FindControl("Label14")
                                        lb.Text = item.Value
                                    End If
                            End Select
                        Next
                    Catch ex As Exception

                    End Try

                Else
                    lb.Text = e.Item.Cells(Columns.BDPreferencias).Text
                End If
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Me.dgReservas.Columns(Columns.Cuartos).HeaderText = ColumunsName(Columns.Cuartos)
            Me.dgReservas.Columns(Columns.Cliente).HeaderText = ColumunsName(Columns.Cliente)
            Me.dgReservas.Columns(Columns.Adultos).HeaderText = ColumunsName(Columns.Adultos)
            Me.dgReservas.Columns(Columns.tipoHabitacion).HeaderText = ColumunsName(Columns.tipoHabitacion)
            Me.dgReservas.Columns(Columns.Preferencias).HeaderText = ColumunsName(Columns.Preferencias)
        End If
    End Sub

    Private Sub hplCancelRes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hplCancelRes.Click
        If Not makecancel() Then
            Me.lblError.Text = PortalCulture.GetString("M000542")
            If Me.lblErrorMotivo.Text.Trim <> "" Then
                Me.lblErrorMotivo.Visible = True
            End If
            Me.lblError.Visible = True
        Else
            Me.lblError.Text = PortalCulture.GetString("M000543")
            Me.lblError.Visible = False
        End If

    End Sub

    Private Function MakeReactivation() As Boolean
        Dim sqlconn As New SqlConnection(AppSettings("HotelConnection"))
        Dim dsReservaciones As ReservaDatos
        Dim idRes As String = Request.QueryString("qs")
        Dim dr As DataRow
        Dim src As String
        Dim noreservacion As String
        Dim trans As Boolean
        Dim dt As Date

        trans = False
        sqlconn.Open()
        Dim transacc As SqlTransaction = sqlconn.BeginTransaction
        trans = True
        Try
            With New ReservaFacade
                dsReservaciones = .GetDataReserva(idRes)
            End With
            If dsEmpty(dsReservaciones) Then
                Return False
            End If

            dr = dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)
            src = dr("source").ToString.ToUpper.Trim
            dt = dr(dsReservaciones.FIELD_CHECKIN)
            noreservacion = dr(dsReservaciones.FIELD_NORESERVACION)

            Dim sqlcmd As New SqlCommand("spReservationReactive", sqlconn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Transaction = transacc
            'Reservacion
            sqlcmd.Parameters.Add("@NoReservacion", SqlDbType.NVarChar, 24).Value = noreservacion
            sqlcmd.Parameters.Add("@rm", SqlDbType.Bit).Value = 1
            Dim afec As Integer = sqlcmd.ExecuteNonQuery()
            transacc.Commit()
            trans = False
            If afec > 0 Then
                enviarcorreo(dsReservaciones, False)

                Dim NR As New WSHotelDataAccess.clsDANetRates
                'NR.InsertNetRateMail(dr(dsReservaciones.FIELD_NORESERVACION), WSHotelRules.clsRUCommon.eEmailTypeNetRate.Cancel, False, DateTime.Now)
                NR.InsertNetRateMail(dr(dsReservaciones.FIELD_NORESERVACION), eOperationPMS.Active, False, DateTime.Now)

                Dim PMS As New WSHotelRules.clsRUPMS
                PMS.ExecuteOperation(dr(dsReservaciones.FIELD_NORESERVACION), eOperationPMS.Active)

                MyBase.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx?qs=" & IdReservacion, PaginaBase.acciones.Modificar, "Se reactivo la reservacion  " & noreservacion)
                Return True
            Else
                Return False
            End If
            'sd
        Catch e As Exception
            If trans Then transacc.Rollback()
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            Return False
        Finally
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
        End Try
    End Function

    Private Function makecancel() As Boolean
        Dim dsReservaciones As ReservaDatos
        Dim idRes As String = Request.QueryString("qs")
        Dim GalileoConfCancelNumber As String = ""
        Dim NoCancelacion As String = ""
        Dim gUI As System.Globalization.CultureInfo
        gUI = Threading.Thread.CurrentThread.CurrentUICulture


        With New ReservaFacade
            dsReservaciones = .GetDataReserva(idRes)
        End With

        Dim docReq, docResp As XmlDocument

        'si existe en galileo hay que cancelarla alla
        If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows.Count > 0 Then
            With dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)

                If .Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString <> "" Then
                    Try
                        'traer la reserva del gds
                        docReq = BuildRequestGetResGDS(.Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString)
                        With New bkHotelesGWS.clsXMLRequest2
                            docResp = .GetReservaDataGDS(docReq)
                        End With
                        If Not docResp Is Nothing Then
                            If docResp.GetElementsByTagName("PhoneInfo").Count > 0 Or docResp.GetElementsByTagName("Control").Count > 0 Or docResp.GetElementsByTagName("GenPNRInfo").Count > 0 Or docResp.GetElementsByTagName("LNameInfo").Count > 0 Then
                                If docResp.GetElementsByTagName("HtlSeg").Count > 0 Then
                                    With New bkHotelesGWS.clsXMLRequest2
                                        GalileoConfCancelNumber = .CancelGalileo(dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).Item(dsReservaciones.FIELD_NoReservacionGalileo))
                                    End With
                                    If GalileoConfCancelNumber = "" Then
                                        Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
                                        Return False
                                    End If
                                End If
                            Else
                                Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
                                Return False
                            End If
                        Else
                            Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
                            Return False
                        End If
                    Catch ex As Exception
                        Me.lblErrorMotivo.Text = ""
                        Return False
                    End Try
                End If
                'si es reservacion que no est� con galileo hay que validar que cumpla con las reglas del minimo dia para cancelar
                If .Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString = "" Then
                    Dim dsHotel As HotelDatos
                    Dim dsEtiq As MonedaDatos
                    Dim idhotel As Integer = CInt(Val(dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_IDHOTEL)))
                    Dim source As String = dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_SOURCE).ToString
                    With New HotelSistema
                        dsHotel = .GetHotelById(idhotel) 'MyBase.cInfoActual.Hotel)
                    End With
                    If idhotel <> 0 Then
                        If Not MyBase.IsSupervisor AndAlso Not Me.isUserChain AndAlso source <> "HTL" Then
                            Dim minimo As Integer
                            minimo = CInt(Val(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_DIASMINCANCELAR).ToString))
                            If CDate(.Item(dsReservaciones.FIELD_CHECKIN)).Subtract(Now.Date).Days < minimo Then
                                'no se puede cancelar por el minimo de dias para cancelar salir de la funcion
                                Me.lblErrorMotivo.Text = PortalCulture.GetString("M000545")
                                Return False
                            End If
                        End If
                    End If

                End If
                Dim idReservacion As String = .Item(dsReservaciones.FIELD_IDRESERVACION)

                If LocalCancel(idReservacion, GalileoConfCancelNumber, CInt(Val(.Item("idUsuario").ToString)), NoCancelacion, idReservacion.ToString) Then

                    'Luis Cota 02/05/2017
                    'No enviar correo de cancelación si la reservación está en proceso
                    If dsReservaciones.Tables.Count > 0 AndAlso dsReservaciones.Tables(0).Rows.Count > 0 Then
                        If dsReservaciones.Tables(0).Rows(0).Item("Status") <> 4 Then
                            enviarcorreo(dsReservaciones)
                            'Correo NetRate
                            Dim NR As New WSHotelDataAccess.clsDANetRates
                            NR.InsertNetRateMail(idReservacion, WSHotelRules.clsRUCommon.eEmailTypeNetRate.Cancel, False, DateTime.Now)
                        End If
                    End If
                    'PMS
                    Dim PMS As New WSHotelRules.clsRUPMS
                    PMS.ExecuteOperation(idReservacion, eOperationPMS.Delete)

                    Threading.Thread.CurrentThread.CurrentUICulture = gUI
                    PortalCulture.SetCulture(gUI.ToString)
                    Me.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx", PaginaBase.acciones.Eliminar, "Cancel� la reservacion " & idReservacion)

                    MyBase.OTA_PushNotif(cInfoActual.Hotel)

                    If Not String.IsNullOrEmpty(AppSettings("ZunUrl")) Then
                        If dsReservaciones.Tables(0).Rows(0).Item("CubanTypesPms") = "ZUN" Then
                            With New WSHotelRules.clsRUZun
                                .clsRUZun(idReservacion, CurrencyConfirm, dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_SOURCE).ToString)
                                .sendReservation(WSHotelRules.ZunPSMws.Estados.eliminar)
                            End With
                        End If
                    End If

                    Return True
                Else
                    Threading.Thread.CurrentThread.CurrentUICulture = gUI
                    PortalCulture.SetCulture(gUI.ToString)
                    Return False
                End If
            End With
        End If

        Return False

    End Function

    Private Sub btnSendZun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendZun.Click
        Dim dsReservaciones As ReservaDatos
        Try
            Dim idRes As String = Request.QueryString("qs")
            With New ReservaFacade
                dsReservaciones = .GetDataReserva(idRes)
            End With

            If Not String.IsNullOrEmpty(AppSettings("ZunUrl")) Then
                If dsReservaciones.Tables(0).Rows(0).Item("CubanTypesPms") = "ZUN" AndAlso dsReservaciones.Tables(0).Rows(0).Item("pmsStatus") = 0 AndAlso dsReservaciones.Tables(0).Rows(0).Item("pmsAct") = "SS" Then
                    With New WSHotelRules.clsRUZun
                        .clsRUZun(IdReservacion, CurrencyConfirm, dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_SOURCE).ToString)
                        .sendReservation(WSHotelRules.ZunPSMws.Estados.nuevo)
                    End With
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub

    Private Sub btnMultiCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMultiCancel.Click
        Dim ids() As String = txtMultiCancel.Text.Split(",")

        For i As Integer = 0 To ids.Length - 1
            If Not makeMultiCancel(ids(i)) Then
                lblError.Text = "Fall�: " & ids(i)
                Return
            End If
        Next

    End Sub

    Private Function makeMultiCancel(ByVal idreservacion As Integer) As Boolean
        Dim dsReservaciones As ReservaDatos
        Dim idRes As String = idreservacion
        Dim GalileoConfCancelNumber As String = ""
        Dim NoCancelacion As String = ""
        Dim gUI As System.Globalization.CultureInfo
        gUI = Threading.Thread.CurrentThread.CurrentUICulture


        With New ReservaFacade
            dsReservaciones = .GetDataReserva(idRes)
        End With

        Dim docReq, docResp As XmlDocument

        'si existe en galileo hay que cancelarla alla
        If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows.Count > 0 Then
            With dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)

                If .Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString <> "" Then
                    Try
                        'traer la reserva del gds
                        docReq = BuildRequestGetResGDS(.Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString)
                        With New bkHotelesGWS.clsXMLRequest2
                            docResp = .GetReservaDataGDS(docReq)
                        End With
                        If Not docResp Is Nothing Then
                            If docResp.GetElementsByTagName("PhoneInfo").Count > 0 Or docResp.GetElementsByTagName("Control").Count > 0 Or docResp.GetElementsByTagName("GenPNRInfo").Count > 0 Or docResp.GetElementsByTagName("LNameInfo").Count > 0 Then
                                If docResp.GetElementsByTagName("HtlSeg").Count > 0 Then
                                    With New bkHotelesGWS.clsXMLRequest2
                                        GalileoConfCancelNumber = .CancelGalileo(dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).Item(dsReservaciones.FIELD_NoReservacionGalileo))
                                    End With
                                    If GalileoConfCancelNumber = "" Then
                                        Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
                                        Return False
                                    End If
                                End If
                            Else
                                Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
                                Return False
                            End If
                        Else
                            Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
                            Return False
                        End If
                    Catch ex As Exception
                        Me.lblErrorMotivo.Text = ""
                        Return False
                    End Try
                End If
                'si es reservacion que no est� con galileo hay que validar que cumpla con las reglas del minimo dia para cancelar
                If .Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString = "" Then
                    Dim dsHotel As HotelDatos
                    Dim dsEtiq As MonedaDatos
                    Dim idhotel As Integer = CInt(Val(dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_IDHOTEL)))
                    Dim source As String = dsReservaciones.Tables(0).Rows(0).Item(dsReservaciones.FIELD_SOURCE).ToString
                    With New HotelSistema
                        dsHotel = .GetHotelById(idhotel) 'MyBase.cInfoActual.Hotel)
                    End With
                    If idhotel <> 0 Then
                        If Not MyBase.IsSupervisor AndAlso Not Me.isUserChain AndAlso source <> "HTL" Then
                            Dim minimo As Integer
                            minimo = CInt(Val(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0).Item(dsHotel.FIELD_DIASMINCANCELAR).ToString))
                            If CDate(.Item(dsReservaciones.FIELD_CHECKIN)).Subtract(Now.Date).Days < minimo Then
                                'no se puede cancelar por el minimo de dias para cancelar salir de la funcion
                                Me.lblErrorMotivo.Text = PortalCulture.GetString("M000545")
                                Return False
                            End If
                        End If
                    End If

                End If
                txtMotivo.Text = "Reservaci�n Erronea"
                If LocalCancel(.Item(dsReservaciones.FIELD_IDRESERVACION), GalileoConfCancelNumber, CInt(Val(.Item("idUsuario").ToString)), NoCancelacion, .Item(dsReservaciones.FIELD_NORESERVACION)) Then
                    enviarcorreo(dsReservaciones)
                    'Correo NetRate
                    Dim NR As New WSHotelDataAccess.clsDANetRates
                    NR.InsertNetRateMail(.Item(dsReservaciones.FIELD_NORESERVACION), WSHotelRules.clsRUCommon.eEmailTypeNetRate.Cancel, False, DateTime.Now)
                    'PMS
                    Dim PMS As New WSHotelRules.clsRUPMS
                    PMS.ExecuteOperation(.Item(dsReservaciones.FIELD_NORESERVACION), eOperationPMS.Delete)

                    Threading.Thread.CurrentThread.CurrentUICulture = gUI
                    PortalCulture.SetCulture(gUI.ToString)
                    Me.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx", PaginaBase.acciones.Eliminar, "Cancel� la reservacion " & .Item(dsReservaciones.FIELD_NORESERVACION).ToString)
                    MyBase.OTA_PushNotif(cInfoActual.Hotel)
                    Return True
                Else
                    Threading.Thread.CurrentThread.CurrentUICulture = gUI
                    PortalCulture.SetCulture(gUI.ToString)
                    Return False
                End If
            End With
        End If

        Return False

    End Function

    Sub CancelInPMS()

    End Sub
    'Private Function makecancel() As Boolean
    '    Dim dsReservaciones As ReservaDatos
    '    Dim idRes As String = Request.QueryString("qs")
    '    Dim GalileoConfCancelNumber As String = ""
    '    Dim NoCancelacion As String = ""


    '    With New ReservaFacade
    '        dsReservaciones = .GetDataReserva(idRes)
    '    End With

    '    Dim docReq, docResp As XmlDocument

    '    'si existe en galileo hay que cancelarla alla
    '    If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows.Count > 0 Then
    '        With dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)

    '            If .Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString <> "" Then
    '                Try
    '                    'traer la reserva del gds
    '                    docReq = BuildRequestGetResGDS(.Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString)
    '                    With New bkHotelesGWS.clsXMLRequest2
    '                        docResp = .GetReservaDataGDS(docReq)
    '                    End With
    '                    If Not docResp Is Nothing Then
    '                        If docResp.GetElementsByTagName("PhoneInfo").Count > 0 Or docResp.GetElementsByTagName("Control").Count > 0 Or docResp.GetElementsByTagName("GenPNRInfo").Count > 0 Or docResp.GetElementsByTagName("LNameInfo").Count > 0 Then
    '                            If docResp.GetElementsByTagName("HtlSeg").Count > 0 Then
    '                                With New bkHotelesGWS.clsXMLRequest2
    '                                    GalileoConfCancelNumber = .CancelGalileo(dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0).Item(dsReservaciones.FIELD_NoReservacionGalileo))
    '                                End With
    '                                If GalileoConfCancelNumber = "" Then
    '                                    Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
    '                                    Return False
    '                                End If
    '                            End If
    '                        Else
    '                            Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
    '                            Return False
    '                        End If
    '                    Else
    '                        Me.lblErrorMotivo.Text = PortalCulture.GetString("M000544")
    '                        Return False
    '                    End If
    '                Catch ex As Exception
    '                    Me.lblErrorMotivo.Text = ""
    '                    Return False
    '                End Try
    '            End If
    '            'si es reservacion que no est� con galileo hay que validar que cumpla con las reglas del minimo dia para cancelar
    '            If .Item(dsReservaciones.FIELD_NoReservacionGalileo).ToString = "" Then
    '                'aki hay ke negar la condici�n 
    '                If MyBase.IsSupervisor Then
    '                    Dim xml As resHotelCancel
    '                    xml = wsMakeCancel(.Item(dsReservaciones.FIELD_NORESERVACION).ToString, .Item(dsReservaciones.FIELD_IDHOTEL).ToString, "UV", .Item("source").ToString)
    '                    If xml.HotelCancel.Rows.Count > 0 Then
    '                        '[spReservationGetByNo]
    '                        enviarcorreo(dsReservaciones)
    '                        Me.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx", PaginaBase.acciones.Eliminar, "Cancel� la reservacion " & .Item(dsReservaciones.FIELD_NORESERVACION).ToString)
    '                        Return True
    '                    Else
    '                        Me.lblErrorMotivo.Text = xml._Error.Rows(0).Item("Message")
    '                        Return False
    '                    End If
    '                End If
    '            End If
    '            If LocalCancel(.Item(dsReservaciones.FIELD_IDRESERVACION), GalileoConfCancelNumber, CInt(Val(.Item("idUsuario").ToString)), NoCancelacion) Then
    '                enviarcorreo(dsReservaciones)
    '                Me.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx", PaginaBase.acciones.Eliminar, "Cancel� la reservacion " & .Item(dsReservaciones.FIELD_NORESERVACION).ToString)
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End With
    '    End If
    '    Return False
    'End Function
    Private Function wsMakeCancel(ByVal NoConf As String, ByVal Hotel As Integer, ByVal chain As String, ByVal source As String) As resHotelCancel
        Dim xdoc As New XmlDataDocument(New reqHotelCancel)
        Dim dsreq As reqHotelCancel = CType(xdoc.DataSet, reqHotelCancel)
        Dim drC As reqHotelCancel.HotelCancelRow
        drC = dsreq.HotelCancel.NewHotelCancelRow
        drC.ChainCode = chain
        drC.ConfirmNumber = NoConf
        If Hotel = 0 Then
            'si es de gds
            'drc.CxNumber='reclog
        End If

        drC.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
        drC.PropertyNumber = Hotel
        drC.Source = source
        dsreq.HotelCancel.AddHotelCancelRow(drC)
        With New WSHotelFacade.clsFACancel
            Return .GetHotelCancel(xdoc.DocumentElement)
        End With
    End Function

    Private Function LocalStatusPMS() As Boolean
        Dim afec As Integer
        Dim trans As Boolean
        trans = False
        Dim sqlconn As New SqlConnection(AppSettings("HotelConnection"))
        sqlconn.Open()
        Dim transacc As SqlTransaction = sqlconn.BeginTransaction
        trans = True
        Try
            Dim sqlcmd As New SqlCommand("spPendientesPMSUpdateStatus", sqlconn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Transaction = transacc
            sqlcmd.Parameters.Add("@noreservacion", SqlDbType.NVarChar, 50).Value = NoReserv

            afec = sqlcmd.ExecuteNonQuery()
            transacc.Commit()
            trans = False
            If afec > 0 Then
                MyBase.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx?qs=" & IdReservacion, PaginaBase.acciones.Modificar, "Cambio status PMS, no reservacion   " & NoReserv)
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            If trans Then transacc.Rollback()
        Finally
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
        End Try
    End Function

    Private Function LocalCancel(ByVal idReservacion As Long, ByVal ConfNumCancel As String, ByVal IDUser As Integer, ByRef refNoCancelacion As String, ByVal noreservacion As String) As Boolean
        Dim trans As Boolean
        trans = False
        Dim sqlconn As New SqlConnection(AppSettings("HotelConnection"))
        sqlconn.Open()
        Dim transacc As SqlTransaction = sqlconn.BeginTransaction
        trans = True
        Try
            Dim sqlcmd As New SqlCommand("spReservationCancel", sqlconn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Transaction = transacc
            'Reservacion
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idReservacion

            '//generar el numero de cancelacion @NoCancelacion
            CancelNumber = "CX" & Format(Now(), "yy") & Format(Now(), "MM") & Format(Now(), "dd") & Format(Now(), "HH") & Format(Now(), "mm") & Format(Now(), "ss") & IDUser
            refNoCancelacion = CancelNumber
            sqlcmd.Parameters.Add("@NoCancelacion", SqlDbType.NVarChar, 50).Value = CancelNumber
            If txtMotivo.Text.Trim = "" Then
                txtMotivo.Text = PortalCulture.GetString("M000604")
            End If
            Dim msg As String
            msg = PortalCulture.GetString("00780", True) & ReadUserCookie.GetValue(0)
            Try
                Dim len = 250 - txtMotivo.Text.Length - msg.Length - 5
                If (len < 0) Then
                    len = txtMotivo.Text.Length - Math.Abs(len)
                    If txtMotivo.Text.Length > len Then
                        txtMotivo.Text = txtMotivo.Text.Substring(0, len)
                    End If
                End If
            Catch ex As Exception
            End Try
            sqlcmd.Parameters.Add("@MotivoCancelacion", SqlDbType.NVarChar, 250).Value = txtMotivo.Text  '& ". " & PortalCulture.GetString("00780", True) & ReadUserCookie.GetValue(0)
            If ConfNumCancel.Trim <> "" Then sqlcmd.Parameters.Add("@NoConfCancelGalileo", SqlDbType.NVarChar, 50).Value = ConfNumCancel
            sqlcmd.Parameters.Add("@iduser", SqlDbType.Int).Value = MyBase.UserIdentityName

            Dim afec As Integer = sqlcmd.ExecuteNonQuery()
            transacc.Commit()
            trans = False
            If afec > 0 Then
                MyBase.guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx?qs=" & idReservacion, PaginaBase.acciones.Eliminar, "Cancelaron la reservacion  " & noreservacion)
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            If trans Then transacc.Rollback()
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            Return False
        Finally
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
        End Try
    End Function

    Private Function BuildRequestGetResGDS(ByVal RecLoc As String) As XmlDocument
        Dim doc As XmlDocument = New XmlDocument
        'Build the request
        Dim req As StringBuilder = New StringBuilder
        req.Append("<PNRBFManagement_7_11>" + vbCrLf)
        req.Append("<PNRBFRetrieveMods>" + vbCrLf)
        req.Append("<PNRAddr>" + vbCrLf)
        req.Append("<FileAddr>" + vbCrLf)
        req.Append("</FileAddr>" + vbCrLf)
        req.Append("<CodeCheck>" + vbCrLf)
        req.Append("</CodeCheck>" + vbCrLf)
        req.Append("<RecLoc>" + RecLoc + "</RecLoc>" + vbCrLf)
        req.Append("</PNRAddr>" + vbCrLf)
        req.Append("</PNRBFRetrieveMods>" + vbCrLf)
        req.Append("</PNRBFManagement_7_11>" + vbCrLf)
        doc.LoadXml(req.ToString)
        Return doc
    End Function

    Private Sub enviarcorreo(ByVal dsreservaciones As ReservaDatos, Optional ByVal isCancel As Boolean = True, Optional ByVal isModify As Boolean = False)
        Dim emailHotel As String
        Dim idioma As String
        With dsreservaciones.Tables(dsreservaciones.RESERVA_TABLE).Rows(0)
            If .Item(dsreservaciones.FIELD_IDHOTEL) <> 0 Then
                Dim hotel As HotelDatos
                With New HotelSistema
                    hotel = .GetHotelById(dsreservaciones.Tables(dsreservaciones.RESERVA_TABLE).Rows(0).Item(dsreservaciones.FIELD_IDHOTEL))
                End With
                If hotel.Tables.Count > 0 AndAlso hotel.Tables(hotel.HOTEL_TABLE).Rows.Count > 0 AndAlso hotel.Tables(hotel.HOTEL_TABLE).Rows(0).Item(hotel.FIELD_EMAIL_RESERVAS) <> "" Then
                    emailHotel = hotel.Tables(hotel.HOTEL_TABLE).Rows(0).Item(hotel.FIELD_EMAIL_RESERVAS)
                    idioma = hotel.Tables(hotel.HOTEL_TABLE).Rows(0).Item(hotel.FIELD_IDIOMAEMAIL).ToString
                End If
            End If
            If emailHotel <> "" Then
                If isCancel Then
                    Fillcorreo(dsreservaciones, idioma, emailHotel, "", "")
                ElseIf isModify Then
                    FillcorreoReservaModificada(dsreservaciones, idioma, emailHotel, "", "")
                Else

                    FillcorreoReservaReactivada(dsreservaciones, idioma, emailHotel, "", "")
                End If
            End If

            If .Item(dsreservaciones.FIELD_EMAILCL).ToString <> "" Then
                If isCancel Then
                    Fillcorreo(dsreservaciones, idioma, .Item(dsreservaciones.FIELD_EMAILCL), "", AppSettings("UnivisitMail"))
                ElseIf isModify Then
                    FillcorreoReservaModificada(dsreservaciones, idioma, .Item(dsreservaciones.FIELD_EMAILCL), "", "")
                Else
                    FillcorreoReservaReactivada(dsreservaciones, idioma, .Item(dsreservaciones.FIELD_EMAILCL), "", AppSettings("UnivisitMail"))
                End If
            End If
            If emailHotel = "" AndAlso .Item(dsreservaciones.FIELD_EMAILCL).ToString = "" Then
                If isCancel Then
                    Fillcorreo(dsreservaciones, idioma, AppSettings("UnivisitMail"), "", "")
                ElseIf isModify Then
                    FillcorreoReservaModificada(dsreservaciones, idioma, AppSettings("UnivisitMail"), "", "")
                Else
                    FillcorreoReservaReactivada(dsreservaciones, idioma, AppSettings("UnivisitMail"), "", "")
                End If
            End If
        End With
    End Sub

    Private Sub Fillcorreo(ByVal dsreservaciones As ReservaDatos, ByVal idioma As String, ByVal _to As String, ByVal _cc As String, ByVal _bcc As String)
        Dim ci As System.Globalization.CultureInfo

        Dim Mail As emailTemplates.Template

        Try
            With dsreservaciones.Tables(dsreservaciones.RESERVA_TABLE).Rows(0)
                If idioma = "" Then
                    idioma = PortalCulture.GetCulture.ToString
                End If

                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(idioma)
                PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)

                Mail = New emailTemplates.Template
                If SourceReserv = "UNI" Or SourceReserv = "IDS" Or SourceReserv = "ADS" Or SourceReserv = "CCT" Then
                    Mail.TemplateName = "T12_HOTELCANCELLATION_LOGO"
                Else
                    Mail.TemplateName = "T12_HOTELCANCELLATION"
                End If
                Mail.To = _to
                If Not String.IsNullOrEmpty(_cc) Then Mail.Cc = _cc
                If Not String.IsNullOrEmpty(_bcc) Then Mail.Bcc = _bcc
                Mail.SubjectParam = .Item(dsreservaciones.FIELD_NORESERVACION)

                Mail.Html = True
                Mail.SubjectParam = .Item(dsreservaciones.FIELD_NORESERVACION)
                Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Mail.AddParameter("UNIVISITPORTAL") = PortalCulture.GetString("00516")

                If SourceReserv = "UNI" Or SourceReserv = "IDS" Or SourceReserv = "ADS" Or SourceReserv = "CCT" Then
                    Mail.AddParameter("PROPERTY_URLLOGO") = Util.Utility.LoadImagen(cInfoActual.Empresa)
                    Mail.AddParameter("PROPERTY_NAME") = cInfoActual.HotelName
                End If

                If txtMotivo.Text <> "" Then
                    Mail.AddParameter("MOTIVO") = txtMotivo.Text
                Else
                    Mail.AddParameter("MOTIVO") = PortalCulture.GetString("M000604")
                End If

                If .Item(dsreservaciones.FIELD_IDHOTEL) = "0" Then
                    Mail.AddParameter("HOTELNAME") = IIf(.IsNull(dsreservaciones.FIELD_HOTELGNOMBRE), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGNOMBRE))
                    Mail.AddParameter("CITY") = IIf(.IsNull(dsreservaciones.FIELD_HOTELGCIUDAD), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGCIUDAD))
                Else
                    Mail.AddParameter("HOTELNAME") = .Item(dsreservaciones.FIELD_HOTELNOMBRE)
                    Dim ciudad As String = .Item(dsreservaciones.FIELD_HOTELCIUDAD) & ", " & .Item(dsreservaciones.FIELD_HOTELESTADO)
                    Mail.AddParameter("CITY") = ciudad
                End If


                Mail.AddParameter("RESERVATIONNUMBER") = .Item(dsreservaciones.FIELD_NORESERVACION)
                Mail.AddParameter("CANCELLATIONNUMBER") = CancelNumber
                Mail.AddParameter("STARTDATE") = CDate(.Item(dsreservaciones.FIELD_CHECKIN)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("ENDDATE") = CDate(.Item(dsreservaciones.FIELD_CHECKOUT)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CUSTOMERNAME") = .Item(dsreservaciones.FIELD_CLIENTE)
                Mail.AddParameter("REGDATE") = CDate(.Item("FechaReservacion")).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CANCELLEDDATE") = Now.Date.ToString("dd/MMM/yyyy")
                Mail.AddParameter("DetCuartos") = GetTable(dsreservaciones)
                If .Item("IsNetRateUv") Then
                    Mail.AddParameter("UVNRPOLICIES") = CargaPoliticasUvNetRates()
                Else
                    Mail.AddParameter("UVNRPOLICIES") = ""
                End If
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
                Mail.Send()

                'Util.Utility.MailerSend(.Item(dsreservaciones.FIELD_NORESERVACION), Mail.GetBody)

            End With
        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
        End Try
    End Sub


    Private Sub FillcorreoReservaReactivada(ByVal dsreservaciones As ReservaDatos, ByVal idioma As String, ByVal _to As String, ByVal _cc As String, ByVal _bcc As String)
        Dim ci As System.Globalization.CultureInfo

        Dim Mail As emailTemplates.Template

        Try
            With dsreservaciones.Tables(dsreservaciones.RESERVA_TABLE).Rows(0)
                If idioma = "" Then
                    idioma = PortalCulture.GetCulture.ToString
                End If

                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(idioma)
                PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)

                Mail = New emailTemplates.Template
                If SourceReserv = "UNI" Or SourceReserv = "IDS" Or SourceReserv = "ADS" Or SourceReserv = "CCT" Then
                    Mail.TemplateName = "TH_HOTELREACTIVE_LOGO"
                Else
                    Mail.TemplateName = "TH_HOTELREACTIVE"
                End If

                Mail.To = _to
                If Not String.IsNullOrEmpty(_cc) Then Mail.Cc = _cc
                If Not String.IsNullOrEmpty(_bcc) Then Mail.Bcc = _bcc
                Mail.SubjectParam = .Item(dsreservaciones.FIELD_NORESERVACION)


                Mail.Html = True
                Mail.SubjectParam = .Item(dsreservaciones.FIELD_NORESERVACION)
                Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Mail.AddParameter("UNIVISITPORTAL") = PortalCulture.GetString("00516")

                If SourceReserv = "UNI" Or SourceReserv = "IDS" Or SourceReserv = "ADS" Or SourceReserv = "CCT" Then
                    Mail.AddParameter("PROPERTY_URLLOGO") = Util.Utility.LoadImagen(cInfoActual.Empresa)
                    Mail.AddParameter("PROPERTY_NAME") = cInfoActual.HotelName
                End If

                If .Item(dsreservaciones.FIELD_IDHOTEL) = "0" Then
                    Mail.AddParameter("HOTELNAME") = IIf(.IsNull(dsreservaciones.FIELD_HOTELGNOMBRE), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGNOMBRE))
                    Mail.AddParameter("CITY") = IIf(.IsNull(dsreservaciones.FIELD_HOTELGCIUDAD), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGCIUDAD))
                Else
                    Mail.AddParameter("HOTELNAME") = .Item(dsreservaciones.FIELD_HOTELNOMBRE)
                    Dim ciudad As String = .Item(dsreservaciones.FIELD_HOTELCIUDAD) & ", " & .Item(dsreservaciones.FIELD_HOTELESTADO)
                    Mail.AddParameter("CITY") = ciudad
                End If


                Mail.AddParameter("RESERVATIONNUMBER") = .Item(dsreservaciones.FIELD_NORESERVACION)
                Mail.AddParameter("STARTDATE") = CDate(.Item(dsreservaciones.FIELD_CHECKIN)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("ENDDATE") = CDate(.Item(dsreservaciones.FIELD_CHECKOUT)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CUSTOMERNAME") = .Item(dsreservaciones.FIELD_CLIENTE)
                Mail.AddParameter("REGDATE") = CDate(.Item("FechaReservacion")).ToString("dd/MMM/yyyy")
                Mail.AddParameter("DetCuartos") = GetTable(dsreservaciones)
                If .Item("IsNetRateUv") Then
                    Mail.AddParameter("UVNRPOLICIES") = CargaPoliticasUvNetRates()
                Else
                    Mail.AddParameter("UVNRPOLICIES") = ""
                End If

                Mail.Send()
                'Util.Utility.MailerSend(.Item(dsreservaciones.FIELD_NORESERVACION), Mail.GetBody)


            End With
        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
        End Try
    End Sub

    Public Function GetTable(ByVal dsreservaciones As ReservaDatos) As String
        Dim table As New StringBuilder
        Dim br = ControlChars.CrLf
        table.Append("<TABLE cellSpacing=""0"" cellPadding=""0"" width=""270px"" border=""0"">")
        table.Append(" <TR>")
        table.Append("   <TD>")
        For i As Integer = 0 To dsreservaciones.Tables(0).Rows.Count - 1
            table.Append("<TABLE cellSpacing=""0"" cellPadding=""0"" width=""270px"" border=""0"">")
            table.Append(" <TR>" & br)
            table.Append("     <TD width=""50%"" class=""txtEtiqueta"" align=""right""><span>" & PortalCulture.GetString("M000066"))
            table.Append("</span>     </TD >")
            table.Append("     <TD width=""50%"" >: <span class=""txtdato"">" & dsreservaciones.Tables(0).Rows(i).Item("NombreHabitacion"))
            table.Append("</span>     </TD>")
            table.Append(" </TR>" & br)
            table.Append(" <TR>" & br)
            table.Append("     <TD width=""50%""  class=""txtEtiqueta"" align=""right""><span>" & PortalCulture.GetString("M000585"))
            table.Append("</span></TD>")
            table.Append("     <TD width=""50%"" >: <span class=""txtdato"">" & dsreservaciones.Tables(0).Rows(i).Item("viajero"))
            table.Append("</span>     </TD>")
            table.Append(" </TR>" & br)
            table.Append("</TABLE>" & br)
        Next
        table.Append("   </TD>" & br)
        table.Append("  </TR>" & br)
        table.Append("</TABLE>" & br)
        Return table.ToString
    End Function

    Private Function GetIdCorporativoPortal(ByVal dsReservaciones As ReservaDatos) As Integer
        Try
            If Not dsReservaciones Is Nothing Then
                Return CType(dsReservaciones.Tables(0).Rows(0).Item("IdCorporativoPortal"), Integer)
            End If
        Catch
        End Try
        Return -1
    End Function

    Private Sub SendMailDepositConfirm(ByVal idRes As Integer)
        Try
            Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
            Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
            Dim xml As resHotelDisplay
            Dim dsReservaciones As ReservaDatos
            Dim drR As reqHotelDisplay.HotelDisplayRow
            Dim drRes As DataRow
            Dim nores As String = ""

            With New ReservaFacade
                dsReservaciones = .GetDataReserva(idRes, PortalCulture.GetIDCulture())
            End With
            If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows.Count > 0 Then
                drRes = dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)
                nores = drRes(dsReservaciones.FIELD_NORESERVACION)

                drR = dsreq.HotelDisplay.NewHotelDisplayRow
                drR.ConfirmNumber = nores
                drR.Language = "en-US"
                dsreq.HotelDisplay.AddHotelDisplayRow(drR)

                With New WSHotelFacade.clsFADisplay
                    xml = .GetHotelDisplay(xdoc.DocumentElement)
                End With

                If Not xml Is Nothing AndAlso xml.Reservation.Rows.Count > 0 Then
                    Dim idioma As String = PortalCulture.GetCulture.ToString
                    If Not xml.Reservation(0).IsNull("IdIdiomaReservation") Then
                        If xml.Reservation(0).IdIdiomaReservation = 2 Then
                            idioma = "en-US"
                        Else
                            idioma = "es-MX"
                        End If
                    End If
                    With New Miscelaneos.SendHotelEmails
                        .sendCustomerEmailReservation(xml, idioma)
                        .SendEmailtoAlHotel(xml, idioma)
                    End With
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnPayConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPayConfirm.Click
        Dim hr As Boolean = False
        If Me.CanShowConfirmButton Then
            Try
                With New SqlCommand("spPagosReservacionesInsert", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@idReservacion", SqlDbType.Int).Value = Me.IdReservacion
                    .Parameters.Add("@Status", SqlDbType.TinyInt).Value = 1
                    .Parameters.Add("@NoAutorizacion", SqlDbType.NVarChar, 50).Value = Me.txtPayAutorization.Text
                    .Parameters.Add("@PaymentSource", SqlDbType.Int).Value = 0 ' Me.lstPayMode.SelectedItem.Value
                    Try
                        .Connection.Open()
                        .ExecuteNonQuery()
                        hr = True
                    Catch ex As Exception
                    Finally
                        .Connection.Close()
                    End Try

                    If hr Then
                        SendMailDepositConfirm(Me.IdReservacion)
                    End If
                End With
            Catch ex As Exception
            End Try
        Else
        End If
    End Sub

    Public Property CanShowConfirmButton() As Boolean
        Get
            Dim flag As Boolean = False
            If Me.ViewState("CanShowConfirmButton") IsNot Nothing Then flag = Me.ViewState("CanShowConfirmButton")
            Return flag
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CanShowConfirmButton") = value
        End Set
    End Property

    Public Property IdReservacion() As Integer
        Get
            Dim temp As String = String.Empty
            If Me.ViewState("IdReservacion") IsNot Nothing Then temp = Me.ViewState("IdReservacion")
            Return temp
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdReservacion") = value
        End Set
    End Property

    Protected Sub btnReactive_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReactive.Click
        MakeReactivation()
    End Sub

#Region "Original Booking"


    Private Function GetIsPerWebSiteOriginal(ByVal dsReservaciones As ReservaDatos) As Boolean
        Try
            If Not dsReservaciones Is Nothing Then
                Return CType(dsReservaciones.Tables(0).Rows(0).Item("IsPerWebSiteOriginal"), Boolean)
            End If
        Catch
        End Try
        Return False
    End Function

    Private Function GetIsPerWebSiteAgencyOriginal(ByVal dsReservaciones As ReservaDatos) As Boolean
        Try
            If GetIsPerWebSiteOriginal(dsReservaciones) Then
                If dsReservaciones.Tables(0).Rows(0).Item("ReservationTypeOriginal") = 2 Then
                    Return True
                End If
            End If
        Catch
        End Try
        Return False
    End Function

    Private Function GetAgencyNameOriginal(ByVal dsReservaciones As ReservaDatos) As String
        Try
            If GetIsPerWebSiteAgencyOriginal(dsReservaciones) Then
                Return dsReservaciones.Tables(0).Rows(0).Item("AgencyNameOriginal")
            End If
        Catch
        End Try
        Return ""
    End Function
    Private Function GetAgencyCodeOriginal(ByVal dsReservaciones As ReservaDatos) As String
        Try
            If GetIsPerWebSiteAgencyOriginal(dsReservaciones) Then
                Return dsReservaciones.Tables(0).Rows(0).Item("AgencyCodeOriginal")
            End If
        Catch
        End Try
        Return ""
    End Function
    Private Function GetAgentNameOriginal(ByVal dsReservaciones As ReservaDatos) As String
        Try
            If GetIsPerWebSiteAgencyOriginal(dsReservaciones) Then
                Return dsReservaciones.Tables(0).Rows(0).Item("AgentCodeOriginal")
            End If
        Catch
        End Try
        Return ""
    End Function
#End Region

    Protected Sub btnStatusPMS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStatusPMS.Click
        LocalStatusPMS()
    End Sub

    Private Sub lnkShowCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowCC.Click
        If Not lblCCExp.Visible Then
            Dim nota As String = "Se desplego los datos de la tarjeta de credito, de la reservacion " & NoReserv
            CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/Pages/ReservationDetails.aspx?qs=" & Request.QueryString("qs"), PaginaBase.acciones.Ver, nota, String.Empty, String.Empty, String.Empty)
        End If

        lblCCType.Visible = Not lblCCExp.Visible
        lblCCName.Visible = Not lblCCExp.Visible
        lblCCcvNumber.Visible = Not lblCCExp.Visible
        lblCCExpDate.Visible = Not lblCCExp.Visible

        lblCCTipo.Visible = Not lblCCExp.Visible
        lblCCNa.Visible = Not lblCCExp.Visible
        lblCCcv.Visible = Not lblCCExp.Visible
        lblCCExp.Visible = Not lblCCExp.Visible




    End Sub

    Private Sub hplSaveConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hplSaveConfirm.Click
        Dim success As Boolean = False
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        Dim tr As SqlTransaction
        Dim sqlconn As SqlConnection = New SqlConnection(AppSettings("HotelConnectionString"))

        sqlconn.Open()
        tr = sqlconn.BeginTransaction
        With dsCommand
            Try
                Dim insertCommand As SqlCommand
                insertCommand = New SqlCommand("spPagosReservacionesInsert", sqlconn)
                insertCommand.CommandType = CommandType.StoredProcedure

                With insertCommand
                    .Transaction = tr
                    .Parameters.Add(New SqlParameter("@idReservacion", SqlDbType.Int, 15))
                    .Parameters.Add(New SqlParameter("@NoAutorizacion", SqlDbType.NVarChar, 80))
                    .Parameters.Add(New SqlParameter("@paymentSource", SqlDbType.Int, 80))
                    .Parameters.Add(New SqlParameter("@Referencia", SqlDbType.NVarChar, 50))
                    .Parameters.Add(New SqlParameter("@Monto", SqlDbType.Money))
                    .Parameters.Add(New SqlParameter("@Moneda", SqlDbType.NVarChar, 6))
                    .Parameters.Add(New SqlParameter("@Status", SqlDbType.Int))

                    .Parameters("@idReservacion").Value = IdReservacion
                    .Parameters("@NoAutorizacion").Value = txtConfirmNumber.Text
                    .Parameters("@paymentSource").Value = 0
                    .Parameters("@Referencia").Value = IdReservacion
                    .Parameters("@Monto").Value = TotalConfirm
                    .Parameters("@Moneda").Value = CurrencyConfirm
                    .Parameters("@Status").Value = 1

                    .ExecuteNonQuery()
                    tr.Commit()
                End With
            Catch ex As Exception
                tr.Rollback()
                sqlconn.Close()
            Finally
                If sqlconn.State = ConnectionState.Open Or sqlconn.State = ConnectionState.Open Then
                    sqlconn.Close()
                End If
            End Try
            btnResConfirm.Visible = False
        End With
    End Sub


    Private Sub hplModifyRes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hplModifyRes.Click

        Dim success As Boolean = False
        Dim dsCommand As SqlDataAdapter = New SqlDataAdapter
        Dim tr As SqlTransaction
        Dim sqlconn As SqlConnection = New SqlConnection(AppSettings("HotelConnectionString"))

        sqlconn.Open()
        tr = sqlconn.BeginTransaction
        With dsCommand
            Try
                .InsertCommand = GetInsertCommand(sqlconn)
                .InsertCommand.Transaction = tr
                .InsertCommand.Parameters.AddWithValue("@idReservacion", CInt(Request.QueryString("qs")))
                .InsertCommand.Parameters.AddWithValue("@NombreCL", txtNombreCL.Text)
                .InsertCommand.Parameters.AddWithValue("@ApellidoCL", txtApelldoCL.Text)
                .InsertCommand.Parameters.AddWithValue("@Total", CDbl(txtTotalModify.Text))
                .InsertCommand.Parameters.AddWithValue("@TotalNR", CDbl(txtTotalNRModify.Text))
                .InsertCommand.Parameters.AddWithValue("@CheckIn", Convert.ToDateTime(txtCheckIn.Text))
                .InsertCommand.Parameters.AddWithValue("@CheckOut", Convert.ToDateTime(txtCheckOut.Text))
                .InsertCommand.ExecuteNonQuery()
                tr.Commit()
                success = True
            Catch ex As Exception
                tr.Rollback()
                sqlconn.Close()
            Finally
                If sqlconn.State = ConnectionState.Open Or sqlconn.State = ConnectionState.Open Then
                    sqlconn.Close()
                End If
            End Try

            If success Then
                Dim reservaDatos As ReservaDatos
                With (New ReservaFacade)
                    reservaDatos = .GetDataReserva(CInt(Request.QueryString("qs")))
                End With

                enviarcorreo(reservaDatos, False, True)
                MyBase.OTA_PushNotif(cInfoActual.Hotel)
            End If
        End With
    End Sub

    Private Function GetInsertCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim insertCommand As SqlCommand
        insertCommand = New SqlCommand("spModificarReservacionByid", sqlconn)
        insertCommand.CommandType = CommandType.StoredProcedure

        With insertCommand.Parameters
            '.Add(New SqlParameter("@idReservacion", SqlDbType.Int, 15))
            '.Add(New SqlParameter("@NombreCL", SqlDbType.NVarChar, 80))
            '.Add(New SqlParameter("@ApellidoCL", SqlDbType.NVarChar, 80))
            '.Add(New SqlParameter("@Total", SqlDbType.Money))
            '.Add(New SqlParameter("@TotalNR", SqlDbType.Money))
            '.Add(New SqlParameter("@CheckIn", SqlDbType.SmallDateTime))
            '.Add(New SqlParameter("@CheckOut", SqlDbType.SmallDateTime))
        End With
        GetInsertCommand = insertCommand
    End Function

    Private Sub FillcorreoReservaModificada(ByVal dsreservaciones As ReservaDatos, ByVal idioma As String, ByVal _to As String, ByVal _cc As String, ByVal _bcc As String)
        Dim ci As System.Globalization.CultureInfo

        Dim Mail As emailTemplates.Template

        Try
            With dsreservaciones.Tables(dsreservaciones.RESERVA_TABLE).Rows(0)
                If idioma = "" Then
                    idioma = PortalCulture.GetCulture.ToString
                End If

                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(idioma)
                PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)

                Mail = New emailTemplates.Template
                If SourceReserv = "UNI" Or SourceReserv = "IDS" Or SourceReserv = "ADS" Or SourceReserv = "CCT" Then
                    Mail.TemplateName = "T16_HOTELMODIFICATION"
                Else
                    Mail.TemplateName = "T16_HOTELMODIFICATION"
                End If

                Mail.To = _to
                If Not String.IsNullOrEmpty(_cc) Then Mail.Cc = _cc
                If Not String.IsNullOrEmpty(_bcc) Then Mail.Bcc = _bcc
                Mail.SubjectParam = .Item(dsreservaciones.FIELD_NORESERVACION)


                Mail.Html = True
                Mail.SubjectParam = .Item(dsreservaciones.FIELD_NORESERVACION)
                Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Mail.AddParameter("PORTALNAME") = PortalCulture.GetString("00516")

                If SourceReserv = "UNI" Or SourceReserv = "IDS" Or SourceReserv = "ADS" Or SourceReserv = "CCT" Then
                    Mail.AddParameter("PROPERTY_URLLOGO") = Util.Utility.LoadImagen(cInfoActual.Empresa)
                    Mail.AddParameter("UNIVISITPORTAL") = cInfoActual.HotelName
                End If

                If .Item(dsreservaciones.FIELD_IDHOTEL) = "0" Then
                    Mail.AddParameter("HOTEL_NOMBRE") = IIf(.IsNull(dsreservaciones.FIELD_HOTELGNOMBRE), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGNOMBRE))
                    Mail.AddParameter("HOTEL_CIUDAD") = IIf(.IsNull(dsreservaciones.FIELD_HOTELGCIUDAD), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGCIUDAD))
                Else
                    Mail.AddParameter("HOTEL_NOMBRE") = .Item(dsreservaciones.FIELD_HOTELNOMBRE)
                    Dim ciudad As String = .Item(dsreservaciones.FIELD_HOTELCIUDAD) & ", " & .Item(dsreservaciones.FIELD_HOTELESTADO)
                    Mail.AddParameter("HOTEL_CIUDAD") = ciudad
                End If

                Mail.AddParameter("ID_RESERVACION") = .Item(dsreservaciones.FIELD_NORESERVACION)
                Mail.AddParameter("CHECKIN") = CDate(.Item(dsreservaciones.FIELD_CHECKIN)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CHECKOUT") = CDate(.Item(dsreservaciones.FIELD_CHECKOUT)).ToString("dd/MMM/yyyy")
                Mail.AddParameter("CONTACTO") = .Item(dsreservaciones.FIELD_CLIENTE)
                Mail.AddParameter("REGDATE") = .Item(dsreservaciones.FIELD_FECHARESERVACION)
                Mail.AddParameter("EMAIL_CONTACTO") = .Item(dsreservaciones.FIELD_EMAILCL)
                'Mail.AddParameter("REGDATE") = CDate(.Item("FechaReservacion")).ToString("dd/MMM/yyyy")
                Mail.AddParameter("HABITACIONES") = GetTable(dsreservaciones)
                If .Item("IsNetRateUv") Then
                    Mail.AddParameter("UVNRPOLICIES") = CargaPoliticasUvNetRates()
                Else
                    Mail.AddParameter("UVNRPOLICIES") = ""
                End If

                Mail.Send()
                'Util.Utility.MailerSend(.Item(dsreservaciones.FIELD_NORESERVACION), Mail.GetBody)


            End With
        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            PortalCulture.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
        End Try
    End Sub
End Class






