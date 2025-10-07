Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Xml.Linq
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports APIServices.Models
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Conflux.OTA.Models.Rates
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Parser.Restriction
Imports RateManager.Utitlities.Hotel
Imports System.Threading
Imports System.Threading.Tasks

Partial Class FaresCatalogue
    Inherits PaginaBase
    Enum dgcolumns
        codigohabitacion
        idtipohabitacion_hotel
        FechaInicia
        FechaFinaliza
        RatePlan
        MinRate
        MaxRate
        CurrencyCode
        chanel
        Plan
        Edit
        Delete
        idTarifa
        rategds
        rateportal
        rateUnip
        rateADS
        startDateNoFormat
        endDateNoFormat
    End Enum
    Const KEY_MINPRICE As String = "mintarifaAdulto"
    Const KEY_MAXPRICE As String = "maxtarifaAdulto"
    Private enabledGoogle As Boolean = False
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
            Return ViewState("_idRoom")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_idRoom") = Value
        End Set
    End Property
    Private Property room() As String
        Get
            Return ViewState("_Room")
        End Get
        Set(ByVal Value As String)
            ViewState("_Room") = Value
        End Set
    End Property

    Public ReadOnly Property IdDg() As String
        Get
            Return Me.CtrlPlanFares2.iddg
        End Get
    End Property
    Public ReadOnly Property IdDgAdult() As String
        Get
            Return Me.CtrlPlanFares2.iddgAdult
        End Get
    End Property
    Public ReadOnly Property IdDgChild() As String
        Get
            Return Me.CtrlPlanFares2.iddgChild
        End Get
    End Property
    Public ReadOnly Property IdDgTeen() As String
        Get
            Return Me.CtrlPlanFares2.iddgTeen
        End Get
    End Property

    Property Editando() As Boolean
        Get
            Return ViewState("Editando")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editando") = Value
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


    'Public Property SourceRateName() As String
    '    Get
    '        Return viewstate("_SRN")

    '    End Get
    '    Set(ByVal Value As String)
    '        viewstate("_SRN") = Value
    '        Me.CtrRateAplication1.SourceRateName = Value
    '    End Set
    'End Property


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CtrlPlanFaresExc2 As CtrlPlanFaresExc
    Protected WithEvents CtrlPlanFares2 As ctrlPlanFares
    Protected WithEvents CtrRateAplication1 As ctrRateAplication
    Protected WithEvents CtlMensajes1 As ctlMensajes
    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Protected ReadOnly Property RateModeView() As Integer
        Get
            Dim value As Integer = 0

            If (Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Adult", Me.CtrRateAplication1.GetFareFor("Adult", False), False) _
                    OrElse
                    Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Children", Me.CtrRateAplication1.GetFareFor("Child", False), False) _
                    OrElse
                    Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Teen", Me.CtrRateAplication1.GetFareFor("Teen", False), False) _
                OrElse
                      (Me.CtrlPlanFaresExc2.FieldException <> "NNNNNNN" _
                        AndAlso (Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Adult", Me.CtrRateAplication1.GetFareFor("Adult", False), False) _
                        OrElse
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Children", Me.CtrRateAplication1.GetFareFor("Child", False), False) _
                        OrElse
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Teen", Me.CtrRateAplication1.GetFareFor("Teen", False), False)
                        ))) Then
                value = 1
            End If

            Return value
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        CtrRateAplication1.m_iHotelId = Me.cInfoActual.Hotel
        If Not IsPostBack Then
            lblPriceError.Visible = False
            lblError.Visible = False
            hplShowRates.Style.Add("display", "none")
            hplShowRates.Visible = False
            hplHideRates.Style.Add("display", "none")
            hplHideRates.Visible = False
            pnlData.Style.Add("display", "none")

            loadDatos()
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
            btnLoad_Click(sender, e)
            'Me.ddlRooms.Attributes.Add("onChange", "javascript:showRoomType('" & Me.ddlRooms.ClientID & "','" & Me.SourceName & "')")
            'Me.ddlratesplans.Attributes.Add("onChange", "javascript:showRatePlan('" & Me.ddlratesplans.ClientID & "','" & Me.SourceRateName & "','" & lblRatePlan.ClientID & "')")
            Me.hplHideRates.NavigateUrl = "javascript:Ocultar('0');"
            hplShowRates.NavigateUrl = "javascript:Ocultar('1');"
            cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", pnlData.ClientID, cmdNew.ClientID, "true"))
            Editando = False
        End If
        btnSave.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.CtrRateAplication1.GetClientID))
        btnPublish.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.CtrRateAplication1.GetClientID))
        btnNew.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.CtrRateAplication1.GetClientID))
        Me.ResizefrmPrincipal()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
        CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, Me.btnNew, "A")
        'CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, Me.btnDelete, "D")
        Dim hpl As HyperLink
        For Each i As DataGridItem In Me.dgRooms.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                Dim ibtnEdit As LinkButton = i.FindControl("lnkEdit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, ibtnEdit, "M")
                hpl = i.FindControl("lnkDelete")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.Tarifas, hpl, "D")
            End If
        Next
        hpEliminate.NavigateUrl = Me.CtlMensajes1.getShow(lnkEliminate.ClientID, PortalCulture.GetString("00133"), PortalCulture.GetString("01661"))
    End Sub

    Public Sub CommandDelete(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        If e.CommandName = "Delete" Then

            Dim maxConcurrentTasks As Integer = 5
            Dim semaphore As New SemaphoreSlim(maxConcurrentTasks)
            Dim tasksToExecute As New List(Of Func(Of Task))()
            Dim userName As String = Me.ReadUserCookie().GetValue(0)
            Dim userId As Integer = Me.UserIdentityName

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            'Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
            'Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

            'Dim rateAmountMessages As RateAmountMessages = New RateAmountMessages()
            'rateAmountMessages.HotelCode = info.Empresa
            'rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            For Each room As DataGridItem In dgRooms.Items
                Dim checkBoxTemp As CheckBox = room.FindControl("deleteCheckbox")
                Dim labelTemp As Label = room.FindControl("glblRatePlanName")
                If checkBoxTemp.Checked Then
                    Dim iFareIdTemp As Integer = Integer.Parse(room.Cells(dgcolumns.idTarifa).Text)
                    Dim lblStartDateNoFormat As Label = room.FindControl("lblStartDateNoFormat")
                    Dim lblEndDateNoFormat As Label = room.FindControl("lblEndDateNoFormat")
                    Dim startDate As DateTime = Convert.ToDateTime(lblStartDateNoFormat.Text)
                    Dim endDate As DateTime = Convert.ToDateTime(lblEndDateNoFormat.Text)

                    Dim vDayRates As List(Of vDayRates) = Helpers.Rates.RatesHelpers.GetVDayRate(iFareIdTemp, startDate, endDate)


                    With New FaresSystem
                        If .DeleteFares(iFareIdTemp) Then

                            Me.guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Eliminar, "Se eliminó la tarifa de la habitación " & room.Cells(dgcolumns.codigohabitacion).Text & " de la fecha " & room.Cells(dgcolumns.FechaInicia).Text & " a la fecha " & room.Cells(dgcolumns.FechaFinaliza).Text & " con el rateplan " & labelTemp.Text)

                            'If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then

                            '    Parser.Parser.ToRateAmountMessagesDelete(vDayRates, Nothing, TypeRateEnum.RoomRate, rateAmountMessages.RateAmountMessagesList)

                            'End If

                            tasksToExecute.Add(Function() SendDeleteAsync(userName, userId, info.Hotel, info.Empresa, vDayRates))

                        End If
                    End With
                End If
            Next

            'If isEnabledGoogleRequest Then
            '    SendDeleteToService(rateAmountMessages, info.Hotel, HotelUtilitie.ENDPOINTDELETE, "Conflux")
            'End If

            'If isEnabledSendingRatesAPICache Then
            '    SendDeleteToService(rateAmountMessages, info.Hotel, HotelUtilitie.ENDPOINTAPIDELETE, "APICache")
            'End If


            For Each taskDelegate As Func(Of Task) In tasksToExecute
                Task.Run(Async Function()
                             Await semaphore.WaitAsync()
                             Try
                                 Await taskDelegate()  ' Aquí se ejecuta MandarTarifasAsync
                             Finally
                                 semaphore.Release()
                             End Try
                         End Function)
            Next



            If dgRooms.CurrentPageIndex > 0 And dgRooms.Items.Count = 1 Then
                dgRooms.CurrentPageIndex = ((dgRooms.CurrentPageIndex * dgRooms.PageSize) \ dgRooms.PageSize) - 1
            End If
            dgRooms.SelectedIndex = -1
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRooms.DataSource = GetRoomFares()
            Me.dgRooms.DataBind()
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            newFare()
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        Page.ClientScript.RegisterForEventValidation(Me.btnSave.UniqueID)
        MyBase.Render(writer)
    End Sub

    Private Sub loadCulture()
        If Editando Then
            lblMsg.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("00133"))
        Else
            lblMsg.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("00133"))
        End If
        lblTitle.Text = PortalCulture.GetString("00133")
        Me.lblEName.Text = PortalCulture.GetString("00170", True)
        lblERatesPlans.Text = PortalCulture.GetString("00016", True)
        If Me.idroom = 0 Then
            Me.lblFaresTitle.Text = PortalCulture.GetString("00171") & PortalCulture.GetString("00423")
        Else
            Me.lblFaresTitle.Text = PortalCulture.GetString("00171") & " " & PortalCulture.GetString("00170") & " " & room
        End If
        Me.lblPreciosTarifa.Text = PortalCulture.GetString("00275")
        Me.btnNew.Text = PortalCulture.GetString("00009")
        Me.btnSave.Text = PortalCulture.GetString("00008")
        Me.cmdNew.Value = PortalCulture.GetString("00102")
        Me.dgRooms.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        Me.dgRooms.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        Me.dgRooms.Columns(dgcolumns.FechaInicia).HeaderText = PortalCulture.GetString("00276")
        Me.dgRooms.Columns(dgcolumns.FechaFinaliza).HeaderText = PortalCulture.GetString("00277")
        Me.dgRooms.Columns(dgcolumns.RatePlan).HeaderText = PortalCulture.GetString("00016")
        Me.dgRooms.Columns(dgcolumns.codigohabitacion).HeaderText = PortalCulture.GetString("00170")
        Me.dgRooms.Columns(dgcolumns.MaxRate).HeaderText = PortalCulture.GetString("M000597")
        Me.dgRooms.Columns(dgcolumns.MinRate).HeaderText = PortalCulture.GetString("M000598")
        Me.dgRooms.Columns(dgcolumns.chanel).HeaderText = PortalCulture.GetString("00575")
        Me.btnLoad.Text = PortalCulture.GetString("00149")
        Me.hplShowRates.Text = PortalCulture.GetString("00280")
        Me.hplHideRates.Text = PortalCulture.GetString("00281")
        lblError.Text = PortalCulture.GetString("00634")
        ddlRooms.Items(0).Text = PortalCulture.GetString("M000272")
        Me.lblNoroomSelected.Text = PortalCulture.GetString("00436")
        Me.lblPriceError.Text = PortalCulture.GetString("00682")
        Me.chkOldDates.Text = PortalCulture.GetString("01050")
        lblPricingNE.Text = PortalCulture.GetString("M000391")
        lblPricingExc.Text = PortalCulture.GetString("M000402")
        Me.hpEliminate.Text = PortalCulture.GetString("01662")
    End Sub

    Private Function RatePlanFilter(ByVal filter As String, ByVal RatesPlan As Portal.General.Common.Data.RatePlanData) As Portal.General.Common.Data.RatePlanData
        'Elimina los planes tarifarios que contengan el tipo de segmento especificado
        Dim dv2 As DataView
        For Each r As DataRow In RatesPlan.Tables("RatePlans").Rows()
            dv2 = RatesPlan.Tables("RatePlans").DefaultView
            dv2.RowFilter = "Segment" & "=" & "'" & filter & "'"
            If dv2.Count > 0 AndAlso r("Segment").ToString() = filter Then
                r.Delete()
            End If
        Next
        RatesPlan.AcceptChanges()
        Return RatesPlan
    End Function


    Public Sub loadDatos()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        Dim RatesPlan As Portal.General.Common.Data.RatePlanData
        With New Portal.Hotel.Facade.RoomFacade
            Rooms = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        RatesPlan = CtrRateAplication1.loadAllRatesplans(1)

        Me.ddlratesplans.DataSource = RatesPlan
        Me.ddlratesplans.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
        Me.ddlratesplans.DataTextField = "texto" 'Portal.General.Common.Data.RatePlanData.FIELD_CODIGOTARIFA
        Me.ddlratesplans.DataBind()
        'lblRatePlan.Text = "-"
        'SourceRateName = ""
        'For j As Integer = 0 To RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows.Count - 1
        '    If RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows(j).Item(RatesPlan.FIELD_NAME).ToString = "" Then
        '        Me.SourceRateName &= "--"
        '    Else
        '        Me.SourceRateName &= RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows(j).Item(RatesPlan.FIELD_NAME).ToString & "//"
        '    End If

        '    lblRatePlan.Text = "-" '& RatesPlan.Tables(RatesPlan.RATEPLAN_TABLE).Rows(0)(RatesPlan.FIELD_NAME)
        'Next

        Me.ddlratesplans.Items.Insert(0, "Todos")
        ddlratesplans.Items(0).Value = "0"
        ddlratesplans.Items(0).Text = PortalCulture.GetString("00172")
        ddlratesplans.SelectedIndex = 0



        Dim links As New LinkRoomTypeData
        With New LinkRoomsFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With

        Dim dv As DataView
        For Each r As DataRow In Rooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
            dv = links.Tables(LinkRoomTypeData.TABLE_LINKROOM).DefaultView
            dv.RowFilter = LinkRoomTypeData.FIELD_TargetRoom & "=" & r(RoomsHotelData.FLD_ID_ROOM_HOTEL)
            If dv.Count > 0 Then
                r.Delete()
            End If
        Next
        Rooms.AcceptChanges()



        ddlRooms.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
        ddlRooms.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        ddlRooms.DataSource = Rooms
        ddlRooms.DataBind()
        ddlRooms.Items.Insert(0, PortalCulture.GetString("M000272"))
        ddlRooms.Items(0).Value = 0

        dsRooms = Rooms

        'For i As Integer = 0 To Rooms.Tables(Rooms.TBL_ROOM_HOTEL).Rows.Count - 1
        '    Me.SourceName &= "//" & Rooms.Tables(Rooms.TBL_ROOM_HOTEL).Rows(i).Item(Rooms.FLD_NOMBRE).ToString
        'Next

        Try
            If ddlRooms.Items.Count > 1 Then
                ddlRooms.SelectedIndex = 1
            Else
                ddlRooms.SelectedIndex = 0
            End If
            ' Me.lblRoomType.Text = Rooms.Tables(Rooms.TBL_ROOM_HOTEL).Rows(0).Item(Rooms.FLD_NOMBRE).ToString
            idroom = ddlRooms.SelectedValue
            Dim ci As System.Globalization.CultureInfo
            ci = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
            Me.dgRooms.DataSource = GetRoomFares()
            Me.dgRooms.DataBind()
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
            newFare()
        Catch ex As Exception
            '     Me.lblRoomType.Text = "-"
        End Try
    End Sub

    Private Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            Dim lk As LinkButton
            Dim lbl As Label


            lk = e.Item.FindControl("lnkEdit")
            lk.Text = PortalCulture.GetString("00065")

            lbl = e.Item.FindControl("lblStartDateNoFormat")
            lbl.Text = CDate(e.Item.Cells(dgcolumns.FechaInicia).Text).ToString("MM/dd/yyyy")

            lbl = e.Item.FindControl("lblEndDateNoFormat")
            lbl.Text = CDate(e.Item.Cells(dgcolumns.FechaFinaliza).Text).ToString("MM/dd/yyyy")


            e.Item.Cells(dgcolumns.FechaInicia).Text = CDate(e.Item.Cells(dgcolumns.FechaInicia).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(dgcolumns.FechaFinaliza).Text = CDate(e.Item.Cells(dgcolumns.FechaFinaliza).Text).ToString("MMM/dd/yyyy")
            lk = e.Item.FindControl("lnkDelete2")
            Dim hpl As HyperLink = e.Item.FindControl("lnkDelete")
            hpl.Text = PortalCulture.GetString("00103")
            hpl.NavigateUrl = Me.CtlMensajes1.getShow(lk.ClientID, PortalCulture.GetString("00133"), PortalCulture.GetString("00467"))
            If e.Item.Cells(dgcolumns.MinRate).Text <> "&nbsp;" Then e.Item.Cells(dgcolumns.MinRate).Text = FCurrency(e.Item.Cells(dgcolumns.MinRate).Text, 2)
            If e.Item.Cells(dgcolumns.MaxRate).Text <> "&nbsp;" Then e.Item.Cells(dgcolumns.MaxRate).Text = FCurrency(e.Item.Cells(dgcolumns.MaxRate).Text, 2)
            lbl = e.Item.Cells(dgcolumns.chanel).FindControl("lblchanel")
            If e.Item.Cells(dgcolumns.rateUnip).Text = "True" Then
                lbl.Text = "One Page"
            End If
            If e.Item.Cells(dgcolumns.rateportal).Text = "True" Then
                lbl.Text &= IIf(lbl.Text <> "", " - " & "Portal", "Portal")
            End If
            If e.Item.Cells(dgcolumns.rategds).Text = "True" Then
                lbl.Text &= IIf(lbl.Text <> "", " - " & "GDS", "GDS")
            End If
            If e.Item.Cells(dgcolumns.rateADS).Text = "True" Then
                lbl.Text &= IIf(lbl.Text <> "", " - " & "ADS", "ADS")
            End If

            lbl = e.Item.FindControl("glblRatePlanName")
            lbl.Text = ""
            If Not DataBinder.Eval(e.Item.DataItem, "coderateplan") Is DBNull.Value Then
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "coderateplan")
            End If
            If Not DataBinder.Eval(e.Item.DataItem, "NameRatePlan") Is DBNull.Value Then
                If lbl.Text.Trim <> "" Then lbl.Text += " -- "
                lbl.Text += DataBinder.Eval(e.Item.DataItem, "NameRatePlan")
            End If

        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.FechaInicia).Text = PortalCulture.GetString("00276")
            e.Item.Cells(dgcolumns.FechaFinaliza).Text = PortalCulture.GetString("00277")
            e.Item.Cells(dgcolumns.codigohabitacion).Text = PortalCulture.GetString("00170")
            e.Item.Cells(dgcolumns.RatePlan).Text = PortalCulture.GetString("00016")
            e.Item.Cells(dgcolumns.MaxRate).Text = PortalCulture.GetString("M000597")
            e.Item.Cells(dgcolumns.MinRate).Text = PortalCulture.GetString("M000598")
            e.Item.Cells(dgcolumns.chanel).Text = PortalCulture.GetString("00575")

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.Delete).Text = CType(dgRooms.DataSource, DataView).Count & " " & PortalCulture.GetString("00268")
        End If
    End Sub

    Private Function GetRoomFares() As DataView
        Dim datFares As FaresData
        Dim fRow As DataRow
        Dim idAsoc As Integer = Me.GetIdAsociation

        If Me.idroom <> 0 Then
            With New FaresSystem
                datFares = .GetFaresByRoomTypeId(CInt(Me.idroom), PortalCulture.GetIDCulture, Me.chkOldDates.Checked, idAsociacion:=idAsoc)
            End With

            datFares.Tables(FaresData.FARES_TABLE).Columns.Add("MaxPrice", GetType(System.Double))
            datFares.Tables(FaresData.FARES_TABLE).Columns.Add("MinPrice", GetType(System.Double))

            'Quitamos las tarifas de habitación por día
            'With datFares.Tables(FaresData.FARES_TABLE)
            '    For i As Integer = 0 To .Rows.Count - 1
            '        If Not DateDiff(DateInterval.Day, .Rows(i).Item(FaresData.STARTDATE_FIELD), .Rows(i).Item(FaresData.ENDDATE_FIELD)) > 1 Then
            '            .Rows(i).Delete()
            '        End If
            '    Next
            '    .AcceptChanges()
            'End With


            Dim dvFares As DataView
            dvFares = datFares.Tables(FaresData.FARES_TABLE).DefaultView

            'dvFares.RowFilter = " tipotarifa <>'K' "
            If Me.ddlratesplans.SelectedIndex <> 0 Then
                dvFares.RowFilter &= FaresData.IDRATEPLAN_FIELD & "='" & ddlratesplans.SelectedItem.Value & "'"
            Else
                If Not CType(Me.Page, PaginaBase).IsUsuarioHomeAgency Then
                    dvFares.RowFilter &= " packagetype =0"
                Else
                    dvFares.RowFilter &= FaresData.IDRATEPLAN_FIELD & "<>'RAC'"
                End If
            End If

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

            Return dvFares
        Else
            Return Nothing
        End If

    End Function


    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Try
            idroom = ddlRooms.SelectedValue
            room = ddlRooms.SelectedItem.Text
        Catch ex As Exception
            idroom = 0
        End Try
        dgRooms.CurrentPageIndex = 0
        dgRooms.SelectedIndex = -1
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgRooms.DataSource = GetRoomFares()
        Me.dgRooms.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
        newFare()
        Me.CtrRateAplication1.Segment(ddlratesplans.SelectedValue)
        Me.CtrRateAplication1.LoadRooms(idroom)
        lblPriceError.Visible = False
        'lblRoomType.Text = ""
        'If SourceName <> "" Then
        '    Me.lblRoomType.Text = Me.SourceName.Split("//")(2 * ddlRooms.SelectedIndex)
        'End If
        'lblRatePlan.Text = "-"
        'If ddlratesplans.SelectedIndex > 0 Then
        '    lblRatePlan.Text = SourceRateName.Split("//")((ddlratesplans.SelectedIndex - 1) * 2)
        'End If

        'If lblRoomType.Text = "" Then
        '    lblRoomType.Text = "-"
        'End If

    End Sub

    Public Function GetFareRestrictions() As FaresRestrictionsData
        CtrlPlanFaresExc2.FieldException = "NNNNNNN"
        If CtrRateAplication1.m_iFareId <> 0 Then
            CtrlPlanFaresExc2.FieldException = CtrRateAplication1.Exceptions
        End If
        Return Me.CtrlPlanFares2.GetFareRestrictions()
    End Function

    Function SetMinRate()
        Dim FareAdultMin As Double
        Dim FareChildMin As Double
        Dim FareJuniorMin As Double

        CtrlPlanFares2.TarifaMinima(FareAdultMin, FareChildMin, FareJuniorMin)
        Dim bPorOcupacion As Integer = RateModeView
        CtrRateAplication1.SetMinRate(FareAdultMin, FareChildMin, FareJuniorMin, bPorOcupacion)
    End Function

    Private Sub dgRooms_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRooms.ItemCommand
        Dim iFareId As Integer = 0
        Dim startDateFareId As Date
        Dim endDateFareId As Date
        If e.CommandName = "Edit" Then
            Try
                iFareId = Integer.Parse(e.Item.Cells(dgcolumns.idTarifa).Text)
                Dim lblStartDateNoFormat As Label = e.Item.Cells(dgcolumns.startDateNoFormat).FindControl("lblStartDateNoFormat")
                Dim lblEndDateNoFormat As Label = e.Item.Cells(dgcolumns.startDateNoFormat).FindControl("lblEndDateNoFormat")
                startDateFareId = CDate(lblStartDateNoFormat.Text)
                endDateFareId = CDate(lblEndDateNoFormat.Text)
            Catch ex As Exception
                Return
            End Try
            lblPriceError.Visible = False
            newFare()
            Me.idroom = Integer.Parse(e.Item.Cells(dgcolumns.idtipohabitacion_hotel).Text)
            Me.dgRooms.SelectedIndex = e.Item.ItemIndex
            Me.CtrRateAplication1.m_iFareId = iFareId
            Me.CtrRateAplication1.m_iHotelId = Me.cInfoActual.Hotel
            Me.CtrRateAplication1.m_StartDateFareId = startDateFareId
            Me.CtrRateAplication1.m_EndDateFareId = endDateFareId
            Me.CtrRateAplication1.LoadFare(iFareId, 0)
            Me.CtrlPlanFares2.m_iRoomId = Integer.Parse(e.Item.Cells(dgcolumns.idtipohabitacion_hotel).Text)
            Me.CtrlPlanFares2.m_iFareId = iFareId
            Me.CtrlPlanFares2.ReFill()
            Me.CtrlPlanFaresExc2.ReFill()


            pnlData.Style.Add("display", "block")
            cmdNew.Style.Add("display", "none")
            Editando = True
            lblTitle.Text = PortalCulture.GetString("01019")
        ElseIf e.CommandName = "Delete" Then

            iFareId = Integer.Parse(dgRooms.Items(e.Item.ItemIndex).Cells(dgcolumns.idTarifa).Text)
            With New FaresSystem
                If .DeleteFares(iFareId) Then
                    Me.guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Eliminar, "Se eliminó la tarifa de la habitación " & dgRooms.Items(e.Item.ItemIndex).Cells(dgcolumns.codigohabitacion).Text & " de la fecha " & dgRooms.Items(e.Item.ItemIndex).Cells(dgcolumns.FechaInicia).Text & " a la fecha " & dgRooms.Items(e.Item.ItemIndex).Cells(dgcolumns.FechaFinaliza).Text & " con el rateplan " & dgRooms.Items(e.Item.ItemIndex).Cells(dgcolumns.RatePlan).Text)
                    If dgRooms.CurrentPageIndex > 0 And dgRooms.Items.Count = 1 Then
                        dgRooms.CurrentPageIndex = ((dgRooms.CurrentPageIndex * dgRooms.PageSize) \ dgRooms.PageSize) - 1
                    End If
                    dgRooms.SelectedIndex = -1
                    Dim ci As System.Globalization.CultureInfo
                    ci = System.Threading.Thread.CurrentThread.CurrentCulture
                    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
                    Me.dgRooms.DataSource = GetRoomFares()
                    Me.dgRooms.DataBind()
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci
                    newFare()
                End If
            End With
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        newFare()

    End Sub
    Private Sub actualizaidroom()
        Try
            idroom = ddlRooms.SelectedValue
        Catch ex As Exception
            idroom = 0
            'Me.lblRoomType.Text = "-"
        End Try
        Me.CtrlPlanFares2.m_iRoomId = CInt(Me.idroom)
    End Sub
    Private Sub newFare()
        actualizaidroom()
        Me.lblNoroomSelected.Visible = False
        Me.dgRooms.SelectedIndex = -1
        Me.CtrRateAplication1.newFare()
        Me.CtrlPlanFares2.m_iFareId = 0
        Me.CtrlPlanFares2.ReFill()
        Me.CtrlPlanFaresExc2.ReFill()
        lblError.Visible = False
        cmdNew.Style.Add("display", "block")
        pnlData.Style.Add("display", "none")
        Editando = False
        lblPriceError.Visible = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublish.Click
        lblPriceError.Visible = False

        Dim publish As Boolean = True '(CType(sender, Button).ID = Me.btnPublish.ID)
        Dim FareAdultMin As Double
        Dim FareChildMin As Double
        Dim FareJuniorMin As Double
        Dim sCorreo As String = ""

        Dim maxConcurrentTasks As Integer = 5
        Dim semaphore As New SemaphoreSlim(maxConcurrentTasks)
        Dim tasksToExecute As New List(Of Func(Of Task))()
        Dim userName As String = Me.ReadUserCookie().GetValue(0)
        Dim userId As Integer = Me.UserIdentityName


        If CtrlPlanFaresExc2.IsValidData AndAlso CtrlPlanFares2.IsValidData Then

            If Me.idroom <> 0 Then
                Me.lblNoroomSelected.Visible = False
                If Page.IsValid Then
                    lblError.Visible = False
                    If CtrRateAplication1.lstDatesCount = 0 Then
                        CtrRateAplication1.lstDatesAdd()
                        'guardar con lo que estan en desde hasta

                        If CDate(CtrRateAplication1.lstDatesItemI(1).Split("-")(1)) < CDate(CtrRateAplication1.lstDatesItemI(1).Split("-")(0)) Then
                            lblError.Visible = True
                            CtrRateAplication1.lstDatesClear()
                            Exit Sub
                        End If

                    End If
                    Dim _exito As Boolean = True
                    '  actualizaidroom()
                    Me.CtrlPlanFaresExc2.createFieldException()
                    CtrRateAplication1.Exceptions = Me.CtrlPlanFaresExc2.FieldException()


                    Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                    'Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                    'Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                    For i As Integer = 1 To CtrRateAplication1.lstDatesCount
                        Dim f1, f2 As Date
                        f1 = CDate(CtrRateAplication1.lstDatesItemI(i).Split("-")(0))
                        f2 = CDate(CtrRateAplication1.lstDatesItemI(i).Split("-")(1))

                        'cuando es modificación la primera se modifica pero las demas son add
                        Dim chLast As String, rpLast As String = "", f1Last As String = "", f2Last As String = ""
                        If dgRooms.SelectedIndex <> -1 Then
                            chLast = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.codigohabitacion).Text()
                            rpLast = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.RatePlan).Text()
                            f1Last = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.FechaInicia).Text()
                            f2Last = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.FechaFinaliza).Text()
                            Dim lbl As Label
                            lbl = dgRooms.Items(dgRooms.SelectedIndex).FindControl("glblRatePlanName")
                            rpLast = lbl.Text
                        Else
                            chLast = ddlRooms.SelectedItem.Text
                        End If

                        Dim bPorOcupacion As Integer = RateModeView
                        CtrlPlanFares2.TarifaMinima(FareAdultMin, FareChildMin, FareJuniorMin)

                        FaresAdjust(f1, f2)

                        Dim auxFareId As Integer = 0
                        'Dim vDayRates As List(Of vDayRates) = Nothing
                        Dim areSameDates = False
                        'If Editando Then
                        '    vDayRates = Helpers.Rates.RatesHelpers.GetVDayRate(CtrRateAplication1.m_iFareId, CtrRateAplication1.m_StartDateFareId, CtrRateAplication1.m_EndDateFareId)
                        'End If

                        If CtrRateAplication1.AddFare(idroom, CtrRateAplication1.m_iFareId, f1, f2, chLast, rpLast, f1Last, f2Last, ddlRooms.SelectedItem.Text, publish, (bPorOcupacion = 1), FareAdultMin, FareChildMin, FareJuniorMin, sCorreo) = True Then
                            CtrlPlanFares2.m_iFareId = CtrRateAplication1.m_iFareId
                            auxFareId = CtrRateAplication1.m_iFareId
                            CtrlPlanFares2.Save(rpLast, ddlRooms.SelectedItem.Text, sCorreo, Me.CtrlPlanFaresExc2.getRatesExceptions())
                            Me.dgRooms.SelectedIndex = -1
                            '-------- Tarifas especiales -------------------------
                            Me.CtrlPlanFares2.m_iRoomId = idroom
                            Me.CtrlPlanFares2.m_iFareId = 0
                            CtrRateAplication1.m_iFareId = 0

                            'Dim ratesForRequest As RatesMessages = Nothing

                            'If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                            '    ratesForRequest = HotelUtilitie.ConfluxServiceHelper.GetRateMessages(auxFareId, f1, f2, info.Hotel, info.Empresa, TypeRateEnum.RoomRate)
                            'End If

                            'If isEnabledGoogleRequest And ratesForRequest IsNot Nothing Then

                            '    Try
                            '        SendRatesToService(ratesForRequest, info.Hotel, HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE, "Conflux", True)
                            '        SendClosureToService(auxFareId, f1, f2, HotelUtilitie.ENDPOINTCLOSURE, "Conflux", info)

                            '    Catch ex As Exception

                            '        Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                            '        Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                            '        errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"), New System.Xml.Linq.XAttribute("Code", "448"), New System.Xml.Linq.XText(ex.Message))
                            '        errorsElement.Add(errorElementProperty)
                            '        Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Sincronizar, "Error al sincronizar con Conflux", "", "", errorsElement.ToString(), info.Hotel)
                            '    End Try
                            'End If

                            'If isEnabledSendingRatesAPICache And ratesForRequest IsNot Nothing Then

                            '    Try
                            '        SendRatesToService(ratesForRequest, info.Hotel, HotelUtilitie.ENDPOINTAPI, HotelUtilitie.ENDPOINTAPIDELETE, "APICache", False)
                            '        SendClosureToService(auxFareId, f1, f2, HotelUtilitie.ENDPOINTAPICLOSURE, "APICache", info)

                            '    Catch ex As Exception
                            '        Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                            '        Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                            '        errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"), New System.Xml.Linq.XAttribute("Code", "448"), New System.Xml.Linq.XText(ex.Message))
                            '        errorsElement.Add(errorElementProperty)
                            '        Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Sincronizar, "Error al sincronizar con APICache", "", "", errorsElement.ToString(), info.Hotel)
                            '    End Try

                            'End If


                            'Async
                            'Agregar a listado de tareas async
                            tasksToExecute.Add(Function() SendRatesAsync(info.Hotel, info.Empresa, auxFareId, f1, f2, userName, userId))

                        Else
                            _exito = False
                            cmdNew.Style.Add("display", "none")
                            pnlData.Style.Add("display", "block")
                        End If
                    Next

                    Dim ci As System.Globalization.CultureInfo
                    ci = System.Threading.Thread.CurrentThread.CurrentCulture
                    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
                    Me.dgRooms.DataSource = GetRoomFares()
                    Me.dgRooms.DataBind()
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci
                    If _exito = True Then
                        newFare()
                        CtrRateAplication1.newFare()
                        Me.CtrlPlanFares2.ReFill()
                        Me.CtrlPlanFaresExc2.ReFill()
                    End If

                    'Ejecutar Tareas Async

                    For Each taskDelegate As Func(Of Task) In tasksToExecute
                        Task.Run(Async Function()
                                     Await semaphore.WaitAsync()
                                     Try
                                         Await taskDelegate()  ' Aquí se ejecuta MandarTarifasAsync
                                     Finally
                                         semaphore.Release()
                                     End Try
                                 End Function)
                    Next


                End If
            Else
                Me.lblNoroomSelected.Visible = True
            End If
        Else
            Dim Script As String
            hplShowRates.Style.Add("display", "block")
            hplHideRates.Style.Add("display", "none")
            'Me.Page.RegisterStartupScript("ShowGrid", "<script>Ocultar('1');</script>")
            Script = "<script>Ocultar('1');</script>"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Script)

            If Not CtrlPlanFaresExc2.IsValidData Then
                'Me.Page.RegisterStartupScript("ShowPrice", "<script>optionSw('1E');</script>")
                Script = "<script>optionSw('1E');</script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Script)
            End If
            If Not CtrlPlanFares2.IsValidData Then
                'Me.Page.RegisterStartupScript("ShowPrice", "<script>optionSw('1P');</script>")
                Script = "<script>optionSw('1P');</script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Script)
            End If

            lblPriceError.Visible = True
            cmdNew.Style.Add("display", "none")
            pnlData.Style.Add("display", "block")
        End If

    End Sub

    Private Sub dgRooms_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRooms.PageIndexChanged
        dgRooms.CurrentPageIndex = e.NewPageIndex
        dgRooms.SelectedIndex = -1
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.dgRooms.DataSource = GetRoomFares()
        Me.dgRooms.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim iFareId As Integer
        iFareId = CtrRateAplication1.m_iFareId
        With New FaresSystem
            If .DeleteFares(iFareId) Then
                Me.guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Eliminar, "Se eliminó la tarifa de la habitación " & dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.codigohabitacion).Text & " de la fecha " & dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.FechaInicia).Text & " a la fecha " & dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.FechaFinaliza).Text & " con el rateplan " & dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.RatePlan).Text)
                If dgRooms.CurrentPageIndex > 0 And dgRooms.Items.Count = 1 Then
                    dgRooms.CurrentPageIndex = ((dgRooms.CurrentPageIndex * dgRooms.PageSize) \ dgRooms.PageSize) - 1
                End If
                dgRooms.SelectedIndex = -1
                Dim ci As System.Globalization.CultureInfo
                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
                Me.dgRooms.DataSource = GetRoomFares()
                Me.dgRooms.DataBind()
                System.Threading.Thread.CurrentThread.CurrentCulture = ci
                btnNew_Click(sender, e)
            End If
        End With
    End Sub

    Private Sub dgRooms_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgRooms.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgRooms.CurrentPageIndex < dgRooms.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub dgRooms_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRooms.Init
    End Sub
    Public Sub FaresAdjust(ByVal F1 As Date, ByVal F2 As Date)
        Dim fares As DataView

        fares = GetRoomFares()
        'CtrRateAplication1.AddFare(fares.
    End Sub

    Private Sub SendClosureToService(ByVal rateId As Integer, ByVal startDate As Date, ByVal endDate As Date, ByVal endpoint As String, ByVal service As String, ByVal info As companyInfo)

        Dim requests As List(Of XDocument) = New List(Of XDocument)

        Dim vDayRatesForClosure As List(Of vDayRates) = Helpers.Rates.RatesHelpers.GetVDayRate(rateId, startDate, endDate)

        RestrictionsParser.Init(info.Empresa)

        Dim availStatusMessages As OTA.Models.Restrictions.AvailStatusMessages = RestrictionsParser.ToAvailStatusMessages(vDayRatesForClosure, "N")

        Dim availStatusMessagesList As List(Of XElement) = APIServices.Xml.OTA.Request.Restrictions.HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessages) 'Meter los dias en el request para google

        For Each availStatusMessage As XElement In availStatusMessagesList
            'Request 
            Dim xmlRequest As XDocument = APIServices.Xml.Soap.Soap.CreateSoapRequestXml(availStatusMessage)
            requests.Add(xmlRequest)
        Next

        Dim restrictionResponseList As List(Of Models.Restrictions.Response.RestrictionResponse) = New List(Of Models.Restrictions.Response.RestrictionResponse)

        For Each request As XDocument In requests
            Dim response As Models.Restrictions.Response.RestrictionResponse = HotelUtilitie.ConfluxServiceHelper.UpdateRestriction(request, endpoint, RestrictionEnum.LockRate)
            restrictionResponseList.Add(response)
        Next

        Dim note As String = String.Format("Tarifa enviada a {0} LockRate", service)
        Dim noteError As String = String.Format("Error al sincronizar LockRate {0}", service)

        For Each response As Models.Restrictions.Response.RestrictionResponse In restrictionResponseList

            If response.IsSuccess Then
                Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Sincronizar, note, "", response.Restrictions(0).XmlRequest(0).ToString(), response.Restrictions(0).Xml(0).ToString(), info.Hotel)
            Else
                Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Sincronizar, noteError, "", response.Xml.ToString(), "", info.Hotel)
            End If

        Next

    End Sub

    Private Async Function SendClosureToServiceAsync(ByVal userName As String, ByVal userId As Integer, ByVal rateId As Integer, ByVal startDate As Date, ByVal endDate As Date,
                                                     ByVal endpoint As String, ByVal service As String, ByVal hotelId As Integer, ByVal companyId As Integer) As Task

        Dim requests As List(Of XDocument) = New List(Of XDocument)

        Dim vDayRatesForClosure As List(Of vDayRates) = Helpers.Rates.RatesHelpers.GetVDayRate(rateId, startDate, endDate)

        RestrictionsParser.Init(companyId)

        Dim availStatusMessages As OTA.Models.Restrictions.AvailStatusMessages = RestrictionsParser.ToAvailStatusMessages(vDayRatesForClosure, "N")

        Dim availStatusMessagesList As List(Of XElement) = APIServices.Xml.OTA.Request.Restrictions.HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessages)

        For Each availStatusMessage As XElement In availStatusMessagesList
            'Request 
            Dim xmlRequest As XDocument = APIServices.Xml.Soap.Soap.CreateSoapRequestXml(availStatusMessage)
            requests.Add(xmlRequest)
        Next

        Dim restrictionResponseList As List(Of Models.Restrictions.Response.RestrictionResponse) = New List(Of Models.Restrictions.Response.RestrictionResponse)

        Dim confluxService As New APIServices.Conflux.ConfluxService()

        For Each request As XDocument In requests
            Dim response As Models.Restrictions.Response.RestrictionResponse = Await confluxService.UpdateRestrictionAsync(request, endpoint, RestrictionEnum.LockRate)
            restrictionResponseList.Add(response)
        Next

        Dim note As String = String.Format("Tarifa enviada a {0} LockRate", service)
        Dim noteError As String = String.Format("Error al sincronizar LockRate {0}", service)

        For Each response As Models.Restrictions.Response.RestrictionResponse In restrictionResponseList

            If response.IsSuccess Then
                HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, RateManager.Utitlities.Hotel.Actions.Sincronizar, note, "", response.Restrictions(0).XmlRequest(0).ToString(), response.Restrictions(0).Xml(0).ToString())

            Else
                HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, Actions.Sincronizar, noteError, "", response.Xml.ToString(), "")
            End If

        Next

    End Function

    Private Sub SendRatesToService(ByVal ratesForRequest As RatesMessages, ByVal hotelId As Integer, ByVal endpoint As String, ByVal endpointDelete As String, ByVal service As String, Optional ByVal deleteRates As Boolean = True)

        Dim ratesMessages As RatesMessages = ratesForRequest

        Dim res As Tuple(Of RateResponse, RateResponse) = HotelUtilitie.ConfluxServiceHelper.UpdateRate(ratesMessages, endpoint, endpointDelete, deleteRates)

        Dim note As String = String.Format("Tarifa envida a {0}", service)
        Dim noteDelete As String = String.Format("Eliminar tarifas {0}", service)

        Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Sincronizar, note, "", res.Item1.RequestXML, res.Item1.Xml, hotelId)

        'Delete Log
        If res.Item2 IsNot Nothing Then
            Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Eliminar, noteDelete, "", res.Item2.RequestXML, res.Item1.Xml, hotelId)
        End If

    End Sub

    Private Async Function SendRatesToServiceAsync(ByVal userName As String, ByVal userId As Integer, ByVal ratesForRequest As RatesMessages, ByVal hotelId As Integer, ByVal endpoint As String, ByVal endpointDelete As String, ByVal service As String, Optional ByVal deleteRates As Boolean = True) As Task

        Await Task.Delay(TimeSpan.FromMinutes(1))

        Dim confluxService As New APIServices.Conflux.ConfluxService()

        Dim ratesMessages As RatesMessages = ratesForRequest

        Dim res As Tuple(Of RateResponse, RateResponse) = Await confluxService.UpdateRateAsync(ratesMessages, endpoint, endpointDelete, deleteRates)

        Dim note As String = String.Format("Tarifa envida a {0}", service)
        Dim noteDelete As String = String.Format("Eliminar tarifas {0}", service)

        HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, RateManager.Utitlities.Hotel.Actions.Sincronizar, note, "", res.Item1.RequestXML, res.Item1.Xml)

        If res.Item2 IsNot Nothing Then
            HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, RateManager.Utitlities.Hotel.Actions.Eliminar, noteDelete, "", res.Item2.RequestXML, res.Item2.Xml)
        End If

    End Function


    Private Sub SendDeleteToService(ByVal rateAmountMessages As RateAmountMessages, ByVal hotelId As Integer, ByVal endpoint As String, ByVal service As String)

        Dim note As String = String.Format("Eliminar tarifas {0}", service)
        Dim noteError As String = String.Format("Error al eliminar tarifas {0}", service)

        Try

            Dim res As RateResponse = HotelUtilitie.ConfluxServiceHelper.DeleteRates(endpoint, rateAmountMessages)
            Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Eliminar, note, "", res.RequestXML, res.Xml, hotelId)

        Catch ex As Exception

            Dim errorsElement As New System.Xml.Linq.XElement("Errors")
            Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
            errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"), New System.Xml.Linq.XAttribute("Code", "448"), New System.Xml.Linq.XText(ex.Message))
            errorsElement.Add(errorElementProperty)

            Me.guardalog("/Pages/FaresCatalogue.aspx", acciones.Eliminar, noteError, "", "", errorsElement.ToString(), hotelId)
        End Try
    End Sub


    Private Async Function SendRatesAsync(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal rateId As Integer, ByVal startDate As Date, ByVal endDate As Date, ByVal userName As String, ByVal userId As Integer) As Task
        Try

            Dim confluxService As New APIServices.Conflux.ConfluxService()

            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            Dim ratesForRequest As RatesMessages = If(isEnabledGoogleRequest Or isEnabledSendingRatesAPICache,
                                          confluxService.GetRateMessages(rateId, startDate, endDate, hotelId, companyId, TypeRateEnum.RoomRate),
                                          Nothing)

            ' Enviar tarifas a Conflux
            Await SendRatesIfEnabledAsync(userName, userId, isEnabledGoogleRequest, ratesForRequest, hotelId, companyId, rateId,
                   startDate, endDate,
                   HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE,
                   HotelUtilitie.ENDPOINTCLOSURE, "Conflux", True)

            ' Enviar tarifas a APICache
            Await SendRatesIfEnabledAsync(userName, userId, isEnabledSendingRatesAPICache, ratesForRequest, hotelId, companyId, rateId,
                   startDate, endDate,
                   HotelUtilitie.ENDPOINTAPI, HotelUtilitie.ENDPOINTAPIDELETE,
                   HotelUtilitie.ENDPOINTAPICLOSURE, "APICache", False)


        Catch ex As Exception

        End Try
    End Function

    Private Async Function SendRatesIfEnabledAsync(ByVal userName As String, ByVal userId As Integer, ByVal isEnabled As Boolean,
                                                ByVal ratesForRequest As RatesMessages, ByVal hotelId As Integer, ByVal companyId As Integer,
                                                ByVal rateId As Integer, ByVal startDate As Date, ByVal endDate As Date,
                                                ByVal endpoint As String, ByVal endpointDelete As String,
                                                ByVal closureEndpoint As String, ByVal serviceName As String, ByVal deleteRates As Boolean) As Task


        If isEnabled AndAlso ratesForRequest IsNot Nothing Then
            Try
                ' Enviar tarifas
                Await SendRatesToServiceAsync(userName, userId, ratesForRequest, hotelId, endpoint, endpointDelete, serviceName, deleteRates)


                Await SendClosureToServiceAsync(userName, userId, rateId, startDate, endDate, closureEndpoint, serviceName, hotelId, companyId)


            Catch ex As Exception
                Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"),
                                         New System.Xml.Linq.XAttribute("Code", "448"),
                                         New System.Xml.Linq.XText(ex.Message))
                errorsElement.Add(errorElementProperty)

                HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, Actions.Sincronizar, $"Error al sincronizar con {serviceName}", "", errorsElement.ToString(), "")

            End Try
        End If

    End Function


    Private Async Function SendDeleteAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer,
                                           ByVal vDayRates As List(Of vDayRates)) As Task
        Try

            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            Dim rateAmountMessages As RateAmountMessages = New RateAmountMessages()
            rateAmountMessages.HotelCode = companyId
            rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            rateAmountMessages.RateAmountMessagesList = Parser.Parser.ToRateAmountMessagesDelete(vDayRates, Nothing, TypeRateEnum.RoomRate)

            Await SendDeleteIfEnabledAsync(userName, userId, hotelId, rateAmountMessages, isEnabledGoogleRequest, "Conflux")


        Catch ex As Exception

        End Try

    End Function

    Private Async Function SendDeleteIfEnabledAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal rateAmountMessages As RateAmountMessages, ByVal isEnabled As Boolean, ByVal service As String) As Task

        If isEnabled Then
            Try
                Await SendDeleteToServiceAsync(userName, userId, rateAmountMessages, hotelId, HotelUtilitie.ENDPOINTDELETE, service)
            Catch ex As Exception

            End Try
        End If

    End Function

    Private Async Function SendDeleteToServiceAsync(ByVal userName As String, ByVal userId As Integer, ByVal rateAmountMessages As RateAmountMessages, ByVal hotelId As Integer, ByVal endpoint As String, ByVal service As String) As Task

        Dim note As String = String.Format("Eliminar tarifas {0}", service)
        Dim noteError As String = String.Format("Error al eliminar tarifas {0}", service)

        Try

            Dim confluxService As New APIServices.Conflux.ConfluxService()

            Dim res As RateResponse = Await confluxService.DeleteRatesAsync(endpoint, rateAmountMessages)

            HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, RateManager.Utitlities.Hotel.Actions.Eliminar, note, "", res.RequestXML, res.Xml)

        Catch ex As Exception

            Dim errorsElement As New System.Xml.Linq.XElement("Errors")
            Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
            errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"), New System.Xml.Linq.XAttribute("Code", "448"), New System.Xml.Linq.XText(ex.Message))
            errorsElement.Add(errorElementProperty)

            HotelUtilitie.Log(userName, userId, "/Pages/FaresCatalogue.aspx", hotelId, RateManager.Utitlities.Hotel.Actions.Eliminar, noteError, "", errorsElement.ToString(), "")

        End Try
    End Function



End Class


