<template>
	<div>
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
			<b-form class="pt-1" inline>
				<b-form-group :description="description_dismiss">
					<b-form-input v-model="code"></b-form-input>&nbsp;
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
</template>
<script>
import ReservationService from "../../../../api/reservation-service";
export default {
	name:'Credit Card',
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
	data() {
		return {

		}
	},
	computed: {
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
    }
  },
}
</script>