Imports System.Configuration.ConfigurationManager
Partial Public Class Default_Home
    Inherits PaginaBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("menu") = "false"

        If Not IsPostBack Then
            
            InitIdiomaLogin = AppSettings("DefaultLanguajeLogin")
            If Not Request.QueryString("ididioma") Is Nothing AndAlso Request.QueryString("ididioma") = 1 Then
                PortalCulture.SetCulture("es-MX")
                Me.IdIdiomaLogin = 1
            ElseIf Not Request.QueryString("ididioma") Is Nothing AndAlso Request.QueryString("ididioma") = 2 Then
                PortalCulture.SetCulture("en-US")
                Me.IdIdiomaLogin = 2
            End If


            If Request.QueryString("SRV") = "S" Then
                Dim msginfo As String
                With Me.cInfoActual
                    msginfo = "<a class=""dropdown-item"">" & .HotelName & "</a>"
                    msginfo &= "<a class=""dropdown-item"">id" & PortalCulture.GetString("M000614", True) & "<b>" & .Empresa & "</b></a>"
                    msginfo &= "<a class=""dropdown-item"">idHotel: <b>" & .Hotel & "</b></a>"
                    msginfo &= "<a class=""dropdown-item""><b>" & .Address & "</b></a>"
                    msginfo &= "<a class=""dropdown-item""><b>" & .City & "," & .State & "</b></a>"
                    msginfo &= "<a class=""dropdown-item"">" & PortalCulture.GetString("00162", True) & "&nbsp;" & "<b>" & .Contact & "</b></a>"
                    msginfo &= "<a class=""dropdown-item"">" & PortalCulture.GetString("00163", True) & "&nbsp;" & "<b>" & .Email & "</b></a>"
                    msginfo &= "<a class=""dropdown-item"">" & PortalCulture.GetString("00164", True) & "&nbsp;" & "<b>" & .Phone & "</b></a>"
                    msginfo &= "<a class=""dropdown-item"">" & PortalCulture.GetString("00522", True) & "&nbsp;" & "<b>" & IIf(String.IsNullOrEmpty(.Url), "-", .Url) & "</b></a>"
                    msginfo &= "<a class=""dropdown-item"">" & PortalCulture.GetString("00074", True) & "&nbsp;" & "<b>" & .Rooms.ToString & "</b></a>"
                End With
                Context.Response.Write(msginfo)
                Context.Response.End()
            End If
        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        Dim b As Char = ControlChars.Cr
        Dim s As New System.Text.StringBuilder
        With s
            .Append("<script>" & b)
            .Append("	  function UpdateMe()" & b)
            .Append("	  { " & b)
            .Append("        //recarga la pagina" & b)
            .Append("        self.location.href='" & (GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/home.aspx';"))).Replace("//", "/") & b)
            .Append("     }" & b)
            .Append("		</script>" & b)
        End With
        writer.Write(s.ToString)
    End Sub


End Class