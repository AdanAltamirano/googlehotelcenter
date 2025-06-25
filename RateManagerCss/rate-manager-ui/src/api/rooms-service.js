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
//const roomClosureGet = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/{hotelid}/{startdate}/{enddate}/`);
//http://test.com/ratemanager/api/closure/save/1978/2020-12-29/2021-01-03
//const roomClosurePost = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/save/{hotelid}/{startdate}/{enddate}`)

const roomClosurePost = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/save/{hotelid}`);

const roomClosureGetRatePlans = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/rateplans/{hotelid}`);

const roomClosureGetRatePlansNoLinks = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/rateplansnolinks/{hotelid}`);

const roomClosureGetRooms = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/rooms/{hotelid}`);

const roomClosureGetRoomsNoLinks = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/roomsnolinks/{hotelid}`);


export default {

    /**
     * 
     * @param {Number} hotelid 
     * @param {Date} startdate 
     * @param {Date} enddate 
     * @param {Array} ratePlans 
     */
    getRoomsClosure(hotelid,startdate,enddate,ratePlans){
        
        const roomClosureGet = Vue.resource(`${process.env.VUE_APP_API_URL}/closure/{hotelid}/{startdate}/{enddate}/`,
        {},
        {},
        {params: {rateplans: ratePlans}});

        return roomClosureGet.get({
            hotelid : hotelid,
            startdate : startdate,
            enddate : enddate,
        });
    },
    /**
     * 
     * @param {Number} hotelid 
     */
    getRatePlansByHotelId(hotelid){
        return roomClosureGetRatePlans.get({
            hotelid : hotelid
        });
    },
    /**
     * 
     * @param {Number} hotelid 
     */
    getRatePlansByHotelIdNoLinks(hotelid){
        return roomClosureGetRatePlansNoLinks.get({
            hotelid: hotelid
        });
    },
    /**
     * 
     * @param {Number} hotelid 
     */
    getRoomsByHotelId(hotelid){
        return roomClosureGetRooms.get({
            hotelid : hotelid
        });
    },
    /***
     * @param {Number} hotelId
     */
    getRoomsByHotelIdNoLinks(hotelId){
        return roomClosureGetRoomsNoLinks.get({
            hotelid:hotelId
        });
    },
    /**
     * 
     * @param {Number} hotelid 
     * @param {Date} startdate 
     * @param {Date} enddate 
     * @param {Object} request 
     */
    saveRoomsClosure(hotelid,request){
        return roomClosurePost.save(
            {
                hotelid : hotelid,
            },
            request
        );
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
