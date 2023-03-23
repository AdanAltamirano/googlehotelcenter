Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml
Imports System.Text
Imports WSHotelCommon
Imports WSHotelFacade
Imports System.Globalization
Imports System.IO
Imports Portal.General.Facade


Partial Class ctrlDeposits
    Inherits UserControlBase
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
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
    Protected Fecha As _Date

    Public Property Reservation() As String
        Get
            Return Me.txtReservacion.Text
        End Get
        Set(ByVal Value As String)
            Me.txtReservacion.Text = Value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'para probar correo.
        'GetDataWS("12032012433015620")
        If Not Me.IsPostBack Then
            CargaMonedas()
        End If
    End Sub
    Private Sub CargaMonedas()
        ddlMoneda.DataSource = (New MonedaSistema).GetMonedaListIdName
        ddlMoneda.DataTextField = "Codigo"
        ddlMoneda.DataValueField = "idMoneda"
        ddlMoneda.DataBind()

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'lblTitle.Text = PortalCulture.GetString("00691")
        Me.lblFecha.Text = PortalCulture.GetString("M000120", True)
        Me.lblFechaRes.Text = PortalCulture.GetString("00392", True)
        Me.lblCiudad.Text = PortalCulture.GetString("00254", True)
        lblReservacion.Text = PortalCulture.GetString("M000119", True)
        lblCliente.Text = PortalCulture.GetString("00616", True)
        lblCheckin.Text = PortalCulture.GetString("M000078", True)
        lblCheckout.Text = PortalCulture.GetString("M000079", True)
        Me.lblAmount.Text = PortalCulture.GetString("00699", True)
        Me.lblAmountDep.Text = PortalCulture.GetString("00700", True)
        Me.lblBanco.Text = PortalCulture.GetString("00694", True)
        Me.lblCuenta.Text = PortalCulture.GetString("00693", True)
        Me.lblReferencia.Text = PortalCulture.GetString("00695", True)
        Me.lblObservacion.Text = PortalCulture.GetString("00696", True)
        Me.rfvBanco.Text = PortalCulture.GetString("00071")
        Me.rfvCuenta.Text = Me.rfvBanco.Text
        Me.rfvMonto.Text = Me.rfvBanco.Text
        Me.rngvMonto.Text = PortalCulture.GetString("M000105")

    End Sub

    Private Sub Nuevo()
        Call Limpia()
    End Sub

    Private Sub Limpia()
        Me.txtBanco.Text = ""
        Me.txtAmount.Text = ""
        Me.txtAmountDep.Text = ""
        Me.txtCheckin.Text = ""
        Me.txtCheckout.Text = ""
        Me.txtCliente.Text = ""
        Me.txtCuenta.Text = ""
        Me.txtHotel.Text = ""
        Me.txtObservacion.Text = ""
        Me.txtReferencia.Text = ""
        Me.txtReservacion.Text = ""
        Me.Fecha.selectedDate = Today
        viewstate("idReservacion") = Nothing
    End Sub

    Function CreaDsLog(ByVal noReserva As String, ByVal noCuenta As String, ByVal banco As String, ByVal depMonto As String, _
                       ByVal dep_moneda As String, ByVal fecha As String, _
                       ByVal observacion As String, ByVal idusuario As String) As String
        Dim ds As New DataSet
        Dim dt As DataTable = New DataTable("Deposit")
        Dim dr As DataRow

        Try
            With dt.Columns
                .Add(New DataColumn("noReservacion", GetType(System.String)))
                .Add(New DataColumn("nocuenta", GetType(System.String)))
                .Add(New DataColumn("banco", GetType(System.String)))
                .Add(New DataColumn("dep_monto", GetType(System.String)))
                .Add(New DataColumn("dep_moneda", GetType(System.String)))
                .Add(New DataColumn("fecha", GetType(System.String)))
                .Add(New DataColumn("observacion", GetType(System.String)))
                .Add(New DataColumn("idUsuario", GetType(System.String)))
            End With
            ds.Tables.Add(dt)

            dr = ds.Tables(0).NewRow
            dr("noReservacion") = noReserva
            dr("nocuenta") = noCuenta
            dr("banco") = banco
            dr("dep_monto") = depMonto
            dr("dep_moneda") = dep_moneda
            dr("fecha") = fecha
            dr("observacion") = observacion
            dr("idUsuario") = idusuario
            ds.Tables(0).Rows.Add(dr)
        Catch ex As Exception
        End Try
        Return Util.Utility.GetXml("Deposit", "UpdateDeposit", ds)

    End Function

    Public Function Save(ByVal noRseravacion As String, ByRef sDatos As String) As Boolean
        Save = False

        Dim ConnectionString As String = AppSettings("HotelConnectionString")
        Dim dsCommand As New SqlCommand
        Dim conex As SqlConnection
        conex = New SqlConnection(ConnectionString)
        conex.Open()
        Dim trans As SqlTransaction
        Dim tr As Boolean
        Dim monto As Double
        Dim dt As Date

        trans = conex.BeginTransaction
        tr = True

        Try
            dsCommand.CommandType = CommandType.StoredProcedure
            dsCommand.CommandText = "spReservationsByDeposit_Update"
            dsCommand.Connection = conex
            dsCommand.Transaction = trans
            dsCommand.Parameters.Add(New SqlParameter("@idReservacion", SqlDbType.Int)).Value = ViewState("idReservacion")
            dsCommand.Parameters.Add(New SqlParameter("@nocuenta", SqlDbType.NVarChar, 20)).Value = Me.txtCuenta.Text
            dsCommand.Parameters.Add(New SqlParameter("@banco", SqlDbType.NVarChar, 20)).Value = Me.txtBanco.Text
            dsCommand.Parameters.Add(New SqlParameter("@dep_monto", SqlDbType.Money)).Value = Me.txtAmountDep.Text
            dsCommand.Parameters.Add(New SqlParameter("@dep_moneda", SqlDbType.NVarChar, 3)).Value = ddlMoneda.SelectedItem.Text
            dsCommand.Parameters.Add(New SqlParameter("@fecha", SqlDbType.SmallDateTime)).Value = Fecha.selectedDate
            dsCommand.Parameters.Add(New SqlParameter("@observacion", SqlDbType.NVarChar, 50)).Value = Me.txtObservacion.Text
            dsCommand.Parameters.Add(New SqlParameter("@idUsuario", SqlDbType.Int)).Value = (New AuthUser).Usuario 'Page.User.Identity.Name
            Dim r As Long
            r = dsCommand.ExecuteNonQuery()

            If r > 0 Then
                'Correcto
                Dim dsCommand2 As New SqlCommand
                dsCommand2.Connection = conex
                dsCommand2.Transaction = trans
                dsCommand2.CommandType = CommandType.StoredProcedure
                dsCommand2.Parameters.Clear()
                dsCommand2.CommandText = "spReservationUpdateStatus"
                dsCommand2.Parameters.Add(New SqlParameter("@idReservacion", SqlDbType.Int)).Value = ViewState("idReservacion")
                dsCommand2.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt)).Value = 1
                r = dsCommand2.ExecuteNonQuery
                If r > 0 Then ' El sp tiene dos veces el modificar[spReservationUpdateStatus], por que antes era r > 1 aqui
                    trans.Commit()
                    tr = False
                    Save = True

                    Double.TryParse(txtAmountDep.Text, monto)
                    Date.TryParse(Fecha.selectedDate, dt)
                    sDatos = CreaDsLog(noRseravacion, txtCuenta.Text, txtBanco.Text, monto.ToString("########0.00"), ddlMoneda.SelectedItem.Text,
                              dt.ToString("yyyy-MM-dd hh:mm"), txtObservacion.Text, (New AuthUser).UserInfoName)
                Else
                    trans.Rollback()
                    tr = False
                End If
            Else
                trans.Rollback()
                tr = False
            End If
        Catch e As Exception
            If tr Then trans.Rollback()
            lblMens.Text = e.Message
            lblMens.Visible = True
        Finally
            If conex.State = ConnectionState.Broken Or conex.State = ConnectionState.Open Then
                conex.Close()
            End If
        End Try
    End Function

    Public Sub ConfirmPaymentRequest(ByVal reference As String)
        Dim connectionString As String = AppSettings("OzPayment")
        If String.IsNullOrEmpty(connectionString) Then
            Return
        End If

        Using connection As New SqlConnection(connectionString)

            connection.Open()
            'buscamos la reserva
            Try
                Dim dr As SqlDataReader
                Dim q As String = "select * from paymentrequest where reference = @reference and method = 'crypto' and authorized = 0"

                Dim f As Boolean = False
                Using cmd As New SqlCommand(q, connection)
                    cmd.Parameters.AddWithValue("@reference", reference)
                    dr = cmd.ExecuteReader()
                    f = dr.HasRows
                End Using
                dr.Close()

                If f Then
                    q = "update paymentrequest set authorized = 1, authorizationNumber = @authorizationNumber where reference = @reference and method = 'crypto'"

                    Using cmd As New SqlCommand(q, connection)
                        cmd.Parameters.AddWithValue("@reference", reference)
                        cmd.Parameters.AddWithValue("@authorizationNumber", txtReferencia.Text)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            Catch ex As Exception
                If connection.State = ConnectionState.Broken Or connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try

            connection.Close()
        End Using
    End Sub

    Public Function enviarcorreo_conf() As Boolean

        'Call GetDataWS(Me.txtReservacion.Text)
        If lblMens.Visible Then Return False

        Return True
        'Dim Mail As emailTemplates.Template
        'Dim cc As String
        'Try
        '    Mail = New emailTemplates.Template
        '    Mail.TemplateName = "T13_HOTELDEPOSITCONFIRM"
        '    Mail.To = lblEmailCli.Text
        '    Mail.SubjectParam = Me.txtReservacion.Text ' .Item(dsreservaciones.FIELD_NORESERVACION)
        '    Mail.Cc = AppSettings("UnivisitMail")
        '    Mail.Html = True
        '    Mail.SubjectParam = Me.txtReservacion.Text '.Item(dsreservaciones.FIELD_NORESERVACION)
        '    Mail.Idioma = System.Threading.Thread.CurrentThread.CurrentCulture.Name
        '    Mail.AddParameter("UNIVISITPORTAL") = PortalCulture.GetString("00516")
        '    Mail.AddParameter("HOTELNAME") = txtHotel.Text 'IIf(.IsNull(dsreservaciones.FIELD_HOTELGNOMBRE), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGNOMBRE))
        '    Mail.AddParameter("CITY") = txtCiudad.Text 'IIf(.IsNull(dsreservaciones.FIELD_HOTELGCIUDAD), PortalCulture.GetString("M000604"), .Item(dsreservaciones.FIELD_HOTELGCIUDAD))
        '    Mail.AddParameter("RESERVATIONNUMBER") = Me.txtReservacion.Text '.Item(dsreservaciones.FIELD_NORESERVACION)
        '    Mail.AddParameter("STARTDATE") = txtCheckin.Text 'CDate(.Item(dsreservaciones.FIELD_CHECKIN)).ToString("dd/MMM/yyyy")
        '    Mail.AddParameter("ENDDATE") = txtCheckout.Text 'CDate(.Item(dsreservaciones.FIELD_CHECKOUT)).ToString("dd/MMM/yyyy")
        '    Mail.AddParameter("CUSTOMERNAME") = txtCliente.Text '.Item(dsreservaciones.FIELD_CLIENTE)
        '    Mail.AddParameter("REGDATE") = txtFechaRes.Text 'CDate(.Item("FechaReservacion")).ToString("dd/MMM/yyyy")
        '    Mail.Send()
        'Catch ex As Exception
        '    Dim sErrorMessage As String = String.Format("The HTML fragment file '{0}' ", ex.ToString)
        'Finally

        'End Try
    End Function

    Private Sub GetDataWS(ByVal nores As String)
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
                'Dim idioma_orig As String
                'Dim idioma As String
                'idioma_orig = PortalCulture.GetCulture.ToString
                'Dim Prov As New PortalPartnersCfg
                'Prov.LoadPartnerById(xml.HotelHeader.Rows(0).Item("idPortal"))
                'idioma = "en_US"
                'If Prov.CurrencyCode = "MXN" Then idioma = "es-MX"
                'PortalCulture.SetCulture(idioma)
                ' Call MandarCorreoAlHotel(xml)
                'Call CorreoHotel(xml)
                Dim idioma As String = PortalCulture.GetCulture.ToString
                If Not xml.Reservation(0).IsNull("IdIdiomaReservation") Then
                    If xml.Reservation(0).IdIdiomaReservation = 2 Then
                        idioma = "en-US"
                    Else
                        idioma = "es-MX"
                    End If
                End If
                With New Miscelaneos.SendHotelEmails
                    .sendCustomerEmailReservation(xml, idioma)                    
                    .SendEmailtoAlHotel(xml, idioma)

                    Util.Utility.MailerSend("Reserva", .GetBody())
                End With

                ' PortalCulture.SetCulture(idioma_orig)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MandarCorreoAlHotel(ByVal resDisp As resHotelDisplay)
        Try
            Dim Mail As emailTemplates.Template = New emailTemplates.Template
            Dim com As Double
            Dim Prov As New PortalPartnersCfg
            Prov.LoadPartnerById(resDisp.HotelHeader.Rows(0).Item("idPortal"))


            'Obtener la preferencia del envio
            Try
                Mail.Html = True 'ctrlLoginHotel1.Passport.getUserAccessInfo.EmailType
            Catch ex As Exception
                Mail.Html = True

            End Try


            Dim x As Byte
            Dim totAd As Byte = 0
            Dim totNi As Byte = 0
            Dim totAdExtras As Byte = 0
            Dim totChExtras As Byte = 0

            For Each dr As resHotelDisplay.RoomRow In resDisp.Room
                totAd += CInt(dr.Adults) + CInt(dr.ExtraAdults)
                totNi += CInt(dr.Children) + CInt(dr.ExtraChildren)
            Next
            Dim idioma As String
            idioma = "en-US"
            If Prov.CurrencyCode.ToUpper = "MXN" Then idioma = "es-MX"
            Mail.Idioma = idioma 'PortalCulture.GetCulture.ToString ' "en" 'HotelLanguage.GetCulture.ToString
            Mail.SubjectParam = resDisp.Reservation(0).ConfirmNumber  '  Reservacion.ReservationNumber
            'Mail.TemplateName = "T13_HOTELDEPOSITCONFIRM"
            Mail.TemplateName = "T15_ReservationToHotel"
            Mail.To = resDisp.Reservation(0).EmailReservations
            Mail.Bcc = AppSettings("UnivisitMail")

            Mail.AddParameter("HEADER") = Prov.EmailHeader
            Mail.AddParameter("FOOTER") = Prov.EmailFooter
            Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css ' type='text/css' rel='stylesheet'>" '"<style>" & ReadCSS(Prov.StyleSheets & "correos.css") & "</style>"
            Mail.AddParameter("PORTALNAME") = Prov.Name
            Mail.AddParameter("URLSITE") = Prov.NonSecureSite



            Mail.AddParameter("HOTEL_NOMBRE") = resDisp.Reservation(0).HotelName
            ' Mail.AddParameter("ID_RESERVACION", resDisp.Reservation(0).ConfirmNumber)
            Mail.AddParameter("CONTACTO") = resDisp.Customer(0).FirstName & " " & resDisp.Customer(0).LastName
            Mail.AddParameter("EMAIL_CONTACTO") = resDisp.Customer(0).Email

            Dim phone As String = "''"
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
            phone = "''"
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


            'Dim strCardNumber As String
            'Dim MaskedCardNumber As String

            'strCardNumber = resDisp.Reservation(0).CreditCardNumber

            'Dim strNewCN As String = strCardNumber.Substring(strCardNumber.Length - 4, 4) '// caracteres 
            'MaskedCardNumber = "<p>" & "************" & strNewCN & "</p>"
            'MaskedCardNumber &= "<p>" & resDisp.Reservation(0).CreditCardHolder() & "</p>"
            'Mail.AddParameter("TARJETACREDITO") = MaskedCardNumber
            Mail.AddParameter("TARJETACREDITO") = "*" & PortalCulture.GetString("00714")
            Mail.AddParameter("DEPOSITO") = ""

            Mail.AddParameter("HOTEL_DIRECCION") = resDisp.Reservation(0).HotelAddress
            Mail.AddParameter("HOTEL_CIUDAD") = resDisp.Reservation(0).CityName
            Mail.AddParameter("HOTEL_PAIS") = resDisp.Reservation(0).CountryName

            If PortalCulture.GetCulture.ToString = "en-MX" Then
                Mail.AddParameter("CHECKIN") = CDate(resDisp.Reservation(0).CheckInDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("CHECKOUT") = CDate(resDisp.Reservation(0).CheckOutDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("REGDATE") = CDate(resDisp.Reservation(0).ReservationDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            Else
                Mail.AddParameter("CHECKIN") = CDate(resDisp.Reservation(0).CheckInDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("CHECKOUT") = CDate(resDisp.Reservation(0).CheckOutDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
                Mail.AddParameter("REGDATE") = CDate(resDisp.Reservation(0).ReservationDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            End If

            Mail.AddParameter("ADULTOS") = totAd
            Mail.AddParameter("NINIOS") = totNi
            Dim nhab As String = ""
            nhab = resDisp.Room(0).NameRoom
            Mail.AddParameter("HABITACIONES") = resDisp.Room.Count & " " & nhab
            Mail.AddParameter("NOCHES") = DateDiff(DateInterval.Day, CDate(resDisp.Reservation(0).CheckInDate), CDate(resDisp.Reservation(0).CheckOutDate))

            For Each dr As resHotelDisplay.RoomRow In resDisp.Room
                Mail.AddParameter("Clientes") = dr.TravelerName
            Next


            Mail.AddParameter("ROOMDETAILS") = GetRoomsDetails(resDisp)

            Dim DetailsRate As String
            Dim DetailsRateValue As String
            Dim emailnote As String

            Dim AltTotal As Single
            Dim AltTaxes As Single
            Dim AltMoney As String
            Dim tc As Single
            If resDisp.Reservation(0).Money <> Prov.CurrencyCode Then
                'tc = getTC
            End If
            AltTotal = resDisp.Reservation(0).Total
            AltTaxes = resDisp.Reservation(0).Taxes
            AltMoney = resDisp.Reservation(0).Money


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

                emailnote = "*" & PortalCulture.GetString("00704")
            End If
            Mail.AddParameter("TEXTOTAR") = DetailsRate
            Mail.AddParameter("VALORESTAR") = DetailsRateValue


            If AltMoney <> resDisp.Reservation(0).Money AndAlso AltMoney = Prov.CurrencyCode Then 'existe un tipo de moneda diferente mostrado.
                If emailnote <> "" Then
                    emailnote &= "<br/>"
                Else
                    emailnote = "*"
                End If
                Dim cadmon = PortalCulture.GetString("00702")   'falta reemplazar el total en el original de la moneda.
                cadmon = String.Format(cadmon, FCurrency(CDbl(resDisp.Reservation.Rows(0).Item("Total")), 2)) & " " & resDisp.Reservation(0).Money

                emailnote &= PortalCulture.GetString("00701") & " " & cadmon
            End If
            If emailnote <> "" Then emailnote = "<DIV  class='footerCard'><p>" & emailnote & "</p></div>"
            Mail.AddParameter("CALCULODETAIL") = emailnote
            Mail.AddParameter("ESTANCIATOTAL") = FCurrency(com + AltTotal, 2) & " " & AltMoney
            Mail.AddParameter("AGENCYINFO") = ""
            Mail.AddParameter("LINKRESERVATION") = Prov.SecureSite & "/hotel/Secure/DisplayReservation.aspx?ConfirmNum=" & resDisp.Reservation(0).ConfirmNumber

            Mail.Send()

        Catch Emsg As Exception
            'PortalServiceTracer.ServiceTracer("No se pudo enviar el correo de reservacion a " & resDisp.Reservation(0).EmailReservations & " el error ocurrido es:" & Emsg.ToString, PortalServiceTacerErrorTypes.Warning)
            'tracer
            lblMens.Text = Emsg.ToString
            lblMens.Visible = True
        Finally
        End Try
    End Sub

    Private Sub MandarCorreoLogHotel(ByVal sDato As String, ByVal sdatoDespues As String)
        Try
            'Dim Mail As emailTemplates.Template = New emailTemplates.Template
            'Dim com As Double
            'Dim Prov As New PortalPartnersCfg
            'Prov.LoadPartnerById(resDisp.HotelHeader.Rows(0).Item("idPortal"))


            ''Obtener la preferencia del envio
            'Try
            '    Mail.Html = True 'ctrlLoginHotel1.Passport.getUserAccessInfo.EmailType
            'Catch ex As Exception
            '    Mail.Html = True

            'End Try


            'Dim x As Byte
            'Dim totAd As Byte = 0
            'Dim totNi As Byte = 0
            'Dim totAdExtras As Byte = 0
            'Dim totChExtras As Byte = 0

            'For Each dr As resHotelDisplay.RoomRow In resDisp.Room
            '    totAd += CInt(dr.Adults) + CInt(dr.ExtraAdults)
            '    totNi += CInt(dr.Children) + CInt(dr.ExtraChildren)
            'Next
            'Dim idioma As String
            'idioma = "en-US"
            'If Prov.CurrencyCode.ToUpper = "MXN" Then idioma = "es-MX"
            'Mail.Idioma = idioma 'PortalCulture.GetCulture.ToString ' "en" 'HotelLanguage.GetCulture.ToString
            'Mail.SubjectParam = resDisp.Reservation(0).ConfirmNumber  '  Reservacion.ReservationNumber
            ''Mail.TemplateName = "T13_HOTELDEPOSITCONFIRM"
            'Mail.TemplateName = "T15_ReservationToHotel"
            'Mail.To = resDisp.Reservation(0).EmailReservations
            'Mail.Bcc = AppSettings("UnivisitMail")

            'Mail.AddParameter("HEADER") = Prov.EmailHeader
            'Mail.AddParameter("FOOTER") = Prov.EmailFooter
            'Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css ' type='text/css' rel='stylesheet'>" '"<style>" & ReadCSS(Prov.StyleSheets & "correos.css") & "</style>"
            'Mail.AddParameter("PORTALNAME") = Prov.Name
            'Mail.AddParameter("URLSITE") = Prov.NonSecureSite



            'Mail.AddParameter("HOTEL_NOMBRE") = resDisp.Reservation(0).HotelName
            '' Mail.AddParameter("ID_RESERVACION", resDisp.Reservation(0).ConfirmNumber)
            'Mail.AddParameter("CONTACTO") = resDisp.Customer(0).FirstName & " " & resDisp.Customer(0).LastName
            'Mail.AddParameter("EMAIL_CONTACTO") = resDisp.Customer(0).Email

            'Dim phone As String = "''"
            'If Not resDisp.Customer(0).IsPhoneHomeNull Then
            '    Dim PhoneHome() As String = CType(resDisp.Customer(0).PhoneHome, String).Split("+")

            '    Dim show As Integer = 0
            '    If PhoneHome.Length > 0 Then
            '        For Each st As String In PhoneHome
            '            If st.Trim <> "" Then
            '                show += 1
            '            End If
            '        Next
            '        If show > 2 Then
            '            phone = resDisp.Customer(0).PhoneHome.ToString.Replace("+", "-")
            '        End If
            '    End If
            'End If
            'Mail.AddParameter("TEL_CASA") = phone
            'phone = "''"
            'If Not resDisp.Customer(0).IsPhoneWorkNull Then
            '    Dim phoneWork() As String = CType(resDisp.Customer(0).PhoneWork, String).Split("+")
            '    Dim show As Integer = 0
            '    If phoneWork.Length > 0 Then
            '        For Each st As String In phoneWork
            '            If st.Trim <> "" Then
            '                show += 1
            '            End If
            '        Next
            '        If show > 2 Then
            '            phone = resDisp.Customer(0).PhoneWork.ToString.Replace("+", "-")
            '        End If
            '    End If
            'End If
            'Mail.AddParameter("TEL_TRABAJO") = phone


            ''Dim strCardNumber As String
            ''Dim MaskedCardNumber As String

            ''strCardNumber = resDisp.Reservation(0).CreditCardNumber

            ''Dim strNewCN As String = strCardNumber.Substring(strCardNumber.Length - 4, 4) '// caracteres 
            ''MaskedCardNumber = "<p>" & "************" & strNewCN & "</p>"
            ''MaskedCardNumber &= "<p>" & resDisp.Reservation(0).CreditCardHolder() & "</p>"
            ''Mail.AddParameter("TARJETACREDITO") = MaskedCardNumber
            'Mail.AddParameter("TARJETACREDITO") = "*" & PortalCulture.GetString("00714")
            'Mail.AddParameter("DEPOSITO") = ""

            'Mail.AddParameter("HOTEL_DIRECCION") = resDisp.Reservation(0).HotelAddress
            'Mail.AddParameter("HOTEL_CIUDAD") = resDisp.Reservation(0).CityName
            'Mail.AddParameter("HOTEL_PAIS") = resDisp.Reservation(0).CountryName

            'If PortalCulture.GetCulture.ToString = "en-MX" Then
            '    Mail.AddParameter("CHECKIN") = CDate(resDisp.Reservation(0).CheckInDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            '    Mail.AddParameter("CHECKOUT") = CDate(resDisp.Reservation(0).CheckOutDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            '    Mail.AddParameter("REGDATE") = CDate(resDisp.Reservation(0).ReservationDate).ToString("MMM/dd/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            'Else
            '    Mail.AddParameter("CHECKIN") = CDate(resDisp.Reservation(0).CheckInDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            '    Mail.AddParameter("CHECKOUT") = CDate(resDisp.Reservation(0).CheckOutDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            '    Mail.AddParameter("REGDATE") = CDate(resDisp.Reservation(0).ReservationDate).ToString("dd/MMM/yyyy", New CultureInfo(PortalCulture.GetCulture.ToString))
            'End If

            'Mail.AddParameter("ADULTOS") = totAd
            'Mail.AddParameter("NINIOS") = totNi
            'Dim nhab As String = ""
            'nhab = resDisp.Room(0).NameRoom
            'Mail.AddParameter("HABITACIONES") = resDisp.Room.Count & " " & nhab
            'Mail.AddParameter("NOCHES") = DateDiff(DateInterval.Day, CDate(resDisp.Reservation(0).CheckInDate), CDate(resDisp.Reservation(0).CheckOutDate))

            'For Each dr As resHotelDisplay.RoomRow In resDisp.Room
            '    Mail.AddParameter("Clientes") = dr.TravelerName
            'Next


            'Mail.AddParameter("ROOMDETAILS") = GetRoomsDetails(resDisp)

            'Dim DetailsRate As String
            'Dim DetailsRateValue As String
            'Dim emailnote As String

            'Dim AltTotal As Single
            'Dim AltTaxes As Single
            'Dim AltMoney As String
            'Dim tc As Single
            'If resDisp.Reservation(0).Money <> Prov.CurrencyCode Then
            '    'tc = getTC
            'End If
            'AltTotal = resDisp.Reservation(0).Total
            'AltTaxes = resDisp.Reservation(0).Taxes
            'AltMoney = resDisp.Reservation(0).Money


            'If resDisp.Reservation(0).Provider = 0 Then
            '    DetailsRate &= "<p>Subtotal:</p>"
            '    DetailsRate &= "<p>" & PortalCulture.GetString("00703", True) & "</p>"
            '    DetailsRate &= "<p>Total:</p>"


            '    If resDisp.Reservation(0).PlusTax = "False" Then
            '        DetailsRateValue &= "<p>" & FormatCurrency(AltTotal - AltTaxes, 2) & " " & AltMoney & "</p>"
            '        DetailsRateValue &= "<p>" & FormatCurrency(AltTaxes, 2) & " " & AltMoney & "</p>"
            '    Else
            '        DetailsRateValue &= "<p>" & FormatCurrency(AltTotal, 2) & " " & AltMoney & "</p>"
            '        DetailsRateValue &= "<p>" & PortalCulture.GetString("00705") & "</p>"
            '    End If
            '    DetailsRateValue &= "<p>" & FormatCurrency(AltTotal, 2) & " " & AltMoney & "</p>"

            '    Mail.AddParameter("ID_RESERVACION") = resDisp.Reservation(0).ConfirmNumber
            'Else

            '    DetailsRate &= "<p>*Total:</p>"

            '    DetailsRateValue &= "<p>" & FormatCurrency(AltTotal, 2) & " " & AltMoney & "</p>"

            '    Mail.AddParameter("ID_RESERVACION") = "UV: " & resDisp.Reservation(0).ConfirmNumber & ", GAL: " & resDisp.Reservation(0).RecLoc

            '    emailnote = "*" & PortalCulture.GetString("00704")
            'End If
            'Mail.AddParameter("TEXTOTAR") = DetailsRate
            'Mail.AddParameter("VALORESTAR") = DetailsRateValue


            'If AltMoney <> resDisp.Reservation(0).Money AndAlso AltMoney = Prov.CurrencyCode Then 'existe un tipo de moneda diferente mostrado.
            '    If emailnote <> "" Then
            '        emailnote &= "<br/>"
            '    Else
            '        emailnote = "*"
            '    End If
            '    Dim cadmon = PortalCulture.GetString("00702")   'falta reemplazar el total en el original de la moneda.
            '    cadmon = String.Format(cadmon, CDbl(resDisp.Reservation.Rows(0).Item("Total")).ToString("$#,###,##0.00")) & " " & resDisp.Reservation(0).Money

            '    emailnote &= PortalCulture.GetString("00701") & " " & cadmon
            'End If
            'If emailnote <> "" Then emailnote = "<DIV  class='footerCard'><p>" & emailnote & "</p></div>"
            'Mail.AddParameter("CALCULODETAIL") = emailnote
            'Mail.AddParameter("ESTANCIATOTAL") = FormatCurrency(com + AltTotal, 2) & " " & AltMoney
            'Mail.AddParameter("AGENCYINFO") = ""
            'Mail.AddParameter("LINKRESERVATION") = Prov.SecureSite & "/hotel/Secure/DisplayReservation.aspx?ConfirmNum=" & resDisp.Reservation(0).ConfirmNumber

            'Mail.Send()

        Catch Emsg As Exception
            'PortalServiceTracer.ServiceTracer("No se pudo enviar el correo de reservacion a " & resDisp.Reservation(0).EmailReservations & " el error ocurrido es:" & Emsg.ToString, PortalServiceTacerErrorTypes.Warning)
            'tracer
            lblMens.Text = Emsg.ToString
            lblMens.Visible = True
        Finally
        End Try
    End Sub


    Private Function CargaPoliticas(ByVal prov As PortalPartnersCfg) As String
        Try
            Dim str As String
            Dim idioma As String
            idioma = "_EN"
            If prov.CurrencyCode.ToUpper = "MXN" Then idioma = "_ES"
            str = ReadHTML(GeRequestApplicationPath(String.Concat("/Politicas/HotelPolicies", idioma, ".txt")), prov)
            str &= "<br/>"
            Return str
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub CorreoHotel(ByVal resDisp As resHotelDisplay)
        Try
            Dim Mail As emailTemplates.Template = New emailTemplates.Template
            Dim Prov As New PortalPartnersCfg
            Prov.LoadPartnerById(resDisp.HotelHeader.Rows(0).Item("idPortal"))

            Dim com As Double
            'Obtener la preferencia del envio
            Dim politicas As String
            politicas = CargaPoliticas(Prov)
            '''''''''''''''''''''''''''''''''
            Mail.Html = True

            Dim x As Byte
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
            idioma = "en-US"
            If Prov.CurrencyCode.ToUpper = "MXN" Then idioma = "es-MX"

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

            '//Obtengo el numero de la tarjeta de credito....
            'Dim strCardNumber As String
            'Dim MaskedCardNumber As String
            'strCardNumber = resDisp.Reservation(0).CreditCardNumber
            'Dim strNewCN As String = strCardNumber.Substring(strCardNumber.Length - 4, 4) '// caracteres 
            'MaskedCardNumber = "************" & strNewCN '//12 X más los ultimos 4 numeros
            'MaskedCardNumber &= "<br/>" & resDisp.Reservation(0).CreditCardHolder()
            'Mail.AddParameter("TARJETACREDITO") = MaskedCardNumber
            'If com > 0 Then
            '    'Mail.AddParameter("DEPOSITO") = String.Format(HotelLanguage.GetString("00801"), FormatCurrency(com, 2))
            'Else
            '    Mail.AddParameter("DEPOSITO") = PortalCulture.GetString("00713")
            'End If
            Mail.AddParameter("TARJETACREDITO") = "*" & PortalCulture.GetString("00714")
            Mail.AddParameter("DEPOSITO") = ""

            If Mail.Html = False Then
                Dim REx As System.Text.RegularExpressions.Regex
                politicas = politicas.Replace("&nbsp;", " ")
                politicas = politicas.Replace("</p>", ControlChars.CrLf)
                politicas = REx.Replace(politicas, "&(?ni:\#((x([\dA-F]){1,5})|(104857[0-5]|10485[0-6]\d|1048[0-4]\d\d|104[0-7]\d{3}|10[0-3]\d{4}|0?\d{1,6}))|([A-Za-z\d.]{2,31}));|<[^>]*>", "")
            End If



            Mail.AddParameter("POLITICAS") = politicas
            Mail.AddParameter("URLPOLICIES") = Prov.SecureSite & "/hotel/hoteldescription.aspx?Provider=" & resDisp.Reservation(0).Provider & "&PropertyNumber=" & resDisp.Reservation(0).PropertyNumber & "&tab=Policies"
            Dim Emailnote As String = ""



            Dim DetailsRate As String
            Dim DetailsRateValue As String


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
                Dim cadmon = PortalCulture.GetString("00702")
                cadmon = String.Format(cadmon, fCurrency(CDbl(resDisp.Reservation.Rows(0).Item("Total")), 2)) & " " & resDisp.Reservation(0).Money
                Emailnote &= PortalCulture.GetString("00701") & " " & cadmon

            End If
            If Emailnote <> "" Then Emailnote = "<DIV  class='footerCard'><p>" & Emailnote & "</p></div>"
            Mail.AddParameter("CALCULODETAIL") = Emailnote
            Mail.AddParameter("AGENCYINFO") = ""
            Mail.AddParameter("LINKRESERVATION") = Prov.SecureSite & "/hotel/Secure/DisplayReservation.aspx?ConfirmNum=" & resDisp.Reservation(0).ConfirmNumber
            Mail.Send()

        Catch exp As Exception
            'PortalServiceTracer.ServiceTracer("No se pudo enviar el correo de reservacion a " & resDisp.Customer(0).Email & " el error ocurrido es:" & exp.ToString, PortalServiceTacerErrorTypes.Warning)
            lblMens.Text = exp.ToString
            lblMens.Visible = True
        Finally
        End Try
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
    Public Shared Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function

    Private Function getTC(ByVal Cur As Single) As Single
        Dim Monedas As Portal.General.Common.Data.MonedaDatos = (New Portal.General.Facade.MonedaSistema).GetMonedaListIdName
        If Monedas Is Nothing Then Exit Function 'salte si no se encontro como realizar la conversion. 
        Dim drs As DataRow()
        Dim TC As Decimal = 1
        Dim Va As Decimal
        Dim nTC As Decimal = 1
        drs = Monedas.Tables(0).Select("codigo='" & Cur & "'")
        If drs.Length > 0 Then
            TC = drs(0).Item("TipoCambio")
        Else
            Exit Function 'no tengo ese tipo de cambio
        End If

    End Function

    Private Function ReadCSS(ByVal RssFile As String)
        Try
            Dim tr As TextReader
            Dim str As String

            tr = New StreamReader(System.Web.HttpContext.Current.Server.MapPath(RssFile), System.Text.Encoding.Default)
            str = tr.ReadToEnd
            tr.Close()
            Return str
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function ReadHTML(ByVal RssFile As String, ByVal Prov As PortalPartnersCfg)
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

    Private Function ReplaceParameters(ByVal htmlvalue As String, ByVal Prov As PortalPartnersCfg) As String
        Dim PPConf As New PortalPartnersCfg
        htmlvalue = htmlvalue.Replace("{SiteName}", PPConf.Name)
        htmlvalue = htmlvalue.Replace("{DomainName}", PPConf.DomainName)
        Return htmlvalue
    End Function

    Private Sub DropDownList1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged, ddlMoneda.SelectedIndexChanged

    End Sub
    Public Function GetReservationData(ByVal NoReservation As String)
        Call Limpia()
        Dim ds As DataSet
        With New despositFacade
            ds = .GetReservationData(NoReservation)
        End With
        If Not IsNothing(ds) Then
            If ds.Tables(0).Rows.Count > 0 Then
                Me.txtReservacion.Text = NoReservation
                Me.txtCliente.Text = ds.Tables(0).Rows(0).Item("Cliente")
                'If ds.Tables(0).Rows(0).Item("idUsuario") = 0 Then
                'Me.txtCliente.Text = ds.Tables(0).Rows(0).Item("nombre_cl") & " " & ds.Tables(0).Rows(0).Item("apellido_cl")
                'Else
                '   Me.txtCliente.Text = ds.Tables(0).Rows(0).Item("clinombre") & " " & ds.Tables(0).Rows(0).Item("cliapellido")
                'End If
            Me.txtFechaRes.Text = Format(ds.Tables(0).Rows(0).Item("FechaReservacion"), "dd/MMM/yyy HH:mm")

            Me.txtCheckin.Text = Format(ds.Tables(0).Rows(0).Item("checkin"), "dd/MMM/yyy")
            Me.txtCheckout.Text = Format(ds.Tables(0).Rows(0).Item("checkout"), "dd/MMM/yyy")
            Me.txtHotel.Text = ds.Tables(0).Rows(0).Item("hotel")
            Me.txtCiudad.Text = ds.Tables(0).Rows(0).Item("ciudad")
            Me.lblEmailCli.Text = ds.Tables(0).Rows(0).Item("cli_email")
                'Me.txtAmount.Text = FCurrency(ds.Tables(0).Rows(0).Item("monto"), 2) & " " & ds.Tables(0).Rows(0).Item("moneda")
                Me.txtAmount.Text = ds.Tables(0).Rows(0).Item("monto") & " " & ds.Tables(0).Rows(0).Item("moneda")
                Me.txtReferencia.Text = ds.Tables(0).Rows(0).Item("Referencia")
                ViewState("idReservacion") = ds.Tables(0).Rows(0).Item("idReservacion")
        End If
        End If

    End Function
End Class
Public Class despositFacade
    Public Function GetReservationData(ByVal NoReservation As String) As DataSet
        With New DepositAccess
            Return .GetReservationData(NoReservation)
        End With
    End Function
End Class


Public Class DepositAccess
    Public Function GetReservationData(ByVal NoReservation As String) As DataSet
        Dim ds As New DataSet
        Dim dv As DataView
        Dim ConnectionString As String = AppSettings("HotelConnectionString")

        Dim dsCommand As New SqlDataAdapter
        dsCommand.SelectCommand = New SqlCommand
        With dsCommand
            Try
                With .SelectCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "spReservationsByDeposit_GetInfoByNoReservation"
                    .Connection = New SqlConnection(ConnectionString)
                    .Parameters.Add(New SqlParameter("@NoReservacion", SqlDbType.NVarChar, 24)).Value = NoReservation
                End With
                .Fill(ds)
                Return ds
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
        Return Nothing
    End Function
End Class

