import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const offers = Vue.resource(`${process.env.VUE_APP_API_URL}/hotels/{hotelId}/offers/{code}`);

export default {
    getByCode(hotelId, code) {
        return offers.get({
            hotelId,
            code
        });
    },
}