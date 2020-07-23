<template>
    <div>
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Promotions') }}</h2>
            <b-row class="mb-4 mt-4">
                <b-col lg="6">
                    <b-card style="border:none">
                        <b-row>
                            <b-col md="5">
                                <b-form-group :label="$t('Promotion code')">
                                    <b-form-input v-model="req.promotionCode" :placeholder="$t('Code')"></b-form-input>
                                </b-form-group>
                                <b-form-group class="pt-2" :label="$t('Promotion name')">
                                    <b-tabs active-nav-item-class="font-weight-bold text-info">
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-input v-model="req.promotionName_es"></b-form-input>
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-input v-model="req.promotionName_en"></b-form-input>
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                            <b-col md="7">
                                <b-form-group :label="$t('Promotion description')">
                                    <b-tabs active-nav-item-class="font-weight-bold text-info">
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-textarea v-model="req.promotionDesc_es" rows="5" max-rows="5"></b-form-textarea>
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-textarea v-model="req.promotionDesc_en" rows="5" max-rows="5"></b-form-textarea>
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                        </b-row>
                        <b-row>
                            <b-col>
                                <b-form-checkbox v-model="req.combinablePromotion" switch>{{ $t('Combinable promotion') }}</b-form-checkbox>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-col>
                <b-col class="mt-3">
                    <type-promotion
                    :typeFreeNight="req.promotionType.typeFreeNight"
                    :freeNight="req.promotionType.freeNight"
                    :typeDiscount="req.promotionType.typeDiscount"
                    :discount="req.promotionType.discount"></type-promotion>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col lg="6">
                    <booking-window></booking-window>
                </b-col>
                <b-col class="sm-margin">
                    <rate-plan-rooms></rate-plan-rooms>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col lg="6">
                    <travel-window></travel-window>
                </b-col>
                <b-col class="sm-margin" :class="offset ? '' : 'offset-row-250'">
                    <restriction></restriction>
                </b-col>
            </b-row>
            <b-row class="mb-4">
                <b-col class="text-right mr-4">
                    <b-button variant="success">{{ $t('Save') }}</b-button>
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
                promotionCode: '',
                promotionName_es: '',
                promotionName_en: '',
                promotionDesc_es: '',
                promotionDesc_en: '',
                combinablePromotion: true,
                //tipo de promocion
                promotionType: {
                    typeFreeNight: '0',
                    freeNight: 0,
                    typeDiscount: '0',
                    discount: 0
                }
            }
        }
    },
    methods: {
        changeOffset() {
            this.offset = true;
        }
    }
}
</script>