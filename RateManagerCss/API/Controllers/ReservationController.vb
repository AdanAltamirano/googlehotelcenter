Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers

Namespace API.Controller
    <RoutePrefix("api/reservations"), AuthorizeUser(Roles:="supervisor")>
    Public Class ReservationController
        Inherits ShurikenController

        Public ReservationService As New ReservationService



        <Route(""), HttpGet, Queryable>
        Public Function GetAll() As IQueryable(Of vReservation)
            Return ReservationService.GetAll()
        End Function


        <Route("{hotelId:int}"), HttpGet, Queryable>
        Public Function GetByHotelId(ByVal hotelId As Integer) As IQueryable(Of vReservation)
            Return ReservationService.Get(hotelId)
        End Function

    End Class
End Namespace

