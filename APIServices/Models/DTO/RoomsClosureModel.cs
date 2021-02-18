using System;
using System.Collections.Generic;

namespace APIServices.Models.DTO
{
    public class RoomsClosureModel
    {
        public string StatusHotel { get; set; } = "";
        public string ColorStatusHotel { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RateRoomsClosureModel> RateRoomsClosureModelList { get; set; }
    }

    public class RateRoomsClosureModel
    {
        public string RatePlan { get; set; }
        public List<CodeRoomModel> CodeRoomModelsList { get; set; }
    }

    public class CodeRoomModel
    {
        public string Code { get; set; }
        public string RoomName { get; set; }
        public string[] Status { get; set; }
    }

    public class RatePlansClosureModel
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }

    public class RoomsModel
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }

}
