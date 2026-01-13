using System;
using System.Collections.Generic;

namespace APIServices.Models.DTO
{
    public class ReservationDetailsModel
    {
        public string ReservationNumber { get; set; }
        public string ReservationNumberIDS { get; set; }
        public string CancellationNumber { get; set; }
        public string HotelName { get; set; }
        public string HotelEmail { get; set; }
        public int? HotelId { get; set; }
        public int? CompanyId { get; set; }
        public Nullable<int> CorporateId { get; set; }
        public string CorporateName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Status { get; set; }
        public string AccessCode { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int Nights { get; set; }
        public int ReservationId { get; set; }
        public string RatePlan { get; set; }
        public string Source { get; set; }
        public string Portal { get; set; }
        public byte TypeCC { get; set; }
        public string DescriptionCC { get; set; }
        public string CollectedBy { get; set; } = string.Empty;
        public string PaymentInformation { get; set; } = string.Empty;
        public bool IsNetRateUV { get; set; }
        public Pms Pms { get; set; }
        public CustomerDetails Customer { get; set; }
        public List<RoomDetails> RoomDetails { get; set; }
        public TotalDetails TotalDetails { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        /**
        * paymentType
        * 0 = bank deposit
        * 1 = online payment
        * 2 = payment at the hotel
        */
        public int? PaymentWay{ get; set; }
        public BankDepositDetails BankDepositDetails { get; set; }
        public PolicyDetails PolicyDetails { get; set; }
        public AgencyDetails AgencyDetails { get; set; }

        public bool AllowsCancel { get; set; } = false;
        public bool AllowsModify { get; set; } = false;
        public bool AllowsReactivate { get; set; } = false;
        public bool AllowsConfirm { get; set; } = false;
        public bool AllowsSendConfirmationEmail { get; set; } = false;

        public string CancellationReason { get; set; }
        public string ModificationReason { get; set; }
        public string Agency { get; set; }
        public string AgencyUser { get; set; }
        public string RatePlanPromotion { get; set; }
        public string NamePromotion { get; set; }
        public Nullable<int> IdCancellationUser  { get; set; }
        public string UserCancellation { get; set; }
        public bool ShowLogs { get; set; }
        public List<HotelItem> ReservationItemsDetails { get; set; } = new List<HotelItem>();
    }

    public class CustomerDetails
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CardDetails CardDetails { get; set; }
    }

    public class CardDetails
    {
        public bool IsSuccess { get; set; } = false;
        public bool AllowsShowCreditCardData { get; set; } = false; //Tiene que ver con permisos para ver la informacion desencryptada de la tarjeta
        public bool ShowBasicCreditCardData { get; set; } = false; //Mostrar informacion basica de la tarjeta(solo el numero de tarjeta con 4 digitos visibles)
        public string CardType { get; set; } = null;
        public string Number { get; set; } = null;
        public string YearExpiration { get; set; } = null;
        public string MonthExpiration { get; set; } = null;
        public string Cvv { get; set; } = null;
        public string Owner { get; set; } = null;
        public string Img { get; set; }

        public string GetCardType(string card)
        {
            string type = string.Empty;
            if (card.Length > 0)
            {
                switch (card.Substring(0, 1))
                {
                    case "3":
                        type = "AMERICAN EXPRESS"; break;
                    case "4":
                        type = "VISA"; break;
                    case "5":
                        type = "MASTER CARD"; break;
                }
            }
            return type;
        }
    }

    public class PaymentDetails
    {
        public int ReservationId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime ReservationDate { get; set; }
        public Byte Status { get; set; }
        public string Source { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public string Pasarela { get; set; }
        public string Reference { get; set; }
        public string AuthorizationNumber { get; set; }
        public Nullable<decimal> TotalPay { get; set; }
        public string CurrencyPay { get; set; }
    }

    public class BankDepositDetails
    {
        public double Total { get; set; }
        public string Currency { get; set; }
        public string Bank { get; set; }
        public string Reference { get; set; }
        public bool HasDebt { get; set; }
        public double Debt { get; set; }
        public string Target { get; set; }

    }

    public class RoomDetails
    {
        public int ReservationId { get; set; }
        public int RoomPriceId { get; set; }
        public string RoomCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Preferences { get; set; }
        public int Adults { get; set; }
        public int ExtraAdults { get; set; }
        public int Childrens { get; set; }
        public int ExtraChildrens { get; set; }
        public string AgeChildren { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string RatePlan { get; set; }
        public string RateCode { get; set; }
        public List<RoomPriceDetails> PriceDetails { get; set; }
        public double Total { get; set; }
        public double TotalNR { get; set; }
        public string Img { get; set; }
        public string ImgDefault { get; set; }
        public int NetRateContract { get; set; }
        public string Currency { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public string RatePlanPromotion { get; set; }
        public string NamePromotion { get; set; }
        public List<PromosRateReservation> PromosRateReservation { get; set; } = new List<PromosRateReservation>();
    }

    public class RoomPriceDetails
    {
        public int RoomPriceId { get; set; }
        public double Price { get; set; }
        public double ExtraPrice { get; set; }
        public double PriceNR { get; set; }
        public double ExtraPriceNR { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Currency { get; set; }
    }

    public class TotalDetails
    {
        public double SubTotal { get; set; }
        public double SubTotalNR { get; set; }
        public double Total { get; set; }
        public double Taxes { get; set; }
        public double  TaxesHotel { get; set; }
        public double Commission { get; set; }
        public bool IncludesTax { get; set; }
        public double Ecotasa { get; set; }
        public double TotalNR { get; set; }
        public string Currency { get; set; }
    }

    public class PolicyDetails
    {
        public string HotelGuarantee { get; set; }
        public string RatePlanGuarantee { get; set; }
        public string HotelCancellation { get; set; }
        public string RatePlanCancellation { get; set; }
        public string HotelCreditCard { get; set; }
        public string RatePlanCreditCard { get; set; }
    }

    public class Pms
    {
        public bool Status { get; set; }
        public string Action { get; set; }
        public int? FailedAttempts { get; set; }
        public string ReservationNumber { get; set; }
        public string PmsCode { get; set; }
        /***
         * 1 = Marcar como no verificada boton,  cambiaria el pms status en 0 y reiniciar el failed attempts
         * 0 =  verificar reservacion button ver pagina cambiar pms status en 1 y guardaria el pmscode
         */
        public bool VerifyAction { get; set; } 
    }

    public class HotelItem
    {
        public int HotelItemReservationId { get; set; }
        public int ReservationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; } = "";
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public string Abbreviation { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public bool AllowPaymentDestination { get; set; }

    }

    public class AgencyDetails
    {
        public bool HasInfoAgency { get; set; } = false;
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AgentName { get; set; }
        public string AgentLastName { get; set; }

    }

    public class PromosRateReservation
    {
        public string PromoCode { get; set; }
        public string Name { get; set; }

    }

}
