using System.Linq;
using APIServices.Models;

namespace APIServices.Helpers.Reservation
{
    public static class ReservationHelper
    {
        public static Reservaciones GetReservation(int idReservacion)
        {
            Reservaciones reservaciones = null;

            using(var context = new OzHotelesEntities())
            {
                reservaciones = context.Reservaciones.FirstOrDefault(r => r.idReservacion == idReservacion);
            }

            return reservaciones;
        }
    }
}
