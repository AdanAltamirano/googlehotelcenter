using APIServices.Models;
using APIServices.Models.DTO;
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



        public ReservationDetailsModel GetDetailsById(int reservationId)
        {
            /*var details = dbContext.vReservationDetails
                .FirstOrDefault(x => x.reservationId == reservationId);

            var model = new ReservationDetailsModel();
            if (details != null)
            {
                model.HotelName = details.hotelName;
                model.Address = details.address;
                model.Status = details.status;
                model.CheckIn = details.checkIn;
                model.CheckOut = details.checkOut;
                model.City = details.city;
                model.Country = details.country;
                model.AccessCode = details.accessCode;
                model.Customer = new CustomerDetails()
                {
                    Name = details.customerName,
                    Email = details.customerEmail,
                    Phone = details.customerPhone
                };
            }

            return model;*/
            return null;
        } 
    }
}
