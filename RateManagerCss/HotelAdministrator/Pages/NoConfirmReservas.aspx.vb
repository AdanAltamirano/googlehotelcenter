
Imports Portal.General.Common.Data
Partial Class NoConfirmReservas
    Inherits PaginaBase
   
    Private Property dgName() As String
        Get
            Return viewstate("_dgname")
        End Get
        Set(ByVal Value As String)
            viewstate("_dgname") = Value
        End Set
    End Property
    Private Property dgpage() As String
        Get
            Return viewstate("_dgpage")
        End Get
        Set(ByVal Value As String)
            viewstate("_dgpage") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ctrlReservationsQuery1 As ctrlReservations

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Dim ds As ReservaDatos
    Private Enum Columns As Integer
        Itinerario
        Fecha
        Cliente
        Llegada
        Salida
        Cantidad
        Status
        StatusNew
        StatusConf
        WizcomPassOn
        idhotel
        Guaranteed
    End Enum
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Me.cInfoActual.Hotel = 0 Then Me.redirectTo(PaginaBase.pages.SearchHotel)

        If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
        End If

        ctrlReservationsQuery1.idHotel = MyBase.cInfoActual.Hotel
        ctrlReservationsQuery1.Supervisor = Me.IsSupervisor 'Me.User.IsInRole("Supervisor")
        ctrlReservationsQuery1.hideExport = False
        ctrlReservationsQuery1.hideAgency = False
        chkAllHotels.Visible = False
        If Me.IsSupervisor Then chkAllHotels.Visible = True
        ctrlReservationsQuery1.ShowHideData(False, False, False, False)
        ctrlReservationsQuery1.hideExport = False

        ctrlReservationsQuery1.UserChain = MyBase.isUserChain
        ctrlReservationsQuery1.idUsuario = Session("idUsuario")


        If Not IsPostBack Then
            chkAllHotels.Checked = False
            dgpage = 0
            dgName = ""
            'loadDatos()
        End If
    End Sub


    'Private Sub loadData()
    '    Dim fecha = CDate(Me.txtInicio.Text)
    '    If Me.User.IsInRole("Supervisor") AndAlso (chkAllHotels.Checked Or Me.cInfoActual.Hotel = 0) Then
    '        With New ReservaAcces
    '            ds = .LoadReservaConfirm(False, fecha)
    '        End With
    '    Else
    '        With New ReservaAcces
    '            ds = .LoadReservaConfirm(False, fecha, Me.cInfoActual.Hotel)
    '        End With
    '    End If
    '    If ds.Tables.Count > 1 Then
    '        Me.dlHoteles.DataKeyField = ReservaDatos.FIELD_IDHOTEL
    '        Dim ci As System.Globalization.CultureInfo
    '        ci = System.Threading.Thread.CurrentThread.CurrentCulture
    '        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
    '        Me.dlHoteles.DataSource = ds.Tables(0)
    '        Me.dlHoteles.DataBind()
    '        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    '    End If
    'End Sub
    Private Sub loadData(ByVal GridCurrentPageIndex As Integer) Handles ctrlReservationsQuery1.loadData
        ctrlReservationsQuery1.SetStatus(250)
        ds = ctrlReservationsQuery1.searchNoconfirmReservations(chkAllHotels.Checked)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 1 Then
            Me.dlHoteles.DataKeyField = ReservaDatos.FIELD_IDHOTEL
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dlHoteles.DataSource = ds.Tables(0)
            Me.dlHoteles.DataBind()
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
        End If

    End Sub


    Private Sub dlHoteles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlHoteles.ItemDataBound
        Dim dv As DataView
        Dim dg As DataGrid
        Dim htls As String()
        Dim tshow As HtmlTable
        If ds.Tables.Count > 1 Then
            dv = ds.Tables(1).DefaultView
            If Me.ctrlReservationsQuery1.idtipohabitacionhotel > 0 Then
                dv.RowFilter = ReservaDatos.FIELD_IDHOTEL & "=" & dlHoteles.DataKeys(e.Item.ItemIndex) & " and idTipoHabitacion=" & ctrlReservationsQuery1.idtipohabitacionhotel
            Else
                dv.RowFilter = ReservaDatos.FIELD_IDHOTEL & "=" & dlHoteles.DataKeys(e.Item.ItemIndex)
            End If



            dg = e.Item.FindControl("dgReservas")
            If Not dg Is Nothing Then
                dg.Visible = False
                AddHandler dg.ItemDataBound, AddressOf dgreservas_ItemDataBound
                ' AddHandler dg.ItemCreated, AddressOf dgReservas_ItemCreated
                AddHandler dg.PageIndexChanged, AddressOf dgReservas_PageIndexChanged


                tshow = e.Item.FindControl("tshow")

                If Not tshow Is Nothing Then
                    tshow.Rows(0).Cells(0).Attributes.Add("onclick", "showReservas('" & dg.ClientID & "','" & tshow.Rows(0).Cells(0).ClientID & "');")
                    tshow.Rows(0).Cells(2).InnerText = dv.Count & " Reservations"
                End If

                Try
                    If dgName.IndexOf(dg.ClientID) <> -1 Then
                        htls = dgName.Split("//")
                        For i As Integer = 0 To htls.Length - 1
                            If htls(i) = dg.ClientID Then
                                dg.CurrentPageIndex = CInt(dgpage.Split("//")(i).ToString)
                            End If
                        Next
                    End If
                Catch ex As Exception
                End Try
                dg.DataSource = dv
                dg.DataBind()
                If dv.Count > 0 Then
                    dg.Visible = True
                    e.Item.Visible = False
                End If
            End If
        End If
    End Sub
    Private Sub dgreservas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim status() As String = {"undefined", PortalCulture.GetString("M000331"), PortalCulture.GetString("M000592"), PortalCulture.GetString("M000333"), PortalCulture.GetString("00719")}
            Dim lb As Label
            lb = e.Item.Cells(Columns.StatusNew).FindControl("lblStatus")
            lb.Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))
            If e.Item.Cells(Columns.Status).Text = "1" And e.Item.Cells(Columns.Guaranteed).Text = "0" Then lb.Text = lb.Text & " " & PortalCulture.GetString("1529")

            If e.Item.Cells(Columns.WizcomPassOn).Text <> "&nbsp;" AndAlso e.Item.Cells(Columns.Status).Text = "1" Then
                lb.Text = PortalCulture.GetString("00439")
                If e.Item.Cells(Columns.StatusConf).Text = "True" Then
                    lb.Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))
                    If e.Item.Cells(Columns.Status).Text = "1" And e.Item.Cells(Columns.Guaranteed).Text = "0" Then lb.Text = lb.Text & " " & PortalCulture.GetString("1529")
                End If
            End If
            e.Item.Cells(Columns.Fecha).Text = CDate(e.Item.Cells(Columns.Fecha).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Salida).Text = CDate(e.Item.Cells(Columns.Salida).Text).ToString("MMM/dd/yyyy")
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Itinerario).Text = PortalCulture.GetString("M000119")
            e.Item.Cells(Columns.Fecha).Text = PortalCulture.GetString("M000120")
            e.Item.Cells(Columns.Cliente).Text = PortalCulture.GetString("M000121")
            e.Item.Cells(Columns.Llegada).Text = PortalCulture.GetString("M000122")
            e.Item.Cells(Columns.Salida).Text = PortalCulture.GetString("M000123")
            e.Item.Cells(Columns.Cantidad).Text = PortalCulture.GetString("00062")
            e.Item.Cells(Columns.Status).Text = PortalCulture.GetString("M000126")
        End If
    End Sub

    Private Sub dgReservas_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Pager Then
            If sender.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If sender.CurrentPageIndex < sender.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgReservas_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        source.CurrentPageIndex = e.NewPageIndex
        loadData(e.NewPageIndex)
    End Sub


    'Private Sub dgReservas_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    CType(source, DataGrid).CurrentPageIndex = e.NewPageIndex
    '    dgpage &= "//" & CType(source, DataGrid).CurrentPageIndex
    '    dgName &= "//" & CType(source, DataGrid).ClientID
    '    loadData(0)
    'End Sub



    Private Sub dlHoteles_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlHoteles.ItemCreated
        Dim dg As DataGrid
        dg = e.Item.FindControl("dgReservas")
        If Not dg Is Nothing Then
            AddHandler dg.PageIndexChanged, AddressOf dgReservas_PageIndexChanged

        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lbltitle.Text = PortalCulture.GetString("M000656")
        chkAllHotels.Text = PortalCulture.GetString("M000657")
        If Not Me.IsPostBack Then
            loadData(0)
        End If
    End Sub
End Class
