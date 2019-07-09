import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
Vue.http.interceptors.push(Interceptor);

const resource = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations{?filter,orderBy,pageSize,page}`);

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
        return resource.get({
            filter,
            orderBy,
            pageSize,
            page,
        });
    },
};
