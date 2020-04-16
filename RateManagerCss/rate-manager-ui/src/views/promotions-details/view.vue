<template>
    <div>
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Promotions') }}</h2>
            <b-row class="mb-4 mt-4">
                <b-col>
                    <b-card style="border:none">
                        <b-row>
                            <b-col md="5">
                                <b-form-group :label="$t('Promotion code')">
                                    <b-form-input></b-form-input>
                                </b-form-group>
                                <b-form-group class="pt-2" :label="$t('Promotion name')">
                                    <b-tabs>
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-input></b-form-input>
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-input></b-form-input>
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                            <b-col md="7">
                                <b-form-group :label="$t('Promotion description')">
                                    <b-tabs>
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-textarea rows="5" max-rows="5"></b-form-textarea>
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-textarea rows="5" max-rows="5"></b-form-textarea>
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-col>
                <b-col>
                    <rate-plan-rooms></rate-plan-rooms>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col :class="offset ? 'offset-row-100' : 'offset-row-30'">
                    <travel-window></travel-window>
                </b-col>
                <b-col :class="offset ? '' : 'offset-row-100'">
                    <booking-window></booking-window>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col :class="offset ? 'offset-row-170' : ''">
                    <restrictions></restrictions>
                </b-col>
                <b-col>
                    <type-promotion></type-promotion>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col class="text-right mr-4">
                    <b-button variant="success">{{ $t('Save') }}</b-button>
                </b-col>
            </b-row>
        </b-container>
    </div>
</template>

<script>
import RatePlanRooms from './components/RatePlan-Rooms.vue';
import TravelWindow from './components/TravelWindow.vue';
import BookingWindow from './components/BookingWindow.vue';
import TypePromotion from './components/TypePromotion.vue';
import Restrictions from './components/Restrictions.vue';
import EventBus from '../../core/event-bus';

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
        Restrictions
    },
    data() {
        return {
            offset: false
        }
    },
    methods: {
        changeOffset() {
            this.offset = true;
        }
    }
}
</script>