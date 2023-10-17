using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using APIServices.Conflux.OTA.Models.Rates;

namespace APIServices.Xml.OTA.Request.Rates
{

    public static class HotelRateAmountNotifRQ
    {
        private static readonly string TYPE = "22"; // Fijados
        private static readonly string ID = "IP"; // Fijados

        private static readonly XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace ns = "http://www.opentravel.org/OTA/2003/05";

        public static XElement CreateHotelRateAmountNotifRQ(RateAmountMessages rateAmountMessages)
        {
            XElement otaHotelRateAmountNotifRQ = new XElement("OTA_HotelRateAmountNotifRQ",
                new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                new XAttribute("EchoToken", Guid.NewGuid()),
                new XAttribute("Version", "1"));

            XElement pos = GetPOS();

            XElement rateAmountMessagesXml = GetRateAmountMessages(rateAmountMessages);

            otaHotelRateAmountNotifRQ.Add(pos, rateAmountMessagesXml);

            return otaHotelRateAmountNotifRQ;
        }

        private static XElement GetPOS()
        {
            XElement pos = new XElement("POS");
            XElement source = new XElement("Source");

            XElement requestorID = new XElement("RequestorID",
                new XAttribute("Type", TYPE),
                new XAttribute("ID", ID));


            source.Add(requestorID);
            pos.Add(source);

            return pos;
        }

        private static XElement GetRateAmountMessages(RateAmountMessages rateAmountMessages)
        {
            XNamespace blank = XNamespace.Get(@"http://www.opentravel.org/OTA/2003/05");

            XElement rateAmountMessagesXml = new XElement(blank + "RateAmountMessages",
                new XAttribute("xmlns", blank.NamespaceName),
                new XAttribute("HotelCode",rateAmountMessages.HotelCode));

            foreach (RateAmountMessage currentRate in rateAmountMessages.RateAmountMessagesList)
            {
                XElement rateAmountMessage = new XElement(blank + "RateAmountMessage");

                XElement statusApplicationControl = new XElement(blank + "StatusApplicationControl",
                    new XAttribute("RatePlanCode", currentRate.statusApplicationControl.RatePlanCode),
                    new XAttribute("InvTypeCode", currentRate.statusApplicationControl.InvTypeCode));

                XElement rates = new XElement(blank + "Rates");

                foreach(Rate rate in currentRate.Rates)
                {
                    XElement ratesXml = new XElement(blank + "Rate",
                        new XAttribute("Start", rate.StartDate),
                        new XAttribute("End", rate.EndDate));

                    XElement baseGuestAmounts = new XElement(blank + "BaseByGuestAmts");

                    foreach(BaseGuestAmount baseGuestAmount in rate.BaseGuestAmounts)
                    {
                        //TODO: Precios
                        //TODO: Sea grega a BaseGuestAmount Element
                    }

                    ratesXml.Add(baseGuestAmounts);

                    rates.Add(ratesXml);
                }

                rateAmountMessage.Add(statusApplicationControl,rates);

                rateAmountMessagesXml.Add(rateAmountMessage);
            }

            return rateAmountMessagesXml;
        }



    }
}
