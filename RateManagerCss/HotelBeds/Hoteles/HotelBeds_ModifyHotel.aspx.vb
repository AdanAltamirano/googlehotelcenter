Public Partial Class HotelBeds_ModifyHotel
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack And Request.QueryString("id") IsNot Nothing Then
            Dim idHotel As Integer
            If Integer.TryParse(Request.QueryString("id"), idHotel) Then
                Dim tab_City As New HotelBedsDS.HotelsDataTable()

                If New HotelBedsRules().Search_UVHotelsByID(idHotel, tab_City) And tab_City.Rows.Count > 0 Then
                    Dim res As String = "IDHotel: " & tab_City(0).ToString() & "<br>"
                    res &= "Nombre: " & tab_City(0).UVName & " <br> "
                    res &= "Ciudad: " & tab_City(0).UVCity & " <br> "
                    res &= "Pais: " & tab_City(0).UVCountry
                    litCity.Text = res
                    ViewState("UVHotelName") = tab_City(0).UVName
                    lblTopCiudad.Text = "Modificando ciudad " & tab_City(0).UVName & " - Listado de destinos hotelbeds relacionados."
                End If

                GridRel_Search(idHotel)
                GridRes_Suggested(idHotel)
            End If
        End If
    End Sub

    Private Sub GridRel_Search(ByVal idHotel As Integer)
        Dim tab_HotelsList As New HotelBedsDS.HotelsDataTable()
        If (New HotelBedsRules().Search_HBHotelsRelByID(idHotel, tab_HotelsList)) Then
            gridHotelRel.DataSource = tab_HotelsList
            gridHotelRel.DataBind()
            If tab_HotelsList.Rows.Count = 0 Then
                lblMsgRel.Text = "No se encontraron hoteles relacionados"
            Else
                lblMsgRel.Text = ""
            End If
        End If
    End Sub

    Private Sub GridRes_Suggested(ByVal idHotel As Integer)
        Dim tab_HotelsList As New HotelBedsDS.HotelsDataTable()
        If New HotelBedsRules().Search_HBHotelsSuggested(idHotel, tab_HotelsList) Then
            gridHotelsRes.DataSource = tab_HotelsList
            gridHotelsRes.DataBind()
            lblMSG_Result.Text = "Destinos HotelBeds sugeridos"
        End If
    End Sub
    Private Sub GridRes_Search(ByVal key As String)
        Dim tab_HotelsList As New HotelBedsDS.HotelsDataTable()
        Dim strError As String = String.Empty
        If New HotelBedsRules().Search_HBHotelsByKey(key, tab_HotelsList, strError) Then
            gridHotelsRes.DataSource = tab_HotelsList
            gridHotelsRes.DataBind()
            If tab_HotelsList.Rows.Count = 0 Then
                lblMSG_Result.Text = "No se encontraron hoteles"
            Else
                lblMSG_Result.Text = ""
            End If
        End If

    End Sub

    Protected Sub gridHotelRes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridHotelsRes.RowCommand
        If e.CommandName = "Add" And Request.QueryString("id") IsNot Nothing Then
            Dim idhotel As Integer
            If Integer.TryParse(Request.QueryString("id"), idhotel) Then
                Dim gvRow As GridViewRow = gridHotelsRes.Rows(Convert.ToInt32(e.CommandArgument))

                If New HotelBedsRules().Insert_HotelFix(idhotel, gvRow.Cells(0).Text) Then

                    If txtSearch.Text = String.Empty Then
                        GridRes_Suggested(idhotel)
                    Else
                        GridRel_Search(txtSearch.Text.Trim())
                    End If
                    GridRel_Search(idhotel)
                End If

            End If
        End If
    End Sub

    Protected Sub gridHotelsRes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridHotelsRes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow And Request.QueryString("id") IsNot Nothing Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)

            If rowView("idHotel").ToString() IsNot "" Then
                e.Row.Cells(5).Text = ""
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = Drawing.Color.LightBlue
                Next
            End If

            If rowView("HBName").ToString() = ViewState("UVHotelName").ToString() Then
                e.Row.Cells(1).Font.Bold = True
            End If


        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        GridRes_Search(txtSearch.Text.Trim())
    End Sub

    Protected Sub gridHotelRel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridHotelRel.RowCommand

    End Sub
End Class