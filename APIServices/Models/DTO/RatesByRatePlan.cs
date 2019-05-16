using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class RatesByRatePlan
    {
        public string RatePlanId { get; set; }
        public string RatePlan { get; set; }
        public string Currency { get; set; }
        public int RoomId { get; set; }
        public string ParentRatePlanId { get; set; }
        public DailyRate[] DailyRates { get; set; }
    }
}
