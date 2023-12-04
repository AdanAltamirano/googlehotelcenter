using System;

namespace APIServices.Conflux.Models.Rates
{
    public class RatePrice
    {
        public Nullable<int> PersonType { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
}
