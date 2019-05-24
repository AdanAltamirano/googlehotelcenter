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
}
