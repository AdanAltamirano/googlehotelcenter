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


        <Route("details"), HttpGet>
        Public Function GetDetailsById(ByVal reservationId As Integer) As DTO.ReservationDetailsModel
            Return ReservationService.GetDetailsById(reservationId)
        End Function

    End Class
End Namespace

