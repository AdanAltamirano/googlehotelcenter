Imports Portal.General.Facade
Imports Portal.General.Common.Data
Partial Class ReservationsReport
    Inherits PaginaBase
    Dim ds As DataSet
    Dim dsCodes As DataSet
    Enum dgcolumns
        cantidad
        source
        sourceName
        nombrehotel
    End Enum

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        chkAllHotels.Visible = False
        If Me.IsSupervisor Then chkAllHotels.Visible = True
        If Not IsPostBack Then
            chkAllHotels.Checked = False
            Me.txtInicio.Text = Today.Date.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Today.Date.ToString("MM/dd/yyyy")
            loadstatus()
        End If
        btnSearch.OnClientClick = "FireShowImg(); return true;"

    End Sub

    Private Sub loadstatus()
        ddlStatus.Items.Clear()
        ddlStatus.Items.Insert(0, PortalCulture.GetString("00172"))
        ddlStatus.Items.Insert(1, PortalCulture.GetString("M000331"))
        ddlStatus.Items.Insert(2, PortalCulture.GetString("M000333"))
        ddlStatus.Items(0).Value = 0
        ddlStatus.Items(1).Value = 1
        ddlStatus.Items(2).Value = 3
    End Sub

    Private Sub loadDatos()
        Dim idAsoc As Integer = Me.GetIdAsociation
        Dim dv As DataView

        If Me.IsSupervisor AndAlso (chkAllHotels.Checked) Then
            With New ReservaFacade
                ds = .getChanelReservations(CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), RdCheckIn.Checked, Me.ddlStatus.SelectedItem.Value, 0, idAsociacion:=idAsoc)
            End With
        Else
            With New ReservaFacade
                ds = .getChanelReservations(CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), RdCheckIn.Checked, Me.ddlStatus.SelectedItem.Value, Me.cInfoActual.Hotel, idAsociacion:=idAsoc)
            End With
        End If
        dsCodes = New DataSet
        dsCodes.ReadXml(Server.MapPath(GeRequestApplicationPath("/Data/RFCode.xml")))
        dv = ds.Tables(0).DefaultView

        Me.dlHoteles.DataKeyField = ReservaDatos.FIELD_IDHOTEL
        Me.dlHoteles.DataSource = dv 'ds.Tables(0)
        Me.dlHoteles.DataBind()

    End Sub

    Private Sub dlHoteles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlHoteles.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataView
            Dim dg As DataGrid
            Dim idAsoc As Integer = Me.GetIdAsociation
            Dim str As String = ""
            'Dim htls As String()
            Dim tshow As HtmlTable
            If ds.Tables.Count > 1 Then
                'str = If(idAsoc > 0, " And Source in ('HTL', 'POR') or (source='CCT' And SourceCode='WI')", "")
                ' se filtra desde el sp spGetChanelReservation
                dv = ds.Tables(1).DefaultView
                dv.RowFilter = ReservaDatos.FIELD_IDHOTEL & "=" & dlHoteles.DataKeys(e.Item.ItemIndex) & str

                dg = e.Item.FindControl("dgReservas")
                If Not dg Is Nothing Then
                    dg.Visible = False
                    AddHandler dg.ItemDataBound, AddressOf dgreservas_ItemDataBound
                    tshow = e.Item.FindControl("tshow")

                    If Not tshow Is Nothing Then
                        tshow.Rows(0).Cells(0).Attributes.Add("onclick", "showReservas('" & dg.ClientID & "','" & tshow.Rows(0).Cells(0).ClientID & "');")
                        '  tshow.Rows(0).Cells(2).InnerText = dv.Count & " Reservations"
                    End If
                    dg.DataSource = dv
                    dg.DataBind()
                    If dv.Count > 0 Then
                        dg.Visible = True
                        e.Item.Visible = False
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub dgreservas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            Select Case e.Item.Cells(dgcolumns.source).Text.ToUpper
                Case "WIZ"
                    e.Item.Cells(dgcolumns.source).Text = "WIZCOM"
                    If Not dsCodes Is Nothing Then
                        Dim dv As DataView
                        dv = dsCodes.Tables("RF").DefaultView
                        dv.RowFilter = "Code='" & e.Item.Cells(dgcolumns.sourceName).Text & "'"
                        If dv.Count > 0 Then
                            e.Item.Cells(dgcolumns.sourceName).Text = dv(0)("Description")
                        End If
                    End If
                Case "UNI"
                    e.Item.Cells(dgcolumns.source).Text = "UNIPANTALLA"
                    e.Item.Cells(dgcolumns.sourceName).Text = "UNIPANTALLA " & e.Item.Cells(dgcolumns.nombrehotel).Text.ToUpper
                Case "CCT"
                    e.Item.Cells(dgcolumns.source).Text = "CALL CENTER"
                Case "POR"
                    e.Item.Cells(dgcolumns.source).Text = "PORTAL"
            End Select
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.cantidad).Text = PortalCulture.GetString("M000125")
            e.Item.Cells(dgcolumns.source).Text = PortalCulture.GetString("M000262")

        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim fecha As DateTime = CDate(Me.txtInicio.Text)
        

        loadDatos()
    End Sub


    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lbltitle.Text = PortalCulture.GetString("00382")
        chkAllHotels.Text = PortalCulture.GetString("M000657")
        lblFecha.Text = PortalCulture.GetString("00108", True)
        lblHasta.Text = PortalCulture.GetString("00109", True)
        btnSearch.Text = PortalCulture.GetString("M000637")
        RdResDate.Text = PortalCulture.GetString("00392")
        RdCheckIn.Text = "Check In"
        ddlStatus.Items(0).Text = PortalCulture.GetString("00172")
        ddlStatus.Items(1).Text = PortalCulture.GetString("M000331")
        ddlStatus.Items(2).Text = PortalCulture.GetString("M000333")

        Dim sFiltro As String

        sFiltro = PortalCulture.GetString("01380") & ","
        sFiltro &= String.Format(PortalCulture.GetString("01370"), If(Me.RdCheckIn.Checked, "check in", PortalCulture.GetString("00392")))
        sFiltro &= String.Format(PortalCulture.GetString("01374"), txtInicio.Text, txtFinal.Text)
        sFiltro &= If(ddlStatus.SelectedIndex = 0, "", String.Format(PortalCulture.GetString("01371"), ddlStatus.SelectedItem.Text))
        sFiltro &= String.Format(PortalCulture.GetString("01373"), If(Me.chkAllHotels.Checked, PortalCulture.GetString("00172"), cInfoActual.HotelName))
        lblFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")

        If Not Me.IsPostBack Then
            loadDatos()
        End If
        'htmlWriteCalendar()
        'htmlSetCalendarTo(lCalendarFrom, txtCheckIn.ClientID, txtCheckOut.ClientID)
        'htmlSetCalendar(lCalendarTo, txtCheckOut.ClientID)
    End Sub
End Class
