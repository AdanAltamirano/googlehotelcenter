using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIServices.Models;
using APIServices.Models.DTO;

namespace APIServices
{
    public class OfferService
    {
        public IEnumerable<Offer> FindOffers(int hotelId, string offerCode = "")
        {
            IEnumerable<Offer> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                result = db.vPromotions.Where(p =>
                   p.HotelId == hotelId && (p.PromotionCode == offerCode || offerCode == "")).ToList().Select(o =>
                       new Offer()
                       {
                           HotelId = o.HotelId,
                           Id = o.PromotionCode,
                           StartDate = o.StartDate,
                           EndDate = o.EndDate,
                           Active = o.Active == 1 ? true : false,
                           Name = Dictionary.Get(o.DescriptionId),
                           Description = Dictionary.Get(o.IdDiccShortDesc),
                           Discount = new OfferDiscount()
                           {
                               NightsDiscounted = 1,
                               NightsRequired = o.DaysFree,
                               Amount = o.DiscountApplicationType == 2 ? o.Discount : 0,
                               Percent = o.Discount == 1 ? o.Discount : 0,
                               ApplicationMode = OfferDiscount.GetApplicationMode(o.DiscountApplicationMode)
                           },                           
                           ApplicableFor = Offer.GetApplicableFor(o.HotelId, o.PromotionCode),
                           Rule = new OfferRule()
                           {
                               ApplyDays = o.AppyDays != null ? Utilities.GetDaysOfWeek(o.AppyDays) : null,
                               NoArrivals = o.NoArrivals != null ? Utilities.GetDaysOfWeek(o.NoArrivals) : null,
                               BookingWindow = new OfferBookingWindow()
                               {
                                   StartDate = o.BookingWindowStartDate,
                                   EndDate = o.BookingWindowEndDate,
                                   StartHour = o.BookingWindowStartHour,
                                   EndHour = o.BookingWindowEndHour
                               },
                               MaxAdvanceBookingOffset = o.MaxAdvanceBooking,
                               MinAdvanceBookingOffset = o.MinAdvanceBookin,
                               ExcludedDates = OfferRule.GetOfferExcludedDates(hotelId, o.PromotionCode),
                               CancelPenalty = new OfferCancelPenalty()
                               {
                                   OffsetDropTime = OfferCancelPenaltyOffsetDropTime.BeforeArrival,
                                   OffsetTimeUnit = OfferCancelPenalty.GetOffsetTimeUnit(o),
                                   OffsetTimeUnitMiltiplier = o.CancelPriorDays != null ? o.CancelPriorDays : o.CancelPriorHours, //Si OffsetTimeUnit = days y OffsetTimeUnitMiltiplier = 0, es no cancelable
                                   Name = o.CancelPenaltyName,
                                   ShortDescription = Dictionary.Get(o.CancelPenaltyReviewId),
                                   DetailedDescription = Dictionary.Get(o.CancelPenaltyDetailedId)
                               }
                           }
                       }
                   );
            }

            return result;
        }

        public KeyValuePair<string, string> Add(Offer offer)
        {
            OzHotelesEntities db = new OzHotelesEntities();
            using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
            {

                //Reglas
                var rules = new RatesPlanRules() {
                    idRule = offer.Rule.Id,
                    IdHotel = offer.HotelId,
                    ReqVerification = true,
                    GuarDep = "N",
                    RateRulesDefault = true,
                    CancelPriorHours = offer.Rule.CancelPenalty.OffsetTimeUnit == OfferCancelPenaltyOffsetTimeUnit.Hours ? offer.Rule.CancelPenalty.OffsetTimeUnitMiltiplier : null,
                    CancelPriorDays = offer.Rule.CancelPenalty.OffsetTimeUnit == OfferCancelPenaltyOffsetTimeUnit.Days ? offer.Rule.CancelPenalty.OffsetTimeUnitMiltiplier : null,
                    CancelPriorSpecificT = offer.Rule.CancelPenalty.OffsetTimeUnit == OfferCancelPenaltyOffsetTimeUnit.SpecificTimeOfDay ? offer.Rule.CancelPenalty.SpecificOffsetTime : null,
                    AdvBooking = offer.Rule.MinAdvanceBookingOffset != 0 ? offer.Rule.MinAdvanceBookingOffset : null,
                    MaxAdvBooking = offer.Rule.MaxAdvanceBookingOffset != 0 ? offer.Rule.MaxAdvanceBookingOffset : null,
                    MaxDias = offer.Rule.MaxLOS != 0 ? offer.Rule.MaxLOS : null,
                    MinDias = offer.Rule.MinLOS != 0 ? offer.Rule.MinLOS : null,
                    NoArrivos = Utilities.GetDaysOfWeekString(offer.Rule.NoArrivals),
                    PromoSpecificDays = Utilities.GetDaysOfWeekString(offer.Rule.ApplyDays),
                    PromoStartDate = offer.StartDate,
                    PromoEndDate = offer.EndDate
                };

                var deal = new ratesPlanDeal()
                {
                    idRatePlan = offer.Id,
                    idHotel = offer.HotelId,
                    FechaInicio = offer.Rule.BookingWindow.StartDate,
                    FechaFin = offer.Rule.BookingWindow.EndDate,
                    HoraInicio = offer.Rule.BookingWindow.StartHour,
                    HoraFin = offer.Rule.BookingWindow.EndHour,
                };

                //Aplicable para
                var rooms = offer.ApplicableFor.Rooms.Select(r =>
                    new Promociones_TipoHabitacionHotel() {
                        IdHotel = offer.HotelId,
                        IdPromocion = offer.Id,
                        IdTipoHabitacionHotel = r
                    }
                ).ToList();

                var ratesplan = offer.ApplicableFor.RatesPlan.Select(r =>
                    new Promociones_RatePlan()
                    {
                        IdHotel = offer.HotelId,
                        IdPromocion = offer.Id,
                        IdRatePlan = r
                    }
                ).ToList();

                //RatePlan
                var rp = new RatesPlan()
                {
                    idRatePlan = offer.Id,
                    Description = offer.Name.Esp,
                    IdHotel = offer.HotelId,
                    Segment = "R",
                    Name = offer.Name.Esp,
                    RateGDS = false,
                    RatePortal = true,
                    RateUnip = false,
                    codigotarifa = offer.Id,
                    DescPromotion = offer.Discount.Percent != 0 ? offer.Discount.Percent : offer.Discount.Amount != 0 ? offer.Discount.Amount : null,
                    DaysFree = (short)offer.Discount.NightsDiscounted,
                    DaysFreeType = false,
                    RateADS = false,
                    gdsApply = "NNNN",
                    ComADS = 0,
                    ComGDS = 0,
                    ComOnePage = 0,
                    ComPortal = 0,
                    WaitListAvailable = false,
                    hotelPayment = false,
                    IsPromo = true,
                    DiscountLevel = GetDiscountApplicationMode(offer.Discount.ApplicationMode),
                    TipoDescuento = 1, //Porcentaje. TODO: Monto
                    IdDictionaryDescription = Dictionary.Insert(offer.Name.Esp, offer.Name.Eng),
                    IdDiccShortDesc = Dictionary.Insert(offer.Description.Esp, offer.Description.Eng),
                    idDiccPromoDesc = Dictionary.Insert("", ""), //Valor Agregado
                    Deleted = false
                };
            }
            return new KeyValuePair<string, string>("1", "success");
        }

        private short GetDiscountApplicationMode(OfferDiscountApplicationMode mode)
        {
            if (mode == OfferDiscountApplicationMode.AdditionalDiscount)
                return 2;
            else if (mode == OfferDiscountApplicationMode.PlusDiscount)
                return 1;
            else
                return 0;
        }
    }
}
