Imports System.Configuration
Imports System.Xml

Partial Class logon
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Protected ctrlLogon1 As ctrlLogon




    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Activamos el enter para el submit
        Me.DefaultButton(ctrlLogon1.EmailText, Me.btnSignIn)
        Me.DefaultButton(ctrlLogon1.PasswordText, Me.btnSignIn)
        If Not IsPostBack Then
            'If Request("ReturnURL") <> "" Then

            '    HttpContext.Current.Session.Clear()
            '    HttpContext.Current.Session.Abandon()
            '    If HttpContext.Current.User.Identity.IsAuthenticated Then
            '        System.Web.Security.FormsAuthentication.SignOut()
            '    End If
            'End If

            If Not Request("ReturnURL") Is Nothing AndAlso HttpContext.Current.User.Identity.Name <> "" Then
                Me.lblDenyAccess1.Visible = True
                Me.lblDenyAccess2.Visible = True
            Else
                Me.lblDenyAccess1.Visible = False
                Me.lblDenyAccess2.Visible = False
            End If
        End If
    End Sub

    Private Sub btnSignIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSignIn.Click
        Me.ctrlLogon1.Login()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTituloLog.Text = PortalCulture.GetString("00174")
        Me.lblDenyAccess1.Text = PortalCulture.GetString("00175")
        Me.lblDenyAccess2.Text = PortalCulture.GetString("00176")
        Me.btnSignIn.Text = PortalCulture.GetString("00177")
        Me.HyperLink1.Text = PortalCulture.GetString("00178")
        TDen.Visible = False
        TDes.Visible = False
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim paso As Boolean = False

        Try
            Dim txtRead As XmlTextReader = New XmlTextReader(Server.MapPath("/RateManager/Data/Groups.xml"))
            ds.ReadXml(txtRead)
            txtRead.Close()
            Dim url As String = Request.RawUrl.ToUpper

            For Each dr In ds.Tables(0).Rows

                If Session("GroupId").ToString.ToUpper = dr("IDGROUP").ToString.ToUpper Then
                    TDes.InnerHtml = TDes.InnerHtml.Replace("[LABELGROUPS]", dr("CustomContact"))
                    TDen.InnerHtml = TDen.InnerHtml.Replace("[LABELGROUPS]", dr("CustomContact"))
                    paso = True
                End If
            Next
        Catch ex As Exception

        End Try
        If Not paso Then
            TDes.InnerHtml = TDes.InnerHtml.Replace("[LABELGROUPS]", "")
            TDen.InnerHtml = TDen.InnerHtml.Replace("[LABELGROUPS]", "")
        End If
        Try
            If PortalCulture.GetCulture.ToString.Substring(0, 2) = "es" Then
                TDes.Visible = True
            ElseIf PortalCulture.GetCulture.ToString.Substring(0, 2) = "en" Then
                TDen.Visible = True
            End If
        Catch
        End Try

    End Sub

    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs)
    '    Me.lblTituloLog.Text = PortalCulture.GetString("00174", False)
    '    Me.lblDenyAccess1.Text = PortalCulture.GetString("00175", False)
    '    Me.lblDenyAccess2.Text = PortalCulture.GetString("00176", False)
    '    Me.btnSignIn.Text = PortalCulture.GetString("00177", False)
    '    Me.HyperLink1.Text = PortalCulture.GetString("00178", False)
    '    Me.TDen.Visible = False
    '    Me.TDes.Visible = False
    'Dim set As New DataSet
    '    Dim flag As Boolean = False
    '    Try
    '        Dim reader As New XmlTextReader(Me.Server.MapPath("/RateManager/Data/Groups.xml"))
    '        [set].ReadXml(reader)
    '        reader.Close()
    '        Dim str As String = Me.Request.RawUrl.ToUpper
    '        Try
    '            Dim row As DataRow
    '            For Each row In [set].Tables.Item(0).Rows
    '                If (StringType.StrCmp(Me.Session.Item("GroupId").ToString.ToUpper, row.Item("IDGROUP").ToString.ToUpper, False) = 0) Then
    '                    Me.TDes.InnerHtml = Me.TDes.InnerHtml.Replace("[LABELGROUPS]", StringType.FromObject(row.Item("CustomContact")))
    '                    Me.TDen.InnerHtml = Me.TDen.InnerHtml.Replace("[LABELGROUPS]", StringType.FromObject(row.Item("CustomContact")))
    '                    flag = True
    '                End If
    '            Next
    '        Finally
    '            Dim enumerator As IEnumerator
    '            If TypeOf enumerator Is IDisposable Then
    '                DirectCast(enumerator, IDisposable).Dispose()
    '            End If
    '        End Try
    '    Catch exception1 As Exception
    '        ProjectData.SetProjectError(exception1)
    '        Dim exception As Exception = exception1
    '        ProjectData.ClearProjectError()
    '    End Try
    '    If Not flag Then
    '        Me.TDes.InnerHtml = Me.TDes.InnerHtml.Replace("[LABELGROUPS]", "")
    '        Me.TDen.InnerHtml = Me.TDen.InnerHtml.Replace("[LABELGROUPS]", "")
    '    End If
    '    If (StringType.StrCmp(PortalCulture.GetCulture.ToString.Substring(0, 2), "es", False) = 0) Then
    '        Me.TDes.Visible = True
    '    ElseIf (StringType.StrCmp(PortalCulture.GetCulture.ToString.Substring(0, 2), "en", False) = 0) Then
    '        Me.TDen.Visible = True
    '    End If
    'End Sub



End Class


