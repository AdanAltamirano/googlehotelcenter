using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{
    public class Utilities
    {
        public static bool IsInExceptionPrice(string exceptionMap, DateTime day)
        {
            // ajustar posición del mapa de excepciones
            // originalmente en la db de univisit el mapa se maneja  LMXJVSD 
            // y lo pasamos a DLMXJVS para poder trabajar con los indices del enum 'DayOfWeek'
            exceptionMap = exceptionMap[exceptionMap.Length - 1] + exceptionMap.Remove(exceptionMap.Length - 1);

                
            return exceptionMap[(int)day.DayOfWeek] == 'Y';
        }
    }

    public class Rate
    {
        public int rateId;
        public int roomId;
        public string ratePlanId;
        public decimal adultPrice;
        public decimal childPrice;
        public decimal teenPrice;
        public decimal extreAdultPrice;
        public decimal extraChildPrice;
        public decimal extraTeenPrice;
        public DateTime startDate;
        public DateTime endDate;
        public string exceptionDays;
        public string noArrival;
        public bool useDefaultRules;
        public string segment;
        public string rateCode;
    }
}
