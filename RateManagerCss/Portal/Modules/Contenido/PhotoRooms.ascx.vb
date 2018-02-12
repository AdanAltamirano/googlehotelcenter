Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Configuration.ConfigurationManager
Imports System.Drawing.Imaging
Imports Contenido.Comun
Imports Contenido.presentacion
Imports System.Drawing
Imports System.IO
Imports System.Runtime.Serialization
Partial Class PhotoRooms
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


    Dim firstImage As String
    Dim defaultImage As String
    Dim selectedImage As Integer
    Dim index As Integer = 0
    Dim maximoGaleria As Integer

    Dim setallBand As Boolean = False
    Private idcont As Integer
    Private idelem As Integer
    ''''''''------------------------- 

    Public Property idHotel() As Integer
        Get
            Return viewstate("idHotel")
        End Get
        Set(ByVal Value As Integer)
            viewstate("idHotel") = Value
        End Set
    End Property
    Private Property idTipoHabitacion() As Integer
        Get
            Return viewstate("idTipoHabitacion")
        End Get
        Set(ByVal Value As Integer)
            viewstate("idTipoHabitacion") = Value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página      
        If Not Page.IsPostBack Then
            Me.idTipoHabitacion = 0
            loadRoomsTypes()
            'LoadImagesRooms()
        End If

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
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        btnAttach.Text = PortalCulture.GetString("A00153")
        ButtonMostrar.Text = PortalCulture.GetString("M0BT0000294")
        Me.lblfiles.InnerText = PortalCulture.GetString("00588")
        lblTypeRoom.Text = PortalCulture.GetString("00072", True)
    End Sub

    Private Sub btnAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttach.Click
        If Me.idTipoHabitacion > 0 Then
            With New Albums.Facade.BusinessFacade
                For i As Integer = 1 To 10
                    Dim hif As HtmlInputFile = FindControl("attach" & i.ToString)
                    If Not hif Is Nothing AndAlso Not hif.PostedFile Is Nothing Then
                        If .VerifyValidImg(hif, Request) Then
                            Try
                                Dim Nombre As String = (New RoomFacade).AddExtraImageRoom(Me.IdEmpresa, Me.idTipoHabitacion)
                                If Nombre <> "" Then
                                    saveImage(Nombre, hif)
                                End If
                            Catch ex As UnauthorizedAccessException
                                Response.Write("<b>No se han especificado los derechos de subdirectorios!!</b>")
                            Catch ex As OutOfMemoryException
                                Response.Write("<b>No se puede cargar la imagen.</b>")
                            Catch ex As Exception
                                Response.Write("<b>Hay un problema con los directorios!!</b>")
                            End Try
                        End If
                    End If
                Next
            End With
        End If
        LoadImagesRooms()
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
        'Dim f As HtmlInputFile
        FindControl("my1input")
    End Sub

    Private Sub loadRoomsTypes()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        With New Portal.Hotel.Facade.RoomFacade
            'Rooms = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
            Rooms = .getRooms(idHotel, PortalCulture.GetIDCulture)
        End With
        Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        lstRoomType.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
        lstRoomType.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        lstRoomType.DataSource = Rooms
        lstRoomType.DataBind()
        lstRoomType.Items.Insert(0, PortalCulture.GetString("M000272"))
        lstRoomType.Items(0).Value = 0

        'Dim room As RoomsTypeData
        'Me.lstRoomType.Items.Clear()
        'With New RoomTypeFacade
        '    room = .getRoomsTypes(PortalCulture.GetIDCulture)
        '    Me.lstRoomType.DataSource = room.Tables(RoomsTypeData.TBL_ROOMTYPE)
        '    Me.lstRoomType.DataTextField = RoomsTypeData.FLD_NAME
        '    Me.lstRoomType.DataValueField = RoomsTypeData.FLD_ID_ROOM_TYPE
        '    Me.lstRoomType.DataBind()
        'End With
    End Sub
    Public Function saveImage(ByVal NombreImagen As String, ByRef lblfileName As HtmlInputFile) As Boolean
        '**********************************************************************************************
        'GUARDA LA IMAGEN DEL TAMAÑO DEL MODULO  
        'AppSettings("DIR_TIPO_HAB") + IdCompamy + Tipohabitacion

        ' Dim oImg As System.Drawing.Image

        If Not Directory.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & IdEmpresa & "/") Then
            Directory.CreateDirectory(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & IdEmpresa & "/")

        End If

        Dim BitImage As New Bitmap(lblfileName.PostedFile.InputStream)
        Dim Bit As Bitmap = Nothing

        Dim newSize As Size
        Try
            'ORIGINAL - SALVANDO IMAGEN  (IdRoom)
            BitImage.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen, ImageFormat.Png)


            'MODULO - SALVANDO IMAGEN    (IdRoom + "_M")
            newSize = New Size(160, 120)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen & "_M", ImageFormat.Png)


            'THUMBNAL - SALVANDO IMAGEN  (IdRoom + "_T")
            newSize = New Size(70, 70)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen & "_T", ImageFormat.Png)
        Catch ex As Exception
            saveImage = False
        Finally
            If Not Bit Is Nothing Then
                Bit.Dispose()
            End If
        End Try
    End Function

    Private Sub dlRoomImages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlRoomImages.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            Dim hpl As LinkButton
            hpl = e.Item.FindControl("Linkbutton1")
            hpl.Attributes.Add("onclick", "javascript:return confirmDeleteImage();")
        End If
    End Sub

    Private Sub dlRoomImages_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlRoomImages.ItemCommand
        If e.CommandName = "Delete" Then
            Dim NombreImagen As String
            NombreImagen = CType(e.Item.FindControl("NombreImagen"), TextBox).Text
            With (New RoomFacade)
                If .DelExtraImageRoom(Me.IdEmpresa, Me.idTipoHabitacion, NombreImagen) Then
                    Try
                        If System.IO.File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen) Then
                            System.IO.File.Delete(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen)
                        End If
                        If System.IO.File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen & "_M") Then
                            System.IO.File.Delete(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen & "_M")
                        End If
                        If System.IO.File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen & "_T") Then
                            System.IO.File.Delete(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/" & NombreImagen & "_T")
                        End If
                    Catch ex As Exception
                        Response.Write("No puedo eliminar los archivos, no tengo permisos sobre el directorio: " & Server.MapPath(Request.ApplicationPath) & AppSettings("DIR_TIPO_HAB"))
                    End Try
                End If
            End With
        End If
        LoadImagesRooms()
    End Sub
    Public Function GetRootPath() As String
        Return AppSettings("urlCRS") & AppSettings("DIR_TIPO_HAB") & Me.IdEmpresa & "/"
    End Function
    Public Sub LoadImagesRooms()
        Dim data As DataSet = New DataSet
        If lstRoomType.Items.Count > 0 Then
            With (New RoomFacade)
                data = .GetExtraImageRoom(Me.IdEmpresa, Me.idTipoHabitacion)
                If data.Tables(0).Rows.Count > 0 Then
                    dlRoomImages.DataSource = data.Tables(0).DefaultView
                    dlRoomImages.DataBind()
                    dlRoomImages.Visible = True
                Else
                    dlRoomImages.Visible = False
                End If
            End With
        Else
            dlRoomImages.Visible = False
        End If
    End Sub
    Public Function GetName() As String
        Return PortalCulture.GetString("00103")
    End Function

    Private Sub lstRoomType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstRoomType.SelectedIndexChanged
        LoadImagesRooms()
    End Sub

    Private Sub ButtonMostrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMostrar.Click
        If lstRoomType.SelectedValue <> 0 Then
            Me.idTipoHabitacion = lstRoomType.SelectedValue
            lblError.Visible = False
            LoadImagesRooms()
        Else
            dlRoomImages.Visible = False
            lblError.Visible = True
        End If
    End Sub
End Class


