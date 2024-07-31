Imports Contenido.Comun
Imports Contenido.presentacion
Imports RateManager.Utitlities.Contenido
Imports System.Configuration.ConfigurationManager

Public Class ctrlImagesHotelItem
    Inherits Opciones

    Private Const IMAGES As String = "Thumbs"
    Private idcont As Integer
    Dim index As Integer = 0
    Dim maximoGaleria As Integer
    Dim firstImage As String
    Dim defaultImage As String
    Dim selectedImage As Integer
    Private idelem As Integer

    Public Property IdHotelItem As String
        Get
            Return ViewState("IdHotelItemCtrlImages")
        End Get
        Set(ByVal value As String)
            ViewState("IdHotelItemCtrlImages") = value
        End Set
    End Property

    Private Property IdElementModule() As Integer
        Get
            Return ViewState("IdElementImagesHotelItem")
        End Get
        Set(value As Integer)
            ViewState("IdElementImagesHotelItem") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            MyBase.IdModulo = CType(AppSettings("idCtrlImagesHotelItem"), Integer)
            Me.IdElementModule = ContenidoUtilitie.GetIdElementByModule(MyBase.IdModulo)
            Me.lblTitle.Text = PortalCulture.GetString("01671") 'Galeria
            Me.lblfiles.InnerText = PortalCulture.GetString("01672") 'Agregar Imagenes
        End If

        If Me.ModeView <> Opciones.ViewMode.gView Then
        End If

        If IsPostBack Then
            LoadImagesByHotelItem(IdHotelItem)
        End If


    End Sub

    Public Sub SaveImages()
        With New Albums.Facade.BusinessFacade
            For i As Integer = 1 To 10
                Dim htmlInputFile As HtmlInputFile = FindControl("attach" & i.ToString)
                If Not htmlInputFile Is Nothing AndAlso Not htmlInputFile.PostedFile Is Nothing Then
                    If .VerifyValidImg(htmlInputFile, Request) Then
                        Try
                            Dim addedImageDirection As String = ""
                            If .CreateImg(htmlInputFile, AppSettings("Albums_Dir").ToString.ToUpper.Replace("ALBUMS", ""), IdRubro, IdEmpresa, addedImageDirection, False, pathToAddToBasePath:="Item\", customId:=IdHotelItem) Then
                                Dim contenido As ComunContenido
                                Dim dataRow As DataRow
                                With New presentacionContenido
                                    contenido = .GetContenidoById(idcont, Me.IdIdioma)
                                    If .CreateContenido(CType(Me.IdElementModule, Integer), CType(Me.Page, PaginaBase).cInfoActual.Empresa, Me.IdIdioma, Val(New Unit(70).Value), Val(New Unit(70).Value), "", "", addedImageDirection, contenido) Then
                                        dataRow = contenido.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
                                        contenido.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dataRow)
                                        dataRow.Item(ComunContenido.PKIDCONTENIDO_FIELD) = contenido.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                                        dataRow.Item(ComunContenido.ARCHIVO_FIELD) = contenido.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                                        dataRow.Item(ComunContenido.URL_FIELD) = contenido.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                                        With New presentacionContenido
                                            If Me.IdIdioma = 1 Then
                                                dataRow.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                                                dataRow.Item(ComunContenido.DESCRIPCION_FIELD) = ""
                                            Else
                                                dataRow.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                                dataRow.Item(ComunContenido.DESCRIPCION_FIELD) = ""
                                            End If
                                            .UpdateContenido(contenido)
                                        End With

                                        'Agregar relacion entre idConteido y idTipoHabitacionHotel

                                        Dim idContenido As Integer = CType(contenido.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD), Integer)
                                        Dim idHotelItem As Integer = CType(Me.IdHotelItem, Integer)

                                        .CreateContenidoHotelItem(idContenido, idHotelItem)
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
    End Sub

    Public Sub LoadImagesByHotelItem(ByVal idHotelItem As Integer)

        Me.tblThumbnails.Rows.Clear()

        Dim rows() As DataRow
        Dim row As DataRow
        Dim idElemento As Integer
        Dim controw As Integer
        Dim contcel As Integer

        Dim countRow As Integer = 0

        rows = GetContenidoImagesHotelItem(IMAGES, idHotelItem)

        If rows Is Nothing Then Exit Sub

        Dim control As Control
        Dim tableCell As TableCell
        Dim tableRow As TableRow

        Index = 0

        For Each tableRow In tblThumbnails.Rows
            controw += 1
            For Each tableCell In tableRow.Cells
                For Each control In tableCell.Controls
                    contcel += 1
                    Page.Controls.Remove(control)
                    control = control.FindControl("change_" & countRow & "_" & contcel)
                    If Not control Is Nothing Then
                        Page.Controls.Remove(control)
                    End If
                Next
            Next
        Next

        Index = 0
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
                lnkChange.EditButton.Visible = False
                lnkChange.EliminaContenido = False
                lnkChange.DeleteButtonIdContent.Visible = True 'AGregar Aqui
                lnkChange.idContenido = row(ComunContenido.PKIDCONTENIDO_FIELD)
                lnkChange.ContentType = "HotelItem"
                lnkChange.IdTypeHotelItemChangeContent = idHotelItem
                'lnkChange.IdTypeRoomHotelChangeContent = idTypeRoomHotel ' Cambiar aqui
                'lnkChange.CodeTypeRoomHotelChangeContent = codeTypeRoomHotel ' Cambiar aqui
                idcont = row(ComunContenido.PKIDCONTENIDO_FIELD)
                'Cargamos el contenido de la imagen
                CargaContenido(row, imgThumbnail, TipoControl.Imagen, lnkChange, Nothing, Nothing, True, False, True, False, False)
                lnkChange.RConte = True
                lnkChange.ID = "change_" & countRow & "_" & contcel
                imgThumbnail.Width = New Unit(70)
                imgThumbnail.Height = New Unit(70)
                imgThumbnail.ImageAlign = ImageAlign.AbsMiddle
                lnkChange.Pagina = "ctrlImagesHotelItem.ascx"
                lnkChange.Nota = "Imagenes del hotel item"

                Dim lnkImage As New WebControls.HyperLink
                Dim imgId As String

                imgThumbnail.ImageUrl = imgThumbnail.ImageUrl.Replace(AppSettings("Img_Prefix_Module"), "")
                imgId = imgThumbnail.ImageUrl
                imgThumbnail.ImageUrl &= AppSettings("Img_Prefix_Thumbs")

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
                col.Controls.Add(lnkChange)
                col.BorderWidth = New Unit(0, UnitType.Pixel)
                col.VerticalAlign = VerticalAlign.Top
                col.Width = New Unit(70)
                col.Height = New Unit(70)

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

    End Sub

End Class