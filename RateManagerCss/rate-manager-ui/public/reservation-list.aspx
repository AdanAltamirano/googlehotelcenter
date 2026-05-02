<%@ Page Language="vb" AutoEventWireup="false" Inherits="RateManager.PaginaBase" %>
<script runat="server">
    Public RedirectFrom As String = ""
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Init     
        RedirectFrom = If(Request.QueryString("redirectfrom"), "")   
    End Sub
</script>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
</head>
<body>
    <script>
        window.app =
            {
                hotelId: <%= Me.cInfoActual.Hotel%>,
                language: '<%= If(Me.IdIdiomaMenu = 1, "es", "en" )%>',
                corporateId: <%= Me.cInfoActual.IdCorporate%>,
                corporateName: '<%= Me.cInfoActual.CorporateName%>',
                isAgencyCompany: '<%= Me.IsAgencyCompany%>',
                isSupervisor : '<%= Me.IsSupervisor%>',
                isHotelUser : '<%= Me.IsUsuarioHotel%>',
                isHotelCompany : '<%= Me.IsHotel%>',
                redirectFrom: '<%= Me.RedirectFrom%>'
            };
    </script>
    <div id="app"></div>
</body>
</html>