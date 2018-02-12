Public Partial Class HotelFix
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Dim tab_CitiesList As New HotelBedsDS.HotelsDataTable()
        If New HotelBedsRules().Select_HotelFixAll(tab_CitiesList) Then
            gridList.DataSource = tab_CitiesList
            gridList.DataBind()
        End If
    End Sub

    Protected Sub gridList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridList.PageIndexChanging
        gridList.PageIndex = e.NewPageIndex
        LoadData()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitulo1.Text = PortalCulture.GetString("01614")
    End Sub
End Class