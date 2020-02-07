import Vue from "vue";
import VueResource from "vue-resource";
import Interceptor from "./interceptor";

Vue.use(VueResource);
Vue.http.interceptors.push(Interceptor);

const reservationList = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations{?filter,orderBy,pageSize,page}`);
const reservationDetails = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/{patch}`);
const creditcard = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/creditcard/{code}`);
const sendNotification = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/{reservationId}/sendnotification`);
const excel = Vue.resource(`${process.env.VUE_APP_API_URL}/reservations/excel{?filter,orderBy,pageSize,page}`);

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

    ReservationUpdate(reservationId, patch, request) {
        return reservationDetails.save({
                reservationId,
                patch
            },
            request
        );
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
    }
};