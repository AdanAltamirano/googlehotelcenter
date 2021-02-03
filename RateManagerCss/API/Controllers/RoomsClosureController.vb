Imports System.Web.Http
Imports NinjAPI
Imports APIServices
Imports APIServices.Models
Imports RateManager.API.Models
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data

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

        'Post api/closure/1978/2020-12-28/2020-12-29
        <Route("save/{idHotel:Int}/{startDate:datetime}/{endDate:datetime}"), HttpPost>
        Public Function SaveClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date, ByVal endDate As Date,
                                             <FromBody> roomClosureRQ As RoomClosureRQ) As String

            Dim allRatePlans As Boolean = (roomClosureRQ.RatePlanOption = "0")
            Dim allRooms As Boolean = (roomClosureRQ.RoomOption = "0")
            Dim dsrateplans As RatePlanData, dsrooms As RoomsHotelData

            If allRatePlans Then
                With New RatePlanFacade
                    dsrateplans = .GetRatePlanByIdHotel(paginaBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, 1, idAsociacion:=paginaBase.GetIdAsociation, DeleteFilter:=1)
                End With
                If allRooms Then
                    With New RoomFacade
                        dsrooms = .getRooms(paginaBase.cInfoActual.Hotel)
                    End With
                    'Se guarda por todos los rateplans y todas las habitaciones
                    For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                        For Each drroom As DataRow In dsrooms.Tables(0).Rows
                            service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
                                             drrateplan(dsrateplans.FIELD_CODIGOTARIFA), drroom(dsrooms.FLD_ID_ROOM_HOTEL),
                                             roomClosureRQ.Status)
                        Next
                    Next
                Else
                    'Se guarda por todos los rateplans y la habitacion que se eligio
                    For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                        service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
                                            drrateplan(dsrateplans.FIELD_CODIGOTARIFA), roomClosureRQ.RoomOption,
                                            roomClosureRQ.Status)
                    Next
                End If
            Else
                If allRooms Then
                    With New RoomFacade
                        dsrooms = .getRooms(paginaBase.cInfoActual.Hotel)
                    End With
                    'Se guarda por el rateplan que se eligio y todas las habitaciones
                    For Each drroom As DataRow In dsrooms.Tables(0).Rows
                        service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
                                           roomClosureRQ.RatePlanOption, drroom(dsrooms.FLD_ID_ROOM_HOTEL),
                                           roomClosureRQ.Status)
                    Next
                Else
                    'Se guarda por el rateplan que se eligio y la habitacion que se eligio
                    service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
                                           roomClosureRQ.RatePlanOption, roomClosureRQ.RoomOption,
                                           roomClosureRQ.Status)
                End If
            End If

            Return "Succesfull"

        End Function


    End Class
End Namespace
