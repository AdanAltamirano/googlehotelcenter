Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess

Namespace Utitlities.Hotel
    Public Module HotelUtilitie
        Function IsEnableGoogleRequest(ByVal hotelId As Integer) As Boolean
            Dim dsHotel As HotelDatos = New HotelSistema().GetHotelById(hotelId)
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                If Not .IsNull(dsHotel.FIELD_ENABLE_GOOGLE) Then
                    Return .Item(dsHotel.FIELD_ENABLE_GOOGLE)
                End If
            End With
            Return False
        End Function
    End Module
End Namespace
