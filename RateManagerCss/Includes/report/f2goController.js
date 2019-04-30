angular.module('app', []);
(function (mod) {

    InitController.$inject = ['$scope', 'Tool'];
    function InitController($$, Tool)
    {
        $$.checkin = '';
        $$.checkout = '';
        $$.loading = false;
        $$.showExcel = false;
        $$.typeSearch =
            {
                value: '0'
            }

        $$.jsonResponse = [];
        $$.Search = function ()
        {
            var parameters = {};
            parameters = Tool.FormateDate($$.checkin, $$.checkout);
            
            if (!parameters.valid)
            {
                $('#errorData h2').text('Fecha invalida');
                $('#popup').trigger('click', function () { });
                return;
            }

            $$.$apply(function () {
                $$.loading = true;
            })

            parameters.typesearch = $$.typeSearch.value;
            if (window.isUv != null)
                parameters.isUv = window.isUv;

            Tool.Search(parameters).success(function (response) {
                $$.jsonResponse = response.result;
                $$.showExcel = $$.jsonResponse.length > 0;
                $$.$apply(function () {
                    $$.loading = false;
                })
            })
        }

        $$.StatusValue = function (value)
        {
            switch (value)
            {
                case 1:
                    return 'Confirmado';
                case 3:
                    return 'Cancelado';
                case 4:
                    return 'En proceso';
            }
        }
    }

    mod.controller('report', InitController);
})(angular.module('app'))