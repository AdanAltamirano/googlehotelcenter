<template>
    <div class="">        
        <details class="text-primary">{{$t('')}}</details>
        <b-row>
            <b-col md="4">
                <b-form-group :label="$t('Rate Plans')" class="mt-3">                
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
                        open-direction="bottom"   
                        @input="RemoveWhenItsAll">
                    </multiselect>
                    <b-form-checkbox
                        v-model="enableRatePlans"
                        class="mt-2">
                        {{$t('Include Rates Of RatePlans')}}
                    </b-form-checkbox>
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
        <b-row>
            <b-col md="4">
                <b-form-group :label="$t('Promotions')" class="mt-3">                
                    <multiselect                    
                        id="promotions"                                   
                        v-model="promosList"
                        label='text'                     
                        :options="optionsPromosList"
                        track-by="value"
                        :multiple="true"                                
                        :selectLabel="''"
                        :selectedLabel="''"
                        :deselectLabel="''"
                        :placeholder="$t('Promotions')"
                        open-direction="bottom"   
                        @input="RemoveWhenItsAll">
                    </multiselect>
                    <b-form-checkbox
                        v-model="enablePromotions"
                        class="mt-2">
                        {{$t('Include Rates Promotions')}}
                    </b-form-checkbox>
                </b-form-group>
            </b-col>
        </b-row>
        <div v-if="callApi" class="mt-3 vld-parent" style="height:80px;">
            <loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
        </div>
        <div else class="mt-4">
            <b-button :disabled="!isEnabledGoogle" class="mt-1" v-if="showButton" variant="primary" @click="deleteRates()">
                {{$t('Delete Rates')}}
            </b-button>
        </div>
        <div class="mt-3">
            <label v-if="isEnabledGoogle" style="color:#dc3545;">
                {{$t("This operation make take a few minutes")}}
            </label>
                <label v-else-if="!isEnabledGoogle" style="color:#dc3545;">
                {{$t("Enable Google Prices in Content / General Information")}}
            </label>
        </div>
        <hr class="solid">
    </div>
</template>

<script>
import Loading from "vue-loading-overlay";
import Multiselect from 'vue-multiselect';
import RoomsClosureService from '../../../api/rooms-service';
import ConfluxService from '../../../api/conflux-service';

export default {
    props:{
        hotelId:{

        },
        isEnabledGoogle:{
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
            options:[],
            roomsList:[],
            optionsRooms:[],
            promosList:[],
            optionsPromosList:[],
            enableRatePlans:false,
            enablePromotions:false
        }
    },
    created() {
        this.loadRatesPlans(this.hotelId);
        this.loadRooms(this.hotelId);
        this.loadPromos(this.hotelId);
    },
    mounted(){
        this.ratePlansList.push({
            value : "0",
            text : this.$t('All')
        });

        this.roomsList.push({
            value: "0",
            text: this.$t('All')
        });

        this.promosList.push({
            value: "0",
            text: this.$t('All')
        });

    },
    methods:{
        deleteRates(){

            const ratesPlans = this.ratePlansList.map(obj => obj.value);
            const rooms = this.roomsList.map(obj => parseInt(obj.value, 10));
            const promos = this.promosList.map(obj => obj.value);

            const payload = {
                startDate: this.dates.start,
                endDate: this.dates.end,
                ratePlansList: ratesPlans,
                roomsList: rooms,
                promosList: promos,
                enableRatePlans: this.enableRatePlans,
                enablePromotions : this.enablePromotions
            };

            this.showButton = false;
            this.callApi = true;

            ConfluxService.DeleteRates(this.hotelId, payload)
            .then(response =>{
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.success(this.$t('Rates Eliminated')));
            })
            .catch(error => {
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.error(this.$t('System Error')))
            });
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
            };
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
            RoomsClosureService.getRatePlansByHotelIdSegmentsValids(hotelId)
            .then(response => {
                console.log(response.body);

                this.options.push({
                    value : "0",
                    text : this.$t('All')
                });

                response.body.forEach(rateplan => {
                    this.options.push(rateplan);                
                });
            });
        },
        loadRooms(hotelId){
            RoomsClosureService.getRoomsByHotelId(hotelId)
            .then(response => {
                console.log("Habitaciones");
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
        loadPromos(hotelId){
            RoomsClosureService.getPromosByHotelId(hotelId)
            .then(response => {

                this.optionsPromosList.push({
                    value : "0",
                    text : this.$t('All')
                });

                response.body.forEach(promo => {
                    this.optionsPromosList.push(promo);                
                });
            });
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

