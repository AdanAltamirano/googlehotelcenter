Public Class Rate

    Private start As String
    Public Property StartDate() As String
        Get
            Return start
        End Get
        Set(ByVal value As String)
            start = value
        End Set
    End Property

    Private [end] As String
    Public Property EndDate() As String
        Get
            Return [end]
        End Get
        Set(ByVal value As String)
            [end] = value
        End Set
    End Property

    Private bseGuestAmountList As List(Of BaseGuestAmount)
    Public Property BaseGuestAmountsList() As List(Of BaseGuestAmount)
        Get
            Return bseGuestAmountList
        End Get
        Set(ByVal value As List(Of BaseGuestAmount))
            bseGuestAmountList = value
        End Set
    End Property

End Class
