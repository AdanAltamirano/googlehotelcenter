<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrPaymentMethod.ascx.vb" Inherits="RateManager.CtrPaymentMethod" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="~/Modulos/CtrlIdioma.ascx" %>

<script type="text/javascript" language="javascript">
    function CtrPaymentMethod_Clear() {
        $("#<%=ddlMethod.ClientID %>").val(1);
        $("#<%=txtOther.ClientID %>").val("");
        $("#<%=txtAccountNumber.ClientID %>").val("");
        $("#<%=chkDefault.ClientID %>").attr('checked', 'checked');
    }

    function CtrPaymentMethod_Val() {
        var res = false
        
        if ($("#<%=ddlMethod.ClientID %>").val() == "1" && $("#<%=txtOther.ClientID %>").val()=="") {
            $("#rfvOther").css("display", "inline-block");
            res= false;
        }
        else {
            $("#rfvOther").css("display", "none");
            res=true;
        }

        if ($("#<%=txtAccountNumber.ClientID %>").val().trim()!="" && $("#<%=txtAccountNumber.ClientID %>").val().length<4 ) {
            $("#rfvAccount").css("display", "inline-block");
            res=res && false;
        }
        else {
            $("#rfvAccount").css("display", "none");
            res = res && true;
        }

        return res;
        
    }

</script>

<table id="Table1" class="Form" cellSpacing="1" cellPadding="1" width="100%" border="0">
    <tbody>
    
		<asp:HiddenField ID="hidCompanyPaymentMethodID" runat="server" />
		<TR>
			<TD align="right" width="25%" >
			    <asp:label id="lblMethod" EnableViewState="False" CssClass="clsLabel" runat="server">Method name</asp:label>
			</TD>
			<TD >
			    <asp:DropDownList ID="ddlMethod" runat="server" style="width:300px;" onchange="val_DDL(this)"></asp:DropDownList>
			</TD>
		</tr>
		<tr>
		    <TD align="right" width="25%" >
			    <asp:label id="lblOther" EnableViewState="False" CssClass="clsLabel" runat="server">Otro:</asp:label>
			</TD>
			<TD>
			    <asp:textbox id="txtOther" runat="server" ></asp:textbox>
			    <div id="rfvOther" style="display:none; color:Red;">*</div>
		    </TD>
		</TR>
	    <tr>
		    <TD align="right" width="25%" >
			    <asp:label id="lblAccountNumber" EnableViewState="False" CssClass="clsLabel" runat="server">Account Number:</asp:label>
			</TD>
			<TD>
			    <asp:textbox id="txtAccountNumber" runat="server" ></asp:textbox>
			    <div id="rfvAccount" style="display:none;color:Red;" >*</div>
		    </TD>
		</TR>
        <TR>
            <TD align="right" width="25%" >
			    <asp:label id="lblDefaul" EnableViewState="False" CssClass="clsLabel" runat="server">Default:</asp:label>
			</TD>
		    <TD>
		        <asp:checkbox id="chkDefault" runat="server" Text=""></asp:checkbox>
		    </TD>
		</TR>
	    
	</tbody>
</table>
<script language="javascript" type="text/javascript">
    function val_DDL(ddl) {
        var txtOther = "#<%  =txtOther.ClientID %>"
        var txtAccountr = "#<%  =txtAccountNumber.ClientID %>"
        if ($(ddl).val() == "1")
            $(txtOther).removeAttr("disabled");
        else
            $(txtOther).val("").attr("disabled", "disabled");

        if ($(ddl).val() == "2")
            $(txtAccountr).val("").attr("disabled", "disabled");
        else
            $(txtAccountr).removeAttr("disabled");
    }
</script>