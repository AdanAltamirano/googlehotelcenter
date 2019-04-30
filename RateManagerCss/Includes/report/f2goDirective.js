(function (mod) {

    function Calendar()
    {
        return {
            restrict: 'A',
            link: function ($$, $tag) {
                var pika = new Pikaday({
                    field: $tag[0],
                    numberOfMonths: 1,
                    bound: true,
                    defaultDate: new Date()
                });
            }
        }
    }

    function Search()
    {
        return {
            restrict: 'A',
            link: function ($$, $tag) {
                $tag.bind('click', function () {
                    if ($$.checkin == '' && $$.checkout == '')
                    {
                        $('#popup').trigger('click', function () { });
                    } else $$.Search();
                })
            }
        }
    }

    function ExportExcel()
    {
        return {
            restrict: 'A',
            link: function ($$, $tag) {
                $tag.bind('click', function () {
                    var url = '../ExportExcel/Reports.aspx?_blank';
                    if (window.isUv != null)
                        url += '&source=ip';
                    else url += '&source=f2go';

                    window.open(url);
                })
            }
        }
    }

    mod.directive('calendar', Calendar);
    mod.directive('search', Search);
    mod.directive('excel', ExportExcel);
})(angular.module('app'))