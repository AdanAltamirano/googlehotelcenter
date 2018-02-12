Imports System.Configuration.ConfigurationManager
Imports System.Web.Security
Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.Hotel.Common.Data
Imports System.Data.SqlClient

Partial Class Search
    Inherits PaginaBase
    Protected CtrlSearchCompany1 As ctrlSearchCompany

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
    Public Enum Columns
        LinkEmpresa = 0
        NombreCorp
        idEmpresa
        idRubro
        idHotel
        NombreEmpresa
        Address
        State
        County
        City
        Contact
        Email
        Phone
        status
        Active
        userperfil
        idpais
        EsMoroso
        IdCorporate
        IdAsociation
        IsHouse
    End Enum

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        '  Me.HeaderPanel = ctrlHeader.Usuario.uPortalAdministrador
        If Not MyBase.IsSupervisor AndAlso Not MyBase.IsUnibilling And Not MyBase.IsContent And Not MyBase.isUserChain And Not MyBase.IsUsuarioHotelAssociation _
        And Not MyBase.IsUsuarioCallCenter Then
            btnSearch.Visible = True
            CtrlSearchCompany1.Visible = True
        End If

        CtrlSearchCompany1.Rubro = AppSettings("idRubro")
        Me.DefaultButton(CtrlSearchCompany1.getTxtName, Me.btnSearch)
        If Request.QueryString("SRV") = "S" AndAlso Not Request.QueryString("Ind") Is Nothing Then
            Grid.SelectedIndex = Request.QueryString("Ind")
            loadVariables()
            Dim msgInfo As String
            If Me.cInfoActual.Hotel > 0 Then
                With cInfoActual
                    'Me.lnkNameCompany.Text = .HotelName
                    'Me.imgArrow.Visible = True
                    'Me.lnkNameCompany.Attributes.Item("onmouseover") = "javascript:show('" & Me.divCompanyInfo.ClientID & "')"
                    'Me.lnkNameCompany.Attributes.Item("onmouseout") = "javascript:hide('" & Me.divCompanyInfo.ClientID & "')"
                    'Me.lnkNameCompany.NavigateUrl = "javascript:;;"

                    msgInfo = .HotelName & "<br>"
                    msgInfo &= .Address & "<br>"
                    msgInfo &= .City & "," & .State & "<br>"
                    msgInfo &= PortalCulture.GetString("00162", True) & .Contact & "<br>"
                    msgInfo &= PortalCulture.GetString("00163", True) & .Email & "<br>"
                    msgInfo &= PortalCulture.GetString("00164", True) & .Phone & "<br>"
                    'Me.lblInfo.Text = msgInfo
                End With
            End If
            context.Response.Write(msgInfo)
            Response.End()
        End If
    End Sub
    Private Sub CtrlHeader1_ChangeLanguage()
        If Grid.CurrentPageIndex >= Grid.Items.Count \ Grid.PageSize Then
            Grid.CurrentPageIndex = (Grid.Items.Count - 1) \ Grid.PageSize
        End If
        CargaRecursosGrid()
        CargaGrid()
    End Sub

    Private Sub Grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Grid.PageIndexChanged
        Try
            Grid.CurrentPageIndex = e.NewPageIndex
            If Not MyBase.IsSupervisor AndAlso Not MyBase.IsUnibilling And Not MyBase.IsContent And Not MyBase.IsUsuarioHotelAssociation And Not MyBase.IsHotel And Not MyBase.isUserChain Then
                Dim ds As DataSet

                If MyBase.IsUsuarioHomeAgency Then
                    ds = GetCasasByUserEmail(Usuario)
                    Me.Grid.DataSource = ds
                    Me.Grid.DataBind()
                Else

                    Dim idAsociacionHotel As Integer = Me.GetIdAsociation
                    '
                    If Not MyBase.IsUsuarioHotel And Not MyBase.IsUsuarioHotelAssociation Then 'AndAlso Not MyBase.IsUsuarioHotelAssociation Then
                        With New HotelSistema
                            ds = .GetHotelsCompanyByUser(Usuario, idAsociacionHotel)
                        End With
                    ElseIf MyBase.IsUsuarioHotel Then
                        With New HotelSistema
                            ds = .GetHotelsCompanyByUserHotelId(Usuario, idAsociacionHotel)
                        End With
                    ElseIf MyBase.IsUsuarioHotelAssociation Then
                        Me.CtrlSearchCompany1.searchCompanys(ds)
                    End If
                End If
                'If Not MyBase.IsUsuarioHotel Then
                '    With New HotelSistema
                '        ds = .GetHotelsCompanyByUser(Usuario)
                '    End With
                'ElseIf MyBase.IsUsuarioHotel Then
                '    With New HotelSistema
                '        ds = .GetHotelsCompanyByUserHotelId(Usuario)
                '    End With
                'End If


                If Not IsUsuarioHotel AndAlso ds.Tables(0).Rows.Count = 1 Then
                    Response.Redirect("Welcome.aspx")
                Else
                    Me.Grid.DataSource = ds
                    Me.Grid.DataBind()
                End If
            Else
                Call CargaGrid()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CargaGrid()
        Dim data As New DataSet
        CtrlSearchCompany1.searchCompanys(data)
        Dim dv As DataView = data.Tables(0).DefaultView

        If Not idSegmento = 0 Then
            dv.RowFilter = "idcorporativo1 = " & AppSettings("idSegmento")
        End If

        Me.Grid.DataSource = dv
        Me.Grid.DataBind()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
       
        loadResources()
        If Not Me.IsPostBack Then
            CargaRecursosGrid()
            If Not MyBase.IsSupervisor AndAlso Not MyBase.IsUnibilling And Not MyBase.IsContent And Not MyBase.IsUsuarioCallCenter Then
                Dim ds As DataSet
                Dim idAsociacionHotel As Integer = Me.GetIdAsociation
                'Si es agencia solo mostrar las casas que tenga en sus convenios
                If MyBase.IsUsuarioHomeAgency Then
                    ds = GetCasasByUserEmail(Usuario)
                    Me.Grid.DataSource = ds.Tables(0).DefaultView
                    Me.Grid.DataBind()
                Else
                    If Not MyBase.IsUsuarioHotel And Not MyBase.IsUsuarioHotelAssociation Then 'AndAlso Not MyBase.IsUsuarioHotelAssociation Then
                        With New HotelSistema
                            ds = .GetHotelsCompanyByUser(Usuario, idAsociacionHotel)
                        End With
                    ElseIf MyBase.IsUsuarioHotel Then
                        With New HotelSistema
                            ds = .GetHotelsCompanyByUserHotelId(Usuario, idAsociacionHotel)
                        End With
                    ElseIf MyBase.IsUsuarioHotelAssociation Then
                        Me.CtrlSearchCompany1.searchCompanys(ds)
                    End If
                    If Not IsUsuarioHotel AndAlso ds.Tables(0).Rows.Count = 1 Then
                        Response.Redirect("Welcome.aspx")
                    Else
                        Me.Grid.DataSource = ds.Tables(0).DefaultView
                        Me.Grid.DataBind()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Grid.CurrentPageIndex >= Grid.Items.Count \ Grid.PageSize Then
            Grid.CurrentPageIndex = (Grid.Items.Count - 1) \ Grid.PageSize
        End If
        Grid.SelectedIndex = -1
        CargaGrid()
    End Sub

    Private Sub Grid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid.SelectedIndexChanged
        loadVariables()
        Me.guardalog("/Portal/Pages/Welcome.aspx", PaginaBase.acciones.LogIn, "Han Accesado al sistema")
        'Me.Page.RegisterStartupScript("loadHotel", "<script>loadInfoHotel();</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "loadHotel", "<script>loadInfoHotel();</script>")
        MyBase.redirectTo(PaginaBase.pages.Welcome)
    End Sub

    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'checamos si trae un ID pues si no lo trae la empresa aun no tiene un registro en hotel en este caso
            If Not IsNumeric(e.Item.Cells(Columns.idHotel).Text) Then
                Dim lnkSelect As LinkButton
                lnkSelect = e.Item.Cells(Columns.LinkEmpresa).FindControl("lnkSelect")
                If Not lnkSelect Is Nothing Then
                    lnkSelect.Enabled = False
                End If
            End If

            Select Case e.Item.Cells(Columns.status).Text
                Case "N"
                    e.Item.Cells(Columns.status).Text = PortalCulture.GetString("M000311")
                Case "C"
                    e.Item.Cells(Columns.status).Text = PortalCulture.GetString("M000310")
                Case Else   'O'
                    e.Item.Cells(Columns.status).Text = PortalCulture.GetString("M000309")
            End Select

            If e.Item.Cells(Columns.Active).Text = "False" Then
                e.Item.Cells(Columns.Active).Text = PortalCulture.GetString("M000333")
            Else
                e.Item.Cells(Columns.Active).Text = PortalCulture.GetString("01537")
            End If

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(11).Text = CType(Grid.DataSource, DataView).ToTable.Rows.Count & " " & PortalCulture.GetString("M0BT0000150")
        End If
    End Sub
    Private Sub loadResources()
        lblTitulo.Text = PortalCulture.GetString("00247")
        btnSearch.Text = PortalCulture.GetString("00248")
        Grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        Grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
    End Sub

    Private Sub CargaRecursosGrid()
        Me.Grid.Columns(0).HeaderText = PortalCulture.GetString("00249")
        Me.Grid.Columns(Columns.NombreCorp).HeaderText = PortalCulture.GetString("00838")
        Me.Grid.Columns(7).HeaderText = PortalCulture.GetString("00252")
        Me.Grid.Columns(8).HeaderText = PortalCulture.GetString("00253")
        Me.Grid.Columns(9).HeaderText = PortalCulture.GetString("00254")
        Me.Grid.Columns(10).HeaderText = PortalCulture.GetString("00257")
        Me.Grid.Columns(11).HeaderText = PortalCulture.GetString("00258")
        Me.Grid.Columns(12).HeaderText = PortalCulture.GetString("00164")
        Me.Grid.Columns(13).HeaderText = PortalCulture.GetString("M000308")
        Me.Grid.Columns(Columns.Active).HeaderText = PortalCulture.GetString("01546")
        Me.Grid.Columns(15).HeaderText = PortalCulture.GetString("00261")
    End Sub
    Private Sub loadVariables()
        Dim cInfo As New companyInfo
        Dim ds As DataSet

        'Ojo debe de venir un link button para sacar el daato de la empresa si no entonces no estara esto.
        cInfo.HotelName = Grid.Items(Grid.SelectedIndex).Cells(Columns.NombreEmpresa).Text
        cInfo.Rubro = Grid.Items(Grid.SelectedIndex).Cells(Columns.idRubro).Text
        cInfo.Empresa = Grid.Items(Grid.SelectedIndex).Cells(Columns.idEmpresa).Text
        cInfo.Hotel = Grid.Items(Grid.SelectedIndex).Cells(Columns.idHotel).Text
        cInfo.Address = Grid.Items(Grid.SelectedIndex).Cells(Columns.Address).Text
        cInfo.State = Grid.Items(Grid.SelectedIndex).Cells(Columns.State).Text
        cInfo.City = Grid.Items(Grid.SelectedIndex).Cells(Columns.City).Text
        cInfo.Contact = Grid.Items(Grid.SelectedIndex).Cells(Columns.Contact).Text
        cInfo.Email = Grid.Items(Grid.SelectedIndex).Cells(Columns.Email).Text
        cInfo.Phone = Grid.Items(Grid.SelectedIndex).Cells(Columns.Phone).Text
        cInfo.UserPerfil = Grid.Items(Grid.SelectedIndex).Cells(Columns.userperfil).Text
        cInfo.IdPais = Grid.Items(Grid.SelectedIndex).Cells(Columns.idpais).Text
        cInfo.IsHouse = IIf(Grid.Items(Grid.SelectedIndex).Cells.Count > Columns.IsHouse, IIf(Grid.Items(Grid.SelectedIndex).Cells(Columns.IsHouse).Text.ToLower() = "true", True, False), False)

        Integer.TryParse(Grid.Items(Grid.SelectedIndex).Cells(Columns.IdAsociation).Text, cInfo.IdAsociation)
        Integer.TryParse(Grid.Items(Grid.SelectedIndex).Cells(Columns.IdCorporate).Text, cInfo.IdCorporate)

        cInfo.EsMoroso = False
        If Not Grid.Items(Grid.SelectedIndex).Cells(Columns.EsMoroso) Is Nothing Then
            cInfo.EsMoroso = Grid.Items(Grid.SelectedIndex).Cells(Columns.EsMoroso).Text
        End If
        ds = CtrlSearchCompany1.HotelData(cInfo.Hotel)
        If Not Me.dsEmpty(ds) Then
            If Not ds.Tables(0).Rows(0).IsNull("urlWebSite") Then
                cInfo.Url = ds.Tables(0).Rows(0)("urlWebSite")
            End If            
            cInfo.Rooms = If(Not ds.Tables(0).Rows(0).IsNull("rooms"), ds.Tables(0).Rows(0)("rooms"), 0)
        End If
        MyBase.cInfoActual = cInfo

    End Sub

    Private Sub Grid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If Me.Grid.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.Grid.CurrentPageIndex < Me.Grid.PageCount - 1 Then
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

    Private Function GetCasasByUserEmail(ByVal IdUsuario As Integer) As DataSet
        Dim ds As New DataSet
        Dim dsCommand As New SqlDataAdapter
        With dsCommand
            Try
                dsCommand.SelectCommand = New SqlCommand("spCasasGetListByUserEmail", New SqlConnection(ConfigurationSettings.AppSettings("HotelConnection")))
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.Parameters.Add("@idUsuario", IdUsuario)
                .Fill(ds)
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
        End With
        Return ds
    End Function
End Class
