Imports System.Configuration.ConfigurationManager
Imports System.Text.RegularExpressions

Partial Class CtrlIdiomaFCk
    Inherits UserControlBase

    Private _Height As Integer = 0
    Private _Width As Integer = 0
    Private conte As String
    'Private Const PATHSCR As String = "/Portal/Modules/Contenido"
    Private Const PATHSCR As String = "/EditorFCk"

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CmdCancelar As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Property IdIndice() As Integer
        Get
            Return viewstate("IdIndice")
        End Get
        Set(ByVal Value As Integer)
            viewstate("IdIndice") = Value
            If Value <> 0 Then CargaDatos()
        End Set
    End Property

    Public Property TextPlain() As Integer
        Get
            If ViewState("TextPlain") Is Nothing Then ViewState("TextPlain") = 1
            Return ViewState("TextPlain")
        End Get
        Set(ByVal Value As Integer)
            ViewState("TextPlain") = Value
        End Set
    End Property

    Public Property isHtml() As Integer
        Get
            If ViewState("isHtml") Is Nothing Then ViewState("isHtml") = 1
            Return ViewState("isHtml")
        End Get
        Set(ByVal Value As Integer)
            ViewState("isHtml") = Value
        End Set
    End Property

    Public Property textodefault() As String
        Get
            Return textDefault()
        End Get
        Set(ByVal Value As String)
            Select Case AppSettings("DefaultLanguage")
                Case "en-US"
                    If Trim(Me.FCKeditorEN.Value) = "" Then
                        Me.FCKeditorEN.Value = Value
                    End If
                Case "es-MX"
                    If Trim(Me.FCKeditorES.Value) = "" Then
                        Me.FCKeditorES.Value = Value
                    End If
            End Select
        End Set
    End Property

    Public Property textoIngles() As String
        Get
            Return HtmlRegex(FCKeditorEN.Value) 'Me.txtenglish.Text
        End Get
        Set(ByVal Value As String)
            Me.txtenglish.Text = Value
            FCKeditorEN.Value = Value
        End Set
    End Property

    Public ReadOnly Property textoInglesTEXT() As String
        Get
            Return RegexHtmlToText(FCKeditorEN.Value) 'Me.txtenglish.Text
        End Get
    End Property

    Public Property textoInglesUpdate() As Boolean
        Get
            Return viewstate("ING_UPD")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("ING_UPD") = Value
        End Set
    End Property

    Public Property textoEspañol() As String
        Get
            Return HtmlRegex(FCKeditorES.Value) 'Me.txtspanish.Text
        End Get
        Set(ByVal Value As String)
            Me.txtspanish.Text = Value
            FCKeditorES.Value = Value
        End Set
    End Property

    Public ReadOnly Property textoEspañolTEXT() As String
        Get
            Return RegexHtmlToText(FCKeditorES.Value) 'Me.txtenglish.Text
        End Get
    End Property

    Public Property textoEspañolUpdate() As Boolean
        Get
            Return viewstate("ESP_UPD")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("ESP_UPD") = Value
        End Set
    End Property

    Function SoloIdiomaDefault1() As Boolean
        If Me.IdIdioma = 1 Then
            DivUpload.Style.Item("display") = ""
            DivUpload.Attributes.Add("class", "TabSelected")
            DivSelect.Style.Item("display") = "none"
            tblspanish.Style.Item("display") = ""
            tblenglish.Style.Item("display") = "none"
        Else
            DivUpload.Style.Item("display") = "none"
            DivSelect.Style.Item("display") = ""
            DivSelect.Style.Item("class") = "TabSelected"
            tblspanish.Style.Item("display") = "none"
            tblenglish.Style.Item("display") = ""
        End If
        Return True
    End Function

    Private Function textDefault() As String
        'TODO POR MIENTRAS
        Select Case AppSettings("DefaultLanguage")
            Case "en-US"
                Return Me.txtenglish.Text.Trim()
            Case "es-MX"
                Return Me.txtspanish.Text.Trim()
        End Select
        Return ""
    End Function

    Public ReadOnly Property ReturnNameTxtEn() As String
        Get
            Return Me.FCKeditorEN.ClientID()
        End Get
    End Property

    Public ReadOnly Property ReturnNameTxtEs() As String
        Get
            Return Me.FCKeditorES.ClientID
        End Get
    End Property

    Public Property Height() As Integer
        Get
            Return _Height
        End Get
        Set(ByVal Value As Integer)
            Me._Height = Value
        End Set
    End Property

    Public Property IdIdioma() As Integer
        Get
            Return ViewState("IdIdioma")
        End Get
        Set(ByVal Value As Integer)
            ViewState("IdIdioma") = Value
        End Set
    End Property

    Public Property Width() As String
        Get
            Return _Width
        End Get
        Set(ByVal Value As String)
            _Width = Value
        End Set
    End Property


    Public Property DatosAutos() As Integer
        Get
            If ViewState("_DatosAutos") Is Nothing Then ViewState("_DatosAutos") = 1
            Return ViewState("_DatosAutos")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_DatosAutos") = Value
        End Set
    End Property

    Public Property IsMultiline() As Boolean
        Get
            Return CBool(Me.txtenglish.TextMode)
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                Me.txtenglish.TextMode = TextBoxMode.MultiLine
                Me.txtspanish.TextMode = TextBoxMode.MultiLine
            Else
                Me.txtenglish.TextMode = TextBoxMode.SingleLine
                Me.txtspanish.TextMode = TextBoxMode.SingleLine
            End If
        End Set
    End Property

    Function RegexHtmlToText(ByVal src As String) As String
        Dim sHtml As String
        Const LF As String = Chr(10)
        Const CR As String = Chr(13)
        Dim pattern As String = "\s+"
        Dim replacement As String = " "
        Dim rgx As New Regex(pattern)


        Try
            sHtml = src
            sHtml = sHtml.Replace("&amp;", "&")
            sHtml = sHtml.Replace("&aacute;", "á").Replace("&eacute;", "é").Replace("&iacute;", "í")
            sHtml = sHtml.Replace("&oacute;", "ó").Replace("&uacute;", "ú").Replace("&Aacute;", "Á")
            sHtml = sHtml.Replace("&Eacute;", "É").Replace("&Iacute;", "Í").Replace("&Oacute;", "Ó")
            sHtml = sHtml.Replace("&Uacute;", "Ú").Replace("&ntilde;", "ñ").Replace("&Ntilde;", "Ñ")
            sHtml = sHtml.Replace("&iquest;", "¿").Replace("¡", "&iexcl;").Replace("'", "\'").Replace("""", "\""")
            sHtml = Regex.Replace(sHtml, "&(?ni:\#((x([\dA-F]){1,5})|(104857[0-5]|10485[0-6]\d|1048[0-4]\d\d|104[0-7]\d{3}|10[0-3]\d{4}|0?\d{1,6}))|([A-Za-z\d.]{2,31}));|<[^>]*>", "")
            sHtml = sHtml.Replace(vbCr, "").Replace(vbCrLf, "").Replace(vbLf, "")
            sHtml = rgx.Replace(sHtml, replacement)

            src = sHtml
        Catch ex As Exception
        End Try
        Return (src)

    End Function

    Function HtmlRegex(ByVal src As String) As String
        Dim sHtml As String
        Const LF As String = Chr(10)
        Const CR As String = Chr(13)


        Try
            sHtml = src
            If isHtml Then
                '// Remove the tags html, header and title (clearing attributes)

                sHtml = Regex.Replace(sHtml, "( )+", " ")
                sHtml = Regex.Replace(sHtml, "<( )*html([^>])*>", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<( )*(/)( )*html( )*>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "<( )*head([^>])*>", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<( )*(/)( )*head( )*>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<head>).*(</head>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "<( )*title([^>])*>", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<( )*(/)( )*title( )*>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<title>).*(</title>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                '// remove all scripts (clearing attributes)
                sHtml = Regex.Replace(sHtml, "<( )*script([^>])*>", "<script>", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<( )*(/)( )*script( )*>)", "</script>", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<script>).*(</script>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "<( )*body([^>])*>", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = Regex.Replace(sHtml, "(<( )*(/)( )*body( )*>)", "", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                sHtml = sHtml.Replace(LF, "").Replace(CR, "")
            Else
                sHtml = sHtml.Replace("&amp;", "&")
                sHtml = sHtml.Replace("&aacute;", "á").Replace("&eacute;", "é").Replace("&iacute;", "í")
                sHtml = sHtml.Replace("&oacute;", "ó").Replace("&uacute;", "ú").Replace("&Aacute;", "Á")
                sHtml = sHtml.Replace("&Eacute;", "É").Replace("&Iacute;", "Í").Replace("&Oacute;", "Ó")
                sHtml = sHtml.Replace("&Uacute;", "Ú").Replace("&ntilde;", "ñ").Replace("&Ntilde;", "Ñ")
                sHtml = sHtml.Replace("&iquest;", "¿").Replace("¡", "&iexcl;").Replace("'", "\'").Replace("""", "\""")
                sHtml = Regex.Replace(sHtml, "&(?ni:\#((x([\dA-F]){1,5})|(104857[0-5]|10485[0-6]\d|1048[0-4]\d\d|104[0-7]\d{3}|10[0-3]\d{4}|0?\d{1,6}))|([A-Za-z\d.]{2,31}));|<[^>]*>", "")

                'sHtml = sHtml.Replace("á", "&aacute;").Replace("é", "&eacute;").Replace("í", "&iacute;")
                'sHtml = sHtml.Replace("ó", "&oacute;").Replace("ú", "&uacute;").Replace("Á", "&Aacute;")
                'sHtml = sHtml.Replace("É", "&Eacute;").Replace("Í", "&Iacute;").Replace("Ó", "&Oacute;")
                'sHtml = sHtml.Replace("Ú", "&Uacute;").Replace("ñ", "&ntilde;").Replace("Ñ", "&Ntilde;")
                'sHtml = sHtml.Replace("¿", "&iquest;").Replace("¡", "&iexcl;").Replace("'", "\'").Replace("""", "\""")
            End If
            src = sHtml

        Catch ex As Exception
        End Try
        Return (src)

    End Function

    Private Sub ConfigEditor()
        Dim RutaString As String = ""
        Dim sLang As String = ""

        Try
            RutaString = AppSettings("FCKeditor:BasePath")
            If RutaString Is Nothing Then
                RutaString = GeRequestApplicationPath(String.Concat(PATHSCR, "/scripts/fckeditor/"))
            End If

            Select Case PortalCulture.GetIDCulture
                Case 1 : sLang = "es"
                Case 2 : sLang = "en"
            End Select

            '// Configura la ruta de js y el lenguaje.
            Me.FCKeditorEN.BasePath = RutaString
            Me.FCKeditorEN.AutoDetectLanguage = False
            Me.FCKeditorEN.DefaultLanguage = sLang
            Me.FCKeditorEN.Height = 240

            Me.FCKeditorES.BasePath = RutaString
            Me.FCKeditorES.AutoDetectLanguage = False
            Me.FCKeditorES.DefaultLanguage = sLang
            Me.FCKeditorES.Height = 240

        Catch ex As Exception
            RutaString = GeRequestApplicationPath(String.Concat(PATHSCR, "/scripts/fckeditor/"))
        End Try

        If RutaString Is Nothing Then
            RutaString = GeRequestApplicationPath(String.Concat(PATHSCR, "/scripts/fckeditor/"))
        End If

    End Sub

    Public Sub Limpia()
        Me.IdIndice = 0
        Me.txtenglish.Text = ""
        Me.txtspanish.Text = ""
        Me.FCKeditorEN.Value = ""
        Me.FCKeditorES.Value = ""
    End Sub

    Private Sub CargaDatos()
        Me.Limpia()
        Me.FCKeditorEN.Value = ""
        Me.FCKeditorES.Value = ""
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ConfigEditor()
        Me.divEditor.Style.Item("width") = "100%"
        Me.divEditor.Style.Item("height") = Me.Height

        '// Selecciona que div, sera mostrado como default.

        If AppSettings("DefaultLanguage") = "en-US" Then
            tblspanish.Style.Item("display") = "none"
            tblenglish.Style.Item("display") = ""
            DivUpload.Attributes.Add("class", "Tab")
            DivSelect.Attributes.Add("class", "TabSelected")

            'trlenglish.Style.Item("display") = ""
            'txtspanish.Style.Item("display") = "none"

        Else
            tblspanish.Style.Item("display") = ""
            tblenglish.Style.Item("display") = "none"

            DivUpload.Attributes.Add("class", "TabSelected")
            DivSelect.Attributes.Add("class", "Tab")

            'trlenglish.Style.Item("display") = "none"
            'txtspanish.Style.Item("display") = ""
        End If

        Me.DivSelect.Attributes("onClick") = " SelectTabFCk('" & DivSelect.ClientID & "','" _
            & DivUpload.ClientID & "','" & Me.tblenglish.ClientID & "','" _
            & Me.tblspanish.ClientID & "','" + trlenglish.ClientID + "','" + trlspanish.ClientID + "');"
        Me.DivUpload.Attributes("onClick") = " SelectTabFCk('" & DivUpload.ClientID & "','" _
            & DivSelect.ClientID & "','" & Me.tblspanish.ClientID & "','" _
            & Me.tblenglish.ClientID & "','" + trlspanish.ClientID + "','" + trlenglish.ClientID + "');"

        '// Evento Onclik comando Aceptar, f() javascript para asignar las descripciones.
        '//cmdAceptar.Attributes("onClick") = "GetInnerText('" + Me.FCKeditorEN.ClientID + "','"                                + Me.TextditorEN.ClientID + "');"                                 + "GetInnerText('" + Me.FCKeditorES.ClientID + "','"                                 + Me.TextditorES.ClientID + "');"
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Me.lblEs.Text = PortalCulture.GetString("M0BT0000080")
        Me.lblEn.Text = PortalCulture.GetString("M0BT0000081")
        MyBase.Render(writer)
    End Sub


End Class
