Imports APIServices.Models

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

        Function GetPromosByRatePlan(ByVal hotelId As Integer, ByVal ratePlanId As String) As List(Of spGetPromosByRatePlan_Result)

            Dim promos As List(Of spGetPromosByRatePlan_Result) = New List(Of spGetPromosByRatePlan_Result)

            Using dbContext As New OzHotelesEntities()

                promos = dbContext.spGetPromosByRatePlan(hotelId, ratePlanId).ToList()

            End Using

            Return promos

        End Function

    End Module
End Namespace
