import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
Vue.http.interceptors.push(Interceptor);

const resource = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{hotelId}{?filter,page,pageSize}`);

export default
{
    /**
     * 
     * @param {*} hotelId
     * @return {Promise<[Any]>}
     */
    GetAll(filter, page, pageSize)
    {
        return resource.get({
            filter,
            page,
            pageSize,
        });
    }
}