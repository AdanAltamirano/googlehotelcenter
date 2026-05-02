using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APIServices.Models;
using APIServices.Helpers.Room;
using APIServices.Conflux.Models.Rates;
using APIServices.Conflux.OTA.Models.Rates;
using APIServices.Conflux.Enum;
using System.Configuration;


namespace APIServices.Conflux.Helpers.Rates
{
    public static partial class RatesHelpers
    {
        #region General
        //General Tarifas Normales
        public static RateAmountMessage CreateRateAmountMessage(spGetCurrentRatesByHotel_Result4 currentRate, vDayRates vDayRate)
        {
            var prices = RatesHelpers.GetPrices(currentRate.RateId);
            //var pricesException = RatesHelpers.GetPricesException(currentRate.RateId);


            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if(vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            //var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date; 01 07 2024
            var diff = vDayRate.EndDate.Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;


            Rate rate = new Rate();
            rate.CurrencyCode = RatesHelpers.Currency;
            rate.StartDate = starDate.Date.ToString("yyyyMMdd");//revisar el formato
            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

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
                    .Where(lkt => lkt.idtipohabitacion_Target == currentRate.RoomHotelId
                    && lkt.IdTipohabitacion_Source == currentRate.ParentRoomHotelId)
                    .FirstOrDefault();

                if (linkedRoom != null)
                {
                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref prices);
                    //RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                }


            }


            //Ver si es plan vinculado y actualizar precios

            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
            {

                vLinkedRatePlans linkedRatePlan = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {

                    if (vDayRate.IsPromotion)
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
                            .FirstOrDefault();
                    }
                    else
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                }

                if (linkedRatePlan != null)
                {
                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref prices);
                    //RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                }

            }


            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, currentRate);
            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

            rates.Add(rate);

            /**
             * Se comenta las exepciones para que cargue primero las tarifas sin excepciones y luego cargue las tarifas con excepciones 
            */

            ////Precios Excepciones
            //if (pricesException.Count > 0)
            //{
            //    Rate rateException = new Rate();
            //    rateException.CurrencyCode = RatesHelpers.Currency;
            //    rateException.HasPriceException = true;
            //    rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
            //    rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

            //    rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
            //    rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
            //    rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
            //    rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
            //    rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
            //    rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
            //    rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

            //    rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);

            //    if (rateException.BaseGuestAmounts.Count() > 0
            //        && (rateException.ApplyMon ||
            //        rateException.ApplyTue ||
            //        rateException.ApplyWed ||
            //        rateException.ApplyThu ||
            //        rateException.ApplyFri ||
            //        rateException.ApplySat ||
            //        rateException.ApplySun)) { rates.Add(rateException); }
            //}

            rateAmountMessage.statusApplicationControl = statusApplicationControl;
            rateAmountMessage.Rates = rates;

            return rateAmountMessage;

        }

        public static RateAmountMessage CreateRateAmountMessageException(spGetCurrentRatesByHotel_Result4 currentRate, vDayRates vDayRate)
        {
            var pricesException = RatesHelpers.GetPricesException(currentRate.RateId);

            if (pricesException == null || pricesException.Count == 0) return null;

             RateAmountMessage rateAmountMessage = new RateAmountMessage();

            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            var diff = vDayRate.EndDate.Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;

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
                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                }

            }

            //Ver si es plan vinculado y actualizar precios

            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
            {

                vLinkedRatePlans linkedRatePlan = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {

                    if (vDayRate.IsPromotion)
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
                            .FirstOrDefault();
                    }
                    else
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                }

                if (linkedRatePlan != null)
                {
                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                }

            }

            //Precios Excepciones
            if (pricesException.Count > 0)
            {
                Rate rateException = new Rate();
                rateException.CurrencyCode = RatesHelpers.Currency;
                rateException.HasPriceException = true;
                rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);
                rateException.AdditionalGuestAmounts = new List<AdditionalGuestAmount>();

                if (rateException.BaseGuestAmounts.Count() > 0
                    && (rateException.ApplyMon ||
                    rateException.ApplyTue ||
                    rateException.ApplyWed ||
                    rateException.ApplyThu ||
                    rateException.ApplyFri ||
                    rateException.ApplySat ||
                    rateException.ApplySun)) 
                { 
                    rates.Add(rateException); 
                }
                else
                {
                    return null;
                }
            }

            rateAmountMessage.statusApplicationControl = statusApplicationControl;
            rateAmountMessage.Rates = rates;

            return rateAmountMessage;

        }

        //General Tarifas Promociones
        public static RateAmountMessage CreateRateAmountMessage(spGetCurrentRatesByHotel_Result4 currentRate, vDayRatesExceptions vDayRate)
        {

            var prices = RatesHelpers.GetPricesPromotion(currentRate.RateId);
            //var pricesException = RatesHelpers.GetPricesPromotionException(currentRate.RateId);

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            //var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
            var diff = vDayRate.EndDate.Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;


            Rate rate = new Rate();
            rate.CurrencyCode = RatesHelpers.Currency;
            rate.TypeRate = TypeRateEnum.RoomRatePromotion;
            rate.StartDate = starDate.Date.ToString("yyyyMMdd");
            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

            if (vDayRate.IsPromotion)
            {
                rate.IsPromotion = true;

                //rate.ApplyMon = vDayRate.PromoDays[0] == 'Y' ? true : false;
                //rate.ApplyTue = vDayRate.PromoDays[1] == 'Y' ? true : false;
                //rate.ApplyWed = vDayRate.PromoDays[2] == 'Y' ? true : false;
                //rate.ApplyThu = vDayRate.PromoDays[3] == 'Y' ? true : false;
                //rate.ApplyFri = vDayRate.PromoDays[4] == 'Y' ? true : false;
                //rate.ApplySat = vDayRate.PromoDays[5] == 'Y' ? true : false;
                //rate.ApplySun = vDayRate.PromoDays[6] == 'Y' ? true : false;

                rate.ApplyMon = vDayRate.ApplyDayMap[0] == 'Y' ? true : false;
                rate.ApplyTue = vDayRate.ApplyDayMap[1] == 'Y' ? true : false;
                rate.ApplyWed = vDayRate.ApplyDayMap[2] == 'Y' ? true : false;
                rate.ApplyThu = vDayRate.ApplyDayMap[3] == 'Y' ? true : false;
                rate.ApplyFri = vDayRate.ApplyDayMap[4] == 'Y' ? true : false;
                rate.ApplySat = vDayRate.ApplyDayMap[5] == 'Y' ? true : false;
                rate.ApplySun = vDayRate.ApplyDayMap[6] == 'Y' ? true : false;

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
                    //RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                }
            }

            //Ver si es plan vinculado y actualizar precios

            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
            {

                vLinkedRatePlans linkedRatePlan = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {

                    if (vDayRate.IsPromotion)
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId &&  lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
                            .FirstOrDefault();
                    }
                    else
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                    //linkedRatePlan = ozHoteles.vLinkedRatePlans
                    //    .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId
                    //    && lrr.TargetRatePlan == vDayRate.RatePlanId)
                    //    .FirstOrDefault();
                }

                if (linkedRatePlan != null)
                {
                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref prices);
                    //RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                }

            }


            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, currentRate);
            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

            rates.Add(rate);

            //Precios Excepciones
            //if (pricesException.Count > 0)
            //{
            //    Rate rateException = new Rate();
            //    rateException.CurrencyCode = RatesHelpers.Currency;
            //    rateException.HasPriceException = true;
            //    rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
            //    rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

            //    rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
            //    rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
            //    rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
            //    rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
            //    rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
            //    rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
            //    rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

            //    rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);

            //    if (rateException.BaseGuestAmounts.Count() > 0
            //        && (rateException.ApplyMon ||
            //        rateException.ApplyTue ||
            //        rateException.ApplyWed ||
            //        rateException.ApplyThu ||
            //        rateException.ApplyFri ||
            //        rateException.ApplySat ||
            //        rateException.ApplySun)) { rates.Add(rateException); }
            //}

            rateAmountMessage.statusApplicationControl = statusApplicationControl;
            rateAmountMessage.Rates = rates;

            return rateAmountMessage;

        }

        public static RateAmountMessage CreateRateAmountMessageException(spGetCurrentRatesByHotel_Result4 currentRate, vDayRatesExceptions vDayRate)
        {
            var pricesException = RatesHelpers.GetPricesPromotionException(currentRate.RateId);

            if (pricesException == null || pricesException.Count == 0) return null;

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            
            var diff = vDayRate.EndDate.Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;

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
                    RatesHelpers.UpdatePricesLinkedRoom(linkedRoom, ref pricesException);
                }
            }

            //Ver si es plan vinculado y actualizar precios

            if (!string.IsNullOrEmpty(vDayRate.ParentRatePlanId))
            {

                vLinkedRatePlans linkedRatePlan = null;

                using (OzHotelesEntities ozHoteles = new OzHotelesEntities())
                {

                    if (vDayRate.IsPromotion)
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
                            .FirstOrDefault();
                    }
                    else
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }

                }

                if (linkedRatePlan != null)
                {
                    RatesHelpers.UpdatePricesLinkedRatePlan(linkedRatePlan, ref pricesException);
                }

            }

            if (pricesException.Count > 0)
            {
                Rate rateException = new Rate();
                rateException.CurrencyCode = RatesHelpers.Currency;
                rateException.HasPriceException = true;
                rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, currentRate);
                rateException.AdditionalGuestAmounts = new List<AdditionalGuestAmount>();

                if (rateException.BaseGuestAmounts.Count() > 0
                    && (rateException.ApplyMon ||
                    rateException.ApplyTue ||
                    rateException.ApplyWed ||
                    rateException.ApplyThu ||
                    rateException.ApplyFri ||
                    rateException.ApplySat ||
                    rateException.ApplySun)) 
                { 
                    rates.Add(rateException); 
                }
                else
                {
                    return null;
                }
            }

            rateAmountMessage.statusApplicationControl = statusApplicationControl;
            rateAmountMessage.Rates = rates;

            return rateAmountMessage;
        }


        #endregion

        #region Por Tarifa
        //Tarifa Normal
        public static RateAmountMessage CreateRateAmountMessage(vDayRates vDayRate)
        {
            var prices = RatesHelpers.GetPrices(vDayRate.RateId);
            var pricesException = RatesHelpers.GetPricesException(vDayRate.RateId);

            var room = RoomHelper.GetRoom(vDayRate.RoomId);

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            //var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
            var diff = vDayRate.EndDate.Date - starDate.Date;
            var endDate = diff.TotalDays > 1096 ? starDate.AddYears(3) : vDayRate.EndDate.Date;

            Rate rate = new Rate();
            rate.CurrencyCode = RatesHelpers.Currency;
            rate.StartDate = starDate.Date.ToString("yyyyMMdd");
            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

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
                    if (vDayRate.IsPromotion)
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
                            .FirstOrDefault();
                    }
                    else
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }
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
                MinAdults = room.MinAdultsOccupancy,
                MaxChildren = room.MaxChildrenOccupancy
            };

            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, roomCapactity);
            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

            rates.Add(rate);

            //Precios Excepciones
            if (pricesException.Count > 0)
            {
                Rate rateException = new Rate();
                rateException.CurrencyCode = RatesHelpers.Currency;
                rateException.HasPriceException = true;
                rateException.StartDate = starDate.Date.ToString("yyyyMMdd");
                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, roomCapactity);

                if (rateException.BaseGuestAmounts.Count() > 0
                    && (rateException.ApplyMon ||
                    rateException.ApplyTue ||
                    rateException.ApplyWed ||
                    rateException.ApplyThu ||
                    rateException.ApplyFri ||
                    rateException.ApplySat ||
                    rateException.ApplySun)) { rates.Add(rateException); }

            }

            rateAmountMessage.statusApplicationControl = statusApplicationControl;
            rateAmountMessage.Rates = rates;

            return rateAmountMessage;

        }
        //Tarifa Promocion
        public static RateAmountMessage CreateRateAmountMessage(vDayRatesExceptions vDayRate)
        {
            var prices = RatesHelpers.GetPricesPromotion(vDayRate.RateId);
            var pricesException = RatesHelpers.GetPricesPromotionException(vDayRate.RateId);

            var room = RoomHelper.GetRoom(vDayRate.RoomId);

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

            List<Rate> rates = new List<Rate>();

            DateTime startDate = vDayRate.StartDate.Date;

            if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                startDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                startDate = DateTime.Now.Date;
            }

            //var startDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;
            var diff = vDayRate.EndDate.Date - startDate.Date;
            var endDate = diff.TotalDays > 1096 ? startDate.AddYears(3) : vDayRate.EndDate.Date;

            Rate rate = new Rate();
            rate.CurrencyCode = RatesHelpers.Currency;
            rate.TypeRate = TypeRateEnum.RoomRatePromotion;
            rate.StartDate = startDate.Date.ToString("yyyyMMdd");
            rate.EndDate = endDate.Date.ToString("yyyyMMdd");

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

                    if (vDayRate.IsPromotion)
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.TargetRatePlan == vDayRate.ParentRatePlanId)
                            .FirstOrDefault();
                    }
                    else
                    {
                        linkedRatePlan = ozHoteles.vLinkedRatePlans
                            .Where(lrr => lrr.IdHotel == RatesHelpers.HotelId && lrr.SourceRatePlan == vDayRate.ParentRatePlanId && lrr.TargetRatePlan == vDayRate.RatePlanId)
                            .FirstOrDefault();
                    }


                    //linkedRatePlan = ozHoteles.vLinkedRatePlans
                    //    .Where(lrr => lrr.SourceRatePlan == vDayRate.ParentRatePlanId
                    //    && lrr.TargetRatePlan == vDayRate.RatePlanId)
                    //    .FirstOrDefault();
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
                MinAdults = room.MinAdultsOccupancy,
                MaxAdults = room.MaxAdultsOccupancy,
                MaxChildren = room.MaxChildrenOccupancy
            };


            rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices, roomCapacity);
            rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices);

            rates.Add(rate);

            //Precios Excepciones
            if (pricesException.Count > 0)
            {
                Rate rateException = new Rate();
                rateException.CurrencyCode = RatesHelpers.Currency;
                rateException.HasPriceException = true;
                rateException.StartDate = startDate.Date.ToString("yyyyMMdd");
                rateException.EndDate = endDate.Date.ToString("yyyyMMdd");

                rateException.ApplyMon = vDayRate.ExceptionMap[0] == 'Y' ? true : false;
                rateException.ApplyTue = vDayRate.ExceptionMap[1] == 'Y' ? true : false;
                rateException.ApplyWed = vDayRate.ExceptionMap[2] == 'Y' ? true : false;
                rateException.ApplyThu = vDayRate.ExceptionMap[3] == 'Y' ? true : false;
                rateException.ApplyFri = vDayRate.ExceptionMap[4] == 'Y' ? true : false;
                rateException.ApplySat = vDayRate.ExceptionMap[5] == 'Y' ? true : false;
                rateException.ApplySun = vDayRate.ExceptionMap[6] == 'Y' ? true : false;

                rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException, roomCapacity);

                if (rateException.BaseGuestAmounts.Count() > 0
                    && (rateException.ApplyMon ||
                    rateException.ApplyTue ||
                    rateException.ApplyWed ||
                    rateException.ApplyThu ||
                    rateException.ApplyFri ||
                    rateException.ApplySat ||
                    rateException.ApplySun)) { rates.Add(rateException); }

            }

            rateAmountMessage.statusApplicationControl = statusApplicationControl;
            rateAmountMessage.Rates = rates;

            return rateAmountMessage;
        }

        #endregion

        #region Delete
        //General Tarifa Normal
        public static RateAmountMessage CreateDeleteRateAmountMessage(spGetCurrentRatesByHotel_Result4 currentRate, vDayRates vDayRate)
        {

            if (DateTime.Now.Date > vDayRate.PromoEndDateBookingWindow && DateTime.Now.Date > vDayRate.EndDate) return null;

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            rateAmountMessage.statusApplicationControl = new StatusApplicationControl { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode ?? "" };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if(DateTime.Now.Date > vDayRate.PromoEndDateBookingWindow && vDayRate.EndDate.Date > DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if(vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            //var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;

            Rate rate = new Rate()
            {
                StartDate = starDate.ToString("yyyyMMdd"),
                EndDate = vDayRate.EndDate.ToString("yyyyMMdd")                           
            };

            rates.Add(rate);

            rateAmountMessage.Rates = rates;


            return rateAmountMessage;
        }

        //General Tarifa Promocion
        public static RateAmountMessage CreateDeleteRateAmountMessage(spGetCurrentRatesByHotel_Result4 currentRate, vDayRatesExceptions vDayRate)
        {
            if (DateTime.Now.Date > vDayRate.PromoEndDateBookingWindow && DateTime.Now.Date > vDayRate.EndDate) return null;

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            rateAmountMessage.statusApplicationControl = new StatusApplicationControl { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode ?? "" };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (DateTime.Now.Date > vDayRate.PromoEndDateBookingWindow && vDayRate.EndDate.Date > DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }

            //var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;

            Rate rate = new Rate()
            {
                StartDate = starDate.ToString("yyyyMMdd"),
                EndDate = vDayRate.EndDate.ToString("yyyyMMdd")
            };

            rates.Add(rate);

            rateAmountMessage.Rates = rates;


            return rateAmountMessage;
        }

        //General Tarifas EndPoint 
        //Aqui llegan las fechas que se escogieron
        public static RateAmountMessage CreateDeleteRateAmountMessage(string rateplanId, string roomCode, DateTime? startDate, DateTime? endDate)
        {

            RateAmountMessage rateAmountMessage = new RateAmountMessage();

            rateAmountMessage.statusApplicationControl = new StatusApplicationControl { RatePlanCode = rateplanId, InvTypeCode = roomCode };

            List<Rate> rates = new List<Rate>();


            Rate rate = new Rate()
            {
                StartDate = startDate.Value.ToString("yyyyMMdd"),
                EndDate = endDate.Value.ToString("yyyyMMdd")
            };

            rates.Add(rate);

            rateAmountMessage.Rates = rates;


            return rateAmountMessage;
        }


        //Por Tarifa Normal
        public static RateAmountMessage CreateDeleteRateAmountMessage(vDayRates vDayRate)
        {
            if (DateTime.Now.Date > vDayRate.PromoEndDateBookingWindow && DateTime.Now.Date > vDayRate.EndDate) return null;

            RateAmountMessage rateAmountMessage = new RateAmountMessage();
            var room = RoomHelper.GetRoom(vDayRate.RoomId);

            rateAmountMessage.statusApplicationControl = new StatusApplicationControl { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = room.Code ?? "" };

            List<Rate> rates = new List<Rate>();

            DateTime starDate = vDayRate.StartDate.Date;

            if (DateTime.Now.Date > vDayRate.PromoEndDateBookingWindow && vDayRate.EndDate.Date > DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date > vDayRate.EndDate.Date && vDayRate.EndDate.Date >= DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }
            else if (vDayRate.StartDate.Date < DateTime.Now.Date)
            {
                starDate = DateTime.Now.Date;
            }


            //var starDate = vDayRate.StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : vDayRate.StartDate.Date;

            Rate rate = new Rate()
            {
                StartDate = starDate.ToString("yyyyMMdd"),
                EndDate = vDayRate.EndDate.ToString("yyyyMMdd")
            };

            rates.Add(rate);

            rateAmountMessage.Rates = rates;


            return rateAmountMessage;

        }

        #endregion

    }
}
