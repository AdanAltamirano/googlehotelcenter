Imports System.Web.Http
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
            RQ.HotelId = hotelId

            Dim result As KeyValuePair(Of String, String) = service.Add(RQ)

            If result.Key = 1 Then

                Dim page As New PaginaBase
                Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                Dim mensajeCreacion As String = String.Format("Se creo la promocion con del codigo {0}", RQ.Id)
                page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensajeCreacion, "", "", "", info.Hotel)

                For Each plan As String In RQ.ApplicableFor.RatesPlan
                    Dim promotionRatePlanId As String = RQ.Id & plan
                    Dim ratePlanNameId As String = plan & "-" & RQ.Name.Esp

                    Dim mensaje As String = String.Format("Sincronizar Nuevo  Codigo de Promocion con RatePlan {0}", promotionRatePlanId)
                    Dim res As RatePlanResponse = HotelUtilitie.ConfluxServiceHelper.InsertRatePlan(info.Hotel, info.Empresa, promotionRatePlanId, ratePlanNameId, RQ.Description.Esp, "ES")
                    page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensaje, "", res.RequestXML, res.Response, info.Hotel)
                Next

                If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                    ExecuteServices(info.Hotel, info.Empresa, RQ.Id.ToUpper(), isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                End If


                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/promotions/1978/update/code/PR04
        <Route("{hotelId:int}/update/code/{code}"), HttpPost>
        Public Function OfferUpdate(<FromBody> RQ As DTO.Offer, hotelId As Integer, code As String) As Net.Http.HttpResponseMessage
            RQ.HotelId = hotelId

            Dim auxOffer As DTO.Offer = service.FindOfferByHotelAndCode(hotelId, code)

            Dim result As KeyValuePair(Of String, String) = service.Update(RQ)

            If result.Key = 1 Then

                Dim page As New PaginaBase
                Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                Dim mensajeActualizacion As String = String.Format("Se actualizo la promocion con del codigo {0}", code)
                page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensajeActualizacion, "", "", "", info.Hotel)

                For Each plan As String In RQ.ApplicableFor.RatesPlan
                    Dim promotionRatePlanId As String = RQ.Id & plan
                    Dim ratePlanNameId As String = plan & "-" & RQ.Name.Esp

                    Dim mensaje As String = String.Format("Sincronizar Modificacion Codigo de Promocion con RatePlan {0}", promotionRatePlanId)
                    Dim res As RatePlanResponse = HotelUtilitie.ConfluxServiceHelper.InsertRatePlan(info.Hotel, info.Empresa, promotionRatePlanId, ratePlanNameId, RQ.Description.Esp, "ES")
                    page.guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Sincronizar, mensaje, "", res.RequestXML, res.Response, info.Hotel)
                Next

                If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                    If auxOffer.StartDate <> RQ.StartDate Then
                        Dim endDate As Date = CType(RQ.StartDate, Date).AddDays(-1)

                        Dim column As String = "idTipoHabitacion_Hotel"
                        Dim filterRooms As String = $" AND {column} IN ({String.Join(", ", auxOffer.ApplicableFor.Rooms.Select(Function(id) id.ToString()))})"

                        Dim roomsList As List(Of DataRow) = RoomsHelper.GetRoomsByHotel(hotelId, filterRooms)
                        Dim roomCodesList As List(Of String) = roomsList.Select(Function(r) r.ItemArray(22).ToString()).ToList()
                        ExecuteServicesDelete(info.Hotel, info.Empresa, code, endDate, auxOffer.ApplicableFor.RatesPlan, roomCodesList, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)

                    End If
                    ExecuteServices(info.Hotel, info.Empresa, code, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                End If

                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/promotions/enable/1978/code/PR04
        <Route("enable/{hotelId:Int}/code/{code}"), HttpPost>
        Public Function EnablePromotion(hotelId As Integer, code As String) As Net.Http.HttpResponseMessage
            With New RatePlanFacade
                If .LogicActiveRatePlan(hotelId, code) Then

                    With New PaginaBase
                        .guardalog("/rate-manager-ui/dist/Promotions.aspx", PaginaBase.acciones.Reactivar, "Ractivo la promoción con el codigo de tarifa " & code)
                    End With

                    Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                    Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                    Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                    If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                        ExecuteServices(info.Hotel, info.Empresa, code, isEnabledGoogleRequest, isEnabledSendingRatesAPICache)
                    End If

                    Return NoContent()
                End If
            End With
            Return BadRequest(New KeyValuePair(Of String, String)("0", "No se pudo activar la promoción"))
        End Function

        'POST api/promotions/disable/1978/code/PR04
        <Route("disable/{hotelId:Int}/code/{code}"), HttpPost>
        Public Function DisablePromotion(hotelId As Integer, code As String) As Net.Http.HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
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

                    ExecuteServicesDelete(info.Hotel, info.Empresa, isEnabledGoogleRequest, isEnabledSendingRatesAPICache, vDayRatesPromotion, vDayRatesPromotionException)

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

    End Class
End Namespace