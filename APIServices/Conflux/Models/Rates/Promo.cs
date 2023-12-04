using System;

namespace APIServices.Conflux.Models.Rates
{
    public class Promo
    {
        public decimal Discount { get; set; }
        public decimal DayDiscount { get; set; }
        public int DiscountLevel { get; set; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
    }
}
