Imports Portal.Hotel.Facade
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.Hotel.Common.Data

Partial Class ctrlReservationQueryDeposits
    Inherits UserControlBase

    Protected Date1 As _Date
    Protected Date2 As _Date
    Public Event SearchByDates(ByVal fecha1 As Date, ByVal fecha2 As Date, ByVal filtro As Byte, ByVal Cliente As String)
    Public Event SearchByNoRes(ByVal NoReservacion As String)

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

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

    Public ReadOnly Property isHotelSelected() As Boolean
        Get
            Return chkHotels.Checked
        End Get
    End Property

    Public ReadOnly Property idHotelSelected() As Integer
        Get
            Return ddlHoteles.SelectedValue
        End Get
    End Property

    Public WriteOnly Property SetReservacion() As String
        Set(ByVal value As String)
            txtnoConfirmacion.Text = value
        End Set
    End Property

    Private Sub ShowLink(ByVal show As Boolean)
        Dim str As String = If(show, "block", "none")
        Dim str2 As String = If(Not show, "block", "none")

        Me.hplShow.Style.Add("display", str)
        Me.hplHide.Style.Add("display", str2)

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página

        If Not Me.IsPostBack Then
            Call initDates()
            Call loadfilter()
            If Me.UserChain Then
                loadUserChainHotels()
            End If
            EnConsulta = 0
            ShowLink(True)
        Else

        End If

        Dim script As String = String.Empty
        If Me.UserChain Then
            Me.chkHotels.Attributes.Add("onclick", "javascript:ShowOrHideHotels('" & Me.chkHotels.ClientID & "','" & Me.lblHoteles.ClientID & "','" & Me.ddlHoteles.ClientID & "');")
            script = "ShowOrHideHotels('" & Me.chkHotels.ClientID & "','" & Me.lblHoteles.ClientID & "','" & Me.ddlHoteles.ClientID & "');"
        End If

        cvlNoConfirmacion.IsValid = True
        chkDates.Attributes.Add("onclick", "javascript:ShowOrHide('" & Me.chkDates.ClientID & "');")
        'Me.Page.RegisterStartupScript("", "<script>ShowOrHide('" & Me.chkDates.ClientID & "');" & script & "</script>")
        lblHoteles.Style.Add("display", "none")
        ddlHoteles.Style.Add("display", "none")

        Me.hplHide.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "false")
        Me.hplShow.NavigateUrl = String.Format("javascript:FireShowOrHidden('{0}','{1}','{2}','{3}',{4})", pnlSearch.ClientID, hplHide.ClientID, hplShow.ClientID, hdnSearch.ClientID, "true")

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


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        lblFiltro.Text = String.Format(PortalCulture.GetString("01378"), txtnoConfirmacion.Text)
        If Me.txtnoConfirmacion.Text.Trim <> "" Then
            RaiseEvent SearchByNoRes(txtnoConfirmacion.Text)
        Else
            cvlNoConfirmacion.IsValid = False
        End If
    End Sub

    Private Sub BtnSpecificSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSpecificSearch.Click
        Dim sFiltro As String

        sFiltro = PortalCulture.GetString("01379") & ","
        sFiltro &= If(chkDates.Checked, String.Format(PortalCulture.GetString("01370"), ddlFilterby.SelectedItem.Text), " ")
        If ddlHoteles.Items.Count > 0 Then
            sFiltro &= If(ddlHoteles.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01373"), ddlHoteles.SelectedItem.Text))
        End If
        sFiltro &= String.Format(PortalCulture.GetString("01374"), Date1.selectedDate.ToString("dd/MM/yyyy"), Date2.selectedDate.ToString("dd/MM/yyyy"))
        sFiltro &= If(String.IsNullOrEmpty(txtNameCustomer.Text), "", String.Format(PortalCulture.GetString("01377"), txtNameCustomer.Text))
        lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")

        If chkDates.Checked Then
            RaiseEvent SearchByDates(Date1.selectedDate, Date2.selectedDate, ddlFilterby.SelectedValue, txtNameCustomer.Text)
        Else
            RaiseEvent SearchByDates(Date1.selectedDate, Date2.selectedDate, 0, txtNameCustomer.Text)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Call loadResources()
        If Not Me.UserChain Then
            chkHotels.Visible = False
            lblHoteles.Visible = False
            ddlHoteles.Visible = False
        End If
        If Not Me.IsPostBack And String.IsNullOrEmpty(txtnoConfirmacion.Text) Then
            BtnSpecificSearch_Click(Me, Nothing)
        End If
    End Sub

    Private Sub loadResources()
        Me.lblDesde.Text = PortalCulture.GetString("M000110", True)
        Me.lblHasta.Text = PortalCulture.GetString("M000111", True)
        Me.lblNombreCliente.Text = PortalCulture.GetString("M000114", True)
        btnBuscar.Text = PortalCulture.GetString("M000637")
        lblNoconfirmacion.Text = PortalCulture.GetString("00447", True)
        Me.lblSearchSpecific.Text = PortalCulture.GetString("00479")
        Me.lblSearchData.Text = PortalCulture.GetString("00478")
        Me.BtnSpecificSearch.Text = PortalCulture.GetString("M000118")
        Me.chkDates.Text = PortalCulture.GetString("00517")
        Me.lblSearchby.Text = PortalCulture.GetString("M0BT0000113", True)
        cvlNoConfirmacion.ErrorMessage = PortalCulture.GetString("M000583")
        lblHoteles.Text = PortalCulture.GetString("M0BT0000150", True)

        Me.hplShow.Text = PortalCulture.GetString("01454")
        Me.hplHide.Text = PortalCulture.GetString("01455")


    End Sub
End Class
