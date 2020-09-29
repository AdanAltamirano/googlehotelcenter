<template>
    <b-container v-if="load" class="main-container" fluid>
        <h2 class="text-primary">{{ $t('Promotions') }}</h2>
        <b-row class="mb-4 mt-2">
            <b-col lg="6">
                <b-card class="border-0">
                    <b-row>
                        <b-col md="6">
                            <b-form-group :label="$t('Promotion code')">
                                <b-form-input v-model="promo.id" :placeholder="$t('Code')" />
                            </b-form-group>
                            <b-form-group class="pt-2" :label="$t('Promotion name')">
                                <b-tabs active-nav-item-class="font-weight-bold text-info">
                                    <b-tab :title="$t('Spanish')">
                                        <b-form-input v-model="promo.name.esp" />
                                    </b-tab>
                                    <b-tab :title="$t('English')">
                                        <b-form-input v-model="promo.name.eng" />
                                    </b-tab>
                                </b-tabs>
                            </b-form-group>
                        </b-col>
                        <b-col>
                            <b-form-group :label="$t('Promotion description')">
                                <b-tabs active-nav-item-class="font-weight-bold text-info">
                                    <b-tab :title="$t('Spanish')">
                                        <b-form-textarea v-model="promo.description.esp" rows="5" max-rows="5" />
                                    </b-tab>
                                    <b-tab :title="$t('English')">
                                        <b-form-textarea v-model="promo.description.eng" rows="5" max-rows="5" />
                                    </b-tab>
                                </b-tabs>
                            </b-form-group>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col>
                            <b-form-checkbox
                            v-model="req.main.combinablePromotion"
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
                <booking-window :dataModel="req.bookingWindow"></booking-window>
            </b-col>
            <b-col class="sm-margin">
                <rate-plan-rooms :dataModel="promo.applicableFor"></rate-plan-rooms>
            </b-col>
        </b-row>
        <b-row class="mb-4">
            <b-col lg="6">
                <restriction :dataModel="promo.rule.cancelPenalty"></restriction>
            </b-col>
            <b-col class="sm-margin" :class="offset ? '' : 'offset-row-250'">
                <travel-window :dataModel="promo"></travel-window>
            </b-col>
        </b-row>
        <b-row class="mb-4">
            <b-col class="text-right mr-4">
                <b-button @click="save" variant="success">{{ $t('Save') }}</b-button>
            </b-col>
        </b-row>
        {{ promo }}
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
        if (this.$appConfig.session.code != null) {
            this.get();
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
            offset: false,
            load: false,
            promo: {
                active: false,
                applicableFor: {
                    ratesPlan: [],
                    rooms: []
                },
                description: {
                    eng: '',
                    esp: '',
                    id: null
                },
                discount: {
                    amount: 0,
                    applicationMode: 0,
                    discountPattern: 0,
                    nightsDiscounted: 1,
                    percent: 25
                },
                endDate: null,
                id: this.$appConfig.session.code,
                name: {
                    eng: '',
                    esp: '',
                    id: null
                },
                rule: {
                    applyDays: {},
                    bookingWindow: {
                        id: null
                    },
                    cancelPenalty: {
                        detailedDescription: {
                            eng: '',
                            esp: '',
                            id: null
                        },
                        name: '',
                        offsetDropTime: null,
                        offsetTimeUnit: null,
                        offsetTimeUnitMultipler: null,
                        shortDescription: {
                            eng: '',
                            esp: '',
                            id: null
                        }
                    },
                    excludedDates: [],
                    id: null,
                    noArrivals: {}
                },
                startDate: null
            },
            req: {
                main: {
                    code: null,
                    nameEs: null,
                    nameEn: null,
                    descEs: null,
                    descEn: null,
                    combinablePromotion: true
                },
                typePromotion: {
                    freeNight: 0,
                    typeFreeNight: 0,
                    discount: 0,
                    typeDiscount: 0
                },
                roomsAndRateplans: {
                    rooms: [],
                    rateplans: []
                },
                bookingWindow: {
                    startDate: null,
                    endDate: null,
                    timeFrom: null,
                    timeTo: null,
                    minDays: 1,
                    maxDays: 1
                },
                travelWindow: {
                    initialDate: null,
                    finalDate: null,
                    validDays: [],
                    noArrivalDays: [],
                    closures: []
                },
                restriction: {
                    minNights: 0,
                    maxNights: 0,
                    cancellationType: -1,
                    byDay: 1,
                    byHour: 1,
                    bySpecificTime: {
                        hour: 1,
                        minuts: 0
                    },
                    prevCancel_es: null,
                    prevCancel_en: null,
                    detsCancel_es: null,
                    detsCancel_en: null
                }
            },
            error: false,
            post: null
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
                if (response.body.length > 0) {
                    console.log(response.body)
                    this.promo = Object.assign({}, response.body[0]);
                    this.$set(this.promo.rule, '_applyDays', []);
                    this.$set(this.promo.rule, '_noArrivals', []);
                    this.$set(this.promo.rule, 'closures', []);
                    this.load = true;
                    loader.hide();
                } else {
                    //do something
                }
            })
        },
        //TODO: Save Updated Promo
        save() {
            this.formValidation();
            if (!this.error) {

            }
        },
        formValidation() {
            const form = new model(this.hotelId, this.req);

            form.validate();
            if (form.errors.length > 0) {
                this.error = true;

                let list = '';
                form.errors.forEach(x => {
                    list += `<div class="list-group-item border-0 p-1">- ${x}</div>`;
                });

                this.$swal({
                    icon: 'warning',
                    title: this.$t('Wrong form'),
                    html: `
                        <div class="list-group">${list}</div>
                    `
                });
            } else {
                this.error = false;
                this.post = form.__$;
            }
        }
    }
}
</script>