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

            LengthOfStay lengthOfStayMin = null;
            LengthOfStay lengthOfStayMax = null;

            if (promotionRestrictions != null)
            {
                //if (promotionRestrictions.PromotionMinDays > 0 || promotionRestrictions.PromotionMaxDays > 0)
                //{
                    lengthOfStayMin = new LengthOfStay()
                    {
                        MinMaxMessageType = "SetMinLOS",
                        Time = promotionRestrictions.PromotionMinDays
                    };

                    lengthOfStayMax = new LengthOfStay()
                    {
                        MinMaxMessageType = "SetMaxLOS",
                        Time = promotionRestrictions.PromotionMaxDays
                    };
                //}

                if (rateRestricions.MinDays > 0 || rateRestricions.MaxDays > 0)
                {

                    if (lengthOfStayMin == null) lengthOfStayMin = new LengthOfStay();
                    if (lengthOfStayMax == null) lengthOfStayMax = new LengthOfStay();


                    lengthOfStayMin.MinMaxMessageType = "SetMinLOS";
                    lengthOfStayMin.Time = rateRestricions.MinDays;

                    lengthOfStayMax.MinMaxMessageType = "SetMaxLOS";
                    lengthOfStayMax.Time = rateRestricions.MaxDays;
                }
            }
            else
            {

                if (restrictionsHotel.HotelMinDays > 0 || restrictionsHotel.HotelMaxDays > 0)
                {

                    if (lengthOfStayMin == null) lengthOfStayMin = new LengthOfStay();
                    if (lengthOfStayMax == null) lengthOfStayMax = new LengthOfStay();

                    lengthOfStayMin.MinMaxMessageType = "SetMinLOS";
                    lengthOfStayMin.Time = restrictionsHotel.HotelMinDays;

                    lengthOfStayMax.MinMaxMessageType = "SetMaxLOS";
                    lengthOfStayMax.Time = restrictionsHotel.HotelMaxDays;
                }
                else
                {
                    if (rateplanRestrictions.MinDays > 0 || rateplanRestrictions.MaxDays > 0)
                    {
                        if (lengthOfStayMin == null) lengthOfStayMin = new LengthOfStay();
                        if (lengthOfStayMax == null) lengthOfStayMax = new LengthOfStay();

                        lengthOfStayMin.MinMaxMessageType = "SetMinLOS";
                        lengthOfStayMin.Time = rateplanRestrictions.MinDays;

                        lengthOfStayMax.MinMaxMessageType = "SetMaxLOS";
                        lengthOfStayMax.Time = rateplanRestrictions.MaxDays;
                    }

                    if (rateRestricions.MinDays > 0 || rateRestricions.MaxDays > 0)
                    {

                        if (lengthOfStayMin == null) lengthOfStayMin = new LengthOfStay();
                        if (lengthOfStayMax == null) lengthOfStayMax = new LengthOfStay();


                        lengthOfStayMin.MinMaxMessageType = "SetMinLOS";
                        lengthOfStayMin.Time = rateRestricions.MinDays;

                        lengthOfStayMax.MinMaxMessageType = "SetMaxLOS";
                        lengthOfStayMax.Time = rateRestricions.MaxDays;
                    }

                }

            }

            if(lengthOfStayMin != null)lengthOfStays.Add(lengthOfStayMin);
            if(lengthOfStayMax != null)lengthOfStays.Add(lengthOfStayMax);

            return lengthOfStays;
        }

        public static AdvanceBookingRestriction CreateAdvanceBookingRestriction(AllRatePlanRestrictionsDto rateplanRestrictions, RateRestrictionsDto rateRestricions, PromotionDto promotionRestrictions)
        {
            AdvanceBookingRestriction advanceBookingRestriction = null;

            if (promotionRestrictions != null)
            {
                //if (promotionRestrictions.PromotionMinAdvDays > 0 || promotionRestrictions.PromotionMaxAdvDays > 0)
                //{
                    if (advanceBookingRestriction == null) advanceBookingRestriction = new AdvanceBookingRestriction();

                    advanceBookingRestriction.MinAdvancedBookingOffset = promotionRestrictions.PromotionMinAdvDays.ToString();
                    advanceBookingRestriction.MaxAdvancedBookingOffset = promotionRestrictions.PromotionMaxAdvDays.ToString();
                //}

                if (rateRestricions.MinAdvDays > 0 || rateRestricions.MaxAdvDays > 0)
                {
                    if (advanceBookingRestriction == null) advanceBookingRestriction = new AdvanceBookingRestriction();

                    advanceBookingRestriction.MinAdvancedBookingOffset = rateRestricions.MinAdvDays.ToString();
                    advanceBookingRestriction.MaxAdvancedBookingOffset = rateRestricions.MaxAdvDays.ToString();
                }

            }
            else
            {
                if (rateplanRestrictions.MinAdvDays > 0 || rateplanRestrictions.MaxAdvDays > 0)
                {
                    if (advanceBookingRestriction == null) advanceBookingRestriction = new AdvanceBookingRestriction();

                    advanceBookingRestriction.MinAdvancedBookingOffset = rateplanRestrictions.MinAdvDays.ToString();
                    advanceBookingRestriction.MaxAdvancedBookingOffset = rateplanRestrictions.MaxAdvDays.ToString();
                }

                if (rateRestricions.MinAdvDays > 0 || rateRestricions.MaxAdvDays > 0)
                {
                    if (advanceBookingRestriction == null) advanceBookingRestriction = new AdvanceBookingRestriction();

                    advanceBookingRestriction.MinAdvancedBookingOffset = rateRestricions.MinAdvDays.ToString();
                    advanceBookingRestriction.MaxAdvancedBookingOffset = rateRestricions.MaxAdvDays.ToString();
                }
            }


            return advanceBookingRestriction;
        }
    }
}
