using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.Restrictions.Rate
{
    public class RateRestrictionDto
    {
        public int RateId { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public HotelRestrictionsDto RestrictionsHotel { get; set; }
        public List<string> RoomsLinked { get; set; }

        public RatePlanDto RatePlan { get; set; }

        public List<PromotionDto> Promotions { get; set; } = new List<PromotionDto>();
    }

    public class HotelRestrictionsDto
    {
        public int HotelId { get; set; }
        public short HotelMinDays { get; set; }
        public byte HotelMaxDays { get; set; }
        public int HotelMinAdvDays { get; set; }
        public int HotelMaxAdvDays { get; set; }
    }

    public class RatePlanDto
    {
        public string RatePlanId { get; set; }
        public string RateCode { get; set; }

        public RateRestrictionsDto RestrictionsRate { get; set; }
        public RatePlanRestrictionsDto RestrictionsRatePlan { get; set; }

        public List<LinkedRatePlanDto> LinkedRatePlans { get; set; }
    }

    public class RateRestrictionsDto
    {
        public byte MinDays { get; set; }
        public byte MaxDays { get; set; }
        public byte MinAdvDays { get; set; }
        public int MaxAdvDays { get; set; }
    }

    public class RatePlanRestrictionsDto
    {
        public byte RatePlanMinDays { get; set; }
        public byte RatePlanMaxDays { get; set; }
        public byte RatePlanMinAdvDays { get; set; }
        public int RatePlanMaxAdvDays { get; set; }
    }

    public class LinkedRatePlanDto
    {
        public string RateCode { get; set; }
        public byte MinDays { get; set; }
        public byte MaxDays { get; set; }
        public byte MinAdvDays { get; set; }
        public int MaxAdvDays { get; set; }
    }

    public class PromotionDto
    {
        public int? RateId { get; set; }
        public int HotelId { get; set; }
        public string RatePlanId { get; set; }
        public string ParentRatePlanId { get; set; }
        public string PromoRatePlanId { get; set; }
        public int? RoomId { get; set; }
        public string RoomCode { get; set; }
        public int PromotionMinDays { get; set; }
        public int PromotionMaxDays { get; set; }
        public int PromotionMinAdvDays { get; set; }
        public int PromotionMaxAdvDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AllRatePlanRestrictionsDto
    {
        public string RateCode { get; set; }
        public int MinDays { get; set; }
        public int MaxDays { get; set; }
        public int MinAdvDays { get; set; }
        public int MaxAdvDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
