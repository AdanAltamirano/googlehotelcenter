Imports APIServices.Models

Namespace API.Helpers
    Public Module UserDataHelper

        Public Function GetUserEmail() As String
            Return HttpContext.Current.Session("EmailLoginUser").ToString
        End Function

        Public Function GetUserId() As Integer?
            Dim userId As Integer

            'no esta logueado
            If Integer.TryParse(HttpContext.Current.Session("idUsuario"), userId) Then
                Return userId
            End If

            Return Nothing
        End Function

        Public Function GetRoles() As String()

            Dim rolesStr As String = HttpContext.Current.Session("RolesUsuario")
            'no tiene roles
            If String.IsNullOrWhiteSpace(rolesStr) Then
                Return New String(0) {}
            End If

            Return rolesStr.Split(",").Select(Function(x) x.Trim().ToLower()).ToArray()
        End Function

        Public Function GetUserCorpId(userId As Integer) As Integer?
            Using db As New OzHotelesEntities
                Return db.vAdministrator.FirstOrDefault(Function(a) a.UserId = userId AndAlso a.Type = 9)?.CorpId 'no se que es el tipo 9 TODO: crear un enum
            End Using
        End Function

        Public Function GetUserHotels(userId As Integer) As vHotelByUser()
            Using db As New OzHotelesEntities()
                'no tiene permisos para el hotel, porque ninguno de los asignados es el que está solicitando
                Return db.vHotelByUser.Where(Function(h) h.UserId = userId).ToArray()
            End Using

        End Function

    End Module
End Namespace

