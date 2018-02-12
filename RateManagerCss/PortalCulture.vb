
Imports System.Web.Security
Imports System.Globalization
Imports System.Threading
Imports System.Resources
Imports System.Configuration.ConfigurationManager

Public Class PortalCulture

	'NameSpace.BaseName
    Private Const BaseName As String = "RateManager.strings"

	Public Shared Sub SetCulture(ByVal name As String)
		Try
			Dim ck As New HttpCookie(FormsAuthentication.FormsCookieName() & "_cUI", name)
			ck.Expires = Date.MaxValue
			ck.Values.Item("ID") = GetIDCultureByName(name)
			HttpContext.Current.Response.Cookies.Add(ck)		

			Dim ci As CultureInfo
			ci = New CultureInfo(name)
			HttpContext.Current.Session("Cultura") = ci
			HttpContext.Current.Session("Cultura_ID") = ck.Values.Item("ID")
			Thread.CurrentThread.CurrentUICulture = ci
		Catch e As Exception
		Finally
		End Try
	End Sub

	Public Shared Function GetCulture() As CultureInfo
		Dim ci As CultureInfo
		If HttpContext.Current.Session("Cultura") Is Nothing Then
			Dim ck As HttpCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName() & "_cUI")
			If ck Is Nothing Then
				SetCulture(AppSettings("DefaultLanguage"))
			Else
				SetCulture(ck.Values(0))
			End If
		End If
		ci = CType(HttpContext.Current.Session("Cultura"), CultureInfo)
		If Not ci Is Nothing Then
			GetCulture = ci
		End If
	End Function

	Public Shared Function GetIDCulture() As Integer
        Return GetIDCulture(HttpContext.Current)
    End Function

    Public Shared Function GetIDCulture(ByVal context As HttpContext) As Integer
        Dim ci As CultureInfo
        If context.Session("Cultura_ID") Is Nothing Then
            Dim ck As HttpCookie = context.Request.Cookies.Get(FormsAuthentication.FormsCookieName() & "_cUI")
            If ck Is Nothing Then
                Return AppSettings("DefaultLanguageId")
            Else
                Return ck.Values.Item("ID")
            End If
        End If
        Return context.Session("Cultura_ID")
    End Function

	Public Shared Function GetString(ByVal idString As String, Optional ByVal w2p As Boolean = False) As String
		' "-" Indica que no se encontro la cadena
		' "*" Indica que sucedio un error al leer el recurso
		Dim resource As String = "-"
		Try
			Dim ci As CultureInfo = GetCulture()
			If Not ci Is Nothing Then
				Thread.CurrentThread.CurrentUICulture = ci
			End If
			Dim rm As ResourceManager = New ResourceManager(BaseName, System.Reflection.Assembly.GetExecutingAssembly())
            resource = rm.GetString(idString) & IIf(w2p, ":", "")
		Catch e As Exception
			resource = "*"
		Finally
			GetString = resource
		End Try
    End Function

    Public Shared Function GetString(ByVal idString As String, ByVal nameCulture As String, Optional ByVal w2p As Boolean = False) As String
        ' "-" Indica que no se encontro la cadena
        ' "*" Indica que sucedio un error al leer el recurso
        Dim resource As String = "-"
        Dim ci As CultureInfo

        Try

            ci = New CultureInfo(nameCulture)
            Thread.CurrentThread.CurrentUICulture = ci

            Dim rm As ResourceManager = New ResourceManager(BaseName, System.Reflection.Assembly.GetExecutingAssembly())
            resource = rm.GetString(idString) & IIf(w2p, ":", "")

        Catch e As Exception
            resource = "*"
        Finally
            GetString = resource
        End Try
    End Function

	Private Shared Function GetIDCultureByName(ByVal name As String) As Integer
		GetIDCultureByName = 0
		Try
			Dim dsCulture As New DataSet
			Dim strXML As String = HttpContext.Current.Request.PhysicalApplicationPath & "/PortalCultures.xml"
			Dim baseActual = name.Substring(0, 2)
			dsCulture.ReadXml(strXML)
			For Each r As DataRow In dsCulture.Tables(0).Rows
				If r.Item("base") = baseActual Then
					Return CType(r.Item("id"), Integer)
				End If
			Next
		Catch ex As Exception
		End Try
	End Function

	Public Shared Function SetCultureByID(ByVal ID As Integer) As Boolean
		SetCultureByID = False
		Try
			Dim dsCulture As New DataSet
			Dim strXML As String = HttpContext.Current.Request.PhysicalApplicationPath & "/PortalCultures.xml"
			dsCulture.ReadXml(strXML)
			For Each r As DataRow In dsCulture.Tables(0).Rows
				If r.Item("id") = ID Then
					SetCulture(r.Item("name"))
					SetCultureByID = True
				End If
			Next
		Catch ex As Exception
		End Try
	End Function

End Class
