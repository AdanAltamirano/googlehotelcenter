Imports System.Web.Http
Imports System.Net.Http
Imports NinjAPI
Imports NinjAPI.Query
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Models
Imports APIServices.Conflux.Models.User
Imports APIServices.Conflux.Models.User.Response
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Models.Restrictions.Response
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

            Dim ratesMessages As RatesMessages = GetMessages(hotelId, info.Empresa, body)

            Dim result As RatesReponse = ConfluxService.UpdateRates(ratesMessages, Utitlities.Hotel.HotelUtilitie.ENDPOINT, Utitlities.Hotel.HotelUtilitie.ENDPOINTDELETE, True)

            Dim ratesToUpdate As RateResponse = result.RateResponseList(0)

            If Not ratesToUpdate.IsSuccess Then

                Log("Error Sincronizar Tarifas Conflux con el hotel: ", ratesToUpdate.Xml, hotelId, String.Empty)

                Return BadRequest(ratesToUpdate.Error)

            End If

            LogRates(hotelId, "Conflux", result)

            ''API CACHE
            If Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId) Then
                Dim resultAPICache As RatesReponse = ConfluxService.UpdateRates(ratesMessages, Utitlities.Hotel.HotelUtilitie.ENDPOINTAPI, Utitlities.Hotel.HotelUtilitie.ENDPOINTAPIDELETE, False)
                LogRates(hotelId, "APICache", result)
            End If

            Dim toObject As Object = ratesToUpdate

            Return Ok(toObject)

        End Function

        <Route("updaterestrictions/{hotelId:int}"), HttpPost>
        Public Function UpdateRestrictions(ByVal hotelId As Integer) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim priorityRequests As List(Of List(Of System.Xml.Linq.XDocument)) = ConfluxService.GetClosureMessages(hotelId, info.Empresa)
            Dim result As RestrictionResponse = ConfluxService.UpdateRestriction(Utitlities.Hotel.HotelUtilitie.ENDPOINTCLOSURE, priorityRequests)

            If Not result.IsSuccess Then
                Log("Error Sincronizar Restricciones con el hotel: ", result.Xml, hotelId)
                Return BadRequest(result.Error)
            End If

            LogClosure(hotelId, "Conflux", result.Restrictions)


            Dim ratesClosureRequest As List(Of System.Xml.Linq.XDocument) = ConfluxService.GetClosureRatesMessages(hotelId, info.Empresa)
            Dim resultRateClosure As RestrictionResponse = ConfluxService.UpdateRestriction(Utitlities.Hotel.HotelUtilitie.ENDPOINTCLOSURE, ratesClosureRequest)

            If Not resultRateClosure.IsSuccess Then
                Log("Error Sincronizar Restricciones LockRate(Cierre de tarifa) con el hotel: ", resultRateClosure.Xml, hotelId)
            Else
                LogClosure(hotelId, "Conflux", resultRateClosure.Restrictions)
            End If

            If Utitlities.Hotel.HotelUtilitie.IsEnableSendRatesAPICache(hotelId) Then
                Dim resultAPICache As RestrictionResponse = ConfluxService.UpdateRestriction(Utitlities.Hotel.HotelUtilitie.ENDPOINTAPICLOSURE, priorityRequests)

                If Not resultAPICache.IsSuccess Then
                    Log("Error Sincronizar Restricciones APICache con el hotel: ", result.Xml, hotelId)
                Else
                    LogClosure(hotelId, "APICache", resultAPICache.Restrictions)
                End If

                Dim resultAPICacheRatesClosure As RestrictionResponse = ConfluxService.UpdateRestriction(Utitlities.Hotel.HotelUtilitie.ENDPOINTAPICLOSURE, ratesClosureRequest)

                If Not resultAPICacheRatesClosure.IsSuccess Then
                    Log("Error Sincronizar Restricciones APICache LockRate(Cierre de tarifa) con el hotel: ", resultAPICacheRatesClosure.Xml, hotelId)
                Else
                    LogClosure(hotelId, "APICache", resultAPICacheRatesClosure.Restrictions)
                End If

            End If

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

        Private Function GetMessages(ByVal hotelId As Integer, ByVal companyId As Integer, ByVal ratePrice As APIServices.Conflux.Models.Rates.RatePrice) As RatesMessages

            Dim ratesMessages As RatesMessages = Nothing

            If ((ratePrice.RatePlansList.Length = 1 And ratePrice.RatePlansList(0) = "0") And (ratePrice.RoomsList.Length = 1 And ratePrice.RoomsList(0) = 0)) Then

                'Todos los planes con todas las habitaciones
                ratesMessages = ConfluxService.GetRateMessages(hotelId, companyId, Nothing, Nothing, ratePrice.StartDate.Value.Date, ratePrice.EndDate.Value.Date)

            ElseIf ((ratePrice.RatePlansList.Length = 1 And ratePrice.RatePlansList(0) = "0") And ((ratePrice.RoomsList.Length = 1 And ratePrice.RoomsList(0) <> 0) Or ratePrice.RoomsList.Length > 1)) Then

                Dim rateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
                Dim deleteRateAmountMessages As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()
                Dim rateAmountMessagesExceptions As OTA.Models.Rates.RateAmountMessages = New OTA.Models.Rates.RateAmountMessages()

                rateAmountMessages.HotelCode = companyId
                deleteRateAmountMessages.HotelCode = companyId
                rateAmountMessagesExceptions.HotelCode = companyId

                ' //0 tarifas, 1: borrar, 2: tarifas excepciones

                'Todos los planes, habitaciones seleccionadas
                For Each roomId As Integer In ratePrice.RoomsList

                    Dim ratesMessagesTemp As RatesMessages = ConfluxService.GetRateMessages(hotelId, companyId, Nothing, roomId, ratePrice.StartDate.Value.Date, ratePrice.EndDate.Value.Date)

                    Dim tarifas As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(0).RateAmountMessagesList
                    Dim borrar As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(1).RateAmountMessagesList
                    Dim excepciones As List(Of OTA.Models.Rates.RateAmountMessage) = ratesMessagesTemp.RateAmountMessagesList(2).RateAmountMessagesList

                    rateAmountMessages.RateAmountMessagesList.AddRange(tarifas)
                    deleteRateAmountMessages.RateAmountMessagesList.AddRange(borrar)
                    rateAmountMessagesExceptions.RateAmountMessagesList.AddRange(excepciones)

                Next

                ratesMessages.RateAmountMessagesList.Add(rateAmountMessages)
                ratesMessages.RateAmountMessagesList.Add(deleteRateAmountMessages)
                ratesMessages.RateAmountMessagesList.Add(rateAmountMessagesExceptions)

            End If

            Return ratesMessages

        End Function


    End Class
End Namespace
