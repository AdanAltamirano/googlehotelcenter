<%@ Page Title="" Language="vb" AutoEventWireup="false"  EnableEventValidation="true"  MasterPageFile="~/Principal.Master"
    CodeBehind="Home.aspx.vb" Inherits="RateManager.Default_Home" %>


<asp:Content ID="cntHead" ContentPlaceHolderID="head" runat="server">
<script src="Includes/Script/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="JavaScript">
        jQuery.noConflict();    
        
        function calcHeight(addHig) {
            try {
                //frzhIframeResize();
                var iframe = document.getElementById('frmPrincipal');
                if (iframe) {
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
                    }
                }
            }
            catch (err) {
                //alert(err);
            }
        }



        function resizeIframe(addHig) {
        
            calcHeight(addHig);
        }

        function btnReload() {
            document.getElementById("btnreload").click();

        }
        function UpdateMe() { //alert(1);                   
            // self.location.href =  "/default.aspx"
        }

        function RemoveDumpUrl(url) {
            if (url) {
                var i = url.indexOf(".aspx");
                if (i != -1) {
                    url = url.substring(0, (i + 5));


                }
            }
            return url;
        }

 
        function SetOptionMenu(url) {
            try {
                setTimeout(functionSetOptionMenu(url), 300); ;
            }
            catch (err) {

            }
        }

        function functionSetOptionMenu(url) {
            try {

                if (url) {
                    var find = false;
                    url = RemoveDumpUrl(url).toLowerCase();

                    if (url.indexOf("portal/pages/welcome.aspx") != -1) {
                        var home = jQuery("#menuPrincipal a[href*='Portal/Pages/Home.aspx']");
                        if (home.length != 0) {
                            HideMenu();
                            home.parent("li").attr("class", "activo-main-menu");
                            find = true;
                            return;
                        }

                    }
                    else {

                        
                        var currentUrl = RemoveDumpUrl(document.URL).toLowerCase().replace("home.aspx", "");
                        url = (url.replace(currentUrl, ""));

                        //var option = jQuery("#submenuDiv a[href*='" + url + "']");

                        var a = jQuery("#submenuDiv a").each(function(i, option) {
                            var href = jQuery(option).attr("href").toLowerCase();
                            if (href.endsWith(url)) {
                                var li_sub_option = jQuery(option).parent().parent().parent();
                                var div_sub_option = li_sub_option.parent().parent();
                                var id_li = div_sub_option.attr("id").replace("_div", "_li");
                                var li_option = jQuery("#menuPrincipal li[id=" + id_li + "]");
                                if (li_sub_option.length != 0 && div_sub_option.length != 0 && li_option.length != 0) {
                                    if (li_sub_option.attr("class") != "activo-subSubmenu" || li_option.attr("class") != "activo-main-menu") {
                                        HideMenu();
                                        div_sub_option.show();                                       
                                        li_option.attr("class", "activo-main-menu");
                                        li_sub_option.attr("class", "activo-subSubmenu");
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

        }

    </script>

</asp:Content>
<asp:Content ID="ctntPrincipal" ContentPlaceHolderID="ContainerPage" runat="server">
    <asp:Button ID="btnreload" Style="display: none" runat="server"></asp:Button>    
    <iframe  id="frmPrincipal" name="frmPrincipal" src='Portal/Pages/Welcome.aspx' class="frmPrincipal"
        scrolling="auto" width="90%" frameborder="0px" height="1" style="border: 0;">
    </iframe>
</asp:Content>
