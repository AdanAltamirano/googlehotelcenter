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
        #region Main Funcionality
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="ratePlan">Optional</param>
        public RoomsClosureModel LoadData(int idHotel, DateTime startDate, DateTime endDate, int idAsoc, string[] ratePlansList, int lang = 1)
        {
            RoomsClosureModel roomsClosureModel = new RoomsClosureModel();
            HotelDatos hotelData = new HotelSistema().GetHotelById(idHotel);
            DataSet availability = new HotelStatusAvailabilityFacade().getStatusAva(idHotel, startDate, endDate);
            if (availability.Tables[HotelDatos.HOTEL_TABLE].Rows.Count > 0)
            {
                roomsClosureModel.StatusHotel = (string)availability.Tables[HotelDatos.HOTEL_TABLE]
                    .Rows[0][HotelDatos.FIELD_STATUSAVAILABILITY];
            }

            roomsClosureModel.ColorStatusHotel = SetStatusColor(roomsClosureModel.StatusHotel);
            roomsClosureModel.StartDate = startDate;
            roomsClosureModel.EndDate = endDate;

            RatePlanData ratePlans = new RatePlanFacade()
               .GetRatePlanByIdHotel(idHotel.ToString(), lang, 0, 1, idAsociacion: idAsoc, DeleteFilter: 1);

            // Create List
            roomsClosureModel.RateRoomsClosureModelList = new List<RateRoomsClosureModel>();

            //Busqueda por rateplans seleccionados
            if (!ratePlansList.Contains("0"))
            {

                foreach (var ratePlan in ratePlansList)
                {

                    RateRoomsClosureModel rateRoomsClosureModel = new RateRoomsClosureModel();
                    rateRoomsClosureModel.CodeRoomModelsList = new List<CodeRoomModel>();

                    string nameRatePlan = ratePlan;
                    rateRoomsClosureModel.RatePlan = nameRatePlan;

                    DataSet roomsTest = new RoomFacade().getRooms(idHotel);

                    //Helper
                    GetAvailability(idHotel, ratePlan,
                           startDate, endDate, roomsTest, hotelData, ref rateRoomsClosureModel);


                    roomsClosureModel.RateRoomsClosureModelList.Add(rateRoomsClosureModel);
                }
            }
            else
            //Busqueda general por cada rateplan
            {

                foreach (DataRow ratePlanRow in ratePlans.Tables[RatePlanData.RATEPLAN_TABLE].Rows)
                {
                    DataSet roomsTest = new RoomFacade().getRooms(idHotel);

                    RateRoomsClosureModel rateRoomsClosureModel = new RateRoomsClosureModel();
                    rateRoomsClosureModel.CodeRoomModelsList = new List<CodeRoomModel>();

                    string nameRatePlan = (string)ratePlanRow[RatePlanData.FIELD_CODIGOTARIFA];
                    rateRoomsClosureModel.RatePlan = nameRatePlan;
                    //Por cada habitacion en roomsTest

                    //Helper
                    GetAvailability(idHotel,ratePlanRow[RatePlanData.FIELD_CODIGOTARIFA].ToString(),
                        startDate,endDate,roomsTest,hotelData, ref rateRoomsClosureModel);

                    roomsClosureModel.RateRoomsClosureModelList.Add(rateRoomsClosureModel);
                }//End for each rateplan
            }// End else 

            return roomsClosureModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void SaveData(int idHotel, DateTime startDate, DateTime endDate,string idRatePlan,string idTipoHabitacionHotel,string statusAvail)
        {
            //D2D Habitacion
            DataSet lockRoomTypes = GetLockRoomTypes(idHotel,idRatePlan, startDate.ToString(), endDate.ToString(), idTipoHabitacionHotel);
            //string statusAvail = "N";

            //Si ya hay cierre para ese plan y esa habitacion
            if(lockRoomTypes.Tables[0].Rows.Count != 0)
            {
                List<DateTime> closureDates = new List<DateTime>();
                List<string> closureStatus = new List<string>();
                List<DataRow> lockRowList = new List<DataRow>();
                

                //Lo hace solamente una vez , se puede quitar el for each?
                foreach (DataRow lockRoom in lockRoomTypes.Tables[0].Rows)
                {
                    lockRowList.Add(lockRoom);
                    string availabilityStatus = lockRoom["StatusAvail"].ToString();
                    DateTime startDateLock = DateTime.Parse(lockRoom["StartDate"].ToString()).Date;
                    DateTime endDateLock = DateTime.Parse(lockRoom["EndDate"].ToString()).Date;
                    //var index = lockRoomTypes.Tables[1].Rows.IndexOf(lockRoom);


                    for (DateTime i = startDateLock.Date; i <= endDateLock.Date; i = i.AddDays(1))
                    {
                        closureDates.Add(i);
                        closureStatus.Add(availabilityStatus);
                    }
                }

                //Buscar indices de la fecha de stardate y endate

                int indexStartDate = closureDates.BinarySearch(startDate.Date);
                int indexEndDate = closureDates.BinarySearch(endDate.Date);

                //Checha la opcion del usuario para actualizar o agrega nuevas fechas
                switch (statusAvail)
                {
                    #region Case Open
                    case "O":

                        //Checar los Closure Status parte izquierda
                        int countLeft = 0;
                        string statusClosureLeft = (indexStartDate < 0) ? "O" : closureStatus.ElementAt(indexStartDate);
                        string statusForNewClosureLock = String.Empty;
                        //Apartir de la fecha se cuenta que el status sea el mismo que la fecha
                        //Solamente va afectar la fecha que tenga el mismo status

                        //Left
                        int iL = indexStartDate  - 1;

                        while (iL >= 0 && closureStatus.ElementAt(iL) == statusClosureLeft)
                        {
                            countLeft++;
                            iL--;
                        }

                        if (countLeft > 0 )
                        {
                            int substractDaysForNewClosureDate = (indexStartDate > 0) ? 1 : 0;
                            DateTime newEndClosureDate = closureDates.ElementAt(indexStartDate - substractDaysForNewClosureDate);
                            DateTime newStartClosureDate = closureDates.ElementAt(indexStartDate).Subtract(TimeSpan.FromDays(countLeft));
                            //Ver si hay un registro con el status para actualizar o para  insertar

                            //Si es left se busca con el startDate de entrada en lockRowList
                           
                            int rowIndex = FindIndexLock(lockRowList,startDate);

                            //Si hay registro
                            if (rowIndex > -1)
                            {
                                //Update
                                UpdateLock(idHotel,idRatePlan,newStartClosureDate.ToString(),newEndClosureDate.ToString(),
                                    lockRowList[rowIndex]["StartDate"].ToString(),lockRowList[rowIndex]["EndDate"].ToString(),
                                    idTipoHabitacionHotel,lockRowList[rowIndex]["StatusAvail"].ToString());

                                statusForNewClosureLock = lockRowList[rowIndex]["StatusAvail"].ToString();
                                lockRowList.RemoveAt(rowIndex);
                            }
                            else
                            {
                                //Insert
                            }

                        }

                        //Right

                        //Checar los Closure Status parte derecha
                        int countRight = 0;
                        string statusClosureRight = (indexEndDate < 0)? "O" : closureStatus.ElementAt(indexEndDate);

                        //Left
                        int iR = indexEndDate + 1;

                        while (iR >= 0 && iR < closureStatus.Count && closureStatus.ElementAt(iR) == statusClosureRight)
                        {
                            countRight++;
                            iR++;
                        }

                        if(countRight > 0)
                        {
                            int addDaysForNewClosureDate = (indexEndDate < closureDates.Count - 1) ? 1 : 0;
                            DateTime newStartClosureDate = closureDates.ElementAt(indexEndDate + addDaysForNewClosureDate);
                            DateTime newEndClosureDate = closureDates.ElementAt(indexEndDate).AddDays(countRight);
                            //Ver si hay un registro con el status para actualizar o para  insertar

                            //Si es right se busca con el endDate de entrada en lockRowList
                            int rowIndex = FindIndexLock(lockRowList, endDate);

                            //Si hay un registro
                            if (rowIndex > -1)
                            {
                                //Update
                                UpdateLock(idHotel,idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),
                                   lockRowList[rowIndex]["StartDate"].ToString(), lockRowList[rowIndex]["EndDate"].ToString(),
                                   idTipoHabitacionHotel, lockRowList[rowIndex]["StatusAvail"].ToString());

                                lockRowList.RemoveAt(rowIndex);
                            }
                            else
                            {
                                //Insert
                                InsertLock(idHotel,idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),                             
                                   idTipoHabitacionHotel,statusForNewClosureLock);

                            }

                        }


                        //El Rango de fechas es de inicio a fin del array, se borran todos los registros
                        if(countLeft == 0 && countRight == 0)
                        {
                            //Borrar los Registros
                            Delete(ref lockRowList,idHotel,idRatePlan,idTipoHabitacionHotel);                           
                        }

                        //Borrar la parte de la derecha y ya actualizo la parte de la izquierda
                        if(countLeft != 0 && countRight == 0)
                        {
                            //Borrar los Registros
                            Delete(ref lockRowList, idHotel,idRatePlan,idTipoHabitacionHotel);
                        }

                        //Borrar la parte de la izquierda y ya yactualizo la parde de la derecha
                        if (countLeft == 0 && countRight != 0)
                        {
                            //Borrar los Registros
                            Delete(ref lockRowList, idHotel,idRatePlan,idTipoHabitacionHotel);
                        }

                        //Borrar la parte intermedia entre derecha y izquierda
                        if(countLeft > 0 && countRight > 0 && lockRowList.Count > 0)
                        {
                            //Borrar los Registros
                            Delete(ref lockRowList, idHotel,idRatePlan,idTipoHabitacionHotel);
                        }

                        break;
                    #endregion

                    #region Case Close
                    case "C":                       
                        UpdateCloseAndNoArrivals(idHotel,indexStartDate,indexEndDate,startDate,endDate,
                            closureStatus,closureDates,ref lockRowList,"C",idRatePlan,idTipoHabitacionHotel);
                        break;
                    #endregion
                   
                    #region No Arrivals
                    case "N":
                        UpdateCloseAndNoArrivals(idHotel, indexStartDate, indexEndDate, startDate, endDate,
                       closureStatus, closureDates, ref lockRowList,"N",idRatePlan,idTipoHabitacionHotel);
                        break;
                        #endregion
                }


            }//Termina If
            else
            {
                InsertLock(idHotel, idRatePlan, startDate.ToString(), endDate.ToString(), idTipoHabitacionHotel, statusAvail);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idAsoc"></param>
        /// <param name="idCorporativoUserChain"></param>
        /// <param name="isHotel"></param>
        /// <param name="isUsuarioHotel"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public List<RatePlansClosureModel> LoadRatePlanByIdHotel(int idHotel,int idAsoc,int idCorporativoUserChain,bool isHotel,bool isUsuarioHotel, int lang = 1)
        {
            RatePlanData ds = new RatePlanFacade()
             .GetRatePlanByIdHotel(idHotel.ToString(), lang, 0, 1, idAsociacion: idAsoc, DeleteFilter: 1);

            if(idCorporativoUserChain == 4 && (isHotel || isUsuarioHotel))
            {
                ds = RatePlanFilter("C", ds);
            }

            List<RatePlansClosureModel> list = new List<RatePlansClosureModel>();
            var ratePlanTable = ds.Tables[RatePlanData.RATEPLAN_TABLE];

            foreach(DataRow row in ratePlanTable.Rows)
            {
                //Indices para el codigo 0 u 8
                string code = row.ItemArray[0].ToString();              
                string name = row.ItemArray[11].ToString();

                string text = code + " - " + name;

                RatePlansClosureModel model = new RatePlansClosureModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;

        }

        public List<RatePlansClosureModel> LoadRatePlanByIdHotelNoSegmentsInvalids(int idHotel, int idAsoc, int idCorporativoUserChain, bool isHotel, bool isUsuarioHotel, int lang = 1)
        {
            RatePlanData ds = new RatePlanFacade()
             .GetRatePlanByIdHotel(idHotel.ToString(), lang, 0, 1, idAsociacion: idAsoc, DeleteFilter: 1);

            List<RatePlansClosureModel> list = new List<RatePlansClosureModel>();
            //var ratePlanTable = ds.Tables[RatePlanData.RATEPLAN_TABLE];

            var ratePlanTable = Conflux.Helpers.RatesPlan.RatesPlanHelper.GetFilteredRatePlans(ds);

            foreach (DataRow row in ratePlanTable)
            {
                //Indices para el codigo 0 u 8
                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[11].ToString();

                string text = code + " - " + name;

                RatePlansClosureModel model = new RatePlansClosureModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;

        }

        public List<RatePlansClosureModel> LoadAllRatePlansByIdHotelNoSegmentsInvalids(int idHotel, int idAsoc, int idCorporativoUserChain, bool isHotel, bool isUsuarioHotel, int lang = 1)
        {
            RatePlanData ds = new RatePlanFacade()
             .GetRatePlanByIdHotel(idHotel.ToString(), lang, 0, 1, idAsociacion: idAsoc, DeleteFilter: -1);

            List<RatePlansClosureModel> list = new List<RatePlansClosureModel>();
            //var ratePlanTable = ds.Tables[RatePlanData.RATEPLAN_TABLE];

            var ratePlanTable = Conflux.Helpers.RatesPlan.RatesPlanHelper.GetFilteredRatePlans(ds);

            foreach (DataRow row in ratePlanTable)
            {
                //Indices para el codigo 0 u 8
                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[11].ToString();

                string text = code + " - " + name;

                RatePlansClosureModel model = new RatePlansClosureModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;

        }



        public List<RatePlansClosureModel> LoadRatePlanByIdHotelNoLinks(int idHotel, int idAsoc, int idCorporativoUserChain, bool isHotel, bool isUsuarioHotel, int lang = 1)
        {
            RatePlanData ds = new RatePlanFacade()
             .GetRatePlanByIdHotel(idHotel.ToString(), lang, 0, 1, idAsociacion: idAsoc, DeleteFilter: 1);

            LinkRatePlanData links = new LinkRatePlanData();
            links = new LinkRatePlanFacade().getList(idHotel, lang, idAsociacion: idAsoc);

            ds = RatePlanFilter(ds,links);

            List<RatePlansClosureModel> list = new List<RatePlansClosureModel>();

            var ratePlanTable = Conflux.Helpers.RatesPlan.RatesPlanHelper.GetFilteredRatePlans(ds);

            //var ratePlanTable = ds.Tables[RatePlanData.RATEPLAN_TABLE];

            foreach (DataRow row in ratePlanTable)
            {
                //Indices para el codigo 0 u 8
                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[11].ToString();

                string text = code + " - " + name;

                RatePlansClosureModel model = new RatePlansClosureModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;

        }

        public List<RatePlansClosureModel> LoadPromosByIdHotel(int idhotel)
        {
            string filterRatesPlansPromos = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"; // Ver si va quedar el mismo filtro

            List<DataRow> promos = APIServices.Conflux.Helpers.RatesPlan.RatesPlanHelper.GetRatePlansPromosByHotel(idhotel, filterRatesPlansPromos);

            List<RatePlansClosureModel> list = new List<RatePlansClosureModel>();

            foreach (DataRow row in promos)
            {
                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[11].ToString();

                string text = code + " - " + name;

                RatePlansClosureModel model = new RatePlansClosureModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;
        }

        public List<RatePlansClosureModel> LoadAllPromosByIdHotel(int idhotel)
        {
            
            List<DataRow> promos = APIServices.Conflux.Helpers.RatesPlan.RatesPlanHelper.GetAllRatePlansPromosByHotel(idhotel);

            List<RatePlansClosureModel> list = new List<RatePlansClosureModel>();

            foreach (DataRow row in promos)
            {
                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[11].ToString();

                string text = code + " - " + name;

                RatePlansClosureModel model = new RatePlansClosureModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public List<RoomsModel> LoadRoomsByIdHotel(int idHotel, int lang = 1)
        {
            List<RoomsModel> list = new List<RoomsModel>();
            RoomsHotelData ds = new RoomFacade().getRooms(idHotel,lang);

            var roomsTable = ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL];

            foreach(DataRow row in roomsTable.Rows)
            {
                //Indices para el codigo 0
                //Indices para la habitacion 2 u 9

                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[9].ToString();
                string shortName = row.ItemArray[22].ToString();

                string text = shortName + " - " + name;

                RoomsModel model = new RoomsModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }

            return list;
        }

        public List<RoomsModel> LoadRoomsByHotelIdHotelNoLinks(int idHotel, int lang = 1)
        {
            List<RoomsModel> list = new List<RoomsModel>();
            RoomsHotelData ds = new RoomFacade().getRooms(idHotel, lang);

            LinkRoomTypeData links = new LinkRoomTypeData();
            links = new LinkRoomsFacade().getList(idHotel, lang);

            DataView dv;

            foreach (DataRow r in ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL].Rows)
            {
                dv = links.Tables[LinkRoomTypeData.TABLE_LINKROOM].DefaultView;
                dv.RowFilter = LinkRoomTypeData.FIELD_TargetRoom + "=" + r[RoomsHotelData.FLD_ID_ROOM_HOTEL];

                if (dv.Count > 0)
                {
                    r.Delete();
                }
            }

            ds.AcceptChanges();

            var roomsTable = ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL];

            foreach (DataRow row in roomsTable.Rows)
            {
                //Indices para el codigo 0
                //Indices para la habitacion 2 u 9

                string code = row.ItemArray[0].ToString();
                string name = row.ItemArray[9].ToString();
                string shortName = row.ItemArray[22].ToString();

                string text = shortName + " - " + name;

                RoomsModel model = new RoomsModel()
                {
                    Value = code,
                    Text = text
                };

                list.Add(model);

            }



            return list;
        }



        #endregion

        #region Helpers General
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
        /// <param name="lockRowList"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private int FindIndexLock (List<DataRow> lockRowList,DateTime date)
        {
            for (int i = 0; i < lockRowList.Count; i++)
            {
                DateTime start = Convert.ToDateTime(lockRowList[i]["StartDate"].ToString());
                DateTime end = Convert.ToDateTime(lockRowList[i]["EndDate"].ToString());

                if (date.Date >= start.Date &&  date.Date <= end.Date)
                {
                    return i;
                }

            }
            return -1;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockRowList"></param>
        /// <param name="idHotel"></param>
        /// <param name="idRatePlan"></param>
        /// <param name="idTipoHabitacionHotel"></param>
        private void Delete(ref List<DataRow> lockRowList,in int idHotel,in string idRatePlan, in string idTipoHabitacionHotel)
        {
            foreach (DataRow row in lockRowList)
            {
                //Delete
                DateTime start = Convert.ToDateTime(row["StartDate"].ToString());
                DateTime end = Convert.ToDateTime(row["EndDate"].ToString());

                DeleteLock(idHotel,idRatePlan, start.ToString(), end.ToString(),idTipoHabitacionHotel);
            }

            lockRowList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="indexStartDate"></param>
        /// <param name="indexEndDate"></param>
        /// <param name="closureStatus"></param>
        /// <param name=""></param>
        private void UpdateCloseAndNoArrivals(in int idHotel, in int indexStartDate, in int indexEndDate,
            DateTime startDate, DateTime endDate, in List<string> closureStatus, in List<DateTime> closureDates,
            ref List<DataRow> lockRowList, in string statusAvail, in string idRatePlan, in string idTipoHabitacionHotel)
        {
            int countLeftClose = 0;

            int iLClose = indexStartDate - 1;
            string statusClosureLeftClose = (indexStartDate < 0) ? "O" : closureStatus.ElementAt(indexStartDate);
            string statusForNewClosureLock = String.Empty;
            DateTime? startDateLock = null;
            DateTime? endDateLock = null;

            while (iLClose >= 0 && closureStatus.ElementAt(iLClose) == statusClosureLeftClose)
            {
                countLeftClose++;
                iLClose--;
            }

            int countRightClose = 0;
            string statusClosureRightClose = (indexEndDate < 0) ? "O" : closureStatus.ElementAt(indexEndDate);

            //Left
            int iRClose = indexEndDate + 1;

            while (iRClose >= 0 && iRClose < closureStatus.Count && closureStatus.ElementAt(iRClose) == statusClosureRightClose)
            {
                countRightClose++;
                iRClose++;
            }

            //Se actualiza izquierda
            if (countLeftClose == 0)
            {
                DateTime newStartClosureDate = startDate.Date;
                DateTime newEndClosureDate = endDate.Date;

                int rowIndex = FindIndexLock(lockRowList, startDate);

                if (rowIndex > -1)
                {
                    UpdateLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),
                      lockRowList[rowIndex]["StartDate"].ToString(), lockRowList[rowIndex]["EndDate"].ToString(),
                      idTipoHabitacionHotel, statusAvail);
                   
                    statusForNewClosureLock = lockRowList[rowIndex]["StatusAvail"].ToString();
                    startDateLock = Convert.ToDateTime(lockRowList[rowIndex]["StartDate"].ToString());
                    endDateLock = Convert.ToDateTime(lockRowList[rowIndex]["EndDate"].ToString());
                    lockRowList.RemoveAt(rowIndex);
                }
                //Si esta abierto para esa fecha se agrega un nuevo cierre
                else
                {
                    //Insert
                    InsertLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(), idTipoHabitacionHotel, statusAvail);
                }
            }

            //El Rango de fechas es de inicio a una del array, se borran todos los registros
            if (countLeftClose == 0 && countRightClose == 0)
            {
                //Borrar los Registros

                if (lockRowList.Count > 0)
                    Delete(ref lockRowList, idHotel, idRatePlan, idTipoHabitacionHotel);
            }

            //Se va actualizar izquierda 
            if (countLeftClose != 0 && countRightClose == 0)
            {
                DateTime newStartClosureDate = startDate.Date;
                DateTime newEndClosureDate = endDate.Date;

                int rowIndex = FindIndexLock(lockRowList, startDate);

                if (rowIndex > -1)
                {
                    UpdateLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),
                      lockRowList[rowIndex]["StartDate"].ToString(), lockRowList[rowIndex]["EndDate"].ToString(),
                      idTipoHabitacionHotel, statusAvail);

                   
                    statusForNewClosureLock = lockRowList[rowIndex]["StatusAvail"].ToString();
                    startDateLock = Convert.ToDateTime(lockRowList[rowIndex]["StartDate"].ToString());
                    endDateLock = Convert.ToDateTime(lockRowList[rowIndex]["EndDate"].ToString());
                    lockRowList.RemoveAt(rowIndex);
                }
                //Si esta abierto para esa fecha se agrega un nuevo cierre
                else
                {
                    //Insert
                    InsertLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(), idTipoHabitacionHotel, statusAvail);
                }

            }

            //Actualiza izquierda
            if (countLeftClose > 0)
            {
                int substractDaysForNewClosureDate = (indexStartDate > 0) ? 1 : 0;
                DateTime newEndClosureDate = closureDates.ElementAt(indexStartDate - substractDaysForNewClosureDate);
                DateTime newStartClosureDate = closureDates.ElementAt(indexStartDate).Subtract(TimeSpan.FromDays(countLeftClose));
             

                int rowIndex = FindIndexLock(lockRowList, startDate);

                //Si hay registro
                if (rowIndex > -1)
                {
                    //Update
                    UpdateLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),
                        lockRowList[rowIndex]["StartDate"].ToString(), lockRowList[rowIndex]["EndDate"].ToString(),
                        idTipoHabitacionHotel, lockRowList[rowIndex]["StatusAvail"].ToString());

                    statusForNewClosureLock = lockRowList[rowIndex]["StatusAvail"].ToString();
                    startDateLock = Convert.ToDateTime(lockRowList[rowIndex]["StartDate"].ToString());
                    endDateLock = Convert.ToDateTime(lockRowList[rowIndex]["EndDate"].ToString());
                    lockRowList.RemoveAt(rowIndex);
                }
                //Ver si se necesita agregar un nuevo registro
                else
                {
                    //Insert
                }

                //Si las nuevas fechas estan dentro del rango guardado se agrega uno nuevo
                if(startDate.Date > startDateLock.Value.Date && endDate.Date < endDateLock.Value.Date)
                {
                    InsertLock(idHotel, idRatePlan, startDate.ToString(), endDate.ToString(),
                       idTipoHabitacionHotel, statusAvail);
                }
                //Si las fechas estan fuera del rango
                else if(newStartClosureDate.Date < startDate.Date && newEndClosureDate.Date < endDate.Date)
                {
                    InsertLock(idHotel, idRatePlan, newStartClosureDate.ToString(),newEndClosureDate.ToString(),
                     idTipoHabitacionHotel, statusForNewClosureLock);
                }


            }

            //Actualiza derecha
            if (countRightClose > 0)
            {
                int addDaysForNewClosureDate = (indexEndDate < closureDates.Count - 1) ? 1 : 0;
                DateTime newStartClosureDate = closureDates.ElementAt(indexEndDate + addDaysForNewClosureDate);
                DateTime newEndClosureDate = closureDates.ElementAt(indexEndDate).AddDays(countRightClose);

                //Si es right se busca con el endDate de entrada en lockRowList
                int rowIndex = FindIndexLock(lockRowList, endDate);

                //Si hay un registro
                if (rowIndex > -1)
                {
                    //Update
                    UpdateLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),
                       lockRowList[rowIndex]["StartDate"].ToString(), lockRowList[rowIndex]["EndDate"].ToString(),
                       idTipoHabitacionHotel, lockRowList[rowIndex]["StatusAvail"].ToString());

                    lockRowList.RemoveAt(rowIndex);
                }
                else
                {
                    //Insert
                    InsertLock(idHotel, idRatePlan, newStartClosureDate.ToString(), newEndClosureDate.ToString(),
                       idTipoHabitacionHotel, statusForNewClosureLock);

                }

            }



        }

        /// <summary>
        /// Filter RatePlans By Segment
        /// </summary>
        /// <param name="segmentType"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private RatePlanData RatePlanFilter(string segmentType,RatePlanData ds)
        {
            DataView dv2 = null;

            foreach(DataRow row in ds.Tables["RatePlans"].Rows)
            {
                dv2 = ds.Tables["RatePlans"].DefaultView;
                dv2.RowFilter = "Segment" + "=" + "'" + segmentType + "'";

                if(dv2.Count > 0 && row["Segment"].ToString().Equals(segmentType))
                {
                    row.Delete();
                }

            }

            ds.AcceptChanges();

            return ds;
        }

        private RatePlanData RatePlanFilter(RatePlanData ds, LinkRatePlanData links)
        {
            DataView dv;

            foreach (DataRow row in ds.Tables[RatePlanData.RATEPLAN_TABLE].Rows)
            {
                dv = links.Tables[LinkRatePlanData.TABLE_LINKRATEPLAN].DefaultView;
                dv.RowFilter = LinkRatePlanData.FIELD_TargetRatePlan + "='" + row[RatePlanData.FIELD_IDRATEPLAN] + "'";

                if (dv.Count > 0 )
                {
                    row.Delete();
                }
            }

            ds.AcceptChanges();

            return ds;
        }

        #endregion


        #region Helpers DB
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idRatePlan"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="roomsTest"></param>
        /// <param name="hotelData"></param>
        /// <param name="rateRoomsClosureModel"></param>
        private void GetAvailability(in int idHotel,in string idRatePlan,DateTime startDate, 
            in DateTime endDate,in DataSet roomsTest,in HotelDatos hotelData,
            ref RateRoomsClosureModel rateRoomsClosureModel)
        {
            DataSet lockRoomTypes;

            foreach (DataRow room in roomsTest.Tables[RoomsHotelData.TBL_ROOM_HOTEL].Rows)
            {
                // GetLockRoomTypes(MyBase.cInfoActual.Hotel, drrateplan(dsrateplans.FIELD_CODIGOTARIFA), dateStart, dateEnd, drroom(dsrooms.FLD_ID_ROOM_HOTEL))
                lockRoomTypes = GetLockRoomTypes(idHotel,idRatePlan
                    , startDate.ToString(), endDate.ToString(), room[RoomsHotelData.FLD_ID_ROOM_HOTEL].ToString());

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
                    for (int i = 0; i <= diff; i++)
                    {
                        codeRoomModel.Status[i] = "C";
                    }

                    rateRoomsClosureModel.CodeRoomModelsList.Add(codeRoomModel);

                }
                else
                {
                    // Si hay cierre o no llegada para esa fecha en la habitacion
                    if (dtLock.Rows.Count != 0)
                    {
                        string roomName = room[RoomsHotelData.FLD_NOMBRE].ToString();
                        string roomCode = room[RoomsHotelData.FLD_ROOM_CODE].ToString();

                        CodeRoomModel codeRoomModel = new CodeRoomModel();
                        codeRoomModel.Code = roomCode;
                        codeRoomModel.RoomName = roomName;

                        int i = 0;
                        int diff = (endDate.Date - startDate.Date).Days + 1;
                        string[] rangeDays = new string[diff];
                        codeRoomModel.Status = new string[diff];

                        //Por cada fila en la que se haya guardado un cierre o un no llegada
                        foreach (DataRow drLock in dtLock.Rows)
                        {
                            DateTime startDay = Convert.ToDateTime(drLock["StartDate"].ToString()).Date;
                            DateTime endDay = Convert.ToDateTime(drLock["EndDate"].ToString()).Date;
                            //int diffDatesLock = (endDay.Date - startDay.Date).Days + 1;
                            int diffDatesLock = (endDate.Date - startDate.Date).Days;
                            
                            for(int j = 0;  j <= diffDatesLock; j++)
                            {
                                rangeDays[j] = (rangeDays[j] == null) ? "" + startDate.AddDays(j).ToString("MM/dd/yyyy") + "," : rangeDays[j];

                                //TODO: Revisar la condicion
                                string statusStrings = ((startDate.AddDays(j).Date <= endDay && startDate.AddDays(j).Date >= startDay))
                                    ? drLock["StatusAvail"].ToString() : "";

                                if(rangeDays[j].Length == 12)
                                {
                                    //rangeDays[j] = rangeDays[j];

                                    if(!String.IsNullOrEmpty(statusStrings))
                                    {
                                        rangeDays[j] = rangeDays[j].Replace("*", statusStrings);
                                    }

                                }
                                else
                                {
                                    rangeDays[j] += (!string.IsNullOrEmpty(statusStrings)) ? statusStrings : "*";
                                }
                               // rangeDays[j] += (!string.IsNullOrEmpty(statusStrings)) ? statusStrings : "*";

                                string rangeDaysSplit = rangeDays[j].Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries)[1];

                                codeRoomModel.Status[j] = (rangeDaysSplit == "*") ? "O" : rangeDaysSplit;

                                i++;
                            }

                        }

                        rateRoomsClosureModel.CodeRoomModelsList.Add(codeRoomModel);


                    }
                    //Si no hay cierre o no llegada para esa fecha en la habitacion
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

            } // End for each room
        }


        /// <summary>
        ///  Update Lock Room
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idRatePlan"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="oldStartDate"></param>
        /// <param name="oldEndDate"></param>
        /// <param name="idTipoHabitacionHotel"></param>
        /// <param name="statusAvail"></param>
        private void UpdateLock(in int idHotel, in string idRatePlan, in string startDate,
            in string endDate,in string oldStartDate,in string oldEndDate,in string idTipoHabitacionHotel,in string statusAvail)
        {
           
            string connectionString = ConfigurationManager.AppSettings["HotelConnectionString"];
            SqlCommand command = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
           
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spUpdateLockRoomTypes";
                command.Connection = sqlConnection;              
                command.Parameters.Clear();

                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime));             
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@OldStart", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@OldEnd", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@idhotel", SqlDbType.Int));            
                command.Parameters.Add(new SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4));            
                command.Parameters.Add(new SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@StatusAvail", SqlDbType.Char,1));

                DateTime dateStart = Convert.ToDateTime(startDate);               
                DateTime dateEnd = Convert.ToDateTime(endDate);

                command.Parameters["@StartDate"].Value = dateStart.ToString("yyyy/MM/dd");
                command.Parameters["@EndDate"].Value = dateEnd.ToString("yyyy/MM/dd");

                if(!String.IsNullOrEmpty(oldStartDate))
                {
                    DateTime oldStart = Convert.ToDateTime(oldStartDate);
                    command.Parameters["@OldStart"].Value = oldStart.ToString("yyyy/MM/dd");

                }
                else
                {
                    command.Parameters["@OldStart"].Value = DBNull.Value;

                }

                if (!String.IsNullOrEmpty(oldEndDate))
                {
                    DateTime oldEnd = Convert.ToDateTime(oldEndDate);
                    command.Parameters["@OldEnd"].Value = oldEnd.ToString("yyyy/MM/dd");

                }
                else
                {
                    command.Parameters["@OldEnd"].Value = DBNull.Value;

                }

                command.Parameters["@idhotel"].Value = idHotel;
                command.Parameters["@IdRatePlan"].Value = idRatePlan;
                command.Parameters["@idTipoHabitacion_Hotel"].Value = Int32.Parse(idTipoHabitacionHotel);
                command.Parameters["@StatusAvail"].Value = statusAvail;

                sqlConnection.Open();
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string msg = ex.Message;           
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }


        }

        /// <summary>
        /// Delete Lock Room
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idRatePlan"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="idTipoHabitacionHotel"></param>
        private void DeleteLock(in int idHotel, in string idRatePlan, in string startDate,
            in string endDate, in string idTipoHabitacionHotel)
        {
            string connectionString = ConfigurationManager.AppSettings["HotelConnectionString"];
            SqlCommand command = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spDeleteLockRoomTypes";
                command.Connection = sqlConnection;
                command.Parameters.Clear();

                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime));              
                command.Parameters.Add(new SqlParameter("@idhotel", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4));
                command.Parameters.Add(new SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int));
                
                if(!String.IsNullOrEmpty(startDate))
                {
                    DateTime dateStart = Convert.ToDateTime(startDate);
                    command.Parameters["@StartDate"].Value = dateStart.ToString("yyyy/MM/dd");
                }
                else
                {
                    command.Parameters["@StartDate"].Value = DBNull.Value;

                }

                DateTime dateEnd = Convert.ToDateTime(endDate);

                command.Parameters["@EndDate"].Value = dateEnd.ToString("yyyy/MM/dd");
                command.Parameters["@idhotel"].Value = idHotel;
                command.Parameters["@IdRatePlan"].Value = idRatePlan;
                command.Parameters["@idTipoHabitacion_Hotel"].Value = Int32.Parse(idTipoHabitacionHotel);

                sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        /// <summary>
        /// Insert Lock Room
        /// </summary>
        /// <param name="idHotel"></param>
        /// <param name="idRatePlan"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="idTipoHabitacionHotel"></param>
        /// <param name="statusAvail"></param>
        private void InsertLock(in int idHotel, in string idRatePlan, in string startDate,
            in string endDate, in string idTipoHabitacionHotel, in string statusAvail)
        {
            string connectionString = ConfigurationManager.AppSettings["HotelConnectionString"];
            SqlCommand command = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spLockRoomTypes";
                command.Connection = sqlConnection;
                command.Parameters.Clear();

                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime));             
                command.Parameters.Add(new SqlParameter("@idhotel", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@IdRatePlan", SqlDbType.NVarChar, 4));
                command.Parameters.Add(new SqlParameter("@idTipoHabitacion_Hotel", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@StatusAvail", SqlDbType.Char, 1));

                DateTime dateStart = Convert.ToDateTime(startDate);
                DateTime dateEnd = Convert.ToDateTime(endDate);

                command.Parameters["@StartDate"].Value = dateStart.ToString("yyyy/MM/dd");
                command.Parameters["@EndDate"].Value = dateEnd.ToString("yyyy/MM/dd");
                command.Parameters["@idhotel"].Value = idHotel;
                command.Parameters["@IdRatePlan"].Value = idRatePlan;
                command.Parameters["@idTipoHabitacion_Hotel"].Value = Int32.Parse(idTipoHabitacionHotel);
                command.Parameters["@StatusAvail"].Value = statusAvail;

                sqlConnection.Open();
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        #endregion
    }
}
