<template>
    <b-container v-if="!load" style="height:400px"></b-container>
    <b-container v-else-if="load" class="main-container" fluid>
        <h2 class="text-primary">{{ $t('Promotions') }}</h2>
        <b-row class="mb-4 mt-2">
            <b-col lg="6">
                <b-card class="border-0">
                    <b-row>
                        <b-col md="6">
                            <b-form-group :label="$t('Promotion code')">
                                <!-- <b-form-input :max="4" v-model="promo.id" :placeholder="$t('Code')" trim/> -->
                                <input :disabled="isEdit" type="text" class="form-control" id="promo" name="promoName" maxlength="4" v-model="promo.id">
                            </b-form-group>
                            <b-form-group class="pt-2" :label="$t('Promotion name')">
                                <b-tabs active-nav-item-class="font-weight-bold text-info">
                                    <b-tab :title="$t('Spanish')">
                                        <b-form-input v-model="promo.description.esp" trim/>
                                    </b-tab>
                                    <b-tab :title="$t('English')">
                                        <b-form-input v-model="promo.description.eng" trim/>
                                    </b-tab>
                                </b-tabs>
                            </b-form-group>
                        </b-col>
                        <b-col>
                            <b-form-group :label="$t('Promotion description')">
                                <b-tabs active-nav-item-class="font-weight-bold text-info">
                                    <b-tab :title="$t('Spanish')">
                                        <b-form-textarea v-model="promo.name.esp" rows="5" max-rows="5" trim/>
                                    </b-tab>
                                    <b-tab :title="$t('English')">
                                        <b-form-textarea v-model="promo.name.eng" rows="5" max-rows="5" trim/>
                                    </b-tab>
                                </b-tabs>
                            </b-form-group>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col>
                            <b-form-checkbox v-if="false"
                            v-model="promo.isCombinablePromotion"
                            switch>
                                {{ $t('Combinable promotion') }}
                            </b-form-checkbox>
                        </b-col>
                    </b-row>
                </b-card>
            </b-col>
            <b-col class="mt-2 align-items-lg-center d-lg-flex">
                <type-promotion :dataModel="promo.discount"></type-promotion>
            </b-col>
        </b-row>
        <b-row class="mb-4">
            <b-col lg="6">
                <travel-window :dataModel="promo"></travel-window>
                <!-- <booking-window :dataModel="promo.rule.bookingWindow"></booking-window> -->
            </b-col>
            <b-col class="sm-margin">
                <rate-plan-rooms :dataModel="promo.applicableFor"></rate-plan-rooms>
            </b-col>
        </b-row>
        <b-row class="mb-4">
            <b-col lg="6">
                <!-- <restriction :dataModel="promo.rule.cancelPenalty"></restriction> -->
                <booking-window :dataModel="promo.rule.bookingWindow"></booking-window>
            </b-col>
            <b-col class="sm-margin" :class="offset ? '' : 'offset-row-250'">
                <!-- <travel-window :dataModel="promo"></travel-window> -->
                 <restriction :dataModel="promo.rule.cancelPenalty"></restriction>
            </b-col>
        </b-row>
        <b-row class="mb-4">
            <b-col class="text-left">
                <b-link :href="$appConfig.basePath + '/rate-manager-ui/dist/promotions.aspx'">{{ $t('Return to Promotions List') }}</b-link>
            </b-col>
            <b-col class="text-right mr-4">
                <b-button @click="save" variant="primary">{{ $t('Save') }}</b-button>
            </b-col>
        </b-row>
    </b-container>
</template>

<script>
import RatePlanRooms from './components/RatePlanRooms.vue';
import TravelWindow from './components/TravelWindow.vue';
import BookingWindow from './components/BookingWindow.vue';
import TypePromotion from './components/TypePromotion.vue';
import Restriction from './components/Restriction.vue';
import EventBus from '../../core/event-bus';

import model from './helper/promotionModel';
import offersService from '../../api/offers-service';

let loader = null;

export default {
    name: 'app',
    created() {
        EventBus.$on('changeOffset', () => this.offset = true);
        if (this.$appConfig.session.code) {
            this.get();
        }
        else
        {
            this.load = true;
        }
       console.log(this.$appConfig)
    },
    components: {
        RatePlanRooms,
        TravelWindow,
        BookingWindow,
        TypePromotion,
        Restriction
    },
    data() {
        return {
            hotelId: this.$appConfig.session.hotelId,
            isClone: this.$appConfig.promotion.clone,
            isEdit: this.$appConfig.promotion.edit,
            offset: false,
            load: false,
            //view Promo Model
            promo: {
                //Promotion Code
                id: this.$appConfig.session.code,
                //Promotion Active
                active: false,
                //Promotion Combinable
                isCombinablePromotion:false,
                //Promotion Travel Window Dates
                startDate: null,
                endDate: null,
                //Promotion Type
                discount: {
                    amount: null,
                    applicationMode: null,
                    discountPattern: null,
                    nightsDiscounted: null,
                    percent: null
                },
                 //Promotion Name
                name: {
                    eng: '',
                    esp: '',
                    id: null
                },
                 //Promotion Description
                description: {
                    eng: '',
                    esp: '',
                    id: null
                },
                //Promotion Rooms & Rate Plans
                applicableFor: {
                    ratesPlan: [],
                    rooms: []
                },
                //Promotion Rules
                rule: {
                    id: null,
                    //API
                    noArrivals: {},
                    //View
                    _noArrivals:[],
                    //API
                    applyDays: {},
                    //View
                    _applyDays:[0,1,2,3,4,5,6],
                     //Booking Window
                    bookingWindow: {
                        id: null,
                        startDate:null,
                        endDate:null,
                        minDays:null,
                        maxDays:null,
                        startHour:null,
                        endHour:null
                    },
                    //Closure
                    excludedDates: [],
                    _excludeDates:null,
                    closures:[],
                    //Cancel Penalty
                    cancelPenalty: {
                        //Detailed Description
                        detailedDescription: {
                            eng: '',
                            esp: '',
                            id: null
                        },
                        name: '',
                        offsetDropTime: null,
                        offsetTimeUnit: 1,
                        offsetTimeUnitMiltiplier: 1,
                        //Previous Description
                        shortDescription: {
                            eng: '',
                            esp: '',
                            id: null
                        },
                        //Nights
                        minNights:null,
                        maxNights:null,
                        //Cancellation Type
                        cancellationType: -1,
                        byDay:null,
                        byHour:null
                    },
                    // minNights: 0,
                    // maxNights: 0,
                }
            },
            error: false,
            post: null,
        }
    },
    methods: {
        get() {
            loader = this.$loading.show({
                color: this.$appConfig.themeColors.info,
                height: 128,
                width: 128
            });

            offersService.getByCode(this.hotelId, this.promo.id)
            .then(response => {
                //if (response.body.length > 0) {
                    //console.log(response.body)
                    this.promo = Object.assign({}, response.body);
                    console.log('Promo Object');
                    console.log(this.promo);
                
                    this.$set(this.promo.rule, '_applyDays', []);
                    this.$set(this.promo.rule, '_noArrivals', []);
                    this.$set(this.promo.rule, 'closures', []);
                    
                    if(this.isClone)
                    {
                        console.log('Clonar Object');

                        this.promo.id = null;
                        this.promo.active = false;
                        this.promo.name.id = null;
                        this.promo.description.id = null;
                        this.promo.rule.id = null;
                        this.promo.rule.bookingWindow.id = null;
                        this.promo.rule.cancelPenalty.name = '';
                        this.promo.rule.cancelPenalty.detailedDescription.id = null;
                        this.promo.rule.cancelPenalty.shortDescription.id = null;

                        console.log(this.promo);
                    }

                    this.load = true;
                    loader.hide();
                //} else {
                    //do something
                //}
            })
        },
        //TODO: Save Updated Promo
        save() {
            this.formValidation();
            if (!this.error) {
                //Update Promotion
                if(this.$appConfig.session.code && !this.isClone)
                {
                    this.$appAlert({
                        type: "question",
                        title: this.$t('Save Promotion ?'),
                        cancelButtonColor: "#d33",
                        showLoaderOnConfirm: true,
                        showCancelButton: true,
                        confirmButtonText: this.$t('Save'),
                        confirmButtonColor: "#3085d6",
                        cancelButtonText:this.$t('Cancel'),
                        //Request Api
                        preConfirm: () => {
                            return offersService.updatePromotion(this.hotelId,this.promo.id,this.post)
                            .then(response => {
                                //CallBack Response Api
                                return response;
                            })
                            .catch(error => {
                                //CallBack Response Api
                                return error;
                            })
                        }
                    })
                    //Result of CallBack
                    .then(response => {
                        console.log(response);
                        //Response Api Object
                        if(response.value.ok)
                        {
                            this.$appAlert({
                                type:"success",
                                title:this.$t('Promotion Saved'),
                                onClose: () => {
                                    const url = `${this.$appConfig.basePath}` + '/rate-manager-ui/dist/promotions.aspx';
                                    console.log(url)
                                    location.href = url;
                                }
                            })
                        }
                        else if(!response.value.ok){
                            this.$appAlert({
                                type:"error",
                                title:this.$t('Could Not Save Promotion')
                            })
                        }
                    })
                }
                //Save New Promotion
                else
                {
                    
                    this.$appAlert({
                        type: "question",
                        title: this.$t('Save Promotion ?'),
                        cancelButtonColor: "#d33",
                        showLoaderOnConfirm: true,
                        showCancelButton: true,
                        confirmButtonText: this.$t('Save'),
                        confirmButtonColor: "#3085d6",
                        cancelButtonText:this.$t('Cancel'),
                        //Request Api
                        preConfirm: () => {
                            return offersService.savePromotion(this.hotelId,this.post)
                            .then(response => {
                                //CallBack Response Api
                                return response;
                            })
                            .catch(error => {
                                //CallBack Response Api
                                return error;
                            })
                        }
                    })
                    //Result of CallBack
                    .then(response => {
                        console.log(response);
                        //Response Api Object
                        if(response.value.ok)
                        {
                            this.$appAlert({
                                type:"success",
                                title:this.$t('Promotion Saved'),
                                onClose: () => {
                                    const url = `${this.$appConfig.basePath}` + '/rate-manager-ui/dist/promotions.aspx';
                                    location.href = url;
                                }
                            })
                        }
                        else if(!response.value.ok){
                            this.$appAlert({
                                type:"error",
                                title:this.$t('Could Not Save Promotion')
                            })
                        }
                       
                    })
                }
                //End Else
            }
        },
        formValidation() {
            const form = new model(this.hotelId, this.promo);
            console.log(this.promo);
            form.validate();
            if (form.errors.length > 0) {
                this.error = true;

                let list = '';
                form.errors.forEach(x => {
                    list += `<div class="list-group-item border-0 p-1">- ${x}</div>`;
                });

                this.$appAlert({
                    icon: 'warning',
                    title: this.$t('Wrong form'),
                    html: `
                        <div class="list-group">${list}</div>
                    `
                });
            } else {
                this.error = false;
                this.post = form.__$;
                console.log("Post");
                console.log(this.post);
            }
        },
    }
}
</script>