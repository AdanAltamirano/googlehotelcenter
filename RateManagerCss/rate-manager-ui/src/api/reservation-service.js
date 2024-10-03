import Vue from "vue";
import VueResource from "vue-resource";
import Interceptor from "./interceptor";

Vue.use(VueResource);
Vue.http.interceptors.push(Interceptor);

const reservationList = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations{?filter,orderBy,pageSize,page}`);
const reservationDetails = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/{patch}`);
const reservationDeposit = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/deposit`);

const reservationPmsUpdate = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/pms/update`);
const reservationPmsStatusUpdate = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/pms/status/update`);
const reservationPmsVerifyUpdate = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/pms/verify/update`);
const reservationPmsReactivate = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/pms/reactivate`);

const creditcard = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/creditcard/{code}`);
const sendNotification = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/sendnotification`);
const excel = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/excel{?filter,orderBy,pageSize,page}`);
const corporate = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/corporate`);
const agencies = Vue.resource(`${process.env.VUE_APP_API_URL}/agencies`);
const agents = Vue.resource(`${process.env.VUE_APP_API_URL}/agencies/agents`);

const reservationHistoryLog = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/history/log`);
const reservationDetailLog = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/history/log/detail/{source}/{idLog}`);

export default {
    /**
     *
     * @param {*} filter
     * @param {*} orderBy
     * @param {*} pageSize
     * @param {*} page
     */
    GetAll(filter, orderBy, pageSize, page) {
        return reservationList.get({
            filter,
            orderBy,
            pageSize,
            page
        });
    },

    GetDetails(reservationId) {
        return reservationDetails.get({
            reservationId
        });
    },
    GetHistoryLog(reservationId) {
        return reservationHistoryLog.get({
            reservationId
        });
    },
    GetDetailLog(reservationId,source,idLog){
        return reservationDetailLog.get({
            reservationId,
            source,
            idLog
        });
    },
    SendCode(reservationId) {
        return creditcard.get({
            reservationId
        });
    },

    GetCreditCard(reservationId, code) {
        return creditcard.get({
            reservationId,
            code
        });
    },

    ReservationUpdate(reservationId, patch, request, sendNotification) {

        let headersParams = {
            'Notification' : sendNotification
        };


        const reservationUpdate = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/{patch}`,
                                    {},
                                    {},
                                    { headers: headersParams });

        return reservationUpdate.save({
                reservationId,
                patch
            },
            request
        );


        // return reservationDetails.save({
        //         reservationId,
        //         patch
        //     },
        //     request
        // );
    },
    ReservationDeposit(reservationId, request) {
        return reservationDeposit.save({
            reservationId
        }, request);
    },
    ReservationPmsUpdate(reservationId, request) {
        return reservationPmsUpdate.save({
            reservationId
        }, request);
    },
    ReservationPmsStatusUpdate(reservationId, request) {
        return reservationPmsStatusUpdate.save({
            reservationId
        }, request);
    },
    ReservationPmsVerifyUpdate(reservationId, request) {
        return reservationPmsVerifyUpdate.save({
            reservationId
        }, request);
    },
    ReservationPmsReactivate(reservationId) {
        return reservationPmsReactivate.get({ reservationId });
    },
    SendNotification(reservationId) {
        return sendNotification.get({
            reservationId
        });
    },
    GetExcel(filter, orderBy, pageSize, page) {
        //const excelUrl = `${process.env.VUE_APP_API_URL}/reservations/excel?filter=Status eq 1`;
        //return excelUrl;
        return excel.get({
            filter,
            orderBy,
            pageSize,
            page
        });
    },
    GetCorporate() {
        return corporate.get();
    },
    GetAgencies() {
        return agencies.get();
    },
    GetAgents(){
        return agents.get();
    }
};