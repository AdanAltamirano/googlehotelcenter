Imports Microsoft.VisualBasic
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.Hotel.Facade

Imports System.Text
Imports System.IO
Imports APIServices.Xml.Soap

Partial Class HomePage
    Inherits PaginaBase
    Public Property SelectedMes() As Integer
        Get
            Return ViewState("SelectedMes")
        End Get
        Set(ByVal Value As Integer)
            ViewState("SelectedMes") = Value
        End Set
    End Property

    Public Property SelectedYear() As String
        Get
            Return ViewState("SelectedYear")
        End Get
        Set(ByVal Value As String)
            ViewState("SelectedYear") = Value
        End Set
    End Property
    Private Property selectedRoomCode() As String
        Get
            Return ViewState("selectedRoomCode")
        End Get
        Set(ByVal Value As String)
            ViewState("selectedRoomCode") = Value
        End Set
    End Property

    Private Property selectedRoom() As Integer
        Get
            Return ViewState("selectedRoom")
        End Get
        Set(ByVal Value As Integer)
            ViewState("selectedRoom") = Value
        End Set
    End Property
    Private Property RackRate() As String
        Get
            Return ViewState("_RackRate")
        End Get
        Set(ByVal Value As String)
            ViewState("_RackRate") = Value
        End Set
    End Property
    Private Property StatusHotel() As String
        Get
            Return ViewState("_SH")
        End Get
        Set(ByVal Value As String)
            ViewState("_SH") = Value
        End Set
    End Property
    'Public Property SourceName() As String
    '    Get
    '        Return viewstate("_SN")
    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SN") = Value
    '    End Set
    'End Property


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dlstRooms As System.Web.UI.WebControls.DataList
    Protected WithEvents lblRAC As System.Web.UI.WebControls.Label
    Protected WithEvents lblCPT As System.Web.UI.WebControls.Label
    Protected WithEvents lblGOV As System.Web.UI.WebControls.Label
    Protected WithEvents lblMLY As System.Web.UI.WebControls.Label
    Protected WithEvents lblSCZ As System.Web.UI.WebControls.Label
    Protected WithEvents lblSPL As System.Web.UI.WebControls.Label
    Protected WithEvents lblWKD As System.Web.UI.WebControls.Label
    Protected WithEvents lblPKG As System.Web.UI.WebControls.Label
    Protected WithEvents lblFMP As System.Web.UI.WebControls.Label
    Protected WithEvents lblASN As System.Web.UI.WebControls.Label
    Protected WithEvents lblTOR As System.Web.UI.WebControls.Label
    Protected WithEvents lblCVN As System.Web.UI.WebControls.Label
    Protected WithEvents lblTIC As System.Web.UI.WebControls.Label
    Protected WithEvents lblNET As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgRAC As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgCPT As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgGOV As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgMLY As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgSCZ As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgSPL As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgWKD As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgPKG As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgFMP As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgASN As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgTOR As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgCVN As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgTIC As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgNET As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextBox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImageButton2 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents txtFecha As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdbyes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdbNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label


    Protected WithEvents lblAllSold As System.Web.UI.WebControls.Label
    Protected WithEvents hplSave As System.Web.UI.WebControls.Button

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Protected CtlMensajes1 As ctlMensajes

    Public Function getFunctionShow() As String
        Return "javascript:var e=document.getElementById('" & Me.TxtRooms.ClientID & "'); if (eval(e.value)==0) { " &
             CtlMensajes1.getShow(Me.btnSaveIntervals.ClientID, "", PortalCulture.GetString("00607")) &
                 "}else{var o;o=document.getElementById('" & Me.btnSaveIntervals.ClientID & "'); o.click();}  "

    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim idAsoc As Integer = Me.GetIdAsociation

        Me.hplOcultar.NavigateUrl = "javascript:Ocultar('0');"
        hplShow.NavigateUrl = "javascript:Ocultar('1');"
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            Me.txtInicio.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
            Me.txtDivVisible.Text = "1"

            Me.cvMissingRoomTypes.IsValid = True
            Dim ds As RatePlanData
            CargaFechas()
            Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
            loadTypes()
            With New RatePlanFacade
                ds = .GetRatePlanByIdHotel(cInfoActual.Hotel, idAsociacion:=idAsoc)
                Dim dv As DataView = ds.Tables(ds.RATEPLAN_TABLE).DefaultView
                dv.RowFilter = ds.FIELD_SegmentRacPrinc & "= true"
                If dv.Count > 0 Then
                    RackRate = dv(0)(ds.FIELD_IDRATEPLAN)
                End If
            End With
            'Me.ddlRoomtype.Attributes.Add("onChange", "javascript:showRoomType('" & Me.ddlRoomtype.ClientID & "','" & Me.SourceName & "')")
        End If

        'hplSave.NavigateUrl = "javascript:;"
        hplSubmit.NavigateUrl = GeRequestApplicationPath("/Pages/HomePage.aspx")
        Me.ResizefrmPrincipal()
        Dim sfction As String = getFunctionShow()
        'btnSave.OnClientClick = "return AlertMessage(); "
        Me.btnSaveIntervals.OnClientClick = "return AlertMessage(); "
        cmdShowInventory.OnClientClick = "return FireUpdateStatus();"
    End Sub
    Private Function resizeframe() As String
        Return ""
    End Function


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadDatos()
        loadculture()
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.InventoryRooms, Me.btnSave, "M")
        ' CType(Me.Page, PaginaBase).Habilitaboton(permisos.InventoryRooms, Me.btnSaveIntervals, "M")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.InventoryRooms, Me.cmdShowInventory, "R")
        If Me.txtDivVisible.Text = "1" Then
            hplShow.Style.Add("display", "block")
            hplOcultar.Style.Add("display", "none")
            Div1.Style.Add("display", "none")
        Else
            hplShow.Style.Add("display", "none")
            hplOcultar.Style.Add("display", "block")
            Div1.Style.Add("display", "block")
        End If

        'If cInfoActual.IsSingleImgInv AndAlso IsSupervisor Then
        btnSingleImgInv.Visible = True
        'End If
        'If SourceName <> "" Then
        '    lblRoomName.Text = Me.SourceName.Split("//")(2 * ddlRoomtype.SelectedIndex)
        'End If

        'If lblRoomName.Text = "" Then
        '    lblRoomName.Text = "-"
        'End If
        'Me.ResizefrmPrincipal()
    End Sub

    Private Sub loadculture()
        lblMsg.Text = PortalCulture.GetString("00169")
        Me.lblTitle.Text = PortalCulture.GetString("00169")
        lblRoomType.Text = PortalCulture.GetString("00072")
        cvMissingRoomTypes.ErrorMessage = PortalCulture.GetString("00138")
        cmdShowInventory.Text = PortalCulture.GetString("00139")
        Me.lblTo.Text = PortalCulture.GetString("00109", True)
        lbltitleintervals.Text = PortalCulture.GetString("00140")
        lblRooms.Text = PortalCulture.GetString("00141", True)
        btnSaveIntervals.Text = PortalCulture.GetString("00168")

        Me.LblLun.Text = PortalCulture.GetString("00300")
        Me.lblmar.Text = PortalCulture.GetString("00301")
        Me.lblmie.Text = PortalCulture.GetString("00302")
        Me.lbljue.Text = PortalCulture.GetString("00303")
        Me.lblvie.Text = PortalCulture.GetString("00304")
        Me.lblsab.Text = PortalCulture.GetString("00305")
        Me.LblDom.Text = PortalCulture.GetString("00306")

        'lblHotel.Text = PortalCulture.GetString("00153", True)
        'btnClose.Text = PortalCulture.GetString("00167")

        Me.btnSave.Text = PortalCulture.GetString("00008")

        Me.hplShow.Text = PortalCulture.GetString("00165")
        Me.hplOcultar.Text = PortalCulture.GetString("00166")
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.lblDomingo.Text = WeekdayName(1, False, FirstDayOfWeek.Sunday)
        Me.lblLunes.Text = WeekdayName(2, False, FirstDayOfWeek.Sunday)
        Me.lblMartes.Text = WeekdayName(3, False, FirstDayOfWeek.Sunday)
        Me.lblMiercoles.Text = WeekdayName(4, False, FirstDayOfWeek.Sunday)
        Me.lblJueves.Text = WeekdayName(5, False, FirstDayOfWeek.Sunday)
        Me.lblViernes.Text = WeekdayName(6, False, FirstDayOfWeek.Sunday)
        Me.lblSabado.Text = WeekdayName(7, False, FirstDayOfWeek.Sunday)
        Me.lbltitletable.Text = MonthName(SelectedMes + 1) & " " & SelectedYear ' PortalCulture.GetString("")
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
        Me.lblTitle.Text = PortalCulture.GetString("00169", True)

        If ddlRoomtype.Items.Count > 0 Then
            Me.lblTitle.Text &= " " & Me.ddlRoomtype.SelectedItem.Text
        End If

        Me.lblFrom.Text = PortalCulture.GetString("00108", True)
        Me.lblNoArrivals.Text = PortalCulture.GetString("00152") & " (N)"
        Me.lblOpen.Text = PortalCulture.GetString("00150") & " (O)"
        Me.lblClose.Text = PortalCulture.GetString("00151") & " (C)"
        lblEstatus.Text = PortalCulture.GetString("00283")
        lblRackRate.Text = PortalCulture.GetString("00284")
        lblAvailRes.Text = PortalCulture.GetString("00285")

        'lblCodelowestRAte.Text = PortalCulture.GetString("00286")

        lbllowestRate.Text = PortalCulture.GetString("00287")
        lblDayOfMonth.Text = PortalCulture.GetString("00288")
        Me.lbltitleDate.Text = PortalCulture.GetString("00313")
        lblMonth.Text = PortalCulture.GetString("00311")
        lblYear.Text = PortalCulture.GetString("00312")
        rvRooms.Text = PortalCulture.GetString("00146")
        valTotalRooms.Text = PortalCulture.GetString("01395")
        lblIsHouse.Text = PortalCulture.GetString("01658")
        'chkOnRequest.Text = PortalCulture.GetString("00314")

        If (cInfoActual.IsHouse) Then
            lblIsHouse.Visible = True
        End If

    End Sub

    Private Sub CargaFechas()
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        ddlMonth.Items.Clear()
        For i As Integer = 1 To 12
            ddlMonth.Items.Add(MonthName(i, True))
        Next
        Me.ddlMonth.SelectedIndex = Now.Month - 1
        Me.SelectedMes = Me.ddlMonth.SelectedIndex
        For i As Integer = Now.Year - 1 To Now.Year + 3
            ddlyear.Items.Add(New ListItem(i, i))
        Next
        ddlyear.SelectedValue = Now.Year
        Me.SelectedYear = ddlyear.SelectedValue
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Function loadTypes() As Boolean
        ddlRoomtype.Items.Clear()
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRooms(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture)

            room.Tables(room.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "SUBSTRING(" & room.FLD_ROOM_CODE & "+ ' ' + '-' + ' ' +" & room.FLD_NOMBRE & ", 1, 25)")
            ddlRoomtype.DataSource = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            ddlRoomtype.DataTextField = "texto" 'RoomsHotelData.FLD_NOMBRE
            ddlRoomtype.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
            ddlRoomtype.DataBind()
        End With
        'SourceName = ""
        'For i As Integer = 0 To room.Tables(room.TBL_ROOM_HOTEL).Rows.Count - 1
        '    Me.SourceName &= "//" & room.Tables(room.TBL_ROOM_HOTEL).Rows(i).Item(room.FLD_NOMBRE).ToString
        'Next

        If ddlRoomtype.Items.Count = 0 Then
            Me.cvMissingRoomTypes.IsValid = False
            Return False
        Else
            Dim i As ListItem = New ListItem
            i.Text = PortalCulture.GetString("00144")
            i.Value = 0
            ddlRoomtype.Items.Insert(0, i)
            Return True
        End If
    End Function

    Private Sub loadDatos()
        SelectedMes = ddlMonth.SelectedIndex
        SelectedYear = ddlyear.SelectedValue
        Dim d1 As Date = New Date(SelectedYear, SelectedMes + 1, 1)

        If Me.ddlRoomtype.Items.Count > 0 Then
            selectedRoom = Me.ddlRoomtype.SelectedValue
            selectedRoomCode = Me.ddlRoomtype.SelectedItem.Text
        End If


        Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
        Dim EndDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), Date.DaysInMonth(SelectedYear, SelectedMes + 1))
        Dim FirstDay As Integer = Weekday(selectDate, FirstDayOfWeek.Sunday)
        Dim data As RoomsInventoryData
        Dim ctrl As Control, strerror As String
        Dim lockgral As clsCommonAvailibilityGral, dsRates As DataSet, dsRateRack As DataSet

        Dim cHotel As HotelDatos

        Me.StatusHotel = "O"
        With New HotelSistema
            cHotel = (.GetHotelById(Me.cInfoActual.Hotel))
        End With

        If cHotel.Tables(cHotel.HOTEL_TABLE).Rows.Count > 0 AndAlso Not cHotel.Tables(cHotel.HOTEL_TABLE).Rows(0).Item(cHotel.FIELD_STATUSAVAILABILITY) Is System.DBNull.Value Then
            Me.StatusHotel = cHotel.Tables(cHotel.HOTEL_TABLE).Rows(0).Item(cHotel.FIELD_STATUSAVAILABILITY)
        End If
        'cambie la funcion 
        With New ClsFacadeAvailibilityGral
            lockgral = .LockGralGetByDay(Me.cInfoActual.Hotel, selectDate, EndDate, strerror)
        End With

        If selectedRoom = 0 Then
            With New RoomsInventoryFacade
                data = .getAllRoomsInventoryByDate_Data(Me.cInfoActual.Hotel, selectDate, EndDate)
            End With
            With New FaresSystem
                dsRates = .GetLowestFarebyDate(selectDate, EndDate, cInfoActual.Hotel)
                If RackRate <> "" Then
                    dsRateRack = .GetLowestFarebyDate(selectDate, EndDate, cInfoActual.Hotel, RackRate)
                End If
            End With

        Else
            With New RoomsInventoryFacade
                data = .getInventoryByDate_Data(selectedRoom, selectDate, EndDate)
            End With
            With New FaresSystem
                dsRates = .GetLowestFarebyDate(selectDate, EndDate, cInfoActual.Hotel, selectedRoom)
                If RackRate <> "" Then
                    dsRateRack = .GetLowestFarebyDate(selectDate, EndDate, cInfoActual.Hotel, RackRate, selectedRoom)
                End If
            End With
        End If

        For i As Integer = 1 To FirstDay - 1
            ctrl = Me.FindControl("ctrlHP" & (i).ToString)
            CType(ctrl, ctrlHP).Show(False)
        Next
        For i As Integer = Date.DaysInMonth(SelectedYear, CInt(SelectedMes + 1)) + 1 To 42
            ctrl = Me.FindControl("ctrlHP" & (i).ToString)
            CType(ctrl, ctrlHP).Show(False)
        Next

        For i As Integer = 0 To Date.DaysInMonth(SelectedYear, CInt(SelectedMes + 1)) - 1
            ctrl = Me.FindControl("ctrlHP" & (i + FirstDay).ToString)
            CType(ctrl, ctrlHP).Show(True)
            CType(ctrl, ctrlHP).Day = i + 1

            If (selectDate.AddDays(i).ToString("yyyy/MM/dd") < Now.Date.ToString("yyyy/MM/dd")) Or Me.cInfoActual.UserPerfil = PaginaBase.PerfilHotel.Basico Then
                CType(ctrl, ctrlHP).Editable = False
            Else
                CType(ctrl, ctrlHP).Editable = True
            End If

            For Each dr As DataRow In data.Tables(data.TBL_ROOMS_INVENTORY).Rows
                If CDate(dr(data.FLD_DATE)).ToString("yyyy/MM/dd") = selectDate.AddDays(i).ToString("yyyy/MM/dd") Then
                    If selectedRoom = 0 Then
                        CType(ctrl, ctrlHP).ShowLink() = False
                        CType(ctrl, ctrlHP).loaddatos(dr(data.FLD_NUMBER_RESERVATIONS) & " / " & dr(data.FLD_LOCKQUANTITY) & " / " & dr(data.FLD_NUMBER_AVAILABILITY))
                    Else
                        CType(ctrl, ctrlHP).ShowLink() = True
                        CType(ctrl, ctrlHP).loaddatos(dr(data.FLD_NUMBER_RESERVATIONS) & " / " & dr(data.FLD_LOCKQUANTITY), dr(data.FLD_NUMBER_AVAILABILITY))
                    End If
                    Exit For
                End If
            Next

            CType(ctrl, ctrlHP).status = System.Drawing.Color.Black
            If Me.StatusHotel.ToUpper = "O" Then
                CType(ctrl, ctrlHP).status = System.Drawing.Color.Green
            ElseIf Me.StatusHotel.ToUpper = "C" Then
                CType(ctrl, ctrlHP).status = System.Drawing.Color.Red
            ElseIf Me.StatusHotel.ToUpper = "N" Then
                CType(ctrl, ctrlHP).status = System.Drawing.Color.BlueViolet
            End If

            If Me.StatusHotel.ToUpper <> "C" And Me.StatusHotel.ToUpper <> "N" Then
                For Each dr As DataRow In lockgral.Tables(lockgral.TABLE_LockGral).Rows
                    If CDate(dr(lockgral.FIELD_StartDate)).ToString("yyyy/MM/dd") <= selectDate.AddDays(i).ToString("yyyy/MM/dd") AndAlso CDate(dr(lockgral.FIELD_EndDate)).ToString("yyyy/MM/dd") >= selectDate.AddDays(i).ToString("yyyy/MM/dd") Then
                        If dr(lockgral.FIELD_AplyWeek) Is System.DBNull.Value OrElse (dr(lockgral.FIELD_AplyWeek).ToString.Length = 7 AndAlso dr(lockgral.FIELD_AplyWeek).ToString.ToUpper.Substring(Weekday(selectDate.AddDays(i), FirstDayOfWeek.Monday) - 1, 1) = "Y") Then
                            If dr(lockgral.FIELD_statusAvailability).ToString.ToUpper = "C" Then
                                CType(ctrl, ctrlHP).status = System.Drawing.Color.Red
                            ElseIf dr(lockgral.FIELD_statusAvailability).ToString.ToUpper = "O" Then
                                CType(ctrl, ctrlHP).status = System.Drawing.Color.Green
                            ElseIf dr(lockgral.FIELD_statusAvailability).ToString.ToUpper = "N" Then
                                CType(ctrl, ctrlHP).status = System.Drawing.Color.BlueViolet
                            End If
                        End If
                        Exit For
                    End If
                Next
            End If

            CType(ctrl, ctrlHP).loadRate("NA", "-")
            If Not dsRates Is Nothing AndAlso dsRates.Tables.Count > 0 Then
                With dsRates.Tables(0)
                    Dim dvRates As DataView
                    dvRates = .DefaultView
                    dvRates.RowFilter = data.FLD_DATE & " ='" & selectDate.AddDays(i) & "'"
                    If dvRates.Count > 0 Then
                        Dim dr As DataRowView
                        dr = dvRates(0)
                        For Each d As DataRowView In dvRates
                            If Me.IsSupervisor Then
                                If Not d("tar") Is System.DBNull.Value AndAlso CInt(d("tar")) < dr("tar") Then
                                    dr = d
                                End If
                            Else
                                If Not dr("tarNr") Is System.DBNull.Value Then
                                    If (Not d("tarNr") Is System.DBNull.Value) AndAlso CInt(d("tarNr")) < dr("tarNr") Then
                                        dr = d
                                    End If
                                Else
                                    If Not d("tar") Is System.DBNull.Value AndAlso CInt(d("tar")) < dr("tar") Then
                                        dr = d
                                    End If
                                End If
                            End If

                            'If Not d("tar") Is System.DBNull.Value AndAlso CInt(d("tar")) < dr("tar") Then
                            '    dr = d
                            'End If

                        Next

                        'If Not dr("tar") Is System.DBNull.Value Then
                        '    CType(ctrl, ctrlHP).loadRate(FCurrency(dr("tar"), 2), dr("codigohabitacion") & dr("codigotarifa"))
                        'End If
                        If Me.IsSupervisor Then
                            If Not dr("tar") Is System.DBNull.Value Then
                                CType(ctrl, ctrlHP).loadRate(FCurrency(dr("tar"), 2), dr("codigohabitacion") & dr("codigotarifa"))
                            End If
                        Else
                            If Not dr("tarNr") Is System.DBNull.Value Then
                                CType(ctrl, ctrlHP).loadRate(FCurrency(dr("tarNr"), 2), dr("codigohabitacion") & dr("codigotarifa"))
                            ElseIf Not dr("tar") Is System.DBNull.Value Then
                                CType(ctrl, ctrlHP).loadRate(FCurrency(dr("tar"), 2), dr("codigohabitacion") & dr("codigotarifa"))
                            End If

                        End If

                    End If
                End With
            End If

            CType(ctrl, ctrlHP).loadRackRate("NA")
            If RackRate <> "" AndAlso Not dsRateRack Is Nothing AndAlso dsRateRack.Tables.Count > 0 Then
                With dsRateRack.Tables(0)
                    Dim dvRates As DataView
                    dvRates = .DefaultView
                    dvRates.RowFilter = data.FLD_DATE & " ='" & selectDate.AddDays(i) & "'"
                    If dvRates.Count > 0 Then
                        Dim dr As DataRowView
                        dr = dvRates(0)
                        For Each d As DataRowView In dvRates

                            If Me.IsSupervisor Then
                                If Not d("tar") Is System.DBNull.Value AndAlso CInt(d("tar")) < dr("tar") Then
                                    dr = d
                                End If
                            Else
                                If Not dr("tarNr") Is System.DBNull.Value Then
                                    If (Not d("tarNr") Is System.DBNull.Value) AndAlso CInt(d("tarNr")) < dr("tarNr") Then
                                        dr = d
                                    End If
                                Else
                                    If Not d("tar") Is System.DBNull.Value AndAlso CInt(d("tar")) < dr("tar") Then
                                        dr = d
                                    End If
                                End If
                            End If
                            'If Not d("tar") Is System.DBNull.Value AndAlso CInt(d("tar")) < dr("tar") Then
                            '    dr = d
                            'End If
                        Next
                        If Me.IsSupervisor Then
                            If Not dr("tar") Is System.DBNull.Value Then
                                CType(ctrl, ctrlHP).loadRackRate(FCurrency(dr("tar"), 2))
                            End If
                        Else
                            If Not dr("tarNr") Is System.DBNull.Value Then
                                CType(ctrl, ctrlHP).loadRackRate(FCurrency(dr("tarNr"), 2))
                            ElseIf Not dr("tar") Is System.DBNull.Value Then
                                CType(ctrl, ctrlHP).loadRackRate(FCurrency(dr("tar"), 2))
                            End If

                        End If

                        'If Not dr("tar") Is System.DBNull.Value Then
                        '    CType(ctrl, ctrlHP).loadRackRate(FCurrency(dr("tar"), 2))
                        'End If
                    End If
                End With
            End If

        Next
    End Sub


    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not InvalidDate() Then
    '        closehotel()
    '    End If

    'End Sub
    'Private Function closehotel() As Boolean
    '    Dim ds As clsCommonAvailibilityGral
    '    Dim strError As String
    '    With New ClsFacadeAvailibilityGral
    '        ds = .LockGralBuscarTransaccion(Me.cInfoActual.Hotel, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), strError)
    '    End With
    '    Dim dstrans, Data As clsCommonAvailibilityGral
    '    dstrans = New clsCommonAvailibilityGral
    '    If Not ds Is Nothing AndAlso ds.Tables(ds.TABLE_LockGral).Rows.Count > 0 Then
    '        For i As Integer = 0 To ds.Tables(ds.TABLE_LockGral).Rows.Count - 1
    '            With ds.Tables(ds.TABLE_LockGral)
    '                copyrow(.Rows(i), .Rows(i).Item(ds.FIELD_StartDate), .Rows(i).Item(ds.FIELD_EndDate), dstrans, True)
    '                If .Rows(i).Item(ds.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
    '                    'YA ESTÁ ELIMINADA
    '                ElseIf .Rows(i).Item(ds.FIELD_StartDate) < CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
    '                    copyrow(.Rows(i), .Rows(i).Item(ds.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
    '                ElseIf .Rows(i).Item(ds.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) > CDate(Me.txtFinal.Text) Then
    '                    copyrow(.Rows(i), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(ds.FIELD_EndDate), dstrans, False)
    '                Else
    '                    copyrow(.Rows(i), ds.Tables(ds.TABLE_LockGral).Rows(i).Item(ds.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
    '                    copyrow(.Rows(i), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(ds.FIELD_EndDate), dstrans, False)
    '                End If
    '            End With
    '        Next
    '    End If
    '    Data = FillData()
    '    If Not Data Is Nothing AndAlso Data.Tables(Data.TABLE_LockGral).Rows.Count > 0 Then
    '        copyrow(Data.Tables(Data.TABLE_LockGral).Rows(0), CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), dstrans, False)
    '    End If
    '    With New ClsFacadeAvailibilityGral
    '        If Not dstrans Is Nothing AndAlso dstrans.Tables(dstrans.TABLE_LockGral).Rows.Count > 0 Then
    '            .updateLocks(dstrans)
    '        End If
    '    End With
    'End Function


    Private Function copyrow(ByVal row As DataRow, ByVal fechainicio As DateTime, ByVal fechafinal As DateTime, ByVal cAva As clsCommonAvailibilityGral, ByVal borrado As Boolean)
        Dim newrow As DataRow
        newrow = cAva.Tables(cAva.TABLE_LockGral).NewRow
        With newrow
            .Item(cAva.FIELD_idHotel) = row.Item(cAva.FIELD_idHotel)
            .Item(cAva.FIELD_StartDate) = fechainicio
            .Item(cAva.FIELD_EndDate) = fechafinal
            .Item(cAva.FIELD_minLengthStay) = row.Item(cAva.FIELD_minLengthStay)
            .Item(cAva.FIELD_RoomsAvailable) = row.Item(cAva.FIELD_RoomsAvailable)
            .Item(cAva.FIELD_statusAvailability) = row.Item(cAva.FIELD_statusAvailability).ToString.ToUpper
            .Item(cAva.FIELD_HurdleRate) = row.Item(cAva.FIELD_HurdleRate)
            .Item(cAva.FIELD_CancelPrioridad) = row.Item(cAva.FIELD_CancelPrioridad)
            .Item(cAva.FIELD_DepartureRestricted) = row.Item(cAva.FIELD_DepartureRestricted)
            .Item(cAva.FIELD_OnRequest) = row.Item(cAva.FIELD_OnRequest)
            .Item(cAva.FIELD_AplyWeek) = row.Item(cAva.FIELD_AplyWeek)
            cAva.Tables(cAva.TABLE_LockGral).Rows.Add(newrow)
            If borrado = True Then
                newrow.AcceptChanges()
                newrow.Delete()
            End If
        End With
    End Function

    Function Nota(ByVal roomCode As String, ByVal sRooms As String, ByVal days As String, ByRef sreference As String) As String
        Dim msg As String = "La habitacion " & roomCode & " cambió su inventario a " & sRooms & " cuartos para el día " & days
        Dim drhotel As DataRow = Me.HotelInfo
        Dim idioma As String

        sreference = "Cambio de disponibilidad"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            sreference = "Update availability"
            msg = "Room " & roomCode & " changed its inventory to " & sRooms & " the day " & days
        End If
        Return msg
    End Function

    Function Nota2(ByVal roomCode As String, ByVal srooms As String, ByVal begindate As String, ByVal enddate As String, ByVal diasExc As String, ByRef sreference As String) As String
        Dim msg As String = "La habitacion " & roomCode & " cambió su inventario a " & srooms & " cuartos del " & begindate & " al " & enddate & " para los dias " & diasExc
        Dim drhotel As DataRow = Me.HotelInfo
        Dim idioma As String

        sreference = ""
        sreference = "Cambio de disponibilidad"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            sreference = "Update availability"
            msg = "Room " & roomCode & " changed its inventory to " & srooms & " from " & begindate & " to " & enddate & " for the days " & diasExc
        End If
        Return msg
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If Not Page.IsValid Then Return
        Dim ctrl As Control
        Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
        Dim EndDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), Date.DaysInMonth(SelectedYear, SelectedMes + 1))
        Dim FirstDay As Integer = Weekday(selectDate, FirstDayOfWeek.Sunday)
        Dim sdato As String
        Dim sdatoDespues As String
        Dim sdatoCorreo As String
        Dim dsBefore As New RoomsInventoryData
        Dim dsTrans As New RoomsInventoryData


        If Me.selectedRoom <> 0 Then
            For i As Integer = 0 To Date.DaysInMonth(CInt(SelectedYear), CInt(SelectedMes + 1))
                ctrl = Me.FindControl("CtrlHP" & (i + FirstDay).ToString)
                If CType(ctrl, ctrlHP).change = "1" Then
                    If IsNumeric(CType(ctrl, ctrlHP).rooms()) Then
                        If SaveRoom(Me.selectedRoom, selectDate.AddDays(i), selectDate.AddDays(i), CType(ctrl, ctrlHP).rooms(), dsBefore, dsTrans) Then
                            Try
                                With New ClsRuWS
                                    .CallRUWS(cInfoActual.Hotel, Me.selectedRoom, selectDate.AddDays(i), selectDate.AddDays(i))
                                End With
                            Catch ex As Exception

                            End Try

                            AddUpdateInventoryByInterval(selectedRoom, dsBefore)
                            sdato = Util.Utility.GetXml(dsTrans.TBL_ROOMS_INVENTORY, "UpdateInventory", dsBefore)

                            AddUpdateInventoryByInterval(selectedRoom, dsTrans)
                            sdatoDespues = Util.Utility.GetXml(dsTrans.TBL_ROOMS_INVENTORY, "UpdateInventory", dsTrans)
                            'sdatoDespues = dsTrans.GetXml.ToString

                            'sdatoCorreo = CreateAvailHtml(Me.ddlRoomtype.SelectedItem.Text, dsBefore, dsTrans, selectedRoom)
                            sdatoCorreo = (New Util.Utility).GeneraCorreoXslt(sdato, sdatoDespues)
                            Dim snota As String
                            Dim sreference As String = ""
                            snota = Nota(Me.selectedRoomCode, CType(ctrl, ctrlHP).rooms(), selectDate.AddDays(i), sreference)
                            Me.guardalog("/Pages/HomePage.aspx", PaginaBase.acciones.Modificar, snota, sreference, sdato, sdatoDespues, sdatoCorreo)
                            'Me.guardalog("/Pages/HomePage.aspx", PaginaBase.acciones.Modificar, "La habitacion " & Me.selectedRoomCode & " cambió su inventario a " & CType(ctrl, ctrlHP).rooms() & " cuartos para el día " & selectDate.AddDays(i), "Update availability", sdato, sdatoDespues, sdatoCorreo)
                        End If
                        CType(ctrl, ctrlHP).change = ""
                    End If
                End If
            Next
        Else
            Dim ds As RoomsInventoryData = New RoomsInventoryData
            For i As Integer = 0 To Date.DaysInMonth(CInt(SelectedYear), CInt(SelectedMes + 1))
                ctrl = Me.FindControl("CtrlHP" & (i + FirstDay).ToString)
                If CType(ctrl, ctrlHP).change = "1" Then
                    If IsNumeric(CType(ctrl, ctrlHP).rooms()) Then
                        saveRowRooms(ds, selectDate.AddDays(i), selectDate.AddDays(i), CType(ctrl, ctrlHP).rooms())
                        CType(ctrl, ctrlHP).change = ""
                    End If
                End If
            Next
            With New RoomsInventoryFacade
                .update(ds)
            End With
        End If

    End Sub
    Private Sub saveRowRooms(ByVal ds As RoomsInventoryData, ByVal inicio As Date, ByVal fin As Date, ByVal Rooms As Integer)
        ds.Tables(0).Columns.Add("RoomCode", GetType(System.String))

        For i As Integer = 0 To Me.ddlRoomtype.Items.Count - 1
            If Me.ddlRoomtype.Items(i).Value <> 0 Then
                Dim dr As DataRow = ds.Tables(ds.TBL_ROOMS_INVENTORY).NewRow
                dr(ds.FLD_DATE) = inicio
                dr(ds.FLD_STARTDATE) = inicio
                dr(ds.FLD_ENDDATE) = fin
                dr(ds.FLD_ID_ROOM_HOTEL) = Me.ddlRoomtype.Items(i).Value
                dr(ds.FLD_NUMBER_ROOMS) = Rooms
                dr(ds.FLD_STATUS) = 0
                dr("RoomCode") = Me.ddlRoomtype.Items(i).Text.Split("-")(0).Trim()
                ds.Tables(ds.TBL_ROOMS_INVENTORY).Rows.Add(dr)
                dr.AcceptChanges()
                dr(ds.FLD_STATUS) = dr(ds.FLD_STATUS)
            End If
        Next

    End Sub

    Private Function SaveRoom(ByVal tipoCuarto As Integer, ByVal inicio As Date, ByVal fin As Date, ByVal Rooms As Integer, ByRef dsBefore As RoomsInventoryData, ByRef dsTrans As RoomsInventoryData) As Boolean
        Dim hr As Boolean
        dsBefore = (New RoomsInventoryFacade).getInventoryByDate_Data(tipoCuarto, inicio, fin)

        If (cInfoActual.IsHouse) AndAlso Rooms > 1 Then
            Rooms = 1
        End If

        If tipoCuarto = 0 Then
            Dim ds As RoomsInventoryData = New RoomsInventoryData
            saveRowRooms(ds, inicio, fin, Rooms)
            With New RoomsInventoryFacade

                hr = .update(ds, GetDataExeption)
                dsTrans = ds
                dsTrans.AcceptChanges()

                If hr AndAlso cInfoActual.IsSingleImgInv Then
                    TwoWayUpdate(ds)
                End If

                Return hr
            End With
        Else
            With New RoomsInventoryFacade
                ' sdatodespues = String.Format("<NewDataSet><InventarioHabitaciones>room {0} begin date {1} end date {2} rooms {3} exception {4} </InventarioHabitaciones></NewDataSet>", tipoCuarto, inicio, fin, Rooms, GetDataExeption)
                hr = .update(tipoCuarto, inicio, fin, Rooms, 0, GetDataExeption)
                dsTrans = (New RoomsInventoryFacade).getInventoryByDate_Data(tipoCuarto, inicio, fin)

                If hr Then
                    Dim ds As RoomsInventoryData = New RoomsInventoryData
                    ds.Tables(0).Columns.Add("RoomCode", GetType(System.String))

                    Dim dr As DataRow = ds.Tables(ds.TBL_ROOMS_INVENTORY).NewRow
                    dr(ds.FLD_DATE) = inicio
                    dr(ds.FLD_STARTDATE) = inicio
                    dr(ds.FLD_ENDDATE) = fin
                    dr(ds.FLD_ID_ROOM_HOTEL) = tipoCuarto 'Me.ddlRoomtype.Items(i).Value
                    dr(ds.FLD_NUMBER_ROOMS) = Rooms
                    dr(ds.FLD_STATUS) = 0
                    dr("RoomCode") = Me.ddlRoomtype.Items(ddlRoomtype.Items.IndexOf(ddlRoomtype.Items.FindByValue(tipoCuarto))).Text.Split("-")(0).Trim()
                    ds.Tables(ds.TBL_ROOMS_INVENTORY).Rows.Add(dr)
                    dr.AcceptChanges()
                    dr(ds.FLD_STATUS) = dr(ds.FLD_STATUS)

                    If hr AndAlso cInfoActual.IsSingleImgInv Then
                        TwoWayUpdate(ds)
                    End If

                End If
                Return hr
            End With
        End If

    End Function


    Public Function CreateAvailHtml(ByVal room As String, ByVal dsBefore As RoomsInventoryData,
                                    ByVal dsTrans As RoomsInventoryData, ByVal IdRoom As Integer) As String
        Dim menu As New Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim dv As New DataView
        Dim sw As StringWriter = New StringWriter
        Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim shtml As String = ""
        Dim idioma As String
        Dim drhotel As DataRow = Me.HotelInfo

        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))

        menu.CellSpacing = 1
        menu.CellPadding = 1
        'menu.BorderWidth = 1
        menu.Width = New System.Web.UI.WebControls.Unit(600, UnitType.Pixel)

        tr = New TableRow
        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00056", idioma) ' "Room"  
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000120", idioma) ' "Date" 
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("A00041", idioma) '"Rooms"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        If IdRoom = 0 Then
            td.Text = String.Format("{0} ({1})", PortalCulture.GetString("01181", idioma), PortalCulture.GetString("00144", idioma)) '"Update Rooms"   
        Else
            td.Text = PortalCulture.GetString("01181", idioma)
        End If

        tr.Cells.Add(td)
        menu.Rows.Add(tr)

        Dim dr As DataRow
        Dim drd As DataRow
        For i As Integer = 0 To dsTrans.Tables(0).Rows.Count - 1
            drd = dsTrans.Tables(0).Rows(i)
            dv = dsBefore.Tables(0).DefaultView
            dv.RowFilter = String.Format("fecha= '{0}'", drd("Fecha"))
            If dv.Count > 0 Then
                tr = New TableRow
                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = room
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dv(0)("Fecha")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dv(0)("Habitaciones")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = drd("Habitaciones")
                tr.Cells.Add(td)
                menu.Rows.Add(tr)
            End If
        Next
        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
    End Function

    Function AddUpdateInventoryByInterval(ByVal selectedRoom As Integer, ByVal dsBefore As RoomsInventoryData)
        Dim str1 As String
        dsBefore.Tables(0).Columns.Add("Descr_room")
        dsBefore.Tables(0).Rows(0)("Descr_room") = selectedRoomCode
    End Function

    Private Sub btnSingleImgInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSingleImgInv.Click
        Dim ds As New RoomsInventoryData
        ds.Tables(0).Columns.Add("RoomCode", GetType(System.String))

        Dim dsActualInventory As RoomsInventoryData
        Dim fini As Date = CDate(txtInicio.Text)
        Dim fend As Date = CDate(txtFinal.Text)

        For i As Integer = 0 To Me.ddlRoomtype.Items.Count - 1
            If Me.ddlRoomtype.Items(i).Value <> 0 Then
                dsActualInventory = (New RoomsInventoryFacade).getInventoryByDate_Data(Me.ddlRoomtype.Items(i).Value, fini, fend)

                For Each row As DataRow In dsActualInventory.Tables(0).Rows
                    Dim dr As DataRow = ds.Tables(RoomsInventoryData.TBL_ROOMS_INVENTORY).NewRow

                    dr(RoomsInventoryData.FLD_DATE) = row(RoomsInventoryData.FLD_DATE)
                    dr(RoomsInventoryData.FLD_STARTDATE) = row(RoomsInventoryData.FLD_DATE)
                    dr(RoomsInventoryData.FLD_ENDDATE) = row(RoomsInventoryData.FLD_DATE)
                    dr(RoomsInventoryData.FLD_ID_ROOM_HOTEL) = Me.ddlRoomtype.Items(i).Value
                    dr(RoomsInventoryData.FLD_NUMBER_ROOMS) = row(RoomsInventoryData.FLD_NUMBER_ROOMS)
                    dr(RoomsInventoryData.FLD_STATUS) = 0
                    dr("RoomCode") = Me.ddlRoomtype.Items(i).Text.Split("-")(0).Trim()
                    ds.Tables(RoomsInventoryData.TBL_ROOMS_INVENTORY).Rows.Add(dr)
                    dr.AcceptChanges()
                    dr(RoomsInventoryData.FLD_STATUS) = dr(RoomsInventoryData.FLD_STATUS)
                Next

            End If
        Next

        TwoWayUpdate(ds)

    End Sub

    Private Sub btnSaveIntervals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveIntervals.Click
        If Not Page.IsValid Then Return
        Dim dsBefore As New RoomsInventoryData
        Dim dsTrans As New RoomsInventoryData
        Dim sdato As String
        Dim sdatoDespues As String
        Dim sdatocorreo As String

        If IsNumeric(Me.TxtRooms.Text) Then
            lblRoomsError.Visible = False
            If Not InvalidDate() Then
                If Me.ddlRoomtype.SelectedValue <> "" Then
                    Me.selectedRoom = Me.ddlRoomtype.SelectedValue
                    Me.selectedRoomCode = Me.ddlRoomtype.SelectedItem.Text
                    If SaveRoom(selectedRoom, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), CInt(Me.TxtRooms.Text), dsBefore, dsTrans) Then
                        'sdato = dsBefore.GetXml.ToString
                        'sdatoDespues = dsTrans.GetXml.ToString
                        Try
                            With New ClsRuWS
                                .CallRUWS(cInfoActual.Hotel, selectedRoom, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text))
                            End With
                        Catch ex As Exception

                        End Try

                        AddUpdateInventoryByInterval(selectedRoom, dsBefore)
                        sdato = Util.Utility.GetXml(dsTrans.TBL_ROOMS_INVENTORY, "UpdateInventoryByInterval", dsBefore)
                        AddUpdateInventoryByInterval(selectedRoom, dsTrans)
                        sdatoDespues = Util.Utility.GetXml(dsTrans.TBL_ROOMS_INVENTORY, "UpdateInventoryByInterval", dsTrans)

                        'sdatocorreo = CreateAvailHtml(selectedRoomCode, dsBefore, dsTrans, selectedRoom)
                        sdatocorreo = (New Util.Utility).GeneraCorreoXslt(sdato, sdatoDespues)
                        Dim sreference As String = ""
                        Dim snota As String = Nota2(Me.selectedRoomCode, CInt(Me.TxtRooms.Text), CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), GetDiaExeption(), sreference)
                        Me.guardalog("/Pages/HomePage.aspx", PaginaBase.acciones.Modificar, snota, sreference, sdato, sdatoDespues, sdatocorreo)
                        ' Me.guardalog("/Pages/HomePage.aspx", PaginaBase.acciones.Modificar, "La habitacion " & Me.selectedRoomCode & " cambió su inventario a " & CInt(Me.TxtRooms.Text) & " cuartos del " & CDate(Me.txtInicio.Text) & " al " & CDate(Me.txtFinal.Text) & " para los dias " & GetDiaExeption(), "Update availability", sdato, sdatoDespues, sdatocorreo)
                        TxtRooms.Text = String.Empty
                    End If
                End If
            End If
        Else
            lblRoomsError.Text = PortalCulture.GetString("00146")
            lblRoomsError.Visible = True
        End If
        Try
            Me.ddlMonth.SelectedIndex = CDate(Me.txtInicio.Text).Month - 1
            Me.ddlyear.SelectedValue = CDate(Me.txtInicio.Text).Year

        Catch ex As Exception
            Me.ddlMonth.SelectedIndex = Now.Date.Month - 1
            Me.ddlyear.SelectedValue = Now.Date.Year
        End Try
    End Sub


    Private Function InvalidDate() As Boolean
        Dim startDate, endDate As Date
        Try
            startDate = Date.Parse(Me.txtInicio.Text.Trim)
            endDate = Date.Parse(Me.txtFinal.Text.Trim)
            If (endDate >= startDate) Then
                Return False
                lblError.Visible = False
            Else
                lblError.Text = PortalCulture.GetString("00145")
                lblError.Visible = True
                Return True
            End If
        Catch ex As Exception
            lblError.Text = PortalCulture.GetString("00145")
            lblError.Visible = True
            Return True
        End Try
    End Function

    Private Function GetAplyWeek() As String
        Dim ck As CheckBox
        Dim AplyWeek As String
        For i As Integer = 1 To 7
            ck = FindControl("Chk" & i.ToString)
            If ck.Checked Then
                AplyWeek &= "Y"
            Else
                AplyWeek &= "N"
            End If
        Next
        Return AplyWeek
    End Function
    Private Function GetDataExeption() As String
        Dim cad As String = ""
        If Chk1.Checked Then
            cad &= "1"
        End If
        If Chk2.Checked Then
            cad &= "2"
        End If
        If Chk3.Checked Then
            cad &= "3"
        End If
        If Chk4.Checked Then
            cad &= "4"
        End If
        If Chk5.Checked Then
            cad &= "5"
        End If
        If Chk6.Checked Then
            cad &= "6"
        End If
        If Chk7.Checked Then
            cad &= "7"
        End If
        Return cad
    End Function
    Private Function GetDiaExeption() As String
        Dim cad As String = ""
        If Chk1.Checked Then
            cad &= "Lunes, "
        End If
        If Chk2.Checked Then
            cad &= "Martes, "
        End If
        If Chk3.Checked Then
            cad &= "Miercoles, "
        End If
        If Chk4.Checked Then
            cad &= "Jueves,"
        End If
        If Chk5.Checked Then
            cad &= "Viernes, "
        End If
        If Chk6.Checked Then
            cad &= "Sabado, "
        End If
        If Chk7.Checked Then
            cad &= "Domingo "
        End If
        Return cad
    End Function

    Private Sub cmdShowInventory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdShowInventory.Click
        Dim month As Integer = Me.ddlMonth.SelectedIndex + 1
        Dim year As Integer = 0
        Integer.TryParse(Me.ddlyear.SelectedValue, year)
        Dim searched As New Date(year, month, 1)

        Me.txtInicio.Text = searched.ToString("MM/dd/yyyy")
        Me.txtFinal.Text = searched.AddDays(Date.DaysInMonth(year, month) - 1).ToString("MM/dd/yyyy")
    End Sub

    Private Sub TwoWayUpdate(ByVal dsRooms As RoomsInventoryData)
        Dim service As New WsConnectWcf.wsConnectWCFv2
        Dim RQ As New WsConnectWcf.OTA_HotelAvailNotifRQ
        Dim POS(0) As WsConnectWcf.SourceType
        POS(0) = New WsConnectWcf.SourceType
        Dim RequestorID As New WsConnectWcf.SourceTypeRequestorID
        Dim AvailStatusMessages As New WsConnectWcf.OTA_HotelAvailNotifRQAvailStatusMessages
        Dim ASMQuantity As Integer = 0
        Dim Index As Integer = 0

        RQ.Version = 1
        RequestorID.Type = "22"
        RequestorID.ID = "IPRM"
        Dim myuuid As Guid = Guid.NewGuid()
        RQ.EchoToken = myuuid.ToString()

        ASMQuantity = dsRooms.Tables(0).Rows.Count - 1

        Dim AvailStatusMessage(ASMQuantity) As WsConnectWcf.AvailStatusMessageType
        For Each dr As DataRow In dsRooms.Tables(0).Rows
            AvailStatusMessage(Index) = New WsConnectWcf.AvailStatusMessageType

            Dim StatusApplicationControl As New WsConnectWcf.StatusApplicationControlType

            AvailStatusMessage(Index).BookingLimit = dr(RoomsInventoryData.FLD_NUMBER_ROOMS)
            StatusApplicationControl.InvTypeCode = dr("RoomCode")
            StatusApplicationControl.Start = CDate(dr(RoomsInventoryData.FLD_STARTDATE)).ToString("yyyy-MM-dd").Replace("-", "")
            StatusApplicationControl.End = CDate(dr(RoomsInventoryData.FLD_ENDDATE)).ToString("yyyy-MM-dd").Replace("-", "")

            If Not Chk1.Checked Or Not Chk2.Checked Or Not Chk3.Checked Or Not Chk4.Checked Or Not Chk5.Checked Or Not Chk6.Checked Or Not Chk7.Checked Then
                StatusApplicationControl.Mon = Chk1.Checked
                StatusApplicationControl.Tue = Chk2.Checked
                StatusApplicationControl.Weds = Chk3.Checked
                StatusApplicationControl.Thur = Chk4.Checked
                StatusApplicationControl.Fri = Chk5.Checked
                StatusApplicationControl.Sat = Chk6.Checked
                StatusApplicationControl.Sun = Chk7.Checked

                StatusApplicationControl.MonSpecified = True
                StatusApplicationControl.WedsSpecified = True
                StatusApplicationControl.ThurSpecified = True
                StatusApplicationControl.TueSpecified = True
                StatusApplicationControl.SatSpecified = True
                StatusApplicationControl.SunSpecified = True
                StatusApplicationControl.FriSpecified = True
            End If
            AvailStatusMessage(Index).StatusApplicationControl = StatusApplicationControl
            Index += 1
        Next

        AvailStatusMessages.HotelCode = cInfoActual.Empresa.ToString()
        AvailStatusMessages.AvailStatusMessage = AvailStatusMessage

        RQ.POS = POS
        RQ.AvailStatusMessages = AvailStatusMessages
        POS(0).RequestorID = RequestorID

        Dim strRequest As String = MyBase.GetXMLFromObject(RQ)

        Dim requestXDocument As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Parse(strRequest)

        Dim xmlRQ As System.Xml.Linq.XElement = requestXDocument.Element("OTA_HotelAvailNotifRQ")

        Dim soapRequest As System.Xml.Linq.XDocument = Soap.CreateSoapRequestXml(xmlRQ)


        MyBase.WriteLog(String.Format("Request: {0}", strRequest), "SingleImgInv")
        Dim url As String = ConfigurationManager.AppSettings("TwoWayUpdateURL")
        Dim strError As String = String.Empty
        Try
            Dim HttpReq As System.Net.HttpWebRequest = System.Net.WebRequest.Create(url)

            HttpReq.Method = "POST"
            Dim bytes() As Byte = System.Text.Encoding.ASCII.GetBytes(soapRequest.ToString())
            HttpReq.ContentType = "application/xml; encoding='utf-8'"
            HttpReq.ContentLength = bytes.Length
            Dim requestStream As System.IO.Stream = HttpReq.GetRequestStream()
            requestStream.Write(bytes, 0, bytes.Length)
            requestStream.Close()
            Dim response As System.Net.HttpWebResponse = HttpReq.GetResponse()
            If Not response.StatusCode = System.Net.HttpStatusCode.OK Then
                lblError.Text = "Falló el envío de inventario a channel manager: " & response.StatusCode.ToString()
                lblError.Visible = True
            Else
                MyBase.WriteLog(String.Format("Response: {0}", response.StatusCode.ToString()), "SingleImgInv")
                lblError.Text = "Inventario Actualizado"
                lblError.Visible = True
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True

            MyBase.WriteLog(String.Format("Response: {0}", ex.StackTrace), "SingleImgInv")
        End Try
    End Sub
End Class
