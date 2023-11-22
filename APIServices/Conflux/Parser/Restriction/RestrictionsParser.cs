using System;
using System.Data;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Helpers.Restriction;
using APIServices.Conflux.OTA.Models.Restrictions;

namespace APIServices.Conflux.Parser.Restriction
{
    public static class RestrictionsParser
    {
        private static int HotelCode { get; set; }

        public static void Init(int hotelCode)
        {
            HotelCode = hotelCode;
        }

        public static AvailStatusMessages ToAvailStatusMessages(List<spGetLockRoomTypesByHotel_Result> lockRoomTypesList)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };

            foreach(var lockRoomType in lockRoomTypesList)
            {
                AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                {
                    Start = (DateTime) lockRoomType.StartDate,
                    End = (DateTime) lockRoomType.EndDate,
                    InvTypeCode = lockRoomType.RoomCode,
                    RatePlanCode = lockRoomType.RatePlanId
                };
                //TODO Checar las combinaciones del No Llegadas con los Status C, N, O
                availStatusMessage.RestrictionStatus =  RestrictionHelper.GetRestrictionStatus(lockRoomType.Status);

                availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);

            }

            return availStatusMessages;

        }

        public static AvailStatusMessages ToAvailStatusMessages(List<DataRow> activeRooms, List<spGetLockRatePlansByHotel_Result> lockRatePlans)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };

            foreach(DataRow activeRoom in activeRooms)
            {
                foreach(var lockRatePlan in lockRatePlans)
                {
                    AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                    availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                    {
                        Start = (DateTime)lockRatePlan.StartDate,
                        End = (DateTime)lockRatePlan.EndDate,
                        InvTypeCode = activeRoom.ItemArray[22].ToString() ?? "",
                        RatePlanCode = lockRatePlan.RatePlanId
                    };

                    availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockRatePlan.Status);

                    if (lockRatePlan.MinDays > 0)
                    {
                        LengthOfStay lengthOfStay = new LengthOfStay()
                        {
                            MinMaxMessageType = "SetMinLOS",
                            Time = (int?) lockRatePlan.MinDays
                        };

                        availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                    }

                    if (lockRatePlan.MaxDays > 0)
                    {
                        LengthOfStay lengthOfStay = new LengthOfStay()
                        {
                            MinMaxMessageType = "SetMaxLOS",
                            Time = (int?)lockRatePlan.MaxDays
                        };

                        availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                    }

                    availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);

                }

            }


            return availStatusMessages;

        }

        public static AvailStatusMessages ToAvailStatusMessages(List<spGetLockGralByHotel_Result> locksGral ,List<DataRow> rooms, List<DataRow> ratePlans)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };

            foreach(var lockGral in locksGral){

                foreach(var room in rooms)
                {
                    foreach(var rateplan in ratePlans)
                    {
                        AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                        availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                        {
                            Start = (DateTime)lockGral.StartDate,
                            End = (DateTime)lockGral.EndDate,
                            InvTypeCode = room.ItemArray[22].ToString() ?? "",
                            RatePlanCode = rateplan.ItemArray[0].ToString()
                        };

                        availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockGral.Status);

                        if (lockGral.MinDays > 0)
                        {
                            LengthOfStay lengthOfStay = new LengthOfStay()
                            {
                                MinMaxMessageType = "SetMinLOS",
                                Time = (int?)lockGral.MinDays
                            };

                            availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                        }

                        if (lockGral.MaxDays > 0)
                        {
                            LengthOfStay lengthOfStay = new LengthOfStay()
                            {
                                MinMaxMessageType = "SetMaxLOS",
                                Time = (int?)lockGral.MaxDays
                            };

                            availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                        }

                        availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);

                    }
                }

            }

            return availStatusMessages;
        }

    }
}
