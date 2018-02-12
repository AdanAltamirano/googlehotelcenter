Public Class ExcelWaitListReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Response.Clear()
            Response.Buffer = True
            Response.ClearHeaders()
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("content-disposition", "attachment;filename=ReservationsReport_" + DateTime.Now.ToShortDateString().ToString() + ".xls")
            Response.Charset = ""
            Response.ContentEncoding = System.Text.Encoding.Default
            Response.ContentType = "application/ms-excel"

            Dim dvReport As DataView = CType(Session("dvReport"), DataView)
            Dim stringWrite As New System.IO.StringWriter
            Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)

            With Grid
                .DataSource = dvReport
                .DataBind()
                .RenderControl(htmlWrite)
            End With
            Response.Write(stringWrite)
        Catch ex As Exception
            Response.Clear()
        Finally
            Response.Clear()
        End Try
        
    End Sub

    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim data As Data.DataRowView = Convert.ChangeType(e.Item.DataItem, GetType(Data.DataRowView))

            If data IsNot Nothing Then
                Select Case Convert.ToInt32(data("Status"))
                    Case 2
                        e.Item.Cells(8).Text = "Notificada"
                    Case 4
                        e.Item.Cells(8).Text = "Cancelada"
                    Case 8
                        e.Item.Cells(8).Text = "Expirada"
                End Select
            End If

        End If
    End Sub
End Class