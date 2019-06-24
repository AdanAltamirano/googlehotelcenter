import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
Vue.http.interceptors.push(Interceptor);

const resource = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{hotelId}{?filter}`);

export default
{
    /**
     * 
     * @param {*} hotelId
     * @return {Promise<[Any]>}
     */
    GetById(hotelId, filter)
    {
        return resource.get({
            hotelId,
            filter
        });
    }
}