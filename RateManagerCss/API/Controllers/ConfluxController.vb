Imports System.Data
Imports System.Data.Entity
Imports System.Web.Http
Imports System.Net.Http
Imports System.Xml.Linq
Imports NinjAPI
Imports NinjAPI.Query
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.GoogleSync
Imports APIServices.Models
Imports APIServices.Conflux.Models.User
Imports APIServices.Conflux.Models.User.Response
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Models.Restrictions.Response
Imports APIServices.Conflux.Models.Inventory.Response
Imports APIServices.Conflux.Models.Delete.Response
Imports Portal.Hotel.Common.Data
Imports RateManager.PaginaBase


Namespace API.Controllers
    <RoutePrefix("api/conflux")>
    Public Class ConfluxController
        Inherits ShurikenController

        Dim ConfluxService As ConfluxService = New ConfluxService()


        <Route("hotels"), HttpGet, Queryable>
        Public Function GetHotels() As IQueryable(Of vHotelActives)
            Return ConfluxService.GetHotels()
        End Function

        <Route("delete/permission"), HttpGet>
        Public Function UserHasPermission() As HttpResponseMessage

            Dim user As APIServices.Conflux.Models.Delete.User = ConfluxService.UserHasPermission(API.Helpers.UserDataHelper.GetUserEmail())

            Dim toObject As Object = user

            Return Ok(toObject)
        End Function



        <Route("users/connectivity"), HttpGet, Queryable>
        Public Function GetUserConnectivities() As IQueryable(Of vUsersConnectivity)
            Return ConfluxService.GetUsersConnectivities()
        End Function

        <Route("create/user"), HttpPost>
        Public Function CreateUser(<FromBody> user As User) As HttpResponseMessage

            Dim result As UserReponse = ConfluxService.CreateUser(user)

            If Not result.IsSuccess Then

                Return BadRequest(result.Error)

            End If

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function


        <Route("updaterates/{hotelId:int}"), HttpPost>
        Public Function UpdateRates(ByVal hotelId As Integer, <FromBody> body As APIServices.Conflux.Models.Rates.RatePrice) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim ConfluxServiceSpecial As ConfluxService = New ConfluxService()

            Dim ratesToUpdate As RateResponse = Nothing

            If Utitlities.Hotel.HotelUtilitie.IsEnableGoogleRequest(hotelId) Then

                ConfluxServiceSpecial.ConfluxSendRatesToGoogle = True

                Dim ratesMessages As RatesMessages = GetMessages(hotelId, info.Empresa, body, ConfluxServiceSpecial)

                Dim result As RatesReponse = ConfluxService.UpdateRates(ratesMessages, Utitlities.Hotel.HotelUtilitie.ENDPOINT, Utitlities.Hotel.HotelUtilitie.ENDPOINTDELETE, True)

                ratesToUpdate = result.RateResponseList(0)

                If Not ratesToUpdate.IsSuccess Then
                    Log("Error Sincronizar Tarifas Conflux con el hotel: ", ratesToUpdate.Xml, hotelId, String.Empty)
                    Return BadRequest(ratesToUpdate.Error)
                End If

                LogRates(hotelId, "Conflux", result)
            End If

            Dim ratesToUpdateAPICache As RateResponse = Nothing

            ''API CACHE
            If Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId) Then

                ConfluxServiceSpecial.ConfluxSendRatesToGoogle = False

                Dim ratesMessages As RatesMessages = GetMessages(hotelId, info.Empresa, body, ConfluxServiceSpecial)

                Dim resultAPICache As RatesReponse = ConfluxService.UpdateRatesPatch(ratesMessages, Utitlities.Hotel.HotelUtilitie.ENDPOINTAPIV2, Utitlities.Hotel.HotelUtilitie.ENDPOINTAPIDELETE, True)

                ratesToUpdateAPICache = resultAPICache.RateResponseList(0)

                If Not ratesToUpdateAPICache.IsSuccess Then
                    Log("Error Sincronizar Tarifas APi Cache con el hotel: ", ratesToUpdateAPICache.Xml, hotelId, String.Empty)
                    Return BadRequest(ratesToUpdateAPICache.Error)
                End If

                LogRates(hotelId, "APICache", resultAPICache)
            End If

            Dim toObject As Object = Nothing

            If ratesToUpdate IsNot Nothing Then
                toObject = ratesToUpdate
            End If

            If ratesToUpdateAPICache IsNot Nothing And ratesToUpdate Is Nothing Then
                toObject = ratesToUpdateAPICache
            End If


            Return Ok(toObject)

        End Function

        <Route("inventory/{hotelId:int}"), HttpPost>
        Public Function UpdateInventory(ByVal hotelId As Integer, <FromBody> body As APIServices.Conflux.Models.Inventory.Inventory) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim result As InventoryResponse = Nothing

            If Utitlities.Hotel.HotelUtilitie.IsEnableGoogleRequest(hotelId) Then
                Dim soapRequests As List(Of XDocument) = GetMessages(hotelId, info.Empresa, PortalCulture.GetIDCulture, body, "Conflux")

                result = ConfluxService.UpdateInventory(soapRequests, Utitlities.Hotel.HotelUtilitie.ENDPOINTINVENTORY)

                If Not result.IsSuccess Then
                    Log("Error Sincronizar Inventario con el hotel: ", result.Xml, hotelId, String.Empty)
                    Return BadRequest(result.Error)
                End If

                LogInventory(hotelId, "Conflux", result)
            End If

            Dim resultAPI As InventoryResponse = Nothing

            If Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId) Then
                Dim soapRequestsAPI As List(Of XDocument) = GetMessages(hotelId, info.Empresa, PortalCulture.GetIDCulture, body, "APICache")

                resultAPI = ConfluxService.UpdateInventoryPatch(soapRequestsAPI, Utitlities.Hotel.HotelUtilitie.ENDPOINTAPICLOSUREV2)

                LogInventory(hotelId, "APICache", resultAPI)

            End If

            Dim toObject As Object = Nothing

            If result IsNot Nothing Then
                toObject = result
            End If

            If resultAPI IsNot Nothing And result Is Nothing Then
                toObject = resultAPI
            End If

            Return Ok(toObject)
        End Function


        <Route("updateclosure/{hotelId:int}"), HttpPost>
        Public Function UpdateClosure(ByVal hotelId As Integer, <FromBody> closure As APIServices.Conflux.Models.Closure.Closure) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim priorityRequests As List(Of List(Of System.Xml.Linq.XDocument)) = ConfluxService.GetClosureMessagesV2(hotelId, info.Empresa, closure)

            Dim result As RestrictionResponse = Nothing
            Dim resultAPICache As RestrictionResponse = Nothing

            Dim isEnabledGoogle As Boolean = Utitlities.Hotel.HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledAPICache As Boolean = Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            If isEnabledGoogle Then

                result = ConfluxService.UpdateRestriction(Utitlities.Hotel.HotelUtilitie.ENDPOINTCLOSURE, priorityRequests)

                If Not result.IsSuccess Then
                    Log("Error Sincronizar Restricciones con el hotel: ", result.Xml, hotelId)
                    Return BadRequest(result.Error)
                End If

                LogClosure(hotelId, "Conflux", result.Restrictions)

            End If

            If isEnabledAPICache Then

                Dim priorityRequestsAPICache As List(Of List(Of System.Xml.Linq.XDocument)) = ConfluxService.GetClosureMessagesV2(hotelId, hotelId, closure)

                resultAPICache = ConfluxService.UpdateRestrictionPatch(Utitlities.Hotel.HotelUtilitie.ENDPOINTAPICLOSUREV2, priorityRequestsAPICache)

                If Not resultAPICache.IsSuccess Then
                    Log("Error Sincronizar Restricciones APICache con el hotel: ", result.Xml, hotelId)
                    Return BadRequest(resultAPICache.Error)
                Else
                    LogClosure(hotelId, "APICache", resultAPICache.Restrictions)
                End If

            End If

            Dim ratesClosureRequest As List(Of System.Xml.Linq.XDocument) = ConfluxService.GetClosureRatesMessages(hotelId, info.Empresa, closure)

            If isEnabledGoogle Then

                Dim resultRateClosure As RestrictionResponse = ConfluxService.UpdateRestriction(Utitlities.Hotel.HotelUtilitie.ENDPOINTCLOSURE, ratesClosureRequest)

                If Not resultRateClosure.IsSuccess Then
                    Log("Error Sincronizar Restricciones LockRate(Cierre de tarifa) con el hotel: ", resultRateClosure.Xml, hotelId)
                Else
                    LogClosure(hotelId, "Conflux", resultRateClosure.Restrictions)
                End If

            End If

            If isEnabledAPICache Then
                Dim ratesClosureRequestAPICache As List(Of System.Xml.Linq.XDocument) = ConfluxService.GetClosureRatesMessages(hotelId, hotelId, closure)

                Dim resultAPICacheRatesClosure As RestrictionResponse = ConfluxService.UpdateRestrictionPatch(Utitlities.Hotel.HotelUtilitie.ENDPOINTAPICLOSUREV2, ratesClosureRequestAPICache)

                If Not resultAPICacheRatesClosure.IsSuccess Then
                    Log("Error Sincronizar Restricciones APICache LockRate(Cierre de tarifa) con el hotel: ", resultAPICacheRatesClosure.Xml, hotelId)
                Else
                    LogClosure(hotelId, "APICache", resultAPICacheRatesClosure.Restrictions)
                End If
            End If

            Dim toObject As Object = Nothing

            If result IsNot Nothing Then
                toObject = result
            End If

            If resultAPICache IsNot Nothing And result Is Nothing Then
                toObject = resultAPICache
            End If

            Return Ok(toObject)

        End Function

        <Route("updaterestrictions/{hotelId:int}"), HttpPost>
        Public Function UpdateRestrictions(ByVal hotelId As Integer, <FromBody> restriction As APIServices.Conflux.Models.Restrictions.Restricion) As HttpResponseMessage
            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim priorityRequests As List(Of List(Of System.Xml.Linq.XDocument)) = ConfluxService.GetRestricionsMessages(hotelId, info.Empresa, restriction)

            Dim isEnabledGoogle As Boolean = Utitlities.Hotel.HotelUtilitie.IsEnableGoogleRequest(hotelId)
            Dim isEnabledAPICache As Boolean = Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId)

            Dim result As RestrictionResponseV2 = Nothing
            Dim resultAPICache As RestrictionResponseV2 = Nothing

            If isEnabledGoogle Then

                result = ConfluxService.UpdateRestrictionNoClosure(Utitlities.Hotel.HotelUtilitie.ENDPOINTRESTRICTIONS, priorityRequests)

                If Not result.IsSuccess Then
                    Log("Error Sincronizar Restricciones con el hotel: ", result.Xml, hotelId)
                    Return BadRequest(result.Error)
                End If

                LogRestrictions(hotelId, "Conflux", result.Restrictions)

            End If

            If isEnabledAPICache Then

                Dim priorityRequestsAPICache As List(Of List(Of System.Xml.Linq.XDocument)) = ConfluxService.GetRestricionsMessages(hotelId, hotelId, restriction)

                resultAPICache = ConfluxService.UpdateRestrictionNoClosure(Utitlities.Hotel.HotelUtilitie.ENDPOINTRESTRICTIONS, priorityRequestsAPICache)

                If Not resultAPICache.IsSuccess Then
                    Log("Error Sincronizar Restricciones APICache con el hotel: ", result.Xml, hotelId)
                    Return BadRequest(resultAPICache.Error)
                Else
                    LogRestrictions(hotelId, "APICache", resultAPICache.Restrictions)
                End If

            End If


            Dim toObject As Object = Nothing

            If result IsNot Nothing Then
                toObject = result
            End If

            Return Ok(toObject)

        End Function

        <Route("deleterates/{hotelId:int}"), HttpPost>
        Public Function DeleteRates(ByVal hotelId As Integer, <FromBody> delete As APIServices.Conflux.Models.Delete.Delete) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            ' Capturar XML de tarifas ANTES de eliminar para poder visualizarlas en el log
            Dim deletedRatesXml As String = String.Empty
            Try
                deletedRatesXml = BuildDeletedRatesXml(hotelId, delete)
            Catch
                ' Si falla la captura no interrumpimos la eliminación
            End Try

            Dim result As DeleteResponse = Nothing

            If Utitlities.Hotel.HotelUtilitie.IsEnableGoogleRequest(hotelId) Then
                Dim messages As OTA.Models.Rates.RateAmountMessages = ConfluxService.GetDeleteMessages(hotelId, info.Empresa, delete)

                Dim soapRequests As List(Of XDocument) = ConfluxService.GetSoapRequests(messages)

                result = ConfluxService.UpdateDelete(soapRequests, Utitlities.Hotel.HotelUtilitie.ENDPOINTDELETE)

                If Not result.IsSuccess Then
                    Log("Error Eliminar Tarifas con el hotel: ", result.Xml, hotelId, String.Empty)
                    Return BadRequest(result.Error)
                End If

                LogDelete(hotelId, "Conflux", result, deletedRatesXml)
            End If

            Dim resultAPICache As DeleteResponse = Nothing

            'API CACHE
            If Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId) Then

                Dim messages As OTA.Models.Rates.RateAmountMessages = ConfluxService.GetDeleteMessages(hotelId, hotelId, delete)

                Dim soapRequests As List(Of XDocument) = ConfluxService.GetSoapRequests(messages)

                resultAPICache = ConfluxService.UpdateDelete(soapRequests, Utitlities.Hotel.HotelUtilitie.ENDPOINTAPIDELETE)

                LogDelete(hotelId, "APICache", resultAPICache, deletedRatesXml)
            End If

            Dim toObject As Object = Nothing

            If result IsNot Nothing Then
                toObject = result
            End If

            If resultAPICache IsNot Nothing And result Is Nothing Then
                toObject = resultAPICache
            End If

            Return Ok(toObject)
        End Function

        <Route("synchistory/{hotelId:int}"), HttpGet>
        Public Function GetSyncHistory(ByVal hotelId As Integer, Optional ByVal status As String = Nothing, Optional ByVal tipoOperacion As String = Nothing, Optional ByVal pageSize As Integer = 50, Optional ByVal page As Integer = 1) As HttpResponseMessage
            Dim result As SyncHistoryResult = GoogleSyncAuditService.GetHistory(hotelId, pageSize, page, status, tipoOperacion)
            Dim toObject As Object = result
            Return Ok(toObject)
        End Function

        Private Sub Log(ByVal note As String, ByVal xml As String, ByVal hotelId As Integer, Optional ByVal requestXMl As String = "")
            With (New PaginaBase)
                .guardalog("/rate-manager-ui/dist/channel-rates-update.aspx", acciones.Sincronizar, note & hotelId, "", requestXMl, xml, hotelId:=hotelId)
            End With
        End Sub

        Private Sub LogRates(ByVal hotelId As Integer, ByVal serviceToSent As String, ByVal result As RatesReponse)

            Dim ratesToUpdate As RateResponse = result.RateResponseList(0)
            Dim ratesToDelete As RateResponse = result.RateResponseList(1)
            Dim ratesToUpdateExceptions As RateResponse = result.RateResponseList(2)

            Dim index As Integer = 1

            If Not ratesToUpdate.IsSuccess Then
                Dim note As String = String.Format("Error Sincronizar Tarifas {1} con el hotel: {0}", hotelId, serviceToSent)
                Log(note, ratesToUpdate.Xml, hotelId, String.Empty)
            Else

                For Each request As APIServices.Conflux.Models.Rates.Response.Rate In ratesToUpdate.Rates
                    Dim note As String = String.Format("Sincronizar request numero {0} Tarifas {1} con el hotel: ", (index), serviceToSent)
                    Log(note, request.Xml, hotelId, request.XmlRequest)
                    index += 1
                Next

            End If

            If ratesToUpdateExceptions IsNot Nothing Then
                If Not ratesToUpdateExceptions.IsSuccess Then
                    Dim note As String = String.Format("Error Sincronizar Tarifas Excepciones {1} con el hotel: {0}", hotelId, serviceToSent)
                    Log(note, ratesToUpdateExceptions.Xml, hotelId, String.Empty)
                Else
                    index = 1

                    For Each request As APIServices.Conflux.Models.Rates.Response.Rate In ratesToUpdateExceptions.Rates
                        Dim note As String = String.Format("Sincronizar excepciones request numero {0} Tarifas {1} con el hotel: ", (index), serviceToSent)
                        Log(note, request.Xml, hotelId, request.XmlRequest)
                        index += 1
                    Next
                End If
            End If

            'Log Delete
            If ratesToDelete IsNot Nothing Then
                Dim noteDelete As String = String.Format("Eliminar Tarifas {1} con el hotel: {0}", hotelId, serviceToSent)
                Log(noteDelete, ratesToDelete.Xml, hotelId, ratesToDelete.RequestXML)
            End If

        End Sub

        Private Sub LogDelete(ByVal hotelId As Integer, ByVal serviceToSent As String, ByVal result As DeleteResponse, Optional ByVal deletedRatesXml As String = "")
            Dim index As Integer = 1

            If Not result.IsSuccess Then
                Dim note As String = String.Format("Error Eliminar Tarifas {1} con el hotel: {0}", hotelId, serviceToSent)
                Log(note, result.Xml, hotelId, String.Empty)
            Else

                ' Un log "maestro" con el detalle de las tarifas eliminadas (visible en LogDetalle via Tarifas.xslt)
                If Not String.IsNullOrEmpty(deletedRatesXml) Then
                    Dim noteMaster As String = String.Format("Se eliminaron tarifas en {0} del hotel: {1}", serviceToSent, hotelId)
                    With (New PaginaBase)
                        .guardalog("/rate-manager-ui/dist/channel-rates-update.aspx", acciones.Eliminar, noteMaster, "", deletedRatesXml, String.Empty, hotelId:=hotelId)
                    End With
                End If

                For Each request As DeleteHttpResponse In result.DeleteHttpResponseList
                    Dim note As String = String.Format("Eliminar Tarifas request numero {0} Tarifas {1} con el hotel: ", (index), serviceToSent)
                    Log(note, request.Xml, hotelId, request.XmlRequest)
                    index += 1
                Next

            End If
        End Sub

        ''' <summary>
        ''' Construye un XML &lt;Tarifas&gt;&lt;UpdateRate/&gt;...&lt;/Tarifas&gt; con las tarifas que coinciden
        ''' con los criterios del delete (rate plans + promos + rooms + rango de fechas).
        ''' Se invoca ANTES de la eliminación para que el log conserve el detalle.
        ''' </summary>
        Private Function BuildDeletedRatesXml(ByVal hotelId As Integer, ByVal delete As APIServices.Conflux.Models.Delete.Delete) As String
            Try
                ' 1) Resolver los rate plans "efectivos" que se borrarán (considerando promos concatenadas)
                Dim effectivePlans As New List(Of String)

                Dim ratePlans As List(Of String) = If(delete.RatePlansList Is Nothing, New List(Of String)(), delete.RatePlansList.ToList())
                Dim promotions As List(Of String) = If(delete.PromosList Is Nothing, New List(Of String)(), delete.PromosList.ToList())

                If delete.EnableRatePlans Then
                    For Each rp As String In ratePlans
                        If Not String.IsNullOrEmpty(rp) AndAlso rp <> "0" Then
                            effectivePlans.Add(rp)
                        End If
                    Next
                End If

                If delete.EnablePromotions Then
                    For Each promo As String In promotions
                        If String.IsNullOrEmpty(promo) OrElse promo = "0" Then Continue For
                        For Each rp As String In ratePlans
                            If String.IsNullOrEmpty(rp) OrElse rp = "0" Then Continue For
                            effectivePlans.Add(promo & rp)
                        Next
                    Next
                End If

                If effectivePlans.Count = 0 Then Return String.Empty

                ' 2) Preparar filtro de habitaciones y fechas
                Dim roomIds As Integer() = If(delete.RoomsList, New Integer() {})
                Dim hasAllRooms As Boolean = (roomIds.Length = 1 AndAlso roomIds(0) = 0) OrElse roomIds.Length = 0
                Dim startDate As DateTime = If(delete.StartDate.HasValue, delete.StartDate.Value.Date, DateTime.MinValue)
                Dim endDate As DateTime = If(delete.EndDate.HasValue, delete.EndDate.Value.Date, DateTime.MaxValue)

                Dim rates As List(Of Tarifas) = Nothing

                ' 3) Consultar las tarifas que coinciden con el filtro
                Using dbContext As New OzHotelesEntities()
                    Dim q As IQueryable(Of Tarifas) = dbContext.Tarifas.AsNoTracking()
                    q = q.Where(Function(t) effectivePlans.Contains(t.idrateplan))

                    If Not hasAllRooms Then
                        q = q.Where(Function(t) roomIds.Contains(t.idTipoHabitacion_Hotel))
                    End If

                    If delete.StartDate.HasValue Then
                        q = q.Where(Function(t) t.FechaFinaliza >= startDate)
                    End If
                    If delete.EndDate.HasValue Then
                        q = q.Where(Function(t) t.FechaInicia <= endDate)
                    End If

                    rates = q.ToList()
                End Using

                If rates Is Nothing OrElse rates.Count = 0 Then Return String.Empty

                ' 4) Construir XML con FaresData (misma estructura que CreateXml en RatesController)
                Dim datFare As New FaresData()
                With datFare.Tables(FaresData.FARES_TABLE)
                    For Each rate As Tarifas In rates
                        Dim rowFare As DataRow = .NewRow()
                        rowFare(FaresData.PKIDFARES_FIELD) = rate.idTarifa
                        rowFare(FaresData.HOTELROOMTYPEID_FIELD) = rate.idTipoHabitacion_Hotel
                        rowFare(FaresData.STARTDATE_FIELD) = rate.FechaInicia
                        rowFare(FaresData.ENDDATE_FIELD) = rate.FechaFinaliza
                        rowFare(FaresData.PRICE_FIELD) = rate.Precio
                        rowFare(FaresData.NINIORATE) = If(rate.NiniosRate.HasValue, rate.NiniosRate.Value, 0D)
                        rowFare(FaresData.RATEENPRICE_FIELD) = If(rate.PrecioAdolescente.HasValue, rate.PrecioAdolescente.Value, 0D)
                        rowFare(FaresData.EXTRAADULTPRICE_FIELD) = rate.PrecioExtraAdulto
                        rowFare(FaresData.EXTRACHILDPRICE_FIELD) = rate.PrecioExtraNinio
                        rowFare(FaresData.EXTRATEENPRICE_FIELD) = If(rate.PrecioAdolescenteExtra.HasValue, rate.PrecioAdolescenteExtra.Value, 0D)
                        rowFare(FaresData.PRICENR_FIELD) = If(rate.PrecioNR.HasValue, rate.PrecioNR.Value, 0D)
                        rowFare(FaresData.NINIORATENR) = If(rate.NiniosRateNR.HasValue, rate.NiniosRateNR.Value, 0D)
                        rowFare(FaresData.RATEENPRICENR_FIELD) = If(rate.PrecioAdolescenteNR.HasValue, rate.PrecioAdolescenteNR.Value, 0D)
                        rowFare(FaresData.EXTRAADULTPRICENR_FIELD) = If(rate.PrecioExtraAdultoNR.HasValue, rate.PrecioExtraAdultoNR.Value, 0D)
                        rowFare(FaresData.EXTRACHILDPRICENR_FIELD) = If(rate.PrecioExtraNinioNR.HasValue, rate.PrecioExtraNinioNR.Value, 0D)
                        rowFare(FaresData.EXTRATEENPRICENR_FIELD) = If(rate.PrecioAdolescenteExtraNR.HasValue, rate.PrecioAdolescenteExtraNR.Value, 0D)
                        rowFare(FaresData.RATETYPE_FIELD) = If(rate.TipoTarifa, String.Empty)
                        rowFare(FaresData.RATECODE_FIELD) = If(rate.CodigoTarifa, String.Empty)
                        rowFare(FaresData.EXCEPTION_FIELD) = If(rate.Excepciones, String.Empty)
                        rowFare(FaresData.NOARRIVOS_FIELD) = If(rate.NoArrivos, String.Empty)
                        rowFare(FaresData.IDRATEPLAN_FIELD) = If(rate.idrateplan, String.Empty)
                        rowFare(FaresData.RULESDEFAULT) = If(rate.RateRulesDefault.HasValue, rate.RateRulesDefault.Value, True)
                        rowFare(FaresData.IDDICCDESCPROM_FIELD) = If(rate.idDiccPromoDesc.HasValue, rate.idDiccPromoDesc.Value, 0)
                        .Rows.Add(rowFare)
                    Next
                End With

                ' Columna auxiliar usada por el XSLT para mostrar el plan/habitación
                datFare.Tables(0).Columns.Add("Descr_rateplan")
                For i As Integer = 0 To rates.Count - 1
                    datFare.Tables(0).Rows(i)("Descr_rateplan") = String.Format("Hab {0} · {1}", rates(i).idTipoHabitacion_Hotel, rates(i).idrateplan)
                Next

                Return Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRate", datFare)
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Private Sub LogClosure(ByVal hotelId As Integer, ByVal service As String, ByVal restrictionList As List(Of Restriction))
            For Each restriction As Restriction In restrictionList
                Select Case restriction.Type
                    Case RestrictionEnum.LockGral
                        Dim index As Integer = 0
                        While index < restriction.Xml.Count()
                            Dim note As String = String.Format("Sincronizar request numero {0} LockGral {1} con el hotel: ", (index + 1), service)
                            Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            index += 1
                        End While
                    Case RestrictionEnum.LockRatePlan
                        Dim index As Integer = 0
                        While index < restriction.Xml.Count()
                            Dim note As String = String.Format("Sincronizar request numero {0} LockRatePlan {1} con el hotel: ", (index + 1), service)
                            Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            index += 1
                        End While
                    Case RestrictionEnum.LockRoomType
                        Dim index As Integer = 0
                        While index < restriction.Xml.Count()
                            Dim note As String = String.Format("Sincronizar request numero {0} LockRoomtype {1} con el hotel: ", (index + 1), service)
                            Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            index += 1
                        End While
                    Case RestrictionEnum.LockRate
                        Dim index As Integer = 0
                        While index < restriction.Xml.Count()
                            Dim note As String = String.Format("Sincronizar request numero {0} LockRate(Cierre de Tarifa) {1} con el hotel: ", (index + 1), service)
                            Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            index += 1
                        End While
                End Select
            Next
        End Sub

        Private Sub LogRestrictions(ByVal hotelId As Integer, ByVal service As String, ByVal restrictionList As List(Of RestrictionV2))
            For Each restriction As RestrictionV2 In restrictionList
                Dim index As Integer = 0
                While index < restriction.Xml.Count()
                    Dim note As String = String.Format("Sincronizar request numero {0} Restriccion {1} con el hotel: ", (index + 1), service)
                    Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                    index += 1
                End While
            Next
        End Sub

        Private Sub LogInventory(ByVal hotelId As Integer, ByVal serviceToSent As String, ByVal result As InventoryResponse)
            Dim index As Integer = 1

            If Not result.IsSuccess Then
                Dim note As String = String.Format("Error Sincronizar Inventario {1} con el hotel: {0}", hotelId, serviceToSent)
                Log(note, result.Xml, hotelId, String.Empty)
            Else

                For Each request As InventoryHttpResponse In result.InventoryHttpResponseList
                    Dim note As String = String.Format("Sincronizar request numero {0} Inventario {1} con el hotel: ", (index), serviceToSent)
                    Log(note, request.Xml, hotelId, request.XmlRequest)
                    index += 1
                Next

            End If

        End Sub

        Private Function GetMessages(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal ratePrice As APIServices.Conflux.Models.Rates.RatePrice, ByRef ConfluxServiceSpecial As ConfluxService) As RatesMessages

            Dim ratesMessages As RatesMessages = Nothing


            If ((ratePrice.RatePlansList.Length = 1 And ratePrice.RatePlansList(0) = "0") And (ratePrice.RoomsList.Length = 1 And ratePrice.RoomsList(0) = 0)) Then

                'Todos los planes con todas las habitaciones
                ratesMessages = ConfluxServiceSpecial.GetRateMessages(hotelId, companyId, Nothing, Nothing, ratePrice.StartDate.Value.Date, ratePrice.EndDate.Value.Date)

            ElseIf ((ratePrice.RatePlansList.Length = 1 And ratePrice.RatePlansList(0) = "0") And ((ratePrice.RoomsList.Length = 1 And ratePrice.RoomsList(0) <> 0) Or ratePrice.RoomsList.Length > 1)) Then
                'Todos los planes con habitaciones seleccionadas
                ratesMessages = New RatesMessages
                RatesAllRatePlans(companyId, hotelId, ratePrice, ratesMessages, ConfluxServiceSpecial)
            ElseIf ((ratePrice.RoomsList.Length = 1 And ratePrice.RoomsList(0) = 0) And ((ratePrice.RatePlansList.Length = 1 And ratePrice.RatePlansList(0) <> "0") Or ratePrice.RatePlansList.Length > 1)) Then
                'Todas las habitaciones con planes seleccionados
                ratesMessages = New RatesMessages
                RatesAllRooms(companyId, hotelId, ratePrice, ratesMessages, ConfluxServiceSpecial)
            Else
                'Planes seleccionados con habitaciones seleccionadas
                ratesMessages = New RatesMessages
                RatesRatePlansRooms(companyId, hotelId, ratePrice, ratesMessages, ConfluxServiceSpecial)
            End If

            Return ratesMessages

        End Function

        Public Function GetMessages(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal lang As Integer, ByVal inventory As APIServices.Conflux.Models.Inventory.Inventory, ByVal service As String) As List(Of XDocument)

            Dim document As List(Of XDocument) = Nothing

            If inventory.RoomsList.Length = 1 And inventory.RoomsList(0) = "0" Then
                'Todas las habitaciones
                Dim roomsIdList As Integer() = ConfluxService.LoadRoomsByIdHotel(hotelId, lang)
                Dim inventoryData As RoomsInventoryData = ConfluxService.GetInventoryData(roomsIdList, inventory.StartDate, inventory.EndDate)

                If service = "Conflux" Then
                    document = GetInventoryXml(companyId, inventory.Days, inventoryData)
                ElseIf service = "APICache" Then
                    document = GetInventoryXml(hotelId, inventory.Days, inventoryData)
                End If

            Else
                Dim inventoryData As RoomsInventoryData = ConfluxService.GetInventoryData(inventory.RoomsList, inventory.StartDate, inventory.EndDate)

                If service = "Conflux" Then
                    document = GetInventoryXml(companyId, inventory.Days, inventoryData)
                ElseIf service = "APICache" Then
                    document = GetInventoryXml(hotelId, inventory.Days, inventoryData)
                End If

            End If

            Return document
        End Function



        Private Sub RatesAllRatePlans(ByVal companyId As Integer, ByVal hotelId As Integer, ByVal ratePrice As APIServices.Conflux.Models.Rates.RatePrice, ByRef ratesMessages As RatesMessages, ByRef ConfluxServiceSpecial As ConfluxService)

            Dim rateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
            Dim deleteRateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
            Dim rateAmountMessagesExceptions As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()

            rateAmountMessages.HotelCode = companyId
            deleteRateAmountMessages.HotelCode = companyId
            rateAmountMessagesExceptions.HotelCode = companyId

            rateAmountMessages.HotelCodeV2 = hotelId
            deleteRateAmountMessages.HotelCodeV2 = hotelId
            rateAmountMessagesExceptions.HotelCodeV2 = hotelId


            rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
            deleteRateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
            rateAmountMessagesExceptions.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            ' //0 tarifas, 1: borrar, 2: tarifas excepciones

            'Todos los planes, habitaciones seleccionadas
            For Each roomId As Integer In ratePrice.RoomsList

                Dim ratesMessagesTemp As RatesMessages = ConfluxServiceSpecial.GetRateMessages(hotelId, companyId, Nothing, roomId, ratePrice.StartDate.Value.Date, ratePrice.EndDate.Value.Date)

                Dim rates As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(0).RateAmountMessagesList
                Dim delete As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(1).RateAmountMessagesList
                Dim exceptions As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(2).RateAmountMessagesList

                rateAmountMessages.RateAmountMessagesList.AddRange(rates)
                deleteRateAmountMessages.RateAmountMessagesList.AddRange(delete)
                rateAmountMessagesExceptions.RateAmountMessagesList.AddRange(exceptions)

            Next

            ratesMessages.RateAmountMessagesList.Add(rateAmountMessages)
            ratesMessages.RateAmountMessagesList.Add(deleteRateAmountMessages)
            ratesMessages.RateAmountMessagesList.Add(rateAmountMessagesExceptions)

        End Sub

        Private Sub RatesAllRooms(ByVal companyId As Integer, ByVal hotelId As Integer, ByVal ratePrice As APIServices.Conflux.Models.Rates.RatePrice, ByRef ratesMessages As RatesMessages, ByRef ConfluxServiceSpecial As ConfluxService)

            Dim rateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
            Dim deleteRateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
            Dim rateAmountMessagesExceptions As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()

            rateAmountMessages.HotelCode = companyId
            deleteRateAmountMessages.HotelCode = companyId
            rateAmountMessagesExceptions.HotelCode = companyId

            rateAmountMessages.HotelCodeV2 = hotelId
            deleteRateAmountMessages.HotelCodeV2 = hotelId
            rateAmountMessagesExceptions.HotelCodeV2 = hotelId

            rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
            deleteRateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
            rateAmountMessagesExceptions.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            ' //0 tarifas, 1: borrar, 2: tarifas excepciones

            'Todas las habitaciones , planes seleccionados
            For Each rateplanId As String In ratePrice.RatePlansList

                Dim ratesMessagesTemp As RatesMessages = ConfluxServiceSpecial.GetRateMessages(hotelId, companyId, rateplanId, Nothing, ratePrice.StartDate.Value.Date, ratePrice.EndDate.Value.Date)

                Dim rates As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(0).RateAmountMessagesList
                Dim delete As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(1).RateAmountMessagesList
                Dim exceptions As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(2).RateAmountMessagesList

                rateAmountMessages.RateAmountMessagesList.AddRange(rates)
                deleteRateAmountMessages.RateAmountMessagesList.AddRange(delete)
                rateAmountMessagesExceptions.RateAmountMessagesList.AddRange(exceptions)

            Next

            ratesMessages.RateAmountMessagesList.Add(rateAmountMessages)
            ratesMessages.RateAmountMessagesList.Add(deleteRateAmountMessages)
            ratesMessages.RateAmountMessagesList.Add(rateAmountMessagesExceptions)

        End Sub

        Private Sub RatesRatePlansRooms(ByVal companyId As Integer, ByVal hotelId As Integer, ByVal ratePrice As APIServices.Conflux.Models.Rates.RatePrice, ByRef ratesMessages As RatesMessages, ByRef ConfluxServiceSpecial As ConfluxService)

            Dim rateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
            Dim deleteRateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
            Dim rateAmountMessagesExceptions As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()

            rateAmountMessages.HotelCode = companyId
            deleteRateAmountMessages.HotelCode = companyId
            rateAmountMessagesExceptions.HotelCode = companyId

            rateAmountMessages.HotelCodeV2 = hotelId
            deleteRateAmountMessages.HotelCodeV2 = hotelId
            rateAmountMessagesExceptions.HotelCodeV2 = hotelId

            rateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
            deleteRateAmountMessages.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)
            rateAmountMessagesExceptions.RateAmountMessagesList = New List(Of OTA.Models.Rates.RateAmountMessage)

            'Planes y Habitaciones seleccionadas

            ' //0 tarifas, 1: borrar, 2: tarifas excepciones
            For Each roomId As Integer In ratePrice.RoomsList
                For Each rateplanId As String In ratePrice.RatePlansList

                    Dim ratesMessagesTemp As RatesMessages = ConfluxServiceSpecial.GetRateMessages(hotelId, companyId, rateplanId, roomId, ratePrice.StartDate.Value.Date, ratePrice.EndDate.Value.Date)

                    Dim rates As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(0).RateAmountMessagesList
                    Dim delete As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(1).RateAmountMessagesList
                    Dim exceptions As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(2).RateAmountMessagesList

                    rateAmountMessages.RateAmountMessagesList.AddRange(rates)
                    deleteRateAmountMessages.RateAmountMessagesList.AddRange(delete)
                    rateAmountMessagesExceptions.RateAmountMessagesList.AddRange(exceptions)
                Next
            Next

            ratesMessages.RateAmountMessagesList.Add(rateAmountMessages)
            ratesMessages.RateAmountMessagesList.Add(deleteRateAmountMessages)
            ratesMessages.RateAmountMessagesList.Add(rateAmountMessagesExceptions)
        End Sub

        Private Function GetInventoryXml(ByVal companyId As Integer, ByVal days As Boolean(), ByVal inventoryData As RoomsInventoryData) As List(Of XDocument)

            Dim soapRequestList As List(Of XDocument) = New List(Of XDocument)

            Dim limitMessages As Integer = 50

            Dim Index As Integer = 0
            Dim totalMessages = inventoryData.Tables(0).Rows.Count - 1

            Dim AvailStatusMessage(totalMessages) As WsConnectWcf.AvailStatusMessageType 'Array tiene tosos los messages 4

            For Each dr As DataRow In inventoryData.Tables(0).Rows
                AvailStatusMessage(Index) = New WsConnectWcf.AvailStatusMessageType

                Dim StatusApplicationControl As New WsConnectWcf.StatusApplicationControlType

                AvailStatusMessage(Index).BookingLimit = dr(RoomsInventoryData.FLD_NUMBER_AVAILABILITY)
                StatusApplicationControl.InvTypeCode = dr("RoomCode")
                StatusApplicationControl.Start = CDate(dr(RoomsInventoryData.FLD_STARTDATE)).ToString("yyyy-MM-dd").Replace("-", "")
                StatusApplicationControl.End = CDate(dr(RoomsInventoryData.FLD_ENDDATE)).ToString("yyyy-MM-dd").Replace("-", "")

                If Not days(0) Or Not days(1) Or Not days(2) Or Not days(3) Or Not days(4) Or Not days(5) Or Not days(6) Then

                    StatusApplicationControl.Mon = days(1)
                    StatusApplicationControl.Tue = days(2)
                    StatusApplicationControl.Weds = days(3)
                    StatusApplicationControl.Thur = days(4)
                    StatusApplicationControl.Fri = days(5)
                    StatusApplicationControl.Sat = days(6)
                    StatusApplicationControl.Sun = days(0)

                    StatusApplicationControl.MonSpecified = True
                    StatusApplicationControl.WedsSpecified = True
                    StatusApplicationControl.ThurSpecified = True
                    StatusApplicationControl.TueSpecified = True
                    StatusApplicationControl.SatSpecified = True
                    StatusApplicationControl.SunSpecified = True
                    StatusApplicationControl.FriSpecified = True
                End If

                AvailStatusMessage(Index).StatusApplicationControl = StatusApplicationControl
                Index += 1
            Next
            'ya tengo todos los mensajes

            Dim messagesAdded As Integer = 0
            Dim _AvailStatusMessage As List(Of WsConnectWcf.AvailStatusMessageType) = Nothing 'Mensajes que lleva el request

            'Dividir mensajes
            For i As Integer = 0 To AvailStatusMessage.Length - 1

                If messagesAdded = 0 Then
                    _AvailStatusMessage = New List(Of WsConnectWcf.AvailStatusMessageType)()
                End If

                If messagesAdded < limitMessages Then
                    _AvailStatusMessage.Add(AvailStatusMessage(i))
                    messagesAdded = messagesAdded + 1
                End If

                'Hacer Request
                If messagesAdded = limitMessages Then

                    Dim RQ As New WsConnectWcf.OTA_HotelAvailNotifRQ
                    Dim POS(0) As WsConnectWcf.SourceType
                    POS(0) = New WsConnectWcf.SourceType
                    Dim RequestorID As New WsConnectWcf.SourceTypeRequestorID
                    Dim AvailStatusMessages As New WsConnectWcf.OTA_HotelAvailNotifRQAvailStatusMessages

                    RQ.Version = 1
                    RequestorID.Type = "22"
                    RequestorID.ID = "IPRM"
                    Dim myuuid As Guid = Guid.NewGuid()
                    RQ.EchoToken = myuuid.ToString()

                    AvailStatusMessages.HotelCode = companyId.ToString()
                    AvailStatusMessages.AvailStatusMessage = _AvailStatusMessage.ToArray()

                    RQ.POS = POS
                    RQ.AvailStatusMessages = AvailStatusMessages
                    POS(0).RequestorID = RequestorID

                    Dim strRequest As String = New PaginaBase().GetXMLFromObject(RQ)

                    Dim requestXDocument As XDocument = XDocument.Parse(strRequest)

                    Dim xmlRQ As XElement = requestXDocument.Element("OTA_HotelAvailNotifRQ")

                    Dim soapRequest As XDocument = APIServices.Xml.Soap.Soap.CreateSoapRequestXml(xmlRQ)

                    'Agregarlo a la lista de requests

                    soapRequestList.Add(soapRequest)

                    _AvailStatusMessage = Nothing
                    messagesAdded = 0
                End If

            Next

            If _AvailStatusMessage IsNot Nothing Then

                Dim RQ As New WsConnectWcf.OTA_HotelAvailNotifRQ
                Dim POS(0) As WsConnectWcf.SourceType
                POS(0) = New WsConnectWcf.SourceType
                Dim RequestorID As New WsConnectWcf.SourceTypeRequestorID
                Dim AvailStatusMessages As New WsConnectWcf.OTA_HotelAvailNotifRQAvailStatusMessages

                RQ.Version = 1
                RequestorID.Type = "22"
                RequestorID.ID = "IPRM"
                Dim myuuid As Guid = Guid.NewGuid()
                RQ.EchoToken = myuuid.ToString()

                AvailStatusMessages.HotelCode = companyId.ToString()
                AvailStatusMessages.AvailStatusMessage = _AvailStatusMessage.ToArray()

                RQ.POS = POS
                RQ.AvailStatusMessages = AvailStatusMessages
                POS(0).RequestorID = RequestorID

                Dim strRequest As String = New PaginaBase().GetXMLFromObject(RQ)

                Dim requestXDocument As XDocument = XDocument.Parse(strRequest)

                Dim xmlRQ As XElement = requestXDocument.Element("OTA_HotelAvailNotifRQ")

                Dim soapRequest As XDocument = APIServices.Xml.Soap.Soap.CreateSoapRequestXml(xmlRQ)

                'Agregarlo a la lista de requests

                soapRequestList.Add(soapRequest)

            End If

            Return soapRequestList
        End Function


    End Class
End Namespace
