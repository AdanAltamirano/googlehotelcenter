<%@ Register TagPrefix="uc1" TagName="ctrlLanguageButton" Src="ctrlLanguageButton.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlRooms.ascx.vb" Inherits="RateManager.ctrlRooms" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdiomaRFCK" Src="CtrlIdiomaRFCk.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlLanguageDictionary" Src="ctrlLanguageDictionary.ascx" %>

<%@ Register TagPrefix="uc1" TagName="ctrlImagesRooms" Src="../Portal/Modules/Contenido/ctrlImagesRooms.ascx" %>

<input id="iSpanishDesc" runat="server" type="hidden"> <input id="iEnglishDesc" runat="server" type="hidden" NAME="Hidden1">
<TABLE id="Table5" class="Form1" cellSpacing="0" cellPadding="0"  border="0" width=100%>
	<tr>
		<td colSpan="4">
		    <TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
		        <tr><td><asp:label id="lblTypeRoom" runat="server" CssClass="clsLabel" EnableViewState="False"> Tipo habitación:</asp:label></td><td colspan= 4>
						<asp:dropdownlist id="lstRoomType" runat="server" style="width:220px;" ></asp:dropdownlist>
						<asp:requiredfieldvalidator id="rfvRoomsType" runat="server" CssClass="validators" ErrorMessage="Campo requerido"
							ControlToValidate="lstRoomType" ForeColor=" " Display="Dynamic"></asp:requiredfieldvalidator></td>
							
							<td><asp:label id="lblNombre" runat="server" CssClass="clsLabel" EnableViewState="False">Nombre:</asp:label></td><td colspan="6"><uc1:ctrlidioma id="mlNameRoom" runat="server" ></uc1:ctrlidioma></td></tr>
							
							
		        <tr>
		        <td>
						<asp:label id="lbPersonas" runat="server" EnableViewState="False" CssClass="clslabel">Personas:</asp:label></td><td><asp:dropdownlist id="lstPeoplesInRoom" runat="server">
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
						</asp:dropdownlist></td><td><asp:label id="lblMinNumberAdults" runat="server" CssClass="clslabel" EnableViewState="False">Min Adultos:</asp:label></td><td><asp:dropdownlist id="lstMinNumberAdults" runat="server">
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
						</asp:dropdownlist></td><td><asp:label id="lblNumberAdults" runat="server" CssClass="clslabel" EnableViewState="False">Adultos:</asp:label></td><td><asp:dropdownlist id="lstNumberAdults" runat="server">
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
						</asp:dropdownlist></td><td><asp:label id="lblNumberRooms" runat="server" CssClass="clsLabel" EnableViewState="False"> Habitaciones:</asp:label></td><td><asp:textbox id="txtNumberRooms" runat="server" CssClass="textbox" MaxLength="3" Columns="3"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" CssClass="validators" Display="Dynamic"
							ForeColor=" " ControlToValidate="txtNumberRooms" ErrorMessage="1-999" ValidationExpression="^\d+$"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="validators" Display="Dynamic"
							ForeColor=" " ControlToValidate="txtNumberRooms" ErrorMessage="*"></asp:requiredfieldvalidator></td><td><asp:label id="lblNumberChildrens" runat="server" CssClass="clslabel" EnableViewState="False">Niños:</asp:label></td><td><asp:dropdownlist id="lstNumberChildrens" runat="server">
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
						</asp:dropdownlist></td><td><asp:label id="lblPeoplesExtras" runat="server" CssClass="clslabel" EnableViewState="False"> Extras:</asp:label></td><td><asp:dropdownlist id="lstPeoplesExtras" runat="server">
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
						</asp:dropdownlist></td></tr>		     
		    </TABLE>			
		</td>
	</tr>
	<tr><td colSpan="4">
	<TABLE cellSpacing="1" cellPadding="0" width="100%" border="0">
				
				<TR>
					<TD align="right" colspan=11>&nbsp;</TD>
				</TR>			
				<tr>
					<TD></TD>
					<TD></TD>
					<td colSpan="3"><asp:customvalidator id="cvImagen" runat="server" CssClass="validators" Display="Dynamic" ForeColor=" ">Imagen invalida</asp:customvalidator></td>
					<td></td>
					<td></td>
					<TD></TD>
					<td align="center" colSpan="3" rowSpan="2"><asp:image id="ImagenHabitacion" runat="server" CssClass="clsBackGround" Width="75px" Height="75px"
							Visible="False"></asp:image></td>
				</tr>
				<tr>
					<TD align="right">
						<asp:label id="lblOrden" runat="server" EnableViewState="False" CssClass="clslabel">Orden:</asp:label></TD>
					<TD align="left">
						<asp:textbox id="txtOrden" runat="server" CssClass="textbox" Width="30px" Columns="3" MaxLength="2"></asp:textbox><asp:regularexpressionvalidator id="Regularexpressionvalidator2" runat="server" CssClass="validators" Display="Dynamic"
							ForeColor=" " ControlToValidate="txtOrden" ErrorMessage="0-99" ValidationExpression="^\d+$"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" CssClass="validators" Display="Dynamic"
							ForeColor=" " ControlToValidate="txtOrden" ErrorMessage="*"></asp:requiredfieldvalidator></TD>
					<TD align="right">
						<asp:label id="lblImgShow" runat="server" EnableViewState="False" CssClass="clsLabel">Imagen:</asp:label></TD>
					<TD align="left" colSpan="3"><INPUT class="textbox" id="ImgFileOpen" style="WIDTH: 100%; HEIGHT: 30px" type="file"
							size="67" name="ImgFileOpen" runat="server"></TD>
					<TD colSpan="5">&nbsp;
					</TD>
				</tr>
				<TR>
					<TD align="center" colSpan="11" height="15"></TD>
				</TR>
				<TR>
					<td class="dgitem" colspan="11">
							<asp:label id="lblExtraData" runat="server" EnableViewState="False">Extra data</asp:label>
					</td>
				</TR>
				<TR>
					<TD align="left"></TD>
					<TD align="left"></TD>
					<TD align="left"></TD>
					<TD></TD>
					<TD align="center"><asp:label id="lblMaxAdultRoll" runat="server" CssClass="clslabel" EnableViewState="False"></asp:label></TD>
					<TD align="center"><asp:label id="lblMaxChildRoll" runat="server" CssClass="clslabel" EnableViewState="False"></asp:label></TD>
					<td align="center" colSpan="2"><asp:label id="lblMaxCribRoll" runat="server" CssClass="clslabel" EnableViewState="False"></asp:label></td>
					<td></td>
					<TD colspan=3></TD>
				</TR>
				<TR >
					<TD style="HEIGHT: 21px" align="right"></TD>
					<TD style="HEIGHT: 21px" align="right"></TD>
					<TD style="HEIGHT: 21px" align="right">
						<asp:label id="lblRollAway" runat="server" EnableViewState="False" CssClass="clslabel"></asp:label></TD>
					<TD style="HEIGHT: 21px" align="right"></TD>
					<TD style="HEIGHT: 21px" align="center">
						<asp:dropdownlist id="ddlMaxAdultRoll" runat="server" Width="50px">
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="HEIGHT: 21px" align="center">
						<asp:dropdownlist id="ddlMaxChildRoll" runat="server" Width="50px">
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="HEIGHT: 22px" align="center" colSpan="2">
						<asp:dropdownlist id="ddlMaxCribRoll" runat="server" Width="50px">
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="HEIGHT: 22px"></TD>
					<TD style="HEIGHT: 22px" colspan =3></TD>
				</TR>
				<TR>
					<TD align="left"></TD>
					<TD align="left"></TD>
					<TD align="left">
						<DIV id="divlabelPrice" align="right" runat="server">
							<asp:label id="lblPriceCribRoll" runat="server" EnableViewState="False" CssClass="clslabel"></asp:label></DIV>
					</TD>
					<TD></TD>
					<TD>
						<DIV id="divPriceAdult" align="center" runat="server"><asp:textbox id="txtPriceAdultRoll" runat="server" CssClass="textbox" Width="50px" MaxLength="10"
								Visible="true"></asp:textbox><asp:rangevalidator id="ValAdultPriceCribRoll" runat="server" CssClass="validators" Display="Dynamic"
								ControlToValidate="txtPriceAdultRoll" ErrorMessage="0-99999" MaximumValue="99999" MinimumValue="0" Type="Double"></asp:rangevalidator></DIV>
					</TD>
					<td>
						<DIV id="divPriceChild" align="center" runat="server"><asp:textbox id="txtPriceChildRoll" runat="server" CssClass="textbox" Width="50px" MaxLength="10"
								Visible="true"></asp:textbox><asp:rangevalidator id="ValChildPriceCribRoll" runat="server" CssClass="validators" Display="Dynamic"
								ControlToValidate="txtPriceChildRoll" ErrorMessage="0-99999" MaximumValue="99999" MinimumValue="0" Type="Double"></asp:rangevalidator></DIV>
					</td>
					<TD align="center" colSpan="2">
						<DIV id="divPriceCrib" runat="server"><asp:textbox id="txtPriceCribRoll" runat="server" CssClass="textbox" Width="50px" MaxLength="10"
								Visible="true"></asp:textbox><asp:rangevalidator id="valPriceCribRoll" runat="server" CssClass="validators" Display="Dynamic" ControlToValidate="txtPriceCribRoll"
								ErrorMessage="0-99999" MaximumValue="99999" MinimumValue="0" Type="Double"></asp:rangevalidator></DIV>
					</TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
			</TABLE>
	
	</td></tr>
	<TR>
		<TD align="center" colSpan="4" height="10"></TD>
	</TR>
	<tr>
		 
	</tr>
	<TR>
		<TD class="dgitem" align="center" colSpan="4"><asp:label id="lblDescription" runat="server" CssClass="clslabel" EnableViewState="False">Descripción para la habitación:</asp:label></TD>
	</TR>
	<TR>
		<TD colSpan="4" style="height:auto; overflow:hidden;"><uc1:ctrlidiomaRFCK id="mlDescriptionRoom" runat="server" Height="400"></uc1:ctrlidiomaRFCK></TD>
	</TR>
	<TR>
		<TD colSpan="4"><asp:requiredfieldvalidator id="rfvDescription" runat="server" CssClass="validators" Display="Dynamic" ForeColor=" "
				ControlToValidate="txtDescription" ErrorMessage="Campo requerido" Visible="False"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD colSpan="4"><uc1:ctrllanguagebutton id="CtrlLanguageButton1" runat="server" Visible="False"></uc1:ctrllanguagebutton></TD>
	</TR>
	<TR>
		<TD colSpan="4"><asp:textbox id="txtDescription" runat="server" CssClass="textbox" Width="100%" Height="200px"
				Visible="False" TextMode="MultiLine"></asp:textbox></TD>
	</TR>
	<% If Me.HasData AndAlso (Not Me.mlDescriptionRoom.Published OrElse Not Me.mlNameRoom.Published) Then%>
	<tr>
	    <td colspan="4">
	        <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
	    </td>
	</tr>
	<% end If %>
	<tr>
		<td colSpan="4"></td>
	</tr>
	<tr>
		<uc1:ctrlImagesRooms id="ctrlImgRooms1" runat="server"></uc1:ctrlImagesRooms>
	</tr>
</TABLE>
<script>
    function ShowPrice(ddl, div, txt, txtV, label, divlabel) {

        var d = document.getElementById(ddl);
        var t = document.getElementById(div);
        var valor = document.getElementById(txt);
        var t2 = document.getElementById(txtV);
        var d1 = document.getElementById('txtDiv1');
        var d2 = document.getElementById('txtDiv2');
        var d3 = document.getElementById('txtDiv3');

        var l = document.getElementById(label);
        var dl = document.getElementById(divlabel);
        if (d.selectedIndex == 0) {
            t.style.display = 'none';
            valor.value = "";
            t2.value = 0;
        }
        else {
            t.style.display = '';
            t2.value = 1;
        }
        if (d1.value == 1 || d2.value == 1 || d3.value == 1) {
            dl.style.display = '';
        }
        else {
            dl.style.display = 'none';
        }
    }

    function ShowName(val, en, es, iSp, iEn) {

        var d = document.getElementById(val);
        var t2 = document.getElementById(en);
        var t3 = document.getElementById(es);
        var texto = d.item(d.selectedIndex);
        var iS = document.getElementById(iSp);
        var SplitIS = iS.value.split("*|*");
        var iE = document.getElementById(iEn);
        var SplitIE = iE.value.split("*|*");

        texto = (texto.text).split("-");
        if (texto.length >= 2) {
            t2.value = SplitIE[d.selectedIndex];//texto[1];
            t3.value = SplitIS[d.selectedIndex];//texto[1];
        }

    }

    //****  Function for FCK Editor.  ****
    function ShowNameFck(val, en, es, iSp, iEn) {
        var d = document.getElementById(val);
        var t2 = document.getElementById(en);
        var t3 = document.getElementById(es);
        var texto = d.item(d.selectedIndex);
        var iS = document.getElementById(iSp);
        var SplitIS = iS.value.split("*|*");
        var iE = document.getElementById(iEn);
        var SplitIE = iE.value.split("*|*");

        texto = (texto.text).split("-");
        if (texto.length >= 2) {
            // Get the editor instance.
            var oEditorEn = FCKeditorAPI.GetInstance(en);
            var oEditorEs = FCKeditorAPI.GetInstance(es);
            var oDOMEn = oEditorEn.EditorDocument;
            var oDOMEs = oEditorEs.EditorDocument;

            if (document.all) {
                // IE engine.
                oDOMEn.body.innerText = SplitIE[d.selectedIndex];
                oDOMEs.body.innerText = SplitIS[d.selectedIndex];
            }
            else {
                // Gecko engine (FF, usw.)
                oEditorEn.EditorDocument.body.innerHTML = SplitIE[d.selectedIndex];
                oEditorEs.EditorDocument.body.innerHTML = SplitIS[d.selectedIndex];
            }
        }
    }

    function saveImg(lbl) {
        var imgFile = document.getElementById("ImgFileOpen");
        imgFile.click();
        var RutaFile = document.getElementById(lbl);
        RutaFile.value = imgFile.value;
    }


</script>
