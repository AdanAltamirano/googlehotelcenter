import Vue from 'vue'
import moment from 'moment';

export default {
    /**
     * @returns {moment} moment date object
     */
    getLastWorkDay() {
        const today = moment();
        if (localStorage) {
            const lastWorkDay = localStorage.getItem('admin.' + Vue.appConfig.session.hotelId + '.rates.workingDate');
            if (lastWorkDay) {
                const start = moment(lastWorkDay);
                if (start.isValid() && start.isAfter(today)) {
                    return start;
                }
            }
            this.setLastWorkDay(today);
        }
        return today;
    },
    /**
     * @param {moment} day
     * Save last selected day to localstorage
     */
    setLastWorkDay(day) {
        if (localStorage) {
            localStorage.setItem('admin.' +  Vue.appConfig.session.hotelId + '.rates.workingDate', day.format('YYYY-MM-DD'));
        }
    },
    /**
     *
     * @param {[Any]} array rateplan array
     */
    makeRatesTree(array) {
        const map = {}; let node; const roots = []; let
            i;

        for (i = 0; i < array.length; i += 1) {
            map[array[i].ratePlanId + array[i].roomId] = i;
            array[i].children = [];
        }

        for (i = 0; i < array.length; i += 1) {
            node = array[i];
            if (node.parentRatePlanId) {
                array[map[node.parentRatePlanId + node.roomId]].children.push(node);
            } else {
                roots.push(node);
            }
        }
        return roots;
    },

    /**
     *
     * @param {[Any]} rooms rooms array
     * @param {[Any]} rates rateplan array
     */
    mixRoomsAndRates(rooms, rates) {
        const ratesTree = this.makeRatesTree(rates);
        return rooms.map((room) => {
            room.rates = ratesTree.filter(plan => plan.roomId === room.id);
            return room;
        }).filter(room => room.rates && room.rates.length > 0);
    },
};
