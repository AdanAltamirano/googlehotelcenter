<%@ Page Language="vb" AutoEventWireup="false" Inherits="RateManager.PaginaBase" %> <!DOCTYPE html><html lang=en><head><meta charset=UTF-8><meta name=viewport content="width=device-width,initial-scale=1"><meta http-equiv=X-UA-Compatible content="ie=edge"><title>Document</title><link href=css/commons.86958629.css rel=preload as=style><link href=css/reservation_list.b2798f14.css rel=preload as=style><link href=js/commons.95015a7a.js rel=preload as=script><link href=js/reservation_list.76514698.js rel=preload as=script><link href=css/commons.86958629.css rel=stylesheet><link href=css/reservation_list.b2798f14.css rel=stylesheet></head><body><script>window.app =
            {
                hotelId: <%= Me.cInfoActual.Hotel%>,
                language: '<%= If(Me.IdIdiomaMenu = 1, "es", "en" )%>',
                corporateId: <%= Me.cInfoActual.IdCorporate%>,
                corporateName: '<%= Me.cInfoActual.CorporateName%>',
		isAgencyCompany: '<%= Me.IsAgencyCompany%>'
            };</script><div id=app></div><script src=js/commons.95015a7a.js></script><script src=js/reservation_list.76514698.js></script></body></html>