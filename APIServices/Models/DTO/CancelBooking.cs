using System;

namespace APIServices.Models.DTO
{
    public class CancelBookingRQ
    {
        public string Reason { get; set; }
    }

    public class CancelBookingRS
    {
        public string CancelNumber { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string Error { get; set; }
        public string CustomerEmail { get; set; } = String.Empty;
        public string HotelEmail { get; set; } = String.Empty;
    }
}
