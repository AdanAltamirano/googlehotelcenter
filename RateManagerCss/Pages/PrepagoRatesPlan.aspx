<%@ Import Namespace="RateManager" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrepagoRatesPlan.aspx.vb"
    Inherits="RateManager.PrepagoRatesPlan" %>
<%@ Register src="../Modulos/ctrlAutoComplete.ascx" tagname="ctrlAutoComplete" tagprefix="uc1" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title></title>
    
    <link rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">      
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
        

    <script type="text/javascript">
        function FireChek(id0, id1, id2, id3, txt, val) {
            var e = document.getElementById(id0);
            var ep = document.getElementById(id1);
            var es = document.getElementById(id2);
            var et = document.getElementById(id3);
            var text = document.getElementById(txt);

            //if (e) e.checked = true;
            if (ep) ep.checked = false;
            if (es) es.checked = false;
            if (et) et.checked = false;
            if (e && e.checked && text && val) {
                text.style.display = "block";
            }
            else {
                if (text) text.style.display = "none";
            }
        }

        SearchStart.AddParam
	    (
		{
		    searchitems: [
			{ Item: 'RatePlan', IDSearch: 'idRatePlan', nameSearch: 'CodigoTarifa', isdefault: true },
			{ Item: 'RatePlan', IDSearch: 'idRatePlan', nameSearch: 'Name', isdefault: false }
			],
		    colModel: [
			    { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
			    { display: '<%= RateManager.PortalCulture.GetString("00073") %>' }
			],
			    Data: [{ catalogo: 'RatesPlanPrepay', idHotel: '<%= MyBase.cInfoActual.Hotel %>',
		                 idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>'
            }],
		        id: 'PrepafoRatePlan',
		        index: 1
		    }
	     );
	     
    </script>

</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <%--<form id="Form1" method="post" runat="server" submitdisabledcontrols="True">--%>
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div id="tituloSeccion">
            <img class="bgplus" src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
            <asp:Label ID="lblTitle" class="tituloSeccion" runat="server" EnableViewState="False">Prepago por planes tarifarios</asp:Label>
            
        </div>
         <div>
         <uc1:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />   
         </div>
        <table id="bookingcontainer" border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td>
                    
                    <asp:DataGrid ID="dgratesplan" GridLines="None" runat="server" Width="99%" AllowPaging="true"
                        PageSize="20" AutoGenerateColumns="False" CssClass="DataGrid" ShowFooter="false">
                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="dgItem"></ItemStyle>
                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="IdPrepagoRatesPlan"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="prepagotipo"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="PrepaymentPerc"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="idAgenciasRate"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="idRatePlan"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CodigoTarifa" HeaderText="C&#243;digo">
                                <HeaderStyle Width="8%" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                                <HeaderStyle Width="40%" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Una noche" HeaderStyle-Width="200px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPrepago0" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Total Estancia" HeaderStyle-Width="200px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPrepago1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="50% Estancia" HeaderStyle-Width="200px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPrepago2" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Porcentaje Prepago" HeaderStyle-Width="240px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="left" />
                                <ItemTemplate>
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkPrepago3" runat="server" />
                                            </td>
                                            <td>
                                                <input class="TextBox" id="txtPorcentaje" type="text" runat="server" style="width:46px;
                                                    display: none;" maxlength="3" enableviewstate="false" />
                                                <asp:RangeValidator ID="rvMargin" runat="server" ErrorMessage="*" MaximumValue="100"
                                                    CssClass="clsValidators" MinimumValue="0" ControlToValidate="txtPorcentaje" Display="Dynamic"
                                                    Enabled="true" Type="Integer"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                            Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr class="trContent" id="dvContent">
                <td class="tdContent" aling="top">
                    <table id="Table2" class="Form" cellspacing="0" cellpadding="0" width="90%" align="center"
                        border="0">
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="padding-top: 4px; padding-bottom: 6px; border-top: solid 1px #ccc;">
                                <asp:Label ID="lblAyuda" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btnSave" runat="server" CssClass="Button" Text="Guardar"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
    
    </form>
</body>
</html>
