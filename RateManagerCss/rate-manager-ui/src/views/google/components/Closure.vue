<template>
    <div class="">        
        <details class="text-primary">{{$t('This action will only send the closings to Google Hotel Center from the selected dates')}}</details>
        <b-row>
            <b-col md="4">
                <b-form-group :label="$t('Rate Plans')" class="mt-3">
                    <multiselect
                        id="planes"
                        v-model="ratePlansList"
                        label='text'
                        group-label="group"
                        group-values="plans"
                        :group-select="false"
                        :options="groupedOptions"
                        track-by="value"
                        :multiple="true"
                        :selectLabel="''"
                        :selectedLabel="''"
                        :deselectLabel="''"
                        :placeholder="$t('Rate Plans')"
                        open-direction="bottom"
                        @input="RemoveWhenItsAll">
                    </multiselect>
                </b-form-group>
            </b-col>
            <b-col md="4">
                <b-form-group :label="$t('Rooms')" class="mt-3">                
                    <multiselect                    
                        id="planes"                                   
                        v-model="roomsList"
                        label='text'                     
                        :options="optionsRooms"
                        track-by="value"
                        :multiple="true"                                
                        :selectLabel="''"
                        :selectedLabel="''"
                        :deselectLabel="''"
                        :placeholder="$t('Rooms')"
                        open-direction="bottom"   
                        @input="RemoveWhenItsAll">
                    </multiselect>
                </b-form-group>
            </b-col>
        </b-row>
        <div v-if="callApi" class="mt-3 vld-parent" style="height:80px;">
            <loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
        </div>
        <div else class="mt-4">
            <b-button :disabled="!isEnabledGoogle && !isEnabledAPICache" class="mt-1" v-if="showButton" variant="primary" @click="updateRestrictions()">
                {{$t('Update Closures')}}
            </b-button>
        </div>
        <div class="mt-3">
            <label v-if="isEnabledGoogle || isEnabledAPICache" style="color:#dc3545;">
                {{$t("This operation make take a few minutes")}}
            </label>
                <label v-else-if="!isEnabledGoogle && !isEnabledAPICache" style="color:#dc3545;">
                {{$t("Enable Google Prices Or Rates APICache in Content / General Information")}}
            </label>
        </div>
        <hr class="solid">
    </div>
</template>

<script>
import Vue from "vue";
import Loading from "vue-loading-overlay";
import Multiselect from 'vue-multiselect';
import RestrictionAlert from "./RestrictionsAlert.vue";
import RoomsClosureService from '../../../api/rooms-service';
import ConfluxService from '../../../api/conflux-service';

export default {
    props:{
        hotelId:{

        },
        isEnabledGoogle:{
            type:Boolean
        },
        isEnabledAPICache:{
            type:Boolean
        },
        dates:{
            
        }
    },
    components:{
        Loading,
        Multiselect
    },
    data(){
        return{
            callApi: false,
            showButton: true,
            ratePlansList:[],
            groupedOptions:[],
            options:[],
            roomsList:[],
            optionsRooms:[]
        }
    },
    created() {
        this.loadRatesPlans(this.hotelId);
        this.loadRooms(this.hotelId);
    },
    mounted(){
        this.ratePlansList.push({
            value : "0",
            text : this.$t('All')
        });

        this.roomsList.push({
            value: "0",
            text: this.$t('All')
        })
    },
    methods:{
        updateRestrictions(){

            const ratesPlans = this.ratePlansList.map(obj => obj.value);
            const rooms = this.roomsList.map(obj => parseInt(obj.value, 10));

            const payload = {
                startDate: this.dates.start,
                endDate: this.dates.end,
                ratePlansList: ratesPlans,
                roomsList: rooms
            };

            this.showButton = false;
            this.callApi = true;

            ConfluxService.UpdateRestrictions(this.hotelId,payload)
            .then(response =>{

                let component = Vue.extend(RestrictionAlert);
                let instance = new component({
                    propsData:{
                        restrictions: response.body.restrictions
                    }
                });

                instance.$mount();
                let html = $("<div>").append(instance.$el);
                console.log(html);
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.successHTML(this.$t('Closures'),html));

            })
            .catch(error => {
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.error(this.$t('System Error')))
            });
        },
        successHTML(title, html) {
            return {
                title: title,
                type: "success",
                html: html,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
            }
        },
        error(title) {
            return {
                type: "error",
                title: title,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
                showConfirmButton: false,
                time: 2500,               
            };
        },
        loadRatesPlans(hotelId){
            Promise.all([
                RoomsClosureService.getRatePlansByHotelIdSegmentsValids(hotelId),
                RoomsClosureService.getAllRatePlansByHotelIdSegmentsValids(hotelId)
            ]).then(([activeRes, allRes]) => {
                const activeIds = new Set(activeRes.body.map(p => p.value));

                const activePlans = [
                    { value: "0", text: this.$t('All') },
                    ...activeRes.body
                ];

                const inactivePlans = allRes.body.filter(p => !activeIds.has(p.value));

                this.groupedOptions = [
                    { group: this.$t('Active Rate Plans'), plans: activePlans }
                ];

                if (inactivePlans.length > 0) {
                    this.groupedOptions.push(
                        { group: this.$t('Inactive Rate Plans'), plans: inactivePlans }
                    );
                }

                this.options = activePlans;
            });
        },
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
            })
        },
        RemoveWhenItsAll(array){

            const predicate = (element) => element.value == '0';

            if(array.some(predicate)){

                const totalOfObject = array.length;

                let i = 0;

                while(i <= totalOfObject)
                {
                    array.pop();
                    i++;
                }

                array.push({
                    value : "0",
                    text : this.$t('All')
                });

            }
        },       
    }
}
</script>

