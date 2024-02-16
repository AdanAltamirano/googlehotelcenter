using System.Collections.Generic;


namespace APIServices.Service.HotelVerse.Models
{
    public class Record
    {
        public string hotelverseLocator { get; set; }
        public string externalLocator { get; set; }
        public string clientName { get; set; }
        public string clientLastName { get; set; }
        public string clientEmail { get;set;}
        public string roomNumber { get; set; }
    }

    public class CancelRequest
    {
        public List<Record> bookingsLocatorToCancelList { get; set; } = null;

    }

    public class ConfirmRequest
    {
        public List<Record> bookingsLocatorToConfirmList { get; set; } = null;
    }

}
