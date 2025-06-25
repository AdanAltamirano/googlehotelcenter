using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.Inventory
{
    public class Inventory
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[] RoomsList { get; set; }
        public bool[] Days { get; set; }
    }
}
