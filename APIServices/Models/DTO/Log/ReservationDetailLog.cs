using System;
using System.Collections.Generic;

namespace APIServices.Models.DTO.Log
{
    public class ReservationDetailLog
    {
        public CustomerLog CustomerBefore { get; set; } = null;
        public CustomerLog CustomerAfter { get; set; } = null;
        public CheckDatesLog CheckDatesBefore  { get; set; } = null;
        public CheckDatesLog CheckDatesAfter { get; set; } = null;
        public List<RoomDetailsLog> RoomDetailsBefore { get; set; } = null;
        public List<RoomDetailsLog> RoomDetailsAfter { get; set; } = null;
        public TotalDetailsLog TotalDetailsBefore { get; set; } = null;
        public TotalDetailsLog TotalDetailsAfter { get; set; } = null;
    }

    public class CheckDatesLog
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }

    public class CustomerLog
    {
        public string FullName { get; set; } = string.Empty;
    }

    public class RoomDetailsLog
    {
        public string RoomCode { get; set; }
        public string Name { get; set; }
        public int Adults { get; set; }
        public int ExtraAdults { get; set; }
        public int Childrens { get; set; }
        public int ExtraChildrens { get; set; }
        public string AgeChildren { get; set; }
        public List<RoomPriceDetailsLog> PriceDetails { get; set; } = null;
        public double Total { get; set; }
        public double TotalNR { get; set; }
        public string Currency { get; set; }
    }

    public class RoomPriceDetailsLog 
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

    public class TotalDetailsLog
    {
        public double SubTotal { get; set; }
        public double SubTotalNR { get; set; }
        public double Total { get; set; }
        public double Taxes { get; set; }
        public double TaxesHotel { get; set; }
        public double Commission { get; set; }
        public bool IncludesTax { get; set; }
        public double Ecotasa { get; set; }
        public double TotalNR { get; set; }
        public string Currency { get; set; }
    }

}
