using System;
using System.Linq;
using System.Threading.Tasks;
using APIServices.Models;
using APIServices.Conflux;
using System.Collections.Generic;
using System.Net.Http;
using APIServices.Xml.Soap;
using System.Configuration;

namespace APIServices.GoogleSync
{
    public class GoogleSyncWorker
    {
        public async Task ProcessPendingSyncs()
        {
            using (var db = new OzHotelesEntities())
            {
                var pendingSyncs = db.GoogleSyncHistory
                    .Where(s => s.Status == "Failed" || s.Status == "Pending")
                    .OrderBy(s => s.Timestamp)
                    .Take(50)
                    .ToList();

                foreach (var sync in pendingSyncs)
                {
                    try
                    {
                        var result = await ReplaySync(sync);
                        GoogleSyncAuditService.UpdateSyncStatus(sync.CorrelationId, result.Success, result.Response, result.Error);
                    }
                    catch (Exception ex)
                    {
                        sync.ErrorMessage = "Worker Error: " + ex.Message;
                        db.SaveChanges();
                    }
                }
            }
        }

        private async Task<(bool Success, string Response, string Error)> ReplaySync(GoogleSyncHistory sync)
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["confluxApiUrl"];
                string endpoint = "";

                if (sync.TipoOperacion.Contains("Rate"))
                {
                    endpoint = apiUrl + "pms/ota/rates/update";
                }
                else if (sync.TipoOperacion.Contains("Closure"))
                {
                    endpoint = apiUrl + "pms/ota/restriction/update";
                }

                if (string.IsNullOrEmpty(endpoint)) return (false, null, "Unknown Operation Type");

                using (var client = new HttpClient())
                {
                    var soapRequest = Soap.CreateSoapRequestXml(System.Xml.Linq.XElement.Parse(sync.RequestXML));
                    var content = new StringContent(soapRequest.ToString(), System.Text.Encoding.UTF8, "text/xml");

                    var response = await client.PostAsync(endpoint, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    return (response.IsSuccessStatusCode, responseBody, response.IsSuccessStatusCode ? null : response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
