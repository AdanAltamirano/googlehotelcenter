Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade

Imports System.Configuration.ConfigurationManager

Partial Class ctrlRoomType
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
    Private Const KEY_IDROOM As String = "idRoomType"
    Private Const KEY_IDNAME As String = "idName"

    Private Property idRoom() As Integer
        Get
            If viewstate.Item(KEY_IDROOM) Is Nothing Then
                Return 0
            Else
                Return viewstate.Item(KEY_IDROOM)
            End If
        End Get
        Set(ByVal Value As Integer)
            viewstate.Add(KEY_IDROOM, Value)
        End Set
    End Property

    Private Property idName() As Integer
        Get
            If viewstate.Item(KEY_IDNAME) Is Nothing Then
                Return 0
            Else
                Return viewstate.Item(KEY_IDNAME)
            End If
        End Get
        Set(ByVal Value As Integer)
            viewstate.Add(KEY_IDNAME, Value)
        End Set
    End Property

    Private Property Editando() As Boolean
        Get
            Return viewstate("Editando")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("Editando") = Value
        End Set
    End Property

    Public Property sError() As String
        Get
            Return ViewState("_sError")
        End Get
        Set(ByVal value As String)
            ViewState("_sError") = value
        End Set
    End Property

	Protected mlNameRoom As CtrlIdioma

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Introducir aquí el código de usuario para inicializar la página
		If Not IsPostBack Then
			Me.newRoomType()
			Me.Editando = False
		End If
        sError = PortalCulture.GetString("00460")
		mlNameRoom.Width = 200
        mlNameRoom.IsMultiline = False
        txtCodeRoom.Attributes.Add("onkeypress", "return validarkeyCode(event);")
	End Sub

	Public Function loadRoomType(ByVal idRoomType As Integer) As Boolean
		loadRoomType = True
		newRoomType()
		With New RoomTypeFacade
			Dim room As RoomsTypeData
			room = .getRoomTypeByID(idRoomType)

			'Obtenemos el tipo de habitacion 
			If room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows.Count = 0 Then Return False
			idRoom = room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.FLD_ID_ROOM_TYPE)
			idName = room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.FLD_ID_NAME)
			Me.txtCodeRoom.Text = "" & room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.FLD_ROOMCODE)

			'Cargamos el diccionario
			Me.mlNameRoom.CargaDatos(room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.FLD_ID_NAME))
			Me.mlNameRoom.textodefault = room.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0).Item(RoomsTypeData.FLD_NAME)
			Me.Editando = True
			Me.txtCodeRoom.Enabled = False
		End With
	End Function

	Public Function getAllRoomsTypes() As DataTable
		With New RoomTypeFacade
			Dim room As RoomsTypeData
			room = .getRoomsTypes()
			getAllRoomsTypes = room.Tables(RoomsTypeData.TBL_ROOMTYPE)
		End With
	End Function

    Public Function SaveRoomType() As Boolean
        Dim rooms As New RoomsTypeData
        If Not Page.IsValid Then Return False

        If Not Regex.IsMatch(Me.txtCodeRoom.Text.Trim, "^[A-Z0-9 a-z]*$") Then
            sError = PortalCulture.GetString("01512")
            Return False
        End If

        If idRoom = 0 Then        'Save
            With New RoomTypeFacade
                SaveRoomType = .createRoomType(Me.mlNameRoom.textodefault, Me.txtCodeRoom.Text.Trim, rooms)
            End With
            CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/PagesGeneral/RoomType.aspx", PaginaBase.acciones.Crear, "Creo la habitación " & txtCodeRoom.Text.Trim)
        Else    'Update
            With New RoomTypeFacade
                SaveRoomType = .updateRoomType(idRoom, Me.mlNameRoom.textodefault, rooms)
                CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/PagesGeneral/RoomType.aspx", PaginaBase.acciones.Modificar, "Modificó la habitación " & txtCodeRoom.Text.Trim)
            End With
        End If
        If SaveRoomType Then
            Me.mlNameRoom.Update(rooms.Tables(RoomsTypeData.TBL_ROOMTYPE).Rows(0)(RoomsTypeData.FLD_ID_NAME))
            Me.newRoomType()
        End If
    End Function

	Public Function deleteRoomType() As Boolean
		If idRoom = 0 Then Return False
		With New RoomTypeFacade
            deleteRoomType = .deleteRoomType(idRoom)
            CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/PagesGeneral/RoomType.aspx", PaginaBase.acciones.Eliminar, "Eliminó la habitación " & txtCodeRoom.Text.Trim)
		End With
		Me.newRoomType()
    End Function
    Public Function deleteRoomType(ByVal room As Integer) As Boolean
        With New RoomTypeFacade
            deleteRoomType = .deleteRoomType(room)
            If deleteRoomType Then
                CType(Me.Page, PaginaBase).guardalog("/HotelAdministrator/PagesGeneral/RoomType.aspx", PaginaBase.acciones.Eliminar, "Eliminó la habitación " & room)
            End If
        End With
        Me.newRoomType()
    End Function

    Public Function newRoomType() As Boolean
        mlNameRoom.Limpia()
        Me.Editando = False
        idRoom = 0
        idName = 0
        Me.txtCodeRoom.Text = ""
        Me.txtCodeRoom.Enabled = True
    End Function

    Private Sub loadResources()
        lblNameDefault.Text = PortalCulture.GetString("M000066", True)
        If Me.Editando Then
            Me.lblEdit.Text = PortalCulture.GetString("M000064")    '& " " & Me.txtNameDefault.Text			 '"Editando tipo de habitación "
        Else
            Me.lblEdit.Text = PortalCulture.GetString("M000063")    '"Nuevo Tipo Habitación"
        End If
        lblCode.Text = PortalCulture.GetString("00441", True)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub
End Class

