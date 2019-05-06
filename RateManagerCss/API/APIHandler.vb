Imports System.Web
Imports System.Web.Services
Imports Newtonsoft.Json

Public MustInherit Class APIHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If context.Request.HttpMethod = "GET" Then
            GetHandler(context)
        ElseIf context.Request.HttpMethod = "POST" Then
            PostHandler(context)
        Else
            NotAllowed(context)
        End If

    End Sub

    Protected Sub OK(ByRef context As HttpContext, ByVal result As Object)
        context.Response.ContentType = "application/json"
        context.Response.StatusCode = 200
        context.Response.Write(SerializeResponse(result))
    End Sub

    Protected Sub BadRequest(ByRef context As HttpContext, ByVal [error] As String)
        context.Response.StatusCode = 400
        context.Response.ContentType = "application/json"
        Dim result = New With {
            .Message = IIf(String.IsNullOrWhiteSpace([error]), "bad request", [error]),
            .Status = 400
        }
        context.Response.Write(SerializeResponse(result))
    End Sub

    Protected Sub NotFound(ByRef context As HttpContext)
        context.Response.StatusCode = 404
        context.Response.ContentType = "application/json"
        Dim result = New With {
            .Message = "not found",
            .Status = 404
        }
        context.Response.Write(SerializeResponse(result))
    End Sub

    Protected Sub NotAllowed(ByRef context As HttpContext)
        context.Response.StatusCode = 405
        context.Response.Status = "method not allowed"
    End Sub

    Protected MustOverride Sub GetHandler(ByRef context As HttpContext)

    Protected MustOverride Sub PostHandler(ByRef context As HttpContext)

    Private JSONsettings As JsonSerializerSettings = New JsonSerializerSettings With {
        .DateFormatHandling = DateFormatHandling.IsoDateFormat
    }

    Private Function SerializeResponse(ByVal response As Object) As String
        Return JsonConvert.SerializeObject(response)
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class