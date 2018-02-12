Imports System.Xml
Imports System.Collections.Specialized
Imports System.Web
Imports System.Web.HttpContext
Imports System.IO


Partial Public Class LogReader
    Inherits System.Web.UI.Page

    Private Function ReadFile(ByVal sFile As String) As String
        Dim tr As TextReader
        Dim sContent As String

        sContent = ""
        Try
            tr = New StreamReader(sFile, System.Text.Encoding.Default)
            sContent = tr.ReadToEnd
            sContent = sContent.Replace(vbCrLf, "<br>")
            'sContent = sContent.Replace(vbCr, "<br>").Replace(vbLf, "<br>").Replace(vbCrLf, "<br>")

            tr.Close()
            Literal1.Text = String.Format("<hr/> {0} <hr/>", sContent)
            lblError.Text = ""
        Catch e As Exception
            lblError.Text = String.Format("The Log fragment file '{0}' cannot be found", sFile)
        End Try
        Return sContent
    End Function

    Function GetFiles(ByVal PathFile As String, ByVal iyear As Integer, ByVal imonth As Integer) As String
        Dim stmp As String
        Dim html As String = ""
        stmp = String.Format("{0}{1}.log", If(imonth > 0, imonth.ToString("00"), ""), If(iyear > 0, iyear.ToString("0000"), ""))
        For Each sfile As String In IO.Directory.GetFiles(PathFile)
            If sfile.Contains(stmp) Then
                html &= String.Format("{0}<br/>", sfile)
            End If
        Next
        Return String.Format("<hr/> {0} <hr/>", html)
    End Function

    Function ReadLog() As Boolean
        Dim iYear As Integer
        Dim iMonth As Integer
        Dim iDay As Integer

        Integer.TryParse(txtYear.Text, iYear)
        Integer.TryParse(txtMonth.Text, iMonth)
        Integer.TryParse(txtDay.Text, iDay)

        Dim PathFile As String = HttpContext.Current.Request.PhysicalApplicationPath & "Portal\Logs\"
        Dim FileName As String = PathFile & iDay.ToString("00") & iMonth.ToString("00") & iYear.ToString("0000") & ".log"
        Dim FileError As New System.IO.FileInfo(FileName)


        If IO.Directory.Exists(PathFile) Then
            FileError = New System.IO.FileInfo(FileName)
            If FileError.Exists Then
                ReadFile(FileName)
            Else
                If iDay = 0 Then
                    lblError.Text = ""
                    Literal1.Text = GetFiles(PathFile, iYear, iMonth)
                    Return True
                End If
                Literal1.Text = ""
                lblError.Text = String.Format("The Log file '{0}' cannot be found", FileName)
            End If
            lblError.Text = String.Format("Path file '{0}' cannot be found", FileName)
        End If
        Return True
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub cmdRead_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRead.Click
        ReadLog()
    End Sub

End Class