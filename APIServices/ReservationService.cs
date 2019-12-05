using APIServices.Models;
using APIServices.Models.DTO;
using PortalLibraries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using APIServices.Extension;

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

        public List<Excel.ReservationList> GetExcel()
        {
            var result = dbContext.vReservation.Select(p => new Excel.ReservationList
            {
                NoReservation = p.ConfirmNumber,
                Hotel = p.Hotel,
                Customer = p.Client,
                Date = p.ReservationDate.ToString("YYYY MM DD"),
                CheckIn = p.CheckIn.ToString("YYYY MM DD"),
                CheckOut = p.CheckOut.ToString("YYYY MM DD"),
                Origin = Excel.ReservationList.GetOrigin(p.Source),
                Status = Excel.ReservationList.GetStatus(p.Status)
            }).ToList();
            return result;
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
                model.ReservationNumber = details.reservationNumber;
                model.ReservationId = reservationId;
                model.CancellationNumber = details.cancellationNumber;
                model.HotelName = details.hotelName;
                model.HotelId = details.hotelId;
                model.CompanyId = details.companyId;
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
                if (isHotelCompany || (showCreditCard.HasValue ? showCreditCard.Value : false))
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
                double totalTax = (double)(details.IsNetRateUV ? details.totalNetRate : details.total);

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
                    Img = $"{ConfigurationManager.AppSettings["pathimgrooms"] ?? ""}/{companyId}/{room.roomTypeId}",
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

                if (MinimumDaysToCancel(rsv.checkIn, minimumDays))
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
        
    }
}
