using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Models.User;
using APIServices.Conflux.Models.User.Response;
using APIServices.Xml.OTA.Request.Rates;
using APIServices.Xml.Soap;

namespace APIServices.Conflux
{
    public class ConfluxService
    {
        private static readonly HttpClient client = new HttpClient();

        private OzHotelesEntities dbContext = new OzHotelesEntities();

        public IQueryable<vHotelActives> GetHotels() => dbContext.vHotelActives.AsQueryable();

        public UserReponse CreateUser(User user)
        {
            UserReponse response = new UserReponse();

            int? companyId = user.CompanyId;
            int? hotelId = user.HotelId;
            string username = user.Username;
            string password = user.Password;
            bool? addToHotelPms = user.AddToHotelPms;

            int? userExists = -1;

            try
            {
                using (OzUniEntities uniEntities = new OzUniEntities())
                {
                    var result = uniEntities.spCreateUserConfigurationConectivity(companyId, hotelId, username, password, addToHotelPms);

                    uniEntities.SaveChanges();

                    userExists = result.ToList()[0];
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            //Se creo el usuario
            if (userExists == 1)
            {
                response.IsSuccess = true;
            }
            //Ya existe el usuario
            else if (userExists == 0) {

                response.IsSuccess = false;
                response.Error = new KeyValuePair<string, string>("501","Usuario Ya existe");
            }
            else if(userExists == -1)
            {
                response.IsSuccess = false;
                response.Error = new KeyValuePair<string, string>("500","Error del Sistema");
            }


            return response;
        }

        public bool UpdateRates(int hotelId, int companyId)
        {
            bool isSuccess = false;

            try
            {
                var currentRates = dbContext.spGetCurrentRatesByHotel(hotelId).ToList();

                var rateAmountMessages = Parser.Parser.ToRateAmountMessages(currentRates, companyId);

                var xml = HotelRateAmountNotifRQ.CreateHotelRateAmountNotifRQ(rateAmountMessages);

                var soapRequest = Soap.CreateSoapRequestXml(xml);

                HttpContent httpContent = new StringContent(soapRequest.ToString());

                string url = ConfigurationManager.AppSettings["confluxApiUrl"] + "pms/ota/rates/update";

                var uri = new Uri(url);

                var response = client.PostAsync(uri, httpContent).Result;

                string result = response.Content.ReadAsStringAsync().Result; //regresa un xml

                var otaRS = HotelRateAmountNotifRS.ParseHotelRateAmountNotifRS(result);

                isSuccess = HotelRateAmountNotifRS.IsSuccessRequest(otaRS);

            }
            catch(Exception ex)
            {
                isSuccess = false;
            }
           

            return isSuccess;

        }

    }
}
