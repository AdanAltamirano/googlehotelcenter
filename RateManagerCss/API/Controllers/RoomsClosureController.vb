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


        'Get api/closure/1978/2020-12-28/2020-12-29/RAC
        <Route("{idHotel:Int}/{startDate:datetime}/{endDate:datetime}/{ratePlan?}"), HttpGet>
        Public Function GetRoomsClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date,
                                                 ByVal endDate As Date, Optional ratePlan As String = "") As DTO.RoomsClosureModel
            Dim idAsoc As Integer = paginaBase.GetIdAsociation
            'Params idhote,startdate,endate,idasoc,rateplan,lang
            Return service.LoadData(idHotel, startDate, endDate, idAsoc, ratePlan, PortalCulture.GetIDCulture)
        End Function

        <Route("save/{idHotel:Int}/{startDate:datetime}/{endDate:datetime}"), HttpGet>
        Public Function SaveClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date, ByVal endDate As Date) As String


            service.SaveData(idHotel, startDate, endDate)

            Return "Succesfull"

        End Function


    End Class
End Namespace
