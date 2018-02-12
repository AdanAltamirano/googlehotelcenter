Imports System.Configuration.ConfigurationManager
Imports System.Web.Security
Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Common
Imports System.Data.SqlClient
Imports System.Xml
Imports Portal.General.DataAccess
Imports Portal.General.Common.Data
Imports Portal.General.Rules

Imports LoginAuthenticate

Partial Class Users
    Inherits PaginaBase
    Protected WithEvents CtlMensajes1 As ctlMensajes

    Private Const rmPATH As String = "/RateManager/"

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lblHoteles As System.Web.UI.WebControls.Label

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Enum dgcolumns
        Nombre
        Read
        Add
        Modify
        Delete
        url
        EsEncabezado
    End Enum
    Public Enum dgHotelColumns
        idHotel
        NombreEmpresa
        Manager
    End Enum

    Private Property iduser() As Integer
        Get
            Return viewstate("_iduser")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_iduser") = Value
        End Set
    End Property

    Private Property Add() As Boolean
        Get
            Return viewstate("_add")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("_add") = Value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack Then
            Me.lblError.Visible = False
            LoadOpciones()
            cargaHoteles()
            cargausuarios()
            Me.Add = True
        End If
        Me.ResizefrmPrincipal()
        Me.btnModify.OnClientClick = String.Format("return HotelSeleccionado('{0}');", Me.dgHoteles.ClientID)
    End Sub

    Private Sub cargausuarios()
        Dim ds As UsuarioHotelData
        With New UsuarioHotelFacade
            ds = .GetUserListByIdMainUser(MyBase.Usuario)
        End With
        dgUsuarios.DataSource = ds
        dgUsuarios.DataBind()
        clearData()
        Me.Add = True
    End Sub

    Private Sub cargaHoteles()

        Dim ds As DataSet
        With New HotelSistema
            ds = .GetHotelsCompanyByUser(Usuario)
        End With
        Me.dgHoteles.DataSource = ds
        Me.dgHoteles.DataBind()


    End Sub

    Private Sub dgPermisos_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPermisos.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim esEncabezado As Integer = CInt(e.Item.Cells(dgcolumns.EsEncabezado).Text)
            Dim chk As CheckBox

            chk = e.Item.FindControl("chkAdd")
            chk.Visible = IIf(esEncabezado = 1, False, True)
            chk = e.Item.FindControl("chkModify")
            chk.Visible = IIf(esEncabezado = 1, False, True)
            chk = e.Item.FindControl("chkRead")
            chk.Visible = IIf(esEncabezado = 1, False, True)
            chk = e.Item.FindControl("chkDelete")
            chk.Visible = IIf(esEncabezado = 1, False, True)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.Nombre).Text = PortalCulture.GetString("00470")
            'e.Item.Cells(dgcolumns.Add).Text = PortalCulture.GetString("00448")
            'e.Item.Cells(dgcolumns.Modify).Text = PortalCulture.GetString("00449")
            'e.Item.Cells(dgcolumns.Read).Text = PortalCulture.GetString("00450")
            'e.Item.Cells(dgcolumns.Delete).Text = PortalCulture.GetString("00451")
        End If
    End Sub

    Private Sub Users_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadresources()
    End Sub

    Private Sub dgUsuarios_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgUsuarios.ItemCommand
        If e.CommandName = "Delete" Then
            With New UsuarioHotelFacade
                If .DeleteUser(dgUsuarios.Items(e.Item.ItemIndex).Cells(0).Text) Then

                    Me.guardalog("/Pages/Users.aspx", PaginaBase.acciones.Eliminar, String.Format("Eliminó el usuario {0}, hotel {1} ", dgUsuarios.Items(e.Item.ItemIndex).Cells(1).Text, Me.cInfoActual.HotelName))

                    If dgUsuarios.CurrentPageIndex > 0 And dgUsuarios.Items.Count = 1 Then
                        dgUsuarios.CurrentPageIndex = ((dgUsuarios.CurrentPageIndex * dgUsuarios.PageSize) \ dgUsuarios.PageSize) - 1
                    End If
                    Me.dgUsuarios.SelectedIndex = -1
                    cargausuarios()
                End If
            End With
            '            CtlMensajes1.Show(PortalCulture.GetString("00383"), PortalCulture.GetString("00461"), ctlMensajes.Tipos.Prompt)
            'Me.iduser = e.Item.Cells(0).Text
            'Me.divEliminar.Style.Add("display", "")
            'Me.dgPermisos.Visible = False
            'Me.btnModify.Visible = False
            'Me.btnNew.Visible = False
        End If
    End Sub

    Public Function validar_Mail(ByVal sMail As String) As Boolean
        ' retorna true o false   
        Return Regex.IsMatch(sMail, "^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$")
    End Function

    Private Sub btnModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModify.Click
        Dim NoError As Boolean = True
        If Not Page.IsValid Then Return
        If Not isHotelSeleccionado() Then Return

        If Me.Add = True Then
            NoError = addUser()
            If NoError Then
                If AppSettings("idSegmento") = "4" AndAlso validar_Mail(Me.txtName.Text) Then
                    Fillcorreo()
                End If
                cargausuarios()
            End If
        Else
            ' si cambio el password
            Dim permisos As PermisosData = New PermisosData
            SavePermisos(permisos, Me.iduser)

            With New PermisosFacade
                If Me.txtPassword.Text <> "" Then
                    Dim pass As String = crypto.EncryptString128Bit(Me.txtPassword.Text, crypto.PublicKey)
                    NoError = .AddPermisos(permisos, Me.iduser, True, pass, Not Me.chkInfoTC.Checked)
                Else
                    NoError = .AddPermisos(permisos, Me.iduser, False, "", Not Me.chkInfoTC.Checked)
                End If
            End With

            'Hoteles a los cuales tiene permiso
            If NoError Then
                NoError = SaveHotelsAccess(Me.iduser)
            End If

        End If
        If NoError Then
            clearData()
            cargausuarios()
        End If
    End Sub

    Private Function isHotelSeleccionado() As Boolean
        Dim chk As CheckBox
        Dim hr As Boolean = False

        For Each item As DataGridItem In dgHoteles.Items
            chk = item.Cells(dgHotelColumns.Manager).FindControl("chkAdministrar")
            If chk.Checked Then
                hr = True
                Exit For
            End If
        Next
        Return hr
    End Function

    Private Function SaveHotelsAccess(ByVal id As Integer) As Boolean
        Dim result As Boolean
        If dgHoteles.Items.Count > 0 Then
            Dim dsHUH As Hoteles_UsuarioHotelData = New Hoteles_UsuarioHotelData
            Dim chk As CheckBox
            For Each item As DataGridItem In dgHoteles.Items
                chk = item.Cells(dgHotelColumns.Manager).FindControl("chkAdministrar")
                If chk.Checked Then                    
                    Dim rowH As DataRow = dsHUH.Tables(Hoteles_UsuarioHotelData.Hoteles_UsuarioHotelTable).NewRow()
                    rowH(Hoteles_UsuarioHotelData.iduserField) = id
                    rowH(Hoteles_UsuarioHotelData.idhotelField) = CType(item.Cells(dgHotelColumns.idHotel).Text, Integer)
                    dsHUH.Tables(Hoteles_UsuarioHotelData.Hoteles_UsuarioHotelTable).Rows.Add(rowH)
                End If
            Next

            With New Hoteles_UsuarioHotelFacade
                result = .AddHoteles_UsuarioHotel(dsHUH, iduser)
                If result Then
                    Me.guardalog("/Pages/Users.aspx", PaginaBase.acciones.Modificar, String.Format("Se modificó el usuario {0}, hotel {1} ", txtName.Text, Me.cInfoActual.HotelName))
                End If
                Return result
            End With
        End If


    End Function

    Private Sub Fillcorreo()
        Dim ci As System.Globalization.CultureInfo

        Dim Mail As emailTemplates.Template

        Dim dsEmpresa As EmpresaDatos
        dsEmpresa = (New EmpresaSistema).GetCompanyById(MyBase.cInfoActual.Empresa)

        Try
            Mail = New emailTemplates.Template

            Mail.TemplateName = "TH_WORLDESTINATION_WELCOME"
            Mail.To = Me.txtName.Text

            Mail.SubjectParam = "Welcome to World Destination Travel Limites"
            Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Mail.Html = True
            Mail.AddParameter("HOTELNAME") = dsEmpresa.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Nombre)
            Mail.AddParameter("USEREMAIL") = Me.txtName.Text
            Mail.AddParameter("PASSWORD") = Me.txtPassword.Text
            Mail.Send()

        Catch ex As Exception
            Dim sErrorMessage As String = String.Format(".. The HTML fragment file '{0}' ", ex.ToString)
            Trace.Write(ex.StackTrace.ToString & sErrorMessage)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Me.Add = True
        clearData()
    End Sub

    Private Sub clearData()
        Me.txtPassword.Text = ""
        Me.txtConfirmPass.Text = ""
        Me.txtName.Text = ""
        Me.dgUsuarios.SelectedIndex = -1
        Me.chkInfoTC.Checked = False
        Me.lblError.Visible = False
        Me.lblError.Visible = False
        Me.txtName.Enabled = True
        Me.iduser = 0
        For Each item As DataGridItem In dgHoteles.Items
            Dim chk As CheckBox = CType(item.Cells(dgHotelColumns.Manager).FindControl("chkAdministrar"), CheckBox)
            chk.Checked = False
        Next

        For Each item As DataGridItem In dgPermisos.Items
            Dim chk As CheckBox = CType(item.Cells(dgcolumns.Read).FindControl("chkRead"), CheckBox)
            chk.Checked = False
        Next


    End Sub


    Private Function addUser() As Boolean
        Dim ds As UsuarioHotelData
        With New UsuarioHotelFacade
            ds = .GetUserByName(Me.txtName.Text.Trim)
        End With
        If ds.Tables(UsuarioHotelData.UsuarioTable).Rows.Count = 0 Then
            Dim newuser As UsuarioHotelData = New UsuarioHotelData
            Dim r As DataRow = newuser.Tables(UsuarioHotelData.UsuarioTable).NewRow
            Dim pass As String = crypto.EncryptString128Bit(Me.txtPassword.Text, crypto.PublicKey)
            r(UsuarioHotelData.idhotelField) = 0
            r(UsuarioHotelData.IdMainUserField) = MyBase.Usuario
            r(UsuarioHotelData.nameField) = Me.txtName.Text
            r(UsuarioHotelData.passwordField) = pass
            newuser.Tables(UsuarioHotelData.UsuarioTable).Rows.Add(r)
            Dim permisos As PermisosData = New PermisosData
            SavePermisos(permisos)
            With New UsuarioHotelFacade
                If .addUser(newuser, permisos) Then
                    Dim id As Integer = newuser.Tables(UsuarioHotelData.UsuarioTable).Rows(0).Item(UsuarioHotelData.iduserField)
                    SaveHotelsAccess(id)

                    Me.guardalog("/Pages/Users.aspx", PaginaBase.acciones.Crear, String.Format("Se creo el usuario {0}, hotel {1} ", txtName.Text, Me.cInfoActual.HotelName))
                End If

            End With
            Me.lblError.Visible = False



            Return True
        Else
            Me.lblError.Visible = True
            Return False
        End If
    End Function

    Private Sub SavePermisos(ByRef permisos As PermisosData)
        Dim r As DataRow
        For Each i As DataGridItem In Me.dgPermisos.Items
            Dim chk As CheckBox
            chk = i.FindControl("chkRead")
            If chk.Visible = True AndAlso chk.Checked Then
                r = permisos.Tables(PermisosData.PermisosTable).NewRow

                r(PermisosData.NameField) = i.Cells(dgcolumns.url).Text.Trim()
                r(PermisosData.PermisosField) = "R"
                permisos.Tables(PermisosData.PermisosTable).Rows.Add(r)
            End If
        Next
    End Sub

    Function ParsePermiso(ByVal src As String) As String
        Dim sTmp As String
        Try
            sTmp = src
            If sTmp.IndexOf("RateManager") = -1 Then
                sTmp = rmPATH & sTmp
                sTmp = sTmp.Replace("//", "/")
            End If
        Catch ex As Exception
            sTmp = src
        End Try
        Return sTmp
    End Function

    Private Sub SavePermisos(ByRef permisos As PermisosData, ByVal userID As Integer)
        Dim r As DataRow
        For Each i As DataGridItem In Me.dgPermisos.Items
            Dim chk As CheckBox
            chk = i.FindControl("chkRead")
            If chk.Visible = True AndAlso chk.Checked Then
                r = permisos.Tables(PermisosData.PermisosTable).NewRow
                r(PermisosData.IdUserField) = userID
                r(PermisosData.NameField) = i.Cells(dgcolumns.url).Text.Trim()
                r(PermisosData.PermisosField) = "R"
                permisos.Tables(PermisosData.PermisosTable).Rows.Add(r)
            End If
        Next
    End Sub

    Private Sub loadresources()
        Me.dgPermisos.Columns(dgcolumns.Add).HeaderText = PortalCulture.GetString("00448")
        Me.dgPermisos.Columns(dgcolumns.Modify).HeaderText = PortalCulture.GetString("00449")
        Me.dgPermisos.Columns(dgcolumns.Read).HeaderText = PortalCulture.GetString("00450")
        Me.dgPermisos.Columns(dgcolumns.Delete).HeaderText = PortalCulture.GetString("00451")
        Me.dgUsuarios.Columns(1).HeaderText = PortalCulture.GetString("00471")
        If Me.Add Then
            Me.lblTitle.Text = PortalCulture.GetString("00329")
        Else
            Me.lblTitle.Text = PortalCulture.GetString("00330")
        End If
        Me.lblName.Text = PortalCulture.GetString("00073")
        Me.lblPass.Text = PortalCulture.GetString("00334")
        Me.lblConfPass.Text = PortalCulture.GetString("00335")
        Me.dgUsuarios.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        Me.dgUsuarios.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        lblError.Text = PortalCulture.GetString("00469")
        btnModify.Text = PortalCulture.GetString("A00153")
        Me.btnNew.Text = PortalCulture.GetString("00102")
        Me.chkInfoTC.Text = PortalCulture.GetString("01339")

    End Sub

    Private Sub dgUsuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgUsuarios.SelectedIndexChanged
        'Limpar PErmisos y Hoteles
        For Each item As DataGridItem In dgHoteles.Items
            Dim chk As CheckBox = CType(item.Cells(dgHotelColumns.Manager).FindControl("chkAdministrar"), CheckBox)
            chk.Checked = False
        Next

        For Each item As DataGridItem In dgPermisos.Items
            Dim chk As CheckBox = CType(item.Cells(dgcolumns.Read).FindControl("chkRead"), CheckBox)
            chk.Checked = False
        Next

        Me.Add = False
        Dim ds As PermisosData
        iduser = dgUsuarios.Items(dgUsuarios.SelectedIndex).Cells(0).Text
        With New PermisosFacade
            ds = .PermisosGetByUser(iduser)
        End With

        Me.txtName.Text = dgUsuarios.Items(dgUsuarios.SelectedIndex).Cells(1).Text
        Me.chkInfoTC.Checked = If(dgUsuarios.Items(dgUsuarios.SelectedIndex).Cells(4).Text.CompareTo("0") = 0, True, False)
        Me.txtName.Enabled = False

        iduser = dgUsuarios.Items(dgUsuarios.SelectedIndex).Cells(0).Text


        If ds.Tables(PermisosData.PermisosTable).Rows.Count > 0 Then
            Dim dvP As DataView = ds.Tables(PermisosData.PermisosTable).DefaultView
            Dim item As DataGridItem
            Dim chk As CheckBox
            Dim permisoName As String
            For Each item In dgPermisos.Items
                permisoName = item.Cells(dgcolumns.url).Text
                chk = CType(item.Cells(dgHotelColumns.Manager).FindControl("chkRead"), CheckBox)
                dvP.RowFilter = String.Format(" PermisoName = '{0}'", permisoName)
                If dvP.Count = 0 Then
                    chk.Checked = False
                Else
                    chk.Checked = True
                End If
            Next
        End If



        'Hoteles

        Dim dsHotel As Hoteles_UsuarioHotelData
        With New Hoteles_UsuarioHotelFacade
            dsHotel = .Hoteles_UsuarioHotelGetByUser(iduser)
        End With
        If dsHotel.Tables(Hoteles_UsuarioHotelData.Hoteles_UsuarioHotelTable).Rows.Count > 0 AndAlso dgHoteles.Items.Count > 0 Then
            Dim dvHUH As DataView = dsHotel.Tables(Hoteles_UsuarioHotelData.Hoteles_UsuarioHotelTable).DefaultView
            Dim item As DataGridItem
            Dim chk As CheckBox
            Dim idHotel As Integer
            For Each item In dgHoteles.Items
                idHotel = item.Cells(dgHotelColumns.idHotel).Text
                chk = CType(item.Cells(dgHotelColumns.Manager).FindControl("chkAdministrar"), CheckBox)
                dvHUH.RowFilter = String.Format(" idHotel = {0}", idHotel)
                If dvHUH.Count = 0 Then
                    chk.Checked = False
                Else
                    chk.Checked = True
                End If
            Next
        End If
    End Sub

    Private Sub dgUsuarios_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUsuarios.PageIndexChanged
        Me.dgUsuarios.CurrentPageIndex = e.NewPageIndex
        Me.dgUsuarios.SelectedIndex = -1
        cargausuarios()
    End Sub

    Private Sub dgUsuarios_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsuarios.ItemCreated

        'Me.dgUsuarios.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        'Me.dgUsuarios.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"

        If e.Item.ItemType = ListItemType.Pager Then
            If dgUsuarios.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgUsuarios.CurrentPageIndex < dgUsuarios.PageCount - 1 Then
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


    Private Sub dgUsuarios_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsuarios.ItemDataBound
        If e.Item.ItemType = ListItemType.EditItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lk As LinkButton
            lk = e.Item.Cells(2).FindControl("lnkEdit")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00093")
            End If
            lk = e.Item.Cells(2).FindControl("lnkdelete2")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00103")
            End If
            Dim LK2 As HyperLink
            LK2 = e.Item.Cells(2).FindControl("lnkdelete")
            LK2.Text = PortalCulture.GetString("00103")
            LK2.NavigateUrl = CtlMensajes1.getShow(lk.ClientID, PortalCulture.GetString("00383"), PortalCulture.GetString("00461"))
        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(1).Text = PortalCulture.GetString("00471")
        End If
    End Sub

    Private Sub LoadOpciones()
        Dim cMenu As New clsMenu
        Dim doc As XmlDocument = New XmlDocument
        Dim rol As String = "UsuarioHotel"
        Dim dt As DataTable = New DataTable
        Dim menuXml As String

        dt.Columns.Add("Nombre")
        dt.Columns.Add("url")
        dt.Columns.Add("EsEncabezado")
        Try
            menuXml = cMenu.GetMenu(AppSettings("idSistema"), Me.IdIdiomaMenu, "sys_dealsHotel")
            If menuXml <> String.Empty Then

                Try
                    Dim dsPages As New DataSet
                    Dim strXML As String = HttpContext.Current.Request.PhysicalApplicationPath & "/Permisos.xml"
                    Dim drnew As DataRow = dt.NewRow
                    dsPages.ReadXml(strXML)
                    For Each dr As DataRow In dsPages.Tables(0).Rows
                        If dr("userHMenu") = "1" Then
                            If Not dr.IsNull("Description") AndAlso Not String.IsNullOrEmpty(dr("Description")) Then
                                drnew("Nombre") = PortalCulture.GetString(dr("Description"))
                            End If
                            drnew("EsEncabezado") = 0
                            drnew("url") = String.Format("/{0}/{1}", Request.ApplicationPath, dr("Id")).Replace("//", "/")
                            dt.Rows.Add(drnew)
                        End If
                    Next
                Catch ex As Exception
                End Try

                doc.LoadXml(menuXml)
                'doc.Load(Server.MapPath(AppSettings("MenuUsuarios") & PortalCulture.GetString("00000") & ".xml"))
                readNodeMenu(doc, doc.SelectNodes("/menu/menuItem"), False, rol, dt)

                dgPermisos.DataSource = dt
                dgPermisos.DataBind()

            End If
        Catch ex As Exception

        End Try

    End Sub


    Private Sub readNodeMenu(ByRef doc As XmlDocument, ByRef nodos As XmlNodeList, ByVal isSubNodo As Boolean, ByVal rol As String, ByRef dt As DataTable)
       

        If nodos.Count > 0 Then
            For Each nodo As XmlNode In nodos
                Dim nText As String = String.Empty
                Dim nUrl As String = String.Empty
                Dim subNodo As XmlNode = Nothing
                Dim nRoles As String = String.Empty
                Dim show As Boolean = True

                For i As Integer = 0 To nodo.ChildNodes.Count - 1
                    Select Case nodo.ChildNodes.Item(i).Name
                        Case "text"
                            nText = nodo.ChildNodes.Item(i).InnerText
                        Case "url"
                            nUrl = nodo.ChildNodes.Item(i).InnerText
                        Case "subMenu"
                            subNodo = nodo.ChildNodes.Item(i)
                        Case "roles"
                            nRoles = nodo.ChildNodes.Item(i).InnerText

                    End Select
                Next
                show = True
                If nRoles <> String.Empty Then
                    Dim r As String
                    show = False
                    For Each r In nRoles.Split(",")
                        If r.ToUpper = rol.ToUpper Then
                            show = True
                        End If
                    Next
                End If

                If show Then
                    Dim dr As DataRow = dt.NewRow
                    dr("Nombre") = IIf(isSubNodo, String.Format("&nbsp;{0}", nText), String.Format("<B>{0}</B>", nText))
                    dr("EsEncabezado") = IIf(isSubNodo, String.Format("0"), String.Format("1"))
                    dr("url") = nUrl
                    dt.Rows.Add(dr)

                End If


                If Not subNodo Is Nothing AndAlso show Then
                    readNodeMenu(doc, subNodo.ChildNodes, True, rol, dt)
                End If
            Next
        End If





    End Sub


    Private Sub dgHoteles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHoteles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgHotelColumns.NombreEmpresa).Text = PortalCulture.GetString("M0BT0000150")

        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim ds As UsuarioHotelData
        ds = GetUserListByIdMainUserAndName(MyBase.Usuario, txtSearch.Text)
        dgUsuarios.DataSource = ds
        dgUsuarios.DataBind()
        clearData()
        Me.Add = True
    End Sub

    Public Function GetUserListByIdMainUserAndName(ByVal IdMainUser As Integer, ByVal Name As String) As UsuarioHotelData
        Dim usuario As UsuarioHotelData = New UsuarioHotelData
        Dim dsCommand As SqlDataAdapter
        dsCommand = New SqlDataAdapter
        dsCommand.TableMappings.Add("Table", UsuarioHotelData.UsuarioTable)
        Try
            dsCommand.SelectCommand = GetLoadCommandByIdMainUserAndName(IdMainUser, Name)
            dsCommand.Fill(usuario)
        Catch e As Exception
        Finally
            If dsCommand.SelectCommand.Connection.State = ConnectionState.Open Or dsCommand.SelectCommand.Connection.State = ConnectionState.Closed Then
                dsCommand.SelectCommand.Connection.Close()
            End If
        End Try
        Return usuario
    End Function

    Private Function GetLoadCommandByIdMainUserAndName(ByVal IdMainUser As Integer, ByVal Name As String) As SqlCommand
        Dim loadCommand As SqlCommand
        loadCommand = New SqlCommand("spUsuarioHotelGetFilterByIdMainUserAndUserName", New SqlConnection((ConfigurationSettings.AppSettings("HotelConnection"))))
        loadCommand.CommandType = CommandType.StoredProcedure
        With loadCommand.Parameters
            .Add("@IdMainUser", SqlDbType.Int)
            .Add("@Name", SqlDbType.NVarChar)

            .Item("@IdMainUser").Value = IdMainUser
            .Item("@Name").Value = Name
        End With
        GetLoadCommandByIdMainUserAndName = loadCommand
    End Function
End Class



Public Class permisos
    Public Const RatePlan As String = "Rateplan"
    Public Const RatePlanRules As String = "Rateplan_Rules"
    Public Const RatePlanLinks As String = "Rateplan_Links"
    Public Const Rooms As String = "Rooms"
    Public Const RoomsLinks As String = "Rooms_Links"
    Public Const Tarifas As String = "Fares"
    Public Const InventoryRooms As String = "Inventory_Rooms"
    Public Const RatePlanInventory As String = "RatePlan_Inventory"
    Public Const SoldOutInventory As String = "Dependency_Inventory"
    Public Const Bloqueos As String = "Locks"
    Public Const ConfirmReservations As String = "ConfirmReservations"
    Public Const ReservationsList As String = "Reservations"
    'Nuevos permisos
    Public Const GeneralInformation As String = "General_Information"
    Public Const Roomtypes As String = "Room_Types"
    Public Shared Function GetPermisosNames() As PermisosData
        Dim p As PermisosData = New PermisosData
        Dim r As DataRow
        With p.Tables(PermisosData.PermisosTable)
            r = .NewRow
            r(PermisosData.NameField) = permisos.RatePlan
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.RatePlanRules
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.RatePlanLinks
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.Rooms
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.RoomsLinks
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.InventoryRooms
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.RatePlanInventory
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.SoldOutInventory
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.Tarifas
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.Bloqueos
            .Rows.Add(r)
            r = .NewRow
            r(PermisosData.NameField) = permisos.ConfirmReservations
            .Rows.Add(r)
        End With
        Return p
    End Function
End Class







