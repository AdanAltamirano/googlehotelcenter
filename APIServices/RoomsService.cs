using APIServices.Models;
using APIServices.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{
    /// <summary>
    /// Métodos para consulta y manejo de habitaciones de hotel
    /// </summary>
    public class RoomsService
    {
        public IEnumerable<Room> FindByHotel(int hotelId, int language = 1, bool showInactive = false)
        {
            IEnumerable<Room> result = null;

            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                var query = db.HotelRoom.Where(r =>
                   r.HotelId == hotelId
                   && r.Language == language);

                if (!showInactive)
                    query.Where(r => r.Active == true);                    

                result = query.OrderBy(r=>r.Order).Select(r=> new Room {
                    Name = r.Name,
                    Code = r.Code,
                    Active = r.Active ?? false,
                    ExtraOccupancyAllowed = r.ExtraOccupancyAllowed,
                    MinAdultsOccupancy = r.MinAdultsOccupancy,
                    MaxAdultsOccupancy = r.MaxAdultsOccupancy,
                    MaxChildrenOccupancy = r.MaxChildrenOccupancy,
                    MaxOccupancy = r.MaxOccupancy,
                    Order = r.Order,
                    TotalRooms = r.TotalRooms,
                    Type = r.Type
                }).ToArray();
            }

            return result;
        }
    }
}
