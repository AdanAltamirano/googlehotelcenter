using System.Collections.Generic;

namespace APIServices.Conflux.Models.Rates.Response
{
    public class RateResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public string RequestXML { get; set; } = string.Empty;
        public KeyValuePair<string, string> Error { get; set; }
    }
}
