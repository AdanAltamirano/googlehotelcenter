using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class DailyRateDetail
    {
        public int RateId { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Discount { get; set; }
        public string Currency { get; set; }
        public List<DailyRateDetailPrice> Prices { get; set; } = new List<DailyRateDetailPrice>();
    }

    public class DailyRateDetailPrice
    {
        public int Id { get; set; }
        public int RateId { get; set; }
        public int Occupation { get; set; }
        public PaxType Type { get; set; }
        public decimal Price { get; set; }
    }

    public enum PaxType
    {
        Adult = 1,
        Child,
        Junior
    }
}
