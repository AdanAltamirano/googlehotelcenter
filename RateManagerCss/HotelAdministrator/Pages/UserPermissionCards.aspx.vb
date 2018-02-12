Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class UserPermissionCards 
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            If Grid.Items.Count > 0 Then
                btnGuardar.Visible = True
            End If
        End If
        btnBuscar.Text = PortalCulture.GetString("M0BT0000115")
        btnGuardar.Text = PortalCulture.GetString("00008")
        lblNombreUsuario.Text = PortalCulture.GetString("M0UT02706")
        lblTitulo.Text = PortalCulture.GetString("M0UT02707")
    End Sub
    Dim ds As DataSet
    Enum gridview
        idusuario
        Email
        Nombre
        Permission
    End Enum
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Me.Grid.CurrentPageIndex = 0
        Me.Grid.SelectedIndex = -1
        ds = getCompanys(Me.txtEmailSearch.Text)
        ltlUserName.Text = Me.txtEmailSearch.Text
        Dim listaIdentificadores As New List(Of String)
        If Not ds Is Nothing Then
            For Each row As DataRow In ds.Tables(0).Rows
                If listaIdentificadores.Contains(row("idusuario").ToString().Trim) Then
                    row.Delete()
                Else
                    listaIdentificadores.Add(row("idusuario").ToString().Trim)
                End If
            Next
            ds.AcceptChanges()
            Me.Grid.DataSource = ds
            Me.Grid.DataBind()

            Dim chk As CheckBox
            Dim userName As String
            For Each item As DataGridItem In Grid.Items
                userName = item.Cells(gridview.Email).Text
                chk = CType(item.Cells(gridview.Permission).FindControl("chkAdd"), CheckBox)
                Dim drow() As DataRow = ds.Tables(0).Select("email = '" + userName + "'")
                If drow(0)("verDatosTarjeta").ToString.ToLower = "true" Then
                    chk.Checked = True
                Else
                    chk.Checked = False
                End If
            Next
        End If
        btnGuardar.Visible = True
        txtEmailSearch.Text = String.Empty
    End Sub

    Private Function getCompanys(ByVal EmailSearch As String) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConnectionString"))
        'Dim command As New SqlCommand("spCompanySearchCompanys", conection)
        Dim spname As String = "spUserGetUserByEmailOrName" 'IIf(isAsoc, "spCompanySearchCompanysAssociation", "spCompanySearchCompanys")
        Dim command As New SqlCommand(spname, conection)
        Dim idUsuario = CType(Me.Page, PaginaBase).Usuario
        Dim idAsociacionHotel As Integer = CType(Me.Page, PaginaBase).GetIdAsociation

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@email", EmailSearch)) 'Me.txtEmailSearch.Text
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim indice As Integer = 0
        Dim chk As CheckBox
        Dim listaIdentificadores As New List(Of String)
        ds = getCompanys(ltlUserName.Text)
        Try
            If Not ds Is Nothing Then
                For Each row As DataRow In ds.Tables(0).Rows
                    If listaIdentificadores.Contains(row("idusuario").ToString().Trim) Then
                        row.Delete()
                    Else
                        listaIdentificadores.Add(row("idusuario").ToString().Trim)
                    End If
                Next
                ds.AcceptChanges()
            End If
            Dim dtable As DataTable = ds.Tables(0).Clone()

            For Each item As DataGridItem In Grid.Items
                Dim iduser As String = Grid.DataKeys(indice)
                chk = CType(item.Cells(gridview.Permission).FindControl("chkAdd"), CheckBox)
                Dim dtrow() As DataRow = ds.Tables(0).Select("idusuario = '" + iduser + "'")
                If Not dtrow Is Nothing AndAlso dtrow(0)("verDatosTarjeta").ToString.ToLower <> chk.Checked.ToString.ToLower Then
                    dtrow(0)("verDatosTarjeta") = chk.Checked
                    Dim row As DataRow = dtable.NewRow()
                    row(0) = dtrow(0)(0)
                    row(1) = dtrow(0)(1)
                    row(2) = dtrow(0)(2)
                    row(3) = dtrow(0)(3)
                    dtable.Rows.Add(row)
                End If
                indice += 1
            Next
            dtable.AcceptChanges()
            If dtable.Rows.Count > 0 Then
                'guardar en la base de datos
                If UpdateDB(dtable) = False Then
                    lblMensajes.Text = PortalCulture.GetString("00189")
                End If
            End If
        Catch
            Me.lblMensajes.Text = PortalCulture.GetString("00189")
        End Try
    End Sub

    Private Function UpdateDB(ByVal dt As DataTable) As Boolean
        Dim conection As New SqlConnection(AppSettings("PortalConnectionString"))
        'Dim command As New SqlCommand("spCompanySearchCompanys", conection)
        Dim spname As String = "UserUpdateDatosTarjetaByIdUsuario" 'IIf(isAsoc, "spCompanySearchCompanysAssociation", "spCompanySearchCompanys")
        Dim command As New SqlCommand(spname, conection)
        Dim myTrans As SqlTransaction
        conection.Open()
        myTrans = conection.BeginTransaction()
        Try
            command.CommandType = CommandType.StoredProcedure
            command.Transaction = myTrans
            For Each row As DataRow In dt.Rows
                command.Parameters.Clear()
                command.Parameters.Add(New SqlParameter("@idusuario", row("idusuario")))
                command.Parameters.Add(New SqlParameter("@verDatosTarjeta", row("verDatosTarjeta")))
                command.ExecuteNonQuery()
            Next
            myTrans.Commit()
            conection.Close()
            Return True
        Catch
            myTrans.Rollback()
            conection.Close()
            Return False
        End Try
    End Function

    Protected Sub Grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Grid.PageIndexChanged
        Me.Grid.CurrentPageIndex = e.NewPageIndex
        Me.Grid.SelectedIndex = -1
        ds = getCompanys(Me.ltlUserName.Text)
        Dim listaIdentificadores As New List(Of String)
        If Not ds Is Nothing Then
            For Each row As DataRow In ds.Tables(0).Rows
                If listaIdentificadores.Contains(row("idusuario").ToString().Trim) Then
                    row.Delete()
                Else
                    listaIdentificadores.Add(row("idusuario").ToString().Trim)
                End If
            Next
            ds.AcceptChanges()
            Me.Grid.DataSource = ds
            Me.Grid.DataBind()
        End If
    End Sub

    Protected Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(gridview.Nombre).Text = PortalCulture.GetString("00073")
            e.Item.Cells(gridview.Email).Text = PortalCulture.GetString("00163")
            e.Item.Cells(gridview.Permission).Text = PortalCulture.GetString("00470")
        End If
    End Sub
End Class