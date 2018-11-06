(function (mod) {

    function Tool()
    {
        return {
            urlPost: 'F2goGetReport.ashx',
            Search: function (params)
            {
                return $.ajax({ type: 'POST', data: params, url: this.urlPost });
            },
            FormateDate: function (checkIn, checkOut)
            {
                var format =
                {
                    checkin: checkIn.split('/')[2] + checkIn.split('/')[1] + checkIn.split('/')[0],
                    checkout: checkOut.split('/')[2] + checkOut.split('/')[1] + checkOut.split('/')[0],
                    valid: true
                }

                if (parseInt(format.checkout) < parseInt(format.checkin))
                    format.valid = false;
                return format;
            }
        }
    }

    mod.factory('Tool', Tool);
})(angular.module('app'))