using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.OTA.Models.Restrictions;

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

    }
}
