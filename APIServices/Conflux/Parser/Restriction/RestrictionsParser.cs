using System;
using System.Data;
using System.Linq;
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
                var starDate = ((DateTime)lockRoomType.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockRoomType.StartDate).Date;
                var diff = ((DateTime)lockRoomType.EndDate).Date - starDate.Date;
                var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockRoomType.EndDate).Date;

                AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                {
                    Start = starDate.Date,
                    End = endDate.Date,
                    InvTypeCode = lockRoomType.RoomCode,
                    RatePlanCode = lockRoomType.RatePlanId
                };
                //TODO Checar las combinaciones del No Llegadas con los Status C, N, O
                availStatusMessage.RestrictionStatus =  RestrictionHelper.GetRestrictionStatus(lockRoomType.Status);

                availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);

            }

            return availStatusMessages;

        }

        public static AvailStatusMessages ToAvailStatusMessages(DataRow activeRoom, List<spGetLockRatePlansByHotel_Result> lockRatePlans)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };


            foreach (var lockRatePlan in lockRatePlans)
            {

                var starDate = ((DateTime)lockRatePlan.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockRatePlan.StartDate).Date;
                var diff = ((DateTime)lockRatePlan.EndDate).Date - starDate.Date;
                var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockRatePlan.EndDate).Date;

                AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                {
                    Start = starDate.Date,
                    End = endDate.Date,
                    InvTypeCode = activeRoom.ItemArray[22].ToString() ?? "",
                    RatePlanCode = lockRatePlan.RatePlanId
                };

                availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockRatePlan.Status);

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
                    var starDate = ((DateTime)lockRatePlan.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockRatePlan.StartDate).Date;
                    var diff = ((DateTime)lockRatePlan.EndDate).Date - starDate.Date;
                    var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockRatePlan.EndDate).Date;

                    AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                    availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                    {
                        Start = starDate.Date,
                        End = endDate.Date,
                        InvTypeCode = activeRoom.ItemArray[22].ToString() ?? "",
                        RatePlanCode = lockRatePlan.RatePlanId
                    };

                    availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockRatePlan.Status);

                    //if (lockRatePlan.MinDays > 0)
                    //{
                    //    LengthOfStay lengthOfStay = new LengthOfStay()
                    //    {
                    //        MinMaxMessageType = "SetMinLOS",
                    //        Time = (int?) lockRatePlan.MinDays
                    //    };

                    //    availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                    //}

                    //if (lockRatePlan.MaxDays > 0)
                    //{
                    //    LengthOfStay lengthOfStay = new LengthOfStay()
                    //    {
                    //        MinMaxMessageType = "SetMaxLOS",
                    //        Time = (int?)lockRatePlan.MaxDays
                    //    };

                    //    availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                    //}

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
                        var starDate = ((DateTime)lockGral.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockGral.StartDate).Date;
                        var diff = ((DateTime)lockGral.EndDate).Date - starDate.Date;
                        var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockGral.EndDate).Date;

                        AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                        availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                        {
                            Start = starDate.Date,
                            End = endDate.Date,
                            InvTypeCode = room.ItemArray[22].ToString() ?? "",
                            RatePlanCode = rateplan.ItemArray[0].ToString()
                        };

                        availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockGral.Status);

                        //if (lockGral.MinDays > 0)
                        //{
                        //    LengthOfStay lengthOfStay = new LengthOfStay()
                        //    {
                        //        MinMaxMessageType = "SetMinLOS",
                        //        Time = (int?)lockGral.MinDays
                        //    };

                        //    availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                        //}

                        //if (lockGral.MaxDays > 0)
                        //{
                        //    LengthOfStay lengthOfStay = new LengthOfStay()
                        //    {
                        //        MinMaxMessageType = "SetMaxLOS",
                        //        Time = (int?)lockGral.MaxDays
                        //    };

                        //    availStatusMessage.LengthsOfStay.Add(lengthOfStay);
                        //}

                        availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);

                    }
                }

            }

            return availStatusMessages;
        }

        //General Promociones
        public static AvailStatusMessages ToAvailStatusMessages(int hotelId,List<spGetLockGralByHotel_Result> locksGral, List<DataRow> rooms, List<DataRow> ratePlans, List<DataRow> promos)
        {

            OzHotelesEntities ozHotelesEntities = new OzHotelesEntities();
            Dictionary<string, List<string>> promosRatePlanDictionary = new Dictionary<string, List<string>>();

            foreach (var promo in promos)
            {
                string promoId = promo.ItemArray[0].ToString();

                foreach (var ratePlan in ratePlans)
                {
                    string ratePlanId = ratePlan.ItemArray[0].ToString();

                    var exist = ozHotelesEntities.Promociones_RatePlan.Any(prr => prr.IdPromocion == promoId && prr.IdRatePlan == ratePlanId  && prr.IdHotel == hotelId);

                    if (exist)
                    {

                        if (!promosRatePlanDictionary.ContainsKey(ratePlanId))
                        {
                            List<string> promoListTemp = new List<string>();
                            promoListTemp.Add(promoId);

                            promosRatePlanDictionary.Add(ratePlanId,promoListTemp);
                        }
                        else
                        {
                            promosRatePlanDictionary[ratePlanId].Add(promoId);
                        }

                    }

                }
            }

            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };


            foreach (var lockGral in locksGral)
            {

                foreach (var room in rooms)
                {
                    foreach (var rateplan in ratePlans)
                    {
                        string ratePlanId = rateplan.ItemArray[0].ToString();

                        if (promosRatePlanDictionary.ContainsKey(ratePlanId))
                        {

                            var starDate = ((DateTime)lockGral.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockGral.StartDate).Date;
                            var diff = ((DateTime)lockGral.EndDate).Date - starDate.Date;
                            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockGral.EndDate).Date;

                            List<string> promoIdListTemp = promosRatePlanDictionary[ratePlanId];

                            foreach (var promoId in promoIdListTemp)
                            {
                                AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                                availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                                {
                                    Start = starDate.Date,
                                    End = endDate.Date,
                                    InvTypeCode = room.ItemArray[22].ToString() ?? "",
                                    RatePlanCode = promoId + ratePlanId
                                };

                                availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockGral.Status);

                                availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);
                            }
                        }

                    }
                }

            }

            return availStatusMessages;

        }

        //LockRoomType Promociones
        public static AvailStatusMessages ToAvailStatusMessages(int hotelId,List<spGetLockRoomTypesByHotel_Result> locksRoomType,List<string> ratePlans, List<DataRow> promos)         
        {
            OzHotelesEntities ozHotelesEntities = new OzHotelesEntities();
            Dictionary<string, List<string>> promosRatePlanDictionary = new Dictionary<string, List<string>>();

            foreach (var promo in promos)
            {
                string promoId = promo.ItemArray[0].ToString();

                foreach (var ratePlan in ratePlans)
                {
                    string ratePlanId = ratePlan;

                    var exist = ozHotelesEntities.Promociones_RatePlan.Any(prr => prr.IdPromocion == promoId && prr.IdRatePlan == ratePlanId && prr.IdHotel == hotelId);

                    if (exist)
                    {

                        if (!promosRatePlanDictionary.ContainsKey(ratePlanId))
                        {
                            List<string> promoListTemp = new List<string>();
                            promoListTemp.Add(promoId);

                            promosRatePlanDictionary.Add(ratePlanId, promoListTemp);
                        }
                        else
                        {
                            promosRatePlanDictionary[ratePlanId].Add(promoId);
                        }

                    }

                }
            }

            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };


            foreach (var lockRoomtype in locksRoomType)
            {

                foreach (var rateplan in ratePlans)
                {
                    string ratePlanId = rateplan;

                    if (promosRatePlanDictionary.ContainsKey(ratePlanId))
                    {

                        var starDate = ((DateTime)lockRoomtype.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockRoomtype.StartDate).Date;
                        var diff = ((DateTime)lockRoomtype.EndDate).Date - starDate.Date;
                        var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockRoomtype.EndDate).Date;

                        List<string> promoIdListTemp = promosRatePlanDictionary[ratePlanId];

                        foreach (var promoId in promoIdListTemp)
                        {
                            AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                            availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                            {
                                Start = starDate.Date,
                                End = endDate.Date,
                                InvTypeCode = lockRoomtype.RoomCode ?? "",
                                RatePlanCode = promoId + ratePlanId
                            };

                            availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockRoomtype.Status);

                            availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);
                        }
                    }

                }
            }

            return availStatusMessages;


        }

        //LockRatePlan Promociones
        public static AvailStatusMessages ToAvailStatusMessages(int hotelId, List<spGetLockRatePlansByHotel_Result> locksRatePlan, List<DataRow> rooms, List<string> ratePlans, List<DataRow> promos)
        {
            OzHotelesEntities ozHotelesEntities = new OzHotelesEntities();
            Dictionary<string, List<string>> promosRatePlanDictionary = new Dictionary<string, List<string>>();

            foreach (var promo in promos)
            {
                string promoId = promo.ItemArray[0].ToString();

                foreach (var ratePlan in ratePlans)
                {
                    string ratePlanId = ratePlan;

                    var exist = ozHotelesEntities.Promociones_RatePlan.Any(prr => prr.IdPromocion == promoId && prr.IdRatePlan == ratePlanId && prr.IdHotel == hotelId);

                    if (exist)
                    {

                        if (!promosRatePlanDictionary.ContainsKey(ratePlanId))
                        {
                            List<string> promoListTemp = new List<string>();
                            promoListTemp.Add(promoId);

                            promosRatePlanDictionary.Add(ratePlanId, promoListTemp);
                        }
                        else
                        {
                            promosRatePlanDictionary[ratePlanId].Add(promoId);
                        }

                    }
                }
            }


            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };


            foreach (var lockRateplan in locksRatePlan)
            {

                foreach (var room in rooms)
                {
                    foreach (var rateplan in ratePlans)
                    {
                        string ratePlanId = rateplan;

                        if (promosRatePlanDictionary.ContainsKey(ratePlanId))
                        {

                            var starDate = ((DateTime)lockRateplan.StartDate).Date < DateTime.Now.Date ? DateTime.Now.Date : ((DateTime)lockRateplan.StartDate).Date;
                            var diff = ((DateTime)lockRateplan.EndDate).Date - starDate.Date;
                            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : ((DateTime)lockRateplan.EndDate).Date;

                            List<string> promoIdListTemp = promosRatePlanDictionary[ratePlanId];

                            foreach (var promoId in promoIdListTemp)
                            {
                                AvailStatusMessage availStatusMessage = new AvailStatusMessage();
                                availStatusMessage.StatusApplicationControl = new StatusApplicationControl()
                                {
                                    Start = starDate.Date,
                                    End = endDate.Date,
                                    InvTypeCode = room.ItemArray[22].ToString() ?? "",
                                    RatePlanCode = promoId + ratePlanId
                                };

                                availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(lockRateplan.Status);

                                availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);
                            }
                        }

                    }
                }

            }

            return availStatusMessages;

        }
    }
}
