<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReservationList.aspx.vb" Inherits="RateManager.Reservations" %>



<html>
    <head></head>
    <body>
        <script src="https://cdn.jsdelivr.net/npm/vue"></script>
        <script src="https://unpkg.com/axios/dist/axios.min.js"></script>
        <script>
            var engine =
                {
                    methods:
                        {
                            GetReservationList: function ()
                            {

                            }
                        }
                }

            var app = new Vue(engine);
        </script>
    </body>
</html>


