Public Class StatusApplicationControl

    Private rtePlanCode As String
    Public Property RatePlanCode() As String
        Get
            Return rtePlanCode
        End Get
        Set(ByVal value As String)
            rtePlanCode = value
        End Set
    End Property

    Private roomTypeCode As String
    Public Property InvTypeCode() As String
        Get
            Return roomTypeCode
        End Get
        Set(ByVal value As String)
            roomTypeCode = value
        End Set
    End Property

End Class
