Imports WSHotelFacade
Imports WSHotelCommon
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Text
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports System.Globalization
Imports System.IO
Imports System.Configuration.ConfigurationManager

Partial Class Polities
    Inherits PaginaBase
    Private Property ResponseHOR() As resHotelRules
        Get
            Return ViewState("_ResponseHOR")
        End Get
        Set(ByVal Value As resHotelRules)
            ViewState("_ResponseHOR") = Value
        End Set
    End Property
    Private Property checkIn() As String
        Get
            Return ViewState("_checkin")
        End Get
        Set(ByVal Value As String)
            ViewState("_checkin") = Value
        End Set
    End Property
    Private Property checkOut() As String
        Get
            Return ViewState("_checkOut")
        End Get
        Set(ByVal Value As String)
            ViewState("_checkOut") = Value
        End Set
    End Property
    Private Property RateCode() As String
        Get
            Return ViewState("_RateCode")
        End Get
        Set(ByVal Value As String)
            ViewState("_RateCode") = Value
        End Set
    End Property
    Private Property totalRooms() As Double
        Get
            Return ViewState("_Rooms")
        End Get
        Set(ByVal Value As Double)
            ViewState("_Rooms") = Value
        End Set
    End Property
    Private Property Adults() As String
        Get
            Return ViewState("_Adults")
        End Get
        Set(ByVal Value As String)
            ViewState("_Adults") = Value
        End Set
    End Property
    Private Property Children() As String
        Get
            Return ViewState("_Children")
        End Get
        Set(ByVal Value As String)
            ViewState("_Children") = Value
        End Set
    End Property

    Private Property AccessCode() As String
        Get
            Return ViewState("_AccessCode")
        End Get
        Set(ByVal Value As String)
            ViewState("_AccessCode") = Value
        End Set
    End Property
    Private Property Convenio() As String
        Get
            Return ViewState("_Convenio")
        End Get
        Set(ByVal Value As String)
            ViewState("_Convenio") = Value
        End Set
    End Property

    Private Property IsNetRate() As Boolean
        Get
            Return ViewState("_IsNetRate")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("_IsNetRate") = Value
        End Set
    End Property

    Private Property NoReservacion() As String
        Get
            Return ViewState("_NoReservacion")
        End Get
        Set(ByVal Value As String)
            ViewState("_NoReservacion") = Value
        End Set
    End Property

    Private Property Ages() As String
        Get
            Return ViewState("_Ages")
        End Get
        Set(ByVal Value As String)
            ViewState("_Ages") = Value
        End Set
    End Property

    Private Enum rows
        adults
        child
        exAd
        exCh
        nights
    End Enum
    Private arrRates As ArrayList

    Public Shared Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function

    ' Public sourceXML() As String = AppSettings.GetValues("rutaXML")
    'Public sourceXML As String = Replace(Server.MapPath("Portal\PortalPartners.xml"), "CallCenter\", "")

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
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim dt As DateTime

        If Not IsPostBack Then
            totalRooms = CDbl(Request.QueryString("Rooms"))
            Adults = Request.QueryString("Adults")
            Children = Request.QueryString("children")
            checkIn = Request.QueryString("CheckIn")
            checkOut = Request.QueryString("CheckOut")
            RateCode = Request.QueryString("RateCode")
            Ages = Request.QueryString("Ages")

            lblConvenio.Visible = False
            lblCodigo.Visible = False
            If Not String.IsNullOrEmpty(Request.QueryString("AccessCode")) Then
                lblCodigo.Visible = True
                AccessCode = Request.QueryString("AccessCode")
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("Convenio")) Then
                lblConvenio.Visible = True
                Convenio = Request.QueryString("Convenio")
            End If
            If Not String.IsNullOrEmpty(Ages) Then
                txtEdadMenor.Text = Ages
            Else
                txtEdadMenor.Visible = False
                lblEdadMenor.Visible = False
            End If

            dt = New Date(checkIn.Substring(0, 4), checkIn.Substring(4, 2), checkIn.Substring(6, 2))
            pnlCredito.Style.Add("display", "")
            pnlDeposito.Style.Add("display", "none")
            'rbTc.Attributes.Add("onclick", String.Format("FireDeposito('{0}','{1}','{2}', true);", pnlCredito.ClientID, pnlDeposito.ClientID, rbTc.ClientID))
            'rbDeposito.Attributes.Add("onclick", String.Format("FireDeposito('{0}','{1}','{2}', false);", pnlDeposito.ClientID, pnlCredito.ClientID, rbDeposito.ClientID))

            'If Not Me.Reservacion Is Nothing Then
            ResponseHOR = GetHOR()
            'End If

            Me.FillPortals(RateCode)
        End If
        If rbDeposito.Checked Then
            pnlCredito.Style.Add("display", "none")
            pnlDeposito.Style.Add("display", "")
        Else
            pnlCredito.Style.Add("display", "")
            pnlDeposito.Style.Add("display", "none")
        End If
        If dt.Date <= Now.Date.AddHours(48) Then
            btnReservar.OnClientClick = String.Format("return FireValidaFechaDeposito('{0}', '{1}');", rbDeposito.ClientID, PortalCulture.GetString("01433"))
        End If

    End Sub

    Function GetChildrenAges(ByVal sAges As String, ByVal index As Integer, ByVal count As Integer) As String
        Dim strAges As String = ""
        If sAges.Split(",").Length >= count Then
            For x As Integer = index To (index + count) - 1
                strAges &= "," & sAges.Split(",")(x)
            Next
        End If
        Return (strAges & ",")
    End Function

    Private Function GetHOR() As resHotelRules
        Dim xdoc As New XmlDataDocument(New reqHotelRules)
        Dim dsreq As reqHotelRules = CType(xdoc.DataSet, reqHotelRules)
        Dim xml As New resHotelRules
        Dim Index As Integer = 0

        Dim drR As reqHotelRules.HotelRulesRow
        drR = dsreq.HotelRules.NewHotelRulesRow
        dsreq.HotelRules.AddHotelRulesRow(drR)

        '//HotelHeader
        Dim drhh As reqHotelRules.HotelHeaderRow
        drhh = dsreq.HotelHeader.NewHotelHeaderRow

        drhh.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
        drhh.CheckinDate = checkIn
        drhh.CheckoutDate = checkOut
        drhh.RateCode = RateCode
        drhh.Source = "CCT"
        drhh.PropertyNumber = Me.cInfoActual.Hotel
        If Not String.IsNullOrEmpty(AccessCode) Then drhh.AccessCode = AccessCode
        If Not String.IsNullOrEmpty(Convenio) Then drhh.ReferenceContract = Convenio

        dsreq.HotelHeader.AddHotelHeaderRow(drhh)
        drhh.SetParentRow(drR)

        '//rooms
        Dim drhrs As reqHotelRules.HotelRoomsRow
        drhrs = dsreq.HotelRooms.NewHotelRoomsRow
        dsreq.HotelRooms.AddHotelRoomsRow(drhrs)
        drhrs.SetParentRow(drR)

        For i As Integer = 0 To totalRooms - 1
            Dim rR As reqHotelRules.RoomRow
            rR = dsreq.Room.NewRoomRow

            If Adults <> "" Then
                If Adults.Split(",").Length > i - 1 Then
                    rR.Adults = Adults.Split(",")(i)
                End If
            End If
            If Children <> "" Then
                If Children.Split(",").Length > i - 1 Then
                    rR.Children = Children.Split(",")(i)
                End If
            End If

            If Not String.IsNullOrEmpty(Ages) Then
                rR.ChildrenAges = GetChildrenAges(Ages, Index, rR.Children)
                Index = rR.Children
            End If

            Dim dr As New HtmlTableRow
            Dim td As HtmlTableCell
            td = New HtmlTableCell
            td.Width = "40%"
            Dim lbl As New Label
            lbl.ID = "lblSpecialReq" & (i + 1).ToString
            lbl.Text = PortalCulture.GetString("00170") & " " & (i + 1).ToString & ":"
            td.Controls.Add(lbl)
            dr.Cells.Add(td)

            td = New HtmlTableCell
            Dim txt As New TextBox
            txt.ID = "txtSpecialReq" & i.ToString
            txt.TextMode = TextBoxMode.MultiLine
            txt.Width = Unit.Percentage(90)
            td.Controls.Add(txt)
            dr.Cells.Add(td)
            divpeticiones.Rows.Add(dr)


            dsreq.Room.AddRoomRow(rR)
            rR.SetParentRow(drhrs)
        Next


        Try
            With New WSHotelFacade.clsFAHOR
                xml = .GetHotelRules(xdoc.DocumentElement)
            End With
        Catch ex As Exception
            Response.Redirect("Booking.aspx")
        End Try


        Return xml

    End Function
    Private Sub loadresources()
        lblCheckIn.Text = PortalCulture.GetString("M000078", True)
        lblCheckOut.Text = PortalCulture.GetString("M000079", True)
        lblCuartos.Text = PortalCulture.GetString("A00041", True)
        lblcodigotarifa.Text = PortalCulture.GetString("00006", True)
        lblPoliticas.Text = PortalCulture.GetString("M000531", True)
        lblPoliticasCancelacion.Text = PortalCulture.GetString("00349", True)
        lblPoliticasGarantia.Text = PortalCulture.GetString("M000528", True)
        lblpoliticastarjeta.Text = PortalCulture.GetString("M000529", True)
        lblCargosExtras.Text = PortalCulture.GetString("M000530", True)
        lblMinimodiasCancelacion.Text = PortalCulture.GetString("00438", True)
        lblMaximoAdultos.Text = PortalCulture.GetString("M0UT00480", True)
        lblmaximodias.Text = PortalCulture.GetString("M0UT00481", True)
        lblenpeticion.Text = PortalCulture.GetString("00314", True)
        lblExtrapersons.Text = PortalCulture.GetString("00076", True)
        lblEdadMaximaninio.Text = PortalCulture.GetString("M0UT00485", True)
        lblMaximoNinios.Text = PortalCulture.GetString("M0UT00479", True)
        lblMaximoAdultos.Text = PortalCulture.GetString("M0UT00480", True)
        lblMinimodias.Text = PortalCulture.GetString("00350", True)
        lblGarantiaDep.Text = PortalCulture.GetString("00351", True)
        lblDatosReserva.Text = PortalCulture.GetString("00352")
        lblInformes.Text = PortalCulture.GetString("00353")
        lblDatosviajero.Text = PortalCulture.GetString("00354")
        lblName.Text = "*" & PortalCulture.GetString("00432", True)
        lblLastName.Text = "*" & PortalCulture.GetString("00355", True)
        lblHomePhone.Text = "*" & PortalCulture.GetString("M000328")
        lblWorkHome.Text = PortalCulture.GetString("M000329")
        lblAddress.Text = PortalCulture.GetString("M000076", True)
        lblCiudad.Text = PortalCulture.GetString("00254", True)
        lblEstado.Text = PortalCulture.GetString("00252", True)

        lblemail.Text = "*" & PortalCulture.GetString("M000327")
        REMail.Text = PortalCulture.GetString("00356") 'email invalido
        lblError.Text = PortalCulture.GetString("00357") 'emeil o nombre es requerido
        lbldatostarjeta.Text = PortalCulture.GetString("M000503")
        lblNumber.Text = "*" & PortalCulture.GetString("M000504")
        RevCardNumber.Text = PortalCulture.GetString("00358") 'invald card number
        lblVerify.Text = "*" & PortalCulture.GetString("M000505")
        lblHolder.Text = "*" & "Holder :"
        lblExpirationDate.Text = "*" & PortalCulture.GetString("00431")
        lblExpirationError.Text = PortalCulture.GetString("00359") 'invalid expiration date
        REVYear.Text = PortalCulture.GetString("00360") 'only numbers please(yyyy)
        RVMonth.Text = PortalCulture.GetString("00361") 'only numbers (MM)
        lblType.Text = "*" & PortalCulture.GetString("00362", True)
        btnReservar.Text = PortalCulture.GetString("00363")
        btnCancel.Text = PortalCulture.GetString("A00143")
        lblRateDetails.Text = PortalCulture.GetString("00366")
        lblPortal.Text = PortalCulture.GetString("00755")
        lblConvenio.Text = PortalCulture.GetString("01426", True)
        lblCodigo.Text = PortalCulture.GetString("01425", True)
        lblDeposito.Text = PortalCulture.GetString("00037", True)
        lblPago.Text = PortalCulture.GetString("01421")
        lblDatosDep.Text = PortalCulture.GetString("01422")
        rbTc.Text = PortalCulture.GetString("01423")
        rbDeposito.Text = PortalCulture.GetString("01424")
        lblTitleLine.Text = PortalCulture.GetString("01435")
        lblMsgLinea.Text = PortalCulture.GetString("01440")
        lblEdadMenor.Text = PortalCulture.GetString("01509", True)
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadresources()
        Dim dMonto As Double
        ' If Not Me.Reservacion Is Nothing Then
        If ResponseHOR Is Nothing Then
            ResponseHOR = GetHOR()
        End If
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        lblFechaInicio.Text = New Date(checkIn.Substring(0, 4), checkIn.Substring(4, 2), checkIn.Substring(6, 2)).ToString("MMM/dd/yyyy")
        Me.lblFechasalida.Text = New Date(checkOut.Substring(0, 4), checkOut.Substring(4, 2), checkOut.Substring(6, 2)).ToString("MMM/dd/yyyy")
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
        Me.lblratecode.Text = RateCode
        Me.tddetalles.InnerHtml = "<table width ='100%'>"
        Me.tddetalles.InnerHtml &= "<tr  class='clslabel'>"
        Me.tddetalles.InnerHtml &= "<td></td>"
        Me.tddetalles.InnerHtml &= "<td>" & PortalCulture.GetString("M000645") & "</td>"
        Me.tddetalles.InnerHtml &= "<td>" & PortalCulture.GetString("M000646") & "</td>"
        Me.tddetalles.InnerHtml &= "</tr>"
        For r As Integer = 0 To totalRooms - 1
            Me.tddetalles.InnerHtml &= "<tr class='bookingNormalLabel'>"
            Me.tddetalles.InnerHtml &= "<td>" & PortalCulture.GetString("M000439") & " " & r + 1 & "</td>"

            If Adults <> "" Then
                If Adults.Split(",").Length > r - 1 Then
                    Me.tddetalles.InnerHtml &= "<td>" & Adults.Split(",")(r) & "</td>"
                End If
            End If
            If Children <> "" Then
                If Children.Split(",").Length > r - 1 Then
                    Me.tddetalles.InnerHtml &= "<td>" & Children.Split(",")(r) & "</td>"
                End If
            End If



            Me.tddetalles.InnerHtml &= "</tr>"
        Next
        Me.tddetalles.InnerHtml += "</table>"


        Dim dTaxes As Double
        'Dim drP As resHotelRules._PropertyRow
        IsNetRate = False
        If ResponseHOR._Property.Count > 0 Then
            With ResponseHOR._Property.Rows(0)
                lblConvenio.Visible = False
                If Not String.IsNullOrEmpty(ResponseHOR._Property(0).CompanyContract) Then
                    lblConvenioEmpresa.Text = String.Format("{0}-{1}", ResponseHOR._Property(0).CompanyContract, ResponseHOR._Property(0).ReferenceContract)
                    lblConvenioEmpresa.Visible = True
                    lblConvenio.Visible = True
                End If
                Double.TryParse(.Item("Prepayment").ToString(), dMonto)
                If dMonto = 0 Then
                    Double.TryParse(.Item("TotalRate").ToString(), dMonto)
                    Double.TryParse(.Item("taxes").ToString(), dTaxes)
                    dMonto += dTaxes
                End If

                lblPrepagoLine.Text = String.Format(PortalCulture.GetString("01441"), FCurrency(dMonto, 2) & " " & .Item("Money"))

                lblPoliticies.Text = .Item("Politicies").ToString
                lblCancelationPoliticies.Text = .Item("CancelationPoliticies").ToString
                lblGuarantyPoliticies.Text = .Item("GuarantyPoliticies").ToString
                lblCreditCardPoliticies.Text = .Item("CreditCardPoliticies").ToString
                lblExtraCharges.Text = .Item("ExtraCharges").ToString
                If .Item("CancelPrior").ToString.IndexOf("T") <> -1 Then
                    lblMinDaysToCancel.Text = PortalCulture.GetString("00442") & " " & .Item("CancelPrior").ToString.Substring(0, .Item("CancelPrior").ToString.IndexOf("T") - 1) & " " & PortalCulture.GetString("00443")
                ElseIf .Item("CancelPrior").ToString.IndexOf("D") <> -1 Then
                    lblMinDaysToCancel.Text = .Item("CancelPrior").ToString.ToString.Substring(0, .Item("CancelPrior").ToString.IndexOf("D") - 1) & " " & PortalCulture.GetString("00444")
                ElseIf .Item("CancelPrior").ToString.IndexOf("H") <> -1 Then
                    lblMinDaysToCancel.Text = .Item("CancelPrior").ToString.ToString.Substring(0, .Item("CancelPrior").ToString.IndexOf("H") - 1) & " " & PortalCulture.GetString("00445")
                End If

                lblMaxAgeOfChildren.Text = .Item("MaxAgeOfChildren").ToString
                Me.lblTotalRate.Text = PortalCulture.GetString("00377")
                Me.lblTotalExtra.Text = PortalCulture.GetString("00378")
                Me.lblTotalTaxes.Text = PortalCulture.GetString("00379")
                Me.lblTotal.Text = PortalCulture.GetString("00380")
                Me.lbTotalRate.Text = FCurrency(CDbl(Val(.Item("TotalRate").ToString)), 2) & " " & .Item("Money")
                Me.lbTotalExtra.Text = FCurrency(CDbl(Val(.Item("TotalExtras").ToString)), 2) & " " & .Item("Money")
                If .Item("PlusTax") Then
                    Me.lbTotalTaxes.Text = PortalCulture.GetString("00433")
                Else
                    Me.lbTotalTaxes.Text = FCurrency(CDbl(Val(.Item("Taxes").ToString)), 2) & " " & .Item("Money")
                End If

                Me.lbTotal.Text = FCurrency(CDbl(Val(.Item("TotalRate").ToString)) + CDbl(Val(.Item("TotalExtras").ToString)) + CDbl(Val(.Item("Taxes").ToString)), 2) & " " & .Item("Money")

                rbDeposito.Visible = False
                If (.Item("AllowBankDeposit").ToString = "Y") Then
                    Dim dTotal As Double
                    rbDeposito.Visible = True
                    lblDepositInfo.Text = .Item("DepositInfo").ToString
                    Double.TryParse(.Item("DepositAmount").ToString, dTotal)
                    lblMontoDeposito.Text = FCurrency(dTotal, 2) & " " & .Item("Money")
                End If

            End With

            If ResponseHOR.Room.Count > 0 Then
                With ResponseHOR.Room.Rows(0)
                    If Not .IsNull("isUvNetRate") Then

                        IsNetRate = .Item("isUvNetRate")
                        If .Item("isUvNetRate") Then
                            Me.lblNetRatePol.Text = CargaPoliticasUvNetRates()
                        End If
                    End If
                    If Not ResponseHOR._Property(0).IsPrepaymentNull AndAlso ResponseHOR._Property(0).Prepayment > 0 Then
                        'parche para que se valla a pago en linea
                        IsNetRate = True
                    End If

                    lblCodigo.Visible = False
                    If Not .IsNull("AccessCode") Then
                        lblAccessCode.Text = .Item("AccessCode").ToString
                        If Not String.IsNullOrEmpty(lblAccessCode.Text) Then
                            lblCodigo.Visible = True
                        End If
                    End If
                    lblMaxAdults.Text = .Item("MaxAdults").ToString
                    lblMaxdays.Text = .Item("MaxDays").ToString
                    lblextrapeople.Text = .Item("ExtraPeople").ToString
                    lblmaxninios.Text = .Item("MaxChildren").ToString
                    lblMindays.Text = .Item("MinDays").ToString
                    If .Item("GuarDep").ToString.ToUpper = "G" Then
                        lblGuardep.Text = PortalCulture.GetString("00364")
                    Else
                        lblGuardep.Text = PortalCulture.GetString("00365")
                    End If
                    If .Item("OnRequest").ToString().ToUpper = "N" Then
                        lblOnRequest.Text = "No"
                    Else
                        lblOnRequest.Text = PortalCulture.GetString("M0BT0000077")
                    End If


                End With
            End If
        End If
        DetailsRate()
        'Me.tdDetailsRate.InnerHtml = GetDetailsRate()       
        lbltitlePet.Text = PortalCulture.GetString("00437")
        If IsNetRate Then
            txtNumber.Text = "4242424242424242"
            txtVerify.Text = "123"
            txtHolder.Text = "x"
            txtMonth.Text = "12"
            txtYear.Text = Now.Year + 1
            ddlTarjetas.SelectedIndex = 1
            pnlCredito.Style.Add("display", "none")
            pnlPagoLinea.Style.Add("display", "")

            rbTc.Attributes.Add("onclick", String.Format("FireDeposito('{0}','{1}','{2}', false);", pnlPagoLinea.ClientID, pnlDeposito.ClientID, rbTc.ClientID))
            rbDeposito.Attributes.Add("onclick", String.Format("FireDeposito('{0}','{1}','{2}', false);", pnlDeposito.ClientID, pnlPagoLinea.ClientID, rbDeposito.ClientID))

        Else
            If rbDeposito.Checked Then
                pnlCredito.Style.Add("display", "none")
                pnlDeposito.Style.Add("display", "")
            Else
                pnlCredito.Style.Add("display", "")
                pnlDeposito.Style.Add("display", "none")
            End If

            pnlPagoLinea.Style.Add("display", "none")
            rbTc.Attributes.Add("onclick", String.Format("FireDeposito('{0}','{1}','{2}', true);", pnlCredito.ClientID, pnlDeposito.ClientID, rbTc.ClientID))
            rbDeposito.Attributes.Add("onclick", String.Format("FireDeposito('{0}','{1}','{2}', false);", pnlDeposito.ClientID, pnlCredito.ClientID, rbDeposito.ClientID))
        End If

        'Else
        'Response.Redirect( "/Callcenter/booking.aspx")
        'End If
    End Sub
    Private Function CargaPoliticasUvNetRates() As String
        Try
            Dim rssFile As String = GeRequestApplicationPath(String.Concat("/Politicas/", PortalCulture.GetCulture.ToString.Substring(0, 2), "/UvNetRatesPolicies.txt"))
            Dim tr As TextReader
            Dim str As String

            tr = New StreamReader(System.Web.HttpContext.Current.Server.MapPath(rssFile), System.Text.Encoding.Default)
            str = tr.ReadToEnd
            tr.Close()
            Return str
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub btnReservar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReservar.Click
        If Not Page.IsValid Then Return
        Dim er As Boolean = False
        Me.lblError.Visible = False
        lblExpirationError.Visible = False
        If rbDeposito.Checked Or IsNetRate Then
            makeReserva()
            Return
        End If
        If (Me.txtNombre.Text <> "" And Me.txtApellido.Text <> "") Or Me.txtEmail.Text <> "" Then
            If Me.txtYear.Text < Now().Year Then
                er = True
            ElseIf Me.txtYear.Text = Now().Year Then
                If Me.txtMonth.Text < Now().Month Then
                    er = True
                End If
            End If
            If er = True Then
                lblExpirationError.Visible = True
            End If
        Else
            Me.lblError.Visible = True
            er = True
        End If
        If er = False And Me.ddlTarjetas.SelectedValue <> "-1" Then
            Dim Tarjeta As Tarjetas = New Tarjetas(Me.txtNumber.Text, GetCardId(Me.ddlTarjetas.SelectedValue))
            If Not Tarjeta.IsValid And Me.txtNumber.Text <> "4242424242424242" Then
                Me.RevCardNumber.IsValid = False
            Else
                makeReserva()
            End If
        End If
    End Sub

    Private Function GetRoomsDetails(ByVal resDisp As resHotelDisplay) As String
        Dim Data As New StringBuilder
        For I As Integer = 0 To resDisp.Room.Count - 1
            Dim dr As resHotelDisplay.RoomRow = resDisp.Room(I)
            Data.Append("<tr><td  valign='top'><br/></td></tr>")
            Data.Append("<tr><td valign='top' ><p>")
            Data.Append(PortalCulture.GetString("00706") & " " & (I + 1).ToString & ". " & PortalCulture.GetString("00711", True))
            Data.Append("</p></td></tr>")
            Data.Append("<tr><td valign='top' class='rgHeader'><p class='txtData'>")
            Data.Append(dr.TravelerName & "</p></td></tr>")
            Data.Append("<tr><td valign='top' ><p>")
            Data.Append(PortalCulture.GetString("00712", True) & "</p></td></tr>")
            Data.Append("<tr><td valign='top' class='rgHeader'><p class='txtData'>")
            Data.Append(dr.Adults & " " & PortalCulture.GetString("00707"))
            If dr.Children > 0 Then
                Data.Append(", " & dr.Children & " " & PortalCulture.GetString("00708") & "</p></td></tr>")
            Else
                Data.Append("</p></td></tr>")
            End If
            Data.Append("<tr><td valign='top' ><p>")
            Data.Append(PortalCulture.GetString("00709", True) & "</p></td></tr>")
            Data.Append("<tr><td valign='top' class='rgHeader'><p class='txtData'>")
            If dr.Preferences.Trim <> "" Then
                Data.Append(dr.Preferences & "</p></td></tr>")
            Else
                Data.Append(PortalCulture.GetString("00710") & "</p></td></tr>")
            End If

        Next
        Return Data.ToString
    End Function

    Private Function ReplaceParameters(ByVal htmlvalue As String, ByVal Prov As PortalPartnersCfg) As String
        Dim PPConf As New PortalPartnersCfg

        htmlvalue = htmlvalue.Replace("{SiteName}", PPConf.Name)
        htmlvalue = htmlvalue.Replace("{DomainName}", PPConf.DomainName)

        Return htmlvalue
    End Function

    Public Function ReadHTML(ByVal RssFile As String, ByVal Prov As PortalPartnersCfg) As String
        Try
            Dim tr As TextReader
            Dim str As String

            tr = New StreamReader(System.Web.HttpContext.Current.Server.MapPath(RssFile), System.Text.Encoding.Default)
            str = tr.ReadToEnd
            tr.Close()

            Return ReplaceParameters(str, Prov)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function CargaPoliticas(ByVal prov As PortalPartnersCfg) As String
        Try
            Dim str As String
            Dim idioma As String

            '*/*/*/*/*/*/*/*/*/     idioma = "_EN"
            '*/*/*/*/*/*/*/*/*/     If prov.CurrencyCode.ToUpper = "MXN" Then idioma = "_ES"

            If PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper = "ES" Then
                idioma = "_ES"
            Else
                idioma = "_EN"
            End If

            str = ReadHTML(GeRequestApplicationPath(String.Concat("/Politicas/HotelPolicies", idioma, ".txt")), prov)
            str &= "<br/>"

            Return str
        Catch ex As Exception
            Return ""
        End Try
    End Function



    Private Sub enviarcorreoClienteTemplate141(ByVal nores As String)
        Try
            Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
            Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
            Dim xml As resHotelDisplay


            Dim drR As reqHotelDisplay.HotelDisplayRow
            drR = dsreq.HotelDisplay.NewHotelDisplayRow
            drR.ConfirmNumber = nores
            drR.Language = "en-US"
            dsreq.HotelDisplay.AddHotelDisplayRow(drR)

            With New WSHotelFacade.clsFADisplay
                xml = .GetHotelDisplay(xdoc.DocumentElement)
            End With

            If Not xml Is Nothing AndAlso xml.Reservation.Rows.Count > 0 Then
                Dim idioma_orig As String
                Dim idioma As String
                idioma_orig = PortalCulture.GetCulture.ToString
                Dim Prov As New PortalPartnersCfg
                Prov.LoadPartnerById(xml.HotelHeader.Rows(0).Item("idPortal"))
                idioma = "en_US"
                If Prov.CurrencyCode = "MXN" Then idioma = "es-MX"
                '*/*/* PortalCulture.SetCulture(idioma)
                Call enviarDatosCorreo(xml)
                '*/*/*/ PortalCulture.SetCulture(idioma_orig)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub enviarDatosCorreo(ByVal resDisp As resHotelDisplay)
        Try
            Dim Mail As emailTemplates.Template = New emailTemplates.Template
            Dim Prov As New PortalPartnersCfg
            Prov.LoadPartnerById(resDisp.HotelHeader.Rows(0).Item("idPortal"))

            Dim com As Double
            'Obtener la preferencia del envio
            Dim politicas As String
            politicas = CargaPoliticas(Prov)

            Mail.Html = True

            'Dim x As Byte
            Dim totAd As Byte = 0
            Dim totNi As Byte = 0

            Dim AltTotal As Single
            Dim AltTaxes As Single
            Dim AltMoney As String

            AltTotal = resDisp.Reservation(0).Total
            AltTaxes = resDisp.Reservation(0).Taxes
            AltMoney = resDisp.Reservation(0).Money

            For Each dr As resHotelDisplay.RoomRow In resDisp.Room
                totAd += CInt(dr.Adults) + CInt(dr.ExtraAdults)
                totNi += CInt(dr.Children) + CInt(dr.ExtraChildren)
            Next

            Dim idioma As String
            '*/*/*/*/*/     idioma = "en-US"
            '*/*/*/*/*/     If Prov.CurrencyCode.ToUpper = "MXN" Then idioma = "es-MX"

            If PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper = "ES" Then
                idioma = "es-MX"
            Else
                idioma = "en-US"
            End If

            Mail.Idioma = idioma 'PortalCulture.GetCulture.ToString
            Mail.SubjectParam = resDisp.Reservation(0).ConfirmNumber  '  Reservacion.ReservationNumber

            'Es empresa de galileo
            'Mail.Rubro = Rubros.Hotel
            Mail.TemplateName = "T14_RESERVATION"

            Mail.To = resDisp.Customer(0).Email
            Mail.Bcc = AppSettings("UnivisitMail")

            Mail.AddParameter("EMPRESA") = resDisp.Reservation(0).HotelName
            Mail.AddParameter("HEADER") = Prov.EmailHeader
            Mail.AddParameter("FOOTER") = Prov.EmailFooter
            Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css ' type='text/css' rel='stylesheet'>" '"<style>" & ReadCSS(Prov.StyleSheets & "correos.css") & "</style>"
            Mail.AddParameter("PORTALNAME") = Prov.Name
            Mail.AddParameter("URLSITE") = Prov.NonSecureSite
            Mail.AddParameter("RESERVATIONINFO") = ""
            Mail.AddParameter("STATUS") = PortalCulture.GetString("M000331")
            Mail.AddParameter("CONTACTO") = resDisp.Customer(0).FirstName & " " & resDisp.Customer(0).LastName

            If resDisp.Customer(0).Email Is Nothing Then
                Mail.AddParameter("EMAIL_CONTACTO") = ""
            Else
                Mail.AddParameter("EMAIL_CONTACTO") = resDisp.Customer(0).Email
            End If

            Dim phone As String = ""
            If Not resDisp.Customer(0).IsPhoneHomeNull Then
                Dim PhoneHome() As String = CType(resDisp.Customer(0).PhoneHome, String).Split("+")

                Dim show As Integer = 0
                If PhoneHome.Length > 0 Then
                    For Each st As String In PhoneHome
                        If st.Trim <> "" Then
                            show += 1
                        End If
                    Next
                    If show > 2 Then
                        phone = resDisp.Customer(0).PhoneHome.ToString.Replace("+", "-")
                    End If
                End If
            End If

            Mail.AddParameter("TEL_CASA") = phone
            phone = ""

            If Not resDisp.Customer(0).IsPhoneWorkNull Then
                Dim phoneWork() As String = CType(resDisp.Customer(0).PhoneWork, String).Split("+")
                Dim show As Integer = 0
                If phoneWork.Length > 0 Then
                    For Each st As String In phoneWork
                        If st.Trim <> "" Then
                            show += 1
                        End If
                    Next
                    If show > 2 Then
                        phone = resDisp.Customer(0).PhoneWork.ToString.Replace("+", "-")
                    End If
                End If
            End If

            Mail.AddParameter("TEL_TRABAJO") = phone

            If Not resDisp.Reservation(0).IsHotelNameNull Then
                Mail.AddParameter("HOTEL_NOMBRE") = resDisp.Reservation(0).HotelName
            Else
                Mail.AddParameter("HOTEL_NOMBRE") = ""
            End If

            If Not resDisp.Reservation(0).IsHotelAddressNull Then
                Mail.AddParameter("HOTEL_DIRECCION") = resDisp.Reservation(0).HotelAddress
            Else
                Mail.AddParameter("HOTEL_DIRECCION") = ""
            End If

            If Not resDisp.Reservation(0).IsCityNameNull Then
                Mail.AddParameter("HOTEL_CIUDAD") = resDisp.Reservation(0).CityName
            Else
                Mail.AddParameter("HOTEL_CIUDAD") = ""
            End If

            If Not resDisp.Reservation(0).IsCountryNameNull Then
                Mail.AddParameter("HOTEL_PAIS") = resDisp.Reservation(0).CountryName
            Else
                Mail.AddParameter("HOTEL_PAIS") = ""
            End If

            If PortalCulture.GetCulture.ToString = "en-MX" Then
                Mail.AddParameter("CHECKIN") = CDate(resDisp.Reservation(0).CheckInDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("CHECKOUT") = CDate(resDisp.Reservation(0).CheckOutDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("REGDATE") = CDate(resDisp.Reservation(0).ReservationDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            Else
                Mail.AddParameter("CHECKIN") = CDate(resDisp.Reservation(0).CheckInDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("CHECKOUT") = CDate(resDisp.Reservation(0).CheckOutDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("REGDATE") = CDate(resDisp.Reservation(0).ReservationDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            End If

            Mail.AddParameter("URLIMGAIR") = ParseAbsolutePath(Prov.ImagesSystem, Prov) & "air.jpg"
            Mail.AddParameter("URLIMGCAR") = ParseAbsolutePath(Prov.ImagesSystem, Prov) & "cars.jpg"
            Mail.AddParameter("URLIMGACTIVITIES") = ParseAbsolutePath(Prov.ImagesSystem, Prov) & "activities.jpg"

            Mail.AddParameter("URLAIR") = Prov.NonSecureSite & "/Flight/pgnAirIncompleteSearch.aspx?CheckIn=" & resDisp.Reservation(0).CheckInDate.Replace("/", "") & "&CheckOut=" & resDisp.Reservation(0).CheckOutDate.Replace("/", "") & "&RefCityD=" & resDisp.Reservation(0).CityName & "&Adultos=" & totAd & "&Ninios=" & totNi
            Mail.AddParameter("URLCAR") = Prov.NonSecureSite & "/AutoSystem/pgnAutoIncompleteSearch.aspx"
            Mail.AddParameter("URLACTIVITIES") = Prov.NonSecureSite & "/activitiesSystem/pgnActIncompleteSearch.aspx"

            Mail.AddParameter("ADULTOS") = totAd
            Mail.AddParameter("NINIOS") = totNi
            Dim nhab As String = ""
            If resDisp.Room.Count > 0 Then nhab = resDisp.Room(0).NameRoom

            Mail.AddParameter("HABITACIONES") = resDisp.Room.Count & " " & nhab
            Mail.AddParameter("NOCHES") = DateDiff(DateInterval.Day, CDate(resDisp.Reservation(0).CheckInDate), CDate(resDisp.Reservation(0).CheckOutDate))

            com = resDisp.Reservation(0).ServiceFee

            Mail.AddParameter("ESTANCIATOTAL") = FCurrency(com + AltTotal, 2) & " " & AltMoney

            For Each dr As resHotelDisplay.RoomRow In resDisp.Room
                Mail.AddParameter("Clientes") = dr.TravelerName
            Next

            Mail.AddParameter("ROOMDETAILS") = GetRoomsDetails(resDisp)

            Dim strCardNumber As String
            Dim MaskedCardNumber As String
            strCardNumber = resDisp.Reservation(0).CreditCardNumber
            Dim strNewCN As String = strCardNumber.Substring(strCardNumber.Length - 4, 4) '// caracteres
            MaskedCardNumber = "************" & strNewCN '//12 X más los ultimos 4 numeros
            MaskedCardNumber &= "<br/>" & resDisp.Reservation(0).CreditCardHolder()
            Mail.AddParameter("TARJETACREDITO") = MaskedCardNumber
            If com > 0 Then
                Mail.AddParameter("DEPOSITO") = String.Format(PortalCulture.GetString("00783"), FCurrency(com, 2))
            Else
                Mail.AddParameter("DEPOSITO") = PortalCulture.GetString("00713")
            End If

            If Mail.Html = False Then
                'Dim REx As System.Text.RegularExpressions.Regex
                politicas = politicas.Replace("&nbsp;", " ")
                politicas = politicas.Replace("</p>", ControlChars.CrLf)
                politicas = System.Text.RegularExpressions.Regex.Replace(politicas, "&(?ni:\#((x([\dA-F]){1,5})|(104857[0-5]|10485[0-6]\d|1048[0-4]\d\d|104[0-7]\d{3}|10[0-3]\d{4}|0?\d{1,6}))|([A-Za-z\d.]{2,31}));|<[^>]*>", "")
            End If

            Mail.AddParameter("POLITICAS") = politicas
            Mail.AddParameter("URLPOLICIES") = Prov.SecureSite & "/hotel/hoteldescription.aspx?Provider=" & resDisp.Reservation(0).Provider & "&PropertyNumber=" & resDisp.Reservation(0).PropertyNumber & "&tab=Policies"

            Dim Emailnote As String = ""
            Dim DetailsRate As String = ""
            Dim DetailsRateValue As String = ""

            If resDisp.Reservation(0).Provider = 0 Then
                DetailsRate &= "<p>Subtotal:</p>"
                DetailsRate &= "<p>" & PortalCulture.GetString("00703", True) & "</p>"
                DetailsRate &= "<p>Total:</p>"

                If resDisp.Reservation(0).PlusTax = "False" Then
                    DetailsRateValue &= "<p>" & FCurrency(AltTotal - AltTaxes, 2) & " " & AltMoney & "</p>"
                    DetailsRateValue &= "<p>" & FCurrency(AltTaxes, 2) & " " & AltMoney & "</p>"
                Else
                    DetailsRateValue &= "<p>" & FCurrency(AltTotal, 2) & " " & AltMoney & "</p>"
                    DetailsRateValue &= "<p>" & PortalCulture.GetString("00705") & "</p>"
                End If

                DetailsRateValue &= "<p>" & FCurrency(AltTotal, 2) & " " & AltMoney & "</p>"
                Mail.AddParameter("ID_RESERVACION") = resDisp.Reservation(0).ConfirmNumber
            Else
                DetailsRate &= "<p>*Total:</p>"
                DetailsRateValue &= "<p>" & FCurrency(AltTotal, 2) & " " & AltMoney & "</p>"
                Mail.AddParameter("ID_RESERVACION") = "UV: " & resDisp.Reservation(0).ConfirmNumber & ", GAL: " & resDisp.Reservation(0).RecLoc
                Emailnote = "*" & PortalCulture.GetString("00704")
            End If

            Mail.AddParameter("TEXTOTAR") = DetailsRate
            Mail.AddParameter("VALORESTAR") = DetailsRateValue

            If AltMoney <> resDisp.Reservation(0).Money AndAlso AltMoney = (New PortalPartnersCfg).CurrencyCode Then 'existe un tipo de moneda diferente mostrado.
                If Emailnote <> "" Then
                    Emailnote &= "<br/>"
                Else
                    Emailnote = "*"
                End If
                Dim cadmon As String = PortalCulture.GetString("00702")
                cadmon = String.Format(cadmon, FCurrency(CDbl(resDisp.Reservation.Rows(0).Item("Total")), 2)) & " " & resDisp.Reservation(0).Money
                Emailnote &= PortalCulture.GetString("00701") & " " & cadmon
            End If

            If Emailnote <> "" Then Emailnote = "<DIV  class='footerCard'><p>" & Emailnote & "</p></div>"
            Mail.AddParameter("CALCULODETAIL") = Emailnote
            Mail.AddParameter("AGENCYINFO") = ""
            Mail.AddParameter("LINKRESERVATION") = Prov.SecureSite & "/hotel/Secure/DisplayReservation.aspx?ConfirmNum=" & resDisp.Reservation(0).ConfirmNumber
            Mail.Send()
        Catch ex As Exception
            Dim mensaje As String = ex.Message
        Finally
        End Try
    End Sub

    Private Sub setPaymentValue(ByVal idreservacion As Integer, ByVal amount As Double, ByVal moneda As String)
        Dim conn As New SqlConnection(AppSettings("HotelConnection"))
        Dim cmd As New SqlCommand("spPagosReservacionesInsert", conn)
        Try
            conn.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@idReservacion", SqlDbType.Int)
            cmd.Parameters.Add("@NoAutorizacion", SqlDbType.NVarChar, 50)
            cmd.Parameters.Add("@paymentSource", SqlDbType.Int)
            cmd.Parameters.Add("@Monto", SqlDbType.Money)
            cmd.Parameters.Add("@Moneda", SqlDbType.NVarChar, 3)
            cmd.Parameters.Add("@Status", SqlDbType.Int)
            cmd.Parameters("@idReservacion").Value = idreservacion
            cmd.Parameters("@NoAutorizacion").Value = ""
            cmd.Parameters("@paymentSource").Value = 4
            cmd.Parameters("@Monto").Value = amount
            cmd.Parameters("@Moneda").Value = moneda
            cmd.Parameters("@Status").Value = 4

            cmd.ExecuteNonQuery()
        Catch ex As Exception
            ''errror en el establecimiento de la reservación
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Function ReservOnLinePayment(ByVal xmlsell As resHotelSell) As Boolean
        Dim payItemOrder As New PaymentLibrary.PaymentSockets
        Dim Card As New PaymentLibrary.CardPayment
        Dim Amount As Decimal = 0
        Dim dTotal As Double
        Dim sBankOnlineCCPayp As String
        Dim sServiceAccount As String
        Dim sCultureFormat As String
        Dim sServicePort As String
        Dim sServiceUrl As String
        Dim sServiceId As String
        Dim ReturnUrl As String
        Dim sCurrency As String
        Dim pagina As String
        Dim sMoney As String
        Dim hr As Boolean = False
        Dim AllowOnlineCCPaypment As Boolean
        Dim dMontoOriginal
        Dim btc As Boolean = False
        Dim sMerchantService_SecureHash As String
        Dim sMerchantService_AccessCode As String

        'Tipo de tarjeta (MasterCard, Visa) y método de cifrado                            
        Card.ccGateway = "ssl"
        sCultureFormat = "es-MX"
        sBankOnlineCCPayp = ConfigurationManager.AppSettings("BankOnlineCCPayp")
        ReturnUrl = ConfigurationManager.AppSettings("UrlPayment")
        sCurrency = ConfigurationManager.AppSettings("MerchantService_Currency")
        sServiceUrl = ConfigurationManager.AppSettings("MerchantService_Url")
        sServicePort = ConfigurationManager.AppSettings("MerchantService_Port")
        sServiceId = ConfigurationManager.AppSettings("idUrlPayment")
        sServiceAccount = ConfigurationManager.AppSettings("MerchantService_Account")
        sMerchantService_SecureHash = ConfigurationManager.AppSettings("MerchantService_SecureHash")
        sMerchantService_AccessCode = ConfigurationManager.AppSettings("MerchantService_AccessCode")

        Boolean.TryParse(ConfigurationManager.AppSettings("AllowOnlineCCPayp"), AllowOnlineCCPaypment)

        If AllowOnlineCCPaypment AndAlso IsNetRate Then
            dMontoOriginal = 0
            Dim DisplayRs As resHotelDisplay = GetHotelDisplay(xmlsell.Reservation(0).ConfirmNumber)
            If Not xmlsell.Reservation(0).IsPrepaymentNull Then

                If DisplayRs.Reservation(0).Money = sCurrency Then
                    Amount = DisplayRs.Reservation(0).Prepayment
                Else
                    With New PortalLibraries.MoneyExchangeService
                        btc = True
                        dMontoOriginal = DisplayRs.Reservation(0).Prepayment
                        Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Money, sCurrency, DisplayRs.Reservation(0).Prepayment)
                    End With
                End If
            Else
                If DisplayRs.Reservation(0).Money.ToUpper = sCurrency Then
                    Amount = DisplayRs.Reservation(0).Total
                    'ElseIf DisplayRs.Reservation(0).AltMoney.ToUpper = sCurrency Then
                    '    Amount = DisplayRs.Reservation(0).AltTotal
                Else
                    Dim TotalConverted As String
                    With New PortalLibraries.MoneyExchangeService
                        btc = True
                        dMontoOriginal = DisplayRs.Reservation(0).Total
                        Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Money, sCurrency, DisplayRs.Reservation(0).Total)

                    End With
                End If
            End If

            If dMontoOriginal = Amount And btc Then

                Me.lblErrorReserva.Text = PortalCulture.GetString("01443")
                Me.lblErrorReserva.Visible = True
                btnReservar.Visible = False
                cmdCancelaReserva.Visible = True
                Return True
            End If

            Try
                '//Guardar el registro de pago de reservacion
                setPaymentValue(xmlsell.Reservation(0).ReservationId, Amount, "MXN")
            Catch ex As Exception
            End Try

            pnlPolities.Style.Add("display", "none")
            frmOnLine.Style.Add("display", "block")
            frmOnLine.Style.Add("Left", "0px")

            If sBankOnlineCCPayp = "BMX" Then

                Dim pay As New PaymentLibrary.PaymentBanamex2(sServiceAccount, sMerchantService_AccessCode, sMerchantService_SecureHash, "https://banamex.dialectpayments.com/vpcpay")
                Dim Pay_Url As String = ""
                Pay_Url = pay.PayOrder(DisplayRs.Reservation(0).ConfirmNumber, _
                                                    DisplayRs.Reservation(0).ConfirmNumber, "", _
                                                    Amount, Card, _
                                                    sCultureFormat.Substring(0, 2), _
                                                    ReturnUrl & "?idURL=" & sServiceId & _
                                                    "&idUser=" & UserIdentityName & _
                                                    "&idReservation=" & DisplayRs.Reservation.Rows(0).Item("ConfirmNumber") & _
                                                    "&Language=" & Request.QueryString("Language") & "&sh=" & sMerchantService_SecureHash, "MXN")


                Try
                    'Pay_Url = "http://localhost:15903/Ratemanager/CallCenter/OnlineConfirmed.aspx?ReservationId=IDRESERV&ConfirmNum=IDRESERV&PropertyNumber=0&Language=0&Currency=MXN&BancomerPayment=false&MasterRedirect=true&AuthorizeId=TEST00"
                    If Pay_Url <> "" Then
                        'Response.Redirect(payItemOrder.digitalOrder)
                        frmOnLine.Attributes.Add("src", Pay_Url)
                        hr = True
                    Else
                        pnlPolities.Visible = True
                        frmOnLine.Visible = False
                    End If
                Catch ex As Exception
                    pnlPolities.Visible = True
                    frmOnLine.Visible = False
                Finally
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''PAGO EN LINEA DE BANAMEX VIEJO'''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'If payItemOrder.PayOrderSocket(sServiceUrl, sServicePort, sServiceAccount, _
                '                               xmlsell.Reservation(0).ConfirmNumber, _
                '                               xmlsell.Reservation(0).ConfirmNumber, "", _
                '                               Amount, Card, _
                '                               sCultureFormat.Substring(0, 2), _
                '                               ReturnUrl & "?idURL=" & sServiceId & _
                '                               "&idUser=" & UserIdentityName & _
                '                               "&idReservation=" & xmlsell.Reservation(0).ConfirmNumber & _
                '                               "&Language=" & PortalCulture.GetIDCulture) Then

                '    'si se recibio respuesta del banco te devuelve la url a donde redireccionaremos en la propiedad digitalOrder
                '    Try
                '        frmOnLine.Attributes.Add("src", payItemOrder.digitalOrder)
                '        hr = True
                '    Catch ex As Exception
                '        pnlPolities.Visible = True
                '        frmOnLine.Visible = False
                '    Finally
                '    End Try
                'End If

                'ClientScript.RegisterStartupScript(Me.GetType, "payItemOrder.digitalOrder", String.Format("window.location = ""{0}"";", payItemOrder.digitalOrder))
                ''Anthem.Manager.AddScriptForClientSideEval("window.location='" & payItemOrder.digitalOrder & "'")
            Else

                pagina = String.Format("/CallCenter/PayBancomer.aspx?idReservation={0}&noConfirmacion={1}&Amount={2}&Currency={3}", xmlsell.Reservation(0).ReservationId, xmlsell.Reservation(0).ConfirmNumber, Amount, sCurrency)
                hr = True
                frmOnLine.Attributes.Add("src", GeRequestApplicationPath(pagina))
            End If

        End If
        Return hr
    End Function

    Function PagoLineaBancomer(ByVal xmlsell As resHotelSell) As Boolean
        Dim DisplayRs As resHotelDisplay = GetHotelDisplay(xmlsell.Reservation(0).ConfirmNumber)
        Dim sCurrency As String
        Dim pagina As String = ""
        Dim Amount As String

        sCurrency = ConfigurationManager.AppSettings("MerchantService_Currency")
        If Not xmlsell.Reservation(0).IsPrepaymentNull Then

            If DisplayRs.Reservation(0).Money = sCurrency Then
                Amount = DisplayRs.Reservation(0).Prepayment
            Else
                With New PortalLibraries.MoneyExchangeService
                    Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Money, sCurrency, DisplayRs.Reservation(0).Prepayment)
                End With
            End If
        Else
            If DisplayRs.Reservation(0).Money.ToUpper = sCurrency Then
                Amount = DisplayRs.Reservation(0).Total
                'ElseIf DisplayRs.Reservation(0).AltMoney.ToUpper = sCurrency Then
                '    Amount = DisplayRs.Reservation(0).AltTotal
            Else
                Dim TotalConverted As String
                With New PortalLibraries.MoneyExchangeService
                    Amount = .GetMoneyExchange(DisplayRs.Reservation(0).Total, sCurrency, DisplayRs.Reservation(0).Total)
                End With
            End If
        End If

        Try
            '//Guardar el registro de pago de reservacion
            setPaymentValue(xmlsell.Reservation(0).ReservationId, Amount, "MXN")
        Catch ex As Exception
        End Try

        sCurrency = ConfigurationManager.AppSettings("MerchantService_Currency")
        pnlPolities.Style.Add("display", "none")
        frmOnLine.Style.Add("display", "block")
        frmOnLine.Style.Add("Left", "0px")
        pagina = String.Format("/CallCenter/PayBancomer.aspx?idReservation={0}&Currency={1}", xmlsell.Reservation(0).ReservationId, sCurrency)
        frmOnLine.Attributes.Add("src", GeRequestApplicationPath(pagina))
    End Function

    'Function ReservOnLinePayment(ByVal xmlsell As resHotelSell) As Boolean
    '    Dim sBankOnlineCCPayp As String

    '    sBankOnlineCCPayp = ConfigurationManager.AppSettings("BankOnlineCCPayp")
    '    If sBankOnlineCCPayp = "BMX1" Then
    '        Pagolinea(xmlsell)
    '    Else
    '        PagoLineaBancomer(xmlsell)          
    '    End If
    '    Return True
    'End Function

    Private Sub makeReserva()
        'Response.Redirect("PayBancomer.aspx?pAmount=" & "200.00" & _
        '                       "&pidReservation=" & "15177" & _
        '                       "&pnoReservation=" & "11052710342915177" & _
        '                       "&pCurrency=" & "MXN")


        'Response.Redirect("OnlineConfirmed.aspx?idURL=" & 9 & _
        '                               "&idUser=" & UserIdentityName & _
        '                               "&idReservation=" & "15177" & _
        '                               "&noReservation=" & "11052710342915177" & _
        '                               "&Language=" & PortalCulture.GetIDCulture & "&Currency=" & "MXN")

        'If Not Me.Reservacion Is Nothing Then

        Dim xdocSell As New XmlDataDocument(New reqHotelSell)
        Dim dsreqSell As reqHotelSell = CType(xdocSell.DataSet, reqHotelSell)
        Dim xmlsell As resHotelSell
        Dim drHS As reqHotelSell.HotelSellRow
        Dim AllowOnlineCCPaypment As Boolean
        Dim isPagoLinea As Boolean = False
        Dim Index As Integer = 0

        Boolean.TryParse(ConfigurationManager.AppSettings("AllowOnlineCCPayp"), AllowOnlineCCPaypment)
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
        drc.City = Me.txtCiudad.Text
        drc.Estate = Me.txtEstado.Text
        drc.Email = Me.txtEmail.Text
        dsreqSell.Customer.AddCustomerRow(drc)
        drc.SetParentRow(drHS)

        '//HotelHeader
        Dim drH As reqHotelSell.HotelHeaderRow
        drH = dsreqSell.HotelHeader.NewHotelHeaderRow
        drH.Language = PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper
        drH.Source = "CCT"
        drH.SendMail = True

        If ddlPortal.SelectedIndex = ddlPortal.Items.Count - 1 Then
            drH.IdPortal = 0
        Else
            drH.IdPortal = ddlPortal.SelectedValue
        End If
        If Not String.IsNullOrEmpty(AccessCode) Then drH.AccessCode = AccessCode
        If Not String.IsNullOrEmpty(Convenio) Then drH.ReferenceContract = Convenio

        dsreqSell.HotelHeader.AddHotelHeaderRow(drH)
        drH.SetParentRow(drHS)

        '//Reservation
        Dim drR As reqHotelSell.ReservationRow
        drR = dsreqSell.Reservation.NewReservationRow
        drR.CheckInDate = checkIn
        drR.CheckOutDate = checkOut
        drR.PropertyNumber = Me.cInfoActual.Hotel
        '// Es por tarjeta de credito
        If rbDeposito.Checked Then
            drR.ReserveByBankDeposit = "Y"
        Else
            If AllowOnlineCCPaypment AndAlso IsNetRate Then
                isPagoLinea = True
                drR.OnlinePayment = "Y"
            Else
                drR.CreditCardExpiration = Me.txtMonth.Text & Me.txtYear.Text 'MMYYYY
                drR.CreditCardHolder = Me.txtHolder.Text
                drR.CreditCardNumber = Me.txtNumber.Text
                drR.CreditCardNumberVerify = Me.txtVerify.Text
                drR.CreditCardType = Me.ddlTarjetas.SelectedValue
            End If
        End If
        drR.RateCode = RateCode
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
        For i As Integer = 0 To totalRooms - 1
            'Dim txt As TextBox

            'txt = Request.Form("txtSpecialReq" & i.ToString) 'FindControl("txtPet" & i.ToString)
            Dim rR As reqHotelSell.RoomRow
            rR = dsreqSell.Room.NewRoomRow
            rR.Preferences = ""

            If Adults <> "" Then
                If Adults.Split(",").Length > i - 1 Then
                    rR.Adults = Adults.Split(",")(i)
                End If
            End If
            If Children <> "" Then
                If Children.Split(",").Length > i - 1 Then
                    rR.Children = Children.Split(",")(i)
                End If

                If Not String.IsNullOrEmpty(Ages) Then
                    rR.ChildrenAges = GetChildrenAges(Ages, Index, rR.Children)
                    Index = rR.Children
                End If

            End If

            If Not Request.Form("txtSpecialReq" & i.ToString) Is Nothing Then
                rR.Preferences = Request.Form("txtSpecialReq" & i.ToString)
            End If
            dsreqSell.Room.AddRoomRow(rR)
            rR.SetParentRow(drhrs)
        Next

        With New WSHotelFacade.clsFASELL
            xmlsell = .GetHotelSell(xdocSell.DocumentElement)
        End With

        If xmlsell.Reservation.Rows.Count > 0 Then
            Try
                NoReservacion = xmlsell.Reservation.Rows(0).Item("ConfirmNumber")
                Me.guardalog("CallCenter/Booking.aspx", PaginaBase.acciones.Crear, "Se realizó la reservación con el num. " & xmlsell.Reservation.Rows(0).Item("ConfirmNumber"))
                Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
                Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
                Dim dsres As resHotelDisplay


                If isPagoLinea Then
                    If ReservOnLinePayment(xmlsell) Then
                        Return
                    End If
                End If


                Dim drRDis As reqHotelDisplay.HotelDisplayRow
                drRDis = dsreq.HotelDisplay.NewHotelDisplayRow
                drRDis.ConfirmNumber = xmlsell.Reservation.Rows(0).Item("ConfirmNumber")
                drRDis.Language = PortalCulture.GetCulture.ToString
                dsreq.HotelDisplay.AddHotelDisplayRow(drRDis)

                With New WSHotelFacade.clsFADisplay
                    dsres = .GetHotelDisplay(xdoc.DocumentElement)
                End With

                Dim dt As DataTable
                With New Portal.General.Facade.ReservaFacade
                    dt = .GetDataReservaByNum(xmlsell.Reservation.Rows(0).Item("ConfirmNumber"))
                End With

                MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDRESERVACION))

            Catch ex As Exception
                Me.lblErrorReserva.Text = "No se pudo hacer la reservacion"
                Me.lblErrorReserva.Visible = True
            End Try

        Else
            Me.lblErrorReserva.Text = xmlsell._Error.Rows(0).Item("Message")
            If xmlsell._Error.Rows(0).Item("ErrorCode") = "009" Then
                lblErrorReserva.Text = PortalCulture.GetString("01442")
            End If
            Me.lblErrorReserva.Visible = True
        End If
        'Else
        'Me.lblErrorReserva.Text = "No se pudo hacer la reservacion"
        'Me.lblErrorReserva.Visible = True
        'End If
    End Sub
    Private Sub dgDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDetail.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(0).Text = arrRates(e.Item.ItemIndex).ToString.Split("|")(0)
            e.Item.Cells(1).Text = arrRates(e.Item.ItemIndex).ToString.Split("|")(1)
            If arrRates(e.Item.ItemIndex).ToString.Split("|")(0).IndexOf(PortalCulture.GetString("00170")) >= 0 Then
                e.Item.Cells(0).CssClass = arrRates(e.Item.ItemIndex).ToString.Split("|")(2)
            Else
                e.Item.Cells(1).CssClass = arrRates(e.Item.ItemIndex).ToString.Split("|")(2)
            End If

        End If
    End Sub
    Private Function DetailsRate()
        Dim tarifaTm As Double
        Dim tarifa As Double = Double.MinValue
        Dim tmptarifa As Double
        Dim tmptarifaNinios As Double = 0
        Dim idx As Integer
        Dim tmpdate, tmpAntDate As Date
        Dim subtotal As Double
        Dim tbl As HtmlTable
        Dim drrates() As WSHotelCommon.resHotelRules.RateRow
        Dim drPrices() As WSHotelCommon.resHotelRules.PriceRow
        arrRates = New ArrayList
        Dim tmptar As Double
        Dim dia As Integer
        Dim txttarifa As String
        Dim deposito As Double


        For Each drRoom As WSHotelCommon.resHotelRules.RTRoomRow In ResponseHOR.RTRoom
            arrRates.Add(PortalCulture.GetString("00170") & " #" & (idx + 1).ToString & "||" & "RatesBold")
            subtotal = 0
            tarifa = Double.MinValue
            dia = 0
            For Each dr As WSHotelCommon.resHotelRules.RateRow In ResponseHOR.Rate
                dia += 1
                drPrices = ResponseHOR.Price.Select("SeassonID=" & dr.SeassonId & " and adults=" & drRoom.Adults & _
                    " and children = " & drRoom.Children & " and teens=" & drRoom.Teens)


                If dr.Exc.ToString.ToUpper = "Y" Then
                    tmptarifa = CDbl(drPrices(0).AltRateAdultExc)
                    tmptarifaNinios = CDbl(drPrices(0).AltRateChildExc)
                Else
                    tmptarifa = CDbl(drPrices(0).AltRateAdult)
                    tmptarifaNinios = CDbl(drPrices(0).AltRateChild)
                End If

                If drRoom.ExtraAdults > 0 Then
                    tmptarifa += CDbl(drPrices(0).AltExtraRateAdult) * CDbl(drRoom.ExtraAdults)
                End If

                If drRoom.ExtraChildren > 0 Then
                    tmptarifaNinios += CDbl(drPrices(0).AltExtraRateChild) * (drRoom.ExtraChildren)
                End If
                If Not drRoom.IsExtraTeensNull AndAlso drRoom.ExtraTeens > 0 Then
                    tmptarifaNinios += CDbl(drPrices(0).AltExtraRateTeen) * (drRoom.ExtraTeens)
                End If


                Dim DescChildren As Boolean = False
                If Not drPrices(0).IsDescChildrenWhenApplyNull Then
                    DescChildren = drPrices(0).DescChildrenWhenApply
                End If

                tmptar = tmptarifa + tmptarifaNinios
                Dim DescPromotion As Double
                DescPromotion = ResponseHOR.Room(0).DescPromotion
                If Not drPrices(0).IsDescPromotionNull Then
                    If drPrices(0).DescPromotion > 0 Then
                        DescPromotion = drPrices(0).DescPromotion
                    End If
                End If


                If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
                    tmptarifa = 0
                ElseIf DescPromotion > 0 Then

                    If DescChildren Then
                        tmptarifa += tmptarifaNinios
                        tmptarifa -= (tmptarifa) * DescPromotion / 100
                    Else
                        tmptarifa -= (tmptarifa * DescPromotion / 100)
                        tmptarifa += tmptarifaNinios
                    End If
                Else
                    tmptarifa += tmptarifaNinios
                End If
                txttarifa = ""
                If tarifa <> tmptarifa Then
                    If tarifa <> Double.MinValue Then
                        txttarifa = tarifa.ToString("#,##0.00")
                        If tarifa = 0 Then
                            txttarifa = PortalCulture.GetString("00684")
                        ElseIf tarifaTm <> tarifa Then
                            txttarifa = "<span style='text-decoration:line-through'>" & tarifaTm.ToString("#,##0.00") & "</span>&nbsp;" & tarifa.ToString("#,##0.00")
                        End If

                        If PortalCulture.GetIDCulture = 2 Then
                            If tmpAntDate <> tmpdate Then
                                arrRates.Add(tmpAntDate.ToString("MM/dd") & "-" & tmpdate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                            Else
                                arrRates.Add(tmpAntDate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                            End If
                        Else
                            If tmpAntDate <> tmpdate Then
                                arrRates.Add(tmpAntDate.ToString("dd/MM") & "-" & tmpdate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                            Else
                                arrRates.Add(tmpAntDate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                            End If

                        End If
                    End If
                    tmpAntDate = dr._Date
                    tarifa = tmptarifa
                    tarifaTm = tmptar
                End If
                tmpdate = CDate(dr._Date)
                subtotal += tmptarifa

            Next

            txttarifa = tarifa.ToString("#,##0.00")
            If tarifa = 0 Then
                txttarifa = PortalCulture.GetString("00684")
            ElseIf tarifaTm <> tarifa Then
                txttarifa = "<span style='text-decoration:line-through'>" & tarifaTm.ToString("#,##0.00") & "</span>&nbsp;" & tarifa.ToString("#,##0.00")
            End If


            If PortalCulture.GetIDCulture = 2 Then
                If tmpAntDate <> tmpdate Then
                    arrRates.Add(tmpAntDate.ToString("MM/dd") & "-" & tmpdate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                Else
                    arrRates.Add(tmpAntDate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                End If
            Else
                If tmpAntDate <> tmpdate Then
                    arrRates.Add(tmpAntDate.ToString("dd/MM") & "-" & tmpdate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                Else
                    arrRates.Add(tmpAntDate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
                End If

            End If
            arrRates.Add(PortalCulture.GetString("M0BT0000038") & "|" & subtotal.ToString("#,##0.00") & "|txtDataBold")
            idx += 1
        Next
        dgDetail.DataSource = arrRates
        dgDetail.DataBind()
    End Function

    Private Function getstrFree(ByVal txt As String) As String
        If txt = PortalCulture.GetString("00684") Then
            Return txt
        End If
        Return txt & " " & PortalCulture.GetString("00660")
    End Function

    Private Function GetDetailsRate() As String
        Dim s2 As String = "img"
        Dim exc As String = "", n As Integer
        Dim dv As DataView, dv2 As DataView ', rRate As resHotelComplete.PriceRow,dv3 As DataView, inicio As String,  r As resHotelComplete.RateRow
        n = 0
        Dim i As Integer = 0
        Dim sw As Boolean = True
        Dim Room() As resHotelRules.RTRoomRow
        Dim chkin As Date = New Date(checkIn.Substring(0, 4), checkIn.Substring(4, 2), checkIn.Substring(6, 2)).ToString("MMM/dd/yyyy")
        Dim chkout As Date = New Date(checkOut.Substring(0, 4), checkOut.Substring(4, 2), checkOut.Substring(6, 2)).ToString("MMM/dd/yyyy")
        Dim SeassonId As String
        If Me.ResponseHOR Is Nothing Then Return ""
        dv2 = ResponseHOR.Price.DefaultView
        If dv2.Count = 0 OrElse ResponseHOR.Room.Count < 0 Then Return ""

        Room = ResponseHOR.Room(0).GetRTRoomsRows(0).GetRTRoomRows
        Dim body As String = String.Empty
        body += "<table width='100%' >"
        body += "<tr class='bookingNormalLabel'  ><td  align='center' ></td>"
        body += "   <td  align='center' >" & PortalCulture.GetString("M000645") & "</td>"
        body += "   <td  align='center' >" & PortalCulture.GetString("M000646") & "</td>"
        body += "   <td  align='right' >"
        body += PortalCulture.GetString("00428") & "</td>"
        body += "   <td  align='right' >"
        body += PortalCulture.GetString("00429") & "</td>"
        body += "   <td  align='right' >"
        body += "   </td>"
        body += "</tr>"

        While i <= DateDiff(DateInterval.Day, CDate(chkin), CDate(chkout))
            dv = ResponseHOR.Rate.DefaultView
            Dim dt As Date
            dt = CDate(chkin).AddDays(i)
            'dv.RowFilter = " convert(date,System.DateTime) ='" & CDate(chkin).AddDays(i) & "'"
            dv.RowFilter = String.Format("date='{0}' ", CDate(chkin).AddDays(i).ToString("yyyy/MM/dd"))
            If dv.Count > 0 Then
                If n = 0 Then
                    exc = dv(0)("Exc")
                    SeassonId = dv(0)("SeassonId")
                    n = 1
                    body += "<tr class='bookingNormalLabel'><td  align='left' colspan='6'>" & CDate(chkin).AddDays(i) & "</td></tR>"
                    sw = False
                Else
                    If SeassonId = dv(0)("SeassonId") Then
                        If ResponseHOR.Room(0).DaysFree <> 0 And (i + 1) Mod ResponseHOR.Room(0).DaysFree = 0 Then
                            'Dim dv4 As DataView
                            'dv4 = dv2
                            'dv.RowFilter = " convert(date,System.DateTime) ='" & CDate(chkin).AddDays(i - 1) & "'"
                            dv.RowFilter = String.Format("date='{0}' ", CDate(chkin).AddDays(i - 1).ToString("yyyy/MM/dd"))
                            writeDatosTabla(body, dv2, dv(0)("Exc"), n, Room, i, SeassonId)
                            body += "<tr class='bookingNormalLabel'><td  align='left' colspan='6'>" & CDate(chkin).AddDays(i) & "</td></tR>"

                            'dv.RowFilter = " convert(date,System.DateTime) ='" & CDate(chkin).AddDays(i) & "'"
                            dv.RowFilter = String.Format("date='{0}' ", CDate(chkin).AddDays(i).ToString("yyyy/MM/dd"))
                            writeDatosTabla(body, dv2, dv(0)("Exc"), 1, Room, i + 1, SeassonId)
                            'i += 1
                            n = 0
                            sw = True
                        ElseIf (exc = dv(0)("Exc")) Then
                            n += 1
                            'If SeassonId <> dv(0)("SeassonId") Then
                            '    SeassonId = dv(0)("SeassonId")
                            '    n = 0
                            '    i -= 1
                            '    sw = True
                            'End If
                        Else
                            'Dim dv4 As DataView
                            'dv4 = dv2
                            writeDatosTabla(body, dv2, exc, n, Room, i, SeassonId)
                            n = 0
                            exc = dv(0)("Exc")
                            i -= 1
                            sw = True
                        End If
                    Else
                        writeDatosTabla(body, dv2, exc, n, Room, i, SeassonId)
                        SeassonId = dv(0)("SeassonId")
                        n = 0
                        sw = True
                        i -= 1
                    End If
                End If
            End If
            i += 1
        End While
        If sw = False Then
            'Dim dv4 As DataView
            'dv4 = dv2
            writeDatosTabla(body, dv2, exc, n, Room, i - 1, SeassonId)
        End If
        body += "</table>"
        Return body
    End Function


    Private Sub writeDatosTabla(ByRef body As String, ByVal dv4 As DataView, ByVal exc As String, ByVal n As Integer, ByVal rooms() As resHotelRules.RTRoomRow, ByVal dia As Integer, ByVal SeassonId As String)
        Dim dvr2 As DataView
        Dim dVariantePrecio As Double = -1
        Dim dTotal As Double
        Dim hr As Boolean = False

        dvr2 = ResponseHOR.Rate.DefaultView
        dvr2.RowFilter = ""

        For ocupancy As Integer = 0 To totalRooms - 1
            'Dim etiqueta As Label
            'dv4.RowFilter = "Adults =" & rooms(ocupancy).Item("Adults") & "And Children=" & rooms(ocupancy).Item("Children")
            dv4.RowFilter = "SeassonId =" + SeassonId + " And Adults =" & rooms(ocupancy).Item("Adults") & " And Children=" & rooms(ocupancy).Item("Children")

            If dv4.Count > 0 Then
                'Dim drv As DataRow
                'drv = dv4(0).Row
                For Each drv As DataRowView In dv4
                    dTotal = drv("RateAdult") + drv("RateChild") + drv("ExtraRateAdult") + drv("ExtraRateChild") + drv("RateAdultExc") + drv("RateChildExc") + drv("ExtraRateTeen")
                    If dVariantePrecio <> dTotal Then
                        dVariantePrecio = dTotal
                        dvr2.RowFilter = String.Format("SeassonId= {0}", drv("SeassonId"))
                        ' If sw Then n = dvr2.Count
                        If hr Then '// Ya se puso la fecha por primera vez, hazlo hasta la segunda diferencia de precio que se encuentre
                            body += "<tr class='bookingNormalLabel'><td  align='left' colspan='6'>" & CDate(dvr2(0).Row("Date")) & "</td></tR>"
                        End If
                        hr = True
                        body += "   <tr class='clslabel'>"
                        body += "   <td  align='center' >" & drv("Adults") & " Ad. " & drv("Children") & " Chd"
                        body += "   </td>"
                        If exc.ToUpper = "N" Then
                            If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateAdult"), 2) & "</span><div> Gratis" & "</div></td>"
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateChild"), 2) & "</span><div> Gratis" & "</div></td>"
                            ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateAdult"), 2) & "</span><div> <span>" & CDbl(drv("RateAdult")) - (CDbl(drv("RateAdult")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateChild"), 2) & "</span><div> <span>" & CDbl(drv("RateChild")) - (CDbl(drv("RateChild")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
                            Else
                                body += "   <td  align='center' >" & FCurrency(drv("RateAdult"), 2) & "</td>"
                                body += "   <td  align='center' >" & FCurrency(drv("RateChild"), 2) & "</td>"
                            End If
                        Else

                            If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateAdultExc"), 2) & "</span><div> Gratis" & "</div></td>"
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateChildExc"), 2) & "</span><div> Gratis" & "</div></td>"
                            ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateAdultExc"), 2) & "</span><div><span>" & CDbl(drv("RateAdultExc")) - (CDbl(drv("RateAdultExc")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
                                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("RateChildExc"), 2) & "</span><div><span>" & CDbl(drv("RateChildExc")) - (CDbl(drv("RateChildExc")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
                            Else
                                body += "   <td  align='center' >" & FCurrency(drv("RateAdultExc"), 2) & "</td>"
                                body += "   <td  align='center' >" & FCurrency(drv("RateChildExc"), 2) & "</td>"
                            End If

                        End If

                        If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
                            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults"), 2) & "</span> <div>Gratis" & "</div></td>"
                            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren"), 2) & "</span> <div>Gratis" & "</div></td>"
                        ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
                            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults"), 2) & "</span><div><span>" & CDbl(drv("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults")) - (CDbl(drv("ExtraRateAdult")) * rooms(ocupancy).Item("ExtraAdults") * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
                            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(drv("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren"), 2) & "</span><div><span>" & CDbl(drv("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren")) - (CDbl(drv("ExtraRateChild")) * rooms(ocupancy).Item("ExtraChildren") * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
                        Else
                            body += "   <td  align='center' >" & FCurrency(drv("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults"), 2)
                            body += "   </td>"
                            body += "   <td  align='center' >" & FCurrency(drv("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren"), 2)
                            body += "   </td>"
                        End If


                        body += "   <td  align='center' >" & n & " " & PortalCulture.GetString("00374") & "</td>"
                        body += "   </tr>"
                    End If
                Next
            End If
        Next
        Return


        'For ocupancy As Integer = 0 To totalRooms - 1
        '    'Dim etiqueta As Label
        '    dv4.RowFilter = "Adults =" & rooms(ocupancy).Item("Adults") & "And Children=" & rooms(ocupancy).Item("Children")
        '    If dv4.Count > 0 Then

        '        body += "   <tr class='clslabel'>"
        '        body += "   <td  align='center' >" & dv4(0)("Adults") & " Ad. " & dv4(0)("Children") & " Chd"
        '        body += "   </td>"
        '        If exc.ToUpper = "N" Then
        '            If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateAdult"), 2) & "</span><div> Gratis" & "</div></td>"
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateChild"), 2) & "</span><div> Gratis" & "</div></td>"
        '            ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateAdult"), 2) & "</span><div> <span>" & CDbl(dv4(0)("RateAdult")) - (CDbl(dv4(0)("RateAdult")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateChild"), 2) & "</span><div> <span>" & CDbl(dv4(0)("RateChild")) - (CDbl(dv4(0)("RateChild")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
        '            Else
        '                body += "   <td  align='center' >" & FCurrency(dv4(0)("RateAdult"), 2) & "</td>"
        '                body += "   <td  align='center' >" & FCurrency(dv4(0)("RateChild"), 2) & "</td>"
        '            End If
        '        Else

        '            If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateAdultExc"), 2) & "</span><div> Gratis" & "</div></td>"
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateChildExc"), 2) & "</span><div> Gratis" & "</div></td>"
        '            ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateAdultExc"), 2) & "</span><div><span>" & CDbl(dv4(0)("RateAdultExc")) - (CDbl(dv4(0)("RateAdultExc")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
        '                body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("RateChildExc"), 2) & "</span><div><span>" & CDbl(dv4(0)("RateChildExc")) - (CDbl(dv4(0)("RateChildExc")) * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
        '            Else
        '                body += "   <td  align='center' >" & FCurrency(dv4(0)("RateAdultExc"), 2) & "</td>"
        '                body += "   <td  align='center' >" & FCurrency(dv4(0)("RateChildExc"), 2) & "</td>"
        '            End If

        '        End If

        '        If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
        '            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults"), 2) & "</span> <div>Gratis" & "</div></td>"
        '            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren"), 2) & "</span> <div>Gratis" & "</div></td>"
        '        ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
        '            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults"), 2) & "</span><div><span>" & CDbl(dv4(0)("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults")) - (CDbl(dv4(0)("ExtraRateAdult")) * rooms(ocupancy).Item("ExtraAdults") * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
        '            body += "   <td  align='center' ><span class='PromoStrike'>" & FCurrency(dv4(0)("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren"), 2) & "</span><div><span>" & CDbl(dv4(0)("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren")) - (CDbl(dv4(0)("ExtraRateChild")) * rooms(ocupancy).Item("ExtraChildren") * ResponseHOR.Room(0).DescPromotion / 100) & "</span></div></td>"
        '        Else
        '            body += "   <td  align='center' >" & FCurrency(dv4(0)("ExtraRateAdult") * rooms(ocupancy).Item("ExtraAdults"), 2)
        '            body += "   </td>"
        '            body += "   <td  align='center' >" & FCurrency(dv4(0)("ExtraRateChild") * rooms(ocupancy).Item("ExtraChildren"), 2)
        '            body += "   </td>"
        '        End If


        '        body += "   <td  align='center' >" & n & " " & PortalCulture.GetString("00374") & "</td>"
        '        body += "   </tr>"
        '    End If

        'Next
    End Sub

    'Private Function writeDatosTabla2(ByRef body As String, ByVal dv4 As DataView, ByVal dv2 As DataView, ByVal exc As String, ByVal n As Integer, ByVal rooms() As resHotelRules.RTRoomRow)
    '    'Dim tarifaTm As Double
    '    Dim tarifa As Double = Double.MinValue
    '    Dim tmptarifa As Double
    '    'Dim idx As Integer
    '    'Dim tmpdate, tmpAntDate As Date
    '    Dim subtotal As Double
    '    'Dim tbl As HtmlTable
    '    'Dim drrates() As resHotelRules.RateRow
    '    Dim drPrices() As resHotelRules.PriceRow

    '    Dim tmptar As Double
    '    Dim dia As Integer
    '    Dim txttarifa As String
    '    For Each drRoom As resHotelRules.RTRoomRow In ResponseHOR.RTRoom
    '        '  arrRates.Add(HotelLanguage.GetString("000011") & " #" & (idx + 1).ToString & "||" & "RatesBold")
    '        subtotal = 0
    '        tarifa = Double.MinValue
    '        dia = 0
    '        For Each dr As resHotelRules.RateRow In ResponseHOR.Rate
    '            dia += 1
    '            drPrices = ResponseHOR.Price.Select("SeassonID=" & dr.SeassonId & " and adults=" & drRoom.Adults & _
    '                " and children = " & drRoom.Children)

    '            If Not dr.IsNull("Exc") AndAlso dr.Exc.ToString.ToUpper = "Y" Then
    '                tmptarifa = CDbl(drPrices(0).RateAdultExc) + CDbl(drPrices(0).RateChildExc)
    '            Else
    '                tmptarifa = CDbl(drPrices(0).RateAdult) + CDbl(drPrices(0).RateChild)
    '            End If

    '            If drRoom.ExtraAdults > 0 Then
    '                tmptarifa += CDbl(drPrices(0).ExtraRateAdult) * CDbl(drRoom.ExtraAdults)
    '            End If

    '            If drRoom.ExtraChildren > 0 Then
    '                tmptarifa += CDbl(drPrices(0).ExtraRateChild) * (drRoom.ExtraChildren)
    '            End If


    '            tmptar = tmptarifa
    '            If CInt(ResponseHOR.Room(0).DaysFree) > 0 AndAlso dia Mod ResponseHOR.Room(0).DaysFree = 0 Then
    '                tmptarifa = 0
    '            ElseIf CDbl(ResponseHOR.Room(0).DescPromotion) > 0 Then
    '                tmptarifa -= tmptarifa * ResponseHOR.Room(0).DescPromotion / 100
    '            End If
    '            txttarifa = ""
    '            If tarifa <> tmptarifa Then
    '                If tarifa <> Double.MinValue Then
    '                    txttarifa = tarifa.ToString("#,##0.00")
    '                    If tarifa = 0 Then
    '                        txttarifa = HotelLanguage.GetString("000289")
    '                    ElseIf tarifaTm <> tarifa Then
    '                        txttarifa = "<span style='text-decoration:line-through'>" & tarifaTm.ToString("#,##0.00") & "</span>&nbsp;" & tarifa.ToString("#,##0.00")
    '                    End If


    '                    ' arrRates.Add(tmpAntDate.ToString("dd/MM") & "-" & tmpdate.ToString("dd/MM") & "|" & tarifa.ToString("#,##0.00") & " " & HotelLanguage.GetString( "000232") & "|" & "txtDataBold")
    '                    If HotelLanguage.GetIDCulture = 2 Then
    '                        If tmpAntDate <> tmpdate Then
    '                            arrRates.Add(tmpAntDate.ToString("MM/dd") & "-" & tmpdate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '                        Else
    '                            arrRates.Add(tmpAntDate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '                        End If
    '                    Else
    '                        If tmpAntDate <> tmpdate Then
    '                            arrRates.Add(tmpAntDate.ToString("dd/MM") & "-" & tmpdate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '                        Else
    '                            arrRates.Add(tmpAntDate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '                        End If

    '                    End If
    '                End If
    '                tmpAntDate = dr._Date
    '                tarifa = tmptarifa
    '                tarifaTm = tmptar
    '            End If
    '            tmpdate = CDate(dr._Date)
    '            subtotal += tmptarifa
    '        Next

    '        txttarifa = tarifa.ToString("#,##0.00")
    '        If tarifa = 0 Then
    '            txttarifa = HotelLanguage.GetString("000289")
    '        ElseIf tarifaTm <> tarifa Then
    '            txttarifa = "<span style='text-decoration:line-through'>" & tarifaTm.ToString("#,##0.00") & "</span>&nbsp;" & tarifa.ToString("#,##0.00")
    '        End If


    '        If HotelLanguage.GetIDCulture = 2 Then
    '            If tmpAntDate <> tmpdate Then
    '                arrRates.Add(tmpAntDate.ToString("MM/dd") & "-" & tmpdate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '            Else
    '                arrRates.Add(tmpAntDate.ToString("MM/dd") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '            End If
    '        Else
    '            If tmpAntDate <> tmpdate Then
    '                arrRates.Add(tmpAntDate.ToString("dd/MM") & "-" & tmpdate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '            Else
    '                arrRates.Add(tmpAntDate.ToString("dd/MM") & "|" & Me.getstrFree(txttarifa) & "|" & "txtDataBold")
    '            End If

    '        End If
    '        arrRates.Add(HotelLanguage.GetString("000071") & "|" & subtotal.ToString("#,##0.00") & "|txtDataBold")
    '        idx += 1
    '    Next
    'End Function

    Public Shared Function GetCardId(ByVal code As String) As tTarjeta
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

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect(GeRequestApplicationPath("/Callcenter/booking.aspx"))
    End Sub

    Private Sub enviarcorreo1(ByVal res As resHotelSell, ByVal req As reqHotelSell)
        Dim ci As System.Globalization.CultureInfo
        Dim cc As String = ""
        Dim Mail As emailTemplates.Template

        Try
            Dim idioma As String = ""
            With res.Reservation.Rows(0)
                'NetRates
                Dim resDisp As resHotelDisplay = GetHotelDisplay(.Item("ConfirmNumber"))
                Dim isNetRateUV As Boolean = GetIsNetRateUV(resDisp)
                Dim totalNR As Double = GetTotalNR(resDisp)
                Dim taxesNR As Double = GetTaxesNR(resDisp)
                Dim subTotalNR As Double = (totalNR - taxesNR)


                'el correo del hotel
                If Not .IsNull("PropertyNumber") Then
                    Dim hotel As HotelDatos
                    With New HotelSistema
                        hotel = .GetHotelById(res.Reservation.Rows(0).Item("PropertyNumber"))
                    End With
                    If hotel.Tables.Count > 0 AndAlso hotel.Tables(HotelDatos.HOTEL_TABLE).Rows.Count > 0 AndAlso hotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_EMAIL_RESERVAS) <> "" Then
                        cc = hotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_EMAIL_RESERVAS)
                        idioma = hotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0).Item(HotelDatos.FIELD_IDIOMAEMAIL).ToString
                    End If
                End If
                If idioma = "" Then
                    idioma = PortalCulture.GetCulture.ToString
                End If

                ci = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(idioma)

                Mail = New emailTemplates.Template




                If MyBase.cInfoActual.EsMoroso Then
                    Mail.TemplateName = "T15"
                Else
                    Mail.TemplateName = "T09"
                End If


                If cc <> "" Then
                    Mail.Cc = cc
                End If

                Mail.SubjectParam = .Item("ConfirmNumber")

                Mail.To = AppSettings("UnivisitMail")
                Mail.AddParameter("ID_RESERVACION") = .Item("ConfirmNumber")
                Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name


                Mail.AddParameter("CHECKIN") = GetDateString(.Item("CheckInDate"))
                Mail.AddParameter("CHECKOUT") = GetDateString(.Item("CheckOutDate"))



                Mail.AddParameter("NOCHES") = DateDiff(DateInterval.Day, GetDate(.Item("CheckInDate")), GetDate(.Item("CheckOutDate")))


                If isNetRateUV Then
                    'NetRate
                    Mail.AddParameter("SUBTOTAL") = FCurrency(subTotalNR, 2) & " " & .Item("Currency")
                    Mail.AddParameter("TAX") = FCurrency(taxesNR, 2) & " " & .Item("Currency")
                    Mail.AddParameter("GRANTOTAL") = FCurrency(totalNR, 2) & " " & .Item("Currency")
                Else ' normal
                    Dim total As Double
                    total = CDbl(.Item("Total"))
                    Mail.AddParameter("SUBTOTAL") = FCurrency(CDbl(.Item("Total")), 2) & " " & .Item("Currency")
                    If .Item("PlusTax") Then
                        Mail.AddParameter("TAX") = PortalCulture.GetString("00433")
                    Else
                        Mail.AddParameter("TAX") = FCurrency(CDbl(.Item("Taxes")), 2) & " " & .Item("Currency")
                        total += CDbl(.Item("Taxes"))
                    End If
                    Mail.AddParameter("GRANTOTAL") = FCurrency(total, 2) & " " & .Item("Currency")
                End If





            End With
            With req.Customer.Rows(0)

                If .Item("Email") <> "" Then
                    Mail.AddParameter("EMAIL_CONTACTO") = .Item("Email")
                Else
                    Mail.AddParameter("EMAIL_CONTACTO") = "NA"
                End If

                Dim nombre As String = ""
                If Not .IsNull("FirstName") AndAlso .Item("FirstName") <> "" Then
                    nombre = .Item("FirstName") & " "
                End If
                If Not .IsNull("LastName") AndAlso .Item("LastName") <> "" Then
                    nombre &= .Item("LastName")
                End If
                Mail.AddParameter("CONTACTO") = nombre
                If .Item("PhoneHome") <> "" Then
                    Mail.AddParameter("TEL_CASA") = .Item("PhoneHome")
                Else
                    Mail.AddParameter("TEL_CASA") = "-"
                End If
                If .Item("PhoneWork") <> "" Then
                    Mail.AddParameter("TEL_TRABAJO") = .Item("PhoneWork")
                Else
                    Mail.AddParameter("TEL_TRABAJO") = "-"
                End If

            End With

            Mail.AddParameter("UNIVISITPORTAL") = "CALL CENTER"
            Mail.Html = True

            With ResponseHOR._Property.Rows(0)

                Mail.AddParameter("HOTEL_NOMBRE") = CType(Me.Page, PaginaBase).cInfoActual.HotelName
                Mail.AddParameter("HOTEL_DIRECCION") = .Item("Address")
                Mail.AddParameter("HOTEL_CIUDAD") = .Item("City")
                Mail.AddParameter("HOTEL_PAIS") = .Item("Country")
            End With

            Dim ad As Integer, ch As Integer
            For Each dr As resHotelSell.RoomRow In res.Room
                ad += dr("Adults")
                ch += dr("Children")
            Next

            Mail.AddParameter("ADULTOS") = ad
            Mail.AddParameter("NINIOS") = ch
            Mail.AddParameter("HABITACIONES") = res.Room.Count


            Mail.Send()
        Catch ex As Exception
            Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = ci
        End Try
    End Sub

    Private Function GetDate(ByVal strDate As String) As Date
        Return New Date(strDate.Substring(0, 4), strDate.Substring(4, 2), strDate.Substring(6, 2))

    End Function

    Private Function GetDateString(ByVal strDate As String) As String
        Return MonthName(strDate.Substring(4, 2), True) & "/" & strDate.Substring(6, 2) & "/" & strDate.Substring(0, 4)
    End Function

    Private Function CheckPortalRateCode(ByVal ds As DataSet, ByVal idportal As Integer) As Boolean
        Dim dv As DataView
        Dim hr As Boolean

        hr = False
        If dsEmpty(ds) Then Return True
        dv = ds.Tables(0).DefaultView
        dv.RowFilter = String.Format("idPortal= {0}", idportal)
        If (dv.Count > 0) Then
            hr = True
        Else
            dv.RowFilter = String.Format("idPortal= {0}", 0)
            hr = If(dv.Count > 0, True, False)
        End If
        Return hr
    End Function

    Private Sub FillPortals(ByVal rateCode As String)

        Dim data As Portal.Catalogos.Common.Data.clsCommonPortales
        Dim canUsePublics As Boolean = (AppSettings("UsePublicPortals") IsNot Nothing AndAlso AppSettings("UsePublicPortals").ToLower() = "true")
        Dim canUsePublicsSup As Boolean = False
        Dim dsPortalsRateCode As DataSet


        dsPortalsRateCode = PortalsRateCode(rateCode)
        Me.ddlPortal.Items.Clear()

        With New Portal.Catalogos.Facade.clsFacadePortales()

            If (Me.IsSupervisor) Then
                Dim currentCorporate As Integer = 0
                Dim currentAsociation As Integer = 0
                If AppSettings("IdCorporate") IsNot Nothing Then Integer.TryParse(AppSettings("IdCorporate"), currentCorporate)
                If AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(AppSettings("IdAsociation"), currentAsociation)
                If currentCorporate > 0 Then
                    data = .GetPortalsByCorporate(currentCorporate)
                ElseIf currentAsociation > 0 Then
                    data = .GetPortalsByAsociation(currentAsociation)
                Else
                    data = .GetPortales(cInfoActual.Empresa)
                    canUsePublicsSup = True
                End If
            ElseIf (Me.isUserChain AndAlso Me.IdCorporativoUserChain > 0) Then
                data = .GetPortalsByCorporate(Me.IdCorporativoUserChain)
            ElseIf (Me.IsUsuarioHotelAssociation AndAlso Me.IdAsociation > 0) Then
                data = .GetPortalsByAsociation(Me.IdAsociation)
            Else
                data = .GetPortals(onlyPublics:=True, corporate:=Me.cInfoActual.IdCorporate, asociation:=Me.cInfoActual.IdAsociation)
            End If

        End With

        If data IsNot Nothing AndAlso data.Tables.Contains(data.TABLA_PORTALES) AndAlso data.Tables(data.TABLA_PORTALES).Rows.Count > 0 Then
            For Each row As DataRow In data.Tables(data.TABLA_PORTALES).Rows
                If canUsePublics OrElse (canUsePublicsSup And row("IsPublic") <> 2) OrElse row("IsPublic") = 0 Then
                    '// Checa si el portal esta disponible para la tarifa
                    If CheckPortalRateCode(dsPortalsRateCode, row(data.FLD_IDPORTAL)) Then

                        Dim item As New ListItem()
                        item.Text = StrConv(row(data.FLD_NOMBRE), VbStrConv.ProperCase)
                        item.Value = row(data.FLD_IDPORTAL).ToString()
                        item.Selected = True
                        Me.ddlPortal.Items.Add(item)
                    End If
                End If
            Next

            If Me.IsSupervisor AndAlso canUsePublics Then
                Me.ddlPortal.Items.Insert(0, New ListItem("-- " & PortalCulture.GetString("00753") & " --", 0))
            End If
        Else
            Me.ddlPortal.Items.Add(New ListItem("-- " & PortalCulture.GetString("M000482") & " --", 0))
            Me.ddlPortal.Enabled = False
        End If

        Me.ddlPortal.SelectedIndex = 0

    End Sub

    'Sub cargarPortal()
    '    Dim dsDatos2 As DataSet = buscarDatos()
    '    Dim i As Integer
    '    Dim j As Integer
    '    Dim ban As Boolean
    '    Dim nombre As String

    '    Try
    '        ddlPortal.Items.Clear()

    '        If Not (dsDatos2 Is Nothing) Then
    '            For i = 0 To dsDatos2.Tables(0).Rows.Count - 1
    '                ban = False

    '                For j = 0 To ddlPortal.Items.Count - 1
    '                    nombre = dsDatos2.Tables(0).Rows(i).Item("Name")
    '                    If ddlPortal.Items.Item(j).Text.ToLower = nombre.ToLower Then
    '                        ban = True
    '                    End If
    '                Next

    '                If ban = False Then
    '                    ddlPortal.Items.Add(New ListItem(dsDatos2.Tables(0).Rows(i).Item("Name"), dsDatos2.Tables(0).Rows(i).Item("Id")))
    '                End If
    '            Next

    '            dsDatos2.ReadXml(sourceXML)
    '            ddlPortal.Items.Add("-- " & PortalCulture.GetString("00753") & " --")
    '            If ddlPortal.Items.Count - 1 > 0 Then
    '                ddlPortal.SelectedIndex = ddlPortal.Items.Count - 1
    '            End If
    '        Else
    '            ddlPortal.Items.Add("-- " & PortalCulture.GetString("00754") & " --")
    '        End If

    '    Catch ex As Exception
    '    End Try
    'End Sub

    'Protected Function buscarDatos() As DataSet
    '    Dim dsDatos2 As DataSet = New DataSet

    '    Try
    '        If (Not File.Exists(sourceXML)) Then
    '            ddlPortal.Items.Add("-- " & PortalCulture.GetString("00754") & " --")

    '            Return Nothing
    '        End If

    '        dsDatos2.ReadXml(sourceXML)
    '    Catch ex As Exception
    '        Dim mensaje As String = ex.Message
    '        dsDatos2 = Nothing
    '    End Try

    '    Return dsDatos2
    'End Function

    Private Function GetHotelDisplay(ByVal noReservacion As String) As resHotelDisplay
        Try
            Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
            Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
            Dim resDis As resHotelDisplay


            Dim drR As reqHotelDisplay.HotelDisplayRow
            drR = dsreq.HotelDisplay.NewHotelDisplayRow
            drR.ConfirmNumber = noReservacion
            drR.Language = "es-MX"
            dsreq.HotelDisplay.AddHotelDisplayRow(drR)

            With New WSHotelFacade.clsFADisplay
                resDis = .GetHotelDisplay(xdoc.DocumentElement)
            End With
            Return resDis
        Catch

        End Try
        Return Nothing

    End Function
    Private Function GetIsNetRateUV(ByVal res As resHotelDisplay) As Boolean
        Try
            Return res.Reservation(0).IsNetRateUV
        Catch
        End Try
        Return False
    End Function
    Private Function GetTotalNR(ByVal res As resHotelDisplay) As Double
        Try
            If Not res.Reservation(0).IsTotalNRNull() Then
                Return res.Reservation(0).TotalNR
            End If
        Catch
        End Try
        Return 0
    End Function
    Private Function GetTaxesNR(ByVal res As resHotelDisplay) As Double
        Try
            If Not res.Reservation(0).IsTaxesNRNull() Then
                Return res.Reservation(0).TaxesNR
            End If
        Catch
        End Try
        Return 0
    End Function

    Protected Sub cmdPayment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdPayment.Click
        Dim dt As DataTable
        With New Portal.General.Facade.ReservaFacade
            dt = .GetDataReservaByNum(NoReservacion)
        End With
        'frmOnLine.Style.Add("Left", "0px")
        MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & dt.Rows(0)(Portal.General.Common.Data.ReservaDatos.FIELD_IDRESERVACION))
    End Sub

    Private Function GetRes(ByVal noReservacion As String) As DataSet
        Dim ds As New DataSet
        With New SqlDataAdapter("spReservationGetByNumber", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
            .SelectCommand.CommandType = CommandType.StoredProcedure
            .SelectCommand.Parameters.Add("@NoReservacion", SqlDbType.NVarChar, 24).Value = noReservacion
            Try
                .SelectCommand.Connection.Open()
                .Fill(ds)
            Catch ex As Exception
            Finally
                .SelectCommand.Connection.Close()
            End Try
        End With
        Return ds
    End Function

    Private Function PortalsRateCode(ByVal rateCode As String) As DataSet
        Dim ds As New DataSet
        With New SqlDataAdapter("spAplicacionportalByRateCode", New SqlConnection(ConfigurationManager.AppSettings("HotelConnection")))
            .SelectCommand.CommandType = CommandType.StoredProcedure
            .SelectCommand.Parameters.Add("@RateCode", SqlDbType.NVarChar, 10).Value = rateCode
            .SelectCommand.Parameters.Add("@idhotel", SqlDbType.Int).Value = cInfoActual.Hotel
            Try
                .SelectCommand.Connection.Open()
                .Fill(ds)
            Catch ex As Exception
            Finally
                .SelectCommand.Connection.Close()
            End Try
        End With
        Return ds
    End Function

    Private Function LocalCancel(ByVal idReservacion As String, ByVal idUser As String) As Boolean
        Dim ds As New DataSet
        Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("HotelConnection"))
        Dim CancelNumber As String
        Dim trans As Boolean
        trans = False

        Try
            sqlconn.Open()
            Dim transacc As SqlTransaction = sqlconn.BeginTransaction
            trans = True
            Try
                Dim sqlcmd As New SqlCommand("spReservationCancel", sqlconn)
                sqlcmd.CommandType = CommandType.StoredProcedure
                sqlcmd.Transaction = transacc
                sqlcmd.Parameters.Add("@idReservacion", SqlDbType.Int).Value = idReservacion

                '// Genera el numero de cancelacion @NoCancelacion
                CancelNumber = "CX" & Format(Now(), "yy") & Format(Now(), "MM") & Format(Now(), "dd") & Format(Now(), "HH") & Format(Now(), "mm") & Format(Now(), "ss") & idUser
                sqlcmd.Parameters.Add("@NoCancelacion", SqlDbType.NVarChar, 50).Value = CancelNumber
                sqlcmd.Parameters.Add("@MotivoCancelacion", SqlDbType.NVarChar, 250).Value = "Cancelacion en pago en linea. " & PortalCulture.GetString("00780", True)
                sqlcmd.Parameters.Add("@iduser", SqlDbType.Int).Value = MyBase.UserIdentityName

                Dim afec As Integer = sqlcmd.ExecuteNonQuery()
                transacc.Commit()
                trans = False
                If afec > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch e As Exception
                If trans Then transacc.Rollback()
                If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
                Return False
            Finally
                If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            End Try

        Catch ex As Exception
        End Try

    End Function

    Protected Sub cmdCancelaReserva_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancelaReserva.Click
        Dim idreserva As Integer
        Dim ds As New DataSet

        ds = GetRes(NoReservacion)
        If (Not ds Is Nothing) AndAlso (ds.Tables(0).Rows.Count > 0) Then
            idreserva = ds.Tables(0).Rows(0)("idReservacion")
            LocalCancel(idreserva, Request.QueryString("idUser"))
            MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & idreserva)
        End If
    End Sub

End Class
