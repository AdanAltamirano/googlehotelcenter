Imports System.Security.Cryptography
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports Common.Data
Imports Facade
Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Imports Portal.TaskManager.Facade
Imports Portal.General.Rules
Imports Portal.General.DataAccess


Partial Public Class AgencyAsociate
    Inherits PaginaBase

    



    Enum dgcolumns
        idAgencia
        Nombre
        tmpEdit
    End Enum

    Enum dgcolumnsUser
        idUser
        Email
        tmpEdit
        tmpDesasociar
    End Enum
    Public idSegmento As String = AppSettings("IdSegmento")
    Private Property idEmpresa() As Integer
        Get
            Return ViewState("idEmpresa")
        End Get
        Set(ByVal value As Integer)
            ViewState("idEmpresa") = value
        End Set
    End Property

    Private Property EmpresaNombre() As String
        Get
            Return ViewState("_EmpresaNombre")
        End Get
        Set(ByVal value As String)
            ViewState("_EmpresaNombre") = value
        End Set
    End Property

    Sub ClearCtrl()
        lblCorreo.Text = ""
        lblPassword.Text = ""
        txtPassword.Text = ""
        txtConfirmPass.Text = ""
        txtApellido.Text = ""
        txtNombre.Text = ""
        txtEmail.Text = ""
        btnModify.Enabled = False
        lblAddError.Visible = False
    End Sub

    Sub LoadResources()
        lblTitleForm.Text = PortalCulture.GetString("01524")
        lblemail.InnerHtml = PortalCulture.GetString("00163", True)
        lblPass.Text = PortalCulture.GetString("M000096", True)
        lblUser.Text = PortalCulture.GetString("M000008", True)
        lblConfPass.Text = PortalCulture.GetString("00335", True)
        btnSave.Text = PortalCulture.GetString("M000515")
        btnModify.Text = PortalCulture.GetString("A00153")
        Me.btnNew.Text = PortalCulture.GetString("M000143")
    End Sub

    'Private Sub EditMode(ByVal Editing As Boolean)
    '    If Editing Then
    '        divEdit.Visible = True
    '        btnSave.Visible = False
    '        txtEmail.Enabled = False
    '    Else
    '        divEdit.Visible = False
    '        btnSave.Visible = True
    '        txtEmail.Enabled = True
    '    End If
    'End Sub

    Sub LoadAgencies()
        Dim ds As Portal.General.Common.Data.EmpresaDatos
        Dim em As New EmpresaSistema
        Dim idSegmento As Integer

        Integer.TryParse(AppSettings("IdSegmento"), idSegmento)
        ds = em.GetCompanyAgencyByIdSegment(idSegmento)
        If Not Me.dsEmpty(ds) Then
            dgAgencies.DataSource = ds
            dgAgencies.DataBind()
        End If
    End Sub

    Private Sub LoadUserEmpresaAgency(ByVal idEmpresa As Integer)
        Dim dsUsuarios As Portal.General.Common.Data.UserData
        Dim sTabla As String = Portal.General.Common.Data.UserCompanyData.USERCOMPANY_TABLE

        pnlUser.Visible = True
        ClearCtrl()
        lblInformacion.Text = String.Format("{0}: {1}", PortalCulture.GetString("01525"), EmpresaNombre)
        dsUsuarios = (New cUserCompanySystem).GetUsersByIdEmpresa(idEmpresa)
        If Not dsUsuarios Is Nothing AndAlso dsUsuarios.Tables(sTabla).Rows.Count > 0 Then
            dgUsuarios.DataSource = dsUsuarios.Tables(sTabla)
        Else
            dgUsuarios.DataSource = (New Portal.General.Common.Data.UserData).Tables(sTabla)
        End If
        dgUsuarios.DataBind()
    End Sub
    

    Private Sub Fillcorreo(ByVal sEmail As String)
        Dim ci As System.Globalization.CultureInfo

        Dim Mail As emailTemplates.Template
        Dim ds As UserData
        Dim sErr As String = ""
        Mail = New emailTemplates.Template
        Mail.TemplateName = "TH_WORLDESTINATION_NEW_AGENCY"

        ds = (New cUserSystem).GetUserByEmail(sEmail)

        If ds.Tables(UserData.USER_TABLE).Rows.Count > 0 Then
            If (ds.Tables(UserData.USER_TABLE).Rows(0).IsNull(UserData.PASSWORDRAS_FIELD)) Then
                sErr = "Email sin password."
            Else
                Dim pas As String = ds.Tables(UserData.USER_TABLE).Rows(0).Item(UserData.PASSWORDRAS_FIELD)
                Mail.AddParameter("PASSWORD") = DecryptString128Bit(pas, crypto.PublicKey)
            End If
        Else
            lblError.Text = PortalCulture.GetString("00327", False) 'No se encontro el E-mail
            lblError.Visible = True
        End If

        Try
            Mail.To = sEmail

            'Mail.SubjectParam
            Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Mail.Html = True
            Mail.AddParameter("USEREMAIL") = sEmail
            Mail.AddParameter("CONTACTO") = sEmail


            Mail.Send()

        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        End Try
    End Sub

    Function AsociaUserAgency() As Boolean
        lblError.Visible = False
        Dim strError As String
        Dim dsAgent As DataSet
        Dim added As Boolean = True
        Dim exists As Boolean = False

        If txtEmail.Text.Trim <> "" Then
            Dim sendmail As Boolean
            If idSegmento = 4 Then
                sendmail = False
            Else
                sendmail = True
            End If
            If (New cUserCompanySystem).createUserCompany(PortalCulture.GetCulture.ToString, txtEmail.Text.Trim, idEmpresa, sendmail) Then
                CType(Me.Page, PaginaBase).guardalog("/Registro/UserAsociate.aspx", PaginaBase.acciones.Crear, String.Format("Asociar usuario a hotel {0}", txtEmail.Text), "", "", "")
                If Not sendmail = True Then
                    'Fillcorreo(txtEmail.Text)
                End If

                Dim dsUsuarios As UserData
                With (New Portal.General.Facade.cUserSystem)
                    dsUsuarios = .GetUserByEmail(txtEmail.Text.Trim)
                End With

                dsAgent = (New Agentes).getAgentByUserId(dsUsuarios.Tables(0).Rows(0).Item(dsUsuarios.IDUSER_FIELD))
                If Not dsAgent Is Nothing AndAlso (dsAgent.Tables(0).Rows.Count > 0) Then
                    Dim row As DataRow
                    For Each row In dsAgent.Tables(0).Rows
                        If row(UserAgentsData.IDAGENCY_FIELD) = idEmpresa Then
                            exists = True
                        End If
                    Next
                End If
                If exists Then
                    lblAddError.Text = PortalCulture.GetString("00097")
                    lblAddError.Visible = True
                    added = False
                Else
                    If Not (New Agentes).insertAgent(idEmpresa, dsUsuarios.Tables(0).Rows(0).Item(UserData.IDUSER_FIELD), txtNombre.Text.Trim, txtApellido.Text.Trim, txtEmail.Text.Trim, strError) Then
                        lblAddError.Text = strError
                        lblAddError.Visible = True
                        added = False
                    Else
                        txtEmail.Text = ""
                    End If
                    lblAddError.Visible = False
                End If
            Else
                lblAddError.Text = PortalCulture.GetString("00853")
                lblAddError.Visible = True
                added = False
            End If
        End If
        If added Then
            LoadUserEmpresaAgency(idEmpresa)
        End If
        Return added
    End Function

    Function AsociaUserAgencyWDT() As Boolean
        lblError.Visible = False
        Dim strError As String
        Dim dsAgent As DataSet
        Dim added As Boolean = True
        Dim exists As Boolean = False

        If txtEmail.Text.Trim <> "" Then
            Dim sendmail As Boolean
            If idSegmento = 4 Then
                sendmail = False
            Else
                sendmail = True
            End If
            If (New cUserCompanySystem).createUserCompany(PortalCulture.GetCulture.ToString, txtEmail.Text.Trim, idEmpresa, sendmail) Then
                CType(Me.Page, PaginaBase).guardalog("/Registro/UserAsociate.aspx", PaginaBase.acciones.Crear, String.Format("Asociar usuario a hotel {0}", txtEmail.Text), "", "", "")
                If Not sendmail = True Then
                    'Fillcorreo(txtEmail.Text)
                End If

                Dim dsUsuarios As UserData
                With (New Portal.General.Facade.cUserSystem)
                    dsUsuarios = .GetUserByEmail(txtEmail.Text.Trim)
                End With

                dsAgent = (New Agentes).getAgentByUserId(dsUsuarios.Tables(0).Rows(0).Item(dsUsuarios.IDUSER_FIELD))
                If Not dsAgent Is Nothing AndAlso (dsAgent.Tables(0).Rows.Count > 0) Then
                    Dim row As DataRow
                    For Each row In dsAgent.Tables(0).Rows
                        If row(UserAgentsData.IDAGENCY_FIELD) = idEmpresa Then
                            exists = True
                        End If
                    Next
                End If
                If exists Then
                    lblAddError.Text = PortalCulture.GetString("00097")
                    lblAddError.Visible = True
                    added = False
                Else
                    If Not (New Agentes).insertAgent(idEmpresa, dsUsuarios.Tables(0).Rows(0).Item(UserData.IDUSER_FIELD), txtNombre.Text.Trim, txtApellido.Text.Trim, txtEmail.Text.Trim, strError) Then
                        lblAddError.Text = strError
                        lblAddError.Visible = True
                        added = False
                    Else
                        txtEmail.Text = ""
                    End If
                    lblAddError.Visible = False
                End If
            Else
                lblAddError.Text = PortalCulture.GetString("00853")
                lblAddError.Visible = True
                added = False
            End If
        End If
        If added Then
            LoadUserEmpresaAgency(idEmpresa)
        End If
        Return added
    End Function

    Private Function addAgent(ByVal IdAgencia As Integer, ByVal IdUsuario As Integer, ByVal NombreUsuario As String, ByVal ApeliidoUsuario As String, Optional ByVal Tipo As Integer = 0) As Boolean
        Dim dsAgent As UserAgentsData
        Dim rAgent As DataRow = dsAgent.Tables(UserAgentsData.USERAGENTS_TABLE).NewRow()

        rAgent.Item(UserAgentsData.IDAGENCY_FIELD) = IdAgencia
        rAgent.Item(UserAgentsData.IDUSER_FIELD) = IdUsuario
        rAgent.Item(UserAgentsData.NAME_FIELD) = NombreUsuario
        rAgent.Item(UserAgentsData.LASTNAME_FIELD) = ApeliidoUsuario
        rAgent.Item(UserAgentsData.IDUSER_FIELD) = Tipo
        rAgent.Item(UserAgentsData.NAME_LENGTH) = NombreUsuario.Length
        rAgent.Item(UserAgentsData.LASTNAME_LENGTH) = ApeliidoUsuario.Length

        With (New cUserAgents)
            Return .AddUserAgent(dsAgent)
        End With
    End Function

    Private Sub BorraUsuario(ByVal IdUser As Integer, ByVal sUsuario As String)
        lblError.Visible = False
        Dim sError As String = ""

        With New cUserCompanySystem
            If Not .DeleteUserCompany(IdUser, idEmpresa, sError) Then
                lblError.Text = PortalCulture.GetString("00852")
                lblError.Visible = True
            Else
                CType(Me.Page, PaginaBase).guardalog("/Registro/UserAsociate.aspx", PaginaBase.acciones.Eliminar, String.Format("Desasociar el usuario {0}", sUsuario), "", "", "")
            End If
        End With
        LoadUserEmpresaAgency(idEmpresa)
    End Sub

    Public Shared Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
        Dim intPosition As Integer
        Dim strStringWithOutNulls As String
        intPosition = 1
        strStringWithOutNulls = vstrStringWithNulls
        Do While intPosition > 0
            intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)
            If intPosition > 0 Then
                strStringWithOutNulls = Microsoft.VisualBasic.Left(strStringWithOutNulls, intPosition - 1) & _
                Microsoft.VisualBasic.Right(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
            End If
            If intPosition > strStringWithOutNulls.Length Then
                Exit Do
            End If
        Loop
        Return strStringWithOutNulls
    End Function

    Public Shared Function DecryptString128Bit(ByVal vstrStringToBeDecrypted As String, ByVal vstrDecryptionKey As String) As String
        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim objRijndaelManaged As New RijndaelManaged
        Dim objMemoryStream As MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim bytDecryptionKey() As Byte
        Dim intLength As Integer
        Dim intRemaining As Integer

        Dim strReturnString As String = String.Empty

        bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted)
        intLength = Len(vstrDecryptionKey)
        If intLength >= 32 Then
            vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32)
        Else
            intLength = Len(vstrDecryptionKey)
            intRemaining = 32 - intLength
            vstrDecryptionKey = vstrDecryptionKey & Strings.StrDup(intRemaining, "X")
        End If
        bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray)
        ReDim bytTemp(bytDataToBeDecrypted.Length)
        objMemoryStream = New MemoryStream(bytDataToBeDecrypted)
        Try
            objCryptoStream = New CryptoStream(objMemoryStream, _
               objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), _
               CryptoStreamMode.Read)
            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)
            objCryptoStream.FlushFinalBlock()
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch
        End Try
        Return StripNullCharacters(Encoding.ASCII.GetString(bytTemp))
    End Function

    Function LoadDataUser(ByVal sEmail As String) As Boolean
        lblError.Text = ""
        Dim ds As UserData
        Dim sErr As String = ""
        Dim dsAgent As DataSet

        btnModify.Enabled = True
        lblCorreo.Text = sEmail
        ds = (New cUserSystem).GetUserByEmail(sEmail)
        If ds.Tables(UserData.USER_TABLE).Rows.Count > 0 Then
            If (ds.Tables(UserData.USER_TABLE).Rows(0).IsNull(UserData.PASSWORDRAS_FIELD)) Then
                sErr = "Email sin password."
            Else
                Dim pas As String = ds.Tables(UserData.USER_TABLE).Rows(0).Item(UserData.PASSWORDRAS_FIELD)
                lblPassword.Text = DecryptString128Bit(pas, crypto.PublicKey)
            End If

            dsAgent = (New Agentes).getAgentByUserId(ds.Tables(0).Rows(0).Item(UserData.IDUSER_FIELD))
            txtApellido.Text = dsAgent.Tables(0).Rows(0).Item("Apellido")
            txtNombre.Text = dsAgent.Tables(0).Rows(0).Item("Nombre")
            'txtEmail.Text = sEmail
        Else            
            lblError.Text = PortalCulture.GetString("00327", False) 'No se encontro el E-mail
            lblError.Visible = True
        End If
    End Function

    Function UpdatePassword() As Boolean
        Dim newPassword As String = txtConfirmPass.Text
        Dim dsUser As UserData = Nothing
        Dim idioma As String
        Dim idUser As Integer
        Dim userName As String
        Dim userLastName As String
        Dim userEmail As String

        If PortalCulture.GetIDCulture = 1 Then
            idioma = "es-MX"
        Else
            idioma = "en-US"
        End If
        If (New cUserSystem).changePasswordUserSinEmail(idioma, lblCorreo.Text, newPassword, dsUser) Then
            Dim strError As String = String.Empty
            Dim dsUsuarios As UserData
            With (New Portal.General.Facade.cUserSystem)
                dsUsuarios = .GetUserByEmail(lblCorreo.Text.Trim)
            End With

            idUser = dsUsuarios.Tables(0).Rows(0).Item(UserData.IDUSER_FIELD)
            userName = txtNombre.Text.Trim
            userLastName = txtApellido.Text.Trim
            userEmail = lblCorreo.Text.Trim

            With (New Agentes)
                If Not .updateAgent(idUser, userName, userLastName, userEmail, strError) Then
                    lblError.Text = strError
                End If
            End With
            ClearCtrl()
        End If
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If String.IsNullOrEmpty(AppSettings("IdSegmento")) Then MyBase.redirectTo(pages.Home)
        If Not Me.IsPostBack Then
            LoadAgencies()
            ClearCtrl()
        End If
    End Sub

    Private Sub dgAgencies_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAgencies.ItemCommand
        Dim idAgencia As Integer
        If e.CommandName.ToLower = "select" Then
            Integer.TryParse(e.Item.Cells(dgcolumns.idAgencia).Text, idAgencia)
            idEmpresa = idAgencia
            EmpresaNombre = e.Item.Cells(dgcolumns.Nombre).Text
            LoadUserEmpresaAgency(idAgencia)
        End If
    End Sub

    Private Sub dgAgencies_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAgencies.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgAgencies.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgAgencies.CurrentPageIndex < dgAgencies.PageCount - 1 Then
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

    Private Sub dgAgencies_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAgencies.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.tmpEdit).Text = PortalCulture.GetString("M000524")
        End If
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then

            Dim LK2 As LinkButton
            LK2 = e.Item.Cells(dgcolumns.tmpEdit).FindControl("lnkedit")
            If (Not LK2 Is Nothing) Then
                LK2.Text = PortalCulture.GetString("00093")
            End If
        End If
    End Sub

    Private Sub dgAgencies_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgAgencies.PageIndexChanged
        dgAgencies.CurrentPageIndex = e.NewPageIndex
        dgAgencies.SelectedIndex = -1
        LoadAgencies()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        AsociaUserAgency()

    End Sub

    Private Sub dgUsuarios_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgUsuarios.ItemCommand
        If e.CommandName = "Delete" Then
            BorraUsuario(CType(e.Item.Cells(dgcolumnsUser.idUser).Text, Integer), e.Item.Cells(dgcolumnsUser.Email).Text)
        End If
        If e.CommandName.ToLower = "select" Then
            ClearCtrl()
            LoadDataUser(e.Item.Cells(dgcolumnsUser.Email).Text)
        End If
    End Sub

    Protected Sub btnModify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModify.Click
        UpdatePassword()
    End Sub

    Private Sub dgUsuarios_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsuarios.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumnsUser.Email).Text = PortalCulture.GetString("00258")
        End If
        If e.Item.ItemType = ListItemType.EditItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lk As LinkButton
            lk = e.Item.Cells(2).FindControl("lnkEdit")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00093")
            End If
            lk = e.Item.Cells(2).FindControl("lnkEliminar")
            If Not lk Is Nothing Then
                lk.Text = PortalCulture.GetString("00848")
            End If


        End If
    End Sub

    Private Sub AgencyAsociate_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        LoadResources()
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNew.Click
        ClearCtrl()
    End Sub

    Private Sub dgUsuarios_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUsuarios.PageIndexChanged
        Me.dgUsuarios.CurrentPageIndex = e.NewPageIndex
        Me.dgUsuarios.SelectedIndex = -1
        LoadUserEmpresaAgency(idEmpresa)
    End Sub

    Sub LoadAgencies(ByVal sFiltro As String)
        Dim ds As Portal.General.Common.Data.EmpresaDatos
        Dim em As New EmpresaSistema
        Dim idSegmento As Integer
        Dim dv As DataView

        Integer.TryParse(AppSettings("IdSegmento"), idSegmento)
        ds = em.GetCompanyAgencyByIdSegment(idSegmento)
        If Not Me.dsEmpty(ds) Then
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = sFiltro

            dgAgencies.DataSource = dv
            dgAgencies.DataKeyField = EmpresaDatos.FIELD_idEmpresa
            dgAgencies.DataBind()
        End If
    End Sub

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgAgencies.CurrentPageIndex = 0
        LoadAgencies(descripcion)
    End Sub


End Class

Public Class Agentes

    Private insertCommand As SqlCommand
    Private updateCommand As SqlCommand
    Private deleteCommand As SqlCommand
    Private dsCommand As SqlDataAdapter

    Private Const USERAGENTS_TABLE As String = "Agentes"
    Private Const IDUSER_FIELD As String = "idUsuario"
    Private Const IDAGENCY_FIELD As String = "idAgencia"
    Private Const NAME_FIELD As String = "Nombre"
    Private Const LASTNAME_FIELD As String = "Apellido"
    Private Const NAME_LENGTH As Integer = 40
    Private Const LASTNAME_LENGTH As Integer = 40

    Public Sub New()
        MyBase.New()
        '
        ' Crea el DataSetCommand
        dsCommand = New SqlDataAdapter
        dsCommand.TableMappings.Add("Table", USERAGENTS_TABLE)
    End Sub

    Public Function insertAgent(ByVal idAgency As Integer, ByVal idUser As Integer, ByVal userName As String, ByVal UserLastName As String, ByVal userEmail As String, ByRef strError As String) As Boolean
        Dim Agentes As New DataSet
        Dim table As DataTable = New DataTable(USERAGENTS_TABLE)

        With table.Columns
            .Add("idAgente", GetType(System.Int32)).AllowDBNull = True
            .Add(Me.IDUSER_FIELD, GetType(System.Int32)).AllowDBNull = False
            .Add(Me.IDAGENCY_FIELD, GetType(System.Int32)).AllowDBNull = False
            .Add(Me.NAME_FIELD, GetType(System.String)).AllowDBNull = False
            .Add(Me.LASTNAME_FIELD, GetType(System.String)).AllowDBNull = False
            .Add("Permisos", GetType(System.String)).AllowDBNull = True
        End With
        Agentes.Tables.Add(table)
        Dim aRow As DataRow = Agentes.Tables(0).NewRow()

        With aRow
            .Item(IDUSER_FIELD) = idUser
            .Item(IDAGENCY_FIELD) = idAgency
            .Item(NAME_FIELD) = userName
            .Item(LASTNAME_FIELD) = UserLastName
        End With

        Agentes.Tables(0).Rows.Add(aRow)

        Try
            dsCommand.InsertCommand = Me.GetAddAgentCommand()
            dsCommand.Update(Agentes, USERAGENTS_TABLE)
        Catch e As SqlException
            'Si no se pudo insertar el registro        
            strError = "Agentes: " & e.Message
            Throw New Exception(e.ToString)
        Finally
            If Agentes.HasErrors Then
                If Agentes.Tables(USERAGENTS_TABLE).HasErrors Then
                    Agentes.Tables(USERAGENTS_TABLE).GetErrors(0).ClearErrors()
                    insertAgent = False
                End If
            Else
                Agentes.AcceptChanges()
                insertAgent = True
            End If
        End Try

        'If insertAgent Then
        '    Dim dsc As New CustomerData
        '    Dim cRow As DataRow = dsc.Tables(CustomerData.CUSTOMER_TABLE).NewRow()
        '    With cRow
        '        .Item(CustomerData.IDUSER_FIELD) = idUser
        '        .Item(CustomerData.NAME_FIELD) = userName
        '        .Item(CustomerData.LASTNAME_FIELD) = UserLastName
        '        .Item(CustomerData.EMAIL_FIELD) = userEmail
        '        .Item(CustomerData.PHONE_FIELD) = "*"
        '        .Item(CustomerData.CITY_FIELD) = "*"
        '        .Item(CustomerData.ADDRESS_FIELD) = "*"
        '        .Item(CustomerData.STATE_FIELD) = "*"
        '        .Item(CustomerData.IDPAIS_FIELD) = "MX"
        '    End With
        '    dsc.Tables(CustomerData.CUSTOMER_TABLE).Rows.Add(cRow)
        '    Dim addCustomer As Boolean

        '    With New cCustomerSystem
        '        addCustomer = .createCustomer(dsc)
        '    End With

        '    If dsc.HasErrors Then
        '        strError = "Clientes: " & dsc.Tables(0).Rows(0).RowError
        '        insertAgent = False
        '    End If
        'End If

    End Function

    Public Function getAgentByUserId(ByVal idUser As Integer) As DataSet
        Dim Agentes As New DataSet
        Dim table As DataTable = New DataTable(USERAGENTS_TABLE)
        With table.Columns
            .Add("idAgente", GetType(System.Int32)).AllowDBNull = True
            .Add(Me.IDUSER_FIELD, GetType(System.Int32)).AllowDBNull = False
            .Add(Me.IDAGENCY_FIELD, GetType(System.Int32)).AllowDBNull = False
            .Add(Me.NAME_FIELD, GetType(System.String)).AllowDBNull = False
            .Add(Me.LASTNAME_FIELD, GetType(System.String)).AllowDBNull = False
            .Add("Permisos", GetType(System.String)).AllowDBNull = True
        End With
        Agentes.Tables.Add(table)

        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spAgent_GetByUserId"
                    .Connection = New SqlConnection(ConfigurationSettings.AppSettings("AgenciesConnection"))
                    Dim param As SqlParameter = New SqlParameter("@idUsuario", SqlDbType.Int)
                    param.Value = idUser
                    .Parameters.Add(param)
                End With
                .Fill(Agentes)
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
        End With
        Return Agentes
    End Function

    Public Function getAgentByIdAgency(ByVal idAgency As Integer) As DataSet
        Dim Agentes As New DataSet
        Dim table As DataTable = New DataTable(USERAGENTS_TABLE)
        With table.Columns
            .Add("idAgente", GetType(System.Int32)).AllowDBNull = True
            .Add(Me.IDUSER_FIELD, GetType(System.Int32)).AllowDBNull = False
            .Add(Me.IDAGENCY_FIELD, GetType(System.Int32)).AllowDBNull = False
            .Add(Me.NAME_FIELD, GetType(System.String)).AllowDBNull = False
            .Add(Me.LASTNAME_FIELD, GetType(System.String)).AllowDBNull = False
            .Add("Permisos", GetType(System.String)).AllowDBNull = True
        End With
        Agentes.Tables.Add(table)

        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spAgent_GetByAgencyId"
                    .Connection = New SqlConnection(ConfigurationSettings.AppSettings("AgenciesConnection"))
                    Dim param As SqlParameter = New SqlParameter("@idAgency", SqlDbType.Int)
                    param.Value = idAgency
                    .Parameters.Add(param)
                End With
                .Fill(Agentes)
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
        End With
        Return Agentes
    End Function

    Public Function updateAgent(ByVal idUser As Integer, ByVal userName As String, ByVal UserLastName As String, ByVal userEmail As String, ByRef strError As String) As Boolean
        Dim cadConn As String = ConexionSQL
        Dim conn As SqlConnection = Nothing
        Dim sp As String = "spAgent_UpdateByIdUsuario"
        Dim ds As New DataSet()
        conn = New SqlConnection(ConfigurationSettings.AppSettings("AgenciesConnection"))
        Dim da As New SqlDataAdapter(sp, conn)
        Try

            da.UpdateCommand = New SqlCommand()
            With da.UpdateCommand
                .Connection = conn
                .CommandText = sp
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter("@idUsuario", SqlDbType.Int, 6, ParameterDirection.Input, 0, 0, Nothing, Global.System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))
                .Parameters.Add(New SqlClient.SqlParameter("@Nombre", SqlDbType.NVarChar, 25, ParameterDirection.Input, 0, 0, Nothing, Global.System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))
                .Parameters.Add(New SqlClient.SqlParameter("@Apellido", SqlDbType.NVarChar, 25, ParameterDirection.Input, 10, 0, Nothing, Global.System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))

                .Parameters("@idUsuario").Value = idUser
                .Parameters("@Nombre").Value = userName
                .Parameters("@Apellido").Value = UserLastName

                .Connection.Open()
                .ExecuteNonQuery()
                .Connection.Close()
            End With
            updateAgent = True
        Catch ex As Exception
            da.UpdateCommand.Connection.Close()
            updateAgent = False
        End Try

        If updateAgent Then
            Dim cReader As SqlDataReader
            Dim dsc As CustomerData
            Dim idCliente As Integer
            Try
                With New cCustomerSystem
                    cReader = .GetCustomersByUserId(idUser)
                    While cReader.Read()
                        If cReader.GetString(3) = userEmail Then
                            idCliente = cReader.GetInt32(0)
                            Exit While
                        End If
                    End While

                    dsc = .GetCustomerById(idCliente)
                    With dsc.Tables(0).Rows(0)
                        .Item(CustomerData.NAME_FIELD) = userName
                        .Item(CustomerData.LASTNAME_FIELD) = UserLastName

                        .AcceptChanges()
                        .Item(CustomerData.NAME_FIELD) = userName
                    End With

                    updateAgent = .updateCustomer(dsc)
                End With
            Catch ex As Exception
                strError = "Actualizar Cliente: " & ex.Message
            Finally

            End Try
            
        End If
    End Function

    Private Function deleteAgent() As Boolean
        Return True
    End Function

    

    Private Function GetAddAgentCommand() As SqlCommand
        If insertCommand Is Nothing Then
            insertCommand = New SqlCommand("spAgent_Insert", New SqlConnection(ConfigurationSettings.AppSettings("AgenciesConnection")))

            insertCommand.CommandType = CommandType.StoredProcedure
            With insertCommand.Parameters
                .Add(New SqlParameter("@idAgente", SqlDbType.Int))
                .Add(New SqlParameter("@idAgencia", SqlDbType.Int))
                .Add(New SqlParameter("@idUsuario", SqlDbType.Int))
                .Add(New SqlParameter("@Nombre", SqlDbType.NVarChar, NAME_LENGTH))
                .Add(New SqlParameter("@Apellido", SqlDbType.NVarChar, LASTNAME_LENGTH))
                .Add(New SqlParameter("@Permisos", SqlDbType.NVarChar, 7))

                'Definimos el mapa en la estructura del dataset
                .Item("@idAgente").SourceColumn = "idAgente"
                .Item("@idUsuario").SourceColumn = IDUSER_FIELD
                .Item("@idAgencia").SourceColumn = IDAGENCY_FIELD
                .Item("@Nombre").SourceColumn = NAME_FIELD
                .Item("@Apellido").SourceColumn = LASTNAME_FIELD
                .Item("@Permisos").SourceColumn = "Permisos"
            End With
        End If

        GetAddAgentCommand = insertCommand
    End Function

    Private Function GetUpdateAgentCommand() As SqlCommand
        If updateCommand Is Nothing Then
            updateCommand = New SqlCommand("spAgent_UpdateByIdUsuario", New SqlConnection(ConfigurationSettings.AppSettings("AgenciesConnection")))

            updateCommand.CommandType = CommandType.StoredProcedure
            With updateCommand.Parameters
                .Add(New SqlParameter("@idUsuario", SqlDbType.Int))
                .Add(New SqlParameter("@Nombre", SqlDbType.NVarChar, NAME_LENGTH))
                .Add(New SqlParameter("@Apellido", SqlDbType.NVarChar, LASTNAME_LENGTH))

                'Definimos el mapa en la estructura del dataset
                .Item("@idUsuario").SourceColumn = "idUsuario"
                .Item("@Nombre").SourceColumn = NAME_FIELD
                .Item("@Apellido").SourceColumn = LASTNAME_FIELD
            End With
        End If

        GetUpdateAgentCommand = updateCommand
    End Function
End Class
