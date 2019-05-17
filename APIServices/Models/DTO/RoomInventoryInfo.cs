using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
    public class RoomInventoryInfo
    {
        public int RoomId { get; set; }
        public DateTime Date  { get; set; }
        public int FullInventory { get; set; }
        public int Reserved { get; set; }
        public int Available { get; set; }
    }
}
