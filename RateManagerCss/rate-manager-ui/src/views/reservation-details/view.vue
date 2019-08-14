<template>
    <div v-if="showInfo" id="app">
        <b-container class="text-muted" style="padding:15px;" fluid>
            <h2 class="text-primary">{{$t('Reservation details')}} - #{{noReservation}}</h2>
            <b-row class="pt-4 pb-1">
                <b-col>
                    <actions :noReservation="noReservation" :result="result"></actions>
                </b-col>
            </b-row>
            <b-row class="pt-1">
                <b-col>
                    <general-info :result="result"></general-info>
                </b-col>
            </b-row>
            <b-row class="pt-3">
                <b-col md="6">
                    <b-row v-if="result.roomDetails">
                        <b-col>
                            <rooms :rooms="result.roomDetails"></rooms>
                        </b-col>
                    </b-row>
                    <b-row v-if="result.policyDetails" class="pt-3">
                        <b-col>
                            <policies :result="result"></policies>
                        </b-col>
                    </b-row>
                </b-col>
                <b-col md="6">
                    <b-row>
                        <b-col>
                            <payment-methods :result="result" :noReservation="this.noReservation"></payment-methods>
                        </b-col>
                    </b-row>
                    <b-row v-if="result.totalDetails" class="pt-3">
                        <b-col>
                            <h5 class="text-info">{{$t('Cost summary')}}</h5>
                            <table class="table table-sm">
                                <tbody>
                                    <tr>
                                        <td>SubTotal</td>
                                        <td>{{result.totalDetails.subTotal | currency}} {{result.totalDetails.currency}}</td>
                                    </tr>
                                    <tr v-if="!result.totalDetails.includesTax">
                                        <td>{{$t('Taxes')}}</td>
                                        <td>{{result.totalDetails.taxes | currency}} {{result.totalDetails.currency}}</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Total</strong></td>
                                        <td>
                                            <strong>{{result.totalDetails.total | currency}} {{result.totalDetails.currency}}</strong>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </b-col>
                    </b-row>
                    <b-row v-if="result.pms">
                        <b-col>
                            <h6 style="cursor:pointer" v-b-toggle.pms>
                                <i class="fa fa-plus-circle"></i> {{$t('Hotel confirmation')}}
                            </h6>
                            <b-collapse visible id="pms">
                                <b-alert show variant="secondary">
                                    <address>
                                        <strong>{{(result.pms.status === 0 ? $t('Waiting to be confirmed') : $t('Reservation confirmed'))}}</strong>
                                        <br>
                                        <strong>{{$t('Status')}}:</strong> {{PmsStatus}}
                                        <br>
                                        <span v-if="result.pms.status === 1">
                                            <strong>{{$t('Reservation number')}}:</strong> {{result.pms.reservationNumber}}
                                        </span>
                                    </address>
                                </b-alert>
                            </b-collapse>
                        </b-col>
                    </b-row>
                </b-col>
            </b-row>
        </b-container>
    </div>
</template>

<script>
import ReservationService from '../../api/reservation-service';
import Actions from './components/Actions.vue';
import GeneralInfo from  './components/GeneralInfo.vue';
import Rooms from './components/Rooms.vue';
import Policies from './components/Policies.vue';
import PaymentMethods from './components/PaymentMethods.vue';
export default {
    name: 'app',
    components: {
        Actions,
        GeneralInfo,
        Rooms,
        Policies,
        PaymentMethods,
    },
    created() {
        this.session();
        this.showLoader();
        ReservationService.GetDetails(this.noReservation).then(response => {
            this.result = response.body;
            this.hideLoader();
        });
    },
    data() {
        return {
            noReservation: this.$appConfig.confirmNumber,
            result: [],
            showInfo: false,
            loader: null,
            minutes_session: 1,
            minutes_session_user: 2,
        }
    },
    computed: {
        PmsStatus() {
            let r;
            switch(this.result.pms.action) {
                case 'SS': r = this.$t('New'); break;
                case 'CC': r = this.$t('Modified'); break;
                case 'XX': r = this.$t('Canceled'); break;
            }
            return r;
        }
    },
    methods: {
        showLoader() {
            this.loader = this.$loading.show({
                color: this.$appConfig.themeColors.info,
                height: 128,
                width: 128,
            });
        },
        hideLoader() {
            this.loader.hide();
            this.showInfo = true;
        },
        session() {
            const minutes_session = 10;
            const minutes_session_user = 3;
            window.session_counter = 0;
            window.wait_user_counter = 0;

            const self = this;
            setInterval(function() {
                window.session_counter++;
                if (window.session_counter >= minutes_session) {
                    self.$swal
                        .fire({
                            title: self.$t('Expired session'),
                            text: self.$t('he session expires due to inactivity'),
                            type: 'warning',
                            confirmButtonText: self.$t('Keep'),
                            cancelButtonText: self.$t('Exit'),
                            showCloseButton: false,
                            showCancelButton: true,
                            allowOutsideClick: false,
                            allowEscapeKey: false
                        }).then(result => {
                            if (result.value) {
                                window.session_counter = 0;
                                window.wait_user_counter = 0;
                            } else window.close();
                        })

                        setInterval(function() {
                            if (window.session_counter >= minutes_session) {
                                window.wait_user_counter++;
                                if (window.wait_user_counter >= minutes_session_user)
                                    window.close();
                            }
                        }, 60000);
                }
            }, 60000);
        },
    }
}
</script>
