Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade

Partial Public Class TicketRegister
    Inherits PaginaBase

    Protected ReadOnly Property WithList() As Boolean
        Get
            Dim _withList As Boolean = False
            Boolean.TryParse(ConfigurationManager.AppSettings("TicketIssueList"), _withList)
            Return _withList
        End Get
    End Property

    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not Me.IsPostBack Then
            Me.pnlForm.Visible = True
            Me.pnlMessage.Visible = False

            
            Me.lstSubject.Visible = withList
            Me.txtSubject.Visible = Not withList
            If withList Then
                Me.lstSubject.Items.Clear()
                Dim controller As New TicketFacade()
                For Each item As KeyValuePair(Of Integer, String) In controller.GetIssueList(PortalCulture.GetCulture().ToString())
                    Me.lstSubject.Items.Add(New ListItem(item.Value, item.Key))
                Next
                Me.lstSubject.SelectedIndex = -1
            End If

        End If
    End Sub

    Protected Sub Send(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click

        If Me.IsValid Then

            If Me.txtFile.HasFile Then
                Dim maxLength As Integer = 0
                Integer.TryParse(ConfigurationManager.AppSettings("TicketMaxFileLenght"), maxLength)

                If maxLength = 0 OrElse Me.txtFile.PostedFile.ContentLength <= (maxLength * 1024) Then
                    Dim extension As String = String.Empty
                    Dim idx As Integer = Me.txtFile.PostedFile.FileName.LastIndexOf(".")
                    If idx > 0 Then extension = Me.txtFile.PostedFile.FileName.Remove(0, idx)
                    Dim allowed As New List(Of String)
                    allowed.AddRange(ConfigurationManager.AppSettings("TicketAllowedFiles").Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries))

                    If Not (extension.Trim().Length > 0 AndAlso allowed.Contains(extension)) Then
                        Me.ClientScript.RegisterStartupScript(GetType(String), "ErrorMessage", "alert('" + Me.GetLabel("01308") + "');", True)
                        Exit Sub
                    End If
                Else
                    Me.ClientScript.RegisterStartupScript(GetType(String), "ErrorMessage", "alert('" + String.Format(Me.GetLabel("01309"), maxLength.ToString()) + "');", True)
                    Exit Sub
                End If
            End If


            Dim info As New TicketData()
            Dim fileInfo As New TicketFileData()
            Dim controller As New TicketFacade()

            info.Owner = Me.cInfoActual.Hotel
            info.OwnerCompany = Me.cInfoActual.HotelName
            info.OwnerEmail = Me.txtEmail.Text.Trim()
            info.OwnerName = Me.txtName.Text.Trim()
            If Me.WithList Then
                info.Subject = Me.lstSubject.SelectedItem.Text
            Else
                info.Subject = Me.txtSubject.Text.Trim()
            End If

            If Me.txtFile.HasFile Then
                With Me.txtFile
                    fileInfo.Name = .FileName
                    fileInfo.Content = .FileBytes
                    fileInfo.Type = .PostedFile.ContentType
                End With
            End If

            Dim reference As Integer = 0
            reference = controller.Add(info, Me.txtDescription.Text.Trim(), "RateManager", PortalCulture.GetCulture().ToString(), fileInfo)

            If reference > 0 Then

                Try
                    Dim Mail As emailTemplates.Template = New emailTemplates.Template()

                    Mail.Idioma = PortalCulture.GetCulture().ToString()
                    Try
                        Mail.TemplateName = "TK_NotificationCustomer"
                    Catch ex As Exception
                    End Try

                    Mail.To = info.OwnerEmail
                    Mail.Html = True
                    Mail.SubjectParam = info.Subject

                    Dim Prov As New PortalPartnersCfg
                    Prov.LoadPartnerById(2)

                    Mail.AddParameter("HEADER") = Prov.EmailHeader
                    Mail.AddParameter("FOOTER") = Prov.EmailFooter
                    Mail.AddParameter("LINKCSS") = "<link href='" & ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css' type='text/css' rel='stylesheet'>"
                    Mail.AddParameter("USER") = info.OwnerName
                    Mail.AddParameter("COMMENT") = Me.txtDescription.Text.Trim()
                    Mail.AddParameter("SUBJECT") = info.Subject
                    Mail.AddParameter("REFERENCE") = reference.ToString().PadLeft(10, "0")
                    Mail.Send()

                Catch Emsg As Exception
                Finally
                End Try

                Try
                    Dim Mail As emailTemplates.Template = New emailTemplates.Template()

                    Mail.Idioma = PortalCulture.GetCulture().ToString()
                    Try
                        Mail.TemplateName = "TK_NotificationSupport"
                    Catch ex As Exception
                        Dim a = 0
                    End Try

                    Mail.To = ConfigurationManager.AppSettings("TiketDefaultEmailAssignament")
                    Mail.Html = True
                    Mail.SubjectParam = reference.ToString().PadLeft(10, "0") + " - " + info.Subject

                    Mail.AddParameter("HEADER") = " "
                    Mail.AddParameter("FOOTER") = " "
                    Mail.AddParameter("LINKCSS") = "<link href='" & ConfigurationManager.AppSettings("TicketUrl") & "Correo/Style.css' type='text/css' rel='stylesheet'>"

                    Mail.AddParameter("USER") = info.OwnerName
                    Mail.AddParameter("COMPANY") = info.OwnerCompany
                    Mail.AddParameter("SUBJECT") = info.Subject
                    Mail.AddParameter("COMMENT") = Me.txtDescription.Text.Trim()
                    Mail.AddParameter("ID") = reference.ToString().PadLeft(10, "0")

                    Mail.Send()

                Catch Emsg As Exception
                Finally
                End Try

                Me.pnlForm.Visible = False
                Me.pnlMessage.Visible = True
                Me.lblReferece.InnerText = reference.ToString().PadLeft(10, "0")
            End If
        End If

    End Sub


    Public Shared Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function


End Class