Imports System
Imports System.Threading
Imports System.Globalization
Imports System.Web
Imports System.Web.SessionState
Imports System.Diagnostics
Imports System.Configuration.ConfigurationManager
Public Class LanguageHandler
    Implements IHttpHandler, IRequiresSessionState

    'IHttpHandler Members
    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        Dim request As HttpRequest = context.Request
        Dim responce As HttpResponse = context.Response

        Try

            Dim CultureUIName As String = request.QueryString("CultureUI")
            If IsNumeric(CultureUIName) Then
                PortalCulture.SetCultureByID(CultureUIName)
                Exit Try
            End If
            Dim ci As CultureInfo
            ci = New CultureInfo(CultureUIName)
            PortalCulture.SetCulture(ci.Name)
        Catch ex As Exception
            PortalCulture.SetCulture(AppSettings("DefaultLanguage"))
        End Try
        'Redirect to the original web page.
        responce.Redirect(request.UrlReferrer.PathAndQuery)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

End Class
