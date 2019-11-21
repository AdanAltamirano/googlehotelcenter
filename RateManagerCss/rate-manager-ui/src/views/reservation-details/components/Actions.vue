<template>
  <div class="d-flex">
    <h4 class="text-info">{{result.hotelName}}</h4>
    <b-button-toolbar v-if="showCancelButton || showModifyButton" class="ml-auto">
      <b-dropdown class="mx-1" right variant="primary" :text="$t('Options')">
        <b-dropdown-item v-if="showModifyButton" @click="modify">{{$t('Modify')}}</b-dropdown-item>
        <b-dropdown-item v-if="showCancelButton" @click="cancel">{{$t('Cancel')}}</b-dropdown-item>
        <b-dropdown-item
          v-if="showSendNotificationButton"
          @click="sendNotification"
        >{{$t('SendNotification')}}</b-dropdown-item>
      </b-dropdown>
    </b-button-toolbar>
  </div>
</template>
<script>
import ReservationService from "../../../api/reservation-service";
import Vue from "vue";
import Modify from "./Modify.vue";
export default {
  props: {
    result: {
      required: true,
      type: Object
    },
    noReservation: {
      required: true,
      type: String
    }
  },
  methods: {
    modify() {
      let component = Vue.extend(Modify);
      let instance = new component({
        propsData: {
          name: this.result.customer.name,
          lastName: this.result.customer.lastName,
          checkIn: this.$moment(this.result.checkIn),
          checkOut: this.$moment(this.result.checkOut),
          total: this.result.totalDetails.total,
          totalNR: this.result.totalDetails.totalNR,
          showTotalNR: this.result.paymentWay == 1
        }
      });
      instance.$mount();

      let self = this;
      this.$swal
        .fire({
          title: self.$t("Modify reservation"),
          type: "info",
          html: "<div></div>",
          showLoaderOnConfirm: true,
          showCancelButton: true,
          cancelButtonText: self.$t("Cancel"),
          cancelButtonColor: "#d33",
          confirmButtonColor: "#3085d6",
          confirmButtonText: self.$t("Modify"),
          onBeforeOpen: () => {
            this.$swal
              .getContent()
              .querySelector("div")
              .append(instance.$el);
          },
          preConfirm: () => {
            let req = {
              name: self.getElement("name"),
              lastName: self.getElement("lastname"),
              checkIn: self.dateFormat(self.getElement("checkin", true)),
              checkOut: self.dateFormat(self.getElement("checkout", true)),
              total: parseFloat(self.getElement("total")),
              totalNR: parseFloat(self.getElement("totalnr"))
            };

            return ReservationService.ReservationUpdate(
              self.noReservation,
              "modify",
              req
            ).then(response => {
              return {
                response: response.body,
                modifyTags: req
              };
            });
          },
          allowOutsideClick: () => !this.$swal.isLoading()
        })
        .then(result => {
          if (result.value) {
            let v = result.value.response;
            let t = result.value.modifyTags;
            if (v.isSuccess) {
              self.result.customer.name = t.name;
              self.result.customer.lastName = t.lastName;
              self.result.checkIn = t.checkIn;
              self.result.checkOut = t.checkOut;
              self.result.totalDetails.total;

              this.$swal.fire(
                self.success(self.$t("Your reservation was modified"))
              );
            } else
              this.$swal.fire(
                self.error(self.$t("Failed to modify the reservation"), v.error)
              );
          }
        });
    },
    cancel() {
      let self = this;
      this.$swal
        .fire({
          title: self.$t("Are you sure you want to cancel?"),
          input: "textarea",
          type: "warning",
          inputPlaceholder: self.$t("Reason to cancel"),
          showCancelButton: true,
          cancelButtonColor: "#d33",
          showLoaderOnConfirm: true,
          confirmButtonText: self.$t("Cancel"),
          confirmButtonColor: "#3085d6",
          cancelButtonText: self.$t("Exit"),
          preConfirm: textarea => {
            self.result.cancellationReason = textarea;
            return ReservationService.ReservationUpdate(
              self.noReservation,
              "cancel",
              { reason: textarea }
            ).then(response => {
              return response.body;
            });
          },
          allowOutsideClick: () => !this.$swal.isLoading()
        })
        .then(result => {
          if (result.value) {
            let v = result.value;
            if (v.isSuccess) {
              self.result.status = 3;
              self.result.cancellationNumber = v.cancelNumber;

              this.$swal.fire(
                self.success(self.$t("Your reservation was canceled"))
              );
            } else
              this.$swal.fire(
                self.error(self.$t("Failed to cancel the reservation"), v.error)
              );
          }
        });
    },
    success(title) {
      return {
        type: "success",
        title: title,
        showConfirmButton: false,
        timer: 2500
      };
    },
    error(title, message) {
      return {
        type: "error",
        title: title,
        text: message,
        showConfirmButton: false
      };
    },
    dateFormat(date) {
      date = date.split("/");
      date = `${date[2]}/${date[1]}/${date[0]}`;
      return this.$moment(date).format("YYYY-MM-DD");
    },
    getElement(tag, isDate) {
      if (isDate || false)
        return document.getElementById(tag).children[0].value;
      else return document.getElementById(tag).value;
    },
    sendNotification() {
      let self = this;
      this.$swal
        .fire({
          title: self.$t("Send Notifications"),
          type: "info",
          showCancelButton: true,
          cancelButtonColor: "#d33",
          confirmButtonText: self.$t("Send"),
          cancelButtonText: self.$t("Exit"),
          showLoaderOnConfirm: true,
          preConfirm: () => {
            return ReservationService.SendNotification(
              this.result.reservationNumber
            ).then(response => {
              return response.ok;
            });
          },
          allowOutsideClick: () => !this.$swal.isLoading()
        })
        .then(result => {
          console.log(result);
          if (result.value) {
            if (result.value) {
              this.$swal.fire({
                type: "success",
                title: self.$t("Notifications Sent"),
                showConfirmButton: false,
                showCloseButton: true,
                timer: 2500
              });
            } else {
              this.$swal.fire({
                type: "error",
                title: self.$t("Notifications Not Sent"),
                showConfirmButton: false,
                showCloseButton: true,
                timer: 2500
              });
            }
          }
        });
    }
  },
  computed: {
    showCancelButton() {
      return this.result.allowsCancel && this.result.status != 3;
    },
    showModifyButton() {
      return this.result.allowsModify;
    },
    showSendNotificationButton() {
      return this.result.status === 1;
    }
  }
};
</script>

