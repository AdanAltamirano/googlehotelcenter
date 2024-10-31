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

        <Route("users/connectivity"), HttpGet, Queryable>
        Public Function GetUserConnectivities() As IQueryable(Of vUsersConnectivity)
            Return ConfluxService.GetUsersConnectivities()
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

            Dim result As Tuple(Of RateResponse, RateResponse) = ConfluxService.UpdateRates(hotelId, info.Empresa)

            Dim ratesToUpdate As RateResponse = result.Item1
            Dim ratesToDelete As RateResponse = result.Item2

            If Not ratesToUpdate.IsSuccess Then

                Log("Sincronizar Tarifas Conflux con el hotel: ", ratesToUpdate.Xml, hotelId, String.Empty)

                Return BadRequest(ratesToUpdate.Error)

            End If

            Dim index As Integer = 1

            For Each request As APIServices.Conflux.Models.Rates.Response.Rate In ratesToUpdate.Rates
                Dim note As String = String.Format("Sincronizar request numero {0} Tarifas Conflux con el hotel: ", (index))
                Log(note, request.Xml, hotelId, request.XmlRequest)
                index += 1
            Next

            Dim toObject As Object = ratesToUpdate

            'Log Delete
            If result.Item2 IsNot Nothing Then
                Dim noteDelete As String = String.Format("Eliminar Tarifas Conflux con el hotel: {0}", hotelId)
                Log(noteDelete, ratesToDelete.Xml, hotelId, ratesToDelete.RequestXML)
            End If


            Return Ok(toObject)

        End Function

        <Route("updaterestrictions/{hotelId:int}"), HttpPost>
        Public Function UpdateRestrictions(ByVal hotelId As Integer) As HttpResponseMessage

            Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

            Dim result As RestrictionResponse = ConfluxService.UpdateRestrictionsGeneral(hotelId, info.Empresa)

            'Cierres de Tarifas

            Dim ratesClosure As RestrictionResponse = ConfluxService.UpdateRestrictionsRates(hotelId, info.Empresa)


            If Not ratesClosure.IsSuccess Then
                Log("Sincronizar Restricciones Tarifas Conflux con el hotel: ", result.Xml, hotelId)
            ElseIf ratesClosure.IsSuccess Then
                For Each restriction As Restriction In ratesClosure.Restrictions
                    Select Case restriction.Type
                        Case RestrictionEnum.LockRate
                            Dim index As Integer = 0
                            While index < restriction.Xml.Count()
                                Dim note As String = String.Format("Sincronizar request numero {0} LockRate(Cierre de Tarifa) Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                                index = index + 1
                            End While
                    End Select
                Next
            End If

            If Not result.IsSuccess Then
                Log("Sincronizar Restricciones Conflux con el hotel: ", result.Xml, hotelId)
                Return BadRequest(result.Error)
            ElseIf result.IsSuccess Then
                For Each restriction As Restriction In result.Restrictions
                    Select Case restriction.Type
                        Case RestrictionEnum.LockGral
                            Dim index As Integer = 0
                            While index < restriction.Xml.Count()
                                Dim note As String = String.Format("Sincronizar request numero {0} LockGral Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                                index += 1
                            End While
                        Case RestrictionEnum.LockRatePlan
                            Dim index As Integer = 0
                            While index < restriction.Xml.Count()
                                Dim note As String = String.Format("Sincronizar request numero {0} LockRatePlan Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                                index += 1
                            End While
                        Case RestrictionEnum.LockRoomType
                            Dim index As Integer = 0
                            While index < restriction.Xml.Count()
                                Dim note As String = String.Format("Sincronizar request numero {0} LockRoomtype Conflux con el hotel: ", (index + 1))
                                Log(note, restriction.Xml(index).ToString(), hotelId, requestXMl:=restriction.XmlRequest(index).ToString())
                                index += 1
                            End While
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
