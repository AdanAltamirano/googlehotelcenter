using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Overload pensado para respuestas SOAP: cuando el servicio responde con &lt;Errors&gt; sin lanzar excepcion,
        /// el campo Error de la respuesta queda como KeyValuePair default y Error.Value es null.
        /// En ese caso usamos el responseXml como ErrorMessage para no perder la traza del error en la auditoria.
        /// </summary>
        public static void UpdateSyncStatus(Guid correlationId, bool success, string responseXml, KeyValuePair<string, string> error)
        {
            string errorMessage;
            if (success)
            {
                errorMessage = string.Empty;
            }
            else if (!string.IsNullOrEmpty(error.Value))
            {
                errorMessage = error.Value;
            }
            else
            {
                // Fallback: el SOAP respondio con <Errors> pero sin contenido en Error.Value.
                // Guardamos el XML crudo de la respuesta para poder diagnosticar el rechazo.
                errorMessage = responseXml;
            }

            UpdateSyncStatus(correlationId, success, responseXml, errorMessage);
        }

        public static SyncHistoryResult GetHistory(int hotelId, int pageSize = 50, int page = 1, string status = null, string tipoOperacion = null)
        {
            using (var db = new OzHotelesEntities())
            {
                var query = db.GoogleSyncHistory.Where(x => x.IdHotel == hotelId);

                if (!string.IsNullOrEmpty(status))
                    query = query.Where(x => x.Status == status);

                if (!string.IsNullOrEmpty(tipoOperacion))
                    query = query.Where(x => x.TipoOperacion == tipoOperacion);

                int total = query.Count();
                int successCount = query.Count(x => x.Status == "Success");
                int failedCount = query.Count(x => x.Status == "Failed");
                int pendingCount = query.Count(x => x.Status == "Pending");

                var items = query
                    .OrderByDescending(x => x.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new SyncHistoryItem
                    {
                        IdSync = x.IdSync,
                        Timestamp = x.Timestamp,
                        TipoOperacion = x.TipoOperacion,
                        Status = x.Status,
                        RatePlanId = x.RatePlanId,
                        RoomId = x.RoomId,
                        Usuario = x.Usuario,
                        ErrorMessage = x.ErrorMessage,
                        RequestXML = x.RequestXML,
                        ResponseXML = x.ResponseXML
                    })
                    .ToList();

                return new SyncHistoryResult
                {
                    Total = total,
                    SuccessCount = successCount,
                    FailedCount = failedCount,
                    PendingCount = pendingCount,
                    Items = items
                };
            }
        }
    }

    public class SyncHistoryItem
    {
        public int IdSync { get; set; }
        public DateTime Timestamp { get; set; }
        public string TipoOperacion { get; set; }
        public string Status { get; set; }
        public string RatePlanId { get; set; }
        public int? RoomId { get; set; }
        public string Usuario { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestXML { get; set; }
        public string ResponseXML { get; set; }
    }

    public class SyncHistoryResult
    {
        public int Total { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public int PendingCount { get; set; }
        public List<SyncHistoryItem> Items { get; set; }
    }
}
