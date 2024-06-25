using APIServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{
    public class UtilsService
    {
        public OzHotelesEntities dbContext = new OzHotelesEntities();

        public List<Canales> GetChannels() => dbContext.Canales.ToList<Canales>();

        public List<vHotelChannel> GetHotelChannels(int idHotel) => dbContext.vHotelChannel.Where(x => x.idHotel == idHotel).ToList<vHotelChannel>();

    }
}
