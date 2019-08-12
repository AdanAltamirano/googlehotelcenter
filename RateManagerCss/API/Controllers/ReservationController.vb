Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers

Namespace API.Controller
    <RoutePrefix("api/reservations"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany")>
    Public Class ReservationController
        Inherits ShurikenController

        Public ReservationService As New ReservationService


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


        <Route("{reservationId:Int}"), HttpGet>
        Public Function GetDetails(ByVal reservationId As Integer) As DTO.ReservationDetailsModel
            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")

            Return ReservationService.GetDetails(reservationId, isSupervisor, GetUserId().Value)
        End Function


        <Route("{reservationId:int}/cancel"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.CancelBookingRQ) As DTO.CancelBookingRS

            Return ReservationService.Cancel(reservationId, GetUserId().Value, req.Reason)
        End Function

        <Route("{reservationId:int}/modify"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.ModifyBookingRQ) As DTO.ModifyBookingRS

            Return ReservationService.Modify(reservationId, req)
        End Function

        <Route("{reservationId:int}/creditcard"), HttpGet>
        Public Function GetCode(ByVal reservationId As Integer)
            Dim code As String = ReservationService.GetCode(10)
            HttpContext.Current.Session("code_cc") = code
            Return Ok(New With {Key .success = ReservationService.SendCodeToEmail(reservationId, code)})
        End Function

        <Route("{reservationId:int}/creditcard/{code}"), HttpGet>
        Public Function GetCreditCard(ByVal reservationId As Integer, ByVal code As String) As DTO.CardDetails
            Dim generatedCode As String = ""
            If HttpContext.Current.Session("code_cc") IsNot Nothing Then
                generatedCode = HttpContext.Current.Session("code_cc")
            End If
            If (code = generatedCode) Then
                HttpContext.Current.Session("code_cc") = Nothing
                Return ReservationService.GetCreditCardDetails(reservationId)
            End If
            Return New DTO.CardDetails()
        End Function
    End Class
End Namespace

