using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Enum;
using APIServices.Conflux.Models.Rates;
using APIServices.Conflux.OTA.Models.Rates;
using System.Globalization;

namespace APIServices.Conflux.Helpers.Rates
{
    public static partial class RatesHelpers
    {
        private static bool? PlusTax { get; set; }
        public static decimal? Tax { get; set; }
        public static string Currency { get; set; }
        public static int HotelId { get; set; }
        public static void Init(int hotelId, bool? plusTax, decimal? tax,string currency)
        {
            HotelId = hotelId;
            PlusTax = plusTax;
            Tax = tax;
            Currency = currency;
        }

        #region vDayRate
        public static List<vDayRates> GetVDayRate(int rateId, DateTime startDate, DateTime endDate)
        {
            List<vDayRates> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRates.Where(
                    dr => dr.RateId == rateId
                    && dr.StartDate >= startDate
                    && dr.EndDate <= endDate
                    && dr.EndDate >= startDate
                    && dr.Language == 1)
                    .AsNoTracking()
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRatesExceptions> GetVDayRateException(int rateId, DateTime startDate, DateTime endDate)
        {
            List<vDayRatesExceptions> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRatesExceptions.Where(
                    dr => dr.RateId == rateId
                    && dr.StartDate >= startDate
                    && dr.EndDate <= endDate
                    && dr.EndDate >= startDate
                    && dr.Language == 1)
                    .AsNoTracking()
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRates> GetVDayRate(spGetCurrentRatesByHotel_Result4 rate)
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
                    && dr.EndDate >= dr.StartDate
                    && dr.Language == 1)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRates> GetVDayRate(spGetCurrentRatesByHotel_Result4 rate, bool deleted)
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
                    && dr.EndDate >= dr.StartDate
                    && dr.Language == 1
                    && dr.DeletedRatePlan == deleted)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRatesExceptions> GetVDayRateException(spGetCurrentRatesByHotel_Result4 rate)
        {
            List<vDayRatesExceptions> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRatesExceptions.Where(
                    dr => dr.RateId == rate.RateId
                    && dr.RoomId == rate.RoomHotelId
                    && dr.HotelId == rate.HotelId
                    && dr.StartDate >= rate.StartDate
                    && dr.EndDate <= rate.EndDate
                    && dr.EndDate >= dr.StartDate
                    && dr.Language == 1)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRatesExceptions> GetVDayRateException(spGetCurrentRatesByHotel_Result4 rate,bool deleted)
        {
            List<vDayRatesExceptions> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRatesExceptions.Where(
                    dr => dr.RateId == rate.RateId
                    && dr.RoomId == rate.RoomHotelId
                    && dr.HotelId == rate.HotelId
                    && dr.StartDate >= rate.StartDate
                    && dr.EndDate <= rate.EndDate
                    && dr.EndDate >= dr.StartDate
                    && dr.Language == 1
                    && dr.DeletedRatePlan == deleted)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }


        public static List<vDayRates> GetVDayRate(int hotelId, string ratePlan)
        {
            List<vDayRates> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRates.Where(                    
                    dr => dr.HotelId == hotelId
                    && dr.EndDate >= DbFunctions.TruncateTime(DateTime.Now)
                    && dr.Language == 1
                    && dr.IsPromotion == false
                    && (dr.RatePlanId == ratePlan
                        || dr.ParentRatePlanId == ratePlan)
                    && dr.DeletedRatePlan == false)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;

        }

        public static List<vDayRatesExceptions> GetVDayRateException(int hotelId, string ratePlan)
        {
            List<vDayRatesExceptions> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRatesExceptions.Where(
                    dr => dr.HotelId == hotelId
                    && dr.EndDate >= DbFunctions.TruncateTime(DateTime.Now)
                    && dr.Language == 1
                    && dr.IsPromotion == false
                    && (dr.RatePlanId == ratePlan
                        || dr.ParentRatePlanId == ratePlan)
                    && dr.DeletedRatePlan == false)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRates> GetVDayRatePromotion(int hotelId, string ratePlan)
        {
            List<vDayRates> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRates.Where(
                    dr => dr.HotelId == hotelId
                    && dr.EndDate >= DbFunctions.TruncateTime(DateTime.Now)
                    && dr.Language == 1
                    && dr.IsPromotion == true
                    && dr.RatePlanId.Contains(ratePlan)
                    && dr.DeletedRatePlan == false)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }

        public static List<vDayRatesExceptions> GetVDayRatePromotionException(int hotelId, string ratePlan)
        {
            List<vDayRatesExceptions> vDayRate = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                vDayRate = dbContext.vDayRatesExceptions.Where(
                    dr => dr.HotelId == hotelId
                    && dr.EndDate >= DbFunctions.TruncateTime(DateTime.Now)
                    && dr.Language == 1
                    && dr.IsPromotion == true
                    && dr.RatePlanId.Contains(ratePlan)
                    && dr.DeletedRatePlan == false)
                    .OrderBy(vdr => vdr.StartDate)
                    .ToList();
            }

            return vDayRate;
        }


        #endregion

        public static List<spGetPricesByRate_Result> GetPrices(int? rateId)
        {
            List<spGetPricesByRate_Result> prices = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                prices = dbContext.spGetPricesByRate(rateId).ToList<spGetPricesByRate_Result>();
            }

            return prices;
        }

        public static List<spGetPricesByRateException_Result> GetPricesException(int? rateId)
        {
            List<spGetPricesByRateException_Result> prices = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                prices = dbContext.spGetPricesByRateException(rateId).ToList<spGetPricesByRateException_Result>();
            }

            return prices;
        }

        public static List<spGetPricesByRatePromotion_Result> GetPricesPromotion(int? rateId)
        {
            List<spGetPricesByRatePromotion_Result> prices = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                prices = dbContext.spGetPricesByRatePromotion(rateId).ToList<spGetPricesByRatePromotion_Result>();
            }

            return prices;
        }

        public static List<spGetPricesByRatePromotionException_Result> GetPricesPromotionException(int? rateId)
        {
            List<spGetPricesByRatePromotionException_Result> prices = null;

            using (OzHotelesEntities dbContext = new OzHotelesEntities())
            {
                prices = dbContext.spGetPricesByRatePromotionException(rateId).ToList<spGetPricesByRatePromotionException_Result>();
            }

            return prices;
        }

        #region Tarifas Habitacion
        public static List<BaseGuestAmount> UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRates vDayRate, List<spGetPricesByRate_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            List<BaseGuestAmount> updatedPrices = null;

            updatedPrices = BaseGuestAmountApplyingTaxes(prices, currentRate);

            if (vDayRate.DayDiscount > 0 && vDayRate.Discount > 0)
            {

                switch (vDayRate.DiscountLevel)
                {
                    case 0:
                        //Solo DayDiscount

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {
                                price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                            }
                        }

                        break;
                    case 1:

                        //Sum of All Discounts

                        var allDiscount = vDayRate.DayDiscount + vDayRate.Discount;

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {

                                price.AmountBeforeTax = AllDiscounts(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = AllDiscounts(price.AmountAfterTax, vDayRate.DayDiscount);

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
                                price.AmountBeforeTax = AdditionalDiscount(price.AmountBeforeTax, vDayRate.DayDiscount, vDayRate.Discount);
                                price.AmountAfterTax = AdditionalDiscount(price.AmountAfterTax, vDayRate.DayDiscount, vDayRate.Discount);
                            }
                        }

                        break;
                }
            }
            else if (vDayRate.Discount > 0 && vDayRate.DayDiscount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.Discount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.Discount);
                    }
                }
            }
            else if (vDayRate.DayDiscount > 0 && vDayRate.Discount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                    }
                }
            }
            return updatedPrices;

        }

        public static List<BaseGuestAmount> UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRates vDayRate, List<spGetPricesByRateException_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            List<BaseGuestAmount> updatedPrices = null;

            updatedPrices = BaseGuestAmountApplyingTaxes(prices, currentRate);

            if (vDayRate.Discount > 0 && vDayRate.DayDiscount > 0)
            {

                switch (vDayRate.DiscountLevel)
                {
                    case 0:
                        //Solo DayDiscount

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {
                                price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                            }
                        }

                        break;
                    case 1:

                        //Sum of All Discounts

                        var allDiscount = vDayRate.DayDiscount + vDayRate.Discount;

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {

                                price.AmountBeforeTax = AllDiscounts(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = AllDiscounts(price.AmountAfterTax, vDayRate.DayDiscount);

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
                                price.AmountBeforeTax = AdditionalDiscount(price.AmountBeforeTax, vDayRate.DayDiscount, vDayRate.Discount);
                                price.AmountAfterTax = AdditionalDiscount(price.AmountAfterTax, vDayRate.DayDiscount, vDayRate.Discount);
                            }
                        }

                        break;
                }
            }
            else if (vDayRate.Discount > 0 && vDayRate.DayDiscount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.Discount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.Discount);
                    }
                }
            }
            else if (vDayRate.DayDiscount > 0 && vDayRate.Discount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                    }
                }
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
                    decimal amount = 0M;
                    switch (PlusTax)
                    {
                        case true:
                            amount = decimal.Round((decimal)price.Price, 2, MidpointRounding.AwayFromZero);
                            break;
                        case false:
                            amount = decimal.Round((decimal)(price.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);
                            break;
                    }

                    AdditionalGuestAmount additionalGuestAmount = new AdditionalGuestAmount();
                    additionalGuestAmount.Amount = decimal.Round(amount, MidpointRounding.AwayFromZero);
                    additionalGuestAmount.AgeQualifyingCode = GetAgeQualifyingCodeExtras(price.PersonType);

                    updatedPrices.Add(additionalGuestAmount);
                }
            }

            return updatedPrices;

        }

        public static List<BaseGuestAmount> BaseGuestAmountApplyingTaxes(List<spGetPricesByRate_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            int maxAdults = Convert.ToInt32(currentRate.MaxAdults);
            int maxChildren = Convert.ToInt32(currentRate.MaxChildren);

            List<BaseGuestAmount> baseGuestAmounts = new List<BaseGuestAmount>();

            switch (PlusTax)
            {
                case true:

                    //Adultos

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);
                        var amountBeforeTaxChild = decimal.Round((decimal)(priceChild.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
                        var amountBeforeTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                        {
                            AmountBeforeTax = amountBeforeTaxChild,
                            AmountAfterTax = priceChild.Price,
                            NumberOfGuests = priceChild.Quantity.ToString(),
                            AgeQualifyingCode = priceChild.PersonType
                        };

                        BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                        {
                            AmountBeforeTax = amountBeforeTaxTeeneger,
                            AmountAfterTax = priceTeeneger.Price,
                            NumberOfGuests = priceTeeneger.Quantity.ToString(),
                            AgeQualifyingCode = priceTeeneger.PersonType
                        };

                        baseGuestAmounts.Add(baseGuestAmountChild);
                        baseGuestAmounts.Add(baseGuestAmountTeeneger);
                    }


                    //foreach (var price in prices)
                    //{
                    //    if (price.PersonType != (int)PersonTypeEnum.ExtraAdult
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraChild
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraTeeneger)
                    //    {

                    //        var amountBeforeTax = decimal.Round((decimal)(price.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                    //        BaseGuestAmount baseGuestAmount = new BaseGuestAmount()
                    //        {
                    //            AmountBeforeTax = amountBeforeTax,
                    //            AmountAfterTax = price.Price,
                    //            NumberOfGuests = price.Quantity.ToString(),
                    //            AgeQualifyingCode = price.PersonType
                    //        };

                    //        baseGuestAmounts.Add(baseGuestAmount);
                    //    }
                    //}

                    break;
                case false:

                    //Adultos

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);
                        var amountAfterTaxChild = decimal.Round((decimal)(priceChild.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
                        var amountAfterTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                        {
                            AmountBeforeTax = priceChild.Price,
                            AmountAfterTax = amountAfterTaxChild,
                            NumberOfGuests = priceChild.Quantity.ToString(),
                            AgeQualifyingCode = priceChild.PersonType
                        };

                        BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                        {
                            AmountBeforeTax = priceTeeneger.Price,
                            AmountAfterTax = amountAfterTaxTeeneger,
                            NumberOfGuests = priceTeeneger.Quantity.ToString(),
                            AgeQualifyingCode = priceTeeneger.PersonType
                        };

                        baseGuestAmounts.Add(baseGuestAmountChild);
                        baseGuestAmounts.Add(baseGuestAmountTeeneger);
                    }

                    //foreach (var price in prices)
                    //{
                    //    if (price.PersonType != (int)PersonTypeEnum.ExtraAdult
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraChild
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraTeeneger)
                    //    {

                    //        var amountAfterTax = decimal.Round((decimal)(price.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                    //        BaseGuestAmount baseGuestAmount = new BaseGuestAmount()
                    //        {
                    //            AmountBeforeTax = price.Price,
                    //            AmountAfterTax = amountAfterTax,
                    //            NumberOfGuests = price.Quantity.ToString(),
                    //            AgeQualifyingCode = price.PersonType
                    //        };

                    //        baseGuestAmounts.Add(baseGuestAmount);
                    //    }
                    //}

                    break;
            }

            return baseGuestAmounts;
        }

        public static List<BaseGuestAmount> BaseGuestAmountApplyingTaxes(List<spGetPricesByRateException_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            int maxAdults = Convert.ToInt32(currentRate.MaxAdults);
            int maxChildren = Convert.ToInt32(currentRate.MaxChildren);

            List<BaseGuestAmount> baseGuestAmounts = new List<BaseGuestAmount>();

            switch (PlusTax)
            {
                case true:

                    //Adultos

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

                        if (price.Price > 0)
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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);

                        if (priceChild.Price > 0)
                        {
                            var amountBeforeTaxChild = decimal.Round((decimal)(priceChild.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                            BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                            {
                                AmountBeforeTax = amountBeforeTaxChild,
                                AmountAfterTax = priceChild.Price,
                                NumberOfGuests = priceChild.Quantity.ToString(),
                                AgeQualifyingCode = priceChild.PersonType
                            };

                            baseGuestAmounts.Add(baseGuestAmountChild);
                        }

                        if (priceTeeneger.Price > 0)
                        {
                            var amountBeforeTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                            BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                            {
                                AmountBeforeTax = amountBeforeTaxTeeneger,
                                AmountAfterTax = priceTeeneger.Price,
                                NumberOfGuests = priceTeeneger.Quantity.ToString(),
                                AgeQualifyingCode = priceTeeneger.PersonType
                            };

                            baseGuestAmounts.Add(baseGuestAmountTeeneger);

                        }
                    }


                    //foreach (var price in prices)
                    //{
                    //    if (price.PersonType != (int)PersonTypeEnum.ExtraAdult
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraChild
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraTeeneger)
                    //    {

                    //        var amountBeforeTax = decimal.Round((decimal)(price.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                    //        BaseGuestAmount baseGuestAmount = new BaseGuestAmount()
                    //        {
                    //            AmountBeforeTax = amountBeforeTax,
                    //            AmountAfterTax = price.Price,
                    //            NumberOfGuests = price.Quantity.ToString(),
                    //            AgeQualifyingCode = price.PersonType
                    //        };

                    //        baseGuestAmounts.Add(baseGuestAmount);
                    //    }
                    //}

                    break;
                case false:

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

                        if (price.Price > 0)
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

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
 
                        if (priceChild.Price > 0)
                        {
                            var amountAfterTaxChild = decimal.Round((decimal)(priceChild.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);
                            BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                            {
                                AmountBeforeTax = priceChild.Price,
                                AmountAfterTax = amountAfterTaxChild,
                                NumberOfGuests = priceChild.Quantity.ToString(),
                                AgeQualifyingCode = priceChild.PersonType
                            };

                            baseGuestAmounts.Add(baseGuestAmountChild);
                        }

                        if (priceTeeneger.Price > 0)
                        {
                            var amountAfterTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);
                            BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                            {
                                AmountBeforeTax = priceTeeneger.Price,
                                AmountAfterTax = amountAfterTaxTeeneger,
                                NumberOfGuests = priceTeeneger.Quantity.ToString(),
                                AgeQualifyingCode = priceTeeneger.PersonType
                            };


                            baseGuestAmounts.Add(baseGuestAmountTeeneger);
                        }
                    }


                    //foreach (var price in prices)
                    //{
                    //    if (price.PersonType != (int)PersonTypeEnum.ExtraAdult
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraChild
                    //        && price.PersonType != (int)PersonTypeEnum.ExtraTeeneger)
                    //    {

                    //        var amountAfterTax = decimal.Round((decimal)(price.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                    //        BaseGuestAmount baseGuestAmount = new BaseGuestAmount()
                    //        {
                    //            AmountBeforeTax = price.Price,
                    //            AmountAfterTax = amountAfterTax,
                    //            NumberOfGuests = price.Quantity.ToString(),
                    //            AgeQualifyingCode = price.PersonType
                    //        };

                    //        baseGuestAmounts.Add(baseGuestAmount);
                    //    }
                    //}

                    break;
            }

            return baseGuestAmounts;
        }
        #endregion

        #region Tarifas Promociones

        public static List<BaseGuestAmount> UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRatesExceptions vDayRate, List<spGetPricesByRatePromotion_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            List<BaseGuestAmount> updatedPrices = null;

            updatedPrices = BaseGuestAmountApplyingTaxes(prices, currentRate);

            if (vDayRate.Discount > 0 && vDayRate.DayDiscount > 0)
            {
                switch (vDayRate.DiscountLevel)
                {
                    case 0:
                        //Solo DayDiscount

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {
                                price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                            }
                        }

                        break;
                    case 1:

                        //Sum of All Discounts

                        var allDiscount = vDayRate.DayDiscount + vDayRate.Discount;

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {

                                price.AmountBeforeTax = AllDiscounts(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = AllDiscounts(price.AmountAfterTax, vDayRate.DayDiscount);

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
                                price.AmountBeforeTax = AdditionalDiscount(price.AmountBeforeTax, vDayRate.DayDiscount, vDayRate.Discount);
                                price.AmountAfterTax = AdditionalDiscount(price.AmountAfterTax, vDayRate.DayDiscount, vDayRate.Discount);
                            }
                        }

                        break;
                }
            }
            else if(vDayRate.Discount > 0 && vDayRate.DayDiscount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.Discount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.Discount);
                    }
                }
            }
            else if(vDayRate.DayDiscount > 0 && vDayRate.Discount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                    }
                }
            }

            return updatedPrices;

        }

        public static List<BaseGuestAmount> BaseGuestAmountApplyingTaxes(List<spGetPricesByRatePromotion_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            int maxAdults = Convert.ToInt32(currentRate.MaxAdults);
            int maxChildren = Convert.ToInt32(currentRate.MaxChildren);

            List<BaseGuestAmount> baseGuestAmounts = new List<BaseGuestAmount>();

            switch (PlusTax)
            {
                case true:

                    //Adultos

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);
                        var amountBeforeTaxChild = decimal.Round((decimal)(priceChild.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
                        var amountBeforeTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                        {
                            AmountBeforeTax = amountBeforeTaxChild,
                            AmountAfterTax = priceChild.Price,
                            NumberOfGuests = priceChild.Quantity.ToString(),
                            AgeQualifyingCode = priceChild.PersonType
                        };

                        BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                        {
                            AmountBeforeTax = amountBeforeTaxTeeneger,
                            AmountAfterTax = priceTeeneger.Price,
                            NumberOfGuests = priceTeeneger.Quantity.ToString(),
                            AgeQualifyingCode = priceTeeneger.PersonType
                        };

                        baseGuestAmounts.Add(baseGuestAmountChild);
                        baseGuestAmounts.Add(baseGuestAmountTeeneger);
                    }




                    break;
                case false:

                    //Adultos

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);
                        var amountAfterTaxChild = decimal.Round((decimal)(priceChild.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
                        var amountAfterTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                        {
                            AmountBeforeTax = priceChild.Price,
                            AmountAfterTax = amountAfterTaxChild,
                            NumberOfGuests = priceChild.Quantity.ToString(),
                            AgeQualifyingCode = priceChild.PersonType
                        };

                        BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                        {
                            AmountBeforeTax = priceTeeneger.Price,
                            AmountAfterTax = amountAfterTaxTeeneger,
                            NumberOfGuests = priceTeeneger.Quantity.ToString(),
                            AgeQualifyingCode = priceTeeneger.PersonType
                        };

                        baseGuestAmounts.Add(baseGuestAmountChild);
                        baseGuestAmounts.Add(baseGuestAmountTeeneger);
                    }


                    break;
            }

            return baseGuestAmounts;
        }

        public static List<BaseGuestAmount> UpdateBaseGuestAmountPricesWithTaxesAndDiscounts(vDayRatesExceptions vDayRate, List<spGetPricesByRatePromotionException_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            List<BaseGuestAmount> updatedPrices = null;

            updatedPrices = BaseGuestAmountApplyingTaxes(prices, currentRate);

            if (vDayRate.Discount > 0 && vDayRate.DayDiscount > 0)
            {
                switch (vDayRate.DiscountLevel)
                {
                    case 0:
                        //Solo DayDiscount

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {
                                price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                            }
                        }

                        break;
                    case 1:

                        //Sum of All Discounts

                        var allDiscount = vDayRate.DayDiscount + vDayRate.Discount;

                        foreach (var price in updatedPrices)
                        {
                            if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                                && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                            {

                                price.AmountBeforeTax = AllDiscounts(price.AmountBeforeTax, vDayRate.DayDiscount);
                                price.AmountAfterTax = AllDiscounts(price.AmountAfterTax, vDayRate.DayDiscount);

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
                                price.AmountBeforeTax = AdditionalDiscount(price.AmountBeforeTax, vDayRate.DayDiscount, vDayRate.Discount);
                                price.AmountAfterTax = AdditionalDiscount(price.AmountAfterTax, vDayRate.DayDiscount, vDayRate.Discount);
                            }
                        }

                        break;
                }
            }
            else if (vDayRate.Discount > 0 && vDayRate.DayDiscount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.Discount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.Discount);
                    }
                }
            }
            else if (vDayRate.DayDiscount > 0 && vDayRate.Discount == 0)
            {
                foreach (var price in updatedPrices)
                {
                    if (price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraAdult
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraChild
                        && price.AgeQualifyingCode != (int)PersonTypeEnum.ExtraTeeneger)
                    {
                        price.AmountBeforeTax = PriorityRateDiscount(price.AmountBeforeTax, vDayRate.DayDiscount);
                        price.AmountAfterTax = PriorityRateDiscount(price.AmountAfterTax, vDayRate.DayDiscount);
                    }
                }
            }

            return updatedPrices;

        }

        public static List<BaseGuestAmount> BaseGuestAmountApplyingTaxes(List<spGetPricesByRatePromotionException_Result> prices, spGetCurrentRatesByHotel_Result4 currentRate)
        {
            int maxAdults = Convert.ToInt32(currentRate.MaxAdults);
            int maxChildren = Convert.ToInt32(currentRate.MaxChildren);

            List<BaseGuestAmount> baseGuestAmounts = new List<BaseGuestAmount>();

            switch (PlusTax)
            {
                case true:

                    //Adultos

                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);
                        var amountBeforeTaxChild = decimal.Round((decimal)(priceChild.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
                        var amountBeforeTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price / (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                        {
                            AmountBeforeTax = amountBeforeTaxChild,
                            AmountAfterTax = priceChild.Price,
                            NumberOfGuests = priceChild.Quantity.ToString(),
                            AgeQualifyingCode = priceChild.PersonType
                        };

                        BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                        {
                            AmountBeforeTax = amountBeforeTaxTeeneger,
                            AmountAfterTax = priceTeeneger.Price,
                            NumberOfGuests = priceTeeneger.Quantity.ToString(),
                            AgeQualifyingCode = priceTeeneger.PersonType
                        };

                        baseGuestAmounts.Add(baseGuestAmountChild);
                        baseGuestAmounts.Add(baseGuestAmountTeeneger);
                    }


                    break;
                case false:

                    //Adults
                    for (var i = 0; i < maxAdults; i++)
                    {
                        var price = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Adult);

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

                    //Ninios

                    for (var i = 0; i < maxChildren; i++)
                    {
                        var priceChild = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Child);
                        var amountAfterTaxChild = decimal.Round((decimal)(priceChild.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        var priceTeeneger = prices.First(p => p.Quantity == (i + 1) && p.PersonType == (int)PersonTypeEnum.Teeneger);
                        var amountAfterTaxTeeneger = decimal.Round((decimal)(priceTeeneger.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);

                        BaseGuestAmount baseGuestAmountChild = new BaseGuestAmount()
                        {
                            AmountBeforeTax = priceChild.Price,
                            AmountAfterTax = amountAfterTaxChild,
                            NumberOfGuests = priceChild.Quantity.ToString(),
                            AgeQualifyingCode = priceChild.PersonType
                        };

                        BaseGuestAmount baseGuestAmountTeeneger = new BaseGuestAmount()
                        {
                            AmountBeforeTax = priceTeeneger.Price,
                            AmountAfterTax = amountAfterTaxTeeneger,
                            NumberOfGuests = priceTeeneger.Quantity.ToString(),
                            AgeQualifyingCode = priceTeeneger.PersonType
                        };

                        baseGuestAmounts.Add(baseGuestAmountChild);
                        baseGuestAmounts.Add(baseGuestAmountTeeneger);
                    }


                    break;
            }

            return baseGuestAmounts;
        }

        public static List<AdditionalGuestAmount> UpdateAdditionalGuestAmountPrices(List<spGetPricesByRatePromotion_Result> prices)
        {
            List<AdditionalGuestAmount> updatedPrices = new List<AdditionalGuestAmount>();

            foreach (var price in prices)
            {
                if (price.PersonType == (int)PersonTypeEnum.ExtraAdult
                    || price.PersonType == (int)PersonTypeEnum.ExtraChild
                    || price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    decimal amount = 0M;
                    switch (PlusTax)
                    {
                        case true:
                            amount = decimal.Round((decimal)price.Price, 2, MidpointRounding.AwayFromZero);
                            break;
                        case false:
                            amount = decimal.Round((decimal)(price.Price * (1 + (Tax / 100))), 2, MidpointRounding.AwayFromZero);
                            break;
                    }

                    AdditionalGuestAmount additionalGuestAmount = new AdditionalGuestAmount();
                    additionalGuestAmount.Amount = decimal.Round(amount, MidpointRounding.AwayFromZero);
                    additionalGuestAmount.AgeQualifyingCode = GetAgeQualifyingCodeExtras(price.PersonType);

                    updatedPrices.Add(additionalGuestAmount);
                }
            }

            return updatedPrices;

        }

        #endregion

        public static List<Promo> GetActiveDatesPromo(DateTime startDate, DateTime endDate, string promoDays, vDayRates vDayRate)
        {
            List<Promo> promosList = new List<Promo>();

            DateTime currentDate = startDate;
            DateTime activeStartDate = DateTime.MinValue;
            DateTime inactiveStartDate = DateTime.MinValue;

            while (currentDate <= endDate)
            {
                DayOfWeek dayOfTheWeek = currentDate.DayOfWeek;

                if (promoDays[Convert.ToInt32(dayOfTheWeek)] == 'Y')
                {
                    if (activeStartDate == DateTime.MinValue) activeStartDate = currentDate;

                    if (inactiveStartDate != DateTime.MinValue)
                    {
                        Console.WriteLine("Rango inactivo: " + inactiveStartDate.ToString("yyyy-MM-dd") + " - " + currentDate.AddDays(-1).ToString("yyyy-MM-dd"));
                        inactiveStartDate = DateTime.MinValue;
                    }
                }
                else
                {
                    if (inactiveStartDate == DateTime.MinValue) inactiveStartDate = currentDate;

                    if (activeStartDate != DateTime.MinValue)
                    {
                        Console.WriteLine("Rango activo: " + activeStartDate.ToString("yyyy-MM-dd") + " - " + currentDate.AddDays(-1).ToString("yyyy-MM-dd"));

                        Promo promo = new Promo();
                        promo.DiscountLevel = vDayRate.DiscountLevel;
                        promo.Discount = vDayRate.Discount;
                        promo.DayDiscount = vDayRate.DayDiscount;
                        promo.StartDate = activeStartDate;
                        promo.EndDate = currentDate.AddDays(-1);

                        promosList.Add(promo);

                        activeStartDate = DateTime.MinValue;
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            if (activeStartDate != DateTime.MinValue)
            {
                Promo promo = new Promo();
                promo.DiscountLevel = vDayRate.DiscountLevel;
                promo.Discount = vDayRate.Discount;
                promo.DayDiscount = vDayRate.DayDiscount;
                promo.StartDate = activeStartDate;
                promo.EndDate = endDate;

                promosList.Add(promo);

                Console.WriteLine("Rango activo: " + activeStartDate.ToString("yyyy-MM-dd") + " - " + endDate.ToString("yyyy-MM-dd"));
            }

            return promosList;
        }

        public static List<Promo> GetNotOverlappedPromoDates(List<Promo> promos)
        {
            if (promos.Count == 0 || promos.Count == 1) return promos;

            int i = 0;

            while (i < promos.Count - 1)
            {
                //Overlap
                if (promos[i].StartDate <= promos[i + 1].EndDate && promos[i + 1].StartDate <= promos[i].EndDate)
                {
                    //Case A
                    if (promos[i + 1].StartDate >= promos[i].StartDate
                        && promos[i + 1].StartDate <= promos[i].EndDate
                        && promos[i + 1].EndDate > promos[i].StartDate
                        && promos[i + 1].EndDate > promos[i].EndDate)
                    {

                        //Primera Promo tiene un descuento Mayor
                        if (promos[i].Discount > promos[i + 1].Discount)
                        {
                            promos[i + 1].StartDate = promos[i].EndDate.AddDays(1);
                        }
                        //El segunto tiene mayor descuento
                        else if (promos[i + 1].Discount > promos[i].Discount)
                        {
                            promos[i].EndDate = promos[i + 1].StartDate.AddDays(-1);
                        }
                        //Tienen descuentos iguales
                        else
                        {

                        }

                    }
                    //Case B
                    else if (promos[i + 1].StartDate >= promos[i].StartDate
                             && promos[i + 1].StartDate <= promos[i].EndDate
                             && promos[i + 1].EndDate >= promos[i].StartDate
                             && promos[i + 1].EndDate <= promos[i].EndDate)
                    {
                        //Primera Promo tiene un descuento Mayor
                        if (promos[i].Discount > promos[i + 1].Discount)
                        {
                            promos.RemoveAt(i + 1);
                            i = i - 1;
                        }
                        //El segunto tiene mayor descuento
                        else if (promos[i + 1].Discount > promos[i].Discount)
                        {
                            DateTime auxEndDate = promos[i].EndDate;

                            promos[i].EndDate = promos[i + i].StartDate.AddDays(-1);

                            Promo promo = new Promo();
                            promo.StartDate = promos[i + 1].EndDate.AddDays(1);
                            promo.EndDate = auxEndDate;
                            promo.DiscountLevel = promos[i].DiscountLevel;
                            promo.Discount = promos[i].Discount;
                            promo.DayDiscount = promos[i].DayDiscount;

                            promos.Add(promo);

                            promos = promos.OrderBy(pr => pr.StartDate).ToList();

                        }
                        //Tienen descuentos iguales
                        else
                        {

                        }
                    }
                }

                i++;
            }

            return promos;
        }

        public static void GetRatesDatesIncludingPromos(DateTime rateStartDate, DateTime rateEndDate, List<Promo> promos)
        {
            List<Tuple<DateTime, DateTime>> ratesDates = new List<Tuple<DateTime, DateTime>>();

            DateTime activeStartDateRate = rateStartDate;
            bool endDateFound = false;

            for (int i = 0; i < promos.Count; i++)
            {
                //Sacar posible dia
                if (activeStartDateRate != promos[i].StartDate && activeStartDateRate < promos[i].StartDate)
                {
                    endDateFound = true;

                    //Sacamos EndDate de la tarifa
                    DateTime activeEndDateRate = promos[i].EndDate.AddDays(-1);
                    //Se agregar a la lista de fechas para la tarifa
                    Tuple<DateTime, DateTime> rateDate = new Tuple<DateTime, DateTime>(activeStartDateRate, activeEndDateRate);
                    ratesDates.Add(rateDate);

                    //Posible fecha para activeStartDate
                    activeStartDateRate = promos[i].EndDate.AddDays(1);
                }
                else
                {
                    activeStartDateRate = promos[i].EndDate.AddDays(1);
                }


            }
        }

        public static string ToDayOfWeek(string days)
        {
            char[] applyDays = new char[7];
            applyDays[0] = days[6];
            applyDays[1] = days[0];
            applyDays[2] = days[1];
            applyDays[3] = days[2];
            applyDays[4] = days[3];
            applyDays[5] = days[4];
            applyDays[6] = days[5];

            return new string(applyDays);
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

            return decimal.Round((decimal)(firstDiscountPrice - (firstDiscountPrice * (dayDiscount / 100))), 2, MidpointRounding.AwayFromZero);
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

        public static void CheckDatesCalendar(DateTime? startDate, DateTime? endDate, ref List<RateAmountMessage> rateAmountMessagesList)
        {

            foreach(var rateAmountMessageTemp in rateAmountMessagesList)
            {
                for(int i = 0; i < rateAmountMessageTemp.Rates.Count; i++)
                {


                    DateTime? rateStartDateTemp = DateTime.ParseExact(rateAmountMessageTemp.Rates[i].StartDate, "yyyyMMdd", CultureInfo.InvariantCulture); 
                    DateTime? rateEndDateTemp = DateTime.ParseExact(rateAmountMessageTemp.Rates[i].EndDate, "yyyyMMdd", CultureInfo.InvariantCulture);

                    if (startDate >= rateStartDateTemp && endDate <= rateEndDateTemp)
                    {
                        // La búsqueda está estrictamente dentro del rango de la tarifa incluyendo border

                        rateAmountMessageTemp.Rates[i].StartDate = startDate.Value.ToString("yyyyMMdd");
                        rateAmountMessageTemp.Rates[i].EndDate = endDate.Value.ToString("yyyyMMdd");
                    }
                    else if (startDate <= rateEndDateTemp && endDate >= rateStartDateTemp)
                    {
                        // Se traslapan los rangos: hay cruce entre la búsqueda y la tarifa

                        if(startDate < rateStartDateTemp &&  (endDate >= rateStartDateTemp && endDate <= rateEndDateTemp))
                        {
                            rateAmountMessageTemp.Rates[i].EndDate = endDate.Value.ToString("yyyyMMdd");
                        }
                        else if (endDate > rateEndDateTemp && (startDate >= rateStartDateTemp && startDate <= rateEndDateTemp))
                        {
                            rateAmountMessageTemp.Rates[i].StartDate = startDate.Value.ToString("yyyyMMdd");                            
                        }
                        else if(startDate < rateStartDateTemp && endDate > rateEndDateTemp)
                        {
                            //No se cambian las fechas, van las fechas de la tarifa
                        }

                    }


                }
            }

        }



    }
}
