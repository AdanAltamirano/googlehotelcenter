Imports System.Configuration.ConfigurationManager
Imports Contenido.Comun
Imports Contenido.presentacion
Imports System.Runtime.Serialization

Partial Class ctrlImages1
    Inherits Opciones

    Private Const Dato1 As String = "PropEvent"
    Private Const Dato2 As String = "PropInfo2"
    Private Const Dato4 As String = "PropInfo3"

    Protected lnkAdd As AgregarContenido
    Private Const IMAGES As String = "imgHeader"
    Dim firstImage As String
    Dim defaultImage As String
    Dim selectedImage As Integer
    Dim index As Integer = 0
    Dim maximoGaleria As Integer

    Dim setallBand As Boolean = False
    Private idcont As Integer

    Protected WithEvents lblInfo2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo3 As System.Web.UI.WebControls.Label
    Protected WithEvents lnkPropiedad As CambiarContenido
    Protected WithEvents ctrlTitle As CambiarContenido
    Protected WithEvents ZoomImage As System.Web.UI.HtmlControls.HtmlImage
    Private idelem As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'Protected WithEvents imgPropiedad As System.Web.UI.WebControls.Image

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub cargatodo()
        'CargaContenido(IMAGES, imgPropiedad, Opciones.TipoControl.Imagen, lnkPropiedad, Nothing, Nothing, True, False, False, False, False)

        Me.ctrlTitle.NoPaso = "1."
        Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")    ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.lnkPropiedad.NoPaso = "1."
        Me.lnkPropiedad.Texto = PortalCulture.GetString("A00751")

        Me.lblTitle.Text = PortalCulture.GetString("A00748")

        'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)

        '	If MyBase.ModeView = Opciones.ViewMode.Edit Then
        'If Me.lblInfo1.Text = "" Then    '' If Me.lblInfo1.Text.Trim = "" Then
        '    Me.lblInfo1.Text = "[ " & PortalCulture.GetString("A00744") & " ]"
        'End If
        'Me.imgPropiedad.AlternateText = "[ " & PortalCulture.GetString("A00750") & " ]"

    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        '/**************************************************
        lnkPropiedad.IsImgHeader = True
        lnkPropiedad.Pagina = "Registro/Images.aspx"
        lnkPropiedad.Nota = "Edición de las imagenes"
        'lnkInfo1.Pagina = "Registro/InformacionPropiedad.aspx"
        'lnkInfo1.Nota = "Edición de la información de propiedad"
        defaultImage = ""
        Me.IsImgHeader = True
        If ModeView <> Opciones.ViewMode.gView Then
            cargatodo()
            loadImages()
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        cargatodo()
        loadImages()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        If HasContent And tblThumbnails.Rows.Count > 0 Then
            If firstImage = "" And defaultImage <> "" Then
                writer.Write(defaultImage)
            ElseIf firstImage <> "" Then
                writer.Write(firstImage)
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00749")
        lnkAdd.Texto = PortalCulture.GetString("A00751")
    End Sub


    Private Sub loadImages(Optional ByVal reload As Boolean = False)
        Dim rows() As DataRow
        Dim row As DataRow
        Dim idElemento As Integer
        Dim controw As Integer
        Dim contcel As Integer

        Dim countRow As Integer = 0
        If reload Then
            rows = GetContenidoUpdated(IMAGES)
        Else
            rows = Contenido(IMAGES)
        End If

        If rows Is Nothing Then Exit Sub
        Dim c As Control
        Dim tc As TableCell
        Dim tr As TableRow
        index = 0
        For Each tr In tblThumbnails.Rows
            controw += 1
            For Each tc In tr.Cells
                For Each c In tc.Controls
                    contcel += 1
                    Page.Controls.Remove(c)
                    c = c.FindControl("change_" & countRow & "_" & contcel)
                    If Not c Is Nothing Then
                        Page.Controls.Remove(c)
                    End If
                Next
            Next
        Next
        Me.tblThumbnails.Rows.Clear()
        index = 0
        controw = 0
        contcel = 0
        maximoGaleria = 0
        For Each row In rows
            If Not row.IsNull(ComunContenido.PKIDCONTENIDO_FIELD) Then
                maximoGaleria += 1

                If Me.tblThumbnails.Rows.Count = 0 Then
                    Me.tblThumbnails.Rows.Add(New TableRow)
                    'Me.tblThumbnails.Rows.Add(New TableRow)
                    controw = 1
                End If
                contcel += 1
                index += 1
                Dim imgThumbnail As New WebControls.Image
                Dim imgDescription As New WebControls.Label
                'Cargamos los links de las imagenes para eliminarlas o cambiarlas
                Dim lnkChange As New CambiarContenido
                'Dim lnkDelete As New EliminarContenido
                lnkChange = LoadControl(GeRequestApplicationPath("/portal/modules/contenido/CambiarContenido.ascx"))
                'lnkDelete = LoadControl( "/EliminarContenido.ascx")
                lnkChange.idContenido = row(ComunContenido.PKIDCONTENIDO_FIELD)
                idcont = row(ComunContenido.PKIDCONTENIDO_FIELD)
                lnkChange.EliminaContenido = True
                'Cargamos el contenido de la imagen
                CargaContenido(row, imgThumbnail, TipoControl.Imagen, lnkChange, Nothing, Nothing, True, False, False, False, False)
                lnkChange.RConte = False
                lnkChange.ID = "change_" & countRow & "_" & contcel
                imgThumbnail.Width = New Unit(70)
                imgThumbnail.Height = New Unit(70)
                imgThumbnail.ImageAlign = ImageAlign.AbsMiddle
                lnkChange.Pagina = "Registro/PhotoInformation.aspx"
                lnkChange.Nota = "Edición de las fotos del encabezado de la pagina de hotel."
                lnkChange.IsImgHeader = True
                'imgThumbnail.BorderWidth = New Unit(1)
                'imgThumbnail.BorderStyle = BorderStyle.Solid
                'imgThumbnail.BorderColor = System.Drawing.Color.Black
                Dim lnkImage As New WebControls.HyperLink
                Dim imgId As String
                'Dim findcad As String
                'Dim p As Integer
                'imgId = imgThumbnail.ImageUrl
                'p = imgId.LastIndexOf("/")
                'If p >= 0 Then
                'imgId = imgId.Substring(p + 1)
                'Else
                imgThumbnail.ImageUrl = imgThumbnail.ImageUrl.Replace(AppSettings("Img_Prefix_Module"), "")
                imgId = imgThumbnail.ImageUrl
                imgThumbnail.ImageUrl &= AppSettings("Img_Prefix_Thumbs")
                'imgId = imgId
                'End If
                'imgThumbnail.ImageUrl = imgThumbnail.ImageUrl.Replace(imgId, "")
                'imgThumbnail.ImageUrl &= AppSettings("Img_Prefix_Thumbs") & imgId
                'Dim imgId As String = imgThumbnail.ImageUrl.Replace(Albums.Comun.General.getImgModulePrefix, "")
                'imgThumbnail.ImageUrl = imgId & Albums.Comun.General.getImgThumbsPrefix
                'Llamamos a la funcion script para mostra las fotos en el zoom image
                If row.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                    lnkImage.NavigateUrl = "javascript:LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD) & """);"
                    defaultImage = "<script> LI('" & imgId & "','" & row(ComunContenido.DESCRIPCION_FIELD) & "'); </script>"
                    If selectedImage = 0 And index = 1 And firstImage = "" Then
                        'Obtenemos por default la primer imagen
                        firstImage = "<script> LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD) & """); </script>"
                    ElseIf firstImage = "" And selectedImage <> 0 And selectedImage = row(ComunContenido.PKIDCONTENIDO_FIELD) Then
                        firstImage = "<script> LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD) & """); </script>"
                    End If
                Else
                    lnkImage.NavigateUrl = "javascript:LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD).Replace(vbCrLf.ToString, "<BR>") & """);"
                    defaultImage = "<script> LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD).Replace(vbCrLf.ToString, "<BR>") & """); </script>"
                    If selectedImage = 0 And index = 1 And firstImage = "" Then
                        'Obtenemos por default la primer imagen
                        firstImage = "<script> LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD).Replace(vbCrLf.ToString, "<BR>") & """); </script>"
                    ElseIf firstImage = "" And selectedImage <> 0 And selectedImage = row(ComunContenido.PKIDCONTENIDO_FIELD) Then
                        firstImage = "<script> LI('" & imgId & "',""" & row(ComunContenido.DESCRIPCION_FIELD).Replace(vbCrLf.ToString, "<BR>") & """); </script>"
                    End If
                End If

                lnkImage.Controls.Add(imgThumbnail)

                Dim col As TableCell

                col = New TableCell
                col.Controls.Add(lnkChange)    ' Ponemos el link de modificar
                'col.Controls.Add(lnkDelete) ' ponemos el link de eliminar
                'Me.tblThumbnails.Rows(countRow).Cells.Add(col)
                'col = New TableCell
                'col.BorderStyle = BorderStyle.Solid
                col.BorderWidth = New Unit(0, UnitType.Pixel)
                col.VerticalAlign = VerticalAlign.Top
                col.Width = New Unit(70)
                col.Height = New Unit(70)

                'col.Controls.Add(New LiteralControl("<BR>"))

                col.Controls.Add(lnkImage)    ' Ponemos la imagen
                'Row                
                Me.tblThumbnails.Rows(countRow).Cells.Add(col)
            End If

            If Not row.IsNull(ComunElementos.PKIDElemento_FIELD) Then
                idElemento = row.Item(ComunElementos.PKIDElemento_FIELD)
                idelem = row.Item(ComunElementos.PKIDElemento_FIELD)
            End If

            'Solo 7 imagenes por row
            If index >= 6 Then
                index = 0
                countRow = countRow + 2
                Me.tblThumbnails.Rows.Add(New TableRow)
                Me.tblThumbnails.Rows.Add(New TableRow)
                controw += 1
            End If
        Next

        If idElemento > 0 Then
            lnkAdd.idElemento = idElemento
            lnkAdd.Rwidth = False
            lnkAdd.RHeight = False
            lnkAdd.RConte = False
            lnkAdd.RArch = True
            lnkAdd.ididioma = Me.IdIdioma
            lnkAdd.IdEmpresa = Me.IdEmpresa
            lnkAdd.IsImgHeader = True
            lnkAdd.ShowMode = AgregarContenido.ShowModeType.ViewImage

            AddHandler lnkAdd.ChangeContenido, AddressOf MyBase.SeleccionaContenido
        End If

        If Me.ModeView <> ViewMode.Edit Then
            lnkAdd.Visible = False
        End If

        validarMaximoGaleria()
    End Sub

    Private Sub validarMaximoGaleria()
        Dim imagenDisponible As Integer = 0

        If maximoGaleria >= AppSettings("maximoGaleria") Then
            lblMensajeImagen.Visible = True
            lblMensajeImagen.Text = PortalCulture.GetString("00792", True) & " " & AppSettings("maximoGaleria")
            lnkAdd.Visible = False
        Else
            lblMensajeImagen.Visible = False
            lnkAdd.Visible = True
            imagenDisponible = AppSettings("maximoGaleria") - maximoGaleria
        End If
    End Sub
End Class
