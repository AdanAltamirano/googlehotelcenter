
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Data.SqlClient
Imports APIServices.Models
Imports APIServices.Conflux
Imports APIServices.Conflux.Models.Restrictions.Room
Imports APIServices.Conflux.Models.Restrictions.Room.Response
Imports RateManager.Utitlities.Hotel
Imports Contenido.presentacion



Imports System.Configuration.ConfigurationManager

Partial Class ctrlRooms
    Inherits System.Web.UI.UserControl



#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblImagen As System.Web.UI.WebControls.Label
    Protected WithEvents FileImagen As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDivTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFaresCatalogue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkLinkRooms As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label


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

    Private Const KEY_IDROOM As String = "idRoom"
    Public Property idRoom() As Integer
        Get
            If ViewState.Item(KEY_IDROOM) Is Nothing Then
                Return 0
            Else
                Return ViewState.Item(KEY_IDROOM)
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add(KEY_IDROOM, Value)
        End Set
    End Property

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Private Const KEY_IDHOTEL As String = "idHotel"
    Public Property idHotel() As Integer
        Get
            If ViewState.Item(KEY_IDHOTEL) Is Nothing Then
                Return 0
            Else
                Return ViewState.Item(KEY_IDHOTEL)
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add(KEY_IDHOTEL, Value)
        End Set
    End Property

    Public Property idCompany() As Integer
        Get
            If ViewState.Item("idCompany") Is Nothing Then
                Return 0
            Else
                Return ViewState.Item("idCompany")
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState.Add("idCompany", Value)
        End Set
    End Property
    Public Property Adultos() As Integer
        Get
            Return ViewState("_Adultos")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_Adultos") = Value
        End Set
    End Property
    Public Property Ninios() As Integer
        Get
            Return ViewState("_Ninios")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_Ninios") = Value
        End Set
    End Property

    Public Property totalRooms As Integer
        Get
            Return ViewState("totalRooms")
        End Get
        Set(ByVal Value As Integer)
            ViewState("totalRooms") = Value
        End Set
    End Property
    Public Property edicion() As Boolean
        Get
            Return ViewState("Edicion")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Edicion") = Value

        End Set
    End Property


    Protected CtrlLanguageButton1 As ctrlLanguageButton

    Protected mlNameRoom As CtrlIdioma
    Protected mlDescriptionRoom As CtrlIdiomaRFCk
    Protected WithEvents ctrlImgRooms1 As ctrlImagesRooms

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'Registramos el script de validacion de cliente  

        If Request.Browser.Browser.IndexOf("IE") <> 0 Then
            Page.ClientTarget = "UpLevel"
        End If

        'Me.clientValidLst()

        If Not IsPostBack Then
            Me.newRoom()
            loadRoomsTypes()

        End If

        mlDescriptionRoom.Height = 200
        mlDescriptionRoom.IsHTML = True
        mlDescriptionRoom.IsMultiline = True
        mlNameRoom.IsMultiline = False
        Me.ddlMaxAdultRoll.Attributes.Add("onChange", "javascript:ShowPrice('" & Me.ddlMaxAdultRoll.ClientID & "','" & divPriceAdult.ClientID & "','" & Me.txtPriceAdultRoll.ClientID & "','txtDiv1','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "')")
        Me.ddlMaxChildRoll.Attributes.Add("onChange", "javascript:ShowPrice('" & Me.ddlMaxChildRoll.ClientID & "', '" & divPriceChild.ClientID & "','" & Me.txtPriceChildRoll.ClientID & "','txtDiv2','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "')")
        Me.ddlMaxCribRoll.Attributes.Add("onChange", "javascript:ShowPrice('" & Me.ddlMaxCribRoll.ClientID & "', '" & divPriceCrib.ClientID & "','" & Me.txtPriceCribRoll.ClientID & "','txtDiv3','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "')")
        Me.lstRoomType.Attributes.Add("onChange", "javascript:ShowName('" & lstRoomType.ClientID & "','" & mlNameRoom.ReturnNameTxtEn & "','" & mlNameRoom.ReturnNameTxtEs & "','" & Me.iSpanishDesc.ClientID & "','" & Me.iEnglishDesc.ClientID & "')")
        'btnfile.Attributes.Add("onclick", "javascript:saveImg('" & Me.lblfile.ClientID & "')")

        If Not IsPostBack Then
            ctrlImgRooms1.IdEmpresa = Me.idCompany
            ctrlImgRooms1.ModeView = Opciones.ViewMode.Edit
            ctrlImgRooms1.IdIdioma = PortalCulture.GetIDCulture
        End If

    End Sub


    Private Sub loadRoomsTypes()
        Dim room As RoomsTypeData
        Me.lstRoomType.Items.Clear()
        Me.iEnglishDesc.Value = ""
        Me.iSpanishDesc.Value = ""
        With New RoomTypeFacade

            room = .getRoomsTypes(PortalCulture.GetIDCulture)
            For Each dr As DataRow In room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows
                Me.iEnglishDesc.Value &= dr(RoomsTypeData.EnDescription).ToString & "*|*"
                Me.iSpanishDesc.Value &= dr(RoomsTypeData.EsDescription).ToString & "*|*"
            Next
            Dim lng As Integer
            For Each dr As DataRow In room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows
                lng = dr(RoomsTypeData.FLD_NAME).ToString.Length
                lng = If(lng > 40, 40, lng)
                dr(RoomsTypeData.FLD_NAME) = dr(RoomsTypeData.FLD_NAME).ToString.Substring(0, lng)
            Next
            Me.lstRoomType.DataSource = room.Tables(RoomsTypeData.TBL_ROOMTYPE)
            Me.lstRoomType.DataTextField = RoomsTypeData.FLD_NAME
            Me.lstRoomType.DataValueField = RoomsTypeData.FLD_ID_ROOM_TYPE
            Me.lstRoomType.DataBind()
            'For Each dr As DataRow In room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows
            '    Me.iEnglishDesc.Value &= dr(RoomsTypeData.EnDescription).ToString & "*|*"
            '    Me.iSpanishDesc.Value &= dr(RoomsTypeData.EsDescription).ToString & "*|*"
            'Next
        End With

        Try
            mlNameRoom.SetEN(room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.EnDescription))
            mlNameRoom.setES(room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.EsDescription))
        Catch ex As Exception
        End Try


    End Sub

    'Private Sub clientValidLst()
    '    If (Not Page.IsClientScriptBlockRegistered("clientScriptList")) Then
    '        Dim strScript As String
    '        strScript = "<script language=""javascript"">" & Chr(13)
    '        strScript &= "      function validateMaxChildrens(source, args){" & Chr(13)
    '        strScript &= "          var lstValidate = document.getElementById(source.controltovalidate);" & Chr(13)
    '        strScript &= "          var lstPeople = document.getElementById('" & Me.lstPeoplesInRoom.ClientID & "');" & Chr(13)
    '        strScript &= "          var lstPeopleExtra = document.getElementById('" & Me.lstPeoplesExtras.ClientID & "');" & Chr(13)
    '        strScript &= "          var maxPeople = eval(lstPeople.value) + eval(lstPeopleExtra.value);" & Chr(13)
    '        strScript &= "          var valueValidate = eval(lstValidate.value);" & Chr(13)
    '        strScript &= "          if(valueValidate < maxPeople)" & Chr(13)
    '        strScript &= "              args.IsValid = true;" & Chr(13)
    '        strScript &= "          else" & Chr(13)
    '        strScript &= "              args.IsValid = false;" & Chr(13)
    '        strScript &= "      }" & Chr(13)
    '        strScript &= "  //-->" & Chr(13)
    '        strScript &= "</script>" & Chr(13)
    '        Page.RegisterClientScriptBlock("clientScriptList", strScript)
    '    End If
    'End Sub




#Region "User control interface"

    Public Function MakeCodeRoom(ByVal CodeRoom As Integer) As String
        Dim dsRooms As DataTable = New DataTable
        Dim dv As DataView
        Dim Code As String = ""
        Dim valor As Integer
        dv = getAllRooms("")
        'dv = dsRooms.DefaultView
        dv.RowFilter = "idtipohabitacion = " & CodeRoom
        dv.Sort = " CodigoHabitacion asc"
        If dv.Count > 0 Then
            If Not dv(dv.Count - 1)("CodigoHabitacion") Is System.DBNull.Value Then
                If dv(dv.Count - 1)("CodigoHabitacion").ToString.Trim.Length > 3 Then
                    valor = Asc(dv(dv.Count - 1)("CodigoHabitacion").ToString.Substring(3, 1))
                    Code = dv(dv.Count - 1)("CodigoHabitacion").ToString.Substring(0, 3) & Chr(valor + 1)
                Else
                    Code = dv(dv.Count - 1)("CodigoHabitacion").ToString.Substring(0, 3) & "A"
                End If
            End If
        End If
        Return Code
    End Function

    Function getDataXML(ByVal idRoomHotel As Integer) As String
        Dim room As RoomsHotelData
        room = (New RoomFacade).getRoomByID(idRoomHotel)
        Return Util.Utility.GetXml(room.TBL_ROOM_HOTEL, "UpdateRooms", room)
    End Function

    Public Function loadRoom(ByVal idRoomHotel As Integer, ByVal codeRoom As String) As Boolean
        loadRoom = True
        'Try
        newRoom()
        With New RoomFacade
            Dim room As RoomsHotelData
            room = .getRoomByID(idRoomHotel)
            If room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
            'Checamos si pertenece a este hotel la habitacion

            Me.HasData = True
            'Ponemos el id en el viestate
            idRoom = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_ROOM_HOTEL)
            idHotel = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_HOTEL)
            'Llenamos los campos  'Valida null

            Me.lstRoomType.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_ROOM_TYPE)
            Me.lstRoomType.Enabled = False


            Me.txtNumberRooms.Text = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NUMBER_ROOMS)
            Me.txtNumberRooms.Enabled = False
            'Me.txtNombre.Text = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NOMBRE)
            mlNameRoom.CargaDatos(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_NOMBRE))
            mlNameRoom.textodefault = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NOMBRE)

            'Me.txtNumberRooms.Enabled = False

            Me.lstNumberAdults.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NUMBER_MAXADULTS)
            Me.lstNumberChildrens.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NUMBER_MAXCHILDREN)
            Me.lstPeoplesInRoom.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NUMBER_PEOPLESINROOM)
            Me.lstPeoplesExtras.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NUMBER_PEOPLESEXTRAS)
            Me.txtDescription.Text = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_DESCRIPTION)

            mlDescriptionRoom.CargaDatos(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_DESCRIPTION))
            mlDescriptionRoom.textodefault = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_DESCRIPTION)

            If Not room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).IsNull(RoomsHotelData.FLD_ORDEN) And Not room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ORDEN).ToString().Equals("---") Then
                'Me.txtOrden.Text = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ORDEN)


                Dim ordenRoom As Integer = CInt(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ORDEN).ToString())

                If (ordenRoom > totalRooms) Then
                    Me.ddlOrden.SelectedValue = totalRooms.ToString()
                Else

                    Me.ddlOrden.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ORDEN)
                End If



            End If

            Me.lstMinNumberAdults.SelectedIndex = 0
            If Not room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).IsNull(RoomsHotelData.FLD_NUMBER_MINADULTS) Then
                Me.lstMinNumberAdults.SelectedValue = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_NUMBER_MINADULTS)
            End If



            'Cargamos el diccionario
            'Me.CtrlLanguageButton1.Visible = True
            'Me.CtrlLanguageButton2.Visible = True

            'Me.CtrlLanguageButton1.loadDictionaryById(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_DESCRIPTION))
            'Me.CtrlLanguageButton2.loadDictionaryById(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_NOMBRE))


            'Modificado: Jorge, agrege esta llamada que checa si existe una imagen que
            'corresponde al room

            ImagenHabitacion.Visible = IMG_Exists(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_ROOM_HOTEL), ImagenHabitacion)
            If ImagenHabitacion.Visible = False Then
                ImagenHabitacion.Visible = IMG_Exists(room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_ROOM_TYPE), ImagenHabitacion)
            End If

            With room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)
                If Not .Item(RoomsHotelData.FLD_MaxAdultCribRoll) Is System.DBNull.Value AndAlso .Item(RoomsHotelData.FLD_MaxAdultCribRoll) > 0 Then
                    Me.ddlMaxAdultRoll.SelectedValue = .Item(RoomsHotelData.FLD_MaxAdultCribRoll)
                    Me.divPriceAdult.Style.Add("display", "block")
                    Me.divlabelPrice.Style.Add("display", "block")
                    CType(Me.Page, Rooms).txtD1 = 1
                    Me.lblPriceCribRoll.Visible = True
                End If
                If Not .Item(RoomsHotelData.FLD_MaxChildCribRoll) Is System.DBNull.Value AndAlso .Item(RoomsHotelData.FLD_MaxChildCribRoll) > 0 Then
                    Me.ddlMaxChildRoll.SelectedValue = .Item(RoomsHotelData.FLD_MaxChildCribRoll)
                    Me.divPriceChild.Style.Add("display", "block")
                    Me.divlabelPrice.Style.Add("display", "block")
                    CType(Me.Page, Rooms).txtD2 = 1
                    Me.lblPriceCribRoll.Visible = True
                End If
                If Not .Item(RoomsHotelData.FLD_MaxCribRoll) Is System.DBNull.Value AndAlso .Item(RoomsHotelData.FLD_MaxCribRoll) > 0 Then
                    Me.ddlMaxCribRoll.SelectedValue = .Item(RoomsHotelData.FLD_MaxCribRoll)
                    Me.divPriceCrib.Style.Add("display", "block")
                    Me.divlabelPrice.Style.Add("display", "block")
                    CType(Me.Page, Rooms).txtD3 = 1
                    Me.lblPriceCribRoll.Visible = True
                End If
                If Not .Item(RoomsHotelData.FLD_PriceAdultRoll) Is System.DBNull.Value AndAlso .Item(RoomsHotelData.FLD_PriceAdultRoll) > 0 Then
                    Me.txtPriceAdultRoll.Text = .Item(RoomsHotelData.FLD_PriceAdultRoll)
                End If
                If Not .Item(RoomsHotelData.FLD_PriceChildRoll) Is System.DBNull.Value AndAlso .Item(RoomsHotelData.FLD_PriceChildRoll) > 0 Then
                    Me.txtPriceChildRoll.Text = .Item(RoomsHotelData.FLD_PriceChildRoll)
                End If
                If Not .Item(RoomsHotelData.FLD_PriceCribRoll) Is System.DBNull.Value AndAlso .Item(RoomsHotelData.FLD_PriceCribRoll) > 0 Then
                    Me.txtPriceCribRoll.Text = .Item(RoomsHotelData.FLD_PriceCribRoll)
                End If
            End With

            ctrlImgRooms1.IdTypeRoomHotel = idRoomHotel
            ctrlImgRooms1.CodeRoomTypeHotel = codeRoom
            ctrlImgRooms1.LoadImagesByRoomHotel(ctrlImgRooms1.IdTypeRoomHotel, ctrlImgRooms1.CodeRoomTypeHotel)


        End With
        'Catch e As Exception
        Return True
        'End Try
    End Function

    Public Function getAllRooms(ByVal sFiltro As String) As DataView
        Dim dv As DataView

        With New RoomFacade
            Dim room As RoomsHotelData
            room = .getAllRooms(idHotel, PortalCulture.GetIDCulture)
            If Not String.IsNullOrEmpty(sFiltro) Then
                dv = room.Tables(0).DefaultView
                dv.RowFilter = sFiltro
                Return dv
            End If
            Return room.Tables(0).DefaultView
        End With
    End Function

    Public Function SaveRoom(ByRef Er As Integer, ByVal publish As Boolean) As Boolean

        Dim roomData As RoomData = New RoomData()

        Dim rooms As RoomsHotelData
        Dim RoomCode As String
        Dim sdataPrev As String
        Dim sdata As String
        Me.Adultos = lstNumberAdults.SelectedValue
        Me.Ninios = lstNumberChildrens.SelectedValue
        If Not Me.Page.IsValid Then
            Return False
        End If

        sdata = ""
        sdataPrev = ""
        RoomCode = lstRoomType.SelectedItem.Text.Split("-")(0).Trim
        If idRoom = 0 Then   'Save
            'Se requiere el id de hotel
            '  If idHotel = 0 Then Return False

            With New RoomFacade

                If ImgFileOpen.Value <> "" Then
                    If IsValidIMG(ImgFileOpen) Then
                        cvImagen.IsValid = True
                        SaveRoom =
                        .createRoom(Me.lstRoomType.SelectedValue,
                           Me.idHotel,
                           Me.txtNumberRooms.Text,
                           Me.lstPeoplesInRoom.SelectedValue,
                           Me.lstPeoplesExtras.SelectedValue,
                           Me.lstNumberAdults.SelectedValue,
                           Me.lstNumberChildrens.SelectedValue,
                           Me.mlDescriptionRoom.textodefault,
                           mlNameRoom.textodefault,
                           RoomCode, 0, 0, Me.ddlMaxAdultRoll.SelectedValue,
                            Me.ddlMaxChildRoll.SelectedValue, Me.ddlMaxCribRoll.SelectedValue,
                            CDbl(Val(Me.txtPriceAdultRoll.Text)), CDbl(Val(Me.txtPriceChildRoll.Text)), CInt(Val(Me.txtPriceCribRoll.Text)),
                           rooms, Me.ddlOrden.SelectedValue, lstMinNumberAdults.SelectedValue)
                        'si se inserto el registro, entonces  se procede a subir la imagen 
                        If SaveRoom Then
                            mlDescriptionRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_DESCRIPTION), publish)
                            mlNameRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_NOMBRE), publish)
                            saveImage(rooms.Tables(0).Rows(0).Item("idTipoHabitacion_Hotel"), ImgFileOpen)

                            'Google Room Data
                            roomData.RoomID = RoomCode
                            roomData.RoomName = mlNameRoom.textodefault
                            roomData.RoomDescription = Me.mlDescriptionRoom.textodefault
                            roomData.Capacity = CType(Me.lstPeoplesInRoom.SelectedValue, Integer)
                            roomData.AdultCapcity = CType(Me.lstNumberAdults.SelectedValue, Integer)

                        End If
                    Else
                        cvImagen.IsValid = False
                        SaveRoom = False
                        Er = 1
                    End If
                Else
                    SaveRoom =
                    .createRoom(Me.lstRoomType.SelectedValue,
                       Me.idHotel,
                       Me.txtNumberRooms.Text,
                       Me.lstPeoplesInRoom.SelectedValue,
                       Me.lstPeoplesExtras.SelectedValue,
                       Me.lstNumberAdults.SelectedValue,
                       Me.lstNumberChildrens.SelectedValue,
                       Me.mlDescriptionRoom.textodefault,
                       mlNameRoom.textodefault,
                       RoomCode, 0, 0, Me.ddlMaxAdultRoll.SelectedValue,
                            Me.ddlMaxChildRoll.SelectedValue, Me.ddlMaxCribRoll.SelectedValue,
                        CDbl(Val(Me.txtPriceAdultRoll.Text)), CDbl(Val(Me.txtPriceChildRoll.Text)), CInt(Val(Me.txtPriceCribRoll.Text)),
                        rooms, Me.ddlOrden.SelectedValue, lstMinNumberAdults.SelectedValue)
                    If SaveRoom Then
                        mlDescriptionRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_DESCRIPTION), publish)
                        mlNameRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_NOMBRE), publish)

                        'Google Room Data
                        roomData.RoomID = RoomCode
                        roomData.RoomName = mlNameRoom.textodefault
                        roomData.RoomDescription = Me.mlDescriptionRoom.textodefault
                        roomData.Capacity = CType(Me.lstPeoplesInRoom.SelectedValue, Integer)
                        roomData.AdultCapcity = CType(Me.lstNumberAdults.SelectedValue, Integer)

                    End If
                End If

                If SaveRoom Then
                    mlDescriptionRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_DESCRIPTION), publish)
                    mlNameRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_NOMBRE), publish)
                End If
            End With
            If SaveRoom Then
                sdata = Util.Utility.GetXml(rooms.TBL_ROOM_HOTEL, "UpdateRooms", rooms)
                CType(Me.Page, PaginaBase).guardalog("/Pages/Rooms.aspx", PaginaBase.acciones.Crear, "Se Creó la habitación " & lstRoomType.SelectedItem.Text & " - " & mlNameRoom.textodefault & " de el hotel " & CType(Me.Page, PaginaBase).cInfoActual.HotelName, "", "", sdata)
                Me.newRoom()
                Me.idRoom = rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ID_ROOM_HOTEL)
                'CType(Me.Page, PaginaBase).guardalog("/Pages/Rooms.aspx", PaginaBase.acciones.Modificar, "Se modificó la habitación " & grid.Items(grid.SelectedIndex).Cells(columns.Tipo).Text & " - " & grid.Items(grid.SelectedIndex).Cells(columns.nameroom).Text & " de el hotel " & Me.cInfoActual.HotelName)

                'Guardar Imagenes cuando es una nueva habitacion

                ctrlImgRooms1.IdTypeRoomHotel = rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_ROOM_HOTEL).ToString()
                ctrlImgRooms1.SaveImages()


                'Google Request

                SendRoomGoogle(roomData)

                Return SaveRoom
            End If
        Else    'Update
            sdataPrev = getDataXML(idRoom)

            With New RoomFacade
                If ImgFileOpen.Value <> "" Then
                    If IsValidIMG(ImgFileOpen) Then
                        'TODO: Se debe de checar el cambio de max adults ect etc
                        'Para modificar las reglas de habitacion (Eliminar reglas que no cumplan con el nuevo criterio)
                        'Y enviar un mensaje al usuario
                        SaveRoom =
                        .updateRoom(Me.idRoom,
                           Me.lstRoomType.SelectedValue,
                           Me.idHotel,
                           Me.txtNumberRooms.Text,
                           Me.lstPeoplesInRoom.SelectedValue,
                           Me.lstPeoplesExtras.SelectedValue,
                           Me.lstNumberAdults.SelectedValue,
                           Me.lstNumberChildrens.SelectedValue,
                           Me.mlDescriptionRoom.textodefault,
                           mlNameRoom.textodefault,
                         "", 0, 0, Me.ddlMaxAdultRoll.SelectedValue,
                            Me.ddlMaxChildRoll.SelectedValue, Me.ddlMaxCribRoll.SelectedValue,
                        CDbl(Val(Me.txtPriceAdultRoll.Text)), CDbl(Val(Me.txtPriceChildRoll.Text)), CInt(Val(Me.txtPriceCribRoll.Text)),
                           rooms, Me.ddlOrden.SelectedValue, lstMinNumberAdults.SelectedValue, False)
                        If SaveRoom Then
                            mlDescriptionRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_DESCRIPTION), publish)
                            mlNameRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_NOMBRE), publish)
                            updateImage(Me.ImagenHabitacion.ImageUrl, ImgFileOpen, Me.idRoom)

                        End If
                    Else
                        cvImagen.IsValid = False
                        SaveRoom = False
                        Er = 1
                    End If
                Else
                    SaveRoom =
                    .updateRoom(Me.idRoom,
                     Me.lstRoomType.SelectedValue,
                     Me.idHotel,
                     Me.txtNumberRooms.Text,
                     Me.lstPeoplesInRoom.SelectedValue,
                     Me.lstPeoplesExtras.SelectedValue,
                     Me.lstNumberAdults.SelectedValue,
                     Me.lstNumberChildrens.SelectedValue,
                     Me.mlDescriptionRoom.textodefault,
                     mlNameRoom.textodefault,
                    "", 0, 0, Me.ddlMaxAdultRoll.SelectedValue,
                            Me.ddlMaxChildRoll.SelectedValue, Me.ddlMaxCribRoll.SelectedValue,
                        CDbl(Val(Me.txtPriceAdultRoll.Text)), CDbl(Val(Me.txtPriceChildRoll.Text)), CInt(Val(Me.txtPriceCribRoll.Text)),
                     rooms, Me.ddlOrden.SelectedValue, lstMinNumberAdults.SelectedValue, False)

                    If SaveRoom Then
                        Dim idDictionryDesc As Integer = rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_DESCRIPTION)
                        Dim idDictionryName As Integer = rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_NOMBRE)
                        .GetDictionaryForUpdate(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_ROOM_HOTEL), idDictionryDesc, idDictionryName)
                        'mlDescriptionRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_DESCRIPTION), publish)
                        'mlNameRoom.Update(rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)(RoomsHotelData.FLD_ID_NOMBRE), publish)

                        mlDescriptionRoom.Update(idDictionryDesc, publish)
                        mlNameRoom.Update(idDictionryName, publish)
                        ctrlImgRooms1.SaveImages()
                    End If
                End If
            End With
        End If
        If Not SaveRoom Then
            'Me.Page.RegisterStartupScript("a", "<script>ShowPrice('" & Me.ddlMaxAdultRoll.ClientID & "','" & divPriceAdult.ClientID & "','" & Me.txtPriceAdultRoll.ClientID & "','txtDiv1','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "');</script>")
            'Me.Page.RegisterStartupScript("b", "<script>ShowPrice('" & Me.ddlMaxChildRoll.ClientID & "', '" & divPriceChild.ClientID & "','" & Me.txtPriceChildRoll.ClientID & "','txtDiv2','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "');</script>")
            'Me.Page.RegisterStartupScript("c", "<script>ShowPrice('" & Me.ddlMaxCribRoll.ClientID & "', '" & divPriceCrib.ClientID & "','" & Me.txtPriceCribRoll.ClientID & "','txtDiv3','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "');</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "c", "<script>ShowPrice('" & Me.ddlMaxAdultRoll.ClientID & "','" & divPriceAdult.ClientID & "','" & Me.txtPriceAdultRoll.ClientID & "','txtDiv1','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "');</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "c", "<script>ShowPrice('" & Me.ddlMaxChildRoll.ClientID & "', '" & divPriceChild.ClientID & "','" & Me.txtPriceChildRoll.ClientID & "','txtDiv2','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "');</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "c", "<script>ShowPrice('" & Me.ddlMaxCribRoll.ClientID & "', '" & divPriceCrib.ClientID & "','" & Me.txtPriceCribRoll.ClientID & "','txtDiv3','" & Me.lblPriceCribRoll.ClientID & "','" & Me.divlabelPrice.ClientID & "');</script>")
        Else

            sdata = Util.Utility.GetXml(rooms.TBL_ROOM_HOTEL, "UpdateRooms", rooms)
            CType(Me.Page, PaginaBase).guardalog("/Pages/Rooms.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Se modificó la habitación " & lstRoomType.SelectedItem.Text & " - " & mlNameRoom.textodefault & " de el hotel " & CType(Me.Page, PaginaBase).cInfoActual.HotelName, "", sdataPrev, sdata)
            If Me.mlDescriptionRoom.HasChanges OrElse Me.mlNameRoom.HasChanges Then
                CType(Me.Page, PaginaBase).NotifyContentModification("Habitacion " & lstRoomType.SelectedItem.Text & " - " & mlNameRoom.textodefault)
            End If
            Me.newRoom()
        End If
    End Function


    Public Function deleteRoom(ByVal elimina As Boolean) As Integer
        With New RoomFacade
            'Modificado: agrege el parametro Me.lstRoomType.SelectedValue  la funcion 
            Dim idmessage As Integer
            With New RoomFacade
                idmessage = .EliminateRoom(idRoom, (Not elimina), Me.idHotel) '.deleteRoom(idRoom, Me.lstRoomType.SelectedValue)
            End With
            Me.newRoom()
            Return 0
            'With New RoomFacade
            '    idmessage = .deleteRoom(idRoom) '.deleteRoom(idRoom, Me.lstRoomType.SelectedValue)
            'End With
            If idmessage = 0 Then
                'modificado: agrege esto la function
                Try
                    If System.IO.File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & idRoom) Then
                        System.IO.File.Delete(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & idRoom)
                    End If
                    If System.IO.File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & idRoom & "_M") Then
                        System.IO.File.Delete(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & idRoom & "_M")
                    End If
                    If System.IO.File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & idRoom & "_T") Then
                        System.IO.File.Delete(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & idRoom & "_T")
                    End If
                Catch ex As Exception
                    Response.Write("No puedo eliminar los archivos, no tengo permisos sobre el directorio: " & Server.MapPath(Request.ApplicationPath) & AppSettings("DIR_TIPO_HAB"))
                End Try
            Else
                Return idmessage
            End If
        End With
        Me.newRoom()
    End Function

    Public Function activeRoom() As Integer
        Dim idmessage As Integer
        With New RoomFacade
            idmessage = .ActiveRoom(idRoom, 0, Me.idHotel)
        End With

        Me.newRoom()

        Return idmessage
    End Function


    Public Function newRoom() As Boolean
        Me.CtrlLanguageButton1.Visible = False
        'Me.CtrlLanguageButton2.Visible = False
        Me.HasData = False
        Me.txtDescription.Text = ""
        mlNameRoom.Limpia()
        mlDescriptionRoom.Limpia()
        Me.txtNumberRooms.Text = 1
        If Me.lstRoomType.TabIndex > 0 Then Me.lstRoomType.TabIndex = 0
        Me.lstNumberAdults.SelectedIndex = 0
        Me.lstMinNumberAdults.SelectedIndex = 0
        Me.lstNumberChildrens.SelectedIndex = 0
        Me.lstPeoplesExtras.SelectedIndex = 0
        Me.lstPeoplesInRoom.SelectedIndex = 0

        Me.ImagenHabitacion.ImageUrl = ""
        Me.ImagenHabitacion.Visible = False
        idRoom = 0
        Me.lstRoomType.Enabled = True
        Me.txtNumberRooms.Enabled = True
        Me.ddlMaxAdultRoll.SelectedIndex = 0
        Me.ddlMaxChildRoll.SelectedIndex = 0
        Me.ddlMaxCribRoll.SelectedIndex = 0

        Me.txtPriceAdultRoll.Text = ""
        Me.txtPriceChildRoll.Text = ""
        Me.txtPriceCribRoll.Text = ""

        'Me.lblPriceCribRoll.Visible = False


        Me.divPriceAdult.Style.Add("display", "none")
        Me.divPriceChild.Style.Add("display", "none")
        Me.divPriceCrib.Style.Add("display", "none")
        Me.divlabelPrice.Style.Add("display", "none")
        Me.txtNumberRooms.Enabled = True
        'Me.txtOrden.Text = String.Empty
        Try
            mlNameRoom.SetEN(lstRoomType.SelectedItem.Text.Split("-")(1))
            mlNameRoom.setES(lstRoomType.SelectedItem.Text.Split("-")(1))
        Catch ex As Exception
        End Try
        'Me.lblfile.Value = ""

        ctrlImgRooms1.IdTypeRoomHotel = 0
        ctrlImgRooms1.CodeRoomTypeHotel = ""
        ctrlImgRooms1.LoadImagesByRoomHotel(ctrlImgRooms1.IdTypeRoomHotel, ctrlImgRooms1.CodeRoomTypeHotel)

    End Function

    Public Function saveImage(ByVal IdImg As Integer, ByRef lblfileName As HtmlInputFile) As Boolean
        '**********************************************************************************************
        'GUARDA LA IMAGEN DEL TAMAÑO DEL MODULO  
        'AppSettings("DIR_TIPO_HAB") + IdCompamy + Tipohabitacion

        ' Dim oImg As System.Drawing.Image

        If Not Directory.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/") Then
            Directory.CreateDirectory(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/")

        End If

        Dim BitImage As New Bitmap(lblfileName.PostedFile.InputStream)
        Dim Bit As Bitmap = Nothing

        Dim newSize As Size
        Try
            'ORIGINAL - SALVANDO IMAGEN  (IdRoom)

            lblfileName.PostedFile.SaveAs(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/" & IdImg)


            'MODULO - SALVANDO IMAGEN    (IdRoom + "_M")
            newSize = New Size(160, 120)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/" & IdImg & "_M", ImageFormat.Jpeg)


            'THUMBNAL - SALVANDO IMAGEN  (IdRoom + "_T")
            newSize = New Size(70, 70)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/" & IdImg & "_T", ImageFormat.Jpeg)
        Catch ex As Exception
            saveImage = False
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

    Public Function updateImage(ByVal oldUrl As String, ByRef lblfile As System.Web.UI.HtmlControls.HtmlInputFile, ByVal Idimg As Integer) As Boolean
        'Dim oImg As System.Drawing.Image

        Try
            If Not Directory.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/") Then
                Directory.CreateDirectory(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/")
            End If

            Dim BitImage As New Bitmap(lblfile.PostedFile.InputStream)
            Dim Bit As Bitmap
            Dim newSize As Size

            '--------------------------------------------------------
            'AppSettings("DIR_TIPO_HAB") + IdCompamy + Tipohabitacion
            '--------------------------------------------------------


            'ORIGINAL - SALVANDO IMAGEN  (IdRoom)
            BitImage.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/" & Idimg, ImageFormat.Png)

            'MODULO - SALVANDO IMAGEN    (IdRoom + "_M")
            newSize = New Size(160, 120)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/" & Idimg & "_M", ImageFormat.Png)

            'THUMBNAiL - SALVANDO IMAGEN  (IdRoom + "_T")
            newSize = New Size(70, 70)
            Bit = BitImage.GetThumbnailImage(newSize.Width, newSize.Height, Nothing, Nothing)
            Bit.Save(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & Me.idCompany & "/" & Idimg & "_T", ImageFormat.Png)


        Catch ex As Exception
            updateImage = False
        End Try

    End Function

    Public Function IMG_Exists(ByVal IdImg As Integer, ByRef img As System.Web.UI.WebControls.Image) As Boolean
        Dim tmpCheckExists As Boolean = File.Exists(AppSettings("MapPathCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & IdImg & "_T")

        If tmpCheckExists Then
            img.ImageUrl = AppSettings("urlCRS") & AppSettings("DIR_TIPO_HAB") & idCompany & "/" & IdImg & "_T?" & Now.ToString
            IMG_Exists = tmpCheckExists
        Else
            img.ImageUrl = ""
            Return False
        End If
    End Function

#End Region
    Public Sub cargarOrden()
        Dim i As Integer

        ddlOrden.Items.Clear()
        ddlOrden.Items.Insert(0, "0")

        For i = 1 To Me.totalRooms
            ddlOrden.Items.Add(i.ToString)
        Next

        If Me.edicion = False Then
            ddlOrden.Items.Add(ddlOrden.Items.Count)
            ddlOrden.SelectedIndex = ddlOrden.Items.Count - 1
        End If
    End Sub

    Private Sub loadResources()
        rfvRoomsType.Text = PortalCulture.GetString("00071")
        lblTypeRoom.Text = PortalCulture.GetString("00072", True)
        lblNombre.Text = PortalCulture.GetString("00073", True)
        lblNumberRooms.Text = PortalCulture.GetString("00074", True)
        Me.lblMinNumberAdults.Text = PortalCulture.GetString("01175", True)
        Me.lblNumberAdults.Text = PortalCulture.GetString("01176", True)
        lblPeoplesExtras.Text = PortalCulture.GetString("00076", True)
        lblNumberChildrens.Text = PortalCulture.GetString("00077", True)
        rfvDescription.Text = PortalCulture.GetString("00078")
        RegularExpressionValidator1.Text = PortalCulture.GetString("00079")
        'Regularexpressionvalidator2.Text = PortalCulture.GetString("00784")

        lblImgShow.Text = PortalCulture.GetString("00080")
        lblDescription.Text = PortalCulture.GetString("00081")
        Requiredfieldvalidator1.Text = PortalCulture.GetString("00078")
        'Requiredfieldvalidator2.Text = PortalCulture.GetString("00785")

        Me.ValAdultPriceCribRoll.Text = PortalCulture.GetString("00083")
        Me.ValChildPriceCribRoll.Text = PortalCulture.GetString("00083")
        Me.valPriceCribRoll.Text = PortalCulture.GetString("00083")
        Me.lblMaxAdultRoll.Text = PortalCulture.GetString("00087")
        Me.lblMaxChildRoll.Text = PortalCulture.GetString("00088")
        Me.lblMaxCribRoll.Text = PortalCulture.GetString("00089")
        Me.lblPriceCribRoll.Text = PortalCulture.GetString("00090", True)
        lblRollAway.Text = PortalCulture.GetString("00091", True)
        lblExtraData.Text = PortalCulture.GetString("00417")
        lblOrden.Text = PortalCulture.GetString("00920", True)
        lbPersonas.Text = PortalCulture.GetString("00082")

        'btnfile.Value = PortalCulture.GetString("00472")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub

#Region "Google"

    Private Sub SendRoomGoogle(ByVal roomData As RoomData)

        Dim confluxService As New ConfluxService()
        Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
        Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)

        If isEnabledGoogleRequest Then

            Dim res As RoomResponse = confluxService.InsertRoom(info.Empresa, roomData)

            Dim note As String = String.Format("Crear habitacion {0} en conflux para el hotel {1}", roomData.RoomID, info.Hotel)

            CType(Me.Page, PaginaBase).guardalog("/Pages/Rooms.aspx", PaginaBase.acciones.Crear, note, "", res.RequestXML, res.Response)

        End If

    End Sub

#End Region


End Class

