// JScript File

var offsetx = 12;
var offsety = 8;

var offsetxUp = 20;
var offsetyUp = 20;
var offsetyLUp = 160;
var mousex;
var mousey;

var ie5 = (document.getElementById && document.all);
var ns6 = (document.getElementById && !document.all);
var ua = navigator.userAgent.toLowerCase();
var isapple = (ua.indexOf('applewebkit') != -1 ? 1 : 0);

function getmousePos(e) {
    if (document.getElementById) {
        var iebody = (document.compatMode && document.compatMode != 'BackCompat') ? document.documentElement : document.body;
        pagex = (isapple == 1 ? 0 : (ie5) ? iebody.scrollLeft : window.pageXOffset);
        pagey = (isapple == 1 ? 0 : (ie5) ? iebody.scrollTop : window.pageYOffset);
        mousex = (ie5) ? event.x : (ns6) ? clientX = e.clientX : false;
        mousey = (ie5) ? event.y : (ns6) ? clientY = e.clientY : false;

    }
}

function getmouseposition(e) {
    if (document.getElementById) {
        var iebody = (document.compatMode && document.compatMode != 'BackCompat') ? document.documentElement : document.body;
        pagex = (isapple == 1 ? 0 : (ie5) ? iebody.scrollLeft : window.pageXOffset);
        pagey = (isapple == 1 ? 0 : (ie5) ? iebody.scrollTop : window.pageYOffset);
        mousex = (ie5) ? event.x : (ns6) ? clientX = e.clientX : false;
        mousey = (ie5) ? event.y : (ns6) ? clientY = e.clientY : false;

        var e_tooltip = document.getElementById('tooltip');
        e_tooltip.style.left = (mousex + pagex + offsetx) + 'px';
        e_tooltip.style.top = (mousey + offsety) + 'px';
    }
}

function getmousepositionUp(e) {
    if (document.getElementById) {
        var iebody = (document.compatMode && document.compatMode != 'BackCompat') ? document.documentElement : document.body;
        pagex = (isapple == 1 ? 0 : (ie5) ? iebody.scrollLeft : window.pageXOffset);
        pagey = (isapple == 1 ? 0 : (ie5) ? iebody.scrollTop : window.pageYOffset);
        mousex = (ie5) ? event.x : (ns6) ? clientX = e.clientX : false;
        mousey = (ie5) ? event.y : (ns6) ? clientY = e.clientY : false;

        var e_tooltip = document.getElementById('tooltipDetail');
        if (e_tooltip) {
            e_tooltip.style.left = (mousex + pagex + offsetxUp) + 'px';
            e_tooltip.style.top = (mousey - offsetyLUp) + 'px';
            document.onmouseup = null;
        }
    }
}

function getMousePositionDetail(pos) {
    if (document.getElementById) {
        var iebody = (document.compatMode && document.compatMode != 'BackCompat') ? document.documentElement : document.body;
        pagex = (isapple == 1 ? 0 : (ie5) ? iebody.scrollLeft : window.pageXOffset);
        pagey = (isapple == 1 ? 0 : (ie5) ? iebody.scrollTop : window.pageYOffset);
        pos.left = (mousex + pagex + offsetxUp) + 'px';
        pos.top = (mousey - offsetyLUp) + 'px';
    }
}

function getMousePositionDetailPos(pos, x) {
    if (document.getElementById) {
        var iebody = (document.compatMode && document.compatMode != 'BackCompat') ? document.documentElement : document.body;
        pagex = (isapple == 1 ? 0 : (ie5) ? iebody.scrollLeft : window.pageXOffset);
        pagey = (isapple == 1 ? 0 : (ie5) ? iebody.scrollTop : window.pageYOffset);
        pos.left = (mousex + pagex + offsetxUp - x) + 'px';
        if (mousey > 1600) offsetyLUp *= 2;
        pos.top = (mousey - offsetyLUp) + 'px';
        offsetyLUp = 160;
    }
}


function newElement$(newid) {
    if (document.createElement && (!document.getElementById(newid))) {
        var e = document.createElement('div');
        e.id = newid;
        with (e.style) {
            display = 'none';
            position = 'absolute';
            zIndex = '999';
        }
        e.innerHTML = '&nbsp;';
        document.body.appendChild(e);
    }
}

/** ! End jsToolTips.js **/