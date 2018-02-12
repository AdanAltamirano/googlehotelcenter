<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctlMensajes.ascx.vb" Inherits="RateManager.ctlMensajes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<script src="<%= Me.ResolveUrl("~/Pages/Scripts/jquery.min.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    function cancel(pnl1, pnl2) {    
        document.getElementById(pnl1).style.display = 'none';
        document.getElementById(pnl2).style.display = 'none';
        var arrSelects = document.getElementsByTagName('SELECT');
        
        for (var i = 0; i < arrSelects.length; i++)
        { arrSelects[i].style.display = ''; }
        //Funciona para no mostrar los combos necesarios
        ocultarCombos();
    }
    
    function _ok()
    {
    }

    function ok(pnl1, pnl2) {
        var txt = document.getElementById('txtIdObj');
        var obj = document.getElementById(txt.value);
        
        if (navigator.userAgent.indexOf("Chrome") != -1) {
            var fireOnThis = document.getElementById(txt.value);
            var evObj = document.createEvent('MouseEvents');
            evObj.initMouseEvent('click', true, true, window, 0, 0, 345, 7, 220, false, false, true, false, 0, null);
            fireOnThis.dispatchEvent(evObj);
            return true;
        }
        else {
            document.getElementById(txt.value).click();
        }
        /*eval($('#' + $('#txtIdObj').val()).attr('onclick'));*/

    }

    function autoResizeObject(obj) {
        var lHeight = 0;
        
        if (typeof (window.innerWidth) == 'number') {
            //Non-IE        
            lHeight = window.innerHeight;
        } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
            //IE 6+ in 'standards compliant mode'        
            lHeight = document.documentElement.clientHeight;
        } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
            //IE 4 compatible
            lHeight = document.body.clientHeight;
        }
        //lHeight = lHeight > 120 ? lHeight - 140 : lHeight;
        //obj2();
        aler();
        obj.style.left = 400 + "px";
        obj.style.top = (lHeight / 2) + "px";
        //obj.style.height = lHeight + "px";
    }


    function show(pnl1, pnl2, obj, _msg) {
        document.getElementById(pnl1).style.display = "block";
        document.getElementById(pnl2).style.display = "block";
        var arrSelects = document.getElementsByTagName('SELECT');
        for (var i = 0; i < arrSelects.length; i++)
        { arrSelects[i].style.display = 'None'; }
        var txt = document.getElementById('txtIdObj');
        txt.value = obj;
        //autoResizeObject(document.getElementById('frmPrincipal'));
       // popup();
        onResizeIframe('frmPrincipal');
        if (_msg) {
            var emsg = document.getElementById('CtlMensajes1_lblPrompt');
            if (emsg) emsg.innerHTML = _msg;
        }        
    }
    function popup() {
        $(document).ready(function() {
            //alert("si entro");
            var html = $(".popup").html();
            $(".popup").remove();
            var htmln = "<div id='dvpopup' class='popup'>" + html + "</div>";
            $("#contenido").prepend(htmln);
            alert(htmln);

        });
    }
    function ocultarCombos() {
        var c3 = document.getElementById('ddlCancelationPolicy');
        if (c3 != null) {
            if (c3.selectedIndex != 2) {
                var c1 = document.getElementById('ddlHour');
                if (c1 != null) {
                    c1.style.display = 'none';
                }
                var c2 = document.getElementById('ddlMinutes');
                if (c2 != null) {
                    c2.style.display = 'none';
                }
            }
        }
    }
</script>


    <input id="txtIdObj" type="hidden" name="txtIdObj">
  <%--  <div runat=server style=" position: absolute; float: left; display: block; height:100%; width:100%; filter: alpha(opacity=25); "	>
        <input value = "ssssssssss" />
    </div>--%>
   <asp:Panel ID="TblPnl" runat="server" Style="display: none; z-index: 997; filter: alpha(opacity=100); left: 0px; 
        position: absolute; top: 0px; background-color: whitesmoke; float:left;" runat="server" Width="99%" Height="99%">
    </asp:Panel>
       <asp:Panel ID="PnlBox" runat="server" Style="display: none; text-align:center; z-index: 998; left: 35%; position:absolute;
        top: 0px" runat="server" >        
        <div class="boxMsg">
            <div class="titleboxMsg">
                <asp:Label ID="lblTitle" runat="server"></asp:Label>
            </div>
            <div class="contentBox">
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td valign="middle">
                            <table id="tblMsj" cellspacing="1" cellpadding="0" align="center" border="0">                                
                                <tr>
                                    <td>
                                        <div class="iconBox">
                                        </div>
                                    </td>
                                    <td align="center">
                                          <asp:Label ID="lblPrompt" runat="server" CssClass="clsDarkLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <input class="button" id="btnOk" onclick="javascript:ok('TblPnl','PnlBox')" type="button"
                                            value="Aceptar" width="60px" runat="server" style="float: none;">
                                        <input class="button" id="btnCancel" type="button" style="float: none;" value="Cancelar"
                                            width="60px" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    
    <%--Style="border-right: black 1px solid; border-top: black 1px solid;
        display: none; z-index: 997; filter: alpha(opacity=25); left: 0px; border-left: black 1px solid;
        border-bottom: black 1px solid; position: absolute; top: 0px; background-color: whitesmoke;
        moz-opacity: .25; opacity: .25" runat="server" Width="100%" Height="100%"--%>
        
       <%--  Style="display: none; z-index: 998; left: 0px; position: fixed;
        top: 0px" runat="server" Width="100%" Height="100%"--%>