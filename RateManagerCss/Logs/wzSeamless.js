var xmlhttp, xmlhttp1, logs;
var factor, ini, fin, sIni;
var j, pb;
var porc;

function procesarLogs(){
	sIni = document.getElementById('txtInicio').value;
	ini = new Date(sIni);
	fin = new Date(document.getElementById('txtFin').value);
	if (fin < ini)
		fin = ini;
	logs = Math.abs(Math.round((ini-fin)/86400000));
	pb = document.getElementById('progreso');
	porc=0;
	factor = (logs==0)?100:100/logs;
	j=1;
	loadXMLDoc('exporter.aspx?ini=' + sIni + '&logs=' + logs + '&ind=' + j);
}

function stateChange()
{
	if (xmlhttp.readyState==4){// if xmlhttp shows "loaded"
		if (xmlhttp.status==200){// if "OK"
			var procesados = xmlhttp.responseText;
			porc=Math.round(procesados * factor);
			pb.style.width = porc + '%';
			if (j<=logs)
			{
				j++;
				pb.innerText=porc + ' %';
				loadXMLDoc('exporter.aspx?ini=' + sIni + '&logs=' + logs + '&ind=' + j);
			}else{
				var dHoy, sHoy;
				var d,m,y;
				pb.innerText='100 %';
				dHoy= new Date();
				d=dHoy.getDate();
				d=(d<10)?'0'+d:d;
				m=dHoy.getMonth()+1;
				m=(m<10)?'0'+m:m;
				y=dHoy.getYear();
				sHoy=m+'/'+d+'/'+y;
				alert('Proceso terminado.');
				document.getElementById('txtInicio').value = sHoy;
				document.getElementById('txtFin').value = sHoy;
				pb.innerText='0 %';
				pb.style.width='0%'
			}
		}
		else {
			alert(xmlhttp.status)
		}
	}
}

function loadXMLDoc(url)
{
	xmlhttp=null
	if (window.XMLHttpRequest){// code for Mozilla, etc.
		xmlhttp=new XMLHttpRequest()
	}
	else if (window.ActiveXObject){// code for IE
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP")
	}
	if (xmlhttp!=null){
		xmlhttp.onreadystatechange=stateChange
		xmlhttp.open("POST",url,true)
		xmlhttp.send(null)
	}
	else{
		alert("El navegador no soporta el objeto xmlHTTP.")
	}
}