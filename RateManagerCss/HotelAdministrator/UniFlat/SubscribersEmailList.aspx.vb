Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class SubscribersEmailList
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not Me.IsPostBack Then
            Dim ds As DataSet = getSubscribersDataSet()
            Me.dgEmailList.DataSource = ds
            Me.dgEmailList.DataBind()
            Session("cCom") = ds
        End If
        If Me.dgEmailList.Items.Count > 0 Then
            Me.btnExport.Enabled = True
            Me.btnExport.Attributes.Item("onclick") = "window.open('exportSubscribers.aspx','list'); return false;"
        Else
            Me.btnExport.Enabled = False
            Me.btnExport.Attributes.Item("onclick") = "return false;"
        End If

        btnExport.Text = PortalCulture.GetString("00591")
        lbltitle.Text = PortalCulture.GetString("01465")
    End Sub

    Private Function getSubscribersDataSet() As DataSet
        Dim ds As New DataSet
        Dim strSQL As String = "SELECT idHotel, Email, convert(varchar,Fecha,120) Fecha FROM WebSite_Contactos where idHotel = @idHotel order by fecha"
        Dim sqlCommand As New SqlCommand(strSQL, New SqlConnection(AppSettings("HotelConnection")))
        Try
            sqlCommand.Parameters.Add(New SqlParameter("@idHotel", Me.cInfoActual.Hotel))
            Dim sqlAdapter As New SqlDataAdapter(sqlCommand)

            sqlAdapter.Fill(ds)
        Catch ex As Exception
        Finally
            If sqlCommand.Connection.State = ConnectionState.Open Or sqlCommand.Connection.State = ConnectionState.Broken Then
                sqlCommand.Connection.Close()
            End If
        End Try

        Return ds
    End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.Write("<script>window.open(""" & GeRequestApplicationPath(String.Concat("/HotelAdministrator/UniFlat/exportSubscribers.aspx?ex=", Me.cInfoActual.Hotel.ToString())) & """);</script>")

        'Response.Clear()
        'Response.AddHeader("content-disposition", "attachment;filename=EmailList.xls")
        'Response.Charset = ""
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/vnd.xls"
        'Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter
        'Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        'dgEmailList.RenderControl(htmlWrite)
        'Response.Write(stringWrite.ToString())
        'Response.End()
    End Sub


End Class
