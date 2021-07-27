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
    public class RatesPlanService
    {
        public OzHotelesEntities DbContext = new OzHotelesEntities();

        public IQueryable<vHotelPlan> GetAll()
        {
            return DbContext.vHotelPlan.AsQueryable();
        }

        public IEnumerable<RatePlan> FindByHotel(int hotelId, int language = 1, bool showInactive = false)
        {
            IEnumerable<RatePlan> result = new List<RatePlan>();

            using (OzHotelesEntities db = new OzHotelesEntities())
            {
                var query = db.vHotelPlan.Where(r =>
                   r.HotelId == hotelId
                   && (r.Language == language || r.Language == null) && r.IsPromo != true);

                if (!showInactive)
                    query = query.Where(r => r.Active == true);
                
                result = query.OrderBy(r => r.Name).Select(r => new RatePlan
                {
                    Name = r.Name,
                    Code = r.Code,
                    CommisionPercentage = r.CommissionPercentage,
                    ContractId = r.ContractId == null ? -1 : r.ContractId

                }).ToArray();
            }

            return result;
        }
    }
}
