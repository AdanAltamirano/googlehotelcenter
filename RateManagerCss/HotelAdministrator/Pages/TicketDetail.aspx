<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TicketDetail.aspx.vb" Inherits="RateManager.TicketDetail" %>
<%@ Import Namespace="Portal.Hotel.Common.Data" %>
<%@ Import Namespace="RateManager" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title>Tickets</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <style>
        .clsGridView
        {
	        font-weight: bold;	
	        font-size: 10px; 
	        color: SteelBlue;	
	        font-family: Verdana;	
	        background-color: White; 
	        border:1px;
        }
        
        .clsGridViewItem
        {
	        font-weight: bold;
	        font-size: 10px;
	        color: #000000;
	        font-family: Verdana, Arial;	
	        background-color: #fFfFfF;
	        border:0px;
        }
        
        .clsGridViewAlternatedItem
        {	
	        border-right: 0px;	
	        border-top: 0px;	
	        font-weight: bold;	
	        font-size: 10px;	
	        border-left: 0px;	
	        color: #000000;	
	        border-bottom: 0px;	
	        font-family: Verdana, Arial;	
	        background-color: #f5f5f5;
        }
    
        div.DescriptionItem {
        padding:5px;
        }
        div.DescriptionItem div {
        font-weight:normal;
        }
        div.DescriptionItem .Comment {
        margin:5px 15px;
        }
        div.DescriptionItem .Date {
        font-style:italic;
        }
        div.DescriptionItem .Owner {
        font-weight:bold;
        }
    </style>
</head>
<body>

    <form id="Form1" runat="server">
        <% If Me.IsValidRequest Then%>
      <div class="clear">		    
	        <div class="mDiv"></div>
	        <div>
	            <asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass=tituloSeccion ><%=Me.GetLabel("01297")%></asp:label> 
	        </div>
	    </div> 
        <table  id="bookingcontainer" Width="750" style="margin-left: 20px;" cellspacing="5" cellpadding="0" border="0">
        <tbody>
            <!--Detalle-->           
            <tr>
                <td>
                    <b><%=Me.GetLabel("00695")%>&nbsp;:&nbsp;<%=Me.TicketData.Id.ToString().PadLeft(8, "0")%></b>
                </td>
                <td align="right" style="color: red;">
                    <%If Me.TicketData.status = 2 Then%>
                        <b><%=Me.GetLabel("01295")%></b>    
                    <%End If%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01298")%>&nbsp;:&nbsp;</b><%=Me.TicketData.OwnerName%>&nbsp;(<%=Me.TicketData.OwnerEmail%>)
                </td>
            </tr>
            <%If Me.TicketData.ownerCompany.Trim().Length > 0 Then%>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01187")%>&nbsp;:&nbsp;</b><%=Me.TicketData.ownerCompany%>
                </td>
            </tr>
            <%End If%>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01299")%>&nbsp;:&nbsp;</b><%=Me.TicketData.StartDate.ToString("dd/MMM/yyyy hh:mm tt")%>
                </td>
            </tr>
            <%If Me.TicketData.Status = TicketData.TicketStatus.Close Then%>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01300")%>&nbsp;:&nbsp;</b><%=Me.TicketData.EndDate.ToString("dd/MMM/yyyy hh:mm tt")%>
                </td>
            </tr>
            <%End If%>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01286")%>&nbsp;:&nbsp;</b><%=Me.TicketData.Subject%>
                </td>
            </tr>
            <%--<tr>
                <td colspan="2">
                    <b>Descripción&nbsp;:&nbsp;</b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    = HttpUtility.HtmlEncode(me.TicketData.)
                </td>
            </tr>--%>
            <%If Me.TicketData.HasFile Then%>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01287")%>&nbsp;:&nbsp;</b>
                    <asp:LinkButton CausesValidation="false" ID="btnGetFile" runat="server"><%=Me.TicketData.FileName%></asp:LinkButton>
                </td>
            </tr>
            <%End If%>
            <!--Comentarios-->
            <tr>
                <td class="Titulo" align="center" colspan="2">
                    <span class="TituloForma"><%=Me.GetLabel("01301")%></span>
                </td>
            </tr> 
            <tr>
                <td colspan="2">
                    <asp:DataList ID="lstComments" runat="server" CssClass="clsGridView" BackColor="#E0E0E0" BorderColor="Black" BorderWidth="0px" GridLines="Both" Width="100%">
                        <AlternatingItemStyle CssClass="clsGridViewItem"/> 
                        <ItemTemplate>
                            <div class="DescriptionItem">
                                <div class="Owner"><%#Eval("OwnerName") + " " + Me.GetLabel("01310") + ":"%></div>
                                <div class="Date"><%#Convert.ToDateTime(Eval("PostDate")).ToString("dd/MMM/yyyy HH:mm tt")%></div>
                                <div class="Comment"><%#HttpUtility.HtmlEncode(Eval("Comment"))%></div>
                            </div>  
                       </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <!--Acciones-->
            <% If Me.IsEditable Then%>
            <tr>
                <td colspan="2">
                    <b><%=Me.GetLabel("01302")%>&nbsp;:&nbsp;</b><br />
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" CssClass="clsTextBoxDescripcion" Height="100px" Width="100%"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RfvDescripcion" runat="server" ControlToValidate="txtDescripcion" CssClass="clsLabelError" ForeColor=""><%=Me.GetLabel("00071")%></asp:RequiredFieldValidator>               
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <%
                        Me.btnActualizar.Text = Me.GetLabel("01303")
                        Me.btnActualizar.OnClientClick = "var flag = ValidateComment();  if (!flag) {alert('" + Me.GetLabel("01304") + "'); } return flag;"
                    %>
                    <asp:Button ID="btnActualizar" runat="server" CssClass="button" />
                </td>
            </tr>
            <%End If%>
        </tbody>
    </table>
    <div style="width:750px; text-align:left; padding-left:10px; padding-top:5px;"> 
        <a href="TicketList.aspx">&lt;&lt; <%=Me.GetLabel("01305")%></a>
    </div>
    <script type="text/javascript">
        function ValidateComment() {
            var input = document.getElementById('<%= Me.txtDescripcion.ClientId %>');
            return (input != null && input.value != '');
        }        
    </script>
    <% End If%>
    </form>
    
</body>
</html>