using System.Collections.Generic;


namespace APIServices.Conflux.OTA.Models.Rates
{
    public class RateAmountMessages
    {
        public int HotelCode { get; set; }
        public List<RateAmountMessage> RateAmountMessagesList { get; set; }
    }
}
