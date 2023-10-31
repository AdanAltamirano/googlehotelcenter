using System.Collections.Generic;

namespace APIServices.Conflux.Models.Rates.Response
{
    public class RateResponse
    {
        public bool IsSuccess { get; set; } = false;
        public KeyValuePair<string, string> Error { get; set; }
    }
}
