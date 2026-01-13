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
        Public ENDPOINTINVENTORY As String = ConfigurationManager.AppSettings("confluxApiUrl") & "pms/ota/inventory/update"
        'APICache EndPoints
        Public ENDPOINTAPI As String = ConfigurationManager.AppSettings("confluxApiUrl") & "pms/ota/calendar/rates"
        Public ENDPOINTAPIV2 As String = "pms/ota/calendar/rates"
        Public ENDPOINTAPIDELETE As String = ConfigurationManager.AppSettings("confluxApiUrl") & "cache/rates"
        Public ENDPOINTAPICLOSURE As String = ConfigurationManager.AppSettings("confluxApiUrl") & "pms/ota/calendar/inventory"
        Public ENDPOINTAPICLOSUREV2 As String = "pms/ota/calendar/inventory"
        Public ENDPOINTAPINVENTORY As String = "inventory/ota/update/batch"

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

        Function GetRatePlans(hotelId As String) As RatePlanData
            Return New RatePlanFacade().GetRatePlanByIdHotel(hotelId, idioma:=1, IncluirPaquetesSegmentoK:=1, incluirNetRatesPlan:=1, idAsociacion:=-1, DeleteFilter:=1)
        End Function

        Function GetRatePlanNameById(ByVal rateplanName As String, ByVal hotelId As Integer) As String

            Dim nameRatePlan As String = String.Empty

            Using dbContext As New OzHotelesEntities()
                Dim rateplanTemp As RatesPlan = dbContext.RatesPlan.FirstOrDefault(Function(rateplan) rateplan.idRatePlan = rateplanName And rateplan.IdHotel = hotelId)

                nameRatePlan = dbContext.Diccionario.First(Function(dictionary) dictionary.IdDiccionario = rateplanTemp.IdDiccShortDesc And dictionary.IdIdioma = 1).Texto

            End Using

            Return nameRatePlan
        End Function

        Function GetRatePlansPromos(hotelId As String) As List(Of DataRow)
            Dim dsRatePlansPromos As RatePlanData = New RatePlanFacade().GetRatePlanByIdHotel(hotelId, idioma:=1, IncluirPaquetesSegmentoK:=1, incluirNetRatesPlan:=1, idAsociacion:=-1, DeleteFilter:=1, getPromos:=True)


            Dim filterPromosDates As String = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"


            Return dsRatePlansPromos.Tables(RatePlanData.RATEPLAN_TABLE).Select(filterPromosDates).ToList()
        End Function

        Function GetPromosByRatePlan(ByVal hotelId As Integer, ByVal ratePlanId As String) As List(Of spGetPromosByRatePlan_Result)

            Dim promos As List(Of spGetPromosByRatePlan_Result) = New List(Of spGetPromosByRatePlan_Result)

            Using dbContext As New OzHotelesEntities()

                promos = dbContext.spGetPromosByRatePlan(hotelId, ratePlanId).ToList()

            End Using

            Return promos

        End Function

        Function GetValidPromos(ByVal activeRatePlansPromos As List(Of DataRow), ByVal promos As List(Of spGetPromosByRatePlan_Result)) As List(Of DataRow)

            Dim validPromos As List(Of DataRow) = New List(Of DataRow)

            Dim dsRatePlans As RatePlanData = New RatePlanData

            Dim dt As DataTable = dsRatePlans.Tables(RatePlanData.RATEPLAN_TABLE)

            For Each activeRatePlanPromo As DataRow In activeRatePlansPromos

                For Each promo As spGetPromosByRatePlan_Result In promos

                    If promo.IdPromocion = activeRatePlanPromo.ItemArray(0).ToString() Then

                        Dim tempData As DataRow = dt.NewRow()

                        With tempData
                            .Item(RatePlanData.FIELD_IDRATEPLAN) = promo.IdPromocion
                        End With

                        validPromos.Add(tempData)

                    End If

                Next

            Next

            Return validPromos

        End Function


        Sub Log(ByVal user As String, ByVal userId As Integer, ByVal page As String, ByVal hotelId As Integer, ByVal action As Actions, ByVal note As String, ByVal request As String, ByVal data As String, ByVal dataAfter As String)

            Dim ds As LogData = New LogData
            Dim dr As DataRow = ds.Tables(LogData.TABLE_LOG).NewRow
            dr(LogData.FIELD_USUARIO) = user
            dr(LogData.FIELD_IDUSUARIO) = userId
            dr(LogData.FIELD_PAGINA) = page
            dr(LogData.FIELD_ACCION) = action
            dr(LogData.FIELD_HOTEL) = hotelId
            dr(LogData.FIELD_NOTA) = note
            dr(LogData.FIELD_FECHA) = Date.Now
            If Not String.IsNullOrEmpty(data) Then dr(LogData.FIELD_DATOS) = data
            If Not String.IsNullOrEmpty(dataAfter) Then dr(LogData.FIELD_DATOSDESPUES) = dataAfter

            ds.Tables(LogData.TABLE_LOG).Rows.Add(dr)
            With New LogFacade
                .insertLog(ds)
            End With

        End Sub

    End Module
End Namespace
