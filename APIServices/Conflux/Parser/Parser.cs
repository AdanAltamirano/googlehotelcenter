using System.Collections.Generic;
using System.Linq;
using APIServices.Models;
using APIServices.Conflux.Helpers;
using APIServices.Conflux.Models.Rates;
using APIServices.Conflux.OTA.Models.Rates;



namespace APIServices.Conflux.Parser
{
    public static class Parser
    {
        public static RateAmountMessages ToRateAmountMessages(List<spGetCurrentRatesByHotel_Result> currentRates, int companyId, bool? plusTax, decimal? tax)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            RatesHelpers.Init(plusTax, tax);

            foreach (var currentRate in currentRates)
            {
                var prices = RatesHelpers.GetPrices(currentRate.RateId);
                var pricesException = RatesHelpers.GetPricesException(currentRate.RateId);
                List<vDayRates> vDayRates = RatesHelpers.GetVDayRate(currentRate);

                foreach(var vDayRate in vDayRates)
                {
                    RateAmountMessage rateAmountMessage = new RateAmountMessage();

                    StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = vDayRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

                    List<Rate> rates = new List<Rate>();
                    
                    Rate rate = new Rate();
                    rate.StartDate = vDayRate.StartDate.ToString("yyyyMMdd");//revisar el formato
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


                    rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, prices);
                    rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices); // Ver si llevan impuestos y descuento los extra

                    rates.Add(rate);

                    //Precios Excepciones
                    if(pricesException.Count > 0 && !vDayRate.IsPromotion)
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

                        rateException.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRate, pricesException);

                        rates.Add(rateException);
                    }

                    rateAmountMessage.statusApplicationControl = statusApplicationControl;
                    rateAmountMessage.Rates = rates;

                    rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
                }


            }


            return rateAmountMessages;
        }
    }
}
