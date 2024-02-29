using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Conflux.Models.Restrictions.Room
{
    public class RoomData
    {
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
        public int Capacity { get; set; }
        public int AdultCapcity { get; set; }
        public int MinOccupancy { get; set; } = 1;
        public int MinAge { get; set; } = 0;
        public string PhotoUrl { get; set; } = string.Empty;
        public string JapaneseHotelRoomStyle { get; set; } = "Western";
        public string BedSize { get; set; } = "double";
        public string RoomSharing { get; set; } = "Private";
        public string Smoking { get; set; } = "non_smoking";
        public string Language { get; set; } = "ES";
        public BathAndToilet BathAndToilet { get; set; } = new BathAndToilet();
    }

    public class BathAndToilet
    {
        public string Relation { get; set; } = "Together";

        public Bath Bath { get; set; } = new Bath();
        public Toilet Toliet { get; set; } = new Toilet();
    }
        public class Bath
    {
        public bool Bathtub { get; set; } = true;
        public bool Shower { get; set; } = true;
    }

    public class Toilet
    {
        public bool ElectronicBidet { get; set; } = false;
    }
}
