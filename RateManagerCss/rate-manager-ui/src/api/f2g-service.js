import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const f2gConfiguration = Vue.resource(`${process.env.VUE_APP_API_URL}/f2g/configuration/{id}`);
const f2gCorporates = Vue.resource(`${process.env.VUE_APP_API_URL}/f2g/corporates`)
const f2gUpdate = Vue.resource(`${process.env.VUE_APP_API_URL}/f2g/update`)
const f2gHotelsRates = Vue.resource(`${process.env.VUE_APP_API_URL}/f2g/hotelsrates/{id}`)

export default {
    /**
     * 
     * @param {Number} corporateId 
     */
    GetF2GHotelsRates(corporateId) {
        return f2gHotelsRates.get({
            id: corporateId
        })
    },
    /** 
     * @param {Number} corporateId
     */
    GetF2GConfigurationByCorporateId(corporateId) {
        return f2gConfiguration.get({
            id: corporateId
        });
    },
    /**
     * Get all corporates
     */
    GetF2GCorporates() {
        return f2gCorporates.get();
    },
    /** 
     * 
     */
    UpdateF2G(f2gSaveModel) {
        return f2gUpdate.save({}, f2gSaveModel)
    }
}