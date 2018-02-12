<%@ Register TagPrefix="uc1" TagName="ctrlPlanFares" Src="../Modulos/ctrlPlanFares.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExc" Src="../Modulos/CtrlPlanFaresExc.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HousesCatalogue.aspx.vb"
    Inherits="RateManager.HousesCatalogue"  %>

<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrRateAplication" Src="../Modulos/ctrRateAplication.ascx" %>
<%@ Import Namespace="RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>FaresCatalogue</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css"></link>
    <script type="text/javascript" src="../Includes/Script/jquery-3.1.1.min.js"></script>
    <style type="text/css">
        .dgHeader
        {
            background-color: #e3e9ed;
            text-shadow: 0 1px 0 #fff;
        }

        .dgHeader td
        {
            font-family: "Times New Roman" , Times, serif;
            font-size: 13px;
            font-weight: bold;
            color: #666666;
            border: 0;
        }

    </style>
    <script language="javascript">
<!--
    function HighlightRow(chkB) {
        var oItem = chkB;
        xState = oItem.checked;
        if (xState) {
            chkB.parentElement.parentElement.className = "DataGridSelectedItem"; //.style.backgroundColor='#CE5D5A';
            //chkB.parentElement.parentElement.style.color='white'; 
        }
        else {
            var itemType = chkB.parentElement.parentElement.getAttribute("itemType")//.className="";//style.backgroundColor='#F7F7DE';         
            if (itemType == "AlternatingItem")
                chkB.parentElement.parentElement.className = "DataGridAlternatedItem";
            else
                chkB.parentElement.parentElement.className = "DataGrid";

            //chkB.parentElement.parentElement.style.color='black'; 
        }
    }

    function hP(ele) {
        var p = document.getElementById(ele);
        if (p) p.style.display = 'none';
    }

    function sP(ele, ele1, ele2) {
        try {
            var p = document.getElementById(ele);
            var p1 = document.getElementById(ele1);
            var p2 = document.getElementById(ele2);
            if (p && p2) {
                if (p.value == '') p.value = 0;
                if (p1) {
                    if (p1.value == '') p1.value = 0;
                    p2.innerHTML = '$' + (formatAsMoney(eval(p.value) + eval(p1.value)));
                } else {
                    p2.innerHTML = '$' + formatAsMoney(eval(p.value))
                }
                p2.style.display = 'block';
            }
        } catch (e) {
            p2.style.display = 'none';
        }
    }


    function formatAsMoney(mnt) {
        mnt -= 0;
        mnt = (Math.round(mnt * 100)) / 100;
        return (mnt == Math.floor(mnt)) ? mnt + '.00'
          : ((mnt * 10 == Math.floor(mnt * 10)) ?
                   mnt + '0' : mnt);
    }

    function FireShow(ID, IDcmd, show) {
        var e = document.getElementById(ID);
        var c = document.getElementById(IDcmd);
        if (e) {
            e.style.display = show ? 'block' : 'none';
        }
        if (c) {
            c.style.display = !show ? 'block' : 'none';
        }
        onResizeIframe();
    }


    //-->
    </script>

    <script>
        /*function showRatePlan2(ddl,array,lbl)
        { 
        var e = document.getElementById(ddl);
        var lbl = document.getElementById(lbl);		  		  	  
        lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex];
		  		  
        }
		  
	  	 function showRatePlan(ddl,array,lbl)
        { 
        var e = document.getElementById(ddl);
        var lbl = document.getElementById(lbl);		  
        if (e.selectedIndex!=0) 
        {		  
        lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex-1];
        }
        else
        {
        lbl.firstChild.nodeValue ='-';
        }
		  
		  }*/
        function Ocultar(v) {
            /*   var e = document.getElementById('<%=DivRates.clientid%>');
            var s = document.getElementById('<%=hplShowRates.clientid%>');
            var o = document.getElementById('=hplHideRates.clientid%>');
            if (v == '1') {
                e.style.display = 'block';
                o.style.display = 'block';
                s.style.display = "none";
            }
            else {
                e.style.display = "none";
                s.style.display = 'block';
                o.style.display = "none";
            }*/
        }

        function optionSw(e) {
            var div1A;
            var div2B;
            var td1A;
            var td2B;

            var txt;
            txt = document.getElementById('txtDivP');
            txt.value = e;

            div1A = document.getElementById('divA2');
            div2B = document.getElementById('divB2');
            td1A = document.getElementById('TdPricing');
            td1B = document.getElementById('TdPricingE');
            switch (e) {
                case '1P':

                    div1A.style.display = 'block';
                    div2B.style.display = "none";

                    td1A.className = 'tabselected';
                    td1B.className = 'tab';
                    break;
                case '1E':

                    div1A.style.display = "none";
                    div2B.style.display = 'block';


                    td1A.className = 'tab';
                    td1B.className = 'tabselected';
                    break;
            }
            return true;
        }

        function FireSave(ident) {
            e = document.getElementById(ident);
            if (e) {
                e.selectedIndex = 0;
            }
        }



    </script>

</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server" >
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(String.Concat("/Calendar/", PortalCulture.GetCulture().Name.Substring(0, 2).ToLower(), "/ipopeng2.htm"))%>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <div class="clear">
        <input type="button" visible="false" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" visible="false" runat="server" EnableViewState="False" Text="Tarifas" CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
        <div id="modalPage2" style="display: none; left: 0px; width: 100%; position: absolute;
        top: 0px; height: 100%" align="left">
        <div style="filter: Alpha(Opacity=5); width: 100%;  background-color: rgba(0, 0, 0,0.7); position: absolute; height: 134.96%;">
        </div>
        <div class="datagrid" id="modalPage" style="border-right: black 1px solid; padding-right: 0px;
            border-top: black 1px solid; display: none; padding-left: 0px; left: 16%; border-left: black 1px solid;
            border-bottom: black 1px solid; position: absolute; top: 24%; background-color: #f5f5f5"
            align="left" runat="server">
            <table width="100%" border="0">
                <tr>
                    <td align="center" colspan="2" nclass="DataGridAlternatedItem">
                        <p>
                            <uc1:ctrlPlanFares ID="Ctrlplanfares1" runat="server"></uc1:ctrlPlanFares>
                        </p>
                        <input id="currentprices" type="hidden">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; height: 14px;">
                        <asp:Label ID="lblValidPriceRes" runat="server" CssClass="validators" EnableViewState="false">Select room and load data</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50%">
                        <asp:HyperLink ID="lnkCloseShowRates" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/ok_.jpg"
                            Style="padding-right: 12px;">[Cerrar]</asp:HyperLink>
                    </td>
                    <td style="width: 50%;">
                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/Close.ico"
                            Style="padding-left: 12px;">[Cancelar]</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table id="bookingcontainer" border="0" cellspacing="0" cellpadding="2" width="750">
        <tr>
            <td>
                <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlData" runat="server" style="padding-bottom: 60px;">
                                <table border="0" cellspacing="1" cellpadding="1" width="100%">
                                    <tr  class="trTitle rounded-corners">
                                        <td class="dgitem" align="center">
                                            <asp:Label ID="lblMsg" runat="server" EnableViewState="False"> Rooms setup</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="dgitem" align="center">
                                            <asp:Label ID="lblFaresTitle" runat="server" EnableViewState="False" >Tarifas especificas  para plan</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <uc1:ctrRateAplication ID="CtrRateAplication1" runat="server"></uc1:ctrRateAplication>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:HyperLink ID="hplShowRates" runat="server" CssClass="dglink" Style="display: none;">ShowRates</asp:HyperLink><asp:HyperLink
                                                ID="hplHideRates" runat="server" CssClass="dglink" Style="display: none;">HideRates</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <div style="display: none" id="DivRates" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblPreciosTarifa" runat="server" EnableViewState="False" CssClass="clsLabel">Tarifas <br>  Defina el precio de tarifa en ocupacion por adulto y el precio para niños, el campo total de tarifas mostrara el precio calculado por noche en habitación.</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="" align="center">
                                                            <asp:Label ID="lblPlusTax" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table border="1" cellspacing="1" cellpadding="1" width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <table id="tblTAB2" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                            <tr>
                                                                                <td id="TdPricing" class="tabselected" onmouseover="javascript:this.style.cursor='pointer';"
                                                                                    onclick="javascript:optionSw('1P');" width="50%" align="center" runat="server">
                                                                                    <asp:Label ID="lblPricingNE" runat="server" EnableViewState="False" Font-Size="XX-Small">[Pricing]</asp:Label>
                                                                                </td>
                                                                                <td id="TdPricingE" class="tab" onmouseover="javascript:this.style.cursor='pointer';"
                                                                                    onclick="javascript:optionSw('1E');" width="45%" align="center" runat="server">
                                                                                    <asp:Label ID="lblPricingExc" runat="server" EnableViewState="False" Font-Size="XX-Small">[Pricing Exception]</asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div style="min-height: 100px; width: 100%;" id="divA2" align="center" runat="server"
                                                                            ms_positioning="FlowLayout">
                                                                            <uc1:ctrlPlanFares ID="CtrlPlanFares2" runat="server"></uc1:ctrlPlanFares>
                                                                        </div>
                                                                        <div style="min-height: 100px; width: 100%; display: none" id="divB2" align="center"
                                                                            runat="server" ms_positioning="FlowLayout">
                                                                            <uc1:CtrlPlanFaresExc ID="CtrlPlanFaresExc2" runat="server"></uc1:CtrlPlanFaresExc>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Label ID="lblPriceError" runat="server" CssClass="validators">Por Favor Agregue tarifa mayor que 0</asp:Label><asp:Label
                                                ID="lblError" runat="server" CssClass="validators" Visible="False">Agregue fecha de aplicación de la tarifa</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblNoroomSelected" runat="server" CssClass="validators" Visible="False">Select room and load data</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">

                                            <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                                                Text="Guardar" Width="85px"></asp:Button>
                                            <asp:Button ID="btnNew" runat="server" EnableViewState="False" CssClass="Button"
                                                Text="Cancelar" CausesValidation="False" Width="85px"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                                <asp:TextBox Style="display: none" ID="txtDivP" runat="server"></asp:TextBox>
                                <hr size="1" width="100%">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="clear">
        <table id="tblFiltro" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr style="padding-bottom: 8px;">
                <td align="center" style="text-align: right; display:none">
                    <asp:Label ID="lblEName" runat="server" EnableViewState="False" CssClass="clslabel">Name:</asp:Label>
                </td>
                <td style="display:none">
                    <asp:DropDownList ID="ddlRooms" runat="server" >
                    </asp:DropDownList>
                </td>
                <td align="center" >
                    <asp:Label ID="lblERatesPlans" runat="server" EnableViewState="False" CssClass="clslabel">Rate Plan:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlratesplans" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnLoad" runat="server" CssClass="button" Text="Load" CausesValidation="False">
                    </asp:Button><asp:CheckBox ID="chkOldDates" Visible="false" runat="server" Text="Incluye Fechas Pasadas"
                        CssClass="clsLabel"></asp:CheckBox>
                </td>
            </tr>
        </table>
        <!--  Tarifas  -->
        <table id="GeneralRatesTable" cellspacing="0" cellpadding="0" width="100%" align="center" border="0" class="bookingcontainer">
            <tr>
                 <td class="dgItem" align="center" colspan="4">
                     <asp:Label runat="server" ID="Label1">Tarifas</asp:Label>
                 </td>
             </tr>
            <tr style="padding-bottom: 8px;">
                <td>
                    <asp:Label runat="server" ID="lblRate">Tarifa Adulto: </asp:Label>
                    <asp:TextBox runat="server" ID="rateA" Width="60px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblRateC">Tarifa Menor: </asp:Label>
                    <asp:TextBox runat="server" ID="rateC" Width="60px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label3">Plan Tarifario: </asp:Label>
                    <asp:DropDownList runat="server" ID="ddlRatePlans" />
                </td>
                <td>
                    <asp:label id="lblStartDateRate" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:label>
			        <asp:textbox id="txtDateFromRate" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%Response.Write(txtDateToRate.ClientID)%>'),document.getElementById('<%Response.Write(txtDateFromRate.ClientID)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
                    alt="" align=absMiddle 
                    src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
                    ></A>
                </td>
                <td>
			        <asp:label id="lblEndDateRate" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:label>
			        <asp:textbox id="txtDateToRate" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%Response.Write(txtDateToRate.ClientID)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
                    alt="" align=absMiddle 
                    src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
                    > </A>
                </td>
                <td>
                    <input type="button" id="addRate" class="ButtonNew" runat="server" causesvalidation="false"
            value="Agregar" />
                </td>
             </tr>
            </table>
        

        <!--  Cierres  -->
         <table id="GeneralLocksTable" cellspacing="0" cellpadding="0" width="100%" align="center" border="0" class="bookingcontainer">
             <tr>
                 <td class="dgItem" align="center" colspan="4">
                     <asp:Label runat="server" ID="GeneralLocksLabel">Cierres</asp:Label>
                 </td>
             </tr>
            <tr style="padding-bottom: 8px;">
                <td>
        <asp:Label runat="server" ID="Label2">Plan Tarifario: </asp:Label>
        <asp:DropDownList runat="server" ID="DropDownList1" />
        </td>
                <td>

        <asp:label id="lblStartDateLock" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:label>
			<asp:textbox id="txtDateFromLock" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%Response.Write(txtDateToLock.ClientID)%>'),document.getElementById('<%Response.Write(txtDateFromLock.ClientID)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
            alt="" align=absMiddle 
            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
            ></A>
                    </td>
                <td>
			<asp:label id="lblEndDateLock" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:label>
			<asp:textbox id="txtDateToLock" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateToLock.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
            alt="" align=absMiddle 
            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
            > </A>
                    </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                                                        </asp:DropDownList>
                </td>

                <td>
                    <input type="button" id="addLock" class="ButtonNew" runat="server" causesvalidation="false"
            value="Agregar" />
                    

      </td>
        </tr>
    </table>
        <div style="filter: Alpha(Opacity=5); width: 100%; background-color: rgba(0, 0, 0,0.7);position: absolute; height: 134.96%;" id="hideMainModal"></div>
        <asp:DataGrid ID="dgRooms" GridLines="None" runat="server" CssClass="datagrid" Width="99%"
            AutoGenerateColumns="False"  >
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server"  CommandName="check"/>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Nombre" HeaderText="Propiedad"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idhotel"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idTipoHabitacion_Hotel"></asp:BoundColumn>
                <asp:BoundColumn DataField="Rate" HeaderText="Tarifas"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Button ID="lnkRates2" runat="server" CausesValidation="False" CssClass="dgLink"
                            CommandName="ShowRates" AutoPostBack="false"  OnClick="link" EnableViewState="true" Text="Ver más"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Lock" HeaderText="Cierres"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Button ID="lnkLocks2" runat="server" CausesValidation="False" CssClass="dgLink"
                            CommandName="ShowLocks" AutoPostBack="false" OnClick="link"  Text="Ver más" ></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
        
        
        <div class="datagrid" id="modalTable" style="border-right: black 1px solid; padding-right: 0px;
            border-top: black 1px solid; padding-left: 0px; left: 18%; border-left: black 1px solid;
            border-bottom: black 1px solid; position: absolute; top: 16%; background-color: #f5f5f5"
            align="left" runat="server" visible="false">
            <div style="filter: Alpha(Opacity=5); width: 100%; background-color: rgba(0, 0, 0,0.7); position: absolute; height: 134.96%;" id="hideSubModal"></div>
            <asp:DataGrid ID="modalGrid" GridLines="None" runat="server" CssClass="datagrid" Width="99%"
            AutoGenerateColumns="False" >
                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="id" visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="data" HeaderText="Información completa"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" CausesValidation="False" CssClass="dgLink"
                            CommandName="Edit"  OnClick="link" EnableViewState="true" Text="Editar"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="dgLink"
                            CommandName="Delete" OnClick="link"  Text="Borrar" ></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>

                </asp:DataGrid>
            <div  align="right" >
            <asp:ImageButton runat="server" ID="closeModalGrid" CssClass="dglink" ImageUrl ="~/Includes/imagenes/Close.ico"></asp:ImageButton>
            </div>
        </div>
        
        <div class="datagrid" id="EditRates" style="border-right: black 1px solid; padding-right: 0px;
            border-top: black 1px solid; padding-left: 0px; left: 30%; border-left: black 1px solid;
            border-bottom: black 1px solid; position: absolute; top: 17%; background-color: #f5f5f5"
            align="left" runat="server" visible="false">
        
            <table>
                <tr class="dgHeader">
                    <td style="text-align:center">
                       <b> Editar Tarifas</b>
                    </td>
                </tr>
            <tr>
                <td>
                    <asp:TextBox ID="editRateId" runat="server" style="display:none"></asp:TextBox>
                    <asp:TextBox ID="editRateRoom" runat="server" style="display:none"></asp:TextBox>
                    <asp:Label runat="server" ID="Label4">Tarifa Adulto: </asp:Label>
                    <asp:TextBox runat="server" ID="editAdulto" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label5">Tarifa Menor: </asp:Label>
                    <asp:TextBox runat="server" ID="editMenor" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label6">Plan Tarifario: </asp:Label>
                    <asp:DropDownList runat="server" ID="ddlEditRateRP" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label7" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:label>
			        <asp:textbox id="editRateFrom" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%Response.Write(editRateTo.ClientID)%>'),document.getElementById('<%Response.Write(editRateFrom.ClientID)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
                    alt="" align=absMiddle 
                    src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
                    ></A>
                </td>
            </tr>
            <tr>
                <td>
			        <asp:label id="Label8" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:label>
			        <asp:textbox id="editRateTo" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%Response.Write(editRateTo.ClientID)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
                    alt="" align=absMiddle 
                    src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
                    > </A>
                </td>
            </tr>
            </table>
            <div  align="right" >
            <asp:ImageButton runat="server" ID="doEditRates" CssClass="dglink" CommandName="EditRate" OnClick="link" ImageUrl="~/Includes/imagenes/ok_.jpg"></asp:ImageButton>
            <asp:ImageButton runat="server" ID="closeEditRates" CssClass="dglink" ImageUrl="~/Includes/imagenes/Close.ico"></asp:ImageButton>
            </div>
        </div>
        
        <div class="datagrid" id="EditLocks" style="border-right: black 1px solid; padding-right: 0px;
            border-top: black 1px solid; padding-left: 0px; left: 30%; border-left: black 1px solid;
            border-bottom: black 1px solid; position: absolute; top: 17%; background-color: #f5f5f5"
            align="left" runat="server" visible="false">
            <table>
                <tr class="dgHeader">
                    <td style="text-align:center"> 
                        <b>Editar Cierres</b>
                    </td>
                </tr>
            <tr>
                <td>
                    <asp:TextBox ID="EditIdHotel" runat="server" style="display:none"></asp:TextBox>
                    <asp:TextBox ID="EditIdTipoH" runat="server" style="display:none"></asp:TextBox>
                    
                    <asp:Label runat="server" ID="Label9">Plan Tarifario: </asp:Label>
                    <asp:DropDownList runat="server" ID="EditratePlan" />
                </td>
            </tr>
            <tr>
                <td>

        <asp:label id="Label10" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:label>
			<asp:textbox id="editLockFrom" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%Response.Write(editLockTo.ClientID)%>'),document.getElementById('<%Response.Write(editLockFrom.ClientID)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
            alt="" align=absMiddle 
            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
            ></A>
                    </td>
                </tr>
            <tr>
                <td>
			<asp:label id="Label11" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:label>
			<asp:textbox id="editLockTo" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(editLockTo.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger border=0 
            alt="" align=absMiddle 
            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
            > </A>
                    </td>
                </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlEditStatus" runat="server">
                                                        </asp:DropDownList>
                </td>
                </tr>

            </table>
            <div  align="right" >
            <asp:ImageButton runat="server" ID="doEditLocks" CssClass="dglink" OnClick="link" CommandName="EditLock" ImageUrl="~/Includes/imagenes/ok_.jpg"></asp:ImageButton>
            <asp:ImageButton runat="server" ID="closeEditLocks" CssClass="dglink" ImageUrl="~/Includes/imagenes/Close.ico"></asp:ImageButton>
            </div>
        </div>

        

    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>

    <script>
        /* function showRoomType(ddl)
        {
        var Code = document.getElementById(ddl);
        var indice = Code.selectedIndex;		   		   
        var lbl = document.getElementById('lblRoomType');
        lbl.innerText= Code[indice].value;
        }*/
        /*function showRoomType(ddl,array)
        {
        var e = document.getElementById(ddl);
        var lbl = document.getElementById('lblRoomType');		  
        if (e.selectedIndex!=0) 
        {		  
        lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex];
        }
        else
        {
        lbl.firstChild.nodeValue ='-';
        }
		  
		  }*/

        $(document).ready(function(){
            //Evento para deshabilitar los botones de atras
            if ($('#EditLocks').is(':visible') || $('#EditRates').is(':visible')){
                $('#hideMainModal').show()
                $('#hideSubModal').show()
            }else{
                $('#hideMainModal').hide()
                $('#hideSubModal').hide()
            }

            if($('#modalTable').is(":visible")){
                $('#hideMainModal').show()
            }else{
                $('#hideMainModal').hide()
            }
            
        });

            function FireShowSelectRates(ddl, rate, ocupa, validators) {            
                if (ddl){
                    var normalValidate = false;
                    if(ddl.selectedIndex==0){
                        $('#' + rate).css('display','inline');
                        $('#' + ocupa).hide();
                        normalValidate = true;
                    }else{
                        $('#' + rate).css('display','none');
                        $('#' + ocupa).show();                
                    }
                
                    $.each(validators, function(){
                        var items = $('#' + this.id)
                        if (this.hide && items.length > 0)
                            ValidatorEnable(items[0], normalValidate);
                        items.hide();                     
                    })                
                }            
            }

            function IniDate() {
                var fecha = new Date();
                var fecha2 = new Date(2030, 12, 31);
                var arr = new Array(3);
                arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
                arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
                return arr;

            }



            function FillPrices(Dg, typeFare, Price) {
                var P = document.getElementById(Price);
                var grid = document.getElementById(Dg);
                var item = grid.getElementsByTagName("tr");

                for (var i = 1; i < item.length; i++) {
                    var txt = item[i].getElementsByTagName("input");
                    var lbl = item[i].getElementsByTagName("span");

                    for (var j = 0; j < lbl.length; j++) {
                        for (var t = 0; t < txt.length; t++) {
                            if (txt[t].id.indexOf(typeFare) != -1) {
                                txt[t].value = P.value;
                            }
                        }

                        if (lbl[j].id.indexOf("lblTotal") != -1) {
                            if (txt.length > 1) {
                                sP(txt[0].id, txt[1].id, lbl[j].id);
                            }
                            else {
                                sP(txt[0].id, '', lbl[j].id);
                            }
                        }
                    }


                }

            }
				
            var rateModeView = <%= Me.RateModeView %>;
		
    </script>

</body>
</html>
