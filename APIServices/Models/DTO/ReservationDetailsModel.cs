using System;

namespace APIServices.Models.DTO
{
    public class ReservationDetailsModel
    {
        public string HotelName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Status { get; set; }
        public string AccessCode { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int ReservationId { get; set; }
        public string Source { get; set; }
        public CustomerDetails Customer { get; set; }
    }

    public class CustomerDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
