<%@ Page Language="vb" AutoEventWireup="false" Inherits="RateManager.PaginaBase" %>
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width,initial-scale=1.0">
    <title>rates-admin</title>
  </head>
  <body>
    <script language="vb" runat="server">
      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(RateManager.PaginaBase.pages.Home)
      End Sub
    </script>
    <noscript>
      <strong>We're sorry but rates-admin doesn't work properly without JavaScript enabled. Please enable it to continue.</strong>
    </noscript>
    <script>
      window.app = {
        hotelId: <%= Me.cInfoActual.Hotel%>,
        language: '<%= If(Me.IdIdiomaMenu = 1, "es", "en")%>'
      }
    </script>
    <div id="app"></div>
    <!-- built files will be auto injected -->
  </body>
</html>