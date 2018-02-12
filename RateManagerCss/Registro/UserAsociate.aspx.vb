Imports System.Security.Cryptography
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports System.IO
Partial Class UserAsociate
    Inherits PaginaBase
    Protected WithEvents CtlMensajes1 As ctlMensajes
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(Me.pages.Home)
        If Not Me.IsPostBack Then
            dgUsuarios.CurrentPageIndex = 0
            dgUsuarios.SelectedIndex = -1
            GetUsuarios()
            lblError.Visible = False
        End If
        CtlMensajes1.Visible = False
        Me.ResizefrmPrincipal()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitleForm.Text = PortalCulture.GetString("00847")
        lblagregar.InnerHtml = PortalCulture.GetString("00849")
        lblemail.InnerHtml = PortalCulture.GetString("00163", True)
        btnSave.Text = PortalCulture.GetString("M000515")
    End Sub
    Private Sub GetUsuarios()
        Dim cUsuarios As Portal.General.Common.Data.UserData
        Dim sTabla As String = Portal.General.Common.Data.UserCompanyData.USERCOMPANY_TABLE
        cUsuarios = (New cUserCompanySystem).GetUsersByIdEmpresa(MyBase.cInfoActual.Empresa)
        If Not cUsuarios Is Nothing AndAlso cUsuarios.Tables(sTabla).Rows.Count > 0 Then
            dgUsuarios.DataSource = cUsuarios.Tables(sTabla)
        Else
            dgUsuarios.DataSource = (New Portal.General.Common.Data.UserData).Tables(cUsuarios.USER_TABLE)
        End If
        dgUsuarios.DataBind()
    End Sub
    Private Sub dgUsuarios_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgUsuarios.ItemCommand
        If e.CommandName = "Delete" Then
            BorraUsuario(CType(e.Item.Cells(1).Text, Integer), e.Item.Cells(2).Text)
        End If
    End Sub

    Private Sub dgUsuarios_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsuarios.ItemCreated
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
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(2).Text = PortalCulture.GetString("00163", False) 'Correo electrónico
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lnk As LinkButton
            lnk = e.Item.Cells(1).FindControl("lnkEliminar2")
            Dim lk As LinkButton = e.Item.Cells(1).FindControl("lnkEliminar")

            If Not lnk Is Nothing AndAlso Not lk Is Nothing Then
                lk.OnClientClick = String.Format("return confirm('{0}');", PortalCulture.GetString("00851"))
                'lk.NavigateUrl = CtlMensajes1.getShow(lnk.ClientID, PortalCulture.GetString("00850"), _
                ' PortalCulture.GetString("00851"))
                lk.Text = PortalCulture.GetString("00848")
            End If

        End If
    End Sub
    Private Sub BorraUsuario(ByVal IdUser As Integer, ByVal sUsuario As String)
        lblError.Visible = False
        Dim sError As String
        With New cUserCompanySystem
            If Not .DeleteUserCompany(IdUser, MyBase.cInfoActual.Empresa, sError) Then
                lblError.Text = PortalCulture.GetString("00852")
                lblError.Visible = True
            Else
                CType(Me.Page, PaginaBase).guardalog("/Registro/UserAsociate.aspx", PaginaBase.acciones.Eliminar, String.Format("Desasociar el usuario {0}", sUsuario), "", "", "")
            End If
        End With
        GetUsuarios()
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        lblError.Visible = False
        If txtEmail.Text.Trim <> "" Then
            If (New cUserCompanySystem).createUserCompany(PortalCulture.GetCulture.ToString, txtEmail.Text.Trim, MyBase.cInfoActual.Empresa) Then
                CType(Me.Page, PaginaBase).guardalog("/Registro/UserAsociate.aspx", PaginaBase.acciones.Crear, String.Format("Asociar usuario a hotel {0}", txtEmail.Text), "", "", "")
                If ConfigurationManager.AppSettings("idSegmento") = 4 Then
                    Fillcorreo(txtEmail.Text, MyBase.cInfoActual.Empresa)
                End If
                txtEmail.Text = ""
                GetUsuarios()
            Else
                lblError.Text = PortalCulture.GetString("00853")
                lblError.Visible = True
            End If
        End If


    End Sub

    Private Sub dgUsuarios_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUsuarios.PageIndexChanged
        dgUsuarios.CurrentPageIndex = e.NewPageIndex
        dgUsuarios.SelectedIndex = -1
        GetUsuarios()
    End Sub

    Private Sub Fillcorreo(ByVal sEmail As String, ByVal iEmpresa As Integer)
        Dim ci As System.Globalization.CultureInfo

        Dim Mail As emailTemplates.Template
        Dim ds As UserData
        Dim sErr As String = ""
        Mail = New emailTemplates.Template
        Mail.TemplateName = "TH_WORLDESTINATION_NEW_HOTEL_USER"

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

        Dim dsEmpresa As New EmpresaDatos
        dsEmpresa = (New EmpresaSistema).GetCompanyById(iEmpresa)

        Try
            Mail.To = sEmail

            'Mail.SubjectParam = "Welcome to World Destination Travel Limites"
            Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Mail.Html = True
            Mail.AddParameter("USEREMAIL") = sEmail
            Mail.AddParameter("HOTELNAME") = dsEmpresa.Tables(EmpresaDatos.COMPANY_TABLE).Rows(0).Item(EmpresaDatos.FIELD_Nombre)



            Mail.Send()

        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        End Try
    End Sub

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

End Class


