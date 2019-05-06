Imports System.Web
Imports System.Web.Services
Imports APIServices

Public Class HotelRooms
    Inherits APIHandler

    Private RoomsService As New RoomsService

    Private Function ValidadGetRequest(ByRef context As HttpContext, ByRef req As GetRoomsRequest) As Boolean

        Dim hotelId As Integer
        If Not Integer.TryParse(context.Request.QueryString("hotelid"), hotelId) Then
            req.Error = "invalid hotelId"
            Return False
        End If

        Dim lang = IIf(context.Request.QueryString("language") = "es", 1, 2)

        Dim showInactive = IIf(context.Request.QueryString("showinactive") = "true", True, False)

        req.HotelId = hotelId
        req.Language = lang
        req.ShowInactive = showInactive

        Return True
    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)
        Dim req As New GetRoomsRequest

        If ValidadGetRequest(context, req) Then
            OK(
                    context:=context,
                    result:=RoomsService.FindByHotel(req.HotelId, req.Language, req.ShowInactive)
                  )
        Else
            BadRequest(context, req.Error)
        End If
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub
End Class

Friend Class GetRoomsRequest
    Public HotelId As Integer
    Public Language As Integer
    Public ShowInactive As Boolean
    Public [Error] As String
End Class