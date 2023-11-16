using System;
using APIServices.Conflux.OTA.Models.Restrictions;
namespace APIServices.Conflux.Helpers.Restriction
{
    public static class RestrictionHelper
    {
        public static RestrictionStatus GetRestrictionStatus(string status)
        {
            RestrictionStatus restrictionStatus = new RestrictionStatus();

            switch (status)
            {
                case "C":
                    restrictionStatus.Status = "Close";
                    break;
                case "N":
                    restrictionStatus.Status = "Close";
                    restrictionStatus.Restriction = "Arrival";
                    break;
                case "O":
                    restrictionStatus.Status = "Open";
                    break;
            }

            return restrictionStatus;
        }
    }
}
