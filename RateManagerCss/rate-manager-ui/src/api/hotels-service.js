import Vue from 'vue';
import VueResource from 'vue-resource';
import Interceptor from './interceptor';

Vue.use(VueResource);
if(Vue.http.interceptors.indexOf(Interceptor) == -1){
    Vue.http.interceptors.push(Interceptor);
}

const resource = Vue.resource(`${process.env.VUE_APP_API_URL}/hotels{/hotelid}{?filter,orderBy,page,pageSize}`);

export default {
    /**
     * @param {String} filter
     * @param {String} orderBy
     * @param {Number} pageSize
     * @param {Number} page
     * @returns {Promise<[Any]>}
     */
    getList(filter, orderBy, pageSize, page) {
        return resource.get({
            filter,
            orderBy,
            page,
            pageSize,
        });
    },

    /**
     *
     * @param {Number} hotelId
     */
    get(hotelId) {
        return resource.get({
            hotelid: hotelId,
        });
    },

};
