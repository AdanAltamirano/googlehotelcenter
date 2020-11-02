Imports System.Web.Http
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Common
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.API.Models
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Facade
Imports Portal.General.Common.Data

Namespace API.Controllers

    <RoutePrefix("api/promotions")>
    Public Class OffersController
        Inherits ShurikenController
        Public service As New OfferService

        'GET api/promotions?querystring 
        <Route(""), HttpGet, Queryable>
        Public Function GetByHotelId() As IQueryable(Of vPromotions)
            Return service.OffersList()
        End Function

        ' GET api/promotions/1978/code/PR01
        <Route("{HotelId:Int}/code/{code}"), HttpGet>
        Public Function GetByCode(HotelId As Integer, code As String) As DTO.Offer
            Return service.FindOfferByHotelAndCode(HotelId, code)
        End Function

        'TODO: Cambiar la ruta de la api en comentarios en las diferentes acciones 

        'POST api/promotions/1978/save
        <Route("{hotelId:int}/save"), HttpPost>
        Public Function OfferAdd(<FromBody> RQ As DTO.Offer, hotelId As Integer) As Net.Http.HttpResponseMessage
            RQ.HotelId = hotelId

            Dim result As KeyValuePair(Of String, String) = service.Add(RQ)

            If result.Key = 1 Then
                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/promotions/1978/update/code/PR04
        <Route("{hotelId:int}/update/code/{code}"), HttpPost>
        Public Function OfferUpdate(<FromBody> RQ As DTO.Offer, hotelId As Integer, code As String) As Net.Http.HttpResponseMessage
            RQ.HotelId = hotelId

            Dim result As KeyValuePair(Of String, String) = service.Update(RQ)

            If result.Key = 1 Then
                Return NoContent()
            End If
            Return BadRequest(result)
        End Function

        'POST api/promotions/enable/1978/code/PR04
        <Route("enable/{hotelId:Int}/code/{code}"), HttpPost>
        Public Function EnablePromotion(hotelId As Integer, code As String) As Net.Http.HttpResponseMessage
            With New RatePlanFacade
                If .LogicActiveRatePlan(hotelId, code) Then
                    Return NoContent()
                End If
            End With
            Return BadRequest(New KeyValuePair(Of String, String)("0", "No se pudo activar la promoción"))
        End Function

        'POST api/promotions/disable/1978/code/PR04
        <Route("disable/{hotelId:Int}/code/{code}"), HttpPost>
        Public Function DisablePromotion(hotelId As Integer, code As String) As Net.Http.HttpResponseMessage
            With New RatePlanFacade
                If .LogicDeleteRatePlan(hotelId, code) Then
                    With New PaginaBase
                        .guardalog("/Pages/RatesPlans.aspx", PaginaBase.acciones.Eliminar, "Eliminó la promoción con el codigo de tarifa " & code)
                    End With
                    Return NoContent()
                End If
            End With
            Return BadRequest(New KeyValuePair(Of String, String)("0", "No se pudo desactivar la promoción"))
        End Function
    End Class
End Namespace