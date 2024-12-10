<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../Modulos/CtrlIdiomaRFCk.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DepositRatesPlan.aspx.vb"
    Inherits="RateManager.DepositRatesPlan" ValidateRequest="false" %>

<%@ Register src="../Modulos/ctrlAutoComplete.ascx" tagname="ctrlAutoComplete" tagprefix="uc2" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>DepositRatesPlan</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
    
</head>
<script type="text/javascript">
    function FireShowPrepay(id, div, visible) {
        var e = document.getElementById(id);
        var idiv = document.getElementById(div);
        if (e && idiv) {
           idiv.style.display = visible? "block": "none";
        }
    }
    
</script>
<script type="text/javascript">
	    function ShowNewInfo(value) {
	        if (value == 1) {
	            document.getElementById("dvContent").style.display = "block";
	            document.getElementById("dvContent2").style.display = "block";

	            document.getElementById("<%=btnSave.clientid %>").style.display = "block";
	            document.getElementById("<%=btnNuevo.clientid %>").style.display = "block";
	            var btn = document.getElementById("<%=btnPublish.clientid %>");
	            if (btn) {
	                btn.style.display = "block";
	            }

	        } else {
	            document.getElementById("dvContent").style.display = "none";
	            document.getElementById("dvContent2").style.display = "none";
	            document.getElementById("<%=btnSave.clientid %>").style.display = "none";
	            document.getElementById("<%=btnNuevo.clientid %>").style.display = "none";
	            var btn = document.getElementById("<%=btnPublish.clientid %>");
	            if (btn) {
	                btn.style.display = "none";
	            }
	        }
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

	    SearchStart.AddParam
	    (
		{
		    searchitems: [
			{ Item: 'RatePlans', IDSearch: 'IdRatePlan', nameSearch: 'CodigoTarifa', isdefault: true },
			{ Item: 'RatePlans', IDSearch: 'IdRatePlan', nameSearch: 'name', isdefault: false }
			],
		    colModel: [
			    { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
			    { display: '<%= RateManager.PortalCulture.GetString("00073") %>' },
			],
		    Data: [{ catalogo: 'DepositRatesPlan', idHotel: '<%= MyBase.cInfoActual.Hotel %>',
		        idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>',
		        incluirNetRatesPlan: '<%=  IIf(MyBase.IsSupervisor Or MyBase.IsUsuarioNetRates, 1, 0) %>'
            }],
		        id: 'RatePlan',
		        index: 1
		    }
	     );
	
</script>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    
    <div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false" value="New" style="width:85px; display: none;"/>
        <div class="mDiv"></div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">Depósitos por planes</asp:Label>
        </div>
</div>
    	  <div runat="server" id="divContenedor">
    	  
			<table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2" width="100%">			
							 <tr class="trTitle rounded-corners"  id="dvContent2">
                                <td class="dgitem" align="left">                                    
                                    <asp:Label ID="lblInfoRatePlan" runat="server">Label</asp:Label>
                                </td>
                            </tr>
                             <tr class="trContent rounded-corners"  id="dvContent">
                                <td class="tdContent">
                                 <table id="Table2" cellspacing="0" cellpadding="0" width="90%" align="center" border="0" class="Form">
                                                     
                                    <tr>
                                        <td style="width:25%; text-align:right; vertical-align:top;">
                                            <asp:Label ID="lblDeposittitle" runat="server">Si acepta depósito defina el tipo</asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdbNone" runat="server" Text="Ninguno" Checked="True" GroupName="deposittype">
                                            </asp:RadioButton>&nbsp;&nbsp;<asp:RadioButton ID="RdbOneNigth" runat="server" GroupName="deposittype"
                                                Text="Una noche"></asp:RadioButton>
                                            &nbsp;&nbsp;<asp:RadioButton ID="rdbAlltotal" runat="server" Text="Total de la reservación"
                                                GroupName="deposittype"></asp:RadioButton><br />
                                                <asp:RadioButton ID="rbPrepago" runat="server" GroupName="deposittype" 
                                                    Text="Porcentaje de Prepago" />
                                                <br />
                                                <div id="divPrepago" runat="server">
                                                <asp:TextBox ID="txtPrepago" runat="server" CssClass="TextBox" MaxLength="5" 
                                                    Width="48px"></asp:TextBox>%                                   
                                                 <asp:RangeValidator ID="rvMargin" runat="server" ErrorMessage="*" MaximumValue="100"
                                                                                CssClass="clsValidators" MinimumValue="0" ControlToValidate="txtPrepago" Display="Dynamic"
                                                                                Enabled="true" Type="Integer" Visible =false ></asp:RangeValidator>
                                                                                 <asp:RangeValidator ID="rvPorcentaje" runat="server" ControlToValidate ="txtPrepago"
                                                        ErrorMessage="RangeValidator" MaximumValue="100" MinimumValue="0" Type="Double" Display=Dynamic CssClass="clsValidators"></asp:RangeValidator>
                                            </div>
                &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblTarget" runat="server">El depositó se hará a:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdbUV" runat="server" Text="ZT" GroupName="DepositSource"></asp:RadioButton><asp:RadioButton
                                                ID="RdbHotel" runat="server" Text="Hotel" GroupName="DepositSource"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDescripcion" runat="server" EnableViewState="False" CssClass="clsLabel">Descripción</asp:Label>
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2" height="220" valign="top">
                                            <uc1:CtrlIdioma ID="txtDescripcion" runat="server"></uc1:CtrlIdioma>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>		
                                    <% If Me.HasData andalso not Me.txtDescripcion.Published Then%>
                                    <tr>
                                        <td colspan="2">
                                            <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
                                        </td>
                                    </tr>
                                    <% end if %>
                                    <tr>
                                        <td align="center" colspan="2">
                                           	
                                            						    
                                            <%  If Me.IsSupervisor Then
                                                    Me.btnPublish.Text = RateManager.PortalCulture.GetString("01364")%>
							                <asp:Button id="btnPublish" runat="server" Text="Publicar" CssClass="button" EnableViewState="False" ></asp:Button>
							                <% End If%>							                
							                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Guardar"></asp:Button>
							                 <asp:Button ID="btnNuevo" runat="server" CssClass="button" Text="Nuevo" CausesValidation="False">
                                            </asp:Button>	
                                        </td>
                                    </tr>
                                  
                                </table>
                                    
                                </td>
                              </tr>
			</table>
			</div>
       <div class="clear">         
         <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
         <asp:DataGrid ID="dgdeposit" GridLines="None" runat="server" Width="99%" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" CssClass="datagrid" ShowFooter="True">
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="codigotarifa" HeaderText="C&#243;digo">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                                        <HeaderStyle Width="50%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idAgenciasRate"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="rategds"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="rateportal"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="rateUnip"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="rateAds"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="CHANEL">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkGDS" CommandName="EditGDS" runat="server" CausesValidation="False"> GDS </asp:LinkButton>
                                            <asp:LinkButton ID="lnkPOR" CommandName="EditPOR" runat="server" CausesValidation="False"> POR </asp:LinkButton>
                                            <asp:LinkButton ID="lnkUNI" CommandName="EditUNI" runat="server" CausesValidation="False"> UNI </asp:LinkButton>
                                            <asp:LinkButton ID="lnkADS" CommandName="EditADS" runat="server" CausesValidation="False"> ADS </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Bottom"  CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
       </div>
    
    </form>
</body>
</html>
