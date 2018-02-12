<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlAutoComplete.ascx.vb" Inherits="RateManager.ctrlAutoComplete" %>

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

<script type="text/javascript" language="javascript">
    jQuery.noConflict();

    (function($) {

        function FireList(xml) {
            arr = [];
            arr1 = [];
            if (xml) {
                if (gp.searchitems.length >= 1) {
                    gp.Count = 1;
                    $(xml).find(gp.searchitems[0].Item).each(function() {
                        arr.push({
                            'id': $(this).find(gp.searchitems[0].IDSearch).text(),
                            'name': $(this).find(gp.searchitems[0].nameSearch).text()
                        });
                    });
                }
                if (gp.searchitems.length >= 2) {
                    gp.Count = 2;
                    //$('#<%= me.lnkFilter.ClientId %>').show();
                    $(xml).find(gp.searchitems[1].Item).each(function() {
                        arr1.push({
                            'id': $(this).find(gp.searchitems[1].IDSearch).text(),
                            'name': $(this).find(gp.searchitems[1].nameSearch).text()
                        });
                    });
                }
            }
        }

        function FirejQueryGridSrv() {
            var dt = new Date();
            var url = gp.url;
            $.ajax({
                url: '<%= Me.ResolveUrl("~/Pages/Data/ExecutorServer.ashx") %>',
                dataType: gp.dataType,
                cache: false,
                type: 'GET',
                mustMatch: false,
                selectFirst: true,
                data: gp.Data[0],
                //data: { 'idHotel': '207', 'Catalogo': gp.id },  // Aqui pon los parametros que le quieras pasar por querystring
                success: function(xml) {
                    FireList(xml);
                },
                error: function(response) {

                },
                complete: function(response) {
                    $('#<%= me.autocomplete.ClientId %>').autocomplete({
                        source: function(request, response) {
                            var arrUi = (gp.index == 0) ? arr : arr1;
                            var list = $.grep(arrUi, function(item, idx) {
                                var filter = request.term.replace(/a/gi, '[a|á|ä|â|à]').replace(/e/gi, '[e|é|ë|ê|è]').replace(/i/gi, '[i|í|ï|î|ì]').replace(/o/gi, '[o|ó|ö|ô|ò]').replace(/u/gi, '[u|ú|ü|û|ù]');
                                if (filter != '*') {
                                    filter = new RegExp('.*' + filter + '.*', 'i');
                                    return filter.test(item.name);
                                }
                                return '';
                            });
                            response(list);
                        },
                        minLength: 1,
                        select: function(event, ui) {
                            $('#<%= me.autocomplete.ClientId %>').val(ui.item.name);
                            $('#<%= me.idCliente.ClientId %>').val(ui.item.id);
                            $('#<%= Me.ImgSaveSearch.ClientId %>').click();
                            return false;
                        },
                        focus: function(event, ui) { }

                    }).data("autocomplete")._renderItem = function(ul, item) {
                        $('#<%= me.lnkCancel.ClientId %>').show();
                        $('#<%= me.lnkSearch.ClientId %>').hide();
                        $('#<%= me.DivRadioButtons.ClientId %>').hide();
                        return $("<li></li>")
				            .data("item.autocomplete", item)
				            .append("<a>" + item.name + "</a>")
				            .appendTo(ul);
                    };
                }
            });
        }

        function divFilter() {
            SetTextRadio('<%= me.lblRb1.ClientId %>', '<%= me.Rb1.ClientId %>', '<%= me.DivRadioButtons.ClientId %>', 1, 0);
            SetTextRadio('<%= me.lblRb2.ClientId %>', '<%= me.Rb2.ClientId %>', '<%= me.DivRadioButtons.ClientId %>', 2, 1);
            if (gp.searchitems.length == 1) {
                document.getElementById('<%= me.Rb1.ClientId %>').checked = true;
                gp.index = 0;
            }
            SetTextFilter('<%= me.spanHeader.ClientId %>', '<%=RateManager.PortalCulture.GetString("M0BT0000113")%>', gp.index);

        }


        $(document).ready(function() {
            if ($('#<%= me.autocomplete.ClientId %>').length > 0) {
                FirejQueryGridSrv();
                SearchStart.SetFilter('#<%= me.idAutocompleteFilter.ClientId %>');

            }
            divFilter();
            if (document.getElementById('<%= me.autocomplete.ClientId %>').value == '') {
                $('#<%= me.lnkCancel.ClientId %>').hide();
                $('#<%= me.lnkSearch.ClientId %>').show();
            }
            else {
                $('#<%= me.lnkCancel.ClientId %>').show();
                $('#<%= me.lnkSearch.ClientId %>').hide();
            }
            SearchStart.Loader(false);
        });

        $(document).keypress(function(e) {
            if (e.keyCode == 13) {
                e.cancelBubble = true;
                e.returnvalue = false;
                $('#<%= Me.ImgSaveSearch.ClientId %>').click();
                return false;
            }
        });

    })(jQuery);

    function FireCrearAutoComplete() {
        (function($) {

            $('#<%= me.autocomplete.ClientId %>').val('');
            $('#<%= me.autocomplete.ClientId %>').focus();
        })(jQuery);

        return;
    }

    function FireHlnkClose() {
        (function($) {
            var ids = document.getElementById('<%= me.inpuFilter.ClientId %>').value;
            $('#<%= me.autocomplete.ClientId %>').val('');
            $('#<%= me.lnkCancel.ClientId %>').hide();
            $('#<%= me.lnkSearch.ClientId %>').show();
            if (!(document.getElementById('<%= me.autocomplete.ClientId %>').value == "" && document.getElementById('<%= me.inpuFilter.ClientId %>').value == "")) {
                $('#<%= Me.ImgSaveSearch.ClientId %>').click();
            }
        })(jQuery);
        return;
    }

    function FireCloseDivAutoComplete(id) {
        id.style.display = 'none';
    }

    function FireShowFilterAutoComplete() {
        (function($) {
            if (gp.Count > 1) {
                $('#<%= me.lnkFilter.ClientId %>').show();
            }
        })(jQuery);
    }
    function FirehideFilterAutoComplete() {
        (function($) {
            $('#<%= me.lnkFilter.ClientId %>').hide();
        })(jQuery);
    }    

</script>

<table id="tblAutocomplete" cellpadding="0" cellspacing="0" border="0" style="width: 99%; padding-top: 12px; margin-top:2px; ">
    <tr>
        <td style="width: 50%; text-align: right;">
        </td>
        <td style="width: 40%; text-align: right;">
            <input type="text" id="autocomplete" class="TextBox" runat="server" style="width: 400px;" />
        </td>
        <td style="width: 10%;">
            <asp:ImageButton ID="ImgSaveSearch" runat="server" ImageUrl="" EnableViewState="False" ValidationGroup="Agreement"
                TabIndex="21" CausesValidation="true" Style="display: none; width: 14px;" />
            <input type="hidden" id="idCliente" runat="server" value="0" />
            <asp:HyperLink ID="lnkSearch" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/Search.png"
                Style="display: none; width: 20px; height: 20px; padding-left: 4px;">[Search]</asp:HyperLink>
            <asp:HyperLink ID="lnkCancel" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/close.png"
                Style="display: none; width: 20px; height: 20px;">[close]</asp:HyperLink>
            <asp:HyperLink ID="lnkFilter" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/Filter.png"
                Style="width: 20px; height: 20px;">[Filter]</asp:HyperLink>
            <input type="hidden" id="idAutocompleteFilter" value="" runat="server" />
            <input id="inpuFilter" type="hidden" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td align="left">
            <div id="DivRadioButtons" style="clear: both; border: solid 1px #ccc; position: absolute; float: right !important;
                height: 50px; padding-left: 4px; padding-right: 8px; padding-top: 4px; margin-left: 8px; background: #ffffcc;
                display: none;" runat="server" onclick="FireCloseDivAutoComplete(this);">
                <table cellpadding="0" cellspacing="0" style="width: 200px;">
                    <tr>
                        <td colspan="4">
                            <span>
                                <%=RateManager.PortalCulture.GetString("M0BT0000113")%>:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rb1" runat="server" GroupName="list" />
                        </td>
                        <td>
                            <asp:Label ID="lblRb1" Text="Codigo" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButton ID="rb2" runat="server" GroupName="list" Style="" />
                        </td>
                        <td>
                            <asp:Label ID="lblRb2" Text="Nombre" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td align="right">
            <span style="font-size: 9px;" id="spanHeader" runat="server">
                <%=RateManager.PortalCulture.GetString("M0BT0000113")%></span>
        </td>
        <td>
        </td>
    </tr>
</table>
