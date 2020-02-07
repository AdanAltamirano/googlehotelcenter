Imports System.Web.Http
Imports NinjAPI
Imports APIServices
Imports System.Threading
Imports System.Globalization
Imports APIServices.Models
Imports RateManager.API.Models

Namespace API.Controllers
    <RoutePrefix("api/config")>
    Public Class HotelConfigurationController
        Inherits ShurikenController

        Public Service As New HotelConfigurationService
        '' GET api/config/1 
        '<Route("{id:int}"), HttpGet>
        'Public Function GetErrors(id As Integer) As DTO.HotelConfiguration
        '    Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
        '    Dim lang = Thread.CurrentThread.CurrentCulture.ToString()
        '    Dim dates As Dictionary(Of String, Dictionary(Of String, Object)) = Service.Dates(id, lang)
        '    Dim creditcard As Dictionary(Of String, Dictionary(Of String, Object)) = Service.CreditCard(id, lang)
        '    Dim properties As Dictionary(Of String, Dictionary(Of String, Object))
        '    properties = dates.Union(creditcard).ToDictionary(Function(p) p.Key, Function(p) p.Value)
        '    Dim enableAvailability As List(Of DTO.RatePlansByHotel) = Service.EnableAvailability(id, lang)
        '    Dim hotel As New DTO.HotelConfiguration()
        '    hotel.Id = id
        '    hotel.Properties = properties
        '    hotel.RatePlansList = enableAvailability
        '    Return hotel
        'End Function

        'Get api/config/1 
        <Route("{id:int}"), HttpGet>
        Public Function GetErrors(id As Integer) As List(Of HotelConfig)
            Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            Dim lang = Thread.CurrentThread.CurrentCulture.ToString()
            Dim listHotelConfig As New List(Of HotelConfig)
            Dim dates As Dictionary(Of String, Dictionary(Of String, Object)) = Service.Dates(id, lang)
            Dim creditcard As Dictionary(Of String, Dictionary(Of String, Object)) = Service.CreditCard(id, lang)
            Dim properties As Dictionary(Of String, Dictionary(Of String, Object))
            properties = dates.Union(creditcard).ToDictionary(Function(p) p.Key, Function(p) p.Value)
            Dim i As Integer
            i = 1
            For Each kvp As KeyValuePair(Of String, Dictionary(Of String, Object)) In properties
                Dim hotelConfig As New HotelConfig
                hotelConfig.idProperty = i
                hotelConfig.nameProperty = kvp.Key
                hotelConfig.message = kvp.Value.Item("Message")
                hotelConfig.status = kvp.Value.Item("Status")
                listHotelConfig.Add(hotelConfig)
                i += 1
            Next
            Return listHotelConfig
        End Function

        'Get api/config/rates/1
        <Route("rates/{id:int}"), HttpGet>
        Public Function GetRatesErrors(id As Integer) As List(Of RatesConfig)
            Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            Dim lang = Thread.CurrentThread.CurrentCulture.ToString()
            Dim listRates As New List(Of RatesConfig)
            Dim enableAvailability As List(Of DTO.RatePlansByHotel) = Service.EnableAvailability(id, lang)
            Dim i As Integer = 0
            While (i < enableAvailability.Count)
                Dim rateConfig As New RatesConfig
                Dim listHotelConfig As New List(Of HotelConfig)
                rateConfig.id = enableAvailability.ElementAt(i).Id
                Dim j As Integer
                j = 1
                For Each kvp As KeyValuePair(Of String, Dictionary(Of String, Object)) In enableAvailability.ElementAt(i).Properties
                    Dim hotelConfig As New HotelConfig
                    hotelConfig.idProperty = j
                    hotelConfig.nameProperty = kvp.Key
                    hotelConfig.message = kvp.Value.Item("Message")
                    hotelConfig.status = kvp.Value.Item("Status")
                    listHotelConfig.Add(hotelConfig)
                    j += 1
                Next
                rateConfig.listProperties = listHotelConfig
                'rateConfig.portals = enableAvailability.ElementAt(i).listPortales
                listRates.Add(rateConfig)
                i += 1
            End While
            Return listRates
        End Function
    End Class
End Namespace
