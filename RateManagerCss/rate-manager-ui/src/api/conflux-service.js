import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const updateRates = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/updaterates/{hotelid}`);
const updateRestrictions = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/updaterestrictions/{hotelid}`);
const hotels = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/hotels{?filter,orderBy,pageSize,page}`);
const createUser = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/create/user`);

export default {
    /**
     * @param filter
     * @param orderBy
     * @param pageSize
     * @param page
    */
    GetHotels(filter, orderBy, pageSize, page){
        return hotels.get({
            filter,
            orderBy,
            pageSize,
            page
        });
    },
    CreateUser(payload){
        return createUser.save({},payload);
    },
    /**
     * @param hotelid
    */
    UpdateRates(hotelid){
        console.log(hotelid);
        return updateRates.save({hotelid},{});
    },
    /**
     * @param hotelid
    */
    UpdateRestrictions(hotelid){
        console.log(hotelid);
        return updateRestrictions.save({hotelid},{});
    },
}