Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports APIServices.Models

Namespace API.Helpers
    ''' <summary>
    ''' Atributo para validar si el hotel que se está solicitando en la api 
    ''' está permitido para el usuario en sesión
    ''' </summary>
    Public Class AuthorizeUserAttribute
        Inherits AuthorizeAttribute

        Protected Overrides Function IsAuthorized(actionContext As HttpActionContext) As Boolean
            Dim userId As Integer? = UserDataHelper.GetUserId()
            Dim userRoles() As String = UserDataHelper.GetRoles()

            'no tengo id de usuario o no tiene roles -> no esta logueado
            If userId Is Nothing OrElse userRoles.Length = 0 Then
                Return False
            End If

            HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = True

            Return AuthorizeByRol(userRoles) AndAlso AuthorizeHotelAccess(actionContext, userId)
        End Function

        Public Function AuthorizeByRol(userRoles() As String) As Boolean
            Dim uvRoles() As String = If(Roles Is Nothing, String.Empty, Roles).Split(",").Select(Function(r) r.Trim().ToLower()).ToArray()
            Return uvRoles.Length = 0 OrElse userRoles.Any(Function(ur) uvRoles.Contains(ur))
        End Function

        Public Function AuthorizeHotelAccess(ActionContext As HttpActionContext, userId As Integer) As Boolean

            Dim hotelId As Integer

            'no hay id de hotel a evaluar por lo que consideramos autorizado
            If Not Integer.TryParse(If(ActionContext.ActionArguments.ContainsKey("HotelId"), ActionContext.ActionArguments("HotelId").ToString(), ""), hotelId) Then
                Return True
            End If

            If Roles.Contains("supervisor") Then
                Return True

                'usuario cadena tiene derecho a todos los hoteles de su corporativo
            ElseIf Roles.Contains("userchain") Then
                Dim userCorpId As Integer? = UserDataHelper.GetUserCorpId(userId)

                Using db As New OzHotelesEntities()
                    'si algún hotel que tiene el id solicitado y coincide con el corportivo del usuario 
                    'entonces tiene permisos 
                    If db.vHotelBasicInfo.Any(Function(h) h.Id = hotelId AndAlso (Not h.CorpId Is Nothing) AndAlso h.CorpId = userCorpId) Then
                        Return True
                    End If

                End Using


            ElseIf Roles.Contains("hotelcompany") Then
                'usuario de hotel tiene derecho a los hoteles que tenga asignados
                If UserDataHelper.GetUserHotels(userId).Any(Function(h) h.HotelId = hotelId) Then
                    Return True
                End If
            End If

            Return False

        End Function
    End Class
End Namespace

