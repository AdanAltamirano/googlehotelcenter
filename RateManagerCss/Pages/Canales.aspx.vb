Imports System
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Partial Class Canales
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Propiedades"
    Public Property idCanal() As Integer
        Get
            Return viewstate("idCanal")
        End Get
        Set(ByVal Value As Integer)
            viewState("idCanal") = Value
        End Set
    End Property
#End Region

    Protected WithEvents CtrlIdiomaNombre As CtrlIdioma
    Protected WithEvents CtrlIdiomaDescripcion As CtrlIdioma
    Protected WithEvents CtlMensajes1 As ctlMensajes

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            LoadFeeds()
            LimpiarControles()
        End If
        CtrlIdiomaNombre.IsHTML = False
        CtrlIdiomaNombre.RequiredText = False
        CtrlIdiomaDescripcion.IsHTML = False
        CtrlIdiomaDescripcion.RequiredText = False
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", pnlData.ClientID, cmdNew.ClientID, "true"))
    End Sub
    Private Sub LoadFeeds()
        Dim dsCanales As DataSet = New DataSet
        dsCanales = GetFeeds(PortalCulture.GetIDCulture())
        If Not dsCanales Is Nothing AndAlso dsCanales.Tables(0).Rows.Count > 0 Then
            dgCanales.DataSource = dsCanales.Tables(0).DefaultView
            dgCanales.DataBind()
        End If
    End Sub
#Region "Metodos"
    Private Function GetFeeds(ByVal idIdioma As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As New DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetCanales", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@idIdioma", idIdioma)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)            
        Catch ex As Exception
        End Try
        Return ds
    End Function

    Private Function getFeedById(ByVal idCanal As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As New DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spgetFeedById", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@idCanal", idCanal)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)

        Catch ex As Exception
        End Try
        Return ds
    End Function

    Private Function CheckNotesInFeed(ByVal idCanal As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As New DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spCheckNotesInFeed", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@idCanal", idCanal)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)

        Catch ex As Exception
        End Try
        Return ds
    End Function

    Public Function DelFeed(ByVal idCanal As Integer) As Boolean
        Dim conStr As String = AppSettings("PortalConnectionString")
        Dim sqlCon As SqlConnection = New SqlConnection(conStr)
        Dim sqlCommand As SqlCommand = New SqlCommand("SpDelFeed", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.Add("@idCanal", SqlDbType.Int).Value = idCanal
        Try
            sqlCommand.Connection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            Return False
        Finally
            If Not sqlCommand Is Nothing Then
                If Not sqlCommand.Connection Is Nothing Then
                    sqlCommand.Connection.Close()
                    sqlCommand.Connection.Dispose()
                End If
            End If
            sqlCommand.Dispose()
        End Try
        Return True
    End Function

    Public Function InsertFeed(ByVal title_es As String, ByVal title_en As String, ByVal link As String, ByVal descripcion_es As String, ByVal descripcion_en As String) As Boolean
        Dim conStr As String = AppSettings("PortalConnectionString")
        Dim sqlCon As SqlConnection = New SqlConnection(conStr)
        Dim sqlCommand As SqlCommand = New SqlCommand("spInsertFeed", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.Add("@title_es", SqlDbType.VarChar).Value = title_es
        sqlCommand.Parameters.Add("@title_en", SqlDbType.VarChar).Value = title_en
        sqlCommand.Parameters.Add("@link", SqlDbType.VarChar).Value = link
        sqlCommand.Parameters.Add("@descripcion_es", SqlDbType.VarChar).Value = descripcion_es
        sqlCommand.Parameters.Add("@descripcion_en", SqlDbType.VarChar).Value = descripcion_en
        Try
            sqlCommand.Connection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            Return False
        Finally
            If Not sqlCommand Is Nothing Then
                If Not sqlCommand.Connection Is Nothing Then
                    sqlCommand.Connection.Close()
                    sqlCommand.Connection.Dispose()
                End If
            End If
            sqlCommand.Dispose()
        End Try
        Return True
    End Function

    Public Function UpdFeed(ByVal idCanal As Integer, ByVal title_es As String, ByVal title_en As String, ByVal link As String, ByVal descripcion_es As String, ByVal descripcion_en As String) As Boolean
        Dim conStr As String = AppSettings("PortalConnectionString")
        Dim sqlCon As SqlConnection = New SqlConnection(conStr)
        Dim sqlCommand As SqlCommand = New SqlCommand("spUpdFeed", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.Add("@idCanal", SqlDbType.Int).Value = idCanal
        sqlCommand.Parameters.Add("@title_es", SqlDbType.VarChar).Value = title_es
        sqlCommand.Parameters.Add("@title_en", SqlDbType.VarChar).Value = title_en
        sqlCommand.Parameters.Add("@link", SqlDbType.VarChar).Value = link
        sqlCommand.Parameters.Add("@descripcion_es", SqlDbType.VarChar).Value = descripcion_es
        sqlCommand.Parameters.Add("@descripcion_en", SqlDbType.VarChar).Value = descripcion_en
        Try
            sqlCommand.Connection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            Return False
        Finally
            If Not sqlCommand Is Nothing Then
                If Not sqlCommand.Connection Is Nothing Then
                    sqlCommand.Connection.Close()
                    sqlCommand.Connection.Dispose()
                End If
            End If
            sqlCommand.Dispose()
        End Try
        Return True
    End Function

    Private Sub LimpiarControles()
        CtrlIdiomaNombre.Limpia()
        CtrlIdiomaDescripcion.Limpia()
        TextBoxLink.Text = String.Empty
        Me.idCanal = -1
        lblMsgActualizacion.Visible = False
        lblMsgTitle.Visible = False
        cmdNew.Style.Add("display", "")
        pnlData.Style.Add("display", "none")

    End Sub

    Private Sub EliminarCanal(ByVal idCanal As Integer)
        'Comprobamos que no tenga noticias Asociadas
        Dim dsNotas As DataSet
        dsNotas = CheckNotesInFeed(idCanal)
        If Not dsNotas Is Nothing AndAlso dsNotas.Tables(0).Rows.Count > 0 Then
            'Existen Notas Asociadas al Canal
            lblMsgActualizacion.Text = "No se puede Eliminar el canal, tiene Noticias asociadas"
        Else
            If DelFeed(idCanal) Then
                lblMsgActualizacion.Text = "Se elimino el canal"
                If dgCanales.CurrentPageIndex > 0 And dgCanales.Items.Count = 1 Then
                    dgCanales.CurrentPageIndex = ((dgCanales.CurrentPageIndex * dgCanales.PageSize) \ dgCanales.PageSize) - 1
                End If
                dgCanales.SelectedIndex = -1
            Else
                lblMsgActualizacion.Text = "Ocurrio un error al Eliminar el Canal"
            End If
        End If
    End Sub
#End Region

    Private Sub dgCanales_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCanales.ItemCommand
        Call LimpiarControles()
        If e.CommandName = "Select" Then
            Try
                Me.idCanal = CType(e.Item.Cells(1).Text, Integer)
                Dim dsCanal As DataSet
                dsCanal = getFeedById(Me.idCanal)
                If Not dsCanal Is Nothing AndAlso dsCanal.Tables(0).Rows.Count > 0 Then
                    CtrlIdiomaNombre.setES(dsCanal.Tables(0).Rows(0)("title_es"))
                    CtrlIdiomaNombre.SetEN(dsCanal.Tables(0).Rows(0)("title_en"))
                    CtrlIdiomaDescripcion.setES(dsCanal.Tables(0).Rows(0)("description_es"))
                    CtrlIdiomaDescripcion.SetEN(dsCanal.Tables(0).Rows(0)("description_en"))
                    TextBoxLink.Text = dsCanal.Tables(0).Rows(0)("link")
                End If
                pnlData.Style.Add("display", "")
                cmdNew.Style.Add("display", "none")

            Catch ex As Exception
                Me.idCanal = -1
                Exit Sub
            End Try
        ElseIf e.CommandName = "Eliminar" Then
            Try
                Me.idCanal = CType(e.Item.Cells(1).Text, Integer)
                EliminarCanal(Me.idCanal)
                LimpiarControles()
                LoadFeeds()
                lblMsgActualizacion.Visible = True
            Catch ex As Exception
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        LimpiarControles()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim title_es As String = String.Empty
        Dim title_en As String = String.Empty        
        Dim Link As String = TextBoxLink.Text.Trim
        Dim descripcion_es As String = String.Empty
        Dim descripcion_en As String = String.Empty

        If Not CtrlIdiomaNombre.GetES Is Nothing Then
            title_es = CtrlIdiomaNombre.GetES.Trim
        Else
            lblMsgTitle.Visible = True
            Exit Sub
        End If
        If Not CtrlIdiomaNombre.GetEN Is Nothing Then
            title_en = CtrlIdiomaNombre.GetEN.Trim
        Else
            lblMsgTitle.Visible = True
            Exit Sub
        End If


        If Not CtrlIdiomaDescripcion.GetES Is Nothing Then
            descripcion_es = CtrlIdiomaDescripcion.GetES.Trim
        End If
        If Not CtrlIdiomaDescripcion.GetEN Is Nothing Then
            descripcion_en = CtrlIdiomaDescripcion.GetEN.Trim
        End If
        If title_es = String.Empty Or title_en = String.Empty Then
            lblMsgTitle.Visible = True
        End If

        If Me.idCanal <> -1 Then
            'Edicion del canal
            If UpdFeed(Me.idCanal, title_es, title_en, Link, descripcion_es, descripcion_en) Then
                lblMsgActualizacion.Text = "Se ha actualizado el canal"
                LimpiarControles()
                LoadFeeds()
            Else
                lblMsgActualizacion.Text = "Ocurrio un error al actualizar el canal"
            End If
        Else
            'Nuevo Canal
            If InsertFeed(title_es, title_en, Link, descripcion_es, descripcion_en) Then
                lblMsgActualizacion.Text = "Se ha agregado el Canal"
                LimpiarControles()
                LoadFeeds()
            Else
                lblMsgActualizacion.Text = "Ocurrio un error al agregar el canal"
            End If
        End If
        lblMsgActualizacion.Visible = True
    End Sub

    Private Sub dgCanales_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCanales.PageIndexChanged
        dgCanales.CurrentPageIndex = e.NewPageIndex
        dgCanales.SelectedIndex = -1
        LoadFeeds()
    End Sub

    Private Sub dgCanales_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCanales.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim LK As HyperLink
            Dim LK2 As LinkButton
            LK = e.Item.Cells(3).FindControl("lnkEliminar")
            LK.Text = PortalCulture.GetString("00103")
            LK2 = e.Item.Cells(3).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")
            LK2 = e.Item.Cells(3).FindControl("lnkEliminar2")
            LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("01114"), _
            PortalCulture.GetString("01137"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00073")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            'e.Item.Cells(dgcolumns.eliminar).Text = CType(dgRatePlans.DataSource, DataSet).Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.cmdNew.Value = PortalCulture.GetString("00102")
    End Sub

End Class
