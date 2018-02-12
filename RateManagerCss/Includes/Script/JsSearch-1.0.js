

jQuery.noConflict();
var arr = [];
var arr1 = [];
var arr2 = [];

var arrID = [];
var t;
var gp;

(function($) {

    SearchStart = function() {
        var x = {};
        x = {

            add: function(t, p) {

                if (t.grid) return false; //return if already exist	

                // apply default properties
                gp = $.extend({
                    colModel: [
			                    { display: 'Codigo' },
			                    { display: 'Nombre' },
			        ],
                    id: 'Id',
                    index: 0,
                    isLoader: true,
                    onSubmit: false,
                    url: '',
                    dataType: 'xml',
                    Count: 1
                }, p)
            },
            AddParam: function(p) {
                this.add(this, p);
            },
            Loader: function(value) {
                gp.isLoader = value;
            },
            ChangeIndex: function(val) {
                gp.index = val;
            },
            SetFilter: function(_id) {
                if (gp.searchitems[1] != undefined) {
                    $(_id).val(gp.searchitems[0].IDSearch + '|' + gp.searchitems[0].nameSearch + '|' + gp.searchitems[1].nameSearch);
                }
                else {
                    $(_id).val(gp.searchitems[0].IDSearch + '|' + gp.searchitems[0].nameSearch + '|');
                }
            },
            ValidCheckedRadio: function(_div) {
                var dv = document.getElementById(_div);
                var list = dv.getElementsByTagName("input");
                var hr = true;
                for (var i = 0; i <= list.length - 1; i++) {
                    if (list[i].type == 'radio') {
                        if (list[i].checked) {
                            gp.index = i;
                            break;
                        }
                    }
                }
                return hr;
            }
        }
        return x;
    } ();


})(jQuery);


function ValidRadio(_iddiv) {
    return SearchStart.ValidCheckedRadio(_iddiv);
}

function FireChangeIndex(val, _id, spn, caption) {
    SearchStart.ChangeIndex(val);
    var e = document.getElementById(_id);
    SetTextFilter(spn, caption, val);
    if (e) {
        e.style.display = 'none';
    }
}

function FireShowCtrlFilter(_id) {
    var e = document.getElementById(_id);
    if (e) {
        e.style.display = (e.style.display == 'none') ? '' : 'none';
    }
}

function SetTextRadio(_id, idRb, idDiv, pos, index) {
    var hr;
    hr = SearchStart.ValidCheckedRadio(idDiv)  
    if (gp.colModel.length >= pos) {
        document.getElementById(_id).innerHTML = gp.colModel[index].display;
    }
    if (hr && gp.index == index) {
        document.getElementById(idRb).checked = true;
    }
}

function SetTextFilter(_id, caption, pos) {
    document.getElementById(_id).innerHTML = caption + ' ' + gp.colModel[pos].display;
}

// !JsSearch-1.0.js 