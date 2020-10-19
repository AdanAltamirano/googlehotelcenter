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

        'GET api/hotels/1978/offers
        <Route(""), HttpGet>
        Public Function GetByHotelId(HotelId As Integer) As IEnumerable(Of DTO.OfferPromotions)
            'Param HotelId,
            'Param IncludeOldPromos
            'Param Active -> Active = 1,InActive = 0, ActiveAndInActive = -1
            'Param SearcyBy -> Name = 2, Code = 3
            'Param SearchValue
            Return service.FindOffers(HotelId, False, 0, 2, "")
        End Function

        ' GET api/hotels/1978/offers/PR01
        <Route("{code}"), HttpGet>
        Public Function GetByCode(HotelId As Integer, code As String) As DTO.Offer
            Return service.FindOfferByHotelAndCode(HotelId, code)
        End Function

        'TODO: Cambiar la ruta de la api en comentarios en las diferentes acciones 

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