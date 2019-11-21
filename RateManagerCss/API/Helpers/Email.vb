Imports System.Globalization
Imports System.Threading
Imports APIServices.Models
Imports RateManager.API.Models
Imports System.Configuration

Namespace API.Helpers
    Public Module Email
        Private username As String = ConfigurationManager.AppSettings("usernameNotification")
        Private password As String = ConfigurationManager.AppSettings("passwordNotification")
        Private host As String = ConfigurationManager.AppSettings("hostNotification")
        Private port As Integer = CInt(ConfigurationManager.AppSettings("portNotification"))

        Sub New()
            Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            PortalCulture.SetCulture(Thread.CurrentThread.CurrentCulture.Name)
        End Sub

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

        Public Function SendCancellationEmail(ByVal rsv As vReservationDetails, ByVal roomRsv As List(Of vReservationRoomDetails), ByVal toEmail As String) As Boolean

            Dim mail As New emailTemplates.Template
            mail.To = toEmail
            mail.TemplateName = "T12_HOTELCANCELLATION"
            mail.Html = True
            mail.SubjectParam = "cancelación"
            mail.Idioma = Thread.CurrentThread.CurrentCulture.Name

            'parametros
            mail.AddParameter("HOTELNAME") = rsv.hotelName
            mail.AddParameter("MOTIVO") = rsv.cancellationReason
            mail.AddParameter("CITY") = rsv.city
            mail.AddParameter("RESERVATIONNUMBER") = rsv.reservationId
            mail.AddParameter("CANCELLATIONNUMBER") = rsv.cancellationNumber
            mail.AddParameter("STARTDATE") = rsv.checkIn.ToString("dd/MMM/yyyy")
            mail.AddParameter("ENDDATE") = rsv.checkOut.ToString("dd/MMM/yyyy")
            mail.AddParameter("CUSTOMERNAME") = rsv.customerName & rsv.customerLastName
            mail.AddParameter("REGDATE") = rsv.reservationDate.ToString("dd/MMM/yyyy")
            mail.AddParameter("CANCELLEDDATE") = Now.Date.ToString("dd/MMM/yyyy")
            mail.AddParameter("UVNRPOLICIES") = ""
            mail.AddParameter("DetCuartos") = GetRooms(roomRsv)

            Try
                mail.Send()
                Return True
            Catch ex As Exception

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


        Function GetRooms(ByVal rooms As List(Of vReservationRoomDetails)) As String
            Dim html As New StringBuilder

            html.Append("<table class=""row"" style=""border-collapse:collapse;border-spacing:0;padding:0;text-align:left;vertical-align:top;width:100%"">")
            For Each room As vReservationRoomDetails In rooms
                html.Append("<tr style=""padding:0;text-align:left;vertical-align:top"">")
                '---
                html.Append("<th class=""small-12 large-6 columns first"" style=""Margin:0 auto;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0 auto;padding:0;padding-bottom:16px;padding-left:16px;padding-right:8px;text-align:left;width:274px"">")

                html.Append("<table style=""border-collapse:collapse;border-spacing:0;padding:0;text-align:left;vertical-align:top;width:100%"">")
                html.Append("<tr style=""padding:0;text-align:left;vertical-align:top"">")
                html.Append("<th style=""Margin:0;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;padding:0;text-align:left"">")
                html.Append("<p style=""Margin:0;Margin-bottom:10px;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;margin-bottom:10px;padding:0;text-align:left"">")
                html.Append("<b>" & PortalCulture.GetString("M000066") & ":</b><br>" & room.roomName)
                html.Append("</p>")
                html.Append("</th>")
                html.Append("</tr>")
                html.Append("</table>")

                html.Append("</th>")

                html.Append("<th class=""small-12 large-6 columns last"" style=""Margin:0 auto;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0 auto;padding:0;padding-bottom:16px;padding-left:8px;padding-right:16px;text-align:left;width:274px"">")

                html.Append("<table style=""border-collapse:collapse;border-spacing:0;padding:0;text-align:left;vertical-align:top;width:100%"">")
                html.Append("<tr style=""padding:0;text-align:left;vertical-align:top"">")
                html.Append("<th style=""Margin:0;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;padding:0;text-align:left"">")
                html.Append("<p style=""Margin:0;Margin-bottom:10px;color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;margin-bottom:10px;padding:0;text-align:left"">")
                html.Append("<b>" & PortalCulture.GetString("M000585") & ":</b><br>" & room.customerName & " " & room.customerLastName)
                html.Append("</p>")
                html.Append("</th>")
                html.Append("</tr>")
                html.Append("</table>")

                html.Append("</th>")

                html.Append("</tr>")
            Next
            html.Append("</table>")
            Return html.ToString
        End Function

    End Module
End Namespace
