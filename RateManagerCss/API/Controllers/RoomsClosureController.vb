Imports System.Web.Http
Imports NinjAPI
Imports APIServices
Imports APIServices.Models

Namespace API.Controllers
    <RoutePrefix("api/closure")>
    Public Class RoomsClosureController
        Inherits ShurikenController

        Private service As New RoomsClosureService
        Private paginaBase As New PaginaBase

        'Get api/closure/1978
        <Route("{idHotel:Int}"), HttpGet>
        Public Function GetRoomsClosureByHotelId(idHotel As Integer) As DTO.RoomsClosureModel
            Dim startDate As Date = Now
            Dim endDate As Date = Now.AddDays(1)
            Dim idAsoc As Integer = paginaBase.GetIdAsociation
            'Params idhote,startdate,endate,idasoc,rateplan,lang
            Return service.LoadData(idHotel, startDate, endDate, idAsoc, "", PortalCulture.GetIDCulture)
        End Function

    End Class
End Namespace
