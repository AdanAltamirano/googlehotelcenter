Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.API.Models

<RoutePrefix("api/hotels/{HotelId:int}/rooms")>
Public Class RoomsController
    Inherits ShurikenController

    Public Service As New RoomsService

    ' GET api/hotels/3164/rooms
    <Route(""), HttpGet, Queryable(MappingDelegate:="MapRooms")>
    Public Function GetAll(HotelId As Integer) As IQueryable(Of vHotelRoom)
        Dim language As Integer = Request.GetLanguageUV()
        Return Service.GetAll().Where(Function(x) x.HotelId = HotelId AndAlso x.Language = language)
    End Function


    ' GET api/hotels/3164/rooms/2347/inventory
    <Route("{RoomId:int}/inventory"), HttpGet>
    Public Function GetInventory(HotelId As Integer, RoomId As Integer, <FromUri> Req As DateRangeRQ) As IEnumerable(Of DTO.RoomInventoryInfo)
        Dim language As Integer = Request.GetLanguageUV()
        Return Service.FindInventoryByRoomId(HotelId, RoomId, Req.StartDate, Req.EndDate)
    End Function

#Region "MAPPING"

    Public Function MapRooms(query As IQueryable) As IQueryable
        Return query.Cast(Of vHotelRoom).Select(Function(r) New DTO.Room With {
            .Id = r.Id,
            .Name = r.Name,
            .Code = r.Code,
            .Active = r.Active,
            .ExtraOccupancyAllowed = r.ExtraOccupancyAllowed,
            .MinAdultsOccupancy = r.MinAdultsOccupancy,
            .MaxAdultsOccupancy = r.MaxAdultsOccupancy,
            .MaxChildrenOccupancy = r.MaxChildrenOccupancy,
            .JuniorsAllowed = r.JuniorsAllowed,
            .MaxOccupancy = r.MaxOccupancy,
            .Order = r.Order,
            .TotalRooms = r.TotalRooms,
            .Type = r.Type,
            .IsLinked = r.IsLinked,
            .ParentRoomId = r.ParentRoomId,
            .ParentRoomCode = r.ParenteRoomCode,
            .Factor = r.Factor,
            .Offset = r.Offset
        })
    End Function

#End Region

End Class
