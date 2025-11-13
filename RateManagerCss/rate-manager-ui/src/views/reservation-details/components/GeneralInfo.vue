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
                        <div v-if="!showClientData">
                            <b-button
                            class="text-info button-like-span"
                            @click="show_alertDatacard"
                            v-show="showBtn"
                            variant="link"
                            >{{$t('View Client Data')}}
                            </b-button>
                        </div>
                        <div v-if="showClientData">
                            <span v-if="result.customer.email != ''">
                            {{$t('Email')}}: <strong>{{result.customer.email}}</strong>
                            <br>
                            </span>
                            <span v-if="result.customer.phone != ''">
                                {{$t('Phone')}}: <strong>{{result.customer.phone}}</strong>
                                <br>
                            </span>
                        </div>
                        <b-alert
                        v-if="!errorSendEmail"
                        :show="dismiss_countDown"
                        @dismissed="dismiss_countDown=0"
                        @dismiss-count-down="countDownChanged"
                        class="mt-1"
                        variant="warning">                       
                            <small>{{$t('A verification code has been sent to your email, with which you can view the client details')}}</small>
                            <b-form class="pt-1" inline>
                                <b-form-group :description="description_dismiss">
                                    <b-form-input v-model="code"></b-form-input>&nbsp;
                                    <b-button @click="authorizeCodeCustomer" variant="primary">{{$t('Send')}}</b-button>
                                </b-form-group>
                            </b-form>
                            <b-alert variant="danger" class="mt-1" :show="codeError">
                                <small>!Error! {{$t('Invalid verification code')}}</small>
                            </b-alert>
                        </b-alert>
                        <b-alert class="mt-1" v-if="errorSendEmail" variant="danger">
                            <small>!Error! {{$t('There was a problem sending the mail')}}</small>
                        </b-alert>
                        <span>{{result.nights}} {{$t('Night(s)')}}</span>
                        <br>
                        <span>{{occupation}}</span>
                        <div v-if="ages.length > 0">
                            <span><strong>{{$t('Children Ages')}}</strong></span><br>
                            <span v-for="(age,index) in ages" :key="index">
                                {{age}} {{age === '1'? $t('year'): $t('years')}} {{index === ages.length - 1? '' : ','}}
                            </span>
                        </div> 
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
import EventBus from '../../../core/event-bus';
import Record from './Record/Log.vue';
import ReservationService from '../../../api/reservation-service';

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
    data(){
        return {
            showClientData: false,
            dismiss_sec: 300,
            dismiss_countDown: 0,
            code: "",
            errorSendEmail: false,
            codeError: false,
        }
    },
    mounted(){
        EventBus.$on('historymovementsreservation', () =>{
            console.log('Llego');
            this.alertHistoryLog();
        })
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

        },
        countDownChanged(dismissCountDown) {
            this.dismiss_countDown = dismissCountDown;
        },
        show_alertDatacard() {
            this.dismiss_countDown = this.dismiss_sec;
            this.sendEmail();
        },
        sendEmail() {
            ReservationService.SendCodeCustomer(this.result.reservationId).then(response => {
                console.log(response.body);
                if (!response.body.success) {
                    this.errorSendEmail = true;
                    this.dismiss_countDown = 0;
                }
            });
        },
        authorizeCodeCustomer(){
            ReservationService.GetAuhtorizationCustomerCode(this.result.reservationId, this.code)
            .then(response =>{
                if(response.body.success){
                    this.showClientData = true;
                    this.dismiss_countDown = 0
                }
                else{
                    this.codeError = true;
                    this.code = "";
                }
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
        },
        showBtn() {
            return this.dismiss_countDown == 0 ? true : false;
        },
        description_dismiss() {
            let txtTime = "";
            let seconds = this.dismiss_countDown;
            let minutes = Math.floor(this.dismiss_countDown / 60);
            if (minutes > 0) {
                seconds -= minutes * 60;
                txtTime += `${minutes} ${
                minutes > 1 ? `${this.$t("minute")}s` : this.$t("minute")
                }`;
            }
            txtTime += `${minutes > 0 && seconds > 0 ? ", " : ""}`;
            if (seconds > 0)
                txtTime += `${seconds} ${
                seconds > 1 ? `${this.$t("second")}s` : this.$t("second")
                }`;

            return `${this.$t("The code must be added before")} ${txtTime}`;
        },
        ages(){
            let ages = []; 

            if(this.result.roomDetails.length > 0){              
                this.result.roomDetails.forEach(r => {
                    if(r.ageChildren.length > 0){
                        let temp = r.ageChildren.split(',');
                        temp.forEach(t => {
                            ages.push(t);
                        });
                    }
                  
                });
            }

            return ages;
        }
    }
}
</script>

