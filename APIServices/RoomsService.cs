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
            IEnumerable<Room> result = new List<Room>();

            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                var query = db.vHotelRoom.Where(r =>
                   r.HotelId == hotelId
                   && r.Language == language);

                if (!showInactive)
                    query = query.Where(r => r.Active == true);                    

                result = query.OrderBy(r=>r.Order).Select(r=> new Room {
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
            }

            return result;
        }

        public IEnumerable<RoomInventoryInfo> FindInventoryByRoomId(int roomId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<RoomInventoryInfo> result = new List<RoomInventoryInfo>();

            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                var query = db.vRoomInventory
                    .Where(r => r.RoomId == roomId && r.Date >= startDate && r.Date <= endDate)
                    .OrderBy(r => r.Date).ToArray();

                // arreglo de días que se usara para dividir el rango de las tarifas por día
                DateTime[] days = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToArray();

                result = days.Select(d => {

                    var IH = query.FirstOrDefault(q => q.Date == d);

                    return new RoomInventoryInfo
                    {
                        RoomId = roomId,
                        Date = d,
                        Available = IH?.Available ?? 0,
                        FullInventory = IH?.FullInventory ?? 0,
                        Reserved = IH?.Reserved ?? 0
                    };
                });
            }

            return result;
        }
    }
}
