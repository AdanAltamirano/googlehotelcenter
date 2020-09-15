import Vue from 'vue';

class formValidation {
    constructor(hotelId, req) {

        this.errors = [];

        this.__$ = {
            Id: req.code,
            HotelId: hotelId,
            Active: null,
            StartDate: req.travelWindow.initialDate,
            EndDate: req.travelWindow.finalDate,
            Discount: {
                DiscountPattern: req.typePromotion.typeFreeNight,
                NightsDiscounted: req.typePromotion.freeNight,
                NightsRequired: null,
                Percent: null,
                Amount: req.typePromotion.discount,
                ApplicationMode: req.typePromotion.typeDiscount
            },
            Name: {
                Eng: req.main.nameEn,
                Esp: req.main.nameEs,
                Id: null
            },
            Description: {
                Eng: req.main.descEn,
                Esp: req.main.descEs,
                Id: null
            },
            ApplicableFor: {
                RatesPlan: req.roomsAndRateplans.rooms,
                Rooms: req.roomsAndRateplans.rateplans
            },
            Rule: {
                Id: null,
                NoArrivals: this.getDays(req.travelWindow.noArrivalDays),
                ApplyDays: this.getDays(req.travelWindow.validDays),
                ExcludedDates: this.getExcludeDates(req.travelWindow.closures),
                BookingWindow: {
                    Id: null,
                    StartDate: req.bookingWindow.startDate,
                    EndDate: req.bookingWindow.endDate,
                    StartHour: req.bookingWindow.timeFrom,
                    EndHour: req.bookingWindow.timeTo
                },
                MinAdvanceBookingOffset: req.bookingWindow.minDays,
                MaxAdvanceBookingOffset: req.bookingWindow.maxDays,
                GetOfferExludedDates: [
                    {
                        Start: null,
                        End: null
                    }
                ],
                CancelPenalty: {
                    OffsetDroptime: null,
                    OffsetTimeUnit: req.restriction.cancellationType,
                    OffsetTimeUnitMiltiplier: this.getUnitMultipler(req),
                    SpeceficOffsetTime: null,
                    Name: null,
                    ShortDescription: {
                        Eng: req.restriction.prevCancel_en,
                        Esp: req.restriction.prevCancel_es,
                        Id: null
                    },
                    DetailedDescription: {
                        Eng: req.restriction.detsCancel_en,
                        Esp: req.restriction.detsCancel_es,
                        Id: null
                    }
                },
                MinLOS: req.restriction.minNights,
                MaxLOS: req.restriction.maxNights
            }
        };
    }

    getUnitMultipler(req) {
        let result = '';
        switch(req.restriction.cancellationType) {
            case 0:
                result = req.restriction.byDay;
                break;
            case 1:
                result = req.restriction.byHour;
                break;
            case 2:
                result = req.restriction.bySpecificTime.hour;
                break;
        }
        return result;
    }

    getDays(array) {
        let result = {
            Sun: false,
            Mon: false,
            Tue: false,
            Weds: false,
            Thur: false,
            Fri: false,
            Sat: false
        };

        Object.keys(result).forEach((key, index) => {
            result[key] = array.includes(index);
        });
        return result;
    }

    getExcludeDates(array) {
        return array.map(x => {
            return {
                Start: x.start,
                End: x.end
            }
        });
    }

    validate() {
        if (!this.__$.Name.Eng || !this.__$.Name.Esp) {
            this.errors.push(this.empty(this.$t('Promotion name')));
        }
            
        if (!this.__$.Description.Eng || !this.__$.Description.Esp) {
            this.errors.push(this.empty(this.$t('Promotion description')));
        }

        //tipo de la promocion
        if ((this.__$.Discount.Amount <= 0 || !this.__$.Discount.Amount) && this.__$.Discount.NightsDiscounted <= 0) {
            this.errors.push(`<h6>${this.$t('Promotion type')}</h6>`);
            this.errors.push(this.msg('The value in free or discount night must be greater than 0'));
        }

        //habitaciones y planes tarifarios
        if (this.__$.ApplicableFor.RatesPlan.length === 0 || this.__$.ApplicableFor.Rooms.length === 0) {
            this.errors.push(`<h6>${this.$t('Rate plans and rooms')}</h6>`);
            if (this.__$.ApplicableFor.RatesPlan.length === 0) {
                this.errors.push(this.msg('You must select rate plan(s)'));
            }

            if (this.__$.ApplicableFor.Rooms.length === 0) {
                this.errors.push(this.msg('You must select room(s)'));
            }
        }

        //restricciones
        if (this.__$.Rule.CancelPenalty.OffsetTimeUnit === -1 || !this.__$.Rule.CancelPenalty.ShortDescription.Eng || !this.__$.Rule.CancelPenalty.ShortDescription.Esp || !this.__$.Rule.CancelPenalty.DetailedDescription.Eng || !this.__$.Rule.CancelPenalty.DetailedDescription.Esp) {
            this.errors.push(`<h6>${this.$t('Restrictions')}</h6>`);

            if (this.__$.Rule.CancelPenalty.OffsetTimeUnit === -1) {
                this.errors.push(this.msg('You must select cancellation policies'))
            }

            if (!this.__$.Rule.CancelPenalty.ShortDescription.Eng || !this.__$.Rule.CancelPenalty.ShortDescription.Esp) {
                this.errors.push(this.empty('Prior cancellation policy'));
            }

            if (!this.__$.Rule.CancelPenalty.DetailedDescription.Eng || !this.__$.Rule.CancelPenalty.DetailedDescription.Esp) {
                this.errors.push(this.empty('Detailed cancellation policy'));
            }
        }

        //travel window
        if (this.__$.StartDate === null || this.__$.EndDate === null) {
            this.errors.push(`<h6>Travel Window</h6>`);
            if (this.__$.StartDate === null) {
                this.errors.push(this.msg('You must select start date of trip'));
            }

            if (this.__$.EndDate === null) {
                this.errors.push(this.msg('You must select end date of the trip'));
            }
        }
    }

    empty(txt) {
        return `<span>${this.$t(txt)} ${this.$t('must not be empty')}</span>`
    }
    msg(txt) {
        return `<span>${this.$t(txt)}</span>`;
    }
    $t(txt) {
        return Vue.i18n.translate(txt);
    }
};

export default formValidation;