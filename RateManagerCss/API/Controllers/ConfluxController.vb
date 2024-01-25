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

            If Not result.IsSuccess Then

                Log("Sincronizar Tarifas Conflux con el hotel: ", result.Xml, hotelId, String.Empty)

                Return BadRequest(result.Error)

            End If

            For Each request As APIServices.Conflux.Models.Rates.Response.Rate In result.Rates
                Log("Sincronizar Tarifas Conflux con el hotel: ", request.Xml, hotelId, request.XmlRequest)
            Next

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function

        <Route("updaterestrictions/{hotelId:int}"), HttpPost>
        Public Function UpdateRestrictions(ByVal hotelId As Integer) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim result As RestrictionResponse = ConfluxService.UpdateRestrictionsGeneral(hotelId, info.Empresa)

            If Not result.IsSuccess Then
                Log("Sincronizar Restricciones Conflux con el hotel: ", result.Xml, hotelId)
                Return BadRequest(result.Error)
            ElseIf result.IsSuccess Then
                For Each restriction As Restriction In result.Restrictions
                    Select Case restriction.Type
                        Case RestrictionEnum.LockGral
                            For index As Integer = 0 To restriction.Xml.Count() Step 1
                                Dim note As String = String.Format("Sincronizar request numero {0} LockGral Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            Next
                        Case RestrictionEnum.LockRatePlan
                            For index As Integer = 0 To restriction.Xml.Count() Step 1
                                Dim note As String = String.Format("Sincronizar request numero {0} LockRatePlan Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            Next
                        Case RestrictionEnum.LockRoomType
                            For index As Integer = 0 To restriction.Xml.Count() Step 1
                                Dim note As String = String.Format("Sincronizar request numero {0} LockRoomtype Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                            Next
                    End Select
                Next
            End If

            Dim toObject As Object = result

            Return Ok(toObject)

        End Function


        Private Sub Log(ByVal note As String, ByVal xml As String, ByVal hotelId As Integer, Optional ByVal requestXMl As String = "")
            With (New PaginaBase)
                .guardalog("/rate-manager-ui/dist/channel-rates-update.aspx", acciones.Sincronizar, note & hotelId, "", requestXMl, xml, hotelId:=hotelId)
            End With
        End Sub

    End Class
End Namespace
