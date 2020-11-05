Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.UI.WebControls
Imports APIServices
Imports APIServices.Models
Imports NinjAPI
Imports NinjAPI.Query
Imports RateManager.API.Helpers
Imports RateManager.PaginaBase
Imports System.IO
Imports System.Linq
Imports APIServices.Utilities
Imports APIServices.Models.DTO
Imports System.Threading

Namespace API.Controller
    <RoutePrefix("api/reservations"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany,agencycompany")>
    Public Class ReservationController
        Inherits ShurikenController

        Public ReservationService As New ReservationService

        'GET api/reservations
        <Route(""), HttpGet, Queryable>
        Public Function GetAll() As IQueryable(Of vReservation)

            Dim roles() As String = GetRoles()
            If roles.Contains("supervisor") Then
                Return ReservationService.GetAll()

            ElseIf roles.Contains("userchain") Then
                Dim page As New PaginaBase
                If page.CorporateId <> 0 And IsNothing(page.CorporateName) <> True Then
                    If page.CorporateName.Contains(":") Then
                        'Return ReservationService.GetAllGalileo(page.CorporateName.Split(New Char() {":"})(1))
                        'Dim corporate As String = page.CorporateName.Split(New Char() {":"})(1)
                        'Return ReservationService.GetAll().Where(Function(h) h.Hotel.Contains(corporate) And h.Provider = "IDISO")
                        Return ReservationService.GetAll().Where(Function(h) h.CorporateId = page.CorporateId)
                    End If
                End If
                Dim userCorpId As Integer = GetUserCorpId(GetUserId().Value)

                Return ReservationService.GetAll().Where(Function(h) h.CorporateId = userCorpId)

            ElseIf roles.Contains("hotelcompany") Then
                Dim hotels() As Integer = GetUserHotels(GetUserId().Value).Select(Function(h) h.HotelId).ToArray()
                Return ReservationService.GetAll().Where(Function(h) hotels.Contains(h.HotelId) And h.Provider = "INTERNET POWER")
            ElseIf roles.Contains("agencycompany") Then
                Dim page As New PaginaBase
                If page.IsAgencyCompany Then
                    Dim userId As Integer = page.UserIdentityName
                    Return ReservationService.GetAll().Where(Function(h) h.AgencyUserId = userId)
                End If
            End If

            Return New vReservation() {}.AsQueryable()
        End Function


        'GET api/reservations/corporate
        <Route("corporate"), HttpGet>
        Public Function GetCorporate() As IQueryable(Of Corporativos)
            Dim roles() As String = GetRoles()
            If roles.Contains("supervisor") Then
                Return ReservationService.GetCorporate()
            End If
            Return New Corporativos() {}.AsQueryable()
        End Function

        'GET api/reservations/excel
        <Route("excel"), HttpGet>
        Public Function GetExcel() As HttpResponseMessage
            Dim roles() As String = GetRoles()
            If roles.Contains("userchain") Then
                Dim page As New PaginaBase
                If page.CorporateId <> 0 And IsNothing(page.CorporateName) <> True Then
                    If page.CorporateName.Contains(":") Then
                        Dim parserG = New QueryParser()
                        Dim _queryG As QueryData = parserG.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
                        Dim queryResultG As IQueryable(Of vReservation)
                        Dim corporate As String = page.CorporateName.Split(New Char() {":"})(1)
                        'queryResultG = _queryG.ApplyTo(ReservationService.GetAllGalileo(page.CorporateName.Split(New Char() {":"})(1)))
                        queryResultG = _queryG.ApplyTo(ReservationService.GetAll().Where(Function(h) h.CorporateId = page.CorporateId))
                        Dim responseG As New HttpResponseMessage
                        responseG = ReservationService.GetExcel(queryResultG)
                        Return responseG
                    End If
                End If
            ElseIf roles.Contains("agencycompany") Then
                Dim page As New PaginaBase
                If page.IsAgencyCompany Then
                    Dim userId As Integer = page.UserIdentityName
                    Dim parserG = New QueryParser()
                    Dim _queryG As QueryData = parserG.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
                    Dim queryResultG As IQueryable(Of vReservation)
                    queryResultG = _queryG.ApplyTo(ReservationService.GetAll().Where(Function(h) h.AgencyUserId = userId))
                    Dim responseG As New HttpResponseMessage
                    responseG = ReservationService.GetExcel(queryResultG)
                    Return responseG
                End If
            End If
            Dim parser = New QueryParser()
            Dim _query As QueryData = parser.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
            Dim queryResult As IQueryable(Of vReservation)
            queryResult = _query.ApplyTo(ReservationService.GetAll())
            Dim response As New HttpResponseMessage
            response = ReservationService.GetExcel(queryResult)
            Return response
        End Function

        'GET api/reservations/1978
        <Route("{reservationId:Int}"), HttpGet>
        Public Function GetDetails(ByVal reservationId As Integer) As DTO.ReservationDetailsModel
            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")
            Dim page As New PaginaBase
            Dim isUserChainIdiso = False
            If page.CorporateId <> 0 And IsNothing(page.CorporateName) <> True Then
                If page.CorporateName.Contains(":") Then
                    isUserChainIdiso = True
                    Return ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)
                End If
            End If
            Return ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)
        End Function

        'POST api/reservations/1978/cancel
        <Route("{reservationId:int}/cancel"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.CancelBookingRQ) As DTO.CancelBookingRS

            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")
            Dim isUserChainIdiso = False
            Dim rsv As vReservationDetails = ReservationService.GetReservation(reservationId)
            Dim result As DTO.CancelBookingRS = ReservationService.Cancel(rsv, GetUserId().Value, req.Reason)
            If result.IsSuccess Then
                Log(reservationId, acciones.Eliminar, rsv.hotelId)

                'no enviar correo de cancelación si está en proceso
                'If rsv.status <> 4 Then
                rsv = ReservationService.GetReservation(reservationId)

                Dim roomRsv As List(Of vReservationRoomDetails) = ReservationService.GetRoomsReservation(reservationId)

                Dim rdm As ReservationDetailsModel =
                    ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)

                If Not String.IsNullOrEmpty(rsv.customerEmail) Then
                    'enviar correo al cliente
                    Dim errorMail As String = String.Empty
                    If SendCancellationEmail(rdm,rsv.customerEmail, errorMail) Then
                        result.CustomerEmail = rsv.customerEmail
                    Else

                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Eliminar, rsv.hotelId, xmlError, xmlError)
                    End If

                End If
                If Not String.IsNullOrEmpty(rsv.hotelEmail) Then
                    'enviar correo al hotel
                    Dim errorMail As String = String.Empty
                    If SendCancellationEmail(rdm, rsv.hotelEmail, errorMail) Then
                        result.HotelEmail = rsv.hotelEmail
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Eliminar, rsv.hotelId, xmlError, xmlError)
                    End If
                End If
                If String.IsNullOrEmpty(rsv.customerEmail) AndAlso String.IsNullOrEmpty(rsv.hotelEmail) Then
                    'enviar correo a algun admin
                    Dim errorMail As String = String.Empty
                    SendCancellationEmail(rdm, "soporte@internetpowerhotel.com", errorMail)
                End If
                'End If
            End If
            Return result
        End Function

        'POST api/reservations/1978/modify
        <Route("{reservationId:int}/modify"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.ModifyBookingRQ) As DTO.ModifyBookingRS

            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")
            Dim isUserChainIdiso = False
            Dim oldData_RDM As ReservationDetailsModel =
                ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)

            Dim result As DTO.ModifyBookingRS = ReservationService.Modify(reservationId, req)

            If result.IsSuccess Then

                Dim updatedData_RDM As ReservationDetailsModel =
                    ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)
                Dim xmlOld As String = Utilities.GetXML(oldData_RDM)
                Dim xmlCurrent As String = Utilities.GetXML(updatedData_RDM)
                Log(reservationId, acciones.Modificar, updatedData_RDM.HotelId, xmlOld, xmlCurrent)

                'If updatedData_RDM.Status <> 4 Then
                If Not String.IsNullOrEmpty(updatedData_RDM.Customer.Email) Then
                    'enviar correo al cliente
                    Dim errorMail As String = String.Empty
                    If SendModificationEmail(updatedData_RDM.Customer.Email, updatedData_RDM, oldData_RDM, errorMail, TypeClient.Customer) Then
                        result.CustomerEmail = updatedData_RDM.Customer.Email
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Modificar, updatedData_RDM.HotelId, xmlError, xmlError)
                    End If
                End If
                If Not String.IsNullOrEmpty(updatedData_RDM.HotelEmail) Then
                    'enviar correo al hotel
                    Dim errorMail As String = String.Empty
                    If SendModificationEmail(updatedData_RDM.HotelEmail, updatedData_RDM, oldData_RDM, errorMail, TypeClient.Hotel) Then
                        result.HotelEmail = updatedData_RDM.HotelEmail
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Modificar, updatedData_RDM.HotelId, xmlError, xmlError)
                    End If
                End If
                If String.IsNullOrEmpty(updatedData_RDM.Customer.Email) AndAlso String.IsNullOrEmpty(updatedData_RDM.HotelEmail) Then
                    'enviar correo a algun admin
                    Dim errorMail As String = String.Empty
                    SendModificationEmail("soporte@internetpowerhotel.com", updatedData_RDM, oldData_RDM, errorMail, TypeClient.Support)
                End If
                ' End If
            End If
            Return result
        End Function

        'POST api/reservations/1978/reactivate
        <Route("{reservationId:int}/reactivate"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> rm As Boolean) As DTO.ModifyBookingRS
            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")
            Dim isUserChainIdiso = False
            Dim result As DTO.ModifyBookingRS = ReservationService.Reactivate(reservationId, rm)

            If result.IsSuccess Then

                Dim rdm As ReservationDetailsModel =
                    ReservationService.GetDetails(reservationId, IsSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)
                Log(reservationId, acciones.Reactivar, rdm.HotelId)

                If Not String.IsNullOrEmpty(rdm.Customer.Email) Then
                    'enviar correo al cliente
                    Dim errorMail As String = String.Empty
                    If SendReactivationEmail(rdm, rdm.Customer.Email, errorMail) Then
                        result.CustomerEmail = rdm.Customer.Email
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Reactivar, rdm.HotelId, xmlError, xmlError)
                    End If
                End If
                If Not String.IsNullOrEmpty(rdm.HotelEmail) Then
                    'enviar correo al hotel
                    Dim errorMail As String = String.Empty
                    If SendReactivationEmail(rdm, rdm.HotelEmail, errorMail) Then
                        result.HotelEmail = rdm.HotelEmail
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Reactivar, rdm.HotelId, xmlError, xmlError)
                    End If
                End If
                If String.IsNullOrEmpty(rdm.Customer.Email) AndAlso String.IsNullOrEmpty(rdm.HotelEmail) Then
                    'enviar correo a algun admin
                    Dim errorMail As String = String.Empty
                    SendReactivationEmail(rdm, "soporte@internetpowerhotel.com", errorMail)
                End If
            End If

            Return result
        End Function
        'GET api/reservations/1978/creditcard
        <Route("{reservationId:int}/creditcard"), HttpGet>
        Public Function GetCode(ByVal reservationId As Integer)
            Dim code As String = ReservationService.GetCode(10)
            HttpContext.Current.Session("code_cc") = code

            Return Ok(New With {Key .success = SendVerificationCodeEmail(code)})
        End Function

        'GET api/reservations/1978/creditcard/1234
        <Route("{reservationId:int}/creditcard/{code}"), HttpGet>
        Public Function GetCreditCard(ByVal reservationId As Integer, ByVal code As String) As DTO.CardDetails
            Dim generatedCode As String = ""
            If HttpContext.Current.Session("code_cc") IsNot Nothing Then
                generatedCode = HttpContext.Current.Session("code_cc")
            End If
            If (code = generatedCode) Then
                HttpContext.Current.Session("code_cc") = Nothing
                Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

                Return ReservationService.GetCreditCardDetails(reservationId, isHotelCompany, GetUserId().Value)
            End If
            Return New DTO.CardDetails()
        End Function

        'GET api/reservations/1978/sendnotification
        <Route("{reservationId:int}/sendnotification"), HttpGet>
        Public Function SendNotification(ByVal reservationId As Integer) As HttpResponseMessage
            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")
            Dim isUserChainIdiso = False
            Dim page As New PaginaBase
            If page.CorporateId <> 0 And IsNothing(page.CorporateName) <> True Then
                If page.CorporateName.Contains(":") Then
                    isUserChainIdiso = True
                End If
            End If
            Dim detailsReservation As DTO.ReservationDetailsModel = ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, isUserChainIdiso, GetUserId().Value)
            'Enviar Correo
            'Dim logoUrl As String = "https://crs.univisit.com/Images/global/ImagesSystem/Logos/LogoCompany_"
            Dim logoUrl As String = System.Configuration.ConfigurationManager.AppSettings("logoUrl")
            'Dim logoUrl As String = "http://test.univisit.com/RateManager/ozportalglobal/ImagesSystem/Logos/LogoCompany_"
            'Dim displayReservation As String = "https://secure.internetpower.com.mx/portals/application/hotel/secure/FindBooking.aspx?ReturnUrl=/portals/application/hotel/Secure/DisplayReservation.aspx?ConfirmNum="
            Dim isSent As Boolean
            Dim template As String = ReservationService.GetTemplate(detailsReservation, logoUrl)
            Dim HotelConfig As New HotelConfigurationService
            Dim emails As String = HotelConfig.Emails(detailsReservation.HotelId)
            emails = emails & "," & detailsReservation.Customer.Email
            isSent = SendNotificationEmail(template, emails, detailsReservation.ReservationNumber)
            If isSent Then
                Dim ok = New HttpResponseMessage(Net.HttpStatusCode.OK)
                Return ok
            End If
            Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", "No se envío el correo")
            Return BadRequest(result)
        End Function

        Sub Log(ByVal reservationId As Integer, ByVal action As acciones, Optional ByVal hotelId As Integer = 0, Optional ByVal oldData As String = "",
                Optional ByVal currentData As String = "")
            'Dim pb As New PaginaBase()
            Dim msg As String = ""
            Select Case action
                Case acciones.Eliminar
                    msg = "Canceló la reserva #" & reservationId
                Case acciones.Modificar
                    msg = "Modifico la reserva #" & reservationId
                Case acciones.Reactivar
                    msg = "Reactivo la reserva #" & reservationId
            End Select
            'pb.guardalog("/rate-manager-ui/dist/reservation-details.aspx?qs=" & reservationId, action, msg, "", oldData, currentData, hotelId)
            With (New PaginaBase)
                .guardalog("/rate-manager-ui/dist/reservation-details.aspx?qs=" & reservationId, action, msg, "", oldData, currentData, hotelId)
            End With
        End Sub
        Function GetQuery(request As HttpRequestMessage, actionContext As Http.Controllers.HttpActionContext) As IQueryable
            Dim parser = New QueryParser()
            Dim _query As QueryData = parser.CreateAndValidateQuery(request, actionContext, "reservationId")
            Dim queryResult As IQueryable
            queryResult = _query.ApplyTo(ReservationService.GetAll())
            Return queryResult
        End Function
    End Class
End Namespace

