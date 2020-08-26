<template>
    <div>
        <b-container class="main-container" fluid>
            <h2 class="text-primary">{{ $t('Promotions') }}</h2>
            <b-row class="mb-4 mt-2">
                <b-col lg="6">
                    <b-card style="border:none">
                        <b-row>
                            <b-col md="6">
                                <b-form-group :label="$t('Promotion code')">
                                    <b-form-input v-model="req.main.code" :placeholder="$t('Code')" />
                                </b-form-group>
                                <b-form-group class="pt-2" :label="$t('Promotion name')">
                                    <b-tabs active-nav-item-class="font-weight-bold text-info">
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-input v-model="req.main.nameEs" />
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-input v-model="req.main.nameEn" />
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                            <b-col>
                                <b-form-group :label="$t('Promotion description')">
                                    <b-tabs active-nav-item-class="font-weight-bold text-info">
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-textarea v-model="req.main.descEs" rows="5" max-rows="5" />
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-textarea v-model="req.main.descEn" rows="5" max-rows="5" />
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
                    <type-promotion :dataModel="req.typePromotion"></type-promotion>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col lg="6">
                    <booking-window :dataModel="req.bookingWindow"></booking-window>
                </b-col>
                <b-col class="sm-margin">
                    <rate-plan-rooms :dataModel="req.roomsAndRateplans"></rate-plan-rooms>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col lg="6">
                    <restriction :dataModel="req.restriction"></restriction>
                </b-col>
                <b-col class="sm-margin" :class="offset ? '' : 'offset-row-250'">
                    <travel-window :dataModel="req.travelWindow"></travel-window>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col class="text-right mr-4">
                    <b-button @click="save" variant="success">{{ $t('Save') }}</b-button>
                </b-col>
            </b-row>
        </b-container>
        {{ req }}
    </div>
</template>

<script>
import RatePlanRooms from './components/RatePlanRooms.vue';
import TravelWindow from './components/TravelWindow.vue';
import BookingWindow from './components/BookingWindow.vue';
import TypePromotion from './components/TypePromotion.vue';
import Restriction from './components/Restriction.vue';
import EventBus from '../../core/event-bus';
import OffersService from '../../api/offers-service';

export default {
    name: 'app',
    created() {
        EventBus.$on('changeOffset', this.changeOffset);
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
            offset: false,
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
                    cancellationType: 0,
                    prevCancel_es: null,
                    prevCancel_en: null,
                    detsCancel_es: null,
                    detsCancel_en: null
                }
            }
        }
    },
    methods: {
        changeOffset() {
            this.offset = true;
        },
        save() {
            
        }
    }
}
</script>