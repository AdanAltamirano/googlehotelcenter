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

            if (hotelRateAmountNotifRS.Element(ns + "Success") != null)
                isSuccessRequest = true;

            var errorsElement = hotelRateAmountNotifRS.Element(ns + "Errors");
            if (errorsElement != null)
            {
                // Un <Errors><Error /></Errors> con el nodo Error completamente vacío
                // (sin atributos ni texto) es un false-positive de Conflux — lo ignoramos.
                bool hasRealErrors = errorsElement.Elements(ns + "Error")
                    .Any(e => e.HasAttributes || !string.IsNullOrWhiteSpace(e.Value));

                if (hasRealErrors)
                    isSuccessRequest = false;
            }

            return isSuccessRequest;
        }

    }
}
