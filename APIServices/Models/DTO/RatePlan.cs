using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class RatePlan
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? HotelId { get; set; }
        public string Currency { get; set; }
        public int? CommisionPercentage { get; set; }
    }
}
