using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesAndAvailability
{
    public static class Fares
    {
        public static IEnumerable<DayRates> GetDayRates(int idHotel, DateTime start, DateTime end, int lan)
        {
            IEnumerable<DayRates> rates;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                rates = db.DayRates.Where(p => p.HotelId == idHotel && p.Language == lan && (p.StartDate >= start || p.EndDate >= start) && p.StartDate <= end);
            }

            return rates;
        }        
    }
}
