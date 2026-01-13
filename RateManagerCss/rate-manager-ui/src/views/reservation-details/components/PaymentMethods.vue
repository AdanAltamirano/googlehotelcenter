<template>
  <div>
    <h5 class="text-info">{{$t('Payment details')}}</h5>
    <div class="card card-accent-primary">
      <div class="card-header">
        <h6>{{$t('Reserved by')}} {{paymentWay}}</h6>
      </div>
      <div class="card-body">
        <!--DEPOSITO BANCARIO-->
        <div v-if="result.paymentWay == 0">
          <address v-if="result.bankDepositDetails">
            <div class="d-flex col-gap-2">
              <span>
                {{$t('Total deposited')}}:
                <strong>{{result.bankDepositDetails.total | currency}}&nbsp;{{result.bankDepositDetails.currency}}</strong>
              </span>
              <confirm-deposit v-if="result.bankDepositDetails.hasDebt" :reservationId="result.reservationId" 
               :reservationNumber="result.reservationNumber" :reference="result.bankDepositDetails.reference">
              </confirm-deposit>
            </div>
            <div>
              {{$t('Bank')}}:
              <strong>{{result.bankDepositDetails.bank}}</strong>
            </div>
            <div>
              {{$t('Reference')}}:
              <strong>{{result.bankDepositDetails.reference}}</strong>
            </div>
            <div>
              {{$t('Deposited to')}}: <strong>{{result.bankDepositDetails.target}}</strong>              
            </div>
          </address>
          <b-alert show v-if="result.status == 4" variant="danger">
            {{$t('Please confirm by email once payment is made to')}}
            <a
              href="mailto:reservas@internetpowerhotel.com"
            >{{$t('Collection dept')}}</a>
          </b-alert>
        </div>
        <!--FIN DEPOSITO BANCARIO-->

        <!--PAGO EN LINEA-->
        <div v-if="result.paymentWay == 1">
          <b-alert show v-if="result.paymentDetails.authorizationNumber.length <= 0 && result.paymentDetails.reference.length <= 0" variant="warning">
            <small>{{$t('Pending payment')}}</small>
          </b-alert>
          <address v-if="(result.isNetRateUV && isSupervisor) || (!result.isNetRateUV)" class="mt-1">
            <div v-show="result.paymentDetails.pasarela.length > 0">
              {{$t('Payment Gateway')}}:
              <strong>{{result.paymentDetails.pasarela}}</strong>
              <br />
            </div>
            <div v-show="result.paymentDetails.authorizationNumber.length > 0">
              {{$t('Authorization number')}}:
              <strong>{{result.paymentDetails.authorizationNumber}}</strong>
              <br />
            </div>
            <div v-show="result.paymentDetails.reference.length > 0">
              {{$t('Reference')}}:
              <strong>{{result.paymentDetails.reference}}</strong>
              <br />
            </div>Total:
            <strong>{{result.paymentDetails.totalPay | currency}} {{result.paymentDetails.currencyPay}}</strong>
          </address>
        </div>
        <!--FIN PAGO EN LINEA-->

        <!--PAGO EN EL HOTEL-->
        <div v-if="result.paymentWay == 2">
          <address v-if="result.customer.cardDetails">
            {{$t('Credit card')}}
            <br />
            <div v-if="!ccDetails.isSuccess" class="d-flex">
              <span class="mt-2">
                {{$t('Card number')}}:
                <strong>{{result.customer.cardDetails.number}}</strong>
              </span>
              <b-button
                v-if="result.customer.cardDetails.allowsShowCreditCardData"
                class="text-info ml-auto"
                @click="show_alertDatacard"
                v-show="showBtn"
                variant="link"
              >{{$t('View card data')}}</b-button>
            </div>
            <div v-if="ccDetails.isSuccess">
              <address class="mt-2">
                {{$t('Name')}}:
                <strong>{{ccDetails.owner}}</strong>
                <br />
                {{$t('Type')}}:
                <strong>{{ccDetails.cardType}}</strong>
                <br />
                {{$t('Card number')}}:
                <strong>{{ccDetails.number}}</strong>
                <br />
                {{$t('Expiration date')}}:
                <strong>{{ccDetails.monthExpiration}}/{{ccDetails.yearExpiration}}</strong>
                <br />
                {{$t('Security code')}}:
                <strong>{{ccDetails.cvv}}</strong>
              </address>
            </div>
          </address>
          <b-alert
            v-if="!errorSendEmail"
            :show="dismiss_countDown"
            @dismissed="dismiss_countDown=0"
            @dismiss-count-down="countDownChanged"
            class="mt-1"
            variant="warning"
          >
            <small>{{$t('A verification code has been sent to your email, with which you can view the card details')}}</small>
            <b-form class="pt-1" inline @submit.prevent>
              <b-form-group :description="description_dismiss">
                <b-form-input v-model="code" @keydown.enter.prevent></b-form-input>&nbsp;
                <b-button @click="getCreditCardData" variant="primary">{{$t('Send')}}</b-button>
              </b-form-group>
            </b-form>
            <b-alert variant="danger" class="mt-1" :show="ccError">
              <small>!Error! {{$t('Invalid verification code')}}</small>
            </b-alert>
          </b-alert>
          <b-alert class="mt-1" v-if="errorSendEmail" variant="danger">
            <small>!Error! {{$t('There was a problem sending the mail')}}</small>
          </b-alert>
        </div>
        <!--FIN PAGO EN EL HOTEL-->

        <!-- PAGO TEXTO CCT -->
        <div v-if="result.paymentWay == 3">
          {{$t('Information Payment')}}:
          <strong>{{result.paymentInformation}}</strong>
        </div>
        <!-- -->

        <!-- OTA -->
        <div v-if="result.paymentWay == 4">
          <template v-if="result.portal.toUpperCase().includes('EXPEDIA')">
            <template v-if="result.typeCC != 10">
              <address v-if="result.customer.cardDetails && result.customer.cardDetails.showBasicCreditCardData">
                {{$t('Credit card')}}
                <br />
                <div v-if="!ccDetails.isSuccess" class="d-flex">
                  <span class="mt-2">
                    {{$t('Card number')}}:
                    <strong>{{result.customer.cardDetails.number}}</strong>
                  </span>
                  <b-button
                    v-if="result.customer.cardDetails.allowsShowCreditCardData"
                    class="text-info ml-auto"
                    @click="show_alertDatacard"
                    v-show="showBtn"
                    variant="link"
                  >{{$t('View card data')}}</b-button>
                </div>
                <div v-if="ccDetails.isSuccess">
                  <address class="mt-2">
                    {{$t('Name')}}:
                    <strong>{{ccDetails.owner}}</strong>
                    <br />
                    {{$t('Type')}}:
                    <strong>{{ccDetails.cardType}}</strong>
                    <br />
                    {{$t('Card number')}}:
                    <strong>{{ccDetails.number}}</strong>
                    <br />
                    {{$t('Expiration date')}}:
                    <strong>{{ccDetails.monthExpiration}}/{{ccDetails.yearExpiration}}</strong>
                    <br />
                    {{$t('Security code')}}:
                    <strong>{{ccDetails.cvv}}</strong>
                  </address>
                </div>
              </address>
              <b-alert
              v-if="!errorSendEmail"
              :show="dismiss_countDown"
              @dismissed="dismiss_countDown=0"
              @dismiss-count-down="countDownChanged"
              class="mt-1"
              variant="warning"
              >
                <small>{{$t('A verification code has been sent to your email, with which you can view the card details')}}</small>
                <b-form class="pt-1" @submit.prevent>
                  <b-form-group :description="description_dismiss">
                    <b-form-input v-model="code" @keydown.enter.prevent></b-form-input>&nbsp;
                    <b-button type="button" @click="getCreditCardData" variant="primary">{{$t('Send')}}</b-button>
                  </b-form-group>
                </b-form>
                <b-alert variant="danger" class="mt-1" :show="ccError">
                  <small>!Error! {{$t('Invalid verification code')}}</small>
                </b-alert>
              </b-alert>
              <b-alert class="mt-1" v-if="errorSendEmail" variant="danger">
                <small>!Error! {{$t('There was a problem sending the mail')}}</small>
              </b-alert>
            </template>
            <template v-if="result.typeCC == 10">
              <div>
                {{$t('Virtual Card')}}               
              </div>
              <div>
                {{$t('Amount to be charged')}}:
                <strong>{{result.descriptionCC | currency}} {{result.totalDetails.currency}}</strong>
              </div>
            </template>
          </template>
          <template v-if="result.portal.toUpperCase().includes('BOOKING')">
            <span><strong>{{$t('No payment information')}}</strong>
          </template>
          <template v-if="result.portal.toUpperCase().includes('DESPEGAR')">
            <span><strong>{{$t('No payment information')}}</strong>
          </template>
          <template v-if="result.portal.toUpperCase().includes('BESTDAY')">
            <span><strong>{{$t('No payment information')}}</strong>
          </template>
          <template v-if="result.portal.toUpperCase().includes('PRICETRAVEL')">
            <span><strong>{{$t('No payment information')}}</strong>
          </template>
          <template v-if="result.portal.toUpperCase().includes('HOTELBEDS')">
            <span><strong>{{$t('No payment information')}}</strong>
          </template>
        </div>
        <!-- FIN OTA -->
      </div>
    </div>
  </div>
</template>
<script>
import ReservationService from "../../../api/reservation-service";
import ConfirmDeposit from "../components/Deposit/Deposit.vue";
export default {
  components: { 
    ConfirmDeposit
  },
  data() {
    return {
      isSupervisor: (this.$appConfig.session.isSupervisor === 'True')? true : false,
      dismiss_sec: 300,
      dismiss_countDown: 0,
      code: "",
      ccDetails: {
        isSuccess: false
      },
      ccError: false,
      errorSendEmail: false
    };
  },
  props: {
    result: {
      required: true,
      type: Object
    },
    reservationId: {
      required: false,
      type: String
    }
  },
  methods: {
    countDownChanged(dismissCountDown) {
      this.dismiss_countDown = dismissCountDown;
    },
    show_alertDatacard() {
      this.dismiss_countDown = this.dismiss_sec;
      this.sendEmail();
    },
    getCreditCardData() {
      ReservationService.GetCreditCard(this.reservationId, this.code).then(
        response => {
          this.ccDetails = response.body;
          if (this.ccDetails.isSuccess) this.dismiss_countDown = 0;
          else {
            this.ccError = true;
            this.code = "";
          }
        }
      );
    },
    sendEmail() {
      ReservationService.SendCode(this.reservationId).then(response => {
        if (!response.body.success) {
          this.errorSendEmail = true;
          this.dismiss_countDown = 0;
        }
      });
    },
    onSubmit(){

    }
  },
  computed: {
    paymentWay() {
      let txt;
      switch (this.result.paymentWay) {
        case 0:
          txt = this.$t("Bank deposit");
          break;
        case 1:
          txt = this.$t("Online payment");
          break;
        case 2:
          txt = this.$t("Payment at the hotel");
          break;
        case 3:
          txt = this.$t("Call Center Other");
          break;
        case 4:
          txt = this.result.portal;
          break;
      }
      return txt;
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
    showBtn() {
      return this.dismiss_countDown == 0 ? true : false;
    }
  }
};
</script>
