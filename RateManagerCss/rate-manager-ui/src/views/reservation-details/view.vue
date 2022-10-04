<template>
  <div v-if="showInfo" id="app">
    <b-container class="text-muted" style="padding:15px;" fluid>
      <h2 class="text-primary">
        <img :src="DefaultImage" style="width:25px;margin-right:5px;" />
        {{$t('Reservation details')}} - #{{reservationId}}
      </h2>
      <b-row class="pt-4 pb-1">
        <b-col>
          <actions :reservationId="reservationId" :result="result"></actions>
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
              <rooms :ratePlan="result.ratePlan" :rooms="result.roomDetails"></rooms>
            </b-col>
          </b-row>
          <b-row v-if="result.policyDetails" class="pt-3">
            <b-col>
              <policies :result="result"></policies>
            </b-col>
          </b-row>
        </b-col>
        <b-col md="6">
          <b-row v-if="result.paymentWay >= 0">
            <b-col>
              <payment-methods :result="result" :reservationId="this.reservationId"></payment-methods>
            </b-col>
          </b-row>
          <b-row v-if="result.totalDetails" class="pt-3">
            <b-col>
              <h5 class="text-info">{{$t('Cost summary')}}</h5>
              <template v-if="result.isNetRateUV">
                <table class="table table-sm">
                  <tbody>
                    <tr>
                      <td>SubTotal</td>
                      <td>{{result.totalDetails.subTotal | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr v-if="isSupervisor || isUsuarioHotelAssociation">
                      <td>{{$t('Taxes')}}</td>
                      <td>{{result.totalDetails.taxes | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                     <tr>
                      <td>{{$t('Ecotax')}}</td>
                      <td>{{result.totalDetails.ecotasa | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr v-if="isSupervisor || isUsuarioHotelAssociation">
                      <td>
                        <strong>Total</strong>
                      </td>
                      <td>
                        <strong>{{result.totalDetails.total | currency}} {{result.totalDetails.currency}}</strong>
                      </td>
                    </tr>
                    <tr v-if="isSupervisor || isUsuarioHotelAssociation" style="font-size:smaller;">
                      <td>{{$t('Commission Internet Power')}}</td>
                      <td>{{result.totalDetails.commission | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr style="font-size:smaller;">
                      <td>{{$t('Taxes Hotel')}}</td>
                      <td>{{result.totalDetails.taxesHotel | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr style="font-size:smaller;">
                      <td>{{$t('Total Hotel')}}</td>
                      <td>{{result.totalDetails.totalNR | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                  </tbody>
                </table>
              </template>
              <template v-else>
                <table class="table table-sm">
                  <tbody>
                    <tr>
                      <td>SubTotal</td>
                      <td>{{result.totalDetails.subTotal | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr>
                      <td>{{$t('Taxes')}}</td>
                      <td>{{result.totalDetails.taxes | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr>
                      <td>{{$t('Ecotax')}}</td>
                      <td>{{result.totalDetails.ecotasa | currency}} {{result.totalDetails.currency}}</td>
                    </tr>
                    <tr>
                      <td>
                        <strong>Total</strong>
                      </td>
                      <td>
                        <strong>{{result.totalDetails.total | currency}} {{result.totalDetails.currency}}</strong>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </template>
            </b-col>
          </b-row>
          <b-row v-if="result.pms">
            <b-col>
              <h6 style="cursor:pointer" v-b-toggle.pms>
                <i class="fa fa-plus-circle"></i>
                {{$t('PMS Status')}}
              </h6>
              <b-collapse visible id="pms">
                <b-alert show variant="secondary">
                  <address>
                    <strong>{{(!result.pms.status ? $t('Waiting to be confirmed') : $t('Reservation confirmed'))}}</strong>
                    <br />
                    <strong>{{$t('Status')}}:</strong>
                    {{PmsStatus}}
                    <br />
                    <span v-if="result.pms.status">
                      <strong>{{$t('Reservation number')}}:</strong>
                      {{result.pms.reservationNumber}}
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
import ReservationService from "../../api/reservation-service";
import Actions from "./components/Actions.vue";
import GeneralInfo from "./components/GeneralInfo.vue";
import Rooms from "./components/Rooms.vue";
import Policies from "./components/Policies.vue";
import PaymentMethods from "./components/PaymentMethods.vue";
import image from "./assets/internetpower.png";
export default {
  name: "app",
  components: {
    Actions,
    GeneralInfo,
    Rooms,
    Policies,
    PaymentMethods
  },
  created() {
    this.session();
    this.showLoader();
    ReservationService.GetDetails(this.reservationId).then(response => {
      this.result = response.body;
      console.log(response.body);
      this.hideLoader();
    });
    console.log(this.isHotelUser);
    console.log(this.isHotelCompany);

  },
  data() {
    return {
      reservationId: this.$appConfig.confirmNumber,
      result: [],
      showInfo: false,
      loader: null,
      image: image,
      domain: document.location.origin,
      isSupervisor: (this.$appConfig.session.isSupervisor === 'True')? true : false,
      isHotelUser: (this.$appConfig.session.isHotelUser === 'True')? true : false,
      isHotelCompany:(this.$appConfig.session.isHotelCompany === 'True')? true : false,
      isUsuarioHotelAssociation: (this.$appConfig.session.isUsuarioHotelAssociation === 'True')? true : false,
    };
  },
  computed: {
    PmsStatus() {
      let r;
      switch (this.result.pms.action) {
        case "SS":
          r = this.$t("New");
          break;
        case "CC":
          r = this.$t("Modified");
          break;
        case "XX":
          r = this.$t("Canceled");
          break;
      }
      return r;
    },
    DefaultImage() {
      return this.image;
    }
    // TestOriginImage() {
    //   return (
    //     this.domain +
    //     "/Ratemanager/Includes/imagenes/istotipo-internetpowerhotel-normal.png"
    //   );
    // },
    // ProductionOriginImage() {
    //   return (
    //     this.domain +
    //     "/Includes/imagenes/istotipo-internetpowerhotel-normal.png"
    //   );
    // },
    // CubaOriginImage() {
    //   return (
    //     this.domain +
    //     "/Ratemanager/Includes/imagenes/istotipo-internetpowerhotel-normal.png"
    //   );
    // }
  },
  methods: {
    showLoader() {
      this.loader = this.$loading.show({
        color: this.$appConfig.themeColors.info,
        height: 128,
        width: 128
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
          if (self.$swal.isVisible()) return;
          self.$swal
            .fire({
              title: self.$t("Expired session"),
              text: self.$t("The session expires due to inactivity"),
              type: "warning",
              confirmButtonText: self.$t("Keep"),
              cancelButtonText: self.$t("Exit"),
              showCloseButton: false,
              showCancelButton: true,
              allowOutsideClick: false,
              allowEscapeKey: false
            })
            .then(result => {
              if (result.value) {
                window.session_counter = 0;
                window.wait_user_counter = 0;
              } else window.close();
            });

          setInterval(function() {
            if (window.session_counter >= minutes_session) {
              window.wait_user_counter++;
              if (window.wait_user_counter >= minutes_session_user)
                window.close();
            }
          }, 60000);
        }
      }, 60000);
    }
  }
};
</script>
