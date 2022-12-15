<template>
  <div>
    <!-- First Row -->
    <div class="row">
        <div :class="col_size_4_3">
            <label>{{$t('Adult(s)')}}</label>
            <div>
                <b-form-input v-model="adults" type="number" class="text-right" :class="{'w-70 m-auto' : !isNetRate}" @blur="emitEventUpdateRoom"></b-form-input>
            </div>
        </div>
        <div :class="col_size_4_3">
            <label>{{$t('Children')}}</label>
            <div>
                <b-form-input v-model="childrens" type="number" class="text-right" :class="{'w-70 m-auto' : !isNetRate}" @blur="emitEventUpdateRoom"></b-form-input>
            </div>
        </div>
         <div v-if="isNetRate" class="col-md-3">
            <label>Total NR</label>
            <div>
                <b-form-input v-model="totalNR" class="text-right" :disabled="true"></b-form-input>
            </div>
        </div>
        <div :class="col_size_4_3">
            <label>Total</label>
            <div>
                <b-form-input v-model="total" class="text-right" :class="{'w-70 m-auto' : !isNetRate}" :disabled="true"></b-form-input>
            </div>
        </div>
    </div>
    <!-- Second Row -->
    <div class="row">
        <div class="col-md-4">
            <label>{{$t('Extra Adult(s)')}}</label>
            <div>
                <b-form-input v-model="extraAdults" type="number" class="w-70 m-auto text-right"  @blur="emitEventUpdateRoom"></b-form-input>
            </div>
        </div>
        <div class="col-md-4">
            <label>{{$t('Extra Children')}}</label>
            <div>
                <b-form-input v-model="extraChildrens" type="number" class="w-70 m-auto text-right" @blur="emitEventUpdateRoom"></b-form-input>
            </div>
        </div>
        <div class="col-md-4">
            <label>{{$t('Children Age')}}</label>
            <b-form-group>
                <template slot="description">
                    <b-form-text style="font-size:10px;">{{$t('The ages are separated by comma (s)')}}</b-form-text>
                </template>
                <div>
                    <b-form-input v-model="ageChildren" class="w-70 m-auto text-right" @blur="emitEventUpdateRoom"></b-form-input>
                </div>
            </b-form-group>
        </div>
    </div>
    <!-- Third Row -->
    <div class="form-row">
        <div class="col-md-12 text-primary">
            <h5>{{$t('Rates per day summary')}}</h5>
            <b-alert 
               v-model="showOverlapError"
                dismissible
                variant="warning"
                @dismissed="showOverlapError=false">
                    <label style="font-size:initial;">{{$t('Rates dates can not overlap')}}</label>
            </b-alert>
        </div>
    </div>
    <!-- Fourth Row -->
    <div class="mt-3">
        <b-button v-b-toggle.new_rates_dates variant="link">{{$t('Add New Rate')}}</b-button>
    </div>
    <!-- Fifth Row -->
     <b-collapse id="new_rates_dates">
        <!-- First Row Collapse -->
        <div class="form-row">
            <div class="form-group" :class="col_size_6_3">
                <label style="font-size:smaller;">{{$t('Price')}}</label>
                <template v-if="isNetRate">
                     <b-form-group>
                        <template slot="description">
                            <b-form-text style="font-size:10px;">{{$t('Until two decimal places')}}</b-form-text>
                        </template>
                        <div>
                            <b-form-input id="totalnr" type="number" step="0.01" class="text-right" v-model="price" @blur="calculatePriceNR(price)"></b-form-input>
                        </div>
                    </b-form-group>                    
                </template>
                 <template v-else>
                    <b-form-group>
                        <template slot="description">
                            <b-form-text style="font-size:10px;">{{$t('Until two decimal places')}}</b-form-text>
                        </template>
                        <div>
                            <b-form-input id="totalnr" type="number" step="0.01" class="w-60 m-auto text-right" v-model="price"></b-form-input>
                        </div>
                    </b-form-group>
                </template>                 
            </div>
            <div class="form-group" :class="col_size_6_3">
                <label style="font-size:smaller;">{{$t('Extra Price')}}</label>
                <template v-if="isNetRate">
                     <b-form-group>
                        <template slot="description">
                            <b-form-text style="font-size:10px;">{{$t('Until two decimal places')}}</b-form-text>
                        </template>
                        <div>
                            <b-form-input id="totalnr" type="number" step="0.01" class="text-right" v-model="extraPrice" @blur="calculateExtraPriceNR(extraPrice)"></b-form-input>
                        </div>
                    </b-form-group>
                </template>
                <template v-else>
                    <b-form-group>
                        <template slot="description">
                            <b-form-text style="font-size:10px;">{{$t('Until two decimal places')}}</b-form-text>
                        </template>
                        <div>
                            <b-form-input id="totalnr" step="0.01" type="number" class="w-60 m-auto text-right" v-model="extraPrice"></b-form-input>
                        </div>
                    </b-form-group>
                </template>
            </div>
            <div v-if="isNetRate" class="form-group col-md-3">
                <label style="font-size:smaller;">{{$t('Price NR')}}</label>
                <b-form-group>
                    <template slot="description">
                        <b-form-text style="font-size:10px;">{{$t('Until two decimal places')}}</b-form-text>
                    </template>
                    <div>
                        <b-form-input id="totalnr" step="0.01" type="number" class="text-right" v-model="priceNR"></b-form-input>
                    </div>
                </b-form-group>
            </div>
            <div v-if="isNetRate" class="form-group col-md-3">
                <label style="font-size:smaller;">{{$t('Extra Price NR')}}</label>
                 <b-form-group>
                    <template slot="description">
                        <b-form-text style="font-size:10px;">{{$t('Until two decimal places')}}</b-form-text>
                    </template>
                    <div>
                        <b-form-input id="totalnr" step="0.01" type="number" class="text-right" v-model="extraPriceNR"></b-form-input>
                    </div>
                </b-form-group>
            </div>
        </div>
        <!-- Second Row Collapse -->
        <div class="form-row">               
            <b-input-group class="mb-3 p-1">
                <v-date-picker
                v-model="dates"
                class="form-control p-0"
                :min-date="minDate"
                :max-date="maxDate"
                mode="range"
                :popover="{ placement: 'bottom', visibility: 'click'}"
                :columns="2">
                </v-date-picker>

                <template v-slot:prepend>
                    <b-input-group-text>
                        <i class="fa fa-calendar"></i>
                    </b-input-group-text>
                </template>

                <b-input-group-append>                        
                    <b-button variant="primary" @click="add()">
                        {{$t('Add Rate')}}
                    </b-button>
                </b-input-group-append>
            </b-input-group>                
        </div>
    </b-collapse>
    <!-- Sixth Row -->
    <div class="form-row">             
        <b-list-group class="col-md-12">
            <price-list :priceDetails="priceDetails" @removePriceDetail="remove" :key="reload"/>
        </b-list-group>
    </div>  
  </div>
</template>

<script>
import { cloneDeep } from '../../../../../../node_modules/lodash';
import PriceList from './Price/List.vue';
import Rate from '../../../models/room/rate/rate';
import RoomVM from '../../../viewModel/room/room';
import { numberTwoDecimal } from '../../../utilities/math/math';
import { overlapDates } from '../../../utilities/date/dates';

export default {
    props:{
        checkIn:{

        },
        checkOut: {

        },
        room: {
            type:Object
        },
        index: {
            type:Number
        },
        isNetRate: {

        },
        totalNights: {

        },
        totalRooms: {

        },
        ecotasa: {

        }
    },
    components: {
        PriceList
    },
    data() {
        return {
            roomIndex: 0,
            roomPriceId:0,
            adults:null,
            extraAdults:null,
            extraChildrens:null,
            childrens:null,
            ageChildren:null,
            priceDetails: null,
            total:0,
            totalNR:0,
            netRateContract:0,
            ecotasaPerRoomRate:0,
            currency: null,
            //New Rate Data
            price:0,
            extraPrice:0,
            priceNR:0,
            extraPriceNR:0,
            dates:{ start: null, end: null},
            minDate: null,
            maxDate: null,
            datesRatesRanges:null,
            //Alert
            showOverlapError:false,
            //Reload
            reload:0
        }
    },
    created() {

        this.roomIndex = this.index;

        [this.minDate,this.maxDate] = [new Date(this.checkIn), new Date(this.checkOut)]
        this.maxDate.setDate(this.maxDate.getDate() - 1);

        [this.dates.start, this.dates.end] = [this.minDate, this.maxDate];

        //Destructuring Object
        ({roomPriceId:this.roomPriceId, adults: this.adults, extraAdults: this.extraAdults, childrens:this.childrens, 
            extraChildrens:this.extraChildrens, ageChildren:this.ageChildren,
            total:this.total, currency:this.currency, netRateContract: this.netRateContract } = this.room)

        console.log(`Contrato NR: ${this.netRateContract}`);

        this.priceDetails = cloneDeep(this.room.priceDetails);
        
         this.datesRatesRanges = this.priceDetails.map(priceDetail => {
            return { start: priceDetail.checkIn, end: priceDetail.checkOut };
        });

        this.ecotasaPerRoomRate = (this.ecotasa / this.totalNights) / this.totalRooms;

        console.log(`Total de noches: ${this.totalNights} Total de habitaciones: ${this.totalRooms} Ecotasa ${this.ecotasa}`);

    },
    watch: {
        price: function(current, previous) {
            console.log(`Price -> current: ${current} -> previous: ${previous}`);
        },
        extraPrice: function (current, previous) {
            console.log(`Extra Price -> current: ${current} -> previous: ${previous}`);
        },
        priceDetails: function() {
            this.reload++;
            console.log('Reload');
            console.log(this.reload);
            this.calculateTotal();
            if(this.isNetRate) this.calculateTotalNR();
        }
    },
    computed: {
        col_size_4_3() {
            return {
                "col-md-4": !this.isNetRate,
                "col-md-3": this.isNetRate,
            };
        },
        col_size_6_3() {
            return {
                "col-md-6": !this.isNetRate,
                "col-md-3": this.isNetRate,
            };
        }
    },
    methods: {
        add(){
            this.datesRatesRanges.push(this.dates);
            
            const resultOverlap = overlapDates(this.datesRatesRanges);

            if(resultOverlap.isOverlap) { 
                this.datesRatesRanges.pop();
                this.showOverlapError = true;
                
                return ;
            }
           
            this.addRate();
            this.emitEventUpdatePriceRoom();
        
        },
        remove(index) {
            this.priceDetails.splice(index, 1);
            this.datesRatesRanges.splice(index,1);
            this.emitEventUpdatePriceRoom();
        },
        addRate() {
            const rate = new Rate(this.roomPriceId,this.dates.start, this.dates.end,
                numberTwoDecimal(parseFloat(this.price)), numberTwoDecimal(parseFloat(this.extraPrice)), 
                numberTwoDecimal(parseFloat(this.priceNR)), numberTwoDecimal(parseFloat(this.extraPriceNR)), this.currency);

            this.priceDetails.push(rate);
        },
        calculateTotal() {
            this.total = 0;

            this.priceDetails.forEach(priceDetail => {
                const diffTime = priceDetail.checkOut.getTime() - priceDetail.checkIn.getTime();
                const nights = (diffTime /  (1000 * 3600 * 24)) + 1;
                const priceRoom = numberTwoDecimal(priceDetail.price) + numberTwoDecimal(priceDetail.extraPrice);
                this.total += numberTwoDecimal((priceRoom * nights));
            });
            
            this.total = numberTwoDecimal(this.total);
        },
        calculateTotalNR() {
            this.totalNR = 0;

             this.priceDetails.forEach(priceDetail => {
                const diffTime = priceDetail.checkOut.getTime() - priceDetail.checkIn.getTime();
                const nights = (diffTime /  (1000 * 3600 * 24)) + 1;
                const priceRoom = numberTwoDecimal(priceDetail.priceNR) + numberTwoDecimal(priceDetail.extraPriceNR);
                this.totalNR += numberTwoDecimal((priceRoom * nights));
            });

            this.totalNR = numberTwoDecimal(this.totalNR);
        },
        calculatePriceNR(price) {
            
            //quitarlse ecotasa, quitarle margen y poner la ecotasa 
            this.priceNR = ((parseFloat(price - this.ecotasaPerRoomRate) * ((100 - this.netRateContract) / 100)) + this.ecotasaPerRoomRate).toFixed(2);
        },
        calculateExtraPriceNR(extraPrice) {
            this.extraPriceNR = ((parseFloat(extraPrice - this.ecotasaPerRoomRate) * ((100 - this.netRateContract) / 100)) + this.ecotasaPerRoomRate).toFixed(2);
        },
        emitEventUpdateRoom() {
            this.$emit('updateRoom', this.roomIndex, 
                new RoomVM(parseInt(this.adults), parseInt(this.extraAdults), parseInt(this.childrens), 
                parseInt(this.extraChildrens),this.ageChildren));
        },
        emitEventUpdatePriceRoom() {
            this.$emit('updatePriceRoom', this.roomIndex, this.priceDetails);
        }
    }
}
</script>
