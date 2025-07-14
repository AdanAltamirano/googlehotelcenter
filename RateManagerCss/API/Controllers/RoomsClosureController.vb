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
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)
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


                                        If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                                            Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                            dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))
                                            Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, drrateplan(dsrateplans.FIELD_CODIGOTARIFA).ToString())
                                            Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString())
                                            ExecuteServices(lockRatePlans, room, idHotel, isEnabledGoogleRequest, isEnabledSendingRatesAPICache, pageBase)
                                        End If

                                    Next
                                Next
                            Else
                                'Se guarda por todos los rateplans y la habitacion que se eligio
                                For Each drrateplan As DataRow In dsrateplans.Tables(0).Rows
                                    service.SaveData(roomClosureRQ.IdHotel, dateClosure.StartDate, dateClosure.EndDate,
                                                drrateplan(dsrateplans.FIELD_CODIGOTARIFA), roomClosureRQ.RoomOption,
                                                roomClosureRQ.Status)

                                    If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                                        Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                        dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))
                                        Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, drrateplan(dsrateplans.FIELD_CODIGOTARIFA).ToString())
                                        Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = roomClosureRQ.RoomOption)
                                        ExecuteServices(lockRatePlans, room, idHotel, isEnabledGoogleRequest, isEnabledSendingRatesAPICache, pageBase)
                                    End If

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

                                    If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                                        Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                        dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))
                                        Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, ratePlan.Code)
                                        Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = drroom(dsrooms.FLD_ID_ROOM_HOTEL).ToString())
                                        ExecuteServices(lockRatePlans, room, idHotel, isEnabledGoogleRequest, isEnabledSendingRatesAPICache, pageBase)
                                    End If

                                Next
                            Else
                                'Se guarda por el rateplan que se eligio y la habitacion que se eligio
                                service.SaveData(roomClosureRQ.IdHotel, dateClosure.StartDate, dateClosure.EndDate,
                                               ratePlan.Code, roomClosureRQ.RoomOption,
                                               roomClosureRQ.Status)


                                If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                                    Dim dates As List(Of Tuple(Of Date, Date)) = New List(Of Tuple(Of Date, Date))
                                    dates.Add(New Tuple(Of Date, Date)(dateClosure.StartDate, dateClosure.EndDate))
                                    Dim lockRatePlans As List(Of spGetLockRatePlansByHotel_Result) = RestrictionHelper.CreateLockRatePlansByHotel(dates, roomClosureRQ.Status, ratePlan.Code)
                                    Dim room As DataRow = rooms.FirstOrDefault(Function(r) r.Item(0).ToString() = roomClosureRQ.RoomOption)
                                    ExecuteServices(lockRatePlans, room, idHotel, isEnabledGoogleRequest, isEnabledSendingRatesAPICache, pageBase)
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

        <Route("rateplanssegmentsvalids/{idHotel:Int}"), HttpGet>
        Public Function GetRatePlansByHotelIdSegmentsValids(ByVal idHotel As Integer) As List(Of DTO.RatePlansClosureModel)

            Dim page As New PaginaBase

            Dim idAsoc As Integer = page.GetIdAsociation()

            Return service.LoadRatePlanByIdHotelNoSegmentsInvalids(idHotel, idAsoc, page.IdCorporativoUserChain, page.IsHotel,
                                          page.IsUsuarioHotel, PortalCulture.GetIDCulture)
        End Function


        <Route("rateplansnolinks/{idHotel:Int}"), HttpGet>
        Public Function GetRatePlansByHotelIdNoLinks(ByVal idHotel As Integer) As List(Of DTO.RatePlansClosureModel)

            Dim page As New PaginaBase

            Dim idAsoc As Integer = page.GetIdAsociation()

            Return service.LoadRatePlanByIdHotelNoLinks(idHotel, idAsoc, page.IdCorporativoUserChain, page.IsHotel,
                                          page.IsUsuarioHotel, PortalCulture.GetIDCulture)
        End Function

        <Route("promos/{idHotel:Int}"), HttpGet>
        Public Function GetPromosByHotelId(ByVal idHotel As Integer) As List(Of DTO.RatePlansClosureModel)

            Return service.LoadPromosByIdHotel(idHotel)
        End Function

        <Route("rooms/{idHotel:Int}"), HttpGet>
        Public Function GetRoomsByHotelId(ByVal idHotel As Integer) As List(Of DTO.RoomsModel)


            Return service.LoadRoomsByIdHotel(idHotel, PortalCulture.GetIDCulture)
        End Function

        <Route("roomsnolinks/{idHotel:Int}"), HttpGet>
        Public Function GetRoomsByHotelIdNoLinks(ByVal idHotel As Integer) As List(Of DTO.RoomsModel)
            Return service.LoadRoomsByHotelIdHotelNoLinks(idHotel, PortalCulture.GetIDCulture)
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

        Private Sub SendClosureToService(ByVal lockRatePlanHotelAvailNotifRQList As List(Of XElement), ByVal idHotel As Integer, ByVal endpoint As String, ByVal service As String, ByRef page As PaginaBase)
            Try
                Dim lockRatePlanHotelAvailNotifSoapRQList As List(Of XDocument) = New List(Of XDocument)
                For Each request As XElement In lockRatePlanHotelAvailNotifRQList
                    Dim lockRatePlanHotelAvailSoapRQ = Soap.CreateSoapRequestXml(request)
                    lockRatePlanHotelAvailNotifSoapRQList.Add(lockRatePlanHotelAvailSoapRQ)
                Next
                Dim index As Integer = 0
                For Each lockRatePlanSoapRQ As XDocument In lockRatePlanHotelAvailNotifSoapRQList
                    Dim restrictionResponse As RestrictionResponse = HotelUtilitie.ConfluxServiceHelper.UpdateRestriction(lockRatePlanSoapRQ, endpoint, RestrictionEnum.LockRoomType)
                    Dim note As String = String.Format("Sincronizar request numero {0} LockRoomType {1} con el hotel: ", (index + 1), service)
                    Dim noteError As String = String.Format("Error al sincronizar con {0}", service)
                    If restrictionResponse.IsSuccess Then
                        page.guardalog(pagina:="/rate-manager-ui/dist/rooms-closure.aspx", action:=acciones.Sincronizar, nota:=note, peticion:="", datos:=restrictionResponse.Restrictions(0).XmlRequest(0).ToString(), datosDespues:=restrictionResponse.Restrictions(0).Xml(0).ToString(), hotelId:=idHotel)

                    Else
                        page.guardalog(pagina:="/rate-manager-ui/dist/rooms-closure.aspx", action:=acciones.Sincronizar, nota:=noteError, peticion:="", datos:=restrictionResponse.Xml.ToString(), datosDespues:="", hotelId:=idHotel)
                    End If
                    index = index + 1
                Next
            Catch ex As Exception
                page.guardalog(pagina:="/rate-manager-ui/dist/rooms-closure.aspx", action:=acciones.Sincronizar, nota:="Error al sincronizar", peticion:="", datos:=ex.Message, datosDespues:="", hotelId:=idHotel)
            End Try
        End Sub

        Private Sub ExecuteServices(ByVal lockRatePlans As List(Of spGetLockRatePlansByHotel_Result), ByVal room As DataRow, ByVal idHotel As Integer, ByVal isEnabledGoogleRequest As Boolean, ByVal isEnabledSendingRatesAPICache As Boolean, ByVal pageBase As PaginaBase)
            Dim availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(room, lockRatePlans)
            Dim lockRatePlanHotelAvailNotifRQList As List(Of XElement) = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockRatePlans)

            If isEnabledGoogleRequest And lockRatePlanHotelAvailNotifRQList IsNot Nothing Then
                SendClosureToService(lockRatePlanHotelAvailNotifRQList, idHotel, HotelUtilitie.ENDPOINTCLOSURE, "Conflux", pageBase)
            End If

            If isEnabledSendingRatesAPICache And lockRatePlanHotelAvailNotifRQList IsNot Nothing Then
                SendClosureToService(lockRatePlanHotelAvailNotifRQList, idHotel, HotelUtilitie.ENDPOINTAPICLOSURE, "APICache", pageBase)
            End If
        End Sub

    End Class
End Namespace
