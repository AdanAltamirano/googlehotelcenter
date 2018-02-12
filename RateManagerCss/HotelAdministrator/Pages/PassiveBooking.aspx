<%@ Import NameSpace = "RateManager"%>
<%@ Register TagPrefix="uc1" TagName="CtlReservaMsj" Src="../../Modulos/CtlReservaMsj.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PassiveBooking.aspx.vb" Inherits="RateManager.PassiveBooking" validateRequest="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PassiveBooking</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body>
        <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
        </iframe>
		<script>

		   var classRooms = function() {
		        config = { sepChar: '_', sepCharC: ',' };

		        var p = {};
		        p = {
		            init: function() {
		            },
		            removeAllOptions: function(selectbox) {
		                var i;
		                for (i = selectbox.options.length - 1; i >= 0; i--) {
		                    selectbox.remove(i);
		                }
		            },
		            addOption: function(selectbox, value, text) {
		                var optn = document.createElement("OPTION");
		                optn.text = text;
		                optn.value = value;
		                selectbox.options.add(optn);
		            },
		            ChangeMaxRoomType: function() {
		                var idRoom = document.getElementById("ddlRoomtype");
		                var idRoomtp = document.getElementById("ddlRoomtypePersonas");
		                var idRooms = document.getElementById("cmbRooms");
		                var iMaxAdults = 4;
		                var iMaxChilds = 4;
		                if (idRoom.length > 0) {
		                    iMaxAdults = idRoomtp.options[idRoom.selectedIndex].text;
		                    iMaxChilds = idRoomtp.options[idRoom.selectedIndex].value;
		                }

		                for (var i = 1; i <= idRooms.length; i++) {
		                    var cboTmp = eval('document.getElementById("dlAduts"+i)');
		                    if (cboTmp) {
		                        this.removeAllOptions(cboTmp);
		                        for (var j = 1; j <= iMaxAdults; j++) {
		                            this.addOption(cboTmp, j, j);
		                        }
		                        if (iMaxAdults == 0) this.addOption(cboTmp, '0', '0');
		                    }
		                    var cboTmp = eval('document.getElementById("dlChild"+i)');
		                    if (cboTmp) {
		                        this.removeAllOptions(cboTmp);
		                        for (var j = 1; j <= iMaxChilds; j++) {
		                            this.addOption(cboTmp, j, j);
		                        }
		                        if (iMaxChilds == 0) this.addOption(cboTmp, '0', '0');
		                    }
		                }

		            }
		        }
		        return p;
		    } ();

		    function ChangeMaxRoomType() {
		        //ChangeMaxRoomTypef();
		    }


		    function ChangeRooms(Rooms, DivName) {
		        var e = document.getElementById(Rooms, DivName);
		        for (var i = 1; i <= 9; i++) {
		            var dv = document.getElementById(DivName + i);
		            if (i <= e.selectedIndex + 1)
		            { dv.style.display = ''; }
		            else { dv.style.display = 'none'; }

		        }
		    }

		    function SelectRoom(Room, DivName, DataName) {
		        var dv = document.getElementById(DivName + Room);
		        for (var i = 1; i <= 9; i++) {
		            var dv = document.getElementById(DivName + i);
		            if (i == Room) {
		                dv.className = 'TabSelected';
		                document.getElementById(DataName + i).style.display = '';
		            }
		            else {
		                document.getElementById(DataName + i).style.display = 'none';
		                dv.className = 'Tab';
		            }

		        }

		    }

		    function IniDate() {
		        var fecha = new Date();
		        var fecha2 = new Date(2030, 12, 31);
		        var arr = new Array(3);
		        arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
		        arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
		        return arr;

		    }
		    
		    function cancel(pnl1, pnl2) {
		        document.getElementById(pnl1).style.display = 'none';
		        document.getElementById(pnl2).style.display = 'none';
		        var arrSelects = document.getElementsByTagName('SELECT');
		        for (var i = 0; i < arrSelects.length; i++)
		        { arrSelects[i].style.display = ''; }

		    }

		    function ok(pnl1, pnl2, Rooms, clntId, msg) {

		        var txtC = document.getElementById('txtIdObj');
		        var sw = 0;
		        var r = document.getElementById(Rooms);

		        for (var i = 1; i <= eval(r.value) && sw == 0; i++) {
		            var txt = document.getElementById(clntId + "_txtPriceRoom" + i);
		            if (eval(txt.value) < 1) { sw = 1; SelectRoom(i, 'CtrldivRoom', 'CtrlDataRoom'); alert(msg + i); }
		        }

		        if (sw == 0) { document.getElementById(txtC.value).click(); }
		    }

		    function show(pnl1, pnl2, obj) {

		        document.getElementById(pnl1).style.display = "";
		        document.getElementById(pnl2).style.display = "";
		        var arrSelects = document.getElementsByTagName('SELECT');
		        for (var i = 0; i < arrSelects.length; i++)
		        { arrSelects[i].style.display = 'None'; }
		        var txt = document.getElementById('txtIdObj');
		        txt.value = obj;
		    }
		</script>
		<form id="Form1" method="post" runat="server">
			<input id="iReq" type="hidden" runat="server"> <input id="iRes" type="hidden" runat="server">
			<input id="txtIdObj" type="hidden" name="txtIdObj">
			
			<div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitle" runat="server" EnableViewState="False"  Text="Información de la reservación" CssClass=tituloSeccion ></asp:label> 
		        </div>
		    </div>  
			<table id="bookingcontainer" cellSpacing="2" cellPadding="0" width="650" border="0">				
				<tr>
					<td>
						<h3 id="lblECheckIn" align="right" runat="server">CheckIn:</h3>
					</td>
					<td><asp:textbox id="txtCheckIn" Columns="10" MaxLength="10" Runat="server"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtCheckIn'),IniDate());return false;"
							href="javascript:void(0)"><IMG class=PopcalTrigger alt="" 
      src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align=absMiddle 
      border=0></A>
						<asp:customvalidator id="cvDates" runat="server" Display="Dynamic" CssClass="validators" ErrorMessage="Invalid dates"></asp:customvalidator></td>
					<td>
						<h3 id="lblRoomType" align="right" runat="server">Room:</h3>
					</td>
					<td><asp:dropdownlist id="ddlRoomtype" runat="server" AutoPostBack="True">
							<asp:ListItem>A1K</asp:ListItem>
						</asp:dropdownlist><asp:dropdownlist id="ddlRoomtypePersonas" runat="server" style="display:none">
							<asp:ListItem>A1K</asp:ListItem>
						</asp:dropdownlist><asp:dropdownlist id="ddlRate" runat="server">
							<asp:ListItem>RAC</asp:ListItem>
						</asp:dropdownlist></td>
				</tr>
				<tr>
					<td>
						<h3 id="lblECheckOut" align="right" runat="server">CheckOut:</h3>
					</td>
					<td><asp:textbox id="txtCheckOut" Columns="10" MaxLength="10" Runat="server"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtCheckOut'),IniDate());return false;"
							href="javascript:void(0)"><IMG class=PopcalTrigger alt="" 
      src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align=absMiddle 
      border=0></A></td>
					<TD vAlign="top" align="center">
						<h3 id="lblRooms" align="right" runat="server">Rooms:</h3>
					</TD>
					<TD vAlign="top" align="left"><asp:dropdownlist id="cmbRooms" runat="server" >
							<asp:ListItem>1</asp:ListItem>
							<asp:ListItem>2</asp:ListItem>
							<asp:ListItem>3</asp:ListItem>
							<asp:ListItem>4</asp:ListItem>
							<asp:ListItem>5</asp:ListItem>
							<asp:ListItem>6</asp:ListItem>
							<asp:ListItem>7</asp:ListItem>
							<asp:ListItem>8</asp:ListItem>
							<asp:ListItem>9</asp:ListItem>
						</asp:dropdownlist></TD>
				</tr>
				<tr>
					<td class="titulo" colSpan="4">
						<h2 id="lblRoomInfo" runat="server">Información de Habitación(es)</h2>
					</td>
				</tr>
				<tr>
					<td colSpan="4">
						<table cellSpacing="2" cellPadding="2" width="99%" border="0">
							<tr>
								<td width="5%"><%response.write(portalculture.getstring("00170",true))%></td>
								<td id="divRoom1" style="CURSOR: pointer" onclick="SelectRoom(1,'divRoom','DataRoom');"
									runat="server">
									<div class="TabSelected" style="DISPLAY: inline; WIDTH: 100%">Room1</div>
								</td>
								<td id="divRoom2" style="CURSOR: pointer" onclick="SelectRoom(2,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room2</div>
								</td>
								<td id="divRoom3" style="CURSOR: pointer" onclick="SelectRoom(3,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room3</div>
								</td>
								<td id="divRoom4" style="CURSOR: pointer" onclick="SelectRoom(4,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room4</div>
								</td>
								<td id="divRoom5" style="CURSOR: pointer" onclick="SelectRoom(5,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room5</div>
								</td>
								<td id="divRoom6" style="CURSOR: pointer" onclick="SelectRoom(6,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room6</div>
								</td>
								<td id="divRoom7" style="CURSOR: pointer" onclick="SelectRoom(7,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room7</div>
								</td>
								<td id="divRoom8" style="CURSOR: pointer" onclick="SelectRoom(8,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room8</div>
								</td>
								<td id="divRoom9" style="CURSOR: pointer" onclick="SelectRoom(9,'divRoom','DataRoom');"
									runat="server">
									<div class="Tab" style="DISPLAY: inline; WIDTH: 100%">Room9</div>
								</td>
							</tr>
							<tr>
								<td width="5%"></td>
								<td colspan="9">
									<div id="DataRoom1">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos1" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts1" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet1" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet1" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild1" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild1" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom2">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos2" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts2" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet2" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet2" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild2" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild2" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom3">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos3" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts3" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet3" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet3" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild3" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild3" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom4">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos4" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts4" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet4" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet4" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild4" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild4" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom5">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos5" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts5" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet5" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet5" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild5" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild5" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom6">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos6" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts6" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet6" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet6" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild6" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild6" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom7">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos7" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts7" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet7" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet7" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild7" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild7" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom8">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos8" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts8" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet8" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet8" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild8" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild8" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
									<div id="DataRoom9">
										<table width="100%">
											<tr>
												<td width="15%">
													<h3 id="lblAdultos9" align="right" runat="server">Adultos</h3>
												</td>
												<td width="5%"><asp:dropdownlist id="dlAduts9" runat="server">
														<asp:ListItem>1</asp:ListItem>
														<asp:ListItem>2</asp:ListItem>
														<asp:ListItem>3</asp:ListItem>
														<asp:ListItem>4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
												<td width="20%" rowSpan="2">
													<h3 id="lbltitlePet9" align="right" runat="server">Petición Especial:</h3>
												</td>
												<td rowSpan="2"><asp:textbox id="txtPet9" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<td>
													<h3 id="lblChild9" align="right" runat="server">Niños</h3>
												</td>
												<td><asp:dropdownlist id="dlChild9" runat="server">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3">3</asp:ListItem>
														<asp:ListItem Value="4">4</asp:ListItem>
														<asp:ListItem>5</asp:ListItem>
														<asp:ListItem>6</asp:ListItem>
														<asp:ListItem>7</asp:ListItem>
														<asp:ListItem>8</asp:ListItem>
														<asp:ListItem>9</asp:ListItem>
														<asp:ListItem>10</asp:ListItem>
														<asp:ListItem>11</asp:ListItem>
														<asp:ListItem>12</asp:ListItem>
														<asp:ListItem>13</asp:ListItem>
														<asp:ListItem>14</asp:ListItem>
														<asp:ListItem>15</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr><td colspan =4>
				        <table width="100%">
							<tr>
								<td class="titulo" colSpan="4">
									<h2 id="lblMainAgenciaData" runat="server">Datos de la agencia</h2>
								</td>
							</tr>
							<tr><td align=right >
                                <asp:Label ID="lblAgencia" runat="server" Text="Agencia:"></asp:Label></td>
                                
                                <td colspan =3><asp:TextBox ID="txtAgencia" runat="server" CssClass="textbox" Columns="40"></asp:TextBox></td></tr>
							</table> 
				</td></tr>
				<tr>
					<td colSpan="4">
						<table width="100%">
							<tr>
								<td class="titulo" colSpan="4">
									<h2 id="lblMainTravData" runat="server">Datos del viajero Principal</h2>
								</td>
							</tr>
							<tr>
								<TD align="right">
									<h3 id="lblName" runat="server">Nombre</h3>
								</TD>
								<TD align="left"><asp:textbox id="txtNombre" runat="server" Columns="25" MaxLength="80" CssClass="textbox"></asp:textbox><asp:requiredfieldvalidator id="RfvNombre" runat="server" Display="Dynamic" CssClass="Validators" ErrorMessage="*"
										ControlToValidate="txtNombre"></asp:requiredfieldvalidator></TD>
								<TD align="left">
									<h3 id="lblLastName" align="right" runat="server">Apellido</h3>
								</TD>
								<TD align="left"><asp:textbox id="txtApellido" runat="server" Columns="25" MaxLength="80" CssClass="textbox"></asp:textbox><asp:requiredfieldvalidator id="RfvApellido" runat="server" Display="Dynamic" CssClass="Validators" ErrorMessage="*"
										ControlToValidate="txtApellido"></asp:requiredfieldvalidator></TD>
							</tr>
							<TR>
								<TD>
									<h3 id="lblHomePhone" align="right" runat="server">Telefono de casa</h3>
								</TD>
								<TD align="left"><asp:textbox id="txtHomePhone" runat="server" Columns="15" MaxLength="28" CssClass="textbox"></asp:textbox></TD>
								<TD>
									<h3 id="lblWorkHome" align="right" runat="server">Telefono de trabajo</h3>
								</TD>
								<TD align="left"><asp:textbox id="txtWorkHome" runat="server" Columns="15" MaxLength="28" CssClass="textbox"></asp:textbox></TD>
							</TR>
							<TR>
								<TD>
									<h3 id="lblAddress" align="right" runat="server">Direccion</h3>
								</TD>
								<TD align="left"><asp:textbox id="txtAddress" runat="server" Columns="20" MaxLength="50" CssClass="textbox"></asp:textbox></TD>
								<TD>
									<h3 id="lblemail" align="right" runat="server">Correo Electronico</h3>
								</TD>
								<TD align="left"><asp:textbox id="txtEmail" runat="server" Columns="20" MaxLength="80" CssClass="textbox"></asp:textbox><asp:regularexpressionvalidator id="REMail" runat="server" Display="Dynamic" CssClass="Validators" ErrorMessage="Invalid Mail"
										ControlToValidate="txtEmail" ValidationExpression="^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$"></asp:regularexpressionvalidator></TD>
							</TR>
						</table>
					</td>
				</tr>
				<tr>
					<td colSpan="4" align="center">
						<table width="100%">
							<tr>
								<td class="titulo" colSpan="6">
									<h2 id="lblCCData" runat="server">Datos de la Tarjeta de Crédito</h2>
								</td>
							</tr>
							<tr>
								<td align="right">
									<h3 id="lblCCType" runat="server">Card Type</h3>
								</td>
								<td><asp:dropdownlist id="ddlCC" Runat="server">
										<asp:ListItem>-Select-</asp:ListItem>
									</asp:dropdownlist></td>
								<td align="right">
									<h3 id="lblCCNumber" runat="server">Card Number</h3>
								</td>
								<td><asp:textbox id="txtCCNumber" runat="server" Columns="16" MaxLength="16"></asp:textbox>
									<asp:RegularExpressionValidator id="RevCCNumber" runat="server" ErrorMessage="Invalid number" CssClass="validators"
										Display="Dynamic" ControlToValidate="txtCCNumber" ValidationExpression="(^\d{15,16})|(^\d{13})"></asp:RegularExpressionValidator></td>
								<td align="right">
									<h3 id="lblCCCode" runat="server">Security code</h3>
								</td>
								<td><asp:textbox id="txtSecCode" Columns="3" MaxLength="3" Runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td align="right">
									<h3 id="llCCHolder" runat="server">Holder</h3>
								</td>
								<td><asp:textbox id="txtCCHolder" Columns="20" MaxLength="80" Runat="server"></asp:textbox></td>
								<td align="right">
									<h3 id="lblCCExp" runat="server">Expiration Date</h3>
								</td>
								<td colSpan="3"><asp:textbox id="txtccMonth" Columns="2" MaxLength="2" Runat="server"></asp:textbox>/<asp:textbox id="txtCCYear" Columns="4" MaxLength="4" Runat="server"></asp:textbox>
									<asp:RegularExpressionValidator id="ReCcY" runat="server" Display="Dynamic" CssClass="validators" ErrorMessage="Invalid Year"
										ControlToValidate="txtCCYear" ValidationExpression="^\d{4}"></asp:RegularExpressionValidator>
									<asp:RangeValidator id="RvCcM" runat="server" Display="Dynamic" CssClass="validators" ErrorMessage="Invalid Month"
										ControlToValidate="txtccMonth" MaximumValue="12" MinimumValue="01"></asp:RangeValidator>
									<asp:RegularExpressionValidator id="ReCcM" runat="server" Display="Dynamic" CssClass="validators" ErrorMessage="Invalid Month"
										ControlToValidate="txtccMonth" ValidationExpression="^\d{2}"></asp:RegularExpressionValidator></td>
							</tr>
							<TR>
								<TD colSpan="6"><asp:customvalidator id="cvCC" runat="server" CssClass="validators" ErrorMessage="Invalid Data"></asp:customvalidator></TD>
							</TR>
						</table>
						<input class="button" id="btnContinue" type="button" value="Save" runat="server">
						<asp:button id="btnSaveReserva" style="DISPLAY: none" runat="server" CssClass="button" EnableViewState="False"
							Text="Save"></asp:button>
						<asp:Label id="lblError" runat="server" CssClass="validators">Label</asp:Label></td>
				</tr>
			</table>
			<uc1:ctlreservamsj id="CtlReservaMsj1" runat="server"></uc1:ctlreservamsj></form>
	</body>
</HTML>
