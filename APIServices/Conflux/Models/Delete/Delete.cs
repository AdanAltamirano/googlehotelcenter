using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.Delete
{
    public class Delete
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string[] RatePlansList { get; set; }
        public int[] RoomsList { get; set; }
        public string[] PromosList { get; set; }
        public bool EnableRatePlans { get; set; }
        public bool EnablePromotions {get;set;}


    }

    public class User
    {
        public bool HasPermissions { get; set; } = false;
    }

}
