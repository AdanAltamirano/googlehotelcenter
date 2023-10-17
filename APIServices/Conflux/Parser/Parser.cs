using System.Collections.Generic;
using System.Linq;

using APIServices.Models;
using APIServices.Conflux.OTA.Models.Rates;


namespace APIServices.Conflux.Parser
{
    public static class Parser
    {
        public static RateAmountMessages ToRateAmountMessages(List<spGetCurrentRatesByHotel_Result> currentRates, int companyId)
        {
            RateAmountMessages rateAmountMessages = new RateAmountMessages();

            rateAmountMessages.HotelCode = companyId;
            rateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            foreach (var currentRate in currentRates)
            {
                RateAmountMessage rateAmountMessage = new RateAmountMessage();
                
                StatusApplicationControl statusApplicationControl = new StatusApplicationControl() { RatePlanCode = currentRate.RatePlanId, InvTypeCode = currentRate.RoomCode };

                List<Rate> rates = new List<Rate>();

                Rate rate = new Rate();

                rate.StartDate = currentRate.StartDate.ToString("yyyyMMdd");//revisar el formato
                rate.EndDate = currentRate.EndDate.ToString("yyyyMMdd");
                rate.BaseGuestAmounts = new List<BaseGuestAmount>(); // Faltan precios

                rates.Add(rate);

                rateAmountMessage.statusApplicationControl = statusApplicationControl;
                rateAmountMessage.Rates = rates;

                rateAmountMessages.RateAmountMessagesList.Add(rateAmountMessage);
            }


            return rateAmountMessages;
        }
    }
}
