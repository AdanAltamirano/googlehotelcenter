function toggleElementVisible(elementId){
	var e = document.getElementById(elementId);
	if (e){
		e.style.display = (e.style.display == "") || (e.style.display == "block") ? "none" : "block";
	}
}

function expandOrCollapse(expanderElementId, expandableElementId){
	var expander = document.getElementById(expanderElementId);
	if (expander){
		if (expander.src){
			expander.src = (expander.src.indexOf("mas.png") != -1) ? expander.src.replace("mas.png", "menos.png") : expander.src.replace("menos.png", "mas.png");
		}
		toggleElementVisible(expandableElementId);
	}
}

function toggleWaitScreen(elementId){
	var element = document.getElementById(elementId);
	if (element){
		element.style.display = element.style.display.toLowerCase() != "block" ? "block" : "none";
	}
}

function getCalendarDateRange(start, end){
	var range = new Array(2);
	range[0] = [start.getFullYear(), start.getMonth(), start.getDate()];
	range[1] = [end.getFullYear(), end.getMonth(), end.getDate()];
	
	return range;
}