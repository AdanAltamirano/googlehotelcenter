namespace APIServices.Models.DTO.Reservation.Deposit.Request
{
    public class ReservationDepositDTO
    {
        public int UserId { get; set; }
        public int ReservationId { get; set; }
        public string Reference { get; set; }
        public string AuthorizationNumber { get; set; }
        public string NumberAccount { get; set; }
        public string Bank { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public System.DateTime DepositDate { get; set; }
        public string Details { get; set; }

    }
}
