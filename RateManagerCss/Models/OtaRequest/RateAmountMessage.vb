Public Class RateAmountMessage

    Private statusControl As StatusApplicationControl
    Public Property StatusApplicationControl() As StatusApplicationControl
        Get
            Return statusControl
        End Get
        Set(ByVal value As StatusApplicationControl)
            statusControl = value
        End Set
    End Property

    Private rateAmtMsgList As List(Of Rate)
    Public Property RateAmountMessageList() As List(Of Rate)
        Get
            Return rateAmtMsgList
        End Get
        Set(ByVal value As List(Of Rate))
            rateAmtMsgList = value
        End Set
    End Property
End Class
