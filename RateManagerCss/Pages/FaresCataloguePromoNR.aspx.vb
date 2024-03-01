Imports System.Runtime.Serialization
Imports System.Xml.Linq
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade
Imports Portal.General.Common
Imports APIServices.Models
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Conflux.OTA.Models.Rates
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Parser.Restriction
Imports RateManager.Utitlities.Hotel

Partial Public Class FaresCataloguePromoNR
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
    Private Property room() As String
        Get
            Return viewstate("_Room")
        End Get
        Set(ByVal Value As String)
            viewstate("_Room") = Value
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

    Public ReadOnly Property IdTextBoxPorcMax() As String
        Get
            Return Me.ctrRateAplicationNRpromo1.m_TextBoxPorcMax
        End Get
    End Property

    Public ReadOnly Property IdTextBoxPorcMin() As String
        Get
            Return Me.ctrRateAplicationNRpromo1.m_TextBoxPorcMin
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
    '        Me.ctrRateAplicationNRpromo1.SourceRateName = Value
    '    End Set
    'End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        ctrRateAplicationNRpromo1.m_iHotelId = Me.cInfoActual.Hotel
        ctrRateAplicationNRpromo1.setPorcMinMax()
        CtrlPlanFares2.m_TextBoxPorcMax = Me.IdTextBoxPorcMax
        CtrlPlanFares2.m_TextBoxPorcMin = Me.IdTextBoxPorcMin
        CtrlPlanFaresExc2.m_TextBoxPorcMax = Me.IdTextBoxPorcMax
        CtrlPlanFaresExc2.m_TextBoxPorcMin = Me.IdTextBoxPorcMin

        If Not IsPostBack Then

            Me.ctrRateAplicationNRpromo1.MinPercentControlId = Me.CtrlPlanFares2.MinStorageControlId
            Me.ctrRateAplicationNRpromo1.MinPercentControlId = Me.CtrlPlanFaresExc2.MinStorageControlId
            Me.ctrRateAplicationNRpromo1.MaxPercentControlId = Me.CtrlPlanFares2.MaxStorageControlId
            Me.ctrRateAplicationNRpromo1.MaxPercentControlId = Me.CtrlPlanFaresExc2.MaxStorageControlId

            lblPriceError.Visible = False
            lblError.Visible = False
            hplShowRates.Style.Add("display", "none")
            hplShowRates.Visible = False
            hplHideRates.Style.Add("display", "none")
            hplHideRates.Visible = False

            loadDatos()

            CtrlPlanFares2.PlusTaxProperty = ctrRateAplicationNRpromo1.PlusTaxProperty
            CtrlPlanFares2.EcotasaProperty = ctrRateAplicationNRpromo1.EcotasaProperty
            CtrlPlanFaresExc2.PlusTaxProperty = ctrRateAplicationNRpromo1.PlusTaxProperty
            CtrlPlanFaresExc2.EcotasaProperty = ctrRateAplicationNRpromo1.EcotasaProperty




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

        btnSave.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.ctrRateAplicationNRpromo1.GetClientID))
        btnPublish.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.ctrRateAplicationNRpromo1.GetClientID))
        btnNew.Attributes.Add("onclick", String.Format("javascript:FireSave('{0}');", Me.ctrRateAplicationNRpromo1.GetClientID))
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
        hpEliminate.NavigateUrl = Me.ctlMensajes1.getShow(lnkEliminate.ClientID, PortalCulture.GetString("00133"), PortalCulture.GetString("01661"))
    End Sub

    Public Sub CommandDelete(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        If e.CommandName = "Delete" Then

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
            Dim rateAmountMessages As RateAmountMessages = New RateAmountMessages()

            rateAmountMessages.HotelCode = info.Empresa
            rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            For Each room As DataGridItem In dgRooms.Items
                Dim checkBoxTemp As CheckBox = room.FindControl("deleteCheckbox")
                Dim labelTemp As Label = room.FindControl("glblRatePlanName")
                If checkBoxTemp.Checked Then
                    Dim iFareIdTemp As Integer = Integer.Parse(room.Cells(dgcolumns.idTarifa).Text)
                    Dim lblStartDateNoFormat As Label = room.FindControl("lblStartDateNoFormat")
                    Dim lblEndDateNoFormat As Label = room.FindControl("lblEndDateNoFormat")
                    Dim startDate As DateTime = Convert.ToDateTime(lblStartDateNoFormat.Text)
                    Dim endDate As DateTime = Convert.ToDateTime(lblEndDateNoFormat.Text)
                    Dim vDayRates As List(Of vDayRatesExceptions) = Helpers.Rates.RatesHelpers.GetVDayRateException(iFareIdTemp, startDate, endDate)
                    With New FaresExcFacade
                        If .DeleteFares(iFareIdTemp) Then
                            Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", PaginaBase.acciones.Eliminar, "Se eliminó la tarifa de la habitación " & room.Cells(dgcolumns.codigohabitacion).Text & " de la fecha " & room.Cells(dgcolumns.FechaInicia).Text & " a la fecha " & room.Cells(dgcolumns.FechaFinaliza).Text & " con el rateplan " & labelTemp.Text)
                            If isEnabledGoogleRequest Then
                                Parser.Parser.ToRateAmountMessagesDelete(Nothing, vDayRates, TypeRateEnum.RoomRatePromotion, rateAmountMessages.RateAmountMessagesList)
                            End If
                        End If
                    End With
                End If
            Next

            If isEnabledGoogleRequest Then
                Dim confluxService As New ConfluxService()
                Try
                    Dim res As RateResponse = confluxService.DeleteRates(rateAmountMessages)
                    Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", acciones.Eliminar, "Error al eliminar tarifas Conflux", "", res.RequestXML, res.Xml, info.Hotel)
                Catch ex As Exception

                    Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                    Dim errorElementProperty As New System.Xml.Linq.XElement("Error")

                    errorElementProperty.Add(
                    New System.Xml.Linq.XAttribute("Type", "3"),
                    New System.Xml.Linq.XAttribute("Code", "448"),
                    New System.Xml.Linq.XText(ex.Message)
                )

                    errorsElement.Add(errorElementProperty)

                    Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", acciones.Eliminar, "Error al eliminar tarifas Conflux", "", "", errorsElement.ToString(), info.Hotel)
                End Try
            End If

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

    Protected ReadOnly Property RateModeView() As Integer
        Get
            Dim value As Integer = 0

            If IsSupervisor Then
                If (Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Adult", Me.ctrRateAplicationNRpromo1.GetFareFor("Adult", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Children", Me.ctrRateAplicationNRpromo1.GetFareFor("Child", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Teen", Me.ctrRateAplicationNRpromo1.GetFareFor("Teen", False), False) _
                        OrElse _
                      (Me.CtrlPlanFaresExc2.FieldException <> "NNNNNNN" _
                        AndAlso (Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Adult", Me.ctrRateAplicationNRpromo1.GetFareFor("Adult", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Children", Me.ctrRateAplicationNRpromo1.GetFareFor("Child", False), False) _
                        OrElse _
                        Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Teen", Me.ctrRateAplicationNRpromo1.GetFareFor("Teen", False), False) _
                        ))) Then
                    value = 1
                End If
            End If
            If (Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Adult", Me.ctrRateAplicationNRpromo1.GetFareFor("Adult", True), True) _
               OrElse _
               Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Children", Me.ctrRateAplicationNRpromo1.GetFareFor("Child", True), True) _
               OrElse _
               Not Me.CtrlPlanFares2.IsFareValuesEqualTo("Teen", Me.ctrRateAplicationNRpromo1.GetFareFor("Teen", True), True) _
               OrElse _
             (Me.CtrlPlanFaresExc2.FieldException <> "NNNNNNN" _
               AndAlso (Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Adult", Me.ctrRateAplicationNRpromo1.GetFareFor("Adult", True), True) _
               OrElse _
               Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Children", Me.ctrRateAplicationNRpromo1.GetFareFor("Child", True), True) _
               OrElse _
               Not Me.CtrlPlanFaresExc2.IsFareValuesEqualTo("Teen", Me.ctrRateAplicationNRpromo1.GetFareFor("Teen", True), True) _
           ))) Then
                value = 1
            End If


            Return value
        End Get
    End Property

    Private Sub loadCulture()
        lblTitle.Text = PortalCulture.GetString("01358")
        If Editando Then
            lblMsg.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("01358"))
        Else
            lblMsg.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("01358"))
        End If
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

    Public Sub loadDatos()
        Dim Rooms As New Portal.Hotel.Common.Data.RoomsHotelData
        Dim RatesPlan As Portal.General.Common.Data.RatePlanData

        With New Portal.Hotel.Facade.RoomFacade
            Rooms = .getRooms(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        Rooms.Tables(Portal.Hotel.Common.Data.RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        RatesPlan = ctrRateAplicationNRpromo1.loadAllRatesplans(1)

        Me.ddlratesplans.DataSource = RatesPlan
        Me.ddlratesplans.DataValueField = Portal.General.Common.Data.RatePlanData.FIELD_IDRATEPLAN
        Me.ddlratesplans.DataTextField = "texto" 'Portal.General.Common.Data.RatePlanData.FIELD_CODIGOTARIFA
        Me.ddlratesplans.DataBind()


        Me.ddlratesplans.Items.Insert(0, "Todos")
        ddlratesplans.Items(0).Value = "0"
        ddlratesplans.Items(0).Text = PortalCulture.GetString("00172")
        ddlratesplans.SelectedIndex = 0

        Dim links As New LinkRoomTypeData
        With New LinkRoomsFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With

        Dim dv As DataView
        For Each r As DataRow In Rooms.Tables(Portal.Hotel.Common.Data.RoomsHotelData.TBL_ROOM_HOTEL).Rows
            dv = links.Tables(LinkRoomTypeData.TABLE_LINKROOM).DefaultView
            dv.RowFilter = LinkRoomTypeData.FIELD_TargetRoom & "=" & r(Portal.Hotel.Common.Data.RoomsHotelData.FLD_ID_ROOM_HOTEL)
            If dv.Count > 0 Then
                r.Delete()
            End If
        Next
        Rooms.AcceptChanges()



        ddlRooms.DataTextField = "texto" 'Rooms.FLD_ROOM_CODE
        ddlRooms.DataValueField = Portal.Hotel.Common.Data.RoomsHotelData.FLD_ID_ROOM_HOTEL
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
            hpl.NavigateUrl = Me.CtlMensajes1.getShow(lk.ClientID, PortalCulture.GetString("01134"), PortalCulture.GetString("00467"))
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

        Dim datFares As FaresDataExc
        Dim idAsoc As Integer = Me.GetIdAsociation

        If Me.idroom <> 0 Then
            With New FaresExcFacade
                datFares = .GetFaresByRoomTypeId(CInt(Me.idroom), PortalCulture.GetIDCulture, Me.chkOldDates.Checked, 1, idAsociacion:=idAsoc)
            End With
            datFares.Tables(FaresDataExc.FARESEXC_TABLE).Columns.Add("MaxPrice", GetType(System.Double))
            datFares.Tables(FaresDataExc.FARESEXC_TABLE).Columns.Add("MinPrice", GetType(System.Double))

            Dim dvFares As DataView
            dvFares = datFares.Tables(FaresDataExc.FARESEXC_TABLE).DefaultView

            'dvFares.RowFilter = " tipotarifa <>'K' "
            If Me.ddlratesplans.SelectedIndex <> 0 Then
                dvFares.RowFilter &= FaresDataExc.IDRATEPLAN_FIELD & "='" & ddlratesplans.SelectedItem.Value & "'"
            Else
                dvFares.RowFilter &= " packagetype = 0"
            End If

            Dim dv As New DataView
            For Each dvr As DataRowView In dvFares
                dv = datFares.Tables(1).DefaultView
                dv.RowFilter = FaresDataExc.PKIDFARESEXC_FIELD & "=" & dvr(FaresDataExc.PKIDFARESEXC_FIELD)
                If dv.Count > 0 Then
                    dvr("MinPrice") = dv(0)(KEY_MINPRICE)
                    dvr("MaxPrice") = dv(0)(KEY_MAXPRICE)
                Else
                    dvr("MinPrice") = dvr(FaresDataExc.PRICE_FIELD)
                    dvr("MaxPrice") = dvr(FaresDataExc.PRICE_FIELD)
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
        Me.ctrRateAplicationNRpromo1.Segment(ddlratesplans.SelectedValue)
        Me.ctrRateAplicationNRpromo1.LoadRooms(idroom)
        'Leemos los valores del porcentaje maximo y minimo


    End Sub

    Public Function GetFareRestrictions() As FaresRestrictionsDataExc
        CtrlPlanFaresExc2.FieldException = "NNNNNNN"
        If ctrRateAplicationNRpromo1.m_iFareId <> 0 Then
            CtrlPlanFaresExc2.FieldException = ctrRateAplicationNRpromo1.Exceptions
        End If
        Return Me.CtrlPlanFares2.GetFareRestrictions()
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
            Me.ctrRateAplicationNRpromo1.m_iFareId = iFareId
            Me.ctrRateAplicationNRpromo1.m_iHotelId = Me.cInfoActual.Hotel
            Me.ctrRateAplicationNRpromo1.m_StartDateFareId = startDateFareId
            Me.ctrRateAplicationNRpromo1.m_EndDateFareId = endDateFareId
            Me.ctrRateAplicationNRpromo1.LoadFare(iFareId, 0)
            Me.CtrlPlanFares2.m_iRoomId = Integer.Parse(e.Item.Cells(dgcolumns.idtipohabitacion_hotel).Text)
            Me.CtrlPlanFares2.m_iFareId = iFareId
            Me.CtrlPlanFares2.ReFill()
            Me.CtrlPlanFaresExc2.ReFill()
            pnlData.Style.Add("display", "")
            cmdNew.Style.Add("display", "none")
            Editando = True

        ElseIf e.CommandName = "Delete" Then

            iFareId = Integer.Parse(dgRooms.Items(e.Item.ItemIndex).Cells(dgcolumns.idTarifa).Text)
            With New FaresExcFacade
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
        Me.ctrRateAplicationNRpromo1.newFare()
        Me.CtrlPlanFares2.m_iFareId = 0
        Me.CtrlPlanFares2.ReFill()
        Me.CtrlPlanFaresExc2.ReFill()
        lblError.Visible = False
        cmdNew.Style.Add("display", "")
        pnlData.Style.Add("display", "none")
        lblPriceError.Visible = False
        Editando = False
    End Sub


    Public Shared Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPublish.Click
        lblPriceError.Visible = False
        Dim publish As Boolean = True '(CType(sender, Button).ID = Me.btnPublish.ID)
        Dim lstFare As Queue(Of Double)

        If CtrlPlanFaresExc2.IsValidData AndAlso CtrlPlanFares2.IsValidData AndAlso Me.ctrRateAplicationNRpromo1.IsValidData Then

            If Me.idroom <> 0 Then
                Me.lblNoroomSelected.Visible = False
                If Page.IsValid Then
                    lblError.Visible = False
                    If ctrRateAplicationNRpromo1.lstDatesCount = 0 Then
                        ctrRateAplicationNRpromo1.lstDatesAdd()
                        'guardar con lo que estan en desde hasta

                        If CDate(ctrRateAplicationNRpromo1.lstDatesItemI(1).Split("-")(1)) < CDate(ctrRateAplicationNRpromo1.lstDatesItemI(1).Split("-")(0)) Then
                            lblError.Visible = True

                            Exit Sub
                        End If


                    End If
                    Dim _exito As Boolean = True
                    Dim isNew As Boolean = (Me.ctrRateAplicationNRpromo1.m_iFareId = 0)
                    '  actualizaidroom()
                    Me.CtrlPlanFaresExc2.createFieldException()
                    ctrRateAplicationNRpromo1.Exceptions = Me.CtrlPlanFaresExc2.FieldException()
                    Dim chLast As String, rpLast As String = "", f1Last As String = "", f2Last As String = ""

                    Dim confluxService As New ConfluxService()
                    Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                    Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)

                    For i As Integer = 1 To ctrRateAplicationNRpromo1.lstDatesCount
                        Dim f1, f2 As Date
                        f1 = CDate(ctrRateAplicationNRpromo1.lstDatesItemI(i).Split("-")(0))
                        f2 = CDate(ctrRateAplicationNRpromo1.lstDatesItemI(i).Split("-")(1))
                        'cuando es modificación la primera se modifica pero las demas son add
                        chLast = String.Empty
                        rpLast = String.Empty
                        f1Last = String.Empty
                        f2Last = String.Empty
                        If dgRooms.SelectedIndex <> -1 Then
                            chLast = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.codigohabitacion).Text()
                            rpLast = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.RatePlan).Text()
                            f1Last = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.FechaInicia).Text()
                            f2Last = dgRooms.Items(dgRooms.SelectedIndex).Cells(dgcolumns.FechaFinaliza).Text()
                        Else
                            chLast = ddlRooms.SelectedItem.Text
                        End If

                        Dim bPorOcupacion As Integer = RateModeView
                        Dim scorreos As String = ""
                        CtrlPlanFares2.TarifaMinima(lstFare)

                        Dim auxFareId As Integer = 0
                        Dim vDayRates As List(Of vDayRatesExceptions) = Nothing
                        If Editando Then
                            vDayRates = Helpers.Rates.RatesHelpers.GetVDayRateException(ctrRateAplicationNRpromo1.m_iFareId, ctrRateAplicationNRpromo1.m_StartDateFareId, ctrRateAplicationNRpromo1.m_EndDateFareId)
                        End If

                        If ctrRateAplicationNRpromo1.AddFare(idroom, ctrRateAplicationNRpromo1.m_iFareId, f1, f2, chLast, rpLast, f1Last, f2Last, ddlRooms.SelectedItem.Text, publish, (bPorOcupacion = 1), lstFare, scorreos) = True Then
                            CtrlPlanFares2.m_iFareId = ctrRateAplicationNRpromo1.m_iFareId
                            auxFareId = ctrRateAplicationNRpromo1.m_iFareId
                            CtrlPlanFares2.Save(rpLast, ddlRooms.SelectedItem.Text, scorreos, Me.CtrlPlanFaresExc2.getRatesExceptions())
                            Me.dgRooms.SelectedIndex = -1
                            '-------- Tarifas especiales -------------------------
                            Me.CtrlPlanFares2.m_iRoomId = idroom
                            Me.CtrlPlanFares2.m_iFareId = 0
                            ctrRateAplicationNRpromo1.m_iFareId = 0

                            'Request Google
                            If isEnabledGoogleRequest Then
                                Try

                                    'If Editando Then
                                    '    'Eliminar Viejitas
                                    '    Dim rateAmountMessages As RateAmountMessages = New RateAmountMessages()

                                    '    rateAmountMessages.HotelCode = info.Empresa
                                    '    rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

                                    '    Parser.Parser.ToRateAmountMessagesDelete(Nothing, vDayRates, TypeRateEnum.RoomRatePromotion, rateAmountMessages.RateAmountMessagesList)

                                    '    Dim deleleteResponse As RateResponse = confluxService.DeleteRates(rateAmountMessages)
                                    '    Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", acciones.Eliminar, "Eliminar tarifa Conflux", "", deleleteResponse.RequestXML, deleleteResponse.Xml, info.Hotel)
                                    'End If


                                    Dim res As Tuple(Of RateResponse, RateResponse) = confluxService.UpdateRate(auxFareId, f1, f2, info.Hotel, info.Empresa, TypeRateEnum.RoomRatePromotion)

                                    Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", acciones.Sincronizar, "Tarifa enviada a Conflux", "", res.Item1.RequestXML, res.Item1.Xml, info.Hotel)

                                    'Delete Log
                                    If res.Item2 IsNot Nothing Then
                                        Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", acciones.Eliminar, "Eliminar tarifas Conflux", "", res.Item2.RequestXML, res.Item1.Xml, info.Hotel)
                                    End If

                                    'Cierre
                                    SendClosureByRateGoogle(auxFareId, f1, f2, confluxService, info)


                                Catch ex As Exception

                                    Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                                    Dim errorElementProperty As New System.Xml.Linq.XElement("Error")

                                    errorElementProperty.Add(
                                    New System.Xml.Linq.XAttribute("Type", "3"),
                                    New System.Xml.Linq.XAttribute("Code", "448"),
                                    New System.Xml.Linq.XText(ex.Message)
                                )

                                    errorsElement.Add(errorElementProperty)

                                    Me.guardalog("/Pages/FaresCataloguePromoNR.aspx", acciones.Sincronizar, "Error al enviar tarifa Conflux", "", "", errorsElement.ToString(), info.Hotel)
                                End Try
                            End If

                        Else
                                _exito = False
                            cmdNew.Style.Add("display", "none")
                            pnlData.Style.Add("display", "")
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
                        ctrRateAplicationNRpromo1.newFare()
                        Me.CtrlPlanFares2.ReFill()
                        Me.CtrlPlanFaresExc2.ReFill()

                        Try
                            Dim Mail As emailTemplates.Template = New emailTemplates.Template()

                            Mail.Idioma = PortalCulture.GetCulture().ToString()
                            Try
                                Mail.TemplateName = "TH_HotelNetRatesModified"
                            Catch ex As Exception
                            End Try
                            Mail.Html = True

                            Dim Prov As New PortalPartnersCfg
                            Prov.LoadPartnerById(2)

                            Mail.AddParameter("HEADER") = Prov.EmailHeader
                            Mail.AddParameter("FOOTER") = Prov.EmailFooter
                            Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css' type='text/css' rel='stylesheet'>"

                            Mail.AddParameter("HOTEL") = Me.cInfoActual.HotelName
                            Mail.AddParameter("OPERATION") = If(isNew, "agregado una nueva", "cambiado una")
                            Mail.AddParameter("ROOMCODE") = chLast
                            Mail.AddParameter("RATECODE") = Me.ctrRateAplicationNRpromo1.RatePlanRow.RATECODE

                            Mail.Send()

                        Catch ex As Exception
                        End Try

                    End If
                End If
            Else
                Me.lblNoroomSelected.Visible = True
            End If
        Else
            'hplShowRates.Style.Add("display", "")
            'hplHideRates.Style.Add("display", "none")
            ''Me.Page.RegisterStartupScript("ShowGrid", "<script>Ocultar('1');</script>")
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowGrid", "<script>Ocultar('1');</script>")

            If Not CtrlPlanFaresExc2.IsValidData Then
                'Me.Page.RegisterStartupScript("ShowPrice", "<script>optionSw('1E');</script>")
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowPrice", "<script>optionSw('1E');</script>")
            End If
            If Not CtrlPlanFares2.IsValidData Then
                'Me.Page.RegisterStartupScript("ShowPrice", "<script>optionSw('1P');</script>")
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowPrice", "<script>optionSw('1P');</script>")
            End If
            cmdNew.Style.Add("display", "none")
            pnlData.Style.Add("display", "")
            lblPriceError.Visible = True
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
        iFareId = ctrRateAplicationNRpromo1.m_iFareId
        With New FaresExcFacade
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

    Private Sub dgRooms_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRooms.PreRender
        If Not Me.IsSupervisor Then
            Me.dgRooms.Columns(dgcolumns.CurrencyCode).Visible = False
            Me.dgRooms.Columns(dgcolumns.MaxRate).Visible = False
            Me.dgRooms.Columns(dgcolumns.MinRate).Visible = False
            Me.dgRooms.Columns(dgcolumns.Delete).Visible = False
        End If
    End Sub

    Private Sub SendClosureByRateGoogle(ByVal rateId As Integer, ByVal startDate As Date, ByVal endDate As Date, ByVal confluxService As ConfluxService, ByVal info As companyInfo)

        Dim requests As List(Of XDocument) = New List(Of XDocument)

        Dim vDayRatesForClosure As List(Of vDayRatesExceptions) = Helpers.Rates.RatesHelpers.GetVDayRateException(rateId, startDate, endDate)

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
            Dim response As Models.Restrictions.Response.RestrictionResponse = confluxService.UpdateRestriction(request, RestrictionEnum.LockRate)
            restrictionResponseList.Add(response)
        Next

        For Each response As Models.Restrictions.Response.RestrictionResponse In restrictionResponseList

            If response.IsSuccess Then
                Me.WriteLog(response.Restrictions(0).XmlRequest(0).ToString(), "LockRateExceptions")
                Me.WriteLog(response.Restrictions(0).Xml(0).ToString(), "LockRateExceptions")
            Else
                Me.WriteLog(response.Xml.ToString(), "LockRate")
            End If

        Next

    End Sub


End Class