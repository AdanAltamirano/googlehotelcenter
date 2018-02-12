

Partial Class exportSubscribers
    Inherits System.Web.UI.Page

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

    Private Property cCom() As DataSet
        Get
            Return Session("cCom")
        End Get
        Set(ByVal Value As DataSet)
            Session("cCom") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not IsPostBack Then
                Response.Clear()
                Response.Charset = ""
                Response.ContentType = "application/vnd.ms-excel"
                Dim stringWrite As New System.IO.StringWriter
                Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
                With dgEmailList
                    .DataSource = cCom
                    .DataBind()
                    .RenderControl(htmlWrite)
                End With
                Response.Write(stringWrite.ToString)
            End If
        Catch ex As Exception
            Response.Clear()
        Finally
            Response.End()
        End Try
    End Sub

End Class
