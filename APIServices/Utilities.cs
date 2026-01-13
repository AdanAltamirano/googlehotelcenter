using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using APIServices.Models.DTO;
using APIServices.Models;
using System.Reflection;
using System.Collections;
using System.IO;
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
                Sun = map[6] == 'Y' ? true : false,
                Mon = map[0] == 'Y' ? true : false,
                Tue = map[1] == 'Y' ? true : false,
                Weds = map[2] == 'Y' ? true : false,
                Thur = map[3] == 'Y' ? true : false,
                Fri = map[4] == 'Y' ? true : false,
                Sat = map[5] == 'Y' ? true : false                
            };
        }

        public static string GetDaysOfWeekString(DaysOfWeekType days)
        {
            string strDays = "";
            if (days.Mon)
                strDays += "Y";
            else
                strDays += "N";

            if (days.Tue)
                strDays += "Y";
            else
                strDays += "N";

            if (days.Weds)
                strDays += "Y";
            else
                strDays += "N";

            if (days.Thur)
                strDays += "Y";
            else
                strDays += "N";

            if (days.Fri)
                strDays += "Y";
            else
                strDays += "N";

            if (days.Sat)
                strDays += "Y";
            else
                strDays += "N";

            if (days.Sun)
                strDays += "Y";
            else
                strDays += "N";

            return strDays;
        }

        /// <summary>
        /// Get XML from data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns>string with xml format</returns>
        public static string GetXML<T>(T data)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(data.GetType());
                xmlTextWriter = new XmlTextWriter(stringWriter);
                serializer.Serialize(xmlTextWriter, data);
            }
            catch
            {

            }
            finally
            {
                stringWriter.Close();
                if (xmlTextWriter != null)
                    xmlTextWriter.Close();
            }

            return stringWriter.ToString();

        }

        public static string Truncate(string texto, int max)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            return texto.Length <= max
                ? texto
                : texto.Substring(0, max);
        }

    }
}
