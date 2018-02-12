<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Imagenes.ascx.vb" Inherits="RateManager.Imagenes"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<meta content="False" name="vs_snapToGrid">
<meta content="False" name="vs_showGrid">
<!-- Estilo necesario para el componente -->
<style>
    .ImgTitulo
    {
        font-weight: bold;
        color: white;
        font-family: Verdana;
        background-color: #336699;
    }
    .ImgLetra
    {
        font-weight: bold;
        font-size: 0.8em;
        color: blue;
        font-family: Verdana;
    }
    .ImgTexto
    {
        font-weight: bold;
        font-size: 0.8em;
        color: black;
        font-family: Verdana;
    }
    .Borde
    {
        border-right: #006699 1px solid;
        border-top: #006699 1px solid;
        border-left: #006699 1px solid;
        width: 75px;
        border-bottom: #006699 1px solid;
    }
</style>
<!--/*****************************************************************/  este no -->
<div class="borde" id="DivOpenImgFile" style="width: 335px; position: absolute;">
    <table class="border1" id="TableModulo" style="width: 100%;" cellspacing="0" cellpadding="0"
        width="339" align="left" bgcolor="#cccccc" runat="server">
        <tr align="center">
            <td class="ImgTitulo" style="width: 371px">
                <asp:Label ID="LblTitulo" runat="server" Font-Size="11px" CssClass="ImgTitulo" EnableViewState="False">Imagen</asp:Label>
            </td>
            <td class="ImgTitulo" align="right">
                <img id="CloseImgDiv" onclick="hideimgs()" src="../Images/delete.gif" align="right">
            </td>
        </tr>
        <tr>
            <td nowrap align="center" colspan="2">
                <asp:Label ID="LblDesc" runat="server" Font-Size="12px" Font-Names="arial" EnableViewState="False" CssClass="clsdarklabel">Agregar un archivo de imagen:</asp:Label><br>
                <input id="FileAbreImagen" style="width: 324px;" type="file" size="34" name="FileAbreImagen"
                    runat="server" >
                <br>
                <asp:Label ID="LblError" runat="server" Font-Size="Smaller" Visible="False" EnableViewState="False"
                    ForeColor="Red"></asp:Label><br>
                <asp:Button ID="BtnImgAdd" runat="server" Width="70px" Text="Añadir" EnableViewState="False" CssClass="Button">
                </asp:Button>&nbsp;<input id="BtnCancelar" style="width: 70px;" onclick="hideimgs()"
                    type="button" value="Cancelar" runat="server" class="Button">
            </td>
        </tr>
    </table>
</div>
<div id="divIMGS" style="">
    <!-- delimita todo el div -->
    <table id="AlbumComponent" style="width: 100%;" bordercolor="#336699" cellspacing="0"
        cellpadding="0" bgcolor="#ffffff" border="1" runat="server">
        <tr>
            <td class="ImgTitulo" align="left" style= " padding-left:8px; ">
                <asp:Label ID="LblTit" runat="server"  CssClass="ImgTitulo">Album&nbsp;de&nbsp;fotos</asp:Label>
            </td>
            <td class="ImgTitulo" valign="middle" align="right" style= " padding-right:8px; ">
                <table id="Table1" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>
                            <img id="ShowLoadsImgs" style="cursor: pointer" onclick="showimgs()" alt="" src="../Images/add.gif">
                        </td>
                        <td>
                            <asp:Label ID="lblAgregar" runat="server" CssClass="ImgTitulo">Agregar</asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="ImgBtnHide" runat="server" Visible="False" ImageUrl="../Images/delete.gif">
                            </asp:ImageButton>
                        </td>
                        <td>
                            <asp:Label ID="lblEliminar" runat="server" Font-Size="11px" CssClass="ImgTitulo"
                                Visible="False">Salir</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="2">
                <div id="DivScroll" style="overflow: auto; width: 660px;">
                    <asp:DataList ID="datalistimagenes" runat="server" Width="352px" HorizontalAlign="Left"
                        RepeatDirection="Horizontal">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <table style="border-top-style: none; border-right-style: none; border-left-style: none;
                                border-bottom-style: none" width="75" align="left">
                                <tr>
                                    <td class="Borde" align="center" colspan="3">
                                        <asp:ImageButton ID="ImageButton1" runat="server" Style='<%# Resize(true,"200px") %>'
                                            ImageUrl='<%# CargaImagen(System.Configuration.ConfigurationManager.AppSettings("Albums_url"),DataBinder.Eval(Container.DataItem,"IdRubro"),DataBinder.Eval(Container.DataItem,"IdEmpresa"), DataBinder.Eval(Container.DataItem,"IdImagen"), "_T") %>'
                                            CommandName="Edit" CommandArgument='<%# CargaImagen(Albums.Comun.General.getPath,DataBinder.Eval(Container.DataItem,"IdRubro"),DataBinder.Eval(Container.DataItem,"IdEmpresa"), DataBinder.Eval(Container.DataItem,"IdImagen"), "") %>'
                                            ToolTip="Seleccionar" ImageAlign="AbsMiddle"></asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td valign="top" align="right">
                                        <asp:ImageButton ID="ImgErase" runat="server" ImageUrl="../../../Images/delete.gif"
                                            CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"IdImagen") %>'
                                            ToolTip="Eliminar"></asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList></div>
            </td>
        </tr>
    </table>
</div>

<script language="javascript">
	//document.getElementById("DivOpenImgFile").style.display="none";
	function showimgs()
	{
	    //muestra el div de abrir archivo
		document.getElementById("divIMGS").style.display="block";
		document.getElementById("DivOpenImgFile").style.display="block";
		return true;
	}
	function hideimgs()
	{
		//oculta el div de abrir archivo
		document.getElementById("DivOpenImgFile").style.display="none";
		//document.getElementById("DivOpenImgFile").style.display="none";
 		//Mod8_lnkImagPropiedades_EditConte_ImgControl_LblError
		return true;
	}	
	function  HideDivImgs()
	{
		//oculta el div de imagenes, y el div de abrir archivo
		document.getElementById("DivOpenImgFile").style.display="none";
		document.getElementById("divIMGS").style.display="none";
		return true;
	}
/*var dragswitch=0
var nsx
var nsy
var nstemp
var dragapproved=false

function drag_dropns(name){
temp=eval(name)
obj = name
temp.captureEvents(Event.MOUSEDOWN | Event.MOUSEUP)
temp.onmousedown=gons
temp.onmousemove=dragns
temp.onmouseup=stopns
}

function gons(e){
temp.captureEvents(Event.MOUSEMOVE)
nsx=e.x
nsy=e.y
}

function dragns(e){
if (dragswitch==1){
temp.moveBy(e.x-nsx,e.y-nsy)
return false
}
}

function stopns(){
temp.releaseEvents(Event.MOUSEMOVE)
}

var obj 

function drag_dropie(){
if (dragapproved==true)
{

	obj.style.pixelLeft=tempx+event.clientX-iex
	obj.style.pixelTop=tempy+event.clientY-iey
	dragapproved=true; //esta borrar
	return false
}
}


function initializedragie(f){
iex=event.clientX
iey=event.clientY
obj = f
tempx=f.style.pixelLeft
tempy=f.style.pixelTop
dragapproved=true
document.onmousemove=drag_dropie;
}


if (document.all){
document.onmouseup=new Function("dragapproved=false")
}

////drag drop functions end here//////


function hidebox(f){
if (document.all)
f.style.visibility="hidden"
else if (document.layers)
document.f.visibility="hide"
}*/
</script>

