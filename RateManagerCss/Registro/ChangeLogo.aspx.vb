Imports System.Drawing
Imports System.Configuration.ConfigurationManager

Partial Class ChangeLogo
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
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(pages.Home)

        img.ImageUrl = AppSettings("VirtualDirectoryLogos") & "LogoCompany_" & Me.cInfoActual.Empresa & "?" & Now.ToString
        'btnfile.Attributes.Add("onclick", "javascript:saveImg('" & Me.lblfile.ClientID & "')")
        If Not IsPostBack Then
            lblError.Visible = False
            lblNoSave.Visible = False
        End If
    End Sub

    Private Sub cmdCambiar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCambiar.Click
        lblError.Visible = False
        lblNoSave.Visible = False
        If ImgFileOpen.Value <> "" Then
            If IsValidIMG(ImgFileOpen) Then
                If saveImage(ImgFileOpen) Then
                    Me.guardalog("Registro/ChangeLogo.aspx", PaginaBase.acciones.Modificar, "Se cambió el logo del hotel " & Me.cInfoActual.HotelName)
                Else
                    lblNoSave.Visible = True
                End If
            Else
                lblError.Visible = True
            End If
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbllogo.Text = PortalCulture.GetString("A00736", True)
        Me.lbltitle.Text = PortalCulture.GetString("A00737")
        Me.btnCancelar.Text = PortalCulture.GetString("A00143")
        Me.cmdCambiar.Text = PortalCulture.GetString("A00139")
        'btnfile.Value = PortalCulture.GetString("00472")
        lblError.Text = PortalCulture.GetString("00474")
    End Sub


    Public Function saveImage(ByRef lblfileName As HtmlInputFile) As Boolean
        'Dim oImg As System.Drawing.Image
        'If Not Directory.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/") Then
        '    Directory.CreateDirectory(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/")
        'End If
        Dim BitImage As New Bitmap(lblfileName.PostedFile.InputStream)
        Dim Bit As Bitmap = Nothing

        ' Dim newSize As Size
        Try
            BitImage.Save((AppSettings("MapPathPortal") & AppSettings("ImgLogoCompany") & "LogoCompany_" & Me.cInfoActual.Empresa).Replace("//", "/"))
            Return True
        Catch ex As Exception
            Return False
        Finally
            If Not Bit Is Nothing Then
                Bit.Dispose()
            End If
        End Try
    End Function


    Public Function IsValidIMG(ByRef file As System.Web.UI.HtmlControls.HtmlInputFile) As Boolean
        Dim oImg As System.Drawing.Image = Nothing
        If file.PostedFile.ContentLength > 0 Then
            Try
                'Tratamos de crear una imagen en memoria para comprobar el formato del unpload
                oImg = System.Drawing.Image.FromStream(file.PostedFile.InputStream)
                IsValidIMG = True
            Catch ex As Exception
                IsValidIMG = False
            Finally
                If Not oImg Is Nothing Then
                    oImg.Dispose()
                End If
            End Try
        End If
    End Function

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        lblError.Visible = False
        lblNoSave.Visible = False
    End Sub
End Class
