Imports Microsoft.VisualBasic
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data

Imports System.IO
Imports System.Text
Imports System.Xml.Linq

Imports APIServices.Models
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Conflux.Helpers.Restriction
Imports APIServices.Conflux.Parser.Restriction
Imports APIServices.Conflux.Models.Restrictions.Response
Imports APIServices.Xml.Soap
Imports APIServices.Xml.OTA.Request.Restrictions
Imports RateManager.Utitlities.Hotel
Imports APIServices.Conflux.OTA.Models.Restrictions

Partial Class AvailabilityRestrictions
    Inherits PaginaBase
    Private Enum cancelpolicy
        bydays
        byhour
        specifichour
    End Enum
    Private Property SelectedMes() As Integer
        Get
            Return ViewState("SelectedMes")
        End Get
        Set(ByVal Value As Integer)
            ViewState("SelectedMes") = Value
        End Set
    End Property

    Private Property SelectedYear() As String
        Get
            Return ViewState("SelectedYear")
        End Get
        Set(ByVal Value As String)
            ViewState("SelectedYear") = Value
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
    Private Property ratesplans() As String
        Get
            Return ViewState("_ratesplans")
        End Get
        Set(ByVal Value As String)
            ViewState("_ratesplans") = Value
        End Set
    End Property




#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents tblMonth4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth8 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth9 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth11 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMonth12 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblMonth4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth5 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth6 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth9 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth8 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth7 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth10 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth11 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMonth12 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents divChk As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lbl_Status As System.Web.UI.WebControls.Label
    Protected WithEvents lblNameRatePlan As System.Web.UI.WebControls.Label
    Protected WithEvents lblRPName As System.Web.UI.WebControls.Label

    Protected WithEvents lnk As System.Web.UI.WebControls.LinkButton


    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

    Private Function RatePlanFilter(ByVal segmentType As String, ByVal RatesPlan As Portal.General.Common.Data.RatePlanData) As Portal.General.Common.Data.RatePlanData
        'Elimina los planes tarifarios que contengan el tipo de segmento especificado
        Dim dv2 As DataView
        For Each r As DataRow In RatesPlan.Tables("RatePlans").Rows()
            dv2 = RatesPlan.Tables("RatePlans").DefaultView
            dv2.RowFilter = "Segment" & "=" & "'" & segmentType & "'"
            If dv2.Count > 0 AndAlso r("Segment").ToString() = segmentType Then
                r.Delete()
            End If
        Next
        RatesPlan.AcceptChanges()
        Return RatesPlan
    End Function

#End Region
    Protected CtlMensajes1 As ctlMensajes
    Protected CtlMensajeRuleConf1 As ctlMensajeRuleConf
    Dim banIni As Integer = 0

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Dim idAsoc As Integer = Me.GetIdAsociation

        ddlCancelationPolicy.Attributes.Add("onchange", "javascript:LoadMsg('" & Me.ddlCancelationPolicy.ClientID _
        & "','" & lblAux.ClientID & "','" & lblEDaysHour.ClientID & "','" & PortalCulture.GetString("00410") _
        & "','" & PortalCulture.GetString("00409") & "','" & PortalCulture.GetString("00411") _
        & "','" & PortalCulture.GetString("00412") & "','" & PortalCulture.GetString("00413") & "')")

        hplHide.NavigateUrl = "javascript:Ocultar('0');"
        hplShow.NavigateUrl = "javascript:Ocultar('1');"
        Me.RbdHotel.Attributes.Add("onclick", "javascript:HideChk()")
        Me.RbdRatePlan.Attributes.Add("onclick", "javascript:HideChk()")

        Me.RdHotel.Attributes.Add("onclick", "javascript:HideChk2()")
        Me.RdRatePlan.Attributes.Add("onclick", "javascript:HideChk2()")

        If Not IsPostBack Then

            lblSep.Style.Add("display", "none")
            ddlHour.Style.Add("display", "none")
            ddlMinutes.Style.Add("display", "none")

            ddlCancelationPolicy.Items.Clear()
            ddlCancelationPolicy.Items.Insert(cancelpolicy.bydays, PortalCulture.GetString("00020"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.byhour, PortalCulture.GetString("00021"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.specifichour, PortalCulture.GetString("00381"))

            CargaFechas(True)
            CargaSegmentos()
            hplShow.Style.Add("display", "")
            hplHide.Style.Add("display", "none")
            Me.ddlStatus.Items.Clear()
            Me.ddlStatus.Items.Add("S")
            Me.ddlStatus.Items.Add("O")
            Me.ddlStatus.Items.Add("C")
            Me.ddlStatus.Items.Add("N")
            Me.txtInicio.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
            Dim ds As RatePlanData
            With New RatePlanFacade
                'If MyBase.IsSupervisor Then
                ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=idAsoc, DeleteFilter:=1)

                If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
                    ds = RatePlanFilter("C", ds)
                End If

                'Else
                '    ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
                'End If                
            End With
            ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", GetType(System.String), "substring(" & ds.FIELD_CODIGOTARIFA & "+' - ' + " & ds.FIELD_NAME & ",1,25)")
            'ratesplans = ""
            'For i As Integer = 0 To ds.Tables(ds.RATEPLAN_TABLE).Rows.Count - 1
            '    If ds.Tables(ds.RATEPLAN_TABLE).Rows(i).IsNull(ds.FIELD_NAME) Then
            '        ratesplans &= "--" & "//"
            '    Else
            '        ratesplans &= ds.Tables(ds.RATEPLAN_TABLE).Rows(i).Item(ds.FIELD_NAME).ToString & "//"
            '    End If
            'Next
            'DlRatePlan.Attributes.Add("onChange", "javascript:showRatePlanName('" & DlRatePlan.ClientID & "','" & ratesplans & "','" & lblNameRatePlan.ClientID & "')")
            'ddlRateplans.Attributes.Add("onChange", "javascript:showRatePlanName('" & ddlRateplans.ClientID & "','" & ratesplans & "','" & lblRPName.ClientID & "')")

            ddlRateplans.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
            ddlRateplans.DataValueField = ds.FIELD_IDRATEPLAN
            ddlRateplans.DataSource = ds
            ddlRateplans.DataBind()

            DlRatePlan.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
            DlRatePlan.DataValueField = ds.FIELD_CODIGOTARIFA
            DlRatePlan.DataSource = ds
            DlRatePlan.DataBind()
        End If
        Me.ResizefrmPrincipal()

        hplNext.NavigateUrl = "javascript:submitpage(3)"
        hplPrevius.NavigateUrl = "javascript:submitpage(-3)"

        If RbdHotel.Checked = True Then
            divApplyAllPlan.Style.Add("display", "")
            'chkApplyAllPlan.Style.Add("display", "")

        Else
            divApplyAllPlan.Style.Add("display", "none")
            'chkApplyAllPlan.Style.Add("display", "none")

        End If

        'Enlazar el control con el evento
        'banIni = 0
        'Page_PreRender1(sender, e)


        CtlMensajeRuleConf1.loadRooms(MyBase.cInfoActual.Hotel)
        CtlMensajeRuleConf1.loadRatePlans(MyBase.cInfoActual.Hotel)
        btnSave.OnClientClick = String.Format("return valCheckbox() && FireUpdateStatus('{0}');", PortalCulture.GetString("00608"))
        btnModalSave.Attributes.Add("onclick", "javascript:openModal('" & btnSave.ClientID & "','¿Está seguro que desea aplicar los cambios?','Confirmar cambios de disponibilidad')")
        btnLoad.OnClientClick = String.Format("return FireUpdateStatus('{0}');", PortalCulture.GetString("00608"))
        btnModalSave.Attributes.Add("onClick", "javascript:openModal('" & btnSave.ClientID & "', '¿Está seguro que desea aplicar los cambios?','Confirmar cambios de disponibilidad'); return false;")
        btnModalSave.Value = RateManager.PortalCulture.GetString("A00153")
        ' Page.ClientScript.ValidateEvent(Me.btnLoad.UniqueID, Me.ToString())
    End Sub

    Public Function getFunctionShow() As String
        Return "javascript:if (evalDates()) { " &
             CtlMensajes1.getShow(Me.btnSave.ClientID, "", PortalCulture.GetString("00608")) &
                 "}else{var o; alert(o); o=document.getElementById('" & Me.btnSave.ClientID & "'); o.click();}  "

    End Function

    Private Sub showStatus()
        Dim _month As Integer = SelectedMes
        Dim _year As Integer = SelectedYear
        Dim selectDate, EndDate As Date, FirstDay As Integer
        Dim data As DataSet

        selectDate = New Date(CDbl(SelectedYear), SelectedMes, 1)
        EndDate = selectDate.AddMonths(3)
        EndDate.AddDays(Date.DaysInMonth(EndDate.Year, EndDate.Month) - 1)

        With New HotelStatusAvailabilityFacade
            data = .getStatusAva(Me.cInfoActual.Hotel, selectDate, EndDate)
        End With

        If data.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            StatusHotel = data.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_STATUSAVAILABILITY)
        End If
        Dim colorStatusHotel = color(StatusHotel)
        Dim dvInventory As DataView, dvReservas As DataView, dvlock As DataView, dvlockrproom As DataView, dvlockrp As DataView
        dvlock = data.Tables(clsCommonAvailibilityGral.TABLE_LockGral).DefaultView
        dvReservas = data.Tables(ReservaDatos.RESERVA_TABLE).DefaultView
        dvlockrproom = data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).DefaultView
        dvInventory = data.Tables(RoomsInventoryData.TBL_ROOMS_INVENTORY).DefaultView
        dvlockrp = data.Tables(lockRatePlanData.TABLE_LockRatePlan).DefaultView
        Dim row, col As Integer
        For i As Integer = 1 To 3
            _month = SelectedMes + i - 1
            If _month > 12 Then
                _month = _month - 12
                _year = SelectedYear + 1
            End If
            selectDate = New Date(_year, _month, 1)
            EndDate = New Date(_year, _month, Date.DaysInMonth(_year, _month))
            FirstDay = Weekday(selectDate, FirstDayOfWeek.Monday)

            Dim tbl As HtmlTable, lbl As Label
            tbl = Me.FindControl("tblMonth" & i.ToString)
            lbl = Me.FindControl("lblMonth" & i.ToString)
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            lbl.Text = MonthName(_month)
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            tbl.Rows(0).Attributes.Add("class", "clsSmallDarkLabel")
            tbl.Rows(1).Attributes.Add("class", "clsHeadTitleSmall")
            tbl.Rows(1).Cells(0).InnerText = PortalCulture.GetString("00306")
            tbl.Rows(1).Cells(1).InnerText = PortalCulture.GetString("00301")
            tbl.Rows(1).Cells(2).InnerText = PortalCulture.GetString("00302")
            tbl.Rows(1).Cells(3).InnerText = PortalCulture.GetString("00303")
            tbl.Rows(1).Cells(4).InnerText = PortalCulture.GetString("00304")
            tbl.Rows(1).Cells(5).InnerText = PortalCulture.GetString("00305")
            tbl.Rows(1).Cells(6).InnerText = PortalCulture.GetString("00300")
            For col = 0 To 6
                tbl.Rows(1).Cells(col).EnableViewState = False
            Next
            For row = 1 To 6
                tbl.Rows(row + 1).Attributes.Add("class", "clsSmallLabel")
                tbl.Rows(row + 1).EnableViewState = False
                For col = 0 To 6

                    With tbl.Rows(row + 1).Cells(col)
                        tbl.Rows(row + 1).Cells(col).EnableViewState = False
                        .InnerText = ((row - 1) * 7) - FirstDay + col + 2
                        If CDbl(.InnerText) <= 0 Or CInt(.InnerText) > Date.DaysInMonth(_year, _month) Then
                            .InnerText = ""
                            .BgColor = "gainsboro"

                        Else
                            .BgColor = colorStatusHotel
                            If StatusHotel.ToUpper = "O" Then
                                Dim substr As Integer
                                'status del lockgral
                                Dim _d As Date = New Date(_year, _month, CInt(.InnerText))
                                '0= domingo 1=lunes 6= sabado
                                '0=lunes
                                substr = _d.DayOfWeek - 1
                                If _d.DayOfWeek = 0 Then
                                    substr = 6
                                End If
                                dvlock.RowFilter = clsCommonAvailibilityGral.FIELD_StartDate & "='" & _d & "'"
                                If dvlock.Count > 0 Then
                                    .BgColor = color(dvlock(0)(clsCommonAvailibilityGral.FIELD_statusAvailability))
                                End If
                                If .BgColor = "" Then
                                    .BgColor = colorStatusHotel
                                End If
                                If .BgColor <> "red" Then
                                    dvInventory.RowFilter = "fecha = '" & _d & "'"
                                    dvReservas.RowFilter = ReservaDatos.FIELD_CHECKIN & "<='" & _d & "' and " & ReservaDatos.FIELD_CHECKOUT & ">'" & _d & "'"
                                    dvlockrproom.RowFilter = LockRatesPlanRoomData.FIELD_StartDate & "<='" & _d & "' and " & LockRatesPlanRoomData.FIELD_EndDate & ">='" & _d & "'"
                                    If dvInventory.Count > 0 Then
                                        If Not checadisponibilidadLRPR(dvInventory, dvReservas, dvlockrproom, _d, data.Tables(RatePlanData.RATEPLAN_TABLE)) Then
                                            .BgColor = "LightSalmon"
                                        Else
                                            dvlockrp.RowFilter = lockRatePlanData.FIELD_StartDate & "<='" & _d & "' and " & lockRatePlanData.FIELD_EndDate & ">='" & _d & "'"
                                            'supuestamente es uno por rate plan, no debe repetirse para un rateplan
                                            Dim Idrps As String = ""
                                            Dim StRps As String = ""
                                            Dim subday As Integer = _d.DayOfWeek()
                                            If subday = 0 Then
                                                subday = 6
                                            Else
                                                subday -= 1
                                            End If
                                            For dvr As Integer = 0 To dvlockrp.Count - 1
                                                'dayofweek 0= domingo, 6=sabado
                                                If dvlockrp(dvr)(lockRatePlanData.FIELD_AplyWeek).ToString.ToUpper.Trim.Substring(subday, 1) = "Y" And (dvlockrp(dvr)(lockRatePlanData.FIELD_StatusAvailability).ToString.ToUpper.Trim = "C" Or dvlockrp(dvr)(lockRatePlanData.FIELD_StatusAvailability).ToString.ToUpper.Trim = "N") Then
                                                    If Idrps.IndexOf(dvlockrp(dvr)(lockRatePlanData.FIELD_RatePlan).ToString.ToUpper.Trim) = -1 Then
                                                        Idrps &= dvlockrp(dvr)(lockRatePlanData.FIELD_RatePlan).ToString.ToUpper.Trim & ","
                                                        StRps &= dvlockrp(dvr)(lockRatePlanData.FIELD_StatusAvailability).ToString.ToUpper.Trim & ","
                                                    End If
                                                End If
                                            Next
                                            'si todos los ratesplan tenian bloquedo
                                            If Idrps <> "" AndAlso Idrps.Split(",").Length - 1 = data.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count Then
                                                'si todos estan cerrados ...
                                                If StRps.IndexOf("C") <> -1 Then
                                                    .BgColor = color("C")
                                                Else
                                                    .BgColor = color("N")
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End With
                Next
            Next
        Next
    End Sub

    Private Function checadisponibilidadLRPR(ByVal dvInventory As DataView, ByVal dvReservas As DataView, ByVal dvlockrp As DataView, ByVal dia As Date, ByVal rateplan As DataTable) As Boolean
        For Each drRp As DataRow In rateplan.Rows
            For Each d As DataRowView In dvInventory
                'ese tipo habitacion hotel tiene lock??? y el rateplan ke pex????
                dvlockrp.RowFilter = RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & d(RoomsHotelData.FLD_ID_ROOM_HOTEL) & " and " & RatePlanData.FIELD_IDRATEPLAN & "='" & drRp(RatePlanData.FIELD_IDRATEPLAN) & "'" _
                & " and " & LockRatesPlanRoomData.FIELD_StartDate & "<='" & dia & "' and " & LockRatesPlanRoomData.FIELD_StartDate & ">='" & dia & "'"
                If dvlockrp.Count > 0 Then
                    'sumar las reservaciones para esa habitación con ese rateplan
                    dvReservas.RowFilter = RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & d(RoomsHotelData.FLD_ID_ROOM_HOTEL) & " and rateplan='" & drRp(RatePlanData.FIELD_CODIGOTARIFA) & "'" _
                    & " and " & ReservaDatos.FIELD_CHECKIN & "<='" & dia & "' and " & ReservaDatos.FIELD_CHECKOUT & ">'" & dia & "'"
                    Dim res As Integer = 0
                    For Each dRes As DataRowView In dvReservas
                        res += dRes("reservas")
                    Next
                    If res < dvlockrp(0)(LockRatesPlanRoomData.FIELD_RoomsAvailable) Then
                        Return True
                    End If
                Else
                    If Not drRp("AvaSegmento") Is System.DBNull.Value Then
                        If AvaRP(d, dvReservas, dia, drRp) Then
                            Return True
                        End If
                    Else
                        If AvaRooms(d, dvReservas, dia) Then
                            Return True
                        End If
                    End If
                End If
            Next
        Next
        Return False
    End Function
    Private Function AvaRP(ByVal drRoom As DataRowView, ByVal dvReservas As DataView, ByVal dia As Date, ByVal rateplan As DataRow) As Boolean
        Dim resRP As Integer = 0
        dvReservas.RowFilter = "rateplan='" & rateplan(RatePlanData.FIELD_CODIGOTARIFA) & "'" _
            & " and " & ReservaDatos.FIELD_CHECKIN & "<='" & dia & "' and " & ReservaDatos.FIELD_CHECKOUT & ">'" & dia & "'"
        For pos As Integer = 0 To dvReservas.Count - 1
            resRP += dvReservas(pos).Item("reservas")
        Next
        If resRP < Int(rateplan("AvaSegmento")) Then
            If drRoom(RoomsInventoryData.FLD_NUMBER_ROOMS) < rateplan("AvaSegmento") Then
                'sumar las reservaciones de un mismo tipo de habitacion
                Dim resHab As Integer = 0
                dvReservas.RowFilter = RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & drRoom(RoomsHotelData.FLD_ID_ROOM_HOTEL) _
                    & " and " & ReservaDatos.FIELD_CHECKIN & "<='" & dia & "' and " & ReservaDatos.FIELD_CHECKOUT & ">'" & dia & "'"
                For pos As Integer = 0 To dvReservas.Count - 1
                    resHab += dvReservas(pos).Item("reservas")
                Next
                If resHab >= drRoom("habitaciones") Then
                    Return False
                End If
            End If
            Return True
        End If
        Return False
    End Function
    Private Function AvaRooms(ByVal drRoom As DataRowView, ByVal dvReservas As DataView, ByVal dia As Date) As Boolean
        'sumar las reservaciones de un mismo tipo de habitacion
        Dim resHab As Integer = 0
        dvReservas.RowFilter = RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & drRoom(RoomsHotelData.FLD_ID_ROOM_HOTEL) _
            & " and " & ReservaDatos.FIELD_CHECKIN & "<='" & dia & "' and " & ReservaDatos.FIELD_CHECKOUT & ">'" & dia & "'"
        For pos As Integer = 0 To dvReservas.Count - 1
            resHab += dvReservas(pos).Item("reservas")
        Next
        If resHab >= drRoom(RoomsInventoryData.FLD_NUMBER_ROOMS) Then
            Return False
        End If
        Return True
    End Function

    Private Function ReturnStatus(ByVal dv As DataView, ByVal dia As Date, ByVal rp As DataTable) As String
        ' dv.RowFilter = "fecha= '" & dia & "'"
        If Not dv(0)(HotelDatos.FIELD_STATUSAVAILABILITY) Is System.DBNull.Value Then
            Select Case dv(0)(HotelDatos.FIELD_STATUSAVAILABILITY).ToUpper.Trim
                Case "O"
                    If Not checadisponibilidadLRPR(dv, dia, rp) Then
                        Return "LightSalmon"
                    End If
                    Return "MediumSeaGreen"
                Case "C"
                    Return "red"
                Case "N"
                    Return "LightSteelBlue"
            End Select
        End If
        If Not checadisponibilidadLRPR(dv, dia, rp) Then
            Return "LightSalmon"
        End If
        Return ""
    End Function

    Private Function color(ByVal status As String) As String
        Select Case status.ToUpper.Trim
            Case "O"
                Return "MediumSeaGreen"
            Case "C"
                Return "Red"
            Case "N"
                Return "LightSteelBlue"
            Case "NR"
                Return "#b8860b"
            Case "NA"
                Return "#ffa07a"
        End Select
        Return ""
    End Function


    Private Function checadisponibilidadLRPR(ByVal dv As DataView, ByVal dia As Date, ByVal data As DataTable) As Boolean
        'dv.RowFilter = "fecha= '" & dia & "'"
        For Each d As DataRowView In dv
            If Not d.Item("AvalockRPRoom") Is System.DBNull.Value Then
                If d.Item("reservas") < d.Item("AvalockRPRoom") Then
                    Return True
                End If
            Else
                'para esa habitacion hay disponibilidad con algun rateplan ???
                Return AvaLRP(d, dv, dia, data)
            End If
        Next
    End Function

    Private Function AvaLRP(ByVal d As DataRowView, ByVal dv As DataView, ByVal dia As Date, ByVal rpData As DataTable) As Boolean
        For Each rp As DataRow In rpData.Rows
            If d.Item("inventariohab") < rp("AvaSegmento") Then
                'sumar las reservaciones de un mismo tipo de habitacion
                Dim resHab As Integer = 0
                dv.RowFilter = RoomsInventoryData.FLD_ID_ROOM_HOTEL & "=" & d(RoomsInventoryData.FLD_ID_ROOM_HOTEL) & " and fecha='" & dia & "'"
                For pos As Integer = 0 To dv.Count - 1
                    resHab += dv(pos).Item("reservas")
                Next
                If resHab < d.Item("inventariohab") Then
                    Return True
                End If
            Else
                'sumar las reservaciones de un mismo rp
                Dim resRP As Integer = 0
                dv.RowFilter = "idrateplan='" & rp("idrateplan") & "' and fecha='" & dia & "'"
                For pos As Integer = 0 To dv.Count - 1
                    resRP += dv(pos).Item("reservas")
                Next
                If resRP < Int(rp("AvaSegmento")) Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function
    Private Function checadisponibilidadLRP(ByVal dv As DataView, ByVal dr As DataRow, ByVal hab As Integer, ByVal rp As Integer, ByVal d As Date) As Boolean

        For h As Integer = 0 To hab - 1

            If dv((dv.Count / hab) * h).Item("inventariohab") < dr("AvaSegmento") And dv((dv.Count / hab) * h).Item("AvalockRPRoom") Is System.DBNull.Value Then
                'sumar las reservaciones de un mismo tipo de habitacion
                Dim resHab As Integer = 0
                For pos As Integer = dv.Count / h To rp
                    resHab += dv(pos).Item("reservas")
                Next
                If resHab < dv((dv.Count / hab) * h).Item("inventariohab") Then
                    Return True
                End If
            End If
        Next
        'sumar las reservaciones de un mismo rp
        dv.RowFilter = "idrateplan='" & dr("idrateplan") & "' and fecha='" & d & "'"
        Dim resRP As Integer = 0
        'si todas las habitaciones tienen bloqueo por cuarto y rateplan este valor no se checa
        Dim checarAvaSegmento As Boolean = False
        For Each r As DataRowView In dv
            resRP += r("reservas")
            If r("AvalockRPRoom") Is System.DBNull.Value Then
                checarAvaSegmento = True
            End If
        Next
        If checarAvaSegmento Then
            If resRP < Int(dr("AvaSegmento")) Then
                Return True
            End If
        End If
        Return False
    End Function


    'Private Sub LoadDataByRP()
    '    Dim _month As Integer = SelectedMes
    '    Dim _year As Integer = SelectedYear
    '    Dim selectDate, EndDate As Date, FirstDay As Integer
    '    Dim data As DataSet

    '    Me.StatusHotel = "O"

    '    selectDate = New Date(CDbl(SelectedYear), SelectedMes, 1)
    '    EndDate = selectDate.AddMonths(12)
    '    EndDate.AddDays(Date.DaysInMonth(EndDate.Year, EndDate.Month) - 1)

    '    With New RoomsInventoryFacade
    '        data = .GetLocksAndAvailByRP(Me.cInfoActual.Hotel, selectDate, EndDate, Me.DlRatePlan.SelectedValue)
    '    End With
    '    If data.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 AndAlso Not data.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_STATUSAVAILABILITY) Is System.DBNull.Value Then
    '        Me.StatusHotel = data.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_STATUSAVAILABILITY)
    '    End If


    '    Dim dv As DataView, dvRP As DataView, dvLH As DataView
    '    Dim row, col As Integer
    '    For i As Integer = 1 To 12
    '        _month = SelectedMes + i - 1
    '        If _month > 12 Then
    '            _month = _month - 12
    '            _year = SelectedYear + 1
    '        End If
    '        selectDate = New Date(_year, _month, 1)
    '        EndDate = New Date(_year, _month, Date.DaysInMonth(_year, _month))
    '        FirstDay = Weekday(selectDate, FirstDayOfWeek.Monday)

    '        Dim tbl As HtmlTable, lbl As Label
    '        tbl = Me.FindControl("tblMonth" & i.ToString)
    '        lbl = Me.FindControl("lblMonth" & i.ToString)
    '        Dim ci As System.Globalization.CultureInfo
    '        ci = System.Threading.Thread.CurrentThread.CurrentCulture
    '        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
    '        lbl.Text = MonthName(_month)
    '        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    '        tbl.Rows(0).Attributes.Add("class", "clsSmallDarkLabel")
    '        tbl.Rows(1).Attributes.Add("class", "clsHeadTitleSmall")
    '        tbl.Rows(1).Cells(0).InnerText = PortalCulture.GetString("00300")
    '        tbl.Rows(1).Cells(1).InnerText = PortalCulture.GetString("00301")
    '        tbl.Rows(1).Cells(2).InnerText = PortalCulture.GetString("00302")
    '        tbl.Rows(1).Cells(3).InnerText = PortalCulture.GetString("00303")
    '        tbl.Rows(1).Cells(4).InnerText = PortalCulture.GetString("00304")
    '        tbl.Rows(1).Cells(5).InnerText = PortalCulture.GetString("00305")
    '        tbl.Rows(1).Cells(6).InnerText = PortalCulture.GetString("00306")
    '        For row = 1 To 6
    '            tbl.Rows(row + 1).Attributes.Add("class", "clsSmallLabel")
    '            tbl.Rows(row + 1).EnableViewState = False
    '            For col = 0 To 6

    '                With tbl.Rows(row + 1).Cells(col)
    '                    .EnableViewState = False
    '                    .InnerText = ((row - 1) * 7) - FirstDay + col + 2
    '                    If CDbl(.InnerText) <= 0 Or CInt(.InnerText) > Date.DaysInMonth(_year, _month) Then
    '                        .InnerText = ""
    '                        .BgColor = "gainsboro"
    '                    Else
    '                        Dim _d As Date = New Date(_year, _month, CInt(.InnerText))

    '                        dvRP = data.Tables(lockRatePlanData.TABLE_LockRatePlan).DefaultView
    '                        dvRP.RowFilter = lockRatePlanData.FIELD_StartDate & "<='" & _d & "' and " & lockRatePlanData.FIELD_EndDate & ">= '" & _d & "'"


    '                        dvLH = data.Tables(clsCommonAvailibilityGral.TABLE_LockGral).DefaultView
    '                        dvLH.RowFilter = clsCommonAvailibilityGral.FIELD_StartDate & "='" & _d & "'"

    '                        .BgColor = color(StatusHotel)
    '                        If .BgColor = "" Then
    '                            .BgColor = "gainsboro"
    '                        End If

    '                        If Me.StatusHotel.ToUpper = "O" Then
    '                            Dim sw As Boolean = True
    '                            If dvLH.Count > 0 Then
    '                                .BgColor = color(dvLH(0)(lockRatePlanData.FIELD_StatusAvailability))

    '                                If .BgColor <> "MediumSeaGreen" Then
    '                                    sw = False
    '                                End If
    '                            End If

    '                            If dvRP.Count > 0 And sw = True Then
    '                                .BgColor = color(dvRP(0)(lockRatePlanData.FIELD_StatusAvailability))
    '                            End If
    '                        End If
    '                        If .BgColor = "" Then
    '                            .BgColor = color(StatusHotel)
    '                        End If
    '                        dvLH.RowFilter = ""
    '                        dvRP.RowFilter = ""

    '                        If .BgColor = "MediumSeaGreen" Then


    '                            Dim dvres As DataView
    '                            dvres = data.Tables("Inventario").DefaultView
    '                            dvres.RowFilter = "fecha ='" & _d & "'"
    '                            If dvres.Count > 0 Then
    '                                'aki es donde tengo ke evaluar los valores
    '                                'columns = SoldOut, AvailLockRPRoom,Inventario
    '                                Dim Reservaciones As Integer = 0, disponibilidad As Integer = 0
    '                                Dim checarsoldout As Boolean = False
    '                                Dim disponible As Boolean = False
    '                                Dim dispbyRoom As Boolean = False
    '                                Dim checa As Boolean = False
    '                                For Each item As DataRowView In dvres
    '                                    Reservaciones += item("reservaciones")
    '                                Next
    '                                For Each item As DataRowView In dvres
    '                                    If Not item("AvailLockRPRoom") Is System.DBNull.Value Or Not item("SoldOut") Is System.DBNull.Value Then
    '                                        checa = True
    '                                        If Not disponible Then
    '                                            If item("AvailLockRPRoom") Is System.DBNull.Value Then
    '                                                If Reservaciones < item("SoldOut") Then
    '                                                    'hay disponibilidad por rateplan ahora hay que checar si el inventari es menor ke el soldout 
    '                                                    If item("Inventario") < item("SoldOut") Then
    '                                                        If item("reservaciones") < item("Inventario") Then
    '                                                            disponible = True
    '                                                        End If
    '                                                    End If
    '                                                End If
    '                                            Else
    '                                                If item("reservaciones") < Int(item("AvailLockRPRoom")) Then
    '                                                    disponible = True
    '                                                End If
    '                                            End If
    '                                        Else
    '                                            Exit For
    '                                        End If
    '                                    End If
    '                                Next
    '                                'For Each item As DataRowView In dvres
    '                                '    If Not disponible Then
    '                                '        'Reservaciones += item("reservaciones")
    '                                '        If item("AvailLockRPRoom") Is System.DBNull.Value Then
    '                                '            If Not item("SoldOut") Is System.DBNull.Value Then
    '                                '                If item("Inventario") < item("SoldOut") Then
    '                                '                    If item("reservaciones") < item("Inventario") Then
    '                                '                        dispbyRoom = True
    '                                '                        checarsoldout = True
    '                                '                    End If
    '                                '                Else
    '                                '                    checarsoldout = True
    '                                '                End If
    '                                '            Else
    '                                '                disponible = True
    '                                '            End If
    '                                '        Else
    '                                '            If item("reservaciones") < Int(item("AvailLockRPRoom")) Then
    '                                '                disponible = True
    '                                '            End If
    '                                '        End If
    '                                '    Else
    '                                '        Exit For
    '                                '    End If
    '                                'Next
    '                                'si hay disponiblidad por cuarto pero puede qeu no haya disponibilidad con los rateplans
    '                                'If checarsoldout And dispbyRoom = True Then
    '                                '    If Reservaciones < Int(dvres(0)("SoldOut")) Then
    '                                '        disponible = True
    '                                '    Else
    '                                '        disponible = False
    '                                '    End If
    '                                'End If
    '                                'If Not dispbyRoom Then
    '                                '    If Not disponible Then
    '                                '        .BgColor = "LightSalmon"
    '                                '    End If
    '                                'End If
    '                                If Not disponible And checa Then
    '                                    .BgColor = "LightSalmon"
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End With
    '            Next
    '        Next
    '    Next
    'End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        'Me.lblMinPrice.Text = PortalCulture.GetString("01048", True)
        'Me.lblMaxPrice.Text = PortalCulture.GetString("01049", True)
        Me.lblEDaysHour.Text = PortalCulture.GetString("00410")
        Me.lblNoArrivals.Text = PortalCulture.GetString("00152")
        Me.lblOpen.Text = PortalCulture.GetString("00150")
        Me.lblClose.Text = PortalCulture.GetString("00151")
        Me.lblStart.Text = PortalCulture.GetString("00154", True)
        Me.lblPriorCancel.Text = PortalCulture.GetString("00440")
        lblnorates.Text = PortalCulture.GetString("00531")
        Me.hplNext.Text = PortalCulture.GetString("00735")
        Me.hplPrevius.Text = PortalCulture.GetString("00734")
        Me.lblSegment.Text = PortalCulture.GetString("M0UT02708")
        'lblNodisponible.Text = PortalCulture.GetString("00532")
        'Me.lblAplyAllPlan.Text = PortalCulture.GetString("00795")

        btnLoad.Text = PortalCulture.GetString("00149")
        Me.lblAllSold.Text = PortalCulture.GetString("00532")


        SelectedYear = ddlyear.SelectedValue
        Me.SelectedMes = Me.ddlMonth.SelectedIndex + 1
        imonth.Value = ddlMonth.SelectedIndex
        iyear.Value = ddlyear.SelectedIndex
        CargaFechas(False)
        Me.hplNext.Visible = True
        If Me.ddlyear.SelectedIndex = 4 AndAlso Me.ddlMonth.SelectedIndex > 9 Then
            Me.hplNext.Visible = False
        End If
        Me.hplPrevius.Visible = True
        If Me.ddlyear.SelectedIndex = 0 AndAlso Me.ddlMonth.SelectedIndex < 3 Then
            Me.hplPrevius.Visible = False
        End If

        If 1 = 1 Then
            If Me.RdHotel.Checked Then
                LoadDataByRP()
                lblTitle.Text = PortalCulture.GetString("00321")
                Me.divOnRequest.Style.Add("display", "")
                'chkMinPrice.Style.Add("display", "")
                'lblMinPrice.Style.Add("display", "")
                'txtMinPrice.Style.Add("display", "")
                'chkMaxPrice.Style.Add("display", "")
                'lblMaxPrice.Style.Add("display", "")
                'txtMaxPrice.Style.Add("display", "")


                Me.divddl.Style.Add("display", "none")
                'Me.divOnRequest.Style.Add("display", "")
                'Me.divddl.Style.Add("display", "none")
                RbdHotel.Checked = True
                divRdRatePlan.Style.Add("display", "none")

                RbdRatePlan.Checked = False
            Else
                LoadDataByRP()
                RbdHotel.Checked = False
                RbdRatePlan.Checked = True
                Me.divOnRequest.Style.Add("display", "none")
                'chkMinPrice.Style.Add("display", "none")
                'lblMinPrice.Style.Add("display", "none")
                'txtMinPrice.Style.Add("display", "none")
                'chkMaxPrice.Style.Add("display", "none")
                'lblMaxPrice.Style.Add("display", "none")
                'txtMaxPrice.Style.Add("display", "none")

                Me.divddl.Style.Add("display", "")
                divRdRatePlan.Style.Add("display", "")
                'Me.divOnRequest.Style.Add("display", "none")
                'Me.divddl.Style.Add("display", "")
                If (ddlRateplans.Items.Count > 0) Then
                    lblTitle.Text = PortalCulture.GetString("00322") & " " & Me.DlRatePlan.SelectedItem.Text
                Else
                    lblErrorGeneral.Text = PortalCulture.GetString("01242")
                    lblErrorGeneral.Visible = True
                End If

            End If
        Else
            banIni = 1
        End If

        Me.ddlStatus.Items(0).Text = PortalCulture.GetString("01090")
        Me.ddlStatus.Items(1).Text = PortalCulture.GetString("00150")
        Me.ddlStatus.Items(2).Text = PortalCulture.GetString("00151")
        Me.ddlStatus.Items(3).Text = PortalCulture.GetString("00152")

        Me.ddlStatus.Items(0).Value = "S"
        Me.ddlStatus.Items(1).Value = "O"
        Me.ddlStatus.Items(2).Value = "C"
        Me.ddlStatus.Items(3).Value = "N"

        Me.lblAdvBook.Text = PortalCulture.GetString("00325", True)
        Me.lblmindays.Text = PortalCulture.GetString("00117", True)
        Me.lblMaxdays.Text = PortalCulture.GetString("00118", True)
        Me.RbdRatePlan.Text = PortalCulture.GetString("00016")
        Me.lblFrom.Text = PortalCulture.GetString("00108")
        Me.lblTo.Text = PortalCulture.GetString("00109")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Bloqueos, Me.btnSave, "M")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Bloqueos, btnLoad, "R")
        hplHide.Text = PortalCulture.GetString("00454")
        hplShow.Text = PortalCulture.GetString("00453")
        Me.lblShowAvail.Text = PortalCulture.GetString("00455", True)
        btnSave.Text = PortalCulture.GetString("A00153")
        btnModalSave.Value = PortalCulture.GetString("A00153")
        chkOnRequest.Text = PortalCulture.GetString("00314")
        RdRatePlan.Text = PortalCulture.GetString("00016")
        'lblRatePlan.Text = PortalCulture.GetString("00016")
        lbldom.Text = PortalCulture.GetString("00551")
        Lbllun.Text = PortalCulture.GetString("00552")
        lblmar.Text = PortalCulture.GetString("00553")
        lblmie.Text = PortalCulture.GetString("00554")
        lbljue.Text = PortalCulture.GetString("00555")
        lblvie.Text = PortalCulture.GetString("00556")

        If RbdHotel.Checked = True Then
            divApplyAllPlan.Style.Add("display", "")
        Else
            divApplyAllPlan.Style.Add("display", "none")
            'chkApplyAllPlan.Style.Add("display", "none")
        End If
        chkApplyAllPlan.Text = PortalCulture.GetString("00797")
        lblAux.Text = PortalCulture.GetString("00413")
        spanMSG.InnerText = PortalCulture.GetString("01571")
    End Sub

    Private Sub CargaFechas(ByVal change As Boolean)
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        ddlMonth.Items.Clear()
        For i As Integer = 1 To 12
            ddlMonth.Items.Add(MonthName(i, True))
        Next

        ddlyear.Items.Clear()
        For i As Integer = Now.Year - 1 To Now.Year + 3
            ddlyear.Items.Add(New ListItem(i, i))
        Next
        If change = True Then
            Me.ddlMonth.SelectedIndex = Now.Month - 1
            Me.SelectedMes = Me.ddlMonth.SelectedIndex + 1
            ddlyear.SelectedValue = Now.Year
            Me.SelectedYear = ddlyear.SelectedValue
        Else
            Me.ddlMonth.SelectedIndex = Me.SelectedMes - 1
            ddlyear.SelectedValue = Me.SelectedYear
        End If

        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub CargaSegmentos()
        DlSegment.Items.Clear()
        Dim item As ListItem
        For i As Integer = 0 To 2
            item = New ListItem()
            Select Case i
                Case 0
                    item.Selected = True
                    item.Text = PortalCulture.GetString("00172")
                    item.Value = 0
                Case 1
                    item.Selected = False
                    item.Text = PortalCulture.GetString("M0UT02709")
                    item.Value = 1
                Case 2
                    item.Selected = False
                    item.Text = PortalCulture.GetString("M0UT02710")
                    item.Value = 2
            End Select
            DlSegment.Items.Add(item)
        Next
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not Page.IsValid Then Return

        Dim drhotel As DataRow = Me.HotelInfo
        Dim idioma As String

        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If Not InvalidDate() Then
            Dim nota As String
            nota &= "del " & txtInicio.Text & " al " & txtFinal.Text & " en los días " & Me.GetDiaExeption
            If chkMindays.Checked Then
                If idioma = "en-US" Then
                    nota &= " Minimum Days: " & Me.txtmindays.Text & ". "
                Else
                    nota &= " Mínimo de dias: " & Me.txtmindays.Text & ". "
                End If
            End If
            If chkMaxDays.Checked Then
                If idioma = "en-US" Then
                    nota &= " Maximum Days: " & Me.txtmindays.Text & ". "
                Else
                    nota &= " Máximo de dias: " & Me.txtMaxdays.Text & ". "
                End If
            End If
            If chkAdvBook.Checked Then
                nota &= " Advance booking: " & Me.txtAdvBook.Text & " Dias. "
            End If
            If chkEstatus.Checked Then
                nota &= " Status: " & ddlStatus.SelectedItem.Text & ". "
            Else
                nota &= " Status: " & ddlStatus.Items(0).Text & ". "
            End If

            'If Me.chkMinPrice.Checked AndAlso Me.txtMinPrice.Text <> "" Then
            '    If idioma = "en-US" Then
            '        nota &= " Minimum price: " & Me.txtmindays.Text & ". "
            '    Else
            '        nota &= " Precio mínimo: " & Me.txtMinPrice.Text & ". "
            '    End If
            'End If
            'If Me.chkMaxPrice.Checked AndAlso Me.txtMaxPrice.Text <> "" Then
            '    If idioma = "en-US" Then
            '        nota &= " Maximum price: " & Me.txtmindays.Text & ". "
            '    Else
            '        nota &= " Precio máximo: " & Me.txtMaxPrice.Text & ". "
            '    End If
            'End If

            Dim confluxService As New ConfluxService()
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)

            Dim roomsByHotel As RoomsHotelData = New RoomFacade().getAllRooms(info.Hotel, 1)
            Dim activeRooms As List(Of DataRow) = roomsByHotel.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Select("eliminada=false").ToList()

            Dim startDate, endDate As Date
            startDate = CDate(Me.txtInicio.Text)
            endDate = CDate(Me.txtFinal.Text)

            Dim apply As String = Me.GetAplyWeek()
            'Dim applyDays As String = RestrictionHelper.ToDayOfWeek(apply)
            'Dim dates As List(Of Tuple(Of Date, Date)) = RestrictionHelper.GetActiveDates(startDate, endDate, applyDays)

            RestrictionsParser.Init(info.Empresa)


            Dim banSelecccion As Boolean = RbdRatePlan.Checked
            If RbdHotel.Checked = True Then
                If chkOnRequest.Checked Then
                    If idioma = "en-US" Then
                        nota &= " On request: yes"
                    Else
                        nota &= " En petición: Si"
                    End If
                Else
                    If idioma = "en-US" Then
                        nota &= " On request: no"
                    Else
                        nota &= " En petición: no"
                    End If
                End If
                closehotel(nota)

                If ddlStatus.SelectedValue = "O" Then
                    For Each r As ListItem In ddlRateplans.Items
                        closeRateplan(nota, r.Value, False, splan:=r.Text)
                    Next
                End If


                If chkApplyAllPlan.Checked AndAlso RbdHotel.Checked Then
                    For Each r As ListItem In ddlRateplans.Items
                        closeRateplan(nota, r.Value, False, splan:=r.Text)
                        'closeRateplan(nota, r.Value, False, splan:=r.Text, hotelId:=info.Hotel, dates:=dates, confluxService:=confluxService, isEnabledGoogleRequest:=isEnabledGoogleRequest, activeRooms:=activeRooms, restrictionType:=RestrictionEnum.LockGral)
                    Next
                End If

                'Google


                If isEnabledGoogleRequest Then

                    Dim dsRatePlans As RatePlanData = New RatePlanFacade().GetRatePlanByIdHotel(info.Hotel.ToString(), idioma:=1, IncluirPaquetesSegmentoK:=1, incluirNetRatesPlan:=1, idAsociacion:=-1, DeleteFilter:=1)
                    Dim dsRatePlansPromos As RatePlanData = New RatePlanFacade().GetRatePlanByIdHotel(info.Hotel.ToString(), idioma:=1, IncluirPaquetesSegmentoK:=1, incluirNetRatesPlan:=1, idAsociacion:=-1, DeleteFilter:=1, getPromos:=True)


                    Dim activeRatePlans = dsRatePlans.Tables(RatePlanData.RATEPLAN_TABLE).Select().ToList()

                    Dim filterPromosDates As String = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"
                    Dim activeRatePlansPromos = dsRatePlansPromos.Tables(RatePlanData.RATEPLAN_TABLE).Select(filterPromosDates).ToList()

                    Dim lockGral As List(Of spGetLockGralByHotel_Result) = New List(Of spGetLockGralByHotel_Result)

                    'For Each [date] As Tuple(Of Date, Date) In dates

                    '    Dim tempLock As spGetLockGralByHotel_Result = New spGetLockGralByHotel_Result
                    '    tempLock.StartDate = [date].Item1
                    '    tempLock.EndDate = [date].Item2
                    '    tempLock.Status = ddlStatus.SelectedValue
                    '    lockGral.Add(tempLock)
                    'Next

                    Dim tempLock As spGetLockGralByHotel_Result = New spGetLockGralByHotel_Result
                    tempLock.StartDate = startDate
                    tempLock.EndDate = endDate
                    tempLock.Status = ddlStatus.SelectedValue
                    tempLock.ApplyWeek = apply
                    lockGral.Add(tempLock)


                    RequestLockGralGoogle(lockGral, activeRooms, activeRatePlans, confluxService)
                    RequestLockGralPromosGoogle(lockGral, activeRooms, activeRatePlansPromos, confluxService)

                End If

            Else
                'Plan Tarifario
                closeRateplan(nota, splan:=ddlRateplans.SelectedItem.Text)
                'closeRateplan(nota, splan:=ddlRateplans.SelectedItem.Text, hotelId:=info.Hotel, dates:=dates, confluxService:=confluxService, isEnabledGoogleRequest:=isEnabledGoogleRequest, activeRooms:=activeRooms, restrictionType:=RestrictionEnum.LockRatePlan)

                If isEnabledGoogleRequest Then
                    Dim dsRatePlansPromos As RatePlanData = New RatePlanFacade().GetRatePlanByIdHotel(info.Hotel.ToString(), idioma:=1, IncluirPaquetesSegmentoK:=1, incluirNetRatesPlan:=1, idAsociacion:=-1, DeleteFilter:=1, getPromos:=True)
                    Dim filterPromosDates As String = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"
                    Dim activeRatePlansPromos As List(Of DataRow) = dsRatePlansPromos.Tables(RatePlanData.RATEPLAN_TABLE).Select(filterPromosDates).ToList()

                    Dim promos As List(Of spGetPromosByRatePlan_Result) = HotelUtilitie.GetPromosByRatePlan(info.Hotel, ddlRateplans.SelectedValue)

                    Dim validPromosList As List(Of DataRow) = GetValidPromos(activeRatePlansPromos, promos)


                    Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = New List(Of spGetLockRatePlansByHotel_Result)

                    'Solo se usa el Modelo para las promos
                    Dim lockPromos As List(Of spGetLockGralByHotel_Result) = New List(Of spGetLockGralByHotel_Result)

                    'For Each [date] As Tuple(Of Date, Date) In dates

                    '    Dim tempLock As spGetLockGralByHotel_Result = New spGetLockGralByHotel_Result
                    '    tempLock.StartDate = [date].Item1
                    '    tempLock.EndDate = [date].Item2
                    '    tempLock.Status = ddlStatus.SelectedValue
                    '    lockPromos.Add(tempLock)
                    'Next

                    Dim tempLockPromos As spGetLockGralByHotel_Result = New spGetLockGralByHotel_Result
                    tempLockPromos.StartDate = startDate
                    tempLockPromos.EndDate = endDate
                    tempLockPromos.Status = ddlStatus.SelectedValue
                    tempLockPromos.ApplyWeek = apply
                    lockPromos.Add(tempLockPromos)


                    Dim tempLockRatePlans As spGetLockRatePlansByHotel_Result = New spGetLockRatePlansByHotel_Result
                    tempLockRatePlans.StartDate = startDate
                    tempLockRatePlans.EndDate = endDate
                    tempLockRatePlans.Status = ddlStatus.SelectedValue
                    tempLockRatePlans.RatePlanId = ddlRateplans.SelectedValue
                    tempLockRatePlans.ApplyWeek = apply
                    lockRatePlans.Add(tempLockRatePlans)

                    'RequestLockRatePlanGoogle(ddlStatus.SelectedValue, ddlRateplans.SelectedValue, dates, activeRooms, confluxService)
                    RequestLockRatePlanGoogle(lockRatePlans, activeRooms, confluxService)
                    RequestLockRatePlanPromosGoogle(lockPromos, activeRooms, validPromosList, confluxService)

                End If
            End If
            iniCtrl()
        End If
    End Sub

    Function Nota2(ByVal nota As String, ByRef sreference As String, ByVal splan As String) As String
        Dim msg As String = "Se modificó el rateplan " & splan & " con los siguientes datos: " & nota
        Dim drhotel As DataRow = Me.HotelInfo
        Dim idioma As String

        sreference = "Cambio de estatus por plan tarifario"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            msg = "rateplan " & splan & " was modified with the following data"
            sreference = "Update status by rate plan"
        End If
        Return msg
    End Function

    Function Nota3(ByVal nota As String, ByRef sReference As String) As String
        Dim msg As String = "Se modificó en la configuración gral. del hotel con los siguientes datos: " & nota
        Dim drhotel As DataRow = Me.HotelInfo
        Dim idioma As String

        sReference = "Cambio de estado del hotel "
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            msg = "Configuration was modified in gral. hotel with the following data: " & nota
            sReference = "Update status by hotel"
        End If
        Return msg
    End Function

    Sub Addrateplan(ByVal ds As lockRatePlanData, ByVal field As String, ByVal valor As String)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    Private Function closeRateplan(ByVal nota As String, Optional ByVal RatePlan As String = "", Optional ByVal loadStatusRateplan As Boolean = True, Optional ByVal splan As String = "",
                                   Optional ByVal hotelId As Integer = 0, Optional ByVal dates As List(Of Tuple(Of Date, Date)) = Nothing, Optional ByVal confluxService As ConfluxService = Nothing, Optional ByVal isEnabledGoogleRequest As Boolean = False,
                                   Optional ByVal activeRooms As List(Of DataRow) = Nothing, Optional ByVal restrictionType As RestrictionEnum = Nothing) As Boolean
        Dim ds As lockRatePlanData
        Dim strError As String
        Dim sDatosDespues As String
        Dim sDatos As String

        With New LockRatePlanFacade
            ds = .GetLockRatePlanList(IIf(RatePlan = "", ddlRateplans.SelectedValue, RatePlan), Me.cInfoActual.Hotel, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), False)
        End With

        Dim dstrans As lockRatePlanData = New lockRatePlanData
        Dim modrow As DataRow = Fill_LockRatePlanRow(dstrans.Tables(ds.TABLE_LockRatePlan), RatePlan)

        Dim rowStartDate, rowEndDate, txtStartDate, txtEndDate As Date
        txtStartDate = CDate(Me.txtInicio.Text)
        txtEndDate = CDate(Me.txtFinal.Text)
        If Not ds Is Nothing AndAlso ds.Tables(ds.TABLE_LockRatePlan).Rows.Count > 0 Then
            For i As Integer = 0 To ds.Tables(ds.TABLE_LockRatePlan).Rows.Count - 1
                With ds.Tables(ds.TABLE_LockRatePlan)
                    rowStartDate = CDate(.Rows(i).Item(ds.FIELD_StartDate))
                    rowEndDate = CDate(.Rows(i).Item(ds.FIELD_EndDate))
                    copyrowRP(.Rows(i), .Rows(i), rowStartDate, rowEndDate, dstrans, True)

                    If txtStartDate > rowStartDate Then
                        copyrowRP(.Rows(i), .Rows(i), rowStartDate, txtStartDate.AddDays(-1), dstrans, False)
                        If txtEndDate >= rowEndDate Then
                            copyrowRP(.Rows(i), modrow, txtStartDate, rowEndDate, dstrans, False, applyModRowWeek:=True)
                        Else
                            copyrowRP(.Rows(i), modrow, txtStartDate, txtEndDate, dstrans, False, applyModRowWeek:=True)
                            copyrowRP(.Rows(i), .Rows(i), txtEndDate.AddDays(1), rowEndDate, dstrans, False)
                        End If
                    Else
                        If txtEndDate >= rowEndDate Then
                            copyrowRP(.Rows(i), modrow, rowStartDate, rowEndDate, dstrans, False, applyModRowWeek:=True)
                        Else
                            copyrowRP(.Rows(i), modrow, rowStartDate, txtEndDate, dstrans, False, applyModRowWeek:=True)
                            copyrowRP(.Rows(i), .Rows(i), txtEndDate.AddDays(1), rowEndDate, dstrans, False)
                        End If
                    End If
                    'If .Rows(i).Item(ds.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
                    '    'YA ESTÁ ELIMINADA
                    'ElseIf .Rows(i).Item(ds.FIELD_StartDate) < CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
                    '    copyrowRP(.Rows(i), .Rows(i).Item(ds.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
                    'ElseIf .Rows(i).Item(ds.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) > CDate(Me.txtFinal.Text) Then
                    '    copyrowRP(.Rows(i), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(ds.FIELD_EndDate), dstrans, False)
                    'Else
                    '    copyrowRP(.Rows(i), ds.Tables(ds.TABLE_LockRatePlan).Rows(i).Item(ds.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
                    '    copyrowRP(.Rows(i), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(ds.FIELD_EndDate), dstrans, False)
                    'End If
                End With
            Next
            Dim rows As DataRow() = dstrans.Tables(ds.TABLE_LockRatePlan).Select("", ds.FIELD_StartDate & " asc")
            Dim progresDate As Date = txtStartDate
            For i As Integer = 0 To rows.Length - 1
                'Reutilizando variables
                rowStartDate = CDate(rows(i).Item(ds.FIELD_StartDate))
                rowEndDate = CDate(rows(i).Item(ds.FIELD_EndDate))

                If progresDate < rowStartDate AndAlso rows(i).RowState <> DataRowState.Deleted Then
                    copyrowRP(modrow, modrow, progresDate, rowStartDate.AddDays(-1), dstrans, False, applyModRowWeek:=True)
                End If

                progresDate = rowEndDate.AddDays(1)
            Next

            If progresDate <= txtEndDate Then
                copyrowRP(modrow, modrow, progresDate, txtEndDate, dstrans, False, applyModRowWeek:=True)
            End If

            rows = dstrans.Tables(ds.TABLE_LockRatePlan).Select("", ds.FIELD_StartDate & " asc")

            For i As Integer = 0 To rows.Length - 2

                If CDate(rows(i)(ds.FIELD_EndDate)).AddDays(1) = CDate(rows(i + 1)(ds.FIELD_StartDate)) _
                    AndAlso comparerow(rows(i), rows(i + 1)) AndAlso rows(i).RowState <> DataRowState.Deleted Then
                    rows(i + 1)(ds.FIELD_StartDate) = rows(i)(ds.FIELD_StartDate)
                    rows(i).Delete()
                End If

            Next
        Else
            copyrowRP(modrow, modrow, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), dstrans, False, applyModRowWeek:=True)

        End If


        Dim sDatoCorreo As String = ""
        Dim sReference As String = ""
        'dstrans.Tables(dstrans.TABLE_LockRatePlan).Rows.Add(row)
        With New LockRatePlanFacade
            If Not dstrans Is Nothing AndAlso dstrans.Tables(dstrans.TABLE_LockRatePlan).Rows.Count > 0 Then
                If .Update(dstrans) Then


                    Addrateplan(ds, "Descr_plan", splan)
                    sDatos = Util.Utility.GetXml(lockRatePlanData.TABLE_LockRatePlan, "UpdateLockRatePlan", ds)

                    Addrateplan(dstrans, "Descr_plan", splan)
                    sDatosDespues = Util.Utility.GetXml(lockRatePlanData.TABLE_LockRatePlan, "UpdateLockRatePlan", dstrans)
                    If Me.ddlStatus.SelectedValue = "C" Or Me.ddlStatus.SelectedValue = "O" Then
                        sDatoCorreo = CreateCloseRPHtml(ds, dstrans, splan)
                        Dim sNotas As String = Nota2(nota, sReference, splan)
                        sDatoCorreo = (New Util.Utility).GeneraCorreoXslt(sDatos, sDatosDespues)
                        Me.guardalog("/Pages/AvailabilityRestrictions.aspx", PaginaBase.acciones.Modificar, sNotas, sReference, sDatos, sDatosDespues, sDatoCorreo)
                    Else
                        sDatoCorreo = (New Util.Utility).GeneraCorreoXslt(sDatos, sDatosDespues)
                        Me.guardalog("/Pages/AvailabilityRestrictions.aspx", PaginaBase.acciones.Modificar, "Se modificó el rateplan " & splan & " con los siguientes datos: " & nota, "Update status by rate plan", sDatos, sDatosDespues, sDatoCorreo)
                    End If

                    'Try

                    '    If isEnabledGoogleRequest Then

                    '        Dim status As String = CType(dstrans.Tables(dstrans.TABLE_LockRatePlan).Rows(0).Item(lockRatePlanData.FIELD_StatusAvailability), String)
                    '        Dim ratePlanId As String = CType(dstrans.Tables(dstrans.TABLE_LockRatePlan).Rows(0).Item(lockRatePlanData.FIELD_RatePlan), String)

                    '        Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, status, ratePlanId)

                    '        Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(activeRooms, lockRatePlans)

                    '        Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

                    '        Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

                    '        Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, restrictionType)


                    '        If Not restrictionResponse.IsSuccess Then

                    '            If restrictionType = RestrictionEnum.LockGral Then
                    '                MyBase.WriteLog(restrictionResponse.Xml, "LockGral")
                    '            ElseIf restrictionType = RestrictionEnum.LockRatePlan Then
                    '                MyBase.WriteLog(restrictionResponse.Xml, "LockRatePlan")
                    '            End If

                    '        ElseIf restrictionResponse.IsSuccess Then
                    '            For Each restriction As Restriction In restrictionResponse.Restrictions
                    '                Select Case restriction.Type
                    '                    Case RestrictionEnum.LockRatePlan
                    '                        MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockRatePlan")
                    '                        MyBase.WriteLog(restriction.Xml(0).ToString(), "LockRatePlan")
                    '                    Case RestrictionEnum.LockGral
                    '                        MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockGral")
                    '                        MyBase.WriteLog(restriction.Xml(0).ToString(), "LockGral")
                    '                End Select
                    '            Next

                    '        End If

                    '    End If 'Termina Google

                    'Catch ex As Exception

                    '    If restrictionType = RestrictionEnum.LockGral Then
                    '        MyBase.WriteLog(ex.Message, "LockGral")
                    '    ElseIf restrictionType = RestrictionEnum.LockRatePlan Then
                    '        MyBase.WriteLog(ex.Message, "LockRatePlan")
                    '    End If
                    'End Try

                End If
            End If
        End With
        If loadStatusRateplan Then
            RdHotel.Checked = False
            RdRatePlan.Checked = True

            Dim dsRatePlan As RatePlanData
            With New RatePlanFacade
                dsRatePlan = .GetDataRatePlan(ddlRateplans.SelectedValue, Me.cInfoActual.Hotel)
            End With
            If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
                DlRatePlan.SelectedValue = dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0).Item(dsRatePlan.FIELD_CODIGOTARIFA)
            End If
        End If

    End Function

    Private Function CreateCloseRPHtml(ByVal ds As lockRatePlanData, ByVal dstrans As lockRatePlanData, ByVal rateplan As String) As String
        Dim menu As New Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim dv As New DataView
        Dim sw As StringWriter = New StringWriter
        Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim shtml As String = ""
        Dim drhotel As DataRow = Me.HotelInfo

        menu.CellSpacing = 1
        menu.CellPadding = 1
        ' menu.BorderWidth = 1
        menu.Width = New System.Web.UI.WebControls.Unit(600, UnitType.Pixel)
        Dim idioma As String
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))


        tr = New TableRow
        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00016", idioma) '"Rate Plan"  
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00283", idioma) '"StatusAvailability"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000192", idioma) '"StartDate " 
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000193", idioma) '"EndDate"   
        tr.Cells.Add(td)
        menu.Rows.Add(tr)


        For Each dr As DataRow In ds.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            Dim sta As String
            If Not dr.IsNull("StatusAvailability") Then


                Select Case dr("StatusAvailability")
                    Case "O"
                        sta = "Open"
                    Case "C"
                        sta = "Close"
                    Case Else
                        sta = "Undefined"
                End Select

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = rateplan
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = sta
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("StartDate")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("EndDate")
                tr.Cells.Add(td)
                menu.Rows.Add(tr)
            End If
        Next

        For Each dr As DataRow In dstrans.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            Dim sta As String
            If Not dr.IsNull("StatusAvailability") Then
                Select Case dr("StatusAvailability")
                    Case "O"
                        sta = "Open"
                    Case "C"
                        sta = "Close"
                    Case Else
                        sta = "Undefined"
                End Select

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = rateplan
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = sta
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("StartDate")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("EndDate")
                tr.Cells.Add(td)
                menu.Rows.Add(tr)
            End If
        Next

        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
    End Function



    Private Function closehotel(ByVal nota As String) As Boolean
        Dim ds As clsCommonAvailibilityGral
        Dim strError As String
        Dim sDatosDespues As String
        Dim sdatos As String = ""
        Dim sAvail As String = ""
        With New ClsFacadeAvailibilityGral
            ds = .LockGralBuscarTransaccion(Me.cInfoActual.Hotel, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), strError)
        End With
        Dim dstrans, Data As clsCommonAvailibilityGral
        dstrans = New clsCommonAvailibilityGral
        Data = FillData(sAvail)
        If Not ds Is Nothing Then
            If ds.Tables(ds.TABLE_LockGral).Rows.Count > 0 Then
                'sdatos = ds.GetXml.ToString

                Dim rowStartDate, rowEndDate, txtStartDate, txtEndDate As Date
                txtStartDate = CDate(Me.txtInicio.Text)
                txtEndDate = CDate(Me.txtFinal.Text)
                For i As Integer = 0 To ds.Tables(ds.TABLE_LockGral).Rows.Count - 1
                    With ds.Tables(ds.TABLE_LockGral)
                        'sAvail = .Rows(i).Item(ds.FIELD_statusAvailability)
                        rowStartDate = CDate(.Rows(i).Item(ds.FIELD_StartDate))
                        rowEndDate = CDate(.Rows(i).Item(ds.FIELD_EndDate))

                        copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), rowStartDate, rowEndDate, dstrans, True)

                        If txtStartDate > rowStartDate Then
                            copyrow(.Rows(i), .Rows(i), rowStartDate, txtStartDate.AddDays(-1), dstrans, False)
                            If txtEndDate >= rowEndDate Then
                                copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), txtStartDate, rowEndDate, dstrans, False)
                            Else
                                copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), txtStartDate, txtEndDate, dstrans, False)
                                copyrow(.Rows(i), .Rows(i), txtEndDate.AddDays(1), rowEndDate, dstrans, False)
                            End If
                        Else
                            If txtEndDate >= rowEndDate Then
                                copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), rowStartDate, rowEndDate, dstrans, False)
                            Else
                                copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), rowStartDate, txtEndDate, dstrans, False)
                                copyrow(.Rows(i), .Rows(i), txtEndDate.AddDays(1), rowEndDate, dstrans, False)
                            End If
                        End If

                        '.Rows(i).Item(ds.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
                        'YA ESTÁ ELIMINADA
                        'ElseIf .Rows(i).Item(ds.FIELD_StartDate) < CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
                        '    copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), .Rows(i).Item(ds.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
                        'ElseIf .Rows(i).Item(ds.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(ds.FIELD_EndDate) > CDate(Me.txtFinal.Text) Then
                        '    copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(ds.FIELD_EndDate), dstrans, False)
                        'Else
                        '    copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), ds.Tables(ds.TABLE_LockGral).Rows(i).Item(ds.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
                        '    copyrow(.Rows(i), Data.Tables(Data.TABLE_LockGral).Rows(0), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(ds.FIELD_EndDate), dstrans, False)
                        'End If
                    End With
                Next
                Dim rows As DataRow() = dstrans.Tables(ds.TABLE_LockGral).Select("", ds.FIELD_StartDate & " asc")
                Dim progresDate As Date = txtStartDate
                For i As Integer = 0 To rows.Length - 1
                    'Reutilizando variables
                    rowStartDate = CDate(rows(i).Item(ds.FIELD_StartDate))
                    rowEndDate = CDate(rows(i).Item(ds.FIELD_EndDate))

                    If progresDate < rowStartDate AndAlso rows(i).RowState <> DataRowState.Deleted Then
                        copyrow(Data.Tables(Data.TABLE_LockGral).Rows(0), Data.Tables(Data.TABLE_LockGral).Rows(0), progresDate, rowStartDate.AddDays(-1), dstrans, False)
                    End If

                    progresDate = rowEndDate.AddDays(1)
                Next

                If progresDate <= txtEndDate Then
                    copyrow(Data.Tables(Data.TABLE_LockGral).Rows(0), Data.Tables(Data.TABLE_LockGral).Rows(0), progresDate, txtEndDate, dstrans, False)
                End If

                rows = dstrans.Tables(ds.TABLE_LockGral).Select("", ds.FIELD_StartDate & " asc")

                For i As Integer = 0 To rows.Length - 2

                    If CDate(rows(i)(ds.FIELD_EndDate)).AddDays(1) = CDate(rows(i + 1)(ds.FIELD_StartDate)) _
                        AndAlso comparerow(rows(i), rows(i + 1)) AndAlso rows(i).RowState <> DataRowState.Deleted Then
                        rows(i + 1)(ds.FIELD_StartDate) = rows(i)(ds.FIELD_StartDate)
                        rows(i).Delete()
                    End If

                Next
            ElseIf Not Data Is Nothing AndAlso Data.Tables(Data.TABLE_LockGral).Rows.Count > 0 Then
                copyrow(Data.Tables(Data.TABLE_LockGral).Rows(0), Data.Tables(Data.TABLE_LockGral).Rows(0), CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), dstrans, False)
            End If
        End If


        Dim sDatosCorreo As String = ""
        Dim sReference As String = ""
        With New ClsFacadeAvailibilityGral
            If Not dstrans Is Nothing AndAlso dstrans.Tables(dstrans.TABLE_LockGral).Rows.Count > 0 Then
                If .updateLocks(dstrans) Then
                    sdatos = Util.Utility.GetXml(dstrans.TABLE_LockGral, "UpdateLockGeneral", ds)
                    sDatosDespues = Util.Utility.GetXml(dstrans.TABLE_LockGral, "UpdateLockGeneral", dstrans)
                    If Me.ddlStatus.SelectedValue = "C" Or Me.ddlStatus.SelectedValue = "O" Then
                        sDatosCorreo = CreateCloseHotelHtml(ds, dstrans)
                        Dim sNotas As String = Nota3(nota, sReference)
                        sDatosCorreo = (New Util.Utility).GeneraCorreoXslt(sdatos, sDatosDespues)
                        Me.guardalog("/Pages/AvailabilityRestrictions.aspx", PaginaBase.acciones.Modificar, sNotas, "Update status by hotel", sdatos, sDatosDespues, sDatosCorreo)
                        'Me.guardalog("/Pages/AvailabilityRestrictions.aspx", PaginaBase.acciones.Modificar, "Se modificó en la configuración gral. del hotel con los siguientes datos: " & nota, "Update status by hotel", sdatos, sDatosDespues, sDatosCorreo)
                    Else
                        sDatosCorreo = (New Util.Utility).GeneraCorreoXslt(sdatos, sDatosDespues)
                        Me.guardalog("/Pages/AvailabilityRestrictions.aspx", PaginaBase.acciones.Modificar, "Se modificó en la configuración gral. del hotel con los siguientes datos: " & nota, sReference, sdatos, sDatosDespues, sDatosCorreo)
                    End If

                End If
            End If
        End With
        RdHotel.Checked = True
        ddlRateplans.Attributes.Add("display", "none")
        chkOnRequest.Attributes.Add("display", "")
        RdRatePlan.Checked = False
    End Function

    Private Function CreateCloseHotelHtml(ByVal ds As clsCommonAvailibilityGral, ByVal dstrans As clsCommonAvailibilityGral) As String
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
        td.Text = PortalCulture.GetString("00283", idioma) '"StatusAvailability"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000192", idioma) '"StartDate " 
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000193", idioma) '"EndDate"   
        tr.Cells.Add(td)
        menu.Rows.Add(tr)

        For Each dr As DataRow In ds.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            Dim sta As String
            If Not dr.IsNull("StatusAvailability") Then
                Select Case dr("StatusAvailability")
                    Case "O"
                        sta = "Open"
                    Case "C"
                        sta = "Close"
                    Case Else
                        sta = "Undefined"
                End Select

                td.Attributes.Add("class", "dow")
                td.Text = sta
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("StartDate")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("EndDate")
                tr.Cells.Add(td)
                menu.Rows.Add(tr)
            End If
        Next

        For Each dr As DataRow In dstrans.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            Dim sta As String
            If Not dr.IsNull("StatusAvailability") Then
                Select Case dr("StatusAvailability")
                    Case "O"
                        sta = "Open"
                    Case "C"
                        sta = "Close"
                    Case Else
                        sta = "Undefined"
                End Select

                td.Attributes.Add("class", "dow")
                td.Text = sta
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("StartDate")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = dr("EndDate")
                tr.Cells.Add(td)
                menu.Rows.Add(tr)
            End If
        Next

        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
    End Function


    Private Function copyrow(ByVal row As DataRow, ByVal modRow As DataRow, ByVal fechainicio As DateTime, ByVal fechafinal As DateTime, ByVal cAva As clsCommonAvailibilityGral, ByVal borrado As Boolean)
        Dim newrow As DataRow
        newrow = cAva.Tables(cAva.TABLE_LockGral).NewRow
        With newrow
            .Item(cAva.FIELD_idHotel) = row.Item(cAva.FIELD_idHotel)
            .Item(cAva.FIELD_StartDate) = fechainicio
            .Item(cAva.FIELD_EndDate) = fechafinal
            '.Item(cAva.FIELD_minLengthStay) = row.Item(cAva.FIELD_minLengthStay)
            '.Item(cAva.FIELD_RoomsAvailable) = row.Item(cAva.FIELD_RoomsAvailable)
            .Item(cAva.FIELD_statusAvailability) = IIf(chkEstatus.Checked, modRow.Item(cAva.FIELD_statusAvailability), row.Item(cAva.FIELD_statusAvailability))
            '.Item(cAva.FIELD_HurdleRate) = row.Item(cAva.FIELD_HurdleRate)
            '.Item(cAva.FIELD_CancelPrioridad) = row.Item(cAva.FIELD_CancelPrioridad)
            '.Item(cAva.FIELD_DepartureRestricted) = row.Item(cAva.FIELD_DepartureRestricted)
            .Item(cAva.FIELD_OnRequest) = modRow.Item(cAva.FIELD_OnRequest)
            .Item(cAva.FIELD_AplyWeek) = modRow.Item(cAva.FIELD_AplyWeek)
            .Item(cAva.FIELD_MinDias) = IIf(chkMindays.Checked, modRow.Item(cAva.FIELD_MinDias), row.Item(cAva.FIELD_MinDias))
            .Item(cAva.FIELD_MaxDias) = IIf(chkMaxDays.Checked, modRow.Item(cAva.FIELD_MaxDias), row.Item(cAva.FIELD_MaxDias))
            .Item(cAva.FIELD_AdvBooking) = IIf(chkAdvBook.Checked, modRow.Item(cAva.FIELD_AdvBooking), row.Item(cAva.FIELD_AdvBooking))

            If chkPriorCancel.Checked Then
                .Item(cAva.FIELD_CancelPriorHours) = IIf(ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour, modRow.Item(cAva.FIELD_CancelPriorHours), row.Item(cAva.FIELD_CancelPriorHours))
                .Item(cAva.FIELD_CancelPriorDays) = IIf(ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays, modRow.Item(cAva.FIELD_CancelPriorDays), row.Item(cAva.FIELD_CancelPriorDays))
                .Item(cAva.FIELD_CancelPriorSpecificT) = IIf(ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour, modRow.Item(cAva.FIELD_CancelPriorSpecificT), row.Item(cAva.FIELD_CancelPriorSpecificT))
            Else
                .Item(cAva.FIELD_CancelPriorHours) = row.Item(cAva.FIELD_CancelPriorHours)
                .Item(cAva.FIELD_CancelPriorDays) = row.Item(cAva.FIELD_CancelPriorDays)
                .Item(cAva.FIELD_CancelPriorSpecificT) = row.Item(cAva.FIELD_CancelPriorSpecificT)
            End If

            '.Item(cAva.FIELD_MinPrice) = row.Item(cAva.FIELD_MinPrice)
            '.Item(cAva.FIELD_MaxPrice) = row.Item(cAva.FIELD_MaxPrice)


            cAva.Tables(cAva.TABLE_LockGral).Rows.Add(newrow)
            If borrado = True Then
                newrow.AcceptChanges()
                newrow.Delete()
            End If
        End With
    End Function
    Private Function comparerow(ByVal row1 As DataRow, ByVal row2 As DataRow) As Boolean
        'And row1(clsCommonAvailibilityGral.FIELD_OnRequest).Equals(row2(clsCommonAvailibilityGral.FIELD_OnRequest)) _
        Return row1(clsCommonAvailibilityGral.FIELD_statusAvailability).Equals(row2(clsCommonAvailibilityGral.FIELD_statusAvailability)) _
            And row1(clsCommonAvailibilityGral.FIELD_AplyWeek).Equals(row2(clsCommonAvailibilityGral.FIELD_AplyWeek)) _
            And row1(clsCommonAvailibilityGral.FIELD_MinDias).Equals(row2(clsCommonAvailibilityGral.FIELD_MinDias)) _
            And row1(clsCommonAvailibilityGral.FIELD_MaxDias).Equals(row2(clsCommonAvailibilityGral.FIELD_MaxDias)) _
            And row1(clsCommonAvailibilityGral.FIELD_AdvBooking).Equals(row2(clsCommonAvailibilityGral.FIELD_AdvBooking)) _
            And row1(clsCommonAvailibilityGral.FIELD_CancelPriorHours).Equals(row2(clsCommonAvailibilityGral.FIELD_CancelPriorHours)) _
            And row1(clsCommonAvailibilityGral.FIELD_CancelPriorDays).Equals(row2(clsCommonAvailibilityGral.FIELD_CancelPriorDays)) _
            And row1(clsCommonAvailibilityGral.FIELD_CancelPriorSpecificT).Equals(row2(clsCommonAvailibilityGral.FIELD_CancelPriorSpecificT))

    End Function
    Private Function copyrowRP(ByVal row As DataRow, ByVal modrow As DataRow, ByVal fechainicio As DateTime, ByVal fechafinal As DateTime, ByVal cAva As lockRatePlanData, ByVal borrado As Boolean, Optional ByVal applyModRowWeek As Boolean = False)
        Dim newrow As DataRow
        newrow = cAva.Tables(cAva.TABLE_LockRatePlan).NewRow
        With newrow
            .Item(cAva.FIELD_idHotel) = row.Item(cAva.FIELD_idHotel)
            .Item(cAva.FIELD_StartDate) = fechainicio
            .Item(cAva.FIELD_EndDate) = fechafinal
            .Item(cAva.FIELD_StatusAvailability) = IIf(chkEstatus.Checked, modrow.Item(cAva.FIELD_StatusAvailability), row.Item(cAva.FIELD_StatusAvailability))
            .Item(cAva.FIELD_RatePlan) = row.Item(cAva.FIELD_RatePlan)
            .Item(cAva.FIELD_AplyWeek) = IIf(applyModRowWeek, modrow.Item(cAva.FIELD_AplyWeek), row.Item(cAva.FIELD_AplyWeek))
            .Item(cAva.FIELD_MinDias) = IIf(chkMindays.Checked, modrow.Item(cAva.FIELD_MinDias), row.Item(cAva.FIELD_MinDias))
            .Item(cAva.FIELD_MaxDias) = IIf(chkMaxDays.Checked, modrow.Item(cAva.FIELD_MaxDias), row.Item(cAva.FIELD_MaxDias))

            .Item(cAva.FIELD_AdvBooking) = IIf(chkAdvBook.Checked, modrow.Item(cAva.FIELD_AdvBooking), row.Item(cAva.FIELD_AdvBooking))

            If chkPriorCancel.Checked Then
                .Item(cAva.FIELD_CancelPriorHours) = IIf(ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour, modrow.Item(cAva.FIELD_CancelPriorHours), row.Item(cAva.FIELD_CancelPriorHours))
                .Item(cAva.FIELD_CancelPriorDays) = IIf(ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays, modrow.Item(cAva.FIELD_CancelPriorDays), row.Item(cAva.FIELD_CancelPriorDays))
                .Item(cAva.FIELD_CancelPriorSpecificT) = IIf(ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour, modrow.Item(cAva.FIELD_CancelPriorSpecificT), row.Item(cAva.FIELD_CancelPriorSpecificT))
            Else
                .Item(cAva.FIELD_CancelPriorHours) = row.Item(cAva.FIELD_CancelPriorHours)
                .Item(cAva.FIELD_CancelPriorDays) = row.Item(cAva.FIELD_CancelPriorDays)
                .Item(cAva.FIELD_CancelPriorSpecificT) = row.Item(cAva.FIELD_CancelPriorSpecificT)
            End If


            cAva.Tables(cAva.TABLE_LockRatePlan).Rows.Add(newrow)
            If borrado = True Then
                newrow.AcceptChanges()
                newrow.Delete()
            End If

        End With
    End Function



    Private Function FillData(ByVal sAvail As String) As clsCommonAvailibilityGral
        Dim NewDs As clsCommonAvailibilityGral = New clsCommonAvailibilityGral
        Dim row As DataRow
        row = NewDs.Tables(NewDs.TABLE_LockGral).NewRow
        With row
            .Item(NewDs.FIELD_idHotel) = Me.cInfoActual.Hotel
            .Item(NewDs.FIELD_StartDate) = CDate(Me.txtInicio.Text)
            .Item(NewDs.FIELD_EndDate) = CDate(Me.txtFinal.Text)
            .Item(NewDs.FIELD_OnRequest) = "Y"
            If Me.chkMindays.Checked And txtmindays.Text <> "" Then
                .Item(NewDs.FIELD_MinDias) = CInt(Val(txtmindays.Text))
            End If
            If Me.chkMaxDays.Checked And txtMaxdays.Text <> "" Then
                .Item(NewDs.FIELD_MaxDias) = CInt(Val(txtMaxdays.Text))
            End If
            If Me.chkAdvBook.Checked And txtAdvBook.Text <> "" Then
                .Item(NewDs.FIELD_AdvBooking) = CInt(Val(txtAdvBook.Text))
            End If

            'If Me.chkMinPrice.Checked AndAlso Me.txtMinPrice.Text <> "" Then
            '    .Item(NewDs.FIELD_MinPrice) = CInt(Val(txtMinPrice.Text))
            'End If
            'If Me.chkMaxPrice.Checked AndAlso Me.txtMaxPrice.Text <> "" Then
            '    .Item(NewDs.FIELD_MaxPrice) = CInt(Val(txtMaxPrice.Text))
            'End If

            If Not Me.chkOnRequest.Checked Then
                .Item(NewDs.FIELD_OnRequest) = "N"
            End If
            If Not Me.chkEstatus.Checked Or Me.ddlStatus.SelectedValue = "S" Then
                .Item(NewDs.FIELD_statusAvailability) = DBNull.Value
            Else
                .Item(NewDs.FIELD_statusAvailability) = Me.ddlStatus.SelectedValue
            End If
            'Else
            '    .Item(NewDs.FIELD_statusAvailability) = Me.ddlStatus.Items(0).Value
            .Item(NewDs.FIELD_AplyWeek) = GetAplyWeek()


            If chkPriorCancel.Checked Then
                Dim prioCancel As String = IIf(txtCancellationPolicy.Text = String.Empty, "0", txtCancellationPolicy.Text)
                If ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays Then
                    .Item(NewDs.FIELD_CancelPriorDays) = prioCancel
                ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour Then
                    .Item(NewDs.FIELD_CancelPriorHours) = prioCancel
                ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour Then
                    .Item(NewDs.FIELD_CancelPriorSpecificT) = ddlHour.SelectedItem.Text & ddlMinutes.SelectedItem.Text
                End If
            End If


        End With
        NewDs.Tables(NewDs.TABLE_LockGral).Rows.Add(row)
        NewDs.AcceptChanges()
        NewDs.Tables(NewDs.TABLE_LockGral).Rows(0).Item(0) = NewDs.Tables(NewDs.TABLE_LockGral).Rows(0).Item(0)
        Return NewDs
    End Function

    Private Function Fill_LockRatePlanRow(ByRef dtTrans As DataTable, ByVal RatePlan As String) As DataRow
        Dim row As DataRow
        row = dtTrans.NewRow
        With row
            .Item(lockRatePlanData.FIELD_idHotel) = Me.cInfoActual.Hotel
            .Item(lockRatePlanData.FIELD_StartDate) = CDate(Me.txtInicio.Text)
            .Item(lockRatePlanData.FIELD_EndDate) = CDate(Me.txtFinal.Text)
            .Item(lockRatePlanData.FIELD_RatePlan) = IIf(RatePlan = "", ddlRateplans.SelectedValue, RatePlan)
            If Not chkEstatus.Checked Or Me.ddlStatus.SelectedValue = "S" Then
                .Item(lockRatePlanData.FIELD_StatusAvailability) = DBNull.Value
            Else
                .Item(lockRatePlanData.FIELD_StatusAvailability) = Me.ddlStatus.SelectedValue
            End If

            .Item(lockRatePlanData.FIELD_AplyWeek) = GetAplyWeek()
            If Me.chkMindays.Checked And txtmindays.Text <> "" Then
                .Item(lockRatePlanData.FIELD_MinDias) = CInt(Val(txtmindays.Text))
            End If
            If Me.chkMaxDays.Checked And txtMaxdays.Text <> "" Then
                .Item(lockRatePlanData.FIELD_MaxDias) = CInt(Val(txtMaxdays.Text))
            End If
            If Me.chkAdvBook.Checked And txtAdvBook.Text <> "" Then
                .Item(lockRatePlanData.FIELD_AdvBooking) = CInt(Val(txtAdvBook.Text))
            End If


            If chkPriorCancel.Checked Then
                Dim prioCancel As String = IIf(txtCancellationPolicy.Text = String.Empty, "0", txtCancellationPolicy.Text)
                If ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays Then
                    .Item(lockRatePlanData.FIELD_CancelPriorDays) = prioCancel
                ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour Then
                    .Item(lockRatePlanData.FIELD_CancelPriorHours) = prioCancel
                ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour Then
                    .Item(lockRatePlanData.FIELD_CancelPriorSpecificT) = Trim(ddlHour.SelectedItem.Text) & Trim(ddlMinutes.SelectedItem.Text)
                End If
            End If

        End With

        Return row
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

    Enum eStatus
        c = 1
        n = 2
        nr = 4
        na = 8
        o = 16
    End Enum

    Private Sub LoadDataByRP()
        Dim _month As Integer = SelectedMes
        Dim _year As Integer = SelectedYear
        Dim selectDate, EndDate As Date, FirstDay As Integer
        Dim data As DataSet
        Dim dt As DataTable
        Dim arStatus(5) As Byte
        Me.StatusHotel = "O"

        selectDate = New Date(CDbl(SelectedYear), SelectedMes, 1)
        EndDate = selectDate.AddMonths(3)
        EndDate.AddDays(Date.DaysInMonth(EndDate.Year, EndDate.Month) - 1)

        'With New RoomsInventoryFacade
        '    data = .GetLocksAndAvailByRP(Me.cInfoActual.Hotel, selectDate, EndDate, Me.DlRatePlan.SelectedValue)
        'End With
        'If data.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 AndAlso Not data.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_STATUSAVAILABILITY) Is System.DBNull.Value Then
        '    Me.StatusHotel = data.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_STATUSAVAILABILITY)
        'End If

        With New clsGetAvail
            If RdHotel.Checked Then
                dt = .GetAvail(selectDate, EndDate, MyBase.cInfoActual.Hotel, "")
                If DlSegment.SelectedValue > 0 Then
                    Dim segPublicos As String = System.Configuration.ConfigurationManager.AppSettings("segmentosPublicos")
                    Dim dtSegmented As DataTable = dt.Copy()
                    If DlSegment.SelectedValue = 1 Then
                        Dim deleteRow() As DataRow = dt.Select("Segment Not IN (" & segPublicos & ")")
                        For Each delRow As DataRow In deleteRow
                            dt.Rows.Remove(delRow)
                        Next
                    Else
                        Dim deleteRow() As DataRow = dt.Select("Segment IN (" & segPublicos & ")")
                        For Each delRow As DataRow In deleteRow
                            dt.Rows.Remove(delRow)
                        Next
                    End If
                    dt.AcceptChanges()
                End If
            Else
                dt = .GetAvail(selectDate, EndDate, MyBase.cInfoActual.Hotel, DlRatePlan.SelectedValue)
            End If
        End With

        Dim dv As DataView, dvRP As DataView, dvLH As DataView
        Dim row, col As Integer

        Dim drRatePlanDate() As DataRow

        For i As Integer = 1 To 3
            _month = SelectedMes + i - 1
            If _month > 12 Then
                _month = _month - 12
                _year = SelectedYear + 1
            End If
            selectDate = New Date(_year, _month, 1)
            EndDate = New Date(_year, _month, Date.DaysInMonth(_year, _month))
            FirstDay = Weekday(selectDate, FirstDayOfWeek.Sunday)

            Dim tbl As HtmlTable, lbl As Label
            tbl = Me.FindControl("tblMonth" & i.ToString)
            lbl = Me.FindControl("lblMonth" & i.ToString)
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            lbl.Text = MonthName(_month)
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            tbl.Rows(0).Attributes.Add("class", "clsSmallDarkLabel")
            tbl.Rows(1).Attributes.Add("class", "clsHeadTitleSmall")
            tbl.Rows(1).Cells(0).InnerText = PortalCulture.GetString("00306")
            tbl.Rows(1).Cells(1).InnerText = PortalCulture.GetString("00300")
            tbl.Rows(1).Cells(2).InnerText = PortalCulture.GetString("00301")
            tbl.Rows(1).Cells(3).InnerText = PortalCulture.GetString("00302")
            tbl.Rows(1).Cells(4).InnerText = PortalCulture.GetString("00303")
            tbl.Rows(1).Cells(5).InnerText = PortalCulture.GetString("00304")
            tbl.Rows(1).Cells(6).InnerText = PortalCulture.GetString("00305")

            For row = 1 To 6
                tbl.Rows(row + 1).Attributes.Add("class", "clsSmallLabel")
                tbl.Rows(row + 1).EnableViewState = False
                For col = 0 To 6

                    With tbl.Rows(row + 1).Cells(col)

                        .EnableViewState = False
                        '.InnerText = ((row - 1) * 7) - FirstDay + col + 2                                            
                        'If CDbl(.InnerText) <= 0 Or CInt(.InnerText) > Date.DaysInMonth(_year, _month) Then
                        If CDbl(((row - 1) * 7) - FirstDay + col + 2) <= 0 Or CInt(((row - 1) * 7) - FirstDay + col + 2) > Date.DaysInMonth(_year, _month) Then
                            '.InnerText = ""
                            .InnerHtml = ""
                            .BgColor = "gainsboro"
                        Else
                            '.InnerHtml = "<asp:LinkButton id='LinkButton1' runat='server' CommandName=' " & (((row - 1) * 7) - FirstDay + col + 2).ToString & "'>" & (((row - 1) * 7) - FirstDay + col + 2).ToString & "<asp:LinkButton>"
                            lnk = New LinkButton
                            lnk.Text = (((row - 1) * 7) - FirstDay + col + 2).ToString
                            lnk.CommandName = "#" & New Date(_year, _month, CInt(((row - 1) * 7) - FirstDay + col + 2)).ToString("M/dd/yy") & "#"
                            lnk.Attributes.Add("onclick", "javascript:return LoadRules('" & New Date(_year, _month, CInt(((row - 1) * 7) - FirstDay + col + 2)).ToString("yyyy/MM/dd") & "')")

                            .Controls.Add(lnk)
                            AddHandler lnk.Click, AddressOf LinkButton1_Click

                            Dim available As String = ""
                            'Dim _d As Date = New Date(_year, _month, CInt(.InnerText))
                            Dim _d As Date = New Date(_year, _month, CInt(((row - 1) * 7) - FirstDay + col + 2))

                            drRatePlanDate = dt.Select(
                                " fecha =#" & _d.ToString("M/dd/yy") & "#")

                            Dim flagStatus As eStatus
                            If drRatePlanDate.Length > 0 Then

                                flagStatus = 0
                                For Each dr As DataRow In drRatePlanDate
                                    Select Case dr("avail")
                                        Case "C"
                                            flagStatus = flagStatus Or eStatus.c
                                        Case "N"
                                            flagStatus = flagStatus Or eStatus.n
                                        Case "NR"
                                            flagStatus = flagStatus Or eStatus.nr
                                        Case "NA"
                                            flagStatus = flagStatus Or eStatus.na
                                        Case "O"
                                            flagStatus = flagStatus Or eStatus.o
                                    End Select
                                    'Select Case dr("avail")
                                    '    Case "C", "N", "NR", "NA" ' Close
                                    '        available = dr("avail")
                                    '    Case "O"
                                    '        available = "O"
                                    '        Exit For
                                    'End Select
                                Next

                                If ((flagStatus And eStatus.o) = eStatus.o) Then
                                    available = "O"
                                ElseIf ((flagStatus And eStatus.n) = eStatus.n) Then
                                    available = "N"
                                ElseIf ((flagStatus And eStatus.c) = eStatus.c) Then
                                    available = "C"
                                ElseIf ((flagStatus And eStatus.na) = eStatus.na) Then
                                    available = "NA"
                                ElseIf ((flagStatus And eStatus.nr) = eStatus.nr) Then
                                    available = "NR"
                                Else
                                    available = "O"
                                End If
                                .BgColor = color(available)
                            Else
                                '' no rates
                                .BgColor = color("NR")
                            End If
                        End If
                    End With
                Next
            Next
        Next
    End Sub
    Private Sub iniCtrl()
        Try
            lblSep.Style.Add("display", "none")
            ddlHour.Style.Add("display", "none")
            ddlMinutes.Style.Add("display", "none")
            ddlCancelationPolicy.SelectedIndex = 0
            txtCancellationPolicy.Text = ""
            chkPriorCancel.Checked = False
            chkEstatus.Checked = False
            If ddlStatus.Items.Count > 0 Then
                ddlStatus.SelectedIndex = 0
            End If

            chkMindays.Checked = False
            chkMaxDays.Checked = False
            chkAdvBook.Checked = False
            'Me.chkMaxPrice.Checked = False
            'Me.chkMinPrice.Checked = False
            txtmindays.Text = ""
            txtMaxdays.Text = ""
            txtAdvBook.Text = ""
            'Me.txtMaxPrice.Text = ""
            'Me.txtMinPrice.Text = ""
            chkApplyAllPlan.Checked = False
            If RbdHotel.Checked = True Then
                divApplyAllPlan.Style.Add("display", "")
            Else
                divApplyAllPlan.Style.Add("display", "none")
            End If
            If ddlRateplans.Items.Count > 0 Then
                ddlRateplans.SelectedIndex = 0
            End If

        Catch ex As Exception

        End Try
        Dim algo As String


    End Sub

    Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim comand As String = String.Empty
        Dim lbt As LinkButton = CType(sender, LinkButton)
        comand = lbt.CommandName

        'Cargamos los datos
        'Dim Script As String = String.Empty
        'CtlMensajeRuleConf1.LoadRules(lbt.CommandName, MyBase.cInfoActual.Hotel)
        'Script = CtlMensajeRuleConf1.getShow("", PortalCulture.GetString("01091"), PortalCulture.GetString("01092"))
        'Page.RegisterStartupScript("UrlScriptShow", "<script type='text/javascript'>" & Script & ";</script>")

    End Sub

#Region "Google"

    Public Sub RequestLockGralGoogle(ByVal lockGral As List(Of spGetLockGralByHotel_Result), ByVal activeRooms As List(Of DataRow), ByVal activeRatePlans As List(Of DataRow), ByVal confluxService As ConfluxService)

        Try

            Dim availStatusMessagesLockGralNoPromos As AvailStatusMessages = RestrictionsParser.ToAvailStatusMessages(lockGral, activeRooms, activeRatePlans)
            Dim lockGralNoPromosHotelAvailNotifRQ As XElement = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockGralNoPromos)
            Dim lockGralNoPromosSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockGralNoPromosHotelAvailNotifRQ)

            Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockGralNoPromosSoapRQ, restrictionEnum:=RestrictionEnum.LockGral)

            If Not restrictionResponse.IsSuccess Then
                MyBase.WriteLog(restrictionResponse.Xml, "LockGral")
            ElseIf restrictionResponse.IsSuccess Then
                For Each restriction As Restriction In restrictionResponse.Restrictions
                    Select Case restriction.Type
                        Case RestrictionEnum.LockGral
                            MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockGral")
                            MyBase.WriteLog(restriction.Xml(0).ToString(), "LockGral")
                    End Select
                Next
            End If
        Catch ex As Exception
            MyBase.WriteLog(ex.Message, "LockGral")
        End Try

    End Sub

    Public Sub RequestLockGralPromosGoogle(ByVal lockGral As List(Of spGetLockGralByHotel_Result), ByVal activeRooms As List(Of DataRow), ByVal activeRatePlansPromos As List(Of DataRow), ByVal confluxService As ConfluxService)

        Try

            If activeRatePlansPromos.Count > 0 Then
                Dim availStatusMessagesLockGralPromos As AvailStatusMessages = RestrictionsParser.ToAvailStatusMessages(lockGral, activeRooms, activeRatePlansPromos)
                Dim lockGralPromosHotelAvailNotifRQ As XElement = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockGralPromos)

                Dim lockGralPromosSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockGralPromosHotelAvailNotifRQ)

                Dim restrictionResponsePromos As RestrictionResponse = confluxService.UpdateRestriction(lockGralPromosSoapRQ, restrictionEnum:=RestrictionEnum.LockGral)

                If Not restrictionResponsePromos.IsSuccess Then
                    MyBase.WriteLog(restrictionResponsePromos.Xml, "LockGralPromos")
                ElseIf restrictionResponsePromos.IsSuccess Then
                    For Each restriction As Restriction In restrictionResponsePromos.Restrictions
                        Select Case restriction.Type
                            Case RestrictionEnum.LockGral
                                MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockGralPromos")
                                MyBase.WriteLog(restriction.Xml(0).ToString(), "LockGralPromos")
                        End Select
                    Next
                End If

            End If

        Catch ex As Exception
            MyBase.WriteLog(ex.Message, "LockGralPromos")
        End Try

    End Sub

    'Public Sub RequestLockRatePlanGoogle(ByVal status As String, ByVal ratePlanId As String, ByVal dates As List(Of Tuple(Of Date, Date)), ByVal activeRooms As List(Of DataRow), ByVal confluxService As ConfluxService)

    '    Try

    '        Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, status, ratePlanId)

    '        Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(activeRooms, lockRatePlans)

    '        Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

    '        Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

    '        Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, RestrictionEnum.LockRatePlan)

    '        If Not restrictionResponse.IsSuccess Then
    '            MyBase.WriteLog(restrictionResponse.Xml, "LockRatePlan")
    '        ElseIf restrictionResponse.IsSuccess Then
    '            For Each restriction As Restriction In restrictionResponse.Restrictions
    '                Select Case restriction.Type
    '                    Case RestrictionEnum.LockRatePlan
    '                        MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockRatePlan")
    '                        MyBase.WriteLog(restriction.Xml(0).ToString(), "LockRatePlan")
    '                End Select
    '            Next

    '        End If
    '    Catch ex As Exception
    '        MyBase.WriteLog(ex.Message, "LockRatePlan")
    '    End Try

    'End Sub

    Public Sub RequestLockRatePlanGoogle(ByVal lockRatePlans As List(Of spGetLockRatePlansByHotel_Result), ByVal activeRooms As List(Of DataRow), ByVal confluxService As ConfluxService)

        Try

            Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(activeRooms, lockRatePlans)

            Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

            Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

            Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, RestrictionEnum.LockRatePlan)

            If Not restrictionResponse.IsSuccess Then
                MyBase.WriteLog(restrictionResponse.Xml, "LockRatePlan")
            ElseIf restrictionResponse.IsSuccess Then
                For Each restriction As Restriction In restrictionResponse.Restrictions
                    Select Case restriction.Type
                        Case RestrictionEnum.LockRatePlan
                            MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockRatePlan")
                            MyBase.WriteLog(restriction.Xml(0).ToString(), "LockRatePlan")
                    End Select
                Next

            End If
        Catch ex As Exception
            MyBase.WriteLog(ex.Message, "LockRatePlan")
        End Try


    End Sub






    Public Sub RequestLockRatePlanPromosGoogle(ByVal lockGral As List(Of spGetLockGralByHotel_Result), ByVal activeRooms As List(Of DataRow), ByVal activeRatePlansPromos As List(Of DataRow), ByVal confluxService As ConfluxService)
        Try

            If activeRatePlansPromos.Count > 0 Then
                Dim availStatusMessagesLockGralPromos As AvailStatusMessages = RestrictionsParser.ToAvailStatusMessages(lockGral, activeRooms, activeRatePlansPromos)
                Dim lockGralPromosHotelAvailNotifRQ As XElement = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockGralPromos)

                Dim lockGralPromosSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockGralPromosHotelAvailNotifRQ)

                Dim restrictionResponsePromos As RestrictionResponse = confluxService.UpdateRestriction(lockGralPromosSoapRQ, restrictionEnum:=RestrictionEnum.LockRatePlan)

                If Not restrictionResponsePromos.IsSuccess Then
                    MyBase.WriteLog(restrictionResponsePromos.Xml, "LockRatePlanPromos")
                ElseIf restrictionResponsePromos.IsSuccess Then
                    For Each restriction As Restriction In restrictionResponsePromos.Restrictions
                        Select Case restriction.Type
                            Case RestrictionEnum.LockGral
                                MyBase.WriteLog(restriction.XmlRequest(0).ToString(), "LockRatePlanPromos")
                                MyBase.WriteLog(restriction.Xml(0).ToString(), "LockRatePlanPromos")
                        End Select
                    Next
                End If

            End If

        Catch ex As Exception
            MyBase.WriteLog(ex.Message, "LockRatePlanPromos")
        End Try
    End Sub


    Private Function GetValidPromos(ByVal activeRatePlansPromos As List(Of DataRow), ByVal promos As List(Of spGetPromosByRatePlan_Result)) As List(Of DataRow)

        Dim validPromos As List(Of DataRow) = New List(Of DataRow)

        Dim dsRatePlans As RatePlanData = New RatePlanData

        Dim dt As DataTable = dsRatePlans.Tables(RatePlanData.RATEPLAN_TABLE)

        For Each activeRatePlanPromo As DataRow In activeRatePlansPromos

            For Each promo As spGetPromosByRatePlan_Result In promos

                If promo.IdPromocion = activeRatePlanPromo.ItemArray(0).ToString() Then

                    Dim tempData As DataRow = dt.NewRow()

                    With tempData
                        .Item(RatePlanData.FIELD_IDRATEPLAN) = promo.IdPromocion
                    End With

                    validPromos.Add(tempData)

                End If

            Next

        Next

        Return validPromos

    End Function

#End Region


End Class
