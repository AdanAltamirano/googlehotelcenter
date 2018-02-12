Imports System
Imports System.Xml
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Partial Class RSS
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
#Region "Propiedades"
    Private intIdCanal As Integer
    Private intIdIdioma As Integer
    Private dateFecha As DateTime
    Private NoCar As Integer
    Private Property idCanal() As Integer
        Get
            Return intIdCanal
        End Get
        Set(ByVal Value As Integer)
            intIdCanal = Value
        End Set
    End Property
    Private Property idIdioma() As Integer
        Get
            Return intIdIdioma
        End Get
        Set(ByVal Value As Integer)
            intIdIdioma = Value
        End Set
    End Property
    Private Property fecha() As DateTime
        Get
            Return dateFecha
        End Get
        Set(ByVal Value As DateTime)
            dateFecha = Value
        End Set
    End Property
    Private Property NumeroDeCaracteres() As Integer
        Get
            Return NoCar
        End Get
        Set(ByVal Value As Integer)
            NoCar = Value
        End Set
    End Property
#End Region


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''Put user code to initialize the page here            
        'Page.Response.ContentType = "text/xml"
        'Establecemos los parametros
        If Not Page.Request.QueryString("idC") Is Nothing Then
            Try
                idCanal = CType(Page.Request.QueryString("idC"), Integer)
            Catch ex As Exception
                idCanal = -1
            End Try
        Else
            idCanal = -1
        End If

        If Not Page.Request.QueryString("idI") Is Nothing Then
            Try
                idIdioma = CType(Page.Request.QueryString("idI"), Integer)
            Catch ex As Exception
                idIdioma = -1
            End Try
        Else
            idIdioma = -1
        End If

        If Not AppSettings("LongPreviewNota") Is Nothing Then
            NumeroDeCaracteres = CType(AppSettings("LongPreviewNota"), Integer)
        Else
            NumeroDeCaracteres = 200
        End If

        'Dim rss As New GeneradorRSS(Page.Response.Output)
        'With rss
        '    'Leemos las noticias mas recientes
        '    Dim dsCanales As DataSet = New DataSet
        '    dsCanales = GetFeeds(idIdioma)
        '    If Not dsCanales Is Nothing AndAlso dsCanales.Tables(0).Rows.Count > 0 Then
        '        Dim drFeed As DataRow
        '        drFeed = dsCanales.Tables(0).Rows(0)
        '        'Iniciamos las propiedades del canal y lo iniciamos
        '        .TituloCanal = drFeed("Title")
        '        .LinkCanal = drFeed("Link")
        '        .DescripcionCanal = drFeed("Description")                
        '        .CopyRight = ""
        '        .WebMaster = "administrator@hotelesmision.com"
        '        .Generador = "Administrator"
        '        .UltimaActualizacion = Today
        '        .IniciarCanal()

        '        'Buscamos las notas del canal
        '        Dim dsNotesRSS As DataSet = New DataSet
        '        dsNotesRSS = LoadNotesRSS(idCanal, idIdioma, Today)
        '        If Not dsNotesRSS Is Nothing AndAlso dsNotesRSS.Tables(0).Rows.Count > 1 Then
        '            For Each dr As DataRow In dsNotesRSS.Tables(0).Rows

        '                Dim TAB As System.Web.UI.HtmlControls.HtmlTextArea
        '                TAB = New System.Web.UI.HtmlControls.HtmlTextArea
        '                TAB.InnerHtml = CType(dr("Title"), String)
        '                Dim cadHTML As String
        '                cadHTML = TAB.InnerText
        '                cadHTML = System.Text.RegularExpressions.Regex.Replace(cadHTML, "<[^>]*>", String.Empty)
        '                dr("Title") = cadHTML

        '                TAB.InnerHtml = CType(dr("Description"), String)
        '                cadHTML = TAB.InnerText
        '                cadHTML = System.Text.RegularExpressions.Regex.Replace(cadHTML, "<[^>]*>", String.Empty)
        '                dr("Description") = cadHTML

        '                'pasar los valores a las propiedades antes de escribir el articulo
        '                .TituloArticulo = dr("Title")
        '                .LinkArticulo = dr("Link")
        '                .DescripcionArticulo = dr("Description")
        '                .Autor = dr("Author")
        '                .FechaPublicacion = dr("pubDate")
        '                .Contenido = ""
        '                .EscribirArticulo()
        '            Next
        '        Else
        '            'No hay noticias para leer                    
        '        End If
        '        .CerrarCanal()
        '    Else
        '        'No existe el canal Solicitado        
        '    End If
        'End With
        'Response.End()


        'Declaramos el objeto XML (o escritor de XML)
        'donde crearemos el documento RSS 2.0
        Dim rss As New GeneradorRSS2
        Dim Escritor As XmlTextWriter = New XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8)

        'Leemos las noticias mas recientes
        Dim dsCanales As DataSet = New DataSet
        dsCanales = GetFeeds(idIdioma)
        If Not dsCanales Is Nothing AndAlso dsCanales.Tables(0).Rows.Count > 0 Then
            Dim drFeed As DataRow
            drFeed = dsCanales.Tables(0).Rows(0)
            'Iniciamos las propiedades del canal y lo iniciamos            
            rss.EscribirInicioRSS(Escritor, drFeed("Title"), drFeed("description"), drFeed("Link"))

            'Buscamos las notas del canal
            Dim dsNotesRSS As DataSet = New DataSet
            dsNotesRSS = LoadNotesRSS(idCanal, idIdioma, Today)
            If Not dsNotesRSS Is Nothing AndAlso dsNotesRSS.Tables(0).Rows.Count > 1 Then
                For Each dr As DataRow In dsNotesRSS.Tables(0).Rows

                    Dim TAB As System.Web.UI.HtmlControls.HtmlTextArea
                    TAB = New System.Web.UI.HtmlControls.HtmlTextArea
                    TAB.InnerHtml = CType(dr("Title"), String)
                    Dim cadHTML As String
                    cadHTML = TAB.InnerText
                    cadHTML = System.Text.RegularExpressions.Regex.Replace(cadHTML, "<[^>]*>", String.Empty)
                    dr("Title") = cadHTML

                    TAB.InnerHtml = CType(dr("Description"), String)
                    cadHTML = TAB.InnerText
                    cadHTML = System.Text.RegularExpressions.Regex.Replace(cadHTML, "<[^>]*>", String.Empty)
                    If cadHTML.Length > NumeroDeCaracteres Then
                        cadHTML = cadHTML.Substring(1, 200) & "..."
                    End If
                    dr("Description") = cadHTML
                    'pasar los valores a las propiedades antes de escribir el articulo
                    rss.AddElementosRSS(Escritor, dr("Title"), dr("Link"), dr("Description"), True, dr("PubDate"))
                Next
            Else
                'No hay noticias para leer                    
            End If
            'Cerramos el documento RSS
            rss.EscribirFinRSS(Escritor)
        Else
            'No existe el canal Solicitado        
        End If

        'Volcamos el contenido en el objeto y lo cerramos
        Escritor.Flush()
        Escritor.Close()

        'Especificamos la codificación del
        'documento y habilitamos la caché
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.ContentType = "text/xml"
        'Response.Cache.SetCacheability(HttpCacheability.Public)

        'Enviamos al cliente la salida del buffer
        Response.End()
    End Sub

    Public Function LoadNotesRSS(ByVal idCanal As Integer, ByVal idIdioma As Integer, ByVal fecha As DateTime) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetNotesRSS", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@idCanal", idCanal)
                .Parameters.Add("@idIdioma", idIdioma)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
            Return ds
        Catch ex As Exception
        End Try
    End Function

    Private Function GetFeeds(ByVal idIdioma As Integer) As DataSet
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetFeeds", New SqlConnection(AppSettings("PortalConnectionString")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@idIdioma", idIdioma)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
            Return ds
        Catch ex As Exception
        End Try
    End Function
End Class
