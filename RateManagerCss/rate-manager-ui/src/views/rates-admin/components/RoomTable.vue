<template>
     <div class="room-description ml-5 mr-5 mt-3">
        <div class="d-flex border dark-gray-created">
            <div class="d-flex w-30 align-items-center">
                <div class="border-right p-1 flex-fill d-flex w-80 justify-content-between">
                    <h4 class="font-weight-bold m-0"><i class="fa fa-bed mr-1"></i> {{room.name}}</h4>
                    <h4 class="text-primary m-0"><i class="fa fa-bolt"></i></h4>
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
            <div :key="rate.ratePlanId" class="d-flex bg-white border-5" :class="{'border-top': !idx, 'border-primary':!idx}">
                <div class="d-flex w-30">
                    <div class="border flex-fill d-flex w-80 align-items-center p-1 pl-4">
                        <h5 class="m-0 pl-2">{{rate.ratePlan}}</h5>
                        <div class="btn-group ml-3" v-if="rate.children.length > 0">
                            <button data-toggle="collapse" class="btn btn-primary badge badge-primary" :data-target="'#' + rate.ratePlanId + '-'+  rate.roomId + '-lk'">
                                <span>{{rate.children.length}} <i class="fa fa-link"></i></span>
                            </button>
                        </div>
                    </div>
                    <div class="border p-1 flex-fill text-center w-20">
                        <span class="text-secondary">
                            <small><i class="fa fa-user"></i>x{{rate.dailyRates[0].occupancy}}</small>
                        </span>
                        <span class="text-uppercase text-warning ml-2">
                            <small>{{rate.currency}}</small>
                        </span>
                    </div>
                </div>
                <div class="d-flex w-70">
                    <div class="border p-1 flex-fill text-center" v-for="day in rate.dailyRates" :key="day.date">
                        <v-popover placement="right">
                            <a href="javascript:;" class="tooltip-target text-decoration-none" v-if="day.price> 0">{{ getPrice(day, rate) | currency}}</a>
                            <a href="javascript:;" v-else class="text-danger tooltip-target text-decoration-none"> N/A </a>
                            <template slot="popover">
                                <day-rate-detail :day-rate="day"/>
                            </template>
                        </v-popover>

                    </div>
                </div>
            </div>
            <div :id="rate.ratePlanId + '-'+  rate.roomId + '-lk'" class="collapse show" v-if="rate.children.length > 0" :key="'c' + rate.ratePlanId">
                <div class="d-flex bg-white" v-for="child in rate.children" :key="child.ratePlanId">
                    <div class="d-flex w-30">
                        <div class="border flex-fill d-flex w-80 justify-content-between align-items-center bg-blue-created p-1 pl-4">
                            <h5 class="m-0 pl-3 text-primary">{{child.ratePlan}}</h5>
                            <span class="text-primary m-0">
                                <i v-tooltip="$t('{discount}% Off', {discount: child.dailyRates[0].discount})" v-if="child.isPromotion" class="fa fa-tag"></i>
                                <i v-else class="fa fa-link"></i>
                            </span>
                        </div>
                        <div class="border flex-fill w-20 text-center bg-blue-created">
                            <span class="text-secondary">
                                <small><i class="fa fa-user"></i>x{{child.dailyRates[0].occupancy}}</small>
                            </span>
                            <span class="text-uppercase text-warning ml-2">
                                <small>{{child.currency}}</small>
                            </span>
                        </div>
                    </div>
                    <div class="d-flex w-70">
                        <div class="border p-1 flex-fill text-center bg-blue-created" v-for="day in child.dailyRates" :key="day.date">
                            <span v-if="day.price > 0">{{ getPrice(day, child) | currency}}</span>
                            <span v-else class="text-danger"> N/A </span>
                        </div>
                    </div>
                </div>
            </div>
        </template>
    </div>
</template>

<script>
import DayRateDetail from './DayRateDetail';

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
        getPrice(dayRate, rate) {
            let price = dayRate.price * (1 - (dayRate.discount / 100));
            if (rate.factor !== undefined) price *= rate.factor;
            else if (rate.offset !== undefined) price += offset;
            return price;
        },
    },
};
</script>
