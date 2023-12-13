using System;
using System.Collections.Generic;
using System.Linq;
using APIServices.Models;
using APIServices.Helpers.Room;
//using APIServices.Conflux.Helpers;
using APIServices.Conflux.Helpers.Rate;
using APIServices.Conflux.Models.Rates;
using APIServices.Conflux.OTA.Models.Rates;
using APIServices.Conflux.Enum;



namespace APIServices.Conflux.Parser
{
    public static class Parser
    {
        //General
        public static RateAmountMessages ToRateAmountMessages(List<spGetCurrentRatesByHotel_Result4> currentRates, int companyId, bool? plusTax, decimal? tax)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            RatesHelpers.Init(plusTax, tax);

            foreach (var currentRate in currentRates)
            {

                switch (currentRate.TypeRate)
                {
                    case (int) TypeRateEnum.RoomRate:

                        RoomRateMessages(currentRate, ref rateAmountMessages);

                        break;
                    case (int)TypeRateEnum.RoomRatePromotion:
                        RoomRatePromotionMessages(currentRate, ref rateAmountMessages);
                        break;

                }
            }


            return rateAmountMessages;
        }
        
        //Por Tarifa
        public static RateAmountMessages ToRateAmountMessages(List<vDayRates> rates,List<vDayRatesExceptions> ratesExceptions, int companyId, bool? plusTax, decimal? tax, TypeRateEnum typeRate)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            RatesHelpers.Init(plusTax, tax);
            //Para Tarifa Promociones ver si se tiene que poner condicion para diferenciar

            switch (typeRate)
            {
                case TypeRateEnum.RoomRate:

                    RoomRateMessages(rates, ref rateAmountMessages);

                    break;
                case TypeRateEnum.RoomRatePromotion:
                    RoomRatePromotionMessages(ratesExceptions, ref rateAmountMessages);
                    break;

            }

            return rateAmountMessages;
        }

        #region Delete
        //Invividual
        public static void ToRateAmountMessagesDelete(List<vDayRates> vDayRates, List<vDayRatesExceptions> ratesExceptions, TypeRateEnum typeRate, ref List<RateAmountMessage> rateAmountMessages)
        {
           
            switch (typeRate)
            {
                case TypeRateEnum.RoomRate:

                    foreach(var vDayRate in vDayRates)
                    {
                        var room = RoomHelper.GetRoom(vDayRate.RoomId);

                        RateAmountMessage rateAmountMessage = new RateAmountMessage();
                        rateAmountMessage.statusApplicationControl = new StatusApplicationControl { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

                        List<Rate> rates = new List<Rate>();

                        Rate rate = new Rate() 
                        {
                            StartDate = vDayRate.StartDate.ToString("yyyyMMdd"),
                            EndDate = vDayRate.EndDate.ToString("yyyyMMdd")
                        };

                        rates.Add(rate);

                        rateAmountMessage.Rates = rates;


                        rateAmountMessages.Add(rateAmountMessage);

                    }
                   
                    break;
                case TypeRateEnum.RoomRatePromotion:

                    foreach (var vDayRate in ratesExceptions)
                    {
                        var room = RoomHelper.GetRoom(vDayRate.RoomId);

                        RateAmountMessage rateAmountMessage = new RateAmountMessage();
                        rateAmountMessage.statusApplicationControl = new StatusApplicationControl { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

                        List<Rate> rates = new List<Rate>();

                        Rate rate = new Rate()
                        {
                            StartDate = vDayRate.StartDate.ToString("yyyyMMdd"),
                            EndDate = vDayRate.EndDate.ToString("yyyyMMdd")
                        };

                        rates.Add(rate);

                        rateAmountMessage.Rates = rates;


                        rateAmountMessages.Add(rateAmountMessage);

                    }

                    break;

            }

        }

        #endregion

        #region General
        public static void RoomRateMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages)
        {
            List<vDayRates> vDayRates = RatesHelpers.GetVDayRate(currentRate);

            foreach (var vDayRate in vDayRates)
            {
                var prices = RatesHelpers.GetPrices(currentRate.RateId);
                var pricesException = RatesHelpers.GetPricesException(currentRate.RateId);


                RateAmountMessage rateAmountMessage = new RateAmountMessage();

                StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

                List<Rate> rates = new List<Rate>();

                Rate rate = new Rate();
                rate.StartDate = vDayRate.StartDate < DateTime.Now.Date? DateTime.Now.Date.ToString("yyyyMMdd"): vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                rate.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                if (vDayRate.IsPromotion)
                {
                    rate.IsPromotion = true;

                    rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
                    rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
                    rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
                    rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
                    rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
                    rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
                    rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
                }

                //Ver si es habitacion vinculada y actualizar precios
                vLinkedRoomTypes linkedRoom = null;

                using(OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {
                    linkedRoom = ozHoteles.vLinkedRoomTypes
                        .Where(lkt => lkt.idtipohabitacion_Target == currentRate.RoomHotelId
                        && lkt.IdTipohabitacion_Source == currentRate.ParentRoomHotelId)
                        .FirstOrDefault();

                    if (linkedRoom != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref prices);
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                    }


                }


                //Ver si es plan vinculado y actualizar precios

                if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId)) {

                    vLinkedRatePlans linkedRatePlan = null;

                    using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                    if (linkedRatePlan != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref prices);
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                    }
                    
                }


                rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, currentRate);
                rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

                rates.Add(rate);

                //Precios Excepciones
                if (pricesException.Count > 0 && !vDayRate.IsPromotion)
                {
                    Rate rateException = new Rate();
                    rateException.HasPriceException = true;
                    rateException.StartDate = vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                    rateException.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                    rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                    rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                    rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                    rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                    rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                    rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                    rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                    rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);

                    if(rateException.BaseGuestAmounts.Count() > 0 )rates.Add(rateException);
                }

                rateAmountMessage.statusApplicationControl = statusApplicationControl;
                rateAmountMessage.Rates = rates;

                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
            }
        }
        
        public static void RoomRatePromotionMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages)
        {
            List<vDayRatesExceptions> vDayRates = RatesHelpers.GetVDayRateException(currentRate);

            foreach (var vDayRate in vDayRates)
            {
                var prices = RatesHelpers.GetPricesPromotion(currentRate.RateId);
                var pricesException = RatesHelpers.GetPricesPromotionException(currentRate.RateId);

                RateAmountMessage rateAmountMessage = new RateAmountMessage();

                StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

                List<Rate> rates = new List<Rate>();

                Rate rate = new Rate();
                rate.TypeRate = TypeRateEnum.RoomRatePromotion;
                rate.StartDate = vDayRate.StartDate < DateTime.Now.Date ? DateTime.Now.Date.ToString("yyyyMMdd") : vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                rate.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                if (vDayRate.IsPromotion)
                {
                    rate.IsPromotion = true;

                    rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
                    rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
                    rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
                    rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
                    rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
                    rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
                    rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
                }
                else if (!vDayRate.IsPromotion)
                {
                    rate.ApplyMon = vDayRate.ApplyDayMap[0] == 'Y' ? true : false;
                    rate.ApplyTue = vDayRate.ApplyDayMap[1] == 'Y' ? true : false;
                    rate.ApplyWed = vDayRate.ApplyDayMap[2] == 'Y' ? true : false;
                    rate.ApplyThu = vDayRate.ApplyDayMap[3] == 'Y' ? true : false;
                    rate.ApplyFri = vDayRate.ApplyDayMap[4] == 'Y' ? true : false;
                    rate.ApplySat = vDayRate.ApplyDayMap[5] == 'Y' ? true : false;
                    rate.ApplySun = vDayRate.ApplyDayMap[6] == 'Y' ? true : false;
                }

                //Ver si es habitacion vinculada y actualizar precios
                vLinkedRoomTypes linkedRoom = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {
                    linkedRoom = ozHoteles.vLinkedRoomTypes
                        .Where(lkt => lkt.idtipohabitacion_Target == currentRate.RoomHotelId
                        && lkt.IdTipohabitacion_Source == currentRate.ParentRoomHotelId)
                        .FirstOrDefault();

                    if (linkedRoom != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref prices);
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                    }
                }

                //Ver si es plan vinculado y actualizar precios

                if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
                {

                    vLinkedRatePlans linkedRatePlan = null;

                    using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId
                            && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                    if (linkedRatePlan != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref prices);
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                    }

                }


                rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, currentRate);
                rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

                rates.Add(rate);

                //Precios Excepciones
                if (pricesException.Count > 0 && !vDayRate.IsPromotion)
                {
                    Rate rateException = new Rate();
                    rateException.HasPriceException = true;
                    rateException.StartDate = vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                    rateException.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                    rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                    rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                    rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                    rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                    rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                    rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                    rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                    rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);

                    rates.Add(rateException);
                }

                rateAmountMessage.statusApplicationControl = statusApplicationControl;
                rateAmountMessage.Rates = rates;

                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
            }

        }
        #endregion

        #region Por Tarifa

        public static void RoomRateMessages(List<vDayRates> vDayRates, ref RateAmountMessages rateAmountMessages)
        {            
            foreach (var vDayRate in vDayRates)
            {
                var prices = RatesHelpers.GetPrices(vDayRate.RateId);
                var pricesException = RatesHelpers.GetPricesException(vDayRate.RateId);

                var room = RoomHelper.GetRoom(vDayRate.RoomId);

                RateAmountMessage rateAmountMessage = new RateAmountMessage();

                StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

                List<Rate> rates = new List<Rate>();

                Rate rate = new Rate();
                rate.StartDate = vDayRate.StartDate < DateTime.Now.Date ? DateTime.Now.Date.ToString("yyyyMMdd") : vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                rate.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                if (vDayRate.IsPromotion)
                {
                    rate.IsPromotion = true;

                    rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
                    rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
                    rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
                    rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
                    rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
                    rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
                    rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
                }

                //Ver si es habitacion vinculada y actualizar precios
                vLinkedRoomTypes linkedRoom = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {
                    linkedRoom = ozHoteles.vLinkedRoomTypes
                        .Where(lkt => lkt.idtipohabitacion_Target == vDayRate.RoomId)
                        .FirstOrDefault();

                    if (linkedRoom != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref prices);
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                    }

                }

                //Ver si es plan vinculado y actualizar precios

                if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
                {

                    vLinkedRatePlans linkedRatePlan = null;

                    using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                    if (linkedRatePlan != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref prices);
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                    }

                }

                //Se va usar para la cantidad maxima de adultos y ninios
                spGetCurrentRatesByHotel_Result4 roomCapactity = new spGetCurrentRatesByHotel_Result4()
                {
                    MaxAdults = room.MaxAdultsOccupancy,
                    MaxChildren = room.MaxChildrenOccupancy
                };

                rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, roomCapactity);
                rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

                rates.Add(rate);

                //Precios Excepciones
                if (pricesException.Count > 0 && !vDayRate.IsPromotion)
                {
                    Rate rateException = new Rate();
                    rateException.HasPriceException = true;
                    rateException.StartDate = vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                    rateException.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                    rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                    rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                    rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                    rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                    rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                    rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                    rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                    rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, roomCapactity);

                    if(rateException.BaseGuestAmounts.Count() > 0) rates.Add(rateException);
                }

                rateAmountMessage.statusApplicationControl = statusApplicationControl;
                rateAmountMessage.Rates = rates;

                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
            }
        }

        public static void RoomRatePromotionMessages(List<vDayRatesExceptions> vDayRates, ref RateAmountMessages rateAmountMessages)
        {
            foreach (var vDayRate in vDayRates)
            {
                var prices = RatesHelpers.GetPricesPromotion(vDayRate.RateId);
                var pricesException = RatesHelpers.GetPricesPromotionException(vDayRate.RateId);

                var room = RoomHelper.GetRoom(vDayRate.RoomId);

                RateAmountMessage rateAmountMessage = new RateAmountMessage();

                StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

                List<Rate> rates = new List<Rate>();

                Rate rate = new Rate();
                rate.TypeRate = TypeRateEnum.RoomRatePromotion;
                rate.StartDate = vDayRate.StartDate < DateTime.Now.Date ? DateTime.Now.Date.ToString("yyyyMMdd") : vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                rate.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                if (vDayRate.IsPromotion)
                {
                    rate.IsPromotion = true;

                    rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
                    rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
                    rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
                    rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
                    rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
                    rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
                    rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
                }
                else if (!vDayRate.IsPromotion)
                {
                    rate.ApplyMon = vDayRate.ApplyDayMap[0] == 'Y' ? true : false;
                    rate.ApplyTue = vDayRate.ApplyDayMap[1] == 'Y' ? true : false;
                    rate.ApplyWed = vDayRate.ApplyDayMap[2] == 'Y' ? true : false;
                    rate.ApplyThu = vDayRate.ApplyDayMap[3] == 'Y' ? true : false;
                    rate.ApplyFri = vDayRate.ApplyDayMap[4] == 'Y' ? true : false;
                    rate.ApplySat = vDayRate.ApplyDayMap[5] == 'Y' ? true : false;
                    rate.ApplySun = vDayRate.ApplyDayMap[6] == 'Y' ? true : false;
                }

                //Ver si es habitacion vinculada y actualizar precios
                vLinkedRoomTypes linkedRoom = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {
                    linkedRoom = ozHoteles.vLinkedRoomTypes
                        .Where(lkt => lkt.idtipohabitacion_Target == vDayRate.RoomId)
                        .FirstOrDefault();

                    if (linkedRoom != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref prices);
                        RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                    }
                }

                //Ver si es plan vinculado y actualizar precios

                if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
                {

                    vLinkedRatePlans linkedRatePlan = null;

                    using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId
                            && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                    if (linkedRatePlan != null)
                    {
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref prices);
                        RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                    }

                }

                //Se va usar para la cantidad maxima de adultos y ninios
                spGetCurrentRatesByHotel_Result4 roomCapacity = new spGetCurrentRatesByHotel_Result4() 
                {
                    MaxAdults = room.MaxAdultsOccupancy,
                    MaxChildren = room.MaxChildrenOccupancy
                };


                rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, roomCapacity);
                rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices);

                rates.Add(rate);

                //Precios Excepciones
                if (pricesException.Count > 0 && !vDayRate.IsPromotion)
                {
                    Rate rateException = new Rate();
                    rateException.HasPriceException = true;
                    rateException.StartDate = vDayRate.StartDate.ToString("yyyyMMdd");
                    rateException.EndDate = vDayRate.EndDate.ToString("yyyyMMdd");

                    rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                    rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                    rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                    rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                    rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                    rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                    rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                    rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, roomCapacity);

                    rates.Add(rateException);
                }

                rateAmountMessage.statusApplicationControl = statusApplicationControl;
                rateAmountMessage.Rates = rates;

                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
            }

        }

        #endregion
    }
}
