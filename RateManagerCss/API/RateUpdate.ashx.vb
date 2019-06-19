Imports System.Web
Imports System.Web.Services
Imports APIServices
Imports APIServices.Models.DTO

Public Class RateUpdate
    Inherits APIHandler


    Private RatesServices As New RatesService()
    Private Function ValidadPostRequest(ByRef context As HttpContext, ByRef req As RateUpdateRQ) As Boolean
        Try


            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Dim req As New RateUpdateRQ()
        If ValidadPostRequest(context, req) Then
            OK(
                context:=context,
                result:=RatesServices.AddRate(req))
        Else
            OkWithError(context, "ERROR")
        End If
    End Sub

End Class