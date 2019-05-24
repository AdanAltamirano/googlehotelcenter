using APIServices.Models;
using APIServices.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

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
                DayRates dayRate = db.DayRates.FirstOrDefault(x => x.RateId == rateId && x.ParentRatePlanId == null);

                result = new DailyRateDetail
                {
                    Currency = dayRate.Currency,
                    Date = day,
                    Discount = dayRate.Discount,
                    RateId = dayRate.RateId,
                };

                var dayRateDetails = db.DayRateDetail.Where(r => r.RateId == rateId).ToArray();

                //adultos
                var adultRates = dayRateDetails.Where(x => x.Children == 0)
                    .Select(x => new DailyRateDetailPrice
                    {
                        Id = x.Id,
                        RateId = x.RateId,
                        Occupation = x.Adults,
                        Type = PaxType.Adult,
                        Price = Utilities.IsInExceptionPrice(dayRate.ExceptionMap, day) ? (x.AdultExceptionPrice ?? 0) : x.AdultPrice,
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
                        Price = Utilities.IsInExceptionPrice(dayRate.ExceptionMap, day) ? (x.ChildExceptionPrice ?? 0) : x.ChildPrice,
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
                        Price = Utilities.IsInExceptionPrice(dayRate.ExceptionMap, day) ? (x.JuniorExceptionPrice ?? 0) : x.JuniorPrice ?? 0,
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
        private IEnumerable<DayRates> FindDayRates(int hotelId, DateTime startDate, DateTime endDate, int language = 1, int? roomId = null)
        {
            IEnumerable<DayRates> result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                var query = db.DayRates.Where(r =>
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
        public IEnumerable<RatesByRatePlan> FindGroupedByRatePlan(int hotelId, DateTime startDate, DateTime endDate, int language = 1, int? hotelRoomId = null)
        {
            IEnumerable<DayRates> dayRates = FindDayRates(hotelId, startDate, endDate, language, hotelRoomId);

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
                            Price = Utilities.IsInExceptionPrice(rate.ExceptionMap, d) ? rate.ExceptionPrice : rate.Price,
                            Discount = rate.Discount
                        });

                    }).ToArray();

                    // agregar fechas en las que no se cargo tarifa y estarán vacias
                    var fixedDays = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                    .Select(offset => startDate.AddDays(offset))
                    .Where(d => !groupedRates.DailyRates.Any(x => x.Date == d)).Select(d => new DailyRate { Date = d });

                    groupedRates.DailyRates = groupedRates.DailyRates.Concat(fixedDays).ToArray();

                    return groupedRates;
                }
               );

            return result;
        }

        public bool AddRate(int hotelId, UpdateRateRequest rate)
        {
            int err = 0;
            OzHotelesEntities db = new OzHotelesEntities();

            using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (RemoveOverlappedRates(rate.RateId, rate.RoomId, rate.RatePlanId, rate.StartDate, rate.EndDate, ref db)
                        || InsertRate(hotelId, rate, ref db))
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        if (!SplitRate(ref err, rate.StartDate, rate.EndDate, rate.RoomId, rate.RatePlanId, ref db))
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        private bool RemoveOverlappedRates(int rateId, int roomId, string ratePlanId, DateTime startDate, DateTime endDate, ref OzHotelesEntities contextDb)
        {
            IEnumerable<Tarifas> overlappedFares = null;
            IEnumerable<TarifasRestricciones> overlappedFaresRestrictions = null;

            overlappedFares = contextDb.Tarifas.Where(o =>
                o.idTarifa != rateId
                && o.idTipoHabitacion_Hotel == roomId
                && o.idrateplan == ratePlanId
                && (((startDate >= o.FechaInicia && startDate <= o.FechaFinaliza) || (endDate >= o.FechaInicia && endDate <= o.FechaFinaliza))
                      || ((o.FechaInicia >= startDate && o.FechaInicia <= endDate) || (o.FechaFinaliza >= startDate && o.FechaFinaliza <= endDate)))).ToArray();

            if (overlappedFares.Count() > 0)
            {
                foreach (var of in overlappedFares)
                {
                    overlappedFaresRestrictions = contextDb.TarifasRestricciones.Where(tr => tr.idTarifa == of.idTarifa);
                    contextDb.TarifasRestricciones.RemoveRange(overlappedFaresRestrictions);

                    contextDb.Tarifas.Remove(of);
                }
                return true;
            }
            return false;
        }

        private bool SplitRate(ref int err, DateTime startDate, DateTime endDate, int roomId, string ratePlanId, ref OzHotelesEntities contextDb)
        {
            try
            {

                SqlDataAdapter dsCommand = new SqlDataAdapter();
                string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["HotelConnectionString"];
                SqlConnection Connection = new SqlConnection(ConnectionString);
                dsCommand = new SqlDataAdapter("spSplitRate", Connection);
                {
                    try
                    {
                        {
                            dsCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                            dsCommand.SelectCommand.CommandText = "spSplitRate";
                            dsCommand.SelectCommand.Parameters.Add("@fechaInicia", SqlDbType.SmallDateTime).Value = startDate;
                            dsCommand.SelectCommand.Parameters.Add("@fechaFinaliza", SqlDbType.SmallDateTime).Value = endDate;
                            dsCommand.SelectCommand.Parameters.Add("@idTipoHabitacion", SqlDbType.Int).Value = roomId;
                            dsCommand.SelectCommand.Parameters.Add("@idRatePlan", SqlDbType.VarChar, 8).Value = ratePlanId;
                            dsCommand.SelectCommand.Parameters.Add("@error", SqlDbType.Int).Direction = ParameterDirection.Output;
                            dsCommand.SelectCommand.Connection.Open();
                        }
                        dsCommand.SelectCommand.ExecuteNonQuery();
                        err = (int)dsCommand.SelectCommand.Parameters["@error"].Value;
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        dsCommand.SelectCommand.Connection.Close();
                        if (dsCommand.SelectCommand != null)
                        {
                            if (dsCommand.SelectCommand.Connection != null)
                                dsCommand.SelectCommand.Connection.Dispose();
                            dsCommand.SelectCommand.Dispose();
                        }
                        dsCommand.Dispose();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                var a = ex.ToString();
            }
        }

        private bool InsertRate(int hotelId, UpdateRateRequest rate, ref OzHotelesEntities contextDb)
        {
            IEnumerable<RatesPlan> ratePlan = FindHotelRatePlan(hotelId, rate.RatePlanId);

            if (ratePlan.Count() <= 0)
                return false;

            decimal adultRatePerRoom = 0;
            decimal childRatePerRoom = 0;
            decimal juniorRatePerRoom = 0;

            foreach (var price in rate.Prices)
            {
                if (price.Type == PaxType.Adult && price.Occupation == 1)
                {
                    adultRatePerRoom = price.Price;
                }

                if (price.Type == PaxType.Child && price.Occupation == 1)
                {
                    childRatePerRoom = price.Price;
                }

                if (price.Type == PaxType.Junior && price.Occupation == 1)
                {
                    juniorRatePerRoom = price.Price;
                }
            }

            var newRate = new Tarifas
            {
                idTarifa = rate.RateId,
                idTipoHabitacion_Hotel = rate.RoomId,
                FechaFinaliza = rate.EndDate,
                FechaInicia = rate.StartDate,
                PrecioExtraAdulto = rate.ExtraAdultPrice,
                PrecioExtraNinio = rate.ExtraChildPrice,
                PrecioAdolescenteExtra = rate.ExtraJuniorPrice,
                idrateplan = rate.RatePlanId,
                NoArrivos = rate.Rules.NoArrival ?? "YYYYYYY",
                Excepciones = rate.Rules.ExceptionDays ?? "NNNNNNN",
                RateRulesDefault = rate.Rules.UseDefaultRules ?? false,
                PrecioAdolescente = juniorRatePerRoom,
                NiniosRate = childRatePerRoom,
                Precio = adultRatePerRoom
            };

            contextDb.Tarifas.Add(newRate);
            return true;
        }
    }
}
