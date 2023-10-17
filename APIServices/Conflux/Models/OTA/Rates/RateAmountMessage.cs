using System.Collections.Generic;

namespace APIServices.Conflux.OTA.Models.Rates
{
    public class RateAmountMessage
    {
        public StatusApplicationControl statusApplicationControl { get; set; }
        public List<Rate> Rates { get; set; }
    }
}