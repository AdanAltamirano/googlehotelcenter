		function doSearch()
		{
			if (sbHttp==null) { sbHttp = new Ajax.Request(); }
			if (!sbHttp.inprogress) 
			{
				window.status = "searching...";
				var url = "DATA/RSS.XML";
				var qry="";
				var sbHttp = new Ajax.Request(url,{method: 'get',parameters:qry,onComplete:ShowRSS});        
			}
			else {
			alert("busy");
			}
		}

		function getTagText(parent,item)
		{
			var result = parent.getElementsByTagName(item)[0];
			if (result)
			{
					if (result.firstChild)
					return result.firstChild.nodeValue;
					else
					return result.text;
			}
			else { return ""; }
		}
		 
		function ShowRSS(client) {
			window.status = "....";
			var ii=client.responseXML.getElementsByTagName("channel"); //para saber cuantos vuelos. 
			var i=0;
			DivContainer.innerHTML=""
			while (ii[i]){
			    //aqui se puede escribir el encabezado principal. autor, title etcs. del channel
			    DivContainer.innerHTML+=getTagText(ii[i],'title');
			    DivContainer.innerHTML+=getTagText(ii[i],'description');
			    //separador..
			    jj=ii[i].getElementsByTagName("item");			    
				DivContainer.innerHTML+="<BR>";			    
			    var j=0;
			    while (jj[j]){			    
			         DivContainer.innerHTML+="<DIV class='element'>" + getTagText(jj[j],'description'); 
				  j++;
				}
				i++;
			}
			window.status = "Done";    
		} //main function  