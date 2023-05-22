using System;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Net.Http;
using APIServices.Extension;
using APIServices.Models;
using APIServices.Models.DTO;
using APIServices.Models.DTO.Reservation.Deposit.Request;
using APIServices.Models.DTO.Reservation.Deposit.Response;
using APIServices.Models.DTO.Reservation.Pms.Response;
using OfficeOpenXml;
using PortalLibraries;


namespace APIServices
{
    public class ReservationService
    {
        public OzHotelesEntities dbContext = new OzHotelesEntities();

        public IQueryable<vReservation> GetAll() => dbContext.vReservation.AsQueryable();
           
        //obtiene todos los corporativos
        public IQueryable<Corporativos> GetCorporate() => dbContext.Corporativos.AsQueryable();

        public IQueryable<vReservation> Get(int hotelId)
        {
            return dbContext.vReservation.AsQueryable()
                .Where(x => x.HotelId == hotelId);
        }

        #region excel
        public HttpResponseMessage GetExcel(IQueryable<vReservation> query)
        {
            var result = query.Select(r => new Excel.ReservationList
            {
                NoReservation = r.ConfirmNumber,
                Hotel = r.Hotel,
                Customer =r.Client,
                Date = r.ReservationDate,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                Origin = r.Portal,
                Corporate = dbContext.Corporativos.FirstOrDefault(x => x.idCorporativo == r.CorporateId).NombreCorp,
                PaymentMethod = r.PaymentMethod,
                Total = r.Total,
                Status = (r.Status == 1) ? "Reservado" : (r.Status == 3) ? "Cancelado" : "En proceso",
            }).ToList();


            MemoryStream file = new MemoryStream();
            var excelPackage = new ExcelPackage(file);

            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("reservaciones");
            //add the headers
            excelWorksheet.Cells[1, 1].Value = "#";
            excelWorksheet.Cells[1, 2].Value = "Hotel";
            excelWorksheet.Cells[1, 3].Value = "Cliente";
            excelWorksheet.Cells[1, 4].Value = "Fecha de reservación";
            excelWorksheet.Cells[1, 5].Value = "Fecha de llegada";
            excelWorksheet.Cells[1, 6].Value = "Fecha de salida";
            excelWorksheet.Cells[1, 7].Value = "Origen";
            excelWorksheet.Cells[1, 8].Value = "Corporativo";
            excelWorksheet.Cells[1, 9].Value = "Forma de Pagó";
            excelWorksheet.Cells[1, 10].Value = "Total";
            excelWorksheet.Cells[1, 11].Value = "Status";

            

            //Add some items...
          
            int row = 2;

            for(int i = 0; i < result.Count; i++)
            {
                string rowNumber = row.ToString();

                excelWorksheet.Cells["A" + rowNumber].Value = result.ElementAt(i).NoReservation;
                excelWorksheet.Cells["B" + rowNumber].Value = result.ElementAt(i).Hotel;
                excelWorksheet.Cells["C" + rowNumber].Value = result.ElementAt(i).Customer;
                excelWorksheet.Cells["D" + rowNumber].Value = result.ElementAt(i).Date.ToString("dd/MM/yyyy");
                excelWorksheet.Cells["E" + rowNumber].Value = result.ElementAt(i).CheckIn.ToString("dd/MM/yyyy");
                excelWorksheet.Cells["F" + rowNumber].Value = result.ElementAt(i).CheckOut.ToString("dd/MM/yyyy");
                excelWorksheet.Cells["G" + rowNumber].Value = result.ElementAt(i).Origin;
                excelWorksheet.Cells["H" + rowNumber].Value = result.ElementAt(i).Corporate;
                excelWorksheet.Cells["I" + rowNumber].Value = result.ElementAt(i).PaymentMethod;
                excelWorksheet.Cells["J" + rowNumber].Value = result.ElementAt(i).Total;
                excelWorksheet.Cells["K" + rowNumber].Value = result.ElementAt(i).Status;
                row++;
            }
            excelWorksheet.Cells["A1:J" + row.ToString()].AutoFitColumns();

            //excelWorksheet.Cells["A2"].Value = "12001";
            //excelWorksheet.Cells["B2"].Value = "Nails";
            //excelWorksheet.Cells["C2"].Value = "asdfasfd";
            //excelWorksheet.Cells["D2"].Value = 3.99;

            excelPackage.Workbook.Properties.Title = "reservaciones";
            excelPackage.Workbook.Properties.Author = "Internet Power";
            excelPackage.Workbook.Properties.Company = "Internet Power";

            excelPackage.Save();

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new ByteArrayContent(file.ToArray());
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "reservaciones.xlsx"
            };

            return response;
        }

        #endregion

        #region detalles de la reserva

        public vReservationDetails GetReservation(int reservationId)
        {
            return dbContext.vReservationDetails.AsNoTracking()
                .FirstOrDefault(x => x.reservationId == reservationId);
        }

        public ReservationDetailsModel GetDetails(int reservationId, bool isSupervisor, bool isHotelCompany,int userId, bool isUserChain = false, 
            bool isUsuarioHotelAssociation = false,int idCorporateUserChain = 0, int idCorporatePortal = -1, int idAsociationPb = 0, int idAsociation = -1)
        {

            var details = 
                 GetReservation(reservationId);


            var model = new ReservationDetailsModel();
            if (details != null)
            {
                model.ReservationNumber = details.reservationNumber;
                model.ReservationId = reservationId;
                model.CancellationNumber = details.cancellationNumber;
                model.HotelName = details.hotelName;
                model.HotelEmail = details.hotelEmail;
                model.HotelId = details.hotelId;
                model.CompanyId = details.companyId;
                model.CorporateId = details.corporateId;
                model.CorporateName = details.corporateName;
                model.Address = details.address;
                model.Status = details.status;
                model.ReservationDate = details.reservationDate;
                model.CheckIn = details.checkIn;
                model.CheckOut = details.checkOut;
                model.Nights = (details.checkOut - details.checkIn).Days;
                model.City = details.city;
                model.Country = details.country;
                model.AccessCode = details.accessCode;
                model.RatePlan = details.ratePlan;
                model.Source = details.source;
                model.CancellationReason = details.cancellationReason;
                model.ModificationReason = details.modificationReason;
                model.Portal = details.Portal;
                model.IsNetRateUV = details.IsNetRateUV;
                model.PaymentWay = details.paymentType;
                model.Agency = details.agency;
                model.AgencyUser = details.agencyUser;
                model.RatePlanPromotion = details.ratePlanPromotion;
                model.NamePromotion = details.namePromotion;
                model.IdCancellationUser = details.idCancellationUser;
                model.UserCancellation = details.userCancellation;
                model.ShowLogs = !details.source.Equals("IDS") ? true : false;
                model.BankDepositDetails = new BankDepositDetails();
                if (details.paymentType == 0)
                {
                    decimal? totalDeposited = details.depositAmount;
                    double debt = 0;
                    bool hasDebt = false;

                    debt = Convert.ToDouble((details.amountTotal -totalDeposited));
                    hasDebt = (debt > 0);

                    model.BankDepositDetails.Target = details.depositTarget;
                    model.BankDepositDetails.Total = Convert.ToDouble(totalDeposited); //(double)details.depositAmount;
                    model.BankDepositDetails.Currency = details.depositCurrency;
                    model.BankDepositDetails.Reference = details.depositReference;
                    model.BankDepositDetails.Debt = debt;
                    model.BankDepositDetails.HasDebt = hasDebt;
                }
                model.PolicyDetails = new PolicyDetails()
                {
                    HotelCancellation = details.h_cancellationPolicies,
                    HotelGuarantee = details.h_guaranteePolicies,
                    HotelCreditCard = details.h_creditcardPolicies,
                    RatePlanCancellation = details.rp_cancellationPolicies,
                    RatePlanGuarantee = details.rp_guaranteePolicies,
                    RatePlanCreditCard = details.rp_creditcardPolicies,
                };

                model.Customer = new CustomerDetails()
                {
                    Name = details.customerName,
                    LastName = details.customerLastName,
                    Email = details.customerEmail,
                    Phone = details.customerPhone,
                    CardDetails = new CardDetails()
                };

                model.Pms = new Pms()
                {
                    Status = details.pmsStatus.Value,
                    Action = details.pmsAction,
                    FailedAttempts = details.pmsFailedAttempts,
                    ReservationNumber = details.pmsReservationNumber
                };

                /*credit card*/
                var showCreditCard = dbContext.vPermissions
                    .FirstOrDefault(x => x.userId == userId)?.showCreditCard;

                if (!string.IsNullOrEmpty(details.cardNumber))
                {
                    string cc = crypto.DecryptString128Bit(details.cardNumber, crypto.PublicKey);
                    model.Customer.CardDetails.CardType = model.Customer.CardDetails.GetCardType(cc);
                    cc = (cc.Length > 0) ? $"XXXXXXXXXXXX{cc.Substring(cc.Length - 4)}" : "" ;
                    model.Customer.CardDetails.Number = cc;
                    model.Customer.CardDetails.IsSuccess = true;
                }
                if (isHotelCompany || (showCreditCard.HasValue ? showCreditCard.Value : false))
                    model.Customer.CardDetails.AllowsShowCreditCardData = true;
                /*fin credit card*/

                GetPayments(ref model, reservationId);

                double totalRooms = 0;
                double totalRoomsNR = 0;
                GetRooms(ref model, reservationId, details , out totalRooms, out totalRoomsNR);

                model.TotalDetails = new TotalDetails();
                model.TotalDetails.SubTotal = totalRooms;
                model.TotalDetails.SubTotalNR = totalRoomsNR;
                model.TotalDetails.TotalNR = Convert.ToDouble(details.totalNetRate); //TODO: Validar si esta en null
                model.TotalDetails.Total = Convert.ToDouble(details.total);
                model.TotalDetails.Ecotasa = Convert.ToDouble(details.ecotasa);
                model.TotalDetails.IncludesTax = details.includesTax.Value;

                double tax = Convert.ToDouble(details.tax);

                double totalTax = Convert.ToDouble(details.total - details.ecotasa);

                double totalTaxHotel = Convert.ToDouble((details.IsNetRateUV ? (details.totalNetRate - details.ecotasa) : 0));


                model.TotalDetails.TaxesHotel = Math.Round(totalTaxHotel - (totalTaxHotel / ((tax / 100) + 1)), 2);
                model.TotalDetails.Taxes = Math.Round(totalTax - (totalTax / ((tax / 100) + 1)), 2);
                
                model.TotalDetails.Currency = details.currency;
                model.TotalDetails.Commission = Convert.ToDouble((details.IsNetRateUV ? (details.total - details.totalNetRate > 0)? details.total - details.totalNetRate : 0 : 0));

                Permissions(ref model, isSupervisor, isUserChain, isUsuarioHotelAssociation, idCorporateUserChain, idCorporatePortal, idAsociationPb, idAsociation);
            }

            return model;
        }

        public List<vReservationRoomDetails> GetRoomsReservation(int reservationId)
        {
            return dbContext.vReservationRoomDetails
                .AsNoTracking().Where(x => x.reservationId == reservationId)
                .ToList();
        }

        public vReservationPayments GetPaymentsDetail(int reservationId)
        {
            return dbContext.vReservationPayments.
                FirstOrDefault(r => r.reservationId == reservationId
                && r.paymentType == 1);
        }

        void GetRooms(ref ReservationDetailsModel model, int reservationId, vReservationDetails details, out double totalRooms, out double totalRoomsNR)
        {
            var rooms = 
                 GetRoomsReservation(reservationId);

            int index = 0;
            double totalRoom = 0;
            double totalRoomNR = 0;

            bool includexTaxes = (bool)details.includesTax;
            decimal tax = details.tax;
            decimal ecotasa = details.ecotasa;
            int totalNights = (details.checkOut - details.checkIn).Days;
            int totalOfRooms = rooms.Count;

            model.RoomDetails = new List<RoomDetails>();
           
            foreach(var room in rooms)
            {
                var prices = dbContext.VReservationRoomPriceDetails
                    .AsNoTracking().Where(x => x.roomPriceId == room.roomPriceId)
                    .ToList();

                var priceDetails = new List<RoomPriceDetails>();
                double totalPerRoom = 0;
                double totalPerRoomNetRate = 0;
                foreach(var price in prices)
                {
                    var checkOutReservation = (DateTime)model.CheckOut;

                    int nights = (DateTime.Compare(checkOutReservation, price.checkOut) == 0) 
                        ? (price.checkOut - price.checkIn).Days 
                        : (price.checkOut - price.checkIn).Days + 1;
                    //int nights = (price.checkOut - price.checkIn).Days + 1;
                    //int nights = (price.checkOut - price.checkIn).Days;

                    //Sino es OTA y Si no incluye taxes se le agregan los taxes para que quede estandarizado con la vista
                    if (!model.Source.Equals("IDS"))
                    {
                        if (!includexTaxes)
                        {
                            price.price = AddTaxToPrice(price.price, tax, ecotasa, totalNights, totalOfRooms);
                            price.extraPrice = AddTaxToPrice(price.extraPrice, tax, ecotasa, totalNights, totalOfRooms);
                            price.priceNR = AddTaxToPrice(price.priceNR, tax, ecotasa, totalNights, totalOfRooms);
                            price.extraPriceNR = AddTaxToPrice(price.extraPriceNR, tax, ecotasa, totalNights, totalOfRooms);
                        }
                    }


                    totalPerRoom += (double)((price.price * nights) + (price.extraPrice * nights));

                    double extraPriceNR = price.extraPriceNR == null ? 0 : (double)price.extraPriceNR;

                    totalPerRoomNetRate += (price.priceNR == null)? 0 :(double)(price.priceNR * nights) + (extraPriceNR);

                    priceDetails.Add(new RoomPriceDetails
                    {
                        RoomPriceId = price.roomPriceId,
                        Price = (double)price.price,
                        ExtraPrice = (double)price.extraPrice,
                        PriceNR = (price.priceNR == null)? 0 : (double)price.priceNR,
                        ExtraPriceNR =(price.extraPriceNR == null)? 0 :  (double)price.extraPriceNR,
                        Currency = price.currency,
                        CheckIn = price.checkIn,
                        CheckOut = price.checkOut,
                    });
                }

                totalRoom += totalPerRoom;
                totalRoomNR += totalPerRoomNetRate;
                

                model.RoomDetails.Add(new RoomDetails
                {
                    ReservationId = room.reservationId,
                    RoomPriceId = room.roomPriceId,
                    RoomCode = room.roomCode,
                    Name = room.roomName,
                    //Description = room.roomDescription,
                    Preferences = room.roomPreferences,
                    CheckIn = room.roomCheckIn,
                    CheckOut = room.roomCheckOut,
                    Adults = room.adults,
                    ExtraAdults = (room.extraAdults == null)? 0 : (int)room.extraAdults,
                    Childrens = room.childrens,
                    ExtraChildrens = (room.extraChildrens == null)? 0 : (int)room.extraChildrens,
                    AgeChildren = room.ageChildren,
                    PriceDetails = new List<RoomPriceDetails>(),
                    Total = totalPerRoom,
                    TotalNR = totalPerRoomNetRate,
                    NetRateContract = room.netRateContract,
                    Currency = room.currency,
                    RatePlan = room.ratePlan,
                    RateCode = room.rateCode,
                    Img = $"{ConfigurationManager.AppSettings["pathimgrooms"] ?? ""}/{details.companyId}/{room.roomTypeId}",
                    ImgDefault = ConfigurationManager.AppSettings["pathimgroomsdefault"],
                    CustomerName = room.customerName ?? "",
                    CustomerLastName = room.customerLastName ?? "",
                    RatePlanPromotion = room.ratePlanPromotion,
                    NamePromotion = room.namePromotion
                });

                model.RoomDetails[index].PriceDetails.AddRange(priceDetails);
                index++;
            }

            //Subtotal Normal y Subtotal NR
            totalRooms = !model.Source.Equals("IDS") ? SubtotalWithoutTax(totalRoom, (double)tax, (double)ecotasa) : totalRoom;
            totalRoomsNR = !model.Source.Equals("IDS")? SubtotalWithoutTax(totalRoomNR,(double)tax, (double)ecotasa) : totalRoomNR;
        }

        void GetPayments(ref ReservationDetailsModel model, int reservationId)
        {
            var payments = GetPaymentsDetail(reservationId);

            if (payments != null)
            {
                model.PaymentDetails = new PaymentDetails()
                {
                    ReservationId = payments.reservationId,
                    CustomerName = payments.customerName,
                    CustomerLastName = payments.customerLastName,
                    ReservationDate = payments.reservationDate,
                    Status = payments.Status,
                    Source = payments.Source,
                    PaymentMethod = payments.paymentmethod,
                    PaymentType = payments.paymentType,
                    Pasarela = payments.Pasarela,
                    Reference = payments.reference,
                    AuthorizationNumber = payments.authorizationNumber,
                    TotalPay = payments.totalPay,
                    CurrencyPay = payments.currencyPay
                };
            }
        }


        #endregion








        #region credit card

        public CardDetails GetCreditCardDetails(int reservationId, bool isHotelCompany, int userId)
        {
            var result = new CardDetails();
            var details = GetReservation(reservationId);
            var showCreditCard = dbContext.vPermissions
                .FirstOrDefault(x => x.userId == userId)?.showCreditCard;

            if (details != null && (isHotelCompany || showCreditCard.Value))
            {
                string cc = crypto.DecryptString128Bit(details.cardNumber, crypto.PublicKey);
                string cvv = details.cardCvv;
                if (Regex.IsMatch(details.cardCvv, "[A-Z]"))
                    cvv = crypto.DecryptString128Bit(cvv, crypto.PublicKey);

                result.Number = cc;
                result.CardType = result.GetCardType(cc);
                result.MonthExpiration = details.cardExpMonth;
                result.YearExpiration = details.cardExpYear;
                result.Owner = details.cardCustomerName;
                result.Cvv = cvv;
                result.IsSuccess = true;
                result.AllowsShowCreditCardData = true;
            }
            return result;
        }

        public string GetCode(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion







        #region permisos para editar la reserva
        void Permissions(ref ReservationDetailsModel model, bool isSupervisor, bool isUserChain, bool isUsuarioHotelAssociation,
            int idCorporateUserChain, int idCorporatePortal, int idAsociationPb, int idAsociation)
        {
            string source = model.Source;
            bool isNetRateUV = model.IsNetRateUV;
 
            switch (model.Status)
            {
                case 1:

                    if(!source.Equals("IDS"))
                    {
                        if (source.Equals("UNI"))
                        {
                            if (!isNetRateUV || isSupervisor) model.AllowsCancel = true;
                        }
                        else if (source.Equals("HTL") || isSupervisor) model.AllowsCancel = true;
                        else if (isUserChain && source.Equals("POR") && idCorporatePortal != -1 && idCorporateUserChain == idCorporatePortal) model.AllowsCancel = true;

                        if (source.Equals("POR") && isNetRateUV && isSupervisor) model.AllowsCancel = true;

                        if (source.Equals("POR") && isUsuarioHotelAssociation) if (idAsociation == idAsociationPb) model.AllowsCancel = true;


                        model.AllowsModify = isSupervisor ? true : false;

                    }

                    break;
                case 3:

                    if (!source.Equals("IDS"))
                    {
                        if (source.Equals("UNI"))
                        {
                            if (!isNetRateUV || isSupervisor) model.AllowsReactivate = true;
                        }
                        else if (source.Equals("HTL") || isSupervisor) model.AllowsReactivate = true;
                        else if (isUserChain && source.Equals("POR") && idCorporatePortal != -1 && idCorporateUserChain == idCorporatePortal) model.AllowsReactivate = true;

                        if (source.Equals("POR") && isNetRateUV && isSupervisor) model.AllowsReactivate = true;

                        if (source.Equals("POR") && isUsuarioHotelAssociation) if (idAsociation == idAsociationPb) model.AllowsReactivate = true;

                    }

                    break;
                case 4:

                    if(!source.Equals("IDS"))
                    {
                        if (isSupervisor || source.Equals("HTL"))
                        {
                            model.AllowsCancel = true;
                            model.AllowsReactivate = true;
                        }
                        else if (isUserChain && source.Equals("POR") && idCorporatePortal != -1 && idCorporateUserChain == idCorporatePortal)
                        {
                            if (!isNetRateUV)
                            {
                                model.AllowsCancel = true;
                                model.AllowsReactivate = true;
                            }
                        }

                        if (source.Equals("POR") && isNetRateUV && isSupervisor)
                        {
                            model.AllowsCancel = true;
                            model.AllowsReactivate = true;
                        }

                        if (source.Equals("POR") && isUsuarioHotelAssociation) 
                            if (idAsociation == idAsociationPb)
                            {
                                model.AllowsCancel = true;
                                model.AllowsReactivate = true;
                            }

                    }

                    break;
            }
        }
        #endregion








        #region cancelar reserva

        public CancelBookingRS Cancel(vReservationDetails rsv, int userId, string reasonToCancel,bool isSupervisor)
        {
            if (rsv != null)
            {
                if (!isSupervisor)
                {
                    var minimumDays = dbContext.Hoteles
                        .FirstOrDefault(x => x.idHotel == rsv.hotelId)?.DiasMinCancelar ?? 0;

                    if (MinimumDaysToCancel(rsv.checkIn, minimumDays))
                    {
                        return new CancelBookingRS
                        {
                            Error = $"No se cumple con el mínimo de días para cancelar. La reserva debe ser cancelada {minimumDays} días antes de la llegada",
                        };
                    }
                }

                return LocalCancel(rsv.reservationId, userId, reasonToCancel);
            }

            return new CancelBookingRS
            {
                Error = $"reservation not found {rsv.reservationId}"
            };
        }

        bool MinimumDaysToCancel(DateTime checkIn, int minimumDays) => (checkIn - DateTime.Now).Days < minimumDays;

        CancelBookingRS LocalCancel(int reservationId, int userId, string reasonToCancel)
        {
            var res = new CancelBookingRS();
            string cancelNumber = $"CX{DateTime.Now.ToString("yyMMddHHmmss")}{userId}";
            using(DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.spReservationCancel(reservationId, null, cancelNumber, null, null, null, 
                        null, null, null, null, reasonToCancel, null, null, userId);
                    transaction.Commit();
                    res.CancelNumber = cancelNumber;
                    res.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    res.Error = $"failed to cancel: {ex.InnerException.Message}";
                    transaction.Rollback();
                }
            }
            return res;
        }

        #endregion






        #region modificar reserva
        public ModifyBookingRS Modify(int reservationId, ModifyBookingRQ req)
        {
            var res = new ModifyBookingRS();


            using(DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var reservation = dbContext.Reservaciones.FirstOrDefault(r => r.idReservacion == reservationId);

                    bool isNetRateUV = (bool)reservation.IsNetRateUV;
                    bool plusTax = reservation.PlusTax ?? false;
                    decimal tax = reservation.Impuesto;
                    decimal ecotasa = reservation.Ecotasa == null ? 0: (decimal) reservation.Ecotasa;
                    int nights = (req.CheckOut - req.CheckIn).Days;
                    int totalRooms = req.RoomsDetails.Count;

                    dbContext.spModificarReservacionByid(reservationId, req.Name, req.LastName, (decimal) req.Total, 
                        (decimal) req.TotalNR, req.CheckIn, req.CheckOut,req.Details);

                    foreach (var room in req.RoomsDetails)
                    {
                        int? idDetalleReservacion = room.RoomPriceId;
                        int? idReservacion = room.ReservationId;
                        byte? adultos = (byte?)room.Adults;
                        byte? ninios = (byte?)room.Childrens;
                        byte? adultosExtra = (byte?)room.ExtraAdults;
                        byte? niniosExtra = (byte?)room.ExtraChildrens;
                        string edadesNinios = room.AgeChildren;

                        dbContext.spModificarDetalleReservaciones(idDetalleReservacion, idReservacion, adultos, ninios, adultosExtra, niniosExtra, edadesNinios);

                        if (room.PriceDetails.Count > 0)
                        {
                            dbContext.spEliminarDetalleTarifasReservaciones(room.RoomPriceId);

                            foreach (var priceDetail in room.PriceDetails)
                            {
                                decimal price = Convert.ToDecimal(priceDetail.Price.ToString("0.00"));
                                decimal extraPrice = Convert.ToDecimal(priceDetail.ExtraPrice.ToString("0.00"));
                                decimal priceNR = Convert.ToDecimal(priceDetail.PriceNR.ToString("0.00"));
                                decimal extraPriceNR = Convert.ToDecimal(priceDetail.ExtraPriceNR.ToString("0.00"));

                                decimal priceRateDetail = !plusTax ? PriceWithoutTax(price, tax, ecotasa, nights, totalRooms) : price;
                                decimal extraPriceRateDetail = !plusTax ? PriceWithoutTax(extraPrice, tax, ecotasa, nights, totalRooms) : extraPrice;
                                decimal priceNRRateDetail = priceNR;
                                decimal extraPriceNRRateDetail = extraPriceNR;

                                if (isNetRateUV)
                                {
                                    priceNRRateDetail = !plusTax ? PriceWithoutTax(priceNR, tax, ecotasa, nights, totalRooms) : priceNRRateDetail;
                                    extraPriceNRRateDetail = !plusTax ? PriceWithoutTax(extraPriceNR, tax, ecotasa, nights, totalRooms) : extraPriceNR;
                                }

                                string currencyPriceRateDetail = priceDetail.Currency ?? "MXN";

                                dbContext.spAgregarDetalleTarifasReservaciones(priceDetail.RoomPriceId, 0, priceRateDetail, extraPriceRateDetail, priceNRRateDetail, extraPriceNRRateDetail, currencyPriceRateDetail, priceDetail.CheckIn.Date, priceDetail.CheckOut.Date);

                            }

                        }

                    }




                    transaction.Commit();

                    res.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    res.Error = new ErrorRS();
                    res.Error.Errors = new List<string>();
                    //
                    res.IsSuccess = false;
                    res.Error.HasErrors = true;
                    res.Error.Errors.Add($"failed to modify: {ex.InnerException.Message}");
                    transaction.Rollback();
                }
            }
            return res;
        }
        #endregion

        #region calculo de precios
        private decimal AddTaxToPrice(decimal? price, decimal tax, decimal ecotasa, int nights, int totalRooms)
        {
            if (price == 0) return 0;
            
            decimal ecotasaPerRoomRate = (ecotasa / nights) / totalRooms;

            decimal total = (decimal)((price * ((tax / 100) + 1)) + ecotasaPerRoomRate);

            return Convert.ToDecimal(total.ToString("0.00"));

        }
        private decimal PriceWithoutTax(decimal total, decimal tax, decimal ecotasa, int nights, int totalRooms)
        {
            if (total == 0) return 0;

            decimal ecotasaPerRoomRate = (ecotasa / nights) / totalRooms;

            var totalNoEcotasa = total - ecotasaPerRoomRate;  //ecotasa / noches totales de la reserva = resultado / totaldehabitaciones  65 / 2

            var price = Convert.ToDecimal(((totalNoEcotasa) / ((tax / 100) + 1)).ToString("0.00"));

            return price;
        }

        private double SubtotalWithoutTax(double total, double tax, double ecotasa)
        {
            double totalNoEcotasa = Convert.ToDouble(total - ecotasa);

            double taxes = Math.Round(totalNoEcotasa - (totalNoEcotasa / ((tax / 100) + 1)), 2);

            string subtotal = (totalNoEcotasa - taxes).ToString("0.00");

            return Convert.ToDouble(subtotal);
        }

        #endregion

        #region reactivar reserva

        public ModifyBookingRS Reactivate(int reservationId, bool rm)
        {
            var res = new ModifyBookingRS();

            using (OzHotelesEntities ctx = new OzHotelesEntities())
            {
                try
                {
                    string noRes = reservationId.ToString();

                    var reservation = ctx.Reservaciones.First(r => r.NoReservacion == noRes);
                    reservation.Status = 1;
                    reservation.FechaStatus = DateTime.Now;
                    reservation.NoCancelacion = "";
                    reservation.pmsAct = "SS";
                    reservation.pmsStatus = false;

                    if(rm == true)
                    {
                        var result = (from r in ctx.Reservaciones
                                    join rd in ctx.ReservationsDeposits on r.idReservacion equals rd.idreservacion into RRD
                                    where r.NoReservacion == noRes
                                    from rrd in RRD.DefaultIfEmpty()
                                    select new ReactivationDTO
                                    {
                                        IdRes = r.idReservacion,
                                        Dep = rrd.dep_monto
                                    }).ToList();


                        if (result[0].Dep == 0)
                        {
                            reservation.Status = 4;
                        }
                        else
                        {
                            reservation.Status = 1;
                        }   
                    }

                    ctx.SaveChanges();
                    res.IsSuccess = true;
                }
                catch (Exception ex)
                {

                    res.Error = new ErrorRS();
                    res.Error.Errors = new List<string>();
                    //
                    res.IsSuccess = false;
                    res.Error.HasErrors = true;
                    res.Error.Errors.Add($"failed to reactivate: {ex.InnerException.Message}");
                }

            }    

            return res;

        }

        #endregion


        #region deposito
            
        public ReservationDepositResponse DepositUpdate(ReservationDepositDTO depositDTO)
        {
            ReservationDepositResponse response = new ReservationDepositResponse();
            
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {

                    dbContext.spReservationsByDeposit_Update(depositDTO.ReservationId, depositDTO.NumberAccount, depositDTO.Bank,
                        Convert.ToDecimal(depositDTO.Amount), depositDTO.Currency, depositDTO.DepositDate.Date, depositDTO.Details, depositDTO.UserId);

                    dbContext.spReservationUpdateStatus(depositDTO.ReservationId, 1);

                    dbContext.spReservationConfirmPaymentRequest(depositDTO.ReservationId.ToString(), depositDTO.Reference, depositDTO.AuthorizationNumber);

                    transaction.Commit();

                    response.IsSuccess = true;

                }
                catch(Exception ex)
                {
                    transaction.Rollback();

                    response.IsSuccess = false;
                } 
            }

            return response;
        }


        #endregion

        #region PMS

        public ReservationPmsResponse PmsUpdate(int reservationId, Pms request)
        {
            ReservationPmsResponse response = new ReservationPmsResponse() { IsSuccess = false };

            try
            {
                var reservation = dbContext.Reservaciones.FirstOrDefault(r => r.idReservacion == reservationId);

                if (reservation != null)
                {

                    reservation.pmsStatus = false;
                    reservation.pmsAct = request.Action;
                    reservation.PmsFailedAttempts = 0;

                    dbContext.SaveChanges();

                    response.IsSuccess = true;

                }


            }
            catch(Exception ex)
            {

            }

            return response;

        }

        public ReservationPmsResponse PmsStatusUpdate(int reservationId, Pms request)
        {
            ReservationPmsResponse response = new ReservationPmsResponse() { IsSuccess = false };

            try
            {
                var reservation = dbContext.Reservaciones.FirstOrDefault(r => r.idReservacion == reservationId);

                if (reservation != null)
                {

                    reservation.pmsStatus = request.Status;

                    dbContext.SaveChanges();

                    response.IsSuccess = true;

                }


            }
            catch (Exception ex)
            {

            }

            return response;

        }





        public bool PmsReactivate (int reservationId)
        {
            bool isReactivated = false;

            try
            {
                var reservation = dbContext.Reservaciones.FirstOrDefault(r => r.idReservacion == reservationId);

                if(reservation != null)
                {

                    reservation.pmsStatus = false;
                    reservation.PmsFailedAttempts = 0;

                    dbContext.SaveChanges();

                    isReactivated = true;

                }

            }
            catch(Exception ex)
            {

            }

            return isReactivated;

        }


        #endregion




        #region obtener template del correo
        public string GetTemplate(ReservationDetailsModel reservationDetails,string logoUrl)
        {
            //OzUniEntities uni = new OzUniEntities();
            //string webPage = uni.Portales.First(p => p.Nombre == reservationDetails.Portal).PaginaWeb;
            //string[] webPageSplit = webPage.Split('/');
            string template = null;
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-MX");
            string dayArrival = String.Format("{0:dd}", reservationDetails.CheckIn);
            string monthArrival = String.Format(culture,"{0:MMMM}", reservationDetails.CheckIn);
            string yearArrival = String.Format("{0:yyyy}", reservationDetails.CheckIn);
            string dayDeparture = String.Format("{0:dd}", reservationDetails.CheckOut);
            string monthDeparture = String.Format(culture, "{0:MMMM}", reservationDetails.CheckOut);
            string yearDeparture = String.Format("{0:yyyy}", reservationDetails.CheckOut);
            string subtotal = reservationDetails.TotalDetails.SubTotal.ToString("C") + " " + reservationDetails.TotalDetails.Currency;
            string total = reservationDetails.TotalDetails.Total.ToString("C") + " " + reservationDetails.TotalDetails.Currency;
            int indexOfAt = reservationDetails.Customer.Email.IndexOf('@');
            string domain = reservationDetails.Customer.Email.Substring(indexOfAt + 1);
            string account = reservationDetails.Customer.Email.Substring(0, indexOfAt + 1);
            DateTime today = DateTime.Today;
            //displayReservation = displayReservation.Replace("application",webPageSplit[webPageSplit.Length - 1]);
            //displayReservation += reservationDetails.ReservationNumber;
            template = template.ReadResourceFile("APIServices.EmailTemplate.HotelConfirmation-ES.html");
            template = template.Replace("[ALT]", reservationDetails.HotelName);
            template = template.Replace("[LOGO]", logoUrl + reservationDetails.CompanyId);
            template = template.Replace("[NOMBREDELCLIENTE]",reservationDetails.Customer.Name + " " + reservationDetails.Customer.LastName);
            template = template.Replace("[FECHA]",String.Format("{0:dd/MM/yyyy}",today));
            template = template.Replace("[NOMBREDELHOTEL]",reservationDetails.HotelName);
            template = template.Replace("[NUMERODERESERVACION]",reservationDetails.ReservationNumber);
            template = template.Replace("[ESTATUS]","Reservado");
            template = template.Replace("[FECHADERESERVACION]",String.Format("{0:dd/MM/yyyy}",reservationDetails.ReservationDate));
            template = template.Replace("[TELEFONODELCLIENTE]", reservationDetails.Customer.Phone);
            template = template.Replace("[CORREOELECTRONICO]",reservationDetails.Customer.Email);
            template = template.Replace("[CUENTA]",account);
            template = template.Replace("[DOMINIO]", domain);
            template = template.Replace("[DIRECCION]",reservationDetails.Address);
            template = template.Replace("[CIUDAD]",reservationDetails.City);
            template = template.Replace("[CODIGO]", reservationDetails.Country);
            template = template.Replace("[TELEFONO]", "2222326666");
            template = template.Replace("[MESLLEGADADETALLES]",monthArrival);
            template = template.Replace("[MESSALIDADETALLES]",monthDeparture);
            template = template.Replace("[FECHALLEGADADETALLES]",dayArrival);
            template = template.Replace("[FECHASALIDADETALLES]",dayDeparture);
            template = template.Replace("[ANOLLEGADADETALLES]",yearArrival);
            template = template.Replace("[ANOSALIDADETALLES]",yearDeparture);
            template = template.Replace("[NOCHE]", reservationDetails.Nights.ToString());

            string rooms = null;
            int adults = 0;
            int childrens = 0;
            for (int i = 0; i < reservationDetails.RoomDetails.Count; i++)
            {
                adults += reservationDetails.RoomDetails.ElementAt(i).Adults + reservationDetails.RoomDetails.ElementAt(i).ExtraAdults;
                childrens += reservationDetails.RoomDetails.ElementAt(i).Childrens + reservationDetails.RoomDetails.ElementAt(i).ExtraChildrens;
                string occupy = reservationDetails.RoomDetails.ElementAt(i).Adults.ToString() + " Adulto(s)";
                int occupyChildren = reservationDetails.RoomDetails.ElementAt(i).Childrens;
                int occupyExtraAdults = reservationDetails.RoomDetails.ElementAt(i).ExtraAdults;
                int occupyExtraChildren = reservationDetails.RoomDetails.ElementAt(i).ExtraChildrens;
                double roomPrice = reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).Price;
                string roomCurrency = reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).Currency;
                string arrivalRoom = String.Format(culture, "{0:dd MMM yyyy}", reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).CheckIn);
                string departureRoom = String.Format(culture, "{0:dd MMM yyyy}", reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).CheckOut);
                double roomTotalPrice = reservationDetails.RoomDetails.ElementAt(i).Total;
                if (occupyChildren != 0) occupy += ", " + occupyChildren.ToString() + " Niño(s)";
                if (occupyExtraAdults != 0) occupy += ", " + occupyExtraAdults.ToString() + " Adulto(s) Extra";
                if (occupyExtraChildren != 0) occupy += ", " + occupyExtraChildren.ToString() + " Niño(s) Extra";
                rooms += "<tr style='padding:0; text-align:left; vertical-align:top'>";
                rooms += "<th style='Margin:0; color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;line-height:1.3;margin:0;padding:0;text-align:left'>";
                rooms += "<div style='background-color:#fff;background-clip: border-box;border:1px solid rgba(0,0,0,.125);border-radius:.25rem; width:100%;'>";
                rooms += "<div style='font-weight:normal;color:#6c757d !important;font-size:18px;padding:2px 16px; padding:.75rem 1.25rem;margin-bottom:0;background-color:rgba(0, 0, 0, .03);border-bottom:1px solid rgba(0, 0, 0, .125);'>";
                rooms += reservationDetails.RoomDetails.ElementAt(i).RoomCode + " - " + reservationDetails.RoomDetails.ElementAt(i).Name;
                rooms += "</div>";
                rooms += "<div style='padding:1.25rem; padding:2px 16px;'>";
                rooms += "<address style='font-weight:normal;font-size:16px; font-style:normal; line-height:1.5em;'>";

                rooms += "<strong>Ocupación: </strong>";
                rooms += occupy;
                rooms += "<br>";
                rooms += "<strong>Plan tarifario: </strong>";
                rooms += reservationDetails.RoomDetails.ElementAt(i).RateCode + " - " + reservationDetails.RoomDetails.ElementAt(i).RatePlan;
                rooms += "<br>";
                if (!String.IsNullOrEmpty(reservationDetails.RoomDetails.ElementAt(i).Preferences))
                {
                    rooms += "<strong>Preferencias: </strong>";
                    rooms += reservationDetails.RoomDetails.ElementAt(i).Preferences;
                    rooms += "<br>";
                }
                rooms += "<strong>Fecha: </strong>";
                rooms += arrivalRoom + " - " + departureRoom;
                rooms += "<br>";
                rooms += "<strong>Precio por noche: </strong>";
                rooms += roomPrice.ToString("C") + " " + roomCurrency;
                rooms += "<br>";
                if(reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).ExtraPrice != 0)
                {
                    rooms += "<strong>Precio extra: </strong>";
                    string extraPrice = reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).ExtraPrice.ToString("C") + " " + reservationDetails.RoomDetails.ElementAt(i).PriceDetails.ElementAt(0).Currency;
                    rooms += extraPrice;
                    rooms += "<br>";
                }
                rooms += "<strong>Total: </strong>";
                rooms += roomTotalPrice.ToString("C") + " " + reservationDetails.RoomDetails.ElementAt(i).Currency;
                rooms += "<br>";
                rooms += "</address>";
                rooms += "</div>";
                rooms += "</div>";
                rooms += "</th>";
                rooms += "</tr>";
                rooms += "<tr style='padding:0;text-align:left;vertical-align:top'>";
                rooms += " <td height='16px' style='-moz-hyphens:auto;-webkit-hyphens:auto;Margin:0;border-collapse:collapse!important; color:#0a0a0a;font-family:'Source Sans Pro',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;font-size:16px;font-weight:400;hyphens:auto;line-height:16px;margin:0;mso-line-height-rule:exactly;padding:0;text-align:left;vertical-align:top;word-wrap:break-word'>";
                rooms += " &nbsp;";
                rooms += "</td></tr>";
            }

            template = template.Replace("[ADULTO]", adults.ToString());
            template = template.Replace("[NINO]", childrens.ToString());
            template = template.Replace("[HABITACIONES]",rooms);
            template = template.Replace("[SUBTOTAL]",subtotal);
            template = template.Replace("[TOTAL]",total);
            template = template.Replace("[TOTALESTIMADO]",total);
            //template = template.Replace("[ENLACE]",displayReservation);
            //template = template.Replace("[TARJETA]", "************4242");
            template = template.Replace("[POLITICASHOTELCANCELACION]",reservationDetails.PolicyDetails.HotelCancellation);
            template = template.Replace("[POLITICASHOTELGARANTIA]", reservationDetails.PolicyDetails.HotelGuarantee);
            template = template.Replace("[POLITICASHOTELTARJETA]", reservationDetails.PolicyDetails.HotelCreditCard);
            template = template.Replace("[POLITICASPLANCANCELACION]",reservationDetails.PolicyDetails.RatePlanCancellation);
            template = template.Replace("[POLITICASPLANGARANTIA]", reservationDetails.PolicyDetails.RatePlanGuarantee);
            template = template.Replace("[POLITICASPLANTARJETA]", reservationDetails.PolicyDetails.RatePlanCreditCard);
            return template;
        }

        #endregion

        #region Dispose DB
        public vReservationDetails GetReservationReleaseDB(int reservationId)
        {
            vReservationDetails details = null;

            using (OzHotelesEntities context = new OzHotelesEntities())
            {
                details = context.vReservationDetails.FirstOrDefault(vRD => vRD.reservationId == reservationId);
            }

            return details;

        }


        #endregion

        #region Log

        public IEnumerable<spReservationLog_Result2> GetReservationHistoryLog(string reservationId) => dbContext.spReservationLog(reservationId).ToList().OrderByDescending(r => r.Date);

        public void SaveMovementReservationLog(ReservationMovementLog reservationMovementLog)
        {
            Reservaciones_Movimientos_Log reservationLog = new Reservaciones_Movimientos_Log()
            {
                Fecha = reservationMovementLog.Fecha,
                IdUsuario = reservationMovementLog.IdUsuario,
                Usuario = reservationMovementLog.Usuario,
                NoReservacion = reservationMovementLog.NoReservacion,
                Accion = reservationMovementLog.Accion,
                Data_Antes = reservationMovementLog.Data_Antes,
                Data_Despues = reservationMovementLog.Data_Despues,
                Motivo = reservationMovementLog.Motivo,
                Comentarios = reservationMovementLog.Comentarios
            };

            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Reservaciones_Movimientos_Log.Add(reservationLog);

                    dbContext.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }



        #endregion

    }
}
