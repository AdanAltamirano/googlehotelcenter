Imports Portal.General.Facade
Imports Portal.General.Common.Data
Partial Class ctlMensajeRuleConf
    Inherits ucBase
    'Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtvalue As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblNotFound As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Enum Tipos
        Prompt  ' Pregunta un yes o no
        Input   ' Permite la entrada de datos
        Information ' Solo un boton de OK
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página        

        'lblNotFound.Text = PortalCulture.GetString("01093")

        lblAdvanceddaysH.Text = PortalCulture.GetString("00119", True)
        lblNoArrivalsH.Text = PortalCulture.GetString("00120", True)
        lblMaxNightsH.Text = PortalCulture.GetString("00118", True)
        lblMinNightsH.Text = PortalCulture.GetString("00117", True)
        lblLunesH.Text = PortalCulture.GetString("00300")
        lblMartesH.Text = PortalCulture.GetString("00301")
        lblMiercolesH.Text = PortalCulture.GetString("00302")
        lblJuevesH.Text = PortalCulture.GetString("00303")
        lblViernesH.Text = PortalCulture.GetString("00304")
        lblSabadoH.Text = PortalCulture.GetString("00305")
        lbldomingoH.Text = PortalCulture.GetString("00306")
        lblPrioridadCHotel.Text = PortalCulture.GetString("00440", True)


        lblAdvanceddaysLH.Text = PortalCulture.GetString("00119", True)
        lblNoArrivalsLH.Text = PortalCulture.GetString("00120", True)
        lblMaxNightsLH.Text = PortalCulture.GetString("00118", True)
        lblMinNightsLH.Text = PortalCulture.GetString("00117", True)
        lblLunesLH.Text = PortalCulture.GetString("00300")
        lblMartesLH.Text = PortalCulture.GetString("00301")
        lblMiercolesLH.Text = PortalCulture.GetString("00302")
        lblJuevesLH.Text = PortalCulture.GetString("00303")
        lblViernesLH.Text = PortalCulture.GetString("00304")
        lblSabadoLH.Text = PortalCulture.GetString("00305")
        lbldomingoLH.Text = PortalCulture.GetString("00306")


        lblAdvanceddaysRP.Text = PortalCulture.GetString("00119", True)
        lblNoArrivalsRP.Text = PortalCulture.GetString("00120", True)
        lblMaxNightsRP.Text = PortalCulture.GetString("00118", True)
        lblMinNightsRP.Text = PortalCulture.GetString("00117", True)
        lblLunesRP.Text = PortalCulture.GetString("00300")
        lblMartesRP.Text = PortalCulture.GetString("00301")
        lblMiercolesRP.Text = PortalCulture.GetString("00302")
        lblJuevesRP.Text = PortalCulture.GetString("00303")
        lblViernesRP.Text = PortalCulture.GetString("00304")
        lblSabadoRP.Text = PortalCulture.GetString("00305")
        lbldomingoRP.Text = PortalCulture.GetString("00306")


        lblAdvanceddaysHRP.Text = PortalCulture.GetString("00119", True)
        lblNoArrivalsHRP.Text = PortalCulture.GetString("00120", True)
        lblMaxNightsHRP.Text = PortalCulture.GetString("00118", True)
        lblMinNightsHRP.Text = PortalCulture.GetString("00117", True)
        lblLunesHRP.Text = PortalCulture.GetString("00300")
        lblMartesHRP.Text = PortalCulture.GetString("00301")
        lblMiercolesHRP.Text = PortalCulture.GetString("00302")
        lblJuevesHRP.Text = PortalCulture.GetString("00303")
        lblViernesHRP.Text = PortalCulture.GetString("00304")
        lblSabadoHRP.Text = PortalCulture.GetString("00305")
        lbldomingoHRP.Text = PortalCulture.GetString("00306")
        lblPrioridadCRPHotel.Text = PortalCulture.GetString("00440", True)
        lblPrioridadCLH.Text = PortalCulture.GetString("00440", True)

        lblPrioridadCRP.Text = PortalCulture.GetString("00440", True)

        btnCancel.Value = PortalCulture.GetString("M000436")

        ddlRatePlanConf.Items.Clear()
        Dim item As ListItem = New ListItem(PortalCulture.GetString("M000272"), "-1")
        ddlRatePlanConf.Items.Add(item)
        ddlRatePlanConf.Attributes.Add("onchange", "javascript:SearchRatePlanRules(3);")

        ddlRatePlanHotel.Items.Clear()
        Dim item2 As ListItem = New ListItem(PortalCulture.GetString("M000272"), "-1")
        ddlRatePlanHotel.Items.Add(item2)
        ddlRatePlanHotel.Attributes.Add("onchange", "javascript:SearchRatePlanRulesHotel(6);")

        lblMsgRatePlanHotel.Text = PortalCulture.GetString("01094")
        lblMsgRateRoom.Text = PortalCulture.GetString("01094")

        lblAdvanceddaysTH.Text = PortalCulture.GetString("00119", True)
        lblNoArrivalsTH.Text = PortalCulture.GetString("00120", True)
        lblMaxNightsTH.Text = PortalCulture.GetString("00118", True)
        lblMinNightsTH.Text = PortalCulture.GetString("00117", True)
        lblLunesTH.Text = PortalCulture.GetString("00300")
        lblMartesTH.Text = PortalCulture.GetString("00301")
        lblMiercolesTH.Text = PortalCulture.GetString("00302")
        lblJuevesTH.Text = PortalCulture.GetString("00303")
        lblViernesTH.Text = PortalCulture.GetString("00304")
        lblSabadoTH.Text = PortalCulture.GetString("00305")
        lbldomingoTH.Text = PortalCulture.GetString("00306")

        lblTituloTarifasHabitacion.Text = PortalCulture.GetString("01095")
        LabelRooms.Text = PortalCulture.GetString("00170")
        LabelRatePlans.Text = PortalCulture.GetString("00016")
        ButtonLoadTarifasHabitacion.Value = PortalCulture.GetString("00149")

        ButtonLoadTarifasHabitacion.Attributes.Add("onclick", "javascript:SearchRoomRate(7); return false;")

        lblMsgLH.Text = PortalCulture.GetString("01097")
        lblMsgRP.Text = PortalCulture.GetString("01096")

        lblLHotel.Text = PortalCulture.GetString("01098")
        Label5.Text = PortalCulture.GetString("01098")

        Label2.Text = PortalCulture.GetString("01099")
        Label4.Text = PortalCulture.GetString("01099")
        btnCancel.Attributes.Add("onclick", getcancel)
    End Sub
    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

        'Response.Write("<script>var ClientID='" & Me.ClientID & "' </script>")
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetVarClientID", String.Format("var ClientID='{0}';", Me.ClientID), True)
    End Sub

    Private ReadOnly Property getcancel() As String
        Get
            Return "cancelRule('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "')"
        End Get
    End Property

    Public Function getShow(ByVal obj As String, ByVal title As String, ByVal question As String) As String
        lblTitle.Text = title
        lblPrompt.Text = question
        btnCancel.Value = PortalCulture.GetString("M000436")
        btnCancel.Attributes.Add("onclick", getcancel)
        Dim hideCmbs As New System.Text.StringBuilder
        'Return "javascript:show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "','" & obj & "')"
        Return "showRule('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "','" & obj & "')"
    End Function

    Public Sub LoadRules(ByVal fecha As String, ByVal idHotel As Integer)
        LoadHotelGeneralRules(fecha, idHotel)
    End Sub

    Private Sub LoadHotelGeneralRules(ByVal fecha As String, ByVal idHotel As Integer)
        Dim dsHotel As HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(idHotel)
        End With
        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                'Noches Minimas
                Me.txtMinNightsH.Text = "0"
                Me.txtMinNightsH.Text = IIf(.IsNull(dsHotel.FIELD_MINLENGTHSTAY), 1, .Item(dsHotel.FIELD_MINLENGTHSTAY))
                If Me.txtMinNightsH.Text = "0" Then
                    Me.txtMinNightsH.Text = 1
                End If
                'Noches Maximas
                txtMaxNightsH.Text = .Item(dsHotel.FIELD_MAXDIASRENTA)
                'Advance booking days
                txtAdvancedDaysH.Text = .Item(dsHotel.FIELD_DIASLIBRES)
                'No Arrivals

                Dim Departure As String
                Dim aux As Char
                Departure = IIf(.IsNull(dsHotel.FIELD_NOARRIVALS), "NNNNNNN", .Item(dsHotel.FIELD_NOARRIVALS))
                aux = Departure.Chars(0)
                If aux = "Y" Then
                    Chk1H.Checked = True
                Else
                    Chk1H.Checked = False
                End If
                aux = Departure.Chars(1)
                If aux = "Y" Then
                    Chk2H.Checked = True
                Else
                    Chk2H.Checked = False
                End If
                aux = Departure.Chars(2)
                If aux = "Y" Then
                    Chk3H.Checked = True
                Else
                    Chk3H.Checked = False
                End If
                aux = Departure.Chars(3)
                If aux = "Y" Then
                    Chk4H.Checked = True
                Else
                    Chk4H.Checked = False
                End If
                aux = Departure.Chars(4)
                If aux = "Y" Then
                    Chk5H.Checked = True
                Else
                    Chk5H.Checked = False
                End If
                aux = Departure.Chars(5)
                If aux = "Y" Then
                    Chk6H.Checked = True
                Else
                    Chk6H.Checked = False
                End If
                aux = Departure.Chars(6)
                If aux = "Y" Then
                    Chk7H.Checked = True
                Else
                    Chk7H.Checked = False
                End If
            End With
        End If
        'LockGral a nivel Hotel
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        btnCancel.Value = PortalCulture.GetString("M000436")
        lblMsgRatePlanHotel.Attributes.Add("display", "none")
        lblMsgRateRoom.Attributes.Add("display", "none")

        lblCancelacionHRP.Attributes.Add("style", "DISPLAY: none;")
        TextboxCHRP.Attributes.Add("style", "DISPLAY: none;")
        lblMsgCHRP.Attributes.Add("style", "DISPLAY: none;")
        btnCancel.Attributes.Add("onclick", getcancel)
    End Sub
    Public Sub loadRooms(ByVal idhotel As Integer)
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        Dim RatesPlan As Portal.General.Common.Data.RatePlanData

        With New Portal.Hotel.Facade.RoomFacade
            Rooms = .getRooms(idhotel, PortalCulture.GetIDCulture)
        End With
        Rooms.Tables(Rooms.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & Rooms.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & Rooms.FLD_NOMBRE & ",1,25)")

        ddlRooms.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
        ddlRooms.DataValueField = Rooms.FLD_ID_ROOM_HOTEL
        ddlRooms.DataSource = Rooms
        ddlRooms.DataBind()
        ddlRooms.Items.Insert(0, PortalCulture.GetString("M000272"))
        ddlRooms.Items(0).Value = "-1"
    End Sub
    Public Sub loadRatePlans(ByVal idHotel As Integer)
        Dim ds As RatePlanData
        Dim incluirPaquetesSegmentoK As Integer = 1
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(idHotel, PortalCulture.GetIDCulture, incluirPaquetesSegmentoK, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With

        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(idHotel, PortalCulture.GetIDCulture)
        End With
        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & ds.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & ds.FIELD_NAME & ",1,25)")
        ''eliminar los ratesplan que ya tienen links
        Dim dv As DataView
        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = links.FIELD_TargetRatePlan & "='" & r(ds.FIELD_IDRATEPLAN) & "'"
            ';If dv.Count > 0 Then 'OrElse r(ds.FIELD_SEGMENT) = "K" Then
            If incluirPaquetesSegmentoK = 0 Then
                If dv.Count > 0 Or r(ds.FIELD_SEGMENT) = "K" Then
                    r.Delete()
                End If
            Else
                If dv.Count > 0 Then
                    r.Delete()
                End If
            End If
        Next
        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()
        ddlRatePlansRooms.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
        ddlRatePlansRooms.DataValueField = ds.FIELD_IDRATEPLAN
        ddlRatePlansRooms.DataSource = ds
        ddlRatePlansRooms.DataBind()
        ddlRatePlansRooms.Items.Insert(0, PortalCulture.GetString("M000272"))
        ddlRatePlansRooms.Items(0).Value = "-1"
    End Sub



  
End Class
