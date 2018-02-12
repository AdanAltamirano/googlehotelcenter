function fireTimer(obj) {
    var myObj = obj;
    function timerFired() {
        if (myObj.inprogress){myObj._abortRequest(myObj);}
    }
    myObj._timeoutID = window.setTimeout(timerFired, myObj.timeoutMS);
}

function HTTPXml(){};
HTTPXml.prototype = {
    uservars: null,url: null,postbuffer: null,xmlhttp: null,inprogress: false,
    thecallback: null,ref:null,timeoutMS: -1,_timeoutID: null,cancelled: false,
    init: function(url, postbuffer, args) {
				this.url = url;
				this.uservars = args;
				this.postbuffer = postbuffer;
				if (window.XMLHttpRequest) {
					this.xmlhttp = new XMLHttpRequest();
				} else if (window.ActiveXObject) {
					this.xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
				} else {
					window.alert('Sorry your browser is not compatible with this functionality');
				}
			},
    setTimeout: function(timeInMillisec) {
				this.timeoutMS = timeInMillisec;
			},
    asyncGET: function (client) {
				if (this.inprogress) {
					throw "Call in progress";
				};
				var self = this;
				if (client==null) {client=self;}
				this.thecallback = client;
				var args = null;
				if (this.postbuffer == null) {
					this.xmlhttp.open('GET',this.url,true);
				} else {
					this.xmlhttp.open("POST", this.url, true);
					this.xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=ISO-8859-1");
					args = this.postbuffer;
				}
				this.xmlhttp.onreadystatechange = function() {
					self.stateChangeCallback(self);
				}
				this.inprogress = true;
				this.cancelled = false;
				this.xmlhttp.send(args);
				if (this.timeoutMS > 0) {
					fireTimer(this);
				}
			},
    stateChangeCallback: function(client) {
				switch (client.xmlhttp.readyState) 
				{
					case 1:
						try {
							if (! client.cancelled) {
								client.thecallback.onInit();
							}
						}catch (e) {  }
					break;
					case 2:
						try {
							if ( client.xmlhttp.status != 200 ) {
								if (! client.cancelled) {
									window.status="error";
									client.thecallback.onError(client.xmlhttp.status,client.xmlhttp.statusText,client);
								}
								client.xmlhttp.abort();
								client.inprogress = false;
							}
						}catch (e) {}
					break;
					case 3:
						var contentLength;
						try {
							try {
								contentLength =client.xmlhttp.getResponseHeader("Content-Length");
							} catch (e) {
								contentLength = NaN;
							}
							if (! client.cancelled) {
								window.status="ping";
								client.thecallback.onProgress(client.xmlhttp.responseText,contentLength);
							}

						}catch (e) {  }
					break;
					case 4:						
						try {
							if (client._timeoutID) {
								window.clearTimeout(client._timeoutID);
								client._timeoutID = null;
							}
							if (client.inprogress) {
								client.inprogress = false;
								if (! client.cancelled) {
									window.status="done";
									client.thecallback.onLoad(client);
								}
							}
						}catch (e){
							client.thecallback.onError(
							'0001','Error Processing.',contentLength);       
						}finally{
							client.inprogress = false;
						}
					break;
				}
			},
	cancelRequest: function() {
				var gizmo = this;
				this.cancelled = true;
				if (this._timeoutID) {
					window.clearTimeout(this._timeoutID);
					this._timeoutID = null;
				}
				gizmo._abortRequest(gizmo);
			},
    _abortRequest: function(gizmo) {
				if (gizmo.xmlhttp!=null) {
					try {
						gizmo.xmlhttp.abort();
						if (gizmo.inprogress){
							window.status="abort";
							gizmo.thecallback.onError('timeout', 'Your request has timed out.',gizmo);
						}
					} catch (e) {}
					gizmo.cancelled = true;
					gizmo.inprogress = false;
				}
			},
    getText: function () {
				return this.xmlhttp.responseText;
			},
    getXML: function () {
				return this.xmlhttp.responseXML;
			},
    getTags: function (tagname) {
				try {
					return this.xmlhttp.responseXML.getElementsByTagName(tagname);
				} catch (e) {return null;}
			},
    getTagsByItem: function (parent,item,index){
				var result = parent.getElementsByTagName(item)[index];
				if (result)
				{
				return result;
				}
				else { return ""; }
			},    
    getTagText: function (parent,item){
				var result = parent.getElementsByTagName(item)[0];
				if (result)
				{
					if (result.firstChild)
						return result.firstChild.nodeValue;
					else
						return result.text;
				}
				else { return ""; }
			},
    getTagAttr: function (parent,item){
				var result = parent;
				if (result)
				{
						return result.getAttribute(item); 
				}
				else { return ""; }
			},    
    onProgress:function(t,l){},
    onError:function(s,t,c){},
    onLoad:function(c) { },
    onInit:function(c) { }
}

