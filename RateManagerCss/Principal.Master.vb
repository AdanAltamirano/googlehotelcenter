Imports System.Xml
Imports LoginAuthenticate

Partial Public Class PrincipalMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lnkTicketList.HRef = GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/HotelAdministrator/Pages/TicketList.aspx"))
        lnkTicket.HRef = GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/HotelAdministrator/Pages/TicketRegister.aspx"))
        lnkhelp.HRef = GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/Documentacion/ayuda.aspx"))

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Dim Version As String = ""
        Dim dsVersion As New XmlDataDocument
        dsVersion.DataSet.ReadXml(Request.PhysicalApplicationPath & "/Portal/Modules/Version.xml")
        If Not dsVersion.DataSet.Tables("Versiones") Is Nothing AndAlso dsVersion.DataSet.Tables("Versiones").Rows.Count > 0 Then
            Version = dsVersion.DataSet.Tables("Versiones").Rows(0).Item("Version").ToString
        End If
        Me.lblTitle.Text = PortalCulture.GetString("00157") & " " & Version

        If (New AuthUser).IsAuthenticated Then
            Me.linkLogOut.Visible = True
            lblUser.Text = ReadUserCookie.GetValue(0)
        End If


        Me.lnkNameCompany.Text = PortalCulture.GetString("00158")

        If CType(Me.Page, PaginaBase).IsHotelSelected Then
            If Not Session(PaginaBase.SESSION_INFO) Is Nothing Then
                Dim cInfo As companyInfo = Session(PaginaBase.SESSION_INFO)
                If cInfo.Hotel > 0 Then
                    With cInfo
                        Me.lnkNameCompany.Text = .HotelName
                        'Me.imgArrow.Visible = True
                        Dim msgInfo As String
                        msgInfo = .HotelName & "<br>"
                        msgInfo &= .Address & "<br>"
                        msgInfo &= .City & "," & .State & "<br>"
                        msgInfo &= "<span class='caption'>" & PortalCulture.GetString("00162", True) & "</span>" & .Contact & "<br>"
                        msgInfo &= "<span class='caption'>" & PortalCulture.GetString("00163", True) & "</span>" & .Email & "<br>"
                        msgInfo &= "<span class='caption'>" & PortalCulture.GetString("00164", True) & "</span>" & .Phone & "<br>"
                        ' Me.Page.RegisterStartupScript("", "<script>SetDiv('" & msgInfo & "')</script>")
                        lblInfoHotel.Text = msgInfo
                    End With
                End If
            End If
        Else
            lblInfoHotel.Text = ""
        End If

    End Sub

    Public Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function


    Public Function GeRequestApplicationPath(ByVal page As String) As String
        Dim sreq As String
        sreq = String.Concat(Request.ApplicationPath, page).Replace("//", "/").Replace("//", "/")
        Return sreq
    End Function

    Protected Sub linkLogOut_Click(ByVal sender As Object, ByVal e As EventArgs) Handles linkLogOut.Click
        With New clsUsuario
            .LogOut(Session("ticketId"))
        End With
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session.Abandon()
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            FormsAuthentication.SignOut()
        End If

        Response.Redirect(GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/Default.aspx?Url=", "/Portal/Pages/Welcome.aspx")))

    End Sub


    Private Function ReadUserCookie() As String()
        Try
            Dim UserInfoArray As String() = {""}
            '// Obtenemos el usuario de la session.
            UserInfoArray = CType((New AuthUser).UserInfoName, String).Split(",")
            Return (UserInfoArray)

            '// Esta parte no se debe ejecutar.
            ' Obtenemos los roles desde la cookie
            Dim CookieName As String = FormsAuthentication.FormsCookieName() & "UI"
            If Not Context.Request.Cookies(CookieName) Is Nothing Then
                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(Context.Request.Cookies(CookieName).Value)
                If Not ticket Is Nothing Then
                    'convertimos a arreglo de elementos la lista de cookies                                
                    UserInfoArray = ticket.UserData.Split(CType(",", Char))
                    If UserInfoArray.Length > 0 Then
                        ReadUserCookie = UserInfoArray
                    End If
                End If
            End If
        Catch cryptoException As System.Security.Cryptography.CryptographicException
            ' Si hay problemas al leer la cookie por aplicacion hacemos que la elimine el navegador
            Dim ck As HttpCookie
            ck = cSecurity.createUserCookie(0, 0, False, New String() {""})
            Response.Cookies.Add(ck)
        Catch e As Exception
        Finally
        End Try
    End Function

End Class