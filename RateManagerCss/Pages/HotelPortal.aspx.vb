Imports System.Data.SqlClient

Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos
Imports Portal.Catalogos.Facade
Imports Portal.Hotel
Imports Portal.General.Common.Data
Imports Portal.General.Facade


Partial Public Class HotelPortal
    Inherits PaginaBase
    Private msError As String = ""

    Private Enum dgcolumns
        chkPortal
        idAplicacionPortal
        IdPortal
        idIdioma
        Nombre
        lstIdioma
    End Enum

    Private Sub ShowError()
        Me.lblError.Visible = (msError.Trim.Length > 0)
        Me.lblError.Text = msError
    End Sub

    Private Sub loadCulture()
        lblTitle.Text = PortalCulture.GetString("01420")
        btnSave.Text = PortalCulture.GetString("M000060")
    End Sub

    Private Function LeePortales() As DataSet
        Dim ds As New DataSet()
        Try
            With New SqlDataAdapter("spContenidoPortalByHotel", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.Parameters.Add("@idHotel", SqlDbType.Int).Value = cInfoActual.Hotel
                Try
                    .SelectCommand.Connection.Open()
                    .Fill(ds)
                Catch ex As Exception
                Finally
                    .SelectCommand.Connection.Close()
                End Try
            End With

        Catch ex As Exception
            ds = New DataSet()
        End Try
        Return ds
    End Function

    Sub LoadPortals()
        Dim ds As New DataSet()
        ds = LeePortales()
        If Not dsEmpty(ds) Then
            dgPortals.DataSource = ds
            dgPortals.DataBind()
        End If
    End Sub

    Function AddRow(ByVal Id As Integer, ByVal idPortal As Integer, ByVal idIdioma As Integer, ByVal ds As clsCommonContenidoPortal) As Boolean
        Dim dr As DataRow

        dr = ds.Tables(clsCommoncontenidoportal.TABLA).NewRow
        dr(clsCommoncontenidoportal.FIELD_idaplicacionptl) = ID
        dr(clsCommoncontenidoportal.FIELD_IdHotel) = cInfoActual.Hotel
        dr(clsCommoncontenidoportal.FIELD_IdPortal) = idPortal
        dr(clsCommoncontenidoportal.FIELD_idIdioma) = idIdioma
        ds.Tables(clsCommoncontenidoportal.TABLA).Rows.Add(dr)
    End Function

    Private Function GetInsertCommand(ByVal sqlconn As SqlConnection) As SqlCommand
        Dim insertCommand As SqlCommand

        insertCommand = New SqlCommand("spContenidoPortal_Insertar", sqlconn)
        insertCommand.CommandType = CommandType.StoredProcedure
        With insertCommand.Parameters
            .Add(New SqlParameter("@idAplicacionPortal", SqlDbType.Int))
            .Add(New SqlParameter("@idHotel", SqlDbType.Int))
            .Add(New SqlParameter("@idPortal", SqlDbType.Int))
            .Add(New SqlParameter("@idIdioma", SqlDbType.Int))

            .Item("@idAplicacionPortal").SourceColumn = clsCommoncontenidoportal.FIELD_idaplicacionptl
            .Item("@idHotel").SourceColumn = clsCommoncontenidoportal.FIELD_IdHotel
            .Item("@idPortal").SourceColumn = clsCommoncontenidoportal.FIELD_IdPortal
            .Item("@idIdioma").SourceColumn = clsCommoncontenidoportal.FIELD_idIdioma
        End With
        Return insertCommand
    End Function

    Public Function insertarAplicacionPortal(ByVal ds As clsCommoncontenidoportal) As Boolean
        Dim dsCommand As New SqlDataAdapter
        Dim connectionString As String
        connectionString = ConfigurationManager.AppSettings("HotelConnection")
        Dim sqlConn As New SqlConnection(connectionString)

        Try
            sqlConn.Open()
            dsCommand.InsertCommand = GetInsertCommand(sqlConn)
            dsCommand.Update(ds, clsCommoncontenidoportal.TABLA)

        Catch ex As Exception
            If sqlConn.State = ConnectionState.Open Or sqlConn.State = ConnectionState.Broken Then
                sqlConn.Close()
            End If
            Return False
        Finally
            If sqlConn.State = ConnectionState.Open Or sqlConn.State = ConnectionState.Broken Then
                sqlConn.Close()
            End If
        End Try
    End Function

    Sub SalvarPortal()
        Dim ds As New clsCommoncontenidoportal
        Dim tmpChk As CheckBox
        Dim ddList As Global.System.Web.UI.WebControls.DropDownList
        Dim idportal As Integer
        Dim isAdd As Boolean = False

        Try
            '// No se selecciono nada, va a eliminar las
            AddRow(0, 0, 0, ds)
            For i As Integer = 0 To dgPortals.Items.Count - 1
                tmpChk = dgPortals.Items(i).FindControl("chkPortal")
                ddList = dgPortals.Items(i).FindControl("ddlIdioma")
                Integer.TryParse(dgPortals.Items(i).Cells(dgcolumns.IdPortal).Text, idportal)
                If Not tmpChk Is Nothing Then
                    If tmpChk.Checked Then
                        If ddList.SelectedIndex > 0 Then
                            AddRow(0, idportal, ddList.SelectedItem.Value, ds)
                            isAdd = True
                        End If
                    End If
                End If
            Next

            '// Guarda en la db.
            insertarAplicacionPortal(ds)
            LoadPortals()
        Catch ex As Exception
            msError = PortalCulture.GetString("M0BT0000050")
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            LoadPortals()
        End If
        Me.btnSave.OnClientClick = String.Format("return FireIdioma('{0}');", dgPortals.ClientID)
    End Sub

    Private Sub HotelPortal_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        loadCulture()
        ShowError()
    End Sub

    Private Sub LoadIdioma(ByRef Ctrl As System.Web.UI.WebControls.DropDownList)
        Ctrl.Items.Add(New ListItem(PortalCulture.GetString("M000272"), 0))
        Ctrl.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000080"), 1))
        Ctrl.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000081"), 2))
    End Sub

    Private Sub dgPortals_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPortals.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Nombre).Text = PortalCulture.GetString("M000025")
            e.Item.Cells(dgcolumns.lstIdioma).Text = PortalCulture.GetString("M000248")
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            If dgPortals.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgPortals.CurrentPageIndex < dgPortals.PageCount - 1 Then
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

    Private Sub dgPortals_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPortals.ItemDataBound
        Dim tmpChk As CheckBox
        Dim ddList As Global.System.Web.UI.WebControls.DropDownList
        Dim i As Short = 0
        Dim idioma As Integer

        If e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            tmpChk = e.Item.FindControl("chkPortal")
            Integer.TryParse(e.Item.Cells(dgcolumns.idIdioma).Text, idioma)
            If Not tmpChk Is Nothing Then
                Short.TryParse(e.Item.Cells(dgcolumns.idAplicacionPortal).Text, i)
                tmpChk.Checked = If(i > 0, True, False)
            End If
            ddList = e.Item.FindControl("ddlIdioma")
            If Not ddList Is Nothing Then
                LoadIdioma(ddList)
                ddList.SelectedIndex = idioma
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        SalvarPortal()
    End Sub

    <System.ComponentModel.DesignerCategory("Code"), SerializableAttribute()> _
    Public Class clsCommonContenidoPortal
        Inherits DataSet

        Public Const TABLA As String = "contenidoportal"
        Public Const FIELD_idaplicacionptl As String = "idaplicacionportal"
        Public Const FIELD_IdHotel As String = "IdHotel"
        Public Const FIELD_IdPortal As String = "IdPortal"
        Public Const FIELD_idIdioma As String = "idIdioma"

        Public Sub New()
            MyBase.New()
            buidTable()
        End Sub

        Private Sub buidTable()
            Dim table As DataTable = New DataTable(TABLA)
            With table.Columns
                .Add(FIELD_idaplicacionptl, GetType(System.Int32))
                .Add(FIELD_IdHotel, GetType(System.Int32))
                .Add(FIELD_IdPortal, GetType(System.Int32))
                .Add(FIELD_idIdioma, GetType(System.Int32))
            End With

            Me.Tables.Add(table)
        End Sub

    End Class

    Private Sub dgPortals_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPortals.PageIndexChanged
        dgPortals.CurrentPageIndex = e.NewPageIndex
        dgPortals.SelectedIndex = -1
        LoadPortals()
    End Sub
End Class