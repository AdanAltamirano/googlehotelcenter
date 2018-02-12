Imports Portal.General.Facade
Partial Class ConfirmReservas
    Inherits PaginaBase

    
    Private Enum Columns As Integer
        Itinerario
        Nombre
        Fecha
        Cliente
        Llegada
        Salida
        Cantidad
        pmsACT  'Nuevo para Actualizaciones
        pmsStatus 'Nuevo para indicar el estado de la reservacion Confirmada no Confirmada
        Status
        StatusNew
        StatusConf
        WizcomPassOn
        Confirmada
        pmscode
        id
        noreservacion
    End Enum

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHelp1 As System.Web.UI.WebControls.Label
    Protected WithEvents CtrlReservationsQuery1 As ctrlReservations

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

        If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
        End If

        CtrlReservationsQuery1.idHotel = MyBase.cInfoActual.Hotel
        CtrlReservationsQuery1.Supervisor = Me.IsSupervisor 'Me.User.IsInRole("Supervisor")
        CtrlReservationsQuery1.hideExport = False
        CtrlReservationsQuery1.UserChain = MyBase.isUserChain
        CtrlReservationsQuery1.idUsuario = Session("idUsuario")
        CtrlReservationsQuery1.hideAgency = False



        CtrlReservationsQuery1.ShowHideData(True, False, True, True)
        If Not Page.IsPostBack Then
            CtrlReservationsQuery1.MostrarFiltroPorFiltroDeTransacciones(True)
        End If
    End Sub

    Private Sub dgReservas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lb As Label, ck As CheckBox, txt As TextBox, lnk As HyperLink

            e.Item.Cells(Columns.Fecha).Text = CDate(e.Item.Cells(Columns.Fecha).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Salida).Text = CDate(e.Item.Cells(Columns.Salida).Text).ToString("MMM/dd/yyyy")

            Dim movement As String = String.Empty
            movement = e.Item.Cells(Columns.pmsACT).Text
            Select Case movement
                Case "SS"
                    e.Item.Cells(Columns.pmsACT).Text = PortalCulture.GetString("01052")
                Case "CC"
                    e.Item.Cells(Columns.pmsACT).Text = PortalCulture.GetString("01053")
                Case "XX"
                    e.Item.Cells(Columns.pmsACT).Text = PortalCulture.GetString("01054")
            End Select

            lb = e.Item.Cells(Columns.Confirmada).FindControl("lblPmsCode")
            ck = e.Item.Cells(Columns.Confirmada).FindControl("chkPmsCode")
            txt = e.Item.Cells(Columns.Confirmada).FindControl("txtPmsCode")
            lnk = e.Item.Cells(Columns.Itinerario).FindControl("Itinerary")

            If ConfigurationManager.AppSettings("idSegmento") = "4" Then
                lnk.NavigateUrl = GeRequestApplicationPath(String.Concat("/HotelAdministrator/Pages/ReservationDetailsV2.aspx?qs=", e.Item.Cells(Columns.id).Text))
            Else
                lnk.NavigateUrl = GeRequestApplicationPath(String.Concat("/HotelAdministrator/Pages/ReservationDetails.aspx?qs=", e.Item.Cells(Columns.id).Text))
            End If

            lnk.Target = "_blank"
            ck.Attributes.Add("onclick", "javascript:ShowControls('" & ck.ClientID & "','" & txt.ClientID & "','" & lb.ClientID & "','" & e.Item.Cells(Columns.pmscode).Text & "')")
            lb.Text = ""
            If e.Item.Cells(Columns.pmscode).Text <> "&nbsp;" Then
                lb.Text = e.Item.Cells(Columns.pmscode).Text
            End If
            ck.Checked = False
            lb.Style.Add("display", "none")
            txt.Style.Add("display", "none")

            'If lb.Text <> "" Then
            '    txt.Text = lb.Text
            '    ck.Checked = True
            '    txt.Style.Add("display", "none")
            '    lb.Style.Add("display", "block")
            'End If
            Dim Active As Boolean = False
            Dim stateConfirm As String = e.Item.Cells(Columns.pmsStatus).Text
            If Not dgReservas.DataSource Is Nothing And Session("welcome_noReservacion") <> String.Empty Then
                If TypeOf dgReservas.DataSource Is DataView Then

                    If Not IsPostBack Then
                        If Session("welcome_noReservacion") = CType(dgReservas.DataSource, DataView).Item(e.Item.ItemIndex)("NoReservacion") Then


                            Active = True
                        End If
                    End If
                End If

            End If



            If stateConfirm = "True" Then
                txt.Text = lb.Text
                ck.Checked = True
                txt.Style.Add("display", "none")
                lb.Style.Add("display", "block")
            End If
            If Active Then
                lb.Style.Add("display", "none")
                txt.Style.Add("display", "")
                ck.Checked = True
            End If
            'If Not MyBase.isUserChain Then
            '    e.Item.Cells(Columns.Nombre).Visible = False
            'End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Itinerario).Text = PortalCulture.GetString("M000119")
            'If Not MyBase.isUserChain Then
            '    e.Item.Cells(Columns.Nombre).Visible = False
            'End If
            e.Item.Cells(Columns.Fecha).Text = PortalCulture.GetString("M000120")
            e.Item.Cells(Columns.Cliente).Text = PortalCulture.GetString("M000121")
            e.Item.Cells(Columns.Llegada).Text = PortalCulture.GetString("M000122")
            e.Item.Cells(Columns.Salida).Text = PortalCulture.GetString("M000123")
            e.Item.Cells(Columns.Cantidad).Text = PortalCulture.GetString("00062")
            e.Item.Cells(Columns.pmsACT).Text = PortalCulture.GetString("01051")
            e.Item.Cells(Columns.Confirmada).Text = PortalCulture.GetString("M000654")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(Columns.Confirmada).Text = CType(dgReservas.DataSource, DataView).Count & " " & PortalCulture.GetString("M000028")
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim lb As Label, ck As CheckBox, txt As TextBox, lnk As HyperLink

        For Each it As DataGridItem In Me.dgReservas.Items
            If it.ItemType = ListItemType.Item Or it.ItemType = ListItemType.AlternatingItem Then
                ck = it.Cells(Columns.Confirmada).FindControl("chkPmsCode")
                lb = it.Cells(Columns.Confirmada).FindControl("lblPmsCode")
                txt = it.Cells(Columns.Confirmada).FindControl("txtPmsCode")
                'If Not ck.Checked AndAlso it.Cells(Columns.pmscode).Text <> "&nbsp;" Then
                If Not ck.Checked Then
                    With New Portal.General.DataAccess.ReservaAcces
                        If .ConfirmReserva(System.DBNull.Value.ToString, False, it.Cells(Columns.id).Text) Then
                            If .ConfirmReservarms(System.DBNull.Value.ToString, False, it.Cells(Columns.noreservacion).Text) Then
                            End If
                            guardalog("/HotelAdministrator/Pages/ConfirmReservas.aspx", PaginaBase.acciones.Modificar, "Quitó la confirmación con el codigo de pms " & lb.Text & " de la reservación " & it.Cells(Columns.noreservacion).Text)
                        End If
                        'If .ConfirmReserva(System.DBNull.Value.ToString, it.Cells(Columns.id).Text) Then
                        '    guardalog("/HotelAdministrator/Pages/ConfirmReservas", PaginaBase.acciones.Modificar, "Quitó la confirmación con el codigo de pms " & lb.Text & " de la reservación " & it.Cells(Columns.noreservacion).Text)
                        'End If
                    End With
                End If
                'If ck.Checked AndAlso txt.Text <> "" AndAlso txt.Text <> it.Cells(Columns.pmscode).Text Then
                If ck.Checked Then
                    With New Portal.General.DataAccess.ReservaAcces
                        If .ConfirmReserva(txt.Text, True, it.Cells(Columns.id).Text) Then
                            If .ConfirmReservaRMS(txt.Text, True, it.Cells(Columns.noreservacion).Text) Then
                            End If
                            guardalog("/HotelAdministrator/Pages/ConfirmReservas.aspx", PaginaBase.acciones.Modificar, "Confirmó la reservación con el código de pms " & txt.Text & " de la reservación " & it.Cells(Columns.noreservacion).Text)
                        End If
                        'If .ConfirmReserva(txt.Text, it.Cells(Columns.id).Text) Then
                        '    guardalog("/HotelAdministrator/Pages/ConfirmReservas", PaginaBase.acciones.Modificar, "Confirmó la reservación con el código de pms " & txt.Text & " de la reservación " & it.Cells(Columns.noreservacion).Text)
                        'End If
                    End With
                End If
            End If
        Next
        loadData(0)
    End Sub

    Private Sub dgReservas_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgReservas.PageIndexChanged
        dgReservas.CurrentPageIndex = e.NewPageIndex
        loadData(e.NewPageIndex)
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not IsPostBack Then
            If Session("welcome_noReservacion") <> String.Empty Then
                CtrlReservationsQuery1.buscarNumeroReservacion(Session("welcome_noReservacion"))
                
            Else
                loadData(0)
            End If
        End If

        Me.btnSave.Text = PortalCulture.GetString("M000060")
        dgReservas.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("M000056")
        dgReservas.PagerStyle.NextPageText = PortalCulture.GetString("M000057") & " >>"
        dgReservas.Columns(Columns.Itinerario).HeaderText = PortalCulture.GetString("M000119")
        dgReservas.Columns(Columns.Fecha).HeaderText = PortalCulture.GetString("M000120")
        dgReservas.Columns(Columns.Cliente).HeaderText = PortalCulture.GetString("M000121")
        dgReservas.Columns(Columns.Llegada).HeaderText = PortalCulture.GetString("M000122")
        dgReservas.Columns(Columns.Salida).HeaderText = PortalCulture.GetString("M000123")
        dgReservas.Columns(Columns.Cantidad).HeaderText = PortalCulture.GetString("00062")
        dgReservas.Columns(Columns.Confirmada).HeaderText = PortalCulture.GetString("M000654")
        lblTitle.Text = PortalCulture.GetString("M000655")
        If Me.dgReservas.Items.Count > 0 Then
            lblHelp2.Text = PortalCulture.GetString("00416")
        Else
            lblHelp2.Text = ""
        End If

        Dim chk As CheckBox
        For Each i As DataGridItem In Me.dgReservas.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then

                chk = i.FindControl("chkPmsCode")
                MyBase.Habilitaboton(permisos.ConfirmReservations, chk, "M")

            End If
        Next
        MyBase.Habilitaboton(permisos.ConfirmReservations, Me.btnSave, "M")
       
       

    End Sub

    Private Sub dgReservas_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservas.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If Me.dgReservas.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.dgReservas.CurrentPageIndex < Me.dgReservas.PageCount - 1 Then
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
    Private Sub ctrlreservationsquery1_fillds(ByVal res As DataView) Handles CtrlReservationsQuery1.Fillds
        dgReservas.CurrentPageIndex = 0
        Me.dgReservas.DataSource = res
        Me.dgReservas.DataBind()

    End Sub

    Private Sub loadData(ByVal GridCurrentPageIndex As Integer) Handles CtrlReservationsQuery1.loadData
        'CtrlReservationsQuery1.SetStatus(1)
        'SetStatus(250) es obsoleto no sirve, por lo menos en esta pagina
        CtrlReservationsQuery1.SetStatus(250) 'Con 250 Se obtienen las reservaciones activas y canceladas
        dgReservas.CurrentPageIndex = GridCurrentPageIndex
        Me.dgReservas.DataKeyField = "ID"
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgReservas.DataSource = CtrlReservationsQuery1.searchReservations(250)
        Me.dgReservas.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
       
    End Sub
End Class
