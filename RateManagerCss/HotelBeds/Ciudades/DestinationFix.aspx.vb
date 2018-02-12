Public Partial Class DestinationFix
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Dim tab_CitiesList As New HotelBedsDS.CitiesListDataTable()
        If New HotelBedsRules().Select_Cities_SkippedByAll(tab_CitiesList) Then
            gridList.DataSource = tab_CitiesList
            gridList.DataBind()
        End If
    End Sub

    Protected Sub gridList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridList.PageIndexChanging
        gridList.PageIndex = e.NewPageIndex
        LoadData()
    End Sub
End Class