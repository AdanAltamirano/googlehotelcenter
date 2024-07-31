Partial Class Noticias
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
    Dim News As DataSet = New DataSet    
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            ddlOrderNews.Items.Add(New ListItem(PortalCulture.GetString("00679"), 1))
            ddlOrderNews.Items.Add(New ListItem(PortalCulture.GetString("00680"), 2))
        End If
        If PortalCulture.GetIDCulture() = 1 Then
            News.ReadXml(Server.MapPath("Noticias_es.xml"))
        Else
            News.ReadXml(Server.MapPath("Noticias_en.xml"))
        End If
        If ddlOrderNews.SelectedValue = 1 Then
            News.Tables(0).DefaultView.Sort = "DateNews DESC"
        Else
            News.Tables(0).DefaultView.Sort = "DateNews ASC"
        End If
        GridNews.DataSource = News.Tables(0).DefaultView
        GridNews.DataBind()
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.InnerHtml = PortalCulture.GetString("00672")
        lblOrderNews.Text = PortalCulture.GetString("00678")
    End Sub
    Private Sub GridNews_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GridNews.PageIndexChanged
        Try
            GridNews.CurrentPageIndex = e.NewPageIndex
            Call LoadNews()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub LoadNews()
        GridNews.DataSource = News.Tables(0).DefaultView
        GridNews.DataBind()
    End Sub
End Class
