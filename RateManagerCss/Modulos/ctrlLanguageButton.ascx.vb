Imports System.Configuration.ConfigurationManager

Partial Class ctrlLanguageButton
    Inherits System.Web.UI.UserControl

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

    Private ConnectionString As String = AppSettings("HotelConnectionString")
    Protected CtrlLanguageDictionary1 As ctrlLanguageDictionary

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            Me.btnHide.Attributes.Add("onclick", "javascript:hideResource('" & Me.divResourceDescription.ClientID & "','" & Me.txtStatus.ClientID & "');")
            Me.btnShow.Attributes.Add("onclick", "javascript:showResource('" & Me.divResourceDescription.ClientID & "','" & Me.txtStatus.ClientID & "');")

            Me.divResourceDescription.Style.Add("display", "none")
            CtrlLanguageDictionary1.m_visibleTitle = False
        Else
            Dim status As Int16 = Int16.Parse(Me.txtStatus.Text.Trim)
            If status = 1 Then
                Me.divResourceDescription.Style.Item("display") = "block"
                Page.RegisterStartupScript("ClientSideCode", "<script>try { showResource('" & Me.divResourceDescription.ClientID & "','" & Me.txtStatus.ClientID & "');} catch (ex) { }</script>")
            Else
                Me.divResourceDescription.Style.Item("display") = "none"
                Page.RegisterStartupScript("ClientSideCode", "<script>try { hideResource('" & Me.divResourceDescription.ClientID & "','" & Me.txtStatus.ClientID & "');} catch (ex) { }</script>")
            End If
        End If

    End Sub

    Public Sub loadDictionaryById(ByVal idDicctionary As Integer)
        Viewstate("TempId") = CStr(idDicctionary)
        CtrlLanguageDictionary1.LoadDictionary(idDicctionary, Me.ConnectionString)
    End Sub
    Public Sub loadDictionaryById(ByVal idDicctionary As Integer, ByVal connectionString As String)
        Viewstate("TempId") = CStr(idDicctionary)
        CtrlLanguageDictionary1.LoadDictionary(idDicctionary, connectionString)
    End Sub

    Private Sub CargaRecursos()
        If Not IsNothing(Viewstate("TempId")) Then
            loadDictionaryById(CInt(Viewstate("TempId")))
        End If
        lblResources.Text = PortalCulture.GetString("00098", False)
        btnShow.Value = PortalCulture.GetString("00099", False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CargaRecursos()
    End Sub

End Class
