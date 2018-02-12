Imports System.Configuration.ConfigurationManager

Partial Class Index
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
    Protected WithEvents CtrlHeader1 As ctrlHeader
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        CtrlHeader1.Panel = Session("HeaderPanel") 'Me.HeaderPanel
        Session("menu") = "false"
        'para las consultas de reservaciones ...Default.aspx?Url=Hotel_Administrator/Pages/ReservationDetails.aspx?qs=1404
        Dim _Url As String = ""
        If Not IsPostBack Then

            _Url = Request.QueryString("Url")
            If Not _Url Is Nothing AndAlso _Url <> "" Then
                Session("urlCurrent") = _Url
            End If

            InitIdiomaLogin = AppSettings("DefaultLanguajeLogin")
            If Not Request.QueryString("ididioma") Is Nothing AndAlso Request.QueryString("ididioma") = 1 Then
                PortalCulture.SetCulture("es-MX")
                Me.IdIdiomaLogin = 1
            ElseIf Not Request.QueryString("ididioma") Is Nothing AndAlso Request.QueryString("ididioma") = 2 Then
                PortalCulture.SetCulture("en-US")
                Me.IdIdiomaLogin = 2

            End If

            If Request.QueryString("SRV") = "S" Then
                Dim msginfo As String
                With Me.cInfoActual
                    msginfo = .HotelName & "<br>"
                    msginfo &= .Address & "<br>"
                    msginfo &= .City & "," & .State & "<br>"
                    msginfo &= PortalCulture.GetString("00162", True) & "&nbsp;" & .Contact & "<br>"
                    msginfo &= PortalCulture.GetString("00163", True) & "&nbsp;" & .Email & "<br>"
                    msginfo &= PortalCulture.GetString("00164", True) & "&nbsp;" & .Phone & "<br>"
                    msginfo &= PortalCulture.GetString("00522", True) & "&nbsp;" & IIf(String.IsNullOrEmpty(.Url), "-", .Url) & "<br>"
                    msginfo &= PortalCulture.GetString("00074", True) & "&nbsp;" & .Rooms.ToString & "<br>"
                End With
                Context.Response.Write(msginfo)
                Response.End()
            End If
        End If

        If Not Session("urlCurrent") Is Nothing Then
            If _Url <> "" Then
                Me.frmPrincipal.Attributes.Add("src", GeRequestApplicationPath(String.Concat("/", _Url)))
            Else
                Me.frmPrincipal.Attributes.Add("src", GeRequestApplicationPath("/Portal/Pages/Welcome.aspx"))
            End If
        End If


    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        Dim b As Char = ControlChars.Cr
        Dim s As New System.Text.StringBuilder
        With s
            .Append("<script>" & b)
            .Append("	  function UpdateMe()" & b)
            .Append("	  { " & b)
            .Append("        //recarga la pagina" & b)
            .Append("        self.location.href='" & (GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/default.aspx';"))).Replace("//", "/") & b)
            .Append("     }" & b)
            .Append("		</script>" & b)
        End With
        writer.Write(s.ToString)
    End Sub

    Private Sub btnreload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreload.Click
        CtrlHeader1.Panel = Session("HeaderPanel")
    End Sub


End Class
