Imports System.Web
Imports System.Web.Services
Imports Portal.Hotel.DataAccess

Public Class WaitList1
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim id As Integer = 0
        Dim lang As Integer = 1
        Dim response As String = String.Empty

        With context
            If .Request.Params("lang") IsNot Nothing Then Integer.TryParse(.Request.Params("lang"), lang)

            If .Request.Params("item") IsNot Nothing AndAlso Integer.TryParse(.Request.Params("item"), id) AndAlso id > 0 Then
                Dim controller As New WaitListDataAccess()
                Dim data As DataTable = controller.GetItem(id, lang)
                If data IsNot Nothing AndAlso data.Rows.Count > 0 Then
                    With data.Rows(0)
                        Dim sysCulture As System.Globalization.CultureInfo
                        sysCulture = System.Threading.Thread.CurrentThread.CurrentCulture
                        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(If(lang = 2, "en-US", "es-MX"))

                        response += "{"

                        Dim columns As String() = {"id", "date", "name", "phone", "email", "cellPhone", "comment", "checkIn", "checkOut", "adults", "rooms", "children", "rateCode", "status", "notifyPrice", "notifyComment", "notifyCurrency", "roomName", "roomDescription", "rateName", "rateDescription", "actor"}
                        Dim sep As String = String.Empty
                        For Each column As String In columns
                            If .Table.Columns.Contains(column) AndAlso Not .IsNull(column) Then
                                Try
                                    If .Table.Columns(column).DataType Is GetType(Date) Then
                                        response += sep + """" + column + """:""" + Convert.ToDateTime(.Item(column)).ToString(If(column = "date", "dd/MMM/yyyy HH:mm", "dd/MMM/yyyy")) + """"
                                    ElseIf .Table.Columns(column).DataType Is GetType(Integer) Then
                                        response += sep + """" + column + """:" + Convert.ToInt32(.Item(column)).ToString()
                                    ElseIf .Table.Columns(column).DataType Is GetType(Double) Then
                                        response += sep + """" + column + """:""" + Convert.ToDouble(.Item(column)).ToString("0.00") + """"
                                    Else
                                        response += sep + """" + column + """:""" + .Item(column).ToString() + """"
                                    End If
                                    sep = ", "
                                Catch ex As Exception
                                End Try
                            End If
                        Next

                        If sep = ", " AndAlso .Table.Columns.Contains("checkIn") AndAlso Not .IsNull("checkIn") Then
                            Dim checkIn As New Date(2000, 1, 1)
                            Date.TryParse(.Item("checkIn"), checkIn)
                            response += sep + """isOlder"":""" + If(checkIn > Today, "false", "true") + """"
                        End If

                        response += "}"

                        System.Threading.Thread.CurrentThread.CurrentCulture = sysCulture
                    End With
                End If
            End If

            .Response.ContentType = "application/json"
            .Response.ContentEncoding = Encoding.UTF8
            .Response.Write(If(response.ToString().Trim().Length = 0, "Empty result", response.Trim()))
        End With

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class