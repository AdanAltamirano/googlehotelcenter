using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace APIServices.Xml.Soap
{
    public static class Soap
    {
		private static XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
		private static XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		private static XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
		private static XNamespace tempuri = "http://tempuri.org/";

		private static string usernameConflux = ConfigurationManager.AppSettings["usernameConflux"];
		private static string passwordConflux = ConfigurationManager.AppSettings["usernamePasswordConflux"];

		public static XDocument CreateSoapRequestXml(XElement request)
        {
			XDocument document = new XDocument();

			XElement envelope = new XElement(soap + "Envelope",
				new XAttribute(XNamespace.Xmlns + "soap", soap),
				new XAttribute(XNamespace.Xmlns + "xsi", xsi),
				new XAttribute(XNamespace.Xmlns + "xsd", xsd));

			XElement header = new XElement(soap + "Header");
			
			var headerContent = GetHeaderContent();

			header.Add(headerContent);

			XElement body = new XElement(soap + "Body");
			body.Add(request);

			envelope.Add(header, body);

			document.Add(envelope);

			return document;

		}

		private static XElement GetHeaderContent()
        {		
			XElement security = new XElement(tempuri + "Security");
			
			XElement userNameToken = new XElement("UsernameToken");
			
			XElement userName = new XElement("Username",
				new XText(usernameConflux));
		
			XElement password = new XElement("Password",
				new XText(passwordConflux));

			userNameToken.Add(userName,password);

			security.Add(userNameToken);


			return security;
        }
    }
}
