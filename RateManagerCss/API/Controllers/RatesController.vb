Imports System.Net
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Validation
Imports RateManager.API.Helpers
Imports RateManager.API.Models

Namespace API.Controllers
    <RoutePrefix("api/hotels/{HotelId:Int}/rates")>
    Public Class RatesController
        Inherits ShurikenController

        Public Service As New RatesService

        ' GET api/hotels/1/rates
        <Route(""), HttpGet>
        Public Function GetById(HotelId As Integer, <FromUri> Req As RatesByPlanRQ) As IEnumerable(Of DTO.RatesByRatePlan)
            Return Service.FindGroupedByRatePlan(HotelId, Req.StartDate, Req.EndDate, Req.RoomId, Request.GetLanguageUV())
        End Function

    End Class
End Namespace