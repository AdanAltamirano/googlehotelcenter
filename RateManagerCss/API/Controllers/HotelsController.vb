Imports System.Net
Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers

Namespace API.Controllers
    <RoutePrefix("api/hotels"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany")>
    Public Class HotelsController
        Inherits ShurikenController

        Public HotelService As New HotelService

        ' GET api/hotels
        <Route(""), HttpGet, Queryable>
        Public Function GetAll() As IQueryable(Of vHotelBasicInfo)

            Dim userRoles() As String = UserDataHelper.GetRoles()

            If userRoles.Contains("supervisor") Then
                Return HotelService.GetAll()
            ElseIf userRoles.Contains("userchain") Then
                Dim userCorpId = UserDataHelper.GetUserCorpId(GetUserId().Value)
                Return HotelService.GetAll().Where(Function(h) (Not h.CorpId Is Nothing) AndAlso h.CorpId = userCorpId)
            ElseIf userRoles.Contains("hotelcompany") Then
                Dim hotels() As Integer = UserDataHelper.GetUserHotels(GetUserId().Value).Select(Function(h) h.HotelId).ToArray()
                Return HotelService.GetAll().Where(Function(h) hotels.Contains(h.Id))
            End If

            'regresa vacio cualquier caso extra
            Return New vHotelBasicInfo() {}.AsQueryable()
        End Function

        ' GET api/hotels/1
        <Route("{HotelId:int}"), HttpGet>
        Public Function GetById(HotelId As Integer) As DTO.HotelInfo
            Return HotelService.Get(HotelId)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)

            If disposing Then
                HotelService.DbContext.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

    End Class
End Namespace

