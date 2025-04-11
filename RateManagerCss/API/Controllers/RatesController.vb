Imports System.Xml.Linq
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Common
Imports RateManager.API.Helpers
Imports RateManager.API.Models
Imports RateManager.PaginaBase
Imports RateManager.Utitlities.Hotel
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Parser.Restriction

Namespace API.Controllers
    <RoutePrefix("api/hotels/{HotelId:int}/rates")>
    Public Class RatesController
        Inherits ShurikenController

        Public Service As New RatesService
        Public ConfluxService As New ConfluxService()

        'GET api/hotels/1/rates
        <Route(""), HttpGet>
        Public Function GetByRatePlan(HotelId As Integer, <FromUri> Req As RatesByPlanRQ) As IEnumerable(Of DTO.RatesByRatePlan)
            Return Service.FindGroupedByRatePlan(HotelId, Req.StartDate, Req.EndDate, Req.RoomId, Request.GetLanguageUV())
        End Function

        'GET api/hotels/1/rates/2345/daily/2019-09-01
        <Route("{RateId:int}/daily/{day:datetime}"), HttpGet>
        Public Function GetDayRate(HotelId As Integer, RateId As Integer, day As Date) As DTO.DailyRateDetail
            Return Service.FindDayRateDetail(RateId, day)
        End Function

        'POST api/hotels/1/rates/
        <Route(""), HttpPost>
        Public Function RateAdd(<FromBody> RQ As RateUpdateRQ, HotelId As Integer) As Net.Http.HttpResponseMessage
            RQ.HotelId = HotelId
            'Listado de tarifas que se usan para el Log
            Dim logRates As List(Of Tarifas) = New List(Of Tarifas)()

            Dim serviceRQ As DTO.RateUpdateRQ = MappingRateUpdateRQ(RQ)
            Dim result As KeyValuePair(Of String, String) = Service.AddRate(serviceRQ, logRates)

            If result.Key = 1 Then

                Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                If logRates IsNot Nothing And logRates.Count > 0 Then

                    If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then

                        Dim updatedRates As IEnumerable(Of Tarifas) = logRates.Distinct()

                        For Each rate As Tarifas In updatedRates
                            ExecuteServices(isEnabledGoogleRequest, isEnabledSendingRatesAPICache, RQ.HotelId, rate, info)
                        Next

                    End If
                End If

                Try
                    'Guardar Log
                    If logRates IsNot Nothing And logRates.Count > 0 Then
                        For Each rate As Tarifas In logRates

                            Dim xml As String = CreateXml(serviceRQ.RoomCode, serviceRQ.RoomName, rate)

                            Dim msg As String = "Se creó la tarifa de la habitación " & serviceRQ.RoomCode & " de la fecha " & rate.FechaInicia.ToString("MM/dd/yyyy") & " a la fecha " & rate.FechaFinaliza.ToString("MM/dd/yyyy") & " con el rateplan " & rate.idrateplan

                            Log(RQ.HotelId, acciones.Crear, serviceRQ.RoomCode, rate.FechaInicia, rate.FechaFinaliza, rate.idrateplan, xml, note:=msg)

                        Next
                    End If
                Catch

                End Try

                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/hotels/1/rates/
        <Route("{RateId:int}/daily/{day:datetime}"), HttpPost>
        Public Function RateUpdate(<FromBody> RQ As RateUpdateRQ, HotelId As Integer, RateId As Integer, day As Date) As Net.Http.HttpResponseMessage

            RQ.HotelId = HotelId
            RQ.RateId = RateId
            RQ.StartDate = day
            RQ.EndDate = day

            'Listado de tarifas que se usan para el Log
            Dim logRates As List(Of Tarifas) = New List(Of Tarifas)()

            Dim serviceRQ As DTO.RateUpdateRQ = MappingRateUpdateRQ(RQ)
            Dim result As KeyValuePair(Of String, String) = Service.AddRate(serviceRQ, logRates)

            If result.Key = 1 Then


                Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)
                Dim isEnabledGoogleRequest As Boolean = HotelUtilitie.IsEnableGoogleRequest(info.Hotel)
                Dim isEnabledSendingRatesAPICache As Boolean = HotelUtilitie.IsEnableSendRatesAPICache(info.Hotel)

                If logRates IsNot Nothing And logRates.Count > 0 Then

                    If isEnabledGoogleRequest Or isEnabledSendingRatesAPICache Then
                        Dim updatedRateDay As IEnumerable(Of Tarifas) = logRates.Where(Function(t) t.FechaInicia = RQ.StartDate And t.FechaFinaliza = RQ.EndDate).Distinct()
                        For Each rate As Tarifas In updatedRateDay
                            ExecuteServices(isEnabledGoogleRequest, isEnabledSendingRatesAPICache, RQ.HotelId, rate, info)
                        Next
                    End If
                End If

                Try
                    'Guardar Log
                    If logRates IsNot Nothing And logRates.Count > 0 Then
                        For Each rate As Tarifas In logRates

                            Dim xml As String = CreateXml(serviceRQ.RoomCode, serviceRQ.RoomName, rate)

                            Dim msg As String = "Se creó la tarifa de la habitación " & serviceRQ.RoomCode & " de la fecha " & rate.FechaInicia.ToString("MM/dd/yyyy") & " a la fecha " & rate.FechaFinaliza.ToString("MM/dd/yyyy") & " con el rateplan " & rate.idrateplan

                            Log(RQ.HotelId, acciones.Crear, serviceRQ.RoomCode, rate.FechaInicia, rate.FechaFinaliza, rate.idrateplan, xml, note:=msg)

                        Next
                    End If

                Catch
                End Try

                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        Private Function MappingRateUpdateRQ(RQ As RateUpdateRQ) As DTO.RateUpdateRQ
            Dim ServiceRQ As New DTO.RateUpdateRQ With {
                .HotelId = RQ.HotelId,
                .RoomId = RQ.RoomId,
                .RoomCode = RQ.RoomCode,
                .RoomName = RQ.RoomName,
                .RatePlanCode = RQ.RatePlanCode,
                .RateId = RQ.RateId,
                .StartDate = RQ.StartDate,
                .EndDate = RQ.EndDate,
                .IsOccupancyRate = RQ.IsOccupancyRate
            }

            Dim ServiceRQRatePlans As New List(Of DTO.RatePlanHeader)
            For Each itemRatePlan As RatePlanHeader In RQ.RatePlans
                Dim tempRatePlan As New DTO.RatePlanHeader With {
                    .Code = itemRatePlan.Code,
                    .Name = itemRatePlan.Name
                }

                ServiceRQRatePlans.Add(tempRatePlan)
            Next

            ServiceRQ.RatePlans = ServiceRQRatePlans


            Dim ServiceRQDates As New List(Of DTO.RateUpdateRQDate)
            For Each itemDate As RateUpdateRQDate In RQ.Dates
                Dim tempDate As New DTO.RateUpdateRQDate With {
                    .StartDate = itemDate.StartDate,
                    .EndDate = itemDate.EndDate
                    }
                ServiceRQDates.Add(tempDate)
            Next

            ServiceRQ.Dates = ServiceRQDates


            Dim ServiceRQPricesBase As New List(Of DTO.DailyRateDetailPrice)
            For Each PriceBase As DailyRateDetailPrice In RQ.Prices.Base
                Dim ServiceRQPriceBase As New DTO.DailyRateDetailPrice With {
                    .Type = PriceBase.Type,
                    .Price = PriceBase.Price,
                    .Occupation = PriceBase.Occupation
                    }
                ServiceRQPricesBase.Add(ServiceRQPriceBase)
            Next

            Dim ServiceRQPricesException As New List(Of DTO.DailyRateDetailPrice)
            If Not RQ.Prices.Exceptions Is Nothing Then
                For Each PriceException As DailyRateDetailPrice In RQ.Prices.Exceptions
                    Dim ServiceRQPriceException As New DTO.DailyRateDetailPrice With {
                        .Type = PriceException.Type,
                        .Price = PriceException.Price,
                        .Occupation = PriceException.Occupation
                        }
                    ServiceRQPricesException.Add(ServiceRQPriceException)
                Next
            End If

            Dim ServiceRQPricesExtra As New List(Of DTO.DailyRateDetailPrice)
            If Not RQ.Prices.Extra Is Nothing Then
                For Each PriceExtra As DailyRateDetailPrice In RQ.Prices.Extra
                    Dim ServiceRQPriceExtra As New DTO.DailyRateDetailPrice With {
                        .Type = PriceExtra.Type,
                        .Price = PriceExtra.Price,
                        .Occupation = PriceExtra.Occupation
                        }
                    ServiceRQPricesExtra.Add(ServiceRQPriceExtra)
                Next
            End If

            Dim ServiceRQPrices As New DTO.RateUpdatePrices With {
                .Base = ServiceRQPricesBase,
                .Exceptions = ServiceRQPricesException,
                .ExceptionDays = If(RQ.Prices?.ExceptionDays Is Nothing, "NNNNNNN", GetDaysOfWeekString(RQ.Prices.ExceptionDays)),
                .Extra = ServiceRQPricesExtra
                }
            ServiceRQ.Prices = ServiceRQPrices

            If Not RQ.Prices.Promotion Is Nothing Then
                Dim serviceRQPricesPromotion As New DTO.RateUpdateRQPromotion
                With serviceRQPricesPromotion
                    .Discount = RQ.Prices.Promotion.Discount
                    .EnglishDescription = RQ.Prices.Promotion.EnglishDescription
                    .SpanishDescription = RQ.Prices.Promotion.SpanishDescription
                End With
                ServiceRQ.Prices.Promotion = serviceRQPricesPromotion
            End If

            If Not RQ.Rules Is Nothing Then
                Dim ServiceRQGuestsRestrictions As New DTO.RateUpdateRQGuestsRestriction
                If Not RQ.Rules.GuestsRestrictions Is Nothing Then
                    With ServiceRQGuestsRestrictions
                        .Children = RQ.Rules.GuestsRestrictions.Children
                        .MaxAdults = RQ.Rules.GuestsRestrictions.MaxAdults
                        .MinAdults = RQ.Rules.GuestsRestrictions.MinAdults
                        .ExtraGuests = RQ.Rules.GuestsRestrictions.ExtraGuests
                        .MaxGuests = RQ.Rules.GuestsRestrictions.ExtraGuests
                    End With
                End If

                Dim ServiceRQBookingWindow As New DTO.RateUpdateRQBookingWindow
                If Not RQ.Rules.BookingWindow Is Nothing Then
                    With ServiceRQBookingWindow
                        .EndDate = RQ.Rules.BookingWindow.EndDate
                        .StartDate = RQ.Rules.BookingWindow.StartDate
                    End With
                End If
                Dim ServiceRQRules As New DTO.RateUpdateRQRules With {
                .UseDefaultRules = RQ.Rules.UseDefaultRules,
                .MinLOS = RQ.Rules.MinLOS,
                .MaxLOS = RQ.Rules.MaxLOS,
                .MaxAdvanceBooking = RQ.Rules.MaxAdvanceBooking,
                .MinAdvanceBooking = RQ.Rules.MinAdvanceBooking,
                .NoArrival = If(RQ.Rules?.NoArrival Is Nothing, "NNNNNNN", GetDaysOfWeekString(RQ.Rules.NoArrival)),
                .GuestsRestrictions = ServiceRQGuestsRestrictions,
                .BookingWindow = ServiceRQBookingWindow
                }
                ServiceRQ.Rules = ServiceRQRules
            End If

            Return ServiceRQ
        End Function

        Private Function GetDaysOfWeekString(Days As DaysOfWeek) As String
            Dim Week As String = ""

            If Days.Mon Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Tue Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Wed Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Thu Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Fri Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Sat Then
                Week += "Y"
            Else
                Week += "N"
            End If
            If Days.Sun Then
                Week += "Y"
            Else
                Week += "N"
            End If

            Return Week
        End Function

        Private Sub Log(ByVal hotelId As Integer, ByVal action As acciones, ByVal room As String, ByVal startDate As Date, ByVal endDate As Date, ByVal rateCode As String, ByVal xml As String, Optional ByVal dataXml As String = "", Optional ByVal note As String = "")

            'Dim msg As String = ""
            'Select Case action
            '    Case acciones.Crear
            '        msg = "Se creó la tarifa de la habitación " & room & " de la fecha " & startDate.ToString("MM/dd/yyyy") & " a la fecha " & endDate.ToString("MM/dd/yyyy") & " con el rateplan " & rateCode
            'End Select



            With (New PaginaBase)
                .guardalog(pagina:="/rate-manager-ui/dist/rates-admin.aspx", action:=action, nota:=note, peticion:="", datos:=dataXml, datosDespues:=xml, hotelId:=hotelId)
            End With
        End Sub

        Private Function CreateXml(ByRef roomCode As String, ByRef roomName As String, ByRef rate As Tarifas)
            Dim datFare As New FaresData
            Dim rowFare As DataRow

            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()
                rowFare(FaresData.PKIDFARES_FIELD) = rate.idTarifa
                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = rate.idTipoHabitacion_Hotel
                rowFare(FaresData.STARTDATE_FIELD) = rate.FechaInicia
                rowFare(FaresData.ENDDATE_FIELD) = rate.FechaFinaliza

                rowFare(FaresData.PRICE_FIELD) = rate.Precio
                rowFare(FaresData.NINIORATE) = rate.NiniosRate
                rowFare(FaresData.RATEENPRICE_FIELD) = rate.PrecioAdolescente


                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = rate.PrecioExtraAdulto
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = rate.PrecioExtraNinio
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = rate.PrecioAdolescenteExtra

                rowFare(FaresData.PRICENR_FIELD) = rate.PrecioNR
                rowFare(FaresData.NINIORATENR) = rate.NiniosRateNR
                rowFare(FaresData.RATEENPRICENR_FIELD) = rate.PrecioAdolescenteNR

                rowFare(FaresData.EXTRAADULTPRICENR_FIELD) = rate.PrecioExtraAdultoNR
                rowFare(FaresData.EXTRACHILDPRICENR_FIELD) = rate.PrecioExtraNinioNR
                rowFare(FaresData.EXTRATEENPRICENR_FIELD) = rate.PrecioAdolescenteExtraNR


                rowFare(FaresData.RATETYPE_FIELD) = rate.TipoTarifa
                rowFare(FaresData.RATECODE_FIELD) = rate.CodigoTarifa
                rowFare(FaresData.EXCEPTION_FIELD) = rate.Excepciones
                rowFare(FaresData.NOARRIVOS_FIELD) = rate.NoArrivos
                rowFare(FaresData.IDRATEPLAN_FIELD) = rate.idrateplan
                rowFare(FaresData.ENDDATE_FIELD) = rate.FechaFinaliza
                rowFare(FaresData.RULESDEFAULT) = IIf(rate.RateRulesDefault Is Nothing, True, rate.RateRulesDefault)
                rowFare(FaresData.IDDICCDESCPROM_FIELD) = IIf(rate.idDiccPromoDesc Is Nothing, 0, rate.idDiccPromoDesc)
                .Rows.Add(rowFare)
            End With

            datFare.Tables(0).Columns.Add("Descr_rateplan")
            datFare.Tables(0).Rows(0)("Descr_rateplan") = String.Format("{0} {1}", roomCode, roomName)

            Dim xml As String = String.Empty

            If rate.PrecioNR > 0 Then
                xml = Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRateNR", datFare)
            Else
                xml = Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRate", datFare)
            End If

            Return xml
        End Function

        Private Sub SendClosureToService(ByVal rateId As Integer, ByVal startDate As Date, ByVal endDate As Date, ByVal endpoint As String, ByVal service As String, ByVal info As companyInfo)

            Dim requests As List(Of XDocument) = New List(Of XDocument)

            Dim vDayRatesForClosure As List(Of vDayRates) = Conflux.Helpers.Rates.RatesHelpers.GetVDayRate(rateId, startDate, endDate)

            RestrictionsParser.Init(info.Empresa)

            Dim availStatusMessages As OTA.Models.Restrictions.AvailStatusMessages = RestrictionsParser.ToAvailStatusMessages(vDayRatesForClosure, "N")

            Dim availStatusMessagesList As List(Of XElement) = Xml.OTA.Request.Restrictions.HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessages) 'Meter los dias en el request para google

            For Each availStatusMessage As XElement In availStatusMessagesList
                'Request 
                Dim xmlRequest As XDocument = Xml.Soap.Soap.CreateSoapRequestXml(availStatusMessage)
                requests.Add(xmlRequest)
            Next

            Dim restrictionResponseList As List(Of Conflux.Models.Restrictions.Response.RestrictionResponse) = New List(Of Conflux.Models.Restrictions.Response.RestrictionResponse)

            For Each request As XDocument In requests
                Dim response As Conflux.Models.Restrictions.Response.RestrictionResponse = HotelUtilitie.ConfluxServiceHelper.UpdateRestriction(request, endpoint, RestrictionEnum.LockRate)
                restrictionResponseList.Add(response)
            Next

            Dim note As String = String.Format("Tarifa enviada a {0} LockRate", service)
            Dim noteError As String = String.Format("Error al sincronizar LockRate {0}", service)

            For Each response As Conflux.Models.Restrictions.Response.RestrictionResponse In restrictionResponseList

                If response.IsSuccess Then
                    With (New PaginaBase)
                        .guardalog(pagina:="/rate-manager-ui/dist/rates-admin.aspx", action:=acciones.Sincronizar, nota:=note, peticion:="", datos:=response.Restrictions(0).XmlRequest(0).ToString(), datosDespues:=response.Restrictions(0).Xml(0).ToString(), hotelId:=info.Hotel)
                    End With
                Else
                    With (New PaginaBase)
                        .guardalog(pagina:="/rate-manager-ui/dist/rates-admin.aspx", action:=acciones.Sincronizar, nota:=noteError, peticion:="", datos:=response.Xml.ToString(), datosDespues:="", hotelId:=info.Hotel)
                    End With
                End If

            Next

        End Sub


        Private Sub SendRatesToService(ByVal ratesMessages As RatesMessages, ByVal endpoint As String, ByVal endpointDelete As String, ByVal hotelId As Integer, ByVal service As String)
            Dim res As Tuple(Of RateResponse, RateResponse) = HotelUtilitie.ConfluxServiceHelper.UpdateRate(ratesMessages, endpoint, endpointDelete)

            Dim note As String = String.Format("Tarifa envida a {0}", service)
            Dim noteDelete As String = String.Format("Tarifa envidada para eliminar a {0}", service)

            Log(hotelId:=hotelId, action:=acciones.Sincronizar, room:="", startDate:=Nothing, endDate:=Nothing, rateCode:="", xml:=res.Item1.Xml, dataXml:=res.Item1.RequestXML, note:=note)

            'Delete Log
            If res.Item2 IsNot Nothing Then
                Log(hotelId:=hotelId, action:=acciones.Eliminar, room:="", startDate:=Nothing, endDate:=Nothing, rateCode:="", xml:=res.Item2.Xml, dataXml:=res.Item2.RequestXML, note:=service)
            End If

        End Sub


        Private Sub ExecuteServices(ByVal isEnabledGoogleRequest As Boolean, ByVal isEnabledSendingRatesAPICache As Boolean, ByVal hotelId As Integer, ByVal rate As Tarifas, ByVal info As companyInfo)

            Dim ratesForRequest As RatesMessages = HotelUtilitie.ConfluxServiceHelper.GetRateMessages(rate.idTarifa, rate.FechaInicia, rate.FechaFinaliza, hotelId, info.Empresa, TypeRateEnum.RoomRate)

            If isEnabledGoogleRequest Then
                Try
                    SendRatesToService(ratesForRequest, HotelUtilitie.ENDPOINT, HotelUtilitie.ENDPOINTDELETE, hotelId, "Conflux")
                    SendClosureToService(rate.idTarifa, rate.FechaInicia, rate.FechaFinaliza, HotelUtilitie.ENDPOINTCLOSURE, "Conflux", info)
                Catch ex As Exception
                    Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                    Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                    errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"), New System.Xml.Linq.XAttribute("Code", "448"), New System.Xml.Linq.XText(ex.Message))
                    errorsElement.Add(errorElementProperty)
                    Log(hotelId, acciones.Sincronizar, "", Nothing, Nothing, "", xml:=errorsElement.ToString(), note:="No se pudo enviar la tarifa a Conflux")
                End Try

            End If

            If isEnabledSendingRatesAPICache Then
                Try
                    SendRatesToService(ratesForRequest, HotelUtilitie.ENDPOINTAPI, HotelUtilitie.ENDPOINTAPIDELETE, hotelId, "APICache")
                    SendClosureToService(rate.idTarifa, rate.FechaInicia, rate.FechaFinaliza, HotelUtilitie.ENDPOINTAPICLOSURE, "APICache", info)
                Catch ex As Exception
                    Dim errorsElement As New System.Xml.Linq.XElement("Errors")
                    Dim errorElementProperty As New System.Xml.Linq.XElement("Error")
                    errorElementProperty.Add(New System.Xml.Linq.XAttribute("Type", "3"), New System.Xml.Linq.XAttribute("Code", "448"), New System.Xml.Linq.XText(ex.Message))
                    errorsElement.Add(errorElementProperty)
                    Log(hotelId, acciones.Sincronizar, "", Nothing, Nothing, "", xml:=errorsElement.ToString(), note:="No se pudo enviar la tarifa a APICache")
                End Try
            End If

        End Sub

    End Class
End Namespace