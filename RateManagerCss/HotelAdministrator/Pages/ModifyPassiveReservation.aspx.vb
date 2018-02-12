Imports WSHotelFacade
Imports WSHotelCommon
Imports System.Xml
Imports WSHotelDataAccess
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Partial Class ModifyPassiveReservation
    Inherits PaginaBase
    Private Property moneda() As String
        Get
            Return viewstate("_moneda")
        End Get
        Set(ByVal Value As String)
            viewstate("_moneda") = Value
        End Set
    End Property
    Private Property impuesto() As Double
        Get
            Return viewstate("_tax")
        End Get
        Set(ByVal Value As Double)
            viewstate("_tax") = Value
        End Set
    End Property
    Private Property plustax() As Boolean
        Get
            Return viewstate("plustax")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("plustax") = Value
        End Set
    End Property
    Private Property noReservation() As String
        Get
            Return viewstate("_noReservation")
        End Get
        Set(ByVal Value As String)
            viewstate("_noReservation") = Value
        End Set
    End Property


#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnSaveReserva As System.Web.UI.WebControls.Button
    Protected WithEvents iRes As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnContinue As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtPriceRoom1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents divChangeRates As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

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
            clearData()
            lblResError.Visible = False
            LoadData()
            CargaPersonas()
            lblImpuesto.InnerHtml = PortalCulture.GetString("M000080", True) & "-"
            Me.lblError.Visible = False
            If Not Request.QueryString("NoReservation") Is Nothing Then
                Me.txtNoReservation.Text = Request.QueryString("NoReservation")
                Me.btnBuscar_Click(Nothing, Nothing)
            End If
        End If
        'Me.Page.RegisterStartupScript("SelectRoomDefault", "<script>ChangeRooms('" & Me.cmbRooms.ClientID & "','divRoom');</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectRoomDefault", "<script>ChangeRooms('" & Me.cmbRooms.ClientID & "','divRoom');</script>")

        'Me.Page.RegisterStartupScript("SelectDefault", "<script>SelectRoom(1,'divRoom','DataRoom');</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectDefault", "<script>SelectRoom(1,'divRoom','DataRoom');</script>")

        cmbRooms.Attributes.Add("onchange", "javascript:ChangeRooms('" & Me.cmbRooms.ClientID & "','divRoom');SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');SelectRoom(1,'divRoom','DataRoom');")
        Me.txtRoomPrice1.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice2.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice3.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice4.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice5.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice6.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice7.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice8.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.txtRoomPrice9.Attributes.Add("onchange", "javascript:SetTotal('" & txtTax.ClientID & "','" & cmbRooms.ClientID & "','" & Me.ClientID & "','" & lblTotalRes.ClientID & "','" & PortalCulture.GetString("M000240") & "');")
        Me.btnModify.Attributes.Add("onclick", "if (!AreValidPrice('" & Me.cmbRooms.ClientID & "','" & PortalCulture.GetString("00651") & "')) return false;")
        ddlRoomtype.Attributes.Add("onchange", "javascript:ChangeMaxRoomType();")
    End Sub
    Private Sub loadCulture()
        lblTitle.Text = PortalCulture.GetString("M000547")

        lblResInfo.InnerHtml = PortalCulture.GetString("00618")
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
        'lblMofidyReservation.InnerHtml=portalculture.GetString()
        lblNoReservation.InnerHtml = PortalCulture.GetString("M000119", True)
        btnBuscar.Text = PortalCulture.GetString("M0BT0000115")
        RevCCNumber.ErrorMessage = PortalCulture.GetString("00358")
        ReCcY.ErrorMessage = PortalCulture.GetString("00360")
        REMail.ErrorMessage = PortalCulture.GetString("00356")
        RvCcM.ErrorMessage = PortalCulture.GetString("00361")
        ReCcM.ErrorMessage = PortalCulture.GetString("00361")
        Me.btnModify.Text = PortalCulture.GetString("M000106")
        lblRoomType.InnerHtml = PortalCulture.GetString("M000439", True)
        lblAgencia.Text = PortalCulture.GetString("M000524", True)
        lblMainAgenciaData.InnerHtml = PortalCulture.GetString("00614")
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
        If ddlCC.SelectedIndex > 1 Then
            'validar tarjeta de credito
            Dim Tarjeta As Tarjetas = New Tarjetas(Me.txtCCNumber.Text.Trim, GetCardId(Me.ddlCC.SelectedValue))
            If Not Tarjeta.IsValid Then
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00635")
                Me.cvCC.IsValid = False
            End If
            If Me.txtSecCode.Text.Trim = "" Then
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00636")
                Me.cvCC.IsValid = False
            End If
            If Me.txtCCHolder.Text.Trim = "" Then
                Me.cvCC.ErrorMessage = PortalCulture.GetString("00637")
                Me.cvCC.IsValid = False
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

        Return cvDates.IsValid And cvCC.IsValid
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
            ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
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

    Private Function LeeReservaPortalAsociacion(ByVal noreservacion As String) As Boolean
        Dim ds As New DataSet
        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        Dim dsCommand As New SqlDataAdapter
        Dim IdAsociation As Integer
        Dim idPortal As Integer
        Dim hr As Boolean = True

        if Me.GetIdAsociation = -1 then Return hr

        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spReservacionesByAsociation"
                    .Connection = New SqlConnection(ConnectionString)
                    .Parameters.Add(New SqlParameter("@noreservacion", SqlDbType.NVarChar, 24)).Value = noreservacion
                    .Parameters.Add(New SqlParameter("@idAsociacion", SqlDbType.Int)).Value = Me.GetIdAsociation                    
                End With
                .Fill(ds)
            Catch ex As Exception
                Dim s As String = ex.Message.ToString
                ds = Nothing
            Finally
                If Not .SelectCommand Is Nothing Then
                    If Not .SelectCommand.Connection Is Nothing Then
                        .SelectCommand.Connection.Dispose()
                    End If
                    .SelectCommand.Dispose()
                End If
                .Dispose()
            End Try
            If dsEmpty(ds) Then
                hr = False
            End If
        End With
        Return hr
    End Function

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim dsReservaciones As DataTable

        noReservation = ""
        clearData()
        If txtNoReservation.Text <> "" Then
            Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
            Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
            Dim resDis As resHotelDisplay

            Dim drR As reqHotelDisplay.HotelDisplayRow
            drR = dsreq.HotelDisplay.NewHotelDisplayRow
            drR.ConfirmNumber = Me.txtNoReservation.Text
            drR.Language = "en-US"
            dsreq.HotelDisplay.AddHotelDisplayRow(drR)

            With New WSHotelFacade.clsFADisplay
                resDis = .GetHotelDisplay(xdoc.DocumentElement)
            End With
            lblResError.Visible = False
            If Not resDis Is Nothing AndAlso resDis.Reservation.Rows.Count > 0 AndAlso resDis.Reservation(0).PropertyNumber = Me.cInfoActual.Hotel Then
                'hay que preguntar por el source si no es htl no se debe poder modificar
                iResDisplay.Value = resDis.GetXml
                If LeeReservaPortalAsociacion(Me.txtNoReservation.Text) = False Then
                    lblResError.Text = PortalCulture.GetString("00642")
                    lblResError.Visible = True
                    Return
                End If
                If resDis.HotelHeader(0).Source <> "HTL" Then
                    lblResError.Text = PortalCulture.GetString("00642")
                    lblResError.Visible = True
                Else
                    If resDis.Reservation(0).Status <> 1 Then
                        lblResError.Text = "No se puede modificar por que está cancelada" 'PortalCulture.GetString("00642")
                        lblResError.Visible = True
                    Else
                        noReservation = Me.txtNoReservation.Text.Trim
                        txtCheckIn.Text = CDate(resDis.Reservation(0).CheckInDate).ToString("MM/dd/yyyy")
                        txtCheckOut.Text = CDate(resDis.Reservation(0).CheckOutDate).ToString("MM/dd/yyyy")

                        Try
                            Me.ddlRoomtype.SelectedValue = resDis.Room(0).idRoomType
                        Catch ex As Exception
                        End Try

                        Try
                            Me.ddlRate.SelectedValue = ddlRate.Items.FindByText(resDis.Reservation(0).RatePlan).Value()
                        Catch ex As Exception
                        End Try


                        moneda = resDis.Reservation(0).Money
                        Dim dsHotel As HotelDatos
                        With New HotelSistema
                            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)
                        End With
                        impuesto = CDbl(dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_IMPUESTO))
                        plustax = IIf(dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).IsNull(HotelDatos.FIELD_PLUSTAX), False, dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_PLUSTAX))
                        If plustax Then
                            lblImpuesto.InnerHtml = PortalCulture.GetString("M000080", True) & PortalCulture.GetString("00433")
                        Else
                            lblImpuesto.InnerHtml = PortalCulture.GetString("M000080", True) & impuesto & "%"
                        End If
                        txtTax.Value = 0
                        If Not plustax Then
                            txtTax.Value = impuesto
                        End If

                        cmbRooms.SelectedValue = resDis.Room.Count
                        Dim total As Double = 0
                        For i As Integer = 1 To resDis.Room.Count
                            CType(Me.FindControl("dlAduts" + i.ToString), DropDownList).SelectedIndex = resDis.Room(i - 1).Adults - 1
                            CType(Me.FindControl("txtPet" + i.ToString), TextBox).Text = resDis.Room(i - 1).Preferences
                            CType(Me.FindControl("dlChild" + i.ToString), DropDownList).SelectedIndex = resDis.Room(i - 1).Children
                            Dim subtotal As Double = 0
                            For Each dr As resHotelDisplay.RateRow In resDis.Room(i - 1).GetRatesRows(0).GetRateRows
                                subtotal += CDbl(dr.AdultRate) + CDbl(dr.ChildRate) + CDbl(dr.ExtraRateAdult) + CDbl(dr.ExtraRateChild)
                            Next
                            total += subtotal
                            CType(Me.FindControl("txtRoomPrice" + i.ToString), TextBox).Text = Math.Round(subtotal, 2)
                            CType(Me.FindControl("lblMoneyRoom" + i.ToString), HtmlGenericControl).InnerHtml = moneda

                        Next
                        lblMoney.InnerHtml = moneda
                        'falta agregar el totaL Y HACER PRUEBAS CON MAS Y MENPOS HABITACIKONES
                        Me.lblTotalRes.InnerHtml = resDis.Reservation(0).Total ' + resDis.Reservation(0).Taxes

                        txtNombre.Text = resDis.Customer(0).FirstName
                        txtApellido.Text = resDis.Customer(0).LastName
                        If Not resDis.Customer(0).IsPhoneHomeNull Then txtHomePhone.Text = resDis.Customer(0).PhoneHome
                        If Not resDis.Customer(0).IsPhoneWorkNull Then txtWorkHome.Text = resDis.Customer(0).PhoneWork
                        If Not resDis.Customer(0).IsAddressNull Then txtAddress.Text = resDis.Customer(0).Address
                        If Not resDis.Customer(0).IsEmailNull Then txtEmail.Text = resDis.Customer(0).Email
                        If resDis.Reservation(0).CreditCardType <> "" Then ddlCC.SelectedValue = resDis.Reservation(0).CreditCardType
                        txtCCNumber.Text = resDis.Reservation(0).CreditCardNumber
                        txtSecCode.Text = resDis.Reservation(0).CreditCardNumberVerify
                        txtCCHolder.Text = resDis.Reservation(0).CreditCardHolder
                        If resDis.Reservation(0).CreditCardExpiration <> "" Then
                            txtccMonth.Text = resDis.Reservation(0).CreditCardExpiration.Substring(0, 2)
                            txtCCYear.Text = resDis.Reservation(0).CreditCardExpiration.Substring(2)
                        End If

                        With New ReservaFacade
                            dsReservaciones = .GetDataReservaByNum(resDis.Reservation(0).ConfirmNumber)
                        End With
                        If Not dsReservaciones Is Nothing AndAlso dsReservaciones.Rows.Count > 0 Then
                            If Not dsReservaciones.Rows(0).IsNull("agency") Then
                                Me.txtAgencia.Text = dsReservaciones.Rows(0)("agency")
                            End If
                        End If


                    End If
                    End If
            Else
                    lblResError.Text = PortalCulture.GetString("00641")
                    lblResError.Visible = True
                End If
            End If

    End Sub
#Region "do modify"

    Private Function GetResponse(ByVal req As reqHotelModify) As Byte
        Try

            Dim resdisplay As resHotelDisplay
            Dim res As New resHotelModify
            Dim dr As resHotelModify.ModifyRow
            Dim drRes As resHotelModify.ReservationRow
            Dim drRT As resHotelModify.RoomsRow
            Dim drRoom As resHotelModify.RoomRow
            Dim drRates As resHotelModify.RatesRow
            Dim drRate As resHotelModify.RateRow
            ''''''''''' inicializacion de la reservacion ''''''''''''''

            Dim xdocReq As New XmlDataDocument(New resHotelDisplay)
            xdocReq.LoadXml(iResDisplay.Value)
            resdisplay = CType(xdocReq.DataSet, resHotelDisplay)

            dr = res.Modify.AddModifyRow()
            drRes = res.Reservation.NewReservationRow()
            drRes.ModifyRow = dr

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

            drRes.ChainCode = req.Reservation(0).ChainCode

            drRes.CheckInDate = req.Reservation(0).CheckInDate
            drRes.CheckOutDate = req.Reservation(0).CheckOutDate
            drRes.MarketText = ""

            drRes.ConfirmNumber = Me.noReservation
            drRes.Segment = Me.ddlRate.SelectedValue
            drRes.PropertyNumber = req.Reservation(0).PropertyNumber
            drRes.CancelPrior = ""

            drRes.Taxes = 0
            drRes.Total = 0

            drRes.PriceRAwayAdult = 0
            drRes.PriceRAwayChild = 0
            drRes.PriceRAwayCrib = 0

            drRes.RateCode = Me.ddlRoomtype.SelectedItem.Text.Substring(0, ddlRoomtype.SelectedItem.Text.IndexOf(" - ")).Trim & Me.ddlRate.SelectedItem.Text
            drRes.RatePlan = drRes.RateCode
            drRes.GuarDep = "N"
            drRes.money = moneda
            res.Reservation.AddReservationRow(drRes)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Dim strExc As String
            Dim chExt As Byte, AdExt As Byte
            Dim indroom As Integer = 0

            drRT = res.Rooms.AddRoomsRow(dr)
            Dim total As Double

            For Each drRqRoom As reqHotelModify.RoomRow In req.Room
                drRoom = res.Room.NewRoomRow
                drRoom.Adults = drRqRoom.Adults
                drRoom.Children = drRqRoom.Children
                drRoom.RoomsRow = drRT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''
                drRoom.HotelRoomId = 0
                drRoom.ExtraAdults = AdExt
                drRoom.ExtraChildren = chExt
                drRoom.Preferences = req.Room(indroom).Preferences
                drRoom.HotelRoomId = Me.ddlRoomtype.SelectedValue

                res.Room.AddRoomRow(drRoom)
                '''''''''''''''' rate '''''''''''''''''''''''''''''''''
                drRates = res.Rates.AddRatesRow(drRoom)

                Dim subtotal As Double = 0
                If resdisplay.Room.Count < indroom Then
                    For Each drRat As resHotelDisplay.RateRow In resdisplay.Room(indroom).GetRatesRows(0).GetRateRows
                        subtotal += CDbl(drRat.AdultRate) + CDbl(drRat.ChildRate) + CDbl(drRat.ExtraRateAdult) + CDbl(drRat.ExtraRateChild)
                    Next
                End If

                If subtotal <> CDbl(CType(Me.FindControl("txtRoomPrice" & indroom + 1), TextBox).Text) Or CDate(Me.txtCheckIn.Text) <> CDate(resdisplay.Reservation(0).CheckInDate) Or CDate(Me.txtCheckOut.Text) <> CDate(resdisplay.Reservation(0).CheckOutDate) Then
                    'cambió la tarifa
                    For i As Integer = 0 To noches - 1
                        drRate = res.Rate.NewRateRow
                        drRate.RatesRow = drRates
                        drRate.AdultRate = CDbl(CType(Me.FindControl("txtRoomPrice" & indroom + 1), TextBox).Text) / noches
                        drRate.ChildRate = 0
                        drRate.ExtraRateAdult = 0
                        drRate.ExtraRateChild = 0
                        drRate._Date = ckin.AddDays(i).Date
                        drRate.idSeasson = 0
                        res.Rate.AddRateRow(drRate)
                        total += drRate.AdultRate
                    Next
                Else
                    If resdisplay.Room.Count < indroom Then
                        For Each drRat As resHotelDisplay.RateRow In resdisplay.Room(indroom).GetRatesRows(0).GetRateRows
                            drRate = res.Rate.NewRateRow
                            drRate.RatesRow = drRates
                            drRate.AdultRate = drRat.AdultRate
                            drRate.ChildRate = drRat.ChildRate
                            drRate.ExtraRateAdult = drRat.ExtraRateAdult
                            drRate.ExtraRateChild = drRat.ExtraRateChild
                            drRate._Date = drRat._Date
                            drRate.idSeasson = drRat.idSeasson
                            res.Rate.AddRateRow(drRate)
                            total += CDbl(drRat.AdultRate) + CDbl(drRat.ChildRate) + CDbl(drRat.ExtraRateAdult) + CDbl(drRat.ExtraRateChild)
                        Next
                    Else
                        'es habitación nueva... hay que agregar nuevas
                        For i As Integer = 0 To noches - 1
                            drRate = res.Rate.NewRateRow
                            drRate.RatesRow = drRates
                            drRate.AdultRate = CDbl(CType(Me.FindControl("txtRoomPrice" & indroom + 1), TextBox).Text) / noches
                            drRate.ChildRate = 0
                            drRate.ExtraRateAdult = 0
                            drRate.ExtraRateChild = 0
                            drRate._Date = ckin.AddDays(i).Date
                            drRate.idSeasson = 0
                            res.Rate.AddRateRow(drRate)
                            total += drRate.AdultRate
                        Next
                        'probar eliminando habitaciones
                    End If

                End If

                indroom += 1
            Next
            res.Reservation(0).Taxes = resdisplay.Reservation(0).Taxes
            res.Reservation(0).Total = resdisplay.Reservation(0).Total
            Dim dsRes As DataSet
            With New clsDASell
                dsRes = .LoadReservationByNumber(Me.noReservation)
            End With
            res.Reservation(0).Taxes = 0
            If total <> resdisplay.Reservation(0).Total Or CDate(Me.txtCheckIn.Text) <> CDate(resdisplay.Reservation(0).CheckInDate) Or CDate(Me.txtCheckOut.Text) <> CDate(resdisplay.Reservation(0).CheckOutDate) Or Me.cmbRooms.SelectedIndex + 1 <> resdisplay.Room.Count Then
                If Not plustax Then
                    res.Reservation(0).Taxes = Math.Round(Math.Round(total * impuesto / 100, 2), 4)
                End If
                res.Reservation(0).Total = Math.Round(Math.Round(total, 2), 4)
                GetResponse = Update(dsRes.Tables(0).Rows(0)("idreservacion"), req, res, plustax, "", "", impuesto)
                'If impuesto > 0 Then
                '    GetResponse = Update(dsRes.Tables(0).Rows(0)("idreservacion"), req, res, False, "", "", impuesto)
                'Else
                '    GetResponse = Update(dsRes.Tables(0).Rows(0)("idreservacion"), req, res, True, "", "", impuesto)
                'End If
            Else
                GetResponse = Update(dsRes.Tables(0).Rows(0)("idreservacion"), req, res, resdisplay.Reservation(0).PlusTax, "", "", impuesto)
            End If
            If GetResponse = 0 Then
                MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & dsRes.Tables(0).Rows(0)("idreservacion"))
            End If
        Catch ex As Exception
            GetResponse = 2
        End Try
    End Function
    Private Function GetRequest() As reqHotelModify
        Try


            Dim xdocMod As New XmlDataDocument(New reqHotelModify)
            Dim dsreqMod As reqHotelModify = CType(xdocMod.DataSet, reqHotelModify)

            Dim drHS As reqHotelModify.HotelModifyRow
            drHS = dsreqMod.HotelModify.NewHotelModifyRow
            dsreqMod.HotelModify.AddHotelModifyRow(drHS)


            '//Customer
            Dim drc As reqHotelModify.CustomerRow
            drc = dsreqMod.Customer.NewCustomerRow
            drc.LastName = Me.txtApellido.Text
            drc.FirstName = Me.txtNombre.Text
            drc.PhoneHome = Me.txtHomePhone.Text
            drc.PhoneWork = Me.txtWorkHome.Text
            drc.Address = Me.txtAddress.Text
            drc.Email = Me.txtEmail.Text
            dsreqMod.Customer.AddCustomerRow(drc)
            drc.SetParentRow(drHS)

            '//HotelHeader
            Dim drH As reqHotelModify.HotelHeaderRow
            drH = dsreqMod.HotelHeader.NewHotelHeaderRow
            drH.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
            drH.Source = "HTL"
            drH.IdPortal = 0
            dsreqMod.HotelHeader.AddHotelHeaderRow(drH)
            drH.SetParentRow(drHS)

            '//Reservation

            Dim drR As reqHotelModify.ReservationRow
            drR = dsreqMod.Reservation.NewReservationRow
            drR.CheckInDate = CDate(Me.txtCheckIn.Text).Year & CDate(Me.txtCheckIn.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckIn.Text).Day.ToString.PadLeft(2, "0")
            drR.CheckOutDate = CDate(Me.txtCheckOut.Text).Year & CDate(Me.txtCheckOut.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtCheckOut.Text).Day.ToString.PadLeft(2, "0")
            drR.PropertyNumber = Me.cInfoActual.Hotel


            If ddlCC.SelectedIndex > 0 Then
                drR.CreditCardExpiration = Me.txtccMonth.Text & Me.txtCCYear.Text 'MMYYYY
                drR.CreditCardHolder = Me.txtCCHolder.Text
                drR.CreditCardNumber = Me.txtCCNumber.Text
                drR.CreditCardNumberVerify = Me.txtSecCode.Text
                drR.CreditCardType = GetCardId(Me.ddlCC.SelectedValue)
            End If

            drR.RateCode = Me.ddlRoomtype.SelectedItem.Text.Substring(0, ddlRoomtype.SelectedItem.Text.IndexOf(" - ")).Trim & Me.ddlRate.SelectedValue
            drR.ChainCode = "UV"
            drR.RAwayAdult = 0
            drR.RAwayChild = 0
            drR.RAwayCrib = 0
            dsreqMod.Reservation.AddReservationRow(drR)
            drR.SetParentRow(drHS)

            '//rooms
            Dim drhrs As reqHotelModify.RoomsRow
            drhrs = dsreqMod.Rooms.NewRoomsRow
            dsreqMod.Rooms.AddRoomsRow(drhrs)
            drhrs.SetParentRow(drHS)

            '//room
            'Dim cmb As DropDownList
            'Dim tr As HtmlTableRow
            For i As Integer = 0 To Me.cmbRooms.SelectedIndex
                Dim rR As reqHotelModify.RoomRow
                rR = dsreqMod.Room.NewRoomRow
                rR.Preferences = ""
                rR.Adults = CType(Me.FindControl("dlAduts" & (i + 1).ToString), DropDownList).SelectedValue
                rR.Children = CType(Me.FindControl("dlChild" & (i + 1).ToString), DropDownList).SelectedValue
                rR.Preferences = CType(FindControl("txtPet" & (i + 1).ToString), TextBox).Text
                dsreqMod.Room.AddRoomRow(rR)
                rR.SetParentRow(drhrs)
            Next
            Return dsreqMod
            'iReq.Value = xdocMod.OuterXml
        Catch ex As Exception

        End Try
        Return Nothing
    End Function
#End Region


    Public Function Update(ByVal idReservation As Long, ByVal Req As reqHotelModify, ByRef res As resHotelModify, ByVal ptax As Boolean, ByVal restext As String, ByRef Caderror As String, ByVal pctTax As Double) As Byte
        Dim errorpago As Boolean = False
        Dim ckin, ckout As Date
        Dim trans As Boolean = False
        Dim sqlconn As New SqlConnection(AppSettings("HotelConnection"))
        Dim transacc As SqlTransaction
        Dim idRes As Long
        Dim strdate As String
        Try
            strdate = Req.Reservation(0).CheckInDate
            ckin = New Date(strdate.Substring(0, 4), strdate.Substring(4, 2), strdate.Substring(6, 2))
            strdate = Req.Reservation(0).CheckOutDate
            ckout = New Date(strdate.Substring(0, 4), strdate.Substring(4, 2), strdate.Substring(6, 2))

            sqlconn.Open()
            transacc = sqlconn.BeginTransaction
            trans = True
            Dim sqlcmd As New SqlCommand("spReservationPassiveUpdate", sqlconn)
            'If Res.idReservation > 0 Then sqlcmd.CommandText = "spReservationUpdate"
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Transaction = transacc

            'Reservacion            
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idReservation

            If Req.Customer.Count > 0 Then
                If Req.Customer(0).IsUserIdNull OrElse Req.Customer(0).UserId = "" Then Req.Customer(0).UserId = 0
                '''''''''''
                sqlcmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = Req.Customer(0).UserId
            End If
            'If res.idHotel > 0 Then
            If res.Reservation(0).PropertyNumber > 0 Then sqlcmd.Parameters.Add("@idHotel", SqlDbType.Int).Value = res.Reservation(0).PropertyNumber
            'Else
            'sqlcmd.Parameters.Add("@idHotel", SqlDbType.Int).Value = 0
            'If res.Hotel.Name <> "" Then sqlcmd.Parameters.Add("@HotelGNombre", SqlDbType.NVarChar, 50).Value = res.Hotel.Name
            'If res.Hotel.City <> "" Then sqlcmd.Parameters.Add("@HotelGCiudad", SqlDbType.NVarChar, 50).Value = res.Hotel.City
            'If res.Hotel.Country <> "" Then sqlcmd.Parameters.Add("@HotelGPais", SqlDbType.NVarChar, 3).Value = res.Hotel.Country
            'End If
            'nuevo para galileo
            If Not Req.HotelHeader(0).IsIdPortalNull AndAlso Req.HotelHeader(0).IdPortal <> "" AndAlso Req.HotelHeader(0).IdPortal > 0 Then sqlcmd.Parameters.Add("@IdPortal", SqlDbType.Int).Value = Req.HotelHeader(0).IdPortal
            ''''''' a ver si aplica '''''''''
            If res.Reservation(0).ChainCode <> "" Then sqlcmd.Parameters.Add("@ChainCode", SqlDbType.NVarChar, 2).Value = res.Reservation(0).ChainCode
            'If res.idHotelGalileo <> "" Then sqlcmd.Parameters.Add("@idHotelGalileo", SqlDbType.NVarChar, 10).Value = res.idHotelGalileo
            '--------------
            'If res.ReservationNumberGalileo.Trim <> "" Then sqlcmd.Parameters.Add("@NoReservacionGalileo", SqlDbType.NVarChar, 50).Value = res.ReservationNumberGalileo
            'If res.ReservationGalileoConfNumber.Trim <> "" Then sqlcmd.Parameters.Add("@NoConfGalileo", SqlDbType.NVarChar, 50).Value = res.ReservationGalileoConfNumber
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
                sqlcmd.Parameters.Add("@Total", SqlDbType.Money).Value = res.Reservation(0).Total  '  res.Total + (res.Total * res.Hotel.Tax / 100)
            End If
            If Not String.IsNullOrEmpty(txtAgencia.Text) Then
                sqlcmd.Parameters.Add("@Agency", SqlDbType.NVarChar, 80).Value = txtAgencia.Text
            End If
            sqlcmd.Parameters.Add("@ReservationPorPlan", SqlDbType.Bit).Value = False
            'Guardamos el tipo de cambio
            'Datos del cliente
            If Req.Customer.Count > 0 AndAlso Req.Customer(0).UserId = 0 Then
                If Req.Customer(0).FirstName <> "" Then sqlcmd.Parameters.Add("@Nombre_cl", SqlDbType.NVarChar, 80).Value = Req.Customer(0).FirstName
                If Req.Customer(0).LastName <> "" Then sqlcmd.Parameters.Add("@Apellido_cl", SqlDbType.NVarChar, 80).Value = Req.Customer(0).LastName
                If Req.Customer(0).PhoneHome <> "" Then sqlcmd.Parameters.Add("@Telefono_cl", SqlDbType.NVarChar, 38).Value = Req.Customer(0).PhoneHome
                If Req.Customer(0).PhoneWork <> "" Then sqlcmd.Parameters.Add("@TelefonoTrabajo_cl", SqlDbType.NVarChar, 38).Value = Req.Customer(0).PhoneWork
                If Req.Customer(0).Email <> "" Then sqlcmd.Parameters.Add("@Email_cl", SqlDbType.NVarChar, 80).Value = Req.Customer(0).Email
                If Req.Customer(0).Address <> "" Then sqlcmd.Parameters.Add("@Direccion_cl", SqlDbType.NVarChar, 50).Value = Req.Customer(0).Address
            End If
            'If res.idAgency > 0 Then
            '    sqlcmd.Parameters.Add("@idAgencia", SqlDbType.Int).Value = res.idAgency
            'End If
            '//''datos de wizcom ''''
            'If Not Me.Ignore Then
            If Req.WizcomData.Rows.Count > 0 Then
                sqlcmd.Parameters.Add("@TxCode", SqlDbType.NVarChar, 2).Value = Req.WizcomData(0).ACTransaction
                If Req.WizcomData(0).TypeMessage = "A" Then
                    sqlcmd.Parameters.Add("@StatusConf", SqlDbType.Bit).Value = False
                Else
                    sqlcmd.Parameters.Add("@StatusConf", SqlDbType.Bit).Value = True
                End If
                If Req.WizcomData(0).SystemCode <> "" Then sqlcmd.Parameters.Add("@SystemCode", SqlDbType.NVarChar, 2).Value = Req.WizcomData(0).SystemCode
            End If
            'End If
            ''''''''''''para los rollaway ''''''''''''''
            'Dim _AdultRoll As Byte
            'Dim _ChildRoll As Byte
            'Dim _CribRoll As Byte
            'Dim _PriceChildRoll As Single
            'Dim _PriceCribRoll As Single
            'Dim _PriceAdultRoll As Single

            If Not Req.Reservation(0).IsRAwayAdultNull Then sqlcmd.Parameters.Add("@AdultRoll", SqlDbType.TinyInt).Value = Req.Reservation(0).RAwayAdult
            If Not Req.Reservation(0).IsRAwayChildNull Then sqlcmd.Parameters.Add("@ChildRoll", SqlDbType.TinyInt).Value = Req.Reservation(0).RAwayChild
            If Not Req.Reservation(0).IsRAwayCribNull Then sqlcmd.Parameters.Add("@CribRoll", SqlDbType.TinyInt).Value = Req.Reservation(0).RAwayCrib
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
            '''''''''''''' 'res.ChainCode            '''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If res.Reservation(0).GuarDep <> "" Then sqlcmd.Parameters.Add("@GuarDep", SqlDbType.Char, 1).Value = res.Reservation(0).GuarDep

            sqlcmd.Parameters.Add("@RatePlan", SqlDbType.NVarChar, 4).Value = res.Reservation(0).RateCode.Substring(3)

            sqlcmd.Parameters.Add("@Source", SqlDbType.NVarChar, 4).Value = Req.HotelHeader(0).Source
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''' executar comando '''''
            If restext <> "" Then
                If Req.WizcomData.Count > 0 AndAlso Req.WizcomData(0).TypeMessage = "A" Then
                    sqlcmd.Parameters.Add("@ReservacionText", SqlDbType.NText).Value = restext 'dataWZ.ReservationStr
                End If
            End If
            If Not Req.Reservation(0).IsCreditCardTypeNull Then
                sqlcmd.Parameters.Add("@Numero_cc", SqlDbType.NText).Value = crypto.EncryptString128Bit(Req.Reservation(0).CreditCardNumber, crypto.PublicKey)
                sqlcmd.Parameters.Add("@Nombre_cc", SqlDbType.NVarChar, 80).Value = Req.Reservation(0).CreditCardHolder
                sqlcmd.Parameters.Add("@Tipo_cc", SqlDbType.TinyInt).Value = Req.Reservation(0).CreditCardType
                sqlcmd.Parameters.Add("@ExpiraMes_cc", SqlDbType.NVarChar, 2).Value = Req.Reservation(0).CreditCardExpiration.Substring(0, 2)
                sqlcmd.Parameters.Add("@ExpiraAnio_cc", SqlDbType.NVarChar, 4).Value = Req.Reservation(0).CreditCardExpiration.Substring(2)
                sqlcmd.Parameters.Add("@Digito_cc", SqlDbType.NVarChar, 4).Value = Req.Reservation(0).CreditCardNumberVerify
            Else
                sqlcmd.Parameters.Add("@Numero_cc", SqlDbType.NText).Value = ""
                sqlcmd.Parameters.Add("@Nombre_cc", SqlDbType.NVarChar, 80).Value = ""
                sqlcmd.Parameters.Add("@Tipo_cc", SqlDbType.TinyInt).Value = System.DBNull.Value
                sqlcmd.Parameters.Add("@ExpiraMes_cc", SqlDbType.NVarChar, 2).Value = ""
                sqlcmd.Parameters.Add("@ExpiraAnio_cc", SqlDbType.NVarChar, 4).Value = ""
                sqlcmd.Parameters.Add("@Digito_cc", SqlDbType.NVarChar, 4).Value = ""
            End If


            sqlcmd.ExecuteNonQuery()
            idRes = idReservation 'sqlcmd.Parameters("@idReservacion").Value
            'If res.idReservation = 0 Then idRes = sqlcmd.Parameters("@idReservacion").Value
            '''' Detalle ''''
            Dim x As Long
            Dim idDet As Long

            sqlcmd.CommandText = "spDeleteTarifasDetalleRes"
            sqlcmd.Parameters.Clear()
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idRes

            sqlcmd.ExecuteNonQuery()

            sqlcmd.CommandText = "spDeleteDetalleRes"
            sqlcmd.Parameters.Clear()
            sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idRes

            sqlcmd.ExecuteNonQuery()

            For x = 0 To res.Room.Count - 1
                'If Res.idReservation = 0 Then
                sqlcmd.CommandText = "spReservationInsertDetails"
                'Else
                '    sqlcmd.CommandText = "spReservationUpdateDetails"
                'End If
                sqlcmd.Parameters.Clear()
                sqlcmd.Parameters.Add("@idDetalleReservacion", SqlDbType.Int).Direction = ParameterDirection.Output
                sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idRes
                'Nuevo galileo
                sqlcmd.Parameters.Add("@idHabitacionGalileo", SqlDbType.NVarChar, 10).Value = DBNull.Value
                'If res.idHabitacionGalileo.Trim <> "" Then
                'sqlcmd.Parameters.Add("@idTipoHabitacion_Hotel", SqlDbType.Int).Value = 0

                'Else
                sqlcmd.Parameters.Add("@idTipoHabitacion_Hotel", SqlDbType.Int).Value = res.Room(x).HotelRoomId
                'End If
                res.Room(x).SetHotelRoomIdNull()
                sqlcmd.Parameters.Add("@idTipoPlan", SqlDbType.Int).Value = 0
                If Req.Customer.Count > 0 AndAlso Req.Customer(0).UserId = 0 Then
                    sqlcmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = 0
                Else
                    sqlcmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = Req.Room(x).idtraveler
                End If
                sqlcmd.Parameters.Add("@Adultos", SqlDbType.TinyInt).Value = res.Room(x).Adults
                sqlcmd.Parameters.Add("@pextras", SqlDbType.TinyInt).Value = res.Room(x).ExtraAdults + res.Room(x).ExtraChildren   'Res.Rooms(x).NumberAdults
                sqlcmd.Parameters.Add("@Ninios", SqlDbType.TinyInt).Value = res.Room(x).Children
                sqlcmd.Parameters.Add("@EdadesNinios", SqlDbType.NVarChar, 255).Value = ""
                sqlcmd.Parameters.Add("@Preferencia", SqlDbType.NVarChar, 255).Value = res.Room(x).Preferences
                sqlcmd.ExecuteNonQuery()
                idDet = sqlcmd.Parameters("@idDetalleReservacion").Value
                Dim y As Long
                Dim chindate As Date = New Date
                Dim choutdate As Date = New Date
                Dim pric As Double = 0, pricE As Double = 0
                Dim seas As Integer
                Dim nights As Integer
                'Dim ratecode As String
                Dim drs() As resHotelModify.RateRow, dr As resHotelModify.RateRow
                nights = DateDiff(DateInterval.Day, ckin, ckout)
                drs = res.Room(x).GetRatesRows(0).GetRateRows()
                y = 0
                For Each dr In drs
                    If chindate.Date = (New Date).Date Then
                        chindate = ckin
                        pric = dr.AdultRate + dr.ChildRate
                        pricE = res.Room(x).ExtraAdults + dr.ExtraRateChild
                        choutdate = dr._Date
                        seas = dr.idSeasson
                    Else
                        'If ckin.Date <= CDate(dr._Date) Then
                        '    Exit For
                        'End If
                    End If
                    choutdate = dr._Date
                    sqlcmd.CommandText = "spReservationInsertRates"
                    If dr.AdultRate + dr.ChildRate <> pric OrElse (ckin.Date = CDate(dr._Date) And y > 0) OrElse y = drs.Length - 1 Then
                        sqlcmd.Parameters.Clear()
                        sqlcmd.Parameters.Add("@idDetalleReservacion", SqlDbType.Int).Value = idDet
                        sqlcmd.Parameters.Add("@idTarifa", SqlDbType.Int).Value = seas
                        sqlcmd.Parameters.Add("@Precio", SqlDbType.Money).Value = pric
                        sqlcmd.Parameters.Add("@PrecioExtra", SqlDbType.Money).Value = pricE
                        sqlcmd.Parameters.Add("@Inicio", SqlDbType.SmallDateTime).Value = chindate
                        sqlcmd.Parameters.Add("@Fin", SqlDbType.SmallDateTime).Value = choutdate
                        sqlcmd.Parameters.Add("@Moneda", SqlDbType.NVarChar, 3).Value = res.Reservation(0).money
                        sqlcmd.ExecuteNonQuery()
                        If ckin.Date = CDate(dr._Date) Then
                            chindate = ckin
                        Else
                            chindate = choutdate.AddDays(1)
                        End If
                    End If
                    pric = dr.AdultRate + dr.ChildRate
                    pricE = dr.ExtraRateAdult + dr.ExtraRateAdult
                    choutdate = dr._Date
                    seas = dr.idSeasson
                    dr.SetidSeassonNull() ' .(res.Rate.idseassoncolumn.columnname) = DBNull.Value
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

    Private Sub btnModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModify.Click
        If Me.noReservation <> "" Then
            If Me.Page.IsValid AndAlso IsValidData() Then
                Dim req As reqHotelModify
                req = GetRequest()
                If GetResponse(req) = 0 Then
                    Me.clearData()
                Else
                    Me.lblError.Visible = True
                    Me.lblError.Text = PortalCulture.GetString("00666")
                End If
            End If
        End If
    End Sub


    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCulture()
    End Sub

    Private Sub clearData()
        Me.txtCheckIn.Text = ""
        Me.txtCheckOut.Text = ""
        Me.cmbRooms.SelectedIndex = 0
        lblImpuesto.InnerHtml = PortalCulture.GetString("M000080", True) & "-"
        For i As Integer = 1 To 9
            CType(Me.FindControl("dlAduts" & i.ToString), DropDownList).SelectedIndex = 0
            CType(Me.FindControl("dlChild" & i.ToString), DropDownList).SelectedIndex = 0
            CType(Me.FindControl("txtRoomPrice" & i.ToString), TextBox).Text = ""
            CType(Me.FindControl("txtPet" & i.ToString), TextBox).Text = ""
        Next
        lblTotalRes.InnerHtml = 0.0
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
End Class
