<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlHotelItem.ascx.vb" Inherits="RateManager.ctrlHotelItem" %>
<%@ Register TagPrefix="uc1" TagName="ctrlImagesHotelItem" Src="../../Portal/Modules/Contenido/ctrlImagesHotelItem.ascx" %>
<script type="text/javascript" language="javascript">
    function CtrHotelItem_Clear() {
        $("#<%=txtName.ClientID %>").val("");
        $("#<%=txtDescription.ClientID %>").val("");
		$("#<%=txtName.ClientID %>").val("");
		$("#<%=txtPrice.ClientID %>").val("");
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
		<TR>
		    <TD align="right" width="25%" >
			    <asp:label ID="lblActive" EnableViewState="False" CssClass="clsLabel" runat="server">Activo:</asp:label>
			</TD>
			<TD>
			    <asp:CheckBox ID="chkActive" runat="server"></asp:CheckBox>
		    </TD>
		</TR>
		<tr>
			<uc1:ctrlImagesHotelItem id="ctrlImgHotelItem1" runat="server"></uc1:ctrlImagesHotelItem>
		</tr>
	</tbody>
</table>