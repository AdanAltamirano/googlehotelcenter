using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{
    public class Utilities
    {
        public static bool IsInUVMap(string map, DateTime day)
        {
            // ajustar posición del mapa de excepciones
            // originalmente en la db de univisit el mapa se maneja  LMXJVSD 
            // y lo pasamos a DLMXJVS para poder trabajar con los indices del enum 'DayOfWeek'
            map = map[map.Length - 1] + map.Remove(map.Length - 1);

                
            return map[(int)day.DayOfWeek] == 'Y';
        }
    }
}
