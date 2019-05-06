using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class Room
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int TotalRooms { get; set; }
        public byte MaxOccupancy { get; set; }
        public byte ExtraOccupancyAllowed { get; set; }
        public byte MaxAdultsOccupancy { get; set; }
        public byte MinAdultsOccupancy { get; set; }
        public byte MaxChildrenOccupancy { get; set; }
        public short Order { get; set; }
        public bool Active { get; set; }
    }
}
