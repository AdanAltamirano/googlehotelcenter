Option Strict On
Option Explicit On 

Imports System.Web.SessionState
Imports System.Globalization
Imports System.Threading
Imports System.Resources

Imports System.Web
Imports System.Web.Security

Imports System.Configuration.ConfigurationManager
Imports System.Xml
Imports System.Security.Principal
Imports System.IO
Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Código generado por el Diseñador de componentes "

    Public Sub New()
        MyBase.New()

        'El Diseñador de componentes requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub

    'Requerido por el Diseñador de componentes
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de componentes requiere el siguiente procedimiento
    'Se puede modificar utilizando el Diseñador de componentes.
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se inicia la aplicación
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se inicia la sesión
    End Sub

    'Private Function Contains(ByRef url As String) As Boolean
    '    Dim ds As New DataSet
    '    Dim dr As DataRow
    '    Try
    '        Dim txtRead As XmlTextReader = New XmlTextReader(Server.MapPath("/RateManager/Data/Groups.xml"))
    '        ds.ReadXml(txtRead)
    '        txtRead.Close()
    '        url = url.ToUpper
    '        For Each dr In ds.Tables(0).Rows
    '            If url.IndexOf(dr("IDGROUP").ToString.ToUpper) > 0 Then
    '                GroupId = dr("IDGROUP").ToString

    '                url = url.Replace("/" & dr("IDGROUP").ToString.ToUpper, "").ToLower
    '                Return True
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Return False
    '    End Try
    '    Return False
    'End Function


    Private Function Contains(ByRef url As String) As Boolean
        Dim ds As New DataSet
        Try
            Dim reader As New XmlTextReader(Me.Server.MapPath("/RateManager/Data/Groups.xml"))
            ds.ReadXml(reader)
            reader.Close()
            url = url.ToUpper
            Dim row As DataRow
            For Each row In ds.Tables.Item(0).Rows
                If (url.IndexOf(row.Item("IDGROUP").ToString.ToUpper) > 0) Then
                    Me.GroupId = row.Item("IDGROUP").ToString
                    url = url.Replace(("/" & row.Item("IDGROUP").ToString.ToUpper), "").ToLower
                    Return True
                End If
            Next

        Catch exception1 As Exception

            '            Dim exception As Exception = exception1

            Return False

        End Try
        Return False
    End Function




    Dim GroupId As String = ""

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)


       

        'Dim currentExecutionFilePath As String = Me.Request.CurrentExecutionFilePath
        'Me.GroupId = ""

        'If Me.Contains(currentExecutionFilePath) Then
        '    Me.Context.RewritePath(currentExecutionFilePath)
        'End If
        'If (currentExecutionFilePath.ToLower.IndexOf("default.aspx") >= 0) Then
        '    If (Not Me.Request.Cookies.Item("GroupId") Is Nothing) Then
        '        Me.Response.Cookies.Item("GroupId").Value = Me.GroupId
        '    Else
        '        Dim cookie As New HttpCookie("GroupId")
        '        cookie.Value = Me.GroupId
        '        Me.Response.Cookies.Add(cookie)
        '    End If
        'End If
    End Sub

    'Public Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim currentExecutionFilePath As String = Me.Request.CurrentExecutionFilePath
    '    Me.GroupId = ""
    '    If Me.Contains((currentExecutionFilePath)) Then
    '        Me.Context.RewritePath(currentExecutionFilePath)
    '    End If
    '    If (currentExecutionFilePath.ToLower.IndexOf("default.aspx") >= 0) Then
    '        If (Not Me.Request.Cookies.Item("GroupId") Is Nothing) Then
    '            Me.Response.Cookies.Item("GroupId").Value = Me.GroupId
    '        Else
    '            Dim cookie As New HttpCookie("GroupId")
    '            cookie.Value = Me.GroupId
    '            Me.Response.Cookies.Add(cookie)
    '        End If
    '    End If
    'End Sub







    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.AuthenticateRequest
        ' Se desencadena al intentar autenticar el uso
        'If Request.IsAuthenticated Then
        '    ' Obtenemos los roles desde la cookie
        '    Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(Context.Request.Cookies(FormsAuthentication.FormsCookieName()).Value)

        '    'convertimos a arreglo de elementos la lista de cookies                
        '    Dim roleListArray As String()
        '    roleListArray = ticket.UserData.Split(CType(",", Char))

        '    'Añadimos los roles al usuario
        '    HttpContext.Current.User = New GenericPrincipal(User.Identity, roleListArray)
        'End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando ocurre un error
        Dim LastError As Exception
        Dim ErrMessage As String = ""

        LastError = Server.GetLastError()

        If Not LastError Is Nothing AndAlso LastError.InnerException IsNot Nothing Then
            ErrMessage = LastError.InnerException.Message & LastError.InnerException.StackTrace & ControlChars.CrLf
        End If

        If ErrMessage <> "" Then
            Dim PathFile As String = HttpContext.Current.Request.PhysicalApplicationPath & "Portal\Logs\"
            Dim FileName As String = PathFile & Now.Day.ToString("00") & Now.Month.ToString("00") & Now.Year.ToString("0000") & ".log"
            Dim FileError As New System.IO.FileInfo(FileName)

            If Not IO.Directory.Exists(PathFile) Then
                IO.Directory.CreateDirectory(PathFile)
            End If
            If Not FileError.Exists Then
                Dim fs As FileStream = File.Create(FileName)
                fs.Close()
            End If
            FileError = New System.IO.FileInfo(FileName)
            If FileError.Exists Then
                FileOpen(1, FileName, OpenMode.Append)
                Print(1, Now.Hour & ":" & Now.Minute & ":" & Now.Second & ": " & ErrMessage)
                FileClose(1)
            End If
        End If
        
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando termina la sesión
        Try
            Session.Abandon()
            Session.Clear()
            FormsAuthentication.SignOut()
        Catch ex As Exception
        End Try
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando termina la aplicación
    End Sub


    'Private Sub Global_AcquireRequestState(ByVal sender As Object, ByVal e As EventArgs)
    '    'If (Me.Request.CurrentExecutionFilePath.ToLower.IndexOf("default.aspx") >= 0) Then
    '    '    HttpContext.Current.Session.Item("GroupId") = Me.GroupId
    '    'End If

    '    If (Me.Request.CurrentExecutionFilePath.ToLower.IndexOf("default.aspx") >= 0) Then
    '        HttpContext.Current.Session.Item("GroupId") = Me.GroupId
    '    End If


    'End Sub

    Private Sub Global_AcquireRequestState(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.AcquireRequestState
        'Dim fullOrigionalpath As String = Request.CurrentExecutionFilePath
        'If Contains(fullOrigionalpath) Then
        'Context.RewritePath(fullOrigionalpath)}
        Dim fullOrigionalpath As String = Request.CurrentExecutionFilePath

        If fullOrigionalpath.ToLower.IndexOf("default.aspx") >= 0 Then
            HttpContext.Current.Session("GroupId") = GroupId
        End If

    End Sub






End Class
