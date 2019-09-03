Namespace API.Models
    Public Class ReservationUpdateRQ
        Public Property Oper As Method
        Public Property Cancel As RsvCancel
    End Class


    Public Class RsvCancel
        Public Property Reason As String
    End Class

    Public Enum Method
        Cancel
        Modify
    End Enum
End Namespace

