function DoSearch(url,obj) {
				if (sbHttp==null) { sbHttp = new HTTPXml(); }
				if (!sbHttp.inprogress) {
					window.status = "searching...";
					var sbHttp = new HTTPXml();
					sbHttp.init(url);
					try {
						sbHttp.setTimeout(60000);
						sbHttp.asyncGET(obj);
					} catch (e){}
				}
				else {
				alert("busy");
				}    
			}
			
			
			
			
			function loadInfoHotel()
			{

			    var url = "../../home.aspx?SRV=S";

			    DoSearch(url, new _ShowDatacompany())
			}
			
				
			function _ShowDatacompany() 
			{
				var texto;
				this.onLoad=function done(client) {
					window.status = "";
					if (!client.cancelled){											
					//var Container= parent.document.getElementById("CtrlHeader1_lblInfo");
					if (parent)					
					{
					texto = client.getText();

					var Container = parent.document.getElementById("contentInfo");
						if (Container)
						{
						Container.style.display='';
						Container.innerHTML=texto;}
						Container = parent.document.getElementById("_ctl0_lnkNameCompany");
						if (!Container) Container = parent.document.getElementById("ctl00_lnkNameCompany"); 
						var i =parent.document.getElementById("imgArrow");
						if(i){i.style.display='';}
						
						if (Container)
						{Container.innerHTML=texto.substring(0,texto.indexOf('<br>'));													 
						}		
																		
					}
					
					
					} 
				} 
			} 		
			
			