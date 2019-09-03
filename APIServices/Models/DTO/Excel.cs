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
            public string Date { get; set; }
            public string CheckIn { get; set; }
            public string CheckOut { get; set; }
            public string Origin { get; set; }
            public string Status { get; set; }

            public static string GetStatus(int status)
            {
                string str = "";
                switch (status)
                {
                    case 1: str = "Reservado"; break;
                    case 3: str = "Cancelado"; break;
                    case 4: str = "En proceso"; break;
                }
                return str;
            }

            public static string GetOrigin(string origin)
            {
                string str = "";
                switch (origin)
                {

                }
                return str;
            }
        }
    }
}
