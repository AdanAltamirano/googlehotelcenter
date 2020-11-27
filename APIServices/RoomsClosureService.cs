using System;
using System.Collections.Generic;
using System.Linq;
using Portal.General.Facade;
using Portal.Hotel.Facade;
using Portal.Hotel.Common.Data;
using Portal.General.Common.Data;
using System.Data;
using APIServices.Models.DTO;
using System.Data.SqlClient;
using System.Configuration;

namespace APIServices
{
    /// <summary>
    /// Servicio para guadar y cargar el cierre de las habitaciones
    /// </summary>
    public class RoomsClosureService
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="idHotel"></param>
       /// <param name="startDate"></param>
       /// <param name="endDate"></param>
       /// <param name="ratePlan">Optional</param>
        public RoomsClosureModel LoadData(int idHotel,DateTime startDate,DateTime endDate,int idAsoc,string ratePlan = "",int lang = 1)
        {
            RoomsClosureModel roomsClosure = new RoomsClosureModel();
            HotelDatos hotelData = new HotelSistema().GetHotelById(idHotel);
            DataSet availability = new HotelStatusAvailabilityFacade().getStatusAva(idHotel, startDate, endDate);
            DataSet lockRoomTypes;

            if (availability.Tables[HotelDatos.HOTEL_TABLE].Rows.Count > 0)
            {
                roomsClosure.StatusHotel = (string)availability.Tables[HotelDatos.HOTEL_TABLE]
                    .Rows[0][HotelDatos.FIELD_STATUSAVAILABILITY];
            }

            roomsClosure.ColorStatusHotel = SetStatusColor(roomsClosure.StatusHotel);
            roomsClosure.StartDate = startDate;
            roomsClosure.EndDate = endDate;

             RatePlanData ratePlans = new RatePlanFacade()
                .GetRatePlanByIdHotel(idHotel.ToString(), lang , 0 , 1 , idAsociacion: idAsoc, DeleteFilter: 1);

            //Por cada rate plan
            foreach (DataRow ratePlanRow  in ratePlans.Tables[RatePlanData.RATEPLAN_TABLE].Rows)
            {
                DataSet roomsTest = new RoomFacade().getRooms(idHotel);
                string nameRatePlan = (string) ratePlanRow[RatePlanData.FIELD_CODIGOTARIFA];
                //Por cada habitacion en roomsTest
             
                foreach(DataRow room in roomsTest.Tables[RoomsHotelData.TBL_ROOM_HOTEL].Rows)
                {
                    // GetLockRoomTypes(MyBase.cInfoActual.Hotel, drrateplan(dsrateplans.FIELD_CODIGOTARIFA), dateStart, dateEnd, drroom(dsrooms.FLD_ID_ROOM_HOTEL))
                    lockRoomTypes = GetLockRoomTypes(idHotel,ratePlanRow[RatePlanData.FIELD_CODIGOTARIFA].ToString()
                        ,startDate.ToString(),endDate.ToString(),room[RoomsHotelData.FLD_ID_ROOM_HOTEL].ToString());
                }


            }


            return roomsClosure;
        }



        /// <summary>
        ///  Set Color Status Hotel
        /// </summary>
        /// <param name="statusHotel"></param>
        /// <returns>String Color</returns>
        private string SetStatusColor(string statusHotel)
        {
            switch (statusHotel.ToUpper().Trim())
            {
                case "O":
                    return "MediumSeaGreen";

                case "C":
                    return "Red";

                case "N":
                    return "LightSteelBlue";

                case "NR":
                    return "#b8860b";

                case "NA":
                    return "#ffa07a";

                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idRatePlan"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="idTipoHabitacionHotel"></param>
        /// <returns></returns>
        private DataSet GetLockRoomTypes(in int idHotel , in string idRatePlan, in string startDate, 
            in string endDate, in string idTipoHabitacionHotel)
        {
            DataSet data = new DataSet();
            SqlDataAdapter command = new SqlDataAdapter();
            string connectionString = ConfigurationManager.AppSettings["HotelConnectionString"];
            command.SelectCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            try
            {
                command.SelectCommand.CommandType = CommandType.StoredProcedure;
                command.SelectCommand.CommandText = "spGetLockRoomTypes";
                command.SelectCommand.Connection = sqlConnection;
                command.SelectCommand.Transaction = sqlTransaction;

                command.SelectCommand.Parameters.Clear();
                command.SelectCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Char, 10));
                command.SelectCommand.Parameters["@StartDate"].Value = Convert.ToDateTime(startDate).ToString("yyyy//MM/dd");
                command.SelectCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Char, 10));
                command.SelectCommand.Parameters["@EndDate"].Value = Convert.ToDateTime(endDate).ToString("yyyy/MM//dd");
                command.SelectCommand.Parameters.Add(new SqlParameter("@idhotel", SqlDbType.Int));
                command.SelectCommand.Parameters["@idhotel"].Value = idHotel;
                command.SelectCommand.Parameters.Add(new SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4));
                command.SelectCommand.Parameters["@IdRatePlan"].Value = idRatePlan;
                command.SelectCommand.Parameters.Add(new SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int));
                command.SelectCommand.Parameters["@idTipoHabitacion_Hotel"].Value = idTipoHabitacionHotel;

                command.Fill(data);
            }
            catch(Exception exception)
            {
                string msg = exception.Message;
            }
            finally
            {
                if(command.SelectCommand != null)
                {
                    if(command.SelectCommand.Connection != null)
                    {
                        command.SelectCommand.Connection.Dispose();
                    }

                    command.SelectCommand.Dispose();
                }

                command.Dispose();
            }

            if(sqlConnection.State == ConnectionState.Open)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
            }

            return data;
        }

    }
}
