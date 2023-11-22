Imports System.Web.Http
Imports System.Net.Http
Imports NinjAPI
Imports NinjAPI.Query
Imports APIServices.Conflux
Imports APIServices.Conflux.Enum
Imports APIServices.Models
Imports APIServices.Conflux.Models.User
Imports APIServices.Conflux.Models.User.Response
Imports APIServices.Conflux.Models.Rates.Response
Imports APIServices.Conflux.Models.Restrictions.Response
Imports RateManager.PaginaBase

Namespace API.Controllers
    <RoutePrefix("api/conflux")>
    Public Class ConfluxController
        Inherits ShurikenController

        Dim ConfluxService As ConfluxService = New ConfluxService()


        <Route("hotels"), HttpGet, Queryable>
        Public Function GetHotels() As IQueryable(Of vHotelActives)
            Return ConfluxService.GetHotels()
        End Function

        <Route("create/user"), HttpPost>
        Public Function CreateUser(<FromBody> user As User) As HttpResponseMessage

            Dim result As UserReponse = ConfluxService.CreateUser(user)

            If Not result.IsSuccess Then

                Return BadRequest(result.Error)

            End If

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function


        <Route("updaterates/{hotelId:int}"), HttpPost>
        Public Function UpdateRates(ByVal hotelId As Integer) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim result As RateResponse = ConfluxService.UpdateRates(hotelId, info.Empresa)

            Log("Sincronizar Tarifas Conflux con el hotel: ", result.Xml, hotelId)

            If Not result.IsSuccess Then

                Return BadRequest(result.Error)

            End If

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function

        <Route("updaterestrictions/{hotelId:int}"), HttpPost>
        Public Function UpdateRestrictions(ByVal hotelId As Integer) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim result As RestrictionResponse = ConfluxService.UpdateRestrictions(hotelId, info.Empresa)

            If Not result.IsSuccess Then
                Log("Sincronizar Restricciones Conflux con el hotel: ", result.Xml, hotelId)
                Return BadRequest(result.Error)
            ElseIf result.IsSuccess Then
                For Each restriction As Restriction In result.Restrictions
                    Select Case restriction.Type
                        Case RestrictionEnum.LockGral
                            Log("Sincronizar Restricciones LockGral No Promo Conflux con el hotel: ", restriction.Xml(0).ToString(), hotelId)
                            Log("Sincronizar Restricciones LockGral Promo Conflux con el hotel: ", restriction.Xml(1).ToString(), hotelId)
                        Case RestrictionEnum.LockRatePlan
                            Log("Sincronizar Restricciones LockRatePlan Conflux con el hotel: ", restriction.Xml(0).ToString(), hotelId)
                        Case RestrictionEnum.LockRoomType
                            Log("Sincronizar Restricciones LockRoomType Conflux con el hotel: ", restriction.Xml(0).ToString(), hotelId)
                    End Select
                Next
            End If

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function


        Private Sub Log(ByVal note As String, ByVal xml As String, ByVal hotelId As Integer)
            With (New PaginaBase)
                .guardalog("/rate-manager-ui/dist/channel-rates-update.aspx", acciones.Sincronizar, note & hotelId, "", "", xml, hotelId:=hotelId)
            End With
        End Sub

    End Class
End Namespace
