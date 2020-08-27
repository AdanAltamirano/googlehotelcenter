Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports RateManager.API.Helpers

Namespace API.Controllers
    <RoutePrefix("api/agencies")>
    Public Class AgenciesController
        Inherits ShurikenController

        Public AgencyService As New AgencyService


        ' GET api/agencies
        <Route(""), HttpGet>
        Public Function GetAgencies() As IQueryable(Of vAgencies)

            Return AgencyService.Get()
        End Function


    End Class
End Namespace

