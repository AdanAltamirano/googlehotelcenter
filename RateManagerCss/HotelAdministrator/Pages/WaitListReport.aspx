<%@ Page Language="vb" Inherits="RateManager.WaitListReport" %>

<%@ Import Namespace="Portal.Hotel.DataAccess" %>
<%@ Import Namespace="RateManager" %>
<%@ Import Namespace="Portal.General.Facade" %>
<%@ Import Namespace="Portal.General.Common.Data" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>

<%@ Register Src="../../ListDate/Date.ascx" TagName="Date" TagPrefix="uc1" %>

<script runat="server">

    Private Const EXPIRETIME As Integer = 2
    
    Private Enum Columns
        Reference = 0
        HotelName = 1
        [Date] = 2
        Name = 3
        CheckIn = 4
        Nights = 5
        Rooms = 6
        RateCode = 7
    End Enum
    
    Private _currencyCodes As Dictionary(Of String, String)
    
    Private Const DATEFORMAT As String = "dd/MMM/yyy"
        
    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
        'Return String.Empty
    End Function
                
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not MyBase.IsSupervisor Then MyBase.redirectTo(PaginaBase.pages.Home)
        
        If Not Me.IsPostBack Then
            Me.lstTipoFechas.Items.Clear()
            Me.lstTipoFechas.Items.Add(New ListItem(Me.GetLabel("00368"), 1))
            Me.lstTipoFechas.Items.Add(New ListItem(Me.GetLabel("M0BT0000235"), 2))
            
            Me.lstOrderField.Items.Clear()
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00695"), "id asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00158"), "hotelName asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("M0BT0000235"), "date asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00073"), "firstName asc, lastName asc"))
            Me.lstOrderField.Items.Add(New ListItem(Me.GetLabel("00368"), "checkIn asc"))
            
            Me.txtDesde.maxYear = Today.Year + 2
            Me.txtDesde.minYear = Today.Year - 1
            Me.txtDesde.selectedDate = Today
            Me.txtHasta.maxYear = Today.Year + 2
            Me.txtHasta.minYear = Today.Year - 1
            Me.txtHasta.selectedDate = Today.AddDays(30)
            'Me.chkFechas.Checked = True
            'Me.RefreshData(sender, e)
        End If
                
    End Sub
    
    Protected Sub ChangePage(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles lstClientes.PageIndexChanged
        
        Dim table As Data.DataTable = Me.GetData()
        Dim pages As Integer = table.Rows.Count \ Me.lstClientes.PageSize
        If table.Rows.Count Mod Me.lstClientes.PageSize > 0 Then pages += 1

        If e.NewPageIndex >= pages Then
            Me.lstClientes.CurrentPageIndex = pages - 1
        Else
            Me.lstClientes.CurrentPageIndex = e.NewPageIndex
        End If
        Me.lstClientes.DataSource = table
        Me.lstClientes.DataBind()
        
    End Sub
    
    Private Function GetData() As Data.DataTable
        Dim data As New Data.DataTable
        Dim controller As New WaitListDataAccess()
        Dim status As Integer = 0
        Dim expiredHours As Integer = 24

        If Me.chkStatusInProcess.Checked Then status += 1
        If Me.chkStatusAccepted.Checked Then status += 2
        If Me.chkStatusRejected.Checked Then status += 4
        If Me.chkStatusExpired.Checked Then status += 8
        If status = 0 Then status = 1

        If Me.chkFechas.Checked Then
            data = controller.GetList(0, Me.txtDesde.selectedDate, Me.txtHasta.selectedDate, Me.lstTipoFechas.Items(Me.lstTipoFechas.SelectedIndex).Value, status, Me.txtNombre.Text, Me.txtReferencia.Text, expiredHours, Me.txtHotelName.Text.Trim())
        Else
            data = controller.GetList(0, status, Me.txtNombre.Text, Me.txtReferencia.Text, expiredHours, Me.txtHotelName.Text.Trim())
        End If

        data.DefaultView.Sort = Me.lstOrderField.SelectedItem.Value
        
        Dim dv As DataView = data.DefaultView
        HttpContext.Current.Session("dvReport") = dv
        
        Return data.DefaultView.Table
    End Function
    
    Protected Sub RefreshData(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        
        Me.lstClientes.CurrentPageIndex = 0
        Me.lstClientes.DataSource = Me.GetData()
        Me.lstClientes.DataBind()

        Me.pnlReport.Visible = (Me.lstClientes.Items.Count > 0)
                
    End Sub
           
    
    Protected Sub lstClientes_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(Columns.Reference).Text = Me.GetLabel("01235")
            e.Item.Cells(Columns.Date).Text = Me.GetLabel("M0BT0000235")
            e.Item.Cells(Columns.Name).Text = Me.GetLabel("00073")
            e.Item.Cells(Columns.CheckIn).Text = Me.GetLabel("M000078")
            e.Item.Cells(Columns.Nights).Text = Me.GetLabel("M0BT0000070")
            e.Item.Cells(Columns.HotelName).Text = Me.GetLabel("00158")
            e.Item.Cells(Columns.Rooms).Text = Me.GetLabel("M000609")
            e.Item.Cells(Columns.RateCode).Text = Me.GetLabel("00006")
        End If
    End Sub
    
    Protected Sub lstClientes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim data As Data.DataRowView = Convert.ChangeType(e.Item.DataItem, GetType(Data.DataRowView))

            If data IsNot Nothing Then
                Select Case Convert.ToInt32(data("Status"))
                    Case 2
                        e.Item.CssClass += If(e.Item.ItemType = ListItemType.AlternatingItem, "dgAlternate", "dgItem") + "Not"
                    Case 4
                        e.Item.CssClass += If(e.Item.ItemType = ListItemType.AlternatingItem, "dgAlternate", "dgItem") + "Can"
                    Case 8
                        e.Item.CssClass += If(e.Item.ItemType = ListItemType.AlternatingItem, "dgAlternate", "dgItem") + "Exp"
                End Select
            End If

        End If
    End Sub
          
    
</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 transitional//EN" >--%><html>
<head id="Head1" runat="server">
    <title><%=Me.GetLabel("01233")%></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

    <style type="text/css">
        .dgItemCan {
            background-color: rgb(254,189,180);
        }

        .dgAlternateCan {
            background-color: rgb(253,169,157);
        }

        .dgItemExp {
            background-color: rgb(255,213,64);
        }

        .dgAlternateExp {
            background-color: rgb(255,232,149);
        }

        .dgItemNot {
            background-color: rgb(204,227,198);
        }

        .dgAlternateNot {
            background-color: rgb(175,211,165);
        }

        #pnlStatus .sample {
            display: inline;
            height: 12px;
            width: 16px;
            border: 1 solid;
        }

        .accepted {
            background-color: rgb(175,211,165);
        }

        .rejected {
            background-color: rgb(253,169,157);
        }

        .expired {
            background-color: rgb(255,232,149);
        }


        /*Printing*/
        @media print {
            #BookingContainer {
                background-color: transparent;
                border: none;
                width: 100%;
            }

                #BookingContainer .screen {
                    display: none;
                }

            #boxInfo {
                border: none;
            }

            .dgHeader span {
                font-size: 14px;
                font-weight: bold;
            }

            .dgHeader .dgItem {
                font-size: 14px;
                font-weight: bold;
            }
        }
    </style>

</head>
<body>
    <%
        Dim sysCulture As System.Globalization.CultureInfo
        sysCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString())
    %>
    <form id="Form1" runat="server" method="post">
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="750" border="0">
            <tr>
                <td class="Titulo" align="center">
                    <asp:Label ID="lblTitle" runat="server" CssClass="TituloForma" EnableViewState="False"><%=Me.GetLabel("01233")%></asp:Label>
                </td>
            </tr>
            <tr class="screen">
                <td align="center">
                    <table id="Table2" border="0" cellspacing="3" cellpadding="1" width="100%">
                        <tr class="dgHeader">
                            <td class="dgitem" align="center" colspan="4">
                                <span><%=GetLabel("00479")%></span>
                            </td>
                        </tr>

                        <tr>
                            <td align="right">
                                <span class="clsLabel"><%=GetLabel("00158")%>&nbsp;:&nbsp;</span>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtHotelName" runat="server" Width="400px"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td align="right">
                                <span class="clsLabel"><%=GetLabel("00073")%>&nbsp;:&nbsp;</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNombre" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td align="right">
                                <span class="clsLabel"><%=GetLabel("00695")%>&nbsp;:&nbsp;</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtReferencia" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4" style="padding-left: 20px;">
                                <asp:CheckBox ID="chkFechas" runat="server" Checked="false" /><%=Me.GetLabel("00517")%>
                            </td>
                        </tr>
                        <tr id="pnlDatesSelection">
                            <td align="right">
                                <span class="clsLabel"><%=GetLabel("01246")%>&nbsp;:&nbsp;</span>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="lstTipoFechas" runat="server" EnableViewState="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="pnlDates">
                            <td align="right">
                                <span class="clsLabel"><%=GetLabel("00108")%>&nbsp;:&nbsp;</span>
                            </td>
                            <td align="left">
                                <uc1:Date ID="txtDesde" runat="server" />
                            </td>
                            <td align="right">
                                <span class="clsLabel"><%=GetLabel("00109")%>&nbsp;:&nbsp;</span>
                            </td>
                            <td align="left">
                                <uc1:Date ID="txtHasta" runat="server" />
                            </td>
                        </tr>
                        <tr id="pnlStatus">
                            <td colspan="4" style="padding-left: 20px;">
                                <%=Me.GetLabel("01319")%>&nbsp;:&nbsp;
                            <input type="checkbox" id="chkStatusInProcess" checked="checked" runat="server" /><div class="sample inProcess"></div>
                                &nbsp;<%=Me.GetLabel("00439")%>&nbsp;                            
                            <input type="checkbox" id="chkStatusAccepted" runat="server" /><div class="sample accepted"></div>
                                &nbsp;<%=Me.GetLabel("01268")%>&nbsp;
                            <input type="checkbox" id="chkStatusRejected" runat="server" /><div class="sample rejected"></div>
                                &nbsp;<%=Me.GetLabel("01318")%>&nbsp;
                            <input type="checkbox" id="chkStatusExpired" runat="server" /><div class="sample expired"></div>
                                &nbsp;<%=Me.GetLabel("01267")%>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left: 20px;">Ordenar por&nbsp;:&nbsp;
                            <asp:DropDownList Style="vertical-align: middle;" ID="lstOrderField" runat="server" EnableViewState="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <% Me.btnFilter.Text = Me.GetLabel("M000637")%>
                                <asp:Button ID="btnFilter" runat="server" Text="Buscar" EnableViewState="False" CssClass="button" CausesValidation="true" />
                            </td>
                            <td>
                                <asp:HyperLink ng-show="btnExcel" ID="hlnkExcel" runat="server" Enabled="true" NavigateUrl="ExcelWaitListReport.aspx"
                                    ToolTip="Excel" ImageUrl="../../Images/excel.png"></asp:HyperLink></td>
                        </tr>
                    </table>

                    <script type="text/javascript">

                        var fechas = $('#<%=Me.chkFechas.ClientId %>');
                        fechas.click(
                            function () {
                                $('#pnlDatesSelection').css('display', ((this.checked) ? '' : 'none'));
                                $('#pnlDates').css('display', ((this.checked) ? '' : 'none'));
                            }
                        );
                        $('#pnlDatesSelection').css('display', ((fechas.attr('checked')) ? '' : 'none'));
                        $('#pnlDates').css('display', ((fechas.attr('checked')) ? '' : 'none'));

                    </script>
                </td>
            </tr>
            <tr height="5">
                <td></td>
            </tr>
            <tr id="pnlReport" visible="false" runat="server">
                <td align="center">
                    <asp:DataGrid ID="lstClientes" runat="server" CssClass="DataGrid"
                        ShowFooter="True" AutoGenerateColumns="False" AllowPaging="True" Width="98%"
                        PageSize="50" OnItemDataBound="lstClientes_ItemDataBound">
                        <SelectedItemStyle CssClass="dgSelected" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                            HorizontalAlign="Left"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="dgAlternate" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left"></AlternatingItemStyle>
                        <ItemStyle CssClass="dgItem" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                            HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle CssClass="dgHeader" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                            HorizontalAlign="Center"></HeaderStyle>
                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Referencia">
                                <ItemTemplate>
                                    <%#Eval("id").ToString().PadLeft(8, "0")%>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="hotelName" HeaderText="Hotel"></asp:BoundColumn>
                            <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy}" DataField="date" HeaderText="Fecha de registro">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Nombre">
                                <ItemTemplate>
                                    <%#Eval("firstName") + " " + Eval("lastName")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy}" DataField="checkIn" HeaderText="Fecha Llegada">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Noches">
                                <ItemTemplate>
                                    <%#Convert.ToDateTime(Eval("checkOut")).Subtract(Convert.ToDateTime(Eval("checkIn"))).TotalDays.ToString()%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Rooms" HeaderText="Rooms">
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="RateCode" HeaderText="Rate Code">
                                <HeaderStyle Width="50px" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                            Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <div class="screen" style="width: 100%; text-align: right; display: none;">
                        <input class="Button" value='<%= me.getlabel("01326") %>' onclick="window.print(); return false;" type="button" /></div>
                </td>
            </tr>
        </table>


    </form>
    <% System.Threading.Thread.CurrentThread.CurrentCulture = sysCulture%>
</body>
</html>
