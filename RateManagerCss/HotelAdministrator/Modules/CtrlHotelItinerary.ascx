<%@ Import Namespace="RateManager" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrlHotelItinerary.ascx.vb" Inherits="RateManager.CtrlHotelItinerary" %>
	 
    <table id="bookingcontainer" cellspacing="0" cellpadding="2"  border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">                   
                    <tr>
                         <td >
                            <asp:Label ID="lblEStatus" runat="server" CssClass="labelBold">Status</asp:Label>
                           <asp:Label ID="lblStatus" runat="server" EnableViewState="False"  Font-Bold="True" CssClass="clsLabel">Reserved</asp:Label>
                          <a id="ADetails" runat="server" href="javascript:;" class="dglink"> </a>
                        </td>
                        <td>
                        </td>
                        <td align="right">
                          <asp:Label ID="lblNoCanc" runat="server" EnableViewState="False" CssClass="clslabel">No. Cancel</asp:Label>
                            <asp:Label ID="lblNoCancelacion" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"></asp:Label>
                            <%If Me.CanShowConfirmButton Then%>
                            <br />
                            <a class="dglink" style="display: inline; margin-top: 5px;" href="javascript:" id="btnShowPayConfirm">
                                <%=RateManager.PortalCulture.GetString("01407")%></a>&nbsp;
                            <%End If%>
                            <br />
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Literal ID="lblpay" runat="server"></asp:Literal>
                            <a id="detailObserv" runat="server" href="javascript:;" class="dglink" visible="FALSE">
                                Observaciones</a>
                            <div id="divObservaciones" style="padding-right: 3px; display: none; padding-left: 3px;
                                padding-bottom: 3px; padding-top: 3px; position: absolute" ms_positioning="FlowLayout">
                                <table id="Table1" align="right">
                                    <tr>
                                        <td width="300" class="titulo">
                                            <asp:Label ID="Label2" runat="server">Observaciones:</asp:Label>
                                        </td>
                                        <td width="10" align="right">
                                            <img align="right" runat="server" id="imgCloseObserv" src="../../Images/close.png"
                                                style="cursor: hand;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblObservaciones" runat="server">Observaciones del deposito</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div id="divDetails" class="boxMsgCan" ms_positioning="FlowLayout" style="display: none;">
                                <table id="bookingcontainer">
                                    <tr>
                                        <td width="300" class="titulo" valign="top">
                                            <asp:Label ID="lblEMotivocancelacion" runat="server">Motivo de Cancelación:</asp:Label>
                                        </td>
                                        <td width="32" align="right" valign="top">
                                            <img align="right" runat="server" id="imgclose" src="~/Includes/imagenes/close.ico"
                                                style="cursor: hand">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblMotivoCancelacion" runat="server">Hay sobrecarga de venta de habitaciones y no tenemos donde hubicar al cliente por lo que lo trasladamos a paradise hotel</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                             <div id="divItems" class="boxMsgCan" ms_positioning="FlowLayout" style="display: none;">
                                <table style="background:#FFF;width:100%;">
                                    <tr>
                                        <td width="300" class="titulo" valign="top">
                                            <asp:Label ID="lblItems" runat="server">Items:</asp:Label>
                                        </td>
                                        <td width="32" align="right" valign="top">
                                            <img align="right" runat="server" id="imgCloseItems" src="~/Includes/imagenes/close.ico"
                                                style="cursor: hand">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:DataGrid ID="dgItems" runat="server" EnableViewState="False" AutoGenerateColumns="False"
                                                HorizontalAlign="Center" Width="100%" CssClass="DataGrid">
                                                <Columns>
                                                    <asp:BoundColumn DataField="Name"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Price"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Currency"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                            Total: <asp:Label ID="lblTotalItems" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="left">
                        </td>
                        <td align="right">
                          
                         
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <%If Me.CanShowConfirmButton Then%>
                            <div id="boxPayConfirmation" class="boxMsgCan" style="display: none; background-color: #f9fcff;">
                                <table id="Table4" style="width: 100%;">
                                    <tr>
                                        <td class="modulo tituloModulo" colspan="2">
                                            <%=RateManager.PortalCulture.GetString("01404")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <span class="label">
                                                <%=RateManager.PortalCulture.GetString("01281")%>:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox class="field" ID="txtPayAutorization" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <span class="label">
                                                <%=RateManager.PortalCulture.GetString("01405")%>:</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="lstPayMode" runat="server">
                                                <asp:ListItem Text="Banamex - UV" Value="1" />
                                                <asp:ListItem Text="Bancomer - HeM" Value="4" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <%
                                                Me.btnPayConfirm.OnClientClick = "if($('#" + Me.txtPayAutorization.ClientID + "').val().length > 0) {return true;} else {$('#" + Me.txtPayAutorization.ClientID + "').focus(); return false;}"
                                                Me.btnPayConfirm.Text = RateManager.PortalCulture.GetString("01406")
                                            %>
                                            <asp:Button CssClass="Button" ID="btnPayConfirm" runat="server" Text="Confirmar" />
                                            <input class="Button" type="button" value='<%=RateManager.PortalCulture.GetString("A00143")%>'
                                                id="btnPayCancel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%End If%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3"><asp:Label ID="lblNHotel" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"
                                Font-Size="9pt">Hotel Marina</asp:Label>,&nbsp;<asp:Label ID="lblDirHotel" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Carretera a Pichilingue km. 2.5</asp:Label>,&nbsp;<asp:Label ID="lblCdHotel" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">La Paz B.C.S.</asp:Label></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblEIn" runat="server" EnableViewState="False" CssClass="MiddleText"> Check-In Date:</asp:Label>&nbsp;<asp:Label
                                ID="lblIn" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">21 Aug 05</asp:Label><br>
                            <asp:Label ID="lblEOut" runat="server" EnableViewState="False" CssClass="MiddleText">Check-Out Date :</asp:Label>&nbsp;<asp:Label
                                ID="lblOut" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">26 Aug 05</asp:Label><br>
                            <asp:Label ID="lblEResDate" runat="server" EnableViewState="False" CssClass="MiddleText"> Reservation Date:</asp:Label>&nbsp;
                            <asp:Label ID="lblResDate" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">26 Aug 05</asp:Label>                        
                        </td>
                        <td align="left">
                            
                            
                        </td>
                        <td align="right" valign=top>                          
                            <table id="Table8" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblESystemCode" runat="server" EnableViewState="False" CssClass="MiddleText">System Code :</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblSystemCode" runat="server" EnableViewState="False" Font-Bold="True"
                                            CssClass="clsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEBookingSource" runat="server" EnableViewState="False" CssClass="MiddleText">BookingSource</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblBookingSource" runat="server" EnableViewState="False" Font-Bold="True"
                                            CssClass="clsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblERecordLocator" runat="server" EnableViewState="False" CssClass="MiddleText">RecordLocator:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblRecordLocator" runat="server" EnableViewState="False" Font-Bold="True"
                                            CssClass="clsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblERF" runat="server" EnableViewState="False" CssClass="MiddleText">RF</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblRF" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblESource" runat="server" EnableViewState="False" CssClass="MiddleText">Source:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblSource" runat="server" EnableViewState="False" Font-Bold="True"
                                            CssClass="clsLabel">Univisit/Galileo</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEIdBooking" runat="server" EnableViewState="False" CssClass="MiddleText">ID booking : </asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblIdBooking" runat="server" EnableViewState="False" Font-Bold="True"
                                            CssClass="clsLabel">000000</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                             <a id="ancItems" runat="server" href="javascript:;" class="dglink">Items</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblRaG" runat="server" EnableViewState="False"  Font-Bold="True"
                                            CssClass="clsLabel">Rooms and Guest :</asp:Label><br>
                            <asp:Literal ID="LitRooms" runat="server"></asp:Literal>
                            <asp:Label ID="lblNCuartos" runat="server" EnableViewState="False" CssClass="clsLabel">1</asp:Label>&nbsp;<asp:Label
                                ID="lblECuartos" runat="server" EnableViewState="False" CssClass="clsLabel">Cuartos,</asp:Label><asp:Label
                                    ID="lblNNoches" runat="server" EnableViewState="False" CssClass="clsLabel">5</asp:Label>&nbsp;<asp:Label
                                        ID="lblENoches" runat="server" EnableViewState="False" CssClass="clsLabel">Noches</asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" colspan="2">
                            <br>
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" colspan="4">
                                        <asp:Label ID="lblReservationData" runat="server" EnableViewState="False"  Font-Bold="True"
                                            CssClass="clsLabel">Reservation data</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width=17%>
                                        <asp:Label ID="lblEID" runat="server" EnableViewState="False" CssClass="MiddleText">Reservation ID :</asp:Label>
                                    </td>
                                    <td width=35%>
                                        <asp:Label ID="lblID" runat="server" EnableViewState="False"  Font-Bold="True" CssClass="clsLabel">1520124585</asp:Label>
                                    </td>
                                    <td align="right"  width=13%>
                                    <asp:Label ID="lblRecLoc" runat="server" EnableViewState="False" CssClass="MiddleText">RecLoc:</asp:Label>
                                        
                                    </td>
                                    <td width=35%>
                                        <asp:Label ID="lblreclocnumber" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">gds123</asp:Label>
                                        
                                    </td>
                                </tr>                                
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNoConf" runat="server" EnableViewState="False" CssClass="MiddleText">No. Confirmacion:</asp:Label>
                                    </td>
                                    <td>                                        
                                        <asp:Label ID="lblConfirmationNumber" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">123456</asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblETipoReservacion" runat="server" EnableViewState="False" CssClass="MiddleText">Tipo:</asp:Label>
                                    </td>
                                    <td>
                                        
                                            <asp:Label ID="lblTipoReservacion" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Commisionable</asp:Label>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEResCurrency" Visible="false" runat="server" EnableViewState="False"
                                            CssClass="MiddleText">Moneda Elegida al Reservar:</asp:Label>
                                    </td>
                                    <td>
                                
                                            <asp:Label ID="lblResCurrency" Visible="false" runat="server" EnableViewState="False"
                                                Font-Bold="True" CssClass="clsLabel">MXN</asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblEMoneyChange" Visible="false" runat="server" EnableViewState="False"
                                            CssClass="MiddleText">Tipo de Cambio:</asp:Label>
                                    </td>
                                    <td>
                                        
                                            <asp:Label ID="lblMoneyChange" Visible="false" runat="server" EnableViewState="False"
                                               Font-Bold="True" CssClass="clsLabel">1</asp:Label>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td align="right">
                                       <span runat=server id="lblConvenio" class="MiddleText"> <%=PortalCulture.GetString("M0BT0000195")%>:</span>
                                    </td>
                                    <td>
                                            <asp:Label ID="lblNoConvenio" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Commisionable</asp:Label>
                                    </td>
                                    <td align="right">
                                       <span runat=server id="lblConvenio2" class="MiddleText"> <%=PortalCulture.GetString("01220")%>:</span>
                                    </td>
                                    <td>                                        
                                            <asp:Label ID="lblEmpresaConvenio" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Commisionable</asp:Label>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCompanySegmentTitle" Visible="true" runat="server" EnableViewState="False"
                                            CssClass="MiddleText">Empresa de Segmento</asp:Label>
                                    </td>
                                    <td>                                
                                            <asp:Label ID="lblCompanySegment" Visible="true" runat="server" EnableViewState="False"
                                                Font-Bold="True" CssClass="clsLabel">1</asp:Label>
                                    </td>
                                     <td align="right">
                                        <asp:Label ID="lblSegmentTitle" Visible="true" runat="server" EnableViewState="False"
                                            CssClass="MiddleText">Segmento</asp:Label>
                                    </td>
                                    <td>                                        
                                            <asp:Label ID="lblSegment" Visible="true" runat="server" EnableViewState="False"
                                                Font-Bold="True" CssClass="clsLabel">1</asp:Label>
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                        <td valign="top">
                            <br>
                            <div id="DivRollAwayData" runat="server">
                                <table width="100%" align="center">
                                    <tr >
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblTitleRollAway" runat="server" EnableViewState="False"  Font-Bold="True"
                                            >Roll Away Data:</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRollAwayAdult" runat="server" EnableViewState="False" class="litletitle">Adult</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRollAwayChild" runat="server" EnableViewState="False" CssClass="litletitle">Child</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCrib" runat="server" EnableViewState="False" CssClass="litletitle">Crib</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblECantidad" runat="server" EnableViewState="False" CssClass="litletitle">Cantidad</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCantAdults" runat="server" EnableViewState="False" Font-Bold="True" CssClass="littletext">0</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCantChild" runat="server" EnableViewState="False" Font-Bold="True" CssClass="littletext">0</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCantCrib" runat="server" EnableViewState="False" Font-Bold="True" CssClass="littletext">0</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblPrice" runat="server" EnableViewState="False" CssClass="litletitle">Costo</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPriceAdult" runat="server" EnableViewState="False" Font-Bold="True" CssClass="littletext">$0.0</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPriceChild" runat="server" EnableViewState="False" Font-Bold="True" CssClass="littletext">$0.0</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPriceCrib" runat="server" EnableViewState="False" Font-Bold="True" CssClass="littletext">$0.0</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="5">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <table id="Table5" cellspacing="0" cellpadding="0" width="99%" border="0">
                                <tr>
                                    <td align="left" colspan="4">
                                        <asp:Label ID="lblCustomerData" runat="server" EnableViewState="False"  Font-Bold="True"
                                            CssClass="clsLabel">Customer Data</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width=17%>
                                        <asp:Label ID="lblEContacto" runat="server" EnableViewState="False" CssClass="MiddleText">Main Contact :</asp:Label>
                                    </td>
                                    <td align="left"  width=35%>
                                        <asp:Label ID="lblContacto" runat="server" EnableViewState="False" 
                                            Font-Bold="True" CssClass="clsLabel"> Contacto</asp:Label>
                                    </td>
                                     <td align="right" width=13%>
                                        <asp:Label ID="lblEAddress" runat="server" EnableViewState="False" CssClass="MiddleText">Address :</asp:Label>
                                    </td>
                                    <td width=35%>
                                        <asp:Label ID="lblAddress" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Callejón A</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEmail" runat="server" EnableViewState="False" CssClass="MiddleText">E-mail Address :</asp:Label>
                                    </td>
                                    <td style="width: 145px">
                                        <asp:Label ID="lblMail" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">E-mail</asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblECode" runat="server" EnableViewState="False" CssClass="MiddleText">Zip Code :</asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblZipCode" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">23000</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEPhoneH" runat="server" EnableViewState="False" CssClass="MiddleText">Home Phone :</asp:Label>
                                    </td>
                                    <td style="width: 145px">
                                        <asp:Label ID="lblPhoneH" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Phone Home</asp:Label>
                                    </td>
                                     <td align="right">
                                        <asp:Label ID="lblCiudad" runat="server" EnableViewState="False" CssClass="MiddleText">City :</asp:Label>
                                    </td>
                                    <td style="width: 145px">
                                        <asp:Label ID="lblCustCity" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">City A</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  align="right">
                                        <asp:Label ID="lblEPhoneW" runat="server" EnableViewState="False" CssClass="MiddleText">Work Phone :</asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblPhoneW" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Phone Work</asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblEstado" runat="server" EnableViewState="False" CssClass="MiddleText">County :</asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblCustCounty" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">County A</asp:Label>&nbsp;
                                        <asp:Label ID="lblCustCountry" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">Country A</asp:Label>
                                    </td>
                                </tr>                               
                            </table>
                            <div id="divClientportal" runat="server">
                                <span id="AgencyInfo" runat="server"></span><span id="AgencyRecepcion" runat="server">
                                </span>
                            </div>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlTC" runat="server">
                                <table id="Table3" cellspacing="0" cellpadding="0" width="300" border="0">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCCTitle" runat="server" EnableViewState="False"  Font-Bold="True"
                                            CssClass="clsLabel">Credit Card Data</asp:Label>
                                        </td>
                                        <td align=right><asp:LinkButton ID="lnkShowCC" runat="server">Mostrar datos</asp:LinkButton></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCCTipo" runat="server" EnableViewState="False" CssClass="MiddleText" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCCType" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCCNa" runat="server" EnableViewState="False" CssClass="MiddleText" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCCName" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCCNu" runat="server" EnableViewState="False" CssClass="MiddleText"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCCNumber" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCCcv" runat="server" EnableViewState="False" CssClass="MiddleText" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCCcvNumber" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCCExp" runat="server" CssClass="MiddleText" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCCExpDate" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                   
                    <tr>
                        <td colspan="2"></td>
                        <td>
                            <a id="aNRPolicies" name="NRPoliticies" runat="server" href="#NRPoliticies" class="showOptions"  style="display:none;">Details</a><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div id="dvNRPolicies" style="padding-right: 3px; display: none; padding-left: 3px;border:solid 1px #ccc;
                                padding-bottom: 3px; padding-top: 3px;-moz-border-radius: 7px; -webkit-border-radius: 7px;  -khtml-border-radius: 7px; border-radius: 7px;">
                                <table id="bookingcontainer" align="right" style="padding:0;">
                                    <tr>
                                        <td width="95%">
                                        </td>
                                        <td width="5%" align="right">
                                            <img align="right" runat="server" id="imgNRclose" src="~/Includes/imagenes/Close.ico"
                                                style="cursor: hand">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Literal ID="lblNRPolicies" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                        <td>
                            <a id="aPolices" runat="server" name="Politicie" href="#Politicie" class="showOptions" style="display:inline-block;">Details</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div id="divPolicies" style="padding-right: 3px; display: none; padding-left: 3px;border:solid 1px #ccc;
                                padding-bottom: 3px; padding-top: 3px;-moz-border-radius: 7px; -webkit-border-radius: 7px;  -khtml-border-radius: 7px; border-radius: 7px;" >
                                <table id="bookingcontainer" align="right" style="padding:0;">
                                    <tr>
                                        <td width="95%">
                                        </td>
                                        <td width="5%" align="right">
                                            <img align="right" runat="server" id="imgPoliceClose" src="~/Includes/imagenes/Close.ico"
                                                style="cursor: hand">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                           <table>
                                                <tr id="trCancelPol" runat="server">
                                                    <td colspan="3" ><p><b><asp:label ID="lblCancelPolTitle" runat="server">Cancelacion:</asp:label></b></p>
                                                        <p><asp:Label ID="lblCancelationPolices" runat="server"></asp:Label></p>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr id="trCardPol" runat="server">
                                                    <td colspan="3"><p><b><asp:label ID="lblCardPolTitle" runat="server">Tarjeta de credito:</asp:label></b></p>
                                                        <p><asp:Label ID="lblCreditCardPolices" runat="server"></asp:Label></p>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr id="trGuarPol" runat="server">
                                                    <td colspan="3"><p><b><asp:label ID="lblGuarPolTitle" runat="server">Garantias:</asp:label></b></p>
                                                        <p><asp:Label ID="lblGuaranteePolices" runat="server"></asp:Label></p>
                                                        <br />
                                                    </td>
                                                </tr>
                                           </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Label ID="lblCostos" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"
                                Font-Size="8pt">Cost and Travel Summary</asp:Label>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td runat="server" id="tdCostoHotel">
                                        <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td colspan="3" id="tdTextHotel" align="center" runat="server">
                                                    <b>HOTEL</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblECosto" runat="server" EnableViewState="False" CssClass="MiddleText"
                                                        Visible="False">Costo por noche</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCosto" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl1" runat="server" EnableViewState="False" CssClass="clslabel" Visible="false"> /noche</asp:Label>
                                                </td>
                                            </tr>                                           
                                           
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblETotal" runat="server" EnableViewState="False" CssClass="MiddleText">Total :</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTotal" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblMsgImpuesto" runat="server" EnableViewState="False" CssClass="MiddleText">Impuestos incluidos</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblEImpuesto" runat="server" EnableViewState="False" CssClass="MiddleText"
                                                        Visible="False">Impuestos</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblImpuestos" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False">$ X.00 </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl2" runat="server" EnableViewState="False" CssClass="clslabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblETotalH" runat="server" EnableViewState="False" CssClass="MiddleText">Total a cobrar al cliente :</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTotalH" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 10px">
                                    </td>
                                    <td runat="server" id="tdCostoUV">
                                        <table id="TableNR" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td colspan="3" id="tdTextUnivisit" align="center" runat="server">
                                                    <b>UNIVISIT</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblECostoUV" runat="server" EnableViewState="False" CssClass="MiddleText"
                                                        Visible="false">Costo por noche</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCostoUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl1UV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False"> /noche</asp:Label>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEComisionUV" runat="server" EnableViewState="False" CssClass="MiddleText"
                                                        Visible="False">Comision</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblComisionUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblETotalUV" runat="server" EnableViewState="False" CssClass="MiddleText">Total :</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTotalUV" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblMsgImpuestoUV" runat="server" EnableViewState="False" CssClass="MiddleText">Impuestos incluidos</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblEImpuestoUV" runat="server" EnableViewState="False" CssClass="MiddleText"
                                                        Visible="False">Impuestos</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblImpuestosUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False">$ X.00 </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl2UV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                        Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblETotalHUV" runat="server" EnableViewState="False" CssClass="MiddleText">Total a cobrar al cliente :</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTotalHUV" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">$ X.00</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3" style="padding-top: 12px;">
                            <asp:Label ID="Label1" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"
                                Font-Size="8pt" Visible="false"><%=PortalCulture.GetString("01226")%></asp:Label>
                            <asp:Literal ID="litDetalleTarifa" runat="server" EnableViewState="False" Visible="false"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblDeposito" runat="server" CssClass="dgpager">Label</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <input visible="false" class="button" id="btnCancel" style="" onclick="javascript:ShowConfirm();"
                                type="button" value="Cancel " name="btnCancel" runat="server">&nbsp;
                            
                            <asp:Button ID="btnStatusPMS" CssClass="button" runat="server" Text="Cambiar Status PMS"
                                Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblError" runat="server" CssClass="validators" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblErrorMotivo" runat="server" CssClass="validators" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div id="ConfirmCancel" style="display: none">
                                <table id="Table7" style="border-right: #cbdced 1px solid; border-top: #cbdced 1px solid;
                                    border-left: #cbdced 1px solid; border-bottom: #cbdced 1px solid" cellspacing="1"
                                    cellpadding="1" width="60%" align="center" bgcolor="#bcc9d8" border="0">
                                    <tr>
                                        <td align="center" bgcolor="#cbdced" colspan="3">
                                            <asp:Label ID="lblConfirmCancel" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel" ForeColor="SteelBlue" BackColor="Transparent">Confirm Cancellation</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: left">
                                            <asp:Label ID="lblMotivo" runat="server" EnableViewState="False" CssClass="clslabel">Motivo</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3" style="vertical-align: top;">
                                            <asp:TextBox ID="txtMotivo" runat="server" Width="600px" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblSure" runat="server" EnableViewState="False" CssClass="clslabel">Sure??</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="50%">
                                            <asp:LinkButton ID="hplCancelRes" runat="server" EnableViewState="False" CssClass="dgLink">Yes</asp:LinkButton>
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td width="50%">
                                            <asp:HyperLink ID="HyperLink2" runat="server" EnableViewState="False" CssClass="Link"
                                                NavigateUrl="javascript:HideConfirm();">No</asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>