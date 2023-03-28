<template>
  <div class="d-flex">
    <h4 class="text-info" style="color:#10467a !important;">
      <i class="fas fa-hotel fa-sm"></i>
      {{result.hotelName}} {{showCorporate}}
    </h4>
    <b-button-toolbar v-if="showCancelButton || showModifyButton || showReactivateButton || showPrintButton" class="ml-auto">
      <b-dropdown class="mx-1" right variant="primary" :text="$t('Options')">
        <b-dropdown-item v-if="showModifyButton" @click="modify">{{$t('Modify')}}</b-dropdown-item>
        <b-dropdown-item v-if="showCancelButton" @click="cancel">{{$t('Cancel')}}</b-dropdown-item>
        <b-dropdown-item v-if="showReactivateButton" @click="reactivate">{{$t('Reactivate')}}</b-dropdown-item>
        <!-- <b-dropdown-item v-if="showSendNotificationButton" @click="sendNotification">{{$t('Send confimation email')}}</b-dropdown-item> -->
        <b-dropdown-item @click="print">{{$t('Print')}}</b-dropdown-item>
      </b-dropdown>
    </b-button-toolbar>
  </div>
</template>
<script>
import ReservationService from "../../../api/reservation-service";
import Vue from "vue";
import Modify from "./Modify.vue";
import ModificationForm from "../helper/modificationForm";
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
      showPrintButton:true
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
          nights: this.result.nights,
          total: this.result.totalDetails.total,
          totalNR: this.result.totalDetails.totalNR,
          ecotasa: this.result.totalDetails.ecotasa,
          showTotalNR: this.result.isNetRateUV,
          roomDetails:this.result.roomDetails,
          details : ''
        }
      });
      instance.$mount();

      let self = this;
      this.$swal
        .fire({
          customClass:{
            container: 'swal2-container-custom',
            popup:'swal2-custom-popup',
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

            const _checkIn = self.getElement("checkin", true).split('/').map(number => { return parseInt(number); }); 
            const _checkOut = self.getElement("checkout", true).split('/').map(number => { return parseInt(number); });

            let [dayCheckIn, monthCheckIn, yearCheckIn] = _checkIn;
            let [dayCheckOut, monthCheckOut, yearCheckOut] = _checkOut;

            const form = new ModificationForm( new Date(yearCheckIn, monthCheckIn - 1, dayCheckIn),
                                               new Date(yearCheckOut, monthCheckOut - 1, dayCheckOut), 
                                               instance.roomsDetails, instance.statesChangesRoomsRates);

            console.log(instance.roomDetails);
            
            const error = form.validate();
            console.log(error);

            if(error.hasErrors)
            {
              return {
                value:true,
                response:{
                  isSuccess:false,
                  modifyTags:req,
                  error:error
                }
              }
            }

            const roomsDetails = form.getRoomsDetails();


            let req = {
              name: self.getElement("name"),
              lastName: self.getElement("lastname"),
              checkIn: self.dateFormat(self.getElement("checkin", true)),
              checkOut: self.dateFormat(self.getElement("checkout", true)),
              total: parseFloat(self.getElement("total")),
              totalNR: parseFloat(self.getElement("totalnr")),
              roomsDetails: roomsDetails,
              details: self.getElement("details"),

            };

            return ReservationService.ReservationUpdate(
              self.reservationId,
              "modify",
              req,
              instance.status
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

            if (v.isSuccess) {
             
              if(!v.sendNotification)
              {
                this.$swal.fire(this.success(self.$t("Your reservation was modified")));
              }
              else 
              {

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

                this.$swal.fire(this.successEmails(self.$t("Your reservation was modified"), instance));

              }

            } else {

              let list = '';

              if(v.error.hasErrors) {
                  v.error.errors.forEach(x => {
                      list += `<div class="list-group-item border-0 p-1">- ${x}</div>`;
                  });
              }

              this.$swal.fire(
                self.errorList(self.$t("Failed to modify the reservation"),list)
              );
            }

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

              let component = Vue.extend(ModificationTemplate);

              let instance = new component({
                propsData:{
                  clientEmail: v.customerEmail,
                  hotelEmail: v.hotelEmail,
                }
              });

              instance.$mount();

              this.$swal.fire(this.successEmails(self.$t("Your reservation was canceled"), instance));

            } else {
              this.$swal.fire(
                self.error(self.$t("Failed to cancel the reservation"), v.error)
              );
            }
          }
        });
    },
    reactivate() {
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

          if (result.value) {
             let v = result.value.response;

             if(v.isSuccess)
             {
                let component = Vue.extend(ModificationTemplate);

                let instance = new component({
                  propsData:{
                    clientEmail: v.customerEmail,
                    hotelEmail: v.hotelEmail
                  }
                });

                instance.$mount();

                this.$swal.fire(this.successEmails(self.$t("Your reservation was reactivated"), instance));
             }
             else
             {

                let list = '';

                if(v.error.hasErrors) {
                    v.error.errors.forEach(x => {
                        list += `<div class="list-group-item border-0 p-1">- ${x}</div>`;
                    });
                }

                this.$swal.fire(
                  self.errorList(self.$t("Failed to modify the reservation"),list)
                );

             }
          }
        })
    },
    success(title) {
      return {
        type: "success",
        title: title,
        showCancelButton: true,
        showConfirmButton:false,
        cancelButtonText: this.$t("Exit"),
        cancelButtonColor: "#d33",
        showConfirmButton: false,
        time: 2500,
        onClose: () => {
          window.location.reload();
        }
      };
    },
    successEmails(title, instance) {
      return {
          title: title,
          type: "success",
          html: "<div></div>",
          showCancelButton: true,
          showConfirmButton:false,
          cancelButtonText: this.$t("Exit"),
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
      }
    },
    error(title, message) {
      return {
        type: "error",
        title: title,
        text: message,
        showConfirmButton: false
      };
    },
    errorList(title,list) {
      return {
        type: "error",
        title: title,
        html: `
          <div class="list-group">${list}</div>
        `,
        showCloseButton: true,
        showConfirmButton: false
      }
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
          title: self.$t("Send confimation email?"),
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
                showCancelButton: true,
                cancelButtonText: self.$t("Close"),
                cancelButtonColor: "#d33",
                timer: 2500
              });
            } else {
              this.$swal.fire({
                type: "error",
                title: self.$t("Email Not Sent"),
                showCloseButton: true,
                showCancelButton: true,
                cancelButtonText: self.$t("Close"),
                cancelButtonColor: "#d33",
                showConfirmButton: false
              });
            }
          }
        });
    },
    print(){
      this.$children[0].$children[0].hideMenu();
      setTimeout(() => {
        window.print();
      },300);
    }
  },
  computed: {
    showCancelButton() {

      // if(this.isAgencyCompany && this.result.status != 3) {
      //    return true;
      // }
       
      // if(!this.isAgencyCompany && this.result.agency) { 
      //   return false;
      // }

      return this.result.allowsCancel && this.result.status != 3;
    },
    showModifyButton() {
      // if(this.isAgencyCompany) { 
      //   return false;
      // }

      // if(!this.isAgencyCompany && this.result.agency) {
      //   return false;
      // }
      
      return this.result.allowsModify;
    },
    showSendNotificationButton() {
      return this.result.status === 1;
    },
    showReactivateButton(){

      // if(this.isAgencyCompany && this.result.status === 3){
      //   return true;
      // }

      // if(!this.isAgencyCompany && this.result.agency){
      //   return false;
      // }
      
      // if(this.result.allowsReactivate)
      // {
      //   if((this.result.source !== 'ADS' 
      //   || this.result.source !== 'IDS') && this.result.status === 3)
      //     return true;
      // }

      return this.result.allowsReactivate;
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

