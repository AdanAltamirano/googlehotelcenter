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

        public static List<XElement> CreateHotelAvailNotifRQList(AvailStatusMessages availStatusMessages)
        {
            List<XElement> otaHotelAvailNotifRQList = new List<XElement>();

            List<XElement> hotelAvailNotifRQXmlList = GetAvailStatusMessages(availStatusMessages.HotelCode, availStatusMessages.AvailStatusMessageList);

            foreach(XElement hotelAvailNotifRQXml in hotelAvailNotifRQXmlList)
            {
                XElement otaHotelAvailNotifRQ = new XElement("OTA_HotelAvailNotifRQ",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                    new XAttribute("EchoToken", Guid.NewGuid()),
                    new XAttribute("Version", "1"));

                XElement pos = GetPOS();

                otaHotelAvailNotifRQ.Add(pos, hotelAvailNotifRQXml);

                otaHotelAvailNotifRQList.Add(otaHotelAvailNotifRQ);
            }

            return otaHotelAvailNotifRQList;
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


        private static List<XElement> GetAvailStatusMessages(int hotelCode, List<AvailStatusMessage> availStatusMessagesList)
        {
            List<XElement> availStatusMessagesListElements = new List<XElement>();

            int limitBytesMessage = 80000;
            int currentBytesMessages = 0;
            XNamespace blank = XNamespace.Get(@"http://www.opentravel.org/OTA/2003/05");
            XElement availStatusMessagesXml = null;

            int index = 0;

            while (index < availStatusMessagesList.Count)
            {
                //Nuevo RateAmountMessages
                if (currentBytesMessages == 0 && availStatusMessagesXml == null)
                {
                    availStatusMessagesXml = new XElement(blank + "AvailStatusMessages",
                        new XAttribute("xmlns", blank.NamespaceName),
                        new XAttribute("HotelCode", hotelCode));
                }

                XElement availStatusMessageXml = new XElement(blank + "AvailStatusMessage");

                var start = availStatusMessagesList[index].StatusApplicationControl.Start.Date < DateTime.Now.Date ?
                    DateTime.Now.Date :
                    availStatusMessagesList[index].StatusApplicationControl.Start;

                XElement statusApplicationControl = new XElement(blank + "StatusApplicationControl",
                    new XAttribute("Start", start.ToString("yyyyMMdd")),
                    new XAttribute("End", availStatusMessagesList[index].StatusApplicationControl.End.ToString("yyyyMMdd")),
                    new XAttribute("Mon", availStatusMessagesList[index].StatusApplicationControl.ApplyMon),
                    new XAttribute("Tue", availStatusMessagesList[index].StatusApplicationControl.ApplyMon),
                    new XAttribute("Weds", availStatusMessagesList[index].StatusApplicationControl.ApplyWed),
                    new XAttribute("Thur", availStatusMessagesList[index].StatusApplicationControl.ApplyThu),
                    new XAttribute("Fri", availStatusMessagesList[index].StatusApplicationControl.ApplyFri),
                    new XAttribute("Sat", availStatusMessagesList[index].StatusApplicationControl.ApplySat),
                    new XAttribute("Sun", availStatusMessagesList[index].StatusApplicationControl.ApplySun),
                    new XAttribute("RatePlanCode", availStatusMessagesList[index].StatusApplicationControl.RatePlanCode),
                    new XAttribute("InvTypeCode", availStatusMessagesList[index].StatusApplicationControl.InvTypeCode));

                XElement restrictionStatus = new XElement(blank + "RestrictionStatus",
                    new XAttribute("Status", availStatusMessagesList[index].RestrictionStatus.Status));

                if (!string.IsNullOrEmpty(availStatusMessagesList[index].RestrictionStatus.Restriction))
                {
                    restrictionStatus.Add(new XAttribute("Restriction", availStatusMessagesList[index].RestrictionStatus.Restriction));
                }

                availStatusMessageXml.Add(statusApplicationControl, restrictionStatus);

                if (availStatusMessagesList[index].LengthsOfStay.Count > 0)
                {
                    XElement lengthsOfStay = new XElement(blank + "LengthsOfStay");

                    foreach (var stay in availStatusMessagesList[index].LengthsOfStay)
                    {
                        XElement lengthOfStay = new XElement(blank + "LengthOfStay",
                            new XAttribute("MinMaxMessageType", stay.MinMaxMessageType),
                            new XAttribute("Time", stay.Time.ToString()));

                        lengthsOfStay.Add(lengthOfStay);
                    }

                    availStatusMessageXml.Add(lengthsOfStay);
                }

                //Calcular Bytes del mensaje

                var availStatusMessageByteSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(availStatusMessageXml.ToString());
                currentBytesMessages += availStatusMessageByteSize;

                if (currentBytesMessages < limitBytesMessage)
                {
                    availStatusMessagesXml.Add(availStatusMessageXml); //Nodo Padre
                    index++;
                }
                else
                {
                    availStatusMessagesListElements.Add(availStatusMessagesXml);
                    currentBytesMessages = 0;
                    availStatusMessagesXml = null;
                }

            }

            if (availStatusMessagesXml != null)
            {
                availStatusMessagesListElements.Add(availStatusMessagesXml);
            }

            return availStatusMessagesListElements;

        }



    }
}
