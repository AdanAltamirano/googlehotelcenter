using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIServices.Models;
using APIServices.Helpers.Room;
using APIServices.Conflux.OTA.Models.Restrictions;
using APIServices.Conflux.Models.Restrictions.Rate;

namespace APIServices.Conflux.Helpers.Restriction
{
    public static partial class RestrictionHelper
    {
        public static AvailStatusMessage CreateAvailStatusMessage(vDayRates vDayRate, string status)
        {

            var starDate = ((DateTime)vDayRate.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)vDayRate.StartDate).Date;
            var diff = ((DateTime)vDayRate.EndDate).Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)vDayRate.EndDate).Date;

            var room = RoomHelper.GetRoom(vDayRate.RoomId);

            AvailStatusMessage availStatusMessage = new AvailStatusMessage();
            availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
            {
                Start = starDate,
                End = endDate,
                InvTypeCode = room.Code ?? "",
                RatePlanCode = vDayRate.RatePlanId,
                ApplyMon = vDayRate.NoArrivalsMap[0] == 'Y' ? true : false,
                ApplyTue = vDayRate.NoArrivalsMap[1] == 'Y' ? true : false,
                ApplyWed = vDayRate.NoArrivalsMap[2] == 'Y' ? true : false,
                ApplyThu = vDayRate.NoArrivalsMap[3] == 'Y' ? true : false,
                ApplyFri = vDayRate.NoArrivalsMap[4] == 'Y' ? true : false,
                ApplySat = vDayRate.NoArrivalsMap[5] == 'Y' ? true : false,
                ApplySun = vDayRate.NoArrivalsMap[6] == 'Y' ? true : false
            };

            availStatusMessage.RestrictionStatus = GetRestrictionStatus(status);

            return availStatusMessage;

        }

        public static AvailStatusMessage CreateAvailStatusMessage(vDayRatesExceptions vDayRate, string status)
        {

            var starDate = ((DateTime)vDayRate.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)vDayRate.StartDate).Date;
            var diff = ((DateTime)vDayRate.EndDate).Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)vDayRate.EndDate).Date;

            var room = RoomHelper.GetRoom(vDayRate.RoomId);

            AvailStatusMessage availStatusMessage = new AvailStatusMessage();
            availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
            {
                Start = starDate,
                End = endDate,
                InvTypeCode = room.Code ?? "",
                RatePlanCode = vDayRate.RatePlanId,
                ApplyMon = vDayRate.NoArrivalsMap[0] == 'Y' ? true : false,
                ApplyTue = vDayRate.NoArrivalsMap[1] == 'Y' ? true : false,
                ApplyWed = vDayRate.NoArrivalsMap[2] == 'Y' ? true : false,
                ApplyThu = vDayRate.NoArrivalsMap[3] == 'Y' ? true : false,
                ApplyFri = vDayRate.NoArrivalsMap[4] == 'Y' ? true : false,
                ApplySat = vDayRate.NoArrivalsMap[5] == 'Y' ? true : false,
                ApplySun = vDayRate.NoArrivalsMap[6] == 'Y' ? true : false
            };

            availStatusMessage.RestrictionStatus = GetRestrictionStatus(status);

            return availStatusMessage;

        }

        public static List<LengthOfStay> CreateLenghtStay(HotelRestrictionsDto restrictionsHotel, AllRatePlanRestrictionsDto rateplanRestrictions, RateRestrictionsDto rateRestricions, PromotionDto promotionRestrictions)
        {
            List<LengthOfStay> lengthOfStays = new List<LengthOfStay>();

            //Si hay de hotel se mandan esas
            if (restrictionsHotel.HotelMinDays > 0 || restrictionsHotel.HotelMaxDays > 0)
            {
                LengthOfStay lengthOfStayMin = new LengthOfStay()
                {
                    MinMaxMessageType = "SetMinLOS",
                    Time = restrictionsHotel.HotelMinDays
                };

                LengthOfStay lengthOfStayMax = new LengthOfStay()
                {
                    MinMaxMessageType = "SetMaxLOS",
                    Time = restrictionsHotel.HotelMaxDays
                };

                lengthOfStays.Add(lengthOfStayMin);
                lengthOfStays.Add(lengthOfStayMax);
            }
            else
            {
                if(promotionRestrictions != null)
                {
                    if(promotionRestrictions.PromotionMinDays > 0 || promotionRestrictions.PromotionMaxDays > 0)
                    {
                        LengthOfStay lengthOfStayMin = new LengthOfStay()
                        {
                            MinMaxMessageType = "SetMinLOS",
                            Time = promotionRestrictions.PromotionMinDays
                        };

                        LengthOfStay lengthOfStayMax = new LengthOfStay()
                        {
                            MinMaxMessageType = "SetMaxLOS",
                            Time = promotionRestrictions.PromotionMaxDays
                        };

                        lengthOfStays.Add(lengthOfStayMin);
                        lengthOfStays.Add(lengthOfStayMax);
                    }
                }
                else if(rateplanRestrictions.MinDays > 0 || rateplanRestrictions.MaxDays > 0)
                {
                    LengthOfStay lengthOfStayMin = new LengthOfStay()
                    {
                        MinMaxMessageType = "SetMinLOS",
                        Time = rateplanRestrictions.MinDays
                    };

                    LengthOfStay lengthOfStayMax = new LengthOfStay()
                    {
                        MinMaxMessageType = "SetMaxLOS",
                        Time = rateplanRestrictions.MaxDays
                    };

                    lengthOfStays.Add(lengthOfStayMin);
                    lengthOfStays.Add(lengthOfStayMax);
                }
                else if(rateRestricions.MinDays > 0 || rateRestricions.MaxDays > 0)
                {
                    LengthOfStay lengthOfStayMin = new LengthOfStay()
                    {
                        MinMaxMessageType = "SetMinLOS",
                        Time = rateRestricions.MinDays
                    };

                    LengthOfStay lengthOfStayMax = new LengthOfStay()
                    {
                        MinMaxMessageType = "SetMaxLOS",
                        Time = rateRestricions.MaxDays
                    };

                    lengthOfStays.Add(lengthOfStayMin);
                    lengthOfStays.Add(lengthOfStayMax);
                }
            }

            return lengthOfStays;
        }

    }
}
