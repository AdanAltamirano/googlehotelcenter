Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class DataClientPermission
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If dgHotels.CurrentPageIndex >= dgHotels.Items.Count \ dgHotels.PageSize Then
            dgHotels.CurrentPageIndex = (dgHotels.Items.Count - 1) \ dgHotels.PageSize
        End If
        dgHotels.SelectedIndex = -1
        CargaGrid()
        btnSave.Visible = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        savePermission()

    End Sub

    Private Sub CargaGrid()
        Dim dsHotels As New DataSet
        dsHotels = getCompanys("", False)
        dgHotels.DataSource = dsHotels
        dgHotels.DataBind()
    End Sub

    Private Sub dgHotels_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHotels.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

        End If
    End Sub

    Private Function getCompanys(ByVal status As String, ByVal isAsoc As Boolean) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        'Dim command As New SqlCommand("spCompanySearchCompanys", conection)
        Dim spname As String = "spCompanySearchCompanys"
        Dim command As New SqlCommand(spname, conection)

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idRubro", 10))
            .Parameters.Add(New SqlParameter("@Nombre", Me.txtName.Text.Trim.Replace("'", "")))
            .Parameters.Add(New SqlParameter("@idEstado", -1))
            .Parameters.Add(New SqlParameter("@idMunicipio", -1))
            .Parameters.Add(New SqlParameter("@idCiudad", -1))
            .Parameters.Add(New SqlParameter("@status", status))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Function savePermission() As Boolean
        Dim Adapter As New SqlDataAdapter
        Dim ds As New DataSet
        Dim table As New DataTable("DataClientPermissions")
        Dim row As DataRow
        Dim connection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim sp As String = "spDataClientPermissioninsert"
        Dim command As New SqlCommand(sp, connection)


        Try
            Adapter.TableMappings.Add("Table", "DataClientPermissions")

            With table.Columns
                .Add("IdHotel", GetType(System.Int32))
                .Add("Permission", GetType(System.Boolean))
            End With

            ds.Tables.Add(table)

            Dim chk As CheckBox = New CheckBox

            For Each Item As DataGridItem In dgHotels.Items
                row = ds.Tables("DataClientPermissions").NewRow
                With Item
                    chk = .Cells(2).FindControl("chkPermission")
                    row.Item("IdHotel") = CInt(.Cells(0).Text)
                    row.Item("Permission") = chk.Checked
                End With

                ds.Tables("DataClientPermissions").Rows.Add(row)
            Next

            With command
                .CommandType = CommandType.StoredProcedure
                .CommandText = sp
                .Parameters.Add(New SqlParameter("@IdHotel", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@Permission", SqlDbType.Bit))

                .Parameters.Item("@IdHotel").SourceColumn = "IdHotel"
                .Parameters.Item("@Permission").SourceColumn = "Permission"
            End With

            Adapter.InsertCommand = command
            Adapter.Update(ds)
            ds.AcceptChanges()

            If ds.HasErrors Then
                ds.Tables(0).GetErrors(0).ClearErrors()
                Return False
            Else
                ds.AcceptChanges()
                Return True
            End If
        Catch ex As Exception

        Finally
            With command.Connection
                If .State = ConnectionState.Broken Or .State = ConnectionState.Open Then
                    .Close()
                End If
            End With
        End Try
    End Function

    Private Sub dgHotels_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgHotels.PageIndexChanged
        dgHotels.CurrentPageIndex = e.NewPageIndex
        CargaGrid()
    End Sub

    Private Sub dgHotels_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHotels.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If Me.dgHotels.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.dgHotels.CurrentPageIndex < Me.dgHotels.PageCount - 1 Then
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

    Private Sub dgHotels_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHotels.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.Cells(3).Text.ToUpper = "TRUE" Then
                Dim chk As CheckBox = New CheckBox
                chk = e.Item.Cells(2).FindControl("chkPermission")
                chk.Checked = True
            End If
        End If
    End Sub

End Class