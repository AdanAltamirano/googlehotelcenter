import Vue from 'vue';
import Vuex from 'vuex';
import reservationService from '../../api/reservation-service';

Vue.use(Vuex);

export default new Vuex.Store({
    state:
    {
        reservations: [],
        hotelId: Vue.appConfig.session.hotelId
    },
    mutations:
    {
        GetAllReservationsById(state, filter)
        {
            reservationService.GetById(state.hotelId, filter).then((response) =>
            {
                state.reservations = response.body;
            })
        }
    },
    getters:
    {
        reservations: state => state.reservations
    }
})