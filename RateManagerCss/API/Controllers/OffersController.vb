Imports System.Web.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Common
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.API.Models
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports RateManager.Utitlities.Hotel
Imports APIServices.Conflux.Models.RatePlan.Response
Imports APIServices.Conflux.Enum
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.OTA.Models.Rates
Imports APIServices.Conflux
Imports APIServices.Conflux.Helpers.Rooms
Imports APIServices.Conflux.Helpers.Rates


Namespace API.Controllers

    <RoutePrefix("api/promotions")>
    Public Class OffersController
        Inherits ShurikenController
        Public service As New OfferService

        'GET api/promotions?querystring 
        <Route(""), HttpGet, Queryable>
        Public Function GetByHotelId() As IQueryable(Of vPromotions)
            Return service.OffersList()
        End Function

        ' GET api/promotions/1978/code/PR01
        <Route("{HotelId:Int}/code/{code}"), HttpGet>
        Public Function GetByCode(HotelId As Integer, code As String) As DTO.Offer
            Return service.FindOfferByHotelAndCode(HotelId, code)
        End Function

        'TODO: Cambiar la ruta de la api en comentarios en las diferentes acciones 

        'POST api/promotions/1978/save
        <Route("{hotelId:int}/save"), HttpPost>
        Public Function OfferAdd(<FromBody> RQ As DTO.Offer, hotelId As Integer) As Net.Http.HttpResponseMessage

            Dim page As New PaginaBase
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)

            Dim tasksToExecuteInsertPromoRatePlan As New List(Of Func(Of Task))()
            Dim userName As String = page.ReadUserCookie().GetValue(0)
            Dim userId As Integer = page.UserIdentityName

            RQ.HotelId = hotelId

            Dim result As KeyValuePair(Of String, String) = service.Add(RQ)

            If result.Key = 1 Then

                'Dim page As New PaginaBase
                'Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                'Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                'Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                Dim mensajeCreacion As String = String.Format("Se creo la promocion con del codigo {0}", RQ.Id)
                Page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensajeCreacion, "", "", "", info.Hotel)

                For Each plan As String In RQ.ApplicableFor.RatesPlan
                    Dim promotionRatePlanId As String = RQ.Id & plan
                    Dim ratePlanNameId As String = HotelUtilitie.GetRatePlanNameById(plan, hotelId) & " - " & RQ.Name.Esp

                    tasksToExecuteInsertPromoRatePlan.Add(Function() InsertPromoRatePlanAsync(userName, userId, info.Hotel, info.Empresa, isEnabledGoogleRequest, promotionRatePlanId, ratePlanNameId, RQ.Description.Esp, "ES"))

                    'Dim mensaje As String = String.Format("Sincronizar Nuevo  Codigo de Promocion con RatePlan {0}", promotionRatePlanId)
                    'Dim res As RatePlanResponse = HotelUtilitie.ConfluxServiceHelper.InsertRatePlan(info.Hotel, info.Empresa, promotionRatePlanId, ratePlanNameId, RQ.Description.Esp, "ES")
                    'Page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensaje, "", res.RequestXML, res.Response, info.Hotel)
                Next

                Task.Run(Async Function()
                             Await InsertRatePlansPromosAndSendRates(userName, userId, info.Hotel, info.Empresa, RQ.Id.ToUpper(), tasksToExecuteInsertPromoRatePlan)
                         End Function)

                'If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                '    ExecuteServices(info.Hotel, info.Empresa, RQ.Id.ToUpper(), isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                'End If


                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/promotions/1978/update/code/PR04
        <Route("{hotelId:int}/update/code/{code}"), HttpPost>
        Public Function OfferUpdate(<FromBody> RQ As DTO.Offer, hotelId As Integer, code As String) As Net.Http.HttpResponseMessage

            Dim page As New PaginaBase
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)

            Dim tasksToExecuteInsertPromoRatePlan As New List(Of Func(Of Task))()
            Dim userName As String = page.ReadUserCookie().GetValue(0)
            Dim userId As Integer = page.UserIdentityName


            RQ.HotelId = hotelId

            Dim auxOffer As DTO.Offer = service.FindOfferByHotelAndCode(hotelId, code)

            Dim result As KeyValuePair(Of String, String) = service.Update(RQ)

            If result.Key = 1 Then

                'Dim page As New PaginaBase
                'Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                'Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                'Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                Dim mensajeActualizacion As String = String.Format("Se actualizo la promocion con del codigo {0}", code)
                page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensajeActualizacion, "", "", "", info.Hotel)

                For Each plan As String In RQ.ApplicableFor.RatesPlan
                    Dim promotionRatePlanId As String = RQ.Id & plan
                    Dim ratePlanNameId As String = HotelUtilitie.GetRatePlanNameById(plan, hotelId) & " - " & RQ.Name.Esp

                    tasksToExecuteInsertPromoRatePlan.Add(Function() InsertPromoRatePlanAsync(userName, userId, info.Hotel, info.Empresa, isEnabledGoogleRequest, promotionRatePlanId, ratePlanNameId, RQ.Description.Esp, "ES"))


                    'Dim mensaje As String = String.Format("Sincronizar Modificacion Codigo de Promocion con RatePlan {0}", promotionRatePlanId)
                    'Dim res As RatePlanResponse = HotelUtilitie.ConfluxServiceHelper.InsertRatePlan(info.Hotel, info.Empresa, promotionRatePlanId, ratePlanNameId, RQ.Description.Esp, "ES")
                    'page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensaje, "", res.RequestXML, res.Response, info.Hotel)
                Next

                Dim deleteRates As Boolean = False
                Dim endDateDelete As Date = Nothing
                Dim column As String = "idTipoHabitacion_Hotel"
                Dim filterRooms As String = $" AND {column} IN ({String.Join(", ", auxOffer.ApplicableFor.Rooms.Select(Function(id) id.ToString()))})"
                Dim roomsList As List(Of DataRow) = New List(Of DataRow)
                Dim roomCodesList As List(Of String) = New List(Of String)



                If auxOffer.StartDate <> RQ.StartDate Then
                    endDateDelete = CType(RQ.StartDate, Date).AddDays(-1)
                    roomsList = RoomsHelper.GetRoomsByHotel(hotelId, filterRooms)
                    roomCodesList = roomsList.Select(Function(r) r.ItemArray(22).ToString()).ToList()
                    deleteRates = True
                End If

                Task.Run(Async Function()
                             Await InsertRatePlansPromosAndSendRates(userName, userId, info.Hotel, info.Empresa, code, tasksToExecuteInsertPromoRatePlan,
                                                                         deleteRates:=deleteRates, promoRatePlanCodeToDelete:=code, dateToDelete:=endDateDelete,
                                                                         ratePlansListAux:=auxOffer.ApplicableFor.RatesPlan, roomsListAux:=roomCodesList)
                         End Function)


                'If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                '    If auxOffer.StartDate <> RQ.StartDate Then
                '        Dim endDate As Date = CType(RQ.StartDate, Date).AddDays(-1)

                '        Dim column As String = "idTipoHabitacion_Hotel"
                '        Dim filterRooms As String = $" AND {column} IN ({String.Join(", ", auxOffer.ApplicableFor.Rooms.Select(Function(id) id.ToString()))})"

                '        Dim roomsList As List(Of DataRow) = RoomsHelper.GetRoomsByHotel(hotelId, filterRooms)
                '        Dim roomCodesList As List(Of String) = roomsList.Select(Function(r) r.ItemArray(22).ToString()).ToList()
                '        ExecuteServicesDelete(info.Hotel, info.Empresa, code, endDate, auxOffer.ApplicableFor.RatesPlan, roomCodesList, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)

                '    End If
                '    ExecuteServices(info.Hotel, info.Empresa, code, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                'End If

                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/promotions/enable/1978/code/PR04
        <Route("enable/{hotelId:Int}/code/{code}"), HttpPost>
        Public Function EnablePromotion(hotelId As Integer, code As String) As Net.Http.HttpResponseMessage

            Dim page As New PaginaBase
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim userName As String = page.ReadUserCookie().GetValue(0)
            Dim userId As Integer = page.UserIdentityName

            With New RatePlanFacade
                If .LogicActiveRatePlan(hotelId, code) Then

                    With New PaginaBase
                        .guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Reactivar, "Ractivo la promoción con el codigo de tarifa " & code)
                    End With

                    Task.Run(Async Function()
                                 Try
                                     Await SendRatesAsync(userName, userId, info.Hotel, info.Empresa, code)
                                 Catch ex As Exception

                                 End Try
                             End Function)


                    'Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                    'Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                    'Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                    'If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                    '    ExecuteServices(info.Hotel, info.Empresa, code, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                    'End If

                    Return NoContent()
                End If
            End With
            Return BadRequest(New KeyValuePair(Of String, String)("0", "No se pudo activar la promoción"))
        End Function

        'POST api/promotions/disable/1978/code/PR04
        <Route("disable/{hotelId:Int}/code/{code}"), HttpPost>
        Public Function DisablePromotion(hotelId As Integer, code As String) As Net.Http.HttpResponseMessage

            Dim page As New PaginaBase
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim userName As String = page.ReadUserCookie().GetValue(0)
            Dim userId As Integer = page.UserIdentityName
            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

            Dim vDayRatesPromotion As List(Of vDayRates) = New List(Of vDayRates)()
            Dim vDayRatesPromotionException As List(Of vDayRatesExceptions) = New List(Of vDayRatesExceptions)()

            If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                vDayRatesPromotion = RatesHelpers.GetVDayRatePromotion(info.Hotel, code)
                vDayRatesPromotionException = RatesHelpers.GetVDayRatePromotionException(info.Hotel, code)
            End If

            With New RatePlanFacade
                If .LogicDeleteRatePlan(hotelId, code) Then
                    With New PaginaBase
                        .guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Eliminar, "Eliminó la promoción con el codigo de tarifa " & code)
                    End With

                    Task.Run(Async Function()
                                 Try
                                     Await DeleteRatesAsync(userName, userId, info.Hotel, info.Empresa, vDayRatesPromotion, vDayRatesPromotionException)
                                 Catch ex As Exception

                                 End Try
                             End Function)

                    'ExecuteServicesDelete(info.Hotel, info.Empresa, isEnabledGoogleRequest, isEnabledSendingRatesAPICache, vDayRatesPromotion, vDayRatesPromotionException)

                    Return NoContent()
                End If
            End With
            Return BadRequest(New KeyValuePair(Of String, String)("0", "No se pudo desactivar la promoción"))
        End Function

        Private Sub ExecuteServices(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal idRatePlan As String, ByVal isEnabledGoogleRequest As Boolean, ByVal isEnabledSendingRatesAPICache As Boolean)
            Dim ratesForRequest As RatesMessages = HotelUtilitie.ConfluxServiceHelper.GetRateMessagesPromotion(hotelId, idRatePlan, companyId, TypeRateEnum.RoomRate)
            Dim ratesForRequestPromotion As RatesMessages = HotelUtilitie.ConfluxServiceHelper.GetRateMessagesPromotion(hotelId, idRatePlan, companyId, TypeRateEnum.RoomRatePromotion)

            If isEnabledGoogleRequest Then
                SendRatesToService(ratesForRequest, ratesForRequestPromotion, HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE, hotelId, "Conflux", True)
            End If

            If isEnabledSendingRatesAPICache Then
                SendRatesToService(ratesForRequest, ratesForRequestPromotion, HotelUtilitie.ENDPOINTAPI, HotelUtilitie.ENDPOINTAPIDELETE, hotelId, "APICache", False)
            End If

        End Sub

        Private Sub SendRatesToService(ByVal ratesForRequest As RatesMessages, ByVal ratesForRequestPromotion As RatesMessages, ByVal endpoint As String, ByVal endpointDelete As String, ByVal hotelId As String, ByVal service As String, Optional ByVal deleteRates As Boolean = True)
            Dim pgBase As PaginaBase = New PaginaBase()

            Dim note As String = String.Format("Sincronizar Promotions {0}", service)
            Dim noteDelete As String = String.Format("Eliminar Promotions {0}", service)

            Dim noteException As String = String.Format("Sincronizar exception Promotions {0}", service)
            Dim noteDeleteException As String = String.Format("Eliminar exception Promotions {0}", service)

            Dim res As Tuple(Of RateResponse, RateResponse) = HotelUtilitie.ConfluxServiceHelper.UpdateRate(ratesForRequest, endpoint, endpointDelete, deleteRates)

            pgBase.guardalog("/rate-manager-ui/dist/Promotions.aspx", pgBase.acciones.Sincronizar, note, "", res.Item1.RequestXML, res.Item1.Xml, hotelId)

            If res.Item2 IsNot Nothing Then
                pgBase.guardalog("/rate-manager-ui/dist/Promotions.aspx", pgBase.acciones.Eliminar, noteDelete, "", res.Item2.RequestXML, res.Item2.Xml, hotelId)
            End If

            Dim resPromotion As Tuple(Of RateResponse, RateResponse) = HotelUtilitie.ConfluxServiceHelper.UpdateRate(ratesForRequestPromotion, endpoint, endpointDelete, deleteRates)
            pgBase.guardalog("/rate-manager-ui/dist/Promotions.aspx", pgBase.acciones.Sincronizar, noteException, "", resPromotion.Item1.RequestXML, resPromotion.Item1.Xml, hotelId)

            If resPromotion.Item2 IsNot Nothing Then
                pgBase.guardalog("/rate-manager-ui/dist/Promotions.aspx", pgBase.acciones.Eliminar, noteDeleteException, "", resPromotion.Item2.RequestXML, resPromotion.Item2.Xml, hotelId)
            End If
        End Sub

        Private Sub ExecuteServicesDelete(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal idRatePlan As String, ByVal endDate As Date, ByVal ratesPlansListAux As List(Of String), ByVal roomsListAux As List(Of String), ByVal isEnabledGoogleRequest As Boolean, ByVal isEnabledSendingRatesAPICache As Boolean)

            Dim deleteMessages As RateAmountMessages = CreateDeleteRateAmountMessages(hotelId, companyId, idRatePlan, endDate, ratesPlansListAux, roomsListAux)

            If isEnabledGoogleRequest Then
                SendDeleteRatesToService(hotelId, HotelUtilitie.ENDPOINTDELETE, "Conflux", deleteMessages)
            End If

            If isEnabledSendingRatesAPICache Then
                SendDeleteRatesToService(hotelId, HotelUtilitie.ENDPOINTAPIDELETE, "APICache", deleteMessages)
            End If
        End Sub

        Private Sub ExecuteServicesDelete(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal isEnabledGoogleRequest As Boolean, ByVal isEnabledSendingRatesAPICache As Boolean, ByVal vDayRatesPromotion As List(Of vDayRates), ByVal vDayRatesPromotionException As List(Of vDayRatesExceptions))

            Dim rateAmountMessages As RateAmountMessages = Nothing
            Dim rateAmountMessagesPromotion As RateAmountMessages = Nothing

            If vDayRatesPromotion.Count > 0 Then
                rateAmountMessages = New RateAmountMessages()
                rateAmountMessages.HotelCode = companyId
                rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
                Parser.Parser.ToRateAmountMessagesDelete(vDayRatesPromotion, Nothing, TypeRateEnum.RoomRate, rateAmountMessages.RateAmountMessagesList)
            End If

            If vDayRatesPromotionException.Count > 0 Then
                rateAmountMessagesPromotion = New RateAmountMessages()
                rateAmountMessagesPromotion.HotelCode = companyId
                rateAmountMessagesPromotion.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
                Parser.Parser.ToRateAmountMessagesDelete(Nothing, vDayRatesPromotionException, TypeRateEnum.RoomRate, rateAmountMessagesPromotion.RateAmountMessagesList)
            End If

            If isEnabledGoogleRequest And rateAmountMessages IsNot Nothing Then
                SendDeleteRatesToService(hotelId, HotelUtilitie.ENDPOINTDELETE, "Conflux", rateAmountMessages)
            End If

            If isEnabledGoogleRequest And rateAmountMessagesPromotion IsNot Nothing Then
                SendDeleteRatesToService(hotelId, HotelUtilitie.ENDPOINTDELETE, "Conflux", rateAmountMessagesPromotion)
            End If

            If isEnabledSendingRatesAPICache And rateAmountMessages IsNot Nothing Then
                SendDeleteRatesToService(hotelId, HotelUtilitie.ENDPOINTAPIDELETE, "APICache", rateAmountMessages)
            End If

            If isEnabledSendingRatesAPICache And rateAmountMessagesPromotion IsNot Nothing Then
                SendDeleteRatesToService(hotelId, HotelUtilitie.ENDPOINTAPIDELETE, "APICache", rateAmountMessagesPromotion)
            End If


        End Sub

        Private Sub SendDeleteRatesToService(ByVal hotelId As Integer, ByVal endpoint As String, ByVal service As String, ByVal rateAmountMessages As RateAmountMessages)
            Dim pgBase As PaginaBase = New PaginaBase()
            Dim rateResponseDelete As RateResponse = HotelUtilitie.ConfluxServiceHelper.DeleteRates(endpoint, rateAmountMessages)
            Dim note As String = String.Format("Eliminar Promotions {0}", service)
            pgBase.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Eliminar, note, "", rateResponseDelete.RequestXML, rateResponseDelete.Xml, hotelId)
        End Sub

        Private Function CreateDeleteRateAmountMessages(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal promotionCode As String, ByVal endDate As Date, ByVal rateplansListAux As List(Of String), ByVal roomsListAux As List(Of String)) As RateAmountMessages
            Dim rateAmountMessages As RateAmountMessages = New RateAmountMessages

            rateAmountMessages.HotelCode = companyId
            rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            For Each ratePlan As String In rateplansListAux
                For Each room As String In roomsListAux

                    Dim ratePlanCode As String = promotionCode & ratePlan

                    Dim rateAmountMessage As OTA.Models.Rates.RateAmountMessage = New OTA.Models.Rates.RateAmountMessage()

                    rateAmountMessage.statusApplicationControl = New OTA.Models.Rates.StatusApplicationControl()
                    rateAmountMessage.statusApplicationControl.RatePlanCode = ratePlanCode
                    rateAmountMessage.statusApplicationControl.InvTypeCode = room

                    Dim ratesList As List(Of OTA.Models.Rates.Rate) = New List(Of OTA.Models.Rates.Rate)

                    Dim rate As OTA.Models.Rates.Rate = New OTA.Models.Rates.Rate()

                    rate.StartDate = DateTime.Now.Date.ToString("yyyyMMdd")
                    rate.EndDate = endDate.Date.ToString("yyyyMMdd")

                    ratesList.Add(rate)

                    rateAmountMessage.Rates = ratesList

                    rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage)

                Next
            Next

            Return rateAmountMessages
        End Function

#Region "Google Async"

        Private Async Function InsertPromoRatePlanAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer, ByVal isEnabled As Boolean,
                                                ByVal promotionId As String, ByVal ratePlanPromoId As String, ByVal description As String, Optional language As String = "ES") As Task
            Try

                If isEnabled Then

                    Dim confluxService As New ConfluxService()

                    Dim res As RatePlanResponse = Await confluxService.InsertRatePlanAsync(hotelId, companyId, promotionId, ratePlanPromoId, description, language)

                    Dim note As String = String.Format("Sincronizar Nuevo  Codigo de Promocion con RatePlan {0}", promotionId)

                    HotelUtilitie.Log(userName, userId, "/Pages/Promotions.aspx", hotelId, Actions.Sincronizar, note, "", res.RequestXML, res.Response)

                End If


            Catch ex As Exception

                Dim noteError As String = String.Format("Error al Sincronizar Nuevo  Codigo de Promocion con RatePlan {0}", promotionId)

                HotelUtilitie.Log(userName, userId, "/Pages/Promotions.aspx", hotelId, Actions.Sincronizar, noteError, "", "", "")

            End Try
        End Function

        Private Async Function InsertRatePlansPromosAndSendRates(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer, ByVal promoRatePlanCode As String,
                                                            ByVal taskToExecute As List(Of Func(Of Task)), Optional deleteRates As Boolean = False,
                                                                 Optional ByVal promoRatePlanCodeToDelete As String = "", Optional ByVal dateToDelete As Date = Nothing,
                                                                 Optional ByVal ratePlansListAux As List(Of String) = Nothing, Optional ByVal roomsListAux As List(Of String) = Nothing) As Task
            Dim maxConcurrentTasks As Integer = 5
            Dim semaphore As New SemaphoreSlim(maxConcurrentTasks)
            Dim tasksToExecuteInsertPromoRatePlan As New List(Of Func(Of Task))()

            Dim insertRatePlanPromoTasks As New List(Of Task)

            'Mandar RatePlans Promos Async
            For Each taskDelegate As Func(Of Task) In taskToExecute
                insertRatePlanPromoTasks.Add(Task.Run(Async Function()
                                                          Await semaphore.WaitAsync()
                                                          Try
                                                              Await taskDelegate()  ' Aquí se ejecuta MandarTarifasAsync
                                                          Finally
                                                              semaphore.Release()
                                                          End Try
                                                      End Function))
            Next


            ' Ejecuta SendRatesAsync después
            Try
                ' Espera todas las promos
                Await Task.WhenAll(insertRatePlanPromoTasks)

                If deleteRates Then
                    Await DeleteRatesAsync(userName, userId, hotelId, companyId, promoRatePlanCodeToDelete, dateToDelete, ratePlansListAux, roomsListAux)
                End If

                Await SendRatesAsync(userName, userId, hotelId, companyId, promoRatePlanCode)
            Catch ex As Exception
                ' loggear error
            End Try

        End Function


        Private Async Function SendRatesAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer, ByVal rateplanId As String) As Task

            Dim confluxService As New APIServices.Conflux.ConfluxService()

            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            Dim ratesForRequest As RatesMessages = confluxService.GetRateMessagesPromotion(hotelId, rateplanId, companyId, TypeRateEnum.RoomRate)
            Dim ratesForRequestPromotion As RatesMessages = confluxService.GetRateMessagesPromotion(hotelId, rateplanId, companyId, TypeRateEnum.RoomRatePromotion)

            'Google
            Await SendRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledGoogleRequest, ratesForRequest, HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE, "Conflux", True, False)
            Await SendRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledGoogleRequest, ratesForRequestPromotion, HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE, "Conflux", True, True)

            'APICache
            'Await SendRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledSendingRatesAPICache, ratesForRequest, HotelUtilitie.ENDPOINTAPIV2, HotelUtilitie.ENDPOINTAPIDELETE, "APICache", False, False)
            'Await SendRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledSendingRatesAPICache, ratesForRequestPromotion, HotelUtilitie.ENDPOINTAPIV2, HotelUtilitie.ENDPOINTAPIDELETE, "APICache", False, True)

        End Function

        Private Async Function SendRatesIfEnabledAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer, ByVal isEnabled As Boolean,
                                                       ByVal ratesForRequest As RatesMessages,
                                                        ByVal endpoint As String, ByVal endpointDelete As String, ByVal serviceName As String, ByVal deleteRates As Boolean, ByVal isException As Boolean) As Task

            If isEnabled Then

                Try
                    Await SendRatesToServiceAsync(userName, userId, hotelId, ratesForRequest, endpoint, endpointDelete, serviceName, deleteRates, isException)

                Catch ex As Exception

                    Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                    Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                    errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"),
                                             New System.Xml.Linq.XAttribute("Code", "448"),
                                             New System.Xml.Linq.XText(ex.Message))
                    errorsElement.Add(errorElementProperty)

                    HotelUtilitie.Log(userName, userId, "/rate-manager-ui/dist/Promotions.aspx", hotelId, Actions.Sincronizar, $"Error al sincronizar con {serviceName}", "", errorsElement.ToString(), "")

                End Try

            End If


        End Function

        Private Async Function SendRatesToServiceAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal ratesForRequest As RatesMessages, ByVal endpoint As String,
                                                       ByVal endpointDelete As String, ByVal service As String, Optional ByVal deleteRates As Boolean = True, Optional ByVal isException As Boolean = False) As Task


            Dim note As String = IIf(isException,
                             String.Format("Sincronizar exception Promotions {0}", service),
                             String.Format("Sincronizar Promotions {0}", service))

            Dim noteDelete As String = IIf(isException,
                                   String.Format("Eliminar exception Promotions {0}", service),
                                   String.Format("Eliminar Promotions {0}", service))


            Dim confluxService As New ConfluxService()

            Dim ratesMessages As RatesMessages = ratesForRequest

            Dim res As Tuple(Of RateResponse, RateResponse) = Nothing

            If service = "APICache" Then
                res = Await confluxService.UpdateRatePatchAsync(ratesMessages, endpoint, endpointDelete, deleteRates)
            Else
                res = Await confluxService.UpdateRateAsync(ratesMessages, endpoint, endpointDelete, deleteRates)
            End If

            HotelUtilitie.Log(userName, userId, "/rate-manager-ui/dist/Promotions.aspx", hotelId, Actions.Sincronizar, note, "", res.Item1.RequestXML, res.Item1.Xml)

            If res.Item2 IsNot Nothing Then
                HotelUtilitie.Log(userName, userId, "/rate-manager-ui/dist/Promotions.aspx", hotelId, Actions.Eliminar, noteDelete, "", res.Item2.RequestXML, res.Item2.Xml)
            End If

        End Function

        Private Async Function DeleteRatesAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer, ByVal promoRatePlanCodeToDelete As String, ByVal dateToDelete As Date,
                                                          ByVal ratePlansListAux As List(Of String), ByVal roomsListAux As List(Of String)) As Task

            Dim confluxService As New APIServices.Conflux.ConfluxService()

            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            Dim deleteMessages As RateAmountMessages = Parser.Parser.ToRateAmountMessagesDelete(companyId, promoRatePlanCodeToDelete, dateToDelete, ratePlansListAux, roomsListAux)

            Await SendDeleteRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledGoogleRequest, deleteMessages, HotelUtilitie.ENDPOINTDELETE, "Conflux")

        End Function

        Private Async Function DeleteRatesAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer,
                                                ByVal vDayRatesPromotion As List(Of vDayRates), ByVal vDayRatesPromotionException As List(Of vDayRatesExceptions)) As Task

            Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            Dim rateAmountMessages As RateAmountMessages = Nothing
            Dim rateAmountMessagesPromotion As RateAmountMessages = Nothing

            If vDayRatesPromotion.Count > 0 Then
                rateAmountMessages = New RateAmountMessages()
                rateAmountMessages.HotelCode = companyId
                rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
                Parser.Parser.ToRateAmountMessagesDelete(vDayRatesPromotion, Nothing, TypeRateEnum.RoomRate, rateAmountMessages.RateAmountMessagesList)
            End If

            If vDayRatesPromotionException.Count > 0 Then
                rateAmountMessagesPromotion = New RateAmountMessages()
                rateAmountMessagesPromotion.HotelCode = companyId
                rateAmountMessagesPromotion.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
                Parser.Parser.ToRateAmountMessagesDelete(Nothing, vDayRatesPromotionException, TypeRateEnum.RoomRate, rateAmountMessagesPromotion.RateAmountMessagesList)
            End If

            Await SendDeleteRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledGoogleRequest, rateAmountMessages, HotelUtilitie.ENDPOINTDELETE, "Conflux")
            Await SendDeleteRatesIfEnabledAsync(userName, userId, hotelId, companyId, isEnabledGoogleRequest, rateAmountMessagesPromotion, HotelUtilitie.ENDPOINTDELETE, "Conflux")

        End Function

        Private Async Function SendDeleteRatesIfEnabledAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer, ByVal isEnabled As Boolean,
                                                             ByVal deleteMessages As RateAmountMessages, ByVal endpoint As String, ByVal service As String) As Task

            If isEnabled And deleteMessages IsNot Nothing Then

                Try

                    Await SendDeleteRatesToServiceAsync(userName, userId, hotelId, companyId, deleteMessages, endpoint, service)

                Catch ex As Exception
                    Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                    Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                    errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"),
                                             New System.Xml.Linq.XAttribute("Code", "448"),
                                             New System.Xml.Linq.XText(ex.Message))
                    errorsElement.Add(errorElementProperty)

                    HotelUtilitie.Log(userName, userId, "/rate-manager-ui/dist/Promotions.aspx", hotelId, Actions.Sincronizar, $"Error al sincronizar eliminar con {service}", "", errorsElement.ToString(), "")
                End Try

            End If

        End Function

        Private Async Function SendDeleteRatesToServiceAsync(ByVal userName As String, ByVal userId As Integer, ByVal hotelId As Integer, ByVal companyId As Integer,
                                                            ByVal deleteMessages As RateAmountMessages, ByVal endpoint As String, ByVal service As String) As Task

            Dim confluxService As New ConfluxService()

            Dim note As String = String.Format("Eliminar Promotions {0}", service)

            Dim rateResponseDelete As RateResponse = Await confluxService.DeleteRatesAsync(endpoint, deleteMessages)

            HotelUtilitie.Log(userName, userId, "/rate-manager-ui/dist/Promotions.aspx", hotelId, Actions.Eliminar, note, "", rateResponseDelete.RequestXML, rateResponseDelete.Xml)

        End Function



#End Region


    End Class
End Namespace