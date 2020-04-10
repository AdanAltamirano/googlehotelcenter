using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Models.DTO
{
   
    public static class Excel
    {  
        public class ReservationList
        {
            public string NoReservation { get; set; }
            public string Hotel { get; set; }
            public string Customer { get; set; }
            public DateTime Date { get; set; }
            public DateTime CheckIn { get; set; }
            public DateTime CheckOut { get; set; }
            public string Origin { get; set; }
            public string Corporate { get; set; }
            public string Total { get; set; }
            public string Status { get; set; }
        }
    }
}
