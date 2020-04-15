using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIServices.Models;
using APIServices.Models.DTO;

namespace APIServices
{
    public class PromotionsService
    {
        public IEnumerable<Offer> FindOffers(int hotelId)
        {
            IEnumerable<Offer> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                result = db.vPromotions.Where(p =>
                   p.HotelId == hotelId).Select(o =>
                       new Offer()
                       {
                           HotelId = o.HotelId,
                           OfferId = o.PromotionCode,
                           StartDate = o.StartDate,
                           EndDate = o.EndDate,
                           Active = o.Active == 1 ? true : false,
                           Name = Dictionary.Get(o.DescriptionId),
                           Discount = new OfferDiscount() {
                               NightsDiscounted = 1,
                               NightsRequired = o.DaysFree,
                               Amount = o.DiscountApplicationType == 2 ? o.Discount : 0,
                               Percent = o.Discount == 1 ? o.Discount : 0,
                               ApplicationMode = new OfferDiscount().GetApplicationMode(o.DiscountApplicationMode)
                           },
                           CancelPenalty = new OfferCancelPenalty() {
                               OffsetDropTime = OfferCancelPenaltyOffsetDropTime.BeforeArrival,
                               OffsetTimeUnit = new OfferCancelPenalty().GetOffsetTimeUnit(o),
                               OffsetTimeUnitMiltiplier = o.CancelPriorDays != null ? o.CancelPriorDays : o.CancelPriorHours, //Si OffsetTimeUnit = days y OffsetTimeUnitMiltiplier = 0, es no cancelable
                               Name = o.CancelPenaltyName,
                               ShortDescription = Dictionary.Get(o.CancelPenaltyReviewId),
                               DetailedDescription = Dictionary.Get(o.CancelPenaltyDetailedId)
                           },
                           ApplicableFor = new Offer().GetApplicableFor(o.HotelId, o.PromotionCode),
                           Rule = new OfferRule() {
                               ApplyDays = o.AppyDays != null ? Utilities.GetDaysOfWeek(o.AppyDays) : null,
                               NoArrivals = o.NoArrivals != null ? Utilities.GetDaysOfWeek(o.NoArrivals) : null,
                               BookingWindow = new OfferBookingWindow() {
                                   StartDate = o.BookingWindowStartDate,
                                   EndDate = o.BookingWindowEndDate
                               },
                               MaxAdvanceBookingOffset = o.MaxAdvanceBooking,
                               MinAdvanceBookingOffset = o.MinAdvanceBookin,
                               ExcludedDates = OfferRule.GetOfferExcludedDates(hotelId, o.PromotionCode)
                           }
                       }
                   ).ToList();
            }

            return result;
        }
    }
}
 