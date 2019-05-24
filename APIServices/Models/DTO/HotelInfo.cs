using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class HotelInfo
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public int? CorpId { get; set; }
        public string Corp { get; set; }
        public byte Status { get; set; }
        public Room[] Rooms { get; set; }
        public RatePlan[] RatePlans { get; set; }
    }
}
