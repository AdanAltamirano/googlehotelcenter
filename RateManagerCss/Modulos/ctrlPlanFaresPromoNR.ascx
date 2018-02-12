<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlPlanFaresPromoNR.ascx.vb" Inherits="RateManager.ctrlPlanFaresPromoNR" %>
<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

<script language="javascript">
<!--
  function HighlightRow(chkB){
    var oItem = chkB;
    xState=oItem.checked;    
    if(xState){
		chkB.parentElement.parentElement.className="DataGridSelectedItem"; //.style.backgroundColor='#CE5D5A';
        //chkB.parentElement.parentElement.style.color='white'; 
    }
    else{
        var itemType = chkB.parentElement.parentElement.getAttribute("itemType")//.className="";//style.backgroundColor='#F7F7DE';         
        if(itemType=="AlternatingItem")
			chkB.parentElement.parentElement.className="DataGridAlternatedItem";
        else
			chkB.parentElement.parentElement.className="DataGrid";
        
        //chkB.parentElement.parentElement.style.color='black'; 
    }    
  }
  
  function hP(ele)
  {
	var p = document.getElementById(ele);
	if(p) p.style.display='none';
  }
  
  function sP(ele,ele1,ele2)
  {
	try{
		var p = document.getElementById(ele);
		var p1= document.getElementById(ele1);
		var p2= document.getElementById(ele2);
		if (p && p2){
		    if(p.value=='') p.value=0;
		    if(p1){
		    	if(p1.value=='') p1.value=0;
		    	p2.innerHTML= '$' +  (formatAsMoney(eval(p.value) + eval(p1.value)));	
		    }else{
				p2.innerHTML= '$' +  formatAsMoney(eval(p.value))
		    }				
			p2.style.display='block';
		}
	}catch(e){
		p2.style.display='none';
	}
  }
  

  function formatAsMoney(mnt) {
    mnt -= 0;
    mnt = (Math.round(mnt*100))/100;
    return (mnt == Math.floor(mnt)) ? mnt + '.00' 
              : ( (mnt*10 == Math.floor(mnt*10)) ? 
                       mnt + '0' : mnt);
  }
 


//-->
</script>
<asp:customvalidator id="cvErrMsg" runat="server" CssClass="Validators" Display="Dynamic">El valor de la tarifa para adulto y niño no puede ser cero  para esta combinación la habitación seria gratiuta</asp:customvalidator>
<input type="hidden" id="varMinPercent" runat="server" />
<input type="hidden" id="varMaxPercent" runat="server" />
<TABLE id="Table1" cellSpacing="2" cellPadding="0" width="100%" border="0">
	<TR>
		<TD></TD>
		<TD></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD vAlign="top" align="center" ><asp:datagrid id="dgAdult" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="100%">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Adultos">
						<ItemTemplate>
							<asp:Label id=lblAdults runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adults") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa NR">
						<HeaderTemplate>
							<asp:Label id="Label1" runat="server">Tarifa NR</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id="txtAdultFareNR" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.priceNR") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							
							<input type="hidden" class="rate" id="varRate" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.price") %>' />
							<asp:CustomValidator id="cvErrAdultsNR" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFareNR">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valAdultExtraPriceNR" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtAdultFareNR" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa UV">
						<HeaderTemplate>
							<asp:Label id="lblHeader2" runat="server">Tarifa UV</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtAdultFare CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.price") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<%--<asp:CompareValidator ID="cmpvAdults" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtAdultFare"  ControlToCompare="txtAdultFareNR"  Type="Double" Operator="GreaterThanEqual" Display=Dynamic style= "width:120px;" ><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>--%>
							<asp:CustomValidator id="cvErrAdults" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valAdultExtraPrice" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare"
								ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="idRestriccion" HeaderText="idRestriccion"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Tarifa Activa">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkActive" onclick="javascript:HighlightRow(this);" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="True" HeaderText="">
						<HeaderTemplate>
						</HeaderTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblAdultValMax" runat="server" CssClass="Validators">Msg</asp:Label>
							<asp:Label id="lblAdultValMin" runat="server" CssClass="Validators">Msg</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid></TD>
		<TD vAlign="top" align="center" style="padding-left:6px;" >
		<asp:datagrid id="dgChild" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="100%">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Ni&#241;os">
						<HeaderTemplate>
							<asp:Label id="lblHeader3" runat="server">Niños</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label id=lblChildren runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Children") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa NR">
						<HeaderTemplate>
							<asp:Label id="Label2" runat="server">Tarifa NR</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id="txtChildrenFareNR" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.PriceNR") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<input type="hidden" class="rate" id="varRate" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.price") %>' />
							<asp:CustomValidator id="cvErrChildsNR" Display="Dynamic" runat="server" ControlToValidate="txtChildrenFareNR">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valChildrenExtraPriceNR" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtChildrenFareNR" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa UV">
						<HeaderTemplate>
							<asp:Label id="lblHeader4" runat="server">Tarifa UV</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtChildrenFare CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.Price") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<%--<asp:CompareValidator ID="cmpvChilds" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtChildrenFare"  ControlToCompare="txtChildrenFareNR"  Type="Double" Operator="GreaterThanEqual" Display=Dynamic style= "width:120px;" ><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>--%>
							<asp:CustomValidator id="cvErrChilds" Display="Dynamic" runat="server" ControlToValidate="txtChildrenFare">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valChildrenExtraPrice" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtChildrenFare" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="idRestriccion" HeaderText="idRestriccion"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Tarifa Activa">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="Checkbox1" onclick="javascript:HighlightRow(this);" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="True" HeaderText="">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblChildValMax" runat="server" CssClass="Validators">Msg</asp:Label>
							<asp:Label id="lblChildValMin" runat="server" CssClass="Validators">Msg</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid></TD>
			<td  vAlign="top" align="center" style="padding-left:6px;" >
			<asp:datagrid id="dgTeen" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="100%">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Adolecentes">
						<HeaderTemplate>
							<asp:Label id="lblHeader3" runat="server">Adolecentes</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label id=lblChildren runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Teen") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa NR">
						<HeaderTemplate>
							<asp:Label id="Label2" runat="server">Tarifa NR</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id="txtTeenFareNR" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.PriceNR") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<input type="hidden" class="rate" id="varRate" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.price") %>' />
							<asp:CustomValidator id="cvErrTeensNR" Display="Dynamic" runat="server" ControlToValidate="txtTeenFareNR">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valChildrenExtraPriceNR" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtTeenFareNR" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa UV">
						<HeaderTemplate>
							<asp:Label id="lblHeader4" runat="server">Tarifa UV</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtTeenFare CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.Price") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<%--<asp:CompareValidator ID="cmpvJuniors" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtTeenFare"  ControlToCompare="txtTeenFareNR"  Type="Double" Operator="GreaterThanEqual"  Display=Dynamic  style= "width:120px;"><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>--%>
							<asp:CustomValidator id="cvErrTeens" Display="Dynamic" runat="server" ControlToValidate="txtTeenFare">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valTeenExtraPrice" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtTeenFare" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="idRestriccion" HeaderText="idRestriccion"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Tarifa Activa">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="Checkbox1" onclick="javascript:HighlightRow(this);" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="True" HeaderText="">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblChildValMax" runat="server" CssClass="Validators">Msg</asp:Label>
							<asp:Label id="lblChildValMin" runat="server" CssClass="Validators">Msg</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid>
			</td>
	</TR>
</TABLE>
