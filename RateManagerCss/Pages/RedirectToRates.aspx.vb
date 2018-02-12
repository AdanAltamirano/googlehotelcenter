Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Partial Class RedirectToRates
    Inherits PaginaBase
    Private Property room() As Integer
        Get
            Return viewstate("_room")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_room") = Value
        End Set
    End Property

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
        If Not IsPostBack Then
            ShowDiv.Style.Add("display", "none")
            ModifDiv.Style.Add("display", "none")
            If Not Request.QueryString("id") Is Nothing AndAlso Request.QueryString("id") = 1 Then
                room = Request.QueryString("Room")
                ChangeTarifasDiv()
            ElseIf Not Request.QueryString("id") Is Nothing AndAlso Request.QueryString("id") = 0 Then
                room = Request.QueryString("Room")
                DefinirTarifasDiv()
            End If
        End If
    End Sub

    Private Sub DefinirTarifasDiv()
        ShowDiv.Style.Add("display", "block")
        lnkLinkRooms.Visible = False
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRooms(Me.cInfoActual.Hotel)
        End With
        If room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count > 1 Then
            lnkLinkRooms.Visible = True
        End If

    End Sub

    Private Sub ChangeTarifasDiv()
        ModifDiv.Style.Add("display", "block")
    End Sub

    Private Sub lnkLinkRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkLinkRooms.Click
        Response.Redirect(GeRequestApplicationPath(String.Concat("/Pages/RoomsLinks.aspx?Room=", room)))
    End Sub

    Private Sub lnkFares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkFares.Click
        Response.Redirect(GeRequestApplicationPath(String.Concat("/Pages/FaresCatalogue.aspx?Room=", room)))
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitleChangeRates.Text = PortalCulture.GetString("00271")
        lnkFares.Text = PortalCulture.GetString("00272")
        lnkFaresCatalogue.Text = PortalCulture.GetString("00272")
        lnkLinkRooms.Text = PortalCulture.GetString("00273")
        lblDivTitle.Text = PortalCulture.GetString("00274")
        lnkRooms.Text = PortalCulture.GetString("00345")
        Me.lblOr.Text = PortalCulture.GetString("00346")
    End Sub

    Private Sub lnkFaresCatalogue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkFaresCatalogue.Click
        Response.Redirect(GeRequestApplicationPath(String.Concat("/Pages/FaresCatalogue.aspx?Room=", room)))
    End Sub

    Private Sub lnkRooms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRooms.Click
        Response.Redirect(GeRequestApplicationPath("/Pages/rooms.aspx"))
    End Sub
End Class
