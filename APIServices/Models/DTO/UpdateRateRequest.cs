using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class RateUpdateRQ
    {
        public int HotelId { get; set; }
        public int RateId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RatePlanCode { get; set; }
        public decimal? ExtraAdultPrice { get; set; }
        public decimal? ExtraChildPrice { get; set; }
        public decimal? ExtraJuniorPrice { get; set; }
        public string RateCode { get; set; }
        public bool IsOccupancyRate { get; set; }
        public RateUpdateRQPromotion Promotion { get; set; }
        public List<DailyRateDetailPrice> Prices { get; set; } = new List<DailyRateDetailPrice>();
        public List<DailyRateDetailPrice> ExceptionPrices { get; set; } = new List<DailyRateDetailPrice>();
        public RateUpdateRQRules Rules { get; set; }
        public RateUpdateRQGuestsRestriction GuestsRestrictions { get; set; }
        public RateUpdateRQBookingWindow BookingWindow { get; set; }
    }

    public class RateUpdateRQRules
    {
        public string ExceptionDays { get; set; }
        public string NoArrival { get; set; }
        public bool? UseDefaultRules { get; set; }
        public string Segment { get; set; }
        public byte? MinLOS { get; set; } //Maximum lenght of stay
        public byte? MaxLOS { get; set; } //Minimum lenght of stay
        public int? MaxAdvanceBooking { get; set; }
        public byte? MinAdvanceBooking { get; set; }
    }

    public class RateUpdateRQPromotion
    {
        public byte? Discount { get; set; }
        public string EnglishDescription { get; set; }
        public string SpanishDescription { get; set; }
    }

    public class RateUpdateRQBookingWindow
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class RateUpdateRQGuestsRestriction
    {
        public byte? MaxGuests { get; set; }
        public byte? MaxAdults { get; set; }
        public byte? MinAdults { get; set; }
        public byte? Children { get; set; }
        public byte? ExtraGuests { get; set; }
    }
}
