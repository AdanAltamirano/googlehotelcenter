Imports System.Globalization
Imports System.Net.Http
Imports System.Threading
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports RateManager.API.Helpers

Namespace API.Controllers
    <RoutePrefix("api/portal")>
    Public Class PortalController
        Inherits ShurikenController

        Public Service As New PortalsService

        'Get api/portal/hotel/1978
        '<Route("hotel/{id:int}"), HttpGet>
        'Public Function GetPortals(id As Integer) As List(Of DTO.Portal)
        '    Return Service.GetPortals(id)
        'End Function

        'Get api/portal/coorp
        <Route("coorp"), HttpGet>
        Public Function GetCoorporatives() As List(Of DTO.Coorporative)
            Return Service.GetCoorporatives()
        End Function

        'Post api/portal/coorp
        <Route("coorp"), HttpPost>
        Public Function CreateCoorporative(<FromBody> coorporative As DTO.Coorporative) As HttpResponseMessage
            Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            Dim lang = Thread.CurrentThread.CurrentCulture.ToString()
            Dim created As Boolean
            created = Service.CreateCoorporative(coorporative)
            If (created = False) Then
                Dim errorCoorporative As String = If(lang.Equals("es-MX"),
                    "Ha ocurrido un error: No se ha podido crear el corporativo ó el corporativo ya existe",
                     "An error has occurred: The corporate could not be created or the corporate already exists")
                Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", errorCoorporative)
                Return BadRequest(result)
            End If
            Dim ok = New HttpResponseMessage(Net.HttpStatusCode.OK)
            Return ok
        End Function

        'Post api/portal/portals
        <Route("portals"), HttpPost>
        Public Function CreatePortals(<FromBody> portals As DTO.NewPortal) As HttpResponseMessage
            Thread.CurrentThread.CurrentCulture = New CultureInfo(PortalCulture.GetCulture.ToString)
            Dim lang = Thread.CurrentThread.CurrentCulture.ToString()
            Dim created As String
            created = Service.CreatePortals(portals)
            If (created.Equals("No se crearon los portales")) Then
                Dim errorPortals As String = If(lang.Equals("es-MX"),
                    "Ha ocurrido un error: No se pudieron crear los portales",
                    "An error ocurred: Could not create portals")
                Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", errorPortals)
                Return BadRequest(result)
            End If
            If (created.Equals("El hotel ya cuenta con portales")) Then
                Dim errorPortals As String = If(lang.Equals("es-MX"),
                    "Ha ocurrido un error: El hotel ya cuenta con portales",
                    "An error has occurred: The hotel already has portals")
                Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", errorPortals)
                Return BadRequest(result)
            End If
            If (created.Equals("El nombre de la aplicacion ya existe")) Then
                Dim errorPortals As String = If(lang.Equals("es-MX"),
                    "Ha ocurrido un error: El nombre de la aplicación ya existe",
                    "An error has occurred: The application name already exists")
                Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", errorPortals)
                Return BadRequest(result)
            End If
            'Enviar Correo
            Dim isSent As Boolean
            isSent = SendPortalMail(created)
            Dim ok = New HttpResponseMessage(Net.HttpStatusCode.OK)
            Return ok
        End Function
    End Class
End Namespace



