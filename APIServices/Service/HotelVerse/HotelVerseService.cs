using System;
using System.Configuration;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using APIServices.Models;
using APIServices.Service.HotelVerse.Models;
using APIServices.Service.HotelVerse.Models.Response;
using System.IO;

namespace APIServices.Service.HotelVerse
{
    public static class HotelVerseService
    {
        public static ResponseRequest CancelReservation(string recordLocator)
        {
            ResponseRequest response = new ResponseRequest();

            CancelRequest cancelRequest = new CancelRequest();
            cancelRequest.bookingsLocatorToCancelList = new List<Record>();

            var infoLocator = recordLocator.Split(':');

            Record record = new Record
            {
                hotelverseLocator = string.IsNullOrEmpty(recordLocator)? "0" : infoLocator[0]
            };

            cancelRequest.bookingsLocatorToCancelList.Add(record);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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

        public static ResponseRequest ConfirmReservation(Reservaciones reservaciones)
        {
            ResponseRequest response = new ResponseRequest();

            ConfirmRequest confirmRequest = new ConfirmRequest();
            confirmRequest.bookingsLocatorToConfirmList = new List<Record>();

            var infoLocator = reservaciones.RecordLocator.Split(':');

            Record record = new Record
            {
                hotelverseLocator = infoLocator[0],
                externalLocator = reservaciones.idReservacion.ToString(),
                clientName = reservaciones.Nombre_cl,
                clientLastName = reservaciones.Apellido_cl,
                clientEmail = reservaciones.Email_cl,
                roomNumber = infoLocator[1]
            };

            confirmRequest.bookingsLocatorToConfirmList.Add(record);

            string url = ConfigurationManager.AppSettings["urlApiHotelVerse"].ToString() + "/be/api/confirmReservation";
            string postData = JsonConvert.SerializeObject(confirmRequest);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            request.Headers.Add("subscription-key", ConfigurationManager.AppSettings["hotelVerseSubscriptionKey"].ToString());


            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    // Convertir la cadena de datos a bytes
                    byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);

                    // Escribir los datos en el flujo de salida
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {


                    response.StatusCode = (int)webResponse.StatusCode;

                    using (Stream responseStream = webResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string responseText = reader.ReadToEnd();
                        response.Response = responseText;
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Response = ex.Message;
            }

            return response;

        }

    }
}
