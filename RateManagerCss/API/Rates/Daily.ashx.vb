Imports System.Web
Imports System.Web.Services
Imports APIServices

Public Class Daily
    Inherits APIHandler

    Private RatesService As New RatesService

    Private Function ValidadGetRequest(ByRef context As HttpContext, ByRef req As GetDailyRateDetailRequest) As Boolean

        Dim rateId As Integer
        If Not Integer.TryParse(context.Request.QueryString("rateid"), rateId) Then
            req.Error = "invalid rateid"
            Return False
        End If

        Dim day As Date
        If Not Date.TryParse(context.Request.QueryString("day"), day) Then
            req.Error = "invalid startDate"
            Return False
        End If

        req.RateId = rateId
        req.Day = day

        Return True
    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)

        Dim req As New GetDailyRateDetailRequest

        If ValidadGetRequest(context, req) Then
            OK(
                    context:=context,
                    result:=RatesService.FindDayRateDetail(req.RateId, req.Day)
                  )
        Else
            BadRequest(context, req.Error)
        End If
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub
End Class

Friend Class GetDailyRateDetailRequest
    Public RateId As Integer
    Public Day As Date
    Public [Error] As String
End Class