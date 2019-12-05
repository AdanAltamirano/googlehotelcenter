Namespace API.Models
    Public Class HotelConfig
        Public Property idProperty As Integer
        Public Property nameProperty As String
        Public Property message As String
        Public Property status As Integer
    End Class

    Public Class RatesConfig
        Public Property id As String
        Public Property listProperties As List(Of HotelConfig)
        'Public Property portals As String()
    End Class

    Public Class NotificationConfig
        Public Property email As String
        Public Property reservationNumber As Integer
    End Class
End Namespace
