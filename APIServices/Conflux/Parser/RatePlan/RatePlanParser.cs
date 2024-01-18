using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIServices.Conflux.Models.RatePlan;


namespace APIServices.Conflux.Parser.RatePlan
{
    public static class RatePlanParser
    {
        public static Transaction ToTransaction(int propertyId, string ratePlanId, string ratePlanName, string ratePlanDescription, string language)
        {
            Transaction transaction = new Transaction()
            {
                TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
            };

            PropertyDataSet propertyDataSet = new PropertyDataSet();
            propertyDataSet.Property = propertyId;

            PackageData packageData = new PackageData();
            packageData.PackageID = ratePlanId;

            packageData.Name = new Name()
            {
                Text = new Text()
                {
                    Txt = ratePlanName,
                    Language = language
                }
            };

            packageData.Description = new Description()
            {
                Text = new Text()
                {
                    Txt = ratePlanDescription,
                    Language = language
                }
            };

            packageData.Refundable = new Refundable()
            {
                Available = true,
                RefundableUntilDays = "1",
                RedundableUntilTime = "20:00"
            };

            packageData.InternetIncluded = 1;
            packageData.ParkingIncluded = 1;

            packageData.PhotoUrl = new PhotoUrl()
            {
                Caption = new Caption()
                {
                    Text = new Text()
                    {
                        Txt = string.Empty,
                        Language = language
                    }
                },
                URL = string.Empty
            };

            packageData.Meals = new Meals()
            {
                IncludeBreakfast = false,
                IncludeDinner = false
            };

            packageData.CheckInTime = "15:00";
            packageData.CheckOutTime = "12:00";


            propertyDataSet.PackageData = packageData;
            transaction.PropertyDataSet = propertyDataSet;


            return transaction;
        } 
    }
}
