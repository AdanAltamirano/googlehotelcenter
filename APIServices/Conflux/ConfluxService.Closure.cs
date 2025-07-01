using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using APIServices.Conflux.Parser.Restriction;
using APIServices.Conflux.Models.Closure;
using APIServices.Conflux.Helpers.Rooms;
using APIServices.Conflux.Helpers.RatesPlan;
using APIServices.Xml.Soap;
using APIServices.Xml.OTA.Request.Restrictions;


namespace APIServices.Conflux
{
    public partial class ConfluxService
    {
       
        public List<XDocument> GetGeneralClosure(int hotelId,Closure closure,out int lockTotal)
        {
            List<XDocument> lockGralPrioritySoapRQ = new List<XDocument>();

            string filterRooms = string.Empty;
            string filterRatesPlans = string.Empty;
            string filterRatesPlansPromos = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"; // Ver si va quedar el mismo filtro

            if ((closure.RoomsList.Length == 1 && closure.RoomsList[0] != 0) || closure.RoomsList.Length >= 2)
            {
                string column = "idTipoHabitacion_Hotel";
                filterRooms = $" AND {column} IN ({string.Join(", ", closure.RoomsList.Select(id => id.ToString()))})";
            }

            if ((closure.RatePlansList.Length == 1 && closure.RatePlansList[0] != "0") || closure.RatePlansList.Length >= 2)
            {
                string column = "IdRatePlan";
                filterRatesPlans = $"{column} IN ({string.Join(", ", closure.RatePlansList.Select(id => $"'{id}'"))})";
            }


            var roomsList = RoomsHelper.GetRoomsByHotel(hotelId,filterRooms);
            var ratesplansList = RatesPlanHelper.GetRatePlansByHotel(hotelId,filterRatesPlans);
            var ratesplansListPromos = RatesPlanHelper.GetRatePlansPromosByHotel(hotelId, filterRatesPlansPromos);

            
            var lockGral = dbContext.spGetLockGralByHotel(hotelId, closure.StartDate.Value.Date, closure.EndDate.Value.Date).ToList();
            var availStatusMessagesLockGralNoPromos = RestrictionsParser.ToAvailStatusMessages(lockGral, roomsList, ratesplansList);
            List<XElement> lockGralNoPromosHotelAvailNotifRQList = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockGralNoPromos);

            foreach (XElement lockGralNoPromosHotelAvailNotifRQ in lockGralNoPromosHotelAvailNotifRQList)
            {
                var lockGralNoPromosSoapRQ = Soap.CreateSoapRequestXml(lockGralNoPromosHotelAvailNotifRQ);
                lockGralPrioritySoapRQ.Add(lockGralNoPromosSoapRQ);
            }

            //Promos
            if (ratesplansListPromos.Count > 0)
            {
                var availStatusMessagesLockGralPromos = RestrictionsParser.ToAvailStatusMessages(hotelId, lockGral, roomsList, ratesplansList, ratesplansListPromos);
                List<XElement> lockGralPromosHotelAvailNotifRQList = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockGralPromos);
                foreach (XElement lockGralPromosHotelAvailNotifRQ in lockGralPromosHotelAvailNotifRQList)
                {
                    var lockGralPromosSoapRQ = Soap.CreateSoapRequestXml(lockGralPromosHotelAvailNotifRQ);
                    lockGralPrioritySoapRQ.Add(lockGralPromosSoapRQ);
                }
            }

            lockTotal = lockGral.Count;

            return lockGralPrioritySoapRQ;

        }


        public List<XDocument> GetRatePlanClosure(int hotelId, Closure closure, out int lockTotal)
        {
            List<XDocument> lockRatePlanPrioritySoapRQ = new List<XDocument>();

            string filterRooms = string.Empty;
            string filterRatesPlansPromos = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"; // Ver si va quedar el mismo filtro

            if ((closure.RoomsList.Length == 1 && closure.RoomsList[0] != 0) || closure.RoomsList.Length >= 2)
            {
                string column = "idTipoHabitacion_Hotel";
                filterRooms = $" AND {column} IN ({string.Join(", ", closure.RoomsList.Select(id => id.ToString()))})";
            }

            List<APIServices.Models.spGetLockRatePlansByHotel_Result> lockRatePlans = new List<APIServices.Models.spGetLockRatePlansByHotel_Result>();

            if ((closure.RatePlansList.Length == 1 && closure.RatePlansList[0] != "0") || closure.RatePlansList.Length >= 2)
            {
                foreach (var ratePlanId in closure.RatePlansList)
                {
                    var lockRatePlansTemp = dbContext.spGetLockRatePlansByHotel(hotelId, ratePlanId, closure.StartDate.Value.Date, closure.EndDate.Value.Date).ToList();
                    lockRatePlans.AddRange(lockRatePlansTemp);
                }
            }
            else if((closure.RatePlansList.Length == 1 && closure.RatePlansList[0] == "0"))
            {
                lockRatePlans = dbContext.spGetLockRatePlansByHotel(hotelId, null, closure.StartDate.Value.Date, closure.EndDate.Value.Date).ToList();
            }

            var roomsList = RoomsHelper.GetRoomsByHotel(hotelId, filterRooms);
            var ratesplansListPromos = RatesPlanHelper.GetRatePlansPromosByHotel(hotelId, filterRatesPlansPromos);

            var availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(roomsList, lockRatePlans);
            List<XElement> lockRatePlanHotelAvailNotifRQList = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockRatePlans);

            foreach (XElement lockRatePlanHotelAvailNotifRQ in lockRatePlanHotelAvailNotifRQList)
            {
                //Request LockRatePlan
                var lockRatePlanSoapRQ = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ);
                lockRatePlanPrioritySoapRQ.Add(lockRatePlanSoapRQ);
            }

            //Promos
            if (ratesplansListPromos.Count > 0)
            {
                var activeRatePlansLockRatePlan = lockRatePlans.Select(lrt => lrt.RatePlanId).Distinct().ToList();
                var availStatusMessagesLockRatePlanPromos = RestrictionsParser.ToAvailStatusMessages(hotelId, lockRatePlans, roomsList, activeRatePlansLockRatePlan, ratesplansListPromos);
                List<XElement> lockRatePlanPromosHotelAvailNotifRQList = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockRatePlanPromos);

                foreach (XElement lockRatePlanPromosHotelAvailNotifRQ in lockRatePlanPromosHotelAvailNotifRQList)
                {
                    var lockRatePlanPromosSoapRQ = Soap.CreateSoapRequestXml(lockRatePlanPromosHotelAvailNotifRQ);
                    lockRatePlanPrioritySoapRQ.Add(lockRatePlanPromosSoapRQ);
                }
            }

            lockTotal = lockRatePlans.Count;

            return lockRatePlanPrioritySoapRQ;
        }

        public List<XDocument> GetRoomTypeClosure(int hotelId, Closure closure, out int lockTotal) 
        {
            List<XDocument> lockRoomTypesPrioritySoapRQ = new List<XDocument>();

            string filterRatesPlansPromos = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"; // Ver si va quedar el mismo filtro

            List<APIServices.Models.spGetLockRoomTypesByHotel_Result> lockRoomTypes = new List<APIServices.Models.spGetLockRoomTypesByHotel_Result>();

            if ((closure.RatePlansList.Length == 1 && closure.RatePlansList[0] == "0") &&
                (closure.RoomsList.Length == 1 && closure.RoomsList[0] == 0))
            {
                // Todos los planes con todas las habitaciones

                lockRoomTypes = dbContext.spGetLockRoomTypesByHotel(hotelId, closure.StartDate.Value.Date, closure.EndDate.Value.Date, null, null).ToList();

            }
            else if ((closure.RatePlansList.Length == 1 && closure.RatePlansList[0] == "0") &&
                        ((closure.RoomsList.Length == 1 && closure.RoomsList[0] != 0) || closure.RoomsList.Length > 1))
            {
                // Todos los planes con habitaciones seleccionadas

                foreach (var roomId in closure.RoomsList)
                {
                    var lockRoomTypesTemp = dbContext.spGetLockRoomTypesByHotel(hotelId, closure.StartDate.Value.Date, closure.EndDate.Value.Date, null, roomId).ToList();
                    lockRoomTypes.AddRange(lockRoomTypesTemp);
                }


            }
            else if ((closure.RoomsList.Length == 1 && closure.RoomsList[0] == 0) &&
                        ((closure.RatePlansList.Length == 1 && closure.RatePlansList[0] != "0") || closure.RatePlansList.Length > 1))
            {
                // Todas las habitaciones con planes seleccionados

                foreach (var rateplanId in closure.RatePlansList)
                {
                    var lockRoomTypesTemp = dbContext.spGetLockRoomTypesByHotel(hotelId, closure.StartDate.Value.Date, closure.EndDate.Value.Date, rateplanId, null).ToList();
                    lockRoomTypes.AddRange(lockRoomTypesTemp);
                }

            }
            else
            {
                // Planes seleccionados con habitaciones seleccionadas

                foreach (var roomId in closure.RoomsList)
                {
                    foreach (var rateplanId in closure.RatePlansList)
                    {
                        var lockRoomTypesTemp = dbContext.spGetLockRoomTypesByHotel(hotelId, closure.StartDate.Value.Date, closure.EndDate.Value.Date, rateplanId, roomId).ToList();
                        lockRoomTypes.AddRange(lockRoomTypesTemp);
                    }
                }
            }

            var ratesplansListPromos = RatesPlanHelper.GetRatePlansPromosByHotel(hotelId, filterRatesPlansPromos);

            var availStatusMessagesLockRoomTypes = RestrictionsParser.ToAvailStatusMessages(lockRoomTypes);
            List<XElement> lockRoomTypeHotelAvailNotifRQList = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockRoomTypes);

            foreach (XElement lockRoomTypeHotelAvailNotifRQ in lockRoomTypeHotelAvailNotifRQList)
            {
                //Request LockRoomType
                var lockRoomTypeSoapRQ = Soap.CreateSoapRequestXml(lockRoomTypeHotelAvailNotifRQ);

                lockRoomTypesPrioritySoapRQ.Add(lockRoomTypeSoapRQ);
            }
            //Promociones
            if (ratesplansListPromos.Count > 0)
            {
                var activeRatePlansLockRoomTypes = lockRoomTypes.Select(lrt => lrt.RatePlanId).Distinct().ToList();
                var availStatusMessagesLockRoomTypesPromos = RestrictionsParser.ToAvailStatusMessages(hotelId, lockRoomTypes, activeRatePlansLockRoomTypes, ratesplansListPromos);
                List<XElement> lockRoomTypePromosHotelAvailNotifRQList = HotelAvailNotifRQ.CreateHotelAvailNotifRQList(availStatusMessagesLockRoomTypesPromos);

                foreach (XElement lockroomTypePromosHotelAvailNotifRQ in lockRoomTypePromosHotelAvailNotifRQList)
                {
                    var lockRoomTypePromosSoapRQ = Soap.CreateSoapRequestXml(lockroomTypePromosHotelAvailNotifRQ);
                    lockRoomTypesPrioritySoapRQ.Add(lockRoomTypePromosSoapRQ);
                }
            }

            lockTotal = lockRoomTypes.Count;

            return lockRoomTypesPrioritySoapRQ;
        }

    }
}
