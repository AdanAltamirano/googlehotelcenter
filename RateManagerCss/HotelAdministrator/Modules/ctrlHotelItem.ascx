<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlHotelItem.ascx.vb" Inherits="RateManager.ctrlHotelItem" %>
<script type="text/javascript" language="javascript">
    function CtrHotelItem_Clear() {
        $("#<%=txtName.ClientID %>").val("");
        $("#<%=txtDescription.ClientID %>").val("");
        $("#<%=txtName.ClientID %>").val("");
    }
</script>


<table id="Table1" class="Form" cellSpacing="1" cellPadding="1" width="100%" border="0">
    <tbody>
    
		<TR>
			<TD align="right" width="25%" >
			    <asp:label ID="lblName" EnableViewState="False" CssClass="clsLabel" runat="server">Nombre</asp:label>
			</TD>
			<TD >
			    <asp:textbox ID="txtName" runat="server" Columns="100" ></asp:textbox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="*"></asp:RequiredFieldValidator>
			</TD>
		</TR>
		<TR>
		    <TD align="right" width="25%" style="vertical-align:top;" >
			    <asp:label id="lblDescripcion" EnableViewState="False" CssClass="clsLabel" runat="server">Descripcion:</asp:label>
			</TD>
			<TD>
			    <asp:textbox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="2" Columns="100"></asp:textbox>
			    <asp:RequiredFieldValidator ID="rfvDescription" ControlToValidate="txtDescription" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
		    </TD>
		</TR>
		<TR>
		    <TD align="right" width="25%" >
			    <asp:label ID="lblPrice" EnableViewState="False" CssClass="clsLabel" runat="server">Precio:</asp:label>
			</TD>
			<TD>
			    <asp:textbox ID="txtPrice" runat="server" Columns="10" ></asp:textbox>
			    <asp:Label ID="lblCurrency" runat="server"></asp:Label>
			    <asp:RequiredFieldValidator ID="rfvPrice" ControlToValidate="txtPrice" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
		    </TD>
		</TR>
	</tbody>
</table>