var iframehide="yes"

var getFFVersion=navigator.userAgent.substring(navigator.userAgent.indexOf("Firefox")).split("/")[1]
var FFextraHeight=parseFloat(getFFVersion)>=0.1? 16 : 0 //extra height in px to add to iframe in FireFox 1.0+ browsers
function resizeIframe(frameid)
{
	
	var currentfr=document.getElementById(frameid)
    if (!currentfr) currentfr=parent.document.getElementById(frameid)	
	if (currentfr && !window.opera)
	{	
	
		try{

		
			currentfr.style.display="inline"
			if (currentfr.contentDocument && currentfr.contentDocument.body.offsetHeight) //ns6 syntax			
				currentfr.height = currentfr.contentDocument.body.offsetHeight+FFextraHeight+5; 			
			else 
				if (currentfr.Document && currentfr.Document.body.scrollHeight) //ie5+ syntax				
					currentfr.height = currentfr.Document.body.scrollHeight+5;							
			if (currentfr.addEventListener)
			currentfr.addEventListener("load", readjustIframe, false)
			else if (currentfr.attachEvent)
			{
				currentfr.detachEvent("onload", readjustIframe) // Bug fix line
				currentfr.attachEvent("onload", readjustIframe)
			}
		}
		catch (e)
		{
		}
	}
	return true;
}

function readjustIframe(loadevt) 
{
	var crossevt=(window.event)? event : loadevt
	var iframeroot=(crossevt.currentTarget)? crossevt.currentTarget : crossevt.srcElement
	if (iframeroot)
		resizeIframe(iframeroot.id);
}