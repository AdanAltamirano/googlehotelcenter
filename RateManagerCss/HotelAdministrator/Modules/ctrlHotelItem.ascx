<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlHotelItem.ascx.vb" Inherits="RateManager.ctrlHotelItem" %>
<%@ Register TagPrefix="uc1" TagName="ctrlImagesHotelItem" Src="../../Portal/Modules/Contenido/ctrlImagesHotelItem.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../../Modulos/CtrlIdioma.ascx" %>
<script type="text/javascript" language="javascript">
    function CtrHotelItem_Clear() {
		$("#<%=txtPrice.ClientID %>").val("");
        $("#<%=txtTax.ClientID %>").val("");
    }
</script>


<table id="Table1" class="Form" cellSpacing="1" cellPadding="1" width="100%" border="0">
    <tbody>
    
		<TR>
			<TD align="right" width="25%" >
			    <asp:label ID="lblName" EnableViewState="False" CssClass="clsLabel" runat="server">Nombre</asp:label>
			</TD>
			<TD >
				<uc1:CtrlIdioma ID="ctrlNameIdioma" runat="server"></uc1:CtrlIdioma>
			</TD>
			<TD>
                
			</TD>
			<TD>
				<asp:Label ID="lblActive" EnableViewState="false" CssClass="clsLabel" runat="server">Activo:</asp:Label>
				<asp:CheckBox ID="chkActive"  runat="server"></asp:CheckBox>
			</TD>
		</TR>
		<TR>
		    <TD align="right" width="25%" style="vertical-align:top;" >
			    <asp:label id="lblDescripcion" EnableViewState="False" CssClass="clsLabel" runat="server">Descripcion:</asp:label>
			</TD>
			<TD>
				<uc1:CtrlIdioma ID="ctrlDescriptionIdioma" runat="server"></uc1:CtrlIdioma>
		    </TD>
			<TD>
				
			</TD>
			<TD>
				<asp:Label ID="lblPaymentDestination" EnableViewState="false" CssClass="clsLabel" runat="server">Pago en destino:</asp:Label>
				<asp:CheckBox ID="chkPaymentDestination" runat="server"></asp:CheckBox>
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

				<asp:Label ID="lblTax" EnableViewState="false" CssClass="clsLabel" runat="server">IVA%</asp:Label>
				<asp:TextBox ID="txtTax" runat="server" MaxLength="5"></asp:TextBox>
				<input ID="txtTaxSrc" type=hidden runat=server />
				<asp:RequiredFieldValidator
                                ID="RequiredFieldValidator8" runat="server" CssClass="Validators" ControlToValidate="txtTax"
                                ErrorMessage="Impuesto es requerido" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RangeValidator
                                    ID="RangeValidator9" runat="server" CssClass="Validators" ControlToValidate="txtTax"
                                    ErrorMessage="Impuesto es numerico (1-99)" Display="Dynamic" Type="Double" MaximumValue="99"
                                    MinimumValue="0">*</asp:RangeValidator>
		    </TD>
			<TD >
				
			</TD>
			<TD>
				<asp:Label ID="lblComisionable" EnableViewState="false" CssClass="clsLabel" runat="server">Comisionable:</asp:Label>
				<asp:CheckBox ID="chkComisionable" runat="server"></asp:CheckBox>
			</TD>
			<TD>
				
			</TD>
			<TD>
				
			</TD>
		</TR>
		<TR>		   
		</TR>
		<tr>
			<uc1:ctrlImagesHotelItem id="ctrlImgHotelItem1" runat="server"></uc1:ctrlImagesHotelItem>
		</tr>
	</tbody>
</table>