import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if (Vue.http.interceptors.indexOf(Interceptor) === -1) {
    Vue.http.interceptors.push(Interceptor);
}

const rateplan = Vue.resource(`${process.env.VUE_APP_API_URL}/hotels/{hotelid}/ratesplan{?filter,orderBy,page,pageSize}`);

export default {
    getList(hotelId, filter, orderBy, pageSize, page) {
        return rateplan.get({
            hotelid: hotelId,
            filter,
            orderBy,
            page,
            pageSize,
        });
    },
}