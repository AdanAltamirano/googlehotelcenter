Public Partial Class DestinationSkipped
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridRel_Update()
    End Sub

    Private Sub GridRel_Update()
        Dim tab_citiesList As New HotelBedsDS.CitiesListDataTable()
        If New HotelBedsRules().Select_Cities_SkippedByAll(tab_citiesList) Then
            gridList.DataSource = tab_citiesList
            gridList.DataBind()
        End If
    End Sub

    Private Sub GridRel_Search()
        Dim tab_CitiesList As New HotelBedsDS.CitiesListDataTable
        If New HotelBedsRules().Select_Cities_SkippedByKey(txtSearch.Text.Trim(), tab_CitiesList) Then
            gridList.DataSource = tab_CitiesList
            gridList.DataBind()
        End If
    End Sub

    Protected Sub gridList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridList.PageIndexChanging
        gridList.PageIndex = e.NewPageIndex
        GridRel_Search()
    End Sub

    Protected Sub gridList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridList.RowCommand
        If e.CommandName = "Res" Then

            If txtSearch.Text = String.Empty Then
                GridRel_Update()
            Else
                GridRel_Search()
            End If

        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        GridRel_Search()
    End Sub
End Class