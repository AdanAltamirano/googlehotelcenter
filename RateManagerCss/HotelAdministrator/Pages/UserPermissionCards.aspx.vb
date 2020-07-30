Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class UserPermissionCards
    Inherits PaginaBase

    Property LastSearchedUser() As String
        Get
            Return ViewState("_LastSearchedUser")
        End Get
        Set(ByVal value As String)
            ViewState("_LastSearchedUser") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            If Grid.Items.Count > 0 Then
                btnGuardar.Visible = True
            End If
        End If
        If Not Page.IsPostBack Then
            btnBuscar.Text = PortalCulture.GetString("M0BT0000115")
            btnGuardar.Text = PortalCulture.GetString("00008")
            lblNombreUsuario.Text = PortalCulture.GetString("M0UT02706")
            lblTitulo.Text = PortalCulture.GetString("M0UT02707")

            MyBase.guardalog("/HotelAdministrator/Pages/UserPermissionCards.aspx", acciones.Ver, "Accedió al módulo")
        End If

    End Sub
    Enum gridview
        idusuario
        Email
        Nombre
        Permission
        CompanyId
        AllowSeeCc
    End Enum

    Private dsCompanies As DataSet

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Me.Grid.CurrentPageIndex = 0
        Me.Grid.SelectedIndex = -1
        LoadCompanies()
    End Sub

    Private Sub LoadCompanies()
        dsCompanies = GetCompanies(Me.txtEmailSearch.Text, Me.txtHotelSearch.Text)
        ltlUserName.Text = Me.txtEmailSearch.Text
        If Not dsCompanies Is Nothing Then

            Me.Grid.DataSource = dsCompanies
            Me.Grid.DataBind()

            Dim chk As CheckBox
            Dim userName As String
            Dim CompanyIdx As Integer
            For Each item As DataGridItem In Grid.Items

                CompanyIdx += 1
            Next
        End If
        btnGuardar.Visible = True
    End Sub

    Private Function GetCompanies(ByVal EmailSearch As String, ByVal companySearch As String) As DataSet
        Dim dRes As New DataSet
        Try
            Dim conection As New SqlConnection(AppSettings("PortalConnectionString"))
            Dim spname As String = "GetUsersCompanies"
            Dim command As New SqlCommand(spname, conection)
            Dim idUsuario = CType(Me.Page, PaginaBase).Usuario

            With command
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@UserEmail", EmailSearch))
                .Parameters.Add(New SqlParameter("@CompanyName", companySearch))

            End With
            Dim adapter As New SqlDataAdapter(command)

            adapter.Fill(dRes)
        Catch ex As Exception

        End Try
        If Not LastSearchedUser = EmailSearch Then
            MyBase.guardalog("/HotelAdministrator/Pages/UserPermissionCards.aspx", acciones.Ver, "Consulto permisos del usuario " & EmailSearch)
        End If
        LastSearchedUser = EmailSearch
        Return dRes
    End Function

    Private Function GetUserCompanyAllowCC() As DataSet
        Dim dRes As New DataSet
        Try
            Dim conection As New SqlConnection(AppSettings("PortalConnectionString"))
            Dim spname As String = "SELECT * FROM UsersCompany_AllowSeeCC WHERE UserId =" & Grid.DataKeys(0)
            Dim command As New SqlCommand(spname, conection)
            Dim idUsuario = CType(Me.Page, PaginaBase).Usuario

            With command
                .CommandType = CommandType.Text
            End With
            Dim adapter As New SqlDataAdapter(command)

            adapter.Fill(dRes)
        Catch ex As Exception

        End Try

        Return dRes
    End Function

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim conection As New SqlConnection(AppSettings("PortalConnectionString"))
        Try
            Dim dsAllowCC As New DataSet

            'SELECT COMMAND
            Dim spname As String = "SELECT * FROM UsersCompany_AllowSeeCC WHERE UserId =" & Grid.DataKeys(0)
            Dim command As New SqlCommand(spname, conection)
            With command
                .CommandType = CommandType.Text
            End With
            Dim adapter As New SqlDataAdapter()
            adapter.SelectCommand = command

            adapter.InsertCommand = New SqlCommand("InsertUserCompanyAllowSeeCC", conection)
            With adapter.InsertCommand
                .Parameters.Add("@CompanyId", SqlDbType.Int)
                .Parameters.Add("@UserId", SqlDbType.Int)

                .Parameters.Item("@CompanyId").SourceColumn = "CompanyId"
                .Parameters.Item("@UserId").SourceColumn = "UserId"

                .CommandType = CommandType.StoredProcedure
            End With

            'DELETE COMMAND
            adapter.DeleteCommand = New SqlCommand("DeleteUserCompanyAllowSeeCC", conection)
            With adapter.DeleteCommand
                .Parameters.Add("@CompanyId", SqlDbType.Int)
                .Parameters.Add("@UserId", SqlDbType.Int)

                .Parameters.Item("@CompanyId").SourceColumn = "CompanyId"
                .Parameters.Item("@UserId").SourceColumn = "UserId"

                .CommandType = CommandType.StoredProcedure
            End With

            adapter.Fill(dsAllowCC)

            Dim chk As CheckBox
            If Not dsAllowCC Is Nothing AndAlso dsAllowCC.Tables.Count > 0 Then
                Dim strHotelsName As String = String.Empty
                For Each item As DataGridItem In Grid.Items
                    chk = CType(item.Cells(gridview.Permission).FindControl("chkAdd"), CheckBox)
                    Dim ccRows() As DataRow = dsAllowCC.Tables(0).Select("CompanyId = " & CInt(item.Cells(gridview.CompanyId).Text))
                    If ccRows.Count > 0 Then
                        If Not chk.Checked Then
                            ccRows(0).Delete()
                        End If
                    Else
                        If chk.Checked Then
                            Dim newRow As DataRow = dsAllowCC.Tables(0).NewRow()
                            newRow.Item("CompanyId") = CInt(item.Cells(gridview.CompanyId).Text)
                            newRow.Item("UserId") = CInt(Grid.DataKeys(0))

                            dsAllowCC.Tables(0).Rows.Add(newRow)
                            strHotelsName = strHotelsName & item.Cells(gridview.Nombre).Text & ", "
                        End If
                    End If
                Next
                If Not String.IsNullOrWhiteSpace(strHotelsName) Then
                    strHotelsName = strHotelsName.Substring(0, strHotelsName.Length - 1)
                    MyBase.guardalog("/HotelAdministrator/Pages/UserPermissionCards.aspx", acciones.Modificar, "Dio permiso al usuario " & txtEmailSearch.Text & " para " & strHotelsName)
                End If
            End If

            adapter.Update(dsAllowCC)
            dsAllowCC.AcceptChanges()
        Catch ex As Exception
            lblMensajes.Text = ex.Message
        Finally
            If Not conection Is Nothing Then
                If conection.State = ConnectionState.Open Or conection.State = ConnectionState.Broken Then
                    conection.Close()
                End If
            End If
            LoadCompanies()
        End Try
    End Sub

    Protected Sub Grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Grid.PageIndexChanged
        Me.Grid.CurrentPageIndex = e.NewPageIndex
        Me.Grid.SelectedIndex = -1
        LoadCompanies()
    End Sub

    Protected Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(gridview.Nombre).Text = PortalCulture.GetString("00073")
            e.Item.Cells(gridview.Email).Text = PortalCulture.GetString("00163")
            e.Item.Cells(gridview.Permission).Text = PortalCulture.GetString("00470")
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim UserName As String
            Dim chk As CheckBox

            chk = CType(e.Item.Cells(gridview.Permission).FindControl("chkAdd"), CheckBox)
            If Not chk Is Nothing Then
                chk.Checked = e.Item.Cells(gridview.AllowSeeCc).Text = "1"
            End If
        End If
    End Sub
End Class