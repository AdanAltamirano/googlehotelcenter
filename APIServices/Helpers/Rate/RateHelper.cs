using System;
using System.Linq;
using System.Data.Entity;
using APIServices.Models;

namespace APIServices.Helpers.Rate
{
    public static class RateHelper
    {
        public static bool RoomHasRates(int hotelId, int roomId)
        {
            bool hasRates = false;

            using (var context = new OzHotelesEntities())
            {
                hasRates = context.vDayRates.Any(vdr => vdr.HotelId == hotelId && vdr.RoomId == roomId && vdr.EndDate >= DbFunctions.TruncateTime(DateTime.Now));
            }

            return hasRates;
        }

        public static bool RatePlanHasRates(int hotelId, string ratePlanId)
        {
            bool hasRates = false;

            using (var context = new OzHotelesEntities())
            {
                hasRates = context.vDayRates.Any(vdr => vdr.HotelId == hotelId && vdr.RatePlanId == ratePlanId && vdr.EndDate >= DbFunctions.TruncateTime(DateTime.Now));

                if (hasRates == false)
                {
                    hasRates = context.vDayRates.Any(vdr => vdr.HotelId == hotelId && vdr.ParentRatePlanId == ratePlanId && vdr.EndDate >= DbFunctions.TruncateTime(DateTime.Now));
                }
            }

            return hasRates;
        }

    }
}
