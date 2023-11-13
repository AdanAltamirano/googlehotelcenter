using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using APIServices.Models;
using APIServices.Conflux.Models.User;
using APIServices.Conflux.Models.User.Response;
using APIServices.Conflux.Models.Rates.Response;
using APIServices.Xml.OTA.Request.Rates;
using APIServices.Xml.Soap;

namespace APIServices.Conflux
{
    public class ConfluxService
    {
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

        public RateResponse UpdateRates(int hotelId, int companyId)
        {
            RateResponse res = new RateResponse();

            try
            {
                var currentRates = dbContext.spGetCurrentRatesByHotel(hotelId).ToList();

                var hotel = dbContext.Hoteles.First(h => h.idHotel == hotelId);


                var rateAmountMessages = Parser.Parser.ToRateAmountMessages(currentRates, companyId, hotel.PlusTax, hotel.Impuesto);

                var xml = HotelRateAmountNotifRQ.CreateHotelRateAmountNotifRQ(rateAmountMessages);

                var soapRequest = Soap.CreateSoapRequestXml(xml);

                HttpContent httpContent = new StringContent(soapRequest.ToString());

                string url = ConfigurationManager.AppSettings["confluxApiUrl"] + "pms/ota/rates/update";

                var uri = new Uri(url);

                System.Xml.Linq.XElement otaRS = null;

                using (var client = new HttpClient())
                {

                    client.Timeout = TimeSpan.FromMinutes(50);
                    var response = client.PostAsync(uri, httpContent).Result;

                    string result = response.Content.ReadAsStringAsync().Result; //regresa un xml

                    otaRS = HotelRateAmountNotifRS.ParseHotelRateAmountNotifRS(result);
                }

                res.Xml = otaRS.ToString();
                res.IsSuccess = HotelRateAmountNotifRS.IsSuccessRequest(otaRS);

            }
            catch(Exception ex)
            {
                res.IsSuccess = false;
                res.Error = new KeyValuePair<string, string>("448", ex.Message);

                var errorsElement = new System.Xml.Linq.XElement("Errors");
                var errorElementProperty = new System.Xml.Linq.XElement("Error");
                errorElementProperty.Add(
                    new System.Xml.Linq.XAttribute("Type", "3"),
                    new System.Xml.Linq.XAttribute("Code", "448"),
                    new System.Xml.Linq.XText(ex.Message));

                errorsElement.Add(errorElementProperty);

                res.Xml = errorsElement.ToString();

            }


            return res;

        }

    }
}
