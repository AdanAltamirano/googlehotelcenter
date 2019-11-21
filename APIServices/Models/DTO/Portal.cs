namespace APIServices.Models.DTO
{
    public class Portal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Corp { get; set; }
        public string WebPage { get; set; }
    }

    public class Coorporative
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? CompanyId { get; set; }
        public string Email { get; set; }
        public bool? Billing { get; set; }
        public bool? Catalog { get; set; }
        public int? Type { get; set; }
    }

    public class NewPortal
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Application { get; set; }
        public int? CoorporativeId { get; set; }
        public int? AsosciationId { get; set; }
        public int? SegmentId { get; set; }
        public bool? Active { get; set; }
        public int? AfiliationId { get; set; }
        public int? CompanyId { get; set; }  
    }
}
