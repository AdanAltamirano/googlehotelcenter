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
        string strError;
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
                        Occupation = x.Children,
                        Type = PaxType.Junior,
                        Price = Utilities.IsInUVMap(dayRate.ExceptionMap, day) ? (x.JuniorExceptionPrice ?? 0) : x.JuniorPrice ?? 0,
                    }).GroupBy(x => x.Occupation).Select(x => x.FirstOrDefault());

                result.Prices.AddRange(juniors);


                // extras
                result.Extras.Add(new DailyRateDetailPrice
                {
                    Occupation = 1,
                    Type = PaxType.Adult,
                    Price = dayRateDetails.FirstOrDefault()?.ExtraAdultPrice ?? 0
                });

                result.Extras.Add(new DailyRateDetailPrice
                {
                    Occupation = 1,
                    Type = PaxType.Child,
                    Price = dayRateDetails.FirstOrDefault()?.ExtraChildPrice ?? 0
                });

                result.Extras.Add(new DailyRateDetailPrice
                {
                    Occupation = 1,
                    Type = PaxType.Junior,
                    Price = dayRateDetails.FirstOrDefault()?.ExtraJuniorPrice ?? 0
                });

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

                result = query.ToArray();
            }

            return result;
        }

        /// <summary>
        /// Búsqueda de un RatePlan por hotel
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="ratePlanId"></param>
        /// <returns>Conjunto de RatesPlan</returns>
        public IEnumerable<RatesPlan> FindHotelRatesPlan(int hotelId)
        {
            IEnumerable<RatesPlan> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                result = db.RatesPlan.Where(r =>
                   r.IdHotel == hotelId).ToArray();
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
                       RatePlanId = r.RatePlanId,
                       RatePlanName = r.RatePlanName,
                       ParentRatePlan = r.ParentRatePlanId,
                       RoomId = r.RoomId,
                       IsPromotion = r.IsPromotion,
                       Currency = r.Currency,
                       Factor = r.Factor,
                       Offset = r.Offset,
                       Discount = r.Discount,
                       DiscountLevel = r.DiscountLevel
                   })
                .Select(r =>
                {
                    var groupedRates = new RatesByRatePlan
                    {
                        RatePlanId = r.Key.RatePlanId,
                        RatePlan = r.Key.RatePlanName,
                        RoomId = r.Key.RoomId,
                        ParentRatePlanId = r.Key.ParentRatePlan,
                        Currency = r.Key.Currency,
                        Factor = r.Key.Factor,
                        Offset = r.Key.Offset,
                        IsPromotion = r.Key.IsPromotion,
                        DiscountLevel = (byte)r.Key.DiscountLevel,
                        Discount = r.Key.Discount
                    };

                    groupedRates.DailyRates = r.SelectMany(rate =>
                    {
                        // cada tarifa siempre traera fecha por lo que es seguro acceder directo 
                        // al valor de start y end date
                        DateTime startDay = startDate >= rate.StartDate ? startDate : rate.StartDate;
                        DateTime endDay = endDate <= rate.EndDate ? endDate : rate.EndDate;

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
                            Discount = rate.DayDiscount
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
            //string strError = "";
            OzHotelesEntities db = new OzHotelesEntities();
            using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //var copy = CopyOverlappedRates(updateRQ.RoomId, updateRQ.RatePlanCode, updateRQ.StartDate, updateRQ.EndDate, ref db);

                    if (updateRQ.RateId > 0)
                    {
                        //Busca la tarifa para crear una copia de las reglas
                        CompleteRateUpdateRQ(ref updateRQ, ref strError);
                        if (strError != "")
                            return new KeyValuePair<string, string>("0", strError);
                    }

                    if (CopyOverlappedRates(updateRQ.RoomId, updateRQ.RatePlanCode, updateRQ.StartDate, updateRQ.EndDate, ref db)
                        && UpdateRatesRate(updateRQ, ref db))
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        return new KeyValuePair<string, string>("0", "AddRate: " + strError);
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new KeyValuePair<string, string>("0", "AddRate: " + e.Message);
                }
                return new KeyValuePair<string, string>("1", "success");
            }
        }

        private bool CopyOverlappedRates(int roomId, string ratePlanId, DateTime startDate, DateTime endDate, ref OzHotelesEntities contextDb)
        {
            bool success = true;
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
                    DateTime startDateAux, endDateAux;
                    //Se recorren las tarifas en conflicto
                    foreach (var of in overlappedFares)
                    {
                        if (startDate > of.FechaInicia && endDate >= of.FechaFinaliza)
                        {
                            startDateAux = startDate;
                            endDateAux = of.FechaFinaliza;
                            //Actualiza la fecha final de la tarifa en conflicto a un día antes de la fecha inicial de la nueva tarifa
                            of.FechaFinaliza = startDate.AddDays(-1);
                            success = InsertCopyRate(startDateAux, endDateAux, of, ref contextDb);
                        }
                        else if (startDate <= of.FechaInicia && endDate < of.FechaFinaliza)
                        {
                            startDateAux = of.FechaInicia;
                            endDateAux = endDate;
                            //Actualiza la fecha inicial de la tarifa en conflicto a un día después de la fecha final de la nueva tarifa
                            of.FechaInicia = endDate.AddDays(1);
                            success = InsertCopyRate(startDateAux, endDateAux, of, ref contextDb);
                        }
                        else if (startDate > of.FechaInicia && endDate < of.FechaFinaliza)
                        {
                            //Actualiza la fecha final de la tarifa en conflicto a un día antes de la ficha inicial de la nueva tarifa
                            DateTime newRateEndDate = of.FechaFinaliza;
                            of.FechaFinaliza = startDate.AddDays(-1);

                            //Copia la tarifa en conflicto con fecha inicial a un día después de la fecha final de la nueva tarifa
                            startDateAux = endDate.AddDays(1);
                            endDateAux = newRateEndDate;

                            success = InsertCopyRate(startDateAux, endDateAux, of, ref contextDb);
                            success = InsertCopyRate(startDate, endDate, of, ref contextDb);
                        }
                        else
                        {
                            continue;
                        }
                    }

                }

                return success;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool UpdateRatesRate(RateUpdateRQ rate, ref OzHotelesEntities contextDb)
        {
            bool isNetRate = false;
            int? newDictionaryId = 0;
            int? promotionDiscount = 0;
            decimal adultRate = -1;
            decimal childRate = -1;
            decimal juniorRate = -1;
            decimal? hotelTaxes = -1;
            decimal extraAdultRate = -1;
            decimal extraChildRate = -1;
            decimal extraJuniorRate = -1;

            bool createNewRate = false;
            DateTime auxIni, auxIni2, auxEnd, auxEnd2;

            int roomId = rate.RoomId;
            string ratePlanId = rate.RatePlanCode;
            DateTime startDate = rate.StartDate;
            DateTime endDate = rate.EndDate;

            IEnumerable<Tarifas> overlappedFares = null;
            IEnumerable<TarifasRestricciones> overlappedFaresRestrictions = null;

            overlappedFares = contextDb.Tarifas.Where(o =>
                    o.idTipoHabitacion_Hotel == roomId
                    && o.idrateplan == ratePlanId
                    && (((startDate >= o.FechaInicia && startDate <= o.FechaFinaliza) || (endDate >= o.FechaInicia && endDate <= o.FechaFinaliza))
                          || ((o.FechaInicia >= startDate && o.FechaInicia <= endDate) || (o.FechaFinaliza >= startDate && o.FechaFinaliza <= endDate)))).ToArray();

            vHotelPlan hotelPlan = contextDb.vHotelPlan.FirstOrDefault(hp => hp.Code == rate.RatePlanCode && hp.HotelId == rate.HotelId);
            vHotelBasicInfo hotelInfo = contextDb.vHotelBasicInfo.FirstOrDefault(h => h.Id == rate.HotelId);
            vHotelRoom hotelRoom = contextDb.vHotelRoom.Where(hr => hr.Id == rate.RoomId && hr.Language == 1).FirstOrDefault();

            hotelTaxes = hotelInfo.Tax;

            if (hotelPlan == null)
                return false;
            if (hotelRoom == null)
                return false;

            isNetRate = (hotelPlan.CommissionPercentage > 0);

            if (rate.Prices != null)
            {
                adultRate = rate.Prices.Base.SingleOrDefault(p => p.Occupation == 1 && p.Type == PaxType.Adult)?.Price ?? -1;
                childRate = rate.Prices.Base.SingleOrDefault(p => p.Occupation == 1 && p.Type == PaxType.Child)?.Price ?? -1;
                juniorRate = rate.Prices.Base.SingleOrDefault(p => p.Occupation == 1 && p.Type == PaxType.Junior)?.Price ?? -1;

                extraAdultRate = rate.Prices.Extra?.SingleOrDefault(ep => ep.Type == PaxType.Adult)?.Price ?? -1;
                extraChildRate = rate.Prices.Extra?.SingleOrDefault(ep => ep.Type == PaxType.Child)?.Price ?? -1;
                extraJuniorRate = rate.Prices.Extra?.SingleOrDefault(ep => ep.Type == PaxType.Junior)?.Price ?? -1;
            }

            if (rate.Prices?.Promotion != null)
            {                
                newDictionaryId = Dictionary.Insert(rate.Prices.Promotion.SpanishDescription, rate.Prices.Promotion.EnglishDescription);
                promotionDiscount = rate.Prices.Promotion.Discount;
                if (newDictionaryId == 0)
                    return false;
            }

            if (overlappedFares.ToArray().Length > 0)
            {
                try
                {
                    foreach (var of in overlappedFares)
                    {
                        of.PrecioExtraAdulto = extraAdultRate != -1 ? extraAdultRate : of.PrecioExtraAdulto; //(decimal)SetPrice(isNetRate, extraAdultRate, (decimal)hotelPlan.CommissionPercentage),
                        of.PrecioExtraNinio = extraChildRate != -1 ? extraChildRate : of.PrecioExtraNinio; //(decimal)SetPrice(isNetRate, extraChildRate, (decimal)hotelPlan.CommissionPercentage),
                        of.PrecioAdolescenteExtra = extraJuniorRate != -1 ? extraJuniorRate : of.PrecioAdolescenteExtra; //(decimal)SetPrice(isNetRate, extraJuniorRate, (decimal)hotelPlan.CommissionPercentage),
                        of.PrecioNR = isNetRate ? adultRate != -1 ? SetPrice(isNetRate, adultRate, (decimal)hotelPlan.CommissionPercentage) : of.PrecioNR : 0;
                        of.NiniosRateNR = isNetRate ? childRate != -1 ? SetPrice(isNetRate, childRate, (decimal)hotelPlan.CommissionPercentage) : of.NiniosRateNR : 0;
                        of.PrecioAdolescenteNR = isNetRate ? juniorRate != -1 ? SetPrice(isNetRate, juniorRate, (decimal)hotelPlan.CommissionPercentage) : of.PrecioAdolescenteNR : 0;
                        of.PrecioExtraAdultoNR = isNetRate ? extraAdultRate != -1 ? SetPrice(isNetRate, extraAdultRate, (decimal)hotelPlan.CommissionPercentage) : of.PrecioExtraAdultoNR : 0;
                        of.PrecioExtraNinioNR = isNetRate ? extraChildRate != -1 ? SetPrice(isNetRate, extraChildRate, (decimal)hotelPlan.CommissionPercentage) : of.PrecioExtraNinioNR : 0;
                        of.PrecioAdolescenteExtraNR = isNetRate ? extraJuniorRate != -1 ? SetPrice(isNetRate, extraJuniorRate, (decimal)hotelPlan.CommissionPercentage) : of.PrecioAdolescenteExtraNR : 0;
                        of.NoArrivos = rate.Rules?.NoArrival ?? of.NoArrivos;
                        of.Excepciones = rate.Prices?.ExceptionDays ?? of.Excepciones;
                        of.RateRulesDefault = rate.Rules?.UseDefaultRules ?? true;
                        of.PrecioAdolescente = juniorRate != -1 ? juniorRate : of.PrecioAdolescente;
                        of.NiniosRate = childRate != -1 ? childRate : of.NiniosRate;
                        of.Precio = adultRate != -1 ? adultRate : of.Precio;
                        of.idDiccPromoDesc = newDictionaryId == 0 ? null : newDictionaryId;
                        of.DescPromotion = promotionDiscount == 0 ? null : promotionDiscount;
                        of.AdvBooking = rate.Rules?.MinAdvanceBooking ?? of.AdvBooking;
                        of.MaxAdvBooking = rate.Rules?.MaxAdvanceBooking ?? of.MaxAdvBooking;
                        of.MinDias = rate.Rules?.MinLOS ?? of.MinDias;
                        of.MaxDias = rate.Rules?.MaxLOS ?? of.MaxDias;
                        of.BookingWindowStart = rate.Rules?.BookingWindow?.StartDate ?? of.BookingWindowStart;
                        of.BookingWindowEnd = rate.Rules?.BookingWindow?.EndDate ?? of.BookingWindowEnd;
                        of.Personas = rate.Rules?.GuestsRestrictions?.MaxGuests ?? of.Personas;
                        of.PersonasExtras = rate.Rules?.GuestsRestrictions?.ExtraGuests ?? of.PersonasExtras;
                        of.MaxAdultos = rate.Rules?.GuestsRestrictions?.MaxAdults ?? of.MaxAdultos;
                        of.MinAdultos = rate.Rules?.GuestsRestrictions?.MinAdults ?? of.MinAdultos;
                        of.MaxNinios = rate.Rules?.GuestsRestrictions?.Children ?? of.MaxNinios;

                        if (rate.Prices != null)
                        {
                            foreach (var price in rate.Prices.Base)
                            {
                                List<TarifasRestricciones> tarifasRestricciones = new List<TarifasRestricciones>();
                                if (price.Type == PaxType.Child || price.Type == PaxType.Junior)
                                {
                                    overlappedFaresRestrictions = rate.IsOccupancyRate ? contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa && tr.Ninios == price.Occupation) : contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa);
                                    foreach (var ofRes in overlappedFaresRestrictions)
                                    {
                                        if (price.Type == PaxType.Child)
                                        {
                                            ofRes.TarifaNinio = price.Price;
                                            ofRes.TarifaNinioNR = isNetRate ? SetPrice(isNetRate, price.Price, (decimal)hotelPlan.CommissionPercentage) : 0;
                                        }
                                        else
                                        {
                                            ofRes.TarifaAdolescente = price.Price;
                                            ofRes.TarifaAdolescenteNR = isNetRate ? SetPrice(isNetRate, price.Price, (decimal)hotelPlan.CommissionPercentage) : 0;
                                        }
                                    }
                                }
                                else if (price.Type == PaxType.Adult)
                                {
                                    overlappedFaresRestrictions = rate.IsOccupancyRate ? contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa && tr.Adultos == price.Occupation) : contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa);
                                    foreach (var ofRes in overlappedFaresRestrictions)
                                    {
                                        ofRes.TarifaAdulto = price.Price;
                                        ofRes.TarifaAdultoNR = isNetRate ? SetPrice(isNetRate, price.Price, (decimal)hotelPlan.CommissionPercentage) : 0;
                                    }
                                }
                            }
                        }
                        if (rate.StartDate >= of.FechaInicia && rate.EndDate > of.FechaFinaliza)
                        {
                            auxIni = of.FechaFinaliza.AddDays(1);
                            auxEnd = rate.EndDate;
                            if (!IsOverlappedFares(rate, auxIni, auxEnd, ref contextDb))
                            {
                                rate.StartDate = auxIni;
                                rate.EndDate = auxEnd;
                                if (!InsertRate(rate, ref contextDb))
                                    return false;
                            }
                        }
                        else if (rate.StartDate < of.FechaInicia && rate.EndDate <= of.FechaFinaliza)
                        {
                            auxEnd = of.FechaInicia.AddDays(-1);
                            auxIni = rate.StartDate;
                            if (!IsOverlappedFares(rate, auxIni, auxEnd, ref contextDb))
                            {
                                rate.StartDate = auxIni;
                                rate.EndDate = auxEnd;
                                if (!InsertRate(rate, ref contextDb))
                                    return false;
                            }
                        }
                        else if (rate.StartDate < of.FechaInicia && rate.EndDate > of.FechaFinaliza)
                        {
                            auxIni = rate.StartDate;
                            auxEnd = of.FechaInicia.AddDays(-1);
                            auxIni2 = of.FechaFinaliza.AddDays(1);
                            auxEnd2 = rate.EndDate;                            
                            if (!IsOverlappedFares(rate, auxIni, auxEnd, ref contextDb))
                            {
                                rate.StartDate = auxIni;
                                rate.EndDate = auxEnd;
                                if (!InsertRate(rate, ref contextDb))
                                    return false;
                            }
                            if (!IsOverlappedFares(rate, auxIni2, auxEnd2, ref contextDb))
                            {
                                rate.StartDate = auxIni2;
                                rate.EndDate = auxEnd2;
                                if (!InsertRate(rate, ref contextDb))
                                    return false;
                            }
                        }
                        else if (rate.StartDate == of.FechaInicia && rate.EndDate == of.FechaFinaliza)
                        {
                            createNewRate = false;
                        }

                    }

                    contextDb.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }


            var newRate = new Tarifas
            {
                idTipoHabitacion_Hotel = rate.RoomId,
                FechaFinaliza = rate.EndDate,
                FechaInicia = rate.StartDate,
                PrecioExtraAdulto = extraAdultRate != -1 ? extraAdultRate : 0,
                PrecioExtraNinio = extraChildRate != -1 ? extraChildRate : 0,
                PrecioAdolescenteExtra = extraJuniorRate != -1 ? extraJuniorRate : 0,
                PrecioNR = isNetRate ? SetPrice(isNetRate, adultRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                NiniosRateNR = isNetRate ? SetPrice(isNetRate, childRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                PrecioAdolescenteNR = isNetRate ? juniorRate != -1 ? SetPrice(isNetRate, juniorRate, (decimal)hotelPlan.CommissionPercentage) : 0 : 0,
                PrecioExtraAdultoNR = isNetRate ? extraAdultRate != -1 ? SetPrice(isNetRate, extraAdultRate, (decimal)hotelPlan.CommissionPercentage) : 0 : 0,
                PrecioExtraNinioNR = isNetRate ? extraChildRate != -1 ? SetPrice(isNetRate, extraChildRate, (decimal)hotelPlan.CommissionPercentage) : 0 : 0,
                PrecioAdolescenteExtraNR = isNetRate ? extraJuniorRate != -1 ? SetPrice(isNetRate, extraJuniorRate, (decimal)hotelPlan.CommissionPercentage) : 0 : 0,
                idrateplan = rate.RatePlanCode,
                NoArrivos = rate.Rules?.NoArrival ?? "NNNNNNN",
                Excepciones = rate.Prices.ExceptionDays ?? "NNNNNNN",
                RateRulesDefault = rate.Rules?.UseDefaultRules ?? true,
                TipoTarifa = hotelPlan.Segment,
                CodigoTarifa = hotelRoom.Code + rate.RatePlanCode,
                PrecioAdolescente = juniorRate != -1 ? juniorRate : 0,
                NiniosRate = childRate != -1 ? childRate : 0,
                Precio = adultRate != -1 ? adultRate : 0, //(decimal)SetPrice(isNetRate, adultRate, (decimal)hotelPlan.CommissionPercentage),
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
            catch (Exception e)
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
                        if (startDate > of.FechaInicia && endDate >= of.FechaFinaliza)
                        {
                            //Actualiza la fecha final de la tarifa en conflicto a un día antes de la ficha inicial de la nueva tarifa
                            of.FechaFinaliza = startDate.AddDays(-1);
                        }
                        else if (startDate <= of.FechaInicia && endDate < of.FechaFinaliza)
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
            var adultPrice = rate.Prices.Base.FirstOrDefault(p => p.Type == PaxType.Adult);
            if (adultPrice == null)
            {
                strError = "No se permite crear tarifas sin precio Adulto";
                return false;
            }
                

            bool isNetRate = false;
            int? newDictionaryId = 0;
            int? promotionDiscount = 0;
            decimal adultRate = 0;
            decimal childRate = 0;
            decimal juniorRate = 0;
            decimal? hotelTaxes = 0;
            decimal extraAdultRate = 0;
            decimal extraChildRate = 0;
            decimal extraJuniorRate = 0;

            if (rate.Prices?.Promotion != null)
            {
                newDictionaryId = Dictionary.Insert(rate.Prices.Promotion.SpanishDescription, rate.Prices.Promotion.EnglishDescription);
                promotionDiscount = rate.Prices.Promotion.Discount;
                if (newDictionaryId == 0)
                    return false;
            }

            vHotelPlan hotelPlan = contextDb.vHotelPlan.FirstOrDefault(hp => hp.Code == rate.RatePlanCode && hp.HotelId == rate.HotelId);
            vHotelBasicInfo hotelInfo = contextDb.vHotelBasicInfo.FirstOrDefault(h => h.Id == rate.HotelId);
            vHotelRoom hotelRoom = contextDb.vHotelRoom.Where(hr => hr.Id == rate.RoomId && hr.Language == 1).FirstOrDefault();

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
                PrecioExtraAdulto = extraAdultRate, //(decimal)SetPrice(isNetRate, extraAdultRate, (decimal)hotelPlan.CommissionPercentage),
                PrecioExtraNinio = extraChildRate, //(decimal)SetPrice(isNetRate, extraChildRate, (decimal)hotelPlan.CommissionPercentage),
                PrecioAdolescenteExtra = extraJuniorRate, //(decimal)SetPrice(isNetRate, extraJuniorRate, (decimal)hotelPlan.CommissionPercentage),
                PrecioNR = isNetRate ? SetPrice(isNetRate, adultRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                NiniosRateNR = isNetRate ? SetPrice(isNetRate, childRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                PrecioAdolescenteNR = isNetRate ? SetPrice(isNetRate, extraJuniorRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                PrecioExtraAdultoNR = isNetRate ? SetPrice(isNetRate, extraAdultRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                PrecioExtraNinioNR = isNetRate ? SetPrice(isNetRate, extraChildRate, (decimal)hotelPlan.CommissionPercentage) : 0,
                PrecioAdolescenteExtraNR = isNetRate ? extraJuniorRate : 0,
                idrateplan = rate.RatePlanCode,
                NoArrivos = rate.Rules?.NoArrival ?? "NNNNNNN",
                Excepciones = rate.Prices.ExceptionDays ?? "NNNNNNN",
                RateRulesDefault = rate.Rules?.UseDefaultRules ?? true,
                TipoTarifa = hotelPlan.Segment,
                CodigoTarifa = hotelRoom.Code + rate.RatePlanCode,
                PrecioAdolescente = juniorRate,
                NiniosRate = childRate,
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
                                TarifaAdultoExc = prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Adult)?.Price ?? 0,
                                TarifaNinioExc = childs > 0 ? prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Child)?.Price ?? 0 : 0,
                                TarifaAdolescenteExc = childs > 0 ? prices.Exceptions?.SingleOrDefault(p => p.Type == PaxType.Junior)?.Price ?? 0 : 0,
                                TarifaAdultoNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Type == PaxType.Adult)?.Price ?? 0, commissionPercentage) : 0,
                                TarifaNinioNR = childs > 0 ? isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Type == PaxType.Child)?.Price ?? 0, commissionPercentage) : 0 : 0,
                                TarifaAdolescenteNR = childs > 0 ? isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Type == PaxType.Junior)?.Price ?? 0, commissionPercentage) : 0 : 0,
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
                                TarifaAdultoExc = prices.Exceptions?.SingleOrDefault(p => p.Occupation == adults && p.Type == PaxType.Adult)?.Price ?? 0,
                                TarifaNinioExc = prices.Exceptions?.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Child)?.Price ?? 0,
                                TarifaAdolescenteExc = prices.Exceptions?.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Junior)?.Price ?? 0,
                                TarifaAdultoNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Occupation == adults && p.Type == PaxType.Adult)?.Price ?? 0, commissionPercentage) : 0,
                                TarifaNinioNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Child)?.Price ?? 0, commissionPercentage) : 0,
                                TarifaAdolescenteNR = isNetRate ? SetPrice(isNetRate, prices.Base.SingleOrDefault(p => p.Occupation == childs && p.Type == PaxType.Junior)?.Price ?? 0, commissionPercentage) : 0,
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

        private bool IsOverlappedFares(RateUpdateRQ rate, DateTime ini, DateTime end, ref OzHotelesEntities db)
        {
            int roomId = rate.RoomId;
            string ratePlanId = rate.RatePlanCode;
            IEnumerable<Tarifas> overlappedFares = null;

            overlappedFares = db.Tarifas.Where(o =>
                    o.idTipoHabitacion_Hotel == roomId
                    && o.idrateplan == ratePlanId
                    && (((ini >= o.FechaInicia && ini <= o.FechaFinaliza) || (end >= o.FechaInicia && end <= o.FechaFinaliza))
                          || ((o.FechaInicia >= ini && o.FechaInicia <= end) || (o.FechaFinaliza >= ini && o.FechaFinaliza <= end)))).ToArray();

            if (overlappedFares.ToArray().Length > 0)
                return true;
            else
                return false;
        }

        private bool InsertCopyRate(DateTime startDate, DateTime endDate, Tarifas rate, ref OzHotelesEntities contextDb)
        {
            IEnumerable<TarifasRestricciones> overlappedFaresRestrictions = null;
            var newRateAux = new Tarifas
            {
                idTipoHabitacion_Hotel = rate.idTipoHabitacion_Hotel,
                FechaInicia = startDate,
                FechaFinaliza = endDate,
                Precio = rate.Precio,
                PrecioExtraAdulto = rate.PrecioExtraAdulto,
                PrecioExtraNinio = rate.PrecioExtraNinio,
                idHotelPlan = rate.idHotelPlan,
                TipoTarifa = rate.TipoTarifa,
                CodigoTarifa = rate.CodigoTarifa,
                CorporateDiscount = rate.CorporateDiscount,
                AccessCode = rate.AccessCode,
                Excepciones = rate.Excepciones,
                AdvBooking = rate.AdvBooking,
                MaxAdvBooking = rate.MaxAdvBooking,
                MaxDias = rate.MaxDias,
                MinDias = rate.MinDias,
                NoArrivos = rate.NoArrivos,
                GuarDep = rate.GuarDep,
                idrateplan = rate.idrateplan,
                NiniosRate = rate.NiniosRate,
                MaxAdultos = rate.MaxAdultos,
                MinAdultos = rate.MinAdultos,
                MaxNinios = rate.MaxNinios,
                PersonasExtras = rate.PersonasExtras,
                Personas = rate.Personas,
                PrecioAdolescenteExtra = rate.PrecioAdolescenteExtra,
                PrecioAdolescenteExtraNR = rate.PrecioAdolescenteExtraNR,
                PrecioExtraAdultoNR = rate.PrecioExtraAdultoNR,
                PrecioExtraNinioNR = rate.PrecioExtraNinioNR,
                PrecioAdolescente = rate.PrecioAdolescente,
                PrecioAdolescenteNR = rate.PrecioAdolescenteNR,
                PrecioNR = rate.PrecioNR,
                BookingWindowEnd = rate.BookingWindowEnd,
                BookingWindowStart = rate.BookingWindowStart,
                NiniosRateNR = rate.NiniosRateNR,
                DescPromotion = rate.DescPromotion
            };

            contextDb.Tarifas.Add(newRateAux);
            //Copia las restricciones (precios por ocupación) de la tarifa en conflicto para agregarlos a la nueva tarifa
            List<TarifasRestricciones> tarifasRestricciones = new List<TarifasRestricciones>();
            overlappedFaresRestrictions = contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == rate.idTarifa);
            foreach (var ofRes in overlappedFaresRestrictions)
            {
                tarifasRestricciones.Add(
                    new TarifasRestricciones
                    {
                        idTarifa = newRateAux.idTarifa,
                        TarifaAdulto = ofRes.TarifaAdulto,
                        TarifaNinio = ofRes.TarifaNinio,
                        Adultos = ofRes.Adultos,
                        Ninios = ofRes.Ninios,
                        TarifaAdultoExc = ofRes.TarifaAdultoExc,
                        TarifaNinioExc = ofRes.TarifaNinioExc,
                        TarifaAdolescente = ofRes.TarifaAdolescente,
                        TarifaAdolescenteExc = ofRes.TarifaAdolescenteExc,
                        TarifaAdolescenteExcNR = ofRes.TarifaAdolescenteExcNR,
                        TarifaAdolescenteNR = ofRes.TarifaAdolescenteNR,
                        TarifaAdultoExcNR = ofRes.TarifaAdultoExcNR,
                        TarifaAdultoNR = ofRes.TarifaAdultoNR,
                        TarifaNinioExcNR = ofRes.TarifaNinioExcNR,
                        TarifaNinioNR = ofRes.TarifaNinioNR,
                        Applyday = ofRes.Applyday
                    }
                    );
            }
            contextDb.TarifasRestricciones.AddRange(tarifasRestricciones);
            contextDb.SaveChanges();
            return true;
        }
    }
}
