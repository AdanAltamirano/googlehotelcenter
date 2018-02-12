Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Partial Class FaresRoom
    Inherits PaginaBase
    Private Property Room() As Integer
        Get
            Return viewstate("_IdRoom")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_IdRoom") = Value
        End Set
    End Property
    Private Property Ninios() As Integer
        Get
            Return viewstate("_ninios")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_ninios") = Value
        End Set
    End Property
    Private Property Adultos() As Integer
        Get
            Return viewstate("_Adults")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_Adults") = Value
        End Set
    End Property
    Private Property RoomCode() As String
        Get
            Return viewstate("_coderoom")
        End Get
        Set(ByVal Value As String)
            viewstate("_coderoom") = Value
        End Set
    End Property
    Protected CtrRateAplication1 As ctrRateAplication
    Private Enum dgcolumns
        FechaInicia
        FechaFinaliza
        Precio
        PrecioExtraAdulto
        PrecioExtraNinio
    End Enum


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
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            Room = Request.QueryString("Room")
            CtrRateAplication1.m_iHotelId = Me.cInfoActual.Hotel
            With New RoomFacade
                Dim rooms As RoomsHotelData
                rooms = .getRoomByID(Room)
                If rooms.Tables(rooms.TBL_ROOM_HOTEL).Rows.Count > 0 Then
                    CtrRateAplication1.Adultos = rooms.Tables(rooms.TBL_ROOM_HOTEL).Rows(0).Item(rooms.FLD_NUMBER_MAXADULTS)
                    CtrRateAplication1.Ninios = rooms.Tables(rooms.TBL_ROOM_HOTEL).Rows(0).Item(rooms.FLD_NUMBER_MAXCHILDREN)
                    RoomCode = rooms.Tables(rooms.TBL_ROOM_HOTEL).Rows(0).Item(rooms.FLD_ROOM_CODE).ToString
                End If
            End With
            CtrRateAplication1.filterRac()
            lblError.Visible = False

        End If
    End Sub
    Private Sub loadculture()
        Me.lblTitle.Text = PortalCulture.GetString("00132", True) & " " & RoomCode
        dgRates.Columns(dgcolumns.FechaFinaliza).HeaderText = PortalCulture.GetString("00109")
        dgRates.Columns(dgcolumns.FechaInicia).HeaderText = PortalCulture.GetString("00108")
        dgRates.Columns(dgcolumns.Precio).HeaderText = PortalCulture.GetString("00121")
        dgRates.Columns(dgcolumns.PrecioExtraAdulto).HeaderText = PortalCulture.GetString("00114")
        dgRates.Columns(dgcolumns.PrecioExtraNinio).HeaderText = PortalCulture.GetString("00115")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not Page.IsValid Then Return
        Dim idtarifa As Integer
        Dim sdato As String
        If Room <> 0 Then
            CtrRateAplication1.Exceptions = "NNNNNNN"
            For i As Integer = 1 To CtrRateAplication1.lstDatesCount
                Dim f1, f2 As Date
                f1 = CDate(CtrRateAplication1.lstDatesItemI(i).Split("-")(0))
                f2 = CDate(CtrRateAplication1.lstDatesItemI(i).Split("-")(1))
                CtrRateAplication1.SaveNewFare(Room, idtarifa, f1, f2, "", sdato)
            Next
            'If CtrRateAplication1.SaveNewFare(Room, idtarifa) = False Then
            '    lblError.Visible = True
            'Else
            '    CtrRateAplication1.cleardata()
            '    loadTarifa(idtarifa)
            'End If
        End If
    End Sub

    Private Sub dgRates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRates.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            e.Item.Cells(dgcolumns.FechaFinaliza).Text = CDate(e.Item.Cells(dgcolumns.FechaFinaliza).Text).ToString("MM/dd/yyyy")
            e.Item.Cells(dgcolumns.FechaInicia).Text = CDate(e.Item.Cells(dgcolumns.FechaInicia).Text).ToString("MM/dd/yyyy")
            e.Item.Cells(dgcolumns.Precio).Text = FCurrency(e.Item.Cells(dgcolumns.Precio).Text, 2)
            e.Item.Cells(dgcolumns.PrecioExtraAdulto).Text = FCurrency(e.Item.Cells(dgcolumns.PrecioExtraAdulto).Text, 2)
            e.Item.Cells(dgcolumns.PrecioExtraNinio).Text = FCurrency(e.Item.Cells(dgcolumns.PrecioExtraNinio).Text, 2)
        End If
        dgRates.Columns(dgcolumns.FechaFinaliza).HeaderText = PortalCulture.GetString("00109")
        dgRates.Columns(dgcolumns.FechaInicia).HeaderText = PortalCulture.GetString("00108")
        dgRates.Columns(dgcolumns.Precio).HeaderText = PortalCulture.GetString("00121")
        dgRates.Columns(dgcolumns.PrecioExtraAdulto).HeaderText = PortalCulture.GetString("00114")
        dgRates.Columns(dgcolumns.PrecioExtraNinio).HeaderText = PortalCulture.GetString("00115")
    End Sub
    Private Sub loadTarifa(ByVal idtar As Integer)
        Dim auxcult As String
        auxcult = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Dim Fares As FaresData
        With New FaresSystem
            Fares = .GetFareById(idtar)
        End With
        dgRates.DataSource = Fares
        dgRates.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(auxcult)
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadculture()
        cmdCancel.Text = 1
    End Sub

End Class
