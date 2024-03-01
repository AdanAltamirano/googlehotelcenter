using System;

namespace APIServices.Conflux.OTA.Models.Restrictions
{
    public class StatusApplicationControl
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string InvTypeCode { get; set; }
        public string RatePlanCode { get; set; }
        public bool ApplyMon { get; set; } = true;
        public bool ApplyTue { get; set; } = true;
        public bool ApplyWed { get; set; } = true;
        public bool ApplyThu { get; set; } = true;
        public bool ApplyFri { get; set; } = true;
        public bool ApplySat { get; set; } = true;
        public bool ApplySun { get; set; } = true;

    }
}
