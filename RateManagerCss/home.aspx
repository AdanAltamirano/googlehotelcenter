<%@ Page Title="" Language="vb" AutoEventWireup="false"  EnableEventValidation="true"  MasterPageFile="~/Principal.Master"
    CodeBehind="Home.aspx.vb" Inherits="RateManager.Default_Home" %>


<asp:Content ID="cntHead" ContentPlaceHolderID="head" runat="server">
<script src="Includes/Script/jquery-1.4.2.min.js" type="text/javascript"></script>
<script type="text/javascript" language="JavaScript">
        jQuery.noConflict();   

        setInterval(function () {
            calcHeight();
        }, 600);

        function calcHeight()
        {
            var iframe = document.getElementById('frmPrincipal');
            if (iframe)
            {
                iframe.visibility = 'hidden';
                iframe.height = '10px';

                var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
                if (iframeWin.document.body)
                    iframe.height = (iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight) + 50;

                var height = ($(iframe).outerHeight() + $('.wrapper').offset().top);

                var minHeight = $(window).outerHeight() - $('.wrapper').offset().top - $('footer').outerHeight();

                if (minHeight > height)
                    height = minHeight;
                else if (height < 300)
                    height += 350;
                else if (height < 500) height = 500;

                $('.wrapper, .overlay, #sidebar').css('height', height);

                iframe.visibility = 'visible';
            }
        }

  
        function resizeIframe(addHig)
        {
            calcHeight(addHig);
        }

        function btnReload()
        {
            document.getElementById("btnreload").click();
        }

        function UpdateMe() { //alert(1);                   
            // self.location.href =  "/default.aspx"
        }

        function RemoveDumpUrl(url)
        {
            if (url)
            {
                var i = url.indexOf(".aspx");
                if (i != -1) url = url.substring(0, (i + 5));
            }

            return url;
        }

 
        function SetOptionMenu(url)
        {
            setTimeout(functionSetOptionMenu(url), 300);
        }


        function functionSetOptionMenu(url)
        {
            if (url)
            {
                url = RemoveDumpUrl(url).toLowerCase();

                if (url.indexOf('portal/pages/welcome.aspx') != -1)
                {
                    var isHome = jQuery('#menuPrincipal a[href*="Portal/Pages/Home.aspx"]');
                    if (isHome.length > 0)
                    {
                        HideMenu();
                        isHome.parent().addClass('active');
                        jQuery('.followMenu').html(capitalize(isHome.html()));
                    }
                } else
                {
                    var currentUrl = RemoveDumpUrl(document.URL).toLowerCase().replace('home.aspx', '');
                    url = url.replace(currentUrl, '');

                    jQuery('#submenuDiv a').map(function(i, tag){

                        var href = jQuery(tag).attr('href').toLowerCase();
                        if (href.endsWith(url))
                        {
                            var li_option_focus = jQuery(tag).parent();
                            var ul_menu = li_option_focus.parent();
                            var li_option_menu = jQuery('#menuPrincipal li[id=' + ul_menu.attr('id').replace('_div', '_li') + ']');

                            if (li_option_focus.attr('class') != 'activo-subSubmenu' || li_option_menu.attr('class') != 'active')
                            {
                                HideMenu();
                                jQuery('#sidebarContent').show();
                                ul_menu.show();
                                li_option_focus.addClass('activo-subSubmenu');
                                li_option_menu.addClass('active');

                                var menuName = capitalize(li_option_menu.children().eq(0).text());
                                var subMenuName = capitalize(li_option_focus.children().html());
                                var home = capitalize(jQuery('#menuPrincipal li a').eq(0).text());
                                jQuery('#titleSideBar').html(menuName);
                                jQuery('.followMenu').html(home + ' / ' + menuName + ' / ' + subMenuName)
                            }
                        }
                    })
                }

                calcHeight();
            }
        }

        function capitalize(s)
        {
            s = s.toLowerCase();
            return s.charAt(0).toUpperCase() + s.slice(1);
        }

    </script>
</asp:Content>
<asp:Content ID="ctntPrincipal" ContentPlaceHolderID="ContainerPage" runat="server">
    <asp:Button ID="btnreload" Style="display: none" runat="server"></asp:Button>    
    <iframe  id="frmPrincipal" name="frmPrincipal" src='Portal/Pages/Welcome.aspx' class="frmPrincipal"
        scrolling="auto" width="90%" frameborder="0px" height="1" style="border: 0;">
    </iframe>
</asp:Content>
