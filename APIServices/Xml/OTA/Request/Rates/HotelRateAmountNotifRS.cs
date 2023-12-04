using System.Xml.Linq;

namespace APIServices.Xml.OTA.Request.Rates
{
    public static class HotelRateAmountNotifRS
    {
        private static readonly XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
        public static XElement ParseHotelRateAmountNotifRS(string xml)
        {
            XDocument document = XDocument.Parse(xml);
            XElement hotelRateAmountNotifRS = document.Root;

            return hotelRateAmountNotifRS;

        }

        public static bool IsSuccessRequest(XElement hotelRateAmountNotifRS)
        {

            bool isSuccessRequest = false;

            if(hotelRateAmountNotifRS.Element(ns + "Success") != null)
            {
                isSuccessRequest = true;
            }

            if (hotelRateAmountNotifRS.Element(ns + "Errors") != null)
            {
                isSuccessRequest = false;
            }

            return isSuccessRequest;

        }

    }
}
