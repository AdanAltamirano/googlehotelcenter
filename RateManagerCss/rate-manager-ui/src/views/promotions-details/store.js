import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
    strict: true,
    state: {
        qs: Vue.appConfig.code || '',
        hotelId: Vue.appConfig.session.hotelId,

        req: {
            code: null,
            name: {
                es: '',
                en: ''
            },
            description: {
                es: '',
                en: ''
            },
            typePromotion: {
                freeNight: {
                    check: false,
                    mode: 0,
                    number: 1,
                },
                discount: {
                    check: false,
                    percentage: 0,
                    mode: ''
                }
            },
            bookingWindow: {
                initialDate: null,
                finalDate: null,
                specifyTime: false,
                minDay: 1,
                maxDay: 1
            },
            ratePlanRooms: {
                rooms: [],
                ratePlans: []
            },
            travelWindow: {
                initialDate: null,
                finalDate: null,
                validDays: [],
                noArrivalDays: [],
                closures: []
            },
            restriction: {

            }
        }
    },
    mutations: {
        getPromotion(state) {
            if (state.qs !== '') {
                OffersService.getByCode(state.hotelId, state.qs).then(response => {
                    
                });
            }
        },
        //updates
        code(state, v) { state.req.code = v },
        nameEs(state, v) { state.req.name.es = v },
        nameEn(state, v) { state.req.name.en = v },
        descriptionEs(state, v) { state.req.description.es = v },
        descriptionEn(state, v) { state.req.description.en = v }
    }
});