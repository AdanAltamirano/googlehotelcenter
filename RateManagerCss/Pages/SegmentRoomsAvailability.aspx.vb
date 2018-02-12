Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Microsoft.VisualBasic
Imports System.Type

Partial Class SegmentRoomsAvailability
    Inherits PaginaBase
    Private Property RatesPlanCount() As Integer
        Get
            Return viewstate("_RatesPlanCount")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_RatesPlanCount") = Value
        End Set
    End Property
    Private Property RPName() As String
        Get
            Return viewstate("_rpname")
        End Get
        Set(ByVal Value As String)
            viewstate("_rpname") = Value
        End Set
    End Property

    Public Property SelectedMes() As Integer
        Get
            Return viewstate("SelectedMes")
        End Get
        Set(ByVal Value As Integer)
            viewstate("SelectedMes") = Value
        End Set
    End Property

    Public Property SelectedYear() As String
        Get
            Return viewstate("SelectedYear")
        End Get
        Set(ByVal Value As String)
            viewstate("SelectedYear") = Value
        End Set
    End Property

    Private Property selectedRoom() As Integer
        Get
            Return viewstate("selectedRoom")
        End Get
        Set(ByVal Value As Integer)
            viewstate("selectedRoom") = Value
        End Set
    End Property

    Private Property selectedRoomCode() As String
        Get
            Return viewstate("selectedRoomCode")
        End Get
        Set(ByVal Value As String)
            viewstate("selectedRoomCode") = Value
        End Set
    End Property

    Private Property RackRate() As String
        Get
            Return viewstate("_RackRate")
        End Get
        Set(ByVal Value As String)
            viewstate("_RackRate") = Value
        End Set
    End Property

    Private Property dt() As DataTable
        Get
            Return ViewState("_dt")
        End Get
        Set(ByVal Value As DataTable)
            ViewState("_dt") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.hplOcultar.NavigateUrl = "javascript:Ocultar('0');"
        hplShow.NavigateUrl = "javascript:Ocultar('1');"

        If Not IsPostBack Then
            '//'''''''''''''''''''''
            If Request.QueryString("Date") <> "" Then
                Try
                    Me.txtInicio.Text = CDate(Request.QueryString("Date")).ToString("MM/dd/yyyy")
                    Me.txtFinal.Text = CDate(Request.QueryString("Date")).ToString("MM/dd/yyyy")
                Catch ex As Exception
                    Me.txtInicio.Text = Now.Date.ToString("MM/dd/yyyy")
                    Me.txtFinal.Text = Now.Date.ToString("MM/dd/yyyy")
                End Try
            Else
                Me.txtInicio.Text = Now.Date.ToString("MM/dd/yyyy")
                Me.txtFinal.Text = Now.Date.ToString("MM/dd/yyyy")
            End If

            CargaFechas()
            loadTypes()

            Me.txtInicio.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
            Me.txtDivVisible.Text = "1"
            lblRoomRatePlan.Text = String.Empty
        End If

        btnSaveIntervals.OnClientClick = "return AlertMessage(); "
        btnLoad.OnClientClick = "return FireUpdateStatus();"
    End Sub
    Private Function FirstCaption(ByVal cad As String) As String
        If cad IsNot Nothing Then
            If cad.Length > 1 Then
                cad = cad.Substring(0, 1).ToUpper + cad.Substring(1, cad.Length - 1).ToLower
            ElseIf cad.Length = 1 Then
                cad = cad.ToUpper()
            End If
        End If
        Return cad
    End Function
    Private Sub loadculture()
        lbltitleintervals.Text = PortalCulture.GetString("00140")
        lblFrom.Text = PortalCulture.GetString("00108")
        lblTo.Text = PortalCulture.GetString("00109")
        btnLoad.Text = PortalCulture.GetString("00139")
        cmdSave.Text = PortalCulture.GetString("00008")
        btnSaveIntervals.Text = PortalCulture.GetString("00008")
        Me.lblTitle.Text = PortalCulture.GetString("00147")
        btnCancel.Text = PortalCulture.GetString("00009")

        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.lblDomingo.Text = FirstCaption(WeekdayName(1, False, (FirstDayOfWeek.Sunday)))
        Me.lblLunes.Text = FirstCaption(WeekdayName(2, False, (FirstDayOfWeek.Sunday)))
        Me.lblMartes.Text = FirstCaption(WeekdayName(3, False, (FirstDayOfWeek.Sunday)))
        Me.lblMiercoles.Text = FirstCaption(WeekdayName(4, False, (FirstDayOfWeek.Sunday)))
        Me.lblJueves.Text = FirstCaption(WeekdayName(5, False, (FirstDayOfWeek.Sunday)))
        Me.lblViernes.Text = FirstCaption(WeekdayName(6, False, (FirstDayOfWeek.Sunday)))
        Me.lblSabado.Text = FirstCaption(WeekdayName(7, False, (FirstDayOfWeek.Sunday)))
        Me.lbltitletable.Text = FirstCaption(MonthName(SelectedMes + 1) & " " & SelectedYear)
        System.Threading.Thread.CurrentThread.CurrentCulture = ci

        Me.lbltitleDate.Text = PortalCulture.GetString("00313")
        lblMonth.Text = PortalCulture.GetString("00311", True)
        lblYear.Text = PortalCulture.GetString("00312", True)
        lblRoomType.Text = PortalCulture.GetString("00072", True)
        lblRatePlanType.Text = PortalCulture.GetString("00816", True)
        lblSelectType.Text = PortalCulture.GetString("00817")
        Me.lblInfoCalendar.Text = PortalCulture.GetString("00818")

        Me.hplShow.Text = PortalCulture.GetString("00165")
        Me.hplOcultar.Text = PortalCulture.GetString("00166")
        lblRooms.Text = PortalCulture.GetString("00141", True)
        rvRooms.Text = PortalCulture.GetString("00146")
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

    Private Sub loadTypes()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        Dim RatesPlan As Portal.General.Common.Data.RatePlanData
        'Cargar meses y años.
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Dim idAsoc As Integer = Me.GetIdAsociation

        ddlMonth.Items.Clear()
        For i As Integer = 1 To 12
            ddlMonth.Items.Add(MonthName(i, False))
        Next
        Me.ddlMonth.SelectedIndex = Now.Month - 1
        For i As Integer = Now.Year - 1 To Now.Year + 3
            ddlyear.Items.Add(New ListItem(i, i))
        Next
        ddlyear.SelectedValue = Now.Year

        System.Threading.Thread.CurrentThread.CurrentCulture = ci

        'Cargamos los tipos de habitacion
        With New Portal.Hotel.Facade.RoomFacade
            Rooms = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        ddlRoomType.DataTextField = "texto"
        ddlRoomType.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        ddlRoomType.DataSource = Rooms
        ddlRoomType.DataBind()
        ddlRoomType.Items.Insert(0, PortalCulture.GetString("M000272"))
        ddlRoomType.Items(0).Value = 0

        'Cargamos los planes del Hotel
        With New RatePlanFacade
            RatesPlan = .GetRatePlanByIdHotel(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture, incluirNetRatesPlan:=1, idAsociacion:=idAsoc)
        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            RatesPlan = RatePlanFilter("C", RatesPlan)
        End If

        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        RatesPlan.Tables(RatePlanData.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RatePlanData.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & RatePlanData.FIELD_NAME & ",1,25)")
        'eliminar los ratesplan que ya tienen links
        'Dim dv As DataView
        '//''''''''For Each r As DataRow In RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows
        '//''''''''    dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
        '//''''''''    dv.RowFilter = links.FIELD_TargetRatePlan & "='" & r(RatesPlan.FIELD_IDRATEPLAN) & "'"
        '//''''''''    If dv.Count > 0 OrElse r(RatesPlan.FIELD_SEGMENT) = "K" Then
        '//''''''''        r.Delete()
        '//''''''''    End If
        '//''''''''Next
        Me.ddlRatePlanType.DataSource = RatesPlan
        Me.ddlRatePlanType.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
        Me.ddlRatePlanType.DataTextField = "Texto" 'Portal.General.Common.Data.RatePlanData.FIELD_CODIGOTARIFA
        Me.ddlRatePlanType.DataBind()

        Me.ddlRatePlanType.Items.Insert(0, "Todos")
        ddlRatePlanType.Items(0).Value = "0"
        ddlRatePlanType.Items(0).Text = PortalCulture.GetString("M000272")
        ddlRatePlanType.SelectedIndex = 0
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadculture()
        loadData()
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanInventory, Me.cmdSave, "M")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanInventory, Me.btnLoad, "R")

        If Me.txtDivVisible.Text = "1" Then
            hplShow.Style.Add("display", "")
            hplOcultar.Style.Add("display", "none")
            Div1.Style.Add("display", "none")
        Else
            hplShow.Style.Add("display", "none")
            hplOcultar.Style.Add("display", "")
            Div1.Style.Add("display", "")
        End If

        Me.TxtRooms.Attributes.Add("onchange", "javascript:validaAvaIntervals ('" & TxtRooms.ClientID & "')")
    End Sub

    Private Sub loadData()
        loadDatosUC()
    End Sub

    Function CreateDataSource() As ICollection
        RPName = ""
        Dim DS As New DataSet, RP As RatePlanData ', 'dsGral As clsCommonAvailibilityGral, strError As String
        Dim dt As New DataTable, dr As DataRow, ratesplans As String = ""
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            RP = .GetRatePlanByIdHotel(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With

        RatesPlanCount = RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count
        dt.Columns.Add(New DataColumn(PortalCulture.GetString("00170"), GetType(String)))
        dt.Columns.Add(New DataColumn("IDROOM", GetType(String)))

        For Each r As DataRow In RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            If r(RatePlanData.FIELD_CODIGOTARIFA) Is System.DBNull.Value OrElse r(RatePlanData.FIELD_CODIGOTARIFA).ToString.Trim = "" Then
                dt.Columns.Add(New DataColumn("_" & r(RatePlanData.FIELD_IDRATEPLAN).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("ID" & r(RatePlanData.FIELD_IDRATEPLAN).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Ava" & r(RatePlanData.FIELD_IDRATEPLAN).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Low" & r(RatePlanData.FIELD_IDRATEPLAN).ToString, GetType(String)))
            Else
                dt.Columns.Add(New DataColumn(r(RatePlanData.FIELD_CODIGOTARIFA).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("ID" & r(RatePlanData.FIELD_IDRATEPLAN).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Ava" & r(RatePlanData.FIELD_CODIGOTARIFA).ToString, GetType(String)))
                dt.Columns.Add(New DataColumn("Low" & r(RatePlanData.FIELD_IDRATEPLAN).ToString, GetType(String)))
            End If

            ratesplans &= r(RatePlanData.FIELD_IDRATEPLAN).ToString & ","
            If r.IsNull(RatePlanData.FIELD_NAME) Then
                RPName &= "--" & ","
            Else
                RPName &= r(RatePlanData.FIELD_NAME).ToString & ","
            End If
        Next

        If ratesplans <> "" Then
            With New RoomsInventoryFacade
                DS = .GetInventoryBySegments(Me.cInfoActual.Hotel, CDate(Me.txtInicio.Text), CDate(Me.txtInicio.Text), ratesplans.Substring(0, ratesplans.Length - 1), PortalCulture.GetIDCulture)
            End With

        End If

        Dim i As Integer
        Dim dvInventory, dvLowest As DataView

        If Not DS Is Nothing Then
            For i = 0 To DS.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count - 1
                dr = dt.NewRow()
                dr(PortalCulture.GetString("00170")) = DS.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(i).Item(RoomsHotelData.FLD_ROOM_CODE) & "--" & DS.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(i).Item(RoomsHotelData.FLD_NOMBRE)
                dr("IDROOM") = DS.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(i).Item(RoomsHotelData.FLD_ID_ROOM_HOTEL)

                For tbl As Integer = 0 To RatesPlanCount - 1
                    dvInventory = DS.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView()
                    dvInventory.RowFilter = RatePlanData.FIELD_IDRATEPLAN & "='" & RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_IDRATEPLAN) & "' and " & RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & dr("IDROOM")

                    If RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_IDRATEPLAN).ToString <> "" Then
                        Try
                            dr(RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_CODIGOTARIFA)) = dvInventory(0)("reservaciones")
                            dr("Ava" & RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_CODIGOTARIFA)) = dvInventory(0)("Avail")
                            If DS.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then
                                dvLowest = DS.Tables(FaresData.FARES_TABLE).DefaultView()
                                dvLowest.RowFilter = RoomsHotelData.FLD_ID_ROOM_HOTEL & "=" & dr("IDROOM") & " and " & RatePlanData.FIELD_IDRATEPLAN & "='" & RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_IDRATEPLAN).ToString & "'"
                                If dvLowest.Count > 0 Then
                                    dr("Low" & RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_IDRATEPLAN)) = dvLowest(0)("tar")
                                End If
                            End If

                        Catch ex As Exception
                            dr("_" & RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_IDRATEPLAN)) = dvInventory(0)("reservaciones")
                            dr("Ava" & RP.Tables(RatePlanData.RATEPLAN_TABLE).Rows(tbl).Item(RatePlanData.FIELD_IDRATEPLAN)) = dvInventory(0)("Avail")
                        End Try
                    End If
                Next
                dt.Rows.Add(dr)
            Next i
        End If
        Dim dv As New DataView(dt)
        Return dv
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If txtCambiaCalendar.Text.Trim <> "" Then
            saveCalendar()
        End If
    End Sub

    Sub Addrateplan(ByVal ds As LockRatesPlanRoomData, ByVal field As String, ByVal valor As String)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    Function getDataXML(ByVal fechaInicio As String, ByVal fechaFinal As String) As String
        Dim ds As LockRatesPlanRoomData
        Dim strError As String = ""
        Dim str1 As String

        With New lockRatesPlanRoomFacade
            ds = .GetListByDates(Me.cInfoActual.Hotel, CDate(fechaInicio), CDate(fechaFinal), strError)
            If selectedRoom = 0 Then
                str1 = String.Format("{0} ({1})", PortalCulture.GetString("01181"), PortalCulture.GetString("00144")) '"Update Rooms"   
            Else
                str1 = PortalCulture.GetString("01181")
            End If
            Addrateplan(ds, "Descr_room", ddlRoomType.SelectedItem.Text)
            Addrateplan(ds, "Descr_rateplans", ddlRatePlanType.SelectedItem.Text)

        End With
        Return Util.Utility.GetXml(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms, "UpdateLockRates", ds)
    End Function

    Function ValidaInventarioCalendario(ByVal ds As DataSet, ByRef sErrorInv As String) As String
        Dim dv As DataView
        Dim imaxInventario As Integer

        Integer.TryParse(hidMaxInventario.Value, imaxInventario)
        For Each row As DataRow In ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
            dv = dt.DefaultView
            dv.RowFilter = String.Format("Idrateplan='{0}' And IdTipoHabitacion_Hotel={1} And fecha= '{2}'", row(LockRatesPlanRoomData.FIELD_IDRATEPLAN), row(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel), row(LockRatesPlanRoomData.FIELD_StartDate))
            If (dv.Count > 0) AndAlso (row(LockRatesPlanRoomData.FIELD_RoomsAvailable) > dv(0)("maxInventario")) Then
                sErrorInv = String.Format(PortalCulture.GetString("01450"), imaxInventario)
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub saveCalendar()
        Dim ds As New LockRatesPlanRoomData
        Dim row As DataRow
        Dim fecha As String
        Dim grabarNota As Boolean = False
        Dim renglon As Integer = (txtCambiaCalendar.Text.Length / 375) + 10
        Dim notas(renglon) As String
        Dim sDataPrev As String
        Dim sData As String
        Dim i As Integer = 0
        Dim sErrorInv As String = ""


        sErrorInv = ""
        notas(i) = "Se cambió disponibilidad."

        For Each st As String In txtCambiaCalendar.Text.Split(",")
            If st.Trim <> "" Then
                st.Split("~")

                If IsNumeric(st.Split("/")(2)) AndAlso st.Split("/")(2).ToString.IndexOf(".") < 0 AndAlso st.Split("/")(2).ToString.IndexOf("-") < 0 Then
                    fecha = st.Split("~")(1)

                    'si hay un registro agregado se eliminará por que esta cadena fue la última
                    For Each row In ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
                        If CInt(row(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel)) = CInt(st.Split("/")(0)) AndAlso row(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = st.Split("/")(1).Substring(2) Then
                            row.Delete()
                            Exit For
                        End If
                    Next
                    ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).AcceptChanges()

                    'ahora si, hay que agregarlo
                    row = ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).NewRow
                    row(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = st.Split("/")(1).Substring(2)
                    row(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel) = CInt(st.Split("/")(0))
                    row(LockRatesPlanRoomData.FIELD_RoomsAvailable) = st.Split("/")(2)
                    row(LockRatesPlanRoomData.FIELD_StartDate) = CDate(fecha)
                    row(LockRatesPlanRoomData.FIELD_EndDate) = CDate(fecha)
                    ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Add(row)

                    If i <= renglon Then
                        If notas(i).Length + (" " & CDate(fecha) & ": ").Length + (st.Split("/")(2) & " Cuartos, habitación " & st.Split("~")(0).Split("/")(4) & ", ratecode " & st.Split("/")(3) & ".").Length > 400 Then
                            i = i + 1
                            notas(i) = "Se cambió disponibilidad."
                        End If
                        notas(i) &= (" " & CDate(fecha) & ": ") + st.Split("/")(2) & " Cuartos, habitación " & st.Split("~")(0).Split("/")(4) & ", ratecode " & st.Split("/")(3) & "."
                    End If

                    '// Esta dentro del rango de inventario permitido, entonces guardalo.
                    If ValidaInventarioCalendario(ds, sErrorInv) Then
                        LockRoomRatePlanCalendar(ds, grabarNota, fecha, fecha, sDataPrev, sData)
                    End If

                End If
            End If
        Next

        If grabarNota = True Then

            'Dim dv As DataView
            'For Each row In ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
            '    dv = dt.DefaultView
            '    dv.RowFilter = String.Format("Idrateplan='{0}' And IdTipoHabitacion_Hotel={1} And fecha= '{2}'", row(LockRatesPlanRoomData.FIELD_IDRATEPLAN), row(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel), row(LockRatesPlanRoomData.FIELD_StartDate))
            '    If (dv.Count > 0) AndAlso (row(LockRatesPlanRoomData.FIELD_RoomsAvailable) > dv(0)("maxInventario")) Then
            '        sErrorInv = String.Format(PortalCulture.GetString("01450"), imaxInventario)
            '    End If
            'Next
            Dim sdatocorreo As String
            For j As Integer = 0 To i
                If notas(j) <> String.Empty Then
                    sdatocorreo = (New Util.Utility).GeneraCorreoXslt(sDataPrev, sData)
                    Me.guardalog("Page/SegmentRoomsAvailability.aspx", PaginaBase.acciones.Modificar, notas(j), "", sDataPrev, sData, sdatocorreo)
                End If
            Next
        End If

        lblSugerencia.Visible = False
        If Not String.IsNullOrEmpty(sErrorInv) Then
            lblSugerencia.Visible = True
        End If
        lblSugerencia.Text = sErrorInv


        txtCambiaCalendar.Text = String.Empty
        lblRoomRatePlan.Text = String.Empty
    End Sub

    Private Function LockRoomRatePlanCalendar(ByVal data As LockRatesPlanRoomData, ByRef grabarNota As Boolean, _
                                              ByVal fechaInicio As String, ByVal fechaFinal As String, _
                                              ByRef sDataPrev As String, ByRef sData As String) As Boolean
        Dim ds As LockRatesPlanRoomData
        Dim strError As String = ""
        With New lockRatesPlanRoomFacade
            ds = .GetListByDates(Me.cInfoActual.Hotel, CDate(fechaInicio), CDate(fechaFinal), strError)
        End With

        sDataPrev = getDataXML(fechaInicio, fechaFinal)


        Dim dstrans As New LockRatesPlanRoomData
        If String.IsNullOrEmpty(strError) Then
            If Not ds Is Nothing AndAlso ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count - 1
                    With ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms)
                        Dim dv As DataView
                        dv = data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).DefaultView
                        dv.RowFilter = LockRatesPlanRoomData.FIELD_IDRATEPLAN & "='" & .Rows(i).Item(LockRatesPlanRoomData.FIELD_IDRATEPLAN) & "' and " & LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel & "=" & .Rows(i).Item(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel)
                        If dv.Count > 0 Then
                            copyrow(.Rows(i), .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate), .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate), dstrans, True)
                            If .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate) >= CDate(fechaInicio) AndAlso .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate) <= CDate(fechaFinal) Then
                                'YA ESTÁ ELIMINADA
                            ElseIf .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate) < CDate(fechaInicio) AndAlso .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate) <= CDate(fechaFinal) Then
                                copyrow(.Rows(i), .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate), CDate(fechaInicio).AddDays(-1), dstrans, False)
                            ElseIf .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate) >= CDate(fechaInicio) AndAlso .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate) > CDate(fechaFinal) Then
                                copyrow(.Rows(i), CDate(fechaFinal).AddDays(1), .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate), dstrans, False)
                            Else
                                copyrow(.Rows(i), .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate), CDate(fechaInicio).AddDays(-1), dstrans, False)
                                copyrow(.Rows(i), CDate(fechaFinal).AddDays(1), .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate), dstrans, False)
                            End If
                        End If
                    End With
                Next
            End If
            If Not data Is Nothing AndAlso data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count > 0 Then
                For Each dr As DataRow In data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
                    copyrow(dr, CDate(fechaInicio), CDate(fechaFinal), dstrans, False)
                Next
            End If

            With New lockRatesPlanRoomFacade
                If Not dstrans Is Nothing AndAlso dstrans.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count > 0 Then
                    If .Update(dstrans) Then
                        Addrateplan(dstrans, "Descr_room", ddlRoomType.SelectedItem.Text)
                        Addrateplan(dstrans, "Descr_rateplans", ddlRatePlanType.SelectedItem.Text)

                        sData = Util.Utility.GetXml(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms, "UpdateLockRates", dstrans)
                        grabarNota = True
                    End If
                End If
            End With
        End If
    End Function

    Private Function LockRoomRatePlan(ByVal data As LockRatesPlanRoomData, ByVal nota As String) As Boolean
        Dim ds As LockRatesPlanRoomData
        Dim strError As String = ""
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        With New lockRatesPlanRoomFacade
            ds = .GetListByDates(Me.cInfoActual.Hotel, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), strError)
        End With

        sDataPrev = getDataXML(Me.txtInicio.Text, Me.txtFinal.Text)

        Dim dstrans As New LockRatesPlanRoomData
        If String.IsNullOrEmpty(strError) Then
            If Not ds Is Nothing AndAlso ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count - 1
                    With ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms)
                        Dim dv As DataView
                        dv = data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).DefaultView
                        dv.RowFilter = LockRatesPlanRoomData.FIELD_IDRATEPLAN & "='" & .Rows(i).Item(LockRatesPlanRoomData.FIELD_IDRATEPLAN) & "' and " & LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel & "=" & .Rows(i).Item(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel)
                        If dv.Count > 0 Then
                            copyrow(.Rows(i), .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate), .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate), dstrans, True)
                            If .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
                                'YA ESTÁ ELIMINADA
                            ElseIf .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate) < CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate) <= CDate(Me.txtFinal.Text) Then
                                copyrow(.Rows(i), .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
                            ElseIf .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate) >= CDate(Me.txtInicio.Text) AndAlso .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate) > CDate(Me.txtFinal.Text) Then
                                copyrow(.Rows(i), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate), dstrans, False)
                            Else
                                copyrow(.Rows(i), .Rows(i).Item(LockRatesPlanRoomData.FIELD_StartDate), CDate(Me.txtInicio.Text).AddDays(-1), dstrans, False)
                                copyrow(.Rows(i), CDate(Me.txtFinal.Text).AddDays(1), .Rows(i).Item(LockRatesPlanRoomData.FIELD_EndDate), dstrans, False)
                            End If
                        End If
                    End With
                Next
            End If
            If Not data Is Nothing AndAlso data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count > 0 Then
                For Each dr As DataRow In data.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
                    copyrow(dr, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), dstrans, False)
                Next
            End If
            Dim sdatocorreo As String
            With New lockRatesPlanRoomFacade
                If Not dstrans Is Nothing AndAlso dstrans.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Count > 0 Then
                    If .Update(dstrans) Then
                        Addrateplan(dstrans, "Descr_room", ddlRoomType.SelectedItem.Text)
                        Addrateplan(dstrans, "Descr_rateplans", ddlRatePlanType.SelectedItem.Text)
                        sData = Util.Utility.GetXml(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms, "UpdateRoomSegment", dstrans)
                        sdatocorreo = (New Util.Utility).GeneraCorreoXslt(sDataPrev, sData)
                        Me.guardalog("Page/SegmentRoomsAvailability.aspx", PaginaBase.acciones.Modificar, nota, "", sDataPrev, sData, sdatocorreo)
                    End If
                End If
            End With
        End If
    End Function

    Private Function copyrow(ByVal row As DataRow, ByVal fechainicio As DateTime, ByVal fechafinal As DateTime, ByVal cAva As LockRatesPlanRoomData, ByVal borrado As Boolean)
        Dim newrow As DataRow
        newrow = cAva.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).NewRow
        With newrow
            .Item(LockRatesPlanRoomData.FIELD_StartDate) = fechainicio
            .Item(LockRatesPlanRoomData.FIELD_EndDate) = fechafinal
            .Item(LockRatesPlanRoomData.FIELD_RoomsAvailable) = row.Item(LockRatesPlanRoomData.FIELD_RoomsAvailable)
            .Item(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel) = row.Item(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel)
            .Item(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = row.Item(LockRatesPlanRoomData.FIELD_IDRATEPLAN)
            cAva.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Add(newrow)
            If borrado = True Then
                .Item(LockRatesPlanRoomData.FIELD_IdLockRatePlanRoom) = row.Item(LockRatesPlanRoomData.FIELD_IdLockRatePlanRoom)
                newrow.AcceptChanges()
                newrow.Delete()
            End If
        End With
    End Function

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim month As Integer = Me.ddlMonth.SelectedIndex + 1
        Dim year As Integer = 0
        Integer.TryParse(Me.ddlyear.SelectedValue, year)
        Dim searched As New Date(year, month, 1)

        Me.txtInicio.Text = searched.ToString("MM/dd/yyyy")
        Me.txtFinal.Text = searched.AddDays(Date.DaysInMonth(year, month) - 1).ToString("MM/dd/yyyy")
        'loadData()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Function LeeInventario() As Boolean
        Dim DS As DataSet = Nothing  ', dsGral As clsCommonAvailibilityGral
        Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
        Dim EndDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), Date.DaysInMonth(SelectedYear, SelectedMes + 1))
        Dim dv As DataView
        Dim iInvet As String
        Dim hr As Boolean = True

        Integer.TryParse(TxtRooms.Text, iInvet)
        With New RoomsInventoryFacade
            If ddlRoomType.SelectedValue <> 0 And ddlRatePlanType.SelectedValue <> "0" Then
                DS = .GetInventoryByRoomAndRatePlan(Me.cInfoActual.Hotel, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), ddlRatePlanType.SelectedValue, ddlRoomType.SelectedValue, PortalCulture.GetIDCulture)
                If Not dsEmpty(DS) Then
                    dv = DS.Tables(0).DefaultView
                    dv.RowFilter = String.Format("max(maxInventario) < {0}", iInvet)
                    If dv.Count > 0 Then hr = False
                End If
            End If
        End With
        Return hr
    End Function

    Private Sub loadDatosUC()
        SelectedMes = ddlMonth.SelectedIndex
        SelectedYear = ddlyear.SelectedValue
        Dim d1 As Date = New Date(SelectedYear, SelectedMes + 1, 1)

        'txtInicio.Text = d1.ToString("MM/dd/yyyy")
        'txtFinal.Text = d1.AddDays(Date.DaysInMonth(d1.Year, d1.Month) - 1).ToString("MM/dd/yyyy")

        If Me.ddlRoomType.Items.Count > 0 Then
            selectedRoom = Me.ddlRoomType.SelectedValue
            selectedRoomCode = Me.ddlRoomType.SelectedItem.Text
        End If

        Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
        Dim EndDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), Date.DaysInMonth(SelectedYear, SelectedMes + 1))
        Dim FirstDay As Integer = Weekday(selectDate, FirstDayOfWeek.Sunday)
        'Dim data As RoomsInventoryData
        Dim ctrl As Control ', strerror As String
        'Dim lockgral As clsCommonAvailibilityGral, dsRates As DataSet, dsRateRack As DataSet

        For i As Integer = 1 To FirstDay - 1
            ctrl = Me.FindControl("ctrRoomRatePlan" & (i).ToString)
            CType(ctrl, ctrRoomRatePlan).Show(False)
        Next
        For i As Integer = Date.DaysInMonth(SelectedYear, CInt(SelectedMes + 1)) + 1 To 42
            ctrl = Me.FindControl("ctrRoomRatePlan" & (i).ToString)
            CType(ctrl, ctrRoomRatePlan).Show(False)
        Next

        Dim DS As DataSet = Nothing  ', dsGral As clsCommonAvailibilityGral
        Dim dt1 As New DataTable, dr2 As DataRow
        Dim dtTarifa As New DataTable, dr3 As DataRow

        Dim columnaFecha As DataColumn = New DataColumn
        columnaFecha.DataType = System.Type.GetType("System.DateTime")
        columnaFecha.ColumnName = "fecha"

        dt = New DataTable
        dt.Columns.Add(columnaFecha)
        dt.Columns.Add("idRatePlan")
        dt.Columns.Add("idTipoHabitacion_hotel")
        dt.Columns.Add("reservaciones")
        dt.Columns.Add("avail")
        dt.Columns.Add("tarifa")
        dt.Columns.Add("maxInventario")
        dt.Columns.Add("availRoom")

        Dim keys(0) As DataColumn
        keys(0) = columnaFecha
        dt.PrimaryKey = keys

        Dim columnaFecha2 As DataColumn = New DataColumn
        columnaFecha2.DataType = System.Type.GetType("System.DateTime")
        columnaFecha2.ColumnName = "fecha"

        dtTarifa.Columns.Add(columnaFecha2)
        dtTarifa.Columns.Add("tarifa")

        Dim keys2(0) As DataColumn
        keys2(0) = columnaFecha2
        dtTarifa.PrimaryKey = keys2

        With New RoomsInventoryFacade
            If ddlRoomType.SelectedValue <> 0 And ddlRatePlanType.SelectedValue <> "0" Then
                lblRoomRatePlan.Text = ddlRoomType.SelectedItem.Text & " / " & ddlRatePlanType.SelectedItem.Text
                DS = .GetInventoryByRoomAndRatePlan(Me.cInfoActual.Hotel, selectDate, EndDate, ddlRatePlanType.SelectedValue, ddlRoomType.SelectedValue, PortalCulture.GetIDCulture)
            Else
                lblRoomRatePlan.Text = String.Empty
            End If
        End With

        Dim DataInv As RoomsInventoryData
        With New RoomsInventoryFacade
            DataInv = .getInventoryByDate_Data(ddlRoomType.SelectedValue, selectDate, EndDate)
        End With

        Dim dvInventory As DataView
        Dim dvTarifa As DataView
        Dim dvInventarioHabitacion As DataView

        If Not DS Is Nothing Then
            dvInventory = DS.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView()
            dvTarifa = DS.Tables(FaresData.FARES_TABLE).DefaultView()
            dvInventarioHabitacion = DataInv.Tables(RoomsInventoryData.TBL_ROOMS_INVENTORY).DefaultView
            For Each drTarifa As DataRow In DS.Tables(FaresData.FARES_TABLE).Rows
                Try
                    dr3 = dtTarifa.NewRow()
                    dr3("fecha") = drTarifa("fecha")
                    dr3("tarifa") = drTarifa("tar")
                    dtTarifa.Rows.Add(dr3)
                Catch
                End Try
            Next

            For cont As Integer = 0 To dvInventory.Count - 1
                dr2 = dt.NewRow()
                dr2("fecha") = dvInventory(cont)("fecha")
                dr2("idRatePlan") = dvInventory(cont)("idRatePlan")
                dr2("idTipoHabitacion_hotel") = dvInventory(cont)("idTipoHabitacion_hotel")
                dr2("reservaciones") = dvInventory(cont)("reservaciones")
                dr2("avail") = dvInventory(cont)("avail")
                dr2("availRoom") = dvInventory(cont)("avail")
                dr2("maxInventario") = dvInventory(cont)("maxInventario")

                If Not (IsDBNull(dvInventory(cont)("RoomsAvailable"))) Then
                    dr2("avail") = dvInventory(cont)("RoomsAvailable")
                End If

                Dim dtInv As Date
                Date.TryParse(dvInventory(cont)("fecha"), dtInv)
                dvInventarioHabitacion.RowFilter = String.Format("Fecha= '{0}'", dtInv)
                If dvInventarioHabitacion.Count > 0 Then
                    dr2("availRoom") = dvInventarioHabitacion(0)("Disponibilidad")
                End If


                'hidMaxInventario.Value = dvInventory(cont)("maxInventario")
                'If dvInventory(cont)("maxInventario2") > 0 Then
                '    If dvInventory(cont)("maxInventario2") < dvInventory(cont)("maxInventario") Then
                '        hidMaxInventario.Value = dvInventory(cont)("maxInventario2")
                '    End If
                'End If
                dr2("maxInventario") = hidMaxInventario.Value

                dr3 = Nothing
                If dtTarifa.Rows.Count > 0 Then
                    dr3 = dtTarifa.Rows.Find(dvInventory(cont)("fecha"))
                End If

                If Not dr3 Is Nothing Then
                    dr2("tarifa") = FCurrency(dr3("tarifa"), 2)
                Else
                    dr2("tarifa") = "NA"
                End If

                dt.Rows.Add(dr2)
            Next
        End If

        For i As Integer = 0 To Date.DaysInMonth(SelectedYear, CInt(SelectedMes + 1)) - 1
            ctrl = Me.FindControl("ctrRoomRatePlan" & (i + FirstDay).ToString)
            If ddlRoomType.SelectedValue <> 0 And ddlRatePlanType.SelectedValue <> "0" Then
                CType(ctrl, ctrRoomRatePlan).Show(True)
            Else
                CType(ctrl, ctrRoomRatePlan).Show(False)
                CType(ctrl, ctrRoomRatePlan).showDay(True)
            End If

            CType(ctrl, ctrRoomRatePlan).Day = i + 1

            If (selectDate.AddDays(i).ToString("yyyy/MM/dd") < Now.Date.ToString("yyyy/MM/dd")) Or Me.cInfoActual.UserPerfil = PaginaBase.PerfilHotel.Basico Then
                CType(ctrl, ctrRoomRatePlan).Editable = False
            Else
                CType(ctrl, ctrRoomRatePlan).Editable = True
            End If

            If dt.Rows.Count > 0 Then
                dr2 = dt.Rows.Find(selectDate.AddDays(i))
                If dr2 Is Nothing OrElse dr2.IsNull("fecha") OrElse dr2.IsNull("reservaciones") OrElse dr2.IsNull("Avail") OrElse dr2.IsNull("tarifa") OrElse dr2.IsNull("AvailRoom") Then
                    CType(ctrl, ctrRoomRatePlan).ShowLink() = False
                    CType(ctrl, ctrRoomRatePlan).loaddatos("0 / 0 / 0", String.Empty)
                Else
                    If CDate(dr2("fecha")).ToString("yyyy/MM/dd") = selectDate.AddDays(i).ToString("yyyy/MM/dd") Then
                        If selectDate.AddDays(i).ToString("yyyy/MM/dd") < Now.Date Then
                            CType(ctrl, ctrRoomRatePlan).ShowLink() = False

                            'CType(ctrl, ctrRoomRatePlan).loaddatos(String.Format("{0} / {1}", dr2("reservaciones"), dr2("Avail")), dr2("tarifa"))
                            CType(ctrl, ctrRoomRatePlan).loaddatos(String.Format("{0} / {1} / {2}", dr2("reservaciones"), dr2("Avail"), dr2("AvailRoom")), dr2("tarifa"))
                        Else
                            CType(ctrl, ctrRoomRatePlan).ShowLink() = True
                            CType(ctrl, ctrRoomRatePlan).loaddatos(dr2("reservaciones"), dr2("Avail"), dr2("AvailRoom"), dr2("tarifa"), 0)
                        End If
                    End If
                End If
            End If
        Next

        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.lbltitletable.Text = FirstCaption(MonthName(SelectedMes + 1) & " " & SelectedYear)
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub btnCancelCalendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect(Request.RawUrl)
    End Sub

    'Private Sub dgRP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub

    Private Sub btnSaveIntervals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveIntervals.Click
        Dim imaxInventario As Integer
        Dim sErrorInv As String = ""
        Integer.TryParse(hidMaxInventario.Value, imaxInventario)

        sErrorInv = ""
        If Not LeeInventario() Then
            sErrorInv = String.Format(PortalCulture.GetString("01450"), imaxInventario)
            If Not String.IsNullOrEmpty(sErrorInv) Then
                lblSugerencia.Visible = True
            End If
            lblSugerencia.Text = sErrorInv
            Return
        End If

        'If TxtRooms.Text.Trim <> String.Empty And txtCambia.Text.Trim <> "error" And ddlRoomType.SelectedValue <> 0 And ddlRatePlanType.SelectedValue <> "0" Then
        If (CType(txtInicio.Text, Date) < CType(txtFinal.Text, Date)) And TxtRooms.Text.Trim <> String.Empty And txtCambia.Text.Trim <> "error" And ddlRoomType.SelectedValue <> 0 And ddlRatePlanType.SelectedValue <> "0" Then
            Dim ds As New LockRatesPlanRoomData
            Dim row As DataRow
            Dim nota As String = "Se cambió la disponibilidad del " & CDate(Me.txtInicio.Text) & " al " & CDate(Me.txtFinal.Text) & " con los datos: "

            txtCambia.Text = ddlRoomType.SelectedValue.ToString & "/ID" & ddlRatePlanType.SelectedValue & "/" & TxtRooms.Text.Trim & "/" & ddlRatePlanType.SelectedValue & "/" & ddlRoomType.SelectedItem.Text & ","

            For Each st As String In txtCambia.Text.Split(",")
                If st.Trim <> "" Then
                    If IsNumeric(st.Split("/")(2)) AndAlso st.Split("/")(2).ToString.IndexOf(".") < 0 AndAlso st.Split("/")(2).ToString.IndexOf("-") < 0 Then
                        'si hay un registro agregado se eliminará por que esta cadena fue la última
                        For Each row In ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
                            If CInt(row(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel)) = CInt(st.Split("/")(0)) AndAlso row(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = st.Split("/")(1).Substring(2) Then
                                row.Delete()
                                Exit For
                            End If
                        Next
                        ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).AcceptChanges()

                        'ahora si, hay que agregarlo
                        row = ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).NewRow
                        row(LockRatesPlanRoomData.FIELD_IDRATEPLAN) = st.Split("/")(1).Substring(2)
                        row(LockRatesPlanRoomData.FIELD_IdTipoHabitacion_Hotel) = CInt(st.Split("/")(0))
                        row(LockRatesPlanRoomData.FIELD_RoomsAvailable) = st.Split("/")(2)
                        row(LockRatesPlanRoomData.FIELD_StartDate) = CDate(Me.txtInicio.Text)
                        row(LockRatesPlanRoomData.FIELD_EndDate) = CDate(Me.txtFinal.Text)
                        ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows.Add(row)

                        nota &= st.Split("/")(2) & " Cuartos para la habitación " & st.Split("/")(4) & " y el ratecode " & st.Split("/")(3) & ". "
                    End If
                End If
            Next
            LockRoomRatePlan(ds, nota)
            lblRoomsError.Visible = False
            lblSugerencia.Visible = False

          
            'For Each row In ds.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).Rows
            '    If (imaxInventario <> 9999 And row(LockRatesPlanRoomData.FIELD_RoomsAvailable) > imaxInventario) Then
            '        sErrorInv = String.Format(PortalCulture.GetString("01450"), imaxInventario)
            '    End If
            'Next
            If Not String.IsNullOrEmpty(sErrorInv) Then
                lblSugerencia.Visible = True                
            End If
            lblSugerencia.Text = sErrorInv
            txtCambia.Text = String.Empty
            TxtRooms.Text = String.Empty


            ddlMonth.SelectedIndex = (CType(txtInicio.Text, Date).Month) - 1
            ddlyear.SelectedValue = CType(txtInicio.Text, Date).Year
        Else
            lblRoomsError.Visible = True

            If txtCambia.Text.Trim = "error" Then
                lblRoomsError.Text = PortalCulture.GetString("00839")
            Else
                If TxtRooms.Text.Trim = String.Empty Then
                    lblRoomsError.Text = PortalCulture.GetString("00840")
                Else
                    If (CType(txtInicio.Text, Date) > CType(txtFinal.Text, Date)) Then
                        lblRoomsError.Text = PortalCulture.GetString("00841")
                    Else
                        If ddlRoomType.SelectedValue = 0 Then
                            lblRoomsError.Text = PortalCulture.GetString("00842")
                        Else
                            If ddlRatePlanType.SelectedValue = "0" Then
                                lblRoomsError.Text = PortalCulture.GetString("00843")
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
End Class


