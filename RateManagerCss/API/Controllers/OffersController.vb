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
        Public service As New OfferService

        <Route(""), HttpGet>
        Public Function GetByHotelId(HotelId As Integer) As IEnumerable(Of DTO.Offer)
            Return service.FindOffers(HotelId)
        End Function

        <Route(""), HttpGet>
        Public Function GetByCode(HotelId As Integer, <FromUri> code As String) As IEnumerable(Of DTO.Offer)
            Return service.FindOffers(HotelId, code)
        End Function

        'POST api/hotels/1/offers
        <Route(""), HttpPost>
        Public Function OfferAdd(<FromBody> RQ As DTO.Offer, HotelId As Integer) As Net.Http.HttpResponseMessage
            RQ.HotelId = HotelId

            Dim result As KeyValuePair(Of String, String) = service.Add(RQ)

            If result.Key = 1 Then
                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/hotels/1/offers/update
        <Route("update"), HttpPost>
        Public Function OfferUpdate(<FromBody> RQ As DTO.Offer, HotelId As Integer) As Net.Http.HttpResponseMessage
            RQ.HotelId = HotelId

            Dim result As KeyValuePair(Of String, String) = service.Update(RQ)

            If result.Key = 1 Then
                Return NoContent()
            End If
            Return BadRequest(result)
        End Function
    End Class
End Namespace