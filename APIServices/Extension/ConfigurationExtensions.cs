using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System;

namespace APIServices.Extension
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Create a capitalize word 
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>Capitalize string</returns>
        public static string Capitalize(this string variable)
        {
            variable = variable.TrimEnd();
            variable = Regex.Replace(variable, @"\s+", " ");
            string[] array = variable.Split(' ');
           
            for (int i = 0; i < array.Length; i++)
            {
                int longword = array[i].Length - 1;
                array[i] = array[i].Substring(0, 1).ToUpper() + array[i].Substring(1, longword);
            }

           string capitalize =  array.Aggregate(new StringBuilder(),
                (current,next) => current.Append(" ")
                .Append(next)).
                ToString().
                TrimEnd();

            return capitalize;
        }

        /// <summary>
        /// Replace word Hotel with H for Application Name Portal
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>String that replace Hotel with H for application name</returns>
        public static string ReplaceHotel(this string variable)
        {
            string[] array = variable.Split(' ');
            array[0] = array[0].Contains("Hotel") ? array[0] = "H" : array[0];
            string response = array.Aggregate(new StringBuilder(), 
                (current, next) => current.Append(" ")
                .Append(next))
                .ToString();

            return response;
        }

        /// <summary>
        /// Create string without spaces
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>String without spaces</returns>
        public static string RemoveAllWhiteSpaces(this string variable)
        {
            return variable = Regex.Replace(variable, @"\s+", "");
        }

        /// <summary>
        /// Read contents of an embedded resource file
        /// </summary>
        public static string ReadResourceFile(this string variable, string filename)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            using (var stream = thisAssembly.GetManifestResourceStream(filename))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Create dictionary for hotel config
        /// </summary>
        /// <param name="responseDictionary"></param>
        /// <param name="document"></param>
        /// <param name="passedConfiguration"></param>
        /// <param name="type"></param>
        /// <returns>Status, Status and Message if its necessary for hotel config</returns>
        public static Dictionary<string, Dictionary<string, object>> CreateResponse(this Dictionary<string, Dictionary<string, object>> responseDictionary,
            XmlDocument document, bool[] passedConfiguration, string type = "", string lang = "")
        {
            switch (type)
            {
                case "dates":
                    XmlNodeList dates = document.GetElementsByTagName("dates");
                    string node = null;
                    string jsonName = null;
                    string availability = (lang.Equals("en-US")) ? "Availability" : "Disponibilidad";
                    string availabilityPortal = (lang.Equals("en-US")) ? "Portal Availability" : "Disponibilidad en Portal";
                    string bankDeposit = (lang.Equals("en-US")) ? "Allow Bank Deposit" : "Permitir Depósito Bancario";
                    string propertyNumber = (lang.Equals("en-US")) ? "Property Number" : "Número de Propiedad";
                    for (int i = 0; i < passedConfiguration.Length; i++)
                    {
                        node = (i == 0) ? "availability" 
                            : (i == 1) ? "availability_portal" 
                            : (i == 2) ? "allow_bank_deposit" 
                            : "property_number";

                        jsonName = (i == 0) ? availability
                          : (i == 1) ? availabilityPortal
                          : (i == 2) ? bankDeposit
                          : propertyNumber;

                        if (passedConfiguration[i])
                       {
                            string success = dates.Item(0)
                                .SelectSingleNode(node)
                                .SelectSingleNode("success").InnerText;

                            string complete = dates.Item(0)
                             .SelectSingleNode(node)
                             .SelectSingleNode("complete").InnerText;

                            responseDictionary.Add(jsonName,CreateStatusAndMessage("Status",success,"Message",complete));
                       }
                       else
                       {
                            string fail = dates.Item(0)
                               .SelectSingleNode(node)
                               .SelectSingleNode((i != 2) ? "fail" : "warning").InnerText;

                            string error = dates.Item(0)
                             .SelectSingleNode(node)
                             .SelectSingleNode("error").InnerText;

                            responseDictionary.Add(jsonName, CreateStatusAndMessage("Status", fail, "Message", error));
                        }
                    }
                break;

                case "creditcard":
                    string message = (lang.Equals("en-US")) ? ". Visa, Master Card International, American Express have been saved by default"  
                        : ". Las tarjetas Visa, Master Card International, American express han sido guardadas por defecto";
                      XmlNodeList creditCart = document.GetElementsByTagName("creditcard");

                    string creditCard = lang.Equals("en-US") ? "Credit Cards" : "Tarjetas de Crédito";    

                      if(passedConfiguration[0])
                      {
                        string success = creditCart.Item(0).SelectSingleNode("success").InnerText;
                        string complete = creditCart.Item(0).SelectSingleNode("complete").InnerText;
                        responseDictionary.Add(creditCard,CreateStatusAndMessage("Status",success,"Message",complete));
                      }
                      else
                      {
                        string fail = creditCart.Item(0).SelectSingleNode("fail").InnerText;
                        string error = creditCart.Item(0).SelectSingleNode("error").InnerText;
                        error = (passedConfiguration[1]) ? error + message : error;
                        responseDictionary.Add(creditCard, CreateStatusAndMessage("Status", fail, "Message", error));
                      }
                    
                break;

                case "enableAvailability":
                    XmlNodeList enableAvailability = document.GetElementsByTagName("rateplan");
                    string nodeEnable = null;
                    string jsonNameEnable = null;
                    string portalAvailability = lang.Equals("en-US") ? "Portal Availability" : "Disponible en Portal";
                    string hotelPayment = lang.Equals("en-US") ? "Hotel Payment" : "Pago en el Hotel";
                    string netRateContract = lang.Equals("en-Us") ? "Net Rate Contract" : "Contrato Tarifa Neta";
                    string name = lang.Equals("en-US") ? "English Name" : "Nombre en Inglés";
                    string nameEs = lang.Equals("en-US") ? "Spanish Name" : "Nombre en Español";
                    string description = lang.Equals("en-US") ? "English Description" : "Descripción en Inglés";
                    string descriptionEs = lang.Equals("en-US") ? "Spanish Description" : "Descripcíón en Español";
                    string prepaidRatePlan = lang.Equals("en-US") ? "Prepaid Rate Plan" : "Prepago Plan Tarifario";

                    for (int i = 0; i < passedConfiguration.Length; i++)
                    {
                        nodeEnable = (i == 0) ? "portal_availability"
                           : (i == 1) ? "hotelpayment"
                           : (i == 2) ? "net_rate_contract"
                           : (i == 3) ? "name"
                           : (i == 4) ? "name_es"
                           : (i == 5) ? "description"
                           : (i == 6 ) ?"description_es"
                           :"prepaid_rate_plan";

                        jsonNameEnable = (i == 0) ? portalAvailability
                         : (i == 1) ? hotelPayment
                         : (i == 2) ? netRateContract
                         : (i == 3) ? name
                         : (i == 4) ? nameEs
                         : (i == 5) ? description
                         : (i == 6)? descriptionEs
                         : prepaidRatePlan;

                        if (passedConfiguration[i])
                        {
                            string success = enableAvailability.Item(0)
                                  .SelectSingleNode(nodeEnable)
                                  .SelectSingleNode("success").InnerText;

                            string complete = enableAvailability.Item(0)
                                  .SelectSingleNode(nodeEnable)
                                  .SelectSingleNode("complete").InnerText;

                            responseDictionary.Add(jsonNameEnable, CreateStatusAndMessage("Status", success,"Message",complete));
                        }
                        else
                        {
                            string fail = enableAvailability.Item(0)
                             .SelectSingleNode(nodeEnable)
                             .SelectSingleNode("fail").InnerText;

                            string error = enableAvailability.Item(0)
                            .SelectSingleNode(nodeEnable)
                            .SelectSingleNode("error").InnerText;

                            responseDictionary.Add(jsonNameEnable, CreateStatusAndMessage("Status", fail, "Message", error));
                        }       
                    }

                    break;
            }
            return responseDictionary;
        }

        /// <summary>
        /// Create Dictionary for status and message
        /// </summary>
        /// <param name="subjects"></param>
        /// <returns>Return status and message response for attributes</returns>
        private static  Dictionary<string, object> CreateStatusAndMessage(params string[] subjects)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            int addDictionary = 0;
            string key = null;
            for (int i = 0; i < subjects.Length; i++)
            {
                switch (addDictionary)
                {
                    case 0:
                        key = subjects[i];
                        addDictionary++;
                        break;
                    case 1:

                        if(key.Equals("Status"))
                        {
                            int code = Int32.Parse(subjects[i]);
                            response.Add(key,code);
                        }
                        else
                        {
                            response.Add(key,subjects[i]);
                        }

                        addDictionary = 0;
                        break;
                }
            }
            return response;
        }





    }
}
