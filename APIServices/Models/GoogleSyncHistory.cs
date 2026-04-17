namespace APIServices.Models
{
    using System;
    using System.Collections.Generic;

    public partial class GoogleSyncHistory
    {
        public int IdSync { get; set; }
        public int IdHotel { get; set; }
        public string RequestXML { get; set; }
        public string ResponseXML { get; set; }
        public string Status { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Usuario { get; set; }
        public string TipoOperacion { get; set; }
        public string RatePlanId { get; set; }
        public Nullable<int> RoomId { get; set; }
        public string ErrorMessage { get; set; }
        public System.Guid CorrelationId { get; set; }
    }
}
