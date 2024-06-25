Imports System.Net
Imports System.Web.Http
Imports System.Web.UI.WebControls
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.PaginaBase

Namespace API.Controller
    <RoutePrefix("api/utils"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany,agencycompany")>
    Public Class UtilsController
        Inherits ShurikenController

        Public UtilsService As New UtilsService

        ' GET api/utils/channelsList
        <Route("channels"), HttpGet>
        Public Function GetChannelsList() As List(Of APIServices.Models.Canales)
            Dim result As List(Of APIServices.Models.Canales) = UtilsService.GetChannels()

            Return result
        End Function

        <Route("channels/{idHotel:int}"), HttpGet>
        Public Function GetHotelChannels(ByVal idHotel As Integer) As List(Of vHotelChannel)
            Dim result As List(Of vHotelChannel) = UtilsService.GetHotelChannels(idHotel)
            Return result
        End Function

    End Class
End Namespace
