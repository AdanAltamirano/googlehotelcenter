using System;
using System.Collections.Generic;
using System.Linq;
using APIServices.Models;
using APIServices.Models.DTO;

namespace APIServices
{
    public class F2GService
    {
        public OzHotelesEntities DbContext = new OzHotelesEntities();
        OzUniEntities DbContextUnivisit = new OzUniEntities();

      /// <summary>
      /// Get F2G codes with hotels
      /// </summary>
      /// <param name="corporateId"></param>
      /// <returns>List of f2g codes with hotels</returns>
       public List<F2GModel> GetF2GConfigurationByCorporateId(int corporateId,string lang)
       {
            var f2gCodes = DbContext.
              GetAllF2GCode(corporateId).
              Select(s => new F2GModel
              {
                  RatePlanIdF2G = s.idrateplan_f2g.Trim(),
                  PromoCode = s.promoCode,
                  hasPromo = (s.promoCode == null)? false : true

              }).ToList();

            var hotels = DbContextUnivisit.
                      GetHotelsByCorporateId(corporateId)
                       .Select(s => new F2GHotel
                       {
                           HotelId = s.idHotel,
                           CompanyId = s.IdEmpresa,
                           CorpId = s.idCorporativo,
                           Name = s.Nombre
                       })
                      .ToList();


            List<F2GRate> allRates = new List<F2GRate>();
        
            for (int i = 0; i < hotels.Count; i++)
            {
                int id = hotels.ElementAt(i).HotelId;

                var rates = DbContext.
                    RatesPlan.
                    Where(r => r.IdHotel == id
                    && (r.Deleted == null || r.Deleted == false)
                    && (r.IsPromo == null || r.IsPromo == false))
                    .Select(s => new F2GRate
                    {
                        RatePlanId = s.idRatePlan,
                        Description = s.Description,
                        RateCode = s.codigotarifa,
                        Name = s.idRatePlan

                    }).ToList();

                allRates = allRates.Concat(rates).ToList();
            }

            var noduplicates =
                allRates.Distinct(new RateComparer()).ToList();

            var promoText = (lang.Equals("es-MX")) 
                ? "Sin código de promoción" 
                : "No promotion code";


            for (int i = 0; i < f2gCodes.Count; i++)
            {
                var list = DbContext.
                        GetMappedHotelsWithRatePlan(corporateId,f2gCodes.ElementAt(i).RatePlanIdF2G,f2gCodes.ElementAt(i).PromoCode).
                         Select(s => new F2GMappedHotels
                         {
                             CompanyId = s.IdEmpresa,
                             HotelId = s.idHotel,
                             Name = s.Nombre,
                             RatePlanIdUv = s.idRatePlan_uv,
                             RatePlanF2GId = s.idrateplan_f2g,
                             Promo = s.promoCode

                         }).
                        ToList();

                f2gCodes.ElementAt(i).PromoCode = 
                    (f2gCodes.ElementAt(i).PromoCode == null) 
                    ? promoText 
                    : f2gCodes.ElementAt(i).PromoCode.Trim();

                f2gCodes.ElementAt(i).HotelsMapped = list;
                f2gCodes.ElementAt(i).Hotels = hotels;
                f2gCodes.ElementAt(i).Rates = noduplicates;
                f2gCodes.ElementAt(i).AddState = false;
                f2gCodes.ElementAt(i).DeleteState = false;
                f2gCodes.ElementAt(i).Hotel = new List<F2GHotel>();
                f2gCodes.ElementAt(i).RatesList = new List<F2GRate>();
            }
             
            return f2gCodes;
       }


        /// <summary>
        /// Get a list of corporates
        /// </summary>
        /// <returns>List of corporates</returns>
        public List<F2GCorporate> GetF2GCorporates()
        {
            var coorporatives = DbContext.Corporativos.
                Select(s => new F2GCorporate
                {
                    Id = s.idCorporativo,
                    Text = s.NombreCorp,
                    Value = s.NombreCorp
                }).
                OrderBy(o => o.Id).
                ToList();

            return coorporatives;
        }


      /// <summary>
      /// Update f2g rate plans
      /// </summary>
      /// <param name="hotels"></param>
      /// <param name="rates"></param>
      /// <param name="ratePlanIdF2G"></param>
      /// <param name="promoCode"></param>
      /// <returns>Boolean if could update table</returns>
        public bool UpdateF2GRatePlan(F2GSaveModel f2g)
        {
            try
            {
                string promo = (f2g.Promo.Equals("Sin código de promoción") 
                    || f2g.Promo.Equals("No promotion code")) 
                    ? null 
                    : f2g.Promo; 

                for (int i = 0; i < f2g.Rates.Count; i++)
                {
                    for (int j = 0; j < f2g.Hotels.Count; j++)
                    {
                        DbContext.UpdateF2GRatePlan(f2g.Rates.ElementAt(i).RatePlanId,
                            f2g.Hotels.ElementAt(j).HotelId,f2g.F2GId,promo,f2g.Action);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="corporateId"></param>
        /// <returns></returns>
        public F2GHotelsRates GetF2GHotelsRates(int corporateId)
        {
            F2GHotelsRates f2g = new F2GHotelsRates();

            var hotels = DbContextUnivisit.
                    GetHotelsByCorporateId(corporateId)
                     .Select(s => new F2GHotel
                     {
                         HotelId = s.idHotel,
                         CompanyId = s.IdEmpresa,
                         CorpId = s.idCorporativo,
                         Name = s.Nombre
                     })
                    .ToList();

            List<F2GRate> allRates = new List<F2GRate>();

            for (int i = 0; i < hotels.Count; i++)
            {
                int id = hotels.ElementAt(i).HotelId;

                var rates = DbContext.
                    RatesPlan.
                    Where(r => r.IdHotel == id
                    && (r.Deleted == null || r.Deleted == false)
                    && (r.IsPromo == null || r.IsPromo == false))
                    .Select(s => new F2GRate
                    {
                        RatePlanId = s.idRatePlan,
                        Description = s.Description,
                        RateCode = s.codigotarifa,
                        Name = s.idRatePlan

                    }).ToList();

                allRates = allRates.Concat(rates).ToList();
            }

            var noduplicates =
                 allRates.Distinct(new RateComparer()).ToList();
                

            f2g.Hotels = hotels;
            f2g.Rates = noduplicates;

            return f2g;
        }

    }


    class RateComparer : IEqualityComparer<F2GRate>
    {
        public bool Equals(F2GRate x, F2GRate y)
        {
            return x.RatePlanId == y.RatePlanId;
           
        }
        public int GetHashCode(F2GRate obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashRatePlanId = obj.RatePlanId == null ? 0 : obj.RatePlanId.GetHashCode();

            return hashRatePlanId;

        }
    }

}
