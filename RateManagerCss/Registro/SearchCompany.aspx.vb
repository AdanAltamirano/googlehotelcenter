Imports Portal.General.Facade
Imports System.Configuration.ConfigurationManager
Partial Class SearchCompany
    Inherits PaginaBase
    protected withevents CtrlSearchCompany1 as ctrlSearchCompany

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum Columns
        LinkEmpresa = 0
        idEmpresa
        NombreEmpresa
        Address
        State
        County
        City
        Contact
        Email
        Phone
        sGuid
        idhotel
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain Then
            btnSearch.Visible = False
            ' CtrlSearchCompany1.Visible = False
        End If

        CtrlSearchCompany1.Rubro = AppSettings("idRubro")
        Me.DefaultButton(CtrlSearchCompany1.getTxtName, Me.btnSearch)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        If Not Me.IsPostBack Then
            CargaRecursosGrid()
            If Not MyBase.IsSupervisor Then
                Dim ds As DataSet
                '
                If MyBase.isUserChain Then
                    With New HotelSistema
                        ds = .GetHotelsCompanyByUser(Usuario)
                    End With
                End If

                Me.Grid.DataSource = ds
                Me.Grid.DataBind()
            End If
        End If
    End Sub
    Private Sub Grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Grid.PageIndexChanged
        Try
            Grid.CurrentPageIndex = e.NewPageIndex
            If Not MyBase.IsSupervisor Then
                Dim ds As DataSet

                '
                If MyBase.isUserChain Then
                    With New HotelSistema
                        ds = .GetHotelsCompanyByUser(Usuario)
                    End With
                End If
                Me.Grid.DataSource = ds
                Me.Grid.DataBind()
            Else
                Call CargaGrid()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub loadResources()
        lblTitulo.Text = PortalCulture.GetString("00846")
        btnSearch.Text = PortalCulture.GetString("00248")
        Grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        Grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
    End Sub

    Private Sub CargaRecursosGrid()
        Me.Grid.Columns(0).HeaderText = PortalCulture.GetString("00249")
        Me.Grid.Columns(Columns.State).HeaderText = PortalCulture.GetString("00252")
        Me.Grid.Columns(Columns.County).HeaderText = PortalCulture.GetString("00253")
        Me.Grid.Columns(Columns.City).HeaderText = PortalCulture.GetString("00254")
        Me.Grid.Columns(Columns.Contact).HeaderText = PortalCulture.GetString("00257")
        Me.Grid.Columns(Columns.Email).HeaderText = PortalCulture.GetString("00258")
        Me.Grid.Columns(Columns.Phone).HeaderText = PortalCulture.GetString("00164")
    End Sub

    Private Sub Grid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid.SelectedIndexChanged
        With Grid.Items(Grid.SelectedIndex)
            Response.Redirect("CompanyRegister.aspx?idempresa=" & .Cells(Columns.idEmpresa).Text & "&sGuid=" & .Cells(Columns.sGuid).Text & "&idhotel=" & .Cells(Columns.idhotel).Text)
        End With
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Grid.CurrentPageIndex >= Grid.Items.Count \ Grid.PageSize Then
            Grid.CurrentPageIndex = (Grid.Items.Count - 1) \ Grid.PageSize
        End If
        Grid.SelectedIndex = -1
        CargaGrid()
    End Sub
    Private Sub CargaGrid()
        Dim data As New DataSet
        CtrlSearchCompany1.searchCompanys(data, "")
        Me.Grid.DataSource = data
        Me.Grid.DataBind()
    End Sub

    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.Cells(Columns.idhotel).Text = "&nbsp;" Then
                e.Item.Cells(Columns.idhotel).Text = 0
            End If
        End If
    End Sub
End Class
