Imports System.Net
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query

Namespace API.Controllers
    <RoutePrefix("api/hotels")>
    Public Class HotelsController
        Inherits ShurikenController

        Public HotelService As New HotelService

        ' GET api/hotels
        <Route(""), HttpGet, Queryable>
        Public Function GetAll() As IQueryable(Of vHotelBasicInfo)
            Return HotelService.GetAll()
        End Function

        ' GET api/hotels/1
        <Route("{Id:int}"), HttpGet>
        Public Function GetById(Id As Integer) As DTO.HotelInfo
            Return HotelService.Get(Id)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)

            If disposing Then
                HotelService.DbContext.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

    End Class
End Namespace

