import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const rates = Vue.resource(`${process.env.VUE_APP_API_URL}/hotels/{hotelid}/rates{?startdate,enddate}`);
const ratesByDay = Vue.resource(`${process.env.VUE_APP_API_URL}/hotels/{hotelid}/rates/{rateid}/daily/{day}`);

export default {
    /**
     * @param {String} startDate fecha en formato ISO
     * @param {String} endDate fecha en formato ISO
     * @returns {Promise<[Any]>}
     */
    getByRatePlan(hotelId, startDate, endDate) {
        return rates.get({
            hotelid: hotelId,
            startdate: startDate,
            enddate: endDate,
        });
    },

    /**
     * @param {*} hotelId
     * @param {Number} rateId
     * @param {String} day fecha en formato ISO
     * @returns {Promise<[Any]>}
     */
    getByDay(hotelId, rateId, day) {
        return ratesByDay.get({
            hotelid: hotelId,
            rateid: rateId,
            day,
            customTracker: 'rates.getByDay',
        });
    },

    bulkUpdate(hotelId, request) {
        return rates.save({ hotelid: hotelId }, request);
    },

    dayUpdate(hotelId, rateId, day, request) {
        return ratesByDay.save({
            hotelid: hotelId,
            rateid: rateId,
            day,
        }, request);
    },

};
