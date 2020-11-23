Imports System.Globalization
Imports System.Threading
Imports APIServices.Models
Imports RateManager.API.Models
Imports System.Configuration
Imports APIServices.Models.DTO

Namespace API.Helpers

    Public Enum TypeClient
        Customer
        Hotel
        Support
    End Enum
    Public Module Email
        Private username As String = ConfigurationManager.AppSettings("usernameNotification")
        Private password As String = ConfigurationManager.AppSettings("passwordNotification")
        Private host As String = ConfigurationManager.AppSettings("hostNotification")
        Private port As Integer = CInt(ConfigurationManager.AppSettings("portNotification"))

        'Sub New()
        '    Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
        '    PortalCulture.SetCulture(Thread.CurrentThread.CurrentCulture.Name)
        'End Sub

        Public Function SendVerificationCodeEmail(ByVal code As String) As Boolean

            Dim mail As New emailTemplates.Template
            mail.To = GetUserEmail()
            mail.TemplateName = "CodeCC"
            mail.Html = True
            mail.SubjectParam = "credit card"
            mail.AddParameter("code") = code
            mail.Idioma = Thread.CurrentThread.CurrentCulture.Name
            Try
                mail.Send()
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Function SendModificationEmail(ByVal toEmail As String, ByVal reservation As ReservationDetailsModel,
                                              ByVal oldReservation As ReservationDetailsModel, ByRef emailError As String, ByVal typeClient As TypeClient) As Boolean
            Try
                'Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
                'PortalCulture.SetCulture(Thread.CurrentThread.CurrentCulture.Name)

                Dim lang As String = "es-MX"

                Dim adults As Integer = 0
                Dim childrens As Integer = 0
                Dim rooms As String = String.Empty
                For Each roomDetail As RoomDetails In reservation.RoomDetails
                    adults += roomDetail.Adults + roomDetail.ExtraAdults
                    childrens += roomDetail.Childrens + roomDetail.ExtraChildrens
                    Dim occupy As String = roomDetail.Adults & IIf(lang = "es-MX", " Adulto", " Adult") & "(s)"
                    Dim occupyChildren As Integer = roomDetail.Childrens
                    Dim occupyExtraAdults As Integer = roomDetail.ExtraAdults
                    Dim occupyExtraChildren As Integer = roomDetail.ExtraChildrens
                    Dim arrivalRoom As String = roomDetail.PriceDetails.ElementAt(0).CheckIn.ToString("dd MMMM yyyy")
                    Dim departureRoom As String = roomDetail.PriceDetails.ElementAt(0).CheckOut.ToString("dd MMMM yyyy")

                    If (occupyChildren <> 0) Then occupy += ", " + occupyChildren.ToString() + IIf(lang = "es-MX", " Niño", " Children") + "(s)"
                    If (occupyExtraAdults <> 0) Then occupy += ", " + occupyExtraAdults.ToString() + IIf(lang = "es-MX", " Adulto", " Adult") & "(s) Extra"
                    If (occupyExtraChildren <> 0) Then occupy += ", " + occupyExtraChildren.ToString() + IIf(lang = "es-MX", " Niño", " Children") + "(s) Extra"
                    rooms += "<tr style='padding:0; text-align:left; vertical-align:top'>"
                    rooms += "<th style='Margin:0; color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;padding:0;text-align:left'>"
                    rooms += "<div style='background-color:#fff;background-clip: border-box;border:1px solid rgba(0,0,0,.125);border-radius:.25rem; width:100%;'>"
                    rooms += "<div style='font-weight:normal;color:#6c757d !important;font-size:18px;padding:2px 16px; padding:.75rem 1.25rem;margin-bottom:0;background-color:rgba(0, 0, 0, .03);border-bottom:1px solid rgba(0, 0, 0, .125);'>"
                    rooms += roomDetail.RoomCode + " - " + roomDetail.Name
                    rooms += "</div>"
                    rooms += "<div style='padding:1.25rem; padding:2px 16px;'>"
                    rooms += "<address style='font-weight:normal;font-size:16px; font-style:normal; line-height:1.5em;'>"

                    rooms += "<strong>" + IIf(lang = "es-MX", "Ocupación", "Ocupation") + "</strong>"
                    rooms += occupy
                    rooms += "<br>"
                    rooms += "<strong>" + IIf(lang = "es-MX", "Plan tarifario: ", "Rate Plan: ") + "</strong>"
                    rooms += roomDetail.RateCode + " - " + roomDetail.RatePlan
                    rooms += "<br>"
                    If (Not String.IsNullOrEmpty(roomDetail.Preferences)) Then
                        rooms += "<strong>" + IIf(lang = "es-MX", "Preferencias: ", "Preferences: ") + " </strong>"
                        rooms += roomDetail.Preferences
                        rooms += "<br>"
                    End If
                    If (Not String.IsNullOrEmpty(roomDetail.RatePlanPromotion) And Not String.IsNullOrEmpty(roomDetail.NamePromotion)) Then
                        rooms += "<strong>" + IIf(lang = "es-MX", "Promoción: ", "Promotion: ") + " </strong>"
                        rooms += roomDetail.RatePlanPromotion + " - " + roomDetail.NamePromotion
                        rooms += "<br>"
                    End If
                    rooms += "<strong>" + IIf(lang = "es-MX", "Fecha: ", "Date: ") + "</strong>"
                    rooms += arrivalRoom + " - " + departureRoom
                    rooms += "<br>"
                    'Precios
                    GetPriceRoom(typeClient, rooms, lang, roomDetail)
                    'Termina Precios
                    rooms += "<br>"
                    rooms += "</address>"
                    rooms += "</div>"
                    rooms += "</div>"
                    rooms += "</th>"
                    rooms += "</tr>"
                    rooms += "<tr style='padding:0;text-align:left;vertical-align:top'>"
                    rooms += " <td height='16px' style='-moz-hyphens:auto;-webkit-hyphens:auto;Margin:0;border-collapse:collapse!important; color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;hyphens:auto;line-height:16px;margin:0;mso-line-height-rule:exactly;padding:0;text-align:left;vertical-align:top;word-wrap:break-word'>"
                    rooms += " &nbsp;"
                    rooms += "</td></tr>"

                Next

                Dim paymentType As String = String.Empty
                Dim total As Double = reservation.TotalDetails.Total
                Dim oldTotal As Double = oldReservation.TotalDetails.Total
                Dim referenceInfo As String = String.Empty
                Dim reference As String = String.Empty
                Dim pasarela As String = String.Empty
                Dim oldTotalNetRate As Double = oldReservation.TotalDetails.TotalNR
                Dim totalNetRate As Double = reservation.TotalDetails.TotalNR
                Dim totalInfo As String = String.Empty

                GetPaymentWayInfo(reservation, lang, paymentType, referenceInfo, reference, pasarela)


                Dim mail As New emailTemplates.Template
                mail.To = toEmail
                mail.TemplateName = "T17_HOTELRESERVATIONMODIFICATION"
                mail.Html = True
                mail.SubjectParam = reservation.ReservationId
                mail.Idioma = "es-MX" 'Thread.CurrentThread.CurrentCulture.Name

                mail.AddParameter("NOMBREDELCLIENTE") = reservation.Customer.Name & " " & reservation.Customer.LastName
                mail.AddParameter("NOMBREDELCLIENTEANTERIOR") = oldReservation.Customer.Name & " " & oldReservation.Customer.LastName
                mail.AddParameter("FECHA") = Now.Date.ToString("dd/MMM/yyyy")
                mail.AddParameter("NOMBREDELHOTEL") = reservation.HotelName
                mail.AddParameter("DIRECCION") = reservation.Address
                mail.AddParameter("NUMERODERESERVACION") = reservation.ReservationId
                mail.AddParameter("FECHADERESERVACION") = reservation.ReservationDate.ToString("dd MMMM yyyy")
                mail.AddParameter("TELEFONODELCLIENTE") = reservation.Customer.Phone
                mail.AddParameter("CORREOELECTRONICO") = reservation.Customer.Email
                mail.AddParameter("MESLLEGADADETALLES") = reservation.CheckIn.Value.ToString("MMMM")
                mail.AddParameter("FECHALLEGADADETALLES") = reservation.CheckIn.Value.Day
                mail.AddParameter("ANOLLEGADADETALLES") = reservation.CheckIn.Value.Year
                mail.AddParameter("MESSALIDADETALLES") = reservation.CheckOut.Value.ToString("MMMM")
                mail.AddParameter("FECHASALIDADETALLES") = reservation.CheckOut.Value.Day
                mail.AddParameter("ANOSALIDADETALLES") = reservation.CheckOut.Value.Year
                mail.AddParameter("MESLLEGADADETALLESANTERIOR") = oldReservation.CheckIn.Value.ToString("MMMM")
                mail.AddParameter("FECHALLEGADADETALLESANTERIOR") = oldReservation.CheckIn.Value.Day
                mail.AddParameter("ANOLLEGADADETALLESANTERIOR") = oldReservation.CheckIn.Value.Year
                mail.AddParameter("MESSALIDADETALLESANTERIOR") = oldReservation.CheckOut.Value.ToString("MMMM")
                mail.AddParameter("FECHASALIDADETALLESANTERIOR") = oldReservation.CheckOut.Value.Day
                mail.AddParameter("ANOSALIDADETALLESANTERIOR") = oldReservation.CheckOut.Value.Year
                mail.AddParameter("NOCHE") = reservation.Nights
                mail.AddParameter("ADULTO") = adults
                mail.AddParameter("NINO") = childrens
                mail.AddParameter("MOTIVODEMODIFICACION") = reservation.ModificationReason
                mail.AddParameter("POLITICASHOTELCANCELACION") = reservation.PolicyDetails.HotelCancellation
                mail.AddParameter("POLITICASHOTELGARANTIA") = reservation.PolicyDetails.HotelGuarantee
                mail.AddParameter("POLITICASHOTELTARJETA") = reservation.PolicyDetails.HotelCreditCard
                mail.AddParameter("POLITICASPLANCANCELACION") = reservation.PolicyDetails.RatePlanCancellation
                mail.AddParameter("POLITICASPLANGARANTIA") = reservation.PolicyDetails.RatePlanGuarantee
                mail.AddParameter("POLITICASPLANTARJETA") = reservation.PolicyDetails.RatePlanCreditCard
                mail.AddParameter("TIPODEPAGO") = paymentType
                mail.AddParameter("REFERENCIAINFO") = referenceInfo
                mail.AddParameter("REFERENCIA") = reference
                If reservation.PaymentDetails IsNot Nothing Then
                    If Not String.IsNullOrEmpty(reservation.PaymentDetails.Pasarela) Then
                        mail.AddParameter("PASARELA") = pasarela
                    End If
                End If

                GetTotalInfo(typeClient, totalInfo, reservation, oldReservation)

                mail.AddParameter("TOTAL") = totalInfo

                'Select Case typeClient
                '    Case TypeClient.Customer
                '        'mail.AddParameter("TOTAL") = total & " " + reservation.TotalDetails.Currency
                '        'mail.AddParameter("TOTALANTERIOR") = oldTotal & " " + oldReservation.TotalDetails.Currency
                '    Case TypeClient.Hotel
                '        'mail.AddParameter("TOTALNR") = totalNetRate
                '        'TODO: Add TOTALNR IN TEMPLATES XML,Correos HTML in REservation FUnction And Cancel Function
                'End Select



                mail.AddParameter("HABITACIONES") = rooms


                mail.Send()
                Return True
            Catch ex As Exception
                emailError = ex.Message
                emailError &= " "
                emailError &= ex.StackTrace
                Return False
            End Try
            Return False
        End Function

        Public Function SendCancellationEmail(ByVal rdm As ReservationDetailsModel,
                                              ByVal toEmail As String, ByRef emailError As String) As Boolean

            'Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            'PortalCulture.SetCulture(Thread.CurrentThread.CurrentCulture.Name)
            Try
                Dim lang As String = "es-MX"
                Dim paymentType As String = String.Empty
                Dim total As Double = rdm.TotalDetails.Total
                Dim referenceInfo As String = String.Empty
                Dim reference As String = String.Empty
                Dim pasarela As String = String.Empty

                GetPaymentWayInfo(rdm, lang, paymentType, referenceInfo, reference, pasarela)

                Dim mail As New emailTemplates.Template
                mail.To = toEmail
                mail.TemplateName = "T12_HOTELCANCELLATION"
                mail.Html = True
                mail.SubjectParam = rdm.ReservationId
                mail.Idioma = "es-MX" 'Thread.CurrentThread.CurrentCulture.Name

                'parametros
                mail.AddParameter("HOTELNAME") = rdm.HotelName 'rsv.hotelName 
                mail.AddParameter("MOTIVO") = rdm.CancellationReason 'rsv.cancellationReason
                mail.AddParameter("CITY") = rdm.City 'rsv.city
                mail.AddParameter("RESERVATIONNUMBER") = rdm.ReservationId 'rsv.reservationId
                mail.AddParameter("CANCELLATIONNUMBER") = rdm.CancellationNumber 'rsv.cancellationNumber
                mail.AddParameter("STARTDATE") = rdm.CheckIn.Value.ToString("dd/MMM/yyy") 'rsv.checkIn.ToString("dd/MMM/yyyy")
                mail.AddParameter("ENDDATE") = rdm.CheckOut.Value.ToString("dd/MMM/yyy") 'rsv.checkOut.ToString("dd/MMM/yyyy")
                mail.AddParameter("CUSTOMERNAME") = rdm.Customer.Name & rdm.Customer.LastName 'rsv.customerName & rsv.customerLastName
                mail.AddParameter("REGDATE") = rdm.ReservationDate.ToString("dd/MMM/yyyy") 'rsv.reservationDate.ToString("dd/MMM/yyyy")
                mail.AddParameter("CANCELLEDDATE") = Now.Date.ToString("dd/MMM/yyyy")
                mail.AddParameter("UVNRPOLICIES") = ""
                mail.AddParameter("DetCuartos") = GetRooms(rdm.RoomDetails)
                mail.AddParameter("TIPODEPAGO") = paymentType
                mail.AddParameter("REFERENCIAINFO") = referenceInfo
                mail.AddParameter("REFERENCIA") = reference
                If rdm.PaymentDetails IsNot Nothing Then
                    If Not String.IsNullOrEmpty(rdm.PaymentDetails.Pasarela) Then
                        mail.AddParameter("PASARELA") = pasarela
                    End If
                End If
                mail.AddParameter("TOTAL") = total & " " + rdm.TotalDetails.Currency

                mail.Send()

                Return True
            Catch ex As Exception
                emailError = ex.Message
                emailError &= " "
                emailError &= ex.StackTrace
            End Try
            Return False
        End Function

        Public Function SendReactivationEmail(ByVal rdm As ReservationDetailsModel,
                                              ByVal toEmail As String, ByRef emailError As String) As Boolean

            'Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            'PortalCulture.SetCulture(Thread.CurrentThread.CurrentCulture.Name)
            Try
                Dim lang As String = "es-MX"
                Dim paymentType As String = String.Empty
                Dim total As Double = rdm.TotalDetails.Total
                Dim referenceInfo As String = String.Empty
                Dim reference As String = String.Empty
                Dim pasarela As String = String.Empty

                GetPaymentWayInfo(rdm, lang, paymentType, referenceInfo, reference, pasarela)


                Dim mail As New emailTemplates.Template
                mail.To = toEmail
                mail.TemplateName = "T18_HOTELRESERVATIONREACTIVATION"
                mail.Html = True
                mail.SubjectParam = rdm.ReservationId
                mail.Idioma = "es-MX" 'Thread.CurrentThread.CurrentCulture.Name

                'parametros
                mail.AddParameter("HOTELNAME") = rdm.HotelName
                mail.AddParameter("CITY") = rdm.City
                mail.AddParameter("RESERVATIONNUMBER") = rdm.ReservationId
                mail.AddParameter("STARTDATE") = rdm.CheckIn.Value.ToString("dd/MMM/yyyy")
                mail.AddParameter("ENDDATE") = rdm.CheckOut.Value.ToString("dd/MMM/yyyy")
                mail.AddParameter("CUSTOMERNAME") = rdm.Customer.Name & rdm.Customer.LastName
                mail.AddParameter("REGDATE") = rdm.ReservationDate.ToString("dd/MMM/yyyy")
                mail.AddParameter("UVNRPOLICIES") = ""
                mail.AddParameter("DetCuartos") = GetRooms(rdm.RoomDetails)
                mail.AddParameter("TIPODEPAGO") = paymentType
                mail.AddParameter("REFERENCIAINFO") = referenceInfo
                mail.AddParameter("REFERENCIA") = reference
                If rdm.PaymentDetails IsNot Nothing Then
                    If Not String.IsNullOrEmpty(rdm.PaymentDetails.Pasarela) Then
                        mail.AddParameter("PASARELA") = pasarela
                    End If
                End If
                mail.AddParameter("TOTAL") = total & " " + rdm.TotalDetails.Currency


                mail.Send()
                Return True
            Catch ex As Exception
                emailError = ex.Message
                emailError &= " "
                emailError &= ex.StackTrace
            End Try
            Return False
        End Function




        Public Function SendPortalMail(ByVal response As String) As Boolean
            Try
                Dim Smtp_Server As New Net.Mail.SmtpClient
                Dim e_mail As New Net.Mail.MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(username, password)
                Smtp_Server.Port = port
                Smtp_Server.EnableSsl = True
                Smtp_Server.Host = host
                e_mail = New Net.Mail.MailMessage()
                e_mail.From = New Net.Mail.MailAddress("not-reply@internetpowerhotel.com")
                e_mail.To.Add("soporte@internetpowerhotel.com")
                e_mail.Subject = "Portal Creado"
                e_mail.IsBodyHtml = False
                e_mail.Body = response
                Smtp_Server.Send(e_mail)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function


        Public Function SendNotificationEmail(ByVal template As String, ByVal emails As String, ByVal reservationNumber As String) As Boolean
            Try
                Dim Smtp_Server As New Net.Mail.SmtpClient
                Dim e_mail As New Net.Mail.MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(username, password)
                Smtp_Server.Port = port
                Smtp_Server.EnableSsl = True
                Smtp_Server.Host = host
                For Each email As String In emails.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
                    e_mail = New Net.Mail.MailMessage()
                    e_mail.From = New Net.Mail.MailAddress("not-reply@internetpowerhotel.com")
                    e_mail.To.Add(email)
                    e_mail.Subject = "Reservación #" & reservationNumber
                    e_mail.IsBodyHtml = True
                    e_mail.Body = template
                    Smtp_Server.Send(e_mail)
                Next
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function


        Function GetRooms(ByVal rooms As List(Of RoomDetails)) As String
            Dim html As New StringBuilder

            html.Append("<table class=""row"" style=""border-collapse:collapse;border-spacing:0;padding:0;text-align:left;vertical-align:top;width:100%"">")
            For Each room As RoomDetails In rooms
                html.Append("<tr style=""padding:0;text-align:left;vertical-align:top"">")
                '---
                html.Append("<th class=""small-12 large-6 columns first"" style=""Margin:0 auto;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0 auto;padding:0;padding-bottom:16px;padding-left:16px;padding-right:8px;text-align:left;width:274px"">")

                html.Append("<table style=""border-collapse:collapse;border-spacing:0;padding:0;text-align:left;vertical-align:top;width:100%"">")
                html.Append("<tr style=""padding:0;text-align:left;vertical-align:top"">")
                html.Append("<th style=""Margin:0;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;padding:0;text-align:left"">")
                html.Append("<p style=""Margin:0;Margin-bottom:10px;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;margin-bottom:10px;padding:0;text-align:left"">")
                html.Append("<b>" & PortalCulture.GetString("M000066") & ":</b><br>" & room.Name)
                html.Append("</p>")
                html.Append("</th>")
                html.Append("</tr>")
                html.Append("</table>")

                html.Append("</th>")

                html.Append("<th class=""small-12 large-6 columns last"" style=""Margin:0 auto;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0 auto;padding:0;padding-bottom:16px;padding-left:8px;padding-right:16px;text-align:left;width:274px"">")

                html.Append("<table style=""border-collapse:collapse;border-spacing:0;padding:0;text-align:left;vertical-align:top;width:100%"">")
                html.Append("<tr style=""padding:0;text-align:left;vertical-align:top"">")
                html.Append("<th style=""Margin:0;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;padding:0;text-align:left"">")
                'html.Append("<p style=""Margin:0;Margin-bottom:10px;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;margin-bottom:10px;padding:0;text-align:left"">")
                'html.Append("<b>" & PortalCulture.GetString("M000585") & ":</b><br>" & room.customerName & " " & room.customerLastName)
                'html.Append("</p>")
                If (Not String.IsNullOrEmpty(room.RatePlanPromotion) And Not String.IsNullOrEmpty(room.NamePromotion)) Then
                    html.Append("<p style=""Margin:0;Margin-bottom:10px;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;margin-bottom:10px;padding:0;text-align:left"">")
                    html.Append("<b>Promoción: </b></br>" & room.RatePlanPromotion & " - " & room.NamePromotion)
                    html.Append("</p>")
                End If
                html.Append("</th>")
                html.Append("</tr>")
                html.Append("</table>")

                html.Append("</th>")

                html.Append("</tr>")
            Next
            html.Append("</table>")
            Return html.ToString
        End Function

        Private Sub GetPaymentWayInfo(ByVal rdm As ReservationDetailsModel, ByVal lang As String, ByRef paymentType As String,
                                      ByRef referenceInfo As String, ByRef reference As String, ByRef pasarela As String)

            Select Case rdm.PaymentWay
                Case 0
                    paymentType = IIf(lang = "es-MX", "Depósito Bancario", "Bank Deposit")
                    referenceInfo = IIf(lang = "es-MX", "Referencia: ", "Reference: ")
                    reference = rdm.BankDepositDetails.Reference
                Case 1
                    paymentType = IIf(lang = "es-MX", "Pago en línea", "Online Payment")
                    referenceInfo = IIf(lang = "es-MX", "Número de autorización: ", "Authorization Number: ")

                    If rdm.PaymentDetails IsNot Nothing Then
                        If Not String.IsNullOrEmpty(rdm.PaymentDetails.AuthorizationNumber) Then
                            reference = rdm.PaymentDetails.AuthorizationNumber
                        End If
                        If Not String.IsNullOrEmpty(rdm.PaymentDetails.Pasarela) Then
                            pasarela = "<b>Pasarela: </b>" & rdm.PaymentDetails.Pasarela
                        End If
                    End If
                Case 2
                    paymentType = IIf(lang = "es-MX", "Pago en hotel", "Payment at the Hotel")
                    referenceInfo = IIf(lang = "es-MX", "Tarjeta de crédito <br>", "Credit Card <br>")
                    referenceInfo = IIf(lang = "es-MX", "Número de tarjeta: ", "Card Number: ")
                    reference = rdm.Customer.CardDetails.Number
            End Select

        End Sub

        Private Sub GetTotalInfo(ByVal typeClient As TypeClient, ByRef totalInfo As String, ByVal reservation As ReservationDetailsModel,
                                 ByVal oldReservation As ReservationDetailsModel)
            Select Case typeClient
                Case TypeClient.Customer
                    totalInfo = "<b>Total Actualizado: </b> " & reservation.TotalDetails.Total.ToString("C") & " " & reservation.TotalDetails.Currency
                    totalInfo &= "<br>"
                    totalInfo &= "<b>Total Anterior: </b> " & oldReservation.TotalDetails.Total.ToString("C") & " " & oldReservation.TotalDetails.Currency
                Case TypeClient.Hotel
                    totalInfo = "<b>Total Actualizado: </b> " & reservation.TotalDetails.TotalNR.ToString("C") & " " & reservation.TotalDetails.Currency
                    totalInfo &= "<br>"
                    totalInfo &= "<b>Total Anterior: </b> " & oldReservation.TotalDetails.TotalNR.ToString("C") & " " & oldReservation.TotalDetails.Currency
            End Select
        End Sub

        Private Sub GetPriceRoom(ByVal typeClient As TypeClient, ByRef rooms As String, ByVal lang As String, ByVal roomDetail As RoomDetails)
            Select Case typeClient
                Case TypeClient.Customer
                    rooms += "<strong>" + IIf(lang = "es-MX", "Precio por noche: ", "Price Per Night: ") + "</strong>"
                    rooms += roomDetail.PriceDetails.ElementAt(0).Price.ToString("C") & " " & roomDetail.PriceDetails.ElementAt(0).Currency
                    rooms += "<br>"
                    If (roomDetail.PriceDetails.ElementAt(0).ExtraPrice <> 0) Then

                        rooms += "<strong>" + IIf(lang = "es-MX", "Precio extra: ", "Extra Price: ") + "</strong>"
                        Dim extraPrice As String = roomDetail.PriceDetails.ElementAt(0).ExtraPrice.ToString("C") + " " + roomDetail.PriceDetails.ElementAt(0).Currency
                        rooms += extraPrice
                        rooms += "<br>"
                    End If
                    rooms += "<strong>Total: </strong>"
                    rooms += roomDetail.Total.ToString("C") + " " + roomDetail.Currency
                Case TypeClient.Hotel
                    rooms += "<strong>" + IIf(lang = "es-MX", "Precio por noche: ", "Price Per Night: ") + "</strong>"
                    rooms += roomDetail.PriceDetails.ElementAt(0).PriceNR.ToString("C") & " " & roomDetail.PriceDetails.ElementAt(0).Currency
                    rooms += "<br>"
                    If (roomDetail.PriceDetails.ElementAt(0).ExtraPriceNR <> 0) Then

                        rooms += "<strong>" + IIf(lang = "es-MX", "Precio extra: ", "Extra Price: ") + "</strong>"
                        Dim extraPrice As String = roomDetail.PriceDetails.ElementAt(0).ExtraPriceNR.ToString("C") + " " + roomDetail.PriceDetails.ElementAt(0).Currency
                        rooms += extraPrice
                        rooms += "<br>"
                    End If
                    rooms += "<strong>Total: </strong>"
                    rooms += roomDetail.TotalNR.ToString("C") + " " + roomDetail.Currency
            End Select

        End Sub

    End Module
End Namespace
