Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Common.Data
Imports Portal.General.Facade

Public Class Promotions
    Inherits PaginaBase

    Dim dsegmentos As DataSet
    Dim dsRatesPlan As DataSet

    Private Enum cancelpolicy
        useDefault
        bydays
        byhour
        specifichour
    End Enum

    Enum dgcolumns
        idrateplan
        orden
        code
        name
        StartDate
        EndDate
        segment
        edit
        eliminar
        activar
        principalSegmentRac
        codigotarifa
        deleted
    End Enum

    Private Property Cerror() As Integer
        Get
            Return ViewState("_cerror")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_cerror") = Value
        End Set
    End Property

    Protected Property HasData() As Boolean
        Get
            HasData = False
            If Me.ViewState("HasData") IsNot Nothing Then HasData = Me.ViewState("HasData")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasData") = value
        End Set
    End Property

    Public Property IdRule() As Integer
        Get
            Return ViewState("_idrule")
        End Get
        Set(ByVal Value As Integer)
            ViewState("_idrule") = Value
        End Set
    End Property

    Private Property idDiccPCReview() As Integer
        Get
            Return ViewState("idDiccPCReview")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDiccPCReview") = Value
        End Set
    End Property

    Private Property idDiccPCFull() As Integer
        Get
            Return ViewState("idDiccPCFull")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDiccPCFull") = Value
        End Set
    End Property

    Private Property idShortDesc() As Integer
        Get
            Return ViewState("idShortDesc")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idShortDesc") = Value
        End Set
    End Property

    Private Property idDicc() As Integer
        Get
            Return ViewState("idDicc")
        End Get
        Set(ByVal Value As Integer)
            ViewState("idDicc") = Value
        End Set
    End Property

    Public Property edicion() As Boolean
        Get
            Return ViewState("Edicion")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Edicion") = Value

        End Set
    End Property

    Public Property IdRatePlan() As String
        Get
            Return ViewState("_IdRatePlan")
        End Get
        Set(ByVal Value As String)
            ViewState("_IdRatePlan") = Value
        End Set
    End Property

    Public Property m_iHotelId() As Integer
        Get
            Return ViewState(KEY_HOTELID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_HOTELID) = Value
        End Set
    End Property

    Const KEY_HOTELID = "HotelId"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            ddlCancelationPolicy.Items.Clear()
            ddlCancelationPolicy.Items.Insert(cancelpolicy.useDefault, PortalCulture.GetString("00796"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.bydays, PortalCulture.GetString("00020"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.byhour, PortalCulture.GetString("00021"))
            ddlCancelationPolicy.Items.Insert(cancelpolicy.specifichour, PortalCulture.GetString("00381"))
            'fillGuar()
            Me.txtCancellationPolicy.Style.Add("display", "none")
            Me.ddlHour.Style.Add("display", "none")
            Me.lblSep.Style.Add("display", "none")
            Me.lblAux.Style.Add("display", "none")
            lblEDaysHour.Style.Add("display", "none")
            Me.ddlMinutes.Style.Add("display", "none")
            Me.lblAux.Text = PortalCulture.GetString("00413")
        End If
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        If Not IsPostBack Then
            m_iHotelId = cInfoActual.Hotel
            ddlDeletedFilter.Items.Clear()
            ddlDeletedFilter.Items.Add(New ListItem(PortalCulture.GetString("01541"), 1))
            ddlDeletedFilter.Items.Add(New ListItem(PortalCulture.GetString("01542"), 0))
            ddlDeletedFilter.Items.Add(New ListItem(PortalCulture.GetString("01543"), -1))

            GetAllRooms()
            GetAllRatePlans()
            LoadResources()
            MostrarCmdNew(True)
            LoadGridRatePlans(ctrlAutoComplete1.GetFilter)
            For Each item As ListItem In chklSpecificArrivals.Items
                item.Selected = False
            Next
        End If
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
    End Sub

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Select Case SavePromo(True)
            Case 0
                LoadGridRatePlans("")
                MostrarCmdNew(True)
            Case 1 Or 2
                lblError.Visible = True
                lblError.Text = "El código de promoción ya existe"
                MostrarCmdNew(False)
            Case 3
                lblError.Visible = True
                lblError.Text = "Ocurrió un error al guardar los Planes Tarifarios"
                MostrarCmdNew(False)
            Case 4
                lblError.Visible = True
                lblError.Text = "Ocurrió un error al guardar las Habitaciones"
                MostrarCmdNew(False)
            Case 5
                lblError.Visible = True
                lblError.Text = "Ocurrió un error al guardar Blackout days"
                MostrarCmdNew(False)
        End Select
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click

        edicion = False
        'lblError.Visible = False
        'lblErrorSource.Visible = False
        ClearData()
        Me.grid.SelectedIndex = -1
        Cerror = 0
        MostrarCmdNew(True)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Private Sub LoadResources()
        ddlCancelationPolicy.Items.Clear()
        ddlCancelationPolicy.Items.Insert(cancelpolicy.useDefault, PortalCulture.GetString("00796"))
        ddlCancelationPolicy.Items.Insert(cancelpolicy.bydays, PortalCulture.GetString("00020"))
        ddlCancelationPolicy.Items.Insert(cancelpolicy.byhour, PortalCulture.GetString("00021"))
        ddlCancelationPolicy.Items.Insert(cancelpolicy.specifichour, PortalCulture.GetString("00381"))

        Me.btncancel.Text = PortalCulture.GetString("00009")
        'fillGuar()
        Me.txtCancellationPolicy.Style.Add("display", "none")
        Me.ddlHour.Style.Add("display", "none")
        Me.lblSep.Style.Add("display", "none")
        Me.lblAux.Style.Add("display", "none")
        lblEDaysHour.Style.Add("display", "none")
        Me.ddlMinutes.Style.Add("display", "none")
        Me.lblAux.Text = PortalCulture.GetString("00413")
        cmdNew.Value = PortalCulture.GetString("00102")

        ddlCancelationPolicy.Items(cancelpolicy.useDefault).Text = PortalCulture.GetString("M000640")
        ddlCancelationPolicy.Items(cancelpolicy.bydays).Text = PortalCulture.GetString("00020")
        ddlCancelationPolicy.Items(cancelpolicy.byhour).Text = PortalCulture.GetString("00021")
        ddlCancelationPolicy.Items(cancelpolicy.specifichour).Text = PortalCulture.GetString("00381")
        If ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays Then
            Me.lblAux.Text = PortalCulture.GetString("00413")
            lblEDaysHour.Text = PortalCulture.GetString("00410")
        ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour Then
            Me.lblAux.Text = PortalCulture.GetString("00413")
            lblEDaysHour.Text = PortalCulture.GetString("00409")
        Else
            Me.lblAux.Text = PortalCulture.GetString("00412")
            lblEDaysHour.Text = PortalCulture.GetString("00411")
        End If
    End Sub

    Private Sub GetAllRooms()
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getAllRooms(cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With

        room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,35)")

        chkListRoom.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
        chkListRoom.DataTextField = "texto"
        chkListRoom.DataSource = room
        chkListRoom.DataBind()
    End Sub

    Private Sub GetAllRatePlans()
        Dim ds As RatePlanData
        Dim idAsoc As Integer = GetIdAsociation()
        Dim dvRp As DataView

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue))
        End With

        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
            ds.Tables(RatePlanData.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String")) ', "substring(" & RatePlanData.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & RatePlanData.FIELD_NAME & ",1,35)")
            ds.Tables(RatePlanData.RATEPLAN_TABLE).Columns("texto").ReadOnly = False
            For Each row As DataRow In ds.Tables(0).Rows
                row.Item("texto") = row.Item(RatePlanData.FIELD_CODIGOTARIFA).ToString & " -- " & row.Item(RatePlanData.FIELD_NAME).ToString
                If row.IsNull(RatePlanData.FIELD_IDCONTRATO) Then
                    row.Item("texto") += " (Comisionable)"
                End If
            Next
            dvRp = ds.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView
            dvRp.RowFilter = "isPromo  is null or isPromo = 0"

            chlListContract.DataValueField = RatePlanData.FIELD_IDRATEPLAN
            chlListContract.DataTextField = "texto"
            chlListContract.DataSource = dvRp.ToTable()
            chlListContract.DataBind()
        End If

    End Sub

    Public Function SavePromo(ByVal publish As Boolean) As Integer
        Dim strError As String = String.Empty

        Dim dsRate As New RatePlanData
        Dim Rp As RatePlanData
        Dim rRate As DataRow
        Dim val As Boolean
        Dim idRate, NuevoIdRate As String
        Dim idHotel As Integer, dvRp As DataView
        Dim GDSAplicado As String = String.Empty
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim idAsoc As Integer = Me.GetIdAsociation


        With New RatePlanFacade
            Rp = .GetRatePlanByIdHotel(Me.m_iHotelId, idAsociacion:=idAsoc)
        End With
        sDataPrev = getDataXML()


        dvRp = Rp.Tables(Rp.RATEPLAN_TABLE).DefaultView
        If Not Me.edicion Then
            dvRp.RowFilter = dsRate.FIELD_CODIGOTARIFA & "='" & Me.txtPromotionCode.Text.Trim & "' and " & dsRate.FIELD_IDHOTEL & "=" & Me.m_iHotelId
        Else
            dvRp.RowFilter = dsRate.FIELD_CODIGOTARIFA & "='" & Me.txtPromotionCode.Text.Trim & "' and " & dsRate.FIELD_IDHOTEL & "=" & Me.m_iHotelId & " and " & dsRate.FIELD_IDRATEPLAN & "<>'" & Me.IdRatePlan & "'"
        End If

        If dvRp.Count > 0 Then
            If dvRp(0)("Deleted").ToString() = "True" Then
                Return 1
            Else
                Return 2
            End If

        End If

        Dim ComGDS As Integer = 0.0
        Dim ComPortal As Integer = 0.0
        Dim ComOnePage As Integer = 0.0
        Dim ComADS As Integer = 0.0
        rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
        With rRate
            If Me.edicion = False Then
                .Item(dsRate.FIELD_IDRATEPLAN) = Me.txtPromotionCode.Text.ToUpper 'Me.txtCodigo.Text.ToUpper
            Else
                .Item(dsRate.FIELD_IDRATEPLAN) = IdRatePlan
            End If
            .Item(dsRate.FIELD_DESCRIPTION) = Me.txtPromoDescription.textodefault


            .Item(dsRate.FIELD_SEGMENT) = "R" 'Me.ddlSegmentos.SelectedValue
            .Item(dsRate.FIELD_IDHOTEL) = Me.m_iHotelId
            .Item(dsRate.FIELD_HOTELPAYMENT) = False 'hotelPayment.Checked
            .Item(dsRate.FIELD_CODIGOTARIFA) = Me.txtPromotionCode.Text.ToUpper
            .Item(dsRate.FIELD_NAME) = Me.txtPromoName.textodefault
            .Item(dsRate.FIELD_IDDICSHORTDESC) = Me.txtPromoName.IdIndice
            .Item(dsRate.FIELD_IDDICCPROMODESC) = Me.txtAddValueDescription.IdIndice
            .Item(dsRate.FIELD_IDDICDESC) = Me.txtPromoDescription.IdIndice
            .Item(dsRate.FIELD_GDS) = False 'chkGDS.Checked
            .Item(dsRate.FIELD_GDSAPPLY) = "NNNN" 'GDSAplicado
            .Item(dsRate.FIELD_PORTAL) = True 'chkPortal.Checked
            .Item(dsRate.FIELD_UNIPANTALLA) = False 'chkUnipantalla.Checked
            .Item(dsRate.FIELD_ADS) = False 'chkADS.Checked
            .Item(dsRate.FIELD_COMGDS) = IIf(ComGDS = -1, System.DBNull.Value, ComGDS)
            .Item(dsRate.FIELD_COMPORTAL) = IIf(ComPortal = -1, System.DBNull.Value, ComPortal)
            .Item(dsRate.FIELD_COMONEPAGE) = IIf(ComOnePage = -1, System.DBNull.Value, ComOnePage)
            .Item(dsRate.FIELD_COMADS) = IIf(ComADS = -1, System.DBNull.Value, ComADS)
            .Item(dsRate.WAITLISTAVAILABLE_FIELD) = False 'Me.chkWaitListAvailable.Checked

            .Item(dsRate.FIELD_ISPROMO) = True

            If (Me.txtFreeNight.Text <> "") AndAlso chkPromoNights.Checked Then '(chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkGDS.Checked)) Then
                .Item(dsRate.FIELD_DAYSFREE) = CInt(txtFreeNight.Text)
                .Item(dsRate.FIELD_DAYSFREETYPE) = CBool(ddlFreeNight.SelectedValue) '0 - Cada, 1 - Solo
            End If

            If (Me.txtPromoDiscount.Text <> "") AndAlso chkPromoDiscount.Checked Then '(chkPortal.Checked Or Me.chkUnipantalla.Checked Or Me.chkGDS.Checked)) Then
                .Item(dsRate.FIELD_DESCPROMOTION) = CDbl(txtPromoDiscount.Text)
                .Item(dsRate.FIELD_TIPODESCUENTO) = rblDiscountOptions.SelectedValue
                .Item(dsRate.FIELD_DISCAPPLICATIONMODE) = ddlApplicationMode.SelectedValue
            End If

            .Item(dsRate.FIELD_IDRULE) = System.DBNull.Value
            .Item(dsRate.FIELD_ORDEN) = System.DBNull.Value
            .Item(RatePlanData.FIELD_IsCOMBINABLEPROMO) = chkIscombinable.Checked
            If SaveRules(True, IdRule) Then
                .Item(dsRate.FIELD_IDRULE) = IdRule
            End If
        End With
        dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)
        Dim dsPromoRateplan As RatePlanData
        Dim dsPromoRoom As RatePlanData
        Dim rPromoRateplan As DataRow
        Dim rPromoRoom As DataRow

        If Me.edicion = False Then
            If IdRule > 0 Then
                With New RatePlanFacade
                    Dim idPromoDescription As Integer = Me.txtAddValueDescription.IdIndice
                    If .InsertRatePlan(dsRate, Me.idDicc, Me.idShortDesc, idPromoDescription) Then
                        Me.txtPromoDescription.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICDESC), publish)
                        Me.txtPromoName.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICSHORTDESC), publish)
                        Me.txtAddValueDescription.Update(idPromoDescription, publish)
                        If bookingWindowFrom.Text <> "" AndAlso bookingWindowTo.Text <> "" Then
                            If CheckBoxDefHora.Checked = True Then
                                .InsertRatePlanDeal(Me.txtPromotionCode.Text.ToUpper, Me.m_iHotelId, Date.ParseExact(bookingWindowFrom.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"),
                                                    Date.ParseExact(bookingWindowTo.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"), HoraInicio.SelectedValue & ":" & MinutoInicio.SelectedValue, HoraFin.SelectedValue & ":" & MinutoFin.SelectedValue)
                            Else
                                .InsertRatePlanDeal(Me.txtPromotionCode.Text.ToUpper, Me.m_iHotelId, Date.ParseExact(bookingWindowFrom.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"),
                                                    Date.ParseExact(bookingWindowTo.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"), "", "")
                            End If
                        End If

                        For Each plan As ListItem In chlListContract.Items
                            If plan.Selected Then
                                dsPromoRateplan = New RatePlanData
                                rPromoRateplan = dsPromoRateplan.Tables(RatePlanData.PROMOCIONES_RATEPLAN_TABLE).NewRow

                                rPromoRateplan.Item(RatePlanData.FIELD_IDRATEPLAN) = plan.Value
                                rPromoRateplan.Item(RatePlanData.FIELD_IDPROMOCION) = txtPromotionCode.Text
                                rPromoRateplan.Item(RatePlanData.FIELD_IDHOTEL) = Me.m_iHotelId

                                dsPromoRateplan.Tables(RatePlanData.PROMOCIONES_RATEPLAN_TABLE).Rows.Add(rPromoRateplan)

                                If Not .InsertPromotionRatePlan(dsPromoRateplan, strError) Then
                                    Return 3
                                End If
                            End If
                        Next

                        For Each room As ListItem In chkListRoom.Items
                            If room.Selected Then
                                dsPromoRoom = New RatePlanData
                                rPromoRoom = dsPromoRoom.Tables(RatePlanData.PROMOCIONES_TIPOHABITACIONHOTEL_TABLE).NewRow

                                rPromoRoom.Item(RatePlanData.FIELD_IDPROMOCION) = txtPromotionCode.Text
                                rPromoRoom.Item(RatePlanData.FIELD_IDTIPOHABITACIONHOTEL) = room.Value
                                rPromoRoom.Item(RatePlanData.FIELD_IDHOTEL) = Me.m_iHotelId

                                dsPromoRoom.Tables(RatePlanData.PROMOCIONES_TIPOHABITACIONHOTEL_TABLE).Rows.Add(rPromoRoom)
                                If Not .InsertPromotionRoom(dsPromoRoom, strError) Then
                                    Return 4
                                End If
                            End If
                        Next

                        If open_blackout.Checked AndAlso Not txtDiasBlackout.Value = "" Then
                            If Not SaveBlackoutDays() Then
                                Return 5
                            End If
                        Else
                            With New RoomClosure
                                If .DeleteLockTypeRoomsByIdRatePlan(txtPromotionCode.Text.Trim, Me.m_iHotelId, strError) Then

                                End If
                            End With
                        End If

                        '            Me.strError.Value = strError

                        ClearData()
                        '            clearConfDealData()

                        Return 0
                    Else
                        lblError.Visible = True
                        lblError.Text = "Ocurrio un error al guardar la promoción. Asegurese que todos los datos seam correctos y que el código de la promoción no haya sido registrado previamente."
                    End If
                End With
            End If
        Else

            If Me.idDicc <> 0 Then
                Me.txtPromoDescription.Update(Me.idDicc, publish)
            Else
                idDicc = Me.txtPromoDescription.Insert()
            End If

            If Me.idShortDesc <> 0 Then
                Me.txtPromoName.Update(Me.idShortDesc, publish)
            Else
                idShortDesc = Me.txtPromoName.Insert()
            End If

            If dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC) <> 0 Then
                Me.txtAddValueDescription.Update(dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC), publish)
            Else
                dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0)(dsRate.FIELD_IDDICCPROMODESC) = Me.txtAddValueDescription.Insert()
            End If

            dsRate.Tables(dsRate.RATEPLAN_TABLE).AcceptChanges()
            dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDHOTEL) = dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDHOTEL)
            If idDicc <> 0 Then
                dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDDICDESC) = idDicc
            End If
            If idShortDesc <> 0 Then
                dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDDICSHORTDESC) = idShortDesc
            End If
            dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_IDRULE) = IdRule
            dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows(0).Item(dsRate.FIELD_ISPROMO) = True
            With New RatePlanFacade
                If .UpdateRatePlan(dsRate) Then

                    sData = Util.Utility.GetXml(dsRate.RATEPLAN_TABLE, "UpdatePlanPlan", dsRate)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatesPlans.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Modifico el rateplan con el id " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "", sDataPrev, sData)
                    If Me.txtAddValueDescription.HasChanges OrElse Me.txtPromoDescription.HasChanges Then
                        CType(Me.Page, PaginaBase).NotifyContentModification("Plan tarifario con el codigo " & rRate(dsRate.FIELD_IDRATEPLAN) & " y el codigo de tarifa " & rRate(dsRate.FIELD_CODIGOTARIFA), "Planes Tarifarios")
                    End If

                    Dim AllWorld As Boolean = True

                    If .DeletePromoRatePlans(rRate(dsRate.FIELD_IDRATEPLAN), Me.m_iHotelId) Then
                        For Each plan As ListItem In chlListContract.Items
                            If plan.Selected Then
                                dsPromoRateplan = New RatePlanData
                                rPromoRateplan = dsPromoRateplan.Tables(RatePlanData.PROMOCIONES_RATEPLAN_TABLE).NewRow

                                rPromoRateplan.Item(RatePlanData.FIELD_IDRATEPLAN) = plan.Value
                                rPromoRateplan.Item(RatePlanData.FIELD_IDPROMOCION) = txtPromotionCode.Text
                                rPromoRateplan.Item(RatePlanData.FIELD_IDHOTEL) = Me.m_iHotelId

                                dsPromoRateplan.Tables(RatePlanData.PROMOCIONES_RATEPLAN_TABLE).Rows.Add(rPromoRateplan)

                                If Not .InsertPromotionRatePlan(dsPromoRateplan, strError) Then
                                    Return 3
                                End If
                            End If
                        Next
                    End If

                    If .DeletePromoRooms(rRate(dsRate.FIELD_IDRATEPLAN), Me.m_iHotelId) Then
                        For Each room As ListItem In chkListRoom.Items
                            If room.Selected Then
                                dsPromoRoom = New RatePlanData
                                rPromoRoom = dsPromoRoom.Tables(RatePlanData.PROMOCIONES_TIPOHABITACIONHOTEL_TABLE).NewRow

                                rPromoRoom.Item(RatePlanData.FIELD_IDPROMOCION) = txtPromotionCode.Text
                                rPromoRoom.Item(RatePlanData.FIELD_IDTIPOHABITACIONHOTEL) = room.Value
                                rPromoRoom.Item(RatePlanData.FIELD_IDHOTEL) = Me.m_iHotelId

                                dsPromoRoom.Tables(RatePlanData.PROMOCIONES_TIPOHABITACIONHOTEL_TABLE).Rows.Add(rPromoRoom)
                                If Not .InsertPromotionRoom(dsPromoRoom, strError) Then
                                    Return 4
                                End If
                            End If
                        Next
                    End If

                    If bookingWindowFrom.Text <> "" AndAlso bookingWindowTo.Text <> "" Then
                        If CheckBoxDefHora.Checked = True Then
                            .UpdateRatePlanDeal(IdRatePlan, Me.m_iHotelId, Date.ParseExact(bookingWindowFrom.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"),
                                                Date.ParseExact(bookingWindowTo.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"), HoraInicio.SelectedValue & ":" & MinutoInicio.SelectedValue, HoraFin.SelectedValue & ":" & MinutoFin.SelectedValue)
                        Else
                            .UpdateRatePlanDeal(IdRatePlan, Me.m_iHotelId, Date.ParseExact(bookingWindowFrom.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"),
                                                Date.ParseExact(bookingWindowTo.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"), "", "")
                        End If
                    Else
                        .DelRateRatePlanDeal(IdRatePlan, Me.m_iHotelId)
                    End If

                    If open_blackout.Checked AndAlso Not txtDiasBlackout.Value = "" Then
                        SaveBlackoutDays()
                    Else
                        With New RoomClosure
                            If .DeleteLockTypeRoomsByIdRatePlan(txtPromotionCode.Text.Trim, Me.m_iHotelId, strError) Then

                            End If
                        End With
                    End If
                    'End If

                    'clearConfDealData()
                    ClearData()
                    Return 0
                End If
            End With
        End If
        Return 2

    End Function

    Private Function SaveBlackoutDays() As Boolean
        Dim BlackoutFrom As String
        Dim BlackoutTo As String
        Dim strError As String = String.Empty
        Dim BlackOutDates As String() = txtDiasBlackout.Value.ToString().Substring(0, txtDiasBlackout.Value.Length - 1).Split("|")
        Try
            With New RoomClosure
                If .DeleteLockTypeRoomsByIdRatePlan(txtPromotionCode.Text.Trim, Me.m_iHotelId, strError) Then
                    For Each dates As String In BlackOutDates
                        If Not dates = "" Then
                            BlackoutFrom = dates.Split("-")(0)
                            BlackoutTo = dates.Split("-")(1)
                            For Each room As ListItem In chkListRoom.Items
                                If Not .LockRoomTypes(cInfoActual.Hotel, txtPromotionCode.Text.Trim, Date.ParseExact(BlackoutFrom, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"),
                                                      Date.ParseExact(BlackoutTo, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo).ToString("MM/dd/yyyy"), room.Value, "C", strError) Then
                                    lblError.Text = strError
                                    lblError.Visible = True
                                    Return False
                                End If
                            Next
                        End If
                    Next
                Else
                    Return False
                End If
            End With
        Catch ex As Exception
            lblError.Text = strError
            lblError.Visible = True
            Return False
        End Try

        Return True
    End Function

    Private Function LoadPromoRatesPlan() As Boolean
        Dim ds As New DataSet
        Try
            With New RatePlanFacade
                ds = .GetPromoRatesPlanByPromoCode(IdRatePlan, cInfoActual.Hotel)
            End With
            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                For Each item As ListItem In chlListContract.Items
                    For Each row As DataRow In ds.Tables(0).Rows
                        If item.Value = row.Item("IdRatePlan") Then
                            item.Selected = True
                        End If
                    Next
                Next
            End If
            Return True
        Catch ex As Exception

        End Try

        Return False

    End Function

    Private Function LoadPromoRooms()
        Dim ds As New DataSet
        Try
            With New RatePlanFacade
                ds = .GetPromoRoomsByPromoCode(IdRatePlan, cInfoActual.Hotel)
            End With
            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                For Each item As ListItem In chkListRoom.Items
                    For Each row As DataRow In ds.Tables(0).Rows
                        If item.Value = row.Item("IdTipoHabitacionHotel") Then
                            item.Selected = True
                        End If
                    Next
                Next
            End If
            Return True
        Catch ex As Exception

        End Try
        Return False
    End Function

    Public Sub loadPromo(ByVal id As String, ByVal Principal As Boolean)
        Dim dsRatePlan As RatePlanData
        Dim dato As String = String.Empty
        Dim dsAssignCountry As DataSet = New DataSet
        'HasAssignamentCountry = False


        With New RatePlanFacade
            dsRatePlan = .GetDataRatePlan(id, Me.m_iHotelId)
        End With
        If dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows.Count > 0 Then
            Me.HasData = True
            With dsRatePlan.Tables(dsRatePlan.RATEPLAN_TABLE).Rows(0)
                If Not .IsNull(dsRatePlan.FIELD_IDDICDESC) Then
                    idDicc = .Item(dsRatePlan.FIELD_IDDICDESC)
                Else
                    idDicc = 0
                End If

                Trace.Write("HotelPayment antes")
                
                Trace.Write("HotelPayment despues")
                If Not .IsNull(dsRatePlan.FIELD_IDDICSHORTDESC) Then
                    idShortDesc = .Item(dsRatePlan.FIELD_IDDICSHORTDESC)
                Else
                    idShortDesc = 0
                End If
                txtPromoDescription.CargaDatos(idDicc)

                Me.txtPromoName.CargaDatos(idShortDesc)
                Me.txtPromoDescription.textodefault = .Item(dsRatePlan.FIELD_DESCRIPTION).ToString
                Me.txtPromoName.CargaDatos(idShortDesc)
                Me.txtPromoName.textodefault = .Item(dsRatePlan.FIELD_NAME).ToString
                Me.txtPromotionCode.Enabled = False
                Me.txtPromotionCode.Text = .Item(dsRatePlan.FIELD_CODIGOTARIFA).ToString
                If Not .IsNull(dsRatePlan.FIELD_DAYSFREE) Then
                    Me.txtFreeNight.Text = .Item(dsRatePlan.FIELD_DAYSFREE).ToString
                    Me.ddlFreeNight.SelectedValue = .Item(dsRatePlan.FIELD_DAYSFREETYPE)
                    chkPromoNights.Checked = True
                Else
                    Me.txtFreeNight.Text = ""
                    Me.ddlFreeNight.SelectedValue = "0"
                End If
                If Not .IsNull(dsRatePlan.FIELD_DESCPROMOTION) Then
                    Me.txtPromoDiscount.Text = .Item(dsRatePlan.FIELD_DESCPROMOTION).ToString
                    chkPromoDiscount.Checked = True
                End If
                If Not .IsNull(dsRatePlan.FIELD_TIPODESCUENTO) Then
                    rblDiscountOptions.SelectedIndex = .Item(dsRatePlan.FIELD_TIPODESCUENTO) - 1
                End If

                If Not .IsNull(dsRatePlan.FIELD_DISCAPPLICATIONMODE) Then
                    ddlApplicationMode.SelectedValue = .Item(dsRatePlan.FIELD_DISCAPPLICATIONMODE)
                End If

                If Not .IsNull(dsRatePlan.FIELD_IDDICCPROMODESC) Then
                    Me.txtAddValueDescription.CargaDatos(.Item(dsRatePlan.FIELD_IDDICCPROMODESC))
                    If Not txtAddValueDescription.GetEN() = "" AndAlso Not txtAddValueDescription.GetES() = "" Then
                        chkPromoAdd.Checked = True
                    End If
                Else
                    Me.txtAddValueDescription.CargaDatos(0)
                End If
                If Not .IsNull(RatePlanData.FIELD_IsCOMBINABLEPROMO) Then
                    chkIscombinable.Checked = .Item(RatePlanData.FIELD_IsCOMBINABLEPROMO)
                End If
                Dim rateCom As Integer

                IdRule = .Item(dsRatePlan.FIELD_IDRULE)

                Try
                    loadRule(IdRule)
                Catch ex As Exception
                    lblError.Visible = True
                    lblError.Text = ex.Message
                End Try
            End With

            Dim dsBlackOutDates As DataSet
            With New RoomClosure
                dsBlackOutDates = .GetLockRoomTypesByIdHotel(Me.m_iHotelId)
            End With
            If dsBlackOutDates.Tables.Count > 0 AndAlso dsBlackOutDates.Tables(0).Rows.Count > 0 Then
                Dim dvBlackoutDates As DataView
                Dim dtDistinct As DataTable
                dvBlackoutDates = dsBlackOutDates.Tables(0).DefaultView
                dvBlackoutDates.RowFilter = "IdRatePlan = '" & IdRatePlan & "'"
                Dim columns(1) As String
                columns(0) = "StartDate"
                columns(1) = "EndDate"
                dtDistinct = dvBlackoutDates.ToTable(True, columns)
                If dtDistinct.Rows.Count > 0 Then
                    For Each row As DataRow In dtDistinct.Rows
                        txtDiasBlackout.Value &= Date.ParseExact(CType(row.Item("StartDate").ToString(), Date).ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("dd/MM/yyyy") & "-" _
                                                & Date.ParseExact(CType(row.Item("EndDate").ToString(), Date).ToString("MM/dd/yyyy"), "MM/dd/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("dd/MM/yyyy") & "|"
                        'CType(row.Item("StartDate").ToString(), Date).ToString("dd/MM/yyyy") & "-" & CType(row.Item("EndDate").ToString(), Date).ToString("dd/MM/yyyy") & "|"
                        open_blackout.Checked = True
                    Next
                    txtDiasBlackout.Value = txtDiasBlackout.Value.Substring(0, txtDiasBlackout.Value.Length - 1)
                End If
            End If

            'Leemos la configuracion de hora si es que tiene
            With New RatePlanFacade
                Dim ds As New DataSet
                ds = .GetRateRatePlanDeal(IdRatePlan, m_iHotelId)
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim startDate, endDate As Date

                    bookingWindowFrom.Text = IIf(Date.TryParse(ds.Tables(0).Rows(0)("fechaInicio"), startDate), startDate.ToString("dd/MM/yyyy"), "")
                    bookingWindowTo.Text = IIf(Date.TryParse(ds.Tables(0).Rows(0)("fechaFin"), endDate), endDate.ToString("dd/MM/yyyy"), "")

                    If ds.Tables(0).Rows(0)("horaInicio") Is DBNull.Value Or ds.Tables(0).Rows(0)("horaInicio").ToString().Length = 0 Then
                        CheckBoxDefHora.Checked = False
                        HoraInicio.Enabled = False
                        MinutoInicio.Enabled = False
                        HoraFin.Enabled = False
                        MinutoFin.Enabled = False
                    Else
                        CheckBoxDefHora.Checked = True
                        HoraInicio.Enabled = True
                        MinutoInicio.Enabled = True
                        HoraFin.Enabled = True
                        MinutoFin.Enabled = True
                        'Leemos la configuracion de la hora
                        HoraInicio.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraInicio"), String).Substring(0, 2)
                        MinutoInicio.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraInicio"), String).Substring(3, 2)
                        HoraFin.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraFin"), String).Substring(0, 2)
                        MinutoFin.SelectedValue = CType(ds.Tables(0).Rows(0)("HoraFin"), String).Substring(3, 2)
                    End If
                Else

                End If
            End With

            If LoadPromoRatesPlan() Then

            End If
            If LoadPromoRooms() Then

            End If
            Dim strError As String = String.Empty
        End If

    End Sub

    Public Sub loadRule(ByVal rule As Integer)
        Dim ds As New RatesPlanRulesData
        With New RatesPlanRulesFacade
            ds = .getById(rule)
        End With
        Me.txtCancellationPolicy.Style.Add("display", "none")
        Me.ddlHour.Style.Add("display", "none")
        Me.lblSep.Style.Add("display", "none")
        Me.ddlMinutes.Style.Add("display", "none")

        Me.lblAux.Style.Add("display", "none")
        lblEDaysHour.Style.Add("display", "none")

        If ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows.Count > 0 Then
            Me.IdRule = rule
            txtCancellationPolicy.Text = ""
            Dim timeObject As String = "{type:"
            With ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows(0)


                If Not .IsNull(RatesPlanRulesData.FIELD_CancelDays) Then
                    Me.txtCancellationPolicy.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_CancelDays)))
                    If CInt(Val(.Item(RatesPlanRulesData.FIELD_CancelDays))) = 0 Then
                        chkNonCancelable.Checked = True
                    End If
                    ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays

                    timeObject += Me.ddlCancelationPolicy.SelectedIndex.ToString() + ", value:'" + Me.txtCancellationPolicy.Text + "'}"

                    Me.lblAux.Text = ""
                    lblEDaysHour.Text = PortalCulture.GetString("00410")
                    Me.txtCancellationPolicy.Style.Add("display", "")
                    Me.lblAux.Style.Add("display", "")
                    lblEDaysHour.Style.Add("display", "")
                ElseIf Not .IsNull(RatesPlanRulesData.FIELD_CancelHours) Then
                    txtCancellationPolicy.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_CancelHours).ToString))
                    ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour

                    timeObject += Me.ddlCancelationPolicy.SelectedIndex.ToString() + ", value:'" + Me.txtCancellationPolicy.Text + "'}"

                    Me.lblAux.Text = ""
                    lblEDaysHour.Text = PortalCulture.GetString("00409")
                    Me.txtCancellationPolicy.Style.Add("display", "")
                    Me.lblAux.Style.Add("display", "")
                    lblEDaysHour.Style.Add("display", "")
                ElseIf Not .IsNull(RatesPlanRulesData.FIELD_CancelSpecificHour) Then
                    ddlCancelationPolicy.SelectedIndex = cancelpolicy.specifichour
                    Me.lblAux.Text = PortalCulture.GetString("00412")
                    lblEDaysHour.Text = PortalCulture.GetString("00411")
                    Me.txtCancellationPolicy.Style.Add("display", "none")
                    Me.ddlHour.Style.Add("display", "")
                    Me.ddlMinutes.Style.Add("display", "")
                    Me.lblSep.Style.Add("display", "")
                    Me.lblAux.Style.Add("display", "")
                    lblEDaysHour.Style.Add("display", "")
                    Try
                        Dim hora As String = .Item(RatesPlanRulesData.FIELD_CancelSpecificHour).ToString
                        Me.ddlHour.SelectedValue = hora.Substring(0, 2)
                        If CInt(hora.Substring(2, 2)) < 15 Then
                            Me.ddlMinutes.SelectedValue = "00"
                        ElseIf CInt(hora.Substring(2, 2)) < 30 Then
                            Me.ddlMinutes.SelectedValue = "15"
                        ElseIf CInt(hora.Substring(2, 2)) < 45 Then
                            Me.ddlMinutes.SelectedValue = "30"
                        Else
                            Me.ddlMinutes.SelectedValue = "45"
                        End If
                    Catch ex As Exception
                        Me.ddlHour.SelectedValue = "01"
                        Me.ddlMinutes.SelectedValue = "00"
                    End Try
                    timeObject += Me.ddlCancelationPolicy.SelectedIndex.ToString() + ", value:'" + Me.ddlHour.SelectedValue + ":" + Me.ddlMinutes.SelectedValue + "'}"
                Else
                    timeObject += "0,value:''}"
                End If

                Me.varCancelationTime.Value = timeObject

                Dim cancelpolities As String
                If Not .IsNull(RatesPlanRulesData.FIELD_PoliticaCancelacion) Then
                    cancelpolities = .Item(RatesPlanRulesData.FIELD_PoliticaCancelacion).ToString
                Else
                    cancelpolities = ""
                End If

                txtCancelPoliciesPreview.textodefault = cancelpolities

                If Not .IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) Then
                    txtCancelPoliciesPreview.CargaDatos(.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview))

                    idDiccPCReview = .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview)
                Else
                    idDiccPCReview = 0
                End If

                If Not .IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) Then
                    txtCancelPoliciesFull.CargaDatos(.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull))
                    idDiccPCFull = .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull)
                Else
                    idDiccPCFull = 0
                End If
                Me.txtCancelPolicyDescription.Text = txtPromoName.GetES '.Item(RatesPlanRulesData.FIELD_DESCRIPTION)
                If Not .IsNull(RatesPlanRulesData.FIELD_RateRulesDef) Then
                    Me.txtCancelPoliciesFull.RequiredText = True
                    Me.txtCancelPoliciesPreview.RequiredText = True
                End If

                txtMinNights.Text = ""
                txtMaxNights.Text = ""
                txtMinAdvBooking.Text = ""
                txtMaxAdvBooking.Text = ""
                If Not .IsNull(RatesPlanRulesData.FIELD_MINDIAS) Then
                    txtMinNights.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_MINDIAS).ToString))
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_MAXDIAS) Then
                    txtMaxNights.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_MAXDIAS).ToString))
                End If

                If Not .IsNull(RatesPlanRulesData.FIELD_ADVBOOKING) Then
                    txtMinAdvBooking.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_ADVBOOKING).ToString))
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_MAXADVBOOKING) Then
                    txtMaxAdvBooking.Text = CInt(Val(.Item(RatesPlanRulesData.FIELD_MAXADVBOOKING).ToString))
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_NOARRIVOS) Then
                    getNoArrrivalsField(.Item(RatesPlanRulesData.FIELD_NOARRIVOS).ToString)
                    If Not .Item(RatesPlanRulesData.FIELD_NOARRIVOS).ToString = "NNNNNNN" Then
                        open_SpecificArrivals.Checked = True
                    End If
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_PromoSpecificDays) Then
                    LoadSpecificDaysField(.Item(RatesPlanRulesData.FIELD_PromoSpecificDays).ToString)
                    If Not .Item(RatesPlanRulesData.FIELD_PromoSpecificDays).ToString = "YYYYYYY" Then
                        open_SpecificDate.Checked = True
                    End If
                End If
                If Not .IsNull(RatesPlanRulesData.FIELD_PromoStartDate) Then
                    travelWindowFrom.Text = CType(.Item(RatesPlanRulesData.FIELD_PromoStartDate), Date).ToString("dd/MM/yyyy") '.Item(RatesPlanRulesData.FIELD_PromoStartDate).ToString
                    travelWindowTo.Text = CType(.Item(RatesPlanRulesData.FIELD_PromoEndDate), Date).ToString("dd/MM/yyyy")
                End If
            End With
        End If
    End Sub



    Private Sub LoadGridRatePlans(ByVal sFiltro As String)
        Dim ds As RatePlanData
        Dim idAsoc As Integer = GetIdAsociation()
        Dim dv As DataView

        With New RatePlanFacade
            If MyBase.IsSupervisor Or MyBase.IsUsuarioHotelAssociation Then
                'Mostramos todos los rateplans incluidos los de tarifas netas.
                ds = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue), getPromos:=True)
            Else
                'Mostramos solamente los ratesplans que no sean de tarifas netas.
                ds = .GetRatePlanByIdHotel(Me.m_iHotelId, PortalCulture.GetIDCulture, 1, 0, idAsociacion:=idAsoc, DeleteFilter:=Integer.Parse(ddlDeletedFilter.SelectedValue), getPromos:=True)
            End If

        End With

        Dim dr As DataRow
        For Each dr In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            If dr(RatePlanData.FIELD_SEGMENT) = "I" Then
                dr.Delete()
            End If
        Next

        ds.AcceptChanges()

        With grid
            'FechaInicio -> BookingWindow
            'FechaFin -> BookingWindow
            'PromoStartDate -> TravelWindow
            'PromoEndDate -> TravelWindow

            '" and FechaFin IS NOT NULL and FechaFin >= " & dateFilter & " Or PromoEndDate >= " & dateFilter
            '" and PromoEndDate >= " & dateFilter

            Dim includeOldPromos As Boolean = oldPromosCheckbox.Checked
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = FilterPromos(includeOldPromos, sFiltro)
            'dsegmentos se utilizará en el databound
            dsegmentos = New DataSet
            dsegmentos.ReadXml(Server.MapPath(Request.ApplicationPath & "/Data/Segmentos.xml"))
            .DataKeyField = RatePlanData.FIELD_IDRATEPLAN
            '.DataSource = ds
            .DataSource = dv
            .DataBind()
        End With

        If ds.Tables(0).Rows.Count > 0 Then
            dsRatesPlan = New DataSet
            dsRatesPlan.Merge(ds)

        Else
        End If
    End Sub

    Private Sub dgRatePlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand
        Cerror = 0
        If e.CommandName = "Select" Then
            Me.ClearData()
            Me.edicion = True
            Me.IdRatePlan = grid.DataKeys(e.Item.ItemIndex)
            If e.Item.Cells(dgcolumns.principalSegmentRac).Text.ToUpper = "True" Then
                Me.loadPromo(grid.DataKeys(e.Item.ItemIndex), True)
            Else
                'Response.Redirect("~/rate-manager-ui/dist/promotions-details.aspx?code=" & IdRatePlan)
                Me.loadPromo(grid.DataKeys(e.Item.ItemIndex), False)
            End If
            Me.btnSave.Enabled = True

            MostrarCmdNew(False)
        ElseIf e.CommandName = "Delete" Then
            Cerror = 0


            With New RatePlanFacade
                If .LogicDeleteRatePlan(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) Then

                    Me.guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Eliminar, "Eliminó el rateplan con el id " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.idrateplan).Text & " y el codigo de tarifa " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.codigotarifa).Text)
                    If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
                        grid.CurrentPageIndex = ((grid.CurrentPageIndex * grid.PageSize) \ grid.PageSize) - 1
                    End If
                    LoadGridRatePlans(ctrlAutoComplete1.GetFilter)
                    Me.grid.SelectedIndex = -1
                    ClearData()
                    MostrarCmdNew(True)
                Else
                    Cerror = 6
                End If
            End With
        ElseIf e.CommandName = "Active" Then
            With New RatePlanFacade
                If .LogicActiveRatePlan(Me.cInfoActual.Hotel, grid.DataKeys(e.Item.ItemIndex)) Then
                    ClearData()
                    LoadGridRatePlans(ctrlAutoComplete1.GetFilter)
                    Me.grid.SelectedIndex = -1
                    
                    MostrarCmdNew(True)
                End If
            End With
        End If
    End Sub

    Private Sub dgRatePlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            If e.Item.Cells(dgcolumns.orden).Text = "0" Or e.Item.Cells(dgcolumns.orden).Text = "100000" Then
                e.Item.Cells(dgcolumns.orden).Text = "---"
            End If

            Dim startDate, endDate As Date

            e.Item.Cells(dgcolumns.StartDate).Text = IIf(Date.TryParse(e.Item.Cells(dgcolumns.StartDate).Text, startDate), startDate.ToString("dd/MM/yyyy"), "")
            e.Item.Cells(dgcolumns.EndDate).Text = IIf(Date.TryParse(e.Item.Cells(dgcolumns.EndDate).Text, endDate), endDate.ToString("dd/MM/yyyy"), "")
            e.Item.Cells(dgcolumns.segment).Text = GetSegmento(e.Item.Cells(dgcolumns.segment).Text)
            Dim LK As HyperLink
            Dim LK2 As LinkButton

            LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkedit")
            LK2.Text = PortalCulture.GetString("00093")

            If e.Item.Cells(dgcolumns.deleted).Text.ToUpper() = "TRUE" Then 'ACTIVAR
                e.Item.Cells(dgcolumns.eliminar).Text = ""
                e.Item.Cells(dgcolumns.orden).Text = ""

                LK2 = e.Item.Cells(dgcolumns.activar).FindControl("lnkActivar2")
                LK = e.Item.Cells(dgcolumns.activar).FindControl("lnkActivar")
                LK.Text = PortalCulture.GetString("01520")
                LK.Attributes.Add("onClick", "javascript:openModal('" & LK2.ClientID & "', '" & PortalCulture.GetString("01650") & "','" & PortalCulture.GetString("01652") & "')")
                If ddlDeletedFilter.SelectedValue = "-1" Then
                    e.Item.Style("background-color") = "#FEE"
                End If
            Else 'DESACTIVAR
                e.Item.Cells(dgcolumns.activar).Text = ""
                LK2 = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar2")
                LK = e.Item.Cells(dgcolumns.eliminar).FindControl("lnkEliminar")
                LK.Text = PortalCulture.GetString("01521")
                LK.Attributes.Add("onClick", "javascript:openModal('" & LK2.ClientID & "', '" & PortalCulture.GetString("01649") & "','" & PortalCulture.GetString("01651") & "')")
                
                If e.Item.Cells(dgcolumns.principalSegmentRac).Text.ToUpper = "TRUE" Then
                    LK.Enabled = False
                End If
            End If

        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.segment).Text = PortalCulture.GetString("00003")
            e.Item.Cells(dgcolumns.code).Text = PortalCulture.GetString("00001")
            e.Item.Cells(dgcolumns.orden).Text = PortalCulture.GetString("00920")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.eliminar).Text = CType(grid.DataSource, DataView).Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Private Function GetSegmento(ByVal Cod As String) As String
        Dim dv As DataView
        dv = dsegmentos.Tables("Segmento").DefaultView
        dv.RowFilter = "Code ='" & Cod & "'"
        If dv.Count > 0 Then
            Return dv(0)("Desc")
        Else
            Return Cod
        End If

    End Function

    Function getDataXML() As String
        Dim ds As New RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(Me.m_iHotelId, idAsociacion:=idAsoc)
        End With
        Return Util.Utility.GetXml(ds.RATEPLAN_TABLE, "UpdateRatesPlan", ds)
    End Function

    Public Sub ClearData()
        chkIscombinable.Checked = False
        txtPromotionCode.Enabled = True
        txtDiasBlackout.Value = ""
        grid.SelectedIndex = -1
        txtPromoDescription.SetEN("")
        txtPromoDescription.setES("")
        txtAddValueDescription.SetEN("")
        txtAddValueDescription.setES("")
        txtCancelPoliciesFull.setES("")
        txtCancelPoliciesFull.SetEN("")
        txtCancelPoliciesPreview.SetEN("")
        txtCancelPoliciesPreview.setES("")
        txtPromoName.SetEN("")
        txtPromoName.setES("")
        txtCancellationPolicy.Text = ""
        txtPromotionCode.Text = ""
        For Each Item As ListItem In chkListRoom.Items
            Item.Selected = False
        Next
        For Each item As ListItem In chlListContract.Items
            item.Selected = False
        Next
        For Each item As ListItem In chklSpecificArrivals.Items
            item.Selected = False
        Next
        For Each item As ListItem In ckhlSpecificDay.Items
            item.Selected = True
        Next
        travelWindowFrom.Text = ""
        travelWindowTo.Text = ""
        bookingWindowFrom.Text = ""
        bookingWindowTo.Text = ""
        txtMaxAdvBooking.Text = ""
        txtMinAdvBooking.Text = ""
        txtMaxNights.Text = ""
        txtMinNights.Text = ""
        blackoutFrom.Text = ""
        blackoutTo.Text = ""
        txtCancelPolicyDescription.Text = ""
        open_blackout.Checked = False
        open_SpecificArrivals.Checked = False
        open_SpecificDate.Checked = False
        CheckBoxDefHora.Checked = False
        HoraFin.Enabled = False
        HoraInicio.Enabled = False
        MinutoFin.Enabled = False
        MinutoInicio.Enabled = False
        chkPromoAdd.Checked = False
        chkPromoDiscount.Checked = False
        chkPromoNights.Checked = False
        txtCancellationPolicy.Text = ""
        ddlCancelationPolicy.SelectedIndex = 0
        ddlApplicationMode.SelectedIndex = 0
        chkNonCancelable.Checked = False
        HoraFin.SelectedIndex = 0
        HoraInicio.SelectedIndex = 0
        MinutoFin.SelectedIndex = 0
        MinutoInicio.SelectedIndex = 0
        txtPromoDiscount.Text = ""
        edicion = False

        idDiccPCFull = 0
        idDiccPCReview = 0
        idDicc = 0
    End Sub

    Public Function SaveRules(ByVal publish As Boolean, ByRef IdRule As Integer) As Boolean
        Dim ds As New RatesPlanRulesData
        Dim dr As DataRow
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        sDataPrev = getDataXML()
        dr = ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).NewRow

        Try

            With dr
                .Item(RatesPlanRulesData.FIELD_CancelDays) = System.DBNull.Value
                .Item(RatesPlanRulesData.FIELD_CancelHours) = System.DBNull.Value
                .Item(RatesPlanRulesData.FIELD_CancelSpecificHour) = System.DBNull.Value
                If chkNonCancelable.Checked Then
                    .Item(RatesPlanRulesData.FIELD_CancelDays) = 0
                Else
                    If ddlCancelationPolicy.SelectedIndex <> cancelpolicy.useDefault Then
                        If Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays AndAlso txtCancellationPolicy.Text <> "" Then
                            .Item(RatesPlanRulesData.FIELD_CancelDays) = CInt(Val(Me.txtCancellationPolicy.Text))
                        ElseIf Me.ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour AndAlso txtCancellationPolicy.Text <> "" Then
                            .Item(RatesPlanRulesData.FIELD_CancelHours) = CInt(Val(txtCancellationPolicy.Text))
                        Else
                            .Item(RatesPlanRulesData.FIELD_CancelSpecificHour) = Me.ddlHour.SelectedValue.ToString & Me.ddlMinutes.SelectedValue.ToString  'Me.txtCancellationPolicy.Text
                        End If
                    End If
                End If

                .Item(RatesPlanRulesData.FIELD_DepNight) = System.DBNull.Value
                .Item(RatesPlanRulesData.FIELD_DepPerc) = System.DBNull.Value
                .Item(RatesPlanRulesData.FIELD_IDHOTEL) = m_iHotelId
                .Item(RatesPlanRulesData.FIELD_GuarDep) = "N" 'ddlGuarDep.SelectedValue
                .Item(RatesPlanRulesData.FIELD_IDRULE) = 0
                .Item(RatesPlanRulesData.FIELD_RateRulesDef) = True 'chkRateRules.Checked
                .Item(RatesPlanRulesData.FIELD_ReqVerif) = True 'ddlVerReq.SelectedValue
                .Item(RatesPlanRulesData.FIELD_DESCRIPTION) = txtCancelPolicyDescription.Text
                .Item(RatesPlanRulesData.FIELD_PromoStartDate) = Date.ParseExact(travelWindowFrom.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo)
                .Item(RatesPlanRulesData.FIELD_PromoEndDate) = Date.ParseExact(travelWindowTo.Text, "dd/MM/yyyy", Globalization.DateTimeFormatInfo.InvariantInfo)
                .Item(RatesPlanRulesData.FIELD_DESCRIPTION) = txtCancelPolicyDescription.Text


                If txtMinAdvBooking.Text <> "" Then
                    .Item(RatesPlanRulesData.FIELD_ADVBOOKING) = CInt(Val(txtMinAdvBooking.Text))
                End If
                If txtMaxAdvBooking.Text <> "" Then
                    .Item(RatesPlanRulesData.FIELD_MAXADVBOOKING) = CInt(Val(txtMaxAdvBooking.Text))
                End If
                If txtMaxNights.Text <> "" Then
                    .Item(RatesPlanRulesData.FIELD_MAXDIAS) = CInt(Val(txtMaxNights.Text))
                End If
                If txtMinNights.Text <> "" Then
                    .Item(RatesPlanRulesData.FIELD_MINDIAS) = CInt(Val(txtMinNights.Text))
                End If

                If open_SpecificArrivals.Checked AndAlso GetArrivosField() <> "NNNNNNN" Then
                    .Item(RatesPlanRulesData.FIELD_NOARRIVOS) = GetArrivosField()
                Else
                    .Item(RatesPlanRulesData.FIELD_NOARRIVOS) = "NNNNNNN"
                End If

                If open_SpecificDate.Checked AndAlso GetSpecificDays() <> "YYYYYYY" Then
                    .Item(RatesPlanRulesData.FIELD_PromoSpecificDays) = GetSpecificDays()
                Else
                    .Item(RatesPlanRulesData.FIELD_PromoSpecificDays) = "YYYYYYY"
                End If

                Dim CancelPolities As String
                CancelPolities = txtCancelPoliciesPreview.textodefault.PadRight(52)
                If txtCancelPoliciesFull.textodefault.Length > 184 Then
                    .Item(RatesPlanRulesData.FIELD_PoliticaCancelacion) = CancelPolities & txtCancelPoliciesFull.textodefault.Substring(0, 184)
                Else
                    .Item(RatesPlanRulesData.FIELD_PoliticaCancelacion) = CancelPolities & txtCancelPoliciesFull.textodefault
                End If

                If idDiccPCReview = 0 Then
                    If validaTexto(txtCancelPoliciesPreview.GetES, txtCancelPoliciesPreview.GetEN) Then
                        .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) = txtCancelPoliciesPreview.Insert()
                    End If
                Else
                    .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) = idDiccPCReview
                End If

                If idDiccPCFull = 0 Then
                    If validaTexto(txtCancelPoliciesFull.GetES, txtCancelPoliciesFull.GetEN) Then
                        .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) = txtCancelPoliciesFull.Insert()
                    End If
                Else
                    .Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) = idDiccPCFull
                End If
            End With
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message

            Return False
        End Try
        ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows.Add(dr)
        Dim sw As Boolean = False
        If edicion = False Then
            Try
                With New RatesPlanRulesFacade
                    sw = .Insert(ds)
                    If sw Then
                        IdRule = ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows(0).Item(RatesPlanRulesData.FIELD_IDRULE)
                        sDataPrev = ""
                        sData = Util.Utility.GetXml(ds.TABLE_RATEPLANRULES, "UpdateRatesPlanRules", ds)
                        CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansRules.aspx", PaginaBase.acciones.Crear, "Se creó la regla " & txtCancelPolicyDescription.Text, "", sDataPrev, sData)

                        If publish Then
                            CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansRules.aspx", PaginaBase.acciones.Publicar, "Se modificó la regla " & txtCancelPolicyDescription.Text, "", sDataPrev, sData)
                        End If

                        If Me.txtCancelPoliciesPreview.HasChanges OrElse Me.txtCancelPoliciesFull.HasChanges Then 'OrElse Me.txtGuaranteePolicy.HasChanges OrElse Me.txtPolicyCreditCard.HasChanges Then
                            CType(Me.Page, PaginaBase).NotifyContentModification("Regla de plan tarifario " & txtCancelPolicyDescription.Text, "Reglas De Plan Tarifario")
                        End If

                        If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) Then
                            txtCancelPoliciesPreview.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview), publish)
                        End If

                        If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) Then
                            txtCancelPoliciesFull.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull), publish)
                        End If
                    End If

                    Return sw
                End With
            Catch ex As Exception
                lblError.Visible = True
                lblError.Text = "Reglas => " + ex.Message

                Return sw
            End Try
        Else
            ds.AcceptChanges()
            ds.Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows(0).Item(RatesPlanRulesData.FIELD_IDRULE) = Me.IdRule
            Try
            With New RatesPlanRulesFacade
                sw = .Update(ds)
                If sw Then
                    sData = Util.Utility.GetXml(ds.TABLE_RATEPLANRULES, "UpdateRatesPlanRules", ds)
                    CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlansRules.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Se modificó la regla " & txtCancelPolicyDescription.Text, "", sDataPrev, sData)

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview) Then
                        txtCancelPoliciesPreview.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionReview), publish)
                    End If

                    If Not dr.IsNull(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull) Then
                        txtCancelPoliciesFull.Update(dr.Item(RatesPlanRulesData.FIELD_idDiccionarioPoliticaCancelacionFull), publish)
                    End If
                    If Me.txtCancelPoliciesPreview.HasChanges OrElse Me.txtCancelPoliciesFull.HasChanges Then 'OrElse Me.txtGuaranteePolicy.HasChanges OrElse Me.txtPolicyCreditCard.HasChanges Then
                        CType(Me.Page, PaginaBase).NotifyContentModification("Regla de plan tarifario " & txtCancelPolicyDescription.Text, "Reglas De Plan Tarifario")
                    End If

                End If

                Return sw
                End With
            Catch ex As Exception
                lblError.Visible = True
                lblError.Text = "Reglas => " + ex.Message

                Return sw
            End Try
        End If
    End Function

    Private Sub getNoArrrivalsField(ByVal Field As String)
        If Field <> "" Then
            Dim i As Integer = 1
            For Each item As ListItem In chklSpecificArrivals.Items
                item.Selected = False
                Try
                    If Field.Substring(i - 1, 1) = "Y" Then
                        item.Selected = True
                    End If
                Catch ex As Exception
                End Try
                i += 1
            Next
        End If
    End Sub

    Private Sub LoadSpecificDaysField(ByVal Field As String)
        If Field <> "" Then
            Dim i As Integer = 1
            For Each item As ListItem In ckhlSpecificDay.Items
                item.Selected = False
                Try
                    If Field.Substring(i - 1, 1) = "Y" Then
                        item.Selected = True
                    End If
                Catch ex As Exception
                End Try
                i += 1
            Next
        End If
    End Sub

    Private Function GetArrivosField() As String
        Dim NoArr As String = ""
        Dim cheked As String
        For Each item As ListItem In chklSpecificArrivals.Items
            cheked = "Y"
            If Not item.Selected Then cheked = "N"
            NoArr &= cheked
        Next
        Return NoArr
    End Function

    Private Function GetSpecificDays() As String
        Dim NoArr As String = ""
        Dim cheked As String
        For Each item As ListItem In ckhlSpecificDay.Items
            cheked = "Y"
            If Not item.Selected Then cheked = "N"
            NoArr &= cheked
        Next
        Return NoArr
    End Function

    Public Function validaTexto(ByVal bIdioma As Boolean, ByVal txtControlIdioma As CtrlIdioma, ByVal idDicc As Integer) As String
        Dim texto As String
        Dim auxControlIdioma As CtrlIdioma = New CtrlIdioma
        Dim auxTxtIng As String = ""
        Dim auxTxtEsp As String = ""

        auxControlIdioma.CargaDatosAuxiliares(idDicc, auxTxtIng, auxTxtEsp)

        If bIdioma = True Then 'valida Idioma Español
            If txtControlIdioma.GetES = Nothing Then
                texto = " "
            Else
                If txtControlIdioma.GetES.Trim = "" Then
                    texto = " "
                Else
                    texto = txtControlIdioma.GetES
                End If
            End If

            If texto = " " Then
                If auxTxtEsp = Nothing Then
                    texto = Nothing
                End If
            End If
        Else 'valida Idioma Ingles
            If txtControlIdioma.GetEN = Nothing Then
                texto = " "
            Else
                If txtControlIdioma.GetEN.Trim = "" Then
                    texto = " "
                Else
                    texto = txtControlIdioma.GetEN
                End If
            End If

            If texto = " " Then
                If auxTxtIng = Nothing Then
                    texto = Nothing
                End If
            End If
        End If

        Return texto
    End Function

    Public Function validaTexto(ByVal txtEsp As String, ByVal txtIng As String) As Boolean
        Dim value As Boolean = False

        If txtEsp <> Nothing Then
            If txtEsp.Trim <> "" Then
                value = True
            End If
        End If

        If value = False Then
            If txtIng <> Nothing Then
                If txtIng.Trim <> "" Then
                    value = True
                End If
            End If
        End If

        Return value
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckBoxDefHora.Attributes.Add("onClick", "javascript:DesabilitarHabilitarHora('" & CheckBoxDefHora.ClientID & "','" & HoraInicio.ClientID & "','" & MinutoInicio.ClientID & "','" & HoraFin.ClientID & "','" & MinutoFin.ClientID & "');")
        open_SpecificArrivals.Attributes.Add("onClick", "javascript:toggle(""exclusiveArrivals"")")
        open_SpecificDate.Attributes.Add("onClick", "javascript:toggle(""exclusiveDate"")")
        open_blackout.Attributes.Add("onClick", "javascript:toggle(""blackoutDays"")")
        chkNonCancelable.Attributes.Add("onclick", "javascript:hideCancelPolicies()")

        For Each item As ListItem In chkListRoom.Items
            item.Attributes.Add("onclick", "notAllRooms();")
        Next
        For Each item As ListItem In chlListContract.Items
            item.Attributes.Add("onclick", "notAllRatePlans();")
        Next

        If ddlCancelationPolicy.SelectedIndex = cancelpolicy.bydays Then
            Me.lblAux.Text = PortalCulture.GetString("00413")
            lblEDaysHour.Text = PortalCulture.GetString("00410")
        ElseIf ddlCancelationPolicy.SelectedIndex = cancelpolicy.byhour Then
            Me.lblAux.Text = PortalCulture.GetString("00413")
            lblEDaysHour.Text = PortalCulture.GetString("00409")
        Else
            Me.lblAux.Text = PortalCulture.GetString("00412")
            lblEDaysHour.Text = PortalCulture.GetString("00411")
        End If
    End Sub

    Private Sub dgRatePlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grid.PageIndexChanged
        Me.grid.CurrentPageIndex = e.NewPageIndex
        Me.grid.SelectedIndex = -1
        LoadGridRatePlans(ctrlAutoComplete1.GetFilter)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Private Sub dgPromo_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If grid.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If grid.CurrentPageIndex < grid.PageCount - 1 Then
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

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        grid.CurrentPageIndex = 0
        LoadGridRatePlans(descripcion)
    End Sub

    Protected Sub ddlDeletedFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDeletedFilter.SelectedIndexChanged
        LoadGridRatePlans(ctrlAutoComplete1.GetFilter)
        Me.grid.SelectedIndex = -1
        ClearData()
        'lblError.Visible = False
        'lblErrorSource.Visible = False
        MostrarCmdNew(True)
    End Sub

    Private Sub oldPromosCheckbox_PostBack(ByVal sender As Object, ByVal e As EventArgs) Handles oldPromosCheckbox.CheckedChanged
        Me.grid.CurrentPageIndex = 0
        LoadGridRatePlans(ctrlAutoComplete1.GetFilter)
    End Sub

    Private Function FilterPromos(ByVal includeOldPromos As Boolean, sFiltro As String) As String
        Dim filter As String
        Dim dateFilter As String = "#" & DateTime.Now.ToString() & "#"
        If (includeOldPromos) Then
            filter = "isPromo = 1 " & IIf(sFiltro = "", "", " and " & sFiltro)
        Else
            filter = "isPromo = 1 " & IIf(sFiltro = "", "", " and " & sFiltro) & "and ((FechaFin IS NOT NULL and FechaFin >= " & dateFilter & ") Or (FechaFin IS NULL and PromoEndDate >= " & dateFilter & "))"
        End If
        Return filter
    End Function

End Class