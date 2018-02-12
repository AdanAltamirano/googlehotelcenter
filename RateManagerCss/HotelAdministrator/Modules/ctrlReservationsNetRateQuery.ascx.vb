Imports Portal.Hotel.Facade
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.Hotel.Common.Data
Imports Oz.UniBilling.Common
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Hotels.Business




Partial Class ctrlReservationsNetRate
    Inherits System.Web.UI.UserControl

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cvlNoConfirmacion As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents hlnkExcel As System.Web.UI.WebControls.HyperLink

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Const KEY_IDHOTEL As String = "idHotel"

    Public Event onSendReservation()

    Public Property DataSource()
        Get
            Return Session(String.Format("{0}:DataSource", Me.ID))
        End Get
        Set(ByVal Value)
            Session(String.Format("{0}:DataSource", Me.ID)) = Value
        End Set
    End Property
	

    Public Property idHotel() As Integer
        Get
            If viewstate.Item(KEY_IDHOTEL) Is Nothing Then
                Return 0
            Else
                Return viewstate.Item(KEY_IDHOTEL)
            End If
        End Get
        Set(ByVal Value As Integer)
            viewstate.Add(KEY_IDHOTEL, Value)
        End Set
    End Property
    Public Property Supervisor() As Boolean
        Get
            Return viewstate("Sup_User")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("Sup_User") = Value
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

    Public ReadOnly Property Moneda() As String
        Get
            If rbMXN.Checked Then
                Return "MXN"
            ElseIf rbUSD.Checked Then
                Return "USD"
            End If
        End Get
    End Property
    Public ReadOnly Property EnDolares() As Boolean
        Get
            If rbUSD.Checked Then
                Return True
            End If
            Return False

        End Get
    End Property

    Private Sub ShowLink(ByVal show As Boolean)
        Dim str As String = If(show, "", "none")
        Dim str2 As String = If(Not show, "", "none")

        Me.hplShow.Style.Add("display", str)
        Me.hplHide.Style.Add("display", str2)

    End Sub


    Protected Date1 As _Date
    Protected Date2 As _Date

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not Me.IsPostBack Then
            Me.initDates()

            Me.loadStatus()
            loadfilter()

            ShowLink(True)
        End If

        chkDates.Attributes.Add("onclick", "javascript:ShowOrHide('" & Me.chkDates.ClientID & "');")
        Me.Page.RegisterStartupScript("", "<script>ShowOrHide('" & Me.chkDates.ClientID & "');</script>")

        Me.hplHide.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "false")
        Me.hplShow.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "true")

    End Sub
    Public Sub SetStatus(ByVal status As Integer)
        Try
            lstStatus.SelectedValue = status
        Catch ex As Exception
        End Try
    End Sub
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


    Private Sub loadStatus()
        Me.lstStatus.Items.Clear()

        Dim item As New ListItem
        'Sele inserta el elemento en la posicion 0 -- Todos --
        item.Text = "-- " & PortalCulture.GetString("M000258") & " --"
        item.Value = 255
        item.Selected = True
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("01142") 'nopagadas
        item.Value = 1
        Me.lstStatus.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("00585") 'pagadas
        item.Value = 2
        Me.lstStatus.Items.Add(item)


    End Sub
    Private Sub loadfilter()
        Dim item As New ListItem
        ddlFilterby.Items.Clear()
        item = New ListItem
        item.Text = PortalCulture.GetString("00392")
        item.Value = 0
        ddlFilterby.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("00368")
        item.Value = 1
        ddlFilterby.Items.Add(item)
        item = New ListItem
        item.Text = PortalCulture.GetString("00369")
        item.Value = 2
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

        item = New ListItem
        item.Text = PortalCulture.GetString("00457")
        item.Value = "UNI"
        ddlCanal.Items.Add(item)

        item = New ListItem
        item.Text = "GDS"
        item.Value = "WIZ"
        ddlCanal.Items.Add(item)

        item = New ListItem
        item.Text = "ADS"
        item.Value = "ADS"


        ddlCanal.Items.Add(item)
    End Sub

    Sub CallFiltro()
        Dim sFiltro As String

        sFiltro = PortalCulture.GetString("01380") & ","
        sFiltro &= If(chkDates.Checked, String.Format(PortalCulture.GetString("01370"), ddlFilterby.SelectedItem.Text), " ")
        sFiltro &= If(lstStatus.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01371"), lstStatus.SelectedItem.Text))
        sFiltro &= If(ddlCanal.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01372"), ddlCanal.SelectedItem.Text))

        sFiltro &= String.Format(PortalCulture.GetString("01374"), Date1.selectedDate.ToString("dd/MM/yyyy"), Date2.selectedDate.ToString("dd/MM/yyyy"))
        lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
    End Sub

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
        Me.lblCanal.Text = PortalCulture.GetString("00670", True)
        Me.lblTotales.Text = PortalCulture.GetString("01143", True)
        Me.hplShow.Text = PortalCulture.GetString("01454")
        Me.hplHide.Text = PortalCulture.GetString("01455")


        If Not Me.IsPostBack Then
            BtnSpecificSearch_Click(Me, Nothing)
            'Dim CheckIn As DateTime = Date1.selectedDate
            'Dim CheckOut As DateTime = Date2.selectedDate
            'Dim noReservation As String = Me.txtnoConfirmacion.Text
            'Dim parIdHotel As Double = -1
            'If Not Supervisor Then
            '    parIdHotel = idHotel
            'End If
            'CallFiltro()
            'With New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
            '    DataSource = .SearchReservationNetRateByFilter(-1, -1, CheckIn, CheckOut, -1, noReservation, Nothing, parIdHotel)

            'End With
            'RaiseEvent onSendReservation()
        End If


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        loadResources()
    End Sub



    Private Sub lstStatus_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstStatus.PreRender
        Dim Sel As Integer
        Sel = Me.lstStatus.SelectedIndex
        Me.loadStatus()
        Me.lstStatus.Items(0).Text = "-- " & PortalCulture.GetString("M000258") & " --"
        Me.lstStatus.SelectedIndex = Sel

    End Sub

    Function GetIdAsociation() As Integer
        Dim IdAsociation As Integer = 0
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim CheckIn As DateTime = Date1.selectedDate
        Dim CheckOut As DateTime = Date2.selectedDate
        Dim noReservation As String = Me.txtnoConfirmacion.Text
        Dim parIdHotel As Integer = -1
        Dim idAsoc As Integer = Me.GetIdAsociation

        If Not Supervisor Then
            parIdHotel = idHotel
        End If
        lblFiltro.Text = String.Format(PortalCulture.GetString("01378"), txtnoConfirmacion.Text)
        With New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
            If Not Supervisor Then
                DataSource = .SearchReservationNetRateByFilter(Nothing, Nothing, CheckIn, CheckOut, Nothing, noReservation, Nothing, parIdHotel, Nothing, Nothing, If(idAsoc > 0, idAsoc, Nothing))
            Else
                DataSource = .SearchReservationNetRateByFilter(Nothing, Nothing, CheckIn, CheckOut, Nothing, noReservation, Nothing, Nothing, Nothing, Nothing, If(idAsoc > 0, idAsoc, Nothing))
            End If

        End With
        RaiseEvent onSendReservation()

    End Sub

    Private Sub BtnSpecificSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSpecificSearch.Click

        Dim CheckIn As DateTime = Date1.selectedDate
        Dim CheckOut As DateTime = Date2.selectedDate
        Dim source As String = Nothing
        Dim filterDate As Integer = -1
        Dim idAsoc As Integer = GetIdAsociation()

        If ddlCanal.SelectedIndex <> -1 AndAlso ddlCanal.SelectedItem.Value <> "ALL" Then
            source = ddlCanal.SelectedItem.Value
        End If

        If chkDates.Checked Then
            filterDate = ddlFilterby.SelectedItem.Value
        End If

        Dim Status As Byte = Me.lstStatus.SelectedValue
        CallFiltro()
        With New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
            DataSource = .SearchReservationNetRateByFilter(-1, filterDate, CheckIn, CheckOut, -1, Nothing, source, idHotel, Nothing, Nothing, associationid:=If(idAsoc > 0, idAsoc, Nothing))
            filterStatus()
        End With
        RaiseEvent onSendReservation()

    End Sub

    Private Sub filterStatus()
        Dim dsRes As ReservationNetRateDataSet = CType(DataSource, ReservationNetRateDataSet)
        If Not dsRes Is Nothing Then
            Select Case lstStatus.SelectedItem.Value
                Case 1 ' sin pagar
                    For Each dr As ReservationNetRateDataSet.ReservationRow In dsRes.Reservation
                        If dr.ConciliationStatus = 3 Then
                            dr.Delete()
                        End If
                    Next
                Case 2 ' pagada
                    For Each dr As ReservationNetRateDataSet.ReservationRow In dsRes.Reservation
                        If dr.ConciliationStatus <> 3 Then
                            dr.Delete()
                        End If
                    Next
            End Select
            dsRes.AcceptChanges()

        End If


    End Sub






End Class

