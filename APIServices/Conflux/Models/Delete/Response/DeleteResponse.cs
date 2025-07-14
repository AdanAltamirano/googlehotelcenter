using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.Delete.Response
{
    public class DeleteResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string Xml { get; set; }
        public string XmlRequest { get; set; }

        public List<DeleteHttpResponse> DeleteHttpResponseList = new List<DeleteHttpResponse>();
        public KeyValuePair<string, string> Error { get; set; } = new KeyValuePair<string, string>();
    }

    public class DeleteHttpResponse
    {
        public string Xml { get; set; }
        public string XmlRequest { get; set; }
        public bool IsSuccess { get; set; }
    }

}
