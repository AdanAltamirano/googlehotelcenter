import Vue from 'vue'
import { sortDatesInPlace } from '../utilities/date/dates';

class modificationForm {
    constructor(checkIn, checkOut, roomsDetails, statesChangesRoomsRates) {
        this.minDate= checkIn;
        this.maxDate = checkOut;
        this.roomsDetails = roomsDetails;
        this.statesChangesRoomsRates = statesChangesRoomsRates;
        this.roomsRatesDatesFull = [];
        this.error = { hasErrors: false, errors: [] };
    }
    validate() {

        if(!this.roomsHasRates()) {
            this.error.hasErrors = true;
            this.error.errors.push(this.msg('All rooms must has rates'));

            return this.error;
        }

        if(!this.validateDates()) {
            this.error.hasErrors = true;
            this.error.errors.push(this.msg('Reservation dates and rooms rates dates does not match'));
        }

        return this.error;
    }

    getRoomsDetails() {
        this.roomsDetails.forEach((roomDetail,roomIndex) => {
            //Rates not modified
            if(!this.statesChangesRoomsRates.at(roomIndex)) {
                roomDetail.priceDetails = [];
            }
        });

        return this.roomsDetails;
    }
    roomsHasRates() {
        console.log(this.roomsDetails);
        for(let index = 0; index < this.roomsDetails.length; index++) {
            if(this.roomsDetails.at(index).priceDetails.length === 0) return false;
        }
        return true;
    }
    validateDates() {
        this.addDates();

        this.roomsRatesDatesFull.forEach(roomRateDateFull => {
            roomRateDateFull.roomsRatesDates = sortDatesInPlace(roomRateDateFull.roomsRatesDates);
        }); 

        return this.checkDates();
    }
    addDates() {

       this.roomsDetails.forEach((roomDetail, roomIndex) => {

            let roomsRatesDatesIndexed = {
                index: roomIndex,
                roomsRatesDates: []
            };

            roomDetail.priceDetails.forEach(priceDetail => {               
                roomsRatesDatesIndexed.roomsRatesDates.push({ start: priceDetail.checkIn, end: priceDetail.checkOut });
            });

            this.roomsRatesDatesFull.push(roomsRatesDatesIndexed);
        });

    }
    checkDates() {
        this.maxDate.setDate(this.maxDate.getDate() - 1);

        for(let i = 0; i < this.roomsRatesDatesFull.length; i++) {
           if(!this.validDate(this.roomsRatesDatesFull.at(i).roomsRatesDates)) return false; 
        }

        return true;
    }
    validDate(dates) {

        return (dates.at(0).start.getTime() === this.minDate.getTime() 
                && dates.at(-1).end.getTime() ===  this.maxDate.getTime());
    }
    msg(txt) {
        return `${this.$t(txt)}`;
    }
    $t(txt) {
        return Vue.i18n.translate(txt);
    }
};

export default modificationForm;