using APIServices.Models;
using System.Linq;

namespace APIServices
{
    public class ReservationService
    {
        public OzHotelesEntities dbContext = new OzHotelesEntities();


        public IQueryable<vReservation> GetAll() => dbContext.vReservation.AsQueryable();

        public IQueryable<vReservation> Get(int hotelId)
        {
            return dbContext.vReservation.AsQueryable()
                .Where(x => x.HotelId == hotelId);
        }
    }
}
