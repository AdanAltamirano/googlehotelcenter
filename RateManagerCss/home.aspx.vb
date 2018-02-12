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
                    msginfo = .HotelName & "<br>"
                    msginfo &= "id" & PortalCulture.GetString("M000614", True) & .Empresa & "<br>"
                    msginfo &= "idHotel: " & .Hotel & "<br>"
                    msginfo &= .Address & "<br>"
                    msginfo &= .City & "," & .State & "<br>"
                    msginfo &= PortalCulture.GetString("00162", True) & "&nbsp;" & .Contact & "<br>"
                    msginfo &= PortalCulture.GetString("00163", True) & "&nbsp;" & .Email & "<br>"
                    msginfo &= PortalCulture.GetString("00164", True) & "&nbsp;" & .Phone & "<br>"
                    msginfo &= PortalCulture.GetString("00522", True) & "&nbsp;" & IIf(String.IsNullOrEmpty(.Url), "-", .Url) & "<br>"
                    msginfo &= PortalCulture.GetString("00074", True) & "&nbsp;" & .Rooms.ToString & "<br>"
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