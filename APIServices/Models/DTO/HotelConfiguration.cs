using System.Collections.Generic;

namespace APIServices.Models.DTO
{
    public class RatePlanHotelConfiguration
    {
        public string Id { get; set; }
        public bool? IsEnablePortal { get; set; }
        public bool? IsEnableHotelPayment { get; set; }
        public int? NetRateContract { get; set; }
        public int? NameId { get; set; }
        public int? DescriptionId { get; set; }
        public string RateCode { get; set; }
        public string Name { get; set; }
        public string NameEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public bool HasPrepaid { get; set; }
        //public List<string> Portals { get; set; }
    }

    public class Dates
    {
        public string Availability { get; set; }
        public bool? AvailablePortal { get; set; }
        public bool? AllowBankDeposit { get; set; }
        public string PropertyNumber { get; set; }
    }

    public class RatePlansByHotel
    {
        public string Id { get; set; }
        public Dictionary<string,Dictionary<string,object>> Properties { get; set; }
        //public string[] listPortales { get; set; }
    }

    public class HotelConfiguration
    {
        public int Id { get; set; }
        public Dictionary<string, Dictionary<string, object>> Properties { get; set; }
        public List<RatePlansByHotel> RatePlansList { get; set; } 
    }
}
