using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Enum;


namespace APIServices.Conflux.Helpers.Rate
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
                    price.Price = UpdatePriceByQuantityLinkedRoom(price.Quantity, price.Price, linkedRoom);

                }
                else if(price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    price.Price = UpdatePrice(price.Price, linkedRoom.ExtraAdultRatio, linkedRoom.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    price.Price = UpdatePrice(price.Price, linkedRoom.ExtraChildRatio, linkedRoom.ExtraChildOffset);
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
                    price.Price = UpdatePriceByQuantityLinkedRatePlan(price.Quantity, price.Price, linkedRatePlan);

                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraAdultRatio, linkedRatePlan.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraChildRatio, linkedRatePlan.ExtraChildOffset);
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
                    price.Price = UpdatePriceByQuantityLinkedRoom(price.Quantity, price.Price, linkedRoom);

                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    price.Price = UpdatePrice(price.Price, linkedRoom.ExtraAdultRatio, linkedRoom.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    price.Price = UpdatePrice(price.Price, linkedRoom.ExtraChildRatio, linkedRoom.ExtraChildOffset);
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
                    price.Price = UpdatePriceByQuantityLinkedRatePlan(price.Quantity, price.Price, linkedRatePlan);

                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraAdult)
                {
                    price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraAdultRatio, linkedRatePlan.ExtraAdultOffset);
                }
                else if (price.PersonType == (int)PersonTypeEnum.ExtraChild ||
                            price.PersonType == (int)PersonTypeEnum.ExtraTeeneger)
                {
                    price.Price = UpdatePrice(price.Price, linkedRatePlan.ExtraChildRatio, linkedRatePlan.ExtraChildOffset);
                }
            }
        }



        #endregion

        #region Calcular Precios por Cantidad

        private static decimal? UpdatePriceByQuantityLinkedRoom(int? quantity ,decimal? price, vLinkedRoomTypes linkedRoom)
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
