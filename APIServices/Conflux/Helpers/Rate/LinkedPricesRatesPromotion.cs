using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Enum;



namespace APIServices.Conflux.Helpers.Rate
{
    public static partial class RatesHelpers
    {
        #region Tarifas Promociones

        public static void UpdatePricesLinkedRoom(vLinkedRoomTypes linkedRoom, ref List<spGetPricesByRatePromotion_Result> prices)
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

        public static void UpdatePricesLinkedRatePlan(vLinkedRatePlans linkedRatePlan, ref List<spGetPricesByRatePromotion_Result> prices)
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

        #region Tarifas Promociones Excepciones

        public static void UpdatePricesLinkedRoom(vLinkedRoomTypes linkedRoom, ref List<spGetPricesByRatePromotionException_Result> prices)
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

        public static void UpdatePricesLinkedRatePlan(vLinkedRatePlans linkedRatePlan, ref List<spGetPricesByRatePromotionException_Result> prices)
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


    }
}
