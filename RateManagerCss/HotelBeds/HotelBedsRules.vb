Imports System
Imports System.Collections.Generic
Imports System.Web
Imports RateManager.HotelBedsDSTableAdapters
Imports System.Diagnostics

Public Class HotelBedsRules
    Dim Log As LogGenerator = New LogGenerator()

    Public Function SelectByCode(ByVal Code As String, ByVal table As HotelBedsDS.DestinationListDataTable) As Boolean
        Dim taDestinationLis As DestinationListTableAdapter

        Try
            taDestinationLis = New DestinationListTableAdapter()
            taDestinationLis.FillByCode(table, Code)
            taDestinationLis.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.SelectByCode: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_Cities_ByKey(ByVal KeyString As String, ByVal table As HotelBedsDS.CitiesListDataTable) As Boolean
        Dim taCitiesList As CitiesListTableAdapter

        Try
            taCitiesList = New CitiesListTableAdapter()
            taCitiesList.Fill(table, KeyString)
            taCitiesList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_Cities_ByKey: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Select_City_ByID(ByVal idCiudad As Integer, ByVal table As HotelBedsDS.CityDataTable) As Boolean
        Dim taCitiesList As CityTableAdapter

        Try
            taCitiesList = New CityTableAdapter()
            taCitiesList.Fill(table, idCiudad)
            taCitiesList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_City_ByID: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Select_Cities_Suggested(ByVal table As HotelBedsDS.CitiesListDataTable) As Boolean
        Dim taCitiesList As CitiesListTableAdapter

        Try
            taCitiesList = New CitiesListTableAdapter()
            taCitiesList.FillBySuggested(table)
            taCitiesList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_Cities_Suggested: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Insert_DestinationSkipped(ByVal idCiudad As Integer) As Boolean
        Dim taDestinationList As DestinationListTableAdapter

        Try
            taDestinationList = New DestinationListTableAdapter()
            taDestinationList.Insert_DestinationsSkipped(idCiudad)
            taDestinationList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Insert_DestinationSkipped: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Delete_DestinationSkipped(ByVal idCiudad As Integer) As Boolean
        Dim taDestinationList As DestinationListTableAdapter

        Try
            taDestinationList = New DestinationListTableAdapter()
            taDestinationList.Delete_DestinationsSkipped(idCiudad)
            taDestinationList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Delete_DestinationSkipped: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Select_Cities_Skipped(ByVal table As HotelBedsDS.CitiesListDataTable) As Boolean
        Dim taCitiesList As CitiesListTableAdapter

        Try
            taCitiesList = New CitiesListTableAdapter()
            taCitiesList.FillBySkipped(table)
            taCitiesList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_Cities_Skipped: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_HBHotelsByKey(ByVal keystring As String, ByVal table As HotelBedsDS.HotelsDataTable, ByRef strError As String) As Boolean
        Dim taHotels As HotelsTableAdapter
        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByHBKey(table, keystring)
            taHotels.Dispose()
            'Log.Add(HttpContext.Current.Request.Url.AbsoluteUri, "Search Hotels by Key", "success")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_HBHotelsByKey: " & ex.Message)
            'Log.Add(HttpContext.Current.Request.Url.AbsoluteUri, "Search Hotels by Key", "[Error] HotelBedsRules.Select_HBHotelsByKey: " & ex.Message)
            strError = "[Error] HotelBedsRules.Select_HBHotelsByKey: " & ex.Message
            Return False
        End Try
        Return True
    End Function

    Public Function Select_Cities_SkippedByKey(ByVal KeyString As String, ByVal table As HotelBedsDS.CitiesListDataTable) As Boolean
        Dim taCitiesList As CitiesListTableAdapter

        Try
            taCitiesList = New CitiesListTableAdapter()
            taCitiesList.FillSkippedByKey(table, KeyString)
            taCitiesList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_Cities_SkippedByKey: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Select_Cities_SkippedByAll(ByVal table As HotelBedsDS.CitiesListDataTable) As Boolean
        Dim taCitiesList As CitiesListTableAdapter

        Try
            taCitiesList = New CitiesListTableAdapter()
            taCitiesList.FillByAll(table)
            taCitiesList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_Cities_SkippedByAll: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_Destinations_ByKey(ByVal Key As String, ByVal table As HotelBedsDS.DestinationsDataTable) As Boolean
        Dim taDestinationsList As DestinationsTableAdapter

        Try
            taDestinationsList = New DestinationsTableAdapter()
            taDestinationsList.FillByKey(table, Key)
            taDestinationsList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_Destinations_ByKey: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_Destinations_Suggested(ByVal idCiudad As String, ByVal table As HotelBedsDS.DestinationsDataTable) As Boolean
        Dim taDestinationsList As DestinationsTableAdapter

        Try
            taDestinationsList = New DestinationsTableAdapter()
            taDestinationsList.FillBySuggested(table, idCiudad)
            taDestinationsList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_Destinations_Suggested: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Insert_DestinationsFix(ByVal destinationcode As String, ByVal zonecode As Integer, ByVal idCiudad As Integer, Optional ByVal idDestination As Integer = 0) As Boolean
        Dim taDestinationsList As DestinationsTableAdapter

        Try
            taDestinationsList = New DestinationsTableAdapter()
            taDestinationsList.Insert_DestinationsFix(destinationcode, zonecode, idCiudad, idDestination)
            taDestinationsList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Update_DestinationsIdCiudad: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_Destinations_ByIdCiudad(ByVal idCiudad As Integer, ByVal table As HotelBedsDS.DestinationsDataTable) As Boolean
        Dim taDestinationsList As DestinationsTableAdapter

        Try
            taDestinationsList = New DestinationsTableAdapter()
            taDestinationsList.FillByKey(table, idCiudad)
            taDestinationsList.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_Destinations_ByIdCiudad: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Update_RemoveIdCiudad(ByVal idDestination As Integer) As Boolean
        Dim taDestination As DestinationsTableAdapter

        Try
            taDestination = New DestinationsTableAdapter()
            taDestination.Delete_DestinationsFix(idDestination)
            taDestination.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Update_RemoveIdCiudad: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Select_UVHotelsSuggested(ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByUVSuggested(table)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_UVHotelsSuggested: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_UVHotelsByKey(ByVal keyString As String, ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByUVKeyString(table, keyString)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_UVHotelsByKey: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_UVHotelsByID(ByVal id As Integer, ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByUVID(table, id)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_UVHotelsByID: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_HBHotelsSuggested(ByVal id As Integer, ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByHBSuggested(table, id)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_HBSuggested: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_HBHotelsRelByID(ByVal id As Integer, ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillHBRelByID(table, id)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_HBSuggested: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Insert_HotelFix(ByVal idHotel As Integer, ByVal hotelCode As String) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.Insert_HotelFix(hotelCode, idHotel)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Insert_HotelFix: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Delete_HotelFix(ByVal idHotelFix As Integer) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.Delete_HotelsFix(idHotelFix)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Delete_HotelFix: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Delete_HotelFix(ByVal keystring As Integer, ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByHBKey(table, keystring)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Select_HBHotelsByKey: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Select_HotelFixAll(ByVal table As HotelBedsDS.HotelsDataTable) As Boolean
        Dim taHotels As HotelsTableAdapter

        Try
            taHotels = New HotelsTableAdapter()
            taHotels.FillByHotelFix(table)
            taHotels.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_HotelFixAll: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_DestinationsFixByKey(ByVal KeyString As String, ByVal table As HotelBedsDS.RelatedListDataTable) As Boolean
        Dim taRelated As RelatedListTableAdapter

        Try
            taRelated = New RelatedListTableAdapter()
            taRelated.FillByRelated(table, KeyString)
            taRelated.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_DestinationsFixByKey: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Search_ZonesByDestinationCode(ByVal KeyString As String, ByVal table As HotelBedsDS.DestinationsDataTable) As Boolean
        Dim taZones As DestinationsTableAdapter

        Try
            taZones = New DestinationsTableAdapter()
            taZones.FillByDestinationCode(table, KeyString)
            taZones.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Search_ZonesByDestinationCode: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function Update_DestinationsFix(ByVal DestinationCode As String, ByVal idCiudad As Integer) As Boolean
        Dim taDestination As DestinationsTableAdapter
        Try
            taDestination = New DestinationsTableAdapter()
            taDestination.Update_DestinationsFix(DestinationCode, idCiudad)
            taDestination.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("[Error] HotelBedsRules.Update_DestinationsFix: " & ex.Message)
            Return False
        End Try
        Return True
    End Function
End Class
