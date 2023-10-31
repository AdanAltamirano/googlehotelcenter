using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.OTA.Models.Rates
{
    public class Rate
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<BaseGuestAmount> BaseGuestAmounts { get; set; }
        public List<AdditionalGuestAmount> AdditionalGuestAmounts { get; set; }
    }
}

