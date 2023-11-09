using System;
using System.Collections.Generic;
using APIServices.Conflux.Enum;
namespace APIServices.Conflux.OTA.Models.Rates
{
    public class Rate
    {
        public bool IsPromotion { get; set; }
        public bool HasPriceException { get; set; }
        public TypeRateEnum TypeRate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<BaseGuestAmount> BaseGuestAmounts { get; set; }
        public List<AdditionalGuestAmount> AdditionalGuestAmounts { get; set; }      
        public bool ApplyMon { get; set; }
        public bool ApplyTue { get; set; }
        public bool ApplyWed { get; set; }
        public bool ApplyThu { get; set; }
        public bool ApplyFri { get; set; }
        public bool ApplySat { get; set; }
        public bool ApplySun { get; set; }
    }
}

