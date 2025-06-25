using System.Collections.Generic;

namespace APIServices.Conflux.Models.Inventory.Response
{
    public class InventoryResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public string XmlRequest { get; set; }

        public List<InventoryHttpResponse> InventoryHttpResponseList = new List<InventoryHttpResponse>();
        public KeyValuePair<string, string> Error { get; set; } = new KeyValuePair<string, string>();
    }

    public class InventoryHttpResponse
    {
        public string Xml { get; set; }
        public string XmlRequest { get; set; }
        public bool IsSuccess { get; set; }
    }

}
