<%@ Page Title="" Language="vb" AutoEventWireup="false"  EnableEventValidation="true"  MasterPageFile="~/Principal.Master"
    CodeBehind="Home.aspx.vb" Inherits="RateManager.Default_Home" %>


<asp:Content ID="cntHead" ContentPlaceHolderID="head" runat="server">
<script src="Includes/Script/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="JavaScript">
        jQuery.noConflict();   

        function calcHeight()
        {
            var iframe = document.getElementById('frmPrincipal');
            if (iframe)
            {
                iframe.visibility = 'hidden';
                iframe.height = '10px';

                var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
                if (iframeWin.document.body)
                    iframe.height = (iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight) + 10;

                var height = (parseInt(iframe.height) + $(iframe).offset().top);
                if ($('#mCSB_1_container').height() > height)
                    height = $('#mCSB_1_container').height();
                else if (height < 300)
                    height += 350;
                else if (height < 500) height = 500;

                $('.wrapper, .overlay, #sidebar').css('height', height);

                iframe.visibility = 'visible';
            }
        }

        /*function _calcHeight(addHig) {
            try {
                //frzhIframeResize();
                var iframe = document.getElementById('frmPrincipal');
                if (iframe)
                {
                    
                    iframe.height = '1px';
                    iframe.style.height = '1px';
                    var the_height = document.getElementById('frmPrincipal').contentWindow.document.body.offsetHeight;
                    if (the_height == '1') {
                        iframe.height = 1;
                        the_height = iframe.contentWindow.document.body.scrollHeight;
                        if (the_height < 300)
                            the_height = 500;
                        else {
                            the_height += 400;
                            if (isNaN(addHig) == false) {
                                the_height += addHig;
                            }
                        }

                        iframe.height = the_height + 'px';
                        iframe.style.height = the_height + 'px';

                    }
                    else {
                        if (the_height < 300)
                            the_height = 600;
                        else {
                            the_height += 400;
                            if (isNaN(addHig) == false) {
                                the_height += addHig;
                            }
                        }
                        iframe.height = the_height + 'px';
                        iframe.style.height = the_height + 'px';

                        $('.wrapper').css('height', the_height);
                    }
                }
            }
            catch (err) {
                //alert(err);
            }
        }*/



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
                                jQuery('#titleSideBar').html(menuName);
                                jQuery('.followMenu').html('/ ' + menuName + ' / ' + subMenuName)
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

        /*function _functionSetOptionMenu(url) {
            try {

                if (url) {
                    var find = false;
                    url = RemoveDumpUrl(url).toLowerCase();

                    if (url.indexOf("portal/pages/welcome.aspx") != -1) {
                        var home = jQuery("#menuPrincipal a[href*='Portal/Pages/Home.aspx']");
                        if (home.length != 0) {
                            HideMenu();
                            home.parent("li").addClass("active");
                            find = true;
                            return;
                        }

                    }
                    else {

                        
                        var currentUrl = RemoveDumpUrl(document.URL).toLowerCase().replace("home.aspx", "");
                        url = (url.replace(currentUrl, ""));

                        //var option = jQuery("#submenuDiv a[href*='" + url + "']");


                        jQuery('#submenuDiv a').map(function(i, tag){
                            console.log(i, tag);
                        })

                        var a = jQuery("#submenuDiv a").each(function(i, option) {
                            var href = jQuery(option).attr("href").toLowerCase();
                            if (href.endsWith(url)) {
                                var li_sub_option = jQuery(option).parent();
                                console.log("wp: " + li_sub_option)
                                var div_sub_option = li_sub_option.parent();
                                var id_li = div_sub_option.attr("id").replace("_div", "_li");
                                var li_option = jQuery("#menuPrincipal li[id=" + id_li + "]");
                                if (li_sub_option.length != 0 && div_sub_option.length != 0 && li_option.length != 0) {
                                    if (li_sub_option.attr("class") != "activo-subSubmenu" || li_option.attr("class") != "activo-main-menu") {
                                        HideMenu();
                                        div_sub_option.show();                                       
                                        //li_option.attr("class", "activo-main-menu");
                                        li_option.addClass("active");
                                        li_sub_option.addClass("class", "activo-subSubmenu");
                                    }
                                }
                                else {
                                    HideMenu();
                                }
                                find = true;
                                return true;
                            }


                        });
                        if (!find) {
                            HideMenu();
                        }
                    }
                }
            }
            catch (err) {
               // alert(err);
            }
            return false;

        }*/

    </script>

</asp:Content>
<asp:Content ID="ctntPrincipal" ContentPlaceHolderID="ContainerPage" runat="server">
    <asp:Button ID="btnreload" Style="display: none" runat="server"></asp:Button>    
    <iframe  id="frmPrincipal" name="frmPrincipal" src='Portal/Pages/Welcome.aspx' class="frmPrincipal"
        scrolling="auto" width="90%" frameborder="0px" height="1" style="border: 0;">
    </iframe>
</asp:Content>
