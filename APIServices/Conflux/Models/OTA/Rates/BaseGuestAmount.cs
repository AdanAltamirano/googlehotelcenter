using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.OTA.Models.Rates
{
    public class BaseGuestAmount
    {
        public decimal? AmountBeforeTax { get; set; }
        public decimal? AmountAfterTax { get; set; }
        public string NumberOfGuests { get; set; }
        public int? AgeQualifyingCode { get; set; }
        //public string CurrencyCode { get; set; }
    }
}
