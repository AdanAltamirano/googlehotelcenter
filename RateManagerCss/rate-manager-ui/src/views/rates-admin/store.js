import Vue from 'vue';
import Vuex from 'vuex';
import Utilities from '../../core/utilities';
import HotelService from '../../api/hotels-service';
import RoomsService from '../../api/rooms-service';
import RatesService from '../../api/rates-service';


const roomsWithRatesAndInventory = function (hotelId, startDate, endDate) {
    const roomsReq = RoomsService.getList(hotelId, 'Active eq True');
    const ratesReq = RatesService.getByRatePlan(hotelId, startDate, endDate);

    return new Promise((resolve, reject) => {
        Promise.all([roomsReq, ratesReq])
            .then(([roomsRes, ratesRes]) => {
                const roomsAndRates = Utilities.mixRoomsAndRates(roomsRes.body, ratesRes.body);

                // ir por el inventario de las habitaciones que tienen tarifas
                const invetoryPromises = roomsAndRates.map(room => RoomsService.getInventory(hotelId, room.id, startDate, endDate));

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
};

export default new Vuex.Store({
    state: {
        dateRange: {
            start: null,
            end: null,
        },
        hotelId: Vue.appConfig.session.hotelId,
        hotel: null,
        rooms: [],
        rates: [],
        inventory: [],
        roomsWithRatesAndInventory: [],
    },
    mutations: {
        /**
         * Obtiene la información básica de un hotel por id
         */
        getHotel(state) {
            HotelService.get(state.hotelId).then(((response) => {
                state.hotel = response.body;
            }))
            .catch((reason) => {

            });
        },

        update(state, { start, end }) {
            roomsWithRatesAndInventory(state.hotelId, start.format('YYYY-MM-DD'), end.format('YYYY-MM-DD'))
                .then((response) => {
                    state.dateRange.start = start;
                    state.dateRange.end = end;
                    state.rooms = response.rooms;
                    state.rates = response.rates;
                    state.inventory = response.inventory;
                    state.roomsWithRatesAndInventory = response.mixin;
                })
                .catch((reason) => {
                });
        },
    },
    getters: {
        hotel: state => state.hotel,
        rooms: state => state.rooms,
        roomsWithRatesAndInventory: state => state.roomsWithRatesAndInventory,
        dateRange: state => state.dateRange,
    },
});
