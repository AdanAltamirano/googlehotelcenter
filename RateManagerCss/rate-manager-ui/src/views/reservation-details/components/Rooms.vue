<template>
  <div>
    <h5 class="text-info" style="cursor:pointer" v-b-toggle.roomCollapse>
      <i class="fa fa-plus-circle"></i>
      {{$t('Rooms')}}
    </h5>
    <b-collapse visible id="roomCollapse">
      <div v-for="room in rooms" :key="room" class="card card-accent-primary mt-2">
        <div class="card-header">
          <h6>{{room.roomCode}} - {{room.name}}</h6>
          <small v-if="room.checkIn && room.checkOut">
            {{$t('Check in')}}: {{$moment(room.checkIn).format('D MMM YYYY')}} /
            {{$t('Check out')}}: {{$moment(room.checkOut).format('D MMM YYYY')}}
          </small>
        </div>
        <div class="card-body">
          <b-row>
            <b-col md="4">
              <b-img
                style="height:120px;"
                :src="room.img"
                @error="(e) => {e.target.src=DefaultImage}"
                fluid-grow
              ></b-img>
            </b-col>
            <b-col md="8">
              <table v-if="RestrictionsHotels && !isSupervisor" class="table table-sm text-center">
                <thead>
                  <tr>
                    <th>{{$t('Date')}}</th>
                    <th>{{$t('Price per night')}}</th>
                    <th>{{$t('Price per night extre person(s)')}}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="price in room.priceDetails" :key="price">
                    <td>
                      {{$moment(price.checkIn).format('D MMM')}}
                      <span
                        v-if="price.checkOut != price.checkIn"
                      >- {{$moment(price.checkOut).format('D MMM')}}</span>
                    </td>
                    <td>{{(price.price) | currency}} {{price.currency}}</td>
                    <td>{{(price.extraPrice) | currency}} {{price.currency}}</td>
                  </tr>
                  <tr>
                    <td>Total</td>
                    <td>{{room.total | currency}} {{room.currency}}</td>
                    <td></td>
                  </tr>
                </tbody>
              </table>
              <table v-else class="table table-sm text-center">
                <thead>
                  <tr>
                    <th>{{$t('Date')}}</th>
                    <th>{{$t('Price per night')}}</th>
                    <th>{{$t('Price per night extre person(s)')}}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="price in room.priceDetails" :key="price">
                    <td>
                      {{$moment(price.checkIn).format('D MMM')}}
                      <span
                        v-if="price.checkOut != price.checkIn"
                      >- {{$moment(price.checkOut).format('D MMM')}}</span>
                    </td>
                    <td v-if="isNetRate && !isSupervisor">{{(price.priceNR) | currency}} {{price.currency}}</td>
                    <td v-else>{{(price.price) | currency}} {{price.currency}}</td>

                    <td v-if="isNetRate && !isSupervisor">{{(price.extraPriceNR) | currency}} {{price.currency}}</td>
                    <td v-else>{{(price.extraPrice) | currency}} {{price.currency}}</td>
                  </tr>
                  <tr>
                    <td>Total</td>
                    <td v-if="isNetRate && !isSupervisor">{{room.totalNR | currency}} {{room.currency}}</td>
                    <td v-else>{{room.total | currency}} {{room.currency}}</td>
                    <td></td>
                  </tr>
                </tbody>
              </table>
            </b-col>
          </b-row>
          <b-row class="mt-2">
            <b-col>
              <address>
                <span v-if="room.customerName != '' && room.customerLastName != ''">
                  {{$t('Guest name')}}:
                  <strong>{{room.customerName}} {{room.customerLastName}}</strong>
                  <br />
                </span>
                {{$t('Occupation')}}:
                <strong>
                  {{room.adults}} {{$t('Adult(s)')}}
                  <span
                    v-if="room.childrens > 0"
                  >, {{room.childrens}} {{$t('Children')}}</span>
                </strong>
                <br>
                <span v-if="room.extraAdults > 0">
                  {{$t('Extra Occupation')}}:
                  <strong>{{room.extraAdults}}  {{$t('Adult(s)')}}</strong>
                  <br />
                </span>
                {{$t('Rate plan')}}:
                <strong>{{room.rateCode != '' ? room.rateCode : ratePlan}} - {{room.ratePlan}}</strong>
                <br />
                <template v-if="room.ratePlanPromotion && room.namePromotion">
                  {{$t('Promotion')}}:
                  <strong>{{room.ratePlanPromotion}} - {{room.namePromotion}}</strong>
                  <br />
                </template>
                {{$t('Preferences')}}:
                <div class="alert alert-info">
                  <strong>{{room.preferences}}</strong>
                </div>
              </address>
            </b-col>
          </b-row>
        </div>
      </div>
    </b-collapse>
  </div>
</template>
<script>
import image from "../assets/hotel_placeholder.jpg";
import listHotels from "../../../json/hotels.json";
export default {
  props: {
    rooms: {
      required: true,
      type: Array
    },
    ratePlan: {
      required: false,
      type: String
    },
    isNetRate: {
      required: false
    }
  },
  data() {
    return {
      hotelId : this.$appConfig.session.hotelId,
      isSupervisor: (this.$appConfig.session.isSupervisor === 'True')? true : false,
      hotelsJson: listHotels.hotels,
      image: image,
    };
  },
  computed: {
    DefaultImage() {
      return this.image;
    },
    RestrictionsHotels() {
      return this.hotelsJson.some(hotelJson => {
        return hotelJson.id === this.hotelId
      });
    }
  }
};
</script>

