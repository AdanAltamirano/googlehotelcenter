<template>
	<div id="app">
    <b-container  v-if="!load" style="height:400px"></b-container>
		<b-container v-else-if="load" fluid>
			<h3 class="text-primary">{{$t("Room and Rateplan Closure")}}</h3>
			<b-row class="mt-3">
				<!-- Dates -->
				<b-col md="3">
					<b-form-group :label='$t("View Availability")' :description='$t("Date Range")'>
						<b-input-group>
							<v-date-picker
								v-model="dates"
								class="form-control p-0"
								mode="range"
								:min-date="minDate"
								:popover="{placement:'',visibility: 'click' }"
								:columns="2">
							</v-date-picker>

							<b-input-group-append>
								<b-button :disabled="dates == null" variant="danger" @click="dates = null">
									<i class="fa fa-times"></i>
								</b-button>
							</b-input-group-append>
						</b-input-group>
					</b-form-group>
				</b-col>
				<!-- Load Button -->
				<b-col md="4">					
            <b-form-group :label='$t("Rate Plans")' :description='$t("Search by rateplan")'>
              <div class="d-flex">
                <!-- <b-form-select v-model="selectedAvailability" :options="options"></b-form-select> -->                
                <multiselect                                   
                  v-model="selectedAvailability"
                  label='text'                     
                  :options="options"
                  track-by="value"
                  :multiple="true"                                
                  :selectLabel="''"
                  :selectedLabel="''"
                  :deselectLabel="''"
                  :placeholder="$t('Rate Plans')"
                  @input="RemoveWhenItsAll">
                </multiselect>
                <!-- <div>
                  <b-button class="ml-4" variant="primary" @click="loadClosure">{{ $t("Load") }}</b-button>
                </div> -->
              </div>
            </b-form-group>            				
				</b-col>
        <b-col md=1>
          <b-button class="ml-4" variant="primary" style="position:relative; top:30%;" @click="loadClosure">{{ $t("Load") }}</b-button>
        </b-col>
				<!-- Status And Button -->
				<b-col md="4">
					<b-form-group :label='$t("Status")' class="text-end">            
						<div class="status-box">
              <div style="background-color:green;"></div>
              <div>{{$t("Open")}}</div>
              <div style="background-color:red;"></div>
              <div>{{$t("Close")}}</div>
              <div style="background-color:gray;"></div>
              <div>{{$t("No Arrivals")}}</div>              
            </div>			
					</b-form-group>
          <button class="btn btn-link" data-toggle="collapse" data-target="#save-closure" style="position:absolute; right:0;">
            <span>{{$t('Show Configurations')}} <i class="fas fa-cog mr-2"></i></span>
          </button>
				</b-col>
			</b-row>
      <!-- Save Closure Collapse -->
      <b-row>
        <b-col md="12">
          <div id="save-closure" class="collapse">
            <b-card class="mt-3 bg-light">
              <!-- 1 Row -->
              <b-row>
                <!-- Dates -->
                <b-col md="4">
                  <b-form-group :label="$t('Dates For Closure')">
                    <b-input-group>
                      <v-date-picker
                        v-model="datesSave"
                        class="form-control p-0"
                        mode="range"
                        :min-date="new Date()"
                        :popover="{placement:'',visibility: 'click' }"
                        :columns="2"
                        :masks="{input: 'DD/MMM/YYYY'}">
                      </v-date-picker>

                      <!-- <b-input-group-append>
                        <b-button :disabled="dates == null" variant="danger" @click="dates = null">
                          <i class="fa fa-times"></i>
                        </b-button>
                      </b-input-group-append> -->

                      <div class="d-flex ml-2">                          
                        <button type="button" class="btn" @click="addDateToList(datesSave)"><i class="fas fa-calendar-plus fa-lg" style="color:#15cc3f;"></i></button>
                        <button type="button" class="btn" @click="removeDateFromList()"><i class="fas fa-calendar-minus fa-lg" style="color:#f55050;"></i></button>
                      </div>

                    </b-input-group>
                  </b-form-group>

                  <!-- Fechas -->
                  <div v-if="datesList.length > 0" class="list-group max-w-79 h-90 scroll-y">                           
                      <span v-for="(date,index) in datesList" :value="date" :key="index" class="list-group-item">{{getDateFormat(date)}}</span>                        
                  </div>
                  <div v-else class="alert alert-warning" role="alert">
                    {{'Add Dates' | translate}} 
                  </div>
                </b-col>
                <!-- Rateplan -->
                <b-col md="4">
                  <b-form-group :label="$t('Rate Plans')">
                    <!-- <b-form-select v-model="selectedClosure" :options="options"></b-form-select> -->
                    <multiselect
                      id="planes"                                   
                      v-model="ratePlansList"
                      label='text'                     
                      :options="options"
                      track-by="value"
                      :multiple="true"                                
                      :selectLabel="''"
                      :selectedLabel="''"
                      :deselectLabel="''"
                      :placeholder="$t('Rate Plans')"
                      @input="RemoveWhenItsAll">
                    </multiselect>

                  </b-form-group>
                </b-col>
                <!-- Status -->
                <b-col md="4">
                  <b-form-group :label="$t('Status')">                                 
                    <b-form-checkbox value="O" v-model="checkStatus" class="custom-control-inline">{{ $t('Open') }}</b-form-checkbox>
                    <b-form-checkbox value="C" v-model="checkStatus" class="custom-control-inline">{{ $t('Close') }}</b-form-checkbox>
                    <b-form-checkbox value="N" v-model="checkStatus" class="custom-control-inline">{{ $t('No Arrivals') }}</b-form-checkbox>                                     
                  </b-form-group>                  
                </b-col>
              </b-row>
              <!-- 2 Row -->
              <b-row>
                <!-- Rooms -->
                <b-col md="4">
                  <b-form-group :label="$t('Rooms')">
                    <b-form-select v-model="selectedRoom" :options="optionsRooms"></b-form-select>
                  </b-form-group>
                </b-col>
                <b-col md="4">
                  <b-form-group>
                    <b-button variant="primary" class="mt-6" :disabled="datesList.length == 0" @click="saveClosure">{{$t('Save')}}</b-button>
                  </b-form-group>
                </b-col>
              </b-row>
            </b-card>
          </div>
        </b-col>
      </b-row>
      <!--Calendar Ribbon -->
      <div v-show="loadingClosure" class="vld-parent" style="height:200px">
        <loading :active="true" :is-full-page="false" color="#007bff"></loading>
      </div>
      <b-row v-show="showClosure" class="mt-3">
        <b-col md="12">
          <div class="ml-3 mr-3">
            <div class="d-flex bg-white border-top border-bottom border-5 pl-0 pr-0">
              <!-- User Controls Ribbon -->
              <div class="d-flex justify-content-end align-items-center border-right w-30 pr-5">
                <button @click="addDays(-7)" :disabled="disableRightButton" class="btn btn-link btn-sm"><i class="fa fa-angle-double-left"></i></button>
               
                <!-- <v-date-picker
                v-model="dates"
                class=""
                mode="range"
                :min-date="minDate"
                :popover="{placement:'',visibility: 'click' }"
                :columns="2">
                  <a href="javascript:;" class="text-decoration-none h3">{{ dates | moment('MMM D, YYYY')}}</a>
                </v-date-picker> -->
                <span class="font-weight-bold text-uppercase color-primary">{{ datesRibbon}}</span>
                <button @click="addDays(7)" :disabled="disableLeftButton" class="btn btn-link btn-sm"><i class="fa fa-angle-double-right"></i></button>
              </div>
              <!-- Dates Ribbon -->
              <!-- <div class="d-flex w-70 two-weeks">
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
              </div> -->
              <ribbon :startDate="this.startDateRibbon" :endDate="this.endDateRibbon"></ribbon>
            </div>
          </div>
        </b-col>
      </b-row>
      <!-- Rooms And Rate Plans -->
      <b-row v-show="showClosure" class="mt-3">
        <b-col md="12">
          <!-- Main -->
          <closure v-for="(rateRooms,index) in this.closure.rateRoomsClosureModelList" :key="index" 
          :rateRooms="rateRooms" :startIndex="startIndex" :endIndex="endIndex">
          </closure>
        </b-col>
      </b-row>
		</b-container>
	<div>
</template>
<script>
import RoomsClosureService from '../../api/rooms-service';
import Closure from './components/Closure.vue';
import Ribbon from './components/Ribbon.vue';
import Multiselect from 'vue-multiselect';
import Loading from "vue-loading-overlay";
let loader = null;

export default {
  name: "rooms_closure",
  components: {
    Closure,
    Ribbon,
    Loading,
    Multiselect
  },
  data() {
    return {
      //Dates For DatePicker
			dates:null,
      datesSave:null,
			//Selected Rateplan
			selectedAvailability:[],
      selectedClosure: "0",
			//Options Rateplan
      options:[],    
      //Response api when load closure
      closure : [],
      //Status to Save
      checkStatus:'O',
      //Selected Room to Save
      selectedRoom:'0',
      //Options for Rooms
      optionsRooms:[],
      //Start Date Ribbon
      startDateRibbon:null,
      //End Date Ribbon
      endDateRibbon:null,
      //Start Date Availability
      startDateAvailability:null,
      //End Date Availability
      endDateAvailability:null,
      //Right Disable Dates Button
      disableRightButton : false,
      //Left Disable Dates Button
      disableLeftButton : false,
      //Start Index Array Status
      startIndex:0,
      //End Index Array Status
      endIndex:0,
      datesRibbon:null,
      //Main Load
      load:false,
      loadRates:false,
      loadRoomsByHotel:false,
      loadingClosure: false,
      showClosure:false,
      datesList:[],
      ratePlansList:[],

    };
  },
  created() {
    loader = this.$loading.show({
      color: this.$appConfig.themeColors.info,
      height: 128,
      width: 128
    });

    //Initialize Dates For DatePicker
    this.dates = this.defaultDates();
    this.datesSave = this.defaultDates();
    //Get RatePlans By HotelId
    const hotelId =  this.$appConfig.session.hotelId;
    this.loadRatePlans(hotelId);
    this.loadRooms(hotelId);

  },
  mounted() {
    this.selectedAvailability.push({
       value : "0",
       text : this.$t('All')
    });

    this.ratePlansList.push({
       value : "0",
       text : this.$t('All')
    });

    this.loadClosure();
  },
  computed: {
    //Set Minimun Date For DatePicker
    minDate() {
      const date = new Date();
      date.setFullYear(date.getFullYear() - 1);
      return date;
    },
  },
  watch: { 
  },
  methods: {

    RemoveWhenItsAll(array){

      const predicate = (element) => element.value == '0';

      if(array.some(predicate)){

        const totalOfObject = array.length;

        let i = 0;

        while(i <= totalOfObject){
          array.pop();
          i++;
        }

        array.push({
          value : "0",
          text : this.$t('All')
        });

      }
    },

    //Set Default Dates For DatePicker
    defaultDates() {
      const start = new Date();
      const end = new Date();
      end.setDate(end.getDate() + 1);
      return {
        start: start,
        end: end
      };
    },
    //Get Closure
    loadClosure(){

      this.loadingClosure = true;
      this.showClosure = false;

      let startDate = this.$moment(this.dates.start).format('YYYY-MM-DD'); 
      let endDate = this.$moment(this.dates.end).format('YYYY-MM-DD');

      let diff = (this.$moment(this.dates.end).diff(this.$moment(this.dates.start),'days')) + 1;

      //console.log(diff);

      let totalDaysToAdd = 0;

      while((diff % 7) != 0){
        totalDaysToAdd++;     
        diff++;
      }

      endDate =  this.$moment(this.dates.end).add(totalDaysToAdd,'days').format('YYYY-MM-DD');

      //console.log(endDate);
      //console.log(totalDaysToAdd);
     // console.log(diff);

      const hotelId =  this.$appConfig.session.hotelId;
      //const ratePlan = (this.selectedAvailability === '0')? '' : this.selectedAvailability;

      //Array Request
      //const ratePlans = 

      //console.log(startDate);
      //console.log(endDate);
      //console.log(hotelId);
      console.log(this.selectedAvailability);

      let ratePlansRequest = [];

      this.selectedAvailability.forEach(ratePlan => {
        ratePlansRequest.push(ratePlan.value);
      });

      console.log(ratePlansRequest);



      RoomsClosureService.getRoomsClosure(hotelId,startDate,endDate,ratePlansRequest)
      .then(response => {
        this.closure = response.body;
        //console.log(this.closure);

        this.startDateAvailability = new Date(response.body.startDate);
        this.endDateAvailability = new Date(response.body.endDate);

        console.log('Fechas availability')
        console.log(this.startDateAvailability);
        console.log(this.endDateAvailability);

        //Calculate 14 days for ribbon
        this.startDateRibbon = new Date(this.$moment(this.startDateAvailability));
        this.endDateRibbon = new Date(this.$moment(this.startDateRibbon).add(6,'days'));
        //this.endDateRibbon.setDate(this.startDateRibbon.getDate() + 6);
        //this.endDateRibbon.setHours(0,0,0,0);

        console.log('Fechas ribbon')
        console.log(this.startDateRibbon);
        console.log(this.endDateRibbon);


        this.startIndex = 0;
        this.endIndex = 7;

        this.datesRibbon = this.$moment(this.startDateRibbon).format('DD MMM') + ' - ' + this.$moment(this.endDateRibbon).format('DD MMM');
        
        console.log("Ribbon")
        console.log(this.endDateAvailability.getDate());
        console.log(this.endDateRibbon.getDate());

        if(this.$moment(this.endDateAvailability).isSame(this.endDateRibbon)){
          this.disableLeftButton = true;
        }
        else{
          this.disableLeftButton = false;
        }

        if(this.$moment(this.startDateAvailability).isSame(this.startDateRibbon)){
          this.disableRightButton = true;
        }
        else{
          this.disableRightButton = false;
        }

        this.loadingClosure = false;
        this.showClosure = true;

        //console.log(this.startDateAvailability);
        //console.log(this.endDateAvailability);
        //console.log(this.startDateRibbon);
        //console.log(this.endDateRibbon)
        //console.log(this.startDateRibbon.getDate() + 6);
      });
    },
    //Save Closure
    saveClosure(){
      // const startDate = this.$moment(this.datesSave.start).format('YYYY-MM-DD'); 
      // const endDate = this.$moment(this.datesSave.end).format('YYYY-MM-DD');
      const hotelId =  this.$appConfig.session.hotelId;
      const ratePlan = (this.selectedClosure === '0')? '0' : this.selectedClosure;
      const room = (this.selectedRoom === '0')? '0' : this.selectedRoom;
      const status = this.checkStatus;

      // console.log(startDate);
      // console.log(endDate);
      // console.log(hotelId);
      // console.log(ratePlan);
      // console.log(room);
      // console.log(status);

      console.log(this.ratePlansList);



      let datesRequest = [];

      this.datesList.forEach(date => {
        const startDate = this.$moment(date.start).format('YYYY-MM-DD'); 
        const endDate = this.$moment(date.end).format('YYYY-MM-DD');
        datesRequest.push({
          startDate,
          endDate
        });
      });

      let ratesPlansRequest = [];

      this.ratePlansList.forEach(ratePlan => {
        ratesPlansRequest.push({
          code: ratePlan.value,
          name: ratePlan.text
        });
      });

      // Mostrar Alerta de Error
      if(ratesPlansRequest.length == 0){
        
         this.$appAlert({
          type: 'error',
          title: this.$t('Select at least one rate plan'),
          showCloseButton: true,
          showConfirmButton:false,
          showCancelButton:false
        });

      }
      else {

        let request = {
          IdHotel : hotelId,
          Dates : datesRequest,
          RatePlans: ratesPlansRequest,
          RatePlanOption : ratePlan,
          RoomOption : room,
          Status : status
        }

        this.loadingClosure = true;
        this.showClosure = false;

        //Request API
        RoomsClosureService.saveRoomsClosure(hotelId,request)
        .then(response =>{
          console.log(response);

          this.loadClosure();

        },error =>{
          this.loadingClosure = false;
          this.showClosure = true;
          this.$appAlert({
          type: "warning",
          title: this.$t('Couldn\'t Save The Closure'),
          confirmButtonText: this.$t("Exit"),
          confirmButtonColor: "#d33"
          });
        });
      }


    },
    //Get Rateplans By Hotel Id
    loadRatePlans(hotelId){
      RoomsClosureService.getRatePlansByHotelId(hotelId)
      .then(response => {
        console.log(response.body);

        this.options.push({
          value : "0",
          text : this.$t('All')
        });

        response.body.forEach(rateplan => {
          this.options.push(rateplan);
        
        });

        this.loadRates = true;

        console.log(this.loadRates);
        console.log(this.loadRoomsByHotel)
        if(this.loadRates && this.loadRoomsByHotel){
          this.load = true;
          loader.hide();
        }
      });
    },
    //Get Rooms By Hotel Id
    loadRooms(hotelId){
      RoomsClosureService.getRoomsByHotelId(hotelId)
      .then(response => {
        console.log(response.body);

        this.optionsRooms.push({
          value : "0",
          text : this.$t('All')
        });

        response.body.forEach(room => {
          this.optionsRooms.push(room);
        
        });

        this.loadRoomsByHotel = true;
        console.log(this.loadRates);
        console.log(this.loadRoomsByHotel)
        if(this.loadRates && this.loadRoomsByHotel){
          this.load = true;
          loader.hide();
        }

      })
    },
    addDays(days){
      const copyStart = new Date(this.$moment(this.startDateRibbon));
      const copyEnd = new Date(this.$moment(this.endDateRibbon));
      // copyStart.setHours(0,0,0,0);
      // copyEnd.setHours(0,0,0,0);

      this.startDateRibbon = new Date();
      this.endDateRibbon = new Date();

      const newDateStart = this.$moment(copyStart).add(days,'days').toString();
      const newDateEnd = this.$moment(copyEnd).add(days,'days').toString();
      
      this.startDateRibbon = new Date(newDateStart);
      this.endDateRibbon = new Date(newDateEnd);

      // this.startDateRibbon.setDate(copyStart.getDate() + days);
      // this.endDateRibbon.setDate(copyEnd.getDate() + days);
      // this.startDateRibbon.setHours(0,0,0,0);
      // this.endDateRibbon.setHours(0,0,0,0);

      if(this.$moment(this.endDateAvailability).isSame(this.endDateRibbon)){
        this.disableLeftButton = true;
      }
      else{
        this.disableLeftButton = false;
      }

      if(this.$moment(this.startDateAvailability).isSame(this.startDateRibbon)){
        this.disableRightButton = true;
      }
      else{
        this.disableRightButton = false;
      }

      this.startIndex += days;
      this.endIndex += days;

      this.datesRibbon = this.$moment(this.startDateRibbon).format('DD MMM') + ' - ' + this.$moment(this.endDateRibbon).format('DD MMM')

      // console.log(copyStart);
      // console.log(copyEnd);
      // console.log(this.startDateRibbon);
      // console.log(this.endDateRibbon);
      // console.log(this.startDateAvailability);
      // console.log(this.endDateAvailability);

    },
    addDateToList(date){
          //Pendiente que no traslapen las fechas
          //Ver que las fechas no traslapen asi solo se puede agregar a la lista
          this.datesList.push(this.datesSave);

          const overlap = this.overlapDates(this.datesList);
          
          if(overlap.overlap)
          {
              this.$appAlert({
                    type: 'error',
                  title: this.$t('Dates Overlap'),
                  showCloseButton: true,
                  showConfirmButton:false,
                  showCancelButton:false
              });
              
              this.removeDateFromList();
              
          }
      },
      overlapDates(dates){
          var sortedRanges = dates.sort((previous, current) => {  
              // get the start date from previous and current
              var previousTime = previous.start.getTime();
              var currentTime = current.start.getTime();

              // if the previous is earlier than the current
              if (previousTime < currentTime) {
              return -1;
              }

              // if the previous time is the same as the current time
              if (previousTime === currentTime) {
              return 0;
              }

              // if the previous time is later than the current time
              return 1;
          });

          var result = sortedRanges.reduce((result, current, idx, arr) => {
              // get the previous range
              if (idx === 0) { return result; }
              var previous = arr[idx-1];
          
              // check for any overlap
              var previousEnd = previous.end.getTime();
              var currentStart = current.start.getTime();
              var overlap = (previousEnd >= currentStart);
          
              // store the result
              if (overlap) {
                  // yes, there is overlap
                  result.overlap = true;
                  // store the specific ranges that overlap
                  result.ranges.push({
                      previous: previous,
                      current: current
                  })
              }
          
              return result;
          
              // seed the reduce  
          }, {overlap: false, ranges: []});
          return result;
      },
      getDateFormat(date) {
          return `${this.$moment(date.start).format('DD/MMM/YYYY')} - ${this.$moment(date.end).format('DD/MMM/YYYY')}`;
      },
      removeDateFromList(){
          this.datesList.pop();
      },
  },
};
</script>

