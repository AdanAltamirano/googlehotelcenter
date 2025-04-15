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
        Public Function UpdateRates(ByVal hotelId As Integer) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
            Dim ratesMessages As RatesMessages = ConfluxService.GetRateMessages(hotelId, info.Empresa)

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

    End Class
End Namespace
