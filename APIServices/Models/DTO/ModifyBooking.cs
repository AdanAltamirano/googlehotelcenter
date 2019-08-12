using System;

namespace APIServices.Models.DTO
{
    public class ModifyBookingRQ
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public double TotalNR { get; set; }
        public double Total { get; set; }
    }

    public class ModifyBookingRS
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }
}
