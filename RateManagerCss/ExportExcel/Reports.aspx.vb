Imports System.IO

Public Class Reports1
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Clear()
            Response.Buffer = True
            Response.ClearHeaders()
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("content-disposition", "attachment;filename=report" & Date.Now.ToShortDateString() & ".xls")
            Response.Charset = ""
            Response.ContentEncoding = Encoding.Unicode
            Response.BinaryWrite(Encoding.Unicode.GetPreamble())
            Response.ContentType = "application/ms-excel"

            Report()
        Catch ex As Exception
            Response.Clear()
        Finally
            Response.End()
        End Try
    End Sub

    Sub Report()
        If Session("reportExcel") IsNot Nothing Then

            Dim stringWrite As New StringWriter
            Dim htmlWrite As New HtmlTextWriter(stringWrite)
            Dim dt As New DataTable

            Try
                dt = CType(Session("reportExcel"), DataTable).Copy()
            Catch ex As Exception
                dt = Nothing
            End Try

            If dt IsNot Nothing Then
                If Request.QueryString("source") IsNot Nothing Then
                    If Request.QueryString("source") = "ip" Then
                        dgReportIP.DataSource = dt
                        dgReportIP.DataBind()
                        dgReportIP.RenderControl(htmlWrite)
                    Else
                        dgReport.DataSource = dt
                        dgReport.DataBind()
                        dgReport.RenderControl(htmlWrite)
                    End If
                End If
            End If
            Response.Write(stringWrite.ToString())
        End If
    End Sub
End Class