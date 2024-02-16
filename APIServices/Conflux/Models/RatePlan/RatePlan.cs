using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.RatePlan
{

    public class Transaction
    {
        public string TimeStamp { get; set; }
        public string Id { get; set; }
        public string Partner { get; set; }
        public PropertyDataSet PropertyDataSet { get; set; }
    }

    public class PropertyDataSet
    {
        public string Action { get; set; }
        public int Property { get; set; }
        public PackageData PackageData { get; set; }

    }

    public class PackageData
    {
        public string PackageID { get; set; }
        public Name Name { get; set; }
        public Description Description { get; set; }
        public Refundable Refundable { get; set; }
        public int InternetIncluded { get; set; }
        public int ParkingIncluded { get; set; }
        public PhotoUrl PhotoUrl { get; set; }
        public Meals Meals { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }

    }

    public class Name
    {
        public Text Text { get; set; }
    }

    public class Description    
    {
        public Text Text { get; set; }
    }

    public class Refundable
    {
        public bool Available { get; set; }
        public string RefundableUntilDays { get; set; }
        public string RedundableUntilTime { get; set; }
    }

    public class PhotoUrl
    {
        public Caption Caption { get; set; }
        public string URL { get; set; }
    }

    public class Caption
    {
        public Text Text { get; set; }
    }

    public class Meals
    {
        public bool IncludeBreakfast { get; set; }
        public bool IncludeDinner { get; set; }
    }

    //Common
    public class Text
    {
        public string Txt { get; set; }
        public string Language { get; set; }
    }



}
