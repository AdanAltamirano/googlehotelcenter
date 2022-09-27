using System.Collections.Generic;
using System.Linq;
using APIServices.Models;
using APIServices.Models.DTO.Currency;

namespace APIServices.Currency
{
    public class CurrencyService
    {
        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>List of currencies</returns>
       public List<CurrencyDTO>  GetCurrencies()
        {
            List<CurrencyDTO> currencies = new List<CurrencyDTO>();

            using(OzUniEntities ozUniEntities = new OzUniEntities())
            {
                var currenciesTemp = ozUniEntities.Monedas.Select(m => 
                    new CurrencyDTO
                    {
                        CurrencyId = m.idMoneda,
                        Name = m.Nombre,
                        Symbol = m.Signo,
                        Abbreviation = m.Abreviatura,
                        Code = m.Codigo

                    }).ToList();

                currencies.AddRange(currenciesTemp);
            }

            return currencies;
        }
       
    }
}
