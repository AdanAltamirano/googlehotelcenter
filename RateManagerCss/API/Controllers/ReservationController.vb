Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports RateManager.API.Helpers

Namespace API.Controller
    <RoutePrefix("api/reservations"), AuthorizeUser(Roles:="supervisor")>
    Public Class ReservationController
        Inherits ApiController

    Public ReservationService As New ReservationService



    <Route(""), HttpGet>
    Public Function GetAll() As IQueryable(Of vReservation)
        Return ReservationService.GetAll()
    End Function

    <Route("{hotelId:int}"), HttpGet>
    Public Function GetById(ByVal hotelId As Integer) As IQueryable(Of vReservation)
        Return ReservationService.Get(hotelId)
    End Function
    End Class
End Namespace

