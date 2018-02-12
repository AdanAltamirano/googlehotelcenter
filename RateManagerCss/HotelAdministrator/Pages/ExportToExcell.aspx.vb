Public Partial Class ExportToExcell
    Inherits System.Web.UI.Page

    Function CreateGrid(ByRef ds As DataSet) As Boolean
        Dim CExport As CExportExcell
        Dim _NameValue As New NameValueCollection
        Dim str As String()
        Dim bc As BoundColumn

        Try
            CExport = CType(Session("__CExport__"), CExportExcell)
            ds = CExport.DataSource
            _NameValue = CExport.GetColumns

            Me.Grid.AutoGenerateColumns = False
            For i As Integer = 0 To _NameValue.Count - 1
                str = Split(_NameValue.Keys(i), "|")
                If str.Length > 0 Then
                    bc = New BoundColumn
                    bc.HeaderText = _NameValue.Item(i)
                    bc.DataField = str(0)
                    If str.Length >= 2 Then
                        bc.DataFormatString = String.Format("{0}", str(1))
                    End If
                    Me.Grid.Columns.Add(bc)
                End If
            Next

        Catch ex As Exception
        End Try
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim ds As DataSet = Nothing
        Try
            If Not IsPostBack Then
                CreateGrid(ds)
               
                Response.Clear()
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default

                Response.ContentType = "application/vnd.ms-excel"
                Dim stringWrite As New System.IO.StringWriter
                Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
                With Grid
                    .DataSource = ds ' Session("dvRes") 'ds.PaymentComisions
                    .DataBind()
                    .RenderControl(htmlWrite)
                End With
                Response.Write(stringWrite.ToString)
            End If
        Catch ex As Exception
            Response.Clear()
        Finally
            Response.End()
        End Try
    End Sub

    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            '  e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")

        End If
    End Sub

End Class