using System.Collections.Generic;

namespace APIServices.Conflux.Models.Restrictions.Response
{
    //Esta Clase se usa para Cierres
    public class RestrictionResponse
    {
        //Seria para Request General
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public List<Restriction> Restrictions { get; set; } = new List<Restriction>();
        public KeyValuePair<string, string> Error { get; set; }
    }

    //Esta clase se usa para restricciones reglas
    public class RestrictionResponseV2
    {
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public List<RestrictionV2> Restrictions { get; set; } = new List<RestrictionV2>();
        public KeyValuePair<string, string> Error { get; set; }

    }

}
