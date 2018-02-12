<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogReader.aspx.vb" Inherits="RateManager.LogReader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: arial, helvetica, sans-serif;
            font-size: 12px;
            font-weight: normal;
        }
        .TextBox
        {
            border: 1px solid #a9a9a9;
            font-family: arial, helvetica, sans-serif;
            font-size: 11px;
            font-weight: normal;
            color: #000033;
            margin-bottom: 0px;
            padding-top: 2px;
            padding-bottom: 2px;
            padding-left: 4px;
        }
        hr
        {
            color: #ccc;
            height: 0.08em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="2" border="0">
            <tr>
                <td style="text-align: right; padding-right: 4px;">
                    <asp:Label ID="Label1" runat="server" Text="Year: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtYear" runat="server" CssClass="TextBox" Columns="4" MaxLength="4"></asp:TextBox>
                </td>
                <td rowspan="3" style="padding-left: 8px;">
                    <asp:Button ID="cmdRead" runat="server" Text="read" Style="width: 60px;" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding-right: 4px;">
                    <asp:Label ID="Label2" runat="server" Text="Month: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMonth" runat="server" CssClass="TextBox" Columns="2" MaxLength="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding-right: 4px;">
                    <asp:Label ID="Label3" runat="server" Text="Day:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDay" runat="server" CssClass="TextBox" Columns="2" MaxLength="2"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    <asp:Label ID="lblError" runat="server"></asp:Label>
    </form>
</body>
</html>
