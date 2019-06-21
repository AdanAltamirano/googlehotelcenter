using APIServices.Models;
using APIServices.Models.DTO;
using System.Linq;

namespace APIServices
{
    public class HotelService
    {

        public OzHotelesEntities DbContext = new OzHotelesEntities();

        /// <summary>
        /// Búsqueda de tarifas diarias
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="language"></param>
        /// <param name="roomId"></param>
        /// <returns>Conjunto de tarifas</returns>
        public HotelInfo Get(int hotelId, int language = 1)
        {
            HotelInfo result = null;

            vHotelBasicInfo hotel = DbContext.vHotelBasicInfo.FirstOrDefault(x => x.Id == hotelId);

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
            var rooms = DbContext.vHotelRoom.Where(r =>
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
                    JuniorsAllowed = r.JuniorsAllowed ?? false,
                    Order = r.Order,
                    TotalRooms = r.TotalRooms,
                    Type = r.Type
                }).ToArray();

            result.Rooms = rooms;

            //rateplans
            var plans = DbContext.vHotelPlan.Where(x => x.HotelId == hotelId && x.Language == language && x.Active.Value && !x.IsLinked.Value)
                .Select(r => new RatePlanHeader
                {
                    Code = r.Code,
                    Name = r.Name,
                    Currency = r.Currency
                }).ToArray();

            result.RatePlans = plans;
             
            return result;
        }

        public IQueryable<vHotelBasicInfo> GetAll()
        {
            return DbContext.vHotelBasicInfo.AsQueryable();
        }
    }
}
