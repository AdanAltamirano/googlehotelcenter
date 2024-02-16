using System;
using System.Linq;
using System.Xml.Linq;
using APIServices.Conflux.Models.RatePlan;

namespace APIServices.Xml.OTA.Request.RatePlan
{
    public static class HotelRatePlanRQ
    {
        public static XElement CreateHotelRatePlanInsertRQ(Transaction transaction)
        {
            XElement transactionRQ = new XElement("Transaction",
                new XAttribute("timestamp", transaction.TimeStamp));

            XElement propertyDataSet = new XElement("PropertyDataSet",
                new XAttribute("action", "delta"));

            XElement property = new XElement("Property",
                new XText(transaction.PropertyDataSet.Property.ToString()));

            XElement packageData = GetPackageData(transaction.PropertyDataSet.PackageData);

            propertyDataSet.Add(property,packageData);
            transactionRQ.Add(propertyDataSet);

            return transactionRQ;

        }

        private static XElement GetPackageData(PackageData packageData)
        {
            XElement packageDataEl= new XElement("PackageData");

            XElement packageIdEl = new XElement("PackageID",
                new XText(packageData.PackageID));

            XElement nameEl = new XElement("Name",
                new XElement("Text", new XAttribute("text",packageData.Name.Text.Txt),
                    new XAttribute("language",packageData.Name.Text.Language)
                    )
                );

            XElement descriptionEl = new XElement("Description",
                new XElement("Text", new XAttribute("text", packageData.Description.Text.Txt),
                    new XAttribute("language", packageData.Description.Text.Language)
                    )
                );

            XElement refundableEl = new XElement("Refundable",
                new XAttribute("available",packageData.Refundable.Available.ToString().ToLower()),
                new XAttribute("refundable_until_days", packageData.Refundable.RefundableUntilDays),
                new XAttribute("refundable_until_time", packageData.Refundable.RedundableUntilTime));

            XElement internetEl = new XElement("InternetIncluded",
                new XText(packageData.InternetIncluded.ToString()));

            XElement parkingEl = new XElement("ParkingIncluded",
                new XText(packageData.ParkingIncluded.ToString()));

            XElement photoEl = new XElement("PhotoURL",
                new XElement("Caption", 
                    new XElement("Text", new XAttribute("text",packageData.PhotoUrl.Caption.Text.Txt),
                    new XAttribute("language",packageData.PhotoUrl.Caption.Text.Language))                    
                ),
                new XElement("URL")
            );

            XElement mealsEl = new XElement("Meals",
                new XElement("Breakfast", new XAttribute("included", packageData.Meals.IncludeBreakfast.ToString().ToLower())),
                new XElement("Dinner", new XAttribute("included", packageData.Meals.IncludeDinner.ToString().ToLower()))
            );

            XElement checkInEl = new XElement("CheckinTime",
                new XText(packageData.CheckInTime));

            XElement checkOutEl = new XElement("CheckoutTime",
                new XText(packageData.CheckOutTime));

            packageDataEl.Add(packageIdEl,nameEl, descriptionEl, refundableEl, internetEl, parkingEl, photoEl, mealsEl, checkInEl, checkOutEl);

            return packageDataEl;


        }

    }
}
