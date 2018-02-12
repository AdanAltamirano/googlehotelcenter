<%@ Register TagPrefix="uc1" TagName="CtrlPreserveScrolls" Src="CtrlPreserveScrolls.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="CtrMenu.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlHeader.ascx.vb"
    Inherits="RateManager.ctrlHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>

<%@ Import Namespace="RateManager" %>

<script type="text/javascript">
function show(e) 
{
   try{
   var ele=document.getElementById(e);
	if(ele)	ele.style.display="block";
   }
   catch (ex)
   {
   }
}
function hide(e)
{
   try{
   var ele=document.getElementById(e);
   if(ele) ele.style.display="none";
   }
   catch (ex)
   {
   }
}

</script>

<table>
    <tr>
        <td>
        </td>
    </tr>
</table>
<table id="tblBanner" cellspacing="1" cellpadding="1" border="0" width="100%" visible="false">
    <tr>
        <td valign="bottom" align="left">
            <h3>
                <asp:Label ID="lblTitle" CssClass="clsHeaderTitle" runat="server">Sistema de Administración de Hotel</asp:Label></h3>
        </td>
        <td valign="baseline" align="right">
        </td>
        <td valign="bottom">
            <table id="Table3" height="100%" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td valign="top" align="right">
                    </td>
                    <td align="center" width="20%" height="50" colspan="2">
                        <div style='display: <%=iif(RateManager.PortalCulture.GetIDCulture =1,"block","none")%>'>
                            <!-- BEGIN ProvideSupport.com Pass Information Chat Button Code -->
                            <div id="ciaFqk" style="z-index: 100; position: absolute">
                            </div>
                            <div id="scaFqk" style="display: inline">
                            </div>
                            <div id="sdaFqk" style="display: none">
                            </div>

                            <script type="text/javascript">var seaFqk=document.createElement("script");seaFqk.type="text/javascript";var seaFqks=(location.protocol.indexOf("https")==0?"https://secure.providesupport.com/image":"http://image.providesupport.com")+"/js/vhm/safe-standard.js?ps_h=aFqk\u0026ps_t="+new Date().getTime()+"\u0026Sitio=Univisit%20CRS-Rate%20Manager";setTimeout("seaFqk.src=seaFqks;document.getElementById('sdaFqk').appendChild(seaFqk)",1)</script>

                            <noscript>
                                <div style="display: inline">
                                    <a href="http://www.providesupport.com?messenger=vhm">Live Help</a></div>
                            </noscript>
                            <!-- END ProvideSupport.com Pass Information Chat Button Code -->
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="100%" height="20">
                        <img id="imgArrow" src='<%= GeRequestApplicationPath("/Images/this.gif") %>'><asp:HyperLink
                            ID="lnkNameCompany" CssClass="bookingNormalLabel" runat="server">Nombre de hotel</asp:HyperLink>
                    </td>
                    <td align="right" width="20%" height="20">
                        <a href='<%= GeRequestApplicationPath(string.concat("/" , Response.Cookies("groupid").value,  "default.aspx?ididioma=1" ))%> '>
                            <img id="imgspaniol" src='<%=GeRequestApplicationPath("/Images/mexico.gif") %>' border="0"
                                style="margin-right: 4px" /></a>
                    </td>
                    <td width="20%" height="20">
                        <a href='<%= GeRequestApplicationPath(string.concat("/" , Response.Cookies("groupid").value, "default.aspx?ididioma=2"))%> '>
                            <img id="imgenglish" src='<%= GeRequestApplicationPath("/Images/usa.gif")%>' border="0"
                                style="margin-right: 4px" /></a>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                    </td>
                    <td align="center" width="20%" height="20" colspan="2">
                        <div style='display: <%=iif(RateManager.PortalCulture.GetIDCulture =1,"","none")%>'>
                            <a target="frmPrincipal" href="<%response.write(GeRequestApplicationPath( "/Documentacion/Preguntasfrecuentes.htm"))%>">
                                Preguntas frecuentes</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom" colspan="3">
                        <div id="divCompanyInfo" style=" display: none; z-index: 1001; width: 290px; position: absolute"
                            runat="server">
                            <div id="bookingcontainer" style="width: 100%">
                                <div id="divInfo" style= "padding:2px; ">
                                    Hotel la huerta<br />
                                    Ave, revoluciones #623<br />
                                    La Paz, Baja California Sur<br />
                                    Contacto: Emmanuel Adalit Meza Cota<br />
                                    Email: Emmanuel@oz.com.mx<br />
                                    Telefono: 612-12-36699</div>
                            </div>
                        </div>
                        <table id="tblLinks" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                            <tr>
                                <td valign="middle" align="left">
                                    &nbsp;
                                    <asp:Label ID="lblSession" CssClass="bookingNormalLabel" runat="server">Sesión Iniciada</asp:Label>&nbsp;
                                    &nbsp;
                                    <asp:Label ID="lblUser" CssClass="bookingNormalLabel" runat="server">UserName</asp:Label>
                                </td>
                                <td valign="bottom" align="right">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <table id="bookingcontainer" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td width="88%">
                        <cc1:Menu ID="userMenu" Layout="Horizontal" HighlightTopMenu="False" ItemPadding="0"
                            ItemSpacing="1" runat="server" DefaultCssClass="skmMenuItem" SubMenuCssClass="skmSubMenu"
                            DefaultTarget="frmPrincipal" IFrameSrc-Length="10" IFrameSrc="" Width=100%>
                            <SelectedMenuItemStyle CssClass="skmSelMenuItem"></SelectedMenuItemStyle>
                        </cc1:Menu>
                    </td>
                    <td valign="bottom" align="right">
                    </td>
                    <td valign="middle" align="right" style="padding-right:15px;">
                        <asp:LinkButton ID="linkLogOut" CssClass="dgLink" runat="server" CausesValidation="False"> Salir</asp:LinkButton>
                    </td>
                        <%If PortalCulture.GetCulture().ToString = "es-MX" Then%>
                        <td width="80px;" align="right">
                        <a style="text-decoration: none;"  runat="server" id="lnkTicket" class="dglink">
                            <img title='<%=me.getlabel("01314") %>' style="height:24px;" id="imgTicket" border="0" src='<%=GeRequestApplicationPath("/Images/support_request.png")%>'/>
                        </a>
                        <a style="text-decoration: none;"  runat="server" id="lnkTicketList" class="dglink">
                            <img title='<%=me.getlabel("01315") %>' style="height:24px;" id="imgTicketList" border="0" src='<%=GeRequestApplicationPath("/Images/support_list.png")%>'/>
                        </a>
                        <%else%>
                        <td width="27px;" align="right">
                        <%End If%>                        
                        <a style="text-decoration: none;" runat="server" id="lnkhelp" class="dglink">
                            <img style="height:24px;" id="imghelp" border="0" src='<%=GeRequestApplicationPath("/Images/support_help.png")%>'/>
                        </a>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<!--<DIV id="separator" style="WIDTH: 100%; POSITION: relative; HEIGHT: 15px"></DIV>
<uc1:ctrlpreservescrolls id="CtrlPreserveScrolls1" runat="server"></uc1:ctrlpreservescrolls>
-->

<script>
function ClearDiv()
 {
  var e = document.getElementById("divInfo")
  var i =document.getElementById("imgArrow")
  if (e) 
  {
   e.innerHTML='';
   e.style.display='none';
  }   
  if (i){i.style.display='none';}   
  
 }
 function SetDiv(data)
 {
  var e = document.getElementById("divInfo")  
  if (e) 
  {
   e.innerHTML=data;   
  }   
   
 }

</script>

