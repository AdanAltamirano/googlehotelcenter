Partial Class ReportAvailXLS
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataList1 As System.Web.UI.WebControls.DataList

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            txtInicio.Text = Now.Date.ToString("MM/dd/yyyy")
        End If
        btnEjecutar.OnClientClick = String.Format("return FireUpdateStatus('{0}');", PortalCulture.GetString("00608"))
    End Sub

    Private Sub btnEjecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEjecutar.Click
        If txtInicio.Text <> "" Then
            Dim pth As String
            With New clsGetAvail
                pth = .GenerateReportAvail(Me.cInfoActual.Hotel, Me.cInfoActual.HotelName, Me.cInfoActual.Empresa, CDate(txtInicio.Text), ddlDisplayType.SelectedValue, ddlDisplayTime.SelectedValue)
            End With
            lnkRepXLS.Visible = True
            If pth <> "" Then
                lnkRepXLS.NavigateUrl = GeRequestApplicationPath(String.Concat("/Reports/", pth))
            End If
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.ddlDisplayType.Items(0).Text = PortalCulture.GetString("00533")
        Me.ddlDisplayType.Items(1).Text = PortalCulture.GetString("00535")
        Me.ddlDisplayType.Items(2).Text = PortalCulture.GetString("00534")
        Me.ddlDisplayType.Items(3).Text = PortalCulture.GetString("00536")

        lblDesde.Text = PortalCulture.GetString("00108")

        lblTitulo.Text = PortalCulture.GetString("00590")

        lblDisplay.Text = PortalCulture.GetString("00541", True)

        lnkRepXLS.Text = PortalCulture.GetString("00591")
        Me.btnEjecutar.Text = PortalCulture.GetString("01010")
    End Sub
End Class
