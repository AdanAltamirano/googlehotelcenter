using System;
using System.Collections.Generic;
using System.Linq;
using APIServices.Models;
using APIServices.Helpers.Room;
//using APIServices.Conflux.Helpers;
using APIServices.Conflux.Helpers.Rates;
using APIServices.Conflux.Models.Rates;
using APIServices.Conflux.OTA.Models.Rates;
using APIServices.Conflux.Enum;
using System.Configuration;

namespace APIServices.Conflux.Parser
{
    public static class Parser
    {
        //General
        public static RateAmountMessages ToRateAmountMessages(List<spGetCurrentRatesByHotel_Result4> currentRates, int companyId, bool? plusTax, decimal? tax, ref RateAmountMessages deleteRateAmountMessages)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            deleteRateAmountMessages.HotelCode = companyId;
            deleteRateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();


            RatesHelpers.Init(plusTax, tax);

            foreach (var currentRate in currentRates)
            {

                switch (currentRate.TypeRate)
                {
                    case (int) TypeRateEnum.RoomRate:

                        RoomRateMessages(currentRate, ref rateAmountMessages, ref deleteRateAmountMessages);

                        break;
                    case (int)TypeRateEnum.RoomRatePromotion:
                        RoomRatePromotionMessages(currentRate, ref rateAmountMessages, ref deleteRateAmountMessages);
                        break;

                }
            }


            return rateAmountMessages;
        }
        
        //Por Tarifa
        public static RateAmountMessages ToRateAmountMessages(List<vDayRates> rates,List<vDayRatesExceptions> ratesExceptions, int companyId, bool? plusTax, decimal? tax, TypeRateEnum typeRate, ref RateAmountMessages deleteRateAmountMessages)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            deleteRateAmountMessages.HotelCode = companyId;
            deleteRateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            RatesHelpers.Init(plusTax, tax);
            //Para Tarifa Promociones ver si se tiene que poner condicion para diferenciar

            switch (typeRate)
            {
                case TypeRateEnum.RoomRate:

                    RoomRateMessages(rates, ref rateAmountMessages, ref deleteRateAmountMessages);

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
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            switch (typeRate)
            {
                case TypeRateEnum.RoomRate:

                    foreach(var vDayRate in vDayRates)
                    {
                        if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
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

                    }
                   
                    break;
                case TypeRateEnum.RoomRatePromotion:

                    foreach (var vDayRate in ratesExceptions)
                    {
                        if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
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

                    }

                    break;

            }

        }

        #endregion

        #region General
        //public static void RoomRateMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages)
        //{
        //    string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

        //    char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

        //    List<vDayRates> vDayRates = RatesHelpers.GetVDayRate(currentRate);

        //    foreach (var vDayRate in vDayRates)
        //    {
        //        //El Segmento no esta en los segmentos no validos
        //        if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
        //        {

        //            var prices = RatesHelpers.GetPrices(currentRate.RateId);
        //            var pricesException = RatesHelpers.GetPricesException(currentRate.RateId);


        //            RateAmountMessage rateAmountMessage = new RateAmountMessage();

        //            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

        //            List<Rate> rates = new List<Rate>();

        //            var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
        //            var diff = vDayRate.EndDate.Date - starDate.Date;
        //            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;

        //            Rate rateException = new Rate();

        //            Rate rate = new Rate();
        //            rate.StartDate = starDate.Date.ToString("yyyyMMdd");//revisar el formato
        //            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

        //            if (vDayRate.IsPromotion)
        //            {
        //                rate.IsPromotion = true;

        //                rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
        //                rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
        //                rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
        //                rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
        //                rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
        //                rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
        //                rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
        //            }

        //            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, currentRate);
        //            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

        //            if (pricesException.Count > 0)
        //            {
        //                rateException.HasPriceException = true;
        //                rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
        //                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

        //                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
        //                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
        //                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
        //                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
        //                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
        //                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
        //                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

        //                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);

        //            }



        //            //Ver si es habitacion vinculada y actualizar precios
        //            vLinkedRoomTypes linkedRoom = null;

        //            using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //            {
        //                linkedRoom = ozHoteles.vLinkedRoomTypes
        //                    .Where(lkt => lkt.idtipohabitacion_Target == currentRate.RoomHotelId
        //                    && lkt.IdTipohabitacion_Source == currentRate.ParentRoomHotelId)
        //                    .FirstOrDefault();

        //                if (linkedRoom != null)
        //                {
        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;

        //                }

        //            }


        //            //Ver si es plan vinculado y actualizar precios

        //            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
        //            {

        //                vLinkedRatePlans linkedRatePlan = null;

        //                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //                {

        //                    if (vDayRate.IsPromotion)
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
        //                            .FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                            .FirstOrDefault();
        //                    }

        //                    //linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                    //    .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                    //    .FirstOrDefault();
        //                }

        //                if (linkedRatePlan != null)
        //                {
        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;
        //                }

        //            }



        //            rates.Add(rate);

        //            if (rateException.BaseGuestAmounts.Count() > 0
        //                && (rateException.ApplyMon ||
        //                rateException.ApplyTue ||
        //                rateException.ApplyWed ||
        //                rateException.ApplyThu ||
        //                rateException.ApplyFri ||
        //                rateException.ApplySat ||
        //                rateException.ApplySun)) { rates.Add(rateException); }


        //            rateAmountMessage.statusApplicationControl = statusApplicationControl;
        //            rateAmountMessage.Rates = rates;

        //            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
        //        }
        //    }
        //}

        //public static void RoomRatePromotionMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages)
        //{
        //    string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

        //    char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();


        //    List<vDayRatesExceptions> vDayRates = RatesHelpers.GetVDayRateException(currentRate);

        //    foreach (var vDayRate in vDayRates)
        //    {
        //        if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
        //        {

        //            var prices = RatesHelpers.GetPricesPromotion(currentRate.RateId);
        //            var pricesException = RatesHelpers.GetPricesPromotionException(currentRate.RateId);

        //            RateAmountMessage rateAmountMessage = new RateAmountMessage();

        //            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

        //            List<Rate> rates = new List<Rate>();

        //            var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
        //            var diff = vDayRate.EndDate.Date - starDate.Date;
        //            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;

        //            Rate rateException = new Rate();

        //            Rate rate = new Rate();
        //            rate.TypeRate = TypeRateEnum.RoomRatePromotion;
        //            rate.StartDate = starDate.Date.ToString("yyyyMMdd");
        //            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

        //            if (vDayRate.IsPromotion)
        //            {
        //                rate.IsPromotion = true;

        //                rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
        //                rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
        //                rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
        //                rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
        //                rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
        //                rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
        //                rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
        //            }
        //            else if (!vDayRate.IsPromotion)
        //            {
        //                rate.ApplyMon = vDayRate.ApplyDayMap[0] == 'Y' ? true : false;
        //                rate.ApplyTue = vDayRate.ApplyDayMap[1] == 'Y' ? true : false;
        //                rate.ApplyWed = vDayRate.ApplyDayMap[2] == 'Y' ? true : false;
        //                rate.ApplyThu = vDayRate.ApplyDayMap[3] == 'Y' ? true : false;
        //                rate.ApplyFri = vDayRate.ApplyDayMap[4] == 'Y' ? true : false;
        //                rate.ApplySat = vDayRate.ApplyDayMap[5] == 'Y' ? true : false;
        //                rate.ApplySun = vDayRate.ApplyDayMap[6] == 'Y' ? true : false;
        //            }

        //            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, currentRate);
        //            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

        //            //Precios Excepciones
        //            if (pricesException.Count > 0)
        //            {
        //                rateException.HasPriceException = true;
        //                rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
        //                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

        //                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
        //                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
        //                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
        //                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
        //                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
        //                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
        //                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

        //                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);
        //            }



        //            //Ver si es habitacion vinculada y actualizar precios
        //            vLinkedRoomTypes linkedRoom = null;

        //            using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //            {
        //                linkedRoom = ozHoteles.vLinkedRoomTypes
        //                    .Where(lkt => lkt.idtipohabitacion_Target == currentRate.RoomHotelId
        //                    && lkt.IdTipohabitacion_Source == currentRate.ParentRoomHotelId)
        //                    .FirstOrDefault();

        //                if (linkedRoom != null)
        //                {
        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;

        //                }
        //            }

        //            //Ver si es plan vinculado y actualizar precios

        //            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
        //            {

        //                vLinkedRatePlans linkedRatePlan = null;

        //                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //                {

        //                    if (vDayRate.IsPromotion)
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
        //                            .FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                            .FirstOrDefault();
        //                    }

        //                    //linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                    //    .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId
        //                    //    && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                    //    .FirstOrDefault();
        //                }

        //                if (linkedRatePlan != null)
        //                {
        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;

        //                }

        //            }


        //            rates.Add(rate);

        //            if (rateException.BaseGuestAmounts.Count() > 0
        //                && (rateException.ApplyMon ||
        //                rateException.ApplyTue ||
        //                rateException.ApplyWed ||
        //                rateException.ApplyThu ||
        //                rateException.ApplyFri ||
        //                rateException.ApplySat ||
        //                rateException.ApplySun)) { rates.Add(rateException); }


        //            rateAmountMessage.statusApplicationControl = statusApplicationControl;
        //            rateAmountMessage.Rates = rates;

        //            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
        //        }
        //    }

        //}

        public static void RoomRateMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages, ref RateAmountMessages deleteRateAmountMessages)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            List<vDayRates> vDayRates = RatesHelpers.GetVDayRate(currentRate);

            foreach (var vDayRate in vDayRates)
            {
                //El Segmento no esta en los segmentos no validos
                if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
                {
                    if (vDayRate.IsPromotion) 
                    {
                        if(vDayRate.PromoStartDateBookingWindow != null && vDayRate.PromoEndDateBookingWindow != null)                            
                        {
                            if (DateTime.Now.Date >= vDayRate.PromoStartDateBookingWindow && DateTime.Now.Date <= vDayRate.PromoEndDateBookingWindow)
                            {
                                RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);

                                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                            }
                            else
                            {
                                //Delete
                                RateAmountMessage rateAmountMessageToDelete = RatesHelpers.CreateDeleteRateAmountMessage(currentRate,vDayRate);
                                deleteRateAmountMessages.RateAmountMessagesList.Add(rateAmountMessageToDelete);
                            }
                        }
                        else
                        {
                            RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);

                            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                        }
                    }
                    else
                    {
                        RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);

                        rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                    }
                }
            }
        }

        public static void RoomRatePromotionMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages, ref RateAmountMessages deleteRateAmountMessages)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();


            List<vDayRatesExceptions> vDayRates = RatesHelpers.GetVDayRateException(currentRate);

            foreach (var vDayRate in vDayRates)
            {
                if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
                {

                    if (vDayRate.IsPromotion)
                    {
                        if (vDayRate.PromoStartDateBookingWindow != null && vDayRate.PromoEndDateBookingWindow != null)
                        {
                            if (DateTime.Now.Date >= vDayRate.PromoStartDateBookingWindow && DateTime.Now.Date <= vDayRate.PromoEndDateBookingWindow)
                            {
                                RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);

                                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                            }
                            else
                            {
                                //Delete
                                RateAmountMessage rateAmountMessageToDelete = RatesHelpers.CreateDeleteRateAmountMessage(currentRate, vDayRate);
                                deleteRateAmountMessages.RateAmountMessagesList.Add(rateAmountMessageToDelete);
                            }
                        }
                        else
                        {
                            RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);

                            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                        }
                    }
                    else
                    {
                        RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);

                        rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                    }

                }
            }

        }

        #endregion

        #region Por Tarifa

        //public static void RoomRateMessages(List<vDayRates> vDayRates, ref RateAmountMessages rateAmountMessages)
        //{
        //    string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

        //    char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();


        //    foreach (var vDayRate in vDayRates)
        //    {

        //        if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
        //        {

        //            var prices = RatesHelpers.GetPrices(vDayRate.RateId);
        //            var pricesException = RatesHelpers.GetPricesException(vDayRate.RateId);

        //            var room = RoomHelper.GetRoom(vDayRate.RoomId);

        //            RateAmountMessage rateAmountMessage = new RateAmountMessage();

        //            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

        //            List<Rate> rates = new List<Rate>();

        //            var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
        //            var diff = vDayRate.EndDate.Date - starDate.Date;
        //            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;

        //            Rate rateException = new Rate();

        //            Rate rate = new Rate();
        //            rate.StartDate = starDate.Date.ToString("yyyyMMdd");
        //            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

        //            if (vDayRate.IsPromotion)
        //            {
        //                rate.IsPromotion = true;

        //                rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
        //                rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
        //                rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
        //                rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
        //                rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
        //                rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
        //                rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
        //            }

        //            //Se va usar para la cantidad maxima de adultos y ninios
        //            spGetCurrentRatesByHotel_Result4 roomCapactity = new spGetCurrentRatesByHotel_Result4()
        //            {
        //                MaxAdults = room.MaxAdultsOccupancy,
        //                MaxChildren = room.MaxChildrenOccupancy
        //            };

        //            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, roomCapactity);
        //            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra


        //            if (pricesException.Count > 0)
        //            {
        //                rateException.HasPriceException = true;
        //                rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
        //                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

        //                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
        //                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
        //                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
        //                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
        //                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
        //                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
        //                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

        //                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, roomCapactity);
        //            }

        //            //Ver si es habitacion vinculada y actualizar precios
        //            vLinkedRoomTypes linkedRoom = null;

        //            using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //            {
        //                linkedRoom = ozHoteles.vLinkedRoomTypes
        //                    .Where(lkt => lkt.idtipohabitacion_Target == vDayRate.RoomId)
        //                    .FirstOrDefault();

        //                if (linkedRoom != null)
        //                {

        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;

        //                }

        //            }

        //            //Ver si es plan vinculado y actualizar precios

        //            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
        //            {

        //                vLinkedRatePlans linkedRatePlan = null;

        //                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //                {
        //                    if (vDayRate.IsPromotion)
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr =>  lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
        //                            .FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                            .FirstOrDefault();
        //                    }
        //                }

        //                if (linkedRatePlan != null)
        //                {
        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;

        //                }

        //            }

        //            if (rateException.BaseGuestAmounts.Count() > 0
        //                && (rateException.ApplyMon ||
        //                rateException.ApplyTue ||
        //                rateException.ApplyWed ||
        //                rateException.ApplyThu ||
        //                rateException.ApplyFri ||
        //                rateException.ApplySat ||
        //                rateException.ApplySun)) { rates.Add(rateException); }


        //            rates.Add(rate);


        //            rateAmountMessage.statusApplicationControl = statusApplicationControl;
        //            rateAmountMessage.Rates = rates;

        //            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
        //        }
        //    }
        //}

        //public static void RoomRatePromotionMessages(List<vDayRatesExceptions> vDayRates, ref RateAmountMessages rateAmountMessages)
        //{
        //    string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

        //    char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

        //    foreach (var vDayRate in vDayRates)
        //    {
        //        if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
        //        {

        //            var prices = RatesHelpers.GetPricesPromotion(vDayRate.RateId);
        //            var pricesException = RatesHelpers.GetPricesPromotionException(vDayRate.RateId);

        //            var room = RoomHelper.GetRoom(vDayRate.RoomId);

        //            RateAmountMessage rateAmountMessage = new RateAmountMessage();

        //            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

        //            List<Rate> rates = new List<Rate>();

        //            var startDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
        //            var diff = vDayRate.EndDate.Date - startDate.Date;
        //            var endDate = diff.TotalDays > 1096 ? startDate.AddYears(3) : vDayRate.EndDate.Date;

        //            Rate rateException = new Rate();

        //            Rate rate = new Rate();
        //            rate.TypeRate = TypeRateEnum.RoomRatePromotion;
        //            rate.StartDate = startDate.Date.ToString("yyyyMMdd");
        //            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

        //            if (vDayRate.IsPromotion)
        //            {
        //                rate.IsPromotion = true;

        //                rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
        //                rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
        //                rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
        //                rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
        //                rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
        //                rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
        //                rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;
        //            }
        //            else if (!vDayRate.IsPromotion)
        //            {
        //                rate.ApplyMon = vDayRate.ApplyDayMap[0] == 'Y' ? true : false;
        //                rate.ApplyTue = vDayRate.ApplyDayMap[1] == 'Y' ? true : false;
        //                rate.ApplyWed = vDayRate.ApplyDayMap[2] == 'Y' ? true : false;
        //                rate.ApplyThu = vDayRate.ApplyDayMap[3] == 'Y' ? true : false;
        //                rate.ApplyFri = vDayRate.ApplyDayMap[4] == 'Y' ? true : false;
        //                rate.ApplySat = vDayRate.ApplyDayMap[5] == 'Y' ? true : false;
        //                rate.ApplySun = vDayRate.ApplyDayMap[6] == 'Y' ? true : false;
        //            }

        //            //Se va usar para la cantidad maxima de adultos y ninios
        //            spGetCurrentRatesByHotel_Result4 roomCapacity = new spGetCurrentRatesByHotel_Result4()
        //            {
        //                MaxAdults = room.MaxAdultsOccupancy,
        //                MaxChildren = room.MaxChildrenOccupancy
        //            };


        //            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, roomCapacity);
        //            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices);

        //            if (pricesException.Count > 0)
        //            {
        //                rateException.HasPriceException = true;
        //                rateException.StartDate = startDate.Date.ToString("yyyyMMdd");
        //                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

        //                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
        //                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
        //                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
        //                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
        //                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
        //                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
        //                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

        //                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, roomCapacity);

        //            }



        //            //Ver si es habitacion vinculada y actualizar precios
        //            vLinkedRoomTypes linkedRoom = null;

        //            using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //            {
        //                linkedRoom = ozHoteles.vLinkedRoomTypes
        //                    .Where(lkt => lkt.idtipohabitacion_Target == vDayRate.RoomId)
        //                    .FirstOrDefault();

        //                if (linkedRoom != null)
        //                {
        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;

        //                }
        //            }

        //            //Ver si es plan vinculado y actualizar precios

        //            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
        //            {

        //                vLinkedRatePlans linkedRatePlan = null;

        //                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
        //                {

        //                    if (vDayRate.IsPromotion)
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
        //                            .FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                            .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                            .FirstOrDefault();
        //                    }


        //                    //linkedRatePlan = ozHoteles.vLinkedRatePlans
        //                    //    .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId
        //                    //    && lrr.TargetRatePlan == vDayRate.RatePlanId)
        //                    //    .FirstOrDefault();
        //                }

        //                if (linkedRatePlan != null)
        //                {

        //                    var tempBaseGuestAmounts = rate.BaseGuestAmounts;
        //                    var tempAdditionalGuestAmounts = rate.AdditionalGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempBaseGuestAmounts, RatesHelpers.Tax);
        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempAdditionalGuestAmounts);

        //                    rate.BaseGuestAmounts = tempBaseGuestAmounts;
        //                    rate.AdditionalGuestAmounts = tempAdditionalGuestAmounts;

        //                    var tempExceptionBaseGuestAmounts = rateException.BaseGuestAmounts;

        //                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref tempExceptionBaseGuestAmounts, RatesHelpers.Tax);

        //                    rateException.BaseGuestAmounts = tempExceptionBaseGuestAmounts;


        //                }

        //            }

        //            if (rateException.BaseGuestAmounts.Count() > 0
        //                && (rateException.ApplyMon ||
        //                rateException.ApplyTue ||
        //                rateException.ApplyWed ||
        //                rateException.ApplyThu ||
        //                rateException.ApplyFri ||
        //                rateException.ApplySat ||
        //                rateException.ApplySun)) { rates.Add(rateException); }

        //            rates.Add(rate);

        //            rateAmountMessage.statusApplicationControl = statusApplicationControl;
        //            rateAmountMessage.Rates = rates;

        //            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
        //        }
        //    }

        //}

        public static void RoomRateMessages(List<vDayRates> vDayRates, ref RateAmountMessages rateAmountMessages, ref RateAmountMessages deleteRateAmountMesages)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();


            foreach (var vDayRate in vDayRates)
            {

                if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
                {
                    if (vDayRate.IsPromotion)
                    {
                        if (vDayRate.PromoStartDateBookingWindow != null && vDayRate.PromoEndDateBookingWindow != null)
                        {
                            if (DateTime.Now.Date >= vDayRate.PromoStartDateBookingWindow && DateTime.Now.Date <= vDayRate.PromoEndDateBookingWindow)
                            {
                                RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(vDayRate);

                                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                            }
                            else
                            {
                                RateAmountMessage deleteRateAmountMessage = RatesHelpers.CreateDeleteRateAmountMessage(vDayRate);
                                deleteRateAmountMesages.RateAmountMessagesList.Add(deleteRateAmountMessage);
                            }
                        }
                        else
                        {
                            RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(vDayRate);

                            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                        }

                    }
                    else
                    {
                        RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(vDayRate);

                        rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                    }
                }
            }
        }

        public static void RoomRatePromotionMessages(List<vDayRatesExceptions> vDayRates, ref RateAmountMessages rateAmountMessages)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            foreach (var vDayRate in vDayRates)
            {
                if (vDayRate.Segment.IndexOfAny(segmentsNoRates) == -1 && (!vDayRate.IsMobileRate && !vDayRate.IsCallCenterOnly))
                {
                    if (vDayRate.IsPromotion)
                    {
                        if (vDayRate.PromoStartDateBookingWindow != null && vDayRate.PromoEndDateBookingWindow != null)
                        {
                            if (DateTime.Now.Date >= vDayRate.PromoStartDateBookingWindow && DateTime.Now.Date <= vDayRate.PromoEndDateBookingWindow)
                            {
                                RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(vDayRate);

                                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                            }
                        }
                        else
                        {
                            RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(vDayRate);

                            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                        }

                    }
                    else
                    {
                        RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(vDayRate);

                        rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                    }
                }
            }

        }


        #endregion
    }
}
