using System;
using System.Linq;
using System.Collections.Generic;
using APIServices.Models;

namespace APIServices.Helpers.Room
{
    public static class RoomHelper
    {
        public static vHotelRoom GetRoom(int roomId)
        {
            vHotelRoom room = null;

            using (OzHotelesEntities hotelesEntities = new OzHotelesEntities())
            {
                room = hotelesEntities.vHotelRoom.FirstOrDefault(hr => hr.Id == roomId);
            }

            return room;
        }
    }
}
