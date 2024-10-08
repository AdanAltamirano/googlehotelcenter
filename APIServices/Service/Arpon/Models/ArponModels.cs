using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Service.Arpon.Models
{
    public class HotelIPH_Arpon_Model
    {
        public string IdHotelIp { get; set; } = string.Empty;
        public string IdHotelArpon { get; set; } = string.Empty;
    }

    public class RatePlansIPH_Arpon_Model
    {
        public string IdRatePlanIp { get; set; } = string.Empty;
        public string IdRatePlanArpon { get; set; } = string.Empty;
        public int? IdHotelIp { get; set; }
    }

    public class RoomsIPH_Arpon_Model
    {
        public string IdRoomIp { get; set; } = string.Empty;
        public string IdRoomArpon { get; set; } = string.Empty;
        public int? IdHotelIp { get; set; }
    }


}
