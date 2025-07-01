using System;

namespace APIServices.Conflux.Models.Closure
{
    public class Closure
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string[] RatePlansList { get; set; }
        public int[] RoomsList { get; set; }
    }
}
