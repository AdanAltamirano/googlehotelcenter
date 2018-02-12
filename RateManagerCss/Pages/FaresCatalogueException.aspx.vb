Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager


Partial Class FaresCatalogueException
    Inherits PaginaBase
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
    Private Property selectedRoomCode() As String
        Get
            Return viewstate("selectedRoomCode")
        End Get
        Set(ByVal Value As String)
            viewstate("selectedRoomCode") = Value
        End Set
    End Property

    Private Property CodigoTarifa() As String
        Get
            Return ViewState("CodigoTarifa")
        End Get
        Set(ByVal Value As String)
            ViewState("CodigoTarifa") = Value
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

    Property Editando() As Boolean
        Get
            Return ViewState("Editando")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editando") = Value
        End Set
    End Property

    Const KEY_MINPRICE As String = "mintarifaAdulto"
    Const KEY_MAXPRICE As String = "maxtarifaAdulto"
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CtrlPlanFares1 As ctrlPlanFares

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim ConnectionString As String = AppSettings("HotelConnectionString")
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)        
        If Not IsPostBack Then
            Call LoadData()
            Call LoadRates()
        End If
        btnSave.Enabled = False
        lblNoroomSelected.Style("display") = "none"
        lblNoPlanRateSelected.Style("display") = "none"
        lnkCloseShowRates.Attributes.Add("onclick", "javascript:Ocultar('0','modalPage')")
        lnkCloseShowRates.ToolTip = PortalCulture.GetString("M000153")
        lnkCancel.Attributes.Add("onclick", "javascript:FireHide()")
        lnkCancel.ToolTip = PortalCulture.GetString("M000384")


        lblValidPrice.Style("display") = "none"
        lblValidPriceRes.Style("display") = "none"
        btnLoad.OnClientClick = String.Format("return FireValRoomsPlan('{0}','{1}');", ddlRooms.ClientID, ddlratesplans.ClientID)
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

    Private Sub LoadData()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        Dim RatesPlan As Portal.General.Common.Data.RatePlanData
        'Cargar meses y años.
        Dim ci As System.Globalization.CultureInfo
        Dim idAsoc As Integer = Me.GetIdAsociation

        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        ddlMonth.Items.Clear()
        For i As Integer = 1 To 12
            ddlMonth.Items.Add(MonthName(i, True))
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
        ddlRooms.DataTextField = "texto"
        ddlRooms.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        ddlRooms.DataSource = Rooms
        ddlRooms.DataBind()
        ddlRooms.Items.Insert(0, PortalCulture.GetString("M000272"))
        ddlRooms.Items(0).Value = 0
        'Cargamos los planes del Hotel
        With New RatePlanFacade
            '// @incluirPaquetesSegK = 1 omite los paquetes segmento K en caso contario se los trae.
            '// la condicion es opuesta en el sp de consulta.
            RatesPlan = .GetRatePlanByIdHotel(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            'RatesPlan = RatePlanFilter("C", RatesPlan)
        End If

        Dim links As New LinkRatePlanData
        With New LinkRatePlanFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        RatesPlan.Tables(RatePlanData.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RatePlanData.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & RatePlanData.FIELD_NAME & ",1,25)")
        'eliminar los ratesplan que ya tienen links
        Dim dv As DataView
        For Each r As DataRow In RatesPlan.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            dv = links.Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = LinkRatePlanData.FIELD_TargetRatePlan & "='" & r(RatePlanData.FIELD_IDRATEPLAN) & "'"
            'If dv.Count > 0 OrElse r(RatePlanData.FIELD_SEGMENT) = "K" Then
            '// Solo elimina los planes vinculados, debe mostrar los planes normales con segmento K, 
            '//   ya fueron filtrados los planes para paquetes.
            If dv.Count > 0 Then
                r.Delete()
            End If
        Next
        Me.ddlratesplans.DataSource = RatesPlan
        Me.ddlratesplans.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
        Me.ddlratesplans.DataTextField = "Texto" 'Portal.General.Common.Data.RatePlanData.FIELD_CODIGOTARIFA
        Me.ddlratesplans.DataBind()

        Me.ddlratesplans.Items.Insert(0, "Todos")
        ddlratesplans.Items(0).Value = "0"
        ddlratesplans.Items(0).Text = PortalCulture.GetString("M000272")
        ddlratesplans.SelectedIndex = 0

    End Sub
    Private Sub LoadRates()
        SelectedMes = ddlMonth.SelectedIndex
        SelectedYear = ddlyear.SelectedValue

        Dim datFares As FaresData
        Dim dvFares As DataView
        Dim ctrl As Control
        Dim IdAsoc As Integer = Me.GetIdAsociation
        Dim iPersonasExtras As Integer = 0

        If ddlRooms.SelectedValue <> 0 And ddlratesplans.SelectedValue <> "0" Then
            With New FaresSystem
                datFares = .GetFaresByRoomTypeId(CInt(ddlRooms.SelectedValue), PortalCulture.GetIDCulture, idAsociacion:=IdAsoc)
            End With
            datFares.Tables(FaresData.FARES_TABLE).Columns.Add("MaxPrice", GetType(System.Double))
            datFares.Tables(FaresData.FARES_TABLE).Columns.Add("MinPrice", GetType(System.Double))

            dvFares = datFares.Tables(FaresData.FARES_TABLE).DefaultView

            'dvFares.RowFilter = " tipotarifa <>'K' "
            If Me.ddlratesplans.SelectedIndex <> 0 Then
                dvFares.RowFilter &= "  " & FaresData.IDRATEPLAN_FIELD & "='" & ddlratesplans.SelectedItem.Value & "'"
            End If

            CodigoTarifa = dvFares(0)("CodigoTarifa")

            Dim dv As New DataView
            For Each dvr As DataRowView In dvFares
                dv = datFares.Tables(1).DefaultView
                dv.RowFilter = FaresData.PKIDFARES_FIELD & "=" & dvr(FaresData.PKIDFARES_FIELD)
                If dv.Count > 0 Then
                    dvr("MinPrice") = dv(0)(KEY_MINPRICE)
                    dvr("MaxPrice") = dv(0)(KEY_MAXPRICE)
                Else
                    dvr("MinPrice") = dvr(FaresData.PRICE_FIELD)
                    dvr("MaxPrice") = dvr(FaresData.PRICE_FIELD)
                End If
            Next
            Integer.TryParse(datFares.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PERSONASEXTRAS_FIELD), iPersonasExtras)
        Else
            dvFares = Nothing
            For i As Integer = 1 To 42
                ctrl = Me.FindControl("CtrlFaresExc" & (i).ToString)
                CType(ctrl, ctrlFaresExc).Show(False)
            Next
            Exit Sub
        End If

        Dim d1 As Date = New Date(SelectedYear, SelectedMes + 1, 1)
        Dim FoundRate As Boolean
        If ddlRooms.Items.Count > 0 Then
            selectedRoom = ddlRooms.SelectedValue
            selectedRoomCode = ddlRooms.SelectedItem.Text
        End If
        Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
        Dim EndDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), Date.DaysInMonth(SelectedYear, SelectedMes + 1))
        Dim FirstDay As Integer = Weekday(selectDate, FirstDayOfWeek.Sunday)

        For i As Integer = 1 To FirstDay - 1
            ctrl = Me.FindControl("CtrlFaresExc" & (i).ToString)
            CType(ctrl, ctrlFaresExc).Show(False)

        Next
        For i As Integer = Date.DaysInMonth(SelectedYear, CInt(SelectedMes + 1)) + 1 To 42
            ctrl = Me.FindControl("CtrlFaresExc" & (i).ToString)
            CType(ctrl, ctrlFaresExc).Show(False)

        Next
        For i As Integer = 0 To Date.DaysInMonth(SelectedYear, CInt(SelectedMes + 1)) - 1
            ctrl = Me.FindControl("CtrlFaresExc" & (i + FirstDay).ToString)
            CType(ctrl, ctrlFaresExc).Show(True)
            CType(ctrl, ctrlFaresExc).SetDefaultValues()

            CType(ctrl, ctrlFaresExc).Day = i + 1
            CType(ctrl, ctrlFaresExc).ShowLink = True
            CType(ctrl, ctrlFaresExc).Editable = True
            CType(ctrl, ctrlFaresExc).m_startDate = selectDate.AddDays(i).ToString("yyyy/MM/dd")
            CType(ctrl, ctrlFaresExc).m_endDate = selectDate.AddDays(i).ToString("yyyy/MM/dd")
            CType(ctrl, ctrlFaresExc).m_iHotelId = Me.cInfoActual.Hotel
            CType(ctrl, ctrlFaresExc).m_iFareId = 0            
            CType(ctrl, ctrlFaresExc).m_RoomId = ddlRooms.SelectedValue
            CType(ctrl, ctrlFaresExc).m_nameRoom = selectedRoomCode
            CType(ctrl, ctrlFaresExc).m_RatePlan = ddlratesplans.SelectedValue
            CType(ctrl, ctrlFaresExc).PersonasExtras = iPersonasExtras
            For Each dvr As DataRowView In dvFares
                If selectDate.AddDays(i).ToString("yyyy/MM/dd") >= dvr(FaresData.STARTDATE_FIELD) And selectDate.AddDays(i).ToString("yyyy/MM/dd") <= dvr(FaresData.ENDDATE_FIELD) Then
                    FoundRate = True
                    CType(ctrl, ctrlFaresExc).m_iFareId = dvr(FaresData.PKIDFARES_FIELD)                                        
                    CType(ctrl, ctrlFaresExc).LoadFare()
                    CType(ctrl, ctrlFaresExc).ReFill()
                    If (dvr(FaresData.STARTDATE_FIELD) <> selectDate.AddDays(i).ToString("yyyy/MM/dd") Or dvr(FaresData.STARTDATE_FIELD) = selectDate.AddDays(i).ToString("yyyy/MM/dd")) And (dvr(FaresData.ENDDATE_FIELD) <> selectDate.AddDays(i).ToString("yyyy/MM/dd") Or dvr(FaresData.ENDDATE_FIELD) = selectDate.AddDays(i).ToString("yyyy/MM/dd")) Then
                        CType(ctrl, ctrlFaresExc).m_iFareId = 0
                    End If
                End If
            Next
            CtrlPlanFares1.m_iRoomId = ddlRooms.SelectedValue
            CtrlPlanFares1.m_iFareId = 0
            CtrlPlanFares1.ReFill()

            If Not FoundRate Then
                CType(ctrl, ctrlFaresExc).ReFill()
            End If
            FoundRate = False            
            'Or Me.cInfoActual.UserPerfil = PaginaBase.PerfilHotel.Basico
            If (selectDate.AddDays(i).ToString("yyyy/MM/dd") < Now.Date.ToString("yyyy/MM/dd")) Then                
                CType(ctrl, ctrlFaresExc).Editable = False
                CType(ctrl, ctrlFaresExc).ShowLink = False
            Else
                btnSave.Enabled = True
            End If
        Next
    End Sub
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        If ddlRooms.SelectedIndex > 0 And ddlratesplans.SelectedIndex > 0 Then
            Call LoadRates()
            Call LoadCulture()
        Else
            Dim Ctrl As Control
            For i As Integer = 1 To 42
                Ctrl = Me.FindControl("CtrlFaresExc" & (i).ToString)
                CType(Ctrl, ctrlFaresExc).Show(False)
            Next
            btnSave.Enabled = False
            'If ddlRooms.SelectedIndex = 0 Then
            '    lblNoroomSelected.Visible = True
            'End If
            'If ddlratesplans.SelectedIndex = 0 Then
            '    lblNoPlanRateSelected.Visible = True
            'End If
        End If
    End Sub
    Private Sub LoadCulture()
        lblTitle.Text = PortalCulture.GetString("01462")
        lbltitleDate.Text = PortalCulture.GetString("00313", True)
        lblMonth.Text = PortalCulture.GetString("00311")
        lblYear.Text = PortalCulture.GetString("00312")
        lblEName.Text = PortalCulture.GetString("00170", True)
        lblERatesPlans.Text = PortalCulture.GetString("00016", True)
        Me.btnLoad.Text = PortalCulture.GetString("00149")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        Me.lblEstatus.Text = PortalCulture.GetString("01472")
        Me.lblA.Text = PortalCulture.GetString("00687")
        Me.lblAE.Text = PortalCulture.GetString("00688")
        Me.lblN.Text = PortalCulture.GetString("00689")
        Me.lblJ.Text = PortalCulture.GetString("01473")
        Me.lblNE.Text = PortalCulture.GetString("00690")
        Me.lblJE.Text = PortalCulture.GetString("01474")
        lblNoroomSelected.Text = PortalCulture.GetString("00436")
        lblNoPlanRateSelected.Text = PortalCulture.GetString("00686")
        lblValidPrice.Text = PortalCulture.GetString("00116")
        lblValidPriceRes.Text = PortalCulture.GetString("00116")
        Me.lblPreciosTarifa.Text = PortalCulture.GetString("00275")
        Me.lnkCloseShowRates.Text = PortalCulture.GetString("M000223")

        'If Not MyBase.isConfigAdolescente() Then
        'lblJ.Style("display") = "none"
        'lblJE.Style("display") = "none"
        'End If

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
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ctrl As Control
        Dim Err As Integer = 0
        Dim selectDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), 1)
        Dim EndDate As Date = New Date(SelectedYear, CInt(SelectedMes + 1), Date.DaysInMonth(SelectedYear, SelectedMes + 1))
        Dim FirstDay As Integer = Weekday(selectDate, FirstDayOfWeek.Sunday)
        Dim FareBefore As DataSet

        If Me.selectedRoom <> 0 And Me.SelectedYear <> 0 Then
            For i As Integer = 0 To Date.DaysInMonth(CInt(SelectedYear), CInt(SelectedMes + 1))
                ctrl = Me.FindControl("ctrlFaresExc" & (i + FirstDay).ToString)
                If CType(ctrl, ctrlFaresExc).change = "1" Then

                    If IsNumeric(CType(ctrl, ctrlFaresExc).Adult()) _
                            And IsNumeric(CType(ctrl, ctrlFaresExc).AdultExtra()) _
                            And IsNumeric(CType(ctrl, ctrlFaresExc).Child()) _
                            And IsNumeric(CType(ctrl, ctrlFaresExc).ChildExtra()) _
                            And IsNumeric(CType(ctrl, ctrlFaresExc).Junior()) _
                            And IsNumeric(CType(ctrl, ctrlFaresExc).JuniorExtra()) Then

                        If CDbl(CType(ctrl, ctrlFaresExc).Adult()) > 0 Then
                            Err = 0
                            If CType(ctrl, ctrlFaresExc).m_iFareId = 0 Then
                                SpliRate(Err, CType(ctrl, ctrlFaresExc).m_startDate, CType(ctrl, ctrlFaresExc).m_endDate, CType(ctrl, ctrlFaresExc).m_RoomId, CType(ctrl, ctrlFaresExc).m_RatePlan)
                            End If
                            Dim Fares As FaresData = (New FaresSystem).GetFaresListByDateAndRateplan(CType(ctrl, ctrlFaresExc).m_startDate, CType(ctrl, ctrlFaresExc).m_endDate, CType(ctrl, ctrlFaresExc).m_iHotelId, CodigoTarifa)
                            If Not Fares Is Nothing AndAlso Fares.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then
                                Dim ds As RatePlanData
                                With New RatePlanFacade
                                    ds = .GetDataRatePlan(CType(ctrl, ctrlFaresExc).m_RatePlan, CType(ctrl, ctrlFaresExc).m_iHotelId)
                                End With

                                CType(ctrl, ctrlFaresExc).m_iFareId = Fares.Tables(0).Rows(0).Item("IdTarifa")
                                CType(ctrl, ctrlFaresExc).RatePlanRow = New RowRatePlan
                                With ds.Tables(ds.RATEPLAN_TABLE).Rows(0)
                                    CType(ctrl, ctrlFaresExc).RatePlanRow.IDRATEPLAN = .Item(ds.FIELD_IDRATEPLAN)
                                    CType(ctrl, ctrlFaresExc).RatePlanRow.SEGMENT = .Item(ds.FIELD_SEGMENT)
                                    CType(ctrl, ctrlFaresExc).RatePlanRow.RATECODE = .Item(ds.FIELD_CODIGOTARIFA)
                                End With
                                CType(ctrl, ctrlFaresExc).UpdateFare(CType(ctrl, ctrlFaresExc).m_RoomId, CType(ctrl, ctrlFaresExc).m_startDate, CType(ctrl, ctrlFaresExc).m_endDate, FareBefore, Fares)
                                CType(CtrlPlanFares1, ctrlPlanFares).Save(CType(ctrl, ctrlFaresExc).RatePlanRow.RATECODE, CType(ctrl, ctrlFaresExc).m_RoomId, "", Nothing, CType(ctrl, ctrlFaresExc).m_iFareId)
                            Else
                                CType(ctrl, ctrlFaresExc).res_Save()
                            End If
                            CType(ctrl, ctrlFaresExc).change = "0"
                        End If
                    End If
                End If
            Next
            btnLoad_Click(sender, e)
        End If
    End Sub
    Private Sub SpliRate(ByRef err As Integer, ByVal startDate As Date, ByVal endDate As Date, ByVal IdTipoHabitacion As Integer, ByVal idRatePlan As String)
        'Dim DS As DataSet
        Try

     
            Dim Connection As SqlConnection
            Dim dsCommand As New SqlDataAdapter
            Connection = New SqlConnection(ConnectionString)
            dsCommand = New SqlDataAdapter("spSplitRate", Connection)
            With dsCommand

                Try
                    With .SelectCommand
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = "spSplitRate"
                        .Parameters.Add("@fechaInicia", SqlDbType.SmallDateTime).Value = startDate
                        .Parameters.Add("@fechaFinaliza", SqlDbType.SmallDateTime).Value = endDate
                        .Parameters.Add("@idTipoHabitacion", SqlDbType.Int).Value = IdTipoHabitacion
                        .Parameters.Add("@idRatePlan", SqlDbType.VarChar, 8).Value = idRatePlan
                        .Parameters.Add("@error", SqlDbType.Int).Direction = ParameterDirection.Output
                        .Connection.Open()
                    End With
                    .SelectCommand.ExecuteNonQuery()
                    err = .SelectCommand.Parameters("@error").Value
                Catch ex As Exception
                Finally
                    .SelectCommand.Connection.Close()
                    If Not .SelectCommand Is Nothing Then
                        If Not .SelectCommand.Connection Is Nothing Then
                            .SelectCommand.Connection.Dispose()
                        End If
                        .SelectCommand.Dispose()
                    End If
                    .Dispose()
                End Try
            End With
        Catch ex As Exception
            Dim a = ex.ToString
        End Try
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender        
        Call LoadCulture()
    End Sub
End Class
