using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Enum;
using APIServices.Conflux.OTA.Models.Rates;
using System;

namespace APIServices.Conflux.Helpers.Rates
{
    public static partial class RatesHelpers
    {
        #region Tarifas
        public static void UpdatePricesLinkedRoom(vLinkedRoomTypes linkedRoom, ref List<spGetPricesByRate_Result> prices)
        {
            foreach(var price in prices)
            {
                if(price.PersonType == (int)PersonTypeEnum.Adult || 
                    price.PersonType == (int)PersonTypeEnum.Child || 
                    price.PersonType == (int)PersonTypeEnum.Teeneger)
                {

                    if(price.Price > 0) price.Price = UpdatePriceByQuantityLinkedRoom(price.Quantity, price.Price, linkedRoom);

                }
                else if(price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRoom.ExtraAdultRatio, linkedRoom.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRoom.ExtraChildRatio, linkedRoom.ExtraChildOffset);
                }
            }

        }

        public static void UpdatePricesLinkedRoom(vLinkedRoomTypes linkedRoom, ref List<BaseGuestAmount> baseGuestAmounts, decimal? hotelTax = null)
        {
            foreach (var baseGuestAmount in baseGuestAmounts)
            {
                int personType = Convert.ToInt32(baseGuestAmount.AgeQualifyingCode);
                int quantity = Convert.ToInt32(baseGuestAmount.NumberOfGuests);

                if (baseGuestAmount.AmountAfterTax > 0) baseGuestAmount.AmountAfterTax = UpdatePriceByQuantityLinkedRoom(quantity, baseGuestAmount.AmountAfterTax, linkedRoom);
                if (baseGuestAmount.AmountBeforeTax > 0) 
                {

                    var taxes = (RatesHelpers.Tax / 100) + 1;

                    decimal amtBeforeTax = decimal.Round(Convert.ToDecimal(baseGuestAmount.AmountAfterTax / taxes),2);

                    baseGuestAmount.AmountBeforeTax = amtBeforeTax;
                }
            }
        }

        public static void UpdatePricesLinkedRoom(vLinkedRoomTypes linkedRoom, ref List<AdditionalGuestAmount> additionalGuestAmounts)
        {
            foreach (var additionalGuestAmount in additionalGuestAmounts)
            {
                int personType = Convert.ToInt32(additionalGuestAmount.AgeQualifyingCode);

                if (personType == 10)
                {
                    if (additionalGuestAmount.Amount > 0)
                    {
                        var tempAmount = UpdatePrice(additionalGuestAmount.Amount, linkedRoom.ExtraAdultRatio, linkedRoom.ExtraAdultOffset);
                        additionalGuestAmount.Amount = (decimal)tempAmount;
                    }
                }
                else if (personType == 8 || personType == 9)
                {
                    if (additionalGuestAmount.Amount > 0)
                    {
                        var tempAmount = UpdatePrice(additionalGuestAmount.Amount, linkedRoom.ExtraChildRatio, linkedRoom.ExtraChildOffset);
                        additionalGuestAmount.Amount = (decimal)tempAmount;
                    }
                }

            }
        }


        public static void UpdatePricesLinkedRatePlan(vLinkedRatePlans linkedRatePlan, ref List<spGetPricesByRate_Result> prices)
        {
            foreach (var price in prices)
            {
                if (price.PersonType == (int)PersonTypeEnum.Adult ||
                    price.PersonType == (int)PersonTypeEnum.Child ||
                    price.PersonType == (int)PersonTypeEnum.Teeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePriceByQuantityLinkedRatePlan(price.Quantity, price.Price, linkedRatePlan);

                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraAdultRatio, linkedRatePlan.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraChildRatio, linkedRatePlan.ExtraChildOffset);
                }
            }
        }

        public static void UpdatePricesLinkedRatePlan(vLinkedRatePlans linkedRatePlan, ref List<BaseGuestAmount> baseGuestAmounts, decimal? hotelTax = null)
        {
            foreach(var baseGuestAmount in baseGuestAmounts)
            {
                int personType = Convert.ToInt32(baseGuestAmount.AgeQualifyingCode);
                int quantity = Convert.ToInt32(baseGuestAmount.NumberOfGuests);

                if (baseGuestAmount.AmountAfterTax > 0) baseGuestAmount.AmountAfterTax = UpdatePriceByQuantityLinkedRatePlan(quantity, baseGuestAmount.AmountAfterTax, linkedRatePlan);
                if (baseGuestAmount.AmountBeforeTax > 0) 
                {
                    var taxes = (RatesHelpers.Tax / 100) + 1;

                    decimal amtBeforeTax = decimal.Round(Convert.ToDecimal(baseGuestAmount.AmountAfterTax / taxes), 2);

                    baseGuestAmount.AmountBeforeTax = amtBeforeTax;
                }
            }
        }

        public static void UpdatePricesLinkedRatePlan(vLinkedRatePlans linkedRatePlan, ref List<AdditionalGuestAmount> additionalGuestAmounts)
        {
            foreach (var additionalGuestAmount in additionalGuestAmounts)
            {
                int personType = Convert.ToInt32(additionalGuestAmount.AgeQualifyingCode);

                if(personType == 10)
                {
                    if (additionalGuestAmount.Amount > 0)
                    {
                        var tempAmount = UpdatePrice(additionalGuestAmount.Amount, linkedRatePlan.ExtraAdultRatio, linkedRatePlan.ExtraAdultOffset);
                        additionalGuestAmount.Amount = (decimal) tempAmount;
                    }
                }
                else if(personType == 8 || personType == 9)
                {
                    if (additionalGuestAmount.Amount > 0)
                    {
                        var tempAmount = UpdatePrice(additionalGuestAmount.Amount, linkedRatePlan.ExtraChildRatio, linkedRatePlan.ExtraChildOffset);
                        additionalGuestAmount.Amount = (decimal)tempAmount;
                    }
                }

            }
        }



        #endregion

        #region Tarifas Excepciones

        public static void UpdatePricesLinkedRoom(vLinkedRoomTypes linkedRoom, ref List<spGetPricesByRateException_Result> prices)
        {
            foreach (var price in prices)
            {
                if (price.PersonType == (int)PersonTypeEnum.Adult ||
                    price.PersonType == (int)PersonTypeEnum.Child ||
                    price.PersonType == (int)PersonTypeEnum.Teeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePriceByQuantityLinkedRoom(price.Quantity, price.Price, linkedRoom);

                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRoom.ExtraAdultRatio, linkedRoom.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRoom.ExtraChildRatio, linkedRoom.ExtraChildOffset);
                }
            }

        }

        public static void UpdatePricesLinkedRatePlan(vLinkedRatePlans linkedRatePlan, ref List<spGetPricesByRateException_Result> prices)
        {
            foreach (var price in prices)
            {
                if (price.PersonType == (int)PersonTypeEnum.Adult ||
                    price.PersonType == (int)PersonTypeEnum.Child ||
                    price.PersonType == (int)PersonTypeEnum.Teeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePriceByQuantityLinkedRatePlan(price.Quantity, price.Price, linkedRatePlan);

                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraAdultRatio, linkedRatePlan.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    if (price.Price > 0) price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraChildRatio, linkedRatePlan.ExtraChildOffset);
                }
            }
        }



        #endregion

        #region Calcular Precios por Cantidad

        private static decimal? UpdatePriceByQuantityLinkedRoom(int? quantity ,decimal? price, vLinkedRoomTypes linkedRoom, int hotelTax = 0)
        {
            decimal? updatedPrice = 0;

            if (quantity == 1)
            {
                updatedPrice = UpdatePrice(price, linkedRoom.OnePersonRatio, linkedRoom.OnePersonOffset);
            }
            else if (quantity == 2)
            {
                updatedPrice = UpdatePrice(price, linkedRoom.TwoPersonRatio, linkedRoom.TwoPersonOffset);
            }
            else if (quantity >= 3)
            {
                updatedPrice = UpdatePrice(price, linkedRoom.OtherOccupationRatio, linkedRoom.OtherOccupationOffset);
            }

            return updatedPrice;
        }

        private static decimal? UpdatePriceByQuantityLinkedRatePlan(int? quantity, decimal? price, vLinkedRatePlans linkedRatePlan)
        {
            decimal? updatedPrice = 0;

            if (quantity == 1)
            {
                updatedPrice = UpdatePrice(price, linkedRatePlan.OnePersonRatio, linkedRatePlan.OnePersonOffset);
            }
            else if (quantity == 2)
            {
                updatedPrice = UpdatePrice(price, linkedRatePlan.TwoPersonRatio, linkedRatePlan.TwoPersonOffset);
            }
            else if (quantity >= 3)
            {
                updatedPrice = UpdatePrice(price, linkedRatePlan.OthersOccupationRatio, linkedRatePlan.OthersOccupationOffset);
            }

            return updatedPrice;
        }


        #endregion


        #region Calcular Precios

        private static decimal? UpdatePrice(decimal? price,decimal? ratio, decimal? offset)
        {
            decimal? updatedPrice = ((price * (ratio * 100)) / 100) + offset; // ver como lo hace cuando offset es negativo

            return updatedPrice;
        }

        #endregion


    }
}
