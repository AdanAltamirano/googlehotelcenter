Imports WSHotelFacade
Imports WSHotelCommon
Imports System.Xml
Imports System.Text

Partial Class Booking
    Inherits PaginaBase
    Enum dgcolumns
        NameRoomType
        RatePlanName
        Total
        ShowDetails
        Bookit
        RateCode
        Availability
        currency
        Message
    End Enum

    Enum ErrorType
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
        NotAllowBankDeposit '022 Este hotel no permite reservar por depósito bancario
        InvalidAccessCode '023 the rate plan have access code
        ModifyRestricted '024 Restricción de modificación
        SessionIdRequired '025 SessionIdRequired
        SessionIdNoFound '026 reservation with this SessionId wasn't found
        InvalidCreditCardExpDate '027 Fecha de Expiracion invalida
        InvalidDates '' 028 fechas de expiracion invalidas 
        InvalidFrecuentCode '029
        PMS '030 Error en PSM
        RestrictedRate 'Tarifa restringida
        MinNumberRoomsRestriced '019'' Excced Maxime Number Rooms
        MinPeople '019'' REstriccion de mìnimo de personas
        StatusIsNotOnRequest
        NotAllowPayPalPayment '022 Este hotel no permite reservar por depósito bancario
    End Enum

    Protected WithEvents ddlTarjetas As System.Web.UI.WebControls.DropDownList
    Protected WithEvents REMail As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents txtMonth As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtYear As System.Web.UI.WebControls.TextBox
    Protected WithEvents REVYear As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RVMonth As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents RfvMonth As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RfvYear As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RevCardNumber As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RfvCardNumber As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RfvNumberVer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RfvNombre As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RfvWorkPhone As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblConfirmationNumber As System.Web.UI.WebControls.Label
    Protected WithEvents lblsubtotal As System.Web.UI.WebControls.Label
    Protected WithEvents lblTaxes As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblExpirationError As System.Web.UI.WebControls.Label


    Private Property RateCode() As String
        Get
            Return viewstate("_RateCode")
        End Get
        Set(ByVal Value As String)
            viewstate("_RateCode") = Value
        End Set
    End Property

    Private Property ResponseHOC() As resHotelCompPortal
        Get
            Return viewstate("_ResponseHOC")
        End Get
        Set(ByVal Value As resHotelCompPortal)
            viewstate("_ResponseHOC") = Value
        End Set
    End Property
    Private Property RequestHOC() As reqHotelComplete
        Get
            Return viewstate("_RequestHOC")
        End Get
        Set(ByVal Value As reqHotelComplete)
            viewstate("_RequestHOC") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub





    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
        btnSearch.OnClientClick = "return isValidAges();"
    End Sub

#End Region

    Private ReadOnly Property HaveParams() As Boolean
        Get
            Dim requireds As String() = {"checkIn", "checkOut", "rooms", "adults", "children"}
            Dim flag As Boolean = True
            For Each key As String In requireds
                If Me.Request.QueryString(key) Is Nothing OrElse Me.Request.QueryString(key).Length = 0 Then
                    flag = False
                    Exit For
                End If
            Next
            Return flag
        End Get
    End Property

    Private Function GetDateFor(ByVal value As String) As Date
        Dim result As Date = Today
        If value IsNot Nothing AndAlso value.Length = 8 Then
            Try
                result = New Date(value.Substring(0, 4), value.Substring(4, 2), value.Substring(6, 2))
            Catch ex As Exception
            End Try
        End If
        Return result
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        If Not IsPostBack Then

            'Se Inicializa el valor de los campos
            'CargaRooms()
            If Me.HaveParams Then
                Me.txtInicio.Text = Me.GetDateFor(Me.Request.QueryString("checkIn")).ToString("MM/dd/yyyy")
                Me.txtFinal.Text = Me.GetDateFor(Me.Request.QueryString("checkOut")).ToString("MM/dd/yyyy")

                Me.cmbRooms.SelectedIndex = Me.cmbRooms.Items.IndexOf(Me.cmbRooms.Items.FindByValue(Me.Request.QueryString("rooms")))
                Me.CmbAdultos1.SelectedIndex = Me.CmbAdultos1.Items.IndexOf(Me.CmbAdultos1.Items.FindByValue(Me.Request.QueryString("adults")))
                Me.cmbChildren1.SelectedIndex = Me.cmbChildren1.Items.IndexOf(Me.cmbChildren1.Items.FindByValue(Me.Request.QueryString("children")))

                btnSearch_Click(sender, e )

            Else
                cmbRooms.SelectedIndex = 0
                Me.txtInicio.Text = Date.Today.ToString("MM/dd/yyyy")
                Me.txtFinal.Text = Date.Today.AddDays(1).ToString("MM/dd/yyyy")
            End If
            Me.lblError.Visible = False


        End If
        btnSearch.OnClientClick = "return FireUpdateStatus();"
    End Sub


    Private Function CreateChildAges(ByVal iChilds As Integer, ByVal cuartos As Integer) As Boolean
        Dim cmb As DropDownList
        Dim tr As HtmlTableRow
        Dim td As HtmlTableCell
        Dim tmpText As TextBox
        Dim tmpLabel As Label

        For i As Integer = 1 To cuartos

            tr = New HtmlTableRow
            tr.ID = "TRTOLD" & i

            td = New HtmlTableCell
            td.ColSpan = iChilds
            td.InnerHtml = String.Format("{0} {1}:", PortalCulture.GetString("00170"), i)
            td.Style("border-bottom") = "solid 1px #ccc"
            tr.Cells.Add(td)
            tblChilds.Rows.Add(tr)

            tr = New HtmlTableRow
            tr.ID = "TRHOLD" & i
            For k As Integer = 1 To iChilds
                td = New HtmlTableCell
                tmpLabel = New Label
                tmpLabel.ID = String.Format("lblMenor{0}{1}", i, k)
                tmpLabel.Text = String.Format("{0} {1}:", "Menor", k)
                tmpLabel.CssClass = "clslabel"
                td.Controls.Add(tmpLabel)
                td.Style("padding-left") = "4px"
                td.Style("padding-right") = "8px"
                tr.Cells.Add(td)
            Next
            tr.Cells.Add(td)
            tblChilds.Rows.Add(tr)

            tr = New HtmlTableRow
            tr.ID = "TRCOLD" & i
            For k As Integer = 1 To iChilds
                td = New HtmlTableCell
                tmpText = New TextBox
                tmpText.ID = String.Format("txtMenor{0}{1}", i, k)
                If Not IsPostBack Then
                    tmpText.Text = ""
                Else
                    tmpText.Text = Request.Form(tmpText.ID)
                End If
                tmpText.CssClass = "textbox"
                tmpText.Width = New Unit(40, UnitType.Pixel)
                td.Controls.Add(tmpText)
                tr.Cells.Add(td)
            Next
            tr.Cells.Add(td)
            tblChilds.Rows.Add(tr)

        Next i
        Return True
    End Function

    Private Sub CargaRooms()
        ' Dim cad As String = rutaRequest(False)
        Dim i, j As Byte
        Dim cuartos As Byte = 10
        Dim MaxNinios As Byte = 10
        Dim MaxAdultos As Byte = 10
        Dim cmb As DropDownList
        Dim tr As HtmlTableRow
        Dim tc As HtmlTableCell
        Dim tblNiniosEdad As HtmlTable = New HtmlTable
        'Dim lc As LiteralControl
        'Dim vPersonas = Nothing

        tblRooms.Rows(1).Cells(1).InnerHtml = PortalCulture.GetString("00062") & "&nbsp;1:"
        tblRooms.Rows(1).Cells(1).Attributes("class") = "clslabel"
        cmb = CmbAdultos1

        cmb.Items.Clear()
        For j = 1 To MaxAdultos
            cmb.Items.Add(j)
            cmb.Items(cmb.Items.Count - 1).Value = j
        Next

        cmb = cmbChildren1
        cmb.Items.Clear()
        For j = 0 To MaxNinios
            cmb.Items.Add(j)
            cmb.Items(cmb.Items.Count - 1).Value = j
        Next
        cmb.Attributes.Add("onchange", "FireShowChildAges(1);")

        cmbRooms.Items.Clear()
        For i = 1 To cuartos
            cmbRooms.Items.Add(i)
            cmbRooms.Items(cmbRooms.Items.Count - 1).Value = i
        Next

        If Not IsPostBack Then
            cmbRooms.SelectedIndex = 0
            CmbAdultos1.SelectedIndex = 0
            cmbChildren1.SelectedIndex = 0

        Else
            If Not Request.Form(cmbRooms.ID) Is Nothing Then
                cmbRooms.SelectedValue = Request.Form(cmbRooms.ID)
            End If
            If Not Request.Form(CmbAdultos1.ID) Is Nothing Then
                CmbAdultos1.SelectedValue = Request.Form(CmbAdultos1.ID)
            End If
            If Not Request.Form(cmbChildren1.ID) Is Nothing Then
                cmbChildren1.SelectedValue = Request.Form(cmbChildren1.ID)

            End If
        End If

        For i = 2 To cuartos
            '//'''room y renglon''''''''''
            cmb = New DropDownList
            tr = New HtmlTableRow

            tr.ID = "TR" & i
            tblRooms.Rows.Add(tr)
            '//celda'' nueva que no llevan nada            
            tc = New HtmlTableCell
            tr.Cells.Add(tc)
            '//'//'//'//'''''''''''''''''''''''''''''''
            tc = New HtmlTableCell
            tr.Cells.Add(tc)

            tc.InnerHtml = PortalCulture.GetString("00062") & "&nbsp;" & i & ":"
            tc.Attributes("class") = "clslabel"
            tc.Align = "Right"
            '//''''''combo adultos'''''''''''''''''
            tc = New HtmlTableCell
            tc.Controls.Add(cmb)
            tc.Align = "Center"
            ''adultos''''''''''''''''''''''''''''''
            'cmb.Width = New Unit(40, UnitType.Pixel)
            cmb.ID = "CmbAdultos" & i

            For j = 1 To MaxAdultos
                cmb.Items.Add(j)
                cmb.Items(cmb.Items.Count - 1).Value = j
            Next

            tr.Cells.Add(tc)

            If Not IsPostBack Then
            Else
                If Not Request.Form(cmb.ID) Is Nothing Then
                    cmb.SelectedValue = Request.Form(cmb.ID)
                End If
            End If

            '//'''combo ninios'''''''''''''
            cmb = New DropDownList
            cmb.ID = "cmbChildren" & i
            cmb.Attributes.Add("onchange", String.Format("FireShowChildAges({0});", i))
            ' cmb.Width = New Unit(40, UnitType.Pixel)

            For j = 0 To MaxNinios
                cmb.Items.Add(j)
                cmb.Items(cmb.Items.Count - 1).Value = j
            Next

            tc = New HtmlTableCell
            tc.Controls.Add(cmb)
            tc.Align = "Center"
            tr.Cells.Add(tc)
            If Not IsPostBack Then
            Else
                If Not Request.Form(cmb.ID) Is Nothing Then
                    cmb.SelectedValue = Request.Form(cmb.ID)
                End If
            End If
        Next
        CreateChildAges(MaxNinios, cuartos)

        MeteEventos()
    End Sub

    Private Sub MeteEventos()
        cmbRooms.Attributes.Add("onchange", "cambiocuartos(this); if (parent.resizeIframe) parent.resizeIframe('iFrame')")
    End Sub

    Function GetChildAgesStr(ByVal Childrens As Integer, ByVal Ctrl As String) As String
        Dim StrChild As String = ""
        Dim iOld As Integer
        For i As Integer = 1 To Childrens
            If Not String.IsNullOrEmpty(Request.Form(Ctrl & i)) Then
                Integer.TryParse(Request.Form(Ctrl & i), iOld)
                StrChild &= "," & iOld.ToString
            End If
        Next
        Return If(Not String.IsNullOrEmpty(StrChild), (StrChild & ","), "")
    End Function

    Private Sub loaddatos()
        Dim xdoc As New XmlDataDocument(New reqHotelComplete)
        Dim dsreq As reqHotelComplete = CType(xdoc.DataSet, reqHotelComplete)
        Dim xml As resHotelCompPortal
        Dim strChildAges As String

        Dim drC As reqHotelComplete.CompleteAvailabilityRow
        drC = dsreq.CompleteAvailability.NewCompleteAvailabilityRow
        dsreq.CompleteAvailability.AddCompleteAvailabilityRow(drC)

        '//HotelHeader
        Dim drhh As reqHotelComplete.HotelHeaderRow
        drhh = dsreq.HotelHeader.NewHotelHeaderRow
        drhh.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
        drhh.CheckinDate = CDate(Me.txtInicio.Text).Year & CDate(Me.txtInicio.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtInicio.Text).Day.ToString.PadLeft(2, "0")
        drhh.CheckoutDate = CDate(Me.txtFinal.Text).Year & CDate(Me.txtFinal.Text).Month.ToString.PadLeft(2, "0") & CDate(Me.txtFinal.Text).Day.ToString.PadLeft(2, "0")
        drhh.Source = "CCT"
        drhh.Language = "es"

        'DirectCast(PortalCulture.GetCulture, System.Globalization.CultureInfo).Name
        If Not String.IsNullOrEmpty(txtAccessCode.Text) Then
            drhh.AccessCode = txtAccessCode.Text
        End If
        If Not String.IsNullOrEmpty(txtConvenio.Text) Then
            drhh.ReferenceContract = txtConvenio.Text
        End If

        dsreq.HotelHeader.AddHotelHeaderRow(drhh)
        drhh.SetParentRow(drC)

        '//HotelRequest
        Dim drhr As reqHotelComplete.HotelRequestsRow
        drhr = dsreq.HotelRequests.NewHotelRequestsRow

        dsreq.HotelRequests.AddHotelRequestsRow(drhr)
        drhr.SetParentRow(drC)

        '//Hotel
        Dim drh As reqHotelComplete.HotelRequestRow
        drh = dsreq.HotelRequest.NewHotelRequestRow
        drh.PropertyNumber = Me.cInfoActual.Hotel  '<************************

        dsreq.HotelRequest.AddHotelRequestRow(drh)
        drh.SetParentRow(drhr)

        Dim drhrs As reqHotelComplete.HotelRoomsRow
        drhrs = dsreq.HotelRooms.NewHotelRoomsRow

        dsreq.HotelRooms.AddHotelRoomsRow(drhrs)
        drhrs.SetParentRow(drC)

        '//Hotel
        'Dim cmb As DropDownList
        'Dim tr As HtmlTableRow
        Dim reserva As Reserva = New Reserva
        Dim r As room
        reserva.Checkin = CDate(Me.txtInicio.Text)
        reserva.Checkout = CDate(Me.txtFinal.Text)
        reserva.Rooms = New _Rooms
        For i As Integer = 1 To Me.cmbRooms.SelectedValue
            Dim rR As reqHotelComplete.RoomRow
            rR = dsreq.Room.NewRoomRow
            rR.Adults = Request.Form("cmbAdultos" & i)
            rR.Children = Request.Form("cmbChildren" & i)

            strChildAges = GetChildAgesStr(rR.Children, String.Format("txtMenor{0}", i))
            If Not String.IsNullOrEmpty(strChildAges) Then
                rR.ChildrenAges = strChildAges
            End If
            dsreq.Room.AddRoomRow(rR)
            rR.SetParentRow(drhrs)
            r = New room
            r.Adults = rR.Adults
            r.Childs = rR.Children

            reserva.Rooms.Add(r)
        Next
        'Me.Reservacion = reserva


        With New WSHotelFacade.clsFAHOC
            Try
                xml = .GetHotelCompPortal(xdoc.DocumentElement)
            Catch ex As Exception
                xml = Nothing
            End Try

        End With


        ResponseHOC = xml
        RequestHOC = dsreq
        If Not xml Is Nothing AndAlso Not xml.RoomType Is Nothing AndAlso xml.RoomType.Count > 0 Then
            Me.lblError.Visible = False
            ParseData(xml)
            'dgRooms.DataSource = xml.RoomType
        Else
            lblError.Visible = True
            'dgRooms.DataSource = Nothing
        End If
        'dgRooms.DataBind()


    End Sub
    Private Function GetBookitPage(ByVal RateCode As String) As String
        Dim Url As String = ""
        Dim strChildAges As String = ""
        Dim ichild As Integer
        Try
            Url += GeRequestApplicationPath("/CallCenter/Polities.aspx?")
            Url += "CheckIn=" & Me.txtInicio.Text.Substring(6, 4) & Me.txtInicio.Text.Substring(0, 2) & Me.txtInicio.Text.Substring(3, 2) & "&"
            Url += "CheckOut=" & Me.txtFinal.Text.Substring(6, 4) & Me.txtFinal.Text.Substring(0, 2) & Me.txtFinal.Text.Substring(3, 2) & "&"
            Url += "RateCode=" & RateCode & "&"
            Url += "Rooms=" & Me.cmbRooms.SelectedValue & "&"

            Dim adults As String = ""
            Dim Children As String = ""
            strChildAges = ""
            For i As Integer = 0 To Int(Me.cmbRooms.SelectedValue) - 1
                'Dim cmbA, cmbC, cmbAges As DropDownList
                adults += Request.Form("cmbAdultos" & (i + 1)) & ","
                Children += Request.Form("CmbChildren" & (i + 1)) & ","
                Integer.TryParse(Request.Form("CmbChildren" & (i + 1)), ichild)
                strChildAges &= GetChildAgesStr(ichild, String.Format("txtMenor{0}", (i + 1)))
                'cmbA.SelectedValue()
                'cmbC.SelectedValue()

            Next

            Url += "Adults=" & adults & "&"
            Url += "Children=" & Children & "&"
            If Not String.IsNullOrEmpty(strChildAges) Then
                strChildAges = strChildAges.Replace(",,", ",").Substring(1)
                If strChildAges.Substring(strChildAges.Length - 1, 1) = "," Then
                    strChildAges = strChildAges.Substring(0, strChildAges.Length - 1)
                End If
                Url += "Ages=" & strChildAges & "&"
            End If
            
            If Not String.IsNullOrEmpty(txtAccessCode.Text) Then
                Url += "AccessCode=" & txtAccessCode.Text & "&"
            End If
            If Not String.IsNullOrEmpty(txtConvenio.Text) Then
                Url += "Convenio=" & txtConvenio.Text & "&"
            End If

        Catch ex As Exception
        End Try
        Return Url
    End Function

    Private Sub ParseData(ByVal HOCRs As resHotelCompPortal)
        If Not HOCRs Is Nothing AndAlso HOCRs.RoomType.Rows.Count > 0 AndAlso HOCRs.RatePlan.Rows.Count > 0 Then
            '//tracker
            Dim Hdr As resHotelCompPortal.PropertyRow = HOCRs._Property.Rows(0)

            'Parsera las tarifas 


            Dim iSD As Date = CDate(Me.txtInicio.Text)
            Dim iED As Date = CDate(Me.txtFinal.Text)
            Dim Nights As Integer = DateDiff(DateInterval.Day, iSD, iED)
            Dim Dow As Date = iSD
            Dim Cols As Integer = Nights
            If Nights > 7 Then Cols = 7
            Dim Body As String = "<TABLE class='HotelRatesDetailsBox' style='width:100%'><THEAD><TR>"
            'Header.             
            Body += "<TD>" & PortalCulture.GetString("00072") & "</TD>"  'Room Type
            Body += "<TD></TD>" 'Wk. 
            Body += "<TD><Table  style='font-weight:normal !important;' width=100% cellspacing=0 border=0 ><TR>" 'Rates
            For i As Integer = 1 To Cols
                Body += "<TD  style='font-weight:normal !important;text-transform: capitalize;' width=" & 100 / Cols & "%>" & Dow.ToString("ddd", New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)) & "</TD>"   'Wk. 
                Dow = Dow.AddDays(1)
            Next
            Body += "</TR></Table></TD>" 'Rates
            Body += "<TD>" & PortalCulture.GetString("00685") & "</TD>"   'Average
            Body += "<TD></TD>" 'Currency
            Body += "<TD></TD>" 'Bookit
            Body += "</TR></THEAD>"
            'tarifas y mostrar rooms. 
            Dim Weeks As Integer = Nights \ 7
            If Nights Mod 7 <> 0 Then Weeks += 1
            Body += "<TBODY>"
            For Each rRoomType As resHotelCompPortal.RoomTypeRow In HOCRs.RoomType.Rows

                Dim drRatePlans() As DataRow = rRoomType.GetChildRows("RoomType_RatePlan")
                For Each drRatePlan As resHotelCompPortal.RatePlanRow In drRatePlans

                    Dim band As Boolean = False
                    Dim Total As Double = 0, ValidTotal As Boolean = True, messageError As String = ""
                    Dim Total2 As Double = 0
                    For i As Integer = 0 To Weeks - 1
                        Dim drRates() As DataRow = drRatePlan.GetChildRows("RatePlan_Price")
                        Dim nombre As String = ""
                        If Not rRoomType.IsRoomNameNull Then nombre = rRoomType.RoomName
                        If Not drRatePlan.IsPlanNameNull Then nombre &= "-" & drRatePlan.PlanName

                        Body &= "<TR>"

                        If i = 0 Then Body &= "<TD  class=HotelRatesListHightColor style='text-align:left' rowspan=" & Weeks & "><span>" & IIf(i = 0, nombre, "") & "</span> " '& _



                        Body &= "<TD class=HotelRatesListHightColor>" & IIf(Weeks > 1, PortalCulture.GetString("00683") & (i + 1).ToString, PortalCulture.GetString("00683") & "1") & "</TD>"
                        Body &= "<TD class=HotelRatesListHightColor><Table id='tblTest' width='100%' cellspacing='0px' border='0px' cellpadding='2px'><TR>" 'Rates

                        'for para los días
                        For j As Integer = 0 To Cols - 1
                            Dim FindRate As Boolean = False
                            Dim drPriceRow As resHotelCompPortal.PriceRow = Nothing
                            If Not drRatePlan.IsOnlyFirstDayNull AndAlso drRatePlan.OnlyFirstDay = 1 AndAlso (i * 7) + j < Nights Then
                                drPriceRow = CType(drRates(0), resHotelCompPortal.PriceRow)
                            ElseIf (i * 7) + j < Nights Then
                                For rates As Integer = 0 To drRates.Length - 1
                                    If CDate(CType(drRates(rates), resHotelCompPortal.PriceRow).Day) = iSD.AddDays((i * 7) + j) Then
                                        drPriceRow = CType(drRates(rates), resHotelCompPortal.PriceRow)
                                        Exit For
                                    End If
                                Next
                            End If
                            If Not drPriceRow Is Nothing AndAlso drRatePlan.Available = "Y" AndAlso drPriceRow.Total <> 0 Then
                                Dim subtotal As Double = CDbl(drPriceRow.Total)
                                Total2 += drPriceRow.Total
                                If subtotal = CDbl(drPriceRow.Total2) Then
                                    Total += subtotal
                                    Body &= "<TD width=" & 100 / Cols & "% class=HotelRatesListHightColor>" & IIf(Not drRatePlan.IsRatesVariationNull AndAlso drRatePlan.RatesVariation = 1 AndAlso (i * 7) + j > 0, "*" & CInt(subtotal), CInt(subtotal)) & "</TD>"
                                ElseIf drPriceRow.Total2 = 0 Then
                                    Body &= "<TD width=" & 100 / Cols & "% class=HotelRatesListHightColor><span class='PromoStrike'>" & drPriceRow.Total & "</span><BR><span class='PromotionSpecialRate'>" & PortalCulture.GetString("00684") & "</span></TD>"
                                    band = True
                                Else
                                    Body &= "<TD width=" & 100 / Cols & "% class=HotelRatesListHightColor><span class='PromoStrike'>" & subtotal & "</span><BR><span class='PromotionSpecialRate'>" & drPriceRow.Total2 & "</span></TD>"
                                    Total += CDbl(drPriceRow.Total2)
                                    band = True
                                End If
                            Else
                                If messageError = "" AndAlso Not drRatePlan.IsMessageNull Then messageError = getErrorMsg(drRatePlan)
                                If ((i * 7) + j < Nights) OrElse (Not drPriceRow Is Nothing AndAlso drPriceRow.Total) Then
                                    ValidTotal = False
                                    Body &= "<TD width=" & 100 / Cols & "% class=HotelRatesListHightColor>" & IIf((i * 7) + j < Nights, "X", "") & "</TD>"

                                End If

                            End If
                        Next
                        Body &= "</TR></Table></TD>" 'Rates
                        If i = 0 Then
                            Body &= "<TD class=HotelRatesListHightColor rowspan=" & Weeks & ">" & IIf(ValidTotal, "AVGRATE", "NA") & "</TD>" 'Average
                            Body &= "<TD class=HotelRatesListHightColor rowspan=" & Weeks & ">" & IIf(ValidTotal, getCurrencyCode(drRatePlan), "") & "</TD>" 'Currency
                            Body &= "<TD class=HotelRatesListHightColor rowspan =" & Weeks & ">"
                            If ValidTotal Then
                                'Body += "<a id=" & drRatePlan.PlanCode & " href='" & GetBookitPage(drRatePlan.PlanCode, Hdr.CityCode, Hdr.ChainCode, Hdr.ServiceProvider, Hdr.PropertyNumber) & "'>" & PortalCulture.GetString("00347") & "</a>"
                                Body += "<a id=" & drRatePlan.PlanCode & " href='" & GetBookitPage(drRatePlan.PlanCode) & "'>" & PortalCulture.GetString("00347") & "</a>"
                            Else
                                Body &= IIf(messageError.IndexOf("-"), messageError.Substring(messageError.IndexOf("-") + 1), messageError)
                            End If
                            Body &= "</TD>"

                        End If




                        Body &= "</TR>"
                    Next
                    If Not band Then
                        Body = Body.Replace("AVGRATE", FCurrency((Total / Nights), 2))
                    Else
                        Body = Body.Replace("AVGRATE", "<span class='PromoStrike'>" + FCurrency((Total2 / Nights), 2) + "</span><BR><span class='PromotionSpecialRate'>" & FCurrency((Total / Nights), 2) & "</span>")
                    End If

                Next
            Next

            Body &= "</TBODY>"
            Body &= "</TABLE>"
            Me.HotelRatesList.InnerHtml = Body


        Else
            'Error en despliege de tarifas pon no disponibilidad y habilita el search
            'HotelDetailsCheckAvailabilityBox.Style.Add("display", "block")
            lblError.Visible = True

        End If

    End Sub
    Private Function getCurrencyCode(ByVal Rate As resHotelCompPortal.RatePlanRow) As String
        Try
            If Not Rate.IsCurrencyNull Then
                Return Rate.Currency
            Else
                Return Rate.RoomTypeRow.PropertyRow.Currency
            End If
        Catch
        End Try
        Return ""
    End Function


   

    Public Shared Function GetErrorMessage(ByVal dr As resHotelCompPortal.RatePlanRow) As String
        Try
            Dim errtype As ErrorType = dr.Message.Split("-")(0)
            Dim sCodigo As String = dr.Message.Split("-")(0)
            Dim sMessage As String = dr.Message.Split("-")(1)
            Dim sDescription As String

            Select Case errtype
                Case ErrorType.AdvBooking
                    sDescription = String.Format(PortalCulture.GetString("01479"), dr.AdvBooking)
                Case ErrorType.Close
                    sDescription = PortalCulture.GetString("01476")   ' " Close Dates"
                Case ErrorType.MaxStay
                    sDescription = String.Format(PortalCulture.GetString("01477"), dr.MaxDays)   ' " Max Stay Restricted"
                Case ErrorType.MinStay
                    sDescription = String.Format(PortalCulture.GetString("01478"), dr.MinDays)   ' " Min Stay Restricted"
                Case ErrorType.NoArrival
                    sDescription = PortalCulture.GetString("01480")   ' " No Arrival Restricted"
                Case ErrorType.NoAvailability
                    sDescription = PortalCulture.GetString("00532")   ' " No Availability"
                Case ErrorType.MaxPeople
                    sDescription = PortalCulture.GetString("01481")   ' " No Exceede Max People"
                Case ErrorType.RateRestrictedSource
                    sDescription = PortalCulture.GetString("01500")   ' " Restricted Rate Source"
                Case ErrorType.RateNoFound
                    sDescription = PortalCulture.GetString("01483")   ' " Rate No Found"
                Case ErrorType.InvalidCreditCard
                    sDescription = PortalCulture.GetString("00635")   ' " Invalid Credit Card"
                Case ErrorType.ReservationNoFound
                    sDescription = PortalCulture.GetString("01484")   ' " Reservation No Found"
                Case ErrorType.CreditCardNoAccept
                    sDescription = PortalCulture.GetString("01485")   ' " No Accept Credit Card"
                Case ErrorType.NoTransaction
                    sDescription = PortalCulture.GetString("01486")   ' " Transaction Not Executed"
                Case ErrorType.CancelPrior
                    sDescription = PortalCulture.GetString("01487")   ' " Cancel Prior Restricted"
                Case ErrorType.InvalidCDNumber
                    sDescription = PortalCulture.GetString("01501")   ' " Invalid CD Number"
                Case ErrorType.InvalidRollAwayAdult
                    sDescription = PortalCulture.GetString("01502")   ' " Invalid Roll Away Adult"
                Case ErrorType.InvalidRollAwayChild
                    sDescription = PortalCulture.GetString("01503")   ' " Invalid Roll Away Child"
                Case ErrorType.InvalidRollAwayCrib
                    sDescription = PortalCulture.GetString("01504")   ' " Invalid Roll Away Crib"
                Case ErrorType.ExccedMaxNumberRooms
                    sDescription = PortalCulture.GetString("01482")   ' " Max Excced Rooms Number"
                Case ErrorType.RequiredDep
                    sDescription = PortalCulture.GetString("01488")   ' " Required Deposit"
                Case ErrorType.RequiredGuar
                    sDescription = PortalCulture.GetString("01489")   ' " Required Guarantee"
                Case ErrorType.NotAllowBankDeposit
                    sDescription = PortalCulture.GetString("01490")   ' " This Hotel doesn't allow Bank Deposit"
                Case ErrorType.InvalidAccessCode
                    sDescription = PortalCulture.GetString("01491")   ' " Invalid access code"
                Case ErrorType.ModifyRestricted
                    sDescription = PortalCulture.GetString("01505")   ' " Modify Prior Restricted"
                Case ErrorType.SessionIdRequired
                    sDescription = PortalCulture.GetString("01492")   ' " Session Id Required"
                Case ErrorType.SessionIdNoFound
                    sDescription = PortalCulture.GetString("01493")   ' " Reservation with this session wasn't Found"
                Case ErrorType.InvalidCreditCardExpDate
                    sDescription = PortalCulture.GetString("01494")   ' " Invalid Credit Card Expiration Date"
                Case ErrorType.InvalidDates
                    sDescription = PortalCulture.GetString("00110")   ' " Invalid Dates"
                Case ErrorType.InvalidFrecuentCode
                    sDescription = PortalCulture.GetString("01496")   ' " Invalid Frecuent Code"
                Case ErrorType.PMS
                    sDescription = sMessage   ' " PMS"
                Case ErrorType.RestrictedRate
                    sDescription = PortalCulture.GetString("01495")   ' " Restricted Rate"
                Case ErrorType.MinNumberRoomsRestriced
                    sDescription = PortalCulture.GetString("01497")   ' " Min Number Rooms Restricted"
                Case ErrorType.MinPeople
                    sDescription = PortalCulture.GetString("01498")   ' " Min Number of person Restricted"
                Case ErrorType.StatusIsNotOnRequest
                    sDescription = PortalCulture.GetString("01499")   ' " Reservation is not on request status"
                Case Else
            End Select
            Return String.Format("{0} - {1}", sCodigo.Trim, sDescription.Trim)

        Catch ex As Exception
            Return dr.Message
        End Try
    End Function

    Private Function getErrorMsg(ByVal dr As resHotelCompPortal.RatePlanRow) As String
        Try
            Return GetErrorMessage(dr)
            'Return dr.Message
        Catch ex As Exception
        End Try
        Return dr.Message
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        loaddatos()
    End Sub

    Private Sub dgRooms_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRooms.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim FILLDIV As Boolean = True
            Dim btn As Button, lbl As Label
            btn = e.Item.FindControl("btnBookit")
            lbl = e.Item.FindControl("lblNA")
            Dim dv As HtmlGenericControl
            Dim a As HtmlAnchor
            a = e.Item.Cells(dgcolumns.ShowDetails).FindControl("AnclaTarifas")
            Dim lb1 As Label = e.Item.Cells(dgcolumns.Total).FindControl("lblPrice1")
            Dim lb2 As Label = e.Item.Cells(dgcolumns.Total).FindControl("lblPrice2")
            Dim lb3 As Label = e.Item.Cells(dgcolumns.Total).FindControl("lblPrice3")
            dv = e.Item.Cells(dgcolumns.ShowDetails).FindControl("divtarifas")

            a.InnerHtml = PortalCulture.GetString("00376")
            Dim moneda As String = "(" & e.Item.Cells(dgcolumns.currency).Text & ")"
            If moneda = "(&nbsps;)" Then moneda = ""
            a.Attributes.Add("onclick", "javascript:ShowDivDetails('" & dv.ClientID & "')")
            If e.Item.Cells(dgcolumns.Availability).Text.ToUpper = "Y" Then
                If Not dv Is Nothing Then
                    Dim STR As String
                    STR = Me.GetTable(e.Item.Cells(dgcolumns.RateCode).Text, lb1, lb2, lb3, moneda)
                    If STR = "" Then
                        FILLDIV = True
                    Else
                        FILLDIV = False
                        dv.InnerHtml = STR
                        Dim v1, v2, v3 As Double
                        Dim c1, c2, c3 As String
                        c1 = vbNullString
                        c2 = vbNullString
                        c3 = vbNullString
                        v1 = CDbl(Val(lb1.Text))
                        v2 = CDbl(Val(lb2.Text))
                        v3 = CDbl(Val(lb3.Text))
                        If lb1.Text.IndexOf("*") > 0 Then
                            v1 = lb1.Text.Substring(0, lb1.Text.Length - 1)
                            c1 = "*"
                        End If
                        If lb2.Text.IndexOf("*") > 0 Then
                            v2 = lb2.Text.Substring(0, lb2.Text.Length - 1)
                            c2 = "*"
                        End If
                        If lb3.Text.IndexOf("*") > 0 Then
                            c3 = "*"
                            v3 = lb3.Text.Substring(0, lb3.Text.Length - 1)
                        End If

                        Select Case Me.cmbRooms.SelectedValue
                            Case 1
                                lb2.Visible = False
                                lb3.Visible = False
                                lb1.Text = FCurrency(v1, 2) & c1 & " " & moneda
                            Case 2
                                lb3.Visible = False

                                If v1 = v2 Then
                                    lb2.Visible = False
                                    lb1.Text = FCurrency(v1, 2) & c1 & " " & moneda
                                Else
                                    lb1.Text = Request.Form("cmbAdultos1") & "(ad) " & Request.Form("cmbChildren1") & "(chd) " & FCurrency(v1, 2) & c1 & " " & moneda
                                    lb2.Text = Request.Form("cmbAdultos2") & "(ad) " & Request.Form("cmbChildren2") & "(chd) " & FCurrency(v2, 2) & c2 & " " & moneda
                                End If
                            Case 3
                                If v1 = v2 AndAlso v1 = v3 Then
                                    lb1.Text = FCurrency(v1, 2) & c1 & " " & moneda
                                    lb2.Visible = False
                                    lb3.Visible = False
                                Else
                                    lb1.Text = Request.Form("cmbAdultos1") & "(ad) " & Request.Form("cmbChildren1") & "(chd) " & FCurrency(v1, 2) & c1 & " " & moneda
                                    lb2.Text = Request.Form("cmbAdultos2") & "(ad) " & Request.Form("cmbChildren2") & "(chd) " & FCurrency(v2, 2) & c2 & " " & moneda
                                    lb3.Text = Request.Form("cmbAdultos3") & "(ad) " & Request.Form("cmbChildren3") & "(chd) " & FCurrency(v3, 2) & c3 & " " & moneda
                                End If
                        End Select
                    End If
                Else
                    FILLDIV = True
                End If
            Else
                FILLDIV = True
            End If
            If FILLDIV = True Then
                lb1.Text = "X"
                lb2.Visible = False
                lb3.Visible = False
                dv.InnerHtml = "<table cellspacing='0' width='99%' class='datagrid' align='center'>"
                dv.InnerHtml += "<tr class='dgitem'>"
                Dim mensaje As String = e.Item.Cells(dgcolumns.Message).Text.Substring(e.Item.Cells(dgcolumns.Message).Text.IndexOf("-") + 1)
                If mensaje = "&nbsp;" Then
                    mensaje = PortalCulture.GetString("00373")
                End If
                dv.InnerHtml += "   <td  align='center' colspan='5' class='bookingNormalLabel'>" & mensaje & "</td>"
                dv.InnerHtml += "   <td  align='right' >"
                dv.InnerHtml += "   <a id='Img' style=""CURSOR: pointer"" onclick=""javascript:NoShowDivDetails();""><img src='../Images/close.png' alt='" & PortalCulture.GetString("00151") & "'></a></td>"
                dv.InnerHtml += "</tr>"
                dv.InnerHtml += "</table>"
            End If
            If Not btn Is Nothing Then
                btn.Text = PortalCulture.GetString("00347")
                btn.Visible = True
                lbl.Visible = False
                If e.Item.Cells(dgcolumns.Availability).Text.ToUpper = "N" Or FILLDIV = True Then
                    btn.Visible = False
                    lbl.Visible = True
                    If e.Item.Cells(dgcolumns.Message).Text <> "&nbsp;" Then
                        lbl.Text = e.Item.Cells(dgcolumns.Message).Text.Substring(e.Item.Cells(dgcolumns.Message).Text.IndexOf("-") + 1)
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CargaRooms()
        lblAdultos.Text = PortalCulture.GetString("00060")
        lblChild.Text = PortalCulture.GetString("00061")
        lblRooms.Text = PortalCulture.GetString("00141")
        btnSearch.Text = PortalCulture.GetString("M0BT0000115")
        lblTitle.Text = PortalCulture.GetString("00367")
        lblCheckin.Text = PortalCulture.GetString("00368", True)
        lblCheckOut.Text = PortalCulture.GetString("00369", True)
        lblPaso1.Text = PortalCulture.GetString("00370")
        lblpaso2.Text = PortalCulture.GetString("00371")
        lblRooms.Text = PortalCulture.GetString("00141")
        lblAdultos.Text = PortalCulture.GetString("M000165")
        lblChild.Text = PortalCulture.GetString("M000166")
        lblRoom1.Text = PortalCulture.GetString("M000439") & " 1"
        lblpaso3.Text = PortalCulture.GetString("00372")
        Me.lblError.Text = PortalCulture.GetString("00430")
        btnSearch.Text = PortalCulture.GetString("M0BT0000115")
        lblConvenio.Text = PortalCulture.GetString("01426", True)
        lblAccessCode.Text = PortalCulture.GetString("01425", True)
        lblErrorEdades.Text = PortalCulture.GetString("01510")
        Me.dgRooms.Columns(dgcolumns.NameRoomType).HeaderText = PortalCulture.GetString("M000439")
        Me.dgRooms.Columns(dgcolumns.RatePlanName).HeaderText = PortalCulture.GetString("00016")
        Me.dgRooms.Columns(dgcolumns.Total).HeaderText = PortalCulture.GetString("00375")
    End Sub
    Protected Function GetTable(ByVal rcode As String, ByVal lb1 As Label, ByVal lb2 As Label, ByVal lb3 As Label, ByVal moneda As String) As String
        Dim s2 As String = "img"
        Dim n As Integer ', inicio As String exc As String,r As resHotelComplete.RateRow dvRate As DataView, dvRoom As DataView
        'Dim rRate As resHotelComplete.PriceRow
        Dim dvPrice As DataView
        Dim Room() As resHotelComplete.RoomRow

        n = 0
        Dim i As Integer = 0
        Dim sw As Boolean = True

        If Me.ResponseHOC Is Nothing Then Return ""
        dvPrice = ResponseHOC.Price.DefaultView
        dvPrice.RowFilter = "RateCode='" & rcode & "'"
        Dim drroom As resHotelComplete.RoomTypeRow
        For Each rRoom As DataRow In ResponseHOC.RoomType.Rows
            If rRoom("RateCode") = rcode Then
                drroom = rRoom
                Exit For
            End If
        Next


        'Dim rooms As DataRelation
        Room = drroom.GetRoomsRows(0).GetRoomRows ' Table.ChildRelations("RoomType_Rooms")
        'rooms = rooms.ChildTable.ChildRelations("Rooms_Room")

        If dvPrice.Count = 0 Then Return ""

        Dim body As String = String.Empty
        body += "<table cellspacing='0' width='99%' class='datagrid' align='center'>"
        body += "<tr class='dgitem'>"
        body += "   <td  align='center' colspan='5' class='bookingNormalLabel'>" & PortalCulture.GetString("00348") & " " & rcode & " " & moneda
        body += "   </td>"
        body += "   <td  align='right' >"
        body += "   <a id='" & s2 & "' style=""CURSOR: pointer"" onclick=""javascript:NoShowDivDetails();""><img src='../Images/close.png' alt='" & PortalCulture.GetString("00151") & "'></a></td>"
        body += "</tr>"

        body += "<tr class='bookingNormalLabel'  ><td  align='center' ></td>"
        body += "   <td  align='center' >" & PortalCulture.GetString("00060") & "</td>"
        body += "   <td  align='center' >" & PortalCulture.GetString("00061") & "</td>"
        body += "   <td  align='right' >"
        body += PortalCulture.GetString("00428") & "</td>"
        body += "   <td  align='right' >"
        body += PortalCulture.GetString("00429") & "</td>"
        body += "   <td  align='right' >"
        body += "   </td>"
        body += "</tr>"

        'While i <= DateDiff(DateInterval.Day, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text))
        '    dvRate = ResponseHOC.Rate.DefaultView
        '    dvRate.RowFilter = " convert(date,System.DateTime) ='" & CDate(Me.txtInicio.Text).AddDays(i) & "'" & "and RateCode='" & rcode & "'"
        '    If dvRate.Count > 0 Then
        '        If n = 0 Then
        '            exc = dvRate(0)("Exc")
        '            n = 1
        '            body += "<tr class='bookingNormalLabel'><td  align='left' colspan='6'>" & CDate(Me.txtInicio.Text).AddDays(i) & "</td></tR>"
        '            sw = False
        '        Else
        '            If exc = dvRate(0)("Exc") Then
        '                n += 1
        '            Else
        '                writeDatosTabla(body, lb1, lb2, lb3, dvPrice, exc, n, rcode, Room)
        '                n = 0
        '                exc = dvRate(0)("Exc")
        '                i -= 1
        '                sw = True
        '            End If
        '        End If
        '    End If
        '    i += 1
        'End While

        'If sw = False Then
        '    writeDatosTabla(body, lb1, lb2, lb3, dvPrice, exc, n, rcode, Room)
        'End If
        body += "</table>"

        Return body
    End Function
    'Protected Function GetTable(ByVal rcode As String, ByVal lb1 As Label, ByVal lb2 As Label, ByVal lb3 As Label, ByVal moneda As String) As String
    '    Dim s2 As String = "img"
    '    Dim r As resHotelComplete.RateRow, exc As String, n As Integer, inicio As String
    '    Dim rRate As resHotelComplete.PriceRow
    '    Dim dvPrice As DataView, dvRate As DataView, dvRoom As DataView
    '    Dim Room() As resHotelComplete.RoomRow

    '    n = 0
    '    Dim i As Integer = 0
    '    Dim sw As Boolean = True

    '    If Me.ResponseHOC Is Nothing Then Return ""
    '    dvPrice = ResponseHOC.Price.DefaultView
    '    dvPrice.RowFilter = "RateCode='" & rcode & "'"
    '    Dim drroom As resHotelComplete.RoomTypeRow
    '    For Each rRoom As DataRow In ResponseHOC.RoomType.Rows
    '        If rRoom("RateCode") = rcode Then
    '            drroom = rRoom
    '            Exit For
    '        End If
    '    Next


    '    'Dim rooms As DataRelation
    '    Room = drroom.GetRoomsRows(0).GetRoomRows ' Table.ChildRelations("RoomType_Rooms")
    '    'rooms = rooms.ChildTable.ChildRelations("Rooms_Room")

    '    If dvPrice.Count = 0 Then Return ""

    '    Dim body As String = String.Empty
    '    body += "<table cellspacing='0' width='99%' class='datagrid' align='center'>"
    '    body += "<tr class='dgitem'>"
    '    body += "   <td  align='center' colspan='5' class='bookingNormalLabel'>" & PortalCulture.GetString("00348") & " " & rcode & " " & moneda
    '    body += "   </td>"
    '    body += "   <td  align='right' >"
    '    body += "   <a id='" & s2 & "' style=""CURSOR: pointer"" onclick=""javascript:NoShowDivDetails();""><img src='../Images/close.png' alt='" & PortalCulture.GetString("00151") & "'></a></td>"
    '    body += "</tr>"

    '    body += "<tr class='bookingNormalLabel'  ><td  align='center' ></td>"
    '    body += "   <td  align='center' >" & PortalCulture.GetString("00060") & "</td>"
    '    body += "   <td  align='center' >" & PortalCulture.GetString("00061") & "</td>"
    '    body += "   <td  align='right' >"
    '    body += PortalCulture.GetString("00428") & "</td>"
    '    body += "   <td  align='right' >"
    '    body += PortalCulture.GetString("00429") & "</td>"
    '    body += "   <td  align='right' >"
    '    body += "   </td>"
    '    body += "</tr>"

    '    While i <= DateDiff(DateInterval.Day, CDate(Me.txtInicio.Text), CDate(Me.txtFinal.Text))
    '        dvRate = ResponseHOC.Rate.DefaultView
    '        dvRate.RowFilter = " convert(date,System.DateTime) ='" & CDate(Me.txtInicio.Text).AddDays(i) & "'" & "and RateCode='" & rcode & "'"
    '        If dvRate.Count > 0 Then
    '            If n = 0 Then
    '                exc = dvRate(0)("Exc")
    '                n = 1
    '                body += "<tr class='bookingNormalLabel'><td  align='left' colspan='6'>" & CDate(Me.txtInicio.Text).AddDays(i) & "</td></tR>"
    '                sw = False
    '            Else
    '                If exc = dvRate(0)("Exc") Then
    '                    n += 1
    '                Else
    '                    writeDatosTabla(body, lb1, lb2, lb3, dvPrice, exc, n, rcode, Room)
    '                    n = 0
    '                    exc = dvRate(0)("Exc")
    '                    i -= 1
    '                    sw = True
    '                End If
    '            End If
    '        End If
    '        i += 1
    '    End While

    '    If sw = False Then
    '        writeDatosTabla(body, lb1, lb2, lb3, dvPrice, exc, n, rcode, Room)
    '    End If
    '    body += "</table>"

    '    Return body
    'End Function

    Private Sub writeDatosTabla(ByRef body As String, ByVal lb1 As Label, ByVal lb2 As Label, ByVal lb3 As Label, ByVal dv4 As DataView, ByVal exc As String, ByVal n As Integer, ByVal rc As String, ByVal rooms() As resHotelComplete.RoomRow)
        For ocupancy As Integer = 1 To Me.cmbRooms.SelectedValue

            Dim etiqueta As Label
            Select Case ocupancy
                Case 1
                    etiqueta = lb1
                Case 2
                    etiqueta = lb2
                Case 3
                    etiqueta = lb3
            End Select
            dv4.RowFilter = "Adults =" & rooms(ocupancy - 1).Item("Adults") & "And Children=" & rooms(ocupancy - 1).Item("Children") & "and ratecode='" & rc & "'"

            If dv4.Count > 0 Then
                body += "   <tr class='clslabel'>"
                body += "   <td  align='center' >" & dv4(0)("Adults") & " Ad. " & dv4(0)("Children") & " Chd"
                body += "   </td>"
                If exc.ToUpper = "N" Then
                    body += "   <td  align='center' >" & FCurrency(dv4(0)("RateAdult"), 2) & "</td>"
                    body += "   <td  align='center' >" & FCurrency(dv4(0)("RateChild"), 2) & "</td>"
                    If etiqueta.Text = "" Then
                        etiqueta.Text = CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))
                    Else
                        If etiqueta.Text.IndexOf("*") > 0 Then
                            If (CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))) <> CDbl(etiqueta.Text.Substring(0, etiqueta.Text.Length - 1)) Then
                                If (CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))) < CDbl(etiqueta.Text.Substring(0, etiqueta.Text.Length - 1)) Then
                                    etiqueta.Text = CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))
                                End If
                            End If
                        Else
                            If (CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))) <> CDbl(etiqueta.Text) Then
                                If (CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))) < CDbl(etiqueta.Text) Then
                                    etiqueta.Text = CDbl(dv4(0)("RateAdult")) + CDbl(dv4(0)("RateChild"))
                                End If
                                etiqueta.Text &= "*"
                            End If
                        End If
                    End If
                Else
                    body += "   <td  align='center' >" & FCurrency(dv4(0)("RateAdultExc"), 2) & "</td>"
                    body += "   <td  align='center' >" & FCurrency(dv4(0)("RateChildExc"), 2) & "</td>"
                    If etiqueta.Text = "" Then
                        etiqueta.Text = CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))
                    Else
                        If etiqueta.Text.IndexOf("*") > 0 Then
                            If (CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))) <> CDbl(etiqueta.Text.Substring(0, etiqueta.Text.Length - 1)) Then
                                If (CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))) < CDbl(etiqueta.Text.Substring(0, etiqueta.Text.Length - 1)) Then
                                    etiqueta.Text = CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))
                                End If
                            End If
                        Else
                            If (CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))) <> CDbl(etiqueta.Text) Then
                                If (CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))) < CDbl(etiqueta.Text) Then
                                    etiqueta.Text = CDbl(dv4(0)("RateAdultExc")) + CDbl(dv4(0)("RateChildExc"))
                                End If
                                etiqueta.Text &= "*"
                            End If
                        End If
                    End If
                End If

                body += "   <td  align='center' >" & FCurrency(dv4(0)("ExtraRateAdult") * rooms(ocupancy - 1).Item("ExtraAdults"), 2)
                body += "   </td>"
                body += "   <td  align='center' >" & FCurrency(dv4(0)("ExtraRateChild") * rooms(ocupancy - 1).Item("ExtraChildren"), 2)
                body += "   </td>"

                body += "   <td  align='center' >" & n & PortalCulture.GetString("00374") & "</td>"
                body += "   </tr>"
            End If
        Next
    End Sub

    'Private Sub dgRooms_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRooms.ItemCommand
    '    Try
    '        Dim responsehor As resHotelRules
    '        For Each r As room In Me.Reservacion.Rooms
    '            r.RateCode = e.Item.Cells(dgcolumns.RateCode).Text
    '        Next
    '        Response.Redirect( "/CallCenter/Polities.aspx")
    '    Catch ex As Exception
    '    End Try
    'End Sub
End Class

