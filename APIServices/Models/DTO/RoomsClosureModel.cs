using System;

namespace APIServices.Models.DTO
{
    public class RoomsClosureModel
    {
        public string StatusHotel { get; set; } = "";
        public string ColorStatusHotel { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
