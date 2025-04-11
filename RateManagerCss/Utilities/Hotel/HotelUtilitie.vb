Imports APIServices.Models

Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess

Namespace Utitlities.Hotel
    Public Module HotelUtilitie

        'Google EndPoints
        Public ENDPOINT As String = ConfigurationManager.AppSettings("confluxApiUrl") & "pms/ota/rates/update"
        Public ENDPOINTDELETE As String = ConfigurationManager.AppSettings("confluxApiUrl") & "pms/ota/rates/delete"
        Public ENDPOINTCLOSURE As String = ConfigurationManager.AppSettings("confluxApiUrl") & "pms/ota/restriction/update"
        'APICache EndPoints
        Public ENDPOINTAPI As String = ""
        Public ENDPOINTAPIDELETE As String = ""
        Public ENDPOINTAPICLOSURE As String = ""

        Public ReadOnly Property ConfluxServiceHelper As APIServices.Conflux.ConfluxService
            Get
                If HttpContext.Current.Session("ConfluxService") Is Nothing Then

                    HttpContext.Current.Session("ConfluxService") = New APIServices.Conflux.ConfluxService()
                End If
                Return CType(HttpContext.Current.Session("ConfluxService"), APIServices.Conflux.ConfluxService)
            End Get
        End Property


        Function IsEnableGoogleRequest(ByVal hotelId As Integer) As Boolean
            Dim dsHotel As HotelDatos = New HotelSistema().GetHotelById(hotelId)
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                If Not .IsNull(dsHotel.FIELD_ENABLE_GOOGLE) Then
                    Return .Item(dsHotel.FIELD_ENABLE_GOOGLE)
                End If
            End With
            Return False
        End Function

        Function IsEnableSendRatesAPICache(ByVal hotelId As Integer) As Boolean
            Dim dsHotel As HotelDatos = New HotelSistema().GetHotelById(hotelId)
            With dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)
                If Not .IsNull(dsHotel.FIELD_SEND_RATES_API_CACHE) Then
                    Return .Item(dsHotel.FIELD_SEND_RATES_API_CACHE)
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
