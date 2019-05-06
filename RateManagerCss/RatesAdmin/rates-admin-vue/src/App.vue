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
            </div>
        </div>
    </div>  
</template>

<script>
import { EventBus } from './core/event-bus'
import ApiService from './api/api-service'
import CalendarRibbon from './components/CalendarRibbon' 
import Utilities from './core/utilities'

export default {
  name: 'app',
  components: {
      CalendarRibbon,
  },
  created(){
      //check for last work day
      this.dateRange.start = Utilities.getLastWorkDay();
      this.dateRange.end = this.dateRange.start.clone().add(13, 'days');

      EventBus.$on('loading', this.showLoader);
      EventBus.$on('loadingDone', this.hideLoader);
  },
  beforeMount(){
      let self = this;
      EventBus.$emit('loading');
      let rooms_req = ApiService.rooms(this.$appConfig.session.hotelId, this.$appConfig.language);
      let rates_req = ApiService.rates(
          this.$appConfig.session.hotelId, 
          this.dateRange.start.format('YYYY-MM-DD'), 
          this.dateRange.end.format('YYYY-MM-DD'), 
          this.$appConfig.language) 
      
      Promise.all([rooms_req, rates_req])
      .then(([rooms_res, rates_res]) => {
        self.roomCatalog = rooms_res;
        self.roomRates = rates_res.body;
        console.log(rooms_res.body);
        EventBus.$emit('loadingDone');
      })
      .catch((reason)=> {
          console.log(reason);
        EventBus.$emit('loadingDone');
      });
  },
  data(){
      return {
          loader: null,
          dateRange: {
              start: '',
              end: ''
          },
          roomCatalog:[],
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
