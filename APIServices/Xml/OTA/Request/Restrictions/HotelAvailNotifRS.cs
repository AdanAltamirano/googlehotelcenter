using System.Xml.Linq;

namespace APIServices.Xml.OTA.Request.Restrictions
{
    public static class HotelAvailNotifRS
    {
        private static readonly XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
        public static XElement ParseHotelAvailNotifRS(string xml)
        {
            XDocument document = XDocument.Parse(xml);
            XElement hotelAvailNotifRS = document.Root;

            return hotelAvailNotifRS;
        }

        public static bool IsSuccessRequest(XElement hotelAvailNotifRS)
        {

            bool isSuccessRequest = false;

            if (hotelAvailNotifRS.Element(ns + "Success") != null)
            {
                isSuccessRequest = true;
            }

            if (hotelAvailNotifRS.Element(ns + "Errors") != null)
            {
                isSuccessRequest = false;
            }

            return isSuccessRequest;

        }
    }
}
