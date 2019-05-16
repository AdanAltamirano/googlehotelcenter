<template>
    <div class="p-0" id="app">
        <div class="bg-light d-flex justify-content-between pl-5 pr-5">
        <button data-toggle="collapse" class="btn btn-primary m-2" data-target="#rates">
            <span>Bulk Update</span>
        </button>
        </div>
        <!--rates form-->
        <div class="pb-5">
            <div class="bg-light">
                <calendar-ribbon></calendar-ribbon>
                <div class="bg-light pr-0 pl-0">
                    <room-table v-for="room in roomRates" :key="room.id" :room="room"></room-table>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { EventBus } from './core/event-bus'
import ApiService from './api/api-service'
import CalendarRibbon from './components/CalendarRibbon' 
import RoomTable from './components/RoomTable' 
import Utilities from './core/utilities'

export default {
  name: 'app',
  components: {
      CalendarRibbon,
      RoomTable
  },
  created(){
      //check for last work day
      this.dateRange.start = Utilities.getLastWorkDay();
      this.dateRange.end = this.dateRange.start.clone().add(13, 'days');

      EventBus.$on('api.call.begin', this.showLoader);
      EventBus.$on('api.call.end', this.hideLoader);
  },
  beforeMount(){
      let self = this;
      EventBus.$emit('loading');
      ApiService.roomsWithRates(
          this.$appConfig.session.hotelId,
          this.dateRange.start.format('YYYY-MM-DD'),
          this.dateRange.end.format('YYYY-MM-DD'),
          this.$appConfig.language)
        .then((response) => {
            self.roomsCatalog = response.rooms;
            self.roomRates = response.mixin;
        });
  },
  data(){
      return {
          loader: null,
          dateRange: {
              start: '',
              end: ''
          },
          roomsCatalog:[],
          roomRates:[]
      } 
  },
  methods:{
      showLoader(){
          this.loader = this.$loading.show({color: '#007bff', height: 128, width: 128});
      },
      hideLoader(){
          this.loader.hide();
      },
      
  }
};
</script>
