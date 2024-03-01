using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Helpers.Room;
using APIServices.Conflux.Enum;
using APIServices.Conflux.Helpers.Restriction;
using APIServices.Conflux.Models.Restrictions;
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

        public static Transaction ToTransaction(int propertyId, Models.Restrictions.Room.RoomData roomData)
        {
            Transaction transaction = new Transaction()
            {
                TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
            };

            PropertyDataSet propertyDataSet = new PropertyDataSet();
            propertyDataSet.Property = propertyId;

            RoomData _roomData = new RoomData();

            _roomData.RoomID = roomData.RoomID;

            _roomData.Name = new Name()
            {
                Text = new Text() 
                {
                    Txt = roomData.RoomName,
                    Language = roomData.Language
                }
            };

            _roomData.Description = new Description()
            {
                Text = new Text()
                {
                    Txt = roomData.RoomDescription,
                    Language = roomData.Language
                }
            };

            _roomData.Capacity = roomData.Capacity;
            _roomData.AdultCapacity = roomData.AdultCapcity;

            _roomData.OccupancySettings = new OccupancySettings()
            {
                MinOccupancy = roomData.MinOccupancy,
                MingAge = roomData.MinAge
            };

            _roomData.PhotoUrl = new PhotoUrl()
            {
                Caption = new Caption()
                {
                    Text = new Text()
                    {
                        Txt = "Room Photo",
                        Language = roomData.Language
                    }
                },
                URL = roomData.PhotoUrl
            };

            _roomData.RoomFeatures = new RoomFeatures()
            {
                JapaneseHotelRoomsStyle = roomData.JapaneseHotelRoomStyle,

                Beds = new List<Bed>()
                { 
                    new Bed()
                    { 
                        Size = roomData.BedSize
                    }
                },

                RoomSharing = roomData.RoomSharing,
                Smoking = roomData.Smoking,

                BathAndToilet = new BathAndToilet()
                { 
                    Relation = roomData.BathAndToilet.Relation,
                    Bath = new Bath()
                    {
                        Bathtub = roomData.BathAndToilet.Bath.Bathtub,
                        Shower = roomData.BathAndToilet.Bath.Shower
                    },
                    Toliet = new Toilet()
                    { 
                        ElectronicBidet = roomData.BathAndToilet.Toliet.ElectronicBidet
                    }
                }
            };

            propertyDataSet.RoomData = _roomData;
            transaction.PropertyDataSet = propertyDataSet;

            return transaction;
        }

        public static AvailStatusMessages ToAvailStatusMessages(List<spGetCurrentRatesByHotel_Result4> currentRates)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };

            foreach (var currentRate in currentRates)
            {

                switch (currentRate.TypeRate)
                {
                    case (int)TypeRateEnum.RoomRate:

                        //RoomRateClosure(currentRate, ref availStatusMessages.AvailStatusMessageList);

                        break;
                    case (int)TypeRateEnum.RoomRatePromotion:
                        //RoomRatePromotionMessages(currentRate, ref rateAmountMessages, ref deleteRateAmountMessages);
                        break;

                }
            }



            return availStatusMessages;
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

        public static AvailStatusMessages ToAvailStatusMessages(List<vDayRates> vDayRates, string status)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };

            foreach(vDayRates vDayRate in vDayRates)
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

                availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(status);

                availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);
            }

            return availStatusMessages;

        }

        public static AvailStatusMessages ToAvailStatusMessages(List<vDayRatesExceptions> vDayRates, string status)
        {
            AvailStatusMessages availStatusMessages = new AvailStatusMessages()
            {
                HotelCode = HotelCode,
                AvailStatusMessageList = new List<AvailStatusMessage>()
            };

            foreach (vDayRatesExceptions vDayRate in vDayRates)
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

                availStatusMessage.RestrictionStatus = RestrictionHelper.GetRestrictionStatus(status);

                availStatusMessages.AvailStatusMessageList.Add(availStatusMessage);
            }

            return availStatusMessages;

        }

        #region RoomRateClosure
        #endregion

    }
}
