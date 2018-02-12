Imports XCrypt
Imports System.Text
Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Partial Class UniFlatConfiguration
    Inherits PaginaBase
    Protected WithEvents txtUrlWebSite As CtrlIdioma
    Protected WithEvents txtEmailComments As CtrlIdiomaRFCk
    Private Property iddiccWebsite()
        Get
            Return viewstate("iddiccWebsite")
        End Get
        Set(ByVal Value)
            viewstate("iddiccWebsite") = Value
        End Set
    End Property
    Private Property idDiccComentsEmails()
        Get
            Return viewstate("idDiccComentsEmails")
        End Get
        Set(ByVal Value)
            viewstate("idDiccComentsEmails") = Value
        End Set
    End Property
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents divSupervisor As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents divUsuarioHotel As System.Web.UI.HtmlControls.HtmlTable

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
        hplUniFlatEs.NavigateUrl = String.Format("javascript:openOnePageUI('{0}')", getCode(String.Format("{0}$ES", Me.cInfoActual.Hotel.ToString)))
        hplUniFlatEn.NavigateUrl = String.Format("javascript:openOnePageUI('{0}')", getCode(String.Format("{0}$EN", Me.cInfoActual.Hotel.ToString)))
        If Not IsPostBack Then
            getdata()
        End If
        Me.txtEmailComments.RequiredText = False
        Me.txtEmailComments.IsHTML = True
        txtEmailComments.Height = 100
        Me.txtUrlWebSite.RequiredText = False
        txtUrlWebSite.IsMultiline = False
    End Sub
    Private Function getCode(ByVal qr As String) As String
        Dim xe As XCryptEngine = New XCryptEngine
        xe.InitializeEngine(XCryptEngine.AlgorithmType.TripleDES)
        Dim decryptedQuery As String = qr
        Dim encryptedQuery As String = xe.Encrypt(decryptedQuery, "Crs-OnePage")
        Return ascii2hex(encryptedQuery)
    End Function


    Private Function ascii2hex(ByVal ascii As String) As String
        Dim hex As New StringBuilder
        Try
            For Each c As Char In ascii
                hex.Append(String.Format("{0:X2}", Asc(c)))
            Next
        Catch ex As Exception
        End Try
        Return hex.ToString
    End Function
    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblmailComments.Text = PortalCulture.GetString("00518")
        Me.lblUrlWebSite.Text = PortalCulture.GetString("00519")
        Me.btnSave.Text = PortalCulture.GetString("A00153")
        hplUniFlatEs.Text = PortalCulture.GetString("00521", True) & getCode(String.Format("{0}$ES", Me.cInfoActual.Hotel.ToString))
        hplUniFlatEn.Text = PortalCulture.GetString("00520", True) & getCode(String.Format("{0}$EN", Me.cInfoActual.Hotel.ToString))
        Me.lblUrlWebSite.Text = PortalCulture.GetString("00522", True)
        Me.lblmailComments.Text = PortalCulture.GetString("00523", True)
        Me.lbltitle.Text = PortalCulture.GetString("00524")
        If MyBase.IsSupervisor Or MyBase.isUserChain Then
            Me.hplUniFlatEn.Visible = True
            Me.hplUniFlatEs.Visible = True
            Me.txtUrlWebSite.deshabilita = True
        Else
            Me.hplUniFlatEn.Visible = False
            Me.hplUniFlatEs.Visible = False
            Me.txtUrlWebSite.deshabilita = False
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        update()
    End Sub
    Private Sub getdata()
        idDiccComentsEmails = 0
        iddiccWebsite = 0
        Dim ds As HotelDatos
        With New HotelSistema
            ds = .OnePageConfigurationGet(Me.cInfoActual.Hotel)
        End With
        If Not ds.Tables(HotelDatos.HOTEL_TABLE) Is Nothing AndAlso ds.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            With ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                If Not .IsNull(HotelDatos.FIELD_emailComments) Then Me.txtEmailComments.textodefault = .Item(HotelDatos.FIELD_emailComments)
                If Not .IsNull(HotelDatos.FIELD_urlWebSite) Then Me.txtUrlWebSite.textodefault = .Item(HotelDatos.FIELD_urlWebSite)
                If Not .IsNull(HotelDatos.FIELD_idDiccemailComments) Then
                    idDiccComentsEmails = .Item(HotelDatos.FIELD_idDiccemailComments)

                End If
                If Not .IsNull(HotelDatos.FIELD_idDiccUrlWebSite) Then
                    iddiccWebsite = .Item(HotelDatos.FIELD_idDiccUrlWebSite)

                End If
            End With
        End If
        Me.txtEmailComments.CargaDatos(idDiccComentsEmails)
        Me.txtUrlWebSite.CargaDatos(iddiccWebsite)
    End Sub

    Private Sub update()
        Dim ds As New HotelDatos
        Dim dr As DataRow
        dr = ds.Tables(HotelDatos.HOTEL_TABLE).NewRow
        dr.Item(HotelDatos.FIELD_urlWebSite) = txtUrlWebSite.textodefault
        dr.Item(HotelDatos.FIELD_emailComments) = txtEmailComments.textodefault
        dr.Item(HotelDatos.FIELD_PKID) = Me.cInfoActual.Hotel
        If Me.iddiccWebsite <> 0 Then
            Me.txtUrlWebSite.Update(Me.iddiccWebsite)
        ElseIf Me.txtUrlWebSite.GetES <> "" Or Me.txtUrlWebSite.GetEN <> "" Then
            iddiccWebsite = Me.txtUrlWebSite.Insert()
        End If
        If Me.idDiccComentsEmails <> 0 Then
            Me.txtEmailComments.Update(Me.idDiccComentsEmails)
        ElseIf Me.txtEmailComments.GetEN <> "" Or Me.txtEmailComments.GetES <> "" Then
            idDiccComentsEmails = Me.txtEmailComments.Insert()
        End If
        ds.Tables(HotelDatos.HOTEL_TABLE).Rows.Add(dr)
        ds.Tables(HotelDatos.HOTEL_TABLE).AcceptChanges()
        ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_PKID) = ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_PKID)
        If iddiccWebsite <> 0 Then
            ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_idDiccUrlWebSite) = iddiccWebsite
        End If
        If idDiccComentsEmails <> 0 Then
            ds.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_idDiccemailComments) = idDiccComentsEmails
        End If
        If iddiccWebsite <> 0 Or idDiccComentsEmails <> 0 Then
            With New HotelSistema
                .OnePageConfigurationUpdate(ds)
            End With
        End If
    End Sub

End Class
