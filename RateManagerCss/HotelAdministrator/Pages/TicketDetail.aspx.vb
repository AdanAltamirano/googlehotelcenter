Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade

Partial Public Class TicketDetail
    Inherits PaginaBase

    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function

    Protected ReadOnly Property TicketID() As Integer
        Get
            Dim temp As Integer = 0
            If Me.Request.Params("id") IsNot Nothing Then Integer.TryParse(Me.Request.Params("id"), temp)
            Return temp
        End Get
    End Property

    Private _ticketData As TicketData
    Protected ReadOnly Property TicketData() As TicketData
        Get
            If Me._ticketData Is Nothing AndAlso Me.TicketID > 0 Then
                Dim controller As New TicketFacade
                Me._ticketData = controller.GetById(Me.TicketID)
            End If
            Return Me._ticketData
        End Get
    End Property

    Protected ReadOnly Property IsValidRequest() As Boolean
        Get
            Return (Me.TicketID > 0 AndAlso Me.TicketData IsNot Nothing AndAlso Me.TicketData.Exists)
        End Get
    End Property

    Protected ReadOnly Property IsEditable() As Boolean
        Get
            Dim flag As Boolean = Me.IsValidRequest
            If flag Then flag = (Me.TicketData.Status = TicketData.TicketStatus.InProcess)
            'If flag Then flag = (Me.TicketData.Owner = Me.cInfoActual.Hotel)
            Return flag
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not Me.IsPostBack Then
            If Not Me.IsValidRequest Then
                Me.Response.Redirect("TicketList.aspx", True)
            Else
                Me.FillComments()
            End If
        End If
    End Sub

    Private Sub FillComments()
        If Me.IsValidRequest Then
            Me.txtDescripcion.Text = String.Empty
            Dim controller As New TicketFacade()
            Me.lstComments.DataSource = controller.GetComments(Me.TicketID)
            Me.lstComments.DataBind()
        End If
    End Sub

    Protected Sub btnActualizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        If txtDescripcion.Text.Trim().Length > 0 AndAlso Me.IsValidRequest Then

            Try
                Dim controller As New TicketFacade()
                controller.PostComment(Me.TicketID, Me.txtDescripcion.Text.Trim())

                Try
                    Dim Mail As emailTemplates.Template = New emailTemplates.Template()

                    Mail.Idioma = PortalCulture.GetCulture().ToString()
                    Try
                        Mail.TemplateName = "TK_NewCommentSupport"
                    Catch ex As Exception
                    End Try

                    Mail.To = Me.TicketData.Emails
                    Mail.Html = True
                    Mail.SubjectParam = Me.TicketData.Id.ToString().PadLeft(10, "0") + " - " + Me.TicketData.Subject

                    Mail.AddParameter("HEADER") = " "
                    Mail.AddParameter("FOOTER") = " "
                    Mail.AddParameter("LINKCSS") = "<link href='" & ConfigurationManager.AppSettings("TicketUrl") & "Correo/Style.css' type='text/css' rel='stylesheet'>"

                    Mail.AddParameter("USER") = Me.TicketData.OwnerName
                    Mail.AddParameter("USERMAIL") = Me.TicketData.OwnerEmail
                    Mail.AddParameter("COMMENT") = Me.txtDescripcion.Text.Trim()
                    Mail.AddParameter("ID") = Me.TicketData.Id.ToString().PadLeft(10, "0")

                    Mail.Send()

                Catch Emsg As Exception
                Finally
                End Try

                Me.FillComments()
            Catch ex As Exception
                Me.ClientScript.RegisterStartupScript(GetType(String), "ErrorMessage", "alert('" + Me.GetLabel("01306") + "');", True)
            End Try

        End If
    End Sub

    Protected Sub btnGetFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetFile.Click
        If Me.IsValidRequest Then
            Dim info As TicketFileData = (New TicketFacade()).GetFile(Me.TicketID)
            If info IsNot Nothing AndAlso info.Exists Then

                Me.Response.Clear()
                Me.Response.ClearContent()
                Me.Response.ClearHeaders()
                Me.Response.Buffer = True
                Me.Response.AddHeader("content-disposition", "attachment;filename=" + info.Name)
                Me.Response.ContentType = info.Type
                Me.Response.Charset = "UTF-8"
                Me.Response.ContentEncoding = Encoding.Default
                Me.Response.BinaryWrite(info.Content)
                Me.Response.End()

            Else
                ClientScript.RegisterStartupScript(GetType(String), "ErrorMessage", "alert('" + Me.GetLabel("01307") + "');")
            End If
        End If
    End Sub

End Class