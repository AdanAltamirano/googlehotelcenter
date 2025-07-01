using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Portal.General.Facade;
using Portal.General.Common.Data;

namespace APIServices.Conflux.Helpers.RatesPlan
{
    public static class RatesPlanHelper
    {
        public static List<DataRow> GetRatePlansByHotel(int hotelId, string filter = "")
        {
            RatePlanData datasetRatePlanData = new RatePlanFacade().GetRatePlanByIdHotel(hotelId.ToString(), idioma: 1, IncluirPaquetesSegmentoK: 1, incluirNetRatesPlan: 1, idAsociacion: -1, DeleteFilter: 1, getPromos: false);

            string rowFilter = string.Empty;

            if (!string.IsNullOrEmpty(filter))
            {
                rowFilter = filter;
            }

            DataView dv = new DataView(datasetRatePlanData.Tables[RatePlanData.RATEPLAN_TABLE])
            {
                RowFilter = rowFilter
            };

            var result = dv.Cast<DataRowView>()
                        .Select(drv => drv.Row)
                        .ToList();

            Restriction.RestrictionHelper.RemoveRatePlansNoValids(ref result);

            return result;

        }

        public static List<DataRow> GetRatePlansPromosByHotel(int hotelId, string filter ="")
        {
            RatePlanData datasetRatePlanData = new RatePlanFacade().GetRatePlanByIdHotel(hotelId.ToString(), idioma: 1, IncluirPaquetesSegmentoK: 1, incluirNetRatesPlan: 1, idAsociacion: -1, DeleteFilter: 1, getPromos: true);

            string rowFilter = string.Empty;

            if (!string.IsNullOrEmpty(filter))
            {
                rowFilter = filter;
            }

            var ratePlansPromos = datasetRatePlanData.Tables[RatePlanData.RATEPLAN_TABLE].Select(rowFilter).ToList();

            return ratePlansPromos;

        }


    }
}
