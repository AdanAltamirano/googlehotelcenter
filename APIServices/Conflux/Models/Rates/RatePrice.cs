using System;

namespace APIServices.Conflux.Models.Rates
{
    public class RatePrice
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string[] RatePlansList { get; set; }
        public int[] RoomsList { get; set; }
    }
}
