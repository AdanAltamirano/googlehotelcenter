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
            RoomsClosureModel roomsClosureModel = new RoomsClosureModel();
            HotelDatos hotelData = new HotelSistema().GetHotelById(idHotel);
            DataSet availability = new HotelStatusAvailabilityFacade().getStatusAva(idHotel, startDate, endDate);
            DataSet lockRoomTypes;
        
            if (availability.Tables[HotelDatos.HOTEL_TABLE].Rows.Count > 0)
            {
                roomsClosureModel.StatusHotel = (string)availability.Tables[HotelDatos.HOTEL_TABLE]
                    .Rows[0][HotelDatos.FIELD_STATUSAVAILABILITY];
            }

            roomsClosureModel.ColorStatusHotel = SetStatusColor(roomsClosureModel.StatusHotel);
            roomsClosureModel.StartDate = startDate;
            roomsClosureModel.EndDate = endDate;

             RatePlanData ratePlans = new RatePlanFacade()
                .GetRatePlanByIdHotel(idHotel.ToString(), lang , 0 , 1 , idAsociacion: idAsoc, DeleteFilter: 1);

            // Create List
            roomsClosureModel.RateRoomsClosureModelList = new List<RateRoomsClosureModel>();

            //Busqueda por rateplan seleccionado
            if(!string.IsNullOrEmpty(ratePlan))
            {


            }

            //Busqueda general por cada rateplan
            foreach (DataRow ratePlanRow  in ratePlans.Tables[RatePlanData.RATEPLAN_TABLE].Rows)
            {
                DataSet roomsTest = new RoomFacade().getRooms(idHotel);

                RateRoomsClosureModel rateRoomsClosureModel = new RateRoomsClosureModel();
                rateRoomsClosureModel.CodeRoomModelsList = new List<CodeRoomModel>();

                string nameRatePlan = (string) ratePlanRow[RatePlanData.FIELD_CODIGOTARIFA];
                rateRoomsClosureModel.RatePlan = nameRatePlan;
                //Por cada habitacion en roomsTest

                foreach (DataRow room in roomsTest.Tables[RoomsHotelData.TBL_ROOM_HOTEL].Rows)
                {
                    // GetLockRoomTypes(MyBase.cInfoActual.Hotel, drrateplan(dsrateplans.FIELD_CODIGOTARIFA), dateStart, dateEnd, drroom(dsrooms.FLD_ID_ROOM_HOTEL))
                    lockRoomTypes = GetLockRoomTypes(idHotel,ratePlanRow[RatePlanData.FIELD_CODIGOTARIFA].ToString()
                        ,startDate.ToString(),endDate.ToString(),room[RoomsHotelData.FLD_ID_ROOM_HOTEL].ToString());

                    DataTable dtLock = (lockRoomTypes.Tables[0].Rows.Count != 0) ? lockRoomTypes.Tables[0] : lockRoomTypes.Tables[1];

                    bool availableOnPortal = (bool)hotelData.Tables[0].Rows[0]["AvailOnPortal"];

                    // Si no esta disponible en portal
                    if (!availableOnPortal)
                    {
                       
                        string roomName = room[RoomsHotelData.FLD_NOMBRE].ToString();
                        string roomCode = room[RoomsHotelData.FLD_ROOM_CODE].ToString();

                        CodeRoomModel codeRoomModel = new CodeRoomModel();
                        codeRoomModel.Code = roomCode;
                        codeRoomModel.RoomName = roomName;

                        int diff = (endDate.Date - startDate.Date).Days;
                        codeRoomModel.Status = new string[diff + 1];
                        for(int i = 0; i <= diff; i++)
                        {
                            codeRoomModel.Status[i] = "C";
                        }

                        rateRoomsClosureModel.CodeRoomModelsList.Add(codeRoomModel);

                    }
                    else
                    {
                        // Si no hay cierre para esa fecha en la habitacion
                        if(dtLock.Rows.Count != 0)
                        {
                            string roomName = room[RoomsHotelData.FLD_NOMBRE].ToString();
                            string roomCode = room[RoomsHotelData.FLD_ROOM_CODE].ToString();

                            CodeRoomModel codeRoomModel = new CodeRoomModel();
                            codeRoomModel.Code = roomCode;
                            codeRoomModel.RoomName = roomName;

                            //Por cada habitacion
                            foreach (DataRow drLock in dtLock.Rows)
                            {
                                DateTime startDay = Convert.ToDateTime(drLock["StartDate"].ToString()).Date;
                                DateTime endDay = Convert.ToDateTime(drLock["EndDate"].ToString()).Date;
                                int diff = (endDate.Date - startDate.Date).Days + 1;
                                string[] rangeDays = new string[diff];

                                codeRoomModel.Status = new string[diff];

                                for (int i = 0; i < diff; i++)
                                {
                                    rangeDays[i] = (rangeDays[i] == null) ? "" + startDate.ToString("yyyy/MM//dd") + "," : rangeDays[i];

                                    string statusStrings = ((startDate.AddDays(i).Date <= endDay && startDate.AddDays(i).Date >= startDay)) 
                                        ? drLock["StatusAvail"].ToString() : "";

                                    rangeDays[i] += (!string.IsNullOrEmpty(statusStrings)) ? statusStrings : " ";

                                    string rangeDaysSplit = rangeDays[i].Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries)[1];


                                    codeRoomModel.Status[i] = (rangeDaysSplit == " ")? "O" : rangeDaysSplit;
                                }

                            }

                            rateRoomsClosureModel.CodeRoomModelsList.Add(codeRoomModel);


                        }
                        else
                        {

                            string roomName = room[RoomsHotelData.FLD_NOMBRE].ToString();
                            string roomCode = room[RoomsHotelData.FLD_ROOM_CODE].ToString();

                            CodeRoomModel codeRoomModel = new CodeRoomModel();
                            codeRoomModel.Code = roomCode;
                            codeRoomModel.RoomName = roomName;

                            int diff = (endDate.Date - startDate.Date).Days;
                            codeRoomModel.Status = new string[diff + 1];
                            for (int i = 0; i <= diff; i++)
                            {
                                codeRoomModel.Status[i] = "O";
                            }

                            rateRoomsClosureModel.CodeRoomModelsList.Add(codeRoomModel);

                        }
                    }

                } // End rooms for

                roomsClosureModel.RateRoomsClosureModelList.Add(rateRoomsClosureModel);
            }


            return roomsClosureModel;
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
                command.SelectCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime));
                DateTime dateStart = Convert.ToDateTime(startDate);
                command.SelectCommand.Parameters["@StartDate"].Value = dateStart.ToString("yyyy/MM/dd");
                command.SelectCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime));
                DateTime dateEnd = Convert.ToDateTime(endDate);
                command.SelectCommand.Parameters["@EndDate"].Value = dateEnd.ToString("yyyy/MM/dd");
                command.SelectCommand.Parameters.Add(new SqlParameter("@idhotel", SqlDbType.Int));
                command.SelectCommand.Parameters["@idhotel"].Value = idHotel;
                command.SelectCommand.Parameters.Add(new SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4));
                command.SelectCommand.Parameters["@IdRatePlan"].Value = idRatePlan;
                command.SelectCommand.Parameters.Add(new SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int));
                command.SelectCommand.Parameters["@idTipoHabitacion_Hotel"].Value = Int32.Parse(idTipoHabitacionHotel);

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

        private void SimilarData()
        {
            //RoomFacade
        }

    }
}
