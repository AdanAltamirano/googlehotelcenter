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
Imports APIServices.Models.DTO.Reservation.Deposit.Request
Imports APIServices.Models.DTO.Reservation.Deposit.Response
Imports APIServices.Models.DTO.Reservation.Pms.Response
Imports RateManager.Utitlities.Email
Imports System.Threading
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports RateManager.Utitlities.XML
Imports APIServices.Helpers.Reservation
Imports APIServices.Service.HotelVerse.Models.Response
Imports APIServices.Service.HotelVerse

Namespace API.Controller
    <RoutePrefix("api/reservations"), AuthorizeUser(Roles:="supervisor,userchain,hotelcompany,agencycompany,usuariohotel")>
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
                        Return ReservationService.GetAll().Where(Function(h) h.CorporateId = page.CorporateId And h.Status <> 4)
                    End If
                End If
                Dim userCorpId As Integer = GetUserCorpId(GetUserId().Value)

                Return ReservationService.GetAll().Where(Function(h) h.CorporateId = userCorpId And h.Status <> 4)

            ElseIf roles.Contains("hotelcompany") Then
                Dim hotels() As Integer = GetUserHotels(GetUserId().Value).Select(Function(h) h.HotelId).ToArray()
                Return ReservationService.GetAll().Where(Function(h) hotels.Contains(h.HotelId) And h.Provider = "INTERNET POWER")
            ElseIf roles.Contains("usuariohotel") Then

                Dim userOzhoteles As UsuarioHotel = GetUserFromOzHoteles(GetUserId().Value)
                Dim hotels() As Integer = GetUserHotels(userOzhoteles.IdMainUser).Select(Function(h) h.HotelId).ToArray()
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
            If roles.Contains("supervisor") Then
                Dim parserS = New QueryParser()
                Dim _queryS As QueryData = parserS.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
                Dim queryResultS As IQueryable(Of vReservation)
                queryResultS = _queryS.ApplyTo(ReservationService.GetAll())
                Dim responseS As New HttpResponseMessage
                responseS = ReservationService.GetExcel(queryResultS)
                Return responseS
            ElseIf roles.Contains("userchain") Then
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
                Dim userCorpId As Integer = GetUserCorpId(GetUserId().Value)
                Dim parserCorporate = New QueryParser()
                Dim _queryCorporate As QueryData = parserCorporate.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
                Dim queryResultCorporate As IQueryable(Of vReservation)
                queryResultCorporate = _queryCorporate.ApplyTo(ReservationService.GetAll().Where(Function(h) h.CorporateId = userCorpId))
                Dim responseCorporate As New HttpResponseMessage
                responseCorporate = ReservationService.GetExcel(queryResultCorporate)
                Return responseCorporate
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
            ElseIf roles.Contains("hotelcompany") Then
                Dim hotels() As Integer = GetUserHotels(GetUserId().Value).Select(Function(h) h.HotelId).ToArray()
                Dim parserHotelCompany = New QueryParser()
                Dim _queryHotelCompany As QueryData = parserHotelCompany.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
                Dim queryResultHotelCompany As IQueryable(Of vReservation)
                queryResultHotelCompany = _queryHotelCompany.ApplyTo(ReservationService.GetAll().Where(Function(h) hotels.Contains(h.HotelId) And h.Provider = "INTERNET POWER"))
                Dim responseHotelCompnay As New HttpResponseMessage
                responseHotelCompnay = ReservationService.GetExcel(queryResultHotelCompany)
                Return responseHotelCompnay
            ElseIf roles.Contains("usuariohotel") Then
                Dim userOzhoteles As UsuarioHotel = GetUserFromOzHoteles(GetUserId().Value)
                Dim hotels() As Integer = GetUserHotels(userOzhoteles.IdMainUser).Select(Function(h) h.HotelId).ToArray()
                Dim queryParser = New QueryParser()
                Dim _queryUserHotel As QueryData = queryParser.CreateAndValidateQuery(ActionContext, "reservationId", GetType(vReservation))
                Dim queryResultUserHotel As IQueryable(Of vReservation)
                queryResultUserHotel = _queryUserHotel.ApplyTo(ReservationService.GetAll().Where(Function(h) hotels.Contains(h.HotelId) And h.Provider = "INTERNET POWER"))
                Dim responseHotelUser As New HttpResponseMessage
                responseHotelUser = ReservationService.GetExcel(queryResultUserHotel)
                Return responseHotelUser
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
            Dim isUserChain As Boolean = GetRoles().Contains("userchain")
            Dim isUsuarioHotelAssociation = GetRoles().Contains("userassociation")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

            Dim paginaBase As New PaginaBase
            Dim idCorporateUserChain As Integer = -1
            Dim idAsociationPb As Integer = 0


            Dim dsReservaciones As ReservaDatos
            Dim idHotel As Integer = 0
            Dim idCorporatePortal As Integer = -1
            Dim idAsociation As Integer = -1

            With New ReservaFacade
                dsReservaciones = .GetDataReserva(reservationId, PortalCulture.GetIDCulture())
            End With

            If Not dsReservaciones Is Nothing Then

                With dsReservaciones.Tables(dsReservaciones.RESERVA_TABLE).Rows(0)
                    idCorporatePortal = CType(.Item("IdCorporativoPortal"), Integer)
                    idAsociation = If(Not .IsNull("idAsociacion"), .Item("idAsociacion"), 0)
                    idHotel = CType(.Item("idHotel"), Integer)
                End With
            End If

            idCorporateUserChain = paginaBase.GetIdCorporativoUserChain(idHotel)
            idAsociationPb = paginaBase.GetIdAsociation(GetUserId().Value)

            Return ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, GetUserId().Value, isUserChain, isUsuarioHotelAssociation, idCorporateUserChain, idCorporatePortal, idAsociationPb, idAsociation)
        End Function

        'Get api/reservations/1978/history/log
        <Route("{reservationId:int}/history/log"), HttpGet>
        Public Function GetReservationHistoryLog(ByVal reservationId As Integer) As IEnumerable(Of spReservationLog_Result2)

            Dim logs = ReservationService.GetReservationHistoryLog(reservationId.ToString())

            Return logs
        End Function

        'POST api/reservations/1978/cancel
        <Route("{reservationId:int}/cancel"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> req As DTO.CancelBookingRQ) As DTO.CancelBookingRS

            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

            Dim rsv As vReservationDetails = ReservationService.GetReservation(reservationId)
            Dim result As DTO.CancelBookingRS = ReservationService.Cancel(rsv, GetUserId().Value, req.Reason, isSupervisor)

            If result.IsSuccess Then


                Log(reservationId, acciones.Eliminar, rsv.hotelId, motivo:=req.Reason, nota:=String.Format("Canceló la reserva #{0}", reservationId))

                Dim reservation As Reservaciones = ReservationHelper.GetReservation(reservationId)

                If reservation.idAgencia IsNot Nothing And reservation.idAgencia = 211 Then

                    Dim response As ResponseRequest = HotelVerseService.CancelReservation(reservation.RecordLocator)

                    Dim xml As String = XmlUtilitie.ToXmlString(response)

                    Log(reservationId, acciones.Eliminar, rsv.hotelId, motivo:="", currentData:=xml, nota:=String.Format("Canceló la reserva Hotel Verse #{0}", reservationId))

                End If



                'no enviar correo de cancelación si está en proceso
                'If rsv.status <> 4 Then
                rsv = ReservationService.GetReservation(reservationId)

                Dim roomRsv As List(Of vReservationRoomDetails) = ReservationService.GetRoomsReservation(reservationId)

                Dim rdm As ReservationDetailsModel =
                    ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, GetUserId().Value)

                If Not String.IsNullOrEmpty(rsv.customerEmail) Then
                    'enviar correo al cliente
                    Dim errorMail As String = String.Empty
                    If SendCancellationEmail(rdm, rsv.customerEmail, errorMail) Then
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

            Dim sendNotification As Boolean = IIf(Me.Request.Headers.GetValues("Notification")(0).Equals("1"), True, False)

            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

            Dim oldData_RDM As ReservationDetailsModel =
                ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, GetUserId().Value)

            Dim result As DTO.ModifyBookingRS = ReservationService.Modify(reservationId, req)

            If result.IsSuccess Then

                Dim updatedData_RDM As ReservationDetailsModel =
                    ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, GetUserId().Value)
                Dim xmlOld As String = Utilities.GetXML(oldData_RDM)
                Dim xmlCurrent As String = Utilities.GetXML(updatedData_RDM)

                Log(reservationId, acciones.Modificar, updatedData_RDM.HotelId, xmlOld, xmlCurrent, motivo:=req.Details)

                result.SendNotification = sendNotification

                If sendNotification Then

                    Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

                    'If updatedData_RDM.Status <> 4 Then
                    If Not String.IsNullOrEmpty(updatedData_RDM.Customer.Email) Then
                        'enviar correo al cliente
                        Dim errorMail As String = String.Empty
                        If SendModificationEmail(updatedData_RDM.Customer.Email, updatedData_RDM, oldData_RDM, errorMail, TypeClient.Customer, info) Then
                            result.CustomerEmail = updatedData_RDM.Customer.Email
                        Else
                            Dim xmlError As String = Utilities.GetXML(errorMail)
                            Log(reservationId, acciones.Modificar, updatedData_RDM.HotelId, xmlError, xmlError)
                        End If
                    End If
                    If Not String.IsNullOrEmpty(updatedData_RDM.HotelEmail) Then
                        'enviar correo al hotel
                        Dim errorMail As String = String.Empty

                        If SendModificationEmail(updatedData_RDM.HotelEmail, updatedData_RDM, oldData_RDM, errorMail, TypeClient.Hotel, info) Then
                            result.HotelEmail = updatedData_RDM.HotelEmail
                        Else
                            Dim xmlError As String = Utilities.GetXML(errorMail)
                            Log(reservationId, acciones.Modificar, updatedData_RDM.HotelId, xmlError, xmlError)
                        End If
                    End If
                    If String.IsNullOrEmpty(updatedData_RDM.Customer.Email) AndAlso String.IsNullOrEmpty(updatedData_RDM.HotelEmail) Then
                        'enviar correo a algun admin
                        'Dim errorMail As String = String.Empty
                        'SendModificationEmail("soporte@internetpowerhotel.com", updatedData_RDM, oldData_RDM, errorMail, TypeClient.Support)
                    End If
                    'End If
                End If
            End If

            Return result
        End Function

        'POST api/reservations/1978/reactivate
        <Route("{reservationId:int}/reactivate"), HttpPost>
        Public Function Update(ByVal reservationId As Integer, <FromBody> rm As Boolean) As DTO.ModifyBookingRS
            Dim isSupervisor As Boolean = GetRoles().Contains("supervisor")
            Dim isHotelCompany As Boolean = GetRoles().Contains("hotelcompany")

            Dim result As DTO.ModifyBookingRS = ReservationService.Reactivate(reservationId, rm)

            If result.IsSuccess Then

                Dim rdm As ReservationDetailsModel =
                    ReservationService.GetDetails(reservationId, isSupervisor, isHotelCompany, GetUserId().Value)

                Log(reservationId, acciones.Reactivar, rdm.HotelId)

                Dim info As companyInfo = CType(HttpContext.Current.Session("infoCompany"), companyInfo)

                If Not String.IsNullOrEmpty(rdm.Customer.Email) Then
                    'enviar correo al cliente
                    Dim errorMail As String = String.Empty
                    If SendReactivationEmail(rdm, rdm.Customer.Email, errorMail, TypeClient.Customer, info) Then
                        result.CustomerEmail = rdm.Customer.Email
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Reactivar, rdm.HotelId, xmlError, xmlError)
                    End If
                End If
                If Not String.IsNullOrEmpty(rdm.HotelEmail) Then
                    'enviar correo al hotel
                    Dim errorMail As String = String.Empty
                    If SendReactivationEmail(rdm, rdm.HotelEmail, errorMail, TypeClient.Hotel, info) Then
                        result.HotelEmail = rdm.HotelEmail
                    Else
                        Dim xmlError As String = Utilities.GetXML(errorMail)
                        Log(reservationId, acciones.Reactivar, rdm.HotelId, xmlError, xmlError)
                    End If
                End If
                If String.IsNullOrEmpty(rdm.Customer.Email) AndAlso String.IsNullOrEmpty(rdm.HotelEmail) Then
                    'enviar correo a algun admin
                    Dim errorMail As String = String.Empty
                    SendReactivationEmail(rdm, "soporte@internetpowerhotel.com", errorMail, TypeClient.Customer, info)
                End If
            End If

            Return result
        End Function

        'GET api/reservations/1978/pms/reactivate
        <Route("{reservationId:int}/pms/reactivate"), HttpGet>
        Public Function PmsReactivate(ByVal reservationId As Integer) As HttpResponseMessage

            Dim isReactivated As Boolean = ReservationService.PmsReactivate(reservationId)

            'return empty message
            If Not isReactivated Then
                Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", "Error")
                Return BadRequest(result)
            End If

            'return empty object
            Return Ok(Nothing)

        End Function

        'POST api/reservations/1978/pms/update
        <Route("{reservationId:int}/pms/update"), HttpPost>
        Public Function PmsUpdate(ByVal reservationId As Integer, <FromBody> request As Pms) As HttpResponseMessage


            Dim result As Object = ReservationService.PmsUpdate(reservationId, request)

            If Not result.IsSuccess Then

                Dim [error] As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", "Error")
                Return BadRequest([error])

            End If

            Return Ok(result)
        End Function

        'POST api/reservations/1978/pms/status/update
        <Route("{reservationId:int}/pms/status/update"), HttpPost>
        Public Function PmsStatusUpdate(ByVal reservationId As Integer, <FromBody> request As Pms) As HttpResponseMessage


            Dim result As Object = ReservationService.PmsStatusUpdate(reservationId, request)

            If Not result.IsSuccess Then

                Dim [error] As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", "Error")
                Return BadRequest([error])

            End If

            Return Ok(result)
        End Function

        'POST api/reservations/1978/pms/verify/update
        <Route("{reservationId:int}/pms/verify/update"), HttpPost>
        Public Function PmsVerifyUpdate(ByVal reservationId As Integer, <FromBody> request As Pms) As HttpResponseMessage

            Dim details As vReservationDetails = ReservationService.GetReservation(reservationId)
            Dim hotelId As Integer = details.hotelId
            Dim pmsCodeBefore As String = details.pmsReservationNumber

            Dim result As Object = ReservationService.PmsVerifyUpdate(reservationId, request)

            'GuardarLog

            If Not result.IsSuccess Then
                Dim [error] As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", "Error")
                Return BadRequest([error])
            End If

            Dim page As String = "/HotelAdministrator/Pages/ConfirmReservas.aspx"
            Dim nota As String = String.Empty

            If request.VerifyAction Then
                'Revisar con Chepe si tambien se va a quitar el codigo pms cuando el status sea 1
                nota = "Quitó la confirmación con el codigo de pms " & pmsCodeBefore & " de la reservación " & reservationId
                Logs(hotelId, reservationId, page, acciones.Modificar, nota)
            Else
                nota = "Confirmó la reservación con el código de pms " & request.PmsCode & " de la reservación " & reservationId
                Logs(hotelId, reservationId, page, acciones.Modificar, nota)
            End If

            Return Ok(result)
        End Function


        'POST api/reservations/1978/deposit
        <Route("{reservationId:int}/deposit"), HttpPost>
        Public Function Deposit(ByVal reservationId As Integer, <FromBody> request As ReservationDepositDTO) As ReservationDepositResponse

            request.UserId = GetUserId()

            Dim result As ReservationDepositResponse = ReservationService.DepositUpdate(request)

            If result.IsSuccess Then
                Dim xml As String = Utilities.GetXML(request)
                Log(reservationId, acciones.CrearDeposito, currentData:=xml)
                'EmailDeposit(reservationId.ToString())
            End If

            Return result
        End Function

        'GET api/reservations/1978/creditcard
        <Route("{reservationId:int}/creditcard"), HttpGet>
        Public Function GetCode(ByVal reservationId As Integer)
            Dim code As String = ReservationService.GetCode(10)
            HttpContext.Current.Session("code_cc") = code

            Dim note As String = String.Format("Se solicitó información de tarjeta bancaria para la reserva {0}", reservationId)

            Log(reservationId, acciones.Ver, nota:=note)

            Return Ok(New With {Key .success = SendVerificationCodeEmail(code, reservationId)})
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

            Dim isSent As Boolean = EmailConfirmation(reservationId.ToString())
            If isSent Then
                Dim ok = New HttpResponseMessage(Net.HttpStatusCode.OK)
                Return ok
            End If

            Dim result As KeyValuePair(Of String, String) = New KeyValuePair(Of String, String)("0", "No se envío el correo")
            Return BadRequest(result)

        End Function

        Sub Log(ByVal reservationId As Integer, ByVal action As acciones, Optional ByVal hotelId As Integer = 0, Optional ByVal oldData As String = "",
                Optional ByVal currentData As String = "", Optional ByVal motivo As String = "", Optional ByVal nota As String = "")
            'Dim pb As New PaginaBase()
            Dim msg As String = ""
            Select Case action
                Case acciones.Eliminar
                    msg = nota
                Case acciones.Modificar
                    msg = String.Format("Modifico la reserva #{0}", reservationId)
                Case acciones.Reactivar
                    msg = String.Format("Reactivo la reserva #{0}", reservationId)
                Case acciones.CrearDeposito
                    msg = String.Format("Creación de depósito para la reserva #{0}", reservationId)
                Case acciones.Ver
                    msg = nota
            End Select
            'pb.guardalog("/rate-manager-ui/dist/reservation-details.aspx?qs=" & reservationId, action, msg, "", oldData, currentData, hotelId)
            With (New PaginaBase)
                .guardalog("/rate-manager-ui/dist/reservation-details.aspx?qs=" & reservationId, action, msg, "", oldData,
                           currentData, hotelId, noReservacion:=reservationId.ToString(), motivo:=motivo)
            End With
        End Sub

        Sub Logs(ByVal hotelId As Integer, ByVal reservationId As Integer, ByVal page As String, ByVal action As acciones, Optional ByVal nota As String = "")

            With (New PaginaBase)
                .guardalog(hotelId, page, action, nota, reservationId.ToString())
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

