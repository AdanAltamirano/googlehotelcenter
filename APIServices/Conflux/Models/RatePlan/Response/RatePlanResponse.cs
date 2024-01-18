namespace APIServices.Conflux.Models.RatePlan.Response
{
    public class RatePlanResponse
    {
        public int StatusCode { get; set; }
        public string Response { get; set; }
        public string RequestXML { get; set; } = string.Empty;
    }
}
