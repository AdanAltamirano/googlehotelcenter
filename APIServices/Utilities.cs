using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIServices.Models.DTO;

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

        public static DaysOfWeekType GetDaysOfWeek(string map)
        {
            return new DaysOfWeekType()
            {
                Sun = map[0] == 'Y' ? true : false,
                Mon = map[1] == 'Y' ? true : false,
                Tue = map[2] == 'Y' ? true : false,
                Thur = map[3] == 'Y' ? true : false,
                Weds = map[4] == 'Y' ? true : false,
                Fri = map[5] == 'Y' ? true : false,
                Sat = map[6] == 'Y' ? true : false                
            };
        }
    }
}
