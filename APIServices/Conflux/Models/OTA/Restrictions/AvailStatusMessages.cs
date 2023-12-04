using System.Collections.Generic;

namespace APIServices.Conflux.OTA.Models.Restrictions
{
    public class AvailStatusMessages
    {
        public int HotelCode { get; set; }
        public List<AvailStatusMessage> AvailStatusMessageList { get; set; }
    }
}
