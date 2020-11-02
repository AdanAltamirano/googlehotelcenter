import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const offers = Vue.resource(`${process.env.VUE_APP_API_URL}/promotions/{HotelId}/code/{code}`);
const offersList = Vue.resource(`${process.env.VUE_APP_API_URL}/promotions{?filter,orderBy,pageSize,page}`)
const enable = Vue.resource(`${process.env.VUE_APP_API_URL}/promotions/enable/{hotelId}/code/{code}`)
const disable = Vue.resource(`${process.env.VUE_APP_API_URL}/promotions/disable/{hotelId}/code/{code}`)
const update = Vue.resource(`${process.env.VUE_APP_API_URL}/promotions/{hotelId}/update/code/{code}`)
const newPromo =  Vue.resource(`${process.env.VUE_APP_API_URL}/promotions/{hotelId}/save`)

export default {
    getByCode(HotelId, code) {
        return offers.get({
            HotelId,
            code
        });
    },
    getByHotelId(filter, orderBy, pageSize, page){
        return offersList.get({
            filter,
            orderBy,
            pageSize,
            page
        })
    },
    enablePromotion(hotelId,code){
        return enable.save({
            hotelId:hotelId,
            code:code
        },{})
    },
    disablePromotion(hotelId,code){
        return disable.save({
            hotelId:hotelId,
            code:code
        },{})
    },
    updatePromotion(hotelId,code,data){
        return update.save({
            hotelId:hotelId,
            code:code
        },data);
    },
    savePromotion(hotelId,data){
        return newPromo.save({hotelId:hotelId},data);
    }
    
}