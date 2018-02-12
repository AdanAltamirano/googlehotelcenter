Imports System.Configuration.ConfigurationManager
Imports Contenido.Comun
Imports Contenido.presentacion
Imports System.Runtime.Serialization
Partial Class Photo
	Inherits Opciones

#Region " Código generado por el Diseñador de Web Forms "

	'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents TablaImagenes As System.Web.UI.WebControls.Table
    Protected WithEvents ZoomImage As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents btndelete As System.Web.UI.WebControls.Button
    Protected WithEvents myDiv As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents file1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file2 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file3 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file4 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file5 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file6 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file7 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file8 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents file9 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents AmoreFiles As System.Web.UI.HtmlControls.HtmlAnchor

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

	Private Const IMAGES As String = "Thumbs"
	Private Const TITLE As String = "Title"

	Protected WithEvents lnkChangeTitle As CambiarContenido
	Protected lnkAdd As AgregarContenido

	Dim firstImage As String
	Dim defaultImage As String
	Dim selectedImage As Integer
    Dim index As Integer = 0
    Dim maximoGaleria As Integer

    Dim setallBand As Boolean = False
    Private idcont As Integer
    Private idelem As Integer
    ''''''''------------------------- 
   


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página      
       

        If Me.ModeView <> Opciones.ViewMode.gView Then


            MyBase.IdModulo = CType(AppSettings("idCtrlPhotos"), Integer)

            'CargaContenido(Me.TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.lnkChangeTitle, Nothing, Nothing, False, False, True, False, False)
            'Forzamos a que sea el mismo modulo para obtener el mismo contenido entre fotos
            'y thumbs
            defaultImage = ""

            selectedImage = 0

            Dim queryString As String = Request.QueryString("imId")
            If IsNumeric(queryString) Then
                selectedImage = CType(queryString, Integer)
            End If

            loadImages()


            Me.lblMoreImages.Text = PortalCulture.GetString("A00711")

            If Me.ModeView <> ViewMode.Edit Then
                lnkAdd.Visible = False
            Else
                If Me.tblThumbnails.Rows.Count = 0 Then
                    Me.lblDescTips.Text = "[ " & PortalCulture.GetString("A00712") & " ]"
                    Me.lblImagesTips.Text = "[ " & PortalCulture.GetString("A00713") & " ]"
                    Me.lblDescTips.Visible = True
                    Me.lblImagesTips.Visible = True
                End If
            End If


        End If

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
                lnkChange.idContenido = row(ComunContenido.PKIDCONTENIDO_FIELD)
                idcont = row(ComunContenido.PKIDCONTENIDO_FIELD)
                lnkChange.EliminaContenido = True
                'Cargamos el contenido de la imagen
                CargaContenido(row, imgThumbnail, TipoControl.Imagen, lnkChange, Nothing, Nothing, True, False, True, False, False)
                lnkChange.RConte = True
                lnkChange.ID = "change_" & countRow & "_" & contcel
                imgThumbnail.Width = New Unit(70)
                imgThumbnail.Height = New Unit(70)
                imgThumbnail.ImageAlign = ImageAlign.AbsMiddle
                lnkChange.Pagina = "Registro/PhotoInformation.aspx"
                lnkChange.Nota = "Edición de las fotos del hotel."
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
            lnkAdd.RConte = True
            lnkAdd.RArch = True
            lnkAdd.ididioma = Me.IdIdioma
            lnkAdd.IdEmpresa = Me.IdEmpresa
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
            btnAttach.Enabled = False
            desHabilitarAttach(True, True, True, True, True, True, True, True, True, True)
        Else
            lblMensajeImagen.Visible = False
            lnkAdd.Visible = True
            btnAttach.Enabled = True
            imagenDisponible = AppSettings("maximoGaleria") - maximoGaleria

            If imagenDisponible >= 10 Then
                desHabilitarAttach(False, False, False, False, False, False, False, False, False, False)
            Else
                Select Case imagenDisponible
                    Case 1
                        desHabilitarAttach(False, True, True, True, True, True, True, True, True, True)

                    Case 2
                        desHabilitarAttach(False, False, True, True, True, True, True, True, True, True)

                    Case 3
                        desHabilitarAttach(False, False, False, True, True, True, True, True, True, True)

                    Case 4
                        desHabilitarAttach(False, False, False, False, True, True, True, True, True, True)

                    Case 5
                        desHabilitarAttach(False, False, False, False, False, True, True, True, True, True)

                    Case 6
                        desHabilitarAttach(False, False, False, False, False, False, True, True, True, True)

                    Case 7
                        desHabilitarAttach(False, False, False, False, False, False, False, True, True, True)

                    Case 8
                        desHabilitarAttach(False, False, False, False, False, False, False, False, True, True)

                    Case 9
                        desHabilitarAttach(False, False, False, False, False, False, False, False, False, True)
                End Select
            End If
        End If
    End Sub

    Private Sub desHabilitarAttach(ByVal valorattach1 As Boolean, ByVal valorattach2 As Boolean, ByVal valorattach3 As Boolean, ByVal valorattach4 As Boolean, ByVal valorattach5 As Boolean, ByVal valorattach6 As Boolean, ByVal valorattach7 As Boolean, ByVal valorattach8 As Boolean, ByVal valorattach9 As Boolean, ByVal valorattach10 As Boolean)
        attach1.Disabled = valorattach1
        attach2.Disabled = valorattach2
        attach3.Disabled = valorattach3
        attach4.Disabled = valorattach4
        attach5.Disabled = valorattach5
        attach6.Disabled = valorattach6
        attach7.Disabled = valorattach7
        attach8.Disabled = valorattach8
        attach9.Disabled = valorattach9
        attach10.Disabled = valorattach10
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        If HasContent Then
            If firstImage = "" And defaultImage <> "" Then
                writer.Write(defaultImage)
            ElseIf firstImage <> "" Then
                writer.Write(firstImage)
            End If
        End If
    End Sub

    Private Sub Page_SetAll(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SetAll
        '        loadImages()
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        'CargaContenido(Me.TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.lnkChangeTitle, Nothing, Nothing, False, False, True, False, False)

        loadImages()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00714")
        Me.lnkChangeTitle.NoPaso = "1."
        Me.lnkChangeTitle.Texto = PortalCulture.GetString("A00674")    ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.lnkAdd.NoPaso = ""
        Me.lnkAdd.Texto = PortalCulture.GetString("A00677")    '"Presione el Icono del lapiz para cambiar el El texto"
        btnAttach.Text = PortalCulture.GetString("A00153")
        Me.lblfiles.InnerText = PortalCulture.GetString("00588")

    End Sub

    Private Sub btnAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttach.Click
        'If Me.lstfiles.Items.Count < 3 Then
        '    Dim fileName As String = FileUpload.PostedFile.FileName
        '    If fileName <> "" Then
        '        If ViewState("items") Is Nothing Then
        '            ViewState("items") = 0
        '        End If
        '        lstfiles.Items.Add(FileUpload.PostedFile.FileName)
        '        Session("myupload" + ViewState("items").ToString) = FileUpload
        '        ViewState("items") += 1
        '    End If
        'Else
        '    lblatacherror.Visible = True
        'End If
        With New Albums.Facade.BusinessFacade
            For i As Integer = 1 To 10

                Dim hif As HtmlInputFile = FindControl("attach" & i.ToString)
                If Not hif Is Nothing AndAlso Not hif.PostedFile Is Nothing Then
                    If .VerifyValidImg(hif, Request) Then
                        Try
                            'AddedImage SE MANDA PARA OBTENER LA DIRECCION POR REFERENCIA 
                            Dim AddedImage As String = ""
                            If .CreateImg(hif, AppSettings("Albums_Dir").ToString.ToUpper.Replace("ALBUMS", ""), IdRubro, IdEmpresa, AddedImage) Then
                                'CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Crear, "Se agregó la imagen " & AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") & AddedImage & ". ")

                                Dim conte As ComunContenido
                                Dim dr2 As DataRow
                                With New presentacionContenido
                                    conte = .GetContenidoById(idcont, Me.IdIdioma)
                                    If .CreateContenido(idelem, CType(Me.Page, PaginaBase).cInfoActual.Empresa, Me.IdIdioma, Val(New Unit(70).Value), Val(New Unit(70).Value), "", "", AddedImage, conte) Then
                                        dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
                                        conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr2)
                                        dr2.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                                        dr2.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                                        dr2.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                                        With New presentacionContenido
                                            If Me.IdIdioma = 1 Then
                                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = ""
                                            Else
                                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = ""
                                            End If
                                            .UpdateContenido(conte)
                                        End With
                                    End If
                                End With
                            End If
                        Catch ex As UnauthorizedAccessException
                            Response.Write("<b>No se han especificado los derechos de subdirectorios!!</b>" + ex.Message)

                        Catch ex As OutOfMemoryException
                            Response.Write("<b>No se puede cargar la imagen.</b>" + ex.Message)
                        Catch ex As Exception


                            Response.Write("<b>Hay un problema con los directorios!!</b>" + ex.Message)
                        End Try
                    End If
                End If
            Next
        End With
        loadImages(True)

    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click
        'If lstfiles.SelectedIndex > -1 Then
        '    Dim uploadedFileIndex As Integer = lstfiles.SelectedIndex
        '    Session.Remove("myupload" + uploadedFileIndex.ToString)
        '    lstfiles.Items.Remove(lstfiles.SelectedValue)
        'End If
        'If lstfiles.Items.Count <= 3 Then
        '    lblatacherror.Visible = False
        'End If
    End Sub

    Public Sub Upload_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '  addimage()
        'Dim f As HtmlInputFile
        FindControl("my1input")


        loadImages(True)

        'For i As Integer = 0 To ViewState("items") - 1
        '    Dim hif As HtmlInputFile = CType(Session("myupload" + i.ToString), HtmlInputFile)
        '    Dim storePath As String = Server.MapPath("~") + "/MultipleUpload"

        '    If Not System.IO.Directory.Exists(storePath) Then
        '        System.IO.Directory.CreateDirectory(storePath)
        '    End If
        '    hif.PostedFile.SaveAs(storePath + "/" + System.IO.Path.GetFileName(hif.PostedFile.FileName))
        '    ListBox1.Items.Clear()
        '    Session("myupload" + i.ToString) = Nothing
        'Next
    End Sub



    'Private Sub addimage()
    '    'Çreando las reglas que validarán
    '    With New Albums.Facade.BusinessFacade
    '        For i As Integer = 0 To ViewState("items") - 1
    '            Dim hif As HtmlInputFile = CType(Session("myupload" + i.ToString), HtmlInputFile)
    '            If Not hif Is Nothing Then
    '                If .VerifyValidImg(hif, Request) Then
    '                    Try
    '                        'AddedImage SE MANDA PARA OBTENER LA DIRECCION POR REFERENCIA 
    '                        Dim AddedImage As String = ""
    '                        If .CreateImg(hif, AppSettings("Albums_Dir").ToString.ToUpper.Replace("ALBUMS", ""), IdRubro, IdEmpresa, AddedImage) Then
    '                            'CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Crear, "Se agregó la imagen " & AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") & AddedImage & ". ")

    '                            Dim conte As ComunContenido
    '                            Dim dr2 As DataRow
    '                            With New presentacionContenido
    '                                conte = .GetContenidoById(idcont, Me.IdIdioma)
    '                                If .CreateContenido(idelem, CType(Me.Page, PaginaBase).cInfoActual.Empresa, Me.IdIdioma, Val(New Unit(70).Value), Val(New Unit(70).Value), "", "", AddedImage, conte) Then
    '                                    dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
    '                                    conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr2)
    '                                    dr2.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
    '                                    dr2.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
    '                                    dr2.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
    '                                    With New presentacionContenido
    '                                        If Me.IdIdioma = 1 Then
    '                                            dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
    '                                            dr2.Item(ComunContenido.DESCRIPCION_FIELD) = ""
    '                                        Else
    '                                            dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
    '                                            dr2.Item(ComunContenido.DESCRIPCION_FIELD) = ""
    '                                        End If
    '                                        .UpdateContenido(conte)
    '                                    End With
    '                                End If
    '                            End With
    '                        End If
    '                    Catch ex As UnauthorizedAccessException
    '                        Response.Write("<b>No se han especificado los derechos de subdirectorios!!</b>")
    '                    Catch ex As OutOfMemoryException
    '                        Response.Write("<b>No se puede cargar la imagen.</b>")
    '                    Catch ex As Exception

    '                        Response.Write("<b>Hay un problema con los directorios!!</b>")
    '                    End Try
    '                End If
    '            End If
    '            Session("myupload" + i.ToString) = Nothing
    '        Next
    '    End With
    '    ViewState("items") = Nothing
    '    lstfiles.Items.Clear()
    'End Sub

End Class


