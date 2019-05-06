Imports System.Web
Imports System.Web.Services
Imports APIServices

Public Class Rates
    Inherits APIHandler

    Private RatesService As New RatesService

    Private Function ValidadGetRequest(ByRef context As HttpContext, ByRef req As GetRatesRequest) As Boolean

        Dim hotelId As Integer
        If Not Integer.TryParse(context.Request.QueryString("hotelid"), hotelId) Then
            req.Error = "invalid hotelId"
            Return False
        End If

        Dim start As Date
        If Not Date.TryParse(context.Request.QueryString("startDate"), start) Then
            req.Error = "invalid startDate"
            Return False
        End If

        Dim [end] As Date
        If Not Date.TryParse(context.Request.QueryString("endDate"), [end]) Then
            req.Error = "invalid endDate"
            Return False
        End If

        Dim hotelRoomId As Integer? = Nothing
        If Not String.IsNullOrWhiteSpace(context.Request.QueryString("hotelRoomId")) AndAlso
                Not Integer.TryParse(context.Request.QueryString("hotelRoomId"), hotelRoomId) Then
            req.Error = "invalid hotelRoomId"
            Return False
        End If

        Dim lang = IIf(context.Request.QueryString("language") = "es", 1, 2)

        req.HotelId = hotelId
        req.StartDate = start
        req.EndDate = [end]
        req.HotelRoomId = hotelRoomId
        req.Language = lang

        Return True
    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)

        Dim req As New GetRatesRequest

        If ValidadGetRequest(context, req) Then
            OK(
                    context:=context,
                    result:=RatesService.FindGroupedByRatePlan(req.HotelId, req.StartDate, req.EndDate, req.Language, req.HotelRoomId)
                  )
        Else
            BadRequest(context, req.Error)
        End If
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub

End Class

Friend Class GetRatesRequest
    Public HotelId As Integer
    Public StartDate As Date
    Public EndDate As Date
    Public Language As Integer
    Public HotelRoomId As Integer?
    Public [Error] As String
End Class