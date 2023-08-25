import Vue from 'vue';

class formValidation {
    constructor(hotelId, req) {

        this.errors = [];

        //Api Model
        this.__$ = {
            Id: req.id,
            HotelId: hotelId,//Paramenter
            AccessCode:req.accessCode,
            Active: req.active,
            IsCombinablePromotion:req.isCombinablePromotion,
            StartDate: req.startDate,
            EndDate: req.endDate,
            Discount: {
                DiscountPattern: req.discount.discountPattern, //DaysFreeType
                NightsDiscounted: req.discount.nightsDiscounted,//DaysFree
                NightsRequired: null,
                Percent: req.discount.percent,//Desc Promotion
                Amount: req.discount.amount,
                ApplicationMode: req.discount.applicationMode//Discount Level
            },
            Name: {
                Eng: req.name.eng,
                Esp: req.name.esp,
                Id: req.name.id
            },
            Description: {
                Eng: req.description.eng,
                Esp: req.description.esp,
                Id: req.description.id
            },
            ApplicableFor: {
                RatesPlan: req.applicableFor.ratesPlan,
                Rooms: req.applicableFor.rooms
            },
            Rule: {
                Id: req.rule.id,
                NoArrivals: this.getDays(req.rule.noArrivals,req.rule._noArrivals),
                ApplyDays: this.getDays(req.rule.applyDays,req.rule._applyDays),
                ExcludedDates: this.getExcludeDates(req.rule.closures),
                BookingWindow: {
                    Id: req.rule.bookingWindow.id,
                    StartDate: req.rule.bookingWindow.startDate,
                    EndDate: req.rule.bookingWindow.endDate,
                    MinDays:req.rule.bookingWindow.minDays,
                    MaxDays: req.rule.bookingWindow.maxDays,
                    StartHour: req.rule.bookingWindow.startHour,
                    EndHour: req.rule.bookingWindow.endHour
                },
                // MinAdvanceBookingOffset: req.rule.minAdvanceBookingOffset,
                // MaxAdvanceBookingOffset: req.rule.maxAdvanceBookingOffset,
                // GetOfferExludedDates: [
                //     {
                //         Start: null,
                //         End: null
                //     }
                // ],
                GetOfferExludedDates:req.rule.closures,
                CancelPenalty: {
                    OffsetDroptime: req.rule.cancelPenalty.offsetDroptime,
                    OffsetTimeUnit: req.rule.cancelPenalty.offsetTimeUnit,
                    OffsetTimeUnitMiltiplier: req.rule.cancelPenalty.offsetTimeUnitMiltiplier, //this.getUnitMultipler(req),
                    SpeceficOffsetTime: null,
                    Name: req.rule.cancelPenalty.name,
                    ShortDescription: {
                        Eng: req.rule.cancelPenalty.shortDescription.eng,
                        Esp: req.rule.cancelPenalty.shortDescription.esp,
                        Id: req.rule.cancelPenalty.shortDescription.id
                    },
                    DetailedDescription: {
                        Eng: req.rule.cancelPenalty.detailedDescription.eng,
                        Esp: req.rule.cancelPenalty.detailedDescription.esp,
                        Id: req.rule.cancelPenalty.detailedDescription.id
                    },
                    MinNights:req.rule.cancelPenalty.minNights,
                    MaxNights:req.rule.cancelPenalty.maxNights,
                    ByDay: req.rule.cancelPenalty.byDay,
                    ByHour: req.rule.cancelPenalty.byHour
                },
                // MinLOS: req.rule.minNights,
                // MaxLOS: req.rule.maxNights
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

    getDays(array,_array) {
        let result = {
            sun: false, //0
            mon: false, // 1
            tue: false, // 2
            weds: false, // 3
            thur: false, // 4
            fri: false, // 5
            sat: false // 6
        };

        console.log(array);
        console.log(_array);

        //if(_array.length > 0)
        //{
            if(_array.includes(0)) array.sun = true; else array.sun = false;
            if(_array.includes(1)) array.mon = true; else array.mon = false;
            if(_array.includes(2)) array.tue = true; else array.tue = false;
            if(_array.includes(3)) array.weds = true; else array.weds = false;
            if(_array.includes(4)) array.thur = true; else array.thur = false;
            if(_array.includes(5)) array.fri = true; else array.fri = false;
            if(_array.includes(6)) array.sat = true; else array.sat = false;
        //}

        result.sun = array.sun;
        result.mon = array.mon; 
        result.tue = array.tue; 
        result.weds = array.weds; 
        result.thur = array.thur; 
        result.fri = array.fri; 
        result.sat = array.sat;

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

        if(!this.__$.Id)
        {
            this.errors.push(this.empty(this.$t('Promotion code')));
        }

        if (!this.__$.Name.Eng || !this.__$.Name.Esp) {
            this.errors.push(this.empty(this.$t('Promotion name')));
        }
            
        if (!this.__$.Description.Eng || !this.__$.Description.Esp) {
            this.errors.push(this.empty(this.$t('Promotion description')));
        }

        //tipo de la promocion
        // if ((this.__$.Discount.Amount <= 0 || !this.__$.Discount.Amount) && this.__$.Discount.NightsDiscounted <= 0) {
        //     this.errors.push(`<h6>${this.$t('Promotion type')}</h6>`);
        //     this.errors.push(this.msg('The value in free or discount night must be greater than 0'));
        // }

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