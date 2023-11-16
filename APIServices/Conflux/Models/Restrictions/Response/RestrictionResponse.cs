using System.Collections.Generic;

namespace APIServices.Conflux.Models.Restrictions.Response
{
    public class RestrictionResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public KeyValuePair<string, string> Error { get; set; }
    }
}
