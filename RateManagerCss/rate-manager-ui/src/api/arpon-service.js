import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const arponHotel = Vue.resource(`${process.env.VUE_APP_API_URL}/arpon/hotel/{hotelId}`);
const rateplans = Vue.resource(`${process.env.VUE_APP_API_URL}/arpon/rateplans/{hotelId}`);
const rooms = Vue.resource(`${process.env.VUE_APP_API_URL}/arpon/rooms/{hotelId}`);
const saveHotel = Vue.resource(`${process.env.VUE_APP_API_URL}/arpon/save/hotel/{hotelId}`);
const saveRatePlans = Vue.resource(`${process.env.VUE_APP_API_URL}/arpon/save/rateplans/{hotelId}`);
const saveRooms = Vue.resource(`${process.env.VUE_APP_API_URL}/arpon/save/rooms/{hotelId}`);

export default {
    /**
     * 
     * @param {*} hotelId 
     * @returns 
     */
    GetHotelArponByIdIP(hotelId){
        return arponHotel.get({hotelId});
    },
    /**
     * 
     * @param {*} hotelId 
     * @returns 
     */
    GetRatePlansByHotelId(hotelId){
        return rateplans.get({hotelId});
    },
    /**
     * 
     * @param {*} hotelId 
     * @returns 
     */
    GetRoomsByHotelId(hotelId){
        return rooms.get({hotelId});
    },
    /**
     * 
     * @param {*} hotelId 
     * @param {*} payload 
     * @returns 
     */
    SaveHotelArpon(hotelId,payload){
        return saveHotel.save({hotelId}, payload);
    },
    /**
     * 
     * @param {*} hotelId 
     * @param {*} payload 
     * @returns 
     */
    SaveRatePlansArpon(hotelId, payload){
        return saveRatePlans.save({hotelId},payload);
    },
    /**
     * 
     * @param {*} hotelId 
     * @param {*} payload 
     * @returns 
     */
    SaveRoomsArpon(hotelId,payload){
        return saveRooms.save({hotelId}, payload);
    }
}