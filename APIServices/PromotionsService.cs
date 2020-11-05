using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using APIServices.Models;
using APIServices.Models.DTO;

namespace APIServices
{
    public class OfferService: IDisposable
    {
        private bool _disposed;

        OzHotelesEntities context = new OzHotelesEntities();

        /// <summary>
        ///  Devuelve una expresión para filtrar las promociones
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="includeOldPromos"></param>
        /// <param name="status"></param>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        private Expression<Func<vPromotions, bool>> Filter(int hotelId, bool includeOldPromos, int status, int searchBy, string searchValue)
        {
            //Filtro Base Hotel
            ParameterExpression parameterExpression = Expression.Parameter(typeof(vPromotions), "promotion");
            MemberExpression memberExpression = Expression.Property(parameterExpression, "HotelId");
            ConstantExpression constantExpression = Expression.Constant(hotelId, typeof(int));
            BinaryExpression equalhotel = Expression.Equal(memberExpression, constantExpression);
            Expression filterHotel = equalhotel;
  
            //Filtro Fechas
            //TODO: Validate BookingWindow Date
            Nullable<DateTime> filterDate = DateTime.Now;
            MemberExpression date = null;
            ConstantExpression dateConstant = null;
            BinaryExpression equalDate = null;

            //No Incluye Promos Viejas
            if (!includeOldPromos)
            {
                date = Expression.Property(parameterExpression, "EndDate");
                dateConstant = Expression.Constant(filterDate, typeof(Nullable<DateTime>));
                equalDate = Expression.GreaterThanOrEqual(date, dateConstant);
            }
            Expression filterHotelDate = filterHotel;
            if (equalDate != null) filterHotelDate = Expression.AndAlso(filterHotel, equalDate);

            //Filtro Status
            // Active = 1, InActive = 0, Active and InActive = -1
            MemberExpression active = null;
            ConstantExpression activeConstant = null;
            BinaryExpression equalActive = null;

            //Selecciona Si la Promo esta Activa o InActiva
            if(status == (int)OfferStatus.Active || status ==  (int) OfferStatus.InActive)
            {
                active = Expression.Property(parameterExpression, "Active");
                activeConstant = Expression.Constant(status,typeof(int?));
                equalActive = Expression.Equal(active, activeConstant);

            }

            Expression filterHotelDateActive = filterHotelDate;
            if (equalActive != null) filterHotelDateActive = Expression.AndAlso(filterHotelDate,equalActive);

            //Filtro de Busqueda
            MemberExpression type = null;
            ConstantExpression typeConstant = null;
            System.Reflection.MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            MethodCallExpression containsMethodExp = null;


            //Contiene Nombre ó Codigo de Promo a Buscar
            if (!String.IsNullOrEmpty(searchValue))
            {
                switch(searchBy)
                {
                    case (int)OfferStatus.Name:
                        type = Expression.Property(parameterExpression, "Description");
                        typeConstant = Expression.Constant(searchValue,typeof(string));
                        containsMethodExp = Expression.Call(type,method,typeConstant);
                        break;

                    case (int)OfferStatus.Code:
                        type = Expression.Property(parameterExpression,"PromotionCode");
                        typeConstant = Expression.Constant(searchValue, typeof(string));
                        containsMethodExp = Expression.Call(type, method, typeConstant);
                        break;
                }

            }

            Expression filterHotelDateActiveSearchValue = filterHotelDateActive;

            if (containsMethodExp != null) filterHotelDateActiveSearchValue = Expression.AndAlso(filterHotelDateActive,containsMethodExp);


            var filter = Expression.Lambda<Func<vPromotions, bool>>(filterHotelDateActiveSearchValue, new[] { parameterExpression });

            return filter;
        }
        
        public IEnumerable<Offer> FindOffers(int hotelId, string offerCode = "")
        {
            IEnumerable<Offer> result = null;

            using (OzHotelesEntities db = new OzHotelesEntities( ))
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
                                   MinDays = o.MinAdvanceBookin,
                                   MaxDays = o.MaxAdvanceBooking,
                                   StartHour = o.BookingWindowStartHour,
                                   EndHour = o.BookingWindowEndHour
                               },
                               //MaxAdvanceBookingOffset = o.MaxAdvanceBooking,
                               //MinAdvanceBookingOffset = o.MinAdvanceBookin,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotelId">Id del Hotel</param>
        /// <param name="filterStatus">Activo 1, InActivo 0, Inactivo y Activo -1</param>
        /// <param name="searchBy">Buscar por codigo o por nombre, 0 es por nombre y 1 es por código</param>
        /// <param name="searchValue">Valor para filtrar por nombre o por codigo</param>
        /// <returns></returns>
        public IQueryable<vPromotions> OffersList()
        {
            //IEnumerable<OfferPromotions> promotions = null;
            //Expression<Func<vPromotions,bool>> filter = this.Filter(hotelId,includeOldPromos,filterStatus,searchBy,searchValue);
            //promotions = context.vPromotions
            //    .Where(filter)
            //    // Linq To Objects with AsEnumerable To Apply Date Format 
            //    // Use AsEnumerable() to force evaluation of that part with Linq to Objects
            //    .AsEnumerable()
            //    .Select(s => new OfferPromotions
            //    {
            //        Code = s.PromotionCode,
            //        Name = s.Description,
            //        StartDate = (s.StartDate.HasValue) ? s.StartDate.Value.ToString("dd/MM/yyyy") : "",
            //        EndDate = (s.EndDate.HasValue) ? s.EndDate.Value.ToString("dd/MM/yyyy") : "",
            //        BookingStartDate = (s.BookingWindowStartDate.HasValue) ? s.BookingWindowStartDate.Value.ToString("dd/MM/yyyy") : "",
            //        BookingEndDate = (s.BookingWindowEndDate.HasValue) ? s.BookingWindowEndDate.Value.ToString("dd/MM/yyyy") : "",
            //        Discount = Decimal.Round((decimal)s.Discount),
            //        Active = s.Active
            //    });

            //return promotions;

            return context.vPromotions.AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="offerCode"></param>
        /// <returns></returns>
        public Offer FindOfferByHotelAndCode(int hotelId, string offerCode)
        {
            vPromotions o = context.vPromotions
                .First(promotion => promotion.HotelId == hotelId
                                   && promotion.PromotionCode == offerCode);

            Offer offer = new Offer()
            {
                HotelId = o.HotelId,
                Id = o.PromotionCode,
                StartDate = o.StartDate,
                EndDate = o.EndDate,
                Active = o.Active == 1 ? true : false,
                IsCombinablePromotion = o.IsCombinablePromotion,
                Name = Dictionary.Get(o.DescriptionId),
                Description = Dictionary.Get(o.IdDiccShortDesc),
                Discount = new OfferDiscount()
                {
                    //NightsDiscounted = 1,
                    DiscountPattern = (o.DaysFreeType == null) ? OfferDiscountDiscountPattern.ForEach : ((bool)o.DaysFreeType) ? OfferDiscountDiscountPattern.Only : OfferDiscountDiscountPattern.ForEach,
                    NightsDiscounted =(o.DaysFree == null)? 0 : o.DaysFree,
                    NightsRequired = (o.DaysFree == null)? 0 : o.DaysFree,
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
                        MinDays = o.MinAdvanceBookin,
                        MaxDays = o.MaxAdvanceBooking,
                        StartHour = o.BookingWindowStartHour,
                        EndHour = o.BookingWindowEndHour
                    },
                    //MaxAdvanceBookingOffset = o.MaxAdvanceBooking,
                    //MinAdvanceBookingOffset = o.MinAdvanceBookin,
                    ExcludedDates = OfferRule.GetOfferExcludedDates(hotelId, o.PromotionCode),
                    CancelPenalty = new OfferCancelPenalty()
                    {
                        OffsetDropTime = OfferCancelPenaltyOffsetDropTime.BeforeArrival,
                        OffsetTimeUnit = OfferCancelPenalty.GetOffsetTimeUnit(o),
                        OffsetTimeUnitMiltiplier = o.CancelPriorDays != null ? o.CancelPriorDays : o.CancelPriorHours, //Si OffsetTimeUnit = days y OffsetTimeUnitMiltiplier = 0, es no cancelable
                        Name = o.CancelPenaltyName,
                        ShortDescription = Dictionary.Get(o.CancelPenaltyReviewId),
                        DetailedDescription = Dictionary.Get(o.CancelPenaltyDetailedId),
                        MinNights = o.MinLOS,
                        MaxNights = o.MaxLOS,
                        ByDay = o.CancelPriorDays,
                        ByHour = o.CancelPriorHours
                    }
                }

            };

            return offer;
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
                            //CancelPriorHours = offer.Rule.CancelPenalty.ByHour,
                            //CancelPriorDays = offer.Rule.CancelPenalty.ByDay,
                            AdvBooking = offer.Rule.BookingWindow.MinDays != 0 ? offer.Rule.BookingWindow.MinDays : null,
                            MaxAdvBooking = offer.Rule.BookingWindow.MaxDays != 0 ? offer.Rule.BookingWindow.MaxDays : null,
                            //MaxDias = offer.Rule.MaxLOS != 0 ? offer.Rule.MaxLOS : null,
                            //MinDias = offer.Rule.MinLOS != 0 ? offer.Rule.MinLOS : null,
                            MaxDias = offer.Rule.CancelPenalty.MaxNights,
                            MinDias = offer.Rule.CancelPenalty.MaxNights,
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
                            idRatePlan = offer.Id.ToUpper(),
                            Description = offer.Name.Esp,
                            IdHotel = offer.HotelId,
                            Segment = "R",
                            Name = offer.Name.Esp,
                            RateGDS = false,
                            RatePortal = true,
                            RateUnip = false,
                            codigotarifa = offer.Id.ToUpper(),
                            DescPromotion = offer.Discount.Percent != 0 ? offer.Discount.Percent : offer.Discount.Amount != 0 ? offer.Discount.Amount : null,
                            DaysFree = (short)offer.Discount.NightsDiscounted,
                            //CAda  y solo de noche gratis
                            //DaysFreeType = false,
                            DaysFreeType = offer.Discount.DiscountPattern == OfferDiscountDiscountPattern.Only ? true : false,
                            //
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
                            idRule = rule.idRule,
                            IsCombinablePromotion = offer.IsCombinablePromotion
                            
                        };
                        db.RatesPlan.Add(rp);

                        string horaInicio = (offer.Rule.BookingWindow.StartHour == null) ? "" : offer.Rule.BookingWindow.StartHour.Substring(0, 5);
                        string horaFin = (offer.Rule.BookingWindow.EndHour == null) ? "" : offer.Rule.BookingWindow.EndHour.Substring(0, 5);

                        var deal = new ratesPlanDeal()
                        {
                            idRatePlan = offer.Id,
                            idHotel = offer.HotelId,
                            FechaInicio = offer.Rule.BookingWindow.StartDate,
                            FechaFin = offer.Rule.BookingWindow.EndDate,
                            HoraInicio = horaInicio,
                            HoraFin = horaFin,
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


                        //Tiene Fecha para el Cierre
                        if (offer.Rule.ExcludedDates.Count > 0)
                        {
                            //ADD
                            DateTime? start = offer.Rule.ExcludedDates[0].Start;
                            DateTime? end = offer.Rule.ExcludedDates.ElementAt(0).End;

                            var lockRoomTypesList = offer.ApplicableFor.Rooms.Select(r =>
                            new LockRoomTypes()
                            {
                                idhotel = offer.HotelId,
                                IdRatePlan = offer.Id.Trim().ToUpper(),
                                idTipoHabitacion_Hotel = r,
                                StatusAvail = "C",
                                StartDate = start,
                                EndDate = end
                            }
                            ).ToList();

                            db.LockRoomTypes.AddRange(lockRoomTypesList);
                        }

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
                    var rp = db.RatesPlan.FirstOrDefault(x => x.idRatePlan == offer.Id && x.IdHotel == offer.HotelId);
                    if (rp != null)
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
                                rule.AdvBooking = offer.Rule.BookingWindow.MinDays != 0 ? offer.Rule.BookingWindow.MinDays : null;
                                rule.MaxAdvBooking = offer.Rule.BookingWindow.MaxDays != 0 ? offer.Rule.BookingWindow.MaxDays : null;
                                //rule.MaxDias = offer.Rule.MaxLOS != 0 ? offer.Rule.MaxLOS : null;
                                //rule.MinDias = offer.Rule.MinLOS != 0 ? offer.Rule.MinLOS : null;
                                rule.MaxDias = offer.Rule.CancelPenalty.MaxNights;
                                rule.MinDias = offer.Rule.CancelPenalty.MinNights;
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
                        //var rp = db.RatesPlan.FirstOrDefault(x => x.idRatePlan == offer.Id && x.IdHotel == offer.HotelId);
                        if (rp != null)
                        {
                            rp.Description = offer.Name.Esp;
                            rp.Name = offer.Name.Esp;
                            rp.codigotarifa = offer.Id.ToUpper();
                            rp.DescPromotion = offer.Discount.Percent != 0 ? offer.Discount.Percent : offer.Discount.Amount != 0 ? offer.Discount.Amount : null;
                            rp.DaysFree = (short)offer.Discount.NightsDiscounted;
                            //rp.DaysFreeType = false;
                            rp.DaysFreeType = offer.Discount.DiscountPattern == OfferDiscountDiscountPattern.Only ? true : false;
                            rp.DiscountLevel = GetDiscountApplicationMode(offer.Discount.ApplicationMode);
                            rp.TipoDescuento = 1; //Porcentaje. TODO: Monto
                            rp.IsCombinablePromotion = offer.IsCombinablePromotion;

                            //Diccionario
                            Dictionary.Update(offer.Name);
                            Dictionary.Update(offer.Description);
                        }

                        if (offer.Rule.BookingWindow != null)
                        {
                            string horaInicio = (offer.Rule.BookingWindow.StartHour == null || offer.Rule.BookingWindow.StartHour == "") ? "" : offer.Rule.BookingWindow.StartHour.Substring(0, 5);
                            string horaFin = (offer.Rule.BookingWindow.EndHour == null || offer.Rule.BookingWindow.StartHour == "") ? "" : offer.Rule.BookingWindow.EndHour.Substring(0, 5);
                            var deal = db.ratesPlanDeal.FirstOrDefault(x => x.idRatePlan == offer.Id && x.idHotel == offer.HotelId);
                            if (deal != null)
                            {
                                deal.FechaInicio = offer.Rule.BookingWindow.StartDate;
                                deal.FechaFin = offer.Rule.BookingWindow.EndDate;
                                deal.HoraInicio = horaInicio;
                                deal.HoraFin = horaFin;
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
                            var toDeleteRatesPlan = db.Promociones_RatePlan.Where(x => x.IdPromocion == offer.Id && x.IdHotel == offer.HotelId).ToList();
                            if (toDeleteRatesPlan != null)
                            {
                                db.Promociones_RatePlan.RemoveRange(toDeleteRatesPlan);
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

                        
                        //Tiene Fecha para el Cierre
                        if (offer.Rule.ExcludedDates.Count > 0)
                        {
                            //DELETE
                            db.Database.ExecuteSqlCommand("DELETE OzHoteles.dbo.LockRoomTypes WHERE IdRatePlan = {0}  and idHotel = {1}",
                                offer.Id.Trim().ToUpper(), offer.HotelId);
                            //ADD
                            DateTime? start = offer.Rule.ExcludedDates[0].Start;
                            DateTime? end = offer.Rule.ExcludedDates.ElementAt(0).End;

                            var lockRoomTypesList = offer.ApplicableFor.Rooms.Select(r =>
                            new LockRoomTypes()
                            {
                                idhotel = offer.HotelId,
                                IdRatePlan = offer.Id.Trim().ToUpper(),
                                idTipoHabitacion_Hotel = r,
                                StatusAvail = "C",
                                StartDate = start,
                                EndDate = end
                            }
                            ).ToList();

                            db.LockRoomTypes.AddRange(lockRoomTypesList);
                        }
                        else
                        {
                            var idRateClosure = offer.Id.Trim().ToUpper();
                            var hasClosure = db.LockRoomTypes.Any(l => l.idhotel == offer.HotelId && l.IdRatePlan == idRateClosure);

                            if (hasClosure) db.Database.ExecuteSqlCommand("DELETE OzHoteles.dbo.LockRoomTypes WHERE IdRatePlan = {0}  and idHotel = {1}",
                                offer.Id.Trim().ToUpper(), offer.HotelId);

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }

            _disposed = true;

        }



    }
}
