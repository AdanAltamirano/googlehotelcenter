using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using APIServices.Conflux.Models.Delete;
using APIServices.Conflux.Models.Delete.Response;
using APIServices.Conflux.Helpers.RatesPlan;
using APIServices.Conflux.Helpers.Rooms;
using APIServices.Conflux.OTA.Models.Rates;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIServices.Conflux
{
    public partial class ConfluxService
    {

        public User UserHasPermission(string userEmail)
        {
            string emailsPersmissions = ConfigurationManager.AppSettings["GooglePermission"];

            bool hasPermissions = emailsPersmissions.Contains(userEmail);

            User user = new User
            {
                HasPermissions = hasPermissions
            };

            return user;

        }

        public DeleteResponse UpdateDelete(List<XDocument> documents, string endpoint)
        {
            DeleteResponse res = new DeleteResponse();

            try
            {
                var uri = new Uri(endpoint);

                foreach (XDocument document in documents)
                {
                    DeleteHttpResponse deleteHttpResponse = new DeleteHttpResponse();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = uri,
                        Content = new StringContent(document.ToString())
                    };

                    string result = string.Empty;

                    using (var client = new HttpClient())
                    {

                        client.Timeout = TimeSpan.FromMinutes(50);
                        var response = client.SendAsync(request).Result;

                        result = response.Content.ReadAsStringAsync().Result; //regresa un xml
                    }

                    deleteHttpResponse.Xml = result;
                    deleteHttpResponse.XmlRequest = document.ToString();
                    deleteHttpResponse.IsSuccess = true;
                    res.DeleteHttpResponseList.Add(deleteHttpResponse);

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                res.IsSuccess = true;
            }
            catch(Exception ex)
            {
                res.IsSuccess = false;
                res.Error = new KeyValuePair<string, string>("448", ex.Message);
                var errorsElement = new System.Xml.Linq.XElement("Errors");
                var errorElementProperty = new System.Xml.Linq.XElement("Error");
                errorElementProperty.Add(new System.Xml.Linq.XAttribute("Type", "3"), new System.Xml.Linq.XAttribute("Code", "448"), new System.Xml.Linq.XText(ex.Message));
                errorsElement.Add(errorElementProperty);
                res.Xml = errorsElement.ToString();

            }


            return res;

        }

        public async Task<DeleteResponse> UpdateDeleteAsync(List<XDocument> documents, string endpoint)
        {
            DeleteResponse res = new DeleteResponse();

            try
            {
                var uri = new Uri(endpoint);

                foreach (XDocument document in documents)
                {
                    DeleteHttpResponse deleteHttpResponse = new DeleteHttpResponse();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = uri,
                        Content = new StringContent(document.ToString())
                    };

                    string result = string.Empty;

                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromMinutes(50);

                        
                        var response = await client.SendAsync(request);

                        
                        result = await response.Content.ReadAsStringAsync();
                    }

                    deleteHttpResponse.Xml = result;
                    deleteHttpResponse.XmlRequest = document.ToString();
                    deleteHttpResponse.IsSuccess = true;
                    res.DeleteHttpResponseList.Add(deleteHttpResponse);

                    
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                res.IsSuccess = true;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Error = new KeyValuePair<string, string>("448", ex.Message);
                var errorsElement = new XElement("Errors");
                var errorElementProperty = new XElement("Error");
                errorElementProperty.Add(
                    new XAttribute("Type", "3"),
                    new XAttribute("Code", "448"),
                    new XText(ex.Message)
                );
                errorsElement.Add(errorElementProperty);
                res.Xml = errorsElement.ToString();
            }

            return res;
        }


        public List<XDocument> GetSoapRequests(RateAmountMessages rateAmountMessages)
        {
            List<XDocument> soapRequests = new List<XDocument>();

            List<XElement> requests = Xml.OTA.Request.Rates.HotelRateAmountNotifRQ.CreateHotelRateAmountNotifRQList(rateAmountMessages);

            foreach(var request in requests)
            {
               soapRequests.Add(Xml.Soap.Soap.CreateSoapRequestXml(request));
            }

            return soapRequests;
        }

        public RateAmountMessages GetDeleteMessages(int hotelId,int companyId ,Delete delete)
        {
            string filterRooms = string.Empty;
            string filterRatesPlansPromos = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))"; // Ver si va quedar el mismo filtro


            if ((delete.RoomsList.Length == 1 && delete.RoomsList[0] != 0) || delete.RoomsList.Length >= 2)
            {
                string column = "idTipoHabitacion_Hotel";
                filterRooms = $" AND {column} IN ({string.Join(", ", delete.RoomsList.Select(id => id.ToString()))})";
            }

            List<string> ratePlans = new List<string>(delete.RatePlansList);
            List<string> promotions = new List<string>(delete.PromosList);
            List<string> rooms = RoomsHelper.GetRoomsByHotel(hotelId, filterRooms).Select(dr => dr.ItemArray[22].ToString()).ToList();

            if (delete.RatePlansList.Length == 1 && delete.RatePlansList[0] == "0")
            {
                //Todos los planes
                ratePlans = RatesPlanHelper.GetRatePlansByHotel(hotelId, string.Empty).Select(dr => dr.ItemArray[0].ToString()).ToList();

            }

            if (delete.PromosList.Length == 1 && delete.PromosList[0] == "0")
            {
                //Todas las promos
                promotions = RatesPlanHelper.GetRatePlansPromosByHotel(hotelId, filterRatesPlansPromos).Select(dr => dr.ItemArray[0].ToString()).ToList();

            }

            RateAmountMessages deleteRateAmountMessages = new RateAmountMessages();
            deleteRateAmountMessages.HotelCode = companyId;
            deleteRateAmountMessages.RateAmountMessagesList = new List<RateAmountMessage>();

            if (delete.EnableRatePlans)
            {
                NoPromotionsDeleteMessages(ratePlans,rooms,delete, ref deleteRateAmountMessages);
            }

            if (delete.EnablePromotions)
            {
                PromotionsDeleteMessages(ratePlans,promotions,rooms,delete, ref deleteRateAmountMessages);
            }


            return deleteRateAmountMessages;
        }

        public void NoPromotionsDeleteMessages(List<string> ratePlans, List<string> rooms, Delete delete, ref RateAmountMessages deleteRateAmountMessages)
        {

            foreach (var ratePlan in ratePlans)
            {
                foreach (var room in rooms)
                {
                    
                    RateAmountMessage deleteRateAmountMessage = Helpers.Rates.RatesHelpers.CreateDeleteRateAmountMessage(ratePlan, room, delete.StartDate, delete.EndDate);

                    deleteRateAmountMessages.RateAmountMessagesList.Add(deleteRateAmountMessage);
                }
            }            
        }


        public void PromotionsDeleteMessages(List<string> ratePlans, List<string> promotions, List<string> rooms, Delete delete, ref RateAmountMessages deleteRateAmountMessages)
        {
            foreach(var promotion in promotions)
            {
                foreach(var rateplan in ratePlans)
                {
                    foreach(var room in rooms)
                    {
                        string ratePlanId = promotion + rateplan;

                        RateAmountMessage deleteRateAmountMessage = Helpers.Rates.RatesHelpers.CreateDeleteRateAmountMessage(ratePlanId,room,delete.StartDate,delete.EndDate);

                        deleteRateAmountMessages.RateAmountMessagesList.Add(deleteRateAmountMessage);
                    }
                }
            }
        }



    }
}
