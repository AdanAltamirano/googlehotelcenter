<template>
  <div class="d-flex">
    <h4 class="text-info" style="color:#10467a !important;">
      <i class="fas fa-hotel fa-sm"></i>
      {{result.hotelName}} {{showCorporate}}
    </h4>
    <b-button-toolbar v-if="showCancelButton || showModifyButton || showReactivateButton" class="ml-auto">
      <b-dropdown class="mx-1" right variant="primary" :text="$t('Options')">
        <b-dropdown-item v-if="showModifyButton" @click="modify">{{$t('Modify')}}</b-dropdown-item>
        <b-dropdown-item v-if="showCancelButton" @click="cancel">{{$t('Cancel')}}</b-dropdown-item>
        <b-dropdown-item v-if="showReactivateButton" @click="reactivate">{{$t('Reactivate')}}</b-dropdown-item>
        <!--<b-dropdown-item
          v-if="showSendNotificationButton"
          @click="sendNotification"
        >{{$t('SendEmail')}}</b-dropdown-item>-->
      </b-dropdown>
    </b-button-toolbar>
  </div>
</template>
<script>
import ReservationService from "../../../api/reservation-service";
import Vue from "vue";
import Modify from "./Modify.vue";
import ModificationTemplate from "./Email/ModificationTemplate.vue"
export default {
  props: {
    result: {
      required: true,
      type: Object
    },
    reservationId: {
      required: true,
      type: String
    }
  },
  data(){
    return {
      isAgencyCompany: (this.$appConfig.session.isAgencyCompany === 'True')? true : false,
    }
  },
  methods: {
    modify() {
      let component = Vue.extend(Modify);
      console.log("TotalNR: " + this.result.totalDetails.totalNR)
      let instance = new component({
        propsData: {
          name: this.result.customer.name,
          lastName: this.result.customer.lastName,
          checkIn: this.$moment(this.result.checkIn),
          checkOut: this.$moment(this.result.checkOut),
          total: this.result.totalDetails.total,
          totalNR: this.result.totalDetails.totalNR,
          showTotalNR: true,
          details : ''
        }
      });
      instance.$mount();

      let self = this;
      this.$swal
        .fire({
          customClass:{
            actions:'swal3-actions'
          },
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
              totalNR: parseFloat(self.getElement("totalnr")),
              details: self.getElement("details")
            };
          
            return ReservationService.ReservationUpdate(
              self.reservationId,
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
          console.log(result)
          if (result.value) {
            let v = result.value.response;
            let t = result.value.modifyTags;
            if (v.isSuccess) {
              // self.result.customer.name = t.name;
              // self.result.customer.lastName = t.lastName;
              // self.result.checkIn = t.checkIn;
              // self.result.checkOut = t.checkOut;
              // self.result.totalDetails.total;

              let component = Vue.extend(ModificationTemplate);
              console.log(v.customerEmail)
              console.log(v.hotelEmail)

              let instance = new component({
                propsData:{
                  clientEmail: v.customerEmail,
                  hotelEmail: v.hotelEmail,
                }
              });
              instance.$mount();

               this.$swal
                  .fire({
                    title: self.$t("Your reservation was modified"),
                    type: "success",
                    html: "<div></div>",
                    showCancelButton: true,
                    showConfirmButton:false,
                    cancelButtonText: self.$t("Exit"),
                    cancelButtonColor: "#d33",
                    onBeforeOpen: () => {
                      this.$swal
                        .getContent()
                        .querySelector("div")
                        .append(instance.$el);
                    },
                    onClose:() => {
                      window.location.reload();
                    }
                  })
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
              self.reservationId,
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
              // self.result.status = 3;
              // self.result.cancellationNumber = v.cancelNumber;

              let component = Vue.extend(ModificationTemplate);
              console.log(v.customerEmail)
              console.log(v.hotelEmail)

              let instance = new component({
                propsData:{
                  clientEmail: v.customerEmail,
                  hotelEmail: v.hotelEmail,
                }
              });
              instance.$mount();

               this.$swal
                  .fire({
                    title: self.$t("Your reservation was canceled"),
                    type: "success",
                    html: "<div></div>",
                    showCancelButton: true,
                    showConfirmButton:false,
                    cancelButtonText: self.$t("Exit"),
                    cancelButtonColor: "#d33",
                    onBeforeOpen: () => {
                      this.$swal
                        .getContent()
                        .querySelector("div")
                        .append(instance.$el);
                    },
                    onClose:() => {
                      window.location.reload();
                    }
                  })
              // this.$swal.fire(
              //   self.success(self.$t("Your reservation was canceled"))
              // );
            } else
              this.$swal.fire(
                self.error(self.$t("Failed to cancel the reservation"), v.error)
              );
          }
        });
    },
    reactivate(){
      let self = this;
      this.$swal.fire({
          title:self.$t('Reactivate Reservation ?'),
          text:'',
          icon: 'warning',
          confirmButtonText:self.$t('Reactivate'),
           cancelButtonText: self.$t("Cancel"),
          showLoaderOnConfirm: true,
          showCancelButton: true,
          showConfirmButton: true,
          cancelButtonColor: "#d33",
          confirmButtonColor: "#3085d6",
          preConfirm: () => {
            let req = true;
              return ReservationService.ReservationUpdate(
                self.reservationId,
                "reactivate",
                req
              ).then(response => {
                return {
                  response: response.body,
                };
              });
            },
            allowOutsideClick: () => !this.$swal.isLoading(),
        })
        .then(result => {
          console.log(result)
          if (result.value) {
             let v = result.value.response;
             if(v.isSuccess)
             {
                let component = Vue.extend(ModificationTemplate);
                console.log(v.customerEmail)
                console.log(v.hotelEmail)
                let instance = new component({
                  propsData:{
                    clientEmail: v.customerEmail,
                    hotelEmail: v.hotelEmail
                  }
                });
                instance.$mount();
                this.$swal
                  .fire({
                    title: self.$t("Your reservation was reactivated"),
                    type: "success",
                    html: "<div></div>",
                    showCancelButton: true,
                    showConfirmButton:false,
                    cancelButtonText: self.$t("Exit"),
                    cancelButtonColor: "#d33",
                    onBeforeOpen: () => {
                      this.$swal
                        .getContent()
                        .querySelector("div")
                        .append(instance.$el);
                    },
                    onClose:() => {
                      window.location.reload();
                    }
                  })
             }
             else
             {
               this.$swal.fire(
                self.error(self.$t("Failed to reactivate the reservation"), v.error)
              );
             }
          }
        })
    },
    success(title) {
      return {
        type: "success",
        title: title,
        showConfirmButton: false,
        time: 2500,
        onClose: () => {
          location.reload();
        }
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
          title: self.$t("Send email with reservation details?"),
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
                title: self.$t("Email Sent"),
                showConfirmButton: false,
                showCloseButton: true,
                timer: 2500
              });
            } else {
              this.$swal.fire({
                type: "error",
                title: self.$t("Email Not Sent"),
                showConfirmButton: false
              });
            }
          }
        });
    }
  },
  computed: {
    showCancelButton() {

      if(this.isAgencyCompany && this.result.status != 3) {
         return true;
      }
       
      if(!this.isAgencyCompany && this.result.agency) { 
        return false;
      }

      return this.result.allowsCancel && this.result.status != 3;
    },
    showModifyButton() {
      if(this.isAgencyCompany) { 
        return false;
      }

      if(!this.isAgencyCompany && this.result.agency) {
        return false;
      }
      
      return this.result.allowsModify;
    },
    showSendNotificationButton() {
      return this.result.status === 1;
    },
    showReactivateButton(){

      if(this.isAgencyCompany && this.result.status === 3){
        return true;
      }

      if(!this.isAgencyCompany && this.result.agency){
        return false;
      }
      
      if(this.result.allowsReactivate)
      {
        if((this.result.source !== 'ADS' 
        || this.result.source !== 'IDS') && this.result.status === 3)
          return true;
      }

      return false;
    },
    showCorporate() {
      if (this.result.corporateName) {
        return "- " + this.result.corporateName;
      }
      return "";
    }
  }
};
</script>

