class formValidation {
    constructor(req) {

        this.errors = [];

        this.__$ = {
            Id,
            HotelId,
            Active,
            StartDate,
            EndDate,
            Discount: {
                DiscountPattern: req.typePromotion.typeFreeNight,
                NightsDiscounted: req.typePromotion.freeNight,
                NightsRequired,
                Percent,
                Amount: req.typePromotion.discount,
                ApplicationMode: req.typePromotion.typeDiscount
            },
            Name: {
                Eng: req.main.nameEn,
                Esp: req.main.nameEs,
                Id
            },
            Description: {
                Eng: req.main.descEn,
                Esp: req.main.descEs,
                Id
            },
            ApplicableFor: {
                RatesPlan: req.roomsAndRateplans.rooms,
                Rooms: req.roomsAndRateplans.rateplans
            },
            Rule: {
                Id,
                NoArrivals: {
                    Sun,
                    Mon,
                    Tue,
                    Thur,
                    Weds,
                    Fri,
                    Sat
                },
                ApplyDays: {
                    Sun,
                    Mon,
                    Tue,
                    Thur,
                    Weds,
                    Fri,
                    Sat
                },
                ExludedDates: [
                    {
                        Start,
                        End
                    }
                ],
                BookingWindow: {
                    Id,
                    StartDate,
                    EndDate,
                    StartHour,
                    EndHour
                },
                MinAdvanceBookingOffset,
                MaxAdvanceBookingOffset,
                GetOfferExludedDates: [
                    {
                        Start,
                        End
                    }
                ],
                CancelPenalty: {
                    OffsetDroptime,
                    OffsetTimeUnit,
                    OffsetTimeUnitMiltiplier,
                    SpeceficOffsetTime,
                    Name,
                    ShortDescription: {
                        Eng,
                        Esp,
                        Id
                    },
                    DetailedDescription: {
                        Eng,
                        Esp,
                        Id
                    }
                },
                MinLOS,
                MaxLOS
            }
        };
    }

    validate() {

    }
};

export default formValidation;