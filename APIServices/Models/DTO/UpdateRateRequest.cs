using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class UpdateRateRequest
    {
        public int RateId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RatePlanId { get; set; }
        public decimal ExtraAdultPrice { get; set; }
        public decimal ExtraChildPrice { get; set; }
        public decimal ExtraJuniorPrice { get; set; }
        public string RateCode { get; set; }
        public UpdateRateRequestDiscount Promotion { get; set; }
        public List<DailyRateDetailPrice> Prices { get; set; } = new List<DailyRateDetailPrice>();
        public UpdateRateRequestRules Rules { get; set; }
    }

    public class UpdateRateRequestRules
    {
        public string ExceptionDays { get; set; }
        public string NoArrival { get; set; }
        public bool? UseDefaultRules { get; set; }
        public string Segment { get; set; }
        public byte MinLOS { get; set; } //Maximum lenght of stay
        public byte MaxLOS { get; set; } //Minimum lenght of stay
        public byte MaxAdvanceBooking { get; set; }
        public byte MinAdvanceBooking { get; set; }
        public DateTime BookingWindowStartDate { get; set; }
        public DateTime BookingWindowEndDate { get; set; }
    }

    public class UpdateRateRequestDiscount
    {
        public byte Discount { get; set; }
        public string EnglishDescription { get; set; }
        public string SpanishDescription { get; set; }
    }
}
