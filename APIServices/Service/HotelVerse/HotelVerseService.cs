using System;
using System.Configuration;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using APIServices.Service.HotelVerse.Models;
using APIServices.Service.HotelVerse.Models.Response;


namespace APIServices.Service.HotelVerse
{
    public static class HotelVerseService
    {
        public static ResponseRequest CancelReservation(string recordLocator)
        {
            ResponseRequest response = new ResponseRequest();

            CancelRequest cancelRequest = new CancelRequest();
            cancelRequest.bookingsLocatorToCancelList = new List<Record>();

            Record record = new Record
            {
                hotelverseLocator = string.IsNullOrEmpty(recordLocator)? "0" : recordLocator
            };

            cancelRequest.bookingsLocatorToCancelList.Add(record);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "be/api/cancelReservation");
            request.Content = new StringContent(JsonConvert.SerializeObject(cancelRequest), Encoding.UTF8, "application/json");
            request.Content.Headers.Add("subscription-key", ConfigurationManager.AppSettings["hotelVerseSubscriptionKey"].ToString());

            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["urlApiHotelVerse"].ToString());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseRequest = client.SendAsync(request).Result;

                    response.StatusCode = (int)responseRequest.StatusCode;
                    response.Response = responseRequest.Content.ReadAsStringAsync().Result;
                }

            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Response = ex.Message;
            }

            return response;
        }

    }
}
