using System.Collections.Generic;

namespace APIServices.Conflux.Models.Restrictions.Response
{
    public class RestrictionResponse
    {
        //Seria para Request General
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public List<Restriction> Restrictions { get; set; } = new List<Restriction>();
        public KeyValuePair<string, string> Error { get; set; }
    }
}
