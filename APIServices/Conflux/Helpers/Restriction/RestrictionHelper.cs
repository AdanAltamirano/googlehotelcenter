using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux;
using APIServices.Conflux.OTA.Models.Restrictions;
using System.Globalization;
using APIServices.Conflux.Models.Restrictions.Rate;

namespace APIServices.Conflux.Helpers.Restriction
{
    public static partial class RestrictionHelper
    {
        public static RestrictionStatus GetRestrictionStatus(string status)
        {
            RestrictionStatus restrictionStatus = new RestrictionStatus();

            switch (status)
            {
                case "C":
                    restrictionStatus.Status = "Close";
                    break;
                case "N":
                    restrictionStatus.Status = "Close";
                    restrictionStatus.Restriction = "Arrival";
                    break;
                case "O":
                    restrictionStatus.Status = "Open";
                    break;
            }

            return restrictionStatus;
        }

        public static List<Tuple<DateTime,DateTime>> GetActiveDates(DateTime startDate, DateTime endDate, string promoDays)
        {
            List<Tuple<DateTime,DateTime>> datesList = new List<Tuple<DateTime,DateTime>>();

            DateTime currentDate = startDate;
            DateTime activeStartDate = DateTime.MinValue;
            DateTime inactiveStartDate = DateTime.MinValue;

            while (currentDate <= endDate)
            {
                DayOfWeek dayOfTheWeek = currentDate.DayOfWeek;

                if (promoDays[Convert.ToInt32(dayOfTheWeek)] == 'Y')
                {
                    if (activeStartDate == DateTime.MinValue) activeStartDate = currentDate;

                    if (inactiveStartDate != DateTime.MinValue)
                    {
                        Console.WriteLine("Rango inactivo: " + inactiveStartDate.ToString("yyyy-MM-dd") + " - " + currentDate.AddDays(-1).ToString("yyyy-MM-dd"));
                        inactiveStartDate = DateTime.MinValue;
                    }
                }
                else
                {
                    if (inactiveStartDate == DateTime.MinValue) inactiveStartDate = currentDate;

                    if (activeStartDate != DateTime.MinValue)
                    {
                        Console.WriteLine("Rango activo: " + activeStartDate.ToString("yyyy-MM-dd") + " - " + currentDate.AddDays(-1).ToString("yyyy-MM-dd"));

                        var date = new Tuple<DateTime, DateTime>(activeStartDate,currentDate.AddDays(-1));                      
                       
                        datesList.Add(date);

                        activeStartDate = DateTime.MinValue;
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            if (activeStartDate != DateTime.MinValue)
            {
                var date = new Tuple<DateTime, DateTime>(activeStartDate, endDate);

                datesList.Add(date);

                Console.WriteLine("Rango activo: " + activeStartDate.ToString("yyyy-MM-dd") + " - " + endDate.ToString("yyyy-MM-dd"));
            }

            return datesList;

        }

        public static string ToDayOfWeek(string days)
        {
            char[] applyDays = new char[7];
            applyDays[0] = days[6];
            applyDays[1] = days[0];
            applyDays[2] = days[1];
            applyDays[3] = days[2];
            applyDays[4] = days[3];
            applyDays[5] = days[4];
            applyDays[6] = days[5];

            return new string(applyDays);
        }

        public static void RemoveRatePlansNoValids(ref List<DataRow> activeRatePlans)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            List<DataRow> activeRatePlansTemp = new List<DataRow>();

            foreach (DataRow row in activeRatePlans)
            {
                var segment = row.ItemArray[1].ToString();
                var isMobileRate = bool.Parse(row.ItemArray[40].ToString());
                var isCallCenterOnly = bool.Parse(row.ItemArray[41].ToString());

                if (isMobileRate || isCallCenterOnly || segment.IndexOfAny(segmentsNoRates) > -1)
                {
                }
                else
                {
                    activeRatePlansTemp.Add(row);
                }
            }

            activeRatePlans = activeRatePlansTemp;

        }

        public static void AllRatePlans(ref List<DataRow> activeRatePlans)
        {

            List<DataRow> activeRatePlansTemp = new List<DataRow>();

            foreach (DataRow row in activeRatePlans)
            {
                activeRatePlansTemp.Add(row);             
            }

            activeRatePlans = activeRatePlansTemp;

        }



        public static List<spGetLockRatePlansByHotel_Result> CreateLockRatePlansByHotel(List<Tuple<DateTime,DateTime>> dates, string status, string rateplanId)
        {
            List<spGetLockRatePlansByHotel_Result> lockRatePlans = new List<spGetLockRatePlansByHotel_Result>();

            foreach (var date in dates)
            {
                spGetLockRatePlansByHotel_Result lockRatePlan = new spGetLockRatePlansByHotel_Result();

                lockRatePlan.StartDate = date.Item1;
                lockRatePlan.EndDate = date.Item2;
                lockRatePlan.Status = status;
                lockRatePlan.RatePlanId = rateplanId;

                lockRatePlans.Add(lockRatePlan);

            }

            return lockRatePlans;
        }

        public static void CheckDatesCalendar(DateTime? startDate, DateTime? endDate, ref List<AvailStatusMessage> availStatusMessageList)
        {

            foreach (var availStatusMessage in availStatusMessageList)
            {

                DateTime availStartDateTemp = availStatusMessage.StatusApplicationControl.Start;
                DateTime availEndDateTemp = availStatusMessage.StatusApplicationControl.End;

                if (startDate >= availStartDateTemp && endDate <= availEndDateTemp)
                {
                    availStatusMessage.StatusApplicationControl.Start = startDate.Value.Date;
                    availStatusMessage.StatusApplicationControl.End = endDate.Value.Date;
                }
                else if (startDate <= availEndDateTemp && endDate >= availStartDateTemp)
                {
                    
                    if (startDate < availStartDateTemp && (endDate >= availStartDateTemp && endDate <= availEndDateTemp))
                    {
                        availStatusMessage.StatusApplicationControl.End = endDate.Value.Date;
                    }
                    else if (endDate > availEndDateTemp && (startDate >= availStartDateTemp && startDate <= availEndDateTemp))
                    {
                        availStatusMessage.StatusApplicationControl.Start = startDate.Value.Date;
                    }
                    else if (startDate < availStartDateTemp && endDate > availEndDateTemp)
                    {
                        //No se cambian las fechas, van las fechas del cierre
                    }

                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="restricion"></param>
        /// <returns></returns>
        public static List<spGetRestrictionsByHotel_Result> GetAllRestrictionsByAvailableRate(int hotelId, Models.Restrictions.Restricion restricion)
        {
            List<APIServices.Conflux.spGetRestrictionsByHotel_Result> listRestrictionsByHotel = new List<APIServices.Conflux.spGetRestrictionsByHotel_Result>();

            using (ConfluxEntities confluxEntities = new ConfluxEntities())
            {

                if ((restricion.RatePlansList.Length == 1 && restricion.RatePlansList[0] == "0") &&
                   (restricion.RoomsList.Length == 1 && restricion.RoomsList[0] == 0))
                {
                    // Todos los planes con todas las habitaciones

                    listRestrictionsByHotel = confluxEntities.spGetRestrictionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, null, null).ToList();

                }
                else if ((restricion.RatePlansList.Length == 1 && restricion.RatePlansList[0] == "0") &&
                            ((restricion.RoomsList.Length == 1 && restricion.RoomsList[0] != 0) || restricion.RoomsList.Length > 1))
                {
                    // Todos los planes con habitaciones seleccionadas

                    foreach (var roomId in restricion.RoomsList)
                    {
                        var restrictionsByHotelTemp = confluxEntities.spGetRestrictionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, roomId, null).ToList();
                        listRestrictionsByHotel.AddRange(restrictionsByHotelTemp);
                    }


                }
                else if ((restricion.RoomsList.Length == 1 && restricion.RoomsList[0] == 0) &&
                            ((restricion.RatePlansList.Length == 1 && restricion.RatePlansList[0] != "0") || restricion.RatePlansList.Length > 1))
                {
                    // Todas las habitaciones con planes seleccionados

                    foreach (var rateplanId in restricion.RatePlansList)
                    {
                        var restrictionsByHotelTemp = confluxEntities.spGetRestrictionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, null, rateplanId).ToList();
                        listRestrictionsByHotel.AddRange(restrictionsByHotelTemp);
                    }

                }
                else
                {
                    // Planes seleccionados con habitaciones seleccionadas

                    foreach (var roomId in restricion.RoomsList)
                    {
                        foreach (var rateplanId in restricion.RatePlansList)
                        {
                            var restrictionsByHotelTemp = confluxEntities.spGetRestrictionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, roomId, rateplanId).ToList();
                            listRestrictionsByHotel.AddRange(restrictionsByHotelTemp);
                        }
                    }
                }
            }

            return listRestrictionsByHotel;
        }

        public static List<spGetRestrictionsExceptionsByHotel_Result> GetAllRestrictionsByAvailableRateException(int hotelId, Models.Restrictions.Restricion restricion)
        {
            List<APIServices.Conflux.spGetRestrictionsExceptionsByHotel_Result> listRestrictionsByHotel = new List<APIServices.Conflux.spGetRestrictionsExceptionsByHotel_Result>();

            using (ConfluxEntities confluxEntities = new ConfluxEntities())
            {

                if ((restricion.RatePlansList.Length == 1 && restricion.RatePlansList[0] == "0") &&
                   (restricion.RoomsList.Length == 1 && restricion.RoomsList[0] == 0))
                {
                    // Todos los planes con todas las habitaciones

                    listRestrictionsByHotel = confluxEntities.spGetRestrictionsExceptionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, null, null).ToList();

                }
                else if ((restricion.RatePlansList.Length == 1 && restricion.RatePlansList[0] == "0") &&
                            ((restricion.RoomsList.Length == 1 && restricion.RoomsList[0] != 0) || restricion.RoomsList.Length > 1))
                {
                    // Todos los planes con habitaciones seleccionadas

                    foreach (var roomId in restricion.RoomsList)
                    {
                        var restrictionsByHotelTemp = confluxEntities.spGetRestrictionsExceptionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, roomId, null).ToList();
                        listRestrictionsByHotel.AddRange(restrictionsByHotelTemp);
                    }


                }
                else if ((restricion.RoomsList.Length == 1 && restricion.RoomsList[0] == 0) &&
                            ((restricion.RatePlansList.Length == 1 && restricion.RatePlansList[0] != "0") || restricion.RatePlansList.Length > 1))
                {
                    // Todas las habitaciones con planes seleccionados

                    foreach (var rateplanId in restricion.RatePlansList)
                    {
                        var restrictionsByHotelTemp = confluxEntities.spGetRestrictionsExceptionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, null, rateplanId).ToList();
                        listRestrictionsByHotel.AddRange(restrictionsByHotelTemp);
                    }

                }
                else
                {
                    // Planes seleccionados con habitaciones seleccionadas

                    foreach (var roomId in restricion.RoomsList)
                    {
                        foreach (var rateplanId in restricion.RatePlansList)
                        {
                            var restrictionsByHotelTemp = confluxEntities.spGetRestrictionsExceptionsByHotel(hotelId, restricion.StartDate.Value.Date, restricion.EndDate.Value.Date, roomId, rateplanId).ToList();
                            listRestrictionsByHotel.AddRange(restrictionsByHotelTemp);
                        }
                    }
                }
            }

            return listRestrictionsByHotel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public static List<RateRestrictionDto> BuildRestrictionsByAvaillableRate(List<spGetRestrictionsByHotel_Result> restrictions,int hotelId)
        {
            return restrictions
                .GroupBy(x => x.RateId)
                .Select(g =>
                {
                        var baseRow = g.First();

                        return new RateRestrictionDto
                        {
                           RateId = baseRow.RateId,
                           RoomId = baseRow.RoomId,
                           RoomCode = baseRow.RoomCode,
                           StartDate = baseRow.StarDate,
                           EndDate = baseRow.EndDate,

                           RestrictionsHotel = new HotelRestrictionsDto
                           {
                               HotelId = hotelId,
                               HotelMinDays = baseRow.HotelMinDays,
                               HotelMaxDays = baseRow.HotelMaxDays,
                               HotelMinAdvDays = baseRow.HotelMinAdvDays,
                               HotelMaxAdvDays = baseRow.HotelMaxAdvDays
                           },

                           RoomsLinked = string.IsNullOrEmpty(baseRow.RoomsLinked)
                               ? new List<string>()
                               : baseRow.RoomsLinked.Split(',').ToList(),

                                RatePlan = new RatePlanDto
                                {
                                    RatePlanId = baseRow.RatePlanId,
                                    RateCode = baseRow.RateCode,

                                    RestrictionsRate = new RateRestrictionsDto
                                    {
                                        MinDays = baseRow.MinDays,
                                        MaxDays = baseRow.MaxDays,
                                        MinAdvDays = baseRow.MinAdvDays,
                                        MaxAdvDays = baseRow.MaxAdvDays
                                    },

                                    RestrictionsRatePlan = new RatePlanRestrictionsDto
                                    {
                                        RatePlanMinDays = baseRow.RatePlanMinDays,
                                        RatePlanMaxDays = baseRow.RatePlanMaxDays,
                                        RatePlanMinAdvDays = baseRow.RatePlanMinAdvDays,
                                        RatePlanMaxAdvDays = baseRow.RatePlanMaxAdvDays
                                    },

                                    LinkedRatePlans = g
                                        .Where(x => x.HasRatePlanLinked == 1)
                                        .Select(x => new LinkedRatePlanDto
                                        {
                                            RateCode = x.RateCodeLinked,
                                            MinDays = x.RatePlanLinkedMinDays,
                                            MaxDays = x.RatePlanLinkedMaxDays,
                                            MinAdvDays = x.RatePlanLinkedMinAdvDays,
                                            MaxAdvDays = x.RatePlanLinkedMaxAdvDays
                                        })
                                        .GroupBy(x => x.RateCode) // evita duplicados
                                        .Select(x => x.First())
                                        .ToList()
                                }
                        };
                }).ToList();
        }

        public static List<RateRestrictionDto> BuildRestrictionsByAvaillableRate(List<spGetRestrictionsExceptionsByHotel_Result> restrictions, int hotelId)
        {
            return restrictions
                .GroupBy(x => x.RateId)
                .Select(g =>
                {
                    var baseRow = g.First();

                    return new RateRestrictionDto
                    {
                        RateId = baseRow.RateId,
                        RoomId = baseRow.RoomId,
                        RoomCode = baseRow.RoomCode,
                        StartDate = baseRow.StartDate,
                        EndDate = baseRow.EndDate,

                        RestrictionsHotel = new HotelRestrictionsDto
                        {
                            HotelId = hotelId,
                            HotelMinDays = baseRow.HotelMinDays,
                            HotelMaxDays = baseRow.HotelMaxDays,
                            HotelMinAdvDays = baseRow.HotelMinAdvDays,
                            HotelMaxAdvDays = baseRow.HotelMaxAdvDays
                        },

                        RoomsLinked = string.IsNullOrEmpty(baseRow.RoomsLinked)
                           ? new List<string>()
                           : baseRow.RoomsLinked.Split(',').ToList(),

                        RatePlan = new RatePlanDto
                        {
                            RatePlanId = baseRow.RatePlanId,
                            RateCode = baseRow.RateCode,

                            RestrictionsRate = new RateRestrictionsDto
                            {
                                MinDays = baseRow.MinDays,
                                MaxDays = baseRow.MaxDays,
                                MinAdvDays = baseRow.MinAdvDays,
                                MaxAdvDays = baseRow.MaxAdvDays
                            },

                            RestrictionsRatePlan = new RatePlanRestrictionsDto
                            {
                                RatePlanMinDays = baseRow.RatePlanMinDays,
                                RatePlanMaxDays = baseRow.RatePlanMaxDays,
                                RatePlanMinAdvDays = baseRow.RatePlanMinAdvDays,
                                RatePlanMaxAdvDays = baseRow.RatePlanMaxAdvDays
                            },

                            LinkedRatePlans = g
                                    .Where(x => x.HasRatePlanLinked == 1)
                                    .Select(x => new LinkedRatePlanDto
                                    {
                                        RateCode = x.RateCodeLinked,
                                        MinDays = x.RatePlanLinkedMinDays,
                                        MaxDays = x.RatePlanLinkedMaxDays,
                                        MinAdvDays = x.RatePlanLinkedMinAdvDays,
                                        MaxAdvDays = x.RatePlanLinkedMaxAdvDays
                                    })
                                    .GroupBy(x => x.RateCode) // evita duplicados
                                    .Select(x => x.First())
                                    .ToList()
                        }
                    };
                }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRestrictionDto"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public static void AddPromotionsByAvailableRate(ref List<RateRestrictionDto> listRestrictionDto, DateTime? startDate, DateTime? endDate)
        {
            using (ConfluxEntities confluxEntities = new ConfluxEntities())
            { 

                foreach (var restriction in listRestrictionDto)
                {


                    var listPromotions = confluxEntities.vRatesPromotions.Where(vrp => vrp.RateId == restriction.RateId && vrp.HotelId == vrp.HotelId
                                                                     && vrp.RoomId == restriction.RoomId && vrp.DeletedRatePlan == false
                                                                     && vrp.Language == 1 && vrp.IsMobileRate == false && vrp.IsCallCenterOnly == false
                                                                     && !new[] { "N", "C", "O" }.Contains(vrp.Segment));                         
                    foreach(var promotion in listPromotions)
                    {
                        if (IsPromotionValid(promotion, startDate, endDate))
                        {
                            PromotionDto promotionTemp = new PromotionDto()
                            {
                                RateId = promotion.RateId,
                                HotelId = promotion.HotelId,
                                RatePlanId = promotion.RatePlanId,
                                RoomId = promotion.RoomId,
                                RoomCode = promotion.RoomCode,
                                ParentRatePlanId = promotion.ParentRatePlanId,
                                PromoRatePlanId = promotion.PromoRatePlanId,
                                PromotionMinDays = promotion.MinDays,
                                PromotionMaxDays = promotion.MaxDays,
                                PromotionMinAdvDays = promotion.MinAdvDays,
                                PromotionMaxAdvDays = promotion.MaxAdvDays,
                                StartDate = promotion.PromoStartDateTravelWindow.Value,
                                EndDate = promotion.PromoEndDateTravelWindow.Value
                            };

                            restriction.Promotions.Add(promotionTemp);
                        }
                    }

                }
            }
        }

        public static void AddPromotionsByAvailableRateException(ref List<RateRestrictionDto> listRestrictionDto, DateTime? startDate, DateTime? endDate)
        {
            using (ConfluxEntities confluxEntities = new ConfluxEntities())
            {

                foreach (var restriction in listRestrictionDto)
                {


                    var listPromotions = confluxEntities.vRatesPromotionsExceptions.Where(vrp => vrp.RateId == restriction.RateId && vrp.HotelId == vrp.HotelId
                                                                     && vrp.RoomId == restriction.RoomId && vrp.DeletedRatePlan == false
                                                                     && vrp.Language == 1 && vrp.IsMobileRate == false && vrp.IsCallCenterOnly == false
                                                                     && !new[] { "N", "C", "O" }.Contains(vrp.Segment));
                    foreach (var promotion in listPromotions)
                    {
                        if (IsPromotionValid(promotion, startDate, endDate))
                        {
                            PromotionDto promotionTemp = new PromotionDto()
                            {
                                RateId = promotion.RateId,
                                HotelId = promotion.HotelId,
                                RatePlanId = promotion.RatePlanId,
                                RoomId = promotion.RoomId,
                                RoomCode = promotion.RoomCode,
                                ParentRatePlanId = promotion.ParentRatePlanId,
                                PromoRatePlanId = promotion.PromoRatePlanId,
                                PromotionMinDays = promotion.MinDays,
                                PromotionMaxDays = promotion.MaxDays,
                                PromotionMinAdvDays = promotion.MinAdvDays,
                                PromotionMaxAdvDays = promotion.MaxAdvDays,
                                StartDate = promotion.PromoStartDateTravelWindow.Value,
                                EndDate = promotion.PromoEndDateTravelWindow.Value
                            };

                            restriction.Promotions.Add(promotionTemp);
                        }
                    }

                }
            }
        }


        public static bool IsPromotionValid(vRatesPromotion promotion,DateTime? searchStart, DateTime? searchEnd)
        {
            // Booking Window
            if (promotion.PromoStartDateBookingWindow.HasValue && promotion.PromoEndDateBookingWindow.HasValue)
            {
                if (!IsOverlapping(searchStart, searchEnd,promotion.PromoStartDateBookingWindow.Value,promotion.PromoEndDateBookingWindow.Value))
                {
                    return false;
                }
            }

            // TravelWindow
            if (promotion.PromoStartDateTravelWindow.HasValue && promotion.PromoEndDateTravelWindow.HasValue)
            {
                if (!IsOverlapping(searchStart, searchEnd, promotion.PromoStartDateTravelWindow.Value, promotion.PromoEndDateTravelWindow.Value))
                {
                    return false;
                }
            }

            // Rate
            if (promotion.RateStartDate.HasValue && promotion.RateEndDate.HasValue)
            {
                if (!IsOverlapping(searchStart, searchEnd, promotion.RateStartDate.Value, promotion.RateEndDate.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsPromotionValid(vRatesPromotionsException promotion, DateTime? searchStart, DateTime? searchEnd)
        {
            // Booking Window
            if (promotion.PromoStartDateBookingWindow.HasValue && promotion.PromoEndDateBookingWindow.HasValue)
            {
                if (!IsOverlapping(searchStart, searchEnd, promotion.PromoStartDateBookingWindow.Value, promotion.PromoEndDateBookingWindow.Value))
                {
                    return false;
                }
            }

            // TravelWindow
            if (promotion.PromoStartDateTravelWindow.HasValue && promotion.PromoEndDateTravelWindow.HasValue)
            {
                if (!IsOverlapping(searchStart, searchEnd, promotion.PromoStartDateTravelWindow.Value, promotion.PromoEndDateTravelWindow.Value))
                {
                    return false;
                }
            }

            // Rate
            if (promotion.RateStartDate.HasValue && promotion.RateEndDate.HasValue)
            {
                if (!IsOverlapping(searchStart, searchEnd, promotion.RateStartDate.Value, promotion.RateEndDate.Value))
                {
                    return false;
                }
            }

            return true;
        }


        private static bool IsOverlapping(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2)
        {
            return start1 <= end2 && end1 >= start2;
        }

    }
}
