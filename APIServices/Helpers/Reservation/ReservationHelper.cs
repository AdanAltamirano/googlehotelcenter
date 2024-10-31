using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using APIServices.Models;
using APIServices.Models.DTO.Log;

namespace APIServices.Helpers.Reservation
{
    public static class ReservationHelper
    {
        public static Reservaciones GetReservation(int idReservacion)
        {
            Reservaciones reservaciones = null;

            using(var context = new OzHotelesEntities())
            {
                reservaciones = context.Reservaciones.FirstOrDefault(r => r.idReservacion == idReservacion);
            }

            return reservaciones;
        }

        public static ReservationDetailLog ValidateReservationChangesCRS(XDocument dataBefore, XDocument dataAfter)
        {
            ReservationDetailLog reservationDetailLog = new ReservationDetailLog();

            //Revisar Nombre
            XElement customerBefore = dataBefore.Root.Element("Customer");
            XElement customerAfter = dataAfter.Root.Element("Customer");

            string fullNameBefore = customerBefore.Element("Name").Value.Trim() + " " + customerBefore.Element("LastName").Value.Trim();
            string fullNameAfter = customerAfter.Element("Name").Value.Trim() + " " + customerAfter.Element("LastName").Value.Trim();

            //Se Comparan Nombres
            if (!string.Equals(fullNameBefore, fullNameAfter))
            {
                //Se Asigna el valor

                CustomerLog customerLogBefore = new CustomerLog() { FullName = fullNameBefore };
                CustomerLog customerLogAfter = new CustomerLog() { FullName = fullNameAfter };

                reservationDetailLog.CustomerBefore = customerLogBefore;
                reservationDetailLog.CustomerAfter = customerLogAfter;
            }

            //Revisar CheckIn y CheckOut
            DateTime checkInBefore = DateTime.Parse(dataBefore.Root.Element("CheckIn").Value).Date;
            DateTime checkOutBefore = DateTime.Parse(dataBefore.Root.Element("CheckOut").Value).Date;

            DateTime checkInAfter = DateTime.Parse(dataAfter.Root.Element("CheckIn").Value).Date;
            DateTime checkOutAfter = DateTime.Parse(dataAfter.Root.Element("CheckOut").Value).Date;

            //Se Comparan Fechas
            if(DateTime.Compare(checkInBefore,checkInAfter) != 0 && DateTime.Compare(checkOutBefore, checkOutAfter) != 0)
            {
                CheckDatesLog checkDatesLogBefore = new CheckDatesLog() { CheckIn = checkInBefore, CheckOut = checkOutBefore };
                CheckDatesLog checkDatesLogAfter = new CheckDatesLog() { CheckIn = checkInAfter, CheckOut = checkOutAfter };

                reservationDetailLog.CheckDatesBefore = checkDatesLogBefore;
                reservationDetailLog.CheckDatesAfter = checkDatesLogAfter;
            }

            //Se Comparan Habitaciones
            RoomsCRSLog(dataBefore, dataAfter, ref reservationDetailLog);

            //Se Compara el Total de la Reserva

            XElement totalDetailsBefore = dataBefore.Root.Element("TotalDetails");
            XElement totalDetailsAfter = dataAfter.Root.Element("TotalDetails");

            double totalBefore = Double.Parse(totalDetailsBefore.Element("Total").Value);
            double totalAfter = Double.Parse(totalDetailsAfter.Element("Total").Value);

            string currencyBefore = totalDetailsBefore.Element("Currency").Value;
            string currencyAfter = totalDetailsAfter.Element("Currency").Value;

            if (totalBefore != totalAfter)
            {
                TotalDetailsLog totalDetailsLogBefore = new TotalDetailsLog() { Total = totalBefore, Currency = currencyBefore };
                TotalDetailsLog totalDetailsLogAfter = new TotalDetailsLog() { Total = totalAfter, Currency = currencyAfter };

                reservationDetailLog.TotalDetailsBefore = totalDetailsLogBefore;
                reservationDetailLog.TotalDetailsAfter = totalDetailsLogAfter;
            }

            return reservationDetailLog;
        }

        public static ReservationDetailLog ValidateReservationChangesUVCC(XDocument dataBefore,XDocument dataAfter)
        {

            ReservationDetailLog reservationDetailLog = new ReservationDetailLog();

            //Son xml's diferentes uno es Hotel_Display_RS para el viejo y reqHotelModifyNoRestrictions para el nuevo

            //Se saca moneda
            string currencyBefore = dataBefore.Root.Element("HotelDisplay").Element("Reservation").Element("Money").Value;
            string currencyAfter = dataAfter.Root.Element("Reservation").Element("CurrencyCode").Value;

            //Revisar Nombre
            XElement customerBefore = dataBefore.Root.Element("HotelDisplay").Element("Customer");
            XElement customerAfter = dataAfter.Root.Element("Client");

            string fullNameBefore = customerBefore.Element("FirstName").Value.Trim() + " " + customerBefore.Element("LastName").Value.Trim();
            string fullNameAfter = customerAfter.Element("Name").Value.Trim() + " " + customerAfter.Element("LastName").Value.Trim();

            //Se Comparan Nombres
            if (!string.Equals(fullNameBefore, fullNameAfter))
            {
                //Se Asigna el valor

                CustomerLog customerLogBefore = new CustomerLog() { FullName = fullNameBefore };
                CustomerLog customerLogAfter = new CustomerLog() { FullName = fullNameAfter };

                reservationDetailLog.CustomerBefore = customerLogBefore;
                reservationDetailLog.CustomerAfter = customerLogAfter;
            }

            //Revisar CheckIn y CheckOut
            DateTime checkInBefore = DateTime.Parse(dataBefore.Root.Element("HotelDisplay").Element("Reservation").Element("CheckInDate").Value).Date;
            DateTime checkOutBefore = DateTime.Parse(dataBefore.Root.Element("HotelDisplay").Element("Reservation").Element("CheckOutDate").Value).Date;

            int[] checkInAfterArray = dataAfter.Root.Element("Reservation").Element("CheckInDate").Value.Split('/').Select(Int32.Parse).ToArray();
            int[] checkOutAfterArray = dataAfter.Root.Element("Reservation").Element("CheckOutDate").Value.Split('/').Select(Int32.Parse).ToArray();

            DateTime checkInAfter = new DateTime(checkInAfterArray[2], checkInAfterArray[1], checkInAfterArray[0]).Date;
            DateTime checkOutAfter = new DateTime(checkOutAfterArray[2], checkOutAfterArray[1], checkOutAfterArray[0]).Date;

            //Se Comparan Fechas
            if (DateTime.Compare(checkInBefore, checkInAfter) != 0 && DateTime.Compare(checkOutBefore, checkOutAfter) != 0)
            {
                CheckDatesLog checkDatesLogBefore = new CheckDatesLog() { CheckIn = checkInBefore, CheckOut = checkOutBefore };
                CheckDatesLog checkDatesLogAfter = new CheckDatesLog() { CheckIn = checkInAfter, CheckOut = checkOutAfter };

                reservationDetailLog.CheckDatesBefore = checkDatesLogBefore;
                reservationDetailLog.CheckDatesAfter = checkDatesLogAfter;
            }

            //Se Comparan Habitaciones

            RoomsUVCCLog(dataBefore, dataAfter, currencyBefore, currencyAfter, ref reservationDetailLog);

            //Se Compara el Total de la Reserva
            XElement totalDetailsBefore = dataBefore.Root.Element("HotelDisplay").Element("Reservation");
            XElement totalDetailsAfter = dataAfter.Root.Element("Reservation");

            double totalBefore = Double.Parse(totalDetailsBefore.Element("Total").Value);
            double totalAfter = Double.Parse(totalDetailsAfter.Element("Total").Value);

            if (totalBefore != totalAfter)
            {
                TotalDetailsLog totalDetailsLogBefore = new TotalDetailsLog() { Total = totalBefore, Currency = currencyBefore };
                TotalDetailsLog totalDetailsLogAfter = new TotalDetailsLog() { Total = totalAfter, Currency = currencyAfter };

                reservationDetailLog.TotalDetailsBefore = totalDetailsLogBefore;
                reservationDetailLog.TotalDetailsAfter = totalDetailsLogAfter;
            }


            return reservationDetailLog;

        }


        private static void RoomsCRSLog(XDocument dataBefore, XDocument dataAfter, ref ReservationDetailLog reservationDetailLog)
        {
            #region Habitaciones
            //Revisar Habitaciones
            List<XElement> roomsBefore = dataBefore.Root.Element("RoomDetails").Elements("RoomDetails").ToList();
            List<XElement> roomsAfter = dataAfter.Root.Element("RoomDetails").Elements("RoomDetails").ToList();

            List<RoomDetailsLog> roomDetailsLogListBefore = new List<RoomDetailsLog>();
            List<RoomDetailsLog> roomDetailsLogListAfter = new List<RoomDetailsLog>();

            int totalRoomsOccupied = roomsAfter.Count;
            //bool hasRoomChange = false;

            for (int i = 0; i < totalRoomsOccupied; i++)
            {
                bool hasRoomChange = false;

                RoomDetailsLog roomDetailsLogBefore = new RoomDetailsLog();
                RoomDetailsLog roomDetailsLogAfter = new RoomDetailsLog();

                //Revisar Adultos y Extra Adultos
                int adultsBefore = Int32.Parse(roomsBefore[i].Element("Adults").Value);
                int adultsAfter = Int32.Parse(roomsAfter[i].Element("Adults").Value);
                int extraAdultsBefore = Int32.Parse(roomsBefore[i].Element("ExtraAdults").Value);
                int extraAdultsAfter = Int32.Parse(roomsAfter[i].Element("ExtraAdults").Value);

                if (adultsBefore != adultsAfter)
                {
                    roomDetailsLogBefore.Adults = adultsBefore;
                    roomDetailsLogAfter.Adults = adultsAfter;
                    hasRoomChange = true;
                }

                if (extraAdultsBefore != extraAdultsAfter)
                {
                    roomDetailsLogBefore.ExtraAdults = extraAdultsBefore;
                    roomDetailsLogAfter.ExtraAdults = extraAdultsAfter;
                    hasRoomChange = true;
                }

                //Revisar Ninios, Extra Ninios y Edades
                int childrensBefore = Int32.Parse(roomsBefore[i].Element("Childrens").Value);
                int childrensAfter = Int32.Parse(roomsAfter[i].Element("Childrens").Value);
                int extraChildrensBefore = Int32.Parse(roomsBefore[i].Element("ExtraChildrens").Value);
                int extraChildrensAfter = Int32.Parse(roomsAfter[i].Element("ExtraChildrens").Value);
                string childrenAgesBefore = roomsBefore[i].Element("AgeChildren").Value ?? string.Empty;
                string childrenAgesAfter = roomsAfter[i].Element("AgeChildren").Value ?? string.Empty;

                if (childrensBefore != childrensAfter)
                {
                    roomDetailsLogBefore.Childrens = childrensBefore;
                    roomDetailsLogAfter.Childrens = childrensAfter;
                    hasRoomChange = true;
                }

                if (extraChildrensBefore != extraChildrensAfter)
                {
                    roomDetailsLogBefore.ExtraAdults = extraAdultsBefore;
                    roomDetailsLogAfter.ExtraAdults = extraAdultsAfter;
                    hasRoomChange = true;
                }


                int totalChildrensBeforeWithExtras = childrensBefore + extraChildrensBefore;
                int totalChildrensAfterWithExtras = childrensAfter + extraChildrensAfter;

                if ((totalChildrensBeforeWithExtras > 0 || totalChildrensAfterWithExtras > 0) && !string.Equals(childrenAgesBefore, childrenAgesAfter))
                {
                    roomDetailsLogBefore.AgeChildren = childrenAgesBefore;
                    roomDetailsLogAfter.AgeChildren = childrenAgesAfter;
                    hasRoomChange = true;
                }

                //Revisar Total,TotalNR por Habitacion
                double totalBefore = Double.Parse(roomsBefore[i].Element("Total").Value);
                double totalAfter = Double.Parse(roomsAfter[i].Element("Total").Value);
                double totalNRBefore = Double.Parse(roomsBefore[i].Element("TotalNR").Value);
                double totalNRAfter = Double.Parse(roomsAfter[i].Element("TotalNR").Value);
                string currencyBefore = roomsBefore[i].Element("Currency").Value;
                string currencyAfter = roomsAfter[i].Element("Currency").Value;

                if (totalBefore != totalAfter)
                {
                    roomDetailsLogBefore.Total = totalBefore;
                    roomDetailsLogAfter.Total = totalAfter;
                    hasRoomChange = true;
                }

                if (totalNRBefore != totalNRAfter)
                {
                    roomDetailsLogBefore.TotalNR = totalNRBefore;
                    roomDetailsLogAfter.TotalNR = totalNRAfter;
                    hasRoomChange = true;
                }

                roomDetailsLogBefore.Currency = currencyBefore;
                roomDetailsLogAfter.Currency = currencyAfter;

                if (totalBefore != totalAfter)
                {
                    //Revisar Precios por habitacion
                    List<XElement> priceDetailsBefore = roomsBefore[i].Element("PriceDetails").Elements("RoomPriceDetails").ToList();
                    List<XElement> priceDetailsAfter = roomsAfter[i].Element("PriceDetails").Elements("RoomPriceDetails").ToList();

                    //Hay diferentes nodos de precio
                    if (priceDetailsBefore.Count != priceDetailsAfter.Count)
                    {
                        List<RoomPriceDetailsLog> roomPriceDetailsBeforeLogsList = new List<RoomPriceDetailsLog>();
                        List<RoomPriceDetailsLog> roomPriceDetailsAfterLogsList = new List<RoomPriceDetailsLog>();

                        //Before
                        foreach (XElement priceDetailBefore in priceDetailsBefore)
                        {
                            RoomPriceDetailsLog roomPriceDetailsLogTemp = new RoomPriceDetailsLog()
                            {
                                Price = Double.Parse(priceDetailBefore.Element("Price").Value),
                                ExtraPrice = Double.Parse(priceDetailBefore.Element("ExtraPrice").Value),
                                PriceNR = Double.Parse(priceDetailBefore.Element("PriceNR").Value),
                                ExtraPriceNR = Double.Parse(priceDetailBefore.Element("ExtraPriceNR").Value),
                                CheckIn = DateTime.Parse(priceDetailBefore.Element("CheckIn").Value).Date,
                                CheckOut = DateTime.Parse(priceDetailBefore.Element("CheckOut").Value).Date,
                                Currency = priceDetailBefore.Element("Currency").Value

                            };

                            roomPriceDetailsBeforeLogsList.Add(roomPriceDetailsLogTemp);
                        }

                        roomDetailsLogBefore.PriceDetails = roomPriceDetailsBeforeLogsList;

                        //After
                        foreach (XElement priceDetailAfter in priceDetailsAfter)
                        {
                            RoomPriceDetailsLog roomPriceDetailsLogTemp = new RoomPriceDetailsLog()
                            {
                                Price = Double.Parse(priceDetailAfter.Element("Price").Value),
                                ExtraPrice = Double.Parse(priceDetailAfter.Element("ExtraPrice").Value),
                                PriceNR = Double.Parse(priceDetailAfter.Element("PriceNR").Value),
                                ExtraPriceNR = Double.Parse(priceDetailAfter.Element("ExtraPriceNR").Value),
                                CheckIn = DateTime.Parse(priceDetailAfter.Element("CheckIn").Value).Date,
                                CheckOut = DateTime.Parse(priceDetailAfter.Element("CheckOut").Value).Date,
                                Currency = priceDetailAfter.Element("Currency").Value

                            };

                            roomPriceDetailsAfterLogsList.Add(roomPriceDetailsLogTemp);
                        }

                        roomDetailsLogAfter.PriceDetails = roomPriceDetailsAfterLogsList;


                    }
                    else
                    {
                        List<RoomPriceDetailsLog> roomPriceDetailsBeforeLogsList = new List<RoomPriceDetailsLog>();
                        List<RoomPriceDetailsLog> roomPriceDetailsAfterLogsList = new List<RoomPriceDetailsLog>();

                        int totalPriceDetailsPerRoomCount = priceDetailsAfter.Count;
                        //bool hasPriceDetailChange = false;

                        for (int j = 0; j < totalPriceDetailsPerRoomCount; j++)
                        {
                            bool hasPriceDetailChange = false;

                            RoomPriceDetailsLog roomPriceDetailsBeforeLogTemp = new RoomPriceDetailsLog();
                            RoomPriceDetailsLog roomPriceDetailsAfterLogTemp = new RoomPriceDetailsLog();

                            double priceDetailBefore = Double.Parse(priceDetailsBefore[j].Element("Price").Value);
                            double priceDetailAfter = Double.Parse(priceDetailsAfter[j].Element("Price").Value);
                            double extraPriceDetailBefore = Double.Parse(priceDetailsBefore[j].Element("ExtraPrice").Value);
                            double extraPriceDetailAfter = Double.Parse(priceDetailsAfter[j].Element("ExtraPrice").Value);
                            double priceNRDetailBefore = Double.Parse(priceDetailsBefore[j].Element("PriceNR").Value);
                            double priceNRDetailAfter = Double.Parse(priceDetailsAfter[j].Element("PriceNR").Value);
                            double extraPriceNRDetailBefore = Double.Parse(priceDetailsBefore[j].Element("ExtraPriceNR").Value);
                            double extraPriceNRDetailAfter = Double.Parse(priceDetailsAfter[j].Element("ExtraPriceNR").Value);
                            DateTime checkInDetailBefore = DateTime.Parse(priceDetailsBefore[j].Element("CheckIn").Value).Date;
                            DateTime checkOutDetailBefore = DateTime.Parse(priceDetailsBefore[j].Element("CheckOut").Value).Date;
                            DateTime checkInDetailAfter = DateTime.Parse(priceDetailsAfter[j].Element("CheckIn").Value).Date;
                            DateTime checkOutDetailAfter = DateTime.Parse(priceDetailsAfter[j].Element("CheckOut").Value).Date;
                            string currencyDetailBefore = priceDetailsBefore[j].Element("Currency").Value;
                            string currencyDetailAfter = priceDetailsAfter[j].Element("Currency").Value;

                            if (priceDetailBefore != priceDetailAfter)
                            {
                                roomPriceDetailsBeforeLogTemp.Price = priceDetailBefore;
                                roomPriceDetailsAfterLogTemp.Price = priceDetailAfter;
                                hasPriceDetailChange = true;
                            }

                            if (extraPriceDetailBefore != extraPriceDetailAfter)
                            {
                                roomPriceDetailsBeforeLogTemp.ExtraPrice = extraPriceDetailBefore;
                                roomPriceDetailsAfterLogTemp.ExtraPrice = extraPriceDetailAfter;
                                hasPriceDetailChange = true;
                            }

                            if (priceNRDetailBefore != priceNRDetailAfter)
                            {
                                roomPriceDetailsBeforeLogTemp.PriceNR = priceNRDetailBefore;
                                roomPriceDetailsAfterLogTemp.PriceNR = priceNRDetailAfter;
                                hasPriceDetailChange = true;
                            }

                            if (extraPriceNRDetailBefore != extraPriceNRDetailAfter)
                            {
                                roomPriceDetailsBeforeLogTemp.ExtraPriceNR = extraPriceNRDetailBefore;
                                roomPriceDetailsAfterLogTemp.ExtraPriceNR = extraPriceNRDetailAfter;
                                hasPriceDetailChange = true;
                            }

                            roomPriceDetailsBeforeLogTemp.CheckIn = checkInDetailBefore;
                            roomPriceDetailsBeforeLogTemp.CheckOut = checkOutDetailBefore;

                            roomPriceDetailsAfterLogTemp.CheckIn = checkInDetailAfter;
                            roomPriceDetailsAfterLogTemp.CheckOut = checkOutDetailAfter;

                            roomPriceDetailsBeforeLogTemp.Currency = currencyDetailBefore;
                            roomPriceDetailsAfterLogTemp.Currency = currencyDetailAfter;

                            if (hasPriceDetailChange)
                            {
                                roomPriceDetailsBeforeLogsList.Add(roomPriceDetailsBeforeLogTemp);
                                roomPriceDetailsAfterLogsList.Add(roomPriceDetailsAfterLogTemp);
                            }

                            //roomPriceDetailsBeforeLogsList.Add(roomPriceDetailsBeforeLogTemp);
                            //roomPriceDetailsAfterLogsList.Add(roomPriceDetailsAfterLogTemp);

                            hasPriceDetailChange = false;
                        }

                        //if (hasPriceDetailChange)
                        //{
                            roomDetailsLogBefore.PriceDetails = roomPriceDetailsBeforeLogsList;
                            roomDetailsLogAfter.PriceDetails = roomPriceDetailsAfterLogsList;
                        //}

                    }
                }



                //Agregar a Listas
                //roomDetailsLogListBefore.Add(roomDetailsLogBefore);
                //roomDetailsLogListAfter.Add(roomDetailsLogAfter);

                if (hasRoomChange)
                {
                    roomDetailsLogListBefore.Add(roomDetailsLogBefore);
                    roomDetailsLogListAfter.Add(roomDetailsLogAfter);
                }

                hasRoomChange = false;

            }

            //Se agrega al objeto main
            //if (hasRoomChange)
            //{
                reservationDetailLog.RoomDetailsBefore = roomDetailsLogListBefore;
                reservationDetailLog.RoomDetailsAfter = roomDetailsLogListAfter;
            //}
            #endregion

        }

        private static void RoomsUVCCLog(XDocument dataBefore, XDocument dataAfter,string currencyBefore, string currencyAfter, ref ReservationDetailLog reservationDetailLog)
        {
            //Revisar Habitaciones
            List<XElement> roomsBefore = dataBefore.Root.Element("HotelDisplay").Element("Rooms").Elements("Room").ToList();
            
            List<XElement> roomsAfterRoomsNodeLevel= dataAfter.Root.Element("Rooms").Elements("Room").ToList();
            List<XElement> roomsAfterRootNodeLevel = dataAfter.Root.Elements("Room").ToList();

            List<XElement> roomsAfter = roomsAfterRoomsNodeLevel.Concat(roomsAfterRootNodeLevel).ToList();

            //Precios dataAfter
            List <XElement> pricesDataAfter = dataAfter.Root.Elements("Prices").ToList();

            List<RoomDetailsLog> roomDetailsLogListBefore = new List<RoomDetailsLog>();
            List<RoomDetailsLog> roomDetailsLogListAfter = new List<RoomDetailsLog>();

            int totalRoomsOccupied = roomsAfter.Count;
            

            for(int i = 0; i < totalRoomsOccupied; i++)
            {
                bool hasRoomChange = false;

                RoomDetailsLog roomDetailsLogBefore = new RoomDetailsLog();
                RoomDetailsLog roomDetailsLogAfter = new RoomDetailsLog();

                //Revisar Adultos y Extra Adultos
                int adultsBefore = Int32.Parse(roomsBefore[i].Element("Adults").Value);
                int extraAdultsBefore = Int32.Parse(roomsBefore[i].Element("ExtraAdults").Value);
                int adultsAfter = Int32.Parse(roomsAfter[i].Element("Adults").Value);                
                int extraAdultsAfter = Int32.Parse(roomsAfter[i].Element("AdultsExtra").Value);

                if (adultsBefore != adultsAfter)
                {
                    roomDetailsLogBefore.Adults = adultsBefore;
                    roomDetailsLogAfter.Adults = adultsAfter;
                    hasRoomChange = true;
                }

                if (extraAdultsBefore != extraAdultsAfter)
                {
                    roomDetailsLogBefore.ExtraAdults = extraAdultsBefore;
                    roomDetailsLogAfter.ExtraAdults = extraAdultsAfter;
                    hasRoomChange = true;
                }

                //Revisar Ninios, Extra Ninios y Edades
                int childrensBefore = Int32.Parse(roomsBefore[i].Element("Children").Value);
                int childrensAfter = Int32.Parse(roomsAfter[i].Element("Children").Value);
                int extraChildrensBefore = Int32.Parse(roomsBefore[i].Element("ExtraChildren").Value);
                int extraChildrensAfter = Int32.Parse(roomsAfter[i].Element("ChildrenExtra").Value);
                string childrenAgesBefore = roomsBefore[i].Element("ChildrenAges").Value ?? string.Empty;
                string childrenAgesAfter = roomsAfter[i].Element("ChildrenAges").Value ?? string.Empty;

                if (childrensBefore != childrensAfter)
                {
                    roomDetailsLogBefore.Childrens = childrensBefore;
                    roomDetailsLogAfter.Childrens = childrensAfter;
                    hasRoomChange = true;
                }

                if (extraChildrensBefore != extraChildrensAfter)
                {
                    roomDetailsLogBefore.ExtraAdults = extraAdultsBefore;
                    roomDetailsLogAfter.ExtraAdults = extraAdultsAfter;
                    hasRoomChange = true;
                }

                int totalChildrensBeforeWithExtras = childrensBefore + extraChildrensBefore;
                int totalChildrensAfterWithExtras = childrensAfter + extraChildrensAfter;

                if ((totalChildrensBeforeWithExtras > 0 || totalChildrensBeforeWithExtras > 0) && !string.Equals(childrenAgesBefore, childrenAgesAfter))
                {
                    roomDetailsLogBefore.AgeChildren = childrenAgesBefore;
                    roomDetailsLogAfter.AgeChildren = childrenAgesAfter;
                    hasRoomChange = true;
                }

                //
               
                //Revisar Precios por habitacion para dataBefore
                List<XElement> priceDetailsBefore = roomsBefore[i].Element("Rates").Elements("Rate").ToList();

                List<RoomPriceDetailsLog> roomPriceDetailsBeforeLogsList = new List<RoomPriceDetailsLog>();

                for (int j = 0; j < priceDetailsBefore.Count; j++)
                {
                    RoomPriceDetailsLog roomPriceDetailsBeforeLogTemp = new RoomPriceDetailsLog()
                    {
                        Price = Double.Parse(priceDetailsBefore[j].Element("AdultRate").Value),
                        ExtraPrice = Double.Parse(priceDetailsBefore[j].Element("ExtraRateAdult").Value),
                        CheckIn = DateTime.Parse(priceDetailsBefore[j].Element("Date").Value).Date,
                        CheckOut = DateTime.Parse(priceDetailsBefore[j].Element("Date").Value).Date,
                        Currency  = currencyBefore
                    };

                    roomPriceDetailsBeforeLogsList.Add(roomPriceDetailsBeforeLogTemp);
                }

                //roomDetailsLogBefore.PriceDetails = roomPriceDetailsBeforeLogsList;
                //Termina precios por habitacion para dataBefore

                //Revisar Precios por habitacion para dataAfter
                //Filtrar por RoomIndex
                List<XElement> priceDetailsAfter = FilterPriceByRoomIndexUVCC(pricesDataAfter,i);

                List<RoomPriceDetailsLog> roomPriceDetailsAfterLogsList = new List<RoomPriceDetailsLog>();

                for (int j = 0; j < priceDetailsAfter.Count; j++)
                {
                    string[] fromNoSpaceArray = priceDetailsAfter[j].Element("From").Value.Split(' ');
                    int[] fromNoSlashArray = fromNoSpaceArray[0].Split('/').Select(Int32.Parse).ToArray();

                    string[] toNoSpaceArray = priceDetailsAfter[j].Element("To").Value.Split(' ');
                    int[] toNoSlashArray = toNoSpaceArray[0].Split('/').Select(Int32.Parse).ToArray();

                    RoomPriceDetailsLog roomPriceDetailsAfterLogTemp = new RoomPriceDetailsLog()
                    {
                        Price = Double.Parse(priceDetailsAfter[j].Element("Rate").Value),
                        ExtraPrice = Double.Parse(priceDetailsAfter[j].Element("Extra").Value),
                        CheckIn = new DateTime(fromNoSlashArray[2], fromNoSlashArray[1], fromNoSlashArray[0]).Date,
                        CheckOut = new DateTime(toNoSlashArray[2], toNoSlashArray[1], toNoSlashArray[0]).Date,
                        Currency = currencyAfter
                    };

                    roomPriceDetailsAfterLogsList.Add(roomPriceDetailsAfterLogTemp);
                }

                //roomDetailsLogAfter.PriceDetails = roomPriceDetailsAfterLogsList;

                //TODO:Comparar total por habitacion para ver si hubo modificaciones
                //Comparar el total de la habitacion esta lista roomPriceDetailsBeforeLogsList con roomPriceDetailsAfterLogsList

                bool hasRoomPriceChange = ComparePricesTotalRoom(roomPriceDetailsBeforeLogsList, roomPriceDetailsAfterLogsList);

                if (hasRoomPriceChange)
                {
                    roomDetailsLogBefore.PriceDetails = roomPriceDetailsBeforeLogsList;
                    roomDetailsLogAfter.PriceDetails = roomPriceDetailsAfterLogsList;

                    hasRoomChange = true;
                }

                //Agregar a Listas
                if (hasRoomChange)
                {
                    roomDetailsLogListBefore.Add(roomDetailsLogBefore);
                    roomDetailsLogListAfter.Add(roomDetailsLogAfter);
                }

                
            }


            reservationDetailLog.RoomDetailsBefore = roomDetailsLogListBefore;
            reservationDetailLog.RoomDetailsAfter = roomDetailsLogListAfter;
            
        }


        private static List<XElement> FilterPriceByRoomIndexUVCC(List<XElement> pricesDataAfter, int index)
        {
            List<XElement> pricesByRoomIndex = new List<XElement>();

            for(int i = 0; i < pricesDataAfter.Count; i++)
            {
                int priceDataAfterIndex = Int32.Parse(pricesDataAfter[i].Element("RoomIndex").Value);

                if (priceDataAfterIndex == index) pricesByRoomIndex.Add(pricesDataAfter[i]);
            }

            return pricesByRoomIndex;
        }


        private static bool  ComparePricesTotalRoom(List<RoomPriceDetailsLog> roomPriceDetailsBeforeLogsList, List<RoomPriceDetailsLog> roomPriceDetailsAfterLogsList)
        {
            //Comparar total de la reserva

            double totalBefore = 0;
            double totalAfter = 0;

            foreach(var roomPriceDetailBeforeLog in roomPriceDetailsBeforeLogsList)
            {
                var checkIn = roomPriceDetailBeforeLog.CheckIn.Date;
                var checkOut = roomPriceDetailBeforeLog.CheckOut.Date;

                if(DateTime.Compare(checkIn,checkOut) == 0)
                {
                    totalBefore += (roomPriceDetailBeforeLog.Price + roomPriceDetailBeforeLog.ExtraPrice);
                }
                else
                {
                    int nights = (checkOut - checkIn).Days;

                    for(int i = 0; i < nights; i++)
                    {
                        totalBefore += (roomPriceDetailBeforeLog.Price + roomPriceDetailBeforeLog.ExtraPrice);
                    }

                }
            }

            foreach(var roomPriceDetailAfterLog in roomPriceDetailsAfterLogsList)
            {
                var checkIn = roomPriceDetailAfterLog.CheckIn.Date;
                var checkOut = roomPriceDetailAfterLog.CheckOut.Date;

                if (DateTime.Compare(checkIn, checkOut) == 0)
                {
                    totalAfter += (roomPriceDetailAfterLog.Price + roomPriceDetailAfterLog.ExtraPrice);
                }
                else
                {
                    int nights = (checkOut - checkIn).Days;

                    for (int i = 0; i <= nights; i++)
                    {
                        totalAfter += (roomPriceDetailAfterLog.Price + roomPriceDetailAfterLog.ExtraPrice);
                    }

                }

            }

            return totalBefore != totalAfter;
        }
    }
}
