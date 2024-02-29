using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.Restrictions
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
        public RoomData RoomData { get; set; }
    }

    public class RoomData
    {
        public string RoomID { get; set; }
        public Name Name { get; set; }
        public Description Description { get; set; }
        public int Capacity { get; set; }
        public int AdultCapacity { get; set; }
        public OccupancySettings OccupancySettings { get; set; }
        public PhotoUrl PhotoUrl { get; set; }
        public RoomFeatures RoomFeatures { get; set; }

    }
    public class Name
    {
        public Text Text { get; set; }
    }

    public class Description
    {
        public Text Text { get; set; }
    }

    public class OccupancySettings
    {
        public int MinOccupancy { get; set; }
        public int MingAge { get; set; }
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

    public class RoomFeatures
    {
        public string JapaneseHotelRoomsStyle { get; set; }
        public List<Bed> Beds { get; set; }
        public string RoomSharing { get; set; }
        public string Smoking { get; set; }
        public BathAndToilet BathAndToilet { get; set; }
    }

    public class BathAndToilet
    {
        public string Relation { get; set; }
        public Bath Bath { get; set; }
        public Toilet Toliet { get; set; }

    }

    //Common
    public class Text
    {
        public string Txt { get; set; }
        public string Language { get; set; }
    }

    public class Bed
    {
        public string Size { get; set; }
    }

    public class Bath
    {
        public bool Bathtub { get; set; }
        public bool Shower { get; set; }
    }

    public class Toilet
    {
        public bool ElectronicBidet { get; set; }
    }


}
