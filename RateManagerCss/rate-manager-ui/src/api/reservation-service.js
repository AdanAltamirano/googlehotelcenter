import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
Vue.http.interceptors.push(Interceptor);

const reservationList = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations{?filter,orderBy,pageSize,page}`);
const reservationDetails = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/details/{reservationId}`);

export default
{
    /**
     *
     * @param {*} filter
     * @param {*} orderBy
     * @param {*} pageSize
     * @param {*} page
     */
    GetAll(filter, orderBy, pageSize, page) {
        return reservationList.get({
            filter,
            orderBy,
            pageSize,
            page,
        });
    },

    GetDetails(reservationId) {
        return reservationDetails.get({
            reservationId
        });
    }
};
