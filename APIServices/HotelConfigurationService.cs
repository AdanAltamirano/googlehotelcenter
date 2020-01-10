using APIServices.Models;
using APIServices.Models.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using APIServices.Extension;



namespace APIServices
{
    public class HotelConfigurationService
    {
        OzHotelesEntities DbContext = new OzHotelesEntities();
        OzUniEntities DbContextUnivisit = new OzUniEntities();

       /// <summary>
       /// Get emails by hotel
       /// </summary>
       /// <param name="hotelId"></param>
       /// <returns>emails</returns>
        public string Emails(int hotelId)
        {
            var emails = DbContext.Hoteles.FirstOrDefault(h => h.idHotel == hotelId).EmailReservas;
        
            return emails;
        } 


        /// <summary>
        /// Search attributes from hotel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lang"></param>
        /// <returns>All errors from dates</returns>
        public Dictionary<string, Dictionary<string, object>> Dates(int id, string lang)
        {
            var dates = DbContext.Hoteles
                .Where(h => h.idHotel == id)
                .Select(s => new Dates
                {
                    Availability = s.StatusAvailability,
                    AvailablePortal = s.AvailOnPortal,
                    AllowBankDeposit = s.AllowBankDeposit,
                    PropertyNumber = s.PropertyNumber
                }).FirstOrDefault();

            Dictionary<string, Dictionary<string, object>> status =
                new Dictionary<string, Dictionary<string, object>>();

            string errors = null;
            errors = (lang.Equals("en-US")) ? errors.ReadResourceFile("APIServices.Errors.xml") :
                errors.ReadResourceFile("APIServices.ErrorsEs.xml");
            XmlDocument xm = new XmlDocument();
            xm.LoadXml(errors);
            bool[] hotelConfig = new bool[4];
            hotelConfig[0] = (dates != null && (dates.Availability != null && dates.Availability.Equals("O")))? true : false;
            hotelConfig[1] = (dates != null && dates.AvailablePortal == true)? true : false;
            hotelConfig[2] = (dates != null && dates.AllowBankDeposit == true)? true : false;
            //hotelConfig[3] = (dates != null && dates.PropertyNumber.Equals(id.ToString()))? true : false;
            hotelConfig[3] = (dates != null && !string.IsNullOrEmpty(dates.PropertyNumber)) ? true : false;
            status.CreateResponse(xm,hotelConfig,"dates",lang);
            return status;
        }
        
        /// <summary>
        /// Check if hotel has enable credit cards
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lang"></param>
        /// <returns>All errors from Credit cards</returns>
        public Dictionary<string,Dictionary<string,object>> CreditCard(int id, string lang)
        {
            bool creditCard = DbContext.TarjetasHotel.Any(t => t.IdHotel == id);
            bool message = false;
            if(creditCard == false)
            {
                using (System.Data.Entity.DbContextTransaction transaction = DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            TarjetasHotel tarjeta = new TarjetasHotel(); 
                            tarjeta.IdHotel = id;
                            tarjeta.Code = (i == 0) ? "AX"
                                : (i == 1) ? "VI"
                                : "CA";
                            DbContext.TarjetasHotel.Add(tarjeta);
                        }

                        DbContext.SaveChanges();
                        transaction.Commit();
                        message = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                       
                    }
                  
                }
            }
         
        Dictionary<string, Dictionary<string, object>> status =
                new Dictionary<string, Dictionary<string, object>>();
            string errors = null;
            errors = (lang.Equals("en-US")) ? errors.ReadResourceFile("APIServices.Errors.xml") :
                errors.ReadResourceFile("APIServices.ErrorsEs.xml");
            XmlDocument xm = new XmlDocument();
            xm.LoadXml(errors);
            bool[] hotelconfig = new bool[2];
            hotelconfig[0] = (creditCard) ? true : false;
            hotelconfig[1] = message;
            status.CreateResponse(xm,hotelconfig,"creditcard",lang);
            return status;
        }


        /// <summary>
        /// Create a list of rate plans by hotel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lang"></param>
        /// <returns>List of rateplans with properties and portals</returns>
        public List<RatePlansByHotel> EnableAvailability(int id, string lang)
        {
            List<RatePlanHotelConfiguration> ratePlan = DbContext.RatesPlan
                .Where(r => r.IdHotel == id 
                && (r.Deleted == null || r.Deleted == false) 
                && (r.IsPromo == null || r.IsPromo == false))
                .Select(s => new RatePlanHotelConfiguration
                {
                    Id = s.idRatePlan,
                    IsEnablePortal = s.RatePortal,
                    IsEnableHotelPayment = s.hotelPayment,
                    NetRateContract = s.idContrato,
                    NameId = s.IdDiccShortDesc,
                    DescriptionId = s.IdDictionaryDescription,
                    RateCode = s.codigotarifa,
                    HasPrepaid = false
                    
                }).OrderBy(o => o.RateCode).ToList();

            for (int i = 0; i < ratePlan.Count; i++)
            {
                int dicNameId = (int) ratePlan[i].NameId;
                int dicDescriptionId = (int)ratePlan[i].DescriptionId;

                var namesList = DbContext.Diccionario
                    .Where(d => d.IdDiccionario == dicNameId)
                    .OrderBy(o => o.IdIdioma)
                    .ToList();

                var descriptionsList = DbContext.Diccionario
                    .Where(d => d.IdDiccionario == dicDescriptionId)
                    .OrderBy(o => o.IdIdioma)
                    .ToList();

                ratePlan[i].Name = (namesList.Count != 0)? namesList[0].Texto : null;
                ratePlan[i].NameEs =(namesList.Count != 0)? namesList[1].Texto : null;
                ratePlan[i].Description = (descriptionsList.Count != 0)? descriptionsList[0].Texto : null;
                ratePlan[i].DescriptionEs = (descriptionsList.Count != 0)? descriptionsList[1].Texto : null;
                
                string idRatePlan = ratePlan[i].Id;

                bool hasPrepaid = DbContext.PrepagoRatesPlan
                    .Any(p => p.IdHotel == id &&  p.RateCode == idRatePlan);

                ratePlan[i].HasPrepaid = (hasPrepaid) ? true : false;
                
                //int? hotelId = id;
               
                //if (ratePlan[i].IsEnablePortal == true)
                //{
                //    var portalsByRatePlanId = DbContextUnivisit
                //    .AplicacionPortal
                //    .Where(a => a.idHotel == hotelId && a.IdRatePlan == idRatePlan)
                //    .Select(s => new { Id = s.idPortal })
                //    .ToList();

                //    List<string> portalsNameList = new List<string>();    
                    
                //    for(int j = 0; j < portalsByRatePlanId.Count; j++)
                //    {
                //        int idPortal = (int) portalsByRatePlanId[j].Id;
                //        portalsNameList.Add(DbContextUnivisit.Portales.First(p => p.IdPortal == idPortal).Nombre);
                //    }

                //    ratePlan[i].Portals = portalsNameList;
                //}
            }
            
            string errors = null;
            errors = (lang.Equals("en-US")) ? errors.ReadResourceFile("APIServices.Errors.xml") :
                errors.ReadResourceFile("APIServices.ErrorsEs.xml");
            XmlDocument xm = new XmlDocument();
            xm.LoadXml(errors);
            List<RatePlansByHotel> response = new List<RatePlansByHotel>();

            for(int i = 0; i < ratePlan.Count; i++)
            {
                Dictionary<string, Dictionary<string, object>> status =
              new Dictionary<string, Dictionary<string, object>>();
                bool[] hotelConfig = new bool[8];
                hotelConfig[0] = (ratePlan[i].IsEnablePortal == true)? true : false;
                hotelConfig[1] = (ratePlan[i].IsEnableHotelPayment == true)? true : false;
                hotelConfig[2] = (ratePlan[i].NetRateContract == null || ratePlan[i].NetRateContract == 0)? false : true;
                hotelConfig[3] = (!string.IsNullOrEmpty(ratePlan[i].Name)) ? true : false;
                hotelConfig[4] = (!string.IsNullOrEmpty(ratePlan[i].NameEs)) ? true : false; ;
                hotelConfig[5] = (!string.IsNullOrEmpty(ratePlan[i].Description)) ? true : false; 
                hotelConfig[6] = (!string.IsNullOrEmpty(ratePlan[i].DescriptionEs)) ? true : false;
                hotelConfig[7] = (ratePlan[i].HasPrepaid == true) ? true : false;
                status.CreateResponse(xm, hotelConfig, "enableAvailability",lang);
                RatePlansByHotel ratePlanToList = new RatePlansByHotel();
                ratePlanToList.Id = ratePlan[i].Id;
                ratePlanToList.Properties = status;
                //ratePlanToList.listPortales = ratePlan[i].Portals.ToArray();
                response.Add(ratePlanToList);
            }

            return response;
        }

    }
}
