import Vue from 'vue';
import VueResource from 'vue-resource';
import Utilities from '../core/utilities';
import EventBus from '../core/event-bus';

Vue.use(VueResource);

const proccessing = { count: 0 };

Vue.http.interceptors.push((request) => {
    console.log(request);
    if (proccessing.count === 0) EventBus.$emit('api.call.begin');
    proccessing.count += 1;
    return () => {
        proccessing.count -= 1;
        if (proccessing.count === 0) EventBus.$emit('api.call.end');
    };
});


const rates = Vue.resource(`${process.env.VUE_APP_API_URL}/rates.ashx{?hotelid,startdate,enddate,language}`);
const ratedatails = Vue.resource(`${process.env.VUE_APP_API_URL}/rates/daily.ashx{?rateid,day}`);
const rooms = Vue.resource(`${process.env.VUE_APP_API_URL}/rooms.ashx{?hotelid,language,showinactive}`);
const inventory = Vue.resource(`${process.env.VUE_APP_API_URL}/rooms/inventory.ashx{?roomid,startdate,enddate}`);


export default {
    /**
       *
       * @param {Number} hotelId
       * @param {String} language idioma de descripciones (en|es)
       * @param {Boolean} showInactive incluir habitaciones inactivas
       * @returns {Promise<[Any]>}
       */
    rooms(hotelId, language, showInactive) {
        return rooms.get({
            hotelid: hotelId,
            language: language || 'en',
            showinactive: (showInactive || 'false').toLowerCase() === 'true',
        });
    },

    /**
       *
       * @param {Number} roomId
       * @param {String} startDate fecha en formato ISO
       * @param {String} endDate fecha en formato ISO
       * @returns {Promise<[Any]>}
       */
    inventory(roomId, startDate, endDate) {
        return inventory.get({
            roomid: roomId,
            startdate: startDate,
            enddate: endDate,
        });
    },

    /**
       *
       * @param {Number} hotelId
       * @param {String} startDate fecha en formato ISO
       * @param {String} endDate fecha en formato ISO
       * @param {String} language idioma de descripciones (en|es)
       * @returns {Promise<[Any]>}
       */
    rates(hotelId, startDate, endDate, language) {
        return rates.get({
            hotelid: hotelId,
            startdate: startDate,
            enddate: endDate,
            language: language || 'en',
        });
    },

    /**
       *
       * @param {Number} rateId
       * @param {String} day fecha en formato ISO
       * @returns {Promise<[Any]>}
       */
    rateDetails(rateId, day) {
        return ratedatails.get({
            rateid: rateId,
            day,
            ignoreTrack: true,
        });
    },

    /**
       *
       * @param {Number} hotelId
       * @param {String} startDate fecha en formato ISO
       * @param {String} endDate fecha en formato ISO
       * @param {String} language idioma de descripciones (en|es)
       */
    roomsWithRatesAndInventory(hotelId, startDate, endDate, language) {
        const roomsReq = this.rooms(hotelId, language);
        const ratesReq = this.rates(hotelId, startDate, endDate, language);
        return new Promise((resolve, reject) => {
            Promise.all([roomsReq, ratesReq])
                .then(([roomsRes, ratesRes]) => {
                    const roomsAndRates = Utilities.mixRoomsAndRates(roomsRes.body, ratesRes.body);

                    // ir por el inventario de las habitaciones que tienen tarifas
                    const invetoryPromises = roomsAndRates.map(room => this.inventory(room.id, startDate, endDate));

                    Promise.all(invetoryPromises)
                        .then((responses) => {
                            const inventory = responses.map(r => r.body);
                            const fullMix = roomsAndRates.map((room) => {
                                [room.inventory] = (inventory.filter(r => (r[0] || {}).roomId === room.id));
                                return room;
                            });
                            resolve({
                                rooms: roomsRes.body, rates: ratesRes.body, inventory, mixin: fullMix,
                            });
                        });
                })
                .catch((reason) => {
                    reject(reason);
                });
        });
    },

};
