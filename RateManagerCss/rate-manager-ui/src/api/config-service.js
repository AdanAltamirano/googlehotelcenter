import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const resource = Vue.resource(`${process.env.VUE_APP_API_URL}/config/{id}`);
const rates = Vue.resource(`${process.env.VUE_APP_API_URL}/config/rates/{id}`);
const corporates = Vue.resource(`${process.env.VUE_APP_API_URL}/portal/coorp`)
const createCorporate = Vue.resource(`${process.env.VUE_APP_API_URL}/portal/coorp`);
const createPortals = Vue.resource(`${process.env.VUE_APP_API_URL}/portal/portals`);
const currencies = Vue.resource(`${process.env.VUE_APP_API_URL}/currencies`);

export default {
    /**
     * 
     * @returns  A list of currencies
     */
    GetCurrencies() {
        return currencies.get();
    },
    /**
     *
     * @param {Number} hotelId
     */
    GetErrors(hotelId) {
        return resource.get({
            id: hotelId,
        });
    },
    /**
     *
     * @param {Number} hotelId
     */
    GetRates(hotelId) {
        return rates.get({
            id: hotelId,
        })
    },
    /**
     * Get All Corporates
     */
    GetCorporates() {
        return corporates.get()
    },
    /**
     * @param {Object} corporate  
     */
    CreateCorporate(corporate) {
        return createCorporate.save({}, corporate)
    },
    /** 
     * @param {Object} portals
     */
    CreatePortals(portals) {
        return createPortals.save({}, portals)
    }
}