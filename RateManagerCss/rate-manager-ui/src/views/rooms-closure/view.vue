<template>
	<div id="app">
		<b-container fluid>
			<h3 class="text-primary">{{$t("Room and Rateplan Closure")}}</h3>
			<b-row class="mt-3">
				<!-- Dates -->
				<b-col md="4">
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
							<b-form-select v-model="selected" :options="options"></b-form-select>
							<b-button class="ml-4" variant="primary" @click="loadClosure">{{ $t("Load") }}</b-button>
						</div>
					</b-form-group>				
				</b-col>
				<!-- Status -->
				<b-col md="4">
					<b-form-group :label='$t("Status")' class="text-end">
            <button data-toggle="collapse" class="btn btn-link mr-4" data-target="">
              <span>{{$t('Save')}} <i class="fa fa-save mr-2"></i></span>
            </button>
						<div class="status-box">
              <div style="background-color:green;"></div>
              <div>{{$t("Open")}}</div>
              <div style="background-color:red;"></div>
              <div>{{$t("Close")}}</div>
              <div style="background-color:gray;"></div>
              <div>{{$t("No Arrivals")}}</div>              
            </div>			
					</b-form-group>
				</b-col>
			</b-row>
      <!--Calendar Ribbon -->
      <b-row class="mt-3">
        <b-col md="12">
          <div class="ml-3 mr-3">
            <div class="d-flex bg-white border-top border-bottom border-5 pl-0 pr-0">
              <div class="d-flex justify-content-end align-items-center border-right w-30 pr-5">
                <button class="btn btn-link btn-sm"><i class="fa fa-angle-double-left"></i></button>
                <button class="btn btn-link btn-sm"><i class="fa fa-angle-left"></i></button>
                <v-date-picker
                v-model="dates"
                class=""
                mode="range"
                :min-date="minDate"
                :popover="{placement:'',visibility: 'click' }"
                :columns="2">
                  <a href="javascript:;" class="text-decoration-none h3">{{ dates | moment('MMM D, YYYY')}}</a>
                </v-date-picker>
                <button class="btn btn-link btn-sm"><i class="fa fa-angle-right"></i></button>
                <button class="btn btn-link btn-sm"><i class="fa fa-angle-double-right"></i></button>
              </div>
              <div class="d-flex w-70 two-weeks">
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                  <span>mie.</span>
                  <h3 class="font-weight-bold mt-0 mb-0">02</h3>
                  <span class="text-uppercase">Feb</span>
                </div>
              </div>
            </div>
          </div>
        </b-col>
      </b-row>
      <!-- Rooms And Rate Plans -->
      <b-row class="mt-3">
        <b-col md="12">
          <!-- Main -->
          <closure v-for="(rateRooms,index) in this.closure.rateRoomsClosureModelList" :key="index" :rateRooms="rateRooms"></closure>
        </b-col>
      </b-row>
		</b-container>
	<div>
</template>
<script>
import RoomsClosureService from '../../api/rooms-service';
import Closure from './components/Closure.vue';
export default {
  name: "rooms_closure",
  components: {
    Closure
  },
  data() {
    return {
      //Dates For DatePicker
			dates:null,
			//Selected Rateplan
			selected:"0",
			//Options Rateplan
			options:[
				{value:"0",text:"Todos"},
				{value:"RAC",text:"Convenio A"},
				{value:"EPB",text:"Tarifa Convenio"},
				{value:"BASE",text:"Base Name"},
				{value:"RACE",text:"Convenio B"},
				{value:"CNVA",text:"Acuerdo Corporativo"},
      ],
      //Response api when load closure
      closure : []

    };
  },
  created() {
    //Initialize Dates For DatePicker
    this.dates = this.defaultDates();
  },
  mounted() {},
  computed: {
    //Set Minimun Date For DatePicker
    minDate() {
      const date = new Date();
      date.setFullYear(date.getFullYear() - 1);
      return date;
    },
  },
  watch: {},
  methods: {
    //Set Default Dates For DatePicker
    defaultDates() {
      const start = new Date();
      start.setMonth(start.getMonth() - 1);
      return {
        start: start,
        end: new Date(),
      };
    },
    //Get Closure
    loadClosure(){
      const startDate = this.$moment(this.dates.start).format('YYYY-MM-DD'); 
      const endDate = this.$moment(this.dates.end).format('YYYY-MM-DD');;
      const hotelId =  this.$appConfig.session.hotelId;
      const ratePlan = (this.selected === '0')? '' : this.selected;

      console.log(startDate);
      console.log(endDate);
      console.log(hotelId);
      console.log(ratePlan);

      RoomsClosureService.getRoomsClosure(hotelId,startDate,endDate,ratePlan)
      .then(response => {
        this.closure = response.body;
        console.log(this.closure);
      });
    }
  },
};
</script>

