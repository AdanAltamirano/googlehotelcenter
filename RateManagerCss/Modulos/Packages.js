	function ShowDetails(divId, display){
				var div = document.getElementById(divId);
				if (div){
					div.style.display = display;
				}
				/*if (display==''){
				(document.getElementById(lstDatesId)).style.display='none';}else{(document.getElementById(lstDatesId)).style.display='';
				}*/
			}
	function FillException(Exc,idx,chk)
  {
	var e = document.getElementById(chk)	
	if (Exc.substring(idx-1,idx)=='Y') e.checked=true;
	else e.checked=false;
  }
  function CancelEditDgOcupancyRate(e1,e2,e3,e4,e5,e6,e7,btnCancel,btnAdd,rooms) {
   
		var str = (document.getElementById('iChkEditId')).value;
		if (str!="")
		{
		var chk =document.getElementById(str);
		if (chk){chk.checked=false;}
		UnSelectDg(str);
		}
					
		ClearEditedIds();
		var txtCancel = document.getElementById(btnCancel);		 
		txtCancel.style.display='none';		
		ClearInputPrices('tblPrices');
		ClearInputPrices('tblPricesExc');
		ClearExceptions(e1, e2, e3, e4, e5, e6, e7);
		
		var containerRef = document.getElementById(rooms);
		if (containerRef) containerRef.style.display = '';
		var cmd$ = document.getElementById(btnAdd);
		if (cmd$) {
		    cmd$.value = resource01026;
		}
}
   
   function UnSelectDg(str)
    {
     var grid = document.getElementById(dgRoomsId);	
		 
		 if (grid)
			{	 
			
				var item = grid.getElementsByTagName("tr");										
				var items=0;
				for (var i = 1; i < item.length; i++)				
				{		
					var tds = item[i].getElementsByTagName("td");			
					var chkEdit = tds[5].getElementsByTagName("input");								
					
					if (chkEdit)
					{  var sw=false;
						for (var j=0;j<=chkEdit.length-1 && sw==false;j++)
						 {
							if (chkEdit[j].type=='checkbox')
								{sw=true;
									if (chkEdit[j].id==str){item[i].className="dgItem";}
								}
						 }
					}					
					  			
				}				
			}
	
    }
  
 
   function AddRatei(index, i,txt,input)
    {
		
		if (txt.value.split(",").length>i+index)
		 {
			 input.value = txt.value.split(",")[i+index];
		 }
		else
		 {
		 	 input.value = txt.value.split(",")[txt.value.split(",").length-1];
		 }
    }
    
  
     
     
function IsValidDate(d)
 {
	if (d.value=="")
	 {
		return false; 
	 }
	var str = d.value.split("/");
	if (str.length!=3){return false;}
	var gDays=[31,31,28,31,30,31,30,31,31,30,31,30,31];
	var year = str[2];	
	if (isNumeric(year,0)==false || eval(year)<eval(NowYear)) {return false}
	var month = str[0];	
	if (isNumeric(month,0)==false || eval(month)<1 || eval(month)>12) {return false}
	var MaxDay=(gDays[eval(month)]);
	if (eval(month)==2){MaxDay=year%4==0&&year%100!=0||year%400==0?29:28;}
	var day =str[1]
	if (isNumeric(day,0)==false || eval(day)<1 || eval(day)>MaxDay) {return false}

	return true;
 }
	
	function onCheckBoxesClick(chk, td){
	
		var c = document.getElementById(chk);
		var t = document.getElementById(td);
						 
		if (c && t){			
			t.style.display = c.checked ? "" : "none";			
		}
		return 0;
	}
	
	 
function ShowOccupation(ocup,adults)
  {
		var rdbO = document.getElementById(ocup);	   
		var txtA = document.getElementById(adults);
		var p = document.getElementById("dvPrices")		
	  if (rdbO.checked) 
	   {			
			p.style.display='';						
	   }
	  else
	   {
	   	 p.style.display='none';					
	   }
	 
  }
  
  
  function CreateDsOcupattion(rdb,tbl,txt,btnAdd)
	 {
		var rdbO = document.getElementById(rdb);
		var txtO = document.getElementById(txt);
		var o=0;
		if (isNumeric(txtO.value,0)){o=txtO.value;}
		if (o>0)
		 {			
			var ni = document.getElementById(tbl);
			var inputs=ni.getElementsByTagName('input');
			
			if (o>inputs.length)
			{
				var aux=inputs.length;
				if (inputs.length>0)
				o = o-inputs.length;
				for (var i=0;i<o;i++)
				{			
				var lbl = document.createElement('span');							
				lbl.innerHTML= i + 1 + aux;				
				lbl.setAttribute('id',ni.id + 'lbl' + (inputs.length));							
				ni.appendChild(lbl);			
				var newinput = document.createElement('input');
				
				newinput.setAttribute("size","8");
				newinput.setAttribute("maxlength","20");

				newinput.setAttribute('id',ni.id + (inputs.length));			
				ni.appendChild(newinput);
				var dv= document.createElement('div');
				dv.setAttribute('id',ni.id + 'dv' + (inputs.length));			
				ni.appendChild(dv);
				}
			}
			else
			 {			 		
			 		for (var i=inputs.length;i>o;i--)
			 		 {			
			 			var olddiv = document.getElementById(ni.id + (i-1));			 			
						ni.removeChild(olddiv);
						olddiv = document.getElementById(ni.id + 'lbl' + (i-1));
						ni.removeChild(olddiv);
					}
						
			 }
			document.getElementById(btnAdd).style.display='';
		 }
		 else
		  {
			document.getElementById(btnAdd).style.display='none';
		  }
	 }

function isNumeric(cadena,ValidPunto)
{
	var strValidChars = "0123456789";
	if (ValidPunto==1){strValidChars = "0123456789.";}
    var blnResult = true;
    var strChar;
    var i;
    var punto = false;
	for (i = 0; i < cadena.length && blnResult == true; i++)
      {		
			strChar = cadena.charAt(i); 
			
			if (strValidChars.indexOf(strChar) == -1)
			{         
				return false;
			}     
			if (strChar == ".")
			 {
				if (punto==true){return false;}
				else {punto=true;}
			 }			
      }
     return blnResult;
}

	
	function optionSw(e,tx,dvA,dvB,tdA,tdB)		
	{
		var div1A = document.getElementById(dvA);
		var div2B = document.getElementById(dvB);	   
		var td1A = document.getElementById(tdA);
		var td1B = document.getElementById(tdB);
		var txt = document.getElementById(tx);

		txt.value = e;

		switch (e)
		{
			case '1P':														
				div1A.style.display='';
				div2B.style.display="none";		
				td1A.className= 'tabselected';
				td1B.className= 'tab';
				break;
			
			case '1E':
				div1A.style.display="none";							
				div2B.style.display='';							
				td1A.className= 'tab';
				td1B.className= 'tabselected';
				break;														
		}    
		return true;			
	}
	
	function FillPrices(Dg,typeFare,Price)
	{					
		var P = document.getElementById(Price);					
		var grid = document.getElementById(Dg);
		var item = grid.getElementsByTagName("tr");					
					
		for (var i = 1; i < item.length; i++)
		{						
			var txt = item[i].getElementsByTagName("input");
			var lbl = item[i].getElementsByTagName("span");
						
			for (var j = 0; j < lbl.length; j++)
			{				
				for (var t = 0; t < txt.length; t++)
				{							
					if (txt[t].id.indexOf(typeFare) != -1)
					{
						txt[t].value = P.value;
					}					
				}
									
				if (lbl[j].id.indexOf("lblTotal") != -1)
				{	
					if (txt.length>1)
					{
						sP(txt[0].id,txt[1].id,lbl[j].id);
					}
					else
					{
						sP(txt[0].id,'',lbl[j].id);
					}								
				}					
			}					
		}
	}
						

  function  OverlapDateRoomFromDg(dg,habs,NewDt1,NewDt2)
  {
	var sw=false;
	
	var grid = document.getElementById(dg);
	
	if (grid)
	{	
	var item = grid.getElementsByTagName("tr");										
	for (var i = 1; i < item.length && sw ==false; i++)
	 {
	 		var tds = item[i].getElementsByTagName("td");			
		//	var chkEdit = tds[5].getElementsByTagName("input");				
			var chkDelete = tds[4].getElementsByTagName("input");				
			var xDia;var xMes; var xYear;
			var xDia2;var xMes2; var xYear2;
			//var IStart= tds[2].getElementsByTagName("input");
			//var IEnd= tds[2].getElementsByTagName("input");
			if (chkDelete.length>0)
			{
				if(chkDelete[0].checked == false)
				{
						var txtstart; var txtEnd;									
						txtstart =tds[1].innerHTML; txtEnd = tds[2].innerHTML;
					
						xDia = txtstart.substring(3,5); xMes = txtstart.substring(0,2);
						xMes = xMes - 1; xYear =txtstart.substring(6,10); xYear = xYear;
						
						xDia2 = txtEnd.substring(3,5); xMes2 = txtEnd.substring(0,2);
						xMes2 = xMes2 - 1; xYear2 =txtEnd.substring(6,10); xYear2 = xYear2;
					var f1 =  new Date (xYear, xMes, xDia);	f1 = Date.parse(f1);	 
					var f2 = new Date (xYear2, xMes2, xDia2); f2 = Date.parse(f2);						
					
				if (((NewDt1 >= f1 && NewDt1 <= f2) || ((NewDt2 >= f1) && NewDt2 <= f2)) || ((f1 >= NewDt1 && f1 <= NewDt2) || ((f2 >= NewDt1) && f2 <= NewDt2)))
				{
				    if ((habs.indexOf("," + tds[0].innerHTML + ",")) >= 0) { sw = false; }						
				} 

				}
			 }
			
	 }
	}
	
	return sw;
  }

   
   function ArePriceOccValid(Exc)
   {
    var Valid = IsRatesValid('tblPrices');
			 if (Valid!=0)
			  {
			   alert (InvalidAdRate + " " + Valid)
			   return false;
			  }
			  //si definió excepcion			  
			  
			
			 if (Exc!='NNNNNNN')
			  {
				Valid = IsRatesValid('tblPricesExc');
				if (Valid!=0)
				{
				alert (InvalidAdExcRate + " " + Valid)
				return false;
				}
			  }
			  return true;
   }
function addOcupancyRate(Price,PriceE,hab,e1,e2,e3,e4,e5,e6,e7,stDt,edDt,lstDates,txtFechas,dgRooms,PkStart,PkEnd,Occ,Paq,Pers,btnCancel,btnAdd)
 {
     var rdbOc = document.getElementById(Occ);
     var containerRef = document.getElementById(hab);
     if (containerRef) containerRef.style.display = '';
     var cmd$ = document.getElementById(btnAdd);
     if (cmd$) cmd$.value = resource01026;
     
	if (rdbOc.checked==false  && (document.getElementById('iChkEditId')).value=="")
	 {	 
		AddDate(stDt,edDt,lstDates,txtFechas,hab,dgRooms,btnCancel);		
		return 0;
	 }
 		var Exc="";
		Exc = Exc+ GetDayExc(e1);
		Exc = Exc+ GetDayExc(e2);
		Exc = Exc+ GetDayExc(e3);
		Exc = Exc+ GetDayExc(e4);
		Exc = Exc+ GetDayExc(e5);
		Exc = Exc+ GetDayExc(e6);
		Exc = Exc+ GetDayExc(e7);
 
	if ((document.getElementById('iChkEditId')).value!="")
	 {
	  
		var strdgPrice = (document.getElementById('itxtPriceEdit')).value;
		var strdgPriceE = (document.getElementById('itxtPriceEEdit')).value;	
		var strdgTipoPaquete = (document.getElementById('itxtTipoPaqueteEdit')).value;	
		
	
		
		 if (rdbOc.checked==false)
		  {	  
			 
			var txtTotNig = document.getElementById(txtNightsId);
			var txtAd =  document.getElementById(txtMaxAdId);
			var txtPre= document.getElementById(txtPrecioId);
  
			if (txtPre.value =="" || isNumeric(txtPre.value,1)==false) {alert("precio no valido");return 0;}
			if (txtTotNig.value =="" || isNumeric(txtTotNig.value,0)==false) {alert("Noches no valido");return 0;}
			if (txtAd.value =="" || isNumeric(txtAd.value,0)==false) {alert("adultos no valido");return 0;}	
  
			var txtdgPrice= document.getElementById(strdgPrice);
			txtdgPrice.value = "$" + document.getElementById("itxtCodigoEdit").value + ",,," + GetRateByPaqOrPer(0);
			txtdgPrice= document.getElementById(strdgPriceE);
			txtdgPrice.value = "$" + document.getElementById("itxtCodigoEdit").value + ",,,NNNNNNN," + GetRateByPaqOrPer(1);		 
			
		  }
		 else
		 {
			if (ArePriceOccValid(Exc)==false)
				 {
					return 0;
				 }
			var txtdgPrice= document.getElementById(strdgPrice);
			txtdgPrice.value = "$" + document.getElementById("itxtCodigoEdit").value + ",,," + GetRate('tblPrices');
			txtdgPrice= document.getElementById(strdgPriceE);
			txtdgPrice.value = "$" + document.getElementById("itxtCodigoEdit").value + ",,," + Exc + "," + GetRate('tblPricesExc');		 
		 }
		 
		 
		 	var rdbPaq = document.getElementById(rdbPaqId);
			var rdbPer = document.getElementById(rdbPerId);		
			var txtPre= document.getElementById(txtPrecioId);
			var ddl = document.getElementById(ddlTypePrecioId);
			var txtTipoprecio =document.getElementById(strdgTipoPaquete);
			var TipoPrecio ="";
			if (ddl.selectedIndex==0){TipoPrecio="N";}else{TipoPrecio="T";}								
			if (rdbPaq.checked==true){ TipoPrecio = TipoPrecio + "0";}
			else if(rdbPer.checked==true){TipoPrecio = TipoPrecio + "1";}
			else {TipoPrecio = TipoPrecio + "2";} 							
			txtTipoprecio.value = "$" + document.getElementById("itxtCodigoEdit").value + ",,," + TipoPrecio + "," + txtPre.value;
		 
		 
		ClearEditedIds();
		ClearInputPrices('tblPrices');
		ClearInputPrices('tblPricesExc');		
		ClearExceptions(e1,e2,e3,e4,e5,e6,e7);
		var txtCancel = document.getElementById(btnCancel);		 
		txtCancel.style.display='none';	
		return 0;
	 }
		 
 
		var habs=",";				
		//var chkTemp = document.getElementById(appT);
		var Inicio =  document.getElementById(stDt);
		var Fin =  document.getElementById(edDt);
		var lst = document.getElementById(lstDates);
		var txtDates= document.getElementById(txtFechas);
		
			if 	(IsValidDate(Inicio)==false){alert (InvalidDate);return 0;}
			if 	(IsValidDate(Fin)==false){alert (InvalidDate);return 0;}
			var NewDt1 = GetDate(Inicio);
			var NewDt2 = GetDate(Fin);
	
		
	 	var tdsBody = document.getElementById(hab).getElementsByTagName('td');	
				for (var i=0;i<=tdsBody.length-1; i++)
				{
						var chk = tdsBody[i].childNodes[0];
						if (chk.checked)
						{					
							var lbl = tdsBody[i].childNodes[1];							
							habs =  habs +  lbl.innerHTML + ",";							
						}
				}
		if (habs==",")
			 {
				alert(SpecifyRoom);
				return 0;
			 }
			
			if (ArePriceOccValid(Exc)==false)
			 {
			  return 0;
			 }	
		  
		var str="";		
		var p = document.getElementById(Price);
		var pE = document.getElementById(PriceE);
		
		for (var i=0;i<habs.split(",").length-1; i++)
			 {
				if (habs.split(",")[i]!='')
				 {	
					
						if (OverlapDateRoom(lst,habs,NewDt1,NewDt2)==true || OverlapDateRoomFromDg(dgRooms,habs,NewDt1,NewDt2)==true)
						 {
						  alert(Overlapped);return 0;
						 }	 					 
					
				 }
			}	
			
			AddlstDateRoom(habs,Inicio,Fin,txtDates,lst);						
	
		for (var i=0;i<habs.split(",").length-1; i++)
			 {
				var texto ="";
				var textoE ="";
				if (habs.split(",")[i]!='')
				 {							 
							texto= "$" + habs.split(",")[i] + "," + Inicio.value + "," +  Fin.value + "," + GetRate('tblPrices');
							textoE = "$" + habs.split(",")[i] + "," + Inicio.value + "," + Fin.value + "," + Exc + "," + GetRate('tblPricesExc');
					
					p.value = p.value + texto;														
					pE.value = pE.value + textoE;					
					
					var txtTipoprecio = document.getElementById(txtTipoPrecioId);
					txtTipoprecio.value = txtTipoprecio.value + "$" + habs.split(",")[i] + "," + Inicio.value + "," +  Fin.value + "," + "N2" + "," + GetRate('tblPrices').split(",")[0];
					 
													
				 }
			 }		 			 
		ClearRooms(hab);
		ClearInputPrices('tblPrices');
		ClearInputPrices('tblPricesExc');		
		ClearExceptions(e1,e2,e3,e4,e5,e6,e7);	
		var txtCancel = document.getElementById(btnCancel);		 
		txtCancel.style.display='none';				
 }
 
 function GetDayExc(D)
  {
  	  var day = document.getElementById(D);
			  if (day.checked==true)
			   {
				return 'Y';
			   }
			  else
			   {
			    return 'N';
			   }
  }
 
 function GetRate(tbl)
  {
		tbl = document.getElementById(tbl);
		var inputs = tbl.getElementsByTagName('input');
		var precios = "";
		for (var i=0;i<=inputs.length-1;i++)
		 {
			if (inputs[i].value ==""){inputs[i].value=0;}
			precios = precios + inputs[i].value 
			if (i<inputs.length-1) precios = precios + ",";		
		 }
		 return precios;
  }
  function GetRateByPaqOrPer(Exc)
  {
		var rdbPaq = document.getElementById(rdbPaqId);
		var rdbPer = document.getElementById(rdbPerId);
		var txtAd =  document.getElementById(txtMaxAdId);
		var txtPre= document.getElementById(txtPrecioId);
		var ddl = document.getElementById(ddlTypePrecioId);
		var txtTotNig = document.getElementById(txtNightsId);
		var precios = "";
		var precio;
	
		for (var i=0;i<=eval(txtAd.value)-1;i++)
		 {
		  precio=0;
		  if (Exc==0)
		  {
			if (rdbPaq.checked==true) 
			 {
				if (ddl.selectedIndex==0){precio = txtPre.value;}
				else{precio = txtPre.value/txtTotNig.value;}
			 }
			else if(rdbPer.checked==true)
			 {
				if (ddl.selectedIndex==0){precio = txtPre.value * (i+1);}
				else{precio = (txtPre.value/txtTotNig.value) * (i+1);}
			 }	
			}		
			precios = precios + precio;
			if (i<eval(txtAd.value)-1) precios = precios + ",";		
		 }
		 return precios;
  }
 function IsRatesValid(tbl)
 {
		tbl = document.getElementById(tbl);
		var inputs = tbl.getElementsByTagName('input');
		
		for (var i=0;i<=inputs.length-1;i++)
		 {
			if (inputs[i].value=='' || isNumeric(inputs[i].value,1)==false || inputs[i].value<1)
			 {				
				return i +1;
			 }	
			
		 }
		 return 0;
 }
 
 

function DeleteDate(lst,tD,tP,tPE)
{
	var e = document.getElementById(lst);
	//var chk = document.getElementById(chkT);
	var txtP =document.getElementById(tP);
	var txtPE =document.getElementById(tPE);	
	
		var txtDates =document.getElementById(tD);
		if (e && e.selectedIndex > 0 && txtDates)
		{
		var Dates = txtDates.value.split("$");
		txtDates.value = "";				 	 
		for (i=1;i<=Dates.length-1;i++)
		{
			if (i!=e.selectedIndex)
			{		 
				txtDates.value = txtDates.value + "$" + Dates[i];
			}
			
		}	 
		var f1 = e.value.split("-")[0]; 
		var str = e.value.split("-")[1]; 
		var f2 = str.split(":")[0];
		var room = str.split(":")[1];
		var Prices = txtP.value.split("$");
		var PricesE = txtPE.value.split("$");
		txtP.value = "";		 
		txtPE.value = "";		 
		for (i=1;i<=Prices.length-1;i++)
		{				
			if (Prices[i].indexOf(room + "," + f1 + "," + f2 + ",")<0)
			{		 
			txtP.value = txtP.value + "$" + Prices[i];
			txtPE.value = txtPE.value + "$" + PricesE[i];
			}
		}	 
		
		var txtTiposPrecios = document.getElementById(txtTipoPrecioId);
		
		var TiposPrecios = txtTiposPrecios.value.split("$");
		txtTiposPrecios.value="";
		for (i=1;i<=TiposPrecios.length-1;i++)
		{				
			if (TiposPrecios[i].indexOf(room + "," + f1 + "," + f2 + ",")<0)
			{		 
			txtTiposPrecios.value = txtTiposPrecios.value + "$" + TiposPrecios[i];			
			}
		}	 
		
		
		e.options[e.selectedIndex] = null;	 	  		
		}
	
	
}

function GetDate(txt)
 {
 	xDia = txt.value.substring(3,5);			
	xMes = txt.value.substring(0,2);	
	xMes = xMes - 1;		
	xYear = txt.value.substring(6,10);
	return new Date (xYear, xMes, xDia);
 
 }
 function  OverlapDateRoom(e,habs,NewDt1,NewDt2)
  {
	var sw=false;
  	for (i = 1; i <= e.options.length -1 && sw ==false; i ++ )
			 {	
				
				xDia = e.options[i].value.substring(3,5);				 			
				xMes = e.options[i].value.substring(0,2);
				xMes = xMes - 1;		
				xYear = e.options[i].value.substring(6,10);
				xYear = xYear;	
				var f1 = new Date (xYear, xMes, xDia);
				
				f1 = Date.parse(f1);
				
				xDia = e.options[i].value.substring(14,16);				 			
				xMes = e.options[i].value.substring(11,13);
				xMes = xMes - 1;		
				xYear = e.options[i].value.substring(17,21);
				xYear = xYear;	
				var f2 = new Date (xYear, xMes, xDia);								
				f2 = Date.parse(f2);			
				if (((NewDt1 >= f1 && NewDt1 <= f2) || ((NewDt2 >= f1) && NewDt2 <= f2)) || ((f1 >= NewDt1 && f1 <= NewDt2) || ((f2 >= NewDt1) && f2 <= NewDt2)))
				{				    
					if (habs.indexOf("," + e.options[i].value.substring(22,e.options[i].value.length) + ",")!= -1){sw = true;}
				}                
			 }
	return sw;
  }

  function AddlstDateRoom(habs,txt1,txt2,txtDates,e)
  {
  var rdbPer= document.getElementById(rdbPerId);
  var rdbPaq= document.getElementById(rdbPaqId);
  var p = document.getElementById(txtPriceId);  
  var pE = document.getElementById(txtPriceEId);
  var txtTotNig = document.getElementById(txtNightsId);
  var txtAd =  document.getElementById(txtMaxAdId);
  var txtPre= document.getElementById(txtPrecioId);
  var txtTipoprecio = document.getElementById(txtTipoPrecioId);
  
  if (rdbPer.checked==true || rdbPaq.checked==true)
   {
		if (txtPre.value =="" || isNumeric(txtPre.value,1)==false) {alert("precio no valido");return 0;}
		if (txtTotNig.value =="" || isNumeric(txtTotNig.value,0)==false) {alert("Noches no valido");return 0;}
		if (txtPre.value<=1) {alert("precio no valido");return 0;}
   }
  
  if (txtAd.value =="" || isNumeric(txtAd.value,0)==false) {alert("adultos no valido");return 0;}	
  
  
  for (var i=0;i<habs.split(",").length-1; i++)
			 {
				if (habs.split(",")[i]!='')
				 {
				 	var texto = txt1.value + "-" + txt2.value + ":" + habs.split(",")[i];
					var optionObject = new Option(texto,texto);
					var optionRank = e.options.length;
					e.options[optionRank]=optionObject;				
					txtDates.value = txtDates.value + "$" + texto;
					if (rdbPer.checked==true || rdbPaq.checked==true)
					 {
						var texto= "$" + habs.split(",")[i] + "," + txt1.value  + "," + txt2.value + "," + GetRateByPaqOrPer(0);
						var textoE= "$" + habs.split(",")[i] + "," + txt1.value  + "," + txt2.value + ",NNNNNNN," + GetRateByPaqOrPer(1);
						p.value = p.value + texto;														
						pE.value = pE.value + textoE;
						
							var rdbPaq = document.getElementById(rdbPaqId);
							var rdbPer = document.getElementById(rdbPerId);		
							var txtPre= document.getElementById(txtPrecioId);
							var ddl = document.getElementById(ddlTypePrecioId);
							var TipoPrecio ="";
							if (ddl.selectedIndex==0){TipoPrecio="N";}else{TipoPrecio="T";}								
							if (rdbPaq.checked==true){ TipoPrecio = TipoPrecio + "0";}
							else if(rdbPer.checked==true){TipoPrecio = TipoPrecio + "1";}							
							txtTipoprecio.value = txtTipoprecio.value + "$" + habs.split(",")[i] + "," + txt1.value  + "," + txt2.value + "," + TipoPrecio + "," + txtPre.value;						
					 }					
				 }
			 }		 
  }
function AddDate(t1,t2,lst,tD,hab,dgRooms,btnCancel)
{

   var txt1 = document.getElementById(t1);
   var txt2 = document.getElementById(t2);
   var txtDates =document.getElementById(tD);
   var e = document.getElementById(lst);
   var xDia,xMes,xYear,i,sw;
   var habs=",";
   
   //Debe haber al menos una habitación seleccionada
 var containerRef = document.getElementById(hab);   
 var tdRefArray = containerRef.getElementsByTagName('td');

  
   if (txt1 && txt2 && e && txtDates)
    {
			
			 if (IsValidDate(txt1)==false || IsValidDate(txt2)==false){alert(InvalidDate);return 0;}		
			
				var tdsBody = document.getElementById(hab).getElementsByTagName('td');	
				for (var i=0;i<=tdsBody.length-1; i++)
				{
						var chk = tdsBody[i].childNodes[0];
						if (chk.checked)
						{
						
							var lbl = tdsBody[i].childNodes[1];
							
							habs =  habs +  lbl.innerHTML + ",";							
						}
				}
				
			if (habs==",")
			 {
				alert(SpecifyRoom);
				return 0;
			 }			
			var NewDt1 = GetDate(txt1);
			
			var NewDt2 = GetDate(txt2);
			
			sw = false; 
			
		    //Set 1 day in milliseconds
		     var one_day=1000*60*60*24;
		     
		    //Calculate difference btw the two dates, and convert to days		
		    one_day = (NewDt2.getTime() -NewDt1.getTime() ) / (one_day);

            NewDt1 = Date.parse(NewDt1);
            NewDt2 = Date.parse(NewDt2);
			var conf;			
			if (eval(one_day)<=3) 
			  {		
			    conf = window.confirm(updateSeasson);			 
				//alert(conf);
				if (! conf)
				  {sw = true; 
				   return;				  
				  }
			  }

			if (NewDt2 < NewDt1)
			 {
				alert(InvalidDate);
				sw = true;
				return;
			 }
			 
			sw = OverlapDateRoom(e,habs,NewDt1,NewDt2);
			
			 if (!sw)
			 {		
				sw = OverlapDateRoomFromDg(dgRooms,habs,NewDt1,NewDt2);
				if (!sw){
					AddlstDateRoom(habs,txt1,txt2,txtDates,e);	 
					}
				else{alert(Overlapped);return;}								
			 }
			else
			{
				alert(Overlapped);return;
			}
			
	//al terminar limpiar las habitaciones
		ClearRooms(hab);
		var txtCancel = document.getElementById(btnCancel);		 
		txtCancel.style.display='none';	
		
    }   
}


 	 function optionSw(e)		
			{
			   var div1A;
			   var div2B;	   
			   var td1A;
			   var td2B;

			
					div1A= document.getElementById('divA2');
					div2B= document.getElementById('divB2');
					td1A= document.getElementById('TdPricing');
					td1B= document.getElementById('TdPricingE');
					 switch (e)
					  {
						case '1P':														
							
							div1A.style.display='';
							div2B.style.display="none";		
							
							td1A.className= 'tabselected';
							td1B.className= 'tab';
							break;
						case '1E':
							
							div1A.style.display="none";							
							div2B.style.display='';							
							
							
							td1A.className= 'tab';
							td1B.className= 'tabselected';
							break;														
					  }    
					  return true;			
			}
			
	

 function ClearInputPrices(tbl)
  {

		tbl = document.getElementById(tbl);
		var inputs = tbl.getElementsByTagName('input');
		var precios = "";
		for (var i=0;i<=inputs.length-1;i++)
		 {
			inputs[i].value ="";			
		 }	 
  }
  




 function ClearExceptions(e1,e2,e3,e4,e5,e6,e7)
 {

	(document.getElementById(e1)).checked=false;
	(document.getElementById(e2)).checked=false;
	(document.getElementById(e3)).checked=false;
	(document.getElementById(e4)).checked=false;
	(document.getElementById(e5)).checked=false;
	(document.getElementById(e6)).checked=false;
	(document.getElementById(e7)).checked=false;
 }
 

 function ClearRooms(hab)
{
		var tdsBody = document.getElementById(hab).getElementsByTagName('td');	
		for (var i=0;i<=tdsBody.length-1; i++)
		{
			var chk = tdsBody[i].childNodes[0];
			if (chk.checked)
			 {chk.checked = false}
		}
} 

  function ClearEditedIds()
   {

   		(document.getElementById('iChkEditId')).value ="";
		(document.getElementById('itxtPriceEdit')).value="";
		(document.getElementById('itxtPriceEEdit')).value="";
		(document.getElementById('itxtCodigoEdit')).value="";
		(document.getElementById('dvtitleEdit')).innerHTML ="";
		(document.getElementById('itxtCodigoEdit')).value="";
		(document.getElementById('itxtTipoPaqueteEdit')).value="";
		
   }
    function EditDgOcupancyRate(Price,PriceE,e1,e2,e3,e4,e5,e6,e7,dgchkEdit,CodHab,btnCancel,TipoPaquete,TipoPaqueteM,cmd,chkRooms)
   {
       var Edit = document.getElementById(dgchkEdit);
       var cmd$ = document.getElementById(cmd);
       var chkRooms$ = document.getElementById(chkRooms);
       if (cmd$) {
           cmd$.value = Edit.checked ? resource00094 : resource01026;
       }
       if (chkRooms$) {
           chkRooms$.style.display = Edit.checked ? 'none' : '';
       }
       
   if (Edit.checked==false)
    {
    UnSelectDg(dgchkEdit);
     return 0;
 }

    
    var ddl = document.getElementById(ddlTypePrecioId);    
    var txtPre= document.getElementById(txtPrecioId);
    
    var str = (document.getElementById(TipoPaqueteM)).value.split(",")[3];
    if (str.substring(0,1)=='T'){ddl.selectedIndex=1;}
    else{ddl.selectedIndex=0;}  
    if(str.substring(1,2)==0){
    (document.getElementById(rdbOccId)).checked=false;
    (document.getElementById(rdbPerId)).checked=false;
    (document.getElementById(rdbPaqId)).checked=true;txtPre.value = (document.getElementById(TipoPaqueteM)).value.split(",")[4];
    }
    else if (str.substring(1,2)==1){
    (document.getElementById(rdbOccId)).checked=false;
    (document.getElementById(rdbPerId)).checked=true;
    (document.getElementById(rdbPaqId)).checked=false;txtPre.value = (document.getElementById(TipoPaqueteM)).value.split(",")[4];
    }
    else{
    (document.getElementById(rdbOccId)).checked=true;
    (document.getElementById(rdbPerId)).checked=false;
    (document.getElementById(rdbPaqId)).checked=false;
    }    
    
    ShowOccupation(rdbOccId,txtMaxAdId,dgRoomsId);
    
    
   		tbl = document.getElementById('tblPrices');
		var inputs = tbl.getElementsByTagName('input');		
		var txtPE = document.getElementById(PriceE);
		var txtP = document.getElementById(Price);		
		
		(document.getElementById('dvtitleEdit')).innerHTML = TitleEditing + ' ' + CodHab;
		for (var i=0;i<=inputs.length-1;i++)
		 {
			AddRatei(3,i,txtP,inputs[i]);			
		 }
		tbl = document.getElementById('tblPricesExc');
		var inputs = tbl.getElementsByTagName('input');		
		for (var i=0;i<=inputs.length-1;i++)
		 {
			AddRatei(4,i,txtPE,inputs[i]);			
		 }
		 var Exc = txtPE.value.split(",")[3]	 
		 FillException(Exc,1,e1);
		 FillException(Exc,2,e2);
		 FillException(Exc,3,e3);
		 FillException(Exc,4,e4);
		 FillException(Exc,5,e5);
		 FillException(Exc,6,e6);
		 FillException(Exc,7,e7);
		 (document.getElementById('iChkEditId')).value=dgchkEdit;
		 (document.getElementById('itxtPriceEdit')).value=Price;
		 (document.getElementById('itxtPriceEEdit')).value=PriceE;		
		 (document.getElementById('itxtCodigoEdit')).value=CodHab;
		 (document.getElementById('itxtTipoPaqueteEdit')).value=TipoPaqueteM;
		 
		 
		 
		 		
		 var txtCancel = document.getElementById(btnCancel);		 
		 txtCancel.style.display='';			 	
		 var grid = document.getElementById(dgRoomsId);	
		 
		 if (grid)
			{	 
			
				var item = grid.getElementsByTagName("tr");										
				var items=0;
				for (var i = 1; i < item.length; i++)
				{
					var tds = item[i].getElementsByTagName("td");			
					var chkEdit = tds[5].getElementsByTagName("input");									
					
					if (chkEdit)
					{  var sw=false;
						for (var j=0;j<=chkEdit.length-1 && sw==false;j++)
						 {
							if (chkEdit[j].type=='checkbox'){sw=true;if (chkEdit[j].id!=dgchkEdit){chkEdit[j].checked=false;item[i].className="dgItem";}else{item[i].className="dgSelected";}}
						 }
					}					  			
				}				
			}
		 return 0;
   }


  function ShowPageDg(dg,posDg)
     {
		var grid = document.getElementById(dg);
	
		if (grid)
			{			 
				var pager= document.getElementById("dgPager");
				var item = grid.getElementsByTagName("tr");										
				var items=0;
				for (var i = 1; i < item.length; i++)
				{
					var tds = item[i].getElementsByTagName("td");			
					var chkEdit = tds[5].getElementsByTagName("input");				
					var chkDelete = tds[4].getElementsByTagName("input");	
				if (chkEdit && chkDelete)
				{
					var iMax = posDg * 5;
					if (i >= iMax-4 && i<=iMax)
					 {
						item[i].style.display='';						
					 }
					 else
					  {
					  	item[i].style.display='none';						
					  }	
					  items ++;
				 }
					  			
				}
				pager.innerHTML ="";		
				var nPages= Math.round(items/5);
				
				if ((5*nPages)<items-1) nPages++;						
				for (var i = 1;i<=nPages;i++)
				 {
					if (i!=1)
						pager.innerHTML = pager.innerHTML + "|";
					if (i==posDg)
					 {
						pager.innerHTML = pager.innerHTML   +  i;
					 }
					 else
					 {
					 pager.innerHTML = pager.innerHTML  + "<a href=javascript:ShowPageDg('" + dg + "'," + i +");>" +  i + "</a>";
					 }
					
				 }
			}
     
     }
 