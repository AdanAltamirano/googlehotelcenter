import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const rooms = Vue.resource(`${process.env.VUE_APP_API_URL}/hotels/{hotelid}/rooms{?filter,orderBy,page,pageSize}`);
const inventory = Vue.resource(
    `${process.env.VUE_APP_API_URL}/hotels/{hotelid}/rooms/{roomid}/inventory{?startdate,enddate}`,
);

//http://test.com/ratemanager/api/closure/1978/2020-12-29/2021-01-03/EPB
const roomClosureGet = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/{hotelid}/{startdate}/{enddate}/{rateplan}`)

export default {

    /**
     * 
     * @param {Number} hotelid 
     * @param {Date} startdate 
     * @param {Date} enddate 
     * @param {String} rateplan 
     */
    getRoomsClosure(hotelid,startdate,enddate,rateplan){
        return roomClosureGet.get({
            hotelid : hotelid,
            startdate : startdate,
            enddate : enddate,
            rateplan : rateplan
        });
    },

    /**
     * @param {Number} hotelId
     * @param {String} filter
     * @param {String} orderBy
     * @param {Number} pageSize
     * @param {Number} page
     * @returns { Promise<[Any]> }
     */
    getList(hotelId, filter, orderBy, pageSize, page) {
        return rooms.get({
            hotelid: hotelId,
            filter,
            orderBy,
            page,
            pageSize,
        });
    },

    /**
     * @param {Number} hotelId
     * @param {Number} roomId
     * @param {String} startDate fecha en formato ISO
     * @param {String} endDate fecha en formato ISO
     * @returns {Promise<[Any]>}
     */
    getInventory(hotelId, roomId, startDate, endDate) {
        return inventory.get({
            hotelid: hotelId,
            roomid: roomId,
            startdate: startDate,
            enddate: endDate,
        });
    },

};
