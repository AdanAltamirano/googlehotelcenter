Imports System.Globalization
Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports RateManager.API.Helpers

Namespace API.Controllers
    <RoutePrefix("api/f2g"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany,usuariohotel")>
    Public Class F2GController
        Inherits ShurikenController

        Public F2GService As New F2GService

        'Get api/f2g/hotelsrates/1
        <Route("hotelsrates/{corporateId:int}"), HttpGet>
        Public Function GetF2GHotelsRates(corporateId As Integer) As DTO.F2GHotelsRates
            Return F2GService.GetF2GHotelsRates(corporateId)
        End Function

        'Get api/f2g/configuration/1
        <Route("configuration/{corporateId:int}"), HttpGet>
        Public Function GetF2GConfigurationByCorporateId(corporateId As Integer) As List(Of DTO.F2GModel)
            Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            Dim lang = Thread.CurrentThread.CurrentCulture.ToString()
            Return F2GService.GetF2GConfigurationByCorporateId(corporateId, lang)
        End Function

        'Get api/f2g/corporates
        <Route("corporates"), HttpGet>
        Public Function GetF2GCorporates() As List(Of DTO.F2GCorporate)
            Return F2GService.GetF2GCorporates()
        End Function

        'Post api/f2g/update
        <Route("update"), HttpPost>
        Public Function UpdateF2G(<FromBody> f2g As DTO.F2GSaveModel) As HttpResponseMessage
            Dim update As Boolean
            update = F2GService.UpdateF2GRatePlan(f2g)
            If (update = False) Then
                Return New HttpResponseMessage(Net.HttpStatusCode.BadRequest)
            End If
            Return New HttpResponseMessage(Net.HttpStatusCode.OK)
        End Function

    End Class
End Namespace
