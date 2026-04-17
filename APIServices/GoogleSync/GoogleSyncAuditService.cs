using System;
using System.Linq;
using APIServices.Models;

namespace APIServices.GoogleSync
{
    public class GoogleSyncAuditService
    {
        public static Guid LogSyncAttempt(int hotelId, string operationType, string requestXml, string ratePlanId = null, int? roomId = null, string user = null)
        {
            using (var db = new OzHotelesEntities())
            {
                var audit = new GoogleSyncHistory
                {
                    IdHotel = hotelId,
                    TipoOperacion = operationType,
                    RequestXML = requestXml,
                    Status = "Pending",
                    Timestamp = DateTime.Now,
                    Usuario = user,
                    RatePlanId = ratePlanId,
                    RoomId = roomId,
                    CorrelationId = Guid.NewGuid()
                };

                db.GoogleSyncHistory.Add(audit);
                db.SaveChanges();
                return audit.CorrelationId;
            }
        }

        public static void UpdateSyncStatus(Guid correlationId, bool success, string responseXml = null, string errorMessage = null)
        {
            using (var db = new OzHotelesEntities())
            {
                var audit = db.GoogleSyncHistory.FirstOrDefault(x => x.CorrelationId == correlationId);
                if (audit != null)
                {
                    audit.Status = success ? "Success" : "Failed";
                    audit.ResponseXML = responseXml;
                    audit.ErrorMessage = errorMessage;
                    db.SaveChanges();
                }
            }
        }
    }
}
