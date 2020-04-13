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
    public class Offer
    {
        public string OfferId { get; set; }
        public int HotelId { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
        public OfferDiscount Discount { get; set; }
        public OfferCancelPenalty CancelPenalty { get; set; }
        public MultilanguageTextType Name { get; set; }
        public MultilanguageTextType Description { get; set; }
        public OfferApplicableFor ApplicableFor { get; set; }      
        public OfferRule Rule { get; set; }
    }

    public class OfferDiscount
    {
        public string DiscountPattern { get; set; } //*Pendiente*
        public int NightsDiscounted { get; set; } //Número de noches que se descontarán
        public int NightsRequired { get; set; } //Número de noches requeridas para aplicar el descuento
        public int Percent { get; set; } //Porcentaje de descuento
        public decimal Amount { get; set; } //Monto de descuento
        public OfferDiscountApplicationMode ApplicationMode { get; set; } //Define el comportamiento de la promoción en caso de haber un descuento a nivel tarifa
    }

    public class OfferCancelPenalty
    {
        public OfferCancelPenaltyOffsetDropTime OffsetDropTime { get; set; }
        public OfferCancelPenaltyOffsetTimeUnit OffsetTimeUnit { get; set; }
        public int OffsetTimeUnitMiltiplier { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public MultilanguageTextType Name { get; set; }
        public MultilanguageTextType ShortDescription { get; set; }
        public MultilanguageTextType DetailedDescription { get; set; }
    }

    public class OfferRule
    {
        public DaysOfWeekType NoArrivals { get; set; }
        public DaysOfWeekType ApplyDays { get; set; }
        public List<OfferExcludedDates> ExcludedDates { get; set; }
        public OfferBookingWindow BookingWindow { get; set; }
        public int MinAdvanceBookingOffset { get; set; }
        public int MaxAdvanceBookingOffset { get; set; }
    }

    public class OfferBookingWindow
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class OfferApplicableFor
    {
        public List<RatePlanApplicableFor> RatesPlan { get; set; }
        public List<RoomApplicableFor> Rooms { get; set; }
    }

    public class RatePlanApplicableFor
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsCommissionable { get; set; }
    }

    public class RoomApplicableFor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OfferExcludedDates
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class MultilanguageTextType
    {
        public string Eng { get; set; }
        public string Esp { get; set; }
        public int Id { get; set; }
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
