Public Class BaseGuestAmount

    Private amtBeforeTax As String
    Public Property AmountBeforeTax() As String
        Get
            Return amtBeforeTax
        End Get
        Set(ByVal value As String)
            amtBeforeTax = value
        End Set
    End Property

End Class

