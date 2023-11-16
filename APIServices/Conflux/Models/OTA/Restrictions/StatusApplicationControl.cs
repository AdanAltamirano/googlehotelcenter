using System;

namespace APIServices.Conflux.OTA.Models.Restrictions
{
    public class StatusApplicationControl
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string InvTypeCode { get; set; }
        public string RatePlanCode { get; set; }
    }
}
