using System;
using System.Collections.Generic;
using System.Linq;
using APIServices.Models;
using APIServices.Service.Arpon.Models;

namespace APIServices.Service.Arpon
{
    public class ArponService
    {
        public bool SaveRatePlansArpon(List<RatePlansIPH_Arpon_Model> ratePlansIPH_Arpon_ModelsList)
        {
            bool success = false;

            try
            {

                foreach (RatePlansIPH_Arpon_Model ratePlansIPH_Arpon_Model in ratePlansIPH_Arpon_ModelsList)
                {
                    AddRatePlanArpon(ratePlansIPH_Arpon_Model);
                }

                success = true;
            }
            catch(Exception ex)
            {
                success = false;
            }

            return success;
        }

        public bool SaveRoomsArpon(List<RoomsIPH_Arpon_Model> roomsIPH_Arpon_ModelsList)
        {
            bool success = false;

            try
            {
                foreach (RoomsIPH_Arpon_Model roomsIPH_Arpon_Model in roomsIPH_Arpon_ModelsList)
                {
                    AddRoomArpon(roomsIPH_Arpon_Model);
                }

                success = true;
            }
            catch(Exception ex)
            {
                success = false;
            }

            return success;
        }

        #region Add Arpon

        public bool AddHotelArpon(HotelIPH_Arpon_Model hotelIPH_Arpon)
        {
            bool success = false;

            try
            {
                using (ArponEntities arponEntities = new ArponEntities())
                {
                    HotelIPH_Arpon _hotelIPH_Arpon = arponEntities.HotelIPH_Arpon.FirstOrDefault(h => h.IdHotelIP.Equals(hotelIPH_Arpon.IdHotelIp));

                    //Nuevo
                    if (_hotelIPH_Arpon == null)
                    {
                        HotelIPH_Arpon hotelArpon = new HotelIPH_Arpon
                        {
                            IdHotelIP = hotelIPH_Arpon.IdHotelIp,
                            IdHotelArpon = hotelIPH_Arpon.IdHotelArpon,
                            UrlArpon = hotelIPH_Arpon.UrlArpon,
                            AgencyArpon = hotelIPH_Arpon.AgencyArpon
                        };

                        arponEntities.HotelIPH_Arpon.Add(hotelArpon);
                    }
                    //Actualizar
                    else
                    {
                        _hotelIPH_Arpon.IdHotelArpon = hotelIPH_Arpon.IdHotelArpon;
                        _hotelIPH_Arpon.UrlArpon = hotelIPH_Arpon.UrlArpon;
                        _hotelIPH_Arpon.AgencyArpon = hotelIPH_Arpon.AgencyArpon;
                    }

                    arponEntities.SaveChanges();
                }

                success = true;
            }
            catch(Exception ex)
            {
                success = false;
            }

            return success;
        }

        public bool AddRatePlanArpon(RatePlansIPH_Arpon_Model ratePlansIPH_Arpon)
        {
            bool success = false;

            try
            {
                using(ArponEntities arponEntities = new ArponEntities())
                {
                    var _ratePlansIPH_Arpon = arponEntities.RatePlansIPH_Arpon.FirstOrDefault(r => 
                    r.idHotelIP == ratePlansIPH_Arpon.IdHotelIp &&
                    r.IdRatePlanIP.Equals(ratePlansIPH_Arpon.IdRatePlanIp));

                    if(_ratePlansIPH_Arpon == null)
                    {
                        RatePlansIPH_Arpon ratePlanArpon = new RatePlansIPH_Arpon
                        {
                            idHotelIP = ratePlansIPH_Arpon.IdHotelIp,
                            IdRatePlanIP = ratePlansIPH_Arpon.IdRatePlanIp,
                            IdRatePlanArpon = ratePlansIPH_Arpon.IdRatePlanArpon
                        };

                        arponEntities.RatePlansIPH_Arpon.Add(ratePlanArpon);
                    }
                    else
                    {
                        _ratePlansIPH_Arpon.IdRatePlanArpon = ratePlansIPH_Arpon.IdRatePlanArpon;
                    }

                    arponEntities.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                success = false;
            }

            return success;
        }

        public bool AddRoomArpon(RoomsIPH_Arpon_Model roomsIPH_Arpon)
        {
            bool success = false;

            try
            {
                using (ArponEntities arponEntities = new ArponEntities())
                {
                    var _roomIPH_Arpon = arponEntities.RoomsIPH_Arpon.FirstOrDefault(r =>
                    r.idHotelIP == roomsIPH_Arpon.IdHotelIp &&
                    r.IdRoomIP.Equals(roomsIPH_Arpon.IdRoomIp));

                    if (_roomIPH_Arpon == null)
                    {
                        RoomsIPH_Arpon roomArpon = new RoomsIPH_Arpon
                        {
                            idHotelIP = roomsIPH_Arpon.IdHotelIp,
                            IdRoomIP = roomsIPH_Arpon.IdRoomIp,
                            IdRoomArpon = roomsIPH_Arpon.IdRoomArpon
                        };

                        arponEntities.RoomsIPH_Arpon.Add(roomArpon);
                    }
                    else
                    {
                        _roomIPH_Arpon.IdRoomArpon = roomsIPH_Arpon.IdRoomArpon;
                    }

                    arponEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        #endregion

        #region Get

        public HotelIPH_Arpon_Model GetHotelArponById(int idHotel)
        {
            HotelIPH_Arpon_Model hotelIPH_Arpon_Model = new HotelIPH_Arpon_Model();

            try
            {
                using (ArponEntities arponEntities = new ArponEntities())
                {
                    string idHotelIp = idHotel.ToString();

                    var hotelArpon = arponEntities.HotelIPH_Arpon.FirstOrDefault(h => h.IdHotelIP.Equals(idHotelIp));

                    if(hotelArpon != null)
                    {
                        hotelIPH_Arpon_Model.IdHotelIp = hotelArpon.IdHotelIP;
                        hotelIPH_Arpon_Model.IdHotelArpon = hotelArpon.IdHotelArpon;
                        hotelIPH_Arpon_Model.UrlArpon = hotelArpon.UrlArpon ?? string.Empty;
                        hotelIPH_Arpon_Model.AgencyArpon = hotelArpon.AgencyArpon ?? string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return hotelIPH_Arpon_Model;
        }


        public List<RatePlansIPH_Arpon_Model> GetRatePlansByIdHotel(int idHotel)
        {
            //DTO
            List<RatePlansIPH_Arpon_Model> ratePlansIPH_Arpon_Models_List = new List<RatePlansIPH_Arpon_Model>();

            //Db
            List<RatesPlan> ratesPlanList = new List<RatesPlan>();

            try
            {
                using (OzHotelesEntities ozHotelesEntities = new OzHotelesEntities())
                {

                    ratesPlanList = ozHotelesEntities.RatesPlan.
                        Where(rp => rp.IdHotel == idHotel &&
                        (rp.Deleted == null || rp.Deleted == false) &&
                        (rp.IsPromo == null || rp.IsPromo == false))
                        .ToList();
                }

                using(ArponEntities arponEntities = new ArponEntities())
                {
                    foreach(RatesPlan ratesPlan in ratesPlanList)
                    {
                        RatePlansIPH_Arpon ratePlansIPH_Arpon = arponEntities.RatePlansIPH_Arpon
                            .FirstOrDefault(rpa => rpa.idHotelIP == ratesPlan.IdHotel
                            && rpa.IdRatePlanIP.Equals(ratesPlan.idRatePlan));

                        RatePlansIPH_Arpon_Model temp_RatePlansIPH_Arpon_Model = new RatePlansIPH_Arpon_Model
                        {
                            IdHotelIp = ratesPlan.IdHotel,
                            IdRatePlanIp = ratesPlan.idRatePlan
                        };

                        if(ratePlansIPH_Arpon != null)
                        {
                            temp_RatePlansIPH_Arpon_Model.IdRatePlanArpon = ratePlansIPH_Arpon.IdRatePlanArpon;
                        }

                        ratePlansIPH_Arpon_Models_List.Add(temp_RatePlansIPH_Arpon_Model);

                    }
                    
                }

            }
            catch(Exception ex)
            {
                string error = ex.Message;
            }

            return ratePlansIPH_Arpon_Models_List;
        }

        public List<RoomsIPH_Arpon_Model> GetRoomsByIdHotel(int idHotel)
        {
            //DTO
            List<RoomsIPH_Arpon_Model> roomsIPH_Arpon_Models_List = new List<RoomsIPH_Arpon_Model>();

            //Db
            List<TipoHabitaciones_Hoteles> roomsList = new List<TipoHabitaciones_Hoteles>();

            try
            {
                using (ArponEntities arponEntities = new ArponEntities())
                {

                    roomsList = arponEntities.TipoHabitaciones_Hoteles.
                        Where(r => r.idHotel == idHotel &&
                        (r.Eliminada == null || r.Eliminada == false))
                        .ToList();

                    foreach(TipoHabitaciones_Hoteles room in roomsList)
                    {
                        RoomsIPH_Arpon roomsIPH_Arpon = arponEntities.RoomsIPH_Arpon.
                            FirstOrDefault(r => r.idHotelIP == room.idHotel && r.IdRoomIP.Equals(room.CodigoHabitacion));

                        RoomsIPH_Arpon_Model temp_RoomsIPH_Arpon_Model = new RoomsIPH_Arpon_Model
                        {
                            IdHotelIp = room.idHotel,
                            IdRoomIp = room.CodigoHabitacion

                        };

                        if(roomsIPH_Arpon != null)
                        {
                            temp_RoomsIPH_Arpon_Model.IdRoomArpon = roomsIPH_Arpon.IdRoomArpon;
                        }

                        roomsIPH_Arpon_Models_List.Add(temp_RoomsIPH_Arpon_Model);

                    }

                }

               

            }
            catch(Exception ex)
            {
                string error = ex.Message;
            }


            return roomsIPH_Arpon_Models_List;
        }


        #endregion
    }
}
