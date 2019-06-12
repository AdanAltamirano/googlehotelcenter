Imports System.Web.Http.Controllers
Imports System.Web.Http.Filters

Namespace API.Helpers
    Public Class ValidateUserAccessAttribute
        Inherits ActionFilterAttribute
        Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)

        End Sub
    End Class
End Namespace

