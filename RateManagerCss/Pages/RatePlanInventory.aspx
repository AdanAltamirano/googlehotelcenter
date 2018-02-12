<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RatePlanInventory.aspx.vb"
    Inherits="RateManager.RatePlanInventory" %>

<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>RatePlanInventory</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>
    
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:TextBox ID="txtCambiaDep" Style="display: none" runat="server" Width="34px"
        CssClass="TextBox"></asp:TextBox><asp:TextBox ID="txtCambiaInd" Style="display: none"
            runat="server" Width="34px" CssClass="TextBox"></asp:TextBox><asp:TextBox ID="txtRatesPlans"
                Style="display: none" runat="server" Width="34px" CssClass="TextBox"></asp:TextBox>

    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="clstitle" runat="server" EnableViewState="False" Text="Porcentaje de Inventario" CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>                
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" border="0" style="width:100%; height:600px">
        <tr>
            <td valign=top >
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">                   
                    <tr>
                        <td colspan="3" height="5">
                        </td>
                    </tr>
                    <tr class="dgitem">
                        <td width="33.3%" align="center">
                            <asp:Label ID="lblRateCode" runat="server" CssClass="clslabel" EnableViewState="False">Rate Code</asp:Label>
                        </td>
                        <td align="center" width="33.3%">
                            <asp:Label ID="lblAvail" runat="server" CssClass="clslabel" EnableViewState="False">Def. # Avail</asp:Label>
                        </td>
                        <td align="center" width="33.3%">
                            <asp:Label ID="lblPerc" runat="server" CssClass="clslabel" EnableViewState="False">% from Source +/-</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:DataList ID="dgIndependent" runat="server" Width="100%" CellSpacing="0" CellPadding="0"
                                BorderWidth="0">
                                <HeaderTemplate>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr class="dgHeader">
                                            <td align="center" width="33.3%">
                                                <asp:DropDownList ID="ddlIndependents" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center" width="33.3%">
                                                <!--
														<asp:HyperLink ID="lnkIndRooms" Runat="server" CssClass="dglink"></asp:HyperLink>
														-->
                                                <asp:TextBox ID="txtIndRooms" CssClass="textbox" runat="server" MaxLength="5" Columns="5"></asp:TextBox>
                                            </td>
                                            <td align="center" width="33.3%">
                                                <%--<asp:RangeValidator ID="rvInventario" CssClass="Validators" runat="server" ControlToValidate="txtIndRooms"  ErrorMessage="RangeValidator" MaximumValue="999" Display=Dynamic ></asp:RangeValidator>--%>
                                                <asp:RangeValidator ID="RValIventory" runat="server" CssClass="validators" ErrorMessage="El valor no debe estar en 0"
                                                    ControlToValidate="txtIndRooms" MinimumValue="0" MaximumValue="1000" Type="Integer"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:DataGrid ID="dgDependent" runat="server" Width="100%" ShowHeader="False" AutoGenerateColumns="False" CssClass=DataGrid >
                                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                        <ItemStyle CssClass="dgItem"></ItemStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="false" DataField="TargetRatePlan"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="RateCodeTarget" ItemStyle-HorizontalAlign="left" ItemStyle-Width="33.3%">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TargetName" ItemStyle-HorizontalAlign="center" Visible="False">
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn ItemStyle-Width="33.3%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    -
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="SoldOutPerc" Visible="False" ItemStyle-Width="0"></asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <ItemStyle HorizontalAlign="center" Width="33.3%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkDepRooms" runat="server" CssClass="dglink"></asp:HyperLink>
                                                    <asp:TextBox ID="txtDepRooms" runat="server" CssClass="textbox" MaxLength="5" Columns="5"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn Visible="False" DataField="SourceRatePlan"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="RateCodeSource"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="texto"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" height="5" align=center >
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <input type=hidden runat=server id= inputMaxInventario  />
                            <asp:Button ID="btnSave" size="20" runat="server" CssClass="button" Text="Save" EnableViewState="False">
                            </asp:Button>
                            <asp:Button ID="btnCancel" runat="server" EnableViewState="False" CssClass="button"
                                Text="Cancel" size="20"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script>

        function showLinks(ddl, lnk, t) {
            var d = document.getElementById(ddl);
            var dg = document.getElementById("dgIndependent");
            var dgs = dg.getElementsByTagName("table");
            for (var i = 1; i < dgs.length; i++) {
                if (i != d.selectedIndex + 1) {
                    dgs[i].style.display = 'none';
                }
                else {
                    dgs[i].style.display = '';
                }
            }
            var txt = document.getElementById('txtRatesPlans');
            var lk = document.getElementById(lnk);
            var s = txt.value.split("/");
            txt = document.getElementById(t);
            if (s[d.selectedIndex + 1] != '') { txt.innerText = s[d.selectedIndex + 1]; } else { txt.innerText = '##'; }


            /*	  var lbl = document.getElementById(label);		  
            if (array.length!='') 
            {		  
            lbl.firstChild.nodeValue =array.split("//")[d.selectedIndex];
            }
            else
            {
            lbl.firstChild.nodeValue ='-';
            }*/
        }


        function ShowTxt(txt, lnk) {
            var t = document.getElementById(txt);
            t.style.display = ''
            var l = document.getElementById(lnk);
            l.style.display = "none"
            if (l.innerHTML != "##") {
                t.value = (l.innerHTML != '0')? l.innerHTML : '';
            }
        }
        
        function SaveDepAva(target, source, txt, targetcode, sourcecode) {

            var e = document.getElementById(txt);
            if (isnumber(e.value)) {
                //source el ddlrateplan selected		   

                var e1 = document.getElementById('txtCambiaDep');
                e1.value = e1.value + target + "/" + source + "/" + e.value + "/" + targetcode + "/" + sourcecode + ",";
            }
            else {
                e.value = 0;
            }
        }


        function SaveIndAva(rp, txt) {

            var e = document.getElementById(txt);
            var ddl = document.getElementById(rp);
            if (isnumber(e.value)) {
                var e1 = document.getElementById('txtCambiaInd');
                e1.value = e1.value + ddl.item(ddl.selectedIndex).value + "/" + e.value + "/" + ddl.item(ddl.selectedIndex).text + ",";
            }
            else {
                e.value = 0;
            }
        }

        function isnumber(valor) {

            var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;
            for (i = 0; i < valor.length && IsNumber == true; i++) {
                Char = valor.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;
        }
	
	
    </script>

</body>
</html>
