using APIServices.Models;
using APIServices.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIServices
{
    /// <summary>
    /// Métodos para consulta y manejo de tarifas
    /// </summary>
    public class RatesService
    {
        /// <summary>
        /// Búsqueda de tarifas diarias
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="language"></param>
        /// <param name="roomId"></param>
        /// <returns>Conjunto de tarifas</returns>
        public DailyRateDetail FindDayRateDetail(int rateId, DateTime day)
        {
            DailyRateDetail result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                vDayRates dayRate = db.vDayRates.FirstOrDefault(x => x.RateId == rateId && x.ParentRatePlanId == null);

                result = new DailyRateDetail
                {
                    Currency = dayRate.Currency,
                    Date = day,
                    Discount = dayRate.Discount,
                    RateId = dayRate.RateId,
                };

                var dayRateDetails = db.vDayRateDetail.Where(r => r.RateId == rateId).ToArray();

                //adultos
                var adultRates = dayRateDetails.Where(x => x.Children == 0)
                    .Select(x => new DailyRateDetailPrice
                    {
                        Id = x.Id,
                        RateId = x.RateId,
                        Occupation = x.Adults,
                        Type = PaxType.Adult,
                        Price = Utilities.IsInUVMap(dayRate.ExceptionMap, day) ? (x.AdultExceptionPrice ?? 0) : x.AdultPrice,
                    });

                result.Prices.AddRange(adultRates);
                // niños
                var children = dayRateDetails.Where(x => x.Children > 0)
                    .Select(x => new DailyRateDetailPrice
                    {
                        Id = x.Id,
                        RateId = x.RateId,
                        Occupation = x.Children,
                        Type = PaxType.Child,
                        Price = Utilities.IsInUVMap(dayRate.ExceptionMap, day) ? (x.ChildExceptionPrice ?? 0) : x.ChildPrice,
                    }).GroupBy(x => x.Occupation).Select(x => x.FirstOrDefault());
                result.Prices.AddRange(children);
                //juniors
                var juniors = dayRateDetails
                    .Where(x => x.Children > 0 && (
                        (x.JuniorPrice != null && x.JuniorPrice > 0) ||
                        (x.JuniorExceptionPrice != null && x.JuniorExceptionPrice > 0)
                        ))
                    .Select(x => new DailyRateDetailPrice
                    {
                        Id = x.Id,
                        RateId = x.RateId,
                        Occupation = x.Children,
                        Type = PaxType.Junior,
                        Price = Utilities.IsInUVMap(dayRate.ExceptionMap, day) ? (x.JuniorExceptionPrice ?? 0) : x.JuniorPrice ?? 0,
                    }).GroupBy(x => x.Occupation).Select(x => x.FirstOrDefault());
                result.Prices.AddRange(juniors);

            }

            return result;
        }

        /// <summary>
        /// Búsqueda de tarifas diarias
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="language"></param>
        /// <param name="roomId"></param>
        /// <returns>Conjunto de tarifas</returns>
        private IEnumerable<vDayRates> FindDayRates(int hotelId, DateTime startDate, DateTime endDate, int language = 1, int? roomId = null)
        {
            IEnumerable<vDayRates> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                var query = db.vDayRates.Where(r =>
                   r.HotelId == hotelId
                   && r.StartDate <= endDate
                   && r.EndDate >= startDate
                   && r.Language == language);

                if (roomId != null)
                    query = query.Where(r => r.RoomId == roomId);

                result = query.OrderBy(r => r.StartDate).ToArray();
            }

            return result;
        }

        /// <summary>
        /// Búsqueda de un RatePlan por hotel
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="ratePlanId"></param>
        /// <returns>Conjunto de RatesPlan</returns>
        private IEnumerable<RatesPlan> FindHotelRatePlan(int hotelId, string ratePlanId)
        {
            IEnumerable<RatesPlan> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                result = db.RatesPlan.Where(r =>
                   r.IdHotel == hotelId
                   && r.idRatePlan == ratePlanId).ToArray();
            }

            return result;
        }

        /// <summary>
        /// Búsqueda de tarifas agrupadas por rate plan
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="language"></param>
        /// <param name="hotelRoomId"></param>
        /// <returns></returns>
        public IEnumerable<RatesByRatePlan> FindGroupedByRatePlan(int hotelId, DateTime startDate, DateTime endDate, int? hotelRoomId = null, int language = 1)
        {
            IEnumerable<vDayRates> dayRates = FindDayRates(hotelId, startDate, endDate, language, hotelRoomId);

            var result = dayRates
                .GroupBy(r =>
                   new
                   {
                       r.RatePlanId,
                       r.RatePlanName,
                       r.RoomId,
                       r.ParentRatePlanId,
                       r.Currency,
                       r.IsPromotion,
                       r.Factor,
                       r.Offset
                   })
                .Select(r =>
                {
                    var groupedRates = new RatesByRatePlan
                    {
                        RatePlanId = r.Key.RatePlanId,
                        RatePlan = r.Key.RatePlanName,
                        RoomId = r.Key.RoomId,
                        ParentRatePlanId = r.Key.ParentRatePlanId,
                        Currency = r.Key.Currency,
                        Factor = r.Key.Factor,
                        Offset = r.Key.Offset,
                        IsPromotion = r.Key.IsPromotion
                    };

                    groupedRates.DailyRates = r.SelectMany(rate =>
                    {
                        // cada tarifa siempre traera fecha por lo que es seguro acceder directo 
                        // al valor de start y end date
                        DateTime startDay = startDate >= rate.StartDate.Value ? startDate : rate.StartDate.Value;
                        DateTime endDay = endDate <= rate.EndDate.Value ? endDate : rate.EndDate.Value;

                        // arreglo de días que se usara para dividir el rango de las tarifas por día
                        DateTime[] days = Enumerable.Range(0, 1 + endDay.Subtract(startDay).Days)
                        .Select(offset => startDay.AddDays(offset))
                        .ToArray();

                        return days.Select(d =>
                        new DailyRate
                        {
                            Date = d,
                            RateId = rate.RateId,
                            Occupancy = rate.Occupancy,
                            Price = Utilities.IsInUVMap(rate.ExceptionMap, d) ? rate.ExceptionPrice : rate.Price,
                            NoArrival = Utilities.IsInUVMap(rate.NoArrivalsMap, d) ? (bool?)true : null,
                            Discount = rate.Discount
                        });

                    }).ToArray();

                    // agregar fechas en las que no se cargo tarifa y estarán vacias
                    var fixedDays = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                    .Select(offset => startDate.AddDays(offset))
                    .Where(d => !groupedRates.DailyRates.Any(x => x.Date == d)).Select(d => new DailyRate { Date = d });

                    groupedRates.DailyRates = groupedRates.DailyRates.Concat(fixedDays).OrderBy(d => d.Date).ToArray();

                    return groupedRates;
                }
               );

            return result;
        }

        /// <summary>
        /// Si RateId es 0, inserta una nueva tarifa reemplazando cualquier otra tarifa que esté dentro del nuevo rango de fechas.
        /// Si RateId es mayor a 0, busca la tarifa para crear una copia de las reglas e insertar una nueva tarifa con el día y precios recibidos.
        /// </summary>
        /// <param name="updateRQ"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> AddRate(RateUpdateRQ updateRQ)
        {
            string strError = "";
            OzHotelesEntities db = new OzHotelesEntities();
            using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (updateRQ.RateId > 0)
                    {
                        //Busca la tarifa para crear una copia de las reglas
                        CompleteRateUpdateRQ(ref updateRQ, ref strError);
                        if(strError != "")
                            return new KeyValuePair<string, string>("0", strError);
                    }

                    if (RemoveOverlappedRates(updateRQ.RoomId, updateRQ.RatePlanCode, updateRQ.StartDate, updateRQ.EndDate, ref db)
                        && InsertRate(updateRQ, ref db))
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        return new KeyValuePair<string, string>("0", "AddRate: An error ocurred. Could not save rate");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new KeyValuePair<string, string>("0", "AddRate: " + e.Message);
                }
                return new KeyValuePair<string, string>("1","success");
            }
        }

        /// <summary>
        /// Busca la tarifa para crear una copia de las reglas
        /// </summary>
        /// <param name="updateRQ"></param>
        /// <returns></returns>
        private bool CompleteRateUpdateRQ(ref RateUpdateRQ updateRQ, ref string strError)
        {
            try
            {
                using (OzHotelesEntities db = new OzHotelesEntities())
                {
                    int rateId = updateRQ.RateId;
                    var rate = db.Tarifas?.Single(t => t.idTarifa == rateId);

                    RateUpdateRQBookingWindow bookingWindow = new RateUpdateRQBookingWindow
                    {
                        StartDate = rate.BookingWindowStart,
                        EndDate = rate.BookingWindowEnd
                    };

                    RateUpdateRQGuestsRestriction guestsRestriction = new RateUpdateRQGuestsRestriction
                    {
                        MaxAdults = rate.MaxAdultos,
                        MinAdults = rate.MinAdultos,
                        ExtraGuests = rate.PersonasExtras,
                        Children = rate.MaxNinios,
                        MaxGuests = rate.Personas
                    };

                    RateUpdateRQRules rules = new RateUpdateRQRules
                    {
                        NoArrival = rate.NoArrivos,
                        UseDefaultRules = rate.RateRulesDefault,
                        Segment = rate.TipoTarifa,
                        MinLOS = rate.MinDias,
                        MaxLOS = rate.MaxDias,
                        MaxAdvanceBooking = rate.MaxAdvBooking,
                        MinAdvanceBooking = rate.AdvBooking,
                        BookingWindow = bookingWindow,
                        GuestsRestrictions = guestsRestriction
                    };
                    updateRQ.Rules = rules;
                }
            }
            catch(Exception e)
            {
                strError = "CompleteRateUpdateRQ: " + e.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ordena las tarifas que estén completa o parcialmente dentro del rango de la nueva tarifa.
        /// </summary>
        /// <param name="contextDb"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private bool RemoveOverlappedRates(int roomId, string ratePlanId, DateTime startDate, DateTime endDate, ref OzHotelesEntities contextDb)
        {
            try
            {
                IEnumerable<Tarifas> overlappedFares = null;
                IEnumerable<TarifasRestricciones> overlappedFaresRestrictions = null;

                //Obtengo las tarifas que estén completa o parcialmente dentro del rango de la nueva tarifa
                overlappedFares = contextDb.Tarifas.Where(o =>
                    o.idTipoHabitacion_Hotel == roomId
                    && o.idrateplan == ratePlanId
                    && (((startDate >= o.FechaInicia && startDate <= o.FechaFinaliza) || (endDate >= o.FechaInicia && endDate <= o.FechaFinaliza))
                          || ((o.FechaInicia >= startDate && o.FechaInicia <= endDate) || (o.FechaFinaliza >= startDate && o.FechaFinaliza <= endDate)))).ToArray();

                if (overlappedFares.Count() > 0)
                {
                    //Se recorren las tarifas en conflicto
                    foreach (var of in overlappedFares)
                    {
                        if (startDate > of.FechaInicia && endDate > of.FechaFinaliza)
                        {
                            //Actualiza la fecha final de la tarifa en conflicto a un día antes de la ficha inicial de la nueva tarifa
                            of.FechaFinaliza = startDate.AddDays(-1);
                        }
                        else if (startDate < of.FechaInicia && endDate < of.FechaFinaliza)
                        {
                            //Actualiza la fecha inicial de la tarifa en conflicto a un día después de la fecha final de la nueva tarifa
                            of.FechaInicia = endDate.AddDays(1);
                        }
                        else if (startDate > of.FechaInicia && endDate < of.FechaFinaliza)
                        {
                            //Actualiza la fecha final de la tarifa en conflicto a un día antes de la ficha inicial de la nueva tarifa
                            DateTime newRateEndDate = of.FechaFinaliza;
                            of.FechaFinaliza = startDate.AddDays(-1);

                            //Copia la tarifa en conflicto con fecha inicial a un día después de la fecha final de la nueva tarifa
                            var newRate = new Tarifas
                            {
                                idTipoHabitacion_Hotel = of.idTipoHabitacion_Hotel,
                                FechaInicia = endDate.AddDays(1),
                                FechaFinaliza = newRateEndDate,
                                Precio = of.Precio,
                                PrecioExtraAdulto = of.PrecioExtraAdulto,
                                PrecioExtraNinio = of.PrecioExtraNinio,
                                idHotelPlan = of.idHotelPlan,
                                TipoTarifa = of.TipoTarifa,
                                CodigoTarifa = of.CodigoTarifa,
                                CorporateDiscount = of.CorporateDiscount,
                                AccessCode = of.AccessCode,
                                Excepciones = of.Excepciones,
                                AdvBooking = of.AdvBooking,
                                MaxDias = of.MaxDias,
                                MinDias = of.MinDias,
                                NoArrivos = of.NoArrivos,
                                GuarDep = of.GuarDep,
                                idrateplan = of.idrateplan,
                                NiniosRate = of.NiniosRate
                            };
                            contextDb.Tarifas.Add(newRate);

                            //Copia las restricciones (precios por ocupación) de la tarifa en conflicto para agregarlos a la nueva tarifa
                            List<TarifasRestricciones> tarifasRestricciones = new List<TarifasRestricciones>();
                            overlappedFaresRestrictions = contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa);
                            foreach (var ofRes in overlappedFaresRestrictions)
                            {
                                tarifasRestricciones.Add(
                                    new TarifasRestricciones
                                    {
                                        idTarifa = newRate.idTarifa,
                                        TarifaAdulto = ofRes.TarifaAdulto,
                                        TarifaNinio = ofRes.TarifaNinio,
                                        Adultos = ofRes.Adultos,
                                        Ninios = ofRes.Ninios,
                                        TarifaAdultoExc = ofRes.TarifaAdultoExc,
                                        TarifaNinioExc = ofRes.TarifaNinioExc
                                    }
                                    );
                            }
                            contextDb.TarifasRestricciones.AddRange(tarifasRestricciones);

                        }
                        else
                        {
                            if (of.idDiccPromoDesc != null && of.idDiccPromoDesc != 0)
                            {
                                contextDb.Indice.Remove(contextDb.Indice.Where(i => i.idDiccionario == of.idDiccPromoDesc).ToArray()[0]);
                                contextDb.Diccionario.RemoveRange(contextDb.Diccionario.Where(d => d.IdDiccionario == of.idDiccPromoDesc).ToArray());
                            }
                            overlappedFaresRestrictions = contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa);
                            contextDb.TarifasRestricciones.RemoveRange(overlappedFaresRestrictions);
                            contextDb.Tarifas.Remove(of);
                        }

                    }
                    contextDb.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertRate(RateUpdateRQ rate, ref OzHotelesEntities contextDb)
        {
            bool isNetRate = false;
            int? newDictionaryId = 0;
            int? promotionDiscount = 0;
            decimal adultRate = 0;
            decimal childRate = 0;
            decimal juniorRate = 0;
            decimal? hotelTaxes = 0;
            decimal? extraAdultRate = 0;
            decimal? extraChildRate = 0;
            decimal? extraJuniorRate = 0;

            if (rate.Prices?.Promotion != null)
            {
                Dictionary dictionary = new Dictionary();
                newDictionaryId = dictionary.Insert(rate.Prices.Promotion.SpanishDescription, rate.Prices.Promotion.EnglishDescription);
                promotionDiscount = rate.Prices.Promotion.Discount;
                if (newDictionaryId == 0)
                    return false;
            }

            vHotelPlan hotelPlan = contextDb.vHotelPlan.FirstOrDefault(hp => hp.Code == rate.RatePlanCode && hp.HotelId == rate.HotelId);
            vHotelBasicInfo hotelInfo = contextDb.vHotelBasicInfo.FirstOrDefault(h => h.Id == rate.HotelId);
            vHotelRoom hotelRoom = contextDb.vHotelRoom.FirstOrDefault(hr => hr.Id == rate.RoomId);

            hotelTaxes = hotelInfo.Tax;

            if (hotelPlan == null)
                return false;
            if (hotelRoom == null)
                return false;

            adultRate = rate.Prices.Base.SingleOrDefault(p => p.Occupation == 1 && p.Type == PaxType.Adult)?.Price ?? 0;
            childRate = rate.Prices.Base.SingleOrDefault(p => p.Occupation == 1 && p.Type == PaxType.Child)?.Price ?? 0;
            juniorRate = rate.Prices.Base.SingleOrDefault(p => p.Occupation == 1 && p.Type == PaxType.Junior)?.Price ?? 0;

            extraAdultRate = rate.Prices.Extra?.SingleOrDefault(ep => ep.Type == PaxType.Adult)?.Price ?? 0;
            extraChildRate = rate.Prices.Extra?.SingleOrDefault(ep => ep.Type == PaxType.Child)?.Price ?? 0;
            extraJuniorRate = rate.Prices.Extra?.SingleOrDefault(ep => ep.Type == PaxType.Junior)?.Price ?? 0;

            isNetRate = (hotelPlan.CommissionPercentage > 0);

            var newRate = new Tarifas
            {
                idTipoHabitacion_Hotel = rate.RoomId,
                FechaFinaliza = rate.EndDate,
                FechaInicia = rate.StartDate,
                PrecioExtraAdulto = (decimal)SetPrice(isNetRate, extraAdultRate, (decimal)hotelPlan.CommissionPercentage),
                PrecioExtraNinio = (decimal)SetPrice(isNetRate, extraChildRate, (decimal)hotelPlan.CommissionPercentage),
                PrecioAdolescenteExtra = (decimal)SetPrice(isNetRate, extraJuniorRate, (decimal)hotelPlan.CommissionPercentage),
                PrecioNR = isNetRate ? SetPrice(isNetRate, adultRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                NiniosRateNR = isNetRate ? childRate : 0,
                PrecioAdolescenteNR = isNetRate ? juniorRate : 0,
                PrecioExtraAdultoNR = isNetRate ? extraAdultRate : 0,
                PrecioExtraNinioNR = isNetRate ? extraChildRate : 0,
                PrecioAdolescenteExtraNR = isNetRate ? extraJuniorRate : 0,
                idrateplan = rate.RatePlanCode,
                NoArrivos = rate.Rules?.NoArrival ?? "NNNNNNN",
                Excepciones = rate.Prices.ExceptionDays ?? "NNNNNNN",
                RateRulesDefault = rate.Rules?.UseDefaultRules ?? true,
                TipoTarifa = hotelPlan.Segment,
                CodigoTarifa = hotelRoom.Code + rate.RatePlanCode,
                PrecioAdolescente = SetPrice(isNetRate, juniorRate, (decimal)hotelPlan.CommissionPercentage),
                NiniosRate = SetPrice(isNetRate, childRate, (decimal)hotelPlan.CommissionPercentage),
                Precio = adultRate, //(decimal)SetPrice(isNetRate, adultRate, (decimal)hotelPlan.CommissionPercentage),
                idDiccPromoDesc = newDictionaryId == 0 ? null : newDictionaryId,
                DescPromotion = promotionDiscount == 0 ? null : promotionDiscount,
                AdvBooking = rate.Rules?.MinAdvanceBooking ?? null,
                MaxAdvBooking = rate.Rules?.MaxAdvanceBooking ?? null,
                MinDias = rate.Rules?.MinLOS ?? null,
                MaxDias = rate.Rules?.MaxLOS ?? null,
                BookingWindowStart = rate.Rules?.BookingWindow?.StartDate ?? null,
                BookingWindowEnd = rate.Rules?.BookingWindow?.EndDate ?? null,
                Personas = rate.Rules?.GuestsRestrictions?.MaxGuests ?? null,
                PersonasExtras = rate.Rules?.GuestsRestrictions?.ExtraGuests ?? null,
                MaxAdultos = rate.Rules?.GuestsRestrictions?.MaxAdults ?? null,
                MinAdultos = rate.Rules?.GuestsRestrictions?.MinAdults ?? null,
                MaxNinios = rate.Rules?.GuestsRestrictions?.Children ?? null
            };

            contextDb.Tarifas.Add(newRate);
            contextDb.SaveChanges();

            if (!InsertGuestsRates(newRate, isNetRate, (decimal)hotelPlan.CommissionPercentage, rate.Prices, rate.IsOccupancyRate, ref contextDb))
            {
                return false;
            }

            return true;
        }

        private bool InsertGuestsRates(Tarifas newRate, bool isNetRate, decimal commissionPercentage, RateUpdatePrices prices, bool isOccupancyRates, ref OzHotelesEntities contextDb)
        {
            vHotelRoom hotelRoom = contextDb.vHotelRoom.FirstOrDefault(r => r.Id == newRate.idTipoHabitacion_Hotel);
            if (hotelRoom == null)
                return false;

            int adults, childs = 0;
            List<TarifasRestricciones> guestRates = new List<TarifasRestricciones>();

            try
            {
                if (!isOccupancyRates)
                {
                    for (adults = hotelRoom.MaxAdultsOccupancy; adults > 0; adults--)
                    {
                        for (childs = hotelRoom.MaxChildrenOccupancy; childs >= 0; childs--)
                        {
                            guestRates.Add(new TarifasRestricciones
                            {
                                idTarifa = newRate.idTarifa,
                                Adultos = adults,
                                Ninios = childs,
                                TarifaAdulto = prices.Base.SingleOrDefault(p => p.Type == PaxType.Adult).Price, 
                                TarifaNinio = childs > 0 ? prices.Base.SingleOrDefault(p => p.Type == PaxType.Child)?.Price ?? 0 : 0,
                                TarifaAdolescente = childs > 0 ? prices.Base.SingleOrDefault(p => p.Type == PaxType.Junior)?.Price ?? 0 : 0,
                                TarifaAdultoExc = prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Adult)?.Price,
                                TarifaNinioExc = childs > 0 ? prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Child)?.Price : 0,
                                TarifaAdolescenteExc = childs > 0 ? prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Junior)?.Price : 0,
                                TarifaAdultoNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Type == PaxType.Adult)?.Price ?? 0, commissionPercentage) : null,
                                TarifaNinioNR = childs > 0 ? isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Type == PaxType.Child)?.Price ?? 0, commissionPercentage) : null : 0,
                                TarifaAdolescenteNR = childs > 0 ? isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Type == PaxType.Junior)?.Price ?? 0, commissionPercentage) : null : 0,
                                TarifaAdultoExcNR = isNetRate ? SetPrice(isNetRate, prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Adult)?.Price ?? 0, commissionPercentage) : 0,
                                TarifaNinioExcNR = childs > 0 ? isNetRate ? SetPrice(isNetRate, prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Child)?.Price ?? 0, commissionPercentage) : 0 : 0,
                                TarifaAdolescenteExcNR = childs > 0 ? isNetRate ? SetPrice(isNetRate, prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Junior)?.Price ?? 0, commissionPercentage) : 0 : 0,
                                Applyday = newRate.Excepciones
                            });
                        }
                    }
                }
                else
                {
                    for (adults = hotelRoom.MaxAdultsOccupancy; adults > 0; adults--)
                    {
                        for (childs = hotelRoom.MaxChildrenOccupancy; childs >= 0; childs--)
                        {
                            guestRates.Add(new TarifasRestricciones
                            {
                                idTarifa = newRate.idTarifa,
                                Adultos = adults,
                                Ninios = childs,
                                TarifaAdulto = prices.Base.SingleOrDefault(p => p.Occupation == adults && p.Type == PaxType.Adult).Price,
                                TarifaNinio = prices.Base.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Child)?.Price ?? 0,
                                TarifaAdolescente = prices.Base.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Junior)?.Price ?? 0,
                                TarifaAdultoExc = prices.Exceptions?.SingleOrDefault(p => p.Occupation == adults && p.Type == PaxType.Adult)?.Price,
                                TarifaNinioExc = prices.Exceptions?.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Child)?.Price,
                                TarifaAdolescenteExc = prices.Exceptions?.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Junior)?.Price,
                                TarifaAdultoNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Occupation == adults && p.Type == PaxType.Adult)?.Price ?? 0, commissionPercentage) : null,
                                TarifaNinioNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Child)?.Price ?? 0, commissionPercentage) : null,
                                TarifaAdolescenteNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Junior)?.Price ?? 0, commissionPercentage) : null,
                                TarifaAdultoExcNR = isNetRate ? SetPrice(isNetRate, prices.Exceptions?.SingleOrDefault(p => p.Occupation == adults && p.Type == PaxType.Adult)?.Price ?? 0, commissionPercentage) : 0,
                                TarifaNinioExcNR = isNetRate ? SetPrice(isNetRate, prices.Exceptions?.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Child)?.Price ?? 0, commissionPercentage) : 0,
                                TarifaAdolescenteExcNR = isNetRate ? SetPrice(isNetRate, prices.Exceptions?.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Junior)?.Price ?? 0, commissionPercentage) : 0,
                                Applyday = newRate.Excepciones
                            });
                        }
                    }
                }

                contextDb.TarifasRestricciones.AddRange(guestRates);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private decimal? SetPrice(bool isNetRate, decimal? price, decimal commissionPercentage)
        {
            try
            {
                decimal? NetPrice = 0;
                if (isNetRate)
                {
                    NetPrice = price * ((100 - commissionPercentage) / 100);
                    return NetPrice;
                }

                return price;
            }
            catch
            {
                return 0;
            }
        }
    }
}
