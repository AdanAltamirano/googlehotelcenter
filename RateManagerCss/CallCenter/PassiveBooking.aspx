<%@ Page Language="vb" Inherits="RateManager.PaginaBase" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Import Namespace="WSHotelFacade" %>
<%@ Import Namespace="WSHotelCommon" %>
<%@ Import Namespace="System.Xml" %>

<%@ Import Namespace="Ratemanager" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Portal.Hotel.DataAccess" %>
<%@ Import Namespace="Portal.Hotel.Common.Data" %>
<%@ Import Namespace="Portal.Hotel.Facade" %>
<%@ Import namespace="Portal.General.Facade" %>
<%@ Import namespace="Portal.General.Common.Data" %>


<script runat="server">
    
    Private _Guid As String
    Private _Data As DataRow

    Protected ReadOnly Property Guid() As String
        Get
            If Me._Guid Is Nothing OrElse Me._Guid.Trim().Length > 0 Then
                Me._Guid = Me.Request.QueryString("guid")
            End If
            Return Me._Guid
        End Get
    End Property

    Protected ReadOnly Property Data() As DataRow
        Get
            If Me._Data Is Nothing Then
                Try
                    Dim table As DataTable = (New WaitListDataAccess()).GetItem(guid:=Me.Guid)
                    If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                        Me._Data = table.Rows(0)
                    End If
                Catch ex As Exception
                End Try
            End If
            Return If(Me._Data Is Nothing, (New DataTable()).NewRow(), Me._Data)
        End Get
    End Property

    Protected ReadOnly Property CheckIn() As Date
        Get
            Return If(Not Me.Data.Table.Columns.Contains("checkIn"), New Date(2000, 1, 1), Me.Data("checkIn"))
        End Get
    End Property

    Protected ReadOnly Property CheckOut() As Date
        Get
            Return If(Not Me.Data.Table.Columns.Contains("checkOut"), New Date(2000, 1, 1), Me.Data("checkOut"))
        End Get
    End Property

    Protected ReadOnly Property RateCode() As String
        Get
            'Return If(Not Me.Data.Table.Columns.Contains("rateCode"), String.Empty, Me.Data("rateCode"))
            Dim temp As String = String.Empty
            If me.Data.Table.Columns.Contains("roomCode") andalso not IsDBNull(Me.Data("roomCode")) andalso me.Data.Table.Columns.Contains("ratePlanCode") andalso not IsDBNull(Me.Data("ratePlanCode")) then
                temp = me.Data("roomCode") + Me.Data("ratePlanCode")
            end if                        
            Return temp
        End Get
    End Property    
    
    Protected ReadOnly Property Rooms() As Integer
        Get
            Return If(Not Me.Data.Table.Columns.Contains("rooms"), 0, Me.Data("rooms"))
        End Get
    End Property

    Protected ReadOnly Property Adults() As Integer
        Get
            Return If(Not Me.Data.Table.Columns.Contains("adults"), 0, Me.Data("adults"))
        End Get
    End Property

    Protected ReadOnly Property Children() As Integer
        Get
            Return If(Not Me.Data.Table.Columns.Contains("children"), 0, Me.Data("children"))
        End Get
    End Property

    Protected ReadOnly Property Total() As Double
        Get
            Return If(Not Me.Data.Table.Columns.Contains("notifyPrice"), 0.0, Me.Data("notifyPrice"))
        End Get
    End Property

    Protected ReadOnly Property Currency() As String
        Get
            Return If(Not Me.Data.Table.Columns.Contains("notifyCurrency"), "MXN", Me.Data("notifyCurrency"))
        End Get
    End Property
    
    Protected ReadOnly Property FirstName() As String
        Get
            Return If(Not Me.Data.Table.Columns.Contains("firstName"), String.Empty, Me.Data("firstName"))
        End Get
    End Property
    
    Protected ReadOnly Property LastName() As String
        Get
            Return If(Not Me.Data.Table.Columns.Contains("lastName"), String.Empty, Me.Data("lastName"))
        End Get
    End Property
    
    Protected ReadOnly Property Email() As String
        Get
            Return If(Not Me.Data.Table.Columns.Contains("email"), String.Empty, Me.Data("email"))
        End Get
    End Property
    
    Protected ReadOnly Property Phone() As String
        Get
            Return If(Not Me.Data.Table.Columns.Contains("phone"), String.Empty, Me.Data("phone"))
        End Get
    End Property
        
    Protected ReadOnly Property Portal() As String
        Get
            Return If(Not Me.Data.Table.Columns.Contains("portal") AndAlso Not Me.Data.IsNull("portal"), String.Empty, Me.Data("portal"))
        End Get
    End Property

    Protected Function GetAdultsForRoom(ByVal noRoom As Integer) As Integer
        Return ((Me.Adults \ Me.Rooms) + If(noRoom <= Me.Adults Mod Me.Rooms, 1, 0))
    End Function

    Protected Function GetChildrenForRoom(ByVal noRoom As Integer) As Integer
        Return ((Me.Children \ Me.Rooms) + If(noRoom <= Me.Children Mod Me.Rooms, 1, 0))
    End Function
    
    Protected Sub Book(ByVal sender As Object, ByVal e As EventArgs) Handles btnReservar.Click
        
        If Me.IsValid Then
                                    
            Dim request As New BookingIDSRQ_1_0()
            
            Dim payment As Integer = 0
            Select Case Me.txtSelectedPayment.Value.ToLower()
                Case "creditcard"
                    payment = 1
                Case "bankdeposit"
                    payment = 2
                Case "guaranteed"
                    payment = 3
            End Select

            'Poblamos la peticion de reservación
            Dim mainRoot As BookingIDSRQ_1_0.ReservationsRow = request.Reservations.AddReservationsRow()
            'Dim root As BookingIDSRQ_1_0.ReservationRow = request.Reservation.AddReservationRow(String.Empty, "SS", String.Empty, ConfigurationManager.AppSettings("PasiveBooking_ChanelId"), ConfigurationManager.AppSettings("PasiveBooking_SourceChanel"), String.Empty, String.Empty, mainRoot, 1)
            Dim root As BookingIDSRQ_1_0.ReservationRow = request.Reservation.AddReservationRow(String.Empty, "SS", String.Empty, String.Empty, "POR", String.Empty, String.Empty, mainRoot, 1, 1, 1, PortalCulture.GetCulture().ToString(), If(payment = 2, "1", "0"), If(payment = 3, "1", "0"), Me.Portal, "WL")
            'request.Hotel.AddHotelRow(Me.Data("idHotel"), root, ConfigurationManager.AppSettings("PasiveBooking_User"), ConfigurationManager.AppSettings("PasiveBooking_Password"))
            request.Hotel.AddHotelRow(Me.HotelInfo(HotelDatos.FIELD_IDEMPRESA), root, ConfigurationManager.AppSettings("wsConectividadUser"), ConfigurationManager.AppSettings("wsConectividadPassword"))
            

            Dim priceIncludedTaxes As Boolean = not Convert.ToBoolean(Me.HotelInfo(HotelDatos.FIELD_PLUSTAX))
            Dim taxes As Double = Convert.ToDouble(Me.HotelInfo(HotelDatos.FIELD_IMPUESTO))

            Dim total As Double = Me.Total
            'Dim taxes As Double = Me.HotelInfo(HotelDatos.FIELD_PLUSTAX)

            request.Total.AddTotalRow(total.ToString("F"), taxes.ToString("F"), Me.Currency, root)

            Dim rootRoomStays As BookingIDSRQ_1_0.RoomStaysRow = request.RoomStays.AddRoomStaysRow(root)

            Dim adultsPerRoom As Integer = Me.Adults \ Me.Rooms
            Dim childrenPerRoom As Integer = Me.Children \ Me.Rooms
            Dim extraAdults As Integer = Me.Adults Mod Me.Rooms
            Dim extraChildren As Integer = Me.Children Mod Me.Rooms

            Dim rootRoomStay As BookingIDSRQ_1_0.RoomStayRow
            Dim maxRooms As Integer = Me.Adults
            
            Dim roomCode As String = Me.Data("roomCode")
            Dim rateCode As String = Me.Data("ratePlanCode")
            If roomCode.Trim().Length = 0 then
                roomCode = Me.lstRooms.Items(me.lstRooms.SelectedIndex).Value
                rateCode = Me.lstRates.Items(me.lstRates.SelectedIndex).Value
            end if
            

            'Empezamos a llenar las habitaciones
            For i As Integer = 1 To Me.Rooms Step 1
                rootRoomStay = request.RoomStay.AddRoomStayRow(roomCode, rateCode, Me.txtNombre.Text.Trim() + " " + Me.txtApellido.Text.Trim(), String.Empty, rootRoomStays)
                request.StayDate.AddStayDateRow(Me.CheckIn.ToString("yyyyMMdd"), Me.CheckOut.ToString("yyyyMMdd"), rootRoomStay)
                request.GuestCount.AddGuestCountRow(If(extraAdults > 0, (adultsPerRoom + 1).ToString(), adultsPerRoom.ToString()), If(extraChildren > 0, (childrenPerRoom + 1).ToString(), childrenPerRoom.ToString()), rootRoomStay, String.Empty)
                extraAdults -= 1
                extraChildren -= 1
                Dim rootRates As BookingIDSRQ_1_0.RatesRow = request.Rates.AddRatesRow(Me.Currency, rootRoomStay)
                request.DayRate.AddDayRateRow(Me.CheckIn.ToString("yyyyMMdd"), Me.CheckOut.ToString("yyyyMMdd"), ((total / Me.Rooms) / (Me.CheckOut - Me.CheckIn).Days).ToString("F"), "0.00", rootRates)
                maxRooms -= 1
            Next

            Dim rootGuest As BookingIDSRQ_1_0.PrimaryGuestRow = request.PrimaryGuest.AddPrimaryGuestRow(root, String.Empty)
            request.Name.AddNameRow(Me.txtNombre.Text.Trim(), Me.txtApellido.Text.Trim(), rootGuest, Me.txtEmail.Text.Trim())
            request.Phone.AddPhoneRow(Me.txtHomePhone.Text.Trim(), rootGuest)

            If payment = 1 Then
                Me.txtYear.Text = Me.txtYear.Text.Trim()
                If Me.txtYear.Text.Length > 2 then Me.txtYear.Text = Me.txtYear.Text.Remove(0, Me.txtYear.Text.Length - 2)
                Dim rootCard As BookingIDSRQ_1_0.PaymentCardRow = request.PaymentCard.AddPaymentCardRow(convert.ToInt16(Me.ddlTarjetas.Items(Me.ddlTarjetas.SelectedIndex).Value), Me.txtMonth.Text.Trim() + Me.txtYear.Text.Trim(), Me.txtNumber.Text.Trim(), Me.txtVerify.Text.Trim(), root)
                request.CardHolder_.AddCardHolder_Row(Me.txtHolder.Text.Trim(), String.Empty, String.Empty, String.Empty, String.Empty, rootCard)
            End If
            
            Try
                Dim service As New WsConectividad.wsConnect()
                Dim xml As String = request.GetXml()
                xml = service.SubmitXML(xml)
            
                Dim response As New BookingIDSRS_1_0()
            
                Dim result As BookingIDSRS_1_0.ReservationRow = response.Reservation.NewReservationRow()
                Try
                    response.ReadXml(New System.IO.StringReader(xml), XmlReadMode.Auto)
                Catch ex As Exception
                    Throw New Exception("Error al leer el esquema xml de la reservacion.", ex)
                End Try
                If response._Error.Rows.Count > 0 AndAlso response._Error(0).code <> 0 Then
                    Throw New Exception(response._Error(0).Description)
                Else
                    If response.Reservation.Rows.Count > 0 Then
                        result = response.Reservation(0)
                        If result.ConfirmNumber.ToString().Trim().Length = 0 Then Throw New Exception("La reservacion no se llevo acabo correctamente.")
                    End If
                End If
            
                Dim controller As New WaitListDataAccess()
                
                controller.SetNotificationUsed(Me.Data("id"), response.Reservation(0).ConfirmNumber.ToString())
                                              
                
                Me.Response.Redirect("~/HotelAdministrator/Pages/ReservationDetails.aspx?qs=" + response.Reservation(0).idreservacion, False)
                
            Catch ex As Exception
                Me.ShowMessageError(ex)
            End Try
            
        End If
                
    End Sub
    
    Protected Overloads ReadOnly Property IsValid() As Boolean
        Get
            Dim flag As Boolean = (Me.txtNombre.Text.Trim.Length > 0 AndAlso Me.txtApellido.Text.Trim().Length > 0 AndAlso Me.txtHomePhone.Text.Trim().Length > 0 AndAlso Me.txtEmail.Text.Trim().Length > 0)
            
            If flag AndAlso Me.txtSelectedPayment.Value.ToLower() = "creditcard" Then
                Try
                    flag = ((New Tarjetas(Me.txtNumber.Text.Trim(), Me.ddlTarjetas.Items(Me.ddlTarjetas.SelectedIndex).Value).IsValid))
                Catch ex As Exception
                    flag = False
                End Try
            End If
            
            Return flag
        End Get
    End Property
    
    Protected ReadOnly Property IsValidRequest() As Boolean
        Get
            Dim flag As Boolean = False
            
            If Me.Guid.Trim().Length > 0 Then
                If Me.Data.Table.Columns.Contains("id") Then
                    flag = True
                End If
            End If
            
            Return flag
        End Get
    End Property
    
    Private Sub ShowMessageError(ByVal message As String)
        Me.boxInfo.Visible = Not (message.Trim.Length > 0)
        Me.boxMessage.Visible = (message.Trim.Length > 0)
        Me.lblMessage.InnerHtml = "<p>" + message + "</p>"
    End Sub
    
    Private Sub ShowMessageError(ByVal e As Exception)
        Dim message As String = String.Empty
        
        While e IsNot Nothing
            message += e.Message + "<br/>"
            e = e.InnerException
        End While
        
        Me.ShowMessageError(message)
    End Sub

    
    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack Then
            Dim idAsoc As Integer = Me.GetIdAsociation
            
            'Veririficar que tenga informacion correcta
            If Not Me.IsValidRequest Then
                Me.ShowMessageError("Error al cargarla información.")
            Else
                Me.ShowMessageError(String.Empty)
                If Me.RateCode.Trim().Length = 0 Then
                    With New RatePlanFacade
                        Dim data As RatePlanData = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
                        data.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView.Sort = "[" + RatePlanData.FIELD_CODIGOTARIFA + "] ASC"
                        Me.lstRates.DataTextField = RatePlanData.FIELD_CODIGOTARIFA
                        Me.lstRates.DataValueField = RatePlanData.FIELD_IDRATEPLAN
                        Me.lstRates.DataSource = data.Tables(RatePlanData.RATEPLAN_TABLE).DefaultView.Table
                        
                        Me.lstRates.DataBind()
                    End With
                    With New RoomFacade
                        Dim data As RoomsHotelData
                        data = .getRooms(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture)
                        data.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "SUBSTRING(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '-' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ", 1, 50)")
                        Me.lstRooms.DataSource = data.Tables(RoomsHotelData.TBL_ROOM_HOTEL)
                        Me.lstRooms.DataTextField = "texto"
                        Me.lstRooms.DataValueField = RoomsHotelData.FLD_ID_ROOM_HOTEL
                        Me.lstRooms.DataBind()
                    End With
                End If
            End If
            
        End If
    End Sub
    
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect(Me.GeRequestApplicationPath("/HotelAdministrator/Pages/WaitList.aspx"), False)
    End Sub
    
</script>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Polities</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
        <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
        <%
            Dim sysCulture As System.Globalization.CultureInfo
            sysCulture = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString())
        %>
		<form id="Form1" method="post" runat="server">
		    <div id="boxInfo" visible="False" runat="server">
		        <table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
		            <tr>
		                <td class="dgitem" style="height: 12px;" colSpan="6"><b><%=PortalCulture.GetString("00352")%></b></td>
		            </tr>
		            <tr>
		                <td align="right"><%=PortalCulture.GetString("M000078", True)%></td>
		                <td colSpan="2"><b><%=Me.CheckIn.ToString("MMM/dd/yyyy")%></b></td>	
		                <td align="right"><%=PortalCulture.GetString("M000079", True)%></td>
		                <td colSpan="2"><b><%=Me.CheckOut.ToString("MMM/dd/yyyy")%></b></td>					    
		            </tr>
		            <tr>
		                <td colspan="3"></td>
		                <td align="center"><%=PortalCulture.GetString("M000645")%></td>
		                <td align="center"><%=PortalCulture.GetString("M000646")%></td>
		                <td>&nbsp;</td>
		            </tr>
		            <% For i As Integer = 1 To Me.Rooms Step 1%>
		            <tr>
		                <%If i = 1 Then%>
		                    <td valign="top" align="right" rowspan="<%= Me.Rooms.ToString() %>" ><%=PortalCulture.GetString("A00041", True)%></td>
		                <%End If%>
		                <td align="right" colspan="2"><b><%=PortalCulture.GetString("M000439") + " " + i.ToString()%></b></td>
		                <td align="center"><%=Me.GetAdultsForRoom(i).ToString()%></td>
		                <td align="center"><%=Me.GetChildrenForRoom(i).ToString()%></td>
		                <%If i = 1 Then%>
		                    <td rowspan="<%= Me.Rooms.ToString() %>">&nbsp;</td>
		                <%End If%>
		            </tr>
		            <% Next i %>			    
		            <tr> 
		                <td align="right" colSpan="2"><%=PortalCulture.GetString("00006", True)%></td>
		                <td colspan="4">     
		                <%If Me.RateCode.Trim().Length > 0 then %>
		                    <b><%=Me.RateCode%></b>	
		                <%Else%>
                            <asp:DropDownList ID="lstRooms" runat="server"></asp:DropDownList>
                            &nbsp;
                            <asp:DropDownList ID="lstRates" runat="server"></asp:DropDownList>		                
		                <%End If %>
		                </td>
		            </tr>
		            <tr>
		                <td class="dgitem" style=""height: 12px;" colSpan="6"><%=PortalCulture.GetString("00366")%></td>
		            </tr>
		            <tr>
		                <td>&nbsp;</td>
		                <td align="center"><b><%=PortalCulture.GetString("M000645")%></b></td>
		                <td align="center"><b><%=PortalCulture.GetString("M000646")%></b></td>
		                <td align="center"><b><%=PortalCulture.GetString("00428")%></b></td>
		                <td align="center"><b><%=PortalCulture.GetString("00429")%></b></td>
		                <td>&nbsp;</td>
		            </tr>
		            <% For i As Integer = 1 To Me.Rooms Step 1%>
		            <tr>
		                <td align="right"><%=String.Format("{0} Ad. {1} Chd", Me.GetAdultsForRoom(i), Me.GetChildrenForRoom(i))%></td>	
		                <td align="center"><%=(Me.Total / Me.Rooms).ToString("0.00")%></td>
		                <td align="center">0.00</td>
		                <td align="center">0.00</td>
		                <td align="center">0.00</td>
		                <td align="right"><%=(Me.CheckOut - Me.CheckIn).Days.ToString() + " " + PortalCulture.GetString("00374")%></td>		        
		            </tr>
		            <% Next i %>	
		            <tr>
		                <td align="right" colspan="3"><%=PortalCulture.GetString("00377")%></td>
		                <td colspan="3"><%=Me.Total.ToString("###,##0.00") + " " + Me.Currency%></td>
		            </tr>	
		            <tr>
		                <td align="right" colspan="3"><%=PortalCulture.GetString("00378")%></td>
		                <td colspan="3"><%="0.00 " + Me.Currency%></td>
		            </tr>	
		            <tr>
		                <td align="right" colspan="3"><%=PortalCulture.GetString("00379")%></td>
		                <td colspan="3"><%=PortalCulture.GetString("00433")%></td>
		            </tr>	
		            <tr>
		                <td align="right" colspan="3"><b><%=PortalCulture.GetString("00380")%></b></td>
		                <td colspan="3"><%=Me.Total.ToString("###,##0.00") + " " + Me.Currency%></td>
		            </tr>
		            <tr>
		                <td class="dgitem" colSpan="6"><b><%=PortalCulture.GetString("00354")%></b></td>
		            </tr>
		            <tr>
		                <td colSpan="2" align="right"><%="*" & PortalCulture.GetString("00432", True)%></td>
		                <td colSpan="4">
		                    <p>
		                        <% Me.txtNombre.Text = Me.FirstName%>
		                        <asp:textbox id="txtNombre" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:textbox>
						        <asp:requiredfieldvalidator id="rfvNombre" runat="server" CssClass="Validators" Display="Dynamic" ErrorMessage="*"
						            ControlToValidate="txtNombre"></asp:requiredfieldvalidator>
					        </p>
		                </td>
		            </tr>
		            <tr>
		                <td colSpan="2" align="right"><%="*" & PortalCulture.GetString("00355", True)%></td>
		                <td colSpan="4">
		                    <p>
		                        <% Me.txtApellido.Text = Me.LastName%>
		                        <asp:textbox id="txtApellido" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:textbox>
						        <asp:requiredfieldvalidator id="rfvApellido" runat="server" CssClass="Validators" Display="Dynamic" ErrorMessage="*"
							        ControlToValidate="txtApellido"></asp:requiredfieldvalidator>
					        </p>
		                </td>
		            </tr>
		            <tr>
		                <td colSpan="2" align="right"><%="*" & PortalCulture.GetString("M000328")%></td>
		                <td colSpan="4">
		                    <% Me.txtHomePhone.Text = Me.Phone%>
	                        <asp:textbox id="txtHomePhone" runat="server" CssClass="textbox" Columns="30" MaxLength="28"></asp:textbox>
	                        <asp:requiredfieldvalidator id="RfvWorkPhone" runat="server" CssClass="Validators" ControlToValidate="txtHomePhone"
					            ErrorMessage="*" Display="Dynamic"></asp:requiredfieldvalidator>
		                </td>
		            </tr>
		            <tr>
		                <td colSpan="2" align="right"><%=PortalCulture.GetString("M000329")%></td>
		                <td colSpan="4">
		                    <asp:textbox id="txtWorkHome" runat="server" CssClass="textbox" Columns="30" MaxLength="28"></asp:textbox>
		                </td>
		            </tr>
		            <tr>
		                <td colSpan="2" align="right"><%=PortalCulture.GetString("M000076", True)%></td>
		                <td colSpan="4">
		                    <asp:textbox id="txtAddress" runat="server" CssClass="textbox" Columns="40" MaxLength="50"></asp:textbox>
		                </td>
		            </tr>
		            <tr>
		                <td colSpan="2" align="right"><%="*" & PortalCulture.GetString("M000327")%></td>
		                <td colSpan="4">
		                    <p>
		                        <% Me.txtEmail.Text = Me.Email%>
		                        <asp:textbox id="txtEmail" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:textbox>
		                        <asp:requiredfieldvalidator id="rfvMail" runat="server" CssClass="Validators" ControlToValidate="txtEmail" ErrorMessage="*"
						            Display="Dynamic"></asp:requiredfieldvalidator>
						        <asp:regularexpressionvalidator id="REMail" runat="server" CssClass="Validators" ControlToValidate="txtEmail" 
						            Display="Dynamic" ValidationExpression="^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$"><%=PortalCulture.GetString("00356")%></asp:regularexpressionvalidator>
					        </p>
		                </td>
		            </tr>
		            <!--<tr> PORTAL
		                <td><%=PortalCulture.GetString("00755")%></td>
		                <td colSpan="3">
		                    <asp:dropdownlist id="ddlPortal" runat="server" Width="272px"></asp:dropdownlist>
		                </td>
		            </tr>-->
		            <tr>
				        <td class="dgitem" colSpan="6"><%=PortalCulture.GetString("M0BT0000202")%></td>
			        </tr>
			        <tr>			            
			            <td valign="top" colspan="2" id="pnlPaymentOptions">
			                <input id="txtSelectedPayment" runat="server" type="hidden"/>
			                <input type="radio" name="PaymentType" id="chkCreditCard"/><%=PortalCulture.GetString("01247")%><br />
			                <%---<input type="radio" name="PaymentType" id="chkBankDeposit"/><%=PortalCulture.GetString("01248")%><br />
			                <input type="radio" name="PaymentType" id="chkGuaranteed"/><%=PortalCulture.GetString("00027")%>--%>
			            </td>
			            <td colspan="6" id="pnlPaymentInfo">
			                <div id="pnlCreditCard">
			                    <script type="text/javascript">
			                        var valCreditCard = new Array('<%= RfvCardNumber.ClientId %>', '<%= RevCardNumber.ClientId %>', '<%= RfvNumberVer.ClientId %>', '<%= Requiredfieldvalidator1.ClientId %>', '<%= RfvMonth.ClientId %>', '<%= RVMonth.ClientId %>', '<%= RfvYear.ClientId %>', '<%= REVYear.ClientId %>');
			                    </script>
                                <table width="100%" cellspacing="0" cellpadding="2">
                                    <tr>
		                                <td style="width:30%;" align="right"><%="*" & PortalCulture.GetString("00362", True)%></td>
		                                <td>
		                                    <asp:dropdownlist id="ddlTarjetas" runat="server">
						                        <asp:ListItem Value="-1">-- Select credit card --</asp:ListItem>
						                        <asp:ListItem Value="5">Master Card</asp:ListItem>
						                        <asp:ListItem Value="4">Visa</asp:ListItem>
						                        <asp:ListItem Value="1">American Express</asp:ListItem>
					                        </asp:dropdownlist>
		                                </td>
		                            </tr>
		                            <tr>
		                                <td align="right"><%="*" & PortalCulture.GetString("M000504")%></td>
		                                <td >
		                                    <asp:textbox id="txtNumber" runat="server" CssClass="textbox" Columns="16" MaxLength="16"></asp:textbox>
		                                    <asp:requiredfieldvalidator id="RfvCardNumber" runat="server" CssClass="Validators" ControlToValidate="txtNumber"
					                            ErrorMessage="*" Display="Dynamic">
					                        </asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RevCardNumber" runat="server" CssClass="Validators" ControlToValidate="txtNumber"
					                            Display="Dynamic" ValidationExpression="(^\d{15,16})|(^\d{13})"><%=PortalCulture.GetString("00358")%></asp:regularexpressionvalidator>
		                                </td>
		                            </tr>
		                            <tr>
		                                <td align="right"><%="*" & PortalCulture.GetString("M000505")%></td>
		                                <td >
		                                    <asp:textbox id="txtVerify" runat="server" CssClass="textbox" Columns="4" MaxLength="4"></asp:textbox>
		                                    <asp:requiredfieldvalidator id="RfvNumberVer" runat="server" CssClass="Validators" ControlToValidate="txtVerify"
					                            ErrorMessage="*" Display="Dynamic"></asp:requiredfieldvalidator>
		                                </td>
		                            </tr>
		                            <tr>
		                                <td align="right"><%="*" & "Holder :"%></td>
		                                <td>
		                                    <asp:textbox id="txtHolder" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:textbox>
		                                    <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="Validators" ControlToValidate="txtHolder"
                                                ErrorMessage="*" Display="Dynamic"></asp:requiredfieldvalidator>
		                                </td>
		                            </tr>
		                            <tr>
		                                <td align="right"><%="*" & PortalCulture.GetString("00431")%></td>
		                                <td>
		                                    <asp:textbox id="txtMonth" runat="server" CssClass="textbox" Columns="2" MaxLength="2"></asp:textbox>&nbsp;/&nbsp;
		                                    <asp:textbox id="txtYear" runat="server" CssClass="textbox" Columns="4" MaxLength="4"></asp:textbox>
		                                    <asp:requiredfieldvalidator id="RfvMonth" runat="server" CssClass="Validators" ControlToValidate="txtMonth"
					                            ErrorMessage="*" Display="Dynamic"></asp:requiredfieldvalidator>
					                        <asp:rangevalidator id="RVMonth" runat="server" CssClass="Validators" ControlToValidate="txtMonth"
					                            Display="Dynamic" MinimumValue="01" MaximumValue="12"><%=PortalCulture.GetString("00361")%></asp:rangevalidator>
		                                    <asp:requiredfieldvalidator id="RfvYear" runat="server" CssClass="Validators" ControlToValidate="txtYear" ErrorMessage="*"
					                            Display="Dynamic"></asp:requiredfieldvalidator>
					                        <asp:label id="lblExpirationError" runat="server" CssClass="Validators" Visible="False">Invalid Expiration Date</asp:label>
					                        <asp:regularexpressionvalidator id="REVYear" runat="server" CssClass="Validators" ControlToValidate="txtYear"
					                            Display="Dynamic" ValidationExpression="^\d{4}"><%=PortalCulture.GetString("00360")%></asp:regularexpressionvalidator>
		                                </td>
		                            </tr>
                                </table>			                
			                </div>
			                <div id="pnlBankDeposit"></div>
			                <div id="pnlGuaranteed"></div>
			            </td>
			        </tr>
			        <tr>
			            <td align="center" colspan="6">
			                <%
			                    Me.btnReservar.Text = PortalCulture.GetString("00347")
			                    Me.btnCancel.Text = PortalCulture.GetString("00413")
			                %>
			                <asp:button id="btnReservar" runat="server" CssClass="button" EnableViewState="False" Text="Reservar"></asp:button>
			                <asp:button id="btnCancel" runat="server" CssClass="button" EnableViewState="False" Text="Cancelar" CausesValidation="False" OnClick="btnCancel_Click"></asp:button>
			            </td>
			        </tr>	
        		    
		        </table>
		        <script type="text/javascript">
		            $('#pnlPaymentOptions input:radio').each(
		                function() {
		                    $(this).click(
		                        function() {
		                            $("#pnlPaymentInfo > div[id^='pnl']").each(
		                                function() {
		                                    $(this).hide();
		                                    $(this).find('.Validators').each(
		                                        function() {
		                                            ValidatorEnable(this, false);
		                                        }
		                                    );
		                                }
		                            );
		                            try {
		                                $('#<%= me.txtSelectedPayment.clientID %>').val($(this).attr('id').replace('chk', ''));
		                                var panel = $('#' + $(this).attr('id').replace('chk', 'pnl'));
		                                panel.show();
		                                panel.find('.Validators').each(
		                                        function() {
		                                            ValidatorEnable(this, true);
		                                            $(this).hide();
		                                        }
		                                    );
		                            } catch (ex) {
		                            }
		                        }
		                    );
		                }
		            );

		            $(document).ready(
	                    function() {
		                    $("#pnlPaymentOptions input[type='radio']:first").click();
	                    }
	                );
		            
		        </script>
			</div>
			<div id="boxMessage" visible="True" runat="server">
			    <span id="lblMessage" runat="server" class="Validators">Ocurrio un error</span>
			</div>
		</form>
        <% System.Threading.Thread.CurrentThread.CurrentCulture = sysCulture%>
	</body>
</HTML>
