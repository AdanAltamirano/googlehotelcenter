using System.Collections.Generic;
using System.Linq;

using APIServices.Models;
using APIServices.Conflux.OTA.Models.Rates;
using APIServices.Conflux.Helpers;


namespace APIServices.Conflux.Parser
{
    public static class Parser
    {
        public static RateAmountMessages ToRateAmountMessages(List<spGetCurrentRatesByHotel_Result> currentRates, int companyId, bool? plusTax, decimal? tax)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            foreach (var currentRate in currentRates)
            {
                RateAmountMessage rateAmountMessage = new RateAmountMessage();
                
                StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = currentRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

                List<Rate> rates = new List<Rate>();

                List<vDayRates> vDayRate = RatesHelpers.GetVDayRate(currentRate);

                List<vDayRates> promos = vDayRate
                    .Where(vdr => vdr.IsPromotion == true)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();

                var startDate = currentRate.StartDate;
                var endDate = currentRate.EndDate;




                //Rate rate = new Rate();

                //rate.StartDate = currentRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                //rate.EndDate = currentRate.EndDate.ToString("yyyyMMdd");

                //RatesHelpers.Init(plusTax, tax);

                //var prices = RatesHelpers.GetPrices(currentRate.RateId);
                //rate.BaseGuestAmounts = RatesHelpers.UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(currentRate, prices);
                //rate.AdditionalGuestAmounts = RatesHelpers.UpdateAdditionalGuestAmountPrices(prices);

                //rates.Add(rate);

                rateAmountMessage.statusApplicationControl = statusApplicationControl;
                rateAmountMessage.Rates = rates;

                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
            }


            return rateAmountMessages;
        }
    }
}
