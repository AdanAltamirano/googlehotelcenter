Imports System.Collections.Generic

Namespace API.Models
    Public Class RoomClosureRQ
        Public Property IdHotel As Integer

        Public Property [Dates] As List(Of DatesClosure)

        'Public Property StartDate As Date

        'Public Property EndDate As Date

        Public Property RatePlanOption As String

        Public Property RoomOption As String

        Public Property Status As String

    End Class

    Public Class DatesClosure
        Public Property StartDate As Date
        Public Property EndDate As Date
    End Class

End Namespace
