using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using APIServices.Models;
using APIServices.Models.DTO;

namespace APIServices
{
    public class OfferService
    { 
        private enum OfferEnum
        {
            ActiveAndInActive = -1,
            Active = 1,
            InActive = 0,
        }

        private Expression<Func<vPromotions,bool>> Filter(int hotelId, string offerCode,int active)
        {


            //Filtra por codigo
            if(!String.IsNullOrEmpty(offerCode))
                return promotion => promotion.HotelId == hotelId
                                   && promotion.PromotionCode == offerCode;

            //Filtra promociones que incluya activos y no activos
            if (active == (int)OfferEnum.ActiveAndInActive
                && String.IsNullOrEmpty(offerCode))
                return promotion => promotion.HotelId == hotelId;

            //Filtra promociones que incluya activo o no activo
            else if ((active == (int)OfferEnum.InActive || active == (int) OfferEnum.Active) 
                    && String.IsNullOrEmpty(offerCode))
                return promotion => promotion.HotelId == hotelId 
                                    && promotion.Active == active;

            return promotion => promotion.HotelId == hotelId;
        }
        
        public IEnumerable<Offer> FindOffers(int hotelId, string offerCode = "")
        {
            IEnumerable<Offer> result = null;

            using (OzHotelesEntities db = new OzHotelesEntities( ))
            {
                int activeOption = -1;

                var filter = this.Filter(hotelId, offerCode, activeOption);

                var filtered = db.vPromotions.Where(filter).ToList();


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
                               Percent = o.DiscountApplicationType == 1 ? o.Discount : 0,
                               ApplicationMode = OfferDiscount.GetApplicationMode(o.DiscountApplicationMode)
                           },
                           ApplicableFor = Offer.GetApplicableFor(o.HotelId, o.PromotionCode),
                           Rule = new OfferRule()
                           {
                               Id = o.idRule,
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
                try
                {
                    var exist = db.RatesPlan.FirstOrDefault(x => x.idRatePlan == offer.Id && x.IdHotel == offer.HotelId);
                    if (exist == null)
                    {

                        //Reglas
                        var rule = new RatesPlanRules()
                        {
                            //idRule = offer.Rule.Id,
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
                            PromoEndDate = offer.EndDate,
                            idDiccionarioPoliticaCancelacionReview = Dictionary.Insert(offer.Rule.CancelPenalty.ShortDescription.Esp, offer.Rule.CancelPenalty.ShortDescription.Eng),
                            idDiccionarioPoliticaCancelacionFull = Dictionary.Insert(offer.Rule.CancelPenalty.DetailedDescription.Esp, offer.Rule.CancelPenalty.DetailedDescription.Eng)
                        };
                        db.RatesPlanRules.Add(rule);

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
                            Deleted = false,
                            idRule = rule.idRule
                        };
                        db.RatesPlan.Add(rp);

                        var deal = new ratesPlanDeal()
                        {
                            idRatePlan = offer.Id,
                            idHotel = offer.HotelId,
                            FechaInicio = offer.Rule.BookingWindow.StartDate,
                            FechaFin = offer.Rule.BookingWindow.EndDate,
                            HoraInicio = offer.Rule.BookingWindow.StartHour,
                            HoraFin = offer.Rule.BookingWindow.EndHour,
                        };
                        db.ratesPlanDeal.Add(deal);

                        //Aplicable para
                        var rooms = offer.ApplicableFor.Rooms.Select(r =>
                            new Promociones_TipoHabitacionHotel()
                            {
                                IdHotel = offer.HotelId,
                                IdPromocion = offer.Id,
                                IdTipoHabitacionHotel = r
                            }
                        ).ToList();
                        db.Promociones_TipoHabitacionHotel.AddRange(rooms);

                        var ratesplan = offer.ApplicableFor.RatesPlan.Select(r =>
                            new Promociones_RatePlan()
                            {
                                IdHotel = offer.HotelId,
                                IdPromocion = offer.Id,
                                IdRatePlan = r
                            }
                        ).ToList();
                        db.Promociones_RatePlan.AddRange(ratesplan);

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        return new KeyValuePair<string, string>("0", "Add: " + "El ID de promoción " + offer.Id + " ya existe.");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new KeyValuePair<string, string>("0", "Add: " + e.Message);
                }
            }
            return new KeyValuePair<string, string>("1", "success");
        }

        public KeyValuePair<string, string> Update(Offer offer)
        {

            OzHotelesEntities db = new OzHotelesEntities();
            using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var exist = db.RatesPlan.FirstOrDefault(x => x.idRatePlan == offer.Id && x.IdHotel == offer.HotelId);
                    if (exist != null)
                    {

                        //Reglas
                        if (offer.Rule != null)
                        {
                            var rule = db.RatesPlanRules.FirstOrDefault(x => x.idRule == offer.Rule.Id);
                            if (rule != null)
                            {
                                rule.CancelPriorHours = offer.Rule.CancelPenalty.OffsetTimeUnit == OfferCancelPenaltyOffsetTimeUnit.Hours ? offer.Rule.CancelPenalty.OffsetTimeUnitMiltiplier : null;
                                rule.CancelPriorDays = offer.Rule.CancelPenalty.OffsetTimeUnit == OfferCancelPenaltyOffsetTimeUnit.Days ? offer.Rule.CancelPenalty.OffsetTimeUnitMiltiplier : null;
                                rule.CancelPriorSpecificT = offer.Rule.CancelPenalty.OffsetTimeUnit == OfferCancelPenaltyOffsetTimeUnit.SpecificTimeOfDay ? offer.Rule.CancelPenalty.SpecificOffsetTime : null;
                                rule.AdvBooking = offer.Rule.MinAdvanceBookingOffset != 0 ? offer.Rule.MinAdvanceBookingOffset : null;
                                rule.MaxAdvBooking = offer.Rule.MaxAdvanceBookingOffset != 0 ? offer.Rule.MaxAdvanceBookingOffset : null;
                                rule.MaxDias = offer.Rule.MaxLOS != 0 ? offer.Rule.MaxLOS : null;
                                rule.MinDias = offer.Rule.MinLOS != 0 ? offer.Rule.MinLOS : null;
                                rule.NoArrivos = Utilities.GetDaysOfWeekString(offer.Rule.NoArrivals);
                                rule.PromoSpecificDays = Utilities.GetDaysOfWeekString(offer.Rule.ApplyDays);
                                rule.PromoStartDate = offer.StartDate;
                                rule.PromoEndDate = offer.EndDate;

                                //Diccionario
                                Dictionary.Update(offer.Rule.CancelPenalty.ShortDescription);
                                Dictionary.Update(offer.Rule.CancelPenalty.DetailedDescription);
                            }
                        }

                        //RatePlan
                        var rp = db.RatesPlan.FirstOrDefault(x => x.idRatePlan == offer.Id && x.IdHotel == offer.HotelId);
                        if (rp != null)
                        {
                            rp.Description = offer.Name.Esp;
                            rp.Name = offer.Name.Esp;
                            rp.codigotarifa = offer.Id;
                            rp.DescPromotion = offer.Discount.Percent != 0 ? offer.Discount.Percent : offer.Discount.Amount != 0 ? offer.Discount.Amount : null;
                            rp.DaysFree = (short)offer.Discount.NightsDiscounted;
                            rp.DaysFreeType = false;
                            rp.DiscountLevel = GetDiscountApplicationMode(offer.Discount.ApplicationMode);
                            rp.TipoDescuento = 1; //Porcentaje. TODO: Monto

                            //Diccionario
                            Dictionary.Update(offer.Name);
                            Dictionary.Update(offer.Description);
                        }

                        if (offer.Rule.BookingWindow != null)
                        {
                            var deal = db.ratesPlanDeal.FirstOrDefault(x => x.idRatePlan == offer.Id && x.idHotel == offer.HotelId);
                            if (deal != null)
                            {
                                deal.FechaInicio = offer.Rule.BookingWindow.StartDate;
                                deal.FechaFin = offer.Rule.BookingWindow.EndDate;
                                deal.HoraInicio = offer.Rule.BookingWindow.StartHour;
                                deal.HoraFin = offer.Rule.BookingWindow.EndHour;
                            }
                        }

                        //Aplicable para
                        if (offer.ApplicableFor.Rooms != null)
                        {
                            var toDeleteRooms = db.Promociones_TipoHabitacionHotel.Where(x => x.IdPromocion == offer.Id && x.IdHotel == offer.HotelId).ToList();
                            if (toDeleteRooms != null)
                            {
                                db.Promociones_TipoHabitacionHotel.RemoveRange(toDeleteRooms);
                            }

                            var rooms = offer.ApplicableFor.Rooms.Select(r =>
                            new Promociones_TipoHabitacionHotel()
                            {
                                IdHotel = offer.HotelId,
                                IdPromocion = offer.Id,
                                IdTipoHabitacionHotel = r
                            }
                            ).ToList();
                            db.Promociones_TipoHabitacionHotel.AddRange(rooms);
                        }

                        if (offer.ApplicableFor.RatesPlan != null)
                        {
                            var toDeleteRatesPlan = db.Promociones_TipoHabitacionHotel.Where(x => x.IdPromocion == offer.Id && x.IdHotel == offer.HotelId).ToList();
                            if (toDeleteRatesPlan != null)
                            {
                                db.Promociones_TipoHabitacionHotel.RemoveRange(toDeleteRatesPlan);
                            }

                            var ratesplan = offer.ApplicableFor.RatesPlan.Select(r =>
                            new Promociones_RatePlan()
                            {
                                IdHotel = offer.HotelId,
                                IdPromocion = offer.Id,
                                IdRatePlan = r
                            }
                            ).ToList();
                            db.Promociones_RatePlan.AddRange(ratesplan);
                        }
                            

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        return new KeyValuePair<string, string>("0", "Add: " + "No se encontró ninguna promoción con código " + offer.Id + ".");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new KeyValuePair<string, string>("0", "Add: " + e.Message);
                }
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
