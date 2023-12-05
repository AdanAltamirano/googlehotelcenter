using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using APIServices.Conflux.OTA.Models.Restrictions;

namespace APIServices.Xml.OTA.Request.Restrictions
{
    public static class HotelAvailNotifRQ
    {
        private static readonly string TYPE = "22"; // Fijados
        private static readonly string ID = "IP"; // Fijados

        private static readonly XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace ns = "http://www.opentravel.org/OTA/2003/05";

        public static XElement CreateHotelAvailNotifRQ(AvailStatusMessages availStatusMessages)
        {
            XElement otaHotelAvailNotifRQ = new XElement("OTA_HotelAvailNotifRQ",
                new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                new XAttribute("EchoToken", Guid.NewGuid()),
                new XAttribute("Version", "1"));

            XElement pos = GetPOS();

            XElement availStatusMessagesXml = GetAvailStatusMessages(availStatusMessages);

            otaHotelAvailNotifRQ.Add(pos, availStatusMessagesXml);

            return otaHotelAvailNotifRQ;
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

        private static XElement GetAvailStatusMessages(AvailStatusMessages availStatusMessages)
        {
            XNamespace blank = XNamespace.Get(@"http://www.opentravel.org/OTA/2003/05");

            XElement availStatusMessagesXml = new XElement(blank + "AvailStatusMessages",
                new XAttribute("xmlns", blank.NamespaceName),
                new XAttribute("HotelCode", availStatusMessages.HotelCode));

            foreach(var availStatusMessage in availStatusMessages.AvailStatusMessageList)
            {
                XElement availStatusMessageXml = new XElement(blank + "AvailStatusMessage");

                var start = availStatusMessage.StatusApplicationControl.Start.Date < DateTime.Now.Date ? 
                    DateTime.Now.Date: 
                    availStatusMessage.StatusApplicationControl.Start;

                XElement statusApplicationControl = new XElement(blank + "StatusApplicationControl",
                    new XAttribute("Start", start.ToString("yyyyMMdd")),
                    new XAttribute("End", availStatusMessage.StatusApplicationControl.End.ToString("yyyyMMdd")),
                    new XAttribute("RatePlanCode", availStatusMessage.StatusApplicationControl.RatePlanCode),
                    new XAttribute("InvTypeCode", availStatusMessage.StatusApplicationControl.InvTypeCode));

                XElement restrictionStatus = new XElement(blank + "RestrictionStatus",
                    new XAttribute("Status", availStatusMessage.RestrictionStatus.Status));

                if (!string.IsNullOrEmpty(availStatusMessage.RestrictionStatus.Restriction))
                {
                    restrictionStatus.Add(new XAttribute("Restriction", availStatusMessage.RestrictionStatus.Restriction));
                }

                availStatusMessageXml.Add(statusApplicationControl, restrictionStatus);

                if(availStatusMessage.LengthsOfStay.Count > 0)
                {
                    XElement lengthsOfStay = new XElement(blank + "LengthsOfStay");

                    foreach(var stay in availStatusMessage.LengthsOfStay)
                    {
                        XElement lengthOfStay = new XElement(blank + "LengthOfStay",
                            new XAttribute("MinMaxMessageType",stay.MinMaxMessageType),
                            new XAttribute("Time", stay.Time.ToString()));

                        lengthsOfStay.Add(lengthOfStay);
                    }

                    availStatusMessageXml.Add(lengthsOfStay);
                }

                availStatusMessagesXml.Add(availStatusMessageXml);

            }

            return availStatusMessagesXml;

        }
    }
}
