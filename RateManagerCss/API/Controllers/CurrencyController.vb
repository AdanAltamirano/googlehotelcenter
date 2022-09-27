Imports System.Web.Http
Imports NinjAPI
Imports APIServices.Currency
Imports APIServices.Models.DTO.Currency

<RoutePrefix("api/currencies")>
Public Class CurrencyController
    Inherits ShurikenController

    Private currencyService As CurrencyService = New CurrencyService()

    'GET api/currencies
    <Route(""), HttpGet>
    Public Function GetCurrencies() As List(Of CurrencyDTO)
        Return currencyService.GetCurrencies()
    End Function

End Class
