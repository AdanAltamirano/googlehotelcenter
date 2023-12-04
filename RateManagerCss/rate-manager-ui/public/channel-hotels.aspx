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