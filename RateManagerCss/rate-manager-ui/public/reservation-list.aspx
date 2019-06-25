<%@ Page Language="vb" AutoEventWireup="false" Inherits="RateManager.PaginaBase" %>
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
            };
    </script>
    <div id="app"></div>
</body>
</html>