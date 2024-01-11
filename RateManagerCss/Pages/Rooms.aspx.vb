Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Partial Class Rooms


    Inherits PaginaBase
    Public WriteOnly Property txtD1() As String
        Set(ByVal Value As String)
            txtDiv1.Text = Value
        End Set
    End Property
    Public WriteOnly Property txtD2() As String
        Set(ByVal Value As String)
            txtDiv2.Text = Value
        End Set
    End Property
    Public WriteOnly Property txtD3() As String
        Set(ByVal Value As String)
            txtDiv3.Text = Value
        End Set
    End Property
    Public Property editar() As Boolean
        Get
            Return ViewState("_noeditar")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("_noeditar") = Value
        End Set
    End Property


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

    Private Const KEY_IDROOM As String = "idroom"

#Region "Propiedades, Enumerativos y Eventos"

    Public Property idRoom() As Integer
        Get
            If ViewState.Item(KEY_IDROOM) Is Nothing Then
                Return 0
            Else
                Return CType(ViewState.Item(KEY_IDROOM), Integer)
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add(KEY_IDROOM, Value)
        End Set
    End Property

    Property Editando() As Boolean
        Get
            Return ViewState("Editando")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editando") = Value
        End Set
    End Property

    Property Habitacion() As String
        Get
            Return ViewState("Habitacion")
        End Get
        Set(ByVal Value As String)
            ViewState("Habitacion") = Value
        End Set
    End Property

    Property TipoEdicion() As Integer
        Get
            Return ViewState("TipoEdicion")
        End Get
        Set(ByVal Value As Integer)
            ViewState("TipoEdicion") = Value
        End Set
    End Property


    Private Enum columns
        idRoomType
        Orden
        codigo
        Tipo
        nameroom
        NumberRooms
        PeoplesInRoom
        MinAdults
        MaxAdults
        MaxChildrens
        PeoplesExtras
        ExtraBeds
        ExtraBedPrice
        Options
        Delete
        Active
        Description
        eliminada
        idDiccionarioNombreHabitacion

    End Enum

    Private Enum Edicion
        NoEdicion = 0
        Rooms = 1
    End Enum

#End Region
    Protected WithEvents CtrlRooms1 As ctrlRooms
    Protected WithEvents CtlMensajes1 As ctlMensajes
    Protected WithEvents CtlMensajes2 As ctlMensajes

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load


        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        CtrlRooms1.idHotel = MyBase.cInfoActual.Hotel
        CtrlRooms1.idCompany = MyBase.cInfoActual.Empresa
        Me.lblError.Visible = False
        Me.lblDeleteError.Visible = False
        If Not IsPostBack Then
            LoadRooms("")
            Me.CtrlRooms1.edicion = False
            TipoEdicion = Edicion.NoEdicion
            Editando = False
            Me.grid.SelectedIndex = -1
            MostrarCmdNew(True)

            ddlFilter.Items.Clear()
            ddlFilter.Items.Add(New ListItem(PortalCulture.GetString("01541"), 1))
            ddlFilter.Items.Add(New ListItem(PortalCulture.GetString("01542"), 0))
            ddlFilter.Items.Add(New ListItem(PortalCulture.GetString("01543"), -1))
        End If

        divContenedor.Style("display") = "none"
        'If (MostarDivContenedor) Then divContenedor.Style("display") = "block"
        'btnMostarDivContenedor.Visible = Not MostarDivContenedor
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))


        Me.ResizefrmPrincipal()
    End Sub

    Private Sub LoadRooms(ByVal sFiltro As String)

        Dim roomsDataView As DataView = Nothing

        With CtrlRooms1
            If ddlFilter.SelectedIndex = 0 Then
                sFiltro = "eliminada=false " & sFiltro
            ElseIf ddlFilter.SelectedIndex = 1 Then
                sFiltro = "eliminada=true " & sFiltro
            End If

            roomsDataView = .getAllRooms(sFiltro)

            'grid.DataSource = .getAllRooms(sFiltro)

            grid.DataSource = roomsDataView
            .totalRooms = roomsDataView.ToTable().Rows.Count

            CType(grid.Columns(columns.idRoomType), BoundColumn).DataField = RoomsHotelData.FLD_ID_ROOM_HOTEL

            CType(grid.Columns(columns.Orden), BoundColumn).DataField = RoomsHotelData.FLD_ORDEN

            CType(grid.Columns(columns.codigo), BoundColumn).DataField = RoomsHotelData.FLD_ROOM_CODE  '//// este nueva
            CType(grid.Columns(columns.Tipo), BoundColumn).DataField = RoomsHotelData.FLD_ROOM_TYPE '//// este nueva
            CType(grid.Columns(columns.nameroom), BoundColumn).DataField = RoomsHotelData.FLD_NOMBRE
            CType(grid.Columns(columns.PeoplesInRoom), BoundColumn).DataField = RoomsHotelData.FLD_NUMBER_PEOPLESINROOM
            CType(grid.Columns(columns.PeoplesExtras), BoundColumn).DataField = RoomsHotelData.FLD_NUMBER_PEOPLESEXTRAS
            CType(grid.Columns(columns.MinAdults), BoundColumn).DataField = RoomsHotelData.FLD_NUMBER_MINADULTS
            CType(grid.Columns(columns.MaxAdults), BoundColumn).DataField = RoomsHotelData.FLD_NUMBER_MAXADULTS
            CType(grid.Columns(columns.MaxChildrens), BoundColumn).DataField = RoomsHotelData.FLD_NUMBER_MAXCHILDREN
            CType(grid.Columns(columns.NumberRooms), BoundColumn).DataField = RoomsHotelData.FLD_NUMBER_ROOMS
            CType(grid.Columns(columns.Description), BoundColumn).DataField = RoomsHotelData.FLD_DESCRIPTION
            CType(grid.Columns(columns.ExtraBedPrice), BoundColumn).DataField = RoomsHotelData.FLD_ExtraBedPrice
            CType(grid.Columns(columns.ExtraBeds), BoundColumn).DataField = RoomsHotelData.FLD_ExtraBeds
            CType(grid.Columns(columns.eliminada), BoundColumn).DataField = "eliminada"
            'Page.RegisterViewStateHandler()
            grid.DataBind()

            If .edicion = False Then
                .cargarOrden()
            End If
        End With

    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        idRoom = 0
        Me.grid.SelectedIndex = -1
        Me.CtrlRooms1.edicion = False
        CtrlRooms1.newRoom()
        TipoEdicion = Edicion.NoEdicion
        Editando = False
    End Sub



    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click, btnPublicar.Click
        Dim _Imgerror As Integer = 0
        If Not Page.IsValid Then Return

        Dim publish As Boolean = (CType(sender, Button).ID = "btnPublicar")

        If Not CtrlRooms1.SaveRoom(_Imgerror, publish) Then
            If _Imgerror <> 1 Then
                lblError.Text = PortalCulture.GetString("00097")  '"No se puede agregar el registro por que ya existe"
                Me.lblError.Visible = True
                MostrarCmdNew(False)
            Else

            End If
        Else
            '  guardalog("/Pages/Rooms.aspx", PaginaBase.acciones.Modificar, "Se modificó la habitación " & grid.Items(grid.SelectedIndex).Cells(columns.Tipo).Text & " - " & grid.Items(grid.SelectedIndex).Cells(columns.nameroom).Text & " de el hotel " & Me.cInfoActual.HotelName)
            Me.lblError.Text = ""
            Me.lblError.Visible = False
            MostrarCmdNew(True)
            'Se ha pedido que se quite la pantalla que pregunta si kiere agregar tarifas ó continuar con el catalogo de habitaciones
            'If Me.idRoom > 0 Then
            ''Cuando hay una modificación se debe modificar la tarifa rack a excepción de
            ''los cuartos que están linkeados
            'Dim ds As LinkRoomTypeData
            'Dim dv As DataView
            'With New LinkRoomsFacade
            '    ds = .getList(Me.cInfoActual.Hotel)
            'End With
            'dv = ds.Tables(ds.TABLE_LINKROOM).DefaultView
            'dv.RowFilter = ds.FIELD_TargetRoom & "=" & idRoom
            'If dv.Count = 0 Then
            '    Response.Redirect("/Pages/RedirectToRates.aspx?id=1" & "&Room=" & CtrlRooms1.idRoom)
            'End If
            'Else
            '    Response.Redirect("/Pages/RedirectToRates.aspx?id=0" & "&Room=" & CtrlRooms1.idRoom)
            'End  If

        End If
        'LoadRooms("")
        Me.grid.SelectedIndex = -1
        TipoEdicion = Edicion.NoEdicion
        Editando = False
        Me.CtrlRooms1.edicion = False
        LoadRooms("")
        Me.idRoom = 0
        CtrlRooms1.newRoom()

        'btnOcultarDivContenedor_Click(Nothing, Nothing)
    End Sub

    Private Sub grid_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grid.PageIndexChanged
        grid.CurrentPageIndex = e.NewPageIndex
        LoadRooms("")
    End Sub

    Private Function isLinked(ByVal idTipoHabitacion As String) As Boolean
        Dim links As New LinkRoomTypeData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New LinkRoomsFacade
            links = .getList(Me.cInfoActual.Hotel)
        End With

        Dim dr() As DataRow = links.Tables(0).Select("idTipoHabitacion_Source = " + idTipoHabitacion)

        If dr.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub grid_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then

            Dim stat As String = e.Item.Cells(columns.eliminada).Text
            Dim LK As LinkButton
            Dim hpl As HyperLink
            Dim sMsg As String

            Dim ordenItem As Integer = CInt(e.Item.Cells(columns.Orden).Text)

            If (ordenItem > CtrlRooms1.totalRooms) Then
                e.Item.Cells(columns.Orden).Text = CtrlRooms1.totalRooms
            End If



            LK = e.Item.FindControl("ibtnEdit")

                If Not LK Is Nothing Then
                    LK.Text = PortalCulture.GetString("M000061") 'PortalCulture Editar
                    If (stat.ToLower.Trim = "true") Then
                        LK.Enabled = False
                    Else
                        LK.Enabled = True
                    End If
                End If

                If stat.ToUpper() = "TRUE" Then ' Activamos, Habitacion esta desactivada

                    e.Item.FindControl("ibtnDelete2").Visible = False
                    e.Item.FindControl("ibtnDelete").Visible = False

                    LK = e.Item.FindControl("ibtnActive2")
                    hpl = e.Item.FindControl("ibtnActive")
                    sMsg = PortalCulture.GetString("01522") 'PortalCulture Realmente Desea Activar la Habitacion
                    hpl.Text = PortalCulture.GetString("01520") 'PortalCulture Activar
                    hpl.NavigateUrl = Me.CtlMensajes2.getShow(LK.ClientID, PortalCulture.GetString("A00041"), sMsg, True)

                    If ddlFilter.SelectedValue = "-1" Then
                        e.Item.Style("background-color") = "#FEE"
                    End If

                Else ' Desactivamos, Habitacion esta activada

                    e.Item.FindControl("ibtnActive2").Visible = False
                    e.Item.FindControl("ibtnActive").Visible = False

                    LK = e.Item.FindControl("ibtnDelete2")
                    hpl = e.Item.FindControl("ibtnDelete")



                    If Not isLinked(e.Item.Cells(columns.idRoomType).Text) Then
                        sMsg = PortalCulture.GetString("01523")
                        hpl.Text = PortalCulture.GetString("01521")
                    Else
                        hpl.Visible = False
                        e.Item.Cells(columns.Delete).Text = PortalCulture.GetString("01588") 'PortalCulture Vinculado
                    End If

                    hpl.NavigateUrl = Me.CtlMensajes1.getShow(LK.ClientID, PortalCulture.GetString("A00041"), sMsg, True)
                End If

                e.Item.Cells(columns.ExtraBedPrice).Text = FCurrency(e.Item.Cells(columns.ExtraBedPrice).Text, 2)


            ElseIf e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(columns.Orden).Text = PortalCulture.GetString("00920")
            ElseIf e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(columns.Delete).Text = CType(grid.DataSource, DataView).Count & " " & PortalCulture.GetString("A00041")
        End If

    End Sub

    Private Sub grid_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand
        Dim elimina As Boolean
        Try
            idRoom = Integer.Parse(e.Item.Cells(0).Text)
            elimina = If(e.Item.Cells(columns.eliminada).Text.ToString.ToLower.Trim = "true", True, False)
            grid.SelectedIndex = e.Item.ItemIndex
        Catch ex As Exception
            Return
        End Try
        CtrlRooms1.loadRoom(idRoom)

        Select Case e.CommandName
            Case "Select"
                TipoEdicion = Edicion.Rooms
                Me.CtrlRooms1.edicion = True
                Me.CtrlRooms1.cargarOrden()
                'CtrlRooms1.loadRoom(idRoom)
                If Not editar = False Then
                    Me.Editando = True
                    Me.Habitacion = e.Item.Cells(1).Text
                Else
                    Me.btnGuardar.Enabled = False
                End If
                MostrarCmdNew(False)

            Case "Delete"
                Dim _error As Integer = CtrlRooms1.deleteRoom(elimina)
                If _error = 0 Then
                    guardalog("/Pages/Rooms.aspx", PaginaBase.acciones.Eliminar, "Se eliminó la habitación " & grid.Items(grid.SelectedIndex).Cells(columns.Tipo).Text & " - " & grid.Items(grid.SelectedIndex).Cells(columns.nameroom).Text & " de el hotel " & Me.cInfoActual.HotelName)
                    If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
                        grid.CurrentPageIndex = (((grid.Items.Count - 1) * grid.PageSize) - 1) \ grid.PageSize
                    End If
                    Me.CtrlRooms1.edicion = False
                    Me.grid.SelectedIndex = -1
                    TipoEdicion = Edicion.NoEdicion
                    Editando = False
                    LoadRooms("")
                Else
                    Select Case _error
                        Case 1
                            lblDeleteError.Text = PortalCulture.GetString("00529")
                        Case 2
                            lblDeleteError.Text = PortalCulture.GetString("00530")
                        Case 3
                            lblDeleteError.Text = PortalCulture.GetString("00528")
                    End Select

                    lblDeleteError.Visible = True
                End If
                MostrarCmdNew(True)
                btnOcultarDivContenedor_Click(Nothing, Nothing)

            Case "Active"
                Dim _error As Integer = CtrlRooms1.activeRoom()

                If _error = 0 Then
                    guardalog("/Pages/Rooms.aspx", PaginaBase.acciones.Eliminar, "Se activó la habitación " & grid.Items(grid.SelectedIndex).Cells(columns.Tipo).Text & " - " & grid.Items(grid.SelectedIndex).Cells(columns.nameroom).Text & " de el hotel " & Me.cInfoActual.HotelName)
                    If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
                        grid.CurrentPageIndex = (((grid.Items.Count - 1) * grid.PageSize) - 1) \ grid.PageSize
                    End If
                    Me.CtrlRooms1.edicion = False
                    Me.grid.SelectedIndex = -1
                    TipoEdicion = Edicion.NoEdicion
                    Editando = False
                    LoadRooms("")
                End If

                MostrarCmdNew(True)
                btnOcultarDivContenedor_Click(Nothing, Nothing)
        End Select
    End Sub
    Private Sub showRoom(ByVal idRooms As Integer)
        Me.PanelRooms.Visible = True
        CtrlRooms1.loadRoom(idRooms)
    End Sub


#Region "msg"
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.hMsg()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.hMsg()
    End Sub

    Private Sub sMsg()
        'Page.RegisterStartupScript("ClientSideCode", "<script>wMsgShow();</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ClientSideCode", "<script>wMsgShow();</script>")
    End Sub
    Private Sub hMsg()
        'Page.RegisterStartupScript("ClientSideCode", "<script>wMsgHide();</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ClientSideCode", "<script>wMsgHide();</script>")
    End Sub
#End Region

    'Esta funcion actualiza todas las imagenes tomando la origina y creando la _M y _T
    Private Sub UpdateImgs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateImgs.Click
        Dim tSubDirs() As String
        tSubDirs = IO.Directory.GetDirectories(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB"))
        For Each dir As String In tSubDirs
            Dim Archivos() As String = IO.Directory.GetFiles(dir)
            For Each Archivo As String In Archivos
                If Not Archivo.LastIndexOfAny("_") > 0 Then
                    Try
                        Dim BitImage As New Bitmap(Archivo)
                        Dim Bit As Bitmap
                        Dim newSize As Size
                        '--------------------------------------------------------
                        'AppSettings("DIR_TIPO_HAB") + IdCompamy + Tipohabitacion
                        '--------------------------------------------------------
                        'MODULO - SALVANDO IMAGEN    (IdRoom + "_M")
                        newSize = New Size(160, 120)
                        Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
                        Bit.Save(Archivo & "_M", ImageFormat.Png)
                        'THUMBNAiL - SALVANDO IMAGEN  (IdRoom + "_T")
                        newSize = New Size(70, 70)
                        Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
                        Bit.Save(Archivo & "_T", ImageFormat.Png)
                    Catch ex As Exception
                    End Try
                End If
            Next
        Next
    End Sub

    Private Sub loadResources()
        cmdNew.Value = PortalCulture.GetString("00102")
        btnOcultarDivContenedor.Text = PortalCulture.GetString("00009")

        lblTitleForm.Text = PortalCulture.GetString("00048")
        btnNuevo.Text = PortalCulture.GetString("00102")




        btnGuardar.Text = PortalCulture.GetString("00008")

        If Editando Then
            lblMsg.Text = PortalCulture.GetString("00054") & " - [ " & Me.Habitacion & " ]"
            Select Case TipoEdicion
                Case Edicion.Rooms
                    MyClass.showRoom(idRoom)

            End Select
        Else
            Me.lblMsg.Text = PortalCulture.GetString("00055")
        End If
        grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        grid.Columns(columns.nameroom).HeaderText = PortalCulture.GetString("M000066")

        grid.Columns(columns.codigo).HeaderText = PortalCulture.GetString("M000432")
        grid.Columns(columns.Tipo).HeaderText = PortalCulture.GetString("00057")
        grid.Columns(columns.PeoplesInRoom).HeaderText = PortalCulture.GetString("00058")
        grid.Columns(columns.PeoplesExtras).HeaderText = PortalCulture.GetString("00059")
        grid.Columns(columns.MinAdults).HeaderText = PortalCulture.GetString("01178")
        grid.Columns(columns.MaxAdults).HeaderText = PortalCulture.GetString("01179")
        grid.Columns(columns.MaxChildrens).HeaderText = PortalCulture.GetString("00061")
        grid.Columns(columns.NumberRooms).HeaderText = PortalCulture.GetString("00062")

        grid.Columns(columns.ExtraBedPrice).HeaderText = PortalCulture.GetString("00063")
        grid.Columns(columns.ExtraBeds).HeaderText = PortalCulture.GetString("00064")
        lblFilter.Text = PortalCulture.GetString("01100")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        LoadRooms(ctrlAutoComplete1.GetFilter)
        editar = True

        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Rooms, Me.btnNuevo, "A")
        For Each i As DataGridItem In Me.grid.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                Dim img As LinkButton
                img = i.Cells(columns.Options).FindControl("ibtnEdit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.Rooms, img, "M")
                Dim lnkdel As HyperLink
                lnkdel = i.Cells(columns.Options).FindControl("ibtnDelete")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.Rooms, lnkdel, "D")
            End If
        Next
    End Sub
    Private Sub lnkFaresCatalogue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect(GeRequestApplicationPath(String.Concat("/Pages/FaresRoom.aspx?Room=", CtrlRooms1.idRoom)))
    End Sub
    Private Sub lnkLinkRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect(GeRequestApplicationPath(String.Concat("/Pages/RoomsLinks.aspx?Room=", CtrlRooms1.idRoom)))
    End Sub

    Private Sub lnkFares_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect(GeRequestApplicationPath(String.Concat("/Pages/FaresCatalogue.aspx?Room=", CtrlRooms1.idRoom)))
    End Sub

    Private Sub grid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            For Each lk As WebControl In e.Item.Controls
                lk.Attributes.Add("onClick", "validaInputFile();")
            Next
            If Me.grid.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.Attributes.Add("onClick", "validaInputFile();")
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.grid.CurrentPageIndex < Me.grid.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.Attributes.Add("onClick", "validaInputFile();")
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
        Dim lnk As LinkButton
        If e.Item.ItemType = ListItemType.EditItem Or _
            e.Item.ItemType = ListItemType.AlternatingItem _
            Or e.Item.ItemType = ListItemType.Item Then
            lnk = e.Item.Cells(10).FindControl("ibtnEdit")
            lnk.Attributes.Add("onClick", "validaInputFile();")
            lnk = e.Item.Cells(11).FindControl("ibtnDelete2")
            lnk.Attributes.Add("onClick", "validaInputFile();")
        End If

    End Sub


#Region "Mostrar Oculatar Formulario"


    'Protected Property MostarDivContenedor() As Boolean
    '    Get
    '        If (ViewState(String.Format("{0}MostarDivContenedor", Me.ClientID)) Is Nothing) Then
    '            Return False
    '        End If
    '        Return CType(ViewState(String.Format("{0}MostarDivContenedor", Me.ClientID)), Boolean)
    '    End Get
    '    Set(ByVal value As Boolean)

    '        ViewState(String.Format("{0}MostarDivContenedor", Me.ClientID)) = value
    '    End Set
    'End Property

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    'Protected Sub btnMostarDivContenedor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMostarDivContenedor.Click
    '    MostarDivContenedor = True
    '    divContenedor.Style("display") = "block"
    '    btnNuevo_Click(sender, e)
    'End Sub
    'Private Sub PrepararParaEditar()
    '    MostarDivContenedor = True
    '    divContenedor.Style("display") = "block"
    'End Sub

    Protected Sub btnOcultarDivContenedor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOcultarDivContenedor.Click
        'MostarDivContenedor = False
        'divContenedor.Style("display") = "none"
        'btnMostarDivContenedor.Visible = Not MostarDivContenedor
        Me.CtrlRooms1.edicion = False
        MostrarCmdNew(True)
        btnNuevo_Click(sender, e)
    End Sub

#End Region

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        grid.CurrentPageIndex = 0
    End Sub

End Class