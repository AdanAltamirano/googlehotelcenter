<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TicketRegister.aspx.vb" Inherits="RateManager.TicketRegister" %>

<html>
<head id="Head1" runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
      <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass=tituloSeccion ><%=Me.GetLabel("01284")%></asp:label> 
		        </div>
		    </div>  
        <table id="bookingcontainer" cellspacing="0" cellpadding="0" width="650" border="0" align="center">
          
            <tr id="pnlForm" runat="server">
                <td>
                    <table width="100%" cellspacing="0" cellpadding="5" border="0" align="center">
                        <tr>
                            <td align="right" class="clsLabel">*<%=Me.GetLabel("00073")%>&nbsp;:&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" MaxLength="200" Width="250px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valName" runat="server"  ControlToValidate="txtName" CssClass="Validators" Display="Dynamic"><%=Me.GetLabel("00071")%></asp:RequiredFieldValidator>   
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="clsLabel">*<%=Me.GetLabel("00163")%>&nbsp;:&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="200" Width="250px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valEmail" runat="server"  ControlToValidate="txtEmail" CssClass="Validators" Display="Dynamic"><%=Me.GetLabel("00071")%></asp:RequiredFieldValidator>   
                                <asp:RegularExpressionValidator ID="expressionEmail" runat="server" ControlToValidate="txtEmail" display="Dynamic" ValidationExpression="^(([^&lt;;&gt;;()[\]\\.,;:\s@\&quot;&quot;]+(\.[^&lt;;&gt;;()[\]\\.,;:\s@\&quot;&quot;]+)*)|(\&quot;&quot;.+\&quot;&quot;))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"><%=Me.GetLabel("01156")%></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="clsLabel">*<%=Me.GetLabel("01286")%>&nbsp;:&nbsp;</td>
                            <td>
                                <asp:DropDownList Width="450px" ID="lstSubject" runat="server">
                                </asp:DropDownList>                               
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="clsTextBoxAsunto" MaxLength="250" Width="450px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RfvAsunto" runat="server"  ControlToValidate="txtSubject" CssClass="Validators" Display="Dynamic"><%=Me.GetLabel("00071")%></asp:RequiredFieldValidator>                
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right" class="clsLabel">*<%=Me.GetLabel("00002")%>&nbsp;:&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" Height="170px" TextMode="MultiLine" CssClass="clsTextBoxDescripcion" MaxLength="2" Width="450px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RfvDescripcion" runat="server" ControlToValidate="txtDescription" CssClass="Validators" Display="Dynamic"><%=Me.GetLabel("00071")%></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="clsLabel"><%=Me.GetLabel("01287")%>&nbsp;:&nbsp;</td>
                            <td>                           
                                
                                <asp:FileUpload Width="450px" ID="txtFile" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="height:52px;" valign="middle">
                                <% Me.btnEnviar.Text = Me.GetLabel("01288")%>
                                <asp:Button ID="btnEnviar" runat="server" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="pnlMessage" runat="server">
                <td align="center">
                    <br />
                    <h2><%=Me.GetLabel("01290")%></h2>
                    <br />                    
                    <p><%=Me.GetLabel("01291")%>&nbsp;<b id="lblReferece" runat="server">0000000000</b>.</p>       
                    <br />
                    <br />
                </td>
            </tr>
            
            
            
        </table>
    </form>
</body>
</html>

