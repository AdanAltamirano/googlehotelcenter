Imports System.Web
Imports System.Web.Services
Imports APIServices

Public Class HotelsAPI
    Inherits APIHandler

    Private HotelsService As New HotelService

    Private Function ValidadGetRequest(ByRef context As HttpContext, ByRef req As GetHotelRequest) As Boolean

        Dim hotelId As Integer
        If Not Integer.TryParse(context.Request.QueryString("hotelid"), hotelId) Then
            req.Error = "invalid hotelId"
            Return False
        End If

        Dim lang = IIf(context.Request.QueryString("language") = "es", 1, 2)

        req.HotelId = hotelId
        req.Language = lang

        Return True
    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)
        Dim req As New GetHotelRequest

        If ValidadGetRequest(context, req) Then
            OK(
                    context:=context,
                    result:=HotelsService.FindById(req.HotelId, req.Language)
                  )
        Else
            BadRequest(context, req.Error)
        End If
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub

End Class

Friend Class GetHotelRequest
    Public HotelId As Integer
    Public Language As Integer
    Public [Error] As String
End Class