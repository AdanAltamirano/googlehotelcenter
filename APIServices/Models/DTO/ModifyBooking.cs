using System;
using System.Collections.Generic;

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
        public string Details { get; set; }
        public List<RoomDetails> RoomsDetails { get; set; }
    }

    public class ModifyBookingRS
    {
        public bool IsSuccess { get; set; }
        public bool SendNotification { get; set; }
        public ErrorRS Error { get; set; }
        public string CustomerEmail { get; set; } = String.Empty;
        public string HotelEmail { get; set; } = String.Empty;
    }

    public class ErrorRS
    {
        public bool HasErrors { get; set; } = false;
        public List<string> Errors { get; set; }
    }

}
