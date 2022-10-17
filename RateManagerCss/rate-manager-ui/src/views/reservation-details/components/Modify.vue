<template>
  <form>
    <div class="form-row">
      <div class="form-group col-md-6">
        <label>{{$t('Name')}}</label>
        <b-form-input id="name" v-model="name"></b-form-input>
      </div>
      <div class="form-group col-md-6">
        <label>{{$t('Last names')}}</label>
        <b-form-input id="lastname" v-model="lastName"></b-form-input>
      </div>
    </div>
    <div class="form-row">
      <div :class="col" class="form-group">
        <label>{{$t('Check in')}}</label>
        <v-date-picker
          v-model="checksIn"
          id="checkin"
          class="form-control p-0 vdp-vc-input"
          :class="{'w-70 m-auto' : !isNetRate}"
          :popover="{ placement: 'bottom', visibility: 'click' }"
        ></v-date-picker>
      </div>
      <div :class="col" class="form-group">
        <label>{{$t('Check out')}}</label>
        <v-date-picker
          v-model="checksOut"
          id="checkout"
          class="form-control p-0 vdp-vc-input"
          :class="{'w-70 m-auto' : !isNetRate}"
          :popover="{ placement: 'bottom', visibility: 'click' }"
        ></v-date-picker>
      </div>
      <div v-show="showTotalNR" class="form-group col-md-3">
        <label>TotalNR</label>
        <b-form-input id="totalnr" class="text-right" v-model="totalNR" :disabled="true"></b-form-input>
      </div>
      <div :class="col" class="form-group">
        <label>Total</label>
        <b-form-input id="total" class="text-right" v-model="total" :class="{'w-70 m-auto' : !isNetRate}" :disabled="true"></b-form-input>
      </div>
      <!-- Tarifas -->
      <div v-if="roomDetails.length > 0" class="form-group col-md-12" id="roomsAccordion">
        <div v-for="(room,index) in roomsDetails" :key="index" class="card card-accent-primary">
         <!-- header -->
          <div class="card-header" :id="'heading' + index">
            <h2 class="mb-0">
              <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" :data-target="'#collapse' + index" aria-expanded="true" :aria-controls="'collapse' + index">
                {{room.name}}
              </button>
            </h2>
          </div>
         <!-- body -->
          <div :id="'collapse' + index" class="collapse" :aria-labelledby="'heading' + index" data-parent="#roomsAccordion">
            <div class="card-body">
              <collapse-content 
                :checkIn="checksIn" 
                :checkOut="checksOut" 
                :room="room" 
                :index="index"
                :isNetRate="isNetRate"
                @updateRoom="updateRoom"
                @updatePriceRoom="updatePriceRoom"
                :key="reload"/>
            </div>
          </div>
        </div>
      </div>
      <!-- -->
      <div class="col-md-12 form-group">
        <label>Detalles</label>
        <b-textarea id="details" v-model="details" :trim="true"></b-textarea>
      </div>
      <div>
        <b-form-checkbox
          id="chkNotification"
          v-model="status"
          value="1"
          unchecked-value="0">
          {{$t('Send notification email')}}
        </b-form-checkbox>
      </div>
    </div>
  </form>
</template>
<script>
import { cloneDeep } from '../../../../node_modules/lodash';
import { numberTwoDecimal } from '../utilities/math/math';
import CollapseContent from './Collapse/Body/Content.vue';

export default {
  props: [
    "name",
    "lastName",
    "total",
    "totalNR",
    "checkIn",
    "checkOut",
    "showTotalNR",
    "roomDetails",
    "details"
  ],
  components: {
    CollapseContent
  },
  computed: {
    // _checkIn() {
    //   return new Date(this.checkIn);
    // },
    // _checkOut() {
    //   return new Date(this.checkOut);
    // },
    col() {
      return {
        "col-md-4": !this.showTotalNR,
        "col-md-3": this.showTotalNR,
      };
    },
  },
  data() {
    return {
      isNetRate:false,
      roomsDetails: null,
      checksIn: new Date(this.checkIn),
      checksOut: new Date(this.checkOut),
      isModifiedDate:false,
      statesChangesRoomsRates:[],
      //Reload
      reload:0,
      //
      status: "1"
    }
  },
  created() {
    this.isNetRate = this.showTotalNR;

    this.roomsDetails = cloneDeep(this.roomDetails);

    this.roomsDetails.forEach(element => {

      element.priceDetails.forEach(price =>{
        price.checkIn = new Date(price.checkIn);
        price.checkOut = new Date(price.checkOut);
      })

      this.statesChangesRoomsRates.push(false);
    });
  },
  watch: {
    checksIn: function() {
      console.log('Checks In');
      this.reload++;
    },
    checksOut: function() {
      console.log('Checks Out');
      this.reload++;
    }
  },
  methods: {
    calculateTotal() {
      let totalPricePerRooms = 0;

      for(const room of this.roomsDetails){
        for(const priceDetail of room.priceDetails){               
          const diffTime = priceDetail.checkOut.getTime() - priceDetail.checkIn.getTime();
          const nights = (diffTime /  (1000 * 3600 * 24)) + 1;
          const priceRoom = numberTwoDecimal(priceDetail.price) + numberTwoDecimal(priceDetail.extraPrice);
          totalPricePerRooms += numberTwoDecimal((priceRoom * nights));
        }
      }

      this.total = numberTwoDecimal(totalPricePerRooms);
    },
    calculateTotalNR() {
      let totalPriceNRPerRooms = 0;

      for(const room of this.roomsDetails){
        for(const priceDetail of room.priceDetails){               
          const diffTime = priceDetail.checkOut.getTime() - priceDetail.checkIn.getTime();
          const nights = (diffTime /  (1000 * 3600 * 24)) + 1;
          const priceRoom = numberTwoDecimal(priceDetail.priceNR) + numberTwoDecimal(priceDetail.extraPriceNR);
          totalPriceNRPerRooms += numberTwoDecimal((priceRoom * nights));
        }
      }

      this.totalNR = numberTwoDecimal(totalPriceNRPerRooms);
    },
    updateRoom(roomIndex, roomVM) {
      
      ({ adults: this.roomsDetails[roomIndex].adults, extraAdults: this.roomsDetails[roomIndex].extraAdults,
         childrens: this.roomsDetails[roomIndex].childrens, extraChildrens: this.roomsDetails[roomIndex].extraChildrens, 
         ageChildren: this.roomsDetails[roomIndex].ageChildren} = roomVM)
    
    },
    updatePriceRoom(roomIndex, priceDetails) {

      this.roomsDetails[roomIndex].priceDetails = [];
      this.roomsDetails[roomIndex].priceDetails = priceDetails;

      this.calculateTotal();
      if(this.isNetRate) this.calculateTotalNR();
      this.updateStateRoomsRates(roomIndex);
    },
    updateStateRoomsRates(index) {
      this.$set(this.statesChangesRoomsRates, index, true);
    },

  }
};
</script>
