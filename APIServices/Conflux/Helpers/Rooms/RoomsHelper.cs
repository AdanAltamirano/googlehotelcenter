using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Portal.Hotel.Facade;
using Portal.Hotel.Common.Data;



namespace APIServices.Conflux.Helpers.Rooms
{
    public static class RoomsHelper
    {
        public static List<DataRow> GetRoomsByHotel(int hotelId, string filter = "")
        {
            RoomsHotelData ds = new RoomFacade().getAllRooms(hotelId, 1);

            string rowFilter = "eliminada = false";

            if (!String.IsNullOrEmpty(filter))
            {
                rowFilter += filter;
            }

            DataView dv = new DataView(ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL])
            {
                RowFilter = rowFilter
            };

            return dv.Cast<DataRowView>()
                        .Select(drv => drv.Row)
                        .ToList();
        }
    }
}
