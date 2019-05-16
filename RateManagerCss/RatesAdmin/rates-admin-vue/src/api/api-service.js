import Vue from 'vue';
import VueResource from 'vue-resource';
import Utilities from '../core/utilities'
import { EventBus } from '../core/event-bus'

Vue.use(VueResource);

const proccessing = { count: 0 };

Vue.http.interceptors.push(() => {
    if(proccessing.count == 0) EventBus.$emit('api.call.begin');
    proccessing.count += 1;
    return () => {
        proccessing.count -= 1;
        if(proccessing.count == 0) EventBus.$emit('api.call.end');
    }
});


let rates = Vue.resource(process.env.VUE_APP_API_URL + '/rates.ashx{?hotelid,startdate,enddate,language}');
let rooms = Vue.resource(process.env.VUE_APP_API_URL + '/rooms.ashx{?hotelid,language,showinactive}');


export default {
    /**
     * 
     * @param {Number} hotelId 
     * @param {String} language idioma de descripciones (en|es)
     * @param {Boolean} showInactive incluir habitaciones inactivas
     * @returns {Promise<[Any]>} 
     */
    rooms(hotelId, language, showInactive){
        return rooms.get({
            'hotelid': hotelId, 
            'language': language || 'en', 
            'showinactive': (showInactive||'false').toLowerCase() == 'true'
        })
    },

    /**
     * 
     * @param {Number} hotelId 
     * @param {String} startDate fecha en formato ISO
     * @param {String} endDate fecha en formato ISO
     * @param {String} language idioma de descripciones (en|es)
     * @returns {Promise<[Any]>} 
     */
    rates(hotelId, startDate, endDate, language){
        return rates.get({
            'hotelid': hotelId,
            'startdate': startDate,
            'enddate': endDate,
            'language': language || 'en'
        });
    },

    /**
     * 
     * @param {Number} hotelId 
     * @param {String} startDate fecha en formato ISO
     * @param {String} endDate fecha en formato ISO
     * @param {String} language idioma de descripciones (en|es)
     */
    roomsWithRates(hotelId, startDate, endDate, language){
        let rooms_req = this.rooms(hotelId, language);
        let rates_req = this.rates(hotelId, startDate, endDate, language);

        return new Promise((resolve, reject) => {
            Promise.all([rooms_req, rates_req])
            .then(([rooms_res, rates_res]) => {
                resolve({rooms: rooms_res.body, rates: rates_res.body, mixin: Utilities.mixRoomsAndRates(rooms_res.body, rates_res.body)});
            })
            .catch((reason)=> {
                reject(reason);
            });
        });
    }
    
}