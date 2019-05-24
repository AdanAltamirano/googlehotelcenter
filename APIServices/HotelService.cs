using APIServices.Models;
using APIServices.Models.DTO;
using System.Linq;

namespace APIServices
{
    public class HotelService
    {
        /// <summary>
        /// Búsqueda de tarifas diarias
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="language"></param>
        /// <param name="roomId"></param>
        /// <returns>Conjunto de tarifas</returns>
        public HotelInfo FindById(int hotelId, int language = 1)
        {
            HotelInfo result = null;
            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                HotelBasicInfo hotel = db.HotelBasicInfo.FirstOrDefault(x => x.Id == hotelId);

                result = new HotelInfo
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    CompanyId = hotel.CompanyId,
                    CorpId = hotel.CorpId,
                    Corp = hotel.Corp,
                    Currency = hotel.Currency,
                    Status = hotel.Status
                };

                //rooms 
                var rooms = db.HotelRoom.Where(r =>
                   r.HotelId == hotelId
                   && r.Language == language 
                   && r.Active == true)
                   .OrderBy(r => r.Order)
                   .Select(r => new Room
                   {
                       Id = r.Id,
                       Name = r.Name,
                       Code = r.Code,
                       Active = r.Active.Value,
                       ExtraOccupancyAllowed = r.ExtraOccupancyAllowed,
                       MinAdultsOccupancy = r.MinAdultsOccupancy,
                       MaxAdultsOccupancy = r.MaxAdultsOccupancy,
                       MaxChildrenOccupancy = r.MaxChildrenOccupancy,
                       MaxOccupancy = r.MaxOccupancy,
                       Order = r.Order,
                       TotalRooms = r.TotalRooms,
                       Type = r.Type
                   }).ToArray();

                result.Rooms = rooms;

                //rateplans
                var plans = db.HotelPlan.Where(x => x.HotelId == hotelId && x.Language == language)
                    .Select(r => new RatePlan
                    {
                        Code = r.Code,
                        Name = r.Name,
                        Currency = r.Currency
                    }).ToArray();

                result.RatePlans = plans;


            }

            return result;
        }
    }
}
