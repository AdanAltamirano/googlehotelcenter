import Vue from 'vue'
import Vuex from 'vuex'
import ApiService from '../api/api-service'


export default new Vuex.Store({
  state: {
    dateRange: {
        start: null,
        end: null
    },
    hotelId: Vue.appConfig.session.hotelId,
    rooms:[],
    rates: [],
    inventory:[],
    roomsWithRatesAndInventory:[]
  },
  mutations: {
    update(state, {start, end}) {
        ApiService.roomsWithRatesAndInventory(state.hotelId, start, end, Vue.appConfig.language)
        .then((response)=>{
            state.dateRange.start = start;
            state.dateRange.end = end;
            state.rooms = response.rooms;
            state.rates = response.rates;
            state.inventory = response.inventory;
            state.roomsWithRatesAndInventory = response.mixin;
        })
        .catch(reason => {

        });
    }
  },
  getters: {
    rooms: state => state.rooms,
    roomsWithRatesAndInventory: state => state.roomsWithRatesAndInventory,
  }
})