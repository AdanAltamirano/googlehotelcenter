Imports System.IO
Imports Contenido.Comun
Imports Contenido.presentacion
Imports System.Configuration.ConfigurationManager

Partial Class InfoComidas
    Inherits opciones
    Private Const Dato1 As String = "ListadoRestaurants"
	Private Const Dato2 As String = "imgRestaurants"

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Para el contenido
    Protected WithEvents lnkListadoRestaurants As CambiarContenido
    'Protected WithEvents lnkRestaurants As CambiarContenido
    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Protected WithEvents ctrlTitle As CambiarContenido
    Protected lnkAdd As AgregarContenido
    Private Const IMAGES As String = "Thumbs"
    Private Const TITLE As String = "Title"

    Dim firstImage As String
    Dim defaultImage As String
    Dim selectedImage As Integer
    Dim index As Integer = 0
    Dim maximoGaleria As Integer
    Private idcont As Integer
    Private idelem As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        ctrlTitle.Pagina = "Registro/FoodInformation.aspx"
        ctrlTitle.Nota = "Se modificó la información de restaurantes"
        'lnkRestaurants.Pagina = "Registro/FoodInformation.aspx"
        'lnkRestaurants.Nota = "Se modificó la imagen a mostrar de la información de restaurants."
        lnkListadoRestaurants.Pagina = "Registro/FoodInformation.aspx"
        lnkListadoRestaurants.Nota = "Edición de la información de restaurants"
		If Me.ModeView <> Opciones.ViewMode.gView Then
			Me.lPanel.Visible = True
			Me.gPanel.Visible = False
			Cargatodo()
		Else
			Me.lPanel.Visible = False
			Me.gPanel.Visible = True
		End If
	End Sub
	Private Sub Cargatodo()
		Dim strPath As String

		'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)
		CargaContenido(Dato1, lblListadoRestaurants, Opciones.TipoControl.Label, lnkListadoRestaurants, Nothing, Nothing, False, False, True, False, False)
        'CargaContenido(Dato2, imgRestaurants, Opciones.TipoControl.Imagen, lnkRestaurants, Nothing, Nothing, True, False, True, False, False)
        loadImages()


        Me.ctrlTitle.NoPaso = "1."
        Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")    ' "Presione el Icono del lapiz para cambiar el Titulo"

        'Me.lnkRestaurants.NoPaso = "1."
        'Me.lnkRestaurants.Texto = PortalCulture.GetString("A00674")
        Me.lnkAdd.Texto = PortalCulture.GetString("A00674")
        Me.lnkAdd.NoPaso = "2."

        Me.lnkListadoRestaurants.NoPaso = "1."
        Me.lnkListadoRestaurants.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"



		If MyBase.ModeView = Opciones.ViewMode.Edit Then
			If Me.lblListadoRestaurants.Text.Trim = "" Then
                Me.lblListadoRestaurants.Text = "[" & PortalCulture.GetString("A00701") & " ]"
			End If
            'Me.imgRestaurants.AlternateText = "[ " & PortalCulture.GetString("A00702") & " ]"

            'If imgRestaurants.ImageUrl = vbNullString Then
            '    imgRestaurants.ImageUrl = GeRequestApplicationPath("/Images/NotAvailable.jpg")
            'End If
           
            If lblTitle.Text.Trim = "" Then
                lblTitle.Text = "[ " & PortalCulture.GetString("A00690") & " ]"
            End If
        End If
        'Util.Images.loadImage(Me.imgRestaurants.ImageUrl, Me.imgRestaurants, MyBase.ModeView)
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
        Index = 0

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
                lnkChange.idContenido = row(ComunContenido.PKIDCONTENIDO_FIELD)
                idcont = row(ComunContenido.PKIDCONTENIDO_FIELD)
                lnkChange.EliminaContenido = True
                'Cargamos el contenido de la imagen
                lnkChange.ShowMode = CambiarContenido.ShowModeType.ViewImage
                lnkChange.EditMode = CambiarContenido.ShowModeType.ViewImage
                CargaContenido(row, imgThumbnail, TipoControl.Imagen, lnkChange, Nothing, Nothing, True, False, False, False, False)
                lnkChange.RConte = False
                'lnkChange.IsMultilanguage = True
                lnkChange.ID = "change_" & countRow & "_" & contcel
                imgThumbnail.Width = New Unit(70)
                imgThumbnail.Height = New Unit(70)
                imgThumbnail.ImageAlign = ImageAlign.AbsMiddle
                lnkChange.Pagina = "Registro/FoodInformation.aspx"
                lnkChange.Nota = "Edición de las fotos del restaurant."
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
                imgThumbnail.ImageUrl = imgThumbnail.ImageUrl '.Replace(AppSettings("Img_Prefix_Module"), "")
                imgId = imgThumbnail.ImageUrl
                'imgThumbnail.ImageUrl &= AppSettings("Img_Prefix_Thumbs")
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
            'lnkAdd.IsMultilanguage = True
            lnkAdd.RArch = True
            lnkAdd.ididioma = Me.IdIdioma
            lnkAdd.IdEmpresa = Me.IdEmpresa
            AddHandler lnkAdd.ChangeContenido, AddressOf MyBase.SeleccionaContenido
        End If

        If Me.ModeView <> ViewMode.Edit Then
            lnkAdd.Visible = False
        End If

        'validarMaximoGaleria()
    End Sub


	Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
		Cargatodo()
	End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00703")
    End Sub
End Class
