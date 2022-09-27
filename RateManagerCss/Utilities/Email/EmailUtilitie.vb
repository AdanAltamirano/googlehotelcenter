Imports System.Xml
Imports WSHotelCommon

Namespace Utitlities.Email
    Module EmailUtilitie
        Sub EmailDeposit(ByVal reservationNumber As String)

            Try
                Dim xdoc As New XmlDataDocument(New reqHotelDisplay)
                Dim dsreq As reqHotelDisplay = CType(xdoc.DataSet, reqHotelDisplay)
                Dim xml As resHotelDisplay


                Dim drR As reqHotelDisplay.HotelDisplayRow
                drR = dsreq.HotelDisplay.NewHotelDisplayRow
                drR.ConfirmNumber = reservationNumber
                drR.Language = "en-US"
                dsreq.HotelDisplay.AddHotelDisplayRow(drR)

                With New WSHotelFacade.clsFADisplay
                    xml = .GetHotelDisplay(xdoc.DocumentElement)
                End With

                If Not xml Is Nothing AndAlso xml.Reservation.Rows.Count > 0 Then
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

                End If
            Catch ex As Exception

            End Try

        End Sub

    End Module
End Namespace
