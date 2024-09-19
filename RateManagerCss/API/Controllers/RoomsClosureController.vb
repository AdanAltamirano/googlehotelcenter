Imports System.Linq
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
Imports System.Xml.Linq
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Xml.Soap
Imports APIServices.Conflux.Helpers.Restriction
Imports APIServices.Conflux.Parser.Restriction
Imports APIServices.Xml.OTA.Request.Restrictions
Imports APIServices.Conflux.Models.Restrictions.Response
Imports RateManager.Utitlities.Hotel

Namespace API.Controller
    <RoutePrefix("api/closure")>
    Public Class RoomsClosureController
        Inherits ShurikenController

        Private service As New RoomsClosureService
        'Private paginaBase As New PaginaBase


        'Get api/closure/1978/2020-12-28/2020-12-29/RAC
        <Route("{idHotel:Int}/{startDate:datetime}/{endDate:datetime}/"), HttpGet>
        Public Function GetRoomsClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date,
                                                 ByVal endDate As Date, <FromUri> ratePlans As String()) As DTO.RoomsClosureModel

            Dim page As New PaginaBase
            Dim idAsoc As Integer = page.GetIdAsociation()
            'Params idhote,startdate,endate,idasoc,rateplan,lang
            Return service.LoadData(idHotel, startDate, endDate, idAsoc, ratePlans, PortalCulture.GetIDCulture)
        End Function


        'Post api/closure/save/1978
        <Route("save/{idHotel:Int}"), HttpPost>
        Public Function SaveClosureByHotelId(ByVal idHotel As Integer, <FromBody> roomClosureRQ As RoomClosureRQ) As HttpResponseMessage

            Dim pageBase As New PaginaBase

            Dim confluxService As New ConfluxService()
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
            RestrictionsParser.Init(info.Empresa)

            Dim dsrooms As RoomsHotelData

            With New RoomFacade
                dsrooms = .getRooms(idHotel)
            End With

            Dim rooms As IEnumerable(Of DataRow) = dsrooms.Tables(0).Rows.Cast(Of DataRow)

            For Each dateClosure As DatesClosure In roomClosureRQ.Dates

                For Each ratePlan As RatePlanHeader In roomClosureRQ.RatePlans


                    Dim allRatePlans As Boolean = (ratePlan.Code = "0")
                    Dim allRooms As Boolean = (roomClosureRQ.RoomOption = "0")
                    Dim dsrateplans As RatePlanData ', dsrooms As RoomsHotelData


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
                        nota &= " en el Rate Plan " & ratePlan.Code
                    End If

                    nota &= " del " & dateClosure.StartDate.ToString("yyyy/MM/dd") & " al " & dateClosure.EndDate.ToString("yyyy/MM/dd") & " "

                    nota &= " Status: " & GetStatus(roomClosureRQ.Status) & ". "

                    'Para cierres LockRoomType Google se utiliza spGetLockRatePlansByHotel_Result como modelo

                    Try
                        If allRatePlans Then
                            With New RatePlanFacade
                                dsrateplans = .GetRatePlanByIdHotel(idHotel.ToString(), PortalCulture.GetIDCulture, 0, 1, idAsociacion:=page.GetIdAsociation, DeleteFilter:=1)
                            End With
                            If allRooms Then
                                'With New RoomFacade
                                '    dsrooms = .getRooms(idHotel)
                                'End With
                                'Se guarda por todos los rateplans y todas las habitaciones
                                For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                                    For Each drroom As DataRow In dsrooms.Tables(0).Rows
                                        service.SaveData(roomClosureRQ.IdHotel, dateClosure.StartDate, dateClosure.EndDate,
                                                 drrateplan(dsrateplans.FIELD_CODIGOTARIFA), drroom(dsrooms.FLD_ID_ROOM_HOTEL),
                                                 roomClosureRQ.Status)


                                        Try

                                            If isEnabledGoogleRequest Then

                                                Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                                dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))

                                                Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, drrateplan(dsrateplans.FIELD_CODIGOTARIFA).ToString())

                                                Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString())

                                                Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(room, lockRatePlans)

                                                Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

                                                Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

                                                Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, RestrictionEnum.LockRoomType)

                                                If restrictionResponse.IsSuccess Then
                                                    pageBase.WriteLog(restrictionResponse.Restrictions(0).XmlRequest(0).ToString(), "LockRoomType")
                                                    pageBase.WriteLog(restrictionResponse.Restrictions(0).Xml(0).ToString(), "LockRoomType")
                                                Else
                                                    pageBase.WriteLog(restrictionResponse.Xml.ToString(), "LockRoomType")
                                                End If

                                            End If 'Termina Google

                                        Catch ex As Exception
                                            pageBase.WriteLog(ex.Message, "LockRoomType")
                                        End Try

                                    Next
                                Next
                            Else
                                'Se guarda por todos los rateplans y la habitacion que se eligio
                                For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                                    service.SaveData(roomClosureRQ.IdHotel, dateClosure.StartDate, dateClosure.EndDate,
                                                drrateplan(dsrateplans.FIELD_CODIGOTARIFA), roomClosureRQ.RoomOption,
                                                roomClosureRQ.Status)

                                    Try

                                        If isEnabledGoogleRequest Then

                                            Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                            dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))

                                            Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, drrateplan(dsrateplans.FIELD_CODIGOTARIFA).ToString())

                                            Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = roomClosureRQ.RoomOption)

                                            Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(room, lockRatePlans)

                                            Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

                                            Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

                                            Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, RestrictionEnum.LockRoomType)

                                            If restrictionResponse.IsSuccess Then
                                                pageBase.WriteLog(restrictionResponse.Restrictions(0).XmlRequest(0).ToString(), "LockRoomType")
                                                pageBase.WriteLog(restrictionResponse.Restrictions(0).Xml(0).ToString(), "LockRoomType")
                                            Else
                                                pageBase.WriteLog(restrictionResponse.Xml.ToString(), "LockRoomType")
                                            End If

                                        End If 'Termina Google
                                    Catch ex As Exception
                                        pageBase.WriteLog(ex.Message, "LockRoomType")
                                    End Try


                                Next
                            End If
                        Else
                            If allRooms Then
                                'With New RoomFacade
                                '    dsrooms = .getRooms(idHotel)
                                'End With
                                'Se guarda por el rateplan que se eligio y todas las habitaciones
                                For Each drroom As DataRow In dsrooms.Tables(0).Rows
                                    service.SaveData(roomClosureRQ.IdHotel, dateClosure.StartDate, dateClosure.EndDate,
                                               ratePlan.Code, drroom(dsrooms.FLD_ID_ROOM_HOTEL),
                                               roomClosureRQ.Status)

                                    Try

                                        If isEnabledGoogleRequest Then

                                            Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                            dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))

                                            Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, ratePlan.Code)

                                            Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString())

                                            Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(room, lockRatePlans)

                                            Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

                                            Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

                                            Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, RestrictionEnum.LockRoomType)

                                            If restrictionResponse.IsSuccess Then
                                                pageBase.WriteLog(restrictionResponse.Restrictions(0).XmlRequest(0).ToString(), "LockRoomType")
                                                pageBase.WriteLog(restrictionResponse.Restrictions(0).Xml(0).ToString(), "LockRoomType")
                                            Else
                                                pageBase.WriteLog(restrictionResponse.Xml.ToString(), "LockRoomType")
                                            End If

                                        End If 'Termina Google

                                    Catch ex As Exception
                                        pageBase.WriteLog(ex.Message, "LockRoomType")
                                    End Try

                                Next
                            Else
                                'Se guarda por el rateplan que se eligio y la habitacion que se eligio
                                service.SaveData(roomClosureRQ.IdHotel, dateClosure.StartDate, dateClosure.EndDate,
                                               ratePlan.Code, roomClosureRQ.RoomOption,
                                               roomClosureRQ.Status)

                                If isEnabledGoogleRequest Then

                                    Try

                                        Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                        dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))

                                        Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, ratePlan.Code)

                                        Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = roomClosureRQ.RoomOption)

                                        Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(room, lockRatePlans)

                                        Dim lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans)

                                        Dim lockRatePlanSoapRQ As XDocument = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ)

                                        Dim restrictionResponse As RestrictionResponse = confluxService.UpdateRestriction(lockRatePlanSoapRQ, RestrictionEnum.LockRoomType)

                                        If restrictionResponse.IsSuccess Then
                                            pageBase.WriteLog(restrictionResponse.Restrictions(0).XmlRequest(0).ToString(), "LockRoomType")
                                            pageBase.WriteLog(restrictionResponse.Restrictions(0).Xml(0).ToString(), "LockRoomType")
                                        Else
                                            pageBase.WriteLog(restrictionResponse.Xml.ToString(), "LockRoomType")
                                        End If

                                    Catch ex As Exception
                                        pageBase.WriteLog(ex.Message, "LockRoomType")
                                    End Try

                                End If

                            End If
                        End If

                        page.guardalog("rate-manager-ui/dist/rooms-closure.aspx", PaginaBase.acciones.Crear, nota, roomClosureRQ.IdHotel)

                    Catch ex As Exception
                        Dim errorMessage As String = "Error: " & ex.Message
                        page.guardalog("rate-manager-ui/dist/rooms-closure.aspx", PaginaBase.acciones.Crear, errorMessage, roomClosureRQ.IdHotel)
                        Return New HttpResponseMessage(Net.HttpStatusCode.InternalServerError)
                    End Try
                Next

            Next

            Return New HttpResponseMessage(Net.HttpStatusCode.OK)

        End Function


        'Post api/closure/save/1978/2020-12-28/2020-12-29
        '<Route("save/{idHotel:Int}/{startDate:datetime}/{endDate:datetime}"), HttpPost>
        'Public Function SaveClosureByHotelId(ByVal idHotel As Integer, ByVal startDate As Date, ByVal endDate As Date,
        '                                     <FromBody> roomClosureRQ As RoomClosureRQ) As HttpResponseMessage

        '    Dim allRatePlans As Boolean = (roomClosureRQ.RatePlanOption = "0")
        '    Dim allRooms As Boolean = (roomClosureRQ.RoomOption = "0")
        '    Dim dsrateplans As RatePlanData, dsrooms As RoomsHotelData

        '    Dim nota As String = String.Empty

        '    Dim hotelName As String

        '    Dim page As New PaginaBase
        '    hotelName = page.HotelName

        '    nota &= "Cierre en el hotel " & hotelName
        '    If allRooms Then
        '        nota &= " en todas las habitaciones,"
        '    Else
        '        nota &= " en la habitación " & roomClosureRQ.RoomOption
        '    End If
        '    If allRatePlans Then
        '        nota &= " en todos los Rate Plans"
        '    Else
        '        nota &= " en el Rate Plan " & roomClosureRQ.RatePlanOption
        '    End If

        '    nota &= " del " & startDate.ToString("yyyy/MM/dd") & " al " & endDate.ToString("yyyy/MM/dd") & " "

        '    nota &= " Status: " & GetStatus(roomClosureRQ.Status) & ". "


        '    Try
        '        If allRatePlans Then
        '            With New RatePlanFacade
        '                dsrateplans = .GetRatePlanByIdHotel(idHotel.ToString(), PortalCulture.GetIDCulture, 0, 1, idAsociacion:=page.GetIdAsociation, DeleteFilter:=1)
        '            End With
        '            If allRooms Then
        '                With New RoomFacade
        '                    dsrooms = .getRooms(idHotel)
        '                End With
        '                'Se guarda por todos los rateplans y todas las habitaciones
        '                For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
        '                    For Each drroom As DataRow In dsrooms.Tables(0).Rows
        '                        service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
        '                                     drrateplan(dsrateplans.FIELD_CODIGOTARIFA), drroom(dsrooms.FLD_ID_ROOM_HOTEL),
        '                                     roomClosureRQ.Status)
        '                    Next
        '                Next
        '            Else
        '                'Se guarda por todos los rateplans y la habitacion que se eligio
        '                For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
        '                    service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
        '                                    drrateplan(dsrateplans.FIELD_CODIGOTARIFA), roomClosureRQ.RoomOption,
        '                                    roomClosureRQ.Status)
        '                Next
        '            End If
        '        Else
        '            If allRooms Then
        '                With New RoomFacade
        '                    dsrooms = .getRooms(idHotel)
        '                End With
        '                'Se guarda por el rateplan que se eligio y todas las habitaciones
        '                For Each drroom As DataRow In dsrooms.Tables(0).Rows
        '                    service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
        '                                   roomClosureRQ.RatePlanOption, drroom(dsrooms.FLD_ID_ROOM_HOTEL),
        '                                   roomClosureRQ.Status)
        '                Next
        '            Else
        '                'Se guarda por el rateplan que se eligio y la habitacion que se eligio
        '                service.SaveData(roomClosureRQ.IdHotel, roomClosureRQ.StartDate, roomClosureRQ.EndDate,
        '                                   roomClosureRQ.RatePlanOption, roomClosureRQ.RoomOption,
        '                                   roomClosureRQ.Status)
        '            End If
        '        End If

        '        page.guardalog("rate-manager-ui/dist/rooms-closure.aspx", PaginaBase.acciones.Crear, nota, roomClosureRQ.IdHotel)

        '    Catch ex As Exception
        '        Dim errorMessage As String = "Error: " & ex.Message
        '        page.guardalog("rate-manager-ui/dist/rooms-closure.aspx", PaginaBase.acciones.Crear, errorMessage, roomClosureRQ.IdHotel)
        '        Return New HttpResponseMessage(Net.HttpStatusCode.InternalServerError)
        '    End Try

        '    Return New HttpResponseMessage(Net.HttpStatusCode.OK)

        'End Function

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
