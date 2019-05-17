Imports System.Web
Imports System.Web.Services
Imports APIServices

Public Class Inventory
    Inherits APIHandler

    Private RoomsService As New RoomsService

    Private Function ValidadGetRequest(ByRef context As HttpContext, ByRef req As GetInventoryRequest) As Boolean

        Dim roomId As Integer
        If Not Integer.TryParse(context.Request.QueryString("roomid"), roomId) Then
            req.Error = "invalid roomId"
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

        req.RoomId = roomId
        req.StartDate = start
        req.EndDate = [end]

        Return True
    End Function

    Protected Overrides Sub GetHandler(ByRef context As HttpContext)
        Dim req As New GetInventoryRequest

        If ValidadGetRequest(context, req) Then
            OK(
                    context:=context,
                    result:=RoomsService.FindInventoryByRoomId(req.RoomId, req.StartDate, req.EndDate)
                  )
        Else
            BadRequest(context, req.Error)
        End If
    End Sub

    Protected Overrides Sub PostHandler(ByRef context As HttpContext)
        Throw New NotImplementedException()
    End Sub
End Class

Friend Class GetInventoryRequest
    Public RoomId As Integer
    Public StartDate As Date
    Public EndDate As Date
    Public [Error] As String
End Class