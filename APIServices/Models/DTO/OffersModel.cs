using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public enum OfferDiscountApplicationMode
    {
        RateDiscountPriority, //Si la tarifa NO tiene descuento, se aplica el descuento de la oferta. Si la tarifa tiene descuento, el descuento de la oferta se descarta.
        PlusDiscount, //Si la tarifa tiene descuento, el descuento de la offerta se suma.
        AdditionalDiscount //Primero se aplica el descuento de la tarifa y al resultado se le aplica el descuento de la oferta.
    }

    public enum OfferCancelPenaltyOffsetDropTime
    {
         AfterArrival,
         AfterBooking,
         AfterConfirmation,
         AfterDeparture,
         BeforeArrival
    }

    public enum OfferCancelPenaltyOffsetTimeUnit
    {
        Days,
        Hours,
        SpecificTimeOfDay
    }

    public enum OfferDiscountDiscountPattern
    {
        ForEach, //Por cada cada vez que se cumpla NightsRequired en el rago de estancia
        Only // Descuento único cuando se cummpla NightsRequired en el rango de la estancia
    }
    public class Offer
    {
        public string OfferId { get; set; }
        public int HotelId { get; set; }
        public bool Active { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }        
        public OfferDiscount Discount { get; set; }
        public OfferCancelPenalty CancelPenalty { get; set; }
        public MultilanguageTextType Name { get; set; }
        public MultilanguageTextType Description { get; set; }
        public OfferApplicableFor ApplicableFor { get; set; }      
        public OfferRule Rule { get; set; }
        public OfferApplicableFor GetApplicableFor(int hotelId, string offerCode)
        {
            OfferApplicableFor applicableFor = new OfferApplicableFor();
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                applicableFor.RatesPlan = db.Promociones_RatePlan
                    .Where(r => r.IdPromocion == offerCode && r.IdHotel == hotelId)
                    .Select(p => p.IdRatePlan).ToList();

                applicableFor.Rooms = db.Promociones_TipoHabitacionHotel
                    .Where(r => r.IdPromocion == offerCode && r.IdHotel == hotelId)
                    .Select(p => p.IdTipoHabitacionHotel).ToList();
            }
                

            return applicableFor;
        }
    }

    public class OfferDiscount
    {
        public OfferDiscountDiscountPattern DiscountPattern { get; set; } // Define el comportamiento de las noches gratis
        public int NightsDiscounted { get; set; } //Número de noches que se descontarán
        public int? NightsRequired { get; set; } //Número de noches requeridas para aplicar el descuento
        public decimal? Percent { get; set; } //Porcentaje de descuento
        public decimal? Amount { get; set; } //Monto de descuento
        public OfferDiscountApplicationMode ApplicationMode { get; set; } //Define el comportamiento de la promoción en caso de haber un descuento a nivel tarifa
        public OfferDiscountApplicationMode GetApplicationMode(int? mode)
        {
            switch (mode)
            {
                case 0:
                    return OfferDiscountApplicationMode.RateDiscountPriority;
                case 1:
                    return OfferDiscountApplicationMode.PlusDiscount;
                case 2:
                    return OfferDiscountApplicationMode.AdditionalDiscount;
                default:
                    return OfferDiscountApplicationMode.RateDiscountPriority;
            }
        }
    }

    public class OfferCancelPenalty
    {
        public OfferCancelPenaltyOffsetDropTime OffsetDropTime { get; set; }
        public OfferCancelPenaltyOffsetTimeUnit OffsetTimeUnit { get; set; }
        public int? OffsetTimeUnitMiltiplier { get; set; }
        public string Name { get; set; }
        public MultilanguageTextType ShortDescription { get; set; }
        public MultilanguageTextType DetailedDescription { get; set; }
        public OfferCancelPenaltyOffsetTimeUnit GetOffsetTimeUnit(vPromotions offer)
        {
            if (offer.CancelPriorDays != null)
                return OfferCancelPenaltyOffsetTimeUnit.Days;
            else if (offer.CancelPriorHours != null)
                return OfferCancelPenaltyOffsetTimeUnit.Hours;
            else if (offer.CancelPenaltySpecificTime != null)
                return OfferCancelPenaltyOffsetTimeUnit.SpecificTimeOfDay;

            return OfferCancelPenaltyOffsetTimeUnit.Days;
        }
    }

    public class OfferRule
    {
        public DaysOfWeekType NoArrivals { get; set; }
        public DaysOfWeekType ApplyDays { get; set; }
        public List<OfferExcludedDates> ExcludedDates { get; set; }
        public OfferBookingWindow BookingWindow { get; set; }
        public int? MinAdvanceBookingOffset { get; set; }
        public int? MaxAdvanceBookingOffset { get; set; }
        public static List<OfferExcludedDates> GetOfferExcludedDates(int hotelId, string offerCode)
        {
            List<OfferExcludedDates> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                result = db.LockRoomTypes.Where(e => e.idhotel == hotelId && e.IdRatePlan == offerCode)
                    .GroupBy(x => new {x.StartDate, x.EndDate})
                    .Select(g =>
                        new OfferExcludedDates()
                        {
                            Start = g.FirstOrDefault().StartDate,
                            End = g.FirstOrDefault().EndDate
                        }
                    ).ToList();
            }
            return result;
        }
    }

    public class OfferBookingWindow
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class OfferApplicableFor
    {
        public List<string> RatesPlan { get; set; }
        public List<int> Rooms { get; set; }
    }

    public class OfferExcludedDates
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

    public class MultilanguageTextType
    {
        public string Eng { get; set; }
        public string Esp { get; set; }
        public int? Id { get; set; }
    }

    public class DaysOfWeekType
    {
        public bool Sun { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Thur { get; set; }
        public bool Weds { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }

    }
}
