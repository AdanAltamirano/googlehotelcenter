Imports System.Web.Mvc

Namespace API.Controllers
    Public Class AgencyController
        Inherits Controller

        ' GET: Agency
        Function Index() As ActionResult
            Return View()
        End Function
    End Class
End Namespace