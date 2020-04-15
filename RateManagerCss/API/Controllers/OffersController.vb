Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Common
Imports RateManager.API.Helpers
Imports RateManager.API.Models

Namespace API.Controllers

    <RoutePrefix("api/hotels/{HotelId:int}/offers")>
    Public Class OffersController
        Inherits ShurikenController
        Public service As New PromotionsService

        <Route(""), HttpGet>
        Public Function GetByHotelId(HotelId As Integer) As IEnumerable(Of DTO.Offer)
            Return service.FindOffers(HotelId)
        End Function
    End Class
End Namespace