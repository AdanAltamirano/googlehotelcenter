Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters

Public Class F2goGetReport
    Implements System.Web.IHttpHandler, IRequiresSessionState

    'este script se usara igual para el reporte
    'de internet power :v

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim q As NameValueCollection = context.Request.Form
        Dim isUv As Boolean = q("isUv") <> Nothing

        Dim idHotel As Integer = 0
        If isUv Then
            If HttpContext.Current.Session("InfoCompany") IsNot Nothing Then
                Dim info As companyInfo = CType(HttpContext.Current.Session("InfoCompany"), companyInfo)
                If info IsNot Nothing AndAlso info.Hotel > 0 Then
                    idHotel = info.Hotel
                End If
            End If

        End If

        Dim response As String = "{ ""result"": [] }"
        If q("checkin") IsNot Nothing AndAlso q("checkout") IsNot Nothing Then

            Dim typeSearch As TypeSearch
            Select Case IIf(q("typesearch") IsNot Nothing, q("typesearch"), 0)
                Case 0
                    typeSearch = TypeSearch.byreservation
                Case 1
                    typeSearch = TypeSearch.byhotelcheckin
                Case 2
                    typeSearch = TypeSearch.byhotelcheckout
            End Select

            Dim responseDb As DataTable = GetData(q("checkin"), q("checkout"), typeSearch, isUv, idHotel)

            If responseDb IsNot Nothing AndAlso responseDb.Rows.Count > 0 Then
                Dim iso As New IsoDateTimeConverter() With {.DateTimeFormat = "dd/MM/yyyy"}
                response = "{ ""result"":" & JsonConvert.SerializeObject(responseDb, Formatting.Indented, iso) & "}"
                context.Session("reportExcel") = responseDb
            End If
        End If

        context.Response.ContentType = "application/json"
        context.Response.Write(response)
    End Sub

    Enum TypeSearch
        byreservation
        byhotelcheckin
        byhotelcheckout
    End Enum

    Function GetData(ByVal checkin As String, ByVal checkout As String,
                     ByVal dateType As TypeSearch, ByVal isUv As Boolean, ByVal idHotel As Integer) As DataTable

        Dim response As New DataTable
        Dim connString As String = ConfigurationManager.AppSettings(If(isUv, "PortalConnectionString", "HotelConnectionString"))
        Dim connection As New SqlConnection(connString)

        Try
            Dim sp As String = If(isUv, "sp_reporte_ventas", "spr_getReportF2go")
            connection.Open()
            Using da As New SqlDataAdapter(sp, connection)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                If isUv Then
                    da.SelectCommand.Parameters.AddWithValue("@idHotel", idHotel)
                End If
                da.SelectCommand.Parameters.AddWithValue("@checkin", checkin)
                da.SelectCommand.Parameters.AddWithValue("@checkout", checkout)
                da.SelectCommand.Parameters.AddWithValue("@dateType", [Enum].GetName(GetType(TypeSearch), dateType))

                da.Fill(response)
                da.Dispose()
            End Using
        Catch ex As Exception
            connection.Close()
        Finally
            connection.Close()
        End Try

        Return response
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class