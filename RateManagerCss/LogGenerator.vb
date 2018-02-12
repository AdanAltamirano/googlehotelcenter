Option Explicit On
Option Strict On
Imports System
Imports System.IO
Imports Microsoft.VisualBasic

Public Class LogGenerator

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub Add(ByVal url As String, ByVal Evento As String, ByVal LogMessage As String)
        Dim pathLog As String = HttpContext.Current.Request.PhysicalApplicationPath & "Logs\Log\"
        Dim save As String = HttpContext.Current.Request.PhysicalApplicationPath & "Logs\Log\"
        Dim pathFile As String = String.Format("{0}{1}.txt", pathLog, Date.Now.ToString("yyyy-MM-dd"))
        Dim strStream As Stream
        Dim strStreamWriter As StreamWriter

        Try
            If Not System.IO.Directory.Exists(pathLog) Then
                System.IO.Directory.CreateDirectory(save)
            End If
        Catch ex As Exception
            Dim e As String = ex.Message
        End Try

        If File.Exists(pathFile) Then
            strStreamWriter = File.AppendText(pathFile)
        Else
            strStream = File.Create(pathFile)
            strStreamWriter = New StreamWriter(strStream, System.Text.Encoding.Default)
        End If

        Log(url, Evento, LogMessage, strStreamWriter)

    End Sub

    Private Sub Log(ByVal url As String, ByVal Evento As String, ByVal LogMessage As String, ByVal w As TextWriter)
        w.Write(ControlChars.CrLf & "Log Entry: ")
        w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString())

        w.WriteLine("URL: ")
        w.WriteLine("{0}", url)

        w.WriteLine("Event:")
        w.WriteLine("{0}", Evento)

        w.WriteLine("Message:")
        w.WriteLine("{0}", LogMessage)
        w.WriteLine("---------------------------------------------")
        w.Flush()
        w.Close()
    End Sub
End Class
