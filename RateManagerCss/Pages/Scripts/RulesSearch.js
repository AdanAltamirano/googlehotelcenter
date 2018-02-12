
var sbHttp = null;   //Objeto principal para realizar la peticion por xmlhttp
var fechaFinal = "";
var dtRequest = "";
var mostrarRules = 0;
//Funcion que busca las reglas por hotel

function searchRulesHotel(fecha, nivelRegla) //Fecha en la que se buscara las reglas
{
    //	if (sbHttp==null) 	
    //	{
    //		sbHttp = new Ajax.Request(); 
    //		if (!sbHttp.inprogress) 
    //			{	
    //				var url = "LoadAvailabilityRestrictions.ashx";
    //				var dt = new Date();
    //				var qry="";
    //				qry="fecha=" + fecha + "&nivelRegla=" + nivelRegla;
    //				var sbHttp = new Ajax.Request(url,{method: 'get',asynchronous:true,parameters:qry,onComplete:HotelRules});        								
    //			}
    //		else 
    //			{
    //				alert("busy");
    //			}
    //    }

    $.ajax({
    url: "LoadAvailabilityRestrictions.ashx",
        type: 'GET',
        data: { fecha: fecha, nivelRegla: nivelRegla, dtrequest: dtRequest },
        dataType: 'xml',
        success: function(resp) {
            HotelRules(resp);
        }
    });
}

//Funcion que recibe las reglas del hotel
function HotelRules(client) {
    hotelCtlInit();
    if (!client.cancelled) {


        var json;
        //var json = client.responseXML.getElementsByTagName("Hoteles");
        var json = $(client).find("Hoteles");

        var j = 0;
        var i = 0;
        var NoArrivals = "";

        var txtMinNightsH = document.getElementById("CtlMensajeRuleConf1_txtMinNightsH");
        var txtAdvancedDaysH = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysH");
        var txtMaxNightsH = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsH");
        var status = document.getElementById("CtlMensajeRuleConf1_LabelStatH");

        var lblCancelacionH = document.getElementById("CtlMensajeRuleConf1_lblCancelacionH");
        var TextboxCH = document.getElementById("CtlMensajeRuleConf1_TextboxCH");
        var lblMsgCH = document.getElementById("CtlMensajeRuleConf1_lblMsgCH");

        while (json[j]) {
            txtMinNightsH.value = getTagText(json[j], 'MinLengthStay');
            txtMaxNightsH.value = getTagText(json[j], 'MaxDiasRenta');
            txtAdvancedDaysH.value = getTagText(json[j], 'DiasLibres');
            NoArrivals = getTagText(json[j], 'NoArrivals');
            status.innerHTML = getTagText(json[j], 'StatusAvail');
            lblCancelacionH.innerHTML = getTagText(json[j], 'CancelacionPor');
            TextboxCH.value = getTagText(json[j], 'CancelacionValor');
            lblMsgCH.innerHTML = getTagText(json[j], 'MsgCancelacion');

            if (NoArrivals.length > 1) {
                for (i = 0; i <= 6; i++) {
                    var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "H");
                    if (NoArrivals.charAt(i) == 'N') {
                        chk.checked = false;
                    }
                    else {
                        chk.checked = true;
                    }
                }
            }
            j++;
        }
    }
    mostrarRules = mostrarRules + 1;
    mostrarR();
}

/*Funcion para obtener el elemento de un xml*/
function getTagText(parent, item) {
    try {
        var result = parent.getElementsByTagName(item)[0];
        if (result) {
            if (result.firstChild)
                return result.firstChild.nodeValue;
            else
                return result.text;
        }
        else { return ""; }
    }
    catch (e)
    { return ""; }
}

/*Funcion que limpia los datos de las reglas del hotel*/
function hotelCtlInit() {
    var txtMinNightsH = document.getElementById("CtlMensajeRuleConf1_txtMinNightsH");
    var txtAdvancedDaysH = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysH");
    var txtMaxNightsH = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsH");

    txtMinNightsH.value = "";
    txtAdvancedDaysH.value = "";
    txtMaxNightsH.value = "";

    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "H");
        chk.checked = false;
    }
}

//Funcion que busca los ratePlans a la fecha
function searchRatePlan(fecha, nivelRegla) //Fecha en la que se buscara las reglas
{
    //    fechaFinal = fecha;
    //    if (sbHttp == null) {
    //        sbHttp = new Ajax.Request();
    //        if (!sbHttp.inprogress) {
    //            var url = "LoadAvailabilityRestrictions.ashx";
    //            var dt = new Date();
    //            var qry = "";
    //            qry = "fecha=" + fecha + "&nivelRegla=" + nivelRegla;
    //            var sbHttp = new Ajax.Request(url, { method: 'get', asynchronous: true, parameters: qry, onComplete: RatePlans });
    //        }
    //        else {
    //            alert("busy");
    //        }
    //    }
    //    else {
    //        alert("No hizo la invocacion con Ajax");
    //    }

    var lblDateSRCH = document.getElementById("CtlMensajeRuleConf1_lblDateSearch");
    if (lblDateSRCH) lblDateSRCH.innerHTML = fecha;
    fechaFinal = fecha;
    dtRequest = new Date;
    $.ajax({
        url: "LoadAvailabilityRestrictions.ashx",
        type: 'GET',
        data: { fecha: fecha, nivelRegla: nivelRegla, dtrequest: dtRequest },
        dataType: 'xml',
        success: function(resp) {
            RatePlans(resp);
        }
    });
}

//Funcion que recibe las reglas del hotel
function RatePlans(client) {
    RPCtlInit();
    if (!client.cancelled) {
        var json;
        //var json = client.responseXML.getElementsByTagName("RatePlan");
        var json = $(client).find("RatePlan");
        var j = 0;
        var rp = false;
        while (json[j]) {
            //alert(getTagText(json[j],'NombreRP'));	
            AddItemRP(getTagText(json[j], 'NombreRP'), getTagText(json[j], 'idRatePlan'));
            rp = true;
            j++;
        }
        if (!rp) {
            //No mostramos el renglon de los rateplans						
            document.getElementById("TblRuleRatePLan").style.display = 'none';
            document.getElementById("TblNoRuleRatePLan").style.display = '';
        }
        else {
            //Mostramos el reglon de los rateplans
            document.getElementById("TblRuleRatePLan").style.display = '';
            document.getElementById("TblNoRuleRatePLan").style.display = 'none';
        }
    }
    mostrarRules = mostrarRules + 1;
    mostrarR();
    //show2("CtlMensajeRuleConf1_PnlBox");
}

function SearchRatePlanRules(nivelRegla) {
    //    if (sbHttp == null) {
    //        sbHttp = new Ajax.Request();
    //        if (!sbHttp.inprogress) {
    //            var ddlRatePlan = document.getElementById("CtlMensajeRuleConf1_ddlRatePlan");
    //            var url = "LoadAvailabilityRestrictions.ashx";
    //            var dt = new Date();
    //            var qry = "";
    //            qry = "fecha=" + fechaFinal + "&nivelRegla=" + nivelRegla + "&idRatePlan=" + ddlRatePlan.options[ddlRatePlan.selectedIndex].value;
    //            if (ddlRatePlan.options[ddlRatePlan.selectedIndex].value != "-1") {
    //                var sbHttp = new Ajax.Request(url, { method: 'get', asynchronous: true, parameters: qry, onComplete: LoadRatePlanRules });
    //            }
    //            else {
    //                RPCtlInit2();
    //            }
    //        }
    //        else {
    //            alert("busy");
    //        }
    //    }

    var ddlRatePlan = document.getElementById("CtlMensajeRuleConf1_ddlRatePlanConf");
    var _valueOpRate = ddlRatePlan.options[ddlRatePlan.selectedIndex].value;
    if (ddlRatePlan.options[ddlRatePlan.selectedIndex].value != "-1") {

        $.ajax({
            url: "LoadAvailabilityRestrictions.ashx",
            type: 'GET',
            cache: false,
            data: { fecha: fechaFinal, nivelRegla: nivelRegla, _idRatePlan: _valueOpRate, dtrequest: dtRequest },
            dataType: 'xml',
            success: function(resp) {
                LoadRatePlanRules(resp);
            }
        });
    }
    else
        RPCtlInit2();

}

function LoadRatePlanRules(client) {
    if (!client.cancelled) {
        RPCtlInit2();
        var json;
        //var json = client.responseXML.getElementsByTagName("RatePlanRules");
        var json = $(client).find("RatePlanRules");
        var j = 0;
        var i = 0;
        var NoArrivals = "";

        var txtMinNightsRP = document.getElementById("CtlMensajeRuleConf1_txtMinNightsRP");
        var txtAdvancedDaysRP = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysRP");
        var txtMaxNightsRP = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsRP");
        var status = document.getElementById("CtlMensajeRuleConf1_LabelStatRP");

        var lblCancelacionRP = document.getElementById("CtlMensajeRuleConf1_lblCancelacionRP");
        var TextboxCRP = document.getElementById("CtlMensajeRuleConf1_TextboxCRP");
        var lblMsgCRP = document.getElementById("CtlMensajeRuleConf1_lblMsgCRP");

        while (json[j]) {
            txtMinNightsRP.value = (getTagText(json[j], 'MinNoches') == '0') ? '' : getTagText(json[j], 'MinNoches');
            txtMaxNightsRP.value = (getTagText(json[j], 'MaxNoches') == '0') ? '' : getTagText(json[j], 'MaxNoches');
            txtAdvancedDaysRP.value = (getTagText(json[j], 'AdvBooking') == '0') ? '' : getTagText(json[j], 'AdvBooking');
            NoArrivals = getTagText(json[j], 'NoArrivals');
            status.innerHTML = getTagText(json[j], 'StatusAvail');

            lblCancelacionRP.innerHTML = getTagText(json[j], 'CancelacionPor');
            TextboxCRP.value = getTagText(json[j], 'CancelacionValor');
            lblMsgCRP.innerHTML = getTagText(json[j], 'MsgCancelacion');

            lblCancelacionRP.style.display = '';

            if (getTagText(json[j], 'CancelacionValor') != '') {
                TextboxCRP.style.display = '';
                lblMsgCRP.style.display = '';
            }

            if (NoArrivals.length > 1) {
                for (i = 0; i <= 6; i++) {
                    var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "RP");                                                       
                    if (NoArrivals.charAt(i) == 'N') {
                        chk.checked = false;
                    }
                    else {
                        chk.checked = true;
                    }
                }
            }
            j++;
        }
    }
}

//Funcion para inicializar la lista de rateplans
function RPCtlInit() {
    var ddlRatePlan = document.getElementById("CtlMensajeRuleConf1_ddlRatePlanConf");
    while (ddlRatePlan.length > 1) {
        ddlRatePlan.remove(1);
    }
    var txtMinNightsRP = document.getElementById("CtlMensajeRuleConf1_txtMinNightsRP");
    var txtAdvancedDaysRP = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysRP");
    var txtMaxNightsRP = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsRP");
    txtMinNightsRP.value = "";
    txtAdvancedDaysRP.value = "";
    txtMaxNightsRP.value = "";
    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "RP");
        if (chk)
            chk.checked = false;
    }
}

function RPCtlInit2() {
    var txtMinNightsRP = document.getElementById("CtlMensajeRuleConf1_txtMinNightsRP");
    var txtAdvancedDaysRP = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysRP");
    var txtMaxNightsRP = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsRP");
    var status = document.getElementById("CtlMensajeRuleConf1_LabelStatRP");
    status.innerHTML = '';
    txtMinNightsRP.value = "";
    txtAdvancedDaysRP.value = "";
    txtMaxNightsRP.value = "";

    var lblCancelacionRP = document.getElementById("CtlMensajeRuleConf1_lblCancelacionRP");
    var TextboxCRP = document.getElementById("CtlMensajeRuleConf1_TextboxCRP");
    var lblMsgCRP = document.getElementById("CtlMensajeRuleConf1_lblMsgCRP");
    TextboxCRP.value = "";
    lblCancelacionRP.style.display = 'none';
    TextboxCRP.style.display = 'none';
    lblMsgCRP.style.display = 'none';

    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "RP");
        if (chk)
            chk.checked = false;
    }
}


//Funcion que agrega un elemento a la lista de rateplans
function AddItemRP(Text, Value) {
    var opt = document.createElement("option");
    opt.text = Text;
    opt.value = Value;
    document.getElementById("CtlMensajeRuleConf1_ddlRatePlanConf").options.add(opt);
}

function AddItemRPH(Text, Value) {
    var opt = document.createElement("option");
    opt.text = Text;
    opt.value = Value;
    document.getElementById("CtlMensajeRuleConf1_ddlRatePlanHotel").options.add(opt);
}

//Funcion que busca las reglas no generales del hotel 
function searchHotelLockRules(fecha, nivelRegla) //Fecha en la que se buscara las reglas
{
    //    fechaFinal = fecha;
    //    if (sbHttp == null) {
    //        sbHttp = new Ajax.Request();
    //        if (!sbHttp.inprogress) {
    //            var url = "LoadAvailabilityRestrictions.ashx";
    //            var dt = new Date();
    //            var qry = "";
    //            qry = "fecha=" + fecha + "&nivelRegla=" + nivelRegla;
    //            var sbHttp = new Ajax.Request(url, { method: 'get', asynchronous: true, parameters: qry, onComplete: LoadLockHotelRules });
    //        }
    //        else {
    //            alert("busy");
    //        }
    //    }
    //    else {
    //        alert("No hizo la invocacion con Ajax");
    //    }

    fechaFinal = fecha;
    $.ajax({
        url: "LoadAvailabilityRestrictions.ashx",
        type: 'GET',
        data: { fecha: fecha, nivelRegla: nivelRegla, dtrequest: dtRequest },
        dataType: 'xml',
        success: function(resp) {
            LoadLockHotelRules(resp);
        }
    });
}

//Funcion que carga los datos de las reglas no generales del hotel
function LoadLockHotelRules(client) {
    var ban = false;
    if (!client.cancelled) {
        var json;
        //var json = client.responseXML.getElementsByTagName("LockHotelRules");
        var json = $(client).find("LockHotelRules");
        var j = 0;
        var i = 0;
        var NoArrivals = "";

        var txtMinNightsLH = document.getElementById("CtlMensajeRuleConf1_txtMinNightsLH");
        var txtAdvancedDaysLH = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysLH");
        var txtMaxNightsLH = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsLH");
        var status = document.getElementById("CtlMensajeRuleConf1_LabelStatLH");

        var lblCancelacionLH = document.getElementById("CtlMensajeRuleConf1_lblCancelacionLH");
        var TextboxCLH = document.getElementById("CtlMensajeRuleConf1_TextboxCLH");
        var lblMsgCLH = document.getElementById("CtlMensajeRuleConf1_lblMsgCLH");


        while (json[j]) {
            ban = true;
            txtMinNightsLH.value = (getTagText(json[j], 'MinNoches') == '0') ? '' : getTagText(json[j], 'MinNoches');
            txtMaxNightsLH.value = (getTagText(json[j], 'MaxNoches') == '0') ? '' : getTagText(json[j], 'MaxNoches');
            txtAdvancedDaysLH.value = (getTagText(json[j], 'AdvBooking') == '0') ? '' : getTagText(json[j], 'AdvBooking');
            NoArrivals = getTagText(json[j], 'NoArrivals');
            status.innerHTML = getTagText(json[j], 'StatusAvail');


            lblCancelacionLH.innerHTML = getTagText(json[j], 'CancelacionPor');
            TextboxCLH.value = getTagText(json[j], 'CancelacionValor');
            lblMsgCLH.innerHTML = getTagText(json[j], 'MsgCancelacion');

            lblCancelacionLH.style.display = '';
            if (getTagText(json[j], 'CancelacionValor') != '') {
                TextboxCLH.style.display = '';
                lblMsgCLH.style.display = '';
            }
            else {
                TextboxCLH.style.display = 'none';
                lblMsgCLH.style.display = 'none';
            }


            if (NoArrivals.length > 1) {
                for (i = 0; i <= 6; i++) {
                    var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "LH");
                    if (NoArrivals.charAt(i) == 'N') {
                        chk.checked = false;
                    }
                    else {
                        chk.checked = true;
                    }
                }
            }
            j++;
        }
        if (!ban) {
            //No mostramos el renglon de los rateplans						
            document.getElementById("TblRuleLockHotel").style.display = 'none';
            document.getElementById("TblNoRuleLockHotel").style.display = '';
        }
        else {
            //Mostramos el reglon de los rateplans
            document.getElementById("TblRuleLockHotel").style.display = '';
            document.getElementById("TblNoRuleLockHotel").style.display = 'none';
        }
    }
    mostrarRules = mostrarRules + 1;
    mostrarR();
}

function hotelLockCtlInit() {
    var txtMinNightsLH = document.getElementById("CtlMensajeRuleConf1_txtMinNightsLH");
    var txtAdvancedDaysLH = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysLH");
    var txtMaxNightsLH = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsLH");

    txtMinNightsLH.value = "";
    txtAdvancedDaysLH.value = "";
    txtMaxNightsLH.value = "";

    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "LH");
        chk.checked = false;
    }
}

function mostrarR() {
    if (mostrarRules == 4) {
        show2("CtlMensajeRuleConf1_PnlBox");
        mostrarRules = 0;
    }
    else {
        //alert(mostrarRules);
    }
}

//Funcion que busca los ratePlans base del hotel
function searchRatePlanHotel(nivelRegla) {
    //    if (sbHttp == null) {
    //        sbHttp = new Ajax.Request();
    //        if (!sbHttp.inprogress) {
    //            var url = "LoadAvailabilityRestrictions.ashx";
    //            var dt = new Date();
    //            var qry = "";
    //            qry = "nivelRegla=" + nivelRegla;
    //            var sbHttp = new Ajax.Request(url, { method: 'get', asynchronous: true, parameters: qry, onComplete: RatePlansHotel });
    //        }
    //        else {
    //            alert("busy");
    //        }
    //    }
    //    else {
    //        alert("No hizo la invocacion con Ajax");
    //    }

    $.ajax({
        url: "LoadAvailabilityRestrictions.ashx",
        type: 'GET',
        data: { nivelRegla: nivelRegla, dtrequest: dtRequest },
        dataType: 'xml',
        success: function(resp) {
            RatePlansHotel(resp);
        }
    });
}

//Funcion que recibe los ratesPlans Base del Hotel
function RatePlansHotel(client) {
    RPHCtlInit();
    if (!client.cancelled) {
        var json;
        //var json = client.responseXML.getElementsByTagName("RatePlans");
        var json = $(client).find("RatePlans");
        var j = 0;
        var rp = false;
        while (json[j]) {
            AddItemRPH(getTagText(json[j], 'texto'), getTagText(json[j], 'id_Rule'));
            //AddItemRPH(getTagText(json[j],'name'),getTagText(json[j],'id_Rule'));			
            rp = true;
            j++;
        }
        if (!rp) {
            //No mostramos el renglon de los rateplans						
            document.getElementById("TblRuleRatePLanHotel").style.display = 'none';
        }
        else {
            //Mostramos el reglon de los rateplans
            document.getElementById("TblRuleRatePLanHotel").style.display = '';
        }
    }
    mostrarRules = mostrarRules + 1;
    mostrarR();
}

function RPHCtlInit() {
    var ddlRatePlanHotel = document.getElementById("CtlMensajeRuleConf1_ddlRatePlanHotel");
    while (ddlRatePlanHotel.length > 1) {
        ddlRatePlanHotel.remove(1);
    }
    var txtMinNightsHRP = document.getElementById("CtlMensajeRuleConf1_txtMinNightsHRP");
    var txtAdvancedDaysHRP = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysHRP");
    var txtMaxNightsHRP = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsHRP");
    txtMinNightsHRP.value = "";
    txtAdvancedDaysHRP.value = "";
    txtMaxNightsHRP.value = "";
    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "HRP");
        chk.checked = false;
    }
    var lbl = document.getElementById("CtlMensajeRuleConf1_lblMsgRatePlanHotel");
    lbl.style.display = 'none';
}

function RPHCtlInit2() {
    var txtMinNightsHRP = document.getElementById("CtlMensajeRuleConf1_txtMinNightsHRP");
    var txtAdvancedDaysHRP = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysHRP");
    var txtMaxNightsHRP = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsHRP");
    txtMinNightsHRP.value = "";
    txtAdvancedDaysHRP.value = "";
    txtMaxNightsHRP.value = "";

    var lblCancelacionHRP = document.getElementById("CtlMensajeRuleConf1_lblCancelacionHRP");
    var TextboxCHRP = document.getElementById("CtlMensajeRuleConf1_TextboxCHRP");
    var lblMsgCHRP = document.getElementById("CtlMensajeRuleConf1_lblMsgCHRP");

    lblCancelacionHRP.innerHTML = "--";
    TextboxCHRP.value = ""
    lblMsgCHRP.innerHTML = "--"

    lblCancelacionHRP.style.display = 'none';
    TextboxCHRP.style.display = 'none';
    lblMsgCHRP.style.display = 'none';


    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "HRP");
        chk.checked = false;
    }
    var lbl = document.getElementById("CtlMensajeRuleConf1_lblMsgRatePlanHotel");
    lbl.style.display = 'none';
}

//Busca la regla del rate plan en caso de tener una asignada
function SearchRatePlanRulesHotel(nivelRegla) {
    //    if (sbHttp == null) {
    //        sbHttp = new Ajax.Request();
    //        if (!sbHttp.inprogress) {
    //            var ddlRatePlan = document.getElementById("CtlMensajeRuleConf1_ddlRatePlanHotel");
    //            var url = "LoadAvailabilityRestrictions.ashx";
    //            var dt = new Date();
    //            var qry = "";
    //            qry = "fecha=" + fechaFinal + "&nivelRegla=" + nivelRegla + "&idRule=" + ddlRatePlan.options[ddlRatePlan.selectedIndex].value;
    //            if (ddlRatePlan.options[ddlRatePlan.selectedIndex].value != "-1") {
    //                var sbHttp = new Ajax.Request(url, { method: 'get', asynchronous: true, parameters: qry, onComplete: LoadRatePlanRulesHotel });
    //            }
    //            else {
    //                RPHCtlInit2();
    //                var lbl = document.getElementById("CtlMensajeRuleConf1_lblMsgRatePlanHotel");
    //                if (ddlRatePlan.selectedIndex > 0) {
    //                    lbl.style.display = '';
    //                }
    //            }
    //        }
    //        else {
    //            alert("busy");
    //        }
    //    }

    var ddlRatePlan = document.getElementById("CtlMensajeRuleConf1_ddlRatePlanHotel");
    var _valueOpRate = ddlRatePlan.options[ddlRatePlan.selectedIndex].value;
    if (ddlRatePlan.options[ddlRatePlan.selectedIndex].value != "-1") {
        $.ajax({
            url: "LoadAvailabilityRestrictions.ashx",
            type: 'GET',
            data: { fecha: fechaFinal, nivelRegla: nivelRegla, idRule: _valueOpRate, dtrequest: dtRequest },
            dataType: 'xml',
            success: function(resp) {
            LoadRatePlanRulesHotel(resp);
            }
        });
    }
    else {
        RPHCtlInit2();
        var lbl = document.getElementById("CtlMensajeRuleConf1_lblMsgRatePlanHotel");
        if (ddlRatePlan.selectedIndex > 0) {
            lbl.style.display = '';
        }
    }
}

//Lee la regla del rate plan., en caso de tener una asignada
function LoadRatePlanRulesHotel(client) {
    RPHCtlInit2();
    var banlocal = false;
    if (!client.cancelled) {
        var json;
        //var json = client.responseXML.getElementsByTagName("RatePlanRuleHotel");
        var json = $(client).find("RatePlanRuleHotel");
        var j = 0;
        var i = 0;
        var NoArrivals = "";
        var ruleDefault = "";

        var txtMinNightsHRP = document.getElementById("CtlMensajeRuleConf1_txtMinNightsHRP");
        var txtAdvancedDaysHRP = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysHRP");
        var txtMaxNightsHRP = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsHRP");
        var lbl = document.getElementById("CtlMensajeRuleConf1_lblMsgRatePlanHotel");

        var lblCancelacionHRP = document.getElementById("CtlMensajeRuleConf1_lblCancelacionHRP");
        var TextboxCHRP = document.getElementById("CtlMensajeRuleConf1_TextboxCHRP");
        var lblMsgCHRP = document.getElementById("CtlMensajeRuleConf1_lblMsgCHRP");

        while (json[j]) {
            ruleDefault = getTagText(json[j], 'RateRulesDefault');
            txtMinNightsHRP.value = (getTagText(json[j], 'MinNoches') == '0') ? '' : getTagText(json[j], 'MinNoches');
            txtMaxNightsHRP.value = (getTagText(json[j], 'MaxNoches') == '0') ? '' : getTagText(json[j], 'MaxNoches');
            txtAdvancedDaysHRP.value = (getTagText(json[j], 'AdvBooking') == '0') ? '' : getTagText(json[j], 'AdvBooking');
            NoArrivals = getTagText(json[j], 'NoArrivals');


            lblCancelacionHRP.innerHTML = getTagText(json[j], 'CancelacionPor');
            TextboxCHRP.value = getTagText(json[j], 'CancelacionValor');
            lblMsgCHRP.innerHTML = getTagText(json[j], 'MsgCancelacion');

            lblCancelacionHRP.style.display = '';
            if (getTagText(json[j], 'CancelacionValor') != '') {
                TextboxCHRP.style.display = '';
                lblMsgCHRP.style.display = '';
            }

            if (NoArrivals.length > 1) {
                for (i = 0; i <= 6; i++) {
                    var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "HRP");
                    if (NoArrivals.charAt(i) == 'N') {
                        chk.checked = false;
                    }
                    else {
                        chk.checked = true;
                    }
                }
            }
            lbl.style.display = "none";
            banlocal = true;
            j++;
        }
        if (banlocal == false) {
            lbl.style.display = "block";
        }
    }
}

function THCtlInit(ban) {
    var txtMinNightsTH = document.getElementById("CtlMensajeRuleConf1_txtMinNightsTH");
    var txtAdvancedDaysTH = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysTH");
    var txtMaxNightsTH = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsTH");

    txtMinNightsTH.value = "";
    txtAdvancedDaysTH.value = "";
    txtMaxNightsTH.value = "";

    for (i = 0; i <= 6; i++) {
        var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "TH");
        chk.checked = false;
    }

    if (ban == 1) {
        var ddlRooms = document.getElementById("CtlMensajeRuleConf1_ddlRooms");
        ddlRooms.selectedIndex = 0;
        var ddlRatePlansRooms = document.getElementById("CtlMensajeRuleConf1_ddlRatePlansRooms");
        ddlRatePlansRooms.selectedIndex = 0;
    }

    var lblmsg = document.getElementById("CtlMensajeRuleConf1_lblMsgRateRoom");
    lblmsg.style.display = "none";
}


//
function SearchRoomRate(nivelRegla) {
//    if (sbHttp == null) {
//        sbHttp = new Ajax.Request();
//        if (!sbHttp.inprogress) {
//            var ddlRatePlansRooms = document.getElementById("CtlMensajeRuleConf1_ddlRatePlansRooms");
//            var ddlRooms = document.getElementById("CtlMensajeRuleConf1_ddlRooms");
//            var url = "LoadAvailabilityRestrictions.ashx";
//            var dt = new Date();
//            var qry = "";
//            qry = "fecha=" + fechaFinal + "&nivelRegla=" + nivelRegla + "&idRatePlan=" + ddlRatePlansRooms.options[ddlRatePlansRooms.selectedIndex].value + "&idRoom=" + ddlRooms.options[ddlRooms.selectedIndex].value;

//            if (ddlRatePlansRooms.options[ddlRatePlansRooms.selectedIndex].value != "-1" && ddlRooms.options[ddlRooms.selectedIndex].value != "-1") {
//                var sbHttp = new Ajax.Request(url, { method: 'get', asynchronous: true, parameters: qry, onComplete: LoadRateRoomRule });
//            }
//            else {
//                THCtlInit(0);
//            }
//        }
//        else {
//            alert("busy");
//        }
//    }

    var ddlRatePlansRooms = document.getElementById("CtlMensajeRuleConf1_ddlRatePlansRooms");
    var ddlRooms = document.getElementById("CtlMensajeRuleConf1_ddlRooms");
    var _valueOp = ddlRatePlansRooms.options[ddlRatePlansRooms.selectedIndex].value;
    var _valueOpRoom = ddlRooms.options[ddlRooms.selectedIndex].value
    if (ddlRatePlansRooms.options[ddlRatePlansRooms.selectedIndex].value != "-1" && ddlRooms.options[ddlRooms.selectedIndex].value != "-1") {
        $.ajax({
            url: "LoadAvailabilityRestrictions.ashx",
            type: 'GET',
            data: { fecha: fechaFinal, nivelRegla: nivelRegla, idRatePlan: _valueOp, idRoom: _valueOpRoom, dtrequest: dtRequest },
            dataType: 'xml',
            success: function(resp) {
            LoadRateRoomRule(resp);
            }
        });
    }
    else {
        THCtlInit(0);
    }
}

function LoadRateRoomRule(client) {

    if (!client.cancelled) {
        THCtlInit(0);
        var json;
        //var json = client.responseXML.getElementsByTagName("RateRoomRules");
        var json = $(client).find("RateRoomRules");
        var j = 0;
        var i = 0;
        var NoArrivals = "";

        var txtMinNightsTH = document.getElementById("CtlMensajeRuleConf1_txtMinNightsTH");
        var txtAdvancedDaysTH = document.getElementById("CtlMensajeRuleConf1_txtAdvancedDaysTH");
        var txtMaxNightsTH = document.getElementById("CtlMensajeRuleConf1_txtMaxNightsTH");
        var lblmsg = document.getElementById("CtlMensajeRuleConf1_lblMsgRateRoom");
        var banlocal = false;
        var ruleDefault = "";
        while (json[j]) {
            ruleDefault = getTagText(json[j], 'RateRulesDefault');
            txtMinNightsTH.value = (getTagText(json[j], 'MinNoches') == '0') ? '' : getTagText(json[j], 'MinNoches');
            txtMaxNightsTH.value = (getTagText(json[j], 'MaxNoches') == '0') ? '' : getTagText(json[j], 'MaxNoches');
            txtAdvancedDaysTH.value = (getTagText(json[j], 'AdvBooking') == '0') ? '' : getTagText(json[j], 'AdvBooking');
            NoArrivals = getTagText(json[j], 'NoArrivals');
            if (NoArrivals.length > 1) {
                for (i = 0; i <= 6; i++) {
                    var chk = document.getElementById("CtlMensajeRuleConf1_Chk" + (i + 1) + "TH");
                    if (NoArrivals.charAt(i) == 'N') {
                        chk.checked = false;
                    }
                    else {
                        chk.checked = true;
                    }
                }
            }
            lblmsg.style.display = "none";
            banlocal = true;
            j++;
        }
        if (banlocal == false) {
            lblmsg.style.display = "block";
        }
    }
}
