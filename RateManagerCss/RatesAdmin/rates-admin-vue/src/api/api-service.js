import Vue from 'vue';
import VueResource from 'vue-resource';

Vue.use(VueResource);

let rates = Vue.resource(process.env.VUE_APP_API_URL + '/rates.ashx{?hotelid,startdate,enddate,language}')
let rooms = Vue.resource(process.env.VUE_APP_API_URL + '/rooms.ashx{?hotelid,language,showinactive}')

export default {
    /**
     * 
     * @param {Number} hotelid 
     * @param {String} language idioma de descripciones (en|es)
     * @param {Boolean} showinactive incluir habitaciones inactivas
     * @returns {Promise<[Any]>} 
     */
    rooms(hotelid, language, showinactive){
        return rooms.get({
            'hotelid': hotelid, 
            'language': language || 'en', 
            'showinactive': (showinactive||'false').toLowerCase() == 'true'
        })
    },

    /**
     * 
     * @param {Number} hotelid 
     * @param {String} startdate fecha en formato ISO
     * @param {String} enddate fecha en formato ISO
     * @param {String} language idioma de descripciones (en|es)
     * @returns {Promise<[Any]>} 
     */
    rates(hotelid, startdate, enddate, language){
        return rates.get({
            'hotelid': hotelid,
            'startdate': startdate,
            'enddate': enddate,
            'language': language || 'en'
        })
    }
}