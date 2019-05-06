using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class DailyRate
    {
        public int RateId { get; set; }
        public DateTime Date { get; set; }
        public byte Occupancy { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Currency { get; set; }
    }
}
