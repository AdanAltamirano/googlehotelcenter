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
        GetAllReservations()
        {
            reservationService.GetAll(this.state.hotelId).then((response) =>
            {
                this.state.reservations = response.body;
            })
        }
    },
    getters:
    {
        reservations: state => state.reservations
    }
})