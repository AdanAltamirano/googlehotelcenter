using System;
using System.Collections.Generic;
using APIServices.Conflux.Enum;
namespace APIServices.Conflux.Models.Restrictions.Response
{
    public class Restriction
    {
        public RestrictionEnum Type { get; set; }
        public List<string> Xml { get; set; } = new List<string>();
        public bool IsSuccess { get; set; }
        public bool IsSuccessPromo { get; set; }
    }
}
