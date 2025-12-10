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
        public static Models.Rates.Response.RatesMessages ToRateAmountMessages(List<spGetCurrentRatesByHotel_Result4> currentRates, int hotelId, int companyId, bool? plusTax, decimal? tax, string currency, DateTime? startDate, DateTime? endDate)
        {
            Models.Rates.Response.RatesMessages ratesMessages = new Models.Rates.Response.RatesMessages();

            RateAmountMessages rateAmountMessages = new RateAmountMessages();
            RateAmountMessages deleteRateAmountMessages = new RateAmountMessages();
            RateAmountMessages rateAmountMessagesExceptions = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.HotelCodeV2 = hotelId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            deleteRateAmountMessages.HotelCode = companyId;
            deleteRateAmountMessages.HotelCodeV2 = hotelId;
            deleteRateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            rateAmountMessagesExceptions.HotelCode = companyId;
            rateAmountMessagesExceptions.HotelCodeV2 = hotelId;
            rateAmountMessagesExceptions.RateAmountMessagesList = new List<RateAmountMessage>();


            RatesHelpers.Init(hotelId, plusTax, tax, currency);

            foreach (var currentRate in currentRates)
            {

                switch (currentRate.TypeRate)
                {
                    case (int)TypeRateEnum.RoomRate:

                        RoomRateMessages(currentRate, ref rateAmountMessages, ref deleteRateAmountMessages, ref rateAmountMessagesExceptions);

                        break;
                    case (int)TypeRateEnum.RoomRatePromotion:
                        RoomRatePromotionMessages(currentRate, ref rateAmountMessages, ref deleteRateAmountMessages, ref rateAmountMessagesExceptions);
                        break;

                }
            }


            var rateAmountMessagesListTemp = rateAmountMessages.RateAmountMessagesList;
            RatesHelpers.CheckDatesCalendar(startDate, endDate, ref rateAmountMessagesListTemp);
            rateAmountMessages.RateAmountMessagesList = rateAmountMessagesListTemp;


            if (rateAmountMessagesExceptions.RateAmountMessagesList.Count > 0)
            {
                var rateAmountMessagesExceptionListTemp = rateAmountMessagesExceptions.RateAmountMessagesList;
                RatesHelpers.CheckDatesCalendar(startDate, endDate, ref rateAmountMessagesExceptionListTemp);
                rateAmountMessagesExceptions.RateAmountMessagesList = rateAmountMessagesExceptionListTemp;
            }



            //0: tarifas, 1: borrar, 2: tarifas excepciones
            ratesMessages.RateAmountMessagesList.Add(rateAmountMessages);
            ratesMessages.RateAmountMessagesList.Add(deleteRateAmountMessages);
            ratesMessages.RateAmountMessagesList.Add(rateAmountMessagesExceptions);

            return ratesMessages;
        }

        //Por Tarifa
        public static RateAmountMessages ToRateAmountMessages(List<vDayRates> rates, List<vDayRatesExceptions> ratesExceptions, int hotelId, int companyId, bool? plusTax, decimal? tax, string currency, TypeRateEnum typeRate, ref RateAmountMessages deleteRateAmountMessages)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();


            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            deleteRateAmountMessages.HotelCode = companyId;
            deleteRateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            RatesHelpers.Init(hotelId, plusTax, tax, currency);
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

        public static Models.Rates.Response.RatesMessages ToRateAmountMessages(List<vDayRates> rates, List<vDayRatesExceptions> ratesExceptions, int hotelId, int companyId, bool? plusTax, decimal? tax, string currency, TypeRateEnum typeRate)
        {
            Models.Rates.Response.RatesMessages ratesMessages = new Models.Rates.Response.RatesMessages();

            RateAmountMessages rateAmountMessages = new RateAmountMessages();
            RateAmountMessages deleteRateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.HotelCodeV2 = hotelId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            deleteRateAmountMessages.HotelCode = companyId;
            deleteRateAmountMessages.HotelCodeV2 = hotelId;
            deleteRateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            RatesHelpers.Init(hotelId, plusTax, tax, currency);
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

            //0: tarifas, 1: borrar, 2: tarifas excepciones
            ratesMessages.RateAmountMessagesList.Add(rateAmountMessages);
            ratesMessages.RateAmountMessagesList.Add(deleteRateAmountMessages);

            return ratesMessages;
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

                    foreach (var vDayRate in vDayRates)
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

        public static List<RateAmountMessage> ToRateAmountMessagesDelete(List<vDayRates> vDayRates, List<vDayRatesExceptions> ratesExceptions, TypeRateEnum typeRate)
        {
            List<RateAmountMessage> result = new List<RateAmountMessage>();

            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            switch (typeRate)
            {
                case TypeRateEnum.RoomRate:

                    foreach (var vDayRate in vDayRates)
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


                            result.Add(rateAmountMessage);
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


                            result.Add(rateAmountMessage);
                        }

                    }

                    break;

            }


            return result;

        }

        public static RateAmountMessages ToRateAmountMessagesDelete(int companyId,int hotelId, string promotionCode, DateTime endDate, List<string> rateplansListAux, List<string> roomsListAux)
        {
            var rateAmountMessages = new RateAmountMessages
            {
                HotelCode = companyId,
                HotelCodeV2 = hotelId,
                RateAmountMessagesList = new List<OTA.Models.Rates.RateAmountMessage>()
            };

            foreach (var ratePlan in rateplansListAux)
            {
                foreach (var room in roomsListAux)
                {
                    string ratePlanCode = promotionCode + ratePlan;

                    var rateAmountMessage = new OTA.Models.Rates.RateAmountMessage
                    {
                        statusApplicationControl = new OTA.Models.Rates.StatusApplicationControl
                        {
                            RatePlanCode = ratePlanCode,
                            InvTypeCode = room
                        }
                    };

                    var rate = new OTA.Models.Rates.Rate
                    {
                        StartDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                        EndDate = endDate.Date.ToString("yyyyMMdd")
                    };

                    rateAmountMessage.Rates = new List<OTA.Models.Rates.Rate> { rate };

                    rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                }
            }

            return rateAmountMessages;
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

        public static void RoomRateMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages, ref RateAmountMessages deleteRateAmountMessages, ref RateAmountMessages rateAmountMessagesExceptions)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            List<vDayRates> vDayRates = RatesHelpers.GetVDayRate(currentRate,false);

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
                                RateAmountMessage rateAmountMessageException = RatesHelpers.CreateRateAmountMessageException(currentRate, vDayRate);

                                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                                if (rateAmountMessageException != null) rateAmountMessagesExceptions.RateAmountMessagesList.Add(rateAmountMessageException);
                            }
                            else
                            {
                                //Delete
                                RateAmountMessage rateAmountMessageToDelete = RatesHelpers.CreateDeleteRateAmountMessage(currentRate,vDayRate);
                                
                                if(rateAmountMessageToDelete != null && vDayRate.DeletedInGoogle == false) deleteRateAmountMessages.RateAmountMessagesList.Add(rateAmountMessageToDelete);
                            }
                        }
                        else
                        {
                            RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);
                            RateAmountMessage rateAmountMessageException = RatesHelpers.CreateRateAmountMessageException(currentRate, vDayRate);

                            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                            if (rateAmountMessageException != null) rateAmountMessagesExceptions.RateAmountMessagesList.Add(rateAmountMessageException);
                        }
                    }
                    else
                    {
                        RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);
                        RateAmountMessage rateAmountMessageException = RatesHelpers.CreateRateAmountMessageException(currentRate, vDayRate);

                        rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                        if (rateAmountMessageException != null) rateAmountMessagesExceptions.RateAmountMessagesList.Add(rateAmountMessageException);
                    }
                }
            }
        }

        public static void RoomRatePromotionMessages(spGetCurrentRatesByHotel_Result4 currentRate, ref RateAmountMessages rateAmountMessages, ref RateAmountMessages deleteRateAmountMessages, ref RateAmountMessages rateAmountMessagesExceptions)
        {
            string[] splitSegmentsNoRates = ConfigurationManager.AppSettings["segmentsNoRates"].Split(',');

            char[] segmentsNoRates = string.Concat(splitSegmentsNoRates).ToCharArray();

            List<vDayRatesExceptions> vDayRates = RatesHelpers.GetVDayRateException(currentRate,false);

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
                                RateAmountMessage rateAmountMessageException = RatesHelpers.CreateRateAmountMessageException(currentRate, vDayRate);

                                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                                if (rateAmountMessageException != null) rateAmountMessagesExceptions.RateAmountMessagesList.Add(rateAmountMessageException);
                            }
                            else
                            {
                                //Delete
                                RateAmountMessage rateAmountMessageToDelete = RatesHelpers.CreateDeleteRateAmountMessage(currentRate, vDayRate);
                                if(rateAmountMessageToDelete != null && vDayRate.DeletedInGoogle == false) deleteRateAmountMessages.RateAmountMessagesList.Add(rateAmountMessageToDelete);
                            }
                        }
                        else
                        {
                            RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);
                            RateAmountMessage rateAmountMessageException = RatesHelpers.CreateRateAmountMessageException(currentRate, vDayRate);

                            rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                            if (rateAmountMessageException != null) rateAmountMessagesExceptions.RateAmountMessagesList.Add(rateAmountMessageException);
                        }
                    }
                    else
                    {
                        RateAmountMessage rateAmountMessage = RatesHelpers.CreateRateAmountMessage(currentRate, vDayRate);
                        RateAmountMessage rateAmountMessageException = RatesHelpers.CreateRateAmountMessageException(currentRate, vDayRate);

                        rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                        if (rateAmountMessageException != null) rateAmountMessagesExceptions.RateAmountMessagesList.Add(rateAmountMessageException);
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
                                if (deleteRateAmountMessage != null) deleteRateAmountMesages.RateAmountMessagesList.Add(deleteRateAmountMessage);
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
