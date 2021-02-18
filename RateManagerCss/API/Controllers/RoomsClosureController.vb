Imports System.Web.Http
Imports System.Net.Http
Imports NinjAPI
Imports APIServices
Imports APIServices.Models
Imports RateManager.API.Models
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports RateManager.PaginaBase
Imports Portal.General.Common.Data

Namespace API.Controller
    <RoutePrefix("api/closure")>
    Public Class RoomsClosureController
        Inherits ShurikenController

        Private service As New RoomsClosureService
        'Private paginaBase As New PaginaBase


        'Get api/closure/1978/2020-12-28/2020-12-29/RAC
        <Route("{idHotel:Int}/{startDate:datetime}/{endDate:datetime}/{ratePlan?}"), HttpGet>
        Public Function GetRoomsClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date,
                                                 ByVal endDate As Date, Optional ratePlan As String = "") As DTO.RoomsClosureModel

            Dim page As New PaginaBase
            Dim idAsoc As Integer = page.GetIdAsociation()
            'Params idhote,startdate,endate,idasoc,rateplan,lang
            Return service.LoadData(idHotel, startDate, endDate, idAsoc, ratePlan, PortalCulture.GetIDCulture)
        End Function

        'Post api/closure/save/1978/2020-12-28/2020-12-29
        <Route("save/{idHotel:Int}/{startDate:datetime}/{endDate:datetime}"), HttpPost>
        Public Function SaveClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date, ByVal endDate As Date,
                                             <FromBody> roomClosureRQ As RoomClosureRQ) As HttpResponseMessage

            Dim allRatePlans As Boolean = (roomClosureRQ.RatePlanOption = "0")
            Dim allRooms As Boolean = (roomClosureRQ.RoomOption = "0")
            Dim dsrateplans As RatePlanData, dsrooms As RoomsHotelData

            Dim nota As String = String.Empty

            Dim hotelName As String

            Dim page As New PaginaBase
            hotelName = page.HotelName

            nota &= "Cierre en el hotel " & hotelName
            If allRooms Then
                nota &= " en todas las habitaciones,"
            Else
                nota &= " en la habitación " & roomClosureRQ.RoomOption
            End If
            If allRatePlans Then
                nota &= " en todos los Rate Plans"
            Else
                nota &= " en el Rate Plan " & roomClosureRQ.RatePlanOption
            End If

            nota &= " del " & startDate.ToString("yyyy/MM/dd") & " al " & endDate.ToString("yyyy/MM/dd") & " "

            nota &= " Status: " & GetStatus(roomClosureRQ.Status) & ". "


            Try
                If allRatePlans Then
                    With New RatePlanFacade
                        dsrateplans = .GetRatePlanByIdHotel(idHotel.ToString(), PortalCulture.GetIDCulture, 0, 1, idAsociacion:=page.GetIdAsociation, DeleteFilter:=1)
                    End With
                    If allRooms Then
                        With New RoomFacade
                            dsrooms = .getRooms(idHotel)
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
                            dsrooms = .getRooms(idHotel)
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

                page.guardalog("rate-manager-ui/dist/rooms-closure.aspx", PaginaBase.acciones.Crear, nota, roomClosureRQ.IdHotel)

            Catch ex As Exception
                Dim errorMessage As String = "Error: " & ex.Message
                page.guardalog("rate-manager-ui/dist/rooms-closure.aspx", PaginaBase.acciones.Crear, errorMessage, roomClosureRQ.IdHotel)
                Return New HttpResponseMessage(Net.HttpStatusCode.InternalServerError)
            End Try

            Return New HttpResponseMessage(Net.HttpStatusCode.OK)

        End Function

        <Route("rateplans/{idHotel:Int}"), HttpGet>
        Public Function GetRatePlansByHotelId(ByVal idHotel As Integer) As List(Of DTO.RatePlansClosureModel)

            Dim page As New PaginaBase

            Dim idAsoc As Integer = page.GetIdAsociation()

            Return service.LoadRatePlanByIdHotel(idHotel, idAsoc, page.IdCorporativoUserChain, page.IsHotel,
                                          page.IsUsuarioHotel, PortalCulture.GetIDCulture)
        End Function

        <Route("rooms/{idHotel:Int}"), HttpGet>
        Public Function GetRoomsByHotelId(ByVal idHotel As Integer) As List(Of DTO.RoomsModel)


            Return service.LoadRoomsByIdHotel(idHotel, PortalCulture.GetIDCulture)
        End Function

        Private Function GetStatus(ByVal status As String) As String
            Select Case status
                Case "O"
                    Return "Abierto"
                Case "C"
                    Return "Cerrado"
                Case "N"
                    Return "No Llegadas"
                Case Else
                    Return ""
            End Select
        End Function
    End Class
End Namespace
