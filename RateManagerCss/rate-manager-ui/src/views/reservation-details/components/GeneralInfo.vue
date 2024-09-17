<template>
    <div class="card card-accent-primary">
        <div class="card-header d-flex">
            <h6>
                <i class="fa fa-user"></i> {{$t('Client data')}}
            </h6>
            <span class="ml-auto">
                {{$t('Reservation date')}}: <strong>{{$moment(result.reservationDate).format('D MMM YYYY, h:mm:ss a')}}</strong>
            </span>
        </div>
        <div class="card-body">
            <b-row>
                <b-col md="5" v-if="result.customer">
                    <h3>{{result.customer.name}} {{result.customer.lastName}}</h3>
                    <address>
                        <span v-if="result.customer.email != ''">
                            {{$t('Email')}}: <strong>{{result.customer.email}}</strong>
                            <br>
                        </span>
                        <span v-if="result.customer.phone != ''">
                            {{$t('Phone')}}: <strong>{{result.customer.phone}}</strong>
                            <br>
                        </span>
                        <span>{{result.nights}} {{$t('Night(s)')}}</span>
                        <br>
                        <span>{{occupation}}</span>
                    </address>
                </b-col>
                <b-col md="4" :class="status_class" class="alert">
                    <h4>{{status}}</h4>
                    <address>
                        {{$t('Reservation number')}}: <strong>{{result.reservationNumber}}</strong>
                        <br>
                        {{$t('Source')}}: <strong>{{result.portal}}</strong>
                        <br>
                        <div v-if="result.source == 'IDS'">
                            {{$t('Confirmation Number')}} IDS: <strong>{{result.reservationNumberIDS}}</strong>
                        </div>
                        <div v-if="result.paymentWay === 0 && result.bankDepositDetails.hasDebt" class="alert alert-warning">
                            <span>{{$t('Total debt')}}: <strong>{{result.bankDepositDetails.debt | currency}} {{result.bankDepositDetails.currency}}</strong></span>
                        </div>
                        <span v-if="result.status == 3 && result.cancellationReason != ''">
                            <br>
                            {{$t('Cancellation number')}}: <strong>{{result.cancellationNumber}}</strong>
                            <br>
                            <b-link @click="alertReason">{{$t('See reason for cancellation')}}</b-link>
                            <br>
                        </span>
                        <span v-if="result.agency !== null && result.agency !== ''">
                            {{$t('Agency')}}: <strong>{{result.agency}}</strong>
                            <br>
                            {{ $t('User') }}: <strong>{{ result.agencyUser }}</strong>
                        </span>
                        <span v-if="result.showLogs">
                            <b-link @click="alertHistoryLog">{{$t('Track record')}} <i class="fas fa-file-alt"></i></b-link>
                            <br>
                        </span>    
                    </address>
                </b-col>
                <b-col md="3">
                    <b-card-group class="text-center small" deck>
                        <div class="card mr-2">
                            <div class="card-header bg-primary text-white">{{$t('Check in')}}</div>
                            <div class="card-body">
                                <small>{{$moment(result.checkIn).format('MMMM')}}</small>
                                <h2>{{$moment(result.checkIn).format('D')}}</h2>
                                <small>{{$moment(result.checkIn).format('YYYY')}}</small>
                            </div>
                        </div>
                        <div class="card ml-2">
                            <div class="card-header bg-danger text-white">{{$t('Check out')}}</div>
                            <div class="card-body">
                                <small>{{$moment(result.checkOut).format('MMMM')}}</small>
                                <h2>{{$moment(result.checkOut).format('D')}}</h2>
                                <small>{{$moment(result.checkOut).format('YYYY')}}</small>
                            </div>
                        </div>
                    </b-card-group>
                </b-col>
            </b-row>
        </div>
    </div>
</template>
<script>
import Vue from "vue";
import Record from './Record/Log.vue';

export default {
    props: {
        result: {
            required: true,
            type: Object,
        },
    },
    components: {
        Record
    },
    methods: {
        alertReason() {
            //TODO:Hide when userCancellation is undefined

            let by = '';

            if(this.result.userCancellation)
            {
                by = `${this.$t('Cancelled By')}: ${this.result.userCancellation}`
            }


            let html = `${this.$t('Reason')}: ${this.result.cancellationReason} 
            <br>${by}`;

            this.$swal
            .fire({
                //text: this.result.cancellationReason,
                title: this.$t('Reason for cancellation'),
                html:html,
                showConfirmButton: false,
            })
        },
        alertHistoryLog(){

            let component = Vue.extend(Record);
            let instance = new component({
                propsData: {
                    reservationNumber : this.result.reservationNumber,
                }
            });
            
            instance.$mount();

             this.$swal
                .fire({
                customClass:{
                    popup:'swal2-max-width'
                },
                title: this.$t("Track Record"),
                type: "info",
                html: "<div></div>",
                showLoaderOnConfirm: false,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Close"),
                cancelButtonColor: "#d33",
                onBeforeOpen: () => {
                    this.$swal
                    .getContent()
                    .querySelector("div")
                    .append(instance.$el);
                },
            });

        }
    },
    computed: {
        status() {
            let val;
            switch(this.result.status) {
                case 1: val = this.$t('Confirmed'); break;
                case 3: val = this.$t('Cancelled'); break;
                case 4: val = this.$t('In process'); break;
            }
            return val;
        },
        status_class() {
            return {
                'alert-success': this.result.status == 1,
                'alert-danger': this.result.status == 3,
                'alert-warning': this.result.status == 4,
            };
        },
        occupation() {
            let adults = 0;
            let childrens = 0;
            this.result.roomDetails.forEach(r => {
                adults += (r.adults + r.extraAdults);
                childrens += (r.childrens + r.extraChildrens)
            });

            return `${adults} ${this.$t('Adult(s)')} ${childrens > 0 ? ', ' : ''}
            ${childrens > 0 ? (childrens + ' ' + this.$t('Children')) : ''}`;
        }
    }
}
</script>

