<template>
<!-- eslint-disable -->
    <div class="room-description ml-3 mr-3 mt-3">
        <div class="d-flex border dark-gray-created">
            <div class="d-flex w-30 align-items-center">
                <div class="border-right p-1 flex-fill d-flex w-80 justify-content-between">
                    <h5 class="font-weight-bold m-0" :class="{ 'text-secondary': room.isLinked}"><i class="fa fa-bed mr-1"></i> {{room.code}} - {{room.name}}</h5>
                    <h5 class="text-primary m-0">
                        <i v-if="room.isLinked" v-tooltip="{ content: getRoomLinkDesc(room), classes: ['primary']}" class="fa fa-link"></i>
                    </h5>
                </div>
                <div class="border p-1 flex-fill w-20">
                    <span class="text-uppercase">{{ 'avail' | translate}}</span>
                </div>
            </div>
            <div class="d-flex w-70">
                <div v-for="day in room.inventory" :key="day.date" class="border p-1 flex-fill text-center">
                    <span :class="inventoryStyles(day)">{{day.available}}</span>
                </div>
            </div>
        </div>
        <template v-for="(rate, idx) in room.rates">
            <div :key="rate.ratePlanId" class="d-flex bg-white border-5" :class="{ 'border-top': !idx, 'border-primary':!idx && !room.isLinked, 'border-secondary':!idx && room.isLinked}">
                <div class="d-flex w-30">
                    <div class="border flex-fill d-flex w-80 align-items-center justify-content-between p-1 pl-2">
                        <h5 class="m-0 pl-2 w-80 text-truncate" :class="{ 'text-muted': rate.parentRatePlanId}" >
                            <i v-tooltip="{ content: getPlanLinkDesc(rate), classes: ['primary']}" v-if="rate.parentRatePlanId" class="fa fa-link text-primary mr-2"></i>
                            <span v-tooltip="rate.ratePlan">{{rate.ratePlanId}} - {{rate.ratePlan}}</span>
                        </h5>
                        <div class="btn-group ml-3" v-if="rate.children.length > 0">
                            <button data-toggle="collapse" class="btn btn-link btn-sm p-0" :data-target="'#' + rate.ratePlanId + '-'+  rate.roomId + '-lk'">
                                <span><i class="fa fa-angle-down"></i></span>
                            </button>
                        </div>
                    </div>
                    <div class="border p-1 flex-fill text-center w-20">
                        <span class="text-secondary">
                            <small><i class="fa fa-user"></i>x{{rate.dailyRates[0].occupancy}}</small>
                        </span>
                        <span class="text-uppercase text-success ml-2">
                            <small>{{rate.currency}}</small>
                        </span>
                    </div>
                </div>
                <div class="d-flex w-70">
                    <div v-for="day in rate.dailyRates"
                        class="border p-1 flex-fill text-center"
                        :class="{'bg-noarrival': day.noArrival, 'text-muted': rate.parentRatePlanId }"
                        :key="day.date"
                        v-tooltip="day.noArrival ? $t('no arrivals') : ''">
                        <day-rate-detail :room="room" :day-rate="day" :rate="rate"/>
                    </div>
                </div>
            </div>
            <div :id="rate.ratePlanId + '-'+  rate.roomId + '-lk'" class="collapse show" v-if="rate.children.length > 0" :key="'c' + rate.ratePlanId">
                <div class="d-flex" v-for="child in rate.children" :key="child.ratePlanId">
                    <div class="d-flex w-30">
                        <div class="border flex-fill d-flex w-80 justify-content-between align-items-center bg-promo p-1 pl-4">
                            <h5 v-tooltip="child.ratePlan" class="m-0 pl-3 text-primary w-80 text-truncate">{{child.ratePlan}}</h5>
                            <span class="text-primary m-0">
                                <i v-tooltip="$t('{discount}% Off', {discount: child.discount})" v-if="child.isPromotion" class="fa fa-tag"></i>
                            </span>
                        </div>
                        <div class="border flex-fill w-20 text-center bg-promo">
                            <span class="text-secondary">
                                <small><i class="fa fa-user"></i>x{{child.dailyRates[0].occupancy}}</small>
                            </span>
                            <span class="text-uppercase text-success ml-2">
                                <small>{{child.currency}}</small>
                            </span>
                        </div>
                    </div>
                    <div class="d-flex w-70">
                        <div v-for="day in child.dailyRates"
                            class="border p-1 flex-fill text-center text-muted"
                            :class="{'bg-promo': !day.noArrival, 'bg-noarrival': day.noArrival }"
                            :key="day.date"
                            v-tooltip="day.noArrival ? $t('no arrivals')  : ''">
                            <span v-if="day.price > 0"
                                v-tooltip="{ content: dayPromotionText(day, child), classes: ['warning']}"
                                :class="{'text-warning': day.discount > 0}">{{ getPrice(day, child, room) | currency}}</span>
                            <span v-else class="text-danger"> N/A </span>
                        </div>
                    </div>
                </div>
            </div>
        </template>
    </div>
<!-- eslint-enable -->
</template>

<script>
import Utilities from '../helpers/utilities';
import DayRateDetail from './DayRateDetail.vue';

export default {
    name: 'room-table',
    components: {
        DayRateDetail,
    },
    props: {
        room: {
            type: Object,
            required: true,
        },
    },
    methods: {
        inventoryStyles(day) {
            return {
                'text-danger': day.available === 0,
                'text-warning': day.available > 0 && day.available < 3,
                'text-success': day.available > 2,
            };
        },
        getPrice(dayRate, rate, room) {
            return Utilities.getFinalPrice(dayRate, rate, room);
        },
        getPlanLinkDesc(rate) {
            let desc = '';
            if (rate.factor !== undefined) {
                desc = `${rate.parentRatePlanId} * ${this.$options.filters.currency(rate.factor)}`;
            } else if (rate.offset !== undefined) {
                /* eslint-disable max-len */
                desc = `${rate.parentRatePlanId} ${rate.offset > 0 ? '+' : '-'} ${this.$options.filters.currency(Math.abs(rate.offset))}`;
                /* eslint-enable max-len */
            }
            return desc;
        },
        getRoomLinkDesc(room) {
            let desc = '';
            if (room.factor !== undefined) {
                desc = `${room.parentRoomCode} * ${this.$options.filters.currency(room.factor)}`;
            } else if (room.offset !== undefined) {
                /* eslint-disable max-len */
                desc = `${room.parentRoomCode} ${room.offset > 0 ? '+' : '-'} ${this.$options.filters.currency(Math.abs(room.offset))}`;
                /* eslint-enable max-len */
            }
            return desc;
        },
        dayPromotionText(dayRate, rate) {
            let desc = '';
            let { discount } = dayRate;
            if (discount > 0) {
                if (rate.discountLevel === 1 && rate.discount > 0) {
                    desc = `${rate.discount}% + ${discount}% = `;
                    discount += rate.discount;
                }
                if (rate.discountLevel === 2 && rate.discount > 0) {
                    desc = `${rate.discount}% -> ${discount}% = `;
                    discount = 100 - ((100 - rate.discount) * (100 - discount) / 100);
                }
                desc += this.$t('{discount}% Off', { discount });
            }
            return desc;
        },
    },
};
</script>
