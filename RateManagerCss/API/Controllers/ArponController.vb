Imports System.Net.Http
Imports System.Web.Http
Imports NinjAPI
Imports APIServices.Models
Imports APIServices.Service.Arpon
Imports APIServices.Service.Arpon.Models

Namespace API.Controllers
    <RoutePrefix("api/arpon")>
    Public Class ArponController
        Inherits ShurikenController

        Dim ArponService As ArponService = New ArponService()

        <Route("hotel/{hotelId:Int}"), HttpGet>
        Public Function CreateUser(ByVal hotelId As Integer) As HttpResponseMessage

            Dim result As HotelIPH_Arpon_Model = ArponService.GetHotelArponById(hotelId)

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function


        <Route("rateplans/{hotelId:Int}"), HttpGet>
        Public Function GetRatePlansByHotel(ByVal hotelId As Integer) As List(Of RatePlansIPH_Arpon_Model)
            Return ArponService.GetRatePlansByIdHotel(hotelId)
        End Function

        <Route("rooms/{hotelId:Int}"), HttpGet>
        Public Function GetRoomsByHotel(ByVal hotelId As Integer) As List(Of RoomsIPH_Arpon_Model)
            Return ArponService.GetRoomsByIdHotel(hotelId)
        End Function

        <Route("save/hotel/{hotelId:Int}"), HttpPost>
        Public Function SaveHotel(ByVal hotelId As Integer, <FromBody> hotelArponModel As HotelIPH_Arpon_Model) As HttpResponseMessage
            Dim success As Boolean = ArponService.AddHotelArpon(hotelArponModel)

            If Not success Then

                Dim [error] As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("500", "Error del Sistema")

                Return BadRequest([error])

            End If

            Return NoContent()

        End Function

        <Route("save/rateplans/{hotelId:Int}"), HttpPost>
        Public Function SaveRatePlans(ByVal hotelId As Integer, <FromBody> ratePlansArponModel As List(Of RatePlansIPH_Arpon_Model)) As HttpResponseMessage

            Dim success As Boolean = ArponService.SaveRatePlansArpon(ratePlansArponModel)

            If Not success Then

                Dim [error] As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("500", "Error del Sistema")

                Return BadRequest([error])

            End If

            Return NoContent()

        End Function

        <Route("save/rooms/{hotelId:Int}"), HttpPost>
        Public Function SaveRooms(ByVal hotelId As Integer, <FromBody> roomsArponModel As List(Of RoomsIPH_Arpon_Model)) As HttpResponseMessage

            Dim success As Boolean = ArponService.SaveRoomsArpon(roomsArponModel)

            If Not success Then

                Dim [error] As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("500", "Error del Sistema")

                Return BadRequest([error])

            End If

            Return NoContent()

        End Function


    End Class
End Namespace
