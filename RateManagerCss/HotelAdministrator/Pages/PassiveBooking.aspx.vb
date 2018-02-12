Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports WSHotelFacade
Imports WSHotelCommon
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml
Imports wshotelrules

Partial Class PassiveBooking
    Inherits PaginaBase
    Private Property impuesto() As Double
        Get
            Return viewstate("_tax")
        End Get
        Set(ByVal Value As Double)
            viewstate("_tax") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtPet0 As System.Web.UI.WebControls.TextBox
    Protected WithEvents tblRooms As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblAdultos As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblChild As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblPet1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblDatosviajero As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lbltitlePet As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Dropdownlist1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents H21 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblRate As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents CtlReservaMsj1 As CtlReservaMsj


    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object
    Private resSell As resHotelSell
    Private reQSell As reqHotelSell

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Function CargaPersonas()
        Dim cboAdults As New DropDownList
        Dim cboChilds As New DropDownList
        Dim x As Integer

        For i As Integer = 0 To Me.cmbRooms.Items.Count - 1

            cboAdults = CType(Me.FindControl("dlAduts" & (i + 1).ToString), DropDownList)
            cboChilds = CType(Me.FindControl("dlChild" & (i + 1).ToString), DropDownList)

            If ddlRoomtype.Items.Count > 0 Then
                ddlRoomtypePersonas.SelectedIndex = ddlRoomtype.SelectedIndex
                x = ddlRoomtypePersonas.SelectedItem.Text
            Else
                x = 4
            End If
            
            cboAdults.Items.Clear()
            For j As Integer = 1 To x
                cboAdults.Items.Add(j)
            Next
            If (x = 0) Then cboAdults.Items.Add("0")

            x = If(ddlRoomtype.Items.Count > 0, CType(ddlRoomtypePersonas.SelectedItem.Value, Integer), 4)
            cboChilds.Items.Clear()
            For j As Integer = 0 To x
                cboChilds.Items.Add(j)
            Next
            If (x = 0) Then cboChilds.Items.Add("0")
        Next

    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            cmbRooms.SelectedIndex = 0
            LoadData()
            clearData()
            CargaPersonas()
            lblError.visible = False
        End If
        'Me.Page.RegisterStartupScript("SelectRoomDefault", "<script>ChangeRooms('" & Me.cmbRooms.ClientID & "','divRoom');</script>")
        'Me.Page.RegisterStartupScript("SelectDefault", "<script>SelectRoom(1,'divRoom','DataRoom');</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectRoomDefault", "<script>ChangeRooms('" & Me.cmbRooms.ClientID & "','divRoom');</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectDefault", "<script>SelectRoom(1,'divRoom','DataRoom');</script>")
        cmbRooms.Attributes.Add("onchange", "javascript:ChangeRooms('" & Me.cmbRooms.ClientID & "','divRoom');")
        ddlRoomtype.Attributes.Add("onchange", "javascript:ChangeMaxRoomType();")

        Me.ResizefrmPrincipal()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
    End Sub
    Private Sub loadCulture()

        lblTitle.Text = PortalCulture.GetString("00618")
        lblECheckIn.InnerHtml = PortalCulture.GetString("M000078", True)
        lblECheckOut.InnerHtml = PortalCulture.GetString("M000079", True)
        'lblRateCode.innerhtml = PortalCulture.GetString("M000283", True)
        lblRooms.InnerHtml = PortalCulture.GetString("00619", True)
        lblRoomInfo.InnerHtml = PortalCulture.GetString("00620")
        For i As Integer = 1 To 9
            Dim dv As HtmlTableCell
            dv = Me.FindControl("divRoom" & i.ToString)
            dv.InnerHtml = " " & i.ToString
            Dim lbl As HtmlGenericControl
            lbl = Me.FindControl("lblAdultos" & i.ToString)
            lbl.InnerHtml = PortalCulture.GetString("00621", True)
            lbl = Me.FindControl("lbltitlePet" & i.ToString)
            lbl.InnerHtml = PortalCulture.GetString("00622", True)
            lbl = Me.FindControl("lblChild" & i.ToString)
            lbl.InnerHtml = PortalCulture.GetString("00623", True)
        Next
        lblMainTravData.InnerHtml = PortalCulture.GetString("00624")
        lblName.InnerHtml = PortalCulture.GetString("00432", True)
        lblLastName.InnerHtml = PortalCulture.GetString("00355", True)
        lblHomePhone.InnerHtml = PortalCulture.GetString("00625", True)
        lblWorkHome.InnerHtml = PortalCulture.GetString("00626", True)
        lblAddress.InnerHtml = PortalCulture.GetString("M000076", True)
        lblemail.InnerHtml = PortalCulture.GetString("M000054", True)
        lblCCData.InnerHtml = PortalCulture.GetString("M000503")
        lblCCType.InnerHtml = PortalCulture.GetString("00362", True)
        lblCCNumber.InnerHtml = PortalCulture.GetString("00627", True)
        lblCCCode.InnerHtml = PortalCulture.GetString("00628", True)
        llCCHolder.InnerHtml = PortalCulture.GetString("00629", True)
        lblCCExp.InnerHtml = PortalCulture.GetString("00630", True)
        RevCCNumber.ErrorMessage = PortalCulture.GetString("00358")
        ReCcY.ErrorMessage = PortalCulture.GetString("00360")
        REMail.ErrorMessage = PortalCulture.GetString("00356")
        RvCcM.ErrorMessage = PortalCulture.GetString("00361")
        ReCcM.ErrorMessage = PortalCulture.GetString("00361")
        lblRoomType.InnerHtml = PortalCulture.GetString("M000439", True)
        btnContinue.Value = PortalCulture.GetString("00008")
        lblAgencia.Text = PortalCulture.GetString("M000524", True)
        lblMainAgenciaData.InnerHtml = PortalCulture.GetString("00614")
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
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation
        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1, idAsociacion:=idAsoc)
        End With

        If MyBase.IdCorporativoUserChain = 4 AndAlso (MyBase.IsHotel Or MyBase.IsUsuarioHotel) Then
            ds = RatePlanFilter("C", ds)
        End If

        ddlRate.DataTextField = RatePlanData.FIELD_CODIGOTARIFA
        ddlRate.DataValueField = RatePlanData.FIELD_IDRATEPLAN
        ddlRate.DataSource = ds
        ddlRate.DataBind()
        With New RoomFacade
            Dim room As RoomsHotelData
            room = .getRooms(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture)
            room.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "SUBSTRING(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '-' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ", 1, 25)")
            ddlRoomtype.DataSource = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            ddlRoomtype.DataTextField = "texto"
            ddlRoomtype.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
            ddlRoomtype.DataBind()

            ddlRoomtypePersonas.DataSource = room.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
            ddlRoomtypePersonas.DataTextField = RoomsHotelData.FLD_NUMBER_MAXADULTS
            ddlRoomtypePersonas.DataValueField = RoomsHotelData.FLD_NUMBER_MAXCHILDREN
            ddlRoomtypePersonas.DataBind()
        End With

        Dim dTarjetas As New TarjetasHotelData
        dTarjetas = (New TarjetasHotelFacade).GetTarjetasByHotelId(Me.cInfoActual.Hotel)

        dTarjetas.Tables(TarjetasHotelData.Tabla_TarjetasHotel).Columns.Add("texto", System.Type.GetType("System.String"))
        For Each dr As DataRow In dTarjetas.Tables(TarjetasHotelData.Tabla_TarjetasHotel).Rows
            dr("texto") = (New Tarjetas).NameTarget(GetCardId(dr(TarjetasHotelData.TableFields.FLD_Code)))
        Next
        Me.ddlCC.DataTextField = "texto"
        Me.ddlCC.DataValueField = TarjetasHotelData.TableFields.FLD_Code
        ddlCC.DataSource = dTarjetas
        ddlCC.DataBind()
        ddlCC.Items.Insert(0, PortalCulture.GetString("M000272"))

    End Sub

    Private Function GetHOR() As Boolean
        Dim xdoc As New XmlDataDocument(New reqHotelRules)
        Dim dsreq As reqHotelRules = CType(xdoc.DataSet, reqHotelRules)
        Dim rsHor As resHotelRules = Nothing


        Dim drR As reqHotelRules.HotelRulesRow
        drR = dsreq.HotelRules.NewHotelRulesRow
        dsreq.HotelRules.AddHotelRulesRow(drR)

        '//HotelHeader
        Dim drhh As reqHotelRules.HotelHeaderRow
        drhh = dsreq.HotelHeader.NewHotelHeaderRow

        drhh.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
        drhh.CheckinDate = CDate(Me.txtCheckIn.Text).Year & CDate(Me.txtCheckIn.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckIn.Text).Day.ToString.PadLeft(2, "0")
        drhh.CheckoutDate = CDate(Me.txtCheckOut.Text).Year & CDate(Me.txtCheckOut.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckOut.Text).Day.ToString.PadLeft(2, "0")
        drhh.RateCode = Me.ddlRoomtype.SelectedItem.Text.Substring(0, ddlRoomtype.SelectedItem.Text.IndexOf(" - ")).Trim & Me.ddlRate.SelectedItem.Text
        drhh.Source = "POR"
        drhh.PropertyNumber = Me.cInfoActual.Hotel
        dsreq.HotelHeader.AddHotelHeaderRow(drhh)
        drhh.SetParentRow(drR)

        '//rooms
        Dim drhrs As reqHotelRules.HotelRoomsRow
        drhrs = dsreq.HotelRooms.NewHotelRoomsRow
        dsreq.HotelRooms.AddHotelRoomsRow(drhrs)
        drhrs.SetParentRow(drR)

        For i As Integer = 1 To cmbRooms.SelectedValue
            Dim rR As reqHotelRules.RoomRow
            rR = dsreq.Room.NewRoomRow
            Dim dd As DropDownList
            dd = Me.FindControl("dlAduts" & i.ToString)
            rR.Adults = dd.SelectedValue
            dd = Me.FindControl("dlChild" & i.ToString)
            rR.Children = dd.SelectedValue
            dsreq.Room.AddRoomRow(rR)
            rR.SetParentRow(drhrs)
        Next


        Try
            With New WSHotelFacade.clsFAHOR
                rsHor = .GetHotelRules(xdoc.DocumentElement)
            End With
        Catch ex As Exception

        End Try
        resSell = New resHotelSell

        Me.CtlReservaMsj1.SetRooms = Me.cmbRooms.SelectedValue
        'Me.Page.RegisterStartupScript("SelectCtrlRoomDefault", "<script>ChangeRooms('" & Me.cmbRooms.ClientID & "','CtrldivRoom');</script>")
        'Me.Page.RegisterStartupScript("SelectCtrlDefault", "<script>SelectRoom(1,'CtrldivRoom','CtrlDataRoom');</script>")

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectCtrlRoomDefault", "<script>ChangeRooms('" & Me.cmbRooms.ClientID & "','CtrldivRoom');</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectCtrlDefault", "<script>SelectRoom(1,'CtrldivRoom','CtrlDataRoom');</script>")
        CtlReservaMsj1.SetTotal = 0

        For i As Integer = 1 To 9
            CtlReservaMsj1.SetPrice(i, 0)
        Next

        If Not rsHor Is Nothing Then
            Me.GetResponse(rsHor)
            Me.GetRequest()

            If rsHor._Error.Count > 0 Then
                Select Case rsHor._Error(0).ErrorCode
                    Case ErrorType.MaxStay
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00633")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00633")))
                    Case ErrorType.MinStay
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00632")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00632")))
                    Case ErrorType.AdvBooking
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00645")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00645")))
                    Case ErrorType.NoArrival
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00646")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00646")))
                    Case ErrorType.Close
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00647")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00647")))
                    Case ErrorType.MaxPeople
                        ' Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00648")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00648")))
                    Case ErrorType.ExccedMaxNumberRooms
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00649")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00649")))
                    Case Else 'NoAvailability 'RateRestrictedSource 'RateNoFound
                        'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00644")))
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00644")))
                End Select
            Else

                If rsHor.Rate.Count = 0 Then
                    'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00644")))
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00644")))
                Else
                    'Dim drrates() As resHotelRules.RateRow
                    Dim drPrices() As resHotelRules.PriceRow
                    Dim total As Double
                    Dim room As Integer = 1
                    For Each drRoom As resHotelRules.RTRoomRow In rsHor.RTRoom
                        Dim subtotal As Double = 0
                        Dim dia As Integer = 0
                        total = 0
                        For Each dr As resHotelRules.RateRow In rsHor.Rate
                            subtotal = 0
                            dia += 1
                            drPrices = rsHor.Price.Select("SeassonID=" & dr.SeassonId & " and adults=" & drRoom.Adults & _
                                " and children = " & drRoom.Children)

                            If Not dr.Exc Is System.DBNull.Value AndAlso dr.Exc.ToString.ToUpper = "Y" Then
                                subtotal = CDbl(drPrices(0).RateAdultExc) + CDbl(drPrices(0).RateChildExc)
                            Else
                                subtotal = CDbl(drPrices(0).RateAdult) + CDbl(drPrices(0).RateChild)
                            End If

                            If drRoom.ExtraAdults > 0 Then
                                subtotal += CDbl(drPrices(0).ExtraRateAdult) * CDbl(drRoom.ExtraAdults)
                            End If

                            If drRoom.ExtraChildren > 0 Then
                                subtotal += CDbl(drPrices(0).ExtraRateChild) * (drRoom.ExtraChildren)
                            End If
                            If CInt(rsHor.Room(0).DaysFree) > 0 AndAlso dia Mod rsHor.Room(0).DaysFree = 0 Then
                                subtotal = 0
                            ElseIf CDbl(rsHor.Room(0).DescPromotion) > 0 Then
                                subtotal -= subtotal * rsHor.Room(0).DescPromotion / 100
                            End If
                            total += subtotal
                        Next
                        CtlReservaMsj1.SetPrice(room, total)
                        room += 1
                    Next
                    CtlReservaMsj1.SetMoneda = rsHor._Property(0).Money
                    CtlReservaMsj1.SetTotal = CDbl(rsHor._Property(0).TotalRate) + CDbl(rsHor._Property(0).TotalExtras) + CDbl(rsHor._Property(0).Taxes)
                    'Me.Page.RegisterStartupScript("showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00639")))
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "showAlert", CtlReservaMsj1.getShow(Me.btnSaveReserva.ClientID, PortalCulture.GetString("00631"), PortalCulture.GetString("00639")))
                End If

            End If
        Else
            Me.lblError.Visible = True
            Me.lblError.Text = PortalCulture.GetString("00662")
        End If
    End Function

    Private Sub btnSaveReserva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveReserva.Click
        If Me.Page.IsValid AndAlso IsValidData() Then

            Dim IdRes As String = "", _err As String = ""
            Dim IdReserva As Double = 0

            Dim xdocReq As New XmlDataDocument(New reqHotelSell)
            xdocReq.LoadXml(iReq.Value)
            reQSell = CType(xdocReq.DataSet, reqHotelSell)
            If iRes.Value <> "" Then
                Dim xdocSell As New XmlDataDocument(New resHotelSell)
                xdocSell.LoadXml(iRes.Value)
                resSell = CType(xdocSell.DataSet, resHotelSell)
            End If
            MatchResponse()
            If Save(PortalCulture.GetIDCulture, reQSell, resSell, resSell.Reservation(0).PlusTax, IdReserva, _err, impuesto) = 0 Then
                MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & IdReserva.ToString)
            Else
                Me.lblError.Visible = True
                Me.lblError.Text = PortalCulture.GetString("00663")
            End If
        End If

    End Sub

    Private Sub btnContinue_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.ServerClick
        If Me.Page.IsValid AndAlso IsValidData() Then
            GetHOR()
        End If
    End Sub
    Private Function IsValidData() As Boolean
        'fechas válidas        
        Dim d1, d2 As Date
        Try
            d1 = CDate(Me.txtCheckIn.Text)
            d2 = CDate(Me.txtCheckOut.Text)
        Catch ex As Exception
            Me.cvDates.ErrorMessage = PortalCulture.GetString("00634")
            cvDates.IsValid = False
            Return False
        End Try
        If d2 <= d1 Then
            Me.cvDates.ErrorMessage = PortalCulture.GetString("00664")
            cvDates.IsValid = False
            Return False
        End If
        If DateDiff(DateInterval.Day, CDate(Me.txtCheckIn.Text), CDate(Me.txtCheckOut.Text)) > 90 Then
            Me.cvDates.ErrorMessage = PortalCulture.GetString("00665")
            cvDates.IsValid = False
            Return False
        End If
        If ddlCC.SelectedIndex > 0 Then
            'validar tarjeta de credito
            Dim Tarjeta As Tarjetas = New Tarjetas(Me.txtCCNumber.Text.Trim, GetCardId(Me.ddlCC.SelectedValue))
            If Not Tarjeta.IsValid Then
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00635")
                Me.cvCC.IsValid = False
                Return False
            End If
            If Me.txtSecCode.Text.Trim = "" Then
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00636")
                Me.cvCC.IsValid = False
                Return False
            End If
            If Me.txtCCHolder.Text.Trim = "" Then
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00637")
                Me.cvCC.IsValid = False
                Return False
            End If
            Try
                d1 = New Date(txtCCYear.Text, Me.txtccMonth.Text, Date.DaysInMonth(txtCCYear.Text, Me.txtccMonth.Text))
                If Now.Date >= d1 Then
                    Me.cvCC.ErrorMessage = PortalCulture.GetString("00359")
                    Me.cvCC.IsValid = False
                    Return False
                End If
            Catch ex As Exception
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00359")
                Me.cvCC.IsValid = False
                Return False
            End Try
        End If

        Return True
    End Function
    Private Enum ErrorType
        NoError ''No error 000
        MaxStay '' Max Stay Restricted 001
        MinStay '' Min Stay Restricted 002
        AdvBooking '' Advanced Booking Restricted 003
        NoArrival '' No arrival Restricted 004
        Close '' Close  rate         005
        NoAvailability '' No Availability 006
        MaxPeople  '' Exceed Max People 007
        RateRestrictedSource '' 008 Rate no availability to Request Source 
        RateNoFound '' 009 Rate no Found in property
        NoTransaction '' 010 error al guaardar la reservacion
        InvalidCreditCard '011 ''invalid Credit Card
        ReservationNoFound '012'Reservation No Found
        CancelPrior '013' dead line poli..
        CreditCardNoAccept '014'Tarjeta de credito no aceptada
        InvalidCDNumber '015' Corporate disscount
        InvalidRollAwayAdult '016''invalid number RollAway adult
        InvalidRollAwayChild '017'' invalid number rollAway Child
        InvalidRollAwayCrib '018'' Invalid Number Rollaway Crib
        ExccedMaxNumberRooms '019'' Excced Maxime Number Rooms
        RequiredGuar '020'' Guarantee Required
        RequiredDep '021'' Deposit Required
    End Enum
    Private Function GetDate(ByVal strDate As String) As Date
        Return New Date(strDate.Substring(0, 4), strDate.Substring(4, 2), strDate.Substring(6, 2))
    End Function
    Private Function GetAdults(ByVal req As reqHotelSell) As Byte
        Dim ad As Byte
        Dim dr As reqHotelSell.RoomRow
        For Each dr In req.Room.Rows
            ad += CByte(dr.Adults)
        Next
        Return ad 'str & ","
    End Function
    Private Function GetChildren(ByVal req As reqHotelSell) As Byte
        Dim ch As Byte
        Dim dr As reqHotelSell.RoomRow
        For Each dr In req.Room.Rows
            ch += CByte(dr.Children)
        Next
        Return ch 'str & ","
    End Function
    Private Function GetCardId(ByVal code As String) As tTarjeta
        Select Case code
            Case "AX"
                Return tTarjeta.ccAmericanExpress
            Case "MC", "CA"
                Return tTarjeta.ccMasterCard
            Case "DC"
                Return tTarjeta.ccDinerClub
            Case "VI"
                Return tTarjeta.ccVisa
            Case "DS"
                Return tTarjeta.ccDiscover
            Case "JC"
                Return tTarjeta.ccJCB
            Case "CB"
                Return tTarjeta.ccCarteBlanche
            Case "AB"
                Return tTarjeta.ccAustralianBankCard
        End Select
    End Function




#Region "SaveReservation"

    Private function LeePortalAsociacion() As Integer 
        Dim portals As Portal.Catalogos.Common.Data.clsCommonPortales
        Dim idPortal As Integer = 0
       
        With New Portal.Catalogos.Facade.clsFacadePortales()
            portals = .GetPortalsByAsociation(Me.GetIdAsociation)
        End With
        If Not dsEmpty(portals) Then
            Dim dv As DataView
            dv = portals.Tables(0).DefaultView
            dv.RowFilter = "ispublic= 0"
            If dv.Count > 0 Then
                idPortal = dv(0)("idPortal")
            End If
        End If
        Return idPortal
        'Dim ds As New DataSet
        'Dim ConnectionString As String = AppSettings("HotelConnectionString")
        'Dim dsCommand As New SqlDataAdapter
        'Dim IdAsociation As Integer
        'Dim idPortal As Integer

        'If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
        'dsCommand.SelectCommand = New SqlCommand
        'With dsCommand
        '    Try
        '        With .SelectCommand
        '            .CommandType = CommandType.StoredProcedure
        '            .CommandText = "spReservationsByDeposit_GetByDates"
        '            .Connection = New SqlConnection(ConnectionString)
        '            If IdAsociation > 0 Then
        '                .Parameters.Add(New SqlParameter("@NoReservation", SqlDbType.Int)).Value = IdAsociation
        '            End If
        '        End With
        '        .Fill(ds)
        '    Catch ex As Exception
        '        Dim s As String = ex.Message.ToString
        '        ds = Nothing 'TODO VS2008
        '    Finally
        '        If Not .SelectCommand Is Nothing Then
        '            If Not .SelectCommand.Connection Is Nothing Then
        '                .SelectCommand.Connection.Dispose()
        '            End If
        '            .SelectCommand.Dispose()
        '        End If
        '        .Dispose()
        '    End Try
        'End With
        'If Not dsEmpty(ds) Then
        '    idPortal = ds.Tables(0).Rows(0)("")
        'End If
    End Function

    Public Function Save(ByVal idioma As String, ByVal Req As reqHotelSell, ByRef res As resHotelSell, ByVal ptax As Boolean, ByRef idreservacion As Long, ByRef Caderror As String, ByVal pctTax As Double) As Byte
        Dim errorpago As Boolean = False
        Dim ckin, ckout As Date
        Dim trans As Boolean = False
        Dim sqlconn As New SqlConnection(AppSettings("HotelConnection"))
        Dim transacc As SqlTransaction
        Dim idRes As Long
        Dim strdate As String
        Dim isGal As Boolean = False
        Dim idPortal As Integer

        Try
            strdate = Req.Reservation(0).CheckInDate
            ckin = New Date(strdate.Substring(0, 4), strdate.Substring(4, 2), strdate.Substring(6, 2))
            strdate = Req.Reservation(0).CheckOutDate
            ckout = New Date(strdate.Substring(0, 4), strdate.Substring(4, 2), strdate.Substring(6, 2))

            sqlconn.Open()
            transacc = sqlconn.BeginTransaction
            trans = True
            Dim sqlcmd As New SqlCommand("spReservationInsert", sqlconn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Transaction = transacc
            ''''''reservationnumber'''''''
            If Not Req.Customer.Rows(0).IsNull("UserId") AndAlso Req.Customer(0).UserId > 0 Then
                res.Reservation(0).ConfirmNumber = Format(Now(), "yy") & Format(Now(), "MM") & Format(Now(), "dd") & Format(Now(), "HH") & Format(Now(), "mm") & Format(Now(), "ss") & Req.Customer(0).UserId
            Else
                res.Reservation(0).ConfirmNumber = Format(Now(), "yy") & Format(Now(), "MM") & Format(Now(), "dd") & Format(Now(), "HH") & Format(Now(), "mm") & Format(Now(), "ss") & "0"
            End If
            'Reservacion
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Direction = ParameterDirection.Output
            If Req.Customer.Rows(0).IsNull("UserId") Then Req.Customer(0).UserId = 0
            '''''''''''
            sqlcmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = Req.Customer(0).UserId
            'If res.idHotel > 0 Then
            If (Req.Reservation(0).IsRecLocNull OrElse Req.Reservation(0).RecLoc = "") AndAlso (Req.Reservation(0).IsConfirmNumberNull OrElse Req.Reservation(0).ConfirmNumber = "") Then
                If res.Reservation(0).PropertyNumber > 0 Then sqlcmd.Parameters.Add("@idHotel", SqlDbType.Int).Value = res.Reservation(0).PropertyNumber
            Else
                isGal = True
                sqlcmd.Parameters.Add("@idHotel", SqlDbType.Int).Value = 0

                If Not Req.Reservation(0).IsHotelNameNull AndAlso Req.Reservation(0).HotelName <> "" Then sqlcmd.Parameters.Add("@HotelGNombre", SqlDbType.NVarChar, 50).Value = Req.Reservation(0).HotelName
                If Not Req.Reservation(0).IsCityNameNull AndAlso Req.Reservation(0).CityName <> "" Then sqlcmd.Parameters.Add("@HotelGCiudad", SqlDbType.NVarChar, 50).Value = Req.Reservation(0).CityName
                If Not Req.Reservation(0).IsCountryNameNull AndAlso Req.Reservation(0).CountryName <> "" Then sqlcmd.Parameters.Add("@HotelGPais", SqlDbType.NVarChar, 3).Value = Req.Reservation(0).CountryName
                sqlcmd.Parameters.Add("@idHotelGalileo", SqlDbType.NVarChar, 10).Value = res.Reservation(0).PropertyNumber
                If Not Req.Reservation(0).IsRecLocNull AndAlso Req.Reservation(0).RecLoc <> "" Then sqlcmd.Parameters.Add("@NoReservacionGalileo", SqlDbType.NVarChar, 50).Value = Req.Reservation(0).RecLoc
                If Not Req.Reservation(0).IsConfirmNumberNull AndAlso Req.Reservation(0).ConfirmNumber <> "" Then sqlcmd.Parameters.Add("@NoConfGalileo", SqlDbType.NVarChar, 50).Value = Req.Reservation(0).ConfirmNumber
            End If
            'nuevo para galileo
            If Not Req.HotelHeader(0).IsIdPortalNull AndAlso Req.HotelHeader(0).IdPortal <> "" AndAlso Req.HotelHeader(0).IdPortal > 0 Then sqlcmd.Parameters.Add("@IdPortal", SqlDbType.Int).Value = Req.HotelHeader(0).IdPortal
            If Me.GetIdAsociation > 0 Then
                idPortal = LeePortalAsociacion()
                If idPortal > 0 Then sqlcmd.Parameters.Add("@IdPortal", SqlDbType.Int).Value = idPortal
            End If

            ''''''' a ver si aplica '''''''''
            If res.Reservation(0).ChainCode <> "" Then sqlcmd.Parameters.Add("@ChainCode", SqlDbType.NVarChar, 2).Value = res.Reservation(0).ChainCode
            'If res.idHotelGalileo <> "" Then sqlcmd.Parameters.Add("@idHotelGalileo", SqlDbType.NVarChar, 10).Value = res.idHotelGalileo
            '--------------            
            If res.Reservation(0).ConfirmNumber <> "" Then sqlcmd.Parameters.Add("@NoReservacion", SqlDbType.NVarChar, 24).Value = res.Reservation(0).ConfirmNumber
            If DateDiff(DateInterval.Day, ckin, ckout) > 0 Then
                sqlcmd.Parameters.Add("@Checkin", SqlDbType.SmallDateTime).Value = ckin
                sqlcmd.Parameters.Add("@Checkout", SqlDbType.SmallDateTime).Value = ckout
            End If
            sqlcmd.Parameters.Add("@Impuesto", SqlDbType.Money).Value = pctTax 'res.Reservation(0).Taxes
            sqlcmd.Parameters.Add("@PlusTax", SqlDbType.Bit).Value = ptax
            If Not ptax Then
                sqlcmd.Parameters.Add("@Total", SqlDbType.Money).Value = res.Reservation(0).Total + res.Reservation(0).Taxes '  res.Total + (res.Total * res.Hotel.Tax / 100)
            Else
                sqlcmd.Parameters.Add("@Total", SqlDbType.Money).Value = res.Reservation(0).Total
            End If
            sqlcmd.Parameters.Add("@ReservationPorPlan", SqlDbType.Bit).Value = False
            If Not Req.Reservation(0).IsCreditCardTypeNull Then
                sqlcmd.Parameters.Add("@Numero_cc", SqlDbType.NText).Value = crypto.EncryptString128Bit(Req.Reservation(0).CreditCardNumber, crypto.PublicKey)
                sqlcmd.Parameters.Add("@Nombre_cc", SqlDbType.NVarChar, 80).Value = Req.Reservation(0).CreditCardHolder
                sqlcmd.Parameters.Add("@Tipo_cc", SqlDbType.TinyInt).Value = Req.Reservation(0).CreditCardType
                sqlcmd.Parameters.Add("@ExpiraMes_cc", SqlDbType.NVarChar, 2).Value = Req.Reservation(0).CreditCardExpiration.Substring(0, 2)
                sqlcmd.Parameters.Add("@ExpiraAnio_cc", SqlDbType.NVarChar, 4).Value = Req.Reservation(0).CreditCardExpiration.Substring(2)
                sqlcmd.Parameters.Add("@Digito_cc", SqlDbType.NVarChar, 4).Value = Req.Reservation(0).CreditCardNumberVerify
            End If
            'Guardamos el tipo de cambio
            'Datos del cliente
            If Req.Customer(0).UserId = 0 Then
                If Req.Customer(0).FirstName <> "" Then sqlcmd.Parameters.Add("@Nombre_cl", SqlDbType.NVarChar, 80).Value = Req.Customer(0).FirstName
                If Req.Customer(0).LastName <> "" Then sqlcmd.Parameters.Add("@Apellido_cl", SqlDbType.NVarChar, 80).Value = Req.Customer(0).LastName
                If Req.Customer(0).PhoneHome <> "" Then sqlcmd.Parameters.Add("@Telefono_cl", SqlDbType.NVarChar, 38).Value = Req.Customer(0).PhoneHome
                If Req.Customer(0).PhoneWork <> "" Then sqlcmd.Parameters.Add("@TelefonoTrabajo_cl", SqlDbType.NVarChar, 38).Value = Req.Customer(0).PhoneWork
                If Req.Customer(0).Email <> "" Then sqlcmd.Parameters.Add("@Email_cl", SqlDbType.NVarChar, 80).Value = Req.Customer(0).Email
                If Req.Customer(0).Address <> "" Then sqlcmd.Parameters.Add("@Direccion_cl", SqlDbType.NVarChar, 50).Value = Req.Customer(0).Address
            End If

            If Req.WizcomData.Rows.Count > 0 Then
                sqlcmd.Parameters.Add("@TxCode", SqlDbType.NVarChar, 2).Value = Req.WizcomData(0).ACTransaction
                If Req.WizcomData(0).TypeMessage = "A" Then
                    sqlcmd.Parameters.Add("@StatusConf", SqlDbType.Bit).Value = False
                Else
                    sqlcmd.Parameters.Add("@StatusConf", SqlDbType.Bit).Value = True
                End If
                If Req.WizcomData(0).SystemCode <> "" Then sqlcmd.Parameters.Add("@SystemCode", SqlDbType.NVarChar, 2).Value = Req.WizcomData(0).SystemCode
                If Req.WizcomData(0).TravelAgencyName <> "" Then sqlcmd.Parameters.Add("@TravelAgencyName", SqlDbType.NVarChar, 30).Value = Req.WizcomData(0).TravelAgencyName
                If Req.WizcomData(0).Voucher <> "" Then sqlcmd.Parameters.Add("@Voucher", SqlDbType.NVarChar, 15).Value = Req.WizcomData(0).Voucher
                If Req.WizcomData(0).WizcomPassOn <> "" Then sqlcmd.Parameters.Add("@WizcomPassOn", SqlDbType.NVarChar, 30).Value = Req.WizcomData(0).WizcomPassOn
                If Req.WizcomData(0).WizcomSequenceNumber <> "" Then sqlcmd.Parameters.Add("@WizcomSequenceNumber", SqlDbType.NVarChar, 7).Value = Req.WizcomData(0).WizcomSequenceNumber

                ''''''''''''''' comando para reservar ''''''''''''''''
                If Req.WizcomData(0).BookingSource <> "" Then sqlcmd.Parameters.Add("@BookingSource", SqlDbType.NVarChar, 100).Value = Req.WizcomData(0).BookingSource
                If Req.WizcomData(0).RecordLocator <> "" Then sqlcmd.Parameters.Add("@RecordLocator", SqlDbType.NVarChar, 8).Value = Req.WizcomData(0).RecordLocator
                If Req.WizcomData(0).RFCode <> "" Then sqlcmd.Parameters.Add("@RFCode", SqlDbType.NVarChar, 20).Value = Req.WizcomData(0).RFCode
            End If

            ''''''''''''para los rollaway ''''''''''''''
            'Dim _AdultRoll As Byte
            'Dim _ChildRoll As Byte
            'Dim _CribRoll As Byte
            'Dim _PriceChildRoll As Single
            'Dim _PriceCribRoll As Single
            'Dim _PriceAdultRoll As Single

            sqlcmd.Parameters.Add("@AdultRoll", SqlDbType.TinyInt).Value = Req.Reservation(0).RAwayAdult
            sqlcmd.Parameters.Add("@ChildRoll", SqlDbType.TinyInt).Value = Req.Reservation(0).RAwayChild
            sqlcmd.Parameters.Add("@CribRoll", SqlDbType.TinyInt).Value = Req.Reservation(0).RAwayCrib
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Not res.Reservation(0).IsPriceRAwayAdultNull Then
                sqlcmd.Parameters.Add("@PriceAdultRoll", SqlDbType.SmallMoney).Value = res.Reservation(0).PriceRAwayAdult
            End If
            If Not res.Reservation(0).IsPriceRAwayCribNull Then
                sqlcmd.Parameters.Add("@PriceCribRoll", SqlDbType.SmallMoney).Value = res.Reservation(0).PriceRAwayCrib
            End If
            If Not res.Reservation(0).IsPriceRAwayChildNull Then
                sqlcmd.Parameters.Add("@PriceChildRoll", SqlDbType.SmallMoney).Value = res.Reservation(0).PriceRAwayChild
            End If
            '''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''' 'res.ChainCode   '''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If res.Reservation(0).GuarDep <> "" Then sqlcmd.Parameters.Add("@GuarDep", SqlDbType.Char, 1).Value = res.Reservation(0).GuarDep

            sqlcmd.Parameters.Add("@RatePlan", SqlDbType.NVarChar, 4).Value = Req.Reservation(0).RateCode.Substring(3)
            sqlcmd.Parameters.Add("@Source", SqlDbType.NVarChar, 4).Value = Req.HotelHeader(0).Source
            If Not String.IsNullOrEmpty(txtAgencia.Text) Then
                sqlcmd.Parameters.Add("@Agency", SqlDbType.NVarChar, 80).Value = txtAgencia.Text
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''' executar comando '''''

            sqlcmd.ExecuteNonQuery()
            idRes = sqlcmd.Parameters("@idReservacion").Value
            idreservacion = idRes
            'If res.idReservation = 0 Then idRes = sqlcmd.Parameters("@idReservacion").Value
            '''' Detalle ''''
            Dim x As Long
            Dim idDet As Long

            sqlcmd.Parameters.Clear()
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idRes
            sqlcmd.Parameters.Clear()
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idRes

            For x = 0 To res.Room.Count - 1

                sqlcmd.CommandText = "spReservationInsertDetails"

                sqlcmd.Parameters.Clear()
                sqlcmd.Parameters.Add("@idDetalleReservacion", SqlDbType.Int).Direction = ParameterDirection.Output
                sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idRes
                'Nuevo galileo
                sqlcmd.Parameters.Add("@idHabitacionGalileo", SqlDbType.NVarChar, 10).Value = DBNull.Value

                sqlcmd.Parameters.Add("@idTipoHabitacion_Hotel", SqlDbType.Int).Value = res.Room(x).HotelRoomId

                res.Room(x).SetHotelRoomIdNull()
                sqlcmd.Parameters.Add("@idTipoPlan", SqlDbType.Int).Value = 0
                If Req.Customer(0).UserId = 0 Then
                    sqlcmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = 0
                Else
                    sqlcmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = Req.Room(x).idtraveler
                End If
                sqlcmd.Parameters.Add("@Adultos", SqlDbType.TinyInt).Value = res.Room(x).Adults
                sqlcmd.Parameters.Add("@pextras", SqlDbType.TinyInt).Value = res.Room(x).ExtraAdults + res.Room(x).ExtraChildren   'Res.Rooms(x).NumberAdults

                sqlcmd.Parameters.Add("@AdultosExtras", SqlDbType.TinyInt).Value = res.Room(x).ExtraAdults
                sqlcmd.Parameters.Add("@NiniosExtras", SqlDbType.TinyInt).Value = res.Room(x).ExtraChildren

                sqlcmd.Parameters.Add("@Ninios", SqlDbType.TinyInt).Value = res.Room(x).Children
                sqlcmd.Parameters.Add("@EdadesNinios", SqlDbType.NVarChar, 255).Value = ""
                sqlcmd.Parameters.Add("@Preferencia", SqlDbType.NVarChar, 255).Value = res.Room(x).Preferences
                If isGal Then
                    If Not Req.Room(x).IsNameRoomNull Then
                        sqlcmd.Parameters.Add("@NameRoom", SqlDbType.NVarChar, 255).Value = Req.Room(x).NameRoom
                    End If
                End If


                sqlcmd.ExecuteNonQuery()

                idDet = sqlcmd.Parameters("@idDetalleReservacion").Value
                Dim y As Long
                Dim chindate As Date = New Date
                Dim choutdate As Date = New Date
                Dim pric As Double = 0, pricE As Double = 0
                Dim seas As Integer
                Dim nights As Integer
                'Dim ratecode As String
                Dim drs() As resHotelSell.RateRow, dr As resHotelSell.RateRow
                Dim dr2 As resHotelSell.RateRow
                Dim cnt As Integer = 0
                nights = DateDiff(DateInterval.Day, ckin, ckout)
                drs = res.Room(x).GetRatesRows(0).GetRateRows()
                y = 0
                For Each dr In drs
                    cnt += 1
                    If cnt < drs.Length Then
                        dr2 = drs(cnt)
                    Else
                        dr2 = drs(cnt - 1)
                    End If
                    If chindate.Date = (New Date).Date Then
                        chindate = ckin
                        pric = dr.AdultRate + dr.ChildRate
                        pricE = dr.ExtraRateAdult
                        If res.Room(x).ExtraChildren > 0 Then
                            pricE += dr.ExtraRateChild
                        End If
                        choutdate += dr._Date
                        seas = dr.idSeasson

                    End If
                    choutdate = dr._Date
                    sqlcmd.CommandText = "spReservationInsertRates"
                    seas = dr.idSeasson
                    If dr2.AdultRate + dr2.ChildRate <> pric OrElse (ckin.Date = CDate(dr._Date) And y > 0) OrElse y = drs.Length - 1 Then

                        pric = dr.AdultRate + dr.ChildRate
                        pricE = dr.ExtraRateAdult + dr.ExtraRateChild

                        sqlcmd.Parameters.Clear()
                        sqlcmd.Parameters.Add("@idDetalleReservacion", SqlDbType.Int).Value = idDet
                        sqlcmd.Parameters.Add("@idTarifa", SqlDbType.Int).Value = seas
                        sqlcmd.Parameters.Add("@Precio", SqlDbType.Money).Value = pric
                        sqlcmd.Parameters.Add("@PrecioExtra", SqlDbType.Money).Value = pricE
                        sqlcmd.Parameters.Add("@Inicio", SqlDbType.SmallDateTime).Value = chindate
                        sqlcmd.Parameters.Add("@Fin", SqlDbType.SmallDateTime).Value = choutdate
                        If isGal Then
                            sqlcmd.Parameters.Add("@idTarifaGalileo", SqlDbType.NVarChar, 10).Value = Req.Reservation(0).RateCode
                            sqlcmd.Parameters.Add("@Moneda", SqlDbType.NVarChar, 3).Value = Req.Reservation(0).Money
                        Else
                            sqlcmd.Parameters.Add("@Moneda", SqlDbType.NVarChar, 3).Value = res.Reservation(0).Currency
                        End If
                        sqlcmd.ExecuteNonQuery()
                        chindate = dr2._Date

                        pric = dr2.AdultRate + dr2.ChildRate
                        pricE = dr2.ExtraRateAdult
                        If res.Room(x).ExtraChildren > 0 Then
                            pricE += dr2.ExtraRateChild
                        End If

                    Else
                        pric = dr.AdultRate + dr.ChildRate
                        pricE = dr.ExtraRateAdult
                        If res.Room(x).ExtraChildren > 0 Then
                            pricE += dr.ExtraRateChild
                        End If
                    End If



                    dr.SetidSeassonNull()
                    y += 1
                Next
            Next
            transacc.Commit()
        Catch e As Exception
            If trans Then transacc.Rollback()
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            Caderror = e.Message
            Return 2
        Finally
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
        End Try
        Return 0
    End Function



#End Region
#Region "Crear esquemas"
    'si el usuario cambia la tarifa
    Private Function GetResponse(ByVal source As String, ByVal req As reqHotelSell, ByVal confirm As String, ByVal taxes As Single, ByVal total As Single) As resHotelSell
        Dim res As New resHotelSell
        Dim dr As resHotelSell.SellRow
        Dim drRes As resHotelSell.ReservationRow
        Dim drRT As resHotelSell.RoomsRow
        Dim drRoom As resHotelSell.RoomRow
        Dim drRates As resHotelSell.RatesRow
        Dim drRate As resHotelSell.RateRow
        Dim strdate As String
        Dim ckin As Date, ckout As Date
        Dim noches As Long
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        strdate = req.Reservation(0).CheckInDate
        ckin = New Date(strdate.Substring(0, 4), strdate.Substring(4, 2), strdate.Substring(6, 2))
        strdate = req.Reservation(0).CheckOutDate
        ckout = New Date(strdate.Substring(0, 4), strdate.Substring(4, 2), strdate.Substring(6, 2))
        noches = DateDiff(DateInterval.Day, ckin, ckout)
        ''''''''''' inicializacion de la reservacion ''''''''''''''''''''''''''''''''''''''''''''''
        dr = res.Sell.AddSellRow()
        drRes = res.Reservation.NewReservationRow
        drRes.SellRow = dr
        drRes.ChainCode = req.Reservation(0).ChainCode

        drRes.CheckInDate = req.Reservation(0).CheckInDate
        drRes.CheckOutDate = req.Reservation(0).CheckOutDate

        drRes.ConfirmNumber = confirm
        drRes.MarketText = "Thanks for Reeservation UV"
        drRes.PropertyNumber = req.Reservation(0).PropertyNumber
        drRes.Taxes = Math.Round(Math.Round(taxes, 2), 4)
        drRes.Total = Math.Round(Math.Round(total, 2), 4)

        drRes.PriceRAwayAdult = 0
        drRes.PriceRAwayChild = 0
        drRes.PriceRAwayCrib = 0

        drRes.RateCode = req.Reservation(0).RateCode
        drRes.GuarDep = "N"
        drRes.OnRequest = "N"
        If Not req.Reservation(0).IsMoneyNull Then
            drRes.Currency = req.Reservation(0).Money
        End If
        drRes.PlusTax = False

        res.Reservation.AddReservationRow(drRes)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Dim strExc As String
        Dim chExt As Byte, AdExt As Byte
        Dim indroom As Integer = 0

        drRT = res.Rooms.AddRoomsRow(dr)

        For Each drRqRoom As reqHotelSell.RoomRow In req.Room
            drRoom = res.Room.NewRoomRow
            drRoom.Adults = drRqRoom.Adults
            drRoom.Children = drRqRoom.Children
            drRoom.RoomsRow = drRT
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            drRoom.HotelRoomId = 0
            drRoom.ExtraAdults = AdExt
            drRoom.ExtraChildren = chExt
            drRoom.Preferences = req.Room(indroom).Preferences
            res.Room.AddRoomRow(drRoom)
            '''''''''''''''' rate '''''''''''''''''''''''''''''''''
            drRates = res.Rates.AddRatesRow(drRoom)
            For i As Integer = 0 To noches - 1
                drRate = res.Rate.NewRateRow
                drRate.RatesRow = drRates
                drRate.AdultRate = total / noches
                drRate.ChildRate = 0
                drRate.ExtraRateAdult = 0
                drRate.ExtraRateChild = 0
                drRate._Date = ckin.AddDays(i).Date
                drRate.idSeasson = 0
                res.Rate.AddRateRow(drRate)
            Next
            indroom += 1
        Next
        Return (res)
    End Function

    Private Sub GetResponse(ByVal ResHor As resHotelRules)


        Dim res As New resHotelSell
        Dim dr As resHotelSell.SellRow
        Dim drRes As resHotelSell.ReservationRow
        Dim drRT As resHotelSell.RoomsRow
        Dim drRoom As resHotelSell.RoomRow
        Dim drRates As resHotelSell.RatesRow
        Dim drRate As resHotelSell.RateRow

        dr = res.Sell.AddSellRow()
        drRes = res.Reservation.NewReservationRow
        drRes.SellRow = dr
        drRes.ChainCode = ""

        drRes.CheckInDate = CDate(Me.txtCheckIn.Text).Year & CDate(Me.txtCheckIn.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckIn.Text).Day.ToString.PadLeft(2, "0")
        drRes.CheckOutDate = CDate(Me.txtCheckOut.Text).Year & CDate(Me.txtCheckOut.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckOut.Text).Day.ToString.PadLeft(2, "0")
        drRes.ConfirmNumber = ""
        'drRes.MarketText = "Thanks for Reeservation UV"
        drRes.PropertyNumber = Me.cInfoActual.Hotel
        drRes.PriceRAwayAdult = 0
        drRes.PriceRAwayChild = 0
        drRes.PriceRAwayCrib = 0
        drRes.GuarDep = "N"
        drRes.OnRequest = "N"
        drRes.RateCode = Me.ddlRoomtype.SelectedItem.Text.Substring(0, ddlRoomtype.SelectedItem.Text.IndexOf(" - ")).Trim & Me.ddlRate.SelectedValue
        Dim dsHotel As HotelDatos
        With New HotelSistema
            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)
        End With
        impuesto = dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_IMPUESTO)
        If ResHor._Property.Count > 0 Then
            drRes.Taxes = Math.Round(Math.Round(CDbl(ResHor._Property(0).Taxes), 2), 4)
            drRes.Total = Math.Round(Math.Round(CDbl(ResHor._Property(0).TotalExtras) + CDbl(ResHor._Property(0).TotalRate), 2), 4)
            drRes.Currency = ResHor._Property(0).Money
            drRes.PlusTax = ResHor._Property(0).PlusTax
        Else
            drRes.Taxes = 0
            drRes.Total = 0

            'Dim dsEtiq As MonedaDatos
            drRes.PlusTax = IIf(dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).IsNull(HotelDatos.FIELD_PLUSTAX), False, dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_PLUSTAX))

            Dim dsMon As MonedaDatos = (New MonedaSistema).GetMonedaById(dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_IDMONEDA))
            If dsMon.Tables(MonedaDatos.MONEDA_TABLE).Rows.Count > 0 Then
                drRes.Currency = dsMon.Tables(MonedaDatos.MONEDA_TABLE).Rows(0).Item("codigo")
            End If


        End If
        If drRes.PlusTax Then
            Me.CtlReservaMsj1.SetTax = 0

        Else
            Me.CtlReservaMsj1.SetTax = impuesto
        End If

        CtlReservaMsj1.SetMoneda = drRes.Currency
        ' If ResHor.Room.Count > 0 Then
        'estos datos solo cuando hay disponibilidad
        'drRes.RateCode = ResHor.Room(0).RateCode
        'drRes.GuarDep = ResHor.Room(0).GuarDep
        'drRes.OnRequest = ResHor.Room(0).OnRequest
        'drRes.RatePlanName = ResHor.Room(0).RatePlanName
        'drRes.RatePlanDescription = ResHor.Room(0).RatePlanDescription
        'drRes.MinStay = ResHor.Room(0).MinDays
        'drRes.MaxStay = ResHor.Room(0).MaxDays
        ' Else
        'drRes.RateCode = Me.ddlRoomtype.SelectedItem.Text.Substring(0, ddlRoomtype.SelectedItem.Text.IndexOf(" - ")).Trim & Me.ddlRate.SelectedValue
        'drRes.RatePlanName = "TARIFA RACK"
        'drRes.RatePlanDescription = "TARIFA RACK DESCRIPTION"
        'End If
        'drRes.CancelationPoliticies = ResHor._Property(0).CancelationPoliticies
        'drRes.CreditCardPoliticies = ResHor._Property(0).CreditCardPoliticies
        'drRes.GuarantyPoliticies = ResHor._Property(0).GuarantyPoliticies

        res.Reservation.AddReservationRow(drRes)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Dim drV As DataRowView, drs() As DataRow
        'Dim strExc As String
        Dim chExt As Byte, AdExt As Byte
        Dim indroom As Integer = 0

        'Dim tarifaadulto As Double, tarifaninio As Double, TarifaAdultoExc As Double, TarifaNinioExc As Double
        Dim precioExtraAdulto As Double, precioExtraNinio As Double


        drRT = res.Rooms.AddRoomsRow(dr)
        'Dim drPrices() As resHotelRules.PriceRow

        For i As Integer = 0 To Me.cmbRooms.SelectedValue - 1
            drRoom = res.Room.NewRoomRow
            If Not ResHor Is Nothing AndAlso ResHor.Room.Count > 0 Then
                Dim drRqRoom As resHotelRules.RTRoomRow = ResHor.RTRoom(i)
                drRoom.Adults = drRqRoom.Adults
                drRoom.Children = drRqRoom.Children
                drRoom.ExtraAdults = drRqRoom.ExtraAdults
                drRoom.ExtraChildren = drRqRoom.ExtraChildren
            Else
                drRoom.Adults = CType(Me.FindControl("dlAduts" & (i + 1).ToString), DropDownList).SelectedValue
                drRoom.Children = CType(Me.FindControl("dlChild" & (i + 1).ToString), DropDownList).SelectedValue
                drRoom.ExtraAdults = 0
                drRoom.ExtraChildren = 0
            End If

            drRoom.RoomsRow = drRT
            drRoom.HotelRoomId = ddlRoomtype.SelectedValue
            drRoom.Preferences = CType(Me.FindControl("txtPet" & i + 1), TextBox).Text
            res.Room.AddRoomRow(drRoom)

            drRates = res.Rates.AddRatesRow(drRoom)

            If Not ResHor Is Nothing AndAlso ResHor.Room.Count > 0 Then
                Dim dia As Integer
                Dim drRqRoom As resHotelRules.RTRoomRow = ResHor.RTRoom(i)
                For Each drR As resHotelRules.RateRow In ResHor.Rate
                    drRate = res.Rate.NewRateRow
                    drRate.RatesRow = drRates
                    Dim drs As DataRow() = ResHor.Price.Select("ratecode='" & drR.RateCode & "' and Adults=" & drRoom.Adults & " and Children=" & drRqRoom.Children & " and SeassonId=" & drR.SeassonId)
                    drRate._Date = drR._Date
                    drRate.idSeasson = drR.SeassonId
                    drRate.AdultRate = drs(0).Item("RateAdult")
                    drRate.ChildRate = 0
                    If drR.Exc = "Y" Then
                        drRate.AdultRate = drs(0).Item("RateAdultExc")
                    End If
                    If drRqRoom.Children > 0 Then
                        drRate.ChildRate = drs(0).Item("RateChild")
                        If drR.Exc = "Y" Then
                            drRate.ChildRate = drs(0).Item("RateChildExc")
                        End If
                    End If
                    drRate.ExtraRateAdult = 0
                    drRate.ExtraRateChild = 0
                    If drRqRoom.ExtraAdults > 0 Then
                        drRate.ExtraRateAdult = AdExt * IIf(precioExtraAdulto <= 0, 0, precioExtraAdulto)
                    End If
                    If drRqRoom.ExtraChildren > 0 Then
                        drRate.ExtraRateChild = chExt * IIf(precioExtraNinio <= 0, 0, precioExtraNinio)
                    End If
                    If ResHor.Room(0).DaysFree > 0 AndAlso dia Mod ResHor.Room(0).DaysFree = 0 Then
                        '' hay dias libres ''
                        drRate.ExtraRateChild = 0
                        drRate.AdultRate = 0
                        drRate.ChildRate = 0
                        drRate.ExtraRateAdult = 0
                    ElseIf ResHor.Room(0).DescPromotion > 0 Then
                        '''' hay descuento x dia ''
                        drRate.AdultRate = drRate.AdultRate - drRate.AdultRate * ResHor.Room(0).DescPromotion / 100
                        drRate.ChildRate = drRate.ChildRate - drRate.ChildRate * ResHor.Room(0).DescPromotion / 100
                        drRate.ExtraRateAdult = drRate.ExtraRateAdult - drRate.ExtraRateAdult * ResHor.Room(0).DescPromotion / 100
                        drRate.ExtraRateChild = drRate.ExtraRateChild - drRate.ExtraRateChild * ResHor.Room(0).DescPromotion / 100
                    End If
                    res.Rate.AddRateRow(drRate)
                    dia += 1
                Next
            Else

                For dia As Integer = 0 To DateDiff(DateInterval.Day, CDate(Me.txtCheckIn.Text), CDate(Me.txtCheckOut.Text)) - 1
                    drRate = res.Rate.NewRateRow
                    drRate.RatesRow = drRates

                    drRate._Date = CDate(Me.txtCheckIn.Text).AddDays(dia)
                    drRate.idSeasson = 0
                    drRate.AdultRate = 0
                    drRate.ChildRate = 0

                    drRate.ExtraRateAdult = 0
                    drRate.ExtraRateChild = 0

                    res.Rate.AddRateRow(drRate)

                Next
            End If
        Next
        iRes.Value = res.GetXml
    End Sub
    Private Function GetRequest() As reqHotelSell

        Dim xdocSell As New XmlDataDocument(New reqHotelSell)
        Dim dsreqSell As reqHotelSell = CType(xdocSell.DataSet, reqHotelSell)

        Dim drHS As reqHotelSell.HotelSellRow
        drHS = dsreqSell.HotelSell.NewHotelSellRow
        dsreqSell.HotelSell.AddHotelSellRow(drHS)


        '//Customer
        Dim drc As reqHotelSell.CustomerRow
        drc = dsreqSell.Customer.NewCustomerRow
        drc.LastName = Me.txtApellido.Text
        drc.FirstName = Me.txtNombre.Text
        drc.PhoneHome = Me.txtHomePhone.Text
        drc.PhoneWork = Me.txtWorkHome.Text
        drc.Address = Me.txtAddress.Text
        drc.Email = Me.txtEmail.Text
        dsreqSell.Customer.AddCustomerRow(drc)
        drc.SetParentRow(drHS)

        '//HotelHeader
        Dim drH As reqHotelSell.HotelHeaderRow
        drH = dsreqSell.HotelHeader.NewHotelHeaderRow
        drH.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
        drH.Source = "HTL"
        drH.IdPortal = 0
        dsreqSell.HotelHeader.AddHotelHeaderRow(drH)
        drH.SetParentRow(drHS)

        '//Reservation

        Dim drR As reqHotelSell.ReservationRow
        drR = dsreqSell.Reservation.NewReservationRow
        drR.CheckInDate = CDate(Me.txtCheckIn.Text).Year & CDate(Me.txtCheckIn.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckIn.Text).Day.ToString.PadLeft(2, "0")
        drR.CheckOutDate = CDate(Me.txtCheckOut.Text).Year & CDate(Me.txtCheckOut.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckOut.Text).Day.ToString.PadLeft(2, "0")
        drR.PropertyNumber = Me.cInfoActual.Hotel
        If ddlCC.SelectedIndex >= 1 Then
            drR.CreditCardExpiration = Me.txtccMonth.Text & Me.txtCCYear.Text 'MMYYYY
            drR.CreditCardHolder = Me.txtCCHolder.Text
            drR.CreditCardNumber = Me.txtCCNumber.Text
            drR.CreditCardNumberVerify = Me.txtSecCode.Text
            drR.CreditCardType = Me.GetCardId(Me.ddlCC.SelectedValue)
        End If

        drR.RateCode = Me.ddlRoomtype.SelectedItem.Text.Substring(0, ddlRoomtype.SelectedItem.Text.IndexOf(" - ")).Trim & Me.ddlRate.SelectedValue
        drR.ChainCode = "UV"
        drR.RAwayAdult = 0
        drR.RAwayChild = 0
        drR.RAwayCrib = 0
        dsreqSell.Reservation.AddReservationRow(drR)
        drR.SetParentRow(drHS)

        '//rooms
        Dim drhrs As reqHotelSell.RoomsRow
        drhrs = dsreqSell.Rooms.NewRoomsRow
        dsreqSell.Rooms.AddRoomsRow(drhrs)
        drhrs.SetParentRow(drHS)

        '//room
        'Dim cmb As DropDownList
        'Dim tr As HtmlTableRow
        For i As Integer = 0 To Me.cmbRooms.SelectedIndex - 1
            Dim rR As reqHotelSell.RoomRow
            rR = dsreqSell.Room.NewRoomRow
            rR.Preferences = ""
            rR.Adults = CType(Me.FindControl("dlAduts" & (i + 1).ToString), DropDownList).SelectedValue
            rR.Children = CType(Me.FindControl("dlChild" & (i + 1).ToString), DropDownList).SelectedValue
            rR.Preferences = CType(FindControl("txtPet" & (i + 1).ToString), TextBox).Text
            dsreqSell.Room.AddRoomRow(rR)
            rR.SetParentRow(drhrs)
        Next

        iReq.Value = xdocSell.OuterXml


    End Function

    Private Sub MatchResponse()
        Dim SellResponse As New resHotelSell
        Dim CgRate As Boolean = False
        Dim totalRate As Double
        For i As Integer = 0 To cmbRooms.SelectedValue - 1

            Dim drr() As resHotelSell.RateRow = (CType(resSell.Room(i).GetRatesRows(0), resHotelSell.RatesRow)).GetRateRows()
            Dim total As Double = 0
            For Rate As Integer = 0 To drr.Length - 1
                total += CType(drr(Rate), resHotelSell.RateRow).AdultRate + CType(drr(Rate), resHotelSell.RateRow).ChildRate + CType(drr(Rate), resHotelSell.RateRow).ExtraRateAdult + CType(drr(Rate), resHotelSell.RateRow).ExtraRateChild
            Next
            If total <> Me.CtlReservaMsj1.GetPrice(i + 1) Then
                total = Me.CtlReservaMsj1.GetPrice(i + 1)
                CgRate = True
                resSell.Room(i).Adults = CType(Me.FindControl("dlAduts" & (i + 1).ToString), DropDownList).SelectedValue
                resSell.Room(i).Children = CType(Me.FindControl("dlChild" & (i + 1).ToString), DropDownList).SelectedValue
                resSell.Room(i).ExtraAdults = 0
                resSell.Room(i).ExtraChildren = 0
                For Rate As Integer = 0 To drr.Length - 1
                    CType(drr(Rate), resHotelSell.RateRow).AdultRate = total / DateDiff(DateInterval.Day, CDate(Me.txtCheckIn.Text), CDate(Me.txtCheckOut.Text))
                    CType(drr(Rate), resHotelSell.RateRow).ChildRate = 0
                    CType(drr(Rate), resHotelSell.RateRow).ExtraRateAdult = 0
                    CType(drr(Rate), resHotelSell.RateRow).ExtraRateChild = 0
                Next

            End If
            totalRate += total

            'con un cuarto que cambie cambia la tarifa total y el iva
        Next
        If CgRate Then
            resSell.Reservation(0).Total = totalRate
            resSell.Reservation(0).Taxes = totalRate * impuesto / 100
        End If

    End Sub
#End Region

    Private Sub clearData()
        Me.txtCheckIn.Text = ""
        Me.txtCheckOut.Text = ""
        Me.cmbRooms.SelectedIndex = 0

        For i As Integer = 1 To 9
            CType(Me.FindControl("dlAduts" & i.ToString), DropDownList).SelectedIndex = 0
            CType(Me.FindControl("dlChild" & i.ToString), DropDownList).SelectedIndex = 0
            CType(Me.FindControl("txtPet" & i.ToString), TextBox).Text = ""
        Next

        Me.txtNombre.Text = ""
        Me.txtApellido.Text = ""
        Me.txtHomePhone.Text = ""
        Me.txtWorkHome.Text = ""
        Me.txtAddress.Text = ""
        Me.txtEmail.Text = ""
        Me.ddlCC.SelectedIndex = 0
        Me.txtCCNumber.Text = ""
        Me.txtSecCode.Text = ""
        Me.txtCCHolder.Text = ""
        Me.txtccMonth.Text = ""
        Me.txtCCYear.Text = ""
    End Sub

    Protected Sub ddlRoomtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRoomtype.SelectedIndexChanged
        CargaPersonas()
    End Sub

End Class
