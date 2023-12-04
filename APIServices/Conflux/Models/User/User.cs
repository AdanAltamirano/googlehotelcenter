namespace APIServices.Conflux.Models.User
{
    public class User
    {
        public int CompanyId { get; set; }
        public int HotelId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool AddToHotelPms { get; set; }
    }
}
