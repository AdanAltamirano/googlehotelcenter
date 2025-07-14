import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const updateRates = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/updaterates/{hotelid}`);
const updateRestrictions = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/updaterestrictions/{hotelid}`);
const updateInventory = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/inventory/{hotelid}`);
const hotels = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/hotels{?filter,orderBy,pageSize,page}`);
const createUser = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/create/user`);
const usersConnectivities = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/users/connectivity{?filter,orderBy,pageSize,page}`)
const userHasPermission = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/delete/permission`);
const deleteRates = Vue.resource(`${process.env.VUE_APP_API_URL}/conflux/deleterates/{hotelid}`);

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
    /**
     * 
     * @param {*} filter 
     * @param {*} orderBy 
     * @param {*} pageSize 
     * @param {*} page 
     */
    GetUsersConnectivities(filter, orderBy, pageSize, page) {
        return usersConnectivities.get({
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
    UpdateRates(hotelid,payload){
        console.log(hotelid);
        return updateRates.save({hotelid}, payload);
    },
    /**
     * @param hotelId
     * @param payload
     */
    UpdateInventory(hotelid, payload){
        return updateInventory.save({hotelid},payload);
    },
    /**
     * @param hotelid
    */
    UpdateRestrictions(hotelid,payload){
        console.log(hotelid);
        return updateRestrictions.save({hotelid},payload);
    },
    /**
     * 
     * @param {*} hotelid 
     * @param {*} payload 
     * @returns 
     */
    DeleteRates(hotelid, payload){
        return deleteRates.save({hotelid},payload);
    }
}

export function GetUserPermission() {
    return userHasPermission.get();
}