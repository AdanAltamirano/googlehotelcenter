Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.UI.WebControls
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.PaginaBase

Namespace API.Controller
    <RoutePrefix("api/reservations"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany")>
    Public Class ReservationController
        Inherits ShurikenController

        Public ReservationService As New ReservationService

        'GET api/reservations
        <Route(""), HttpGet, Queryable>
        Public Function GetAll() As IQueryable(Of vReservation)

            Dim roles() As String = GetRoles()
            If roles.Contains("supervisor") Then
                Return ReservationService.GetAll()

            ElseIf roles.Contains("userchain") Then
                Dim userCorpId = GetUserCorpId(GetUserId().Value)
                Return ReservationService.GetAll().Where(Function(h) h.CompanyId = userCorpId)

            ElseIf roles.Contains("hotelcompany") Then
                Dim hotels() As Integer = GetUserHotels(GetUserId().Value).Select(Function(h) h.HotelId).ToArray()
                Return ReservationService.GetAll().Where(Function(h) hotels.Contains(h.HotelId))
            End If

            Return New vReservation() {}.AsQueryable()
        End Function

        'GET api/reservations/excel
        '<Route("excel"), HttpGet, Queryable>
        Public Function GetExcel() As HttpResponseMessage
            Dim gv As New GridView()
            gv.DataSource = ReservationService.GetExcel()
            gv.DataBind()

            Dim response As New HttpResponseMessage(Net.HttpStatusCode.OK)

            response.Content.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/ms-excel")

            Return response
        End Function

        'GET api/reservations/1978
        <Route("{reservationId:Int}"), HttpGet>
        Public Function GetDetails(ByVal reservationId As Integer) As DTO.ReservationDetailsModel
            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

            Return ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, GetUserId().Value)
        End Function

        'POST api/reservations/1978/cancel
        <Route("{reservationId:int}/cancel"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.CancelBookingRQ) As DTO.CancelBookingRS

            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim rsv As vReservationDetails = ReservationService.GetReservation(reservationId)

            Dim result As DTO.CancelBookingRS = ReservationService.Cancel(rsv, GetUserId().Value, req.Reason)
            If result.IsSuccess Then
                Log(reservationId, acciones.Eliminar)

                'no enviar correo de cancelación si está en proceso
                If rsv.status <> 4 Then
                    rsv = ReservationService.GetReservation(reservationId)
                    Dim roomRsv As List(Of vReservationRoomDetails) = ReservationService.GetRoomsReservation(reservationId)

                    If Not String.IsNullOrEmpty(rsv.customerEmail) Then
                        'enviar correo al cliente
                        SendCancellationEmail(rsv, roomRsv, rsv.customerEmail)
                    End If
                    If Not String.IsNullOrEmpty(rsv.hotelEmail) Then
                        'enviar correo al hotel
                        SendCancellationEmail(rsv, roomRsv, rsv.hotelEmail)
                    End If
                    If String.IsNullOrEmpty(rsv.customerEmail) AndAlso String.IsNullOrEmpty(rsv.hotelEmail) Then
                        'enviar correo a algun admin
                        SendCancellationEmail(rsv, roomRsv, "soporte@internetpowerhotel.com")
                    End If
                End If
            End If
            Return result
        End Function

        'POST api/reservations/1978/modify
        <Route("{reservationId:int}/modify"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.ModifyBookingRQ) As DTO.ModifyBookingRS

            Dim result As DTO.ModifyBookingRS = ReservationService.Modify(reservationId, req)
            If result.IsSuccess Then
                Log(reservationId, acciones.Modificar)
            End If
            Return result
        End Function

        'GET api/reservations/1978/creditcard
        <Route("{reservationId:int}/creditcard"), HttpGet>
        Public Function GetCode(ByVal reservationId As Integer)
            Dim code As String = ReservationService.GetCode(10)
            HttpContext.Current.Session("code_cc") = code

            Return Ok(New With {Key .success = SendVerificationCodeEmail(code)})
        End Function

        'GET api/reservations/1978/creditcard/1234
        <Route("{reservationId:int}/creditcard/{code}"), HttpGet>
        Public Function GetCreditCard(ByVal reservationId As Integer, ByVal code As String) As DTO.CardDetails
            Dim generatedCode As String = ""
            If HttpContext.Current.Session("code_cc") IsNot Nothing Then
                generatedCode = HttpContext.Current.Session("code_cc")
            End If
            If (code = generatedCode) Then
                HttpContext.Current.Session("code_cc") = Nothing
                Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

                Return ReservationService.GetCreditCardDetails(reservationId, isHotelCompany, GetUserId().Value)
            End If
            Return New DTO.CardDetails()
        End Function




        Sub Log(ByVal reservationId As Integer, ByVal action As acciones)
            Dim pb As New PaginaBase
            Dim msg As String = ""
            Select Case action
                Case acciones.Eliminar
                    msg = "Canceló la reserva#" & reservationId
                Case acciones.Modificar
                    msg = "Modifico la reserva#" & reservationId
            End Select
            pb.guardalog("/rate-manager-ui/dist/reservation-details.aspx?qs=" & reservationId, action, msg)
        End Sub
    End Class
End Namespace

