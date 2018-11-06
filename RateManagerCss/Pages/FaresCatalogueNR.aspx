<%@ Import Namespace="RateManager" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FaresCatalogueNR.aspx.vb"
    Inherits="RateManager.FaresCatalogueNR" EnableEventValidation="true" %>

<%@ Register TagPrefix="uc1" TagName="ctrRateAplication" Src="../Modulos/ctrRateAplicationNR.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExc" Src="../Modulos/CtrlPlanFaresExcNR.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlPlanFares" Src="../Modulos/ctrlPlanFaresNR.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>

--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>FaresCatalogue</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />

    <script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>

    <style type="text/css">
        input.currency
        {
            text-align: right;
            width: 60px;
        }
    </style>

    <script language="javascript" type="text/javascript">
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
                        p2.innerHTML = '' + (formatAsMoney(eval(p.value) + eval(p1.value)));
                    } else {
                        p2.innerHTML = '' + formatAsMoney(eval(p.value))
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
    
        function CheckValContract(min, max, nr, uv, msgMax, msgMin) {
            <% if me.issupervisor then %>
            valMin = parseFloat(document.getElementById(min).value);
            valMax = parseFloat(document.getElementById(max).value);
            valNR = parseFloat(document.getElementById(nr).value);

            if (document.getElementById(uv)) {
                //valUV = parseFloat(document.getElementById(uv).value);
                var NR = parseFloat(valNR) * (1 + (parseFloat(valMin)/100));
                document.getElementById(uv).value = Math.ceil((parseFloat(valNR) / ((100 - valMin) / 100)).toFixed(2)); //Math.ceil(parseFloat(NR).toFixed(2) * 100) / 100;
            }
            valUV = parseFloat(document.getElementById(uv).value);
            /*if ((isNaN(valMin) == false) && (isNaN(valMax) == false)) {
                if ((isNaN(valMin) == false) && (isNaN(valMax) == false)) {
                    if ((valNR != 0) & (valUV != 0)) {
                        if ((((valNR * valMax) / 100) + valNR) < valUV) {
                            document.getElementById(msgMax).style.display = "block";
                            document.getElementById(msgMin).style.display = "none";
                        }
                        else {
                            if ((((valNR * valMin) / 100) + valNR) > valUV) {
                                document.getElementById(msgMax).style.display = "none";
                                document.getElementById(msgMin).style.display = "block";
                            }
                            else {
                                document.getElementById(msgMax).style.display = "none";
                                document.getElementById(msgMin).style.display = "none";
                            }
                        }
                    }
                }
            }*/           
            if (valNR > valUV) {
                document.getElementById(msgMax).style.display = "none";
                document.getElementById(msgMin).style.display = "none";            
            }
            <% else %>
                
            <% end if %>
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
           /* var e = document.getElementById('DivRates');
            var s = document.getElementById('hplShowRates');
            var o = document.getElementById('hplHideRates');
            if (v == '1') {
                e.style.display = 'block';
                o.style.display = 'block';
                s.style.display = "none";
            }
            else {
                e.style.display = "none";
                s.style.display = 'block';00.
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

        function FireSave(ident) {
            e = document.getElementById(ident);
            if (e) {
                e.selectedIndex = 0;
            }
        }	
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Rooms setup"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="780" border="0">
        <tr>
            <td>
                <table id="Table2" cellspacing="0" cellpadding="0" width="100%" align="center" border="0" >
                    <tr>
                        <td>
                            <asp:Panel ID="pnlData" runat="server" style="padding-bottom: 60px;">
                                <table cellspacing="1" cellpadding="1" width="100%" border="0" >
                                    <tr class="trTitle rounded-corners">
                                        <td class="dgitem" align="center" >
                                            <asp:Label ID="lblMsg" runat="server" EnableViewState="False">Rooms setup</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr >
                                                    <td class="dgitem" align="center">
                                                        <asp:Label ID="lblFaresTitle" runat="server" EnableViewState="False">Tarifas especificas  para plan</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <uc1:ctrRateAplication ID="CtrRateAplication1" runat="server"></uc1:ctrRateAplication>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:HyperLink ID="hplShowRates" runat="server" CssClass="dglink">ShowRates</asp:HyperLink><asp:HyperLink
                                                            ID="hplHideRates" runat="server" CssClass="dglink">HideRates</asp:HyperLink>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <div id="DivRates" style="display: none" runat="server">
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblPreciosTarifa" runat="server" EnableViewState="False" CssClass="clsLabel">Tarifas 
                        <BR>  Defina el precio de tarifa en ocupacion por adulto y el precio para niños, el campo total de tarifas mostrara el precio calculado por noche en habitación.</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="" align="center">
                                                                        <asp:Label ID="lblPlusTax" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <table cellspacing="1" cellpadding="1" width="100%" border="1">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <table id="tblTAB2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                        <tr>
                                                                                            <td class="tabselected" id="TdPricing" onmouseover="javascript:this.style.cursor='pointer';"
                                                                                                onclick="javascript:optionSw('1P');" align="center" width="50%" runat="server">
                                                                                                <asp:Label ID="lblPricingNE" runat="server" EnableViewState="False" Font-Size="XX-Small">[Pricing]</asp:Label>
                                                                                            </td>
                                                                                            <td class="tab" id="TdPricingE" onmouseover="javascript:this.style.cursor='pointer';"
                                                                                                onclick="javascript:optionSw('1E');" align="center" width="45%" runat="server">
                                                                                                <asp:Label ID="lblPricingExc" runat="server" EnableViewState="False" Font-Size="XX-Small">[Pricing Exception]</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <div id="divA2" style="min-height: 100px; width: 100%" align="center" runat="server"
                                                                                        ms_positioning="FlowLayout">
                                                                                        <uc1:ctrlPlanFares ID="CtrlPlanFares2" runat="server"></uc1:ctrlPlanFares>
                                                                                    </div>
                                                                                    <div id="divB2" style="min-height: 100px; width: 100%; display: none" align="center"
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
                                                        <%
                                                            'If Not Me.IsSupervisor Then
                                                            'Me.btnSave.OnClientClick = "return (" + Me.CtrRateAplication1.ID + "_ValidateRates() && " + Me.CtrlPlanFares2.ClientID + "_ValidateRates() && " + Me.CtrlPlanFaresExc2.ClientID + "_ValidateRates());"
                                                            'End If
                                                        %>
                                                        <%  If Me.IsSupervisor Then
                                                                Me.btnPublish.Text = RateManager.PortalCulture.GetString("01364")
                                                        %>
                                                        <asp:Button ID="btnPublish" runat="server" EnableViewState="False" CssClass="Button"
                                                            Text="Publicar"></asp:Button>
                                                        <% End If%>
                                                        <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                                                            Text="Guardar" Width="85px"></asp:Button>
                                                        <asp:Button ID="btnNew" runat="server" EnableViewState="False" CssClass="Button"
                                                            Text="Cancelar" CausesValidation="False" Width="85px"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:TextBox ID="txtDivP" Style="display: none" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <hr size="1" width="100%;">
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
        <table id="tblFiltro" cellspacing="0" cellpadding="0" width="100%" align="center"
            border="0" >
            <tr style="padding-bottom: 8px;">
                <td align="center">
                    <asp:Label ID="lblEName" runat="server" EnableViewState="False" CssClass="clslabel">Name:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRooms" runat="server">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <asp:Label ID="lblERatesPlans" runat="server" EnableViewState="False" CssClass="clslabel">Rate Plan:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlratesplans" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnLoad" runat="server" CssClass="button" Text="Load" CausesValidation="False">
                    </asp:Button><asp:CheckBox ID="chkOldDates" runat="server" Text="Incluye Fechas Pasadas"
                        CssClass="clsLabel"></asp:CheckBox>
                </td>
            </tr>
        </table>
        <asp:DataGrid ID="dgRooms" GridLines="None" runat="server" CssClass="datagrid" Width="99%"
            AutoGenerateColumns="False" AllowPaging="True" ShowFooter="True" PageSize="20">
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <Columns>
                <asp:BoundColumn HeaderText="Habitación" DataField="Codigohabitacion"></asp:BoundColumn>
                <asp:BoundColumn DataField="idtipohabitacion_hotel" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Inicio" DataField="FechaInicia"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Fin" DataField="FechaFinaliza"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Plan tarifario">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="glblRatePlanName"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Tarifa Minima" DataField="MinPrice">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Tarifa Máxima" DataField="MaxPrice">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CodigoMoneda"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Tarifa disponible en">
                    <ItemTemplate>
                        <asp:Label ID="lblchanel" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Plan" DataField="NombrePlan" Visible="False"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="dgLink" CausesValidation="False"
                            CommandName="Edit"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete2" Style="display: none" runat="server" CssClass="dgLink"
                            CausesValidation="False" CommandName="Delete"></asp:LinkButton>
                        <asp:HyperLink ID="lnkDelete" runat="server" CssClass="dglink">Delete</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="idTarifa" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="rategds" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="rateportal" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="rateUnip" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="rateADS" Visible="False"></asp:BoundColumn>
            </Columns>
            <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages" Position="Bottom"
                PrevPageText="<< Anterior" NextPageText="Siguiente >>"></PagerStyle>
        </asp:DataGrid>
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
                            if ((txt[t].id.length - txt[t].id.indexOf(typeFare)) == typeFare.length) {
                                txt[t].value = P.value;
                            }
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
