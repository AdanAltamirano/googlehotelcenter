Imports System.Runtime.Serialization


Public Class Reservationes
    Inherits PaginaBase
    Public Property Reservacion() As Reserva
        Get
            Return Session("_Res")
        End Get
        Set(ByVal Value As Reserva)
            Session("_Res") = Value
        End Set
    End Property
End Class
<Serializable()> _
Public Class Reserva
    Private cRooms As _Rooms
    Private _checkin As Date
    Private _checkout As Date
    Public Property Rooms() As _Rooms
        Get
            Return cRooms
        End Get
        Set(ByVal Value As _Rooms)
            cRooms = Value
        End Set
    End Property
    Public Property Checkin() As Date
        Get
            Return _checkin
        End Get
        Set(ByVal Value As Date)
            _checkin = Value
        End Set
    End Property
    Public Property Checkout() As Date
        Get
            Return _checkout
        End Get
        Set(ByVal Value As Date)
            _checkout = Value
        End Set
    End Property
End Class


<Serializable()> _
Public Class _Rooms
    Inherits CollectionBase
    Public Sub New()
    End Sub

    Public Function Add(ByVal Room As room) As Integer
        Return MyBase.InnerList.Add(Room)
    End Function

    Public Property Item(ByVal Index As Integer) As room
        Get
            Return MyBase.InnerList.Item(Index)
        End Get

        Set(ByVal Value As room)
            MyBase.InnerList.Item(Index) = Value
        End Set
    End Property
End Class

<Serializable()> Public Class room
    Implements ISerializable

    Public Adults As Integer
    Public Childs As Integer
    Public RateCode As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        With info
            Me.Adults = .GetValue("Adultos", GetType(String))
            Me.Childs = .GetValue("Ninios", GetType(String))
            Me.RateCode = .GetValue("Ratecode", GetType(String))
        End With
    End Sub

    Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        With info
            .AddValue("Adultos", Me.Adults, GetType(Integer))
            .AddValue("Ninios", Me.Childs, GetType(Integer))
            .AddValue("Ratecode", Me.RateCode, GetType(String))
        End With
    End Sub

End Class
