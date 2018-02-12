
var _locationLoadingID = 'imgLoading';
var _locationLoadingImage = 'images/indicator.gif';

var _locationParameters = new Object();


$().ready(function() {

    $("body").append("<div id='" + _locationLoadingID + "' style='display:none; position:absolute; z-index:2;'><img style='vertical-align:middle;' src='" + _locationLoadingImage + "'/></div>");

    $Parameters().controls.city = $('.' + $Parameters().controls.city);
    $Parameters().controls.district = $('#' + $Parameters().controls.district);
    $Parameters().controls.state = $('#' + $Parameters().controls.state);
    $Parameters().controls.country = $('#' + $Parameters().controls.country);
    //por mientras...
    //$Parameters().controls.city2 = $('#' + $Parameters().controls.city2);

    //Empieza la sincornizacion de la ubicacion    

    //Pais onchange
    $('select.country').change(function() {
        var G = "";
        if ($(this).attr("Group"))
            G = "[Group=" + $(this).attr("Group") + "]";

        UpdateLoading($('select.state' + G), { "display": "", "position": "Right" });
        $('select.state' + G).html('');
        var selected = $('#' + this.id + ' option:selected').attr('value');
        $Parameters().controls.country.val(selected);
        $.ajax({
            url: $Parameters().baseUrl + 'servicios/locationitems.ashx',
            dataType: 'json',
            data: { type: 'state', parent: selected },
            success: function(states) {
                $.each(states, function() {
                    var item = $('<option></option>');
                    item.val(this['id']);
                    if ($Parameters().controls && $Parameters().controls.state && $Parameters().controls.state.val() == this['id'])
                        item.attr('selected', 'selected');
                    item.html(this['name']);
                    $('select.state' + G).append(item);
                });
            },
            complete: function(result) {
                UpdateLoading(this, { "display": "none" });
                $('select.state' + G).change();
            }
        });
    });

    //Estado onchange
    $('select.state').change(function() {
        var G = "";
        if ($(this).attr("Group"))
            G = "[Group=" + $(this).attr("Group") + "]";
        UpdateLoading($('select.district' + G), { "display": "", "position": "Right" });
        $('select.district' + G).html('');
        var selected = $(this).find('option:selected').attr('value');
        $Parameters().controls.state.val(selected);
        $.ajax({
            url: $Parameters().baseUrl + 'servicios/locationitems.ashx',
            dataType: 'json',
            data: { type: 'district', parent: selected },
            success: function(districts) {
                $.each(districts, function() {
                    var item = $('<option></option>');
                    item.val(this['id']);
                    if ($Parameters().controls && $Parameters().controls.district && $Parameters().controls.district.val() == this['id'])
                        item.attr('selected', 'selected');
                    item.html(this['name']);
                    $('select.district' + G).append(item);
                });
            },
            complete: function(result) {
                UpdateLoading(this, { "display": "none" });
                $('select.district' + G).change();
            }
        });
    });

    //Municipio onchange
    $('select.district').change(function() {
        var G = "";
        if ($(this).attr("Group"))
            G = "[Group=" + $(this).attr("Group") + "]";
        UpdateLoading($('select.city' + G), { "display": "", "position": "Right" });
        $('select.city' + G).html('');
        var selected = $(this).find('option:selected').attr('value');
        $Parameters().controls.district.val(selected);
        $.ajax({
            url: $Parameters().baseUrl + 'servicios/locationitems.ashx',
            dataType: 'json',
            data: { type: 'city', parent: selected },
            success: function(cities) {
                $.each(cities, function() {
                    var item = $('<option></option>');
                    item.val(this['id']);
                    if ($Parameters().controls && $Parameters().controls.city && $Parameters().controls.city.val() == this['id'])
                        item.attr('selected', 'selected');
                    item.html(this['name']);
                    $('select.city' + G).append(item);
                });
            },
            complete: function(result) {
                UpdateLoading(this, { "display": "none" });
                $('select.city' + G).change();
            }
        });
    });


    //Ciudad onchange
    $('select.city').change(function() {
        var G = "";
        if ($(this).attr("Group"))
            G = "[Group=" + $(this).attr("Group") + "]";

        if ($('select.city').find('option').length > 0) {
            if ($('select.country').attr('disabled') == false || $('select.country').attr('disabled') == undefined) {
                $('select.city').removeAttr('disabled');
            }
        } else {
            $('select.city').attr('disabled', 'true');
        }
        var grupo = $(this).attr("Group");
        $Parameters().controls.city.each(function() {
        if ($(this).attr("Group") == grupo)
                $(this).val(($('select.city' + G + ' option:selected').length > 0) ? $('select.city' + G + ' option:selected').val() : '');
        });

    });

    //Inicializa informacion de paises
    UpdateLoading($('select.country'), { "display": "", "position": "Right" });
    $('select.country').html('');
    $.ajax({
        url: $Parameters().baseUrl + 'servicios/locationitems.ashx',
        dataType: 'json',
        data: { type: 'country', lang: $Parameters().lang },
        success: function(countries) {
            $.each(countries, function() {
                var item = $('<option></option>');
                item.val(this['id']);
                if ($Parameters().controls && $Parameters().controls.country && $Parameters().controls.country.val() == this['id'])
                    item.attr('selected', 'selected');
                item.html(this['name']);
                $('select.country').append(item);
            });
        },
        complete: function(result) {
            UpdateLoading(this, { "display": "none" });

            $('select.country').val($Parameters().controls.country.val());
            $('select.country').change();

        }
    });
});

function UpdateLoading(target, e,hasHC) {
   
    var image = $('#' + _locationLoadingID);
    var height = 16;

    if(e.display && e.display.toLowerCase() == 'none'){
        image.hide();
    }
    else{
        image.show();
        
        if (target.size() == 0)
            return;
        var position = target.offset();
        if(e.position && e.position.toLowerCase() == 'left'){
            position.left = position.left + 3;
        }
        else{
            position.left = position.left + target.width() - (height*2);                  
        }
        
        image.find('img').css("margin-top",3);
        
        image.css("top",position.top);
        image.css("left",position.left);
        image.width(height);
        image.height(height);                        
    }                    
}   

function SetInitialLocation(country, state, district, city){
    //inicia el proceso de inicializacion

    $('#' + $Parameters().controls.city).val(city);
    $('#' + $Parameters().controls.district).val(district);
    $('#' + $Parameters().controls.state).val(state);
    $('#' + $Parameters().controls.country).val(country);
}

function $Parameters() {
        
    return _locationParameters;
}

function SetParameters(param) {

    if (param.exists) {
               
        if (param.baseUrl == null) { param.baseUrl = ''; }
        if (param.lang == null) { param.lang = ''; }
        if (param.controls == null) { param.controls = new Object(); }
        if (param.controls.city == null) { param.controls.city = ''; }
        if (param.controls.district == null) { param.controls.city = ''; }
        if (param.controls.state == null) { param.controls.city = ''; }
        if (param.controls.country == null) { param.controls.city = ''; }
        
        _locationParameters = param;

        if (!($Parameters().baseUrl.match('/$') == '/')) { $Parameters().baseUrl += '/'; }
    }

}
