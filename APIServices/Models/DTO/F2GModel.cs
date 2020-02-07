using System.Collections.Generic;

namespace APIServices.Models.DTO
{
    public class F2GModel
    {
        public string RatePlanIdF2G { get; set; }
        public string PromoCode { get; set; }
        public bool hasPromo { get; set; }
        public List<F2GMappedHotels> HotelsMapped { get; set; }
        public List<F2GHotel> Hotels { get; set; } //Mostrar
        public List<F2GRate> Rates { get; set; } //Mostrar
        public bool AddState { get; set; }
        public bool DeleteState { get; set; }
        public List<F2GHotel> Hotel { get; set; } //Guardar
        public List<F2GRate> RatesList { get; set; } //Guardar
    }

    public class F2GMappedHotels
    {
        public int CompanyId { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string RatePlanIdUv { get; set; }
        public string RatePlanF2GId { get; set; }
        public string Promo { get; set; }
    }

    public class F2GHotel
    {
        public int HotelId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public int? CorpId { get; set; }
    }

    public class F2GRate
    {
        public string RatePlanId { get; set; }
        public string Description { get; set; }
        public string RateCode { get; set; }
        public string Name { get; set; }
    }

    public class F2GCorporate
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public int Id { get; set; }
    }

    public class F2GSaveModel
    {
        public string F2GId { get; set; }
        public string Promo { get; set; }
        public List<F2GHotel> Hotels { get; set; } //Guardar
        public List<F2GRate> Rates { get; set; } //Guardar
        public int Action { get; set; }

    }

    public class F2GHotelsRates
    {
        public List<F2GHotel> Hotels { get; set; }
        public List<F2GRate> Rates { get; set; }
    }

}
