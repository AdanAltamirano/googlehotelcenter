Imports Microsoft.VisualBasic
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data

Imports System.IO
Imports System.Text

Partial Class RoomClosure
    Inherits PaginaBase
    Private Enum cancelpolicy
        bydays
        byhour
        specifichour
    End Enum
    Enum dgcolumns
        Fecha
        Codigo
        oneperson
        twoperson
        Min
        Max
        Avail
    End Enum
    Private Property dsRooms() As RoomsHotelData
        Get
            Return Session("_dsrooms")
        End Get
        Set(ByVal Value As RoomsHotelData)
            Session("_dsrooms") = Value
        End Set
    End Property

    Private Property idroom() As Integer
        Get
            Return viewstate("_idRoom")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_idRoom") = Value
        End Set
    End Property
    Private Property totalrooms() As Integer
        Get
            Return viewstate("_tr")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_tr") = Value
        End Set
    End Property
    Private Property DifDays() As Integer
        Get
            Return viewstate("_DifDays")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_DifDays") = Value
        End Set
    End Property
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
    Protected WithEvents searchByRatePlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnk As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblErrorStatus As System.Web.UI.WebControls.Label
    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub
    Private Sub btnload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        DifDays = DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        Me.dgRatesHorizontal.DataSource = CreateDataSourceHorizontal()
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgRatesHorizontal.DataBind()
        dgRatesHorizontal.Visible = True
        System.Threading.Thread.CurrentThread.CurrentCulture = ci


    End Sub
    Function CreateDataSourceHorizontal() As ICollection
        Dim DS As DataSet, RP As RatePlanData, strError As String
        Dim dt As New DataTable, drrp As DataRow, dr As DataRow, dr1 As DataRow, dr2 As DataRow, ratesplans As String = "", dr3 As DataRow, dr4 As DataRow, dr5 As DataRow
        Dim dsrooms As RoomsHotelData, data As DataSet ', Reservas As DataSet ,dsGral As clsCommonAvailibilityGral,
        Dim dsrateplans As RatePlanData, planData As DataSet
        Dim dsAva As DataSet, hotelData As DataSet
        Dim idAsoc As Integer = Me.GetIdAsociation
        Dim strColumns As String
        Dim checkin As Date
        Dim dtRates As DataTable
        Dim dsLockRoomTypes As DataSet
        Dim linkRP As DataSet, linkRoom As DataSet
        Dim dateStart As DateTime = CDate(txtDateFrom.Text), dateEnd As DateTime = CDate(txtDateTo.Text)
        checkin = CDate(txtDateFrom.Text)

        'NO SE VA A OCUPAR EN ESTE METODO PERO SE VA A DEJAR PARA PRUEBAS
        '////////////////////// carga datos disponibilidad ///////
        'With New clsGetAvail
        'dtRates = .GetAvail(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), MyBase.cInfoActual.Hotel, ddlRateplans.SelectedItem.Text)
        'End With

        With New LinkRatePlanAccess
            linkRP = .GetLinkById(Me.cInfoActual.Hotel, ddlRatePlanFilter.SelectedValue)
        End With

        With New RoomLinksAccess
            linkRoom = .GetLinkById(Me.cInfoActual.Hotel)
        End With


        With New HotelSistema
            hotelData = .GetHotelById(MyBase.cInfoActual.Hotel)
        End With

        With New HotelStatusAvailabilityFacade
            dsAva = .getStatusAva(MyBase.cInfoActual.Hotel, dateStart, dateEnd)
        End With

        If dsAva.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 Then
            StatusHotel = dsAva.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_STATUSAVAILABILITY)
        End If
        Dim colorStatusHotel = color(StatusHotel)
        Dim dvInventory As DataView, dvReservas As DataView, dvlock As DataView, dvlockrproom As DataView, dvlockrp As DataView, dvRooms As DataView
        dvlock = dsAva.Tables(clsCommonAvailibilityGral.TABLE_LockGral).DefaultView
        dvReservas = dsAva.Tables(ReservaDatos.RESERVA_TABLE).DefaultView
        dvlockrproom = dsAva.Tables(LockRatesPlanRoomData.TABLE_LockRatesPlanRooms).DefaultView
        dvInventory = dsAva.Tables(RoomsInventoryData.TBL_ROOMS_INVENTORY).DefaultView
        dvlockrp = dsAva.Tables(lockRatePlanData.TABLE_LockRatePlan).DefaultView
        dvRooms = dsAva.Tables("TipoHabitaciones_Hotel").DefaultView


        With New RatePlanFacade
            dsrateplans = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With

        '///////////////////////////////////////////////////////// Quitar, estas columnas son del datatable para la vista
        dt.Columns.Add(New DataColumn("RATEPLAN", GetType(String)))
        dt.Columns.Add(New DataColumn("ROOMCODE", GetType(String)))
        dt.Columns.Add(New DataColumn("ROOMNAME", GetType(String)))

        For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))
            dt.Columns.Add(New DataColumn(CDate(txtDateFrom.Text).AddDays(i), GetType(String)))
        Next
        Dim dvRP, dvRes, dvgral, dvlocks As DataView
        Dim drRatePlanDate() As DataRow
        Dim drAvailableForDay() As DataRow


        'Checar Lock Rate Plans
        'dvlockrproom.RowFilter = LockRatesPlanRoomData.FIELD_StartDate & "<='" & _d & "' and " & LockRatesPlanRoomData.FIELD_EndDate & ">='" & _d & "'"

        'Checar si tiene busqueda por RatePlan

        If (searchByRatePlan.Checked) Then
            'RatePlan Row
            Dim roomTest As DataSet
            drrp = dt.NewRow()
            drrp("RATEPLAN") = ddlRatePlanFilter.SelectedValue
            dt.Rows.Add(drrp)
            With New RoomFacade
                roomTest = .getRooms(Me.cInfoActual.Hotel)
            End With

            For Each drroom As DataRow In roomTest.Tables(dsrooms.TBL_ROOM_HOTEL).Rows
                dsLockRoomTypes = GetLockRoomTypes(MyBase.cInfoActual.Hotel, ddlRatePlanFilter.SelectedValue, dateStart, dateEnd, drroom(dsrooms.FLD_ID_ROOM_HOTEL))

                Dim dtLock As DataTable = IIf(dsLockRoomTypes.Tables(0).Rows.Count <> 0, dsLockRoomTypes.Tables(0), dsLockRoomTypes.Tables(1))
                If Not hotelData.Tables(0).Rows(0)("AvailOnPortal") Then

                    'IF HOTEL IS NOT AVAILABLE ON PORTAL IS CLOSED'
                    dr1 = dt.NewRow()
                    dr1("ROOMNAME") = drroom(dsrooms.FLD_NOMBRE)
                    dr1("ROOMCODE") = drroom(dsrooms.FLD_ROOM_CODE)

                    'For each day
                    For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))
                        dr1(CDate(txtDateFrom.Text).AddDays(i)) = "C"
                    Next
                Else
                    If dtLock.Rows.Count <> 0 Then
                        dr1 = dt.NewRow()
                        dr1("ROOMNAME") = drroom(dsrooms.FLD_NOMBRE)
                        dr1("ROOMCODE") = drroom(dsrooms.FLD_ROOM_CODE)
                        
                        Dim rangeDays(DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))) As String

                        'IF THERE ARE ROWS ON TABLE THEN FOR EACH ROW CHECK CLOSURES
                        For Each drLock As DataRow In dtLock.Rows

                            'AVAILABILITY STATUS AVAILABLE ON NEW TABLE
                            Dim startDay = drLock("StartDate").ToString()
                            Dim endDay = drLock("EndDate").ToString()


                            ' FOR EACH DAY...
                            For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))

                                'Agarrar los dias y ver cual esta cerrado que tengan un cierre

                                rangeDays(i) = IIf(rangeDays(i) = Nothing, "" & CDate(txtDateFrom.Text).AddDays(i).ToString("MM/dd/yyyy") & ",", rangeDays(i))

                                Dim statusString As String = IIf((CDate(txtDateFrom.Text).AddDays(i) <= CDate(endDay) And CDate(txtDateFrom.Text).AddDays(i) >= CDate(startDay)), _
                                        drLock("StatusAvail").ToString(), "")

                                rangeDays(i) &= IIf(statusString <> "", statusString, "")
                                ' SEARCH THE ROOM ON DSROOMS (ROOMS WITH RATES)


                                dr1(CDate(txtDateFrom.Text).AddDays(i)) = IIf(rangeDays(i).Split(",")(1) = "", "O", rangeDays(i).Split(",")(1))

                            Next

                        Next

                        dt.Rows.Add(dr1)

                    Else
                        'NOT AVA ON NEW TABLE (LockRoomType)
                        dr1 = dt.NewRow()
                        dr1("ROOMNAME") = drroom(dsrooms.FLD_NOMBRE)
                        dr1("ROOMCODE") = drroom(dsrooms.FLD_ROOM_CODE)

                        For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))


                            dr1(CDate(txtDateFrom.Text).AddDays(i)) = "O"
                        Next
                        dt.Rows.Add(dr1)
                    End If


                End If
            Next
        Else

            '---------------por cada rate plan ---------------------------
            For Each drrateplan As DataRow In dsrateplans.Tables(dsrateplans.RATEPLAN_TABLE).Rows
                Dim roomTest As DataSet
                'RatePlan Row
                drrp = dt.NewRow()
                drrp("RATEPLAN") = drrateplan(dsrateplans.FIELD_CODIGOTARIFA)
                dt.Rows.Add(drrp)
                With New RoomFacade
                    roomTest = .getRooms(Me.cInfoActual.Hotel)
                End With
                ''''''''''''''''por cada habitacion ''''''''''''''''''''''''''''''''''''
                For Each drroom As DataRow In roomTest.Tables(dsrooms.TBL_ROOM_HOTEL).Rows


                    dsLockRoomTypes = GetLockRoomTypes(MyBase.cInfoActual.Hotel, drrateplan(dsrateplans.FIELD_CODIGOTARIFA), dateStart, dateEnd, drroom(dsrooms.FLD_ID_ROOM_HOTEL))

                    Dim dtLock As DataTable = IIf(dsLockRoomTypes.Tables(0).Rows.Count <> 0, dsLockRoomTypes.Tables(0), dsLockRoomTypes.Tables(1))
                    'Not Available Portal
                    If Not hotelData.Tables(0).Rows(0)("AvailOnPortal") Then

                        'IF HOTEL IS NOT AVAILABLE ON PORTAL IS CLOSED'
                        dr1 = dt.NewRow()
                        dr1("ROOMNAME") = drroom(dsrooms.FLD_NOMBRE)
                        dr1("ROOMCODE") = drroom(dsrooms.FLD_ROOM_CODE)

                        'For each day
                        For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))
                            dr1(CDate(txtDateFrom.Text).AddDays(i)) = "C"
                        Next
                    Else
                        If dtLock.Rows.Count <> 0 Then
                            dr1 = dt.NewRow()
                            dr1("ROOMNAME") = drroom(dsrooms.FLD_NOMBRE)
                            dr1("ROOMCODE") = drroom(dsrooms.FLD_ROOM_CODE)
                            Dim rangeDays(DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))) As String

                            'IF THERE ARE ROWS ON TABLE THEN FOR EACH ROW CHECK CLOSURES
                            For Each drLock As DataRow In dtLock.Rows

                                'AVAILABILITY STATUS AVAILABLE ON NEW TABLE
                                Dim startDay = drLock("StartDate").ToString()
                                Dim endDay = drLock("EndDate").ToString()


                                ' FOR EACH DAY...
                                For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))

                                    'Agarrar los dias y ver cual esta cerrado que tengan un cierre

                                    rangeDays(i) = IIf(rangeDays(i) = Nothing, "" & CDate(txtDateFrom.Text).AddDays(i).ToString("MM/dd/yyyy") & ",", rangeDays(i))

                                    Dim statusString As String = IIf((CDate(txtDateFrom.Text).AddDays(i) <= CDate(endDay) And CDate(txtDateFrom.Text).AddDays(i) >= CDate(startDay)), _
                                            drLock("StatusAvail").ToString(), "")

                                    rangeDays(i) &= IIf(statusString <> "", statusString, "")
                                    ' SEARCH THE ROOM ON DSROOMS (ROOMS WITH RATES)


                                    dr1(CDate(txtDateFrom.Text).AddDays(i)) = IIf(rangeDays(i).Split(",")(1) = "", "O", rangeDays(i).Split(",")(1))

                                Next

                            Next

                            dt.Rows.Add(dr1)

                        Else
                            'NOT AVA ON NEW TABLE (LockRoomType)
                            dr1 = dt.NewRow()
                            dr1("ROOMNAME") = drroom(dsrooms.FLD_NOMBRE)
                            dr1("ROOMCODE") = drroom(dsrooms.FLD_ROOM_CODE)

                            
                            For i As Integer = 0 To DateDiff(DateInterval.Day, CDate(txtDateFrom.Text), CDate(txtDateTo.Text))


                                dr1(CDate(txtDateFrom.Text).AddDays(i)) = "O"
                            Next
                            dt.Rows.Add(dr1)
                        End If


                    End If
                Next

            Next
        End If


        Dim dv As New DataView(dt)
        Return dv

    End Function
   


    Private Sub dgRatesHorizontal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatesHorizontal.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            For i As Integer = 0 To DifDays


                ' Ignore Blank Spaces

                CType(e.Item.Cells(3 + i), TableCell).BorderColor = Drawing.Color.Black

                Dim pos As Integer = -1
                pos = CType(e.Item.Cells(3 + i), TableCell).Text.IndexOf("nbsp")
                If pos <> 1 Then
                    'If CType(e.Item.Cells(3 + i), TableCell).Text.Length > 1 AndAlso (CType(e.Item.Cells(3 + i), TableCell).Text <> "NA" And CType(e.Item.Cells(3 + i), TableCell).Text <> "NR") Then
                    'IF THERE ARE MORE THAN 1 STATUS IT WILL CONCAT (EXCEPT FOR NR AND NA)'
                    'CType(e.Item.Cells(3 + i), TableCell).Text = CType(e.Item.Cells(3 + i), TableCell).Text.Substring((CType(e.Item.Cells(3 + i), TableCell).Text.Length - 1), 1)
                    'End If
                    Select Case CType(e.Item.Cells(3 + i), TableCell).Text.Substring(pos + 1).ToUpper.Trim
                        Case "O"
                            CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.White
                            CType(e.Item.Cells(3 + i), TableCell).BackColor = System.Drawing.Color.Green
                            CType(e.Item.Cells(3 + i), TableCell).Text = PortalCulture.GetString("M0UT02715")
                            CType(e.Item.Cells(3 + i), TableCell).HorizontalAlign = HorizontalAlign.Center
                        Case "C"
                            CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.White
                            CType(e.Item.Cells(3 + i), TableCell).BackColor = System.Drawing.Color.Red
                            CType(e.Item.Cells(3 + i), TableCell).Text = PortalCulture.GetString("M0UT02716")
                            CType(e.Item.Cells(3 + i), TableCell).HorizontalAlign = HorizontalAlign.Center
                        Case "N"
                            CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.White
                            CType(e.Item.Cells(3 + i), TableCell).BackColor = System.Drawing.Color.LightSteelBlue
                            CType(e.Item.Cells(3 + i), TableCell).Text = PortalCulture.GetString("M0UT02717")
                            CType(e.Item.Cells(3 + i), TableCell).HorizontalAlign = HorizontalAlign.Center
                    End Select
                    'If CType(e.Item.Cells(3 + i), TableCell).Text.Substring(pos + 1, 1).ToUpper.Trim = "C" Then
                    '    CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.Red
                    'ElseIf CType(e.Item.Cells(3 + i), TableCell).Text.Substring(pos + 1, 1).ToUpper.Trim = "N" Then
                    '    CType(e.Item.Cells(3 + i), TableCell).ForeColor = System.Drawing.Color.LightSteelBlue
                    'End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Header Then
            CType(e.Item.Cells(0), TableCell).Text = PortalCulture.GetString("00016")
            CType(e.Item.Cells(1), TableCell).Text = PortalCulture.GetString("M000432")
            CType(e.Item.Cells(2), TableCell).Text = PortalCulture.GetString("M000066")

            CType(e.Item.Cells(0), TableCell).Width = 90
            CType(e.Item.Cells(1), TableCell).Width = 60
            For i As Integer = 0 To DifDays
                With CType(e.Item.Cells(3 + i), TableCell)
                    Dim mes As String, str As String
                    mes = MonthName(CInt(.Text.Substring(0, .Text.IndexOf("/"))), True)
                    str = .Text.Substring(.Text.IndexOf("/") + 1)
                    .Text = mes & "/" & str.Substring(0, str.IndexOf("/")) 'CDate(CType(e.Item.Cells(0), TableCell).Text).ToString("MMM/dd")


                End With

            Next
        End If

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData

        'Rooms Load

        Dim idAsoc As Integer = Me.GetIdAsociation

        idroom = Request.QueryString("Room")
        If idroom <> 0 Then
            Me.ddlRooms.SelectedValue = idroom
        Else
            Try
                idroom = ddlRooms.SelectedValue
            Catch ex As Exception
                idroom = 0
            End Try
        End If




        hplHide.NavigateUrl = "javascript:Ocultar('0');"
        hplShow.NavigateUrl = "javascript:Ocultar('1');"



        If Not IsPostBack Then

            
            CargaFechas(True)
            hplShow.Style.Add("display", "")
            hplHide.Style.Add("display", "none")
            Me.ddlStatus.Items.Clear()
            Me.ddlStatus.Items.Add("Select")
            Me.ddlStatus.Items.Add("O")
            Me.ddlStatus.Items.Add("C")
            Me.ddlStatus.Items.Add("N")
            Me.txtInicio.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtFinal.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")

            Me.txtDateFrom.Text = Date.Today.ToString("MM/dd/yyyy")
            Me.txtDateTo.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")

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

            ' CONFIGURACION DEL CIERRE
            ddlRateplans.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
            ddlRateplans.DataValueField = ds.FIELD_IDRATEPLAN
            ddlRateplans.DataSource = ds
            ddlRateplans.DataBind()
            ddlRateplans.Items.Insert(0, PortalCulture.GetString("M000271"))
            ddlRateplans.Items(0).Value = 0

            ' FILTRO POR RATEPLAN
            ddlRatePlanFilter.DataTextField = "texto" 'ds.FIELD_CODIGOTARIFA
            ddlRatePlanFilter.DataValueField = ds.FIELD_IDRATEPLAN
            ddlRatePlanFilter.DataSource = ds
            ddlRatePlanFilter.DataBind()

            
            With New Portal.Hotel.Facade.RoomFacade
                Rooms = .getRooms(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture)
            End With
            Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")

            Rooms.AcceptChanges()



            ddlRooms.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
            ddlRooms.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
            ddlRooms.DataSource = Rooms
            ddlRooms.DataBind()
            ddlRooms.Items.Insert(0, PortalCulture.GetString("M000271"))
            ddlRooms.Items(0).Value = 0

            ddlRooms.SelectedValue = 0
            dsRooms = Rooms
            idroom = ddlRooms.SelectedValue
            

        End If
        Me.ResizefrmPrincipal()


        'Enlazar el control con el evento
        'banIni = 0
        'Page_PreRender1(sender, e)


        CtlMensajeRuleConf1.loadRooms(MyBase.cInfoActual.Hotel)
        CtlMensajeRuleConf1.loadRatePlans(MyBase.cInfoActual.Hotel)

        btnSave.OnClientClick = String.Format("return FireUpdateStatus('{0}');", PortalCulture.GetString("00608"))
        btnLoad.OnClientClick = String.Format("return FireUpdateStatus('{0}');", PortalCulture.GetString("00608"))
        ' Page.ClientScript.ValidateEvent(Me.btnLoad.UniqueID, Me.ToString())
    End Sub
    Public Function getFunctionShow() As String
        Return "javascript:if (evalDates()) { " & _
             CtlMensajes1.getShow(Me.btnSave.ClientID, "", PortalCulture.GetString("00608")) & _
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

    Private Sub RoomClosure_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        
    End Sub


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

        Me.lblNoArrivals.Text = PortalCulture.GetString("00152")
        Me.lblOpen.Text = PortalCulture.GetString("00150")
        Me.lblClose.Text = PortalCulture.GetString("00151")
        Me.lblStart.Text = PortalCulture.GetString("00154", True)

        'lblNodisponible.Text = PortalCulture.GetString("00532")
        'Me.lblAplyAllPlan.Text = PortalCulture.GetString("00795")

        btnLoad.Text = PortalCulture.GetString("00149")
        Me.lblRoomToClosure.Text = PortalCulture.GetString("00170", True)
        ddlRooms.Items(0).Text = PortalCulture.GetString("M000271")



        CargaFechas(False)

        
        'LoadDataByRP()
        lblTitle.Text = PortalCulture.GetString("00001234")

        'chkMinPrice.Style.Add("display", "")
        'lblMinPrice.Style.Add("display", "")
        'txtMinPrice.Style.Add("display", "")
        'chkMaxPrice.Style.Add("display", "")
        'lblMaxPrice.Style.Add("display", "")
        'txtMaxPrice.Style.Add("display", "")


        'Me.divOnRequest.Style.Add("display", "")
        'Me.divddl.Style.Add("display", "none")
        

        Me.ddlStatus.Items(0).Text = PortalCulture.GetString("M000272")
        Me.ddlStatus.Items(0).Value = "0"

        Me.ddlStatus.Items(1).Text = PortalCulture.GetString("00150")
        Me.ddlStatus.Items(2).Text = PortalCulture.GetString("00151")
        Me.ddlStatus.Items(3).Text = PortalCulture.GetString("00152")

        Me.ddlStatus.Items(1).Value = "O"
        Me.ddlStatus.Items(2).Value = "C"
        Me.ddlStatus.Items(3).Value = "N"

        'Me.RbdRatePlan.Text = PortalCulture.GetString("00016")
        Me.lblFrom.Text = PortalCulture.GetString("00108")
        Me.lblTo.Text = PortalCulture.GetString("00109")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Bloqueos, Me.btnSave, "M")
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Bloqueos, btnLoad, "R")
        hplHide.Text = PortalCulture.GetString("00454")
        hplShow.Text = PortalCulture.GetString("00453")
        Me.lblShowAvail.Text = PortalCulture.GetString("00455", True)
        btnSave.Text = PortalCulture.GetString("A00153")
        'lblRatePlan.Text = PortalCulture.GetString("00016")



        lblErrorStatus.Text = PortalCulture.GetString("01161")

        spanMSG.InnerText = PortalCulture.GetString("01571")
    End Sub

    Private Sub CargaFechas(ByVal change As Boolean)

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not Page.IsValid Then Return
        lblErrorGeneral.Visible = False
        Dim drhotel As DataRow = Me.HotelInfo
        Dim idioma As String

        Dim allRooms As Boolean = (ddlRooms.SelectedValue = "0")
        Dim allRatePlans As Boolean = ddlRateplans.SelectedValue = "0"

        If ddlStatus.SelectedValue <> "0" Then



            idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
            If Not InvalidDate() Then
                Dim nota As String
                nota &= "Cierre en el hotel " & MyBase.cInfoActual.HotelName
                If allRooms Then
                    nota &= " en todas las habitaciones,"
                Else
                    nota &= " en la habitación " & ddlRooms.SelectedValue
                End If
                If allRatePlans Then
                    nota &= " en todos los Rate Plans"
                Else
                    nota &= " en el Rate Plan " & ddlRateplans.SelectedValue
                End If

                nota &= " del " & txtInicio.Text & " al " & txtFinal.Text & " en los días "

                nota &= " Status: " & ddlStatus.SelectedItem.Text & ". "


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
                Dim dsrateplans As RatePlanData, dsrooms As RoomsHotelData


                'GetLockRoomTypes(MyBase.cInfoActual.Hotel,idRatePlan:=,StartDate:=,EndDate:=,IdTipoHabitacionHotel:=)


                If allRatePlans Then
                    With New RatePlanFacade
                        dsrateplans = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=Me.GetIdAsociation, DeleteFilter:=1)
                    End With
                    If allRooms Then
                        'ALL ROOMS AND ALL RATEPLANS
                        With New RoomFacade
                            dsrooms = .getRooms(Me.cInfoActual.Hotel)
                        End With

                        For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                            For Each drroom As DataRow In dsrooms.Tables(0).Rows
                                CheckLockRoomTypes(Me.cInfoActual.Hotel, drrateplan(dsrateplans.FIELD_CODIGOTARIFA), txtInicio.Text, txtFinal.Text, drroom(dsrooms.FLD_ID_ROOM_HOTEL), ddlStatus.SelectedItem.Value)
                            Next
                        Next
                    Else
                        'ALL RATEPLANS 
                        For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                            CheckLockRoomTypes(Me.cInfoActual.Hotel, drrateplan(dsrateplans.FIELD_CODIGOTARIFA), txtInicio.Text, txtFinal.Text, ddlRooms.SelectedItem.Value, ddlStatus.SelectedItem.Value)
                        Next
                    End If
                Else
                    If allRooms Then
                        'ALL ROOMS
                        With New RoomFacade
                            dsrooms = .getRooms(Me.cInfoActual.Hotel)
                        End With
                        For Each drroom As DataRow In dsrooms.Tables(0).Rows
                            CheckLockRoomTypes(Me.cInfoActual.Hotel, ddlRateplans.SelectedItem.Value, txtInicio.Text, txtFinal.Text, drroom(dsrooms.FLD_ID_ROOM_HOTEL), ddlStatus.SelectedItem.Value)
                        Next
                    Else
                        'NORMAL LOCK
                        CheckLockRoomTypes(Me.cInfoActual.Hotel, ddlRateplans.SelectedItem.Value, txtInicio.Text, txtFinal.Text, ddlRooms.SelectedItem.Value, ddlStatus.SelectedItem.Value)
                    End If
                End If
                'Me.guardalog("/Pages/AvailabilityRestrictions.aspx", PaginaBase.acciones.Crear, sNotas, sReference, sDatos, sDatosDespues, sDatoCorreo)

                DeleteLockRoomType(Me.cInfoActual.Hotel, ddlRateplans.SelectedValue, txtFinal.Text, ddlRooms.SelectedValue)
                Me.guardalog("/Pages/RoomClosure.aspx", PaginaBase.acciones.Crear, nota)
                iniCtrl()
            End If
        Else

            lblErrorGeneral.Text = PortalCulture.GetString("01161")
            lblErrorGeneral.Visible = True
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

    Private Function closeRateplan(ByVal nota As String, Optional ByVal RatePlan As String = "", Optional ByVal loadStatusRateplan As Boolean = True, Optional ByVal splan As String = "") As Boolean
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
                            copyrowRP(.Rows(i), modrow, txtStartDate, rowEndDate, dstrans, False)
                        Else
                            copyrowRP(.Rows(i), modrow, txtStartDate, txtEndDate, dstrans, False)
                            copyrowRP(.Rows(i), .Rows(i), txtEndDate.AddDays(1), rowEndDate, dstrans, False)
                        End If
                    Else
                        If txtEndDate >= rowEndDate Then
                            copyrowRP(.Rows(i), modrow, rowStartDate, rowEndDate, dstrans, False)
                        Else
                            copyrowRP(.Rows(i), modrow, rowStartDate, txtEndDate, dstrans, False)
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
                    copyrowRP(modrow, modrow, progresDate, rowStartDate.AddDays(-1), dstrans, False)
                End If

                progresDate = rowEndDate.AddDays(1)
            Next

            If progresDate <= txtEndDate Then
                copyrowRP(modrow, modrow, progresDate, txtEndDate, dstrans, False)
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
            copyrowRP(modrow, modrow, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text), dstrans, False)

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
                End If
            End If
        End With
        If loadStatusRateplan Then
           
            Dim dsRatePlan As RatePlanData
            With New RatePlanFacade
                dsRatePlan = .GetDataRatePlan(ddlRateplans.SelectedValue, Me.cInfoActual.Hotel)
            End With
            If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
                ddlRateplans.SelectedValue = dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0).Item(dsRatePlan.FIELD_CODIGOTARIFA)
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
        ddlRateplans.Attributes.Add("display", "none")
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
            '.Item(cAva.FIELD_HurdleRate) = row.Item(cAva.FIELD_HurdleRate)
            '.Item(cAva.FIELD_CancelPrioridad) = row.Item(cAva.FIELD_CancelPrioridad)
            '.Item(cAva.FIELD_DepartureRestricted) = row.Item(cAva.FIELD_DepartureRestricted)
            .Item(cAva.FIELD_OnRequest) = modRow.Item(cAva.FIELD_OnRequest)
            .Item(cAva.FIELD_AplyWeek) = modRow.Item(cAva.FIELD_AplyWeek)

            

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
    Private Function copyrowRP(ByVal row As DataRow, ByVal modrow As DataRow, ByVal fechainicio As DateTime, ByVal fechafinal As DateTime, ByVal cAva As lockRatePlanData, ByVal borrado As Boolean)
        Dim newrow As DataRow
        newrow = cAva.Tables(cAva.TABLE_LockRatePlan).NewRow
        With newrow
            .Item(cAva.FIELD_idHotel) = row.Item(cAva.FIELD_idHotel)
            .Item(cAva.FIELD_StartDate) = fechainicio
            .Item(cAva.FIELD_EndDate) = fechafinal
            .Item(cAva.FIELD_RatePlan) = row.Item(cAva.FIELD_RatePlan)
            .Item(cAva.FIELD_AplyWeek) = row.Item(cAva.FIELD_AplyWeek)



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

            'If Me.chkMinPrice.Checked AndAlso Me.txtMinPrice.Text <> "" Then
            '    .Item(NewDs.FIELD_MinPrice) = CInt(Val(txtMinPrice.Text))
            'End If
            'If Me.chkMaxPrice.Checked AndAlso Me.txtMaxPrice.Text <> "" Then
            '    .Item(NewDs.FIELD_MaxPrice) = CInt(Val(txtMaxPrice.Text))
            'End If

            
                .Item(NewDs.FIELD_statusAvailability) = Me.ddlStatus.SelectedValue
            'Else
            '    .Item(NewDs.FIELD_statusAvailability) = Me.ddlStatus.Items(0).Value
            .Item(NewDs.FIELD_AplyWeek) = GetAplyWeek()


            


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
            
                .Item(lockRatePlanData.FIELD_StatusAvailability) = Me.ddlStatus.SelectedValue

            .Item(lockRatePlanData.FIELD_AplyWeek) = GetAplyWeek()


           

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

                            drRatePlanDate = dt.Select( _
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
           


            btnload_Click(Me, e:=Nothing)


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



    Private Function GetLockRoomTypes(ByVal idHotel As Integer, ByVal idRatePlan As String, ByVal StartDate As String, ByVal EndDate As String, ByVal IdTipoHabitacionHotel As String) As DataSet
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.SelectCommand = New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        sqlConn.Open()
        trans = sqlConn.BeginTransaction
        With dscommand
            Try
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.CommandText = "spGetLockRoomTypes"
                .SelectCommand.Connection = sqlConn
                .SelectCommand.Transaction = trans
                With .SelectCommand
                    .Parameters.Clear()
                    .Parameters.Add(New SqlClient.SqlParameter("@StartDate", SqlDbType.Char, 10))
                    .Parameters("@StartDate").Value = CType(StartDate, DateTime).ToString("yyyy/MM/dd")
                    .Parameters.Add(New SqlClient.SqlParameter("@EndDate", SqlDbType.Char, 10))
                    .Parameters("@EndDate").Value = CType(EndDate, DateTime).ToString("yyyy/MM/dd")
                    .Parameters.Add(New SqlClient.SqlParameter("@idhotel", SqlDbType.Int))
                    .Parameters("@idhotel").Value = idHotel
                    .Parameters.Add(New SqlClient.SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4))
                    .Parameters("@IdRatePlan").Value = idRatePlan
                    .Parameters.Add(New SqlClient.SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int))
                    .Parameters("@idTipoHabitacion_Hotel").Value = IdTipoHabitacionHotel
                End With
                .Fill(data)
            Catch ex As Exception
                Dim msg As String
                msg = ex.Message
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()

            End Try
        End With
        If sqlConn.State = ConnectionState.Open Then
            trans.Rollback()
            sqlConn.Close()
        End If
        Return data
    End Function

    Public Function GetLockRoomTypesByIdHotel(ByVal idHotel As Integer) As DataSet
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.SelectCommand = New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        sqlConn.Open()
        trans = sqlConn.BeginTransaction
        With dscommand
            Try
                .SelectCommand.CommandType = CommandType.StoredProcedure
                .SelectCommand.CommandText = "spGetAllLockRoomTypes"
                .SelectCommand.Connection = sqlConn
                .SelectCommand.Transaction = trans
                With .SelectCommand
                    .Parameters.Clear()
                    .Parameters.Add(New SqlClient.SqlParameter("@idhotel", SqlDbType.Int))
                    .Parameters("@idhotel").Value = idHotel
                End With
                .Fill(data)
            Catch ex As Exception
                Dim msg As String
                msg = ex.Message
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()

            End Try
        End With
        If sqlConn.State = ConnectionState.Open Then
            trans.Rollback()
            sqlConn.Close()
        End If
        Return data
    End Function

    Public Function LockRoomTypes(ByVal idHotel As Integer, ByVal idRatePlan As String, ByVal StartDate As String, ByVal EndDate As String, ByVal IdTipoHabitacionHotel As String, ByVal StatusAvail As String, ByRef strError As String) As Boolean
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.InsertCommand = New SqlClient.SqlCommand
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        Dim cmd As New SqlClient.SqlCommand()

        'With dscommand
        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "spLockRoomTypes"
            cmd.Connection = sqlConn

            With cmd
                .Parameters.Clear()
                .Parameters.Add(New SqlClient.SqlParameter("@StartDate", SqlDbType.Char, 10))
                .Parameters("@StartDate").Value = CType(StartDate, DateTime).ToString("yyyy/MM/dd")
                .Parameters.Add(New SqlClient.SqlParameter("@EndDate", SqlDbType.Char, 10))
                .Parameters("@EndDate").Value = CType(EndDate, DateTime).ToString("yyyy/MM/dd")
                .Parameters.Add(New SqlClient.SqlParameter("@idhotel", SqlDbType.Int))
                .Parameters("@idhotel").Value = idHotel
                .Parameters.Add(New SqlClient.SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4))
                .Parameters("@IdRatePlan").Value = idRatePlan
                .Parameters.Add(New SqlClient.SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int))
                .Parameters("@idTipoHabitacion_Hotel").Value = IdTipoHabitacionHotel
                .Parameters.Add(New SqlClient.SqlParameter("@StatusAvail", SqlDbType.Char, 1))
                .Parameters("@StatusAvail").Value = StatusAvail

            End With
            sqlConn.Open()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            strError = ex.Message
            Return False
        Finally
            sqlConn.Close()
            sqlConn.Dispose()


        End Try
        'End With
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
        End If
    End Function

    Public Function DeleteLockTypeRoomsByIdRatePlan(ByVal PromoCode As String, ByVal idHotel As Integer, ByRef strError As String) As Boolean
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.InsertCommand = New SqlClient.SqlCommand
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        Dim cmd As New SqlClient.SqlCommand()

        'With dscommand
        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "spDeleteLockTypeRoomsByIdRarePlan"
            cmd.Connection = sqlConn

            With cmd
                .Parameters.Clear()
                .Parameters.Add(New SqlClient.SqlParameter("@IdRatePlan", SqlDbType.Char, 4))
                .Parameters("@IdRatePlan").Value = PromoCode
                .Parameters.Add(New SqlClient.SqlParameter("@IdHotel", SqlDbType.Int))
                .Parameters("@IdHotel").Value = idHotel

            End With
            sqlConn.Open()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            strError = ex.Message
            Return False
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
        End Try
        'End With
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
        End If
    End Function

    Public Function UpdateLockRoomType(ByVal idHotel As Integer, ByVal idRatePlan As String, ByVal StartDate As String, ByVal EndDate As String, ByVal IdTipoHabitacionHotel As String, ByVal StatusAvail As String, Optional OldStart As String = "", Optional OldEnd As String = "")
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.UpdateCommand = New SqlClient.SqlCommand
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        Dim cmd As New SqlClient.SqlCommand()

        'With dscommand
        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "spUpdateLockRoomTypes"
            cmd.Connection = sqlConn

            With cmd
                .Parameters.Clear()
                .Parameters.Add(New SqlClient.SqlParameter("@StartDate", SqlDbType.Char, 10))
                .Parameters("@StartDate").Value = CType(StartDate, DateTime).ToString("yyyy/MM/dd")
                .Parameters.Add(New SqlClient.SqlParameter("@EndDate", SqlDbType.Char, 10))
                .Parameters("@EndDate").Value = CType(EndDate, DateTime).ToString("yyyy/MM/dd")
                .Parameters.Add(New SqlClient.SqlParameter("@idhotel", SqlDbType.Int))
                .Parameters("@idhotel").Value = idHotel
                .Parameters.Add(New SqlClient.SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4))
                .Parameters("@IdRatePlan").Value = idRatePlan
                .Parameters.Add(New SqlClient.SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int))
                .Parameters("@idTipoHabitacion_Hotel").Value = IdTipoHabitacionHotel
                .Parameters.Add(New SqlClient.SqlParameter("@StatusAvail", SqlDbType.Char, 1))
                .Parameters("@StatusAvail").Value = StatusAvail
                .Parameters.Add(New SqlClient.SqlParameter("@OldStart", SqlDbType.Char, 10))
                .Parameters.Add(New SqlClient.SqlParameter("@OldEnd", SqlDbType.Char, 10))
                If (OldStart <> "") Then
                    .Parameters("@OldStart").Value = CType(OldStart, DateTime).ToString("yyyy/MM/dd")
                Else
                    .Parameters("@OldStart").Value = DBNull.Value
                End If
                If (OldEnd <> "") Then
                    .Parameters("@OldEnd").Value = CType(OldEnd, DateTime).ToString("yyyy/MM/dd")
                Else
                    .Parameters("@OldEnd").Value = DBNull.Value
                End If
            End With
            sqlConn.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim msg As String
            msg = ex.Message
        Finally
            sqlConn.Close()
            sqlConn.Dispose()


        End Try
        'End With
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
        End If
    End Function
    Public Function DeleteLockRoomType(ByVal idHotel As Integer, ByVal idRatePlan As String, ByVal EndDate As String, ByVal IdTipoHabitacionHotel As String, Optional ByVal StartDate As String = "")
        Dim data As New DataSet
        Dim dscommand As New SqlClient.SqlDataAdapter
        Dim ConnectionString As String
        ConnectionString = ConfigurationSettings.AppSettings("HotelConnectionString")
        dscommand.DeleteCommand = New SqlClient.SqlCommand
        Dim sqlConn As New SqlClient.SqlConnection(ConnectionString)
        Dim cmd As New SqlClient.SqlCommand()

        'With dscommand
        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "spDeleteLockRoomTypes"
            cmd.Connection = sqlConn

            With cmd
                .Parameters.Clear()
                .Parameters.Add(New SqlClient.SqlParameter("@StartDate", SqlDbType.Char, 10))
                If (StartDate <> "") Then
                    .Parameters("@StartDate").Value = CType(StartDate, DateTime).ToString("yyyy/MM/dd")
                Else
                    .Parameters("@StartDate").Value = DBNull.Value
                End If
                .Parameters.Add(New SqlClient.SqlParameter("@EndDate", SqlDbType.Char, 10))
                .Parameters("@EndDate").Value = CType(EndDate, DateTime).ToString("yyyy/MM/dd")
                .Parameters.Add(New SqlClient.SqlParameter("@idhotel", SqlDbType.Int))
                .Parameters("@idhotel").Value = idHotel
                .Parameters.Add(New SqlClient.SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4))
                .Parameters("@IdRatePlan").Value = idRatePlan
                .Parameters.Add(New SqlClient.SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int))
                .Parameters("@idTipoHabitacion_Hotel").Value = IdTipoHabitacionHotel
            End With
            sqlConn.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim msg As String
            msg = ex.Message
        Finally
            sqlConn.Close()
            sqlConn.Dispose()


        End Try
        'End With
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
        End If
    End Function

    Public Sub CheckLockRoomTypes(ByVal idHotel As Integer, ByVal idRatePlan As String, ByVal StartDate As String, ByVal EndDate As String, ByVal IdTipoHabitacionHotel As String, ByVal StatusAvail As String)
        Dim dsLockRoomTypes As DataSet
        Dim drLock As DataRow
        Dim deleteRows As Boolean = False
        Dim sameStatus As Boolean = False
        Dim strError As String = String.Empty
        dsLockRoomTypes = GetLockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel)

        If (dsLockRoomTypes.Tables(1).Rows.Count <> 0) Then
            'IF THERE ARE ROWS ON THE RANGE OF THE DATES...
            For Each drLock In dsLockRoomTypes.Tables(1).Rows
                'If is not the first it will be another lock, so, dont unify 
                If (StatusAvail = drLock("StatusAvail") And dsLockRoomTypes.Tables(1).Rows.IndexOf(drLock) = 0) Then
                    'If they have the same status unify the lock
                    If (CType(drLock("StartDate"), DateTime) > CType(StartDate, DateTime)) Then
                        If (CType(drLock("EndDate"), DateTime) > CType(EndDate, DateTime)) Then
                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, drLock("EndDate"), IdTipoHabitacionHotel, StatusAvail, OldStart:=drLock("StartDate"))
                        Else
                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, OldEnd:=drLock("EndDate"), OldStart:=drLock("StartDate"))
                        End If
                    Else
                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("StartDate"), EndDate, IdTipoHabitacionHotel, StatusAvail, OldEnd:=drLock("EndDate"))
                    End If
                    sameStatus = True
                Else
                    If (sameStatus) Then
                        If (CType(drLock("EndDate"), DateTime) <= CType(EndDate, DateTime)) Then
                            'If existing lock is longer than lock dont delete, just update
                            DeleteLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("EndDate"), IdTipoHabitacionHotel, drLock("StartDate"))
                        Else
                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, CType(EndDate, DateTime).AddDays(1).ToString("MM/dd/yyyy"), drLock("EndDate"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldStart:=drLock("StartDate"))
                        End If
                    Else
                        If (dsLockRoomTypes.Tables(1).Rows.Count > 1) Then
                            'If there are more than 1 row delete or update
                            If (dsLockRoomTypes.Tables(1).Rows.Count >= 2) Then
                                'If there are more than 2 rows on the table it will need an update 
                                If (dsLockRoomTypes.Tables(1).Rows.IndexOf(drLock) = 0) Then
                                    'If is the first one, then, according to the start and end date will be updated
                                    If (StartDate <= drLock("StartDate")) Then
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, OldStart:=drLock("StartDate"), OldEnd:=drLock("EndDate"))
                                    Else
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("StartDate"), CType(StartDate, DateTime).AddDays(-1).ToString("MM/dd/yyyy"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldEnd:=drLock("EndDate"))
                                        LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
                                    End If
                                Else
                                    If (drLock("StartDate") >= StartDate And drLock("EndDate") <= EndDate) Then
                                        DeleteLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("EndDate"), IdTipoHabitacionHotel, drLock("StartDate"))
                                    Else
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, CType(EndDate, DateTime).AddDays(1).ToString("MM/dd/yyyy"), drLock("EndDate"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldStart:=drLock("StartDate"))
                                    End If
                                End If
                            Else
                                If (drLock("StartDate") >= StartDate And drLock("EndDate") <= EndDate) Then
                                    DeleteLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("EndDate"), IdTipoHabitacionHotel, drLock("StartDate"))
                                End If
                                deleteRows = True
                            End If
                        Else
                            If (drLock("StartDate") = StartDate) Then
                                If (drLock("EndDate") = EndDate) Then
                                    'Exact match
                                    UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail)
                                Else
                                    'Same start date
                                    If (drLock("EndDate") < EndDate) Then
                                        'if its shorter replace
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, OldEnd:=drLock("EndDate"))
                                    Else
                                        LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, CType(EndDate, DateTime).AddDays(1).ToString("MM/dd/yyyy"), drLock("EndDate"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldStart:=drLock("StartDate"))
                                    End If
                                End If
                            Else
                                If (drLock("EndDate") = EndDate) Then
                                    'Same end date
                                    If (StartDate < drLock("StartDate")) Then
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, OldStart:=drLock("StartDate"))
                                    Else
                                        LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
                                        UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("StartDate"), CType(StartDate, DateTime).AddDays(-1).ToString("MM/dd/yyyy"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldEnd:=drLock("EndDate"))
                                    End If
                                Else
                                    'On the range
                                    If (CType(StartDate, DateTime) < drLock("StartDate")) Then
                                        'Earlier
                                        If (CType(EndDate, DateTime) > drLock("EndDate")) Then
                                            'Earlier and Longer
                                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, OldStart:=drLock("StartDate"), OldEnd:=drLock("EndDate"))
                                        Else
                                            'Just Earlier
                                            LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
                                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, CType(EndDate, DateTime).AddDays(1).ToString("MM/dd/yyyy"), drLock("EndDate"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldStart:=drLock("StartDate"))
                                        End If
                                    Else
                                        If (CType(EndDate, DateTime) > drLock("EndDate")) Then
                                            'Longer
                                            LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
                                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("StartDate"), CType(StartDate, DateTime).AddDays(-1).ToString("MM/dd/yyyy"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldEnd:=drLock("EndDate"))
                                        Else
                                            'Between start and end
                                            UpdateLockRoomType(MyBase.cInfoActual.Hotel, idRatePlan, drLock("StartDate"), CType(StartDate, DateTime).AddDays(-1).ToString("MM/dd/yyyy"), IdTipoHabitacionHotel, drLock("StatusAvail"), OldEnd:=drLock("EndDate"))
                                            LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
                                            LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, CType(EndDate, DateTime).AddDays(1).ToString("MM/dd/yyyy"), drLock("EndDate"), IdTipoHabitacionHotel, drLock("StatusAvail"), strError)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            If (deleteRows) Then
                LockRoomTypes(MyBase.cInfoActual.Hotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
            End If
        Else
            'ELSE JUST INSERT THE LOCK
            LockRoomTypes(idHotel, idRatePlan, StartDate, EndDate, IdTipoHabitacionHotel, StatusAvail, strError)
        End If

    End Sub
End Class
