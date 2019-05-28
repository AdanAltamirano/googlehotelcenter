Imports System.Net
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query

<RoutePrefix("api/hotelstest")>
Public Class HotelController
    Inherits ShurikenController

    Public HotelService As New HotelService

    ' GET api/<controller>
    <Route(""), HttpGet, Queryable>
    Public Function GetAll() As IQueryable(Of HotelBasicInfo)
        Return HotelService.GetAll()
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)

        If disposing Then
            HotelService.DbContext.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

End Class
