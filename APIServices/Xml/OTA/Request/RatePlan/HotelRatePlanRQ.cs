using System;
using System.Linq;
using System.Xml.Linq;

namespace APIServices.Xml.OTA.Request.RatePlan
{
    public static class HotelRatePlanRQ
    {
        public static XElement CreateHotelRatePlanInsertRQ(Conflux.Models.RatePlan.Transaction transaction)
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

        public static XElement CreateHotelRoomInsertRQ(Conflux.Models.Restrictions.Transaction transaction)
        {
            XElement transactionRQ = new XElement("Transaction",
                new XAttribute("timestamp", transaction.TimeStamp));

            XElement propertyDataSet = new XElement("PropertyDataSet",
                new XAttribute("action", "delta"));

            XElement property = new XElement("Property",
                new XText(transaction.PropertyDataSet.Property.ToString()));

            XElement roomData = GetRoomData(transaction.PropertyDataSet.RoomData);

            propertyDataSet.Add(property, roomData);
            transactionRQ.Add(propertyDataSet);

            return transactionRQ;

        }


        private static XElement GetPackageData(Conflux.Models.RatePlan.PackageData packageData)
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

        private static XElement GetRoomData(Conflux.Models.Restrictions.RoomData roomData)
        {
            XElement roomDataEl = new XElement("RoomData");

            XElement roomIdEl = new XElement("RoomID",
                new XText(roomData.RoomID));

            XElement nameEl = new XElement("Name",
                new XElement("Text", new XAttribute("text", roomData.Name.Text.Txt),
                    new XAttribute("language", roomData.Name.Text.Language)
                    )
                );

            XElement descriptionEl = new XElement("Description",
                new XElement("Text", new XAttribute("text", roomData.Description.Text.Txt),
                    new XAttribute("language", roomData.Description.Text.Language)
                    )
                );

            XElement capacityEl = new XElement("Capacity",
                new XText(roomData.Capacity.ToString()));

            XElement adultCapacityEl = new XElement("AdultCapacity",
                new XText(roomData.AdultCapacity.ToString()));

            XElement occupancySettingsEl = new XElement("OccupancySettings",
                new XElement("MinOccupancy", new XText(roomData.OccupancySettings.MinOccupancy.ToString())),
                new XElement("MinAge", new XText(roomData.OccupancySettings.MingAge.ToString()))
                );

            XElement photoEl = new XElement("PhotoURL",
                new XElement("Caption",
                    new XElement("Text", new XAttribute("text", roomData.PhotoUrl.Caption.Text.Txt),
                    new XAttribute("language", roomData.PhotoUrl.Caption.Text.Language))
                ),
                new XElement("URL",new XText(roomData.PhotoUrl.URL))
            );

            XElement roomFeaturesEl = new XElement("RoomFeatures",
                new XElement("JapaneseHotelRoomStyle", new XText(roomData.RoomFeatures.JapaneseHotelRoomsStyle)),
                GetBeds(roomData),
                new XElement("Roomsharing", new XText(roomData.RoomFeatures.RoomSharing)),
                new XElement("Smoking", new XText(roomData.RoomFeatures.Smoking)),
                new XElement("BathAndToilet", new XAttribute("relation",roomData.RoomFeatures.BathAndToilet.Relation),
                    new XElement("Bath", new XAttribute("bathtub", roomData.RoomFeatures.BathAndToilet.Bath.Bathtub.ToString().ToLower()), new XAttribute("shower", roomData.RoomFeatures.BathAndToilet.Bath.Shower.ToString().ToLower())),
                    new XElement("Toilet", new XAttribute("electronic_bidet", roomData.RoomFeatures.BathAndToilet.Toliet.ElectronicBidet.ToString().ToLower()))
                )
            );

            roomDataEl.Add(roomIdEl, nameEl, descriptionEl, capacityEl, adultCapacityEl, occupancySettingsEl, photoEl, roomFeaturesEl);



            return roomDataEl;
        }

        private static XElement GetBeds(Conflux.Models.Restrictions.RoomData roomData)
        {
            XElement bedsEl = new XElement("Beds");

            foreach(var bed in roomData.RoomFeatures.Beds)
            {
                XElement _bed = new XElement("Bed", new XAttribute("size", bed.Size));

                bedsEl.Add(_bed);
            }

            return bedsEl;

        }


    }
}
