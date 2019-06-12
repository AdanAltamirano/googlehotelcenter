Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports RateManager.API.Helpers
Imports RateManager.API.Models

Namespace API.Controllers
    <RoutePrefix("api/hotels/{HotelId:int}/rates")>
    Public Class RatesController
        Inherits ShurikenController

        Public Service As New RatesService

        'GET api/hotels/1/rates
        <Route(""), HttpGet>
        Public Function GetByRatePlan(HotelId As Integer, <FromUri> Req As RatesByPlanRQ) As IEnumerable(Of DTO.RatesByRatePlan)
            Return Service.FindGroupedByRatePlan(HotelId, Req.StartDate, Req.EndDate, Req.RoomId, Request.GetLanguageUV())
        End Function

        'GET api/hotels/1/rates/2345/daily/2019-09-01
        <Route("{RateId:int}/daily/{day:datetime}"), HttpGet>
        Public Function GetDayRate(HotelId As Integer, RateId As Integer, day As Date) As DTO.DailyRateDetail
            Return Service.FindDayRateDetail(RateId, day)
        End Function

    End Class
End Namespace