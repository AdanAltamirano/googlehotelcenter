Imports System
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Configuration.ConfigurationManager

Partial Class NoticiasRSS
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'Protected WithEvents CtrlIdiomaFCk_Titulo As CtrlIdiomaFCk
    Protected WithEvents CtrlIdiomaFCk_Descripcion As CtrlIdiomaFCk
    Protected WithEvents CtlMensajes1 As ctlMensajes
    Protected WithEvents CtrlIdiomaFCk_Titulo As CtrlIdioma

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Propiedades"
    Public Property idCanal() As Integer
        Get
            Return CType(viewstate("idFeed"), Integer)
        End Get
        Set(ByVal Value As Integer)
            viewstate("idFeed") = Value
        End Set
    End Property
    Public Property idNota() As Integer
        Get
            Return CType(viewstate("idNota"), Integer)
        End Get
        Set(ByVal Value As Integer)
            viewstate("idNota") = Value
        End Set
    End Property
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            idCanal = -1
            idNota = -1
            cargaCanales()
            CtrlIdiomaFCk_Titulo.IsHTML = False
            CtrlIdiomaFCk_Titulo.RequiredText = False
            LimpiarControles()
            txtDateFromFiltro.Text = Now.ToString("MM/dd/yyyy")
            txtDateToFiltro.Text = Now.AddDays(1).ToString("MM/dd/yyyy")
        End If
        CtrlIdiomaFCk_Titulo.IsHTML = False
        CtrlIdiomaFCk_Titulo.RequiredText = False
        txtDateTo.Style.Add("Display", "none")
        Me.ResizefrmPrincipal()
    End Sub
    Private Sub cargaCanales()
        Dim dsCanales As DataSet = New DataSet
        dsCanales = GetFeeds(PortalCulture.GetIDCulture())
        ddlCanal.DataSource = dsCanales.Tables(0).DefaultView
        ddlCanal.DataTextField = "Title"
        ddlCanal.DataValueField = "idCanal"
        ddlCanal.DataBind()
        Dim item As ListItem = New ListItem(PortalCulture.GetString("M000272"), "-1")
        ddlCanal.Items.Insert(0, item)

        ddlCanalNuevo.DataSource = dsCanales.Tables(0).DefaultView
        ddlCanalNuevo.DataTextField = "Title"
        ddlCanalNuevo.DataValueField = "idCanal"
        ddlCanalNuevo.DataBind()
        ddlCanalNuevo.Items.Insert(0, item)

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Me.idNota = -1
        Call LimpiarControles()
    End Sub
    Private Sub LimpiarControles()
        TextBoxAutor.Text = ""
        TextBoxLink.Text = ""
        CtrlIdiomaFCk_Titulo.Limpia()        
        CtrlIdiomaFCk_Descripcion.Limpia()
        ddlCanalNuevo.SelectedIndex = 0
        ddlCanalNuevo.Enabled = True
        txtDateFrom.Text = Now.ToString("MM/dd/yyyy")

        MsgCanal.Visible = False
        lblMsgTitulo.Visible = False
        lblMsgFecha.Visible = False
        lblMsgUpdateNote.Visible = False
        LabelMsgCanalLoad.Visible = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' Dim str As String
        Dim idCanal As Integer = -1
        If Me.idNota = -1 Then
            idCanal = ddlCanalNuevo.SelectedValue
        Else
            idCanal = Me.idCanal
        End If

        If idCanal = -1 Then
            MsgCanal.Visible = True
            Exit Sub
        Else
            MsgCanal.Visible = False
        End If

        Dim title_Es As String
        If CtrlIdiomaFCk_Titulo.GetES Is Nothing Then
            title_Es = ""
        Else
            title_Es = CtrlIdiomaFCk_Titulo.GetES
        End If

        Dim title_En As String
        If CtrlIdiomaFCk_Titulo.GetEN Is Nothing Then
            title_En = ""
        Else
            title_En = CtrlIdiomaFCk_Titulo.GetEN
        End If

        Dim link As String = TextBoxLink.Text
        Dim pubDate As DateTime
        Try
            pubDate = CType(txtDateFrom.Text, DateTime)
            lblMsgFecha.Visible = False
        Catch ex As Exception
            lblMsgFecha.Visible = True
            Exit Sub
        End Try

        Dim descripcion_Es As String = CtrlIdiomaFCk_Descripcion.textoEspañol
        Dim descripcion_En As String = CtrlIdiomaFCk_Descripcion.textoIngles
        Dim Author As String = TextBoxAutor.Text

        'If (title_Es = String.Empty Or title_Es = "     <p>&nbsp;</p> ") Or (title_En = String.Empty Or title_En = "     <p>&nbsp;</p> ") Then
        If (title_Es = String.Empty Or title_Es = "     <p>&nbsp;</p> ") Then
            lblMsgTitulo.Visible = True
            Exit Sub
        Else
            lblMsgTitulo.Visible = False
        End If

        If txtDateFrom.Text = String.Empty Then
            lblMsgFecha.Visible = True
            Exit Sub
        Else
            lblMsgFecha.Visible = False
        End If

        'If link = String.Empty Then
        'End If
        'If descripcion_Es = String.Empty Then
        '    Exit Sub
        'End If
        'If descripcion_En = String.Empty Then
        '    Exit Sub
        'End If
        'If Author = String.Empty Then
        '    Exit Sub
        'End If

        If Me.idNota = -1 Then
            Dim linkDefault As String = String.Empty
            linkDefault = AppSettings("LinkDefaultNotasRSS")
            If AddNoteRSS(idCanal, title_Es.Trim, title_En.Trim, link, pubDate, descripcion_Es, descripcion_En, Author, linkDefault) Then
                Call LimpiarControles()
                If Me.idCanal <> -1 Then
                    LoadNotesRSS()
                End If
                lblMsgUpdateNote.Text = PortalCulture.GetString("01104", False)
                lblMsgUpdateNote.Visible = True
            Else
                'No se logro agregar la nota
            End If
        Else
            Dim linkDefault As String = String.Empty
            linkDefault = AppSettings("LinkDefaultNotasRSS")
            If UpdNoteRSS(Me.idNota, idCanal, title_Es, title_En, link, pubDate, descripcion_Es, descripcion_En, Author, linkDefault) Then
                Me.idNota = -1
                Call LimpiarControles()
                LoadNotesRSS()
                lblMsgUpdateNote.Text = PortalCulture.GetString("01105", False)
                lblMsgUpdateNote.Visible = True
            Else
                'Error al intentar acutualizar la nota
            End If
        End If

    End Sub
    'Acceso a datos Nota: Cambiar Posteriormente a una DLL
    Private Function GetFeeds(ByVal idIdioma As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As New DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetFeeds", New SqlConnection(AppSettings("PortalConnectionString")))
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

    Private Function LoadNotasRSS(ByVal idCanal As Integer, ByVal idIdioma As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetNotesRSS", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@idCanal", idCanal)
                .Parameters.AddWithValue("@idIdioma", idIdioma)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
            Return ds
        Catch ex As Exception
        End Try
    End Function

    Private Function LoadNoteRSS(ByVal idCanal As Integer, ByVal idNota As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetNoteRSS", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@idCanal", idCanal)
                .Parameters.AddWithValue("@idNota", idNota)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
            Return ds
        Catch ex As Exception
        End Try        
    End Function

    Private Function AddNoteRSS(ByVal idCanal As Integer, ByVal title_Es As String, ByVal title_En As String, ByVal link As String, ByVal pubDate As DateTime, ByVal descripcion_Es As String, ByVal descripcion_En As String, ByVal Author As String, ByVal linkDefault As String) As Boolean
        Dim conStr As String = AppSettings("PortalConnection")
        Dim sqlCon As SqlConnection = New SqlConnection(AppSettings("PortalConnectionString"))
        Dim sqlCommand As SqlCommand = New SqlCommand("spAddNoteRSS", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.AddWithValue("@idCanal", idCanal)
        sqlCommand.Parameters.AddWithValue("@title_Es", title_Es)
        sqlCommand.Parameters.AddWithValue("@title_En", title_En)
        sqlCommand.Parameters.AddWithValue("@link", link)
        sqlCommand.Parameters.AddWithValue("@pubDate", pubDate)
        sqlCommand.Parameters.AddWithValue("@descripcion_Es", descripcion_Es)
        sqlCommand.Parameters.AddWithValue("@descripcion_En", descripcion_En)
        sqlCommand.Parameters.AddWithValue("@Author", Author)
        sqlCommand.Parameters.AddWithValue("@LinkDefault", linkDefault)

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

    Public Function UpdNoteRSS(ByVal idNota As Integer, ByVal idCanal As Integer, ByVal title_Es As String, ByVal title_En As String, ByVal link As String, ByVal pubDate As DateTime, ByVal descripcion_Es As String, ByVal descripcion_En As String, ByVal Author As String, ByVal linkDefault As String) As Boolean
        Dim conStr As String = AppSettings("PortalConnection")
        Dim sqlCon As SqlConnection = New SqlConnection(AppSettings("PortalConnectionString"))
        Dim sqlCommand As SqlCommand = New SqlCommand("spUpdNoteRSS", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.AddWithValue("@idNota", idNota)
        sqlCommand.Parameters.AddWithValue("@idCanal", idCanal)
        sqlCommand.Parameters.AddWithValue("@title_Es", title_Es)
        sqlCommand.Parameters.AddWithValue("@title_En", title_En)
        sqlCommand.Parameters.AddWithValue("@link", link)
        sqlCommand.Parameters.AddWithValue("@pubDate", pubDate)
        sqlCommand.Parameters.AddWithValue("@descripcion_Es", descripcion_Es)
        sqlCommand.Parameters.AddWithValue("@descripcion_En", descripcion_En)
        sqlCommand.Parameters.AddWithValue("@Author", Author)
        sqlCommand.Parameters.AddWithValue("@linkDefault", linkDefault)
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

    Public Function delNoteRSS(ByVal idNote As Integer, ByVal idCanal As Integer) As Boolean
        Dim conStr As String = AppSettings("PortalConnection")
        Dim sqlCon As SqlConnection = New SqlConnection(AppSettings("PortalConnectionString"))
        Dim sqlCommand As SqlCommand = New SqlCommand("spDelNoteRSS", sqlCon)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.AddWithValue("@idCanal", idCanal)
        sqlCommand.Parameters.AddWithValue("@idNota", idNota)
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

    Public Sub deleteNoteRSS(ByVal idNota As Integer, ByVal idCanal As Integer)
        If delNoteRSS(Me.idNota, Me.idCanal) Then
            Call LimpiarControles()
            If dgNotasRSS.CurrentPageIndex > 0 And dgNotasRSS.Items.Count = 1 Then
                dgNotasRSS.CurrentPageIndex = ((dgNotasRSS.CurrentPageIndex * dgNotasRSS.PageSize) \ dgNotasRSS.PageSize) - 1
            End If
            dgNotasRSS.SelectedIndex = -1
            Me.idNota = -1
            Call LoadNotesRSS()
        End If
    End Sub

    Private Sub LoadNotesRSS(Optional ByVal isLoad As Integer = 0)
        LimpiarControles()
        If ddlCanal.SelectedValue <> -1 Then

            Dim filtros As String = String.Empty
            If isLoad = 1 Then
                If CheckBoxFiltro.Checked Then
                    If TextboxTituloFiltro.Text <> String.Empty Then
                        filtros = "title like '%" & TextboxTituloFiltro.Text & "%' "
                    End If

                    If TextBoxDescripcionFiltro.Text <> String.Empty Then
                        filtros = filtros & IIf(filtros <> String.Empty, " Or ", "") & "description like '%" & TextBoxDescripcionFiltro.Text & "%' "
                    End If

                    If CheckBoxIncluirFechas.Checked Then
                        If txtDateFromFiltro.Text <> String.Empty And txtDateToFiltro.Text <> String.Empty Then
                            filtros = filtros & IIf(filtros <> String.Empty, " AND ", "") & "(pubdate >= '" & txtDateFromFiltro.Text & "' and pubdate <= '" & txtDateToFiltro.Text & "')"
                        End If
                    End If
                End If
            End If

            Me.idCanal = ddlCanal.SelectedValue
            ddlCanalNuevo.Enabled = True
            Dim ds As DataSet = New DataSet
            ds = LoadNotasRSS(idCanal, PortalCulture.GetIDCulture)


            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim TAB As System.Web.UI.HtmlControls.HtmlTextArea
                    TAB = New System.Web.UI.HtmlControls.HtmlTextArea
                    TAB.InnerHtml = CType(dr(1), String)
                    Dim cadHTML As String
                    cadHTML = TAB.InnerText
                    cadHTML = System.Text.RegularExpressions.Regex.Replace(cadHTML, "<[^>]*>", String.Empty)
                    dr(1) = cadHTML

                    TAB.InnerHtml = CType(dr(2), String)
                    cadHTML = TAB.InnerText
                    cadHTML = System.Text.RegularExpressions.Regex.Replace(cadHTML, "<[^>]*>", String.Empty)
                    dr(2) = cadHTML
                Next
                Dim dv As DataView = ds.Tables(0).DefaultView
                If filtros <> String.Empty Then
                    dv.RowFilter = filtros
                End If
                If dv.Count > 0 Then
                    If isLoad = 1 Then
                        dgNotasRSS.CurrentPageIndex = 0
                        dgNotasRSS.SelectedIndex = -1
                    End If
                    dgNotasRSS.DataSource = dv
                    dgNotasRSS.DataBind()
                    dgNotasRSS.Visible = True
                    lblNoNotesFound.Visible = False
                Else
                    dgNotasRSS.Visible = False
                    lblNoNotesFound.Visible = True
                End If
            Else
                'No hay Notas
                dgNotasRSS.Visible = False
                lblNoNotesFound.Visible = True
            End If
        Else
            LabelMsgCanalLoad.Visible = True
        End If
    End Sub



    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim Script As String
        CheckBoxFiltro.Attributes.Add("onclick", "javascript:MostrarFiltro('" & CheckBoxFiltro.ClientID & "');")
        'RegisterStartupScript("Mostrar", "<script> MostrarFiltro('" & CheckBoxFiltro.ClientID & "'); </script>")
        Script = String.Format("<script> MostrarFiltro('{0}'); </script>", CheckBoxFiltro.ClientID)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Script)

        'Recursos
        lblTitle.Text = PortalCulture.GetString("01107", False)
        lblFaresTitle.Text = PortalCulture.GetString("01108", False)

        lblEName.Text = PortalCulture.GetString("00791", True)
        CheckBoxFiltro.Text = PortalCulture.GetString("01100", False)
        LabelTituloFiltro.Text = PortalCulture.GetString("01101", True)
        LabelDescripcionFiltro.Text = PortalCulture.GetString("00002", True)
        LabelDesdeFiltro.Text = PortalCulture.GetString("00108", True)
        LabelHastaFiltro.Text = PortalCulture.GetString("00109", True)
        CheckBoxIncluirFechas.Text = PortalCulture.GetString("00517", False)
        lblNoNotesFound.Text = PortalCulture.GetString("01102", False)
        LabelCanalNuevo.Text = PortalCulture.GetString("00791", True)
        lblFechaNoticia.Text = PortalCulture.GetString("M000120", True)
        LabelTituloNuevo.Text = PortalCulture.GetString("01101", True)
        LabelDescripcionNuevo.Text = PortalCulture.GetString("00002", True)
        LabelAutorNuevo.Text = PortalCulture.GetString("01106", True)
        btnNew.Text = PortalCulture.GetString("00102", False)
        btnSave.Text = PortalCulture.GetString("A00153", False)
        btnLoad.Text = PortalCulture.GetString("00149", False)
        LabelMsgCanalLoad.Text = PortalCulture.GetString("01109", False)
        MsgCanal.Text = PortalCulture.GetString("01109", False)
        lblMsgFecha.Text = PortalCulture.GetString("01110", False)
        lblMsgTitulo.Text = PortalCulture.GetString("00129", False)
    End Sub


    Private Sub EditNote(ByVal idCanal As Integer, ByVal idNota As Integer)
        LimpiarControles()
        Dim ds As DataSet = New DataSet
        ds = LoadNoteRSS(idCanal, idNota)
        If ds.Tables(0).Rows.Count > 0 Then
            txtDateFrom.Text = CType(ds.Tables(0).Rows(0)("pubDate"), DateTime).ToString("MM/dd/yyyy")
            ddlCanalNuevo.SelectedValue = idCanal
            ddlCanalNuevo.Enabled = False
            CtrlIdiomaFCk_Titulo.setES(ds.Tables(0).Rows(0)("title_es"))
            CtrlIdiomaFCk_Titulo.SetEN(ds.Tables(0).Rows(0)("title_en"))
            CtrlIdiomaFCk_Descripcion.textoEspañol = ds.Tables(0).Rows(0)("Description_es")
            CtrlIdiomaFCk_Descripcion.textoIngles = ds.Tables(0).Rows(0)("Description_en")
            TextBoxLink.Text = ds.Tables(0).Rows(0)("link")
            TextBoxAutor.Text = ds.Tables(0).Rows(0)("Author")
        Else
            'No se encontro la nota
        End If
    End Sub
    Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Call LoadNotesRSS(1)
    End Sub

    Private Sub dgNotasRSS_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgNotasRSS.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            Dim lkEdit As LinkButton
            lkEdit = e.Item.FindControl("lnkEdit")
            lkEdit.Text = PortalCulture.GetString("00065")

            Dim lk As LinkButton
            'Dim lbl As Label
            lk = e.Item.FindControl("lnkEdit")
            lk.Text = PortalCulture.GetString("00065")
            lk = e.Item.FindControl("lnkDelete2")
            Dim hpl As HyperLink = e.Item.FindControl("lnkDelete")
            hpl.Text = PortalCulture.GetString("00103")
            hpl.NavigateUrl = Me.CtlMensajes1.getShow(lk.ClientID, PortalCulture.GetString("01112"), PortalCulture.GetString("01113"))

            e.Item.Cells(2).Text = CDate(e.Item.Cells(2).Text).ToString("MMM/dd/yyyy")
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("01101")
            e.Item.Cells(2).Text = PortalCulture.GetString("01103")
        End If
    End Sub

    Private Sub dgNotasRSS_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgNotasRSS.ItemCommand
        idNota = -1
        If e.CommandName = "Edit" Then
            Try
                idNota = Integer.Parse(e.Item.Cells(1).Text)
                Call EditNote(idCanal, idNota)
            Catch ex As Exception
                Return
            End Try
        ElseIf e.CommandName = "Delete" Then
            Me.idNota = Integer.Parse(e.Item.Cells(1).Text)
            Call deleteNoteRSS(idNota, idCanal)
        End If
    End Sub

    Private Sub dgNotasRSS_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgNotasRSS.PageIndexChanged
        dgNotasRSS.CurrentPageIndex = e.NewPageIndex
        dgNotasRSS.SelectedIndex = -1
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        LoadNotesRSS()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub
End Class
