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

        public String SaveChannelComission(int idHotel, int idCanal, int Comision)
        {
            String result = "ERROR: ";

            if (idCanal > 0 && Comision > 0 && Comision <= 100)
            {
                List<HotelCanales> canales = dbContext.HotelCanales.Where(x => x.idHotel == idHotel && x.idCanal == idCanal).ToList<HotelCanales>();

                try
                {
                    if (canales.Count() > 0)
                    { // Actualiza el registro existente
                        canales[0].Comision = Comision;
                        dbContext.SaveChanges();
                        result = "SUCCESS_UPDATED";
                    }
                    else
                    { // Si no existe un registro, lo inserta
                        HotelCanales newComision = new HotelCanales(idHotel, idCanal, Comision);

                        dbContext.HotelCanales.Add(newComision);
                        dbContext.SaveChanges();
                        result = "SUCCESS_REGISTERED";
                    }
                }
                catch (Exception exc)
                {
                    result += exc.Message;
                }

            }
            else {
                result += "Invalid parametter";
            }

            return result;

        }

    }
}
