using APIServices.Models;
using APIServices.Models.DTO;
using PortalLibraries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace APIServices
{
    public class ReservationService
    {
        public OzHotelesEntities dbContext = new OzHotelesEntities();


        public IQueryable<vReservation> GetAll() => dbContext.vReservation.AsQueryable();

        public IQueryable<vReservation> Get(int hotelId)
        {
            return dbContext.vReservation.AsQueryable()
                .Where(x => x.HotelId == hotelId);
        }



        #region detalles de la reserva

        public vReservationDetails GetReservation(int reservationId)
        {
            return dbContext.vReservationDetails
                .FirstOrDefault(x => x.reservationId == reservationId);
        }

        public ReservationDetailsModel GetDetails(int reservationId, bool isSupervisor, bool isHotelCompany, int userId)
        {

            var details = GetReservation(reservationId);

            var model = new ReservationDetailsModel();
            if (details != null)
            {
                model.ReservationNumber = details.reservationId.ToString();
                model.CancellationNumber = details.cancellationNumber;
                model.HotelName = details.hotelName;
                model.Address = details.address;
                model.Status = details.status;
                model.ReservationDate = details.reservationDate;
                model.CheckIn = details.checkIn;
                model.CheckOut = details.checkOut;
                model.Nights = (details.checkOut - details.checkIn).Days;
                model.City = details.city;
                model.Country = details.country;
                model.AccessCode = details.accessCode;
                model.Source = details.source;
                model.CancellationReason = details.cancellationReason;
                model.Portal = details.Portal;
                model.PaymentWay = details.paymentType;
                model.BankDepositDetails = new BankDepositDetails();
                if (details.paymentType == 0)
                {
                    model.BankDepositDetails.Total = (double)details.depositAmount;
                    model.BankDepositDetails.Currency = details.depositCurrency;
                    model.BankDepositDetails.Reference = details.depositReference;
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
                    ReservationNumber = details.pmsReservationNumber
                };

                /*credit card*/
                var showCreditCard = dbContext.vPermissions
                    .FirstOrDefault(x => x.userId == userId)?.showCreditCard;

                if (!string.IsNullOrEmpty(details.cardNumber))
                {
                    string cc = crypto.DecryptString128Bit(details.cardNumber, crypto.PublicKey);
                    model.Customer.CardDetails.CardType = model.Customer.CardDetails.GetCardType(cc);
                    cc = $"XXXXXXXXXXXX{cc.Substring(cc.Length - 4)}";
                    model.Customer.CardDetails.Number = cc;
                    model.Customer.CardDetails.IsSuccess = true;
                }
                if (isHotelCompany || showCreditCard.Value)
                    model.Customer.CardDetails.AllowsShowCreditCardData = true;
                /*fin credit card*/

                double totalRooms = 0;
                GetRooms(ref model, reservationId, details.companyId, out totalRooms);

                model.TotalDetails = new TotalDetails();
                model.TotalDetails.SubTotal = totalRooms;
                model.TotalDetails.TotalNR = (double)details.totalNetRate;
                model.TotalDetails.Total = (double)details.total;
                model.TotalDetails.IncludesTax = details.includesTax.Value;

                double tax = (double)details.tax;
                double totalTax = (double)(details.IsNetRateUV.Value ? details.totalNetRate : details.total);

                model.TotalDetails.Taxes = Math.Round(totalTax - (totalTax / ((tax / 100) + 1)), 2);
                model.TotalDetails.Currency = details.currency;

                Permissions(ref model, isSupervisor);
            }

            return model;
        }

        public List<vReservationRoomDetails> GetRoomsReservation(int reservationId)
        {
            return dbContext.vReservationRoomDetails
                .Where(x => x.reservationId == reservationId)
                .ToList();
        }

        void GetRooms(ref ReservationDetailsModel model, int reservationId, int companyId, out double totalRooms)
        {
            var rooms = GetRoomsReservation(reservationId);

            model.RoomDetails = new List<RoomDetails>();
            int index = 0;
            double totalRoom = 0;
            foreach(var room in rooms)
            {
                var prices = dbContext.VReservationRoomPriceDetails
                    .Where(x => x.roomPriceId == room.roomPriceId)
                    .ToList();

                var priceDetails = new List<RoomPriceDetails>();
                double totalPerRoom = 0;
                foreach(var price in prices)
                {
                    int nights = (price.checkOut - price.checkIn).Days + 1;
                    totalPerRoom += (double)price.price * nights;
                    priceDetails.Add(new RoomPriceDetails
                    {
                        Price = (double)price.price,
                        ExtraPrice = (double)price.extraPrice,
                        Currency = price.currency,
                        CheckIn = price.checkIn,
                        CheckOut = price.checkOut,
                    });
                }

                totalRoom += totalPerRoom;

                model.RoomDetails.Add(new RoomDetails
                {
                    RoomCode = room.roomCode,
                    Name = room.roomName,
                    //Description = room.roomDescription,
                    Preferences = room.roomPreferences,
                    CheckIn = room.roomCheckIn,
                    CheckOut = room.roomCheckOut,
                    Adults = room.adults,
                    ExtraAdults = (int)room.extraAdults,
                    Childrens = room.childrens,
                    ExtraChildrens = (int)room.extraChildrens,
                    AgeChildren = room.ageChildren,
                    PriceDetails = new List<RoomPriceDetails>(),
                    Total = totalPerRoom,
                    Currency = room.currency,
                    RatePlan = room.ratePlan,
                    RateCode = room.rateCode,
                    Img = $"http://test.univisit.com/RateManager/ozportalglobal/Images/Rooms/{companyId}/{room.roomTypeId}",
                    CustomerName = room.customerName ?? "",
                    CustomerLastName = room.customerLastName ?? "",
                });

                model.RoomDetails[index].PriceDetails.AddRange(priceDetails);
                index++;
            }

            totalRooms = totalRoom;
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
        void Permissions(ref ReservationDetailsModel model, bool isSupervisor)
        {
            string source = model.Source;
            switch (model.Status)
            {
                case 1:
                    if (isSupervisor)
                    {
                        model.AllowsCancel = true;
                        model.AllowsModify = true;
                    }
                    break;
                case 3:
                    
                    break;
                case 4:
                    if (isSupervisor)
                    {
                        model.AllowsCancel = true;
                        model.AllowsModify = true;
                    }
                    break;
            }
        }
        #endregion








        #region cancelar reserva

        public CancelBookingRS Cancel(vReservationDetails rsv, int userId, string reasonToCancel)
        {
            if (rsv != null)
            {
                var minimumDays = dbContext.Hoteles
                    .FirstOrDefault(x => x.idHotel == rsv.hotelId)?.DiasMinCancelar ?? 0;

                if (!MinimumDaysToCancel(rsv.checkIn, minimumDays))
                {
                    return new CancelBookingRS
                    {
                        Error = $"No se cumple con el mínimo de días para cancelar. La reserva debe ser cancelada {minimumDays} días antes de la llegada",
                    };
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
                    dbContext.spModificarReservacionByid(reservationId, req.Name, req.LastName, (decimal)req.Total, 
                        (decimal)req.TotalNR, req.CheckIn, req.CheckOut);
                    transaction.Commit();
                    res.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    res.Error = $"failed to modify: {ex.InnerException.Message}";
                    transaction.Rollback();
                }
            }
            return res;
        }
        #endregion
    }
}
