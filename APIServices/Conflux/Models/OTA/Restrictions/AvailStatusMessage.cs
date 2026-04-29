
using System.Collections.Generic;

namespace APIServices.Conflux.OTA.Models.Restrictions
{
    public class AvailStatusMessage
    {
        public StatusApplicationControl StatusApplicationControl { get; set; }
        public RestrictionStatus RestrictionStatus { get; set; }
        public List<LengthOfStay> LengthsOfStay { get; set; } = new List<LengthOfStay>();
        public AdvanceBookingRestriction AdvanceBookingRestriction { get; set; }
    }
}
