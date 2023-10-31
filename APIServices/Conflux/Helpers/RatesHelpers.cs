using System;
using System.Linq;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Enum;
using APIServices.Conflux.Models.Rates;
using APIServices.Conflux.OTA.Models.Rates;

namespace APIServices.Conflux.Helpers
{
    public static class RatesHelpers
    {
        private static bool? PlusTax { get; set; }
        private static decimal? Tax { get; set; }
        public static void Init(bool? plusTax, decimal? tax)
        {
            PlusTax = plusTax;
            Tax = tax;
        }
        public static List<vDayRates> GetVDayRate(spGetCurrentRatesByHotel_Result rate)
        {
            List<vDayRates> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRates.Where(
                    dr => dr.RateId == rate.RateId
                    && dr.RoomId == rate.RoomHotelId
                    && dr.HotelId == rate.HotelId
                    && dr.StartDate >= rate.StartDate
                    && dr.EndDate <= rate.EndDate
                    && dr.Language == 1)
                    .ToList();
            }

            return vDayRate;
        }
        public static List<spGetPricesByRate_Result> GetPrices(int? rateId)
        {
            List<spGetPricesByRate_Result> prices = null;

            using(OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                prices = dbContext.spGetPricesByRate(rateId).ToList<spGetPricesByRate_Result>();                
            }

            return prices;
        }

        public static List<BaseGuestAmount> UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(spGetCurrentRatesByHotel_Result rate , List<spGetPricesByRate_Result> prices)
        {
            List<vDayRates> vDayRate = null;
            List<BaseGuestAmount> updatedPrices = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRates.Where(
                    dr => dr.RateId == rate.RateId
                    && dr.RoomId == rate.RoomHotelId
                    && dr.HotelId == rate.HotelId
                    && dr.StartDate >= rate.StartDate
                    && dr.EndDate <= rate.EndDate                   
                    && dr.Language == 1)
                    .ToList();
            }

            updatedPrices = BaseGuestAmountApplyingTaxes(prices);

            switch (vDayRate[0].DiscountLevel)
            {
                case 0:
                    //Solo DayDiscount

                    foreach (var price in updatedPrices)
                    {
                        if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                            && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                            && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                        {
                            price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate[0].DayDiscount);
                            price.AmountAfterTax  = PriorityRateDiscount(price.AmountAfterTax, vDayRate[0].DayDiscount);
                        }
                    }

                    break;
                case 1:

                    //Sum of All Discounts

                    var allDiscount = vDayRate[0].DayDiscount + vDayRate[0].Discount;

                    foreach (var price in updatedPrices)
                    {
                        if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                            && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                            && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                        {

                            price.AmountBeforeTax = AllDiscounts(price.AmountBeforeTax, vDayRate[0].DayDiscount);
                            price.AmountAfterTax = AllDiscounts(price.AmountAfterTax, vDayRate[0].DayDiscount);

                        }

                    }

                    break;
                case 2:

                    //Additional Discount

                    foreach (var price in updatedPrices)
                    {

                        if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                            && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                            && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                        {
                            price.AmountBeforeTax = AdditionalDiscount(price.AmountBeforeTax, vDayRate[0].DayDiscount, vDayRate[0].Discount);
                            price.AmountAfterTax = AdditionalDiscount(price.AmountAfterTax, vDayRate[0].DayDiscount, vDayRate[0].Discount);
                        }
                    }

                    break;
            }

            return updatedPrices;

        }

        public static List<AdditionalGuestAmount> UpdateAdditionalGuestAmountPrices(List<spGetPricesByRate_Result> prices)
        {
            List<AdditionalGuestAmount> updatedPrices = new List<AdditionalGuestAmount>();

            foreach (var price in prices)
            {
                if (price.PersonType == (int)PersonTypeEnum.ExtraAdult
                    || price.PersonType == (int)PersonTypeEnum.ExtraChild
                    || price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {

                    AdditionalGuestAmount additionalGuestAmount = new AdditionalGuestAmount();
                    additionalGuestAmount.Amount = decimal.Round((decimal)price.Price, 2, MidpointRounding.AwayFromZero);
                    additionalGuestAmount.AgeQualifyingCode = GetAgeQualifyingCodeExtras(price.PersonType);

                    updatedPrices.Add(additionalGuestAmount);
                }
            }

            return updatedPrices;

        }

        public static List<BaseGuestAmount> BaseGuestAmountApplyingTaxes(List<spGetPricesByRate_Result> prices)
        {
            List<BaseGuestAmount> baseGuestAmounts = new List<BaseGuestAmount>();

            switch (PlusTax)
            {
                case true:

                    foreach (var price in prices)
                    {
                        if (price.PersonType != (int)PersonTypeEnum.ExtraAdult
                            && price.PersonType != (int)PersonTypeEnum.ExtraChild
                            && price.PersonType != (int)PersonTypeEnum.ExtraTeeneger)
                        {

                            var amountBeforeTax = decimal.Round((decimal)(price.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                            BaseGuestAmount baseGuestAmount = new BaseGuestAmount()
                            {
                                AmountBeforeTax = amountBeforeTax,
                                AmountAfterTax = price.Price,
                                NumberOfGuests = price.Quantity.ToString(),
                                AgeQualifyingCode = price.PersonType
                            };

                            baseGuestAmounts.Add(baseGuestAmount);
                        }
                    }

                    break;
                case false:

                    foreach (var price in prices)
                    {
                        if (price.PersonType != (int)PersonTypeEnum.ExtraAdult
                            && price.PersonType != (int)PersonTypeEnum.ExtraChild
                            && price.PersonType != (int)PersonTypeEnum.ExtraTeeneger)
                        {

                            var amountAfterTax = decimal.Round((decimal)(price.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                            BaseGuestAmount baseGuestAmount = new BaseGuestAmount()
                            {
                                AmountBeforeTax = price.Price,
                                AmountAfterTax = amountAfterTax,
                                NumberOfGuests = price.Quantity.ToString(),
                                AgeQualifyingCode = price.PersonType
                            };

                            baseGuestAmounts.Add(baseGuestAmount);
                        }
                    }

                    break;
            }

            return baseGuestAmounts;
        }

        private static decimal PriorityRateDiscount(decimal? price, decimal dayDiscount)
        {
            return decimal.Round((decimal)(price - (price * (dayDiscount / 100))), 2, MidpointRounding.AwayFromZero);
        }

        private static decimal AllDiscounts(decimal? price, decimal discount)
        {
            return decimal.Round((decimal)(price - (price * (discount / 100))), 2, MidpointRounding.AwayFromZero);

        }

        private static decimal AdditionalDiscount(decimal? price, decimal dayDiscount, decimal discount)
        {
            var firstDiscountPrice = price - (price * (discount / 100));

            return decimal.Round((decimal)(firstDiscountPrice - (firstDiscountPrice * (dayDiscount / 100))), 2 , MidpointRounding.AwayFromZero);          
        }

        private static string GetAgeQualifyingCodeExtras(int? personType)
        {
            string ageQualifyingCode = string.Empty;

            switch (personType)
            {
                case (int)PersonTypeEnum.ExtraAdult:
                    ageQualifyingCode = "10";
                    break;

                case (int)PersonTypeEnum.ExtraChild:
                    ageQualifyingCode = "8";
                    break;

                case (int)PersonTypeEnum.ExtraTeeneger:
                    ageQualifyingCode = "9";
                    break;
            }

            return ageQualifyingCode;

        }
    }
}
