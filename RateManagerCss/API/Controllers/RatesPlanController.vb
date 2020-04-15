Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.API.Models

<RoutePrefix("api/hotels/{HotelId:int}/ratesplan")>
Public Class RatesPlanController
    Inherits ShurikenController

    Public Service As New RatesPlanService

    ' GET api/hotels/3164/ratesplan
    <Route(""), HttpGet>
    Public Function GetAll(HotelId As Integer) As IEnumerable(Of DTO.RatePlan)
        Dim language As Integer = Request.GetLanguageUV()
        Return Service.FindByHotel(HotelId).ToList()
    End Function

End Class
