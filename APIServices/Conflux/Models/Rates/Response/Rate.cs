using System.Collections.Generic;


namespace APIServices.Conflux.Models.Rates.Response
{
    public class Rate
    {
        public string Xml { get; set; }
        public string XmlRequest { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class RatesMessages
    {
        public List<OTA.Models.Rates.RateAmountMessages> RateAmountMessagesList { get; set; } = new List<OTA.Models.Rates.RateAmountMessages>();
    }


}
