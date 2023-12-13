using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Xml.Linq;
using APIServices.Models;
using APIServices.Conflux.Enum;
using APIServices.Conflux.Models.User;
using APIServices.Conflux.Models.User.Response;
using APIServices.Conflux.OTA.Models.Rates;
using APIServices.Conflux.Models.Rates.Response;
using APIServices.Conflux.Models.Restrictions.Response;
using APIServices.Conflux.Parser.Restriction;
using APIServices.Xml.Soap;
using APIServices.Xml.OTA.Request.Rates;
using APIServices.Xml.OTA.Request.Restrictions;
using Portal.General.Facade;
using Portal.Hotel.Facade;
using Portal.Hotel.Common.Data;
using Portal.General.Common.Data;


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

        public RateResponse UpdateRate(int rateId,DateTime startDate, DateTime endDate, int hotelId ,int companyId, TypeRateEnum typeRate)
        {
            RateResponse res = new RateResponse();

            try
            {
                List<vDayRates> rates = null;
                List<vDayRatesExceptions> ratesExceptions = null;

                switch (typeRate)
                {
                    case TypeRateEnum.RoomRate:
                        rates = APIServices.Conflux.Helpers.Rate.RatesHelpers.GetVDayRate(rateId, startDate, endDate);
                        break;
                    case TypeRateEnum.RoomRatePromotion:
                        ratesExceptions = APIServices.Conflux.Helpers.Rate.RatesHelpers.GetVDayRateException(rateId, startDate, endDate);
                        break;
                }

                var hotel = dbContext.Hoteles.First(h => h.idHotel == hotelId);

                var rateAmountMessages = Parser.Parser.ToRateAmountMessages(rates, ratesExceptions, companyId, hotel.PlusTax, hotel.Impuesto, typeRate);

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
                res.RequestXML = soapRequest.ToString();
                res.IsSuccess = HotelRateAmountNotifRS.IsSuccessRequest(otaRS);
            }
            catch (Exception ex)
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
                res.RequestXML = soapRequest.ToString();
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

        public RestrictionResponse UpdateRestrictions(int hotelId, int companyId)
        {
            RestrictionResponse res = new RestrictionResponse();

            try
            {

                //Armar Requests

                #region Habitaciones
                RoomsHotelData ds = new RoomFacade().getAllRooms(hotelId, 1);
                var activeRooms = ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL].Select("eliminada=false").ToList();
                #endregion

                #region Planes Tarifarios
                RatePlanData dsRatePlans = new RatePlanFacade().GetRatePlanByIdHotel(hotelId.ToString(), idioma: 1, IncluirPaquetesSegmentoK: 1, incluirNetRatesPlan: 1, idAsociacion: -1, DeleteFilter: 1, getPromos: false);
                RatePlanData dsRatePlansPromos = new RatePlanFacade().GetRatePlanByIdHotel(hotelId.ToString(), idioma: 1, IncluirPaquetesSegmentoK: 1, incluirNetRatesPlan: 1, idAsociacion: -1, DeleteFilter: 1, getPromos: true);


                var activeRatePlans = dsRatePlans.Tables[RatePlanData.RATEPLAN_TABLE].Select().ToList();

                string filterPromosDates = "((FechaFin IS NOT NULL AND FechaFin>= '" + DateTime.Now.Date.ToString() + "') OR (FechaFin IS NULL AND PromoEndDate >= '" + DateTime.Now.Date.ToString() + "'))";
                var activeRatePlansPromos = dsRatePlansPromos.Tables[RatePlanData.RATEPLAN_TABLE].Select(filterPromosDates).ToList(); //Validar si no hay promociones en request response y log


                #endregion

                #region Init RestrictionParser
                RestrictionsParser.Init(companyId);
                #endregion

                #region LockGral

                var lockGral = dbContext.spGetLockGralByHotel(hotelId).ToList();

                var availStatusMessagesLockGralNoPromos = RestrictionsParser.ToAvailStatusMessages(lockGral, activeRooms, activeRatePlans);
                var lockGralNoPromosHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockGralNoPromos);
                var lockGralNoPromosSoapRQ = Soap.CreateSoapRequestXml(lockGralNoPromosHotelAvailNotifRQ);

                List<XDocument> lockGralPrioritySoapRQ = new List<XDocument>();
                lockGralPrioritySoapRQ.Add(lockGralNoPromosSoapRQ);

                if (activeRatePlansPromos.Count > 0)
                {
                    var availStatusMessagesLockGralPromos = RestrictionsParser.ToAvailStatusMessages(lockGral, activeRooms, activeRatePlansPromos);
                    var lockGralPromosHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockGralPromos);

                    var lockGralPromosSoapRQ = Soap.CreateSoapRequestXml(lockGralPromosHotelAvailNotifRQ);
                    lockGralPrioritySoapRQ.Add(lockGralPromosSoapRQ);
                }

                #endregion

                #region LockRoomType
                var lockRoomTypes = dbContext.spGetLockRoomTypesByHotel(hotelId).ToList();

                var availStatusMessagesLockRoomTypes = RestrictionsParser.ToAvailStatusMessages(lockRoomTypes);

                var lockRoomTypeHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRoomTypes);

                //Request LockRoomType
                var lockRoomTypeSoapRQ = Soap.CreateSoapRequestXml(lockRoomTypeHotelAvailNotifRQ);

                List<XDocument> lockRoomTypesPrioritySoapRQ = new List<XDocument>();
                lockRoomTypesPrioritySoapRQ.Add(lockRoomTypeSoapRQ);

                #endregion

                #region LockRatePlan

                var lockRatePlans = dbContext.spGetLockRatePlansByHotel(hotelId).ToList();

                var availStatusMessagesLockRatePlans = RestrictionsParser.ToAvailStatusMessages(activeRooms, lockRatePlans);

                var lockRatePlanHotelAvailNotifRQ = HotelAvailNotifRQ.CreateHotelAvailNotifRQ(availStatusMessagesLockRatePlans);

                //Request LockRatePlan
                var lockRatePlanSoapRQ = Soap.CreateSoapRequestXml(lockRatePlanHotelAvailNotifRQ);

                List<XDocument> lockRatePlanPrioritySoapRQ = new List<XDocument>();
                lockRatePlanPrioritySoapRQ.Add(lockRatePlanSoapRQ);

                #endregion


                #region Crear Prioridad Request
                int priorityLockRoomType = Convert.ToInt32(ConfigurationManager.AppSettings["PriorityLockRoomTypes"]); //2
                int priorityratePlanLock = Convert.ToInt32(ConfigurationManager.AppSettings["PriorityLockRatePlans"]); // 1
                int priorityLockGral = Convert.ToInt32(ConfigurationManager.AppSettings["PriorityLockGral"]); //3

                int size = 3;

                //if (lockGral.Count > 0) size++;
                //if (lockRatePlans.Count > 0) size++;
                //if (lockRoomTypes.Count > 0) size++;

                List<List<XDocument>> priorityRequests = new List<List<XDocument>>(size);

                //Init
                for (int i = 0; i < size; i++)
                {
                    priorityRequests.Add(null);
                }


                if (lockRoomTypes.Count > 0) priorityRequests[priorityLockRoomType - 1] = lockRoomTypesPrioritySoapRQ;
                if (lockRatePlans.Count > 0) priorityRequests[priorityratePlanLock - 1] = lockRatePlanPrioritySoapRQ;
                if (lockGral.Count > 0) priorityRequests[priorityLockGral - 1] = lockGralPrioritySoapRQ;
                

                string url = ConfigurationManager.AppSettings["confluxApiUrl"] + "pms/ota/restriction/update"; 
                var uri = new Uri(url);

                /***
                 * NOTA: Request Index se usa para el request de LockGral
                 * porque manda dos request, uno para cuando es no promociones y otro para cuando son promociones
                 * los demas por ahora mandan uno.
                */

                int requestIndex = 0;
                int index = 0;

                foreach (var priorityRequest in priorityRequests)
                {
                    Restriction restriction = new Restriction();

                    if (priorityRequest != null) 
                    { 

                        foreach (var soapRequest in priorityRequest)
                        {
                            //Request

                            System.Xml.Linq.XElement otaRS = null;
                            HttpContent httpContent = new StringContent(soapRequest.ToString());

                            using (var client = new HttpClient())
                            {

                                client.Timeout = TimeSpan.FromMinutes(50);
                                var response = client.PostAsync(uri, httpContent).Result;

                                string result = response.Content.ReadAsStringAsync().Result; //regresa un xml

                                otaRS = HotelAvailNotifRS.ParseHotelAvailNotifRS(result); //Cambiar

                            }

                            //Repuesta API
                            restriction.Xml.Add(otaRS.ToString());
                            restriction.XmlRequest.Add(soapRequest.ToString());

                            if (requestIndex == 0)
                            {
                                restriction.IsSuccess = HotelAvailNotifRS.IsSuccessRequest(otaRS);
                            }

                            if (requestIndex == 1)
                            {
                                restriction.IsSuccessPromo = HotelAvailNotifRS.IsSuccessRequest(otaRS);
                            }

                            requestIndex++;

                        }

                        if ((index + 1) == priorityLockGral)
                        {
                            restriction.Type = Enum.RestrictionEnum.LockGral;
                        }
                        else if ((index + 1) == priorityratePlanLock)
                        {
                            restriction.Type = Enum.RestrictionEnum.LockRatePlan;
                        }
                        else if ((index + 1) == priorityLockRoomType)
                        {
                            restriction.Type = Enum.RestrictionEnum.LockRoomType;
                        }

                        res.Restrictions.Add(restriction);

                        requestIndex = 0;
                        
                    }

                    index++;

                }

                res.IsSuccess = true;
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

            #endregion

            return res;
        }

        public RateResponse DeleteRates(RateAmountMessages rateAmountMessages)
        {
            RateResponse res = new RateResponse();

            try
            {
                var xml = HotelRateAmountNotifRQ.CreateHotelRateAmountNotifRQ(rateAmountMessages);

                var soapRequest = Soap.CreateSoapRequestXml(xml);

                HttpContent httpContent = new StringContent(soapRequest.ToString());

                string url = ConfigurationManager.AppSettings["confluxApiUrl"] + "pms/ota/rates/delete";

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
                res.RequestXML = soapRequest.ToString();
                res.IsSuccess = HotelRateAmountNotifRS.IsSuccessRequest(otaRS);

            }
            catch (Exception ex)
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
